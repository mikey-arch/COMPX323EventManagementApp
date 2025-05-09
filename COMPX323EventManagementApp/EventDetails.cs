using COMPX323EventManagementApp.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace COMPX323EventManagementApp
{
    public partial class EventDetails : Form
    {
        private string eventName;
        private DateTime eventDate;
        private string venueName;
        private int attendeeId; 

        // Constructor accepting event details (composite primary key)
        public EventDetails(string eventName, DateTime eventDate, string venueName)
        {
            InitializeComponent();
            this.eventName = eventName;
            this.eventDate = eventDate;
            this.venueName = venueName;


            Member user = Session.CurrentUser;
            attendeeId = user.Id;
            //labelAccNum.Text = attendeeId.ToString();
            //labelName.Text = user.Fname + " " + user.Lname; 
            LoadEventDetails();
        }

        // Load event details based on the passed values
        public void LoadEventDetails()
        {
            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();

                    // Query to get basic event details (description, time, price)
                    string query = @"
                        SELECT 
                            e.description AS eventDescription, 
                            ei.time AS eventTime, 
                            ei.price AS eventPrice
                        FROM 
                            event e
                            JOIN event_instance ei ON e.ename = ei.ename
                        WHERE 
                            e.ename = :eventName 
                            AND ei.event_date = :eventDate 
                            AND ei.vname = :venueName";

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;

                        // Pass parameters for eventName, eventDate, and venueName
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                        cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;
                        cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Set the event details into the form controls
                                textBoxDesc.Text = reader.GetString(reader.GetOrdinal("eventDescription"));
                                textBoxTime.Text = reader.GetDateTime(reader.GetOrdinal("eventTime")).ToString("HH:mm");
                                textBoxPrice.Text = reader.GetDecimal(reader.GetOrdinal("eventPrice")).ToString("C");
                            }
                        }
                    }

                    // Retrieve categories for the event
                    StringBuilder categories = new StringBuilder();
                    string categoryQuery = @"
                        SELECT ct.cname AS categoryName
                        FROM 
                            event e
                            JOIN has_a ha ON e.ename = ha.ename
                            JOIN category_tag ct ON ha.cname = ct.cname
                        WHERE 
                            e.ename = :eventName";

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = categoryQuery;
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("categoryName")))
                                {
                                    categories.Append(reader.GetString(reader.GetOrdinal("categoryName")) + ", ");
                                }
                            }
                        }
                    }
                    textBoxTags.Text = categories.ToString().TrimEnd(new char[] { ',', ' ' });

                    // Display eventName, venueName, and eventDate in the form
                    labelEventName.Text = eventName;
                    textBoxVenue.Text = venueName;
                    textBoxDate.Text = eventDate.ToString("dd-MM-yyyy");

                    // Retrieve restrictions for the event
                    StringBuilder restrictions = new StringBuilder();
                    string restrictionQuery = @"
                        SELECT r.rname AS restrictionName
                        FROM 
                            event e
                            JOIN has h ON e.ename = h.ename
                            JOIN restrictions r ON h.rname = r.rname
                        WHERE 
                            e.ename = :eventName";

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = restrictionQuery;
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("restrictionName")))
                                {
                                    restrictions.Append(reader.GetString(reader.GetOrdinal("restrictionName")) + ", ");
                                }
                            }
                        }
                    }
                    textBoxRestrictions.Text = restrictions.ToString().TrimEnd(new char[] { ',', ' ' });

                    // Query to get the current RSVP status for the attendee
                    string rsvpQuery = @"
                        SELECT status
                        FROM rsvp
                        WHERE acc_num = :attendeeId
                        AND ename = :eventName
                        AND vname = :venueName
                        AND event_date = :eventDate";

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = rsvpQuery;
                        cmd.Parameters.Add("attendeeId", OracleDbType.Int32).Value = attendeeId;
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                        cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                        cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string status = reader.GetString(reader.GetOrdinal("status"));
                                if (status == "attending")
                                {
                                    radioButtonAttending.Checked = true;
                                }
                                else if (status == "on hold")
                                {
                                    radioButtonInterested.Checked = true;
                                }
                                labelStatus.Text = status;


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading event details: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSubmitRSVP_Click(object sender, EventArgs e)
        {
            
                try
                {
                    string selectedStatus = "";

                    if (radioButtonAttending.Checked)
                    {
                        selectedStatus = "attending";
                    }
                    else if (radioButtonInterested.Checked)
                    {
                        selectedStatus = "on hold";
                    }
                    else
                    {
                        // If no valid option is selected, exit early
                        MessageBox.Show("Please select an RSVP status.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    labelStatus.Text = selectedStatus;

                    // Check if the user already has an RSVP entry for the event
                    string checkRsvpQuery = @"
                        SELECT COUNT(*) 
                        FROM rsvp
                        WHERE acc_num = :attendeeId
                        AND ename = :eventName
                        AND vname = :venueName
                        AND event_date = :eventDate";

                    using (var conn = DbConfig.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = checkRsvpQuery;
                            cmd.Parameters.Add("attendeeId", OracleDbType.Int32).Value = attendeeId;
                            cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                            cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                            cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;

                            int count = Convert.ToInt32(cmd.ExecuteScalar());

                            if (count > 0)
                            {
                                // If RSVP exists, update the status
                                string updateQuery = @"
                                    UPDATE rsvp
                                    SET status = :status
                                    WHERE acc_num = :attendeeId
                                    AND ename = :eventName
                                    AND vname = :venueName
                                    AND event_date = :eventDate";

                                using (var updateCmd = conn.CreateCommand())
                                {
                                    updateCmd.CommandText = updateQuery;
                                    updateCmd.Parameters.Add("status", OracleDbType.Varchar2).Value = selectedStatus;
                                    updateCmd.Parameters.Add("attendeeId", OracleDbType.Int32).Value = attendeeId;
                                    updateCmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                                    updateCmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                                    updateCmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;

                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // If no RSVP exists, insert a new entry
                                string insertQuery = @"
                                    INSERT INTO rsvp (acc_num, ename, vname, event_date, status)
                                    VALUES (:attendeeId, :eventName, :venueName, :eventDate, :status)";

                                using (var insertCmd = conn.CreateCommand())
                                {
                                    insertCmd.CommandText = insertQuery;
                                    insertCmd.Parameters.Add("attendeeId", OracleDbType.Int32).Value = attendeeId;
                                    insertCmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                                    insertCmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                                    insertCmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;
                                    insertCmd.Parameters.Add("status", OracleDbType.Varchar2).Value = selectedStatus;

                                    insertCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating RSVP status: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

        }

        private void labelExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
