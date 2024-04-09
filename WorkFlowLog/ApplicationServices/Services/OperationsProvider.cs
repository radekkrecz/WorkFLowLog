using WorkFlowLog.ApplicationServices.Components.CsvFile;
using WorkFlowLog.DataAccess.Data.Entities;
using WorkFlowLog.DataAccess.Data.Repositories;
using WorkFlowLog.UI;

namespace WorkFlowLog.ApplicationServices.Services;

internal class OperationsProvider : IOperationsProvider
{
    private readonly IRepository<Operation> _operationRepository;
    private readonly IEmployeesProvider _employeesProvider;
    private readonly IProjectsProvider _projectsProvider;
    private readonly IUserCommunication _userCommunication;
    private readonly ICsvReader _csvReader;
    private readonly ICsvWriter _csvWriter;

    private Employee _currentEmployee = new();
    private Project _currentProject = new();
    private Operation _currentOperation = new();

    public OperationsProvider(IRepository<Operation> operationRepository,
                            IEmployeesProvider employeesProvider,
                            IUserCommunication userCommunication,
                            IProjectsProvider projectsProvider,
                            ICsvWriter csvWriter,
                            ICsvReader csvReader)
    {
        _operationRepository = operationRepository;
        _employeesProvider = employeesProvider;
        _projectsProvider = projectsProvider;
        _userCommunication = userCommunication;
        _csvReader = csvReader;
        _csvWriter = csvWriter;
    }

    public void AddOperation(Operation operation)
    {
        _operationRepository.Add(operation);

        _operationRepository.Save();

        _userCommunication.ShowMessage($"Dodano czynność {operation.Name}");
    }


    Employee EditEmployeeInOperation(Operation operation)
    {
        var currentEmployee = _employeesProvider.GetEmployeeById(operation.EmployeeId);
        if (currentEmployee == null)
        {
            _userCommunication.ShowWarning("\nW bazie nie znaleziono pracownika przypisanego do czynności.");
        }
        else
        {
            _userCommunication.ShowMessage($"\nPracownik {currentEmployee.FirstName} {currentEmployee.LastName}");
        }

        var employeeLastName = _userCommunication.GetInputAndConvertToLower("Podaj nowe nazwisko pracownika:");

        if (employeeLastName == null || employeeLastName == "")
        {
            employeeLastName = currentEmployee!.LastName;
        }

        var employee = _employeesProvider.LookForEmployeeByLastName(employeeLastName);

        if (employee.Id == 0)
        {
            _userCommunication.ShowWarning("Nie znaleziono pracownika o podanym nazwisku\n");
            return new();
        }

        if (operation.EmployeeId != employee.Id)
            _userCommunication.ShowMessage($"Zmieniono pracownika na {employee.FirstName} {employee.LastName}\n");

        return employee;
    }

    Project EditProjectInOperation(Operation operation)
    {
        var currentProject = _projectsProvider.GetProjectById(operation.ProjectId);
        if (currentProject == null)
        {
            _userCommunication.ShowWarning("\nW bazie nie znaleziono projektu przypisanego do czynności.");
        }
        else
        {
            _userCommunication.ShowMessage($"\nProjekt {currentProject.Name}");
        }

        var projectName = _userCommunication.GetInput("Podaj nową nazwę projektu:");

        if (projectName == null || projectName == "")
        {
            projectName = currentProject!.Name;
        }

        var project = _projectsProvider.LookForProjectByName(projectName);

        if (project.Id == 0)
        {
            _userCommunication.ShowWarning("Nie znaleziono projektu o podanej nazwie");
            return new();
        }

        if (operation.ProjectId != project.Id)
            _userCommunication.ShowMessage($"Zmieniono projekt na {project.Name}");

        return project;
    }

    public void EditOperation()
    {
        var operationToEdit = AskForOperationAndCheckInDb();

        if (operationToEdit.Id == 0)
        {
            return;
        }

        _userCommunication.ShowMessage($"Nazwa czynności {operationToEdit.Name}");
        var operationName = _userCommunication.GetInput("Podaj nową nazwę czynności:");

        if (operationName == null || operationName == "")
        {
            operationName = operationToEdit.Name;
        }

        var employee = EditEmployeeInOperation(operationToEdit);

        if (employee.Id == 0)
        {
            return;
        }

        var project = EditProjectInOperation(operationToEdit);

        if (project.Id == 0)
        {
            return;
        }

        _userCommunication.ShowMessage($"\nData rozpoczęcia {operationToEdit.StartDate:g}");
        var startDate = _userCommunication.GetInput("Podaj nową datę rozpoczęcia czynności (yyyy-mm-dd hh:mm):");

        if (startDate == null || startDate == "")
        {
            startDate = operationToEdit.StartDate.ToString();
        }

        _userCommunication.ShowMessage($"\nData zakończenia {operationToEdit.EndDate:g}");
        var endDate = _userCommunication.GetInput("Podaj nową datę zakończenia czynności (yyyy-mm-dd hh:mm):");

        if (endDate == null || endDate == "")
        {
            endDate = operationToEdit.EndDate.ToString();
        }

        if (DateTime.TryParse(startDate, out DateTime start) && DateTime.TryParse(endDate, out DateTime end))
        {
            if (start > end)
            {
                _userCommunication.ShowWarning("Data rozpoczęcia nie może być późniejsza niż data zakończenia");
                return;
            }

            operationToEdit.Name = operationName;
            operationToEdit.EmployeeId = employee.Id;
            operationToEdit.ProjectId = project.Id;
            operationToEdit.StartDate = start;
            operationToEdit.EndDate = end;
            operationToEdit.isActive = true;


            _operationRepository.Update(operationToEdit);

            _operationRepository.Save();

            _userCommunication.ShowMessage($"Zmieniono czynność {operationToEdit.Name}");
        }
        else
        {
            _userCommunication.ShowWarning("Błąd w danych daty");
            return;
        }
    }

    public Operation? GetCurrentlyActiveOperationByEmployee(Employee employee)
    {
        return _operationRepository
            .GetAllActive()
            .FirstOrDefault(x => x.EmployeeId == employee.Id && x.EndDate == x.StartDate);
    }

    public Employee GetCurrentEmployee()
    {
        return _currentEmployee;
    }

    public Project GetCurrentProject()
    {
        return _currentProject;
    }

    public void ReadAllOperations()
    {
        var operations = _operationRepository.GetAllActive().ToList();

        _userCommunication.ShowOperations(operations);

        _userCommunication.ShowMessage($"Ilość czynności w bazie: {operations.Count}");
    }

    public void ReadOperationsByEmployee()
    {
        if (_currentEmployee.Id == 0)
        {
            if (!SetCurrentEmployee())
            {
                return;
            }
        }

        var operations = _operationRepository
            .GetAllActive()
            .Where(x => x.EmployeeId == _currentEmployee.Id)
            .OrderBy(x => x.StartDate)
            .ThenBy(x => x.ProjectId)
            .ToList();

        _userCommunication.ShowOperations(operations);

        _userCommunication.ShowMessage($"Ilość czynności pracownika {_currentEmployee.FirstName} {_currentEmployee.LastName}: {operations.Count}");

    }

    public void ReadOperationsByProject()
    {
        if (_currentProject.Id == 0)
        {
            if (!SetCurrentProject())
            {
                return;
            }
        }

        var operations = _operationRepository
            .GetAllActive()
            .Where(x => x.ProjectId == _currentProject.Id)
            .OrderBy(x => x.StartDate)
            .ThenBy(x => x.EmployeeId)
            .ToList();

        _userCommunication.ShowOperations(operations);

        _userCommunication.ShowMessage($"Ilość czynności w projekcie {_currentProject.Name}: {operations.Count}");
    }

    public void RemoveOperation()
    {
        var operationToRemove = AskForOperationAndCheckInDb();

        if (operationToRemove.Id == 0)
        {
            return;
        }

        operationToRemove.isActive = false;   

        _operationRepository.Update(operationToRemove);
        _operationRepository.Save();

        _userCommunication.ShowMessage($"Usunięto czynność {operationToRemove.Name}.");

    }

    public void StartOperation()
    {
        if (_currentEmployee.Id == 0)
        {
            if (!SetCurrentEmployee())
            {
                return;
            }
        }
        else
        {
            _userCommunication.ShowMessage($"Bieżący pracownik: {_currentEmployee.FirstName} {_currentEmployee.LastName}");
            var key = _userCommunication.GetInput("Czy chcesz zmienić pracownika? (Y/n)");
            if (key != null)
            {
                if (key == "Y")
                {
                    if (!SetCurrentEmployee())
                    {
                        return;
                    }
                }
            }
        }

        if (_currentProject.Id == 0)
        {
            if (!SetCurrentProject())
            {
                return;
            }
        }
        else
        {
            _userCommunication.ShowMessage($"Bieżący projekt: {_currentProject.Name}");
            var key = _userCommunication.GetInput("Czy chcesz zmienić projekt? (Y/n)");
            if (key != null)
            {
                if (key == "Y")
                {
                    if (!SetCurrentProject())
                    {
                        return;
                    }
                }
            }
        }

        if (_currentOperation.ProjectId != 0)
        {
            var key = _userCommunication.GetInput("Czy chcesz zakończyć bieżącą czynność (Y/n)");
            if (key == "Y")
            {
                StopOperation();
            }
            else
            {
                _userCommunication.ShowWarning("Nie można rozpocząć nowej czynności, jeśli poprzednia nie została zakończona");
                return;
            }
        }

        var operationName = _userCommunication.GetInput("Podaj nazwę czynności:");

        if (operationName == null || operationName == "")
        {
            _userCommunication.ShowWarning("Błąd w danych czynności");
            return;
        }

        var operation = new Operation
        {
            Name = operationName,
            EmployeeId = _currentEmployee.Id,
            ProjectId = _currentProject.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now,
            isActive = true
        };

        AddOperation(operation);

        _currentOperation = operation;

        _userCommunication.ShowMessage($"Rozpoczęto czynność {operationName} o {_currentOperation.StartDate:g} w {_currentProject.Name}");
    }

    public void StopOperation()
    {
        if (_currentOperation.Id == 0)
        {
            _userCommunication.ShowWarning("Nie ustawiono bieżącej czynności");
            return;
        }

        _currentOperation.EndDate = DateTime.Now;

        _operationRepository.Update(_currentOperation);
        _operationRepository.Save();

        _userCommunication.ShowMessage($"Zakończono czynność {_currentOperation.Name} o {_currentOperation.EndDate} w {_currentProject.Name}");
        _currentOperation = new();
    }

    public void InsertOperations()
    {
        if (_operationRepository.GetAll().ToList().Count == 0)
        {
            _userCommunication.ShowWarning("Baza jest pusta. Wczytuję dane z pliku CSV.");
        }
        else
        {
            var deleteAll = _userCommunication.GetInput("Czy usunąć wszystkie czynności? (Y/n)");
            if (deleteAll == "Y")
            {
                _operationRepository.RemoveRange(_operationRepository.GetAll());
                _operationRepository.Save();
            }
        }

        var operations = _csvReader.ProcessOperations("DataAccess\\Resources\\Files\\operations.csv");

        foreach (var operation in operations)
        {
            try
            {
                _operationRepository.Add(new Operation
                {
                    Name = operation.Name,
                    EmployeeId = _employeesProvider.GetEmployeeByLastName(operation.EmployeeLastName).Id,
                    ProjectId = _projectsProvider.GetProjectByName(operation.ProjectName).Id,
                    StartDate = operation.StartDate,
                    EndDate = operation.EndDate,
                    isActive = true
                });
            }
            catch (NullReferenceException e)
            {
                _userCommunication.ShowWarning($"Nie znaleziono pracownika lub projektu w bazie: {operation.EmployeeLastName} {operation.ProjectName}");
                var askForProceed = _userCommunication.GetInput("Czy kontynuować? (Y/n)");
                if (askForProceed != "Y")
                {
                    return;
                }
            }
            catch (Exception e)
            {
                _userCommunication.ShowWarning($"Błąd w danych czynności: {operation.EmployeeFirstName} {operation.EmployeeLastName} {operation.Name} {operation.StartDate}-{operation.EndDate}");
                return;
            }

        }
        _operationRepository.Save();

        _userCommunication.ShowMessage($"Ilość dodanych czynności do bazy: {operations.Count}");
        _userCommunication.ShowMessage($"Ilość czynności w bazie: {_operationRepository.GetAll().ToList().Count}");
    }

    public bool SetCurrentEmployee()
    {
        var lastName = _userCommunication.GetInputAndConvertToLower("Podaj nazwisko pracownika:");

        if (lastName == null)
        {
            return false;
        }

        var employee = _employeesProvider.LookForEmployeeByLastName(lastName);

        if (employee.Id == 0)
        {
            return false;
        }

        _currentEmployee = employee;

        _userCommunication.ShowMessage($"Ustawiono bieżącego pracownika: {employee.FirstName} {employee.LastName}");

        return true;
    }

    public bool SetCurrentProject()
    {
        var projectName = _userCommunication.GetInput("Podaj nazwę projektu:");

        if (projectName == null)
        {
            return false;
        }

        var project = _projectsProvider.LookForProjectByName(projectName);

        if (project.Id == 0)
        {
            return false;
        }

        _currentProject = project;

        _userCommunication.ShowMessage($"Ustawiono bieżący projekt: {project.Name}");

        return true;
    }

    public void AddNewOperation()
    {
        var operationName = _userCommunication.GetInput("Podaj nazwę czynności:");

        if (operationName == null || operationName == "")
        {
            _userCommunication.ShowWarning("Błąd w danych czynności");
            return;
        }

        var employeeLastName = _userCommunication.GetInputAndConvertToLower("Podaj nazwisko pracownika:");

        if (employeeLastName == null || employeeLastName == "")
        {
            _userCommunication.ShowWarning("Błąd w danych pracownika");
            return;
        }

        var employee = _employeesProvider.LookForEmployeeByLastName(employeeLastName);

        if (employee.Id == 0)
        {
            _userCommunication.ShowWarning("Nie znaleziono pracownika o podanym nazwisku");
            return;
        }

        var projectName = _userCommunication.GetInput("Podaj nazwę projektu:");

        if (projectName == null || projectName == "")
        {
            _userCommunication.ShowWarning("Błąd w danych projektu");
            return;
        }

        var project = _projectsProvider.LookForProjectByName(projectName);
        if (project == null)
        {
            _userCommunication.ShowWarning("Nie znaleziono projektu o podanej nazwie");
            return;
        }

        var startDate = _userCommunication.GetInput("Podaj datę rozpoczęcia czynności (yyyy-mm-dd hh:mm):");

        if (startDate == null || startDate == "")
        {
            _userCommunication.ShowWarning("Błąd w danych daty rozpoczęcia");
            return;
        }

        var endDate = _userCommunication.GetInput("Podaj datę zakończenia czynności (yyyy-mm-dd hh:mm):");

        if (endDate == null || endDate == "")
        {
            _userCommunication.ShowWarning("Błąd w danych daty zakończenia");
            return;
        }

        if (DateTime.TryParse(startDate, out DateTime start) && DateTime.TryParse(endDate, out DateTime end))
        {
            if (start > end)
            {
                _userCommunication.ShowWarning("Data rozpoczęcia nie może być późniejsza niż data zakończenia");
                return;
            }

            var res = _operationRepository.Any(x => x.StartDate <= start && x.EndDate >= end);

            if (res)
            {
                _userCommunication.ShowWarning("Czynność nie może się pokrywać z inną czynnością");
                return;
            }

            _operationRepository.Add(new Operation
            {
                Name = operationName,
                EmployeeId = employee.Id,
                ProjectId = project.Id,
                StartDate = start,
                EndDate = end,
                isActive = true
            });

            _operationRepository.Save();

            _userCommunication.ShowMessage($"Dodano czynność {operationName}");
        }
        else
        {
            _userCommunication.ShowWarning("Błąd w danych daty");
            return;
        }
    }

    public void CreateReportForEmployee()
    {
        var employee = _employeesProvider.AskForEmployeeAndCheckInDb();

        if (employee.Id == 0)
        {
            return;
        }

        var startDate = _userCommunication.GetInput("Podaj datę początkową raportu (yyyy-mm-dd):");

        if (startDate == null || !DateTime.TryParse(startDate, out DateTime start))
        {
            _userCommunication.ShowWarning("Błąd w danych daty początkowej");
            return;
        }

        var endDate = _userCommunication.GetInput("Podaj datę końcową raportu (yyyy-mm-dd):");

        if (endDate == null || !DateTime.TryParse(endDate, out DateTime end))
        {
            _userCommunication.ShowWarning("Błąd w danych daty końcowej");
            return;
        }

        if (start > end)
        {
            _userCommunication.ShowWarning("Data początkowa nie może być późniejsza niż data końcowa");
            return;
        }

        _userCommunication.ShowMessage("Wybierz kolejność sortowania:");

        _userCommunication.ShowMessage("1 - Data rozpoczęcia rosnąco");
        _userCommunication.ShowMessage("2 - Data rozpoczęcia malejąco");
        _userCommunication.ShowMessage("3 - Projekt rosnąco");
        _userCommunication.ShowMessage("4 - Projekt malejąco");

        var key = _userCommunication.GetInput(string.Empty);

        if (key == null || !int.TryParse(key, out int sort))
        {
            _userCommunication.ShowWarning("Błąd w danych sortowania");
            return;
        }

        var operationsTemporary = _operationRepository
                    .GetAllActive()
                    .Where(x => x.EmployeeId == employee.Id)
                    .Where(x => x.StartDate >= start && x.EndDate <= end);


        var operations = new List<Operation>(); 

        switch (sort)
        {
            default:
            case 1:
                operations = operationsTemporary
                    .OrderBy(x => x.StartDate)
                    .ThenBy(x => x.Name)
                    .ToList();
                break;

            case 2:
                operations = operationsTemporary
                    .OrderByDescending(x => x.StartDate)
                    .ThenBy(x => x.Name)
                    .ToList();
                break;

            case 3:
                operations = operationsTemporary
                    .OrderBy(x => x.ProjectId)
                    .ThenBy(x => x.Name)
                    .ToList();
                break;

            case 4:
                operations = operationsTemporary
                    .OrderByDescending(x => x.ProjectId)
                    .ThenBy(x => x.Name)
                    .ToList();
                break;
        }

        _userCommunication.ShowOperations(operations);

        _userCommunication.ShowMessage($"Ilość czynności pracownika {employee.FirstName} {employee.LastName}: {operations.Count}");

        var sumOfWorkingHours = GetSumOfWorkingHours(operations);

        _userCommunication.ShowMessage($"Suma czasu pracy: {(int)(sumOfWorkingHours.TotalHours)}:{sumOfWorkingHours.Minutes:D2}");

        List<Components.CsvFile.Models.Operation> operationsToSave = new();

        foreach (var operation in operations)
        {
            operationsToSave.Add(new Components.CsvFile.Models.Operation
            {
                Name = operation.Name,
                ProjectName = _projectsProvider.GetProjectById(operation.ProjectId).Name,
                EmployeeFirstName = _employeesProvider.GetEmployeeById(operation.EmployeeId).FirstName,
                EmployeeLastName = _employeesProvider.GetEmployeeById(operation.EmployeeId).LastName,
                StartDate = operation.StartDate,
                EndDate = operation.EndDate
            });
        }

        string filePath = _userCommunication.GetInput("Podaj ścieżkę do pliku CSV:");

        if (filePath == null || filePath == "")
        {
            filePath = $"DataAccess\\Resources\\Files\\report_{employee.FirstName}_{employee.LastName}.csv";
        }

        var fileName = filePath.Split(".").First();
        filePath = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}.csv";

        _csvWriter.WriteOperations(filePath, operationsToSave, sumOfWorkingHours);

        _userCommunication.ShowMessage($"Raport zapisano w {filePath}");

    }

    public TimeSpan GetSumOfWorkingHours(List<Operation> operations)
    {
        TimeSpan sum = new();

        foreach (var operation in operations)
        {
            sum += operation.EndDate - operation.StartDate;
        }

        return sum;
    }

    Operation AskForOperationAndCheckInDb()
    {
        var operationName = _userCommunication.GetInput("Podaj nazwę czynności:");

        if (operationName == null)
        {
            return new();
        }

        var operation = LookForOperationByName(operationName);

        if (operation == null || operation.Id == 0)
        {
            return new();
        }

        return operation;
    }

    Operation LookForOperationByName(string name)
    {
        var operations = _operationRepository.GetAllActive()
                    .Where(x => x.Name.ToLower().Contains(name.ToLower()))
                    .OrderBy(x => x.Name)
                    .ToList();

        if (operations == null || operations.Count() == 0)
        {
            _userCommunication.ShowWarning($"Nie znaleziono czynności {name}");
            return new();
        }

        if (operations.Count() > 50)
        {
            _userCommunication.ShowWarning("Znaleziono więcej niż 50 czynności o podanej nazwie. Podaj więcej szczegółów.");
            return new();
        }

        if (operations.Count() > 1)
        {
            _userCommunication.ShowWarning($"Znaleziono kilka czynności o podanej nazwie:");
            foreach (var operation in operations)
            {
                _userCommunication.ShowOperation(operation);
            }

            var operationId = _userCommunication.GetInput("Podaj ID czynności:");

            if (operationId == null || !int.TryParse(operationId, out int id))
            {
                _userCommunication.ShowWarning("Nie znaleziono czynności o podanym ID");
                return new();
            }

            return operations.Where(x => x.Id == id).ToList().FirstOrDefault()!;
        }
        else
        {
            var operation = operations.First();
            _userCommunication.ShowMessage($"Znaleziono w bazie czynność {operation.Name}");

            return operation;
        }
    }
}
