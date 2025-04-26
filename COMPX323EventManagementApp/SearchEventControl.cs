using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using COMPX323EventManagementApp.Models;
using Oracle.ManagedDataAccess.Client;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace COMPX323EventManagementApp
{
    public partial class SearchEventControl : UserControl
    {
        public SearchEventControl()
        {
            InitializeComponent();

            try
            {
                using (var conn = DbConfig.GetConnection())
                {
                    conn.Open();

                    // Search for events and display all events details when form opens
                    using (var cmd = conn.CreateCommand())
                    {
                        
                        cmd.CommandText = "select ename, description, creation_date, company_club from event";

                        using (var reader = cmd.ExecuteReader())
                        {
                            // Setup listView
                            listViewEvents.View = View.Details;
                            listViewEvents.Columns.Add("Name", 150);
                            listViewEvents.Columns.Add("Description", 300);
                            listViewEvents.Columns.Add("Date", 100);
                            listViewEvents.Columns.Add("Club", 120);

                            // Add items to ListView
                            while (reader.Read())
                            {
                                var item = new ListViewItem(reader.GetString(0));
                                item.SubItems.Add(reader.GetString(1));
                                item.SubItems.Add(reader.GetDateTime(2).ToShortDateString());
                                item.SubItems.Add(reader.GetString(3));
                                listViewEvents.Items.Add(item);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
            
        }
    }
}
