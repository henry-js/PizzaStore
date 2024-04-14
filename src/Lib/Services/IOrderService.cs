using PizzaStore.Lib.Data.Models;

namespace PizzaStore.Lib.Services;

public interface IOrderService
{
    Order GetOrder(int orderId);
    void DeleteOrder(Order order);
    IEnumerable<PizzaBase> GetPizzaBases();
    IEnumerable<Topping> GetPizzaToppings();
    void Save(Order order);
}
