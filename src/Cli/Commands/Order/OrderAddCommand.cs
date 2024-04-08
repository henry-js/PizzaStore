
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
        var selectedCustomer = _console.Prompt(new SelectionPrompt<Customer>()
        .Title("Select an existing customer")
        .AddChoices(customers)
        .UseConverter(cust => $""));

        return await Task.FromResult(0);
    }
}
