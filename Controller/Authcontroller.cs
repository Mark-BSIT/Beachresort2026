using Microsoft.AspNetCore.Mvc;
using Dapper;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly DbConnection _db;

    public AuthController(DbConnection db)
    {
        _db = db;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest data)
    {
        using var conn = _db.CreateConnection();

        var user = conn.QueryFirstOrDefault(
            "SELECT * FROM users WHERE username = @username AND password = @password",
            new
            {
                username = data.username,
                password = data.password
            });

        if (user == null)
            return Unauthorized("Invalid credentials");

        return Ok(user);
    }
}