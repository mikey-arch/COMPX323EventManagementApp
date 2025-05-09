using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Windows.Forms;
using COMPX323EventManagementApp.Models;
using Oracle.ManagedDataAccess.Client;

namespace COMPX323EventManagementApp
{
    public partial class CreateEventControl : UserControl
    {
        private List<string> validCategories = new List<string> 
        { 
            "Art", "Music", "Theatre", "Talk", "Science", "Culture", 
            "Food", "Sports", "Education", "Tech", "Health", "Travel", 
            "History", "Literature", "Fashion", "Business", "Finance", 
            "Law", "Politics", "Dating", "Family", "Animals", "Gaming", "Environment" 
        };
        
        private List<string> validCities = new List<string> 
        { 
            "Auckland", "Wellington", "Christchurch", "Hamilton", "Tauranga", 
            "Dunedin", "Palmerston North", "Napier", "Hastings", "Nelson", 
            "Rotorua", "New Plymouth", "Whangarei", "Invercargill", "Whanganui", 
            "Gisborne", "Timaru", "Blenheim", "Queenstown", "Taupo" 
        };
        
        private List<string> validRestrictions = new List<string> 
        { 
            "Adults(R18+)", "All Ages", "Teens(R13+)", "Seniors(65+)" 
        };
        
        public CreateEventControl()
        {
            InitializeComponent();
            LoadEventName();
            LoadVenues();

            LoadCategories();
            LoadRestrictions();
            LoadCities();

            dateTimePickerDate.Value = DateTime.Today;
            dateTimePickerTime.Value = DateTime.Today.AddHours(17); 
        }

        //populate city box with valid cities
        private void LoadCities()
        {
            comboBoxCity.Items.Clear();
            foreach (string c in validCities)
            {
                comboBoxCity.Items.Add(c);
            }
        }

        //load venue with current venues 
        private void LoadVenues()
        {
            try
            {
                comboBoxVenue.Items.Clear();
                List<string> venues = DataAccess.GetAllVenues();
                foreach (string v in  venues)
                {
                    comboBoxVenue.Items.Add(v);
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading venues: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //populate restrictions combobox with valid options
        private void LoadRestrictions()
        {
            comboBoxRestrictions.Items.Clear();
            foreach (string r in validRestrictions)
            {
                comboBoxRestrictions.Items.Add(r);
            }
        }

        //populates listbox with only valid options
        private void LoadCategories()
        {
            checkedListBoxCategories.Items.Clear();
            foreach (string c in validCategories)
            {
                checkedListBoxCategories.Items.Add(c);
            }
        }

        //load any previous events created by user into the combobox 
        private void LoadEventName()
        {
            try
            {
                comboBoxEventName.Items.Clear();
                List<string> events = DataAccess.GetOrganiserEvents(Session.CurrentUser.Id);
                foreach (string e in events)
                {
                    comboBoxEventName.Items.Add(e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading event names: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            
            return true;
        }

        private void buttonCreateEvent_Click(object sender, EventArgs e)
        {

            if (!ValidateInputs())
            {
                return;
            }
            
            try
            {
                //create event
                Event eventObj = new Event
                {
                    Ename = comboBoxEventName.Text.Trim(),
                    Description = textBoxDescription.Text.Trim(),
                    Restriction = comboBoxRestrictions.SelectedItem?.ToString(),
                    CreatorNum = Session.CurrentUser.Id
                };

                //create venue
                Venue venue = new Venue
                {
                    Vname = comboBoxVenue.Text.Trim(),
                    Capacity = (int)numericUpDownCapacity.Value,
                    StreetNum = (int)numericUpDownStreetNum.Value,
                    StreetName = textBoxStreetName.Text,
                    Suburb = textBoxSuburb.Text,
                    City = comboBoxCity.Text,
                    Postcode = textBoxPostCode.Text,
                    Country = "New Zealand" 
                };

                //create eventinstance model
                DateTime eventDate = dateTimePickerDate.Value.Date;
                DateTime eventTime = dateTimePickerTime.Value;
                DateTime combinedDateTime = eventDate.Add(eventTime.TimeOfDay);

                EventInstance instance = new EventInstance
                {
                    EventDate = eventDate,
                    Vname = venue.Vname,
                    Ename = eventObj.Ename,
                    Price = numericUpDownPrice.Value,
                    Time = combinedDateTime,
                };

                //get selected categories
                List<string> categories = new List<string>();
                foreach (object item in checkedListBoxCategories.CheckedItems)
                {
                    categories.Add(item.ToString());
                }
                
                //get selected restriction
                string restriction = comboBoxRestrictions.SelectedItem?.ToString();
                
                //create event
                bool success = DataAccess.CreateOrUpdateEvent(
                    eventObj, instance, venue, categories, Session.CurrentUser.Id);
                
                if (success)
                {
                    MessageBox.Show("Event created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Could not create event. The event name is already taken or an instance already exists.", 
                        "Creation Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating event: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //if an existing event is selected from the user then load its details 
        private void comboBoxEventName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedEvent = comboBoxEventName.SelectedItem?.ToString();
            bool isExistingEvent = !string.IsNullOrEmpty(selectedEvent); //for enabling or disabling input fields if an existing even is selected
            
            if (isExistingEvent)
            {
                try
                {
                    //get event details
                    Event eventObject = DataAccess.GetEvent(selectedEvent);

                    if(eventObject != null)
                    {
                        textBoxDescription.Text = eventObject.Description;

                        //disable event fields
                        textBoxDescription.ReadOnly = true;
                        textBoxCompanyClub.ReadOnly = true;
                        textBoxDescription.BackColor = SystemColors.Control;
                        textBoxCompanyClub.BackColor = SystemColors.Control;

                        //get and display event cvategories, uncheccking and checking the correct categories
                        List<string> categories = DataAccess.GetEventCategories(selectedEvent);
                        for (int i = 0; i < checkedListBoxCategories.Items.Count; i++)
                        {
                            checkedListBoxCategories.SetItemChecked(i, false);
                        }

                        foreach (string c in categories)
                        {
                            int index = checkedListBoxCategories.Items.IndexOf(c);
                            if (index >= 0)
                            {
                                checkedListBoxCategories.SetItemChecked(index, true);
                            }
                        }

                        //get and display event restriction 
                        if (!string.IsNullOrEmpty(eventObject.Restriction))
                        {
                            comboBoxRestrictions.SelectedItem = eventObject.Restriction;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading event details: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //enable fields
                textBoxDescription.ReadOnly = false;
                textBoxCompanyClub.ReadOnly = false;
                textBoxDescription.BackColor= SystemColors.Window;
                textBoxDescription.BackColor = SystemColors.Window;
            }
        }

        //if an existing venue is selected from the user then load its details 
        private void comboBoxVenue_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedVenue = comboBoxVenue.SelectedItem?.ToString();
            
            if (!string.IsNullOrEmpty(selectedVenue))
            {
                try
                {
                    //get and display venue details
                    Venue venue = DataAccess.GetVenue(selectedVenue);
                    if (venue != null)
                    {
                        numericUpDownCapacity.Value = venue.Capacity;
                        numericUpDownStreetNum.Value = venue.StreetNum;
                        textBoxStreetName.Text = venue.StreetName;
                        textBoxSuburb.Text = venue.Suburb;
                        comboBoxCity.Text = venue.City;
                        textBoxPostCode.Text = venue.Postcode;

                        // Disable or grey out venue fields
                        numericUpDownCapacity.Enabled = false;
                        numericUpDownStreetNum.Enabled = false;
                        textBoxStreetName.ReadOnly = true;
                        textBoxSuburb.ReadOnly = true;
                        comboBoxCity.Enabled = false;
                        textBoxPostCode.ReadOnly = true;
                        
                        // Change background color to indicate fields are locked
                        textBoxStreetName.BackColor = SystemColors.Control;
                        textBoxSuburb.BackColor = SystemColors.Control;
                        textBoxPostCode.BackColor = SystemColors.Control;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading venue details: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Enable venue fields for new venue
                numericUpDownCapacity.Enabled = true;
                numericUpDownStreetNum.Enabled = true;
                textBoxStreetName.ReadOnly = false;
                textBoxSuburb.ReadOnly = false;
                comboBoxCity.Enabled = true;
                textBoxPostCode.ReadOnly = false;
                
                // Reset background colors
                textBoxStreetName.BackColor = SystemColors.Window;
                textBoxSuburb.BackColor = SystemColors.Window;
                textBoxPostCode.BackColor = SystemColors.Window;
            }

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
            textBoxCompanyClub.Text = "";
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
            textBoxCompanyClub.ReadOnly = false;
            numericUpDownCapacity.Enabled = true;
            numericUpDownStreetNum.Enabled = true;
            textBoxStreetName.ReadOnly = false;
            textBoxSuburb.ReadOnly = false;
            comboBoxCity.Enabled = true;
            textBoxPostCode.ReadOnly = false;
            checkedListBoxCategories.Enabled = true;
            comboBoxRestrictions.Enabled = true;
            textBoxDescription.BackColor = SystemColors.Window;
            textBoxCompanyClub.BackColor = SystemColors.Window;
            textBoxStreetName.BackColor = SystemColors.Window;
            textBoxSuburb.BackColor = SystemColors.Window;
            textBoxPostCode.BackColor = SystemColors.Window;
            

            LoadEventName();
        }

        //to allow only digits and backspacve to work in postcode input
        private void textBoxPostCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //handles chanigng text in eventname combobox
        private void comboBoxEventName_TextChanged(object sender, EventArgs e)
        {
            string eventName = comboBoxEventName.Text.Trim();
    
            // Check if the event name exists
            bool eventExists = !string.IsNullOrEmpty(eventName) && DataAccess.EventNameTaken(eventName);
            
            if (eventExists)
            {
                try
                {
                    // Get event details
                    Event eventObject = DataAccess.GetEvent(eventName);
                    
                    if(eventObject != null)
                    {
                        // Populate fields with event data
                        textBoxDescription.Text = eventObject.Description;
                        
                        // Make these fields read-only or change their appearance
                        textBoxDescription.ReadOnly = true;
                        textBoxCompanyClub.ReadOnly = true;
                        textBoxDescription.BackColor = SystemColors.Control;
                        textBoxCompanyClub.BackColor = SystemColors.Control;
                        
                        // Load categories and restrictions
                        List<string> categories = DataAccess.GetEventCategories(eventName);
                        
                        // Reset categories first
                        for (int i = 0; i < checkedListBoxCategories.Items.Count; i++)
                        {
                            checkedListBoxCategories.SetItemChecked(i, false);
                        }
                        
                        // Check appropriate categories
                        foreach (string c in categories)
                        {
                            int index = checkedListBoxCategories.Items.IndexOf(c);
                            if (index >= 0)
                            {
                                checkedListBoxCategories.SetItemChecked(index, true);
                            }
                        }
                        
                        // Disable categories control
                        checkedListBoxCategories.Enabled = false;
                        
                        // Get and display restriction
                        if (!string.IsNullOrEmpty(eventObject.Restriction))
                        {
                            comboBoxRestrictions.SelectedItem = eventObject.Restriction;
                        }
                        comboBoxRestrictions.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading event details: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Reset and enable fields for a new event
                textBoxDescription.ReadOnly = false;
                textBoxCompanyClub.ReadOnly = false;
                textBoxDescription.BackColor = SystemColors.Window;
                textBoxCompanyClub.BackColor = SystemColors.Window;
                checkedListBoxCategories.Enabled = true;
                comboBoxRestrictions.Enabled = true;
                
                // Only clear if the field had content before
                if (!string.IsNullOrEmpty(textBoxDescription.Text))
                {
                    textBoxDescription.Text = "";
                    textBoxCompanyClub.Text = "";
                    
                    // Clear category selections
                    for (int i = 0; i < checkedListBoxCategories.Items.Count; i++)
                    {
                        checkedListBoxCategories.SetItemChecked(i, false);
                    }
                    
                    comboBoxRestrictions.SelectedIndex = -1;
                }
            }
        }

        //handles changing selected venue in combobox
        private void comboBoxVenue_TextChanged(object sender, EventArgs e)
        {
            string venueName = comboBoxVenue.Text.Trim();
    
            // Check if the venue exists
            bool venueExists = !string.IsNullOrEmpty(venueName) && DataAccess.VenueExists(venueName);
            
            if (venueExists)
            {
                try
                {
                    Venue venue = DataAccess.GetVenue(venueName);
                    if (venue != null)
                    {
                        // Populate venue fields
                        numericUpDownCapacity.Value = venue.Capacity;
                        numericUpDownStreetNum.Value = venue.StreetNum;
                        textBoxStreetName.Text = venue.StreetName;
                        textBoxSuburb.Text = venue.Suburb;
                        comboBoxCity.Text = venue.City;
                        textBoxPostCode.Text = venue.Postcode;
                        
                        // Disable or grey out venue fields
                        numericUpDownCapacity.Enabled = false;
                        numericUpDownStreetNum.Enabled = false;
                        textBoxStreetName.ReadOnly = true;
                        textBoxSuburb.ReadOnly = true;
                        comboBoxCity.Enabled = false;
                        textBoxPostCode.ReadOnly = true;
                        
                        // Change background color to indicate fields are locked
                        textBoxStreetName.BackColor = SystemColors.Control;
                        textBoxSuburb.BackColor = SystemColors.Control;
                        textBoxPostCode.BackColor = SystemColors.Control;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading venue details: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // reset if fields were previously populated
                if (!string.IsNullOrEmpty(textBoxStreetName.Text))
                {
                    // Reset venue fields
                    numericUpDownCapacity.Value = numericUpDownCapacity.Minimum;
                    numericUpDownStreetNum.Value = numericUpDownStreetNum.Minimum;
                    textBoxStreetName.Text = "";
                    textBoxSuburb.Text = "";
                    comboBoxCity.SelectedIndex = -1;
                    textBoxPostCode.Text = "";
                }
                
                // Enable venue fields for new venue and reset background colours
                numericUpDownCapacity.Enabled = true;
                numericUpDownStreetNum.Enabled = true;
                textBoxStreetName.ReadOnly = false;
                textBoxSuburb.ReadOnly = false;
                comboBoxCity.Enabled = true;
                textBoxPostCode.ReadOnly = false;
                textBoxStreetName.BackColor = SystemColors.Window;
                textBoxSuburb.BackColor = SystemColors.Window;
                textBoxPostCode.BackColor = SystemColors.Window;
            }
        }
    }
}
