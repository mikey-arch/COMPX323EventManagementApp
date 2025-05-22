using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using COMPX323EventManagementApp.Models;

namespace COMPX323EventManagementApp
{
    public partial class MongoCreateEventControl : UserControl
    {
        private List<string> validCities = new List<string> 
        { 
            "Auckland", "Wellington", "Christchurch", "Hamilton", "Tauranga", 
            "Dunedin", "Palmerston North", "Napier", "Hastings", "Nelson", 
            "Rotorua", "New Plymouth", "Whangarei", "Invercargill", "Whanganui", 
            "Gisborne", "Timaru", "Blenheim", "Queenstown", "Taupo" 
        };

         private List<string> validCategories = new List<string> 
         { 
            "Art", "Music", "Theatre", "Talk", "Science", "Culture", 
            "Food", "Sports", "Education", "Tech", "Health", "Travel", 
            "History", "Literature", "Fashion", "Business", "Finance", 
            "Law", "Politics", "Dating", "Family", "Animals", "Gaming", "Environment" 
         };

        private List<string> validRestrictions = new List<string> 
        { 
            "Adults(R18+)", "All Ages", "Teens(R13+)", "Seniors(65+)" 
        };

        public MongoCreateEventControl()
        {
            InitializeComponent();

            LoadCategories();
            LoadRestrictions();
            LoadCities();
            LoadEventName();
            LoadVenues();

            dateTimePickerDate.Value = DateTime.Today;
            dateTimePickerTime.Value = DateTime.Today.AddHours(17); 

            this.VisibleChanged += MongoCreateEventControl_VisibleChanged;
        }

        private void LoadCategories()
        {
            checkedListBoxCategories.Items.Clear();
            foreach (string c in validCategories)
            {
                checkedListBoxCategories.Items.Add(c);
            }
        }

        private void LoadRestrictions()
        {
            comboBoxRestrictions.Items.Clear();
            foreach (string r in validRestrictions)
            {
                comboBoxRestrictions.Items.Add(r);
            }
        }

        private void LoadCities()
        {
            comboBoxCity.Items.Clear();
            foreach (string c in validCities)
            {
                comboBoxCity.Items.Add(c);
            }
        }

        private void LoadEventName()
        {
            try
            {
                comboBoxEventName.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading event names: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadVenues()
        {
            try
            {
                comboBoxVenue.Items.Clear();
                List<string> venues = MongoDBDataAccess.GetVenues();
                foreach (string venue in venues)
                {
                    comboBoxVenue.Items.Add(venue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading venues: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void buttonCreateEvent_Click(object sender, EventArgs e)
        {

        }

        //clears all inputs, unchecks categoreis, resets dropdowns and refreshes event names list
        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

                //helper method that clears all inputs, unchecks categoreis, resets dropdowns and refreshes event names list
        private void ClearForm()
        {
            comboBoxEventName.Text = "";
            textBoxDescription.Text = "";
            comboBoxVenue.Text = "";
            textBoxStreetName.Text = "";
            textBoxSuburb.Text = "";
            textBoxPostCode.Text = "";

            dateTimePickerDate.Value = DateTime.Today;
            dateTimePickerTime.Value = DateTime.Now;

            numericUpDownCapacity.Value = numericUpDownCapacity.Minimum;
            numericUpDownStreetNum.Value = numericUpDownStreetNum.Minimum;
            numericUpDownPrice.Value = 0;

            comboBoxCity.SelectedIndex = -1;
            comboBoxRestrictions.SelectedIndex = -1;
            
            for (int i = 0; i < checkedListBoxCategories.Items.Count; i++)
            {
                checkedListBoxCategories.SetItemChecked(i, false);
            }

             // Enable all disabled controls if submitted with already before event and venue
            textBoxDescription.ReadOnly = false;
            numericUpDownCapacity.Enabled = true;
            numericUpDownStreetNum.Enabled = true;
            textBoxStreetName.ReadOnly = false;
            textBoxSuburb.ReadOnly = false;
            comboBoxCity.Enabled = true;
            textBoxPostCode.ReadOnly = false;
            checkedListBoxCategories.Enabled = true;
            comboBoxRestrictions.Enabled = true;
            textBoxDescription.BackColor = SystemColors.Window;
            textBoxStreetName.BackColor = SystemColors.Window;
            textBoxSuburb.BackColor = SystemColors.Window;
            textBoxPostCode.BackColor = SystemColors.Window;
            

            LoadEventName();
        }

                //error checking for if an input is left empty , then notify user 
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(comboBoxEventName.Text))
            {
                MessageBox.Show("Please enter an event name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            if (checkedListBoxCategories.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(comboBoxVenue.Text))
            {
                MessageBox.Show("Please enter a venue name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            if (!validCities.Contains(comboBoxCity.Text))
            {
                MessageBox.Show("Please select a valid New Zealand city.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(textBoxStreetName.Text))
            {
                MessageBox.Show("Please enter a street name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(textBoxPostCode.Text))
            {
                MessageBox.Show("Please enter a postcode.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            if (!System.Text.RegularExpressions.Regex.IsMatch(textBoxPostCode.Text, "^[0-9]{4,5}$"))
            {
                MessageBox.Show("Postcode must be 4-5 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            if (dateTimePickerDate.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Event date cannot be in the past.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (numericUpDownCapacity.Value == 0)
            {
                MessageBox.Show("Venue capacity must be greater than 0. Please enter a valid capacity.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numericUpDownCapacity.Focus();
                return false;
            }
    
            if (numericUpDownStreetNum.Value == 0)
            {
                MessageBox.Show("Street number must be greater than 0. Please enter a valid street number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numericUpDownStreetNum.Focus();
                return false;
            }
            
            return true;
        }

        private void MongoCreateEventControl_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
            {
                LoadEventName();
                LoadVenues();
            }

        }

    }
}
