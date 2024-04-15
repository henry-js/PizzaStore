using PizzaStore.Lib.Data.Models;

namespace PizzaStore.Lib.Services;

public interface ICustomerService
{
    public CustomerResult AddCustomer(string firstName, string lastName, int houseNumber, string postCode, int deliveryDistance);
    IEnumerable<Customer> GetAll();
}
