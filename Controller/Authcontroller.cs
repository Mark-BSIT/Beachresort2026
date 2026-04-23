using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using BCrypt.Net;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            return BadRequest("Username and password are required");

        try
        {
            using var conn = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
            conn.Open();

            var cmd = new MySqlCommand("SELECT id, username, password, role FROM users WHERE username=@u", conn);
            cmd.Parameters.AddWithValue("@u", model.Username);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                var hashedPassword = reader["password"].ToString();

                if (BCrypt.Net.BCrypt.Verify(model.Password, hashedPassword))
                {
                    return Ok(new
                    {
                        id = reader["id"],
                        username = reader["username"],
                        role = reader["role"]
                    });
                }
            }

            return Unauthorized("Invalid login");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

public class LoginModel
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}