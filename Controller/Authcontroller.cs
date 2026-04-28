using Microsoft.AspNetCore.Mvc;
using Dapper;

[ApiController]
[Route("api/auth")]
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

    if (string.IsNullOrEmpty(data.username) || string.IsNullOrEmpty(data.password))
        return BadRequest("Missing fields");

    // 🔍 FIND USER
    var user = conn.QueryFirstOrDefault(@"
        SELECT * FROM users WHERE username = @u
    ", new { u = data.username });

    if (user == null)
        return Unauthorized("Invalid credentials");

    // 🔐 CHECK PASSWORD
    if ((string)user.password != data.password)
        return Unauthorized("Invalid credentials");

    return Ok(new
    {
        username = user.username,
        role = user.role
    });
}

    // ✅ REGISTER
    [HttpPost("register")]
public IActionResult Register([FromBody] RegisterRequest data)
{
    using var conn = _db.CreateConnection();

    if (string.IsNullOrEmpty(data.username) || string.IsNullOrEmpty(data.password))
        return BadRequest("Missing fields");

    var exists = conn.ExecuteScalar<int>(
        "SELECT COUNT(*) FROM users WHERE username=@u",
        new { u = data.username }
    );

    if (exists > 0)
        return BadRequest("Username already exists");

    conn.Execute(@"
        INSERT INTO users (username, password, role)
        VALUES (@u, @p, @r)
    ", new
    {
        u = data.username,
        p = data.password,
        r = data.role
    });

    return Ok("Account created");
}
    // ✅ RESET PASSWORD
    [HttpPost("reset")]
    public IActionResult Reset([FromBody] dynamic data)
    {
        using var conn = _db.CreateConnection();

        string username = data.username;
        string password = data.password;

        var exists = conn.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM users WHERE username=@u",
            new { u = username }
        );

        if (exists == 0)
            return BadRequest("User not found");

        conn.Execute(
            "UPDATE users SET password=@p WHERE username=@u",
            new { p = password, u = username }
        );

        return Ok("Password updated");
    }
}