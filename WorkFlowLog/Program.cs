using Microsoft.Extensions.DependencyInjection;
using WorkFlowLog;
using WorkFlowLog.Components.CsvReader;
using WorkFlowLog.Components.DataProviders;
using WorkFlowLog.Components.DataProviders.Interfaces;
using WorkFlowLog.Data;
using WorkFlowLog.Data.Entities;
using WorkFlowLog.Data.Repositories;

Console.Title = "WorkFlowLog";

var services = new ServiceCollection();

services.AddSingleton<IApp, App>();
services.AddSingleton<IRepository<Employee>, SqlRepository<Employee>>();
services.AddSingleton<IRepository<Project>, SqlRepository<Project>>();
services.AddSingleton<IUserCommunication, UserCommunication>();
services.AddDbContext<WorkFlowDbContext>();
services.AddSingleton<ICsvReader, CsvReader>();

var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetService<IApp>()!;

app.Run();
