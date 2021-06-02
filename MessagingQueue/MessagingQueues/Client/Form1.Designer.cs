namespace Client
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
            this.OuthputInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OuthputInfo
            // 
            this.OuthputInfo.Location = new System.Drawing.Point(12, 12);
            this.OuthputInfo.Multiline = true;
            this.OuthputInfo.Name = "OuthputInfo";
            this.OuthputInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OuthputInfo.Size = new System.Drawing.Size(736, 237);
            this.OuthputInfo.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 261);
            this.Controls.Add(this.OuthputInfo);
            this.Name = "Form1";
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox OuthputInfo;
    }
}

