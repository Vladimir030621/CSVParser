using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSVParser.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string PayrollNumber { get; set; }

        [Required(ErrorMessage = "Enter a new name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter a new surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Enter a new day of birth")]
        [Column(TypeName = "datetime2")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Enter a new telephone number")]
        [DataType(DataType.PhoneNumber)]
        public int Telephone { get; set; }

        [Required(ErrorMessage = "Enter a new mobile number")]
        [DataType(DataType.PhoneNumber)]
        public int Mobile { get; set; }

        [Required(ErrorMessage = "Enter a new address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Enter a new address2")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "Enter a new postcode")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Enter a new email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter a new start date")]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "datetime2")]
        public DateTime StartDate { get; set; }
    }
}
