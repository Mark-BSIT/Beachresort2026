using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeachResortAPIWinForm.Models
{
    public class Booking
    {
        public int id { get; set; }
        public string customer_name { get; set; }
        public string contact { get; set; }
        public string address { get; set; }
        public DateTime date { get; set; }
        public int num_people { get; set; }
        public int? cottage_id { get; set; }
        public int? boat_id { get; set; }
        public decimal total { get; set; }
        public string status { get; set; }
        public string item_type { get; set; }
        public int? item_id { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
    }
}
