

namespace PizzaStore.Cli.Commands;

public class CustomerViewCommand : AsyncCommand<CustomerViewCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<Id>")]
        public string CustomerId { get; set; } = string.Empty;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.WriteLine("Listing all customers");
        return await Task.FromResult(1);
    }
}