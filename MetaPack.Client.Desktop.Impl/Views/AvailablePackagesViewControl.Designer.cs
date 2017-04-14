namespace MetaPack.Client.Desktop.Impl.Views
{
    partial class AvailablePackagesViewControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grData = new MetroFramework.Controls.MetroGrid();
            this.cmData = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.tsViewPackageDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.tsDeployPackage = new System.Windows.Forms.ToolStripMenuItem();
            this.pActions = new MetroFramework.Controls.MetroPanel();
            this.lRefresh = new MetroFramework.Controls.MetroLink();
            this.lAddNuGetConnection = new MetroFramework.Controls.MetroLink();
            this.pTopActions = new MetroFramework.Controls.MetroPanel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.cbIncludePrerelease = new MetroFramework.Controls.MetroCheckBox();
            this.tbSearch = new MetroFramework.Controls.MetroTextBox();
            this.bRefresh = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.grData)).BeginInit();
            this.cmData.SuspendLayout();
            this.pActions.SuspendLayout();
            this.pTopActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // grData
            // 
            this.grData.AllowUserToAddRows = false;
            this.grData.AllowUserToDeleteRows = false;
            this.grData.AllowUserToResizeRows = false;
            this.grData.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grData.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.grData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grData.ContextMenuStrip = this.cmData;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grData.DefaultCellStyle = dataGridViewCellStyle2;
            this.grData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grData.EnableHeadersVisualStyles = false;
            this.grData.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grData.Location = new System.Drawing.Point(0, 40);
            this.grData.Name = "grData";
            this.grData.ReadOnly = true;
            this.grData.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grData.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grData.Size = new System.Drawing.Size(691, 357);
            this.grData.TabIndex = 2;
            // 
            // cmData
            // 
            this.cmData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsViewPackageDetails,
            this.tsDeployPackage});
            this.cmData.Name = "cmData";
            this.cmData.Size = new System.Drawing.Size(138, 48);
            // 
            // tsViewPackageDetails
            // 
            this.tsViewPackageDetails.Name = "tsViewPackageDetails";
            this.tsViewPackageDetails.Size = new System.Drawing.Size(137, 22);
            this.tsViewPackageDetails.Text = "View Details";
            // 
            // tsDeployPackage
            // 
            this.tsDeployPackage.Name = "tsDeployPackage";
            this.tsDeployPackage.Size = new System.Drawing.Size(137, 22);
            this.tsDeployPackage.Text = "Deploy";
            // 
            // pActions
            // 
            this.pActions.Controls.Add(this.lRefresh);
            this.pActions.Controls.Add(this.lAddNuGetConnection);
            this.pActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pActions.HorizontalScrollbarBarColor = true;
            this.pActions.HorizontalScrollbarHighlightOnWheel = false;
            this.pActions.HorizontalScrollbarSize = 10;
            this.pActions.Location = new System.Drawing.Point(0, 397);
            this.pActions.Name = "pActions";
            this.pActions.Size = new System.Drawing.Size(691, 31);
            this.pActions.TabIndex = 3;
            this.pActions.VerticalScrollbarBarColor = true;
            this.pActions.VerticalScrollbarHighlightOnWheel = false;
            this.pActions.VerticalScrollbarSize = 10;
            this.pActions.Visible = false;
            // 
            // lRefresh
            // 
            this.lRefresh.Location = new System.Drawing.Point(116, 3);
            this.lRefresh.Name = "lRefresh";
            this.lRefresh.Size = new System.Drawing.Size(122, 23);
            this.lRefresh.TabIndex = 3;
            this.lRefresh.Text = "Refresh";
            this.lRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lRefresh.UseSelectable = true;
            // 
            // lAddNuGetConnection
            // 
            this.lAddNuGetConnection.Location = new System.Drawing.Point(0, 3);
            this.lAddNuGetConnection.Name = "lAddNuGetConnection";
            this.lAddNuGetConnection.Size = new System.Drawing.Size(122, 23);
            this.lAddNuGetConnection.TabIndex = 2;
            this.lAddNuGetConnection.Text = "Add NuGet Gallery |";
            this.lAddNuGetConnection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lAddNuGetConnection.UseSelectable = true;
            // 
            // pTopActions
            // 
            this.pTopActions.Controls.Add(this.bRefresh);
            this.pTopActions.Controls.Add(this.metroLabel1);
            this.pTopActions.Controls.Add(this.cbIncludePrerelease);
            this.pTopActions.Controls.Add(this.tbSearch);
            this.pTopActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTopActions.HorizontalScrollbarBarColor = true;
            this.pTopActions.HorizontalScrollbarHighlightOnWheel = false;
            this.pTopActions.HorizontalScrollbarSize = 10;
            this.pTopActions.Location = new System.Drawing.Point(0, 0);
            this.pTopActions.Name = "pTopActions";
            this.pTopActions.Size = new System.Drawing.Size(691, 40);
            this.pTopActions.TabIndex = 4;
            this.pTopActions.VerticalScrollbarBarColor = true;
            this.pTopActions.VerticalScrollbarHighlightOnWheel = false;
            this.pTopActions.VerticalScrollbarSize = 10;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel1.Location = new System.Drawing.Point(3, 7);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(106, 19);
            this.metroLabel1.TabIndex = 4;
            this.metroLabel1.Text = "Search package:";
            // 
            // cbIncludePrerelease
            // 
            this.cbIncludePrerelease.AutoSize = true;
            this.cbIncludePrerelease.Location = new System.Drawing.Point(366, 11);
            this.cbIncludePrerelease.Name = "cbIncludePrerelease";
            this.cbIncludePrerelease.Size = new System.Drawing.Size(118, 15);
            this.cbIncludePrerelease.TabIndex = 3;
            this.cbIncludePrerelease.Text = "Include Prerelease";
            this.cbIncludePrerelease.UseSelectable = true;
            // 
            // tbSearch
            // 
            // 
            // 
            // 
            this.tbSearch.CustomButton.Image = null;
            this.tbSearch.CustomButton.Location = new System.Drawing.Point(228, 1);
            this.tbSearch.CustomButton.Name = "";
            this.tbSearch.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.tbSearch.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbSearch.CustomButton.TabIndex = 1;
            this.tbSearch.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbSearch.CustomButton.UseSelectable = true;
            this.tbSearch.CustomButton.Visible = false;
            this.tbSearch.Lines = new string[0];
            this.tbSearch.Location = new System.Drawing.Point(110, 7);
            this.tbSearch.MaxLength = 32767;
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.PasswordChar = '\0';
            this.tbSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbSearch.SelectedText = "";
            this.tbSearch.SelectionLength = 0;
            this.tbSearch.SelectionStart = 0;
            this.tbSearch.ShortcutsEnabled = true;
            this.tbSearch.Size = new System.Drawing.Size(250, 23);
            this.tbSearch.TabIndex = 2;
            this.tbSearch.UseSelectable = true;
            this.tbSearch.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.tbSearch.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // bRefresh
            // 
            this.bRefresh.Location = new System.Drawing.Point(490, 7);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(100, 23);
            this.bRefresh.TabIndex = 5;
            this.bRefresh.Text = "Refresh";
            this.bRefresh.UseSelectable = true;
            // 
            // AvailablePackagesViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grData);
            this.Controls.Add(this.pTopActions);
            this.Controls.Add(this.pActions);
            this.Name = "AvailablePackagesViewControl";
            this.Size = new System.Drawing.Size(691, 428);
            ((System.ComponentModel.ISupportInitialize)(this.grData)).EndInit();
            this.cmData.ResumeLayout(false);
            this.pActions.ResumeLayout(false);
            this.pTopActions.ResumeLayout(false);
            this.pTopActions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroGrid grData;
        private MetroFramework.Controls.MetroPanel pActions;
        private MetroFramework.Controls.MetroLink lAddNuGetConnection;
        private MetroFramework.Controls.MetroContextMenu cmData;
        private MetroFramework.Controls.MetroLink lRefresh;
        private System.Windows.Forms.ToolStripMenuItem tsViewPackageDetails;
        private System.Windows.Forms.ToolStripMenuItem tsDeployPackage;
        private MetroFramework.Controls.MetroPanel pTopActions;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroCheckBox cbIncludePrerelease;
        private MetroFramework.Controls.MetroTextBox tbSearch;
        private MetroFramework.Controls.MetroButton bRefresh;

    }
}
