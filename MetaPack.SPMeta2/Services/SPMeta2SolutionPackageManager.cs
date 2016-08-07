using System;
using System.Linq;
using System.Reflection;
using MetaPack.NuGet.Services;
using Microsoft.SharePoint.Client;
using NuGet;
using SPMeta2.Definitions;
using SPMeta2.ModelHosts;
using SPMeta2.Services;

namespace MetaPack.SPMeta2.Services
{
    public class SPMeta2SolutionPackageManager : MetaPackSolutionPackageManager
    {
        #region constructors
        public SPMeta2SolutionPackageManager(IPackageRepository sourceRepository, ClientContext context) : base(sourceRepository, context)
        {
            InitProvisionEvents();
        }

        public SPMeta2SolutionPackageManager(IPackageRepository sourceRepository, IPackagePathResolver pathResolver, IFileSystem fileSystem) : base(sourceRepository, pathResolver, fileSystem)
        {
            InitProvisionEvents();
        }

        public SPMeta2SolutionPackageManager(IPackageRepository sourceRepository, IPackagePathResolver pathResolver, IFileSystem fileSystem, IPackageRepository localRepository) : base(sourceRepository, pathResolver, fileSystem, localRepository)
        {
            InitProvisionEvents();
        }

        #endregion

        #region properties

        public ProvisionServiceBase ProvisionService { get; set; }

        public object ProvisionServiceHost { get; set; }

        #endregion

        #region methods

        private void InitProvisionEvents()
        {
            this.PackageInstalling += OnPackageInstalling;
        }

        protected virtual void OnPackageInstalling(object sender, PackageOperationEventArgs e)
        {
            // unsuccessful provision would raise an exceptino failing the whole package 

            if (ProvisionService == null)
                throw new ArgumentNullException("ProvisionService");

            if (ProvisionServiceHost == null)
                throw new ArgumentNullException("ProvisionServiceHost");

            var isCSOMService = ProvisionService.GetType().Name.Contains("CSOM");

            // unpack solution package
            var solutionPackageService = new SPMeta2SolutionPackageService();
            var solutionPackage = solutionPackageService.Unpack(e.Package.GetStream(), null) as SPMeta2SolutionPackage;

            Assembly baseAssembly = null;

            if (isCSOMService)
            {
                baseAssembly = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .FirstOrDefault(a => a.FullName.Contains("SPMeta2.CSOM,"));
            }
            else
            {
                baseAssembly = AppDomain.CurrentDomain
                  .GetAssemblies()
                  .FirstOrDefault(a => a.FullName.Contains("SPMeta2.CSOM,"));
            }

            var siteModelHostType = baseAssembly.GetTypes().FirstOrDefault(t => t.Name == "SiteModelHost");
            var webModelHostType = baseAssembly.GetTypes().FirstOrDefault(t => t.Name == "WebModelHost");

            foreach (var model in solutionPackage.Models)
            {
                var isSiteModel = model.Value is SiteDefinition;
                var isWebModel = model.Value is WebDefinition;

                if (!isSiteModel && !isWebModel)
                {
                    throw new ArgumentException(
                        string.Format(
                            "Model with root type [{0}] is unsupported. Onlysite and web models are supported.",
                            model.Value.GetType().Name));
                }

                ModelHostBase modelHostInstance = null;

                if (isSiteModel)
                    modelHostInstance = Activator.CreateInstance(siteModelHostType, ProvisionServiceHost) as ModelHostBase;
                else
                    modelHostInstance = Activator.CreateInstance(webModelHostType, ProvisionServiceHost) as ModelHostBase;

                if (modelHostInstance == null)
                {
                    throw new ArgumentNullException("modelHostInstance");
                }

                ProvisionService.DeployModel(modelHostInstance, model);
            }
        }

        #endregion
    }
}
