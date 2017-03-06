using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MetaPack.Client.Common.Commands.Base;
using MetaPack.Client.Common.Services;
using MetaPack.Core.Common;
using MetaPack.NuGet.Services;
using Microsoft.SharePoint.Client;
using NuGet;
using MetaPack.Core.Utils;
using MetaPack.NuGet.Common;

namespace MetaPack.Client.Common.Commands
{
    public class DefaultInstallCommand : CommandBase
    {
        #region properties
        public override string Name
        {
            get { return "install"; }
            set
            {

            }
        }


        public string Id { get; set; }
        public string Version { get; set; }

        public bool Force { get; set; }

        #endregion

        #region methods
        public override object Execute()
        {
            if (PackageSources.Count == 0)
                throw new ArgumentException("Source");

            if (string.IsNullOrEmpty(Id))
                throw new ArgumentException("Id");

            if (string.IsNullOrEmpty(Url))
                throw new ArgumentException("Url");

            if (IsSharePointOnline)
            {
                if (string.IsNullOrEmpty(UserName))
                    throw new ArgumentException("UserName");

                if (string.IsNullOrEmpty(UserPassword))
                    throw new ArgumentException("UserPassword");
            }

            var spService = new SharePointService();

            spService.WithSharePointContext(Url,
                        UserName,
                        UserPassword,
                        IsSharePointOnline,
                context =>
                {
                    // connect to remote repo
                    var repo = new AggregateRepository(PackageRepositoryFactory.Default, PackageSources, true);
                    IPackage package = null;

                    if (!string.IsNullOrEmpty(Version))
                    {
                        MetaPackTrace.Info("Fetching package [{0}] with version [{1}]", Id, Version);
                        package = repo.FindPackage(Id, new SemanticVersion(Version));
                    }
                    else
                    {
                        MetaPackTrace.Info("Fetching the latest package [{0}]", Id);
                        package = repo.FindPackage(Id);
                    }

                    if (package == null)
                    {
                        MetaPackTrace.Info("Cannot find package [{0}]. Throwing exception.", Id);
                        throw new ArgumentException("package");
                    }
                    else
                    {

                    }

                    MetaPackTrace.Info("Found package [{0}] version [{1}].",
                            package.Id,
                            package.Version);

                    MetaPackTrace.Info("Installing package to SharePoint web site...");

                    // create manager with repo and current web site
                    var packageManager = CreatePackageManager(repo, context);

                    // install package
                    if (Force)
                    {
                        MetaPackTrace.Info("Force flag is true. Looking for existing package...");

                        var currentPackage = packageManager.LocalRepository.FindPackage(
                            package.Id,
                            package.Version,
                            this.PreRelease,
                            true);

                        if (currentPackage != null)
                        {
                            MetaPackTrace.Info(string.Format(
                                "Package [{0}] version [{1}] already exists. Uninstalling...",
                                currentPackage.Id,
                                currentPackage.Version));

                            packageManager.UninstallPackage(package);

                            // we need a fresh start due to cached nuet packages
                            // TODO - rewrite SharePointCSOMFileSystem to support deletions better
                            packageManager = CreatePackageManager(repo, context);
                        }
                        else
                        {
                            MetaPackTrace.Info(string.Format(
                                "Package [{0}] version [{1}] does not exist. It will be deployed.",
                                package.Id,
                                package.Version));
                        }
                    }
                    else
                    {
                        var currentPackage = packageManager.LocalRepository.FindPackage(
                                                package.Id,
                                                package.Version,
                                                this.PreRelease,
                                                true);

                        if (currentPackage != null)
                        {
                            MetaPackTrace.Info(string.Format(
                                "Package [{0}] version [{1}] already exists. Use --force flag to redeploy it.",
                                currentPackage.Id,
                                currentPackage.Version));

                            return;
                        }
                    }

                    packageManager.InstallPackage(package, false, PreRelease);
                    MetaPackTrace.Info("Completed installation. All good!");
                });

            return null;
        }

        protected virtual MetaPackSolutionPackageManagerBase CreatePackageManager(IPackageRepository repo, ClientContext context)
        {
            var packageManager = new DefaultMetaPackSolutionPackageManager(repo, context);

            // add options
            packageManager.SolutionOptions.Add(new OptionValue
            {
                Name = DefaultOptions.SharePoint.Api.Id,
                Value = this.SharePointApi
            });

            packageManager.SolutionOptions.Add(new OptionValue
            {
                Name = DefaultOptions.SharePoint.Edition.Id,
                Value = this.SharePointEdition
            });

            packageManager.SolutionOptions.Add(new OptionValue
            {
                Name = DefaultOptions.SharePoint.Version.Id,
                Value = this.SharePointVersion
            });

            packageManager.SolutionOptions.Add(new OptionValue
            {
                Name = DefaultOptions.Site.Url.Id,
                Value = context.Url
            });

            packageManager.SolutionOptions.Add(new OptionValue
            {
                Name = DefaultOptions.User.Name.Id,
                Value = UserName
            });

            packageManager.SolutionOptions.Add(new OptionValue
            {
                Name = DefaultOptions.User.Password.Id,
                Value = UserPassword
            });

            if (!string.IsNullOrEmpty(ToolId))
            {
                packageManager.SolutionToolPackage = new SolutionToolPackage
                {
                    Id = ToolId,
                    Version = ToolVersion
                };
            }

            return packageManager;
        }

        #endregion
    }


}
