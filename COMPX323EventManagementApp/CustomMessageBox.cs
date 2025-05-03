using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMPX323EventManagementApp
{
    public partial class CustomMessageBox : Form
    {
        public string SelectedOption { get; private set; }
        public CustomMessageBox()
        {
            InitializeComponent();
        }

        private void buttonDelRSVP_Click(object sender, EventArgs e)
        {
            SelectedOption = "Delete RSVP";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonEventDetails_Click(object sender, EventArgs e)
        {
            SelectedOption = "View Event Details";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void labelExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
