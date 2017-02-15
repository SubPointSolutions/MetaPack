using System;
using System.Diagnostics;
using System.Linq;
using MetaPack.Client.Common.Commands.Base;
using MetaPack.Client.Common.Services;
using MetaPack.NuGet.Services;
using MetaPack.SPMeta2.Services;
using Microsoft.SharePoint.Client;
using NuGet;

namespace MetaPack.Client.Common.Commands
{
    public class NuGetListCommand : CommandBase
    {

        #region properties
        public override string Name
        {
            get { return "list"; }
            set
            {

            }
        }
        #endregion

        #region methods
        public override object Execute()
        {
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
                            false);

                    packages = packages.GroupBy(p => p.Id)
                        .Select(g => g.OrderByDescending(p => p.Version).FirstOrDefault());

                    packages = packages.Where(p => p != null);

                    foreach (var package in packages)
                    {
                        Console.WriteLine(package.GetFullName());
                        Trace.WriteLine(package.GetFullName());
                    }
                });

            return null;
        }

        #endregion
    }
}
