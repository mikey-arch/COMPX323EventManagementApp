namespace COMPX323EventManagementApp
{
    partial class ProfileControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelAccountNum = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonRsvps = new System.Windows.Forms.Button();
            this.buttonEvents = new System.Windows.Forms.Button();
            this.buttonReviews = new System.Windows.Forms.Button();
            this.listViewDisplay = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(21, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "DETAILS:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.labelEmail);
            this.panel1.Controls.Add(this.labelName);
            this.panel1.Controls.Add(this.labelAccountNum);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(64, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(333, 129);
            this.panel1.TabIndex = 1;
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(108, 98);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(175, 17);
            this.labelEmail.TabIndex = 6;
            this.labelEmail.Text = "Lucas.swag123@gmail.com";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(108, 70);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(75, 17);
            this.labelName.TabIndex = 5;
            this.labelName.Text = "lucas_swag";
            // 
            // labelAccountNum
            // 
            this.labelAccountNum.AutoSize = true;
            this.labelAccountNum.Location = new System.Drawing.Point(108, 42);
            this.labelAccountNum.Name = "labelAccountNum";
            this.labelAccountNum.Size = new System.Drawing.Size(36, 17);
            this.labelAccountNum.TabIndex = 4;
            this.labelAccountNum.Text = "1234";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Email:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Account#:";
            // 
            // buttonRsvps
            // 
            this.buttonRsvps.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRsvps.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.buttonRsvps.Location = new System.Drawing.Point(61, 192);
            this.buttonRsvps.Name = "buttonRsvps";
            this.buttonRsvps.Size = new System.Drawing.Size(223, 49);
            this.buttonRsvps.TabIndex = 2;
            this.buttonRsvps.Text = "RSVPS";
            this.buttonRsvps.UseVisualStyleBackColor = true;
            this.buttonRsvps.Click += new System.EventHandler(this.buttonRsvps_Click);
            // 
            // buttonEvents
            // 
            this.buttonEvents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEvents.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.buttonEvents.Location = new System.Drawing.Point(539, 192);
            this.buttonEvents.Name = "buttonEvents";
            this.buttonEvents.Size = new System.Drawing.Size(225, 49);
            this.buttonEvents.TabIndex = 3;
            this.buttonEvents.Text = "EVENTS";
            this.buttonEvents.UseVisualStyleBackColor = true;
            this.buttonEvents.Click += new System.EventHandler(this.buttonEvents_Click);
            // 
            // buttonReviews
            // 
            this.buttonReviews.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReviews.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.buttonReviews.Location = new System.Drawing.Point(288, 192);
            this.buttonReviews.Name = "buttonReviews";
            this.buttonReviews.Size = new System.Drawing.Size(245, 49);
            this.buttonReviews.TabIndex = 4;
            this.buttonReviews.Text = "REVIEWS";
            this.buttonReviews.UseVisualStyleBackColor = true;
            this.buttonReviews.Click += new System.EventHandler(this.buttonReviews_Click);
            // 
            // listViewDisplay
            // 
            this.listViewDisplay.HideSelection = false;
            this.listViewDisplay.Location = new System.Drawing.Point(61, 247);
            this.listViewDisplay.Name = "listViewDisplay";
            this.listViewDisplay.Size = new System.Drawing.Size(701, 169);
            this.listViewDisplay.TabIndex = 5;
            this.listViewDisplay.UseCompatibleStateImageBehavior = false;
            this.listViewDisplay.DoubleClick += new System.EventHandler(this.listViewDisplay_DoubleClick);
            // 
            // ProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.listViewDisplay);
            this.Controls.Add(this.buttonReviews);
            this.Controls.Add(this.buttonEvents);
            this.Controls.Add(this.buttonRsvps);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ProfileControl";
            this.Size = new System.Drawing.Size(799, 438);
            this.VisibleChanged += new System.EventHandler(this.ProfileControl_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelAccountNum;
        private System.Windows.Forms.Button buttonRsvps;
        private System.Windows.Forms.Button buttonEvents;
        private System.Windows.Forms.Button buttonReviews;
        private System.Windows.Forms.ListView listViewDisplay;
    }
}
