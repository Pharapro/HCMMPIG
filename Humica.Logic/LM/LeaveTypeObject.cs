using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF.MD;
using System;
using System.Collections.Generic;

namespace Humica.Logic.LM
{
    public class LeaveTypeObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public FTFilterData Filter { get; set; }
        public HRLeaveType Header { get; set; }
        public List<HRLeaveType> ListHeader { get; set; }
        public List<HRLeaveProRate> ListLeaveProRate { get; set; }
        public LeaveTypeObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public static decimal GetUnitLeaveDeductionAmoun(HRLeaveType leaveType, decimal? salary, decimal numDayInMonth, decimal workingHour)
        {
            if (!salary.HasValue)
            {
                return 0m;
            }

            decimal result = 0;
            if (leaveType.Foperand == "B")
            {
                if (leaveType.Operator == "/")
                {
                    result = Convert.ToDecimal(salary / leaveType.Soperand);
                }
                else if (leaveType.Operator == "-")
                {
                    result = Convert.ToDecimal(salary - leaveType.Soperand);
                }
                else if (leaveType.Operator == "+")
                {
                    result = Convert.ToDecimal(salary + leaveType.Soperand);
                }
                else if (leaveType.Operator == "*")
                {
                    result = Convert.ToDecimal(salary * leaveType.Soperand);
                }
            }
            else if (leaveType.Foperand == "B/W")
            {
                if (leaveType.Operator == "/")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth / leaveType.Soperand);
                }
                else if (leaveType.Operator == "-")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth - leaveType.Soperand);
                }
                else if (leaveType.Operator == "+")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth + leaveType.Soperand);
                }
                else if (leaveType.Operator == "*")
                {
                    result = Convert.ToDecimal(salary / (decimal?)numDayInMonth * leaveType.Soperand);
                }
            }
            else if (leaveType.Foperand == "B/D*H")
            {
                if (leaveType.Operator == "/")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth * workingHour) / leaveType.Soperand);
                }
                else if (leaveType.Operator == "-")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth - workingHour) / leaveType.Soperand);
                }
                else if (leaveType.Operator == "+")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth + workingHour) / leaveType.Soperand);
                }
                else if (leaveType.Operator == "*")
                {
                    result = Convert.ToDecimal(salary / (decimal?)(numDayInMonth + workingHour) / leaveType.Soperand);
                }
            }

            return result;
        }

    }
}