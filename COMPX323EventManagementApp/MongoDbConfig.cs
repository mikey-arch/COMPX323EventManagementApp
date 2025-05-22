using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;

namespace COMPX323EventManagementApp
{
    /// <summary>
    /// This class maanges the MongoDB database connections
    /// </summary>
    public static class MongoDbConfig
    {
        // Gets MongoDB connection string from app.config
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["MongoDbConnection"].ConnectionString;

        private static IMongoDatabase _database;

        /// <summary>
        /// Helper function to get a fresh connection
        /// </summary>
        /// <returns> A new MongoDB database</returns>
        public static IMongoDatabase GetDatabase()
        {
            if (_database == null)
            {
                var client = new MongoClient(ConnectionString);
                var url = MongoUrl.Create(ConnectionString);
                _database = client.GetDatabase(url.DatabaseName);
            }
            return _database;
        }

        /// <summary>
        /// Helper function to get a collection from the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <returns> the collection</returns>
        public static IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return GetDatabase().GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Test the connection to the database by running a ping command
        /// </summary>
        /// <returns> true if connection successful , else false</returns>
        public static bool TestConnection()
        {
            try
            {
                var database = GetDatabase();
                database.RunCommand<object>("{ ping: 1 }");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"MongoDB Connection Failed: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


    }
}
