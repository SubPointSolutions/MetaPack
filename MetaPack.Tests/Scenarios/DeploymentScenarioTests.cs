using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

namespace MetaPack.Tests.Scenarios
{
    [TestClass]
    public class DeploymentScenarioTests : BaseScenarioTest
    {
        #region spmeta2

        [TestMethod]
        [TestCategory("Metapack.Deployment.SPMeta2")]
        [TestCategory("CI.Core")]
        public void Can_Deploy_SPMeta2_Package_To_Root_Web()
        {
            Can_Deploy_SPMeta2_Package_Internal(true, 1, false);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.SPMeta2")]
        [TestCategory("CI.Core")]
        public void Can_Deploy_SPMeta2_Package_To_Root_Web_Twice()
        {
            Can_Deploy_SPMeta2_Package_Internal(true, 2, false);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.SPMeta2")]
        [TestCategory("CI.Core")]
        public void Can_Deploy_SPMeta2_Package_To_Root_Web_With_Dependencies()
        {
            Can_Deploy_SPMeta2_Package_Internal(true, 1, true);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.SPMeta2")]
        [TestCategory("CI.Core")]
        public void Can_Deploy_SPMeta2_Package_To_Root_Web_With_Dependencies_Twice()
        {
            Can_Deploy_SPMeta2_Package_Internal(true, 2, true);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.SPMeta2")]
        [TestCategory("CI.Core")]
        public void Can_Deploy_SPMeta2_Package_To_Sub_Web()
        {
            Can_Deploy_SPMeta2_Package_Internal(false, 1, false);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.SPMeta2")]
        [TestCategory("CI.Core")]
        public void Can_Deploy_SPMeta2_Package_To_Sub_Web_Twice()
        {
            Can_Deploy_SPMeta2_Package_Internal(false, 2, false);
        }


        [TestMethod]
        [TestCategory("Metapack.Deployment.SPMeta2")]
        [TestCategory("CI.Core")]
        public void Can_Deploy_SPMeta2_Package_To_Sub_Web_With_Dependencies()
        {
            Can_Deploy_SPMeta2_Package_Internal(false, 1, true);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.SPMeta2")]
        [TestCategory("CI.Core")]
        public void Can_Deploy_SPMeta2_Package_To_Sub_Web_With_Dependencies_Twice()
        {
            Can_Deploy_SPMeta2_Package_Internal(false, 2, true);
        }

       
        #endregion

        #region utils

        private void Can_Deploy_SPMeta2_Package_Internal(bool isRootUrl, int provisionCount, bool? useDependencies)
        {
            var type = SolutionPackageType.SPMeta2;
            var packagingService = new SPMeta2SolutionPackageService();

            // push
            var solutionPackage = CreateNewSolutionPackage(type);

            var solutionDependencies = new List<SolutionPackageBase>();

            if (useDependencies.HasValue && useDependencies.Value == true)
            {
                // adding a few dependencies
                for (var i = 1; i < 3; i++)
                {
                    var solutionDep = CreateNewSolutionPackage(type, solution =>
                    {
                        solution.Id = solution.Id + "dep" + i;

                    });

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

            WithNuGetContext((apiUrl, apiKey, repoUrl) =>
            {
                var repo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);

                foreach (var soutionDependency in solutionDependencies)
                {
                    packagingService.Push(soutionDependency, apiUrl, apiKey);
                }

                packagingService.Push(solutionPackage, apiUrl, apiKey);

                // get the package
                var ciRepo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
                var ciPackage = ciRepo.FindPackageSafe(packageId, new SemanticVersion(packageVersion));

                Assert.IsNotNull(ciPackage, "Solution package");

                canFind = ciPackage != null;

                Action<ClientContext> action = context =>
                {
                    for (var index = 0; index < provisionCount; index++)
                    {
                        Trace.WriteLine(string.Format("Installing:[{0}/{1}]", index, provisionCount));

                        // create nuget package manager within current SharePoint context
                        var packageManager = new SPMeta2SolutionPackageManager(repo, context);

                        // setup provision services
                        packageManager.ProvisionService = new StandardCSOMProvisionService();
                        packageManager.ProvisionServiceHost = context;

                        // just for tracing / logging
                        packageManager.ProvisionService.OnModelNodeProcessed += (sender, args) =>
                        {
                            Trace.WriteLine(
                                string.Format(" Processed: [{0}/{1}] - [{2}%] - [{3}] [{4}]",
                                    new object[]
                                    {
                                        args.ProcessedModelNodeCount,
                                        args.TotalModelNodeCount,
                                        100d*(double) args.ProcessedModelNodeCount/(double) args.TotalModelNodeCount,
                                        args.CurrentNode.Value.GetType().Name,
                                        args.CurrentNode.Value
                                    }));
                        };

                        // install package
                        packageManager.InstallPackage(ciPackage, false, false);
                    }
                };

                if (isRootUrl)
                    WithRootSharePointContext(action);
                else
                    WithSubWebSharePointContext(action);
            });

            Assert.IsTrue(canFind);
        }


        #endregion
    }
}
