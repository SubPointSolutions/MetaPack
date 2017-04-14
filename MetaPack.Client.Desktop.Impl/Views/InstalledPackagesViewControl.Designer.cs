namespace MetaPack.Client.Desktop.Impl.Views
{
    partial class InstalledPackagesViewControl
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
            this.pActions = new MetroFramework.Controls.MetroPanel();
            this.lSwitchConnection = new MetroFramework.Controls.MetroLink();
            this.lAddSharePointConnection = new MetroFramework.Controls.MetroLink();
            this.pTopActions = new MetroFramework.Controls.MetroPanel();
            this.cbSharePointConnection = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.cbShowWithUpdates = new MetroFramework.Controls.MetroCheckBox();
            this.tbSearch = new MetroFramework.Controls.MetroTextBox();
            this.bRefresh = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.grData)).BeginInit();
            this.cmData.SuspendLayout();
            this.pActions.SuspendLayout();
            this.pTopActions.SuspendLayout();
            this.metroPanel1.SuspendLayout();
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
            this.grData.Location = new System.Drawing.Point(0, 80);
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
            this.grData.Size = new System.Drawing.Size(725, 319);
            this.grData.TabIndex = 0;
            // 
            // cmData
            // 
            this.cmData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsViewPackageDetails});
            this.cmData.Name = "cmData";
            this.cmData.Size = new System.Drawing.Size(138, 26);
            // 
            // tsViewPackageDetails
            // 
            this.tsViewPackageDetails.Name = "tsViewPackageDetails";
            this.tsViewPackageDetails.Size = new System.Drawing.Size(137, 22);
            this.tsViewPackageDetails.Text = "View Details";
            // 
            // pActions
            // 
            this.pActions.Controls.Add(this.lSwitchConnection);
            this.pActions.Controls.Add(this.lAddSharePointConnection);
            this.pActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pActions.HorizontalScrollbarBarColor = true;
            this.pActions.HorizontalScrollbarHighlightOnWheel = false;
            this.pActions.HorizontalScrollbarSize = 10;
            this.pActions.Location = new System.Drawing.Point(0, 399);
            this.pActions.Name = "pActions";
            this.pActions.Size = new System.Drawing.Size(725, 31);
            this.pActions.TabIndex = 1;
            this.pActions.VerticalScrollbarBarColor = true;
            this.pActions.VerticalScrollbarHighlightOnWheel = false;
            this.pActions.VerticalScrollbarSize = 10;
            this.pActions.Visible = false;
            // 
            // lSwitchConnection
            // 
            this.lSwitchConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lSwitchConnection.Location = new System.Drawing.Point(161, 3);
            this.lSwitchConnection.Name = "lSwitchConnection";
            this.lSwitchConnection.Size = new System.Drawing.Size(561, 23);
            this.lSwitchConnection.TabIndex = 3;
            this.lSwitchConnection.Text = "Switch active SharePoint connection";
            this.lSwitchConnection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lSwitchConnection.UseSelectable = true;
            this.lSwitchConnection.Visible = false;
            // 
            // lAddSharePointConnection
            // 
            this.lAddSharePointConnection.Location = new System.Drawing.Point(0, 3);
            this.lAddSharePointConnection.Name = "lAddSharePointConnection";
            this.lAddSharePointConnection.Size = new System.Drawing.Size(168, 23);
            this.lAddSharePointConnection.TabIndex = 2;
            this.lAddSharePointConnection.Text = "Add SharePoint connection |";
            this.lAddSharePointConnection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lAddSharePointConnection.UseSelectable = true;
            // 
            // pTopActions
            // 
            this.pTopActions.Controls.Add(this.cbSharePointConnection);
            this.pTopActions.Controls.Add(this.metroLabel1);
            this.pTopActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTopActions.HorizontalScrollbarBarColor = true;
            this.pTopActions.HorizontalScrollbarHighlightOnWheel = false;
            this.pTopActions.HorizontalScrollbarSize = 10;
            this.pTopActions.Location = new System.Drawing.Point(0, 40);
            this.pTopActions.Name = "pTopActions";
            this.pTopActions.Size = new System.Drawing.Size(725, 40);
            this.pTopActions.TabIndex = 2;
            this.pTopActions.VerticalScrollbarBarColor = true;
            this.pTopActions.VerticalScrollbarHighlightOnWheel = false;
            this.pTopActions.VerticalScrollbarSize = 10;
            // 
            // cbSharePointConnection
            // 
            this.cbSharePointConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSharePointConnection.FormattingEnabled = true;
            this.cbSharePointConnection.ItemHeight = 23;
            this.cbSharePointConnection.Location = new System.Drawing.Point(110, 5);
            this.cbSharePointConnection.Name = "cbSharePointConnection";
            this.cbSharePointConnection.Size = new System.Drawing.Size(612, 29);
            this.cbSharePointConnection.TabIndex = 8;
            this.cbSharePointConnection.UseSelectable = true;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel1.Location = new System.Drawing.Point(2, 9);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(102, 19);
            this.metroLabel1.TabIndex = 7;
            this.metroLabel1.Text = "SharePoint site:";
            // 
            // metroPanel1
            // 
            this.metroPanel1.Controls.Add(this.bRefresh);
            this.metroPanel1.Controls.Add(this.metroLabel2);
            this.metroPanel1.Controls.Add(this.cbShowWithUpdates);
            this.metroPanel1.Controls.Add(this.tbSearch);
            this.metroPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(0, 0);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(725, 40);
            this.metroPanel1.TabIndex = 5;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel2.Location = new System.Drawing.Point(3, 7);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(106, 19);
            this.metroLabel2.TabIndex = 4;
            this.metroLabel2.Text = "Search package:";
            // 
            // cbShowWithUpdates
            // 
            this.cbShowWithUpdates.AutoSize = true;
            this.cbShowWithUpdates.Location = new System.Drawing.Point(366, 11);
            this.cbShowWithUpdates.Name = "cbShowWithUpdates";
            this.cbShowWithUpdates.Size = new System.Drawing.Size(204, 15);
            this.cbShowWithUpdates.TabIndex = 3;
            this.cbShowWithUpdates.Text = "Show Only Packages with Updates";
            this.cbShowWithUpdates.UseSelectable = true;
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
            this.bRefresh.Location = new System.Drawing.Point(576, 7);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(100, 23);
            this.bRefresh.TabIndex = 6;
            this.bRefresh.Text = "Refresh";
            this.bRefresh.UseSelectable = true;
            // 
            // InstalledPackagesViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grData);
            this.Controls.Add(this.pTopActions);
            this.Controls.Add(this.metroPanel1);
            this.Controls.Add(this.pActions);
            this.Name = "InstalledPackagesViewControl";
            this.Size = new System.Drawing.Size(725, 430);
            ((System.ComponentModel.ISupportInitialize)(this.grData)).EndInit();
            this.cmData.ResumeLayout(false);
            this.pActions.ResumeLayout(false);
            this.pTopActions.ResumeLayout(false);
            this.pTopActions.PerformLayout();
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroGrid grData;
        private MetroFramework.Controls.MetroPanel pActions;
        private MetroFramework.Controls.MetroLink lAddSharePointConnection;
        private MetroFramework.Controls.MetroLink lSwitchConnection;
        private MetroFramework.Controls.MetroContextMenu cmData;
        private System.Windows.Forms.ToolStripMenuItem tsViewPackageDetails;
        private MetroFramework.Controls.MetroPanel pTopActions;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroComboBox cbSharePointConnection;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroCheckBox cbShowWithUpdates;
        private MetroFramework.Controls.MetroTextBox tbSearch;
        private MetroFramework.Controls.MetroButton bRefresh;

    }
}
