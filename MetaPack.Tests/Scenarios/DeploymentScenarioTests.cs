using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetaPack.Core.Common;
using MetaPack.NuGet.Services;
using MetaPack.SPMeta2.Services;
using MetaPack.Tests.Base;
using MetaPack.Tests.Extensions;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.Services;
using SPMeta2.Services;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using MetaPack.NuGet.Common;

namespace MetaPack.Tests.Scenarios
{
    public enum RegressionSandbox
    {
        O365,
        SharePoint
    }

    public enum RegressionAPI
    {
        CSOM,
        SSOM
    }

    public enum RegressinModelLevel
    {
        Farm,
        WebApplication,
        Site,
        Web
    }

    public class RegressionDeploymentProfile
    {
        public RegressionDeploymentProfile()
        {

        }

        public RegressionSandbox Sandbox { get; set; }
        public RegressionAPI API { get; set; }

        public RegressinModelLevel ModelLevel { get; set; }
    }

    [TestClass]
    public class DeploymentScenarioTests : MetaPackScenarioTestBase
    {
        #region constructors

        public DeploymentScenarioTests()
        {
            UseLocaNuGet = true;
        }

        #endregion

        #region classes


        #endregion

        #region deployment baseline for SharePoint

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        [TestCategory("Metapack.API.Deployment.SharePoint")]
        [TestCategory("Metapack.API.Deployment.Farm")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_SharePoint_Farm_SSOM()
        {
            var regressionProfile = new RegressionDeploymentProfile();

            regressionProfile.Sandbox = RegressionSandbox.SharePoint;
            regressionProfile.API = RegressionAPI.SSOM;
            regressionProfile.ModelLevel = RegressinModelLevel.Farm;

            WithMetaPackServices(s =>
            {
                Internal_Deploy_SolutionPackage(s, regressionProfile);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        [TestCategory("Metapack.API.Deployment.SharePoint")]
        [TestCategory("Metapack.API.Deployment.SharePoint.WebApplication")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_SharePoint_WebApplication_SSOM()
        {
            var regressionProfile = new RegressionDeploymentProfile();

            regressionProfile.Sandbox = RegressionSandbox.SharePoint;
            regressionProfile.API = RegressionAPI.SSOM;
            regressionProfile.ModelLevel = RegressinModelLevel.WebApplication;

            WithMetaPackServices(s =>
            {
                Internal_Deploy_SolutionPackage(s, regressionProfile);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        [TestCategory("Metapack.API.Deployment.SharePoint")]
        [TestCategory("Metapack.API.Deployment.SharePoint.Site")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_SharePoint_Site_CSOM()
        {
            var regressionProfile = new RegressionDeploymentProfile();

            regressionProfile.Sandbox = RegressionSandbox.SharePoint;
            regressionProfile.API = RegressionAPI.CSOM;
            regressionProfile.ModelLevel = RegressinModelLevel.Site;

            WithMetaPackServices(s =>
            {
                Internal_Deploy_SolutionPackage(s, regressionProfile);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        [TestCategory("Metapack.API.Deployment.SharePoint")]
        [TestCategory("Metapack.API.Deployment.SharePoint.Site")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_SharePoint_Site_SSOM()
        {
            var regressionProfile = new RegressionDeploymentProfile();

            regressionProfile.Sandbox = RegressionSandbox.SharePoint;
            regressionProfile.API = RegressionAPI.SSOM;
            regressionProfile.ModelLevel = RegressinModelLevel.Site;

            WithMetaPackServices(s =>
            {
                Internal_Deploy_SolutionPackage(s, regressionProfile);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        [TestCategory("Metapack.API.Deployment.SharePoint")]
        [TestCategory("Metapack.API.Deployment.SharePoint.Web")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_SharePoint_Web_CSOM()
        {
            var regressionProfile = new RegressionDeploymentProfile();

            regressionProfile.Sandbox = RegressionSandbox.SharePoint;
            regressionProfile.API = RegressionAPI.CSOM;
            regressionProfile.ModelLevel = RegressinModelLevel.Web;

            WithMetaPackServices(s =>
            {
                Internal_Deploy_SolutionPackage(s, regressionProfile);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        [TestCategory("Metapack.API.Deployment.SharePoint")]
        [TestCategory("Metapack.API.Deployment.SharePoint.Web")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_SharePoint_Web_SSOM()
        {
            var regressionProfile = new RegressionDeploymentProfile();

            regressionProfile.Sandbox = RegressionSandbox.SharePoint;
            regressionProfile.API = RegressionAPI.SSOM;
            regressionProfile.ModelLevel = RegressinModelLevel.Web;

            WithMetaPackServices(s =>
            {
                Internal_Deploy_SolutionPackage(s, regressionProfile);
            });
        }

        #endregion

        #region deployment baseline for O365

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        [TestCategory("Metapack.API.Deployment.O365")]
        [TestCategory("Metapack.API.Deployment.O365.Site")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_O365_Site_CSOM()
        {
            var regressionProfile = new RegressionDeploymentProfile();

            regressionProfile.Sandbox = RegressionSandbox.O365;
            regressionProfile.API = RegressionAPI.CSOM;
            regressionProfile.ModelLevel = RegressinModelLevel.Site;

            WithMetaPackServices(s =>
            {
                Internal_Deploy_SolutionPackage(s, regressionProfile);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        [TestCategory("Metapack.API.Deployment.O365")]
        [TestCategory("Metapack.API.Deployment.O365.Web")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_O365_Web_CSOM()
        {
            var regressionProfile = new RegressionDeploymentProfile();

            regressionProfile.Sandbox = RegressionSandbox.O365;
            regressionProfile.API = RegressionAPI.CSOM;
            regressionProfile.ModelLevel = RegressinModelLevel.Web;

            WithMetaPackServices(s =>
            {
                Internal_Deploy_SolutionPackage(s, regressionProfile);
            });
        }

        #endregion

        #region utils

        private void Internal_Deploy_SolutionPackage(
            MetaPackServiceContext regressionContext,
            RegressionDeploymentProfile regressionProfile)
        {
            var packagingService = regressionContext.PackagingService;
            var deploymentService = regressionContext.DeploymentService;

            // create new solution package for this deployment
            var ciSolutionPackage = CreateNewSolutionPackage(packagingService, regressionProfile.ModelLevel);
            UpdatePackageVersion(ciSolutionPackage);

            var packageId = ciSolutionPackage.Id;
            var packageVersion = ciSolutionPackage.Version;

            // push to repo
            PushPackageToCIRepository(ciSolutionPackage, null, packagingService);

            // find
            var ciPackage = FindPackageInCIRepository(packageId, packageVersion);
            Assert.IsNotNull(ciPackage, "Solution package");

            var solutionOptions = new List<OptionValue>();

            WithCIRepositoryContext(ciNuGetRepository =>
            {
                // actual deployment callback
                Action<ClientContext, IPackage, List<OptionValue>> metapackDeployment =
                    (clientContext, package, options) =>
                    {
                        var packageManager = new DefaultMetaPackSolutionPackageManager(ciNuGetRepository, clientContext);

                        // configura options
                        packageManager.SolutionOptions.AddRange(options);

                        // install package
                        packageManager.InstallPackage(package, false, false);
                    };

                if (regressionProfile.Sandbox == RegressionSandbox.O365)
                {
                    var siteUrl = O365RootWebUrl;
                    var userName = O365UserName;
                    var userPassword = O365UserPassword;

                    // checking correct model
                    if (regressionProfile.ModelLevel == RegressinModelLevel.Site)
                        siteUrl = O365RootWebUrl;
                    else if (regressionProfile.ModelLevel == RegressinModelLevel.Web)
                        siteUrl = O365RootWebUrl;
                    else
                    {
                        throw new NotImplementedException(
                               string.Format("Unsupported ModelLevel type:[{0}]",
                               regressionProfile.ModelLevel));
                    }

                    // csom related options
                    solutionOptions.Add(DefaultOptions.SharePoint.Api.CSOM);
                    solutionOptions.Add(DefaultOptions.SharePoint.Edition.Foundation);
                    solutionOptions.Add(DefaultOptions.SharePoint.Version.O365);

                    solutionOptions.Add(new OptionValue
                    {
                        Name = DefaultOptions.Site.Url.Id,
                        Value = siteUrl
                    });

                    solutionOptions.Add(new OptionValue
                    {
                        Name = DefaultOptions.User.Name.Id,
                        Value = userName
                    });

                    solutionOptions.Add(new OptionValue
                    {
                        Name = DefaultOptions.User.Password.Id,
                        Value = userPassword
                    });

                    WithCIO365ClientContext(siteUrl, userName, userPassword, context =>
                    {
                        metapackDeployment(context, ciPackage, solutionOptions);
                    });
                }
                else if (regressionProfile.Sandbox == RegressionSandbox.SharePoint)
                {
                    if (regressionProfile.API == RegressionAPI.CSOM)
                    {
                        // csom related options
                        solutionOptions.Add(DefaultOptions.SharePoint.Api.CSOM);
                        solutionOptions.Add(DefaultOptions.SharePoint.Edition.Foundation);
                        solutionOptions.Add(DefaultOptions.SharePoint.Version.SP2013);

                        var siteUrl = SP2013RootWebUrl;

                        // checking correct model
                        if (regressionProfile.ModelLevel == RegressinModelLevel.Site)
                            siteUrl = SP2013RootWebUrl;
                        else if (regressionProfile.ModelLevel == RegressinModelLevel.Web)
                            siteUrl = SP2013RootWebUrl;
                        else
                        {
                            throw new NotImplementedException(
                                   string.Format("Unsupported ModelLevel type:[{0}]",
                                   regressionProfile.ModelLevel));
                        }

                        solutionOptions.Add(new OptionValue
                        {
                            Name = DefaultOptions.Site.Url.Id,
                            Value = siteUrl
                        });

                        WithCISharePointClientContext(siteUrl, context =>
                        {
                            metapackDeployment(context, ciPackage, solutionOptions);
                        });

                    }
                    else if (regressionProfile.API == RegressionAPI.SSOM)
                    {
                        // csom related options
                        solutionOptions.Add(DefaultOptions.SharePoint.Api.SSOM);
                        solutionOptions.Add(DefaultOptions.SharePoint.Edition.Foundation);
                        solutionOptions.Add(DefaultOptions.SharePoint.Version.SP2013);

                        var siteUrl = SP2013RootWebUrl;

                        // checking correct model
                        if (regressionProfile.ModelLevel == RegressinModelLevel.Site)
                            siteUrl = SP2013RootWebUrl;
                        else if (regressionProfile.ModelLevel == RegressinModelLevel.Web)
                            siteUrl = SP2013RootWebUrl;
                        else
                        {
                            throw new NotImplementedException(
                                   string.Format("Unsupported ModelLevel type:[{0}]",
                                   regressionProfile.ModelLevel));
                        }

                        solutionOptions.Add(new OptionValue
                        {
                            Name = DefaultOptions.Site.Url.Id,
                            Value = siteUrl
                        });

                        WithCISharePointClientContext(siteUrl, context =>
                        {
                            metapackDeployment(context, ciPackage, solutionOptions);
                        });
                    }
                    else
                    {
                        throw new NotImplementedException(
                              string.Format("Unsupported API type:[{0}]",
                              regressionProfile.API));
                    }
                }
                else
                {
                    throw new NotImplementedException(
                          string.Format("Unsupported Sandbox type:[{0}]",
                          regressionProfile.Sandbox));
                }
            });
        }



        #endregion
    }
}