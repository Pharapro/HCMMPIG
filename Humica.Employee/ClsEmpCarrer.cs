using System;

namespace Humica.Employee
{
    public class ClsEmpCarrer
    {
        public int ID { get; set; }
        public string CompanyCode { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string CareerType { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        //public string OldSalary { get; set; }
        //public string Increase { get; set; }
        //public string NewSalary { get; set; }
        public DateTime EffectDate { get; set; }

        public string Status { get; set; }
    }
}
