using CSVParser.Domain.Interfaces;
using CSVParser.Models;
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
            if (id > 0)
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
            List<Employee> result = new List<Employee>();

            if(employee != null)
            {
                context.UpdateEmployee(employee);

                var employees = context.GetEmployees();

                result = employees
                    .Skip(employees.Count() - countOfParsedEmployees)
                    .Take(countOfParsedEmployees)
                    .ToList();
            }
            else
            {
                return NotFound();
            }
           
            return View("ShowResults", result);
        }

        #region Save results in DB

        /// <summary>
        /// Save parsed results in DB
        /// </summary>
        /// <param name="employeeViewModels"></param>
        /// <returns></returns>
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

        #endregion

        #region Save input csv file
        /// <summary>
        /// Save input csv file
        /// </summary>
        /// <param name="inputFile"></param>
        /// <returns></returns>
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

        #endregion

        #region Delete csv file
        /// <summary>
        /// Delete csv file
        /// </summary>
        /// <param name="filePath"></param>
        private void DeleteUploadedFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }

        #endregion

        #region Create a unique name for input file
        /// <summary>
        /// Create a unique name for input file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        #endregion
    }
}
