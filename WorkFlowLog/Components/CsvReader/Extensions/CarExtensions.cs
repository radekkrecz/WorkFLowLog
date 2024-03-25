using System.Globalization;
using WorkFlowLog.Components.CsvReader.Models;

namespace WorkFlowLog.Components.CsvReader.Extensions;

public static class CarExtensions
{
    public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
    {
        foreach (var line in source)
        {
            var columns = line.Split(',');

            yield return new Car
            {
                Year = int.Parse(columns[0]),
                Manufacturer = columns[1],
                Name = columns[2],
                Displacement = double.Parse(columns[3], CultureInfo.InvariantCulture),
                Cylinders = int.Parse(columns[4]),
                City = double.Parse(columns[5]),
                Highway = double.Parse(columns[6]),
                Combined = double.Parse(columns[7])
            };
        }
    }
}
