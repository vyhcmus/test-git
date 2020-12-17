using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectLab04.Models
{
    public class EmployeesModel
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int Age { get; set; }

        public string Type { get; set; }
    }
}