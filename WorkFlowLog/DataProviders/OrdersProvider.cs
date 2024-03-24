using WorkFlowLog.DataProviders.Interfaces;
using WorkFlowLog.Entities;
using WorkFlowLog.Repositories;
using WorkFlowLog.Repositories.Extensions;

namespace WorkFlowLog.DataProviders;

public class OrdersProvider : IOrdersProvider
{
    private readonly IRepository<Order> _repository;
    private readonly IFileDataProvider<Order> _fileDataProvider;
    private bool _isEventAddOrderAdded;
    private bool _isEventRemoveOrderAdded;

    public OrdersProvider(IRepository<Order> repository, IFileDataProvider<Order> fileDataProvider)
    {
        _repository = repository;
        _fileDataProvider = fileDataProvider;
    }

    public void AddEvent(EventHandler<Order> eventHandler)
    {
        _repository.ItemAdded += eventHandler;  
    }

    public void AddOrder(Order order)
    {
        _repository.Add(order);
        _repository.Save();
    }

    public void AddOrders(Order[] orders)
    {
        _repository.AddBatch(orders);
        _repository.Save();
    }   

    public void DeleteOrder(Order order)
    {
        _repository.Remove(order);
        _repository.Save();
    }

    public Order? GetOrder(int id)
    {
        return _repository.GetById(id);
    }

    public List<Order> GetOrders()
    {
        return _repository.GetAll().OrderBy(o => o.Name).ThenBy(o => o.OrderId).ToList();
    }

    public void RemoveEvent(EventHandler<Order> eventHandler)
    {
        _repository.ItemRemoved += eventHandler;
    }

    public void LoadOrdersFromFile(string path)
    {
        var orders = _fileDataProvider.ReadFile(path);

        if (orders == null)
        {
            return;
        }

        _repository.AddBatch(orders.ToArray());
    }

    public void SaveOrdersToFile(string path)
    {
        _fileDataProvider.WriteFile(path, _repository.GetAll().ToList());
    }

    public void AddEventAddOrder(EventHandler<Order> eventHandler)
    {
        if(!_isEventAddOrderAdded)
            _repository.ItemAdded += eventHandler;
        _isEventAddOrderAdded = true;
    }

    public void RemoveEventAddOrder(EventHandler<Order> eventHandler)
    {
        if(_isEventAddOrderAdded)
            _repository.ItemAdded -= eventHandler;
        _isEventAddOrderAdded = false;
    }

    public void AddEventRemoveOrder(EventHandler<Order> eventHandler)
    {
        if(!_isEventRemoveOrderAdded)
            _repository.ItemRemoved += eventHandler;
        _isEventRemoveOrderAdded = true;
    }

    public void RemoveEventRemoveOrder(EventHandler<Order> eventHandler)
    {
        if(_isEventRemoveOrderAdded)
            _repository.ItemRemoved -= eventHandler;
        _isEventRemoveOrderAdded = false;
    }
}
