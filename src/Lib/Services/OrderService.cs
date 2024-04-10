using PizzaStore.Lib.Data;
using PizzaStore.Lib.Data.Models;
using PizzaStore.Lib.Services;

namespace PizzaStore.Lib;

public class OrderService(PizzaStoreContext dbContext) : IOrderService
{
    private readonly PizzaStoreContext _db = dbContext;

    public void DeleteOrder(Order order)
    {
    }

    public Order GetOrder(int orderId)
    {
        return _db.Orders.Single(o => o.Id == orderId);
    }

    public IEnumerable<PizzaBase> GetPizzaBases()
    {
        return [.. _db.PizzaBases];
    }

    public IEnumerable<Topping> GetPizzaToppings()
    {
        return [.. _db.Toppings];
    }

    public void Save(Order order)
    {
        _db.Orders.Add(order);
        _db.SaveChanges();
    }

    public void UpdateOrder(Order order)
    {
    }
}