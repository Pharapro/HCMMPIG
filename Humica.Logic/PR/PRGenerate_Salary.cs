using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.Helper;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.Atts;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;

namespace Humica.Logic.PR
{
    public class PRGenerate_Salary
    {
        public HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity DMS = new SMSystemEntity();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PREmpAllw Header { get; set; }
        public FTFilerPayroll Filter { get; set; }
        public string ScreenId { get; set; }
        public string CountryCode { get; set; }
        public string MessageCode { get; set; }
        public string MessageError { get; set; }
        public int ApproveLevel { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public HISGenSalary HeaderSalary { get; set; }
        public List<PREmpAllw> ListHeader { get; set; }
        public List<HR_STAFF_VIEW> ListStaff { get; set; }
        public List<HR_View_EmpGenSalary> ListEmployeeGen { get; set; }
        public List<HR_PR_EmpSalary> ListEmpPaySlip { get; set; }
        public List<HISGenSalaryD> ListBasicSalary { get; set; }
        public List<HISGenOTHour> ListOverTime { get; set; }
        public List<HISGenAllowance> ListAllowance { get; set; }
        public List<HISGenBonu> ListBonus { get; set; }
        public List<HISGenDeduction> ListDeduction { get; set; }
        public List<LeaveDeduction> ListLeaveDed { get; set; }
        public List<PR_GLCharge_View> ListGLCharge { get; set; }
        public List<HisCostCharge> ListCostCharge { get; set; }
        public List<HISPaySlip> ListPaySlip { get; set; }
        public List<EmpSeniority> ListEmpSeniority { get; set; }
        public List<HISApproveSalary> ListApproveSalary { get; set; }
        public List<SalaryApproval> ListSalaryApprove { get; set; }
        public List<HisPendingAppSalary> ListAppSalaryPending { get; set; }
        public ClsEmail EmailObject { get; set; }
        public GEN_Filter GenFilter { get; set; }
        public string EmpID { get; set; }
        public int Progress { get; set; }
        public int InYear { get; set; }
        public bool IsJan { get; set; }
        public bool IsFeb { get; set; }
        public bool IsMar { get; set; }
        public bool IsApr { get; set; }
        public bool IsMay { get; set; }
        public bool IsJun { get; set; }
        public bool IsJul { get; set; }
        public bool IsAug { get; set; }
        public bool IsSep { get; set; }
        public bool IsOct { get; set; }
        public bool IsNov { get; set; }
        public bool IsDec { get; set; }

        public static List<ListProgress> ListProgress { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public HISApproveSalary HeaderAppSalary { get; set; }
        public PRGenerate_Salary()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public void SetAutoApproval(string SCREEN_ID, string DocType)
        {
            ListApproval = new List<ExDocApproval>();
            var DBX = new HumicaDBContext();
            var objDoc = DBX.ExCfWorkFlowItems.Find(SCREEN_ID, DocType);
            if (objDoc != null)
            {
                if (objDoc.IsRequiredApproval == true)
                {
                    var listDefaultApproval = DBX.ExCfWFSalaryApprovers.Where(w => w.WFObject == objDoc.ApprovalFlow && w.IsSelected == true).ToList();
                    foreach (var read in listDefaultApproval)
                    {
                        var objApp = new ExDocApproval();
                        objApp.Approver = read.Employee;
                        objApp.ApproverName = read.EmployeeName;
                        objApp.DocumentType = DocType;
                        objApp.ApproveLevel = read.ApproveLevel;
                        objApp.WFObject = objDoc.ApprovalFlow;
                        ListApproval.Add(objApp);
                    }
                }
            }
        }
        public string isValidApproval(ExDocApproval Approver, EnumActionGridLine Action)
        {
            if (Action == EnumActionGridLine.Add)
            {
                if (ListApproval.Where(w => w.Approver == Approver.Approver).ToList().Count > 0)
                {
                    return "DUPLICATED_ITEM";
                }
            }

            return SYConstant.OK;
        }
        public HRStaffProfile getNextApprover(string id, string pro)
        {
            var objStaff = new HRStaffProfile();
            var DBX = new HumicaDBContext();
            var objHeader = DBX.HISApproveSalaries.Find(id);
            if (objHeader == null)
            {
                return new HRStaffProfile();
            }
            string open = SYDocumentStatus.OPEN.ToString();
            var listCanApproval = DBX.ExDocApprovals.Where(w => w.DocumentNo == id && w.Status == open && w.DocumentType == objHeader.DocumentType).ToList();

            if (listCanApproval.Count == 0)
            {
                return new HRStaffProfile();
            }

            var min = listCanApproval.Min(w => w.ApproveLevel);
            var NextApp = listCanApproval.Where(w => w.ApproveLevel == min).First();
            objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == NextApp.Approver);//, objHeader.Property);
            return objStaff;
        }
        public List<EmpSeniority> LoadDataSeniority(FTFilerPayroll Filter1, List<HRBranch> _lstBranch)
        {
            DateTime FromDate = new DateTime(Filter1.FromDate.Year, Filter1.FromDate.Month, 1);
            DateTime ToDate = new DateTime(Filter1.ToDate.Year, Filter1.ToDate.Month, DateTime.DaysInMonth(Filter1.ToDate.Year, Filter1.ToDate.Month));
            var _List = new List<EmpSeniority>();
            var _staff = DB.HRStaffProfiles.ToList();
            _staff = _staff.ToList();
            var staff = DBV.HR_View_EmpGenSalary.ToList();
            var _listStaff = staff.ToList();
            DateTime date = new DateTime(1900, 1, 1);
            _listStaff = _listStaff.Where(w => w.StartDate.Value.Date <= ToDate.Date && (w.DateTerminate.Date == date.Date
            || w.DateTerminate.AddDays(-1) >= ToDate.Date)).ToList();
            //Staff Atfer probation
            var _StaffPro = _staff.Where(w => w.Probation.Value.Date >= FromDate.Date && w.Probation.Value.Date <= ToDate.Date).ToList();
            var pro = new List<HRStaffProfile>();
            var PayPar = DB.PRParameters.ToList();
            _StaffPro = _StaffPro.Where(w => w.Probation.Value.Year == ToDate.Year && w.Probation.Value.Month == ToDate.Month).ToList();
            foreach (var item in _StaffPro)
            {
                var _parm = PayPar.FirstOrDefault(w => w.Code == item.PayParam);
                var Day = Get_WorkingDay(_parm, item.Probation.Value, ToDate);
                if (Day < 21)
                {
                    pro.Add(item);
                }
            }
            // _listStaff = _listStaff.Where(w => w.SericeLenghtDay >= 21).ToList();
            _listStaff = _listStaff.Where(w => !pro.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();

            _listStaff = _listStaff.Where(x => _lstBranch.Where(w => w.Code == x.Branch).Any()).ToList();
            if (Filter1.Branch != null)
                _listStaff = _listStaff.Where(w => w.Branch == Filter1.Branch).ToList();
            if (Filter1.Division != null)
                _listStaff = _listStaff.Where(w => w.Division == Filter1.Division).ToList();
            if (Filter1.Department != null)
                _listStaff = _listStaff.Where(w => w.DEPT == Filter1.Department).ToList();
            if (Filter1.Section != null)
                _listStaff = _listStaff.Where(w => w.SECT == Filter1.Section).ToList();
            if (Filter1.Position != null)
                _listStaff = _listStaff.Where(w => w.JobCode == Filter1.Position).ToList();
            if (Filter1.Level != null)
                _listStaff = _listStaff.Where(w => w.LevelCode == Filter1.Level).ToList();
            // if (Filter1.SalaryType != null)
            //    _listStaff = _listStaff.Where(w => w.SalaryType == Filter1.SalaryType).ToList();

            var Emp_Salary = (from C in DB.HISGenSalaries
                              where ((C.FromDate >= FromDate && C.FromDate <= ToDate) || (C.ToDate >= FromDate && C.ToDate <= ToDate) ||
                       (FromDate >= C.FromDate && FromDate <= C.ToDate) || (ToDate >= C.FromDate && ToDate <= C.ToDate))
                              select C).ToList();
            GetDataIsMonth(FromDate, ToDate);
            decimal? TotalAmount = 0;
            
            foreach (var item in _listStaff)
            {
                TotalAmount = 0;
                int CMonth = DateTimeHelper.MonthDiff(FromDate, ToDate) + 1;
                decimal RateInDate = Filter1.Rate / CMonth;
                decimal _SPrate = Filter1.Rate;
                var Staff = _staff.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                var Parameter = PayPar.FirstOrDefault(w => w.Code == Staff.PayParam);
                DateTime FFromDate = DateTimeHelper.StartDateOfMonth(ToDate);
                DateTime FToDate = DateTimeHelper.EndDateOfMonth(ToDate);
                if (!Parameter.IsPrevoiuseMonth.IsNullOrZero())
                {
                    DateTime tempDate = FFromDate.AddMonths(-1);
                    if (Parameter.FROMDATE.HasValue)
                        FromDate = new DateTime(tempDate.Year, tempDate.Month, Parameter.FROMDATE.Value().Day);
                    if (Parameter.TODATE.HasValue)
                        ToDate = new DateTime(ToDate.Year, ToDate.Month, Parameter.TODATE.Value().Day);
                }

                decimal DayInMonth = Get_WorkingDay_Allw(Parameter, FFromDate, FToDate);

                var EmpSin = new EmpSeniority();
                EmpSin.EmpCode = item.EmpCode;
                EmpSin.AllName = item.AllName;
                EmpSin.Department = item.Department;
                EmpSin.Position = item.Position;
                EmpSin.StartDate = item.StartDate.Value;
                EmpSin.Probation = item.Probation.Value;
                EmpSin.Salary = Staff.Salary;
                EmpSin.Rate = 0;
                EmpSin.Balance = 0;
                EmpSin.Average = 0;
                var ListHis = Emp_Salary.Where(w => w.EmpCode == item.EmpCode).ToList();
                
                foreach (var _emp in ListHis)
                {
                    decimal Salary = 0;
                    if (Filter.SalaryType == "BS")
                    {
                        Salary = _emp.Salary.Value;
                    }
                    else if (Filter.SalaryType == "GP")
                    {
                        Salary = _emp.Salary.Value;
                        //Salary = _emp.GrossPay.Value;
                        if( _emp.AlwBeforTax.HasValue)
                        {
                            Salary += _emp.AlwBeforTax.Value;
                        }
                    }
                    else
                    {
                        Salary = _emp.NetWage.Value + _emp.FirstPaymentAmount;
                    }
                    decimal _Seniority = 0;
                    if (_emp.INYear == ToDate.Year && _emp.INMonth == ToDate.Month && _emp.Seniority.HasValue)
                        _Seniority = _emp.Seniority.Value + _emp.SeniorityTaxable.Value;
                    Salary = Salary - _Seniority;
                    if (Staff.Probation.Value.Year > _emp.ToDate.Value.Year)
                    {
                        Salary = 0;
                        CMonth -= 1;
                    }
                    
                    if (Staff.Probation.Value.Year == ToDate.Year)
                    {
                        if (Staff.Probation.Value.Date <= _emp.FromDate.Value.Date)
                        {
                            if (_emp.INYear == Staff.Probation.Value.Year && _emp.INMonth == Staff.Probation.Value.Month)
                            {
                                DateTime Prob = Staff.Probation.Value;
                                DateTime _ToDate = new DateTime(Prob.Year, Prob.Month, DateTime.DaysInMonth(Prob.Year, Prob.Month));
                                decimal _DayInMonth = Get_WorkingDay(Parameter, Prob, _ToDate);
                                if (_DayInMonth < 21)
                                {
                                    Salary = 0;
                                    CMonth -= 1;
                                }
                            }
                        }
                        else
                        {
                            if (_emp.INYear == Staff.Probation.Value.Year && _emp.INMonth == Staff.Probation.Value.Month)
                            {
                                DateTime Prob = Staff.Probation.Value;
                                DateTime _ToDate = new DateTime(Prob.Year, Prob.Month, DateTime.DaysInMonth(Prob.Year, Prob.Month));
                                decimal _DayInMonth = Get_WorkingDay(Parameter, Prob, _ToDate);
                                if (_DayInMonth < 21)
                                {
                                    Salary = 0;
                                    CMonth -= 1;
                                }
                            }
                            else
                            {
                                Salary = 0;
                                CMonth -= 1;
                            }
                        }
                    }
                    GetDataSenior(EmpSin, _emp.ToDate.Value, Salary);
                    TotalAmount += Salary;
                }
                EmpSin.Rate = CMonth * RateInDate;
                if (TotalAmount > 0)
                    EmpSin.Average = ClsRounding.Rounding((TotalAmount / CMonth / DayInMonth).Value(), SYConstant.DECIMAL_PLACE, "N");
                EmpSin.TotalAmount = ClsRounding.Rounding(EmpSin.Average.Value * EmpSin.Rate.Value, SYConstant.DECIMAL_PLACE, "N");
                EmpSin.Remark = CMonth.ToString();
                _List.Add(EmpSin);
            }
            return _List.OrderBy(w => w.EmpCode).ToList();
        }
        public List<HR_PR_EmpSalary> LoadEmpPaySlip(FTFilerPayroll Filter1, List<HRBranch> _ListBranch)
        {
            var _List = new List<HR_PR_EmpSalary>();
            var _lstEmpPaySlip = DBV.HR_PR_EmpSalary.ToList();
            var _staff = DB.HRStaffProfiles.ToList();
            var _listStaff = _staff.ToList();
            DateTime date = new DateTime(1900, 1, 1);
            _lstEmpPaySlip = _lstEmpPaySlip.Where(w => w.INYear == Filter1.InMonth.Year && w.INMonth == Filter1.InMonth.Month).ToList();
            _listStaff = _listStaff.Where(w => _ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            if (Filter1.Branch != null)
                _listStaff = _listStaff.Where(w => w.Branch == Filter1.Branch).ToList();
            if (Filter1.Division != null)
                _listStaff = _listStaff.Where(w => w.Division == Filter1.Division).ToList();
            if (Filter1.Department != null)
                _listStaff = _listStaff.Where(w => w.DEPT == Filter1.Department).ToList();
            if (Filter1.Section != null)
                _listStaff = _listStaff.Where(w => w.SECT == Filter1.Section).ToList();
            if (Filter1.Position != null)
                _listStaff = _listStaff.Where(w => w.JobCode == Filter1.Position).ToList();
            if (Filter1.Level != null)
                _listStaff = _listStaff.Where(w => w.LevelCode == Filter1.Level).ToList();

            _List = _lstEmpPaySlip.Where(w => _listStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            return _List.OrderBy(w => w.EmpCode).ToList();
        }

        public void Calculate_Misscan_Deduction(GEN_Filter _filter, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                decimal? MisscanDed = 0.0M;
                var dayrate = 0.0M;
                var STAFF = DB.HRStaffProfiles.Where(w => w.EmpCode == _filter.EmpCode).FirstOrDefault();
                //var param = DB.PRParameters.Where(w => w.Code == STAFF.PayParam).FirstOrDefault();
                var MISS = DB.PREmpLateDeducts.Where(w => w.EmpCode == _filter.EmpCode && w.FromDate >= FromDate && w.ToDate <= ToDate && w.DedCode == "MissScan").ToList();
                decimal WorkDayPerMonth = Get_WorkingDay_Salary(_filter.Parameter, FromDate, ToDate);
                if (STAFF.IsAtten != true)
                {
                    dayrate = STAFF.Salary / WorkDayPerMonth;
                }
                else
                {
                    dayrate = 0;
                }
                string Cutday = string.Empty;
                if (MISS.Sum(w => w.Day) > 0)
                {
                    MisscanDed = MISS.Sum(w => w.Day) * dayrate;
                    Cutday = MISS.Sum(w => w.Day) + " Day";
                    var _GenDed = new HISGenDeduction()
                    {
                        INYear = _filter.InYear,
                        INMonth = _filter.InMonth,
                        EmpCode = _filter.EmpCode,
                        FromDate = _filter.FromDate,
                        ToDate = _filter.ToDate,
                        WorkDay = 0,
                        RatePerDay = dayrate,
                        DedCode = "NOSCAN",
                        DedDesc = "Misscan",
                        TaxAble = true,
                        DedAm = Convert.ToDecimal(MisscanDed),
                        Remark = Cutday,
                        CreateBy = User.UserName,
                        CreateOn = DateTime.Now
                    };
                    _filter.ListHisEmpDed.Add(_GenDed);
                    DB.HISGenDeductions.Add(_GenDed);
                    DB.SaveChanges();
                }

            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }

        }
        public void Calculate_Late_Early_Deduction(GEN_Filter _filter, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                var EmpCareer = _filter.LstEmpCareer.ToList();
                decimal WorkDay = Get_WorkingDay_Ded(_filter.Parameter, FromDate, ToDate);
                PRSRewardType rewardType = new PRSRewardType();
                #region ******** InMonth ********
                //var EmpDed = DB.PREmpLateDeducts.Where(w => w.EmpCode == _filter.EmpCode && w.FromDate.Value.Year == _filter.InYear && w.FromDate.Value.Month == _filter.InMonth &&
                //   w.ToDate.Value.Year == _filter.InYear && w.ToDate.Value.Month == _filter.InMonth && w.Status == 1).ToList();
                var EmpDed = DB.PREmpLateDeducts.Where(w => w.EmpCode == _filter.EmpCode && w.InMonth.Year == _filter.InYear && w.InMonth.Month == _filter.InMonth && w.Status == 1).ToList();
                var List = EmpDed.GroupBy(w => new { w.EmpCode, w.DedCode, w.FromDate, w.ToDate }).
                    Select(x => new PREmpLateDeduct
                    {
                        EmpCode = x.Key.EmpCode,
                        DedCode = x.Key.DedCode,
                        FromDate = x.Key.FromDate,
                        ToDate = x.Key.ToDate,
                        Minute = x.Sum(s => s.Minute),
                        Day = x.Sum(s => s.Day)
                    }).ToList();
                var Reward_Type = rewardType.GetRewardsType(_filter.LstRewardsType, RewardTypeCode.DED.ToString());

                foreach (var Ded in List)
                {
                    var Emp_C = EmpCareer.FirstOrDefault(w => ToDate >= w.FromDate && ToDate <= w.ToDate);
                    // ActualWorkDay = ToDate.Subtract(FromDate).Days + 1;
                    var _Ded = Reward_Type.FirstOrDefault(w => w.Code == Ded.DedCode);
                    if (_Ded != null && Emp_C != null)
                    {
                        decimal Salary = Emp_C.NewSalary;
                        decimal Rate = (Salary / WorkDay);
                        decimal Total = Convert.ToDecimal(Ded.Minute);
                        decimal Hours = Math.Round(Total / 60.00M, 2);
                        decimal RateH = Math.Round(Convert.ToDecimal(Rate / _filter.Parameter.WHOUR), 2);
                        //  decimal Amount = ((Hours + Convert.ToDecimal(Ded.Day * _filter.Parameter.WHOUR)) * RateH);
                        decimal Amount = Convert.ToDecimal((Ded.Day * Rate)) + (Hours * RateH);
                        var _GenDed = new HISGenDeduction()
                        {
                            INYear = _filter.InYear,
                            INMonth = _filter.InMonth,
                            EmpCode = _filter.EmpCode,
                            FromDate = Ded.FromDate,
                            ToDate = Ded.ToDate,
                            WorkDay = 0,
                            RatePerDay = 0,
                            DedCode = Ded.DedCode,
                            DedDesc = _Ded.Description,
                            OthDesc = _Ded.OthDesc,
                            TaxAble = _Ded.Tax,
                            Remark = Ded.Minute.ToString(),
                            DedAm = Convert.ToDecimal(Amount),
                            DedAMPM = Convert.ToDecimal(Amount / WorkDay),
                            CreateBy = User.UserName,
                            CreateOn = DateTime.Now
                        };
                        if (_filter.LstPayHis.Where(w => w.PayType == "DED" && w.Code == _Ded.Code).Any())
                        {
                            _filter.LstPayHis.Where(w => w.PayType == "DED" && w.Code == _Ded.Code).ToList().ForEach(x => x.Amount -= Convert.ToDecimal(Amount));
                        }
                        else
                        {
                            _filter.LstPayHis.Add(new ClsPayHis()
                            {
                                EmpCode = _filter.EmpCode,
                                SGROUP = "H",
                                PayType = "DED",
                                Code = _Ded.Code,
                                Description = _Ded.Description.ToUpper(),
                                Amount = -Convert.ToDecimal(Amount)
                            });
                        }
                        _filter.ListHisEmpDed.Add(_GenDed);
                        DB.HISGenDeductions.Add(_GenDed);
                        DB.SaveChanges();
                    }
                }
                #endregion
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
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
        public void Commit_PayHis(GEN_Filter _filter, List<HisPayHi> ListPayH)
        {
            var DBD = new HumicaDBContext();
            try
            {
                DBD.Configuration.AutoDetectChangesEnabled = false;
                DB.Configuration.AutoDetectChangesEnabled = false;
                var _lstPayH = ListPayH.Where(w => w.EmpCode == _filter.EmpCode).ToList();
                var NoResult = _lstPayH.Where(w => !_filter.LstPayHis.Where(x => x.Code == w.Code && w.PayType == x.PayType).Any()).ToList();
                decimal Amount = 0;
                var PayHis = (from PH in _filter.LstPayHis
                              group PH by new { PH.SGROUP, PH.Code, PH.PayType }
                             into g
                              select new
                              {
                                  SGROUP = g.Key.SGROUP,
                                  Code = g.Key.Code,
                                  PayType = g.Key.PayType,
                              }).ToList().OrderBy(w => w.SGROUP);
                foreach (var read in PayHis)
                {
                    var ListHis = _filter.LstPayHis.Where(w => w.Code == read.Code && w.SGROUP == read.SGROUP && w.PayType == read.PayType).ToList();//.OrderBy(w => w.SGROUP);
                    var item = ListHis.FirstOrDefault(w => w.Code == read.Code && w.SGROUP == read.SGROUP && w.PayType == read.PayType);//.OrderBy(w => w.SGROUP);
                                                                                                                                        //foreach (var item in _filter.LstPayHis.Where().OrderBy(w => w.SGROUP))
                                                                                                                                        //{
                    int result = 0;
                    var _payHis = _lstPayH.Where(w => w.PayType == item.PayType && w.Code == item.Code).FirstOrDefault();
                    Amount = ListHis.Sum(w => w.Amount);
                    var PayH = new HisPayHi();

                    if (_payHis != null)
                        PayH = _payHis;
                    else
                    {
                        result = 1;
                        PayH.EmpCode = item.EmpCode;
                        PayH.InYear = _filter.InYear;
                        PayH.SGROUP = item.SGROUP;
                        PayH.PayType = item.PayType;
                        PayH.Code = item.Code;
                        PayH.Description = item.Description;

                    }
                    if (_filter.InMonth == 1)
                        PayH.JAN = Amount;
                    else if (_filter.InMonth == 2)
                        PayH.FEB = Amount;
                    else if (_filter.InMonth == 3)
                        PayH.MAR = Amount;
                    else if (_filter.InMonth == 4)
                        PayH.APR = Amount;
                    else if (_filter.InMonth == 5)
                        PayH.MAY = Amount;
                    else if (_filter.InMonth == 6)
                        PayH.JUN = Amount;
                    else if (_filter.InMonth == 7)
                        PayH.JUL = Amount;
                    else if (_filter.InMonth == 8)
                        PayH.AUG = item.Amount;
                    else if (_filter.InMonth == 9)
                        PayH.SEP = Amount;
                    else if (_filter.InMonth == 10)
                        PayH.OCT = Amount;
                    else if (_filter.InMonth == 11)
                        PayH.NOV = Amount;
                    else if (_filter.InMonth == 12)
                        PayH.DECE = Amount;
                    if (result == 1)
                        DB.HisPayHis.Add(PayH);
                    else
                    {
                        DB.HisPayHis.Attach(PayH);
                        DB.Entry(PayH).State = System.Data.Entity.EntityState.Modified;
                    }

                }
                //foreach(var item in NoResult)
                //{
                //    if (_filter.InMonth == 1)
                //        item.JAN = 0;
                //    else if (_filter.InMonth == 2)
                //        item.FEB = 0;
                //    else if (_filter.InMonth == 3)
                //        item.MAR = 0;
                //    else if (_filter.InMonth == 4)
                //        item.APR = 0;
                //    else if (_filter.InMonth == 5)
                //        item.MAY = 0;
                //    else if (_filter.InMonth == 6)
                //        item.JUN = 0;
                //    else if (_filter.InMonth == 7)
                //        item.JUL = 0;
                //    else if (_filter.InMonth == 8)
                //        item.AUG = 0;
                //    else if (_filter.InMonth == 9)
                //        item.SEP = 0;
                //    else if (_filter.InMonth == 10)
                //        item.OCT = 0;
                //    else if (_filter.InMonth == 11)
                //        item.NOV = 0;
                //    else if (_filter.InMonth == 12)
                //        item.DECE = 0;

                //    var result = item.JAN + item.FEB + item.MAR + item.APR + item.MAY +
                //        item.JUN + item.JUL + item.AUG + item.SEP + item.OCT + item.NOV + item.DECE;
                //    if(result==0)
                //    {
                //        DBD.HisPayHis.Attach(item);
                //        DBD.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                //    }
                //    else
                //    {
                //        DB.HisPayHis.Attach(item);
                //        DB.Entry(item).State = System.Data.Entity.EntityState.Modified;
                //    }
                //}
                //var rowD = DBD.SaveChanges();
                var row1 = DB.SaveChanges();
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
                DBD.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string Commit_PayCostAccount(GEN_Filter _filter)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;

                var HisSal = _filter.HisGensalarys;
                DateTime DateNoW = DateTime.Now;
                var GLmappings = DB.PRGLmappings.ToList();
                int ID = Convert.ToInt32(_filter.Staff.GrpGLAcc);
                var CostCenter = DB.PRCostCenters.FirstOrDefault(w => w.ID == ID);
                int LineItem = 0;
                if (CostCenter != null)
                {
                    GLmappings = GLmappings.Where(w => w.ID == CostCenter.ID).ToList();
                    if (HisSal != null && GLmappings.Count > 0)
                    {
                        LineItem = 0;
                        var GLMap = GLmappings.Where(w => w.BenGrp == "SYS" && w.BenCode != "_TAXC" && !string.IsNullOrEmpty(w.GLCode)).ToList();
                        var ListSyAccount = DB.SYSHRBuiltinAccs.ToList();
                        List<object> ListObjectDictionary = new List<object>();
                        ListObjectDictionary.Add(_filter.HisGensalarys);
                        foreach (var item in GLMap)
                        {
                            if (item.GLCode == null) continue;
                            var objParam = ListSyAccount.FirstOrDefault(w => w.Code == item.BenCode);
                            if (objParam.ObjectName != null && objParam.FieldName != null)
                            {
                                LineItem += 1;
                                var GLBanChar = new HISGLBenCharge();
                                SwapValueBenCharge(GLBanChar, _filter, item, LineItem, objParam.IsPO,objParam.IsCredit);
                                var Amount = ClsInformation.GetFieldValues(objParam.ObjectName, ListObjectDictionary, objParam.FieldName);
                                if (Amount != null)
                                {
                                    if (Convert.ToDecimal(Amount) != 0)
                                    {
                                        GLBanChar.Amount = Convert.ToDecimal(Amount);
                                        if (item.BenCode == "_TAX" || item.BenCode == "_SPF")
                                            GLBanChar.IsDebitNote = true;
                                        DB.HISGLBenCharges.Add(GLBanChar);
                                        if (item.BenCode == "_TAX" && GLBanChar.Amount > 0)
                                        {
                                            DB.HISGLBenCharges.Add(GLBanChar);
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
                                                    DB.HISGLBenCharges.Add(GLBanChar1);
                                                }
                                                else
                                                {
                                                    return "INVALID_TAX_Payroll_Tax";
                                                }
                                            }
                                        }
                                        if(item.BenCode== "_CPF" && GLBanChar.Amount > 0)
                                        {
                                            LineItem += 1;
                                            var GLBanChar1 = new HISGLBenCharge();
                                            SwapValueBenCharge(GLBanChar1, _filter, item, LineItem, false,true);
                                            GLBanChar1.Amount = Convert.ToDecimal(Amount);
                                            DB.HISGLBenCharges.Add(GLBanChar1);
                                        }
                                    }
                                }
                            }
                        }
                        // var row = DB.SaveChanges();
                    }
                    //*************OverTime*************
                    var HisOT = _filter.ListHisEmpOT; //DB.HISGenOTHours.ToList();
                    HisOT = HisOT.Where(w => w.INYear == _filter.InYear && w.INMonth == _filter.InMonth && _filter.Staff.EmpCode == w.EmpCode).ToList();
                    var GLOT = GLmappings.Where(w => w.BenGrp == "OT" && HisOT.Where(x => x.OTCode == w.BenCode).Any()).ToList();
                    if (GLOT.Count > 0)
                    {
                        LineItem = 0;
                        foreach (var item in GLOT)
                        {
                            var OTValue = HisOT.FirstOrDefault(w => w.OTCode == item.BenCode);
                            if (OTValue != null)
                            {
                                LineItem += 1;
                                var GLBanChar1 = new HISGLBenCharge();
                                SwapValueBenCharge(GLBanChar1, _filter, item,LineItem,true);
                                if (GLBanChar1.GLCode == null) continue;
                                GLBanChar1.Amount = OTValue.OTRate * Convert.ToDecimal(OTValue.OTHour);
                                DB.HISGLBenCharges.Add(GLBanChar1);
                            }
                        }
                    }
                    //*************Allowance*************
                    var HisAllow = _filter.ListHisEmpAllw; //DB.HISGenAllowances.ToList();
                    HisAllow = HisAllow.Where(w => w.INYear == _filter.InYear && w.INMonth == _filter.InMonth && _filter.Staff.EmpCode == w.EmpCode).ToList();
                    var GLAllow = GLmappings.Where(w => w.BenGrp == "ALW" && HisAllow.Where(x => x.AllwCode == w.BenCode).Any()).ToList();
                    if (GLAllow.Count > 0)
                    {
                        LineItem = 0;
                        foreach (var item in GLAllow)
                        {
                            var AllwValue = HisAllow.FirstOrDefault(w => w.AllwCode == item.BenCode);
                            if (AllwValue != null)
                            {
                                LineItem += 1;
                                var GLBanChar1 = new HISGLBenCharge();
                                SwapValueBenCharge(GLBanChar1, _filter, item,LineItem,true);
                                GLBanChar1.Amount = AllwValue.AllwAm;
                                GLBanChar1.IsPO = true;
                                if (GLBanChar1.GLCode == null) continue;
                                DB.HISGLBenCharges.Add(GLBanChar1);
                            }
                        }
                    }
                    //*************Bonus*************
                    var HisBon = _filter.ListHisEmpBonu;// DB.HISGenBonus.ToList();
                    HisBon = HisBon.Where(w => w.INYear == _filter.InYear && w.INMonth == _filter.InMonth && _filter.Staff.EmpCode == w.EmpCode).ToList();
                    var GLBon = GLmappings.Where(w => w.BenGrp == "BON" && HisBon.Where(x => x.BonusCode == w.BenCode).Any()).ToList();
                    if (GLBon.Count > 0)
                    {
                        LineItem = 0;
                        foreach (var item in GLBon)
                        {
                            var BonValue = HisBon.FirstOrDefault(w => w.BonusCode == item.BenCode);
                            if (BonValue != null)
                            {
                                LineItem += 1;
                                var GLBanChar1 = new HISGLBenCharge();
                                SwapValueBenCharge(GLBanChar1, _filter, item,LineItem,true);
                                if (GLBanChar1.GLCode == null) continue;
                                GLBanChar1.IsPO = true;
                                GLBanChar1.Amount = Convert.ToDecimal(BonValue.BonusAM);
                                DB.HISGLBenCharges.Add(GLBanChar1);
                            }
                        }
                    }
                    //*************Deduction*************
                    var HisDed = _filter.ListHisEmpDed;// DB.HISGenDeductions.ToList();
                    HisDed = HisDed.Where(w => w.INYear == _filter.InYear && w.INMonth == _filter.InMonth && _filter.Staff.EmpCode == w.EmpCode).ToList();
                    var GLDed = GLmappings.Where(w => w.BenGrp == "DED" && HisDed.Where(x => x.DedCode == w.BenCode).Any()).ToList();
                    if (GLDed.Count > 0)
                    {
                        LineItem = 0;
                        foreach (var item in GLDed)
                        {
                            var DedValue = HisDed.FirstOrDefault(w => w.DedCode == item.BenCode);
                            if (DedValue != null)
                            {
                                LineItem += 1;
                                var GLBanChar1 = new HISGLBenCharge();
                                SwapValueBenCharge(GLBanChar1, _filter, item, LineItem,true, true);
                                if (GLBanChar1.GLCode == null) continue;
                                GLBanChar1.IsPO = true;
                                GLBanChar1.IsDebitNote = true;
                                if (item.BenCode == "LATE" || item.BenCode == "EARLY")
                                    GLBanChar1.IsDebitNote = null;
                                GLBanChar1.Amount = Convert.ToDecimal(DedValue.DedAm);
                                DB.HISGLBenCharges.Add(GLBanChar1);
                            }
                        }
                    }
                    var row1 = DB.SaveChanges();
                }
                return SYConstant.OK;
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public void Get_CostCenter(GEN_Filter _filter)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                var His_Gen = DB.HisCostCharges.Where(w => w.EmpCode == _filter.EmpCode && w.InYear == _filter.InYear && w.InMonth == _filter.InMonth).ToList();
                if (His_Gen.Count() > 0)
                {
                    foreach (var item in His_Gen)
                    {
                        DB.HisCostCharges.Attach(item);
                        DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        var rowD = DB.SaveChanges();
                    }
                }
                var CostGroup = DB.PRCostCenterGroups.FirstOrDefault(w => w.Code == _filter.Staff.Costcent);
                if (CostGroup != null)
                {
                    var lstCost = DB.PRCostCenterGroupItems.Where(w => w.CodeCCGoup == _filter.Staff.Costcent).ToList();
                    foreach (var Item in lstCost)
                    {
                        var HisCost = new HisCostCharge();
                        HisCost.CodeCCGoup = CostGroup.Code;
                        HisCost.GroupDescription = CostGroup.Description;
                        HisCost.EmpCode = _filter.EmpCode;
                        HisCost.InYear = _filter.InYear;
                        HisCost.InMonth = _filter.InMonth;
                        HisCost.CostCenter = Item.CostCenterType;
                        HisCost.Description = Item.Description;
                        HisCost.Charge = Convert.ToDecimal(Item.Charge);
                        HisCost.CreatedBy = User.UserName;
                        HisCost.CreatedOn = DateTime.Now;
                        DB.HisCostCharges.Add(HisCost);
                    }
                }
                var row1 = DB.SaveChanges();
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string Generate_NSSF(int PeriodID, List<HRBranch> _listBranch)
        {
            DB = new HumicaDBContext();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                var Company = DMS.SYHRCompanies.First();
                var ExchangeRate = DB.PRExchRates.FirstOrDefault(w => w.PeriodID == PeriodID);
                if (ExchangeRate == null)
                {
                    return "EXCHANGERATE";
                }
                Filter.ExchangeRate = ExchangeRate.NSSFRate;
                if (Filter.ExchangeRate > 0)
                {
                    var _listEmp = DB.HRStaffProfiles.Where(w => w.ISNSSF == true).ToList();

                    _listEmp = _listEmp.Where(w => _listBranch.Where(x => x.Code == w.Branch).Any()).ToList();
                    if (Filter.Branch != null)
                    {
                        string[] Branch = Filter.Branch.Split(',');
                        List<string> LstBranch = new List<string>();
                        foreach (var read in Branch)
                        {
                            if (read.Trim() != "")
                            {
                                LstBranch.Add(read.Trim());
                            }
                        }
                        _listEmp = _listEmp.Where(w => LstBranch.Contains(w.Branch)).ToList();
                    }
                    var result = DB.HISGenSalaries.Where(w => w.PeriodID == PeriodID).ToList();
                    result = result.Where(w => _listEmp.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                    var _His_Gen = DB.HISGLBenCharges.Where(w => w.PeriodID == PeriodID && (w.BenCode == "_NSSF" || w.BenCode == "_PAYABLE")).ToList();
                    //var GLmappings = DB.PRGLmappings.ToList();
                    var GLmappings = DB.PRGLmappings.Where(w => w.BenGrp == "SYS").ToList();
                    var HRSetting = DB.SYHRSettings.FirstOrDefault();
                    decimal? GrossPay = 0;
                    decimal? maxContribution = 1200000;
                    decimal? minContribution = 400000;
                    if (HRSetting != null)
                    {
                        maxContribution = HRSetting.MaxContribute;
                        minContribution = HRSetting.MinContribue;
                    }
                    foreach (var item in result)
                    {
                        if (HRSetting.NSSFSalaryType == SalaryType.NP.ToString())
                        {
                            GrossPay = item.NetWage;
                        }
                        else if (HRSetting.NSSFSalaryType == SalaryType.GP.ToString())
                        {
                            GrossPay = item.GrossPay;
                        }
                        else GrossPay = item.Salary;

                        var rate = GrossPay * ExchangeRate.NSSFRate;
                        if (rate > maxContribution) rate = maxContribution;
                        else if (rate < minContribution) rate = minContribution;

                        if (HRSetting.ComRisk == null) HRSetting.ComRisk = 0;
                        if (HRSetting.ComHealthCare == null) HRSetting.ComHealthCare = 0;
                        item.SOSEC = Math.Round((decimal)(rate * HRSetting.ComRisk), 0);
                        item.AVGGrSOSC = rate;
                        item.NSSFRate = ExchangeRate.NSSFRate;
                        item.CompHealth = Math.Round((decimal)(rate * HRSetting.ComHealthCare), 0);
                        decimal TotalRisk = ClsRounding.Rounding(item.SOSEC.Value / ExchangeRate.NSSFRate, SYConstant.DECIMAL_PLACE, "N");
                        item.TotalRisk =item.StaffRisk + TotalRisk;
                        decimal TotalHealthCare = ClsRounding.Rounding(item.CompHealth.Value / ExchangeRate.NSSFRate, SYConstant.DECIMAL_PLACE, "N");
                        item.TotalHealthCare = item.StaffHealthCareUSD + TotalHealthCare;
                        DB.HISGenSalaries.Attach(item);
                        DB.Entry(item).Property(w => w.SOSEC).IsModified = true;
                        DB.Entry(item).Property(w => w.AVGGrSOSC).IsModified = true;
                        DB.Entry(item).Property(w => w.NSSFRate).IsModified = true;
                        DB.Entry(item).Property(w => w.CompHealth).IsModified = true;
                        DB.Entry(item).Property(w => w.StaffHealth).IsModified = true;
                        DB.Entry(item).Property(w => w.TotalRisk).IsModified = true;
                        DB.Entry(item).Property(w => w.TotalHealthCare).IsModified = true;

                        //*****************************

                        var His_Gen = _His_Gen.Where(w => w.EmpCode == item.EmpCode).ToList();
                        if (His_Gen.Count() > 0)
                        {
                            foreach (var item1 in His_Gen)
                            {
                                DB.HISGLBenCharges.Attach(item1);
                                DB.Entry(item1).State = System.Data.Entity.EntityState.Deleted;
                            }
                        }

                        var Staff = _listEmp.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                        if (Staff == null) continue;
                        var ListGL = GLmappings.Where(w => Staff.GrpGLAcc == w.Branch).ToList();
                        var GLMap = ListGL.FirstOrDefault(w => w.BenCode == "_NSSF");
                        if (GLMap != null && (item.TotalRisk + item.TotalHealthCare)>0)
                        {
                            int LineItem = 1;
                            var GLBanChar1 = new HISGLBenCharge();
                            GEN_Filter _filter = new GEN_Filter();
                            _filter.Staff = Staff;
                            _filter.CompanyCode = Company;
                            //_filter.InYear = InMonth.Year;
                            //_filter.InMonth = InMonth.Month;
                            //_filter.ToDate = InMonth;
                            SwapValueBenCharge(GLBanChar1, _filter, GLMap, LineItem,true);
                            GLBanChar1.Amount = item.TotalRisk + item.TotalHealthCare;
                            DB.HISGLBenCharges.Add(GLBanChar1);

                            var _GL1 = ListGL.FirstOrDefault(w => w.BenCode == "_PAYABLE");
                            if (_GL1 != null)
                            {
                                var GLBanChar2 = new HISGLBenCharge();
                                SwapValueBenCharge(GLBanChar2, _filter, _GL1, LineItem,false, true);
                                GLBanChar2.Amount = GLBanChar1.Amount;
                                DB.HISGLBenCharges.Add(GLBanChar2);
                            }
                        }
                    }
                    int row = DB.SaveChanges();
                }
                else
                {
                    return "EXCHANGERATE";
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Filter.INYear.ToString();
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
                //log.DocurmentAction = Filter.INYear.ToString();
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
                //log.DocurmentAction = Filter.INYear.ToString();
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
        public string Transfer_NSSF(int PeriodID, List<HRBranch> _listBranch)
        {
            DB = new HumicaDBContext();
            try
            {
                var ExchangeRate = DB.PRExchRates.FirstOrDefault(w => w.PeriodID == PeriodID);
                var Period = DB.PRPayPeriodItems.FirstOrDefault(w => w.PeriodID == PeriodID);
                if (ExchangeRate == null)
                {
                    return "EXCHANGERATE";
                }
                else if (ExchangeRate.NSSFRate == 0)
                {
                    return "EXCHANGERATE";
                }
                string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Content/TEMPLATE/E-Form_v10.accdb");

                OleDbCommand cmd = new OleDbCommand();
                OleDbCommand cmd1 = new OleDbCommand();
                //string Password = "P@$$wdEForm";
                //string myStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fileName};" +
                //       $"Jet OLEDB:Database Password={Password};Persist Security Info=True;";

                OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder
                {
                    Provider = "Microsoft.ACE.OLEDB.12.0",
                    DataSource = fileName,
                    PersistSecurityInfo = true
                };
                builder["Jet OLEDB:Database Password"] = "P@$$wdEForm";
                using (OleDbConnection Con = new OleDbConnection(builder.ConnectionString))
                {
                    Con.Open();
                    cmd.Connection = Con;
                    cmd.CommandText = "DELETE FROM TBL_BENEFICIARY";
                    cmd.ExecuteNonQuery();

                    string str = "";
                    var listBranch = DMS.HRBranches.ToList();
                    var listCompany = DMS.HRCompanies.ToList();
                    var _listNSFF = DBV.HR_NSSF_Transfer.Where(w => w.INYear == Period.EndDate.Year && w.INMonth == Period.EndDate.Month).ToList();
                    _listNSFF = _listNSFF.Where(w => _listBranch.Where(x => x.Code == w.Branch).Any()).ToList();
                    if (Filter.Branch != null)
                    {
                        string[] Branch = Filter.Branch.Split(',');
                        List<string> LstBranch = new List<string>();
                        foreach (var read in Branch)
                        {
                            if (read.Trim() != "")
                            {
                                LstBranch.Add(read.Trim());
                            }
                        }
                        _listNSFF = _listNSFF.Where(w => LstBranch.Contains(w.Branch)).ToList();
                        //_listNSFF = _listNSFF.Where(w => w.Branch == Filter.Branch).ToList();
                    }
                    _listNSFF = _listNSFF.Where(w => w.INYear == Period.EndDate.Year && w.INMonth == Period.EndDate.Month).ToList();
                    foreach (var item in _listNSFF)
                    {
                        int Sex = 0;
                        var DOB = item.DOB.Value.ToString("MM/dd/yyyy");
                        var StartDate = item.StartDate.Value.ToString("MM/dd/yyyy");
                        if (item.Sex == "M" || item.Sex == "Male") Sex = 1;
                        str = @"INSERT INTO TBL_BENEFICIARY(Enter_ID,NSSF_ID,ID_National,Ben_FName_kh,Ben_LName_kh,Ben_FName_Eng,Ben_LName_Eng,Sex,
                         Date_of_Birth, Nationality, Hired_Date, Ben_Group, Ben_Position,Ben_Sector, Ben_Status, Ben_Wage, Ben_WageInDollar,Ben_Assum, Ben_Contribution)
                         VALUES('" + item.EmpCode + "','" + item.SOCSO + "','" + item.IDCard + "','" + item.OthFirstName + "','" + item.OthLastName + "','" +
                             item.FirstName + "','" + item.LastName + "'," + Sex + ",'" + DOB + "','" + item.NATIONID + "','" + StartDate + "','" + item.DeptDesc + "','" +
                             item.PostDesc + "',1,0," + item.NSSFGROSS + "," + item.NSSFGROSSUSD + "," + item.AVGGrSOSC + "," + item.SOSEC + ")";
                        cmd.CommandText = str;
                        cmd.ExecuteNonQuery();
                    }
                    string enterID = "";
                    if (Filter.Branch != null)
                    {
                        var result = listBranch.FirstOrDefault(w => w.Code == Filter.Branch);
                        if (result != null)
                        {
                            var _company = listCompany.FirstOrDefault(w => w.Company == result.CompanyCode);
                            enterID = _company.NSSFNo;
                        }
                    }
                    else enterID = listCompany.FirstOrDefault().NSSFNo;

                    if (enterID == null)
                    {
                        return "INVALID_NSSF_NO";
                    }
                    cmd1 = new OleDbCommand("Select COUNT(*) from tbl_DateContribution", Con);
                    string cmdText = !Microsoft.VisualBasic.CompilerServices.Operators.ConditionalCompareObjectGreater(cmd1.ExecuteScalar(), 0, false) ? "Insert into tbl_DateContribution(Enter_ID,Month_Contribution,Year_Contribution,ExchangeRate) values(@enterID,@month,@year,@exchangerate)" : "Update tbl_DateContribution SET Enter_ID=@enterID, Month_Contribution = @month,Year_Contribution = @year,ExchangeRate=exchangerate";
                    cmd1 = new OleDbCommand(cmdText, Con);
                    cmd1.Parameters.Add("@enterID", OleDbType.VarChar).Value = enterID;
                    cmd1.Parameters.Add("@month", OleDbType.Integer).Value = Period.EndDate.Month;
                    cmd1.Parameters.Add("@year", OleDbType.Integer).Value = Period.EndDate.Year;
                    cmd1.Parameters.Add("@exchangerate", OleDbType.VarChar).Value = ExchangeRate.NSSFRate;
                    cmd1.ExecuteNonQuery();
                    Con.Close();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Filter.INYear.ToString();
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
                //log.DocurmentAction = Filter.INYear.ToString();
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
                //log.DocurmentAction = Filter.INYear.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string Generate_Senority(string EmpCode, FTFilerPayroll Filter1)
        {
            string ID = "";
            try
            {
                try
                {
                    DB = new HumicaDBContext();
                    DB.Configuration.AutoDetectChangesEnabled = false;
                    DateTime FromDate = new DateTime(Filter1.FromDate.Year, Filter1.FromDate.Month, 1);
                    DateTime ToDate = new DateTime(Filter1.ToDate.Year, Filter1.ToDate.Month, DateTime.DaysInMonth(Filter1.ToDate.Year, Filter1.ToDate.Month));
                    var Temp = DB.PRSincerities.ToList();
                    if (Temp.Count() > 0)
                    {
                        foreach (var item in Temp)
                        {
                            DB.PRSincerities.Attach(item);
                            DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                            var rowD = DB.SaveChanges();
                        }
                    }
                    string[] Emp = EmpCode.Split(';');
                    var ListSalary = DB.HISGenSalaries.ToList();
                    var ListStaff = DB.HRStaffProfiles.ToList();
                    var listParameter = DB.PRParameters.ToList();
                    var ListEmpCareer = DB.HREmpCareers.ToList();
                    ListStaff = ListStaff.ToList();
                    listParameter = listParameter.ToList();
                    ListSalary = ListSalary.Where(w => w.ToDate.Value.Date >= FromDate.Date && w.ToDate.Value.Date <= ToDate.Date).ToList();
                    var _StaffInPro = ListStaff.Where(w => w.Probation.Value.Date >= ToDate.Date.AddMonths(-1)).ToList();
                    ListSalary = ListSalary.Where(w => !_StaffInPro.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                    var _StaffPro = ListStaff.Where(w => w.Probation.Value.Date >= FromDate.Date && w.Probation.Value.Date <= ToDate.Date).ToList();
                    decimal RateInDate = 1.25M;

                    foreach (var Code in Emp)
                    {
                        decimal _SPrate = Filter1.Rate;
                        int CMonth = GetMonthDifference(FromDate, ToDate) + 1;
                        ID = Code;
                        if (Code.Trim() != "")
                        {
                            var Seniority = new PRSincerity();
                            var Staff = ListStaff.FirstOrDefault(w => w.EmpCode == Code);
                            var Parameter = listParameter.FirstOrDefault(w => w.Code == Staff.PayParam);
                            DateTime FFromDate = GetFromDate(Parameter, Filter1.ToDate);
                            DateTime FToDate = GetToDate(Parameter, Filter1.ToDate);

                            decimal DayInMonth = Get_WorkingDay_Salary(Parameter, FFromDate, FToDate);
                            decimal AVGSalary = 0;
                            decimal Rate = 0;
                            decimal Balance = 0;
                            var result = ListSalary.Where(w => w.EmpCode == Code).ToList();
                            if (result.Count() == 0) continue;
                            //Probation
                            if (_StaffPro.Where(w => w.EmpCode == Staff.EmpCode && w.Probation.Value.Year == FToDate.Year
                            && w.Probation.Value.Month == FToDate.Month).Any())
                            {
                                var _result = result.Where(w => w.INYear == Staff.Probation.Value.Year &&
                                  w.INMonth == Staff.Probation.Value.Month).ToList();
                                DateTime Prob = Staff.Probation.Value;
                                DateTime _ToDate = new DateTime(Prob.Year, Prob.Month, DateTime.DaysInMonth(Prob.Year, Prob.Month));
                                decimal _DayInMonth = Get_WorkingDay(Parameter, Prob, _ToDate);
                                if (_DayInMonth <= 21)
                                {
                                    result = result.Where(w => w.ToDate.Value.Date >= _ToDate.Date).ToList();
                                }
                                //else
                                //{
                                //    result = result.Where(w => w.FromDate.Value.Date >= _ToDate.Date).ToList();
                                //}
                                CMonth = result.Count;
                                _SPrate = RateInDate * CMonth;
                            }

                            #region "Resgin"
                            var LstEmpAllow = DB.PREmpAllws.ToList();
                            var LstEmpBon = DB.PREmpBonus.ToList();
                            decimal TempSalary = 0;
                            decimal Salary = 0;
                            decimal TempAmount = 0;
                            var _listGenD = new List<HISGenSalaryD>();
                            var LstEmpCareer = ListEmpCareer.Where(w => w.EmpCode == Code).ToList();
                            if (Staff.TerminateStatus == "TERMINAT" && Staff.SalaryFlag == false
                                && Staff.DateTerminate.Year == Filter1.ToDate.Year && Staff.DateTerminate.Month == Filter1.ToDate.Month)
                            {
                                //
                                result = result.Where(w => w.ToDate.Value.Date != ToDate.Date).ToList();

                                var Emp_C = from C in LstEmpCareer
                                            where ((C.FromDate >= FFromDate && C.FromDate <= ToDate) || (C.ToDate >= FFromDate && C.ToDate <= ToDate) ||
                                     (FFromDate >= C.FromDate && FFromDate <= C.ToDate) || (ToDate >= C.FromDate && ToDate <= C.ToDate))
                                     && C.EmpCode == Code && C.CareerCode != "TERMINAT"
                                            select new { C.CareerCode, C.EmpCode, C.NewSalary, C.FromDate, C.ToDate };
                                int C_Career = Emp_C.Count();
                                foreach (var emp in Emp_C)
                                {
                                    DateTime PToDate = emp.ToDate.Value;
                                    DateTime PFromDate = emp.FromDate.Value;
                                    if (PToDate > ToDate) PToDate = ToDate;
                                    if (PFromDate < FFromDate) PFromDate = FFromDate;
                                    Decimal ActualWorkDay = Get_WorkingDay(Parameter, PFromDate, PToDate);

                                    if (PFromDate == FFromDate && PToDate == ToDate)
                                    {
                                        TempSalary = emp.NewSalary;
                                    }
                                    else
                                    {
                                        decimal TMPD = 0;
                                        TMPD = Convert.ToDecimal(_listGenD.Sum(x => x.ActWorkDay));
                                        if ((TMPD + ActualWorkDay) > DayInMonth) ActualWorkDay = Convert.ToDecimal(DayInMonth) - TMPD;
                                        Rate = Convert.ToDecimal(emp.NewSalary / DayInMonth);
                                        TempSalary = ClsRounding.Rounding((Rate * ActualWorkDay), SYConstant.DECIMAL_PLACE, "N");
                                    }
                                    Salary += TempSalary;
                                }

                                var EmpAllw = LstEmpAllow.Where(w => w.EmpCode == Code && w.FromDate.Value.Year == ToDate.Year && w.FromDate.Value.Month == ToDate.Month &&
                                   w.ToDate.Value.Year == ToDate.Year && w.ToDate.Value.Month == ToDate.Month && w.Status == 1).ToList();
                                Salary += Convert.ToDecimal(EmpAllw.Where(w => w.AllwCode != "SP").Sum(w => w.Amount));

                                #region ********Allw Period********
                                var EmpAllwP = LstEmpAllow.Where(w => w.EmpCode == Code && ((FFromDate >= w.FromDate && FFromDate <= w.ToDate) || (ToDate >= w.FromDate && ToDate <= w.ToDate))
                                && w.Status == 0).ToList();
                                foreach (var Allw in EmpAllwP.Where(w => w.AllwCode != "SP"))
                                {
                                    var ActualWorkDay = Get_WorkingDay_Allw(Parameter, FFromDate, ToDate);
                                    DateTime TempGenFromDate = new DateTime();
                                    DateTime TempGenToDate = new DateTime();
                                    int HasAllowanceDetail = 0;
                                    if (FFromDate >= Allw.FromDate && ToDate <= Allw.ToDate)
                                    {
                                        TempGenFromDate = FFromDate;
                                        TempGenToDate = ToDate;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (FFromDate >= Allw.FromDate && Allw.ToDate <= ToDate)
                                    {
                                        TempGenFromDate = FFromDate;
                                        TempGenToDate = Allw.ToDate.Value;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (Allw.FromDate >= FFromDate && Allw.FromDate <= ToDate && Allw.ToDate >= ToDate)
                                    {
                                        TempGenFromDate = Allw.FromDate.Value;
                                        TempGenToDate = ToDate;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (Allw.FromDate >= FFromDate && Allw.ToDate <= ToDate)
                                    {
                                        TempGenFromDate = Allw.FromDate.Value;
                                        TempGenToDate = Allw.ToDate.Value;
                                        HasAllowanceDetail = 1;
                                    }
                                    if (HasAllowanceDetail == 1)
                                    {
                                        decimal TempWorkDay = Get_WorkingDay(Parameter, TempGenFromDate, TempGenToDate);
                                        if (TempGenFromDate == FFromDate && TempGenToDate == ToDate)
                                            TempAmount = Convert.ToDecimal(Allw.Amount);
                                        else
                                            TempAmount = Convert.ToDecimal((Allw.Amount / ActualWorkDay) * TempWorkDay);
                                    }
                                    Salary += TempAmount;
                                }
                                #endregion

                                TempAmount = 0;
                                var EmpBon = LstEmpBon.Where(w => w.EmpCode == Code && w.FromDate.Value.Year == ToDate.Year && w.FromDate.Value.Month == ToDate.Month &&
                                   w.ToDate.Value.Year == ToDate.Year && w.ToDate.Value.Month == ToDate.Month && w.Status == 1).ToList();
                                Salary += EmpBon.Sum(w => w.Amount);

                                #region ********Bon Period********
                                var EmpBonP = LstEmpBon.Where(w => w.EmpCode == Code && ((FFromDate >= w.FromDate && FFromDate <= w.ToDate) || (ToDate >= w.FromDate && ToDate <= w.ToDate))
                                && w.Status == 0).ToList();
                                foreach (var Allw in EmpBonP)
                                {
                                    var ActualWorkDay = Get_WorkingDay_Allw(Parameter, FFromDate, ToDate);
                                    DateTime TempGenFromDate = new DateTime();
                                    DateTime TempGenToDate = new DateTime();
                                    int HasAllowanceDetail = 0;
                                    if (FFromDate >= Allw.FromDate && ToDate <= Allw.ToDate)
                                    {
                                        TempGenFromDate = FFromDate;
                                        TempGenToDate = ToDate;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (FFromDate >= Allw.FromDate && Allw.ToDate <= ToDate)
                                    {
                                        TempGenFromDate = FFromDate;
                                        TempGenToDate = Allw.ToDate.Value;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (Allw.FromDate >= FFromDate && Allw.FromDate <= ToDate && Allw.ToDate >= ToDate)
                                    {
                                        TempGenFromDate = Allw.FromDate.Value;
                                        TempGenToDate = ToDate;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (Allw.FromDate >= FFromDate && Allw.ToDate <= ToDate)
                                    {
                                        TempGenFromDate = Allw.FromDate.Value;
                                        TempGenToDate = Allw.ToDate.Value;
                                        HasAllowanceDetail = 1;
                                    }
                                    if (HasAllowanceDetail == 1)
                                    {
                                        decimal TempWorkDay = Get_WorkingDay(Parameter, TempGenFromDate, TempGenToDate);
                                        if (TempGenFromDate == FFromDate && TempGenToDate == ToDate)
                                            TempAmount = Convert.ToDecimal(Allw.Amount);
                                        else
                                            TempAmount = Convert.ToDecimal((Allw.Amount / ActualWorkDay) * TempWorkDay);
                                        Salary += TempAmount;
                                    }
                                }
                                #endregion


                                TempAmount = 0;
                                var EmpDed = DB.PREmpDeducs.Where(w => w.EmpCode == Code && w.FromDate.Value.Year == ToDate.Year && w.FromDate.Value.Month == ToDate.Month &&
                                   w.ToDate.Value.Year == ToDate.Year && w.ToDate.Value.Month == ToDate.Month && w.Status == 1).ToList();

                                Salary += Convert.ToDecimal(EmpDed.Sum(w => w.Amount));

                                #region ********Deducs Period ********
                                var EmpDedP = DB.PREmpDeducs.Where(w => w.EmpCode == Code && ((FFromDate >= w.FromDate && FFromDate <= w.ToDate) || (ToDate >= w.FromDate && ToDate <= w.ToDate))
                                && w.Status == 0).ToList();
                                foreach (var Ded in EmpDedP)
                                {
                                    var ActualWorkDay = Get_WorkingDay_Ded(Parameter, FFromDate, ToDate);
                                    DateTime TempGenFromDate = new DateTime();
                                    DateTime TempGenToDate = new DateTime();
                                    int HasAllowanceDetail = 0;
                                    if (FFromDate >= Ded.FromDate && ToDate <= Ded.ToDate)
                                    {
                                        TempGenFromDate = FFromDate;
                                        TempGenToDate = ToDate;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (FFromDate >= Ded.FromDate && Ded.ToDate <= ToDate)
                                    {
                                        TempGenFromDate = FFromDate;
                                        TempGenToDate = Ded.ToDate.Value;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (Ded.FromDate >= FFromDate && Ded.FromDate <= ToDate && Ded.ToDate >= ToDate)
                                    {
                                        TempGenFromDate = Ded.FromDate.Value;
                                        TempGenToDate = ToDate;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (Ded.FromDate >= FFromDate && Ded.ToDate <= ToDate)
                                    {
                                        TempGenFromDate = Ded.FromDate.Value;
                                        TempGenToDate = Ded.ToDate.Value;
                                        HasAllowanceDetail = 1;
                                    }
                                    if (HasAllowanceDetail == 1)
                                    {
                                        decimal TempWorkDay = Get_WorkingDay(Parameter, TempGenFromDate, TempGenToDate);
                                        if (TempGenFromDate == FFromDate && TempGenToDate == ToDate)
                                            TempAmount = Convert.ToDecimal(Ded.Amount);
                                        else
                                            TempAmount = Convert.ToDecimal((Ded.Amount / ActualWorkDay) * TempWorkDay);
                                        Salary += TempAmount;
                                    }
                                }
                                #endregion

                            }
                            #endregion

                            if (result.Count > 0)
                            {

                                if (Filter.SalaryType == "BS")
                                {
                                    AVGSalary = Convert.ToDecimal(result.Sum(x => x.Salary) / CMonth);
                                }
                                else if (Filter.SalaryType == "GP")
                                {
                                    var _Seniority = result.Where(w => w.INYear == ToDate.Year && w.INMonth == ToDate.Month).Sum(x => x.Seniority);

                                    AVGSalary = (Convert.ToDecimal(result.Sum(x => x.GrossPay) + Salary - _Seniority) / CMonth);
                                }
                                else
                                {
                                    AVGSalary = (Convert.ToDecimal(result.Sum(x => x.NetWage + x.FirstPaymentAmount) + Salary) / CMonth);
                                }
                                AVGSalary = ClsRounding.Rounding(AVGSalary, SYConstant.DECIMAL_PLACE, "N");
                                Rate = AVGSalary / DayInMonth;
                                Rate = ClsRounding.Rounding(Rate, SYConstant.DECIMAL_PLACE, "N");
                                Balance = Rate * _SPrate;
                            }
                            Seniority.InYear = Filter1.ToDate.Year;
                            Seniority.InMonth = Filter1.ToDate.Month;
                            Seniority.EmpCode = Code;
                            Seniority.FromDate = FromDate;
                            Seniority.ToDate = ToDate;
                            Seniority.Salary = AVGSalary;
                            Seniority.Rate = Rate;
                            Seniority.TotalBalance = ClsRounding.Rounding(Balance, SYConstant.DECIMAL_PLACE, "N");
                            DB.PRSincerities.Add(Seniority);
                        }
                    }
                    DB.SaveChanges();
                    return SYConstant.OK;
                }
                finally
                {
                    DB.Configuration.AutoDetectChangesEnabled = true;
                }
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ID;
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
                log.DocurmentAction = ID;
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
                log.DocurmentAction = ID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string Generate_Severance(string EmpCode, FTFilterEmployee Filter1)
        {
            try
            {
                try
                {
                    DB = new HumicaDBContext();
                    DB.Configuration.AutoDetectChangesEnabled = false;
                    DateTime FromDate = new DateTime(Filter1.FromDate.Year, Filter1.FromDate.Month, 1);
                    DateTime ToDate = new DateTime(Filter1.ToDate.Year, Filter1.ToDate.Month, DateTime.DaysInMonth(Filter1.ToDate.Year, Filter1.ToDate.Month));
                    var Temp = DB.PRSeverancePays.ToList();
                    if (Temp.Count() > 0)
                    {
                        foreach (var item in Temp)
                        {
                            DB.PRSeverancePays.Attach(item);
                            DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                            var rowD = DB.SaveChanges();
                        }
                    }
                    string[] Emp = EmpCode.Split(';');
                    var ListSalary = DB.HISGenSalaries.ToList();
                    var ListStaff = DB.HRStaffProfiles.ToList();
                    var listParameter = DB.PRParameters.ToList();
                    var ListEmpCareer = DB.HREmpCareers.ToList();
                    ListStaff = ListStaff.ToList();
                    listParameter = listParameter.ToList();
                    ListSalary = ListSalary.Where(w => w.ToDate.Value.Date >= FromDate.Date && w.ToDate.Value.Date <= ToDate.Date).ToList();
                    int CMonth = GetMonthDifference(FromDate, ToDate) + 1;

                    var _StaffPro = ListStaff.Where(w => w.Probation.Value.Date >= FromDate.Date && w.Probation.Value.Date <= ToDate.Date).ToList();
                    decimal RateInDate = 1.25M;
                    foreach (var Code in Emp)
                    {
                        if (Code.Trim() != "")
                        {
                            var Seniority = new PRSincerity();
                            var Staff = ListStaff.FirstOrDefault(w => w.EmpCode == Code);
                            var Parameter = listParameter.FirstOrDefault(w => w.Code == Staff.PayParam);
                            DateTime FFromDate = GetFromDate(Parameter, Filter1.ToDate);
                            DateTime FToDate = GetToDate(Parameter, Filter1.ToDate);

                            decimal DayInMonth = Get_WorkingDay_Salary(Parameter, FFromDate, FToDate);
                            decimal AVGSalary = 0;
                            decimal Rate = 0;
                            decimal Balance = 0;
                            var result = ListSalary.Where(w => w.EmpCode == Code).ToList();

                            //Probation
                            if (_StaffPro.Where(w => w.EmpCode == Staff.EmpCode).Any())
                            {
                                var _result = result.Where(w => w.INYear == Staff.Probation.Value.Year &&
                                  w.INMonth == Staff.Probation.Value.Month).ToList();
                                DateTime Prob = Staff.Probation.Value;
                                DateTime _ToDate = new DateTime(Prob.Year, Prob.Month, DateTime.DaysInMonth(Prob.Year, Prob.Month));
                                decimal _DayInMonth = Get_WorkingDay(Parameter, Prob, _ToDate);
                                if (_DayInMonth >= 21)
                                {
                                    result = result.Where(w => w.ToDate.Value.Date >= _ToDate.Date).ToList();
                                }
                                else
                                {
                                    result = result.Where(w => w.FromDate.Value.Date >= _ToDate.Date).ToList();
                                }
                                CMonth = result.Count;
                                Filter1.Rate = RateInDate * CMonth;
                            }

                            #region "Resgin"
                            var LstEmpAllow = DB.PREmpAllws.ToList();
                            var LstEmpBon = DB.PREmpBonus.ToList();
                            decimal TempSalary = 0;
                            decimal Salary = 0;
                            decimal TempAmount = 0;
                            var _listGenD = new List<HISGenSalaryD>();
                            var LstEmpCareer = ListEmpCareer.Where(w => w.EmpCode == Code).ToList();
                            if (Staff.TerminateStatus == "TERMINAT" && Staff.SalaryFlag == false
                                && Staff.DateTerminate.Year == Filter1.ToDate.Year && Staff.DateTerminate.Month == Filter1.ToDate.Month)
                            {
                                //
                                result = result.Where(w => w.ToDate.Value.Date != ToDate.Date).ToList();

                                var Emp_C = from C in LstEmpCareer
                                            where ((C.FromDate >= FFromDate && C.FromDate <= ToDate) || (C.ToDate >= FFromDate && C.ToDate <= ToDate) ||
                                     (FFromDate >= C.FromDate && FFromDate <= C.ToDate) || (ToDate >= C.FromDate && ToDate <= C.ToDate))
                                     && C.EmpCode == Code && C.CareerCode != "TERMINAT"
                                            select new { C.CareerCode, C.EmpCode, C.NewSalary, C.FromDate, C.ToDate };
                                int C_Career = Emp_C.Count();
                                foreach (var emp in Emp_C)
                                {
                                    DateTime PToDate = emp.ToDate.Value;
                                    DateTime PFromDate = emp.FromDate.Value;
                                    if (PToDate > ToDate) PToDate = ToDate;
                                    if (PFromDate < FFromDate) PFromDate = FFromDate;
                                    Decimal ActualWorkDay = Get_WorkingDay(Parameter, PFromDate, PToDate);

                                    if (PFromDate == FFromDate && PToDate == ToDate)
                                    {
                                        TempSalary = emp.NewSalary;
                                    }
                                    else
                                    {
                                        decimal TMPD = 0;
                                        TMPD = Convert.ToDecimal(_listGenD.Sum(x => x.ActWorkDay));
                                        if ((TMPD + ActualWorkDay) > DayInMonth) ActualWorkDay = Convert.ToDecimal(DayInMonth) - TMPD;
                                        Rate = Convert.ToDecimal(emp.NewSalary / DayInMonth);
                                        TempSalary = Math.Round((Rate * ActualWorkDay), 2);
                                    }
                                    Salary += TempSalary;
                                }

                                var EmpAllw = LstEmpAllow.Where(w => w.EmpCode == Code && w.FromDate.Value.Year == ToDate.Year && w.FromDate.Value.Month == ToDate.Month &&
                                   w.ToDate.Value.Year == ToDate.Year && w.ToDate.Value.Month == ToDate.Month && w.Status == 1).ToList();
                                Salary += Convert.ToDecimal(EmpAllw.Where(w => w.AllwCode != "SP").Sum(w => w.Amount));

                                #region ********Allw Period********
                                var EmpAllwP = LstEmpAllow.Where(w => w.EmpCode == Code && ((FFromDate >= w.FromDate && FFromDate <= w.ToDate) || (ToDate >= w.FromDate && ToDate <= w.ToDate))
                                && w.Status == 0).ToList();
                                foreach (var Allw in EmpAllwP.Where(w => w.AllwCode != "SP"))
                                {
                                    var ActualWorkDay = Get_WorkingDay_Allw(Parameter, FFromDate, ToDate);
                                    DateTime TempGenFromDate = new DateTime();
                                    DateTime TempGenToDate = new DateTime();
                                    int HasAllowanceDetail = 0;
                                    if (FFromDate >= Allw.FromDate && ToDate <= Allw.ToDate)
                                    {
                                        TempGenFromDate = FFromDate;
                                        TempGenToDate = ToDate;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (FFromDate >= Allw.FromDate && Allw.ToDate <= ToDate)
                                    {
                                        TempGenFromDate = FFromDate;
                                        TempGenToDate = Allw.ToDate.Value;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (Allw.FromDate >= FFromDate && Allw.FromDate <= ToDate && Allw.ToDate >= ToDate)
                                    {
                                        TempGenFromDate = Allw.FromDate.Value;
                                        TempGenToDate = ToDate;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (Allw.FromDate >= FFromDate && Allw.ToDate <= ToDate)
                                    {
                                        TempGenFromDate = Allw.FromDate.Value;
                                        TempGenToDate = Allw.ToDate.Value;
                                        HasAllowanceDetail = 1;
                                    }
                                    if (HasAllowanceDetail == 1)
                                    {
                                        decimal TempWorkDay = Get_WorkingDay(Parameter, TempGenFromDate, TempGenToDate);
                                        if (TempGenFromDate == FFromDate && TempGenToDate == ToDate)
                                            TempAmount = Convert.ToDecimal(Allw.Amount);
                                        else
                                            TempAmount = Convert.ToDecimal((Allw.Amount / ActualWorkDay) * TempWorkDay);
                                    }
                                    Salary += TempAmount;
                                }
                                #endregion

                                TempAmount = 0;
                                var EmpBon = LstEmpBon.Where(w => w.EmpCode == Code && w.FromDate.Value.Year == ToDate.Year && w.FromDate.Value.Month == ToDate.Month &&
                                   w.ToDate.Value.Year == ToDate.Year && w.ToDate.Value.Month == ToDate.Month && w.Status == 1).ToList();
                                Salary += EmpBon.Sum(w => w.Amount);

                                #region ********Bon Period********
                                var EmpBonP = LstEmpBon.Where(w => w.EmpCode == Code && ((FFromDate >= w.FromDate && FFromDate <= w.ToDate) || (ToDate >= w.FromDate && ToDate <= w.ToDate))
                                && w.Status == 0).ToList();
                                foreach (var Allw in EmpBonP)
                                {
                                    var ActualWorkDay = Get_WorkingDay_Allw(Parameter, FFromDate, ToDate);
                                    DateTime TempGenFromDate = new DateTime();
                                    DateTime TempGenToDate = new DateTime();
                                    int HasAllowanceDetail = 0;
                                    if (FFromDate >= Allw.FromDate && ToDate <= Allw.ToDate)
                                    {
                                        TempGenFromDate = FFromDate;
                                        TempGenToDate = ToDate;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (FFromDate >= Allw.FromDate && Allw.ToDate <= ToDate)
                                    {
                                        TempGenFromDate = FFromDate;
                                        TempGenToDate = Allw.ToDate.Value;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (Allw.FromDate >= FFromDate && Allw.FromDate <= ToDate && Allw.ToDate >= ToDate)
                                    {
                                        TempGenFromDate = Allw.FromDate.Value;
                                        TempGenToDate = ToDate;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (Allw.FromDate >= FFromDate && Allw.ToDate <= ToDate)
                                    {
                                        TempGenFromDate = Allw.FromDate.Value;
                                        TempGenToDate = Allw.ToDate.Value;
                                        HasAllowanceDetail = 1;
                                    }
                                    if (HasAllowanceDetail == 1)
                                    {
                                        decimal TempWorkDay = Get_WorkingDay(Parameter, TempGenFromDate, TempGenToDate);
                                        if (TempGenFromDate == FFromDate && TempGenToDate == ToDate)
                                            TempAmount = Convert.ToDecimal(Allw.Amount);
                                        else
                                            TempAmount = Convert.ToDecimal((Allw.Amount / ActualWorkDay) * TempWorkDay);
                                        Salary += TempAmount;
                                    }
                                }
                                #endregion


                                TempAmount = 0;
                                var EmpDed = DB.PREmpDeducs.Where(w => w.EmpCode == Code && w.FromDate.Value.Year == ToDate.Year && w.FromDate.Value.Month == ToDate.Month &&
                                   w.ToDate.Value.Year == ToDate.Year && w.ToDate.Value.Month == ToDate.Month && w.Status == 1).ToList();

                                Salary += Convert.ToDecimal(EmpDed.Sum(w => w.Amount));

                                #region ********Deducs Period ********
                                var EmpDedP = DB.PREmpDeducs.Where(w => w.EmpCode == Code && ((FFromDate >= w.FromDate && FFromDate <= w.ToDate) || (ToDate >= w.FromDate && ToDate <= w.ToDate))
                                && w.Status == 0).ToList();
                                foreach (var Ded in EmpDedP)
                                {
                                    var ActualWorkDay = Get_WorkingDay_Ded(Parameter, FFromDate, ToDate);
                                    DateTime TempGenFromDate = new DateTime();
                                    DateTime TempGenToDate = new DateTime();
                                    int HasAllowanceDetail = 0;
                                    if (FFromDate >= Ded.FromDate && ToDate <= Ded.ToDate)
                                    {
                                        TempGenFromDate = FFromDate;
                                        TempGenToDate = ToDate;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (FFromDate >= Ded.FromDate && Ded.ToDate <= ToDate)
                                    {
                                        TempGenFromDate = FFromDate;
                                        TempGenToDate = Ded.ToDate.Value;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (Ded.FromDate >= FFromDate && Ded.FromDate <= ToDate && Ded.ToDate >= ToDate)
                                    {
                                        TempGenFromDate = Ded.FromDate.Value;
                                        TempGenToDate = ToDate;
                                        HasAllowanceDetail = 1;
                                    }
                                    else if (Ded.FromDate >= FFromDate && Ded.ToDate <= ToDate)
                                    {
                                        TempGenFromDate = Ded.FromDate.Value;
                                        TempGenToDate = Ded.ToDate.Value;
                                        HasAllowanceDetail = 1;
                                    }
                                    if (HasAllowanceDetail == 1)
                                    {
                                        decimal TempWorkDay = Get_WorkingDay(Parameter, TempGenFromDate, TempGenToDate);
                                        if (TempGenFromDate == FFromDate && TempGenToDate == ToDate)
                                            TempAmount = Convert.ToDecimal(Ded.Amount);
                                        else
                                            TempAmount = Convert.ToDecimal((Ded.Amount / ActualWorkDay) * TempWorkDay);
                                        Salary += TempAmount;
                                    }
                                }
                                #endregion

                            }
                            #endregion

                            if (result.Count > 0)
                            {

                                if (Filter.SalaryType == "BS")
                                {
                                    AVGSalary = Convert.ToDecimal(result.Sum(x => x.Salary) / CMonth);
                                }
                                else if (Filter.SalaryType == "GP")
                                {
                                    AVGSalary = (Convert.ToDecimal(result.Sum(x => x.GrossPay) + Salary) / CMonth);
                                }
                                else
                                {
                                    AVGSalary = (Convert.ToDecimal(result.Sum(x => x.NetWage) + Salary) / CMonth); ;
                                }
                                AVGSalary = Math.Round(AVGSalary, 2);
                                Rate = AVGSalary / DayInMonth;
                                Rate = Math.Round(Rate, 2);
                                Balance = Rate * Filter1.Rate;
                            }
                            Seniority.InYear = Filter1.ToDate.Year;
                            Seniority.InMonth = Filter1.ToDate.Month;
                            Seniority.EmpCode = Code;
                            Seniority.FromDate = FromDate;
                            Seniority.ToDate = ToDate;
                            Seniority.Salary = AVGSalary;
                            Seniority.Rate = Rate;
                            Seniority.TotalBalance = Balance;
                            DB.PRSincerities.Add(Seniority);
                        }
                    }
                    DB.SaveChanges();
                    return SYConstant.OK;
                }
                finally
                {
                    DB.Configuration.AutoDetectChangesEnabled = true;
                }
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Filter.INYear.ToString();
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
                //log.DocurmentAction = Filter.INYear.ToString();
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
                //log.DocurmentAction = Filter.INYear.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string TransferSenority(string EmpCode, DateTime InMonth)
        {
            try
            {
                SYHRSetting Setting = DB.SYHRSettings.FirstOrDefault();
                if (Setting == null)
                    return "INVALTID_ALLOWANCE";
                var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "ALLW").ToList();
                var AllwType = RewardType.FirstOrDefault(w => w.Code == Setting.SeniorityType);
                if (AllwType == null)
                {
                    return "INVALTID_ALLOWANCE";
                }

                var DBI = new HumicaDBContext();
                var ListEmpAllw = DB.PREmpAllws.ToList();
                var ID = ListEmpAllw.OrderByDescending(u => u.TranNo).FirstOrDefault();
                string[] _TranNo = EmpCode.Split(';');
                DateTime FromDate = new DateTime(InMonth.Year, InMonth.Month, 1);
                DateTime ToDate = new DateTime(InMonth.Year, InMonth.Month, DateTime.DaysInMonth(InMonth.Year, InMonth.Month));
                //var TempAllw = DB.PRSincerities.ToList();
                //if (TempAllw.Count > 0)
                //{
                foreach (var Code in _TranNo)
                {
                    //var _TBA = TempAllw.Where(w => w.EmpCode == Code).ToList();
                    var ObjUpdate = ListEmpSeniority.FirstOrDefault(w => w.EmpCode == Code);
                    if (ObjUpdate != null)
                    {
                        //var ObjUpdate = _TBA.First();
                        var _empAllw = ListEmpAllw.FirstOrDefault(w => w.EmpCode == Code && w.AllwCode == AllwType.Code &&
           ((FromDate.Date >= w.FromDate.Value.Date && FromDate.Date <= w.ToDate.Value.Date) || (ToDate.Date >= w.FromDate.Value.Date && ToDate.Date <= w.ToDate.Value.Date)));

                        if (_empAllw != null)
                        {
                            DBI.PREmpAllws.Attach(_empAllw);
                            DBI.Entry(_empAllw).State = System.Data.Entity.EntityState.Deleted;
                        }
                        if (ObjUpdate.TotalAmount > 0)
                        {
                            var Header = new PREmpAllw();
                            Header.TranNo = ID.TranNo + 1;
                            Header.EmpCode = Code;
                            Header.AllwCode = AllwType.Code;
                            Header.AllwDescription = AllwType.Description;
                            Header.FromDate = FromDate;
                            Header.ToDate = ToDate;
                            Header.Remark = "Transfer from system";
                            Header.Amount = Convert.ToDecimal(ObjUpdate.TotalAmount);
                            Header.Status = 1;

                            //ObjUpdate.Remark = "Transfer on " + DateTime.Now.ToString("dd.MM.yyyy");

                            DB.PREmpAllws.Add(Header);
                            //DB.PRSincerities.Attach(ObjUpdate);
                            //DB.Entry(ObjUpdate).Property(w => w.Remark).IsModified = true;
                        }
                    }
                }
                //}
                var rowD = DBI.SaveChanges();
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
        public decimal Get_WorkingDay(PRParameter PayPram, DateTime FromDate, DateTime ToDate)
        {
            decimal? Result = 0;
            DateTime TempDate = new DateTime();
            if (PayPram != null)
            {
                int Count = 0;
                //for (int i = FromDate.Day; i <= ToDate.Day; i++)
                for (var day = FromDate.Date; day.Date <= ToDate.Date; day = day.AddDays(1))
                {
                    //TempDate = FromDate.AddDays(Count);
                    TempDate = day;
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
        public decimal Get_WorkingDay_Salary(PRParameter PayPram, DateTime FromDate, DateTime ToDate)
        {
            decimal Result = 0;
            if (PayPram.SALWKTYPE == 1)
                Result = Get_WorkingDay(PayPram, FromDate, ToDate);
            else if (PayPram.SALWKTYPE == 2)
                Result = ToDate.Subtract(FromDate).Days + 1;
            else if (PayPram.SALWKTYPE == 3)
                Result = Convert.ToInt32(PayPram.SALWKVAL);
            return Result;
        }
        public decimal Get_WorkingDay_Allw(PRParameter PayPram, DateTime FromDate, DateTime ToDate)
        {
            decimal Result = 0;
            if (PayPram.ALWTYPE == 1)
                Result = Get_WorkingDay(PayPram, FromDate, ToDate);
            else if (PayPram.ALWTYPE == 2)
                Result = ToDate.Subtract(FromDate).Days + 1;
            else if (PayPram.ALWTYPE == 3)
                Result = Convert.ToDecimal(PayPram.ALWVAL);
            return Result;
        }
        public decimal Get_WorkingDay_Ded(PRParameter PayPram, DateTime FromDate, DateTime ToDate)
        {
            decimal Result = 0;
            if (PayPram.DEDTYPE == 1)
                Result = Get_WorkingDay(PayPram, FromDate, ToDate);
            else if (PayPram.DEDTYPE == 2)
                Result = ToDate.Subtract(FromDate).Days + 1;
            else if (PayPram.DEDTYPE == 3)
                Result = Convert.ToInt32(PayPram.DEDVAL);
            return Result;
        }
        public DateTime GetFromDate(PRParameter Parameter, DateTime InDate)
        {
            DateTime FromDate = InDate.Date;
            DateTime tempFromDate = FromDate;
            DateTime ToDate = DateTimeHelper.DateInMonth(InDate);

            if (!Parameter.IsPrevoiuseMonth.IsNullOrZero())
            {
                DateTime tempDate = tempFromDate.AddMonths(-1);
                FromDate = new DateTime(tempDate.Year, tempDate.Month, Parameter.FROMDATE.Value().Day);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, Parameter.TODATE.Value().Day);
            }

            DateTime Result = new DateTime();
            if (FromDate.Year != ToDate.Year)
            {
                if (FromDate.Month > ToDate.Month)
                {
                    int Month = InDate.Month - 1;
                    int Year = InDate.Year;
                    if (Month == 0)
                    {
                        Month = 12;
                        Year -= 1;
                    }
                    Result = new DateTime(Year, Month, FromDate.Day);
                }
            }
            else if (FromDate.Month < ToDate.Month)
            {
                int Month = InDate.Month - 1;
                int Year = InDate.Year;
                if (Month == 0)
                {
                    Month = 12;
                    Year -= 1;
                }
                Result = new DateTime(Year, Month, FromDate.Day);
            }

            else Result = new DateTime(InDate.Year, InDate.Month, FromDate.Day);
            return Result;
        }
        public DateTime GetToDate(PRParameter Parameter, DateTime InDate)
        {
            DateTime Result = new DateTime();
            DateTime ToDate = DateTimeHelper.DateInMonth(InDate);
            if (!Parameter.IsPrevoiuseMonth.IsNullOrZero())
            {
                ToDate = new DateTime(ToDate.Year, ToDate.Month, Parameter.TODATE.Value().Day);
            }
            Result = ToDate;
            return Result;
        }
        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
        #region Approval Salary
        public string CreateAppSalary()
        {
            try
            {
                var DBR = new HumicaDBContext();
                var objCF = DB.ExCfWorkFlowItems.Find(ScreenId, HeaderAppSalary.DocumentType);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }

                var objNumber = new CFNumberRank(HeaderAppSalary.DocumentType, ScreenId);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                HeaderAppSalary.ASNumber = objNumber.NextNumberRank.Trim();
                HeaderAppSalary.Status = SYDocumentStatus.OPEN.ToString(); ;
                //Add approver
                foreach (var read in ListApproval)
                {
                    read.ID = 0;
                    read.LastChangedDate = DateTime.Now;
                    read.DocumentNo = HeaderAppSalary.ASNumber;
                    read.DocumentType = HeaderAppSalary.DocumentType;
                    read.Status = SYDocumentStatus.OPEN.ToString();
                    read.WFObject = objCF.ApprovalFlow;
                    read.ApprovedBy = "";
                    read.ApprovedName = "";
                    read.ApproverName = "";
                    var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                    if (objStaff != null)
                    {
                        read.ApproverName = objStaff.AllName;
                    }
                    DB.ExDocApprovals.Add(read);
                }
                var objUpdate = DBR.HisPendingAppSalaries.FirstOrDefault(w => w.PeriodID == HeaderAppSalary.PeriodID);

                HeaderAppSalary.InYear = objUpdate.ToDate.Year;
                HeaderAppSalary.InMonth = objUpdate.ToDate.Month;
                HeaderAppSalary.PayInMonth = new DateTime(objUpdate.ToDate.Year, objUpdate.ToDate.Month, 1);
                HeaderAppSalary.CreatedOn = DateTime.Now;
                HeaderAppSalary.CreatedBy = User.UserName;

                DB.HISApproveSalaries.Add(HeaderAppSalary);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderAppSalary.ASNumber;
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
                log.DocurmentAction = HeaderAppSalary.ASNumber;
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
                log.DocurmentAction = HeaderAppSalary.ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string requestToApprove(string id)
        {
            try
            {
                HumicaDBContext DBX = new HumicaDBContext();
                var objMatch = DB.HISApproveSalaries.Find(id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                HeaderAppSalary = objMatch;

                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }

                objMatch.Status = SYDocumentStatus.PENDING.ToString();
                DB.HISApproveSalaries.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                int row = DBX.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = HeaderAppSalary.ASNumber;
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
                log.DocurmentAction = HeaderAppSalary.ASNumber;
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
                log.DocurmentAction = HeaderAppSalary.ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string approveTheDoc(int id)
        {
            try
            {
                HumicaDBContext DBX = new HumicaDBContext();
                var objMatch = DB.HISApproveSalaries.FirstOrDefault(w => w.PeriodID == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                HeaderAppSalary = objMatch;

                if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                {
                    return "INV_DOC";
                }
                string open = SYDocumentStatus.OPEN.ToString();
                var listApproval = DBX.ExDocApprovals.Where(w => w.DocumentType == objMatch.DocumentType &&
                                                  w.DocumentNo == objMatch.ASNumber && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                var b = false;
                var approverLevel = 0;
                foreach (var read in listApproval)
                {
                    approverLevel = read.ApproveLevel;
                    ApproveLevel = read.ApproveLevel;
                    var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
                    if (checklist.Count > 0)
                    {
                        if (read.Status == SYDocumentStatus.APPROVED.ToString())
                        {
                            return "USER_ALREADY_APP";
                        }

                        if (read.ApproveLevel > listApproval.Min(w => w.ApproveLevel))
                        {
                            return "REQUIRED_PRE_LEVEL";
                        }
                        var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                        if (objStaff != null)
                        {
                            ////New
                            //if (listApproval.Where(w => w.ApproveLevel <= read.ApproveLevel).Count() >= listApproval.Count())
                            //{
                            //    listApproval.ForEach(w => w.Status = SYDocumentStatus.APPROVED.ToString());
                            //}
                            //StaffProfile = objStaff;
                            read.ApprovedBy = objStaff.EmpCode;
                            read.ApprovedName = objStaff.AllName;
                            read.LastChangedDate = DateTime.Now.Date;
                            read.ApprovedDate = DateTime.Now;
                            read.Status = SYDocumentStatus.APPROVED.ToString();
                            DBX.ExDocApprovals.Attach(read);
                            DBX.Entry(read).State = System.Data.Entity.EntityState.Modified;
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
                // objMatch.IsApproved = true;
                if ((listApproval.Where(w => w.ApproveLevel > approverLevel && w.Status == SYDocumentStatus.OPEN.ToString()).Count() > 0))
                {
                    status = SYDocumentStatus.PENDING.ToString();

                }
                if (status == SYDocumentStatus.APPROVED.ToString())
                {
                    var objUpdate = DB.HisPendingAppSalaries.FirstOrDefault(w => w.PeriodID == objMatch.PeriodID);
                    if (objUpdate != null)
                    {
                        objUpdate.IsLock = true;
                        DB.HisPendingAppSalaries.Attach(objUpdate);
                        DB.Entry(objUpdate).Property(w => w.IsLock).IsModified = true;
                    }
                }

                objMatch.Status = status;
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                int row = DBX.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = HeaderAppSalary.ASNumber;
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
                log.DocurmentAction = HeaderAppSalary.ASNumber;
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
                log.DocurmentAction = HeaderAppSalary.ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string RejectSalary(int Peroid)
        {
            try
            {
                // DBBusinessProcess DBI = new DBBusinessProcess();

                string Reject = SYDocumentStatus.REJECTED.ToString();
                var objmatch = DB.HISApproveSalaries.FirstOrDefault(w => w.PeriodID == Peroid);
                if (objmatch == null)
                {
                    return "INV_EN";
                }
                if (objmatch.Status == Reject)
                {
                    return "DOC_RJ_AR";
                }
                string open = SYDocumentStatus.OPEN.ToString();
                var listApproval = DB.ExDocApprovals.Where(w => w.DocumentType == objmatch.DocumentType
                                    && w.DocumentNo == objmatch.ASNumber && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                var b = false;
                foreach (var read in listApproval)
                {
                    var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
                    if (checklist.Count > 0)
                    {
                        if (read.Status == SYDocumentStatus.APPROVED.ToString())
                        {
                            return "USER_ALREADY_APP";
                        }

                        if (read.ApproveLevel > listApproval.Min(w => w.ApproveLevel))
                        {
                            return "REQUIRED_PRE_LEVEL";
                        }
                        var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                        if (objStaff != null)
                        {
                            read.LastChangedDate = DateTime.Now.Date;
                            read.Status = SYDocumentStatus.REJECTED.ToString();
                            DB.ExDocApprovals.Attach(read);
                            DB.Entry(read).State = System.Data.Entity.EntityState.Modified;
                            b = true;
                            break;
                        }
                    }

                }
                if (listApproval.Count > 0)
                {
                    if (b == false)
                    {
                        return "USER_NOT_REJECT";
                    }
                }
                var status = SYDocumentStatus.REJECTED.ToString();
                //var open = SYDocumentStatus.OPEN.ToString();
                // objMatch.IsApproved = true;
                if (listApproval.Where(w => w.Status == open).ToList().Count > 0)
                {
                    status = SYDocumentStatus.REJECTED.ToString();
                    // objMatch.IsApproved = false;
                }

                objmatch.Status = status;
                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                #region Send Email
                var excfObject = DB.ExCfWorkFlowItems.Find(ScreenId, objmatch.DocumentType);
                if (excfObject != null)
                {
                    SYWorkFlowEmailObject wfo =
                           new SYWorkFlowEmailObject(excfObject.ApprovalFlow, WorkFlowType.REJECTOR,
                                UserType.N, SYDocumentStatus.REJECTED.ToString());
                    wfo.SelectListItem = new SYSplitItem(Convert.ToString(Peroid));
                    wfo.User = User;
                    wfo.BS = BS;
                    wfo.ScreenId = ScreenId;
                    wfo.Module = "HR";// CModule.PURCHASE.ToString();
                    wfo.ListLineRef = new List<BSWorkAssign>();
                    wfo.Action = SYDocumentStatus.PENDING.ToString();
                    wfo.ObjectDictionary = HeaderAppSalary;
                    wfo.ListObjectDictionary = new List<object>();
                    wfo.ListObjectDictionary.Add(objmatch);
                    HRStaffProfile Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objmatch.Requestor);
                    wfo.ListObjectDictionary.Add(Staff);
                    if (!string.IsNullOrEmpty(Staff.Email))
                    {
                        WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Staff);
                        MessageError = wfo.getErrorMessage(result1);
                    }
                }
                #endregion
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Peroid.ToString();
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
                log.DocurmentAction = Peroid.ToString();
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
                log.DocurmentAction = Peroid.ToString();
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public void SendEmail(int Peroid, string filePath1, int approverLevel, string URL)
        {
            try
            {
                #region Email
                var objMatch = DB.HISApproveSalaries.FirstOrDefault(w => w.PeriodID == Peroid);
                if (objMatch != null)
                {
                    //var Appro_ = objMatch.Requestor;

                    var listApproval = DB.ExDocApprovals.Where(w => w.DocumentType == objMatch.DocumentType &&
                                         w.DocumentNo == objMatch.ASNumber && w.Status == SYDocumentStatus.OPEN.ToString())
                                        .OrderBy(w => w.ApproveLevel).ToList();
                    if (listApproval.Any(w => w.ApproveLevel > approverLevel))
                    {
                        string nextAppro_ = listApproval.FirstOrDefault().Approver;
                        var Appro_ = DB.ExDocApprovals.Where(w => w.ApproveLevel == approverLevel && w.DocumentNo == objMatch.ASNumber
                                     && w.DocumentType == objMatch.DocumentType && w.Status == SYDocumentStatus.APPROVED.ToString()).Select(w => w.Approver)?.FirstOrDefault();

                        if (!string.IsNullOrEmpty(Appro_))
                        {
                           var currentApproval = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == Appro_);
                            var alertTo = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == nextAppro_);
                            var emailConfig = DMS.CFEmailAccounts.FirstOrDefault();

                            if (emailConfig != null && alertTo != null)
                            {
                                CFEmailAccount emailAccount = emailConfig;
                                string subject = "Payroll Approval";
                                string description = string.Format(
                                    "Dear {0} {1},<br />" +
                                    "I hope this email finds you well. I would like to request you preview and approve the payroll list for {2:MMMM-yyyy}.<br />" +
                                    "Please click on the link as below to log in for preview the payroll list.<br /><br />" +
                                    "Best regards,<br /><br />{3}<br /><br />" +
                                    "Please login at <a href='{4}'>URL</a>",
                                    alertTo.Title, alertTo.AllName, DateTime.Today, currentApproval.AllName, URL
                                );

                                string body = description;
                                EmailObject = new ClsEmail();
                                string[] filePaths = new string[] { filePath1 };
                                int result = EmailObject.SendMails(emailAccount, "", alertTo.Email, subject, body, filePaths);
                                // Optionally handle the result of the email sending
                                if (result != 0) // Assuming 0 indicates success
                                {
                                    // Log or handle the error
                                    Console.WriteLine("Email sending failed.");
                                }
                            }
                        }
                    }
                    else
                    {
                        var AlertTO = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objMatch.Requestor);
                        var EmailConf = DMS.CFEmailAccounts.FirstOrDefault();
                        if (EmailConf != null && AlertTO != null)
                        {
                            CFEmailAccount emailAccount = EmailConf;
                            string Subject = "Payroll Approval";
                            //string Body = string.Format("Your Salary Requset has been Approved  {0:dd-MMM-yyyy}", DateTime.Today);
                            string Body = string.Format("Your salary approval request has been approved.");

                            //EmailObject = new ClsEmail();
                            //int rs = EmailObject.SendMail(emailAccount, "", AlertTO.Email,
                            //    Subject, Body, "", "");
                        }
                    }
                    #endregion
                }
            }
            catch
            {
                throw new Exception("FAIL_TO_SEND_MAIL");
            }
        }
        public string CancelAppSalary(int Peroid)
        {
            try
            {
                string approved = SYDocumentStatus.CANCELLED.ToString();
                var objmatch = DB.HISApproveSalaries.FirstOrDefault(w => w.PeriodID == Peroid);
                if (objmatch == null)
                {
                    return "INV_EN";
                }
                var _obj = DB.ExDocApprovals.Where(x => x.DocumentNo == objmatch.ASNumber);
                if (objmatch.IsPostGL == true)
                {
                    return "DOC_POST_GL";
                }
                foreach (var read in _obj)
                {
                    read.Status = approved;
                    read.LastChangedDate = DateTime.Now;
                    DB.Entry(read).Property(w => w.Status).IsModified = true;
                    DB.Entry(read).Property(w => w.LastChangedDate).IsModified = true;
                }
                objmatch.Status = approved;
                objmatch.ChangedOn = DateTime.Now;
                objmatch.ChangedBy = User.UserName;
                DB.HISApproveSalaries.Attach(objmatch);
                DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;
                DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Peroid.ToString();
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
                log.DocurmentAction = Peroid.ToString();
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
                log.DocurmentAction = Peroid.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion
        public void SwapValueBenCharge(HISGLBenCharge GLBanChar, GEN_Filter _filter, PRGLmapping item,int LineItem,bool? IsPO ,bool? IsCredit = false)
        {
            DateTime DateNoW = DateTime.Now;
            GLBanChar.CompanyCode = _filter.CompanyCode.CompanyCode;
            GLBanChar.LineItem = LineItem;
            GLBanChar.CurrencyCode = _filter.CompanyCode.BaseCurrency;
            GLBanChar.Branch = _filter.Staff.Branch;
            GLBanChar.CostCenter = item.Branch;
            GLBanChar.Warehouse = item.Warehouse;
            GLBanChar.Project = item.Project;
            GLBanChar.EmpCode = _filter.Staff.EmpCode;
            GLBanChar.InMonth = _filter.InMonth;
            GLBanChar.InYear = _filter.InYear;
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
        public void GetDataSenior(EmpSeniority Senior,DateTime InMonth, decimal Salary)
        {
            if (InMonth.Month == 1)
                Senior.Jan = Salary;
            else if (InMonth.Month == 2)
                Senior.Feb = Salary;
            else if (InMonth.Month == 3)
                Senior.Mar = Salary;
            else if (InMonth.Month == 4)
                Senior.Apr = Salary;
            else if (InMonth.Month == 5)
                Senior.May = Salary;
            else if (InMonth.Month == 6)
                Senior.Jun = Salary;
            else if (InMonth.Month == 7)
                Senior.Jul = Salary;
            else if (InMonth.Month == 8)
                Senior.Aug = Salary;
            else if (InMonth.Month == 9)
                Senior.Sep = Salary;
            else if (InMonth.Month == 10)
                Senior.Oct = Salary;
            else if (InMonth.Month == 11)
                Senior.Nov = Salary;
            else if (InMonth.Month == 12)
                Senior.Dec = Salary;
        }
        public void GetDataIsMonth(DateTime FromDate, DateTime ToDate)
        {
            InYear = ToDate.Year;
            for (DateTime date = FromDate; date <= ToDate; date = date.AddMonths(1))
            {
                if (date.Month == 1)
                    IsJan = true;
                else if (date.Month == 2)
                    IsFeb = true;
                else if (date.Month == 3)
                    IsMar = true;
                else if (date.Month == 4)
                    IsApr = true;
                else if (date.Month == 5)
                    IsMay = true;
                else if (date.Month == 6)
                    IsJun = true;
                else if (date.Month == 7)
                    IsJul = true;
                else if (date.Month == 8)
                    IsAug = true;
                else if (date.Month == 9)
                    IsSep = true;
                else if (date.Month == 10)
                    IsOct = true;
                else if (date.Month == 11)
                    IsNov = true;
                else if(date.Month == 12)
                    IsDec = true;
            }
        }
    }
    public class LeaveDeduction
    {
        public string LeaveCode { get; set; }
        public string LeaveDescription { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal DayLeave { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
    }
    public class EmpSeniority
    {
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string Sex { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Probation { get; set; }
        public decimal Salary { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Average { get; set; }
        public decimal? Balance { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Remark { get; set; }
        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Aug { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }
    }
    public class GEN_Filter
    {
        public string EmpCode { get; set; }
        public int InYear { get; set; }
        public int InMonth { get; set; }
        public string Status { get; set; }
        public string PayType { get; set; }
        public string Default_Curremcy { get; set; }
        public string Round_UP { get; set; }
        public SYHRSetting Setting { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public SYHRCompany CompanyCode { get; set; }
        public HRStaffProfile Staff { get; set; }
        public PRParameter Parameter { get; set; }
        public HisPendingAppSalary HisAppSalary { get; set; }
        public HISGenSalary HisGensalarys { get; set; }
        public List<PREmpAllw> LstEmpAllow { get; set; }
        public List<PREmpBonu> LstEmpBon { get; set; }
        public List<HREmpCareer> LstEmpCareer { get; set; }
        public List<PR_RewardsType> LstRewardsType { get; set; }
        public List<PRBankFee> LstBankFee { get; set; }
        public List<ClsPayHis> LstPayHis { get; set; }
        public List<HISGenAllowance> ListHisEmpAllw { get; set; }
        public List<HISGenBonu> ListHisEmpBonu { get; set; }
        public List<HISGenDeduction> ListHisEmpDed { get; set; }
        public List<HISGenOTHour> ListHisEmpOT { get; set; }
        public List<HISGenLeaveDeduct> ListHisEmpLeave { get; set; }
        public List<PREmpHold> LstEmpHold { get; set; }
        public List<PRTaxSetting> ListTaxSetting { get; set; }
        public string NSSFSalaryType { get; set; }
    }
    public class ClsSelaryType
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public static List<ClsSelaryType> LoadData()
        {
            List<ClsSelaryType> _lst = new List<ClsSelaryType>();
            _lst.Add(new ClsSelaryType { Code = "BS", Description = "Basic Salary" });
            _lst.Add(new ClsSelaryType { Code = "NP", Description = "Net Pay" });
            _lst.Add(new ClsSelaryType { Code = "GP", Description = "Gross Pay" });
            return _lst;
        }
    }
    public class ClsPayHis
    {
        public string EmpCode { get; set; }
        public string SGROUP { get; set; }
        public string PayType { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
    public class SalaryApproval
    {

        public int PayPeriodID { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsLock { get; set; }

    }
}