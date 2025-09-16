using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.Atts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.PR
{
    public class PRGenerateSeverance
    {
        public HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity DMS = new SMSystemEntity();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PREmpAllw Header { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public string ScreenId { get; set; }
        public string CountryCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public HISGenSalary HeaderSalary { get; set; }
        public List<PREmpAllw> ListHeader { get; set; }
        public List<HR_STAFF_VIEW> ListStaff { get; set; }
        public List<HR_View_EmpGenSalary> ListEmployeeGen { get; set; }
        public List<HR_PR_EmpSalary> ListEmpPaySlip { get; set; }
        public List<HISGenSalaryD> ListBasicSalary { get; set; }
        public List<EmpSeverance> ListEmpSeverance { get; set; }
        public List<PR_GLCharge_View> ListGLCharge { get; set; }
        public List<HISPaySlip> ListPaySlip { get; set; }
        //public List<PR_ApproveSalary_view> ListApproveSalary { get; set; }
        public GEN_Filter GenFilter { get; set; }
        public string EmpID { get; set; }
        public int Progress { get; set; }
        public List<GetDataYear> ListDataYear { get; set; }
        public List<GetDataYear> ListYear { get; set; }
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
        public bool IsDesc { get; set; }
        public static List<ListProgress> ListProgress { get; set; }
        public PRGenerateSeverance()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<EmpSeverance> LoadDataSeverance(FTFilterEmployee Filter1, List<HRBranch> _lstBranch)
        {
            DateTime InMonth = DateTimeHelper.EndDateOfMonth(Filter1.InMonth);
            DateTime FromDate = InMonth.AddYears(-2).AddDays(1);

            var _List = new List<EmpSeverance>();
            var staff = DBV.HR_View_EmpGenSalary.ToList();
            var Contracts = DBV.HREmpContracts.Where(w => w.ConType == Filter1.ContractType && w.ToDate.Value.Year == InMonth.Year
                                                    && w.ToDate.Value.Month == InMonth.Month).ToList();
            var _listStaff = staff.ToList();
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
            if (Contracts != null)
                _listStaff = _listStaff.Where(w => Contracts.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            //if (Filter1.ContractType != null)
            //    _listStaff = _listStaff.Where(w => w.ConType == Filter1.ContractType && w.ToDate.Value.Month == InMonth.Month
            //                                       && w.ToDate.Value.Year == InMonth.Year).ToList();
            var listSin = DB.HISGenSalaries.AsEnumerable().Where(w => Contracts.AsEnumerable().Where(x => w.EmpCode == x.EmpCode && w.INYear >= x.FromDate.Value.Year
                       && w.INYear <= x.ToDate.Value.Year && w.INMonth >= x.FromDate.Value.Month && w.INMonth <= x.ToDate.Value.Month).Any()).ToList();
            decimal? Amount = 0;
            GetDataIsMonth(FromDate, InMonth);
            foreach (var item in _listStaff)
            {
                var EmpContr_ = Contracts.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                var _sincer = listSin.Where(w => w.EmpCode == item.EmpCode).ToList();
                var EmpSin = new EmpSeverance();
                EmpSin.EmpCode = item.EmpCode;
                EmpSin.AllName = item.AllName;
                EmpSin.Department = item.Department;
                EmpSin.Position = item.Position;
                EmpSin.StartDate = item.StartDate.Value;
                if (EmpContr_ != null) EmpSin.EndContract = EmpContr_.ToDate.Value;
                EmpSin.Rate = 0;
                EmpSin.Balance = 0;
                EmpSin.TotalSalary = 0;
                EmpSin.Rate = Filter1.Rate;
                EmpSin.Balance = 0;
                EmpSin.Remark = "";
                foreach (var read in _sincer)
                {
                    decimal Salary = 0;
                    Salary = Convert.ToDecimal(read.Salary + read.AlwBeforTax);
                    GetDataSever_(EmpSin, read.ToDate.Value, Salary);
                }
                if (_sincer.Count > 0)
                {
                    Amount = _sincer.Sum(w => w.Salary) + _sincer.Sum(w => w.AlwBeforTax);
                    EmpSin.TotalSalary = Convert.ToDecimal(Amount);
                    EmpSin.Rate = Filter1.Rate;
                    EmpSin.Balance = Amount * (Filter1.Rate / 100.00M);
                    EmpSin.Remark = "Transfer from Severance Pay";
                }
                else continue;

                _List.Add(EmpSin);
            }
            return _List.OrderBy(w => w.EmpCode).ToList();
        }
        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
        public string Generate_Severance(string EmpCode, FTFilterEmployee Filter1)
        {
            try
            {
                try
                {
                    DB = new HumicaDBContext();
                    DB.Configuration.AutoDetectChangesEnabled = false;
                    DateTime InMonth = new DateTime(Filter1.InMonth.Year, Filter1.InMonth.Month, 1);
                    string[] Emp = EmpCode.Split(';');
                    foreach (var Code in Emp)
                    {
                        var list = ListEmpSeverance.FirstOrDefault(w => w.EmpCode == Code);
                        if (Code.Trim() != "")
                        {
                            var Temp = DB.PRSeverancePays.Where(w => w.EmpCode == Code && w.InYear == Filter1.InMonth.Year
                                                                    && w.InMonth == Filter1.InMonth.Month).ToList();
                            if (Temp.Count() > 0)
                            {
                                foreach (var item in Temp)
                                {
                                    DB.PRSeverancePays.Attach(item);
                                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                                }
                            }
                            var SeverancePay = new PRSeverancePay();
                            SeverancePay.EmpCode = Code;
                            SeverancePay.InMonth = Filter1.InMonth.Month;
                            SeverancePay.InYear = Filter1.InMonth.Year;
                            if (list != null)
                            {
                                SeverancePay.TotalSalary = list.TotalSalary;
                                SeverancePay.TotalMonth = 0;
                                SeverancePay.Rate = list.Rate.Value;
                                SeverancePay.TotalAmount = list.Balance;
                                SeverancePay.Remark = "Transfer from Severance Pay";
                            }
                            DB.PRSeverancePays.Add(SeverancePay);
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
        public void GetDataSever_(EmpSeverance Serverance, DateTime InMonth, decimal Salary)
        {
            var FType = ListDataYear.FirstOrDefault(w => w.InYear == InMonth.Year);
            if (FType != null)
            {
                if (FType.FType == "O")
                {
                    if (InMonth.Month == 1) Serverance.Jan = Salary;
                    else if (InMonth.Month == 2) Serverance.Feb = Salary;
                    else if (InMonth.Month == 3) Serverance.Mar = Salary;
                    else if (InMonth.Month == 4) Serverance.Apr = Salary;
                    else if (InMonth.Month == 5) Serverance.May = Salary;
                    else if (InMonth.Month == 6) Serverance.Jun = Salary;
                    else if (InMonth.Month == 7) Serverance.Jul = Salary;
                    else if (InMonth.Month == 8) Serverance.Aug = Salary;
                    else if (InMonth.Month == 9) Serverance.Sep = Salary;
                    else if (InMonth.Month == 10) Serverance.Oct = Salary;
                    else if (InMonth.Month == 11) Serverance.Nov = Salary;
                    else if (InMonth.Month == 12) Serverance.Dec = Salary;
                }
                else
                {
                    if (InMonth.Month == 1) Serverance.NJan = Salary;
                    else if (InMonth.Month == 2) Serverance.NFeb = Salary;
                    else if (InMonth.Month == 3) Serverance.NMar = Salary;
                    else if (InMonth.Month == 4) Serverance.NApr = Salary;
                    else if (InMonth.Month == 5) Serverance.NMay = Salary;
                    else if (InMonth.Month == 6) Serverance.NJun = Salary;
                    else if (InMonth.Month == 7) Serverance.NJul = Salary;
                    else if (InMonth.Month == 8) Serverance.NAug = Salary;
                    else if (InMonth.Month == 9) Serverance.NSep = Salary;
                    else if (InMonth.Month == 10) Serverance.NOct = Salary;
                    else if (InMonth.Month == 11) Serverance.NNov = Salary;
                    else if (InMonth.Month == 12) Serverance.NDec = Salary;
                }
            }

        }
        public void GetDataIsMonth(DateTime FromDate, DateTime ToDate)
        {
            string _name = "";
            int Year = FromDate.Year;
            for (DateTime date = FromDate; date <= ToDate; date = date.AddMonths(1))
            {
                if (!ListYear.Where(w => w.InYear == date.Year).Any())
                {
                    var obj1 = new GetDataYear();
                    obj1.InYear = date.Year;
                    obj1.FType = "";
                    if (Year != date.Year)
                        obj1.FType = "N";
                    ListYear.Add(obj1);
                }
                var obj = new GetDataYear();
                obj.InYear = date.Year;
                if (Year != date.Year)
                    _name = "N";
                obj.FType = _name;
                obj.FieldName = _name + date.ToString("MMM");
                obj.IsFieldName = true;
                ListDataYear.Add(obj);

            }
        }
    }
    public class EmpSeverance
    {
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string Sex { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndContract { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Balance { get; set; }
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
        public decimal NJan { get; set; }
        public decimal NFeb { get; set; }
        public decimal NMar { get; set; }
        public decimal NApr { get; set; }
        public decimal NMay { get; set; }
        public decimal NJun { get; set; }
        public decimal NJul { get; set; }
        public decimal NAug { get; set; }
        public decimal NSep { get; set; }
        public decimal NOct { get; set; }
        public decimal NNov { get; set; }
        public decimal NDec { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class GetDataYear
    {
        public int InYear { get; set; }
        public string FieldName { get; set; }
        public string FType { get; set; }
        public bool IsFieldName { get; set; }
    }
}
