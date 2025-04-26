using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using COMPX323EventManagementApp.Properties;
using Oracle.ManagedDataAccess.Client;

namespace COMPX323EventManagementApp
{
    public partial class CreateReviewControl : UserControl
    {
        private int rating = 1;

        public CreateReviewControl()
        {
            InitializeComponent();
        }

        private void textBoxReview_Enter(object sender, EventArgs e)
        {
            if(textBoxReview.Text == "Enter text here...")
            {
                textBoxReview.Text = "";
                textBoxReview.ForeColor = Color.Black;
            }

        }

        private void textBoxReview_Leave(object sender, EventArgs e)
        {
            if(textBoxReview.Text == "")
            {
                textBoxReview.Text = "Enter text here...";
                textBoxReview.ForeColor = Color.Gray;
            }

        }

        private void pictureBoxStar1_Click(object sender, EventArgs e)
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_white;
            pictureBoxStar3.Image = Resources.star_64_white;
            pictureBoxStar4.Image = Resources.star_64_white;
            pictureBoxStar5.Image = Resources.star_64_white;
            rating = 1;

        }

        private void pictureBoxStar2_Click(object sender, EventArgs e)
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_gold;
            pictureBoxStar3.Image = Resources.star_64_white;
            pictureBoxStar4.Image = Resources.star_64_white;
            pictureBoxStar5.Image = Resources.star_64_white;
            rating = 2;
        }

        private void pictureBoxStar3_Click(object sender, EventArgs e)
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_gold;
            pictureBoxStar3.Image = Resources.star_64_gold;
            pictureBoxStar4.Image = Resources.star_64_white;
            pictureBoxStar5.Image = Resources.star_64_white;
            rating = 3;
        }

        private void pictureBoxStar4_Click(object sender, EventArgs e)
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_gold;
            pictureBoxStar3.Image = Resources.star_64_gold;
            pictureBoxStar4.Image = Resources.star_64_gold;
            pictureBoxStar5.Image = Resources.star_64_white;
            rating = 4;
        }

        private void pictureBoxStar5_Click(object sender, EventArgs e)
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_gold;
            pictureBoxStar3.Image = Resources.star_64_gold;
            pictureBoxStar4.Image = Resources.star_64_gold;
            pictureBoxStar5.Image = Resources.star_64_gold;
            rating = 5;
        }

        private void buttonSubmitReview_Click(object sender, EventArgs e)
        {
            int rate = rating;

            if(string.IsNullOrWhiteSpace(textBoxReview.Text) || textBoxReview.Text == "Enter text here...")
            {
                MessageBox.Show("Please enter a review.");
                return;
            }

            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        //need to get the acc_num of the logged in user, get proper values from the actual event and venue sessions
                        cmd.CommandText = @"
                            INSERT INTO Reviews (acc_num, ename, vname, event_date, rating, text_review)
                            VALUES (:acc_num, :ename, :vname, :event_date, :rating, :text_review)";
                        
                        cmd.Parameters.Add(":acc_num", OracleDbType.Int32).Value = 1; 
                        cmd.Parameters.Add(":ename", OracleDbType.Varchar2).Value = "Roblox Olympics"; 
                        cmd.Parameters.Add(":vname", OracleDbType.Varchar2).Value = "Royal Albert Hall"; 
                        cmd.Parameters.Add(":event_date", OracleDbType.Date).Value = new DateTime(2025, 6, 2);
                        cmd.Parameters.Add(":rating", OracleDbType.Int32).Value = rating;
                        cmd.Parameters.Add(":text_review", OracleDbType.Clob).Value = textBoxReview.Text;

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Thank you for the Review", "Review Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // maybe return back to profle?
                    //reset text box then reset rating to 1
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
