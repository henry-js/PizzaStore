using System.ComponentModel.DataAnnotations;

namespace PizzaStore.Lib.Data.Models;

public class Pizza
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    [Required]
    public string Description { get; set; }
    public PizzaBase Base { get; set; }
    public List<PizzaTopping> Toppings { get; set; } = [];
}
