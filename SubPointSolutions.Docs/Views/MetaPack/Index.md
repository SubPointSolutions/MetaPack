---
Title: 'MetaPack Home'
Tile: true
TileTitle: 'MetaPack'
TileOrder: 50
TileLink: true
TileLinkOrder: 9
TileDescription: 'The package manager for SPMeta2 based SharePoint customizations.'
---

# MetaPack
The package manager for SharePoint customizations. CI test 6

Deploying, updating and managing SharePoint customizations takes effort. Not only deployment but also versioning, modularization and dependency management require too much effort slowing down development, expanding cost and the delivery dates. Time spent on writing plumbing code, scripts or other means to handle all these areas should be spend on really important things. 

MetaPack introduces a complete solution for packaging, versioning, deploying and updating SPMeta2 based customizations. It is built on top of NuGet platform and offers a smooth API for developers, a CLI for better CI/CD scenarios and a handy GUI app for IT-pros.

Here is how we see it working - develop model in Visual Studio, package and push to NuGet, consume via API later on
![MetaPack Vision](https://subpointsolutions-dev.netlify.com/content/img/products/metapack/metapack-vision.png)

[Link to the full image](https://subpointsolutions-dev.netlify.com/content/img/products/metapack/metapack-vision.png)

### Build status
[![Build status](https://ci.appveyor.com/api/projects/status/1weq7g33dfp3xi6i?svg=true)](https://ci.appveyor.com/project/SubPointSupport/metapack)

### MetaPack in details

#### Introduces solution packaging
Packaging SharePoint customization is never an easy task. Should it be console app? Should it be provider hosted app? A PowerShell script?

Don't worry. MetaPack packages your solution as a self-contained NuGet package. As simple as that.

#### Handles solution versioning
Version history is another pain point while delivering SharePoint customizations. It is not easy to keep track of all customizations deployed not to talk about versioning them.

MetaPack uses NuGet Gallery infrastructure to provide solution version tracking. Semantic versioning naturally comes along.

#### Makes dependency management possible
Did you ever want to reuse and modularize your customizations so that you can compose bigger building blocks? We know it's hard to implement.

MetaPack brings the best of NuGet platform: package dependency management, versioning and easy modularization.

#### Simplifies deployment and updates
Solution life-cycle does not end with the first deployment. New features are built, new versions are released so that a smooth update process is a must.

MetaPack offers the best experience ever to deploy and update your models. It handles all the details and even shows if updates are available.


#### Offers API, a CLI and user friendly GUI
Modern software development blurs the boundaries between developers, IT-pros and business. Team needs to work closely having a solid, smooth delivery workflow.

MetaPack offers ultimate experience for all team: developers leverage API, IT-pros have a CLI and business have a friendly GUI based application.

#### Improves CI/CD story
Ultimately, MetaPack not only helps to ship SharePoint customizations to the client but also helps to improve continuous integration and deployment story.

Create SPMeta2 models in Visual Studios, use API to create NuGet packages, ship them the way you like: API, CLI or GUI - it's all yours.

As for the API, that's how you can push your SPMeta2 models into NuGet Gallery: 
```cs
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
```

Next, you can get package from the NuGet Gallery and unpack it:
```cs
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

```

But why not to deploy your solution straight to SharePoint? Too easy!
```cs
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

```

We have more samples as unit tests here, check them out:
* [MetaPack API Samples](https://github.com/SubPointSolutions/MetaPack/blob/dev/MetaPack.Tests/Scenarios/ApiSamplesTests.cs)

#### Feature requests, support and contributions
In case you have unexpected issues or keen to see new features please contact support on SPMeta2 Yammer or here at github:

* [https://www.yammer.com/spmeta2feedback](https://www.yammer.com/spmeta2feedback/#/threads/inGroup?type=in_group&feedId=7897894&view=all)
