using System.Collections;
using PizzaStore.Lib.Data.Models;

namespace PizzaStore.Lib.Services;

public interface IOrderService
{
    Order GetOrder(int orderId);
    void DeleteOrder(Order order);
    IEnumerable<PizzaBase> GetPizzaBases();
    IEnumerable<Topping> GetPizzaToppings();
    public IEnumerable<Customer> GetCustomers();

    void Save(Order order);
    IEnumerable<User> GetUsers();
    public Invoice? InvoiceOrder(Order order);

}
