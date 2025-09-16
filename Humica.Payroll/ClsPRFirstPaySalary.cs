using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
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
    public class ClsPRFirstPaySalary : IClsPRFirstPaySalary
    {
        protected IUnitOfWork unitOfWork;
        public string ScreenId { get; set; }
        public string EmpID { get; set; }
        public string MessageCode { get; set; }
        public SYUser User = new SYUser();
        public int Progress { get; set; }
        public SYUserBusiness BS { get; set; }
        public FTFilerPayroll Filter { get; set; }
        public HISGenFirstPay HeaderSalaryFP { get; set; }
        public HR_STAFF_VIEW STAFF_VIEW { get; set; }
        public List<HISGenFirstPaySalaryD> ListBasicSalary { get; set; }
        public List<HisGenLeaveDFirstPay> ListHisGenLeaveDFirstPay { get; set; }
        public List<ClsEmpGenerateSalary> ListEmployeeGen { get; set; }
        public List<HisGenOTFirstPay> ListEmpOTFP { get; set; }
        public List<HisEmpRewardFP> ListEmpAllowanceFP { get; set; }

        public List<HisEmpRewardFP> ListEmpBonusFP { get; set; }

        public List<HisEmpRewardFP> ListDeductionFP { get; set; }
        public List<LeaveDeduction> ListLeaveDed { get; set; }
        public List<HISPaySlipFirstPay> ListPaySlip { get; set; }
        public List<HISApproveSalary> ListApproveSalary { get; set; }
        public List<HisPendingAppSalaryFP> ListAppSalaryPending { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public HISApproveSalary HeaderAppSalary { get; set; }
        public List<SYEventLog> ListLog { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        public ClsPRFirstPaySalary()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        private bool IsSalaryChanged(List<HREmpCareer> careers)
        {
            return careers.FindAll(x => !(String.IsNullOrEmpty(x.Increase.ToString()) || x.Increase == 0)).Count > 0;
        }

        public void OnIndexLoading()
        {
            ListEmployeeGen = new List<ClsEmpGenerateSalary>();
            Filter = new FTFilerPayroll();
            var Period = unitOfWork.Set<PRPayPeriodItem>().Where(w => w.IsActive == true).OrderByDescending(w => w.StartDate).ToList();
            if (Period.Count > 0)
            {
                Filter.Period = Period.FirstOrDefault().PeriodID;
            }
        }
        public string OnIndexLoadingFilter()
        {
            var Period = unitOfWork.Set<PRPayPeriodItem>().FirstOrDefault(w => w.PeriodID == Filter.Period);
            if (Period == null)
            {
                return "PERIOD_REQURED";
            }
            List<HRBranch> ListBranch = SYConstant.getBranchDataAccess();
            DateTime date = new DateTime(1900, 1, 1);
            DateTime InMonth = new DateTime(Period.EndDate.Year, Period.EndDate.Month, 16);
            var TerminateDate = Period.StartDate.Date.AddDays(1);

            var _ListEmpGen = new List<ClsEmpGenerateSalary>();
            var _listStaff = unitOfWork.Set<HRStaffProfile>().Where(w => w.IsPayPartial == true &&
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
            (DbFunctions.TruncateTime(w.DateTerminate) == date.Date
            || DbFunctions.TruncateTime(w.DateTerminate) >= TerminateDate))
            ).ToList();

            _listStaff = _listStaff.Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();

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
            //new
            var ListParameter = unitOfWork.Set<PRParameter>().ToList();
            var monthlySalarySetting = unitOfWork.Set<BiMonthlySalarySetting>().ToList();
            foreach (var item in ListParameter)
            {
                DateTime startDate = Period.StartDate;
                var Bi = monthlySalarySetting.FirstOrDefault(w => w.PayrollParameterID == item.Code);
                if (Bi != null)
                {
                    DateTime E_InMonth = new DateTime(Period.EndDate.Year, Period.EndDate.Month, Bi.FirstEndDay ?? 15);
                    var staff1 = ListEmpSalary.Where(x => x.PayParam == item.Code).ToList();
                    staff1 = staff1.Where(w => w.StartDate.Value.Date <= E_InMonth && ((w.DateTerminate.Date == date.Date
                    || w.DateTerminate.AddDays(-1) >= startDate))).ToList();

                    staff1.Where(x => _ListEmpGen.All(y => y.EmpCode != x.EmpCode)).ToList().ForEach(w => _ListEmpGen.Add(w));
                }
            }
            ListEmployeeGen = _ListEmpGen.OrderBy(w => w.EmpCode).ToList();
            return SYConstant.OK;
        }
        public void OnIndexLoadingDetail()
        {
            HeaderSalaryFP = new HISGenFirstPay();
            Filter = new FTFilerPayroll();
            var Period = unitOfWork.Set<PRPayPeriodItem>().OrderByDescending(w => w.StartDate).ToList();
            if (Period.Count > 0)
            {
                Filter.Period = Period.FirstOrDefault().PeriodID;
            }
            ListBasicSalary = new List<HISGenFirstPaySalaryD>();
            ListEmpOTFP = new List<HisGenOTFirstPay>();
            ListEmpAllowanceFP = new List<HisEmpRewardFP>();
            ListEmpBonusFP = new List<HisEmpRewardFP>();
            ListDeductionFP = new List<HisEmpRewardFP>();
            ListLeaveDed = new List<LeaveDeduction>();
            ListPaySlip = new List<HISPaySlipFirstPay>();
        }
        public string GenerateSalaryFirstPay()
        {
            string Result = "";
            try
            {
                var Period = unitOfWork.Repository<PRPayPeriodItem>().FirstOrDefault(w => w.PeriodID == Filter.Period);
                if (Period == null)
                {
                    return "PERIOD_REQURED";
                }
                if (string.IsNullOrEmpty(EmpID))
                {
                    return "EMPCODE_EN";
                }
                var ExchangeRate = unitOfWork.Set<PRBiExchangeRate>().FirstOrDefault(w => w.Period == Period.PeriodID);
                if (ExchangeRate == null)
                {
                    return "EXCHANGERATE";
                }
                var LstAppSalary = unitOfWork.Repository<HisPendingAppSalaryFP>().Queryable().Where(w => w.PeriodID == Period.PeriodID).ToList();
                if (LstAppSalary.Where(w => w.IsLock == true).Count() > 0)
                {
                    return "APPROVE_SALARY";
                }
                //var prFiscalYear = DB.PRFiscalYears.Where(x => x.InMonth.Value == Filter.InMonth).FirstOrDefault();
                //var _listPayHis = DB.HISPaySlipFirstPays.Where(w => w.INYear == Filter.InMonth.Year & w.INMonth == Filter.InMonth.Month).ToList();
                var _lstRewardType = unitOfWork.Set<PR_RewardsType>().ToList();
                var ListBranch = unitOfWork.Set<HRBranch>().ToList();

                string CLEARED = SYDocumentStatus.CLEARED.ToString();
                var _lstCateCode = unitOfWork.Set<HRCareerHistory>().Where(w => w.NotCalSalary == true).ToList();
                bool IsTax = false;
                Filter.ExchangeRate = ExchangeRate.Rate;
                if (Filter.ExchangeRate > 0)
                {
                    string[] Emp = EmpID.Split(';');
                    Progress = Emp.Count();
                    //_lstStaff = _lstStaff.ToList();
                    //_lstEmpCareer = _lstEmpCareer.ToList();
                    _lstRewardType = _lstRewardType.ToList();
                    var ListTaxSetting = unitOfWork.Set<PRTaxSetting>().ToList();
                    int i = 0;
                    var SYData = unitOfWork.Set<SYData>().Where(w => w.DropDownType == "PR_YEAR_SELECT").ToList();
                    if (SYData.Where(w => w.SelectValue == Filter.InMonth.Year.ToString()).Count() == 0)
                    {
                        var _sydata = new SYData()
                        {
                            SelectText = Filter.InMonth.Year.ToString(),
                            SelectValue = Filter.InMonth.Year.ToString(),
                            DropDownType = "PR_YEAR_SELECT",
                            IsActive = true
                        };
                        unitOfWork.Add(_sydata);
                        unitOfWork.Save();
                    }
                    List<string> ListEmpCode = EmpID.Split(';').Where(id => !string.IsNullOrWhiteSpace(id)).ToList();
                    bool IsGenerate = false;
                    ClsFilterPayroll _FPayrllMaster = new ClsFilterPayroll();
                    _FPayrllMaster.OnLoadSetting();
                    _FPayrllMaster.OnLoadDataEmp(ListEmpCode);
                    _FPayrllMaster.OnLoadSettingFP();
                    _FPayrllMaster.OnLoadDataBenefit(ListEmpCode, Period);
                    var error = Delete_Generate(ListEmpCode, Period.PeriodID);
                    if(error !=SYConstant.OK)
                    {
                        return error;
                    }
                    //ClsCalculateLeave OnLeave = new ClsCalculateLeave();
                    //var ListLeave = OnLeave.OnLoadDataLeave(ListEmpCode, Period.StartDate, Period.EndDate);
                    foreach (var Code in Emp)
                    {
                        MessageCode = "";
                        if (Code.Trim() != "")
                        {
                            Result = Code;
                            ClsFPSalary _Filter = new ClsFPSalary();
                            _Filter = New_Filter();
                            _Filter.ExchangeRate = ExchangeRate.Rate;
                            _Filter.Staff = _FPayrllMaster.ListEmployee.FirstOrDefault(w => w.EmpCode == Code);
                            _Filter.Parameter = _FPayrllMaster.ListParameter.FirstOrDefault(w => w.Code == _Filter.Staff.PayParam);
                            if (_Filter.Parameter == null)
                            {
                                continue;
                            }
                            _Filter.BiMonthlySalary = _FPayrllMaster.ListBiMonthlySalary.Where(x => x.PayrollParameterID == _Filter.Parameter.Code).FirstOrDefault();
                            DateTime FromDate = Period.StartDate;
                            DateTime ToDate = Period.EndDate;
                            int num = 15;
                            _Filter.PayFrom = FromDate;
                            if (_Filter.BiMonthlySalary != null && _Filter.BiMonthlySalary.FirstEndDay > 0)
                                num = _Filter.BiMonthlySalary.FirstEndDay.Value;
                            _Filter.PayTo = _Filter.PayFrom.AddDays(num).AddDays(-1);

                            _Filter.LstRewardsType = new List<PR_RewardsType>();
                            _Filter.LstEmpHold = new List<PREmpHold>();
                            _Filter.EmpCode = Code;
                            _Filter.Period = Period.PeriodID;
                            _Filter.InYear = Period.EndDate.Year;
                            _Filter.InMonth = Period.EndDate.Month;
                            _Filter.CompanyCode = _Filter.Staff.CompanyCode;
                            _Filter.FromDate = FromDate;
                            _Filter.ToDate = ToDate;
                            _Filter.ATFromDate = Period.ATStartDate;
                            _Filter.ATToDate = Period.ATEndDate;
                            _Filter.LstEmpCareer = _FPayrllMaster.ListtEmpCareer.Where(w => w.EmpCode == _Filter.Staff.EmpCode).ToList();
                            _Filter.LstRewardsType = _lstRewardType.ToList();
                            var Branch = ListBranch.Where(w => w.Code == _Filter.Staff.Branch).FirstOrDefault();
                            _Filter.HisAppSalary = LstAppSalary.FirstOrDefault(w => w.IsLock == false);
                            //Delete_Generate(_Filter);
                            Generate_Salary(_Filter, _FPayrllMaster, ref IsGenerate);
                            if (_Filter.HisGenFirstPay.EmpCode != null)
                            {
                                if (_Filter.BiMonthlySalary != null)
                                {
                                    if (_Filter.BiMonthlySalary.IsCalOvertime == 1)
                                    {
                                        Calculate_Overtime(_Filter, _FPayrllMaster);
                                    }
                                    if (_Filter.BiMonthlySalary.IsCalAllowan == 1)
                                    {
                                        Calculate_Allowance(_Filter, _FPayrllMaster);
                                    }
                                    if (_Filter.BiMonthlySalary.IsCalBounus == 1)
                                    {
                                        Calculate_Bonus(_Filter, _FPayrllMaster);
                                    }
                                    if (_Filter.BiMonthlySalary.IsCalDeduction == 1)
                                    {
                                        Calculate_Deduction(_Filter, _FPayrllMaster);
                                    }
                                    if (_Filter.BiMonthlySalary.IsCalLeaveDed == 1)
                                    {
                                        Calculate_LeaveDeduction(_Filter, _FPayrllMaster);
                                    }
                                }
                                Calculate_Tax(_Filter, _FPayrllMaster);
                                Commit_PaySlip(_Filter, _FPayrllMaster);
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
                }
                else
                {
                    return "EXCHANGERATE";
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
        public string Delete_Generate(List<string> ListEmpCode, int Period)
        {
            unitOfWork.BeginTransaction();
            try
            {
                var ListEmpSalary = unitOfWork.Repository<HISGenFirstPay>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpSalaryFPD = unitOfWork.Repository<HISGenFirstPaySalaryD>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpOT = unitOfWork.Repository<HisGenOTFirstPay>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpEmpReward = unitOfWork.Repository<HisEmpRewardFP>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpDedLeave = unitOfWork.Repository<HisGenLeaveDFirstPay>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                var ListEmpPaySlip = unitOfWork.Repository<HISPaySlipFirstPay>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period).ToList();
                if (ListEmpSalary.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpSalary);
                }
                // delete salary d
                if (ListEmpSalaryFPD.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpSalaryFPD);
                }
                // delete overtime
                if (ListEmpOT.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpOT);
                }
                // delete ListEmpEmpReward
                if (ListEmpEmpReward.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpEmpReward);
                }
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
                unitOfWork.BulkCommit();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
               return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.DELETE.ToString(), e, true);
                
            }
        }
        public void Generate_Salary(ClsFPSalary _filter, ClsFilterPayroll _FPayrllMaster, ref bool IsGenerate)
        {
            ClsBenefit objBeneift = new ClsBenefit();
            try
            {
                DateTime _PStartDate = _filter.FromDate;
                DateTime _PToDate = _filter.ToDate;
                DateTime payFrom = _filter.PayFrom;
                DateTime payTo = _filter.PayTo;
                decimal TempSalary = 0;
                decimal Rate = 0;
                decimal Amount = 0;
                objBeneift.WorkHour = Convert.ToDecimal(_filter.Parameter.WHOUR);
                DateTime ToDate = _filter.FromDate.AddMonths(1).AddDays(-1);
                decimal DayInMonth = ClsPRParameter.Get_WorkingDay_Salary(_filter.Parameter, _PStartDate, _PToDate);
                decimal halfWorkedDay = DayInMonth / 2;
                objBeneift.BaseSalary = 0;
                int addDay = 0;
                bool HasOldID = false;
                var Emp_C = _filter.LstEmpCareer.Where(w => w.FromDate.Value().AddDays(-addDay) <= payTo && w.ToDate.Value() >= payFrom
                && w.EmpCode == _filter.EmpCode).OrderBy(x => x.FromDate).ToList();

                if (_FPayrllMaster.ListCareerType.Where(w => w.NotCalSalary == true && Emp_C.Where(x => x.CareerCode == w.Code).Any()).Any())
                {
                    var _lstInAct = Emp_C.Where(w => _FPayrllMaster.ListCareerType.Where(x => w.CareerCode == x.Code).Any()).ToList();
                    Emp_C = Emp_C.Where(w => !_FPayrllMaster.ListCareerType.Where(x => w.CareerCode == x.Code && x.NotCalSalary == true).Any()).ToList();
                    var ResType = _FPayrllMaster.ListResginType.Where(w => w.IsCalSalary == true).ToList();
                    if (!_lstInAct.Where(w => ResType.Where(x => x.Code == w.resigntype).Any()).Any())
                    {
                        return;
                    }
                }
                //LeaveDeduction ML
                ClsCalculateLeave leave = new ClsCalculateLeave();
                var ML = leave.Calculate(_FPayrllMaster.ListEmpLeaveDed, _FPayrllMaster.ListLeaveType, _filter.EmpCode, payFrom, payTo);

                decimal Balance = 0;
                foreach (var emp in Emp_C)
                {
                    Rate = TempSalary = Amount = 0;

                    DateTime PFromDate = (emp.FromDate == null || emp.FromDate.Value < payFrom) ? payFrom : emp.FromDate.Value;
                    DateTime PToDate = (emp.ToDate == null || emp.ToDate.Value > payTo) ? payTo : emp.ToDate.Value;
                    decimal prorateWorkedDay = 0;
                    if (HasOldID)
                    {
                        //emp.NewSalary = OldID.Salary;
                        PFromDate = payFrom;
                        PToDate = payTo;
                    }
                    prorateWorkedDay = ClsPRParameter.Get_WorkingDay(_filter.Parameter, payFrom, payTo, PFromDate, PToDate, Emp_C.Count);
                    decimal TMPD = 0;
                    TMPD = Convert.ToDecimal(_filter.ListFirstPayItem.Sum(x => x.ActWorkDay));
                    if ((TMPD + prorateWorkedDay) > halfWorkedDay) prorateWorkedDay = Convert.ToDecimal(halfWorkedDay) - TMPD;


                    if (!IsSalaryChanged(Emp_C) || Emp_C.Count == 1)
                    {
                        Rate = emp.NewSalary / DayInMonth;
                        if (payFrom.Date == PFromDate.Date && PToDate.Date == payTo.Date)
                            TempSalary = emp.NewSalary / 2;
                        else
                            TempSalary = prorateWorkedDay * Rate;
                    }
                    else
                    {
                        Rate = emp.NewSalary / DayInMonth;
                        TempSalary = prorateWorkedDay * Rate;
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
                    TempSalary = Math.Round(TempSalary, SYConstant.DECIMAL_PLACE) - Balance;
                    var _His_GenD = New_FirstPayItem(_filter, emp, prorateWorkedDay, DayInMonth, objBeneift.WorkHour.Value, Rate, TempSalary);

                    _filter.ListFirstPayItem.Add(_His_GenD);
                }
                var EmpFamaily = _FPayrllMaster.ListEmpFamily.Where(w => w.EmpCode == _filter.EmpCode).ToList();
                var Except = _FPayrllMaster.PayrollSetting;
                var EMP_GENERATESALARY_C = (from Emp_salary in _filter.ListFirstPayItem
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
                var ADV = _FPayrllMaster.ListEmpAdvance.Where(w => w.EmpCode == _filter.EmpCode && (w.TranDate >= _filter.FromDate && w.TranDate <= _filter.ToDate)).ToList();
                objBeneift.AdvPay = ADV.Sum(w => w.Amount) / 2;
                objBeneift.Loan = _FPayrllMaster.ListEmpLoan.Where(w => w.EmpCode == _filter.EmpCode && ((w.PayMonth >= payFrom && w.PayMonth <= payTo && w.LoanDate >= payFrom))).ToList().Sum(x => x.LoanAm);
                objBeneift.Loan /= 2;
                objBeneift.Payback = _filter.LstEmpHold.Sum(w => w.Salary / 2);
                var empCardId = _FPayrllMaster.ListEmpIdentity.FirstOrDefault(w => w.EmpCode == _filter.EmpCode);
                if (empCardId != null) objBeneift.IDCard = empCardId.IDCardNo;

                if (_filter.ListFirstPayItem.Count > 0)
                    objBeneift.BaseSalary = _filter.ListFirstPayItem.OrderByDescending(x => x.PayTo).FirstOrDefault().BasicSalary;
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
                    var _GenSala = New_FirstPay(_filter, objBeneift);
                    Amount = Emp.Amount.Value;
                    _filter.HisGenFirstPay = _GenSala;
                }
                if (_filter.ListFirstPayItem.Count == 0)
                {
                    if (_filter.Staff.ReSalary.Year == _filter.InYear && _filter.Staff.ReSalary.Month == _filter.InMonth
                    && _filter.Staff.SalaryFlag == true)
                    {
                        var _EmpCareer = _filter.LstEmpCareer.FirstOrDefault(w => _FPayrllMaster.ListCareerType.Where(x => x.Code == w.CareerCode).Any() && w.EmpCode == _filter.EmpCode);
                        var _His_GenD = New_FirstPayItem(_filter, _EmpCareer, 0, DayInMonth, objBeneift.WorkHour.Value, Rate, TempSalary);
                        var _GenSala = New_FirstPay(_filter, objBeneift);

                        Amount = 0;
                        _filter.ListFirstPayItem.Add(_His_GenD);
                        _filter.HisGenFirstPay = _GenSala;
                    }
                }
                _filter.LstPayHis.Add(new ClsPayHis()
                {
                    EmpCode = _filter.EmpCode,
                    SGROUP = "A",
                    PayType = "BS",
                    Code = "BS",
                    Description = "BASIC SALARY",
                    Amount = Amount
                });
                if (_filter.HisAppSalary == null && IsGenerate == false)
                {
                    IsGenerate = true;
                    var _AppSalary = new HisPendingAppSalaryFP();
                    _AppSalary.CompanyCode = _filter.CompanyCode;
                    _AppSalary.PeriodID = _filter.Period;
                    _AppSalary.FromDate = _filter.FromDate;
                    _AppSalary.ToDate = _filter.ToDate;
                    _AppSalary.IsLock = false;
                    _filter.AppSalaryFP.Add(_AppSalary);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Calculate_Overtime(ClsFPSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            try
            {
                decimal OTAmount = 0;
                decimal WorkHourPerDay = Convert.ToDecimal(_filter.Parameter.WHOUR);
                DateTime ToDate = _filter.FromDate.AddMonths(1).AddDays(-1);
                var EmpOT = _FPayrllMaster.ListEmpOT.Where(w => w.EmpCode == _filter.EmpCode).ToList();//&& w.PayMonth.Value.Year == _filter.InYear && w.PayMonth.Value.Month == _filter.InMonth && w.OTHour > 0
                ClsOTCalculate CalOT = new ClsOTCalculate();
                foreach (var OT in EmpOT.OrderBy(w => w.OTDate))
                {
                    decimal TempRate = 0;
                    decimal DailyRate = 0;
                    decimal WorkDayPerMonth = ClsPRParameter.Get_WorkingDay_Salary(_filter.Parameter, _filter.FromDate, ToDate);
                    if (_filter.LstEmpCareer.Where(w => OT.OTDate >= w.FromDate && OT.OTDate <= w.ToDate.Value()).Any())
                    {
                        decimal BaseSalary = Convert.ToDecimal(_filter.LstEmpCareer.FirstOrDefault(w => OT.OTDate >= w.FromDate && OT.OTDate <= w.ToDate.Value()).NewSalary);
                        var Result = _FPayrllMaster.ListOTType.FirstOrDefault(w => w.OTCode == OT.OTType);
                        DailyRate = BaseSalary / WorkDayPerMonth;
                        TempRate = CalOT.CalculateOT(Result, DailyRate, WorkHourPerDay);

                        OTAmount = OTAmount + (Convert.ToDecimal(OT.OTHour) * TempRate);
                        OTAmount = Math.Round(OTAmount, SYConstant.DECIMAL_PLACE);
                        var Gen_OT = new HisGenOTFirstPay()
                        {
                            PeriodID = _filter.Period,
                            CompanyCode = _filter.CompanyCode,
                            EmpCode = _filter.EmpCode,
                            OTDate = OT.OTDate,
                            BaseSalary = BaseSalary,
                            WorkDay = WorkDayPerMonth,
                            WorkHour = WorkHourPerDay,
                            InYear = _filter.InYear,
                            InMonth = _filter.InMonth,
                            OTHour = OT.OTHour,
                            OTDesc = Result.OTType,
                            OTHDesc = Result.OTHDESC,
                            OTCode = OT.OTType,
                            OTRate = TempRate,
                            OTFormula = "(" + Result.Foperand + ")" + Result.Soperator + Result.Toperand,
                            Measure = Result.Measure,
                            Amount = Math.Round(TempRate * Convert.ToDecimal(OT.OTHour), SYConstant.DECIMAL_PLACE),
                            CreateBy = User.UserName,
                            CreateOn = DateTime.Now
                        };
                        _filter.ListEmpOTFP.Add(Gen_OT);
                    }
                }
                _filter.HisGenFirstPay.OTAM = OTAmount;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Calculate_Allowance(ClsFPSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            ClsEmpReward clsEmpReward = new ClsEmpReward();
            try
            {
                DateTime startDate = _filter.FromDate;
                DateTime endDate = _filter.ToDate;
                DateTime payFrom = _filter.PayFrom;
                DateTime payTo = _filter.PayTo;
                decimal workedDay = ClsPRParameter.Get_WorkingDay_Allw(_filter.Parameter, startDate, endDate);
                ClsPRRewardType rewardType = new ClsPRRewardType();
                var allowances = _FPayrllMaster.LstEmpAllow.Where(w => w.EmpCode == _filter.EmpCode).ToList();
                var allowanceTypes = rewardType.GetRewardsType(_filter.LstRewardsType, RewardTypeCode.ALLW.ToString(), TermCalculation.FIRST.ToString());
                int percetage = 100;
                allowances = allowances.Where(x => allowanceTypes.Where(w => w.Code.Contains(x.AllwCode)).Any()).ToList();
                foreach (var allowance in allowances)
                {
                    decimal Amount = 0;
                    decimal rate = 0;
                    decimal ActualWorkDay = workedDay / 2;

                    DateTime fromDate = (allowance.FromDate.HasValue && allowance.FromDate.Value > startDate) ? allowance.FromDate.Value : startDate;
                    DateTime toDate = (allowance.ToDate.HasValue && allowance.ToDate.Value < endDate) ? allowance.ToDate.Value : endDate;
                    bool isFullMonth = fromDate.Date == startDate.Date && toDate.Date == endDate.Date;
                    rate = (decimal)(allowance.Amount / workedDay);
                    var AllwType = allowanceTypes.FirstOrDefault(w => w.Code == allowance.AllwCode);
                    if (ActualWorkDay != null)
                    {
                        rate = allowance.Amount.Value / workedDay;
                        if (isFullMonth)
                        {
                            Amount = allowance.Amount.ToDecimal();
                        }
                        else if (fromDate.Date == payFrom.Date && toDate.Date == payTo.Date)
                        {
                            ActualWorkDay = workedDay;
                            Amount = allowance.Amount.Value;
                        }
                        else
                        {
                            ActualWorkDay = ClsPRParameter.Get_WorkingDay(_filter.Parameter, fromDate, toDate);
                            Amount = ActualWorkDay * rate;
                        }
                        Amount = (Amount * AllwType.BIPercentageAm) / percetage;
                        var _GenAllw = clsEmpReward.AddEmpRewardFP(_filter, AllwType, Amount, ActualWorkDay);

                        _filter.LstPayHis.Add(new ClsPayHis()
                        {
                            EmpCode = _filter.EmpCode,
                            SGROUP = "B",
                            PayType = "ALW",
                            Code = AllwType.Code,
                            Description = allowance.AllwDescription.ToUpper(),
                            Amount = Convert.ToDecimal(allowance.Amount)
                        });
                        _filter.ListEmpRewardFP.Add(_GenAllw);
                    }
                }

                _filter.HisGenFirstPay.TaxALWAM = _filter.ListEmpRewardFP.Where(w =>w.RewardType== "Allowance" && w.TaxType== "TX-001").Sum(x => x.Amount.Value);
                _filter.HisGenFirstPay.UTaxALWAM = _filter.ListEmpRewardFP.Where(w => w.RewardType == "Allowance" && string.IsNullOrEmpty(w.TaxType)).Sum(x => x.Amount.Value);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Calculate_Bonus(ClsFPSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            ClsEmpReward clsEmpReward = new ClsEmpReward();
            try
            {
                DateTime startDate = _filter.FromDate;
                DateTime endDate = _filter.ToDate;
                DateTime payFrom = _filter.PayFrom;
                DateTime payTo = _filter.PayTo;
                decimal workedDay = ClsPRParameter.Get_WorkingDay_Allw(_filter.Parameter, startDate, endDate);
                ClsPRRewardType rewardType = new ClsPRRewardType();
                #region bonus fix amount
                var bonus = _FPayrllMaster.LstEmpBon.Where(w => w.EmpCode == _filter.EmpCode).ToList();
                var bonusTypes = rewardType.GetRewardsType(_filter.LstRewardsType, RewardTypeCode.BON.ToString(), TermCalculation.FIRST.ToString());
                bonus = bonus.Where(w => bonusTypes.Where(x => x.Code == w.BonCode).Any()).ToList();
                foreach (var bon in bonus)
                {
                    decimal Amount = 0;
                    decimal rate = 0;
                    decimal actualWorkedDay = workedDay / 2;

                    DateTime fromDate = (bon.FromDate.HasValue && bon.FromDate.Value > startDate) ? bon.FromDate.Value : startDate;
                    DateTime toDate = (bon.ToDate.HasValue && bon.ToDate.Value < endDate) ? bon.ToDate.Value : endDate;
                    bool isFullMonth = fromDate.Date == startDate.Date && toDate.Date == endDate.Date;
                    rate = (decimal)(bon.Amount / workedDay);
                    var bonusType = bonusTypes.FirstOrDefault(w => w.Code == bon.BonCode);
                    int percetage = 100;
                    if (bonusType != null)
                    {
                        rate = bon.Amount / workedDay;
                        if (isFullMonth)//(bon.Status == 1 || isFullMonth)
                        {
                            Amount = bon.Amount;
                        }
                        else if (fromDate.Date == payFrom.Date && toDate.Date == payTo.Date)
                        {
                            actualWorkedDay = workedDay;
                            Amount = bon.Amount;
                        }
                        else
                        {
                            actualWorkedDay = ClsPRParameter.Get_WorkingDay(_filter.Parameter, fromDate, toDate);
                            Amount = actualWorkedDay * rate;
                        }
                        Amount = (Amount * bonusType.BIPercentageAm) / percetage;
                        var _GenAllw = clsEmpReward.AddEmpRewardFP(_filter, bonusType, Amount, 0);
                        _filter.ListEmpRewardFP.Add(_GenAllw);
                        _filter.LstPayHis.Add(new ClsPayHis()
                        {
                            EmpCode = _filter.EmpCode,
                            SGROUP = "B",
                            PayType = "ALW",
                            Code = bonusType.Code,
                            Description = bon.BonDescription.ToUpper(),
                            Amount = Convert.ToDecimal(bon.Amount)
                        });
                    }
                }
                _filter.HisGenFirstPay.TaxBONAM = _filter.ListEmpRewardFP.Where(w => w.RewardType == "Bonus" && w.TaxType == "TX-001").Sum(x => x.Amount.Value);
                _filter.HisGenFirstPay.UTaxBONAM = _filter.ListEmpRewardFP.Where(w => w.RewardType == "Bonus" && string.IsNullOrEmpty(w.TaxType)).Sum(x => x.Amount);

                #endregion
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Calculate_LeaveDeduction(ClsFPSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            try
            {
                decimal DayRate = 0;
                string Approve = SYDocumentStatus.APPROVED.ToString();
                decimal workHour = _filter.HisGenFirstPay.WorkHour.Value;
                decimal NoDayInMonth = _filter.HisGenFirstPay.WorkDay.Value;
                DateTime FromDate = _filter.ATFromDate;
                DateTime ToDate = _filter.ATToDate;
                //DateTime FromDate = _filter.HisGenFirstPay.PayFrom.Value;
                //DateTime ToDate = _filter.HisGenFirstPay.PayTo.Value;
                decimal totalDeductAmount = 0;
                decimal actWorkedDay = _filter.HisGenFirstPay.ActWorkDay.Value;
                ClsCalculateLeave OnLeave = new ClsCalculateLeave();
                var EmpLeave = OnLeave.OnLoadData(_FPayrllMaster.ListEmpLeaveDed, _filter.EmpCode, FromDate, ToDate);
                int i = 1;
                Dictionary<string, string> ltype = new Dictionary<string, string>();

                foreach (var Leave in EmpLeave)
                {
                    DayRate = 0;
                    var Emp_C = _filter.LstEmpCareer.FirstOrDefault(C => Leave.LeaveDate >= C.FromDate && Leave.LeaveDate <= C.ToDate.Value());
                    var _Type = _FPayrllMaster.ListLeaveType.FirstOrDefault(w => w.Code == Leave.LeaveCode);
                    DayRate = OnLeave.GetUnitLeaveDeductionAmounFP(Leave, _filter, _Type, Emp_C.NewSalary, NoDayInMonth, workHour);
                    decimal LHour = Convert.ToDecimal(Leave.LHour);
                    string Measure = "H";
                    if (_Type.CUTTYPE == 1) Measure = "D";
                    if (Measure == "D") LHour = Convert.ToDecimal(Leave.LHour / _filter.Parameter.WHOUR);
                    decimal? Amount = LHour * DayRate;
                    if (!ltype.ContainsKey(_Type.Code))
                    {
                        ltype.Add(_Type.Code, _Type.Code);
                        i = 1;
                    }
                    if (actWorkedDay < i)
                    {
                        DayRate = 0;
                        Amount = 0;
                    }
                    var _Gen = new HisGenLeaveDFirstPay()
                    {
                        CompanyCode = _filter.CompanyCode,
                        PeriodID = _filter.Period,
                        InYear = _filter.InYear,
                        InMonth = _filter.InMonth,
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
                    totalDeductAmount += Convert.ToDecimal(Amount);
                    _filter.ListDedLeaveFP.Add(_Gen);
                }
                _filter.HisGenFirstPay.LeaveDeduct = Math.Round(totalDeductAmount, SYConstant.DECIMAL_PLACE, MidpointRounding.AwayFromZero);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Calculate_Deduction(ClsFPSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            ClsEmpReward clsEmpReward = new ClsEmpReward();
            try
            {
                DateTime startDate = _filter.FromDate;
                DateTime endDate = _filter.ToDate;
                DateTime payFrom = _filter.PayFrom;
                DateTime payTo = _filter.PayTo;
                decimal workedDay = ClsPRParameter.Get_WorkingDay_Ded(_filter.Parameter, startDate, endDate);
                ClsPRRewardType rewardType = new ClsPRRewardType();
                var Reward_Type = rewardType.GetRewardsType(_filter.LstRewardsType, RewardTypeCode.DED.ToString(), TermCalculation.FIRST.ToString());

                var _lstDedection = _FPayrllMaster.ListEmpDedection.Where(w => w.EmpCode == _filter.EmpCode).ToList();
                _lstDedection = _lstDedection.Where(w => Reward_Type.Where(x => x.Code == w.DedCode).Any()).ToList();
                foreach (var deduction in _lstDedection)
                {
                    decimal ActualWorkDay = 0;
                    decimal Amount = 0;
                    decimal rate = 0;
                    ActualWorkDay = workedDay / 2;
                    DateTime fromDate = (deduction.FromDate.HasValue && deduction.FromDate.Value > startDate) ? deduction.FromDate.Value : startDate;
                    DateTime toDate = (deduction.ToDate.HasValue && deduction.ToDate.Value < endDate) ? deduction.ToDate.Value : endDate;
                    bool isFullMonth = fromDate.Date == startDate.Date && toDate.Date == endDate.Date;
                    rate = (decimal)(deduction.Amount / workedDay);
                    var deductType = Reward_Type.FirstOrDefault(w => w.Code == deduction.DedCode);
                    int percetage = 100;
                    if (deductType != null)
                    {
                        rate = deduction.Amount.Value / workedDay;
                        if (deduction.Status == 1 || isFullMonth)
                        {
                            Amount = deduction.Amount.ToDecimal();
                        }
                        else if (fromDate.Date == payFrom.Date && toDate.Date == endDate.Date)
                        {
                            ActualWorkDay = workedDay;
                            Amount = deduction.Amount.Value;
                        }
                        else
                        {
                            ActualWorkDay = ClsPRParameter.Get_WorkingDay(_filter.Parameter, fromDate, toDate);
                            Amount = ActualWorkDay * rate;
                        }
                        Amount = (Amount * deductType.BIPercentageAm) / percetage;

                        var _GenDed = clsEmpReward.AddEmpRewardFP(_filter, deductType, Amount, ActualWorkDay);
                        _filter.ListEmpRewardFP.Add(_GenDed);
                    }
                }
                _filter.HisGenFirstPay.TaxDEDAM = _filter.ListEmpRewardFP.Where(w => w.RewardType == "Deduction" && w.TaxType == "TX-001").Sum(x => x.Amount);
                _filter.HisGenFirstPay.UTaxDEDAM = _filter.ListEmpRewardFP.Where(w => w.RewardType == "Deduction" && string.IsNullOrEmpty(w.TaxType)).Sum(x => x.Amount);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Calculate_Tax(ClsFPSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            ClsEmpReward clsEmpReward = new ClsEmpReward();
            try
            {
                string CareerCode = _filter.Staff.CareerDesc;
                decimal NetAmount = 0;
                decimal GrossPay = 0;
                decimal AmountToBeTax = 0;
                //decimal AdvPayAmout = 0;
                decimal TaxKH = 0;
                decimal TaxUSD = 0;
                decimal TaxRate = 0;
                decimal TaxExemptionAmount = 0;
                decimal salary = 0;
                decimal AMFRINGTAX = 0;

                ////foreach (var read in ADV.Where(w => w.IsBIMonthly == true))
                ////{
                //AdvPayAmout = ADV.Sum(w => w.Amount.Value) / 2;
                ////}
                var hisGenSalary = _filter.HisGenFirstPay;
                decimal ExchangeRate = Convert.ToDecimal(hisGenSalary.ExchRate);
                decimal RealAmountTobeTax = 0;
                if (hisGenSalary.Salary == null) hisGenSalary.Salary = 0;
                if (hisGenSalary.OTAM == null) hisGenSalary.OTAM = 0;
                if (hisGenSalary.PayBack == null) hisGenSalary.PayBack = 0;
                if (hisGenSalary.LeaveDeduct == null) hisGenSalary.LeaveDeduct = 0;
                AMFRINGTAX = _filter.ListEmpRewardFP.Where(w => w.TaxType == "TX-002").Sum(x => x.Amount.Value);
                //hisGenSalary.AMFRINGTAX = 0;
                //hisGenSalary.FRINGRATE = 0;
                //hisGenSalary.FRINGAM = 0;
                salary = hisGenSalary.Salary.ToDecimal();
                GrossPay = salary +
                hisGenSalary.OTAM.ToDecimal() +
                hisGenSalary.PayBack.ToDecimal() +
                hisGenSalary.TaxALWAM.ToDecimal() +
                hisGenSalary.UTaxALWAM.ToDecimal() +
                hisGenSalary.TaxBONAM.ToDecimal() +
                hisGenSalary.UTaxBONAM.ToDecimal() -
                hisGenSalary.LeaveDeduct.ToDecimal();
                GrossPay = GrossPay;
                TaxExemptionAmount = (hisGenSalary.UTAXSP.Value / hisGenSalary.ExchRate.Value + hisGenSalary.UTAXCH.Value / hisGenSalary.ExchRate.Value);
                NetAmount = GrossPay -
                (hisGenSalary.ADVPay.ToDecimal() +
                hisGenSalary.LOAN.ToDecimal() +
                hisGenSalary.TaxDEDAM.ToDecimal() +
                hisGenSalary.UTaxDEDAM.ToDecimal()
                );
                if (hisGenSalary.UTAXSP == null) hisGenSalary.UTAXSP = 0;
                if (hisGenSalary.UTAXCH == null) hisGenSalary.UTAXCH = 0;
                if (_filter.BiMonthlySalary != null && _filter.BiMonthlySalary.IsAccrualTax == 1)
                {
                    var TxPayType = hisGenSalary.TXPayType;
                    AmountToBeTax = salary +
                    hisGenSalary.OTAM.Value +
                    hisGenSalary.PayBack.ToDecimal() +
                    hisGenSalary.TaxALWAM.ToDecimal() +
                    hisGenSalary.TaxBONAM.ToDecimal() -
                    (hisGenSalary.LeaveDeduct.ToDecimal() + hisGenSalary.TaxDEDAM.ToDecimal()) -
                    TaxExemptionAmount;
                    AmountToBeTax = GrossPay - (hisGenSalary.UTAXSP.Value / hisGenSalary.ExchRate.Value + hisGenSalary.UTAXCH.Value / hisGenSalary.ExchRate.Value);
                    //AmountToBeTax = Math.Round(AmountToBeTax, SYConstant.DECIMAL_PLACE);
                    RealAmountTobeTax = AmountToBeTax * ExchangeRate;
                    if (RealAmountTobeTax < 0)
                    {
                        RealAmountTobeTax = 0;
                        AmountToBeTax = 0;
                    }

                    var ListTax = _FPayrllMaster.ListTaxSetting;
                    if (_filter.Staff.IsResident == false)
                    {
                        TaxRate = 20;
                        TaxUSD = ((RealAmountTobeTax * TaxRate) / 100) / ExchangeRate;
                    }
                    else
                    {
                        var TaxSetting = ListTax.Where(w => w.TaxFrom <= RealAmountTobeTax && w.TaxTo >= RealAmountTobeTax);
                        foreach (var tax in TaxSetting)
                        {
                            TaxKH = (RealAmountTobeTax * tax.TaxPercent.Value / 100) - tax.Amdeduct.Value;
                            TaxUSD = Math.Round(TaxKH / ExchangeRate, SYConstant.DECIMAL_PLACE);
                            TaxRate = tax.TaxPercent.Value;
                        }
                    }

                    if (SYConstant.DECIMAL_PLACE == 0) TaxKH = Math.Round(TaxKH, SYConstant.DECIMAL_PLACE);
                    if (TxPayType != "C")
                    {
                        NetAmount -= TaxUSD;
                    }

                }
                if (_FPayrllMaster.LstBankFee.Where(w => w.BrankCode == hisGenSalary.BankName).Count() > 0)
                {
                    var _bankFee = _FPayrllMaster.LstBankFee.Where(w => w.BrankCode == hisGenSalary.BankName).ToList();
                    var ListBabkFee = _bankFee.Where(w => w.FeeFrom <= hisGenSalary.NetWage && w.FeeTo >= hisGenSalary.NetWage).ToList();
                    foreach (var item in ListBabkFee)
                    {
                        if (item.Type == "Amount")
                        {
                            hisGenSalary.BankFee = item.Rate;
                        }
                    }
                }
                if (AMFRINGTAX > 0)
                {
                    //decimal AmFringRate = 0.2M;
                    //hisGenSalary.AMFRINGTAX = AMFRINGTAX;
                    //hisGenSalary.FRINGRATE = 20;
                    //hisGenSalary.FRINGAM = Math.Round(AMFRINGTAX * AmFringRate, SYConstant.DECIMAL_PLACE);
                    //NetAmount += Convert.ToDecimal((AMFRINGTAX - hisGenSalary.FRINGAM));
                }
                NetAmount = clsEmpReward.RoundReward(_FPayrllMaster, NetAmount, "NETF");
                //if (_filter.Default_Curremcy == "KH" && _filter.Round_UP == "YES") NetAmount = Rounding(NetAmount);

                hisGenSalary.TAXTYPE = 0;
                hisGenSalary.AMTOBETAX = AmountToBeTax;
                hisGenSalary.AmountKH = RealAmountTobeTax;
                hisGenSalary.TaxAmountUSD = TaxUSD;
                hisGenSalary.TaxAmountKH = TaxKH;
                hisGenSalary.GrossPay = GrossPay;
                hisGenSalary.NetWage = NetAmount;
                hisGenSalary.CareerCode = CareerCode;

                _filter.ListFirstPay.Add(hisGenSalary);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }

        public void CommitData(ClsFPSalary _filter)
        {
            try
            {
                unitOfWork.BeginTransaction();
                unitOfWork.BulkInsert(_filter.ListFirstPayItem);
                unitOfWork.BulkInsert(_filter.AppSalaryFP);
                unitOfWork.BulkInsert(_filter.ListEmpOTFP);
                unitOfWork.BulkInsert(_filter.ListEmpRewardFP);
                unitOfWork.BulkInsert(_filter.ListDedLeaveFP);
                unitOfWork.BulkInsert(_filter.ListFirstPay);
                unitOfWork.BulkInsert(_filter.ListPaySlip);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public void Commit_PaySlip(ClsFPSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            try
            {
                string SalaryDesc = "";

                ListPaySlip = new List<HISPaySlipFirstPay>();
                var Gen_salry = _filter.HisGenFirstPay;
                var _listLeave = _filter.ListDedLeaveFP;
                var _listMaternity = _listLeave.Where(x => x.LeaveCode == "ML").ToList();
                var OverTime_G = _filter.ListEmpOTFP;
                var allowances = _filter.ListEmpRewardFP.Where(w=>w.RewardType== "Allowance").ToList();
                var bonus = _filter.ListEmpRewardFP.Where(w => w.RewardType == "Bonus").ToList();
                var deductions = _filter.ListEmpRewardFP.Where(w => w.RewardType == "Deduction").ToList();
                var LeaveDed_G = _listLeave.Where(x => x.LeaveCode != "ML").ToList();
                DateTime payFrom = _filter.PayFrom;
                DateTime payTo = _filter.PayTo;
                int TranLine = 1;
                for (int i = 1; i <= 13; i++)
                {
                    var Gen = new HISPaySlipFirstPay()
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
                else SalaryDesc = "Basic First Pay";
                var Salary = Gen_salry.Salary - Math.Round(Maternity, SYConstant.DECIMAL_PLACE);
                TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 1);
                ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EAmount = Salary);
                ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EarnDesc = SalaryDesc);
                //-------Allowance----------
                foreach (var item in allowances)
                {
                    TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 1);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EAmount = Math.Round(Convert.ToDecimal(item.Description), SYConstant.DECIMAL_PLACE));
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
                    TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 1);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EAmount = Math.Round(Convert.ToDecimal(item.Amount), SYConstant.DECIMAL_PLACE));
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EarnDesc = StrDes);
                }
                //---------Bonus------------
                foreach (var item in bonus)
                {
                    TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 1);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EAmount = Math.Round(item.Amount.Value, SYConstant.DECIMAL_PLACE));
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EarnDesc = item.Description);
                }
                //---------PayBack------------
                var Payback = Gen_salry.PayBack;
                if (Payback > 0)
                {
                    TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 1);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EAmount = Math.Round(Convert.ToDecimal(Payback), SYConstant.DECIMAL_PLACE));
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EarnDesc = "Pay Back");
                }

                //---------Deduction------------
                foreach (var item in deductions)
                {
                    TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
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
                    TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = Amount);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = item.LeaveDesc);
                }
                //---------Loan------------
                var Loan_value = Gen_salry.LOAN;
                if (Loan_value > 0)
                {
                    Loan_value = Math.Round(Convert.ToDecimal(Loan_value), SYConstant.DECIMAL_PLACE);
                    TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = Loan_value);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = "Loan");
                }
                //---------Advance------------
                var ADVPay_value = Gen_salry.ADVPay;
                if (ADVPay_value > 0)
                {
                    TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = ADVPay_value);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = "Advance Pay");
                }
                _filter.ListPaySlip.AddRange(ListPaySlip);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) MessageCode = ex.InnerException.Message;
                else MessageCode = ex.Message;
            }
        }
        public string Delete_GenerateAll(int PeriodID)
        {
            OnLoad();
            unitOfWork.BeginTransaction();

            var Period = unitOfWork.Set<PRPayPeriodItem>().FirstOrDefault(w => w.PeriodID == PeriodID);
            if (Period == null)
            {
                return "PERIOD_REQURED";
            }
            List<HRBranch> ListBranch = SYConstant.getBranchDataAccess();
            var AppSalry = unitOfWork.Set<HisPendingAppSalaryFP>().Where(w => w.PeriodID == Period.PeriodID && w.IsLock == true).ToList();
            if (AppSalry.Count() > 0)
            {
                return "APPROVE_SALARY";
            }
            var ListEmpSalaryFP = unitOfWork.Set<HISGenFirstPay>().Where(w => w.PeriodID == Period.PeriodID).ToList();
            ListEmpSalaryFP = ListEmpSalaryFP.Where(x => ListBranch.Where(w => w.Code == x.Branch).Any()).ToList();
            var empCodes = ListEmpSalaryFP.Select(x => x.EmpCode).ToList();
            try
            {
                var ListEmpSalaryFPD = unitOfWork.Set<HISGenFirstPaySalaryD>().Where(w => w.PeriodID == Period.PeriodID
                && empCodes.Contains(w.EmpCode)).ToList();
                if (ListEmpSalaryFPD.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpSalaryFPD);
                }
                if (ListEmpSalaryFP.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpSalaryFP);
                }
                var ListEmpOT = unitOfWork.Set<HisGenOTFirstPay>().Where(w => w.PeriodID == Period.PeriodID
                && empCodes.Contains(w.EmpCode)).ToList();
                if (ListEmpOT.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpOT);
                }
                // delete Reward
                var ListEmpEmpReward = unitOfWork.Repository<HisEmpRewardFP>().Queryable().Where(w => w.PeriodID == Period.PeriodID
                && empCodes.Contains(w.EmpCode)).ToList();
                if (ListEmpEmpReward.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpEmpReward);
                }
                var ListEmpDedLeave = unitOfWork.Set<HisGenLeaveDFirstPay>().Where(w => w.PeriodID == Period.PeriodID
                && empCodes.Contains(w.EmpCode)).ToList();
                if (ListEmpDedLeave.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpDedLeave);
                }
                var ListEmpPaySlip = unitOfWork.Set<HISPaySlipFirstPay>().Where(w => w.PeriodID == Period.PeriodID
                && empCodes.Contains(w.EmpCode)).ToList();
                if (ListEmpPaySlip.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpPaySlip);
                }
                //var His_TransferToMgrs = DB.HisTransferToMgrs.Where(w => w.InYear == PayrollMonth.Year && w.InMonth == PayrollMonth.Month).ToList();
                //foreach (var item in His_TransferToMgrs)
                //{
                //    DBD.HisTransferToMgrs.Attach(item);
                //    DBD.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                //}
                var EmpLoan = unitOfWork.Set<HREmpLoan>().Where(w => w.PayMonth.Year == Period.EndDate.Year
                && w.PayMonth.Month == Period.EndDate.Month && empCodes.Contains(w.EmpCode)).ToList();
                List<HREmpLoan> ListEmpLoan = new List<HREmpLoan>();
                foreach (var read in EmpLoan)
                {
                    read.Status = SYDocumentStatus.OPEN.ToString();
                    ListEmpLoan.Add(read);
                }
                if (ListEmpLoan.Count > 0)
                {
                    unitOfWork.BulkUpdate(ListEmpLoan);
                }
                unitOfWork.BulkCommit();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.DELETE.ToString(), e, true);
            }
        }
        public void Delete_Generate(ClsFPSalary _filter)
        {
            unitOfWork.BeginTransaction();
            try
            {
                var ListEmpSalary = unitOfWork.Set<HISGenFirstPay>().Where(w => w.EmpCode == _filter.EmpCode && w.PeriodID == _filter.Period).ToList();
                var ListEmpSalaryFPD = unitOfWork.Set<HISGenFirstPaySalaryD>().Where(w => w.EmpCode == _filter.EmpCode && w.PeriodID == _filter.Period).ToList();
                var ListEmpOT = unitOfWork.Set<HisGenOTFirstPay>().Where(w => w.EmpCode == _filter.EmpCode && w.PeriodID == _filter.Period).ToList();
                var ListEmpDedLeave = unitOfWork.Set<HisGenLeaveDFirstPay>().Where(w => w.EmpCode == _filter.EmpCode && w.PeriodID == _filter.Period).ToList();
                var ListEmpAllownace = unitOfWork.Set<HisEmpRewardFP>().Where(w => w.EmpCode == _filter.EmpCode && w.PeriodID == _filter.Period).ToList();
                var ListEmpPaySlip = unitOfWork.Set<HISPaySlipFirstPay>().Where(w => w.EmpCode == _filter.EmpCode && w.PeriodID == _filter.Period).ToList();
                if (ListEmpSalary.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpSalary);
                }
                // delete salary d
                if (ListEmpSalaryFPD.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpSalaryFPD);
                }
                // delete overtime
                if (ListEmpOT.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpOT);
                }
               // delete leave deduction
                if (ListEmpDedLeave.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpDedLeave);
                }
                // delete allowance
                if (ListEmpAllownace.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpAllownace);
                }
                // delete payslip
                if (ListEmpPaySlip.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpPaySlip);
                }
                unitOfWork.BulkCommit();
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.DELETE.ToString(), e, true);
            }
        }
        public string DeleteGenerate(string EmpCode, int Period)
        {
            unitOfWork.BeginTransaction();
            try
            {
                var ListEmpSalary = unitOfWork.Set<HISGenFirstPay>().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                var ListEmpSalaryFPD = unitOfWork.Set<HISGenFirstPaySalaryD>().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                var ListEmpOT = unitOfWork.Set<HisGenOTFirstPay>().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                var ListEmpDedLeave = unitOfWork.Set<HisGenLeaveDFirstPay>().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                var ListEmpAllownace = unitOfWork.Set<HisEmpRewardFP>().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                var ListEmpPaySlip = unitOfWork.Set<HISPaySlipFirstPay>().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
                if (ListEmpSalary.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpSalary);
                }
                // delete salary d
                if (ListEmpSalaryFPD.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpSalaryFPD);
                }
                // delete overtime
                if (ListEmpOT.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpOT);
                }
                // delete leave deduction
                if (ListEmpDedLeave.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpDedLeave);
                }
                // delete allowance
                if (ListEmpAllownace.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpAllownace);
                }
                // delete payslip
                if (ListEmpPaySlip.Count > 0)
                {
                    unitOfWork.BulkDelete(ListEmpPaySlip);
                }
                unitOfWork.BulkCommit();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.DELETE.ToString(), e, true);
            }
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

        public void GetDataDetailFP(string EmpCode, int Period)
        {
            OnLoad();
            ListPaySlip = new List<HISPaySlipFirstPay>();
            ListLeaveDed = new List<LeaveDeduction>();
            STAFF_VIEW = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == EmpCode);
            HeaderSalaryFP = unitOfWork.Set<HISGenFirstPay>().FirstOrDefault(w => w.EmpCode == EmpCode && w.PeriodID == Period);
            ListBasicSalary = unitOfWork.Set<HISGenFirstPaySalaryD>().AsQueryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
            ListEmpOTFP = unitOfWork.Set<HisGenOTFirstPay>().AsQueryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
            var LstEmpReward = unitOfWork.Repository<HisEmpRewardFP>().Queryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
            ListEmpAllowanceFP = LstEmpReward.Where(w => w.RewardType == "Allowance").ToList();
            ListEmpBonusFP = LstEmpReward.Where(w => w.RewardType == "Bonus").ToList();
            ListDeductionFP = LstEmpReward.Where(w => w.RewardType == "Deduction").ToList();
            var LeaveDed = unitOfWork.Set<HisGenLeaveDFirstPay>().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
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
                              Amount = myGroup.Sum(w => w.Qty) * myGroup.Key.Rate
                          }).ToList();
            foreach (var item in result)
            {
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
            var ListPay = unitOfWork.Set<HISPaySlipFirstPay>().AsQueryable().Where(w => w.EmpCode == EmpCode && w.PeriodID == Period).ToList();
            foreach (var item in ListPay)
            {
                var Pay = new HISPaySlipFirstPay();
                Pay.TranLine = item.TranLine;
                Pay.EmpCode = item.EmpCode;
                if (item.EarnDesc != "EARNING")
                {
                    Pay.EarnDesc = item.EarnDesc;
                    Pay.EAmount = item.EAmount;
                }
                if (item.DeductDesc != "DEDUCTIONS")
                {
                    Pay.DeductDesc = item.DeductDesc;
                    Pay.DeductAmount = item.DeductAmount;
                }
                ListPaySlip.Add(Pay);
            }
        }
        public Dictionary<string, dynamic> OnDataSelectorDetailFP(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            ClsFilterJob clsFilter = new ClsFilterJob();

            keyValues.Add("PERIOD_SELECT", clsFilter.LoadPeriod());
            return keyValues;
        }

        //#endregion
        public ClsFPSalary New_Filter()
        {
            ClsFPSalary _filter = new ClsFPSalary();
            _filter.HisGenFirstPay = new HISGenFirstPay();
            _filter.ListFirstPay = new List<HISGenFirstPay>();
            _filter.AppSalaryFP = new List<HisPendingAppSalaryFP>();
            _filter.ListFirstPayItem = new List<HISGenFirstPaySalaryD>();
            _filter.ListEmpOTFP = new List<HisGenOTFirstPay>();
            _filter.ListEmpRewardFP = new List<HisEmpRewardFP>();
            _filter.ListDedLeaveFP = new List<HisGenLeaveDFirstPay>();
            _filter.LstPayHis = new List<ClsPayHis>();
            _filter.ListPaySlip = new List<HISPaySlipFirstPay>();
            return _filter;
        }
        public HISGenFirstPay New_FirstPay(ClsFilerFPSalary _filter, ClsBenefit objBeneift)
        {
            var _GenSala = new HISGenFirstPay()
            {
                PeriodID = _filter.Period,
                CompanyCode = _filter.Staff.CompanyCode,
                Status = SYDocumentStatus.OPEN.ToString(),
                TermStatus = _filter.Staff.TerminateStatus,
                TermRemark = _filter.Staff.TerminateRemark,
                TermDate = _filter.Staff.DateTerminate,
                INYear = _filter.InYear,
                INMonth = _filter.InMonth,
                FromDate = _filter.FromDate,
                ToDate = _filter.ToDate,
                PayFrom = _filter.PayFrom,
                PayTo = _filter.PayTo,
                WorkDay = objBeneift.DayInMonth,
                WorkHour = objBeneift.WorkHour,
                ExchRate = _filter.ExchangeRate,
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
                BasicSalary = objBeneift.BaseSalary
            };

            return _GenSala;
        }
        public HISGenFirstPaySalaryD New_FirstPayItem(ClsFilerFPSalary _filter, HREmpCareer _EmpCareer, decimal ActWorkDay, decimal DayInMonth
        , decimal WorkHourPerDay, decimal Rate, decimal TempSalary)
        {
            var _His_GenD = new HISGenFirstPaySalaryD()
            {
                PeriodID = _filter.Period,
                CompanyCode = _filter.Staff.CompanyCode,
                INYear = _filter.InYear,
                INMonth = _filter.InMonth,
                FromDate = _filter.FromDate,
                ToDate = _filter.ToDate,
                CareerCode = _EmpCareer.CareerCode,
                WorkDay = DayInMonth,
                WorkHour = WorkHourPerDay,
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
                PayFrom = _filter.FromDate,
                PayTo = _filter.ToDate,
                ActWorkDay = ActWorkDay,
                BasicSalary = _EmpCareer.NewSalary,
                Rate = Rate,
                Amount = TempSalary,
                SalaryPartial = TempSalary,
                CreateBy = User.UserName,
                CreateOn = DateTime.Now,
            };

            return _His_GenD;
        }
        public int Rounding(decimal? Salary)
        {
            int _result = 0;
            if (Salary == 0) return 0;
            int _netPay = Convert.ToInt32(Salary);
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
    }
}