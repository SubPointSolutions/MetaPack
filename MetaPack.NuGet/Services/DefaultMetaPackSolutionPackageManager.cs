using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AppDomainToolkit;
using MetaPack.Core;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using MetaPack.NuGet.Common;
using MetaPack.NuGet.Utils;
using Microsoft.SharePoint.Client;
using NuGet;
using System.Xml.Linq;
using MetaPack.Core.Common;
using MetaPack.Core.Utils;

namespace MetaPack.NuGet.Services
{

    /// <summary>
    /// Internal one just to get some base methods of NuGetSolutionPackageService
    /// </summary>
    internal class DefaultNuGetSolutionPackageService : NuGetSolutionPackageService
    {

        public override Stream Pack(SolutionPackageBase package, SolutionPackageOptions options)
        {
            throw new NotImplementedException();
        }

        public override SolutionPackageBase Unpack(Stream package, SolutionPackageOptions options)
        {
            throw new NotImplementedException();
        }
    }

    public class DefaultMetaPackSolutionPackageManager : MetaPackSolutionPackageManagerBase
    {
        #region constructors
        public DefaultMetaPackSolutionPackageManager(IPackageRepository sourceRepository, ClientContext context)
            : base(sourceRepository, context)
        {
            InitProvisionEvents();
        }

        public DefaultMetaPackSolutionPackageManager(IPackageRepository sourceRepository, IPackagePathResolver pathResolver, IFileSystem fileSystem)
            : base(sourceRepository, pathResolver, fileSystem)
        {
            InitProvisionEvents();
        }

        public DefaultMetaPackSolutionPackageManager(IPackageRepository sourceRepository, IPackagePathResolver pathResolver, IFileSystem fileSystem, IPackageRepository localRepository)
            : base(sourceRepository, pathResolver, fileSystem, localRepository)
        {
            InitProvisionEvents();
        }

        #endregion

        #region methods
        private void InitProvisionEvents()
        {
            this.PackageInstalling += OnPackageInstalling;
        }

        protected virtual string SavePackageAsTempFile(IPackage package)
        {
            var tmpPackageFilePath = Path.GetTempFileName();

            using (var packageStream = package.GetStream())
            {
                using (var fileStream = new FileStream(tmpPackageFilePath, FileMode.Create, FileAccess.Write))
                {
                    packageStream.CopyTo(fileStream);
                }
            }

            return tmpPackageFilePath;
        }

        protected List<SolutionToolPackage> ResolveAdditionalTooling()
        {
            var result = new List<SolutionToolPackage>();

            return result;
        }


        /// <summary>
        /// Resolves solution tool package.
        /// 
        /// If .SolutionToolPackage is null, looking for NuGet package tags, and then into solution
        /// </summary>
        /// <returns></returns>
        protected virtual SolutionToolPackage ResolveSolutionToolPackage(IPackage package)
        {
            SolutionToolPackage result = null;

            if (SolutionToolPackage == null)
            {
                // resolve from the tags
                var tags = string.IsNullOrEmpty(package.Tags)
                            ? package.Tags.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                            : Enumerable.Empty<string>();

                var packageId = ExtractTagValueByPrefix(tags, "metapack-toolid-");
                var packageVersion = ExtractTagValueByPrefix(tags, "metapack-toolid-");

                if (!string.IsNullOrEmpty(packageId))
                {
                    result = new SolutionToolPackage
                    {
                        Id = packageId,
                        Version = packageVersion
                    };
                }
                else
                {
                    MetaPackTrace.WriteLine("Cannot find any solution tool package tags.");
                }

                var defaultSolutionPackagingService = new DefaultNuGetSolutionPackageService();
                var solutionPackageFile = defaultSolutionPackagingService.FindSolutionPackageFile(package);

                var serializationService = new DefaultXMLSerializationService();
                serializationService.RegisterKnownType(typeof(SolutionPackageBase));

                using (var streamReader = new StreamReader(solutionPackageFile.GetStream()))
                {
                    var solutionFileContent = streamReader.ReadToEnd();

                    var xDoc = XDocument.Parse(solutionFileContent);
                    var rootElementName = xDoc.Root.Name.LocalName;

                    var defNamespace = xDoc.Root.Attribute("xmlns").Value;
                    var genericXmlDoc = xDoc.ToString()
                                        .Replace(
                                            string.Format("<{0}", rootElementName),
                                            string.Format("<{0}", typeof(SolutionPackageBase).Name))
                                         .Replace(
                                            string.Format("</{0}>", rootElementName),
                                            string.Format("</{0}>", typeof(SolutionPackageBase).Name))

                                        .Replace(defNamespace, "http://schemas.datacontract.org/2004/07/MetaPack.Core.Packaging");



                    var typedPackage = serializationService.Deserialize(
                                        typeof(SolutionPackageBase), genericXmlDoc)
                                        as SolutionPackageBase;

                    var packageIdFromPackage = ExtractAdditionalOption(typedPackage.AdditionalOptions, DefaultOptions.SolutionToolPackage.PackageId.Id);
                    var packageVersionFromPackage = ExtractAdditionalOption(typedPackage.AdditionalOptions, DefaultOptions.SolutionToolPackage.PackageVersion.Id);

                    if (!string.IsNullOrEmpty(packageIdFromPackage))
                    {
                        result = new SolutionToolPackage
                        {
                            Id = packageIdFromPackage,
                            Version = packageVersionFromPackage
                        };
                    }
                    else
                    {
                        MetaPackTrace.WriteLine("Cannot find any solution tool package options in SolutionPackage.AdditionalOptions");
                    }
                }

                if (result == null)
                    throw new ArgumentException("Can't resolve solution tool package from .SolutionToolPackage, tags and solution package itself");
            }
            else
            {
                result = SolutionToolPackage;
            }
            return result;
        }

        private string ExtractAdditionalOption(List<OptionValue> list, string optionId)
        {
            if (list == null)
                return null;

            var option = list.FirstOrDefault(o => o.Name.ToUpper() == optionId.ToUpper());

            if (option != null)
            {
                return option.Value;
            }

            return null;
        }

        private static void SetDefaultXmlNamespace(XElement xelem, XNamespace xmlns)
        {
            xelem.Name = xmlns + xelem.Name.LocalName;

            foreach (var e in xelem.Elements())
                SetDefaultXmlNamespace(e, xmlns);
        }

        protected virtual string ExtractTagValueByPrefix(IEnumerable<string> tags, string valuePrefix)
        {
            var tagValue = tags.FirstOrDefault(t => t.StartsWith(valuePrefix));

            if (!string.IsNullOrEmpty(tagValue))
            {
                var result = tagValue.Replace(tagValue, valuePrefix).Trim();
                return result;
            }

            return null;
        }

        protected virtual void OnPackageInstalling(object sender, PackageOperationEventArgs e)
        {
            // save pachage to a local folder
            // cause package can't be serialized to be passed to a new app domain
            var tmpPackageFilePath = SavePackageAsTempFile(e.Package);

            var toolPackage = ResolveSolutionToolPackage(e.Package);

            if (string.IsNullOrEmpty(toolPackage.Id))
                throw new Exception("ToolPackage.Id is null or empty");

            // install main tool package
            var toolResolver = MetaPackServiceContainer.Instance.GetService<ToolResolutionService>();

            if (toolResolver == null)
                toolResolver = new ToolResolutionService();

            var toolRepo = toolResolver.PackageManager.LocalRepository;

            toolResolver.InstallTool(toolPackage);

            // resolve main assembly, resolve additional tooling
            var toolNuGetPackage = toolResolver.PackageManager.LocalRepository.FindPackage(toolPackage.Id);

            var assemblyHint = toolPackage.AssemblyNameHint ?? toolPackage.Id + ".dll";
            var additionalTools = toolResolver.ResolveAdditionalTooling(toolRepo, toolNuGetPackage, assemblyHint);
            toolResolver.InstallTools(additionalTools);

            // resolve package and deployment classes
            // by default lookup firs implementations of the following classes:
            //     * SolutionPackageServiceBase
            //     * SolutionPackageDeploymentServiceBase
            var packagingServiceClassFullName = toolResolver.ResolveClassImplementationFullName(toolRepo, toolNuGetPackage,
                assemblyHint,
                typeof(SolutionPackageServiceBase).Name);

            if (string.IsNullOrEmpty(packagingServiceClassFullName))
                throw new Exception(string.Format("Cannot find impl for service:[{0}]", typeof(SolutionPackageServiceBase).Name));

            var deploymentServiceFullName = toolResolver.ResolveClassImplementationFullName(toolRepo, toolNuGetPackage,
                assemblyHint,
                typeof(SolutionPackageDeploymentServiceBase).Name);

            if (string.IsNullOrEmpty(deploymentServiceFullName))
                throw new Exception(string.Format("Cannot find impl for service:[{0}]", typeof(SolutionPackageDeploymentServiceBase).Name));

            var toolAssemblies = toolResolver.ResolveAssemblyPaths(toolRepo, toolNuGetPackage, "net45", false);
            var toolAssembly = toolAssemblies.FirstOrDefault(a => a.EndsWith(assemblyHint));

            MetaPackTrace.WriteLine(string.Format("Current Domain:[{0}]", AppDomain.CurrentDomain.Id));

            // unpack, deploy
            using (var context = AppDomainContext.Create())
            {
                var detectedAdditionalToolAssemblies = new List<string>();
                var detectedAdditionalToolAllAssemblies = new List<string>();

                CrossDomainTraceHelper.StartListening(context.Domain);

                // tool assembly
                MetaPackTrace.WriteLine(string.Format("Loading main tool assembly:[{0}]", toolAssembly));
                context.LoadAssembly(LoadMethod.LoadFile, toolAssembly);

                var deploymentOptions = new AppDomainDeploymentOptions
                {
                    PackageFilePath = tmpPackageFilePath,

                    PackagingServiceClassFullName = packagingServiceClassFullName,
                    DeploymentServiceClassFullName = deploymentServiceFullName
                };

                foreach (var opt in SolutionOptions)
                {
                    deploymentOptions.AdditionalOptions.Add(new DeploymentOption
                    {
                        Name = opt.Name,
                        Value = opt.Value
                    });
                }

                // install additional tools

                var result = RemoteFunc.Invoke(context.Domain, deploymentOptions, (ops) =>
                {
                    var ress = new AppDomainDeploymentOptions();

                    MetaPackTrace.WriteLine(string.Format("[!] Domain:[{0}] Call from the remote domain:[{1}]", AppDomain.CurrentDomain.Id, AppDomain.CurrentDomain.Id));

                    MetaPackTrace.WriteLine(string.Format("Package path:[{0}]", ops.PackageFilePath));

                    MetaPackTrace.WriteLine(string.Format("Packaging impl:[{0}]", ops.PackagingServiceClassFullName));
                    MetaPackTrace.WriteLine(string.Format("Deployment impl:[{0}]", ops.DeploymentServiceClassFullName));


                    var allClasses = AppDomain.CurrentDomain
                                             .GetAssemblies()
                                             .SelectMany(a => a.GetTypes());

                    var packagingClassType = allClasses.FirstOrDefault(c => c.FullName.ToUpper() == ops.PackagingServiceClassFullName.ToUpper());
                    var deploymentClassType = allClasses.FirstOrDefault(c => c.FullName.ToUpper() == ops.DeploymentServiceClassFullName.ToUpper());

                    if (packagingClassType == null)
                        throw new Exception(string.Format("Cannot find type by full name:[{0}]", ops.PackagingServiceClassFullName));

                    if (deploymentClassType == null)
                        throw new Exception(string.Format("Cannot find type by full name:[{0}]", ops.DeploymentServiceClassFullName));

                    MetaPackTrace.WriteLine("Creating packaging service implementation...");
                    var packagingService = Activator.CreateInstance(packagingClassType) as SolutionPackageServiceBase;

                    if (packagingService == null)
                        throw new Exception("Cannot create instance of packaging service");

                    MetaPackTrace.WriteLine("Creating deployment service implementation...");
                    var deploymentService = Activator.CreateInstance(deploymentClassType) as SolutionPackageDeploymentServiceBase;

                    if (deploymentService == null)
                        throw new Exception("Cannot create instance of deployment service");

                    // unpack package
                    // TODO
                    MetaPackTrace.WriteLine(string.Format("Reading package:[{0}]", ops.PackageFilePath));
                    using (var packageStream = System.IO.File.OpenRead(ops.PackageFilePath))
                    {
                        MetaPackTrace.WriteLine(string.Format("Unpacking package..."));
                        var solutionPackage = packagingService.Unpack(packageStream);

                        if (solutionPackage != null)
                            MetaPackTrace.WriteLine(string.Format("Succesfully unpacked package."));

                        // deployment options
                        var solutionDeploymentOptions = new SolutionPackageProvisionOptions
                        {
                            SolutionPackage = solutionPackage,
                        };

                        // fill out deployment options
                        foreach (var option in ops.AdditionalOptions)
                            solutionDeploymentOptions.SetOptionValue(option.Name, option.Value);

                        // check for additional tools
                        MetaPackTrace.WriteLine(string.Format("Checking additional tools for unpacked package..."));
                        if (deploymentService is SolutionPackageDeploymentService)
                        {
                            MetaPackTrace.WriteLine(string.Format("Calling SolutionPackageDeploymentService.GetAdditionalToolPackages()..."));

                            var toolableDeploymentService = deploymentService as SolutionPackageDeploymentService;
                            var additonalTool2s =
                                toolableDeploymentService.GetAdditionalToolPackages(solutionDeploymentOptions);

                            if (additonalTool2s.Count() > 0)
                            {
                                foreach (var tmp in additonalTool2s)
                                {
                                    ress.ToolAdditionalPackages.Add(new SolutionToolPackage
                                    {
                                        Id = tmp.Id,
                                        Version = tmp.Version,
                                        AssemblyNameHint = tmp.AssemblyNameHint
                                    });
                                }
                            }
                            else
                            {
                                MetaPackTrace.WriteLine(string.Format("No additional tools were found"));
                            }
                        }
                        else
                        {
                            MetaPackTrace.WriteLine(string.Format("No additional tools are found. Current deployment service isn't of type 'SolutionPackageDeploymentService'"));
                        }

                        MetaPackTrace.WriteLine(string.Format("Deploying package..."));

                        // check fo

                        //deploymentService.Deploy(solutionDeploymentOptions);
                    }

                    return ress;
                });

                // checking detected additional tools
                var detecedAdditionalTools = result.ToolAdditionalPackages;

                foreach (var additionalTool in detecedAdditionalTools)
                {
                    toolResolver.InstallTool(additionalTool.Id);
                    var addToolPackage = toolRepo.FindPackage(additionalTool.Id);

                    var additionalToolAssemblies = toolResolver.ResolveAssemblyPaths(toolRepo, addToolPackage, "net45", false);

                    if (!string.IsNullOrEmpty(additionalTool.AssemblyNameHint))
                    {
                        detectedAdditionalToolAssemblies.AddRange(additionalToolAssemblies
                            .Where(p => p.ToUpper().Contains(additionalTool.AssemblyNameHint.ToUpper())));
                    }
                    else
                    {
                        detectedAdditionalToolAssemblies.AddRange(additionalToolAssemblies);
                    }

                    detectedAdditionalToolAllAssemblies.AddRange(
                        toolResolver.ResolveAssemblyPaths(toolRepo, addToolPackage, "net45", true)
                        );
                }

                // tool additional tool assemblies
                foreach (var tt in detectedAdditionalToolAssemblies)
                {
                    MetaPackTrace.WriteLine(string.Format("Loading additional tool assembly:[{0}]", toolAssembly));
                    context.LoadAssembly(LoadMethod.LoadFile, tt);
                }

                var paths = new List<string>();

                // add probing path for ALL tool assemblies 
                foreach (var assemblyPath in detectedAdditionalToolAllAssemblies)
                {
                    var assemblyDir = Path.GetDirectoryName(assemblyPath);

                    MetaPackTrace.WriteLine(string.Format("Addint probe path:[{0}]", assemblyDir));
                    paths.Add(assemblyDir);

                    context.AssemblyImporter.AddProbePath(assemblyDir);
                }

                // add probing path for ALL tool assemblies 
                foreach (var assemblyPath in toolAssemblies)
                {
                    var assemblyDir = Path.GetDirectoryName(assemblyPath);

                    MetaPackTrace.WriteLine(string.Format("Addint probe path:[{0}]", assemblyDir));
                    paths.Add(assemblyDir);

                    context.AssemblyImporter.AddProbePath(assemblyDir);
                }

                deploymentOptions.AssemblyProbingPaths = paths;

                var result2 = RemoteFunc.Invoke(context.Domain, deploymentOptions, (ops) =>
                {
                    AppDomain.CurrentDomain.AssemblyResolve += (sss, eee) =>
                    {
                        MetaPackTrace.WriteLine(string.Format("WHO:[{0}]", eee));
                        MetaPackTrace.WriteLine(string.Format("WHAT:[{0}]", eee.Name));

                        var assemblyName = eee.Name.Split(',')[0] + ".dll";

                        foreach (var dir in ops.AssemblyProbingPaths)
                        {
                            var tmpAssemblyPath = Path.Combine(dir, assemblyName);

                            if (System.IO.File.Exists(tmpAssemblyPath))
                            {
                                return Assembly.LoadFile(tmpAssemblyPath);
                            }
                        }

                        throw new Exception(string.Format("Cannot load requested assembly [{0}]. Requested by [{1}]",
                            eee.Name,
                            eee.RequestingAssembly));
                    };

                    MetaPackTrace.WriteLine(string.Format("[!] Domain:[{0}] Call from the remote domain:[{1}]", AppDomain.CurrentDomain.Id, AppDomain.CurrentDomain.Id));

                    MetaPackTrace.WriteLine(string.Format("Package path:[{0}]", ops.PackageFilePath));

                    MetaPackTrace.WriteLine(string.Format("Packaging impl:[{0}]", ops.PackagingServiceClassFullName));
                    MetaPackTrace.WriteLine(string.Format("Deployment impl:[{0}]", ops.DeploymentServiceClassFullName));



                    var allClasses = AppDomain.CurrentDomain
                                             .GetAssemblies()
                                             .SelectMany(a => a.GetTypes());

                    var packagingClassType = allClasses.FirstOrDefault(c => c.FullName.ToUpper() == ops.PackagingServiceClassFullName.ToUpper());
                    var deploymentClassType = allClasses.FirstOrDefault(c => c.FullName.ToUpper() == ops.DeploymentServiceClassFullName.ToUpper());

                    if (packagingClassType == null)
                        throw new Exception(string.Format("Cannot find type by full name:[{0}]", ops.PackagingServiceClassFullName));

                    if (deploymentClassType == null)
                        throw new Exception(string.Format("Cannot find type by full name:[{0}]", ops.DeploymentServiceClassFullName));

                    MetaPackTrace.WriteLine("Creating packaging service implementation...");
                    var packagingService = Activator.CreateInstance(packagingClassType) as SolutionPackageServiceBase;

                    if (packagingService == null)
                        throw new Exception("Cannot create instance of packaging service");

                    MetaPackTrace.WriteLine("Creating deployment service implementation...");
                    var deploymentService = Activator.CreateInstance(deploymentClassType) as SolutionPackageDeploymentServiceBase;

                    if (deploymentService == null)
                        throw new Exception("Cannot create instance of deployment service");

                    // unpack package
                    // TODO
                    MetaPackTrace.WriteLine(string.Format("Reading package:[{0}]", ops.PackageFilePath));
                    using (var packageStream = System.IO.File.OpenRead(ops.PackageFilePath))
                    {
                        MetaPackTrace.WriteLine(string.Format("Unpacking package..."));
                        var solutionPackage = packagingService.Unpack(packageStream);

                        if (solutionPackage != null)
                            MetaPackTrace.WriteLine(string.Format("Succesfully unpacked package."));

                        // deployment options
                        var solutionDeploymentOptions = new SolutionPackageProvisionOptions
                        {
                            SolutionPackage = solutionPackage,
                        };

                        // fill out deployment options
                        foreach (var option in ops.AdditionalOptions)
                            solutionDeploymentOptions.SetOptionValue(option.Name, option.Value);

                        MetaPackTrace.WriteLine(string.Format("Deploying package..."));
                        deploymentService.Deploy(solutionDeploymentOptions);
                    }

                    return ops;
                });

            }
        }

        #endregion
    }
}
