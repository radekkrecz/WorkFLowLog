using Microsoft.Extensions.DependencyInjection;
using WorkFlowLog;
using WorkFlowLog.Components.CsvReader;
using WorkFlowLog.Components.DataProviders;
using WorkFlowLog.Components.DataProviders.Interfaces;
using WorkFlowLog.Components.ReportCreator;
using WorkFlowLog.Data;
using WorkFlowLog.Data.Entities;
using WorkFlowLog.Data.Repositories;

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
services.AddSingleton<ICsvReader, CsvReader>();
services.AddSingleton<IReportCreator, ReportCreator>();

var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetService<IApp>()!;

app.Run();
