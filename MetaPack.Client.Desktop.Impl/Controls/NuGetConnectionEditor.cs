using System.Windows.Forms;
using MetaPack.Client.Desktop.Impl.Controls.Base;
using MetaPack.Client.Desktop.Impl.ViewModels;
using SubPointSolutions.Shelly.Desktop.Services;

namespace MetaPack.Client.Desktop.Impl.Controls
{
    public partial class NuGetConnectionEditor : NuGetConnectionEditorProxy
    {
        public NuGetConnectionEditor()
        {
            InitializeComponent();
        }

        protected override void OnBindViewModel()
        {
            base.OnBindViewModel();

            BindService.AutoBindProperty(o => o.Name, tbUserName);
            BindService.AutoBindProperty(o => o.Url, tbUrl);
        }
    }

    public class NuGetConnectionEditorProxy : ViewUserControl<NuGetGalleryConnectionViewModel>
    {

    }
}
