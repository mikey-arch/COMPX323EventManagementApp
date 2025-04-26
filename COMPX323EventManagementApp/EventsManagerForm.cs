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

namespace COMPX323EventManagementApp
{
    /// <summary>
    /// This class represent the main dashboard form that manages navigation between different user controls.
    /// </summary>
    public partial class EventsManagerForm : Form
    {
        // User controls for different sections
        private SearchEventControl searchControl;
        private ProfileControl profileControl;
        private CreateEventControl createEventControl;
        private ManageEventControl manageEventControl;
        private CreateReviewControl reviewControl;

        public EventsManagerForm()
        {
            InitializeComponent();
            //Initialises all the user control isntances
            searchControl = new SearchEventControl();
            profileControl = new ProfileControl();
            createEventControl = new CreateEventControl();
            manageEventControl = new ManageEventControl();
            reviewControl = new CreateReviewControl();

            User user = Session.CurrentUser;
            labelProfilePicName.Text = user.Fname + " " + user.Lname;
        }

        //Displays the search event user control 
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "SEARCH FOR EVENT";
            panelContent.Controls.Clear();
            searchControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(searchControl);

        }

        //Displays the profile user control
        private void buttonProfile_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "PROFILE";
            panelContent.Controls.Clear();
            profileControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(profileControl);

        }

        //Displays the create event user control
        private void buttonCreateEvent_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "EVENT DETAILS";
            panelContent.Controls.Clear();
            createEventControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(createEventControl);

        }

        //Displays the manage event user control
        private void buttonManageEvent_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "MANAGE DETAILS";
            panelContent.Controls.Clear();
            manageEventControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(manageEventControl);

        }

        //Displays the create review user control
        private void buttonCreateReview_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "RATING AND REVIEWS";
            panelContent.Controls.Clear();
            reviewControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(reviewControl);

        }

        // Handles the logout button click event navigating back to the login form.
        private void buttonLogout_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            this.Hide();
            loginForm.ShowDialog();
            this.Close();
        }

        // Handles the exit button click event to close the application.
        private void labelExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
