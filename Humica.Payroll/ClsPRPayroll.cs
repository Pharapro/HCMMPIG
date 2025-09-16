using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.Helper;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Payroll
{
    public class ClsPRPayroll : IClsPRPayroll
    {
        protected IUnitOfWork unitOfWork;
        public string ScreenId { get; set; }
        public string EmpID { get; set; }
        public string MessageCode { get; set; }
        public SYUser User = new SYUser();
        public SYUserBusiness BS { get; set; }
        public FTFilerPayroll Filter { get; set; }
        public List<ClsEmpGenerateSalary> ListEmployeeGen { get; set; }
        public List<HISPaySlip> ListPaySlip { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public List<ListUploadPayHis> ListImport { get; set; }
        public List<SYEventLog> ListLog { get; set; }
        public string MessageError { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        public ClsPRPayroll()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }

        public void OnIndexLoading()
        {
            ListEmployeeGen = new List<ClsEmpGenerateSalary>();
            Filter = new FTFilerPayroll();
            var Period = unitOfWork.Repository<PRPayPeriodItem>().Queryable().Where(w => w.IsActive == true).OrderByDescending(w => w.StartDate).ToList();
            if (Period.Count > 0)
            {
                Filter.Period = Period.FirstOrDefault().PeriodID;
            }
        }
        public string OnIndexLoadingFilter()
        {
            var Period = unitOfWork.Repository<PRPayPeriodItem>().Queryable().FirstOrDefault(w => w.PeriodID == Filter.Period);
            if (Period == null)
            {
                return "PERIOD_REQURED";
            }
            List<HRBranch> ListBranch = SYConstant.getBranchDataAccess();
            DateTime date = new DateTime(1900, 1, 1);
            DateTime InMonth = Period.EndDate;
            var TerminateDate = Period.StartDate.Date.AddDays(1);

            var _ListEmpGen = new List<ClsEmpGenerateSalary>();
            var _listStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w =>
            (string.IsNullOrEmpty(Filter.Branch) || w.Branch == Filter.Branch) &&
            (string.IsNullOrEmpty(Filter.BusinessUnit) || w.GroupDept == Filter.BusinessUnit) &&
            (string.IsNullOrEmpty(Filter.Division) || w.Division == Filter.Division) &&
            (string.IsNullOrEmpty(Filter.Department) || w.DEPT == Filter.Department) &&
            (string.IsNullOrEmpty(Filter.Office) || w.Office == Filter.Office) &&
            (string.IsNullOrEmpty(Filter.Section) || w.SECT == Filter.Section) &&
            (string.IsNullOrEmpty(Filter.Group) || w.Groups == Filter.Group) &&
            (string.IsNullOrEmpty(Filter.Position) || w.JobCode == Filter.Position) &&
            (string.IsNullOrEmpty(Filter.Level) || w.LevelCode == Filter.Level) &&
            (DbFunctions.TruncateTime(w.StartDate.Value) <= InMonth.Date &&
            ((DbFunctions.TruncateTime(w.DateTerminate) == date.Date
            || DbFunctions.TruncateTime(w.DateTerminate) >= TerminateDate))
            || (w.SalaryFlag == true
            && w.ReSalary.Year == InMonth.Year && w.ReSalary.Month == InMonth.Month))
            ).ToList();

            _listStaff = _listStaff.Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            _listStaff = _listStaff.Where(w => w.IsHold != true).ToList();
            ClsFilterStaff staffJob = new ClsFilterStaff();
            List<ClsEmpGenerateSalary> ListEmpSalary = new List<ClsEmpGenerateSalary>();
            foreach (var emp in _listStaff)
            {
                var empSalary = new ClsEmpGenerateSalary();
                empSalary.EmpCode = emp.EmpCode;
                empSalary.AllName = emp.AllName;
                empSalary.Sex = emp.Sex;
                empSalary.Department = staffJob.Get_Department(emp.DEPT);
                empSalary.Position = staffJob.Get_Positon(emp.JobCode);
                empSalary.StartDate = emp.StartDate;
                empSalary.Status = staffJob.Get_Career(emp.CareerDesc);
                if (emp.DateTerminate.Year == 1900)
                {
                    empSalary.EndDate = null;
                }
                else
                {
                    empSalary.EndDate = emp.DateTerminate;
                }
                empSalary.PayParam = emp.PayParam;
                empSalary.DateTerminate = emp.DateTerminate;
                ListEmpSalary.Add(empSalary);
            }
            ListEmployeeGen = ListEmpSalary;
            return SYConstant.OK;
        }
        public string GenerateSalary()
        {
            OnLoad();
            string Result = "";
            try
            {
                var Period = unitOfWork.Repository<PRPayPeriodItem>().Queryable().FirstOrDefault(w => w.PeriodID == Filter.Period);
                if (Period == null)
                {
                    return "PERIOD_REQURED";
                }
                if (string.IsNullOrEmpty(EmpID))
                {
                    return "EMPCODE_EN";
                }
                var ExchangeRate = unitOfWork.Repository<PRExchRate>().Queryable().FirstOrDefault(w => w.PeriodID == Period.PeriodID);
                var LstAppSalary = unitOfWork.Repository<HisPendingAppSalary>().Queryable().Where(w => w.PeriodID == Period.PeriodID).ToList();
                if (LstAppSalary.Where(w => w.IsLock == true).Count() > 0)
                {
                    return "APPROVE_SALARY";
                }

                List<string> ListEmpCode = EmpID.Split(';').Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
                bool IsGenerate = false;
                ClsFilterPayroll _FPayrllMaster = new ClsFilterPayroll();
                _FPayrllMaster.OnLoadSetting();
                _FPayrllMaster.OnLoadDataEmp(ListEmpCode);
                _FPayrllMaster.OnLoadDataBenefit(ListEmpCode, Period);
                _FPayrllMaster.OnLadaDataSalryFrist(ListEmpCode, Period);
                _FPayrllMaster.OnLoadDataGL();
                Delete_Generate(ListEmpCode, Period.PeriodID);
                string[] Emp = EmpID.Split(';');
                foreach (var Code in Emp)
                {
                    MessageCode = "";
                    if (Code.Trim() != "")
                    {
                        Result = Code;
                        ClsSalary _Filter = new ClsSalary();
                        _Filter = New_Filter(Period);
                        _Filter.HisAppSalary = LstAppSalary.FirstOrDefault(w => w.IsLock == false);
                        //_Filter.NSSFSalaryType = _FPayrllMaster.HRSetting.NSSFSalaryType;
                        _Filter.Staff = _FPayrllMaster.ListEmployee.FirstOrDefault(w => w.EmpCode == Code);
                        if (_FPayrllMaster.PayrollSetting.BaseCurrency != _Filter.Staff.Currency)
                        {
                            if (ExchangeRate == null)
                            {
                                return "EXCHANGERATE";
                            }
                            else
                            {
                                _Filter.ExchangeRate = ExchangeRate.Rate;
                                _Filter.NSSFRate = ExchangeRate.NSSFRate;
                            }
                        }
                        else
                        {
                            _Filter.ExchangeRate = 1;
                            _Filter.NSSFRate = 1;
                        }
                        DateTime FromDate = Period.StartDate;
                        DateTime ToDate = Period.EndDate;
                        DateTime tempFromDate = FromDate;
                        _Filter.Parameter = _FPayrllMaster.ListParameter.FirstOrDefault(w => w.Code == _Filter.Staff.PayParam);
                        if (!_Filter.Parameter.IsPrevoiuseMonth.IsNullOrZero())
                        {
                            DateTime tempDate = tempFromDate.AddMonths(-1);
                            FromDate = new DateTime(tempDate.Year, tempDate.Month, _Filter.Parameter.FROMDATE.Value().Day);
                            ToDate = new DateTime(ToDate.Year, ToDate.Month, _Filter.Parameter.TODATE.Value().Day);
                        }
                        _Filter.EmpCode = Code;
                        _Filter.CompanyCode = _Filter.Staff.CompanyCode;
                        _Filter.FromDate = FromDate;
                        _Filter.ToDate = ToDate;
                        _Filter.ATFromDate = Period.ATStartDate;
                        _Filter.ATToDate = Period.ATEndDate;
                        _Filter = Get_Filter(_Filter, _FPayrllMaster);
                        Generate_Salary(_Filter, _FPayrllMaster, ref IsGenerate);
                        if (_Filter.HeaderSalary.EmpCode != null)
                        {
                            if (_FPayrllMaster.ListSalaryFistPay.Where(w => w.EmpCode == _Filter.EmpCode).ToList().Count() > 0)
                            {
                                var _netPay = _FPayrllMaster.ListSalaryFistPay.Where(w => w.EmpCode == _Filter.EmpCode).ToList().Sum(w => w.NetWage);
                                _Filter.HeaderSalary.FirstPaymentAmount = _netPay.Value;
                            }
                            if (_Filter.HeaderSalary.FirstPaymentAmount > 0 && _Filter.Staff.IsPayPartial != true) _Filter.HeaderSalary.FirstPaymentAmount = 0;

                            Calculate_Overtime(_Filter, _FPayrllMaster);
                            Calculate_Allowance(_Filter, _FPayrllMaster);
                            Calculate_Deduction(_Filter, _FPayrllMaster);
                            //Calculate_Late_Early_Deduction(_Filter, FromDate, ToDate);
                            //Calculate_Misscan_Deduction(_Filter, FromDate, ToDate);
                            Calculate_Bonus(_Filter, _FPayrllMaster);
                            if (_Filter.Staff.IsAtten != true)
                                Calculate_LeaveDeduct(_Filter, _FPayrllMaster);
                            Calculate_Tax(_Filter, _FPayrllMaster, FromDate, ToDate);
                            //Commit_PaySlip(_Filter, _FPayrllMaster);
                            //    Commit_PayHis(_Filter, _listPayHis.Where(w => w.EmpCode == Code).ToList());
                            if (!string.IsNullOrEmpty(_Filter.Staff.GrpGLAcc) && _Filter.Staff.GrpGLAcc != "null")
                                Commit_PayCostAccount(_Filter, _FPayrllMaster);
                            //    Get_CostCenter(_Filter);
                            CommitData(_Filter);
                        }
                        if (!string.IsNullOrEmpty(MessageCode))
                        {
                            if (ListLog == null) ListLog = new List<SYEventLog>();
                            var obj = new SYEventLog();
                            obj.LogDate = DateTime.Now;
                            obj.UserId = User.UserName;
                            obj.ScreenId = ScreenId;
                            obj.LogTime = DateTime.Now.Date.TimeOfDay;
                            obj.Action = SYActionBehavior.ADD.ToString();
                            obj.DocurmentAction = Result;
                            obj.LogErrorMessage = MessageCode;
                            ListLog.Add(obj);
                        }
                    }
                }
                if (ListLog != null && ListLog.Count > 0)
                {
                    try
                    {
                        unitOfWork.BeginTransaction();
                        unitOfWork.BulkInsert(ListLog);
                        unitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.Rollback();
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Result, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Result, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Result, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public void Delete_Generate(List<string> ListEmpCode, int Period)
        {
            unitOfWork.BeginTransaction();
            try
            {
                var ListEmpSalary = unitOfWork.Repository<HISGenSalary>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpSalaryD = unitOfWork.Repository<HISGenSalaryD>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpOT = unitOfWork.Repository<HISGenOTHour>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpReward = unitOfWork.Repository<HisEmpReward>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period);
                var ListEmpDedLeave = unitOfWork.Repository<HISGenLeaveDeduct>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpPaySlip = unitOfWork.Repository<HISPaySlip>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpGLCharge = unitOfWork.Repository<HISGLBenCharge>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpPayment = unitOfWork.Repository<HisEmpPayment>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpSocialSecurity = unitOfWork.Repository<HisEmpSocialSecurity>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpSalaryPay = unitOfWork.Repository<HisSalaryPay>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                if (ListEmpSalary.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpSalary);
                }
                // delete salary d
                if (ListEmpSalaryD.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpSalaryD);
                }
                // delete overtime
                if (ListEmpOT.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpOT);
                }
                unitOfWork.BulkDelete(ListEmpReward.ToList());
                // delete leave deduction
                if (ListEmpDedLeave.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpDedLeave);
                }
                // delete payslip
                if (ListEmpPaySlip.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpPaySlip);
                }
                if (ListEmpGLCharge.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpGLCharge);
                }
                unitOfWork.BulkDelete(ListEmpPayment);
                unitOfWork.BulkDelete(ListEmpSocialSecurity);
                unitOfWork.BulkDelete(ListEmpSalaryPay);
                unitOfWork.BulkCommit();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.DELETE.ToString(), e, true);
            }
        }
        public string Delete_PayRecord(string EmpCode, int Period)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var AppSalary = unitOfWork.Repository<HisPendingAppSalary>().Queryable().Where(w => w.PeriodID == Period && w.IsLock == true).ToList();
                if (AppSalary.Count() > 0)
                {
                    return "APPROVE_SALARY";
                }
                var ListEmpSalary = unitOfWork.Repository<HISGenSalary>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                var ListEmpSalaryD = unitOfWork.Repository<HISGenSalaryD>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                var ListEmpOT = unitOfWork.Repository<HISGenOTHour>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                var ListEmpReward = unitOfWork.Repository<HisEmpReward>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                var ListEmpDedLeave = unitOfWork.Repository<HISGenLeaveDeduct>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                var ListEmpGLCharge = unitOfWork.Repository<HISGLBenCharge>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                var ListEmpPayment = unitOfWork.Repository<HisEmpPayment>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                var ListEmpSoSe = unitOfWork.Repository<HisEmpSocialSecurity>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                var ListEmpSalaryPay = unitOfWork.Repository<HisSalaryPay>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();

                unitOfWork.BulkDelete(ListEmpSalary);
                unitOfWork.BulkDelete(ListEmpSalaryD);
                unitOfWork.BulkDelete(ListEmpOT);
                unitOfWork.BulkDelete(ListEmpReward);
                unitOfWork.BulkDelete(ListEmpDedLeave);
                unitOfWork.BulkDelete(ListEmpPayment);
                unitOfWork.BulkDelete(ListEmpSoSe);
                unitOfWork.BulkDelete(ListEmpSalaryPay);
                unitOfWork.BulkCommit();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.DELETE.ToString(), e, true);
            }
        }
        public string uploaddata()
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                if (!ListImport.Any()) return "INV_DATA";
                bool IsDelete = false;
                foreach (var item in ListImport)
                {
                    if (item.ToDate == null || item.ToDate == DateTime.MinValue) return "INV_DATE";
                    if (item.FromDate == null || item.FromDate == DateTime.MinValue) return "INV_DATE";
                    item.EmpCode = item.EmpCode.Trim();
                    item.Company = item.Company.Trim();
                    if (string.IsNullOrEmpty(item.Company)) return "INV_COMPANYCODE";
                    var staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == item.EmpCode);
                    if (staff == null)
                    {
                        MessageError = item.EmpCode;
                        return "INV_EMPCODE";
                    }
                    var year = item.ToDate.Year;
                    var month = item.ToDate.Month;
                    if (!IsDelete)
                    {
                        IsDelete = true;
                        var salariesDelete = unitOfWork.Repository<HISGenSalary>().Queryable().Where(w => w.INYear == year && w.INMonth == month).ToList();
                        if (salariesDelete.Any())
                            unitOfWork.BulkDelete(salariesDelete);
                        var salaryDsDelete = unitOfWork.Repository<HISGenSalaryD>().Queryable().Where(w => w.INYear == year && w.INMonth == month).ToList();
                        if (salaryDsDelete.Any())
                            unitOfWork.BulkDelete(salaryDsDelete);
                        var leaveDelete = unitOfWork.Repository<HISGenLeaveDeduct>().Queryable().Where(w => w.INYear == year && w.INMonth == month).ToList();
                        if (leaveDelete.Any())
                            unitOfWork.BulkDelete(leaveDelete);
                        var AlwDelete = unitOfWork.Repository<HISGenAllowance>().Queryable().Where(w => w.INYear == year && w.INMonth == month).ToList();
                        if (AlwDelete.Any())
                            unitOfWork.BulkDelete(AlwDelete);
                    }
                    if (item.Extra_Motivation > 0 || item.Monthly_Seniority > 0 || item.Semester_Seniority > 0 || item.Gasoline_Phone > 0 || item.Adjustment > 0)
                    {
                        void AddAllowance(string code, string description, decimal amount)
                        {
                            if (amount > 0)
                            {
                                var allowance = new HISGenAllowance
                                {
                                    //Company = item.Company,
                                    INYear = year,
                                    INMonth = month,
                                    EmpCode = item.EmpCode,
                                    FromDate = item.FromDate,
                                    ToDate = item.ToDate,
                                    WorkDay = 0,
                                    RatePerDay = 0,
                                    TaxAble = false,
                                    FringTax = false,
                                    AllwAmPM = 0,
                                    CreateBy = User.UserName,
                                    CreateOn = DateTime.Now,
                                    AllwCode = code,
                                    AllwDesc = description,
                                    OthDesc = description,
                                    AllwAm = amount
                                };
                                unitOfWork.Add(allowance);
                            }
                        }

                        AddAllowance("EM", "ExtraMotivation", item.Extra_Motivation);
                        AddAllowance("MS", "MonthlySeniority", item.Monthly_Seniority);
                        AddAllowance("SS", "SemesterSeniority", item.Semester_Seniority);
                        AddAllowance("GP", "GasolinePhone", item.Gasoline_Phone);
                        AddAllowance("ADJ", "Adjustment", item.Adjustment);
                    }
                    if (item.UnpaidLeave > 0 && item.UnpaidLeaveD > 0)
                    {
                        var _Gen = new HISGenLeaveDeduct
                        {
                            //Company = item.Company,
                            INYear = year,
                            INMonth = month,
                            EmpCode = item.EmpCode,
                            LeaveCode = "UPLOAND",
                            LeaveDesc = "UPLOAND",
                            LeaveOthDesc = "UPLOAND",
                            LeaveDate = item.ToDate,
                            //Branch = item.Company,
                            ForMular = " ",
                            BaseSalary = item.BasicSalary,
                            WorkDay = item.WorkDay,
                            WorkHour = 8,
                            Measure = "",
                            Qty = item.UnpaidLeaveD,
                            Rate = item.Rate,
                            Amount = item.UnpaidLeave,
                            CreateBy = User.UserName,
                            CreateOn = DateTime.Now
                        };
                        unitOfWork.Add(_Gen);
                    }
                    var newSalaryDetail = new HISGenSalaryD
                    {
                        CompanyCode = item.Company,
                        INYear = year,
                        INMonth = month,
                        FromDate = item.FromDate,
                        ToDate = item.ToDate,
                        CareerCode = staff.CareerDesc,
                        WorkDay = item.WorkDay,
                        WorkHour = 0,
                        EmpCode = item.EmpCode,
                        EmpType = staff.EmpType,
                        Branch = staff.Branch,
                        Location = staff.LOCT,
                        Division = staff.Division,
                        DEPT = staff.DEPT,
                        LINE = staff.Line,
                        CATE = staff.CATE,
                        Sect = staff.SECT,
                        POST = staff.JobCode,
                        LevelCode = staff.LevelCode,
                        PayFrom = item.ToDate,
                        PayTo = item.FromDate,
                        ActWorkDay = 0,
                        BasicSalary = item.BasicSalary,
                        Rate = item.Rate,
                        Amount = item.BasicSalary,
                        CreateBy = User.UserName,
                        CreateOn = DateTime.Now
                    };
                    unitOfWork.Add(newSalaryDetail);

                    var newSalaryRecord = new HISGenSalary
                    {
                        CompanyCode = item.Company,
                        Status = SYDocumentStatus.OPEN.ToString(),
                        TermStatus = staff.TerminateStatus,
                        TermRemark = staff.TerminateRemark,
                        TermDate = staff.DateTerminate,
                        INYear = year,
                        INMonth = month,
                        FromDate = item.FromDate,
                        ToDate = item.ToDate,
                        WorkDay = item.WorkDay,
                        WorkHour = 0,
                        ExchRate = item.ExchangeRate,
                        NSSFRate = item.ExchangeRate,
                        EmpCode = item.EmpCode,
                        EmpName = staff.AllName,
                        EmpType = staff.EmpType,
                        Branch = staff.Branch,
                        Location = staff.LOCT,
                        Division = staff.Division,
                        DEPT = staff.DEPT,
                        LINE = staff.Line,
                        CATE = staff.CATE,
                        Sect = staff.SECT,
                        JobGrade = staff.JobCode,
                        LevelCode = staff.LevelCode,
                        Sex = staff.Sex,
                        ICNO = string.Empty,
                        SOCSO = staff.SOCSO,
                        BankName = staff.BankName,
                        BankBranch = staff.BankBranch,
                        BankAcc = staff.BankAcc,
                        Spouse = item.Spouse,
                        Child = item.Child,
                        DateJoin = staff.StartDate,
                        ActWorkDay = 0,
                        Salary = item.BasicSalary,
                        //BaseSalary = item.BasicSalary,
                        ADVPay = 0,
                        UTAXCH = 0,
                        UTAXSP = 0,
                        USERGEN = User.UserName,
                        DATEGEN = DateTime.Now,
                        TAXBONAM = 0,
                        CostCenter = staff.Costcent,
                        TXPayType = staff.TXPayType,
                        NWAM = 0,
                        PayBack = 0,
                        ShiftPay = 0,
                        LOAN = item.Loan,
                        LeaveDeduct = item.UnpaidLeave,
                        OTAM = item.OT,
                        GrossPay = item.GrossPay,
                        TAXAM = item.Tax,
                        //Association = item.Association,
                        StaffPensionFundAmount = item.CompanyPensionFundAmount,
                        NetWage = item.NetWage,
                        FirstPaymentAmount = 0
                    };
                    unitOfWork.Add(newSalaryRecord);
                }

                var firstItem = ListImport.FirstOrDefault();
                var existingApproval = unitOfWork.Repository<HisPendingAppSalary>().Queryable()
                    .FirstOrDefault(w => w.ToDate.Month == firstItem.ToDate.Month && w.ToDate.Year == firstItem.ToDate.Year);
                if (existingApproval == null)
                {
                    var Period = unitOfWork.Repository<PRPayPeriodItem>().Queryable()
                        .FirstOrDefault(x => x.StartDate.Month == firstItem.ToDate.Month && x.EndDate.Year == firstItem.ToDate.Year);

                    if (Period == null) return "INV_PAYPERIOD";

                    var newAppSalary = new HisPendingAppSalary
                    {
                        CompanyCode = firstItem.Company,
                        PeriodID = Period.PeriodID,
                        FromDate = Period.StartDate,
                        ToDate = Period.EndDate,
                        IsLock = false,
                    };
                    unitOfWork.Add(newAppSalary);
                }

                unitOfWork.Save();
                unitOfWork.Commit();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.DELETE.ToString(), e, true);
            }
        }
        public string Delete_GenerateAll(int Period)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var AppSalary = unitOfWork.Repository<HisPendingAppSalary>().Queryable().Where(w => w.PeriodID == Period && w.IsLock == true).ToList();
                if (AppSalary.Count() > 0)
                {
                    return "APPROVE_SALARY";
                }
                List<HRBranch> ListBranch = SYConstant.getBranchDataAccess();
                var ListBramchCode = ListBranch.Select(b => b.Code).ToList();
                var ListStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w => ListBramchCode.Contains(w.Branch)).ToList();
                var ListEmpCode = ListStaff.Select(b => b.EmpCode).ToList();

                var ListEmpSalary = unitOfWork.Repository<HISGenSalary>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpSalaryD = unitOfWork.Repository<HISGenSalaryD>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpOT = unitOfWork.Repository<HISGenOTHour>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                //var ListEmpAllownace = unitOfWork.Repository<HISGenAllowance>().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                //var ListEmpBonus = unitOfWork.Repository<HISGenBonu>().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                //var ListEmpDeduct = unitOfWork.Repository<HISGenDeduction>().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpReward = unitOfWork.Repository<HisEmpReward>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpDedLeave = unitOfWork.Repository<HISGenLeaveDeduct>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpGLCharge = unitOfWork.Repository<HISGLBenCharge>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpPayment = unitOfWork.Repository<HisEmpPayment>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpSoSe = unitOfWork.Repository<HisEmpSocialSecurity>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpSalaryPay = unitOfWork.Repository<HisSalaryPay>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();


                //var AppSalary = DB.HisPendingAppSalaries.Where(w => w.InMonth == InMonth.Month && w.InYear == InMonth.Year && w.IsLock == true).ToList();
                //if (AppSalary.Count() > 0)
                //{
                //    return "APPROVE_SALARY";
                //}
                unitOfWork.BulkDelete(ListEmpSalary);
                unitOfWork.BulkDelete(ListEmpSalaryD);
                unitOfWork.BulkDelete(ListEmpOT);
                unitOfWork.BulkDelete(ListEmpReward);
                unitOfWork.BulkDelete(ListEmpDedLeave);
                unitOfWork.BulkDelete(ListEmpPayment);
                unitOfWork.BulkDelete(ListEmpSoSe);
                unitOfWork.BulkDelete(ListEmpSalaryPay);

                //var EmpLoan = DB.HREmpLoans.Where(w => w.PayMonth.Year == InMonth.Year && w.PayMonth.Month == InMonth.Month).ToList();

                //foreach (var read in EmpLoan)
                //{
                //    read.Status = SYDocumentStatus.OPEN.ToString();
                //    DBD.HREmpLoans.Attach(read);
                //    DBD.Entry(read).Property(w => w.Status).IsModified = true;
                //}

                unitOfWork.BulkCommit();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public void Generate_Salary(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster, ref bool IsGenerate)
        {
            ClsBenefit objBeneift = new ClsBenefit();
            try
            {
                DateTime _PStartDate = _filter.FromDate;
                DateTime _PToDate = _filter.ToDate;
                DateTime payFrom = _filter.PayFrom;
                DateTime payTo = _filter.PayTo;
                DateTime ATpayFrom = _filter.ATFromDate;
                DateTime ATpayTo = _filter.ATToDate;
                decimal TempSalary = 0;
                decimal Rate = 0;
                decimal Amount = 0;
                int addDay = 0;
                objBeneift.Payback = 0;
                objBeneift.WorkHour = Convert.ToDecimal(_filter.Parameter.WHOUR);
                decimal prorateWorkedDay = ClsPRParameter.Get_WorkingDay(_filter.Parameter, _PStartDate, _PToDate);
                decimal DayInMonth = ClsPRParameter.Get_WorkingDay_Salary(_filter.Parameter, _PStartDate, _PToDate);
                //var Emp_C = from C in _filter.LstEmpCareer
                //            where ((C.FromDate >= _filter.FromDate && C.FromDate <= _filter.ToDate) || (C.ToDate >= _filter.FromDate && C.ToDate <= _filter.ToDate) ||
                //     (_filter.FromDate >= C.FromDate && _filter.FromDate <= C.ToDate) || (_filter.ToDate >= C.FromDate && _filter.ToDate <= C.ToDate))
                //     && C.EmpCode == _filter.EmpCode
                //     select C;
                //            //select new { C.CompanyCode, C.CareerCode, C.EmpCode, C.EmpType, C.Branch, C.LOCT, C.Division, C.DEPT, C.LINE, C.CATE, C.SECT, C.JobCode, C.LevelCode, C.NewSalary, C.FromDate, C.ToDate, C.resigntype };
                //Emp_C = Emp_C.ToList();
                var Emp_C = _filter.LstEmpCareer.Where(w => w.FromDate.Value().AddDays(-addDay) <= payTo && w.ToDate.Value() >= payFrom
                && w.EmpCode == _filter.EmpCode).OrderBy(x => x.FromDate).ToList();

                if (_FPayrllMaster.ListCareerType.Where(w => w.NotCalSalary == true && Emp_C.Where(x => x.CareerCode == w.Code).Any()).Any())
                {
                    var _lstInAct = Emp_C.Where(w => _FPayrllMaster.ListCareerType.Where(x => w.CareerCode == x.Code).Any()).ToList();
                    Emp_C = Emp_C.Where(w => !_FPayrllMaster.ListCareerType.Where(x => w.CareerCode == x.Code && x.NotCalSalary == true).Any()).ToList();
                    var ResType = _FPayrllMaster.ListResginType.Where(w => w.IsCalSalary == true).ToList();
                    if (!_lstInAct.Where(w => ResType.Where(x => x.Code == w.resigntype).Any()).Any()
                    && Emp_C.Where(w => w.CareerCode != "RENEW").Any())
                    {
                        return;
                    }
                }
                ClsCalculateLeave leave = new ClsCalculateLeave();
                var ML = leave.Calculate(_FPayrllMaster.ListEmpLeaveDed, _FPayrllMaster.ListLeaveType, _filter.EmpCode, payFrom, payTo);
                decimal Balance = 0;
                int C_Career = Emp_C.Count();
                foreach (var emp in Emp_C)
                {
                    ClsBenefit objBeneiftItem = new ClsBenefit();
                    DateTime PFromDate = (emp.FromDate == null || emp.FromDate.Value < payFrom) ? payFrom : emp.FromDate.Value;
                    DateTime PToDate = (emp.ToDate == null || emp.ToDate.Value > payTo) ? payTo : emp.ToDate.Value;
                    objBeneiftItem.PayFrom = PFromDate;
                    objBeneiftItem.PayTo = PToDate;

                    if (PToDate > _filter.ToDate) PToDate = _filter.ToDate;
                    if (PFromDate < _filter.FromDate) PFromDate = _filter.FromDate;
                    Decimal ActualWorkDay = ClsPRParameter.Get_WorkingDay(_filter.Parameter, PFromDate, PToDate);
                    if (PFromDate == _filter.FromDate && PToDate == _filter.ToDate)
                    {
                        Rate = emp.NewSalary / DayInMonth;
                        TempSalary = emp.NewSalary;
                    }
                    else
                    {
                        decimal TMPD = 0;
                        //if (emp.CareerCode == "NEWJOIN" && C_Career == 1 && _filter.Parameter.SALWKTYPE == 3)
                        //{
                        //    ActualWorkDay = DayInMonth - Get_WorkingDay(_filter.Parameter, FromDate, PFromDate);
                        //}
                        TMPD = Convert.ToDecimal(_filter.ListSalaryItem.Sum(x => x.ActWorkDay));
                        if ((TMPD + ActualWorkDay) > DayInMonth) ActualWorkDay = Convert.ToDecimal(DayInMonth) - TMPD;
                        Rate = Convert.ToDecimal(emp.NewSalary / DayInMonth);
                        TempSalary = Math.Round((Rate * ActualWorkDay), 2);
                    }
                    if (leave.IsML)
                    {
                        DateTime MLDate = leave.DateFromML;
                        if (payFrom < MLDate)
                        {
                            MLDate = MLDate.AddDays(-1);
                        }
                        var WorkingDay = ClsPRParameter.Get_WorkingDay(_filter.Parameter, payFrom, MLDate);
                        int totalDays = (payTo - payFrom).Days + 1;
                        Balance = TempSalary * leave.RateDed;
                        if (ML < totalDays) Balance -= (WorkingDay) * (Rate * leave.RateDed);
                        //if (ML < totalDays) Balance -= (totalDays - ML) * (Rate * leave.RateDed);
                    }
                    TempSalary = Math.Round(TempSalary, SYConstant.DECIMAL_PLACE)- Balance;
                    Rate = Convert.ToDecimal(emp.NewSalary / DayInMonth);
                    objBeneiftItem.ActWorkDay = ActualWorkDay;
                    objBeneiftItem.Salary = TempSalary;
                    objBeneiftItem.BaseSalary = emp.NewSalary;
                    objBeneiftItem.WorkHour = objBeneift.WorkHour;
                    var _His_GenD = New_SalaryItem(_filter, emp, objBeneiftItem, prorateWorkedDay, Rate);

                    _filter.ListSalaryItem.Add(_His_GenD);
                }
                //---------------------------------------
                var EmpFamaily = _FPayrllMaster.ListEmpFamily.Where(w => w.EmpCode == _filter.EmpCode).ToList();
                var Except = _FPayrllMaster.PayrollSetting;
                var EMP_GENERATESALARY_C = (from Emp_salary in _filter.ListSalaryItem
                                            group Emp_salary by new { Emp_salary.EmpCode }
                into myGroup
                                            where myGroup.Count() > 0
                                            select new
                                            {
                                                myGroup.Key.EmpCode,
                                                ActWorkDay = myGroup.Sum(w => w.ActWorkDay),
                                                Amount = myGroup.Sum(w => w.Amount),
                                                ActworkHour = myGroup.Sum(w => w.ActWorkHours)
                                            }).ToList();
                var staff = _filter.Staff;
                objBeneift.AdvPay = _FPayrllMaster.ListEmpAdvance.Where(w => w.EmpCode == _filter.EmpCode && w.TranDate.Value.Year == _filter.InYear
                && w.TranDate.Value.Month == _filter.InMonth).Sum(x => x.Amount);
                objBeneift.Loan = _FPayrllMaster.ListEmpLoan.Where(w => w.EmpCode == _filter.EmpCode && w.PayMonth.Year == _filter.InYear
                && w.PayMonth.Month == _filter.InMonth).Sum(x => x.LoanAm);
                var empCardId = _FPayrllMaster.ListEmpIdentity.FirstOrDefault(w => w.EmpCode == _filter.EmpCode && w.IdentityTye == "IDCard");
                if (empCardId != null)
                    objBeneift.IDCard = empCardId.IDCardNo;

                if (_filter.ListSalaryItem.Count > 0)
                {
                    objBeneift.BaseSalary = _filter.ListSalaryItem.OrderBy(x => x.PayTo).FirstOrDefault().BasicSalary;
                }
                foreach (var Emp in EMP_GENERATESALARY_C)
                {
                    objBeneift.Child = EmpFamaily.Where(w => w.Child == true && w.TaxDeduc == true).ToList().Count;
                    objBeneift.Spouse = EmpFamaily.Where(w => w.Spouse == true && w.TaxDeduc == true).ToList().Count;
                    objBeneift.ChildAmount = Convert.ToDecimal(objBeneift.Child * Except.Child.Value);
                    objBeneift.SpouseAmount = Convert.ToDecimal(objBeneift.Spouse * Except.Spouse.Value);

                    if (Emp.Amount - objBeneift.BaseSalary > 0)
                        objBeneift.Increased = Emp.Amount - objBeneift.BaseSalary;
                    else if (_FPayrllMaster.ListCareerType.Where(x => x.Code == staff.CareerDesc).Any() && (Emp.Amount - objBeneift.BaseSalary) > 0)
                        objBeneift.Increased = Emp.Amount - objBeneift.BaseSalary;

                    objBeneift.DayInMonth = DayInMonth;
                    objBeneift.ActWorkDay = Emp.ActWorkDay.Value;
                    objBeneift.Salary = Emp.Amount;
                    var _GenSala = New_Salary(_filter, objBeneift);
                    Amount = Convert.ToDecimal(Emp.Amount);
                    _filter.HeaderSalary = _GenSala;
                }
                if (_filter.ListSalaryItem.Count == 0)
                {
                    if (_filter.Staff.ReSalary.Year == _filter.InYear && _filter.Staff.ReSalary.Month == _filter.InMonth && _filter.Staff.SalaryFlag == true)
                    {
                        var _EmpCareer = _filter.LstEmpCareer.FirstOrDefault(w => _FPayrllMaster.ListCareerType.Where(x => x.Code == w.CareerCode).Any() && w.EmpCode == _filter.EmpCode);
                        var _His_GenD = New_SalaryItem(_filter, _EmpCareer, objBeneift, 0, Rate);
                        var _GenSala = New_Salary(_filter, objBeneift);
                        Amount = 0;
                        _filter.ListSalaryItem.Add(_His_GenD);
                        _filter.HeaderSalary = _GenSala;
                    }
                }
                ClsEmpReward clsEmpReward = new ClsEmpReward();
                _filter.ListHisEmpPayment.Add(clsEmpReward.Emp_Payment(_filter,_FPayrllMaster ,"SALARY", "BS", "Salary", Amount,true, "TX-001"));
                var changeStatus= _FPayrllMaster.ListEmpLoan.Where(s => s.LoanDate.Month == _filter.InMonth && s.LoanDate.Year == _filter.InYear).ToList();
				foreach (var read in changeStatus)
				{
					read.Status = SYDocumentStatus.CLEARED.ToString();
					unitOfWork.Repository<HREmpLoan>().Update(read);
				}
				if (_filter.HisAppSalary == null && IsGenerate == false)
                {
                    IsGenerate = true;
                    var _AppSalary = new HisPendingAppSalary();
                    _AppSalary.CompanyCode = _filter.CompanyCode;
                    _AppSalary.PeriodID = _filter.Period;
                    _AppSalary.FromDate = _filter.FromDate;
                    _AppSalary.ToDate = _filter.ToDate;
                    _AppSalary.IsLock = false;
                    _filter.ApprovalSalary.Add(_AppSalary);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Calculate_Overtime(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            try
            {
                decimal OTAmount = 0;
                decimal WorkHourPerDay = Convert.ToDecimal(_filter.Parameter.WHOUR);
                var EmpOT = _FPayrllMaster.ListEmpOT.Where(w => w.EmpCode == _filter.EmpCode).ToList();//&& w.PayMonth.Value.Year == _filter.InYear && w.PayMonth.Value.Month == _filter.InMonth && w.OTHour > 0).ToList();
                ClsOTCalculate CalOT = new ClsOTCalculate();
                ClsEmpReward clsEmpReward = new ClsEmpReward();
                var Round_ = unitOfWork.Set<SYRoundingRule>().AsQueryable().FirstOrDefault(w => w.EffectiveDate <= _filter.ToDate && w.EndDate >= _filter.ToDate && w.Type == "OT");
                foreach (var OT in EmpOT.OrderBy(w => w.OTDate))
                {
                    decimal TempRate = 0;
                    decimal DailyRate = 0;
                    decimal WorkDayPerMonth = ClsPRParameter.Get_WorkingDay_Salary(_filter.Parameter, _filter.FromDate, _filter.ToDate);
                    if (_filter.LstEmpCareer.Where(w => OT.OTDate >= w.FromDate && OT.OTDate <= w.ToDate).Any())
                    {
                        decimal BaseSalary = Convert.ToDecimal(_filter.LstEmpCareer.FirstOrDefault(w => OT.OTDate >= w.FromDate && OT.OTDate <= w.ToDate).NewSalary);
                        var Result = _FPayrllMaster.ListOTType.FirstOrDefault(w => w.OTCode == OT.OTType);
                        DailyRate = BaseSalary / WorkDayPerMonth;
                        TempRate = CalOT.CalculateOT(Result, DailyRate, WorkHourPerDay);

                        OTAmount = OTAmount + (Convert.ToDecimal(OT.OTHour) * TempRate);
                        var OT_Amount = clsEmpReward.RoundReward(_FPayrllMaster, OTAmount, "OT");

                        var Gen_OT = clsEmpReward.AddEmpOverTime(_filter, Result, OT, TempRate);
                        Gen_OT.BaseSalary = BaseSalary;
                        Gen_OT.WorkDay = WorkDayPerMonth;
                        Gen_OT.WorkHour = WorkHourPerDay;
                        if (clsEmpReward.IncludeIsDaily)
                            Gen_OT.Amount = clsEmpReward.RoundReward(_FPayrllMaster, Gen_OT.Amount ?? 0, "OT");
                       _filter.ListEmpOT.Add(Gen_OT);
                        //if (_filter.LstPayHis.Where(w => w.PayType == "OT" && w.Code == OT.OTType).Any())
                        //{
                        //    _filter.LstPayHis.Where(w => w.PayType == "OT" && w.Code == OT.OTType).ToList().ForEach(x => x.Amount += Convert.ToDecimal(Gen_OT.Amount));
                        //}
                        //else
                        //{
                        //    _filter.LstPayHis.Add(EmpNewHistory(_filter, "D", "OT", OT.OTType, OT.OTDescription.ToUpper(), Convert.ToDecimal(Gen_OT.Amount)));
                        //}
                        if (_filter.ListHisEmpPayment.Where(w => w.IncomeType == "OT" && w.Code == OT.OTType).Any())
                        {
                            _filter.ListHisEmpPayment.Where(w => w.IncomeType == "OT" && w.Code == OT.OTType).ToList().ForEach(x => x.Amount += Convert.ToDecimal(Gen_OT.Amount));
                        }
                        else
                        {
                            _filter.ListHisEmpPayment.Add(clsEmpReward.Emp_Payment(_filter, _FPayrllMaster, "OT", OT.OTType, OT.OTDescription, Gen_OT.Amount.Value, true, "TX-001"));
                        }
                    }
                }
                if (Round_ != null)
                    _filter.HeaderSalary.OTAM = ClsRounding.Rounding(OTAmount, Round_.RoundPlaces, Round_.RoundMethod);
                else
                    _filter.HeaderSalary.OTAM = OTAmount;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Calculate_Allowance(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            ClsEmpReward clsEmpReward = new ClsEmpReward();
            try
            {
                decimal ActualWorkDay = 0;
                decimal exchangeRate = _filter.ExchangeRate.Value;
                ClsPRRewardType rewardType = new ClsPRRewardType();
                var lstAllwType = rewardType.GetRewardsType(_FPayrllMaster.LstRewardsType, RewardTypeCode.ALLW.ToString());
                ClsCalculateReward Cal_Reward = new ClsCalculateReward();

                var listAllowance = _FPayrllMaster.LstEmpAllow.Where(w => w.EmpCode == _filter.EmpCode).ToList();
                foreach (var Allow in listAllowance)
                {
                    decimal Amount = Allow.Amount.Value;
                    var AllwType = lstAllwType.FirstOrDefault(w => w.Code == Allow.AllwCode);
                    if (AllwType == null) continue;
                    //Monthly
                    if (Allow.Status == 1)
                    {
                        ActualWorkDay = _filter.ToDate.Subtract(_filter.FromDate).Days + 1;
                    }
                    else
                    {
                        if (Allow.AllwCode is "SP") continue;
                        ActualWorkDay = ClsPRParameter.Get_WorkingDay_Allw(_filter.Parameter, _filter.FromDate, _filter.ToDate);
                        decimal? ALWperDay = Allow.Amount / ActualWorkDay;
                        Cal_Reward.Validate_Date(_filter, Allow.FromDate.Value, Allow.ToDate.Value);

                        if (Cal_Reward.IsProRate == true)
                        {
                            if (_filter.Staff.StartDate.Value.Date > Cal_Reward.R_FromDate)
                            {
                                Cal_Reward.R_FromDate = _filter.Staff.StartDate.Value;
                            }
                            decimal TempWorkDay = ClsPRParameter.Get_WorkingDay(_filter.Parameter, Cal_Reward.R_FromDate, Cal_Reward.R_ToDate);
                            if (Cal_Reward.R_FromDate == _filter.FromDate && Cal_Reward.R_ToDate == _filter.ToDate)
                            {
                                Amount = Convert.ToDecimal(Allow.Amount);
                            }
                            else
                            {
                                var Temp_Amount = Convert.ToDecimal((Allow.Amount / ActualWorkDay) * TempWorkDay);
                                Amount = clsEmpReward.RoundReward(_FPayrllMaster, Amount, AllwType.IncomeType);
                                //Amount = Convert.ToDecimal((Allow.Amount / ActualWorkDay) * TempWorkDay);
                            }
                        }
                    }
                    var _GenAllw = clsEmpReward.AddEmpReward(_filter, AllwType, Amount, ActualWorkDay);

                    var objIncomeType = _FPayrllMaster.ListIncomeType.FirstOrDefault(w => w.Code == AllwType.IncomeType);
                    if (objIncomeType != null)
                    {
                        _filter.ListHisEmpPayment.Add(
                        clsEmpReward.Emp_Payment(_filter, _FPayrllMaster, objIncomeType.Code, AllwType.Code,
                        AllwType.Description, Amount, true, AllwType.TaxType));
                    }
                    _filter.ListEmpReward.Add(_GenAllw);
                }

                SYHRSetting Pay_Setting = _FPayrllMaster.HRSetting;
                decimal seniorityAmount = 0;
                string[] allowanceType = { "SP" };
                var BaseSalary = _filter.Staff.Salary;

                // seniority allowance
                string empType = string.Empty;
                if (Pay_Setting != null && !Pay_Setting.IsTax.IsNullOrZero() && _filter.ListEmpReward.Count > 0)
                {
                    empType = Pay_Setting.EmpType;
                    List<HisEmpReward> allowances = _filter.ListEmpReward;
                    decimal amountUS = allowances.Where(x =>x.Code == Pay_Setting.SeniorityType)
                    .Sum(x => x.Amount).ToDecimal();
                    decimal amountKH = amountUS * exchangeRate;
                    //if (_filter.HeaderSalary.EmpType == empType)
                    //{
                    //    seniorityAmount += amountUS;
                    //}
                    if (amountKH > Pay_Setting.SeniorityException.ToDecimal() && _filter.HeaderSalary.EmpType == empType)
                    {
                        seniorityAmount = ((amountKH - Pay_Setting.SeniorityException.ToDecimal()) / exchangeRate);
                    }
                }
                _filter.HeaderSalary.SeniorityTaxable = ClsRounding.Rounding(seniorityAmount, SYConstant.DECIMAL_PLACE, "N");


                //#region ********Service Charge********
                //var _ListSVC = unitOfWork.Repository<HISSVCMonthly>().Where(w => w.EmpCode == _filter.EmpCode && w.InYear == _filter.InYear && w.InMonth == _filter.InMonth).ToList();
                //var SVCAm = _ListSVC.Sum(w => w.Amount);
                //if (SVCAm > 0)
                //{
                //    ActualWorkDay = _filter.ToDate.Subtract(_filter.FromDate).Days + 1;
                //    var AllwTypeSVC = _FPayrllMaster.LstRewardsType.Where(w => w.ReCode == "ALLW").ToList();
                //    var _allw = lstAllwType.FirstOrDefault(w => w.Code == "SVC");
                //    if (_allw != null)
                //    {
                //        var _GenAllw = clsEmpReward.AddEmpAllowance(_filter, _allw, SVCAm.Value, ActualWorkDay);

                //        _filter.LstPayHis.Add(EmpNewHistory(_filter, "B", "ALW", _allw.Code, _allw.Description.ToUpper(), Convert.ToDecimal(SVCAm)));
                //        _filter.ListEmpReward.Add(_GenAllw);
                //    }
                //}
                //#endregion

                //#region ********Leave Balance********
                //var Emp_C = from C in _filter.LstEmpCareer
                //            where ((C.FromDate >= _filter.FromDate && C.FromDate <= _filter.ToDate) || (C.ToDate >= _filter.FromDate && C.ToDate <= _filter.ToDate) ||
                //            (_filter.FromDate >= C.FromDate && _filter.FromDate <= C.ToDate) || (_filter.ToDate >= C.FromDate && _filter.ToDate <= C.ToDate))
                //            && C.EmpCode == _filter.EmpCode && C.resigntype != "RES"
                //            select new { C.NewSalary, C.FromDate, C.resigntype };
                //decimal DayInMonth = ClsPRParameter.Get_WorkingDay_Salary(_filter.Parameter, _filter.FromDate, _filter.ToDate);
                //if (_filter.Staff.TerminateStatus != "")
                //{
                //    var _allw = lstAllwType.FirstOrDefault(w => w.Code == "ALPALY");
                //    if (_allw != null)
                //    {
                //        var ALBalance = unitOfWork.Repository<HREmpLeaveB>().FirstOrDefault(w => w.EmpCode == _filter.EmpCode && w.InYear == _filter.InYear && w.InMonth == _filter.InMonth && w.LeaveCode == "AL");
                //        if (ALBalance != null)
                //        {
                //            decimal Balance = Convert.ToDecimal((BaseSalary / DayInMonth) * ALBalance.ALPayBalance);
                //            if (Balance > 0)
                //            {
                //                var _GenAllw = new HISGenAllowance()
                //                {
                //                    PeriodID = _filter.Period,
                //                    CompanyCode = _filter.CompanyCode,
                //                    INYear = _filter.InYear,
                //                    INMonth = _filter.InMonth,
                //                    EmpCode = _filter.EmpCode,
                //                    FromDate = _filter.FromDate,
                //                    ToDate = _filter.ToDate,
                //                    WorkDay = 0,
                //                    RatePerDay = 0,
                //                    AllwCode = "ALPALY",
                //                    AllwDesc = _allw.Description,
                //                    OthDesc = _allw.OthDesc,
                //                    TaxAble = _allw.Tax,
                //                    FringTax = _allw.FTax,
                //                    AllwAm = Balance,
                //                    AllwAmPM = Balance / ActualWorkDay,
                //                    CreateBy = User.UserName,
                //                    CreateOn = DateTime.Now,
                //                    Remark = "Generate From System"
                //                };
                //                _filter.ListEmpReward.Add(_GenAllw);
                //                _filter.LstPayHis.Add(EmpNewHistory(_filter, "B", "ALW", _allw.Code, _allw.Description.ToUpper(), Convert.ToDecimal(Balance)));
                //            }
                //        }
                //    }
                //}
                //#endregion
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Calculate_Bonus(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            ClsEmpReward clsEmpReward = new ClsEmpReward();
            try
            {
                decimal ActualWorkDay = 0;
                ClsPRRewardType rewardType = new ClsPRRewardType();
                ClsCalculateReward Cal_Reward = new ClsCalculateReward();
                var lstAllwType = rewardType.GetRewardsType(_FPayrllMaster.LstRewardsType, RewardTypeCode.BON.ToString());

                var lstEmpBon = _FPayrllMaster.LstEmpBon.Where(w => w.EmpCode == _filter.EmpCode).ToList();

                foreach (var item in lstEmpBon)
                {
                    decimal Amount = item.Amount;
                    var AllwType = lstAllwType.FirstOrDefault(w => w.Code == item.BonCode);
                    if (AllwType == null) continue;
                    if (item.Status == 1)
                    {
                        ActualWorkDay = _filter.ToDate.Subtract(_filter.FromDate).Days + 1;
                    }
                    else
                    {
                        Cal_Reward.Validate_Date(_filter, item.FromDate.Value, item.ToDate.Value);
                        ActualWorkDay = ClsPRParameter.Get_WorkingDay_Allw(_filter.Parameter, _filter.FromDate, _filter.ToDate);
                        if (Cal_Reward.IsProRate == true)
                        {
                            decimal TempWorkDay = ClsPRParameter.Get_WorkingDay(_filter.Parameter, Cal_Reward.R_FromDate, Cal_Reward.R_ToDate);
                            if (Cal_Reward.R_FromDate == _filter.FromDate && Cal_Reward.R_ToDate == _filter.ToDate)
                            {
                                Amount = Convert.ToDecimal(item.Amount);
                            }
                            else
                            {
                                var Temp_Amount = Convert.ToDecimal((item.Amount / ActualWorkDay) * TempWorkDay);
                                Amount = clsEmpReward.RoundReward(_FPayrllMaster, Amount, AllwType.IncomeType);
                                //Amount = Convert.ToDecimal((item.Amount / ActualWorkDay) * TempWorkDay);
                            }
                        }
                    }
                    var _GenAllw = clsEmpReward.AddEmpReward(_filter, AllwType, Amount, 0);
                    _filter.ListEmpReward.Add(_GenAllw);
                    var objIncomeType = _FPayrllMaster.ListIncomeType.FirstOrDefault(w => w.Code == AllwType.IncomeType);
                    if (objIncomeType != null)
                    {
                        _filter.ListHisEmpPayment.Add(
                        clsEmpReward.Emp_Payment(_filter, _FPayrllMaster, objIncomeType.Code, AllwType.Code,
                        AllwType.Description, Amount, true, AllwType.TaxType));
                    }

                }
                var BaseSalary = _filter.Staff.Salary;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Calculate_LeaveDeduct(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            try
            {
                decimal DayRate = 0;
                string Approve = SYDocumentStatus.APPROVED.ToString();
                decimal workHour = _filter.Parameter.WHOUR.Value;
                decimal NoDayInMonth = _filter.HeaderSalary.WorkDay.Value;
                DateTime FromDate = _filter.ATFromDate;
                DateTime ToDate = _filter.ATToDate;

                //DateTime Lfromdate = new DateTime(_filter.FromDate.Year, 1, 1);
                //DateTime Ltodate = _filter.FromDate.AddDays(-1);
                //var Lpolicy= unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
                //if (Lpolicy != null) Lfromdate = Lpolicy.LFromDate;

                ClsCalculateLeave OnLeave = new ClsCalculateLeave();
                var EmpLeave = OnLeave.OnLoadData(_FPayrllMaster.ListEmpLeaveDed, _filter.EmpCode, FromDate, ToDate);
                if (_filter.Staff.Status == "I" && _filter.Staff.DateTerminate.Year == _filter.InYear && _filter.Staff.DateTerminate.Month == _filter.InMonth)
                {
                    EmpLeave = EmpLeave.Where(w => w.LeaveDate < _filter.Staff.DateTerminate).ToList();
                }
                foreach (var Leave in EmpLeave)
                {
                    DayRate = 0;
                    var Emp_C = _filter.LstEmpCareer.FirstOrDefault(C => Leave.LeaveDate >= C.FromDate && Leave.LeaveDate <= C.ToDate);
                    var _Type = _FPayrllMaster.ListLeaveType.FirstOrDefault(w => w.Code == Leave.LeaveCode);
                    DayRate = OnLeave.GetUnitLeaveDeductionAmoun(Leave, _filter, _Type, Emp_C.NewSalary, NoDayInMonth, workHour);
                    decimal LHour = Convert.ToDecimal(Leave.LHour);
                    string Measure = "H";
                    //if (_Type.CUTTYPE == 1) Measure = "D";
                    //if (Measure == "D") LHour = Convert.ToDecimal(Leave.LHour / _filter.Parameter.WHOUR);
                    LHour = Convert.ToDecimal(Leave.LHour / workHour);
                    decimal Amount = LHour * DayRate;
                    ClsEmpReward clsEmpReward = new ClsEmpReward();
                    Amount = clsEmpReward.RoundReward(_FPayrllMaster, Amount, "LD");
                    var _Gen = new HISGenLeaveDeduct()
                    {
                        PeriodID = _filter.Period,
                        CompanyCode = _filter.CompanyCode,
                        INYear = _filter.InYear,
                        INMonth = _filter.InMonth,
                        EmpCode = _filter.EmpCode,
                        LeaveCode = Leave.LeaveCode,
                        LeaveDesc = _Type.Description,
                        LeaveOthDesc = _Type.OthDesc,
                        LeaveDate = Leave.LeaveDate,
                        ForMular = "(" + _Type.Foperand + ")" + _Type.Operator + _Type.Soperand,
                        BaseSalary = Emp_C.NewSalary,
                        WorkDay = NoDayInMonth,
                        WorkHour = _filter.Parameter.WHOUR,
                        Measure = Measure,
                        Qty = LHour,
                        Rate = Math.Round(DayRate, 2),
                        Amount = Amount,
                        CreateBy = User.UserName,
                        CreateOn = DateTime.Now
                    };
                    _filter.ListDedLeave.Add(_Gen);
                    if (_filter.ListHisEmpPayment.Where(w => w.IncomeType == "LD" && w.Code == _Gen.LeaveCode).Any())
                    {
                        _filter.ListHisEmpPayment.Where(w => w.IncomeType == "LD" && w.Code == _Gen.LeaveCode).ToList().ForEach(x => x.Amount -= Convert.ToDecimal(_Gen.Amount));
                    }
                    else
                    {
                        _filter.ListHisEmpPayment.Add(clsEmpReward.Emp_Payment(_filter, _FPayrllMaster, "LD",
                        _Gen.LeaveCode, _Gen.LeaveDesc,- _Gen.Amount.Value, true, "TX-001"));
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Calculate_Deduction(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            ClsEmpReward clsEmpReward = new ClsEmpReward();
            try
            {
                DateTime FromDate = _filter.FromDate;
                DateTime ToDate = _filter.ToDate;
                var _listGenD = new List<HISGenSalaryD>();
                decimal ActualWorkDay = 0;
                ClsPRRewardType rewardType = new ClsPRRewardType();
                ClsCalculateReward Cal_Reward = new ClsCalculateReward();
                var listEmpDed = _FPayrllMaster.ListEmpDedection.Where(w => w.EmpCode == _filter.EmpCode).ToList();
                var LstReward_Type = rewardType.GetRewardsType(_FPayrllMaster.LstRewardsType, RewardTypeCode.DED.ToString());

                foreach (var item in listEmpDed)
                {
                    decimal Amount = item.Amount.Value;
                    var AllwType = LstReward_Type.FirstOrDefault(w => w.Code == item.DedCode);
                    if (AllwType == null) continue;
                    if (item.Status == 1)
                    {
                        ActualWorkDay = _filter.ToDate.Subtract(_filter.FromDate).Days + 1;
                    }
                    else
                    {
                        Cal_Reward.Validate_Date(_filter, item.FromDate.Value, item.ToDate.Value);
                        ActualWorkDay = ClsPRParameter.Get_WorkingDay_Ded(_filter.Parameter, FromDate, ToDate);

                        if (Cal_Reward.IsProRate == true)
                        {
                            decimal TempWorkDay = ClsPRParameter.Get_WorkingDay(_filter.Parameter, Cal_Reward.R_FromDate, Cal_Reward.R_ToDate);
                            if (Cal_Reward.R_FromDate == FromDate && Cal_Reward.R_ToDate == ToDate)
                            {
                                Amount = Convert.ToDecimal(item.Amount);
                            }
                            else
                            {
                                var Temp_Amount = Convert.ToDecimal((item.Amount / ActualWorkDay) * TempWorkDay);
                                Amount = clsEmpReward.RoundReward(_FPayrllMaster, Amount, AllwType.IncomeType);
                                //Amount = Convert.ToDecimal((item.Amount / ActualWorkDay) * TempWorkDay);
                            }
                        }

                    }
                    var _GenDed = clsEmpReward.AddEmpReward(_filter, AllwType, Amount, ActualWorkDay);
                    _filter.ListEmpReward.Add(_GenDed);

                    var objIncomeType = _FPayrllMaster.ListIncomeType.FirstOrDefault(w => w.Code == AllwType.IncomeType);
                    if (objIncomeType != null)
                    {
                        _filter.ListHisEmpPayment.Add(
                        clsEmpReward.Emp_Payment(_filter, _FPayrllMaster, objIncomeType.Code, AllwType.Code,
                        AllwType.Description, -Amount, true, AllwType.TaxType));
                    }

                }

                #region ******** Staff Deposit ********
                //var EmpStaffDep = unitOfWork.Repository<HREmpDeposit>().Where(w => w.EmpCode == _filter.EmpCode && ((FromDate >= w.FromDate && FromDate <= w.ToDate) || (ToDate >= w.FromDate && ToDate <= w.ToDate))).ToList();
                //var EmpStaffDepItem = unitOfWork.Repository<HREmpDepositItem>().Where(w => w.EmpCode == _filter.EmpCode
                //&& w.PayMonth.Month == _filter.InMonth && w.PayMonth.Year == _filter.InYear).ToList();
                //foreach (var Ded in EmpStaffDep)
                //{
                //    decimal Deposit = 0;
                //    Deposit = EmpStaffDepItem.Where(w => w.DepositNum == Ded.DepositNum).Sum(x => x.Deposit);
                //    ActualWorkDay = ClsPRParameter.Get_WorkingDay_Ded(_filter.Parameter, FromDate, ToDate);
                //    var _Ded = LstReward_Type.FirstOrDefault(w => w.Code == Ded.DeductionType);
                //    if (_Ded != null)
                //    {
                //        var _GenDed = new HISGenDeduction()
                //        {
                //            PeriodID = _filter.Period,
                //            CompanyCode = _filter.CompanyCode,
                //            INYear = _filter.InYear,
                //            INMonth = _filter.InMonth,
                //            EmpCode = _filter.EmpCode,
                //            FromDate = FromDate,
                //            ToDate = ToDate,
                //            WorkDay = ActualWorkDay,
                //            RatePerDay = 0,
                //            DedCode = Ded.DeductionType,
                //            DedDesc = _Ded.Description,
                //            OthDesc = _Ded.OthDesc,
                //            TaxAble = _Ded.Tax,
                //            DedAm = Deposit,
                //            DedAMPM = Deposit / ActualWorkDay,
                //            Remark = "Staff Deposit",
                //            CreateBy = User.UserName,
                //            CreateOn = DateTime.Now
                //        };
                //        _filter.ListEmpReward.Add(_GenDed);
                //        _filter.LstPayHis.Add(EmpNewHistory(_filter, "H", "DED", _Ded.Code, _Ded.Description.ToUpper(), -Convert.ToDecimal(Deposit)));
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Calculate_Tax(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster, DateTime FromDate, DateTime ToDate)
        {
            ClsCalculateTax Cal_tax = new ClsCalculateTax();
            ClsEmpReward clsEmpReward = new ClsEmpReward();
            try
            {
                Cal_tax.Calculate_Benefit(_filter, _FPayrllMaster);
                Cal_tax.CalculateSocialSecurity(_filter, _FPayrllMaster);
                Cal_tax.Calculate_Amount_Before_Tax(_filter, _FPayrllMaster);
                string CareerCode = _filter.Staff.CareerDesc;
                //------------------------------------
                var ObjSalary = _filter.HeaderSalary;
                ObjSalary.AMFRINGTAX = 0;
                ObjSalary.FRINGAM = 0;
                ObjSalary.FRINGRATE = 0;
                if (_FPayrllMaster.HRSetting != null)
                    ObjSalary.Seniority = _filter.ListEmpReward.Where(w => w.Code == _FPayrllMaster.HRSetting.SeniorityType).Sum(w => w.Amount);

                var UnTaxAmount = ObjSalary.UTAXALWAM + ObjSalary.UTAXDEDAM + ObjSalary.UTAXBONAM;
                decimal ExchangeRate = Convert.ToDecimal(ObjSalary.ExchRate);
                var TxPayType = ObjSalary.TXPayType;

                ObjSalary.TAXTYPE = 0;

                var AdvPay = _FPayrllMaster.ListEmpAdvance.Where(w => w.EmpCode == _filter.EmpCode && w.TranDate.Value.Year == _filter.InYear && w.TranDate.Value.Month == _filter.InMonth).ToList();
                var AdvpayAm = AdvPay.Sum(w => w.Amount);
                Cal_tax.IsFBEmployee = true;
                if (TxPayType == "C")
                {
                    Cal_tax.IsFBEmployee = false;
                }
                decimal Netpay = Cal_tax.Cal_NetPay(_filter, _FPayrllMaster);
                if (TxPayType == "N")
                {
                    ObjSalary.TAXAM = 0;//Math.Round(Convert.ToDecimal(TaxUSD), SYConstant.DECIMAL_PLACE);
                    ObjSalary.TAXRATE = 0;
                    ObjSalary.TaxKH = 0;
                    ObjSalary.NetWage = (ObjSalary.GrossPay - AdvpayAm - ObjSalary.LOAN - ObjSalary.TAXDEDAM - ObjSalary.UTAXDEDAM) + ObjSalary.UTAXALWAM + ObjSalary.UTAXBONAM;
                }
                ObjSalary.NetWage = Netpay;
                if (Cal_tax.IsFBEmployee)
                {
                    ObjSalary.NetWage -= ObjSalary.FRINGAM;
                }
                if (_filter.Staff.ISNSSF == true)
                {
                    ObjSalary.NetWage -= (ObjSalary.StaffRisk + ObjSalary.StaffHealthCareUSD);
                }
                ObjSalary.CareerCode = CareerCode;
                if (_FPayrllMaster.LstBankFee.Where(w => w.BrankCode == ObjSalary.BankName).Count() > 0)
                {
                    var _bankFee = _FPayrllMaster.LstBankFee.Where(w => w.BrankCode == ObjSalary.BankName).ToList();
                    var ListBabkFee = _bankFee.Where(w => w.FeeFrom <= ObjSalary.NetWage && w.FeeTo >= ObjSalary.NetWage).ToList();
                    foreach (var item in ListBabkFee)
                    {
                        if (item.Type == "Amount")
                        {
                            ObjSalary.BankFee = item.Rate;
                        }
                    }
                }
                if (ObjSalary.FirstPaymentAmount > 0)
                {
                    ObjSalary.NetWage -= ObjSalary.FirstPaymentAmount;
                }
                ObjSalary.NetWage = clsEmpReward.RoundReward(_FPayrllMaster, ObjSalary.NetWage ?? 0, "NP");
                //ObjSalary.NetWage = ClsRounding.RoundNormal(ObjSalary.NetWage ?? 0, 2);
                _filter.ListSalary.Add(ObjSalary);
                var ObjSalaryPay = new HisSalaryPay()
                {
                    CompanyCode = ObjSalary.CompanyCode,
                    PeriodID = ObjSalary.PeriodID,
                    EmpCode = ObjSalary.EmpCode,
                    Salary = ObjSalary.Salary,
                    AllwTax = ObjSalary.TaxALWAM,
                    TaxableIncome = ObjSalary.TotalTaxableIncome,
                    Tax = ObjSalary.TAXAM,
                    BenefitAfterTax = UnTaxAmount,
                    NetPay = ObjSalary.NetWage,
                    DocumentDate = new DateTime(ObjSalary.ToDate.Value.Year, ObjSalary.ToDate.Value.Month, 1)
                };

                _filter.ListHisSalaryPay.Add(ObjSalaryPay);
                if (ObjSalary.FRINGAM.Value > 0 && Cal_tax.IsFBEmployee)
                {
                    _filter.ListHisEmpPayment.Add(clsEmpReward.Emp_Payment(_filter, _FPayrllMaster, "OTD", "FB", "FringeBenefit", -ObjSalary.FRINGAM.Value, true, ""));
                }
                if (ObjSalary.FirstPaymentAmount > 0)
                    _filter.ListHisEmpPayment.Add(clsEmpReward.Emp_Payment(_filter, _FPayrllMaster, "OTD", "FP", "First Payment", -ObjSalary.FirstPaymentAmount, true, ""));
                if (ObjSalary.ADVPay > 0)
                    _filter.ListHisEmpPayment.Add(clsEmpReward.Emp_Payment(_filter, _FPayrllMaster, "OTD", "ADV", "Advance Pay", -ObjSalary.ADVPay.Value, true, ""));
                if (ObjSalary.LOAN > 0)
                    _filter.ListHisEmpPayment.Add(clsEmpReward.Emp_Payment(_filter, _FPayrllMaster, "OTD", "FP", "Employee Loan", -ObjSalary.LOAN.Value, true, ""));
                _filter.ListHisEmpPayment.Add(clsEmpReward.Emp_Payment(_filter, _FPayrllMaster, "NP", "NET", "Net Pay", ObjSalary.NetWage.Value, true, ""));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Commit_PaySlip(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            try
            {
                string SalaryDesc = "";

                ListPaySlip = new List<HISPaySlip>();
                var Gen_salry = _filter.HeaderSalary;
                var _listLeave = _filter.ListDedLeave;
                var _listMaternity = _listLeave.Where(x => x.LeaveCode == "ML").ToList();
                var OverTime_G = _filter.ListEmpOT;
                var Allowance_G = _filter.ListEmpReward.Where(w => w.RewardType == "Allowance").ToList();
                var Bonus_G = _filter.ListEmpReward.Where(w => w.RewardType == "Bonus").ToList();
                var Deduct_G = _filter.ListEmpReward.Where(w => w.RewardType == "Deduction").ToList();

                var LeaveDed_G = _listLeave.Where(x => x.LeaveCode != "ML").ToList();

                int TranLine = 1;
                for (int i = 1; i <= 50; i++)
                {
                    var Gen = new HISPaySlip()
                    {
                        TranLine = i,
                        EmpCode = _filter.EmpCode,
                        PeriodID = _filter.Period,
                        CompanyCode = _filter.CompanyCode,
                        INYear = _filter.InYear,
                        INMonth = _filter.InMonth,
                        EarnDesc = "EARNING",
                        EAmount = 0,
                        DeductDesc = "DEDUCTIONS",
                        DeductAmount = 0,
                    };
                    ListPaySlip.Add(Gen);
                }
                decimal Maternity = 0;
                if (_listMaternity.Count > 0) Maternity = _listMaternity.Sum(x => x.Amount).Value;
                if (Maternity > 0) SalaryDesc = "Basic Pay - Maternity Leave";
                else SalaryDesc = "Basic Pay";
                var Salary = Gen_salry.Salary - ClsRounding.Rounding(Maternity, SYConstant.DECIMAL_PLACE, _filter.Round_UP);
                TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 1);
                ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EAmount = Salary);
                ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EarnDesc = SalaryDesc);

                //-------Allowance----------
                foreach (var item in Allowance_G)
                {
                    TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 1);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EAmount = Math.Round(Convert.ToDecimal(item.Amount), SYConstant.DECIMAL_PLACE));
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EarnDesc = item.Description);
                }
                //---------OverTime-------
                var G_Over = from F in OverTime_G
                             group F by new { F.OTCode, F.OTDesc, F.OTRate } into myGroup
                             select new
                             {
                                 myGroup.Key.OTCode,
                                 OTHour = myGroup.Sum(w => w.OTHour),
                                 myGroup.Key.OTRate,
                                 Amount = myGroup.Sum(w => w.Amount)
                             };
                foreach (var item in G_Over)
                {
                    string Desc = "DAYS";
                    string StrDes = "";
                    var Type = _FPayrllMaster.ListOTType.FirstOrDefault(w => w.OTCode == item.OTCode);
                    if (Type != null)
                    {
                        if (Type.Measure == "H") Desc = "HOURS";
                        StrDes = Type.OTType + "(" + item.OTHour + " " + Desc + ") *" + Math.Round(item.OTRate.Value, 2);
                    }
                    else
                    {
                        Desc = "HOURS";
                        StrDes = "OT Claim";
                    }
                    TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 1);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EAmount = Math.Round(Convert.ToDecimal(item.Amount), SYConstant.DECIMAL_PLACE));
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EarnDesc = StrDes);
                }
                //---------Bonus------------
                foreach (var item in Bonus_G)
                {
                    TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 1);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EAmount = Math.Round(item.Amount.Value, SYConstant.DECIMAL_PLACE));
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EarnDesc = item.Description);
                }
                //---------PayBack------------
                var Payback = Gen_salry.PayBack;
                if (Payback > 0)
                {
                    TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 1);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EAmount = Math.Round(Convert.ToDecimal(Payback), SYConstant.DECIMAL_PLACE));
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EarnDesc = "Pay Back");
                }
                //---------Tax------------
                var Tax_Value = Gen_salry.TAXAM.Value;
                //if (_filter.Staff.TXPayType == "S")
                //{
                TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = Tax_Value);
                ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = "Income Tax");
                // }
                //Fringe Benefit Tax
                var FrinAm = Gen_salry.FRINGAM;
                if (FrinAm > 0)
                {
                    TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = FrinAm);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = "Fringe Benefit Tax");
                }
                //---------Deduction------------
                foreach (var item in Deduct_G)
                {
                    TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = Math.Round(Convert.ToDecimal(item.Amount), SYConstant.DECIMAL_PLACE));
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = item.Description);
                }
                //---------Leave Deduction------------
                var G_Leave = from F in LeaveDed_G
                              group F by new { F.LeaveDesc } into myGroup
                              select new
                              {
                                  myGroup.Key.LeaveDesc,
                                  Amount = myGroup.Sum(w => w.Amount),
                              };
                foreach (var item in G_Leave)
                {
                    var Amount = Math.Round(Convert.ToDecimal(item.Amount), SYConstant.DECIMAL_PLACE);
                    TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = Amount);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = item.LeaveDesc);
                }
                //---------Loan------------
                var Loan_value = Gen_salry.LOAN;
                if (Loan_value > 0)
                {
                    Loan_value = Math.Round(Convert.ToDecimal(Loan_value), SYConstant.DECIMAL_PLACE);
                    TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = Loan_value);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = "Loan");
                }
                //---------Advance------------
                var ADVPay_value = Gen_salry.ADVPay;
                if (ADVPay_value > 0)
                {
                    TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = ADVPay_value);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = "Advance Pay");
                }
                //add it below deduction
                if (_filter.HeaderSalary.StaffPensionFundAmount.HasValue)
                {
                    TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = _filter.HeaderSalary.StaffPensionFundAmount);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = "Employee Pension Fund Contribution");
                }
                //add it below deduction
                if (_filter.HeaderSalary.StaffRisk.Value > 0)
                {
                    TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = _filter.HeaderSalary.StaffRisk);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = "Employee Risk");
                }
                //add it below deduction
                if (_filter.HeaderSalary.StaffHealthCareUSD.Value > 0)
                {
                    TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = _filter.HeaderSalary.StaffHealthCareUSD);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = "Employee Health Care");
                }
                if (_filter.HeaderSalary.FirstPaymentAmount > 0)
                {
                    TranLine = Get_LineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = _filter.HeaderSalary.FirstPaymentAmount);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = "First Payment");
                }
                foreach (var item in ListPaySlip.Where(w => w.EAmount > 0 || w.DeductAmount > 0))
                {
                    _filter.ListPaySlip.Add(item);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }

        public string Commit_PayCostAccount(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {

            var HisSal = _filter.HeaderSalary;
            DateTime DateNoW = DateTime.Now;
            var GLmappings = _FPayrllMaster.ListGLMap;
            int ID = Convert.ToInt32(_filter.Staff.GrpGLAcc);
            var CostCenter = _FPayrllMaster.ListCostCenter.FirstOrDefault(w => w.ID == ID);
            int LineItem = 0;
            var ListReward = ClsFilterGeneral.OnLoadRewardType();
            if (CostCenter != null)
            {
                GLmappings = GLmappings.Where(w => w.ID == CostCenter.ID).ToList();
                if (HisSal != null && GLmappings.Count > 0)
                {
                    LineItem = 0;
                    var GLMap = GLmappings.Where(w => w.BenGrp == "SYS" && w.BenCode != "_TAXC" && !string.IsNullOrEmpty(w.GLCode)).ToList();
                    var ListSyAccount = _FPayrllMaster.ListSyAccount;
                    List<object> ListObjectDictionary = new List<object>();
                    ListObjectDictionary.Add(_filter.HeaderSalary);
                    foreach (var item in GLMap)
                    {
                        if (item.GLCode == null) continue;
                        var objParam = ListSyAccount.FirstOrDefault(w => w.Code == item.BenCode);
                        if (objParam.ObjectName != null && objParam.FieldName != null)
                        {
                            LineItem += 1;
                            var GLBanChar = new HISGLBenCharge();
                            SwapValueBenCharge(GLBanChar, _filter, item, LineItem, objParam.IsPO, objParam.IsCredit);
                            var Amount = ClsInformation.GetFieldValues(objParam.ObjectName, ListObjectDictionary, objParam.FieldName);
                            if (Amount != null)
                            {
                                if (Convert.ToDecimal(Amount) != 0)
                                {
                                    GLBanChar.Amount = Convert.ToDecimal(Amount);
                                    if (item.BenCode == "_TAX" || item.BenCode == "_SPF")
                                        GLBanChar.IsDebitNote = true;
                                    GLBanChar.Description = objParam.Description;
                                    _filter.ListHisGLCharge.Add(GLBanChar);
                                    if (item.BenCode == "_TAX" && GLBanChar.Amount > 0)
                                    {
                                        GLBanChar.Description = objParam.Description;
                                        _filter.ListHisGLCharge.Add(GLBanChar);
                                        if (HisSal.TXPayType == "C")
                                        {
                                            LineItem += 1;
                                            PRGLmapping _TAXC = GLmappings.FirstOrDefault(w => w.BenGrp == "SYS" && w.BenCode == "_TAXC");
                                            var objParam1 = ListSyAccount.FirstOrDefault(w => w.Code == _TAXC.BenCode);
                                            var GLBanChar1 = new HISGLBenCharge();
                                            SwapValueBenCharge(GLBanChar1, _filter, _TAXC, LineItem, objParam.IsPO, objParam1.IsCredit);
                                            if (_TAXC != null)
                                            {
                                                GLBanChar1.GLCode = _TAXC.GLCode;
                                                GLBanChar1.Amount = Convert.ToDecimal(Amount);
                                                GLBanChar1.Description = objParam1.Description;
                                                _filter.ListHisGLCharge.Add(GLBanChar1);
                                            }
                                            else
                                            {
                                                return "INVALID_TAX_Payroll_Tax";
                                            }
                                        }
                                    }
                                    if (item.BenCode == "_CPF" && GLBanChar.Amount > 0)
                                    {
                                        LineItem += 1;
                                        var GLBanChar1 = new HISGLBenCharge();
                                        SwapValueBenCharge(GLBanChar1, _filter, item, LineItem, false, true);
                                        GLBanChar1.Amount = Convert.ToDecimal(Amount);
                                        GLBanChar1.Description = objParam.Description;
                                        _filter.ListHisGLCharge.Add(GLBanChar1);
                                    }
                                }
                            }
                        }
                    }
                }
                ////*************OverTime*************
                LineItem = 0;
                foreach (var item in _filter.ListHisEmpPayment.Where(w => w.IsSalary == true
                //&& w.IncomeType == "OT"
                ).ToList())
                {
                    var ObjMap = GLmappings.FirstOrDefault(w => w.BenGrp == item.IncomeType && w.BenCode == item.Code);
                    if (ObjMap == null || ObjMap.GLCode == null) continue;

                    LineItem += 1;
                    var GLBanChar1 = new HISGLBenCharge();
                    SwapValueBenCharge(GLBanChar1, _filter, ObjMap, LineItem, true);
                    if (GLBanChar1.GLCode == null) continue;
                    GLBanChar1.Description = item.Description;
                    if (item.Amount < 0)
                    {
                        GLBanChar1.IsCredit = true;
                        GLBanChar1.IsDebitNote = true;
                        if (item.Code == "LATE" || item.Code == "EARLY")
                            GLBanChar1.IsDebitNote = null;
                        GLBanChar1.Amount = Math.Abs(item.Amount.Value);
                    }
                    else GLBanChar1.Amount = item.Amount;
                    _filter.ListHisGLCharge.Add(GLBanChar1);
                }

                //foreach (var item in _filter.ListEmpReward)
                //{
                //    var ObjReward = ListReward.FirstOrDefault(w => w.Code == item.RewardType || w.Description == item.RewardType);
                //    if (ObjReward == null) continue;
                //    var ObjMap = GLmappings.FirstOrDefault(w => w.BenGrp == ObjReward.Code && w.BenCode == item.Code);
                //    if (ObjMap == null || ObjMap.GLCode == null) continue;
                //    LineItem += 1;
                //    var GLBanChar1 = new HISGLBenCharge();
                //    SwapValueBenCharge(GLBanChar1, _filter, ObjMap, LineItem, true);
                //    GLBanChar1.Description = item.Description;
                //    if (GLBanChar1.GLCode == null) continue;
                //    GLBanChar1.IsPO = true;
                //    if (ObjReward.Code == "DED")
                //    {
                //        GLBanChar1.IsDebitNote = true;
                //        if (item.Code == "LATE" || item.Code == "EARLY")
                //            GLBanChar1.IsDebitNote = null;
                //    }
                //    GLBanChar1.Amount = item.Amount;
                //    _filter.ListHisGLCharge.Add(GLBanChar1);
                //}
            }
            return SYConstant.OK;
        }
        public void CommitData(ClsSalary _filter)
        {
            try
            {
                unitOfWork.BeginTransaction();
                unitOfWork.BulkInsert(_filter.ListSalary);
                unitOfWork.BulkInsert(_filter.ListSalaryItem);
                unitOfWork.BulkInsert(_filter.ApprovalSalary);
                unitOfWork.BulkInsert(_filter.ListEmpOT);
                unitOfWork.BulkInsert(_filter.ListEmpReward);
                unitOfWork.BulkInsert(_filter.ListDedLeave);
                unitOfWork.BulkInsert(_filter.ListHisEmpPayment);
                unitOfWork.BulkInsert(_filter.ListHisSalaryPay);
                unitOfWork.BulkInsert(_filter.ListEmpSoSe);
                unitOfWork.BulkInsert(_filter.ListHisGLCharge);
                unitOfWork.BulkInsert(_filter.ListPaySlip);
                unitOfWork.BulkCommit();
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public ClsSalary New_Filter(PRPayPeriodItem Period)
        {
            ClsSalary _filter = new ClsSalary();
            _filter.UserName = User.UserName;
            _filter.Period = Period.PeriodID;
            _filter.InYear = Period.EndDate.Year;
            _filter.InMonth = Period.EndDate.Month;
            _filter.HeaderSalary = new HISGenSalary();
            _filter.ApprovalSalary = new List<HisPendingAppSalary>();
            _filter.ListSalary = new List<HISGenSalary>();
            _filter.ListSalaryItem = new List<HISGenSalaryD>();
            _filter.ListEmpOT = new List<HISGenOTHour>();
            _filter.ListEmpReward = new List<HisEmpReward>();
            _filter.ListDedLeave = new List<HISGenLeaveDeduct>();
            _filter.ListPaySlip = new List<HISPaySlip>();
            _filter.ListHisEmpPayment = new List<HisEmpPayment>();
            _filter.ListHisSalaryPay = new List<HisSalaryPay>();
            _filter.ListEmpSoSe = new List<HisEmpSocialSecurity>();
            _filter.ListHisGLCharge = new List<HISGLBenCharge>();
            return _filter;
        }
        public ClsSalary Get_Filter(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            _filter.PayFrom = _filter.FromDate;
            _filter.PayTo = _filter.ToDate;
            _filter.LstEmpCareer = _FPayrllMaster.ListtEmpCareer.Where(w => w.EmpCode == _filter.EmpCode).ToList();
            _filter.SalaryBase = 1;
            _filter.SalarySoSe = 0;
            if (_FPayrllMaster.PayrollSetting.TaxBase == "Yearly")
            {
                _filter.SalaryBase = 12;
            }
            return _filter;
        }
        public HISGenSalary New_Salary(ClsSalary _filter, ClsBenefit objBeneift)
        {
            var _GenSala = new HISGenSalary()
            {
                PeriodID = _filter.Period,
                CompanyCode = _filter.Staff.CompanyCode,
                Status = SYDocumentStatus.OPEN.ToString(),
                TermStatus = _filter.Staff.TerminateStatus,
                TermRemark = _filter.Staff.TerminateRemark,
                TermDate = _filter.Staff.DateTerminate,
                INYear = _filter.InYear,
                INMonth = _filter.InMonth,
                FromDate = _filter.PayFrom,
                ToDate = _filter.PayTo,
                WorkDay = objBeneift.DayInMonth,
                WorkHour = objBeneift.WorkHour,
                ExchRate = _filter.ExchangeRate,
                NSSFRate = _filter.NSSFRate,
                EmpCode = _filter.EmpCode,
                EmpName = _filter.Staff.AllName,
                EmpType = _filter.Staff.EmpType,
                Branch = _filter.Staff.Branch,
                Location = _filter.Staff.LOCT,
                Division = _filter.Staff.Division,
                DEPT = _filter.Staff.DEPT,
                LINE = _filter.Staff.Line,
                CATE = _filter.Staff.CATE,
                Sect = _filter.Staff.SECT,
                POST = _filter.Staff.JobCode,
                JobGrade = _filter.Staff.JobGrade,
                LevelCode = _filter.Staff.LevelCode,
                Sex = _filter.Staff.Sex,
                ICNO = objBeneift.IDCard,
                SOCSO = _filter.Staff.SOCSO,
                BankName = _filter.Staff.BankName,
                BankBranch = _filter.Staff.BankBranch,
                BankAcc = _filter.Staff.BankAcc,
                DateJoin = _filter.Staff.StartDate,
                ActWorkDay = objBeneift.ActWorkDay,
                Salary = objBeneift.Salary,
                ADVPay = objBeneift.AdvPay,
                USERGEN = User.UserName,
                DATEGEN = DateTime.Now,
                PayBack = objBeneift.Payback,
                LOAN = objBeneift.Loan,
                Increased = objBeneift.Increased,
                Child = objBeneift.Child,
                Spouse = objBeneift.Spouse,
                UTAXCH = objBeneift.ChildAmount,
                UTAXSP = objBeneift.SpouseAmount,
                TAXBONAM = 0,
                CostCenter = _filter.Staff.Costcent,
                TXPayType = _filter.Staff.TXPayType,
                NWAM = 0,
                ShiftPay = 0,
                SFT_Salary = _filter.Staff.SalaryTax,
                SFT_AmToBeTax = 0,
                SFT_GrossPay = 0,
                SFT_Tax = 0,
                SFT_TaxRate = 0,
                SFT_NetPay = 0,
                FirstPaymentAmount = 0,
                StaffHealth = 0,
                StaffRisk = 0,
                StaffHealthCareUSD = 0,
            };

            return _GenSala;
        }
        public HISGenSalaryD New_SalaryItem(ClsFilerFPSalary _filter, HREmpCareer _EmpCareer, ClsBenefit objBeneift, decimal DayInMonth
        , decimal Rate)
        {
            var _His_GenD = new HISGenSalaryD()
            {
                PeriodID = _filter.Period,
                CompanyCode = _filter.Staff.CompanyCode,
                INYear = _filter.InYear,
                INMonth = _filter.InMonth,
                FromDate = _filter.FromDate,
                ToDate = _filter.ToDate,
                CareerCode = _EmpCareer.CareerCode,
                WorkDay = DayInMonth,
                WorkHour = objBeneift.WorkHour,
                EmpCode = _EmpCareer.EmpCode,
                EmpType = _EmpCareer.EmpType,
                Branch = _EmpCareer.Branch,
                Location = _EmpCareer.LOCT,
                Division = _EmpCareer.Division,
                DEPT = _EmpCareer.DEPT,
                LINE = _EmpCareer.LINE,
                CATE = _EmpCareer.CATE,
                Sect = _EmpCareer.SECT,
                POST = _EmpCareer.JobCode,
                LevelCode = _EmpCareer.LevelCode,
                PayFrom = objBeneift.PayFrom,
                PayTo = objBeneift.PayTo,
                ActWorkDay = objBeneift.ActWorkDay,
                BasicSalary = objBeneift.BaseSalary,
                Rate = Rate,
                Amount = objBeneift.Salary,
                CreateBy = User.UserName,
                CreateOn = DateTime.Now
            };

            return _His_GenD;
        }
        public int Get_LineTranNo(string EmpCode, int InYear, int InMonth, int Status)
        {
            int Result = 0;
            if (Status == 1)
            {
                var PaySlips = ListPaySlip.Where(w => w.EmpCode == EmpCode && w.INYear == InYear && w.INMonth == InMonth && w.EarnDesc == "EARNING").ToList();
                if (PaySlips == null) Result = 0;
                else
                    Result = Convert.ToInt32(PaySlips.Min(w => w.TranLine));
            }
            else if (Status == 2)
            {
                var PaySlips = ListPaySlip.Where(w => w.EmpCode == EmpCode && w.INYear == InYear && w.INMonth == InMonth && w.DeductDesc == "DEDUCTIONS").ToList();
                if (PaySlips == null) Result = 0;
                else
                    Result = Convert.ToInt32(PaySlips.Min(w => w.TranLine));
            }
            return Result;
        }
        public int Rounding(decimal? Salary)
        {
            int _result = 0;
            if (Salary == 0) return 0;
            int _netPay = Convert.ToInt32(Salary);
            if (_netPay.ToString().Length < 2) return _result;
            int Values = Convert.ToInt32(_netPay.ToString().Substring(_netPay.ToString().Length - 2, 2));
            int _rounding = 100;
            if (Values >= 50)
            {
                int result = _rounding - Values;
                _result = _netPay + result;
            }
            else
            {
                _result = _netPay - Values;
            }
            return _result;
        }
        public void CalculatePensionFund(ClsSalary filter, ClsFilterPayroll _FPayrllMaster, decimal? Salary)
        {
            filter.HeaderSalary.StaffPensionFundRate = 0;
            filter.HeaderSalary.StaffPensionFundAmount = 0;
            filter.HeaderSalary.StaffPensionFundAmountKH = 0;
            filter.HeaderSalary.CompanyPensionFundRate = 0;
            filter.HeaderSalary.CompanyPensionFundAmount = 0;
            filter.HeaderSalary.CompanyPensionFundAmountKH = 0;
            filter.HeaderSalary.SOSEC = 0;
            filter.HeaderSalary.CompHealth = 0;
            filter.HeaderSalary.StaffRiskKH = 0;
            filter.HeaderSalary.StaffRisk = 0;
            filter.HeaderSalary.TotalRisk = 0;
            filter.HeaderSalary.StaffHealthCareUSD = 0;
            filter.HeaderSalary.TotalHealthCare = 0;
            //Get employee service length
            if (Salary.Value > 0)
            {
                EmploymentInfo empInfo = new EmploymentInfo();
                DateTime FromDate = filter.Staff.StartDate.Value;
                DateTime ToDate = new DateTime(filter.ToDate.Year, filter.ToDate.Month, 1);
                double serviceLength = empInfo.GetEmploymentServiceLength(FromDate, filter.ToDate, ServiceLengthType.Month);
                double EmpLength = empInfo.GetEmploymentServiceLength(filter.Staff.DOB.Value, ToDate, ServiceLengthType.Month);

                decimal? maxContribution = 1200000;
                decimal? minContribution = 400000;
                decimal? basicSalary = 0;
                decimal? ExchangeRate = filter.HeaderSalary.NSSFRate;
                //Get Pension Fund Setting
                List<PRPensionFundSetting> lists = _FPayrllMaster.ListPensionFund.ToList();
                var list = lists.Where(x => x.SeviceLenghtFrom <= serviceLength & x.SeviceLenghtTo >= serviceLength).FirstOrDefault();

                var _Setting = _FPayrllMaster.HRSetting;
                if (_Setting != null)
                {
                    maxContribution = _Setting.MaxContribute;
                    minContribution = _Setting.MinContribue;
                }
                if (filter.Staff.ISNSSF == true)
                {
                    decimal? Rate = 0;
                    decimal? _RateUSD = 0;
                    //if (filter.HeaderSalary != null && (filter.Staff != null && filter.Staff.NSSFContributionType == "RHP"))
                    if (filter.HeaderSalary != null && (filter.Staff != null))
                    {
                        basicSalary = Salary;

                        //Convert local currency to foreign currency
                        if ((EmpLength / 12.00) < 60)
                        {

                            basicSalary = basicSalary * ExchangeRate;// filter.HeaderSalary.ExchRate;
                            if (basicSalary > maxContribution) basicSalary = maxContribution;
                            else if (basicSalary < minContribution) basicSalary = minContribution;

                            if (list != null && list.StaffPercentage > 0)
                            {
                                Rate = ((basicSalary * list.StaffPercentage) / 100);
                                filter.HeaderSalary.StaffPensionFundRate = list.StaffPercentage;
                                filter.HeaderSalary.StaffPensionFundAmount = (Rate / ExchangeRate).Value();
                                filter.HeaderSalary.StaffPensionFundAmountKH = Rate;
                            }
                            if (list != null && list.ComPercentage > 0)
                            {
                                Rate = (basicSalary * list.ComPercentage) / 100;
                                filter.HeaderSalary.CompanyPensionFundRate = list.ComPercentage;
                                filter.HeaderSalary.CompanyPensionFundAmount = (Rate / ExchangeRate).Value();
                                filter.HeaderSalary.CompanyPensionFundAmountKH = Rate;
                            }
                            filter.HeaderSalary.AVGGrSOSC = basicSalary;
                            filter.HeaderSalary.CompanyPensionFundAmountKH = Math.Round((decimal)filter.HeaderSalary.CompanyPensionFundAmountKH, 0);
                            filter.HeaderSalary.StaffPensionFundAmountKH = Math.Round((decimal)filter.HeaderSalary.StaffPensionFundAmountKH, 0);
                        }
                    }

                    filter.HeaderSalary.AVGGrSOSC = basicSalary;
                    if (_Setting.StaffRisk.HasValue)
                    {
                        Rate = (decimal)(basicSalary * _Setting.StaffRisk);
                        _RateUSD = (Rate.Value / ExchangeRate).Value();
                        filter.HeaderSalary.StaffRiskKH = Math.Round(Rate.Value, 0);
                        filter.HeaderSalary.StaffRisk = ClsRounding.Rounding(_RateUSD.Value, SYConstant.DECIMAL_PLACE, "N");
                        filter.HeaderSalary.TotalRisk = filter.HeaderSalary.StaffRisk;
                    }
                    if (_Setting.StaffHealthCare.HasValue)
                    {
                        Rate = (decimal)(basicSalary * _Setting.StaffHealthCare);
                        _RateUSD = (Rate.Value / ExchangeRate).Value();
                        filter.HeaderSalary.StaffHealth = Math.Round(Rate.Value, 0);
                        filter.HeaderSalary.StaffHealthCareUSD = ClsRounding.Rounding(_RateUSD.Value, SYConstant.DECIMAL_PLACE, "N");
                        filter.HeaderSalary.TotalHealthCare = filter.HeaderSalary.StaffHealthCareUSD;
                    }
                }
            }
        }
        public void SwapValueBenCharge(HISGLBenCharge GLBanChar, ClsSalary _filter, PRGLmapping item, int LineItem, bool? IsPO, bool? IsCredit = false)
        {
            DateTime DateNoW = DateTime.Now;
            GLBanChar.CompanyCode = _filter.CompanyCode;
            GLBanChar.LineItem = LineItem;
            GLBanChar.CurrencyCode = _filter.Staff.Currency;
            GLBanChar.Branch = _filter.Staff.Branch;
            GLBanChar.CostCenter = item.Branch;
            GLBanChar.Warehouse = item.Warehouse;
            GLBanChar.Project = item.Project;
            GLBanChar.EmpCode = _filter.Staff.EmpCode;
            GLBanChar.InMonth = _filter.InMonth;
            GLBanChar.InYear = _filter.InYear;
            GLBanChar.PeriodID = _filter.Period;
            GLBanChar.PaymentDate = _filter.ToDate;
            GLBanChar.PostPeriod = _filter.ToDate.ToString("MM-yyyy");
            GLBanChar.CreateBy = User.UserName;
            GLBanChar.CreateOn = DateNoW;
            GLBanChar.GRPBen = item.BenGrp;
            GLBanChar.BenCode = item.BenCode;
            GLBanChar.GLCode = item.GLCode;
            GLBanChar.MaterialCode = item.MaterialCode;
            GLBanChar.IsPO = IsPO;

            if (IsCredit != true) GLBanChar.IsCredit = false;
            else
                GLBanChar.IsCredit = IsCredit;
        }
        public int GetLineTranNo(string EmpCode, int InYear, int InMonth, int Status)
        {
            int Result = 0;
            if (Status == 1)
            {
                var PaySlips = ListPaySlip.Where(w => w.EmpCode == EmpCode && w.INYear == InYear && w.INMonth == InMonth && w.EarnDesc == "EARNING").ToList();

                if (PaySlips.Count > 0) Result = Convert.ToInt32(PaySlips.Min(w => w.TranLine));
            }
            else if (Status == 2)
            {
                var PaySlips = ListPaySlip.Where(w => w.EmpCode == EmpCode && w.INYear == InYear && w.INMonth == InMonth && w.DeductDesc == "DEDUCTIONS").ToList();

                if (PaySlips.Count > 0) Result = Convert.ToInt32(PaySlips.Min(w => w.TranLine));
            }
            return Result;
        }
        public Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            ClsFilterJob clsFilter = new ClsFilterJob();

            keyValues.Add("BRANCHES_SELECT", clsFilter.LoadBranch());
            keyValues.Add("BUSINESSUNIT_SELECT", clsFilter.LoadBusinessUnit());
            keyValues.Add("DIVISION_SELECT", clsFilter.LoadDivision());
            keyValues.Add("DEPARTMENT_SELECT", clsFilter.LoadDepartment());
            keyValues.Add("OFFICE_SELECT", clsFilter.LoadOffice());
            keyValues.Add("SECTION_SELECT", clsFilter.LoadSection());
            keyValues.Add("GROUP_SELECT", clsFilter.LoadGroups());
            keyValues.Add("POSITION_SELECT", clsFilter.LoadPosition());
            keyValues.Add("PERIOD_SELECT", clsFilter.LoadPeriod().Where(w => w.IsActive == true));

            return keyValues;
        }

        #region Pay Detail
        public HISGenSalary HeaderSalary { get; set; }
        public HR_STAFF_VIEW STAFF_VIEW { get; set; }
        public List<HISGenSalaryD> ListBasicSalary { get; set; }
        public List<HISGenOTHour> ListOverTime { get; set; }
        public List<HisEmpReward> ListAllowance { get; set; }
        public List<HisEmpReward> ListBonus { get; set; }
        public List<Payroll> ListEmpPayment { get; set; }
        public List<HisEmpReward> ListDeduction { get; set; }
        public List<HisEmpSocialSecurity> ListEmpSoSe { get; set; }
        public List<LeaveDeduction> ListLeaveDed { get; set; }
        public List<ClsGLCharge> ListGLCharge { get; set; }
        public void OnIndexLoadingDetail()
        {
            HeaderSalary = new HISGenSalary();
            Filter = new FTFilerPayroll();
            Filter.InMonth = DateTime.Now;
            var Period = unitOfWork.Repository<PRPayPeriodItem>().Queryable().Where(w => w.IsActive == true).OrderByDescending(w => w.StartDate).ToList();
            if (Period.Count > 0)
            {
                Filter.Period = Period.FirstOrDefault().PeriodID;
            }

            ListBasicSalary = new List<HISGenSalaryD>();
            ListOverTime = new List<HISGenOTHour>();
            ListAllowance = new List<HisEmpReward>();
            ListBonus = new List<HisEmpReward>();
            ListDeduction = new List<HisEmpReward>();
            ListLeaveDed = new List<LeaveDeduction>();
            ListEmpSoSe = new List<HisEmpSocialSecurity>();
            ListEmpPayment = new List<Payroll>();
            ListGLCharge = new List<ClsGLCharge>();
        }
        public void GetDataDetail(string EmpCode, int Period)
        {
            OnLoad();
            STAFF_VIEW = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().FirstOrDefault(w => w.EmpCode == EmpCode);
            HeaderSalary = unitOfWork.Repository<HISGenSalary>().Queryable().FirstOrDefault(w => w.EmpCode == EmpCode && w.PeriodID == Period);
            if (HeaderSalary == null) HeaderSalary = new HISGenSalary();
            ListLeaveDed = new List<LeaveDeduction>();
            ListBasicSalary = unitOfWork.Repository<HISGenSalaryD>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
            ListOverTime = unitOfWork.Repository<HISGenOTHour>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
            var LstEmpReward = unitOfWork.Repository<HisEmpReward>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
            ListAllowance = LstEmpReward.Where(w => w.RewardType == "Allowance").ToList();
            ListBonus = LstEmpReward.Where(w => w.RewardType == "Bonus").ToList();
            ListDeduction = LstEmpReward.Where(w => w.RewardType == "Deduction").ToList();
            ListEmpSoSe = unitOfWork.Repository<HisEmpSocialSecurity>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
            var ListGLChargeTM = unitOfWork.Repository<HISGLBenCharge>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
            foreach (var item in ListGLChargeTM)
            {
                var objGL = new ClsGLCharge();
                SwapValueGL(objGL, item);
                ListGLCharge.Add(objGL);
            }
            //if (ListGLCharge.Count() > 0)
            //{
            //    ListGLCharge = ListGLCharge.OrderBy(w => w.SortKey).ToList();
            //}
            ListEmpPayment = GetEmpPayments(EmpCode, Period);

            //        //BSM.ListCostCharge = DB.HisCostCharges.Where(w => w.EmpCode == EmpCode && w.PeriodID == PayPeriodID).ToList();
            var LeaveDed = unitOfWork.Repository<HISGenLeaveDeduct>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
            var result = (from Leave in LeaveDed
                          group Leave by new { Leave.LeaveCode, Leave.LeaveDesc, Leave.Measure, Leave.Rate }
              into myGroup
                          where myGroup.Count() > 0
                          select new
                          {
                              myGroup.Key.LeaveCode,
                              myGroup.Key.LeaveDesc,
                              FromDate = myGroup.Min(w => w.LeaveDate),
                              ToDate = myGroup.Max(w => w.LeaveDate),
                              DayLeave = myGroup.Sum(w => w.Qty),
                              myGroup.Key.Rate,
                              Amount = myGroup.Sum(w => w.Qty) * myGroup.Key.Rate,
                              Amount_ = myGroup.Sum(w => w.Amount)
                          }).ToList();
            foreach (var item in result)
            {
                decimal amount = item.Amount.Value;
                if (item.LeaveCode == "ML") amount = item.Amount_.Value;
                var res = new LeaveDeduction()
                {
                    LeaveCode = item.LeaveCode,
                    LeaveDescription = item.LeaveDesc,
                    FromDate = item.FromDate.Value,
                    ToDate = item.ToDate.Value,
                    DayLeave = Convert.ToDecimal(item.DayLeave.Value),
                    Rate = Convert.ToDecimal(item.Rate),
                    Amount = Convert.ToDecimal(item.Amount)
                };
                ListLeaveDed.Add(res);
            }
        }
        public Dictionary<string, dynamic> OnDataSelector(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            ClsFilterJob clsFilter = new ClsFilterJob();
            keyValues.Add("PERIOD_SELECT", clsFilter.LoadPeriod());

            return keyValues;
        }
        public List<Payroll> GetEmpPayments(string EmpCode, int PayPeriodID)
        {
            ListEmpPayment = new List<Payroll>();
            var ListIncome = unitOfWork.Repository<PRIncomeType>().Queryable().ToList();
            var List_EmpPayment = unitOfWork.Repository<HisEmpPayment>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == PayPeriodID).ToList();
            foreach (var Income in ListIncome.OrderBy(w => w.InOrder))
            {
                //var lstPayment = List_EmpPayment.Where(w => w.IncomeType == Income.Code && w.Amount != 0).ToList();
                var lstPayment = List_EmpPayment.Where(w => w.IncomeType == Income.Code).ToList();
                if (lstPayment.Count > 0)
                {
                    Payroll payroll = CreateDepartment(Income.Code, Income.IncomeType, "", null, Income.Code);
                    ListEmpPayment.Add(payroll);
                }
                foreach (var item in lstPayment)
                {
                    ListEmpPayment.Add(CreateDepartment(Income.Code, null, item.Description, item.Amount.Value, item.Code));
                }
            }
            return ListEmpPayment;
        }
        public Payroll CreateDepartment(string parentID, string IncomeType, string name, decimal? Amount, string location)
        {
            return new Payroll
            {
                //ID = id,
                IncomeType = IncomeType,
                ParentID = parentID,
                Description = name,
                Amount = Amount,
                PayType = location,
            };
        }
        public void SwapValueGL(ClsGLCharge S, HISGLBenCharge D)
        {
            S.GLCode = D.GLCode;
            S.Description = D.Description;
            if (D.IsCredit == true)
                S.Credit = D.Amount.Value;
            else S.Debit = D.Amount.Value;
        }

        #endregion
    }
    public class Payroll
    {
        public string ParentID { get; set; }
        public string IncomeType { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public bool TaxAble { get; set; }
        public string PayType { get; set; }
    }
    public class ClsGLCharge
    {
        public string GLCode { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
    public class ListUploadPayHis
    {
        public string EmpCode { get; set; }
        public string Company { get; set; }
        public decimal WorkDay { get; set; }
        public decimal Rate { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal UnpaidLeaveD { get; set; }
        public decimal UnpaidLeave { get; set; }
        public decimal OT { get; set; }
        public int Spouse { get; set; }
        public int Child { get; set; }
        public decimal Tax { get; set; }
        public decimal GrossPay { get; set; }
        public decimal Association { get; set; }
        public decimal CompanyPensionFundAmount { get; set; }
        public decimal Loan { get; set; }
        public decimal NetWage { get; set; }
        public decimal Extra_Motivation { get; set; }
        public decimal Monthly_Seniority { get; set; }
        public decimal Semester_Seniority { get; set; }
        public decimal Gasoline_Phone { get; set; }
        public decimal Adjustment { get; set; }
    }
}