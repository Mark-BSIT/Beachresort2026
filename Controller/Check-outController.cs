using Microsoft.AspNetCore.Mvc;
using Dapper;

[ApiController]
[Route("api/checkout")]
public class CheckOutController : ControllerBase
{
    private readonly DbConnection _db;

    public CheckOutController(DbConnection db)
    {
        _db = db;
    }

    [HttpPut("{id}")]
    public IActionResult CheckOut(int id, [FromBody] PaymentRequest p)
    {
        try
        {
            using var conn = _db.CreateConnection();

            // 🔍 GET BOOKING
            var booking = conn.QueryFirstOrDefault(@"
                SELECT * FROM bookings WHERE id = @id
            ", new { id });

            if (booking == null)
                return NotFound("Invalid Booking");

            // ❌ prevent double checkout
            if (booking.status == "Checked-out")
                return BadRequest("Already checked out");

            decimal total = booking.total;
            decimal cash = p.cash;

            if (cash < total)
                return BadRequest("Not enough cash");

            decimal change = cash - total;

            // 💾 SAVE TRANSACTION
            conn.Execute(@"
                INSERT INTO transactions 
                (booking_id, total_amount, cash, change_amount, date)
                VALUES (@b, @t, @c, @ch, NOW())
            ", new
            {
                b = id,
                t = total,
                c = cash,
                ch = change
            });

            // 🔥 UPDATE STATUS
            conn.Execute(@"
                UPDATE bookings 
                SET status = 'Checked-out' 
                WHERE id = @id
            ", new { id });

            return Ok(new
            {
                message = "Checkout successful",
                total,
                cash,
                change
            });
        }
        catch (Exception ex)
        {
            return Content("ERROR: " + ex.ToString());
        }
    }
}