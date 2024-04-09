using WorkFlowLog.ApplicationServices.Components.CsvFile;
using WorkFlowLog.DataAccess.Data.Entities;
using WorkFlowLog.DataAccess.Data.Repositories;
using WorkFlowLog.UI;

namespace WorkFlowLog.ApplicationServices.Services;

public class EmployeesProvider : IEmployeesProvider
{
    private readonly IRepository<Employee> _employeeRepository;
    private readonly IUserCommunication _userCommunication;
    private readonly ICsvReader _csvReader;

    public EmployeesProvider(IRepository<Employee> employeeRepository,
                             IUserCommunication userCommunication,
                             ICsvReader csvReader)
    {
        _employeeRepository = employeeRepository;
        _userCommunication = userCommunication;
        _csvReader = csvReader;
    }
    public List<Employee> GetEmployees()
    {
        return _employeeRepository.GetAllActive().ToList();
    }

    public Employee? GetEmployeeById(int id)
    {
        return _employeeRepository.GetById(id);
    }

    public void InsertEmployees()
    {
        if (_employeeRepository.GetAll().ToList().Count == 0)
        {
            _userCommunication.ShowWarning("Baza jest pusta. Wczytuję dane z pliku CSV.");
        }
        else
        {
            var deleteAll = _userCommunication.GetInput("Czy usunąć wszystkich pracowników? (Y/n)");
            if (deleteAll == "Y")
            {
                _employeeRepository.RemoveRange(_employeeRepository.GetAll());
                _employeeRepository.Save();
            }
        }

        var employees = _csvReader.ProcessEmployees("DataAccess\\Resources\\Files\\employees.csv");

        if (employees == null)
        {
            _userCommunication.ShowWarning("Brak pracowników do dodania");
            return;
        }

        foreach (var employee in employees)
        {
            _employeeRepository.Add(new Employee
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                FullTimeEmployee = employee.FullTimeEmployee,
                isActive = true
            });
        }
        _employeeRepository.Save();

        _userCommunication.ShowMessage($"Ilość dodanych pracowników do bazy: {employees.Count}");
        _userCommunication.ShowMessage($"Ilość pracowników w bazie: {_employeeRepository.GetAll().ToList().Count}");
    }

    public void ReadAllEmployees()
    {
        var employees = _employeeRepository.GetAllActive().ToList();

        _userCommunication.ShowEmployees(employees);

        _userCommunication.ShowMessage($"Ilość pracowników w bazie: {employees.Count}");
    }

    public void AddEmployee()
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
                FullTimeEmployee = result,
                isActive = true
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

    public void EditEmployee()
    {
        var employeeToEdit = AskForEmployeeAndCheckInDb();

        if (employeeToEdit == null || employeeToEdit.Id == 0)
        {
            return;
        }

        _userCommunication.ShowMessage($"Imię pracownka: {employeeToEdit.FirstName}");
        var firstName = _userCommunication.GetInput("Podaj nowe imię pracownika:");
        if (firstName == null || firstName == "")
        {
            firstName = employeeToEdit.FirstName;
        }

        _userCommunication.ShowMessage($"Nazwisko pracownika: {employeeToEdit.LastName}");
        var lastName = _userCommunication.GetInput("Podaj nowe nazwisko pracownika:");
        if (lastName == null || lastName == "")
        {
            lastName = employeeToEdit.LastName;
        }

        _userCommunication.ShowMessage($"Wymiar etatu pracownika: {employeeToEdit.FullTimeEmployee}");
        var hourlyRateString = _userCommunication.GetInput("Podaj nowy wymiar etatu pracownika:");
        if (hourlyRateString == null || !double.TryParse(hourlyRateString, out double hourlyRate))
        {
            hourlyRate = employeeToEdit.FullTimeEmployee;
        }

        employeeToEdit.FirstName = firstName;
        employeeToEdit.LastName = lastName;
        employeeToEdit.FullTimeEmployee = hourlyRate;
        employeeToEdit.isActive = true;

        _employeeRepository.Update(employeeToEdit);
        _employeeRepository.Save();

        _userCommunication.ShowMessage($"Zaktualizowano dane pracownika {firstName} {lastName}");

    }

    public void RemoveEmployee()
    {
        var employeeToRemove = AskForEmployeeAndCheckInDb();

        if (employeeToRemove == null || employeeToRemove.Id == 0)
        {
            return;
        }

        employeeToRemove.isActive = false;    

        _employeeRepository.Update(employeeToRemove);
        _employeeRepository.Save();

        _userCommunication.ShowMessage($"Usunięto pracownika {employeeToRemove.FirstName} {employeeToRemove.LastName}.");

    }

    public Employee AskForEmployeeAndCheckInDb()
    {
        var lastName = _userCommunication.GetInput("Podaj nazwisko pracownika:");
        if (lastName == null)
        {
            return new();
        }

        var employee = LookForEmployeeByLastName(lastName);

        if (employee == null || employee.Id == 0)
        {
            _userCommunication.ShowWarning("Nie znaleziono pracownika o podanym nazwisku");
            return new();
        }

        return employee;
    }

    public Employee LookForEmployeeByLastName(string lastName)
    {
        var employees = _employeeRepository.GetAllActive()
                    .Where(x => x.LastName.ToLower().Contains(lastName.ToLower()))
                    .OrderBy(x => x.LastName)
                    .ToList();

        if (employees == null || employees.Count() == 0)
        {
            Console.WriteLine($"Nie znaleziono pracownika {lastName}");
            return new();
        }

        if (employees.Count() > 10)
        {
            _userCommunication.ShowWarning("Znaleziono więcej niż 10 pracowników o podanym nazwisku. Podaj więcej szczegółów.");
            return new();
        }

        if (employees.Count() > 1)
        {
            _userCommunication.ShowWarning($"Znaleziono kilku pracowników o podanym nazwisku:");
            foreach (var employee in employees)
            {
                _userCommunication.ShowMessage($"{employee.Id} - {employee.FirstName} {employee.LastName}");
            }

            var employeeId = _userCommunication.GetInput("Podaj ID pracownika:");

            if (employeeId == null || !int.TryParse(employeeId, out int id))
            {
                return new();
            }

            return employees.Where(x => x.Id == id).ToList().FirstOrDefault()!;
        }
        else
        {
            var employee = employees.First();
            _userCommunication.ShowMessage($"Znaleziono w bazie pracownika {employee.FirstName} {employee.LastName}");

            return employee;
        }
    }

    public Employee GetEmployeeByLastName(string lastName)
    {
        return _employeeRepository.GetAllActive()
            .Where(x => x.LastName == lastName).FirstOrDefault()!;
    }
}
