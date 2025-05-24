using COMPX323EventManagementApp.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
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
            InitialiseRSVPListView();

            // Refresh the data when control is brought up
            this.VisibleChanged += ManageEventControl_VisibleChanged;
        }

        // Initialize ComboBox to show placeholder text initially
        private void InitialiseComboBox()
        {
            comboBoxEventList.Items.Clear();
            comboBoxEventList.SelectedIndex = -1;  // Ensuring ComboBox is empty initially
        }
        private void InitialiseRSVPListView()
        {
            if (listViewRSVP.Columns.Count == 0)
            {
                listViewRSVP.View = View.Details;
                listViewRSVP.FullRowSelect = true;
                listViewRSVP.GridLines = true;

                listViewRSVP.Columns.Add("Name", 200);         
                listViewRSVP.Columns.Add("Email", 220);        
                listViewRSVP.Columns.Add("Status", 100);       
            }
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
                Member user = Session.CurrentUser;
                int memberId = user.Id;

                // SQL query to get all events created by the current user
                string query = @"
                    SELECT e.ename 
                    FROM Event e
                    WHERE e.creator_num = :memberId";

                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.Add("memberId", OracleDbType.Int32).Value = memberId;

                        using (var reader = cmd.ExecuteReader())
                        {
                            List<string> eventsList = new List<string> { "-- Select an event --" }; // Placeholder
                            while (reader.Read())
                            {
                                string eventName = reader.GetString(reader.GetOrdinal("ename"));
                                eventsList.Add(eventName);
                            }
                            comboBoxEventList.DataSource = eventsList;
                            comboBoxEventList.SelectedIndex = 0; // Set to placeholder
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
                if (comboBoxEventList.SelectedIndex == 0) // Placeholder selected
                {
                    // Clear all related UI elements
                    listViewEvents.Items.Clear();
                    listViewRSVP.Items.Clear();
                    return;
                }
                if (comboBoxEventList.SelectedIndex > 0) // ignore placeholder at index 0
                {
                    string selectedEventName = comboBoxEventList.SelectedItem.ToString();
                    listViewEvents.Items.Clear();
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
                                
                                DateTime eventDate = reader.GetDateTime(reader.GetOrdinal("event_date"));
                                DateTime eventTime = reader.GetDateTime(reader.GetOrdinal("time"));

                                item.SubItems.Add(Convert.ToDateTime(reader["event_date"]).ToString("dd-MM-yyyy"));

                                item.SubItems.Add(eventTime.ToString("HH:mm"));      // Shows actual time


                                // Store the original DateTime in the tag
                                item.Tag = new
                                {
                                    EventName = selectedEventName,
                                    EventDateTime = eventDate,
                                    VenueName = reader.GetString(reader.GetOrdinal("vname"))
                                };


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
                    select m.fname, m.lname, m.email, r.status
                    FROM RSVP r
                    join event e on r.ename = e.ename
                    join member m on m.acc_num = r.acc_num
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
                            listViewRSVP.Items.Clear(); // Clear previous data in ListBox
                            listViewRSVP.Enabled = true;
                            bool foundRSVPs = false;

                            while (reader.Read())
                            {
                                foundRSVPs = true;

                                string fullName = $"{reader.GetString(reader.GetOrdinal("fname"))} {reader.GetString(reader.GetOrdinal("lname"))}";
                                string email = reader.GetString(reader.GetOrdinal("email"));

                                string status = reader.GetString(reader.GetOrdinal("status"));

                                ListViewItem item = new ListViewItem(fullName);
                                item.SubItems.Add(email);
                                item.SubItems.Add(status);

                                // Store event info as an object (anonymous or custom class)
                                item.Tag = new { EventName = eventName, EventDate = eventDate, VenueName = venueName };

                                listViewRSVP.Items.Add(item);

                            }

                            // If no RSVPs found, show a message in the ListBox
                            if (!foundRSVPs)
                            {
                                listViewRSVP.Items.Add("No RSVPs for this event instance.");
                                listViewRSVP.Enabled = false;
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
                listViewRSVP.Items.Clear();

                // Refresh the ComboBox events list
                comboBoxEventList.SelectedIndex = -1; // Clear the selection

                // Refresh event data and RSVPs if needed
                if (comboBoxEventList.SelectedIndex > 0)
                {
                    DisplayEvents(comboBoxEventList.SelectedItem.ToString());
                }

            }
        }

        private void buttonDelInstance_Click(object sender, EventArgs e)
        {
            try
            {
                Delete(2);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the event instance: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void listViewRSVP_Click(object sender, EventArgs e)
        {
            if (listViewRSVP.SelectedItems.Count > 0)
            {
                CustomMessageBox customMsgBox = new CustomMessageBox();
                customMsgBox.Configure("Do you want to delete this RSVP?", showDelete: true, showDetails: false);
                customMsgBox.ShowDialog();

                var selectedItem = listViewRSVP.SelectedItems[0];
                var tag = (dynamic)selectedItem.Tag;

                if (customMsgBox.SelectedOption == "Delete RSVP")
                {
                    //DeleteRsvp(tag.EventName, tag.EventDate, tag.VenueName);
                    Delete(3);
                }


            }
        }

        private void Delete(int num)
        {
            try {

                if (comboBoxEventList.SelectedItem == null || comboBoxEventList.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select an event first.");
                    return;
                }

                if ((num == 2 || num == 3) && listViewEvents.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select an event instance first.");
                    return;
                }

                if (num == 3 && listViewRSVP.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select an RSVP to delete.");
                    return;
                }

                // Get the attendee ID from the current user session
                Member user = Session.CurrentUser;
                int attendeeId = user.Id;

                string query = "";
                string venueName = "";
                DateTime eventDate = DateTime.MinValue;

                string eventName = comboBoxEventList.SelectedItem.ToString();

                if (num == 2 || num == 3)
                {
                    var selectedItem = listViewEvents.SelectedItems[0];
                    //eventName = selectedItem.Text;

                    var tag = (dynamic)selectedItem.Tag;

                    eventDate = DateTime.Parse(selectedItem.SubItems[1].Text);

                    venueName = tag.VenueName;

                }


                // Determine the item to delete based on the parameter (1 for event, 2 for instance, 3 for RSVP)
                switch (num)
                {
                    case 3: // RSVP
                        if (listViewRSVP.SelectedItems.Count > 0)
                        {

                            query = @"
                            DELETE FROM RSVP
                            WHERE acc_num = :attendeeId
                            AND ename = :eventName
                            AND vname = :venueName
                            AND event_date = :eventDate";
                        }
                        break;

                    case 2: // Instance
                        if (listViewEvents.SelectedItems.Count > 0)
                        {

                            // Delete all RSVPs for this event instance before deleting the instance
                            DeleteRSVPs(eventName, eventDate, venueName);

                            query = @"
                            DELETE FROM event_instance
                            WHERE ename = :eventName
                            AND vname = :venueName
                            AND event_date = :eventDate";
                        }
                        break;

                    case 1: // Event
                        if (comboBoxEventList.SelectedItem != null)
                        {
                            // Delete all event instances and their RSVPs before deleting the event
                            DeleteAllEventInstances(eventName);
                            DeleteRelations(eventName);

                            query = @"
                            DELETE FROM event
                            WHERE ename = :eventName";
                        }
                        break;
                }

                // Execute the delete query only if query is not empty
                if (!string.IsNullOrEmpty(query))
                {
                    using (var conn = DbConfig.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = query;
                            // Add parameters for deletion
                            if (num == 3)
                            {
                                cmd.Parameters.Add("attendeeId", Oracle.ManagedDataAccess.Client.OracleDbType.Int32).Value = attendeeId;
                                cmd.Parameters.Add("eventName", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2).Value = eventName;
                                cmd.Parameters.Add("venueName", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2).Value = venueName;
                                cmd.Parameters.Add("eventDate", Oracle.ManagedDataAccess.Client.OracleDbType.Date).Value = eventDate;

                            }
                            else if (num == 2)
                            {
                                cmd.Parameters.Add("eventName", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2).Value = eventName;
                                cmd.Parameters.Add("venueName", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2).Value = venueName;
                                cmd.Parameters.Add("eventDate", Oracle.ManagedDataAccess.Client.OracleDbType.Date).Value = eventDate;
                            }
                            else if (num == 1)
                            {
                                cmd.Parameters.Add("eventName", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2).Value = eventName;

                            }


                            // Execute the delete command
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Delete successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                RefreshUI();  // You can implement a method to refresh your UI after deletion.
                            }
                            else
                            {
                                MessageBox.Show("No matching record found to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            } catch (Exception ex){
                MessageBox.Show("Please select an event instance to delete/delete from. Error: " + ex.Message);
            }
            
        }


        private void DeleteRelations(string eventName)
        {
            // Queries for deleting relations from 'has_a' and 'organises' tables
            string[] queries = new string[]
            {
                @"DELETE FROM event_category WHERE ename = :eventName",
            };

            // Execute both queries in a single connection
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    foreach (var query in queries)
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.Clear();  // Clear previous parameters
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }


        private void DeleteRSVPs(string eventName, DateTime eventDate, string venueName)
        {
            string query;

            // If eventDate or venueName is a placeholder, delete all RSVPs for the event
            if (eventDate == DateTime.MinValue && string.IsNullOrEmpty(venueName))
            {
                query = @"
                    DELETE FROM RSVP
                    WHERE ename = :eventName";  // Only filter by event name to delete all RSVPs for the event
            }
            else
            {
                // Delete specific RSVPs based on event date and venue
                query = @"
                    DELETE FROM RSVP
                    WHERE ename = :eventName
                    AND vname = :venueName
                    AND event_date = :eventDate";
            }

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;

                    // Only add venue and event_date parameters if not deleting all RSVPs
                    if (!string.IsNullOrEmpty(venueName))
                        cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;

                    if (eventDate != DateTime.MinValue)
                        cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;

                    // Execute the delete command for RSVPs
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void DeleteAllEventInstances(string eventName)
        {

            // After deleting all instances, delete all RSVPs related to the event
            DeleteRSVPs(eventName, DateTime.MinValue, string.Empty);  // We pass placeholder values because we're deleting all instances

            // Delete all instances of the event before deleting the event itself
            string query = @"
        DELETE FROM event_instance
        WHERE ename = :eventName";

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;

                    // Execute the delete command for all instances
                    cmd.ExecuteNonQuery();
                }
            }

        }

        private void RefreshUI()
        {
            // Refresh your UI elements (ListView, ComboBox, etc.)
            listViewEvents.Items.Clear();
            listViewRSVP.Items.Clear();
            comboBoxEventList.SelectedIndex = -1;  // Clear ComboBox selection
                                                   
        }

        private void buttonDeleteEvent_Click(object sender, EventArgs e)
        {
            Delete(1);
        }


    }
}
