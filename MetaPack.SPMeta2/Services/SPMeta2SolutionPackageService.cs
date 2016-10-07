using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using MetaPack.NuGet.Services;
using NuGet;

namespace MetaPack.SPMeta2.Services
{
    public class SPMeta2SolutionPackageService : NuGetSolutionPackageService
    {
        #region contructors
        public SPMeta2SolutionPackageService()
        {

        }

        #endregion

        #region methods

        public override Stream Pack(SolutionPackageBase package, SolutionPackageOptions options)
        {
            var result = new MemoryStream();
            var hasResult = false;

            var metadata = new ManifestMetadata()
            {
                Title = package.Title,
                Description = package.Description,
                Id = package.Id,
                Authors = package.Authors,

                Version = package.Version,
                Owners = package.Owners,

                ReleaseNotes = package.ReleaseNotes,
                Summary = package.Summary,

                ProjectUrl = package.ProjectUrl,
                IconUrl = package.IconUrl,
                LicenseUrl = package.LicenseUrl,
                Copyright = package.Copyright,
                Tags = package.Tags
            };

            if (package.Dependencies.Any())
            {
                if (metadata.DependencySets == null)
                    metadata.DependencySets = new List<ManifestDependencySet>();

                var dependencySet = new ManifestDependencySet
                {
                    Dependencies = new List<ManifestDependency>()
                };

                foreach (var dependency in package.Dependencies)
                {
                    dependencySet.Dependencies.Add(new ManifestDependency
                    {
                        Id = dependency.Id,
                        Version = dependency.Version
                    });
                }

                metadata.DependencySets.Add(dependencySet);
            }

            var builder = new PackageBuilder();

            var solutionPackageManifestFile = new ManifestFile();

            var solutionFileContent = SerializationService.SerializeSolutionPackage(package);
            var solutionFilePath = Path.Combine(Path.GetTempPath(),
                                                string.Format("{0}.xml", Guid.NewGuid().ToString("N")));

            solutionPackageManifestFile.Source = solutionFilePath;
            solutionPackageManifestFile.Target = "SolutionPackage.xml";

            try
            {
                File.WriteAllText(solutionFilePath, solutionFileContent);

                builder.PopulateFiles("", new[] { solutionPackageManifestFile });
                builder.Populate(metadata);

                builder.Save(result);

                result.Position = 0;

                hasResult = true;

                return result;
            }
            finally
            {
                if (!hasResult)
                {
                    try
                    {
                        if (result != null)
                        {
                            result.Dispose();
                            result = null;
                        }
                    }
                    catch
                    {

                    }
                }

                if (File.Exists(solutionFilePath))
                {
                    try
                    {
                        File.Delete(solutionFilePath);
                    }
                    catch { }
                }
            }
        }

        public override SolutionPackageBase Unpack(Stream package, SolutionPackageOptions options)
        {
            SolutionPackageBase result = null;

            package.Position = 0;
            var packageReader = new ZipPackage(package);

            var files = packageReader.GetFiles();
            var solutionPackageFile = files.FirstOrDefault(f => f.Path.ToUpper() == "SOLUTIONPACKAGE.XML");

            if (solutionPackageFile == null)
            {
                throw new ArgumentNullException(
                    string.Format("Cannot find SolutionPackage.xml in the provided NuGet package"));
            }

            using (var streamReader = new StreamReader(solutionPackageFile.GetStream()))
            {
                var solutionFileContent = streamReader.ReadToEnd();
                var solutionPackage = SerializationService.DeserializeSolutionPackage(solutionFileContent);

                return solutionPackage;
            }
        }


        #endregion
    }
}
