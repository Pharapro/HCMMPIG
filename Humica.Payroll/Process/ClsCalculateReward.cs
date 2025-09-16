using Humica.Core.DB;
using System;

namespace Humica.Payroll
{
    public class ClsCalculateReward
    {
        public DateTime R_FromDate { get; set; }
        public DateTime R_ToDate { get; set; }
        public bool IsProRate { get; set; }
        public void Validate_Date(ClsSalary _filter, DateTime FromDate, DateTime ToDate)
        {
            if (_filter.FromDate.Date >= FromDate.Date && _filter.ToDate.Date <= ToDate.Date)
            {
                R_FromDate = _filter.FromDate;
                R_ToDate = _filter.ToDate;
                IsProRate = true;
            }
            else if (_filter.FromDate >= FromDate && ToDate <= _filter.ToDate)
            {
                R_FromDate = _filter.FromDate;
                R_ToDate = ToDate;
                IsProRate = true;
            }
            else if (FromDate >= _filter.FromDate && FromDate <= _filter.ToDate && ToDate >= _filter.ToDate)
            {
                R_FromDate = FromDate;
                R_ToDate = _filter.ToDate;
                IsProRate = true;
            }
            else if (FromDate >= _filter.FromDate && ToDate <= _filter.ToDate)
            {
                R_FromDate = FromDate;
                R_ToDate = ToDate;
                IsProRate = true;
            }
        }
    }
}
