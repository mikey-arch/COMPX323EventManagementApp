using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using COMPX323EventManagementApp.Models;
using Oracle.ManagedDataAccess.Client;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

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
            LoadCategories();
            LoadRestrictions();
            LoadVenues();
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
                
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select vname from Venue";
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxVenue.Items.Add(reader["vname"].ToString());
                            }
                        }
                    }
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

                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        //get only name of events which the current user has made
                        cmd.CommandText = @"select e.ename from Event e join Organises o on o.ename = e.ename where o.acc_num = :userId";

                        cmd.Parameters.Add("userId", OracleDbType.Int32).Value = Session.CurrentUser.Id;

                        using (var reader = cmd.ExecuteReader()) 
                        {
                            while (reader.Read())
                            {
                                comboBoxEventName.Items.Add(reader["ename"].ToString());
                            }
                        }
                    }
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
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    
                    // Begin transaction
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string eventName = comboBoxEventName.Text.Trim();
                            string venueName = comboBoxVenue.Text.Trim();
                            
                            // Check if venue exists, create if not
                            bool venueExists = false;
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "SELECT COUNT(*) FROM Venue WHERE vname = :venueName";
                                cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                                
                                venueExists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                            }
                            
                            // If venue doesn't exist, create it
                            if (!venueExists)
                            {
                                using (var cmd = conn.CreateCommand())
                                {
                                    cmd.CommandText = @"INSERT INTO Venue (vname, capacity, street_num, street_name, 
                                                      suburb, city, postcode, country) 
                                                      VALUES (:venueName, :capacity, :streetNum, :streetName, 
                                                      :suburb, :city, :postcode, :country)";
                                                      
                                    cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                                    cmd.Parameters.Add("capacity", OracleDbType.Int32).Value = (int)numericUpDownCapacity.Value;
                                    cmd.Parameters.Add("streetNum", OracleDbType.Int32).Value = (int)numericUpDownStreetNum.Value;
                                    cmd.Parameters.Add("streetName", OracleDbType.Varchar2).Value = textBoxStreetName.Text;
                                    cmd.Parameters.Add("suburb", OracleDbType.Varchar2).Value = textBoxSuburb.Text;
                                    cmd.Parameters.Add("city", OracleDbType.Varchar2).Value = comboBoxCity.Text;
                                    cmd.Parameters.Add("postcode", OracleDbType.Varchar2).Value = textBoxPostCode.Text;
                                    
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            
                            // Check if event exists for this organizer
                            bool eventExists = false;
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = @"SELECT COUNT(*) FROM Event e 
                                                   JOIN Organises o ON e.ename = o.ename 
                                                   WHERE e.ename = :eventName AND o.acc_num = :accNum";
                                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                                cmd.Parameters.Add("accNum", OracleDbType.Int32).Value = Session.CurrentUser.Id;
                                
                                eventExists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                            }
                            
                            // If event doesn't exist, create it and relationships
                            if (!eventExists)
                            {
                                // Check if any other organizer has this event name
                                bool eventNameTaken = false;
                                using (var cmd = conn.CreateCommand())
                                {
                                    cmd.CommandText = "SELECT COUNT(*) FROM Event WHERE ename = :eventName";
                                    cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                                    
                                    eventNameTaken = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                                }
                                
                                if (eventNameTaken)
                                {
                                    MessageBox.Show("This event name is already taken by another organizer.", 
                                        "Event Name Taken", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                
                                // Insert into Event table
                                using (var cmd = conn.CreateCommand())
                                {
                                    cmd.CommandText = @"INSERT INTO Event (ename, description, creation_date, company_club) 
                                                      VALUES (:eventName, :description, DEFAULT, :companyClub)";
                                                      
                                    cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                                    cmd.Parameters.Add("description", OracleDbType.Clob).Value = textBoxDescription.Text;
                                    cmd.Parameters.Add("companyClub", OracleDbType.Varchar2).Value = textBoxCompanyClub.Text;
                                    
                                    cmd.ExecuteNonQuery();
                                }
                                
                                // Insert into Organises table
                                using (var cmd = conn.CreateCommand())
                                {
                                    cmd.CommandText = "INSERT INTO Organises (acc_num, ename) VALUES (:accNum, :eventName)";
                                    cmd.Parameters.Add("accNum", OracleDbType.Int32).Value = Session.CurrentUser.Id;
                                    cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                                    
                                    cmd.ExecuteNonQuery();
                                }
                                
                                // First ensure categories exist in the Category_Tag table
                                foreach (string category in checkedListBoxCategories.CheckedItems)
                                {
                                    // Check if category exists
                                    bool categoryExists = false;
                                    using (var cmd = conn.CreateCommand())
                                    {
                                        cmd.CommandText = "SELECT COUNT(*) FROM Category_Tag WHERE cname = :categoryName";
                                        cmd.Parameters.Add("categoryName", OracleDbType.Varchar2).Value = category;
                                        
                                        categoryExists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                                    }
                                    
                                    // If category doesn't exist, create it
                                    if (!categoryExists)
                                    {
                                        using (var cmd = conn.CreateCommand())
                                        {
                                            string icon = "📝"; // Default icon
                                            
                                            // Assign specific icons based on category
                                            switch (category)
                                            {
                                                case "Art": icon = "🎨"; break;
                                                case "Music": icon = "🎵"; break;
                                                case "Theatre": icon = "🎭"; break;
                                                case "Talk": icon = "🎤"; break;
                                                case "Science": icon = "🔬"; break;
                                                case "Culture": icon = "🌏"; break;
                                                case "Food": icon = "🍴"; break;
                                                case "Sports": icon = "⚽"; break;
                                                case "Education": icon = "📚"; break;
                                                case "Tech": icon = "💻"; break;
                                                case "Health": icon = "❤️"; break;
                                                case "Travel": icon = "✈️"; break;
                                                case "History": icon = "📜"; break;
                                                case "Literature": icon = "📖"; break;
                                                case "Fashion": icon = "👗"; break;
                                                case "Business": icon = "💼"; break;
                                                case "Finance": icon = "💰"; break;
                                                case "Law": icon = "⚖️"; break;
                                                case "Politics": icon = "🏛️"; break;
                                                case "Dating": icon = "❤️"; break;
                                                case "Family": icon = "👪"; break;
                                                case "Animals": icon = "🐾"; break;
                                                case "Gaming": icon = "🎮"; break;
                                                case "Environment": icon = "🌿"; break;
                                            }
                                            
                                            cmd.CommandText = @"INSERT INTO Category_Tag (cname, description, icon) 
                                                              VALUES (:categoryName, :description, :icon)";
                                            cmd.Parameters.Add("categoryName", OracleDbType.Varchar2).Value = category;
                                            cmd.Parameters.Add("description", OracleDbType.Clob).Value = 
                                                $"Events related to {category.ToLower()}.";
                                            cmd.Parameters.Add("icon", OracleDbType.Varchar2).Value = icon;
                                            
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                
                                // Insert categories
                                foreach (string category in checkedListBoxCategories.CheckedItems)
                                {
                                    using (var cmd = conn.CreateCommand())
                                    {
                                        cmd.CommandText = "INSERT INTO Has_a (cname, ename) VALUES (:categoryName, :eventName)";
                                        cmd.Parameters.Add("categoryName", OracleDbType.Varchar2).Value = category;
                                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                                        
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                
                                // Check if restriction exists
                                if (comboBoxRestrictions.SelectedItem != null)
                                {
                                    string restrictionName = comboBoxRestrictions.SelectedItem.ToString();
                                    
                                    // Check if restriction exists in Restrictions table
                                    bool restrictionExists = false;
                                    using (var cmd = conn.CreateCommand())
                                    {
                                        cmd.CommandText = "SELECT COUNT(*) FROM Restrictions WHERE rname = :restrictionName";
                                        cmd.Parameters.Add("restrictionName", OracleDbType.Varchar2).Value = restrictionName;
                                        
                                        restrictionExists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                                    }
                                    
                                    // If restriction doesn't exist, create it
                                    if (!restrictionExists)
                                    {
                                        using (var cmd = conn.CreateCommand())
                                        {
                                            string description = "";
                                            
                                            switch (restrictionName)
                                            {
                                                case "Adults(R18+)":
                                                    description = "Event restricted to adults 18 years and older.";
                                                    break;
                                                case "All Ages":
                                                    description = "Event suitable for people of all ages.";
                                                    break;
                                                case "Teens(R13+)":
                                                    description = "Event restricted to teens 13 years and older.";
                                                    break;
                                                case "Seniors(65+)":
                                                    description = "Event primarily for seniors 65 years and older.";
                                                    break;
                                            }
                                            
                                            cmd.CommandText = @"INSERT INTO Restrictions (rname, description) 
                                                              VALUES (:restrictionName, :description)";
                                            cmd.Parameters.Add("restrictionName", OracleDbType.Varchar2).Value = restrictionName;
                                            cmd.Parameters.Add("description", OracleDbType.Clob).Value = description;
                                            
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    
                                    // Insert restriction
                                    using (var cmd = conn.CreateCommand())
                                    {
                                        cmd.CommandText = "INSERT INTO Has (rname, ename) VALUES (:restrictionName, :eventName)";
                                        cmd.Parameters.Add("restrictionName", OracleDbType.Varchar2).Value = restrictionName;
                                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                                        
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            
                            // Check if this event instance already exists
                            bool instanceExists = false;
                            DateTime eventDate = dateTimePickerDate.Value.Date;
                            
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = @"SELECT COUNT(*) FROM Event_Instance 
                                                   WHERE event_date = :eventDate AND vname = :venueName AND ename = :eventName";
                                cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;
                                cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                                
                                instanceExists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                            }
                            
                            if (instanceExists)
                            {
                                MessageBox.Show("An instance of this event already exists on this date at this venue.", 
                                    "Duplicate Event Instance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            
                            // Insert into Event_Instance table
                            DateTime eventTime = dateTimePickerTime.Value;
                            DateTime combinedDateTime = eventDate.Add(eventTime.TimeOfDay);
                            
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = @"INSERT INTO Event_Instance (event_date, vname, ename, price, time) 
                                                  VALUES (:eventDate, :venueName, :eventName, :price, :eventTime)";
                                                  
                                cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;
                                cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                                cmd.Parameters.Add("price", OracleDbType.Decimal).Value = numericUpDownPrice.Value;
                                cmd.Parameters.Add("eventTime", OracleDbType.TimeStamp).Value = combinedDateTime;
                                
                                cmd.ExecuteNonQuery();
                            }
                            
                            // Commit transaction
                            transaction.Commit();
                            
                            MessageBox.Show("Event created successfully!", "Success", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                                
                            // Reset form or close
                        }
                        catch (Exception ex)
                        {
                            // Rollback transaction on error
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating event: {ex.Message}", "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            /*string eName = textBoxEventName.Text.Trim();
            string eDescription = textBoxDescription.Text.Trim();
            DateTime creationDate = DateTime.Now;
            string club = textBoxCompany.Text.Trim();


            DateTime eDate = dateTimePicker.Value.Date;
            string vName = comboBoxLocation.Text.Trim();
            string price = textBoxPrice.Text.Trim();
            //string time = textBoxTime.Text.Trim();


            string category = comboBoxCategory.Text.Trim();
            //string max = textBoxMaxAttendees.Text.Trim();
            string restriction = comboBoxRestrictions.Text.Trim();

            // Build & execute a parameterized INSERT
            const string sql = @"INSERT INTO Event values (ename, description, creation_date, company_club) 
                                VALUES(:eName, :eDescription, :date, :club)";

            using (var conn = DbConfig.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;

                // bind parameters
                cmd.Parameters.Add("ename", OracleDbType.Varchar2).Value = eName;
                cmd.Parameters.Add("description", OracleDbType.Varchar2).Value = eDescription;
                cmd.Parameters.Add("date", OracleDbType.Varchar2).Value = creationDate;
                cmd.Parameters.Add("club", OracleDbType.Varchar2).Value = club;

                conn.Open();
                int inserted = cmd.ExecuteNonQuery();  // returns #rows affected

                MessageBox.Show(inserted == 1 ? "Registration successful!" : "Oops—no rows inserted.");


            }

            // Build & execute a parameterized INSERT
            const string sql1 = @"INSERT INTO Event_Instance values (event_date, vname, ename, price, time) 
                                VALUES(:eDate, :vName, :eName, :price, :time)";

            using (var conn = DbConfig.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql1;

                // bind parameters
                cmd.Parameters.Add("eDate", OracleDbType.Varchar2).Value = eDate;
                cmd.Parameters.Add("vName", OracleDbType.Varchar2).Value = vName;
                cmd.Parameters.Add("eName", OracleDbType.Varchar2).Value = eName;
                cmd.Parameters.Add("price", OracleDbType.Varchar2).Value = price;
                //cmd.Parameters.Add("time", OracleDbType.Varchar2).Value = time;
                cmd.Parameters.Add("time", OracleDbType.Varchar2).Value = "2025-06-01 18:00:00";

                conn.Open();
                int inserted = cmd.ExecuteNonQuery();  // returns #rows affected

                MessageBox.Show(inserted == 1 ? "Registration successful!" : "Oops—no rows inserted.");


            }

            // Build & execute a parameterized INSERT
            const string sql2 = @"INSERT INTO has values (rname, ename) 
                                VALUES(:restriction, :eName)";

            using (var conn = DbConfig.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql2;

                // bind parameters
                cmd.Parameters.Add("rname", OracleDbType.Varchar2).Value = restriction;
                cmd.Parameters.Add("ename", OracleDbType.Varchar2).Value = eName;
                

                conn.Open();
                int inserted = cmd.ExecuteNonQuery();  // returns #rows affected

                MessageBox.Show(inserted == 1 ? "Registration successful!" : "Oops—no rows inserted.");


            }

            // Build & execute a parameterized INSERT
            const string sql3 = @"INSERT INTO has_a values (cname, ename) 
                                VALUES(:cName, :eName)";

            using (var conn = DbConfig.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql3;

                // bind parameters
                cmd.Parameters.Add("cName", OracleDbType.Varchar2).Value = category;
                cmd.Parameters.Add("eName", OracleDbType.Varchar2).Value = eName;


                conn.Open();
                int inserted = cmd.ExecuteNonQuery();  // returns #rows affected

                MessageBox.Show(inserted == 1 ? "Registration successful!" : "Oops—no rows inserted.");


            }

            // Build & execute a parameterized INSERT
            const string sql4 = @"INSERT INTO organises values (acc_num, ename) 
                                VALUES(:userId, :eName)";

            using (var conn = DbConfig.GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql4;

                // bind parameters
                cmd.Parameters.Add("userId", OracleDbType.Varchar2).Value = Session.CurrentUser;
                cmd.Parameters.Add("eName", OracleDbType.Varchar2).Value = eName;


                conn.Open();
                int inserted = cmd.ExecuteNonQuery();  // returns #rows affected

                MessageBox.Show(inserted == 1 ? "Registration successful!" : "Oops—no rows inserted.");


            }*/
        }

        private void comboBoxLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            /*try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();

                    // Search for events and display all events details when form opens
                    using (var cmd = conn.CreateCommand())
                    {
                        int max = 0;


                        cmd.CommandText = "select capacity from venue";

                        using (var reader = cmd.ExecuteReader())
                        {
                            // Add items to comboBox
                            if (reader.Read())
                            {
                                max = reader.GetInt32(0);

                            }
                        }
                        textBoxMaxAttendees.Text = max.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/

        }

        //if an existing event is selected from the user then load its details 
        private void comboBoxEventName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedEvent = comboBoxEventName.SelectedItem?.ToString();
            
            if (!string.IsNullOrEmpty(selectedEvent))
            {
                try
                {
                    using (var conn = DbConfig.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SELECT description, company_club FROM Event WHERE ename = :eventName";
                            cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = selectedEvent;
                            
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    textBoxDescription.Text = reader["description"].ToString();
                                    textBoxCompanyClub.Text = reader["company_club"].ToString();
                                }
                            }
                        }
                        
                        // Load categories
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SELECT cname FROM Has_a WHERE ename = :eventName";
                            cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = selectedEvent;
                            
                            using (var reader = cmd.ExecuteReader())
                            {
                                // Uncheck all first
                                for (int i = 0; i < checkedListBoxCategories.Items.Count; i++)
                                {
                                    checkedListBoxCategories.SetItemChecked(i, false);
                                }
                                
                                // Check categories that apply to this event
                                while (reader.Read())
                                {
                                    string categoryName = reader["cname"].ToString();
                                    int index = checkedListBoxCategories.Items.IndexOf(categoryName);
                                    if (index >= 0)
                                    {
                                        checkedListBoxCategories.SetItemChecked(index, true);
                                    }
                                }
                            }
                        }
                        
                        // Load restrictions
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SELECT rname FROM Has WHERE ename = :eventName";
                            cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = selectedEvent;
                            
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string restrictionName = reader["rname"].ToString();
                                    comboBoxRestrictions.SelectedItem = restrictionName;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading event details: {ex.Message}", "Database Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                    using (var conn = DbConfig.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = @"SELECT capacity, street_num, street_name, suburb, 
                                               city, postcode, country FROM Venue 
                                               WHERE vname = :venueName";
                                               
                            cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = selectedVenue;
                            
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    numericUpDownCapacity.Value = Convert.ToDecimal(reader["capacity"]);
                                    numericUpDownStreetNum.Value = Convert.ToDecimal(reader["street_num"]);
                                    textBoxStreetName.Text = reader["street_name"].ToString();
                                    textBoxSuburb.Text = reader["suburb"].ToString();
                                    comboBoxCity.Text = reader["city"].ToString();
                                    textBoxPostCode.Text = reader["postcode"].ToString();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading venue details: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        //clears all inputs, unchecks categoreis, resets dropdowns and refreshes event names list
        private void buttonClear_Click(object sender, EventArgs e)
        {
            comboBoxEventName.Text = "";
            textBoxDescription.Text = "";
            textBoxCompanyClub.Text = "";
            dateTimePickerDate.Value = DateTime.Today;
            dateTimePickerTime.Value = DateTime.Now;
            comboBoxVenue.Text = "";
            numericUpDownCapacity.Value = numericUpDownCapacity.Minimum;
            numericUpDownStreetNum.Value = numericUpDownStreetNum.Minimum;
            textBoxStreetName.Text = "";
            textBoxSuburb.Text = "";
            comboBoxCity.SelectedIndex = -1;
            textBoxPostCode.Text = "";
            numericUpDownPrice.Value = 0;
            
            for (int i = 0; i < checkedListBoxCategories.Items.Count; i++)
            {
                checkedListBoxCategories.SetItemChecked(i, false);
            }
            
            comboBoxRestrictions.SelectedIndex = -1;

            LoadEventName();
        }
    }
}
