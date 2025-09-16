using System;

namespace Humica.Payroll
{
    public class LeaveDeduction
    {
        public string LeaveCode { get; set; }
        public string LeaveDescription { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal DayLeave { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
    }
}
