using System;
using System.Security;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using MetaPack.SPMeta2;
using MetaPack.Tests.Common;
using MetaPack.Tests.Consts;
using MetaPack.Tests.Utils;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using System.Collections.Generic;
using MetaPack.NuGet.Services;
using MetaPack.SPMeta2.Services;
using System.Diagnostics;
using System.IO;
using MetaPack.Core;
using MetaPack.NuGet.Common;
using MetaPack.SharePointPnP;
using MetaPack.SharePointPnP.Services;
using MetaPack.Core.Common;
using MetaPack.Tests.Services;

namespace MetaPack.Tests.Base
{
    [TestClass]
    public class BaseScenarioTest
    {
        #region constructors

        public BaseScenarioTest()
        {
            var regressionTraceService = new RegressionTraceService();

            MetaPackServiceContainer.Instance.ReplaceService(typeof(TraceServiceBase), regressionTraceService);

            var useSPMeta2 = true;
            var usePnP = false;

            UseLocaNuGet = true;

            O365UserName = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.O365UserName);
            O365UserPassword = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.O365UserPassword);

            if (!Environment.Is64BitProcess)
                throw new Exception("x64 process is requred. VS -> Test -> Test Settings -> Default process architecture -> x64");

            // packaging
            MetaPackService = new List<MetaPackServiceContext>();

            if (useSPMeta2)
            {
                MetaPackService.Add(new MetaPackServiceContext
                {
                    PackagingService = new SPMeta2SolutionPackageService(),
                    DeploymentService = new SPMeta2SolutionPackageDeploymentService(),

                    ToolPackage = new SolutionToolPackage
                    {
                        Id = "MetaPack.SPMeta2"
                    }
                });
            }

            if (usePnP)
            {
                MetaPackService.Add(new MetaPackServiceContext
                {
                    PackagingService = new SharePointPnPSolutionPackageService(),
                    DeploymentService = new SharePointPnPSolutionDeploymentService(),

                    ToolPackage = new SolutionToolPackage
                    {
                        Id = "MetaPack.SharePointPnP"
                    }
                });
            }

            var localNuGetFolder = "local-nuget-packages";
            LocalNuGetFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, localNuGetFolder);

            Directory.CreateDirectory(LocalNuGetFolderPath);

            if (UseLocaNuGet)
            {
                var toolResolutioService = new ToolResolutionService();

                var localToolPackagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Build\packages");
                localToolPackagesPath = Path.GetFullPath(localToolPackagesPath);

                toolResolutioService.PackageSources.Add(localToolPackagesPath);

                MetaPackServiceContainer.Instance.RegisterService(typeof(ToolResolutionService), toolResolutioService);
            }
        }

        #endregion

        #region classes

        public class MetaPackServiceContext
        {
            public SolutionPackageDeploymentService DeploymentService { get; set; }
            public NuGetSolutionPackageService PackagingService { get; set; }

            public SolutionToolPackage ToolPackage { get; set; }
        }

        #endregion

        #region properties

        public string O365UserName { get; set; }
        public string O365UserPassword { get; set; }

        public bool UseLocaNuGet { get; set; }
        public string LocalNuGetFolderPath { get; set; }

        protected List<MetaPackServiceContext> MetaPackService { get; set; }

        #endregion

        #region general

        protected void WithRootSharePointContext(Action<ClientContext> action)
        {
            var rootWebUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.RootWebUrl);

            if (string.IsNullOrEmpty(rootWebUrl))
                throw new NullReferenceException("rootWebUrl");

            WithSharePointContext(rootWebUrl, action);
        }

        protected void WithSubWebSharePointContext(Action<ClientContext> action)
        {
            var subwebUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.SubWebUrl);

            if (string.IsNullOrEmpty(subwebUrl))
                throw new NullReferenceException("subwebUrl");

            WithSharePointContext(subwebUrl, action);
        }

        protected void WithSharePointContext(string url, Action<ClientContext> action)
        {
            var userName = O365UserName;
            var userPassword = O365UserPassword;

            using (var context = new ClientContext(url))
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPassword))
                {
                    var securePassword = new SecureString();

                    foreach (var c in userPassword)
                        securePassword.AppendChar(c);

                    context.Credentials = new SharePointOnlineCredentials(userName, securePassword);
                    context.ExecuteQuery();
                }

                action(context);
            }
        }

        protected void WithNuGetContext(Action<string, string, string> action)
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

        #endregion

        #region actions

        protected virtual void WithMetaPackServices(Action<MetaPackServiceContext> action)
        {
            foreach (var service in MetaPackService)
            {
                Trace.WriteLine(string.Format("Testing service:[{0}]", service.GetType()));
                action(service);
            }
        }

        #endregion

        #region utils

        protected virtual SolutionPackageBase CreateNewSolutionPackage(NuGetSolutionPackageService service)
        {
            return CreateNewSolutionPackage(service, null);
        }

        protected virtual SolutionPackageBase CreateNewSolutionPackage(NuGetSolutionPackageService service, Action<SolutionPackageBase> action)
        {
            var knownPackageType = false;

            SolutionPackageBase solutionPackage = null;

            if (service is SPMeta2SolutionPackageService)
            {
                var m2package = new SPMeta2SolutionPackage();

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

                var models = new ModelNode[]
                {
                    SPMeta2Model.NewSiteModel(site => { }),
                    SPMeta2Model.NewWebModel(web => { }),
                };

                var tmpFolder = GetTempFolderPath();

                foreach (var model in models)
                {
                    var xmlContext = SPMeta2Model.ToXML(model);

                    var tmpFileName = GetTempXmlFileName();
                    var tmpFilePath = Path.Combine(tmpFolder, tmpFileName);

                    System.IO.File.WriteAllText(tmpFilePath, xmlContext);
                }

                m2package.ModelFolders.Add(tmpFolder);

                m2package.AdditionalOptions.Add(new OptionValue
                {
                    Name = DefaultOptions.SolutionToolPackage.PackageId.Id,
                    Value = "MetaPack.SPMeta2"
                });

                solutionPackage = m2package;
            }

            if (service is SharePointPnPSolutionPackageService)
            {
                var pnpPackage = new SharePointPnPSolutionPackage();

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

                foreach (var folder in Directory.GetDirectories(@"Data/PnPTemplates/Folders"))
                    pnpPackage.ProvisioningTemplateFolders.Add(folder);

                pnpPackage.ProvisioningTemplateOpenXmlPackageFolders.Add(@"Data/PnPTemplates/OpenXML");

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

        #endregion
    }
}
