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
using System.Diagnostics;
using MetaPack.SharePointPnP;

namespace MetaPack.Tests.Scenarios
{
    [TestClass]
    public class PackagingScenarioTests : MetaPackScenarioTestBase
    {
        #region spmeta2

        [TestMethod]
        [TestCategory("Metapack.Core.Packaging")]
        [TestCategory("CI.Core")]
        public void Can_Pack_Solution_Package()
        {
            WithMetaPackServices(service =>
            {
                Can_Pack_Internal(service.PackagingService);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.Core.Packaging")]
        [TestCategory("CI.Core")]
        public void Can_Unpack_Solution_Package()
        {
            WithMetaPackServices(service =>
            {
                Can_Unpack_Internal(service.PackagingService,
                    (rawPackage, unpackedPackage) =>
                    {
                        var solutionPackage = rawPackage as SolutionPackageBase;
                        var unpackedSolutionPackage = unpackedPackage as SolutionPackageBase;

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
                            var unpackedDependency =
                                unpackedSolutionPackage.Dependencies.FirstOrDefault(d => d.Id == dependency.Id);

                            Assert.AreEqual(dependency.Id, unpackedDependency.Id);
                            Assert.AreEqual(dependency.Version, unpackedDependency.Version);
                        }

                        // package specific tests
                        if (solutionPackage is SPMeta2SolutionPackage)
                        {
                            var m2package = solutionPackage as SPMeta2SolutionPackage;
                            var m2unpackedPackage = unpackedSolutionPackage as SPMeta2SolutionPackage;

                            Assert.IsNotNull(m2package);
                            Assert.IsNotNull(m2unpackedPackage);

                            Assert.AreEqual(m2package.ModelFolders.Count, m2unpackedPackage.ModelFolders.Count);

                            // each folders should have the same amount of *.xml files
                            foreach (var folderPath in m2package.ModelFolders)
                            {
                                var folderName = new DirectoryInfo(folderPath).Name;
                                var srcFilesCount = Directory.GetFiles(folderPath).Length;

                                var dstFolder = m2unpackedPackage.ModelFolders
                                    .FirstOrDefault(f => f.EndsWith(folderName));

                                Assert.IsNotNull(dstFolder);

                                var dstFilesCount = Directory.GetFiles(dstFolder).Length;

                                Assert.AreEqual(srcFilesCount, dstFilesCount);
                            }
                        }
                        if (solutionPackage is SharePointPnPSolutionPackage)
                        {
                            var pnpPackage = solutionPackage as SharePointPnPSolutionPackage;
                            var pnpUnpackedPackage = unpackedSolutionPackage as SharePointPnPSolutionPackage;

                            Assert.IsNotNull(pnpPackage);

                            // should have same amount of folders
                            Assert.AreEqual(pnpPackage.ProvisioningTemplateFolders.Count,
                                            pnpUnpackedPackage.ProvisioningTemplateFolders.Count);

                            // same amount of files per forlders
                            foreach (var folderPath in pnpPackage.ProvisioningTemplateFolders)
                            {
                                var folderName = new DirectoryInfo(folderPath).Name;
                                var srcFilesCount = Directory.GetFiles(folderPath).Length;

                                var dstFolder = pnpUnpackedPackage.ProvisioningTemplateFolders
                                    .FirstOrDefault(f => f.EndsWith(folderName));

                                Assert.IsNotNull(dstFolder);

                                var dstFilesCount = Directory.GetFiles(dstFolder).Length;

                                Assert.AreEqual(srcFilesCount, dstFilesCount);
                            }

                            // same amount of folders for OpenXml packages
                            Assert.AreEqual(pnpPackage.ProvisioningTemplateOpenXmlPackageFolders.Count,
                                            pnpUnpackedPackage.ProvisioningTemplateOpenXmlPackageFolders.Count);

                            // same amount of files per forlders
                            foreach (var folderPath in pnpPackage.ProvisioningTemplateOpenXmlPackageFolders)
                            {
                                var folderName = new DirectoryInfo(folderPath).Name;
                                var srcFilesCount = Directory.GetFiles(folderPath).Length;

                                var dstFolder = pnpUnpackedPackage.ProvisioningTemplateOpenXmlPackageFolders
                                    .FirstOrDefault(f => f.EndsWith(folderName));

                                Assert.IsNotNull(dstFolder);

                                var dstFilesCount = Directory.GetFiles(dstFolder).Length;

                                Assert.AreEqual(srcFilesCount, dstFilesCount);
                            }
                        }
                    });
            });
        }


        [TestMethod]
        [TestCategory("Metapack.Core.Packaging")]
        [TestCategory("CI.Core")]
        public void Can_Push_Solution_Package()
        {
            WithMetaPackServices(service =>
            {
                Can_Push_Internal(service.PackagingService);
            });
        }

        #endregion

        #region internal tests impl

        private void Can_Pack_Internal(NuGetSolutionPackageService packagingService)
        {
            var solutionPackage = CreateNewSolutionPackage(packagingService);
            var nuGetPackage = packagingService.Pack(solutionPackage);

            // mem stream
            Assert.IsNotNull(nuGetPackage);
            Assert.IsTrue(nuGetPackage.Length > 0);

            var filePath = GetTempNuGetFilePath();
            packagingService.PackToFile(solutionPackage, filePath);

            Assert.IsTrue(File.Exists(filePath));
        }

        private void Can_Unpack_Internal(
            NuGetSolutionPackageService packagingService,
            Action<SolutionPackageBase, SolutionPackageBase> action)
        {
            var solutionPackage = CreateNewSolutionPackage(packagingService);
            var nuGetPackage = packagingService.Pack(solutionPackage);

            Assert.IsNotNull(nuGetPackage);
            Assert.IsTrue(nuGetPackage.Length > 0);

            var filePath = GetTempNuGetFilePath();

            packagingService.PackToFile(solutionPackage, filePath);

            // unpacking and checking props
            using (var streamReader = File.OpenRead(filePath))
            {
                var unpackedSolutionPackage = packagingService.Unpack(streamReader) as SolutionPackageBase;
                action(solutionPackage, unpackedSolutionPackage);
            }
        }

        private void Can_Push_Internal(NuGetSolutionPackageService packagingService)
        {
            WithCINuGetContext((apiUrl, apiKey, repoUrl) =>
            {
                var solutionPackage = CreateNewSolutionPackage(packagingService);

                UpdatePackageVersion(solutionPackage);
                packagingService.Push(solutionPackage, apiUrl, apiKey);

                var ciRepo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
                var ciPackage = ciRepo.FindPackageSafe(solutionPackage.Id);

                Assert.IsNotNull(ciPackage);

                Trace.WriteLine(string.Format("Found package:[{0}]", ciPackage));
            });
        }

        #endregion
    }
}
