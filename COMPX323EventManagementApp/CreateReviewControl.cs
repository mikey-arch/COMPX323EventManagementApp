using System;
using System.Drawing;
using System.Windows.Forms;
using COMPX323EventManagementApp.Properties;
using Oracle.ManagedDataAccess.Client;
using COMPX323EventManagementApp.Models;


namespace COMPX323EventManagementApp
{
    public partial class CreateReviewControl : UserControl
    {
        private int rating = 1; //holds star rating defaulted at 1 star

        public CreateReviewControl()
        {
            InitializeComponent();
            LoadUsersRSVPS();
        }

        //populates combobox with rsvps to be reviewed
        private void LoadUsersRSVPS()
        {
            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        //shows rsvps that were already attended , and not already reviewed
                        cmd.CommandText = @"
                        select r.ename , r.vname, r.event_date
                        from RSVP r
                        where r.acc_num = :acc_num
                        and r.status = 'attending'
                        and r.event_date < sysdate
                        and not exists (select 1 from Reviews rev 
                        where rev.acc_num = r.acc_num 
                        and rev.ename = r.ename 
                        and rev.vname = r.vname 
                        and rev.event_date = r.event_date)
                        order by r.event_date desc";

                        cmd.Parameters.Add(":acc_num", OracleDbType.Int32).Value = Session.CurrentUser.Id;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var rsvp = new RSVP
                                {
                                    EName = reader.GetString(0),
                                    VName = reader.GetString(1),
                                    EventDate = reader.GetDateTime(2),
                                };

                                comboBoxEvents.Items.Add(rsvp);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading RSVPs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //placeholder text to point user to where to write review 
        private void textBoxReview_Enter(object sender, EventArgs e)
        {
            if(textBoxReview.Text == "Enter text here...")
            {
                textBoxReview.Text = "";
                textBoxReview.ForeColor = Color.Black;
            }

        }

        //repopulate placeholder text to point user to where to write review when leaving textbox
        private void textBoxReview_Leave(object sender, EventArgs e)
        {
            if(textBoxReview.Text == "")
            {
                textBoxReview.Text = "Enter text here...";
                textBoxReview.ForeColor = Color.Gray;
            }

        }

        //visually changes and updates rating when clicked
        private void pictureBoxStar1_Click(object sender, EventArgs e)
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_white;
            pictureBoxStar3.Image = Resources.star_64_white;
            pictureBoxStar4.Image = Resources.star_64_white;
            pictureBoxStar5.Image = Resources.star_64_white;
            rating = 1;

        }

        //visually changes and updates rating when clicked
        private void pictureBoxStar2_Click(object sender, EventArgs e)
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_gold;
            pictureBoxStar3.Image = Resources.star_64_white;
            pictureBoxStar4.Image = Resources.star_64_white;
            pictureBoxStar5.Image = Resources.star_64_white;
            rating = 2;
        }

        //visually changes and updates rating when clicked
        private void pictureBoxStar3_Click(object sender, EventArgs e)
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_gold;
            pictureBoxStar3.Image = Resources.star_64_gold;
            pictureBoxStar4.Image = Resources.star_64_white;
            pictureBoxStar5.Image = Resources.star_64_white;
            rating = 3;
        }

        //visually changes and updates rating when clicked
        private void pictureBoxStar4_Click(object sender, EventArgs e)
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_gold;
            pictureBoxStar3.Image = Resources.star_64_gold;
            pictureBoxStar4.Image = Resources.star_64_gold;
            pictureBoxStar5.Image = Resources.star_64_white;
            rating = 4;
        }

        //visually changes and updates rating when clicked
        private void pictureBoxStar5_Click(object sender, EventArgs e) 
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_gold;
            pictureBoxStar3.Image = Resources.star_64_gold;
            pictureBoxStar4.Image = Resources.star_64_gold;
            pictureBoxStar5.Image = Resources.star_64_gold;
            rating = 5;
        } 

        //handles submiting review and updating the database with new review and rating
        private void buttonSubmitReview_Click(object sender, EventArgs e)
        {
            int rate = rating;

            if(comboBoxEvents.SelectedItem == null)
            {
                MessageBox.Show("Please select an event to review.");
                return;
            }

            if(string.IsNullOrWhiteSpace(textBoxReview.Text) || textBoxReview.Text == "Enter text here...")
            {
                MessageBox.Show("Please enter a review.");
                return;
            }

            var selectedRSVP = (RSVP)comboBoxEvents.SelectedItem;

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
                        
                        cmd.Parameters.Add(":acc_num", OracleDbType.Int32).Value = Session.CurrentUser.Id; 
                        cmd.Parameters.Add(":ename", OracleDbType.Varchar2).Value = selectedRSVP.EName; 
                        cmd.Parameters.Add(":vname", OracleDbType.Varchar2).Value = selectedRSVP.VName; 
                        cmd.Parameters.Add(":event_date", OracleDbType.Date).Value = selectedRSVP.EventDate;
                        cmd.Parameters.Add(":rating", OracleDbType.Int32).Value = rating;
                        cmd.Parameters.Add(":text_review", OracleDbType.Clob).Value = textBoxReview.Text;

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Thank you for the Review", "Review Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //submitted so reset text box then reset rating to 1
                    comboBoxEvents.SelectedItem = null;
                    comboBoxEvents.Items.Clear();
                    LoadUsersRSVPS();
                    textBoxReview.Text = "Enter text here...";
                    textBoxReview.ForeColor = Color.Gray;
                    pictureBoxStar1_Click(null, null);
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
