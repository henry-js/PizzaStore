using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Lib.Data;
using PizzaStore.Lib.Data.Models;
using PizzaStore.Lib.Services;

namespace Lib.Tests;

public class DatabaseFixture : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<PizzaStoreContext> _contextOptions;

    public PizzaStoreContext Context { get; }

    public DatabaseFixture()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<PizzaStoreContext>()
            .UseSqlite(_connection)
            .Options;

        // Create the schema and seed some data
        using var context = new PizzaStoreContext(_contextOptions);
        context.Database.EnsureCreated();

        Context = new PizzaStoreContext(_contextOptions);
    }

    PizzaStoreContext CreateContext() => new(_contextOptions);
    public void Dispose()
    {
        Context.Dispose();
    }
}