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

namespace CSVParser.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository context;
        private readonly IHostingEnvironment hostingEnvironment;

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

            var result = engine.ReadFile(filePath).ToList();

            SaveResults(result);

            return View("ShowResults", result);
        }


        public IActionResult ShowResults()
        {
            return View();
        }

        private void SaveResults(List<EmployeeViewModel> employeeViewModels)
        {
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

                context.SaveEmployee(currentEmployee);
            }
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
