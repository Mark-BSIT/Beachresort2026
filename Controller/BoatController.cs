using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

[ApiController]
[Route("api/[controller]")]
public class BoatController : ControllerBase
{
    private readonly DbConnection _db;

    public BoatController(DbConnection db)
    {
        _db = db;
    }

    // ✅ MODEL
    public class Boat
    {
        public string name { get; set; }
        public decimal price { get; set; }
        public int total_units { get; set; }
        public string image { get; set; }
    }
[HttpPost]
public IActionResult Add([FromBody] Boat b)
{
    try
    {
        using var conn = _db.CreateConnection();

        conn.Execute(@"
            INSERT INTO boats(name, price, total_units, image)
            VALUES(@name,@price,@total_units,@image)
        ", b);

        return Ok("Saved");
    }
    catch(Exception ex)
    {
        return Content("ERROR: " + ex.ToString());
    }
}
    // ✅ GET ALL
    [HttpGet]
    public IActionResult Get()
    {
        using var conn = _db.CreateConnection();
        return Ok(conn.Query("SELECT * FROM boats ORDER BY id DESC"));
    }

    // ✅ UPLOAD IMAGE
    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file");

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { image = fileName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    // ✅ UPDATE BOAT (ADD THIS)
    [HttpPut("{id}")]
public IActionResult Update(int id, [FromBody] UpdateItemRequest b)
{
    using var conn = _db.CreateConnection();

    if (string.IsNullOrWhiteSpace(b.name))
        return BadRequest("Name is required");

    if (b.image == null)
    {
        conn.Execute(@"
            UPDATE boats
            SET name=@name, price=@price
            WHERE id=@id
        ", new { id, name = b.name, price = b.price });
    }
    else
    {
        conn.Execute(@"
            UPDATE boats
            SET name=@name, price=@price, image=@image
            WHERE id=@id
        ", new { id, name = b.name, price = b.price, image = b.image });
    }

    return Ok("Updated");
}
[HttpDelete("{id}")]
public IActionResult Delete(int id)
{
    using var conn = _db.CreateConnection();

    conn.Execute("DELETE FROM boats WHERE id=@id", new { id });

    return Ok("Deleted");
}
}