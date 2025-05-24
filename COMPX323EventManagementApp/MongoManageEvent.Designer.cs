namespace COMPX323EventManagementApp
{
    partial class MongoManageEvent
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
            this.buttonDeleteEvent = new System.Windows.Forms.Button();
            this.listViewRSVP = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonDelInstance = new System.Windows.Forms.Button();
            this.listViewEvents = new System.Windows.Forms.ListView();
            this.comboBoxEventList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonDeleteEvent
            // 
            this.buttonDeleteEvent.Location = new System.Drawing.Point(298, 15);
            this.buttonDeleteEvent.Name = "buttonDeleteEvent";
            this.buttonDeleteEvent.Size = new System.Drawing.Size(111, 37);
            this.buttonDeleteEvent.TabIndex = 19;
            this.buttonDeleteEvent.Text = "DELETE EVENT";
            this.buttonDeleteEvent.UseVisualStyleBackColor = true;
            this.buttonDeleteEvent.Click += new System.EventHandler(this.buttonDeleteEvent_Click);
            // 
            // listViewRSVP
            // 
            this.listViewRSVP.HideSelection = false;
            this.listViewRSVP.Location = new System.Drawing.Point(43, 297);
            this.listViewRSVP.Margin = new System.Windows.Forms.Padding(2);
            this.listViewRSVP.Name = "listViewRSVP";
            this.listViewRSVP.Size = new System.Drawing.Size(466, 128);
            this.listViewRSVP.TabIndex = 18;
            this.listViewRSVP.UseCompatibleStateImageBehavior = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Event Instances";
            // 
            // buttonDelInstance
            // 
            this.buttonDelInstance.Location = new System.Drawing.Point(43, 215);
            this.buttonDelInstance.Name = "buttonDelInstance";
            this.buttonDelInstance.Size = new System.Drawing.Size(105, 37);
            this.buttonDelInstance.TabIndex = 16;
            this.buttonDelInstance.Text = "DELETE INSTANCE";
            this.buttonDelInstance.UseVisualStyleBackColor = true;
            this.buttonDelInstance.Click += new System.EventHandler(this.buttonDelInstance_Click);
            // 
            // listViewEvents
            // 
            this.listViewEvents.HideSelection = false;
            this.listViewEvents.Location = new System.Drawing.Point(43, 98);
            this.listViewEvents.Margin = new System.Windows.Forms.Padding(2);
            this.listViewEvents.Name = "listViewEvents";
            this.listViewEvents.Size = new System.Drawing.Size(537, 112);
            this.listViewEvents.TabIndex = 15;
            this.listViewEvents.UseCompatibleStateImageBehavior = false;
            // 
            // comboBoxEventList
            // 
            this.comboBoxEventList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEventList.FormattingEnabled = true;
            this.comboBoxEventList.Location = new System.Drawing.Point(118, 24);
            this.comboBoxEventList.Name = "comboBoxEventList";
            this.comboBoxEventList.Size = new System.Drawing.Size(164, 21);
            this.comboBoxEventList.TabIndex = 14;
            this.comboBoxEventList.DropDown += new System.EventHandler(this.comboBoxEventList_DropDown);
            this.comboBoxEventList.SelectedIndexChanged += new System.EventHandler(this.comboBoxEventList_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 270);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "RSVP List:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Select Event:";
            // 
            // MongoManageEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonDeleteEvent);
            this.Controls.Add(this.listViewRSVP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonDelInstance);
            this.Controls.Add(this.listViewEvents);
            this.Controls.Add(this.comboBoxEventList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MongoManageEvent";
            this.Size = new System.Drawing.Size(800, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonDeleteEvent;
        private System.Windows.Forms.ListView listViewRSVP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonDelInstance;
        private System.Windows.Forms.ListView listViewEvents;
        private System.Windows.Forms.ComboBox comboBoxEventList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}