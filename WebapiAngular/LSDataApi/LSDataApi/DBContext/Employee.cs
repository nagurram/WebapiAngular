using System;
using System.Collections.Generic;

namespace LSDataApi.DBContext
{
    public partial class Employee
    {
        public int Empid { get; set; }
        public DateTime? DateofJoining { get; set; }
        public int? Managerid { get; set; }
        public int? BusinessUnit { get; set; }
    }
}
