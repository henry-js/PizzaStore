using PizzaStore.Lib.Services;

namespace PizzaStore.Cli.Commands;

public class CustomerAddCommand(IAnsiConsole console, ICustomerService service) : AsyncCommand<CustomerAddCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<FNAME>")]
        public string FirstName { get; set; } = default!;

        [CommandArgument(1, "<LNAME>")]
        public string LastName { get; set; } = default!;

        [CommandArgument(2, "<Postcode>")]
        public string PostCode { get; set; } = default!;

        [CommandArgument(3, "<HouseNum>")]
        public int HouseNumber { get; set; }
    }
    private readonly IAnsiConsole _console = console;
    private readonly ICustomerService _service = service;

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        _console.WriteLine("Adding a customer");

        var customerResult = _service.AddCustomer(settings.FirstName, settings.LastName, settings.HouseNumber, settings.PostCode);

        if (customerResult.Success)
        {
            _console.MarkupLine($"[green]Customer {customerResult.Customer!.Code} Added.[/]");
        }
        else
        {
            _console.MarkupLine("[red]Add customer failed.[/]");
        }
        return await Task.FromResult(0);
    }
}