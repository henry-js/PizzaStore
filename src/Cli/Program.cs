
using Community.Extensions.Spectre.Cli.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PizzaStore.Cli.Commands;
using PizzaStore.Lib.Extensions;

var builder = Host.CreateApplicationBuilder(args);

// Only use configuration in appsettings.json
builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", false);

//Disable logging
builder.Logging.ClearProviders();

// Bind configuration section to object
var pizzaDbString = builder.Configuration.GetConnectionString("PizzaStoreDb")!;
builder.Services
      .AddPizzaStoreDb(connectionString: pizzaDbString)
      .AddPizzaStoreServices();


// Add a command and optionally configure it.

builder.Services.AddScoped<DemoCommand>();

builder.Services.AddScoped<CustomerListCommand>();
builder.Services.AddScoped<CustomerAddCommand>();
builder.Services.AddScoped<CustomerDeleteCommand>();
builder.Services.AddScoped<CustomerViewCommand>();

builder.Services.AddScoped<OrderAddCommand>();

//
// The standard call save for the commands will be pre-added & configured
//

builder.UseSpectreConsole<MainMenuCommand>(config =>
{
      // All commands above are passed to config.AddCommand() by this point

      config.SetApplicationName("pz");
      config.UseBasicExceptionHandler();

      config.AddCommand<DemoCommand>("demo")
            .WithDescription("Demo various components");

      config.AddBranch("customer", branch =>
      {
            branch.AddCommand<CustomerListCommand>("list")
                .WithDescription("List all available customers");
            branch.AddCommand<CustomerAddCommand>("add")
                .WithDescription("Add a new customer to the system");
            branch.AddCommand<CustomerDeleteCommand>("delete")
                .WithDescription("Delete customer by Id code");
            branch.AddCommand<CustomerViewCommand>("view")
                .WithDescription("View customer by Id code");
      });

      config.AddBranch("order", order =>
      {
            order.AddCommand<OrderAddCommand>("add")
              .WithDescription("Create a new order for a customer");

      });
});

var app = builder.Build();

await app.RunAsync();

#if DEBUG
Console.Write("Press any key to continue");
Console.ReadKey(intercept: true);
#endif
