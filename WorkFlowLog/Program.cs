using WorkFlowLog.Data;
using WorkFlowLog.Entities;
using WorkFlowLog.Repositories;

var employeeRepository = new SqlRepository<Employee>(new WorkFlowDbContext());
var orderRepository = new SqlRepository<Order>(new WorkFlowDbContext());

AddEmployees(employeeRepository);
AddEngineers(employeeRepository);
AddOrders(orderRepository);

Console.WriteLine("Lista pracowników:");
WriteAllToConsole(employeeRepository);

Console.WriteLine();
Console.WriteLine("Lista zleceń:");
WriteAllToConsole(orderRepository);


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

static void AddEngineers(IWriteRepository<EngineerEmployee> repository)
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
static void AddOrders(IWriteRepository<Order> repository)
{
    repository.Add(new Order
    {
        Name = "Czujnik poziomu",
        Description = "Wykonanie na potrzeby wewnętrzne",
        OrderId = 321,
    });

    repository.Add(new Order
    {
        Name = "Czujnik obecności cieczy",
        Description = "Wykonanie na zamówienie nr 24/04/12",
        OrderId = 322,
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
