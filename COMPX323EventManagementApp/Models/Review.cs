using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPX323EventManagementApp.Models
{
    public class Review
    {
        public int AccNum { get; set; }
        public string EName { get; set; }
        public string VName { get; set; }
        public DateTime EventDate { get; set; }
        public int Rating { get; set; }
        public string TextReview { get; set; }
        public DateTime ReviewTimestamp { get; set; }
    }
}
