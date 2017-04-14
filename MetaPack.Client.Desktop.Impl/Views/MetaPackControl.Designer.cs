namespace MetaPack.Client.Desktop.Impl.Controls
{
    partial class MetaPackControl
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("");
            this.tvConnections = new System.Windows.Forms.TreeView();
            this.pCenter = new System.Windows.Forms.Panel();
            this.lvPackages = new System.Windows.Forms.ListView();
            this.pNuGetActions = new System.Windows.Forms.Panel();
            this.bUpdate = new System.Windows.Forms.Button();
            this.bInstall = new System.Windows.Forms.Button();
            this.bNuGetRefresh = new System.Windows.Forms.Button();
            this.pRight = new System.Windows.Forms.Panel();
            this.gbPackageInfo = new System.Windows.Forms.GroupBox();
            this.llProjectUrl = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbAuthors = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbId = new System.Windows.Forms.TextBox();
            this.lCreated = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pLeft = new System.Windows.Forms.Panel();
            this.gbConnections = new System.Windows.Forms.GroupBox();
            this.gbPackages = new System.Windows.Forms.GroupBox();
            this.pCenter.SuspendLayout();
            this.pNuGetActions.SuspendLayout();
            this.pRight.SuspendLayout();
            this.gbPackageInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.pLeft.SuspendLayout();
            this.gbConnections.SuspendLayout();
            this.gbPackages.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvConnections
            // 
            this.tvConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvConnections.Location = new System.Drawing.Point(5, 18);
            this.tvConnections.Name = "tvConnections";
            this.tvConnections.Size = new System.Drawing.Size(202, 560);
            this.tvConnections.TabIndex = 0;
            // 
            // pCenter
            // 
            this.pCenter.Controls.Add(this.gbPackages);
            this.pCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pCenter.Location = new System.Drawing.Point(225, 0);
            this.pCenter.Name = "pCenter";
            this.pCenter.Padding = new System.Windows.Forms.Padding(5);
            this.pCenter.Size = new System.Drawing.Size(523, 593);
            this.pCenter.TabIndex = 2;
            // 
            // lvPackages
            // 
            this.lvPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPackages.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4});
            this.lvPackages.Location = new System.Drawing.Point(5, 48);
            this.lvPackages.Name = "lvPackages";
            this.lvPackages.Size = new System.Drawing.Size(503, 530);
            this.lvPackages.TabIndex = 0;
            this.lvPackages.UseCompatibleStateImageBehavior = false;
            this.lvPackages.View = System.Windows.Forms.View.List;
            // 
            // pNuGetActions
            // 
            this.pNuGetActions.Controls.Add(this.bUpdate);
            this.pNuGetActions.Controls.Add(this.bInstall);
            this.pNuGetActions.Controls.Add(this.bNuGetRefresh);
            this.pNuGetActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pNuGetActions.Location = new System.Drawing.Point(5, 18);
            this.pNuGetActions.Name = "pNuGetActions";
            this.pNuGetActions.Size = new System.Drawing.Size(503, 30);
            this.pNuGetActions.TabIndex = 1;
            // 
            // bUpdate
            // 
            this.bUpdate.Location = new System.Drawing.Point(162, 3);
            this.bUpdate.Name = "bUpdate";
            this.bUpdate.Size = new System.Drawing.Size(75, 23);
            this.bUpdate.TabIndex = 0;
            this.bUpdate.Text = "Update";
            this.bUpdate.UseVisualStyleBackColor = true;
            // 
            // bInstall
            // 
            this.bInstall.Location = new System.Drawing.Point(81, 3);
            this.bInstall.Name = "bInstall";
            this.bInstall.Size = new System.Drawing.Size(75, 23);
            this.bInstall.TabIndex = 0;
            this.bInstall.Text = "Install";
            this.bInstall.UseVisualStyleBackColor = true;
            // 
            // bNuGetRefresh
            // 
            this.bNuGetRefresh.Location = new System.Drawing.Point(0, 3);
            this.bNuGetRefresh.Name = "bNuGetRefresh";
            this.bNuGetRefresh.Size = new System.Drawing.Size(75, 23);
            this.bNuGetRefresh.TabIndex = 0;
            this.bNuGetRefresh.Text = "Refresh";
            this.bNuGetRefresh.UseVisualStyleBackColor = true;
            // 
            // pRight
            // 
            this.pRight.Controls.Add(this.gbPackageInfo);
            this.pRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pRight.Location = new System.Drawing.Point(748, 0);
            this.pRight.Name = "pRight";
            this.pRight.Padding = new System.Windows.Forms.Padding(5);
            this.pRight.Size = new System.Drawing.Size(225, 593);
            this.pRight.TabIndex = 1;
            // 
            // gbPackageInfo
            // 
            this.gbPackageInfo.Controls.Add(this.llProjectUrl);
            this.gbPackageInfo.Controls.Add(this.label4);
            this.gbPackageInfo.Controls.Add(this.pbIcon);
            this.gbPackageInfo.Controls.Add(this.tbDescription);
            this.gbPackageInfo.Controls.Add(this.label3);
            this.gbPackageInfo.Controls.Add(this.tbAuthors);
            this.gbPackageInfo.Controls.Add(this.label2);
            this.gbPackageInfo.Controls.Add(this.tbTitle);
            this.gbPackageInfo.Controls.Add(this.label1);
            this.gbPackageInfo.Controls.Add(this.tbId);
            this.gbPackageInfo.Controls.Add(this.lCreated);
            this.gbPackageInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPackageInfo.Location = new System.Drawing.Point(5, 5);
            this.gbPackageInfo.Name = "gbPackageInfo";
            this.gbPackageInfo.Size = new System.Drawing.Size(215, 583);
            this.gbPackageInfo.TabIndex = 0;
            this.gbPackageInfo.TabStop = false;
            this.gbPackageInfo.Text = "Package Info";
            // 
            // llProjectUrl
            // 
            this.llProjectUrl.AutoSize = true;
            this.llProjectUrl.Location = new System.Drawing.Point(6, 182);
            this.llProjectUrl.Name = "llProjectUrl";
            this.llProjectUrl.Size = new System.Drawing.Size(108, 13);
            this.llProjectUrl.TabIndex = 10;
            this.llProjectUrl.TabStop = true;
            this.llProjectUrl.Text = "http://metapack.com";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Project URL:";
            // 
            // pbIcon
            // 
            this.pbIcon.Location = new System.Drawing.Point(6, 374);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(203, 203);
            this.pbIcon.TabIndex = 8;
            this.pbIcon.TabStop = false;
            // 
            // tbDescription
            // 
            this.tbDescription.Enabled = false;
            this.tbDescription.Location = new System.Drawing.Point(6, 218);
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(203, 150);
            this.tbDescription.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 202);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Description:";
            // 
            // tbAuthors
            // 
            this.tbAuthors.Enabled = false;
            this.tbAuthors.Location = new System.Drawing.Point(6, 132);
            this.tbAuthors.Name = "tbAuthors";
            this.tbAuthors.Size = new System.Drawing.Size(203, 20);
            this.tbAuthors.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Authors:";
            // 
            // tbTitle
            // 
            this.tbTitle.Enabled = false;
            this.tbTitle.Location = new System.Drawing.Point(6, 85);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(203, 20);
            this.tbTitle.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Title:";
            // 
            // tbId
            // 
            this.tbId.Enabled = false;
            this.tbId.Location = new System.Drawing.Point(6, 46);
            this.tbId.Name = "tbId";
            this.tbId.Size = new System.Drawing.Size(203, 20);
            this.tbId.TabIndex = 1;
            // 
            // lCreated
            // 
            this.lCreated.AutoSize = true;
            this.lCreated.Location = new System.Drawing.Point(6, 30);
            this.lCreated.Name = "lCreated";
            this.lCreated.Size = new System.Drawing.Size(67, 13);
            this.lCreated.TabIndex = 0;
            this.lCreated.Text = "Package ID:";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(5, 5);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 583);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // pLeft
            // 
            this.pLeft.Controls.Add(this.gbConnections);
            this.pLeft.Controls.Add(this.splitter1);
            this.pLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pLeft.Location = new System.Drawing.Point(0, 0);
            this.pLeft.Name = "pLeft";
            this.pLeft.Padding = new System.Windows.Forms.Padding(5);
            this.pLeft.Size = new System.Drawing.Size(225, 593);
            this.pLeft.TabIndex = 0;
            // 
            // gbConnections
            // 
            this.gbConnections.Controls.Add(this.tvConnections);
            this.gbConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbConnections.Location = new System.Drawing.Point(8, 5);
            this.gbConnections.Name = "gbConnections";
            this.gbConnections.Padding = new System.Windows.Forms.Padding(5);
            this.gbConnections.Size = new System.Drawing.Size(212, 583);
            this.gbConnections.TabIndex = 2;
            this.gbConnections.TabStop = false;
            this.gbConnections.Text = "SharePoint connections";
            // 
            // gbPackages
            // 
            this.gbPackages.Controls.Add(this.lvPackages);
            this.gbPackages.Controls.Add(this.pNuGetActions);
            this.gbPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPackages.Location = new System.Drawing.Point(5, 5);
            this.gbPackages.Name = "gbPackages";
            this.gbPackages.Padding = new System.Windows.Forms.Padding(5);
            this.gbPackages.Size = new System.Drawing.Size(513, 583);
            this.gbPackages.TabIndex = 2;
            this.gbPackages.TabStop = false;
            this.gbPackages.Text = "Package management";
            // 
            // MetaPackControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pCenter);
            this.Controls.Add(this.pRight);
            this.Controls.Add(this.pLeft);
            this.Name = "MetaPackControl";
            this.Size = new System.Drawing.Size(973, 593);
            this.pCenter.ResumeLayout(false);
            this.pNuGetActions.ResumeLayout(false);
            this.pRight.ResumeLayout(false);
            this.gbPackageInfo.ResumeLayout(false);
            this.gbPackageInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.pLeft.ResumeLayout(false);
            this.gbConnections.ResumeLayout(false);
            this.gbPackages.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pCenter;
        private System.Windows.Forms.TreeView tvConnections;
        private System.Windows.Forms.ListView lvPackages;
        private System.Windows.Forms.Panel pRight;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel pLeft;
        private System.Windows.Forms.Panel pNuGetActions;
        private System.Windows.Forms.Button bNuGetRefresh;
        private System.Windows.Forms.Button bUpdate;
        private System.Windows.Forms.Button bInstall;
        private System.Windows.Forms.GroupBox gbPackageInfo;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbAuthors;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbId;
        private System.Windows.Forms.Label lCreated;
        private System.Windows.Forms.PictureBox pbIcon;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel llProjectUrl;
        private System.Windows.Forms.GroupBox gbPackages;
        private System.Windows.Forms.GroupBox gbConnections;
    }
}
