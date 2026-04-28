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
    // ✅ GET ALL BOOKINGS
[HttpGet]
public IActionResult GetAll()
{
    using var conn = _db.CreateConnection();

    var data = conn.Query(@"
        SELECT 
            b.*,
            c.name AS cottage_name,
            bt.name AS boat_name
        FROM bookings b
        LEFT JOIN cottages c ON b.cottage_id = c.id
        LEFT JOIN boats bt ON b.boat_id = bt.id
    ");

    return Ok(data);
}

    // ✅ GET ALL BOOKINGS
    [HttpGet("{id}")]
public IActionResult GetById(int id)
{
    using var conn = _db.CreateConnection();

    var data = conn.QueryFirstOrDefault(@"
        SELECT 
            b.*,
            c.name AS cottage_name,
            bt.name AS boat_name
        FROM bookings b
        LEFT JOIN cottages c ON b.cottage_id = c.id
        LEFT JOIN boats bt ON b.boat_id = bt.id
        WHERE b.id = @id
    ", new { id });

    if (data == null)
        return NotFound(new { message = "Booking not found" });

    return Ok(data);
}

    // ✅ CREATE BOOKING
    [HttpPost]
    public IActionResult Book([FromBody] Booking b)
    {
        try
        {
            using var conn = _db.CreateConnection();

            // 🔥 VALIDATION
            if (b == null ||
                string.IsNullOrEmpty(b.customer_name) ||
                string.IsNullOrEmpty(b.contact) ||
                string.IsNullOrEmpty(b.address) ||
                string.IsNullOrEmpty(b.date))
            {
                return BadRequest(new { message = "Missing required fields" });
            }

            // 📅 DATE VALIDATION
            if (!DateTime.TryParse(b.date, out DateTime bookingDate))
                return BadRequest(new { message = "Invalid date format" });

            if (bookingDate.Date < DateTime.Today)
                return BadRequest(new { message = "Cannot book past dates" });

            // 🔒 CHECK DUPLICATE COTTAGE
            var cottageExists = conn.ExecuteScalar<int>(@"
                SELECT COUNT(*) FROM bookings
                WHERE DATE(date) = DATE(@date)
                AND cottage_id = @cid
                AND status != 'Checked-out'
            ", new
            {
                date = bookingDate,
                cid = b.cottage_id
            });

            if (cottageExists > 0)
                return BadRequest(new { message = "Cottage already booked for this date" });

            // 🔒 CHECK DUPLICATE BOAT (ONLY IF SELECTED)
            if (b.boat_id != null)
            {
                var boatExists = conn.ExecuteScalar<int>(@"
                    SELECT COUNT(*) FROM bookings
                    WHERE DATE(date) = DATE(@date)
                    AND boat_id = @bid
                    AND status != 'Checked-out'
                ", new
                {
                    date = bookingDate,
                    bid = b.boat_id
                });

                if (boatExists > 0)
                    return BadRequest(new { message = "Boat already booked for this date" });
            }

            // 💰 VALIDATE TOTAL
            if (b.total <= 0)
                return BadRequest(new { message = "Invalid total amount" });

            // ✅ INSERT BOOKING
                var bookingId = conn.ExecuteScalar<int>(@"
               INSERT INTO bookings
               (customer_name, contact, address, date, num_people, cottage_id, boat_id, total, status)
               VALUES(@customer_name, @contact, @address, @date, @num_people, @cottage_id, @boat_id, @total, 'Booked');

               SELECT LAST_INSERT_ID();
               ", new
            {
               b.customer_name,
               b.contact,
               b.address,
               date = bookingDate,
               b.num_people,
               b.cottage_id,
               b.boat_id,
               b.total
            });

            // ✅ RETURN JSON (IMPORTANT)
            return Ok(new
            {
                message = "Booking successful",
                booking_id = bookingId
            });
        }
        catch (Exception ex)
        {
            // 🔥 ALWAYS RETURN JSON
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPut("status/{id}")]
public IActionResult UpdateStatus(int id, [FromBody] Dictionary<string, string> body)
{
    using var conn = _db.CreateConnection();

    if (body == null || !body.ContainsKey("status") || string.IsNullOrEmpty(body["status"]))
        return BadRequest(new { message = "Invalid status" });

    var rows = conn.Execute(
        "UPDATE bookings SET status=@s WHERE id=@id",
        new { id, s = body["status"] });

    if (rows == 0)
        return NotFound(new { message = "Booking not found or not updated" });

    return Ok(new { message = "Status updated" });
}
    [HttpDelete("{id}")]
public IActionResult Delete(int id)
{
    using var conn = _db.CreateConnection();

    var rows = conn.Execute("DELETE FROM bookings WHERE id=@id", new { id });

    if (rows == 0)
        return NotFound("Not found");

    return Ok("Deleted");
}
}