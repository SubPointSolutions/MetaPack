using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetaPack.Client.Common.Commands;
using MetaPack.Core;
using MetaPack.NuGet.Services;
using MetaPack.SPMeta2.Services;
using MetaPack.Tests.Base;
using MetaPack.Tests.Extensions;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.Services;
using SPMeta2.Services;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using MetaPack.Tests.Consts;
using MetaPack.Tests.Utils;
using TraceServiceBase = MetaPack.Core.Services.TraceServiceBase;

namespace MetaPack.Tests.Scenarios
{
    //[TestClass]
    //public class _ClientScenarioTests : _DeploymentScenarioTests
    //{
    //    #region commands

    //    [TestMethod]
    //    [TestCategory("Metapack.Client.API")]
    //    //[TestCategory("CI.Core")]
    //    public void Can_Call_List_Command()
    //    {
    //        var webSiteUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.RootWebUrl);

    //        var command = new NuGetListCommand
    //        {
    //            Url = webSiteUrl
    //        };

    //        command.Execute();
    //    }

    //    [TestMethod]
    //    [TestCategory("Metapack.Client.API")]
    //    [TestCategory("CI.Core")]
    //    public void Can_Call_List_Command_O365()
    //    {
    //        var webSiteUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.RootWebUrl);
    //        var userName = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.O365UserName);
    //        var userPassword = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.O365UserPassword);

    //        var command = new NuGetListCommand
    //        {
    //            Url = webSiteUrl,

    //            UserName = userName,
    //            UserPassword = userPassword,

    //            IsSharePointOnline = true
    //        };

    //        command.Execute();
    //    }

    //    [TestMethod]
    //    [TestCategory("Metapack.Client.API")]
    //    //[TestCategory("CI.Core")]
    //    public void Can_Call_Install_Command()
    //    {
    //        var webSiteUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.RootWebUrl);
    //        var hasInstallHit = false;

    //        var solutionPackage = CreateNewSolutionPackage(SolutionPackageType.SPMeta2);
    //        UpdatePackageVersion(solutionPackage);

    //        var packageVersion = solutionPackage.Version;
    //        var packageId = solutionPackage.Id;

    //        var packagingService = new SPMeta2SolutionPackageService();

    //        WithNuGetContext((apiUrl, apiKey, repoUrl) =>
    //        {
    //            // push a new version of the package
    //            packagingService.Push(solutionPackage, apiUrl, apiKey);

    //            // get the package
    //            var ciRepo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
    //            var ciPackage = ciRepo.FindPackageSafe(packageId, new SemanticVersion(packageVersion));

    //            Assert.IsNotNull(ciPackage, "Solution package");

    //            // command testing
    //            var command = new DefaultInstallCommand
    //            {
    //                Source = repoUrl,
    //                Url = webSiteUrl,

    //                Id = packageId,
    //                Version = packageVersion,

    //                PreRelease = true
    //            };

    //            command.Execute();

    //            hasInstallHit = true;
    //        });



    //    }


    //    [TestMethod]
    //    [TestCategory("Metapack.Client.API")]
    //    [TestCategory("CI.Core")]
    //    public void Can_Install_SPMeta2SolutionPackageDeploymentService()
    //    {
    //        WithRootSharePointContext(context =>
    //        {
    //            var service = new SPMeta2SolutionPackageDeploymentService();

    //            var solutionPackage = CreateNewSolutionPackage(SolutionPackageType.SPMeta2);
    //            var solutionPackageProvisionOptions = new SolutionPackageProvisionOptions
    //            {
    //                SolutionPackage = solutionPackage,

    //                SharePointClientContext = context,
    //                SharePointSiteUrl = context.Url
    //            };

    //            service.Deploy(solutionPackageProvisionOptions);
    //        });
    //    }

    //    [TestMethod]
    //    [TestCategory("Metapack.Client.API")]
    //    [TestCategory("CI.Core")]
    //    public void Can_Call_Install_Command_O365()
    //    {
    //        var instance = MetaPackServiceContainer.Instance;

    //        instance.Services.Clear();

    //        instance.RegisterService(typeof(TraceServiceBase), new MetaPack.Core.Services.TraceSourceService());

    //        instance.RegisterService(typeof(NuGetSolutionPackageService), new SPMeta2SolutionPackageService());
    //        instance.RegisterService(typeof(SolutionPackageDeploymentService), new SPMeta2SolutionPackageDeploymentService());

    //        var webSiteUrl = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.RootWebUrl);
    //        var userName = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.O365UserName);
    //        var userPassword = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.O365UserPassword);

    //        var hasInstallHit = false;

    //        var solutionPackage = CreateNewSolutionPackage(SolutionPackageType.SPMeta2);
    //        UpdatePackageVersion(solutionPackage);

    //        var packageVersion = solutionPackage.Version;
    //        var packageId = solutionPackage.Id;

    //        var packagingService = new SPMeta2SolutionPackageService();

    //        WithNuGetContext((apiUrl, apiKey, repoUrl) =>
    //        {
    //            // push a new version of the package
    //            packagingService.Push(solutionPackage, apiUrl, apiKey);

    //            // get the package
    //            var ciRepo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
    //            var ciPackage = ciRepo.FindPackageSafe(packageId, new SemanticVersion(packageVersion));

    //            Assert.IsNotNull(ciPackage, "Solution package");

    //            // command testing
    //            var command = new DefaultInstallCommand
    //            {
    //                Source = repoUrl,
    //                Url = webSiteUrl,

    //                Id = packageId,
    //                Version = packageVersion,

    //                PreRelease = true,

    //                UserName = userName,
    //                UserPassword = userPassword,

    //                IsSharePointOnline = true
    //            };

    //            command.Execute();

    //            hasInstallHit = true;
    //        });



    //    }

    //    [TestMethod]
    //    [TestCategory("Metapack.Client.API")]
    //    [TestCategory("CI.Core")]
    //    public void Can_Call_Push_Command()
    //    {
    //        var hasInstallHit = false;

    //        var solutionPackage = CreateNewSolutionPackage(SolutionPackageType.SPMeta2);
    //        UpdatePackageVersion(solutionPackage);

    //        var packageVersion = solutionPackage.Version;
    //        var packageId = solutionPackage.Id;

    //        var packagingService = new SPMeta2SolutionPackageService();
    //        var packageStream = packagingService.Pack(solutionPackage, null);

    //        WithNuGetContext((apiUrl, apiKey, repoUrl) =>
    //        {
    //            // command testing
    //            var command = new DefaultNuGetPushCommand
    //            {
    //                Source = repoUrl,
    //                ApiKey = apiKey,

    //                Package = packageStream
    //            };

    //            command.Execute();

    //            // find package in the nuget gallery
    //            var ciRepo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
    //            var ciPackage = ciRepo.FindPackageSafe(packageId, new SemanticVersion(packageVersion));

    //            Assert.IsNotNull(ciPackage, "Solution package caanot be found");

    //            hasInstallHit = true;
    //        });

    //        Assert.IsTrue(hasInstallHit);

    //    }

    //    #endregion

    //}
}
