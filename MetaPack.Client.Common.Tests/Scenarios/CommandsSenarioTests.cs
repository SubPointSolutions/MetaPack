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

namespace MetaPack.Client.Common.Tests.Scenarios
{
    [TestClass]
    public class CommandsSenarioTests : DeploymentScenarioTests
    {
        #region list command

        [TestMethod]
        [TestCategory("Metapack.Client.Commands")]
        [TestCategory("Metapack.Client.Commands.OnPremise")]
        public void Can_Call_List_Command_OnPremise()
        {
            var webSiteUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.OnPremis.RootWebUrl);

            var command = new NuGetListCommand
            {
                Url = webSiteUrl
            };

            command.Execute();
        }

        [TestMethod]
        [TestCategory("Metapack.Client.Commands")]
        [TestCategory("Metapack.Client.Commands.O365")]
        public void Can_Call_List_Command_O365()
        {
            var webSiteUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.O365.RootWebUrl);

            var userName = EnvironmentUtils.GetEnvironmentVariable(RegConsts.O365.UserName);
            var userPassword = EnvironmentUtils.GetEnvironmentVariable(RegConsts.O365.UserPassword);

            var command = new NuGetListCommand
            {
                Url = webSiteUrl,

                UserName = userName,
                UserPassword = userPassword,

                IsSharePointOnline = true
            };

            command.Execute();
        }


        #endregion

        #region install command

        [TestMethod]
        [TestCategory("Metapack.Client.Commands")]
        [TestCategory("Metapack.Client.Commands.O365")]
        //[TestCategory("CI.Core")]
        public void Can_Call_Install_Command_O365()
        {
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

                        IsSharePointOnline = true,

                        PreRelease = true
                    };

                    command.PackageSources.Add(repoUrl);

                    if(UseLocaNuGet)
                        command.PackageSources.Add(LocalNuGetRepositoryFolderPath);

                    command.Execute();

                    actualInstallHits++;
                });

            });

            Assert.AreEqual(expectedInstallHits, actualInstallHits);
        }


        #endregion
    }
}
