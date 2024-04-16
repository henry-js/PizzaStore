using System.Globalization;
using Bogus;
using PizzaStore.Lib.Data.Models;

namespace PizzaStore.Lib.Utils;

public static class BogusGenerator
{
    public static IEnumerable<Customer> GenerateCustomers(int customers)
    {
        int custId = 0;

        var custFaker = new Faker<Customer>()
        .RuleFor(c => c.Id, custId++)
            .RuleFor(c => c.FirstName, f => f.Person.FirstName)
            .RuleFor(c => c.LastName, f => f.Person.LastName)
            .RuleFor(c => c.PostCode, f => f.Address.ZipCode("??##??"))
            .RuleFor(c => c.HouseNumber, f => f.Random.Number(1, 100).ToString(CultureInfo.InvariantCulture))
            .RuleFor(c => c.DeliveryDistance, f => f.Random.Number(0, 20));

        return custFaker.Generate(customers);
    }

    public static IEnumerable<Order> GenerateOrders(int orders)
    {
        int orderId = 0;
        var orderFaker = new Faker<Order>();

        return orderFaker.Generate(orders);
    }
    public static IEnumerable<OrderPizza> GeneratePizzas(IEnumerable<PizzaBase> bases, IEnumerable<Topping> toppings, int count)
    {
        int pizzaId = 0;
        var fakePizza = new Faker<OrderPizza>()
            .CustomInstantiator(f =>
                OrderPizza.CreateNew(
                    f.PickRandom(bases),
                    f.Random.ArrayElements(toppings.ToArray(), f.Random.Number(toppings.Count())))
            )
            .RuleFor(p => p.Id, pizzaId++);
        var pizzas = fakePizza.Generate(count);

        return pizzas;
    }
}