using MetaPack.Client.Desktop.Impl.Common;
using MetaPack.Client.Desktop.Impl.Controls.Base;
using MetaPack.Client.Desktop.Impl.ViewModels;
using System.Collections.Generic;

namespace MetaPack.Client.Desktop.Impl.Controls
{
    public partial class SharePointConnectionEditor : SharePointConnectionEditorProxy
    {
        public SharePointConnectionEditor()
        {
            InitializeComponent();

            InitData();
            InitControls();
        }

        private void InitData()
        {
            ViewModelAuthModes = SharePointConnectionViewModel.AuthModes;
        }

        private void InitControls()
        {
            cbAuthMode.SelectedIndexChanged += (s, e) =>
            {
                UpdateUseCurrentCredentialsState();
            };
        }

        private void UpdateUseCurrentCredentialsState()
        {
            if (cbAuthMode.SelectedItem == SharePointConnectionAuthMode.WindowsAuthentication)
                cbUseCurrentCredentials.Enabled = true;
            else
                cbUseCurrentCredentials.Enabled = false;
        }

        public List<SharePointConnectionAuthMode> ViewModelAuthModes { get; set; }

        protected override void OnBindViewModel()
        {
            base.OnBindViewModel();

            //BindService.AutoBindProperty(o => o.Name, tbUserName);
            BindService.AutoBindProperty(o => o.Url, tbUrl);

            BindService.AutoBindProperty(o => o.UserName, tbUserName);
            BindService.AutoBindProperty(o => o.UserPassword, tbUserPassoword);

            BindService.AutoBindProperty(o => o.AuthMode, cbAuthMode, ViewModelAuthModes);

            UpdateUseCurrentCredentialsState();
        }
    }

    public class SharePointConnectionEditorProxy : ViewUserControl<SharePointConnectionViewModel>
    {

    }
}