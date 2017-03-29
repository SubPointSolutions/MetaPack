using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetaPack.Client.Desktop.Impl.Commands;
using MetaPack.Client.Desktop.Impl.Services;
using MetaPack.Client.Desktop.Impl.Utils;
using MetaPack.Client.Desktop.Impl.ViewModels;
using MetroFramework.Controls;
using NuGet;
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Core.Utils;
using SubPointSolutions.Shelly.Desktop.Controls;
using SubPointSolutions.Shelly.Desktop.Services;
using MetaPack.Client.Desktop.Impl.Common;
using MetaPack.Core;
using MetaPack.Core.Services;
using SubPointSolutions.Shelly.Core.Data;

namespace MetaPack.Client.Desktop.Impl.Views
{
    public partial class InstalledPackagesViewControl : ShUserControlBase
    {
        #region constructors
        public InstalledPackagesViewControl()
        {
            InitializeComponent();
            InitControl();
        }
        #endregion

        #region propeties

        private bool _tmpAllPackagesFetch;

        private IEnumerable<NuGetPackageViewModel> _tmpAllPackages;

        protected MetaPackDataService AppService
        {
            get
            {
                return ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>();
            }
        }

        protected ShDesktopAppService AppDesktopService
        {
            get
            {
                return ShServiceContainer.Instance.AppMetadataService as ShDesktopAppService;
            }
        }

        protected NuGetPackageViewModel SelectedPackage
        {
            get
            {
                if (grData.SelectedRows.Count > 0)
                {
                    return grData.SelectedRows[0].DataBoundItem as NuGetPackageViewModel;
                }

                return null;
            }
        }

        protected SharePointConnectionViewModel SelectedSharePointConnection
        {
            get { return AppService.ActiveSharePointConnection; }
        }

        protected bool IsAllowedSharePointConnection
        {
            get
            {
                return SelectedSharePointConnection != null
                       && SelectedSharePointConnection.Dto.AuthMode.Id != SharePointConnectionAuthMode.NotConnected.Id;
            }
        }

        private void SelectedConnectionChanged(PropertyChangedEventArgs e)
        {
            if (!this.Visible
                || e.PropertyName != "ActiveConnection"
                || !IsAllowedSharePointConnection)
            {
                MpCommands.Instance.FetchInstalledNuGetPackagesCommand.Data = null;
                return;
            }

            MpCommands.Instance.FetchInstalledNuGetPackagesCommand.Data = AppService.ActiveSharePointConnection;

            RefreshInstalledPackages();
        }

        #endregion

        #region methods

        protected FilteredBindingListView<SharePointConnectionViewModel> SharePointConnectionsDataSource
        {
            get
            {
                var sharePointConnections =
                new FilteredBindingListView<SharePointConnectionViewModel>(AppService.SharePointConnections);

                sharePointConnections.SortFunc = (f, s) => string.CompareOrdinal(f.Url, s.Url);
                sharePointConnections.ApplyFiter();
                sharePointConnections.ApplySort();

                sharePointConnections.Insert(0, SharePointConnectionViewModel.NotConnected);

                return sharePointConnections;
            }
        }

        private void InitControl()
        {
            var installedPackagesViewModel = new InstalledPackageViewModel();
            installedPackagesViewModel.ActiveConnection = AppService.ActiveSharePointConnection;

            if (AppService.ActiveSharePointConnection != null)
                cbSharePointConnection.SelectedItem = AppService.ActiveSharePointConnection;

            ShBinder.NewBinder(MpCommands.Instance.FetchInstalledNuGetPackagesCommand)
                .BindProperty(o => o.Enabled, bRefresh, c => c.Enabled)
                .BindEvent(bRefresh, c => c.Click += (s, e) => RefreshInstalledPackages());

            ShBinder.NewBinder(installedPackagesViewModel)
                .AutoBindProperty(o => o.ActiveConnection, cbSharePointConnection, SharePointConnectionsDataSource)
                .AutoBindProperty(o => o.SearchFilter, tbSearch)
                .AutoBindProperty(o => o.ShowWithUpdates, cbShowWithUpdates);


            Application.Idle += (s, e) => { UpdateNewPackages(); };

            installedPackagesViewModel.PropertyChanged += (s, e) =>
            {
                SelectedConnectionChanged(e);
            };

            grData
                .AddDefaultNuGetPackageFields()
                .SetDefaultGridLookAndFeel()
                .SetDataSource(AppService.InstalledNuGetPackages);

            tsViewPackageDetails.Click += (s, e) =>
            {
                if (SelectedPackage != null)
                    MpCommands.Instance.ShowNuGetPackageDetails(SelectedPackage);
            };

            grData.DoubleClick += (s, e) =>
            {
                if (SelectedPackage != null)
                    MpCommands.Instance.ShowNuGetPackageDetails(SelectedPackage);
            };
        }

        private void RefreshInstalledPackages()
        {
            // "Fetching installed packages..."
            MetroUtils.WithLongOperation(ParentForm,
                progressForm => { progressForm.Text = "Fetching installed packages..."; },
                handle =>
                {
                    var metaLog = MetaPackServiceContainer.Instance.GetService<TraceServiceBase>()
                        as MetaPackUITraceService;

                    EventHandler<TraceServiceEventArgs> onLogHandler = (sender, args) =>
                    {
                        if (handle != null)
                            handle.Output(args.Message);
                    };

                    WithSafeUIUpdate(() =>
                    {
                        var activeConnection = AppService.ActiveSharePointConnection;
                        var url = activeConnection.Url;

                        AppDesktopService.SetApplicationTitle(string.Format("{0} connecting to {1}",
                            AppDesktopService.AppName, url));
                    });

                    ShAsync.DoAsync(new ShAsyncOptions
                    {
                        Do = () =>
                        {
                            metaLog.OnTrace += onLogHandler;

                            _tmpAllPackagesFetch = false;
                            _tmpAllPackages = MpCommands.Instance.FetchInstalledNuGetPackages();
                        },
                        OnDone = () =>
                        {
                            metaLog.OnTrace -= onLogHandler;
                            handle.End();
                            _tmpAllPackagesFetch = true;

                            WithSafeUIUpdate(() =>
                            {
                                var activeConnection = AppService.ActiveSharePointConnection;
                                var url = activeConnection.Url;

                                AppDesktopService.SetApplicationTitle(string.Format("{0} on {1}",
                                    AppDesktopService.AppName, url));
                            });
                        }
                    });
                });
        }

        private void UpdateNewPackages()
        {
            if (_tmpAllPackagesFetch)
            {
                WithSafeUIUpdate(() =>
                {
                    AppService.InstalledNuGetPackages.Clear();
                    AppService.InstalledNuGetPackages.AddRange(_tmpAllPackages);

                    _tmpAllPackagesFetch = false;

                    grData.Enabled = true;
                });
            }
        }

        #endregion
    }
}