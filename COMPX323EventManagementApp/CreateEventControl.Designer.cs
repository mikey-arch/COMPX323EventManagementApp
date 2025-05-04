namespace COMPX323EventManagementApp
{
    partial class CreateEventControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonCreateEvent = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.oracleCommand1 = new Oracle.ManagedDataAccess.Client.OracleCommand();
            this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
            this.comboBoxRestrictions = new System.Windows.Forms.ComboBox();
            this.comboBoxCity = new System.Windows.Forms.ComboBox();
            this.dateTimePickerTime = new System.Windows.Forms.DateTimePicker();
            this.numericUpDownPrice = new System.Windows.Forms.NumericUpDown();
            this.comboBoxEventName = new System.Windows.Forms.ComboBox();
            this.comboBoxVenue = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkedListBoxCategories = new System.Windows.Forms.CheckedListBox();
            this.numericUpDownCapacity = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownStreetNum = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxStreetName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxSuburb = new System.Windows.Forms.TextBox();
            this.textBoxPostCode = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBoxCountry = new System.Windows.Forms.ComboBox();
            this.textBoxCompanyClub = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.buttonClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCapacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStreetNum)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(405, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "City";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxDescription.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDescription.Location = new System.Drawing.Point(129, 84);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(242, 49);
            this.textBoxDescription.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "Event Description";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(57, 312);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(49, 13);
            this.label12.TabIndex = 47;
            this.label12.Text = "Category";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(37, 242);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 13);
            this.label11.TabIndex = 46;
            this.label11.Text = "Event Start Time";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(57, 198);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 45;
            this.label8.Text = "Event Date";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(57, 38);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 13);
            this.label9.TabIndex = 44;
            this.label9.Text = "Event Name";
            // 
            // buttonCreateEvent
            // 
            this.buttonCreateEvent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.buttonCreateEvent.FlatAppearance.BorderSize = 0;
            this.buttonCreateEvent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCreateEvent.ForeColor = System.Drawing.Color.White;
            this.buttonCreateEvent.Location = new System.Drawing.Point(408, 440);
            this.buttonCreateEvent.Name = "buttonCreateEvent";
            this.buttonCreateEvent.Size = new System.Drawing.Size(218, 50);
            this.buttonCreateEvent.TabIndex = 17;
            this.buttonCreateEvent.Text = "CREATE EVENT";
            this.buttonCreateEvent.UseVisualStyleBackColor = false;
            this.buttonCreateEvent.Click += new System.EventHandler(this.buttonCreateEvent_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 386);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 51;
            this.label3.Text = "Event Entry Price";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(402, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 53;
            this.label4.Text = "Max Attendees";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 276);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 57;
            this.label6.Text = "Restrictions";
            // 
            // oracleCommand1
            // 
            this.oracleCommand1.RowsToFetchPerRoundTrip = ((long)(0));
            this.oracleCommand1.Transaction = null;
            // 
            // dateTimePickerDate
            // 
            this.dateTimePickerDate.Location = new System.Drawing.Point(129, 198);
            this.dateTimePickerDate.Name = "dateTimePickerDate";
            this.dateTimePickerDate.Size = new System.Drawing.Size(242, 20);
            this.dateTimePickerDate.TabIndex = 2;
            // 
            // comboBoxRestrictions
            // 
            this.comboBoxRestrictions.FormattingEnabled = true;
            this.comboBoxRestrictions.Location = new System.Drawing.Point(129, 276);
            this.comboBoxRestrictions.Name = "comboBoxRestrictions";
            this.comboBoxRestrictions.Size = new System.Drawing.Size(242, 21);
            this.comboBoxRestrictions.TabIndex = 4;
            // 
            // comboBoxCity
            // 
            this.comboBoxCity.FormattingEnabled = true;
            this.comboBoxCity.Location = new System.Drawing.Point(477, 212);
            this.comboBoxCity.Name = "comboBoxCity";
            this.comboBoxCity.Size = new System.Drawing.Size(242, 21);
            this.comboBoxCity.TabIndex = 13;
            // 
            // dateTimePickerTime
            // 
            this.dateTimePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerTime.Location = new System.Drawing.Point(129, 236);
            this.dateTimePickerTime.Name = "dateTimePickerTime";
            this.dateTimePickerTime.ShowUpDown = true;
            this.dateTimePickerTime.Size = new System.Drawing.Size(242, 20);
            this.dateTimePickerTime.TabIndex = 3;
            // 
            // numericUpDownPrice
            // 
            this.numericUpDownPrice.DecimalPlaces = 2;
            this.numericUpDownPrice.Location = new System.Drawing.Point(251, 384);
            this.numericUpDownPrice.Name = "numericUpDownPrice";
            this.numericUpDownPrice.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownPrice.TabIndex = 6;
            // 
            // comboBoxEventName
            // 
            this.comboBoxEventName.FormattingEnabled = true;
            this.comboBoxEventName.Location = new System.Drawing.Point(129, 38);
            this.comboBoxEventName.Name = "comboBoxEventName";
            this.comboBoxEventName.Size = new System.Drawing.Size(242, 21);
            this.comboBoxEventName.TabIndex = 0;
            this.comboBoxEventName.SelectedIndexChanged += new System.EventHandler(this.comboBoxEventName_SelectedIndexChanged);
            this.comboBoxEventName.TextChanged += new System.EventHandler(this.comboBoxEventName_TextChanged);
            // 
            // comboBoxVenue
            // 
            this.comboBoxVenue.FormattingEnabled = true;
            this.comboBoxVenue.Location = new System.Drawing.Point(477, 38);
            this.comboBoxVenue.Name = "comboBoxVenue";
            this.comboBoxVenue.Size = new System.Drawing.Size(242, 21);
            this.comboBoxVenue.TabIndex = 8;
            this.comboBoxVenue.SelectedIndexChanged += new System.EventHandler(this.comboBoxVenue_SelectedIndexChanged);
            this.comboBoxVenue.TextChanged += new System.EventHandler(this.comboBoxVenue_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(402, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 66;
            this.label5.Text = "Venue Name";
            // 
            // checkedListBoxCategories
            // 
            this.checkedListBoxCategories.FormattingEnabled = true;
            this.checkedListBoxCategories.Location = new System.Drawing.Point(129, 313);
            this.checkedListBoxCategories.Name = "checkedListBoxCategories";
            this.checkedListBoxCategories.Size = new System.Drawing.Size(242, 49);
            this.checkedListBoxCategories.TabIndex = 5;
            // 
            // numericUpDownCapacity
            // 
            this.numericUpDownCapacity.Location = new System.Drawing.Point(599, 77);
            this.numericUpDownCapacity.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownCapacity.Name = "numericUpDownCapacity";
            this.numericUpDownCapacity.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownCapacity.TabIndex = 9;
            // 
            // numericUpDownStreetNum
            // 
            this.numericUpDownStreetNum.Location = new System.Drawing.Point(599, 113);
            this.numericUpDownStreetNum.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownStreetNum.Name = "numericUpDownStreetNum";
            this.numericUpDownStreetNum.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownStreetNum.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(402, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 70;
            this.label7.Text = "Street Num";
            // 
            // textBoxStreetName
            // 
            this.textBoxStreetName.Location = new System.Drawing.Point(532, 145);
            this.textBoxStreetName.Name = "textBoxStreetName";
            this.textBoxStreetName.Size = new System.Drawing.Size(187, 20);
            this.textBoxStreetName.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(402, 153);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 72;
            this.label10.Text = "Street Name";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(405, 180);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 13);
            this.label13.TabIndex = 73;
            this.label13.Text = "Suburb";
            // 
            // textBoxSuburb
            // 
            this.textBoxSuburb.Location = new System.Drawing.Point(532, 180);
            this.textBoxSuburb.Name = "textBoxSuburb";
            this.textBoxSuburb.Size = new System.Drawing.Size(187, 20);
            this.textBoxSuburb.TabIndex = 12;
            // 
            // textBoxPostCode
            // 
            this.textBoxPostCode.Location = new System.Drawing.Point(532, 239);
            this.textBoxPostCode.MaxLength = 4;
            this.textBoxPostCode.Name = "textBoxPostCode";
            this.textBoxPostCode.Size = new System.Drawing.Size(187, 20);
            this.textBoxPostCode.TabIndex = 14;
            this.textBoxPostCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPostCode_KeyPress);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(405, 246);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 13);
            this.label14.TabIndex = 76;
            this.label14.Text = "PostCode";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(405, 279);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(43, 13);
            this.label15.TabIndex = 77;
            this.label15.Text = "Country";
            // 
            // comboBoxCountry
            // 
            this.comboBoxCountry.FormattingEnabled = true;
            this.comboBoxCountry.Items.AddRange(new object[] {
            "New Zealand"});
            this.comboBoxCountry.Location = new System.Drawing.Point(469, 279);
            this.comboBoxCountry.Name = "comboBoxCountry";
            this.comboBoxCountry.Size = new System.Drawing.Size(250, 21);
            this.comboBoxCountry.TabIndex = 15;
            // 
            // textBoxCompanyClub
            // 
            this.textBoxCompanyClub.Location = new System.Drawing.Point(129, 146);
            this.textBoxCompanyClub.Name = "textBoxCompanyClub";
            this.textBoxCompanyClub.Size = new System.Drawing.Size(242, 20);
            this.textBoxCompanyClub.TabIndex = 7;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(41, 152);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 13);
            this.label16.TabIndex = 80;
            this.label16.Text = "Comapny/Club";
            // 
            // buttonClear
            // 
            this.buttonClear.BackColor = System.Drawing.Color.White;
            this.buttonClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.buttonClear.Location = new System.Drawing.Point(153, 440);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(218, 50);
            this.buttonClear.TabIndex = 16;
            this.buttonClear.Text = "CLEAR";
            this.buttonClear.UseVisualStyleBackColor = false;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // CreateEventControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.textBoxCompanyClub);
            this.Controls.Add(this.comboBoxCountry);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.textBoxPostCode);
            this.Controls.Add(this.textBoxSuburb);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxStreetName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numericUpDownStreetNum);
            this.Controls.Add(this.numericUpDownCapacity);
            this.Controls.Add(this.checkedListBoxCategories);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxVenue);
            this.Controls.Add(this.comboBoxEventName);
            this.Controls.Add(this.numericUpDownPrice);
            this.Controls.Add(this.dateTimePickerTime);
            this.Controls.Add(this.comboBoxCity);
            this.Controls.Add(this.comboBoxRestrictions);
            this.Controls.Add(this.dateTimePickerDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.buttonCreateEvent);
            this.Name = "CreateEventControl";
            this.Size = new System.Drawing.Size(784, 629);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCapacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStreetNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonCreateEvent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private Oracle.ManagedDataAccess.Client.OracleCommand oracleCommand1;
        private System.Windows.Forms.DateTimePicker dateTimePickerDate;
        private System.Windows.Forms.ComboBox comboBoxRestrictions;
        private System.Windows.Forms.ComboBox comboBoxCity;
        private System.Windows.Forms.DateTimePicker dateTimePickerTime;
        private System.Windows.Forms.NumericUpDown numericUpDownPrice;
        private System.Windows.Forms.ComboBox comboBoxEventName;
        private System.Windows.Forms.ComboBox comboBoxVenue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckedListBox checkedListBoxCategories;
        private System.Windows.Forms.NumericUpDown numericUpDownCapacity;
        private System.Windows.Forms.NumericUpDown numericUpDownStreetNum;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxStreetName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxSuburb;
        private System.Windows.Forms.TextBox textBoxPostCode;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboBoxCountry;
        private System.Windows.Forms.TextBox textBoxCompanyClub;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button buttonClear;
    }
}
