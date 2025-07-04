﻿using COMPX323EventManagementApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;
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

namespace COMPX323EventManagementApp
{
    /// <summary>
    /// User control for displaying a MongoDB-based profile view for the logged-in user.
    /// Shows their upcoming RSVPs, submitted reviews, and created events. 
    /// Allows RSVP deletion from the list view.
    /// </summary>
    public partial class ProfileMongo : UserControl
    {
        String currentlySelected;
        Member user;
        /// <summary>
        /// Initializes the profile view with user information and sets up visibility event listener.
        /// </summary>
        public ProfileMongo()
        {
            InitializeComponent();
            user = Session.CurrentUser;
            labelAccountNum.Text = user.Id.ToString();
            labelName.Text = user.Fname + " " + user.Lname;
            labelEmail.Text = user.Email;
            currentlySelected = "";

            this.VisibleChanged += ProfileMongoControl_VisibleChanged; 
        }

        //Clears and prepares the list view control for displaying new content.
        private void ResetListViews()
        {
            //clear and set up list view columns for RSVPS
            listViewDisplay.Items.Clear();
            listViewDisplay.Columns.Clear();
            listViewDisplay.View = View.Details;
            listViewDisplay.FullRowSelect = true;
        }

        /// <summary>
        /// Displays the user's upcoming RSVPs in the list view. 
        /// Allows RSVP deletion on double-click.
        /// </summary>
        private void buttonRsvps_Click(object sender, EventArgs e)
        {
            listViewDisplay.Enabled = true;
            currentlySelected = "RSVP";
            try
            {
                ResetListViews();

                listViewDisplay.Columns.Add("Event", 180);
                listViewDisplay.Columns.Add("Date", 100);
                listViewDisplay.Columns.Add("Venue", 150);
                listViewDisplay.Columns.Add("Status", 80);

                user = Session.CurrentUser;
                var rsvps = MongoDBDataAccess.GetUpcomingRsvps(user.Id);

                foreach (var (ename, eventDate, venue, status) in rsvps)
                {
                    
                    //DateTime fullDateTime = DateTime.Parse(eventDateStr).ToLocalTime();
                    DateTime fullDateTime = eventDate.ToLocalTime(); // You already have the correct value


                    var item = new ListViewItem(ename);
                    item.SubItems.Add(eventDate.ToString("yyyy-MM-dd"));
                    item.SubItems.Add(venue);
                    item.SubItems.Add(status);
                    item.Tag = eventDate; // preserve original DateTime
                    listViewDisplay.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "DataBase Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Displays all reviews submitted by the user.
        /// </summary>
        private void buttonReviews_Click(object sender, EventArgs e)
        {
            listViewDisplay.Enabled = true;
            currentlySelected = "Reviews";
            try
            {
                ResetListViews();

                listViewDisplay.Columns.Add("Event", 180);
                listViewDisplay.Columns.Add("Venue", 150);
                listViewDisplay.Columns.Add("Date", 100);
                listViewDisplay.Columns.Add("Rating", 50);
                listViewDisplay.Columns.Add("Review", 300);

                user = Session.CurrentUser;
                var userReviews = MongoDBDataAccess.GetUserCreatedReviews(user.Id);

                foreach (var (ename, venue, eventDate, rating, review) in userReviews)
                {
                    var item = new ListViewItem(ename);
                    item.SubItems.Add(venue);
                    item.SubItems.Add(eventDate);
                    item.SubItems.Add(rating.ToString());
                    item.SubItems.Add(review);
                    listViewDisplay.Items.Add(item);
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "DataBase Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Displays all events created by the user in the list view.
        /// </summary>
        private void buttonEvents_Click(object sender, EventArgs e)
        {
            listViewDisplay.Enabled = true;
            currentlySelected = "Events";

            try
            {
                ResetListViews();

                listViewDisplay.Columns.Add("Event Name", 180);
                listViewDisplay.Columns.Add("Description", 250);
                listViewDisplay.Columns.Add("Creation Date", 100);

                user = Session.CurrentUser;
                var userEvents = MongoDBDataAccess.GetUserCreatedEvents(user.Id);

                foreach (var (ename, description, creationDate) in userEvents)
                {
                    var item = new ListViewItem(ename);
                    item.SubItems.Add(description);
                    item.SubItems.Add(creationDate);
                    listViewDisplay.Items.Add(item);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "DataBase Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Refreshes the current list view data when the control becomes visible,
        /// depending on which button was previously selected.
        /// </summary>
        private void ProfileMongoControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (currentlySelected == "RSVP")
                {
                    buttonRsvps_Click(null, null);
                }
                else if (currentlySelected == "Reviews")
                {
                    buttonReviews_Click(null, null);
                }
                else if (currentlySelected == "Events")
                {
                    buttonEvents_Click(null, null);
                }
            }
        }

        /// <summary>
        /// Handles double-clicks on RSVP list items.
        /// Prompts the user to confirm RSVP deletion and performs it if confirmed.
        /// </summary>
        private void listViewDisplay_DoubleClick(object sender, EventArgs e)
        {
            if (listViewDisplay.SelectedItems.Count > 0 && currentlySelected == "RSVP")
            {
                // Get the selected item. Assuming it contains event data (event name, date, and venue)
                var selectedEvent = listViewDisplay.SelectedItems[0];

                // Extract event name, event date, and venue name from the ListView columns
                string eventName = selectedEvent.Text;

                DateTime eventDate = ((DateTime)selectedEvent.Tag).ToUniversalTime();

                string venueName = selectedEvent.SubItems[2].Text;
                string status = selectedEvent.SubItems[3].Text;

                DialogResult result = MessageBox.Show(
                $"Event Details:\n" +
                $"Name: {eventName}\n" +
                $"Date: {eventDate:dd-MM-yyyy}\n" +
                $"Venue: {venueName}\n" +
                $"RSVP Status: {status}\n\n" +
                $"Do you want delete your RSVP to this event?",
                "Event Options",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        //bool success = MongoDBDataAccess.DeleteRSVP(user.Id, eventName, venueName, eventDate);
                        bool success = MongoDBDataAccess.DeleteRSVP(user.Id, eventName, venueName, eventDate.ToUniversalTime());

                        if (success)
                        {
                            MessageBox.Show("RSVP successfully deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            buttonRsvps_Click(null, null); // refresh view
                        }
                        else
                        {
                            MessageBox.Show("No RSVP found to delete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting RSVP: {ex.Message}",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
        }
    }
}
