using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MetaPack.Core;
using MetaPack.Core.Common;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using NuGet;
using MetaPack.Core.Consts;
using MetaPack.Core.Utils;

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

        #region methods

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
            var ps = new PackageServer(apiUrl, "MetaPack Packaging API");
            ps.PushPackage(apiKey, package, packageSize, timeoutInMilliseconds, disableBuffering);
        }

        #endregion

        #region protected

        protected virtual void AddFolderToPackage(PackageBuilder nugetPackageBuilder, string srcFolder, string dstFolder)
        {
            var folderName = new DirectoryInfo(srcFolder).Name;

            var files = Directory.GetFiles(srcFolder, "*.*");
            var manifestFiles = files.Select(f => new ManifestFile
            {
                Source = f,
                Target = dstFolder + "/" + folderName + "/" + Path.GetFileName(f)
            });

            nugetPackageBuilder.PopulateFiles("", manifestFiles);
        }

        protected virtual void UnpackFoldersPackage(IPackage nuGetPackage, string srcFolderPath, List<string> dstFolders)
        {
            var tmpDir = GetTempFolderPath();
            var templateDirs = new List<string>();

            var files = nuGetPackage.GetFiles();
            var templateFolderFiles = files.Where(f => f.Path.ToUpper().StartsWith(srcFolderPath.ToUpper()));

            foreach (var templateFile in templateFolderFiles)
            {
                var filePartPath = templateFile.Path.Replace(srcFolderPath, string.Empty).Replace(@"\\", @"\").Replace((char)92, '/').Trim('/');
                var filePath = Path.Combine(tmpDir, filePartPath);
                var dirPath = Path.GetDirectoryName(filePath);

                var dirInfo = Directory.CreateDirectory(dirPath);

                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    templateFile.GetStream().CopyTo(fileStream);
                }

                templateDirs.Add(dirPath);
            }

            templateDirs = templateDirs.Distinct().ToList();
            dstFolders.AddRange(templateDirs);
        }

        protected virtual ManifestMetadata GetManifestMetadata(SolutionPackageBase solution)
        {
            return GetManifestMetadata<SolutionPackageBase>(solution, null);
        }

        protected virtual ManifestMetadata GetManifestMetadata<TSolutionPackage>(SolutionPackageBase package, Action<TSolutionPackage, ManifestMetadata> action)
            where TSolutionPackage : SolutionPackageBase
        {
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

            if (action != null && package is TSolutionPackage)
            {
                action(package as TSolutionPackage, metadata);
            }

            return metadata;
        }

        protected virtual string GetTempFolderPath()
        {
            return GetTempFolderPath(true);
        }

        protected virtual string GetTempFolderPath(bool ensureFolder)
        {
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));

            if (ensureFolder)
                Directory.CreateDirectory(path);

            return path;
        }

        protected virtual string GetTempXmlFileName()
        {
            return string.Format("{0}.xml", Guid.NewGuid().ToString("N"));
        }

        protected virtual string GetTempXmlFilePath()
        {
            return Path.Combine(GetTempFolderPath(), GetTempXmlFileName());
        }

        protected virtual string SaveMetaPackSolutionFile<TSolutionPackage>(ManifestFile manifestFile, TSolutionPackage package)
            where TSolutionPackage : SolutionPackageBase
        {
            SerializationService.RegisterKnownType(typeof(SolutionPackageBase));
            SerializationService.RegisterKnownType(typeof(TSolutionPackage));

            var solutionFileContent = SerializationService.Serialize(package);
            var solutionFilePath = GetTempXmlFilePath();

            manifestFile.Source = solutionFilePath;
            manifestFile.Target = MetaPackConsts.SolutionFileName;

            File.WriteAllText(solutionFilePath, solutionFileContent);

            return solutionFilePath;
        }

        protected virtual ManifestFile GetModelManifestFile(ModelContainerBase model)
        {
            var result = new ManifestFile();

            SerializationService.RegisterKnownType(typeof(ModelContainerBase));
            SerializationService.RegisterKnownType(typeof(OptionValue));

            var fileContent = SerializationService.Serialize(model);
            var filePath = GetTempXmlFilePath();

            result.Source = filePath;
            result.Target = MetaPackConsts.ModelsContainerFolder + "/" + Path.GetFileName(filePath);

            File.WriteAllText(filePath, fileContent);

            return result;
        }

        public IPackageFile FindSolutionPackageFile(IPackage package)
        {
            var solutionPackageFile = package.GetFiles().FirstOrDefault(f => f.Path.ToUpper() == MetaPackConsts.SolutionFileName.ToUpper());

            if (solutionPackageFile == null)
                throw new ArgumentNullException(string.Format("Cannot find SolutionPackage.xml in the provided NuGet package"));

            return solutionPackageFile;
        }

        #endregion

        #region pacjing

        public override Stream Pack(SolutionPackageBase package, SolutionPackageOptions options)
        {
            var typedPackage = package;

            MetaPackTrace.Verbose("Packing solution package...");

            // create result stream and NuGet package
            var resultStream = new MemoryStream();
            var metadata = GetManifestMetadata(package);

            var nugetPackageBuilder = new PackageBuilder();

            var solutionPackageManifestFile = new ManifestFile();
            var solutionFilePath = SaveMetaPackSolutionFile<SolutionPackageBase>(solutionPackageManifestFile, typedPackage);

            nugetPackageBuilder.PopulateFiles("", new[] { solutionPackageManifestFile });
            nugetPackageBuilder.Populate(metadata);

            // TODO
            // implement container persistance

            var models = package.GetModels();

            foreach (var model in models)
            {
                var modelManifestFile = GetModelManifestFile(model);
                nugetPackageBuilder.PopulateFiles("", new[] { modelManifestFile });
            }

            // save nuget package into the final stream
            nugetPackageBuilder.Save(resultStream);

            resultStream.Position = 0;

            MetaPackTrace.Verbose("Packing solution package completed...");

            return resultStream;
        }

        public override SolutionPackageBase Unpack(Stream package, SolutionPackageOptions options)
        {
            package.Position = 0;

            var zipPackage = new ZipPackage(package);
            var solutionPackageFile = FindSolutionPackageFile(zipPackage);

            SerializationService.RegisterKnownType(typeof(SolutionPackageBase));
            //SerializationService.RegisterKnownType(typeof(SPMeta2SolutionPackage));

            using (var streamReader = new StreamReader(solutionPackageFile.GetStream()))
            {
                var solutionFileContent = streamReader.ReadToEnd();

                var solutionPackage = SerializationService.Deserialize(typeof(SolutionPackageBase), solutionFileContent) as SolutionPackageBase;
                var modelContainerFiles = zipPackage.GetFiles(MetaPackConsts.ModelsContainerFolder);

                foreach (var modelContainerFile in modelContainerFiles)
                {
                    using (var modelContainerStream = modelContainerFile.GetStream())
                    {
                        var modelContainerBinary = modelContainerStream.ReadToEnd();

                        var modelContainer = SerializationService.Deserialize(typeof(ModelContainerBase), modelContainerBinary) as ModelContainerBase;
                        solutionPackage.AddModel(modelContainer);
                    }
                }

                return solutionPackage;
            }
        }

        #endregion
    }
}
