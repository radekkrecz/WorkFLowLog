using System.Text;
using System.Xml.Linq;
using WorkFlowLog.Components.CsvReader;
using WorkFlowLog.Components.DataProviders.Interfaces;
using WorkFlowLog.Data;

namespace WorkFlowLog;

public class App : IApp
{
    private readonly IUserCommunication _userCommunication;
    private readonly ICsvReader _csvReader;
    private readonly WorkFlowDbContext _workFlowDbContext;

    public App(IUserCommunication userCommunication, ICsvReader csvReader, WorkFlowDbContext workFlowDbContext)
    {
        _userCommunication = userCommunication;
        _csvReader = csvReader;
        _workFlowDbContext = workFlowDbContext;

        _workFlowDbContext.Database.EnsureCreated();
    }

    public void Run()
    {
        _userCommunication.ShowMessage("Witaj w programie WorkFlowLog");
        _userCommunication.ShowMessage("WorkFlowLog to aplikacja zaprojektowana w celu usprawnienia " +
            "śledzenia czasu i zarządzania godzinami pracy pracowników, przydzielonymi zadaniami " +
            "i harmonogramami realizacji projektów.");

        bool exitKeyPressed = false;

        while (!exitKeyPressed)
        {
            _userCommunication.ShowMenu();
            var key = _userCommunication.GetInput(string.Empty);

            switch (key)
            {
                case "1":
                    ReadAllEmployees();
                    break;

                case "2":
                    AddEmployee();
                    break;

                case "3":
                    RemoveEmployee();
                    break;

                case "4":
                    ReadAllProjects();
                    break;

                case "5":
                    AddProject();
                    break;

                case "6":
                    RemoveProject();
                    break;

                case "7":
                    InsertEmployees();
                    break;

                case "8":
                    InsertProjects();
                    break;

                case "Q":
                    exitKeyPressed = true;
                    break;

                default:
                    _userCommunication.ShowWarning("\n\nNieznana komenda!");
                    break;
            }
        }
    }
    void RemoveProject()
    {
        var projectNumber = _userCommunication.GetInput("Podaj numer projektu do usunięcia:");

        if (projectNumber == null)
        {
            return;
        }

        var allOrders = _workFlowDbContext.Projects.ToList();

        var projectToRemove = allOrders.FirstOrDefault
            (x => x.OrderId == int.Parse(projectNumber));

        if (projectToRemove == null)
        {
            _userCommunication.ShowWarning("Nie znaleziono zamówienia o podanym numerze");
            return;
        }

        _workFlowDbContext.Projects.Remove(projectToRemove);
        _workFlowDbContext.SaveChanges();

        _userCommunication.ShowMessage($"Usunięto projekt nr {projectToRemove.OrderId}");
    }

    void AddProject()
    {
        var projectName = _userCommunication.GetInput("Podaj nazwę projektu:", false);
        if (projectName == null || projectName == "")
        {
            _userCommunication.ShowWarning("Błąd w danych projektu");
            return;
        }

        _userCommunication.ShowMessage("Podaj numer zlecenia dla projektu:");
        var orderNumber = _userCommunication.GetInput(string.Empty);
        if (orderNumber == null || !int.TryParse(orderNumber, out int orderId))
        {
            _userCommunication.ShowWarning("Błąd w danych projektu");
            return;
        }

        _userCommunication.ShowMessage("Podaj identyfikator klienta dla projektu:");
        var clientNumber = _userCommunication.GetInput(string.Empty);
        if (clientNumber == null || !int.TryParse(clientNumber, out int clientId))
        {
            _userCommunication.ShowWarning("Błąd w danych projektu");
            return;
        }

        _workFlowDbContext.Projects.Add(new Data.Entities.Project
        {
            Name = projectName,
            OrderId = orderId,
            CustomerId = clientId
        });

        _workFlowDbContext.SaveChanges();

        _userCommunication.ShowMessage($"Dodano projekt {projectName}");
    }

    void ReadAllProjects()
    {
        var projects = _workFlowDbContext.Projects.ToList();

        _userCommunication.ShowMessage($"Ilość zamówień w bazie: {projects.Count}");

        _userCommunication.ShowProjects(projects);
    }

    void RemoveEmployee()
    {
        var lastName = _userCommunication.GetInput("Podaj nazwisko pracownika do usunięcia:");

        if (lastName == null)
        {
            return;
        }

        var allEmployees = _workFlowDbContext.Employees.ToList();
        var employeesToRemove = allEmployees.Where(x => x.LastName == lastName).ToList();

        if (employeesToRemove == null)
        {
            _userCommunication.ShowWarning("Nie znaleziono pracownika o podanym numerze PESEL");
            return;
        }

        if (employeesToRemove.Count() > 1)
        {
            _userCommunication.ShowMessage("Znaleziono kilku pracowników o podanym nazwisku:");
            _userCommunication.ShowEmployees(employeesToRemove);

            var employeeNumber = _userCommunication.GetInput("Podaj ID pracownika do usunięcia:");

            if (employeeNumber == null || !int.TryParse(employeeNumber, out int employeeId))
            {
                return;
            }

            var employeeToRemove = employeesToRemove.FirstOrDefault(x => x.Id == employeeId);

            if (employeeToRemove == null)
            {
                _userCommunication.ShowWarning("Nie znaleziono pracownika o podanym numerze ID");
                return;
            }

            _workFlowDbContext.Employees.Remove(employeeToRemove);
            _workFlowDbContext.SaveChanges();

            _userCommunication.ShowMessage($"Usunięto pracownika {employeeToRemove.FirstName} {employeeToRemove.LastName}.");
            return;
        }
        else
        {
            var employeeToRemove = employeesToRemove.FirstOrDefault();
            _workFlowDbContext.Employees.Remove(employeeToRemove);
            _workFlowDbContext.SaveChanges();

            _userCommunication.ShowMessage($"Usunięto pracownika {employeeToRemove.FirstName} {employeeToRemove.LastName}.");
        }
    }

    void AddEmployee()
    {
        var firstName = _userCommunication.GetInput("Podaj imię pracownika:", false);

        if (firstName == null || firstName == "")
        {
            _userCommunication.ShowWarning("Błąd w danych pracownika");
            return;
        }

        var lastName = _userCommunication.GetInput("Podaj nazwisko pracownika:", false);
        if (lastName == null || lastName == "")
        {
            _userCommunication.ShowWarning("Błąd w danych pracownika");
            return;
        }

        var hourlyRate = _userCommunication.GetInput("Podaj wymiar etatu pracownika:");
        if (hourlyRate == null)
        {
            _userCommunication.ShowWarning("Błąd w danych pracownika");
            return;
        }

        if (double.TryParse(hourlyRate, out double result))
        {
            _workFlowDbContext.Employees.Add(new Data.Entities.Employee
            {
                FirstName = firstName,
                LastName = lastName,
                FullTimeEmployee = result
            });

            _workFlowDbContext.SaveChanges();

            _userCommunication.ShowMessage($"Dodano pracownika {firstName} {lastName}");
        }
        else
        {
            _userCommunication.ShowWarning("Błąd w danych pracownika");
            return;
        }
    }
    void ReadAllEmployees()
    {
        var employees = _workFlowDbContext.Employees.ToList();

        _userCommunication.ShowMessage($"Ilość pracowników w bazie: {employees.Count}");

        _userCommunication.ShowEmployees(employees);
    }

    void InsertEmployees()
    {
        var deleteAll = _userCommunication.GetInput("Czy usunąć wszystkich pracowników? (Y/n)", false);
        if (deleteAll == "Y")
        {
            _workFlowDbContext.Employees.RemoveRange(_workFlowDbContext.Employees);
            _workFlowDbContext.SaveChanges();
        }

        var employees = _csvReader.ProcessEmployees("Resources\\Files\\employees.csv");

        foreach (var employee in employees)
        {
            _workFlowDbContext.Employees.Add(new Data.Entities.Employee
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                FullTimeEmployee = employee.FullTimeEmployee
            });
        }
        _workFlowDbContext.SaveChanges();

        _userCommunication.ShowMessage($"Ilość pracowników w bazie: {employees.Count}");
    }
    void InsertProjects()
    {
        var deleteAll = _userCommunication.GetInput("Czy usunąć wszystkie projekty? (Y/n)", false);
        if (deleteAll == "Y")
        {
            _workFlowDbContext.Projects.RemoveRange(_workFlowDbContext.Projects);
            _workFlowDbContext.SaveChanges();
        }

        var projects = _csvReader.ProcessProjects("Resources\\Files\\projects.csv");

        foreach (var project in projects)
        {
            _workFlowDbContext.Projects.Add(new Data.Entities.Project
            {
                Name = project.Name,
                OrderId = project.OrderId,
                CustomerId = project.CustomerId
            });
        }
        _workFlowDbContext.SaveChanges();

        _userCommunication.ShowMessage($"Ilość projektów w bazie: {projects.Count}");
    }
}
