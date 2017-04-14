using System;
using System.Diagnostics;
using System.Linq;
using MetaPack.Client.Common.Commands.Base;
using MetaPack.Client.Common.Services;
using MetaPack.NuGet.Services;
using Microsoft.SharePoint.Client;
using NuGet;
using System.Collections.Generic;

namespace MetaPack.Client.Common.Commands
{
    public class NuGetListCommand : CommandBase
    {
        #region constructors

        public NuGetListCommand()
        {

        }

        #endregion


        #region properties
        public override string Name
        {
            get { return "list"; }
            set
            {

            }
        }

        private List<IPackage> _packages = new List<IPackage>();

        public IEnumerable<IPackage> Packages
        {
            get
            {
                return _packages;
            }
        }

        #endregion

        #region methods
        public override object Execute()
        {
            WithEmitingTraceEvents(InternalExecute);

            return null;
        }

        private void InternalExecute()
        {
            _packages.Clear();

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
                    var repoFolder = AppDomain.CurrentDomain.BaseDirectory;
                    var repo = PackageRepositoryFactory.Default.CreateRepository(repoFolder);

                    // create manager with empty repo to avoid connectivity
                    var packageManager = new DefaultMetaPackSolutionPackageManager(repo, context);

                    //var packages = packageManager.LocalRepository.GetPackages();
                    var packages = packageManager.LocalRepository.Search(
                            string.Empty,
                            Enumerable.Empty<string>(),
                            this.PreRelease);

                    packages = packages.GroupBy(p => p.Id)
                        .Select(g => g.OrderByDescending(p => p.Version).FirstOrDefault());

                    packages = packages.Where(p => p != null);

                    foreach (var package in packages)
                    {
                        _packages.Add(package);
                        Out.WriteLine(package.GetFullName());
                    }
                });
        }

        #endregion
    }
}
