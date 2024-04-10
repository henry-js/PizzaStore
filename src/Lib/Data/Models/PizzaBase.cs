namespace PizzaStore.Lib.Data.Models;

public class PizzaBase
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
}
