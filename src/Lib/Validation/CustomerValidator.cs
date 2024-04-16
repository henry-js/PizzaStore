using FluentValidation;
using PizzaStore.Lib.Data.Models;

namespace PizzaStore.Lib.Validation;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.FirstName).NotEmpty();
        RuleFor(customer => customer.LastName).NotEmpty();
        RuleFor(customer => customer.HouseNumber).NotEmpty();
        RuleFor(customer => customer.PostCode).NotEmpty();
    }
}
