using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace COMPX323EventManagementApp
{
    /// <summary>
    ///  This class manages the Oracle database connections
    /// </summary>
    public static class DbConfig
    {
        // Gets Oracle connection string from app.config
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;

        /// <summary>
        /// Helper function to get a fresh connection
        /// </summary>
        /// <returns> A new OracleConnection</returns>
        public static OracleConnection GetConnection()
        {
            OracleConnection conn = new OracleConnection(ConnectionString);
            return conn;
        }

        /// <summary>
        /// Test the connection to the database by opening and closing a connection
        /// Displays appropriate error message if connection fails
        /// </summary>
        /// <returns> True if connection is successful, else false </returns>
        public static bool TestConnection()
        {
            try
            {
                using (OracleConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Test Connection Failed: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
