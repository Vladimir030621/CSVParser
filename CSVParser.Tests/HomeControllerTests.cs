using CSVParser.Controllers;
using CSVParser.Domain.Interfaces;
using CSVParser.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xunit;

namespace CSVParser.Tests
{
    public class HomeControllerTests
    {
        private IEnumerable<Employee> GetEmployees()
        {
            string format = "yyyy.MM.dd HH:mm:ss:ffff";

            List<Employee> employees = new List<Employee>
            {
                new Employee
                {
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
            mock.Setup(repo => repo.GetEmployees()).Returns(GetEmployees());
            HomeController controller = new HomeController(environment, mock.Object);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
        }
    }
}
