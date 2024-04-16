
using System.ComponentModel.DataAnnotations.Schema;
using FluentResults;

namespace PizzaStore.Lib.Data.Models;

public class Order
{
    public Order() { }
    public int Id { get; set; }
    public required User User { get; set; }
    public Customer Customer { get; set; } = default!;
    public IEnumerable<OrderPizza> Pizzas { get; set; } = [];
    public decimal DeliveryPrice { get; set; }
    public decimal OrderPrice { get; set; }
    public bool IsActive { get; set; }
    public bool IsInvoiced { get; set; }
    public bool IsDelivery { get; set; }
    [NotMapped]
    public decimal TotalPrice => DeliveryPrice + OrderPrice;

    public decimal VAT => TotalPrice * 0.2m;

    public static Order CreateNew(Customer customer, IEnumerable<OrderPizza> orderPizzas, User user)
    {
        var order = new Order()
        {
            Customer = customer,
            Pizzas = orderPizzas.ToArray(),
            OrderPrice = orderPizzas.Sum(p => p.Price),
            DeliveryPrice = CalcDeliveryCharge(customer.DeliveryDistance),
            IsActive = true,
            User = user
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

    public void MarkAsForDelivery()
    {
        IsDelivery = Customer.SupportsDelivery;
    }

    internal void DeActivate()
    {
        IsActive = false;
    }

    internal Invoice? ToInvoice()
    {
        if (IsActive && !IsInvoiced)
        {
            var orderPrice = OrderPrice + DeliveryPrice;
            var invoice = new Invoice()
            {
                Order = this,
                Customer = Customer,
                CreatedAt = DateTime.UtcNow,
                Price = this.TotalPrice,
                VatPrice = this.VAT,
                User = this.User,
                IsReadOnly = true,
            };
            IsInvoiced = true;
            return invoice;
        }
        return null;
    }
}