using Microsoft.EntityFrameworkCore;
using PizzaStore.Lib.Data.Models;

namespace PizzaStore.Lib.Data;

public class PizzaStoreContext(DbContextOptions<PizzaStoreContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<PizzaBase> Bases { get; set; }
    public DbSet<PizzaTopping> Toppings { get; set; }
}