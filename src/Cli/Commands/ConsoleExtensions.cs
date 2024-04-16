using System.Collections;
using System.Globalization;
using PizzaStore.Lib.Data.Models;

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
        panel.Header("[green]Customer Created[/]")
            .Padding(2, 1)
            .BorderColor(Color.Green);

        console.Write(panel);
    }
    public static Customer WriteCustomerPrompt(this IAnsiConsole console, IEnumerable<Customer> customers)
    {
        var grid = new Grid();

        grid.AddColumns(5);
        grid.AddRow([
            new Text(nameof(Customer.Code), new Style(Color.Yellow)),
            new Text(nameof(Customer.FirstName), new Style(Color.Yellow)),
            new Text(nameof(Customer.LastName), new Style(Color.Yellow)),
            new Text(nameof(Customer.PostCode), new Style(Color.Yellow)),
            new Text(nameof(Customer.DeliveryDistance), new Style(Color.Yellow)),
        ]);

        foreach (var cust in customers)
        {
            Text[] row = [
            new Text(cust.Code),
            new Text(cust.FirstName),
            new Text(cust.LastName),
            new Text(cust.PostCode),
            new Text(cust.DeliveryDistance.ToString(CultureInfo.InvariantCulture)).RightJustified(),
            ];
            grid.AddRow(row);
        }

        var panel = new Panel(grid)
        {
            Header = new PanelHeader("[yellow]Customers[/]", Justify.Center),
            Padding = new Padding(2, 1),
            BorderStyle = new Style(Color.Yellow),
        };

        console.Write(panel);
        console.WriteLine();

        console.Write("Press any key to continue");
        Console.ReadKey();

        var prompt = new SelectionPrompt<Customer>()
        .Title("Select an existing customer")
        .AddChoices(customers)
        .UseConverter(cust => cust.Code);

        var customer = console.Prompt(prompt);

        return customer;
    }
    public static PizzaBase PromptForPizzaBase(this IAnsiConsole console, IEnumerable<PizzaBase> bases)
    {
        var prompt = new SelectionPrompt<PizzaBase>()
            .Title("Select a base")
            .AddChoices(bases)
            .UseConverter(pb => $"{pb.Name}: {pb.Price:C}");

        return console.Prompt(prompt);
    }
    public static IEnumerable<Topping> PromptForToppings(this IAnsiConsole console, IEnumerable<Topping> toppings)
    {
        bool addingToppings = console.Confirm("Add topping?");
        List<Topping> chosenToppings = [];
        while (addingToppings)
        {
            var prompt = new SelectionPrompt<Topping>()
                .Title("Select a topping")
                .AddChoices(toppings)
                .UseConverter(t => $"{t.Id}.\t{t.Name}, [green]{t.Price:C}[/]");
            var topping = console.Prompt(prompt);
            chosenToppings.Add(topping);

            addingToppings = console.Confirm("Continue adding toppings?", defaultValue: false);
        }
        return chosenToppings;
    }

    public static void DisplayOrder(this IAnsiConsole console, Order order)
    {
        var userCustomerRow = new Columns(
        [
            new Text($"User ID: {order.User.Id}"),
            new Text($"Order ID: {order.Id}"),
            new Text($"Customer Code: {order.Customer.Code}").RightJustified(),
        ]);
        var table = new Table();
        table.AddColumns(
            [
                new TableColumn("Item").Footer("[underline]Order Total[/]"),
                new TableColumn("Details"),
                new TableColumn("Order Price").Footer($"[red bold underline]{order.TotalPrice + order.VAT:C}[/]"),
            ]);
        // table.AddRow(
        //     [
        //         new Text("")
        //     ]
        // );
        int pizzaNum = 1;
        foreach (var pizza in order.Pizzas)
        {
            table.AddRow([
                new Text($"Pizza {pizzaNum++}"),
                new Text(pizza.Description),
                new Text($"{pizza.Price:C}", new Style(Color.Yellow))
            ]);
        }
        table.AddEmptyRow();

        table.AddRow([
            new Text("Pizza Sub Total"),
            new Text(""),
            new Text($"{order.OrderPrice:C}", new Style(Color.Orange3)),
        ]);
        table.AddRow([
            new Text("Delivery Charge"),
            new Text(""),
            new Text($"{order.DeliveryPrice:C}", new Style(Color.Orange3)),
        ]);
        table.AddRow([
            new Text("VAT"),
            new Text(""),
            new Text($"{order.VAT:C}", new Style(Color.Orange3)),
        ]);
        console.Write(userCustomerRow);
        table.ShowRowSeparators();
        table.Expand();
        table.RoundedBorder();
        console.Write(table);
    }
}