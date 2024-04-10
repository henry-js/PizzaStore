
namespace PizzaStore.Lib.Data.Models;

public class Order
{
    private readonly List<OrderPizza> _pizzas = [];
    public int Id { get; set; }
    public Customer Customer { get; set; } = default!;
    public IEnumerable<OrderPizza> Pizzas => _pizzas;
    public decimal DeliveryCharge { get; set; }
    public decimal Price { get; set; }

    public void AddPizza(OrderPizza pizza)
    {
        _pizzas.Add(pizza);

        Price = CalculatePrice();
    }

    private decimal CalculatePrice() => _pizzas.Sum(p => p.Price);
}
