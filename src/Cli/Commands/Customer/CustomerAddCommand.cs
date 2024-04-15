using PizzaStore.Lib.Services;

namespace PizzaStore.Cli.Commands;

public class CustomerAddCommand(IAnsiConsole console, ICustomerService service) : AsyncCommand<CustomerAddCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<FirstName>")]
        public string FirstName { get; set; } = default!;

        [CommandArgument(1, "<LastName>")]
        public string LastName { get; set; } = default!;

        [CommandArgument(2, "<Postcode>")]
        public string PostCode { get; set; } = default!;

        [CommandArgument(3, "<HouseNumber>")]
        public int HouseNumber { get; set; }

        [CommandArgument(4, "<Distance(miles)>")]
        public int Distance { get; set; }
    }
    private readonly IAnsiConsole _console = console;
    private readonly ICustomerService _service = service;

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        _console.WriteLine("Adding a customer");

        var customerResult = _service.AddCustomer(settings.FirstName, settings.LastName, settings.HouseNumber, settings.PostCode, settings.Distance);

        if (customerResult.Success)
        {
            _console.WriteCustomerComponent(customerResult.Customer!);
        }
        else
        {
            _console.MarkupLine("[red]Add customer failed.[/]");
        }
        return await Task.FromResult(0);
    }
}
