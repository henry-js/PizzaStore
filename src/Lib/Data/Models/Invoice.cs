namespace PizzaStore.Lib.Data.Models;

public class Invoice
{
    public const double VatRate = 0.20d;
    public Invoice() { }

    public int Id { get; set; }
    public required User User { get; set; }
    public DateTime CreatedAt { get; set; }
    public required Order Order { get; set; }
    public required Customer Customer { get; set; }
    public decimal Price { get; set; }
    public decimal VatPrice { get; set; }
    public bool IsReadOnly { get; internal set; } = true;
}
