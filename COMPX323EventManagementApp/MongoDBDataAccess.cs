using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using COMPX323EventManagementApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace COMPX323EventManagementApp
{
    /// <summary>
    /// Handles all database interactions with MongoDB collections (events, reviews, rsvps, members)
    /// </summary>
    public static class MongoDBDataAccess
    {

        /// <summary>
        /// Retrieves all unique venue names from event instances across all events
        /// </summary>
        /// <returns>List of unique venue names sorted alphabetically</returns>
        public static List<string> GetVenues()
        {
            try
            {
                var eventsCollection = MongoDbConfig.GetCollection<BsonDocument>("events");

                var pipeline = new BsonDocument[]
                {
                    new BsonDocument("$unwind", "$instances"),
                    new BsonDocument("$group", new BsonDocument("_id", "$instances.venue.vname")),
                    new BsonDocument("$sort", new BsonDocument("_id", 1))
                };

                var result = eventsCollection.Aggregate<BsonDocument>(pipeline).ToList();
                return result.Select(doc => doc["_id"].AsString).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving venues: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves detailed event information for events created by a specific user
        /// </summary>
        /// <param name="userId">The creator's account number</param>
        /// <returns>List of tuples containing event name, description, and formatted creation date</returns>
        public static List<(string ename, string description, string creationDate)> GetUserCreatedEvents(int userId)
        {
            try
            {
                var eventsCollection = MongoDbConfig.GetCollection<BsonDocument>("events");

                var pipeline = new BsonDocument[]
                {
                new BsonDocument("$match", new BsonDocument("creatorNum", userId)),
                new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 0 },
                    { "ename", 1 },
                    { "description", 1 },
                    { "creationDate", 1 }
                }),
                new BsonDocument("$sort", new BsonDocument("ename", 1))
                };

                var result = eventsCollection.Aggregate<BsonDocument>(pipeline).ToList();

                return result.Select(doc => (
                    doc.GetValue("ename", "").AsString,
                    doc.GetValue("description", "").AsString,
                    doc.GetValue("creationDate", BsonNull.Value).ToLocalTime().ToString("yyyy-MM-dd")
                )).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving events for user {userId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves all reviews written by a specific user
        /// </summary>
        /// <param name="userId">The reviewer's account number</param>
        /// <returns>List of tuples containing review details</returns>
        public static List<(string ename, string venue, string eventDate, int rating, string review)> GetUserCreatedReviews(int userId)
        {
            try
            {
                var reviewsCollection = MongoDbConfig.GetCollection<BsonDocument>("reviews");

                var pipeline = new BsonDocument[]
                {
            new BsonDocument("$match", new BsonDocument("accNum", userId)),
            new BsonDocument("$project", new BsonDocument
            {
                { "_id", 0 },
                { "ename", 1 },
                { "vname", 1 },
                { "eventDate", 1 },
                { "rating", 1 },
                { "textReview", 1 }
            }),
            new BsonDocument("$sort", new BsonDocument("eventDate", 1))
                };

                var result = reviewsCollection.Aggregate<BsonDocument>(pipeline).ToList();

                return result.Select(doc => (
                    doc.GetValue("ename", "").AsString,
                    doc.GetValue("vname", "").AsString,
                    doc.GetValue("eventDate", BsonNull.Value).ToLocalTime().ToString("yyyy-MM-dd"),
                    doc.GetValue("rating", new BsonInt32(0)).AsInt32,
                    doc.GetValue("textReview", "").AsString
                )).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving reviews for user {userId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves all RSVPs made by a specific user
        /// </summary>
        /// <param name="userId">The user's account number</param>
        /// <returns>List of tuples containing RSVP details</returns>
        public static List<(string ename, string eventDate, string venue, string status)> GetUserRsvps(int userId)
        {
            try
            {
                var rsvpsCollection = MongoDbConfig.GetCollection<BsonDocument>("rsvps");

                var pipeline = new BsonDocument[]
                {
            new BsonDocument("$match", new BsonDocument("accNum", userId)),
            new BsonDocument("$project", new BsonDocument
            {
                { "_id", 0 },
                { "ename", 1 },
                { "eventDate", 1 },
                { "vname", 1 },
                { "status", 1 }
            }),
            new BsonDocument("$sort", new BsonDocument("eventDate", 1))
                };

                var result = rsvpsCollection.Aggregate<BsonDocument>(pipeline).ToList();

                return result.Select(doc => (
                    doc.GetValue("ename", "").AsString,
                    doc.GetValue("eventDate", BsonNull.Value).ToLocalTime().ToString("yyyy-MM-dd"),
                    doc.GetValue("vname", "").AsString,
                    doc.GetValue("status", "n/a").AsString
                )).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving RSVPs for user {userId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get events created by a specific user
        /// </summary>
        /// <param name="creatorNum">Creator's account number</param>
        /// <returns>List of event names</returns>
        public static List<string> GetEventsByCreator(int creatorNum)
        {
            try
            {
                var eventsCollection = MongoDbConfig.GetCollection<BsonDocument>("events");
                
                var filter = new BsonDocument("creatorNum", creatorNum);
                var events = eventsCollection.Find(filter).ToList();
                
                return events.Select(doc => doc["ename"].AsString).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user events: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Create a new event in MongoDB if the event already exists then add event instance to that event
        /// </summary>
        /// <param name="eventDocument">The event document to create</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool CreateEvent(BsonDocument eventDocument)
        {
            try
            {
                var eventsCollection = MongoDbConfig.GetCollection<BsonDocument>("events");

                string eventName = eventDocument["ename"].AsString;
                var filter = new BsonDocument("ename", eventDocument["ename"].AsString);
                var existingEvent = eventsCollection.Find(filter).FirstOrDefault();
                
                // the event already exists , so we instead add a new instance to the exising event
                if (existingEvent != null)
                {
                    var newInstance = eventDocument["instances"].AsBsonArray[0]; // Get the new instance
            
                    // Check if this exact instance already exists (same date, venue)
                    var existingInstances = existingEvent["instances"].AsBsonArray;
                    var newInstanceDate = newInstance["eventDate"].ToUniversalTime();
                    var newInstanceVenue = newInstance["venue"]["vname"].AsString;
                    
                    foreach (var instance in existingInstances)
                    {
                        var existingDate = instance["eventDate"].ToUniversalTime();
                        var existingVenue = instance["venue"]["vname"].AsString;
                        
                        if (existingDate.Date == newInstanceDate.Date && existingVenue == newInstanceVenue)
                        {
                            return false; // Instance already exists for this date and venue
                        }
                    }
                    
                    // Add the new instance to the existing event
                    var update = Builders<BsonDocument>.Update.Push("instances", newInstance);
                    var result = eventsCollection.UpdateOne(filter, update);
                    
                    return result.ModifiedCount > 0;
                }
                else
                {
                    // Event doesn't exist - create new event
                    eventsCollection.InsertOne(eventDocument);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating event: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Check if an event name already exists
        /// </summary>
        /// <param name="eventName">Event name to check</param>
        /// <returns>True if exists, false otherwise</returns>
        public static bool EventNameExists(string eventName)
        {
            try
            {
                var eventsCollection = MongoDbConfig.GetCollection<BsonDocument>("events");
                
                var filter = new BsonDocument("ename", eventName);
                var count = eventsCollection.CountDocuments(filter);
                
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking event name: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// Gets detailed event information for auto-population
        /// Returns event description, categories, and restriction
        /// </summary>
        /// <param name="eventName">Name of the event to retrieve</param>
        /// <returns>Event details or null if not found</returns>
        public static (string description, List<string> categories, string restriction)? GetEventDetails(string eventName)
        {
            try
            {
                var eventsCollection = MongoDbConfig.GetCollection<BsonDocument>("events");
                
                var filter = new BsonDocument("ename", eventName);
                var eventDoc = eventsCollection.Find(filter).FirstOrDefault();
                
                if (eventDoc != null)
                {
                    string description = eventDoc.GetValue("description", "").AsString;
                    string restriction = eventDoc.GetValue("restriction", new BsonDocument()).AsBsonDocument.GetValue("name", "").AsString;
                    
                    // Extract category names from categories array
                    var categoriesArray = eventDoc.GetValue("categories", new BsonArray()).AsBsonArray;
                    var categories = new List<string>();
                    
                    foreach (var categoryDoc in categoriesArray)
                    {
                        categories.Add(categoryDoc.AsBsonDocument.GetValue("name", "").AsString);
                    }
                    
                    return (description, categories, restriction);
                }
                
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving event details: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets detailed venue information for auto-population
        /// Returns venue capacity, address details, etc.
        /// </summary>
        /// <param name="venueName">Name of the venue to retrieve</param>
        /// <returns>Venue details or null if not found</returns>
        public static (int capacity, int streetNum, string streetName, string suburb, string city, string postcode, string country)? GetVenueDetails(string venueName)
        {
            try
            {
                var eventsCollection = MongoDbConfig.GetCollection<BsonDocument>("events");
                
                // Use aggregation to find venue details from any event instance
                var pipeline = new BsonDocument[]
                {
                    new BsonDocument("$unwind", "$instances"),
                    new BsonDocument("$match", new BsonDocument("instances.venue.vname", venueName)),
                    new BsonDocument("$limit", 1),
                    new BsonDocument("$project", new BsonDocument("venue", "$instances.venue"))
                };
                
                var result = eventsCollection.Aggregate<BsonDocument>(pipeline).FirstOrDefault();
                
                if (result != null)
                {
                    var venueDoc = result.GetValue("venue", new BsonDocument()).AsBsonDocument;
                    
                    return (
                        capacity: venueDoc.GetValue("capacity", 0).AsInt32,
                        streetNum: venueDoc.GetValue("streetNum", 0).AsInt32,
                        streetName: venueDoc.GetValue("streetName", "").AsString,
                        suburb: venueDoc.GetValue("suburb", "").AsString,
                        city: venueDoc.GetValue("city", "").AsString,
                        postcode: venueDoc.GetValue("postcode", "").AsString,
                        country: venueDoc.GetValue("country", "New Zealand").AsString
                    );
                }
                
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving venue details: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if a venue exists in any event instance
        /// </summary>
        /// <param name="venueName">Name of the venue to check</param>
        /// <returns>True if venue exists, false otherwise</returns>
        public static bool VenueExists(string venueName)
        {
            try
            {
                var eventsCollection = MongoDbConfig.GetCollection<BsonDocument>("events");
                
                var pipeline = new BsonDocument[]
                {
                    new BsonDocument("$unwind", "$instances"),
                    new BsonDocument("$match", new BsonDocument("instances.venue.vname", venueName)),
                    new BsonDocument("$limit", 1)
                };
                
                var result = eventsCollection.Aggregate<BsonDocument>(pipeline).FirstOrDefault();
                return result != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking if venue exists: {ex.Message}", ex);
            }
        }

    }
}
