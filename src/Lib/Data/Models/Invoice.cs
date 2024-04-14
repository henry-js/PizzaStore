namespace PizzaStore.Lib.Data.Models;

public class Invoice
{
    public const double VatRate = 0.20d;
    public Invoice() { }

    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Order Order { get; set; }
    public Customer Customer { get; set; }
    public decimal Price { get; set; }
    public decimal VatPrice { get; set; }

}

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}