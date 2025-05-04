using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPX323EventManagementApp.Models
{
    public class Venue
    {
        public string Vname { get; set; }
        public int Capacity { get; set; }
        public int StreetNum { get; set; }
        public string StreetName { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
    }
}
