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
        public void Can_Call_List_Command()
        {
            var url = EnvironmentUtils.GetEnvironmentVariable(RegConsts.SharePoint.RootWebUrl);

            var command = new NuGetListCommand
            {
                Url = url
            };

            command.Execute();
        }

        #endregion
    }
}
