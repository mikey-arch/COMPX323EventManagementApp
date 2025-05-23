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
using Oracle.ManagedDataAccess.Client;

namespace COMPX323EventManagementApp
{
    public partial class ProfileControl : UserControl
    {
        String currentlySelected;
        public ProfileControl()
        {
            InitializeComponent();

            Member user = Session.CurrentUser;

            labelAccountNum.Text = user.Id.ToString();
            labelName.Text = user.Fname + " " + user.Lname;
            labelEmail.Text = user.Email;
            currentlySelected = "";

            this.VisibleChanged += ProfileControl_VisibleChanged;

        }

        //displays users rsvps / upcoming events
        private void buttonRsvps_Click(object sender, EventArgs e)
        {
            currentlySelected = "RSVP";
            try
            {
                //clear and set up list view columns for RSVPS
                listViewDisplay.Items.Clear();
                listViewDisplay.Columns.Clear();
                listViewDisplay.View = View.Details;
                listViewDisplay.FullRowSelect = true;
                
                listViewDisplay.Columns.Add("Event", 180);
                listViewDisplay.Columns.Add("Date", 100);
                listViewDisplay.Columns.Add("Venue", 150);
                listViewDisplay.Columns.Add("Status", 80);

                using(var conn = DbConfig.GetConnection()){
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"select e.ename, r.event_date, r.vname, r.status from RSVP r
                        join Event e ON r.ename = e.ename
                        where r.acc_num = :userId and r.event_date > SYSDATE
                        order by r.event_date DESC";
                
                        cmd.Parameters.Add("userId", Oracle.ManagedDataAccess.Client.OracleDbType.Int32).Value = Session.CurrentUser.Id;
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            bool foundRSVPs = false;
                            
                            while (reader.Read())
                            {
                                foundRSVPs = true;
                                
                                // Create list view item for each RSVP
                                ListViewItem item = new ListViewItem(reader["ename"].ToString());
                                item.SubItems.Add(Convert.ToDateTime(reader["event_date"]).ToString("dd-MM-yyyy"));
                                item.SubItems.Add(reader["vname"].ToString());
                                item.SubItems.Add(reader["status"].ToString());
                                
                                listViewDisplay.Items.Add(item);
                            }
                            
                            // Show message if no RSVPs found
                            if (!foundRSVPs)
                            {
                                ListViewItem noDataItem = new ListViewItem("No upcoming events found");
                                listViewDisplay.Items.Add(noDataItem);
                            }
                        }
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "DataBase Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        //displays users reviews
        private void buttonReviews_Click(object sender, EventArgs e)
        {
            currentlySelected = "Reviews";
            try
            {
                // clear listbox and setup list view columns for reviews
                listViewDisplay.Items.Clear();
                listViewDisplay.Columns.Clear();
                listViewDisplay.View = View.Details;
                listViewDisplay.FullRowSelect = true;
                
                listViewDisplay.Columns.Add("Event", 180);
                listViewDisplay.Columns.Add("Venue", 150);
                listViewDisplay.Columns.Add("Date", 100);
                listViewDisplay.Columns.Add("Rating", 50);
                listViewDisplay.Columns.Add("Review", 300);
                
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        // Query to get user's reviews
                        cmd.CommandText = @"select r.ename, r.vname, r.event_date, r.rating, r.text_review from Reviews r
                            where r.acc_num = :userId
                            order by r.review_timestamp DESC";
                        
                        cmd.Parameters.Add("userId", Oracle.ManagedDataAccess.Client.OracleDbType.Int32).Value = Session.CurrentUser.Id;
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            bool foundReviews = false;
                            
                            while (reader.Read())
                            {
                                foundReviews = true;
                                
                                // Create list view item for each review
                                ListViewItem item = new ListViewItem(reader["ename"].ToString());
                                item.SubItems.Add(reader["vname"].ToString());
                                item.SubItems.Add(Convert.ToDateTime(reader["event_date"]).ToString("dd-MM-yyyy"));
                                item.SubItems.Add(reader["rating"].ToString());
                                item.SubItems.Add(reader["text_review"].ToString());
                                
                                listViewDisplay.Items.Add(item);
                            }
                            
                            // message if no reviews found
                            if (!foundReviews)
                            {
                                ListViewItem noDataItem = new ListViewItem("No reviews submitted yet");
                                listViewDisplay.Items.Add(noDataItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //displays users organised events
        private void buttonEvents_Click(object sender, EventArgs e)
        {
            currentlySelected = "Events";
            try
            {
                // clear listview and setup columns
                listViewDisplay.Items.Clear();
                listViewDisplay.Columns.Clear();
                listViewDisplay.View = View.Details;
                listViewDisplay.FullRowSelect = true;
                
                listViewDisplay.Columns.Add("Event Name", 180);
                listViewDisplay.Columns.Add("Description", 250);
                listViewDisplay.Columns.Add("Creation Date", 100);
                
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        //query to get events organised by the user
                        cmd.CommandText = @"select ename, description, creation_date from Event
                                            where creator_num = :userId
                                            order by creation_date desc";
                        
                        cmd.Parameters.Add("userId", Oracle.ManagedDataAccess.Client.OracleDbType.Int32).Value = Session.CurrentUser.Id;
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            bool foundEvents = false;
                            
                            while (reader.Read())
                            {
                                foundEvents = true;
                                
                                // Create list view item for each event
                                ListViewItem item = new ListViewItem(reader["ename"].ToString());
                                
                                // trim long descriptions
                                string description = reader["description"].ToString();
                                if (description.Length > 100)
                                    description = description.Substring(0, 97) + "...";
                                
                                item.SubItems.Add(description);
                                item.SubItems.Add(Convert.ToDateTime(reader["creation_date"]).ToString("dd-MM-yyyy"));
                                
                                listViewDisplay.Items.Add(item);
                            }
                            
                            //if no events found display nothing found
                            if (!foundEvents)
                            {
                                ListViewItem noDataItem = new ListViewItem("No events organised yet");
                                listViewDisplay.Items.Add(noDataItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void listViewDisplay_DoubleClick(object sender, EventArgs e)
        {

            
            if (listViewDisplay.SelectedItems.Count == 0 || currentlySelected != "RSVP")
                return;

            var selectedEvent = listViewDisplay.SelectedItems[0];
            string eventName = selectedEvent.Text;
            DateTime eventDate = DateTime.Parse(selectedEvent.SubItems[1].Text);
            string venueName = selectedEvent.SubItems[2].Text;


            // Show custom message box
            CustomMessageBox customMsgBox = new CustomMessageBox();
            customMsgBox.ShowDialog();

            if (customMsgBox.SelectedOption == "Delete RSVP")
            {
                DeleteRsvp(eventName, eventDate, venueName);
                buttonRsvps_Click(sender, e);
            }
            else if (customMsgBox.SelectedOption == "View Event Details")
            {
                EventDetails eventDetailsForm = new EventDetails(eventName, eventDate, venueName);
                eventDetailsForm.FormClosed += (s, args) => buttonRsvps_Click(s, args);
                eventDetailsForm.Show();
            }

           
        }

        private void DeleteRsvp(string eventName, DateTime eventDate, string venueName)
        {
            try
            {
                // Get the attendee ID from the current user session
                Member user = Session.CurrentUser;
                int attendeeId = user.Id;

                // SQL query to delete the RSVP record from the database
                string deleteQuery = @"
                    DELETE FROM RSVP
                    WHERE acc_num = :attendeeId
                    AND ename = :eventName
                    AND vname = :venueName
                    AND event_date = :eventDate";

                // Execute the delete query
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = deleteQuery;

                        // Add parameters for deletion
                        cmd.Parameters.Add("attendeeId", Oracle.ManagedDataAccess.Client.OracleDbType.Int32).Value = attendeeId;
                        cmd.Parameters.Add("eventName", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2).Value = eventName;
                        cmd.Parameters.Add("venueName", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2).Value = venueName;
                        cmd.Parameters.Add("eventDate", Oracle.ManagedDataAccess.Client.OracleDbType.Date).Value = eventDate;

                        // Execute the delete command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected < 0)
                        {
                            MessageBox.Show("No matching RSVP found to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting RSVP: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // refreshes when specific view is currently selected
        private void ProfileControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (currentlySelected == "RSVP")
                {
                    buttonRsvps_Click(null, null);
                }
                else if (currentlySelected == "Reviews")
                {
                    buttonReviews_Click(null, null);
                }
                else if (currentlySelected == "Events")
                {
                    buttonEvents_Click(null, null);
                }
            }
        }
    }
}
