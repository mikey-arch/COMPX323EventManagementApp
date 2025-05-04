using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPX323EventManagementApp.Models
{
    public class EventInstance
    {
        public DateTime EventDate { get; set; }
        public string Vname { get; set; }
        public string Ename { get; set; }
        public decimal Price { get; set; }
        public DateTime Time { get; set; }
    }
}
