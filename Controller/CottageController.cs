using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Data;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CottageController : ControllerBase
{
    private readonly DbConnection _db;

    public CottageController(DbConnection db)
    {
        _db = db;
    }

    // ✅ MODEL (PUT HERE OR CREATE SEPARATE FILE)
    public class Cottage
    {
        public string name { get; set; }
        public decimal price { get; set; }
        public int total_units { get; set; }
        public string image { get; set; }
    }
    [HttpGet]
public IActionResult GetAll()
{
    using var conn = _db.CreateConnection();

    var data = conn.Query("SELECT * FROM cottages");

    return Ok(data);
}

    [HttpPost]
public IActionResult Add([FromBody] Cottage c)
{
    try
    {
        using var conn = _db.CreateConnection();

        conn.Execute(@"
            INSERT INTO cottages(name, price, total_units, image)
            VALUES(@name,@price,@total_units,@image)
        ", c);

        return Ok("Saved");
    }
    catch(Exception ex)
    {
        // 🔥 FORCE FULL ERROR RETURN
        return Content("ERROR: " + ex.ToString());
    }
}
    // ✅ AVAILABILITY (DATE BASED)
    [HttpGet("availability")]
    public IActionResult GetAvailability(string date)
    {
        using var conn = _db.CreateConnection();

        int totalCottages = conn.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM cottages"
        );

        int bookedCottages = conn.ExecuteScalar<int>(@"
            SELECT COUNT(DISTINCT cottage_id)
            FROM bookings
            WHERE date = @date AND status='Booked'
        ", new { date });

        int totalBoats = conn.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM boats"
        );

        int bookedBoats = conn.ExecuteScalar<int>(@"
            SELECT COUNT(DISTINCT boat_id)
            FROM bookings
            WHERE date = @date AND status='Booked'
        ", new { date });

        return Ok(new
        {
            cottages = new
            {
                available = totalCottages - bookedCottages,
                total = totalCottages
            },
            boats = new
            {
                available = totalBoats - bookedBoats,
                total = totalBoats
            }
        });
    }
    // ✅ UPDATE COTTAGE (ADD THIS)
    [HttpPut("{id}")]
public IActionResult Update(int id, [FromBody] UpdateItemRequest c)
{
    using var conn = _db.CreateConnection();

    if (string.IsNullOrWhiteSpace(c.name))
        return BadRequest("Name is required");

    if (c.image == null)
    {
        // update without image
        conn.Execute(@"
            UPDATE cottages
            SET name=@name, price=@price
            WHERE id=@id
        ", new { id, name = c.name, price = c.price });
    }
    else
    {
        // update with image
        conn.Execute(@"
            UPDATE cottages
            SET name=@name, price=@price, image=@image
            WHERE id=@id
        ", new { id, name = c.name, price = c.price, image = c.image });
    }

    return Ok("Updated");
}
[HttpDelete("{id}")]
public IActionResult Delete(int id)
{
    using var conn = _db.CreateConnection();

    conn.Execute("DELETE FROM cottages WHERE id=@id", new { id });

    return Ok("Deleted");
}
[HttpPost("upload")]
public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
{
    try
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        // 📁 Create folder if not exists
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        // 🔥 Unique filename
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        var fullPath = Path.Combine(folderPath, fileName);

        // 💾 Save file
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { image = fileName });
    }
    catch (Exception ex)
    {
        return Content("UPLOAD ERROR: " + ex.ToString());
    }
}
}