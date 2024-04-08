using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PizzaStore.Lib.Data;
using PizzaStore.Lib.Services;

namespace PizzaStore.Lib;

public static class PizzaStoreExtensions
{

    public static IServiceCollection AddPizzaStoreServices(this IServiceCollection services)
    {
        services.AddTransient<ICustomerService, CustomerService>();
        return services;
    }
    public static IServiceCollection AddPizzaStoreDb(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContext<PizzaStoreContext>(options =>
        options.UseSqlite(connectionString));
    }
}
