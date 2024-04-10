using PizzaStore.Lib.Data.Models;

namespace PizzaStore.Lib.Services;

public interface ICustomerService
{
    CustomerResult AddCustomer(string firstName, string lastName, int houseNumber, string postCode);
    IEnumerable<Customer> GetAll();
}
