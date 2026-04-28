using Microsoft.AspNetCore.Mvc;
using Dapper;

[ApiController]
[Route("api/checkin")]
public class CheckInController : ControllerBase
{
    private readonly DbConnection _db;

    public CheckInController(DbConnection db)
    {
        _db = db;
    }

    [HttpPut("{id}")]
    public IActionResult CheckIn(int id)
    {
        try
        {
            using var conn = _db.CreateConnection();

            var booking = conn.QueryFirstOrDefault(@"
                SELECT * FROM bookings WHERE id = @id
            ", new { id });

            if (booking == null)
                return NotFound("Invalid Booking ID");

            if (booking.status == "Checked-in")
                return BadRequest("Already checked-in");

            if (booking.status == "Checked-out")
                return BadRequest("Already checked-out");

            conn.Execute(@"
                UPDATE bookings 
                SET status = 'Checked-in' 
                WHERE id = @id
            ", new { id });

            return Ok("Check-in successful");
        }
        catch (Exception ex)
        {
            return Content("ERROR: " + ex.ToString());
        }
    }
}