using MySql.Data.MySqlClient;

public class DbConnection
{
    private readonly string _connString;

    public DbConnection(string connString)
    {
        _connString = connString;
    }

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(_connString);
    }
}