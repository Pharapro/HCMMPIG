using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.Helper;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Integration.EF.Models;
using Humica.Logic.Atts;
using Humica.Logic.LM;
using Humica.Logic.PR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Humica.Logic.Att
{
    public class ATEmpSchObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public DateTime Period { get; set; }
        public ATEmpSchedule Header { get; set; }
        public ATImpRosterHeader RosterHeader { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public FTFilterAttendance Attendance { get; set; }
        public FTMonthlySum FMonthly { get; set; }
        public DayInMonth HeaderDayInMonth { get; set; }
        public List<ATEmpSchedule> ListHeader { get; set; }
        public List<DayInMonth> ListDayInMonth { get; set; }
        public List<VIEW_ATEmpSchedule> ListEmpSchdule { get; set; }
        public List<ClsSumLaEa> ListSumLaEa { get; set; }
        public List<VIEW_ATEmpSchedule> ListEmpSchduleDetail { get; set; }
        public List<ListEmpSch> LIstEmplSch { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public List<ATImpRosterHeader> ListImpRoster { get; set; }
		public List<ListImportDetail> ListImportDetail { get; set; }
		public string EmpID { get; set; }
        public List<HRStaffProfile> ListStaffs { get; set; }
        public int Progress { get; set; }
        public static List<ListProgress> ListProgress { get; set; }
        public List<PRParameter> ListPayParameter { get; set; }
        public List<ATGenMeal> ListMeal { get; set; }
        public List<VIEW_EmpOnsite> ListEmpOnSite { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public ATEmpSchObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<DayInMonth> listDay()
        {
            ListDayInMonth = new List<DayInMonth>();
            for (int i = 1; i <= 31; i++)
            {
                var DayIn = new DayInMonth();
                DayIn.Day = i;
                ListDayInMonth.Add(DayIn);
            }
            return ListDayInMonth;
        }
        public List<DayInMonth> listEmployeeRoster()
        {
            var ATEmp = DB.ATEmpSchedules.Where(w => w.EmpCode == Filter.EmpCode && w.TranDate.Year == Filter.INYear).ToList();
            ListHeader = new List<ATEmpSchedule>();
            ListHeader = ATEmp.ToList();
            if (ListHeader.ToList().Count > 0)
            {
                //ListDayInMonth = new List<DayInMonth>();
                foreach (var item in ListHeader)
                {
                    var DayIn = new DayInMonth();
                    DayIn.Day = item.TranDate.Day;
                    DayIn.EmpCode = item.EmpCode;
                    if (item.TranDate.Month == 1)
                        ListDayInMonth.Where(w => w.Day == DayIn.Day).ToList().ForEach(x => x.Jan = item.SHIFT);
                    else if (item.TranDate.Month == 2)
                        ListDayInMonth.Where(w => w.Day == DayIn.Day).ToList().ForEach(x => x.Feb = item.SHIFT);
                    else if (item.TranDate.Month == 3)
                        ListDayInMonth.Where(w => w.Day == DayIn.Day).ToList().ForEach(x => x.Mar = item.SHIFT);
                    else if (item.TranDate.Month == 4)
                        ListDayInMonth.Where(w => w.Day == DayIn.Day).ToList().ForEach(x => x.Apr = item.SHIFT);
                    else if (item.TranDate.Month == 5)
                        ListDayInMonth.Where(w => w.Day == DayIn.Day).ToList().ForEach(x => x.May = item.SHIFT);
                    else if (item.TranDate.Month == 6)
                        ListDayInMonth.Where(w => w.Day == DayIn.Day).ToList().ForEach(x => x.Jun = item.SHIFT);
                    else if (item.TranDate.Month == 7)
                        ListDayInMonth.Where(w => w.Day == DayIn.Day).ToList().ForEach(x => x.Jul = item.SHIFT);
                    else if (item.TranDate.Month == 8)
                        ListDayInMonth.Where(w => w.Day == DayIn.Day).ToList().ForEach(x => x.Aug = item.SHIFT);
                    else if (item.TranDate.Month == 9)
                        ListDayInMonth.Where(w => w.Day == DayIn.Day).ToList().ForEach(x => x.Sep = item.SHIFT);
                    else if (item.TranDate.Month == 10)
                        ListDayInMonth.Where(w => w.Day == DayIn.Day).ToList().ForEach(x => x.Oct = item.SHIFT);
                    else if (item.TranDate.Month == 11)
                        ListDayInMonth.Where(w => w.Day == DayIn.Day).ToList().ForEach(x => x.Nov = item.SHIFT);
                    else if (item.TranDate.Month == 12)
                        ListDayInMonth.Where(w => w.Day == DayIn.Day).ToList().ForEach(x => x.Dec = item.SHIFT);
                }
            }

            return ListDayInMonth;
        }
        public string CreateShift(List<ListMonth> _list)
        {
            var DBI = new HumicaDBContext();
            var DBD = new HumicaDBContext();
            try
            {
                try
                {
                    DBI.Configuration.AutoDetectChangesEnabled = false;
                    DBD.Configuration.AutoDetectChangesEnabled = false;

                    var _listATEmpSch = DB.ATEmpSchedules.Where(w => w.EmpCode == Filter.EmpCode &&
                    w.TranDate.Year == Filter.INYear).ToList();
                    _listATEmpSch = _listATEmpSch.ToList();
                    var listShift = DB.ATShifts.ToList();
                    var ListLeaveType = DB.HRLeaveTypes.ToList();
                    var listLeaveH = DB.HREmpLeaves.Where(w => w.EmpCode == Filter.EmpCode).ToList();
                    listLeaveH = listLeaveH.Where(w => w.Status == SYDocumentStatus.APPROVED.ToString()).ToList();
                    var _lstLeave = DB.HREmpLeaveDs.Where(w => w.EmpCode == Filter.EmpCode).ToList();
                    listShift = listShift.ToList();
                    ListLeaveType = ListLeaveType.ToList();
                    _lstLeave = _lstLeave.Where(x => listLeaveH.Where(w => w.EmpCode == x.EmpCode && w.Increment == x.LeaveTranNo).Any()).ToList();

                    DateTime CheckDate = new DateTime(1900, 1, 1);
                    foreach (var item in _list)
                    {
                        DateTime CheckIN1 = new DateTime(1900, 1, 1);
                        DateTime CheckOut1 = new DateTime(1900, 1, 1);
                        DateTime CheckIN2 = new DateTime(1900, 1, 1);
                        DateTime CheckOut2 = new DateTime(1900, 1, 1);
                        if (DateTime.DaysInMonth(Filter.INYear, item.ID) >= HeaderDayInMonth.Day)
                        {
                            DateTime MyDate = new DateTime(Filter.INYear, item.ID, HeaderDayInMonth.Day);
                            Temp_Roster temp_Roster = new Temp_Roster();
                            if (item.Months != null)
                            {
                                temp_Roster.Shift = item.Months.Trim().ToUpper();
                                temp_Roster.InDate = MyDate;
                                temp_Roster.EmpCode = Filter.EmpCode;
                                if (_listATEmpSch.Where(w => w.TranDate == MyDate && w.SHIFT == temp_Roster.Shift).Any()) continue;
                                var Att = getEmpSchedule(temp_Roster, listShift, ListLeaveType, listLeaveH,
                                    _lstLeave);

                                if (listShift.Where(w => w.Code == temp_Roster.Shift).Count() == 0)
                                {
                                    if (Att.SHIFT.ToUpper().Trim() != temp_Roster.Shift.ToUpper().Trim())
                                        continue;
                                }
                                if (_listATEmpSch.Where(w => w.EmpCode == Filter.EmpCode && w.TranDate == MyDate.Date).Any())
                                {
                                    var ObjMatch = _listATEmpSch.FirstOrDefault(w => w.EmpCode == Filter.EmpCode && w.TranDate == MyDate.Date);
                                    DBD.ATEmpSchedules.Attach(ObjMatch);
                                    DBD.Entry(ObjMatch).State = System.Data.Entity.EntityState.Deleted;
                                }
                                DBI.ATEmpSchedules.Add(Att);
                            }
                            else
                            {
                                if (_listATEmpSch.Where(w => w.EmpCode == Filter.EmpCode && w.TranDate == MyDate.Date).Any())
                                {
                                    var objAT = _listATEmpSch.FirstOrDefault(w => w.EmpCode == Filter.EmpCode && w.TranDate == MyDate.Date);
                                    DBD.ATEmpSchedules.Attach(objAT);
                                    DBD.Entry(objAT).State = System.Data.Entity.EntityState.Deleted;
                                }
                            }
                        }
                    }
                    //   int row1 = DBC.SaveChanges();
                    int row2 = DBD.SaveChanges();
                    int row = DBI.SaveChanges();
                    return SYConstant.OK;
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DBI.Configuration.AutoDetectChangesEnabled = true;
                DBD.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
        public string Set_ShiftDefault(DateTime FromDate, DateTime ToDate, List<HRBranch> ListBranch)
        {
            var DBD = new HumicaDBContext();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                DBD.Configuration.AutoDetectChangesEnabled = false;
                string Status_Error = "";
                try
                {
                    var ListStaff = DB.HRStaffProfiles.Where(w => (w.ROSTER != "" && w.ROSTER != null)).ToList();
                    ListStaff = ListStaff.Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
                    var ListEmpSch = DB.ATEmpSchedules.Where(w => EntityFunctions.TruncateTime(w.TranDate) >= FromDate.Date &&
                    EntityFunctions.TruncateTime(w.TranDate) <= ToDate.Date).ToList();
                    var ListPub = DB.HRPubHolidays.ToList();
                    var ListBatch = DB.ATBatches.ToList();
                    var ListBatchItem = DB.ATBatchItems.ToList();
                    ListBatchItem = ListBatchItem.Where(w => ListBatch.Where(x => x.Code == w.BatchNo).Any()).ToList();
                    var _listShift = DB.ATShifts.ToList();
                    ListPub = ListPub.ToList();
                    foreach (DateTime InDate in EachDay(FromDate, ToDate))
                    {
                        var _Shift = new ATShift();
                        var Staff = ListStaff.Where(w => w.StartDate.Value.Date <= InDate.Date &&
                        (w.TerminateStatus == "" || w.TerminateStatus == null || w.DateTerminate.Date > InDate.Date)
                        && (w.ROSTER != "" && w.ROSTER != null)).ToList();
                        //Staff = Staff.Where(w => !ListEmpSch.Where(x => x.EmpCode == w.EmpCode && x.TranDate.Date == InDate.Date).Any()).ToList();

                        foreach (var st in Staff)
                        {
                            if (ListEmpSch.Where(x => x.EmpCode == st.EmpCode && x.TranDate.Date == InDate.Date).ToList().Any())
                                continue;
                            //foreach (var item in ListEmpSch.Where(x => x.EmpCode == st.EmpCode && x.TranDate.Date == InDate.Date).ToList())
                            //{
                            //    DBD.ATEmpSchedules.Attach(item);
                            //    DBD.Entry(item).State = EntityState.Deleted;
                            //}
                            Status_Error = st.EmpCode + ":" + InDate.ToString();

                            DateTime CheckIN1 = new DateTime(1900, 1, 1);
                            DateTime CheckOut1 = new DateTime(1900, 1, 1);
                            DateTime CheckIN2 = new DateTime(1900, 1, 1);
                            DateTime CheckOut2 = new DateTime(1900, 1, 1);
                            DateTime CheckDate = new DateTime(1900, 1, 1);
                            string ShiftCode = "";

                            var att = new ATEmpSchedule();
                            att.Flag1 = 2;
                            att.Flag2 = 2;
                            bool Result = false;
                            if (ListPub.Where(w => w.PDate.Date == InDate.Date).Any()) { Result = true; ShiftCode = "PH"; }
                            var Batch = ListBatchItem.Where(w => w.BatchNo == st.ROSTER).ToList();
                            if (Batch.Count() > 0 && Result == false)
                            {
                                //Result = true;
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
                                    _Shift = _listShift.Where(w => w.Code == _Batch.ShiftCode).FirstOrDefault();
                                    if (_Shift == null)
                                        continue;
                                    else ShiftCode = _Shift.Code;
                                }
                                else
                                {
                                    ShiftCode = "OFF";
                                    Result = true;
                                }
                            }
                            if (ShiftCode == "")
                                continue;
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
                            // att.Flag2 = 0;

                            att.EmpCode = st.EmpCode;
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
                            att.CreateBy = "System";
                            att.CreateOn = DateTime.Now;
                            att.LeaveCode = "";
                            DB.ATEmpSchedules.Add(att);
                        }
                    }
                    DBD.SaveChanges();
                    int row = DB.SaveChanges();
                    return SYConstant.OK;
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Status_Error;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Status_Error;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Status_Error;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
                DBD.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string Set_Shift(DateTime FromDate, DateTime ToDate, string EmpCode, string Shift)
        {
            var DBD = new HumicaDBContext();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                DBD.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    var lstAttendance = DB.ATEmpSchedules.Where(w => w.TranDate >= FromDate.Date && w.TranDate <= ToDate.Date).ToList();
                    lstAttendance = lstAttendance.ToList();
                    var listShift = DB.ATShifts.ToList();
                    var ListLeaveType = DB.HRLeaveTypes.ToList();
                    var Approval = SYDocumentStatus.APPROVED.ToString();
                    var listLeaveH = DB.HREmpLeaves.Where(w => w.Status == Approval).ToList();
                    listLeaveH = listLeaveH.Where(w => ((w.FromDate.Date >= FromDate.Date && w.FromDate.Date <= ToDate.Date) ||
                      (w.ToDate.Date >= FromDate.Date && w.ToDate.Date <= ToDate.Date) ||
                            (FromDate.Date >= w.FromDate.Date && FromDate.Date <= w.ToDate.Date) || (ToDate.Date >= w.FromDate.Date && ToDate.Date <= w.ToDate.Date))).ToList();

                    var ListLeave = DB.HREmpLeaveDs.Where(w => EntityFunctions.TruncateTime(w.LeaveDate) >= FromDate.Date
           && EntityFunctions.TruncateTime(w.LeaveDate) <= ToDate.Date).ToList();
                    listShift = listShift.ToList();
                    ListLeaveType = ListLeaveType.ToList();
                    ListLeave = ListLeave.Where(x => listLeaveH.Where(w => w.EmpCode == x.EmpCode && w.Increment == x.LeaveTranNo).Any()).ToList();

                    string[] Emp = EmpCode.Split(';');
                    foreach (var Code in Emp)
                    {
                        if (Code.Trim() != "")
                        {
                            foreach (DateTime InDate in EachDay(FromDate, ToDate))
                            {
                                Temp_Roster temp_Roster = new Temp_Roster();
                                temp_Roster.EmpCode = Code;
                                temp_Roster.Shift = Shift;
                                temp_Roster.InDate = InDate;

                                lstAttendance = lstAttendance.Where(w => Code == w.EmpCode &&
                                      w.TranDate.Date == InDate.Date).ToList();

                                int Counts = 0;
                                foreach (var item in lstAttendance)
                                {
                                    Counts += 1;

                                    DBD.ATEmpSchedules.Attach(item);
                                    DBD.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                                }

                                var Att = new ATEmpSchedule();
                                Att = getEmpSchedule(temp_Roster, listShift, ListLeaveType, listLeaveH, ListLeave);
                                DB.ATEmpSchedules.Add(Att);
                            }
                        }
                    }

                    DBD.SaveChanges();
                    int row = DB.SaveChanges();
                    return SYConstant.OK;
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
                DBD.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public async Task<string> GenrateAttendance(string ID)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                Header = new ATEmpSchedule();
                ListHeader = new List<ATEmpSchedule>();
                var ListInOut = new List<ATInOut>();
                var ListSHift = new List<ATShift>();
                var listLeaveH = new List<HREmpLeave>();
                var ListLeaveD = new List<HREmpLeaveD>();
                string Approval = SYDocumentStatus.APPROVED.ToString();
                var _LeaveType = DB.HRLeaveTypes.ToList();
                List<HRStaffProfile> ListStaff = DB.HRStaffProfiles.ToList();
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
                // List<ATInOut> _listINOut = GetListInOutAsync(Attendance,ListStaff).Result.ToList();

                var _list = DB.ATEmpSchedules.Where(w =>
                EntityFunctions.TruncateTime(w.TranDate) >= Attendance.FromDate.Date && EntityFunctions.TruncateTime(w.TranDate) <= Attendance.ToDate.Date).ToList();
                _list = _list.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                //  var _listINOut = DB.ATInOuts.Where(w =>w.EmpCode.Trim()!="" && EntityFunctions.TruncateTime(w.FullDate) >= Attendance.FromDate.Date
                //&& EntityFunctions.TruncateTime(w.FullDate) <= Attendance.ToDate.Date).ToList();
                //  _listINOut = _listINOut.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                var FromDate = Attendance.FromDate.AddDays(-1);
                var ToDate = Attendance.ToDate.AddDays(1);
                var _listINOut = (from s in DB.ATInOuts
                                      //join M in ListStaff
                                      //on s.EmpCode equals M.EmpCode
                                  where EntityFunctions.TruncateTime(s.FullDate) >= FromDate.Date
                                  && EntityFunctions.TruncateTime(s.FullDate) <= ToDate.Date
                                  && s.EmpCode.Trim() != ""
                                  select s).ToList();


                var _listShift = DB.ATShifts.ToList();
                var _listLeaveh = DB.HREmpLeaves.Where(w => w.Status == Approval && w.TranType == false).ToList();
                _listLeaveh = _listLeaveh.Where(w => ((w.FromDate.Date >= FromDate.Date && w.FromDate.Date <= ToDate.Date) ||
                  (w.ToDate.Date >= FromDate.Date && w.ToDate.Date <= ToDate.Date) ||
                        (FromDate.Date >= w.FromDate.Date && FromDate.Date <= w.ToDate.Date) || (ToDate.Date >= w.FromDate.Date && ToDate.Date <= w.ToDate.Date))).ToList();

                var _listLeaveD = DB.HREmpLeaveDs.Where(w => EntityFunctions.TruncateTime(w.LeaveDate) >= FromDate.Date
            && EntityFunctions.TruncateTime(w.LeaveDate) <= ToDate.Date).ToList();
                var ListPub = DB.HRPubHolidays.ToList();
                var ListLa_Ea = DB.ATPolicyLeEas.ToList();
                ListInOut = _listINOut.ToList();
                ListHeader = _list.ToList();
                ListSHift = _listShift.ToList();
                ListSHift = ListSHift.ToList();
                listLeaveH = _listLeaveh.ToList();
                ListLeaveD = _listLeaveD.ToList();
                _LeaveType = _LeaveType.ToList();
                ListLeaveD = ListLeaveD.Where(w => listLeaveH.Where(x => x.Increment == w.LeaveTranNo && x.Status == Approval).Any()).ToList();
                ATPolicy NWPolicy = DB.ATPolicies.FirstOrDefault();
                string[] _TranNo = ID.Split(';');
                int i = 0;
                List<ATEmpSchedule> _listHeader = new List<ATEmpSchedule>();
                var listMaternity = DB.HRReqMaternities.ToList();
                var listEmpRework = DB.ATEmpRelWorks.Where(w => w.InDate >= Attendance.FromDate.Date && w.InDate <= Attendance.ToDate.Date).ToList();
                var listReqLateEarly = DB.HRReqLateEarlies.ToList();
                var ListPeriod = DB.ATPayperiods.Where(w => EntityFunctions.TruncateTime(w.FromDate) <= ToDate
                && EntityFunctions.TruncateTime(w.ToDate) >= FromDate).ToList();
                Progress = _TranNo.Count();
                decimal _p = 0;
                var ListPara = DB.PRParameters.ToList();
                ListProgress.Where(w => w.UserName == User.UserName).ToList().ForEach(x => x.Progress = Progress);

                var ListEmpOTReq = DB.HRRequestOTs.Where(w => w.Status == Approval && EntityFunctions.TruncateTime(w.OTEndTime) >= Attendance.FromDate.Date && EntityFunctions.TruncateTime(w.OTEndTime) <= Attendance.ToDate.Date).ToList();
                var ListOTPolic = await DB.ATOTPolicies.ToListAsync();
                DateTime FromDateMin = new DateTime(FromDate.Year, FromDate.Month, 1);
                DateTime ToDateMax = new DateTime(ToDate.Year, ToDate.Month, DateTime.DaysInMonth(ToDate.Year, ToDate.Month));
               
                var IS_PERIORD = SMS.SYSettings.FirstOrDefault(w => w.SettingName == "IS_PERIORD");
               
                if (ListPeriod.Count > 0)
                {
                    FromDateMin = ListPeriod.Min(w => w.FromDate);
                    ToDateMax = ListPeriod.Min(w => w.ToDate);
                }
                var ListEmpLateEarly = (from s in DB.ATLateEarlyDeducts
                                        where EntityFunctions.TruncateTime(s.DocumentDate) >= FromDateMin.Date
                                         && EntityFunctions.TruncateTime(s.DocumentDate) <= ToDateMax.Date
                                        select s).ToList();
                ListEmpLateEarly = ListEmpLateEarly.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                if (ListEmpLateEarly.Count() > 0)
                {
                    ListEmpLateEarly.ForEach(w => w.IsLate_Early = 0);
                    ListEmpLateEarly.ForEach(w => w.IsMissScan = 0);
                    ListEmpLateEarly = ListEmpLateEarly.OrderBy(w => w.DocumentDate).ToList();
                }
                List<HRStaffProfile> ListStaffRes = ListStaff.Where(w => w.Status=="I").ToList();
                var ListAtTRes = ListHeader.Where(w => ListStaffRes.Where(x => x.EmpCode == w.EmpCode && (w.TranDate.Date >= x.DateTerminate.Date || x.DateTerminate.Year==1900 )).Any()).ToList();
                foreach (var item in ListAtTRes)
                {
                    DB.ATEmpSchedules.Remove(item);
                }
                foreach (var TranNo in _TranNo)
                {

                    i += 1;
                    if (TranNo.Trim() != "")
                    {
                        int No = Convert.ToInt32(TranNo);

                        var _Empsch = ListHeader.Where(w => w.TranNo == No).ToList();
                        foreach (var item in _Empsch)
                        {
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
                            var _para = ListPara.FirstOrDefault(w => w.Code == Staff.PayParam);
                            var _Inout = ListInOut.Where(w => w.EmpCode == item.EmpCode).ToList();
                            var _EmpResMater = listMaternity.Where(w => w.EmpCode == item.EmpCode
                            && w.FromDate.Date <= item.TranDate.Date && w.ToDate.Date >= item.TranDate.Date).ToList();
                            var _EmpResLateEalry = listReqLateEarly.Where(w => w.EmpCode == item.EmpCode && w.LeaveDate.Date == item.TranDate.Date && w.Status == "APPROVED").ToList();
                            if (listEmpRework.Where(w => w.FromEmpCode == item.EmpCode && w.InDate.Date == item.TranDate.Date).Any())
                            {
                                var _EmpRework = listEmpRework.FirstOrDefault(w => w.FromEmpCode == item.EmpCode && w.InDate.Date == item.TranDate.Date);
                                _Inout = ListInOut.Where(w => w.EmpCode == _EmpRework.ToEmpCode).ToList();
                            }

                            Header.EmpCode = item.EmpCode;
                            ATEmpSchedule result = new ATEmpSchedule();
                            
                            item.OTStart = null;
                            item.OTEnd = null;
                            //if (Staff.IsAtten == true)
                            //{
                            //result = GetResult_Hour(item, ListSHift, _Inout);
                            //}
                            //else
                            //{
                            var EmpOTReq = ListEmpOTReq.Where(w => w.EmpCode == item.EmpCode && w.OTEndTime.Date == item.TranDate.Date).ToList();
                            bool IsOT = false;
                            if (EmpOTReq.Count() > 0)
                                IsOT = true;
                            result = await GetResult(item, ListSHift, _Inout, IsOT);
                            //}
                            DateTime NWFromDate = result.TranDate + NWPolicy.NWFROM.Value.TimeOfDay;
                            DateTime NWToDate = result.TranDate + NWPolicy.NWTO.Value.TimeOfDay;
                            NWToDate = NWToDate.AddDays(1);
                            result = Attendance_New(result);
                            
                            if (ListPub.Where(w => w.PDate.Date == result.TranDate.Date).Any())
                            {
                                result.Remark2 = "PH";
                            }
                            if (result.Flag1 == 1)
                            {
                                var _Shift = ListSHift.Where(w => w.Code == result.SHIFT).ToList();
                                if (ListLeaveD.Where(w => w.LeaveDate.Date == result.TranDate.Date && w.EmpCode == item.EmpCode).Any())
                                {
                                    var Leave = ListLeaveD.Where(w => w.LeaveDate == result.TranDate.Date && w.EmpCode == item.EmpCode).FirstOrDefault();
                                    if (Leave != null)
                                    {
                                        result.LeaveNo = Leave.LeaveTranNo;
                                        result.LeaveCode = Leave.LeaveCode;
                                        result.LeaveDesc = Leave.LeaveCode;
                                    }
                                }
                                var Atten = ValidateAttendance(result.LeaveCode, result.IN1.Value, result.OUT1.Value,
                                    result.Start1, result.End1, _Shift.FirstOrDefault(),
                                    NWFromDate, NWToDate, result.Flag1.Value, result.Flag2.Value);
                                result.LeaveDesc = Atten.Leave;
                                result.GEN = Atten.GEN;
                                result.Late1 = Atten.Late;
                                result.Early1 = Atten.Early;
                                result.WHOUR = Atten.WHour;
                                result.NWH = Atten.NWH;
                                result.WOT = Atten.WOT;
                                result.WOTMin = Atten.OTMin;
                            }
                            if (result.Flag2 == 1)
                            {
                                var _Shift = ListSHift.Where(w => w.Code == result.SHIFT).ToList();
                                var Atten = ValidateAttendance(result.LeaveCode, result.IN2.Value, result.OUT2.Value,
                                result.Start2, result.End2, _Shift.FirstOrDefault(),
                                NWFromDate, NWToDate, result.Flag1.Value, result.Flag2.Value);
                                result.LeaveDesc = Atten.Leave;
                                result.GEN = Atten.GEN;
                                result.Late2 = Atten.Late;
                                result.Early2 = Atten.Early;
                                result.WHOUR += Atten.WHour;
                                result.NWH = Atten.NWH;
                                result.WOT = Atten.WOT;
                                result.WOTMin = Atten.OTMin;
                            }
                            if (result.Flag1 == 2 && result.Flag2 == 2)
                            {
                                if (result.Start1.Value.Year != 1900 && result.End1.Value.Year != 1900)
                                {
                                    var interval = result.End1.Value.Subtract(result.Start1.Value).TotalHours;
                                    result.WHOUR = Math.Round(Convert.ToDecimal(interval), 2);
                                    result.WOT = result.WHOUR;
                                    result.GEN = true;
                                }
                            }
                           
                            if (_LeaveType.Where(w => w.Code == result.SHIFT).Any())
                            {
                                result.LeaveDesc = result.SHIFT;
                            }
                            if (result.LeaveDesc == "" || result.LeaveDesc == null)
                                result.LeaveDesc = item.LeaveDesc;

                            //NWPolicy
                            //bool IsLate = false;
                            //bool IsEarly = false;
                            decimal Late = 0;
                            string LeaveLa = "";
                            decimal Ealry = 0;
                            string LeaveEa = "";
                            int MissScanIN = 0;
                            int MissScanIN2 = 0;
                            int MissScanOUT = 0;
                            int MissScanOUT2 = 0;
                            int NumMissSCanIN = 0;
                            int NumMissSCanIN2 = 0;
                            int NumMissSCanOUT = 0;
                            int NumMissSCanOUT2 = 0;
                            ClsMissScan _MissScan = new ClsMissScan();
                            if (result.Start1.Value.Year != 1900)
                            {
                                if (result.Start1 > result.IN1) Late = (int)result.Start1.Value.Subtract(result.IN1.Value).TotalMinutes;
                            }
                            if (result.End1.Value.Year != 1900)
                            {
                                if (result.End1 < result.OUT1) Ealry = Math.Abs((int)result.End1.Value.Subtract(result.OUT1.Value).TotalMinutes);
                            }
                            if (result.Start2.Value.Year != 1900)
                            {
                                if (result.Start2 > result.IN2) Late += (int)result.Start2.Value.Subtract(result.IN2.Value).TotalMinutes;
                            }
                            if (result.End2.Value.Year != 1900)
                            {
                                if (result.End2 < result.OUT2) Ealry += Math.Abs((int)result.End2.Value.Subtract(result.OUT2.Value).TotalMinutes);
                            }
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
                                            if (_Shift.BreakEnd.HasValue)
                                            {
                                                if (result.Start1.Value.TimeOfDay < _Shift.BreakEnd.Value.TimeOfDay)
                                                {
                                                    result.Late1 = 0;
                                                    Late = 0;
                                                }
                                            }
                                        }
                                        IsMissScan(_para, _LeaveCode, item, _MissScan, Leave.Remark);
                                    }
                                    else if (Leave.Remark == "Afternoon")
                                    {
                                        LeaveEa = Leave.LeaveCode;
                                        if (_Shift != null && result.Early1 > 0)
                                        {
                                            if (_Shift.BreakStart.HasValue)
                                            {
                                                if (result.End1.Value.TimeOfDay > _Shift.BreakStart.Value.TimeOfDay)
                                                {
                                                    result.Early1 = 0;
                                                    Ealry = 0;
                                                }
                                            }
                                        }
                                        IsMissScan(_para, _LeaveCode, item, _MissScan, Leave.Remark);
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
                            int CountLa = ListEmpLateEarly.Where(w => w.DocumentDate.Date >= PFromDate.Date && w.DocumentDate.Date <= PToDate.Date
                            && w.EmpCode == item.EmpCode && w.DeductType == "LATE").Sum(x => x.IsLate_Early);
                            int CountEa = ListEmpLateEarly.Where(w => w.DocumentDate.Date >= PFromDate.Date && w.DocumentDate.Date <= PToDate.Date
                            && w.EmpCode == item.EmpCode && w.DeductType == "EARLY").Sum(x => x.IsLate_Early);
                            //if (CountLa < NWPolicy.IsLate_Early)
                            //{
                            //    IsLate = true;
                            //}
                            //if (CountEa < NWPolicy.IsLate_Early)
                            //{
                            //    IsEarly = true;
                            //}

                            int _late1 = 0;
                            int _Early1 = 0;
                            if (ListLa_Ea.Where(w => w.Branch == Staff.Branch).Any())
                            {
                                var tempLata = ListLa_Ea.Where(w => w.Branch == Staff.Branch).FirstOrDefault();
                                _late1 = tempLata.Late1;
                                _Early1 = tempLata.Early1;
                            }
                            //var _late1 = ListLa_Ea.Sum(w => w.Late);
                            //var _Early1 = ListLa_Ea.Sum(w => w.Early);
                            if (result.Late1.Value <= _late1)
                            {
                                Late = 0;
                                result.Late1 = 0;
                                result.Late2 = 0;
                            }
                            if (result.Early1.Value <= _Early1)
                            {
                                Ealry = 0;
                                result.Early1 = 0;
                                result.Early2 = 0;
                            }
                            if (_late1 >= result.Late1) result.Late1 = 0;
                            //else if (_late1 < result.Late1 && IsLate == true)
                            //{
                            //    result.Late1 = result.Late1 - _late1;
                            //}
                            if (_Early1 >= result.Early1) result.Early1 = 0;
                            //else if (_Early1 < result.Early1 && IsEarly == true)
                            //{
                            //    result.Early1 = result.Early1 - _late1;
                            //}
                            int _late2 = 0;
                            int _Early2 = 0;
                            if (ListLa_Ea.Where(w => w.Branch == Staff.Branch).Any())
                            {
                                var tempLata = ListLa_Ea.Where(w => w.Branch == Staff.Branch).FirstOrDefault();
                                _late2 = tempLata.Late2;
                                _Early2 = tempLata.Early2;
                            }
                            if (_late2 >= result.Late2) result.Late2 = 0;
                            //else if (_late2 < result.Late2 && IsLate == true)
                            //{
                            //    result.Late2 = result.Late2 - _late2;
                            //}
                            if (_Early2 >= result.Early2) result.Early2 = 0;
                            //else if (_Early2 < result.Early2 && IsEarly == true)
                            //{
                            //    result.Early2 = result.Early2 - _late2;
                            //}

                            if (IsPeriod == true)
                            {
                                var _empLaEa = ListEmpLateEarly.Where(w => w.EmpCode == item.EmpCode && w.DocumentDate.Date == item.TranDate.Date).ToList();
                                var _empLa = _empLaEa.Where(w => w.DeductType == "LATE" || w.DeductType == "EARLY").ToList();
                                foreach (var _Late in _empLa)
                                {
                                    DB.ATLateEarlyDeducts.Remove(_Late);
                                }
                                if (Late > 0)
                                {
                                    var Amount = result.Late1.Value + result.Late2.Value;
                                    var EmpLateEalry = new ATLateEarlyDeduct();
                                    //if (IsLate == true)
                                    EmpLateEalry.IsLate_Early = 1;
                                    InsertLateEarly(EmpLateEalry, "LATE", (int)Late, Amount, "", "", item, _MissScan);
                                    DB.ATLateEarlyDeducts.Add(EmpLateEalry);
                                    ListEmpLateEarly.Add(EmpLateEalry);
                                }
                                if (Ealry > 0)
                                {
                                    var Amount = result.Early1.Value + result.Early2.Value;
                                    var EmpLateEalry = new ATLateEarlyDeduct();
                                    //if (IsEarly == true)
                                    EmpLateEalry.IsLate_Early = 1;
                                    InsertLateEarly(EmpLateEalry, "EARLY", (int)Ealry, Amount, "", "", item, _MissScan);
                                    DB.ATLateEarlyDeducts.Add(EmpLateEalry);
                                    ListEmpLateEarly.Add(EmpLateEalry);
                                }
                                string Remark1 = "";
                                string RemarkOut = "";
                                string RemarkIN2 = "";
                                string RemarkOUT2 = "";
                                string StrMissScanIN = "";
                                string StrMissScanIN2 = "";
                                string StrMissScanOUT2 = "";
                                string StrMissScanOUT = "";
                                int C_MissScanIN = ListEmpLateEarly.Where(w => w.DocumentDate.Date >= PFromDate.Date && w.DocumentDate.Date <= PToDate.Date
                                && w.EmpCode == item.EmpCode && w.DeductType == "MissScan" && w.Remark == "MissScanIN").Sum(x => x.IsMissScan);

                                int C_MissScanOUT = ListEmpLateEarly.Where(w => w.DocumentDate.Date >= PFromDate.Date && w.DocumentDate.Date <= PToDate.Date
                                 && w.EmpCode == item.EmpCode && w.DeductType == "MissScan" && w.Remark == "MissScanOut").Sum(x => x.IsMissScan);
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
                                if (result.LeaveDesc == "ABS")
                                {
                                    _MissScan.IsMissIN = true;
                                    _MissScan.IsMissOut = true;
                                    _MissScan.IsMissIN2 = true;
                                    _MissScan.IsMissOut2 = true;
                                }
                                if (result.Flag1 == 1)
                                {
                                    if (result.Start1.Value.Year == 1900 && _MissScan.IsMissIN == false)
                                    {
                                        MissScanIN = 1;
                                        NumMissSCanIN = 1;
                                        StrMissScanIN = "MissScanIN";
                                        Remark1 = "MissScanIN1";
                                    }
                                    if (result.End1.Value.Year == 1900 && _MissScan.IsMissOut == false)
                                    {
                                        MissScanOUT = 1;
                                        NumMissSCanOUT = 1;
                                        StrMissScanOUT = "MissScanOut";
                                        RemarkOut = "MissScanOut1";
                                    }
                                }
                                if (result.Flag2 == 1)
                                {
                                    if (result.Start2.Value.Year == 1900 && _MissScan.IsMissIN2 == false)
                                    {
                                        MissScanIN2 = 1;
                                        NumMissSCanIN2 = 1;
                                        StrMissScanIN2 = "MissScanIN";
                                        RemarkIN2 = "MissScanIN2";
                                    }
                                    if (result.End2.Value.Year == 1900 && _MissScan.IsMissOut2 == false)
                                    {
                                        MissScanOUT2 = 1;
                                        NumMissSCanOUT2 = 1;
                                        StrMissScanOUT2 = "MissScanOut";
                                        RemarkOUT2 = "MissScanOut2";
                                    }
                                }

                                if (C_MissScanIN < NWPolicy.MissScan)
                                {
                                    NumMissSCanIN = 0;
                                    NumMissSCanIN2 = 0;
                                }
                                if (C_MissScanOUT < NWPolicy.MissScan)
                                {
                                    NumMissSCanOUT = 0;
                                    NumMissSCanOUT2 = 0;
                                }
                                var _empMissScanIN = _empLaEa.Where(w => w.DeductType == "MissScan" || w.DeductType == "ABS").ToList();
                                foreach (var ScanIn in _empMissScanIN)
                                {
                                    DB.ATLateEarlyDeducts.Remove(ScanIn);
                                }
                                if (result.Flag1 == 1)
                                {
                                    if ((MissScanIN > 0 && !string.IsNullOrEmpty(StrMissScanIN)) &&
                                       MissScanOUT > 0 && !string.IsNullOrEmpty(StrMissScanOUT))
                                    {
                                        var EmpLateEalry = new ATLateEarlyDeduct();
                                        InsertLateEarly(EmpLateEalry, "ABS", 0.5M, 0.5M, "ABS1", StrMissScanIN, item, _MissScan);
                                        DB.ATLateEarlyDeducts.Add(EmpLateEalry);
                                        ListEmpLateEarly.Add(EmpLateEalry);
                                    }
                                    else
                                    {
                                        if (MissScanIN > 0 && !string.IsNullOrEmpty(StrMissScanIN))
                                    {
                                        var EmpLateEalry = new ATLateEarlyDeduct();
                                        InsertLateEarly(EmpLateEalry, "MissScan", MissScanIN, NumMissSCanIN, Remark1, StrMissScanIN, item, _MissScan);
                                        DB.ATLateEarlyDeducts.Add(EmpLateEalry);
                                        ListEmpLateEarly.Add(EmpLateEalry);
                                    }
                                    if (MissScanOUT > 0 && !string.IsNullOrEmpty(StrMissScanOUT))
                                    {
                                        var EmpLateEalry = new ATLateEarlyDeduct();
                                        InsertLateEarly(EmpLateEalry, "MissScan", MissScanOUT, NumMissSCanOUT, RemarkOut, StrMissScanOUT, item, _MissScan);
                                        DB.ATLateEarlyDeducts.Add(EmpLateEalry);
                                        ListEmpLateEarly.Add(EmpLateEalry);
                                    }
                                }
                                }
                                if (result.Flag2 == 1)
                                {
                                    if ((MissScanIN2 > 0 && !string.IsNullOrEmpty(StrMissScanIN2)) &&
                                       MissScanOUT2 > 0 && !string.IsNullOrEmpty(StrMissScanOUT2))
                                    {
                                        var EmpLateEalry = new ATLateEarlyDeduct();
                                        InsertLateEarly(EmpLateEalry, "ABS", 0.5M, 0.5M, "ABS2", StrMissScanIN2, item, _MissScan);
                                        DB.ATLateEarlyDeducts.Add(EmpLateEalry);
                                        ListEmpLateEarly.Add(EmpLateEalry);
                                    }
                                    else
                                    {
                                        if (MissScanIN2 > 0 && !string.IsNullOrEmpty(StrMissScanIN2))
                                        {
                                            var EmpLateEalry = new ATLateEarlyDeduct();
                                            InsertLateEarly(EmpLateEalry, "MissScan", MissScanIN2, NumMissSCanIN2, RemarkIN2, StrMissScanIN2, item, _MissScan);
                                            DB.ATLateEarlyDeducts.Add(EmpLateEalry);
                                            ListEmpLateEarly.Add(EmpLateEalry);
                                        }
                                        if (MissScanOUT2 > 0 && !string.IsNullOrEmpty(StrMissScanOUT2))
                                        {
                                            var EmpLateEalry = new ATLateEarlyDeduct();
                                            InsertLateEarly(EmpLateEalry, "MissScan", MissScanOUT2, NumMissSCanOUT2, RemarkOUT2, StrMissScanOUT2, item, _MissScan);
                                            DB.ATLateEarlyDeducts.Add(EmpLateEalry);
                                            ListEmpLateEarly.Add(EmpLateEalry);
                                        }
                                    }
                                }
                            }
                            //Request Maternity Late/Early
                            if (_EmpResMater.Sum(x => x.LateEarly) > 0)
                            {
                                result = Cal_Maternity_LateEarly(result, _EmpResMater);
                            }
                            //"Req Late/Early"
                            if (_EmpResLateEalry.Sum(x => x.Qty) > 0)
                            {
                                result = Cal_Req_Late_Early(result, _EmpResLateEalry);
                            }

                            //Validate_OT
                            result = Cal_OT(result, EmpOTReq, ListOTPolic.ToList());
                            foreach (var _OT in EmpOTReq)
                            {
                                DB.HRRequestOTs.Attach(_OT);
                                DB.Entry(_OT).Property(w => w.OTActStart).IsModified = true;
                                DB.Entry(_OT).Property(w => w.OTActEnd).IsModified = true;
                                DB.Entry(_OT).Property(w => w.OTActual).IsModified = true;
                                DB.Entry(_OT).Property(w => w.ReferenceNo).IsModified = true;
                            }

                            if (result.OTApproval < 0) result.OTApproval = 0;
                            //if (result.Start1.Value.Year == 1900 && result.End1.Value.Year == 1900
                            //    && result.Start2.Value.Year == 1900 && result.End2.Value.Year == 1900
                            //    && (result.LeaveCode == "" || result.LeaveCode == null || result.LeaveCode == "ABS") && (result.LeaveDesc == "" || result.LeaveDesc == null))
                            //{
                            //    result.LeaveDesc = "ABS";
                            //}
                            //else if(result.Start1.Value.Year != 1900 || result.End1.Value.Year != 1900
                            //    || result.Start2.Value.Year != 1900 || result.End2.Value.Year != 1900)
                            //{
                            //    result.LeaveDesc = "";
                            //}
                            //if (result.LeaveDesc == "ABS")
                            //{
                            //    if (result.SHIFT == "OFF" || result.SHIFT == "PH") result.LeaveDesc = "";
                            //}
                            if (result.WHOUR > 0)
                            {
                                result.LeaveDesc = "";
                                if (result.TranDate.DayOfWeek == DayOfWeek.Saturday && _para.WDSAT == true && _para.WDSATDay == 0.5M)
                                {
                                    result.ActWHour = (_para.WHOUR / 2) - (Convert.ToDecimal(result.Late1 + result.Late2 + result.Early1 + result.Early2) / 60.00M);
                                }
                                else
                                    result.ActWHour = _para.WHOUR - (Convert.ToDecimal(result.Late1 + result.Late2 + result.Early1 + result.Early2) / 60.00M);
                                result.WokingHour = "";
                                string[] Hour = item.WHOUR.ToString().Split('.');
                                if (Convert.ToInt32(Hour[0]) > 0)
                                    result.WokingHour = Hour[0] + "h";
                                if (Hour.Length > 1 && Convert.ToInt32(Hour[1]) > 0)
                                    result.WokingHour += Convert.ToInt32((result.WHOUR - Convert.ToDecimal(Hour[0])) * 60) + "min";
                            }
                            result.ChangedBy = User.UserName;
                            result.ChangedOn = DateTime.Now;
                            DB.ATEmpSchedules.Attach(result);
                            DB.Entry(result).Property(w => w.GEN).IsModified = true;
                            DB.Entry(result).Property(w => w.ActWHour).IsModified = true;
                            DB.Entry(result).Property(w => w.Meal).IsModified = true;
                            DB.Entry(result).Property(w => w.Start1).IsModified = true;
                            DB.Entry(result).Property(w => w.End1).IsModified = true;
                            DB.Entry(result).Property(w => w.Late1).IsModified = true;
                            DB.Entry(result).Property(w => w.Early1).IsModified = true;
                            DB.Entry(result).Property(w => w.Start2).IsModified = true;
                            DB.Entry(result).Property(w => w.End2).IsModified = true;
                            DB.Entry(result).Property(w => w.Late2).IsModified = true;
                            DB.Entry(result).Property(w => w.Early2).IsModified = true;
                            DB.Entry(result).Property(w => w.WokingHour).IsModified = true;
                            DB.Entry(result).Property(w => w.WHOUR).IsModified = true;
                            DB.Entry(result).Property(w => w.WOT).IsModified = true;
                            DB.Entry(result).Property(w => w.OTTYPE).IsModified = true;
                            DB.Entry(result).Property(w => w.OTApproval).IsModified = true;
                            DB.Entry(result).Property(w => w.OTRequest).IsModified = true;
                            DB.Entry(result).Property(w => w.LeaveNo).IsModified = true;
                            DB.Entry(result).Property(w => w.LeaveCode).IsModified = true;
                            DB.Entry(result).Property(w => w.BreakFast).IsModified = true;
                            DB.Entry(result).Property(w => w.Lunch).IsModified = true;
                            DB.Entry(result).Property(w => w.Dinner).IsModified = true;
                            DB.Entry(result).Property(w => w.NightMeal).IsModified = true;
                            DB.Entry(result).Property(w => w.LeaveDesc).IsModified = true;
                            DB.Entry(result).Property(w => w.WOTMin).IsModified = true;
                            DB.Entry(result).Property(w => w.NWH).IsModified = true;
                            DB.Entry(result).Property(w => w.OTStart).IsModified = true;
                            DB.Entry(result).Property(w => w.OTEnd).IsModified = true;
                            DB.Entry(result).Property(w => w.OTReason).IsModified = true;
                            DB.Entry(result).Property(w => w.ChangedBy).IsModified = true;
                            DB.Entry(result).Property(w => w.ChangedOn).IsModified = true;
                        }
                    }
                    _p = i;
                    ListProgress.Where(w => w.UserName == User.UserName).ToList().ForEach(x => x.Percen = _p / x.Progress * 100);
                }
                int row = DB.SaveChanges();
                return SYConstant.OK;

            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }

            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public async Task<ATEmpSchedule> GetResult(ATEmpSchedule emp_sch, List<ATShift> ListShift, List<ATInOut> raws, bool IsOT)
        {
            ListShift = ListShift.ToList();
            DateTime Checkdate = new DateTime(1900, 1, 1);
            var attendancexTemp = new ATEmpSchedule();
            attendancexTemp = new ATEmpSchedule()
            {
                Start1 = Checkdate,
                End1 = Checkdate,
                Start2 = Checkdate,
                End2 = Checkdate,
            };
            if (emp_sch.SHIFT.ToUpper() == "PH" || emp_sch.SHIFT.ToUpper() == "OFF")
            {
                DateTime TranDate = emp_sch.TranDate;
                DateTime P1 = new DateTime(TranDate.Year, TranDate.Month, TranDate.Day, 0, 0, 0);
                DateTime P2 = new DateTime(TranDate.Year, TranDate.Month, TranDate.Day, 23, 59, 0);
                var r = raws.Where(x => x.FullDate >= P1 && x.FullDate <= P2).ToList().OrderBy(x => x.FullDate);
                if (r.ToList().Count > 0)
                {
                    attendancexTemp.Start1 = r.First().FullDate;
                    if (r.ToList().Count > 1) attendancexTemp.End1 = r.LastOrDefault().FullDate;
                }
                emp_sch.Start1 = attendancexTemp.Start1;
                emp_sch.End1 = attendancexTemp.End1;
                if (IsOT == true)
                {
                    emp_sch.OTStart = attendancexTemp.Start1;
                    emp_sch.OTEnd = attendancexTemp.End1;
                }
            }
            else
            {
                var _shift = ListShift.Where(w => w.Code == emp_sch.SHIFT);
                foreach (var shift in _shift)
                {
                    DateTime P1 = new DateTime();
                    DateTime P2 = new DateTime();
                    DateTime P3 = new DateTime();
                    DateTime P4 = new DateTime();
                    DateTime P5 = new DateTime();
                    DateTime P6 = new DateTime();
                    DateTime P7 = new DateTime();
                    DateTime P8 = new DateTime();
                    attendancexTemp = emp_sch;
                    if (emp_sch.Flag1 == 1 && emp_sch.Flag2 == 1)
                    {
                        if (shift.CheckInBefore1.HasValue && shift.CheckInAfter1.HasValue && shift.CheckOutBefore1.HasValue && shift.CheckOutAfter1.HasValue
                            && shift.CheckInBefore2.HasValue && shift.CheckInAfter2.HasValue && shift.CheckOutBefore2.HasValue && shift.CheckOutAfter2.HasValue)
                        {
                            P1 = emp_sch.IN1.Value.Date + shift.CheckInBefore1.Value.TimeOfDay;
                            P2 = emp_sch.IN1.Value.Date + shift.CheckInAfter1.Value.TimeOfDay;
                            P3 = emp_sch.OUT1.Value.Date + shift.CheckOutBefore1.Value.TimeOfDay;
                            P4 = emp_sch.OUT1.Value.Date + shift.CheckOutAfter1.Value.TimeOfDay;
                            P5 = emp_sch.IN2.Value.Date + shift.CheckInBefore2.Value.TimeOfDay;
                            P6 = emp_sch.IN2.Value.Date + shift.CheckInAfter2.Value.TimeOfDay;
                            P7 = emp_sch.OUT2.Value.Date + shift.CheckOutBefore2.Value.TimeOfDay;
                            P8 = emp_sch.OUT2.Value.Date + shift.CheckOutAfter2.Value.TimeOfDay;

                        }
                        else
                        {
                            P1 = emp_sch.IN1.Value.AddMinutes(-300);
                            int Min2 = Convert.ToInt32(emp_sch.OUT1.Value.Subtract(emp_sch.IN1.Value).TotalMinutes / 2);
                            int Min3 = Convert.ToInt32(emp_sch.IN2.Value.Subtract(emp_sch.OUT1.Value).TotalMinutes / 2);
                            int Min4 = Convert.ToInt32(emp_sch.OUT2.Value.Subtract(emp_sch.IN2.Value).TotalMinutes / 2);
                            P2 = emp_sch.IN1.Value.AddMinutes(Min2);
                            P3 = P2;
                            P4 = emp_sch.OUT1.Value.AddMinutes(Min3);

                            P5 = emp_sch.OUT1.Value.AddMinutes(Min3);
                            P6 = emp_sch.IN2.Value.AddMinutes(Min4);
                            P7 = P6;
                            P8 = emp_sch.OUT2.Value.AddMinutes(300);
                        }
                    }
                    else
                    {
                        if (shift.CheckInBefore1.HasValue && shift.CheckInAfter1.HasValue
                            && shift.CheckOutBefore1.HasValue && shift.CheckOutAfter1.HasValue)
                        {
                            P1 = emp_sch.IN1.Value.Date + shift.CheckInBefore1.Value.TimeOfDay;
                            P2 = emp_sch.IN1.Value.Date + shift.CheckInAfter1.Value.TimeOfDay;
                            P3 = emp_sch.OUT1.Value.Date + shift.CheckOutBefore1.Value.TimeOfDay;
                            P4 = emp_sch.OUT1.Value.Date + shift.CheckOutAfter1.Value.TimeOfDay;
                        }
                        else
                        {
                            P1 = emp_sch.IN1.Value.AddMinutes(-300);
                            P4 = emp_sch.OUT1.Value.AddMinutes(300);
                            int TotalMin = Convert.ToInt32(emp_sch.OUT1.Value.Subtract(emp_sch.IN1.Value).TotalMinutes / 2);
                            P2 = emp_sch.IN1.Value.AddMinutes(TotalMin);
                            P3 = P2;
                        }
                    }
                    if (emp_sch.Flag1 == 1)
                    {
                        var r = raws.Where(x => x.FullDate >= P1 && x.FullDate <= P4 && x.LCK != 1).ToList();
                        attendancexTemp.Start1 = await Check_Scan_Attendance(r, P1, P2);
                        if (r.Where(w => w.FullDate >= P3 && w.FullDate <= P4 && w.LCK != 1).Any())
                        {
                            ATInOut _INOut = new ATInOut();
                            if (IsOT == true)
                            {
                                attendancexTemp.End1 = await Check_Scan_Attendance(r, P3, P4);
                                attendancexTemp.OTStart = await Check_Scan_OT(r, P3, P4);
                                attendancexTemp.OTEnd = await Check_Scan_OT(r, P3, P4, false);
                            }
                            else
                            {
                                if (emp_sch.Flag2 != 1)
                                    attendancexTemp.End1 = await Check_Scan_Attendance(r, P3, P4, false);
                                else
                                    attendancexTemp.End1 = await Check_Scan_Attendance(r, P3, P4);
                            }
                        }
                        else
                            attendancexTemp.End1 = new DateTime(1900, 1, 1);
                    }
                    if (emp_sch.Flag2 == 1)
                    {
                        var r = raws.Where(x => x.FullDate >= P5 && x.FullDate <= P8 && x.LCK != 1).ToList();
                        attendancexTemp.Start2 = await Check_Scan_Attendance(r, P5, P6);
                        if (r.Where(w => w.FullDate >= P7 && w.FullDate <= P8 && w.LCK != 1).Any())
                        {
                            if (IsOT == true)
                            {
                                attendancexTemp.End2 = await Check_Scan_Attendance(r, P7, P8);
                                attendancexTemp.OTStart = await Check_Scan_OT(r, P7, P8);
                                attendancexTemp.OTEnd = await Check_Scan_OT(r, P7, P8, false);
                            }
                            else
                            {
                                attendancexTemp.End2 = await Check_Scan_Attendance(r, P7, P8, false);
                            }
                        }
                        else
                            attendancexTemp.End2 = new DateTime(1900, 1, 1);
                    }
                }
            }
            return emp_sch;
        }
        public ATEmpSchedule GetResult_Hour(ATEmpSchedule emp_sch, List<ATShift> ListShift, List<ATInOut> raws)
        {
            ListShift = ListShift.ToList();
            DateTime Checkdate = new DateTime(1900, 1, 1);
            var attendancexTemp = new ATEmpSchedule();
            attendancexTemp = new ATEmpSchedule()
            {
                Start1 = Checkdate,
                End1 = Checkdate,
                Start2 = Checkdate,
                End2 = Checkdate,
            };
            var _shift = ListShift.Where(w => w.Code == emp_sch.SHIFT);
            if (_shift.Count() > 0)
            {
                foreach (var shift in _shift)
                {
                    DateTime tmpInBefore = new DateTime();
                    DateTime tmpOutAfter = new DateTime();
                    attendancexTemp = emp_sch;
                    DateTime P1 = new DateTime();
                    DateTime P2 = new DateTime();
                    DateTime P3 = new DateTime();
                    DateTime P4 = new DateTime();
                    DateTime P5 = new DateTime();
                    if (emp_sch.Flag1 == 1 && emp_sch.Flag2 != 1)
                    {
                        tmpInBefore = emp_sch.IN1.Value.AddMinutes(-300);
                        tmpOutAfter = emp_sch.OUT1.Value.AddMinutes(300);
                    }
                    if (emp_sch.Flag1 == 1 && emp_sch.Flag2 == 1)
                    {
                        P1 = emp_sch.IN1.Value.AddMinutes(-300);
                        int Min2 = Convert.ToInt32(emp_sch.OUT1.Value.Subtract(emp_sch.IN1.Value).TotalMinutes / 2);
                        int Min3 = Convert.ToInt32(emp_sch.IN2.Value.Subtract(emp_sch.OUT1.Value).TotalMinutes / 2);
                        int Min4 = Convert.ToInt32(emp_sch.OUT2.Value.Subtract(emp_sch.IN2.Value).TotalMinutes / 2);
                        P2 = emp_sch.IN1.Value.AddMinutes(Min2);
                        P3 = emp_sch.OUT1.Value.AddMinutes(Min3);
                        P4 = emp_sch.IN2.Value.AddMinutes(Min4);
                        P5 = emp_sch.OUT2.Value.AddMinutes(300);
                    }
                    else
                    {
                        P1 = emp_sch.IN1.Value.AddMinutes(-300);
                        P3 = emp_sch.OUT1.Value.AddMinutes(300);
                        int TotalMin = Convert.ToInt32(emp_sch.OUT1.Value.Subtract(emp_sch.IN1.Value).TotalMinutes / 2);
                        P2 = emp_sch.IN1.Value.AddMinutes(TotalMin);
                    }
                    //var tmpInBefore = emp_sch.IN1.Value + shift.CheckIn1.Value.TimeOfDay;
                    //tmpInBefore = tmpInBefore.AddMinutes(-300);
                    //var tmpOutAfter = emp_sch.OUT1.Value + shift.CheckOut1.Value.TimeOfDay;
                    //tmpOutAfter = tmpOutAfter.AddMinutes(500);

                    var r = raws.Where(x => x.EmpCode == emp_sch.EmpCode && x.FullDate >= tmpInBefore && x.FullDate <= tmpOutAfter).ToList();
                    if (r.ToList().Count > 0)
                    {
                        attendancexTemp.Start1 = r.First().FullDate;
                        if (r.Where(w => w.FullDate != attendancexTemp.Start1).ToList().Count > 0)
                            attendancexTemp.End1 = r.LastOrDefault().FullDate;
                        else attendancexTemp.End1 = new DateTime(1900, 1, 1);
                    }
                    else
                    {
                        attendancexTemp.Start1 = new DateTime(1900, 1, 1);
                        attendancexTemp.End1 = new DateTime(1900, 1, 1);
                    }
                    emp_sch.Start1 = attendancexTemp.Start1;
                    emp_sch.End1 = attendancexTemp.End1;
                }
            }
            else
            //if (emp_sch.SHIFT.ToUpper() == "PH" || emp_sch.SHIFT.ToUpper() == "OFF")
            {
                DateTime TranDate = emp_sch.TranDate;
                DateTime P1 = new DateTime(TranDate.Year, TranDate.Month, TranDate.Day, 0, 0, 0);
                DateTime P2 = new DateTime(TranDate.Year, TranDate.Month, TranDate.Day, 23, 59, 0);
                var r = raws.Where(x => x.EmpCode == emp_sch.EmpCode && x.FullDate >= P1 && x.FullDate <= P2).ToList().OrderBy(x => x.FullDate).ToList();
                if (r.ToList().Count > 0)
                {
                    attendancexTemp.Start1 = r.First().FullDate;
                    if (r.ToList().Count > 1) attendancexTemp.End1 = r.LastOrDefault().FullDate;
                }
                emp_sch.Start1 = attendancexTemp.Start1;
                emp_sch.End1 = attendancexTemp.End1;
            }

            return emp_sch;
        }
        public List<ListEmpSch> LoadEmpSch(DateTime InMonth, List<HRBranch> _listBranch)
        {
            
            int targetYear = InMonth.Year;
            int targetMonth = InMonth.Month;

            var branchCodes = _listBranch.Select(x => x.Code).ToList();

            var d = (
                from f in DBV.ATEmpSchedules
                join emp in DBV.HR_STAFF_VIEW
                    on f.EmpCode equals emp.EmpCode
                where f.TranDate.Year == targetYear
                      && f.TranDate.Month == targetMonth
                      && (branchCodes.Contains(emp.BranchID)) // Filter by Branch IDs
                      && (string.IsNullOrEmpty(FMonthly.Branch) || emp.BranchID == FMonthly.Branch) // Filter by Branch
                      && (string.IsNullOrEmpty(FMonthly.Division) || emp.Division == FMonthly.Division) // Filter by Division
                      && (string.IsNullOrEmpty(FMonthly.Department) || emp.DEPT == FMonthly.Department) // Filter by Department
                      && (string.IsNullOrEmpty(FMonthly.Location) || emp.LOCT == FMonthly.Location) // Filter by Location
                group f by new { f.EmpCode, f.TranDate.Year, f.TranDate.Month, emp.AllName, emp.Position } into myGroup
                where myGroup.Count() > 0
                select new
                {
                    myGroup.Key.EmpCode,
                    myGroup.Key.AllName,
                    myGroup.Key.Position,
                    ListShift = myGroup.GroupBy(f => f.TranDate.Day)
                                       .Select(m => new
                                       {
                                           day = m.Key,
                                           Shift = m.Select(w => w.SHIFT.ToString()).FirstOrDefault()
                                       })
                }
            ).ToList();


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
        public List<ListEmpSch> LoadEmpImport(string DocumentNo)
        {
            var EmpSch = DB.ATImpRosters.Where(w => w.DocumentNo == DocumentNo).ToList();
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var d = (from f in EmpSch
                     join emp in Staff on f.EmpCode equals emp.EmpCode
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
        public List<ListEmpSch> LoadEmpSch(DateTime InMonth, List<HRBranch> _listBranch, List<SYAccessDepartment> _lstAccDept)
        {
            // LIstEmplSch = new List<ListEmpSch>();
            var EmpSch = DB.ATEmpSchedules.ToList();
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            Staff = Staff.Where(w => _listBranch.Where(x => x.Code == w.BranchID).Any()).ToList();
            EmpSch = EmpSch.Where(w => w.TranDate.Year == InMonth.Year && w.TranDate.Month == InMonth.Month).ToList();
            Staff = Staff.Where(w => _lstAccDept.Where(x => x.DeptCode == w.LOCT).Any()).ToList();
            if (FMonthly.Branch != null)
                Staff = Staff.Where(w => w.BranchID == FMonthly.Branch).ToList();
            if (FMonthly.Division != null)
                Staff = Staff.Where(w => w.Division == FMonthly.Division).ToList();
            if (FMonthly.Department != null)
                Staff = Staff.Where(w => w.DEPT == FMonthly.Department).ToList();
            var d = (from f in EmpSch
                     join emp in Staff on f.EmpCode equals emp.EmpCode
                     group f by new { f.EmpCode, f.TranDate.Year, f.TranDate.Month, emp.AllName, emp.Position }
                         into myGroup
                     where myGroup.Count() > 0
                     select new
                     {
                         myGroup.Key.EmpCode,
                         myGroup.Key.AllName,
                         myGroup.Key.Position,
                         ListShift = myGroup.GroupBy(f => f.TranDate.Day).Select
                         (m => new { day = m.Key, Shift = m.Select(w => w.SHIFT.ToString()).First() })
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
        public List<ClsSumLaEa> LoadDataLateEarly(FTMonthlySum _Filter)
        {
            ListSumLaEa = new List<ClsSumLaEa>();
            var ListEmpSchdule = DBV.VIEW_ATEmpSchedule.Where(w => DbFunctions.TruncateTime(w.TranDate) >= _Filter.FromDate.Date
            && DbFunctions.TruncateTime(w.TranDate) <= _Filter.ToDate.Date).ToList();
            var listBranch = SYConstant.getBranchDataAccess();
            ListEmpSchdule = ListEmpSchdule.Where(w => listBranch.Where(x => x.Code == w.Branch).Any()).ToList();

            if (_Filter.Branch != null)
                ListEmpSchdule = ListEmpSchdule.Where(w => w.Branch == _Filter.Branch).ToList();
            if (_Filter.Department != null)
                ListEmpSchdule = ListEmpSchdule.Where(w => w.DEPT == _Filter.Department).ToList();
            if (_Filter.Location != null)
                ListEmpSchdule = ListEmpSchdule.Where(w => w.LOCT == _Filter.Location).ToList();
            if (_Filter.Division != null)
                ListEmpSchdule = ListEmpSchdule.Where(w => w.DivisionCode == _Filter.Division).ToList();
            if (_Filter.EmpCode != null)
                ListEmpSchdule = ListEmpSchdule.Where(w => w.EmpCode == _Filter.EmpCode).ToList();
            var _empMonthlySum = ListEmpSchdule.GroupBy(w => new { w.EmpCode, w.AllName, w.Department, w.LevelCode, w.Position, w.StartDate }).
                Select(x => new ClsSumLaEa
                {
                    EmpCode = x.Key.EmpCode,
                    AllName = x.Key.AllName,
                    Department = x.Key.Department,
                    LevelCode = x.Key.LevelCode,
                    Position = x.Key.Position,
                    StartDate = x.Key.StartDate,
                    WHOUR = x.Sum(s => s.WHOUR),
                    Late = x.Sum(s => s.Late1) + x.Sum(s => s.Late2),
                    Early = x.Sum(s => s.Early1) + x.Sum(s => s.Early2),
                    CountLate = x.Where(s => s.Late1 > 0).Count() + x.Where(s => s.Late2 > 0).Count(),
                    CountEarly = x.Where(s => s.Early1 > 0).Count() + x.Where(s => s.Early2 > 0).Count(),
                    NWH = x.Sum(s => s.NWH),
                    WOT = x.Sum(s => s.WOT),
                    MEAL = x.Sum(s => s.MEAL)
                }).ToList();
            _empMonthlySum.ToList().ForEach(w => w.Total = w.Late.Value + w.Early.Value);
            _empMonthlySum = _empMonthlySum.Where(w => w.Total > 0).ToList();
            PRGenerate_Salary Gen = new PRGenerate_Salary();
            var ListPara = DB.PRParameters.ToList();
            var ListStaff = DB.HRStaffProfiles.ToList();
            DateTime ToDate = _Filter.ToDate;
            DateTime FromDate = DateTimeHelper.StartDateOfMonth(ToDate);
            var ListAttMisscan = DB.ATLateEarlyDeducts.ToList();
            foreach (var Item in _empMonthlySum)
            {
                var CountMiss= ListAttMisscan.Where(w => w.LeaveCode == null && w.IsMissScan == 0 && w.IsTransfer == false &&
                w.EmpCode == Item.EmpCode && w.DeductType == "MissScan").ToList();
                var Staff = ListStaff.FirstOrDefault(w => w.EmpCode == Item.EmpCode);
                var Parameter = ListPara.FirstOrDefault(w => w.Code == Staff.PayParam);
                DateTime FFromDate = DateTimeHelper.StartDateOfMonth(ToDate);
                DateTime FToDate = DateTimeHelper.EndDateOfMonth(ToDate);
                if (!Parameter.IsPrevoiuseMonth.IsNullOrZero())
                {
                    DateTime tempDate = FFromDate.AddMonths(-1);
                    FromDate = new DateTime(tempDate.Year, tempDate.Month, Parameter.FROMDATE.Value().Day);
                    ToDate = new DateTime(ToDate.Year, ToDate.Month, Parameter.TODATE.Value().Day);
                }
                decimal WorkDay = Gen.Get_WorkingDay_Ded(Parameter, FromDate, ToDate);
                decimal Hours = ClsRounding.Rounding(Item.Total / 60.00M, SYConstant.DECIMAL_PLACE, "N");
                decimal Rate = (Staff.Salary / WorkDay);
                decimal RateH = ClsRounding.Rounding(Convert.ToDecimal(Rate / Parameter.WHOUR), SYConstant.DECIMAL_PLACE, "N");
                decimal Amount = (Hours * RateH);
                Item.Deduct = Amount;
                Item.CountMissedScan = CountMiss.Count();
                ListSumLaEa.Add(Item);
            }
            return ListSumLaEa;
        }
		
		#region "Upload Shift" 
		public string uploadRoster(List<Temp_Roster> lsttemp_Rosters, DateTime _Period)
        {
            var DBD = new HumicaDBContext();
            var DBI = new HumicaDBContext();
            try
            {
                if (lsttemp_Rosters.Count == 0)
                {
                    return "NO_DATA";
                }
                var date = DateTime.Now;
                try
                {
                    DBI.Configuration.AutoDetectChangesEnabled = false;
                    DBD.Configuration.AutoDetectChangesEnabled = false;
                    Header = new ATEmpSchedule();
                    DateTime FromDate = new DateTime(_Period.Year, _Period.Month, 1);
                    DateTime ToDate = new DateTime(_Period.Year, _Period.Month, DateTime.DaysInMonth(_Period.Year, _Period.Month));

                    var lstAttendance = DB.ATEmpSchedules.Where(w => w.TranDate.Year == _Period.Year
                    && w.TranDate.Month == _Period.Month).ToList();
                    lstAttendance = lstAttendance.ToList();
                    var listShift = DB.ATShifts.ToList();
                    var ListLeaveType = DB.HRLeaveTypes.ToList();
                    var Approval = SYDocumentStatus.APPROVED.ToString();
                    var listLeaveH = DB.HREmpLeaves.Where(w => w.Status == Approval).ToList();
                    listLeaveH = listLeaveH.Where(w => ((w.FromDate.Date >= FromDate.Date && w.FromDate.Date <= ToDate.Date) ||
                      (w.ToDate.Date >= FromDate.Date && w.ToDate.Date <= ToDate.Date) ||
                            (FromDate.Date >= w.FromDate.Date && FromDate.Date <= w.ToDate.Date) || (ToDate.Date >= w.FromDate.Date && ToDate.Date <= w.ToDate.Date))).ToList();

                    var ListLeave = DB.HREmpLeaveDs.Where(w => w.LeaveDate.Year == _Period.Year && w.LeaveDate.Month == _Period.Month).ToList();
                    listShift = listShift.ToList();
                    ListLeaveType = ListLeaveType.ToList();
                    ListLeave = ListLeave.Where(x => listLeaveH.Where(w => w.EmpCode == x.EmpCode && w.Increment == x.LeaveTranNo).Any()).ToList();
                    lstAttendance = lstAttendance.Where(w => lsttemp_Rosters.Where(x => x.EmpCode == w.EmpCode &&
                                       w.TranDate.Date == x.InDate.Date && x.Shift != "").Any()).ToList();

                    int Counts = 0;
                    foreach (var item in lstAttendance)
                    {
                        Counts += 1;
                        DBD.ATEmpSchedules.Attach(item);
                        DBD.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }
                    Counts = 0;
                    foreach (var item in lsttemp_Rosters)
                    {
                        var _listLeaveH = listLeaveH.Where(w => w.EmpCode == item.EmpCode).ToList();
                        var _ListLeave = ListLeave.Where(w => w.EmpCode == item.EmpCode && w.LeaveDate.Date == item.InDate.Date).ToList();
                        Header.EmpCode = item.EmpCode;
                        Counts += 1;
                        if (item.Shift.Trim() == "") continue;
                        var Att = getEmpSchedule(item, listShift, ListLeaveType, _listLeaveH, _ListLeave);
                        DBI.ATEmpSchedules.Add(Att);
                    }
                    int row1 = DBD.SaveChanges();
                    int row = DBI.SaveChanges();
                    return SYConstant.OK;

                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DBI.Configuration.AutoDetectChangesEnabled = true;
                DBD.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string ImportdRosterTem(List<Temp_Roster> lsttemp_Rosters, string DocumentNo, MDUploadTemplate _Template)
        {
            var DBI = new HumicaDBContext();
            try
            {
                if (lsttemp_Rosters.Count == 0)
                {
                    return "NO_DATA";
                }
                var date = DateTime.Now;
                try
                {
                    DBI.Configuration.AutoDetectChangesEnabled = false;
                    Header = new ATEmpSchedule();
                    var HeaderImport = new ATImpRosterHeader();
                    HeaderImport.DocumentNo = DocumentNo;
                    HeaderImport.UploadBy = _Template.UploadBy;
                    HeaderImport.UploadByName = _Template.UploadByName;
                    HeaderImport.UploadName = _Template.UploadName;
                    HeaderImport.InMonth = Period;
                    HeaderImport.UploadDate = _Template.UploadDate;
                    HeaderImport.UpoadPath = _Template.UpoadPath;
                    HeaderImport.Status = SYDocumentStatus.PENDING.ToString();
                    HeaderImport.CreatedBy = User.UserName;
                    HeaderImport.CreatedOn = DateTime.Now;
                    DBI.ATImpRosterHeaders.Add(HeaderImport);
                    int Counts = 0;
                    Counts = 0;
                    foreach (var item in lsttemp_Rosters)
                    {
                        Header.EmpCode = item.EmpCode;
                        Counts += 1;
                        if (item.Shift.Trim() == "") continue;
                        var ImpRoster = new ATImpRoster();
                        ImpRoster.DocumentNo = DocumentNo;
                        ImpRoster.EmpCode = item.EmpCode;
                        ImpRoster.InDate = item.InDate;
                        ImpRoster.Shift = item.Shift.Trim().ToUpper();
                        ImpRoster.InYear = item.InDate.Year;
                        ImpRoster.InMonth = item.InDate.Month;

                        ImpRoster.LineItem = Counts;
                        DBI.ATImpRosters.Add(ImpRoster);
                    }
                    SetAutoApproval_(_Template.UploadBy);
                    if (ListApproval.Count() <= 0)
                    {
                        return "NO_LINE_MN";
                    }
                    //Add approver
                    string Approver = "";
                    foreach (var read in ListApproval)
                    {
                        Approver = read.Approver;
                        read.ID = 0;
                        read.LastChangedDate = DateTime.Now;
                        read.DocumentNo = DocumentNo;
                        read.Status = SYDocumentStatus.OPEN.ToString();
                        read.ApprovedBy = "";
                        read.ApprovedName = "";
                        DBI.ExDocApprovals.Add(read);
                    }

                    int row = DBI.SaveChanges();
                    #region Notifican on Mobile
                    var FirebaseID = DB.TokenResources.FirstOrDefault(w => w.IsLock == true && w.UserName == Approver);

                    if (FirebaseID != null)
                    {
                        string Desc = "សូមពិនិត្យសម្រេចលើការផ្លាស់ប្ដូរវេនរបស់បុគ្គលិក"; //+ Staff.Title + " " + objMatch.EmpName;
                        Notification.Notificationf Noti = new Notification.Notificationf();
                        var clientToken = new List<string>();
                        clientToken.Add(FirebaseID.FirebaseID);
                        //clientToken.Add("ez3uAH-qDEHRgMGhlf18cr:APA91bHliAC1icIxXFMNoxMCMG18AG_5I9OVCbpvkrtTy7wY2hq4Axa1p5DFYGeUlV9u3vunPb3ANkrNkwwQNJZmIYsDLA-4lEPblIDoT60Zv9F9kFMZvKcddG3Gi5F7yMJbNds963Aw");
                        var dd = Noti.SendNotification(clientToken, " Upload Roster", Desc);
                    }

                    #endregion
                    return SYConstant.OK;

                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DBI.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        #endregion

        public void SetAutoApproval_(string EmpCode)
        {
            ListApproval = new List<ExDocApproval>();
            var StaffReq = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (StaffReq != null)
            {
                HRStaffProfile StaffApp = new HRStaffProfile();
                var chkFLName = DB.HRStaffProfiles.Where(w => w.EmpCode == StaffReq.FirstLine).ToList();
                if (chkFLName.Count() > 0)
                {
                    StaffApp = chkFLName.First();
                }
                else if (chkFLName.Count() == 0)
                {
                    var chkSLName = DB.HRStaffProfiles.Where(w => w.EmpCode == StaffReq.SecondLine).ToList();
                    if (chkSLName.Count() > 0)
                    {
                        StaffApp = chkSLName.First();
                    }
                }
                int line = 0;
                if (StaffApp != null)
                {
                    if (StaffApp.EmpCode != null)
                    {
                        line += 1;
                        var DocApproval = new ExDocApproval();
                        DocApproval.Approver = StaffApp.EmpCode;
                        DocApproval.ApproverName = StaffApp.AllName;
                        DocApproval.ApprovedBy = "";
                        DocApproval.ApprovedName = "";
                        DocApproval.ApproveLevel = line;
                        DocApproval.DocumentType = "ROSTER";
                        DocApproval.Status = SYDocumentStatus.OPEN.ToString();
                        DocApproval.WFObject = "WF02";
                        ListApproval.Add(DocApproval);
                    }
                }
            }
        }
        public string ApporvalImport(string ID)
        {
            var DBD = new HumicaDBContext();
            var DBI = new HumicaDBContext();
            try
            {
                DBI.Configuration.AutoDetectChangesEnabled = false;
                DBD.Configuration.AutoDetectChangesEnabled = false;
                var objmatch = DB.ATImpRosterHeaders.FirstOrDefault(w => w.DocumentNo == ID);
                if (objmatch == null) return "INV_EN";
                var HOD = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == User.UserName);
                var Approver = DB.ExDocApprovals.FirstOrDefault(w => w.Approver == User.UserName && w.DocumentNo == ID && w.DocumentType == "ROSTER");
                if (Approver == null) {
                    var DocApp= DB.ExDocApprovals.FirstOrDefault(w => w.DocumentNo == ID && w.DocumentType == "ROSTER");
                    DocApp.ApprovedBy = User.UserName;

                    if (HOD != null)
                        DocApp.ApprovedName = HOD.AllName;
                    else
                        DocApp.ApprovedName = "";
                    DocApp.LastChangedDate = DateTime.Now.Date;
                    DocApp.ApprovedDate = DateTime.Now;
                    DocApp.Status = SYDocumentStatus.APPROVED.ToString();
                    DBI.ExDocApprovals.Attach(DocApp);
                    DBI.Entry(DocApp).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    Approver.ApprovedBy = User.UserName;

                    if (HOD != null)
                        Approver.ApprovedName = HOD.AllName;
                    else
                        Approver.ApprovedName = "";
                    Approver.LastChangedDate = DateTime.Now.Date;
                    Approver.ApprovedDate = DateTime.Now;
                    Approver.Status = SYDocumentStatus.APPROVED.ToString();
                    DBI.ExDocApprovals.Attach(Approver);
                    DBI.Entry(Approver).State = System.Data.Entity.EntityState.Modified;
                }
                
                objmatch.Status = SYDocumentStatus.APPROVED.ToString();
                objmatch.ChangedBy = User.UserName;
                objmatch.ChangedOn = DateTime.Now;
                DBI.ATImpRosterHeaders.Attach(objmatch);
                DBI.Entry(objmatch).Property(w => w.Status).IsModified = true;
                DBI.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                DBI.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;

                var date = DateTime.Now;
                var lsttemp_Rosters = DB.ATImpRosters.Where(w => w.DocumentNo == ID).ToList();


                Header = new ATEmpSchedule();
                DateTime FromDate = lsttemp_Rosters.Min(w => w.InDate).Value;
                DateTime ToDate = lsttemp_Rosters.Max(w => w.InDate).Value;

                var lstAttendance = DB.ATEmpSchedules.Where(w => EntityFunctions.TruncateTime(w.TranDate) >= FromDate.Date
            && EntityFunctions.TruncateTime(w.TranDate) <= ToDate.Date).ToList();
                lstAttendance = lstAttendance.ToList();
                var listShift = DB.ATShifts.ToList();
                var ListLeaveType = DB.HRLeaveTypes.ToList();
                var Approval = SYDocumentStatus.APPROVED.ToString();
                var listLeaveH = DB.HREmpLeaves.Where(w => w.Status == Approval).ToList();
                listLeaveH = listLeaveH.Where(w => ((w.FromDate.Date >= FromDate.Date && w.FromDate.Date <= ToDate.Date) ||
                (w.ToDate.Date >= FromDate.Date && w.ToDate.Date <= ToDate.Date) ||
                (FromDate.Date >= w.FromDate.Date && FromDate.Date <= w.ToDate.Date) || (ToDate.Date >= w.FromDate.Date && ToDate.Date <= w.ToDate.Date))).ToList();
                var ListLeave = DB.HREmpLeaveDs.Where(w => EntityFunctions.TruncateTime(w.LeaveDate) >= FromDate.Date
            && EntityFunctions.TruncateTime(w.LeaveDate) <= ToDate.Date).ToList();
                listShift = listShift.ToList();
                ListLeaveType = ListLeaveType.ToList();
                ListLeave = ListLeave.Where(x => listLeaveH.Where(w => w.EmpCode == x.EmpCode && w.Increment == x.LeaveTranNo).Any()).ToList();
                lstAttendance = lstAttendance.Where(w => lsttemp_Rosters.Where(x => x.EmpCode == w.EmpCode &&
                                   w.TranDate.Date == x.InDate.Value.Date && x.Shift != "").Any()).ToList();

                int Counts = 0;
                foreach (var item in lstAttendance)
                {
                    Counts += 1;

                    DBD.ATEmpSchedules.Attach(item);
                    DBD.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                Counts = 0;
                foreach (var read in lsttemp_Rosters)
                {
                    var item = new Temp_Roster();
                    var _listLeaveH = listLeaveH.Where(w => w.EmpCode == read.EmpCode).ToList();
                    var _ListLeave = ListLeave.Where(w => w.EmpCode == read.EmpCode && w.LeaveDate == read.InDate.Value).ToList();
                    item.EmpCode = read.EmpCode;
                    item.Shift = read.Shift;
                    item.InDate = read.InDate.Value;
                    Header.EmpCode = item.EmpCode;
                    Counts += 1;
                    if (item.Shift.Trim() == "") continue;
                    var Att = getEmpSchedule(item, listShift, ListLeaveType, _listLeaveH, _ListLeave);
                    DBI.ATEmpSchedules.Add(Att);
                }
                int row1 = DBD.SaveChanges();
                int row = DBI.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ID;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ID;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ID;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            finally
            {
                DBI.Configuration.AutoDetectChangesEnabled = true;
                DBD.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string CancelImport(string ID)
        {
            try
            {
                var objmatch = DB.ATImpRosterHeaders.FirstOrDefault(w => w.DocumentNo == ID);
                if (objmatch == null)
                {
                    return "INV_EN";
                }
                var Approver = DB.ExDocApprovals.FirstOrDefault(w => w.DocumentNo == ID && w.DocumentType == "ROSTER");
                Approver.ApprovedBy = User.UserName;
                var HOD = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == User.UserName);
                if (HOD != null)
                    Approver.ApprovedName = HOD.AllName;
                else
                    Approver.ApprovedName = "";
                Approver.LastChangedDate = DateTime.Now.Date;
                Approver.ApprovedDate = DateTime.Now;
                Approver.Status = SYDocumentStatus.CANCELLED.ToString();
                DB.ExDocApprovals.Attach(Approver);
                DB.Entry(Approver).State = System.Data.Entity.EntityState.Modified;
                objmatch.Status = SYDocumentStatus.CANCELLED.ToString();
                objmatch.ChangedBy = User.UserName;
                objmatch.ChangedOn = DateTime.Now;
                DB.ATImpRosterHeaders.Attach(objmatch);
                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;

                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ID;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ID;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ID;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string TransferNight(string ID, List<VIEW_ATEmpSchedule> List)
        {
            if (ID.Trim() == "")
            {
                return "INV_DOC";
            }
            try
            {
                var DBI = new HumicaDBContext();
                Header = new ATEmpSchedule();
                ListHeader = new List<ATEmpSchedule>();
                string[] _TranNo = ID.Split(';');
                var OTRate = DB.PROTRates.ToList();
                foreach (var TranNo in _TranNo)
                {
                    Header.EmpCode = TranNo;
                    string OTCode = "NM";
                    DateTime FromDate = FMonthly.FromDate.Date;
                    DateTime ToDate = FMonthly.ToDate.Date;

                    var Result = DB.ATEmpSchedules.Where(w => w.EmpCode == TranNo &&
                    FromDate.Date <= w.TranDate && w.TranDate <= ToDate.Date && w.NWH > 0).ToList();
                    foreach (var item in Result)
                    {
                        var OTH = new PREmpOverTime();
                        OTH.OTHour = Convert.ToDecimal(item.NWH);
                        OTH.EmpCode = item.EmpCode;
                        OTH.EmpName = "";
                        OTH.OTDate = item.TranDate;
                        OTH.PayMonth = item.TranDate;
                        OTH.OTType = OTCode;
                        OTH.LCK = 0;
                        OTH.CreateBy = User.UserName;
                        OTH.CreateOn = DateTime.Now;
                        if (OTRate.Where(w => w.OTCode == OTCode).Any())
                            OTH.OTDescription = OTRate.FirstOrDefault(w => w.OTCode == OTCode).OTType;
                        DB.PREmpOverTimes.Add(OTH);
                        int row = DB.SaveChanges();
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        private ATLaEaPolicy LateEarlyDeduction(decimal? amount, string type)
        {
            var result = new ATLaEaPolicy();
            result = DB.ATLaEaPolicies.Where(x => x.OTFrom <= amount &&
              x.OTTo >= amount && x.DedType == type).FirstOrDefault();
            return result;
        }
        public string TransferLateEarly(string ID)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                if (ID.Trim() == "")
                {
                    return "No recorde selected.";
                }
                var tbHRStaff = DB.HRStaffProfiles.ToList();
                var HRSetting = DB.SYHRSettings.First();
                Header = new ATEmpSchedule();
                //ListHeader = new List<ATEmpSchedule>();
                var ListLateEarly = new List<ATLateEarlyDeduct>();
                ListStaffs = new List<HRStaffProfile>();
                ListStaffs = tbHRStaff.OrderBy(w => w.EmpCode).ToList();
                string[] _TranNo = ID.Split(';');
                DateTime FromDate = FMonthly.FromDate.Date;
                DateTime ToDate = FMonthly.ToDate.Date;
                var tAtEmpAtt = DB.ATLateEarlyDeducts;
                ListLateEarly = tAtEmpAtt.Where(w => w.DocumentDate >= FromDate.Date && w.DocumentDate <= ToDate.Date).OrderBy(w => w.EmpCode).ThenBy(w => w.DocumentDate).ToList();
                string msg = string.Empty;
                var LeaveType = DB.SYHRSettings.FirstOrDefault().MisScanAL;
                foreach (var staff in _TranNo)
                {
                    
                    var CurrentBalance = DB.HREmpLeaveBs.Where(w => w.EmpCode == staff && w.InYear == ToDate.Year && w.LeaveCode == "AL").FirstOrDefault();
                    if (CurrentBalance == null)
                        msg = "NO_BALANCE";
                    if (CurrentBalance.Current_AL <= (decimal)0.5)
                    {
                        msg = TransferSalaryDeduction(staff, ToDate.Date, FromDate, ToDate, HRSetting, ListLateEarly);
                    }
                    else
                    {
                        msg = TransferLeaveDeduction(staff, LeaveType, FromDate, ToDate);
                    }
                }
                
                return msg;
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string TransferLeaveDeduction(string id, string leaveType, DateTime fromDate, DateTime toDate)
        {
            var DBI = new HumicaDBContext();
            var DBD = new HumicaDBContext();
            try
            {
                DBI.Configuration.AutoDetectChangesEnabled = false;
                DBD.Configuration.AutoDetectChangesEnabled = false;
                var listAtt = DB.ATEmpSchedules.Where(w => EntityFunctions.TruncateTime(w.TranDate) >= fromDate.Date
                && EntityFunctions.TruncateTime(w.TranDate) <= toDate.Date).OrderBy(w => w.EmpCode).ThenBy(w => w.TranDate).ToList();


                var LstLaEa = DB.ATLaEaPolicies.ToList();

                string Approval = SYDocumentStatus.APPROVED.ToString();

                ListPayParameter = DB.PRParameters.ToList();
                var _lstEmpLeave = DB.HREmpLeaves.ToList();
                var _lstEmployeeD = DB.HREmpLeaveDs.ToList();

                int Increment;
                if (_lstEmpLeave.Count == 0) Increment = 0;
                else Increment = _lstEmpLeave.Max(w => w.Increment);
                int i = 0;
                var resLeave = _lstEmpLeave.Where(w => w.TranType == true).ToList();
                var resLeaveD = _lstEmployeeD.Where(w => resLeave.Where(x => x.Increment == w.LeaveTranNo).Any()).ToList();
                //foreach (var id in ids)
                //{
                    var Staff = ListStaffs.Where(w => w.EmpCode == id).FirstOrDefault();
                    //if (Staff == null) continue;

                    Header.EmpCode = id;


                    decimal leaveDay = 0;
                    decimal leaveHour = 0;
                    var parameter = ListPayParameter.Find(w => w.Code == Staff.PayParam);
                    decimal workHour = parameter != null ? parameter.WHOUR.Value() : 8;
                    leaveHour = workHour;


                    var DEmpLeave = resLeave.Where(w => w.EmpCode == id && w.FromDate <= toDate && w.ToDate >= fromDate).ToList();
                    var DEmpLeaveD = resLeaveD.Where(w => DEmpLeave.Where(x => x.Increment == w.LeaveTranNo).Any()).ToList();
                    foreach (var item in DEmpLeave)
                    {
                        var objEmp = DEmpLeaveD.Where(w => w.LeaveTranNo == item.Increment).ToList();
                        foreach (var item1 in objEmp)
                        {
                            DBD.HREmpLeaveDs.Attach(item1);
                            DBD.Entry(item1).State = EntityState.Deleted;
                            DBD.SaveChanges();
                        }
                        DBD.HREmpLeaves.Attach(item);
                        DBD.Entry(item).State = EntityState.Deleted;
                        DBD.SaveChanges();
                    }
                    var empAtts = listAtt.ToList().FindAll(x => ((x.Late1.Value() + x.Late2.Value() > 0) ||
                        (x.Early1.Value() + x.Early2.Value() > 0)) && x.EmpCode == id);
                    foreach (var item in empAtts)
                    {
                        var _empLeave = new HREmpLeave();
                        var _empLeaveD = new HREmpLeaveD();
                        DateTime StartTime = new DateTime();
                        DateTime EndTime = new DateTime();
                        if (item.Late1.Value() > 0)
                        {
                            var _late1 = item.Late1.Value();
                            EndTime = item.Start1.Value;
                            StartTime = EndTime.AddMinutes(-_late1);
                            _empLeaveD.StartTime = StartTime;
                            _empLeaveD.EndTime = EndTime;
                            Increment += 1;
                            GetLeaveAuto(_empLeave, _empLeaveD, workHour, item.TranDate, Increment, leaveType, item.EmpCode);
                            DB.HREmpLeaves.Add(_empLeave);
                            DB.HREmpLeaveDs.Add(_empLeaveD);

                        }
                        if (item.Late2.Value() > 0)
                        {
                            var _late1 = item.Late2.Value();
                            EndTime = item.Start2.Value;
                            StartTime = EndTime.AddMinutes(-_late1);
                            _empLeave = new HREmpLeave();
                            _empLeaveD = new HREmpLeaveD();
                            _empLeaveD.StartTime = StartTime;
                            _empLeaveD.EndTime = EndTime;
                            Increment += 1;
                            GetLeaveAuto(_empLeave, _empLeaveD, workHour, item.TranDate, Increment, leaveType, item.EmpCode);
                            DB.HREmpLeaves.Add(_empLeave);
                            DB.HREmpLeaveDs.Add(_empLeaveD);
                        }
                        if (item.Early1.Value() > 0)
                        {
                            var _late1 = item.Early1.Value();
                            EndTime = item.End1.Value;
                            StartTime = EndTime.AddMinutes(-_late1);
                            _empLeave = new HREmpLeave();
                            _empLeaveD = new HREmpLeaveD();
                            _empLeaveD.StartTime = StartTime;
                            _empLeaveD.EndTime = EndTime;
                            Increment += 1;
                            GetLeaveAuto(_empLeave, _empLeaveD, workHour, item.TranDate, Increment, leaveType, item.EmpCode);
                            DB.HREmpLeaves.Add(_empLeave);
                            DB.HREmpLeaveDs.Add(_empLeaveD);
                        }
                        if (item.Early2.Value() > 0)
                        {
                            var _late1 = item.Early2.Value();
                            EndTime = item.End2.Value;
                            StartTime = EndTime.AddMinutes(-_late1);
                            _empLeave = new HREmpLeave();
                            _empLeaveD = new HREmpLeaveD();
                            _empLeaveD.StartTime = StartTime;
                            _empLeaveD.EndTime = EndTime;
                            Increment += 1;
                            GetLeaveAuto(_empLeave, _empLeaveD, workHour, item.TranDate, Increment, leaveType, item.EmpCode);
                            DB.HREmpLeaves.Add(_empLeave);
                            DB.HREmpLeaveDs.Add(_empLeaveD);
                        }
                    DB.SaveChanges();
                        GenerateLeaveObject leave = new GenerateLeaveObject();
                        leave.GET_Leave_LeaveBalance(item.EmpCode, item.TranDate.Year);
                    //}
                }
                #region  Misscan
                var empLeave = new HREmpLeave();
                var empLeaveD = new HREmpLeaveD();
                int Year = 0;
                //MissScan
                var ListAttMisscan = DB.ATLateEarlyDeducts.Where(w => w.LeaveCode == null && w.IsMissScan == 0 && w.IsTransfer == false &&
                w.EmpCode == id​​​​​ && w.DeductType == "MissScan" && w.EmpCode == id).ToList();

                //ABS
                var ListABS = DB.ATLateEarlyDeducts.Where(w => w.LeaveCode == null && w.IsTransfer == false &&
                w.EmpCode == id​​​​​ && w.DeductType == "ABS" && w.EmpCode == id).ToList();
                int CountMissCan = DB.SYHRSettings.FirstOrDefault().CountMisscan;
                int num_count = ListAttMisscan.Count();
                int num = 0;

                if (ListABS != null)
                {
                    foreach (var leave in ListABS)
                    {
                        DateTime FromDate = leave.DocumentDate;
                        DateTime ToDate = leave.DocumentDate;
                        empLeave = new HREmpLeave();
                        empLeaveD = new HREmpLeaveD();
                        empLeaveD.StartTime = FromDate;
                        empLeaveD.EndTime = ToDate;
                        Increment += 1;
                        GetLeaveAutoForMissScan(empLeave, empLeaveD, workHour, ToDate, Increment, leaveType, id);
                        DB.HREmpLeaves.Add(empLeave);
                        DB.HREmpLeaveDs.Add(empLeaveD);
                        Year = ToDate.Year;
                        DB.SaveChanges();
                        GenerateLeaveObject _leave = new GenerateLeaveObject();
                        _leave.GET_Leave_LeaveBalance(id, Year);

                        foreach (var item in ListABS)
                        {
                            item.IsTransfer = true;
                            DB.ATLateEarlyDeducts.Attach(item);
                            DB.Entry(item).Property(w => w.IsTransfer).IsModified = true;
                        }

                    }
                }
                if (ListAttMisscan.Count() >= CountMissCan)
                {
                    DateTime FromDate = ListAttMisscan.FirstOrDefault().DocumentDate;
                    DateTime ToDate = ListAttMisscan.LastOrDefault().DocumentDate;
                    int j;
                    for (j = 1; num_count >= CountMissCan; j++)
                    {
                        num_count -= CountMissCan;
                        empLeave = new HREmpLeave();
                        empLeaveD = new HREmpLeaveD();
                        empLeaveD.StartTime = FromDate.AddDays(j);
                        empLeaveD.EndTime = FromDate.AddDays(j); 
                        Increment += 1;
                        GetLeaveAutoForMissScan(empLeave, empLeaveD,workHour, FromDate.AddDays(j), Increment, leaveType, id);
                        DB.HREmpLeaves.Add(empLeave);
                        DB.HREmpLeaveDs.Add(empLeaveD);
                        Year = ToDate.Year;
                        DB.SaveChanges();
                        GenerateLeaveObject _leave = new GenerateLeaveObject();
                        _leave.GET_Leave_LeaveBalance(id, Year);

                        num += CountMissCan;
                        var ListTransfer = ListAttMisscan.ToList().OrderBy(w => w.DocumentDate).Take(num).ToList();
                        foreach (var item in ListTransfer)
                        {
                            item.IsTransfer = true;
                            DB.ATLateEarlyDeducts.Attach(item);
                            DB.Entry(item).Property(w => w.IsTransfer).IsModified = true;
                        }

                    }

                }
                DB.SaveChanges();
                #endregion
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }

            finally
            {
                DBI.Configuration.AutoDetectChangesEnabled = true;
                DBD.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string TransferSalaryDeduction(string id, DateTime PayInMonth, DateTime fromDate, DateTime toDate, SYHRSetting setting, List<ATLateEarlyDeduct> listAtt)
        {
            var DBI = new HumicaDBContext();
            var DBD = new HumicaDBContext();
            try
            {
                DBI.Configuration.AutoDetectChangesEnabled = false;
                DBD.Configuration.AutoDetectChangesEnabled = false;
                string msg = string.Empty;
                if (PayInMonth == null)
                {
                    return msg = "Please select pay in month";
                }
                decimal sumLateMN = 0;
                decimal sumEarlyMN = 0;
                decimal sumLateDay, sumEarlyDay;
                var _lstEmpLeave = DB.HREmpLeaves.ToList();
                int Increment;
                if (_lstEmpLeave.Count == 0) Increment = 0;
                else Increment = _lstEmpLeave.Max(w => w.Increment);
                string DEDType = "DED";
                var ListDed = DB.PR_RewardsType.Where(w => w.ReCode == DEDType).ToList();
                string Approval = SYDocumentStatus.APPROVED.ToString();

                if (setting == null)
                {
                    return msg = "DEDCUTION_EN";
                }
                if (ListDed.Where(w => w.Code == setting.DeductEalary).Count() == 0)
                {
                    return msg = "INVALID_EARLY";
                }
                if (ListDed.Where(w => w.Code == setting.DeductLate).Count() == 0)
                {
                    return msg = "INVALID_LATE";
                }

                //foreach (var id in Ids)
                //{
                    sumLateDay = 
                    sumEarlyDay =
                    sumLateMN =
                    sumEarlyMN = 0;
                    //if (ListStaffs.Where(w => w.EmpCode == id).Count() == 0)
                    //    continue;

                    Header.EmpCode = id;

                    var objMatch = DB.PREmpLateDeducts.Where(w => w.EmpCode == id).ToList();
                    var ListAtt = listAtt.Where(w => w.EmpCode == id).ToList();
                    var EmpLate = ListAtt.Where(w => w.IsLate_Early == 0 && w.DeductType == "LATE").ToList();
                    var EmpEarly = ListAtt.Where(w => w.IsLate_Early == 0 && w.DeductType == "EARLY").ToList();
                    var EmpMissScanIN = ListAtt.Where(w => w.DeductType == "MissScan" && w.Remark == "MissScanIN" && w.LeaveCode == "").ToList();
                    var EmpMissScanOut = ListAtt.Where(w => w.DeductType == "MissScan" && w.Remark == "MissScanOut" && w.LeaveCode == "").ToList();
                    string statusEalary = setting.DeductEalary;
                    string statusLate = setting.DeductLate;
                    // string statusMissScan = "MissScan";

                    decimal? Late_Min = EmpLate.Sum(x => x.Amount);
                    decimal? Early_Min = EmpEarly.Sum(x => x.Amount);
                    decimal? MissIN = EmpMissScanIN.Sum(x => x.Amount);
                    decimal? MissOUT = EmpMissScanOut.Sum(x => x.Amount);
                    var late = LateEarlyDeduction(Late_Min, "LATE");
                    var early = LateEarlyDeduction(Early_Min, "EARLY");
                    if (late != null)
                    {
                        if (late.RuleType != "D")
                            sumLateMN = Convert.ToDecimal(late.Rate);
                        else if (late.RuleType == "D")
                            sumLateDay = Convert.ToDecimal(late.Rate);
                    }
                    if (early != null)
                    {
                        if (early.RuleType != "D")
                            sumEarlyMN = Convert.ToDecimal(early.Rate);
                        else if (early.RuleType == "D")
                            sumEarlyDay = Convert.ToDecimal(early.Rate);
                    }
                    var Result = objMatch.FirstOrDefault(w => w.DedCode == statusLate && w.InMonth.Date == PayInMonth.Date);
                    if (Result != null)
                    {
                        DB.PREmpLateDeducts.Attach(Result);
                        DB.Entry(Result).State = EntityState.Deleted;
                        var rowD = DB.SaveChanges();
                    }
                    var Result1 = objMatch.FirstOrDefault(w => w.DedCode == statusEalary && w.InMonth.Date == PayInMonth.Date);
                    if (Result1 != null)
                    {
                        DB.PREmpLateDeducts.Attach(Result1);
                        DB.Entry(Result1).State = EntityState.Deleted;
                        var rowD = DB.SaveChanges();
                    }
                    var Result2 = objMatch.Where(w => w.DedCode == "SCANIN" && w.InMonth.Date == PayInMonth.Date).ToList();
                    foreach (var item in Result2)
                    {
                        DB.PREmpLateDeducts.Attach(item);
                        DB.Entry(item).State = EntityState.Deleted;
                        var rowD = DB.SaveChanges();
                    }
                    var Result3 = objMatch.Where(w => w.DedCode == "SCANOUT" && w.InMonth.Date == PayInMonth.Date).ToList();
                    foreach (var item in Result3)
                    {
                        DB.PREmpLateDeducts.Attach(item);
                        DB.Entry(item).State = EntityState.Deleted;
                        var rowD = DB.SaveChanges();
                    }
                    var lateEarly = new PREmpLateDeduct();
                    if (MissIN > 0)
                    {
                        lateEarly.EmpCode = id;
                        lateEarly.DedCode = "SCANIN";
                        lateEarly.Day = 0.5M;
                        lateEarly.Minute = 0;
                        lateEarly.InMonth = PayInMonth;
                        lateEarly.Remark = "Generate from attendance";
                        lateEarly.FromDate = fromDate;
                        lateEarly.ToDate = toDate;
                        lateEarly.CreateBy = User.UserName;
                        lateEarly.CreateOn = DateTime.Now;
                        lateEarly.Status = 1;
                        DB.PREmpLateDeducts.Add(lateEarly);
                        int row = DB.SaveChanges();
                    }
                    lateEarly = new PREmpLateDeduct();
                    if (MissOUT > 0)
                    {
                        lateEarly.EmpCode = id;
                        lateEarly.DedCode = "SCANOUT";
                        lateEarly.Day = 0.5M;
                        lateEarly.Minute = 0;
                        lateEarly.InMonth = PayInMonth;
                        lateEarly.Remark = "Generate from attendance";
                        lateEarly.FromDate = fromDate;
                        lateEarly.ToDate = toDate;
                        lateEarly.CreateBy = User.UserName;
                        lateEarly.CreateOn = DateTime.Now;
                        lateEarly.Status = 1;
                        DB.PREmpLateDeducts.Add(lateEarly);
                        int row = DB.SaveChanges();
                    }
                    if (sumLateMN + sumLateDay > 0)
                    {
                        lateEarly.EmpCode = id;
                        lateEarly.DedCode = statusLate;
                        lateEarly.Day = sumLateDay;
                        lateEarly.Minute = sumLateMN;
                        lateEarly.InMonth = PayInMonth;
                        lateEarly.Remark = "Generate from attendance";
                        lateEarly.FromDate = fromDate;
                        lateEarly.ToDate = toDate;
                        lateEarly.CreateBy = User.UserName;
                        lateEarly.CreateOn = DateTime.Now;
                        lateEarly.Status = 1;
                        DB.PREmpLateDeducts.Add(lateEarly);
                        int row = DB.SaveChanges();
                    }
                    if (sumEarlyDay + sumEarlyMN > 0)
                    {
                        #region Ealary
                        var LateDeduct = new PREmpLateDeduct();
                        LateDeduct.EmpCode = id;
                        LateDeduct.DedCode = statusEalary;
                        LateDeduct.Day = sumEarlyDay;
                        LateDeduct.Minute = sumEarlyMN;
                        LateDeduct.InMonth = PayInMonth;
                        LateDeduct.Remark = "Generate from attendance";
                        LateDeduct.FromDate = fromDate;
                        LateDeduct.ToDate = toDate;
                        LateDeduct.CreateBy = User.UserName;
                        LateDeduct.CreateOn = DateTime.Now;
                        LateDeduct.Status = 1;
                        DB.PREmpLateDeducts.Add(LateDeduct);
                        int row = DB.SaveChanges();
                        #endregion
                    }

                #region  Misscan
                var empLeave = new HREmpLeave();
                var empLeaveD = new HREmpLeaveD();
                var Staff = DB.HRStaffProfiles.Where(w => w.EmpCode == id).FirstOrDefault();
                var parameter = ListPayParameter.Find(w => w.Code == Staff.PayParam);
                decimal workHour = parameter != null ? parameter.WHOUR.Value() : 8;
                
                int Year = 0;
                var ListAttMisscan = DB.ATLateEarlyDeducts.Where(w => w.LeaveCode == null && w.IsMissScan == 0 && w.IsTransfer == false &&
                w.EmpCode == id​​​​​ && w.DeductType == "MissScan").ToList();
                var LeaveType = DB.SYHRSettings.FirstOrDefault().MisScanUP;
                int CountMissCan = DB.SYHRSettings.FirstOrDefault().CountMisscan;
                int num_count = ListAttMisscan.Count();
                int num = 0;

                if (ListAttMisscan.Count() > CountMissCan)
                {
                    DateTime FromDate = ListAttMisscan.LastOrDefault().DocumentDate;
                    DateTime ToDate = ListAttMisscan.LastOrDefault().DocumentDate;
                    int j;
                    for (j = 1; num_count >= CountMissCan; j++)
                    {
                        num_count -= CountMissCan;
                        empLeave = new HREmpLeave();
                        empLeaveD = new HREmpLeaveD();
                        empLeaveD.StartTime = FromDate;
                        empLeaveD.EndTime = ToDate;
                        Increment += 1;
                        GetLeaveAutoForMissScan(empLeave, empLeaveD, workHour, ToDate, Increment, LeaveType, id);
                        DB.HREmpLeaves.Add(empLeave);
                        DB.HREmpLeaveDs.Add(empLeaveD);
                        Year = ToDate.Year;
                        DB.SaveChanges();
                        GenerateLeaveObject _leave = new GenerateLeaveObject();
                        _leave.GET_Leave_LeaveBalance(id, Year);

                        num += CountMissCan;
                        var ListTransfer = ListAttMisscan.ToList().OrderBy(w => w.DocumentDate).Take(num).ToList();
                        foreach (var item in ListTransfer)
                        {
                            item.IsTransfer = true;
                            DB.ATLateEarlyDeducts.Attach(item);
                            DB.Entry(item).Property(w => w.IsTransfer).IsModified = true;
                        }

                    }

                }

                #endregion

                //}
                return msg = SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            finally
            {
                DBI.Configuration.AutoDetectChangesEnabled = true;
                DBD.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        #region TransferLateEarly_
        public string TransferLateEarly_(string ID, bool? IsDudSalary, string LeaveType, DateTime PayInMonth)
        {
            if (PayInMonth == null) return "Please select pay in month";
            if (ID.Trim() == "")
            {
                return "No recorde selected.";
            }
            var DBI = new HumicaDBContext();
            var DBD = new HumicaDBContext();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                DBI.Configuration.AutoDetectChangesEnabled = false;
                DBD.Configuration.AutoDetectChangesEnabled = false;
                Header = new ATEmpSchedule();
                ListHeader = new List<ATEmpSchedule>();
                string[] _TranNo = ID.Split(';');
                var LstLeaveType = DB.HRLeaveTypes.ToList();
                var LstLaEa = DB.ATLaEaPolicies.ToList();
                var HRSetting = DB.SYHRSettings.First();
                DateTime FromDate = FMonthly.FromDate.Date;
                DateTime ToDate = FMonthly.ToDate.Date;
                string DEDType = "DED";
                var ListDed = DB.PR_RewardsType.Where(w => w.ReCode == DEDType).ToList();
                ListHeader = DB.ATEmpSchedules.Where(w => w.TranDate >= FromDate.Date
                    && w.TranDate <= ToDate.Date).ToList();
                var _Policy = DB.ATPolicies.First();
                string Approval = SYDocumentStatus.APPROVED.ToString();
                string Early = SYDocumentStatus.EARLY.ToString();
                string Late = SYDocumentStatus.LATE.ToString();
                if (IsDudSalary == true)
                {
                    foreach (var TranNo in _TranNo)
                    {
                        var _Staffs = DB.HRStaffProfiles.Where(w => w.EmpCode == TranNo);
                        if (_Staffs == null) continue;

                        Header.EmpCode = TranNo;

                        var objMatch = DB.PREmpLateDeducts.Where(w => w.EmpCode == TranNo).ToList();
                        var ListAtt = ListHeader.Where(w => w.EmpCode == TranNo).ToList();

                        if (HRSetting == null)
                        {
                            return "DEDCUTION_EN";
                        }
                        if (!ListDed.Where(w => w.Code == HRSetting.DeductLate).Any())
                        {
                            return "DEDCUTION_EN";
                        }
                        if (!ListDed.Where(w => w.Code == HRSetting.DeductEalary).Any())
                        {
                            return "DEDCUTION_EN";
                        }
                        decimal sumLateMN = 0;
                        decimal sumEarlyMN = 0;
                        decimal sumLateDay, sumEarlyDay;
                        sumLateDay = sumEarlyDay = 0;

                        decimal? Late_Min = ListAtt.Sum(x => x.Late1) + ListAtt.Sum(x => x.Late2);
                        decimal? Early_Min = ListAtt.Sum(x => x.Early1) + ListAtt.Sum(x => x.Early2);
                        var late = this.LateEarlyDeduction(Late_Min, "LATE");
                        var early = this.LateEarlyDeduction(Early_Min, "EARLY");

                        if (late != null)
                        {

                            if (late.RuleType != "D")
                                sumLateMN = Convert.ToDecimal(late.Rate);
                            else if (late.RuleType == "D")
                                sumLateDay = Convert.ToDecimal(late.Rate);
                        }
                        else sumLateMN = Late_Min.Value;
                        if (early != null)
                        {
                            if (early.RuleType != "D")
                                sumEarlyMN = Convert.ToDecimal(early.Rate);
                            else if (early.RuleType == "D")
                                sumEarlyDay = Convert.ToDecimal(early.Rate);
                        }
                        else sumEarlyMN = Early_Min.Value;

                        string statusEalary = HRSetting.DeductEalary;
                        string statusLate = HRSetting.DeductLate;


                        var Result = objMatch.FirstOrDefault(w => w.DedCode == statusLate && w.InMonth.Date == PayInMonth.Date);
                        if (Result != null)
                        {
                            DB.PREmpLateDeducts.Attach(Result);
                            DB.Entry(Result).State = EntityState.Deleted;
                            var rowD = DB.SaveChanges();
                        }
                        var Result1 = objMatch.FirstOrDefault(w => w.DedCode == statusEalary && w.InMonth.Date == PayInMonth.Date);
                        if (Result1 != null)
                        {
                            DB.PREmpLateDeducts.Attach(Result1);
                            DB.Entry(Result1).State = EntityState.Deleted;
                            var rowD = DB.SaveChanges();
                        }
                        var lateEarly = new PREmpLateDeduct();
                        if (sumLateMN + sumLateDay > 0)
                        {
                            lateEarly.EmpCode = TranNo;
                            lateEarly.DedCode = statusLate;
                            lateEarly.Day = sumLateDay;
                            lateEarly.Minute = sumLateMN;
                            lateEarly.InMonth = PayInMonth;
                            lateEarly.Remark = "Generate from attendance";
                            lateEarly.FromDate = FromDate;
                            lateEarly.ToDate = ToDate;
                            lateEarly.CreateBy = User.UserName;
                            lateEarly.CreateOn = DateTime.Now;
                            lateEarly.Status = 1;
                            DB.PREmpLateDeducts.Add(lateEarly);
                            int row = DB.SaveChanges();
                        }
                        if (sumEarlyDay + sumEarlyMN > 0)
                        {
                            #region Ealary
                            var LateDeduct = new PREmpLateDeduct();
                            LateDeduct.EmpCode = TranNo;
                            LateDeduct.DedCode = statusEalary;
                            LateDeduct.Day = sumEarlyDay;
                            LateDeduct.Minute = sumEarlyMN;
                            LateDeduct.InMonth = PayInMonth;
                            LateDeduct.Remark = "Generate from attendance";
                            LateDeduct.FromDate = FromDate;
                            LateDeduct.ToDate = ToDate;
                            LateDeduct.CreateBy = User.UserName;
                            LateDeduct.CreateOn = DateTime.Now;
                            LateDeduct.Status = 1;
                            DB.PREmpLateDeducts.Add(LateDeduct);
                            int row = DB.SaveChanges();
                            #endregion
                        }
                    }
                }
                else
                {
                    var _ListStaff = DB.HRStaffProfiles.ToList();
                    var Para = DB.PRParameters.ToList();
                    var _lstEmpLeave = DB.HREmpLeaves.ToList();
                    var _lstEmployeeD = DB.HREmpLeaveDs.ToList();

                    int Increment;
                    if (_lstEmpLeave.Count == 0) Increment = 0;
                    else Increment = _lstEmpLeave.Max(w => w.Increment);
                    int i = 0;
                    var resLeave = _lstEmpLeave.Where(w => w.TranType == true).ToList();
                    var resLeaveD = _lstEmployeeD.Where(w => resLeave.Where(x => x.Increment == w.LeaveTranNo).Any()).ToList();
                    foreach (var TranNo in _TranNo)
                    {
                        Header.EmpCode = TranNo;
                        var ListAtt = ListHeader.Where(w => w.EmpCode == TranNo).ToList();
                        var Late_Min = ListAtt.Sum(x => x.Late1) + ListAtt.Sum(x => x.Late2);
                        var Early_Min = ListAtt.Sum(x => x.Early1) + ListAtt.Sum(x => x.Early2);
                        var Late_Count = ListAtt.Where(w => w.Late1 > 0).Count() + ListAtt.Where(w => w.Late2 > 0).Count();
                        var Early_Count = ListAtt.Where(w => w.Early1 > 0).Count() + ListAtt.Where(w => w.Early2 > 0).Count();
                        decimal LeaveDay = 0.00M;
                        decimal LeaveHour = 0.00M;
                        var Staff = _ListStaff.FirstOrDefault(w => w.EmpCode == TranNo);
                        var Par = Para.FirstOrDefault(w => w.Code == Staff.PayParam);
                        if (LstLaEa.Count > 0)
                        {
                            if (LstLaEa.Where(w => w.Type == "TIME").Count() > 0)
                            {
                                var Result = LstLaEa.Where(w => w.DedType == Late && w.OTFrom <= Late_Count && w.OTTo >= Late_Count).
                                    OrderBy(x => x.OTTo).Sum(x => x.Rate);
                                LeaveDay += Result;

                                var Result1 = LstLaEa.Where(w => w.DedType == Early && w.OTFrom <= Early_Count && w.OTTo >= Early_Count).
                                    OrderBy(x => x.OTTo).Sum(x => x.Rate);
                                LeaveDay += Result1;
                            }
                            else
                            {
                                var Result = LstLaEa.Where(w => w.DedType == Late && w.OTFrom <= Late_Min && w.OTTo >= Late_Min).ToList();
                                if (Result.Count() > 0)
                                {
                                    LeaveDay += Result.First().Rate;
                                }
                                var Result1 = LstLaEa.Where(w => w.DedType == Early && w.OTFrom <= Early_Min && w.OTTo >= Early_Min).ToList();
                                if (Result1.Count() > 0)
                                {
                                    LeaveDay += Result1.First().Rate;
                                }
                            }
                        }
                        else
                        {
                            LeaveHour = Math.Round(Convert.ToDecimal((Late_Min + Early_Min) / 60.00), 2);
                            LeaveDay = LeaveHour / Convert.ToDecimal(Par.WHOUR);
                        }
                        if (LeaveDay > 1)
                        {

                        }

                        var DEmpLeave = resLeave.Where(w => w.EmpCode == TranNo && ((w.FromDate >= FromDate && w.FromDate <= ToDate) || (w.ToDate >= FromDate && w.ToDate <= ToDate) ||
                        (FromDate >= w.FromDate && FromDate <= w.ToDate) || (ToDate >= w.FromDate && ToDate <= w.ToDate))).ToList();
                        var DEmpLeaveD = resLeaveD.Where(w => DEmpLeave.Where(x => x.Increment == w.LeaveTranNo).Any()).ToList();
                        foreach (var item in DEmpLeave)
                        {
                            var objEmp = DEmpLeaveD.Where(w => w.LeaveTranNo == item.Increment).ToList();
                            foreach (var item1 in objEmp)
                            {
                                DBD.HREmpLeaveDs.Attach(item1);
                                DBD.Entry(item1).State = EntityState.Deleted;
                            }
                            DBD.HREmpLeaves.Attach(item);
                            DBD.Entry(item).State = EntityState.Deleted;
                        }
                        foreach (var item in ListAtt)
                        {
                            var Obj = new ATEmpSchedule();
                            Obj = item;
                            i += 1;
                            HREmpLeave empLeave = new HREmpLeave();
                            empLeave.EmpCode = item.EmpCode;
                            empLeave.Increment = Increment + i;
                            if (item.LeaveDesc != "")
                                empLeave.LeaveCode = item.LeaveDesc;
                            else if (item.LeaveCode != "")
                                empLeave.LeaveCode = item.LeaveCode;
                            empLeave.FromDate = item.TranDate;
                            empLeave.ToDate = item.TranDate;
                            empLeave.NoDay = 1;
                            empLeave.TranType = true;
                            empLeave.NoPH = 0;
                            empLeave.NoRest = 0;
                            empLeave.Reason = "Transfer from attendance";
                            empLeave.RequestDate = item.TranDate;
                            empLeave.CreateBy = User.UserName;
                            empLeave.CreateOn = DateTime.Now;
                            empLeave.Status = SYDocumentStatus.APPROVED.ToString();
                            DB.HREmpLeaves.Add(empLeave);
                            HREmpLeaveD EmpLeaveD = new HREmpLeaveD();
                            EmpLeaveD.LeaveTranNo = Increment + i;
                            EmpLeaveD.LineItem = 1;
                            EmpLeaveD.EmpCode = item.EmpCode;
                            EmpLeaveD.LeaveCode = LeaveType;
                            EmpLeaveD.LeaveDate = item.TranDate;
                            EmpLeaveD.CutMonth = item.TranDate;
                            EmpLeaveD.Status = "Leave";
                            EmpLeaveD.LHour = Par.WHOUR;
                            EmpLeaveD.Remark = "FullDay";
                            EmpLeaveD.CreateBy = User.UserName;
                            EmpLeaveD.CreateOn = DateTime.Now;
                            Obj = item;
                            Obj.LeaveCode = item.LeaveDesc;
                            DB.HREmpLeaveDs.Add(EmpLeaveD);
                            DBI.ATEmpSchedules.Attach(Obj);
                            DBI.Entry(Obj).Property(w => w.LeaveCode).IsModified = true;
                        }
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }

            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
                DBI.Configuration.AutoDetectChangesEnabled = true;
                DBD.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        #endregion
        public string TransferAttendance(string ID, bool? IsDudSalary, string LeaveType)
        {
            if (ID.Trim() == "")
            {
                return "INV_DOC";
            }
            var DBI = new HumicaDBContext();
            var DBD = new HumicaDBContext();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                DBI.Configuration.AutoDetectChangesEnabled = false;
                DBD.Configuration.AutoDetectChangesEnabled = false;
                Header = new ATEmpSchedule();
                ListHeader = new List<ATEmpSchedule>();
                string[] _TranNo = ID.Split(';');
                var LstLeaveType = DB.HRLeaveTypes.ToList();
                var LstLaEa = DB.ATLaEaPolicies.ToList();
                var HRSetting = DB.SYHRSettings.First();
                DateTime FromDate = FMonthly.FromDate.Date;
                DateTime ToDate = FMonthly.ToDate.Date;
                string DEDType = "DED";
                var ListDed = DB.PR_RewardsType.Where(w => w.ReCode == DEDType).ToList();
                ListHeader = DB.ATEmpSchedules.Where(w => w.TranDate >= FromDate.Date
                    && w.TranDate <= ToDate.Date).ToList();
                var _Policy = DB.ATPolicies.First();
                string Approval = SYDocumentStatus.APPROVED.ToString();
                string Early = SYDocumentStatus.EARLY.ToString();
                string Late = SYDocumentStatus.LATE.ToString();
                var ListResLaEa = DB.HRReqLateEarlies.Where(w => w.Status == Approval).ToList();
                ListResLaEa = ListResLaEa.Where(w => w.LeaveDate.Date >= FromDate.Date && w.LeaveDate.Date <= ToDate.Date).ToList();
                if (IsDudSalary == true)
                {
                    foreach (var TranNo in _TranNo)
                    {

                        Header.EmpCode = TranNo;
                        var objMatch = DB.PREmpLateDeducts.Where(w => w.EmpCode == TranNo).ToList();
                        var ListAtt = ListHeader.Where(w => w.EmpCode == TranNo).ToList();
                        decimal MisCan = 0;
                        foreach (var item in ListAtt)
                        {
                            if (item.Start1.Value.Year == 1900 && item.End1.Value.Year != 1900)
                                MisCan += 0.5M;
                            else if (item.Start1.Value.Year != 1900 && item.End1.Value.Year == 1900)
                                MisCan += 0.5M;
                        }
                        decimal DayLa = 0;
                        decimal DayEa = 0;
                        DayLa = MisCan - Convert.ToDecimal(_Policy.MissScan);
                        if (DayLa <= 0) DayLa = 0;
                        if (HRSetting == null)
                        {
                            return "DEDCUTION_EN";
                        }
                        if (!ListDed.Where(w => w.Code == HRSetting.DeductLate).Any())
                        {
                            return "DEDCUTION_EN";
                        }
                        if (!ListDed.Where(w => w.Code == HRSetting.DeductEalary).Any())
                        {
                            return "DEDCUTION_EN";
                        }
                        decimal SumLa_min = 0;
                        decimal SumEa_min = 0;

                        var Late_Min = ListAtt.Sum(x => x.Late1) + ListAtt.Sum(x => x.Late2);
                        var Early_Min = ListAtt.Sum(x => x.Early1) + ListAtt.Sum(x => x.Early2);
                        var Late_Count = ListAtt.Where(w => w.Late1 > 0).Count() + ListAtt.Where(w => w.Late2 > 0).Count();
                        var Early_Count = ListAtt.Where(w => w.Early1 > 0).Count() + ListAtt.Where(w => w.Early2 > 0).Count();
                        var SumLa = Late_Count;
                        var SumEa = Early_Count;
                        SumLa_min = Convert.ToDecimal(Late_Min);
                        SumEa_min = Convert.ToDecimal(Early_Min);
                        var Count_Re_LaEa = ListResLaEa.Where(w => w.EmpCode == TranNo).Count();// - _Policy.IsLate_Early;
                        string RemarkLate = "";
                        string RemarkEarly = "";
                        if (LstLaEa.Count > 0)
                        {
                            //SumEa_min = 0;
                            //SumLa_min = 0;
                            if (LstLaEa.Where(w => w.Type == "TIME").Count() > 0)
                            {
                                var Result = LstLaEa.Where(w => w.DedType == Late && w.OTFrom <= SumLa && w.OTTo >= SumLa).
                                    OrderBy(x => x.OTTo).Sum(x => x.Rate);
                                RemarkLate = "(Late:" + Late_Count + ")";
                                if (Count_Re_LaEa > 0)
                                {
                                    Result += Convert.ToDecimal(Count_Re_LaEa);
                                }
                                DayLa += Result;

                                var Result1 = LstLaEa.Where(w => w.DedType == Early && w.OTFrom <= SumEa && w.OTTo >= SumEa).
                                    OrderBy(x => x.OTTo).Sum(x => x.Rate);
                                RemarkEarly = "(Early:" + Early_Count + ")";
                                DayEa += Result1;
                            }
                            else
                            {
                                var Result = LstLaEa.Where(w => w.DedType == Late && w.OTFrom <= SumLa_min && w.OTTo >= SumLa_min).ToList();
                                if (Result.Count() > 0)
                                {
                                    RemarkLate = "(Late: " + Late_Min + " Min)";
                                    DayLa += Result.First().Rate;
                                }
                                var Result1 = LstLaEa.Where(w => w.DedType == Early && w.OTFrom <= SumEa_min && w.OTTo >= SumEa_min).ToList();
                                if (Result1.Count() > 0)
                                {
                                    RemarkEarly = "(Ealry: " + Early_Min + " Min)";
                                    DayEa += Result1.First().Rate;
                                }
                            }
                        }
                        string Status_Ealary = HRSetting.DeductEalary;
                        string Status_Late = HRSetting.DeductLate;
                        if (SumEa_min + SumLa_min + DayLa + DayEa > 0)
                        {
                            #region Late
                            if (objMatch.Where(w => w.DedCode == Status_Late && w.FromDate == FromDate && w.ToDate == ToDate).Any())
                            {
                                var obj = objMatch.Where(w => w.DedCode == Status_Late && w.FromDate == FromDate && w.ToDate == ToDate).First();
                                obj.Minute = SumLa_min;
                                obj.Day = DayLa;
                                obj.ChangedBy = User.UserName;
                                obj.ChangedOn = DateTime.Now;
                                DB.PREmpLateDeducts.Attach(obj);
                                DB.Entry(obj).Property(w => w.Minute).IsModified = true;
                                DB.Entry(obj).Property(w => w.Day).IsModified = true;
                                DB.Entry(obj).Property(w => w.ChangedBy).IsModified = true;
                                DB.Entry(obj).Property(w => w.ChangedOn).IsModified = true;
                                int row = DB.SaveChanges();
                            }
                            else
                            {
                                var LateDeduct = new PREmpLateDeduct();
                                LateDeduct.EmpCode = TranNo;
                                LateDeduct.DedCode = Status_Late;
                                LateDeduct.Day = DayLa;
                                LateDeduct.Minute = SumLa_min;
                                LateDeduct.InMonth = ToDate;
                                LateDeduct.Remark = "Generate from attendance" + RemarkLate;
                                LateDeduct.FromDate = FromDate;
                                LateDeduct.ToDate = ToDate;
                                LateDeduct.CreateBy = User.UserName;
                                LateDeduct.CreateOn = DateTime.Now;
                                LateDeduct.Status = 1;
                                DB.PREmpLateDeducts.Add(LateDeduct);
                                int row = DB.SaveChanges();
                            }
                            #endregion
                            #region Ealary
                            if (objMatch.Where(w => w.DedCode == Status_Ealary && w.FromDate == FromDate && w.ToDate == ToDate).Any())
                            {
                                var obj = objMatch.Where(w => w.DedCode == Status_Ealary && w.FromDate == FromDate && w.ToDate == ToDate).First();
                                obj.Minute = SumEa_min;
                                obj.Day = DayEa;
                                obj.ChangedBy = User.UserName;
                                obj.ChangedOn = DateTime.Now;
                                DB.PREmpLateDeducts.Attach(obj);
                                DB.Entry(obj).Property(w => w.Minute).IsModified = true;
                                DB.Entry(obj).Property(w => w.Day).IsModified = true;
                                DB.Entry(obj).Property(w => w.ChangedBy).IsModified = true;
                                DB.Entry(obj).Property(w => w.ChangedOn).IsModified = true;
                                int row = DB.SaveChanges();
                            }
                            else
                            {
                                var LateDeduct = new PREmpLateDeduct();
                                LateDeduct.EmpCode = TranNo;
                                LateDeduct.DedCode = Status_Ealary;
                                LateDeduct.Day = DayEa;
                                LateDeduct.Minute = SumEa_min;
                                LateDeduct.InMonth = ToDate;
                                LateDeduct.Remark = "Generate from attendance" + RemarkEarly;
                                LateDeduct.FromDate = FromDate;
                                LateDeduct.ToDate = ToDate;
                                LateDeduct.CreateBy = User.UserName;
                                LateDeduct.CreateOn = DateTime.Now;
                                LateDeduct.Status = 1;
                                DB.PREmpLateDeducts.Add(LateDeduct);
                                int row = DB.SaveChanges();
                            }
                            #endregion
                        }
                        else
                        {
                            var Result = objMatch.FirstOrDefault(w => w.DedCode == Status_Late && w.FromDate == FromDate && w.ToDate == ToDate);
                            if (Result != null)
                            {
                                DB.PREmpLateDeducts.Attach(Result);
                                DB.Entry(Result).State = EntityState.Deleted;
                                var rowD = DB.SaveChanges();
                            }
                            var Result1 = objMatch.FirstOrDefault(w => w.DedCode == Status_Ealary && w.FromDate == FromDate && w.ToDate == ToDate);
                            if (Result1 != null)
                            {
                                DB.PREmpLateDeducts.Attach(Result1);
                                DB.Entry(Result1).State = EntityState.Deleted;
                                var rowD = DB.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    var _ListStaff = DB.HRStaffProfiles.ToList();
                    var Para = DB.PRParameters.ToList();
                    var _lstEmpLeave = DB.HREmpLeaves.ToList();
                    var _lstEmployeeD = DB.HREmpLeaveDs.ToList();

                    int Increment;
                    if (_lstEmpLeave.Count == 0) Increment = 0;
                    else Increment = _lstEmpLeave.Max(w => w.Increment);
                    int i = 0;
                    var resLeave = _lstEmpLeave.Where(w => w.TranType == true).ToList();
                    var resLeaveD = _lstEmployeeD.Where(w => resLeave.Where(x => x.Increment == w.LeaveTranNo).Any()).ToList();
                    foreach (var TranNo in _TranNo)
                    {
                        Header.EmpCode = TranNo;
                        var ListAtt = ListHeader.Where(w => w.EmpCode == TranNo).ToList();
                        var Late_Min = ListAtt.Sum(x => x.Late1) + ListAtt.Sum(x => x.Late2);
                        var Early_Min = ListAtt.Sum(x => x.Early1) + ListAtt.Sum(x => x.Early2);
                        var Late_Count = ListAtt.Where(w => w.Late1 > 0).Count() + ListAtt.Where(w => w.Late2 > 0).Count();
                        var Early_Count = ListAtt.Where(w => w.Early1 > 0).Count() + ListAtt.Where(w => w.Early2 > 0).Count();
                        decimal LeaveDay = 0.00M;
                        decimal LeaveHour = 0.00M;
                        var Staff = _ListStaff.FirstOrDefault(w => w.EmpCode == TranNo);
                        var Par = Para.FirstOrDefault(w => w.Code == Staff.PayParam);
                        if (LstLaEa.Count > 0)
                        {
                            if (LstLaEa.Where(w => w.Type == "TIME").Count() > 0)
                            {
                                var Result = LstLaEa.Where(w => w.DedType == Late && w.OTFrom <= Late_Count && w.OTTo >= Late_Count).
                                    OrderBy(x => x.OTTo).Sum(x => x.Rate);
                                LeaveDay += Result;

                                var Result1 = LstLaEa.Where(w => w.DedType == Early && w.OTFrom <= Early_Count && w.OTTo >= Early_Count).
                                    OrderBy(x => x.OTTo).Sum(x => x.Rate);
                                LeaveDay += Result1;
                            }
                            else
                            {
                                var Result = LstLaEa.Where(w => w.DedType == Late && w.OTFrom <= Late_Min && w.OTTo >= Late_Min).ToList();
                                if (Result.Count() > 0)
                                {
                                    LeaveDay += Result.First().Rate;
                                }
                                var Result1 = LstLaEa.Where(w => w.DedType == Early && w.OTFrom <= Early_Min && w.OTTo >= Early_Min).ToList();
                                if (Result1.Count() > 0)
                                {
                                    LeaveDay += Result1.First().Rate;
                                }
                            }
                        }
                        else
                        {
                            LeaveHour = Math.Round(Convert.ToDecimal((Late_Min + Early_Min) / 60.00), 2);
                            LeaveDay = LeaveHour / Convert.ToDecimal(Par.WHOUR);
                        }
                        if (LeaveDay > 1)
                        {

                        }

                        var DEmpLeave = resLeave.Where(w => w.EmpCode == TranNo && ((w.FromDate >= FromDate && w.FromDate <= ToDate) || (w.ToDate >= FromDate && w.ToDate <= ToDate) ||
                        (FromDate >= w.FromDate && FromDate <= w.ToDate) || (ToDate >= w.FromDate && ToDate <= w.ToDate))).ToList();
                        var DEmpLeaveD = resLeaveD.Where(w => DEmpLeave.Where(x => x.Increment == w.LeaveTranNo).Any()).ToList();
                        foreach (var item in DEmpLeave)
                        {
                            var objEmp = DEmpLeaveD.Where(w => w.LeaveTranNo == item.Increment).ToList();
                            foreach (var item1 in objEmp)
                            {
                                DBD.HREmpLeaveDs.Attach(item1);
                                DBD.Entry(item1).State = EntityState.Deleted;
                            }
                            DBD.HREmpLeaves.Attach(item);
                            DBD.Entry(item).State = EntityState.Deleted;
                        }
                        foreach (var item in ListAtt)
                        {
                            var Obj = new ATEmpSchedule();
                            Obj = item;
                            i += 1;
                            HREmpLeave empLeave = new HREmpLeave();
                            empLeave.EmpCode = item.EmpCode;
                            empLeave.Increment = Increment + i;
                            if (item.LeaveDesc != "")
                                empLeave.LeaveCode = item.LeaveDesc;
                            else if (item.LeaveCode != "")
                                empLeave.LeaveCode = item.LeaveCode;
                            empLeave.FromDate = item.TranDate;
                            empLeave.ToDate = item.TranDate;
                            empLeave.NoDay = 1;
                            empLeave.TranType = true;
                            empLeave.NoPH = 0;
                            empLeave.NoRest = 0;
                            empLeave.Reason = "Transfer from attendance";
                            empLeave.RequestDate = item.TranDate;
                            empLeave.CreateBy = User.UserName;
                            empLeave.CreateOn = DateTime.Now;
                            empLeave.Status = SYDocumentStatus.APPROVED.ToString();
                            DB.HREmpLeaves.Add(empLeave);
                            HREmpLeaveD EmpLeaveD = new HREmpLeaveD();
                            EmpLeaveD.LeaveTranNo = Increment + i;
                            EmpLeaveD.LineItem = 1;
                            EmpLeaveD.EmpCode = item.EmpCode;
                            EmpLeaveD.LeaveCode = LeaveType;
                            EmpLeaveD.LeaveDate = item.TranDate;
                            EmpLeaveD.CutMonth = item.TranDate;
                            EmpLeaveD.Status = "Leave";
                            EmpLeaveD.LHour = Par.WHOUR;
                            EmpLeaveD.Remark = "FullDay";
                            EmpLeaveD.CreateBy = User.UserName;
                            EmpLeaveD.CreateOn = DateTime.Now;
                            Obj = item;
                            Obj.LeaveCode = item.LeaveDesc;
                            DB.HREmpLeaveDs.Add(EmpLeaveD);
                            DBI.ATEmpSchedules.Attach(Obj);
                            DBI.Entry(Obj).Property(w => w.LeaveCode).IsModified = true;
                        }
                    }
                    //DBD.SaveChanges();
                    //DBI.SaveChanges();
                    //var row = DB.SaveChanges();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }

            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
                DBI.Configuration.AutoDetectChangesEnabled = true;
                DBD.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string TransferLeave(string ID)
        {
            if (ID.Trim() == "")
            {
                return "INV_DOC";
            }
            var DBI = new HumicaDBContext();
            try
            {
                DBI.Configuration.AutoDetectChangesEnabled = false;
                var ListEmpLeave = new List<HREmpLeave>();
                try
                {
                    Header = new ATEmpSchedule();
                    ListHeader = new List<ATEmpSchedule>();
                    string[] _TranNo = ID.Split(';');
                    var LeaveType = DB.HRLeaveTypes.ToList();
                    var period = DB.ATPayperiods.FirstOrDefault(w => w.PeriodID == FMonthly.PayPeriodId);
                    DateTime PFromDate = period.FromDate;
                    DateTime PToDate = period.ToDate;

                    ListHeader = DB.ATEmpSchedules.Where(w => w.TranDate >= PFromDate.Date
                    && w.TranDate <= PToDate.Date).ToList();
                    ListHeader = ListHeader.Where(w => LeaveType.Where(x => x.Code == w.SHIFT || x.Code == w.LeaveDesc).Any()).ToList();
                    var _ListStaff = DB.HRStaffProfiles.ToList();
                    var Para = DB.PRParameters.ToList();
                    var _lstEmpLeave = DB.HREmpLeaves.Where(w => ((EntityFunctions.TruncateTime(w.FromDate) >= PFromDate.Date && EntityFunctions.TruncateTime(w.FromDate) <= PToDate.Date) ||
                        (EntityFunctions.TruncateTime(w.ToDate) >= PFromDate.Date && EntityFunctions.TruncateTime(w.ToDate) <= PToDate.Date) ||
                        (PFromDate.Date >= EntityFunctions.TruncateTime(w.FromDate) && PFromDate.Date <= EntityFunctions.TruncateTime(w.ToDate))
                        || (PToDate.Date >= EntityFunctions.TruncateTime(w.FromDate) && PToDate.Date <= EntityFunctions.TruncateTime(w.ToDate)))).ToList();


                    var _lstEmployeeD = DB.HREmpLeaveDs.Where(w => w.CutMonth >= PFromDate.Date
                    && w.CutMonth <= PToDate.Date).ToList();
                    var resLeave = _lstEmpLeave.ToList();

                    int Increment = GenerateLeaveObject.GetLastIncrement();
                    //if (resLeave.Count == 0) Increment = 0;
                    //else Increment = resLeave.Max(w => w.Increment);
                    int i = 0;
                    var resLeaveD = _lstEmployeeD.Where(w => resLeave.Where(x => w.LeaveTranNo == x.Increment && w.EmpCode == x.EmpCode).Any()).ToList();
                    foreach (var TranNo in _TranNo)
                    {
                        DateTime FromDate = PFromDate;
                        DateTime ToDate = PToDate;
                        var Staff = _ListStaff.FirstOrDefault(w => w.EmpCode == TranNo);
                        if (Staff.StartDate > FromDate) FromDate = Staff.StartDate.Value;
                        if (Staff.DateTerminate.Year != 1900)
                        {
                            if (Staff.DateTerminate < ToDate) ToDate = Staff.DateTerminate;
                        }
                        Header.EmpCode = TranNo;
                        var ListAtt = ListHeader.Where(w => w.EmpCode == TranNo && w.TranDate >= FromDate && w.TranDate <= ToDate).ToList();
                        var Par = Para.FirstOrDefault(w => w.Code == Staff.PayParam);
                        var DEmpLeave = resLeave.Where(w => w.EmpCode == TranNo && ((w.FromDate >= FromDate && w.FromDate <= ToDate) || (w.ToDate >= FromDate && w.ToDate <= ToDate) ||
                                                        (FromDate >= w.FromDate && FromDate <= w.ToDate) || (ToDate >= w.FromDate && ToDate <= w.ToDate)) && w.TranType == true).ToList();
                        var DEmpLeaveD = resLeaveD.Where(w => DEmpLeave.Where(x => x.Increment == w.LeaveTranNo && x.EmpCode == w.EmpCode).Any()).ToList();
                        foreach (var item in DEmpLeave)
                        {
                            var objEmp = DEmpLeaveD.Where(w => w.LeaveTranNo == item.Increment && w.EmpCode == item.EmpCode).ToList();
                            foreach (var item1 in objEmp)
                            {
                                DBI.HREmpLeaveDs.Attach(item1);
                                DBI.Entry(item1).State = System.Data.Entity.EntityState.Deleted;
                            }
                            DBI.HREmpLeaves.Attach(item);
                            DBI.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        }
                        foreach (var item in ListAtt)
                        {
                            var Leave_ = resLeave.Where(w => w.EmpCode == TranNo && item.TranDate >= w.FromDate && item.TranDate <= w.ToDate && w.TranType != true).ToList();
                            if (Leave_.Count > 0) continue;
                            var Obj = new ATEmpSchedule();
                            Obj = item;
                            i += 1;
                            HREmpLeave empLeave = new HREmpLeave();
                            empLeave.EmpCode = item.EmpCode;
                            empLeave.Increment = Increment + i;
                            if (item.LeaveDesc != "")
                                empLeave.LeaveCode = item.LeaveDesc;
                            else if (item.LeaveCode != "")
                                empLeave.LeaveCode = item.LeaveCode;
                            else if (LeaveType.Where(w => w.Code == item.SHIFT.Trim()).Any())
                                empLeave.LeaveCode = item.SHIFT.Trim();

                            empLeave.FromDate = item.TranDate;
                            empLeave.ToDate = item.TranDate;
                            empLeave.NoDay = 1;
                            //if (LeaveType.Where(w => w.IsParent == true && w.Code == empLeave.LeaveCode).Any())
                            //{
                            //    empLeave.NoDay = 0.5;
                            //}
                            empLeave.TranType = true;
                            empLeave.Units = "Day";
                            empLeave.NoPH = 0;
                            empLeave.NoRest = 0;
                            empLeave.Reason = "Transfer from time and attendance";
                            empLeave.RequestDate = item.TranDate;
                            empLeave.CreateBy = User.UserName;
                            empLeave.CreateOn = DateTime.Now;
                            empLeave.Status = SYDocumentStatus.APPROVED.ToString();
                            DBI.HREmpLeaves.Add(empLeave);
                            HREmpLeaveD EmpLeaveD = new HREmpLeaveD();
                            EmpLeaveD.LeaveTranNo = Increment + i;
                            EmpLeaveD.LineItem = 1;
                            EmpLeaveD.EmpCode = item.EmpCode;
                            EmpLeaveD.LeaveCode = empLeave.LeaveCode;
                            EmpLeaveD.LeaveDate = item.TranDate;
                            EmpLeaveD.CutMonth = item.TranDate;
                            EmpLeaveD.Status = "Leave";
                            EmpLeaveD.LHour = Par.WHOUR;
                            EmpLeaveD.Remark = "FullDay";
                            //if (LeaveType.Where(w => w.IsParent == true && w.Code == empLeave.LeaveCode).Any())
                            //{
                            //    //if (item.Remark == "Morning" || item.Remark == "Afternoon")
                            //    EmpLeaveD.LHour = Par.WHOUR / 2.00M;
                            //    EmpLeaveD.Remark = "Morning";
                            //}
                            EmpLeaveD.CreateBy = User.UserName;
                            EmpLeaveD.CreateOn = DateTime.Now;
                            Obj = item;
                            Obj.LeaveCode = item.LeaveDesc;
                            DBI.HREmpLeaveDs.Add(EmpLeaveD);
                            DBI.ATEmpSchedules.Attach(Obj);
                            DBI.Entry(Obj).Property(w => w.LeaveCode).IsModified = true;

                            ListEmpLeave.Add(empLeave);
                        }
                    }
                    var row = DBI.SaveChanges();

                    LM.GenerateLeaveObject leave = new LM.GenerateLeaveObject();
                    //var EmpGroup = (from Emp in ListEmpLeave
                    //               group Emp by new { Emp.EmpCode, Emp.RequestDate.Value.Year } into g
                    //               select new { EmpCode = g.Key.EmpCode, Year = g.Key.Year }).ToList();
                    foreach (var item in ListEmpLeave)
                    {
                        int Year = item.FromDate.Year;
                        if (item.FromDate.Month == 12 && item.FromDate.Day > 19)
                            Year = item.FromDate.AddYears(1).Year;
                        leave.GET_Leave_LeaveBalance(item.EmpCode, Year);
                    }
                    return SYConstant.OK;
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DBI.Configuration.AutoDetectChangesEnabled = true;
                //DBI.Configuration.AutoDetectChangesEnabled = true;
                //DBD.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string TransferWorkingHour(string ID)
        {
            try
            {
                DB = new HumicaDBContext();
                DB.Configuration.AutoDetectChangesEnabled = false;
                Header = new ATEmpSchedule();
                ListHeader = new List<ATEmpSchedule>();
                string[] _TranNo = ID.Split(';');
                var Shift = DB.ATShifts.ToList();
                var Parameter = DB.PRParameters.ToList();
                var Staff = DB.HRStaffProfiles.ToList();
                var period = DB.ATPayperiods.FirstOrDefault(w => w.PeriodID == FMonthly.PayPeriodId);
                DateTime FromDate = period.FromDate;
                DateTime ToDate = period.ToDate;
                foreach (var TranNo in _TranNo)
                {
                    var _Staffs = Staff.FirstOrDefault(w => w.EmpCode == TranNo && w.IsAtten == true);
                    if (_Staffs == null) continue;
                    Header.EmpCode = TranNo;
                    ListHeader = DB.ATEmpSchedules.Where(w => w.EmpCode == TranNo).ToList();
                    ListHeader = ListHeader.Where(w => w.TranDate.Date >= FromDate.Date && w.TranDate.Date <= ToDate.Date).ToList();
                    decimal WorkingHour = 0;
                    decimal Hours = 0;
                    var _InOut = DB.ATPolicies.FirstOrDefault();
                    string[] workStatus = { WorkStatus.OFF, WorkStatus.PH };
                    foreach (var item in ListHeader)
                    {
                        string shiftCode = item.SHIFT.ToUpper();
                        if (!workStatus.Contains(shiftCode))
                        {
                            if (item.Flag1 == 1 && item.Flag2 == 2)
                            {
                                if (item.Start1.Value.Year == 1900 && item.End1.Value.Year == 1900 &&
                                    item.Start2.Value.Year == 1900 && item.End2.Value.Year == 1900)
                                    continue;

                                if ((item.Start1.Value.Year != 1900 && item.End1.Value.Year == 1900) |
                                    (item.Start1.Value.Year == 1900 && item.End1.Value.Year != 1900))
                                    WorkingHour += 0.5M;

                                if ((item.Start2.Value.Year != 1900 && item.End2.Value.Year == 1900) |
                                    (item.Start2.Value.Year == 1900 && item.End2.Value.Year != 1900))
                                    WorkingHour += 0.5M;

                            }
                            else
                            {
                                if (item.Start1.Value.Year == 1900 && item.End1.Value.Year == 1900) continue;
                                else
                                {
                                    if (item.WHOUR <= 5) WorkingHour += 0.5M;
                                    else WorkingHour += 1;
                                }
                            }
                        }
                        //    Hours += Convert.ToDecimal(item.WHOUR);
                        //    DateTime EndDate = Convert.ToDateTime(item.End1);
                        //    var _par = Parameter.FirstOrDefault(w => w.Code == _Staffs.PayParam);
                        //    if (Shift.Where(w => w.Code == item.SHIFT).Any())
                        //    {
                        //        var _Time = Shift.FirstOrDefault(w => w.Code == item.SHIFT);
                        //        var Eal = EndDate.AddMinutes(Convert.ToDouble(_InOut.Early));
                        //        if (Eal.TimeOfDay >= _Time.CheckOut1.Value.TimeOfDay)
                        //        {
                        //            WorkingHour += 1;
                        //        }
                        //        else if (Eal.TimeOfDay >= _Time.BreakStart.Value.TimeOfDay && _Time.BreakEnd.Value > _Time.BreakStart.Value
                        //         && Eal.TimeOfDay <= _Time.BreakEnd.Value.TimeOfDay)
                        //        {
                        //            WorkingHour += 0.5M;
                        //        }
                        //        else WorkingHour += Convert.ToDecimal(item.WHOUR / _par.WHOUR);
                        //    }
                        //    else if (item.SHIFT == "OFF" || item.SHIFT == "PH")
                        //    {
                        //        //if (item.WHOUR.Value >= _par.WHOUR) WorkingHour += 1;
                        //        //else WorkingHour += Convert.ToDecimal(item.WHOUR / _par.WHOUR);
                        //    }
                        //}
                    }
                    var objMatch = DB.PREmpWorkings.Where(w => w.EmpCode == TranNo && w.PayPeriodId == period.PeriodID).ToList();
                    if (objMatch.Count() > 0)
                    {
                        var obj = objMatch.First();
                        obj.ActWorkDay = WorkingHour;
                        obj.Hours = Hours;
                        obj.FromDate = FromDate;
                        obj.ToDate = ToDate;
                        obj.ChangedBy = User.UserName;
                        obj.ChangedOn = DateTime.Now;
                        DB.PREmpWorkings.Attach(obj);
                        DB.Entry(obj).Property(w => w.ActWorkDay).IsModified = true;
                        DB.Entry(obj).Property(w => w.Hours).IsModified = true;
                        DB.Entry(obj).Property(w => w.FromDate).IsModified = true;
                        DB.Entry(obj).Property(w => w.ToDate).IsModified = true;
                        DB.Entry(obj).Property(w => w.ChangedBy).IsModified = true;
                        DB.Entry(obj).Property(w => w.ChangedOn).IsModified = true;
                        //int row = DB.SaveChanges();
                    }
                    else
                    {
                        if (WorkingHour > 0)
                        {
                            var _obj = new PREmpWorking();
                            _obj.EmpCode = TranNo;
                            _obj.PayPeriodId = period.PeriodID;
                            _obj.InYear = ToDate.Year;
                            _obj.InMonth = ToDate.Month;
                            _obj.FromDate = FromDate;
                            _obj.ToDate = ToDate;
                            _obj.ActWorkDay = WorkingHour;
                            _obj.Hours = Hours;
                            _obj.Remark = "Get data from attendance";
                            _obj.CreatedBy = User.UserName;
                            _obj.CreatedOn = DateTime.Now;
                            DB.PREmpWorkings.Add(_obj);
                        }
                    }

                    int row = DB.SaveChanges();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public  string DeleteDoc(string DocumentNo)
        {
            if (DocumentNo==null)
            {
                return "NO_DATA";
            }
            try
            {
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                var Doc = DB.ATImpRosterHeaders.Where(w => w.DocumentNo == DocumentNo && w.Status== Cancel).FirstOrDefault();
                if(Doc != null)
                {
                    if (Doc.Status != Cancel)
                    {
                        return "DOC_INV";
                    }
                    else
                    {
                        DB.ATImpRosterHeaders.Attach(Doc);
                        DB.Entry(Doc).State = System.Data.Entity.EntityState.Deleted;
                    }
                }
                int row = DB.SaveChanges();
                return SYSConstant.OK;
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }

        }
        private async Task<string> Delete(List<Temp_Roster> lsttemp_Rosters)
        {
            if (lsttemp_Rosters.Count == 0)
            {
                return "NO_DATA";
            }
            var lstAttendance = await DB.ATEmpSchedules.ToListAsync();
            lstAttendance = lstAttendance.ToList();
            lstAttendance = lstAttendance.Where(w => lsttemp_Rosters.Where(x => x.EmpCode == w.EmpCode &&
                               w.TranDate.Date == x.InDate.Date).Any()).ToList();
            int Counts = 0;
            try
            {

                DB.Configuration.AutoDetectChangesEnabled = false;
                foreach (var item in lstAttendance)
                {
                    Counts += 1;
                    DB.ATEmpSchedules.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                int row = await DB.SaveChangesAsync();
                return SYSConstant.OK;
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }

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
            ObjEmpSch.SHIFT = _Roster.Shift.Trim().ToUpper();
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

            else if (_Roster.Shift.Trim().ToUpper() == "PH" || _Roster.Shift.Trim().ToUpper() == "OFF")
            {

            }
            else if (ListLeaveType.Where(w => w.Code == _Roster.Shift.ToString()).Count() > 0)
            {
                LeaveDesc = _Roster.Shift.Trim().ToUpper();
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
        public async Task<string> GenerateMeal(int PayPeriodId)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                Header = new ATEmpSchedule();
                ListHeader = new List<ATEmpSchedule>();

                var PeriodId = DB.ATPayperiods.FirstOrDefault(w => w.PeriodID == PayPeriodId);
                if (PeriodId == null) return "PERIOD";
                var _PFromDate = PeriodId.FromDate;
                var _PToDate = PeriodId.ToDate;

                List<HRStaffProfile> ListStaff = DB.HRStaffProfiles.Where(w => w.IsMealAllowance == true).ToList();
                var MealSetup = await DB.ATMealSetups.FirstOrDefaultAsync();
                var MealSetupItem = await DB.ATMealSetupItems.ToListAsync();
                ListStaff = ListStaff.Where(w => MealSetupItem.Where(x => x.LevelCode == w.LevelCode).Any()).ToList();

                var _listShift = DB.ATShifts.Where(w => w.BreakFast == true || w.Lunch == true || w.Dinner == true || w.NightMeal == true).ToList();


                var _list = DB.ATEmpSchedules.Where(w => EntityFunctions.TruncateTime(w.TranDate) >= _PFromDate.Date && EntityFunctions.TruncateTime(w.TranDate) <= _PToDate.Date).ToList();
                _list = _list.Where(w => _listShift.Where(x => x.Code == w.SHIFT).Any()).ToList();
                _list = _list.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();

                var FromDate = PeriodId.FromDate.AddDays(-1);
                var ToDate = PeriodId.ToDate.AddDays(1);
                var ListInOut = (from s in DB.ATInOuts
                                  where EntityFunctions.TruncateTime(s.FullDate) >= FromDate.Date
                                  && EntityFunctions.TruncateTime(s.FullDate) <= ToDate.Date
                                  && s.EmpCode.Trim() != ""
                                  select s).ToList();
                ListInOut = ListInOut.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();

                var _GenMeal = DB.ATGenMeals.Where(w => w.PayPeriodID == PeriodId.PeriodID).ToList();
                foreach (var _Meal in _GenMeal)
                {
                    DB.ATGenMeals.Attach(_Meal);
                    DB.Entry(_Meal).State = EntityState.Deleted;
                }
                List<ATGenMeal> ListMeal = new List<ATGenMeal>();
                var _Empsch = _list.ToList();
                foreach (var item in _Empsch)
                {
                    DateTime PFromDate = item.TranDate;

                    var Staff = ListStaff.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                    var MealRate = MealSetupItem.FirstOrDefault(w => w.LevelCode == w.LevelCode);
                    var CheckInOut = ListInOut.Where(w => w.FullDate.Date >= PFromDate.AddDays(-1) && w.FullDate.Date <= PFromDate.AddDays(1)).ToList();

                    Header.EmpCode = item.EmpCode;
                    var _shift = _listShift.FirstOrDefault(w => w.Code == item.SHIFT);
                    GenerateMeal(item, Staff, _shift, MealSetup, CheckInOut);
                    var Obj = new ATGenMeal();
                    Obj.BreakFast = item.BreakFast;
                    Obj.Lunch = item.Lunch;
                    Obj.Dinner = item.Dinner;
                    Obj.EmpCode = Staff.EmpCode;
                    Obj.EmpName = Staff.AllName;
                    if (ListMeal.Where(w => w.EmpCode == item.EmpCode).Any())
                    {
                        ListMeal.Where(w => w.EmpCode == item.EmpCode).ToList().ForEach(w => w.BreakFast += item.BreakFast);
                        ListMeal.Where(w => w.EmpCode == item.EmpCode).ToList().ForEach(w => w.Lunch += item.Lunch);
                        ListMeal.Where(w => w.EmpCode == item.EmpCode).ToList().ForEach(w => w.Dinner += item.Dinner);
                    }
                    else
                    {
                        Obj.BreakFastRate = MealRate.BreakFast;
                        Obj.LunchRate = MealRate.Lunch;
                        Obj.DinnerRate = MealRate.Dinner;
                        ListMeal.Add(Obj);
                    }

                    DB.ATEmpSchedules.Attach(item);
                    DB.Entry(item).Property(w => w.BreakFast).IsModified = true;
                    DB.Entry(item).Property(w => w.Lunch).IsModified = true;
                    DB.Entry(item).Property(w => w.Dinner).IsModified = true;
                    DB.Entry(item).Property(w => w.NightMeal).IsModified = true;
                }
                foreach(var _Meal in ListMeal)
                {
                    var Obj = new ATGenMeal();
                    Obj = _Meal;
                    Obj.PayPeriodID = PeriodId.PeriodID;
                    Obj.FromDate = _PFromDate;
                    Obj.ToDate = _PToDate;
                    Obj.BreakFastAM = Obj.BreakFast * Obj.BreakFastRate;
                    Obj.LunchAM = Obj.Lunch * Obj.LunchRate;
                    Obj.DinnerAM = Obj.Dinner * Obj.DinnerRate;
                    Obj.Amount = Obj.BreakFastAM + Obj.LunchAM + Obj.DinnerAM;
                    DB.ATGenMeals.Add(Obj);
                }
                int row = DB.SaveChanges();
                return SYConstant.OK;

            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }

            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        public ATEmpSchedule GenerateMeal(ATEmpSchedule Atenn, HRStaffProfile staffProfile, ATShift Shift,
            ATMealSetup MealSetup, List<ATInOut> CheckInOut)
        {
            Atenn.BreakFast = 0;
            Atenn.Lunch = 0;
            Atenn.Dinner = 0;
            Atenn.NightMeal = 0;
            if (staffProfile.IsMealAllowance == true && Shift != null && MealSetup != null)
            {
                DateTime tempDate = Atenn.TranDate;

                if (Shift.BreakFast == true && MealSetup.BreakfastFrom.HasValue && MealSetup.BreakfastTo.HasValue)
                {
                    DateTime DFrom = DateTimeHelper.SetTimeInDate(tempDate, MealSetup.BreakfastFrom.Value);
                    DateTime DTo = DateTimeHelper.SetTimeInDate(tempDate, MealSetup.BreakfastTo.Value);

                    int countMeal = CheckInOut.Where(x => x.EmpCode == Atenn.EmpCode & x.FullDate >= DFrom & x.FullDate <= DTo).Count();
                    if (countMeal > 0)
                        Atenn.BreakFast = 1;
                }
                if (Shift.Lunch == true && MealSetup.LunchFrom.HasValue && MealSetup.LunchTo.HasValue)
                {
                    DateTime DFrom = DateTimeHelper.SetTimeInDate(tempDate, MealSetup.LunchFrom.Value);
                    DateTime DTo = DateTimeHelper.SetTimeInDate(tempDate, MealSetup.LunchTo.Value);

                    int countMeal = CheckInOut.Where(x => x.EmpCode == Atenn.EmpCode & x.FullDate >= DFrom & x.FullDate <= DTo).Count();
                    if (countMeal > 0)
                        Atenn.Lunch = 1;
                }
                if (Shift.Dinner == true && MealSetup.DinnerFrom.HasValue && MealSetup.DinnerTo.HasValue)
                {
                    DateTime DFrom = DateTimeHelper.SetTimeInDate(tempDate, MealSetup.DinnerFrom.Value);
                    DateTime DTo = DateTimeHelper.SetTimeInDate(tempDate, MealSetup.DinnerTo.Value);

                    int countMeal = CheckInOut.Where(x => x.EmpCode == Atenn.EmpCode & x.FullDate >= DFrom & x.FullDate <= DTo).Count();
                    if (countMeal > 0)
                        Atenn.Dinner = 1;
                }
                if (Shift.NightMeal == true && MealSetup.NightFrom.HasValue && MealSetup.NightTo.HasValue)
                {
                    DateTime DFrom = DateTimeHelper.SetTimeInDate(tempDate, MealSetup.NightFrom.Value);
                    DateTime DTo = DateTimeHelper.SetTimeInDate(tempDate, MealSetup.NightTo.Value);

                    int countMeal = CheckInOut.Where(x => x.EmpCode == Atenn.EmpCode & x.FullDate >= DFrom & x.FullDate <= DTo).Count();
                    if (countMeal > 0)
                        Atenn.NightMeal = 1;
                }
            }
            return Atenn;
        }
        public string TransferMeal(string ID, int payperiod)
        {
            DB = new HumicaDBContext();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                var PeriodId = DB.ATPayperiods.FirstOrDefault(w => w.PeriodID == payperiod);
                if (PeriodId == null) return "PERIOD";

                DateTime FromDate = DateTimeHelper.StartDateOfMonth(PeriodId.ToDate);
                DateTime ToDate = DateTimeHelper.DateInMonth(FromDate);
                DateTime tempFromDate = FromDate;
                List<HRStaffProfile> ListStaff = DB.HRStaffProfiles.ToList();
                List<PRParameter> ListParameter = DB.PRParameters.ToList();


                var ListReward = DB.PR_RewardsType.Where(w => w.ReCode == "ALLW").ToList();
                var MealSetup = DB.ATMealSetups.FirstOrDefault();
                var MealAllowanceType = ListReward.FirstOrDefault(w => w.Code == MealSetup.MealAllowanceType);
                if (MealAllowanceType == null) return "MealAllowanceType";


                var EmpAllw = DB.PREmpAllws.Where(w => w.InvoceNo == payperiod.ToString()).ToList();
                foreach (var item in EmpAllw)
                {
                    DB.PREmpAllws.Attach(item);
                    DB.Entry(item).State = EntityState.Deleted;
                }
                string[] _TranNo = ID.Split(',');
                var ListMealGen = DB.ATGenMeals.Where(w => w.PayPeriodID == payperiod && w.Amount > 0).ToList();
                foreach (var TranNo in _TranNo)
                {
                    var Staff = ListStaff.FirstOrDefault(w => w.EmpCode == TranNo);
                    var Parameter = ListParameter.FirstOrDefault(w => w.Code == Staff.PayParam);
                    if (!Parameter.IsPrevoiuseMonth.IsNullOrZero())
                    {
                        DateTime tempDate = tempFromDate.AddMonths(-1);
                        FromDate = new DateTime(tempDate.Year, tempDate.Month, Parameter.FROMDATE.Value().Day);
                        ToDate = new DateTime(ToDate.Year, ToDate.Month, Parameter.TODATE.Value().Day);
                    }

                    var _MealGen = ListMealGen.FirstOrDefault(w => w.EmpCode == TranNo);
                    if (_MealGen == null) continue;
                    _MealGen.IsTransfer = true;
                    DB.ATGenMeals.Attach(_MealGen);
                    DB.Entry(_MealGen).Property(w => w.IsTransfer).IsModified = true;
                    //Allowance
                    var objAllw = new PREmpAllw();
                    objAllw.EmpCode = _MealGen.EmpCode;
                    objAllw.EmpName = _MealGen.EmpName;
                    objAllw.Amount = _MealGen.Amount;
                    objAllw.Status = 0;
                    objAllw.FromDate = FromDate;
                    objAllw.ToDate = ToDate;
                    objAllw.AllwCode = MealAllowanceType.Code;
                    objAllw.AllwDescription = MealAllowanceType.Description;
                    objAllw.Remark = "Transfer from Attendance";
                    objAllw.CreateBy = User.UserName;
                    objAllw.CreateOn = DateTime.Now;
                    objAllw.InvoceNo = payperiod.ToString();
                    DB.PREmpAllws.Add(objAllw);
                }
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = payperiod.ToString();
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = payperiod.ToString();
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = payperiod.ToString();
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }

            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public void GetLeaveAuto(HREmpLeave empLeave, HREmpLeaveD empLeaveD, decimal WHour, DateTime InDate, int Increment, string leaveType, string EmpCode)
        {
            empLeaveD.EndTime = DateTimeHelper.DateInHourMin(empLeaveD.EndTime.Value);
            empLeaveD.StartTime = DateTimeHelper.DateInHourMin(empLeaveD.StartTime.Value);

            var totals = empLeaveD.EndTime.Value.Subtract(empLeaveD.StartTime.Value).TotalHours;
            var totalMin = empLeaveD.EndTime.Value.Subtract(empLeaveD.StartTime.Value).TotalMinutes;
            empLeaveD.LHour = (decimal)totals;
            var NoDay = Math.Round(((decimal)totals / WHour), 3);
            empLeave.EmpCode = EmpCode;
            empLeave.Increment = Increment;
            empLeave.LeaveCode = leaveType;
            empLeave.FromDate = InDate;
            empLeave.ToDate = InDate;
            empLeave.NoDay = (double)NoDay;
            empLeave.TranType = true;
            empLeave.NoPH = 0;
            empLeave.NoRest = 0;
            empLeave.Reason = "Transfer from attendance (Minute:" + totalMin + ")";
            empLeave.RequestDate = InDate;
            empLeave.CreateBy = "System";
            empLeave.CreateOn = DateTime.Now;
            empLeave.Units = "Hours";
            empLeave.Status = SYDocumentStatus.APPROVED.ToString();

            empLeaveD.LeaveTranNo = Increment;
            empLeaveD.LineItem = 1;
            empLeaveD.EmpCode = EmpCode;
            empLeaveD.LeaveCode = leaveType;
            empLeaveD.LeaveDate = InDate;
            empLeaveD.CutMonth = InDate;
            empLeaveD.Status = "Leave";
            empLeaveD.Remark = "Hours";
            empLeaveD.CreateBy = "System";
            empLeaveD.CreateOn = DateTime.Now;
            empLeaveD.CutMonth = InDate;
        }
        public void GetLeaveAutoForMissScan(HREmpLeave empLeave, HREmpLeaveD empLeaveD, decimal WHour, DateTime InDate, int Increment, string leaveType, string EmpCode)
        {
            decimal HDay = 0.5M;
            decimal LHour = WHour * HDay;
            empLeave.EmpCode = EmpCode;
            empLeave.Increment = Increment;
            empLeave.LeaveCode = leaveType;
            empLeave.FromDate = InDate;
            empLeave.ToDate = InDate;
            empLeave.NoDay = 0.5;
            empLeave.TranType = true;
            empLeave.NoPH = 0;
            empLeave.NoRest = 0;
            empLeave.Reason = "Transfer from attendance (0.5 Day)";
            empLeave.RequestDate = InDate;
            empLeave.CreateBy = User.UserName;
            empLeave.CreateOn = DateTime.Now;
            empLeave.Units = "Day";
            empLeave.Status = SYDocumentStatus.APPROVED.ToString();

            empLeaveD.LeaveTranNo = Increment;
            empLeaveD.LineItem = 1;
            empLeaveD.EmpCode = EmpCode;
            empLeaveD.LeaveCode = leaveType;
            empLeaveD.LeaveDate = InDate;
            empLeaveD.CutMonth = InDate;
            empLeaveD.LHour = LHour;
            empLeaveD.Status = "Leave";
            empLeaveD.Remark = "Morning";
            empLeaveD.CreateBy = User.UserName;
            empLeaveD.CreateOn = DateTime.Now;
            empLeaveD.CutMonth = InDate;
        }
        private void InsertLateEarly(ATLateEarlyDeduct EmpLateEalry, string DeductType, decimal BeforeAmount, decimal Amount, string Remark, string StrMissScanIN, ATEmpSchedule item, ClsMissScan _MissScan)
        {
            if (Amount == 0)
                EmpLateEalry.IsMissScan = 1;
            else EmpLateEalry.IsMissScan = 0;
            EmpLateEalry.Remark1 = Remark;

            EmpLateEalry.Remark = StrMissScanIN;
            EmpLateEalry.EmpCode = item.EmpCode;
            EmpLateEalry.DeductType = DeductType;
            EmpLateEalry.DocumentDate = item.TranDate;
            EmpLateEalry.BeforeAmount = BeforeAmount;
            EmpLateEalry.Amount = Amount;
            EmpLateEalry.LeaveCode = _MissScan.LeaveCode;
        }
        private void IsMissScan(PRParameter _param, HRLeaveType LeaveType, ATEmpSchedule item, ClsMissScan _MissScan, string Type)
        {
            DateTime day = item.TranDate;
            if (item.Flag1 == 1 && Type == "Morning")
            {
                _MissScan.LeaveCode = LeaveType.Code + "(M)";
                _MissScan.IsMissIN = true;
                _MissScan.IsMissOut = true;
            }
            if (item.Flag2 == 1 && Type == "Afternoon")
            {
                _MissScan.LeaveCode = LeaveType.Code + "(A)";
                _MissScan.IsMissIN2 = true;
                _MissScan.IsMissOut2 = true;
            }
            if (_param.WDSAT == true && day.DayOfWeek == DayOfWeek.Saturday && LeaveType.InRest == true && _param.WDSATDay == 0.5M)
            {
                _MissScan.IsMissIN = true;
                _MissScan.IsMissOut = true;
            }
        }
        private ClsAtt ValidateAttendance(string LeaveCode,DateTime CheckIn, DateTime CheckOut,
            DateTime? StartDate,DateTime? EndDate,ATShift Shift,DateTime NWFromDate,DateTime NWToDate,
            int Flag1, int Flag2)
        {
            ClsAtt Atten = new ClsAtt();
            DateTime TempFromDate = new DateTime();
            DateTime TempToDate = new DateTime();
            if (StartDate.Value.Year != 1900 && EndDate.Value.Year != 1900)
            {
                if (StartDate > CheckIn) Atten.Late = (int)StartDate.Value.Subtract(CheckIn).TotalMinutes;
                if (EndDate < CheckOut) Atten.Early = Math.Abs((int)EndDate.Value.Subtract(CheckOut).TotalMinutes);
                if (EndDate > CheckOut)
                {
                    Atten.WOT = Convert.ToDecimal(Math.Round(EndDate.Value.Subtract(CheckOut).TotalHours, 2));
                    Atten.OTMin = (decimal)EndDate.Value.Subtract(CheckOut).TotalMinutes;
                }
                DateTime Start1 = new DateTime();
                DateTime End1 = new DateTime();
                Start1 = StartDate.Value;
                End1 = EndDate.Value;
                var interval = End1.Subtract(Start1);

                if (Shift.BreakStart > Shift.BreakEnd) Shift.BreakEnd = Shift.BreakEnd.Value.AddDays(1);
                var BreakHour = Shift.BreakEnd.Value.Subtract(Shift.BreakStart.Value);
                if (Flag1 == 1 && Flag2 == 2)
                {
                    decimal WH = Convert.ToDecimal(interval.Subtract(BreakHour).TotalHours);
                    if (WH > 0)
                        Atten.WHour = Math.Round(WH, 2);
                }
                else
                {
                    decimal WH = Convert.ToDecimal(interval.TotalHours);
                    if (WH > 0)
                        Atten.WHour = Math.Round(WH, 2);
                }
                Atten.GEN = true;
                if (StartDate.Value > NWFromDate) TempFromDate = StartDate.Value;
                else if (StartDate.Value <= NWFromDate) TempFromDate = NWFromDate;
                if (EndDate.Value > NWToDate) TempToDate = NWToDate;
                else if (EndDate.Value <= NWToDate && EndDate <= CheckOut) TempToDate = EndDate.Value;
                else if (EndDate.Value <= NWToDate && CheckOut <= EndDate) TempToDate = CheckOut;
                Atten.NWH = Convert.ToDecimal(TempToDate.Subtract(TempFromDate).TotalHours);
                if (Atten.NWH < 0) Atten.NWH = 0;
            }
            else
            {
                if (StartDate.Value.Year == 1900 && EndDate.Value.Year == 1900 && LeaveCode == "")
                {
                    Atten.Leave = "ABS";
                    Atten.GEN = true;
                }
                else if (StartDate.Value.Year != 1900)
                {
                    if (StartDate > CheckIn) Atten.Late = (int)StartDate.Value.Subtract(CheckIn).TotalMinutes;
                    Atten.GEN = true;
                }
                else if (EndDate.Value.Year != 1900)
                {
                    if (EndDate < CheckOut) Atten.Early = Math.Abs((int)EndDate.Value.Subtract(CheckOut).TotalMinutes);
                    if (EndDate > CheckOut)
                    {
                        Atten.WOT = Math.Round(Convert.ToDecimal(EndDate.Value.Subtract(CheckOut).TotalHours), 2);
                        Atten.GEN = true;
                    }
                }
            }

            return Atten;
        }
        private ATEmpSchedule Attendance_New(ATEmpSchedule result)
        {
            result.NWH = 0;
            result.WOT = 0;
            result.OTApproval = 0;
            result.OTRequest = 0;
            result.WOTMin = 0;
            result.WHOUR = 0;
            result.LeaveNo = -1;
            result.LeaveCode = "";
            result.Late1 = 0;
            result.Early1 = 0;
            result.Late2 = 0;
            result.Early2 = 0;
            result.LeaveDesc = "";
            result.OTTYPE = "";
            result.ActWHour = 0;
            result.OTReason = "";
            result.GEN = false;

            return result;
        }
        private ATEmpSchedule Cal_Maternity_LateEarly(ATEmpSchedule result, List<HRReqMaternity> _EmpResMater)
        {
            var LaEa = _EmpResMater.Sum(x => x.LateEarly);
            int? Total_Late = result.Late1 + result.Late2;
            int? Total_Ealry = result.Early1 + result.Early2;
            int? Total = LaEa;
            if ((Total - Total_Late) >= 0)
            {
                Total -= (result.Late1 + result.Late2);
                result.Late1 = 0;
                result.Late2 = 0;
            }
            else if (Total - Total_Late < 0)
            {
                if ((Total - result.Late1) >= 0)
                {
                    Total -= result.Late1;
                    result.Late1 = 0;
                    if ((Total - result.Late2) < 0)
                    {
                        result.Late2 -= Total;
                        Total = 0;
                    }
                }
                else if ((Total - result.Late1) < 0)
                {
                    result.Late1 -= Total;
                    Total = 0;
                }
            }
            if (Total > 0)
            {
                if ((Total - result.Early1) >= 0)
                {
                    Total -= result.Early1;
                    result.Early1 = 0;
                    if (result.Early2 > 0)
                    {
                        if ((Total - result.Early2) < 0)
                        {
                            result.Early2 -= Total;
                            Total = 0;
                        }
                        else
                        {
                            Total -= result.Early2;
                            result.Early2 = 0;
                        }
                    }
                }
                else if ((Total - result.Early1) < 0)
                {
                    result.Early1 -= Total;
                    Total = 0;
                }
            }

            if (Total == 0) result.WHOUR += Convert.ToDecimal((LaEa / 60.00));
            else result.WHOUR += Convert.ToDecimal(((LaEa - Total) / 60.00));

            return result;
        }
        private ATEmpSchedule Cal_Req_Late_Early(ATEmpSchedule result, List<HRReqLateEarly> _EmpResLateEalry)
        {
            int? Total_Ealry = _EmpResLateEalry.Where(x => x.RequestType == "EARLY1").Sum(w => w.Qty);
            int? Total_Late = _EmpResLateEalry.Where(x => x.RequestType == "LATE1").Sum(w => w.Qty);
            int? Total_Ealry_ = _EmpResLateEalry.Where(x => x.RequestType == "EARLY2").Sum(w => w.Qty);
            int? Total_Late_ = _EmpResLateEalry.Where(x => x.RequestType == "LATE2").Sum(w => w.Qty);
            //late1
            if (Total_Late > 0)
            {
                var Total = Total_Late;
                if (Total - result.Late1 >= 0)
                {
                    Total -= result.Late1;
                    result.Late1 = 0;
                }
                else if (Total - result.Late1 < 0)
                {
                    Total -= result.Late1;
                    result.Late1 = result.Late1 - Total;
                }

                if (Total == 0) result.WHOUR += Convert.ToDecimal((Total_Late / 60.00));
                else result.WHOUR += Convert.ToDecimal(((Total_Late - Total) / 60.00));
            }
            //late2
            if (Total_Late_ > 0)
            {
                var Total = Total_Late_;
                if (Total - result.Late2 >= 0)
                {
                    Total -= result.Late2;
                    result.Late2 = 0;
                }
                else if (Total - result.Late2 < 0)
                {
                    Total -= result.Late2;
                    result.Late2 = result.Late2 - Total_Late_;
                }

                if (Total == 0) result.WHOUR += Convert.ToDecimal((Total_Late_ / 60.00));
                else result.WHOUR += Convert.ToDecimal(((Total_Late_ - Total) / 60.00));
            }
            //early1
            if (Total_Ealry > 0)
            {
                var Total = Total_Ealry;
                if (Total - result.Early1 >= 0)
                {
                    Total -= result.Early1;
                    result.Early1 = 0;
                }
                else if (Total - result.Early1 < 0)
                {
                    Total -= result.Early1;
                    result.Early1 = result.Early1 - Total;
                }

                if (Total == 0) result.WHOUR += Convert.ToDecimal((Total_Ealry / 60.00));
                else result.WHOUR += Convert.ToDecimal(((Total_Ealry - Total) / 60.00));
            }
            //early2
            if (Total_Ealry_ > 0)
            {
                var Total = Total_Ealry_;
                if (Total - result.Early2 >= 0)
                {
                    Total -= result.Early2;
                    result.Early2 = 0;
                }
                else if (Total - result.Early2 < 0)
                {
                    Total -= result.Early2;
                    result.Early2 = result.Early2 - Total;
                }

                if (Total == 0) result.WHOUR += Convert.ToDecimal((Total_Ealry_ / 60.00));
                else result.WHOUR += Convert.ToDecimal(((Total_Ealry_ - Total) / 60.00));
            }
            return result;
        }
        private ATEmpSchedule Cal_OT(ATEmpSchedule result, List<HRRequestOT> EmpOTReq, List<ATOTPolicy> ListOTPolic)
        {
            foreach (var OTR in EmpOTReq)
            {
                OTR.OTActStart = result.OTStart;
                OTR.OTActEnd = result.OTEnd;
                OTR.OTActual = 0;
                decimal OTClaim = 0;
                if (result.OTStart != null && result.OTEnd != null)
                {
                    DateTime OTStartTime = result.OTStart.Value;
                    DateTime OTEndTime = result.OTEnd.Value;
                    if (result.OTEnd.Value >= OTR.OTEndTime)
                    {
                        OTEndTime = OTR.OTEndTime;
                    }
                    else if (result.OTEnd.Value < OTR.OTEndTime)
                    {
                        OTEndTime = result.OTEnd.Value;
                    }
                    if (result.OTStart.Value <= OTR.OTStartTime)
                    {
                        OTStartTime = OTR.OTStartTime;
                    }
                    else if (result.OTStart.Value > OTR.OTStartTime)
                    {
                        OTStartTime = result.OTStart.Value;
                    }
                    var OTNM = (int)OTEndTime.Subtract(OTStartTime).TotalMinutes - OTR.BreakTime;
                    var _OTR = ListOTPolic.Where(w => w.OTFrom <= OTNM && w.OTTo >= OTNM).ToList();
                    if (_OTR.Count() > 0)
                        OTClaim = ClsRounding.Rounding(_OTR.Sum(w => w.OTHour), 2, "N");
                    else
                    {
                        OTClaim = ClsRounding.Rounding((OTNM - OTR.BreakTime) / 60.00M, 2, "N");
                    }
                    if (OTClaim < 0) OTClaim = 0;
                    if (OTClaim > OTR.Hours) OTClaim = OTR.Hours.Value;
                    if (OTClaim != 0)
                    {
                        result.OTStart = OTStartTime;
                        result.OTEnd = OTEndTime;
                        result.OTApproval += OTClaim;
                        result.WOT += OTClaim;
                        OTR.OTActual = OTClaim;
                        OTR.ReferenceNo = Convert.ToInt32(result.TranNo);
                    }
                    result.OTReason = OTR.Reason;
                }
            }

            return result;
        }
        private Task<DateTime> Check_Scan_Attendance(List<ATInOut> raws, DateTime P1, DateTime P2, bool FirstIn = true)
        {
            DateTime InDate = new DateTime(1900, 1, 1);
            if (raws.Where(w => w.FullDate >= P1 && w.FullDate <= P2 && w.LCK != 1).Any())
            {
                ATInOut _INOut = raws.Where(w => w.FullDate >= P1 && w.FullDate <= P2 && w.LCK != 1).OrderBy(x => x.FullDate).FirstOrDefault();
                if (FirstIn == false)
                    _INOut = raws.Where(w => w.FullDate >= P1 && w.FullDate <= P2 && w.LCK != 1).OrderByDescending(x => x.FullDate).FirstOrDefault();
                if (_INOut != null)
                {
                    _INOut.LCK = 1;
                    InDate = _INOut.FullDate;
                }
            }
            return Task.FromResult(InDate);
        }
        private Task<DateTime?> Check_Scan_OT(List<ATInOut> raws, DateTime P1, DateTime P2, bool FirstIn = true)
        {
            DateTime? InDate = null;
            if (raws.Where(w => w.FullDate >= P1 && w.FullDate <= P2 && w.LCK != 1).Any())
            {
                ATInOut _INOut = raws.Where(w => w.FullDate >= P1 && w.FullDate <= P2 && w.LCK != 1).OrderBy(x => x.FullDate).FirstOrDefault();
                if (FirstIn == false)
                    _INOut = raws.Where(w => w.FullDate >= P1 && w.FullDate <= P2 && w.LCK != 1).OrderByDescending(x => x.FullDate).FirstOrDefault();
                if (_INOut != null)
                {
                    _INOut.LCK = 1;
                    InDate = _INOut.FullDate;
                }
            }
            return Task.FromResult(InDate);
        }
        public Dictionary<string, dynamic> OnDataLoadingOther(params object[] keys)
        {
            ClsFilterStaff clsfStaff = new ClsFilterStaff();
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            keyValues.Add("STAFF_SELECT", clsfStaff.OnLoadStaff(true));

            return keyValues;
        }
        public IEnumerable<HRStaffProfile> OnLoadStaff(bool IsCurrent = false)
        {
            DateTime DateResign = DateTime.Now.AddYears(-2).Date;
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListBranchCode = ListBranch.Select(s => s.Code).ToList();
            IEnumerable<HRStaffProfile> ListTemp = DB.HRStaffProfiles.Where(w => ListBranchCode.Contains(w.Branch) &&
             (w.DateTerminate == null || w.DateTerminate.Year == 1900 || w.DateTerminate > DateResign)).ToList();
            if (IsCurrent)
            {
                ListTemp = ListTemp.Where(w => w.Status == "A").ToList();
            }
            return ListTemp;
        }
    }

    public class DayInMonth
    {
        public string EmpCode { get; set; }
        public int Year { get; set; }
        public int Day { get; set; }
        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sep { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }
    }
    public class ListMonth
    {
        public int ID { get; set; }
        public string Months { get; set; }
    }
    public class Temp_Roster
    {
        public string EmpCode { get; set; }
        public DateTime InDate { get; set; }
        public string Shift { get; set; }
    }
    public class ListEmpSch
    {
        public string Error { get; set; }
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string Position { get; set; }
        public string Er_D_1 { get; set; }
        public string Er_D_2 { get; set; }
        public string Er_D_3 { get; set; }
        public string Er_D_4 { get; set; }
        public string Er_D_5 { get; set; }
        public string Er_D_6 { get; set; }
        public string Er_D_7 { get; set; }
        public string Er_D_8 { get; set; }
        public string Er_D_9 { get; set; }
        public string Er_D_10 { get; set; }
        public string Er_D_11 { get; set; }
        public string Er_D_12 { get; set; }
        public string Er_D_13 { get; set; }
        public string Er_D_14 { get; set; }
        public string Er_D_15 { get; set; }
        public string Er_D_16 { get; set; }
        public string Er_D_17 { get; set; }
        public string Er_D_18 { get; set; }
        public string Er_D_19 { get; set; }
        public string Er_D_20 { get; set; }
        public string Er_D_21 { get; set; }
        public string Er_D_22 { get; set; }
        public string Er_D_23 { get; set; }
        public string Er_D_24 { get; set; }
        public string Er_D_25 { get; set; }
        public string Er_D_26 { get; set; }
        public string Er_D_27 { get; set; }
        public string Er_D_28 { get; set; }
        public string Er_D_29 { get; set; }
        public string Er_D_30 { get; set; }
        public string Er_D_31 { get; set; }
        public string D_1 { get; set; }
        public string D_2 { get; set; }
        public string D_3 { get; set; }
        public string D_4 { get; set; }
        public string D_5 { get; set; }
        public string D_6 { get; set; }
        public string D_7 { get; set; }
        public string D_8 { get; set; }
        public string D_9 { get; set; }
        public string D_10 { get; set; }
        public string D_11 { get; set; }
        public string D_12 { get; set; }
        public string D_13 { get; set; }
        public string D_14 { get; set; }
        public string D_15 { get; set; }
        public string D_16 { get; set; }
        public string D_17 { get; set; }
        public string D_18 { get; set; }
        public string D_19 { get; set; }
        public string D_20 { get; set; }
        public string D_21 { get; set; }
        public string D_22 { get; set; }
        public string D_23 { get; set; }
        public string D_24 { get; set; }
        public string D_25 { get; set; }
        public string D_26 { get; set; }
        public string D_27 { get; set; }
        public string D_28 { get; set; }
        public string D_29 { get; set; }
        public string D_30 { get; set; }
        public string D_31 { get; set; }

    }
    public class ClsSumLaEa
    {
        public long TranNo { get; set; }
        public Nullable<bool> GEN { get; set; }
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string LevelCode { get; set; }
        public string LeaveCode { get; set; }
        public string SHIFT { get; set; }
        public string Position { get; set; }
        public Nullable<System.DateTime> Start1 { get; set; }
        public Nullable<System.DateTime> End1 { get; set; }
        public Nullable<System.DateTime> Start2 { get; set; }
        public Nullable<System.DateTime> End2 { get; set; }
        public string DivisionCode { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public decimal WHOUR { get; set; }
        public decimal WOT { get; set; }
        public decimal NWH { get; set; }
        public string GMSTATUS { get; set; }
        public int? Late { get; set; }
        public int CountLate { get; set; }
        public int? Early { get; set; }
        public int CountEarly { get; set; }
        public int CountMissedScan { get; set; }
        public string TerminateStatus { get; set; }
        public string Remark2 { get; set; }
        public Nullable<System.DateTime> WIN1 { get; set; }
        public Nullable<System.DateTime> WOUT1 { get; set; }
        public Nullable<int> WIN_LATE1 { get; set; }
        public Nullable<int> WOUT_EALY1 { get; set; }
        public Nullable<System.DateTime> WIN2 { get; set; }
        public Nullable<System.DateTime> WOUT2 { get; set; }
        public Nullable<int> WIN_LATE2 { get; set; }
        public Nullable<int> WOUT_EALY2 { get; set; }
        public string LEAVEDESC { get; set; }
        public System.DateTime TranDate { get; set; }
        public string LOCT { get; set; }
        public string DEPT { get; set; }
        public string Branch { get; set; }
        public Nullable<System.DateTime> DateTerminate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public decimal OTApproval { get; set; }
        public Nullable<int> MEAL { get; set; }
        public int Total { get; set; }
        public decimal Deduct { get; set; }
    }
    public class ClsMissScan
    {
        public ATShift Shift { get; set; }
        public string LeaveCode { get; set; }
        public string Remark { get; set; }
        public bool IsMissIN { get; set; }
        public bool IsMissOut { get; set; }
        public bool IsMissIN2 { get; set; }
        public bool IsMissOut2 { get; set; }
    }
    public class WorkStatus
    {
        public const string PH = "PH";
        public const string OFF = "OFF";
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