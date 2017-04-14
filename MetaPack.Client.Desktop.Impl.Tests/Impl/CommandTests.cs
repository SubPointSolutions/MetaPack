using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.Commands;
using MetaPack.Client.Desktop.Impl.Services;
using MetaPack.Client.Desktop.Impl.ViewModels;
using SubPointSolutions.Shelly.Core;

namespace MetaPack.Client.Desktop.Impl.Tests.Impl
{
    [TestClass]
    public class CommandTests
    {
        [TestInitialize]
        public void Init()
        {
            var appService = new MetaPackAppService();
            ShServiceContainer.Instance.AppMetadataService = appService;

            appService.InitAppServices();
        }

        [TestMethod]
        [TestCategory("Metapack.Client.UI")]
        public void Can_Execute_ShowOptionsWindowCommand()
        {
            var cmd = new ShowOptionsWindowCommand();

            cmd.Data = new MetaPackDesktopSettingsViewModel();
            cmd.Execute();
        }

        [TestMethod]
        [TestCategory("Metapack.Client.UI")]
        public void Can_Execute_ProvisionNuGetPackageCommand()
        {
            var cmd = new ProvisionNuGetPackageCommand();

            cmd.Data = new NuGetPackageViewModel();
            cmd.Execute();
        }
    }
}