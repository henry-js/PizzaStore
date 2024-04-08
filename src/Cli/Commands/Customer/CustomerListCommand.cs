namespace PizzaStore.Cli.Commands;

public class CustomerListCommand : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        AnsiConsole.WriteLine("Listing all customers");
        return await Task.FromResult(1);
    }
}
