using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.Services;
using SubPointSolutions.Shelly.Core;

namespace MetaPack.Client.Desktop.Impl.ViewModels
{
    public class InstalledPackageViewModel : ViewModelBase<InstalledPackageViewModel>
    {
        protected MetaPackDataService AppService
        {
            get
            {
                return ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>();
            }
        }

        public string SearchFilter { get; set; }
        public bool ShowWithUpdates { get; set; }

        public SharePointConnectionViewModel ActiveConnection
        {
            get
            {
                return AppService.ActiveSharePointConnection;
            }
            set
            {
                AppService.ActiveSharePointConnection = value;

                OnPropertyChanged(c => c.ActiveConnection);
            }
        }
    }
}
