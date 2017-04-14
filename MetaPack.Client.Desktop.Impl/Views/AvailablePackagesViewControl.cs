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
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;
using NuGet;
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Core.Utils;
using SubPointSolutions.Shelly.Desktop.Controls;
using SubPointSolutions.Shelly.Desktop.Services;

namespace MetaPack.Client.Desktop.Impl.Views
{
    public partial class AvailablePackagesViewControl : ShUserControlBase
    {
        #region constructors
        public AvailablePackagesViewControl()
        {
            InitializeComponent();
            InitControl();

            var appService = ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>();

            if (appService.Settings.LoadAvailablePackagesOnStart)
                InternalAsyncRefresh(string.Empty, false, null);
        }
        #endregion

        #region properties

        protected MetaPackDataService AppService
        {
            get
            {
                return ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>();
            }
        }

        private IEnumerable<NuGetPackageViewModel> _tmpAllPackages;

        private bool _tmpAllPackagesFetch;

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

        protected virtual string SeachFilter
        {
            get { return tbSearch.Text; }
        }

        protected virtual bool IncludePrerelease
        {
            get { return cbIncludePrerelease.Checked; }
        }

        #endregion

        #region methods

        private void InitControl()
        {
            grData
                .AddDefaultNuGetPackageFields()
                .SetDefaultGridLookAndFeel()
                .SetDataSource(AppService.AvailableNuGetPackages);

            ShBinder.NewBinder(MpCommands.Instance.FetchAvailableNuGetPackagesCommand)
                .BindProperty(o => o.Enabled, bRefresh, c => c.Enabled)
                .BindEvent(bRefresh, c => c.Click += (s, e) => RefreshAvailablePackages());


            tbSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    RefreshAvailablePackages(tbSearch.Text, cbIncludePrerelease.Checked);
                }
            };

            cbIncludePrerelease.CheckedChanged += (s, e) =>
            {
                RefreshAvailablePackages(tbSearch.Text, cbIncludePrerelease.Checked);
            };

            lRefresh.Click += (s, e) =>
            {
                RefreshAvailablePackages(tbSearch.Text, cbIncludePrerelease.Checked);
            };

            grData.DataSource = AppService.AvailableNuGetPackages;

            tsViewPackageDetails.Click += (s, e) =>
            {
                if (SelectedPackage != null)
                    MpCommands.Instance.ShowNuGetPackageDetails(SelectedPackage);
            };

            tsDeployPackage.Click += (s, e) =>
            {
                if (SelectedPackage != null)
                    MpCommands.Instance.ProvisionNuGetPackage(SelectedPackage);
            };

            grData.DoubleClick += (s, e) =>
            {
                if (SelectedPackage != null)
                    MpCommands.Instance.ShowNuGetPackageDetails(SelectedPackage);
            };


            lAddNuGetConnection.Click += (s, e) =>
            {
                MpCommands.Instance.NewNuGetGalleryConnection();
            };

            Application.Idle += (s, e) =>
            {
                if (_tmpAllPackagesFetch)
                {
                    WithSafeUIUpdate(() =>
                    {
                        AppService.AvailableNuGetPackages.Clear();
                        AppService.AvailableNuGetPackages.AddRange(_tmpAllPackages);

                        _tmpAllPackagesFetch = false;

                        grData.Enabled = true;
                    });
                }
            };
        }

        private void RefreshAvailablePackages()
        {
            RefreshAvailablePackages(SeachFilter, IncludePrerelease);
        }

        protected void RefreshAvailablePackages(string search, bool incluePrerelease)
        {
            grData.Enabled = false;

            //  "Fetching available packages...",
            MetroUtils.WithLongOperation(this.ParentForm,
                progressForm =>
                {
                    progressForm.Text = "Fetching available packages...";



                }, handle =>
                {
                    handle.Output("Fetching available packages...");

                    InternalAsyncRefresh(search, incluePrerelease, () =>
                    {
                        handle.End();
                    });
                });
        }

        private void InternalAsyncRefresh(string search, bool incluePrerelease, Action onFinished)
        {
            ShAsync.DoAsync(new ShAsyncOptions
            {
                Do = () =>
                {
                    _tmpAllPackagesFetch = false;
                    _tmpAllPackages = MpCommands.Instance.FetchAvailableNuGetPackages(search, incluePrerelease);
                },
                OnDone = () =>
                {
                    _tmpAllPackagesFetch = true;

                    if (onFinished != null)
                        onFinished();
                }
            });
        }

        #endregion

    }
}
