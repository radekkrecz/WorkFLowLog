using System.Globalization;
using WorkFlowLog.ApplicationServices.Components.CsvFile.Models;

namespace WorkFlowLog.ApplicationServices.Components.CsvFile;

public class CsvReader : ICsvReader
{
    public List<Employee> ProcessEmployees(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<Employee>();
        }

        var all = File.ReadAllLines(filePath);

        var employees = File.ReadAllLines(filePath)
            .Skip(1)
            .Where(line => line.Length > 1)
            .Select(line =>
            {
                var columns = line.Split(';');
                return new Employee
                {
                    FirstName = columns[0],
                    LastName = columns[1],
                    FullTimeEmployee = ConvertStringToDouble(columns[2])
                };
            });

        return employees.ToList();
    }

    public List<Project> ProcessProjects(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<Project>();
        }

        var projects = File.ReadAllLines(filePath)
            .Skip(1)
            .Where(line => line.Length > 1)
            .Select(line =>
            {
                var columns = line.Split(';');
                return new Project
                {
                    Name = columns[0],
                    OrderId = ConvertStringToInt(columns[1]),
                    CustomerId = ConvertStringToInt(columns[2])
                };
            });

        return projects.ToList();
    }

    public List<Operation> ProcessOperations(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<Operation>();
        }

        var operations = File.ReadAllLines(filePath)
            .Skip(1)
            .Where(line => line.Length > 1)
            .Select(line =>
            {
                var columns = line.Split(';');
                return new Operation
                {
                    Name = columns[0].Trim(),
                    StartDate = ConvertStringToDateTime(columns[1]),
                    EndDate = ConvertStringToDateTime(columns[2]),
                    EmployeeFirstName = ConvertStringToFirstAndLastName(columns[3])[0],
                    EmployeeLastName = ConvertStringToFirstAndLastName(columns[3])[1],
                    ProjectName = columns[4].Trim()
                };
            });

        return operations.ToList();
    }

    int ConvertStringToInt(string input)
    {
        if (int.TryParse(input, out int result))
        {
            return result;
        }
        return 0;
    }

    string[] ConvertStringToFirstAndLastName(string input)
    {
        var nameCount = input.Split(' ').Length;
        string firstName = input.Split(' ')[0];
        string lastName = input.Split(' ')[1];
        if (nameCount > 2)
        {
            lastName = input
                .Split(' ')
                .Skip(1)
                .Aggregate((current, next) => current + " " + next);

        }
        return [firstName, lastName];
    }

    double ConvertStringToDouble(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return 0;
        }

        CultureInfo cultureInfo;

        // Określenie, czy używany jest format europejski czy amerykański
        if (input.Contains(",") && (input.IndexOf(',') < input.IndexOf('.') || !input.Contains(".")))
        {
            // Format europejski, np. 1.234,56
            cultureInfo = (CultureInfo)CultureInfo.GetCultureInfo("pl-PL").Clone();
        }
        else
        {
            // Format amerykański, np. 1,234.56
            cultureInfo = (CultureInfo)CultureInfo.GetCultureInfo("en-US").Clone();
        }

        cultureInfo.NumberFormat.NumberGroupSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator == "." ? "," : ".";

        if (double.TryParse(input, NumberStyles.Any, cultureInfo, out double result))
        {
            return result;
        }
        return 0;
    }


    private DateTime ConvertStringToDateTime(string v)
    {
        if (DateTime.TryParse(v, out DateTime result))
        {
            return result;
        }
        return DateTime.MinValue;
    }
}
