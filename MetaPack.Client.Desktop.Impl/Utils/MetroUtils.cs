using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetaPack.Client.Desktop.Impl.Controls;
using MetroFramework.Controls;
using MetroFramework.Forms;
using SubPointSolutions.Shelly.Desktop.Extensions;
using SubPointSolutions.Shelly.Desktop.Interfaces;
using SubPointSolutions.Shelly.Desktop.Services;
using SubPointSolutions.Shelly.Desktop.Utils;
using SubPointSolutions.Shelly.Desktop.Controls;

namespace MetaPack.Client.Desktop.Impl.Utils
{
    internal static class MetroUtils
    {
        internal static MetroGrid SetDataSource(this MetroGrid grData, object data)
        {
            grData.DataSource = data;

            return grData;
        }

        public static MetroGrid SetDefaultGridLookAndFeel(this MetroGrid grid)
        {
            grid.AutoGenerateColumns = false;

            grid.EditMode = DataGridViewEditMode.EditProgrammatically;

            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.MultiSelect = false;

            return grid;
        }

        internal static MetroGrid AddDefaultNuGetPackageFields(this MetroGrid grid)
        {
            grid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Title",
                HeaderText = "Title",
                DataPropertyName = "Title"
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Id",
                HeaderText = "Id",
                DataPropertyName = "Id"
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Authors",
                HeaderText = "Authors",
                DataPropertyName = "Authors"
            });

            grid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Version",
                HeaderText = "Version",
                DataPropertyName = "Version"
            });

            return grid;
        }

        public static void WithLongOperation(Action<ShBusinessEntityDataService.LongOperationHandle> action)
        {
            var currentAppForm = Form.ActiveForm;

            WithLongOperation(currentAppForm, action);
        }

        public static void WithLongOperation(
            Form parentForm,
            Action<ShBusinessEntityDataService.LongOperationHandle> action)
        {
            WithLongOperation(parentForm, null, action);
        }

        public static void WithLongOperation(
            Form parentForm,
            Action<Form> progressForm,
            Action<ShBusinessEntityDataService.LongOperationHandle> action)
        {
            var form = GetProgressForm(parentForm, f =>
            {
                f.Text = "Working...";
            });

            if (progressForm != null)
                progressForm(form);

            var hasClose = false;

            try
            {
                form.ShowIcon = false;
                form.ShowInTaskbar = false;

                var longOperationControl = ShUtils.GetUIComponentType<ILongOperationControl>();
                var instance = Activator.CreateInstance(longOperationControl);
                var instanceControl = instance as ShUserControlBase;

                instanceControl.Dock = DockStyle.Fill;

                form.MaximizeBox = false;
                form.MinimizeBox = false;

                //form.StartPosition = FormStartPosition.CenterScreen;

                form.Controls.Add(instanceControl);

                form.Shown += (sss, eee) =>
                {
                    var handle = new ShBusinessEntityDataService.LongOperationHandle();

                    handle.OnOutput = (m) =>
                    {
                        var cc = (MetroLongOperationControl)instance;
                        cc.SetLogMessage(m);
                    };

                    handle.OnEnd += (rr, tt) =>
                    {
                        form.WithSafeUIUpdate(() =>
                        {
                            form.DialogResult = DialogResult.OK;
                            form.Close();
                        });

                        hasClose = true;
                    };

                    action(handle);
                };

                form.ShowDialog();

                hasClose = true;
            }
            catch (Exception longOperationException)
            {
                if (!hasClose)
                {
                    form.WithSafeUIUpdate(() =>
                    {
                        form.DialogResult = DialogResult.OK;
                        form.Close();
                    });
                }

                throw longOperationException;
            }
        }

        private static Form GetProgressForm(Form parentForm, Action<Form> setup)
        {
            var control = new MetroForm();

            control.ShowInTaskbar = false;
            control.MaximizeBox = false;
            control.MinimizeBox = false;

            //var spinner = new MetroProgressBar();

            //spinner.ProgressBarStyle = ProgressBarStyle.Marquee;
            //spinner.Enabled = true;
            //spinner.Dock = DockStyle.Top;

            //control.Controls.Add(spinner);
            control.ControlBox = false;

            control.Size = new Size(parentForm.Width, control.Height);
            control.StartPosition = FormStartPosition.CenterParent;

            if (setup != null)
                setup(control);

            return control;
        }

        //public static Control WithProgressDialog(Form parentForm, string message,
        //    Action workAction,
        //    Action<Control> doneAction)
        //{


        //    control.Shown += (ss, ee) =>
        //    {
        //        workAction();
        //    };

        //    control.ShowDialog();
        //    return control;
        //}
    }
}
