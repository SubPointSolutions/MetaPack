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
using MetaPack.Tests.Common;
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
    public class DeploymentScenarioTests : BaseScenarioTest
    {
        #region constructors

        public DeploymentScenarioTests()
        {
            UseLocaNuGet = true;
        }

        #endregion

        #region spmeta2

        [TestMethod]
        [TestCategory("Metapack.Deployment.RootWeb")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_Solution_Package_To_Root_Web()
        {
            Can_Deploy_Solution_Package_Internal(true, 1, false);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.RootWeb")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_Solution_Package_To_Root_Web_Twice()
        {
            Can_Deploy_Solution_Package_Internal(true, 2, false);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.RootWeb")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_Solution_Package_To_Root_Web_With_Dependencies()
        {
            Can_Deploy_Solution_Package_Internal(true, 1, true);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.RootWeb")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_Solution_Package_To_Root_Web_With_Dependencies_Twice()
        {
            Can_Deploy_Solution_Package_Internal(true, 2, true);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.SubWeb")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_Solution_Package_To_Sub_Web()
        {
            Can_Deploy_Solution_Package_Internal(false, 1, false);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.SubWeb")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_Solution_Package_To_Sub_Web_Twice()
        {
            Can_Deploy_Solution_Package_Internal(false, 2, false);
        }


        [TestMethod]
        [TestCategory("Metapack.Deployment.SubWeb")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_Solution_Package_To_Sub_Web_With_Dependencies()
        {
            Can_Deploy_Solution_Package_Internal(false, 1, true);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.SubWeb")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_Solution_Package_To_Sub_Web_With_Dependencies_Twice()
        {
            Can_Deploy_Solution_Package_Internal(false, 2, true);
        }

        #endregion

        #region utils



        private void Can_Deploy_Solution_Package_Internal(bool isRootUrl, int provisionCount, bool? useDependencies)
        {
            WithMetaPackServices(service =>
            {
                var packagingService = service.PackagingService;

                // push
                var solutionPackage = CreateNewSolutionPackage(packagingService);

                var solutionDependencies = new List<SolutionPackageBase>();

                if (useDependencies.HasValue && useDependencies.Value == true)
                {
                    // adding a few dependencies
                    for (var i = 1; i < 3; i++)
                    {
                        var solutionDep = CreateNewSolutionPackage(packagingService,
                            solution => { solution.Id = solution.Id + "dep" + i; });

                        UpdatePackageVersion(solutionDep);
                        solutionDependencies.Add(solutionDep);

                        solutionPackage.Dependencies.Add(new SolutionPackageDependency
                        {
                            Id = solutionDep.Id,
                            Version = solutionDep.Version
                        });
                    }
                }

                UpdatePackageVersion(solutionPackage);

                var packageId = solutionPackage.Id;
                var packageVersion = solutionPackage.Version;

                var nuGetPackageDependencies = new List<Stream>();

                foreach (var soutionDependency in solutionDependencies)
                {
                    var nuGetPackageDependency = packagingService.Pack(soutionDependency, null);
                    nuGetPackageDependencies.Add(nuGetPackageDependency);
                }

                var nuGetPackage = packagingService.Pack(solutionPackage, null);
                var canFind = false;

                WithCINuGetContext((apiUrl, apiKey, repoUrl) =>
                {
                    // push
                    PushPackageToCIRepository(solutionPackage, solutionDependencies, packagingService);
                    // find
                    var ciPackage = FindPackageInCIRepository(packageId, packageVersion);

                    Assert.IsNotNull(ciPackage, "Solution package");
                    canFind = ciPackage != null;

                    Action<ClientContext> action = context =>
                    {
                        for (var index = 0; index < provisionCount; index++)
                        {
                            Trace.WriteLine(string.Format("Installing:[{0}/{1}]", index, provisionCount));

                            WithCIRepositoryContext(repo =>
                            {
                                // create nuget package manager within current SharePoint context
                                var packageManager = new DefaultMetaPackSolutionPackageManager(repo, context);

                                // add options
                                packageManager.SolutionOptions.Add(DefaultOptions.SharePoint.Api.CSOM);
                                packageManager.SolutionOptions.Add(DefaultOptions.SharePoint.Edition.Foundation);
                                packageManager.SolutionOptions.Add(DefaultOptions.SharePoint.Version.O365);

                                packageManager.SolutionOptions.Add(new OptionValue
                                {
                                    Name = DefaultOptions.Site.Url.Id,
                                    Value = context.Url
                                });

                                // if o365 - add user name and password
                                packageManager.SolutionOptions.Add(new OptionValue
                                {
                                    Name = DefaultOptions.User.Name.Id,
                                    Value = O365UserName
                                });

                                packageManager.SolutionOptions.Add(new OptionValue
                                {
                                    Name = DefaultOptions.User.Password.Id,
                                    Value = O365UserPassword
                                });

                                // install package
                                packageManager.InstallPackage(ciPackage, false, false);
                            });
                        }
                    };

                    if (isRootUrl)
                        WithCIRootSharePointContext(action);
                    else
                        WithCISubWebSharePointContext(action);
                });

                Assert.IsTrue(canFind);
            });
        }

        #endregion
    }
}