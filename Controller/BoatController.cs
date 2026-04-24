using Microsoft.AspNetCore.Mvc;
using Dapper;
[ApiController]
[Route("api/[controller]")]
public class BoatController : ControllerBase
{
    private readonly DbConnection _db;

    public BoatController(DbConnection db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Get()
    {
        using var conn = _db.CreateConnection();
        return Ok(conn.Query("SELECT * FROM boats"));
    }

    [HttpPost]
    public IActionResult Add([FromBody] dynamic b)
    {
        using var conn = _db.CreateConnection();

        conn.Execute("INSERT INTO boats(name, capacity, price, total_units) VALUES(@n,@c,@p,@u)", new
        {
            n = b.name,
            c = b.capacity,
            p = b.price,
            u = b.total_units
        });

        return Ok();
    }
}