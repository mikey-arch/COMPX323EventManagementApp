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
using MongoDB.Bson;
using MongoDB.Driver;

namespace COMPX323EventManagementApp
{
    public partial class MongoCreateEventControl : UserControl
    {
        /// <summary>
        /// User control for creating events in MongoDB.
        /// </summary>
        private List<string> validCities = new List<string> 
        { 
            "Auckland", "Wellington", "Christchurch", "Hamilton", "Tauranga", 
            "Dunedin", "Palmerston North", "Napier", "Hastings", "Nelson", 
            "Rotorua", "New Plymouth", "Whangarei", "Invercargill", "Whanganui", 
            "Gisborne", "Timaru", "Blenheim", "Queenstown", "Taupo" 
        };

        /// <summary>
        /// List of valid cities in New Zealand.
        /// </summary>
        private List<string> validCategories = new List<string> 
         { 
            "Art", "Music", "Theatre", "Talk", "Science", "Culture", 
            "Food", "Sports", "Education", "Tech", "Health", "Travel", 
            "History", "Literature", "Fashion", "Business", "Finance", 
            "Law", "Politics", "Dating", "Family", "Animals", "Gaming", "Environment" 
         };

        /// <summary>
        /// List of valid event restrictions.
        /// </summary>
        private List<string> validRestrictions = new List<string> 
        { 
            "Adults(R18+)", "All Ages", "Teens(R13+)", "Seniors(65+)" 
        };

        /// <summary>
        /// Initializes the user control and loads dropdown data.
        /// </summary>
        public MongoCreateEventControl()
        {
            InitializeComponent();

            // Load predefined dropdown values and comboBox
            LoadCategories();
            LoadRestrictions();
            LoadCities();
            LoadEventName();
            LoadVenues();
            comboBoxCountry.Text = "New Zealand";

            // Set default date/time for event (today @ 5PM)
            dateTimePickerDate.Value = DateTime.Today;
            dateTimePickerTime.Value = DateTime.Today.AddHours(17);

            // Reload values when control becomes visible again
            this.VisibleChanged += MongoCreateEventControl_VisibleChanged;
        }

        /// <summary>
        /// Populates the category checklist with valid event categories.
        /// </summary>
        private void LoadCategories()
        {
            checkedListBoxCategories.Items.Clear();
            foreach (string c in validCategories)
            {
                checkedListBoxCategories.Items.Add(c);
            }
        }

        /// <summary>
        /// Populates the restriction combo box with valid restrictions.
        /// </summary>
        private void LoadRestrictions()
        {
            comboBoxRestrictions.Items.Clear();
            foreach (string r in validRestrictions)
            {
                comboBoxRestrictions.Items.Add(r);
            }
        }

        /// <summary>
        /// Loads a list of valid NZ cities into the city combo box.
        /// </summary>
        private void LoadCities()
        {
            comboBoxCity.Items.Clear();
            foreach (string c in validCities)
            {
                comboBoxCity.Items.Add(c);
            }
        }

        /// <summary>
        /// Loads event names created by the currently logged-in user.
        /// </summary>
        private void LoadEventName()
        {
            try
            {
                comboBoxEventName.Items.Clear();
                if (Session.CurrentUser != null)
                {
                    List<string> events = MongoDBDataAccess.GetEventsByCreator(Session.CurrentUser.Id);
                    foreach (string eventName in events)
                    {
                        comboBoxEventName.Items.Add(eventName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading event names: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads available venues from MongoDB.
        /// </summary>
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


        /// <summary>
        /// Creates a new event or adds an instance to an existing one in MongoDB.
        /// </summary>
        private void buttonCreateEvent_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {

                string eventName = comboBoxEventName.Text.Trim();
                bool eventExists = MongoDBDataAccess.EventNameExists(eventName);

                // Create MongoDB event document
                var mongoEvent = new BsonDocument
                {
                    {"ename", comboBoxEventName.Text.Trim()},
                    {"description", textBoxDescription.Text.Trim()},
                    {"creationDate", DateTime.Now},
                    {"creatorNum", Session.CurrentUser.Id},
                    {"creator", new BsonDocument
                        {
                            {"fname", Session.CurrentUser.Fname},
                            {"lname", Session.CurrentUser.Lname},
                            {"email", Session.CurrentUser.Email}
                        }
                    },
                    {"restriction", new BsonDocument
                        {
                            {"name", comboBoxRestrictions.SelectedItem?.ToString() ?? "All Ages"},
                            {"description", "Event restriction"}
                        }
                    }
                };

                // Add selected categories
                var categoriesArray = new BsonArray();
                foreach (object item in checkedListBoxCategories.CheckedItems)
                {
                    categoriesArray.Add(new BsonDocument
                    {
                        {"name", item.ToString()},
                        {"description", $"{item} category"}
                    });
                }
                mongoEvent["categories"] = categoriesArray;

                // Prepare event date and time
                DateTime eventDate = dateTimePickerDate.Value.Date;
                DateTime eventTime = dateTimePickerTime.Value;
                DateTime combinedDateTime = eventDate.Add(eventTime.TimeOfDay);

                // Create event instance with venue
                var eventInstance = new BsonDocument
                {
                    {"eventDate", eventDate},
                    {"time", combinedDateTime},
                    {"price", numericUpDownPrice.Value},
                    {"venue", new BsonDocument
                        {
                            {"vname", comboBoxVenue.Text.Trim()},
                            {"city", comboBoxCity.Text},
                            {"capacity", (int)numericUpDownCapacity.Value},
                            {"streetNum", (int)numericUpDownStreetNum.Value},
                            {"streetName", textBoxStreetName.Text.Trim()},
                            {"suburb", textBoxSuburb.Text.Trim()},
                            {"postcode", textBoxPostCode.Text.Trim()},
                            {"country", "New Zealand"}
                        }
                    }
                };

                var instancesArray = new BsonArray { eventInstance };
                mongoEvent["instances"] = instancesArray;

                // Push to DB
                bool success = MongoDBDataAccess.CreateEvent(mongoEvent);

                if (success)
                {
                    if (eventExists)
                    {
                        MessageBox.Show("New event instance added successfully to existing event!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("New event created successfully in MongoDB!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    ClearForm();
                }
                else
                {
                    if (eventExists)
                    {
                        MessageBox.Show("Could not add event instance. An instance for this date and venue may already exist.", "Creation Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Could not create event. Please try again.", "Creation Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating event: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Clears all inputs on the form and resets the UI.
        /// </summary>
        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        /// <summary>
        /// Helper to reset all form fields to default state.
        /// </summary>
        private void ClearForm()
        {
            // Clear text fields
            comboBoxEventName.Text = "";
            textBoxDescription.Text = "";
            comboBoxVenue.Text = "";
            textBoxStreetName.Text = "";
            textBoxSuburb.Text = "";
            textBoxPostCode.Text = "";
            comboBoxCountry.Text = "New Zealand";

            // Reset dropdowns and pickers
            dateTimePickerDate.Value = DateTime.Today;
            dateTimePickerTime.Value = DateTime.Now;

            // Reset numeric values
            numericUpDownCapacity.Value = numericUpDownCapacity.Minimum;
            numericUpDownStreetNum.Value = numericUpDownStreetNum.Minimum;
            numericUpDownPrice.Value = 0;

            // Uncheck all categories
            comboBoxCity.SelectedIndex = -1;
            comboBoxRestrictions.SelectedIndex = -1;

            // Re-enable UI fields
            for (int i = 0; i < checkedListBoxCategories.Items.Count; i++)
            {
                checkedListBoxCategories.SetItemChecked(i, false);
            }

            // Re-enable UI fields
            textBoxDescription.ReadOnly = false;
            numericUpDownCapacity.Enabled = true;
            numericUpDownStreetNum.Enabled = true;
            textBoxStreetName.ReadOnly = false;
            textBoxSuburb.ReadOnly = false;
            comboBoxCity.Enabled = true;
            textBoxPostCode.ReadOnly = false;
            checkedListBoxCategories.Enabled = true;
            comboBoxRestrictions.Enabled = true;
            comboBoxCountry.Enabled = true;
            textBoxDescription.BackColor = SystemColors.Window;
            textBoxStreetName.BackColor = SystemColors.Window;
            textBoxSuburb.BackColor = SystemColors.Window;
            textBoxPostCode.BackColor = SystemColors.Window;

            // Reload user events
            LoadEventName();
        }

        /// <summary>
        /// Validates all input fields on the form before allowing event creation.
        /// Displays specific messages for any invalid input.
        /// </summary>
        /// <returns>True if all inputs are valid; false otherwise.</returns>
        private bool ValidateInputs()
        {
            if (comboBoxRestrictions.SelectedItem == null || comboBoxRestrictions.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a restriction.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
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

        // Refreshes dropdowns when control becomes visible
        private void MongoCreateEventControl_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
            {
                LoadEventName();
                LoadVenues();
            }

        }

        /// <summary>
        /// Triggered when the event name text box changes.
        /// If the event name exists in MongoDB, populates form fields with its data and disables editing.
        /// If it doesn't exist, resets the fields for new input.
        /// </summary>
        private void comboBoxEventName_TextChanged(object sender, EventArgs e)
        {
            string eventName = comboBoxEventName.Text.Trim();

            // Check if the event name exists in MongoDB
            bool eventExists = !string.IsNullOrEmpty(eventName) && MongoDBDataAccess.EventNameExists(eventName);
            
            if (eventExists)
            {
                try
                {
                    // Get event details from MongoDB
                    var eventDetails = MongoDBDataAccess.GetEventDetails(eventName);
                    
                    if(eventDetails.HasValue)
                    {
                        var eventData = eventDetails.Value;
                        // Populate fields with event data
                        textBoxDescription.Text = eventData.description;
                        
                        // Make these fields read-only and change appearance
                        textBoxDescription.ReadOnly = true;
                        textBoxDescription.BackColor = SystemColors.Control;
                        
                        // Load and set categories
                        var categories = eventData.categories;
                        
                        // Reset categories first
                        for (int i = 0; i < checkedListBoxCategories.Items.Count; i++)
                        {
                            checkedListBoxCategories.SetItemChecked(i, false);
                        }
                        
                        // Check appropriate categories
                        foreach (var category in categories)
                        {
                            int index = checkedListBoxCategories.Items.IndexOf(category);
                            if (index >= 0)
                            {
                                checkedListBoxCategories.SetItemChecked(index, true);
                            }
                        }
                        
                        // Disable categories control
                        checkedListBoxCategories.Enabled = false;
                        
                        // Set restriction
                        if (!string.IsNullOrEmpty(eventData.restriction))
                        {
                            comboBoxRestrictions.SelectedItem = eventData.restriction;
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
                textBoxDescription.BackColor = SystemColors.Window;
                checkedListBoxCategories.Enabled = true;
                comboBoxRestrictions.Enabled = true;
                
                // Only clear if the field had content before
                if (!string.IsNullOrEmpty(textBoxDescription.Text))
                {
                    textBoxDescription.Text = "";
                    
                    // Clear category selections
                    for (int i = 0; i < checkedListBoxCategories.Items.Count; i++)
                    {
                        checkedListBoxCategories.SetItemChecked(i, false);
                    }
                    
                    comboBoxRestrictions.SelectedIndex = -1;
                }
            }

        }

        /// <summary>
        /// Triggered when the text of the venue combo box changes.
        /// Loads existing venue data if the venue exists in MongoDB and disables editing.
        /// Otherwise, resets the input fields and enables user input for a new venue.
        /// </summary>
        private void comboBoxVenue_TextChanged(object sender, EventArgs e)
        {
            string venueName = comboBoxVenue.Text.Trim();

            // Check if the venue exists in MongoDB
            bool venueExists = !string.IsNullOrEmpty(venueName) && MongoDBDataAccess.VenueExists(venueName);
            
            if (venueExists)
            {
                try
                {
                    var venueDetails = MongoDBDataAccess.GetVenueDetails(venueName);
                    if (venueDetails.HasValue)
                    {
                        var venueData = venueDetails.Value;
                        // Populate venue fields
                        numericUpDownCapacity.Value = venueData.capacity;
                        numericUpDownStreetNum.Value = venueData.streetNum;
                        textBoxStreetName.Text = venueData.streetName;
                        textBoxSuburb.Text = venueData.suburb;
                        comboBoxCity.Text = venueData.city;
                        textBoxPostCode.Text = venueData.postcode;
                        comboBoxCountry.Text = venueData.country;
                        
                        // Disable or grey out venue fields
                        numericUpDownCapacity.Enabled = false;
                        numericUpDownStreetNum.Enabled = false;
                        textBoxStreetName.ReadOnly = true;
                        textBoxSuburb.ReadOnly = true;
                        comboBoxCity.Enabled = false;
                        textBoxPostCode.ReadOnly = true;
                        comboBoxCountry.Enabled = false;
                        
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
                // Reset if fields were previously populated
                if (!string.IsNullOrEmpty(textBoxStreetName.Text))
                {
                    // Reset venue fields
                    numericUpDownCapacity.Value = numericUpDownCapacity.Minimum;
                    numericUpDownStreetNum.Value = numericUpDownStreetNum.Minimum;
                    textBoxStreetName.Text = "";
                    textBoxSuburb.Text = "";
                    comboBoxCity.SelectedIndex = -1;
                    textBoxPostCode.Text = "";
                    comboBoxCountry.Text = "New Zealand";
                }
                
                // Enable venue fields for new venue and reset background colors
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

        /// <summary>
        /// Triggered when the selected event name changes.
        /// Loads existing event details if the event exists and locks certain fields accordingly.
        /// </summary>
        private void comboBoxEventName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedEvent = comboBoxEventName.SelectedItem?.ToString();
            bool isExistingEvent = !string.IsNullOrEmpty(selectedEvent);
            
            if (isExistingEvent)
            {
                try
                {
                    // Get event details from MongoDB
                    var eventDetails = MongoDBDataAccess.GetEventDetails(selectedEvent);

                    if(eventDetails.HasValue)
                    {
                        var eventData = eventDetails.Value;

                        textBoxDescription.Text = eventData.description;

                        // Disable event fields
                        textBoxDescription.ReadOnly = true;
                        textBoxDescription.BackColor = SystemColors.Control;

                        // Set categories
                        for (int i = 0; i < checkedListBoxCategories.Items.Count; i++)
                        {
                            checkedListBoxCategories.SetItemChecked(i, false);
                        }

                        foreach (var category in eventData.categories)
                        {
                            int index = checkedListBoxCategories.Items.IndexOf(category);
                            if (index >= 0)
                            {
                                checkedListBoxCategories.SetItemChecked(index, true);
                            }
                        }

                        // Set restriction
                        if (!string.IsNullOrEmpty(eventData.restriction))
                        {
                            comboBoxRestrictions.SelectedItem = eventData.restriction;
                        }
                        
                        checkedListBoxCategories.Enabled = false;
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
                // Enable fields
                textBoxDescription.ReadOnly = false;
                textBoxDescription.BackColor = SystemColors.Window;
                checkedListBoxCategories.Enabled = true;
                comboBoxRestrictions.Enabled = true;
            }

        }

        /// <summary>
        /// Triggered when the selected venue changes.
        /// Loads existing venue details if the venue exists and disables editing of those fields.
        /// </summary>
        private void comboBoxVenue_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedVenue = comboBoxVenue.SelectedItem?.ToString();
    
            if (!string.IsNullOrEmpty(selectedVenue))
            {
                try
                {
                    // Get venue details from MongoDB
                    var venueDetails = MongoDBDataAccess.GetVenueDetails(selectedVenue);
                    if (venueDetails.HasValue)
                    {
                        var venueData = venueDetails.Value;
                        numericUpDownCapacity.Value = venueData.capacity;
                        numericUpDownStreetNum.Value = venueData.streetNum;
                        textBoxStreetName.Text = venueData.streetName;
                        textBoxSuburb.Text = venueData.suburb;
                        comboBoxCity.Text = venueData.city;
                        textBoxPostCode.Text = venueData.postcode;

                        // Disable venue fields
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
    }
}
