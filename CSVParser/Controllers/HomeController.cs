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
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IHostingEnvironment environment)
        {
            hostingEnvironment = environment;
        }

        public IActionResult ShowResults()
        {
            return View(new InputFile());
        }

        [HttpPost]
        public IActionResult ShowResults(InputFile model)
        {
            string filePath = "";

            if (model.Uploadedfile != null)
            {
                filePath = SaveUploadedFile(model);
            }

            var engine = new FileHelperEngine<EmployeeViewModel>();

            var result = engine.ReadFile(filePath).ToList();

            return View("Index", result);
        }


        public IActionResult Index()
        {
            return View();
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
