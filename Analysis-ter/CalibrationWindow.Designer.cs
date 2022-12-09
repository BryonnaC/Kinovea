
namespace Analysistem
{
    partial class CalibrationWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lengthBox = new System.Windows.Forms.TextBox();
            this.heightBox = new System.Windows.Forms.TextBox();
            this.weightBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lengthCm = new System.Windows.Forms.RadioButton();
            this.lengthIn = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.widthCm = new System.Windows.Forms.RadioButton();
            this.widthIn = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.heightCm = new System.Windows.Forms.RadioButton();
            this.heightIn = new System.Windows.Forms.RadioButton();
            this.nextButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(203, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Calibration Object";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Length:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Width:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(24, 193);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "Height:";
            // 
            // lengthBox
            // 
            this.lengthBox.Location = new System.Drawing.Point(114, 68);
            this.lengthBox.Name = "lengthBox";
            this.lengthBox.Size = new System.Drawing.Size(100, 20);
            this.lengthBox.TabIndex = 4;
            // 
            // heightBox
            // 
            this.heightBox.Location = new System.Drawing.Point(114, 198);
            this.heightBox.Name = "heightBox";
            this.heightBox.Size = new System.Drawing.Size(100, 20);
            this.heightBox.TabIndex = 5;
            // 
            // weightBox
            // 
            this.weightBox.Location = new System.Drawing.Point(114, 128);
            this.weightBox.Name = "weightBox";
            this.weightBox.Size = new System.Drawing.Size(100, 20);
            this.weightBox.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lengthCm);
            this.groupBox1.Controls.Add(this.lengthIn);
            this.groupBox1.Location = new System.Drawing.Point(220, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(131, 55);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // lengthCm
            // 
            this.lengthCm.AutoSize = true;
            this.lengthCm.Location = new System.Drawing.Point(46, 27);
            this.lengthCm.Name = "lengthCm";
            this.lengthCm.Size = new System.Drawing.Size(39, 17);
            this.lengthCm.TabIndex = 1;
            this.lengthCm.Text = "cm";
            this.lengthCm.UseVisualStyleBackColor = true;
            this.lengthCm.CheckedChanged += new System.EventHandler(this.lengthCm_CheckedChanged);
            // 
            // lengthIn
            // 
            this.lengthIn.AutoSize = true;
            this.lengthIn.Checked = true;
            this.lengthIn.Location = new System.Drawing.Point(7, 27);
            this.lengthIn.Name = "lengthIn";
            this.lengthIn.Size = new System.Drawing.Size(33, 17);
            this.lengthIn.TabIndex = 0;
            this.lengthIn.TabStop = true;
            this.lengthIn.Text = "in";
            this.lengthIn.UseVisualStyleBackColor = true;
            this.lengthIn.CheckedChanged += new System.EventHandler(this.lengthIn_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.widthCm);
            this.groupBox2.Controls.Add(this.widthIn);
            this.groupBox2.Location = new System.Drawing.Point(220, 104);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(131, 64);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            // 
            // widthCm
            // 
            this.widthCm.AutoSize = true;
            this.widthCm.Location = new System.Drawing.Point(46, 27);
            this.widthCm.Name = "widthCm";
            this.widthCm.Size = new System.Drawing.Size(39, 17);
            this.widthCm.TabIndex = 3;
            this.widthCm.Text = "cm";
            this.widthCm.UseVisualStyleBackColor = true;
            // 
            // widthIn
            // 
            this.widthIn.AutoSize = true;
            this.widthIn.Checked = true;
            this.widthIn.Location = new System.Drawing.Point(6, 27);
            this.widthIn.Name = "widthIn";
            this.widthIn.Size = new System.Drawing.Size(33, 17);
            this.widthIn.TabIndex = 3;
            this.widthIn.TabStop = true;
            this.widthIn.Text = "in";
            this.widthIn.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.heightCm);
            this.groupBox3.Controls.Add(this.heightIn);
            this.groupBox3.Location = new System.Drawing.Point(220, 174);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(131, 58);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            // 
            // heightCm
            // 
            this.heightCm.AutoSize = true;
            this.heightCm.Location = new System.Drawing.Point(46, 24);
            this.heightCm.Name = "heightCm";
            this.heightCm.Size = new System.Drawing.Size(39, 17);
            this.heightCm.TabIndex = 2;
            this.heightCm.Text = "cm";
            this.heightCm.UseVisualStyleBackColor = true;
            // 
            // heightIn
            // 
            this.heightIn.AutoSize = true;
            this.heightIn.Checked = true;
            this.heightIn.Location = new System.Drawing.Point(7, 25);
            this.heightIn.Name = "heightIn";
            this.heightIn.Size = new System.Drawing.Size(33, 17);
            this.heightIn.TabIndex = 2;
            this.heightIn.TabStop = true;
            this.heightIn.Text = "in";
            this.heightIn.UseVisualStyleBackColor = true;
            // 
            // nextButton
            // 
            this.nextButton.BackColor = System.Drawing.Color.LimeGreen;
            this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.nextButton.Location = new System.Drawing.Point(529, 217);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(102, 40);
            this.nextButton.TabIndex = 15;
            this.nextButton.Text = "SAVE";
            this.nextButton.UseVisualStyleBackColor = false;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // CalibrationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 269);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.weightBox);
            this.Controls.Add(this.heightBox);
            this.Controls.Add(this.lengthBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CalibrationWindow";
            this.Text = "CalibrationWindow";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox lengthBox;
        private System.Windows.Forms.TextBox heightBox;
        private System.Windows.Forms.TextBox weightBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton lengthCm;
        private System.Windows.Forms.RadioButton lengthIn;
        private System.Windows.Forms.RadioButton widthIn;
        private System.Windows.Forms.RadioButton heightIn;
        private System.Windows.Forms.RadioButton widthCm;
        private System.Windows.Forms.RadioButton heightCm;
        private System.Windows.Forms.Button nextButton;
    }
}