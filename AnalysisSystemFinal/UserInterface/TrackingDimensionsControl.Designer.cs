
namespace AnalysisSystemFinal.UserInterface
{
    partial class TrackingDimensionsControl
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
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblSearchWindow = new System.Windows.Forms.Label();
            this.lblObjectWindow = new System.Windows.Forms.Label();
            this.cmbSearchWindowUnit = new System.Windows.Forms.ComboBox();
            this.cmbBlockWindowUnit = new System.Windows.Forms.ComboBox();
            this.tbSearchHeight = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbSearchWidth = new System.Windows.Forms.TextBox();
            this.tbBlockHeight = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbBlockWidth = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(3, 41);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(137, 13);
            this.lblDescription.TabIndex = 70;
            this.lblDescription.Text = "Default tracking parameters";
            // 
            // lblSearchWindow
            // 
            this.lblSearchWindow.AutoSize = true;
            this.lblSearchWindow.Location = new System.Drawing.Point(54, 106);
            this.lblSearchWindow.Name = "lblSearchWindow";
            this.lblSearchWindow.Size = new System.Drawing.Size(86, 13);
            this.lblSearchWindow.TabIndex = 69;
            this.lblSearchWindow.Text = "Search window :";
            // 
            // lblObjectWindow
            // 
            this.lblObjectWindow.AutoSize = true;
            this.lblObjectWindow.Location = new System.Drawing.Point(54, 79);
            this.lblObjectWindow.Name = "lblObjectWindow";
            this.lblObjectWindow.Size = new System.Drawing.Size(83, 13);
            this.lblObjectWindow.TabIndex = 68;
            this.lblObjectWindow.Text = "Object window :";
            // 
            // cmbSearchWindowUnit
            // 
            this.cmbSearchWindowUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchWindowUnit.Location = new System.Drawing.Point(303, 103);
            this.cmbSearchWindowUnit.Name = "cmbSearchWindowUnit";
            this.cmbSearchWindowUnit.Size = new System.Drawing.Size(116, 21);
            this.cmbSearchWindowUnit.TabIndex = 78;
            // 
            // cmbBlockWindowUnit
            // 
            this.cmbBlockWindowUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlockWindowUnit.Location = new System.Drawing.Point(303, 76);
            this.cmbBlockWindowUnit.Name = "cmbBlockWindowUnit";
            this.cmbBlockWindowUnit.Size = new System.Drawing.Size(116, 21);
            this.cmbBlockWindowUnit.TabIndex = 77;
            // 
            // tbSearchHeight
            // 
            this.tbSearchHeight.Location = new System.Drawing.Point(256, 103);
            this.tbSearchHeight.Name = "tbSearchHeight";
            this.tbSearchHeight.Size = new System.Drawing.Size(30, 20);
            this.tbSearchHeight.TabIndex = 76;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(237, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 75;
            this.label5.Text = "×";
            // 
            // tbSearchWidth
            // 
            this.tbSearchWidth.Location = new System.Drawing.Point(197, 103);
            this.tbSearchWidth.Name = "tbSearchWidth";
            this.tbSearchWidth.Size = new System.Drawing.Size(30, 20);
            this.tbSearchWidth.TabIndex = 74;
            // 
            // tbBlockHeight
            // 
            this.tbBlockHeight.Location = new System.Drawing.Point(256, 77);
            this.tbBlockHeight.Name = "tbBlockHeight";
            this.tbBlockHeight.Size = new System.Drawing.Size(30, 20);
            this.tbBlockHeight.TabIndex = 73;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(237, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 72;
            this.label4.Text = "×";
            // 
            // tbBlockWidth
            // 
            this.tbBlockWidth.Location = new System.Drawing.Point(197, 77);
            this.tbBlockWidth.Name = "tbBlockWidth";
            this.tbBlockWidth.Size = new System.Drawing.Size(30, 20);
            this.tbBlockWidth.TabIndex = 71;
            // 
            // TrackingDimensionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbSearchWindowUnit);
            this.Controls.Add(this.cmbBlockWindowUnit);
            this.Controls.Add(this.tbSearchHeight);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbSearchWidth);
            this.Controls.Add(this.tbBlockHeight);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbBlockWidth);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblSearchWindow);
            this.Controls.Add(this.lblObjectWindow);
            this.Name = "TrackingDimensionsControl";
            this.Size = new System.Drawing.Size(429, 178);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblSearchWindow;
        private System.Windows.Forms.Label lblObjectWindow;
        private System.Windows.Forms.ComboBox cmbSearchWindowUnit;
        private System.Windows.Forms.ComboBox cmbBlockWindowUnit;
        private System.Windows.Forms.TextBox tbSearchHeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbSearchWidth;
        private System.Windows.Forms.TextBox tbBlockHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbBlockWidth;
    }
}
