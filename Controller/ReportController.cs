using Microsoft.AspNetCore.Mvc;
using Dapper;
[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly DbConnection _db;

    public ReportController(DbConnection db)
    {
        _db = db;
    }

    [HttpGet("daily")]
    public IActionResult Daily()
    {
        using var conn = _db.CreateConnection();

        var total = conn.ExecuteScalar<decimal>(
            "SELECT IFNULL(SUM(total_amount),0) FROM transactions WHERE DATE(date)=CURDATE()");

        return Ok(total);
    }
}