using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetaPack.Core.Common;
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
using MetaPack.NuGet.Common;

namespace MetaPack.Tests.Scenarios
{
    [TestClass]
    public class DeploymentScenarioTests : MetaPackScenarioTestBase
    {
        #region constructors

        public DeploymentScenarioTests()
        {
            UseLocaNuGet = true;
        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy()
        {
            DeployInternal();
        }

        [TestMethod]
        [TestCategory("Metapack.API.Deployment")]
        //[TestCategory("CI.Core")]
        public void Can_Deploy_To_WebApplication()
        {
            DeployInternal();
        }

        #endregion

        #region utils

        private void DeployInternal()
        {
            throw new NotImplementedException("");
        }

        #endregion
    }
}