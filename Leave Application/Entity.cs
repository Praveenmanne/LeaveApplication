using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leave_Application
{
    class Entity
    {
    }
    public class Employee
    {
        public string EmpId { get; set; }
        public string EmployeeName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeType { get; set; }
        public string Department { get; set; }
        public string Desigination { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Manager { get; set; }
        public string ManagerWithID { get; set; }
        public DateTime DOJ { get; set; }
        public DateTime DOB { get; set; }
    }
}
