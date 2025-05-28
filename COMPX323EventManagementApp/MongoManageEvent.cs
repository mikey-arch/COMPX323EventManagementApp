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
    /// <summary>
    /// Control for managing events and RSVPs stored in MongoDB.
    /// </summary>
    public partial class MongoManageEvent : UserControl
    {

        /// <summary>
        /// Constructor. Initializes UI elements and sets up visibility event.
        /// </summary>
        public MongoManageEvent()
        {
            InitializeComponent();
            InitialiseComboBox();
            InitialiseListView();
            InitialiseRSVPListView();

            // Refresh the data when control is brought up
            this.VisibleChanged += MongoManageEvent_VisibleChanged;
        }

        /// <summary>
        /// Initializes the combo box for event selection.
        /// </summary>
        private void InitialiseComboBox()
        {
            try
            {
                // Clear everything to ensure fresh loading
                comboBoxEventList.DataSource = null;
                comboBoxEventList.Items.Clear();

                Member user = Session.CurrentUser;
                int memberId = user.Id;

                var events = MongoDBDataAccess.GetEventsByCreator(memberId);
                var eventsList = new List<string> { "-- Select an event --" };
                eventsList.AddRange(events);

                comboBoxEventList.DataSource = eventsList;
                comboBoxEventList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading events: " + ex.Message, "MongoDB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Initializes the RSVP list view.
        /// </summary>
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
            }
        }

        /// <summary>
        /// Initializes the event instance list view.
        /// </summary>
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


        /// <summary>
        /// Called when the control becomes visible. Resets the ComboBox and clears the ListViews.
        /// </summary>
        private void MongoManageEvent_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                RefreshUI();  
                
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

        /// <summary>
        /// Called when a new event is selected in the ComboBox. 
        /// Clears the ListViews and displays event instances.
        /// </summary>
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

        /// <summary>
        /// Displays all instances of the selected event in the event ListView.
        /// </summary>
        /// <param name="selectedEventName">The name of the event selected by the user.</param>
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

                    // Parse as UTC
                    DateTime eventDateUtc = instDoc.GetValue("eventDate", BsonNull.Value).ToUniversalTime();
                    DateTime eventTimeUtc = instDoc.GetValue("time", BsonNull.Value).ToUniversalTime();

                    // Convert UTC to local time (your local timezone)
                    DateTime eventDateLocal = eventDateUtc.ToLocalTime();
                    DateTime eventTimeLocal = eventTimeUtc.ToLocalTime();


                    item.SubItems.Add(eventDateLocal.ToString("dd-MM-yyyy"));
                    item.SubItems.Add(eventTimeLocal.ToString("HH:mm"));

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
                        EventDateTime = eventDateLocal,
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

        /// <summary>
        /// Retrieves RSVP documents from the database for a specific event instance.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="eventDate">Date of the event (UTC).</param>
        /// <param name="venueName">Venue of the event.</param>
        /// <returns>List of BSON RSVP documents.</returns>
        public static List<BsonDocument> GetRsvpsForEventInstance(string eventName, DateTime eventDate, string venueName)
        {
            var rsvpCollection = MongoDbConfig.GetCollection<BsonDocument>("rsvps");

            // Convert eventDate to UTC midnight start and end of that day
            DateTime startUtc = new DateTime(eventDate.Year, eventDate.Month, eventDate.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(-1);
            DateTime endUtc = startUtc.AddDays(2);  // covers 2 days total


            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("ename", eventName),
                Builders<BsonDocument>.Filter.Eq("vname", venueName),
                Builders<BsonDocument>.Filter.Gte("eventDate", startUtc),
                Builders<BsonDocument>.Filter.Lt("eventDate", endUtc)
            );

            return rsvpCollection.Find(filter).ToList();
        }

        /// <summary>
        /// Displays RSVP entries for the selected event instance in the RSVP ListView.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventDate">The date of the event.</param>
        /// <param name="venueName">The venue name for the event instance.</param>
        private void DisplayRSVPs(string eventName, DateTime eventDate, string venueName)
        {
            try
            {
                var memberCollection = MongoDbConfig.GetCollection<BsonDocument>("members");


                Console.WriteLine("Event Date before method: " + eventDate.ToString("o"));
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

        /// <summary>
        /// Handles deletion of an RSVP, a specific event instance, or an entire event.
        /// </summary>
        /// <param name="num">
        /// 1 = delete entire event and all RSVPs, 
        /// 2 = delete selected instance and its RSVPs, 
        /// 3 = delete only one RSVP (current user).
        /// </param>
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
                    eventDate = eventDate.ToUniversalTime();

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
                            Builders<BsonDocument>.Filter.Eq("eventDate", eventDate),
                            Builders<BsonDocument>.Filter.Eq("accNum", attendeeId)
                        );
                        rsvpsCol.DeleteOne(rsvpFilter);
                        MessageBox.Show("RSVP deleted.");
                        break;
                    case 2: // Delete instance + Related RSVPs

                        var selectedItem = listViewEvents.SelectedItems[0];

                        // Get correct date
                        string eventDateText = selectedItem.SubItems[1].Text;
                        string eventTimeText = selectedItem.SubItems[2].Text;

                        // Parse date and time separately
                        DateTime datePart = DateTime.ParseExact(eventDateText, "dd-MM-yyyy", null);
                        TimeSpan timePart = TimeSpan.Parse(eventTimeText);

                        // Combine them
                        DateTime eventDateTime = datePart.Date + timePart;

                        Console.WriteLine("EventDate parameter (Local): " + eventDateTime.ToString("o"));

                        // Convert eventDate to UTC midnight start and end of that day
                        DateTime startUtc = new DateTime(eventDateTime.Year, eventDateTime.Month, eventDateTime.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(-1);
                        DateTime endUtc = startUtc.AddDays(2);  // covers 2 days total

                        Console.WriteLine("Start UTC: " + startUtc.ToString("o"));
                        Console.WriteLine("End UTC: " + endUtc.ToString("o"));

                        var eventFilter = Builders<BsonDocument>.Filter.Eq("ename", eventName);

                        var updatePull = Builders<BsonDocument>.Update.PullFilter("instances",
                            Builders<BsonDocument>.Filter.And(
                                Builders<BsonDocument>.Filter.Gte("eventDate", startUtc),
                                Builders<BsonDocument>.Filter.Lt("eventDate", endUtc),
                                Builders<BsonDocument>.Filter.Eq("venue.vname", venueName)
                            )
                        );

                        var updateResult = eventsCol.UpdateOne(eventFilter, updatePull);


                        var instanceRsvpFilter = Builders<BsonDocument>.Filter.And(
                            Builders<BsonDocument>.Filter.Eq("ename", eventName),
                            Builders<BsonDocument>.Filter.Eq("vname", venueName),
                            Builders<BsonDocument>.Filter.Gte("eventDate", startUtc),
                            Builders<BsonDocument>.Filter.Lt("eventDate", endUtc)
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

        /// <summary>
        /// Called when an event instance is selected in the ListView.
        /// Loads related RSVP entries into the RSVP ListView.
        /// </summary>
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
                    string eventName = selectedItem.Text; 
                    string venueName = selectedItem.SubItems[3].Text;


                    // Get correct date
                    string eventDateText = selectedItem.SubItems[1].Text; 
                    string eventTimeText = selectedItem.SubItems[2].Text; 

                    // Parse date and time separately
                    DateTime datePart = DateTime.ParseExact(eventDateText, "dd-MM-yyyy", null);
                    TimeSpan timePart = TimeSpan.Parse(eventTimeText);

                    // Combine them
                    DateTime eventDateTime = datePart.Date + timePart;

                    // Call the method to display RSVPs for this specific event instance
                    DisplayRSVPs(eventName, eventDateTime, venueName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting event instance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Refreshes the UI components: clears lists and resets combo box.
        /// Re-fetches event data from MongoDB for the current user.
        /// </summary>
        private void RefreshUI()
        {
            // refresh  UI elements (ListView, ComboBox)
            listViewEvents.Items.Clear();
            listViewRSVP.Items.Clear();

            // reset the combo box correctly
            comboBoxEventList.DataSource = null; 

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
    
