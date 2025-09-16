using Humica.Core.DB;
using System;

namespace Humica.Payroll
{
    public class ClsOTCalculate
    {
        public decimal CalculateOT(PROTRate OTRate, decimal DailyRate, decimal WorkingHour)
        {
            decimal Amount = 0;
            decimal Rate = 0;
            if (OTRate.Soperator == "+")
                Rate = Convert.ToDecimal(DailyRate + OTRate.Toperand);
            else if (OTRate.Soperator == "-")
                Rate = Convert.ToDecimal(DailyRate - OTRate.Toperand);
            else if (OTRate.Soperator == "/")
                Rate = Convert.ToDecimal(DailyRate / OTRate.Toperand);
            else if (OTRate.Soperator == "*")
                Rate = Convert.ToDecimal(DailyRate * OTRate.Toperand);

            if (OTRate.Measure == "H") Amount = Convert.ToDecimal(Rate / WorkingHour);

            else if (OTRate.Measure == "D") Amount = Convert.ToDecimal(Rate);

            return Amount;
        }
    }
}
