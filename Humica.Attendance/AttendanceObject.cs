using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using Humica.Logic.Att;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Contexts;

namespace Humica.Attendance
{
    public class AttendanceObject : IAttendanceObject
    {
        protected IUnitOfWork unitOfWork;
        public string ScreenId { get; set; }
        public string EmpID { get; set; }
        public int Progress { get; set; }
        public string MessageError { get; set; }
        public DateTime Period { get; set; }
        public SYUser User { get; set; }
        public FTFilterAttendance Attendance { get; set; }
        public HRStaffProfile StaffProfile { get; set; }
        public ATEmpSchedule Header { get; set; }
        public ATImpRosterHeader RosterHeader { get; set; }
        public List<VIEW_ATEmpSchedule> ListEmpSchdule { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public List<ATEmpSchedule> ListHeader { get; set; }
        public List<HRRequestOT> ListOT { get; set; }
        public List<ClsEmpShift> ListEmpShift { get; set; }
        public List<ListEmpSch> LIstEmplSch { get; set; }
        public List<ListImportDetail> ListImportPending { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }

        public AttendanceObject()
        {
            User = SYSession.getSessionUser();
            OnLoad();
        }
        public void LoadDataEmpShift(int PeriodID)
        {
            var _listBranch = SYConstant.getBranchDataAccess();
            DateTime fromdate, todate = DateTime.Now;
            var payPeriod = unitOfWork.Repository<ATPayperiod>().Queryable()
                           .FirstOrDefault(w => w.PeriodID == PeriodID);
            fromdate = payPeriod?.FromDate ?? DateTime.Now.StartDateOfMonth();
            todate = payPeriod?.ToDate ?? DateTime.Now.EndDateOfMonth();

            var branchCodes = _listBranch.Select(x => x.Code).ToList();

            List<ClsEmpShift> ListEmpTemp = (from Att in unitOfWork.Repository<ATEmpSchedule>().Queryable()
                                             join emp in unitOfWork.Repository<HRStaffProfile>().Queryable()
                                                 on Att.EmpCode equals emp.EmpCode
                                             join Post in unitOfWork.Repository<HRPosition>().Queryable()
                                                 on emp.JobCode equals Post.Code into postGroup
                                             from Post in postGroup.DefaultIfEmpty()
                                             where Att.TranDate >= fromdate
                                                   && Att.TranDate <= todate
                                                   && (branchCodes.Contains(emp.Branch))
                                                   && (string.IsNullOrEmpty(Attendance.Branch) || emp.Branch == Attendance.Branch)
                                                   && (string.IsNullOrEmpty(Attendance.Division) || emp.Division == Attendance.Division)
                                                   && (string.IsNullOrEmpty(Attendance.Department) || emp.DEPT == Attendance.Department)
                                                   && (string.IsNullOrEmpty(Attendance.Locations) || emp.LOCT == Attendance.Locations)
                                             select new ClsEmpShift
                                             {
                                                 EmpCode = Att.EmpCode,
                                                 AllName = emp.AllName,
                                                 Position = Post != null ? Post.Description : string.Empty,
                                                 InDate = Att.TranDate,
                                                 Shift = Att.SHIFT,
                                                 GroupNo = 1,
                                                 TransDateStr = null
                                             }).ToList();

            // Calculate summaries more efficiently
            var summaries = new List<ClsEmpShift>();
            var employeeGroups = ListEmpTemp.GroupBy(x => new { x.EmpCode, x.AllName, x.Position });

            // Use fixed summary dates that are clearly outside the normal date range
            DateTime summaryBaseDate = todate.AddMonths(1); // Move to next month for summaries

            foreach (var empGroup in employeeGroups)
            {
                var empCode = empGroup.Key.EmpCode;
                var allName = empGroup.Key.AllName;
                var position = empGroup.Key.Position;

                // Count days using more efficient approach
                var shiftCounts = empGroup
                    .GroupBy(x => x.Shift)
                    .ToDictionary(g => g.Key, g => g.Count());

                int workingDays = empGroup.Count(x => !string.IsNullOrEmpty(x.Shift) &&
                                                    x.Shift != "OFF" && x.Shift != "PH");
                int offDays = 0;
                if (shiftCounts.TryGetValue("OFF", out var offCount))
                    offDays = offCount;

                int phDays = 0;
                if (shiftCounts.TryGetValue("PH", out var phCount))
                    phDays = phCount;

                // Add summary records with sequential dates for proper ordering
                summaries.Add(new ClsEmpShift
                {
                    EmpCode = empCode,
                    AllName = allName,
                    Position = position,
                    InDate = summaryBaseDate,
                    Shift = workingDays.ToString(),
                    GroupNo = 2, // Use different group number for summaries
                    TransDateStr = "Working Days"
                });

                summaries.Add(new ClsEmpShift
                {
                    EmpCode = empCode,
                    AllName = allName,
                    Position = position,
                    InDate = summaryBaseDate.AddDays(1),
                    Shift = offDays.ToString(),
                    GroupNo = 2,
                    TransDateStr = "OFF Days"
                });

                summaries.Add(new ClsEmpShift
                {
                    EmpCode = empCode,
                    AllName = allName,
                    Position = position,
                    InDate = summaryBaseDate.AddDays(2),
                    Shift = phDays.ToString(),
                    GroupNo = 2,
                    TransDateStr = "PH Days"
                });
            }

            // Combine and order all data
            ListEmpShift = ListEmpTemp
                .Concat(summaries)
                .OrderBy(x => x.EmpCode)
                .ThenBy(x => x.GroupNo)
                .ThenBy(x => x.InDate)
                .ToList();

            ListImportPending = LoadImportDetail() ?? new List<ListImportDetail>();
        }
        public void OnIndexLoading()
        {
            var ListAttendance = unitOfWork.Set<VIEW_ATEmpSchedule>().Where(w => DbFunctions.TruncateTime(w.TranDate) >= Attendance.FromDate.Date &&
            DbFunctions.TruncateTime(w.TranDate) <= Attendance.ToDate.Date &&
             (string.IsNullOrEmpty(Attendance.EmpCode) || w.EmpCode == Attendance.EmpCode)
             ).ToList();
            var ListBranch = SYConstant.getBranchDataAccess();
            ListEmpSchdule = ListAttendance.Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            ListEmpSchdule = ListEmpSchdule.Where(w => w.DateTerminate.Value.Year == 1900 || w.DateTerminate.Value.Date > Attendance.FromDate.Date).ToList();
            if (Attendance.Branch != null)
            {
                string[] Branch = Attendance.Branch.Split(',');
                List<string> LstBranch = new List<string>();
                foreach (var read in Branch)
                {
                    if (read.Trim() != "")
                    {
                        LstBranch.Add(read.Trim());
                    }
                }

                ListEmpSchdule = ListEmpSchdule.Where(w => LstBranch.Contains(w.Branch)).ToList();
            }
            if (Attendance.Locations != null)
                ListEmpSchdule = ListEmpSchdule.Where(w => w.LOCT == Attendance.Locations).ToList();
            if (Attendance.Department != null)
                ListEmpSchdule = ListEmpSchdule.Where(w => w.DEPT == Attendance.Department).ToList();
            if (Attendance.Division != null)
                ListEmpSchdule = ListEmpSchdule.Where(w => w.DivisionCode == Attendance.Division).ToList();
            if (Attendance.Shift != null)
                ListEmpSchdule = ListEmpSchdule.Where(w => w.SHIFT == Attendance.Shift).ToList();
            ListEmpSchdule = ListEmpSchdule.OrderBy(x => x.TranDate).ToList();
            Progress = ListEmpSchdule.Count();
        }
        public List<ListImportDetail> LoadImportDetail()
        {
            var Approve = SYDocumentStatus.APPROVED.ToString();
            var query = from ros in unitOfWork.Repository<ATImpRosterHeader>().Queryable()
                        join emp in unitOfWork.Repository<HRStaffProfile>().Queryable() on ros.UploadBy equals emp.EmpCode
                        join branch in unitOfWork.Repository<HRBranch>().Queryable() on emp.Branch equals branch.Code into branchGroup
                        from branch in branchGroup.DefaultIfEmpty()
                        join dept in unitOfWork.Repository<HRDepartment>().Queryable() on emp.DEPT equals dept.Code into deptGroup
                        from dept in deptGroup.DefaultIfEmpty()
                        join loct in unitOfWork.Repository<HRLocation>().Queryable() on emp.LOCT equals loct.Code into loctGroup
                        from loct in loctGroup.DefaultIfEmpty()
                        join post in unitOfWork.Repository<HRPosition>().Queryable() on emp.JobCode equals post.Code into postGroup
                        from post in postGroup.DefaultIfEmpty()
                        where ros.Status != Approve
                        orderby ros.DocumentNo descending
                        select new ListImportDetail
                        {
                            DocumentNo = ros.DocumentNo,
                            UploadBy = ros.UploadByName,
                            UploadDate = ros.UploadDate,
                            Status = ros.Status,
                            Branch = branch != null ? branch.Description : null,
                            Department = dept != null ? dept.Description : null,
                            Location = loct != null ? loct.Description : null,
                            Position = post != null ? post.Description : null
                        };

            return query.ToList();
        }
        public void OnFilterStaff(string EmpCode)
        {
            StaffProfile = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (StaffProfile == null)
            {
                StaffProfile = new HRStaffProfile();
            }
        }
        public string GenrateAttendance(string ID)
        {
            string Error_Status = "";
            try
            {
                unitOfWork.BeginTransaction();
                Header = new ATEmpSchedule();
                ListHeader = new List<ATEmpSchedule>();
                var ListInOut = new List<ATInOut>();
                var ListSHift = new List<ATShift>();
                var listLeaveH = new List<HREmpLeave>();
                var ListLeaveD = new List<HREmpLeaveD>();
                string Approval = SYDocumentStatus.APPROVED.ToString();
                var _LeaveType = unitOfWork.Set<HRLeaveType>().ToList();
                List<HRStaffProfile> ListStaff = unitOfWork.Set<HRStaffProfile>().ToList();
                if (Attendance.Branch != null)
                    ListStaff = ListStaff.Where(w => w.Branch == Attendance.Branch).ToList();
                if (Attendance.Locations != null)
                    ListStaff = ListStaff.Where(w => w.LOCT == Attendance.Locations).ToList();
                if (Attendance.Department != null)
                    ListStaff = ListStaff.Where(w => w.DEPT == Attendance.Department).ToList();
                if (Attendance.Division != null)
                    ListStaff = ListStaff.Where(w => w.Division == Attendance.Division).ToList();
                if (Attendance.EmpCode != null)
                    ListStaff = ListStaff.Where(w => w.EmpCode == Attendance.EmpCode).ToList();

                var _list = unitOfWork.Set<ATEmpSchedule>().Where(w =>
                DbFunctions.TruncateTime(w.TranDate) >= Attendance.FromDate.Date && DbFunctions.TruncateTime(w.TranDate) <= Attendance.ToDate.Date).ToList();
                _list = _list.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                var FromDate = Attendance.FromDate.AddDays(-1);
                var ToDate = Attendance.ToDate.AddDays(1);
                var _listINOut = (from s in unitOfWork.Set<ATInOut>()
                                  where DbFunctions.TruncateTime(s.FullDate) >= FromDate.Date
                                  && DbFunctions.TruncateTime(s.FullDate) <= ToDate.Date
                                  && s.EmpCode.Trim() != ""
                                  select s).ToList();


                var _listShift = unitOfWork.Set<ATShift>().ToList();
                var _listLeaveh = unitOfWork.Set<HREmpLeave>().Where(w => w.Status == Approval && w.TranType == false).ToList();
                _listLeaveh = _listLeaveh.Where(w => ((w.FromDate.Date >= FromDate.Date && w.FromDate.Date <= ToDate.Date) ||
                (w.ToDate.Date >= FromDate.Date && w.ToDate.Date <= ToDate.Date) ||
                (FromDate.Date >= w.FromDate.Date && FromDate.Date <= w.ToDate.Date) || (ToDate.Date >= w.FromDate.Date && ToDate.Date <= w.ToDate.Date))).ToList();

                var _listLeaveD = unitOfWork.Set<HREmpLeaveD>().Where(w => DbFunctions.TruncateTime(w.LeaveDate) >= FromDate.Date
                && DbFunctions.TruncateTime(w.LeaveDate) <= ToDate.Date).ToList();
                var ListPub = unitOfWork.Set<HRPubHoliday>().ToList();
                var ListLa_Ea = unitOfWork.Set<ATPolicyLaEa>().ToList();
                ListInOut = _listINOut.ToList();
                ListHeader = _list.ToList();
                ListSHift = _listShift.ToList();
                ListSHift = ListSHift.ToList();
                listLeaveH = _listLeaveh.ToList();
                ListLeaveD = _listLeaveD.ToList();
                _LeaveType = _LeaveType.ToList();
                ListLeaveD = ListLeaveD.Where(w => listLeaveH.Where(x => x.Increment == w.LeaveTranNo && x.Status == Approval).Any()).ToList();
                ATPolicy NWPolicy = unitOfWork.Set<ATPolicy>().FirstOrDefault();
                string[] _TranNo = ID.Split(';');
                int i = 0;
                List<ATEmpSchedule> _listHeader = new List<ATEmpSchedule>();
                var listMaternity = unitOfWork.Set<HRReqMaternity>().ToList();
                var listEmpRework = unitOfWork.Set<ATEmpRelWork>().Where(w => w.InDate >= Attendance.FromDate.Date && w.InDate <= Attendance.ToDate.Date).ToList();
                var listReqLateEarly = unitOfWork.Set<HRReqLateEarly>().ToList();
                var ListPeriod = unitOfWork.Set<ATPayperiod>().Where(w => DbFunctions.TruncateTime(w.FromDate) <= ToDate
                && DbFunctions.TruncateTime(w.ToDate) >= FromDate).ToList();
                Progress = _TranNo.Count();
                var ListPara = unitOfWork.Set<PRParameter>().ToList();

                var ListEmpOTReq = unitOfWork.Set<HRRequestOT>().Where(w => w.Status == Approval && DbFunctions.TruncateTime(w.OTEndTime) >= Attendance.FromDate.Date && DbFunctions.TruncateTime(w.OTEndTime) <= Attendance.ToDate.Date).ToList();
                var ListOTPolic = unitOfWork.Set<ATOTPolicy>().ToList();
                DateTime FromDateMin = new DateTime(FromDate.Year, FromDate.Month, 1);
                DateTime ToDateMax = new DateTime(ToDate.Year, ToDate.Month, DateTime.DaysInMonth(ToDate.Year, ToDate.Month));

                var IS_PERIORD = unitOfWork.Set<SYSetting>().FirstOrDefault(w => w.SettingName == "IS_PERIORD");

                if (ListPeriod.Count > 0)
                {
                    FromDateMin = ListPeriod.Min(w => w.FromDate);
                    ToDateMax = ListPeriod.Min(w => w.ToDate);
                }
                var ListEmpLateEarly = (from s in unitOfWork.Set<ATLateEarlyDeduct>()
                                        where DbFunctions.TruncateTime(s.DocumentDate) >= FromDateMin.Date
                                        && DbFunctions.TruncateTime(s.DocumentDate) <= ToDateMax.Date
                                        select s).ToList();
                ListEmpLateEarly = ListEmpLateEarly.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                if (ListEmpLateEarly.Count() > 0)
                {
                    ListEmpLateEarly.ForEach(w => w.IsLate_Early = 0);
                    ListEmpLateEarly.ForEach(w => w.IsMissScan = 0);
                    ListEmpLateEarly = ListEmpLateEarly.OrderBy(w => w.DocumentDate).ToList();
                }
                List<HRStaffProfile> ListStaffRes = ListStaff.Where(w => w.Status == "I").ToList();
                var ListAtTRes = ListHeader.Where(w => ListStaffRes.Where(x => x.EmpCode == w.EmpCode && (w.TranDate.Date >= x.DateTerminate.Date || x.DateTerminate.Year == 1900)).Any()).ToList();
                //foreach (var item in ListAtTRes)
                //{
                if (ListAtTRes.Count() > 0)
                {
                    unitOfWork.BulkDelete(ListAtTRes);
                }
                //}

                List<ATEmpSchedule> ListUpdateDataATT = new List<ATEmpSchedule>();
                List<ATEmpRelWork> ListUpdateRelWork = new List<ATEmpRelWork>();
                List<ATLateEarlyDeduct> ListDeleteLaEa = new List<ATLateEarlyDeduct>();
                List<ATLateEarlyDeduct> ListAddLaEa = new List<ATLateEarlyDeduct>();
                List<HRRequestOT> ListUpdateOT = new List<HRRequestOT>();
                foreach (var TranNo in _TranNo)
                {
                    ATEmpRelWork _EmpRework = new ATEmpRelWork();
                    ClsAttendance ObjAttendance = new ClsAttendance();
                    if (TranNo.Trim() != "")
                    {
                        i++;
                        int No = Convert.ToInt32(TranNo);

                        var _Empsch = ListHeader.Where(w => w.TranNo == No).ToList();
                        foreach (var item in _Empsch)
                        {
                            Error_Status = item.EmpCode + ":" + item.TranDate.ToString("dd-MMM-yyyy");
                            bool IsPeriod = false;
                            if (IS_PERIORD != null)
                            {
                                if (IS_PERIORD.SettinValue == "YES") IsPeriod = true;
                            }
                            DateTime PFromDate = item.TranDate;
                            DateTime PToDate = item.TranDate;
                            if (IsPeriod == false)
                            {
                                IsPeriod = true;
                                PFromDate = new DateTime(item.TranDate.Year, item.TranDate.Month, 1);
                                PToDate = new DateTime(item.TranDate.Year, item.TranDate.Month, DateTime.DaysInMonth(item.TranDate.Year, item.TranDate.Month));
                            }
                            else
                            {
                                var _Period = ListPeriod.FirstOrDefault(w => w.FromDate.Date <= item.TranDate.Date && w.ToDate.Date >= item.TranDate);
                                if (_Period != null)
                                {
                                    PFromDate = _Period.FromDate;
                                    PToDate = _Period.ToDate;
                                }
                                else IsPeriod = false;
                            }
                            var Staff = ListStaff.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                            if (Staff.IsExceptScan == true)
                            {
                                continue;
                            }
                            var _para = ListPara.FirstOrDefault(w => w.Code == Staff.PayParam);
                            var _Inout = ListInOut.Where(w => w.EmpCode == item.EmpCode).ToList();
                            var _EmpResMater = listMaternity.Where(w => w.EmpCode == item.EmpCode
                            && w.FromDate.Date <= item.TranDate.Date && w.ToDate.Date >= item.TranDate.Date).ToList();
                            var _EmpResLateEalry = listReqLateEarly.Where(w => w.EmpCode == item.EmpCode && w.LeaveDate.Date == item.TranDate.Date && w.Status == "APPROVED").ToList();
                            if (listEmpRework.Where(w => w.FromEmpCode == item.EmpCode && w.InDate.Date == item.TranDate.Date).Any())
                            {
                                _EmpRework = listEmpRework.FirstOrDefault(w => w.FromEmpCode == item.EmpCode && w.InDate.Date == item.TranDate.Date);
                                _Inout = ListInOut.Where(w => w.EmpCode == _EmpRework.ToEmpCode).ToList();
                            }

                            Header.EmpCode = item.EmpCode;
                            item.OTStart = null;
                            item.OTEnd = null;
                            decimal OTRequest = 0;
                            var EmpOTReq = ListEmpOTReq.Where(w => w.EmpCode == item.EmpCode && w.OTEndTime.Date == item.TranDate.Date).ToList();
                            bool IsOT = false;
                            if (EmpOTReq.Count() > 0)
                            {
                                OTRequest = Convert.ToDecimal(EmpOTReq.Sum(w => w.Hours));
                                IsOT = true;
                            }
							ATEmpSchedule result = ObjAttendance.GetResult(item, ListSHift, _Inout, IsOT, NWPolicy, EmpOTReq);
                            DateTime NWFromDate = result.TranDate + NWPolicy.NWFROM.Value.TimeOfDay;
                            DateTime NWToDate = result.TranDate + NWPolicy.NWTO.Value.TimeOfDay;
                            NWToDate = NWToDate.AddDays(1);

                            result = ObjAttendance.GenrateAttendance(result, item.EmpCode, NWPolicy, item.LeaveDesc, ListPub.ToList(),
                                      ListSHift, ListLeaveD, _LeaveType.ToList());
                            result.OTRequest = OTRequest;
                            if (ListPub.Where(w => w.PDate.Date == result.TranDate.Date).Any())
                            {
                                result.Remark2 = "PH";
                            }
                            if (_LeaveType.Where(w => w.Code == result.SHIFT).Any())
                            {
                                result.LeaveDesc = result.SHIFT;
                            }
                            if (result.LeaveDesc == "" || result.LeaveDesc == null)
                                result.LeaveDesc = item.LeaveDesc;

                            //NWPolicy
                            string LeaveLa = "";
                            string LeaveEa = "";
                            ClsMissScan _MissScan = new ClsMissScan();
                            if (ListLeaveD.Where(w => w.LeaveDate.Date == result.TranDate.Date && w.EmpCode == item.EmpCode).Any())
                            {
                                var _Shift = ListSHift.FirstOrDefault(w => w.Code == result.SHIFT);
                                var _Leave = ListLeaveD.Where(w => w.LeaveDate == result.TranDate && w.EmpCode == item.EmpCode).ToList();
                                foreach (var Leave in _Leave)
                                {
                                    var _LeaveCode = _LeaveType.FirstOrDefault(w => w.Code == Leave.LeaveCode);
                                    if (Leave.Remark == "Morning")
                                    {
                                        LeaveLa = Leave.LeaveCode;
                                        if (_Shift != null && result.Late1 > 0)
                                        {
                                            if (result.Start1.Value.TimeOfDay > _Shift.BreakEnd.Value.TimeOfDay)
                                            {
                                                result.Late1 = (int)(result.Start1.Value.TimeOfDay - _Shift.BreakEnd.Value.TimeOfDay).TotalMinutes;
                                            }
                                            if (result.Start1.Value.TimeOfDay < _Shift.BreakEnd.Value.TimeOfDay)
                                            {
                                                result.Late1 = 0;
                                            }
                                        }

                                        ObjAttendance.IsMissScan(_para, _LeaveCode, item, _MissScan, Leave.Remark);
                                    }
                                    else if (Leave.Remark == "Afternoon")
                                    {
                                        LeaveEa = Leave.LeaveCode;
                                        if (_Shift != null && result.Early1 > 0)
                                        {
                                            if (result.End1.Value.TimeOfDay > _Shift.BreakStart.Value.TimeOfDay)
                                            {
                                                result.Early1 = 0;
                                            }
                                        }
                                        ObjAttendance.IsMissScan(_para, _LeaveCode, item, _MissScan, Leave.Remark);
                                    }
                                    else if (Leave.Remark == "Hours")
                                    {
                                        int TotalMin = (int)Leave.EndTime.Value().Subtract(Leave.StartTime.Value()).TotalMinutes;
                                        if (result.IN1 >= Leave.StartTime && result.IN1 <= Leave.EndTime)
                                        {
                                            result.Late1 -= TotalMin; _MissScan.IsMissIN = true;
                                        }
                                        if (result.OUT1 >= Leave.StartTime && result.OUT1 <= Leave.EndTime)
                                        {
                                            result.Early1 -= TotalMin; _MissScan.IsMissOut = true;
                                        }
                                        if (result.Flag2 == 1)
                                        {
                                            if (result.IN2 >= Leave.StartTime && result.IN2 <= Leave.EndTime)
                                            {
                                                result.Late2 -= TotalMin;
                                                _MissScan.IsMissIN2 = true;
                                            }
                                            if (result.OUT2 >= Leave.StartTime && result.OUT2 <= Leave.EndTime)
                                            {
                                                result.Early2 -= TotalMin;
                                                _MissScan.IsMissOut2 = true;
                                            }
                                        }
                                        if (result.Late1 < 0) result.Late1 = 0;
                                        if (result.Early1 < 0) result.Early1 = 0;
                                        if (result.Late2 < 0) result.Late2 = 0;
                                        if (result.Early2 < 0) result.Early2 = 0;
                                    }
                                    else
                                    {
                                        result.Late1 = 0; result.Early1 = 0;
                                        result.Late2 = 0; result.Early2 = 0;
                                        _MissScan.IsMissIN = true; _MissScan.IsMissOut = true;
                                        _MissScan.IsMissIN2 = true; _MissScan.IsMissOut2 = true;
                                    }
                                }
                            }

                            //int CountLa = ListEmpLateEarly.Where(w => w.DocumentDate.Date >= PFromDate.Date && w.DocumentDate.Date <= PToDate.Date
                            //&& w.EmpCode == item.EmpCode && w.DeductType == "LATE").Sum(x => x.IsLate_Early);
                            //int CountEa = ListEmpLateEarly.Where(w => w.DocumentDate.Date >= PFromDate.Date && w.DocumentDate.Date <= PToDate.Date
                            //&& w.EmpCode == item.EmpCode && w.DeductType == "EARLY").Sum(x => x.IsLate_Early);

                            //Request Maternity Late/Early
                            if (_EmpResMater.Sum(x => x.LateEarly) > 0)
                            {
                                result = ObjAttendance.Cal_Maternity_LateEarly(result, _EmpResMater);
                            }

                            //Validate Late Early
                            ObjAttendance.Validate_Late_Early(result, Staff, ListLa_Ea);

                            if (IsPeriod == true)
                            {
                                var _empLaEa = ListEmpLateEarly.Where(w => w.EmpCode == item.EmpCode && w.DocumentDate.Date == item.TranDate.Date).ToList();
                                //Late & Early
                                var LstlateEarly = _empLaEa.Where(w => w.DeductType == "LATE" || w.DeductType == "EARLY").ToList();
                                ListDeleteLaEa.AddRange(LstlateEarly);
                                ObjAttendance.Insert_LateEarly(unitOfWork, result, _MissScan, ListAddLaEa);

                                //MissScan
                                var _empMissScanIN = _empLaEa.Where(w => w.DeductType == "MissScan" || w.DeductType == "ABS").ToList();
                                ListDeleteLaEa.AddRange(_empMissScanIN);
                                ObjAttendance.Insert_MissScan(unitOfWork, result, _MissScan, ListAddLaEa);
                            }

                            //"Req Late/Early"
                            if (_EmpResLateEalry.Sum(x => x.Qty) > 0)
                            {
                                result = ObjAttendance.Cal_Req_Late_Early(result, _EmpResLateEalry);
                            }

                            //Validate_OT
                            result = ObjAttendance.Cal_OT(result, EmpOTReq, ListOTPolic, _Inout);
                            foreach (var _OT in EmpOTReq)
                            {
                                ListUpdateOT.Add(_OT);
                            }
                            result = ObjAttendance.Check_NightShift(result, NWPolicy);
                            if (result.OTApproval < 0) result.OTApproval = 0;
                            if (result.Start1.Value.Year == 1900 && result.End1.Value.Year == 1900
                            && result.Start2.Value.Year == 1900 && result.End2.Value.Year == 1900
                            && (result.LeaveCode == "" || result.LeaveCode == null || result.LeaveCode == "ABS") && (result.LeaveDesc == "" || result.LeaveDesc == null))
                            {
                                result.LeaveDesc = "ABS";
                            }
                            else if (result.Start1.Value.Year != 1900 || result.End1.Value.Year != 1900
                            || result.Start2.Value.Year != 1900 || result.End2.Value.Year != 1900)
                            {
                                result.LeaveDesc = "";
                            }
                            if (result.LeaveDesc == "ABS")
                            {
                                if (result.SHIFT == "OFF" || result.SHIFT == "PH") result.LeaveDesc = "";
                            }
                            if (result.WHOUR > 0)
                            {
                                if (result.TranDate.DayOfWeek == DayOfWeek.Saturday && _para.WDSAT == true && _para.WDSATDay == 0.5M)
                                {
                                    result.ActWHour = (_para.WHOUR / 2) - (Convert.ToDecimal(result.Late1 + result.Late2 + result.Early1 + result.Early2) / 60.00M);
                                }
                                else
                                    result.ActWHour = _para.WHOUR - (Convert.ToDecimal(result.Late1 + result.Late2 + result.Early1 + result.Early2) / 60.00M);
                                result.WokingHour = "";

                                string[] Hour = result.WHOUR.ToString().Split('.');
                                if (Convert.ToInt32(Hour[0]) > 0)
                                    result.WokingHour = Hour[0] + "h";
                                if (Hour.Length > 1 && Convert.ToInt64(Hour[1]) > 0)
                                    result.WokingHour += Convert.ToInt64((result.WHOUR - Convert.ToDecimal(Hour[0])) * 60) + "min";
                            }
                            if (!string.IsNullOrEmpty(_EmpRework.FromEmpCode))
                            {
                                if (result.Start1.Value.Year != 1900)
                                    _EmpRework.StartTime = result.Start1;
                                if (result.Flag1 == 1)
                                    _EmpRework.EndTime = result.End1;
                                if (result.Flag2 == 1)
                                    _EmpRework.EndTime = result.End2;
                                _EmpRework.WorkingHour = result.WHOUR;
                                ListUpdateRelWork.Add(_EmpRework);
                            }
                            result.ChangedBy = User.UserName;
                            result.ChangedOn = DateTime.Now;
                            //unitOfWork.Update(result);
                            ListUpdateDataATT.Add(result);
                        }
                    }
                }
                if (ListUpdateDataATT.Count > 0)
                {
                    unitOfWork.BulkDelete(ListDeleteLaEa);
                    unitOfWork.BulkInsert(ListAddLaEa);
                    if (ListUpdateOT.Count > 0)
                    {
                        unitOfWork.BulkUpdate(ListUpdateOT);
                    }
                    unitOfWork.BulkUpdate(ListUpdateDataATT);
                    if (ListUpdateRelWork.Count > 0)
                    {
                        unitOfWork.BulkUpdate(ListUpdateRelWork);
                    }
                }
                //unitOfWork.Save();

                unitOfWork.BulkCommit();

                return SYConstant.OK;

            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Error_Status, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Error_Status, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Error_Status, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Set_DefaultShift(DateTime FromDate, DateTime ToDate, List<HRBranch> ListBranch)
        {
            string Status_Error = "";
            try
            {
                var _listShift = unitOfWork.Set<ATShift>().ToList();
                var ListPub = unitOfWork.Set<HRPubHoliday>().ToList();
                var ListEmpSch = unitOfWork.Set<ATEmpSchedule>().Where(w => DbFunctions.TruncateTime(w.TranDate) >= FromDate.Date &&
                DbFunctions.TruncateTime(w.TranDate) <= ToDate.Date).ToList();
                var branchCodes = ListBranch.Select(b => b.Code).ToList();
                var ListStaff = (from staff in unitOfWork.Set<HRStaffProfile>()
                                 join branchCode in branchCodes on staff.Branch equals branchCode
                                 where !string.IsNullOrEmpty(staff.ROSTER)
                                 select staff).ToList();

                var ListBatch = unitOfWork.Set<ATBatch>().ToList();
                var ListBatchItem = unitOfWork.Set<ATBatchItem>().ToList();
                ListBatchItem = ListBatchItem.Where(w => ListBatch.Where(x => x.Code == w.BatchNo).Any()).ToList();
                ListPub = ListPub.ToList();
                List<ATEmpSchedule> ListAddSchedule = new List<ATEmpSchedule>();
                foreach (DateTime InDate in DateTimeHelper.EachDay(FromDate, ToDate))
                {
                    var _Shift = new ATShift();
                    var Staff = ListStaff.Where(w => w.StartDate.Value.Date <= InDate.Date &&
                           (w.TerminateStatus == "" || w.TerminateStatus == null || w.DateTerminate > InDate.Date)
                           && (w.ROSTER != "" && w.ROSTER != null)).ToList();

                    foreach (var st in Staff)
                    {
                        if (ListEmpSch.Where(x => x.EmpCode == st.EmpCode && x.TranDate.Date == InDate.Date).ToList().Any())
                            continue;
                        Status_Error = st.EmpCode + ":" + InDate.ToString();

                        var att = new ATEmpSchedule();
                        Shift_Default(att, InDate, st, _listShift, ListPub, ListBatchItem);
                        if (!string.IsNullOrEmpty(att.EmpCode))
                            ListAddSchedule.Add(att);
                    }
                }
                if (ListAddSchedule.Count > 0)
                {
                    unitOfWork.BulkInsert(ListAddSchedule);
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Status_Error, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Status_Error, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Status_Error, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public ATEmpSchedule Shift_Default(ATEmpSchedule att, DateTime InDate, HRStaffProfile Staff,
           List<ATShift> ListShift, List<HRPubHoliday> ListHoliday, List<ATBatchItem> ListBatchItem)
        {
            string ShiftCode = "";
            var _Shift = new ATShift();
            att.Flag1 = 2;
            att.Flag2 = 2;
            DateTime CheckIN1 = new DateTime(1900, 1, 1);
            DateTime CheckOut1 = new DateTime(1900, 1, 1);
            DateTime CheckIN2 = new DateTime(1900, 1, 1);
            DateTime CheckOut2 = new DateTime(1900, 1, 1);
            DateTime CheckDate = new DateTime(1900, 1, 1);
            bool Result = false;
            if (ListHoliday.Where(w => w.PDate.Date == InDate.Date).Any()) { Result = true; ShiftCode = "PH"; }
            var Batch = ListBatchItem.Where(w => w.BatchNo == Staff.ROSTER).ToList();
            if (ListShift.Where(w => Batch.Where(x => x.ShiftCode == w.Code).Any()).Count() > 0)
            {
                if (Batch.Count() > 0 && Result == false)
                {
                    var _Batch = new ATBatchItem();
                    if (InDate.DayOfWeek.ToString() == "Monday")
                    {
                        Batch = Batch.Where(w => w.Mon == true).ToList();
                        if (Batch.Count() > 0)
                        {
                            _Batch = Batch.First();
                        }
                    }
                    else if (InDate.DayOfWeek.ToString() == "Tuesday")
                    {
                        Batch = Batch.Where(w => w.Tue == true).ToList();
                        if (Batch.Count() > 0)
                            _Batch = Batch.First();
                    }
                    else if (InDate.DayOfWeek.ToString() == "Wednesday")
                    {
                        Batch = Batch.Where(w => w.Wed == true).ToList();
                        if (Batch.Count() > 0)
                            _Batch = Batch.First();
                    }
                    else if (InDate.DayOfWeek.ToString() == "Thursday")
                    {
                        Batch = Batch.Where(w => w.Thu == true).ToList();
                        if (Batch.Count() > 0)
                            _Batch = Batch.First();
                    }
                    else if (InDate.DayOfWeek.ToString() == "Friday")
                    {
                        Batch = Batch.Where(w => w.Fri == true).ToList();
                        if (Batch.Count() > 0)
                            _Batch = Batch.First();
                    }
                    else if (InDate.DayOfWeek.ToString() == "Saturday")
                    {
                        Batch = Batch.Where(w => w.Sat == true).ToList();
                        if (Batch.Count() > 0)
                            _Batch = Batch.First();
                    }
                    else if (InDate.DayOfWeek.ToString() == "Sunday")
                    {
                        Batch = Batch.Where(w => w.Sun == true).ToList();
                        if (Batch.Count() > 0)
                            _Batch = Batch.First();
                    }

                    if (_Batch.BatchNo != null)
                    {
                        _Shift = ListShift.Where(w => w.Code == _Batch.ShiftCode).FirstOrDefault();
                        if (_Shift == null)
                            return null;
                        else ShiftCode = _Shift.Code;
                    }
                    else
                    {
                        ShiftCode = "OFF";
                        Result = true;
                    }
                }
                if (ShiftCode == "")
                    return null;
                if (Result == false)
                {
                    CheckIN1 = new DateTime(InDate.Year, InDate.Month, InDate.Day, _Shift.CheckIn1.Value.Hour, _Shift.CheckIn1.Value.Minute, 0);
                    CheckOut1 = new DateTime(InDate.Year, InDate.Month, InDate.Day, _Shift.CheckOut1.Value.Hour, _Shift.CheckOut1.Value.Minute, 0);
                    if (_Shift.OverNight1 == true) CheckOut1 = CheckOut1.AddDays(1);
                    if (_Shift.SplitShift == true)
                    {
                        att.Flag2 = 1;
                        CheckIN2 = new DateTime(InDate.Year, InDate.Month, InDate.Day, _Shift.CheckIn2.Value.Hour, _Shift.CheckIn2.Value.Minute, 0);
                        CheckOut2 = new DateTime(InDate.Year, InDate.Month, InDate.Day, _Shift.CheckOut2.Value.Hour, _Shift.CheckOut2.Value.Minute, 0);
                    }
                    att.Flag1 = 1;
                }
                att.EmpCode = Staff.EmpCode;
                att.TranDate = InDate;
                att.IN1 = CheckIN1;
                att.OUT1 = CheckOut1;
                att.Start1 = CheckDate;
                att.End1 = CheckDate;
                att.Late1 = 0;
                att.LateDesc1 = "";
                att.LateVal1 = 0;
                att.LateFlag1 = "";
                att.LateAm1 = 0;
                att.Early1 = 0;
                att.DepDesc1 = "";
                att.DepVal1 = 0;
                att.DepFlag1 = "";
                att.DepAm1 = 0;
                att.IN2 = CheckIN2;
                att.OUT2 = CheckOut2;
                att.Start2 = CheckDate;
                att.End2 = CheckDate;
                att.Late2 = 0;
                att.LateDesc2 = "";
                att.LateVal2 = 0;
                att.LateFlag2 = "";
                att.LateAm2 = 0;
                att.Early2 = 0;
                att.DepDesc2 = "";
                att.DepVal2 = 0;
                att.DEPFLAG2 = "";
                att.DEPAM2 = 0;
                att.OTTYPE = "-1";
                att.LeaveNo = -1;
                att.SHIFT = ShiftCode;
                att.STATUS = 0;
                att.REMARK = "";
                att.CreateBy = User.CreatedBy;
                att.CreateOn = DateTime.Now;
                att.LeaveCode = "";
            }

            return att;
        }
        public string GenrateAttendance_(string TranNo)
        {
            List<string> ListTranNo = TranNo.Split(';').Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
            var FromDate = Attendance.FromDate.AddDays(-1);
            var ToDate = Attendance.ToDate.AddDays(1);
            string Approval = SYDocumentStatus.APPROVED.ToString();

            //var _list = unitOfWork.Set<ATEmpSchedule>().Where(w => ListTranNo.Contains(w.TranNo.ToString()) &&
            //        w.TranDate >= Attendance.FromDate.Date && w.TranDate <= Attendance.ToDate.Date).ToList();

            var _listAttendance = unitOfWork.Set<ATEmpSchedule>().Where(w => w.TranDate >= Attendance.FromDate.Date && w.TranDate <= Attendance.ToDate.Date).ToList();

            List<string> ListEmpCode = _listAttendance.Where(w => ListTranNo.Contains(w.TranNo.ToString()))
                .GroupBy(y => y.EmpCode).Select(a => a.Key).ToList();

            var _LeaveType = unitOfWork.Set<HRLeaveType>().ToList();
            var _listShift = unitOfWork.Set<ATShift>().ToList();
            List<HRStaffProfile> ListStaff = unitOfWork.Set<HRStaffProfile>()
                .Where(w => ListEmpCode.Contains(w.EmpCode) &&
                    (string.IsNullOrEmpty(Attendance.Branch) || w.Branch == Attendance.Branch) &&
                    (string.IsNullOrEmpty(Attendance.Locations) || w.LOCT == Attendance.Locations) &&
                    (string.IsNullOrEmpty(Attendance.Department) || w.DEPT == Attendance.Department) &&
                    (string.IsNullOrEmpty(Attendance.Division) || w.Division == Attendance.Division) &&
                    (string.IsNullOrEmpty(Attendance.EmpCode) || w.EmpCode == Attendance.EmpCode)
                ).ToList();
            var ListInOut = unitOfWork.Set<ATInOut>().Where(w => ListEmpCode.Contains(w.EmpCode) &&
          DbFunctions.TruncateTime(w.FullDate) >= FromDate.Date.AddDays(-1) && DbFunctions.TruncateTime(w.FullDate) <= ToDate).ToList();


            var ListLeaveD = (from leaveD in unitOfWork.Set<HREmpLeaveD>()
                              join leaveh in unitOfWork.Set<HREmpLeave>()
                              on leaveD.LeaveTranNo equals leaveh.Increment
                              where leaveh.Status == Approval
                              && leaveD.LeaveDate >= FromDate && leaveD.LeaveDate <= ToDate
                              && leaveh.EmpCode == leaveh.EmpCode
                              && leaveD.EmpCode == User.UserName
                              select leaveD
                                        ).ToListAsync();

            List<ATShift> ListShift = unitOfWork.Set<ATShift>().ToList();

            foreach (var item in _listAttendance)
            {
                var _Inout = ListInOut.Where(w => w.EmpCode == item.EmpCode).ToList();
                //ATEmpSchedule result = ClsAttendance.GetAtten_InOut(item, ListShift, _Inout, false);

            }
            return SYConstant.OK;
        }
        public string TransferOT(string ID, List<HRRequestOT> List)
        {
            if (ID.Trim() == "")
            {
                return "INV_DOC";
            }
            try
            {
                var Policy = unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
                if (Policy == null) Policy = new ATPolicy();
                string approve = SYDocumentStatus.APPROVED.ToString();
                var payperiord = unitOfWork.Repository<ATPayperiod>().Queryable().FirstOrDefault(w => w.PeriodID == Attendance.PeriodID);
                var ListEmpAtt = unitOfWork.Repository<HRRequestOT>().Queryable()
                    .Where(w => ((DbFunctions.TruncateTime(w.OTStartTime) >= payperiord.FromDate && DbFunctions.TruncateTime(w.OTStartTime) <= payperiord.ToDate) ||
                    (DbFunctions.TruncateTime(w.OTEndTime) >= payperiord.FromDate && DbFunctions.TruncateTime(w.OTEndTime) <= payperiord.ToDate) ||
                    (payperiord.FromDate >= DbFunctions.TruncateTime(w.OTStartTime) && payperiord.FromDate <= DbFunctions.TruncateTime(w.OTEndTime)) ||
                    (payperiord.ToDate >= DbFunctions.TruncateTime(w.OTStartTime) && payperiord.ToDate.Date <= DbFunctions.TruncateTime(w.OTEndTime))) && w.Status == approve).ToList();

                string[] _TranNo = ID.Split(';');
                var OTRate = unitOfWork.Repository<PROTRate>().Queryable().ToList();
                List<ATOTSetting> ListOTSetting = unitOfWork.Repository<ATOTSetting>().Queryable().ToList();
                var ListEmpOT = unitOfWork.Repository<PREmpOverTime>().Queryable().Where(w => DbFunctions.TruncateTime(w.OTDate) >= payperiord.FromDate &&
                  DbFunctions.TruncateTime(w.OTDate) <= payperiord.ToDate && w.OTType != Policy.ExtraNS).ToList();
                ListEmpOT = ListEmpOT.ToList();
                foreach (var _OT in ListEmpOT.Where(w => w.TranType == 1))
                {
                    unitOfWork.Delete(_OT);
                }
                foreach (var TranNo in _TranNo)
                {
                    var item = ListEmpAtt.FirstOrDefault(w => w.OTRNo == TranNo);
                    if (item == null) continue;
                    var atsc = unitOfWork.Repository<ATEmpSchedule>().Queryable().FirstOrDefault(w => w.TranNo == item.ReferenceNo);
                    if (atsc == null) continue;
                    decimal OT = 0;
                    var OTH = new PREmpOverTime();
                    string OTCode = "";
                    if (item.OTActual > 0)
                    {
                        ATOTSetting OTSetting = new ATOTSetting();
                        if (atsc.SHIFT == "PH" || atsc.SHIFT == "OFF")
                        {
                            if (atsc.SHIFT == "PH")
                            {
                                OTSetting = ListOTSetting.FirstOrDefault(w => w.IsPH == true);
                            }
                            else if (atsc.SHIFT == "OFF")
                            {

                                if (atsc.TranDate.DayOfWeek == DayOfWeek.Sunday)
                                    OTSetting = ListOTSetting.FirstOrDefault(w => w.IsSunday == true);
                                else OTSetting = ListOTSetting.FirstOrDefault(w => w.IsDayOFF == true);
                            }
                            if (OTSetting != null)
                                atsc.OTTYPE = OTSetting.OTTYPE;
                            if (OTSetting == null) return "Please Check EmpCode " + item.EmpCode + " " + atsc.TranDate.ToString("dd/MMM/yyyy");
                            OTCode = OTSetting.OTTYPE;
                            if (!OTRate.Where(w => w.OTCode == OTCode).Any())
                            {
                                return "INVALID_OTCODE";
                            }
                            OT = 0;
                            var _staff = List.Where(w => w.EmpCode == item.EmpCode).FirstOrDefault();
                            OT = item.OTActual ?? 0;
                            OTH.EmpCode = item.EmpCode;
                            OTH.EmpName = _staff.AllName;
                            OTH.OTDate = atsc.TranDate;
                            OTH.PayMonth = atsc.TranDate;
                            OTH.OTType = OTCode;
                            OTH.LCK = 0;
                            OTH.TranType = 1;
                            OTH.OTHour = OT;
                            OTH.CreateBy = User.UserName;
                            OTH.CreateOn = DateTime.Now;
                            OTH.Reason = item.Reason;
                            OTH.OTFromTime = item.OTActStart;
                            OTH.OTToTime = item.OTActEnd;
                            if (OTRate.Where(w => w.OTCode == OTCode).Any())
                                OTH.OTDescription = OTRate.FirstOrDefault(w => w.OTCode == OTCode).OTType;
                            var EmpOT = ListEmpOT.Where(w => w.EmpCode == TranNo && w.OTType == OTCode && w.OTDate == atsc.TranDate.Date).ToList();
                            unitOfWork.Add(OTH);
                            atsc.OTTYPE = OTCode;
                            unitOfWork.Update(item);
                        }
                        else
                        {
                            DateTime? StartOT = item.OTActStart;
                            DateTime? OTEnd = item.OTActEnd;
                            var _LstOTSetting = ListOTSetting.Where(w => w.IsPH != true && w.IsDayOFF != true).ToList();
                            _LstOTSetting.ToList().ForEach(w => w.StartTime = atsc.TranDate.Date + w.StartTime.Value.TimeOfDay);
                            _LstOTSetting.ToList().ForEach(w => w.EndTime = atsc.TranDate.Date + w.EndTime.Value.TimeOfDay);
                            _LstOTSetting.Where(w => w.StartTime.Value.TimeOfDay >= w.EndTime.Value.TimeOfDay).ToList().ForEach(w => w.EndTime = w.EndTime.Value.AddDays(1));
                            _LstOTSetting = _LstOTSetting.Where(w => w.StartTime <= item.OTActStart && w.EndTime >= item.OTActEnd).ToList();
                            int Count_OT = _LstOTSetting.Count();
                            int i = 0;
                            decimal Temp_OT = item.OTActual ?? 0;
                            foreach (var _OT in _LstOTSetting)
                            {
                                i += 1;
                                OT = 0;
                                OTH = new PREmpOverTime();
                                OTCode = "";
                                var S_OT = atsc.TranDate.Date + _OT.StartTime.Value.TimeOfDay;
                                var E_OT = atsc.TranDate.Date + _OT.EndTime.Value.TimeOfDay;
                                //if (S_OT > E_OT) E_OT = E_OT.AddDays(1);
                                if (S_OT <= StartOT)
                                {
                                    if (E_OT <= item.OTActEnd)
                                    {
                                        OT = (decimal)E_OT.Subtract(StartOT.Value).TotalHours;
                                        OTH.OTFromTime = StartOT;
                                        OTH.OTToTime = E_OT;
                                    }
                                    else
                                    {
                                        OT = (decimal)item.OTActEnd.Value.Subtract(StartOT.Value).TotalHours;
                                        OTH.OTFromTime = StartOT;
                                        OTH.OTToTime = item.OTActEnd;
                                    }
                                }
                                else if (OTEnd <= item.OTActEnd)
                                {
                                    OT = (decimal)item.OTActEnd.Value.Subtract(S_OT).TotalHours;
                                    OTH.OTFromTime = S_OT;
                                    OTH.OTToTime = item.OTActEnd;
                                }
                                if (OT < 0) continue;
                                OTCode = _OT.OTTYPE;
                                if (!OTRate.Where(w => w.OTCode == OTCode).Any())
                                {
                                    return "INVALID_OTCODE";
                                }
                                var _staff1 = List.Where(w => w.EmpCode == item.EmpCode).FirstOrDefault();
                                OTH.EmpCode = item.EmpCode;
                                OTH.EmpName = _staff1.AllName;
                                OTH.OTDate = atsc.TranDate;
                                OTH.PayMonth = atsc.TranDate.Date;
                                OTH.OTType = OTCode;
                                OTH.LCK = 0;
                                OTH.TranType = 1;
                                OTH.OTHour = Math.Round(OT, 2);
                                if (_LstOTSetting.Count() == 1)
                                    OTH.OTHour = item.OTActual ?? 0;
                                else if (i != Count_OT)
                                {
                                    Temp_OT -= OTH.OTHour;
                                }
                                else if (i == Count_OT) OTH.OTHour = Temp_OT;
                                OTH.CreateBy = User.UserName;
                                OTH.CreateOn = DateTime.Now;
                                OTH.Reason = item.Reason;
                                if (OTRate.Where(w => w.OTCode == OTCode).Any())
                                    OTH.OTDescription = OTRate.FirstOrDefault(w => w.OTCode == OTCode).OTType;
                                unitOfWork.Add(OTH);
                                atsc.OTTYPE = OTCode;
                                unitOfWork.Update(item);
                            }
                            if (_LstOTSetting.Count() == 0)
                            {
                                return "Please Check EmpCode " + item.EmpCode + " " + atsc.TranDate.ToString("dd/MMM/yyyy");
                            }

                        }

                    }
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Attendance.PeriodID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Attendance.PeriodID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Attendance.PeriodID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public void OnLoadRoster()
        {
            LIstEmplSch = new List<ListEmpSch>();
            DateTime FromDate = new DateTime(Period.Year, Period.Month, 1);
            DateTime ToDate = new DateTime(Period.Year, Period.Month,
                DateTime.DaysInMonth(Period.Year, Period.Month));
            var ListBranch = SYConstant.getBranchDataAccess().Select(x => x.Code).ToList();
            var dtHeader = unitOfWork.Repository<HRStaffProfile>().Queryable()
                .Where(w => (ListBranch.Contains(w.Branch)) &&
                    (string.IsNullOrEmpty(Attendance.Branch) || w.Branch == Attendance.Branch) &&
                    (string.IsNullOrEmpty(Attendance.Division) || w.Division == Attendance.Division) &&
                    (string.IsNullOrEmpty(Attendance.Locations) || w.LOCT == Attendance.Locations) &&
                    (string.IsNullOrEmpty(Attendance.Department) || w.DEPT == Attendance.Department) &&
                    (Attendance.IsIncludeBatch || string.IsNullOrEmpty(w.ROSTER)))
                .Where(w => DbFunctions.TruncateTime(w.StartDate.Value) <= ToDate.Date && (w.TerminateStatus == ""
                || w.TerminateStatus == null || DbFunctions.TruncateTime(w.DateTerminate) >= FromDate.Date))
                .Where(w => w.IsExceptScan != true);

            var ListEmp = dtHeader.ToList();
            var Pos = unitOfWork.Repository<HRPosition>().Queryable().ToList();
            foreach (var read in dtHeader)
            {
                string poss = "";
                if (read.JobCode != null)
                {
                    if (Pos.Where(w => w.Code == read.JobCode).Any())
                        poss = Pos.Where(w => w.Code == read.JobCode).First().Description;
                }

                var objHeader = new ListEmpSch();

                objHeader.EmpCode = read.EmpCode;
                objHeader.AllName = read.AllName;
                objHeader.Position = poss;
                LIstEmplSch.Add(objHeader);
            }
        }
        public Dictionary<string, dynamic> OnDataSelector(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            var ListBranch = SYConstant.getBranchDataAccess();
            var ListBranchCodes = ListBranch.Select(b => b.Code).ToList();
            var ListStaff = (from staff in unitOfWork.Set<HRStaffProfile>()
                             join branchCode in ListBranchCodes on staff.Branch equals branchCode
                             select staff).ToList();

            keyValues.Add("STAFF_SELECT", ListStaff);
            keyValues.Add("SHIFT_SELECT", unitOfWork.Set<ATShift>().ToList());
            keyValues.Add("BRANCHES_SELECT", ListBranch);
            keyValues.Add("DIVISION_SELECT", ClsFilter.LoadDivision());
            keyValues.Add("DEPARTMENT_SELECT", ClsFilter.LoadDepartment());
            keyValues.Add("LOCATION_SELECT", unitOfWork.Set<HRLocation>().ToList());
            keyValues.Add("PERIOD_SELECT", unitOfWork.Repository<ATPayperiod>().Queryable().ToList().OrderByDescending(w => w.PeriodID));


            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataSelectorRoster(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            var ListBranch = SYConstant.getBranchDataAccess();
            keyValues.Add("BRANCHES_SELECT", ListBranch);
            keyValues.Add("DIVISION_SELECT", ClsFilter.LoadDivision());
            keyValues.Add("DEPARTMENT_SELECT", ClsFilter.LoadDepartment());
            keyValues.Add("LOCATION_SELECT", unitOfWork.Set<HRLocation>().ToList());

            return keyValues;
        }

        #region Import Roster
        public string uploadRoster(List<Temp_Roster> lsttemp_Rosters, DateTime _Period)
        {
            string Error_Status = "";
            OnLoad();
            if (lsttemp_Rosters.Count == 0)
            {
                return "NO_DATA";
            }
            var date = DateTime.Now;
            try
            {
                unitOfWork.BeginTransaction();
                Header = new ATEmpSchedule();
                DateTime FromDate = new DateTime(_Period.Year, _Period.Month, 1);
                DateTime ToDate = new DateTime(_Period.Year, _Period.Month, DateTime.DaysInMonth(_Period.Year, _Period.Month));

                var lstAttendance = unitOfWork.Repository<ATEmpSchedule>().Queryable().Where(w => w.TranDate.Year == _Period.Year
                && w.TranDate.Month == _Period.Month).ToList();
                lstAttendance = lstAttendance.ToList();
                var listShift = unitOfWork.Repository<ATShift>().Queryable().ToList();
                var ListLeaveType = unitOfWork.Repository<HRLeaveType>().Queryable().ToList();
                var Approval = SYDocumentStatus.APPROVED.ToString();
                var listLeaveH = unitOfWork.Repository<HREmpLeave>().Queryable().Where(w => w.Status == Approval).ToList();
                listLeaveH = listLeaveH.Where(w => ((w.FromDate.Date >= FromDate.Date && w.FromDate.Date <= ToDate.Date) ||
                  (w.ToDate.Date >= FromDate.Date && w.ToDate.Date <= ToDate.Date) ||
                        (FromDate.Date >= w.FromDate.Date && FromDate.Date <= w.ToDate.Date) || (ToDate.Date >= w.FromDate.Date && ToDate.Date <= w.ToDate.Date))).ToList();

                var ListLeave = unitOfWork.Repository<HREmpLeaveD>().Queryable().Where(w => w.LeaveDate.Year == _Period.Year && w.LeaveDate.Month == _Period.Month).ToList();
                listShift = listShift.ToList();
                ListLeaveType = ListLeaveType.ToList();
                ListLeave = ListLeave.Where(x => listLeaveH.Where(w => w.EmpCode == x.EmpCode && w.Increment == x.LeaveTranNo).Any()).ToList();
                lstAttendance = lstAttendance.Where(w => lsttemp_Rosters.Where(x => x.EmpCode == w.EmpCode &&
                                   w.TranDate.Date == x.InDate.Date && x.Shift != "").Any()).ToList();

                int Counts = 0;
                if (lstAttendance.Count() > 0)
                {
                    unitOfWork.BulkDelete(lstAttendance);
                }
                Counts = 0;
                var ListEmpRoster = new List<ATEmpSchedule>();
                foreach (var item in lsttemp_Rosters)
                {
                    Error_Status = item.EmpCode + ":" + item.InDate.ToString("dd.MM.yyyy");
                    var _listLeaveH = listLeaveH.Where(w => w.EmpCode == item.EmpCode).ToList();
                    var _ListLeave = ListLeave.Where(w => w.EmpCode == item.EmpCode && w.LeaveDate.Date == item.InDate.Date).ToList();
                    Header.EmpCode = item.EmpCode;
                    Counts += 1;
                    if (item.Shift.Trim() == "") continue;
                    var Att = getEmpSchedule(item, listShift, ListLeaveType, _listLeaveH, _ListLeave);
                    ListEmpRoster.Add(Att);
                }
                unitOfWork.BulkInsert(ListEmpRoster);
                unitOfWork.BulkCommit();
                return SYConstant.OK;

            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Error_Status, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Error_Status, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Error_Status, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public List<ListEmpSch> LoadEmpImport(string DocumentNo)
        {
            //var EmpSch = unitOfWork.Repository<ATImpRoster>().Queryable().Where(w => w.DocumentNo == DocumentNo).ToList();
            //var Staff = unitOfWork.Repository < HR_STAFF_VIEW>().Queryable().ToList();
            var d = (from f in unitOfWork.Repository<ATImpRoster>().Queryable()
                     join emp in unitOfWork.Repository<HR_STAFF_VIEW>().Queryable()
                     on f.EmpCode equals emp.EmpCode
                     where f.DocumentNo == DocumentNo
                     group f by new { f.EmpCode, f.InYear, f.InMonth, emp.AllName, emp.Position }
                         into myGroup
                     where myGroup.Count() > 0
                     select new
                     {
                         myGroup.Key.EmpCode,
                         myGroup.Key.AllName,
                         myGroup.Key.Position,
                         ListShift = myGroup.GroupBy(f => f.InDate.Value.Day).Select
                         (m => new { day = m.Key, Shift = m.Select(w => w.Shift.ToString()).First() })
                     }).ToList();

            foreach (var item in d)
            {
                var Restult1 = (from result in item.ListShift select new { result.day, result.Shift }).ToList();
                var empSch = new ListEmpSch();
                empSch.EmpCode = item.EmpCode;
                empSch.AllName = item.AllName;
                empSch.Position = item.Position;
                foreach (var u_item in Restult1)
                {
                    switch (u_item.day)
                    {
                        case 1: empSch.D_1 = u_item.Shift.ToString(); break;
                        case 2: empSch.D_2 = u_item.Shift.ToString(); break;
                        case 3: empSch.D_3 = u_item.Shift.ToString(); break;
                        case 4: empSch.D_4 = u_item.Shift.ToString(); break;
                        case 5: empSch.D_5 = u_item.Shift.ToString(); break;
                        case 6: empSch.D_6 = u_item.Shift.ToString(); break;
                        case 7: empSch.D_7 = u_item.Shift.ToString(); break;
                        case 8: empSch.D_8 = u_item.Shift.ToString(); break;
                        case 9: empSch.D_9 = u_item.Shift.ToString(); break;
                        case 10: empSch.D_10 = u_item.Shift.ToString(); break;
                        case 11: empSch.D_11 = u_item.Shift.ToString(); break;
                        case 12: empSch.D_12 = u_item.Shift.ToString(); break;
                        case 13: empSch.D_13 = u_item.Shift.ToString(); break;
                        case 14: empSch.D_14 = u_item.Shift.ToString(); break;
                        case 15: empSch.D_15 = u_item.Shift.ToString(); break;
                        case 16: empSch.D_16 = u_item.Shift.ToString(); break;
                        case 17: empSch.D_17 = u_item.Shift.ToString(); break;
                        case 18: empSch.D_18 = u_item.Shift.ToString(); break;
                        case 19: empSch.D_19 = u_item.Shift.ToString(); break;
                        case 20: empSch.D_20 = u_item.Shift.ToString(); break;
                        case 21: empSch.D_21 = u_item.Shift.ToString(); break;
                        case 22: empSch.D_22 = u_item.Shift.ToString(); break;
                        case 23: empSch.D_23 = u_item.Shift.ToString(); break;
                        case 24: empSch.D_24 = u_item.Shift.ToString(); break;
                        case 25: empSch.D_25 = u_item.Shift.ToString(); break;
                        case 26: empSch.D_26 = u_item.Shift.ToString(); break;
                        case 27: empSch.D_27 = u_item.Shift.ToString(); break;
                        case 28: empSch.D_28 = u_item.Shift.ToString(); break;
                        case 29: empSch.D_29 = u_item.Shift.ToString(); break;
                        case 30: empSch.D_30 = u_item.Shift.ToString(); break;
                        case 31: empSch.D_31 = u_item.Shift.ToString(); break;
                    }
                }
                LIstEmplSch.Add(empSch);
            }
            return LIstEmplSch;
        }
        public string DeleteDoc(string DocumentNo)
        {
            OnLoad();
            if (string.IsNullOrEmpty(DocumentNo))
            {
                return "NO_DATA";
            }
            string Cancel = SYDocumentStatus.CANCELLED.ToString();
            var Doc = unitOfWork.Repository<ATImpRosterHeader>().Queryable().Where(w => w.DocumentNo == DocumentNo && w.Status == Cancel).FirstOrDefault();
            if (Doc != null)
            {
                if (Doc.Status != Cancel)
                {
                    return "DOC_INV";
                }
                else
                {
                    unitOfWork.Repository<ATImpRosterHeader>().Delete(Doc);
                }
            }
            unitOfWork.Save();
            return SYSConstant.OK;
        }
        public ATEmpSchedule getEmpSchedule(Temp_Roster _Roster,
           List<ATShift> listShift, List<HRLeaveType> ListLeaveType, List<HREmpLeave> listLeaveH,
           List<HREmpLeaveD> ListLeave)
        {
            var ObjEmpSch = new ATEmpSchedule();
            string LeaveCode = "";
            string LeaveDesc = "";
            listLeaveH = listLeaveH.Where(w => w.Status == SYDocumentStatus.APPROVED.ToString()).ToList();
            ListLeave = ListLeave.Where(x => listLeaveH.Where(w => w.EmpCode == x.EmpCode && w.Increment == x.LeaveTranNo).Any()).ToList();

            DateTime CheckIN1 = new DateTime(1900, 1, 1);
            DateTime CheckOut1 = new DateTime(1900, 1, 1);
            DateTime CheckIN2 = new DateTime(1900, 1, 1);
            DateTime CheckOut2 = new DateTime(1900, 1, 1);
            DateTime CheckDate = new DateTime(1900, 1, 1);
            ObjEmpSch.EmpCode = _Roster.EmpCode;
            ObjEmpSch.TranDate = _Roster.InDate;
            ObjEmpSch.Flag1 = 2;
            ObjEmpSch.Flag2 = 2;
            ObjEmpSch.SHIFT = _Roster.Shift.ToUpper();
            if (listShift.Where(w => w.Code == _Roster.Shift).Any())
            {
                var _shift = listShift.FirstOrDefault(w => w.Code == _Roster.Shift);
                CheckIN1 = _Roster.InDate + _shift.CheckIn1.Value.TimeOfDay;
                CheckOut1 = _Roster.InDate + _shift.CheckOut1.Value.TimeOfDay;
                if (_shift.OverNight1 == true) CheckOut1 = CheckOut1.AddDays(1);
                if (_shift.SplitShift == true && _shift.CheckIn2.HasValue && _shift.CheckOut2.HasValue)
                {
                    ObjEmpSch.Flag2 = 1;
                    CheckIN2 = _Roster.InDate + _shift.CheckIn2.Value.TimeOfDay;
                    CheckOut2 = _Roster.InDate + _shift.CheckOut2.Value.TimeOfDay;
                }
                ObjEmpSch.Flag1 = 1;
            }
            var result = ListLeave.FirstOrDefault(w => w.EmpCode == _Roster.EmpCode
            && w.LeaveDate.Date == _Roster.InDate.Date);
            if (result != null) LeaveCode = result.LeaveCode;

            else if (_Roster.Shift.ToUpper() == "PH" || _Roster.Shift.ToUpper() == "OFF")
            {

            }
            else if (ListLeaveType.Where(w => w.Code == _Roster.Shift.ToString()).Count() > 0)
            {
                LeaveDesc = _Roster.Shift.ToUpper();
            }
            ObjEmpSch.LeaveDesc = LeaveDesc;
            ObjEmpSch.IN1 = CheckIN1;
            ObjEmpSch.OUT1 = CheckOut1;
            ObjEmpSch.IN2 = CheckIN2;
            ObjEmpSch.OUT2 = CheckOut2;
            ObjEmpSch.LeaveCode = LeaveCode;
            ObjEmpSch.Late1 = 0;
            ObjEmpSch.LateVal1 = 0;
            ObjEmpSch.Early1 = 0;
            ObjEmpSch.DepVal1 = 0;
            ObjEmpSch.Late2 = 0;
            ObjEmpSch.LateVal2 = 0;
            ObjEmpSch.Early2 = 0;
            ObjEmpSch.DepVal2 = 0;
            ObjEmpSch.CreateBy = User.UserName;
            ObjEmpSch.CreateOn = DateTime.Now;
            ObjEmpSch.LateAm1 = 0;
            ObjEmpSch.DepAm1 = 0;
            ObjEmpSch.LateAm2 = 0;
            ObjEmpSch.DEPAM2 = 0;
            ObjEmpSch.OTTYPE = "-1";
            ObjEmpSch.LeaveNo = -1;
            ObjEmpSch.WHOUR = 0;
            ObjEmpSch.WOT = 0;
            ObjEmpSch.NWH = 0;
            ObjEmpSch.Start1 = CheckDate;
            ObjEmpSch.End1 = CheckDate;
            ObjEmpSch.Start2 = CheckDate;
            ObjEmpSch.End2 = CheckDate;

            return ObjEmpSch;
        }
        #endregion
    }
    public class ClsAtt
    {
        public string Leave { get; set; }
        public bool? GEN { get; set; }
        public decimal WHour { get; set; }
        public decimal NWH { get; set; }
        public decimal WOT { get; set; }
        public decimal OTMin { get; set; }
        public int Late { get; set; }
        public int Early { get; set; }
    }
    public class ClsEmpShift
    {
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string Position { get; set; }
        public DateTime InDate { get; set; }
        public string Shift { get; set; }
        public int GroupNo { get; set; }
        public string TransDateStr { get; set; }

        public string SummaryGroup { get; set; }
        public int WorkingDays { get; set; }
        public int OFFDays { get; set; }
        public int PHDays { get; set; }
    }
    public class ListImportDetail
    {
        public string DocumentNo { get; set; }
        public string UploadBy { get; set; }
        public DateTime? UploadDate { get; set; }
        public string Branch { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
    }
}
