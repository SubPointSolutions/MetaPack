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
using MetaPack.Core.Common;
using MetaPack.SharePointPnP;
using MetaPack.SharePointPnP.Services;

namespace MetaPack.Tests.Scenarios
{
    [TestClass]
    public class PackagingScenarioTests : MetaPackScenarioTestBase
    {
        #region core packaging

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


                        if (service.DeploymentService is SPMeta2SolutionPackageDeploymentService)
                        {
                            var m2package = solutionPackage as SolutionPackageBase;
                            var m2unpackedPackage = unpackedSolutionPackage as SolutionPackageBase;

                            Assert.IsNotNull(m2package);
                            Assert.IsNotNull(m2unpackedPackage);

                            Assert.AreEqual(m2package.GetModels().Count(), m2unpackedPackage.GetModels().Count());

                            foreach (var modelContainer in m2unpackedPackage.GetModels())
                            {
                                Assert.IsNotNull(modelContainer.Model);
                                Assert.IsNotNull(modelContainer.AdditionalOptions);

                                // we should be able o unpack it
                                var modelXml = Encoding.UTF8.GetString(modelContainer.Model);

                                Assert.IsTrue(!string.IsNullOrEmpty(modelXml));

                                var m2model = SPMeta2Model.FromXML(modelXml);
                                Assert.IsNotNull(m2model);
                            }
                        }

                        if (service.DeploymentService is SharePointPnPSolutionDeploymentService)
                        {
                            var pnpPackage = solutionPackage as SolutionPackageBase;
                            var pnpUnpackedPackage = unpackedSolutionPackage as SolutionPackageBase;

                            Assert.IsNotNull(pnpPackage);

                            Assert.IsNotNull(pnpPackage);
                            Assert.IsNotNull(pnpUnpackedPackage);

                            Assert.AreEqual(pnpPackage.GetModels().Count(), pnpUnpackedPackage.GetModels().Count());

                            foreach (var modelContainer in pnpUnpackedPackage.GetModels())
                            {
                                Assert.IsNotNull(modelContainer.Model);
                                Assert.IsNotNull(modelContainer.AdditionalOptions);
                            }

                            // TODO

                            //// same amount of files per forlders
                            //foreach (var folderPath in pnpPackage.ProvisioningTemplateFolders)
                            //{
                            //    var folderName = new DirectoryInfo(folderPath).Name;
                            //    var srcFilesCount = Directory.GetFiles(folderPath).Length;

                            //    var dstFolder = pnpUnpackedPackage.ProvisioningTemplateFolders
                            //        .FirstOrDefault(f => f.EndsWith(folderName));

                            //    Assert.IsNotNull(dstFolder);

                            //    var dstFilesCount = Directory.GetFiles(dstFolder).Length;

                            //    Assert.AreEqual(srcFilesCount, dstFilesCount);
                            //}

                            //// same amount of folders for OpenXml packages
                            //Assert.AreEqual(pnpPackage.ProvisioningTemplateOpenXmlPackageFolders.Count,
                            //                pnpUnpackedPackage.ProvisioningTemplateOpenXmlPackageFolders.Count);

                            //// same amount of files per forlders
                            //foreach (var folderPath in pnpPackage.ProvisioningTemplateOpenXmlPackageFolders)
                            //{
                            //    var folderName = new DirectoryInfo(folderPath).Name;
                            //    var srcFilesCount = Directory.GetFiles(folderPath).Length;

                            //    var dstFolder = pnpUnpackedPackage.ProvisioningTemplateOpenXmlPackageFolders
                            //        .FirstOrDefault(f => f.EndsWith(folderName));

                            //    Assert.IsNotNull(dstFolder);

                            //    var dstFilesCount = Directory.GetFiles(dstFolder).Length;

                            //    Assert.AreEqual(srcFilesCount, dstFilesCount);
                            //}
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

        #region extended api

        [TestMethod]
        [TestCategory("Metapack.Core.Packaging.ModelOrder")]
        [TestCategory("CI.Core")]
        public void Can_Unpack_Solution_Package_WithOrderedModels()
        {
            WithMetaPackServices(service =>
            {
                Can_Unpack_Internal(service.PackagingService,
                        prePackagedSolution =>
                        {
                            // clean models
                            var models = prePackagedSolution.GetModels().ToArray();

                            foreach (var model in models)
                                prePackagedSolution.RemoveModel(model);


                            var firstModel = new ModelContainerBase();
                            firstModel.AdditionalOptions.Add(new OptionValue
                            {
                                Name = DefaultOptions.Model.Order.Id,
                                Value = "100"
                            });

                            var secondModel = new ModelContainerBase();
                            secondModel.AdditionalOptions.Add(new OptionValue
                            {
                                Name = DefaultOptions.Model.Order.Id,
                                Value = "200"
                            });

                            var thirdModel = new ModelContainerBase();
                            thirdModel.AdditionalOptions.Add(new OptionValue
                            {
                                Name = DefaultOptions.Model.Order.Id,
                                Value = "300"
                            });

                            prePackagedSolution.AddModel(secondModel);
                            prePackagedSolution.AddModel(firstModel);
                            prePackagedSolution.AddModel(thirdModel);

                        },
                        (rawPackage, unpackedPackage) =>
                        {
                            var solutionPackage = rawPackage as SolutionPackageBase;
                            var unpackedSolutionPackage = unpackedPackage as SolutionPackageBase;

                            Assert.IsNotNull(solutionPackage);

                            // these should be ordred by Order flag
                            var models = unpackedPackage.GetModels();

                            var orderValues = models.Select(m =>
                            {
                                var orderValue = m.AdditionalOptions
                                    .FirstOrDefault(o => o.Name == DefaultOptions.Model.Order.Id);

                                return int.Parse(orderValue.Value);
                            }).ToArray();

                            for (var index = 0; index < orderValues.Count() - 1; index++)
                            {
                                var firstOrder = orderValues[index];
                                var secondOrder = orderValues[index + 1];

                                Assert.IsTrue(firstOrder <= secondOrder);
                            }

                            // must be array of int-s 
                        });
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
            Can_Unpack_Internal(packagingService, null, action);
        }

        private void Can_Unpack_Internal(
            NuGetSolutionPackageService packagingService,
            Action<SolutionPackageBase> basePackageAction,
            Action<SolutionPackageBase, SolutionPackageBase> action)
        {
            var solutionPackage = CreateNewSolutionPackage(packagingService);

            if (basePackageAction != null)
                basePackageAction(solutionPackage);

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
