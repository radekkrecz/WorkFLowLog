using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorkFlowLog;
using WorkFlowLog.Data;
using WorkFlowLog.DataProviders;
using WorkFlowLog.DataProviders.Interfaces;
using WorkFlowLog.Entities;
using WorkFlowLog.Repositories;

Console.Title = "WorkFlowLog";

var services = new ServiceCollection();

services.AddSingleton<IApp, App>();
services.AddSingleton<IRepository<Employee>, SqlRepository<Employee>>();
services.AddSingleton<IRepository<Order>, SqlRepository<Order>>();
services.AddSingleton<IEmployeesProvider, EmployeesProvider>();
services.AddSingleton<IOrdersProvider, OrdersProvider>();
services.AddSingleton<IUserCommunication, UserCommunication>();
services.AddSingleton<IFileDataProvider<Employee>, FileDataProvider<Employee>>();   
services.AddSingleton<IFileDataProvider<Order>, FileDataProvider<Order>>();
services.AddDbContext<WorkFlowDbContext>();

var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetService<IApp>()!;

app.Run();
