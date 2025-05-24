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
    public partial class MongoManageEvent : UserControl
    {
        public MongoManageEvent()
        {
            InitializeComponent();
            InitialiseComboBox();
            InitialiseListView();
            InitialiseRSVPListView();

            // Refresh the data when control is brought up
            this.VisibleChanged += MongoManageEvent_VisibleChanged;
        }

        private void InitialiseComboBox()
        {
            comboBoxEventList.Items.Clear();
            comboBoxEventList.SelectedIndex = -1;  // Ensuring ComboBox is empty initially

        }


        private void InitialiseRSVPListView()
        {
            if (listViewRSVP.Columns.Count == 0)
            {
                listViewRSVP.View = View.Details;
                listViewRSVP.FullRowSelect = true;
                listViewRSVP.GridLines = true;

                listViewRSVP.Columns.Add("Name", 200);
                listViewRSVP.Columns.Add("Email", 220);
                listViewRSVP.Columns.Add("Status", 100);

                // Refresh the data when control is brought up
                this.VisibleChanged += MongoManageEvent_VisibleChanged;
            }
        }

        private void InitialiseListView()
        {
            // Check if ListView is already initialized
            if (listViewEvents.Columns.Count == 0)
            {
                listViewEvents.View = View.Details;
                listViewEvents.FullRowSelect = true;
                listViewEvents.GridLines = true;

                // Add columns for the ListView
                listViewEvents.Columns.Add("Event Name", 180);
                listViewEvents.Columns.Add("Date", 100);
                listViewEvents.Columns.Add("Time", 80);
                listViewEvents.Columns.Add("Venue", 150);
                listViewEvents.Columns.Add("City", 120);
                listViewEvents.Columns.Add("Price", 80);
            }
        }

        private void comboBoxEventList_DropDown(object sender, EventArgs e)
        {
            try
            {

                // Prevent from loading multiple times
                if (comboBoxEventList.Items.Count > 0) return;


                // Get the current user's ID
                Member user = Session.CurrentUser;
                int memberId = user.Id;

                // Get all events for that user
                var events = MongoDBDataAccess.GetEventsByCreator(memberId);

                foreach (var name in events)
                {
                    comboBoxEventList.Items.Add(name);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading events: " + ex.Message, "MongoDB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        // Called every time the visibility of the control changes (e.g., when brought back to view)
        private void MongoManageEvent_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                listViewEvents.Items.Clear();
                listViewRSVP.Items.Clear();
                comboBoxEventList.SelectedIndex = -1;

                // Trigger selection if valid
                if (comboBoxEventList.Items.Count > 1)
                {
                    comboBoxEventList.SelectedIndex = 0;
                }
            }
        }

        private void buttonDeleteEvent_Click(object sender, EventArgs e)
        {

        }

        private void buttonDelInstance_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxEventList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxEventList.SelectedIndex >= 0)
                {
                    string selectedEventName = comboBoxEventList.SelectedItem.ToString();
                    listViewEvents.Items.Clear();
                    listViewRSVP.Items.Clear();
                    DisplayEvents(selectedEventName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading event instances: {ex.Message}", "MongoDB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DisplayEvents(string selectedEventName)
        {
            try
            {
                listViewEvents.Items.Clear();
                var eventsCollection = MongoDbConfig.GetCollection<BsonDocument>("events");

                // Get the event by name
                var filter = Builders<BsonDocument>.Filter.Eq("ename", selectedEventName);
                var eventDoc = eventsCollection.Find(filter).FirstOrDefault();

                // Check if there are any instances
                if (eventDoc == null || !eventDoc.Contains("instances"))
                {
                    listViewEvents.Items.Add(new ListViewItem("No event instances found."));
                    return;
                }

                var instances = eventDoc["instances"].AsBsonArray;

                foreach (var inst in instances)
                {
                    var instDoc = inst.AsBsonDocument;
                    ListViewItem item = new ListViewItem(selectedEventName);

                    DateTime eventDate = instDoc.GetValue("eventDate", BsonNull.Value).ToUniversalTime();
                    DateTime eventTime = instDoc.GetValue("time", BsonNull.Value).ToUniversalTime();

                    item.SubItems.Add(eventDate.ToString("dd-MM-yyyy"));
                    item.SubItems.Add(eventTime.ToString("HH:mm"));

                    var venueDoc = instDoc.GetValue("venue", new BsonDocument()).AsBsonDocument;
                    string venueName = venueDoc.GetValue("vname", "").AsString;
                    string city = venueDoc.GetValue("city", "").AsString;

                    item.SubItems.Add(venueName);
                    item.SubItems.Add(city);

                    var priceValue = instDoc.GetValue("price", BsonNull.Value);
                    decimal price = 0;
                    if (priceValue.IsDecimal128) price = (decimal)priceValue.AsDecimal;
                    else if (priceValue.IsDouble) price = (decimal)priceValue.AsDouble;
                    else if (priceValue.IsInt32) price = priceValue.AsInt32;

                    item.SubItems.Add(price.ToString("C"));

                    item.Tag = new
                    {
                        EventName = selectedEventName,
                        EventDateTime = eventDate,
                        VenueName = venueName
                    };

                    listViewEvents.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying events: {ex.Message}", "MongoDB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
    
