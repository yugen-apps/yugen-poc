using Microsoft.Data.Sqlite;

namespace Poc.Ef.Persistence.Helpers;

public static class SqliteInMemoryHelper
{
    private static SqliteConnection? _connection;

    public static SqliteConnection Initialize()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        return _connection;
    }

    public static void Dispose()
    {
        // closing destroys the in-memory database
        _connection?.Close(); 
        _connection?.Dispose();
    }
}