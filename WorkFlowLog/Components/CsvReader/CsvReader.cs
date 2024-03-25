using WorkFlowLog.Components.CsvReader.Extensions;
using WorkFlowLog.Components.CsvReader.Models;

namespace WorkFlowLog.Components.CsvReader;

public class CsvReader : ICsvReader
{
    public List<Car> ProcessCars(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<Car>();
        }

        var cars = File.ReadAllLines(filePath)
            .Skip(1)
            .Where(line => line.Length > 1)
            .ToCar();

        return cars.ToList();
    }

    public List<Manufacturer> ProcessManufacturers(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<Manufacturer>();
        }

        var manufacturers = File.ReadAllLines(filePath)
            .Where(line => line.Length > 1)
            .Select(line =>
            {
                var columns = line.Split(',');

                return new Manufacturer
                {
                    Name = columns[0],
                    Country = columns[1],
                    Year = int.Parse(columns[2])
                };
            });

        return manufacturers.ToList();
    }
}
