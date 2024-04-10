using PizzaStore.Lib.Data.Models;

namespace PizzaStore.Lib.Services;

public interface IOrderService
{
    Order GetOrder(string orderId);
    void DeleteOrder(Order order);
    void UpdateOrder(Order order);
    PizzaBase[] GetPizzaBases();
    Topping[] GetPizzaToppings();
    void Save(Order order);
}