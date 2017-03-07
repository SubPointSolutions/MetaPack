using System;
using System.Collections.Generic;
using System.Text;
using MetaPack.Core.Common;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using MetaPack.SPMeta2.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SubPointSolutions.Docs.Views.MetaPack
{
    [TestClass]
    public class Index
    {
        [TestMethod]
        [TestCategory("Docs.Basics")]
        public void Create_Package_SPMeta2()
        {
            // A high level abstraction for solution package.
            // Follows NuGet spec design - https://docs.nuget.org/ndocs/schema/nuspec
            // Solution package is a container for SERIALIZED models.
            // It means that solution package does not depend on a particular API oe assembly so that  models have to be in serialazable, platform and api independent way.

            var solutionPackage = new SolutionPackageBase();

            solutionPackage.Name = "Contoso Intranet - Site Fields";
            solutionPackage.Title = "Contoso Intranet - Site Fields";

            solutionPackage.Description = "Contains site level fields for Contoso intranet";
            solutionPackage.Id = "Contoso.Intranet.SiteFields";
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

                // add model cintaner to solution
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
            solutionPackageService.PackToFile(solutionPackage, "Contoso.Intranet.SiteFields.nupkg");

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
            Console.WriteLine("PnP 2");
        }
    }
}
