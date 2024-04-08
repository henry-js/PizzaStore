namespace PizzaStore.Lib.Data.Models;

public class Customer
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PostCode { get; set; }
    public int HouseNumber { get; set; }
    public string Code { get; set; }
}
