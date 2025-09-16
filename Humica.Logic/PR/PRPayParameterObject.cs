using Humica.Core;
using Humica.Core.DB;
using System;

namespace Humica.Logic.PR
{
    public class PRPayParameterObject
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
                            //decimal nonWorkedDay = Get_WorkingDay(parameter, toDate.AddDays(1), endDate);
                            //result = workedDay - nonWorkedDay;
                            result = Get_WorkingDay(parameter, fromDate, toDate);
                        }
                    }
                }
            }
            return result;
        }
        public decimal GetWorkedDaySalary(PRParameter PayParam, DateTime startDate, DateTime endDate)
        {
            if (PayParam.SALWKTYPE == 1) // working day
            {
                if (endDate < startDate)
                    return -1;

                return Get_WorkingDay(PayParam, startDate, endDate);
            }
            else if (PayParam.SALWKTYPE == 2) // system calendar
            {
                if (endDate < startDate)
                    return -1;

                return endDate.Subtract(startDate).Days + 1;
            }
            else if (PayParam.SALWKTYPE == 3) // fix day
                return (decimal)PayParam.SALWKVAL;

            return -1;
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

        public static decimal GetProratedSalaryAmount(PRParameter PayParam, decimal? basicSalary, decimal actualWorkingDayInMonth, DateTime startDate, DateTime endDate)
        {
            if (PayParam == null) return 0;
            decimal workingDayInMonth = Get_WorkingDay_Salary(PayParam, startDate, endDate);
            return (decimal)(basicSalary * actualWorkingDayInMonth) / workingDayInMonth;
        }
        public static void GetDateTime(PRParameter payParameter, DateTime today, ref DateTime fromDate, ref DateTime toDate)
        {
            DateTime tempDate = today;
            fromDate = today.StartDateOfMonth();
            toDate = today.EndDateOfMonth();
            DateTime FD = DateTime.Now;
            DateTime TD = DateTime.Now;
            int s1 = 0;
            int s2 = 0;

            if (payParameter.IsPrevoiuseMonth.HasValue && payParameter.IsPrevoiuseMonth.Value)
            {
                s1 = payParameter.FROMDATE?.Day ?? 1; 
                s2 = payParameter.TODATE?.Day ?? TD.Day;

                tempDate = today.AddMonths(-1);

                int daysInMonthFrom = DateTime.DaysInMonth(tempDate.Year, tempDate.Month);
                s1 = Math.Min(s1, daysInMonthFrom);

                int daysInMonthTo = DateTime.DaysInMonth(today.Year, today.Month);
                s2 = Math.Min(s2, daysInMonthTo);

                fromDate = new DateTime(tempDate.Year, tempDate.Month, s1);
                toDate = new DateTime(today.Year, today.Month, s2);
            }
        }
    }
    public class PRPayParameterOption
    {
        public const int WorkingDay = 1;
        public const int SystemCalendar = 2;
        public const int FixDay = 3;
    }
}
