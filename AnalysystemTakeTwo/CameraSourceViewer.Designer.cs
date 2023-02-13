
namespace AnalysystemTakeTwo
{
    partial class CameraSourceViewer
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
            this.camOption = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // camOption
            // 
            this.camOption.Location = new System.Drawing.Point(145, 117);
            this.camOption.Name = "camOption";
            this.camOption.Size = new System.Drawing.Size(265, 172);
            this.camOption.TabIndex = 0;
            this.camOption.Text = "You have a camera plugged in!";
            this.camOption.UseVisualStyleBackColor = true;
            this.camOption.Click += new System.EventHandler(this.camOption_Click);
            // 
            // nextButton
            // 
            this.nextButton.BackColor = System.Drawing.Color.LimeGreen;
            this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.nextButton.Location = new System.Drawing.Point(423, 370);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(102, 40);
            this.nextButton.TabIndex = 31;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = false;
            // 
            // CameraSourceViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.camOption);
            this.Name = "CameraSourceViewer";
            this.Size = new System.Drawing.Size(582, 450);
            this.Load += new System.EventHandler(this.CameraSourceViewer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button camOption;
        private System.Windows.Forms.Button nextButton;
    }
}
