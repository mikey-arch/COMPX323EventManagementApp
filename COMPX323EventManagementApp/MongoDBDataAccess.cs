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
    }
}
