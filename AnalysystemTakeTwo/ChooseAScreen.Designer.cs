
namespace AnalysystemTakeTwo
{
    partial class ChooseAScreen
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
            this.LiveVideo = new System.Windows.Forms.Button();
            this.CalibrationObject = new System.Windows.Forms.Button();
            this.SelectVideo = new System.Windows.Forms.Button();
            this.IntakeInfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LiveVideo
            // 
            this.LiveVideo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(230)))));
            this.LiveVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.LiveVideo.ForeColor = System.Drawing.Color.Black;
            this.LiveVideo.Location = new System.Drawing.Point(47, 22);
            this.LiveVideo.Name = "LiveVideo";
            this.LiveVideo.Size = new System.Drawing.Size(278, 169);
            this.LiveVideo.TabIndex = 0;
            this.LiveVideo.Text = "Start with Live Video Capture";
            this.LiveVideo.UseVisualStyleBackColor = false;
            this.LiveVideo.Click += new System.EventHandler(this.LiveVideo_Click);
            // 
            // CalibrationObject
            // 
            this.CalibrationObject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(230)))));
            this.CalibrationObject.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.CalibrationObject.ForeColor = System.Drawing.Color.Black;
            this.CalibrationObject.Location = new System.Drawing.Point(47, 219);
            this.CalibrationObject.Name = "CalibrationObject";
            this.CalibrationObject.Size = new System.Drawing.Size(278, 169);
            this.CalibrationObject.TabIndex = 1;
            this.CalibrationObject.Text = "Enter Calibration Object Dimensions";
            this.CalibrationObject.UseVisualStyleBackColor = false;
            this.CalibrationObject.Click += new System.EventHandler(this.CalibrationObject_Click);
            // 
            // SelectVideo
            // 
            this.SelectVideo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(230)))));
            this.SelectVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.SelectVideo.ForeColor = System.Drawing.Color.Black;
            this.SelectVideo.Location = new System.Drawing.Point(364, 22);
            this.SelectVideo.Name = "SelectVideo";
            this.SelectVideo.Size = new System.Drawing.Size(278, 169);
            this.SelectVideo.TabIndex = 2;
            this.SelectVideo.Text = "Start with Pre-recorded Video";
            this.SelectVideo.UseVisualStyleBackColor = false;
            this.SelectVideo.Click += new System.EventHandler(this.SelectVideo_Click);
            // 
            // IntakeInfo
            // 
            this.IntakeInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(230)))));
            this.IntakeInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.IntakeInfo.ForeColor = System.Drawing.Color.Black;
            this.IntakeInfo.Location = new System.Drawing.Point(364, 219);
            this.IntakeInfo.Name = "IntakeInfo";
            this.IntakeInfo.Size = new System.Drawing.Size(278, 169);
            this.IntakeInfo.TabIndex = 3;
            this.IntakeInfo.Text = "Enter Subject Information";
            this.IntakeInfo.UseVisualStyleBackColor = false;
            this.IntakeInfo.Click += new System.EventHandler(this.IntakeInfo_Click);
            // 
            // InitialWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(124)))));
            this.Controls.Add(this.IntakeInfo);
            this.Controls.Add(this.SelectVideo);
            this.Controls.Add(this.CalibrationObject);
            this.Controls.Add(this.LiveVideo);
            this.Name = "InitialWindow";
            this.Size = new System.Drawing.Size(693, 419);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LiveVideo;
        private System.Windows.Forms.Button CalibrationObject;
        private System.Windows.Forms.Button SelectVideo;
        private System.Windows.Forms.Button IntakeInfo;
    }
}
