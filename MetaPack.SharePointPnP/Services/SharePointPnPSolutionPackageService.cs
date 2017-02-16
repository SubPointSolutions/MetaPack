using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Consts;
using MetaPack.Core.Packaging;
using MetaPack.NuGet.Services;
using NuGet;

namespace MetaPack.SharePointPnP.Services
{
    /// <summary>
    /// Solution packaging service implementation for PnP models
    /// </summary>
    public class SharePointPnPSolutionPackageService : NuGetSolutionPackageService
    {
        #region constructors

        public SharePointPnPSolutionPackageService()
        {
            TemplateFoldersPath = "TemplateFolders";
            TemplateOpenXmlFoldersPath = "TemplateOpenXmlFolders";
        }

        #endregion

        #region properties

        public string TemplateFoldersPath { get; set; }
        public string TemplateOpenXmlFoldersPath { get; set; }

        #endregion

        #region methods

        public override Stream Pack(SolutionPackageBase package, SolutionPackageOptions options)
        {
            var typedPackage = package as SharePointPnPSolutionPackage;

            if (typedPackage == null)
                throw new ArgumentNullException(string.Format("package must be of type: [{0}]", typeof(SharePointPnPSolutionPackage)));

            // create result stream and NuGet package
            var resultStream = new MemoryStream();
            var metadata = GetManifestMetadata(package);

            var nugetPackageBuilder = new PackageBuilder();

            var solutionPackageManifestFile = new ManifestFile();
            var solutionFilePath = SaveMetaPackSolutionFile<SharePointPnPSolutionPackage>(solutionPackageManifestFile, typedPackage);

            nugetPackageBuilder.PopulateFiles("", new[] { solutionPackageManifestFile });
            nugetPackageBuilder.Populate(metadata);

            // add PnP folder-based tenmplates
            foreach (var srcFolder in typedPackage.ProvisioningTemplateFolders)
                AddFolderToPackage(nugetPackageBuilder, srcFolder, TemplateFoldersPath);

            // add PnP OpenXml packages
            foreach (var srcFolder in typedPackage.ProvisioningTemplateOpenXmlPackageFolders)
                AddFolderToPackage(nugetPackageBuilder, srcFolder, this.TemplateOpenXmlFoldersPath);

            nugetPackageBuilder.Save(resultStream);

            resultStream.Position = 0;

            return resultStream;
        }

        public override SolutionPackageBase Unpack(Stream package, SolutionPackageOptions options)
        {
            package.Position = 0;

            var zipPackage = new ZipPackage(package);
            var solutionPackageFile = FindSolutionPackageFile(zipPackage);

            SerializationService.RegisterKnownType(typeof(SolutionPackageBase));
            SerializationService.RegisterKnownType(typeof(SharePointPnPSolutionPackage));

            using (var streamReader = new StreamReader(solutionPackageFile.GetStream()))
            {
                var solutionFileContent = streamReader.ReadToEnd();
                var typedPackage = SerializationService.Deserialize(
                                    typeof(SharePointPnPSolutionPackage), solutionFileContent)
                                    as SharePointPnPSolutionPackage;

                // unpack PnP folder-based tenmplates
                UnpackFoldersPackage(zipPackage, TemplateFoldersPath, typedPackage.ProvisioningTemplateFolders);

                // unpack PnP OpenXml packages
                UnpackFoldersPackage(zipPackage, TemplateOpenXmlFoldersPath, typedPackage.ProvisioningTemplateOpenXmlPackageFolders);

                return typedPackage;
            }
        }

        #endregion
    }
}