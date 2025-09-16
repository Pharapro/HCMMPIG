using Humica.Core;
using Humica.Core.DB;
using Humica.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Humica.ESS
{
    public class ClsLeaveBalance
    {
        public ClsPeriodLeave GetDate(ClsPeriodLeave periodLeave, HRStaffProfile _staff, DateTime LastDate)
        {
            if (_staff.LeaveConf.Value.Date > periodLeave.FromDate.Date)
            {
                periodLeave.StartDate = _staff.LeaveConf.Value;
            }
            else
            {
                periodLeave.StartDate = periodLeave.FromDate;
            }
            //EndDate
            if (_staff.DateTerminate.Year != 1900)
                periodLeave.EndDate = _staff.DateTerminate.AddDays(-1);
            else
            {
                periodLeave.EndDate = LastDate;
                DateTime DateNow = DateTime.Now;
                if (periodLeave.LeaveType.BalanceType == "SB")
                {
                    DateTime date = periodLeave.ToDate.AddMonths(-6);
                    if (date.Date < LastDate.Date)
                    {
                        periodLeave.EndDate = periodLeave.ToDate;
                    }
                    else
                    {
                        if (date.Date >= DateNow.Date)
                        {
                            periodLeave.EndDate = date;
                        }
                        else
                        {
                            periodLeave.EndDate = periodLeave.ToDate;
                        }
                    }
                }
                if (periodLeave.EndDate < periodLeave.StartDate) periodLeave.EndDate = periodLeave.StartDate;
            }
            if (_staff.CompanyCode == "LEGB")
            {
                periodLeave.servicelength = DateTimeHelper.GetMonth(_staff.LeaveConf.Value, periodLeave.StartDate);
            }
            else
            {
                periodLeave.servicelength = DateTimeHelper.GetMonth(_staff.LeaveConf.Value, periodLeave.EndDate);
            }
            periodLeave.EmpCode = _staff.EmpCode;
            periodLeave.ServicePeriod = DateTimeHelper.GetMonth(periodLeave.FromDate, periodLeave.EndDate);
            periodLeave.StartDate = periodLeave.StartDate.Date;
            periodLeave.EndDate = periodLeave.EndDate.Date;

            return periodLeave;
        }
        public decimal Get_BalanceForward(List<HREmpLeaveD> _LeaveD, DateTime? ForWardExp, decimal? Forward, decimal WorkingHour)
        {
            decimal Re_Forward = 0;
            if (ForWardExp.HasValue)
            {
                if (ForWardExp.Value.Year != 1900)
                {
                    List<HREmpLeaveD> Result = _LeaveD.Where(w => w.LeaveDate.Date <= ForWardExp.Value.Date).ToList();
                    Re_Forward = Convert.ToDecimal(Result.Sum(x => x.LHour) / WorkingHour);
                    if (Re_Forward >= Forward)
                        Re_Forward = Convert.ToDecimal(Forward);
                }
            }
            return Re_Forward;
        }
        public clsForwards Calculate_Forward(decimal DayLeave, decimal Forward, decimal ForwardUse, decimal ForwardUsed)
        {
            clsForwards _Forward = new clsForwards();
            decimal Used = 0;
            if (Forward == ForwardUsed)
            {
                DayLeave = DayLeave - Forward;
                Used = Forward;
            }
            else
            {
                if (DayLeave > Forward)
                {
                    DayLeave = DayLeave - Forward;
                    Used = Forward;
                }
                else if (DayLeave > 0)
                {
                    decimal Use = DayLeave;
                    DayLeave = 0;
                    Used = Use;
                }
            }
            _Forward.ForwardUse = Used;
            _Forward.DayLeave = DayLeave;
            return _Forward;
        }

        public decimal GetCurrentBalance(string LeaveCode, DateTime LastDate,
            ClsPeriodLeave periodLeave)
        {
            decimal _Balance = 0;
            decimal prorate_Amount_END = 0;
            DateTime DateNow = DateTime.Now;
            DateTime StartDate = periodLeave.StartDate;
            if (periodLeave.ListLeaveProRate.Count() > 0)
            {
                var LeaveRate = periodLeave.ListLeaveProRate.Where(w => w.Status == "NEWJOIN" && w.LeaveType == LeaveCode).ToList();
                DateTime EDate_OfMonth_new = periodLeave.StartDate.EndDateOfMonth();

                decimal _actWorkDayNew = Get_WorkingDay(periodLeave.payParam, periodLeave.StartDate, EDate_OfMonth_new);
                HRLeaveProRate _prorateNew = LeaveRate.Where(w => w.ActWorkDayFrom <= _actWorkDayNew && w.ActWorkDayTo
                >= _actWorkDayNew).FirstOrDefault();
                decimal prorate_Amount_new = _prorateNew == null ? 0 : _prorateNew.Rate;
                if (!(periodLeave.StartDate.Year == periodLeave.EndDate.Year && periodLeave.StartDate.Month == periodLeave.EndDate.Month))
                {
                    var LeaveRate_RESIGN = periodLeave.ListLeaveProRate.Where(w => w.Status == "RESIGN" && w.LeaveType == LeaveCode).ToList();
                    DateTime EndDate_OfMonth_END = periodLeave.EndDate.StartDateOfMonth();
                    decimal _actWorkDayEND = Get_WorkingDay(periodLeave.payParam, EndDate_OfMonth_END, periodLeave.EndDate);
                    HRLeaveProRate _prorateEnd = LeaveRate_RESIGN.Where(w => w.ActWorkDayFrom <= _actWorkDayEND
                    && w.ActWorkDayTo >= _actWorkDayEND).FirstOrDefault();
                    prorate_Amount_END = _prorateEnd == null ? 0 : _prorateEnd.Rate;
                }
                int C_Month = DateTimeHelper.GetMonth(periodLeave.StartDate.AddMonths(1), periodLeave.EndDate.AddMonths(-1));
                _Balance = Convert.ToDecimal(periodLeave.Rate * C_Month);
                _Balance += prorate_Amount_new + prorate_Amount_END;
            }
            else
            {
                var _Entitle = periodLeave.ListTempEntitle.FirstOrDefault();
                decimal WorkLen = periodLeave.servicelength;
                if (WorkLen >= _Entitle.ToMonth)
                    _Balance = (decimal)_Entitle.Entitle;
                else
                {
                    int C_Day = DateTimeHelper.GetDay(periodLeave.StartDate, periodLeave.EndDate);
                    decimal EDay = (decimal)_Entitle.Entitle;
                    _Balance = ClsRounding.RoundNormal(C_Day * (EDay / 365), 2);
                    if (_Balance > (decimal)_Entitle.Entitle)
                    {
                        _Balance = (decimal)_Entitle.Entitle;
                    }

                }
            }
            return _Balance;
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
        public ClsPeriodLeave GetCurrent(ClsPeriodLeave periodLeave, string LeaveCode)
        {
            decimal? WHour = periodLeave.payParam.WHOUR;
            periodLeave.ListLeaveCode = GetLeave_SubParent(periodLeave.ListLeaveType, LeaveCode);
            List<HREmpLeaveD> _ListLeaveD = periodLeave.ListLeaveItem;
            decimal DayLeave = Convert.ToDecimal(_ListLeaveD.Sum(w => w.LHour) / WHour);
            ClsLeaveBalance clsLeaveBalance = new ClsLeaveBalance();
            decimal Forward = clsLeaveBalance.Get_BalanceForward(_ListLeaveD, periodLeave.EmpLeaveB.ForWardExp, periodLeave.EmpLeaveB.Forward, WHour.Value);
            //decimal Forward2 = clsLeaveBalance.Get_BalanceForward(_ListLeaveD, periodLeave.EmpLeaveB.ForWardExp2, periodLeave.EmpLeaveB.Forward2, WHour.Value);
            //decimal Forward3 = clsLeaveBalance.Get_BalanceForward(_ListLeaveD, periodLeave.EmpLeaveB.ForWardExp3, periodLeave.EmpLeaveB.Forward3, WHour.Value);
            clsForwards _Forward = new clsForwards();
            decimal ForwardUse = 0;
            //decimal ForwardUse2 = 0;
            //decimal ForwardUse3 = 0;
            if (periodLeave.EmpLeaveB.ForWardExp.HasValue)
            {
                _Forward = Calculate_Forward(DayLeave, Forward, ForwardUse, periodLeave.EmpLeaveB.ForwardUse);
                DayLeave = _Forward.DayLeave;
                ForwardUse = _Forward.ForwardUse;
            }
            //if (periodLeave.EmpLeaveB.ForWardExp2.HasValue)
            //{
            //    _Forward = Calculate_Forward(DayLeave, Forward2, ForwardUse2, periodLeave.EmpLeaveB.ForwardUse2.ToDecimal());
            //    DayLeave = _Forward.DayLeave;
            //    ForwardUse2 = _Forward.ForwardUse;
            //}
            //if (periodLeave.EmpLeaveB.ForWardExp3.HasValue)
            //{
            //    _Forward = Calculate_Forward(DayLeave, Forward2, ForwardUse3, periodLeave.EmpLeaveB.ForwardUse3.ToDecimal());
            //    DayLeave = _Forward.DayLeave;
            //    ForwardUse3 = _Forward.ForwardUse;
            //}
            periodLeave.Token = DayLeave;
            return periodLeave;
        }
        public List<ListLeaveType> GetLeave_SubParent(List<HRLeaveType> leaveTypes, string Code)
        {
            List<ListLeaveType> _lstStr = new List<ListLeaveType>();
            _lstStr.Add(new ListLeaveType() { LeaveCode = Code });
            foreach (HRLeaveType read in leaveTypes.Where(w => w.IsParent == true && w.Parent == Code).ToList())
            {
                if (!_lstStr.Where(x => x.LeaveCode == read.Code).Any())
                    _lstStr.Add(new ListLeaveType() { LeaveCode = read.Code });
            }
            return _lstStr;
        }

    }
    public class ClsPeriodLeave
    {
        public string EmpCode { get; set; }
        public PRParameter payParam { get; set; }
        public HREmpLeaveB EmpLeaveB { get; set; }
        public HREmpLeave EmpLeave { get; set; }
        public HRLeaveType LeaveType { get; set; }
        public List<HRLeaveType> ListLeaveType { get; set; }
        public List<ListLeaveType> ListLeaveCode { get; set; }
        public List<HRSetEntitleD> ListTempEntitle { get; set; }
        public List<HRLeaveProRate> ListLeaveProRate { get; set; }
        public List<HREmpLeaveD> ListLeaveItem { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal Rate { get; set; }
        public int servicelength { get; set; }
        public int ServicePeriod { get; set; }
        public decimal SeniorityBalance { get; set; }
        public decimal Balance { get; set; }
        public decimal Entitle { get; set; }
        public decimal Token { get; set; }
    }
    public class ListLeaveType
    {
        public string LeaveCode { get; set; }
    }
    public class clsForwards
    {
        public decimal DayLeave { get; set; }
        public decimal Forward { get; set; }
        public decimal ForwardUse { get; set; }
        public decimal ForwardUsed { get; set; }
    }
}
