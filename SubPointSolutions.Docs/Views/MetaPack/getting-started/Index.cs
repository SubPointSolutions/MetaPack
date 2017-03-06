using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SubPointSolutions.Docs.Views.MetaPack.getting_started
{
    [TestClass]
    public class Index
    {
        [TestMethod]
        [TestCategory("Docs.Basics")]
        public void Create_Package_SPMeta2()
        {
            Console.WriteLine("SPMeta2 1");
        }

        [TestMethod]
        [TestCategory("Docs.Basics")]
        public void Create_Package_PnP()
        {
            Console.WriteLine("PnP 2");
        }
    }
}
