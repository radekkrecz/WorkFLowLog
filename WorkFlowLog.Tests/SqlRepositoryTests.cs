using WorkFlowLog.Data;
using WorkFlowLog.DataAccess.Data.Entities;
using WorkFlowLog.Repositories;

namespace WorkFlowLog.Tests
{
    public class SqlRepositoryTests
    {
        [Test]
        public void WhenOneEmployeeAdded()
        {
            var employeeRepository = new SqlRepository<Employee>(new WorkFlowDbContext());

            var employee = new Employee
            {
                FirstName = "Jacek",
                LastName = "Doborowy",
                PeselNumber = "12345678901",
                JobName = "Developer",
                HourlyRate = 100
            };
            employeeRepository.Add(employee);
            employeeRepository.Save();

            var result = employeeRepository.GetById(employee.Id);

            Assert.That(result, Is.EqualTo(employee));
        }

        [Test]
        public void WhenOneEmployeeRemoved()
        {
            var employeeRepository = new SqlRepository<Employee>(new WorkFlowDbContext());

            var employee = new Employee
            {
                FirstName = "Jacek",
                LastName = "Doborowy",
                PeselNumber = "12345678901",
                JobName = "Developer",
                HourlyRate = 100
            };
            employeeRepository.Add(employee);
            employeeRepository.Save();

            employeeRepository.Remove(employee);
            employeeRepository.Save();

            var result = employeeRepository.GetById(employee.Id);

            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void WhenOneEmployeeAddedAndRemovedAndAdded()
        {
            var employeeRepository = new SqlRepository<Employee>(new WorkFlowDbContext());

            var employee = new Employee
            {
                FirstName = "Jacek",
                LastName = "Doborowy",
                PeselNumber = "12345678901",
                JobName = "Developer",
                HourlyRate = 100
            };
            employeeRepository.Add(employee);
            employeeRepository.Save();

            employeeRepository.Remove(employee);
            employeeRepository.Save();

            employeeRepository.Add(employee);
            employeeRepository.Save();

            var result = employeeRepository.GetById(employee.Id);

            Assert.That(result, Is.EqualTo(employee));
        }

        [Test]
        public void WhenOneEmployeeAddedAndRemovedAndAddedAndRemoved()
        {
            var employeeRepository = new SqlRepository<Employee>(new WorkFlowDbContext());

            var employee = new Employee
            {
                FirstName = "Jacek",
                LastName = "Doborowy",
                PeselNumber = "12345678901",
                JobName = "Developer",
                HourlyRate = 100
            };

            var employee1 = new Employee
            {
                FirstName = "Barbara",
                LastName = "Obecna",
                PeselNumber = "58946512453",
                JobName = "Tester",
                HourlyRate = 50
            };

            var employee2 = new Employee
            {
                FirstName = "Gra¿yna",
                LastName = "Podsiad³o",
                PeselNumber = "77408557098",
                JobName = "Monta¿ysta",
                HourlyRate = 35.4
            };

            employeeRepository.Add(employee1);
            employeeRepository.Add(employee);
            employeeRepository.Add(employee2);
            employeeRepository.Save();

            employeeRepository.Remove(employee);
            employeeRepository.Save();

            employeeRepository.Add(employee);
            employeeRepository.Save();

            employeeRepository.Remove(employee);
            employeeRepository.Save();

            var result = employeeRepository.GetById(employee1.Id);

            Assert.That(result, Is.EqualTo(employee1));
        }
    }
}