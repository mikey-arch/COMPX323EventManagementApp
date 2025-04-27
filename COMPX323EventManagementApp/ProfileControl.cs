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
    public partial class ProfileControl : UserControl
    {
        public ProfileControl()
        {
            InitializeComponent();

            User user = Session.CurrentUser;

            labelAccountNum.Text = user.Id.ToString();
            labelName.Text = user.Fname + " " + user.Lname;
            labelEmail.Text = user.Email;
            
        }

        //displays users rsvps / upcoming events
        private void buttonRsvps_Click(object sender, EventArgs e)
        {
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
                        where r.acc_num = :userId
                        order by r.event_date";
                
                        cmd.Parameters.Add("userId", Oracle.ManagedDataAccess.Client.OracleDbType.Int32).Value = Session.CurrentUser.Id;
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            bool foundRSVPs = false;
                            
                            while (reader.Read())
                            {
                                foundRSVPs = true;
                                
                                // Create list view item for each RSVP
                                ListViewItem item = new ListViewItem(reader["ename"].ToString());
                                item.SubItems.Add(Convert.ToDateTime(reader["event_date"]).ToString("yyyy-MM-dd"));
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
                                item.SubItems.Add(Convert.ToDateTime(reader["event_date"]).ToString("yyyy-MM-dd"));
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
            try
            {
                // clear listview and setup columns
                listViewDisplay.Items.Clear();
                listViewDisplay.Columns.Clear();
                listViewDisplay.View = View.Details;
                listViewDisplay.FullRowSelect = true;
                
                listViewDisplay.Columns.Add("Event Name", 180);
                listViewDisplay.Columns.Add("Description", 250);
                listViewDisplay.Columns.Add("Company/Club", 150);
                listViewDisplay.Columns.Add("Creation Date", 100);
                
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        //query to get events organised by the user
                        cmd.CommandText = @"select e.ename, e.description, e.company_club, e.creation_date from Event e
                            join Organises o ON e.ename = o.ename
                            where o.acc_num = :userId
                            order by e.creation_date DESC";
                        
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
                                item.SubItems.Add(reader["company_club"].ToString());
                                item.SubItems.Add(Convert.ToDateTime(reader["creation_date"]).ToString("yyyy-MM-dd"));
                                
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
    }
}
