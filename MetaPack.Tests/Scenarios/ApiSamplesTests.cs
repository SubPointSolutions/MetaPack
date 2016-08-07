using System;
using System.Diagnostics;
using System.IO;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using MetaPack.SPMeta2;
using MetaPack.SPMeta2.Services;
using MetaPack.Tests.Base;
using MetaPack.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;
using SPMeta2.CSOM.Standard.Services;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace MetaPack.Tests.Scenarios
{
    [TestClass]
    public class ApiSamplesTests : BaseScenarioTest
    {
        #region tests

        [TestMethod]
        [TestCategory("Metapack.Samples")]
        public void Can_Package_SPMeta2_Models_To_NuGet_Package()
        {
            // create package service instance
            var packagingService = new SPMeta2SolutionPackageService();

            // create SPMeta2 solution package
            var solutionPackage = new SPMeta2SolutionPackage();

            solutionPackage.Title = "SPMeta2 CI Package";
            solutionPackage.Id = "SPMeta2.CI";
            solutionPackage.Version = "1.0.0.0";
            solutionPackage.Authors = "SubPoint Solutions";
            solutionPackage.Description = "A test package for SPMeta1 models.";

            // add some models
            var models = new ModelNode[]
                {
                    SPMeta2Model.NewSiteModel(site => { }),
                    SPMeta2Model.NewWebModel(web => { }),
                };

            foreach (var model in models)
            {
                solutionPackage.Models.Add(model);
            }

            // pack your solution into NuGet Package
            var fileName = string.Format("{0}.{1}.nupkg", solutionPackage.Id, solutionPackage.Version);

            var fileDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            var filePath = Path.Combine(fileDir, fileName);

            Directory.CreateDirectory(fileDir);

            packagingService.PackToFile(solutionPackage, filePath);

            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod]
        [TestCategory("Metapack.Samples")]
        public void Can_Package_SPMeta2_Models_To_NuGet_Gallery()
        {
            WithNuGetContext((apiUrl, apiKey, repoUrl) =>
            {
                // you need to apiUrl / apiKey for your NuGet Gallery
                // apiUrl - something like 'https://{your-nuget-gallery}/api/v2'
                // apiKey - a long GUID in your NuGet Gallery profile

                // also, https://www.myget.org might help to get started

                // create package service instance
                var packagingService = new SPMeta2SolutionPackageService();

                // create SPMeta2 solution package
                var solutionPackage = new SPMeta2SolutionPackage();

                solutionPackage.Title = "SPMeta2 CI Package";
                solutionPackage.Id = "SPMeta2.CI";
                solutionPackage.Version = "1.0.0.0";
                solutionPackage.Authors = "SubPoint Solutions";
                solutionPackage.Description = "A test package for SPMeta1 models.";

                // add some models
                var models = new ModelNode[]
                {
                    SPMeta2Model.NewSiteModel(site => { }),
                    SPMeta2Model.NewWebModel(web => { }),
                };

                foreach (var model in models)
                {
                    solutionPackage.Models.Add(model);
                }

                // we increment solution version within current test
                UpdatePackageVersion(solutionPackage);

                // publish your solution into NuGet Gallery
                packagingService.Push(solutionPackage, apiUrl, apiKey);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.Samples")]
        public void Can_Unpack_Package_From_NuGet_Gallery()
        {
            WithNuGetContext((apiUrl, apiKey, repoUrl) =>
            {
                // you need to apiUrl / apiKey for your NuGet Gallery
                // apiUrl - something like 'https://{your-nuget-gallery}/api/v2'
                // apiKey - a long GUID in your NuGet Gallery profile

                // also, https://www.myget.org might help to get started

                // create package service instance
                var packagingService = new SPMeta2SolutionPackageService();

                // create SPMeta2 solution package
                var solutionPackage = new SPMeta2SolutionPackage();

                solutionPackage.Title = "SPMeta2 CI Package";
                solutionPackage.Id = "SPMeta2.CI";
                solutionPackage.Version = "1.0.0.0";
                solutionPackage.Authors = "SubPoint Solutions";
                solutionPackage.Description = "A test package for SPMeta1 models.";

                // add some models
                var models = new ModelNode[]
                {
                    SPMeta2Model.NewSiteModel(site => { }),
                    SPMeta2Model.NewWebModel(web => { }),
                };

                foreach (var model in models)
                {
                    solutionPackage.Models.Add(model);
                }

                // we increment solution version within current test
                UpdatePackageVersion(solutionPackage);

                // publish your solution into NuGet Gallery
                packagingService.Push(solutionPackage, apiUrl, apiKey);

                // get package from the NuGet Gallery
                // we use an extension method to find package, as NuGet Gallery takes time to refresh the cache
                // get the package
                var nuGetGalleryRepository = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
                var nuGetPackage = nuGetGalleryRepository.FindPackageSafe(solutionPackage.Id, new SemanticVersion(solutionPackage.Version));

                // unpacking the solution
                using (var streamReader = nuGetPackage.GetStream())
                {
                    // here is your solution with all the models
                    var unpackedSolutionPackage = packagingService.Unpack(streamReader) as SPMeta2SolutionPackage;

                    // do something with the solution
                    foreach (var model in unpackedSolutionPackage.Models)
                    {
                        // do some stuff with models
                        // deploy the way you like? 
                    }
                }
            });
        }

        [TestMethod]
        [TestCategory("Metapack.Samples")]
        public void Can_Unpack_Package_From_NuGet_Gallery_To_SharePoint()
        {
            // context is an instance of ClientContext
            // use CSOM for both SharePoint Online and SharePoint 2013 

            WithRootSharePointContext(context =>
            {
                WithNuGetContext((apiUrl, apiKey, repoUrl) =>
                {
                    // you need to apiUrl / apiKey for your NuGet Gallery
                    // apiUrl - something like 'https://{your-nuget-gallery}/api/v2'
                    // apiKey - a long GUID in your NuGet Gallery profile

                    // also, https://www.myget.org might help to get started

                    // create package service instance
                    var packagingService = new SPMeta2SolutionPackageService();

                    // create SPMeta2 solution package
                    var solutionPackage = new SPMeta2SolutionPackage();

                    solutionPackage.Title = "SPMeta2 CI Package";
                    solutionPackage.Id = "SPMeta2.CI";
                    solutionPackage.Version = "1.0.0.0";
                    solutionPackage.Authors = "SubPoint Solutions";
                    solutionPackage.Description = "A test package for SPMeta1 models.";

                    // add some models
                    var models = new ModelNode[]
                    {
                        SPMeta2Model.NewSiteModel(site => { }),
                        SPMeta2Model.NewWebModel(web => { }),
                    };

                    foreach (var model in models)
                    {
                        solutionPackage.Models.Add(model);
                    }

                    // we increment solution version within current test
                    UpdatePackageVersion(solutionPackage);

                    // publish your solution into NuGet Gallery
                    packagingService.Push(solutionPackage, apiUrl, apiKey);

                    // get package from the NuGet Gallery
                    // we use an extension method to find package, as NuGet Gallery takes time to refresh the cache
                    // get the package
                    var nuGetGalleryRepository = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
                    var nuGetPackage = nuGetGalleryRepository.FindPackageSafe(solutionPackage.Id,
                        new SemanticVersion(solutionPackage.Version));

                    // create numage package manager within current SharePoint context
                    var packageManager = new SPMeta2SolutionPackageManager(nuGetGalleryRepository, context);

                    // setup provision services
                    packageManager.ProvisionService = new StandardCSOMProvisionService();

                    // but you can also setup SSOM based provision service as following
                    //packageManager.ProvisionService = new StandardSSOMProvisionService();

                    // set the provision host 
                    // CSOM -> ClientContext
                    // SSOM - either SPSite or SPWeb
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

                    // go fo install!
                    packageManager.InstallPackage(nuGetPackage, true, false);
                });
            });
        }

        #endregion
    }
}
