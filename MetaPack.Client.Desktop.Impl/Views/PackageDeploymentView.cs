using System.Windows.Forms;
using MetaPack.Client.Desktop.Impl.Common;
using MetaPack.Client.Desktop.Impl.Controls.Base;
using MetaPack.Client.Desktop.Impl.Services;
using MetaPack.Client.Desktop.Impl.ViewModels;
using NuGet;
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Desktop.Extensions;

namespace MetaPack.Client.Desktop.Impl.Views
{
    public partial class PackageDeploymentView : PackageDeploymentViewProxy
    {

        public PackageDeploymentView()
        {
            InitializeComponent();

            pProgress.Visible = false;
        }

        protected override void OnBindViewModel()
        {
            base.OnBindViewModel();

            var appDataService = ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>();

            if (ViewModel.SharePointConnection != null)
                BindService.AutoBindProperty(m => m.SharePointConnection, cbConnection, appDataService.SharePointConnections);

            BindService.AutoBindProperty(m => m.NuGetPackage, cbPackage, new[] { ViewModel.NuGetPackage });
            BindService.AutoBindProperty(m => m.IsForceDeploy, cbForceDeploy);
        }

        internal void ReportProgress(string message, int current, int max)
        {
            this.WithSafeUIUpdate(() =>
            {
                // progreaa bar
                if (current > 0 && max > 0 && (current < max))
                {
                    var realValue = current + 1;

                    if (pbProgress.Minimum != 0)
                        pbProgress.Minimum = 0;

                    if (pbProgress.Maximum != max)
                        pbProgress.Maximum = max;

                    if (pbProgress.Value != realValue)
                        pbProgress.Value = realValue;
                }

                // output
                lbOutput.Items.Add(message);

                if (lbOutput.Items.Count > 0)
                    lbOutput.SelectedIndex = lbOutput.Items.Count - 1;
            });
        }
    }

    public partial class PackageDeploymentViewProxy : ViewUserControl<NuGetPackageDeploymentViewModel>
    {

    }


}
