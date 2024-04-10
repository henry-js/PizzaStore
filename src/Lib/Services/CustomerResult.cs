using FluentValidation.Results;
using PizzaStore.Lib.Data.Models;

namespace PizzaStore.Lib.Services;

public class CustomerResult
{
    private readonly string[] _errorMessages = [];

    internal CustomerResult(List<ValidationFailure> errors)
    {
        _errorMessages = errors.Select(e => e.ErrorMessage).ToArray();
    }
    internal CustomerResult(Customer customer)
    {
        Success = true;
        Customer = customer;
    }

    public bool Success { get; }
    public Customer? Customer { get; }
    public string[] ErrorMessages => _errorMessages;
}