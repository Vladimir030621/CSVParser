using CSVParser.Domain.Interfaces;
using CSVParser.Models;
using CSVParser.ViewModels;
using FileHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CSVParser.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository context;
        private readonly IHostingEnvironment hostingEnvironment;
        private static int countOfParsedEmployees;
        
        public HomeController(IHostingEnvironment environment, IEmployeeRepository context)
        {
            hostingEnvironment = environment;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(new InputFile());
        }

        [HttpPost]
        public IActionResult Index(InputFile inputFile)
        {
            string filePath = "";
            List<Employee> result;

            if (inputFile.Uploadedfile != null)
            {
                filePath = SaveUploadedFile(inputFile);

                var engine = new FileHelperEngine<EmployeeViewModel>();

                var parsedEmployees = engine.ReadFile(filePath).OrderBy(r => r.Surname).ToList();

                DeleteUploadedFile(filePath);

                result = SaveResults(parsedEmployees);

                ViewData["countParsedEmployees"] = result.Count();

                countOfParsedEmployees = result.Count();
            }
            else
            {
                return BadRequest();
            }

            return View("ShowResults", result);
        }


        public IActionResult ShowResults()
        {
            return View();
        }


        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                var employee = context.GetEmployees().FirstOrDefault(e => e.Id == id);

                if (employee != null)
                {
                    return View(employee);
                }          
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            context.UpdateEmployee(employee);

            var employees = context.GetEmployees();

            var result = employees
                .Skip(employees.Count() - countOfParsedEmployees)
                .Take(countOfParsedEmployees)
                .ToList();
           
            return View("ShowResults", result);
        }


        private List<Employee> SaveResults(List<EmployeeViewModel> employeeViewModels)
        {
            var result = new List<Employee>();

            foreach(var employee in employeeViewModels)
            {
                Employee currentEmployee = new Employee();
                currentEmployee.PayrollNumber = employee.PayrollNumber;
                currentEmployee.Name = employee.Name;
                currentEmployee.Surname = employee.Surname;
                currentEmployee.DateOfBirth = employee.DateOfBirth;
                currentEmployee.Telephone = employee.Telephone;
                currentEmployee.Mobile = employee.Mobile;
                currentEmployee.Address = employee.Address;
                currentEmployee.Address2 = employee.Address2;
                currentEmployee.Postcode = employee.Postcode;
                currentEmployee.Email = employee.Email;
                currentEmployee.StartDate = employee.StartDate;

                result.Add(currentEmployee);
                context.SaveEmployee(currentEmployee);
            }

            return result;
        }

        private string SaveUploadedFile(InputFile inputFile)
        {
            var uniqueFileName = GetUniqueFileName(inputFile.Uploadedfile.FileName);

            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");

            var filePath = Path.Combine(uploads, uniqueFileName);

            var newfile = new FileStream(filePath, FileMode.CreateNew);

            inputFile.Uploadedfile.CopyTo(newfile);

            newfile.Close();

            return filePath;
        }

        private void DeleteUploadedFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}
