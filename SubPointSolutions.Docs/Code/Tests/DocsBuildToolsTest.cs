using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubPointSolutions.Docs.Code.Data;
using SubPointSolutions.Docs.Code.Services;
using SubPointSolutions.Docs.Code.Utils;
using SubPointSolutions.Docs.Services;
using SubPointSolutions.Docs.Services.Base;
using SubPointSolutions.Docs.Code.Tests;
namespace SubPointSolutions.Docs.Tests
{
    [TestClass]
    public class DocSamplesServiceTests
    {
        #region tests

        [TestMethod]
        [TestCategory("CI.Docs")]
        public void Can_Create_CS_Samples_As_Xml_Db()
        {
            var assemblyPath = Path.GetDirectoryName(GetType().Assembly.Location);
            var samples = new List<SampleMetadata>();


            samples.Add(new SampleMetadata
            {
                ContentFolderPath = assemblyPath + @"\..\..\Views\MetaPack",
            });

            foreach (var sample in samples)
            {
                CreateSamplesDbAsXml(sample.ContentFolderPath,
                                     sample.Resursive,
                                     "SubPointSolutions.Docs.Code.Samples",
                                     sample.StaticClassName,
                                     assemblyPath + @"\..\..\SampleFiles\MetaPack");
            }
        }

        [TestMethod]
        [TestCategory("CI.Docs.System")]
        public void Generate_Sample_Ref_Files()
        {
            var docsPrj = @"..\..\Views";
            var allSamples = GetAllSamples(docsPrj, true);

            // sys
            foreach (var sampleFolder in allSamples.GroupBy(s => s.SourceFileFolder))
            {
                foreach (var sampleFiles in sampleFolder.GroupBy(s => s.SourceFileName))
                {
                    var sample = sampleFiles.First();

                    var directoryPath = Path.Combine(sample.SourceFileFolder, "_samples");
                    Directory.CreateDirectory(directoryPath);

                    var sampleFileName = string.Format("{0}-SysAll.sample-ref",
                        sample.SourceFileNameWithoutExtension);

                    var sampleFilePath = Path.Combine(directoryPath, sampleFileName);

                    File.WriteAllText(sampleFilePath, "ref");
                }
            }

            // one-by-one
            foreach (var sample in allSamples)
            {
                var sampleBody = sample.MethodBody;

                var sampleFileName = string.Format("{0}-{1}.sample-ref",
                                    sample.SourceFileNameWithoutExtension,
                                    sample.MethodName);

                var directoryPath = Path.Combine(sample.SourceFileFolder, "_samples");
                Directory.CreateDirectory(directoryPath);

                var sampleFilePath = Path.Combine(directoryPath, sampleFileName);

                File.WriteAllText(sampleFilePath, sampleBody);
            }
        }

        #endregion

        #region utils
        private void CreateSamplesDbAsXml(
                string path,
                bool recursive,
                string namespaceName,
                string staticClassName,
                string samplesFolderPath)
        {
            Directory.CreateDirectory(samplesFolderPath);

            var samples = GetAllSamples(path, recursive);

            foreach (var sample in samples)
            {
                var className = sample.ClassName;
                var methodName = sample.MethodName;

                var fileName = String.Format("{0}.{1}.xml", className, methodName);
                var sampleFilePath = Path.Combine(samplesFolderPath, fileName);

                var sampleAsXml = sample.ToXml();

                File.WriteAllText(sampleFilePath, sampleAsXml);
            }
        }

        private List<DocSample> GetAllSamples(string path, bool resursive)
        {
            var result = new List<DocSample>();

            var services = ReflectionUtils.GetTypesFromAssembly<SamplesServiceBase>(GetType().Assembly);

            foreach (var service in services.Select(a => Activator.CreateInstance(a) as SamplesServiceBase))
                result.AddRange(service.LoadSamples(path, resursive));

            return result;
        }

        #endregion
    }
}
