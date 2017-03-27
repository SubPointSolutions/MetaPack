using System;
using System.Diagnostics;
using System.Linq;
using MetaPack.Client.Common.Commands.Base;
using MetaPack.Client.Common.Services;
using MetaPack.NuGet.Services;
using Microsoft.SharePoint.Client;
using NuGet;

namespace MetaPack.Client.Common.Commands
{
    public class DefaultUpdateCommand : CommandBase
    {
        #region properties
        public override string Name
        {
            get { return "update"; }
            set
            {

            }
        }

        public string Source { get; set; }
        public string Id { get; set; }
        public string Version { get; set; }

        public bool Force { get; set; }

        #endregion

        #region methods
        public override object Execute()
        {
            WithEmitingTraceEvents(InternalExecute);
            return null;
        }

        private void InternalExecute()
        {
            if (string.IsNullOrEmpty(Source))
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
                    Out.WriteLine("Connecting to NuGet repository:[{0}]", Source);
                    var repo = PackageRepositoryFactory.Default.CreateRepository(Source);
                    IPackage package = null;

                    if (!string.IsNullOrEmpty(Version))
                    {
                        Out.WriteLine("Fetching package [{0}] with version [{1}]", Id, Version);
                        package = repo.FindPackage(Id, new SemanticVersion(Version));
                    }
                    else
                    {
                        Out.WriteLine("Fetching the latest package [{0}]", Id);
                        package = repo.FindPackage(Id);
                    }

                    if (package == null)
                    {
                        Out.WriteLine("Cannot find package [{0}]. Throwing exception.", Id);
                        throw new ArgumentException("package");
                    }
                    else
                    {
                        Out.WriteLine("Found remote package [{0}].", package.GetFullName());
                    }

                    Out.WriteLine("Checking local package [{0}]", Id);
                    // create manager with repo and current web site
                    var packageManager = new DefaultMetaPackSolutionPackageManager(repo, context);

                    //var localPackage = packageManager.LocalRepository.FindPackage(package.Id, package.Version, true, true);
                    var localPackages = packageManager.LocalRepository.FindPackagesById(package.Id);
                    var localPackage = localPackages.OrderByDescending(p => p.Version)
                        .FirstOrDefault();

                    if (localPackage != null)
                    {
                        Out.WriteLine("Found local package [{0}] with version [{1}]",
                            localPackage.Id,
                            localPackage.Version);
                    }

                    if (localPackage == null)
                    {
                        Out.WriteLine("Cannot find local package. Performing install...");
                    }
                    else
                    {
                        if (localPackage.Version < package.Version)
                        {
                            Out.WriteLine("Local package version is behind remote one: [{0}] < [{1}]",
                                    localPackage.Version, package.Version);
                        }
                        else
                        {
                            Out.WriteLine("Local package version greater or equal remote one: [{0}] >= [{1}]. No update is required.",
                                    localPackage.Version, package.Version);
                            return;
                        }
                    }

                    Out.WriteLine("Installing package [{0}] to SharePoint web site...", package.GetFullName());


                    // install package
                    packageManager.InstallPackage(package, false, PreRelease);

                    Out.WriteLine("Completed installation. All good!");
                });
        }

        #endregion
    }
}
