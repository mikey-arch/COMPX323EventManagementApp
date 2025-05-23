﻿using System;
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
        private MongoCreateEventControl mongoCreateEventControl;
        private ProfileMongo mongoProfileControl;
        private MongoSearchEvent mongoSearchEventControl;
        private MongoCreateReview mongoCreateReviewControl;
        private MongoManageEvent mongoManageEventControl;
        public EventsManagerForm()
        {
            InitializeComponent();
            //Initialises all the user control isntances
            searchControl = new SearchEventControl();
            profileControl = new ProfileControl();
            createEventControl = new CreateEventControl();
            manageEventControl = new ManageEventControl();
            reviewControl = new CreateReviewControl();
            mongoCreateEventControl = new MongoCreateEventControl();
            mongoProfileControl = new ProfileMongo();
            mongoSearchEventControl = new MongoSearchEvent();
            mongoCreateReviewControl = new MongoCreateReview();
            mongoManageEventControl = new MongoManageEvent();

            Member user = Session.CurrentUser;
            labelProfilePicName.Text = user.Fname + " " + user.Lname;
            this.Load += new EventHandler(EventsManagerForm_Load);
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

        //Displays the create event user control MONGODB version
        private void buttonMongoCreateEvent_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "EVENT DETAILS MongoDB";
            panelContent.Controls.Clear();
            profileControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(mongoCreateEventControl);

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
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Handles the exit button click event to close the application.
        private void labelExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Handles showing the profile control by default/on load
        private void EventsManagerForm_Load(object sender, EventArgs e)
        {
            labelTitle.Text = "PROFILE";
            panelContent.Controls.Clear();
            profileControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(profileControl);
        }

        //handles showing the mongo version of viewing profile control
        private void buttonMongoViewProfile_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "VIEW PROFILE MongoDB";
            panelContent.Controls.Clear();
            mongoProfileControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(mongoProfileControl);
        }

        //handles showing the mongo version search event control
        private void buttonMongoSearchEvent_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "SEARCH FOR EVENT MongoDB";
            panelContent.Controls.Clear();
            mongoProfileControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(mongoSearchEventControl);

        }

        private void buttonMongoReview_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "CREATE REVIEW MongoDB";
            panelContent.Controls.Clear();
            mongoProfileControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(mongoCreateReviewControl);
        }

        private void buttonMongoManageEvent_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "MANAGE EVENT MongoDB";
            panelContent.Controls.Clear();
            mongoProfileControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(mongoManageEventControl);
        }
    }
}
