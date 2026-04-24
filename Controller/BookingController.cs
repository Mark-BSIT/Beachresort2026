using Microsoft.AspNetCore.Mvc;
using Dapper;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly DbConnection _db;

    public BookingController(DbConnection db)
    {
        _db = db;
    }

    // ✅ CREATE BOOKING
    [HttpPost]
    public IActionResult Book([FromBody] dynamic b)
    {
        using var conn = _db.CreateConnection();

        conn.Execute(@"INSERT INTO bookings(customer_name, contact, date, num_people, cottage_id, boat_id, status)
                       VALUES(@n,@c,@d,@p,@co,@bo,'Pending')", new
        {
            n = b.customer_name,
            c = b.contact,
            d = b.date,
            p = b.num_people,
            co = b.cottage_id,
            bo = b.boat_id
        });

        return Ok("Booked");
    }

    // ✅ GET ALL BOOKINGS (THIS FIXES YOUR ERROR)
    [HttpGet]
    public IActionResult GetAll()
    {
        using var conn = _db.CreateConnection();
        var data = conn.Query("SELECT * FROM bookings");
        return Ok(data);
    }

    // ✅ GET BY ID
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        using var conn = _db.CreateConnection();
        return Ok(conn.QueryFirstOrDefault("SELECT * FROM bookings WHERE id=@id", new { id }));
    }
}