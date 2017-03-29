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
using SubPointSolutions.Shelly.Desktop.Interfaces;

namespace MetaPack.Client.Desktop.Impl.Controls
{
    public partial class MetroLongOperationControl : ShUserControlBase, ILongOperationControl
    {
        public MetroLongOperationControl()
        {
            InitializeComponent();
        }

        public void SetLogMessage(string message)
        {
            this.WithSafeUIUpdate(() =>
            {
                lbOutput.BeginUpdate();

                // output
                lbOutput.Items.Add(message);

                if (lbOutput.Items.Count > 0)
                    lbOutput.SelectedIndex = lbOutput.Items.Count - 1;

                lbOutput.EndUpdate();
            });
        }
    }
}
