using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Lib.Data;
using PizzaStore.Lib.Data.Models;
using PizzaStore.Lib.Services;

namespace Lib.Tests;

public class CustomerServiceTests
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<PizzaStoreContext> _contextOptions;

    public CustomerServiceTests()
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
    }

    PizzaStoreContext CreateContext() => new(_contextOptions);

    [Theory]
    [InlineData("", "Doe", 1, "DY134G", 1, 0)]
    [InlineData("Jeff", "", 1, "DY134G", 1, 0)]
    [InlineData("Jeff", "Doe", 1, "", 1, 0)]
    [InlineData("", "", 1, "DY134G", 2, 0)]
    [InlineData("", "", 1, "", 3, 0)]
    public void WhenInputsAreInvalidCustomerResultShouldBeFailed(string firstName, string lastName, int houseNumber, string postCode, int errors, int deliveryDistance)
    {
        using var context = CreateContext();
        ICustomerService service = new CustomerService(context);

        var result = service.AddCustomer(firstName, lastName, houseNumber, postCode, deliveryDistance);

        result.Success.Should().BeFalse();
        result.ErrorMessages.Should().HaveCount(errors);
    }

    [Fact]
    public void WhenCustomerIsAddedCustomersTableShouldNotBeEmpty()
    {
        using var context = CreateContext();

        ICustomerService service = new CustomerService(context);

        var result = service.AddCustomer("Jude", "Law", 99, "CR03RL", 0);

        result.Success.Should().BeTrue();
    }

    [Theory]
    [InlineData("Jude", "Law", 1, "DY134G", 0)]
    [InlineData("Kendrick", "Lamar", 38, "CR03YL", 0)]
    [InlineData("Brian", "Krannstein", 5, "BK12RT", 0)]
    [InlineData("Jeremy", "Homme", 66, "PL09QA", 0)]

    public void WhenCustomerIsAddedCustomerCodeFormatShouldBeValid(string firstName, string lastName, int houseNumber, string postCode, int deliveryDistance)
    {
        using var context = CreateContext();

        ICustomerService service = new CustomerService(context);

        var result = service.AddCustomer(firstName, lastName, houseNumber, postCode, deliveryDistance);

        Customer customer = result.Customer!;

        var code = $"{lastName}{houseNumber}{postCode}";
        customer.Code.Should().BeEquivalentTo(code);
    }
}