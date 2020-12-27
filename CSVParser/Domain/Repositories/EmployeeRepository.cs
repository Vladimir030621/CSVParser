using CSVParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSVParser.Domain.Interfaces
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDBContext context;

        public EmployeeRepository(EmployeeDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return context.Employees;
        }

        public void SaveEmployee(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
        }

        public void UpdateEmployee(Employee employee)
        {
            context.Employees.Update(employee);
            context.SaveChanges();
        }
    }
}
