using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly DbConnection _db;

    public ReportController(DbConnection db)
    {
        _db = db;
    }

    // ================================
    // ✅ 1. DAILY REVENUE (KEEP THIS)
    // ================================
    [HttpGet("daily")]
    public IActionResult Daily()
    {
        using var conn = _db.CreateConnection();

        var total = conn.ExecuteScalar<decimal>(
            "SELECT IFNULL(SUM(total_amount),0) FROM transactions WHERE DATE(date)=CURDATE()"
        );

        return Ok(total);
    }

    // ================================
    // ✅ 2. SEND FEEDBACK (NEW)
    // ================================
    [HttpPost]
    public IActionResult SendReport([FromBody] Report model)
    {
        using var conn = _db.CreateConnection();

        var sql = @"
            INSERT INTO reports (name, message, created_at)
            VALUES (@Name, @Message, NOW())
        ";

        conn.Execute(sql, model);

        return Ok("Feedback submitted successfully!");
    }

    // ================================
    // ✅ 3. GET ALL REPORTS (ADMIN)
    // ================================
    [HttpGet]
    public IActionResult GetReports()
    {
        using var conn = _db.CreateConnection();

        var sql = @"
            SELECT id, name, message, created_at AS CreatedAt
            FROM reports
            ORDER BY created_at DESC
        ";

        var data = conn.Query(sql);

        return Ok(data);
    }
}