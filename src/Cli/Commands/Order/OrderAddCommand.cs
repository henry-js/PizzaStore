
using System.Globalization;
using Bogus.Extensions.UnitedKingdom;
using PizzaStore.Lib.Data.Models;
using PizzaStore.Lib.Services;

namespace PizzaStore.Cli.Commands;

public class OrderAddCommand(IAnsiConsole console, ICustomerService customerService, IOrderService orderService) : AsyncCommand<OrderAddCommand.Settings>
{
    private readonly IAnsiConsole _console = console;
    private readonly ICustomerService _customerService = customerService;
    private readonly IOrderService _orderService = orderService;

    public class Settings : CommandSettings
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        // Select customer
        var customers = _customerService.GetAll();
        var bases = _orderService.GetPizzaBases();
        var toppings = _orderService.GetPizzaToppings();
        var user = _orderService.GetUsers().First();
        var selectedCustomer = _console.WriteCustomerPrompt(customers);

        bool finished = false;
        var orderPizzas = new List<OrderPizza>();
        while (!finished)
        {
            var pizzaBase = _console.PromptForPizzaBase(bases);
            var pizzaToppings = _console.PromptForToppings(toppings);
            var pizza = OrderPizza.CreateNew(pizzaBase, pizzaToppings);

            orderPizzas.Add(pizza);

            _console.MarkupLineInterpolated(CultureInfo.CurrentCulture, @$"[cyan]{pizza.Description} added.[/]");
            finished = !_console.Confirm("Do you want more pizzas?");
        }
        var order = Order.CreateNew(selectedCustomer, orderPizzas, user);

        _console.DisplayOrder(order);
        if (order.Customer.SupportsDelivery && _console.Confirm("Is this for delivery?", true))
        {
            order.IsDelivery = true;
        }

        if (_console.Confirm("Place order?"))
            _orderService.Save(order);

        if (_console.Confirm("Invoice now?"))
        {
            var invoice = _orderService.InvoiceOrder(order);
        }
        return await Task.FromResult(0);
    }

}
