namespace COMPX323EventManagementApp
{
    partial class EventDetails
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelEventName = new System.Windows.Forms.Label();
            this.labelExit = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButtonInterested = new System.Windows.Forms.RadioButton();
            this.radioButtonAttending = new System.Windows.Forms.RadioButton();
            this.labelRSVP = new System.Windows.Forms.Label();
            this.buttonSubmitRSVP = new System.Windows.Forms.Button();
            this.textBoxDate = new System.Windows.Forms.TextBox();
            this.textBoxVenue = new System.Windows.Forms.TextBox();
            this.labelDate = new System.Windows.Forms.Label();
            this.labelVenue = new System.Windows.Forms.Label();
            this.labelTags = new System.Windows.Forms.Label();
            this.textBoxTags = new System.Windows.Forms.TextBox();
            this.labelDesc = new System.Windows.Forms.Label();
            this.textBoxDesc = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.textBoxRestrictions = new System.Windows.Forms.TextBox();
            this.labelRestrictions = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.textBoxPrice = new System.Windows.Forms.TextBox();
            this.labelPrice = new System.Windows.Forms.Label();
            this.textBoxTime = new System.Windows.Forms.TextBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.labelCurrStatus = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelAccNum = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelExitButton = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.labelExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 74);
            this.panel2.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.panel3.Controls.Add(this.labelExitButton);
            this.panel3.Controls.Add(this.labelName);
            this.panel3.Controls.Add(this.labelAccNum);
            this.panel3.Controls.Add(this.labelEventName);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(800, 74);
            this.panel3.TabIndex = 11;
            // 
            // labelEventName
            // 
            this.labelEventName.AutoSize = true;
            this.labelEventName.Font = new System.Drawing.Font("Nirmala UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEventName.ForeColor = System.Drawing.Color.Black;
            this.labelEventName.Location = new System.Drawing.Point(25, 21);
            this.labelEventName.Name = "labelEventName";
            this.labelEventName.Size = new System.Drawing.Size(212, 37);
            this.labelEventName.TabIndex = 0;
            this.labelEventName.Text = "EVENT DETAILS";
            // 
            // labelExit
            // 
            this.labelExit.AutoSize = true;
            this.labelExit.Location = new System.Drawing.Point(1037, 9);
            this.labelExit.Name = "labelExit";
            this.labelExit.Size = new System.Drawing.Size(15, 16);
            this.labelExit.TabIndex = 10;
            this.labelExit.Text = "X";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.panel1.Controls.Add(this.labelStatus);
            this.panel1.Controls.Add(this.labelCurrStatus);
            this.panel1.Controls.Add(this.radioButtonInterested);
            this.panel1.Controls.Add(this.radioButtonAttending);
            this.panel1.Controls.Add(this.labelRSVP);
            this.panel1.Controls.Add(this.buttonSubmitRSVP);
            this.panel1.Location = new System.Drawing.Point(541, 95);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(247, 343);
            this.panel1.TabIndex = 6;
            // 
            // radioButtonInterested
            // 
            this.radioButtonInterested.AutoSize = true;
            this.radioButtonInterested.Font = new System.Drawing.Font("Nirmala UI", 10.75F, System.Drawing.FontStyle.Bold);
            this.radioButtonInterested.Location = new System.Drawing.Point(58, 205);
            this.radioButtonInterested.Name = "radioButtonInterested";
            this.radioButtonInterested.Size = new System.Drawing.Size(120, 29);
            this.radioButtonInterested.TabIndex = 3;
            this.radioButtonInterested.TabStop = true;
            this.radioButtonInterested.Text = "Interested";
            this.radioButtonInterested.UseVisualStyleBackColor = true;
            // 
            // radioButtonAttending
            // 
            this.radioButtonAttending.AutoSize = true;
            this.radioButtonAttending.Font = new System.Drawing.Font("Nirmala UI", 10.75F, System.Drawing.FontStyle.Bold);
            this.radioButtonAttending.Location = new System.Drawing.Point(58, 170);
            this.radioButtonAttending.Name = "radioButtonAttending";
            this.radioButtonAttending.Size = new System.Drawing.Size(119, 29);
            this.radioButtonAttending.TabIndex = 2;
            this.radioButtonAttending.TabStop = true;
            this.radioButtonAttending.Text = "Attending";
            this.radioButtonAttending.UseVisualStyleBackColor = true;
            // 
            // labelRSVP
            // 
            this.labelRSVP.AutoSize = true;
            this.labelRSVP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.labelRSVP.Font = new System.Drawing.Font("Nirmala UI", 15.75F, System.Drawing.FontStyle.Bold);
            this.labelRSVP.Location = new System.Drawing.Point(83, 36);
            this.labelRSVP.Name = "labelRSVP";
            this.labelRSVP.Size = new System.Drawing.Size(85, 37);
            this.labelRSVP.TabIndex = 1;
            this.labelRSVP.Text = "RSVP";
            this.labelRSVP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSubmitRSVP
            // 
            this.buttonSubmitRSVP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.buttonSubmitRSVP.Location = new System.Drawing.Point(35, 275);
            this.buttonSubmitRSVP.Name = "buttonSubmitRSVP";
            this.buttonSubmitRSVP.Size = new System.Drawing.Size(182, 50);
            this.buttonSubmitRSVP.TabIndex = 0;
            this.buttonSubmitRSVP.Text = "Submit/Update";
            this.buttonSubmitRSVP.UseVisualStyleBackColor = true;
            this.buttonSubmitRSVP.Click += new System.EventHandler(this.buttonSubmitRSVP_Click);
            // 
            // textBoxDate
            // 
            this.textBoxDate.Location = new System.Drawing.Point(105, 16);
            this.textBoxDate.Name = "textBoxDate";
            this.textBoxDate.ReadOnly = true;
            this.textBoxDate.Size = new System.Drawing.Size(100, 22);
            this.textBoxDate.TabIndex = 9;
            this.textBoxDate.Text = "Date";
            // 
            // textBoxVenue
            // 
            this.textBoxVenue.Location = new System.Drawing.Point(105, 60);
            this.textBoxVenue.Name = "textBoxVenue";
            this.textBoxVenue.ReadOnly = true;
            this.textBoxVenue.Size = new System.Drawing.Size(149, 22);
            this.textBoxVenue.TabIndex = 10;
            this.textBoxVenue.Text = "Venue";
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(12, 19);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(39, 16);
            this.labelDate.TabIndex = 11;
            this.labelDate.Text = "Date:";
            // 
            // labelVenue
            // 
            this.labelVenue.AutoSize = true;
            this.labelVenue.Location = new System.Drawing.Point(12, 66);
            this.labelVenue.Name = "labelVenue";
            this.labelVenue.Size = new System.Drawing.Size(49, 16);
            this.labelVenue.TabIndex = 12;
            this.labelVenue.Text = "Venue:";
            // 
            // labelTags
            // 
            this.labelTags.AutoSize = true;
            this.labelTags.Location = new System.Drawing.Point(7, 17);
            this.labelTags.Name = "labelTags";
            this.labelTags.Size = new System.Drawing.Size(42, 16);
            this.labelTags.TabIndex = 13;
            this.labelTags.Text = "Tags:";
            // 
            // textBoxTags
            // 
            this.textBoxTags.Location = new System.Drawing.Point(111, 17);
            this.textBoxTags.Name = "textBoxTags";
            this.textBoxTags.ReadOnly = true;
            this.textBoxTags.Size = new System.Drawing.Size(208, 22);
            this.textBoxTags.TabIndex = 14;
            this.textBoxTags.Text = "tags...";
            // 
            // labelDesc
            // 
            this.labelDesc.AutoSize = true;
            this.labelDesc.Location = new System.Drawing.Point(9, 79);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(78, 16);
            this.labelDesc.TabIndex = 15;
            this.labelDesc.Text = "Description:";
            // 
            // textBoxDesc
            // 
            this.textBoxDesc.Location = new System.Drawing.Point(111, 76);
            this.textBoxDesc.Multiline = true;
            this.textBoxDesc.Name = "textBoxDesc";
            this.textBoxDesc.ReadOnly = true;
            this.textBoxDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDesc.Size = new System.Drawing.Size(357, 67);
            this.textBoxDesc.TabIndex = 16;
            this.textBoxDesc.Text = "description...";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.textBoxRestrictions);
            this.panel4.Controls.Add(this.labelRestrictions);
            this.panel4.Controls.Add(this.textBoxTags);
            this.panel4.Controls.Add(this.textBoxDesc);
            this.panel4.Controls.Add(this.labelTags);
            this.panel4.Controls.Add(this.labelDesc);
            this.panel4.Location = new System.Drawing.Point(32, 95);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(487, 143);
            this.panel4.TabIndex = 17;
            // 
            // textBoxRestrictions
            // 
            this.textBoxRestrictions.Location = new System.Drawing.Point(111, 49);
            this.textBoxRestrictions.Name = "textBoxRestrictions";
            this.textBoxRestrictions.ReadOnly = true;
            this.textBoxRestrictions.Size = new System.Drawing.Size(357, 22);
            this.textBoxRestrictions.TabIndex = 18;
            this.textBoxRestrictions.Text = "restrictions...";
            // 
            // labelRestrictions
            // 
            this.labelRestrictions.AutoSize = true;
            this.labelRestrictions.Location = new System.Drawing.Point(9, 52);
            this.labelRestrictions.Name = "labelRestrictions";
            this.labelRestrictions.Size = new System.Drawing.Size(80, 16);
            this.labelRestrictions.TabIndex = 17;
            this.labelRestrictions.Text = "Restrictions:";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.textBoxPrice);
            this.panel5.Controls.Add(this.labelPrice);
            this.panel5.Controls.Add(this.textBoxTime);
            this.panel5.Controls.Add(this.labelTime);
            this.panel5.Controls.Add(this.textBoxVenue);
            this.panel5.Controls.Add(this.textBoxDate);
            this.panel5.Controls.Add(this.labelVenue);
            this.panel5.Controls.Add(this.labelDate);
            this.panel5.Location = new System.Drawing.Point(32, 265);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(487, 173);
            this.panel5.TabIndex = 18;
            // 
            // textBoxPrice
            // 
            this.textBoxPrice.Location = new System.Drawing.Point(105, 105);
            this.textBoxPrice.Name = "textBoxPrice";
            this.textBoxPrice.ReadOnly = true;
            this.textBoxPrice.Size = new System.Drawing.Size(149, 22);
            this.textBoxPrice.TabIndex = 17;
            this.textBoxPrice.Text = "Price";
            // 
            // labelPrice
            // 
            this.labelPrice.AutoSize = true;
            this.labelPrice.Location = new System.Drawing.Point(12, 111);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(41, 16);
            this.labelPrice.TabIndex = 15;
            this.labelPrice.Text = "Price:";
            // 
            // textBoxTime
            // 
            this.textBoxTime.Location = new System.Drawing.Point(358, 19);
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.ReadOnly = true;
            this.textBoxTime.Size = new System.Drawing.Size(100, 22);
            this.textBoxTime.TabIndex = 13;
            this.textBoxTime.Text = "Time";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(265, 22);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(41, 16);
            this.labelTime.TabIndex = 14;
            this.labelTime.Text = "Time:";
            // 
            // labelCurrStatus
            // 
            this.labelCurrStatus.AutoSize = true;
            this.labelCurrStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.labelCurrStatus.Location = new System.Drawing.Point(54, 86);
            this.labelCurrStatus.Name = "labelCurrStatus";
            this.labelCurrStatus.Size = new System.Drawing.Size(136, 22);
            this.labelCurrStatus.TabIndex = 19;
            this.labelCurrStatus.Text = "Current Status: ";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(86, 121);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(61, 22);
            this.labelStatus.TabIndex = 20;
            this.labelStatus.Text = "Status";
            // 
            // labelAccNum
            // 
            this.labelAccNum.AutoSize = true;
            this.labelAccNum.Location = new System.Drawing.Point(550, 14);
            this.labelAccNum.Name = "labelAccNum";
            this.labelAccNum.Size = new System.Drawing.Size(89, 16);
            this.labelAccNum.TabIndex = 19;
            this.labelAccNum.Text = "Account Num:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(550, 42);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(47, 16);
            this.labelName.TabIndex = 20;
            this.labelName.Text = "Name:";
            // 
            // labelExitButton
            // 
            this.labelExitButton.AutoSize = true;
            this.labelExitButton.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelExitButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(165)))), ((int)(((byte)(169)))));
            this.labelExitButton.Location = new System.Drawing.Point(778, 5);
            this.labelExitButton.Name = "labelExitButton";
            this.labelExitButton.Size = new System.Drawing.Size(21, 23);
            this.labelExitButton.TabIndex = 38;
            this.labelExitButton.Text = "X";
            this.labelExitButton.Click += new System.EventHandler(this.labelExitButton_Click);
            // 
            // EventDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EventDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EventDetails";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelExit;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelEventName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxDate;
        private System.Windows.Forms.TextBox textBoxVenue;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.Label labelVenue;
        private System.Windows.Forms.Label labelRSVP;
        private System.Windows.Forms.Button buttonSubmitRSVP;
        private System.Windows.Forms.Label labelTags;
        private System.Windows.Forms.TextBox textBoxTags;
        private System.Windows.Forms.Label labelDesc;
        private System.Windows.Forms.TextBox textBoxDesc;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox textBoxRestrictions;
        private System.Windows.Forms.Label labelRestrictions;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox textBoxPrice;
        private System.Windows.Forms.Label labelPrice;
        private System.Windows.Forms.TextBox textBoxTime;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.RadioButton radioButtonInterested;
        private System.Windows.Forms.RadioButton radioButtonAttending;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelCurrStatus;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelAccNum;
        private System.Windows.Forms.Label labelExitButton;
    }
}