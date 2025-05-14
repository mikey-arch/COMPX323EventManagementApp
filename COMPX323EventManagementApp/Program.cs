using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using COMPX323EventManagementApp.Models;

namespace COMPX323EventManagementApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Test the database connection before starting the application
            if (DbConfig.TestConnection())
            {
                // Skip Logging in while working on program
                Application.Run(new LoginForm());

                //Session.CurrentUser = new Member
                //{
                //    Id = 999,
                //    Fname = "Dev",
                //    Lname = "User",
                //    Email = "dev@example.com",
                //    PhoneNum = "0210000000",
                //    DOB = DateTime.Now
                //};
                //
                //Application.Run(new EventsManagerForm());
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
