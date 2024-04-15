using Bogus;
using PizzaStore.Lib.Data;
using PizzaStore.Lib.Data.Models;

namespace Lib.Tests;

public class FakeOrders
{
    public static IEnumerable<OrderPizza> GeneratePizzas(IEnumerable<PizzaBase> bases, Topping[] toppings, int count)
    {
        int pizzaId = 0;
        var fakePizza = new Faker<OrderPizza>()
            .CustomInstantiator(f =>
                OrderPizza.CreateNew(f.PickRandom(bases), f.Random.ArrayElements(toppings, f.Random.Number(toppings.Length)))
            )
            .RuleFor(p => p.Id, pizzaId++);
        var pizzas = fakePizza.Generate(count);

        return pizzas;
    }
}