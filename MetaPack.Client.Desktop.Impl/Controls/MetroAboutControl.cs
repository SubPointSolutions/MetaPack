using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SubPointSolutions.Shelly.Core;

namespace MetaPack.Client.Desktop.Impl.Controls
{
    public partial class MetroAboutControl : UserControl
    {
        public MetroAboutControl()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            this.Text = string.Format("About");

            lAppTitle.Text = ShServiceContainer.Instance.AppMetadataService.AppName;
            lAppVersion.Text = string.Format("Version: {0}", ShServiceContainer.Instance.AppMetadataService.AppVersion);
        }
    }
}
