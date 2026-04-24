using Microsoft.AspNetCore.Mvc;
using Dapper;
[ApiController]
[Route("api/[controller]")]
public class PricingController : ControllerBase
{
    private readonly DbConnection _db;

    public PricingController(DbConnection db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Get()
    {
        using var conn = _db.CreateConnection();
        return Ok(conn.QueryFirst("SELECT * FROM pricing LIMIT 1"));
    }

    [HttpPut]
    public IActionResult Update([FromBody] dynamic p)
    {
        using var conn = _db.CreateConnection();

        conn.Execute("UPDATE pricing SET entry_fee=@e, cottage_price=@c, boat_price=@b", new
        {
            e = p.entry_fee,
            c = p.cottage_price,
            b = p.boat_price
        });

        return Ok("Updated");
    }
}