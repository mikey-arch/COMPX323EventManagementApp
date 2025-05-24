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
using COMPX323EventManagementApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace COMPX323EventManagementApp
{
    public partial class MongoSearchEvent : UserControl
    {
        public MongoSearchEvent()
        {
            InitializeComponent();
            LoadComboBoxes();
            DisplayEvents(textBoxSearch.Text.Trim());
            this.VisibleChanged += MongoSearchEvent_VisibleChanged;
        }

        //loads all comboboxes with appropriate filters names
        private void LoadComboBoxes()
        {
            try
            {
                dateTimePickerMonth.CustomFormat = "MMMM yyyy";
                dateTimePickerMonth.Value = DateTime.Now;
                comboBoxPrice.Items.Add("Asc");
                comboBoxPrice.Items.Add("Desc");
                comboBoxPrice.Items.Add("Free");
                comboBoxPrice.SelectedIndex = 0;

                var eventsCollection = MongoDbConfig.GetCollection<BsonDocument>("events");

                // Load locations (cities) from venue data in events
                var locationPipeline = new BsonDocument[]
                {
                    new BsonDocument("$unwind", "$instances"),
                    new BsonDocument("$group", new BsonDocument("_id", "$instances.venue.city")),
                    new BsonDocument("$sort", new BsonDocument("_id", 1))
                };

                var locationResults = eventsCollection.Aggregate<BsonDocument>(locationPipeline).ToList();
                comboBoxLocation.Items.Add("All Locations");
                foreach (var location in locationResults)
                {
                    comboBoxLocation.Items.Add(location["_id"].AsString);
                }
                comboBoxLocation.SelectedIndex = 0;

                // Load categories from events
                var categoryPipeline = new BsonDocument[]
                {
                    new BsonDocument("$unwind", "$categories"),
                    new BsonDocument("$group", new BsonDocument("_id", "$categories.name")),
                    new BsonDocument("$sort", new BsonDocument("_id", 1))
                };

                var categoryResults = eventsCollection.Aggregate<BsonDocument>(categoryPipeline).ToList();
                comboBoxCategory.Items.Add("All Categories");
                foreach (var category in categoryResults)
                {
                    comboBoxCategory.Items.Add(category["_id"].AsString);
                }
                comboBoxCategory.SelectedIndex = 0;

                // Load restrictions from events
                var restrictionPipeline = new BsonDocument[]
                {
                    new BsonDocument("$group", new BsonDocument("_id", "$restriction.name")),
                    new BsonDocument("$sort", new BsonDocument("_id", 1))
                };

                var restrictionResults = eventsCollection.Aggregate<BsonDocument>(restrictionPipeline).ToList();
                comboBoxRestriction.Items.Add("All Restrictions");
                foreach (var restriction in restrictionResults)
                {
                    var restrictionName = restriction["_id"].AsString;
                    if (!string.IsNullOrEmpty(restrictionName))
                    {
                        comboBoxRestriction.Items.Add(restrictionName);
                    }
                }
                comboBoxRestriction.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading filters: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //displays updated filtered events in the listview
        private void DisplayEvents(string searchWord = "")
        {
            try
            {
                // Clear existing items
                listViewEvents.Items.Clear();

                // Make sure ListView is properly set up
                if (listViewEvents.Columns.Count == 0)
                {
                    listViewEvents.View = View.Details;
                    listViewEvents.FullRowSelect = true;
                    listViewEvents.GridLines = true;

                    listViewEvents.Columns.Add("Event Name", 180);
                    listViewEvents.Columns.Add("Date", 100);
                    listViewEvents.Columns.Add("Time", 80);
                    listViewEvents.Columns.Add("Venue", 150);
                    listViewEvents.Columns.Add("Price", 80);
                    listViewEvents.Columns.Add("Category", 100);
                    listViewEvents.Columns.Add("Restrictions", 120);
                }

                var eventsCollection = MongoDbConfig.GetCollection<BsonDocument>("events");
                
                // Build the aggregation pipeline
                var pipeline = new List<BsonDocument>();

                // Unwind instances to work with individual event instances
                pipeline.Add(new BsonDocument("$unwind", "$instances"));

                // Match conditions
                var matchConditions = new BsonDocument();

               
                DateTime selectedDate = dateTimePickerMonth.Value;
                var startOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                var endOfMonth = startOfMonth.AddMonths(1);

                DateTime nowUtc = DateTime.UtcNow;

                var lowerBound = nowUtc > startOfMonth ? nowUtc : startOfMonth;

                matchConditions.Add("instances.eventDate", new BsonDocument
                {
                    { "$gte", lowerBound },
                    { "$lt", endOfMonth }
                });

                // Search filter
                if (!string.IsNullOrEmpty(searchWord))
                {
                    var searchRegex = new BsonDocument("$regex", searchWord)
                    {
                        {"$options", "i"} // case insensitive
                    };
                    matchConditions.Add("$or", new BsonArray
                    {
                        new BsonDocument("ename", searchRegex),
                        new BsonDocument("description", searchRegex)
                    });
                }

                // Price filter
                string priceFilter = comboBoxPrice.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(priceFilter) && priceFilter == "Free")
                {
                    matchConditions.Add("instances.price", 0);
                }

                // Location filter
                string locationFilter = comboBoxLocation.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(locationFilter) && locationFilter != "All Locations")
                {
                    matchConditions.Add("instances.venue.city", locationFilter);
                }

                // Category filter
                string categoryFilter = comboBoxCategory.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "All Categories")
                {
                    matchConditions.Add("categories.name", categoryFilter);
                }

                // Restriction filter
                string restrictionFilter = comboBoxRestriction.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(restrictionFilter) && restrictionFilter != "All Restrictions")
                {
                    matchConditions.Add("restriction.name", restrictionFilter);
                }

                pipeline.Add(new BsonDocument("$match", matchConditions));

                // Project the fields we need
                pipeline.Add(new BsonDocument("$project", new BsonDocument
                {
                    {"ename", 1},
                    {"description", 1},
                    {"restriction", 1},
                    {"categories", 1},
                    {"eventDate", "$instances.eventDate"},
                    {"time", "$instances.time"},
                    {"venue", "$instances.venue"},
                    {"price", "$instances.price"}
                }));

                // Sort based on price filter
                BsonDocument sortStage;
                if (priceFilter == "Asc")
                {
                    sortStage = new BsonDocument("$sort", new BsonDocument
                    {
                        {"price", 1},
                        {"eventDate", 1}
                    });
                }
                else if (priceFilter == "Desc")
                {
                    sortStage = new BsonDocument("$sort", new BsonDocument
                    {
                        {"price", -1},
                        {"eventDate", 1}
                    });
                }
                else
                {
                    sortStage = new BsonDocument("$sort", new BsonDocument
                    {
                        {"eventDate", 1},
                        {"time", 1}
                    });
                }
                pipeline.Add(sortStage);

                var results = eventsCollection.Aggregate<BsonDocument>(pipeline).ToList();

                bool foundResults = false;
                foreach (var result in results)
                {
                    foundResults = true;

                    // Create list view item with event name
                    ListViewItem item = new ListViewItem(result.GetValue("ename", "").AsString);

                    // Add date
                    var eventDate = result.GetValue("eventDate", BsonNull.Value);
                    if (eventDate != BsonNull.Value)
                    {
                        item.SubItems.Add(eventDate.ToLocalTime().ToString("dd-MM-yyyy"));
                    }
                    else
                    {
                        item.SubItems.Add("");
                    }

                    // Add time
                    var eventTime = result.GetValue("time", BsonNull.Value);
                    if (eventTime != BsonNull.Value)
                    {
                        item.SubItems.Add(eventTime.ToLocalTime().ToString("HH:mm"));
                    }
                    else
                    {
                        item.SubItems.Add("");
                    }

                    // Add venue name
                    var venue = result.GetValue("venue", new BsonDocument()).AsBsonDocument;
                    item.SubItems.Add(venue.GetValue("vname", "").AsString);

                    // Add price - handle different BSON numeric types
                    var priceValue = result.GetValue("price", BsonNull.Value);
                    decimal price = 0;
                    if (priceValue != BsonNull.Value)
                    {
                        if (priceValue.IsDecimal128)
                        {
                            price = priceValue.AsDecimal;
                        }
                        else if (priceValue.IsDouble)
                        {
                            price = (decimal)priceValue.AsDouble;
                        }
                        else if (priceValue.IsInt32)
                        {
                            price = priceValue.AsInt32;
                        }
                        else if (priceValue.IsInt64)
                        {
                            price = priceValue.AsInt64;
                        }
                    }
                    item.SubItems.Add(price.ToString("C"));

                    // Add categories (concatenated)
                    var categories = result.GetValue("categories", new BsonArray()).AsBsonArray;
                    var categoryNames = categories.Select(c => c.AsBsonDocument.GetValue("name", "").AsString).ToList();
                    item.SubItems.Add(string.Join(", ", categoryNames));

                    // Add restrictions
                    var restriction = result.GetValue("restriction", new BsonDocument()).AsBsonDocument;
                    item.SubItems.Add(restriction.GetValue("name", "").AsString);

                    listViewEvents.Items.Add(item);
                }

                labelEvents.Text = foundResults ? $"Found {listViewEvents.Items.Count} events" : "No events found matching your criteria";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying events: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //refrehses dispalying events when filters are changed
        private void dateTimePickerMonth_ValueChanged(object sender, EventArgs e)
        {
            DisplayEvents(textBoxSearch.Text.Trim());
        }

        private void comboBoxLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayEvents(textBoxSearch.Text.Trim());
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayEvents(textBoxSearch.Text.Trim());
        }

        private void comboBoxRestriction_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayEvents(textBoxSearch.Text.Trim());
        }

        private void comboBoxPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayEvents(textBoxSearch.Text.Trim());
        }

        //handles double clicking an event youre interested in where you can rsvp
        private void listViewEvents_DoubleClick(object sender, EventArgs e)
        {
            if (listViewEvents.SelectedItems.Count > 0)
            {
                // Get the selected item. Assuming it contains event data (event name, date, and venue)
                var selectedEvent = listViewEvents.SelectedItems[0];

                // Extract event name, event date, and venue name from the ListView columns
                string eventName = selectedEvent.Text; 
                DateTime eventDate = DateTime.ParseExact(selectedEvent.SubItems[1].Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string venueName = selectedEvent.SubItems[3].Text; 
                string eventTime = selectedEvent.SubItems[2].Text;
                string price = selectedEvent.SubItems[4].Text;
                string categories = selectedEvent.SubItems[5].Text;
                string restrictions = selectedEvent.SubItems[6].Text;
                
                DialogResult result = MessageBox.Show(
                $"Event Details:\n" +
                $"Name: {eventName}\n" +
                $"Date: {eventDate:dd-MM-yyyy}\n" +
                $"Time: {eventTime}\n" +
                $"Venue: {venueName}\n" +
                $"Price: {price}\n" +
                $"Categories: {categories}\n" +
                $"Restrictions: {restrictions}\n\n" +
                $"Do you want to RSVP to this event?",
                "Event Options",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Create RSVP in MongoDB
                        bool rsvpSuccess = MongoDBDataAccess.CreateRSVP(
                            Session.CurrentUser.Id, 
                            eventName, 
                            venueName, 
                            eventDate, 
                            "attending"
                        );
                        
                        if (rsvpSuccess)
                        {
                            MessageBox.Show("RSVP successful! You are now attending this event.", "RSVP Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error creating RSVP: {ex.Message}", 
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (result == DialogResult.No)
                {
                        //do something
                }
            }
        }

        //refreshes when searc event control is shown
        private void MongoSearchEvent_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                DisplayEvents(textBoxSearch.Text.Trim());
            }
        }

        //updates listb ox when search is updated
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            DisplayEvents(textBoxSearch.Text.Trim());
        }
    }
}
