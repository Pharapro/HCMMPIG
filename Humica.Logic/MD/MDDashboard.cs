using Humica.Core.DB;
using Humica.EF.MD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Logic.MD
{
    public class MDDashboard
    {
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public MDDashboard()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public static List<HR_ViewChartNew> MOVEMENTSTAFF(List<HRBranch> listBranch)
        {
            var DB = new HumicaDBContext();
            DateTime DNow = DateTime.Now;
            List<HR_ViewChartNew> LStMovementStaff = new List<HR_ViewChartNew>();
            int y1 = DNow.Year;

            var lst = HR_ViewChartNew.LoadMonth().ToList();
            var lstStaff = DB.HRStaffProfiles.ToList();

            lstStaff = lstStaff.Where(w => listBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            var Staff = lstStaff.Where(w => w.StartDate.Value.Year >= DNow.Year - 1).ToList();
            var d = (from emp in Staff
                     group emp by new { emp.StartDate.Value.Year, emp.StartDate.Value.Month }
                        into myGroup
                     where myGroup.Count() > 0
                     select new
                     {
                         Year = myGroup.Key.Year,
                         Month = myGroup.Key.Month,
                         TotalEmp = myGroup.Count()
                     }).ToList();

            foreach (var item in lst)
            {
                DateTime FromDate = new DateTime(y1, item.Month, 1);
                DateTime ToDate = new DateTime(y1, item.Month, DateTime.DaysInMonth(y1, item.Month));
                var TotalEmp = lstStaff.Where(w => w.DateTerminate.Date >= FromDate.Date && w.DateTerminate.Date <= ToDate.Date).ToList().Count;
                var res1 = new HR_ViewChartNew();
                res1.Year = y1;
                res1.Month = item.Month;
                res1.MonthDesc = item.MonthDesc;
                res1.TotalEmp = TotalEmp;
                res1.MovementStaff = "Resign";
                LStMovementStaff.Add(res1);
                var res = new HR_ViewChartNew();
                res.Year = y1;
                res.Month = item.Month;
                res.MonthDesc = item.MonthDesc;
                res.TotalEmp = 0;
                res.MovementStaff = "New Join";
                // *******New Join*********
                var Result = d.Where(w => w.Month == item.Month && w.Year == y1).ToList();
                foreach (var item1 in Result)
                {
                    res.TotalEmp += item1.TotalEmp;
                }
                LStMovementStaff.Add(res);
            }

            return LStMovementStaff;
        }
        public static List<HR_ViewChartNew> NewStaff(List<HRBranch> listBranch)
        {
            var DB = new HumicaDBContext();
            List<HR_ViewChartNew> LstNewStaff = new List<HR_ViewChartNew>();
            DateTime date = new DateTime(1900, 1, 1);
            DateTime InMonth = DateTime.Now;
            DateTime DNow = DateTime.Now;
            var lst = HR_ViewChartNew.LoadMonth().ToList();
            var lstStaff = DB.HRStaffProfiles.ToList();

            lstStaff = lstStaff.Where(w => listBranch.Where(x => x.Code == w.Branch).Any()).ToList();

            for (int y = DNow.Year - 1; y <= DNow.Year; y++)
            {
                foreach (var item in lst)
                {
                    DateTime FromDate = new DateTime(y, item.Month, 1);
                    DateTime ToDate = new DateTime(y, item.Month, DateTime.DaysInMonth(y, item.Month));
                    DateTime cDate = new DateTime(InMonth.Year, InMonth.Month, 1);
                    if (cDate.Date < FromDate.Date) continue;
                    //**********Total Staff****************
                    var TotalEmp = lstStaff.Where(w => w.StartDate.Value.Date <= ToDate.Date && (w.DateTerminate.Date == date.Date
             || w.DateTerminate.AddDays(-1) >= FromDate.Date)).ToList().Count;

                    var res = new HR_ViewChartNew();
                    res.Year = y;
                    res.Month = item.Month;
                    res.MonthDesc = item.MonthDesc;
                    res.TotalEmp = TotalEmp;
                    LstNewStaff.Add(res);
                }
            }

            return LstNewStaff;
        }

        public static List<ClsPendingLeave> PandingLeave(string URL)
        {
            HumicaDBViewContext b = new HumicaDBViewContext();
            List<ClsPendingLeave> Data = new List<ClsPendingLeave>();
            //  string approved = SYDocumentStatus.APPROVED.ToString();
            // string Reject = SYDocumentStatus.REJECTED.ToString();
            var _list = b.HR_PendingLeave.ToList();
            foreach (var item in _list)
            {
                var Result = new ClsPendingLeave();
                Result.View = URL + item.TranNo.ToString();
                Result.TranNo = item.TranNo;
                Result.EmpCode = item.EmpCode;
                Result.AllName = item.AllName;
                Result.Sex = item.Sex;
                Result.Department = item.Department;
                Result.Position = item.Position;
                Result.RequestDate = item.RequestDate;
                Result.Reason = item.Reason;
                Result.LeaveCode = item.LeaveCode;
                Result.FromDate = item.FromDate;
                Result.ToDate = item.ToDate;
                Result.NoDay = item.NoDay;
                Result.CreatedOn = item.CreatedOn;
                Data.Add(Result);
            }
            //DateTime todays = DateTime.Now;
            //string id = User.UserID;
            //var hrleave = b.HREmpLeaves.Where(x => x.Status != approved && x.Status != Reject).ToList();
            //var hrleaveDay = b.HREmpLeaveDs.ToList();
            //var hrstaff = b.HR_STAFF_VIEW.ToList();
            //var staff = DB.HRStaffProfiles.ToList();
            //if (staff.Where(w => w.EmpCode == User.UserName).ToList().Count > 0)
            //{
            //    var ListApp = DB.ExDocApprovals.ToList();
            //    var ListEmpLEave = DB.HREmpLeaves.ToList();
            //    ListApp = ListApp.Where(w => w.Approver == User.UserName).ToList();
            //    ListEmpLEave = ListEmpLEave.Where(w => ListApp.Where(x => x.DocumentNo == w.Increment.ToString()).Any()).ToList();

            //    //var staff = DB.HRStaffProfiles.ToList();
            //    //  string Division = "";
            //    // long TranNo = 0;
            //    //Division = staff.FirstOrDefault(w => w.EmpCode == User.UserName).Division;
            //    //TranNo = staff.FirstOrDefault(w => w.EmpCode == User.UserName).TRANNO;
            //    //var hEmpcode = staff.Where(w => w.HODCode == TranNo).ToList();
            //    //staff = staff.Where(w => w.Division == Division || hEmpcode.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            //    _list = _list.Where(x => ListEmpLEave.Where(w => w.EmpCode == x.EmpCode).Any() || x.EmpCode == User.UserName).ToList();
            //}
            //var hrleaveD = hrleaveDay.Where(x => hrleave.Where(w => w.TranNo == x.LeaveTranNo).Any()).ToList();
            //var employee = hrstaff.Where(x => hrleaveD.Where(w => w.EmpCode == x.EmpCode).Any()).ToList();
            // return _list.OrderBy(w => w.RequestDate).ToList();
            return Data;
        }
        public static List<HR_AlertNotifications> getAlertNotifications()
        {
            HumicaDBContext db1 = new HumicaDBContext();
            HumicaDBViewContext dbV = new HumicaDBViewContext();
            List<HR_AlertNotifications> data = new List<HR_AlertNotifications>();
            var LstStaff = dbV.HR_STAFF_VIEW.ToList();
            var _Alert = db1.SYSHRAlerts.FirstOrDefault();
            var _LstStaff = LstStaff.Where(w => w.DateTerminate == null).ToList();
            DateTime DateNow = DateTime.Now;
            DateTime todays = DateTime.Now;
            //Probation
            if (_Alert.PBCHK == true)
            {
                todays = DateTime.Now;
                todays = todays.AddDays(_Alert.PBValue.Value);
                var _lstAlert = _LstStaff.Where(x => x.Probation.Value >= DateNow.Date && x.Probation.Value <= todays.Date).ToList();
                foreach (var item in _lstAlert)
                {
                    var _AlertN = new HR_AlertNotifications();
                    _AlertN.EmpCode = item.EmpCode;
                    _AlertN.EmpName = item.AllName;
                    _AlertN.Gender = item.Sex;
                    _AlertN.Department = item.Department;
                    _AlertN.Position = item.Position;
                    _AlertN.AlertType = "Probation";
                    _AlertN.StartDate = item.StartDate.Value;
                    _AlertN.EndDate = item.Probation.Value;
                    data.Add(_AlertN);
                }
            }

            //Birth Day
            //if (_Alert.BDCHK == true)
            //{
            //    // var District = DB.OBDescs.SqlQuery("SELECT distinct District as Code,DistrictDesc1 as Desc1 ,DistrictDesc2 as Desc2 FROM CFPostalCode WHERE Province=" + ProvinceCode).ToList();
            //    string Str = @"SELECT 'Birthday' AlertType,M.EmpCode,M.AllName as EmpName ,M.Sex as Gender,Department,Position,M.StartDate,
            //                (CAST(CAST(MONTH(M.DOB) AS VARCHAR(2)) + '/' + CAST(DAY(M.DOB) AS VARCHAR(2)) + '/' +
            //               (Case When (YEAR(GETDATE())%4=1 AND MONTH(M.DOB)=2 AND Day(M.DOB)=29) then
            //                  CAST(YEAR(M.DOB) AS VARCHAR(4)) else  CAST(YEAR(GETDATE()) AS VARCHAR(4)) end) AS DATETIME)) EndDate
            //                FROM HR_STAFF_VIEW M WHERE(M.TerminateStatus = '' OR M.TerminateStatus IS null)
            //                AND(M.DOB = M.DOB) AND
            //                 (CAST(CAST(MONTH(M.DOB) AS VARCHAR(2)) + '/' + CAST(DAY(M.DOB) AS VARCHAR(2)) + '/' +
            //                 (Case When (YEAR(GETDATE())%4=1 AND MONTH(M.DOB)=2 AND Day(M.DOB)=29) then
            //                  CAST(YEAR(M.DOB) AS VARCHAR(4)) else  CAST(YEAR(GETDATE()) AS VARCHAR(4)) end) AS DATETIME)) BETWEEN DATEADD(DAY,-1,GETDATE()) AND DATEADD(DAY," + _Alert.BDValue.Value + ",GETDATE()) ";
            //    var _lstAlert = db1.HRNotifications.SqlQuery(Str).ToList();
            //    _lstAlert = _lstAlert.ToList();
            //    foreach (var item in _lstAlert)
            //    {
            //        var _AlertN = new HR_AlertNotifications();
            //        _AlertN.EmpCode = item.EmpCode;
            //        _AlertN.EmpName = item.EmpName;
            //        _AlertN.Gender = item.Gender;
            //        _AlertN.Department = item.Department;
            //        _AlertN.Position = item.Position;
            //        _AlertN.AlertType = item.AlertType;
            //        _AlertN.StartDate = item.StartDate;
            //        _AlertN.EndDate = item.EndDate;
            //        data.Add(_AlertN);
            //    }
            //}
            //Contract Expiry
            if (_Alert.CECHK == true)
            {
                string Str = @"select 'Contract' AlertType,EmpCode,AllName as EmpName ,Sex as Gender,Department,Position,StartDate,
                                ConExpiryDate as EndDate from HR_EmpAlert
                                WHERE (TerminateStatus = '' OR TerminateStatus IS null) AND 
                                ConExpiryDate BETWEEN GETDATE() AND DATEADD(DAY," + _Alert.CEValue + ",GETDATE()) ORDER BY ConExpiryDate ASC";
                var _lstAlert = db1.HRNotifications.SqlQuery(Str).ToList();
                _lstAlert = _lstAlert.ToList();
                foreach (var item in _lstAlert)
                {
                    var _AlertN = new HR_AlertNotifications();
                    _AlertN.EmpCode = item.EmpCode;
                    _AlertN.EmpName = item.EmpName;
                    _AlertN.Gender = item.Gender;
                    _AlertN.Department = item.Department;
                    _AlertN.Position = item.Position;
                    _AlertN.AlertType = item.AlertType;
                    _AlertN.StartDate = item.StartDate;
                    _AlertN.EndDate = item.EndDate;
                    data.Add(_AlertN);
                }
            }
            //Separate
            if (_Alert.TMCHK == true)
            {
                todays = DateTime.Now;
                todays = todays.AddDays(_Alert.TMValue.Value);
                var Staff = LstStaff.Where(w => w.DateTerminate != null).ToList();
                var _lstAlert = Staff.Where(x => x.DateTerminate.Value >= DateNow.Date
                && x.DateTerminate.Value <= todays.Date).ToList();
                foreach (var item in _lstAlert)
                {
                    var _AlertN = new HR_AlertNotifications();
                    _AlertN.EmpCode = item.EmpCode;
                    _AlertN.EmpName = item.AllName;
                    _AlertN.Gender = item.Sex;
                    _AlertN.Department = item.Department;
                    _AlertN.Position = item.Position;
                    _AlertN.AlertType = "Separate";
                    _AlertN.StartDate = item.StartDate.Value;
                    _AlertN.EndDate = item.Probation.Value;
                    data.Add(_AlertN);
                }
            }
            //Passport Expiry
            if (_Alert.PPCHK == true)
            {
                string Str = @"select 'Passport' AlertType,EmpCode,AllName as EmpName ,Sex as Gender,Department,Position,StartDate,
                                PassportExp as EndDate from HR_EmpAlert
                                WHERE (TerminateStatus = '' OR TerminateStatus IS null) AND 
                                PassportExp BETWEEN GETDATE() AND DATEADD(DAY," + _Alert.PPValue + ",GETDATE()) ORDER BY PassportExp ASC";
                var _lstAlert = db1.HRNotifications.SqlQuery(Str).ToList();
                _lstAlert = _lstAlert.ToList();
                foreach (var item in _lstAlert)
                {
                    var _AlertN = new HR_AlertNotifications();
                    _AlertN.EmpCode = item.EmpCode;
                    _AlertN.EmpName = item.EmpName;
                    _AlertN.Gender = item.Gender;
                    _AlertN.Department = item.Department;
                    _AlertN.Position = item.Position;
                    _AlertN.AlertType = item.AlertType;
                    _AlertN.StartDate = item.StartDate;
                    _AlertN.EndDate = item.EndDate;
                    data.Add(_AlertN);
                }
            }
            //Work Book Expiry
            if (_Alert.WBCHK == true)
            {
                string Str = @"select 'Work Permit' AlertType,EmpCode,AllName as EmpName ,Sex as Gender,Department,Position,StartDate,
                                WPExp as EndDate from HR_EmpAlert
                                WHERE (TerminateStatus = '' OR TerminateStatus IS null) AND 
                                WPExp BETWEEN GETDATE() AND DATEADD(DAY," + _Alert.WBValue + ",GETDATE()) ORDER BY WPExp ASC";
                var _lstAlert = db1.HRNotifications.SqlQuery(Str).ToList();
                _lstAlert = _lstAlert.ToList();
                foreach (var item in _lstAlert)
                {
                    var _AlertN = new HR_AlertNotifications();
                    _AlertN.EmpCode = item.EmpCode;
                    _AlertN.EmpName = item.EmpName;
                    _AlertN.Gender = item.Gender;
                    _AlertN.Department = item.Department;
                    _AlertN.Position = item.Position;
                    _AlertN.AlertType = item.AlertType;
                    _AlertN.StartDate = item.StartDate;
                    _AlertN.EndDate = item.EndDate;
                    data.Add(_AlertN);
                }
            }
            //VISA Expiry
            if (_Alert.VISACHK == true)
            {
                string Str = @"select 'VISA' AlertType,EmpCode,AllName as EmpName ,Sex as Gender,Department,Position,StartDate,
                                VISAExp as EndDate from HR_EmpAlert
                                WHERE (TerminateStatus = '' OR TerminateStatus IS null) AND 
                                VISAExp BETWEEN GETDATE() AND DATEADD(DAY," + _Alert.VISAValue + ",GETDATE()) ORDER BY VISAExp ASC";
                var _lstAlert = db1.HRNotifications.SqlQuery(Str).ToList();
                _lstAlert = _lstAlert.ToList();
                foreach (var item in _lstAlert)
                {
                    var _AlertN = new HR_AlertNotifications();
                    _AlertN.EmpCode = item.EmpCode;
                    _AlertN.EmpName = item.EmpName;
                    _AlertN.Gender = item.Gender;
                    _AlertN.Department = item.Department;
                    _AlertN.Position = item.Position;
                    _AlertN.AlertType = item.AlertType;
                    _AlertN.StartDate = item.StartDate;
                    _AlertN.EndDate = item.EndDate;
                    data.Add(_AlertN);
                }
            }
            //Open Shirt Date
            if (_Alert.OpenShirtDateCHK == true)
            {
                todays = DateTime.Now;
                todays = todays.AddDays(_Alert.OPENSHIRTDATEValue);
                var _lstAlert = _LstStaff.Where(x => x.OpenDateShirt >= DateNow.Date && x.OpenDateShirt <= todays.Date).ToList();
                foreach (var item in _lstAlert)
                {
                    var _AlertN = new HR_AlertNotifications();
                    _AlertN.EmpCode = item.EmpCode;
                    _AlertN.EmpName = item.AllName;
                    _AlertN.Gender = item.Sex;
                    _AlertN.Department = item.Department;
                    _AlertN.Position = item.Position;
                    _AlertN.AlertType = "Employee Open Date Shirt";
                    _AlertN.StartDate = item.StartDate.Value;
                    _AlertN.EndDate = item.OpenDateShirt.Value;
                    data.Add(_AlertN);
                }
            }
            return data;
        }
    }
    public class HR_ViewChartNew
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public String MonthDesc { get; set; }
        public int TotalEmp { get; set; }
        public string MovementStaff { get; set; }
        public static List<HR_ViewChartNew> LoadMonth()
        {
            List<HR_ViewChartNew> _lst = new List<HR_ViewChartNew>();
            _lst.Add(new HR_ViewChartNew { Month = 1, MonthDesc = "Jan", TotalEmp = 0 });
            _lst.Add(new HR_ViewChartNew { Month = 2, MonthDesc = "Feb", TotalEmp = 0 });
            _lst.Add(new HR_ViewChartNew { Month = 3, MonthDesc = "Mar", TotalEmp = 0 });
            _lst.Add(new HR_ViewChartNew { Month = 4, MonthDesc = "Apr", TotalEmp = 0 });
            _lst.Add(new HR_ViewChartNew { Month = 5, MonthDesc = "May", TotalEmp = 0 });
            _lst.Add(new HR_ViewChartNew { Month = 6, MonthDesc = "Jun", TotalEmp = 0 });
            _lst.Add(new HR_ViewChartNew { Month = 7, MonthDesc = "Jul", TotalEmp = 0 });
            _lst.Add(new HR_ViewChartNew { Month = 8, MonthDesc = "Aug", TotalEmp = 0 });
            _lst.Add(new HR_ViewChartNew { Month = 9, MonthDesc = "Sep", TotalEmp = 0 });
            _lst.Add(new HR_ViewChartNew { Month = 10, MonthDesc = "Oct", TotalEmp = 0 });
            _lst.Add(new HR_ViewChartNew { Month = 11, MonthDesc = "Nov", TotalEmp = 0 });
            _lst.Add(new HR_ViewChartNew { Month = 12, MonthDesc = "Dec", TotalEmp = 0 });
            return _lst;
        }
    }
    public class ClsPendingLeave
    {
        public string View { get; set; }
        public long TranNo { get; set; }
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string Sex { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public string Reason { get; set; }
        public string LeaveCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double? NoDay { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
    public class HR_AlertNotifications
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string AlertType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}