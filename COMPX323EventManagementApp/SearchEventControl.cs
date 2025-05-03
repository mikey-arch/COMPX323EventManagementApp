using System;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace COMPX323EventManagementApp
{
    public partial class SearchEventControl : UserControl
    {
        public SearchEventControl()
        {
            InitializeComponent();
            LoadComboBoxes();
            DisplayEvents();
        }

        //loads all comboboxes with appropriate filters names
        private void LoadComboBoxes()
        {
            try
            {
                dateTimePickerMonth.CustomFormat = "MMMM yyyy";
                dateTimePickerMonth.Value = DateTime.Now;
                comboBoxPrice.Items.Add("Asc");
                comboBoxPrice.Items.Add("Desc");
                comboBoxPrice.Items.Add("Free");
                comboBoxPrice.SelectedIndex = 0;

                using(var conn = DbConfig.GetConnection())
                {
                    conn.Open();

                    //add locations city names from venue table 
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select distinct city from venue order by city asc";
                        comboBoxLocation.Items.Add("All Locations");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) 
                            {
                                comboBoxLocation.Items.Add(reader.GetString(0));
                            }
                        }
                        comboBoxLocation.SelectedIndex = 0;
                    }

                    //add in each category from category table
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select cname from category_tag order by cname asc";
                        comboBoxCategory.Items.Add("All Categories");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) 
                            {
                                comboBoxCategory.Items.Add(reader.GetString(0));
                            }
                        }
                        comboBoxCategory.SelectedIndex = 0;
                    }

                    //fill restrictions combobox with restrictions from restrictions table
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select rname from Restrictions order by rname asc";
                        comboBoxRestriction.Items.Add("All Restrictions");
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxRestriction.Items.Add(reader.GetString(0));
                            }
                        }
                        comboBoxRestriction.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading filters: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //displays updated filtered events in the listview
        private void DisplayEvents()
        {
            try
            {
                // Clear existing items
                listViewEvents.Items.Clear();

                // Make sure ListView is properly set up
                if (listViewEvents.Columns.Count == 0)
                {
                    listViewEvents.View = View.Details;
                    listViewEvents.FullRowSelect = true;
                    listViewEvents.GridLines = true;

                    listViewEvents.Columns.Add("Event Name", 180);
                    listViewEvents.Columns.Add("Date", 100);
                    listViewEvents.Columns.Add("Time", 80);
                    listViewEvents.Columns.Add("Venue", 150);
                    listViewEvents.Columns.Add("Price", 80);
                    listViewEvents.Columns.Add("Category", 100);
                    listViewEvents.Columns.Add("Restrictions", 120);
                }

                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();

                    // Build the query with all the filters
                    string query = @"
                    select distinct e.ename, ei.event_date, ei.time, v.vname, v.city, ei.price, ct.cname, r.rname
                    from event e
                    join event_instance ei on e.ename = ei.ename
                    join venue v on ei.vname = v.vname
                    left join has_a ha on e.ename = ha.ename
                    left join category_tag ct on ha.cname = ct.cname
                    left join has h on e.ename = h.ename
                    left join restrictions r on h.rname = r.rname
                    where 1=1";

                    // handles the price combo box filtering selection
                    string priceFilter = comboBoxPrice.SelectedItem?.ToString();
                    if (!string.IsNullOrEmpty(priceFilter))
                    {
                        if (priceFilter == "Free")
                        {
                            query += " and ei.price = 0";
                        }
                    }

                    // handles location filter
                    string locationFilter = comboBoxLocation.SelectedItem?.ToString();
                    if (!string.IsNullOrEmpty(locationFilter) && locationFilter != "All Locations")
                    {
                        query += " and v.city = :location";
                    }

                    // hanldes category filter
                    string categoryFilter = comboBoxCategory.SelectedItem?.ToString();
                    if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "All Categories")
                    {
                        query += " and ct.cname = :category";
                    }

                    // handles restrictions filter
                    string restrictionFilter = comboBoxRestriction.SelectedItem?.ToString();
                    if (!string.IsNullOrEmpty(restrictionFilter) && restrictionFilter != "All Restrictions")
                    {
                        query += " and r.rname = :restriction";
                    }

                    //add date filtering
                    DateTime selectedDate = dateTimePickerMonth.Value;
                    query += " and extract(month from ei.event_date) = :month and extract(year from ei.event_date) = :year";

                    //add sorting filter
                    if (priceFilter == "Asc")
                    {
                        query += " order by ei.price asc, ei.event_date asc";
                    }
                    else if (priceFilter == "Desc")
                    {
                        query += " order by ei.price desc, ei.event_date asc";
                    }
                    else
                    {
                        query += " order by ei.event_date asc, ei.time asc";
                    }

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;

                        // add location, category, restriction and date parameters
                        if (!string.IsNullOrEmpty(locationFilter) && locationFilter != "All Locations")
                        {
                            cmd.Parameters.Add("location", OracleDbType.Varchar2).Value = locationFilter;
                        }

                        if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "All Categories")
                        {
                            cmd.Parameters.Add("category", OracleDbType.Varchar2).Value = categoryFilter;
                        }

                        if (!string.IsNullOrEmpty(restrictionFilter) && restrictionFilter != "All Restrictions")
                        {
                            cmd.Parameters.Add("restriction", OracleDbType.Varchar2).Value = restrictionFilter;
                        }

                        cmd.Parameters.Add("month", OracleDbType.Int32).Value = selectedDate.Month;
                        cmd.Parameters.Add("year", OracleDbType.Int32).Value = selectedDate.Year;

                        using (var reader = cmd.ExecuteReader())
                        {
                            bool foundResults = false;

                            while (reader.Read())
                            {
                                foundResults = true;

                                // create list view item with event details, adding eventname, date, time, venue, price, category and restrictiosn
                                ListViewItem item = new ListViewItem(reader.GetString(0)); 

                                DateTime eventDate = reader.GetDateTime(1);
                                item.SubItems.Add(eventDate.ToString("dd-MM-yyyy"));

                                DateTime eventTime = reader.GetDateTime(2);
                                item.SubItems.Add(eventTime.ToString("HH:mm"));

                                item.SubItems.Add(reader.GetString(3));

                                decimal price = reader.GetDecimal(5);
                                item.SubItems.Add(price.ToString("C"));

                                item.SubItems.Add(reader.GetString(6));
                                item.SubItems.Add(reader.GetString(7));
                                listViewEvents.Items.Add(item);
                            }
                            labelEvents.Text = foundResults ? $"Found {listViewEvents.Items.Count} events" : "No events found matching your criteria";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying events: " + ex.Message, "Database Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayEvents();
        }
        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayEvents();
        }
        private void comboBoxRestriction_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayEvents();
        }
        private void comboBoxPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayEvents();
        }
        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DisplayEvents();
        }

        //should handle if selected then showeventdetails and go to that events details
        private void listViewEvents_DoubleClick(object sender, EventArgs e)
        {
            if (listViewEvents.SelectedItems.Count > 0)
            {
                // Get the selected item. Assuming it contains event data (event name, date, and venue)
                var selectedEvent = listViewEvents.SelectedItems[0];

                // Extract event name, event date, and venue name from the ListView columns
                string eventName = selectedEvent.Text; // First column is event name
                DateTime eventDate = DateTime.Parse(selectedEvent.SubItems[1].Text); // Second column is event date
                string venueName = selectedEvent.SubItems[3].Text; // Fourth column is venue name

                // Open the EventDetails form and pass the event name, event date, and venue name
                EventDetails eventDetailsForm = new EventDetails(eventName, eventDate, venueName);
                eventDetailsForm.Show();
            }


        }
    }
}
