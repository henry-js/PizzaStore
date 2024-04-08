namespace PizzaStore.Lib.Data.Models;

public class Order
{
    public int Id { get; set; }
    public Customer Customer { get; set; } = default!;
    public List<Pizza> Pizzas { get; set; } = [];
    public decimal DeliveryCharge { get; set; }

}
