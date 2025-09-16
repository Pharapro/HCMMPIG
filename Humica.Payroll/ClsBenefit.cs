using System;

namespace Humica.Payroll
{
    public class ClsBenefit
    {
        public decimal DayInMonth { get; set; }
        public decimal ActWorkDay { get; set; }
        public decimal? Increased { get; set; } = 0;
        public decimal? Salary { get; set; } = 0;
        public decimal? BaseSalary { get; set; }
        public decimal? AdvPay { get; set; }
        public decimal? Loan { get; set; }
        public decimal? Payback { get; set; }
        public decimal? WorkHour { get; set; }
        public int? Child { get; set; }
        public int? Spouse { get; set; }
        public decimal? SpouseAmount { get; set; }
        public decimal? ChildAmount { get; set; }

        public string IDCard { get; set; }
        public DateTime PayFrom { get; set; }
        public DateTime PayTo { get; set; }
    }
}
