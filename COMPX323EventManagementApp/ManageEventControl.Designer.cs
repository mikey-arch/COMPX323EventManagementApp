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
            this.listBoxRSVPDisplay = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBoxEventList = new System.Windows.Forms.ComboBox();
            this.listViewEvents = new System.Windows.Forms.ListView();
            this.buttonDelInstance = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Event:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 241);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "RSVP List:";
            // 
            // listBoxRSVPDisplay
            // 
            this.listBoxRSVPDisplay.FormattingEnabled = true;
            this.listBoxRSVPDisplay.ItemHeight = 16;
            this.listBoxRSVPDisplay.Location = new System.Drawing.Point(62, 271);
            this.listBoxRSVPDisplay.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxRSVPDisplay.Name = "listBoxRSVPDisplay";
            this.listBoxRSVPDisplay.Size = new System.Drawing.Size(555, 148);
            this.listBoxRSVPDisplay.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(416, 24);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(148, 46);
            this.button2.TabIndex = 5;
            this.button2.Text = "EDIT EVENT";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // comboBoxEventList
            // 
            this.comboBoxEventList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEventList.FormattingEnabled = true;
            this.comboBoxEventList.Location = new System.Drawing.Point(161, 36);
            this.comboBoxEventList.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxEventList.Name = "comboBoxEventList";
            this.comboBoxEventList.Size = new System.Drawing.Size(217, 24);
            this.comboBoxEventList.TabIndex = 6;
            this.comboBoxEventList.DropDown += new System.EventHandler(this.comboBoxEventList_DropDown);
            this.comboBoxEventList.SelectedIndexChanged += new System.EventHandler(this.comboBoxEventList_SelectedIndexChanged);
            // 
            // listViewEvents
            // 
            this.listViewEvents.HideSelection = false;
            this.listViewEvents.Location = new System.Drawing.Point(62, 128);
            this.listViewEvents.Name = "listViewEvents";
            this.listViewEvents.Size = new System.Drawing.Size(715, 97);
            this.listViewEvents.TabIndex = 7;
            this.listViewEvents.UseCompatibleStateImageBehavior = false;
            this.listViewEvents.SelectedIndexChanged += new System.EventHandler(this.listViewEvents_SelectedIndexChanged);
            // 
            // buttonDelInstance
            // 
            this.buttonDelInstance.Location = new System.Drawing.Point(637, 271);
            this.buttonDelInstance.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDelInstance.Name = "buttonDelInstance";
            this.buttonDelInstance.Size = new System.Drawing.Size(140, 46);
            this.buttonDelInstance.TabIndex = 8;
            this.buttonDelInstance.Text = "DELETE INSTANCE";
            this.buttonDelInstance.UseVisualStyleBackColor = true;
            this.buttonDelInstance.Click += new System.EventHandler(this.buttonDelInstance_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(59, 98);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Event Instances";
            // 
            // ManageEventControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonDelInstance);
            this.Controls.Add(this.listViewEvents);
            this.Controls.Add(this.comboBoxEventList);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listBoxRSVPDisplay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ManageEventControl";
            this.Size = new System.Drawing.Size(937, 578);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxRSVPDisplay;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBoxEventList;
        private System.Windows.Forms.ListView listViewEvents;
        private System.Windows.Forms.Button buttonDelInstance;
        private System.Windows.Forms.Label label3;
    }
}
