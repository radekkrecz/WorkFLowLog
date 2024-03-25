using System.Xml.Linq;
using WorkFlowLog.Components.CsvReader;

namespace WorkFlowLog.Components.ReportCreator;

public class ReportCreator : IReportCreator
{
    private readonly ICsvReader _csvReader;

    public ReportCreator(ICsvReader csvReader)
    {
        _csvReader = csvReader;
    }

    public void CreateXmlReportCarsCombined(string outputFile)
    {
        var cars = _csvReader.ProcessCars("Resources\\Files\\fuel.csv");
        var manuf = _csvReader.ProcessManufacturers("Resources\\Files\\manufacturers.csv");

        var groups = manuf.GroupJoin(
                       cars,
                       m => m.Name,
                       c => c.Manufacturer,
                       (m, g) => new
                       {
                           Manufacturer = new
                           {
                               m.Name,
                               m.Country
                           },
                           Cars = new
                           {
                               country = m.Country,
                               CombinedSum = g.Sum(c => c.Combined),
                               Car = g.Select(c => new
                               {
                                   c.Name,
                                   c.Combined
                               })
                           }
                       })
            .OrderBy(m => m.Manufacturer.Name)
            .ThenByDescending(m => m.Cars.CombinedSum);

        var document = new XDocument();
        var reportElement = new XElement("Manufacturers", groups
            .Select(g =>
            new XElement("Manufacturer",
            new XAttribute("Name", g.Manufacturer.Name),
            new XAttribute("Country", g.Manufacturer.Country),
            new XElement("Cars",
                new XAttribute("country", g.Cars.country),
                new XAttribute("CombinedSum", g.Cars.CombinedSum),
                g.Cars.Car.Select(c =>
                    new XElement("Car",
                    new XAttribute("Name", c.Name),
                    new XAttribute("Combined", c.Combined)
                ))))));

        document.Add(reportElement);
        document.Save(outputFile);
    }
}
