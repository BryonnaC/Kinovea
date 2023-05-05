
namespace AnalysystemTakeTwo
{
    partial class SubjectInformationScreen
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
            this.nextButton = new System.Windows.Forms.Button();
            this.imperialWeight = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.weightInput = new System.Windows.Forms.MaskedTextBox();
            this.metricHeight = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.imperialHeight = new System.Windows.Forms.RadioButton();
            this.inchInput = new System.Windows.Forms.MaskedTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.feetInput = new System.Windows.Forms.MaskedTextBox();
            this.ageInput = new System.Windows.Forms.MaskedTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.declineRadio = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.maskedTextBox7 = new System.Windows.Forms.MaskedTextBox();
            this.metricWeight = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.radioButton11 = new System.Windows.Forms.RadioButton();
            this.radioButton12 = new System.Windows.Forms.RadioButton();
            this.maskedTextBox5 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBox6 = new System.Windows.Forms.MaskedTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.femaleRadio = new System.Windows.Forms.RadioButton();
            this.maleRadio = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nextButton
            // 
            this.nextButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(163)))), ((int)(((byte)(158)))));
            this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.nextButton.Location = new System.Drawing.Point(492, 348);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(102, 40);
            this.nextButton.TabIndex = 30;
            this.nextButton.Text = "Save";
            this.nextButton.UseVisualStyleBackColor = false;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // imperialWeight
            // 
            this.imperialWeight.AutoSize = true;
            this.imperialWeight.Checked = true;
            this.imperialWeight.Location = new System.Drawing.Point(68, 49);
            this.imperialWeight.Name = "imperialWeight";
            this.imperialWeight.Size = new System.Drawing.Size(38, 17);
            this.imperialWeight.TabIndex = 22;
            this.imperialWeight.TabStop = true;
            this.imperialWeight.Text = "lbs";
            this.imperialWeight.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 29);
            this.label6.TabIndex = 32;
            this.label6.Text = "Weight:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 29);
            this.label5.TabIndex = 31;
            this.label5.Text = "Sex:";
            // 
            // weightInput
            // 
            this.weightInput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.weightInput.Location = new System.Drawing.Point(338, 240);
            this.weightInput.Name = "weightInput";
            this.weightInput.Size = new System.Drawing.Size(58, 20);
            this.weightInput.TabIndex = 29;
            this.weightInput.Text = "weight";
            this.weightInput.Click += new System.EventHandler(this.weightInput_Click);
            // 
            // metricHeight
            // 
            this.metricHeight.AutoSize = true;
            this.metricHeight.Location = new System.Drawing.Point(115, 51);
            this.metricHeight.Name = "metricHeight";
            this.metricHeight.Size = new System.Drawing.Size(39, 17);
            this.metricHeight.TabIndex = 19;
            this.metricHeight.Text = "cm";
            this.metricHeight.UseVisualStyleBackColor = true;
            this.metricHeight.CheckedChanged += new System.EventHandler(this.metricHeight_CheckedChanged_1);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(33, 205);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 29);
            this.label7.TabIndex = 33;
            this.label7.Text = "Age:";
            // 
            // imperialHeight
            // 
            this.imperialHeight.AutoSize = true;
            this.imperialHeight.Checked = true;
            this.imperialHeight.Location = new System.Drawing.Point(68, 51);
            this.imperialHeight.Name = "imperialHeight";
            this.imperialHeight.Size = new System.Drawing.Size(45, 17);
            this.imperialHeight.TabIndex = 20;
            this.imperialHeight.TabStop = true;
            this.imperialHeight.Text = "ft, in";
            this.imperialHeight.UseVisualStyleBackColor = true;
            this.imperialHeight.CheckedChanged += new System.EventHandler(this.imperialHeight_CheckedChanged_1);
            // 
            // inchInput
            // 
            this.inchInput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.inchInput.Location = new System.Drawing.Point(334, 153);
            this.inchInput.Name = "inchInput";
            this.inchInput.Size = new System.Drawing.Size(58, 20);
            this.inchInput.TabIndex = 35;
            this.inchInput.Text = "height";
            this.inchInput.Click += new System.EventHandler(this.inchInput_Click_1);
            this.inchInput.TabIndexChanged += new System.EventHandler(this.inchInput_TabIndexChanged_1);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 29);
            this.label8.TabIndex = 34;
            this.label8.Text = "Height:";
            // 
            // feetInput
            // 
            this.feetInput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.feetInput.Location = new System.Drawing.Point(334, 127);
            this.feetInput.Name = "feetInput";
            this.feetInput.Size = new System.Drawing.Size(58, 20);
            this.feetInput.TabIndex = 28;
            this.feetInput.Text = "height";
            this.feetInput.Click += new System.EventHandler(this.feetInput_Click_1);
            // 
            // ageInput
            // 
            this.ageInput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.ageInput.Location = new System.Drawing.Point(45, 240);
            this.ageInput.Name = "ageInput";
            this.ageInput.Size = new System.Drawing.Size(100, 20);
            this.ageInput.TabIndex = 27;
            this.ageInput.Text = "age";
            this.ageInput.Click += new System.EventHandler(this.ageInput_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(230)))));
            this.groupBox4.Controls.Add(this.imperialHeight);
            this.groupBox4.Controls.Add(this.metricHeight);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Location = new System.Drawing.Point(334, 76);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 100);
            this.groupBox4.TabIndex = 36;
            this.groupBox4.TabStop = false;
            // 
            // declineRadio
            // 
            this.declineRadio.AutoSize = true;
            this.declineRadio.Location = new System.Drawing.Point(15, 98);
            this.declineRadio.Name = "declineRadio";
            this.declineRadio.Size = new System.Drawing.Size(111, 17);
            this.declineRadio.TabIndex = 13;
            this.declineRadio.Text = "Decline to Answer";
            this.declineRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(230)))));
            this.groupBox5.Controls.Add(this.maskedTextBox7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.imperialWeight);
            this.groupBox5.Controls.Add(this.metricWeight);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.textBox2);
            this.groupBox5.Controls.Add(this.groupBox7);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.groupBox8);
            this.groupBox5.Controls.Add(this.maskedTextBox5);
            this.groupBox5.Controls.Add(this.maskedTextBox6);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Location = new System.Drawing.Point(334, 192);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 100);
            this.groupBox5.TabIndex = 37;
            this.groupBox5.TabStop = false;
            // 
            // maskedTextBox7
            // 
            this.maskedTextBox7.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.maskedTextBox7.Location = new System.Drawing.Point(0, -39);
            this.maskedTextBox7.Name = "maskedTextBox7";
            this.maskedTextBox7.Size = new System.Drawing.Size(58, 20);
            this.maskedTextBox7.TabIndex = 23;
            this.maskedTextBox7.Text = "height";
            // 
            // metricWeight
            // 
            this.metricWeight.AutoSize = true;
            this.metricWeight.Location = new System.Drawing.Point(115, 48);
            this.metricWeight.Name = "metricWeight";
            this.metricWeight.Size = new System.Drawing.Size(42, 17);
            this.metricWeight.TabIndex = 21;
            this.metricWeight.Text = "kgs";
            this.metricWeight.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(-5, -96);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 29);
            this.label12.TabIndex = 18;
            this.label12.Text = "Height:";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(-178, -153);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(236, 31);
            this.textBox2.TabIndex = 0;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Information Collection";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.radioButton8);
            this.groupBox7.Controls.Add(this.radioButton9);
            this.groupBox7.Location = new System.Drawing.Point(0, -116);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(200, 100);
            this.groupBox7.TabIndex = 24;
            this.groupBox7.TabStop = false;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Checked = true;
            this.radioButton8.Location = new System.Drawing.Point(68, 51);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(45, 17);
            this.radioButton8.TabIndex = 20;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "ft, in";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Location = new System.Drawing.Point(115, 51);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(39, 17);
            this.radioButton9.TabIndex = 19;
            this.radioButton9.Text = "cm";
            this.radioButton9.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(-290, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 29);
            this.label11.TabIndex = 17;
            this.label11.Text = "Age:";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.radioButton10);
            this.groupBox8.Controls.Add(this.radioButton11);
            this.groupBox8.Controls.Add(this.radioButton12);
            this.groupBox8.Location = new System.Drawing.Point(-304, -116);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(200, 129);
            this.groupBox8.TabIndex = 25;
            this.groupBox8.TabStop = false;
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Location = new System.Drawing.Point(15, 51);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(59, 17);
            this.radioButton10.TabIndex = 11;
            this.radioButton10.Text = "Female";
            this.radioButton10.UseVisualStyleBackColor = true;
            // 
            // radioButton11
            // 
            this.radioButton11.AutoSize = true;
            this.radioButton11.Location = new System.Drawing.Point(15, 75);
            this.radioButton11.Name = "radioButton11";
            this.radioButton11.Size = new System.Drawing.Size(48, 17);
            this.radioButton11.TabIndex = 12;
            this.radioButton11.Text = "Male";
            this.radioButton11.UseVisualStyleBackColor = true;
            // 
            // radioButton12
            // 
            this.radioButton12.AutoSize = true;
            this.radioButton12.Location = new System.Drawing.Point(15, 98);
            this.radioButton12.Name = "radioButton12";
            this.radioButton12.Size = new System.Drawing.Size(111, 17);
            this.radioButton12.TabIndex = 13;
            this.radioButton12.Text = "Decline to Answer";
            this.radioButton12.UseVisualStyleBackColor = true;
            // 
            // maskedTextBox5
            // 
            this.maskedTextBox5.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.maskedTextBox5.Location = new System.Drawing.Point(-289, 48);
            this.maskedTextBox5.Name = "maskedTextBox5";
            this.maskedTextBox5.Size = new System.Drawing.Size(100, 20);
            this.maskedTextBox5.TabIndex = 6;
            this.maskedTextBox5.Text = "age";
            // 
            // maskedTextBox6
            // 
            this.maskedTextBox6.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.maskedTextBox6.Location = new System.Drawing.Point(0, -65);
            this.maskedTextBox6.Name = "maskedTextBox6";
            this.maskedTextBox6.Size = new System.Drawing.Size(58, 20);
            this.maskedTextBox6.TabIndex = 8;
            this.maskedTextBox6.Text = "height";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(-290, -96);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 29);
            this.label9.TabIndex = 15;
            this.label9.Text = "Sex:";
            // 
            // femaleRadio
            // 
            this.femaleRadio.AutoSize = true;
            this.femaleRadio.Location = new System.Drawing.Point(15, 51);
            this.femaleRadio.Name = "femaleRadio";
            this.femaleRadio.Size = new System.Drawing.Size(59, 17);
            this.femaleRadio.TabIndex = 11;
            this.femaleRadio.Text = "Female";
            this.femaleRadio.UseVisualStyleBackColor = true;
            // 
            // maleRadio
            // 
            this.maleRadio.AutoSize = true;
            this.maleRadio.Location = new System.Drawing.Point(15, 75);
            this.maleRadio.Name = "maleRadio";
            this.maleRadio.Size = new System.Drawing.Size(48, 17);
            this.maleRadio.TabIndex = 12;
            this.maleRadio.Text = "Male";
            this.maleRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(230)))));
            this.groupBox6.Controls.Add(this.femaleRadio);
            this.groupBox6.Controls.Add(this.maleRadio);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.declineRadio);
            this.groupBox6.Location = new System.Drawing.Point(30, 76);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 129);
            this.groupBox6.TabIndex = 38;
            this.groupBox6.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(156, 39);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(236, 31);
            this.textBox1.TabIndex = 26;
            this.textBox1.TabStop = false;
            this.textBox1.Text = "Information Collection";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(230)))));
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(561, 339);
            this.panel1.TabIndex = 39;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 312);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(282, 20);
            this.label2.TabIndex = 35;
            this.label2.Text = "Otherwise a default profile will be used.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 292);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(544, 20);
            this.label1.TabIndex = 34;
            this.label1.Text = "If height and weight are entered and saved, they will be used for calculations";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // SubjectInformationScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(230)))));
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.weightInput);
            this.Controls.Add(this.inchInput);
            this.Controls.Add(this.feetInput);
            this.Controls.Add(this.ageInput);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.Name = "SubjectInformationScreen";
            this.Size = new System.Drawing.Size(597, 391);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.RadioButton imperialWeight;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox weightInput;
        private System.Windows.Forms.RadioButton metricHeight;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton imperialHeight;
        private System.Windows.Forms.MaskedTextBox inchInput;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MaskedTextBox feetInput;
        private System.Windows.Forms.MaskedTextBox ageInput;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton declineRadio;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.MaskedTextBox maskedTextBox7;
        private System.Windows.Forms.RadioButton metricWeight;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.RadioButton radioButton11;
        private System.Windows.Forms.RadioButton radioButton12;
        private System.Windows.Forms.MaskedTextBox maskedTextBox5;
        private System.Windows.Forms.MaskedTextBox maskedTextBox6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton femaleRadio;
        private System.Windows.Forms.RadioButton maleRadio;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
