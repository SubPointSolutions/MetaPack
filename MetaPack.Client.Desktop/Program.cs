using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetaPack.Client.Desktop.Impl.Services;
using SubPointSolutions.Shelly.Core;
using System.IO;

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
            // tmp fix
            DeleteRelativeAppFile("Microsoft.SharePoint.Client.dll");
            DeleteRelativeAppFile("Microsoft.SharePoint.Client.Runtime.dll");

            var appService = new MetaPackAppService();

            if (args != null)
                appService.Args = args.ToList();

            ShServiceContainer.Instance.AppMetadataService = appService;

            appService.Run();
        }

        private static void DeleteRelativeAppFile(string fileName)
        {
            try
            {
                var currentDir = Application.StartupPath;
                var filePath = Path.Combine(currentDir, fileName);

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch
            {

            }
        }
    }
}
