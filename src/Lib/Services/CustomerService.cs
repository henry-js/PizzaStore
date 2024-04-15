using System.Globalization;
using PizzaStore.Lib.Data;
using PizzaStore.Lib.Data.Models;
using PizzaStore.Lib.Validation;

namespace PizzaStore.Lib.Services;

public class CustomerService(PizzaStoreContext dbContext) : ICustomerService
{
    private readonly PizzaStoreContext db = dbContext;

    public CustomerResult AddCustomer(string firstName, string lastName, int houseNumber, string postCode, int deliveryDistance)
    {
        var customer = new Customer()
        {
            FirstName = firstName,
            LastName = lastName,
            HouseNumber = houseNumber.ToString(CultureInfo.CurrentCulture),
            PostCode = postCode,
            DeliveryDistance = deliveryDistance
        };

        var validator = new CustomerValidator();

        var result = validator.Validate(customer);

        if (!result.IsValid)
        {
            return new CustomerResult(result.Errors);
        }

        db.Customers.Add(customer);
        db.SaveChanges();
        return new CustomerResult(customer);
    }

    public IEnumerable<Customer> GetAll()
    {
        var customers = db.Customers.ToList();

        return customers;
    }
}
