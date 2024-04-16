using PizzaStore.Lib.Services;
using PizzaStore.Lib.Utils;

namespace PizzaStore.Cli.Commands;

public class DemoCommand(IAnsiConsole console, ICustomerService service) : AsyncCommand<DemoCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-c|--cust")]
        public int Customers { get; set; }
        [CommandOption("-o|--orders")]
        public int Orders { get; set; }
    }
    private readonly IAnsiConsole _console = console;
    private readonly ICustomerService _service = service;

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        if (settings.Customers > 0)
        {
            var customers = BogusGenerator.GenerateCustomers(settings.Customers);

            foreach (var customer in customers)
            {
                _console.WriteCustomerComponent(customer);
            }
        }
        // if (settings.Orders > 0)
        // {
        //     var orders = FakeDataGenerator.GenerateOrders(settings.Orders);

        //     foreach (var order in orders)
        //     {
        //         _console.DisplayOrder(order);
        //     }
        // }
        return await Task.FromResult(0);
    }
}
