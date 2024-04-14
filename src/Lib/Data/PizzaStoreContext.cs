using Microsoft.EntityFrameworkCore;
using PizzaStore.Lib.Data.Models;

namespace PizzaStore.Lib.Data;

public class PizzaStoreContext(DbContextOptions<PizzaStoreContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<PizzaBase> PizzaBases { get; set; }
    public DbSet<Topping> Toppings { get; set; }
    public DbSet<OrderPizza> OrderPizzas { get; set; }
    public DbSet<Invoice> Invoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PizzaBase>().HasData(
            [
            new PizzaBase() { Id = 1, Name = "Deep Pan", Price = 6.00m },
            new PizzaBase() { Id = 2, Name = "Thin Crust", Price = 5.00m },
        ]);
        modelBuilder.Entity<Topping>().HasData(
            [
                new() { Id = 1, Name = "Ham", Price = 3.00m },
                new() { Id = 2, Name = "Mushrooms", Price = 2.00m },
                new() { Id = 3, Name = "Mediterranean vegetables", Price = 3.00m },
                new() { Id = 4, Name = "Extra cheese", Price = 1.50m },
            ]);

        modelBuilder.Entity<OrderPizza>()
            .HasMany(p => p.Toppings)
            .WithMany();

        modelBuilder.Entity<Customer>().HasData(
            [
                new () { Id = 1, FirstName = "John", LastName = "Doe", PostCode = "LL572EG", HouseNumber = "1" },
                new () { Id = 2, FirstName = "Alan", LastName = "Wake", PostCode = "B161AN", HouseNumber = "2" },
                new () { Id = 3, FirstName = "Ian", LastName = "Mile", PostCode = "EH64DA", HouseNumber = "3" },
                new () { Id = 4, FirstName = "Stephen", LastName = "Bonnell", PostCode = "BR75PN", HouseNumber = "4" },
                new () { Id = 5, FirstName = "Kate", LastName = "Braithwaite", PostCode = "FY76SU", HouseNumber = "5" },
            ]);
    }
}