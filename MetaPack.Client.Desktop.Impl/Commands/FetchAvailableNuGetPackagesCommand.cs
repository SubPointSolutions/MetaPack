using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.Services;
using MetaPack.Client.Desktop.Impl.ViewModels;
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Core.Command;
using SubPointSolutions.Shelly.Desktop.Services;

namespace MetaPack.Client.Desktop.Impl.Commands
{
    public class FetchAvailableNuGetPackagesCommand : CommandBase
    {
        public FetchAvailableNuGetPackagesCommand()
        {
            Enabled = true;
            Packages = new List<NuGetPackageViewModel>();
        }

        #region properties

        public IEnumerable<NuGetPackageViewModel> Packages { get; set; }

        public string SearchPattern { get; set; }
        public bool IncluePrerelease { get; set; }

        #endregion

        #region methods
        public override void Execute()
        {
            var appDataServices = ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>();

            Packages = appDataServices.FetchAvailableNuGetPackages(SearchPattern, IncluePrerelease);
        }

        #endregion
    }
}
