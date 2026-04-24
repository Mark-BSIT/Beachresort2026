using Microsoft.AspNetCore.Mvc;
using Dapper;

[ApiController]
[Route("api/[controller]")]
public class CottageController : ControllerBase
{
    private readonly DbConnection _db;

    public CottageController(DbConnection db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Get()
    {
        using var conn = _db.CreateConnection();
        var data = conn.Query("SELECT * FROM cottages");
        return Ok(data);
    }

    [HttpPost]
    public IActionResult Add([FromBody] dynamic c)
    {
        using var conn = _db.CreateConnection();

        conn.Execute(@"INSERT INTO cottages(name, capacity, price, total_units)
                       VALUES(@name,@capacity,@price,@units)", new
        {
            name = c.name,
            capacity = c.capacity,
            price = c.price,
            units = c.total_units
        });

        return Ok("Added");
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] dynamic c)
    {
        using var conn = _db.CreateConnection();

        conn.Execute(@"UPDATE cottages SET name=@name, capacity=@capacity,
                       price=@price, total_units=@units WHERE id=@id", new
        {
            id,
            name = c.name,
            capacity = c.capacity,
            price = c.price,
            units = c.total_units
        });

        return Ok("Updated");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        using var conn = _db.CreateConnection();
        conn.Execute("DELETE FROM cottages WHERE id=@id", new { id });
        return Ok("Deleted");
    }
}
[HttpGet("availability")]
public IActionResult GetAvailability()
{
    using var conn = _db.CreateConnection();

    int totalCottages = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM cottages");

    int bookedCottages = conn.ExecuteScalar<int>(@"
        SELECT COUNT(DISTINCT cottage_id)
        FROM bookings
        WHERE date = CURDATE() AND status != 'Cancelled'
    ");

    int totalBoats = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM boats");

    int bookedBoats = conn.ExecuteScalar<int>(@"
        SELECT COUNT(DISTINCT boat_id)
        FROM bookings
        WHERE date = CURDATE() AND status != 'Cancelled'
    ");

    return Ok(new {
        cottages = new {
            available = totalCottages - bookedCottages,
            total = totalCottages
        },
        boats = new {
            available = totalBoats - bookedBoats,
            total = totalBoats
        }
    });
}