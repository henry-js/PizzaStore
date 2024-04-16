using System.Collections;
using PizzaStore.Lib.Data;
using PizzaStore.Lib.Data.Models;
using PizzaStore.Lib.Services;

namespace PizzaStore.Lib;

public class OrderService(PizzaStoreContext dbContext) : IOrderService
{
    private readonly PizzaStoreContext _db = dbContext;


    public Order? GetOrder(int orderId)
    {
        return _db.Orders.SingleOrDefault(o => o.Id == orderId);
    }

    public IEnumerable<PizzaBase> GetPizzaBases() => [.. _db.PizzaBases];

    public IEnumerable<Topping> GetPizzaToppings() => [.. _db.Toppings];

    public IEnumerable<Customer> GetCustomers() => [.. _db.Customers];

    public IEnumerable<User> GetUsers() => [.. _db.Users];

    public void Save(Order order)
    {
        if (!order.Pizzas.Any()) return;

        if (!_db.Orders.Any(o => o.Id == order.Id))
        {
            _db.Orders.Add(order);
        }
        _db.SaveChanges();
    }

    public void Delete(int orderId)
    {
        var order = _db.Orders.SingleOrDefault(o => o.Id == orderId);

        if (order == null) return;
        if (!order.IsInvoiced)
        {
            _db.Orders.Remove(order);
            _db.SaveChanges();
        }
    }

    public Invoice? InvoiceOrder(Order order)
    {
        order = _db.Orders.Single(o => o.Id == order.Id);
        Invoice? invoice = null;
        if (order.IsActive && !order.IsInvoiced)
        {
            invoice = order.ToInvoice();
        }
        if (invoice == null) return null;

        _db.Invoices.Add(invoice);
        _db.SaveChanges();
        return invoice;
    }

    public void MarkAsForDelivery(Order order)
    {
        order.MarkAsForDelivery();
        _db.SaveChanges();
    }
}