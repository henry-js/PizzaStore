
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
        .UseConverter(cust => cust.Code)
        );

        bool finished = false;
        var orderPizzas = new List<OrderPizza>();
        while (!finished)
        {
            var pizzaBase = _console.Prompt(new SelectionPrompt<PizzaBase>()
            .Title("Select a base")
            .AddChoices(_orderService.GetPizzaBases())
            .UseConverter(pb => $"{pb.Name}: {pb.Price:C}"));

            bool finishedToppings = !_console.Confirm("Add topping?");

            var pizzaToppings = new List<Topping>();
            while (!finishedToppings)
            {
                var topping = _console.Prompt(new SelectionPrompt<Topping>()
                .Title("Select a topping")
                .AddChoices(_orderService.GetPizzaToppings())
                .UseConverter(t => $"{t.Name}: {t.Price:C}"));

                pizzaToppings.Add(topping);

                finishedToppings = !_console.Confirm("Continue adding toppings?", defaultValue: false);
            }
            var pizza = OrderPizza.CreateNew(pizzaBase, pizzaToppings);
            orderPizzas.Add(pizza);
            _console.WriteLine($"{pizza.Description} added.");
            finished = !_console.Confirm("Do you want more pizzas?");
        }
        var order = Order.CreateNew(selectedCustomer, orderPizzas);
        DisplayOrder(order);

        if (_console.Confirm("Place order?"))
            _orderService.Save(order);

        return await Task.FromResult(0);
    }

    private void DisplayOrder(Order order)
    {
        var table = new Table();
        foreach (var pizza in order.Pizzas)
        {
            _console.WriteLine($"{pizza.Description}, {pizza.Price:C}");
            _console.WriteLine($"Total Pizza Cost: {order.OrderPrice:C}");
        }
    }
}
