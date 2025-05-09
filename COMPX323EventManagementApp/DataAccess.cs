using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMPX323EventManagementApp.Models;
using Oracle.ManagedDataAccess.Client;

namespace COMPX323EventManagementApp
{
    public static class DataAccess
    {
        //gets the events for the user thats organising the event
        public static List<string> GetOrganiserEvents(int userId)
        {
            List<string> events = new List<string>();

            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"select ename from Event where creator_num = :userId"; 
                        cmd.Parameters.Add("userId", OracleDbType.Int32).Value = userId;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                events.Add(reader["ename"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving users events: {ex.Message}", ex);
            }
            return events;
        }

       
        //gets all venues
        public static List<string> GetAllVenues()
        {
            List<string> venues = new List<string>();

            try
            {
                using(var conn= DbConfig.GetConnection()) 
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand()) 
                    {
                        cmd.CommandText = "select vname from Venue";

                        using(var reader = cmd.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                venues.Add(reader["vname"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving venues: {ex.Message}", ex);
            }
            return venues;
        }

        //gets events details
        public static Event GetEvent (string eventName)
        {
            Event eventObj = null;

            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select description, restriction, creator_num from event where ename = :eventName";
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;

                        using(var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                eventObj = new Event
                                {
                                    Ename = eventName,
                                    Description = reader["description"].ToString(),
                                    Restriction = reader["restriction"].ToString(),
                                    CreatorNum = Convert.ToInt32(reader["creator_num"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving events: {ex.Message}", ex);
            }
            return eventObj;
        }

         // Get categories for an event
        public static List<string> GetEventCategories(string eventName)
        {
            List<string> categories = new List<string>();
            
            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select cname from Event_Category where ename = :eventName";
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.Add(reader["cname"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving event categories: {ex.Message}", ex);
            }
            
            return categories;
        }
        
        // Get restriction for an event
        public static string GetEventRestriction(string eventName)
        {
            string restriction = null;
            
            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select restriction from event where ename = :eventName";
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                restriction = reader["restriction"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving event restriction: {ex.Message}", ex);
            }
            
            return restriction;
        }
        
        // Get venue details
        public static Venue GetVenue(string venueName)
        {
            Venue venue = null;
            
            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"select capacity, street_num, street_name, suburb, 
                                           city, postcode, country from Venue 
                                           where vname = :venueName";
                                           
                        cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                venue = new Venue
                                {
                                    Vname = venueName,
                                    Capacity = Convert.ToInt32(reader["capacity"]),
                                    StreetNum = Convert.ToInt32(reader["street_num"]),
                                    StreetName = reader["street_name"].ToString(),
                                    Suburb = reader["suburb"].ToString(),
                                    City = reader["city"].ToString(),
                                    Postcode = reader["postcode"].ToString(),
                                    Country = reader["country"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving venue details: {ex.Message}", ex);
            }
            
            return venue;
        }

        private static void CreateVenue(OracleConnection conn, Venue venue)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"insert into Venue (vname, capacity, street_num, street_name, suburb, city, postcode, country) 
                                  values (:venueName, :capacity, :streetNum, :streetName, :suburb, :city, :postcode, :country)";
                                  
                cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venue.Vname;
                cmd.Parameters.Add("capacity", OracleDbType.Int32).Value = venue.Capacity;
                cmd.Parameters.Add("streetNum", OracleDbType.Int32).Value = venue.StreetNum;
                cmd.Parameters.Add("streetName", OracleDbType.Varchar2).Value = venue.StreetName;
                cmd.Parameters.Add("suburb", OracleDbType.Varchar2).Value = venue.Suburb;
                cmd.Parameters.Add("city", OracleDbType.Varchar2).Value = venue.City;
                cmd.Parameters.Add("postcode", OracleDbType.Varchar2).Value = venue.Postcode;
                cmd.Parameters.Add("country", OracleDbType.Varchar2).Value = venue.Country;
                
                cmd.ExecuteNonQuery();
            }
        }
        
        private static void CreateEvent(OracleConnection conn, Event eventObj)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"insert into Event (ename, description, creation_date, restriction, creator_num) 
                                  values (:eventName, :description, DEFAULT, :restriction, :creatorNum)";
                                  
                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventObj.Ename;
                cmd.Parameters.Add("description", OracleDbType.Clob).Value = eventObj.Description;
                cmd.Parameters.Add("restriction", OracleDbType.Clob).Value = eventObj.Restriction;
                cmd.Parameters.Add("creatorNum", OracleDbType.Int32).Value = eventObj.CreatorNum;
                
                cmd.ExecuteNonQuery();
            }
        }

        // Check if venue exists
        public static bool VenueExists(string venueName)
        {
            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM Venue WHERE vname = :venueName";
                        cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                        
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking if venue exists: {ex.Message}", ex);
            }
        }
        
        // Check if event exists for organizer
        public static bool EventExistsForOrganizer(string eventName, int organizerId)
        {
            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT COUNT(*) FROM Event WHERE ename = :eventName AND creator_num = :organizerId";
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                        cmd.Parameters.Add("organizerId", OracleDbType.Int32).Value = organizerId;
                        
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking if event exists for organizer: {ex.Message}", ex);
            }
        }
        
        // Check if event name is taken
        public static bool EventNameTaken(string eventName)
        {
            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM Event WHERE ename = :eventName";
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                        
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking if event name is taken: {ex.Message}", ex);
            }
        }
        
        // Check if event instance exists
        public static bool EventInstanceExists(string eventName, string venueName, DateTime eventDate)
        {
            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT COUNT(*) FROM Event_Instance 
                                           WHERE event_date = :eventDate AND vname = :venueName AND ename = :eventName";
                        cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = eventDate;
                        cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = venueName;
                        cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                        
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking if event instance exists: {ex.Message}", ex);
            }
        }
        
        private static void AssignCategoryToEvent(OracleConnection conn, string category, string eventName)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Event_Category (cname, ename) VALUES (:categoryName, :eventName)";
                cmd.Parameters.Add("categoryName", OracleDbType.Varchar2).Value = category;
                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = eventName;
                
                cmd.ExecuteNonQuery();
            }
        }
        private static void CreateEventInstance(OracleConnection conn, EventInstance instance)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO Event_Instance (event_date, vname, ename, price, time) 
                                  VALUES (:eventDate, :venueName, :eventName, :price, :eventTime)";
                                  
                cmd.Parameters.Add("eventDate", OracleDbType.Date).Value = instance.EventDate;
                cmd.Parameters.Add("venueName", OracleDbType.Varchar2).Value = instance.Vname;
                cmd.Parameters.Add("eventName", OracleDbType.Varchar2).Value = instance.Ename;
                cmd.Parameters.Add("price", OracleDbType.Decimal).Value = instance.Price;
                cmd.Parameters.Add("eventTime", OracleDbType.TimeStamp).Value = instance.Time;
                
                cmd.ExecuteNonQuery();
            }
        }

         // Create or update event with transaction handling
        public static bool CreateOrUpdateEvent(Event eventObj, EventInstance instance, Venue venue, 
            List<string> categories, int organizerId)
        {
            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();
                    
                    // Begin transaction
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Create venue if it doesn't exist
                            if (!VenueExists(venue.Vname))
                            {
                                CreateVenue(conn, venue);
                            }
                            
                            // Check if event exists for this organizer
                            bool eventExists = EventExistsForOrganizer(eventObj.Ename, organizerId);
                            
                            // If event doesn't exist, create it and relationships
                            if (!eventExists)
                            {
                                // Check if any other organizer has this event name
                                if (EventNameTaken(eventObj.Ename))
                                {
                                    return false; // Event name taken
                                }

                                eventObj.CreatorNum = organizerId;
                                
                                CreateEvent(conn, eventObj);
                                
                                // Assign categories to event
                                foreach (string category in categories)
                                {
                                    AssignCategoryToEvent(conn, category, eventObj.Ename);
                                }
                            }
                            
                            // Check if this event instance already exists
                            if (EventInstanceExists(eventObj.Ename, venue.Vname, instance.EventDate))
                            {
                                return false; 
                            }
                            
                            CreateEventInstance(conn, instance);
                            transaction.Commit();
                            
                            return true;
                        }
                        catch (Exception)
                        {
                            // Rollback transaction on error
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating or updating event: {ex.Message}", ex);
            }
        }
    }
}
