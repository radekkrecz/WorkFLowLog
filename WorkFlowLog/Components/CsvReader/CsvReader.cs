using System.Globalization;
using WorkFlowLog.Components.CsvReader.Models;

namespace WorkFlowLog.Components.CsvReader;

public class CsvReader : ICsvReader
{
    public List<Employee> ProcessEmployees(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<Employee>();
        }

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

    int ConvertStringToInt(string input)
    {
        if (int.TryParse(input, out int result))
        {
            return result;
        }
        return 0;
    }

    double ConvertStringToDouble(string input)
    {
        if(string.IsNullOrEmpty(input))
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
}
