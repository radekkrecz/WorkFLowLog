using WorkFlowLog.ApplicationServices.Components.CsvFile;
using WorkFlowLog.DataAccess.Data.Entities;
using WorkFlowLog.DataAccess.Data.Repositories;
using WorkFlowLog.UI;

namespace WorkFlowLog.ApplicationServices.Services;

public class ProjectsProvider : IProjectsProvider
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IUserCommunication _userCommunication;
    private readonly ICsvReader _csvReader;

    public ProjectsProvider(IRepository<Project> projectRepository,
                            IUserCommunication userCommunication,
                            ICsvReader csvReader)
    {
        _projectRepository = projectRepository;
        _userCommunication = userCommunication;
        _csvReader = csvReader;
    }

    public void AddProject()
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
            CustomerId = clientId,
            isActive = true,
        });

        _projectRepository.Save();

        _userCommunication.ShowMessage($"Dodano projekt {projectName}");
    }

    public void EditProject()
    {
        var projectToEdit = AskForProjectAndCheckInDb();

        if (projectToEdit.Id == 0)
        {
            _userCommunication.ShowWarning("Nie znaleziono projektu o podanej nazwie");
            return;
        }

        _userCommunication.ShowMessage($"Nazwa projektu: {projectToEdit.Name}");
        var newName = _userCommunication.GetInput("Podaj nową nazwę projektu:");
        if (newName == null || newName == "")
        {
            newName = projectToEdit.Name;
        }

        _userCommunication.ShowMessage($"Numer zlecenia: {projectToEdit.OrderId}");
        var newOrderNumber = _userCommunication.GetInput("Podaj nowy numer zlecenia dla projektu:");
        if (newOrderNumber == null || !int.TryParse(newOrderNumber, out int newOrderId))
        {
            newOrderId = projectToEdit.OrderId;
        }

        _userCommunication.ShowMessage($"Identyfikator klienta: {projectToEdit.CustomerId}");
        var newClientNumber = _userCommunication.GetInput("Podaj nowy identyfikator klienta dla projektu:");

        if (newClientNumber == null || !int.TryParse(newClientNumber, out int newClientId))
        {
            newClientId = projectToEdit.CustomerId;
        }

        projectToEdit.Name = newName;
        projectToEdit.OrderId = newOrderId;
        projectToEdit.CustomerId = newClientId;
        projectToEdit.isActive = true;

        _projectRepository.Save();

        _userCommunication.ShowMessage($"Zmieniono dane projektu {projectToEdit.Name}");
    }

    public void RemoveProject()
    {
        var projectToRemove = AskForProjectAndCheckInDb();

        if (projectToRemove.Id == 0)
        {
            _userCommunication.ShowWarning("Nie znaleziono projektu o podanym numerze");
            return;
        }

        projectToRemove.isActive = false;

        _projectRepository.Update(projectToRemove);
        _projectRepository.Save();

        _userCommunication.ShowMessage($"Usunięto projekt {projectToRemove.Name}");
    }

    public void ReadAllProjects()
    {
        var projects = _projectRepository.GetAllActive().ToList();

        _userCommunication.ShowProjects(projects);

        _userCommunication.ShowMessage($"Ilość projektów w bazie: {projects.Count}");
    }

    public void InsertProjects()
    {
        if (_projectRepository.GetAll().ToList().Count == 0)
        {
            _userCommunication.ShowWarning("Baza jest pusta. Wczytuję dane z pliku CSV.");
        }
        else
        {
            var deleteAll = _userCommunication.GetInput("Czy usunąć wszystkie projekty? (Y/n)");
            if (deleteAll == "Y")
            {
                _projectRepository.RemoveRange(_projectRepository.GetAll());
                _projectRepository.Save();
            }
        }

        var projects = _csvReader.ProcessProjects("DataAccess\\Resources\\Files\\projects.csv");

        foreach (var project in projects)
        {
            _projectRepository.Add(new Project
            {
                Name = project.Name,
                OrderId = project.OrderId,
                CustomerId = project.CustomerId,
                isActive = true,
            });
        }
        _projectRepository.Save();

        _userCommunication.ShowMessage($"Ilość dodanych projektów do bazy: {projects.Count}");
        _userCommunication.ShowMessage($"Ilość projektów w bazy: {_projectRepository.GetAll().ToList().Count}");
    }

    Project AskForProjectAndCheckInDb()
    {
        var projectName = _userCommunication.GetInputAndConvertToLower("Podaj nazwę projektu (lub jej część):");

        if (projectName == null)
        {
            return new();
        }

        var project = LookForProjectByName(projectName);

        if (project == null || project.Id == 0)
        {
            return new();
        }

        return project;
    }

    public Project LookForProjectByName(string name)
    {
        var projects = _projectRepository.GetAllActive()
            .Where(x => x.Name.ToLower().Contains(name.ToLower()))
            .OrderBy(x => x.Name)
            .ToList();

        if (projects == null || projects.Count() == 0)
        {
            _userCommunication.ShowWarning($"Nie znaleziono projektu {name}");
            return new();
        }

        if (projects.Count() > 10)
        {
            _userCommunication.ShowWarning("Znaleziono więcej niż 10 projektów o podanej nazwie. Podaj więcej szczegółów.");
            return new();
        }

        if (projects.Count() > 1)
        {
            _userCommunication.ShowWarning($"Znaleziono kilka projektów o podanej nazwie:");
            foreach (var project in projects)
            {
                _userCommunication.ShowMessage($"{project.Id} - {project.Name}");
            }

            var projectId = _userCommunication.GetInput("Podaj ID projektu:");

            if (projectId == null || !int.TryParse(projectId, out int id))
            {
                return new();
            }

            return projects.Where(x => x.Id == id).ToList().FirstOrDefault()!;
        }
        else
        {
            var project = projects.First();
            _userCommunication.ShowMessage($"Znaleziono w bazie projekt {project.Name}");

            return project;
        }
    }

    public Project GetProjectByName(string name)
    {
        return _projectRepository.GetAllActive()
            .Where(x => x.Name == name).FirstOrDefault()!;
    }

    public Project? GetProjectById(int id)
    {
        return _projectRepository.GetById(id);
    }
}
