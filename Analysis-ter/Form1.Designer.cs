
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
            this.weightInput = new System.Windows.Forms.MaskedTextBox();
            this.femaleRadio = new System.Windows.Forms.RadioButton();
            this.maleRadio = new System.Windows.Forms.RadioButton();
            this.declineRadio = new System.Windows.Forms.RadioButton();
            this.nextButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.inchInput = new System.Windows.Forms.MaskedTextBox();
            this.metricHeight = new System.Windows.Forms.RadioButton();
            this.imperialHeight = new System.Windows.Forms.RadioButton();
            this.metricWeight = new System.Windows.Forms.RadioButton();
            this.imperialWeight = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.ageInput.Click += new System.EventHandler(this.ageInput_Click);
            // 
            // feetInput
            // 
            this.feetInput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.feetInput.Location = new System.Drawing.Point(365, 91);
            this.feetInput.Name = "feetInput";
            this.feetInput.Size = new System.Drawing.Size(58, 20);
            this.feetInput.TabIndex = 8;
            this.feetInput.Text = "height";
            this.feetInput.Click += new System.EventHandler(this.feetInput_Click);
            // 
            // weightInput
            // 
            this.weightInput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.weightInput.Location = new System.Drawing.Point(369, 204);
            this.weightInput.Name = "weightInput";
            this.weightInput.Size = new System.Drawing.Size(58, 20);
            this.weightInput.TabIndex = 9;
            this.weightInput.Text = "weight";
            this.weightInput.Click += new System.EventHandler(this.lbsInput_Click);
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
            // inchInput
            // 
            this.inchInput.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.inchInput.Location = new System.Drawing.Point(365, 117);
            this.inchInput.Name = "inchInput";
            this.inchInput.Size = new System.Drawing.Size(58, 20);
            this.inchInput.TabIndex = 23;
            this.inchInput.Text = "height";
            this.inchInput.Click += new System.EventHandler(this.inchInput_Click);
            this.inchInput.TabIndexChanged += new System.EventHandler(this.inchInput_TabIndexChanged);
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
            this.metricHeight.CheckedChanged += new System.EventHandler(this.metricHeight_CheckedChanged);
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
            this.imperialHeight.CheckedChanged += new System.EventHandler(this.imperialHeight_CheckedChanged);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.imperialHeight);
            this.groupBox1.Controls.Add(this.metricHeight);
            this.groupBox1.Location = new System.Drawing.Point(365, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.imperialWeight);
            this.groupBox2.Controls.Add(this.metricWeight);
            this.groupBox2.Location = new System.Drawing.Point(365, 156);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.femaleRadio);
            this.groupBox3.Controls.Add(this.maleRadio);
            this.groupBox3.Controls.Add(this.declineRadio);
            this.groupBox3.Location = new System.Drawing.Point(61, 40);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 129);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(643, 344);
            this.Controls.Add(this.inchInput);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.weightInput);
            this.Controls.Add(this.feetInput);
            this.Controls.Add(this.ageInput);
            this.Controls.Add(this.titleBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "Form1";
            this.Text = "Information Collection";
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

        private System.Windows.Forms.TextBox titleBox;
        private System.Windows.Forms.MaskedTextBox ageInput;
        private System.Windows.Forms.MaskedTextBox feetInput;
        private System.Windows.Forms.MaskedTextBox weightInput;
        private System.Windows.Forms.RadioButton femaleRadio;
        private System.Windows.Forms.RadioButton maleRadio;
        private System.Windows.Forms.RadioButton declineRadio;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox inchInput;
        private System.Windows.Forms.RadioButton metricHeight;
        private System.Windows.Forms.RadioButton imperialHeight;
        private System.Windows.Forms.RadioButton metricWeight;
        private System.Windows.Forms.RadioButton imperialWeight;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}