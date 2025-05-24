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
                List<string> eventsList = new List<string> { "-- Select an event --" }; // Placeholder
                foreach (var name in events)
                {
                    eventsList.Add(name);
                    comboBoxEventList.Items.Add(name);
                }
                comboBoxEventList.DataSource = eventsList;
                comboBoxEventList.SelectedIndex = 0; // Set to placeholder
                    
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
            Delete(1);
        }

        private void buttonDelInstance_Click(object sender, EventArgs e)
        {
            Delete(2);
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

        public static List<BsonDocument> GetRsvpsForEventInstance(string eventName, DateTime eventDate, string venueName)
        {
            var rsvpCollection = MongoDbConfig.GetCollection<BsonDocument>("rsvps");

            var utcEventDate = DateTime.SpecifyKind(eventDate, DateTimeKind.Utc);

            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("ename", eventName),
                Builders<BsonDocument>.Filter.Eq("eventDate", utcEventDate),
                Builders<BsonDocument>.Filter.Eq("vname", venueName)
            );

            return rsvpCollection.Find(filter).ToList();
        }


        private void DisplayRSVPs(string eventName, DateTime eventDate, string venueName)
        {
            try
            {
                var memberCollection = MongoDbConfig.GetCollection<BsonDocument>("members");

                // get RSVPs
                var rsvps = GetRsvpsForEventInstance(eventName, eventDate, venueName);

                listViewRSVP.Items.Clear();
                listViewRSVP.Enabled = true;

                bool foundRSVPs = false;

                foreach (var rsvp in rsvps)
                {
                    foundRSVPs = true;

                    string fname = "Unknown";
                    string lname = "";
                    string email = "";
                    string status = "n/a";

                    Console.WriteLine(rsvp.ToJson());

                    // Get accNum safely
                    if (rsvp.Contains("accNum") && rsvp["accNum"].IsInt32)
                    {
                        int accNum = rsvp["accNum"].AsInt32;

                        // Look up member from members collection
                        var memberFilter = Builders<BsonDocument>.Filter.Eq("_id", accNum);
                        var member = memberCollection.Find(memberFilter).FirstOrDefault();

                        if (member != null)
                        {
                            fname = member.GetValue("fname", "").AsString;
                            lname = member.GetValue("lname", "").AsString;
                            email = member.GetValue("email", "").AsString;
                        }
                    }

                    if (rsvp.Contains("status") && rsvp["status"].IsString)
                    {
                        status = rsvp["status"].AsString;
                    }

                    ListViewItem item = new ListViewItem($"{fname} {lname}");
                    item.SubItems.Add(email);
                    item.SubItems.Add(status);
                    item.Tag = new { EventName = eventName, EventDate = eventDate, VenueName = venueName };

                    listViewRSVP.Items.Add(item);
                }


                if (!foundRSVPs)
                {
                    listViewRSVP.Items.Add("No RSVPs for this event instance.");
                    listViewRSVP.Enabled = false;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying RSVPs: {ex.Message}", "MongoDB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Delete(int num)
        {
            try
            {
                if (comboBoxEventList.SelectedItem == null || comboBoxEventList.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select an event first.");
                    return;
                }

                if ((num == 2 || num == 3) && listViewEvents.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select an event instance first.");
                    return;
                }

                if (num == 3 && listViewRSVP.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select an RSVP to delete.");
                    return;
                }

                // Get the attendee ID from the current user session
                Member user = Session.CurrentUser;
                int attendeeId = user.Id;

                string eventName = comboBoxEventList.SelectedItem.ToString();
                string venueName = "";
                DateTime eventDate = DateTime.MinValue;

                if (num == 2 || num == 3)
                {
                    if (listViewEvents.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Please select an event instance.");
                        return;
                    }

                    var tag = (dynamic)listViewEvents.SelectedItems[0].Tag;
                    venueName = tag.VenueName;
                    eventDate = DateTime.SpecifyKind(tag.EventDateTime, DateTimeKind.Utc);

                }

                if (num == 3 && listViewRSVP.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select an RSVP.");
                    return;
                }

                // Collections
                var eventsCol = MongoDbConfig.GetCollection<BsonDocument>("events");
                var rsvpsCol = MongoDbConfig.GetCollection<BsonDocument>("rsvps");

                switch (num)
                {
                    case 3: // Delete RSVP
                        var rsvpFilter = Builders<BsonDocument>.Filter.And(
                            Builders<BsonDocument>.Filter.Eq("ename", eventName),
                            Builders<BsonDocument>.Filter.Eq("venue.vname", venueName),
                            Builders<BsonDocument>.Filter.Eq("event_date", eventDate),
                            Builders<BsonDocument>.Filter.Eq("acc_num", attendeeId)
                        );
                        rsvpsCol.DeleteOne(rsvpFilter);
                        MessageBox.Show("RSVP deleted.");
                        break;
                    case 2: // Delete instance + Related RSVPs
                        var eventFilter = Builders<BsonDocument>.Filter.Eq("ename", eventName);

                        var updatePull = Builders<BsonDocument>.Update.PullFilter("instances",
                            Builders<BsonDocument>.Filter.And(
                                Builders<BsonDocument>.Filter.Eq("eventDate", eventDate),
                                Builders<BsonDocument>.Filter.Eq("venue.vname", venueName)
                            )
                        );

                        var updateResult = eventsCol.UpdateOne(eventFilter, updatePull);

                        var instanceRsvpFilter = Builders<BsonDocument>.Filter.And(
                            Builders<BsonDocument>.Filter.Eq("ename", eventName),
                            Builders<BsonDocument>.Filter.Eq("venue.vname", venueName),
                            Builders<BsonDocument>.Filter.Eq("event_date", eventDate)
                        );
                        rsvpsCol.DeleteMany(instanceRsvpFilter);
                        


                        MessageBox.Show("Event instance and related RSVPs deleted.");
                        break;
                    case 1: // Delete Entire event (Event + All instances + RSVPs)
                        eventsCol.DeleteOne(Builders<BsonDocument>.Filter.Eq("ename", eventName));
                        rsvpsCol.DeleteMany(Builders<BsonDocument>.Filter.Eq("ename", eventName));

                        MessageBox.Show("Event and all related data deleted.");
                        break;


                }
                RefreshUI();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select an event instance to delete/delete from. Error: " + ex.Message);
            }

            
            
        }

        private void listViewEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Ensure an item is selected in the ListView
                if (listViewEvents.SelectedItems.Count > 0)
                {
                    var selectedItem = listViewEvents.SelectedItems[0];

                    if (listViewEvents.SelectedItems[0].Text == "No event instances found.")
                    {
                        return;
                    }

                    // Get event details from the selected ListView item
                    string eventName = selectedItem.Text; // The first column: Event Name
                    DateTime eventDate = DateTime.Parse(selectedItem.SubItems[1].Text); // Second column: Event Date
                    string venueName = selectedItem.SubItems[3].Text; // Fourth column: Venue Name
                    // Call the method to display RSVPs for this specific event instance
                    DisplayRSVPs(eventName, eventDate, venueName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting event instance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshUI()
        {
            // Refresh your UI elements (ListView, ComboBox, etc.)
            listViewEvents.Items.Clear();
            listViewRSVP.Items.Clear();

            // Reset the combo box correctly
            comboBoxEventList.DataSource = null; // Unbind first

            try
            {
                Member user = Session.CurrentUser;
                int memberId = user.Id;
                var events = MongoDBDataAccess.GetEventsByCreator(memberId);

                // Add placeholder manually
                var eventsList = new List<string> { "-- Select an event --" };
                eventsList.AddRange(events);

                comboBoxEventList.DataSource = eventsList;
                comboBoxEventList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing events: " + ex.Message, "MongoDB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
    
