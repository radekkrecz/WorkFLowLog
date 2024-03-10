using WorkFlowLog.Data;
using WorkFlowLog.Entities;
using WorkFlowLog.Repositories;
using WorkFlowLog.Entities;
using WorkFlowLog.Repositories;

var employeeRepository = new SqlRepository<Employee>(new WorkFlowDbContext());
AddEmployees(employeeRepository);
AddEngineer(employeeRepository);
WriteAllToConsole(employeeRepository);

static void AddEmployees(IRepository<Employee> repository)
{
    repository.Add(new Employee
    {
        FirstName = "Barbara",
        LastName = "Nowak", 
        PeselNumber = 94717046242, 
        JobName = "Starszay montażysta", 
        HourlyRate = 47.2 
    });

    repository.Add(new Employee
    {
        FirstName = "Grażyna",
        LastName = "Podsiadło",
        PeselNumber = 77408557098,
        JobName = "Montażysta",
        HourlyRate = 35.4
    });

    repository.Add(new Employee
    {
        FirstName = "Robert",
        LastName = "Lewandowski",
        PeselNumber = 87367854248,
        JobName = "Montażysta",
        HourlyRate = 33.8
    });

    repository.Save();
}

static void AddEngineer(IWriteRepository<EngineerEmployee> repository)
{
    repository.Add(new EngineerEmployee
    {
        FirstName = "Jacek",
        LastName = "Baran",
        PeselNumber = 87367854248,
        JobName = "Starszy inżynier elelktronik",
        HourlyRate = 87.9,
        LaboratoryName = "PRE"
    });

    repository.Add(new EngineerEmployee
    {
        FirstName = "Aldona",
        LastName = "Zdradzisz",
        PeselNumber = 90419808357,
        JobName = "Kierownik pracowni chemicznej",
        HourlyRate = 75.4,
        LaboratoryName = "PCH"
    });
    repository.Save();

}

static void WriteAllToConsole(IReadRepository<IEntity> repository)
{
    var items = repository.GetAll();
    foreach (var item in items)
    {
        Console.WriteLine(item);
    }
}
