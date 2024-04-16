using System.Collections;
using Bogus;
using FluentAssertions;
using PizzaStore.Lib;
using PizzaStore.Lib.Data.Models;
using PizzaStore.Lib.Services;
using PizzaStore.Lib.Utils;

namespace Lib.Tests;

public class OrderServiceTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly IEnumerable<Topping> toppings;
    private readonly IEnumerable<User> users;
    private readonly IEnumerable<PizzaBase> bases;
    private readonly IEnumerable<Customer> customers;
    private OrderService service;

    public OrderServiceTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        service = new OrderService(_fixture.Context);
        bases = service.GetPizzaBases();
        toppings = service.GetPizzaToppings();
        users = service.GetUsers();
        customers = service.GetCustomers();

    }

    [Fact]
    public void OrderShouldCorrectlySumValueOfPizzasWithVaryingToppings()
    {
        // Arrange
        service = new OrderService(_fixture.Context);
        var users = service.GetUsers();
        var customer = customers.First();

        // Act
        var orderPizzas = FakeOrders.GeneratePizzas(bases, toppings.ToArray(), 3);
        var order = Order.CreateNew(customer, orderPizzas, users.First());

        // Assert
        order.OrderPrice.Should().Be(orderPizzas.Sum(p => p.Price), "Order price should be the sum cost of all pizzas");
    }

    [Theory]
    [ClassData(typeof(InRangeCustomerTestData))]
    public void OrderCustomerShouldBeSameAsCustomerReceived(Customer customer)
    {
        var service = new OrderService(_fixture.Context);
        var order = Order.CreateNew(customer, [], users.First());

        order.Customer.Code.Should().Be(customer.Code, "The customer code is generated using properties from the customer");
    }

    [Theory]
    [ClassData(typeof(InRangeCustomerTestData))]
    public void OrderShouldSupportMarkingAsForDeliveryIfCustomerIsInRange(Customer customer)
    {
        var service = new OrderService(_fixture.Context);
        var order = Order.CreateNew(customer, [], users.First());

        order.MarkAsForDelivery();
        order.IsDelivery.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(OutOfRangeCustomerTestData))]
    public void MarkAsForDeliveryShouldDoNothingWhenCustomerIsOutOfRange(Customer customer)
    {
        var service = new OrderService(_fixture.Context);
        var order = Order.CreateNew(customer, [], users.First());

        order.MarkAsForDelivery();
        order.IsDelivery.Should().BeFalse();
    }
    [Fact]
    public void InvoicedOrderShouldBeMarkedAsSuch()
    {
        var service = new OrderService(_fixture.Context);
        var customer = customers.First();
        var pizzas = BogusGenerator.GeneratePizzas(bases, toppings, 3);
        var order = Order.CreateNew(customer, pizzas, users.First());
        service.Save(order);
        service.InvoiceOrder(order);

        order.IsInvoiced.Should().BeTrue();
    }

    [Fact]
    public void OrderToInvoiceShouldDoNothingIfAlreadyInvoiced()
    {
        // Arrange
        var service = new OrderService(_fixture.Context);
        var customer = customers.First();
        var pizzas = BogusGenerator.GeneratePizzas(bases, toppings, 3);
        var order = Order.CreateNew(customer, pizzas, users.First());

        // Act
        service.Save(order);
        service.InvoiceOrder(order);
        order.IsInvoiced.Should().BeTrue();
        var invoice = service.InvoiceOrder(order);

        // Assert
        invoice.Should().BeNull();
        order.IsInvoiced.Should().BeTrue();
    }

    [Fact]
    public void OrderServiceShouldNotSaveOrderIfOrderPizzasIsEmpty()
    {
        // Arrange
        var service = new OrderService(_fixture.Context);
        var pizzas = new List<OrderPizza>();
        var order = Order.CreateNew(customers.First(), pizzas, users.First());

        // Act
        service.Save(order);
        var notSavedOrder = service.GetOrder(order.Id);

        // Assert
        notSavedOrder.Should().BeNull();
    }
}

internal class OutOfRangeCustomerTestData : TheoryData<Customer>
{
    public OutOfRangeCustomerTestData()
    {
        var customers = BogusGenerator.GenerateCustomers(4);
        foreach (var cust in customers)
        {
            cust.DeliveryDistance = Random.Shared.Next(9, 100);
        }
        AddRange(customers.ToArray());
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