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
using MetaPack.Core.Consts;
using MetaPack.Core.Utils;

namespace MetaPack.SPMeta2.Services
{
    /// <summary>
    /// Solution packaging service implementation for SPMeta2 models
    /// </summary>
    public class SPMeta2SolutionPackageService : NuGetSolutionPackageService
    {
        #region contructors
        public SPMeta2SolutionPackageService()
        {
            ModelFoldersPath = "Models";
        }

        #endregion

        #region properties

        public string ModelFoldersPath { get; set; }

        #endregion

        #region methods

        public override Stream Pack(SolutionPackageBase package, SolutionPackageOptions options)
        {
            MetaPackTrace.Verbose("Packing solution package...");

            var typedPackage = package as SPMeta2SolutionPackage;

            if (typedPackage == null)
                throw new ArgumentException(string.Format("package must be of type: [{0}]", typeof(SPMeta2SolutionPackage)));

            // create result stream and NuGet package
            var resultStream = new MemoryStream();
            var metadata = GetManifestMetadata(package);

            var nugetPackageBuilder = new PackageBuilder();

            var solutionPackageManifestFile = new ManifestFile();
            var solutionFilePath = SaveMetaPackSolutionFile<SPMeta2SolutionPackage>(solutionPackageManifestFile, typedPackage);

            nugetPackageBuilder.PopulateFiles("", new[] { solutionPackageManifestFile });
            nugetPackageBuilder.Populate(metadata);

            // add models folders
            foreach (var srcFolder in typedPackage.ModelFolders)
                AddFolderToPackage(nugetPackageBuilder, srcFolder, ModelFoldersPath);

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
            SerializationService.RegisterKnownType(typeof(SPMeta2SolutionPackage));

            using (var streamReader = new StreamReader(solutionPackageFile.GetStream()))
            {
                var solutionFileContent = streamReader.ReadToEnd();
                var typedPackage = SerializationService.Deserialize(typeof(SPMeta2SolutionPackage), solutionFileContent) as SPMeta2SolutionPackage;

                // unpack models
                UnpackFoldersPackage(zipPackage, ModelFoldersPath, typedPackage.ModelFolders);

                return typedPackage;
            }
        }

        #endregion
    }
}
