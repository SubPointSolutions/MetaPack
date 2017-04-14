using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SubPointSolutions.Shelly.Desktop.Controls;
using MetaPack.Client.Desktop.Impl.Data;
using SubPointSolutions.Shelly.Core.Utils;
using SubPointSolutions.Shelly.Desktop.Services;

namespace MetaPack.Client.Desktop.Impl.Views
{
    public partial class SharePointConnectionViewControl : ShUserControlBase
    {
        #region constructors
        public SharePointConnectionViewControl()
        {
            InitializeComponent();

            if (metroComboBox1.Items.Count > 0)
                metroComboBox1.SelectedIndex = 0;

        }
        #endregion


        #region methods

        #endregion
    }
}
