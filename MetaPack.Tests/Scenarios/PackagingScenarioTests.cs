using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Packaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetaPack.NuGet.Services;
using MetaPack.SPMeta2;
using MetaPack.SPMeta2.Services;
using MetaPack.Tests.Common;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using MetaPack.Core.Services;


using MetaPack.Tests.Base;
using MetaPack.Tests.Consts;
using MetaPack.Tests.Extensions;
using MetaPack.Tests.Utils;
using NuGet;

using File = System.IO.File;

namespace MetaPack.Tests.Scenarios
{
    [TestClass]
    public class PackagingScenarioTests : BaseScenarioTest
    {
        #region spmeta2

        [TestMethod]
        [TestCategory("Metapack.Packaging.SPMeta2")]
        public void Can_Pack_SPMeta2_Package()
        {
            Can_Pack_Internal(SolutionPackageType.SPMeta2, new SPMeta2SolutionPackageService());
        }

        [TestMethod]
        [TestCategory("Metapack.Packaging.SPMeta2")]
        public void Can_Unpack_SPMeta2_Package()
        {
            Can_Unpack_Internal(SolutionPackageType.SPMeta2, new SPMeta2SolutionPackageService(),
                (rawPackage, unpackedPackage) =>
                {
                    var solutionPackage = rawPackage as SPMeta2SolutionPackage;
                    var unpackedSolutionPackage = unpackedPackage as SPMeta2SolutionPackage;

                    Assert.IsNotNull(solutionPackage);
                    Assert.IsNotNull(unpackedSolutionPackage);

                    Assert.AreEqual(solutionPackage.Name, unpackedSolutionPackage.Name);
                    Assert.AreEqual(solutionPackage.Title, unpackedSolutionPackage.Title);
                    Assert.AreEqual(solutionPackage.Description, unpackedSolutionPackage.Description);
                    Assert.AreEqual(solutionPackage.Id, unpackedSolutionPackage.Id);
                    Assert.AreEqual(solutionPackage.Authors, unpackedSolutionPackage.Authors);
                    Assert.AreEqual(solutionPackage.Company, unpackedSolutionPackage.Company);
                    Assert.AreEqual(solutionPackage.Version, unpackedSolutionPackage.Version);
                    Assert.AreEqual(solutionPackage.Owners, unpackedSolutionPackage.Owners);

                    Assert.AreEqual(solutionPackage.ReleaseNotes, unpackedSolutionPackage.ReleaseNotes);
                    Assert.AreEqual(solutionPackage.Summary, unpackedSolutionPackage.Summary);

                    Assert.AreEqual(solutionPackage.ProjectUrl, unpackedSolutionPackage.ProjectUrl);
                    Assert.AreEqual(solutionPackage.IconUrl, unpackedSolutionPackage.IconUrl);
                    Assert.AreEqual(solutionPackage.LicenseUrl, unpackedSolutionPackage.LicenseUrl);
                    Assert.AreEqual(solutionPackage.Copyright, unpackedSolutionPackage.Copyright);
                    Assert.AreEqual(solutionPackage.Tags, unpackedSolutionPackage.Tags);

                    Assert.AreEqual(solutionPackage.Dependencies.Count, unpackedSolutionPackage.Dependencies.Count);

                    foreach (var dependency in solutionPackage.Dependencies)
                    {
                        var unpackedDependency = unpackedSolutionPackage.Dependencies.FirstOrDefault(d => d.Id == dependency.Id);

                        Assert.AreEqual(dependency.Id, unpackedDependency.Id);
                        Assert.AreEqual(dependency.Version, unpackedDependency.Version);
                    }

                    Assert.AreEqual(solutionPackage.Models.Count, unpackedSolutionPackage.Models.Count);
                });
        }


        [TestMethod]
        [TestCategory("Metapack.Packaging.SPMeta2")]
        public void Can_Push_SPMeta2_Package()
        {
            Can_Push_Internal(SolutionPackageType.SPMeta2, new SPMeta2SolutionPackageService());
        }

        #endregion

        #region internal tests impl

        private void Can_Pack_Internal(SolutionPackageType type, NuGetSolutionPackageService packagingService)
        {
            var solutionPackage = CreateNewSolutionPackage(type);
            var nuGetPackage = packagingService.Pack(solutionPackage);

            // mem stream
            Assert.IsNotNull(nuGetPackage);
            Assert.IsTrue(nuGetPackage.Length > 0);

            // to file extension
            var fileName = string.Format("{0}.{1}.nupkg", solutionPackage.Id, solutionPackage.Version);

            var fileDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            var filePath = Path.Combine(fileDir, fileName);

            Directory.CreateDirectory(fileDir);

            packagingService.PackToFile(solutionPackage, filePath);

            Assert.IsTrue(File.Exists(filePath));
        }

        private void Can_Unpack_Internal(SolutionPackageType type,
            NuGetSolutionPackageService packagingService,
            Action<SolutionPackageBase, SolutionPackageBase> action)
        {
            var solutionPackage = CreateNewSolutionPackage(type);
            var nuGetPackage = packagingService.Pack(solutionPackage);

            Assert.IsNotNull(nuGetPackage);
            Assert.IsTrue(nuGetPackage.Length > 0);

            var fileName = string.Format("{0}.{1}.nupkg", solutionPackage.Id, solutionPackage.Version);

            var fileDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            var filePath = Path.Combine(fileDir, fileName);

            Directory.CreateDirectory(fileDir);
            packagingService.PackToFile(solutionPackage, filePath);

            // unpacking and checking props
            using (var streamReader = File.OpenRead(filePath))
            {

                var unpackedSolutionPackage = packagingService.Unpack(streamReader) as SolutionPackageBase;
                action(solutionPackage, unpackedSolutionPackage);
            }
        }

        private void Can_Push_Internal(SolutionPackageType type, NuGetSolutionPackageService packagingService)
        {
            WithNuGetContext((apiUrl, apiKey, repoUrl) =>
            {
                var solutionPackage = CreateNewSolutionPackage(type);

                UpdatePackageVersion(solutionPackage);
                packagingService.Push(solutionPackage, apiUrl, apiKey);

                var ciRepo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
                var ciPackage = ciRepo.FindPackageSafe(solutionPackage.Id);

                Assert.IsNotNull(ciPackage);
            });
        }

        #endregion
    }
}
