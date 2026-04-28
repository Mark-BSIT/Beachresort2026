public class UpdateItemRequest
{
    public string name { get; set; }
    public decimal price { get; set; }
    public string? image { get; set; }   // OPTIONAL
}