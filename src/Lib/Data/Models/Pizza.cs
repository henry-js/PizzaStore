using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace PizzaStore.Lib.Data.Models;

public class OrderPizza
{
    private readonly List<Topping> _toppings = [];
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
    public PizzaBase Base { get; set; } = default!;
    public IEnumerable<Topping> Toppings => _toppings;

    public void AddBase(PizzaBase pizzaBase)
    {
        Base = pizzaBase;
        Price = CalculatePrice();
    }

    public void AddTopping(Topping topping)
    {
        _toppings.Add(topping);
        Price = CalculatePrice();
    }

    private decimal CalculatePrice() => Base.Price + Toppings.Sum(t => t.Price);
}