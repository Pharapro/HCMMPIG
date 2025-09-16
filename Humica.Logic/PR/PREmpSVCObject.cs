using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.PR
{
    public class PREmpSVCObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PREmpSVC Header { get; set; }
        public string ScreenId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public string CountryCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<PR_EMPSVC_VIEW> ListHeader { get; set; }
        public List<HISSVCMonthly> ListSVCMonthly { get; set; }
        public List<HR_EMPSVC_VIEW> ListEmpSVC { get; set; }
        public PREmpSVCObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateSVCs()
        {
            try
            {

                if (EmpCode == null)
                {
                    return "EMPCodeEmty";
                }
                var objMatch = DB.PREmpSVCs.Where(w => w.EmpCode == Filter.EmpCode).ToList();
                if (objMatch.Count > 0)
                {
                    return "RECORD_EXIST";
                }
                Header.EmpCode = Filter.EmpCode;
                Header.Point = 1;
                if (Header.InActive == false)
                {
                    Header.EndDate = new DateTime(5000, 1, 1);
                }
                Header.CreateBy = User.UserName;
                Header.CreateOn = DateTime.Now;
                DB.PREmpSVCs.Add(Header);
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.TranNo.ToString();
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
                log.DocurmentAction = Header.TranNo.ToString();
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
                log.DocurmentAction = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditSVC(int id)
        {
            try
            {
                var ObjMatch = DB.PREmpSVCs.Find(id);
                if (ObjMatch == null)
                {
                    return "INVALID_DATA";
                }
                Header.TranNo = id;
                ObjMatch.StartDate = Header.StartDate;
                if (Header.InActive == false)
                {
                    ObjMatch.EndDate = new DateTime(5000, 1, 1);
                }
                else
                {
                    ObjMatch.EndDate = Header.EndDate;
                }
                ObjMatch.InActive = Header.InActive;
                ObjMatch.Remark = Header.Remark;
                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;

                DB.PREmpSVCs.Attach(ObjMatch);
                DB.Entry(ObjMatch).Property(w => w.StartDate).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.EndDate).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.InActive).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.Remark).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.ChangedOn).IsModified = true;
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.TranNo.ToString();
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
                log.DocurmentAction = Header.TranNo.ToString();
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
                log.DocurmentAction = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteEmpSVCs(int id)
        {
            try
            {
                var EmpSVC = DB.PREmpSVCs.Find(id);
                if (EmpSVC == null)
                {
                    return "INVALID_DATA";
                }

                DB.PREmpSVCs.Remove(EmpSVC);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = id.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = id.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public void CreateNewEmpSVC(DateTime InMonth)
        {
            try
            {
                var ListEmpSVC = DB.PREmpSVCs.ToList();
                var ListStaff = DB.HRStaffProfiles.ToList();
                DateTime ToDate = new DateTime(InMonth.Year, InMonth.Month, DateTime.DaysInMonth(InMonth.Year, InMonth.Month));
                DateTime FromDate = new DateTime(InMonth.Year, InMonth.Month, 1);
                var d = (from SVC in ListEmpSVC
                         join emp in ListStaff on SVC.EmpCode equals emp.EmpCode
                         where ((FromDate >= SVC.StartDate.Value.Date && FromDate <= SVC.EndDate.Value.Date)
                         || (ToDate >= SVC.StartDate.Value.Date && ToDate <= SVC.EndDate.Value.Date)
                         || (SVC.StartDate.Value.Date >= FromDate && SVC.StartDate.Value.Date <= ToDate)
                         || emp.StartDate >= FromDate && (emp.DateTerminate.Year == 1900 || emp.DateTerminate >= FromDate)
                         )
                         // && SVC.EndDate.Value.Month != FromDate.Month
                         select new
                         {
                             SVC.EmpCode,
                             emp.AllName,
                             emp.StartDate,
                             emp.StaffType,
                             SVCStartDate = SVC.StartDate.Value,
                             SVC.EndDate,
                             SVC.Point
                         }).ToList();
                foreach (var item in d)
                {
                    var Svc = new HISSVCMonthly();
                    Svc.EmpCode = item.EmpCode;
                    Svc.InYear = InMonth.Year;
                    Svc.InMonth = InMonth.Month;
                    Svc.GetPoint = Convert.ToDecimal(item.Point);
                    Svc.Day = Filter.Day;
                    if (item.StaffType == "PT")
                    {
                        Svc.Day = Svc.Day / 2;
                    }
                    Svc.NoDay = 0;
                    Svc.ProRate = Convert.ToDecimal(item.Point);
                    Svc.Rate = 0;
                    Svc.Deduct = 0;
                    Svc.Adding = 0;
                    Svc.Amount = 0;
                    Svc.LCK = 0;
                    DB.HISSVCMonthlies.Add(Svc);
                }
                int row = DB.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Filter.InMonth.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Filter.InMonth.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
            }
        }
        public List<HR_EMPSVC_VIEW> LoadDataGenSVC(DateTime InMonth)
        {
            var ListSVC = DB.HISSVCMonthlies.ToList();
            var _List = new List<HR_EMPSVC_VIEW>();
            ListSVC = ListSVC.Where(w => w.InYear == InMonth.Year && w.InMonth == InMonth.Month).ToList();
            if (ListSVC.Count == 0)
            {
                CreateNewEmpSVC(InMonth);
            }
            _List = DBV.HR_EMPSVC_VIEW.ToList();
            _List = _List.Where(w => w.InYear == InMonth.Year && w.InMonth == InMonth.Month).ToList();
            return _List;
        }
        public string getCalculate_NoSVCDay(string EmpCode)
        {
            try
            {
                var ListEmpSVC = DB.PREmpSVCs.ToList();
                string Approve = SYDocumentStatus.APPROVED.ToString();
                var LeaveType = DB.HRLeaveTypes.ToList();
                var LeaveH = DB.HREmpLeaves.Where(w => w.Status == Approve).ToList();
                var EmpLeave = DB.HREmpLeaveDs.ToList();
                EmpLeave = EmpLeave.Where(w => LeaveH.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                LeaveType = LeaveType.Where(w => w.SVC == true).ToList();
                ListEmpSVC = ListEmpSVC.ToList();
                string[] Emp = EmpCode.Split(';');
                //decimal TotalSDay = 0;
                var ListSVC = DB.HISSVCMonthlies.ToList();
                ListSVC = ListSVC.Where(w => w.InYear == Filter.InMonth.Year && w.InMonth == Filter.InMonth.Month).ToList();

                foreach (var Code in Emp)
                {
                    if (Code.Trim() != "")
                    {
                        var First = ListSVC.Where(w => w.EmpCode == Code).FirstOrDefault();
                        decimal? NoLeave = 0;
                        decimal? NoFromStart = 0;
                        decimal? NoFromEnd = 0;
                        DateTime StartServiceCharge = ListEmpSVC.Where(w => w.EmpCode == Code).FirstOrDefault().StartDate.Value;
                        DateTime EndServiceCharge = ListEmpSVC.Where(w => w.EmpCode == Code).FirstOrDefault().EndDate.Value;
                        string PayPram = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == Code).PayParam;
                        var Pay = DB.PRParameters.Find(PayPram);
                        DateTime FromDate = GetFromDate(Filter.InMonth, Code);
                        DateTime ToDate = GetToDate(Filter.InMonth, Code);

                        if (StartServiceCharge > FromDate)
                        {
                            NoFromStart = First.Day - Convert.ToDecimal(Get_WorkingDay(Pay, StartServiceCharge, ToDate));
                        }
                        if (EndServiceCharge < ToDate)
                        {
                            NoFromEnd = First.Day - Convert.ToDecimal(Get_WorkingDay(Pay, StartServiceCharge, ToDate));
                        }
                        if (EndServiceCharge.Date > ToDate.Date && EndServiceCharge.Year != 5000 && EndServiceCharge.Month == ToDate.Month)
                        {
                            First.Day = Get_WorkingDay(Pay, FromDate, EndServiceCharge);
                        }
                        LeaveH = LeaveH.Where(w => w.EmpCode == Code).ToList();
                        EmpLeave = EmpLeave.Where(w => LeaveH.Where(y => y.EmpCode == w.EmpCode).Any()).ToList();
                        EmpLeave = EmpLeave.Where(w => LeaveType.Where(x => x.Code == w.LeaveCode && x.SVC == true).Any()).ToList();
                        EmpLeave = EmpLeave.Where(w => w.LeaveDate.Date >= FromDate.Date && w.LeaveDate.Date <= ToDate.Date).ToList();
                        foreach (var item in EmpLeave)
                        {
                            NoLeave += Convert.ToDecimal(item.LHour / Pay.WHOUR);
                        }
                        NoLeave += NoFromStart + NoFromEnd;
                        var Point = ((Filter.Day - NoLeave) * First.GetPoint) / Filter.Day;
                        First.NoDay = NoLeave;
                        First.ProRate = Point;
                        DB.HISSVCMonthlies.Attach(First);
                        DB.Entry(First).Property(w => w.NoDay).IsModified = true;
                        DB.Entry(First).Property(w => w.ProRate).IsModified = true;
                    }
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
                log.DocurmentAction = Filter.INYear.ToString();
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
                log.DocurmentAction = Filter.InMonth.ToString();
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
                log.DocurmentAction = Filter.InMonth.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string Calculate_SVC()
        {
            try
            {
                var SVCValue = DB.PRSVCValues.ToList();
                SVCValue = SVCValue.Where(w => w.InYear == Filter.InMonth.Year && w.InMonth == Filter.InMonth.Month).ToList();
                var ListSVC = DB.HISSVCMonthlies.ToList();
                ListSVC = ListSVC.Where(w => w.InYear == Filter.InMonth.Year && w.InMonth == Filter.InMonth.Month).ToList();
                var TotalRate = ListSVC.Sum(w => w.Day) - ListSVC.Sum(w => w.NoDay);
                var Rate = SVCValue.Sum(w => w.Amount) / TotalRate;
                decimal? SVCAmount = 0;
                foreach (var item in ListSVC)
                {
                    SVCAmount = 0;
                    SVCAmount = Rate * (item.Day - item.NoDay);
                    item.Amount = SVCAmount;
                    DB.HISSVCMonthlies.Attach(item);
                    DB.Entry(item).Property(w => w.Amount).IsModified = true;
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
                log.DocurmentAction = Filter.INYear.ToString();
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
                log.DocurmentAction = Filter.InMonth.ToString();
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
                log.DocurmentAction = Filter.InMonth.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string Delete_Emp_SVC(string EmpCode, DateTime InMonth)
        {
            var DBI = new HumicaDBContext();
            try
            {
                DBI.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    Header = new PREmpSVC();
                    string[] _TranNo = EmpCode.Split(';');
                    var ListSVC = DB.HISSVCMonthlies.ToList();
                    ListSVC = ListSVC.Where(w => w.InYear == InMonth.Year && w.InMonth == InMonth.Month).ToList();
                    foreach (var Code in _TranNo)
                    {
                        Header.EmpCode = Code;
                        var Result = ListSVC.Where(w => w.EmpCode == Code).ToList();
                        foreach (var item in Result)
                        {
                            DBI.HISSVCMonthlies.Attach(item);
                            DBI.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        }
                    }
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
            { DBI.Configuration.LazyLoadingEnabled = true; }
        }

        public DateTime GetFromDate(DateTime InDate, string EmpCode)
        {
            DateTime Result = new DateTime();
            string PayPram = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode).PayParam;
            var Pay = DB.PRParameters.Find(PayPram);
            DateTime FromDate = Pay.FROMDATE.Value;
            DateTime ToDate = Pay.TODATE.Value;
            if (FromDate.Month < ToDate.Month)
            {
                int Month = InDate.Month - 1;
                int Year = InDate.Year;
                if (Month == 0)
                {
                    Month = 12;
                    Year = -1;
                }
                Result = new DateTime(Year, Month, FromDate.Day);
            }
            else Result = new DateTime(InDate.Year, InDate.Month, FromDate.Day);
            return Result;
        }
        public DateTime GetToDate(DateTime InDate, string EmpCode)
        {
            DateTime Result = new DateTime();
            string PayPram = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode).PayParam;
            var Pay = DB.PRParameters.Find(PayPram);
            DateTime ToDate = Pay.TODATE.Value;
            if (ToDate.Day == 31 || ToDate.Day == 30 || ToDate.Day == 29 || ToDate.Day == 28)
            {
                Result = new DateTime(InDate.Year, InDate.Month, DateTime.DaysInMonth(InDate.Year, InDate.Month));
            }
            else Result = new DateTime(InDate.Year, InDate.Month, ToDate.Day);
            return Result;
        }
        public decimal Get_WorkingDay(PRParameter PayPram, DateTime FromDate, DateTime ToDate)
        {
            decimal? Result = 0;
            DateTime TempDate = new DateTime();
            if (PayPram != null)
            {
                int Count = 0;
                for (int i = FromDate.Day; i <= ToDate.Day; i++)
                {
                    TempDate = FromDate.AddDays(Count);
                    if (PayPram.WDMON == true && TempDate.DayOfWeek == DayOfWeek.Monday) Result += PayPram.WDMONDay;
                    else if (PayPram.WDTUE == true && TempDate.DayOfWeek == DayOfWeek.Tuesday) Result += PayPram.WDTUEDay;
                    else if (PayPram.WDWED == true && TempDate.DayOfWeek == DayOfWeek.Wednesday) Result += PayPram.WDWEDDay;
                    else if (PayPram.WDTHU == true && TempDate.DayOfWeek == DayOfWeek.Thursday) Result += PayPram.WDTHUDay;
                    else if (PayPram.WDFRI == true && TempDate.DayOfWeek == DayOfWeek.Friday) Result += PayPram.WDFRIDay;
                    else if (PayPram.WDSAT == true && TempDate.DayOfWeek == DayOfWeek.Saturday) Result += PayPram.WDSATDay;
                    else if (PayPram.WDSUN == true && TempDate.DayOfWeek == DayOfWeek.Sunday) Result += PayPram.WDSUNDay;
                    Count++;
                }
            }
            return Convert.ToDecimal(Result);
        }
    }
}


