using System.Xml.Linq;

namespace WorkFlowLog.Entities
{
    public class Employee : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long PeselNumber { get; set; }
        public string JobName { get; set; }
        public double HourlyRate { get; set; }

        public override string ToString() => $"ID: {Id}, {FirstName} {LastName}";
    }
}
