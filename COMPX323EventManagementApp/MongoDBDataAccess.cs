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
    public static class MongoDBDataAccess
    {
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


    }
}
