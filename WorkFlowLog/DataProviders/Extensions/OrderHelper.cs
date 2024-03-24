using WorkFlowLog.DataProviders.Interfaces;
using WorkFlowLog.Entities;

namespace WorkFlowLog.DataProviders.Extensions;

public static class OrderHelper
{
    public static void PrintOrder(this Order order)
    {
        Console.WriteLine($"Order: {order.Id}, {order.Description}");
    }

    public static void PrintOrders(this List<Order> orders)
    {
        foreach (var order in orders)
        {
            order.PrintOrder();
        }
    }

    public static void AddOrders(this IOrdersProvider provider, List<Order> orders)
    {
        foreach (var order in orders)
        {
            provider.AddOrder(order);
        }
    }
}
