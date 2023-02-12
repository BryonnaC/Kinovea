
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
            // 
            // CameraSourceViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.camOption);
            this.Name = "CameraSourceViewer";
            this.Size = new System.Drawing.Size(582, 450);
            this.Load += new System.EventHandler(this.CameraSourceViewer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button camOption;
    }
}
