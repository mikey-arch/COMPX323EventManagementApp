using COMPX323EventManagementApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMPX323EventManagementApp
{
    public partial class ProfileMongo : UserControl
    {
        String currentlySelected;
        Member user;
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

        private void ResetListViews()
        {
            //clear and set up list view columns for RSVPS
            listViewDisplay.Items.Clear();
            listViewDisplay.Columns.Clear();
            listViewDisplay.View = View.Details;
            listViewDisplay.FullRowSelect = true;
        }

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
                var rsvps = MongoDBDataAccess.GetUserRsvps(user.Id);

                foreach (var (ename, eventDate, venue, status) in rsvps)
                {
                    var item = new ListViewItem(ename);
                    item.SubItems.Add(eventDate);
                    item.SubItems.Add(venue);
                    item.SubItems.Add(status);
                    listViewDisplay.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "DataBase Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

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

        // refreshes when specific view is currently selected
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
    }
}
