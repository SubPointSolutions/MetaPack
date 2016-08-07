using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet;
using System.Threading;

namespace MetaPack.Tests.Extensions
{
    public static class IPackageRepositoryExtensions
    {

        public static IPackage FindPackageSafe(this IPackageRepository repo,
            string packageId)
        {
            return FindPackageSafe(repo, packageId, null);
        }

        public static IPackage FindPackageSafe(this IPackageRepository repo,
            string packageId, SemanticVersion version)
        {
            var maxTimeoutInMilliSeconds = 120 * 1000;
            var currentTimeoutInMilliSeconds = 0;

            var queryIntervalInMilliSeconds = 5 * 1000;

            IPackage package = null;

            while (package == null && currentTimeoutInMilliSeconds < maxTimeoutInMilliSeconds)
            {
                if (version == null)
                    package = repo.FindPackage(packageId);
                else
                    package = repo.FindPackage(packageId, version);

                currentTimeoutInMilliSeconds += queryIntervalInMilliSeconds;

                Thread.Sleep(queryIntervalInMilliSeconds);
            }

            if (currentTimeoutInMilliSeconds > maxTimeoutInMilliSeconds)
            {
                throw new Exception(
                    string.Format("Cannot find package by ID:[{0}] and Version:[{1}] within [{2}] milliseconds.",
                        packageId, version, maxTimeoutInMilliSeconds));
            }

            return package;
        }
    }
}
