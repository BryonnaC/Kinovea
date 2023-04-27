
namespace AnalysisSystemFinal.UserInterface
{
    partial class PositionDataSelection
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.openHori = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.openVert = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(161, 20);
            this.textBox1.TabIndex = 3;
            // 
            // openHori
            // 
            this.openHori.Location = new System.Drawing.Point(224, 10);
            this.openHori.Name = "openHori";
            this.openHori.Size = new System.Drawing.Size(134, 23);
            this.openHori.TabIndex = 2;
            this.openHori.Text = "Open Horizontal CSV";
            this.openHori.UseVisualStyleBackColor = true;
            this.openHori.Click += new System.EventHandler(this.openHoriz_click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 58);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(161, 20);
            this.textBox2.TabIndex = 5;
            // 
            // openVert
            // 
            this.openVert.Location = new System.Drawing.Point(224, 56);
            this.openVert.Name = "openVert";
            this.openVert.Size = new System.Drawing.Size(134, 23);
            this.openVert.TabIndex = 4;
            this.openVert.Text = "Open Vertical CSV";
            this.openVert.UseVisualStyleBackColor = true;
            this.openVert.Click += new System.EventHandler(this.openVert_Click);
            // 
            // PositionDataSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 91);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.openVert);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.openHori);
            this.Name = "PositionDataSelection";
            this.Text = "PositionDataSelection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button openHori;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button openVert;
    }
}