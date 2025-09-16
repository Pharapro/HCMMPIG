using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Service
{
    public class ClsLeaveRequest
    {
        public HREmpLeave HeaderEmpLeave { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public HRStaffProfile StaffProfile { get; set; }
        public List<HRStaffProfile> ListStaffProfile { get; set; }
        public PRParameter Parameter { get; set; }
        public List<HREmpLeaveB> ListLeaveB { get; set; }
        public List<HREmpLeave> ListLeave { get; set; }
        public List<HREmpLeaveD> ListLeaveD { get; set; }
        public List<HREmpLeaveD> ListEmpReqLeaveD { get; set; }
        public List<HRLeaveType> ListLeaveType { get; set; }
        public List<HRLeaveProRate> ListLeaveProRate { get; set; }
        public SYHRAnnouncement Announcement { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<HRWorkFlowLeave> ListWorkFlowLeave { get; set; }
        public static int MonthDiff(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
        public void getLeaveDay(string EmpCode)
        {
            double NoLeave = 0;
            double PH = 0;
            double Rest = 0;
            double LHour = 0;
            double WHour = 0;
            if (ListEmpReqLeaveD.Count > 0)
            {
                var Pay = Parameter;
                WHour = Convert.ToDouble(Pay.WHOUR);
                foreach (var item in ListEmpReqLeaveD)
                {
                    LHour = WHour;
                    if (HeaderEmpLeave.Units != "Day")
                    {
                        LHour = (double)item.LHour;
                    }
                    if (item.Remark == "Morning" || item.Remark == "Afternoon")
                    {
                        LHour = Convert.ToDouble(WHour / 2);
                    }
                    LHour = LHour / WHour;
                    if (item.Status == "Leave")
                        NoLeave += LHour;
                    else if (item.Status == "PH")
                        PH += LHour;
                    else if (item.Status == "Rest")
                        Rest += LHour;
                }
                HeaderEmpLeave.NoDay = NoLeave;
                HeaderEmpLeave.NoPH = PH;
                HeaderEmpLeave.NoRest = Rest;
            }
        }
        public decimal GetLeaveCurrent(int InYear, HREmpLeave _Leave, List<HREmpLeave> _lstEmpLeave)
        {
            DateTime DateNow = DateTime.Now;
            DateTime StartDate = new DateTime(InYear, 1, 1);
            DateTime FromDate = new DateTime(InYear, 1, 1);
            DateTime ToDate = new DateTime(InYear, DateNow.Month, DateTime.DaysInMonth(InYear, DateNow.Month));
            DateTime E_Date = new DateTime();
            var LeaveType = ListLeaveType.FirstOrDefault(w => w.IsParent == true && w.Code == _Leave.LeaveCode);
            List<ListLeaveType> _lstStr = new List<ListLeaveType>();
            string LeaveCode = _Leave.LeaveCode;
            _lstStr.Add(new ListLeaveType() { LeaveCode = _Leave.LeaveCode });
            if (LeaveType != null && LeaveType.Parent != null)
            {
                LeaveCode = LeaveType.Parent;
                _lstStr.Add(new ListLeaveType() { LeaveCode = LeaveType.Parent });
            }
            string Cancel = SYDocumentStatus.CANCELLED.ToString();
            string Reject = SYDocumentStatus.REJECTED.ToString();

            var EmpLeaveB = ListLeaveB.Where(w => w.LeaveCode == LeaveCode && w.EmpCode == _Leave.EmpCode
            && w.InYear == _Leave.ToDate.Year).ToList();
            var EmpLeave = _lstEmpLeave.Where(w => _lstStr.Where(x => x.LeaveCode == w.LeaveCode).Any() && w.EmpCode == _Leave.EmpCode).ToList();
            var empLeaveD = ListLeaveD.Where(w => w.EmpCode == _Leave.EmpCode).ToList();
            List<HREmpLeave> _ListLeave = EmpLeave.Where(w => w.Status != Cancel && w.Status != Reject).ToList();
            List<HREmpLeaveD> _ListLeaveD = empLeaveD.Where(w => _lstStr.Where(x => x.LeaveCode == w.LeaveCode).Any() && w.LeaveDate.Year == InYear).ToList();
            _ListLeaveD = _ListLeaveD.Where(w => _ListLeave.Where(x => x.Increment == w.LeaveTranNo).Any()).ToList();
            var ListLeave_Rate = ListLeaveProRate.ToList();
            decimal Balance = 0;
            decimal User_AL = 0;
            foreach (var emp in EmpLeaveB)
            {
                var ObjMatch = emp;
                decimal UserF_AL = 0;

                decimal Balance_new = 0;
                DateTime MonthLy = new DateTime();
                var Staff = StaffProfile;

                if (ToDate.Date > emp.ForWardExp.Value.Date)
                    E_Date = ToDate;
                else E_Date = emp.ForWardExp.Value;
                var WHDPay = Parameter;
                var Leave = _ListLeaveD.Where(w => w.EmpCode == emp.EmpCode && w.LeaveDate.Date <= ToDate.Date).ToList();
                User_AL = (decimal)Leave.Sum(w => w.LHour) / (decimal)WHDPay.WHOUR;
                if (emp.ForWardExp.Value.Year != 1900)
                {
                    var LeaveF = _ListLeaveD.Where(w => w.EmpCode == emp.EmpCode && w.LeaveDate.Date <= E_Date.Date.Date).ToList();
                    if (LeaveF.Count > 0)
                        UserF_AL = (decimal)Leave.Sum(w => w.LHour) / (decimal)WHDPay.WHOUR;
                    if (UserF_AL >= emp.Forward)
                        UserF_AL = emp.Forward.Value;
                }
                User_AL -= UserF_AL;
                if (ToDate.Date > emp.ForWardExp.Value.Date)
                    UserF_AL = 0;
                if (ListLeave_Rate.Where(w => w.LeaveType == emp.LeaveCode).Any())
                {
                    if (Staff.LeaveConf.Value.Date > StartDate.Date)
                    {
                        MonthLy = Staff.LeaveConf.Value;
                        var LeaveRate = ListLeave_Rate.Where(w => w.Status == "NEWJOIN" && w.LeaveType == emp.LeaveCode).ToList();
                        Balance_new = LeaveRate.Where(w => w.ActWorkDayFrom <= MonthLy.Day && MonthLy.Day <= w.ActWorkDayTo).Sum(x => x.Rate);
                    }
                    else
                    {
                        MonthLy = StartDate;
                        Balance_new = 1.5M;
                    }
                    DateTime _Todate = new DateTime();
                    if (Staff.DateTerminate.Year != 1900)
                    {
                        _Todate = Staff.DateTerminate;
                    }
                    else
                    {
                        _Todate = ToDate;
                    }
                    Balance = Convert.ToDecimal(1.5 * MonthDiff(MonthLy, _Todate));
                }
                else
                {
                    Balance_new = Convert.ToDecimal(emp.YTD);
                }
                Balance += Balance_new;
                ObjMatch.Current_AL = Balance - User_AL;
            }
            return Balance - User_AL;
        }
        public void SetAutoApproval(string EmpCode, string Branch)
        {
            ListApproval = new List<ExDocApproval>();
            var DBX = new HumicaDBContext();
            var LstStaff = ListStaffProfile.ToList();
            LstStaff = LstStaff.Where(w => w.Status == "A").ToList();
            var ListWorkFlow = ListWorkFlowLeave.ToList();
            var _staffApp = new HRStaffProfile();
            foreach (var item in ListWorkFlow)
            {
                var Staff = LstStaff.FirstOrDefault(w => w.EmpCode == EmpCode);
                if (item.ApproveBy == "1st")
                {
                    var Read = LstStaff.Where(w => w.JobCode == Staff.FirstLine).ToList();
                    if (Read.Count() == 0)
                        Read = LstStaff.Where(w => w.EmpCode == Staff.FirstLine).ToList();
                    if (Read.Count() > 1)
                    {
                        _staffApp = Read.FirstOrDefault(w => w.Branch == Branch);
                        if (_staffApp == null)
                            _staffApp = Read.FirstOrDefault(w => w.DEPT == Staff.DEPT);
                    }
                    else
                        _staffApp = Read.FirstOrDefault();
                }
                else if (item.ApproveBy == "2nd")
                {
                    if (Staff.SecondLine != null)
                    {
                        var Read = LstStaff.Where(w => w.JobCode == Staff.SecondLine).ToList();
                        if (Read.Count() == 0)
                            Read = LstStaff.Where(w => w.EmpCode == Staff.SecondLine).ToList();
                        if (Read.Count() > 1)
                        {
                            _staffApp = Read.FirstOrDefault(w => w.Branch == Branch);
                            if (_staffApp == null)
                                _staffApp = Read.FirstOrDefault(w => w.DEPT == Staff.DEPT);
                        }
                        else
                            _staffApp = Read.FirstOrDefault();
                    }
                }
                else
                {
                    _staffApp = LstStaff.FirstOrDefault(w => w.JobCode == item.ApproveBy && w.Branch == Branch);
                }
                if (_staffApp == null)
                    _staffApp = LstStaff.FirstOrDefault(w => w.JobCode == item.ApproveBy);
                if (_staffApp == null) continue;
                if (ListApproval.Where(w => w.Approver == _staffApp.EmpCode).Count() > 0) continue;
                var objApp = new ExDocApproval();
                objApp.Approver = _staffApp.EmpCode;
                objApp.ApproverName = _staffApp.AllName;
                objApp.DocumentType = "LR";
                objApp.ApproveLevel = item.Step;
                objApp.WFObject = "WF02";
                ListApproval.Add(objApp);
            }
        }
        public string isValidData()
        {
            string Reject = SYDocumentStatus.REJECTED.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();
            var leaveH = ListLeave.Where(w => w.Status != Cancel && w.Status != Reject && w.EmpCode == HeaderStaff.EmpCode).ToList();

            if (leaveH.Where(w => ((w.FromDate.Date >= HeaderEmpLeave.FromDate.Date && w.FromDate.Date <= HeaderEmpLeave.ToDate.Date) ||
              (w.ToDate.Date >= HeaderEmpLeave.FromDate.Date && w.ToDate.Date <= HeaderEmpLeave.ToDate.Date) ||
                    (HeaderEmpLeave.FromDate.Date >= w.FromDate.Date && HeaderEmpLeave.FromDate.Date <= w.ToDate.Date) || (HeaderEmpLeave.ToDate.Date >= w.FromDate.Date && HeaderEmpLeave.ToDate.Date <= w.ToDate.Date))).Any())
            {

                var EmpLeaveD = ListLeaveD.Where(w => w.EmpCode == HeaderStaff.EmpCode).ToList();
                EmpLeaveD = EmpLeaveD.Where(w => w.LeaveDate.Date >= HeaderEmpLeave.FromDate.Date && w.LeaveDate.Date <= HeaderEmpLeave.ToDate.Date).ToList();
                var Result = ListEmpReqLeaveD.Where(w => EmpLeaveD.Where(x => x.LeaveDate.Date == w.LeaveDate.Date).Any()).ToList();
                if (Result.Where(w => w.Remark == "FullDay").Any())
                {
                    return "INV_DATE";
                }
                else if (EmpLeaveD.Where(w => Result.Where(x => x.LeaveDate.Date == w.LeaveDate.Date && x.Remark == w.Remark).Any()).Any())
                {
                    return "INV_DATE";
                }
                else if (EmpLeaveD.Where(w => w.Remark == "FullDay").Any())
                {
                    return "INV_DATE";
                }
            }
            return SYSConstant.OK;
        }
        public string isValidESSRequest()
        {
            var LeaveD = new List<HREmpLeaveD>();
            var _LeaveType = ListLeaveType.FirstOrDefault(w => w.Code == HeaderEmpLeave.LeaveCode);
            if (_LeaveType == null)
            {
                return "INV_DOC";
            }
            if (ListEmpReqLeaveD.Count <= 0)
            {
                return "INV_DOC";
            }
            if (HeaderEmpLeave.Urgent == false)
            {
                if (_LeaveType.BeforeDay > 0)
                {
                    DateTime DateNow = DateTime.Now;
                    DateNow = DateNow.AddDays(Convert.ToDouble(_LeaveType.BeforeDay));
                    if (HeaderEmpLeave.FromDate.Date < DateNow.Date)
                    {
                        return "INVALID_DATE";
                    }
                }
            }
            string LeaveType = _LeaveType.Description;

            if (HeaderEmpLeave.FromDate.Date > HeaderEmpLeave.ToDate.Date)
            {
                return "INVALID_DATE";
            }
            DateTime D_Now = DateTime.Now;
            var B_Date = D_Now.AddDays(-Convert.ToDouble(_LeaveType.Beforebackward));
            if (_LeaveType.Allowbackward == false && HeaderEmpLeave.FromDate.Date < D_Now.Date)
            {
                return "NOT_ALLOW_BACKWARD";
            }
            else if (_LeaveType.Allowbackward == true && HeaderEmpLeave.FromDate.Date < B_Date.Date)
            {
                return "NOT_ALLOW_BACKWARD";
            }
            if (_LeaveType.ReqDocument == true && _LeaveType.NumDay <= HeaderEmpLeave.NoDay)
            {
                if (HeaderEmpLeave.Attachment == null)
                {
                    return "REQUIRED_DOCUMENT";
                }
            }
            var msg = isValidData();
            if (msg != SYConstant.OK)
            {
                return msg;
            }
            if (HeaderEmpLeave.Reason == "" || HeaderEmpLeave.Reason == null)
                return "REASON_EN";
            if (ListEmpReqLeaveD.Count <= 0)
            {
                return "INV_DOC";
            }
            if (_LeaveType != null)
            {
                if (_LeaveType.Probation == true)
                {
                    if (StaffProfile.Probation.Value.Date > HeaderEmpLeave.FromDate.Date)
                    {
                        return "CANNOT_PROBATION";
                    }
                }
            }
            getLeaveDay(HeaderStaff.EmpCode);
            HeaderEmpLeave.EmpCode = HeaderStaff.EmpCode;
            if (_LeaveType.IsCurrent == false)
            {
                var Balance = GetLeaveCurrent(HeaderEmpLeave.ToDate.Year, HeaderEmpLeave, ListLeave);
                if (Balance - Convert.ToDecimal(HeaderEmpLeave.NoDay) < 0)
                {
                    return "INV_BALANCE";
                }
            }

            SetAutoApproval(StaffProfile.EmpCode, StaffProfile.Branch);
            if (ListApproval.Count() <= 0)
            {
                return "NO_LINE_MN";
            }
            string Status = SYDocumentStatus.PENDING.ToString();

            int Increment;
            if (ListLeave.Count == 0) Increment = 0;
            else Increment = ListLeave.Max(w => w.Increment);

            HeaderEmpLeave.Status = Status;
            HeaderEmpLeave.RequestDate = DateTime.Now;
            HeaderEmpLeave.Increment = Increment + 1;

            int Line = 0;
            foreach (var item in ListEmpReqLeaveD)
            {
                Line += 1;
                var result = new HREmpLeaveD();
                result.LeaveTranNo = Increment + 1;
                result.EmpCode = HeaderStaff.EmpCode;
                result.LeaveCode = HeaderEmpLeave.LeaveCode;
                result.LeaveDate = item.LeaveDate;
                result.CutMonth = item.CutMonth;
                result.Status = item.Status;
                result.Remark = item.Remark;
                result.LineItem = Line;
                result.LHour = item.LHour;
                if (HeaderEmpLeave.Units == "Day" && _LeaveType.IsParent == true && _LeaveType.CUT != true && item.Remark == "FullDay")
                {
                    return "HALF_DAY";
                }
                if (HeaderEmpLeave.Units != "Day")
                {
                    result.Remark = "Hours";
                    result.StartTime = item.StartTime;
                    result.EndTime = item.EndTime;
                }
                if (_LeaveType.IsParent == true && _LeaveType.CUT != true && result.Remark == "FullDay")
                {
                    return "HALF_DAY";
                }
                if (item.Remark == "Morning" || item.Remark == "Afternoon")
                {
                    result.LHour = item.LHour / 2;
                }
            }

            //Add approver
            foreach (var read in ListApproval)
            {
                read.ID = 0;
                read.LastChangedDate = DateTime.Now;
                read.DocumentNo = Convert.ToString(Increment + 1);
                read.Status = SYDocumentStatus.OPEN.ToString();
                read.ApprovedBy = "";
                read.ApprovedName = "";
            }
            Announcement = new SYHRAnnouncement();
            if (ListApproval.Count() > 0)
            {
                Announcement.Type = "LeaveRequest";
                Announcement.Subject = StaffProfile.AllName;
                Announcement.Description = @"Leave type of " + _LeaveType.Description +
                        " from " + HeaderEmpLeave.FromDate.ToString("yyyy.MM.dd") + " to " + HeaderEmpLeave.ToDate.ToString("yyyy.MM.dd") + " My Reason: " + HeaderEmpLeave.Reason;
                Announcement.DocumentNo = HeaderEmpLeave.Increment.ToString();
                Announcement.DocumentDate = DateTime.Now;
                Announcement.IsRead = false;
            }


            return SYConstant.OK;
        }
        public string isValidAdmin()
        {
            var LeaveD = new List<HREmpLeaveD>();
            var _LeaveType = ListLeaveType.FirstOrDefault(w => w.Code == HeaderEmpLeave.LeaveCode);
            if (_LeaveType == null)
            {
                return "INV_DOC";
            }
            string Status = SYDocumentStatus.APPROVED.ToString();
            var lstempLeave = ListLeave.ToList();
            HeaderEmpLeave.EmpCode = HeaderStaff.EmpCode;

            if (HeaderEmpLeave.Reason == "" || HeaderEmpLeave.Reason == null)
                return "REASON_EN";
            if (ListEmpReqLeaveD.Count <= 0)
            {
                return "INV_DOC";
            }
            if (HeaderEmpLeave.FromDate.Date > HeaderEmpLeave.ToDate.Date)
            {
                return "INVALID_DATE";
            }
            var msg = isValidData();
            if (msg != SYConstant.OK)
            {
                return msg;
            }
            getLeaveDay(HeaderStaff.EmpCode);

            if (_LeaveType.IsCurrent == false)
            {
                var Balance = GetLeaveCurrent(HeaderEmpLeave.ToDate.Year, HeaderEmpLeave, lstempLeave);
                if (Balance - Convert.ToDecimal(HeaderEmpLeave.NoDay) < 0)
                {
                    return "INV_BALANCE";
                }
            }
            if (_LeaveType != null)
            {
                if (_LeaveType.Probation == true)
                {
                    if (StaffProfile.Probation.Value.Date > HeaderEmpLeave.FromDate.Date)
                    {
                        return "CANNOT_PROBATION";
                    }
                }
            }

            int Increment;
            if (lstempLeave.Count == 0) Increment = 0;
            else Increment = lstempLeave.Max(w => w.Increment);

            HeaderEmpLeave.Status = Status;
            HeaderEmpLeave.Attachment = HeaderEmpLeave.Attachment;
            HeaderEmpLeave.RequestDate = DateTime.Now;
            HeaderEmpLeave.Increment = Increment + 1;

            int Line = 0;
            foreach (var item in ListEmpReqLeaveD)
            {
                Line += 1;
                var result = new HREmpLeaveD();
                result.LeaveTranNo = Increment + 1;
                result.EmpCode = HeaderStaff.EmpCode;
                result.LeaveCode = HeaderEmpLeave.LeaveCode;
                result.LeaveDate = item.LeaveDate;
                result.CutMonth = item.CutMonth;
                result.Status = item.Status;
                result.Remark = item.Remark;
                result.LineItem = Line;
                result.LHour = item.LHour;
                if (HeaderEmpLeave.Units != "Day")
                {
                    result.Remark = "Hours";
                    result.StartTime = item.StartTime;
                    result.EndTime = item.EndTime;
                }
                if (_LeaveType.IsParent == true && _LeaveType.CUT != true && result.Remark == "FullDay")
                {
                    return "HALF_DAY";
                }
                if (item.Remark == "Morning" || item.Remark == "Afternoon")
                {
                    result.LHour = item.LHour / 2;
                }
            }
            return SYConstant.OK;
        }
    }
    public class ListLeaveType
    {
        public string LeaveCode { get; set; }
    }
}
