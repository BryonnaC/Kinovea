
namespace CombineCSV
{
    partial class CombineCSV
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CombineCSV));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblUserMessage = new System.Windows.Forms.Label();
            this.btnCombine = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblXPos = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtXPos = new System.Windows.Forms.TextBox();
            this.btnPasteXPos = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblYPos = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtYPos = new System.Windows.Forms.TextBox();
            this.btnPasteYPos = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.panel1.Size = new System.Drawing.Size(1236, 315);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(10, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1216, 297);
            this.panel2.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.lblUserMessage);
            this.panel3.Controls.Add(this.btnCombine);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel3.Location = new System.Drawing.Point(0, 229);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.panel3.Size = new System.Drawing.Size(1216, 68);
            this.panel3.TabIndex = 2;
            // 
            // lblUserMessage
            // 
            this.lblUserMessage.AutoSize = true;
            this.lblUserMessage.Font = new System.Drawing.Font("Bahnschrift", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserMessage.ForeColor = System.Drawing.SystemColors.Control;
            this.lblUserMessage.Location = new System.Drawing.Point(216, 22);
            this.lblUserMessage.Name = "lblUserMessage";
            this.lblUserMessage.Size = new System.Drawing.Size(0, 27);
            this.lblUserMessage.TabIndex = 3;
            // 
            // btnCombine
            // 
            this.btnCombine.BackColor = System.Drawing.SystemColors.Control;
            this.btnCombine.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCombine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCombine.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCombine.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCombine.Location = new System.Drawing.Point(10, 9);
            this.btnCombine.Name = "btnCombine";
            this.btnCombine.Size = new System.Drawing.Size(200, 50);
            this.btnCombine.TabIndex = 2;
            this.btnCombine.Text = "Combine";
            this.btnCombine.UseVisualStyleBackColor = false;
            this.btnCombine.Click += new System.EventHandler(this.btnCombine_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1214, 225);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.lblXPos);
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 9);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1194, 94);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // lblXPos
            // 
            this.lblXPos.AutoSize = true;
            this.lblXPos.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXPos.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblXPos.Location = new System.Drawing.Point(3, 0);
            this.lblXPos.MinimumSize = new System.Drawing.Size(80, 29);
            this.lblXPos.Name = "lblXPos";
            this.lblXPos.Size = new System.Drawing.Size(255, 29);
            this.lblXPos.TabIndex = 0;
            this.lblXPos.Text = "Path to X positions file:";
            this.toolTip.SetToolTip(this.lblXPos, resources.GetString("lblXPos.ToolTip"));
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 94.14317F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.856833F));
            this.tableLayoutPanel2.Controls.Add(this.txtXPos, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnPasteXPos, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 32);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1190, 57);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // txtXPos
            // 
            this.txtXPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtXPos.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtXPos.Location = new System.Drawing.Point(4, 10);
            this.txtXPos.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.txtXPos.Name = "txtXPos";
            this.txtXPos.Size = new System.Drawing.Size(1112, 36);
            this.txtXPos.TabIndex = 0;
            // 
            // btnPasteXPos
            // 
            this.btnPasteXPos.AccessibleDescription = "";
            this.btnPasteXPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPasteXPos.FlatAppearance.BorderSize = 0;
            this.btnPasteXPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPasteXPos.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPasteXPos.Image = global::CombineCSV.Properties.Resources.paste_32;
            this.btnPasteXPos.Location = new System.Drawing.Point(1126, 5);
            this.btnPasteXPos.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnPasteXPos.Name = "btnPasteXPos";
            this.btnPasteXPos.Size = new System.Drawing.Size(60, 47);
            this.btnPasteXPos.TabIndex = 2;
            this.btnPasteXPos.TabStop = false;
            this.toolTip.SetToolTip(this.btnPasteXPos, "Paste path from clipboard");
            this.btnPasteXPos.UseVisualStyleBackColor = true;
            this.btnPasteXPos.Click += new System.EventHandler(this.btnPasteXPos_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.lblYPos);
            this.flowLayoutPanel2.Controls.Add(this.tableLayoutPanel3);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(10, 121);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1194, 95);
            this.flowLayoutPanel2.TabIndex = 2;
            // 
            // lblYPos
            // 
            this.lblYPos.AutoSize = true;
            this.lblYPos.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYPos.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblYPos.Location = new System.Drawing.Point(3, 0);
            this.lblYPos.MinimumSize = new System.Drawing.Size(80, 29);
            this.lblYPos.Name = "lblYPos";
            this.lblYPos.Size = new System.Drawing.Size(253, 29);
            this.lblYPos.TabIndex = 0;
            this.lblYPos.Text = "Path to Y positions file:";
            this.toolTip.SetToolTip(this.lblYPos, resources.GetString("lblYPos.ToolTip"));
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 94.14317F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.856833F));
            this.tableLayoutPanel3.Controls.Add(this.txtYPos, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnPasteYPos, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 32);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1185, 55);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // txtYPos
            // 
            this.txtYPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtYPos.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtYPos.Location = new System.Drawing.Point(4, 10);
            this.txtYPos.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.txtYPos.Name = "txtYPos";
            this.txtYPos.Size = new System.Drawing.Size(1108, 36);
            this.txtYPos.TabIndex = 1;
            // 
            // btnPasteYPos
            // 
            this.btnPasteYPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPasteYPos.FlatAppearance.BorderSize = 0;
            this.btnPasteYPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPasteYPos.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPasteYPos.Image = global::CombineCSV.Properties.Resources.paste_32;
            this.btnPasteYPos.Location = new System.Drawing.Point(1122, 5);
            this.btnPasteYPos.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnPasteYPos.Name = "btnPasteYPos";
            this.btnPasteYPos.Size = new System.Drawing.Size(59, 45);
            this.btnPasteYPos.TabIndex = 2;
            this.btnPasteYPos.TabStop = false;
            this.toolTip.SetToolTip(this.btnPasteYPos, "Paste path from clipboard");
            this.btnPasteYPos.UseVisualStyleBackColor = true;
            this.btnPasteYPos.Click += new System.EventHandler(this.btnPasteYPos_Click);
            // 
            // CombineCSV
            // 
            this.AcceptButton = this.btnCombine;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1236, 315);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimumSize = new System.Drawing.Size(97, 91);
            this.Name = "CombineCSV";
            this.Text = "CombineCSV";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblXPos;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label lblYPos;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnCombine;
        private System.Windows.Forms.Button btnPasteXPos;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtXPos;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox txtYPos;
        private System.Windows.Forms.Button btnPasteYPos;
        private System.Windows.Forms.Label lblUserMessage;
        private System.Windows.Forms.ToolTip toolTip;
    }
}

