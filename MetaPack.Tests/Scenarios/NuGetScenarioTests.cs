using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetaPack.Core.Services;
using MetaPack.NuGet.Services;
using MetaPack.SPMeta2.Services;
using MetaPack.Tests.Base;
using MetaPack.Tests.Common;
using MetaPack.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;

namespace MetaPack.Tests.Scenarios
{
    [TestClass]
    public class NuGetScenarioTests : BaseScenarioTest
    {
        #region spmeta2

        [TestMethod]
        [TestCategory("Metapack.NuGet")]
        [TestCategory("CI.Core")]
        public void Can_Push_And_Find_SPMeta2_Package()
        {
            Can_Push_And_Find_Package_Internal(SolutionPackageType.SPMeta2, new SPMeta2SolutionPackageService());
        }

        #endregion

        #region utils

        private void Can_Push_And_Find_Package_Internal(SolutionPackageType type,
            NuGetSolutionPackageService packagingService)
        {
            // push
            var solutionPackage = CreateNewSolutionPackage(type);
            UpdatePackageVersion(solutionPackage);

            var packageId = solutionPackage.Id;
            var packageVersion = solutionPackage.Version;

            var nuGetPackage = packagingService.Pack(solutionPackage, null);
            var canFind = false;

            WithNuGetContext((apiUrl, apiKey, repoUrl) =>
            {
                packagingService.Push(solutionPackage, apiUrl, apiKey);

                var ciRepo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
                var ciPackage = ciRepo.FindPackageSafe(packageId, new SemanticVersion(packageVersion));

                Assert.IsNotNull(ciPackage);

                canFind = ciPackage != null;
            });

            Assert.IsTrue(canFind);
        }

        #endregion
    }
}
