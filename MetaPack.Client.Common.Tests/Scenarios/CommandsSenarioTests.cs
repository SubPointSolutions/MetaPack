using MetaPack.Client.Common.Commands;
using MetaPack.Core;
using MetaPack.Core.Services;
using MetaPack.NuGet.Services;
using MetaPack.Tests.Consts;
using MetaPack.Tests.Extensions;
using MetaPack.Tests.Scenarios;
using MetaPack.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System;
using System.Text;

namespace MetaPack.Client.Common.Tests.Scenarios
{
    [TestClass]
    public class CommandsSenarioTests : DeploymentScenarioTests
    {
        #region list command

        [TestMethod]
        [TestCategory("Metapack.Client.API")]
        [TestCategory("Metapack.Client.API.SP2013")]
        public void Can_Call_List_Command_OnPremise()
        {
            var hasOnTraceEvents = false;
            var webSiteUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SP2013.RootWebUrl);

            var command = new NuGetListCommand
            {
                Url = webSiteUrl,

                SharePointApi = "CSOM",
                SharePointEdition = "Standard",
                SharePointVersion = "SP2013",

                UserName = SP2013UserName,
                UserPassword = SP2013UserPassword,
            };

            command.OnTraceEvent += (s, e) =>
            {
                hasOnTraceEvents = true;
                Trace.WriteLine(e.Message);
            };

            command.Execute();

            Assert.IsTrue(command.Packages.Count() > 0);
            Assert.IsTrue(hasOnTraceEvents);
        }

        [TestMethod]
        [TestCategory("Metapack.Client.API")]
        [TestCategory("Metapack.Client.API.O365")]
        public void Can_Call_List_Command_O365()
        {
            var hasOnTraceEvents = false;
            var webSiteUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.O365.RootWebUrl);

            var userName = EnvironmentUtils.GetEnvironmentVariable(RegConsts.O365.UserName);
            var userPassword = EnvironmentUtils.GetEnvironmentVariable(RegConsts.O365.UserPassword);

            var command = new NuGetListCommand
            {
                Url = webSiteUrl,

                UserName = userName,
                UserPassword = userPassword,

                SharePointVersion = "o365"
            };

            command.OnTraceEvent += (s, e) =>
            {
                hasOnTraceEvents = true;
                Trace.WriteLine(e.Message);
            };

            command.Execute();

            Assert.IsTrue(command.Packages.Count() > 0);
            Assert.IsTrue(hasOnTraceEvents);
        }

        public class TraceWriter : TextWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }

            public override void Write(string value)
            {
                WriteLine(value);
            }

            public override void WriteLine(string value)
            {
                Trace.WriteLine(value);
            }
        }


        #endregion

        #region install command

        [TestMethod]
        [TestCategory("Metapack.Client.API")]
        [TestCategory("Metapack.Client.API.O365")]
        //[TestCategory("CI.Core")]
        public void Can_Call_Install_Command_O365()
        {
            var hasOnTraceEvents = false;

            var expectedInstallHits = 0;
            var actualInstallHits = 0;

            WithMetaPackServices(service =>
            {
                expectedInstallHits++;

                var packagingService = service.PackagingService;
                var solutionPackage = CreateNewSolutionPackage(packagingService);

                UpdatePackageVersion(solutionPackage);

                PushPackageToCIRepository(solutionPackage, null, packagingService);

                var webSiteUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.O365.RootWebUrl);

                var userName = EnvironmentUtils.GetEnvironmentVariable(RegConsts.O365.UserName);
                var userPassword = EnvironmentUtils.GetEnvironmentVariable(RegConsts.O365.UserPassword);

                var packageVersion = solutionPackage.Version;
                var packageId = solutionPackage.Id;

                WithCINuGetContext((apiUrl, apiKey, repoUrl) =>
                {
                    var command = new DefaultInstallCommand
                    {
                        Url = webSiteUrl,

                        Id = packageId,
                        Version = packageVersion,

                        UserName = userName,
                        UserPassword = userPassword,

                        SharePointVersion = "o365",

                        PreRelease = true
                    };

                    command.OnTraceEvent += (s, e) =>
                    {
                        hasOnTraceEvents = true;
                        Trace.WriteLine(e.Message);
                    };

                    command.PackageSources.Add(repoUrl);

                    if (UseLocaNuGet)
                        command.PackageSources.Add(LocalNuGetRepositoryFolderPath);

                    command.Execute();

                    actualInstallHits++;
                });

            });

            Assert.AreEqual(expectedInstallHits, actualInstallHits);
            Assert.IsTrue(hasOnTraceEvents);
        }


        #endregion
    }
}
