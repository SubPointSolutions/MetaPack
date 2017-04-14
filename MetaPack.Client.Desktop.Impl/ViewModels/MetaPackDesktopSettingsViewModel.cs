using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.Services;
using SubPointSolutions.Shelly.Core;

namespace MetaPack.Client.Desktop.Impl.ViewModels
{
    public class MetaPackDesktopSettingsViewModel : ViewModelBase<MetaPackDesktopSettingsViewModel>
    {
        public BindingList<SharePointConnectionViewModel> SharePointConnections
        {
            get { return AppService.SharePointConnections; }
            set { }
        }

        public BindingList<NuGetGalleryConnectionViewModel> NuGetConnections
        {
            get { return AppService.NuGetConnections; }
            set { }
        }

        protected MetaPackDataService AppService
        {
            get
            {
                return ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>();
            }
        }

        public bool LoadAvailablePackagesOnStart
        {
            get
            {
                return AppService.Settings.LoadAvailablePackagesOnStart;
            }
            set
            {
                AppService.Settings.LoadAvailablePackagesOnStart = value;

                OnPropertyChanged(c => c.LoadAvailablePackagesOnStart);
            }
        }

    }
}
