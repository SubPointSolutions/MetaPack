namespace MetaPack.Client.Desktop.Impl.Views
{
    partial class StartPageViewControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartPageViewControl));
            this.pMetroPanel = new MetroFramework.Controls.MetroPanel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.pHelp = new System.Windows.Forms.FlowLayoutPanel();
            this.pStart = new System.Windows.Forms.FlowLayoutPanel();
            this.pMetroPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pMetroPanel
            // 
            this.pMetroPanel.Controls.Add(this.metroLabel4);
            this.pMetroPanel.Controls.Add(this.metroLabel3);
            this.pMetroPanel.Controls.Add(this.metroLabel2);
            this.pMetroPanel.Controls.Add(this.metroLabel1);
            this.pMetroPanel.Controls.Add(this.pHelp);
            this.pMetroPanel.Controls.Add(this.pStart);
            this.pMetroPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMetroPanel.HorizontalScrollbarBarColor = true;
            this.pMetroPanel.HorizontalScrollbarHighlightOnWheel = false;
            this.pMetroPanel.HorizontalScrollbarSize = 10;
            this.pMetroPanel.Location = new System.Drawing.Point(0, 0);
            this.pMetroPanel.Name = "pMetroPanel";
            this.pMetroPanel.Size = new System.Drawing.Size(658, 373);
            this.pMetroPanel.TabIndex = 5;
            this.pMetroPanel.VerticalScrollbarBarColor = true;
            this.pMetroPanel.VerticalScrollbarHighlightOnWheel = false;
            this.pMetroPanel.VerticalScrollbarSize = 10;
            // 
            // metroLabel4
            // 
            this.metroLabel4.Location = new System.Drawing.Point(288, 33);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(355, 289);
            this.metroLabel4.TabIndex = 8;
            this.metroLabel4.Text = resources.GetString("metroLabel4.Text");
            this.metroLabel4.WrapToLine = true;
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel3.Location = new System.Drawing.Point(288, 8);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(188, 19);
            this.metroLabel3.TabIndex = 7;
            this.metroLabel3.Text = "Welcome to MetaPack GUI";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel2.Location = new System.Drawing.Point(3, 160);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(41, 19);
            this.metroLabel2.TabIndex = 6;
            this.metroLabel2.Text = "Help";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel1.Location = new System.Drawing.Point(3, 8);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(41, 19);
            this.metroLabel1.TabIndex = 5;
            this.metroLabel1.Text = "Start";
            // 
            // pHelp
            // 
            this.pHelp.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pHelp.Location = new System.Drawing.Point(3, 182);
            this.pHelp.Name = "pHelp";
            this.pHelp.Size = new System.Drawing.Size(279, 140);
            this.pHelp.TabIndex = 1;
            // 
            // pStart
            // 
            this.pStart.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pStart.Location = new System.Drawing.Point(3, 33);
            this.pStart.Name = "pStart";
            this.pStart.Size = new System.Drawing.Size(279, 124);
            this.pStart.TabIndex = 0;
            // 
            // StartPageViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pMetroPanel);
            this.Name = "StartPageViewControl";
            this.Size = new System.Drawing.Size(658, 373);
            this.pMetroPanel.ResumeLayout(false);
            this.pMetroPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel pMetroPanel;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private System.Windows.Forms.FlowLayoutPanel pHelp;
        private System.Windows.Forms.FlowLayoutPanel pStart;


    }
}
