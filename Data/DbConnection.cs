using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

public class DbConnection
{
    private readonly IConfiguration _config;

    public DbConnection(IConfiguration config)
    {
        _config = config;
    }

    public IDbConnection CreateConnection()
    {
        return new MySqlConnection(
            _config.GetConnectionString("DefaultConnection")
        );
    }
}