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
using COMPX323EventManagementApp.Properties;

namespace COMPX323EventManagementApp
{
    public partial class MongoCreateReview : UserControl
    {
        private int rating = 1; //holds star rating defaulted at 1 star

        public MongoCreateReview()
        {
            InitializeComponent();
            LoadUsersRSVPS();

            this.VisibleChanged += MongoCreateReview_VisibleChanged;

        }

        //populates combobox with rsvps to be reviewed
        private void LoadUsersRSVPS()
        {
            try
            {
                comboBoxEvents.Items.Clear();
                
                // Get attended RSVPs that haven't been reviewed yet
                var attendedRsvps = MongoDBDataAccess.GetAttendedRSVPsForReview(Session.CurrentUser.Id);
                
                foreach (var rsvp in attendedRsvps)
                {
                    comboBoxEvents.Items.Add(rsvp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading RSVPs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //as is a usercontrol refresh when pops up everytime
        private void MongoCreateReview_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                comboBoxEvents.Items.Clear();
                LoadUsersRSVPS();
            }

        }

        private void pictureBoxStar1_Click_1(object sender, EventArgs e)
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_white;
            pictureBoxStar3.Image = Resources.star_64_white;
            pictureBoxStar4.Image = Resources.star_64_white;
            pictureBoxStar5.Image = Resources.star_64_white;
            rating = 1;
        }

        //visually changes and updates rating when clicked
        private void pictureBoxStar2_Click_1(object sender, EventArgs e)
        {

            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_gold;
            pictureBoxStar3.Image = Resources.star_64_white;
            pictureBoxStar4.Image = Resources.star_64_white;
            pictureBoxStar5.Image = Resources.star_64_white;
            rating = 2;
        }

        //visually changes and updates rating when clicked
        private void pictureBoxStar3_Click_1(object sender, EventArgs e)
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_gold;
            pictureBoxStar3.Image = Resources.star_64_gold;
            pictureBoxStar4.Image = Resources.star_64_white;
            pictureBoxStar5.Image = Resources.star_64_white;
            rating = 3;
        }

        private void pictureBoxStar4_Click_1(object sender, EventArgs e)
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_gold;
            pictureBoxStar3.Image = Resources.star_64_gold;
            pictureBoxStar4.Image = Resources.star_64_gold;
            pictureBoxStar5.Image = Resources.star_64_white;
            rating = 4;
        }

        private void pictureBoxStar5_Click_1(object sender, EventArgs e)
        {
            pictureBoxStar1.Image = Resources.star_64_gold;
            pictureBoxStar2.Image = Resources.star_64_gold;
            pictureBoxStar3.Image = Resources.star_64_gold;
            pictureBoxStar4.Image = Resources.star_64_gold;
            pictureBoxStar5.Image = Resources.star_64_gold;
            rating = 5;
        }

        //placeholder text to point user to where to write review 
        private void textBoxReview_Enter_1(object sender, EventArgs e)
        {
            if(textBoxReview.Text == "Enter text here...")
            {
                textBoxReview.Text = "";
                textBoxReview.ForeColor = Color.Black;
            }
        }

        //repopulate placeholder text to point user to where to write review when leaving textbox
        private void textBoxReview_Leave_1(object sender, EventArgs e)
        {
            if(textBoxReview.Text == "")
            {
                textBoxReview.Text = "Enter text here...";
                textBoxReview.ForeColor = Color.Gray;
            }
        }

        //handles submiting review and updating the database with new review and rating
        private void buttonSubmitReview_Click_1(object sender, EventArgs e)
        {
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
                // Create review in MongoDB
                bool reviewSuccess = MongoDBDataAccess.CreateReview(
                    Session.CurrentUser.Id,
                    selectedRSVP.EName,
                    selectedRSVP.VName,
                    selectedRSVP.EventDate,
                    rating,
                    textBoxReview.Text
                );

                if (reviewSuccess)
                {
                    MessageBox.Show("Thank you for the Review", "Review Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //submitted so reset text box then reset rating to 1
                    comboBoxEvents.SelectedItem = null;
                    comboBoxEvents.Items.Clear();
                    LoadUsersRSVPS();
                    textBoxReview.Text = "Enter text here...";
                    textBoxReview.ForeColor = Color.Gray;
                    pictureBoxStar1_Click_1(null, null);
                }
                else
                {
                    MessageBox.Show("Failed to create review. You may have already reviewed this event.", 
                        "Review Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
