using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticalTest_Crud.Models
{
    public class EmployeeMaster
    {
        public int EmployeeId { get; set; }
        public string EmployeeRollNo { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpMiddleName { get; set; }
        public string EmpLastName { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public int  CountryId { get; set; }
        public string CountryName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}