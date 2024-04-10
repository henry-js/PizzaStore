using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace PizzaStore.Lib.Data.Models;

public class OrderPizza
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
    public PizzaBase Base { get; set; } = default!;
    public IEnumerable<Topping> Toppings { get; set; }
    public Order Order { get; set; }


    public static OrderPizza CreateNew(PizzaBase pizzaBase, IEnumerable<Topping> toppings)
    {
        var toppingsDesc = !toppings.Any() ? "" : $" {string.Join(", ", toppings.Select(t => t.Name))}";
        var pizza = new OrderPizza()
        {
            Base = pizzaBase,
            Toppings = toppings.ToList(),
            Price = pizzaBase.Price + toppings.Sum(t => t.Price),
            Description = $"{pizzaBase.Name} with {toppings.Count()} toppings{toppingsDesc}",
        };

        return pizza;
    }
}