using Microsoft.Extensions.DependencyInjection;
using WorkFlowLog.ApplicationServices.Components.CsvFile;
using WorkFlowLog.ApplicationServices.Services;
using WorkFlowLog.DataAccess.Data;
using WorkFlowLog.DataAccess.Data.Entities;
using WorkFlowLog.DataAccess.Data.Repositories;
using WorkFlowLog.UI;

Console.Title = "WorkFlowLog";

var services = new ServiceCollection();

services.AddSingleton<IApp, App>();
services.AddSingleton<IRepository<Employee>, SqlRepository<Employee>>();
services.AddSingleton<IRepository<Project>, SqlRepository<Project>>();
services.AddSingleton<IRepository<Operation>, SqlRepository<Operation>>();
services.AddSingleton<IUserCommunication, UserCommunication>();
services.AddDbContext<WorkFlowDbContext>();
services.AddSingleton<ICsvReader, CsvReader>();
services.AddSingleton<ICsvWriter, CsvWriter>();
services.AddSingleton<IOperationsProvider, OperationsProvider>();
services.AddSingleton<IEmployeesProvider, EmployeesProvider>();
services.AddSingleton<IProjectsProvider, ProjectsProvider>();

var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetService<IApp>()!;

app.Run();
