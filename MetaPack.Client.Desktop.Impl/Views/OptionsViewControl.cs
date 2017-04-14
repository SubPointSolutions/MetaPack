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
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Desktop.Controls;
using SubPointSolutions.Shelly.Desktop.Interfaces;
using MetaPack.Client.Desktop.Impl.Controls.Base;

namespace MetaPack.Client.Desktop.Impl.Views
{
    public partial class OptionsViewControl : OptionsViewControlProxy
    {
        public OptionsViewControl()
        {
            InitializeComponent();
            InitControls();
        }

        protected override void OnBindViewModel()
        {
            base.OnBindViewModel();

            InitViewModelData();
        }

        private void InitControls()
        {
            
            InitTabControl();
        }

        private void InitViewModelData()
        {
            InitNuGetPage(gcNuGet);
            InitSharePointPage(gcSharePoint);
            InitMiscPage();
        }

        private void InitTabControl()
        {
            if (tcOptions.TabPages.Count > 0)
                tcOptions.SelectedIndex = 0;
        }

        private void InitSharePointPage(MetroGrid grid)
        {
            DefaultGridLookAndFeel(grid);
            grid.DataSource = ViewModel.SharePointConnections;

            lSharePoint.Click += (s, e) =>
            {
                MpCommands.Instance.NewSharePointConnection();
            };

            tsSharePointAdd.Click += (s, e) =>
            {
                MpCommands.Instance.NewSharePointConnection();
            };

            grid.DoubleClick += (s, e) =>
            {
                if (grid.SelectedRows.Count > 0)
                {
                    var viewModel = grid.SelectedRows[0].DataBoundItem as SharePointConnectionViewModel;
                    MpCommands.Instance.EditSharePointConnection(viewModel);
                }
            };

            tsSharePointEdit.Click += (s, e) =>
            {
                if (grid.SelectedRows.Count > 0)
                {
                    var viewModel = grid.SelectedRows[0].DataBoundItem as SharePointConnectionViewModel;
                    MpCommands.Instance.EditSharePointConnection(viewModel);
                }
            };

            tsSharePointDelete.Click += (s, e) =>
            {
                if (grid.SelectedRows.Count > 0)
                {
                    var viewModel = grid.SelectedRows[0].DataBoundItem as SharePointConnectionViewModel;
                    MpCommands.Instance.DeleteSharePointConnection(viewModel);
                }
            };

            grid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Url",
                HeaderText = "Url",
                DataPropertyName = "Url"
            });

            //gcNuGet.Columns.Add(new DataGridViewTextBoxColumn()
            //{
            //    Name = "Url",
            //    HeaderText = "Url",
            //    DataPropertyName = "Url"
            //});
        }

        private void InitNuGetPage(MetroGrid grid)
        {
            DefaultGridLookAndFeel(grid);
            grid.DataSource = ViewModel.NuGetConnections;

            lNuGet.Click += (s, e) =>
            {
                MpCommands.Instance.NewNuGetGalleryConnection();
            };

            tsNuGetAdd.Click += (s, e) =>
            {
                MpCommands.Instance.NewNuGetGalleryConnection();
            };

            tsNuGetEdit.Click += (s, e) =>
            {
                if (grid.SelectedRows.Count > 0)
                {
                    var viewModel = grid.SelectedRows[0].DataBoundItem as NuGetGalleryConnectionViewModel;
                    MpCommands.Instance.EditNuGetGalleryConnection(viewModel);
                }
            };

            tsNuGetDelete.Click += (s, e) =>
            {
                if (grid.SelectedRows.Count > 0)
                {
                    var viewModel = grid.SelectedRows[0].DataBoundItem as NuGetGalleryConnectionViewModel;
                    MpCommands.Instance.DeleteNuGetGalleryConnection(viewModel);
                }
            };

            grid.DoubleClick += (s, e) =>
            {
                if (grid.SelectedRows.Count > 0)
                {
                    var viewModel = grid.SelectedRows[0].DataBoundItem as NuGetGalleryConnectionViewModel;
                    MpCommands.Instance.EditNuGetGalleryConnection(viewModel);
                }
            };

            grid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Name",
                HeaderText = "Name",
                DataPropertyName = "Name"
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Url",
                HeaderText = "Url",
                DataPropertyName = "Url"
            });
        }

        private void InitMiscPage()
        {
            BindService.AutoBindProperty(m => m.LoadAvailablePackagesOnStart, cbLoadAvailablePackagesOnStart);
        }

        private void DefaultGridLookAndFeel(MetroGrid grid)
        {
            MetroUtils.SetDefaultGridLookAndFeel(grid);
        }
    }

    public class OptionsViewControlProxy : ViewUserControl<MetaPackDesktopSettingsViewModel>
    {

    }
}
