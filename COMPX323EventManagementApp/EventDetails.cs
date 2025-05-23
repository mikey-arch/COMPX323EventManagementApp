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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;


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
            Console.WriteLine($"eventDate1: {eventDate:dd-MM-YYYY}");

            Member user = Session.CurrentUser;
            attendeeId = user.Id;
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
                    labelEName.Text = eventName;

                    // Query to get basic event details (description, time, price)
                    string query = @"select description, time, price, restriction, vname from event e 
                                join event_category ec on ec.ename = e.ename
                                join event_instance ei on ei.ename = e.ename
                                WHERE 
                                e.ename = :eventName
                                AND TRUNC(ei.time) = :eventDate 
                                AND ei.vname = :venueName";

                    string tagQuery = @"
                                SELECT cname
                                FROM event_category
                                WHERE ename = :eventName";


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
                                textBoxDesc.Text = reader.GetString(0);
                                DateTime dateTime = reader.GetDateTime(1); 
                                textBoxTime.Text = dateTime.ToString("HH:mm");
                                textBoxDate.Text = dateTime.ToString("dd/MM/yy");
                                textBoxPrice.Text = reader.GetDecimal(2).ToString("C");
                                textBoxRestrictions.Text = reader.GetString(3);
                                textBoxVenue.Text = reader.GetString(4);

                            }
                            
                            
                        }
                    }
                    using (var tagCmd = conn.CreateCommand())
                    {
                        tagCmd.CommandText = tagQuery;
                        tagCmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;

                        using (var tagReader = tagCmd.ExecuteReader())
                        {
                            List<string> tags = new List<string>();
                            while (tagReader.Read())
                            {
                                tags.Add(tagReader.GetString(0));
                            }

                            // Join the tags into a CSV format
                            textBoxTags.Text = string.Join(", ", tags);
                        }
                    }


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
