using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                Application.Run(new LoginForm());
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
