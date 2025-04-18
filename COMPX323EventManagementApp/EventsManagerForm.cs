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
    public partial class EventsManagerForm : Form
    {

        private SearchEventControl searchControl;
        private ProfileControl profileControl;
        private CreateEventControl createEventControl;
        private ManageEventControl manageEventControl;
        private CreateReviewControl reviewControl;

        public EventsManagerForm()
        {
            InitializeComponent();
            //initialise all the user control isntances
            searchControl = new SearchEventControl();
            profileControl = new ProfileControl();
            createEventControl = new CreateEventControl();
            manageEventControl = new ManageEventControl();
            reviewControl = new CreateReviewControl();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "SEARCH FOR EVENT";
            panelContent.Controls.Clear();
            searchControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(searchControl);

        }

        private void buttonProfile_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "PROFILE";
            panelContent.Controls.Clear();
            profileControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(profileControl);

        }

        private void buttonCreateEvent_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "EVENT DETAILS";
            panelContent.Controls.Clear();
            createEventControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(createEventControl);

        }

        private void buttonManageEvent_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "MANAGE DETAILS";
            panelContent.Controls.Clear();
            manageEventControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(manageEventControl);

        }

        private void buttonCreateReview_Click(object sender, EventArgs e)
        {
            labelTitle.Text = "RATING AND REVIEWS";
            panelContent.Controls.Clear();
            reviewControl.Dock = DockStyle.Fill;
            panelContent.Controls.Add(reviewControl);

        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            //prob decide whether exit or back to login
            LoginForm loginForm = new LoginForm();
            this.Hide();
            loginForm.ShowDialog();
            this.Close();
        }
    }
}
