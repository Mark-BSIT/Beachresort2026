public class Report
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Message { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}