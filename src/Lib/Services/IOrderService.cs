using System.Collections;
using PizzaStore.Lib.Data.Models;

namespace PizzaStore.Lib.Services;

public interface IOrderService
{
    Order? GetOrder(int orderId);
    IEnumerable<PizzaBase> GetPizzaBases();
    IEnumerable<Topping> GetPizzaToppings();
    IEnumerable<Customer> GetCustomers();
    IEnumerable<User> GetUsers();
    void Save(Order order);
    void Delete(int orderId);
    Invoice? InvoiceOrder(Order order);
}
