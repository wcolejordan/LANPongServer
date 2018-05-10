namespace NetworkedGameServer
{
    partial class Diagnostics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Diagnostics));
            this.label1 = new System.Windows.Forms.Label();
            this.connectedLBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.currentGamesLBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Diagnostics";
            // 
            // connectedLBox
            // 
            this.connectedLBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.connectedLBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.connectedLBox.ForeColor = System.Drawing.Color.Lime;
            this.connectedLBox.FormattingEnabled = true;
            this.connectedLBox.ItemHeight = 16;
            this.connectedLBox.Location = new System.Drawing.Point(20, 72);
            this.connectedLBox.Margin = new System.Windows.Forms.Padding(4);
            this.connectedLBox.Name = "connectedLBox";
            this.connectedLBox.Size = new System.Drawing.Size(235, 354);
            this.connectedLBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Lime;
            this.label2.Location = new System.Drawing.Point(18, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Connected Players";
            // 
            // currentGamesLBox
            // 
            this.currentGamesLBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.currentGamesLBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentGamesLBox.ForeColor = System.Drawing.Color.Lime;
            this.currentGamesLBox.FormattingEnabled = true;
            this.currentGamesLBox.ItemHeight = 16;
            this.currentGamesLBox.Location = new System.Drawing.Point(306, 72);
            this.currentGamesLBox.Name = "currentGamesLBox";
            this.currentGamesLBox.Size = new System.Drawing.Size(223, 354);
            this.currentGamesLBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Lime;
            this.label3.Location = new System.Drawing.Point(303, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Current Games";
            // 
            // Diagnostics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(564, 471);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.currentGamesLBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.connectedLBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Diagnostics";
            this.Text = "Diagnostics";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox connectedLBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox currentGamesLBox;
        private System.Windows.Forms.Label label3;
    }
}

