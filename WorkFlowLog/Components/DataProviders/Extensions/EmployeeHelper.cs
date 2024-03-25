using WorkFlowLog.Data.Entities;

namespace WorkFlowLog.Components.DataProviders.Extensions;

public static class EmployeeHelper
{
    public static IEnumerable<Employee> ByHourlyRate(this IEnumerable<Employee> empl, int hourlyRate)
    {
        return empl.Where(e => Math.Ceiling(e.HourlyRate) == hourlyRate);
    }

    public static IEnumerable<Employee> ByFirstName(this IEnumerable<Employee> empl, string firstName)
    {
        return empl.Where(e => e.FirstName.StartsWith(firstName));
    }

    public static IEnumerable<Employee> ByLastName(this IEnumerable<Employee> empl, string lastName)
    {
        return empl.Where(e => e.LastName.StartsWith(lastName));
    }

    public static IEnumerable<Employee> ByJobName(this IEnumerable<Employee> empl, string jobName)
    {
        return empl.Where(e => e.JobName.StartsWith(jobName));
    }
}
