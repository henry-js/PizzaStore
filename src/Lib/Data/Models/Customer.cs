using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace PizzaStore.Lib.Data.Models;

public class Customer
{
    // private Customer() { }
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PostCode { get; set; }
    public required string HouseNumber { get; set; }
    public double DeliveryDistance { get; set; } = 0;
    // public IEnumerable<Order> Orders { get; set; } = [];
    [NotMapped]
    public string Code => $"{LastName}{HouseNumber}{PostCode}".ToUpper(CultureInfo.CurrentCulture);

    public static Customer CreateNew(string firstName, string lastName, string postCode, string houseNumber)
    {
        var customer = new Customer()
        {
            FirstName = firstName,
            LastName = lastName,
            PostCode = postCode,
            HouseNumber = houseNumber,
        };

        return customer;
    }
}
