using Humica.EF.Repo;
using Humica.Core.DB;
using Humica.Core.FT;
using System;
using System.Collections.Generic;
using Humica.Core;
using System.Linq;

namespace Humica.ESS
{
    public class ClsCalculateLeave
    {
        IUnitOfWork unitOfWork;
        public ClsCalculateLeave()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public decimal CalculateCurrent(ClsPeriodLeave Period, HRLeaveType Leave,ATPolicy Policy
            , HRStaffProfile Employee, List<HRSetEntitleD> ListetEntitleD, int InYear, string LeaveCode)
        {
            decimal Rate = 0;
            decimal _Balance = 0;
            decimal _Entitle = 0;
            Period.SeniorityBalance = 0;
            DateTime From = Policy.LFromDate;
            DateTime To = Policy.LToDate;
            int year = InYear;
            if (From.Year != To.Year) year = InYear - 1;
            DateTime FromDate = new DateTime(year, From.Month, From.Day);
            DateTime EndDate = new DateTime(InYear, To.Month, To.Day);
            if (Employee.LeaveConf.Value.Date > FromDate.Date)
                Period.StartDate = Employee.LeaveConf.Value;
            else
                Period.StartDate = FromDate;
            if (Employee.DateTerminate.Year != 1900)
                Period.EndDate = Employee.DateTerminate.AddDays(-1);
            else
            {
                Period.EndDate = DateTime.Now;
                if (Period.EndDate < Period.StartDate) Period.EndDate = Period.StartDate;
            }
            Period.servicelength = DateTimeHelper.GetMonth(Employee.LeaveConf.Value, Period.EndDate);
            Period.ListTempEntitle = ListetEntitleD.Where(w => w.LeaveCode == LeaveCode && w.CodeH == Employee.TemLeave &&
                   w.FromMonth <= Period.servicelength && w.ToMonth >= Period.servicelength).ToList();
            Period.EmpCode = Employee.EmpCode;
            Period.ServicePeriod = DateTimeHelper.GetMonth(FromDate, EndDate);
            foreach (var temp in Period.ListTempEntitle)
            {
                Period.SeniorityBalance = temp.SeniorityBalance;
                if (temp.IsProRate == true)
                {
                    Rate = (decimal)(temp.Entitle / Period.ServicePeriod);
                }
                else
                {
                    _Balance = (decimal)temp.Entitle;
                    _Entitle = (decimal)temp.Entitle;
                }
            }
                if (Leave.BalanceType== "CB")
            {                      
                                   
            }
            else if (Leave.BalanceType=="QB")
            {

            }
            else if (Leave.BalanceType=="SB")
            {

            }

            return 0;
        }
        public decimal Get_WorkingDay(PRParameter PayParam, DateTime FromDate, DateTime ToDate)
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

    }
    public class ClsPeriodLeave
    {
        public PRParameter payParam { get; set; }
        public List<HRSetEntitleD> ListTempEntitle { get; set; }
        public List<HRLeaveProRate> ListLeaveProRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int servicelength { get; set; }
        public int ServicePeriod { get; set; }
        public decimal SeniorityBalance { get; set; }
        public decimal Balance { get; set; }
        public decimal Entitle { get; set; }
        public string EmpCode { get; set; }
    }
}
