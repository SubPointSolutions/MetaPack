using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDomainToolkit;
using MetaPack.Core.Exceptions;
using MetaPack.Core.Utils;
using MetaPack.NuGet.Common;
using MetaPack.NuGet.Utils;
using NuGet;

namespace MetaPack.NuGet.Services
{
    public class ToolResolutionService
    {
        #region constructors

        public ToolResolutionService()
        {
            PackageSources = new List<string>();
            PackageSources.Add("https://packages.nuget.org/api/v2");

            var toolFolder = "tool-packages";
            ToolPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, toolFolder);
        }

        #endregion

        #region classes

        [Serializable]
        public class ResolveAdditionalToolingOptions : MarshalByRefObject
        {
            public string AssemblyName { get; set; }
        }

        [Serializable]
        public class ResolveClassImplementationOptions : MarshalByRefObject
        {
            public string AssemblyName { get; set; }
            public string ClassName { get; set; }
        }

        #endregion

        #region properties

        protected PackageManager toolPackageManager;

        public PackageManager ToolPackageManager
        {
            get
            {
                if (toolPackageManager == null)
                    InitPackageManager();

                return toolPackageManager;
            }
            set { toolPackageManager = value; }
        }

        public string ToolPath { get; set; }

        public List<string> PackageSources { get; set; }

        #endregion

        #region methods

        public string ResolveClassImplementationFullName(IPackageRepository packageRepository,
           IPackage nuGetPackage,
           string assemblyName,
           string className)
        {
            var result = string.Empty;

            if (string.IsNullOrEmpty(assemblyName))
                throw new Exception("AssemblyNameHint is null");

            var assemblies = ResolveAssemblyPaths(packageRepository, nuGetPackage, "net45", false);
            var targetAssemblyPath = assemblies.FirstOrDefault(p => p.Contains(assemblyName));

            if (string.IsNullOrEmpty(targetAssemblyPath))
                throw new Exception(string.Format("Cannot find assembly [{0}] in nuget package:[{1}]",
                    assemblyName, nuGetPackage.Id));

            using (var context = AppDomainContext.Create())
            {
                context.LoadAssembly(LoadMethod.LoadFile, targetAssemblyPath);

                var options = new ResolveClassImplementationOptions
                {
                    AssemblyName = assemblyName,
                    ClassName = className
                };

                var tmpResult = RemoteFunc.Invoke(context.Domain, options, (ops) =>
                {
                    var tt = typeof(NuGetSolutionPackageService);
                    var tmp = new List<SolutionToolPackage>();

                    var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .OrderBy(s => s.Name)
                        .ToList();

                    var toolResolutioNServiceBaseType = allTypes.FirstOrDefault(t => t.Name.ToUpper() == ops.ClassName.ToUpper());

                    if (toolResolutioNServiceBaseType == null)
                        throw new Exception(string.Format("Cannot find type for class:[{0}]", ops.ClassName));

                    var assembly = AppDomain.CurrentDomain.GetAssemblies()
                                            .FirstOrDefault(a => a.Location.Contains(ops.AssemblyName));

                    var toolResolutionType = assembly.GetTypes()
                        .FirstOrDefault(t => toolResolutioNServiceBaseType.IsAssignableFrom(t));

                    if (toolResolutionType != null)
                    {
                        return toolResolutionType.FullName;
                    }

                    return null;
                });

                result = tmpResult.Clone() as string;
            }

            return result;
        }

        public List<SolutionToolPackage> ResolveAdditionalTooling(IPackageRepository packageRepository,
            IPackage nuGetPackage,
            string assemblyName)
        {
            var result = new List<SolutionToolPackage>();

            if (string.IsNullOrEmpty(assemblyName))
                throw new Exception("AssemblyNameHint is null");

            var assemblies = ResolveAssemblyPaths(packageRepository, nuGetPackage, "net45", false);
            var targetAssemblyPath = assemblies.FirstOrDefault(p => p.Contains(assemblyName));

            if (string.IsNullOrEmpty(targetAssemblyPath))
                throw new Exception(string.Format("Cannot find assembly [{0}] in nuget package:[{1}]",
                    assemblyName, nuGetPackage.Id));

            using (var context = AppDomainContext.Create())
            {
                context.LoadAssembly(LoadMethod.LoadFile, targetAssemblyPath);

                var options = new ResolveAdditionalToolingOptions
                {
                    AssemblyName = assemblyName
                };

                var tmpResult = RemoteFunc.Invoke(context.Domain, options, (ops) =>
                {
                    var tmp = new List<SolutionToolPackage>();

                    var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());
                    var toolResolutioNServiceBaseType = allTypes.FirstOrDefault(t => t.Name == "ToolResolutionServiceBase");

                    var assembly = AppDomain.CurrentDomain.GetAssemblies()
                                            .FirstOrDefault(a => a.Location.Contains(ops.AssemblyName));

                    var toolResolutionType = assembly.GetTypes()
                        .FirstOrDefault(t => toolResolutioNServiceBaseType.IsAssignableFrom(t));

                    if (toolResolutionType != null)
                    {
                        var toolResolutionImpl = Activator.CreateInstance(toolResolutionType);

                        var additionalTools = ReflectionUtils.InvokeMethod(toolResolutionImpl, "GetAdditionalToolPackages") as List<SolutionToolPackage>;

                        if (additionalTools != null)
                        {
                            tmp.AddRange(additionalTools);
                        }
                    }

                    return tmp;
                });

                foreach (var tool in tmpResult)
                {
                    result.Add(new SolutionToolPackage
                    {
                        Id = tool.Id,
                        Version = tool.Version,
                        AssemblyNameHint = tool.AssemblyNameHint
                    });
                }
            }

            return result;
        }

        protected virtual void InitPackageManager()
        {
            Directory.CreateDirectory(ToolPath);

            var repo = new AggregateRepository(PackageRepositoryFactory.Default, PackageSources, true);

            toolPackageManager = new PackageManager(
               repo,
               new DefaultPackagePathResolver("https://packages.nuget.org/api/v2"),
               new PhysicalFileSystem(ToolPath)
           );

            toolPackageManager.PackageInstalling += (s, e) =>
            {
                MetaPackTrace.Info("Installing tool [{0}] version [{1}]", e.Package.Id, e.Package.Version);
            };

            toolPackageManager.PackageInstalled += (s, e) =>
            {
                MetaPackTrace.Info("Installed tool [{0}] version [{1}]", e.Package.Id, e.Package.Version);
            };
        }

        public void RefreshPackageManager()
        {
            InitPackageManager();
        }

        internal void InstallTool(SolutionToolPackage toolPackage)
        {
            InstallTool(toolPackage.Id, toolPackage.Version);
        }

        public virtual void InstallTool(string packageId, string packageVersion)
        {
            InstallTools(new Dictionary<string, string>
            {
                {packageId, packageVersion}
            });
        }

        public virtual void InstallTools(IEnumerable<SolutionToolPackage> packages)
        {
            foreach (var tool in packages)
            {
                InstallTool(tool.Id, tool.Version);
            }
        }

        public virtual void InstallTools(Dictionary<string, string> packages)
        {
            // install
            foreach (var id in packages.Keys)
            {
                var toolPackage = id;
                var toolPackageVersion = packages[id];

                MetaPackTrace.Verbose(string.Format("Resolving tool:[{0}] version:[{1}]", toolPackage, toolPackageVersion));
                IPackage localPackage = null;

                if (!string.IsNullOrEmpty(toolPackageVersion))
                    localPackage = ToolPackageManager.LocalRepository.FindPackage(toolPackage, new SemanticVersion(toolPackageVersion));
                else
                    localPackage = ToolPackageManager.LocalRepository.FindPackage(toolPackage);

                if (localPackage == null)
                {
                    MetaPackTrace.Verbose(string.Format("Tool package does not exist locally. Installing..."));

                    IPackage package;

                    if (!string.IsNullOrEmpty(toolPackageVersion))
                        package = ToolPackageManager.SourceRepository.FindPackage(toolPackage, new SemanticVersion(toolPackageVersion));
                    else
                        package = ToolPackageManager.SourceRepository.FindPackage(toolPackage);

                    if (package == null)
                    {
                        throw new MetaPackException(string.Format("Cannot find package:[{0}] version:[{1}]", toolPackage, toolPackageVersion));
                    }



                    ToolPackageManager.InstallPackage(package, false, true, false);
                }
                else
                {
                    MetaPackTrace.Verbose(string.Format("Tool exists. No need for install"));
                }
            }
        }

        public virtual List<string> ResolveAssemblyPaths(IPackageRepository packageRepository, IPackage localToolPackage,
            string netVersion)
        {
            return ResolveAssemblyPaths(packageRepository, localToolPackage, netVersion, true);
        }

        public virtual List<string> ResolveAssemblyPaths(IPackageRepository packageRepository, IPackage localToolPackage, string netVersion, bool recursive)
        {
            var result = new List<string>();

            _resolvedPackages.Clear();
            ResolveAssemblyPathsInternal(packageRepository, localToolPackage, netVersion, result, recursive);

            return result;
        }

        private Dictionary<string, List<string>> _resolvedPackages = new Dictionary<string, List<string>>();
        protected virtual void ResolveAssemblyPathsInternal(IPackageRepository packageRepository,
            IPackage localToolPackage, string netVersion,
            List<string> paths,
             bool recursive)
        {
            var packageId = localToolPackage.Id;
            var packageVersion = localToolPackage.Version.Version.ToString();

            if (!_resolvedPackages.ContainsKey(packageId))
                _resolvedPackages.Add(packageId, new List<string>());
            else
            {
                if (!_resolvedPackages[packageId].Contains(packageVersion))
                    _resolvedPackages[packageId].Add(packageVersion);
                else
                    return;
            }

            var packageFolder = string.Format("{0}.{1}", localToolPackage.Id, localToolPackage.Version);
            var packageFolderPath = Path.Combine(packageRepository.Source, packageFolder);

            var assemblies = localToolPackage.AssemblyReferences;

            var foundTargetNetVersion = false;

            // todo, implement a better assembly resolution here
            foreach (var assembly in assemblies)
            {
                if (assembly.Path.Contains(@"\" + netVersion + @"\"))
                {
                    foundTargetNetVersion = true;

                    var assemblyPath = Path.Combine(packageFolderPath, assembly.Path);

                    if (!File.Exists(assemblyPath))
                        throw new ArgumentException(string.Format("Cannot find file:[{0}]", assemblyPath));

                    if (!paths.Contains(assemblyPath))
                        paths.Add(assemblyPath);
                }
            }

            if (!foundTargetNetVersion && assemblies.Count() > 0)
            {
                var targetAssemblye = assemblies.First();

                var assemblyPath = Path.Combine(packageFolderPath, targetAssemblye.Path);

                if (!paths.Contains(assemblyPath))
                    paths.Add(assemblyPath);
            }

            if (recursive)
            {
                foreach (var depSet in localToolPackage.DependencySets)
                {
                    foreach (var dependency in depSet.Dependencies)
                    {
                        var package = packageRepository.FindPackage(dependency.Id, dependency.VersionSpec, true, false);
                        ResolveAssemblyPathsInternal(packageRepository, package, netVersion, paths, recursive);
                    }
                }
            }
        }

        #endregion

        public void InitPackageSourcesFromString(string value)
        {
            var paths = ResolveNuGetGalleryPaths(value);
            InitPackageSources(paths);
        }

        private void InitPackageSources(List<string> paths)
        {
            foreach (var path in paths)
                if (!PackageSources.Contains(path))
                    PackageSources.Add(path);
        }

        public void InitPackageSourcesFromGetEnvironmentVariable(string variableName, EnvironmentVariableTarget variableTarget)
        {
            var paths = ResolveNuGetGalleryPaths(Environment.GetEnvironmentVariable(variableName, variableTarget));
            InitPackageSources(paths);
        }

        protected virtual List<string> ResolveNuGetGalleryPaths(string value)
        {
            var result = new List<string>();

            if (!string.IsNullOrEmpty(value))
            {
                var urls = value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var url in urls)
                {
                    if (url.ToLower().StartsWith("http"))
                    {
                        result.Add(url);
                    }
                    else
                    {
                        var localPath = Path.GetFullPath(url);
                        result.Add(localPath);
                    }
                }
            }

            return result;
        }
    }
}
