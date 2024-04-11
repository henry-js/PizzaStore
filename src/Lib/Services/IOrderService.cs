using PizzaStore.Lib.Data.Models;

namespace PizzaStore.Lib.Services;

public interface IOrderService
{
    Order GetOrder(int orderId);
    void DeleteOrder(Order order);
    void UpdateOrder(Order order);
    IEnumerable<PizzaBase> GetPizzaBases();
    IEnumerable<Topping> GetPizzaToppings();
    Invoice Save(Order order);
}
