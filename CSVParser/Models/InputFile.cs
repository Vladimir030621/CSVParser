using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSVParser.Models
{
    public class InputFile
    {
        [Required]
        public IFormFile Uploadedfile { set; get; }
    }
}
