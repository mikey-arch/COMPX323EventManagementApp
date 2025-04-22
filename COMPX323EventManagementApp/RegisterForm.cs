using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using static COMPX323EventManagementApp.LoginForm;

namespace COMPX323EventManagementApp
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            
        }
        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string email = textBoxEmail.Text;
            string phoneNum = textBoxPhoneNumber.Text;
            string dateOfBirth = textBoxBirthday.Text;
            string password = textBoxPassword.Text;
            string confirmPassword = textBoxConfirmPassword.Text;

  
            // 1) Read & validate your inputs
            var pwd = textBoxPassword.Text;
            var confirm = textBoxConfirmPassword.Text;
            if (pwd != confirm)
            {
                MessageBox.Show("Passwords don’t match.");
                return;
            }

            if (!DateTime.TryParseExact(
                textBoxBirthday.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime dob))
            {
                MessageBox.Show(
                  "Please enter your birthday in the format YYYY-MM-DD, e.g. 2004-10-01.");
                return;
            }



            // 2) Build & execute a parameterized INSERT
            const string sql = @"INSERT INTO Attendee (password, mob_num, fname, lname, email, DOB, payment_status) 
                                VALUES(:pwd, :mob, :fname, :lname, :email, :dob, :status)";

            using (var conn = new OracleConnection(DbConfig.oradb))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;

                // bind parameters
                cmd.Parameters.Add("pwd", OracleDbType.Varchar2).Value = pwd;
                cmd.Parameters.Add("mob", OracleDbType.Varchar2).Value = textBoxPhoneNumber.Text.Trim();
                cmd.Parameters.Add("fname", OracleDbType.Varchar2).Value = textBoxUsername.Text.Trim();
                cmd.Parameters.Add("lname", OracleDbType.Varchar2).Value = textBoxUsername.Text.Trim();
                cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = textBoxEmail.Text.Trim();
                cmd.Parameters.Add("dob", OracleDbType.Date).Value = dob;
                cmd.Parameters.Add("status", OracleDbType.Varchar2).Value = "up_to_date";

                conn.Open();
                int inserted = cmd.ExecuteNonQuery();  // returns #rows affected

                MessageBox.Show(
                    inserted == 1
                      ? "Registration successful!"
                      : "Oops—no rows inserted.");
            }
        }


        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxUsername.Clear();
            textBoxEmail.Clear();
            textBoxPhoneNumber.Clear();
            textBoxBirthday.Clear();
            textBoxPassword.Clear();
            textBoxConfirmPassword.Clear();
            textBoxUsername.Focus();
        }

        private void labelLogin_Click(object sender, EventArgs e)
        {
            new LoginForm().Show();
            this.Hide();
        }
    }
}
