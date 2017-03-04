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
        public enum RegressionSandbox
        {
            SharePoint_Farm,
            SharePoint_WebApplication,
            SharePoint_Site,
            SharePoint_Web,
            SharePoint_SubWeb,

            O365_Site,
            O365_Web,
            O365_SubWeb,
        }

        #endregion

        #region deployment baseline for SharePoint

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_SharePoint_Farm_Level()
        {
            WithMetaPackServices(s =>
            {
                Deploy_SolutionPackage_Internal(s, RegressionSandbox.SharePoint_Farm);
            });
        }


        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_SharePoint_WebApplication_Level()
        {
            WithMetaPackServices(s =>
            {
                Deploy_SolutionPackage_Internal(s, RegressionSandbox.SharePoint_WebApplication);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_SharePoint_Site()
        {
            WithMetaPackServices(s =>
            {
                Deploy_SolutionPackage_Internal(s, RegressionSandbox.SharePoint_Site);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_SharePoint_Web()
        {
            WithMetaPackServices(s =>
            {
                Deploy_SolutionPackage_Internal(s, RegressionSandbox.SharePoint_Web);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_SharePoint_SubWeb()
        {
            WithMetaPackServices(s =>
            {
                Deploy_SolutionPackage_Internal(s, RegressionSandbox.SharePoint_SubWeb);
            });
        }

        #endregion

        #region deployment baseline for O365

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_O365_Site()
        {
            WithMetaPackServices(s =>
            {
                Deploy_SolutionPackage_Internal(s, RegressionSandbox.O365_Site);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_O365_Web()
        {
            WithMetaPackServices(s =>
            {
                Deploy_SolutionPackage_Internal(s, RegressionSandbox.O365_Web);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_SolutionPackage_On_O365_SubWeb()
        {
            WithMetaPackServices(s =>
            {
                Deploy_SolutionPackage_Internal(s, RegressionSandbox.O365_SubWeb);
            });
        }

        #endregion

        #region utils

        private void Deploy_SolutionPackage_Internal(
            MetaPackServiceContext regressionContext,
            RegressionSandbox regressionSandbox)
        {
            var packagingService = regressionContext.PackagingService;

            // create new solution package for this deployment
            var solutionPackage = CreateNewSolutionPackage(packagingService);
            UpdatePackageVersion(solutionPackage);

            var packageId = solutionPackage.Id;
            var packageVersion = solutionPackage.Version;

            // push to repo
            PushPackageToCIRepository(solutionPackage, null, packagingService);

            // find
            var ciPackage = FindPackageInCIRepository(packageId, packageVersion);
            Assert.IsNotNull(ciPackage, "Solution package");

            WithCIRepositoryContext(ciNuGetRepository =>
            {
                // 1 - metapack 'connection' details
                // 2 - consigure packageManager with correct options



                switch (regressionSandbox)
                {
                    case RegressionSandbox.SharePoint_Farm:
                        throw new NotImplementedException(regressionSandbox.ToString());
                        break;

                    case RegressionSandbox.SharePoint_WebApplication:
                        throw new NotImplementedException(regressionSandbox.ToString());
                        break;

                    case RegressionSandbox.SharePoint_Site:
                        throw new NotImplementedException(regressionSandbox.ToString());
                        break;

                    case RegressionSandbox.SharePoint_Web:
                        throw new NotImplementedException(regressionSandbox.ToString());
                        break;

                    case RegressionSandbox.SharePoint_SubWeb:
                        throw new NotImplementedException(regressionSandbox.ToString());
                        break;

                    case RegressionSandbox.O365_Site:

                        WithCIRootSharePointContext(context =>
                        {
                            var webSiteUrl = context.Url;

                            var userName = O365UserName;
                            var userPassword = O365UserPassword;

                            var spApiOption = DefaultOptions.SharePoint.Api.CSOM;
                            var spEditioniOption = DefaultOptions.SharePoint.Edition.Foundation;
                            var spVersionOption = DefaultOptions.SharePoint.Version.O365;

                            var packageManager = new DefaultMetaPackSolutionPackageManager(ciNuGetRepository, context);

                            // add options
                            packageManager.SolutionOptions.Add(spApiOption);
                            packageManager.SolutionOptions.Add(spEditioniOption);
                            packageManager.SolutionOptions.Add(spVersionOption);

                            packageManager.SolutionOptions.Add(new OptionValue
                            {
                                Name = DefaultOptions.Site.Url.Id,
                                Value = webSiteUrl
                            });

                            if (!string.IsNullOrEmpty(userName))
                            {
                                packageManager.SolutionOptions.Add(new OptionValue
                                {
                                    Name = DefaultOptions.User.Name.Id,
                                    Value = userName
                                });
                            }

                            if (!string.IsNullOrEmpty(userPassword))
                            {
                                packageManager.SolutionOptions.Add(new OptionValue
                                {
                                    Name = DefaultOptions.User.Password.Id,
                                    Value = userPassword
                                });
                            }

                            // install package
                            packageManager.InstallPackage(ciPackage, false, false);
                        });

                        break;

                    case RegressionSandbox.O365_Web:

                        // todo, here should be a web level model
                        WithCIRootSharePointContext(context =>
                        {
                            var webSiteUrl = context.Url;

                            var userName = O365UserName;
                            var userPassword = O365UserPassword;

                            var spApiOption = DefaultOptions.SharePoint.Api.CSOM;
                            var spEditioniOption = DefaultOptions.SharePoint.Edition.Foundation;
                            var spVersionOption = DefaultOptions.SharePoint.Version.O365;

                            var packageManager = new DefaultMetaPackSolutionPackageManager(ciNuGetRepository, context);

                            // add options
                            packageManager.SolutionOptions.Add(spApiOption);
                            packageManager.SolutionOptions.Add(spEditioniOption);
                            packageManager.SolutionOptions.Add(spVersionOption);

                            packageManager.SolutionOptions.Add(new OptionValue
                            {
                                Name = DefaultOptions.Site.Url.Id,
                                Value = webSiteUrl
                            });

                            if (!string.IsNullOrEmpty(userName))
                            {
                                packageManager.SolutionOptions.Add(new OptionValue
                                {
                                    Name = DefaultOptions.User.Name.Id,
                                    Value = userName
                                });
                            }

                            if (!string.IsNullOrEmpty(userPassword))
                            {
                                packageManager.SolutionOptions.Add(new OptionValue
                                {
                                    Name = DefaultOptions.User.Password.Id,
                                    Value = userPassword
                                });
                            }

                            // install package
                            packageManager.InstallPackage(ciPackage, false, false);
                        });

                        break;

                    case RegressionSandbox.O365_SubWeb:

                        WithCISubWebSharePointContext(context =>
                        {
                            var webSiteUrl = context.Url;

                            var userName = O365UserName;
                            var userPassword = O365UserPassword;

                            var spApiOption = DefaultOptions.SharePoint.Api.CSOM;
                            var spEditioniOption = DefaultOptions.SharePoint.Edition.Foundation;
                            var spVersionOption = DefaultOptions.SharePoint.Version.O365;

                            var packageManager = new DefaultMetaPackSolutionPackageManager(ciNuGetRepository, context);

                            // add options
                            packageManager.SolutionOptions.Add(spApiOption);
                            packageManager.SolutionOptions.Add(spEditioniOption);
                            packageManager.SolutionOptions.Add(spVersionOption);

                            packageManager.SolutionOptions.Add(new OptionValue
                            {
                                Name = DefaultOptions.Site.Url.Id,
                                Value = webSiteUrl
                            });

                            if (!string.IsNullOrEmpty(userName))
                            {
                                packageManager.SolutionOptions.Add(new OptionValue
                                {
                                    Name = DefaultOptions.User.Name.Id,
                                    Value = userName
                                });
                            }

                            if (!string.IsNullOrEmpty(userPassword))
                            {
                                packageManager.SolutionOptions.Add(new OptionValue
                                {
                                    Name = DefaultOptions.User.Password.Id,
                                    Value = userPassword
                                });
                            }

                            // install package
                            packageManager.InstallPackage(ciPackage, false, false);
                        });

                        break;

                    default:
                        break;
                }
            });
        }

        #endregion
    }
}