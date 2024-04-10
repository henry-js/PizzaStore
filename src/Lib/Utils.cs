using PizzaStore.Lib.Data.Models;

namespace PizzaStore.Lib;

internal static class Utils
{
    internal static string GenerateCustomerCode(string lastName, int houseNumber, string postCode) => $"{lastName}{houseNumber}{postCode}";
    internal static string GenerateCustomerCode(Customer customer) => $"{customer.LastName}{customer.HouseNumber}{customer.PostCode}";

}