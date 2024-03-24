using WorkFlowLog.Entities;
using WorkFlowLog.Repositories;

namespace WorkFlowLog.DataProviders.Interfaces;
public interface IOrdersProvider
{
    List<Order> GetOrders();

    Order? GetOrder(int id);

    void AddOrder(Order order);

    void AddOrders(Order[] orders); 

    void DeleteOrder(Order order);

    void AddEventAddOrder(EventHandler<Order> eventHandler);

    void RemoveEventAddOrder(EventHandler<Order> eventHandler);

    void AddEventRemoveOrder(EventHandler<Order> eventHandler);

    void RemoveEventRemoveOrder(EventHandler<Order> eventHandler);


    void LoadOrdersFromFile(string path);

    void SaveOrdersToFile(string path);
}
