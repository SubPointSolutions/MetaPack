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

namespace MetaPack.Tests.Scenarios
{
    [TestClass]
    public class DeploymentScenarioTests : BaseScenarioTest
    {
        #region spmeta2

        [TestMethod]
        [TestCategory("Metapack.Deployment.SPMeta2")]
        public void Can_Deploy_SPMeta2_Package_To_Root_Web()
        {
            Can_Deploy_SPMeta2_Package_Internal(true, 1);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.SPMeta2")]
        public void Can_Deploy_SPMeta2_Package_To_Root_Web_Twice()
        {
            Can_Deploy_SPMeta2_Package_Internal(true, 2);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.SPMeta2")]
        public void Can_Deploy_SPMeta2_Package_To_Sub_Web()
        {
            Can_Deploy_SPMeta2_Package_Internal(false, 1);
        }

        [TestMethod]
        [TestCategory("Metapack.Deployment.SPMeta2")]
        public void Can_Deploy_SPMeta2_Package_To_Sub_Web_Twice()
        {
            Can_Deploy_SPMeta2_Package_Internal(false, 2);
        }

        private void Can_Deploy_SPMeta2_Package_Internal(bool isRootUrl, int provisionCount)
        {
            var type = SolutionPackageType.SPMeta2;
            var packagingService = new SPMeta2SolutionPackageService();

            // push
            var solutionPackage = CreateNewSolutionPackage(type);
            UpdatePackageVersion(solutionPackage);

            var packageId = solutionPackage.Id;
            var packageVersion = solutionPackage.Version;

            var nuGetPackage = packagingService.Pack(solutionPackage, null);
            var canFind = false;

            WithNuGetContext((apiUrl, apiKey, repoUrl) =>
            {
                var repo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);

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

                        // create numage package manager within current SharePoint context
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
                        packageManager.InstallPackage(ciPackage, true, false);
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

        #region utils



        #endregion
    }
}
