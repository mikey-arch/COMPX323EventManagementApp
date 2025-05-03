using COMPX323EventManagementApp.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace COMPX323EventManagementApp
{
    public partial class ManageEventControl : UserControl
    {
        public ManageEventControl()
        {
            InitializeComponent();
            InitialiseComboBox();
            InitialiseListView();
        
            // Refresh the data when control is brought up
            this.VisibleChanged += ManageEventControl_VisibleChanged;
        }

        // Initialize ComboBox to show placeholder text initially
        private void InitialiseComboBox()
        {
            comboBoxEventList.Items.Clear();
            comboBoxEventList.SelectedIndex = -1;  // Ensuring ComboBox is empty initially
        }

        private void InitialiseListView()
        {
            // Check if ListView is already initialized
            if (listViewEvents.Columns.Count == 0)
            {
                listViewEvents.View = View.Details;
                listViewEvents.FullRowSelect = true;
                listViewEvents.GridLines = true;

                // Add columns for the ListView
                listViewEvents.Columns.Add("Event Name", 180);
                listViewEvents.Columns.Add("Date", 100);
                listViewEvents.Columns.Add("Time", 80);
                listViewEvents.Columns.Add("Venue", 150);
                listViewEvents.Columns.Add("City", 120);
                listViewEvents.Columns.Add("Price", 80);
            }
        }


        private void comboBoxEventList_DropDown(object sender, EventArgs e)
        {
            try
            {
                // Get the current user's ID
                User user = Session.CurrentUser;
                int attendeeId = user.Id;

                // SQL query to get all events created by the current user
                string query = @"
                    SELECT e.ename 
                    FROM Event e
                    JOIN Organises o ON e.ename = o.ename
                    WHERE o.acc_num = :attendeeId";

                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.Add("attendeeId", OracleDbType.Int32).Value = attendeeId;

                        using (var reader = cmd.ExecuteReader())
                        {
                            List<string> eventsList = new List<string>();

                            while (reader.Read())
                            {
                                string eventName = reader.GetString(reader.GetOrdinal("ename"));
                                eventsList.Add(eventName);
                            }

                            // Fill the ComboBox with the event names
                            comboBoxEventList.DataSource = eventsList;
                            comboBoxEventList.SelectedIndex = -1;  // Reset to unselected
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading events: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxEventList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Check if the user selected an actual event (not the placeholder)
                if (comboBoxEventList.SelectedIndex >= 0)
                {
                    string selectedEventName = comboBoxEventList.SelectedItem.ToString();
                    listViewEvents.Items.Clear(); 

                    // Call the DisplayEvents method to show event instances for the selected event
                    DisplayEvents(selectedEventName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading event instances: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayEvents(string selectedEventName)
        {
            try
            {
                // SQL query to get all event instances for the selected event
                string query = @"
            SELECT DISTINCT e.ename, ei.event_date, ei.time, v.vname, v.city, ei.price
            FROM event_instance ei
            JOIN event e ON ei.ename = e.ename
            JOIN venue v ON ei.vname = v.vname
            WHERE e.ename = :eventName";

                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = selectedEventName;

                        using (var reader = cmd.ExecuteReader())
                        {
                            bool foundResults = false;
                            listViewEvents.Items.Clear(); // Clear any previous data in the ListView

                            // Iterate through each result
                            while (reader.Read())
                            {
                                foundResults = true;

                                // Create a new ListViewItem with the event name as the first column
                                ListViewItem item = new ListViewItem(reader.GetString(reader.GetOrdinal("ename")));

                                // Add subitems to the ListViewItem based on the data
                                item.SubItems.Add(reader.GetDateTime(reader.GetOrdinal("event_date")).ToString("yyyy-MM-dd")); // Date
                                item.SubItems.Add(reader.GetDateTime(reader.GetOrdinal("time")).ToString("HH:mm")); // Time
                                item.SubItems.Add(reader.GetString(reader.GetOrdinal("vname"))); // Venue Name
                                item.SubItems.Add(reader.GetString(reader.GetOrdinal("city"))); // Venue City
                                item.SubItems.Add(reader.GetDecimal(reader.GetOrdinal("price")).ToString("C")); // Price (Currency)

                                // Add the item to the ListView
                                listViewEvents.Items.Add(item);
                            }

                            // If no results are found, display a message
                            if (!foundResults)
                            {
                                ListViewItem noDataItem = new ListViewItem("No event instances found.");
                                listViewEvents.Items.Add(noDataItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying event instances: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listViewEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Ensure an item is selected in the ListView
                if (listViewEvents.SelectedItems.Count > 0)
                {
                    var selectedItem = listViewEvents.SelectedItems[0];

                    // Get event details from the selected ListView item
                    string eventName = selectedItem.Text; // The first column: Event Name
                    DateTime eventDate = DateTime.Parse(selectedItem.SubItems[1].Text); // Second column: Event Date
                    string venueName = selectedItem.SubItems[3].Text; // Fourth column: Venue Name

                    // Call the method to display RSVPs for this specific event instance
                    DisplayRSVPs(eventName, eventDate, venueName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting event instance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to display RSVPs for the selected event instance
        private void DisplayRSVPs(string eventName, DateTime eventDate, string venueName)
        {
            try
            {
                // SQL query to fetch RSVPs for the selected event instance
                string query = @"
            SELECT a.fname, a.lname, a.email, r.status
            FROM RSVP r
            JOIN Attendee a ON r.acc_num = a.acc_num
            JOIN Event e ON r.ename = e.ename
            WHERE e.ename = :eventName
            AND r.event_date = :eventDate
            AND r.vname = :venueName";

                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                        cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;
                        cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;

                        using (var reader = cmd.ExecuteReader())
                        {
                            listBoxRSVPDisplay.Items.Clear(); // Clear previous data in ListBox

                            bool foundRSVPs = false;
                            while (reader.Read())
                            {
                                foundRSVPs = true;

                                // Format the attendee details and add to ListBox
                                string attendeeInfo = $"{reader.GetString(reader.GetOrdinal("fname"))} " +
                                                      $"{reader.GetString(reader.GetOrdinal("lname"))} " +
                                                      $"({reader.GetString(reader.GetOrdinal("email"))}) - " +
                                                      $"{reader.GetString(reader.GetOrdinal("status"))}";

                                listBoxRSVPDisplay.Items.Add(attendeeInfo);
                            }

                            // If no RSVPs found, show a message in the ListBox
                            if (!foundRSVPs)
                            {
                                listBoxRSVPDisplay.Items.Add("No RSVPs for this event instance.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading RSVPs: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Called every time the visibility of the control changes (e.g., when brought back to view)
        private void ManageEventControl_VisibleChanged(object sender, EventArgs e)
        {
            // Check if the control is being shown
            if (this.Visible)
            {
                // Refresh data
                listViewEvents.Items.Clear();
                listBoxRSVPDisplay.Items.Clear();

                // Refresh the ComboBox events list
                comboBoxEventList.SelectedIndex = -1; // Clear the selection

                // Refresh event data and RSVPs if needed
                // You can either call comboBoxEventList_SelectedIndexChanged or DisplayEvents
                DisplayEvents(comboBoxEventList.SelectedItem?.ToString());
            }
        }

        private void buttonDelInstance_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure an item is selected in the ListView
                if (listViewEvents.SelectedItems.Count > 0)
                {
                    var selectedItem = listViewEvents.SelectedItems[0];

                    // Extract event details from the selected ListView item
                    string eventName = selectedItem.Text; // The first column: Event Name
                    DateTime eventDate = DateTime.Parse(selectedItem.SubItems[1].Text); // Second column: Event Date
                    string venueName = selectedItem.SubItems[3].Text; // Fourth column: Venue Name

                    // Confirm deletion
                    DialogResult result = MessageBox.Show(
                        $"Are you sure you want to delete the event instance '{eventName}' on {eventDate.ToString("yyyy-MM-dd")} at {venueName}?",
                        "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        // SQL query to delete the selected event instance
                        string query = @"
                            DELETE FROM event_instance
                            WHERE ename = :eventName
                            AND event_date = :eventDate
                            AND vname = :venueName";

                        using (var conn = DbConfig.GetConnection())
                        {
                            conn.Open();
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = query;
                                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                                cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;
                                cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;

                                // Execute the delete query
                                int rowsAffected = cmd.ExecuteNonQuery();

                                // If no rows are affected, the deletion failed
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Event instance deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Refresh the ListView and ListBox
                                    listViewEvents.Items.Clear();
                                    listBoxRSVPDisplay.Items.Clear();
                                    comboBoxEventList.SelectedIndex = -1; // Clear ComboBox selection
                                    DisplayEvents(comboBoxEventList.SelectedItem?.ToString()); // Refresh the event instances
                                }
                                else
                                {
                                    MessageBox.Show("Error: The event instance could not be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select an event instance to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the event instance: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
