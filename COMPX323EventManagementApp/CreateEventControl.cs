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
        public CreateEventControl()
        {
            InitializeComponent();

            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();

                    // Search for events and display all events details when form opens
                    using (var cmd = conn.CreateCommand())
                    {

                        cmd.CommandText = "select cname, description, icon from category_tag";

                        using (var reader = cmd.ExecuteReader())
                        {
                            // Populate category comboBox
                            comboBoxCategory.Items.Clear();
                            

                            // Add items to comboBox
                            while (reader.Read())
                            {
                                comboBoxCategory.Items.Add(reader.GetString(0));
                                
                            }
                        }

                        cmd.CommandText = "select rname, description from restrictions";

                        using (var reader = cmd.ExecuteReader())
                        {
                            // Populate restrictions comboBox
                            comboBoxRestrictions.Items.Clear();


                            // Add items to comboBox
                            while (reader.Read())
                            {
                                comboBoxRestrictions.Items.Add(reader.GetString(0));

                            }
                        }

                        cmd.CommandText = "select vname, capacity, street_num, street_name, suburb, city, postcode, country from venue";

                        using (var reader = cmd.ExecuteReader())
                        {
                            // Populate restrictions comboBox
                            comboBoxLocation.Items.Clear();


                            // Add items to comboBox
                            while (reader.Read())
                            {
                                comboBoxLocation.Items.Add(reader.GetString(0));

                            }
                        }

                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCreateEvent_Click(object sender, EventArgs e)
        {
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
    }
}
