using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Payroll
{
    public class ClsCalculateLeave
    {
        protected IUnitOfWork unitOfWork;
        public bool IsML { get; set; }
        public DateTime DateFromML { get; set; }
        public DateTime DateToML { get; set; }
        public int TotalLeave { get; set; }
        public decimal RateDed { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsCalculateLeave()
        {
            OnLoad();
        }
        public int Calculate(List<HREmpLeaveD> ListLeaveDed, List<HRLeaveType> ListLeaveType, string EmpCode, DateTime FromDate, DateTime ToDate
            , string LeaveType = "ML")
        {
            var objLeaveType = ListLeaveType.FirstOrDefault(w => w.Code == LeaveType && w.CUT == true);
            if (objLeaveType == null) return 0;
            var EmpLeave = (from LeaveD in ListLeaveDed
                            where LeaveD.EmpCode == EmpCode
                            && LeaveD.LeaveCode == LeaveType
                            && LeaveD.Status == "Leave"
                            && LeaveD.LeaveDate >= FromDate
                            && LeaveD.LeaveDate <= ToDate
                            select LeaveD
                           ).ToList();
            if (EmpLeave.Count() > 0)
            {
                DateFromML = EmpLeave.Min(w => w.LeaveDate);
                DateToML = EmpLeave.Max(w => w.LeaveDate);
                IsML = true;
                RateDed = objLeaveType.Soperand.Value;
                int totalDays = (ToDate - FromDate).Days + 1;
            }
            return EmpLeave.Count();
        }

        public List<HREmpLeaveD> OnLoadData(List<HREmpLeaveD> EmpLeaveDede, string EmpCode, DateTime FromDate, DateTime ToDate)
        {
            List<HREmpLeaveD> EmpLeave = new List<HREmpLeaveD>();
            string Approve = SYDocumentStatus.APPROVED.ToString();
            EmpLeave = (from LeaveD in EmpLeaveDede
                        where LeaveD.EmpCode == EmpCode
                        && LeaveD.Status == "Leave"
                        && LeaveD.LeaveDate >= FromDate
                        && LeaveD.LeaveDate <= ToDate
                        && LeaveD.LeaveCode != "ML"
                        select LeaveD
                          ).ToList();
            return EmpLeave;
        }

        public IEnumerable<HREmpLeaveD> OnLoadDataLeave(List<string> ListEmpCode, DateTime FromDate, DateTime ToDate)
        {
            string Approve = SYDocumentStatus.APPROVED.ToString();
            var EmpLeave = (from Leave in unitOfWork.Set<HREmpLeave>()
                            join LeaveD in unitOfWork.Set<HREmpLeaveD>()
                               on new { Increment = (int)Leave.Increment, EmpCode = Leave.EmpCode }
                                equals new { Increment = (int)LeaveD.LeaveTranNo, EmpCode = LeaveD.EmpCode }
                            join leaveType in unitOfWork.Set<HRLeaveType>()
                                   on LeaveD.LeaveCode equals leaveType.Code
                            where ListEmpCode.Contains(LeaveD.EmpCode)
                            && Leave.Status == Approve
                            && LeaveD.Status == "Leave"
                            && LeaveD.LeaveDate >= FromDate
                            && LeaveD.LeaveDate <= ToDate
                            && leaveType.CUT == true
                            select LeaveD
                          ).ToList();
            return EmpLeave;
        }
        public decimal GetUnitLeaveDeductionAmoun(HREmpLeaveD LeaveD, ClsSalary _filter, HRLeaveType leaveType, decimal? salary, decimal numDayInMonth, decimal workingHour)
        {
            if (!salary.HasValue)
            {
                return 0m;
            }
            decimal Soperand = leaveType.Soperand.Value;
            var LeavePo_ = unitOfWork.Repository<HRLeaveDedPolicy>().Queryable().Where(w => w.LeaveCode == leaveType.Code
                            && w.WorkingType == "TOKEN").ToList();
            if (LeavePo_.Any())
            {
                DateTime Lfromdate = new DateTime(_filter.FromDate.Year, 1, 1);
                DateTime Ltodate = LeaveD.LeaveDate;
                var Lpolicy = unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
                if (Lpolicy != null) Lfromdate = Lpolicy.LFromDate;

                decimal NoDay = OnLoadDataInyear(_filter.EmpCode, Lfromdate, Ltodate, leaveType.Code);
                NoDay = NoDay / _filter.Parameter.WHOUR ?? 0;
                var obj = LeavePo_.FirstOrDefault(w => w.FromDay > NoDay && w.ToDay <= NoDay);
                if (obj != null) Soperand = obj.RateDeduct;
            }
            decimal result = 0;
            if (leaveType.Foperand == "B")
            {
                if (leaveType.Operator == "/")
                {
                    result = Convert.ToDecimal(salary / Soperand);
                }
                else if (leaveType.Operator == "-")
                {
                    result = Convert.ToDecimal(salary - Soperand);
                }
                else if (leaveType.Operator == "+")
                {
                    result = Convert.ToDecimal(salary + Soperand);
                }
                else if (leaveType.Operator == "*")
                {
                    result = Convert.ToDecimal(salary * Soperand);
                }
            }
            else if (leaveType.Foperand == "B/W")
            {
                if (leaveType.Operator == "/")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth / Soperand);
                }
                else if (leaveType.Operator == "-")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth - Soperand);
                }
                else if (leaveType.Operator == "+")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth + Soperand);
                }
                else if (leaveType.Operator == "*")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth * Soperand);
                }
            }
            else if (leaveType.Foperand == "B/D*H")
            {
                if (leaveType.Operator == "/")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth * workingHour) / Soperand);
                }
                else if (leaveType.Operator == "-")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth - workingHour) / Soperand);
                }
                else if (leaveType.Operator == "+")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth + workingHour) / Soperand);
                }
                else if (leaveType.Operator == "*")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth + workingHour) / Soperand);
                }
            }

            return result;
        }
        public decimal GetUnitLeaveDeductionAmounFP(HREmpLeaveD LeaveD, ClsFPSalary _filter, HRLeaveType leaveType, decimal? salary, decimal numDayInMonth, decimal workingHour)
        {
            if (!salary.HasValue)
            {
                return 0m;
            }
            decimal Soperand = leaveType.Soperand.Value;
            var LeavePo_ = unitOfWork.Repository<HRLeaveDedPolicy>().Queryable().Where(w => w.LeaveCode == leaveType.Code
                            && w.WorkingType == "TOKEN").ToList();
            if (LeavePo_.Any())
            {
                DateTime Lfromdate = new DateTime(_filter.FromDate.Year, 1, 1);
                DateTime Ltodate = LeaveD.LeaveDate;
                var Lpolicy = unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
                if (Lpolicy != null) Lfromdate = Lpolicy.LFromDate;

                decimal NoDay = OnLoadDataInyear(_filter.EmpCode, Lfromdate, Ltodate, leaveType.Code);
                NoDay = NoDay / _filter.Parameter.WHOUR ?? 0;
                var obj = LeavePo_.FirstOrDefault(w => w.FromDay > NoDay && w.ToDay <= NoDay);
                if (obj != null) Soperand = obj.RateDeduct;
            }
            decimal result = 0;
            if (leaveType.Foperand == "B")
            {
                if (leaveType.Operator == "/")
                {
                    result = Convert.ToDecimal(salary / Soperand);
                }
                else if (leaveType.Operator == "-")
                {
                    result = Convert.ToDecimal(salary - Soperand);
                }
                else if (leaveType.Operator == "+")
                {
                    result = Convert.ToDecimal(salary + Soperand);
                }
                else if (leaveType.Operator == "*")
                {
                    result = Convert.ToDecimal(salary * Soperand);
                }
            }
            else if (leaveType.Foperand == "B/W")
            {
                if (leaveType.Operator == "/")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth / Soperand);
                }
                else if (leaveType.Operator == "-")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth - Soperand);
                }
                else if (leaveType.Operator == "+")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth + Soperand);
                }
                else if (leaveType.Operator == "*")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth * Soperand);
                }
            }
            else if (leaveType.Foperand == "B/D*H")
            {
                if (leaveType.Operator == "/")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth * workingHour) / Soperand);
                }
                else if (leaveType.Operator == "-")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth - workingHour) / Soperand);
                }
                else if (leaveType.Operator == "+")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth + workingHour) / Soperand);
                }
                else if (leaveType.Operator == "*")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth + workingHour) / Soperand);
                }
            }

            return result;
        }

        public decimal OnLoadDataInyear(string EmpCode, DateTime FromDate, DateTime ToDate,string LeaveCode)
        {
            decimal result = 0;
            string Approve = SYDocumentStatus.APPROVED.ToString();
            var EmpLeave = (from Leave in unitOfWork.Set<HREmpLeave>()
                            join LeaveD in unitOfWork.Set<HREmpLeaveD>()
                               on new { Increment = (int)Leave.Increment, EmpCode = Leave.EmpCode }
                                equals new { Increment = (int)LeaveD.LeaveTranNo, EmpCode = LeaveD.EmpCode }
                            join leaveType in unitOfWork.Set<HRLeaveType>()
                                   on LeaveD.LeaveCode equals leaveType.Code
                            where LeaveD.EmpCode == EmpCode && Leave.LeaveCode == LeaveCode
                            && Leave.Status == Approve
                            //&& LeaveD.Status == "Leave"
                            && LeaveD.LeaveDate >= FromDate
                            && LeaveD.LeaveDate <= ToDate
                            && leaveType.CUT == true
                            select LeaveD
                          ).ToList();
            if (EmpLeave.Any()) result = Convert.ToDecimal(EmpLeave.Sum(w => w.LHour));
            return result;
        }
    }
}
