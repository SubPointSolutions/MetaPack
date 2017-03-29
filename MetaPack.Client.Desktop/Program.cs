using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetaPack.Client.Desktop.Impl.Services;
using SubPointSolutions.Shelly.Core;

namespace MetaPack.Client.Desktop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var appService = new MetaPackAppService();

            if (args != null)
                appService.Args = args.ToList();

            ShServiceContainer.Instance.AppMetadataService = appService;

            appService.Run();
        }
    }
}
