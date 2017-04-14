using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetaPack.Client.Desktop.Impl.Controls.Base;
using MetaPack.Client.Desktop.Impl.ViewModels;
using MetroFramework.Controls;

namespace MetaPack.Client.Desktop.Impl.Views
{
    public partial class PackageDetailsView : PackageDetailsViewProxy
    {
        public PackageDetailsView()
        {
            InitializeComponent();
        }

        protected override void OnBindViewModel()
        {
            base.OnBindViewModel();

            lTitle.Text = string.Format("{0} v{1}", ViewModel.Title, ViewModel.Version);

            BindService.AutoBindProperty(o => o.Authors, this.mAuthors);
            BindService.AutoBindProperty(o => o.Id, this.lId);
            BindService.AutoBindProperty(o => o.Downloads, this.lDownloads);
            BindService.AutoBindProperty(o => o.TotalDownloads, this.mTotalDownloads);
            BindService.AutoBindProperty(o => o.Created, this.lCreated);
            BindService.AutoBindProperty(o => o.Description, this.lDescription);
            BindService.AutoBindProperty(o => o.LastUpdated, this.lLastUpdated);
            BindService.AutoBindProperty(o => o.PackageSize, this.lPackageSize);
            BindService.AutoBindProperty(o => o.Dependencies, this.lDependencies);
            BindService.AutoBindProperty(o => o.ReleaseNotes, this.lReleaseNotes);

            BindLink(ViewModel.ProjectSite, lProjectSite);
            BindLink(ViewModel.License, lLicense);
            BindLink(ViewModel.Gallery, lGallery);


            ;

            //BindService.AutoBindProperty(o => o.ProjectSite, this.lProjectSite);
            //BindService.AutoBindProperty(o => o.License, this.lLicense);
            //BindService.AutoBindProperty(o => o.Gallery, this.lGallery);
        }

        private void BindLink(string url, MetroLink link)
        {
            if (!string.IsNullOrEmpty(ViewModel.ProjectSite))
            {
                lProjectSite.Click += (s, e) =>
                {
                    Process.Start(url);
                };
            }
            else
            {
                link.Enabled = false;
            }
        }
    }

    public partial class PackageDetailsViewProxy : ViewUserControl<NuGetPackageViewModel>
    {

    }
}
