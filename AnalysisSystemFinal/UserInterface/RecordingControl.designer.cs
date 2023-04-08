
namespace AnalysystemTakeTwo
{
    partial class RecordingControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonToolStrip = new System.Windows.Forms.ToolStrip();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(179, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(100);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 500);
            this.panel1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(60, 602);
            this.button1.Margin = new System.Windows.Forms.Padding(20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Back to Dashboard";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonToolStrip);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(172, 429);
            this.panel2.TabIndex = 6;
            // 
            // buttonToolStrip
            // 
            this.buttonToolStrip.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.buttonToolStrip.Location = new System.Drawing.Point(0, 0);
            this.buttonToolStrip.Name = "buttonToolStrip";
            this.buttonToolStrip.Size = new System.Drawing.Size(172, 111);
            this.buttonToolStrip.TabIndex = 0;
            this.buttonToolStrip.Text = "buttonToolStrip";
            // 
            // RecordingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(124)))));
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "RecordingControl";
            this.Size = new System.Drawing.Size(1250, 625);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.ToolStrip buttonToolStrip;
    }
}
