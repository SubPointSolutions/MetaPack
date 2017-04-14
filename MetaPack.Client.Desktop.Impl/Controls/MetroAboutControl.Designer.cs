namespace MetaPack.Client.Desktop.Impl.Controls
{
    partial class MetroAboutControl
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
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.lAppTitle = new MetroFramework.Controls.MetroLabel();
            this.lAppVersion = new MetroFramework.Controls.MetroLabel();
            this.metroPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroPanel1
            // 
            this.metroPanel1.Controls.Add(this.lAppVersion);
            this.metroPanel1.Controls.Add(this.lAppTitle);
            this.metroPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(0, 0);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.metroPanel1.Size = new System.Drawing.Size(427, 224);
            this.metroPanel1.TabIndex = 0;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // lAppTitle
            // 
            this.lAppTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lAppTitle.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lAppTitle.Location = new System.Drawing.Point(15, 10);
            this.lAppTitle.Name = "lAppTitle";
            this.lAppTitle.Size = new System.Drawing.Size(399, 23);
            this.lAppTitle.TabIndex = 2;
            this.lAppTitle.Text = "App Title";
            // 
            // lAppVersion
            // 
            this.lAppVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lAppVersion.Location = new System.Drawing.Point(15, 41);
            this.lAppVersion.Name = "lAppVersion";
            this.lAppVersion.Size = new System.Drawing.Size(399, 19);
            this.lAppVersion.TabIndex = 3;
            this.lAppVersion.Text = "App Version";
            // 
            // MetroAboutControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.metroPanel1);
            this.Name = "MetroAboutControl";
            this.Size = new System.Drawing.Size(427, 224);
            this.metroPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroLabel lAppVersion;
        private MetroFramework.Controls.MetroLabel lAppTitle;
    }
}
