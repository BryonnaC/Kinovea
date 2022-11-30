
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
            this.sexTextBox = new System.Windows.Forms.TextBox();
            this.orTextBox = new System.Windows.Forms.TextBox();
            this.weightTextBox = new System.Windows.Forms.TextBox();
            this.ageTextBox = new System.Windows.Forms.TextBox();
            this.heightTextBox = new System.Windows.Forms.TextBox();
            this.ageInput = new System.Windows.Forms.MaskedTextBox();
            this.inchInput = new System.Windows.Forms.MaskedTextBox();
            this.feetInput = new System.Windows.Forms.MaskedTextBox();
            this.lbsInput = new System.Windows.Forms.MaskedTextBox();
            this.kgsInput = new System.Windows.Forms.MaskedTextBox();
            this.femaleRadio = new System.Windows.Forms.RadioButton();
            this.maleRadio = new System.Windows.Forms.RadioButton();
            this.declineRadio = new System.Windows.Forms.RadioButton();
            this.nextButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // titleBox
            // 
            this.titleBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleBox.Location = new System.Drawing.Point(280, 5);
            this.titleBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.titleBox.Name = "titleBox";
            this.titleBox.ReadOnly = true;
            this.titleBox.Size = new System.Drawing.Size(352, 43);
            this.titleBox.TabIndex = 0;
            this.titleBox.Text = "Information Collection";
            // 
            // sexTextBox
            // 
            this.sexTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sexTextBox.Location = new System.Drawing.Point(114, 91);
            this.sexTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sexTextBox.Name = "sexTextBox";
            this.sexTextBox.ReadOnly = true;
            this.sexTextBox.Size = new System.Drawing.Size(148, 35);
            this.sexTextBox.TabIndex = 1;
            this.sexTextBox.Text = "Sex:";
            // 
            // orTextBox
            // 
            this.orTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orTextBox.Location = new System.Drawing.Point(554, 354);
            this.orTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.orTextBox.Name = "orTextBox";
            this.orTextBox.ReadOnly = true;
            this.orTextBox.Size = new System.Drawing.Size(148, 30);
            this.orTextBox.TabIndex = 2;
            this.orTextBox.Text = "OR";
            this.orTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // weightTextBox
            // 
            this.weightTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weightTextBox.Location = new System.Drawing.Point(554, 265);
            this.weightTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.weightTextBox.Name = "weightTextBox";
            this.weightTextBox.ReadOnly = true;
            this.weightTextBox.Size = new System.Drawing.Size(148, 35);
            this.weightTextBox.TabIndex = 3;
            this.weightTextBox.Text = "Weight:";
            // 
            // ageTextBox
            // 
            this.ageTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ageTextBox.Location = new System.Drawing.Point(114, 265);
            this.ageTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ageTextBox.Name = "ageTextBox";
            this.ageTextBox.ReadOnly = true;
            this.ageTextBox.Size = new System.Drawing.Size(148, 35);
            this.ageTextBox.TabIndex = 4;
            this.ageTextBox.Text = "Age:";
            // 
            // heightTextBox
            // 
            this.heightTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.heightTextBox.Location = new System.Drawing.Point(548, 91);
            this.heightTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.heightTextBox.Name = "heightTextBox";
            this.heightTextBox.ReadOnly = true;
            this.heightTextBox.Size = new System.Drawing.Size(154, 35);
            this.heightTextBox.TabIndex = 5;
            this.heightTextBox.Text = "Height:";
            // 
            // ageInput
            // 
            this.ageInput.Location = new System.Drawing.Point(114, 314);
            this.ageInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ageInput.Name = "ageInput";
            this.ageInput.Size = new System.Drawing.Size(148, 26);
            this.ageInput.TabIndex = 6;
            // 
            // inchInput
            // 
            this.inchInput.Location = new System.Drawing.Point(644, 140);
            this.inchInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.inchInput.Name = "inchInput";
            this.inchInput.Size = new System.Drawing.Size(58, 26);
            this.inchInput.TabIndex = 7;
            // 
            // feetInput
            // 
            this.feetInput.Location = new System.Drawing.Point(548, 140);
            this.feetInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.feetInput.Name = "feetInput";
            this.feetInput.Size = new System.Drawing.Size(85, 26);
            this.feetInput.TabIndex = 8;
            // 
            // lbsInput
            // 
            this.lbsInput.Location = new System.Drawing.Point(554, 314);
            this.lbsInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lbsInput.Name = "lbsInput";
            this.lbsInput.Size = new System.Drawing.Size(85, 26);
            this.lbsInput.TabIndex = 9;
            // 
            // kgsInput
            // 
            this.kgsInput.Location = new System.Drawing.Point(554, 397);
            this.kgsInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.kgsInput.Name = "kgsInput";
            this.kgsInput.Size = new System.Drawing.Size(85, 26);
            this.kgsInput.TabIndex = 10;
            // 
            // femaleRadio
            // 
            this.femaleRadio.AutoSize = true;
            this.femaleRadio.Location = new System.Drawing.Point(114, 142);
            this.femaleRadio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.femaleRadio.Name = "femaleRadio";
            this.femaleRadio.Size = new System.Drawing.Size(87, 24);
            this.femaleRadio.TabIndex = 11;
            this.femaleRadio.TabStop = true;
            this.femaleRadio.Text = "Female";
            this.femaleRadio.UseVisualStyleBackColor = true;
            // 
            // maleRadio
            // 
            this.maleRadio.AutoSize = true;
            this.maleRadio.Location = new System.Drawing.Point(114, 177);
            this.maleRadio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.maleRadio.Name = "maleRadio";
            this.maleRadio.Size = new System.Drawing.Size(68, 24);
            this.maleRadio.TabIndex = 12;
            this.maleRadio.TabStop = true;
            this.maleRadio.Text = "Male";
            this.maleRadio.UseVisualStyleBackColor = true;
            // 
            // declineRadio
            // 
            this.declineRadio.AutoSize = true;
            this.declineRadio.Location = new System.Drawing.Point(114, 212);
            this.declineRadio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.declineRadio.Name = "declineRadio";
            this.declineRadio.Size = new System.Drawing.Size(162, 24);
            this.declineRadio.TabIndex = 13;
            this.declineRadio.TabStop = true;
            this.declineRadio.Text = "Decline to Answer";
            this.declineRadio.UseVisualStyleBackColor = true;
            // 
            // nextButton
            // 
            this.nextButton.BackColor = System.Drawing.SystemColors.Desktop;
            this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.nextButton.Location = new System.Drawing.Point(783, 451);
            this.nextButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(153, 62);
            this.nextButton.TabIndex = 14;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 530);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.declineRadio);
            this.Controls.Add(this.maleRadio);
            this.Controls.Add(this.femaleRadio);
            this.Controls.Add(this.kgsInput);
            this.Controls.Add(this.lbsInput);
            this.Controls.Add(this.feetInput);
            this.Controls.Add(this.inchInput);
            this.Controls.Add(this.ageInput);
            this.Controls.Add(this.heightTextBox);
            this.Controls.Add(this.ageTextBox);
            this.Controls.Add(this.weightTextBox);
            this.Controls.Add(this.orTextBox);
            this.Controls.Add(this.sexTextBox);
            this.Controls.Add(this.titleBox);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox titleBox;
        private System.Windows.Forms.TextBox sexTextBox;
        private System.Windows.Forms.TextBox orTextBox;
        private System.Windows.Forms.TextBox weightTextBox;
        private System.Windows.Forms.TextBox ageTextBox;
        private System.Windows.Forms.TextBox heightTextBox;
        private System.Windows.Forms.MaskedTextBox ageInput;
        private System.Windows.Forms.MaskedTextBox inchInput;
        private System.Windows.Forms.MaskedTextBox feetInput;
        private System.Windows.Forms.MaskedTextBox lbsInput;
        private System.Windows.Forms.MaskedTextBox kgsInput;
        private System.Windows.Forms.RadioButton femaleRadio;
        private System.Windows.Forms.RadioButton maleRadio;
        private System.Windows.Forms.RadioButton declineRadio;
        private System.Windows.Forms.Button nextButton;
    }
}