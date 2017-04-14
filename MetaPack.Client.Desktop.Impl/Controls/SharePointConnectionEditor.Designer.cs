namespace MetaPack.Client.Desktop.Impl.Controls
{
    partial class SharePointConnectionEditor
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
            this.lURL = new System.Windows.Forms.Label();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.gbConnectionDetails = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbUserPassoword = new System.Windows.Forms.TextBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.cbUseCurrentCredentials = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbAuthMode = new System.Windows.Forms.ComboBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.gbConnectionDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // lURL
            // 
            this.lURL.AutoSize = true;
            this.lURL.Location = new System.Drawing.Point(3, 6);
            this.lURL.Name = "lURL";
            this.lURL.Size = new System.Drawing.Size(201, 13);
            this.lURL.TabIndex = 0;
            this.lURL.Text = "URL of SharePoint site collection or web:";
            // 
            // tbUrl
            // 
            this.tbUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUrl.Location = new System.Drawing.Point(6, 22);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(407, 20);
            this.tbUrl.TabIndex = 1;
            // 
            // gbConnectionDetails
            // 
            this.gbConnectionDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbConnectionDetails.Controls.Add(this.label3);
            this.gbConnectionDetails.Controls.Add(this.label1);
            this.gbConnectionDetails.Controls.Add(this.tbUserPassoword);
            this.gbConnectionDetails.Controls.Add(this.tbUserName);
            this.gbConnectionDetails.Controls.Add(this.cbUseCurrentCredentials);
            this.gbConnectionDetails.Controls.Add(this.label2);
            this.gbConnectionDetails.Controls.Add(this.cbAuthMode);
            this.gbConnectionDetails.Location = new System.Drawing.Point(6, 48);
            this.gbConnectionDetails.Name = "gbConnectionDetails";
            this.gbConnectionDetails.Size = new System.Drawing.Size(407, 128);
            this.gbConnectionDetails.TabIndex = 2;
            this.gbConnectionDetails.TabStop = false;
            this.gbConnectionDetails.Text = "Connection details";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "User password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "User name:";
            // 
            // tbUserPassoword
            // 
            this.tbUserPassoword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUserPassoword.Location = new System.Drawing.Point(119, 96);
            this.tbUserPassoword.Name = "tbUserPassoword";
            this.tbUserPassoword.PasswordChar = '*';
            this.tbUserPassoword.Size = new System.Drawing.Size(282, 20);
            this.tbUserPassoword.TabIndex = 7;
            // 
            // tbUserName
            // 
            this.tbUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUserName.Location = new System.Drawing.Point(119, 70);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(282, 20);
            this.tbUserName.TabIndex = 6;
            // 
            // cbUseCurrentCredentials
            // 
            this.cbUseCurrentCredentials.AutoSize = true;
            this.cbUseCurrentCredentials.Location = new System.Drawing.Point(119, 47);
            this.cbUseCurrentCredentials.Name = "cbUseCurrentCredentials";
            this.cbUseCurrentCredentials.Size = new System.Drawing.Size(135, 17);
            this.cbUseCurrentCredentials.TabIndex = 5;
            this.cbUseCurrentCredentials.Text = "Use current credentials";
            this.cbUseCurrentCredentials.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Authentication mode:";
            // 
            // cbAuthMode
            // 
            this.cbAuthMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAuthMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAuthMode.FormattingEnabled = true;
            this.cbAuthMode.Items.AddRange(new object[] {
            "Windows Authentication",
            "SharePoint Online (O365)"});
            this.cbAuthMode.Location = new System.Drawing.Point(119, 19);
            this.cbAuthMode.Name = "cbAuthMode";
            this.cbAuthMode.Size = new System.Drawing.Size(282, 21);
            this.cbAuthMode.TabIndex = 0;
            // 
            // SharePointConnectionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbConnectionDetails);
            this.Controls.Add(this.tbUrl);
            this.Controls.Add(this.lURL);
            this.Name = "SharePointConnectionEditor";
            this.Size = new System.Drawing.Size(420, 181);
            this.gbConnectionDetails.ResumeLayout(false);
            this.gbConnectionDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lURL;
        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.GroupBox gbConnectionDetails;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.CheckBox cbUseCurrentCredentials;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbAuthMode;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbUserPassoword;
    }
}
