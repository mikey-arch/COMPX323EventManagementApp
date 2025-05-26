using COMPX323EventManagementApp.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace COMPX323EventManagementApp
{

    /// <summary>
    /// User control for managing events, event instances, and RSVPs.
    /// Allows event creators to view and delete their events, event instances, and associated RSVPs.
    /// </summary>
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

        /// <summary>
        /// Initialize ComboBox to show placeholder text initially
        /// </summary>
        private void InitialiseComboBox()
        {
            comboBoxEventList.Items.Clear();
            comboBoxEventList.SelectedIndex = -1;  // Ensuring ComboBox is empty initially
        }

        /// <summary>
        /// Initialize the RSVP ListView with columns
        /// </summary>
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

        /// <summary>
        /// Initializes the event instances ListView with appropriate columns and settings.
        /// </summary>
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

        // Event handler for when the ComboBox dropdown is opened to show the list of events
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

        /// <summary>
        /// when the user clicks on an event display the related event instances
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Method to display all event instances for the selected event in the ListView
        /// </summary>
        /// <param name="selectedEventName"></param>
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


                                item.SubItems.Add(reader.GetString(reader.GetOrdinal("vname")));
                                item.SubItems.Add(reader.GetString(reader.GetOrdinal("city"))); 
                                item.SubItems.Add(reader.GetDecimal(reader.GetOrdinal("price")).ToString("C")); 

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

        /// <summary>
        /// Event handler for when an item in the Event ListView is selected
        /// when the user selects an event instance display all the rsvps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Ensure an item is selected in the ListView
                if (listViewEvents.SelectedItems.Count > 0)
                {
                    var selectedItem = listViewEvents.SelectedItems[0];

                    if (listViewEvents.SelectedItems[0].Text == "No event instances found.")
                    {
                        return;
                    }

                    // Get event details from the selected ListView item
                    string eventName = selectedItem.Text; 
                    DateTime eventDate = DateTime.Parse(selectedItem.SubItems[1].Text); 
                    string venueName = selectedItem.SubItems[3].Text; 
                    // Call the method to display RSVPs for this specific event instance
                    DisplayRSVPs(eventName, eventDate, venueName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting event instance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Method to display RSVPs for the selected event instance
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventDate"></param>
        /// <param name="venueName"></param>
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

                                // Store event info as an object 
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

        /// <summary>
        /// Called every time the visibility of the control changes (e.g., when brought back to view)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManageEventControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                // Refresh data
                listViewEvents.Items.Clear();
                listViewRSVP.Items.Clear();

                // Refresh the ComboBox events list
                comboBoxEventList.SelectedIndex = -1; 

                //refresh event data and RSVPs if needed
                if (comboBoxEventList.SelectedIndex > 0)
                {
                    DisplayEvents(comboBoxEventList.SelectedItem.ToString());
                }

            }
        }

        /// <summary>
        /// Event handler for the Delete Event Instance button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Event handler for the ListViewRSVP click event to handle RSVP deletion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    Delete(3);
                }
            }
        }

        /// <summary>
        /// Performs deletion operations based on the specified type.
        /// Uses transactions to ensure data integrity.
        /// </summary>
        /// <param name="num">Deletion type: 1 = entire event, 2 = event instance, 3 = single RSVP</param>
        private void Delete(int num)
        {
            try
            {

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

                int attendeeId = 0;
                string venueName = "";
                DateTime eventDate = DateTime.MinValue;
                string eventName = comboBoxEventList.SelectedItem.ToString();

                //event instance or single rsvp
                if (num == 2 || num == 3)
                {
                    var selectedItem = listViewEvents.SelectedItems[0];
                    var tag = (dynamic)selectedItem.Tag;
                    eventDate = tag.EventDateTime;
                    venueName = tag.VenueName;
                }

                //single rsvp
                if (num == 3)
                {
                    var selectedRSVP = listViewRSVP.SelectedItems[0];
                    string email = selectedRSVP.SubItems[1].Text;
                    attendeeId = GetAttendeeIdFromRSVP(email, eventName, eventDate, venueName);
                }

                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            switch (num)
                            {
                                case 3:
                                    DeleteSingleRSVP(conn, attendeeId, eventName, venueName, eventDate);
                                    break;

                                case 2:
                                    DeleteEventInstance(conn, eventName, venueName, eventDate);
                                    break;

                                case 1:
                                    DeleteEntireEvent(conn, eventName);
                                    break;
                            }
                            transaction.Commit();
                            MessageBox.Show("Delete successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshUI();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during deletion: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Retrieves the attendee ID for a specific RSVP based on email and event details.
        /// </summary>
        /// <param name="email">The email address of the attendee.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventDate">The date of the event instance.</param>
        /// <param name="venueName">The venue name of the event instance.</param>
        /// <returns>The attendee ID, or -1 if not found.</returns>
        private int GetAttendeeIdFromRSVP(string email, string eventName, DateTime eventDate, string venueName)
        {
            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            SELECT r.acc_num 
                            FROM RSVP r 
                            JOIN Member m ON r.acc_num = m.acc_num 
                            WHERE m.email = :email 
                            AND r.ename = :eventName 
                            AND r.event_date = :eventDate 
                            AND r.vname = :venueName";

                        cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email;
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                        cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;
                        cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;

                        var result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding attendee: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        /// <summary>
        /// Deletes all instances of an event, including all related RSVPs, reviews, and categories in the correct order
        /// then finally delete the entire event
        /// </summary>
        /// <param name="conn"> the connection</param>
        /// <param name="eventName"> the event to be deleted</param>
        private void DeleteEntireEvent(OracleConnection conn, string eventName)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Reviews WHERE ename = :eventName";
                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                cmd.ExecuteNonQuery();
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM RSVP WHERE ename = :eventName";
                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                cmd.ExecuteNonQuery();
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM event_instance WHERE ename = :eventName";
                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                cmd.ExecuteNonQuery();
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM event_category WHERE ename = :eventName";
                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                cmd.ExecuteNonQuery();
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM event WHERE ename = :eventName";
                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes a single RSVP record for a specific attendee and event instance.
        /// </summary>
        /// <param name="conn">The database connection.</param>
        /// <param name="attendeeId">The ID of the attendee whose RSVP to delete.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="venueName">The venue name of the event instance.</param>
        /// <param name="eventDate">The date of the event instance.</param>
        private void DeleteSingleRSVP(OracleConnection conn, int attendeeId, string eventName, string venueName, DateTime eventDate)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    DELETE FROM RSVP
                    WHERE acc_num = :attendeeId
                    AND ename = :eventName
                    AND vname = :venueName
                    AND event_date = :eventDate";
                    
                cmd.Parameters.Add("attendeeId", OracleDbType.Int32).Value = attendeeId;
                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;
                
                cmd.ExecuteNonQuery();
            }
        }

         /// <summary>
        /// Deletes a specific event instance and all its related data in the correct order.
        /// Deletes: Reviews → RSVPs → Event Instance
        /// </summary>
        /// <param name="conn">The database connection.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="venueName">The venue name of the event instance.</param>
        /// <param name="eventDate">The date of the event instance.</param>
        private void DeleteEventInstance(OracleConnection conn, string eventName, string venueName, DateTime eventDate)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    DELETE FROM Reviews
                    WHERE ename = :eventName
                    AND vname = :venueName
                    AND event_date = :eventDate";
                    
                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;
                
                cmd.ExecuteNonQuery();
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    DELETE FROM RSVP
                    WHERE ename = :eventName
                    AND vname = :venueName
                    AND event_date = :eventDate";
                    
                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;
                
                cmd.ExecuteNonQuery();
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    DELETE FROM event_instance
                    WHERE ename = :eventName
                    AND vname = :venueName
                    AND event_date = :eventDate";
                    
                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;
                
                cmd.ExecuteNonQuery();
            }
        }

        //refreshes the UI elements after a deletion operation
        private void RefreshUI()
        {
            listViewEvents.Items.Clear();
            listViewRSVP.Items.Clear();
            comboBoxEventList.SelectedIndex = -1;  
        }

        // Event handler for the Delete Event button click
        private void buttonDeleteEvent_Click(object sender, EventArgs e)
        {
            Delete(1);
        }
    }
}
