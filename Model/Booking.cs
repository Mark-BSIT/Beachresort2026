public class Booking
{
    public string customer_name { get; set; }
    public string contact { get; set; }
    public string address { get; set; }
    public string date { get; set; }
    public int num_people { get; set; }
    public int cottage_id { get; set; }
    public int? boat_id { get; set; }
    public decimal total { get; set; }
}