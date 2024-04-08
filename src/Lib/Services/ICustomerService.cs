using FluentValidation.Results;
using PizzaStore.Lib.Data.Models;
using PizzaStore.Lib.Validation;

namespace PizzaStore.Lib.Services;

public interface ICustomerService
{
    CustomerResult AddCustomer(string firstName, string lastName, int houseNumber, string postCode);
    IEnumerable<Customer> GetAll();
}

public class CustomerService : ICustomerService
{
    public CustomerResult AddCustomer(string firstName, string lastName, int houseNumber, string postCode)
    {
        var customer = new Customer()
        {
            FirstName = firstName,
            LastName = lastName,
            HouseNumber = houseNumber,
            PostCode = postCode,
            Code = $"{lastName}{houseNumber}{postCode}",
        };

        var validator = new CustomerValidator();

        var result = validator.Validate(customer);

        if (!result.IsValid)
        {
            var customerResult = new CustomerResult(result.Errors);
            return customerResult;
        }

        // TODO: dbcontext.SAVECHANGES();
        return new CustomerResult(customer);
    }

    public IEnumerable<Customer> GetAll()
    {
        var customers = new List<Customer>();
        return customers;
    }
}

public class CustomerResult
{
    private readonly List<string> _errorMessages = [];

    internal CustomerResult(List<ValidationFailure> errors)
    {
        _errorMessages = errors.Select(e => e.ErrorMessage).ToList();
    }
    internal CustomerResult(Customer customer)
    {
        Success = true;
        Customer = customer;
    }

    public bool Success { get; }
    public Customer? Customer { get; }
    public List<string> ErrorMessages => _errorMessages;
}