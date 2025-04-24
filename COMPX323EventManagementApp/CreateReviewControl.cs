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
    public partial class CreateReviewControl : UserControl
    {
        public CreateReviewControl()
        {
            InitializeComponent();
        }

        private void textBoxReview_Enter(object sender, EventArgs e)
        {
            if(textBoxReview.Text == "Enter text here...")
            {
                textBoxReview.Text = "";
                textBoxReview.ForeColor = Color.Black;
            }

        }

        private void textBoxReview_Leave(object sender, EventArgs e)
        {
            if(textBoxReview.Text == "")
            {
                textBoxReview.Text = "Enter text here...";
                textBoxReview.ForeColor = Color.Gray;
            }

        }
    }
}
