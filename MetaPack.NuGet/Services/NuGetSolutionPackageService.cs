using System;
using System.IO;
using System.Linq;
using MetaPack.Core;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using NuGet;

namespace MetaPack.NuGet.Services
{
    public abstract class NuGetSolutionPackageService : SolutionPackageServiceBase
    {
        #region constructors

        public NuGetSolutionPackageService()
        {
            SerializationService = new DefaultXMLSerializationService();
        }

        protected DefaultXMLSerializationService SerializationService { get; set; }

        #endregion

        public virtual void Push(SolutionPackageBase package, string apiUrl, string apiKey)
        {
            Push(package, apiUrl, apiKey, 2 * 60 * 1000, false);
        }

        public virtual void Push(Stream package, string apiUrl, string apiKey)
        {
            Push(package, apiUrl, apiKey, 2 * 60 * 1000, false);
        }

        public virtual void Push(SolutionPackageBase package, string apiUrl, string apiKey,
            int timeoutInMilliseconds,
            bool disableBuffering)
        {
            var packageFileFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            var packageFilePath = Path.Combine(packageFileFolder,
                                               string.Format("{0}.{1}.nupkg", package.Id, package.Version));

            Directory.CreateDirectory(packageFileFolder);

            try
            {
                this.PackToFile(package, packageFilePath);

                var packageFile = new FileInfo(packageFilePath);
                var packageSize = packageFile.Length;

                var nuGetPackage = new ZipPackage(packageFilePath);

                Push(nuGetPackage, packageSize, apiUrl, apiKey, timeoutInMilliseconds, disableBuffering);

            }
            finally
            {
                if (File.Exists(packageFilePath))
                {
                    try
                    {
                        File.Delete(packageFilePath);
                    }
                    catch
                    {

                    }
                }
            }
        }

        public virtual void Push(Stream package, string apiUrl, string apiKey,
            int timeoutInMilliseconds,
            bool disableBuffering)
        {
            var packageSize = package.Length;
            var nuGetPackage = new ZipPackage(package);

            Push(nuGetPackage, packageSize, apiUrl, apiKey, timeoutInMilliseconds, disableBuffering);
        }

        protected virtual void Push(ZipPackage package, long packageSize,
            string apiUrl, string apiKey,
            int timeoutInMilliseconds,
            bool disableBuffering)
        {
            var ps = new PackageServer(apiUrl, "SPMeta2 NuGet Packaging API");
            ps.PushPackage(apiKey, package, packageSize, timeoutInMilliseconds, disableBuffering);
        }
    }
}
