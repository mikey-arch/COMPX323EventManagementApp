using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPX323EventManagementApp.Models
{
    public class Event
    {
        public string Ename { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string CompanyClub { get; set; }
        
        public Event()
        {
            CreationDate = DateTime.Now;
        }
    }
}
