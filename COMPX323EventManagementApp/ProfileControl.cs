using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using COMPX323EventManagementApp.Models;

namespace COMPX323EventManagementApp
{
    public partial class ProfileControl : UserControl
    {
        public ProfileControl()
        {
            InitializeComponent();

            User user = Session.CurrentUser;

            labelAccountNum.Text = user.Id.ToString();
            labelName.Text = user.Fname + " " + user.Lname;
            labelEmail.Text = user.Email;
            
        }

    }
}
