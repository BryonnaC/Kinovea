
namespace Analysistem
{
    partial class Form1
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
            this.titleBox = new System.Windows.Forms.TextBox();
            this.ageInput = new System.Windows.Forms.MaskedTextBox();
            this.feetInput = new System.Windows.Forms.MaskedTextBox();
            this.lbsInput = new System.Windows.Forms.MaskedTextBox();
            this.femaleRadio = new System.Windows.Forms.RadioButton();
            this.maleRadio = new System.Windows.Forms.RadioButton();
            this.declineRadio = new System.Windows.Forms.RadioButton();
            this.nextButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.metricHeight = new System.Windows.Forms.RadioButton();
            this.imperialHeight = new System.Windows.Forms.RadioButton();
            this.imperialWeight = new System.Windows.Forms.RadioButton();
            this.metricWeight = new System.Windows.Forms.RadioButton();
            this.inchInput = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // titleBox
            // 
            this.titleBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleBox.Location = new System.Drawing.Point(187, 3);
            this.titleBox.Name = "titleBox";
            this.titleBox.ReadOnly = true;
            this.titleBox.Size = new System.Drawing.Size(236, 31);
            this.titleBox.TabIndex = 0;
            this.titleBox.TabStop = false;
            this.titleBox.Text = "Information Collection";
            // 
            // ageInput
            // 
            this.ageInput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.ageInput.Location = new System.Drawing.Point(76, 204);
            this.ageInput.Name = "ageInput";
            this.ageInput.Size = new System.Drawing.Size(100, 20);
            this.ageInput.TabIndex = 6;
            this.ageInput.Text = "age";
            this.ageInput.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.ageInput_MaskInputRejected);
            // 
            // feetInput
            // 
            this.feetInput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.feetInput.Location = new System.Drawing.Point(365, 91);
            this.feetInput.Name = "feetInput";
            this.feetInput.Size = new System.Drawing.Size(58, 20);
            this.feetInput.TabIndex = 8;
            this.feetInput.Text = "height";
            // 
            // lbsInput
            // 
            this.lbsInput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.lbsInput.Location = new System.Drawing.Point(369, 204);
            this.lbsInput.Name = "lbsInput";
            this.lbsInput.Size = new System.Drawing.Size(58, 20);
            this.lbsInput.TabIndex = 9;
            this.lbsInput.Text = "weight";
            // 
            // femaleRadio
            // 
            this.femaleRadio.AutoSize = true;
            this.femaleRadio.Location = new System.Drawing.Point(76, 92);
            this.femaleRadio.Name = "femaleRadio";
            this.femaleRadio.Size = new System.Drawing.Size(59, 17);
            this.femaleRadio.TabIndex = 11;
            this.femaleRadio.TabStop = true;
            this.femaleRadio.Text = "Female";
            this.femaleRadio.UseVisualStyleBackColor = true;
            // 
            // maleRadio
            // 
            this.maleRadio.AutoSize = true;
            this.maleRadio.Location = new System.Drawing.Point(76, 115);
            this.maleRadio.Name = "maleRadio";
            this.maleRadio.Size = new System.Drawing.Size(48, 17);
            this.maleRadio.TabIndex = 12;
            this.maleRadio.TabStop = true;
            this.maleRadio.Text = "Male";
            this.maleRadio.UseVisualStyleBackColor = true;
            // 
            // declineRadio
            // 
            this.declineRadio.AutoSize = true;
            this.declineRadio.Location = new System.Drawing.Point(76, 138);
            this.declineRadio.Name = "declineRadio";
            this.declineRadio.Size = new System.Drawing.Size(111, 17);
            this.declineRadio.TabIndex = 13;
            this.declineRadio.TabStop = true;
            this.declineRadio.Text = "Decline to Answer";
            this.declineRadio.UseVisualStyleBackColor = true;
            // 
            // nextButton
            // 
            this.nextButton.BackColor = System.Drawing.Color.LimeGreen;
            this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.nextButton.Location = new System.Drawing.Point(522, 293);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(102, 40);
            this.nextButton.TabIndex = 14;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = false;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(75, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 29);
            this.label1.TabIndex = 15;
            this.label1.Text = "Sex:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(360, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 29);
            this.label2.TabIndex = 16;
            this.label2.Text = "Weight:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(75, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 29);
            this.label3.TabIndex = 17;
            this.label3.Text = "Age:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(360, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 29);
            this.label4.TabIndex = 18;
            this.label4.Text = "Height:";
            // 
            // metricHeight
            // 
            this.metricHeight.AutoSize = true;
            this.metricHeight.Location = new System.Drawing.Point(480, 91);
            this.metricHeight.Name = "metricHeight";
            this.metricHeight.Size = new System.Drawing.Size(39, 17);
            this.metricHeight.TabIndex = 19;
            this.metricHeight.TabStop = true;
            this.metricHeight.Text = "cm";
            this.metricHeight.UseVisualStyleBackColor = true;
            // 
            // imperialHeight
            // 
            this.imperialHeight.AutoSize = true;
            this.imperialHeight.Location = new System.Drawing.Point(429, 91);
            this.imperialHeight.Name = "imperialHeight";
            this.imperialHeight.Size = new System.Drawing.Size(45, 17);
            this.imperialHeight.TabIndex = 20;
            this.imperialHeight.Text = "ft, in";
            this.imperialHeight.UseVisualStyleBackColor = true;
            // 
            // imperialWeight
            // 
            this.imperialWeight.AutoSize = true;
            this.imperialWeight.Checked = true;
            this.imperialWeight.Location = new System.Drawing.Point(433, 205);
            this.imperialWeight.Name = "imperialWeight";
            this.imperialWeight.Size = new System.Drawing.Size(38, 17);
            this.imperialWeight.TabIndex = 22;
            this.imperialWeight.TabStop = true;
            this.imperialWeight.Text = "lbs";
            this.imperialWeight.UseVisualStyleBackColor = true;
            // 
            // metricWeight
            // 
            this.metricWeight.AutoSize = true;
            this.metricWeight.Location = new System.Drawing.Point(484, 205);
            this.metricWeight.Name = "metricWeight";
            this.metricWeight.Size = new System.Drawing.Size(42, 17);
            this.metricWeight.TabIndex = 21;
            this.metricWeight.TabStop = true;
            this.metricWeight.Text = "kgs";
            this.metricWeight.UseVisualStyleBackColor = true;
            // 
            // inchInput
            // 
            this.inchInput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.inchInput.Location = new System.Drawing.Point(365, 117);
            this.inchInput.Name = "inchInput";
            this.inchInput.Size = new System.Drawing.Size(58, 20);
            this.inchInput.TabIndex = 23;
            this.inchInput.Text = "height";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(643, 344);
            this.Controls.Add(this.inchInput);
            this.Controls.Add(this.imperialWeight);
            this.Controls.Add(this.metricWeight);
            this.Controls.Add(this.imperialHeight);
            this.Controls.Add(this.metricHeight);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.declineRadio);
            this.Controls.Add(this.maleRadio);
            this.Controls.Add(this.femaleRadio);
            this.Controls.Add(this.lbsInput);
            this.Controls.Add(this.feetInput);
            this.Controls.Add(this.ageInput);
            this.Controls.Add(this.titleBox);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "Form1";
            this.Text = "Information Collection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox titleBox;
        private System.Windows.Forms.MaskedTextBox ageInput;
        private System.Windows.Forms.MaskedTextBox feetInput;
        private System.Windows.Forms.MaskedTextBox lbsInput;
        private System.Windows.Forms.RadioButton femaleRadio;
        private System.Windows.Forms.RadioButton maleRadio;
        private System.Windows.Forms.RadioButton declineRadio;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton metricHeight;
        private System.Windows.Forms.RadioButton imperialHeight;
        private System.Windows.Forms.RadioButton imperialWeight;
        private System.Windows.Forms.RadioButton metricWeight;
        private System.Windows.Forms.MaskedTextBox inchInput;
    }
}