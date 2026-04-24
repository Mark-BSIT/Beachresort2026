using Microsoft.AspNetCore.Mvc;
using Dapper;

[ApiController]
[Route("api/[controller]")]
public class CheckOutController : ControllerBase
{
    private readonly DbConnection _db;

    public CheckOutController(DbConnection db)
    {
        _db = db;
    }

    [HttpPost]
    public IActionResult CheckOut([FromBody] dynamic data)
    {
        using var conn = _db.CreateConnection();

        var booking = conn.QueryFirstOrDefault(
            "SELECT * FROM bookings WHERE id = @id",
            new { id = data.bookingId }
        );

        if (booking == null)
            return NotFound("Invalid Booking");

        var pricing = conn.QueryFirst("SELECT * FROM pricing LIMIT 1");

        decimal total =
            (booking.num_people * pricing.entry_fee) +
            pricing.cottage_price +
            pricing.boat_price;

        decimal cash = data.cash;

        if (cash < total)
            return BadRequest("Not enough cash");

        decimal change = cash - total;

        // save transaction
        conn.Execute(@"
            INSERT INTO transactions (booking_id, total_amount, cash, change_amount, date)
            VALUES (@b, @t, @c, @ch, NOW())
        ", new
        {
            b = data.bookingId,
            t = total,
            c = cash,
            ch = change
        });

        return Ok(new
        {
            total,
            change
        });
    }
}