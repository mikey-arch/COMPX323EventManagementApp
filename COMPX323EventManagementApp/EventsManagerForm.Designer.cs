namespace COMPX323EventManagementApp
{
    partial class EventsManagerForm
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
            this.buttonLogout = new System.Windows.Forms.Button();
            this.buttonCreateReview = new System.Windows.Forms.Button();
            this.buttonManageEvent = new System.Windows.Forms.Button();
            this.buttonProfile = new System.Windows.Forms.Button();
            this.buttonCreateEvent = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelExit = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelProfilePicName = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonLogout
            // 
            this.buttonLogout.BackColor = System.Drawing.Color.White;
            this.buttonLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonLogout.FlatAppearance.BorderSize = 0;
            this.buttonLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogout.Location = new System.Drawing.Point(-14, 661);
            this.buttonLogout.Name = "buttonLogout";
            this.buttonLogout.Size = new System.Drawing.Size(218, 48);
            this.buttonLogout.TabIndex = 7;
            this.buttonLogout.Text = "LOG OUT";
            this.buttonLogout.UseVisualStyleBackColor = false;
            this.buttonLogout.Click += new System.EventHandler(this.buttonLogout_Click);
            // 
            // buttonCreateReview
            // 
            this.buttonCreateReview.BackColor = System.Drawing.Color.White;
            this.buttonCreateReview.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCreateReview.FlatAppearance.BorderSize = 0;
            this.buttonCreateReview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCreateReview.Location = new System.Drawing.Point(-2, 534);
            this.buttonCreateReview.Name = "buttonCreateReview";
            this.buttonCreateReview.Size = new System.Drawing.Size(204, 86);
            this.buttonCreateReview.TabIndex = 6;
            this.buttonCreateReview.Text = "CREATE REVIEW";
            this.buttonCreateReview.UseVisualStyleBackColor = false;
            this.buttonCreateReview.Click += new System.EventHandler(this.buttonCreateReview_Click);
            // 
            // buttonManageEvent
            // 
            this.buttonManageEvent.BackColor = System.Drawing.Color.White;
            this.buttonManageEvent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonManageEvent.FlatAppearance.BorderSize = 0;
            this.buttonManageEvent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonManageEvent.Location = new System.Drawing.Point(-12, 446);
            this.buttonManageEvent.Name = "buttonManageEvent";
            this.buttonManageEvent.Size = new System.Drawing.Size(214, 92);
            this.buttonManageEvent.TabIndex = 5;
            this.buttonManageEvent.Text = "MANAGE EVENT";
            this.buttonManageEvent.UseVisualStyleBackColor = false;
            this.buttonManageEvent.Click += new System.EventHandler(this.buttonManageEvent_Click);
            // 
            // buttonProfile
            // 
            this.buttonProfile.BackColor = System.Drawing.Color.White;
            this.buttonProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonProfile.FlatAppearance.BorderSize = 0;
            this.buttonProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProfile.Location = new System.Drawing.Point(-5, 159);
            this.buttonProfile.Name = "buttonProfile";
            this.buttonProfile.Size = new System.Drawing.Size(204, 103);
            this.buttonProfile.TabIndex = 4;
            this.buttonProfile.Text = "VIEW PROFILE";
            this.buttonProfile.UseVisualStyleBackColor = false;
            this.buttonProfile.Click += new System.EventHandler(this.buttonProfile_Click);
            // 
            // buttonCreateEvent
            // 
            this.buttonCreateEvent.BackColor = System.Drawing.Color.White;
            this.buttonCreateEvent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCreateEvent.FlatAppearance.BorderSize = 0;
            this.buttonCreateEvent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCreateEvent.Location = new System.Drawing.Point(-12, 357);
            this.buttonCreateEvent.Name = "buttonCreateEvent";
            this.buttonCreateEvent.Size = new System.Drawing.Size(216, 92);
            this.buttonCreateEvent.TabIndex = 4;
            this.buttonCreateEvent.Text = "CREATE EVENT";
            this.buttonCreateEvent.UseVisualStyleBackColor = false;
            this.buttonCreateEvent.Click += new System.EventHandler(this.buttonCreateEvent_Click);
            // 
            // buttonSearch
            // 
            this.buttonSearch.BackColor = System.Drawing.Color.White;
            this.buttonSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonSearch.FlatAppearance.BorderSize = 0;
            this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSearch.Location = new System.Drawing.Point(-2, 259);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(202, 101);
            this.buttonSearch.TabIndex = 3;
            this.buttonSearch.Text = "SEARCH FOR EVENT";
            this.buttonSearch.UseVisualStyleBackColor = false;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.panel2.Controls.Add(this.labelExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1232, 47);
            this.panel2.TabIndex = 3;
            // 
            // labelExit
            // 
            this.labelExit.AutoSize = true;
            this.labelExit.Location = new System.Drawing.Point(1199, 9);
            this.labelExit.Name = "labelExit";
            this.labelExit.Size = new System.Drawing.Size(17, 17);
            this.labelExit.TabIndex = 10;
            this.labelExit.Text = "X";
            this.labelExit.Click += new System.EventHandler(this.labelExit_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.labelProfilePicName);
            this.panel1.Controls.Add(this.buttonSearch);
            this.panel1.Controls.Add(this.buttonLogout);
            this.panel1.Controls.Add(this.buttonProfile);
            this.panel1.Controls.Add(this.buttonCreateReview);
            this.panel1.Controls.Add(this.buttonCreateEvent);
            this.panel1.Controls.Add(this.buttonManageEvent);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(197, 709);
            this.panel1.TabIndex = 8;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::COMPX323EventManagementApp.Properties.Resources.blank_profile_picture;
            this.pictureBox1.Location = new System.Drawing.Point(35, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(136, 111);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // labelProfilePicName
            // 
            this.labelProfilePicName.AutoSize = true;
            this.labelProfilePicName.Location = new System.Drawing.Point(60, 114);
            this.labelProfilePicName.Name = "labelProfilePicName";
            this.labelProfilePicName.Size = new System.Drawing.Size(75, 17);
            this.labelProfilePicName.TabIndex = 8;
            this.labelProfilePicName.Text = "User Name";
            this.labelProfilePicName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.panel3.Controls.Add(this.labelTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(197, 47);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1035, 42);
            this.panel3.TabIndex = 9;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Nirmala UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.Black;
            this.labelTitle.Location = new System.Drawing.Point(3, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(212, 30);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "SEARCH FOR EVENT";
            // 
            // panelContent
            // 
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(197, 89);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1035, 667);
            this.panelContent.TabIndex = 10;
            // 
            // EventsManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1232, 756);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EventsManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EventsManagerForm";
            this.Load += new System.EventHandler(this.EventsManagerForm_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonProfile;
        private System.Windows.Forms.Button buttonCreateEvent;
        private System.Windows.Forms.Button buttonManageEvent;
        private System.Windows.Forms.Button buttonLogout;
        private System.Windows.Forms.Button buttonCreateReview;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelProfilePicName;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label labelExit;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}