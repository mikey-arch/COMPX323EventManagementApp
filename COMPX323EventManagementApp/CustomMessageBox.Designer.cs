namespace COMPX323EventManagementApp
{
    partial class CustomMessageBox
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
            this.labelExit = new System.Windows.Forms.Label();
            this.labelMsg = new System.Windows.Forms.Label();
            this.buttonDelRSVP = new System.Windows.Forms.Button();
            this.buttonEventDetails = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelExit
            // 
            this.labelExit.AutoSize = true;
            this.labelExit.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.labelExit.Location = new System.Drawing.Point(381, 9);
            this.labelExit.Name = "labelExit";
            this.labelExit.Size = new System.Drawing.Size(21, 23);
            this.labelExit.TabIndex = 38;
            this.labelExit.Text = "X";
            this.labelExit.Click += new System.EventHandler(this.labelExit_Click);
            // 
            // labelMsg
            // 
            this.labelMsg.AutoSize = true;
            this.labelMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.labelMsg.Location = new System.Drawing.Point(12, 33);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(132, 22);
            this.labelMsg.TabIndex = 39;
            this.labelMsg.Text = "Choose Action:";
            // 
            // buttonDelRSVP
            // 
            this.buttonDelRSVP.Location = new System.Drawing.Point(35, 107);
            this.buttonDelRSVP.Name = "buttonDelRSVP";
            this.buttonDelRSVP.Size = new System.Drawing.Size(109, 51);
            this.buttonDelRSVP.TabIndex = 40;
            this.buttonDelRSVP.Text = "Delete RSVP";
            this.buttonDelRSVP.UseVisualStyleBackColor = true;
            this.buttonDelRSVP.Click += new System.EventHandler(this.buttonDelRSVP_Click);
            // 
            // buttonEventDetails
            // 
            this.buttonEventDetails.Location = new System.Drawing.Point(259, 107);
            this.buttonEventDetails.Name = "buttonEventDetails";
            this.buttonEventDetails.Size = new System.Drawing.Size(109, 51);
            this.buttonEventDetails.TabIndex = 41;
            this.buttonEventDetails.Text = "View Event Details";
            this.buttonEventDetails.UseVisualStyleBackColor = true;
            this.buttonEventDetails.Click += new System.EventHandler(this.buttonEventDetails_Click);
            // 
            // CustomMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 190);
            this.Controls.Add(this.buttonEventDetails);
            this.Controls.Add(this.buttonDelRSVP);
            this.Controls.Add(this.labelMsg);
            this.Controls.Add(this.labelExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomMessageBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelExit;
        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.Button buttonDelRSVP;
        private System.Windows.Forms.Button buttonEventDetails;
    }
}