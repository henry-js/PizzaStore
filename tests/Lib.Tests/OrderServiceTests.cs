using Bogus;
using FluentAssertions;
using PizzaStore.Lib;
using PizzaStore.Lib.Data.Models;
using PizzaStore.Lib.Services;

namespace Lib.Tests;

public class OrderServiceTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public OrderServiceTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void OrderShouldCorrectlySumValueOfPizzasWithVaryingToppings()
    {
        IOrderService service = new OrderService(_fixture.Context);
        var customers = service.GetCustomers();
        var customer = customers.First();
        var bases = service.GetPizzaBases();
        var toppings = service.GetPizzaToppings().ToArray();

        var orderPizzas = FakeOrders.GeneratePizzas(bases, toppings, 3);

        var pizzaCost = orderPizzas.Sum(p => p.Price);

        var order = Order.CreateNew(customer, orderPizzas);

        order.OrderPrice.Should().Be(pizzaCost, "Order price should be the sum cost of all pizzas");
    }

    [Theory]
    [ClassData(typeof(InRangeCustomerTestData))]
    public void OrderShouldBeDeliverableIfCustomerIsInRange(Customer customer)
    {
        var order = Order.CreateNew(customer, []);

        order.IsDeliverable.Should().BeTrue();
    }
}

public class InRangeCustomerTestData : TheoryData<Customer>
{
    public InRangeCustomerTestData()
    {
        AddRange(GenerateCustomers().ToArray());
    }

    public IEnumerable<Customer> GenerateCustomers()
    {
        int customerId = 0;
        var fakeCustomer = new Faker<Customer>()
            .RuleFor(c => c.Id, _ => customerId++)
            .RuleFor(c => c.FirstName, f => f.Person.FirstName)
            .RuleFor(c => c.LastName, f => f.Person.LastName)
            .RuleFor(c => c.PostCode, f => f.Address.ZipCode("??##??"))
            .RuleFor(c => c.HouseNumber, f => f.Random.Int(1, 100).ToString())
            .RuleFor(c => c.FirstName, f => f.Person.FirstName)
            .RuleFor(c => c.DeliveryDistance, f => f.Random.Int(0, 7))
        ;

        return fakeCustomer.Generate(5);
    }
}