using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPX323EventManagementApp.Models
{
    public class Attendee : User
    {
        public string PaymentStatus { get; set; }
        public DateTime Dob { get; set; }
    }
}
