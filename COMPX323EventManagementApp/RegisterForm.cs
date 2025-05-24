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
using MongoDB.Bson;
using Oracle.ManagedDataAccess.Client;
using static COMPX323EventManagementApp.LoginForm;

namespace COMPX323EventManagementApp
{
    /// <summary>
    /// This class represents the registration form for new users to create an account.
    /// </summary>
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        // Registers a new user by inserting their details into the database once the Register button is clicked.
        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string firstName = textBoxFirstName.Text.Trim();
            string lastName = textBoxLastName.Text.Trim();
            string email = textBoxEmail.Text.Trim();
            string phoneNum = maskedTextBoxPhoneNumber.Text.Replace("-", "");
            DateTime dob = dateTimePickerBirthday.Value;
            string password = textBoxPassword.Text;
            string confirmPassword = textBoxConfirmPassword.Text;

            if (!ValidateUserInputs(firstName, lastName, email, phoneNum, dob, password, confirmPassword)) return;

            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    // Check if the email already exists in the database
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select count(*) from Member where email = :email";
                        cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email;

                        using (var reader = cmd.ExecuteReader())
                        {
                            int totalCount = 0;
                            while (reader.Read())
                            {
                                totalCount += Convert.ToInt32(reader.GetValue(0));
                            }

                            if (totalCount > 0)
                            {
                                MessageBox.Show("Email already exists. Please use a different email.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    //register the user into member table
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"insert into Member (password, mob_num, fname, lname, email, DOB) values (:pwd, :mob, :fname, :lname, :email, :dob)";
                        cmd.Parameters.Add("pwd", OracleDbType.Varchar2).Value = password;
                        cmd.Parameters.Add("mob", OracleDbType.Varchar2).Value = phoneNum;
                        cmd.Parameters.Add("fname", OracleDbType.Varchar2).Value = firstName;
                        cmd.Parameters.Add("lname", OracleDbType.Varchar2).Value = lastName;
                        cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email;
                        cmd.Parameters.Add("dob", OracleDbType.Date).Value = dob;

                        int memberInserted = cmd.ExecuteNonQuery();
                        if (memberInserted != 1)
                        {
                            MessageBox.Show("Failed to register as member.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Get the newly created user's ID from Oracle and add the same user to MongoDB
                    int newUserId = 0;
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select acc_num from Member where email = :email";
                        cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email;
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                newUserId = reader.GetInt32(0);
                            }
                        }
                    }

                    if (newUserId > 0)
                    {
                        RegisterUserToMongoDB(newUserId, firstName, lastName, email, phoneNum, dob, password);
                    }

                    MessageBox.Show("Registration successful! You can now login with your email and password.", "Registration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                    // Return to login form
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Synchronizes newly created user to MongoDB
        /// </summary>
        private void RegisterUserToMongoDB(int userId, string firstName, string lastName, string email, string phoneNum, DateTime dob, string password)
        {
            try
            {
                var membersCollection = MongoDbConfig.GetCollection<BsonDocument>("members");
                
                var mongoMember = new BsonDocument
                {
                    {"_id", userId},
                    {"password", password},
                    {"mobNum", phoneNum},
                    {"fname", firstName},
                    {"lname", lastName},
                    {"email", email},
                    {"dob", dob}
                };

                membersCollection.InsertOne(mongoMember);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mongo Database Error: {ex.Message}", "Mongo Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        /// <summary>
        /// Validates all user input fields
        /// </summary>
        /// <returns>True if all validation passes, false otherwise</returns>
        private bool ValidateUserInputs(string firstName, string lastName, string email, string phoneNum, DateTime dob, string password, string confirmPassword)
        {
            // Basic empty field validation
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all fields.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // First name validation - only letters
            if (!System.Text.RegularExpressions.Regex.IsMatch(firstName, "^[a-zA-Z]+$"))
            {
                MessageBox.Show("First name can only contain letters.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxFirstName.Focus();
                return false;
            }

            // Last name validation - only letters
            if (!System.Text.RegularExpressions.Regex.IsMatch(lastName, "^[a-zA-Z]+$"))
            {
                MessageBox.Show("Last name can only contain letters.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxLastName.Focus();
                return false;
            }

            //  email validation using the same regex pattern as database
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Please enter a valid email address (e.g., user@example.com). Email must contain '@' symbol and a valid domain.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxEmail.Focus();
                return false;
            }

            // Password confirmation validation
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords don't match.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxConfirmPassword.Focus();
                return false;
            }

            // Phone number validation
            if (!phoneNum.StartsWith("02") || phoneNum.Length < 9)
            {
                MessageBox.Show("Please enter a valid NZ phone number starting with 02 and at least 9 digits", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                maskedTextBoxPhoneNumber.Focus();
                return false;
            }

            // Age validation
            DateTime minDate = DateTime.Today.AddYears(-13);
            if (dob > minDate)
            {
                MessageBox.Show("You must be at least 13 years old to register.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePickerBirthday.Focus();
                return false;
            }

            return true;
        }

        // Clears all the text boxes when the Clear button is clicked then focuses on the username text box.
        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxFirstName.Clear();
            textBoxLastName.Clear();
            textBoxEmail.Clear();
            maskedTextBoxPhoneNumber.Clear();
            dateTimePickerBirthday.Value = DateTime.Now;
            textBoxPassword.Clear();
            textBoxConfirmPassword.Clear();
            textBoxFirstName.Focus();
        }

        // Opens the LoginForm when the Login label is clicked and hides the RegisterForm.
        private void labelLogin_Click(object sender, EventArgs e)
        {
            this.DialogResult= DialogResult.Cancel;
            this.Close();
        }

        // Handles the exit label click event. It closes the application. 
        private void labelExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //toggles showing password and confirms password
        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !checkBoxShowPassword.Checked;
            textBoxConfirmPassword.UseSystemPasswordChar= !checkBoxShowPassword.Checked;
        }

        //handles only letter keypresses so can only enter valid firstname and second name
        private void textBoxFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //handles only letter keypresses
        private void textBoxLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
