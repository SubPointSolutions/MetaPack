using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet;

namespace MetaPack.Client.Desktop.Impl.ViewModels
{
    public class NuGetPackageViewModel
    {
        public NuGetPackageViewModel()
        {
        }

        public NuGetPackageViewModel(IPackage package)
        {
            Package = package;
        }

        public string Id
        {
            get { return Package.Id; }
        }

        public string Title
        {
            get { return Package.Title; }
        }

        public string Authors
        {
            get
            {
                if (Package.Authors != null)
                    return string.Join(", ", Package.Authors);

                return string.Empty;
            }
        }

        public string Version
        {
            get { return Package.Version.ToString(); }
        }

        public IPackage Package { get; set; }

        public int Downloads
        {
            get { return Package.DownloadCount; }
            set { }
        }

        public int TotalDownloads
        {
            get
            {
                return Package.DownloadCount;
            }
            set { }
        }

        public string Created
        {
            get
            {
                if (Package.Published.HasValue)
                    return Package.Published.Value.Date.ToShortDateString();

                return string.Empty;
            }
            set { }
        }

        public string Description
        {
            get
            {
                return Package.Description;
            }
            set { }
        }

        public string LastUpdated
        {
            get
            {
                if (Package.Published.HasValue)
                    return Package.Published.Value.Date.ToShortDateString();

                return string.Empty;
            }
            set { }
        }

        public string PackageSize
        {
            get
            {
                var result = String.Empty;

                using (var stream = Package.GetStream())
                {
                    result = SizeSuffix(stream.Length);
                }

                return result;

            }
            set { }
        }

        public string Dependencies
        {
            get
            {
                var packageDeps = new List<PackageDependency>();
                var deps = Package.DependencySets;

                foreach (var dep in deps)
                    foreach (var package in dep.Dependencies)
                        packageDeps.Add(package);

                return string.Join(", ", packageDeps.Select(d => string.Format("{0} {1}", d.Id, d.VersionSpec)));
            }
            set { }
        }

        public string ReleaseNotes
        {
            get { return Package.ReleaseNotes; }
            set { }
        }

        public string ProjectSite
        {
            get
            {
                if (Package.ProjectUrl != null)
                    return Package.ProjectUrl.ToString();

                return string.Empty;
            }
            set { }
        }

        public string License
        {
            get
            {
                if (Package.LicenseUrl != null)
                    return Package.LicenseUrl.ToString();

                return string.Empty;
            }
            set { }
        }

        public string Gallery
        {
            get
            {
                // TODO

                if (Package.ProjectUrl != null)
                    return Package.ProjectUrl.ToString();

                return string.Empty;
            }
            set { }
        }

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        static string SizeSuffix(long value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }
    }
}
