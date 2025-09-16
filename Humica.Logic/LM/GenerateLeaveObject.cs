using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.Helper;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.ESS;
using Humica.Logic.PR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;

namespace Humica.Logic.LM
{
    public class GenerateLeaveObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SMSystemEntity DP = new SMSystemEntity();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public string MessageError2 { get; set; }
        public HREmpLeave HeaderEmpLeave { get; set; }
        public HREmpEditLeaveEntitle HeaderEditEntitle { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public FTFilterData Filter { get; set; }
        public FTFilerIndex FInYear { get; set; }
        public HREmpLeaveB EmpLeaveB { get; set; }
        public List<HREmpLeaveB> ListEmpLeaveB { get; set; }
        public List<HREmpEditLeaveEntitle> ListEmpEditLeaveEntitle { get; set; }
        public List<HREmpLeave> ListEmpLeave { get; set; }
        public List<HREmpLeaveD> ListEmpLeaveD { get; set; }
        public List<Employee_Generate_Leave> ListEmpGen { get; set; }
        public HRApproverLeave ApprovalWorkFlow { get; set; }
        public List<HRApproverLeave> ListApproverLeave { get; set; }
        public List<Employee_ListForwardLeave> ListForward { get; set; }
        public List<HR_VIEW_EmpLeave> ListEmpLeaveReq { get; set; }
        public List<HR_VIEW_EmpLeave> ListEmpLeavePending { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<HR_STAFF_VIEW> ListStaff { get; set; }
        public string EmpID { get; set; }
        public string Units { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public GenerateLeaveObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void LoadData(string userName)
        {
            ListEmpLeaveReq = new List<HR_VIEW_EmpLeave>();
            var staff = DB.HRStaffProfiles.Where(w => w.EmpCode == userName).ToList();
            if (staff.Any())
            {
                string approved = SYDocumentStatus.APPROVED.ToString();
                string Reject = SYDocumentStatus.REJECTED.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                string Pending = SYDocumentStatus.PENDING.ToString();
                var ListLeaveCreater = DB.HREmpLeaves.Where(x => x.Status == FInYear.Status && x.CreateBy == userName).ToList();
                if (FInYear.Status != Reject && FInYear.Status != Cancel)
                    ListLeaveCreater = DB.HREmpLeaves.Where(x => x.Status == FInYear.Status && x.CreateBy == userName && (x.Status == approved || x.Status == Pending)).ToList();
                var ListEmpLEave = DB.HREmpLeaves.Where(x => x.FromDate.Year >= FInYear.InYear).ToList();
                var ListApp = DB.ExDocApprovals.AsEnumerable().Where(w => ListEmpLEave.Where(x => x.Increment.ToString() == w.DocumentNo
                              && w.Approver == userName).Any()).ToList();
                foreach (var read in ListApp)
                {
                    var objemp = new HR_VIEW_EmpLeave();
                    if (read.ApproveLevel > 1 && read.Status == Open)
                    {
                        var level = DB.ExDocApprovals.Where(w => w.DocumentNo == read.DocumentNo && w.DocumentType == read.DocumentType
                                    && w.ApproveLevel < read.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                        if (level != null) continue;
                        var EmpStaff = ListEmpLEave.FirstOrDefault(w => w.Increment.ToString() == read.DocumentNo);
                        if (EmpStaff == null || EmpStaff.Status != Pending) continue;
                        var EmpStaff_ = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.EmpCode == EmpStaff.EmpCode && w.TranNo == EmpStaff.TranNo);
                        if (EmpStaff_ == null) continue;
                        objemp = EmpStaff_;
                        objemp.Waiting = read.ApproverName;
                        ListEmpLeaveReq.Add(objemp);
                    }
                    else if (read.ApproveLevel == 1 && read.Status == Open)
                    {
                        var EmpStaff = ListEmpLEave.FirstOrDefault(w => w.Increment.ToString() == read.DocumentNo);
                        if (EmpStaff == null || EmpStaff.Status != Pending) continue;
                        var EmpStaff_ = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.EmpCode == EmpStaff.EmpCode && w.TranNo == EmpStaff.TranNo);
                        if (EmpStaff_ == null) continue;
                        objemp = EmpStaff_;
                        objemp.Waiting = read.ApproverName;
                        ListEmpLeaveReq.Add(objemp);
                    }
                    else if (read.Status == FInYear.Status)
                    {
                        var level = DB.ExDocApprovals.Where(w => w.DocumentNo == read.DocumentNo && w.DocumentType == read.DocumentType
                                    && w.Status != approved && w.ApproveLevel > read.ApproveLevel).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                        var EmpStaff = ListEmpLEave.FirstOrDefault(w => w.Increment.ToString() == read.DocumentNo);
                        if (EmpStaff == null) continue;
                        var EmpStaff_ = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.EmpCode == EmpStaff.EmpCode && w.TranNo == EmpStaff.TranNo);
                        if (EmpStaff_ == null) continue;
                        objemp = EmpStaff_;
                        if (EmpStaff_.Status != approved && level != null)
                            objemp.Waiting = level.ApproverName;
                        ListEmpLeaveReq.Add(objemp);
                    }

                }
                foreach (var read in ListLeaveCreater)
                {
                    var level = DB.ExDocApprovals.Where(w => w.DocumentNo == read.Increment.ToString() && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                    var objemp = new HR_VIEW_EmpLeave();
                    var Staff = ListEmpLeaveReq.FirstOrDefault(w => w.EmpCode == read.EmpCode && w.TranNo == read.TranNo);
                    if (Staff != null) continue;
                    var EmpStaff = ListLeaveCreater.FirstOrDefault(w => w.Increment.ToString() == read.Increment.ToString() && w.EmpCode == read.EmpCode);
                    if (EmpStaff == null) continue;
                    var EmpStaff_ = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.EmpCode == EmpStaff.EmpCode && w.TranNo == EmpStaff.TranNo);
                    if (EmpStaff_ == null) continue;
                    objemp = EmpStaff_;
                    if (EmpStaff_.Status != approved && level != null)
                        objemp.Waiting = level.ApproverName;
                    ListEmpLeaveReq.Add(objemp);
                }

                var ListEmpLeave_ = DB.HREmpLeaves
                    .Where(x => x.FromDate.Year >= FInYear.InYear && x.IsReqCancel == true && x.Status != Cancel && x.Status != Reject).ToList();
                ListEmpLeave_ = ListEmpLeave_.Where(x => DB.ExDocApprovals.Any(w => x.Increment.ToString() == w.DocumentNo && w.Approver == userName)).ToList();

                foreach (var leave in ListEmpLeave_)
                {
                    var filReqCancel = ListEmpLeaveReq.Where(w => w.TranNo == leave.TranNo).ToList();
                    if (filReqCancel.Any())
                        continue;
                    var leaveRequestToCancel = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.TranNo == leave.TranNo);
                    if (leaveRequestToCancel != null)
                    {
                        ListEmpLeaveReq.Add(leaveRequestToCancel);
                    }
                }
            }
        }
        public static List<HRLeaveType> GetLeaveType(List<HRLeaveType> ListLeaveType, string TemLeave, string Sex, int WorkMonth, bool IsProcess = false)
        {
            List<HRLeaveType> Temp = ListLeaveType.Where(w => w.IsParent == true).ToList();
            var DBI = new HumicaDBContext();
            List<HRSetEntitleD> ListTemp = DBI.HRSetEntitleDs.Where(w => w.CodeH == TemLeave && w.FromMonth <= WorkMonth).ToList();
            //List<HRSetEntitleD> ListTemp = DBI.HRSetEntitleDs.Where(w => w.CodeH == TemLeave).ToList();
            ListLeaveType = ListLeaveType.Where(w => ListTemp.Where(x => w.Code == x.LeaveCode).Any()).ToList();
            if (IsProcess == true)
            {
                foreach (var item in Temp)
                {
                    if (!ListLeaveType.Where(w => w.Code == item.Code).Any())
                    {
                        ListLeaveType.Add(item);
                    }
                }
            }

            ListLeaveType = ListLeaveType.Where(w => w.Gender == Sex || w.Gender == "B").ToList();
            return ListLeaveType;
        }
        public void ReGenerateLeaveToken(string EmpCode, DateTime FromDate, DateTime ToDate)
        {
            var yearGroups = new Dictionary<int, List<DateTime>>();
            var Policy = DB.ATPolicies.FirstOrDefault();
            DateTime To = Policy.LToDate;
            for (var day = FromDate.Date; day.Date <= ToDate.Date; day = day.AddDays(1))
            {
                int year = day.Year;
                if (day.Month == 12 && day.Day > To.Day)
                    year = day.AddYears(1).Year;
                if (!yearGroups.ContainsKey(year))
                    yearGroups[year] = new List<DateTime>();
                yearGroups[year].Add(day);
            }
            foreach (var yearGroup in yearGroups)
            {
                int year = yearGroup.Key;
                Console.WriteLine($"Processing for year: {year}");
                GET_Leave_LeaveBalance(EmpCode, year);
            }
        }
        public void GET_Leave_LeaveBalance(string EmpCode, int InYear)
        {
            var DBI = new HumicaDBContext();
            DB = new HumicaDBContext();
            try
            {
                DBI.Configuration.AutoDetectChangesEnabled = false;

                string approved = SYDocumentStatus.APPROVED.ToString();
                var Employee = DB.HRStaffProfiles.FirstOrDefault(x => x.EmpCode == EmpCode);
                var LstEmpClaim = DB.HRClaimLeaves.Where(w => w.EmpCode == EmpCode && w.Status == approved && w.WorkingDate.Year == InYear && (w.IsUsed.Value == true || w.IsExpired.Value == false)).ToList();
                var _listLEaveB = DB.HREmpLeaveBs.Where(w => w.EmpCode == EmpCode && w.InYear == InYear).ToList();
                List<HRLeaveType> ListLeaveType = DB.HRLeaveTypes.ToList();
                decimal _DayLeave = 0;

                var Policy = DB.ATPolicies.FirstOrDefault();
                DateTime From = Policy.LFromDate;
                DateTime To = Policy.LToDate;
                int year = InYear;
                if (From.Year != To.Year) year = InYear - 1;
                DateTime FromDate = new DateTime(year, From.Month, From.Day);
                DateTime ToData = new DateTime(InYear, To.Month, To.Day);

                DateTime StartDate = Convert.ToDateTime(Employee.LeaveConf);
                int WorkMonth = DateTimeHelper.CountMonth(StartDate, ToData);
                List<HREmpLeaveD> _ListLeaveD = GetLeaveToken(FromDate, ToData).ToList();
                var LeaveByDay = _ListLeaveD.Where(w => w.EmpCode == Employee.EmpCode).ToList();
                var payParam = DB.PRParameters.FirstOrDefault(w => w.Code == Employee.PayParam);
                var ListLeaveCode = LeaveByDay.GroupBy(w => w.LeaveCode).ToList();
                List<HRSetEntitleD> ListetEntitleD = DB.HRSetEntitleDs.Where(w => w.CodeH == Employee.TemLeave).ToList();
                var ListLeave_Rate = DB.HRLeaveProRates.Where(w => (w.EntitleType == Employee.TemLeave) || (string.IsNullOrEmpty(w.EntitleType))).ToList();

                if (_listLEaveB.Count > 0)
                {
                    string LeaveCode = "";
                    var _LT = ListLeaveType.Where(w => w.IsParent == true).ToList();
                    List<HRLeaveType> TempLeave = GetLeaveType(ListLeaveType, Employee.TemLeave, Employee.Sex, WorkMonth);
                    foreach (var Code in ListLeaveCode)
                    {
                        if (!TempLeave.Where(w => w.Code == Code.Key).Any())
                        {
                            HRLeaveType _L = ListLeaveType.FirstOrDefault(w => w.Code == Code.Key);
                            if (_L == null) continue;
                            TempLeave.Add(_L);
                        }
                    }
                    foreach (var item in TempLeave)
                    {
                        LeaveCode = item.Code;

                        var _LeaveB = _listLEaveB.FirstOrDefault(w => w.LeaveCode == LeaveCode);
                        bool IsNew = false;
                        if (_LeaveB == null)
                        {
                            IsNew = true;
                            _LeaveB = NewEmpLeaveB(EmpCode, Employee.AllName, LeaveCode, InYear);
                        }

                        ClsPeriodLeave periodLeave = new ClsPeriodLeave();
                        periodLeave.payParam = payParam;
                        periodLeave.ListLeaveProRate = ListLeave_Rate.Where(w => w.LeaveType == LeaveCode && (string.IsNullOrEmpty(w.EntitleType) || w.EntitleType == Employee.TemLeave)).ToList();
                        periodLeave.LeaveType = item;
                        periodLeave = GetPeriod(periodLeave, Employee, ListetEntitleD, InYear, _LeaveB.LeaveCode);

                        decimal Balance = _LeaveB.DayEntitle.Value + periodLeave.SeniorityBalance;
                        if (item.IsCurrent == false)
                        {
                            Balance = periodLeave.Balance;
                        }
                        HREmpLeaveB lB = Calculate_Token(_LeaveB, _ListLeaveD, ListLeaveType, payParam, Balance, LstEmpClaim);

                        decimal DayLeave = lB.Balance.Value;
                        if (IsNew == false)
                        {
                            _DayLeave = DayLeave;
                            _LeaveB.AllName = Employee.AllName;
                            _LeaveB.DayLeave = lB.Balance;
                            //_LeaveB.Balance = _LeaveB.DayEntitle + periodLeave.SeniorityBalance + _LeaveB.Rest_Edit + _LeaveB.PH_Edit - _DayLeave;

                            _LeaveB.ChangeBy = User.UserName;
                            _LeaveB.ChangeOn = DateTime.Now;
                            _LeaveB.ForwardUse = lB.ForwardUse;
                            _LeaveB.SeniorityBalance = periodLeave.SeniorityBalance;
                            _LeaveB.YTD = _LeaveB.DayEntitle + periodLeave.SeniorityBalance + _LeaveB.Rest_Edit + _LeaveB.PH_Edit;
                            _LeaveB = Calculate_Balance(_LeaveB);
                            DBI.HREmpLeaveBs.Attach(_LeaveB);
                            DBI.Entry(_LeaveB).Property(w => w.AllName).IsModified = true;
                            DBI.Entry(_LeaveB).Property(w => w.DayLeave).IsModified = true;
                            DBI.Entry(_LeaveB).Property(w => w.TakenHour).IsModified = true;
                            DBI.Entry(_LeaveB).Property(w => w.ForWardExp).IsModified = true;
                            DBI.Entry(_LeaveB).Property(w => w.Balance).IsModified = true;
                            DBI.Entry(_LeaveB).Property(w => w.BalanceHour).IsModified = true;
                            DBI.Entry(_LeaveB).Property(w => w.YTD).IsModified = true;
                            DBI.Entry(_LeaveB).Property(w => w.Rest_Edit).IsModified = true;
                            DBI.Entry(_LeaveB).Property(w => w.PH_Edit).IsModified = true;
                            DBI.Entry(_LeaveB).Property(w => w.ForwardUse).IsModified = true;
                            DBI.Entry(_LeaveB).Property(w => w.SeniorityBalance).IsModified = true;
                            DBI.Entry(_LeaveB).Property(w => w.Current_AL).IsModified = true;
                            DBI.Entry(_LeaveB).Property(w => w.ChangeOn).IsModified = true;
                            DBI.Entry(_LeaveB).Property(w => w.ChangeBy).IsModified = true;
                        }
                        else
                        {
                            var LeaveP = ListLeaveType.FirstOrDefault(w => w.IsParent == true && w.Code == _LeaveB.LeaveCode);
                            if (DayLeave != 0 && LeaveP == null)
                            {
                                var EmpLeaveB = new HREmpLeaveB();
                                EmpLeaveB = _LeaveB;
                                EmpLeaveB.DayLeave = DayLeave;
                                DBI.HREmpLeaveBs.Add(EmpLeaveB);
                            }
                        }
                    }
                    int row = DBI.SaveChanges();
                }
            }
            finally
            {
                DBI.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public void Generate_Leave_Entitle(string EmpCode, int InYear, HRStaffProfile _StaffProfile, List<HRLeaveType> leaveTypes)
        {
            var Policy = unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
            DateTime From = Policy.LFromDate;
            DateTime To = Policy.LToDate;
            int year = InYear;
            if (From.Year != To.Year) year = InYear - 1;
            DateTime FromDate = new DateTime(year, From.Month, From.Day);
            DateTime EndDate = new DateTime(InYear, To.Month, To.Day);

            DateTime StartDate = Convert.ToDateTime(_StaffProfile.LeaveConf);
            int WorkMonth = DateTimeHelper.CountMonth(StartDate, EndDate);
            List<HRLeaveType> ListLeaveType = leaveTypes.Where(w => w.IsParent != true).ToList();
            List<HRSetEntitleD> ListetEntitleD = unitOfWork.Repository<HRSetEntitleD>().Queryable().Where(w => w.CodeH == _StaffProfile.TemLeave).ToList();
            List<HRLeaveType> TempLeave = GetLeaveType(ListLeaveType, _StaffProfile.TemLeave, _StaffProfile.Sex, WorkMonth);
            var _listLeaveProRate = unitOfWork.Repository<HRLeaveProRate>().Where(w => (w.EntitleType == _StaffProfile.TemLeave) || (string.IsNullOrEmpty(w.EntitleType))).ToList();
            var ListPH = unitOfWork.Repository<HRPubHoliday>().Queryable().Where(w => w.PDate.Year == InYear).ToList();
            PRParameter payParam = unitOfWork.Repository<PRParameter>().Find(_StaffProfile.PayParam);
            int CountMonth = DateTimeHelper.CountMonth(FromDate, EndDate);
            foreach (var leave in TempLeave)
            {
                if (leave.Code == "AL")
                {
                    var leaveB_AL = unitOfWork.Repository<HREmpLeaveB>().Queryable().Where(w => w.LeaveCode == leave.Code && w.EmpCode == EmpCode && w.InYear == InYear);
                    if (leaveB_AL.Any()) continue;
                }
                decimal ENTITLE = 0;
                decimal SeniorityBalance = 0;
                var LeaveTemp = ListetEntitleD.Where(w => w.LeaveCode == leave.Code & w.FromMonth <= WorkMonth && w.ToMonth >= WorkMonth).ToList();
                foreach (var temp in LeaveTemp)
                {
                    SeniorityBalance = temp.SeniorityBalance;
                    if (temp.IsProRate == true)
                    {
                        decimal Rate = (decimal)(temp.Entitle / CountMonth);
                        List<HRLeaveProRate> LeaveProRate = _listLeaveProRate.Where(w => w.Status == "NEWJOIN" && w.LeaveType == leave.Code).ToList();
                        if (LeaveProRate.Count() > 0)
                        {
                            if (StartDate.Year == InYear)
                            {
                                DateTime FromDateInMonth = StartDate.StartDateOfMonth();
                                DateTime EndDatetOfMonth = StartDate.EndDateOfMonth();
                                decimal actWorkDay = PRPayParameterObject.Get_WorkingDay(payParam, FromDateInMonth, EndDatetOfMonth, StartDate, EndDatetOfMonth, 0);
                                ENTITLE = Convert.ToDecimal(Rate * DateTimeHelper.MonthDiff(EndDate, StartDate));
                                decimal prorateAmount = 0;
                                if (LeaveProRate.Count > 0)
                                {
                                    HRLeaveProRate prorateLeave = LeaveProRate.Where(w => w.ActWorkDayFrom <= actWorkDay && w.ActWorkDayTo >= actWorkDay).FirstOrDefault();
                                    prorateAmount = prorateLeave == null ? 0 : prorateLeave.Rate;
                                    ENTITLE += prorateAmount;
                                }
                            }
                            else
                            {
                                ENTITLE = (decimal)temp.Entitle;
                            }
                        }
                        else
                        {

                            if (WorkMonth >= temp.ToMonth)
                                ENTITLE = (decimal)temp.Entitle;
                            else
                            {
                                int C_Day = DateTimeHelper.GetDay(StartDate, EndDate);
                                decimal EDay = (decimal)temp.Entitle;
                                ENTITLE = ClsRounding.RoundNormal(C_Day * (EDay / 365), 2);
                                if (ENTITLE > (decimal)temp.Entitle)
                                {
                                    ENTITLE = (decimal)temp.Entitle;
                                }

                            }
                        }
                    }
                    else
                    {
                        ENTITLE = (decimal)temp.Entitle;
                    }
                }
                EmpLeaveB = new HREmpLeaveB();
                if (leave.Code == "PH")
                {
                    ENTITLE = ListPH.Count();
                }
                if (leave.Code == "DO" || leave.Code == "OFF")
                {
                    ENTITLE = 0;
                    if (StartDate.Year == InYear)
                    {
                        FromDate = StartDate;
                    }
                    for (var day = FromDate.Date; day.Date <= EndDate.Date; day = day.AddDays(1))
                    {
                        if (day.DayOfWeek == DayOfWeek.Sunday)
                        {
                            ENTITLE += 1;
                        }
                    }
                }
                EmpLeaveB.AllName = _StaffProfile.AllName;
                EmpLeaveB.DayEntitle = ENTITLE;
                EmpLeaveB.EmpCode = EmpCode;
                EmpLeaveB.InYear = InYear;
                EmpLeaveB.YTD = ENTITLE + SeniorityBalance;
                EmpLeaveB.LeaveCode = leave.Code;
                EmpLeaveB.Balance = ENTITLE + SeniorityBalance;
                EmpLeaveB.Forward = 0;
                EmpLeaveB.DayLeave = 0;
                EmpLeaveB.ForWardExp = new DateTime(1900, 1, 1);
                EmpLeaveB.SeniorityBalance = SeniorityBalance;
                EmpLeaveB.CreateBy = User.UserName;
                EmpLeaveB.CreateOn = DateTime.Now;
                ListEmpLeaveB.Add(EmpLeaveB);
            }
        }
        public void Generate_Leave_Cu(int InYear)
        {
            OnLoad();
            var DBI = new HumicaDBContext();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                DBI.Configuration.AutoDetectChangesEnabled = false;
                var Policy = unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
                DateTime From = Policy.LFromDate;
                DateTime To = Policy.LToDate;
                int year = InYear;
                if (From.Year != To.Year) year = InYear - 1;
                DateTime StartDate = new DateTime(year, From.Month, From.Day);
                DateTime ToDate = new DateTime(InYear, To.Month, To.Day);
                DateTime DateNow = DateTime.Now;

                var Employee = unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w => w.LeaveConf.HasValue && (w.Status == "A" || (w.Status == "I" && w.DateTerminate.Year >= InYear))).ToList();
                var ListParam = unitOfWork.Repository<PRParameter>().Queryable().ToList();
                List<HRStaffProfile> _listStaff = new List<HRStaffProfile>();
                _listStaff = Employee.Where(w => w.StartDate <= ToDate && (w.DateTerminate.Year == 1900 || w.DateTerminate.AddDays(-1) >= StartDate)).ToList();

                var EmpLeaveB = unitOfWork.Repository<HREmpLeaveB>().Queryable().Where(w => w.InYear == InYear).ToList();
                var LeaveType = unitOfWork.Repository<HRLeaveType>().Queryable().ToList();

                EmpLeaveB = EmpLeaveB.Where(w => _listStaff.Where(x => w.EmpCode == x.EmpCode).Any()).ToList();
                var ListLeave_Rate = unitOfWork.Repository<HRLeaveProRate>().Queryable().ToList();
                List<HRSetEntitleD> ListetEntitleD = unitOfWork.Repository<HRSetEntitleD>().Queryable().Where(w => w.IsProRate == true).ToList();
                EmpLeaveB = EmpLeaveB.Where(w => w.InYear == InYear && ListetEntitleD.Where(x => x.LeaveCode == w.LeaveCode).Any()).ToList();
                List<HREmpLeaveD> _ListLeaveD = GetLeaveToken(StartDate, ToDate).ToList();
                //_ListLeaveD = _ListLeaveD.Where(w => w.LeaveDate.Year == InYear).ToList();// && ListetEntitleD.Where(x => x.LeaveCode == w.LeaveCode).Any()).ToList();
                //DateTime EndDate = new DateTime(InYear, 12, 31);
                //int CountMonth = DateTimeHelper.GetMonth(FromDate, EndDate);
                string approved = SYDocumentStatus.APPROVED.ToString();
                List<HRClaimLeave> LstEmpClaim = unitOfWork.Repository<HRClaimLeave>().Queryable().Where(w => w.Status == approved && w.WorkingDate.Year == InYear && (w.IsUsed.Value == true || w.IsExpired.Value == false)).ToList();

                foreach (var emp in EmpLeaveB)
                {
                    var Staff = _listStaff.FirstOrDefault(w => w.EmpCode == emp.EmpCode);
                    if (Staff == null || string.IsNullOrEmpty(Staff.PayParam)) continue;
                    PRParameter payParam = ListParam.FirstOrDefault(w => w.Code == Staff.PayParam);
                    var item = LeaveType.FirstOrDefault(w => w.Code == emp.LeaveCode);
                    if (payParam == null) continue;
                    ClsPeriodLeave periodLeave = new ClsPeriodLeave();
                    periodLeave.payParam = payParam;
                    periodLeave.ListLeaveProRate = ListLeave_Rate.Where(w => w.LeaveType == emp.LeaveCode && (string.IsNullOrEmpty(w.EntitleType) || w.EntitleType == Staff.TemLeave)).ToList();
                    periodLeave.LeaveType = item;
                    periodLeave = GetPeriod(periodLeave, Staff, ListetEntitleD, InYear, emp.LeaveCode);

                    var EmpClaim = LstEmpClaim.Where(w => w.EmpCode == emp.EmpCode).ToList();
                    decimal Balance = periodLeave.SeniorityBalance + periodLeave.Entitle;//emp.DayEntitle.Value;
                    if (item != null)
                    {
                        if (item.IsCurrent == false)
                        {
                            Balance = periodLeave.Balance;
                        }
                    }
                    HREmpLeaveB lB = Calculate_Token(emp, _ListLeaveD, LeaveType, payParam, Balance, EmpClaim);

                    var ObjMatch = emp;
                    //ObjMatch.CurrentEntitle = periodLeave.Balance + periodLeave.SeniorityBalance;
                    ObjMatch.Current_AL = periodLeave.Balance - lB.Balance.Value + periodLeave.SeniorityBalance;
                    //ObjMatch.YTD = ObjMatch.DayEntitle + periodLeave.SeniorityBalance;
                    ObjMatch.SeniorityBalance = periodLeave.SeniorityBalance;
                    ObjMatch.ForwardUse = lB.ForwardUse;
                    ObjMatch.DayLeave = lB.Balance;
                    ObjMatch.TakenHour = lB.TakenHour;
                    ObjMatch.DayEntitle = periodLeave.Entitle;
                    //ObjMatch.Balance = ObjMatch.DayEntitle + periodLeave.SeniorityBalance - lB.Balance;
                    ObjMatch = Calculate_Balance(ObjMatch);
                    DBI.HREmpLeaveBs.Attach(ObjMatch);
                    DBI.Entry(ObjMatch).Property(w => w.Balance).IsModified = true;
                    DBI.Entry(ObjMatch).Property(w => w.DayLeave).IsModified = true;
                    DBI.Entry(ObjMatch).Property(w => w.Current_AL).IsModified = true;
                    //DBI.Entry(ObjMatch).Property(w => w.CurrentEntitle).IsModified = true;
                    DBI.Entry(ObjMatch).Property(w => w.YTD).IsModified = true;
                    DBI.Entry(ObjMatch).Property(w => w.SeniorityBalance).IsModified = true;
                    DBI.Entry(ObjMatch).Property(w => w.TakenHour).IsModified = true;
                    DBI.Entry(ObjMatch).Property(w => w.ForwardUse).IsModified = true;
                    DBI.Entry(ObjMatch).Property(w => w.DayEntitle).IsModified = true;
                }
                DBI.SaveChanges();
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
                DBI.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string GenerateLeave(string EmpCode)
        {
            try
            {
                using (DB = new HumicaDBContext())
                {
                    DB.Configuration.AutoDetectChangesEnabled = false;

                    var policy = unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
                    DateTime from = policy.LFromDate;
                    DateTime to = policy.LToDate;
                    int year = Filter.INYear;

                    if (from.Year != to.Year) year = Filter.INYear - 1;
                    DateTime fromDate = new DateTime(year, from.Month, from.Day);

                    ListEmpLeaveB = new List<HREmpLeaveB>();
                    string[] empCodes = EmpCode.Split(';');

                    var empLeave = unitOfWork.Repository<HREmpLeaveB>().Queryable()
                        .Where(x => x.InYear == Filter.INYear).ToList();

                    var temLeave = unitOfWork.Repository<HRSetEntitleH>().Queryable().ToList();
                    var leaveTypes = unitOfWork.Repository<HRLeaveType>().Queryable().ToList();
                    var employees = unitOfWork.Repository<HRStaffProfile>().Queryable()
                        .Where(w => (w.Status == "A" || (w.Status == "I" && DbFunctions.TruncateTime(w.DateTerminate) > fromDate)))
                        .ToList();

                    foreach (var code in empCodes)
                    {
                        var trimmedCode = code.Trim();
                        if (string.IsNullOrEmpty(trimmedCode)) continue;
                        var objEmpB = DB.HREmpLeaveBs.Where(w => w.EmpCode == trimmedCode && w.LeaveCode != "AL").ToList();
                        if (objEmpB.Any())
                        {
                            DB.HREmpLeaveBs.RemoveRange(objEmpB);
                        }
                        var staff = employees.FirstOrDefault(w => w.EmpCode == trimmedCode);
                        if (staff != null)
                        {
                            PRParameter payParam = DB.PRParameters.Find(staff.PayParam);
                            if (payParam == null)
                            {
                                return "Invalid Parameter: " + trimmedCode;
                            }

                            if (staff.TemLeave == null)
                            {
                                return "Please assign Template Leave to EmpCode: " + trimmedCode;
                            }

                            if (temLeave.Any(w => w.Code == staff.TemLeave))
                            {
                                Generate_Leave_Entitle(trimmedCode, Filter.INYear, staff, leaveTypes);
                            }
                        }
                    }

                    foreach (var leave in ListEmpLeaveB)
                    {
                        var staff = employees.FirstOrDefault(w => w.EmpCode == leave.EmpCode);
                        if (staff != null)
                        {
                            EmpLeaveB = new HREmpLeaveB();
                            EmpLeaveB = leave;
                            EmpLeaveB.CreateOn = DateTime.Now;
                            EmpLeaveB.CreateBy = User.UserName;
                            DB.HREmpLeaveBs.Add(EmpLeaveB);
                        }
                    }
                    //var empTerminate = empLeave.Where(w => !employees.Any(x => w.EmpCode == x.EmpCode)).ToList();
                    //if (empTe​rminate.Any())
                    //    DB.HREmpLeaveBs.RemoveRange(empTerminate);
                    var empTerminateCodes = empLeave.Where(w => !employees.Any(x => w.EmpCode == x.EmpCode))
                                 .Select(w => w.EmpCode).ToList();
                    if (empTerminateCodes.Any())
                    {
                        var empTerminate = DB.HREmpLeaveBs.Where(e => empTerminateCodes.Contains(e.EmpCode)).ToList();
                        if (empTerminate.Any())
                        {
                            DB.HREmpLeaveBs.RemoveRange(empTerminate);
                        }
                    }
                    DB.SaveChanges();

                    foreach (var code in empCodes)
                    {
                        if (!string.IsNullOrEmpty(code.Trim()))
                        {
                            GET_Leave_LeaveBalance(code.Trim(), Filter.INYear);
                        }
                    }

                    return SYConstant.OK;
                }
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
                log.DocurmentAction = Filter.INYear.ToString();
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
                log.DocurmentAction = Filter.INYear.ToString();
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
        public string TransferLeave(string EmpCode)
        {
            try
            {
                var DBI = new HumicaDBContext();
                ListEmpLeaveB = new List<HREmpLeaveB>();
                string[] Emp = EmpCode.Split(';');
                //var EmpLeave = DB.HREmpLeaveBs.ToList();
                var LEmpLeave = new List<HREmpLeaveB>();
                //LEmpLeave = EmpLeave.ToList();
                LEmpLeave = DB.HREmpLeaveBs.Where(x => x.InYear == Filter.TYear).ToList();
                if (Filter.FYear >= Filter.TYear)
                {
                    return "INVALID_YEAR";
                }
                foreach (var Code in Emp)
                {
                    if (Code.Trim() != "")
                    {
                        var result = LEmpLeave.Where(w => w.EmpCode == Code && w.InYear == Filter.TYear && w.LeaveCode == Filter.LeaveType).ToList();
                        var resultFor = ListForward.Where(w => w.EmpCode == Code).ToList().FirstOrDefault();
                        foreach (var item in result)
                        {
                            if (resultFor.ForWard <= 0)
                            {
                                item.Forward = 0;
                                item.ForWardExp = new DateTime(1900, 1, 1);
                            }
                            else
                            {
                                item.Forward = resultFor.ForWard;
                                item.ForWardExp = Filter.ForwardExp;
                            }

                            DB.HREmpLeaveBs.Attach(item);
                            DB.Entry(item).Property(w => w.Forward).IsModified = true;
                            DB.Entry(item).Property(w => w.ForWardExp).IsModified = true;

                        }
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
                log.DocurmentAction = Filter.TYear.ToString();
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
                log.DocurmentAction = Filter.TYear.ToString();
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
                log.DocurmentAction = Filter.TYear.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public void getLeaveDay(string EmpCode)
        {
            double NoLeave = 0;
            double PH = 0;
            double Rest = 0;
            double LHour = 0;
            double WHour = 0;
            if (ListEmpLeaveD.Count > 0)
            {
                string PayPram = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode).PayParam;
                var Pay = DB.PRParameters.Find(PayPram);
                WHour = Convert.ToDouble(Pay.WHOUR);
                foreach (var item in ListEmpLeaveD)
                {
                    // LHour = Convert.ToDouble(item.LHour)/WHour;
                    LHour = WHour;// Convert.ToDouble(item.LHour);
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
        public string CreateLeaveRequest(string id)
        {
            try
            {
                DB = new HumicaDBContext();
                DB.Configuration.AutoDetectChangesEnabled = false;
                var LeaveD = new List<HREmpLeaveD>();
                //var StaffList = DB.HRStaffProfiles.ToList();
                var staff = DB.HRStaffProfiles.FirstOrDefault(x => x.EmpCode == HeaderStaff.EmpCode);
                string Status = SYDocumentStatus.APPROVED.ToString();
                string Reject = SYDocumentStatus.REJECTED.ToString();
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                var lstempLeave = DB.HREmpLeaves.Where(w => w.Status != Cancel && w.Status != Reject && w.EmpCode == HeaderStaff.EmpCode).ToList();
                HeaderEmpLeave.EmpCode = HeaderStaff.EmpCode;
                var ATSche = DB.ATEmpSchedules.Where(w => w.EmpCode == HeaderEmpLeave.EmpCode
                            && DbFunctions.TruncateTime(w.TranDate) >= HeaderEmpLeave.FromDate.Date
                            && DbFunctions.TruncateTime(w.TranDate) <= HeaderEmpLeave.ToDate.Date).ToList();
                int Increment = GenerateLeaveObject.GetLastIncrement();
                //int Increment;
                //if (lstempLeave.Count == 0) Increment = 0;
                //else Increment = lstempLeave.Max(w => w.Increment);
                if (string.IsNullOrEmpty(HeaderEmpLeave.Reason))
                    return "REASON";
                if (ListEmpLeaveD.Count <= 0)
                    return "INV_DOC";
                decimal balance = EmpLeaveB.Balance ?? 0;
                if (HeaderEmpLeave.FromDate.Date > HeaderEmpLeave.ToDate.Date)
                    return "INVALID_DATE";
                var leaveH = lstempLeave.ToList();

                if (leaveH.Where(w => ((w.FromDate.Date >= HeaderEmpLeave.FromDate.Date && w.FromDate.Date <= HeaderEmpLeave.ToDate.Date) ||
                  (w.ToDate.Date >= HeaderEmpLeave.FromDate.Date && w.ToDate.Date <= HeaderEmpLeave.ToDate.Date) ||
                        (HeaderEmpLeave.FromDate.Date >= w.FromDate.Date && HeaderEmpLeave.FromDate.Date <= w.ToDate.Date) || (HeaderEmpLeave.ToDate.Date >= w.FromDate.Date && HeaderEmpLeave.ToDate.Date <= w.ToDate.Date))).Any())
                {
                    var EmpLeaveD = DB.HREmpLeaveDs.Where(w => w.EmpCode == HeaderStaff.EmpCode).ToList();
                    EmpLeaveD = EmpLeaveD.Where(w => w.LeaveDate.Date >= HeaderEmpLeave.FromDate.Date && w.LeaveDate.Date <= HeaderEmpLeave.ToDate.Date).ToList();
                    var Result = ListEmpLeaveD.Where(w => EmpLeaveD.Where(x => x.LeaveDate.Date == w.LeaveDate.Date && w.Remark != "Hours").Any()).ToList();
                    var Result_ = ListEmpLeaveD.Where(w => EmpLeaveD.Where(x => x.LeaveDate.Date == w.LeaveDate.Date && w.Remark != "FullDay").Any()).ToList();
                    var ResultHour = ListEmpLeaveD.Where(w => EmpLeaveD.Where(x => x.LeaveDate.Date == w.LeaveDate.Date && (x.Remark == "Hours" || w.Remark == "Hours")).Any()).ToList();
                    var empATSche = ATSche.Where(w => ResultHour.Where(x => w.TranDate == x.LeaveDate && w.EmpCode == HeaderStaff.EmpCode).Any()).ToList();
                    if (Result.Where(w => w.Remark == "FullDay").Any())
                        if (ResultHour.Count > 0) return "INV_DATE";
                    if (EmpLeaveD.Where(w => Result.Where(x => x.LeaveDate.Date == w.LeaveDate.Date && x.Remark == w.Remark).Any()).Any())
                        return "INV_DATE";
                    if (EmpLeaveD.Where(w => ResultHour.Where(x => x.LeaveDate.Date == w.LeaveDate.Date).Any()).Any())
                    {
                        if (empATSche.Count > 0)
                        {
                            foreach (var read in empATSche)
                            {
                                TimeSpan time = new TimeSpan(0, 4, 0, 0);
                                DateTime brackstart = read.IN1.Value.Add(time);
                                DateTime brackend = read.OUT1.Value.Subtract(time);
                                if (read.Flag1 == 1 && read.Flag2 == 1 && ResultHour.Count > 0)
                                {
                                    if (EmpLeaveD.Where(w => Result_.Where(x => x.LeaveDate.Date == read.TranDate.Date && ((read.IN1 >= w.StartTime && read.IN1 <= w.EndTime)
                                                        || (read.OUT1 >= w.StartTime && read.OUT1 <= w.EndTime) || (w.StartTime >= read.IN1 && w.StartTime <= read.OUT1)
                                                        || (w.EndTime >= read.IN1 && w.EndTime <= read.OUT1)) && x.Remark == "Morning").Any()).Any()) return "INV_DATE";
                                    else if (EmpLeaveD.Where(w => Result_.Where(x => x.LeaveDate.Date == read.TranDate.Date && ((read.IN2 >= w.StartTime && read.IN2 <= w.EndTime)
                                                        || (read.OUT2 >= w.StartTime && read.OUT2 <= w.EndTime) || (w.StartTime >= read.IN2 && w.StartTime <= read.OUT2)
                                                        || (w.EndTime >= read.IN2 && w.EndTime <= read.OUT2)) && x.Remark == "Afternoon").Any()).Any()) return "INV_DATE";
                                }
                                else if (read.Flag1 == 1 && read.Flag2 == 2 && ResultHour.Count > 0)
                                {
                                    if (EmpLeaveD.Where(w => Result_.Where(x => x.LeaveDate.Date == read.TranDate.Date && ((read.IN1 >= w.StartTime && read.IN1 <= w.EndTime)
                                                        || (brackstart >= w.StartTime && brackstart <= w.EndTime) || (w.StartTime >= read.IN1 && w.StartTime <= brackstart)
                                                        || (w.EndTime >= read.IN1 && w.EndTime <= brackstart)) && x.Remark == "Morning").Any()).Any()) return "INV_DATE";
                                    else if (EmpLeaveD.Where(w => Result_.Where(x => x.LeaveDate.Date == read.TranDate.Date && ((brackend >= w.StartTime && brackend <= w.EndTime)
                                                        || (read.OUT2 >= w.StartTime && read.OUT2 <= w.EndTime) || (w.StartTime >= brackend && w.StartTime <= read.OUT2)
                                                        || (w.EndTime >= brackend && w.EndTime <= read.OUT2)) && x.Remark == "Afternoon").Any()).Any()) return "INV_DATE";
                                }
                            }
                        }
                    }
                    if (EmpLeaveD.Where(w => w.Remark == "FullDay").Any())
                    {
                        if (ResultHour.Count > 0) return "INV_DATE";
                        if (Result.Count > 0) return "INV_DATE";
                    }
                    if (ResultHour.Count > 0)
                    {
                        var _EmpLeaveItem = EmpLeaveD.Where(w => w.Remark == "Hours").ToList();
                        if (_EmpLeaveItem.Any())
                        {
                            foreach (var item in ResultHour)
                            {
                                if (_EmpLeaveItem.Where(w => w.StartTime <= item.StartTime.Value.AddMinutes(1) && w.EndTime >= item.StartTime.Value.AddMinutes(1)).Any())
                                    return "INV_DATE";
                                else if (_EmpLeaveItem.Where(w => w.StartTime <= item.EndTime.Value.AddMinutes(1) && w.EndTime >= item.EndTime.Value.AddMinutes(1)).Any())
                                    return "INV_DATE";
                            }
                        }
                    }
                    if (ResultHour.Count > 0)
                    {
                        if (EmpLeaveD.Where(w => w.Remark == "FullDay").Any()) return "INV_DATE";
                        if (Result.Where(w => w.Remark == "FullDay").Any()) return "INV_DATE";
                    }
                }
                getLeaveDay(HeaderStaff.EmpCode);
                var LeaveType = DB.HRLeaveTypes.Find(HeaderEmpLeave.LeaveCode);
                string leaveCode = HeaderEmpLeave.LeaveCode;
                if (LeaveType != null && LeaveType.IsParent == true && string.IsNullOrEmpty(LeaveType.Parent)) leaveCode = LeaveType.Parent;
                PRParameter payParam = DB.PRParameters.FirstOrDefault(w => w.Code == staff.PayParam);
                bool IsParent = false;
                if (LeaveType != null && LeaveType.IsParent == true && !string.IsNullOrEmpty(LeaveType.Parent))
                {
                    leaveCode = LeaveType.Parent;
                    IsParent = true;
                }
                var ListLeave_Rate = DB.HRLeaveProRates.Where(w => (w.EntitleType == staff.TemLeave) || (string.IsNullOrEmpty(w.EntitleType))).ToList();
                //if (LeaveType.IsCurrent == false)
                //{
                //    ClsPeriodLeave periodLeave = new ClsPeriodLeave();
                //    List<HRSetEntitleD> ListetEntitleD = DB.HRSetEntitleDs.Where(w => w.CodeH == staff.TemLeave).ToList();
                //    periodLeave.payParam = payParam;
                //    periodLeave.ListLeaveProRate = ListLeave_Rate;
                //    if (IsParent)
                //    {
                //        leaveCode = LeaveType.Parent;
                //    }
                //    periodLeave = GetPeriod(periodLeave, staff, ListetEntitleD, HeaderEmpLeave.ToDate.Year, leaveCode);

                //    if (periodLeave.Balance - Convert.ToDecimal(HeaderEmpLeave.NoDay) < 0)
                //        return "INV_BALANCE";
                //}
                //if (LeaveType.IsOverEntitle == false)
                //{
                //    if (IsParent)
                //    {
                //        var obj = DB.HREmpLeaveBs.FirstOrDefault(w => w.LeaveCode == HeaderEmpLeave.LeaveCode && w.EmpCode == HeaderEmpLeave.EmpCode && w.InYear == HeaderEmpLeave.ToDate.Year);
                //        if (obj != null)
                //        {
                //            if (obj.Balance - Convert.ToDecimal(HeaderEmpLeave.NoDay) < 0) return "INT_Entile";
                //        }
                //        else
                //        {
                //            ClsPeriodLeave periodLeave = new ClsPeriodLeave();
                //            List<HRSetEntitleD> ListetEntitleD = DB.HRSetEntitleDs.Where(w => w.CodeH == staff.TemLeave).ToList();
                //            periodLeave.payParam = payParam;
                //            periodLeave.ListLeaveProRate = ListLeave_Rate;
                //            periodLeave = GetPeriod(periodLeave, staff, ListetEntitleD, HeaderEmpLeave.ToDate.Year, HeaderEmpLeave.LeaveCode);
                //            if (periodLeave.Balance - Convert.ToDecimal(HeaderEmpLeave.NoDay) < 0)
                //                return "INV_BALANCE";
                //        }
                //        leaveCode = LeaveType.Parent;
                //    }
                //   var Policy = DB.ATPolicies.FirstOrDefault();
                //    DateTime From = Policy.LFromDate;
                //    DateTime To = Policy.LToDate;
                //    int year = HeaderEmpLeave.ToDate.Year;
                //    if (From.Year != To.Year) year = HeaderEmpLeave.ToDate.Year - 1;
                //    DateTime FromDate = new DateTime(year, From.Month, From.Day);
                //    DateTime ToData = new DateTime(HeaderEmpLeave.ToDate.Year, To.Month, To.Day);

                //    List<HREmpLeaveD> _ListLeaveD = GetLeaveToken(FromDate, ToData).ToList();
                //    _ListLeaveD = _ListLeaveD.Where(w => w.EmpCode == HeaderStaff.EmpCode).ToList();
                //    var EmpLeaveB_ = DB.HREmpLeaveBs.FirstOrDefault(w => w.InYear == HeaderEmpLeave.ToDate.Year && w.LeaveCode == leaveCode && w.EmpCode == HeaderStaff.EmpCode);
                //    if (EmpLeaveB_ == null) return "INT_Entile";
                //    if ((balance - Convert.ToDecimal(HeaderEmpLeave.NoDay)) < 0 || IsParent)
                //    {
                //        _ListLeaveD.AddRange(GetLeave_D(ListEmpLeaveD, HeaderStaff.EmpCode, leaveCode, HeaderEmpLeave.Units));
                //        var ListLeaveType = DB.HRLeaveTypes.ToList();
                //        List<HRClaimLeave> LstEmpClaim = DB.HRClaimLeaves.Where(w => w.Status == Status && w.WorkingDate.Year == HeaderEmpLeave.ToDate.Year && (w.IsUsed.Value == true || w.IsExpired.Value == false)).ToList();
                //        var EmpClaim = LstEmpClaim.Where(w => w.EmpCode == HeaderEmpLeave.EmpCode).ToList();
                //        HREmpLeaveB lB = Calculate_Token(EmpLeaveB_, _ListLeaveD, ListLeaveType, payParam, EmpLeaveB_.YTD ?? EmpLeaveB_.DayEntitle ?? 0, LstEmpClaim);
                //        if ((EmpLeaveB_.YTD - lB.Balance) < 0)
                //            return "INT_Entile";
                //    }
                //}
                if (LeaveType != null)
                {
                    if (LeaveType.Probation == true)
                    {
                        if (staff.Probation.Value.Date > HeaderEmpLeave.FromDate.Date)
                            return "CANNOT_PROBATION";
                    }
                }
                HeaderEmpLeave.Status = Status;
                HeaderEmpLeave.Attachment = HeaderEmpLeave.Attachment;
                HeaderEmpLeave.RequestDate = DateTime.Now;
                HeaderEmpLeave.Increment = Increment + 1;
                HeaderEmpLeave.CreateBy = User.UserName;
                HeaderEmpLeave.CreateOn = DateTime.Now;
                DB.HREmpLeaves.Add(HeaderEmpLeave);

                DB.HREmpLeaveDs.AddRange(GetLeave_D(ListEmpLeaveD, HeaderStaff.EmpCode, HeaderEmpLeave.LeaveCode, HeaderEmpLeave.Units));
                var ListAtt = ATSche.ToList();
                foreach (var item in ListEmpLeaveD)
                {
                    if (ListAtt.Where(w => w.TranDate.Date == item.LeaveDate.Date).Any())
                    {
                        var attFirst = ListAtt.First(w => w.TranDate.Date == item.LeaveDate.Date);
                        attFirst.LeaveDesc = "";
                        attFirst.LeaveCode = HeaderEmpLeave.LeaveCode;
                        attFirst.LeaveNo = Increment + 1;
                        DB.ATEmpSchedules.Attach(attFirst);
                        DB.Entry(attFirst).Property(w => w.LeaveDesc).IsModified = true;
                        DB.Entry(attFirst).Property(w => w.LeaveCode).IsModified = true;
                        DB.Entry(attFirst).Property(w => w.LeaveNo).IsModified = true;
                    }
                }
                string Approval = SYDocumentStatus.APPROVED.ToString();
                List<HRClaimLeave> _listClaim = DB.HRClaimLeaves.Where(w => w.EmpCode == HeaderEmpLeave.EmpCode
                && (w.IsExpired.Value != true || w.IsUsed.Value != true) && w.Status == Approval).ToList();
                DateTime DateNow = DateTime.Now;
                bool Isused = false;
                foreach (var claim in _listClaim.ToList().OrderBy(w => w.WorkingDate))
                {
                    if (Isused == true) continue;
                    if (claim.Expired.Value.Date < DateNow.Date)
                    {
                        claim.IsExpired = true;
                        DB.HRClaimLeaves.Attach(claim);
                        DB.Entry(claim).Property(x => x.IsExpired).IsModified = true;
                    }
                    else
                    {
                        Isused = true;
                        claim.IsUsed = true;
                        claim.DocumentRef = HeaderEmpLeave.Increment.ToString();
                        DB.HRClaimLeaves.Attach(claim);
                        DB.Entry(claim).Property(x => x.IsUsed).IsModified = true;
                        DB.Entry(claim).Property(x => x.DocumentRef).IsModified = true;
                    }
                }
                int row = DB.SaveChanges();
                ReGenerateLeaveToken(HeaderEmpLeave.EmpCode, HeaderEmpLeave.FromDate, HeaderEmpLeave.ToDate);
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = HeaderEmpLeave.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string EditLeaveRequest(int id, bool ESS = false)
        {
            OnLoad();
            try
            {
                var LeaveType = unitOfWork.Repository<HRLeaveType>().Queryable().FirstOrDefault(w => w.Code == HeaderEmpLeave.LeaveCode);
                decimal balance = EmpLeaveB.Balance ?? 0;
                var objMatch = unitOfWork.Repository<HREmpLeave>().Queryable().FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                    return "LEAVE_NE";
                HeaderEmpLeave.EmpCode = objMatch.EmpCode;
                var staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                if (staff == null) return "INV_STAFF";
                if (string.IsNullOrEmpty(staff.PayParam)) return "INV_PARAMETER";
                if (!ListEmpLeaveD.Any())
                    return "INV_DOC";
                if (string.IsNullOrEmpty(HeaderEmpLeave.Reason))
                    return "REASON";
                if (HeaderEmpLeave.FromDate.Date > HeaderEmpLeave.ToDate)
                    return "INVALID_DATE";
                var payParam = unitOfWork.Repository<PRParameter>().Queryable().FirstOrDefault(w => w.Code == staff.PayParam);
                if (payParam == null) return "INV_PARAMETER";
                if (ESS)
                {
                    if (LeaveType.NotAllowRequest == true) return "NOT_ALLOW_REQUEST";

                    if ((HeaderEmpLeave.NoDay ?? 0 + HeaderEmpLeave.NoPH ?? 0 + HeaderEmpLeave.NoRest ?? 0) < LeaveType.MinTaken)
                    {
                        MessageError = LeaveType.MinTaken.ToString();
                        return "MUSTBE_OVER";
                    }
                    if (HeaderEmpLeave.Urgent == false)
                    {
                        if (LeaveType.BeforeDay > 0)
                        {
                            if (HeaderEmpLeave.Units == "Day")
                            {
                                int day = LeaveType.BeforeDay.Value;
                                DateTime currentDate = DateTime.Now;
                                DateTime requiredDate = HeaderEmpLeave.FromDate.AddDays(-1);

                                var message = DP.SYMessages.FirstOrDefault(w => w.MessageID == "REQUIRE_DATE" && w.Lang == User.Lang);
                                if (message == null)
                                {
                                    return "Message not found.";
                                }
                                ClsValidateLeave LeaveDValidate = new ClsValidateLeave();
                                var Leaved = LeaveDValidate.Get_LeaveDay(objMatch.EmpCode, currentDate, requiredDate, HeaderEmpLeave.LeaveCode, HeaderEmpLeave.Units);
                                var LeaveNo = Leaved.Where(w => w.Status == "Leave").Count();
                                if (LeaveType.BeforeDay > LeaveNo)
                                {
                                    MessageError = LeaveType.BeforeDay.ToString();
                                    return "REQUIRE_DATE";
                                    //return string.Format(message.Description, _LeaveType.BeforeDay);
                                }
                            }
                            else
                            {
                                decimal WHour_ = Convert.ToDecimal(payParam.WHOUR) / 2.00M;
                                foreach (var item in ListEmpLeaveD)
                                {
                                    double totals = item.EndTime.Value.Subtract(item.StartTime.Value).TotalHours;
                                    if ((decimal)totals > WHour_)
                                    {
                                        var str = "ការស្នើសុំមិនអាចលើសពីកន្លះថ្ងៃ";
                                        str += "\nThe requests cannot be exceeded half day";
                                        return str;
                                    }
                                }
                            }
                        }
                    }
                    DateTime D_Now = DateTime.Now;
                    var B_Date = D_Now.AddDays(-Convert.ToDouble(LeaveType.Beforebackward));
                    if (LeaveType.Allowbackward == false && HeaderEmpLeave.FromDate.Date < D_Now.Date)
                        return "NOT_ALLOW_BACKWARD";
                    else if (LeaveType.Allowbackward == true && HeaderEmpLeave.FromDate.Date < B_Date.Date)
                        return "NOT_ALLOW_BACKWARD";
                    if (LeaveType.ReqDocument == true && LeaveType.NumDay <= HeaderEmpLeave.NoDay)
                    {
                        if (HeaderEmpLeave.Attachment == null)
                            return "REQUIRED_DOCUMENT";
                    }
                    if (LeaveType != null)
                    {
                        if (LeaveType.Probation == true)
                        {
                            if (staff.Probation.Value.Date > HeaderEmpLeave.FromDate.Date)
                                return "CANNOT_PROBATION";
                        }
                    }
                    string leaveCode = HeaderEmpLeave.LeaveCode;
                    if (LeaveType != null && LeaveType.IsParent == true && !string.IsNullOrEmpty(LeaveType.Parent)) leaveCode = LeaveType.Parent;
                    var MSM = ValidateEntile(LeaveType, leaveCode, payParam, staff);
                    if (MSM != SYConstant.OK)
                    {
                        return MSM;
                    }
                }
                var MSM_ = EvaluateLeave(ListEmpLeaveD, objMatch.EmpCode, HeaderEmpLeave.FromDate, HeaderEmpLeave.ToDate, HeaderEmpLeave.Units, objMatch.Increment, true);
                if (MSM_ != SYConstant.OK)
                {
                    return MSM_;
                }
                var objEmp = unitOfWork.Repository<HREmpLeaveD>().Queryable().Where(w => w.LeaveTranNo == objMatch.Increment && w.EmpCode == objMatch.EmpCode).ToList();
                foreach (var detail in objEmp)
                {
                    unitOfWork.Delete(detail);
                }

                getLeaveDay(objMatch.EmpCode);
                objMatch.LeaveCode = HeaderEmpLeave.LeaveCode;
                objMatch.FromDate = HeaderEmpLeave.FromDate;
                objMatch.ToDate = HeaderEmpLeave.ToDate;
                objMatch.Units = HeaderEmpLeave.Units;
                objMatch.TaskHand_Over = HeaderEmpLeave.TaskHand_Over;
                objMatch.Attachment = HeaderEmpLeave.Attachment;
                objMatch.NoDay = HeaderEmpLeave.NoDay;
                objMatch.NoPH = HeaderEmpLeave.NoPH;
                objMatch.NoRest = HeaderEmpLeave.NoRest;
                objMatch.ResonToCancel = HeaderEmpLeave.ResonToCancel;
                objMatch.Reason = HeaderEmpLeave.Reason;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                unitOfWork.Update(objMatch);

                decimal WHour = Convert.ToDecimal(payParam.WHOUR);
                int line = 0;
                var listAtt = unitOfWork.Repository<ATEmpSchedule>().Queryable().Where(w => w.EmpCode == HeaderEmpLeave.EmpCode).ToList();
                foreach (var item in ListEmpLeaveD)
                {
                    line += 1;
                    var result = new HREmpLeaveD
                    {
                        LeaveTranNo = objMatch.Increment,
                        EmpCode = HeaderStaff.EmpCode,
                        LeaveCode = HeaderEmpLeave.LeaveCode,
                        LeaveDate = item.LeaveDate,
                        CutMonth = item.CutMonth,
                        Status = item.Status,
                        Remark = item.Remark,
                        LineItem = line,
                        LHour = item.LHour,
                        CreateBy = User.UserName,
                        CreateOn = DateTime.Now
                    };
                    if (HeaderEmpLeave.Units != "Day" && item.StartTime.HasValue && item.StartTime.Value.Year == 100)
                    {
                        item.StartTime = item.LeaveDate + item.StartTime.Value.TimeOfDay;
                        item.EndTime = item.LeaveDate + item.EndTime.Value.TimeOfDay;
                    }
                    if (HeaderEmpLeave.Units != "Day")
                    {
                        result.Remark = "Hours";
                        result.StartTime = item.StartTime;
                        result.EndTime = item.EndTime;
                    }

                    if (item.Remark == "Morning" || item.Remark == "Afternoon")
                        result.LHour = WHour / 2;
                    unitOfWork.Add(result);
                    var attFirst = listAtt.FirstOrDefault(w => w.TranDate.Date == result.LeaveDate.Date);
                    if (attFirst != null)
                    {
                        attFirst.LeaveDesc = "";
                        attFirst.LeaveCode = result.LeaveCode;
                        attFirst.LeaveNo = result.LeaveTranNo;
                        unitOfWork.Update(attFirst);
                    }
                }
                unitOfWork.Save();
                ReGenerateLeaveToken(HeaderEmpLeave.EmpCode, HeaderEmpLeave.FromDate, HeaderEmpLeave.ToDate);
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderEmpLeave.EmpCode, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderEmpLeave.EmpCode, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderEmpLeave.EmpCode, SYActionBehavior.EDIT.ToString(), e, true);
            }
        }
        public string ESSRequestLeave(string id, string URL)
        {
            OnLoad();
            MessageError = "";
            try
            {
                DB = new HumicaDBContext();
                var DBI = new HumicaDBContext();
                var LeaveD = new List<HREmpLeaveD>();
                var LeaveType = DB.HRLeaveTypes.Find(HeaderEmpLeave.LeaveCode);
                if (LeaveType == null)
                    return "INV_DOC";
                if (ListEmpLeaveD.Count <= 0)
                    return "INV_DOC";
                if (string.IsNullOrEmpty(HeaderEmpLeave.Reason))
                    return "REASON";
                if (LeaveType.NotAllowRequest == true)
                {
                    return "NOT_ALLOW_REQUEST";
                }
                if ((HeaderEmpLeave.NoDay ?? 0 + HeaderEmpLeave.NoPH ?? 0 + HeaderEmpLeave.NoRest ?? 0) < LeaveType.MinTaken)
                {
                    MessageError = LeaveType.MinTaken.ToString();
                    return "MUSTBE_OVER";
                }
                decimal balance = EmpLeaveB.Balance ?? 0;
                var staff = DB.HRStaffProfiles.FirstOrDefault(x => x.EmpCode == HeaderStaff.EmpCode);
                var Approver = new ExDocApproval();
                if (HeaderEmpLeave.Urgent == false)
                {
                    if (LeaveType.BeforeDay > 0)
                    {
                        if (HeaderEmpLeave.Units == "Day")
                        {
                            int day = LeaveType.BeforeDay.Value;
                            DateTime currentDate = DateTime.Now;
                            DateTime requiredDate = HeaderEmpLeave.FromDate.AddDays(-1);

                            var message = DP.SYMessages.FirstOrDefault(w => w.MessageID == "REQUIRE_DATE" && w.Lang == User.Lang);
                            if (message == null)
                            {
                                return "Message not found.";
                            }
                            ClsValidateLeave LeaveDValidate = new ClsValidateLeave();
                            var Leaved = LeaveDValidate.Get_LeaveDay(HeaderStaff.EmpCode, currentDate, requiredDate, HeaderEmpLeave.LeaveCode, HeaderEmpLeave.Units);
                            var LeaveNo = Leaved.Where(w => w.Status == "Leave").Count();
                            if (LeaveType.BeforeDay > LeaveNo)
                            {
                                MessageError = LeaveType.BeforeDay.ToString();
                                return "REQUIRE_DATE";
                                //return string.Format(message.Description, _LeaveType.BeforeDay);
                            }
                        }
                        else
                        {
                            PRParameter payParam_ = DB.PRParameters.FirstOrDefault(w => w.Code == staff.PayParam);
                            decimal WHour = Convert.ToDecimal(payParam_.WHOUR) / 2.00M;
                            foreach (var item in ListEmpLeaveD)
                            {
                                double totals = item.EndTime.Value.Subtract(item.StartTime.Value).TotalHours;
                                if ((decimal)totals > WHour)
                                {
                                    var str = "ការស្នើសុំមិនអាចលើសពីកន្លះថ្ងៃ";
                                    str += "\nThe requests cannot be exceeded half day";
                                    return str;
                                }
                            }
                        }

                    }
                }
                if (HeaderEmpLeave.FromDate.Date > HeaderEmpLeave.ToDate.Date)
                    return "INVALID_DATE";
                DateTime D_Now = DateTime.Now;
                var B_Date = D_Now.AddDays(-Convert.ToDouble(LeaveType.Beforebackward));
                if (LeaveType.Allowbackward == false && HeaderEmpLeave.FromDate.Date < D_Now.Date)
                    return "NOT_ALLOW_BACKWARD";
                else if (LeaveType.Allowbackward == true && HeaderEmpLeave.FromDate.Date < B_Date.Date)
                    return "NOT_ALLOW_BACKWARD";
                if (LeaveType.ReqDocument == true && LeaveType.NumDay <= HeaderEmpLeave.NoDay)
                {
                    if (HeaderEmpLeave.Attachment == null)
                        return "REQUIRED_DOCUMENT";
                }
                HeaderEmpLeave.EmpCode = HeaderStaff.EmpCode;
                getLeaveDay(HeaderStaff.EmpCode);
                if (ListEmpLeaveD.Count <= 0)
                    return "INV_DOC";
                var MSM_ = EvaluateLeave(ListEmpLeaveD, HeaderStaff.EmpCode, HeaderEmpLeave.FromDate, HeaderEmpLeave.ToDate, HeaderEmpLeave.Units, 0);
                if (MSM_ != SYConstant.OK)
                {
                    return MSM_;
                }
                if (LeaveType != null)
                {
                    if (LeaveType.Probation == true)
                    {
                        if (staff.Probation.Value.Date > HeaderEmpLeave.FromDate.Date)
                            return "CANNOT_PROBATION";
                    }
                }
                string leaveCode = HeaderEmpLeave.LeaveCode;
                if (LeaveType != null && LeaveType.IsParent == true && !string.IsNullOrEmpty(LeaveType.Parent)) leaveCode = LeaveType.Parent;
                PRParameter payParam = DB.PRParameters.FirstOrDefault(w => w.Code == staff.PayParam);
                var MSM = ValidateEntile(LeaveType, leaveCode, payParam, staff);
                if (MSM != SYConstant.OK)
                {
                    return MSM;
                }
                SetAutoApproval(staff.EmpCode, staff.Branch, HeaderEmpLeave.ToDate.Date);
                if (ListApproval.Count() <= 0)
                    return "NO_LINE_MN";
                string Status = SYDocumentStatus.PENDING.ToString();
                int Increment = GenerateLeaveObject.GetLastIncrement();
                if (staff.IsAutoAppLeave == true)
                    Status = SYDocumentStatus.APPROVED.ToString();
                HeaderEmpLeave.Increment = Increment + 1;
                HeaderEmpLeave.Status = Status;
                HeaderEmpLeave.RequestDate = DateTime.Now;
                HeaderEmpLeave.CreateBy = User.UserName;
                HeaderEmpLeave.CreateOn = DateTime.Now;
                DB.HREmpLeaves.Add(HeaderEmpLeave);

                DB.HREmpLeaveDs.AddRange(GetLeave_D(ListEmpLeaveD, HeaderStaff.EmpCode, HeaderEmpLeave.LeaveCode, HeaderEmpLeave.Units));

                //Add approver
                foreach (var read in ListApproval)
                {
                    read.ID = 0;
                    read.LastChangedDate = DateTime.Now;
                    read.DocumentNo = Convert.ToString(Increment + 1);
                    read.Status = SYDocumentStatus.OPEN.ToString();
                    read.ApprovedBy = "";
                    read.ApprovedName = "";
                    DB.ExDocApprovals.Add(read);
                }
                Approver = ListApproval.OrderBy(w => w.ApproveLevel).FirstOrDefault();
                SYHRAnnouncement _announ = new SYHRAnnouncement();
                if (ListApproval.Count() > 0)
                {
                    _announ.Type = "LeaveRequest";
                    _announ.Subject = staff.AllName;
                    _announ.Description = @"Leave type of " + LeaveType.Description +
                        " from " + HeaderEmpLeave.FromDate.ToString("yyyy.MM.dd") + " to " + HeaderEmpLeave.ToDate.ToString("yyyy.MM.dd") + " My Reason: " + HeaderEmpLeave.Reason;
                    _announ.DocumentNo = HeaderEmpLeave.Increment.ToString();
                    _announ.DocumentDate = DateTime.Now;
                    _announ.IsRead = false;
                    _announ.UserName = ListApproval.First().Approver;
                    _announ.CreatedBy = User.UserName;
                    _announ.CreatedOn = DateTime.Now;
                    DB.SYHRAnnouncements.Add(_announ);
                }

                int row = DB.SaveChanges();
                URL += HeaderEmpLeave.TranNo;
                HR_VIEW_EmpLeave EmpLeave = new HR_VIEW_EmpLeave();
                EmpLeave = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.TranNo == HeaderEmpLeave.TranNo);

                #region Send Email
                SYWorkFlowEmailObject wfo =
                           new SYWorkFlowEmailObject("ESSLR", WorkFlowType.REQUESTER,
                                UserType.N, SYDocumentStatus.PENDING.ToString());
                wfo.SelectListItem = new SYSplitItem(Convert.ToString(Increment + 1));
                wfo.User = User;
                wfo.BS = BS;
                wfo.UrlView = SYUrl.getBaseUrl();
                wfo.ScreenId = ScreenId;
                wfo.Module = "HR";// CModule.PURCHASE.ToString();
                wfo.ListLineRef = new List<BSWorkAssign>();
                wfo.DocNo = HeaderEmpLeave.TranNo.ToString();
                wfo.Action = SYDocumentStatus.PENDING.ToString();
                wfo.ObjectDictionary = HeaderEmpLeave;
                wfo.ListObjectDictionary = new List<object>();
                wfo.ListObjectDictionary.Add(EmpLeave);
                HRStaffProfile Staff = getNextApprover(Convert.ToString(Increment + 1), "");
                wfo.ListObjectDictionary.Add(Staff);
                if (Staff != null && !string.IsNullOrEmpty(Staff.Email))
                {
                    WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Staff);
                    MessageError = wfo.getErrorMessage(result1);
                }
                #endregion
                var LeaveBalance = DB.HREmpLeaveBs.FirstOrDefault(w => w.EmpCode == HeaderEmpLeave.EmpCode
               && w.InYear == HeaderEmpLeave.ToDate.Year && w.LeaveCode == HeaderEmpLeave.LeaveCode);

                decimal? _Balance = 0;
                if (LeaveBalance != null)
                {
                    if (LeaveBalance.DayEntitle > 0)
                        _Balance = LeaveBalance.Balance - Convert.ToDecimal(HeaderEmpLeave.NoDay);
                }

                #region ---Send To Telegram---
                //string Urgent = "";
                //if (HeaderEmpLeave.Urgent == true) Urgent = "<b style=\"color: red\">Urgent</b>%0A%0A";
                var EmailTemplate = DP.TPEmailTemplates.Find("TELEGRAM");
                if (EmailTemplate != null && !string.IsNullOrEmpty(staff.TeleGroup))
                {
                    string strComp = "";
                    string text = "";
                    var staff_ = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == User.UserName);
                    var _Company = DP.SYHRCompanies.FirstOrDefault(w => w.CompanyCode == staff_.CompanyCode);
                    if (_Company != null && _Company.CompanyCode == "TELA" && payParam != null)
                    {
                        strComp = _Company.CompanyCode;
                        text = GetCconMessage(EmpLeave, ListEmpLeaveD, Staff, HeaderEmpLeave.Units, HeaderStaff.Phone1, URL, _Balance, payParam.WHOUR);
                    }
                    else
                        text = GetCconMessageEN(EmpLeave, ListEmpLeaveD, Staff, HeaderEmpLeave.Units, HeaderStaff.Phone1, URL, _Balance, payParam.WHOUR);
                    SYSendTelegramObject Tel = new SYSendTelegramObject();
                    List<object> ListObjectDictionary = new List<object>();
                    ListObjectDictionary.Add(EmpLeave);
                    ListObjectDictionary.Add(Staff);
                    Tel.Send_SMS_Telegrams(EmailTemplate, staff.TeleGroup, ListObjectDictionary, URL, strComp, text);
                }
                #endregion
                #region ---Telegram alert to Line Manager---
                var EmailTemplate1 = DP.TPEmailTemplates.Find("TELEGRAM");
                var HOD = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == staff.FirstLine);
                if (EmailTemplate1 != null && !string.IsNullOrEmpty(HOD.TeleChartID))
                {
                    SYSendTelegramObject Tel = new SYSendTelegramObject();
                    Tel.User = User;
                    Tel.BS = BS;
                    List<object> ListObjectDictionary = new List<object>();
                    ListObjectDictionary.Add(EmpLeave);
                    ListObjectDictionary.Add(Staff);
                    ListObjectDictionary.Add(Approver);
                    if (Staff.TeleChartID != null && Staff.TeleChartID != "")
                    {
                        WorkFlowResult result2 = Tel.Send_SMS_Telegram("TELEGRAM", EmailTemplate1.RequestContent, HOD.TeleChartID, ListObjectDictionary, URL);
                        MessageError = Tel.getErrorMessage(result2);
                    }
                }
                #endregion

                #region Notifican on Mobile
                var access = DB.TokenResources.FirstOrDefault(w => w.UserName == _announ.UserName);
                if (access != null)
                {
                    if (!string.IsNullOrEmpty(access.FirebaseID))
                    {
                        string Desc = staff.AllName + @" to request Leave type of " + LeaveType.Description +
                            " from " + HeaderEmpLeave.FromDate.ToString("yyyy.MM.dd") + " to " + HeaderEmpLeave.ToDate.ToString("yyyy.MM.dd");
                        Notification.Notificationf Noti = new Notification.Notificationf();
                        var clientToken = new List<string>();
                        clientToken.Add(access.FirebaseID);
                        //clientToken.Add("d7Xt0qR7JkfnnLKGf4xCw2:APA91bHfJMAlQRQlZDwDqG9h8FQfbf8lEijFo4zlzI1i17tEVhZVT7lzTAy3q7ePb7vbgok5bxJWQjdSgBM37NKkSQ_mYnsQInV7ZmRHyVOmM6xektGYp0e8AhGSulzpZZnhvuR19v32");
                        var dd = Noti.SendNotification(clientToken, "LeaveRequest", Desc);
                    }
                }
                #endregion
                if (HeaderEmpLeave.Status == SYDocumentStatus.APPROVED.ToString())
                {
                    ReGenerateLeaveToken(HeaderEmpLeave.EmpCode, HeaderEmpLeave.FromDate, HeaderEmpLeave.ToDate);
                    var ListAtt = DB.ATEmpSchedules.Where(w => w.EmpCode == HeaderEmpLeave.EmpCode
                                  && DbFunctions.TruncateTime(w.TranDate) >= HeaderEmpLeave.FromDate.Date
                                  && DbFunctions.TruncateTime(w.TranDate) <= HeaderEmpLeave.ToDate.Date).ToList();
                    foreach (var item in ListAtt)
                    {
                        item.LeaveDesc = "";
                        item.LeaveCode = HeaderEmpLeave.LeaveCode;
                        item.LeaveNo = HeaderEmpLeave.Increment;
                        DBI.ATEmpSchedules.Attach(item);
                        DBI.Entry(item).Property(w => w.LeaveDesc).IsModified = true;
                        DBI.Entry(item).Property(w => w.LeaveCode).IsModified = true;
                        DBI.Entry(item).Property(w => w.LeaveNo).IsModified = true;
                    }
                    DBI.SaveChanges();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = HeaderEmpLeave.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string AssetBusinessClosures(string EmpCode, DateTime FromDate, DateTime ToDate, string Remark, string LeaveType, string TimeShift)
        {
            string ErrorCode = "";
            try
            {
                var _LeaveType = DB.HRLeaveTypes.Find(LeaveType);
                if (_LeaveType == null)
                {
                    return "INV_DOC";
                }
                if (FromDate.Date > ToDate.Date)
                {
                    return "INVALID_DATE";
                }
                string[] Emp = EmpCode.Split(';');
                //List<HRStaffProfile> ListStaffPro = DB.HRStaffProfiles.ToList();
                List<PRParameter> _listParam = DB.PRParameters.ToList();
                string Units = "Day";
                int Increment = GetLastIncrement();
                foreach (var Code in Emp)
                {
                    ErrorCode = Code;
                    HRStaffProfile Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == Code);
                    if (Staff == null) continue;
                    var _param = _listParam.FirstOrDefault(w => w.Code == Staff.PayParam);
                    decimal WHOUR = 8;
                    if (_param != null) WHOUR = (decimal)_param.WHOUR;
                    HeaderEmpLeave = new HREmpLeave();
                    HeaderEmpLeave.EmpCode = Staff.EmpCode;
                    HeaderEmpLeave.FromDate = FromDate;
                    HeaderEmpLeave.ToDate = ToDate;
                    HeaderEmpLeave.LeaveCode = LeaveType;
                    HeaderEmpLeave.Units = Units;
                    HeaderEmpLeave.Status = SYDocumentStatus.APPROVED.ToString();
                    HeaderEmpLeave.Increment = Increment + 1;
                    HeaderEmpLeave.RequestDate = DateTime.Now;
                    HeaderEmpLeave.Reason = Remark;
                    HeaderEmpLeave.CreateBy = User.UserName;
                    HeaderEmpLeave.CreateOn = DateTime.Now;
                    int Line = 0;
                    decimal NoDay = 0;
                    for (DateTime date = FromDate; date <= ToDate; date = date.AddDays(1))
                    {
                        Line += 1;
                        HREmpLeaveD Obj = new HREmpLeaveD();
                        Obj.LeaveTranNo = Increment + 1;
                        Obj.EmpCode = Staff.EmpCode;
                        Obj.LeaveCode = LeaveType;
                        Obj.CutMonth = date.Date;
                        Obj.LeaveDate = date.Date;
                        Obj.Status = "Leave";
                        Obj.Remark = TimeShift;
                        Obj.LHour = WHOUR;
                        if (Obj.Remark == "Morning" || Obj.Remark == "Afternoon")
                        {
                            Obj.LHour = WHOUR / 2;
                        }
                        Obj.LineItem = Line;
                        NoDay += (decimal)(Obj.LHour / WHOUR);
                        DB.HREmpLeaveDs.Add(Obj);
                    }
                    HeaderEmpLeave.NoDay = (double)NoDay;
                    HeaderEmpLeave.NoPH = 0;
                    HeaderEmpLeave.NoRest = 0;
                    DB.HREmpLeaves.Add(HeaderEmpLeave);
                    Increment += 1;
                }
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ErrorCode;
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
                log.DocurmentAction = ErrorCode;
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
                log.DocurmentAction = ErrorCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string approveTheDoc(string id, string URL, string Remark)
        {
            try
            {
                DB = new HumicaDBContext();
                int TranNo = Convert.ToInt32(id);
                var objMatch = DB.HREmpLeaves.Find(TranNo);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                if (Remark == "null") Remark = null;
                if (objMatch.IsReqCancel == true)
                {
                    CancelLeave(objMatch.TranNo, Remark, false);
                }
                else
                {
                    var _LeaveType = DB.HRLeaveTypes.Find(objMatch.LeaveCode);
                    string open = SYDocumentStatus.OPEN.ToString();
                    string DocNo = objMatch.Increment.ToString();
                    var listApproval = DB.ExDocApprovals
                        .Where(w => w.DocumentType == "LR" && w.DocumentNo == DocNo)
                        .OrderBy(w => w.ApproveLevel)
                        .ToList();
                    var listUser = DB.HRStaffProfiles
                        .Where(w => w.EmpCode == User.UserName)
                        .ToList();
                    var b = false;
                    if (listApproval.Count == 0)
                    {
                        return "RESTRICT_ACCESS";
                    }
                    var APPROVED = SYDocumentStatus.APPROVED.ToString();
                    var REJECTED = SYDocumentStatus.REJECTED.ToString();
                    var CANCELLED = SYDocumentStatus.CANCELLED.ToString();
                    var approver = "";
                    foreach (var read in listApproval)
                    {
                        if (listApproval.Where(w => w.ApproveLevel == read.ApproveLevel
                            && (w.Status == APPROVED || w.Status == REJECTED || w.Status == CANCELLED)).Any())
                            continue;
                        var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
                        if (checklist.Count > 0)
                        {
                            if (read.Status == SYDocumentStatus.APPROVED.ToString())
                            {
                                return "USER_ALREADY_APP";
                            }

                            if (read.ApproveLevel > listApproval.Where(w => w.Status == open).Min(w => w.ApproveLevel))
                            {
                                return "REQUIRED_PRE_LEVEL";
                            }
                            var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                            if (objStaff != null)
                            {
                                foreach (var item in listApproval.Where(w => w.ApproveLevel == read.ApproveLevel))
                                {
                                    item.ApprovedBy = objStaff.EmpCode;
                                    item.ApprovedName = objStaff.AllName;
                                    item.LastChangedDate = DateTime.Now.Date;
                                    item.ApprovedDate = DateTime.Now;
                                    item.Status = SYDocumentStatus.APPROVED.ToString();
                                    DB.ExDocApprovals.Attach(item);
                                    DB.Entry(item).State = System.Data.Entity.EntityState.Modified;
                                    approver = objStaff.EmpCode;
                                }
                                b = true;
                                break;
                            }
                        }
                    }
                    if (listApproval.Count > 0)
                    {
                        if (b == false)
                        {
                            return "USER_NOT_APPROVOR";
                        }
                    }
                    var status = SYDocumentStatus.APPROVED.ToString();
                    //var open = SYDocumentStatus.OPEN.ToString();
                    // objMatch.IsApproved = true;
                    if (listApproval.Where(w => w.Status == open).ToList().Count > 0)
                    {
                        status = SYDocumentStatus.PENDING.ToString();
                    }

                    objMatch.Status = status;
                    DB.HREmpLeaves.Attach(objMatch);
                    DB.Entry(objMatch).Property(w => w.Status).IsModified = true;

                    //Update Leave in Att
                    var ListAtt = DB.ATEmpSchedules.Where(w => w.EmpCode == objMatch.EmpCode
                    && DbFunctions.TruncateTime(w.TranDate) >= objMatch.FromDate.Date
                    && DbFunctions.TruncateTime(w.TranDate) <= objMatch.ToDate.Date).ToList();
                    foreach (var item in ListAtt)
                    {
                        item.LeaveDesc = "";
                        item.LeaveCode = objMatch.LeaveCode;
                        item.LeaveNo = objMatch.Increment;
                        DB.ATEmpSchedules.Attach(item);
                        DB.Entry(item).Property(w => w.LeaveDesc).IsModified = true;
                        DB.Entry(item).Property(w => w.LeaveCode).IsModified = true;
                        DB.Entry(item).Property(w => w.LeaveNo).IsModified = true;
                    }

                    SYHRAnnouncement _announ = new SYHRAnnouncement();
                    var _Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                    var _Lstapp = listApproval.Where(w => w.Status == SYDocumentStatus.OPEN.ToString()).ToList();
                    _announ.Type = "LeaveRequest";
                    if (_Lstapp.Count() > 0)
                    {
                        var min = _Lstapp.Min(w => w.ApproveLevel);
                        _announ.Subject = _Staff.AllName;
                        _announ.UserName = _Lstapp.FirstOrDefault(w => w.ApproveLevel == min).Approver;
                        _announ.Description = @"Leave type of " + _LeaveType.Description +
                            " from " + objMatch.FromDate.ToString("yyyy.MM.dd") + " to " + objMatch.ToDate.ToString("yyyy.MM.dd") + " My Reason: " + objMatch.Reason;
                    }
                    if (status == SYDocumentStatus.APPROVED.ToString())
                    {
                        _announ.Type = "LeaveApproved";
                        _announ.Subject = "Approved";
                        _announ.UserName = objMatch.EmpCode;
                        _announ.Description = "Leave type of " + _LeaveType.Description;
                    }
                    if (!string.IsNullOrEmpty(_announ.Description))
                    {
                        _announ.DocumentNo = objMatch.Increment.ToString();
                        _announ.DocumentDate = DateTime.Now;
                        _announ.IsRead = false;
                        _announ.CreatedBy = User.UserName;
                        _announ.CreatedOn = DateTime.Now;
                        DB.SYHRAnnouncements.Add(_announ);
                    }

                    string Approval = SYDocumentStatus.APPROVED.ToString();
                    List<HRClaimLeave> _listClaim = DB.HRClaimLeaves.Where(w => w.EmpCode == objMatch.EmpCode
                    && (w.IsExpired.Value != true || w.IsUsed.Value != true) && w.Status == Approval).ToList();
                    DateTime DateNow = DateTime.Now;
                    bool Isused = false;
                    foreach (var claim in _listClaim.ToList().OrderBy(w => w.WorkingDate))
                    {
                        if (Isused == true) continue;
                        if (claim.Expired.Value.Date < DateNow.Date)
                        {
                            claim.IsExpired = true;
                            DB.HRClaimLeaves.Attach(claim);
                            DB.Entry(claim).Property(x => x.IsExpired).IsModified = true;
                        }
                        else
                        {
                            Isused = true;
                            claim.IsUsed = true;
                            claim.DocumentRef = objMatch.Increment.ToString();
                            DB.HRClaimLeaves.Attach(claim);
                            DB.Entry(claim).Property(x => x.IsUsed).IsModified = true;
                            DB.Entry(claim).Property(x => x.DocumentRef).IsModified = true;
                        }
                    }

                    int row = DB.SaveChanges();
                    DBV = new HumicaDBViewContext();
                    if (objMatch.Status == SYDocumentStatus.APPROVED.ToString())
                    {
                        ReGenerateLeaveToken(objMatch.EmpCode, objMatch.FromDate, objMatch.ToDate);
                        #region Send Email
                        HR_VIEW_EmpLeave EmpLeave = new HR_VIEW_EmpLeave();
                        EmpLeave = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.TranNo == objMatch.TranNo);
                        SYWorkFlowEmailObject wfo =
                                   new SYWorkFlowEmailObject("ESSLA", WorkFlowType.REQUESTER,
                                        UserType.N, SYDocumentStatus.PENDING.ToString());
                        wfo.SelectListItem = new SYSplitItem(Convert.ToString(DocNo));
                        wfo.User = User;
                        wfo.BS = BS;
                        wfo.UrlView = URL;
                        wfo.ScreenId = ScreenId;
                        wfo.Module = "HR";// CModule.PURCHASE.ToString();
                        wfo.DocNo = objMatch.TranNo.ToString();
                        wfo.ListLineRef = new List<BSWorkAssign>();
                        wfo.Action = SYDocumentStatus.PENDING.ToString();
                        wfo.ObjectDictionary = HeaderEmpLeave;
                        wfo.ListObjectDictionary = new List<object>();
                        wfo.ListObjectDictionary.Add(EmpLeave);
                        HRStaffProfile Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpLeave.EmpCode);
                        if (!string.IsNullOrEmpty(Staff.Email))
                        {
                            wfo.ListObjectDictionary.Add(Staff);
                            WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Staff);
                            MessageError = wfo.getErrorMessage(result1);
                        }
                        #endregion

                        #region *****Send to Telegram
                        if (!string.IsNullOrEmpty(Staff.TeleGroup))
                        {
                            Humica.Core.SY.SYSendTelegramObject wfo1 =
                              new Humica.Core.SY.SYSendTelegramObject("ESSLA", WorkFlowType.APPROVER, objMatch.Status);
                            wfo1.User = User;
                            wfo1.ListObjectDictionary = new List<object>();
                            wfo1.ListObjectDictionary.Add(EmpLeave);
                            wfo1.ListObjectDictionary.Add(Staff);
                            #region Tembody
                            string str = "";
                            str = BuildMessage(approver, objMatch, Remark);
                            #endregion
                            wfo1.Send_SMS_TelegramLeaveApproval(Staff.TeleGroup, "", str);
                        }
                        #endregion
                        //Alert to HR

                        var EmailTemplateCC = DP.TPEmailTemplates.Find("ESSLEAVE_TELECC_HR");
                        var Sett = DB.SYHRSettings.FirstOrDefault();
                        if (EmailTemplateCC != null && !string.IsNullOrEmpty(Sett.TelegLeave))
                        {
                            SYSendTelegramObject Tel = new SYSendTelegramObject();
                            Tel.User = User;
                            Tel.BS = BS;
                            List<object> ListObjectDictionary = new List<object>();
                            ListObjectDictionary.Add(EmpLeave);
                            ListObjectDictionary.Add(Staff);
                            WorkFlowResult result2 = Tel.Send_SMS_Telegram("ESSLEAVE_TELECC_HR", EmailTemplateCC.RequestContent, Sett.TelegLeave, ListObjectDictionary, URL);
                            MessageError = Tel.getErrorMessage(result2);
                        }
                        ////Alert to HOD
                        //var EmailTemplate_HOD = DP.TPEmailTemplates.Find("ESSLEAVE_TELECC_HOD");
                        //if (EmailTemplate_HOD != null)
                        //{
                        //    var HOD = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == Staff.HODCode);
                        //    if (HOD != null && !string.IsNullOrEmpty(HOD.TeleChartID))
                        //    {
                        //        SYSendTelegramObject Tel = new SYSendTelegramObject();
                        //        Tel.User = User;
                        //        Tel.BS = BS;
                        //        List<object> ListObjectDictionary = new List<object>();
                        //        ListObjectDictionary.Add(EmpLeave);
                        //        ListObjectDictionary.Add(Staff);
                        //        ListObjectDictionary.Add(HOD);
                        //        WorkFlowResult result2 = Tel.Send_SMS_Telegram(EmailTemplate_HOD.EMTemplateObject, EmailTemplate_HOD.RequestContent, HOD.TeleChartID, ListObjectDictionary, URL);
                        //        MessageError = Tel.getErrorMessage(result2);
                        //    }
                        //}
                        ////Alert to Requester 
                        //var Template_Req = DP.TPEmailTemplates.Find("TAPPLEAVE");
                        //if (Template_Req != null)
                        //{
                        //    if (!string.IsNullOrEmpty(Staff.TeleChartID))
                        //    {
                        //        SYSendTelegramObject Tel = new SYSendTelegramObject();
                        //        Tel.User = User;
                        //        Tel.BS = BS;
                        //        List<object> ListObjectDictionary = new List<object>();
                        //        ListObjectDictionary.Add(EmpLeave);
                        //        ListObjectDictionary.Add(Staff);
                        //        WorkFlowResult result2 = Tel.Send_SMS_Telegram(Template_Req.EMTemplateObject, Template_Req.RequestContent, Staff.TeleChartID, ListObjectDictionary, URL);
                        //        MessageError = Tel.getErrorMessage(result2);
                        //    }
                        //}

                        #region Notifican on Mobile
                        var access = DB.TokenResources.FirstOrDefault(w => w.UserName == _Staff.EmpCode);
                        if (access != null)
                        {
                            if (!string.IsNullOrEmpty(access.FirebaseID))
                            {
                                string Desc = _announ.Description;
                                Notification.Notificationf Noti = new Notification.Notificationf();
                                var clientToken = new List<string>();
                                clientToken.Add(access.FirebaseID);
                                //clientToken.Add("d7Xt0qR7JkfnnLKGf4xCw2:APA91bHfJMAlQRQlZDwDqG9h8FQfbf8lEijFo4zlzI1i17tEVhZVT7lzTAy3q7ePb7vbgok5bxJWQjdSgBM37NKkSQ_mYnsQInV7ZmRHyVOmM6xektGYp0e8AhGSulzpZZnhvuR19v32");
                                var dd = Noti.SendNotification(clientToken, "LeaveApproved", Desc);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region Send Email
                        HR_VIEW_EmpLeave EmpLeave = new HR_VIEW_EmpLeave();
                        EmpLeave = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.TranNo == objMatch.TranNo);
                        SYWorkFlowEmailObject wfo =
                                   new SYWorkFlowEmailObject("ESSLR", WorkFlowType.REQUESTER,
                                        UserType.N, SYDocumentStatus.PENDING.ToString());
                        wfo.SelectListItem = new SYSplitItem(Convert.ToString(DocNo));
                        wfo.User = User;
                        wfo.BS = BS;
                        wfo.UrlView = URL;
                        wfo.ScreenId = ScreenId;
                        wfo.Module = "HR";// CModule.PURCHASE.ToString();
                        wfo.ListLineRef = new List<BSWorkAssign>();
                        wfo.Action = SYDocumentStatus.PENDING.ToString();
                        wfo.ObjectDictionary = HeaderEmpLeave;
                        wfo.ListObjectDictionary = new List<object>();
                        wfo.ListObjectDictionary.Add(EmpLeave);
                        HRStaffProfile Staff = getNextApprover(DocNo, "");
                        if (!string.IsNullOrEmpty(Staff.Email))
                        {
                            wfo.ListObjectDictionary.Add(Staff);
                            WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Staff);
                            MessageError = wfo.getErrorMessage(result1);
                        }
                        #endregion

                        var access = DB.TokenResources.FirstOrDefault(w => w.UserName == Staff.EmpCode);
                        if (access != null)
                        {
                            if (!string.IsNullOrEmpty(access.FirebaseID))
                            {
                                string Desc = _announ.Description;
                                Notification.Notificationf Noti = new Notification.Notificationf();
                                var clientToken = new List<string>();
                                clientToken.Add(access.FirebaseID);
                                var dd = Noti.SendNotification(clientToken, "LeaveRequest", Desc);
                            }
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
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string BuildMessage(string approver, HREmpLeave objMatch, string Remark)
        {
            var EMP = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == approver);
            var ObjectStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
            var Handover = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objMatch.TaskHand_Over);
            var CurrentAL = DB.HREmpLeaveBs.FirstOrDefault(w => w.LeaveCode == objMatch.LeaveCode && w.EmpCode == objMatch.EmpCode && w.InYear == objMatch.FromDate.Year);
            var ListLeave = DB.HREmpLeaveDs.Where(w => w.LeaveTranNo == objMatch.Increment && w.EmpCode == objMatch.EmpCode).ToList();

            string title = EMP?.Title == "Ms" ? "Miss" : EMP?.Title;
            string message = "";

            if (DB.SYHRCompanies.Any(w => w.CompanyCode == ObjectStaff.CompanyCode && w.CompanyCode == "TELA"))
            {
                message = FormatMessageKH(EMP, ObjectStaff, CurrentAL, ListLeave, Handover, objMatch);
            }
            else
            {
                message = FormatMessage(EMP, ObjectStaff, CurrentAL, ListLeave, Handover, objMatch, Remark);
            }

            return message;
        }

        private string FormatMessageKH(HRStaffProfile emp, HRStaffProfile objectStaff, HREmpLeaveB currentAL, List<HREmpLeaveD> listLeave, HRStaffProfile handover, HREmpLeave objMatch)
        {
            var leaveType = DB.HRLeaveTypes.FirstOrDefault(w => w.Code == objMatch.LeaveCode);
            string str = "<b>កម្មវត្ថុ៖</b>: Leave Approval";
            str += $"<b>%0Aអនុម័តដោយ៖</b> {emp.Title}. {emp.AllName} ({emp.EmpCode})";
            str += $"<b>%0Aទៅកាន់៖</b> {objectStaff.Title}. {objectStaff.AllName} ({objectStaff.EmpCode})";
            str += $"<b>%0Aកាលបរិច្ឆេទ៖</b> {objMatch.CreateOn.Value.Date:dd-MMM-yyyy}";
            str += $"<b>%0Aប្រភេទសុំច្បាប់៖</b> {leaveType.Description}";
            str += $"<b>%0Aសមតុល្យ៖</b> {currentAL.CurrentEntitle}";
            str += $"<b>%0Aកាលបរិច្ឆេទឈប់សម្រាក៖</b> {objMatch.FromDate:dd-MMM-yyyy} - {objMatch.ToDate.Date:dd-MMM-yyyy}";

            foreach (var item in listLeave)
            {
                if (item.LeaveCode != "ML")
                {
                    str += FormatLeaveItemKH(item);
                }
            }

            str += $"<b>%0Aចំនួនថ្ងៃបានឈប់៖</b> {objMatch.NoDay}";
            str += $"<b>%0Aមូលហេតុ៖</b> {objMatch.Reason}";

            if (handover != null)
            {
                str += $"<b>%0Aផ្ទេរសិទ្ធិជួន៖</b> {handover.EmpCode}: {handover.AllName}";
            }
            return str;
        }

        private string FormatMessage(HRStaffProfile emp, HRStaffProfile objectStaff, HREmpLeaveB currentAL, List<HREmpLeaveD> listLeave, HRStaffProfile handover, HREmpLeave objMatch, string Remark)
        {
            var leaveType = DB.HRLeaveTypes.FirstOrDefault(w => w.Code == objMatch.LeaveCode);
            string str = "<b>Subject</b>: Leave Approval";
            str += $"<b>%0AApproved By:</b> {emp.Title}. {emp.AllName} ({emp.EmpCode})";
            str += $"<b>%0ARequested By:</b> {objectStaff.Title}. {objectStaff.AllName} ({objectStaff.EmpCode})";
            str += $"<b>%0ARequested Date:</b> {objMatch.CreateOn.Value.Date:dd-MMM-yyyy}";
            str += $"<b>%0ALeave Type:</b> {leaveType.Description}";
            str += $"<b>%0ABalance:</b> {currentAL.CurrentEntitle}";
            str += $"<b>%0ALeave Date:</b> {objMatch.FromDate:dd-MMM-yyyy} - {objMatch.ToDate.Date:dd-MMM-yyyy}";

            foreach (var item in listLeave)
            {
                //if (item.LeaveCode != "ML")
                //{
                str += FormatLeaveItem(item);
                //}
            }

            str += $"<b>%0ANo. of Day (s) Taken:</b> {objMatch.NoDay}";
            str += $"<b>%0AReason:</b> {objMatch.Reason}";
            if (!string.IsNullOrEmpty(Remark))
            {
                str += $"<b>%0AComment:</b> {Remark}";
            }
            if (handover != null)
            {
                str += $"<b>%0ATask Hand Over:</b> {handover.EmpCode}: {handover.AllName}";
            }

            return str;
        }
        private string FormatLeaveItem(HREmpLeaveD item)
        {
            if (item.Remark == "Hours") item.Remark = item.LHour + " Hour";
            else if (item.Remark == "FullDay") item.Remark = "Full Day";
            else if (item.Remark == "Morning") item.Remark = "First Half";
            else item.Remark = "Second Half";
            return $"%0A{item.LeaveDate.Date:dd-MMM-yyyy}: {item.Remark}";
        }
        private string FormatLeaveItemKH(HREmpLeaveD item)
        {
            if (item.Remark == "Hours") item.Remark = item.LHour + " ម៉ោង";
            else if (item.Remark == "FullDay") item.Remark = "ពេញមួយថ្ងៃ";
            else if (item.Remark == "Morning") item.Remark = "ពេលព្រឹក";
            else item.Remark = "ពេលរសៀល";

            return $"%0A{item.LeaveDate.Date:dd-MMM-yyyy}: {item.Remark}";
        }
        public string ApproveHRLeave(string ApprovalID, string URL)
        {
            HumicaDBContext DBI = new HumicaDBContext();
            try
            {
                DBI.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    string[] c = ApprovalID.Split(';');
                    foreach (var r in c)
                    {
                        if (r == "") continue;
                        //var ListStaff = DBI.HRStaffProfiles.ToList();
                        string approved = SYDocumentStatus.APPROVED.ToString();
                        int TranNo = Convert.ToInt32(r);
                        var objmatch = DBI.HREmpLeaves.Find(TranNo);
                        //var ObjLeaveB = DBI.HREmpLeaveBs;
                        if (objmatch == null)
                            return "INV_EN";
                        objmatch.Status = approved;
                        objmatch.ChangedBy = User.UserName;
                        objmatch.ChangedOn = DateTime.Now;
                        DBI.HREmpLeaves.Attach(objmatch);
                        DBI.Entry(objmatch).Property(w => w.RejectDate).IsModified = true;
                        DBI.Entry(objmatch).Property(w => w.Status).IsModified = true;
                        DBI.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                        DBI.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;
                        //Update Leave in Att
                        var ListAtt = DB.ATEmpSchedules.Where(w => w.EmpCode == objmatch.EmpCode
                                                        && DbFunctions.TruncateTime(w.TranDate) >= objmatch.FromDate.Date
                                                        && DbFunctions.TruncateTime(w.TranDate) <= objmatch.ToDate.Date).ToList();
                        foreach (var item in ListAtt)
                        {
                            item.LeaveDesc = "";
                            item.LeaveCode = objmatch.LeaveCode;
                            item.LeaveNo = objmatch.Increment;
                            DBI.ATEmpSchedules.Attach(item);
                            DBI.Entry(item).Property(w => w.LeaveDesc).IsModified = true;
                            DBI.Entry(item).Property(w => w.LeaveCode).IsModified = true;
                            DBI.Entry(item).Property(w => w.LeaveNo).IsModified = true;
                        }
                        string Approval = SYDocumentStatus.APPROVED.ToString();
                        List<HRClaimLeave> _listClaim = DBI.HRClaimLeaves.Where(w => w.EmpCode == objmatch.EmpCode
                && (w.IsExpired.Value != true || w.IsUsed.Value != true) && w.Status == Approval).ToList();
                        DateTime DateNow = DateTime.Now;
                        bool Isused = false;
                        foreach (var claim in _listClaim.ToList().OrderBy(w => w.WorkingDate))
                        {
                            if (Isused == true) continue;
                            if (claim.Expired.Value.Date < DateNow.Date)
                            {
                                claim.IsExpired = true;
                                DBI.HRClaimLeaves.Attach(claim);
                                DBI.Entry(claim).Property(x => x.IsExpired).IsModified = true;
                            }
                            else
                            {
                                Isused = true;
                                claim.IsUsed = true;
                                claim.DocumentRef = HeaderEmpLeave.Increment.ToString();
                                DBI.HRClaimLeaves.Attach(claim);
                                DBI.Entry(claim).Property(x => x.IsUsed).IsModified = true;
                                DBI.Entry(claim).Property(x => x.DocumentRef).IsModified = true;
                            }
                        }

                        DBI.SaveChanges();
                        ReGenerateLeaveToken(objmatch.EmpCode, objmatch.FromDate, objmatch.ToDate);
                        //string DocNo = objmatch.Increment.ToString();
                        //DBV = new HumicaDBViewContext();
                        //HR_VIEW_EmpLeave EmpLeave = new HR_VIEW_EmpLeave();
                        //EmpLeave = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.TranNo == objmatch.TranNo);
                        //HRStaffProfile Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpLeave.EmpCode);
                        //SYWorkFlowEmailObject wfo =
                        //           new SYWorkFlowEmailObject("ESSLA", WorkFlowType.REQUESTER,
                        //                UserType.N, SYDocumentStatus.PENDING.ToString());
                        //#region Send Email
                        //if (!string.IsNullOrEmpty(Staff.Email))
                        //{
                        //    wfo.SelectListItem = new SYSplitItem(Convert.ToString(DocNo));
                        //    wfo.User = User;
                        //    wfo.BS = BS;
                        //    wfo.UrlView = URL;
                        //    wfo.ScreenId = ScreenId;
                        //    wfo.Module = "HR";// CModule.PURCHASE.ToString();
                        //    wfo.ListLineRef = new List<BSWorkAssign>();
                        //    wfo.Action = SYDocumentStatus.PENDING.ToString();
                        //    wfo.ObjectDictionary = HeaderEmpLeave;
                        //    wfo.ListObjectDictionary = new List<object>();
                        //    wfo.ListObjectDictionary.Add(EmpLeave);

                        //    wfo.ListObjectDictionary.Add(Staff);
                        //    WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Staff);
                        //    MessageError = wfo.getErrorMessage(result1);
                        //}
                        //#endregion

                        //#region *****Send to Telegram
                        //if (!string.IsNullOrEmpty(Staff.TeleGroup))
                        //{
                        //    Humica.Core.SY.SYSendTelegramObject wfo1 =
                        //   new Humica.Core.SY.SYSendTelegramObject("ESSLA", WorkFlowType.APPROVER, objmatch.Status);
                        //    wfo1.User = User;
                        //    wfo1.ListObjectDictionary = new List<object>();
                        //    wfo1.ListObjectDictionary.Add(EmpLeave);
                        //    wfo1.ListObjectDictionary.Add(Staff);
                        //    wfo1.Send_SMS_Telegram(Staff.TeleGroup, "");
                        //}
                        //#endregion
                    }
                    return SYConstant.OK;
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = ApprovalID;
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
                    log.DocurmentAction = ApprovalID;
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
                    log.DocurmentAction = ApprovalID;
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
        public string RejectLeave(string ApprovalID, string Remark, bool admin)
        {
            try
            {
                HumicaDBContext DBI = new HumicaDBContext();
                if (Remark == "null") Remark = null;
                if (string.IsNullOrEmpty(Remark) && !admin) return "INVALID_COMMENT";
                string[] c = ApprovalID.Split(';');
                foreach (var r in c)
                {
                    if (r == "") continue;
                    int TranNo = Convert.ToInt32(r);
                    string Reject = SYDocumentStatus.REJECTED.ToString();
                    int ID = 0;
                    HREmpLeave objmatch = DB.HREmpLeaves.First(w => w.TranNo == TranNo);
                    if (objmatch == null)
                        return "INV_EN";
                    var _obj = DB.ExDocApprovals.Where(x => x.DocumentNo == objmatch.Increment.ToString());
                    if (admin)
                    {
                        foreach (var read in _obj)
                        {
                            read.Status = Reject;
                            read.LastChangedDate = DateTime.Now;
                            DB.Entry(read).Property(w => w.Status).IsModified = true;
                            DB.Entry(read).Property(w => w.LastChangedDate).IsModified = true;
                        }
                    }
                    else
                    {
                        var Exdoc = _obj.FirstOrDefault(w => w.Approver == User.UserName);
                        if (Exdoc != null)
                        {
                            ID = Exdoc.ID;
                            Exdoc.Comment = Remark;
                            Exdoc.Status = Reject;
                            Exdoc.LastChangedDate = DateTime.Now;
                            DB.Entry(Exdoc).Property(w => w.Comment).IsModified = true;
                            DB.Entry(Exdoc).Property(w => w.Status).IsModified = true;
                            DB.Entry(Exdoc).Property(w => w.LastChangedDate).IsModified = true;
                        }
                        else return "RESTRICT_ACCESS";
                    }
                    objmatch.RejectDate = DateTime.Now;
                    objmatch.Status = Reject;
                    objmatch.ChangedBy = User.UserName;
                    objmatch.ChangedOn = DateTime.Now;
                    DB.HREmpLeaves.Attach(objmatch);
                    DB.Entry(objmatch).Property(w => w.RejectDate).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;


                    var Staff = DB.HRStaffProfiles.FirstOrDefault(x => x.EmpCode == objmatch.EmpCode);

                    DB.SaveChanges();
                    if (!admin)
                    {

                        var _Company = DP.SYHRCompanies.FirstOrDefault(w => w.CompanyCode == Staff.CompanyCode);
                        if (_Company != null && _Company.CompanyCode == "TELA")
                        {
                            #region *****Send to Telegram
                            if (!string.IsNullOrEmpty(Staff.TeleGroup))
                            {
                                Humica.Core.SY.SYSendTelegramObject wfo =
                                new Humica.Core.SY.SYSendTelegramObject("ESSLA", WorkFlowType.REJECTOR, objmatch.Status);
                                wfo.User = User;
                                wfo.ListObjectDictionary = new List<object>();
                                wfo.ListObjectDictionary.Add(objmatch);
                                #region Tembody
                                var Rejector = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == User.UserName);
                                var ObjectStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objmatch.EmpCode);
                                var Handover = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objmatch.TaskHand_Over);
                                var leaveTypeKH = DB.HRLeaveTypes.FirstOrDefault(w => w.Code == objmatch.LeaveCode);

                                var title = "";
                                if (Rejector != null)
                                {
                                    title = Rejector.Title;
                                }
                                if (title == "Ms")
                                {
                                    title = "Miss";
                                }

                                string str = "កម្មវត្ថុ៖ Leave Reject";

                                str += "<b>%0Aបដិសេដទៅកាន់៖</b> " + ObjectStaff.Title + (". ") + ObjectStaff.AllName + "(" + ObjectStaff.EmpCode + ")";
                                str += "<b>%0Aដោយ៖</b> " + Rejector.Title + (". ") + Rejector.AllName + "(" + Rejector.EmpCode + ")";
                                str += "<b>%0Aកាលបរិច្ឆេទ៖</b> " + objmatch.ChangedOn.Value.Date.ToString("dd-MMM-yyyy");
                                str += "<b>%0Aប្រភេទសុំច្បាប់៖</b> " + leaveTypeKH.Description;
                                str += "<b>%0Aកាលបរិច្ឆេទឈប់សម្រាក៖</b> " + objmatch.FromDate.ToString("dd-MMM-yyyy") + " - " + objmatch.ToDate.Date.ToString("dd-MMM-yyyy");

                                #endregion
                                wfo.Send_SMS_TelegramLeaveApproval(Staff.TeleGroup, "", str);
                            }
                            #endregion
                        }
                        else
                        {
                            var ViewLeave = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.EmpCode == objmatch.EmpCode && w.TranNo == objmatch.TranNo);
                            #region *****Send to Requester 
                            var Approver = DB.ExDocApprovals.FirstOrDefault(w => w.ID == ID);
                            if (Staff != null && Staff.TeleChartID != null)
                            {
                                var EmailTemplate = DP.TPEmailTemplates.Find("ESSL_REJ");
                                if (EmailTemplate != null)
                                {
                                    SYSendTelegramObject Tel = new SYSendTelegramObject();
                                    Tel.User = User;
                                    Tel.BS = BS;
                                    List<object> ListObjectDictionary = new List<object>();
                                    ListObjectDictionary.Add(objmatch);
                                    ListObjectDictionary.Add(ViewLeave);
                                    ListObjectDictionary.Add(Staff);
                                    ListObjectDictionary.Add(Approver);
                                    var URL = "";
                                    WorkFlowResult result1 = Tel.Send_SMS_Telegram(EmailTemplate.EMTemplateObject, EmailTemplate.RequestContent, Staff.TeleChartID, ListObjectDictionary, URL);
                                    MessageError = Tel.getErrorMessage(result1);
                                }
                            }
                            #endregion
                            #region *****Send to Telegram to Approver
                            var listApproval_ = DB.ExDocApprovals.Where(w => w.DocumentType == "LR"
                                            && w.DocumentNo == objmatch.Increment.ToString() && w.Status == SYDocumentStatus.APPROVED.ToString()).OrderBy(w => w.ApproveLevel).ToList();
                            foreach (var list in listApproval_)
                            {
                                var HOD = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == list.Approver);
                                if (HOD != null && !string.IsNullOrEmpty(HOD.TeleChartID))
                                {
                                    var EmailTemplate = DP.TPEmailTemplates.Find("ESSL_REJ");
                                    if (EmailTemplate != null)
                                    {
                                        SYSendTelegramObject Tel = new SYSendTelegramObject();
                                        Tel.User = User;
                                        Tel.BS = BS;
                                        List<object> ListObjectDictionary = new List<object>();
                                        ListObjectDictionary.Add(objmatch);
                                        ListObjectDictionary.Add(ViewLeave);
                                        ListObjectDictionary.Add(Staff);
                                        ListObjectDictionary.Add(Approver);
                                        var URL = "";
                                        WorkFlowResult result1 = Tel.Send_SMS_Telegram(EmailTemplate.EMTemplateObject, EmailTemplate.RejectContent, HOD.TeleChartID, ListObjectDictionary, URL);
                                        MessageError = Tel.getErrorMessage(result1);
                                    }
                                }
                            }
                            #endregion
                            #region *****Send to Telegram to CreatedBy
                            if (objmatch.CreateBy != objmatch.EmpCode)
                            {
                                var CreatedBy = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objmatch.CreateBy);
                                if (CreatedBy != null && !string.IsNullOrEmpty(CreatedBy.TeleChartID))
                                {
                                    var EmailTemplate = DP.TPEmailTemplates.Find("ESSL_REJ");
                                    if (EmailTemplate != null)
                                    {
                                        SYSendTelegramObject Tel = new SYSendTelegramObject();
                                        Tel.User = User;
                                        Tel.BS = BS;
                                        List<object> ListObjectDictionary = new List<object>();
                                        ListObjectDictionary.Add(objmatch);
                                        ListObjectDictionary.Add(ViewLeave);
                                        ListObjectDictionary.Add(Staff);
                                        ListObjectDictionary.Add(Approver);
                                        var URL = "";
                                        WorkFlowResult result1 = Tel.Send_SMS_Telegram(EmailTemplate.EMTemplateObject, EmailTemplate.RejectContent, CreatedBy.TeleChartID, ListObjectDictionary, URL);
                                        MessageError = Tel.getErrorMessage(result1);
                                    }
                                }
                            }
                            #endregion
                            #region Notifican on Mobile

                            var access = DB.TokenResources.FirstOrDefault(w => w.UserName == objmatch.EmpCode);
                            if (access != null)
                            {
                                if (!string.IsNullOrEmpty(access.FirebaseID))
                                {
                                    var _leaveType = DB.HRLeaveTypes.FirstOrDefault(w => w.Code == objmatch.LeaveCode);
                                    string Desc = "Leave type of " + _leaveType.Description + " is rejected";
                                    Notification.Notificationf Noti = new Notification.Notificationf();
                                    var clientToken = new List<string>();
                                    clientToken.Add(access.FirebaseID);
                                    //clientToken.Add("d7Xt0qR7JkfnnLKGf4xCw2:APA91bHfJMAlQRQlZDwDqG9h8FQfbf8lEijFo4zlzI1i17tEVhZVT7lzTAy3q7ePb7vbgok5bxJWQjdSgBM37NKkSQ_mYnsQInV7ZmRHyVOmM6xektGYp0e8AhGSulzpZZnhvuR19v32");
                                    var dd = Noti.SendNotification(clientToken, "LeaveReject", Desc);
                                }
                            }
                            #endregion
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
                log.DocurmentAction = ApprovalID;
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
                log.DocurmentAction = ApprovalID;
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
                log.DocurmentAction = ApprovalID;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CancelLeave(long TranNo, string Remark, bool admin)
        {
            try
            {
                string Reject = SYDocumentStatus.CANCELLED.ToString();
                HREmpLeave objmatch = DB.HREmpLeaves.FirstOrDefault(w => w.TranNo == TranNo);
                if (objmatch == null)
                    return "INV_EN";
                if (!admin)
                {
                    var _obj = DB.ExDocApprovals.Where(x => x.DocumentNo == objmatch.Increment.ToString());
                    var Exdoc = _obj.FirstOrDefault(w => w.Approver == User.UserName);
                    if (Exdoc != null)
                    {
                        Exdoc.Comment = Remark;
                        Exdoc.Status = Reject;
                        Exdoc.LastChangedDate = DateTime.Now;
                        DB.Entry(Exdoc).Property(w => w.Comment).IsModified = true;
                        DB.Entry(Exdoc).Property(w => w.Status).IsModified = true;
                        DB.Entry(Exdoc).Property(w => w.LastChangedDate).IsModified = true;
                    }
                    else return "RESTRICT_ACCESS";
                }
                objmatch.RejectDate = DateTime.Now;
                objmatch.Status = Reject;
                objmatch.ChangedBy = User.UserName;
                objmatch.ChangedOn = DateTime.Now;
                DB.HREmpLeaves.Attach(objmatch);
                DB.Entry(objmatch).Property(w => w.RejectDate).IsModified = true;
                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;

                DB.SaveChanges();

                #region Notifican on Mobile
                var access = DB.TokenResources.FirstOrDefault(w => w.UserName == objmatch.EmpCode);
                if (access != null)
                {
                    var LeaveType = DB.HRLeaveTypes.FirstOrDefault(w => w.Code == objmatch.LeaveCode);
                    if (!string.IsNullOrEmpty(access.FirebaseID) && LeaveType != null)
                    {
                        string Desc = string.Format(@"Your requested Cancel Leave has been approved for leave type of {0} 
                                    from {1:yyyy.MM.dd} to {2:yyyy.MM.dd}.", LeaveType.Description, HeaderEmpLeave.FromDate,
                                        HeaderEmpLeave.ToDate);
                        Notification.Notificationf Noti = new Notification.Notificationf();
                        var clientToken = new List<string>();
                        clientToken.Add(access.FirebaseID);
                        var dd = Noti.SendNotification(clientToken, "RequestCancel", Desc);
                    }
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
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string RequestCancel(string ApprovalID, string URL, string Remark)
        {
            try
            {
                HumicaDBContext DBI = new HumicaDBContext();
                HeaderEmpLeave = new HREmpLeave();
                if (Remark == "null") Remark = null;
                if (string.IsNullOrEmpty(Remark)) return "INVALIED_SEASON";
                string[] c = ApprovalID.Split(';');
                DateTime CurrentDate = DateTime.Now;
                foreach (var r in c)
                {
                    if (r == "") continue;
                    int TranNo = Convert.ToInt32(r);
                    HREmpLeave objmatch = DB.HREmpLeaves.FirstOrDefault(w => w.TranNo == TranNo);
                    if (objmatch == null)
                    {
                        return "INV_EN";
                    }
                    if (objmatch.FromDate < CurrentDate || objmatch.ToDate < CurrentDate) return "OVER_DATE";
                    HeaderEmpLeave = objmatch;
                    string DocNo = objmatch.Increment.ToString();
                    var _obj = DB.ExDocApprovals
                        .Where(x => x.DocumentNo == objmatch.Increment.ToString())
                        .OrderBy(w => w.ApproveLevel).FirstOrDefault();
                    if (_obj == null) { return "NO_LINE_MN"; }
                    var HOD = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == _obj.Approver);

                    objmatch.ResonToCancel = Remark;
                    objmatch.IsReqCancel = true;
                    objmatch.ChangedBy = User.UserName;
                    objmatch.ChangedOn = DateTime.Now;
                    DB.HREmpLeaves.Attach(objmatch);
                    DB.Entry(objmatch).Property(w => w.ResonToCancel).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.IsReqCancel).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;

                    DB.SaveChanges();

                    if (HOD != null)
                    {
                        #region Send Email
                        HR_VIEW_EmpLeave EmpLeave = new HR_VIEW_EmpLeave();
                        EmpLeave = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.TranNo == objmatch.TranNo);
                        SYWorkFlowEmailObject wfo =
                                   new SYWorkFlowEmailObject("REQCANCEL", WorkFlowType.REQUESTER,
                                        UserType.N, SYDocumentStatus.PENDING.ToString());
                        wfo.SelectListItem = new SYSplitItem(Convert.ToString(DocNo));
                        wfo.User = User;
                        wfo.BS = BS;
                        wfo.UrlView = URL;
                        wfo.ScreenId = ScreenId;
                        wfo.Module = "HR";
                        wfo.ListLineRef = new List<BSWorkAssign>();
                        wfo.Action = SYDocumentStatus.PENDING.ToString();
                        wfo.ObjectDictionary = HeaderEmpLeave;
                        wfo.ListObjectDictionary = new List<object>();
                        wfo.ListObjectDictionary.Add(EmpLeave);
                        wfo.ListObjectDictionary.Add(HOD);
                        if (!string.IsNullOrEmpty(HOD.Email))
                        {
                            WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(HOD);
                            MessageError = wfo.getErrorMessage(result1);
                        }
                        #endregion
                        #region Notifican on Mobile
                        var access = DB.TokenResources.FirstOrDefault(w => w.UserName == HOD.EmpCode);
                        if (access != null)
                        {
                            var LeaveType = DB.HRLeaveTypes.FirstOrDefault(w => w.Code == objmatch.LeaveCode);
                            if (!string.IsNullOrEmpty(access.FirebaseID) && LeaveType != null)
                            {
                                string Desc = EmpLeave.AllName + @" to request Cancel Leave type of " + LeaveType.Description +
                                    " from " + HeaderEmpLeave.FromDate.ToString("yyyy.MM.dd") + " to " + HeaderEmpLeave.ToDate.ToString("yyyy.MM.dd");
                                Notification.Notificationf Noti = new Notification.Notificationf();
                                var clientToken = new List<string>();
                                clientToken.Add(access.FirebaseID);
                                //clientToken.Add("d7Xt0qR7JkfnnLKGf4xCw2:APA91bHfJMAlQRQlZDwDqG9h8FQfbf8lEijFo4zlzI1i17tEVhZVT7lzTAy3q7ePb7vbgok5bxJWQjdSgBM37NKkSQ_mYnsQInV7ZmRHyVOmM6xektGYp0e8AhGSulzpZZnhvuR19v32");
                                var dd = Noti.SendNotification(clientToken, "RequestCancel", Desc);
                            }
                        }
                        #endregion
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
                log.DocurmentAction = ApprovalID;
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
                log.DocurmentAction = ApprovalID;
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
                log.DocurmentAction = ApprovalID;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteLeave(int id)
        {
            try
            {
                HumicaDBContext DBM = new HumicaDBContext();
                HeaderEmpLeave = new HREmpLeave();
                HeaderEmpLeave.TranNo = id;
                var objLeave = DB.HREmpLeaves.Find(id);
                if (objLeave == null)
                    return "LEAVE_NE";
                HeaderEmpLeave = objLeave;
                var objEmp = DB.HREmpLeaveDs.Where(w => w.LeaveTranNo == objLeave.Increment && w.EmpCode == objLeave.EmpCode).ToList();
                foreach (var item in objEmp)
                {
                    DB.HREmpLeaveDs.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                string Approval = SYDocumentStatus.APPROVED.ToString();
                var objEmpClaim = DB.HRClaimLeaves.Where(w => w.DocumentRef == objLeave.Increment.ToString() && w.Status == Approval).ToList();
                foreach (var item in objEmpClaim)
                {
                    item.IsUsed = false;
                    item.DocumentRef = "";
                    DB.HRClaimLeaves.Attach(item);
                    DB.Entry(item).Property(x => x.IsUsed).IsModified = true;
                    DB.Entry(item).Property(x => x.DocumentRef).IsModified = true;
                }
                //Att
                var ListAtt = DB.ATEmpSchedules.Where(w => w.LeaveNo == objLeave.Increment && w.EmpCode == objLeave.EmpCode).ToList();
                foreach (var item in ListAtt)
                {
                    item.LeaveCode = "";
                    item.LeaveNo = -1;
                    DB.ATEmpSchedules.Attach(item);
                    DB.Entry(item).Property(w => w.LeaveNo).IsModified = true;
                    DB.Entry(item).Property(w => w.LeaveCode).IsModified = true;
                }
                DB.HREmpLeaves.Attach(objLeave);
                DB.Entry(objLeave).State = System.Data.Entity.EntityState.Deleted;
                string Increment = objLeave.Increment.ToString();
                var ListApp = DB.ExDocApprovals.Where(w => w.DocumentNo == Increment).ToList();
                foreach (var item in ListApp)
                {
                    DB.ExDocApprovals.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }

                int row = DB.SaveChanges();
                ReGenerateLeaveToken(HeaderEmpLeave.EmpCode, HeaderEmpLeave.FromDate, HeaderEmpLeave.ToDate);
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = HeaderEmpLeave.TranNo.ToString();
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
                log.ScreenId = HeaderEmpLeave.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public void SetAutoApproval(string EmpCode, string Branch, DateTime ToDate)
        {
            ListApproval = new List<ExDocApproval>();
            var DBX = new HumicaDBContext();
            // var LstStaff = DB.HRStaffProfiles.ToList();
            var LstStaff = DB.HRStaffProfiles.Where(w => w.Status == "A" || (DbFunctions.TruncateTime(w.DateTerminate) > DbFunctions.TruncateTime(ToDate) && w.Status == "I")).ToList();
            var ListWorkFlow = DB.HRWorkFlowLeaves.ToList();
            var _staffApp = new HRStaffProfile();
            foreach (var item in ListWorkFlow)
            {
                var Staff = LstStaff.FirstOrDefault(w => w.EmpCode == EmpCode);
                if (item.ApproveBy == "1st")
                {
                    var Read = LstStaff.Where(w => w.EmpCode == Staff.FirstLine).ToList();
                    _staffApp = Read.FirstOrDefault();
                    if (_staffApp != null)
                    {
                        ExDocApproval objApp1 = AddDocApproval(_staffApp, item.Step);
                        ListApproval.Add(objApp1);
                    }
                    HRStaffProfile _staff = LstStaff.FirstOrDefault(w => w.EmpCode == Staff.FirstLine2);
                    if (_staff != null)
                    {
                        ExDocApproval objApp1 = AddDocApproval(_staff, item.Step);
                        ListApproval.Add(objApp1);
                    }
                }
                else if (item.ApproveBy == "2nd")
                {
                    List<HRStaffProfile> Read = LstStaff.Where(w => w.EmpCode == Staff.SecondLine).ToList();
                    _staffApp = Read.FirstOrDefault();
                    if (_staffApp != null)
                    {
                        ExDocApproval objApp1 = AddDocApproval(_staffApp, item.Step);
                        ListApproval.Add(objApp1);
                    }
                    HRStaffProfile _staff = LstStaff.FirstOrDefault(w => w.EmpCode == Staff.SecondLine2);
                    if (_staff != null)
                    {
                        ExDocApproval objApp1 = AddDocApproval(_staff, item.Step);
                        ListApproval.Add(objApp1);
                    }
                }
                else
                {
                    _staffApp = LstStaff.FirstOrDefault(w => w.JobCode == item.ApproveBy && w.Branch == Branch);
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
        }
        public HRStaffProfile getNextApprover(string id, string pro)
        {
            var objStaff = new HRStaffProfile();
            var DBX = new HumicaDBContext();
            //var objHeader = DBX.HRReqPayments.Find(id);
            //if (objHeader == null)
            //{
            //    return new HRStaffProfile();
            //}
            string open = SYDocumentStatus.OPEN.ToString();
            var listCanApproval = DBX.ExDocApprovals.Where(w => w.DocumentNo == id && w.Status == open && w.DocumentType == "LR").ToList();

            if (listCanApproval.Count == 0)
            {
                return new HRStaffProfile();
            }

            var min = listCanApproval.Min(w => w.ApproveLevel);
            var NextApp = listCanApproval.Where(w => w.ApproveLevel == min).First();
            objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == NextApp.Approver);//, objHeader.Property);
            return objStaff;
        }

        #region Edit Entitle
        public string CreateEmpEditEnTitle(string id)
        {
            try
            {
                DB = new HumicaDBContext();
                DB.Configuration.AutoDetectChangesEnabled = false;
                var staff = DB.HRStaffProfiles.FirstOrDefault(x => x.EmpCode == HeaderEditEntitle.EmpCode);
                var lstempLeave = DB.HREmpEditLeaveEntitles.ToList();

                HeaderEditEntitle.Position = HeaderEditEntitle.Position;
                HeaderEditEntitle.DocumentDate = HeaderEditEntitle.DocumentDate;
                HeaderEditEntitle.Balance = HeaderEditEntitle.Balance;
                HeaderEditEntitle.CreatedBy = User.UserName;
                HeaderEditEntitle.CreatedOn = DateTime.Now;
                DB.HREmpEditLeaveEntitles.Add(HeaderEditEntitle);


                int row = DB.SaveChanges();
                Generate_Balance_Edit(HeaderEditEntitle.EmpCode, HeaderEditEntitle.LeaveType, HeaderEditEntitle.InYear);
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = HeaderEmpLeave.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string EditLeaveEnTitle(int id)
        {
            try
            {

                var objMatch = DB.HREmpEditLeaveEntitles.FirstOrDefault(w => w.ID == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                HeaderEditEntitle.EmpCode = objMatch.EmpCode;
                objMatch.EmpName = HeaderEditEntitle.EmpName;
                objMatch.Balance = HeaderEditEntitle.Balance;
                objMatch.DocumentDate = HeaderEditEntitle.DocumentDate;
                objMatch.LeaveType = HeaderEditEntitle.LeaveType;
                objMatch.Position = HeaderEditEntitle.Position;
                objMatch.InYear = HeaderEditEntitle.InYear;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangeOn = DateTime.Now;

                DB.HREmpEditLeaveEntitles.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Modified;
                int row1 = DB.SaveChanges();
                Generate_Balance_Edit(objMatch.EmpCode, objMatch.LeaveType, objMatch.InYear);
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderEditEntitle.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteEditLeaveEnTitle(int id)
        {
            try
            {
                using (var DB = new HumicaDBContext())
                {
                    var objLeave = DB.HREmpEditLeaveEntitles.Find(id);
                    if (objLeave == null)
                    {
                        return "LEAVE_NE";
                    }

                    decimal balance = 0;
                    var Leave = DB.HREmpEditLeaveEntitles
                    .Where(w => w.EmpCode == objLeave.EmpCode && w.LeaveType == objLeave.LeaveType && w.InYear == objLeave.InYear && w.ID != objLeave.ID).ToList();
                    if (Leave.Any()) balance = (decimal)Leave.Sum(w => w.Balance);
                    var leaveb = DB.HREmpLeaveBs.FirstOrDefault(w =>
                        w.LeaveCode == objLeave.LeaveType &&
                        w.EmpCode == objLeave.EmpCode &&
                        w.InYear == objLeave.InYear);

                    if (leaveb != null)
                    {
                        leaveb.Adjustment = balance;
                        DB.Entry(leaveb).State = System.Data.Entity.EntityState.Modified;
                    }

                    DB.Entry(objLeave).State = System.Data.Entity.EntityState.Deleted;

                    int rowsAffected = DB.SaveChanges();
                    return rowsAffected > 0 ? SYConstant.OK : "LEAVE_DEL_FAIL";
                }
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = HeaderEditEntitle.ID.ToString();
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
                log.ScreenId = HeaderEditEntitle.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion

        public List<Employee_Generate_Leave> LoadDataEmpGen(FTFilterData Filter1, List<HRBranch> ListBranch)
        {
            var Policy = DB.ATPolicies.FirstOrDefault();
            DateTime From = Policy.LFromDate;
            DateTime To = Policy.LToDate;
            int year = Filter1.INYear;
            if (From.Year != To.Year) year = Filter1.INYear - 1;
            DateTime FromDate = new DateTime(year, From.Month, From.Day);
            DateTime ToData = new DateTime(Filter1.INYear, To.Month, To.Day);

            var _List = new List<Employee_Generate_Leave>();
            var _listStaff = DB.HRStaffProfiles.AsEnumerable().Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            var _listBanaceLeave = DB.HREmpLeaveBs.Where(w => w.InYear == Filter1.INYear).ToList();
            _listStaff = _listStaff.Where(w => w.LeaveConf.HasValue && w.LeaveConf.Value.Date <= ToData.Date
                    && (w.TerminateStatus == "" || w.TerminateStatus == null)).ToList();
            if (Filter1.Department != null)
                _listStaff = _listStaff.Where(w => w.DEPT == Filter1.Department).ToList();
            if (Filter1.Branch != null)
                _listStaff = _listStaff.Where(w => w.Branch == Filter1.Branch).ToList();
            foreach (var item in _listStaff)
            {
                string StrAction = "No Action";
                if (_listBanaceLeave.Where(w => w.EmpCode == item.EmpCode).Any())
                    StrAction = "Action";
                int years = -1, months = -1, days = -1;
                DateTimeHelper.TimeSpanToDate(DateTime.Now, item.LeaveConf.Value, out years, out months, out days);
                string SLength = "";
                if (years != 0) SLength += years + "y ";
                if (months != 0) SLength += months + "m ";
                if (days != 0) SLength += days + "d";
                DateTime? Enddate = new DateTime();
                Enddate = null;
                string Status = "Active";
                if (item.DateTerminate.Year != 1900)
                {
                    Status = "InActive";
                    Enddate = item.DateTerminate;
                }
                var emp = new Employee_Generate_Leave()
                {
                    Action = StrAction,
                    EmpCode = item.EmpCode,
                    AllName = item.AllName,
                    Gender = item.Sex,
                    StartDate = item.StartDate.Value,
                    EndDate = Enddate,
                    Status = Status,
                    ServiceLength = SLength
                };
                _List.Add(emp);
            }

            return _List;
        }
        public List<HR_STAFF_VIEW> LoadDataEmp(FTFilterData Filter1, List<HRBranch> ListBranch)
        {
            var branchCodes = ListBranch.Select(b => b.Code).ToList();
            var _staffQuery = DBV.HR_STAFF_VIEW.Where(w => w.StatusCode == "A" && branchCodes.Contains(w.BranchID));

            if (!string.IsNullOrEmpty(Filter1.Branch))
                _staffQuery = _staffQuery.Where(w => w.BranchID == Filter1.Branch);

            if (!string.IsNullOrEmpty(Filter1.Division))
                _staffQuery = _staffQuery.Where(w => w.Division == Filter1.Division);

            if (!string.IsNullOrEmpty(Filter1.Department))
                _staffQuery = _staffQuery.Where(w => w.DEPT == Filter1.Department);

            if (!string.IsNullOrEmpty(Filter1.Section))
                _staffQuery = _staffQuery.Where(w => w.Section == Filter1.Section);

            if (!string.IsNullOrEmpty(Filter1.Position))
                _staffQuery = _staffQuery.Where(w => w.JobCode == Filter1.Position);

            if (!string.IsNullOrEmpty(Filter1.LevelCode))
                _staffQuery = _staffQuery.Where(w => w.LevelCode == Filter1.LevelCode);

            return _staffQuery.ToList();
        }

        public List<Employee_ListForwardLeave> LoadDataEmpForward(FTFilterData Filter1, List<HRBranch> ListBranch)
        {
            var _List = new List<Employee_ListForwardLeave>();
            var _listEmpB = DB.HREmpLeaveBs.Where(w => w.InYear == Filter1.FYear).ToList();
            //var staff = DB.HRStaffProfiles.ToList();
            var dep = DB.HRDepartments.ToList();
            var Post = DB.HRPositions.ToList();
            var Level = DP.HRLevels.ToList();
            var ListLeave_Rate = DB.HRLeaveProRates.ToList();
            var ListParam = DB.PRParameters.ToList();
            var forward_up = DP.SYSettings.FirstOrDefault(w => w.SettingName == "LEAVE_FORWARD_BANANCE_UP");
            var staff = DB.HRStaffProfiles.AsEnumerable().Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            var _listStaff = from Staff1 in staff
                             join EmB in _listEmpB on Staff1.EmpCode equals EmB.EmpCode
                             join d in dep on Staff1.DEPT equals d.Code
                             join p in Post on Staff1.JobCode equals p.Code
                             join L in Level on Staff1.LevelCode equals L.Code
                             where EmB.LeaveCode == Filter1.LeaveType && EmB.InYear == Filter1.FYear && Staff1.Status != "I"
                             select new
                             {
                                 Staff1.EmpCode,
                                 Staff1.AllName,
                                 EmB.Balance,
                                 Departmetn = d.Description,
                                 Position = p.Description,
                                 Staff1.StartDate,
                                 Level = L.Description,
                                 Staff1.Branch,
                                 Staff1.DEPT,
                                 Staff1.LevelCode
                             };
            _listStaff = _listStaff.ToList();
            if (Filter1.Department != null)
                _listStaff = _listStaff.Where(w => w.DEPT == Filter1.Department).ToList();
            if (Filter1.Branch != null)
                _listStaff = _listStaff.Where(w => w.Branch == Filter1.Branch).ToList();
            if (Filter1.LevelCode != null)
                _listStaff = _listStaff.Where(w => w.LevelCode == Filter1.LevelCode).ToList();
            decimal _forward = 0;
            if (forward_up != null) _forward = Convert.ToDecimal(forward_up.SettinValue);
            _listStaff = _listStaff.Where(_w => _w.Balance >= _forward).ToList();
            foreach (var item in _listStaff)
            {
                var fw = new Employee_ListForwardLeave();
                fw.EmpCode = item.EmpCode;
                fw.AllName = item.AllName;
                fw.Level = item.Level;
                fw.Department = item.Departmetn;
                fw.Position = item.Position;
                fw.StartDate = item.StartDate.Value;
                fw.Balance = item.Balance.Value;
                if (item.Balance > Filter1.MaxForward)
                    fw.ForWard = Filter1.MaxForward;
                else fw.ForWard = Convert.ToDecimal(item.Balance);
                _List.Add(fw);
            }

            return _List;

        }

        private WorkFlowResult Send_SMS_Telegram(string TemBody, string ChatID, List<object> ListObjectDictionary, string URL)
        {
            try
            {
                // var Telegram = DB.TelegramBots.FirstOrDefault(w=>w.Description== "LeaveRequest");
                var Telegram = DB.TelegramBots.FirstOrDefault(w => w.ChatID == ChatID);
                if (Telegram != null)
                {
                    string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=HTML";
                    string apiToken = Telegram.TokenID;// "835670290:AAGoq8pHBgi0vGHJgCimeMLVGhpNrYzdEfM";
                                                       // "872155850:AAHlcg1gcH6MjZaKtzaPhtQu03PxHQN4ZZU";
                                                       //  string chatId = "504467938"; -1001405576397,-1001429819055 
                    string chatId = Telegram.ChatID;//1001446018688
                    string text = getEmailContentByParam(TemBody, ListObjectDictionary, URL); //TemBody;
                    urlString = String.Format(urlString, apiToken, chatId, text);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    System.Net.WebRequest request = System.Net.WebRequest.Create(urlString);
                    Stream rs = request.GetResponse().GetResponseStream();
                    StreamReader reader = new StreamReader(rs);
                    string line = "";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    while (line != null)
                    {
                        line = reader.ReadLine();
                        if (line != null)
                            sb.Append(line);
                    }
                    string response = sb.ToString();
                    return WorkFlowResult.COMPLETED;
                }
                return WorkFlowResult.TELEGRAM_NOT_SEND;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = HeaderEmpLeave.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return WorkFlowResult.ERROR;
            }

        }
        public string getEmailContentByParam(string text, List<object> ListObjectDictionary, string URL)
        {
            string[] textsp = text.Split(' ');
            if (textsp.LongLength > 0)
            {
                foreach (string param in textsp)
                {
                    if (param.Trim() == "") continue;
                    if (param.Substring(0, 1) == "@")
                    {
                        var objParam = DP.SYEmailParameters.FirstOrDefault(w => w.Parameter == param
                        && w.TemplateID == "TELEGRAM");
                        if (objParam != null)
                        {

                            if (ListObjectDictionary.Count > 0)
                            {
                                string textstr = ClsInformation.GetFieldValues(objParam.ObjectName, ListObjectDictionary, objParam.FieldName, param);
                                if (textstr != null)
                                {
                                    text = text.Replace(param, textstr);
                                }
                            }
                        }
                    }

                }

            }

            string link = URL;
            // text = text.Replace("@NUMBER", number);
            text = text.Replace("@LINK", link);
            // text = text.Replace("@BATCH_NUMBER", batch);

            return text;
        }
        public decimal GetEnitile(string LeaveCode, ClsPeriodLeave periodLeave, PRParameter payParam, List<HRLeaveProRate> ListLeave_Rate, decimal Rate, bool IsResign = false)
        {
            decimal _Balance = 0;
            decimal prorate_Amount_END = 0;
            decimal prorate_Amount_new = 0;
            DateTime DateNow = DateTime.Now;
            DateTime StartDate = periodLeave.StartDate;

            int day = Math.Min(DateNow.Day, DateTime.DaysInMonth(StartDate.Year, StartDate.Month));

            DateTime EDate_OfMonth_new = new DateTime(StartDate.Year, StartDate.Month, day);
            if (DateNow < periodLeave.StartDate) EDate_OfMonth_new = periodLeave.StartDate;
            else if (DateNow.Month != periodLeave.StartDate.Month) EDate_OfMonth_new = periodLeave.StartDate.EndDateOfMonth();
            if (ListLeave_Rate.Count() > 0)
            {
                var LeaveRate = ListLeave_Rate.Where(w => w.Status == "NEWJOIN" && w.LeaveType == LeaveCode).ToList();

                decimal _actWorkDayNew = PRPayParameterObject.Get_WorkingDay(payParam, periodLeave.StartDate, EDate_OfMonth_new);
                HRLeaveProRate _prorateNew = LeaveRate.Where(w => w.ActWorkDayFrom <= _actWorkDayNew && w.ActWorkDayTo
                >= _actWorkDayNew).FirstOrDefault();
                prorate_Amount_new = _prorateNew == null ? 0 : _prorateNew.Rate;
                if (IsResign == true)
                {
                    if (!(periodLeave.StartDate.Year == periodLeave.EndDate.Year && periodLeave.StartDate.Month == periodLeave.EndDate.Month))
                    {
                        var LeaveRate_RESIGN = ListLeave_Rate.Where(w => w.Status == "RESIGN" && w.LeaveType == LeaveCode).ToList();
                        DateTime EndDate_OfMonth_END = periodLeave.EndDate.StartDateOfMonth();
                        decimal _actWorkDayEND = PRPayParameterObject.Get_WorkingDay(payParam, EndDate_OfMonth_END, periodLeave.EndDate);
                        HRLeaveProRate _prorateEnd = LeaveRate_RESIGN.Where(w => w.ActWorkDayFrom <= _actWorkDayEND
                        && w.ActWorkDayTo >= _actWorkDayEND).FirstOrDefault();
                        prorate_Amount_END = _prorateEnd == null ? 0 : _prorateEnd.Rate;
                    }
                }
                int C_Month = DateTimeHelper.GetMonth(periodLeave.StartDate.AddMonths(1), periodLeave.EndDate.AddMonths(-1));
                _Balance = Convert.ToDecimal(Rate * C_Month);
                _Balance += prorate_Amount_new + prorate_Amount_END;
            }
            else
            {
                //int C_Month = DateTimeHelper.GetMonth(periodLeave.StartDate, periodLeave.EndDate);
                //_Balance = Convert.ToDecimal(Rate * C_Month);
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
        public IQueryable<HREmpLeaveD> GetLeaveToken(DateTime fromdate, DateTime todate)
        {
            string approved = SYDocumentStatus.APPROVED.ToString();
            var _list = from Leave in unitOfWork.Repository<HREmpLeave>().Queryable()
                        join Item in unitOfWork.Repository<HREmpLeaveD>().Queryable()
                        on new
                        {
                            Increment = (int)Leave.Increment,
                            EmpCode = Leave.EmpCode
                        }
                        equals new { Increment = (int)Item.LeaveTranNo, EmpCode = Item.EmpCode }
                        where Leave.Status == approved && Item.Status == "Leave" && DbFunctions.TruncateTime(Item.LeaveDate) >= fromdate.Date
                                && DbFunctions.TruncateTime(Item.LeaveDate) <= todate.Date
                        select Item;
            return _list;
        }
        public IQueryable<HREmpLeaveD> GetLeaveTokenPending(DateTime fromdate, DateTime todate, string EmpCode)
        {
            string Pending = SYDocumentStatus.PENDING.ToString();
            string Approve = SYDocumentStatus.APPROVED.ToString();
            IQueryable<HREmpLeaveD> _list = from Leave in DB.HREmpLeaves
                                            join Item in DB.HREmpLeaveDs on new { Increment = (int)Leave.Increment, EmpCode = Leave.EmpCode }
                                                                         equals new { Increment = (int)Item.LeaveTranNo, EmpCode = Item.EmpCode }
                                            where (Leave.Status == Pending || Leave.Status == Approve) && Item.Status == "Leave" && DbFunctions.TruncateTime(Item.LeaveDate) >= fromdate.Date
                                                  && DbFunctions.TruncateTime(Item.LeaveDate) <= todate.Date && Leave.EmpCode == EmpCode
                                            select Item;
            return _list;
        }
        public HREmpLeaveB Calculate_Token(HREmpLeaveB LeaveB, List<HREmpLeaveD> ListLeaveD, List<HRLeaveType> leaveTypes,
            PRParameter payParam, decimal _Balance, List<HRClaimLeave> LstEmpClaim)
        {
            decimal? WHPDay = 8;
            decimal Forward = 0;
            decimal ForwardUse = 0;
            //decimal ForwardUse2 = 0;
            if (payParam != null) WHPDay = payParam.WHOUR;
            List<ListLeaveType> _lstStr = new List<ListLeaveType>();
            _lstStr = GetLeave_SubParent(leaveTypes, LeaveB.LeaveCode);
            var _LeaveD = ListLeaveD.Where(w => w.EmpCode == LeaveB.EmpCode && _lstStr.Where(x => x.LeaveCode == w.LeaveCode).Any()).ToList();
            var _lstLeaveByHour = ListLeaveD.Where(w => w.Remark == "Hours" && w.EmpCode == LeaveB.EmpCode && _lstStr.Where(x => x.LeaveCode == w.LeaveCode).Any()).ToList();

            var Claim = LstEmpClaim.Where(w => _lstStr.Where(x => x.LeaveCode == w.ClaimLeave).Any()).ToList();
            var Rest = Claim.Where(w => w.WorkingType == "RD").Sum(x => x.WorkingHour);
            var PH = Claim.Where(w => w.WorkingType == "PH").Sum(x => x.WorkingHour);
            LeaveB.Rest_Edit = Convert.ToDecimal(Rest / WHPDay);
            LeaveB.PH_Edit = Convert.ToDecimal(PH / WHPDay);

            if (LeaveB.ForWardExp.HasValue)
            {
                if (LeaveB.ForWardExp.Value.Year != 1900)
                {
                    var Result = _LeaveD.Where(w => w.LeaveDate.Date <= LeaveB.ForWardExp.Value.Date).ToList();
                    Forward = Convert.ToDecimal(Result.Sum(x => x.LHour) / WHPDay);
                    if (Forward >= LeaveB.Forward)
                        Forward = Convert.ToDecimal(LeaveB.Forward);
                }
            }

            decimal DayLeave = Convert.ToDecimal(_LeaveD.Sum(w => w.LHour) / WHPDay);
            decimal LeaveByHour = Convert.ToDecimal(_lstLeaveByHour.Sum(w => w.LHour));
            decimal TempLeave = DayLeave;
            //if (DayLeave > _Balance)
            //{
            //    DayLeave = DayLeave - _Balance;
            clsForwards _Forward = new clsForwards();
            _Forward = Calculate_Forward(DayLeave, Forward, ForwardUse, LeaveB.ForwardUse);
            DayLeave = _Forward.DayLeave;
            ForwardUse = _Forward.ForwardUse;
            //}
            DayLeave = DayLeave - _Balance - Forward;
            if (ForwardUse > 0) LeaveB.ForwardUse = ForwardUse;
            var leaveTye = leaveTypes.FirstOrDefault(w => w.Code == LeaveB.LeaveCode && w.IsParent == true);
            if (leaveTye != null)
                LeaveB.Balance = 0;
            else
                LeaveB.Balance = TempLeave - ForwardUse;
            LeaveB.TakenHour = LeaveByHour;
            return LeaveB;
        }
        public List<ListLeaveType> GetLeave_SubParent(List<HRLeaveType> leaveTypes, string Code)
        {
            List<ListLeaveType> _lstStr = new List<ListLeaveType>();
            _lstStr.Add(new ListLeaveType() { LeaveCode = Code });
            foreach (var read in leaveTypes.Where(w => w.IsParent == true && w.Parent == Code).ToList())
            {
                if (!_lstStr.Where(x => x.LeaveCode == read.Code).Any())
                    _lstStr.Add(new ListLeaveType() { LeaveCode = read.Code });
            }
            return _lstStr;
        }
        public List<HREmpLeaveD> GetLeave_D(List<HREmpLeaveD> ListLeaveD, string EmpCode, string LeaveCode, string Unit)
        {
            List<HREmpLeaveD> _lstStr = new List<HREmpLeaveD>();
            int Line_ = 0;
            int Increment_ = GetLastIncrement();
            foreach (var item in ListLeaveD)
            {
                Line_ += 1;
                var result = new HREmpLeaveD();
                result.LeaveTranNo = Increment_ + 1;
                result.EmpCode = EmpCode;
                result.LeaveCode = LeaveCode;
                result.LeaveDate = item.LeaveDate;
                result.CutMonth = item.CutMonth;
                result.Status = item.Status;
                result.Remark = item.Remark;
                result.LineItem = Line_;
                result.LHour = item.LHour;
                if (Unit != "Day")
                {
                    result.Remark = "Hours";
                    result.StartTime = item.StartTime;
                    result.EndTime = item.EndTime;
                }
                if (item.Remark == "Morning" || item.Remark == "Afternoon")
                    result.LHour = item.LHour / 2;
                result.CreateBy = User.UserName;
                result.CreateOn = DateTime.Now;
                _lstStr.Add(result);
            }
            return _lstStr;
        }
        public HREmpLeaveB NewEmpLeaveB(string EmpCode, string EmpName, string LeaveCode, int? InYear)
        {
            var EmpLeaveB = new HREmpLeaveB();
            EmpLeaveB.EmpCode = EmpCode;
            EmpLeaveB.AllName = EmpName;
            EmpLeaveB.LeaveCode = LeaveCode;
            EmpLeaveB.InYear = InYear;
            EmpLeaveB.DayLeave = 0;
            EmpLeaveB.DayEntitle = 0;
            EmpLeaveB.TakenHour = 0;
            EmpLeaveB.BalanceHour = 0;
            EmpLeaveB.YTD = 0;
            EmpLeaveB.Balance = 0;
            EmpLeaveB.Forward = 0;
            EmpLeaveB.ForWardExp = new DateTime(1900, 1, 1);
            EmpLeaveB.CreateOn = DateTime.Now;
            EmpLeaveB.CreateBy = User.UserName;
            return EmpLeaveB;
        }
        public string GetCconMessage(HR_VIEW_EmpLeave Leave, List<HREmpLeaveD> ListEmpleave, HRStaffProfile StaffApp, string Units, string Phone, string URL, decimal? Balance, decimal? WHOUR)
        {
            decimal NoDay = 0;
            decimal TotalHour = 0;
            var data = DP.SYDatas.Where(w => w.DropDownType == "LEAVE_TIME");
            var Approve = DB.HRStaffProfiles.Where(w => w.EmpCode == Leave.EmpCode).FirstOrDefault();
            var Requestor = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).FirstOrDefault();
            var First = DB.HRStaffProfiles.Where(w => w.EmpCode == Approve.FirstLine).FirstOrDefault();
            var second = DB.HRStaffProfiles.Where(w => w.EmpCode == Approve.SecondLine).FirstOrDefault();
            var handOver = DB.HREmpLeaves.Where(w => w.TranNo == Leave.TranNo).FirstOrDefault();
            var NameHandOver = DB.HRStaffProfiles.Where(W => W.EmpCode == handOver.TaskHand_Over).FirstOrDefault();
            //var Handover = DB.HREmpLeaves.Where(w => w.EmpCode == Handover.).FirstOrDefault();

            string str = "<b>កម្មវត្ថុ៖</b> ការស្នើរសុំច្បាប់";
            if (StaffApp.Title == "Ms")
            {
                StaffApp.Title = "Miss";
            }
            str += "<b>%0Aអ្នកស្នើរសុំ៖</b> " + Requestor.Title + (" ") + Requestor.AllName + "(" + Requestor.EmpCode + ")";
            str += "<b>%0Aរង់ចាំបញ្ជាក់ដោយ៖</b> " + First.Title + (" ") + First.AllName + "(" + First.EmpCode + ")";
            str += "<b>%0Aរង់ចាំអនុម័តដោយ៖</b> " + second.Title + (" ") + second.AllName + "(" + second.EmpCode + ")";
            str += "<b>%0Aកាលបរិច្ឆេទស្នើសុំ៖</b> " + Leave.RequestDate.Value.ToString("dd-MMM-yyyy");
            str += "<b>%0Aប្រភេទសុំច្បាប់៖</b> " + Leave.LeaveType;
            str += "<b>%0Aសមតុល្យ៖</b> " + Math.Round((decimal)Balance, 3);
            str += "<b>%0Aកាលបរិច្ឆេទឈប់សម្រាក៖</b> " + Leave.FromDate.ToString("dd-MMM-yyyy") + " to " + Leave.ToDate.ToString("dd-MMM-yyyy");
            //foreach (var item in ListEmpleave)
            //{
            //    if (Units == "Day")
            //    {
            //        NoDay += 1;
            //        string remark = "FullDay";
            //        var result = data.FirstOrDefault(w => w.SelectValue == item.Remark);
            //        if (result != null) { remark = result.SelectText.ToString(); }
            //        str += "%0A" + item.LeaveDate.ToString("<b>dd-MMM-yyyy</b>") + "<b>:</B> " + remark;
            //        if (item.Remark != "FullDay") NoDay -= 0.5m;
            //    }
            //    else if (Units == "Hours") str += "%0A<b>Leave Time:</b>" + item.LeaveDate.ToString("hh:mm tt") + " to " + item.LeaveDate.ToString("hh:mm tt"); //Math.Round(Convert.ToDecimal(item.LHour), 2) + " hours";
            //}
            if ((decimal)ListEmpleave.Sum(w => w.LHour.Value) / WHOUR < WHOUR)
            {
                foreach (var item in ListEmpleave)
                {
                    if (Units == "Day")
                    {
                        if (item.Status != "Rest" && item.Status != "PH")
                            NoDay += 1;
                        string remark = "FullDay";
                        var result = data.FirstOrDefault(w => w.SelectValue == item.Remark);
                        if (result != null) { remark = result.SelectText.ToString(); }
                        if (item.Status != "Leave") remark = item.Status;
                        str += "%0A<b>" + item.LeaveDate.ToString("dd-MMM-yyyy") + ":</b> " + remark;
                        if (item.Remark != "FullDay") NoDay -= 0.5m;
                    }
                    else if (Units == "Hours")
                    {
                        str += "%0A<b>ម៉ោងត្រូវឈប់៖</b> " + item.StartTime.Value.ToString("hh:mm tt") + " to " + item.EndTime.Value.ToString("hh:mm tt");
                        TotalHour += Convert.ToDecimal(item.LHour);
                    }
                }
            }
            else
            {
                foreach (var item in ListEmpleave)
                {
                    if (Units == "Day")
                    {
                        NoDay += 1;
                        if (item.Remark != "FullDay") NoDay -= 0.5m;
                    }
                }
            }
            if (Units == "Day")
                str += "<b>%0Aចំនួនថ្ងៃបានឈប់៖</b> " + (Leave.NoDay - Leave.NoRest);
            else if (Units == "Hours") str += "<b>%0Aចំនួនម៉ោងឈប់៖</b> " + Math.Round(Convert.ToDecimal(ListEmpleave.Sum(w => w.LHour)), 2);
            str += "<b>%0Aមូលហេតុ៖ </b> " + Leave.Reason;
            if (handOver.TaskHand_Over != null)
            {
                str += "<b>%0Aផ្ទេរសិទ្ធិជូន៖</b> " + NameHandOver.Title + (" ") + NameHandOver.AllName + "(" + handOver.TaskHand_Over + ")";
            }
            //str += "<b>%0Aលេខទូរស័ព្ទ  ៖</b> " + Phone;
            //str += "%0A%0A<b>សូមចូលទៅកាន់ប្រព័ន្ធតាមរយៈតំណរភ្ជាប់ ៖</b> <a href=\"@LINK\\\">Link</a>";
            //str = str.Replace("@LINK", URL);
            return str;
        }
        public string GetCconMessageEN(HR_VIEW_EmpLeave Leave, List<HREmpLeaveD> ListEmpleave, HRStaffProfile StaffApp, string Units, string Phone, string URL, decimal? Balance, decimal? WHOUR)
        {
            decimal NoDay = 0;
            decimal TotalHour = 0;
            var data = DP.SYDatas.Where(w => w.DropDownType == "LEAVE_TIME");
            var Approve = DB.HRStaffProfiles.Where(w => w.EmpCode == Leave.EmpCode).FirstOrDefault();
            var Requestor = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).FirstOrDefault();
            var First = DB.HRStaffProfiles.Where(w => w.EmpCode == Approve.FirstLine).FirstOrDefault();
            //var second = DB.HRStaffProfiles.Where(w => w.EmpCode == Approve.SecondLine).FirstOrDefault();
            var handOver = DB.HREmpLeaves.Where(w => w.TranNo == Leave.TranNo).FirstOrDefault();
            var NameHandOver = DB.HRStaffProfiles.Where(W => W.EmpCode == handOver.TaskHand_Over).FirstOrDefault();

            string str = "<b>Subject:</b> Leave Request";

            if (StaffApp.Title == "Ms")
            {
                StaffApp.Title = "Miss";
            }

            str += "<b>%0ARequested By:</b> " + Requestor.Title + (" ") + Requestor.AllName + "(" + Requestor.EmpCode + ")";
            str += "<b>%0ARequested To:</b> " + First.Title + (" ") + First.AllName + "(" + First.EmpCode + ")";
            //str += "<b>%0AWaiting for approval by:</b> " + second.Title + (" ") + second.AllName + "(" + second.EmpCode + ")";
            str += "<b>%0ARequest Date:</b> " + Leave.RequestDate.Value.ToString("dd-MMM-yyyy");
            str += "<b>%0ALeave Type:</b> " + Leave.LeaveType;
            str += "<b>%0ABalance:</b> " + Math.Round((decimal)Balance, 3);
            str += "<b>%0ALeave Dates:</b> " + Leave.FromDate.ToString("dd-MMM-yyyy") + " to " + Leave.ToDate.ToString("dd-MMM-yyyy");

            if ((decimal)ListEmpleave.Sum(w => w.LHour.Value) / WHOUR < WHOUR)
            {
                foreach (var item in ListEmpleave)
                {
                    if (Units == "Day")
                    {
                        if (item.Status != "Rest" && item.Status != "PH")
                            NoDay += 1;
                        string remark = "FullDay";
                        var result = data.FirstOrDefault(w => w.SelectValue == item.Remark);
                        if (result != null) { remark = result.SelectText.ToString(); }
                        if (item.Status != "Leave") remark = item.Status;
                        str += "%0A<b>" + item.LeaveDate.ToString("dd-MMM-yyyy") + ":</b> " + remark;
                        if (item.Remark != "FullDay") NoDay -= 0.5m;
                    }
                    else if (Units == "Hours")
                    {
                        str += "%0A<b>Leave Time:</b> " + item.StartTime.Value.ToString("hh:mm tt") + " to " + item.EndTime.Value.ToString("hh:mm tt");
                        TotalHour += Convert.ToDecimal(item.LHour);
                    }
                }
            }
            else
            {
                foreach (var item in ListEmpleave)
                {
                    if (Units == "Day")
                    {
                        NoDay += 1;
                        if (item.Remark != "FullDay") NoDay -= 0.5m;
                    }
                }
            }

            if (Units == "Day")
                str += "<b>%0ANumber of Days Taken:</b> " + (Leave.NoDay - Leave.NoRest);
            else if (Units == "Hours") str += "<b>%0ANo. of Hour (s) Taken:</b> " + Math.Round(Convert.ToDecimal(ListEmpleave.Sum(w => w.LHour)), 2);

            str += "<b>%0AReason:</b> " + Leave.Reason;
            if (!string.IsNullOrEmpty(Requestor.Phone1))
                str += "<b>%APhone No:</b> " + Requestor.Phone1;
            if (handOver.TaskHand_Over != null)
            {
                str += "<b>%0ATask Hand Over:</b> " + NameHandOver.Title + (" ") + NameHandOver.AllName + "(" + handOver.TaskHand_Over + ")";
            }

            return str;
        }
        public ClsPeriodLeave GetPeriod(ClsPeriodLeave Period, HRStaffProfile Employee, List<HRSetEntitleD> ListetEntitleD,
            int InYear, string LeaveCode)
        {
            bool IsSB = false, IsNew = false, IsRes = false;
            decimal RateSB = 0.5M;
            decimal Rate = 0;
            decimal _Balance = 0;
            decimal _Entitle = 0;
            Period.SeniorityBalance = 0;
            var Policy = unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
            if (Policy.LFromDate.Year == 001)
            {
                Policy.LFromDate = new DateTime(InYear, 01, 01);
                Policy.LToDate = Policy.LFromDate.AddYears(1).AddDays(-1);
            }
            DateTime From = Policy.LFromDate;
            DateTime To = Policy.LToDate;
            int year = InYear;
            if (From.Year != To.Year) year = InYear - 1;
            DateTime FromDate = new DateTime(year, From.Month, From.Day);
            DateTime EndDate = new DateTime(InYear, To.Month, To.Day);
            if (Employee.LeaveConf.Value.Date > FromDate.Date)
            {
                Period.StartDate = Employee.LeaveConf.Value;
                IsNew = true;
            }
            else
                Period.StartDate = FromDate;
            if (Employee.DateTerminate.Year != 1900)
            {
                Period.EndDate = Employee.DateTerminate.AddDays(-1);
                IsRes = true;
            }
            else
            {
                DateTime DateNow = DateTime.Now;
                Period.EndDate = DateTime.Now;
                if (Period.LeaveType.BalanceType == "SB")
                {
                    IsSB = true;
                    DateTime date = To.AddMonths(-6);
                    if (date.Date >= DateNow.Date)
                    {

                        Period.EndDate = date;
                    }
                    else
                    {
                        RateSB = 1;
                        Period.EndDate = To;
                    }
                }
                if (Period.EndDate < Period.StartDate) Period.EndDate = Period.StartDate;
            }
            var servicelength = DateTimeHelper.GetMonth(Employee.LeaveConf.Value, Period.StartDate);
            var ListTempEntitle = ListetEntitleD.Where(w => w.LeaveCode == LeaveCode && w.CodeH == Employee.TemLeave &&
                  w.FromMonth <= servicelength && w.ToMonth >= servicelength).ToList();
            if (Employee.CompanyCode == "LEGB")
            {
                Period.servicelength = DateTimeHelper.GetMonth(Employee.LeaveConf.Value, Period.StartDate);
            }
            else
            {
                Period.servicelength = DateTimeHelper.GetMonth(Employee.LeaveConf.Value, Period.EndDate);
            }
            Period.ListTempEntitle = ListetEntitleD.Where(w => w.LeaveCode == LeaveCode && w.CodeH == Employee.TemLeave &&
                   w.FromMonth <= Period.servicelength && w.ToMonth >= Period.servicelength).ToList();
            Period.EmpCode = Employee.EmpCode;
            Period.ServicePeriod = DateTimeHelper.GetMonth(FromDate, EndDate);
            decimal Entitle = 0;
            decimal SeniorityBalance = 0;
            foreach (var temp in Period.ListTempEntitle)
            {
                Entitle = (decimal)temp.Entitle;
                SeniorityBalance = temp.SeniorityBalance;
                if (ListTempEntitle.FirstOrDefault(w => w.LineItem == temp.LineItem) != null)
                {
                    var TempEntitle = ListTempEntitle.FirstOrDefault(w => w.LineItem == temp.LineItem);
                    Entitle = (decimal)TempEntitle.Entitle + TempEntitle.SeniorityBalance;
                    SeniorityBalance = 0;
                }
                Period.SeniorityBalance = SeniorityBalance;
                if (IsSB && IsNew == false && IsRes == false)
                {
                    _Balance = Entitle * RateSB;
                    //_Entitle = _Balance;
                    _Entitle = (decimal)temp.Entitle;
                }
                else
                {
                    if (temp.IsProRate == true)
                    {
                        Rate = (decimal)(temp.Entitle / Period.ServicePeriod);
                        _Balance = GetEnitile(LeaveCode, Period, Period.payParam, Period.ListLeaveProRate, Rate, true);
                        if ((Employee.Status != "A"))
                            _Entitle = _Balance;
                        else if ((Employee.Status == "A"
                            && Employee.StartDate.Value.Year == InYear))
                        {
                            Period.EndDate = EndDate;
                            _Entitle = GetEnitile(LeaveCode, Period, Period.payParam, Period.ListLeaveProRate, Rate, true);
                        }
                        else if (Employee.DateTerminate.Year == 1900 && Employee.StartDate.Value.Year < InYear)
                            _Entitle = (decimal)temp.Entitle;
                    }
                    else
                    {
                        _Balance = (decimal)temp.Entitle;
                        _Entitle = (decimal)temp.Entitle;
                    }
                }
            }
            Period.Balance = _Balance;
            Period.Entitle = _Entitle;
            return Period;
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
        public static int GetLastIncrement()
        {
            HumicaDBContext DBI = new HumicaDBContext();
            int Increment = DBI.HREmpLeaves.Select(w => w.Increment).DefaultIfEmpty(0).Max();

            return Increment;
        }
        public void Generate_Balance_Edit(string EmpCode, string LeaveType, int? Year)
        {
            HumicaDBContext DBI = new HumicaDBContext();
            string Approval = SYDocumentStatus.APPROVED.ToString();
            var EmpClaim = DBI.HREmpEditLeaveEntitles.Where(w => w.EmpCode == EmpCode && w.InYear == Year && w.LeaveType == LeaveType).ToList();
            HREmpLeaveB LeaveB = DBI.HREmpLeaveBs.Where(w => w.EmpCode == EmpCode && w.LeaveCode == LeaveType
                  && w.InYear == Year).FirstOrDefault();
            HRStaffProfile _Staff = DBI.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode);
            DateTime DateNow = DateTime.Now;

            decimal LeaveAdj = EmpClaim.Sum(w => w.Balance).Value;
            if (LeaveB == null)
            {
                LeaveB = new HREmpLeaveB();
                LeaveB = NewEmpLeaveB(EmpCode, _Staff.AllName, LeaveType, Year);
                LeaveB.YTD = (LeaveB.DayEntitle ?? 0) + LeaveAdj;
                LeaveB.Adjustment = LeaveAdj;
                LeaveB.CreateBy = User.UserName;
                LeaveB.CreateOn = DateTime.Now;
                LeaveB = Calculate_Balance(LeaveB);
                DBI.HREmpLeaveBs.Add(LeaveB);
            }
            else
            {
                LeaveB.Adjustment = LeaveAdj;
                LeaveB.YTD = LeaveB.DayEntitle + LeaveB.PH_Edit + LeaveB.Rest_Edit + LeaveB.SeniorityBalance + LeaveAdj;
                LeaveB.Balance = LeaveB.DayEntitle + LeaveB.PH_Edit + LeaveB.Rest_Edit - LeaveB.DayLeave + LeaveB.SeniorityBalance + LeaveAdj;
                LeaveB = Calculate_Balance(LeaveB);
                DBI.HREmpLeaveBs.Attach(LeaveB);
                DBI.Entry(LeaveB).Property(w => w.Balance).IsModified = true;
                DBI.Entry(LeaveB).Property(w => w.YTD).IsModified = true;
                DBI.Entry(LeaveB).Property(w => w.Rest_Edit).IsModified = true;
                DBI.Entry(LeaveB).Property(w => w.PH_Edit).IsModified = true;
                DBI.Entry(LeaveB).Property(w => w.Adjustment).IsModified = true;
            }
            DBI.SaveChanges();
        }
        public HREmpLeaveB Calculate_Balance(HREmpLeaveB LeaveB)
        {
            if (!LeaveB.Adjustment.HasValue) LeaveB.Adjustment = 0;
            if (!LeaveB.SeniorityBalance.HasValue) LeaveB.SeniorityBalance = 0;
            LeaveB.YTD = LeaveB.DayEntitle + LeaveB.SeniorityBalance + LeaveB.Rest_Edit + LeaveB.PH_Edit + LeaveB.Adjustment;
            LeaveB.Balance = LeaveB.DayEntitle + LeaveB.SeniorityBalance + LeaveB.Rest_Edit + LeaveB.PH_Edit + LeaveB.Adjustment - LeaveB.DayLeave;

            return LeaveB;
        }
        public ExDocApproval AddDocApproval(HRStaffProfile Staff, int Step)
        {
            ExDocApproval objApp = new ExDocApproval();
            objApp.Approver = Staff.EmpCode;
            objApp.ApproverName = Staff.AllName;
            objApp.DocumentType = "LR";
            objApp.ApproveLevel = Step;
            objApp.WFObject = "WF02";

            return objApp;
        }
        public string ValidateEntile(HRLeaveType LeaveType, string leaveCode, PRParameter payParam, HRStaffProfile staff)
        {
            var lstLeaveType = unitOfWork.Repository<HRLeaveType>().Queryable().ToList();

            if (LeaveType.IsCurrent == false || LeaveType.IsOverEntitle == false)
            {
                var EmpLeaveB_ = unitOfWork.Repository<HREmpLeaveB>().Queryable().FirstOrDefault(w => w.InYear == HeaderEmpLeave.FromDate.Year && w.LeaveCode == leaveCode && w.EmpCode == HeaderStaff.EmpCode);
                if (EmpLeaveB_ == null) return "INT_Entile";
                var LeaveD = ListEmpLeaveD.Where(w => w.Status == "Leave").ToList();
                var approved = SYDocumentStatus.APPROVED.ToString();
                List<HRClaimLeave> LstEmpClaim = unitOfWork.Repository<HRClaimLeave>().Queryable().Where(w => w.Status == approved && w.WorkingDate.Year == HeaderEmpLeave.ToDate.Year && (w.IsUsed.Value == true || w.IsExpired.Value == false)).ToList();
                var EmpClaim = LstEmpClaim.Where(w => w.EmpCode == HeaderEmpLeave.EmpCode).ToList();
                HREmpLeaveB lB = new HREmpLeaveB();


                Humica.ESS.ClsPeriodLeave periodLeave1 = new Humica.ESS.ClsPeriodLeave();
                periodLeave1 = Getperiod(HeaderEmpLeave.EmpCode, leaveCode, HeaderEmpLeave.ToDate);
                if (LeaveType.IsCurrent == false)
                {
                    if ((periodLeave1.Balance + periodLeave1.SeniorityBalance) - Convert.ToDecimal(HeaderEmpLeave.NoDay) < 0)
                        return "INV_BALANCE";
                }
                if (LeaveType.IsOverEntitle == false)
                {
                    periodLeave1.ListLeaveItem.AddRange(GetLeave_D(LeaveD, HeaderStaff.EmpCode, leaveCode, HeaderEmpLeave.Units));
                    lB = Calculate_Token(EmpLeaveB_, periodLeave1.ListLeaveItem, lstLeaveType, payParam, EmpLeaveB_.YTD ?? EmpLeaveB_.DayEntitle ?? 0, LstEmpClaim);
                    if ((EmpLeaveB_.YTD - lB.Balance) < 0)
                        return "INT_Entile";
                }
            }

            return SYConstant.OK;
        }
        public Humica.ESS.ClsPeriodLeave Getperiod(string EmpCode, string LeaveCode, DateTime LastDate)
        {
            string UserName = EmpCode;
            Humica.ESS.ClsPeriodLeave periodLeave = new Humica.ESS.ClsPeriodLeave();
            ATPolicy Policy = unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
            ClsLeaveBalance leaveBalance = new ClsLeaveBalance();
            HRStaffProfile _staff = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == UserName);
            HREmpLeaveB empLeaveB = unitOfWork.Repository<HREmpLeaveB>().FirstOrDefault(w => w.EmpCode == UserName && w.LeaveCode == LeaveCode && w.InYear == LastDate.Year);
            PRParameter payParam = unitOfWork.Repository<PRParameter>().FirstOrDefault(w => w.Code == _staff.PayParam);
            var ListLeave_Rate = unitOfWork.Repository<HRLeaveProRate>().Queryable();
            periodLeave.ListLeaveProRate = ListLeave_Rate.ToList();
            var ListetEntitleD = unitOfWork.Repository<HRSetEntitleD>().Queryable().Where(w => w.CodeH == _staff.TemLeave);
            var ListLeaveType = unitOfWork.Repository<HRLeaveType>().Queryable();
            DateTime From = Policy.LFromDate;
            DateTime To = Policy.LToDate;
            int year = LastDate.Year;
            if (From.Year != To.Year) year = LastDate.Year - 1;
            periodLeave.FromDate = new DateTime(year, From.Month, From.Day);
            periodLeave.ToDate = new DateTime(LastDate.Year, To.Month, To.Day);

            periodLeave.ListLeaveType = ListLeaveType.ToList();
            periodLeave.LeaveType = ListLeaveType.FirstOrDefault(w => w.Code == LeaveCode);
            periodLeave.payParam = payParam;
            periodLeave.EmpLeaveB = empLeaveB;
            periodLeave = leaveBalance.GetDate(periodLeave, _staff, LastDate);
            periodLeave.ListTempEntitle = ListetEntitleD.Where(w => w.LeaveCode == LeaveCode && w.CodeH == _staff.TemLeave &&
              w.FromMonth <= periodLeave.servicelength && w.ToMonth >= periodLeave.servicelength).ToList();
            foreach (HRSetEntitleD temp in periodLeave.ListTempEntitle)
            {
                periodLeave.SeniorityBalance = temp.SeniorityBalance;
                if (temp.IsProRate == true)
                {
                    periodLeave.Rate = (decimal)(temp.Entitle / periodLeave.ServicePeriod);
                    periodLeave.Balance = leaveBalance.GetCurrentBalance(LeaveCode, LastDate, periodLeave);
                    if (periodLeave.FromDate.Date == periodLeave.FromDate)
                    {
                        if (periodLeave.ToDate.Date == periodLeave.EndDate)
                        {
                            periodLeave.Balance = (decimal)temp.Entitle;
                        }
                    }
                }
                else
                {
                    periodLeave.Balance = (decimal)temp.Entitle;
                }
            }
            periodLeave.ListLeaveCode = leaveBalance.GetLeave_SubParent(periodLeave.ListLeaveType, LeaveCode);
            periodLeave.ListLeaveItem = GetLeave(periodLeave);
            periodLeave = leaveBalance.GetCurrent(periodLeave, LeaveCode);
            periodLeave.Balance -= periodLeave.Token;
            return periodLeave;
        }
        public List<HREmpLeaveD> GetLeave(Humica.ESS.ClsPeriodLeave periodLeave)
        {
            string Pending = SYDocumentStatus.PENDING.ToString();
            string Approve = SYDocumentStatus.APPROVED.ToString();
            var _list = (from Leave in unitOfWork.Set<HREmpLeave>().AsQueryable()
                         join Item in unitOfWork.Set<HREmpLeaveD>().AsQueryable() on new { Increment = (int)Leave.Increment, EmpCode = Leave.EmpCode }
                                                         equals new { Increment = (int)Item.LeaveTranNo, EmpCode = Item.EmpCode }
                         where (Leave.Status == Pending || Leave.Status == Approve) && Item.Status == "Leave" && Item.LeaveDate >= periodLeave.FromDate.Date
                                 && Item.LeaveDate <= periodLeave.ToDate.Date
                                 && Item.EmpCode == periodLeave.EmpCode
                         select Item).ToList();
            _list = _list.Where(w => periodLeave.ListLeaveCode.Where(x => x.LeaveCode == w.LeaveCode).Any()).ToList();
            return _list;
        }
        public string EvaluateLeave(List<HREmpLeaveD> ListEmpLeaveD, string EmpCode, DateTime FromDate, DateTime ToDate, string Unit, long Increment, bool isedit = false)
        {
            var ATSche = unitOfWork.Repository<ATEmpSchedule>().Queryable().Where(w => w.EmpCode == EmpCode
                && DbFunctions.TruncateTime(w.TranDate) >= FromDate.Date
                && DbFunctions.TruncateTime(w.TranDate) <= ToDate.Date).ToList();

            var Leave_ = new Humica.ESS.ClsPeriodLeave();
            Leave_.ListLeaveCode = unitOfWork.Repository<HRLeaveType>().Queryable().ToList().Select(w => new Humica.ESS.ListLeaveType { LeaveCode = w.Code }).ToList();
            Leave_.FromDate = FromDate;
            Leave_.ToDate = ToDate;
            Leave_.EmpCode = EmpCode;
            var Leave_D = GetLeave(Leave_);
            if (isedit)
            {
                Leave_D = Leave_D.Where(w => w.LeaveTranNo != Increment).ToList();
            }
            foreach (var item in ListEmpLeaveD)
            {
                var Result = Leave_D.Where(x => x.LeaveDate.Date == item.LeaveDate.Date).ToList();
                if (item.Remark == "FullDay")
                    if (Result.Any()) return "INV_DATE";
                if (Unit == "Day")
                {
                    var remark = Result.Where(w => w.Remark == item.Remark).ToList();
                    if (remark.Any()) return "INV_DATE";
                    var Roster = ATSche.FirstOrDefault(w => w.TranDate == item.LeaveDate);
                    if (Roster != null)
                    {
                        TimeSpan timeOffset = new TimeSpan(0, 4, 0, 0);
                        DateTime brackstart = Roster.IN1.Value.Add(timeOffset);
                        DateTime brackend = Roster.OUT1.Value.Subtract(timeOffset);
                        var hour = Result.Where(w => w.Remark == "Hours").ToList();
                        if (hour.Any())
                        {
                            var minStartTime = hour.Min(p => p.StartTime);
                            var minEndTime = hour.Min(p => p.EndTime);
                            bool morningOverlap =
                                (Roster.IN1 >= minStartTime && Roster.IN1 <= minEndTime) ||
                                (Roster.OUT1 >= minStartTime && Roster.OUT1 <= minEndTime) ||
                                (minStartTime >= Roster.IN1 && minStartTime <= Roster.OUT1) ||
                                (minEndTime >= Roster.IN1 && minEndTime <= Roster.OUT1);
                            bool afternoonOverlap =
                                (Roster.IN2 >= minStartTime && Roster.IN2 <= minEndTime) ||
                                (Roster.OUT2 >= minStartTime && Roster.OUT2 <= minEndTime) ||
                                (minStartTime >= Roster.IN2 && minStartTime <= Roster.OUT2) ||
                                (minEndTime >= Roster.IN2 && minEndTime <= Roster.OUT2);
                            if (Roster.Flag1 == 1 && Roster.Flag2 == 1)
                            {
                                if (morningOverlap && item.Remark == "Morning") return "INV_DATE";
                                if (afternoonOverlap && item.Remark == "Afternoon") return "INV_DATE";
                            }
                            else if (Roster.Flag1 == 1 && Roster.Flag2 == 2)
                            {
                                morningOverlap =
                                    (Roster.IN1 >= minStartTime && Roster.IN1 <= minEndTime) ||
                                    (brackstart >= minStartTime && brackstart <= minEndTime) ||
                                    (minStartTime >= Roster.IN1 && minStartTime <= brackstart) ||
                                    (minEndTime >= Roster.IN1 && minEndTime <= brackstart);
                                afternoonOverlap =
                                    (brackend >= minStartTime && brackend <= minEndTime) ||
                                    (Roster.OUT1 >= minStartTime && Roster.OUT1 <= minEndTime) ||
                                    (minStartTime >= brackend && minStartTime <= Roster.OUT1) ||
                                    (minEndTime >= brackend && minEndTime <= Roster.OUT1);
                                if (morningOverlap && item.Remark == "Morning") return "INV_DATE";
                                if (afternoonOverlap && item.Remark == "Afternoon") return "INV_DATE";
                            }
                        }
                    }
                }
                if (Unit == "Hours")
                {
                    var hour = Result.Where(w => w.Remark == "Hours").ToList();
                    var fullday = Result.Where(w => w.Remark == "FullDay").ToList();
                    if (fullday.Any()) return "INV_DATE";
                    var Roster = ATSche.FirstOrDefault(w => w.TranDate == item.LeaveDate);
                    if (Roster != null)
                    {
                        TimeSpan timeOffset = new TimeSpan(0, 4, 0, 0);
                        DateTime brackstart = Roster.IN1.Value.Add(timeOffset);
                        DateTime brackend = Roster.OUT1.Value.Subtract(timeOffset);
                        var morningLeave = Result.FirstOrDefault(w => w.Remark == "Morning");
                        var afternoonLeave = Result.FirstOrDefault(w => w.Remark == "Afternoon");
                        bool IsTimeOverlap(DateTime inTime, DateTime outTime) =>
                            (inTime >= item.StartTime && inTime <= item.EndTime) ||
                            (outTime >= item.StartTime && outTime <= item.EndTime) ||
                            (item.StartTime >= inTime && item.StartTime <= outTime) ||
                            (item.EndTime >= inTime && item.EndTime <= outTime);
                        if (Roster.Flag1 == 1 && Roster.Flag2 == 1)
                        {
                            if (Roster.IN1.Value > item.StartTime)
                            {
                                MessageError = Roster.IN1.Value.ToString("hh:mm tt");
                                MessageError2 = Roster.OUT2.Value.ToString("hh:mm tt");
                                return "INV_TIME_SHIFT";
                            }
                            if (Roster.OUT2.Value < item.EndTime)
                            {
                                MessageError = Roster.IN1.Value.ToString("hh:mm tt");
                                MessageError2 = Roster.OUT2.Value.ToString("hh:mm tt");
                                return "INV_TIME_SHIFT";
                            }
                            if (morningLeave != null && IsTimeOverlap(Roster.IN1.Value, Roster.OUT1.Value)) return "INV_DATE";
                            if (afternoonLeave != null && IsTimeOverlap(Roster.IN2.Value, Roster.OUT2.Value)) return "INV_DATE";
                        }
                        else if (Roster.Flag1 == 1 && Roster.Flag2 == 2)
                        {
                            if (Roster.IN1.Value > item.StartTime)
                            {
                                MessageError = Roster.IN1.Value.ToString("hh:mm tt");
                                MessageError2 = Roster.OUT1.Value.ToString("hh:mm tt");
                                return "INV_TIME_SHIFT";
                            }
                            if (Roster.OUT1.Value < item.EndTime)
                            {
                                MessageError = Roster.IN1.Value.ToString("hh:mm tt");
                                MessageError2 = Roster.OUT1.Value.ToString("hh:mm tt");
                                return "INV_TIME_SHIFT";
                            }
                            if (morningLeave != null && IsTimeOverlap(Roster.IN1.Value, brackstart)) return "INV_DATE";
                            if (afternoonLeave != null && IsTimeOverlap(brackend, Roster.OUT1.Value)) return "INV_DATE";
                        }
                    }
                    if (hour.Any())
                    {
                        var minStartTime = hour.Min(p => p.StartTime);
                        var minEndTime = hour.Min(p => p.EndTime);
                        bool startsInRange = minStartTime <= item.StartTime.Value.AddMinutes(1) && minEndTime >= item.StartTime.Value.AddMinutes(1);
                        bool endsInRange = minStartTime <= item.EndTime.Value.AddMinutes(1) && minEndTime >= item.EndTime.Value.AddMinutes(1);
                        if (startsInRange || endsInRange) return "INV_DATE";
                    }
                }
            }
            return SYConstant.OK;
        }
        
    }
    public class Employee_Generate_Leave
    {
        public string Action { get; set; }
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string Gender { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public string ServiceLength { get; set; }
    }
    public class ListLeaveType
    {
        public string LeaveCode { get; set; }
    }
    public partial class Employee_ListForwardLeave
    {
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Balance { get; set; }
        public string Level { get; set; }
        public int InYear { get; set; }
        public string LeaveCode { get; set; }
        public string DEPT { get; set; }
        public string LevelCode { get; set; }
        public string Branch { get; set; }
        public decimal ForWard { get; set; }
    }
    public class ClsUnits
    {
        public string Description { get; set; }
        public string SecDescription { get; set; }
        public static List<ClsUnits> LoadUnit()
        {
            List<ClsUnits> _lst = new List<ClsUnits>();
            _lst.Add(new ClsUnits { Description = "Day", SecDescription = "ថ្ងៃ" });
            _lst.Add(new ClsUnits { Description = "Hours", SecDescription = "ម៉ោង" });
            return _lst;
        }
    }
    public class ClsPeriodLeave
    {
        public PRParameter payParam { get; set; }
        public HREmpLeaveB EmpLeaveB { get; set; }
        public List<HRLeaveType> ListLeaveType { get; set; }
        public HRLeaveType LeaveType { get; set; }
        public List<HRSetEntitleD> ListTempEntitle { get; set; }
        public List<HRLeaveProRate> ListLeaveProRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int servicelength { get; set; }
        public int ServicePeriod { get; set; }
        public decimal SeniorityBalance { get; set; }
        public decimal Balance { get; set; }
        public decimal Entitle { get; set; }
        public string EmpCode { get; set; }
    }
    public class clsForwards
    {
        public decimal DayLeave { get; set; }
        public decimal Forward { get; set; }
        public decimal ForwardUse { get; set; }
        public decimal ForwardUsed { get; set; }
    }
}//2757