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
        private static int count;
        
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
        public IActionResult Index(InputFile model)
        {
            string filePath = "";

            if (model.Uploadedfile != null)
            {
                filePath = SaveUploadedFile(model);
            }

            var engine = new FileHelperEngine<EmployeeViewModel>();

            var result = engine.ReadFile(filePath).OrderBy(r => r.Surname).ToList();

            var employees = SaveResults(result);

            ViewData["count"] = employees.Count();

            count = employees.Count();

            return View("ShowResults", employees);
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

            var result = employees.Skip(employees.Count() - count).Take(count).ToList();
           
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

        private string SaveUploadedFile(InputFile model)
        {
            var uniqueFileName = GetUniqueFileName(model.Uploadedfile.FileName);

            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");

            var filePath = Path.Combine(uploads, uniqueFileName);

            var newfile = new FileStream(filePath, FileMode.CreateNew);

            model.Uploadedfile.CopyTo(newfile);

            newfile.Close();

            return filePath;
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
