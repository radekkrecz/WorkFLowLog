using System.Text;
using System.Xml.Linq;
using WorkFlowLog.Components.CsvReader;
using WorkFlowLog.Components.DataProviders.Interfaces;
using WorkFlowLog.Components.ReportCreator;
using WorkFlowLog.Data.Entities;

namespace WorkFlowLog;

public class App : IApp
{
    private readonly IReportCreator _reportCreator;

    public App(IReportCreator reportCreator)
    {
        _reportCreator = reportCreator;
    }


    public void Run()
    {
        _reportCreator.CreateXmlReportCarsCombined("report_cars_combined.xml");
    }
}
