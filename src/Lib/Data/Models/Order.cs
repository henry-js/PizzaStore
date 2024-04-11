
using FluentResults;

namespace PizzaStore.Lib.Data.Models;

public class Order
{
    // private readonly List<OrderPizza> _pizzas = [];

    public Order() { }
    public int Id { get; set; }
    public Customer Customer { get; set; } = default!;
    public IEnumerable<OrderPizza> Pizzas { get; set; } = [];
    public double CustomerDistance { get; set; }
    public decimal DeliveryPrice { get; set; }
    public decimal OrderPrice { get; set; }
    public bool IsActive { get; set; }
    public bool IsInvoiced { get; set; }
    public bool IsDeliverable { get; set; }

    // public void AddPizza(OrderPizza pizza)
    // {
    //     if (IsActive || IsInvoiced)
    //         return;

    //     _pizzas.Add(pizza);
    //     Price = CalculatePrice();
    // }

    public static Order CreateNew(Customer customer, IEnumerable<OrderPizza> orderPizzas)
    {
        var order = new Order()
        {
            Customer = customer,
            Pizzas = orderPizzas.ToArray(),
            OrderPrice = orderPizzas.Sum(p => p.Price),
            CustomerDistance = customer.DeliveryDistance,
            DeliveryPrice = CalcDeliveryCharge(customer.DeliveryDistance),
            IsDeliverable = customer.DeliveryDistance > 8,
        };

        return order;
    }

    private static decimal CalcDeliveryCharge(double deliveryDistance)
        => deliveryDistance switch
        {
            <= 2 => 0,
            <= 5 => 3.00m,
            <= 8 => 5.00m,
            _ => 0,
        };

    public void DeActivate()
    {
        IsActive = false;
    }
}