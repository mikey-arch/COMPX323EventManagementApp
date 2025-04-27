using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPX323EventManagementApp.Models
{
    public class RSVP
    {
        public int AccNum { get; set; }
        public string EName { get; set; }
        public string VName { get; set; }
        public DateTime EventDate { get; set; }
        public string Status { get; set; }
        public DateTime RsvpTimestamp { get; set; }
        public override string ToString()
        {
            return $"{EName} at {VName} on {EventDate:dd/MM/yyyy}";
        }
    }
}
