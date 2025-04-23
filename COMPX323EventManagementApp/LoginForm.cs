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
    /// <summary>
    /// This class represents the login form for the event management application handling authenticantion and navigation to main dashboard.
    /// </summary>
    public partial class LoginForm : Form
    {

        public LoginForm()
        {
            InitializeComponent();

        }

        // Handles the login button click event. It validates the user credentials and opens the EventsManagerForm if successful.
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (OracleConnection conn = DbConfig.GetConnection())
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

        // Handles the clear button click event. It clears the username and password fields then focuses on username textbox.
        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxUsername.Clear();
            textBoxPassword.Clear();
            textBoxUsername.Focus();
        }

        // Handles the register label click event. It opens the RegisterForm and hides the current form.
        private void label5_Click(object sender, EventArgs e)
        {
            new RegisterForm().Show();
            this.Hide();
        }

        // Exit label, exits application once clicked.
        private void labelExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
