using Microsoft.AspNetCore.Mvc;
using Dapper;

[ApiController]
[Route("api/[controller]")]
public class CheckInController : ControllerBase
{
    private readonly DbConnection _db;

    public CheckInController(DbConnection db)
    {
        _db = db;
    }

    [HttpPost]
    public IActionResult CheckIn([FromBody] dynamic data)
    {
        using var conn = _db.CreateConnection();

        var booking = conn.QueryFirstOrDefault(
            "SELECT * FROM bookings WHERE id = @id",
            new { id = data.bookingId }
        );

        if (booking == null)
            return NotFound("Invalid Booking ID");

        // update status
        conn.Execute(
            "UPDATE bookings SET status = 'Confirmed' WHERE id = @id",
            new { id = data.bookingId }
        );

        return Ok("Check-in successful");
    }
}