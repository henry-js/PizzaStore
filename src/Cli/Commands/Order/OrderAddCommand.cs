
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

        var order = new Order() { Customer = selectedCustomer };
        bool finished = false;

        while (!finished)
        {
            OrderPizza pizza = new OrderPizza();

            var pizzaBase = _console.Prompt(new SelectionPrompt<PizzaBase>()
            .Title("Select a base")
            .AddChoices(_orderService.GetPizzaBases())
            .UseConverter(pb => $"{pb.Name}: {pb.Price:C}"));

            pizza.AddBase(pizzaBase);

            pizza.Description = $"{pizza.Base.Name}, Cheese & Tomato";
            bool finishedToppings = !_console.Confirm("Add topping?");

            while (!finishedToppings)
            {
                var topping = _console.Prompt(new SelectionPrompt<Topping>()
                .Title("Select a topping")
                .AddChoices(_orderService.GetPizzaToppings())
                .UseConverter(t => $"{t.Name:30}: {t.Price:C}"));

                pizza.AddTopping(topping);

                finishedToppings = !_console.Confirm("Continue adding toppings?", defaultValue: false);
            }
            order.AddPizza(pizza);
            _console.WriteLine("Pizza added.");
            finished = !_console.Confirm("Do you want more pizzas?");
        }
        DisplayOrder(order);

        if (_console.Confirm("Place order?"))
            _orderService.Save(order);

        return await Task.FromResult(0);
    }

    private void DisplayOrder(Order order)
    {
        foreach (var pizza in order.Pizzas)
        {
            _console.WriteLine($"{pizza.Description}, {pizza.Price:C}");
            _console.WriteLine($"Total Pizza Cost: {order.Price:C}");
        }
    }
}
