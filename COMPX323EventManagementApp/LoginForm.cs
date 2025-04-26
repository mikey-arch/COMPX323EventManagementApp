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
            string email = textBoxEmail.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            // if no email or password is entered, prompt user
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your username and password.");
                return;
            }

            try
            {
                bool authenticated = false;

                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();

                    // log in check using Attendee table first 
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Select acc_num, fname, lname, mob_num, email, dob, payment_status from Attendee where email = :email and password = :password";
                        cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email;
                        cmd.Parameters.Add("password", OracleDbType.Varchar2).Value = password;

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                authenticated = true;
                                int userId = reader.GetInt32(0);
                                string firstName = reader.GetString(1);
                                string lastName = reader.GetString(2);
                                string phoneNum = reader.GetString(3);
                                email = reader.GetString(4);
                                DateTime dob = reader.GetDateTime(5);
                                string paymentStatus = reader.GetString(6);


                                // 2) Map your DB record to your domain model
                                var me = new User
                                {
                                    Id = userId,
                                    Fname = firstName,
                                    Lname = lastName,
                                    Email = email,
                                    PhoneNum = phoneNum,
                                    Dob = dob,
                                    PaymentStatus = paymentStatus

                                };

                                // 3) Store it in the Session
                                Session.CurrentUser = me;

                            }
                        }
                    }

                    // check organiser table
                    if(!authenticated)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "Select acc_num, fname from Organiser where email = :email and password = :password";
                            cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email;
                            cmd.Parameters.Add("password", OracleDbType.Varchar2).Value = password;

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    authenticated = true;
                                    int userId = reader.GetInt32(0);
                                    string firstName = reader.GetString(1);
                                }
                            }
                        }
                    }

                    // if wrong username and password
                    if(!authenticated)
                    {
                        MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if(authenticated)
                {
                    //then create the instance of the events manager form
                    EventsManagerForm eventsManagerForm = new EventsManagerForm();
                    this.Hide();
                    eventsManagerForm.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Handles the clear button click event. It clears the username and password fields then focuses on username textbox.
        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxEmail.Clear();
            textBoxPassword.Clear();
            textBoxEmail.Focus();
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

        // Toggles showing and hiding the password
        private void checkBoxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !checkBoxShowPass.Checked;
        }
    }
}
