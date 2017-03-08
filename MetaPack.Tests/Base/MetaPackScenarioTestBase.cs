using System;
using System.Security;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using MetaPack.SPMeta2;
using MetaPack.Tests.Consts;
using MetaPack.Tests.Utils;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using System.Collections.Generic;
using System.Configuration;
using MetaPack.NuGet.Services;
using MetaPack.SPMeta2.Services;
using System.Diagnostics;
using System.IO;
using System.Text;
using MetaPack.Core;
using MetaPack.NuGet.Common;
using MetaPack.SharePointPnP;
using MetaPack.SharePointPnP.Services;
using MetaPack.Core.Common;
using MetaPack.Tests.Extensions;
using MetaPack.Tests.Services;
using NuGet;
using System.IO.Compression;
using System.Net;
using MetaPack.Tests.Scenarios;

namespace MetaPack.Tests.Base
{
    [TestClass]
    public class MetaPackScenarioTestBase
    {
        #region constructors

        public MetaPackServiceContext SPMeta2ServiceContext { get; set; }
        public MetaPackServiceContext SharePointPnPServiceContext { get; set; }

        public MetaPackScenarioTestBase()
        {
            var regressionTraceService = new RegressionTraceService();

            SPMeta2SolutionPackagingService = new SPMeta2SolutionPackageService();
            SharePointPnPSolutionPackagingService = new SharePointPnPSolutionPackageService();

            MetaPackServiceContainer.Instance.ReplaceService(typeof(TraceServiceBase), regressionTraceService);

            UseLocaNuGet = true;

            InitEnvironmentVariables();

            SPMeta2ServiceContext = new MetaPackServiceContext
            {
                PackagingService = new SPMeta2SolutionPackageService(),
                DeploymentService = new SPMeta2SolutionPackageDeploymentService(),

                ToolPackage = new SolutionToolPackage
                {
                    Id = "MetaPack.SPMeta2"
                },

                CIPackageId = "MetaPack.SPMeta2.CI"
            };

            SharePointPnPServiceContext = new MetaPackServiceContext
            {
                PackagingService = new SharePointPnPSolutionPackageService(),
                DeploymentService = new SharePointPnPSolutionDeploymentService(),

                ToolPackage = new SolutionToolPackage
                {
                    Id = "MetaPack.SharePointPnP"
                },

                CIPackageId = "MetaPack.SharePointPnP.CI"
            };

            //if (!Environment.Is64BitProcess)
            //    throw new Exception("x64 process is requred. VS -> Test -> Test Settings -> Default process architecture -> x64");

            // packaging
            MetaPackService = new List<MetaPackServiceContext>();

            MetaPackService.Add(SPMeta2ServiceContext);
            MetaPackService.Add(SharePointPnPServiceContext);

            var localAssemblyDirectoryPath = Path.GetDirectoryName(GetType().Assembly.Location);
            var localNuGetFolder = Path.GetFullPath(localAssemblyDirectoryPath + @"\..\..\..\Build\local-ci-packages");

            LocalNuGetRepositoryFolderPath = localNuGetFolder;

            Directory.CreateDirectory(LocalNuGetRepositoryFolderPath);

            if (UseLocaNuGet)
            {
                var toolResolutionService = new ToolResolutionService();
                toolResolutionService.PackageSources.Add(LocalNuGetRepositoryFolderPath);

                toolResolutionService.InitPackageSourcesFromString(ConfigurationManager.AppSettings["NuGet.Galleries"]);
                toolResolutionService.InitPackageSourcesFromGetEnvironmentVariable("MetaPack.NuGet.Galleries", EnvironmentVariableTarget.Machine);
                toolResolutionService.InitPackageSourcesFromGetEnvironmentVariable("MetaPack.NuGet.Galleries", EnvironmentVariableTarget.User);
                toolResolutionService.InitPackageSourcesFromGetEnvironmentVariable("MetaPack.NuGet.Galleries", EnvironmentVariableTarget.Process);

                toolResolutionService.RefreshPackageManager();

                MetaPackServiceContainer.Instance.RegisterService(typeof(ToolResolutionService), toolResolutionService);
            }
        }


        protected virtual void CreateNewSolutionPackage(MetaPackServiceContext service)
        {
            CreateNewSolutionPackage(service, RegressinModelLevel.Site);
        }

        protected virtual void CreateNewSolutionPackage(MetaPackServiceContext service,
            RegressinModelLevel modelLevel)
        {
            var solutionPackage = CreateNewSolutionPackage(service.PackagingService, null, modelLevel);
            UpdatePackageVersion(solutionPackage);
            PushPackageToCIRepository(solutionPackage, null, service.PackagingService);
        }

        private void InitEnvironmentVariables()
        {
            O365UserName = EnvironmentUtils.GetEnvironmentVariable(RegConsts.O365.UserName);
            O365UserPassword = EnvironmentUtils.GetEnvironmentVariable(RegConsts.O365.UserPassword);

            O365RootWebUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.O365.RootWebUrl);
            O365SubWebUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.O365.SubWebUrl);

            SP2013RootWebUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SP2013.RootWebUrl);
            SP2013SubWebUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SP2013.SubWebUrl);

            SP2013UserName = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SP2013.UserName);
            SP2013UserPassword = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SP2013.UserPassword);
        }

        private static List<string> ResolveNuGetGalleryPaths(string value)
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

        #endregion

        #region classes

        public class MetaPackServiceContext
        {
            public SolutionPackageDeploymentService DeploymentService { get; set; }
            public NuGetSolutionPackageService PackagingService { get; set; }

            public SolutionToolPackage ToolPackage { get; set; }

            public string CIPackageId { get; set; }
        }

        #endregion

        #region properties

        public NuGetSolutionPackageService SharePointPnPSolutionPackagingService { get; set; }
        public NuGetSolutionPackageService SPMeta2SolutionPackagingService { get; set; }

        public string O365UserName { get; set; }
        public string O365UserPassword { get; set; }

        public string O365RootWebUrl { get; set; }

        public string O365SubWebUrl { get; set; }

        public bool UseLocaNuGet { get; set; }
        public string LocalNuGetRepositoryFolderPath { get; set; }

        protected List<MetaPackServiceContext> MetaPackService { get; set; }

        #endregion

        #region general

        protected virtual void PushPackageToCIRepository(
           SolutionPackageBase solutionPackage,
           NuGetSolutionPackageService packagingService
       )
        {
            PushPackageToCIRepository(solutionPackage, null, packagingService);
        }

        protected virtual void PushPackageToCIRepository(
            SolutionPackageBase solutionPackage,
            List<SolutionPackageBase> solutionDependencies,
            NuGetSolutionPackageService packagingService)
        {
            PushPackageToCIRepository(solutionPackage, solutionDependencies, packagingService, UseLocaNuGet);
        }

        protected virtual void PushPackageToCIRepository(
            SolutionPackageBase solutionPackage,
            List<SolutionPackageBase> solutionDependencies,
            NuGetSolutionPackageService packagingService,
            bool useLocal
            )
        {
            IPackageRepository repo = null;

            if (solutionDependencies != null)
            {
                foreach (var soutionDependency in solutionDependencies)
                {
                    if (useLocal)
                    {
                        var filePath = Path.Combine(LocalNuGetRepositoryFolderPath,
                            String.Format("{0}.{1}.nupkg", soutionDependency.Id, soutionDependency.Version));
                        packagingService.PackToFile(soutionDependency, filePath);

                    }
                    else
                    {
                        WithCINuGetContext((apiUrl, apiKey, repoUrl) =>
                        {
                            packagingService.Push(soutionDependency, apiUrl, apiKey);
                        });
                    }
                }
            }

            if (useLocal)
            {
                var filePath = Path.Combine(LocalNuGetRepositoryFolderPath,
                    String.Format("{0}.{1}.nupkg", solutionPackage.Id, solutionPackage.Version));
                packagingService.PackToFile(solutionPackage, filePath);
            }
            else
            {
                WithCINuGetContext((apiUrl, apiKey, repoUrl) =>
                {
                    packagingService.Push(solutionPackage, apiUrl, apiKey);
                });
            }
        }

        protected virtual void WithCIRepositoryContext(Action<IPackageRepository> action)
        {
            if (UseLocaNuGet)
            {
                var ciRepo = PackageRepositoryFactory.Default.CreateRepository(LocalNuGetRepositoryFolderPath);

                action(ciRepo);
            }
            else
            {
                WithCINuGetContext((apiUrl, apiKey, repoUrl) =>
                {
                    var ciRepo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
                    action(ciRepo);
                });
            }
        }

        protected IPackage FindPackageInCIRepository(string packageId, string packageVersion)
        {
            IPackage result = null;

            WithCIRepositoryContext(repo =>
            {
                result = repo.FindPackageSafe(packageId, new SemanticVersion(packageVersion));
            });

            return result;
        }



        protected void WithCINuGetContext(Action<string, string, string> action)
        {
            var apiUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.NuGet.ApiUrl);
            var apiKey = EnvironmentUtils.GetEnvironmentVariable(RegConsts.NuGet.ApiKey);
            var repoUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.NuGet.RepoUrl);

            if (string.IsNullOrEmpty(apiUrl))
                throw new NullReferenceException("apiUrl");

            if (string.IsNullOrEmpty(apiKey))
                throw new NullReferenceException("apiKey");

            if (string.IsNullOrEmpty(repoUrl))
                throw new NullReferenceException("repoUrl");

            action(apiUrl, apiKey, repoUrl);
        }


        protected static void UpdatePackageVersion(SolutionPackageBase package)
        {
            var date = DateTime.UtcNow;
            package.Version = string.Format("1.{0}.{1}.{2}",
                date.ToString("yyyy"),
                date.ToString("MMdd"),
                date.ToString("HHHmm"));
        }

        protected void WithCIO365ClientContext(string url,
           string userName,
           string userPassword,
           Action<ClientContext> action)
        {
            using (var context = new ClientContext(url))
            {
                var securePassword = new SecureString();

                foreach (var c in userPassword)
                    securePassword.AppendChar(c);

                context.Credentials = new SharePointOnlineCredentials(userName, securePassword);
                context.ExecuteQuery();

                action(context);
            }
        }

        protected void WithCISharePointClientContext(string url,
           Action<ClientContext> action)
        {
            WithCISharePointNetworkCredsClientContext(url, null, null, action);
        }

        protected void WithCISharePointNetworkCredsClientContext(string url,
           string userName,
           string userPassword,
           Action<ClientContext> action)
        {
            using (var context = new ClientContext(url))
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPassword))
                {
                    var securePassword = new SecureString();

                    foreach (var c in userPassword)
                        securePassword.AppendChar(c);

                    context.Credentials = new NetworkCredential(userName, securePassword);
                }

                context.ExecuteQuery();

                action(context);
            }
        }

        #endregion

        #region actions

        protected virtual void WithMetaPackServices(Action<MetaPackServiceContext> action)
        {
            foreach (var service in MetaPackService)
            {
                Trace.WriteLine(string.Format("Testing services:"));

                Trace.WriteLine(string.Format(" ToolID:[{0}]", service.ToolPackage.Id));
                Trace.WriteLine(string.Format(" ToolVersion:[{0}]", service.ToolPackage.Version));
                Trace.WriteLine(string.Format(" PackagingService:[{0}]", service.PackagingService));
                Trace.WriteLine(string.Format(" DeploymentService:[{0}]", service.DeploymentService));

                Trace.WriteLine(string.Format(" CI package ID:[{0}]", service.DeploymentService));

                CreateNewSolutionPackage(service);

                action(service);
            }
        }

        #endregion

        #region utils

        protected virtual SolutionPackageBase CreateNewSolutionPackage(NuGetSolutionPackageService service)
        {
            return CreateNewSolutionPackage(service, null, RegressinModelLevel.Site);
        }

        protected virtual SolutionPackageBase CreateNewSolutionPackage(NuGetSolutionPackageService service,
               RegressinModelLevel modelLevel)
        {
            return CreateNewSolutionPackage(service, null, modelLevel);
        }

        protected virtual SolutionPackageBase CreateNewSolutionPackage(
                NuGetSolutionPackageService service,
                Action<SolutionPackageBase> action,
                RegressinModelLevel modelLevel)
        {
            var knownPackageType = false;

            SolutionPackageBase solutionPackage = null;

            if (service is SPMeta2SolutionPackageService)
            {
                var m2package = new SolutionPackageBase();

                m2package.Name = "SPMeta2 CI Package Name";
                m2package.Title = "SPMeta2 CI Package Title";

                m2package.Description = "SPMeta2 CI Package description";
                m2package.Id = "MetaPack.SPMeta2.CI";
                m2package.Authors = "SubPoint Solutions Authors";
                m2package.Company = "SubPoint Solutions Company";
                m2package.Version = "1.0.0.0";
                m2package.Owners = "SubPoint Solutions Owners";

                m2package.ReleaseNotes = "ReleaseNotes";
                m2package.Summary = "Summary";
                m2package.ProjectUrl = "https://github.com/SubPointSolutions/metapack";
                m2package.IconUrl = "https://github.com/SubPointSolutions/metapack/metapack.png";
                m2package.LicenseUrl = "https://opensource.org/licenses/MIT";

                m2package.Copyright = "Some copyright here";
                m2package.Tags = "CI SPMeta2 MetaPack Tags";

                var models = new List<ModelNode>();

                switch (modelLevel)
                {
                    case RegressinModelLevel.Farm:
                        models.Add(SPMeta2Model.NewFarmModel(farm => { }));
                        break;

                    case RegressinModelLevel.WebApplication:
                        models.Add(SPMeta2Model.NewWebApplicationModel(webApp => { }));
                        break;

                    case RegressinModelLevel.Site:
                        models.Add(SPMeta2Model.NewSiteModel(site => { }));
                        models.Add(SPMeta2Model.NewWebModel(web => { }));
                        break;

                    case RegressinModelLevel.Web:
                        models.Add(SPMeta2Model.NewWebModel(web => { }));
                        break;

                    default:
                        throw new NotImplementedException(string.Format(
                            "Unsupported model level:[{0}] for model genaration",
                            modelLevel));
                }

                var index = 0;

                foreach (var model in models)
                {
                    index++;

                    var xmlContext = SPMeta2Model.ToXML(model);

                    var modelContainer = new ModelContainerBase
                    {
                        Model = Encoding.UTF8.GetBytes(xmlContext),
                    };

                    modelContainer.AdditionalOptions.Add(new OptionValue
                    {
                        Name = DefaultOptions.Model.Order.Id,
                        Value = index.ToString()
                    });

                    m2package.AddModel(modelContainer);
                }

                m2package.AdditionalOptions.Add(new OptionValue
                {
                    Name = DefaultOptions.SolutionToolPackage.PackageId.Id,
                    Value = "MetaPack.SPMeta2"
                });

                solutionPackage = m2package;
            }

            if (service is SharePointPnPSolutionPackageService)
            {
                var pnpPackage = new SolutionPackageBase();

                pnpPackage.Name = "SharePointPnP CI Package Name";
                pnpPackage.Title = "SharePointPnP Package Title";

                pnpPackage.Description = "SPMeta2 CI Package description";
                pnpPackage.Id = "MetaPack.SharePointPnP.CI";
                pnpPackage.Authors = "SubPoint Solutions Authors";
                pnpPackage.Company = "SubPoint Solutions Company";
                pnpPackage.Version = "1.0.0.0";
                pnpPackage.Owners = "SubPoint Solutions Owners";

                pnpPackage.ReleaseNotes = "ReleaseNotes";
                pnpPackage.Summary = "Summary";
                pnpPackage.ProjectUrl = "https://github.com/SubPointSolutions/metapack";
                pnpPackage.IconUrl = "https://github.com/SubPointSolutions/metapack/metapack.png";
                pnpPackage.LicenseUrl = "https://opensource.org/licenses/MIT";

                pnpPackage.Copyright = "Some copyright here";
                pnpPackage.Tags = "CI SPMeta2 MetaPack Tags";


                // TODO
                // Zip up and set the model type
                var asmFolder = Path.GetDirectoryName(GetType().Assembly.Location);

                var foldersPath = Path.Combine(asmFolder, @"Data/PnPTemplates/Folders");
                var openXmlFolderPath = Path.Combine(asmFolder, @"Data/PnPTemplates/OpenXML");

                foreach (var templateFolder in Directory.GetDirectories(foldersPath))
                {
                    // package up into zip
                    var templateFolderZipFile = GetTempZipFilePath();
                    ZipFile.CreateFromDirectory(templateFolder, templateFolderZipFile);

                    var modelContainer = new ModelContainerBase
                    {
                        Model = System.IO.File.ReadAllBytes(templateFolderZipFile),
                    };

                    modelContainer.AdditionalOptions.Add(new OptionValue
                    {
                        Name = DefaultOptions.Model.Type.Id,
                        Value = "SharePointPnP.FolderZip"
                    });

                    pnpPackage.AddModel(modelContainer);
                }




                var openXmlPackages = Directory.GetFiles(openXmlFolderPath, "*.pnp");

                foreach (var file in openXmlPackages)
                {
                    var modelContainer = new ModelContainerBase
                    {
                        Model = System.IO.File.ReadAllBytes(file),
                    };

                    modelContainer.AdditionalOptions.Add(new OptionValue
                    {
                        Name = DefaultOptions.Model.Type.Id,
                        Value = "SharePointPnP.OpenXml"
                    });

                    pnpPackage.AddModel(modelContainer);
                }

                pnpPackage.AdditionalOptions.Add(new OptionValue
                {
                    Name = DefaultOptions.SolutionToolPackage.PackageId.Id,
                    Value = "MetaPack.SharePointPnP"
                });

                solutionPackage = pnpPackage;
            }

            if (solutionPackage == null)
            {
                throw new NotImplementedException(string.Format(
                        "Unknown service type:[{0}]", service.GetType()));
            }

            if (action != null)
                action(solutionPackage);

            return solutionPackage;
        }

        protected virtual string GetTempFolderPath()
        {
            return GetTempFolderPath(true);
        }

        protected virtual string GetTempFolderPath(bool ensureFolder)
        {
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));

            if (ensureFolder)
                Directory.CreateDirectory(path);

            return path;
        }

        protected virtual string GetTempXmlFileName()
        {
            return string.Format("{0}.xml", Guid.NewGuid().ToString("N"));
        }

        protected virtual string GetTempXmlFilePath()
        {
            return Path.Combine(GetTempFolderPath(), GetTempXmlFileName());
        }

        protected virtual string GetTempZipFilePath()
        {
            return Path.Combine(GetTempFolderPath(), string.Format("{0}.zip", Guid.NewGuid().ToString("N")));
        }

        protected virtual string GetTempNuGetFileName()
        {
            return string.Format("{0}.nupkg", Guid.NewGuid().ToString("N"));
        }

        protected virtual string GetTempNuGetFilePath()
        {
            return Path.Combine(GetTempFolderPath(), GetTempXmlFileName());
        }

        #endregion



        public string SP2013RootWebUrl { get; set; }

        public string SP2013SubWebUrl { get; set; }

        public string SP2013UserName { get; set; }

        public string SP2013UserPassword { get; set; }
    }
}
