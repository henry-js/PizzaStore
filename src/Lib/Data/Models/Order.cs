
using FluentResults;

namespace PizzaStore.Lib.Data.Models;

public class Order
{
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

    public static Order CreateNew(Customer customer, IEnumerable<OrderPizza> orderPizzas)
    {
        var order = new Order()
        {
            Customer = customer,
            Pizzas = orderPizzas.ToArray(),
            OrderPrice = orderPizzas.Sum(p => p.Price),
            CustomerDistance = customer.DeliveryDistance,
            DeliveryPrice = CalcDeliveryCharge(customer.DeliveryDistance),
            IsDeliverable = customer.DeliveryDistance < 8,
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

    internal void DeActivate()
    {
        IsActive = false;
    }

    internal Invoice? ToInvoice(int userId)
    {
        if (IsActive && !IsInvoiced)
        {
            var orderPrice = IsDeliverable ? OrderPrice + DeliveryPrice : OrderPrice;
            var invoice = new Invoice()
            {
                Order = this,
                Customer = Customer,
                CreatedAt = DateTime.UtcNow,
                Price = orderPrice,
                VatPrice = orderPrice * (decimal)Invoice.VatRate,
                UserId = userId,
            };
            IsInvoiced = true;
            return invoice;
        }
        return null;
    }
}