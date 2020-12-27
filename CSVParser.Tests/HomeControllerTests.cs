using CSVParser.Controllers;
using CSVParser.Domain.Interfaces;
using CSVParser.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace CSVParser.Tests
{
    public class HomeControllerTests
    {
        private IEnumerable<Employee> GetTestEmployees()
        {
            string format = "yyyy.MM.dd HH:mm:ss:ffff";

            List<Employee> employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    PayrollNumber = "AAAA11",
                    Name = "aaa",
                    Surname = "AAA",
                    DateOfBirth = DateTime.ParseExact("2013.07.12 15:32:04:4687", format, null),
                    Telephone = 123456789,
                    Mobile = 234567891,
                    Address = "aaa aaa",
                    Address2 = "AAA AAA",
                    Postcode = "AA11AA",
                    Email = "aaaa@gmail.com",
                    StartDate = DateTime.ParseExact("2013.07.12 15:32:04:4687", format, null)
                },
                new Employee
                {
                    Id = 2,
                    PayrollNumber = "BBBB11",
                    Name = "bbb",
                    Surname = "BBB",
                    DateOfBirth = DateTime.ParseExact("2013.07.12 15:32:04:4687", format, null),
                    Telephone = 345678912,
                    Mobile = 456789123,
                    Address = "bbb bbb",
                    Address2 = "BBB BBB",
                    Postcode = "BB11BB",
                    Email = "bbbb@gmail.com",
                    StartDate = DateTime.ParseExact("2013.07.12 15:32:04:4687", format, null)
                },
                new Employee
                {
                    Id = 3,
                    PayrollNumber = "CCCC11",
                    Name = "ccc",
                    Surname = "CCC",
                    DateOfBirth = DateTime.ParseExact("2013.07.12 15:32:04:4687", format, null),
                    Telephone = 567891234,
                    Mobile = 678912345,
                    Address = "ccc ccc",
                    Address2 = "CCC CCC",
                    Postcode = "CC11CC",
                    Email = "cccca@gmail.com",
                    StartDate = DateTime.ParseExact("2013.07.12 15:32:04:4687", format, null)
                }
            };

            return employees;
        }

        private readonly IHostingEnvironment environment;

        [Fact]
        public void IndexReturnsView()
        {
            var mock = new Mock<IEmployeeRepository>();
            HomeController controller = new HomeController(environment, mock.Object);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<InputFile>(viewResult.Model);
        }

        [Fact]
        public void ShowResultsReturnsView()
        {
            var mock = new Mock<IEmployeeRepository>();
            HomeController controller = new HomeController(environment, mock.Object);

            var result = controller.ShowResults();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void EditReturnsNotFound()
        {
            var mock = new Mock<IEmployeeRepository>();
            HomeController controller = new HomeController(environment, mock.Object);
            int id = 0;

            var result = controller.Edit(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void EditReturnsViewWithEmployee()
        {
            var mock = new Mock<IEmployeeRepository>();
            mock.Setup(repo => repo.GetEmployees()).Returns(GetTestEmployees());
            HomeController controller = new HomeController(environment, mock.Object);
            int id = 1;
            var testEmployee = GetTestEmployees().FirstOrDefault(e => e.Id == id);

            var result = controller.Edit(id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Employee>(viewResult.Model);
            Assert.Equal("AAAA11", model.PayrollNumber);
            Assert.Equal("aaa", model.Name);
            Assert.Equal("AAA", model.Surname);   
        }

        [Fact]
        public void EditReturnsShowResultsViewWithChangedResults()
        {
            var mock = new Mock<IEmployeeRepository>();
            mock.Setup(repo => repo.GetEmployees()).Returns(GetTestEmployees());
            HomeController controller = new HomeController(environment, mock.Object);
            int countOfParsedEmployees = 0;
            Employee currentEmployee = new Employee();
            var employees = GetTestEmployees();
            var testEmployees = employees
                .Skip(employees.Count() - countOfParsedEmployees)
                .Take(countOfParsedEmployees)
                .ToList();

            var result = controller.Edit(currentEmployee);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<List<Employee>>(viewResult.Model);
            Assert.Equal(testEmployees.Count(), model.Count());
        }
    }
}
