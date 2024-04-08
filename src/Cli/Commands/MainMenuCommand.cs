using System.Text.Json;
using Microsoft.Extensions.Options;
using PizzaStore.Lib.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PizzaStore.Cli.Commands;

public class MainMenuCommand(IAnsiConsole ansiConsole) : AsyncCommand<MainMenuCommand.Settings>
{
    private readonly IAnsiConsole _console = ansiConsole;

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        return await Task.FromResult(0);
    }
    public class Settings : CommandSettings { }
}