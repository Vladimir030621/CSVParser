using FileHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSVParser.Models
{
    [DelimitedRecord(",")]
    [IgnoreFirst]
    public class EmployeeViewModel
    {
        public string PayrollNumber { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        [Column(TypeName = "datetime2")]
        [FieldConverter(ConverterKind.Date, "dd/M/yyyy")]
        public DateTime DateOfBirth { get; set; }

        public int Telephone { get; set; }

        public int Mobile { get; set; }

        public string Address { get; set; }

        public string Address2 { get; set; }

        public string Postcode { get; set; }

        public string Email { get; set; }

        [Column(TypeName = "datetime2")]
        [FieldConverter(ConverterKind.Date, "dd/M/yyyy")]
        public DateTime StartDate { get; set; }
    }
}
