namespace COMPX323EventManagementApp
{
    partial class ManageEventControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxEventDisplay = new System.Windows.Forms.ListBox();
            this.listBoxRSVPDisplay = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBoxEventList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Event:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "RSVP List:";
            // 
            // listBoxEventDisplay
            // 
            this.listBoxEventDisplay.FormattingEnabled = true;
            this.listBoxEventDisplay.Location = new System.Drawing.Point(47, 74);
            this.listBoxEventDisplay.Name = "listBoxEventDisplay";
            this.listBoxEventDisplay.Size = new System.Drawing.Size(417, 69);
            this.listBoxEventDisplay.TabIndex = 2;
            // 
            // listBoxRSVPDisplay
            // 
            this.listBoxRSVPDisplay.FormattingEnabled = true;
            this.listBoxRSVPDisplay.Location = new System.Drawing.Point(47, 192);
            this.listBoxRSVPDisplay.Name = "listBoxRSVPDisplay";
            this.listBoxRSVPDisplay.Size = new System.Drawing.Size(417, 121);
            this.listBoxRSVPDisplay.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(47, 316);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 37);
            this.button1.TabIndex = 4;
            this.button1.Text = "CANCEL EVENT";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(353, 316);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 37);
            this.button2.TabIndex = 5;
            this.button2.Text = "EDIT EVENT";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // comboBoxEventList
            // 
            this.comboBoxEventList.FormattingEnabled = true;
            this.comboBoxEventList.Location = new System.Drawing.Point(121, 33);
            this.comboBoxEventList.Name = "comboBoxEventList";
            this.comboBoxEventList.Size = new System.Drawing.Size(164, 21);
            this.comboBoxEventList.TabIndex = 6;
            // 
            // ManageEventControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxEventList);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBoxRSVPDisplay);
            this.Controls.Add(this.listBoxEventDisplay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ManageEventControl";
            this.Size = new System.Drawing.Size(703, 470);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxEventDisplay;
        private System.Windows.Forms.ListBox listBoxRSVPDisplay;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBoxEventList;
    }
}
