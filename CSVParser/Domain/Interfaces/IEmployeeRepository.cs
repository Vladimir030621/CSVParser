using CSVParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSVParser.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetEmployees();

        void SaveEmployee(Employee employee);

        void UpdateEmployee(Employee employee);
    }
}
