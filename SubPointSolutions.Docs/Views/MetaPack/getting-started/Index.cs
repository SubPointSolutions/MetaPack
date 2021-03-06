﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Text;
using MetaPack.Core.Common;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using MetaPack.NuGet.Services;
using MetaPack.SharePointPnP.Services;
using MetaPack.SPMeta2.Services;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using NuGet;

namespace SubPointSolutions.Docs.Views.MetaPack
{
    [TestClass]
    public class Index
    {
        [TestMethod]
        [TestCategory("Docs.Basics")]
        public void Create_Package_SPMeta2()
        {
            // Follows NuGet spec design - https://docs.nuget.org/ndocs/schema/nuspec
            // Solution package is a container for SERIALIZED models.
            var solutionPackage = new SolutionPackageBase();

            solutionPackage.Name = "Contoso Intranet SPMeta2 - Site Fields";
            solutionPackage.Title = "Contoso Intranet SPMeta2 - Site Fields";

            solutionPackage.Description = "Contains site level fields for Contoso intranet";
            solutionPackage.Id = "Contoso.Intranet.SiteFields.SPMeta2";
            solutionPackage.Authors = "SubPoint Solutions";
            solutionPackage.Company = "SubPoint Solutions";
            solutionPackage.Version = "1.0.0.0";
            solutionPackage.Owners = "SubPoint Solutions";

            solutionPackage.ReleaseNotes = "Initial set of the site fields for Contoso Intranet";
            solutionPackage.Summary = "All site fields required for Contoso intranet";
            solutionPackage.ProjectUrl = "https://github.com/SubPointSolutions/DefinitelyPacked";
            solutionPackage.IconUrl = "https://github.com/SubPointSolutions/metapack/metapack.png";
            solutionPackage.LicenseUrl = "https://opensource.org/licenses/MIT";

            solutionPackage.Copyright = "All yours";
            solutionPackage.Tags = "MetaPack SPMeta2 SiteFields Taxonomy";

            // here are all your SPMeta2 models
            var models = new List<ModelNode>();

            // create a new ModelContainerBase for every model
            // and then add to solution package
            // you can put "Order" option to control deployment order of the models

            for (var index = 0; index < models.Count; index++)
            {
                var model = models[index];
                var xmlContext = SPMeta2Model.ToXML(model);

                // create ModelContainerBase, put serialized model there
                var modelContainer = new ModelContainerBase
                {
                    Model = Encoding.UTF8.GetBytes(xmlContext),
                };

                // add sort order to control deployment order of the models
                modelContainer.AdditionalOptions.Add(new OptionValue
                {
                    Name = DefaultOptions.Model.Order.Id,
                    Value = index.ToString()
                });

                // add model container to solution
                solutionPackage.AddModel(modelContainer);
            }

            // flag a provider which will be used for solution package deployment
            solutionPackage.AdditionalOptions.Add(new OptionValue
            {
                Name = DefaultOptions.SolutionToolPackage.PackageId.Id,
                Value = "MetaPack.SPMeta2"
            });

            var solutionPackageService = new SPMeta2SolutionPackageService();

            // save your NuGet solution package as stream
            var nuGetPackageStream = solutionPackageService.Pack(solutionPackage, null);

            // or save it straight to file, for instance, on shared folder
            solutionPackageService.PackToFile(solutionPackage, "Contoso.Intranet.SiteFields.SPMeta2.nupkg");

            // or push it straight to NuGet gallery you've got - http://NuGet.org or http://MyGet.org
            // follow instructions on how obtain Url/Key for a specific NuGet Gallery
            var nuGetGallery_ApiUrl = "";
            var nuGetGallery_ApiKey = "";

            solutionPackageService.Push(solutionPackage, nuGetGallery_ApiUrl, nuGetGallery_ApiKey);
        }

        [TestMethod]
        [TestCategory("Docs.Basics")]
        public void Create_Package_PnP()
        {
            // A high level abstraction for solution package.
            // Follows NuGet spec design - https://docs.nuget.org/ndocs/schema/nuspec
            // Solution package is a container for SERIALIZED models.
            // It means that solution package does not depend on a particular API oe assembly so that  models have to be in serialazable, platform and api independent way.

            var solutionPackage = new SolutionPackageBase();

            solutionPackage.Name = "Contoso Intranet PnP - Site Fields";
            solutionPackage.Title = "Contoso Intranet PnP - Site Fields";

            solutionPackage.Description = "Contains site level fields for Contoso intranet";
            solutionPackage.Id = "Contoso.Intranet.SiteFields.PnP";
            solutionPackage.Authors = "SubPoint Solutions";
            solutionPackage.Company = "SubPoint Solutions";
            solutionPackage.Version = "1.0.0.0";
            solutionPackage.Owners = "SubPoint Solutions";

            solutionPackage.ReleaseNotes = "Initial set of the site fields for Contoso Intranet";
            solutionPackage.Summary = "All site fields required for Contoso intranet";
            solutionPackage.ProjectUrl = "https://github.com/SubPointSolutions/DefinitelyPacked";
            solutionPackage.IconUrl = "https://github.com/SubPointSolutions/metapack/metapack.png";
            solutionPackage.LicenseUrl = "https://opensource.org/licenses/MIT";

            solutionPackage.Copyright = "All yours";
            solutionPackage.Tags = "MetaPack SPMeta2 SiteFields Taxonomy";

            // create a new ModelContainerBase for every SharePointPnP solution
            // * currently only OpenXML PnP packages as supported
            // then add to solution package
            // you can put "Order" option to control deployment order of the models

            // all your SharePointPnP OpenXML packages
            var allPnPOpenXmlFiles = new List<string>();

            for (var index = 0; index < allPnPOpenXmlFiles.Count; index++)
            {
                var pnpPackageFilePath = allPnPOpenXmlFiles[index];
                var pnpPackageBytes = System.IO.File.ReadAllBytes(pnpPackageFilePath);

                var modelContainer = new ModelContainerBase
                {
                    Model = pnpPackageBytes
                };

                // add sort order to control deployment order of the models
                modelContainer.AdditionalOptions.Add(new OptionValue
                {
                    Name = DefaultOptions.Model.Order.Id,
                    Value = index.ToString()
                });

                // add type of the SharePointPnP package
                // current only OpenXml is supoported
                modelContainer.AdditionalOptions.Add(new OptionValue
                {
                    Name = DefaultOptions.Model.Type.Id,
                    Value = "SharePointPnP.OpenXml"
                });

                // add model container to solution
                solutionPackage.AddModel(modelContainer);
            }

            // flag a provider which will be used for solution package deployment
            solutionPackage.AdditionalOptions.Add(new OptionValue
            {
                Name = DefaultOptions.SolutionToolPackage.PackageId.Id,
                Value = "MetaPack.SharePointPnP"
            });

            var solutionPackageService = new SharePointPnPSolutionPackageService();

            // save your NuGet solution package as stream
            var nuGetPackageStream = solutionPackageService.Pack(solutionPackage, null);

            // or save it straight to file, for instance, on shared folder
            solutionPackageService.PackToFile(solutionPackage, "Contoso.Intranet.SiteFields.PnP.nupkg");

            // or push it straight to NuGet gallery you've got - http://NuGet.org or http://MyGet.org
            // follow instructions on how obtain Url/Key for a specific NuGet Gallery
            var nuGetGallery_ApiUrl = "";
            var nuGetGallery_ApiKey = "";

            solutionPackageService.Push(solutionPackage, nuGetGallery_ApiUrl, nuGetGallery_ApiKey);
        }

        [TestMethod]
        [TestCategory("Docs.Basics")]
        public void Install_Package()
        {
            // create SharePoint client contex under which MetaPack will be working

            var siteUrl = "http://contoso-intranet.local";

            using (var clientContext = new ClientContext(siteUrl))
            {
                // you can use it with SharePointOnlineCredentials for O365
                // as well as NetworkCredential for SP2013
                // context.Credentials = new NetworkCredential(userName, securePassword);
                // context.Credentials = new SharePointOnlineCredentials(userName, securePassword);


                // create package manager providing client context and NuGet Gallery to work with
                // for instance, as following:
                // nuget.org - https://packages.nuget.org/api/v2
                // myget.org - https://www.myget.org/F/subpointsolutions-staging/api/v2/package
                var nuGetRepository = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
                var packageManager = new DefaultMetaPackSolutionPackageManager(nuGetRepository, clientContext);

                // get your NuGet package by its ID 
                var nuGetPackage = nuGetRepository.FindPackage("Contoso.Intranet.SiteFields.PnP");

                // configure additional options
                // deployment provider such as PnP or SPMeta2 uses these flags to understand:
                // - SharePoint version (SP2013, SP2016, O365
                // - SharePoint API (CSOM, SSOM for SPMeta2 and CSOM for PnP)
                // - SharePoint Edition (Standard / Foundation for SPMeta2)
                var solutionOptions = new List<OptionValue>();

                solutionOptions.Add(DefaultOptions.SharePoint.Api.CSOM);
                solutionOptions.Add(DefaultOptions.SharePoint.Edition.Standard);
                solutionOptions.Add(DefaultOptions.SharePoint.Version.O365);

                // target site Url on which youe solution will be deployed
                solutionOptions.Add(new OptionValue
                {
                    Name = DefaultOptions.Site.Url.Id,
                    Value = siteUrl
                });

                // provide username/password if needed
                // deployment provider will use these in conjunction with CSOM deployment
                // to connect to O365 or local SharePoint via CSOM
                // if SharePoint version was O365, then SharePointOnlineCredentials will be used
                // if SharePoint version was Sp2013/2016, then NetworkCredential will be used
                // if no username/password is provided, then defaultcreds will be used as if you were on SharePoint box itself
                solutionOptions.Add(new OptionValue
                {
                    Name = DefaultOptions.User.Name.Id,
                    Value = "user@contoso.com"
                });

                solutionOptions.Add(new OptionValue
                {
                    Name = DefaultOptions.User.Password.Id,
                    Value = "pass@word1"
                });

                packageManager.SolutionOptions.AddRange(solutionOptions);

                // install package
                // metapack will resolve and install all the things for you
                packageManager.InstallPackage(nuGetPackage, false, false);
            }
        }
    }
}
