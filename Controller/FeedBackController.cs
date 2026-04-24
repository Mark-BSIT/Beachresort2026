using Microsoft.AspNetCore.Mvc;
using Dapper;

[ApiController]
[Route("api/[controller]")]
public class FeedBackController : ControllerBase
{
    private readonly DbConnection _db;

    public FeedBackController(DbConnection db)
    {
        _db = db;
    }

    [HttpPost]
    public IActionResult Add([FromBody] dynamic f)
    {
        using var conn = _db.CreateConnection();

        conn.Execute("INSERT INTO feedbacks(message,date) VALUES(@m,NOW())", new
        {
            m = f.message
        });

        return Ok();
    }

    [HttpGet]
    public IActionResult Get()
    {
        using var conn = _db.CreateConnection();
        return Ok(conn.Query("SELECT * FROM feedbacks"));
    }
}