namespace MetaPack.Client.Desktop.Impl.Views
{
    partial class OptionsViewControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tcOptions = new MetroFramework.Controls.MetroTabControl();
            this.tpSharePoint = new MetroFramework.Controls.MetroTabPage();
            this.lSharePoint = new MetroFramework.Controls.MetroLink();
            this.gcSharePoint = new MetroFramework.Controls.MetroGrid();
            this.cmSharePoint = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.tsSharePointAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSharePointEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSharePointDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tpNuGet = new MetroFramework.Controls.MetroTabPage();
            this.lNuGet = new MetroFramework.Controls.MetroLink();
            this.gcNuGet = new MetroFramework.Controls.MetroGrid();
            this.cmNuGet = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.tsNuGetAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.tsNuGetEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsNuGetDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tpMisc = new MetroFramework.Controls.MetroTabPage();
            this.cbLoadAvailablePackagesOnStart = new MetroFramework.Controls.MetroCheckBox();
            this.tcOptions.SuspendLayout();
            this.tpSharePoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcSharePoint)).BeginInit();
            this.cmSharePoint.SuspendLayout();
            this.tpNuGet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcNuGet)).BeginInit();
            this.cmNuGet.SuspendLayout();
            this.tpMisc.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcOptions
            // 
            this.tcOptions.Controls.Add(this.tpSharePoint);
            this.tcOptions.Controls.Add(this.tpNuGet);
            this.tcOptions.Controls.Add(this.tpMisc);
            this.tcOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcOptions.Location = new System.Drawing.Point(0, 0);
            this.tcOptions.Name = "tcOptions";
            this.tcOptions.SelectedIndex = 2;
            this.tcOptions.Size = new System.Drawing.Size(500, 300);
            this.tcOptions.TabIndex = 1;
            this.tcOptions.UseSelectable = true;
            // 
            // tpSharePoint
            // 
            this.tpSharePoint.Controls.Add(this.lSharePoint);
            this.tpSharePoint.Controls.Add(this.gcSharePoint);
            this.tpSharePoint.HorizontalScrollbarBarColor = true;
            this.tpSharePoint.HorizontalScrollbarHighlightOnWheel = false;
            this.tpSharePoint.HorizontalScrollbarSize = 10;
            this.tpSharePoint.Location = new System.Drawing.Point(4, 38);
            this.tpSharePoint.Name = "tpSharePoint";
            this.tpSharePoint.Size = new System.Drawing.Size(492, 258);
            this.tpSharePoint.TabIndex = 0;
            this.tpSharePoint.Text = "SharePoint ";
            this.tpSharePoint.VerticalScrollbarBarColor = true;
            this.tpSharePoint.VerticalScrollbarHighlightOnWheel = false;
            this.tpSharePoint.VerticalScrollbarSize = 10;
            // 
            // lSharePoint
            // 
            this.lSharePoint.Location = new System.Drawing.Point(3, 232);
            this.lSharePoint.Name = "lSharePoint";
            this.lSharePoint.Size = new System.Drawing.Size(220, 23);
            this.lSharePoint.TabIndex = 26;
            this.lSharePoint.Text = "Add SharePoint connection...";
            this.lSharePoint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lSharePoint.UseSelectable = true;
            // 
            // gcSharePoint
            // 
            this.gcSharePoint.AllowUserToResizeRows = false;
            this.gcSharePoint.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcSharePoint.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gcSharePoint.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gcSharePoint.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gcSharePoint.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gcSharePoint.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(211)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gcSharePoint.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.gcSharePoint.ColumnHeadersHeight = 30;
            this.gcSharePoint.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gcSharePoint.ContextMenuStrip = this.cmSharePoint;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(211)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gcSharePoint.DefaultCellStyle = dataGridViewCellStyle8;
            this.gcSharePoint.EnableHeadersVisualStyles = false;
            this.gcSharePoint.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gcSharePoint.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gcSharePoint.HighLightPercentage = 0.5F;
            this.gcSharePoint.Location = new System.Drawing.Point(3, 3);
            this.gcSharePoint.Name = "gcSharePoint";
            this.gcSharePoint.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(211)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gcSharePoint.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.gcSharePoint.RowHeadersVisible = false;
            this.gcSharePoint.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gcSharePoint.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gcSharePoint.Size = new System.Drawing.Size(486, 223);
            this.gcSharePoint.TabIndex = 25;
            // 
            // cmSharePoint
            // 
            this.cmSharePoint.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsSharePointAdd,
            this.tsSharePointEdit,
            this.tsSharePointDelete});
            this.cmSharePoint.Name = "cmSharePoint";
            this.cmSharePoint.Size = new System.Drawing.Size(108, 70);
            // 
            // tsSharePointAdd
            // 
            this.tsSharePointAdd.Name = "tsSharePointAdd";
            this.tsSharePointAdd.Size = new System.Drawing.Size(107, 22);
            this.tsSharePointAdd.Text = "Add";
            // 
            // tsSharePointEdit
            // 
            this.tsSharePointEdit.Name = "tsSharePointEdit";
            this.tsSharePointEdit.Size = new System.Drawing.Size(107, 22);
            this.tsSharePointEdit.Text = "Edit";
            // 
            // tsSharePointDelete
            // 
            this.tsSharePointDelete.Name = "tsSharePointDelete";
            this.tsSharePointDelete.Size = new System.Drawing.Size(107, 22);
            this.tsSharePointDelete.Text = "Delete";
            // 
            // tpNuGet
            // 
            this.tpNuGet.Controls.Add(this.lNuGet);
            this.tpNuGet.Controls.Add(this.gcNuGet);
            this.tpNuGet.HorizontalScrollbarBarColor = true;
            this.tpNuGet.HorizontalScrollbarHighlightOnWheel = false;
            this.tpNuGet.HorizontalScrollbarSize = 10;
            this.tpNuGet.Location = new System.Drawing.Point(4, 38);
            this.tpNuGet.Name = "tpNuGet";
            this.tpNuGet.Size = new System.Drawing.Size(492, 258);
            this.tpNuGet.TabIndex = 1;
            this.tpNuGet.Text = "NuGet Galleries";
            this.tpNuGet.VerticalScrollbarBarColor = true;
            this.tpNuGet.VerticalScrollbarHighlightOnWheel = false;
            this.tpNuGet.VerticalScrollbarSize = 10;
            // 
            // lNuGet
            // 
            this.lNuGet.Location = new System.Drawing.Point(3, 232);
            this.lNuGet.Name = "lNuGet";
            this.lNuGet.Size = new System.Drawing.Size(220, 23);
            this.lNuGet.TabIndex = 24;
            this.lNuGet.Text = "Add NuGet Gallery connection...";
            this.lNuGet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lNuGet.UseSelectable = true;
            // 
            // gcNuGet
            // 
            this.gcNuGet.AllowUserToAddRows = false;
            this.gcNuGet.AllowUserToDeleteRows = false;
            this.gcNuGet.AllowUserToResizeRows = false;
            this.gcNuGet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcNuGet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gcNuGet.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gcNuGet.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gcNuGet.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gcNuGet.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(211)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gcNuGet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gcNuGet.ColumnHeadersHeight = 30;
            this.gcNuGet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gcNuGet.ContextMenuStrip = this.cmNuGet;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(211)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gcNuGet.DefaultCellStyle = dataGridViewCellStyle2;
            this.gcNuGet.EnableHeadersVisualStyles = false;
            this.gcNuGet.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gcNuGet.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gcNuGet.HighLightPercentage = 0.5F;
            this.gcNuGet.Location = new System.Drawing.Point(3, 3);
            this.gcNuGet.Name = "gcNuGet";
            this.gcNuGet.ReadOnly = true;
            this.gcNuGet.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(211)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gcNuGet.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gcNuGet.RowHeadersVisible = false;
            this.gcNuGet.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gcNuGet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gcNuGet.Size = new System.Drawing.Size(486, 223);
            this.gcNuGet.TabIndex = 14;
            // 
            // cmNuGet
            // 
            this.cmNuGet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsNuGetAdd,
            this.tsNuGetEdit,
            this.tsNuGetDelete});
            this.cmNuGet.Name = "cmNuGet";
            this.cmNuGet.Size = new System.Drawing.Size(108, 70);
            // 
            // tsNuGetAdd
            // 
            this.tsNuGetAdd.Name = "tsNuGetAdd";
            this.tsNuGetAdd.Size = new System.Drawing.Size(107, 22);
            this.tsNuGetAdd.Text = "Add";
            // 
            // tsNuGetEdit
            // 
            this.tsNuGetEdit.Name = "tsNuGetEdit";
            this.tsNuGetEdit.Size = new System.Drawing.Size(107, 22);
            this.tsNuGetEdit.Text = "Edit";
            // 
            // tsNuGetDelete
            // 
            this.tsNuGetDelete.Name = "tsNuGetDelete";
            this.tsNuGetDelete.Size = new System.Drawing.Size(107, 22);
            this.tsNuGetDelete.Text = "Delete";
            // 
            // tpMisc
            // 
            this.tpMisc.Controls.Add(this.cbLoadAvailablePackagesOnStart);
            this.tpMisc.HorizontalScrollbarBarColor = true;
            this.tpMisc.HorizontalScrollbarHighlightOnWheel = false;
            this.tpMisc.HorizontalScrollbarSize = 10;
            this.tpMisc.Location = new System.Drawing.Point(4, 38);
            this.tpMisc.Name = "tpMisc";
            this.tpMisc.Size = new System.Drawing.Size(492, 258);
            this.tpMisc.TabIndex = 2;
            this.tpMisc.Text = "Miscellaneous";
            this.tpMisc.VerticalScrollbarBarColor = true;
            this.tpMisc.VerticalScrollbarHighlightOnWheel = false;
            this.tpMisc.VerticalScrollbarSize = 10;
            // 
            // cbLoadAvailablePackagesOnStart
            // 
            this.cbLoadAvailablePackagesOnStart.AutoSize = true;
            this.cbLoadAvailablePackagesOnStart.Location = new System.Drawing.Point(15, 16);
            this.cbLoadAvailablePackagesOnStart.Name = "cbLoadAvailablePackagesOnStart";
            this.cbLoadAvailablePackagesOnStart.Size = new System.Drawing.Size(193, 15);
            this.cbLoadAvailablePackagesOnStart.TabIndex = 2;
            this.cbLoadAvailablePackagesOnStart.Text = "Load available packages on start";
            this.cbLoadAvailablePackagesOnStart.UseSelectable = true;
            // 
            // OptionsViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcOptions);
            this.Name = "OptionsViewControl";
            this.Size = new System.Drawing.Size(500, 300);
            this.tcOptions.ResumeLayout(false);
            this.tpSharePoint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcSharePoint)).EndInit();
            this.cmSharePoint.ResumeLayout(false);
            this.tpNuGet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcNuGet)).EndInit();
            this.cmNuGet.ResumeLayout(false);
            this.tpMisc.ResumeLayout(false);
            this.tpMisc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTabControl tcOptions;
        private MetroFramework.Controls.MetroTabPage tpSharePoint;
        private MetroFramework.Controls.MetroLink lSharePoint;
        private MetroFramework.Controls.MetroGrid gcSharePoint;
        private MetroFramework.Controls.MetroTabPage tpNuGet;
        private MetroFramework.Controls.MetroLink lNuGet;
        private MetroFramework.Controls.MetroGrid gcNuGet;
        private MetroFramework.Controls.MetroTabPage tpMisc;
        private MetroFramework.Controls.MetroContextMenu cmNuGet;
        private System.Windows.Forms.ToolStripMenuItem tsNuGetEdit;
        private System.Windows.Forms.ToolStripMenuItem tsNuGetDelete;
        private System.Windows.Forms.ToolStripMenuItem tsNuGetAdd;
        private MetroFramework.Controls.MetroContextMenu cmSharePoint;
        private System.Windows.Forms.ToolStripMenuItem tsSharePointAdd;
        private System.Windows.Forms.ToolStripMenuItem tsSharePointEdit;
        private System.Windows.Forms.ToolStripMenuItem tsSharePointDelete;
        private MetroFramework.Controls.MetroCheckBox cbLoadAvailablePackagesOnStart;
    }
}
