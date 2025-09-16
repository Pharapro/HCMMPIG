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

namespace Humica.Logic.HR
{
    public class HRInsuranceObject
    {
        public SMSystemEntity DBI = new SMSystemEntity();
        public HumicaDBContext DB = new HumicaDBContext();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public HREmpInsurance Header { get; set; }
        public HRStaffProfile Staff { get; set; }
        public string EmpID { get; set; }
        public static int Progress { get; set; }
        public static decimal Percen { get; set; }
        public string CompanyCode { get; set; }
        public string Plant { get; set; }
        public string MessageCode { get; set; }
        public List<HREmpInsurance> ListHeader { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<HR_STAFF_VIEW> ListHeaderStaff { get; set; }
        public List<InsuranceTable> GridInsurance { get; set; }
        public List<GridReport> GridReportInsurance { get; set; }
        public HisInsurance HeaderHisInsurance { get; set; }
        public List<HisInsurance> ListHeaderHisInsurance { get; set; }
        public HRInsuranceObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateEmp()
        {
            try
            {
                DB = new HumicaDBContext();
                if (HeaderStaff.EmpCode == null) return "EMPCODE_EN";
                if (Header.InsurType == null) return "INSURTYPE_EN";
                var listStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                // Header.HireDate = Convert.ToDateTime(listStaff.StartDate);
                Header.Status = listStaff.Status;
                //Header.ResignDate = listStaff.DateTerminate;
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                DB.HREmpInsurances.Add(Header);
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
        }
        #region Generate Insurance
        public class InsuranceTable
        {
            public int TranNo { get; set; }
            public string EmpCode { get; set; }
            public string AllName { get; set; }
            public string OthAllName { get; set; }
            public Nullable<System.DateTime> DOB { get; set; }
            public string Sex { get; set; }
            public Nullable<System.DateTime> StartDate { get; set; }
            public Nullable<System.DateTime> EndDate { get; set; }
            public Nullable<System.DateTime> Probation { get; set; }
            public string Phone1 { get; set; }
            public string EmpType { get; set; }
            public string Division { get; set; }
            public string LevelCode { get; set; }
            public string CardNo { get; set; }
            public string Department { get; set; }
            public string Section { get; set; }
            public string Position { get; set; }
            public string Level { get; set; }
            public string EmployeeType { get; set; }
            public string TerminateStatus { get; set; }
            public string PayParam { get; set; }
            public string BranchID { get; set; }
            public string Peraddress { get; set; }
            public Nullable<System.DateTime> DateTerminate { get; set; }
            public string Branch { get; set; }
            public string PayParameter { get; set; }
            public string PostFamily { get; set; }
            public string DEPT { get; set; }
            public string DivisionDesc { get; set; }
            public string Location { get; set; }
            public string LOCT { get; set; }
            public string Nationality { get; set; }
            public string PeraddressOth { get; set; }
            public string BankBranch { get; set; }
            public string BankName { get; set; }
            public string BankAccName { get; set; }
            public string BankAcc { get; set; }
            public string NSSF { get; set; }
            public string TXPayType { get; set; }
            public string ServicesLenght { get; set; }
            public string Status { get; set; }
            public string StatusCode { get; set; }
            public string IDCard { get; set; }
            public string PassportNo { get; set; }
            public string SexKH { get; set; }
            public decimal Amount { get; set; }
            public string InsurType { get; set; }
            public string InsurComp { get; set; }
            public Nullable<int> Age { get; set; }
            public string Province { get; set; }
            public string District { get; set; }
            public string Commune { get; set; }
            public string Village { get; set; }
            public Nullable<int> Warning { get; set; }
            public Nullable<bool> IsHold { get; set; }
            public Nullable<System.DateTime> IDCard_IssueDate { get; set; }
            public Nullable<System.DateTime> IDCard_ExpiryDate { get; set; }

        }
        public class GridReport
        {
            public int No { get; set; }
            public string Category { get; set; }
            public string Sex { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public int? Age { get; set; }
            public string Marital { get; set; }
            public string Status { get; set; }
            public string InsuredName { get; set; }
            public string Nationality { get; set; }
            public string IDType { get; set; }
            public string IP { get; set; }
            public string IDNo { get; set; }
            public string EmployeeNo { get; set; }
            public DateTime EmploymentDate { get; set; }
            public string Position { get; set; }
            public decimal? SumAssured { get; set; }
            public decimal CostPersonYear { get; set; }
            public DateTime PolicyExpire { get; set; }
            public int NoOfDays { get; set; }
            public string EndTran { get; set; }
            public DateTime EndEffective { get; set; }
            public decimal EndCostProRate { get; set; }
        }
        public List<InsuranceTable> GridList()
        {
            ListHeader = new List<HREmpInsurance>();
            ListHeaderStaff = new List<HR_STAFF_VIEW>();
            DateTime DateGenerateIn = new DateTime();
            DateGenerateIn = LastDayOfMonth(Filter.InMonth.Date);
            ListHeader = DB.HREmpInsurances.Where(w => w.StartDate <= DateGenerateIn && w.EndDate >= DateGenerateIn).ToList();
            if (Filter.RewardsType != null)
            {
                ListHeader = ListHeader.Where(w => w.InsurType == Filter.RewardsType).ToList();
            }
            ListHeaderStaff = DBV.HR_STAFF_VIEW.ToList();
            ListHeaderStaff = ListHeaderStaff.Where(w => ListHeader.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            List<InsuranceTable> ListR = new List<InsuranceTable>();
            InsuranceTable obj;
            var list_ = (from f in ListHeader
                         join E in ListHeaderStaff on new { f.EmpCode } equals new { E.EmpCode } into MyNew
                         select (new
                         {
                             TranNo = f.TranNo,
                             EmpCode = MyNew.First().EmpCode,
                             AllName = MyNew.First().AllName,
                             OthAllName = MyNew.First().OthAllName,
                             DOB = MyNew.First().DOB,
                             Department = MyNew.First().Department,
                             Sex = MyNew.First().Sex,
                             StartDate = f.StartDate,
                             EndDate = f.EndDate,
                             Probation = MyNew.First().Probation,
                             EmployeeType = MyNew.First().EmployeeType,
                             Level = MyNew.First().Level,
                             Phone1 = MyNew.First().Phone1,
                             Age = MyNew.First().Age,
                             Position = MyNew.First().Position,
                             Nationality = MyNew.First().Nationality,
                             IDCard = MyNew.First().IDCard,
                             DEPT = MyNew.First().DEPT,

                         })).ToList();
            foreach (var item in list_.ToList())
            {
                var amt = ListHeader.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                obj = new InsuranceTable();
                obj.TranNo = item.TranNo;
                obj.EmpCode = item.EmpCode;
                obj.AllName = item.AllName;
                obj.DOB = item.DOB;
                obj.Sex = item.Sex;
                obj.StartDate = item.StartDate;
                obj.EndDate = item.EndDate;
                obj.Department = item.DEPT;
                obj.EmployeeType = item.EmployeeType;
                obj.Level = item.Level;
                obj.Amount = amt.Amount;
                obj.InsurType = amt.InsurType;
                obj.InsurComp = amt.InsurComp;
                obj.Position = item.Position;
                obj.IDCard = item.IDCard;
                obj.Nationality = item.Nationality;
                ListR.Add(obj);
            }
            return ListR.OrderBy(w => w.TranNo).ToList();
        }
        public string GenerateInsurance(string EmpCode_)
        {
            try
            {
                DB = new HumicaDBContext();
                DateTime DateGenerateIn = new DateTime();
                DateGenerateIn = LastDayOfMonth(Filter.InMonth);
                string[] Em = EmpCode_.Split(';');
                Progress = Em.Count();
                decimal _p = 0;
                int i = 0;
                var ListStaff = DB.HRStaffProfiles.ToList();
                foreach (var Code in Em)
                {
                    string[] listCod = Code.Split(',');
                    int TranNo = Convert.ToInt32(listCod[0]);
                    string Empcode = listCod[1];
                    var ListInS = DB.HisInsurances.Where(w => w.TranNo == TranNo && w.EmpCode == Empcode && w.InYear == Filter.InMonth.Year && w.InMonth == Filter.InMonth.Month).ToList();
                    if (ListInS.Count > 0)
                    {
                        foreach (var item in ListInS)
                        {
                            DB.HisInsurances.Remove(item);
                            DB.SaveChanges();
                        }
                    }
                    i += 1;
                    _p = i;
                    Percen = _p / Progress * 100;
                    var List = DB.HREmpInsurances.FirstOrDefault(w => w.TranNo == TranNo && w.EmpCode == Empcode);
                    var liStInsurCom = DB.HRInsuranceCompanies.FirstOrDefault(w => w.Code == List.InsurComp);
                    DateTime joinDay = Convert.ToDateTime(List.StartDate);
                    HisInsurance ob = new HisInsurance();
                    ob.TranNo = TranNo;
                    ob.EmpCode = List.EmpCode;
                    ob.DocumentDate = DateTime.Now.Date;
                    ob.InMonth = Filter.InMonth.Month;
                    ob.InYear = Filter.InMonth.Year;
                    ob.Period = Convert.ToDateTime(Filter.InMonth);
                    ob.InsurType = List.InsurType;
                    ob.InsurComp = List.InsurComp;
                    ob.InsurDescription = liStInsurCom.CompanyName;
                    int TotalDay = DateTime.DaysInMonth(Filter.InMonth.Year, Filter.InMonth.Month);
                    var Staff = ListStaff.FirstOrDefault(w => w.EmpCode == ob.EmpCode);
                    if (Staff.StartDate < List.StartDate && Staff.DateTerminate.Year != 1900)
                    {

                        int Dat_ = Staff.DateTerminate.Subtract(List.StartDate).Days;
                        ob.Amount = Convert.ToDecimal(Dat_ / 365.00);
                        ob.Amount = ob.Amount * List.Amount;
                    }
                    else
                    {
                        if (Staff.StartDate == List.StartDate)
                        {
                            int Dat_ = Staff.DateTerminate.Subtract(List.StartDate).Days;
                            ob.Amount = Convert.ToDecimal(Dat_ / 356.00);
                            ob.Amount = ob.Amount * List.Amount;
                        }
                        else
                        {
                            ob.Amount = List.Amount;
                        }

                    }
                    //if (joinDay.Year==DateGenerateIn.Year)
                    //{
                    //    if (joinDay.Month == DateGenerateIn.Month)
                    //    {
                    //        ob.WorkingDay = (DateGenerateIn - joinDay).Days;
                    //        ob.Rate = Math.Round(TotalDay / List.Amount, 2);
                    //        ob.Amount = ob.Rate * ob.WorkingDay;
                    //    }
                    //    else
                    //    {
                    //        ob.WorkingDay = TotalDay;
                    //        ob.Rate = Math.Round(TotalDay / List.Amount, 2);
                    //        ob.Amount = ob.Rate * ob.WorkingDay;
                    //    }
                    //}
                    //else
                    //{
                    //    ob.WorkingDay = TotalDay;
                    //    ob.Rate =Math.Round(TotalDay / List.Amount,2);
                    //    ob.Amount = ob.Rate * ob.WorkingDay;
                    //}
                    ob.CreatedBy = User.UserName;
                    ob.CreatedOn = DateTime.Now;
                    DB.HisInsurances.Add(ob);
                }
                int row = DB.SaveChanges();
                return SYSConstant.OK.ToString();
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
        }
        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
        public static DateTime LastDayOfMonth(DateTime dt)
        {
            DateTime ss = new DateTime(dt.Year, dt.Month, 1);
            return ss.AddMonths(1).AddDays(-1);
        }
        public string DeleteInsuranceGenerated(int ID, string EmpCode)
        {
            try
            {
                DB = new HumicaDBContext();
                var listHisInsurace = DB.HisInsurances.FirstOrDefault(w => w.ID == ID && w.EmpCode == EmpCode);
                DB.HisInsurances.Remove(listHisInsurace);
                DB.SaveChanges();
                return SYSConstant.OK;

            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.EmpCode.ToString();
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
                log.ScreenId = Header.EmpCode.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public List<GridReport> GridListReportInsurance()
        {
            List<GridReport> ListRP = new List<GridReport>();
            GridReport obj;
            ListHeader = new List<HREmpInsurance>();
            ListHeaderHisInsurance = new List<HisInsurance>();
            ListHeaderHisInsurance = DB.HisInsurances.Where(w => w.DocumentDate >= Filter.FromDate && w.DocumentDate <= Filter.ToDate).ToList();
            int i = 1;
            foreach (var item in ListHeaderHisInsurance)
            {
                var ListInsuranceStaff = DB.HREmpInsurances.Find(item.EmpCode, item.InsurType);
                var Staff = DB.HRStaffProfiles.Find(item.EmpCode);
                var ListStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                obj = new GridReport();
                obj.No = i;
                obj.Category = ListStaff.DEPT;
                obj.InsuredName = Staff.AllName;
                obj.Sex = ListStaff.Sex;
                obj.DateOfBirth = ListStaff.DOB;
                obj.Age = ListStaff.Age;
                obj.Marital = Staff.Marital;
                obj.Nationality = ListStaff.Nationality;
                obj.IDType = "";
                obj.EmployeeNo = item.EmpCode;
                obj.IDNo = ListStaff.IDCard;
                obj.EmploymentDate = ListStaff.StartDate.Value;
                obj.Position = ListStaff.Position;
                obj.SumAssured = 0;
                obj.CostPersonYear = ListInsuranceStaff.Amount;
                obj.PolicyExpire = ListInsuranceStaff.EndDate;
                obj.NoOfDays = 0;
                obj.EndTran = "";
                obj.EndEffective = DateTime.Now.Date;
                obj.EndCostProRate = 0;
                ListRP.Add(obj);
                i++;
            }
            return ListRP;
        }
        #endregion
        //public List<HREmpInsurance> LoadDataInsuranceGen(FTFilterEmployee Filter1, List<HRBranch> _ListBranch)
        //{
        //    Filter1.FromDate = new DateTime(Filter1.InMonth.Year, Filter1.InMonth.Month, 1);
        //    Filter1.ToDate = new DateTime(Filter1.InMonth.Year, Filter1.InMonth.Month, DateTime.DaysInMonth(Filter.InMonth.Year, Filter.InMonth.Month));
        //    var _List = new List<HREmpInsurance>();
        //    var staff = DB.HREmpInsurances.ToList();
        //    var _listStaff = staff.ToList();
        //    DateTime date = new DateTime(1900, 1, 1);
        //    _listStaff = _listStaff.Where(w => w.StartDate.Value.Date <= Filter1.ToDate.Date && ((w.DateTerminate.Value.Date == date.Date
        //    || w.DateTerminate.Value.AddDays(-1) >= Filter1.FromDate.Date) || (w.SalaryFlag == true
        //    && w.ReSalary.Year == Filter1.InMonth.Year && w.ReSalary.Month == Filter1.InMonth.Month))).ToList();
        //    _listStaff = _listStaff.Where(w => _ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
        //    if (Filter1.Branch != null)
        //        _listStaff = _listStaff.Where(w => w.Branch == Filter1.Branch).ToList();
        //    if (Filter1.Division != null)
        //        _listStaff = _listStaff.Where(w => w.Division == Filter1.Division).ToList();
        //    if (Filter1.Department != null)
        //        _listStaff = _listStaff.Where(w => w.DEPT == Filter1.Department).ToList();
        //    if (Filter1.Section != null)
        //        _listStaff = _listStaff.Where(w => w.SECT == Filter1.Section).ToList();
        //    if (Filter1.Position != null)
        //        _listStaff = _listStaff.Where(w => w.JobCode == Filter1.Position).ToList();
        //    if (Filter1.Level != null)
        //        _listStaff = _listStaff.Where(w => w.LevelCode == Filter1.Level).ToList();
        //    //if (Filter1.SalaryType != null)
        //    //    _listStaff = _listStaff.Where(w => w.SalaryType == Filter1.SalaryType).ToList();
        //    _listStaff = _listStaff.Where(w => w.IsHold != true).ToList();
        //   // _List = _listStaff;
        //    return _List.OrderBy(w => w.EmpCode).ToList();
        //}
        public string EditEmp(int TranNo)
        {
            try
            {
                var objMatch = DB.HREmpInsurances.FirstOrDefault(w => w.TranNo == TranNo);
                if (objMatch == null)
                {
                    return "INV_DOC";
                }
                Header.EmpCode = objMatch.EmpCode;
                objMatch.InsurType = Header.InsurType;
                objMatch.InsurComp = Header.InsurComp;
                objMatch.StartDate = Header.StartDate;
                objMatch.EndDate = Header.EndDate;
                objMatch.Amount = Header.Amount;
                objMatch.Status = Header.Status;
                objMatch.Remark = Header.Remark;
                objMatch.Dependent = Header.Dependent;
                objMatch.PolicyNo = Header.PolicyNo;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                DB.HREmpInsurances.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Modified;
                int row1 = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderStaff.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string DeleteEmp(int TranNo)
        {
            try
            {
                Header = new HREmpInsurance();
                var objMatch = DB.HREmpInsurances.FirstOrDefault(w => w.TranNo == TranNo);
                if (objMatch == null)
                {
                    return "INV_DOC";
                }
                DB.HREmpInsurances.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Deleted;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.EmpCode.ToString();
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
                log.ScreenId = Header.EmpCode.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }


    }
}
