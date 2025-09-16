using Humica.Core.DB;
using System;

namespace Humica.Payroll
{
    public class ClsPRParameter
    {
        public static decimal Get_WorkingDay_Salary(PRParameter parameter, DateTime startDate, DateTime endDate)
        {
            decimal Result = 0;
            if (parameter.SALWKTYPE == 1)
            {
                Result = Get_WorkingDay(parameter, startDate, endDate);
            }
            else if (parameter.SALWKTYPE == 2)
            {
                Result = endDate.Subtract(startDate).Days + 1;
            }
            else if (parameter.SALWKTYPE == 3)
            {
                Result = Convert.ToInt32(parameter.SALWKVAL);
            }
            return Result;
        }
        public static decimal Get_WorkingDay(PRParameter parameter, DateTime startDate, DateTime endDate, DateTime fromDate, DateTime toDate, int count)
        {
            decimal result = 0;
            if (parameter.SALWKTYPE == 1)
            {
                result = Get_WorkingDay(parameter, startDate, endDate);
                if (fromDate.Date != startDate.Date || toDate.Date != endDate.Date)
                {
                    result = Get_WorkingDay(parameter, fromDate, toDate);
                }
            }
            else if (parameter.SALWKTYPE == 2)
            {
                result = endDate.Subtract(startDate).Days + 1;
                if (fromDate.Date != startDate.Date || toDate.Date != endDate.Date)
                {
                    result = Get_WorkingDay(parameter, fromDate, toDate);
                }
            }
            else if (parameter.SALWKTYPE == 3)
            {
                result = Convert.ToInt32(parameter.SALWKVAL);
                decimal workedDay = Convert.ToInt32(parameter.SALWKVAL);
                if (fromDate.Date != startDate.Date || toDate.Date != endDate.Date)
                {
                    if (fromDate > startDate)
                    {
                        if (toDate <= endDate)
                            result = Get_WorkingDay(parameter, fromDate, toDate);
                        else
                        {
                            decimal nonWorkedDay = Get_WorkingDay(parameter, startDate, fromDate.AddDays(-1));
                            result = workedDay - nonWorkedDay;
                        }
                    }
                    else if (toDate < endDate)
                    {
                        if (count > 1)
                        {
                            result = Get_WorkingDay(parameter, fromDate, toDate);
                        }
                        else
                        {
                            result = Get_WorkingDay(parameter, fromDate, toDate);
                        }
                    }
                }
            }
            return result;
        }
        public static decimal Get_WorkingDay(PRParameter PayParam, DateTime FromDate, DateTime ToDate)
        {
            if (ToDate < FromDate)
                return -1;

            decimal? Result = 0;
            DateTime TempDate = FromDate;
            if (PayParam != null)
            {
                while (TempDate <= ToDate)
                {
                    if (PayParam.WDMON == true && TempDate.DayOfWeek == DayOfWeek.Monday) Result += PayParam.WDMONDay;
                    else if (PayParam.WDTUE == true && TempDate.DayOfWeek == DayOfWeek.Tuesday) Result += PayParam.WDTUEDay;
                    else if (PayParam.WDWED == true && TempDate.DayOfWeek == DayOfWeek.Wednesday) Result += PayParam.WDWEDDay;
                    else if (PayParam.WDTHU == true && TempDate.DayOfWeek == DayOfWeek.Thursday) Result += PayParam.WDTHUDay;
                    else if (PayParam.WDFRI == true && TempDate.DayOfWeek == DayOfWeek.Friday) Result += PayParam.WDFRIDay;
                    else if (PayParam.WDSAT == true && TempDate.DayOfWeek == DayOfWeek.Saturday) Result += PayParam.WDSATDay;
                    else if (PayParam.WDSUN == true && TempDate.DayOfWeek == DayOfWeek.Sunday) Result += PayParam.WDSUNDay;
                    TempDate = TempDate.AddDays(1);

                }
            }
            return Convert.ToDecimal(Result);
        }
        // allowance
        public static decimal Get_WorkingDay_Allw(PRParameter PayPram, DateTime FromDate, DateTime ToDate)
        {
            decimal Result = 0;
            if (PayPram.ALWTYPE == 1)
            {
                Result = Get_WorkingDay(PayPram, FromDate, ToDate);
            }

            if (PayPram.ALWTYPE == 2)
            {
                Result = ToDate.Subtract(FromDate).Days + 1;
            }

            if (PayPram.ALWTYPE == 3)
            {
                Result = Convert.ToDecimal(PayPram.ALWVAL);
            }
            return Result;
        }

        public static decimal Get_WorkingDay_Ded(PRParameter PayPram, DateTime FromDate, DateTime ToDate)
        {
            decimal Result = -1;
            if (PayPram.DEDTYPE == 1)
            {
                if (ToDate < FromDate)
                {
                    return -1;
                }
                Result = Get_WorkingDay(PayPram, FromDate, ToDate);
            }
            else if (PayPram.DEDTYPE == 2)
            {
                Result = ToDate.Subtract(FromDate).Days + 1;
            }
            else if (PayPram.DEDTYPE == 3)
            {
                Result = Convert.ToInt32(PayPram.DEDVAL);
            }
            return Result;
        }
    }
}
