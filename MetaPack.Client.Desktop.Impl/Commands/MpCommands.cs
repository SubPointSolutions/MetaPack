using System;
using System.Linq;
using MetaPack.Client.Desktop.Impl.Data;
using MetaPack.Client.Desktop.Impl.Events;
using MetaPack.Client.Desktop.Impl.Services;
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Core.Services;
using SubPointSolutions.Shelly.Desktop.Events.App;
using SubPointSolutions.Shelly.Desktop.Services;
using SubPointSolutions.Shelly.Desktop.Utils;
using System.Windows.Forms;
using MetaPack.Client.Desktop.Impl.Views;
using MetaPack.Client.Desktop.Impl.Controls;
using MetaPack.Client.Desktop.Impl.ViewModels;
using System.Collections.Generic;
using SubPointSolutions.Shelly.Core.Utils;
using System.Threading;
using MetaPack.Client.Common.Commands;
using MetaPack.Client.Desktop.Impl.Common;
using NuGet;
using MetaPack.Core;
using MetaPack.Core.Services;

namespace MetaPack.Client.Desktop.Impl.Commands
{
    public class MpCommands
    {
        #region constructors

        private MpCommands()
        {
            FetchInstalledNuGetPackagesCommand = new FetchInstalledNuGetPackagesCommand();
            FetchAvailableNuGetPackagesCommand = new FetchAvailableNuGetPackagesCommand();
        }

        #endregion

        #region properties

        public FetchAvailableNuGetPackagesCommand FetchAvailableNuGetPackagesCommand { get; set; }
        public FetchInstalledNuGetPackagesCommand FetchInstalledNuGetPackagesCommand { get; set; }

        public static readonly MpCommands Instance = new MpCommands();

        public MetaPackDataService AppDataService
        {
            get
            {
                return ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>();
            }
        }

        #endregion

        public void NewSharePointConnection()
        {
            var cmd = new NewSharePointConnectionCommand();

            cmd.Data = new SharePointConnectionViewModel();
            cmd.Execute();
        }

        public void ExitApp()
        {
            var cmd = new ExitAppCommand();

            cmd.Execute();
        }

        public void ShowAboutWindow()
        {
            var cmd = new ShowAboutWindowCommand();

            cmd.Execute();
        }

        public void ShowOptionsWindow()
        {
            var cmd = new ShowOptionsWindowCommand();

            cmd.Data = new MetaPackDesktopSettingsViewModel();
            cmd.Execute();
        }

        public void DeleteNuGetGalleryConnection(NuGetGalleryConnectionViewModel viewModel)
        {
            var cmd = new DeleteNuGetGalleryConnectionCommand();

            cmd.Data = viewModel;
            cmd.Execute();
        }

        public void EditNuGetGalleryConnection(NuGetGalleryConnectionViewModel viewModel)
        {
            var cmd = new EditNuGetGalleryConnectionCommand();

            cmd.Data = viewModel;
            cmd.Execute();
        }

        public void EditSharePointConnection(SharePointConnectionViewModel viewModel)
        {
            var cmd = new EditSharePointConnectionCommand();

            cmd.Data = viewModel;
            cmd.Execute();
        }

        public void DeleteSharePointConnection(SharePointConnectionViewModel viewModel)
        {
            var cmd = new DeleteSharePointConnectionCommand();

            cmd.Data = viewModel;
            cmd.Execute();
        }

        public void NewNuGetGalleryConnection()
        {
            var cmd = new AddNuGetGalleryConnectionCommand();

            cmd.Execute();
        }

        public IEnumerable<NuGetPackageViewModel> FetchAvailableNuGetPackages(string searchPattern, bool incluePrerelease)
        {
            var cmd = FetchAvailableNuGetPackagesCommand;

            cmd.SearchPattern = searchPattern;
            cmd.IncluePrerelease = incluePrerelease;

            cmd.Execute();

            return cmd.Packages;
        }



        public IEnumerable<NuGetPackageViewModel> FetchInstalledNuGetPackages()
        {
            var appDataServices = ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>();
            var cmd = FetchInstalledNuGetPackagesCommand;

            cmd.Data = appDataServices.ActiveSharePointConnection;
            cmd.Execute();

            return cmd.Packages;
        }

        public void ShowNuGetPackageDetails(NuGetPackageViewModel viewModel)
        {
            var cmd = new ShowNuGetPackageDetailsCommand();

            cmd.Data = viewModel;
            cmd.Execute();
        }

        public void ProvisionNuGetPackage(NuGetPackageViewModel viewModel)
        {
            var cmd = new ProvisionNuGetPackageCommand();

            cmd.Data = viewModel;
            cmd.Execute();
        }
    }
}
