using WorkFlowLog.Components.CsvReader;
using WorkFlowLog.Components.DataProviders;
using WorkFlowLog.Components.DataProviders.Interfaces;
using WorkFlowLog.Data.Entities;
using WorkFlowLog.Data.Repositories;

namespace WorkFlowLog;

public class App : IApp
{
    private readonly IUserCommunication _userCommunication;
    private readonly ICsvReader _csvReader;
    private readonly IRepository<Employee> _employeeRepository;
    private readonly IRepository<Project> _projectRepository;

    public App(IUserCommunication userCommunication,
        ICsvReader csvReader, 
        IRepository<Employee> employeeRepository, 
        IRepository<Project> projectRepository )
    {
        _userCommunication = userCommunication;
        _csvReader = csvReader;
        _employeeRepository = employeeRepository;
        _projectRepository = projectRepository;
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
            var key = _userCommunication.GetInputAndConvertToLower(string.Empty);

            switch (key)
            {
                case UserCommunication.ReadEmployeesCommand:
                    ReadAllEmployees();
                    break;

                case UserCommunication.AddEmployeeCommand:
                    AddEmployee();
                    break;

                case UserCommunication.RemoveEmployeeCommand:
                    RemoveEmployee();
                    break;

                case UserCommunication.ReadProjectsCommand:
                    ReadAllProjects();
                    break;

                case UserCommunication.AddProjectCommand:
                    AddProject();
                    break;

                case UserCommunication.RemoveProjectCommand:
                    RemoveProject();
                    break;

                case UserCommunication.LoadEmployeesFromCsvCommand:
                    InsertEmployees();
                    break;

                case UserCommunication.LoadProjectsFromCsvCommand:
                    InsertProjects();
                    break;

                case UserCommunication.ExitCommand:
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

        var allOrders = _projectRepository.GetAll().ToList();

        var projectToRemove = allOrders.FirstOrDefault
            (x => x.OrderId == int.Parse(projectNumber));

        if (projectToRemove == null)
        {
            _userCommunication.ShowWarning("Nie znaleziono zamówienia o podanym numerze");
            return;
        }

        _projectRepository.Remove(projectToRemove);
        _projectRepository.Save();

        _userCommunication.ShowMessage($"Usunięto projekt nr {projectToRemove.OrderId}");
    }

    void AddProject()
    {
        var projectName = _userCommunication.GetInput("Podaj nazwę projektu:");
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

        _projectRepository.Add(new Project
        {
            Name = projectName,
            OrderId = orderId,
            CustomerId = clientId
        });

        _projectRepository.Save();

        _userCommunication.ShowMessage($"Dodano projekt {projectName}");
    }

    void ReadAllProjects()
    {
        var projects = _projectRepository.GetAll().ToList();

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

        var allEmployees = _employeeRepository.GetAll().ToList();
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

            _employeeRepository.Remove(employeeToRemove);
            _employeeRepository.Save();

            _userCommunication.ShowMessage($"Usunięto pracownika {employeeToRemove.FirstName} {employeeToRemove.LastName}.");
            return;
        }
        else
        {
            var employeeToRemove = employeesToRemove.FirstOrDefault();
            _employeeRepository.Remove(employeeToRemove);
            _employeeRepository.Save();

            _userCommunication.ShowMessage($"Usunięto pracownika {employeeToRemove.FirstName} {employeeToRemove.LastName}.");
        }
    }

    void AddEmployee()
    {
        var firstName = _userCommunication.GetInput("Podaj imię pracownika:");

        if (firstName == null || firstName == "")
        {
            _userCommunication.ShowWarning("Błąd w danych pracownika");
            return;
        }

        var lastName = _userCommunication.GetInput("Podaj nazwisko pracownika:");
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
            _employeeRepository.Add(new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                FullTimeEmployee = result
            });

            _employeeRepository.Save();

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
        var employees = _employeeRepository.GetAll().ToList();

        _userCommunication.ShowMessage($"Ilość pracowników w bazie: {employees.Count}");

        _userCommunication.ShowEmployees(employees);
    }

    void InsertEmployees()
    {
        var deleteAll = _userCommunication.GetInput("Czy usunąć wszystkich pracowników? (Y/n)");
        if (deleteAll == "Y")
        {
            _employeeRepository.RemoveRange(_employeeRepository.GetAll());
            _employeeRepository.Save();
        }

        var employees = _csvReader.ProcessEmployees("Resources\\Files\\employees.csv");

        foreach (var employee in employees)
        {
            _employeeRepository.Add(new Employee
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                FullTimeEmployee = employee.FullTimeEmployee
            });
        }
        _employeeRepository.Save();

        _userCommunication.ShowMessage($"Ilość dodanych pracowników do bazy: {employees.Count}");
        _userCommunication.ShowMessage($"Ilość pracowników w bazie: {_employeeRepository.GetAll().ToList().Count}");
    }
    void InsertProjects()
    {
        var deleteAll = _userCommunication.GetInput("Czy usunąć wszystkie projekty? (Y/n)");
        if (deleteAll == "Y")
        {
            _projectRepository.RemoveRange(_projectRepository.GetAll());
            _employeeRepository.Save();
        }

        var projects = _csvReader.ProcessProjects("Resources\\Files\\projects.csv");

        foreach (var project in projects)
        {
            _projectRepository.Add(new Project
            {
                Name = project.Name,
                OrderId = project.OrderId,
                CustomerId = project.CustomerId
            });
        }
        _employeeRepository.Save();

        _userCommunication.ShowMessage($"Ilość dodanych projektów do bazy: {projects.Count}");
        _userCommunication.ShowMessage($"Ilość projektów w bazy: {_projectRepository.GetAll().ToList().Count}");
    }
}
