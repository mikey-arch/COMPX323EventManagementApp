using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;



namespace COMPX323EventManagementApp
{
    public partial class LoginForm : Form
    {

        public LoginForm()
        {
            InitializeComponent();

        }

        public static class DbConfig
        {
            // a single place to update if/when your string changes
            public static string oradb = "Data Source=localhost:1521/xe;User Id=SYSTEM;Password=lucaszin13;";
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                
                using (var conn = new OracleConnection(DbConfig.oradb))
                {
                    conn.Open();
                    // If want to test the connection is working
                    /*
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT password FROM attendee WHERE acc_num = 1";

                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                // only now do we assign the textbox
                                textBoxUsername.Text = dr.GetString(0);
                            }
                            else
                            {
                                MessageBox.Show("No attendee found with acc_num = 1");
                            }
                        }
                    }
                    */
                    conn.Close();
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            
            //first if the login is validated 

            //then create the instance of the events manager form
            EventsManagerForm eventsManagerForm = new EventsManagerForm();
            this.Hide();
            eventsManagerForm.ShowDialog();
            this.Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxUsername.Clear();
            textBoxPassword.Clear();
            textBoxUsername.Focus();

        }

        private void label5_Click(object sender, EventArgs e)
        {
            new RegisterForm().Show();
            this.Hide();
        }
    }
}
