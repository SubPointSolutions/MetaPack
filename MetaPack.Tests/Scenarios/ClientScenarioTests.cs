using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetaPack.Client.Common.Commands;
using MetaPack.NuGet.Services;
using MetaPack.SPMeta2.Services;
using MetaPack.Tests.Base;
using MetaPack.Tests.Common;
using MetaPack.Tests.Extensions;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.Services;
using SPMeta2.Services;
using MetaPack.Core.Packaging;
using MetaPack.Tests.Consts;
using MetaPack.Tests.Utils;

namespace MetaPack.Tests.Scenarios
{
    [TestClass]
    public class ClientScenarioTests : DeploymentScenarioTests
    {
        #region commands

        [TestMethod]
        [TestCategory("Metapack.Client.Commands")]
        //[TestCategory("CI.Core")]
        public void Can_Call_List_Command()
        {
            var webSiteUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.RootWebUrl);

            var command = new NuGetListCommand
            {
                Url = webSiteUrl
            };

            command.Execute();
        }

        [TestMethod]
        [TestCategory("Metapack.Client.Commands")]
        [TestCategory("CI.Core")]
        public void Can_Call_List_Command_O365()
        {
            var webSiteUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.RootWebUrl);
            var userName = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.O365UserName);
            var userPassword = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.O365UserPassword);

            var command = new NuGetListCommand
            {
                Url = webSiteUrl,

                UserName = userName,
                UserPassword = userPassword,

                IsSharePointOnline = true
            };

            command.Execute();
        }

        [TestMethod]
        [TestCategory("Metapack.Client.Commands")]
        //[TestCategory("CI.Core")]
        public void Can_Call_Install_Command()
        {
            var webSiteUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.RootWebUrl);
            var hasInstallHit = false;

            var solutionPackage = CreateNewSolutionPackage(SolutionPackageType.SPMeta2);
            UpdatePackageVersion(solutionPackage);

            var packageVersion = solutionPackage.Version;
            var packageId = solutionPackage.Id;

            var packagingService = new SPMeta2SolutionPackageService();

            WithNuGetContext((apiUrl, apiKey, repoUrl) =>
            {
                // push a new version of the package
                packagingService.Push(solutionPackage, apiUrl, apiKey);

                // get the package
                var ciRepo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
                var ciPackage = ciRepo.FindPackageSafe(packageId, new SemanticVersion(packageVersion));

                Assert.IsNotNull(ciPackage, "Solution package");

                // command testing
                var command = new DefaultInstallCommand
                {
                    Source = repoUrl,
                    Url = webSiteUrl,

                    Id = packageId,
                    Version = packageVersion,

                    PreRelease = true
                };

                command.Execute();

                hasInstallHit = true;
            });



        }

        [TestMethod]
        [TestCategory("Metapack.Client.Commands")]
        [TestCategory("CI.Core")]
        public void Can_Call_Install_Command_O365()
        {
            var webSiteUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.RootWebUrl);
            var userName = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.O365UserName);
            var userPassword = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.O365UserPassword);

            var hasInstallHit = false;

            var solutionPackage = CreateNewSolutionPackage(SolutionPackageType.SPMeta2);
            UpdatePackageVersion(solutionPackage);

            var packageVersion = solutionPackage.Version;
            var packageId = solutionPackage.Id;

            var packagingService = new SPMeta2SolutionPackageService();

            WithNuGetContext((apiUrl, apiKey, repoUrl) =>
            {
                // push a new version of the package
                packagingService.Push(solutionPackage, apiUrl, apiKey);

                // get the package
                var ciRepo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
                var ciPackage = ciRepo.FindPackageSafe(packageId, new SemanticVersion(packageVersion));

                Assert.IsNotNull(ciPackage, "Solution package");

                // command testing
                var command = new DefaultInstallCommand
                {
                    Source = repoUrl,
                    Url = webSiteUrl,

                    Id = packageId,
                    Version = packageVersion,

                    PreRelease = true,

                    UserName = userName,
                    UserPassword = userPassword,

                    IsSharePointOnline = true
                };

                command.Execute();

                hasInstallHit = true;
            });



        }

        #endregion
    }
}
