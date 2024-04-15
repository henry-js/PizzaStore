using PizzaStore.Lib.Data.Models;
using Spectre.Console.Rendering;

namespace PizzaStore.Cli.Commands;

public static class ConsoleExtensions
{
    public static void WriteCustomerComponent(this IAnsiConsole console, Customer customer)
    {
        var grid = new Grid()
            .AddColumns(2)
            .AddRow(nameof(customer.Code), customer.Code)
            .AddRow(nameof(customer.FirstName), customer.FirstName)
            .AddRow(nameof(customer.LastName), customer.LastName)
            .AddRow(nameof(customer.PostCode), customer.PostCode)
            .AddRow(nameof(customer.HouseNumber), customer.HouseNumber)
            ;

        var panel = new Panel(grid);
        panel.Header("Customer Created");
    }
}