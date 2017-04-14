using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.Services;
using MetaPack.Client.Desktop.Impl.ViewModels;
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Desktop.Services;
using MetaPack.Client.Desktop.Impl.Commands.Base;

namespace MetaPack.Client.Desktop.Impl.Commands
{
    public class FetchInstalledNuGetPackagesCommand : MetaPackDataCommandBase<SharePointConnectionViewModel>
    {
        public FetchInstalledNuGetPackagesCommand()
        {
            Enabled = false;
            DisplayName = "Fetch installed NuGet packages";

            Packages = new List<NuGetPackageViewModel>();
        }

        #region properties

        public IEnumerable<NuGetPackageViewModel> Packages { get; set; }

        private SharePointConnectionViewModel _data;

        public override SharePointConnectionViewModel Data
        {
            get { return _data; }
            set
            {
                _data = value;

                var newEnabledValue = (value != null);

                if (newEnabledValue != Enabled)
                    Enabled = newEnabledValue;
            }
        }

        #endregion

        #region methods
        public override void Execute()
        {
            var appDataServices = ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>();
            var activeSharePointConnection = Data;

            appDataServices.ActiveSharePointConnection = activeSharePointConnection;

            var installedPackages = appDataServices.FetchInstalledNuGetPackages(activeSharePointConnection);

            Packages = installedPackages;
        }

        #endregion
    }
}
