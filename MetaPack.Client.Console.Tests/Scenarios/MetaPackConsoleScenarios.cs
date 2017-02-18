using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Exceptions;
using MetaPack.Tests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetaPack.Client.Console.Tests.Scenarios
{
    [TestClass]
    public class MetaPackConsoleScenarios : MetaPackScenarioTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Metapack.Client.Console")]
        public void Can_Create_Instance()
        {
            var client = new MetaPackConsoleClient();

            Assert.IsNotNull(client);
        }

        [TestMethod]
        [TestCategory("Metapack.Client.Console")]
        public void Wrong_Command_Should_Return_One()
        {
            var result = ExecuteClientWithArgs(new Dictionary<string, string>()
            {

            });

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        [TestCategory("Metapack.Client.Console")]
        [TestCategory("CI.O365")]
        public void Can_Run_Install_Command_O365()
        {
            WithMetaPackServices(service =>
            {
                var packageId = service.CIPackageId;

                var siteUrl = O365RootWebUrl;
                var userName = O365UserName;
                var userPassword = O365UserPassword;

                var result = ExecuteClientWithArgs(new Dictionary<string, string>()
                {
                    { "install", string.Empty},
                    { "--id", packageId},
                    { "--url", siteUrl},
                    { "--username", userName},
                    { "--userpassword", userPassword},
                    { "--spversion", "o365"},
                });

                Assert.AreEqual(0, result);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.Client.Console")]
        [TestCategory("CI.O365")]
        public void Can_Run_Install_Command_O365_From_CustomSource()
        {
            WithMetaPackServices(service =>
            {
                var packageId = service.CIPackageId;

                var siteUrl = O365RootWebUrl;
                var userName = O365UserName;
                var userPassword = O365UserPassword;

                var result = ExecuteClientWithArgs(new Dictionary<string, string>()
                {
                    { "install", string.Empty},
                    
                    { "--id", "DefinitelyPacked.jQuery"},
                    { "--version", "1.12.4"},

                    { "--url", siteUrl},
                    { "--username", userName},
                    { "--userpassword", userPassword},
                    { "--spversion", "o365"},

                    { "--source", "https://ci.appveyor.com/nuget/definitelypacked-nuget"},
                });

                Assert.AreEqual(0, result);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.Client.Console")]
        [TestCategory("CI.O365")]
        public void Can_Run_Install_Command_O365_Force()
        {
            WithMetaPackServices(service =>
            {
                var packageId = service.CIPackageId;

                var siteUrl = O365RootWebUrl;
                var userName = O365UserName;
                var userPassword = O365UserPassword;

                var result = ExecuteClientWithArgs(new Dictionary<string, string>()
                {
                    { "install", string.Empty},
                    
                    { "--id", "DefinitelyPacked.jQuery"},
                    { "--version", "1.12.4"},

                    { "--url", siteUrl},
                    { "--username", userName},
                    { "--userpassword", userPassword},
                    { "--spversion", "o365"},
                    { "--force", string.Empty },

                    { "--source", "https://ci.appveyor.com/nuget/definitelypacked-nuget"},
                });

                Assert.AreEqual(0, result);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.Client.Console")]
        [TestCategory("CI.O365")]

        public void Can_Run_Install_Command_O365_WithCustomToolId()
        {
            WithMetaPackServices(service =>
            {
                var packageId = service.CIPackageId;

                var siteUrl = O365RootWebUrl;
                var userName = O365UserName;
                var userPassword = O365UserPassword;

                var result = ExecuteClientWithArgs(new Dictionary<string, string>()
                {
                    { "install", string.Empty},
                    { "--id", packageId},
                    { "--url", siteUrl},
                    { "--username", userName},
                    { "--userpassword", userPassword},
                    { "--spversion", "o365"},
                    { "--toolid", "MetaPack.SPMeta2"}
                });

                Assert.AreEqual(0, result);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.Client.Console")]
        [TestCategory("CI.O365")]
        [ExpectedException(typeof(MetaPackException))]
        public void Can_Run_Install_Command_O365_WithCustomToolIdAndVersion()
        {
            // should raise MetaPackException on cannot find the tool
            WithMetaPackServices(service =>
            {
                var packageId = service.CIPackageId;

                var siteUrl = O365RootWebUrl;
                var userName = O365UserName;
                var userPassword = O365UserPassword;

                var result = ExecuteClientWithArgs(new Dictionary<string, string>()
                {
                    { "install", string.Empty},
                    { "--id", packageId},
                    { "--url", siteUrl},
                    { "--username", userName},
                    { "--userpassword", userPassword},
                    { "--spversion", "o365"},
                    { "--toolid", "MetaPack.SPMeta2"},
                    { "--toolversion", "1.1.0." + new Random().Next(10,100)}
                });

                Assert.AreEqual(0, result);
            });
        }


        [TestMethod]
        [TestCategory("Metapack.Client.Console")]
        [TestCategory("CI.O365")]
        public void Can_Run_Install_Command_O365_WithCustomToolIdAndVersion_12100()
        {
            WithMetaPackServices(service =>
            {
                var packageId = service.CIPackageId;

                var siteUrl = O365RootWebUrl;
                var userName = O365UserName;
                var userPassword = O365UserPassword;

                var result = ExecuteClientWithArgs(new Dictionary<string, string>()
                {
                    { "install", string.Empty},
                    { "--id", packageId},
                    { "--url", siteUrl},
                    { "--username", userName},
                    { "--userpassword", userPassword},
                    { "--spversion", "o365"},
                    { "--toolid", "MetaPack.SPMeta2"},
                    { "--toolversion", "1.2.100"}
                });

                Assert.AreEqual(0, result);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.Client.Console")]
        [TestCategory("CI.OnPremis")]
        public void Can_Run_Install_Command_OnPremis_With_CSOM()
        {
            WithMetaPackServices(service =>
            {
                var packageId = service.CIPackageId;

                var siteUrl = OnPremisRootWebUrl;

                var result = ExecuteClientWithArgs(new Dictionary<string, string>()
                {
                    { "install", string.Empty},
                    { "--id", packageId},
                    { "--url", siteUrl},
                    { "--spversion", "sp2013"},
                });

                Assert.AreEqual(0, result);
            });
        }

        [TestMethod]
        [TestCategory("Metapack.Client.Console")]
        [TestCategory("CI.OnPremis")]
        public void Can_Run_Install_Command_OnPremis_With_SSOM()
        {
            WithMetaPackServices(service =>
            {
                var packageId = service.CIPackageId;

                var siteUrl = OnPremisRootWebUrl;

                var result = ExecuteClientWithArgs(new Dictionary<string, string>()
                {
                    { "install", string.Empty},
                    { "--id", packageId},
                    { "--url", siteUrl},
                    { "--spversion", "sp2013"},
                    { "--spruntime", "ssom"}
                });

                Assert.AreEqual(0, result);
            });
        }

        #endregion

        #region utils

        protected virtual int ExecuteClientWithArgs(Dictionary<string, string> arguments)
        {
            var args = new List<string>();
            var finalString = string.Empty;

            foreach (var name in arguments.Keys)
            {
                args.Add(name);
                finalString += " " + name;

                var value = arguments[name].Trim();

                if (!string.IsNullOrEmpty(value))
                {
                    args.Add(value);
                    finalString += " ***";
                }
            }

            var client = new MetaPackConsoleClient();

            Trace.WriteLine(string.Format("Running client with params:[{0}]", finalString));
            return client.Run(args.ToArray());
        }

        #endregion
    }
}
