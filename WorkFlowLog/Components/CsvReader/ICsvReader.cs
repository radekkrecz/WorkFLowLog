using WorkFlowLog.Components.CsvReader.Models;

namespace WorkFlowLog.Components.CsvReader;

public interface ICsvReader
{
    List<Car> ProcessCars(string filePath);

    List<Manufacturer> ProcessManufacturers(string filePath);
}
