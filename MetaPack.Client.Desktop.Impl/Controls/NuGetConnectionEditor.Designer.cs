namespace MetaPack.Client.Desktop.Impl.Controls
{
    partial class NuGetConnectionEditor
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
            this.gbConnectionDetails = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.gbConnectionDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbConnectionDetails
            // 
            this.gbConnectionDetails.Controls.Add(this.label3);
            this.gbConnectionDetails.Controls.Add(this.label1);
            this.gbConnectionDetails.Controls.Add(this.tbUrl);
            this.gbConnectionDetails.Controls.Add(this.tbUserName);
            this.gbConnectionDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbConnectionDetails.Location = new System.Drawing.Point(0, 0);
            this.gbConnectionDetails.Name = "gbConnectionDetails";
            this.gbConnectionDetails.Size = new System.Drawing.Size(434, 103);
            this.gbConnectionDetails.TabIndex = 3;
            this.gbConnectionDetails.TabStop = false;
            this.gbConnectionDetails.Text = "Connection details";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Url:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Name:";
            // 
            // tbUrl
            // 
            this.tbUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUrl.Location = new System.Drawing.Point(9, 71);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(419, 20);
            this.tbUrl.TabIndex = 7;
            // 
            // tbUserName
            // 
            this.tbUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUserName.Location = new System.Drawing.Point(9, 32);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(419, 20);
            this.tbUserName.TabIndex = 6;
            // 
            // NuGetConnectionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbConnectionDetails);
            this.Name = "NuGetConnectionEditor";
            this.Size = new System.Drawing.Size(434, 103);
            this.gbConnectionDetails.ResumeLayout(false);
            this.gbConnectionDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConnectionDetails;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.TextBox tbUserName;


    }
}
