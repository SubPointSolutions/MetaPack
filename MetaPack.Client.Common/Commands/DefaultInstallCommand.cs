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
                    MetaPackSolutionPackageManagerBase packageManager = new DefaultMetaPackSolutionPackageManager(repo, context);

                    // TODO, transform command options to SolutionOptions

                    // add options
                    packageManager.SolutionOptions.Add(DefaultOptions.SharePoint.Api.CSOM);
                    packageManager.SolutionOptions.Add(DefaultOptions.SharePoint.Edition.Foundation);
                    packageManager.SolutionOptions.Add(DefaultOptions.SharePoint.Version.O365);

                    packageManager.SolutionOptions.Add(new OptionValue
                    {
                        Name = DefaultOptions.Site.Url.Id,
                        Value = context.Url
                    });

                    if (IsSharePointOnline)
                    {
                        // if o365 - add user name and password
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
                    }

                    if (!string.IsNullOrEmpty(ToolId))
                    {
                        packageManager.SolutionToolPackage = new SolutionToolPackage
                        {
                            Id = ToolId,
                            Version = ToolVersion
                        };
                    }

                    // install package
                    packageManager.InstallPackage(package, false, PreRelease);

                    MetaPackTrace.Info("Completed installation. All good!");
                });

            return null;
        }

        #endregion
    }
}
