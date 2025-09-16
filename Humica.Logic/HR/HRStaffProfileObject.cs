using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using Humica.Logic.PR;
using Humica.Logic.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Humica.Logic.HR
{
    public class HRStaffProfileObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SMSystemEntity DP = new SMSystemEntity();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string NewEmpCode { get; set; }
        public string OldEmpCode { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public HRStaffProfile Header { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public HREmpCareer HeaderCareer { get; set; }
        public List<HRStaffProfile> ListHeader { get; set; }
        public List<HR_STAFF_VIEW> ListStaffView { get; set; }
        public List<HR_EmpCareer_View> ListEmpCareer { get; set; }
        public List<HREmpCareer> ListCareer { get; set; }
        public List<HR_Career> ListCareerHis { get; set; }
        public List<HREmpIdentity> ListEmpIdentity { get; set; }
        public string ToDate { get; set; }
        public string MessageCode { get; set; }
        public List<HREmpDisciplinary> ListEmpDisciplinary { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public List<HRStaffProfile> ListEmp { get; set; }
        public HRDelayProbation HeaderDP { get; set; }
        public List<HRDelayProbation> ListDP { get; set; }
        public FTFilterEmployee FilterEmployee { get; set; }
        public string ErrorMessage { get; set; }
        public string RoleSelected = "";
        public string DataSelected = "";
        public string Salary { get; set; }
        public string NewSalary { get; set; }
        public string OldSalary { get; set; }
        public string Increase { get; set; }
        public string SalaryTax { get; set; }
        public string SalaryNSSF { get; set; }
        public bool IsTax { get; set; }
        public bool IsEmpAuto { get; set; }
        public bool IsInAtive { get; set; }
        public string EmpCode { get; set; }
        public bool Fist_Last { get; set; }
        public bool Fist_Last_KH { get; set; }
        public HRStaffProfileObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public static IEnumerable<HRStaffProfile> LoadAllStaff(FTFilterReport _Filter)
        {
            HumicaDBContext context = new HumicaDBContext();
            List<HRStaffProfile> _list = new List<HRStaffProfile>();
            DateTime DocumentDate = DateTime.Now.StartDateOfMonth();
            _list = context.HRStaffProfiles.Where(w => (w.Status == "A" || (w.Status != "A" && w.DateTerminate > DocumentDate)) &&
                           (string.IsNullOrEmpty(_Filter.Branch) || w.Branch == _Filter.Branch) &&
                           (string.IsNullOrEmpty(_Filter.Division) || w.Division == _Filter.Division) &&
                           (string.IsNullOrEmpty(_Filter.BusinessUnit) || w.GroupDept == _Filter.BusinessUnit) &&
                           (string.IsNullOrEmpty(_Filter.Department) || w.DEPT == _Filter.Department) &&
                           (string.IsNullOrEmpty(_Filter.Office) || w.Office == _Filter.Office) &&
                           (string.IsNullOrEmpty(_Filter.Section) || w.DEPT == _Filter.Section) &&
                           (string.IsNullOrEmpty(_Filter.Group) || w.Groups == _Filter.Group) &&
                           (string.IsNullOrEmpty(_Filter.Position) || w.DEPT == _Filter.Position) &&
                           (string.IsNullOrEmpty(_Filter.Level) || w.LevelCode == _Filter.Level)
                          ).ToList();
            return _list;
        }

        public static async Task<IEnumerable<ClsStaff>> LoadStaff(string branch = "", string Department = "")
        {
            HumicaDBContext context = new HumicaDBContext();
            List<ClsStaff> _list = new List<ClsStaff>();

            _list = await (from Staff in context.HRStaffProfiles
                           join Post in context.HRPositions on Staff.JobCode equals Post.Code
                           join Dept in context.HRDepartments on Staff.DEPT equals Dept.Code
                           where (string.IsNullOrEmpty(branch) || Staff.Branch == branch)
                           && (string.IsNullOrEmpty(Department) || Staff.DEPT == Department)
                           select new ClsStaff
                           {
                               EmpCode = Staff.EmpCode,
                               EmployeeName = Staff.AllName,
                               EmployeeName2 = Staff.OthAllName,
                               Position = Post.Description,
                               Department = Dept.Description,
                               Status = Staff.Status,
                               BranchCode = Staff.Branch,
                               Dept = Staff.DEPT,
                           }).ToListAsync();
            return _list;
        }
        public async Task<List<HR_STAFF_VIEW>> LoadData(string Status = "", string Department = null, string Office = null, string Team = null, string Branch = null)
        {
            List<HR_STAFF_VIEW> _listTemp = new List<HR_STAFF_VIEW>();
            List<HRCompany> _ListCompany = SYConstant.getCompanyDataAccess();
            List<HRBranch> _ListBranch = SYConstant.getBranchDataAccess();
            List<HRLevel> _ListLevel = SYConstant.getLevelDataAccess();
            var branchid = _ListBranch.Select(s => s.Code).ToList();
            var companyCodes = _ListCompany.Select(x => x.Company).ToList();
            var levelCodes = _ListLevel.Select(x => x.Code).ToList();
            _listTemp = await new HumicaDBViewContext().HR_STAFF_VIEW
                           .Where(w => companyCodes.Contains(w.CompanyCode))
                           .Where(w => levelCodes.Contains(w.LevelCode))
                           .Where(w => branchid.Contains(w.BranchID))
                           .ToListAsync();
            if (Filter?.StartDate != null)
            {
                if (Filter.Status == "A")
                    _listTemp = _listTemp.Where(w => w.StatusCode == Filter.Status
                                    && w.StartDate.Value.Month == Filter.StartDate.Value.Month && w.StartDate.Value.Year == Filter.StartDate.Value.Year).ToList();
                else if (Filter.Status == "I")
                    _listTemp = _listTemp.Where(w => w.StatusCode == Filter.Status
                                && w.DateTerminate.Value.AddDays(-1).Month == Filter.StartDate.Value.Month && w.DateTerminate.Value.AddDays(-1).Year == Filter.StartDate.Value.Year).ToList();
                if (Filter.Status == "I/A")
                {
                    _listTemp = _listTemp.Where(w => w.StatusCode == "A" || (w.DateTerminate.Value.AddDays(-1).Month == Filter.StartDate.Value.Month
                                                   && w.DateTerminate.Value.AddDays(-1).Year == Filter.StartDate.Value.Year)).ToList();
                }
            }
            else if (Status != "I/A" && Status != "")
                _listTemp = _listTemp.Where(w => w.StatusCode == Status).ToList();
            if (!string.IsNullOrEmpty(Branch))
            {
                var branchFilter = Branch.Split(',').Select(b => b.Trim()).Where(b => !string.IsNullOrEmpty(b)).ToList();
                _listTemp = _listTemp.Where(w => branchFilter.Contains(w.BranchID)).ToList();
            }
            if (!string.IsNullOrEmpty(Department))
                _listTemp = _listTemp.Where(w => w.DEPT == Department).ToList();
            if (!string.IsNullOrEmpty(Office))
                _listTemp = _listTemp.Where(w => w.Office == Office).ToList();
            if (!string.IsNullOrEmpty(Team))
                _listTemp = _listTemp.Where(w => w.Groups == Team).ToList();

            return _listTemp;
        }
        public List<HR_EmpCareer_View> LoadDataCareer(string Status, string BU = null, string Department​ = null, string Office = null, string Team = null, string Branch = null)
        {
            DateTime CurrentDate = DateTime.Now;
            List<HR_EmpCareer_View> _listTemp = new List<HR_EmpCareer_View>();
            List<HRCompany> _ListCompany = SYConstant.getCompanyDataAccess();
            List<HRBranch> _ListBranch = SYConstant.getBranchDataAccess();
            List<HRLevel> _ListLevel = SYConstant.getLevelDataAccess();
            var branchid = _ListBranch.Select(s => s.Code).ToList();
            var companyCodes = _ListCompany.Select(x => x.Company).ToList();
            var levelCodes = _ListLevel.Select(x => x.Code).ToList();
            _listTemp = new HumicaDBViewContext().HR_EmpCareer_View
                           .Where(w => companyCodes.Contains(w.CompanyCode))
                           .Where(w => levelCodes.Contains(w.LevelCode))
                           .Where(w => branchid.Contains(w.BranchID))
                           .ToList();
            if (Filter.StartDate != null)
            {
                if (Filter.Status == "A")
                    _listTemp = _listTemp.Where(w => (w.Status == Filter.Status
                                    && w.StartDate.Value.Month == Filter.StartDate.Value.Month
                                    && w.StartDate.Value.Year == Filter.StartDate.Value.Year && w.ToDate >= CurrentDate)
                                    || (w.Status == "I" && w.FromDate > CurrentDate)).ToList();
                else if (Filter.Status == "I")
                    _listTemp = _listTemp.Where(w => w.Status == Filter.Status
                                && w.FromDate.Value.AddDays(-1).Month == Filter.StartDate.Value.Month
                                && w.FromDate.Value.AddDays(-1).Year == Filter.StartDate.Value.Year
                                && w.FromDate <= CurrentDate).ToList();
                if (Filter.Status == "I/A")
                {
                    _listTemp = _listTemp.Where(w => ((w.Status == "A" && w.ToDate >= CurrentDate) || (w.Status == "I" && w.FromDate > CurrentDate)) 
                                                    || (w.FromDate.Value.AddDays(-1).Month == Filter.StartDate.Value.Month
                                                   && w.FromDate.Value.AddDays(-1).Year == Filter.StartDate.Value.Year)).ToList();
                }
            }
            else
            {
                if (Filter.Status == "A")
                    _listTemp = _listTemp.Where(w => (w.Status == Filter.Status && w.ToDate >= CurrentDate) || (w.Status == "I" && w.FromDate > CurrentDate)).ToList();
                if (Filter.Status == "I")
                    _listTemp = _listTemp.Where(w => w.Status == Filter.Status && w.FromDate <= CurrentDate).ToList();
            }
            if (!string.IsNullOrEmpty(Branch))
            {
                var branchFilter = Branch.Split(',').Select(b => b.Trim()).Where(b => !string.IsNullOrEmpty(b)).ToList();
                _listTemp = _listTemp.Where(w => branchFilter.Contains(w.BranchID)).ToList();
            }
            if (!string.IsNullOrEmpty(BU))
                _listTemp = _listTemp.Where(w => w.GroupDept == BU).ToList();
            if (!string.IsNullOrEmpty(Department))
                _listTemp = _listTemp.Where(w => w.DEPT == Department).ToList();
            if (!string.IsNullOrEmpty(Office))
                _listTemp = _listTemp.Where(w => w.Office == Office).ToList();
            if (!string.IsNullOrEmpty(Team))
                _listTemp = _listTemp.Where(w => w.Groups == Team).ToList();
            _listTemp.OrderBy(w => w.EmpCode).OrderByDescending(w => w.FromDate);

            //var ListCareer = new List<HR_EmpCareer_View>();
            //foreach (var read in _listTemp)
            //{
            //    var check = ListCareer.FirstOrDefault(w => w.EmpCode == read.EmpCode);
            //    if (check != null) continue;
            //    ListCareer.Add(read);
            //}

            var ListCareer = _listTemp
          .GroupBy(w => w.EmpCode)
          .Select(g => g.OrderByDescending(w => w.FromDate.HasValue ? w.FromDate.Value : DateTime.MinValue).FirstOrDefault())
          .Where(entry => entry != null)
          .OrderBy(w => w.EmpCode)
          .ToList();

            return ListCareer;
        }
        public async Task<List<HRDelayProbation>> LoadDataDelayProbation()
        {
            List<HRStaffProfile> _listTemp = new List<HRStaffProfile>();
            List<HRCompany> _ListCompany = SYConstant.getCompanyDataAccess();
            List<HRBranch> _ListBranch = SYConstant.getBranchDataAccess();
            List<HRLevel> _ListLevel = SYConstant.getLevelDataAccess();
            var branchids = _ListBranch.Select(s => s.Code).ToList();

            _listTemp = await new HumicaDBContext().HRStaffProfiles.Where(w => branchids.Contains(w.Branch)).ToListAsync();
            _listTemp = _listTemp.Where(w => _ListCompany.Where(x => x.Company == w.CompanyCode).Any()).ToList();
            _listTemp = _listTemp.Where(w => _ListLevel.Where(x => x.Code == w.LevelCode).Any()).ToList();

            ListDP = await new HumicaDBViewContext().HRDelayProbations.ToListAsync();
            ListDP = ListDP.Where(w => _listTemp.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            return ListDP;
        }
        public string creatStaff()
        {
            try
            {
                HumicaDBContext DBM = new HumicaDBContext();
                DB = new HumicaDBContext();
                var _Company = DP.SYHRCompanies.FirstOrDefault();
                string Company = _Company.CompENG;
                if (Header.EmpType == null)
                    return "EMPTYPE_EN";
                if (Header.Branch == null)
                    return "BRANCH_EN";
                if (Header.DEPT == null)
                    return "DEPARTMENT_EN";
                if (Header.JobCode == null)
                    return "POSITION_EN";
                if (Header.PayParam == null)
                    return "PAYPARAM_EN";
                if (Header.TXPayType == null)
                    return "SALRY_PAID_EN";
                if (Header.LevelCode == null)
                    return "LEVEL_EN";
                if (Header.ProbationType == null)
                    return "PROBATION_TYPE_EN";
                if (Header.Nation == null)
                    return "Nation_EN";
                if (Header.SalaryType == null)
                {
                    return "SALARY_TYPE_EN";
                }
                if (IsEmpAuto == true)
                {
                    if (Header.EmpCode == "" || Header.EmpCode is null) return "EMPLOYEE_NE";
                }
                else
                {
                    var objNumber = new CFNumberRank(Header.CompanyCode, Header.Branch, Header.GroupDept, true);
                    if (objNumber.NextNumberRank == null)
                    {
                        return "NUMBER_RANK_NE";
                    }
                    Header.EmpCode = objNumber.NextNumberRank.Trim();
                }
                if(string.IsNullOrEmpty(Header.EmpCode)) return "EMPLOYEE_NE";
                Header.EmpCode = Header.EmpCode.Trim().ToUpper();
                var EmpD = DB.HRStaffProfiles.Where(w => w.EmpCode == Header.EmpCode).ToList();
                if (EmpD.Count() > 0) return "DUPL_KEY";
                bool IsSalary = IsHideSalary(Header.LevelCode);
                if (IsSalary == true) Header.Salary = Convert.ToDecimal(Salary);
                else Header.Salary = 0;
                if (IsTax == true)
                {
                    Header.SalaryTax = Convert.ToDecimal(SalaryTax);
                    Header.SalaryNSSF = Convert.ToDecimal(SalaryNSSF);
                }
                else
                {
                    Header.SalaryTax = 0;
                    Header.SalaryNSSF = 0;
                }
                if (Header.FirstLine == Header.FirstLine2 && !string.IsNullOrEmpty(Header.FirstLine))
                    return "FIRSTLINE_EN";
                if (Header.SecondLine == Header.SecondLine2 && !string.IsNullOrEmpty(Header.SecondLine))
                    return "SECONDLINE_EN";
                if (Header.LeaveConf == null) Header.LeaveConf = Header.StartDate;
                Header.EffectDate = Header.StartDate;
                //Header.AllName = Header.FirstName + " " + Header.LastName;
                //Header.OthAllName = Header.OthFirstName + " " + Header.OthLastName;
                Header.Attachment = Header.Attachment;
                Header.GPSData = Header.GPSData;
                Header.AttachFile = Header.AttachFile;
                //Header.AttachJD = Header.AttachJD;
                //Header.AttachmentJD = Header.AttachmentJD;
                Header.CreatedOn = DateTime.Now.Date;
                Header.CreatedBy = User.UserName;
                Header.DateTerminate = new DateTime(1900, 1, 1);
                Header.Status = SYDocumentStatus.A.ToString();

                foreach (var item in ListEmpIdentity)
                {
                    if (item.Attachment != "")
                    {
                        string DocumentType = ClsFilter.GetSYData("IdentityTye", item.IdentityTye);
                        var strpath = ClsCopyFile.CopyNewStructurePath(DocumentType, Header.EmpCode, item.Attachment);
                        item.Attachment = strpath;
                    }
                    var empIden = new HREmpIdentity();
                    empIden = item;
                    empIden.EmpCode = Header.EmpCode;
                    DBM.HREmpIdentities.Add(empIden);
                }

                //foreach (var read in ListEmpOther)
                //{
                //    lineItem++;
                //    var objLeave = new HREmpOther();
                //    objLeave.EmpCode = Header.EmpCode;
                //    objLeave.UserAcess = read.UserAcess;
                //    objLeave.Descr = read.Descr;
                //    objLeave.Line = lineItem;
                //    DBM.HREmpOthers.Add(objLeave);
                //}

                HeaderCareer = new HREmpCareer();
                HeaderCareer.EmpCode = Header.EmpCode;
                HeaderCareer.CareerCode = Header.CareerDesc;
                HeaderCareer.EmpType = Header.EmpType;
                HeaderCareer.CompanyCode = Header.CompanyCode;
                HeaderCareer.Branch = Header.Branch;
                HeaderCareer.DEPT = Header.DEPT;
                HeaderCareer.LOCT = Header.LOCT;
                HeaderCareer.Division = Header.Division;
                HeaderCareer.LINE = Header.Line;
                HeaderCareer.SECT = Header.SECT;
                HeaderCareer.CATE = Header.CATE;
                HeaderCareer.LevelCode = Header.LevelCode;
                HeaderCareer.JobCode = Header.JobCode;
                HeaderCareer.JobGrade = Header.JobGrade;
                HeaderCareer.JobDesc = Header.POSTDESC;
                HeaderCareer.JobSpec = Header.JOBSPEC;
                HeaderCareer.EstartSAL = Header.Salary.ToString();
                HeaderCareer.EIncrease = Header.Salary.ToString();
                HeaderCareer.ESalary = Header.ESalary;
                HeaderCareer.SupCode = Header.SubFunction;
                HeaderCareer.FromDate = Header.StartDate;
                HeaderCareer.ToDate = Convert.ToDateTime("01/01/5000");
                HeaderCareer.EffectDate = Header.EffectDate;
                HeaderCareer.ProDate = Header.StartDate;
                HeaderCareer.Reason = "New Join";
                HeaderCareer.Remark = "Welcome to " + Company;
                HeaderCareer.Appby = "";
                HeaderCareer.AppDate = Header.StartDate.Value.ToString("dd-MM-yyyy");
                HeaderCareer.VeriFyBy = "";
                HeaderCareer.VeriFYDate = Header.StartDate.Value.ToString("dd-MM-yyyy");
                HeaderCareer.LCK = 1;
                HeaderCareer.OldSalary = Header.Salary;
                HeaderCareer.Increase = 0;
                HeaderCareer.Functions = Header.Functions;
                HeaderCareer.NewSalary = Header.Salary;
                HeaderCareer.JobGrade = Header.JobGrade;
                HeaderCareer.PersGrade = Header.PersGrade;
                HeaderCareer.HomeFunction = Header.HomeFunction;
                HeaderCareer.Functions = Header.Functions;
                HeaderCareer.SubFunction = Header.SubFunction;
                HeaderCareer.StaffType = Header.StaffType;
                HeaderCareer.CreateBy = User.UserName;
                HeaderCareer.CreateOn = DateTime.Now;
                HeaderCareer.GroupDept = Header.GroupDept;
                HeaderCareer.Office = Header.Office;
                HeaderCareer.Groups = Header.Groups;
                //HeaderCareer.AttachJD = Header.AttachJD;
                //HeaderCareer.JobDiscription = Header.JobDiscription;
                //HeaderCareer.JobResponsive = Header.JobResponsive;
                if (!string.IsNullOrEmpty(Header.Images))
                {
                    string savedFilePath = ClsCopyFile.CopyNewStructurePath("PIC", Header.EmpCode, Header.Images, true);
                    Header.Images = savedFilePath;
                }
                DBM.HREmpCareers.Add(HeaderCareer);
                DBM.HRStaffProfiles.Add(Header);

                int row = DBM.SaveChanges();
                //GenerateLeaveObject Leave = new GenerateLeaveObject();
                //Leave.Filter.INYear = Header.LeaveConf.Value.Year;
                //Leave.Filter.TemLeave = "LB";
                //Leave.GenerateLeave(Header.EmpCode);
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
                var objNumber = new CFNumberRank(Header.CompanyCode, Header.Branch, Header.GroupDept, true);
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
                var objNumber = new CFNumberRank(Header.CompanyCode, Header.Branch, Header.GroupDept, true);
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
                var objNumber = new CFNumberRank(Header.CompanyCode, Header.Branch, Header.GroupDept, true);
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CreatProbation()
        {
            try
            {
                if (HeaderDP.Reason == "" || HeaderDP.Reason == null)
                    return "REASON_EN";
                HeaderDP.EmpCode = HeaderStaff.EmpCode;
                var ObjMatch = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == HeaderDP.EmpCode);
                if (ObjMatch == null)
                {
                    return "STAFF_NE";
                }
                var ProType = DB.HRProbationTypes.FirstOrDefault(w => w.Code == ObjMatch.ProbationType);
                DateTime Prob = ObjMatch.StartDate.Value.AddMonths(ProType.InMonth);
                var _listProb = DB.HRDelayProbations.ToList();
                int ProbM = _listProb.Where(x => x.EmpCode == HeaderDP.EmpCode).Sum(w => w.Day);

                ObjMatch.Probation = Prob.AddDays(ProbM + HeaderDP.Day);
                DB.HRStaffProfiles.Attach(ObjMatch);
                DB.Entry(ObjMatch).Property(w => w.Probation).IsModified = true;

                HeaderDP.AllName = ObjMatch.AllName;
                HeaderDP.EndProbation = Convert.ToDateTime(ObjMatch.Probation);
                HeaderDP.Probation = Prob.AddDays(ProbM);
                HeaderDP.CretaedBy = User.UserName;
                HeaderDP.CreatedOn = DateTime.Now;
                DB.HRDelayProbations.Add(HeaderDP);
                var row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderDP.EmpCode;
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
                log.DocurmentAction = HeaderDP.EmpCode;
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
                log.DocurmentAction = HeaderDP.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditStaff(string ID)
        {
            try
            {
                DB = new HumicaDBContext();
                HRStaffProfile objMast = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == ID);
                if (objMast == null)
                {
                    return "STAFF_NE";
                }
                if (Header.SalaryType == null)
                {
                    return "SALARY_TYPE_EN";
                }
                DateTime LeaveDate = objMast.LeaveConf.Value;
                objMast.FirstName = Header.FirstName;
                objMast.LastName = Header.LastName;
                objMast.AllName = Header.AllName;
                //objMast.AllName = Header.FirstName + " " + Header.LastName;
                objMast.OthFirstName = Header.OthFirstName;
                objMast.OthLastName = Header.OthLastName;
                objMast.OthAllName = Header.OthAllName;
                // objMast.OthAllName = Header.OthFirstName + " " + Header.OthLastName;
                objMast.Sex = Header.Sex;
                objMast.Title = Header.Title;
                objMast.CardNo = Header.CardNo;
                objMast.Marital = Header.Marital;
                objMast.POB = Header.POB;
                objMast.Country = Header.Country;
                objMast.Nation = Header.Nation;
                objMast.State = Header.State;
                objMast.Phone1 = Header.Phone1;
                objMast.Phone2 = Header.Phone2;
                objMast.PhoneExt = Header.PhoneExt;
                objMast.Email = Header.Email;
                objMast.Email2 = Header.Email2;
                objMast.PayParam = Header.PayParam;
                objMast.TXPayType = Header.TXPayType;
                objMast.BankName = Header.BankName;
                objMast.BankAccName = Header.BankAccName;
                objMast.BankAcc = Header.BankAcc;
                objMast.Peraddress = Header.Peraddress;
                objMast.PerPhone = Header.PerPhone;
                objMast.PeraddressOth = Header.PeraddressOth;
                objMast.ChangedOn = DateTime.Now;
                objMast.IsOTApproval = Header.IsOTApproval;
                objMast.ChangedBy = User.UserName;
                objMast.Images = Header.Images;
                objMast.Signature = Header.Signature;
                objMast.PostFamily = Header.PostFamily;
                objMast.SOCSO = Header.SOCSO;
                objMast.ISNSSF = Header.ISNSSF;
                objMast.IsResident = Header.IsResident;
                objMast.HODCode = Header.HODCode;
                objMast.SalaryProb = Header.SalaryProb;
                objMast.IsHold = Header.IsHold;
                objMast.ROSTER = Header.ROSTER;
                objMast.ProbationType = Header.ProbationType;
                objMast.Probation = Header.Probation;
                objMast.ReSalary = Header.ReSalary;
                objMast.SalaryFlag = Header.SalaryFlag;
                objMast.DOB = Header.DOB;
                objMast.TeleGroup = Header.TeleGroup;
                objMast.TeleGroupOT = Header.TeleGroupOT;
                objMast.TeleGroupScan = Header.TeleGroupScan;
                objMast.TeleChartID = Header.TeleChartID;
                if (Header.FirstLine == Header.FirstLine2 && !string.IsNullOrEmpty(Header.FirstLine))
                    return "FIRSTLINE_EN";
                if (Header.SecondLine == Header.SecondLine2 && !string.IsNullOrEmpty(Header.SecondLine))
                    return "SECONDLINE_EN";
                objMast.FirstLine = Header.FirstLine;
                objMast.SecondLine = Header.SecondLine;
                objMast.FirstLine2 = Header.FirstLine2;
                objMast.SecondLine2 = Header.SecondLine2;
                objMast.Supervisory = Header.Supervisory;
                objMast.BankFee = Header.BankFee;
                objMast.CheckSum = Header.CheckSum;
                objMast.BankBranch = Header.BankBranch;
                objMast.BankAccType = Header.BankAccType;
                objMast.Province = Header.Province;
                objMast.District = Header.District;
                objMast.Commune = Header.Commune;
                objMast.Village = Header.Village;
                objMast.TID = Header.TID;
                objMast.Costcent = Header.Costcent;
                objMast.SalaryType = Header.SalaryType;
                objMast.IsAtten = Header.IsAtten;
                objMast.StaffType = Header.StaffType;
                objMast.LeaveConf = Header.LeaveConf;
                objMast.HODCode = Header.HODCode;
                objMast.NSSFContributionType = Header.NSSFContributionType;
                objMast.IsCalSalary = Header.IsCalSalary;
                objMast.TemLeave = Header.TemLeave;
                objMast.IsPayPartial = Header.IsPayPartial;
                objMast.ConAddress = Header.ConAddress;
                objMast.ConAddressOth = Header.ConAddressOth;
                objMast.PassPayslip = Header.PassPayslip;
                objMast.OTFirstLine = Header.OTFirstLine;
                objMast.OTSecondLine = Header.OTSecondLine;
                objMast.OTthirdLine = Header.OTthirdLine;
                objMast.IsMealAllowance = Header.IsMealAllowance;
                objMast.GPSData = Header.GPSData;
                objMast.AttachFile = Header.AttachFile;
                objMast.Attachment = Header.Attachment;
                objMast.IsAutoAppLeave = Header.IsAutoAppLeave;
                objMast.IsAutoAppKPITraing = Header.IsAutoAppKPITraing;
                //objMast.JobDiscription = Header.JobDiscription;
                //objMast.JobResponsive = Header.JobResponsive;
                //objMast.AttachJD = Header.AttachJD;
                //objMast.AttachmentJD = Header.AttachmentJD;
                objMast.IsOnsite = Header.IsOnsite;
                objMast.APPTracking = Header.APPTracking;
                objMast.APPEvaluator = Header.APPEvaluator;
                objMast.APPAppraisal = Header.APPAppraisal;
                objMast.APPAppraisal2 = Header.APPAppraisal2;
                //var DBD = new HumicaDBContext();
                var objId = DB.HREmpIdentities.Where(w => w.EmpCode == Header.EmpCode).ToList();
                foreach (var item in objId)
                {
                    DB.HREmpIdentities.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                foreach (var item in ListEmpIdentity)
                {
                    if (item.Attachment != "")
                    {
                        string DocumentType = ClsFilter.GetSYData("IdentityTye", item.IdentityTye);
                        var strpath = ClsCopyFile.CopyNewStructurePath(DocumentType, Header.EmpCode, item.Attachment);
                        item.Attachment = strpath;
                    }

                    var empIden = new HREmpIdentity();
                    empIden.EmpCode = Header.EmpCode;
                    empIden.IdentityTye = item.IdentityTye;
                    empIden.IDCardNo = item.IDCardNo;
                    empIden.IssueDate = item.IssueDate;
                    empIden.ExpiryDate = item.ExpiryDate;
                    empIden.Attachment = item.Attachment;
                    DB.HREmpIdentities.Add(empIden);
                }

                //if (!string.IsNullOrEmpty(Header.Images))
                //{
                //    string savedFilePath = ClsCopyFile.CopyNewStructurePath("PIC", Header.EmpCode, Header.Images, true);
                //    objMast.Images = savedFilePath;
                //}
                //HumicaDBContext DBM = new HumicaDBContext();

                DB.Entry(objMast).State = System.Data.Entity.EntityState.Modified;
                int row = DB.SaveChanges();
                if (LeaveDate.Date != Header.LeaveConf.Value.Date)
                {
                    //GenerateLeaveObject Leave = new GenerateLeaveObject();
                    //Leave.GenerateLeave(Header, Header.LeaveConf.Value.Year);
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
        public string EditProbation(int ID)
        {
            try
            {
                var objMast = DB.HRDelayProbations.FirstOrDefault(w => w.TranNo == ID);
                if (objMast == null)
                {
                    return "STAFF_NE";
                }
                HeaderDP.EmpCode = objMast.EmpCode;
                var ObjMatch = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == HeaderDP.EmpCode);
                if (ObjMatch == null)
                {
                    return "STAFF_NE";
                }

                objMast.Day = HeaderDP.Day;
                objMast.ChangedOn = DateTime.Now;
                objMast.ChangedBy = User.UserName;
                var ProType = DB.HRProbationTypes.FirstOrDefault(w => w.Code == ObjMatch.ProbationType);
                DateTime Prob = ObjMatch.StartDate.Value.AddMonths(ProType.InMonth);
                var _listProb = DB.HRDelayProbations.ToList();
                int ProbM = _listProb.Where(x => x.TranNo != objMast.TranNo && x.EmpCode == HeaderDP.EmpCode).Sum(w => w.Day) + HeaderDP.Day;
                ObjMatch.Probation = Prob.AddDays(ProbM);
                DB.HRStaffProfiles.Attach(ObjMatch);
                DB.Entry(ObjMatch).Property(w => w.Probation).IsModified = true;

                objMast.EndProbation = HeaderDP.EndProbation;
                objMast.Reason = HeaderDP.Reason;
                DB.HRDelayProbations.Attach(objMast);
                DB.Entry(objMast).Property(w => w.Probation).IsModified = true;
                DB.Entry(objMast).Property(w => w.EndProbation).IsModified = true;
                DB.Entry(objMast).Property(w => w.Day).IsModified = true;
                DB.Entry(objMast).Property(w => w.Reason).IsModified = true;
                DB.Entry(objMast).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objMast).Property(w => w.ChangedOn).IsModified = true;
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
        public string CreateCareerStaff()
        {
            try
            {
                HumicaDBContext DBI = new HumicaDBContext();
                HRStaffProfile objMatchHeader = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                //var objMatch = DB.HREmpCareers.FirstOrDefault(w => w.TranNo == HeaderCareer.TranNo && w.EmpCode == ID);
                ListCareer = new List<HREmpCareer>();
                var LstCareerCode = DB.HRCareerHistories.ToList();
                ListCareer = DB.HREmpCareers.Where(w => w.EmpCode == objMatchHeader.EmpCode).ToList();
                if (objMatchHeader == null)
                {
                    return "CAREERHISTORY_NE";
                }
                var _CarCode = LstCareerCode.FirstOrDefault(x => x.Code == HeaderCareer.CareerCode);
                if (_CarCode == null)
                {
                    return "CAREERCODE_EN";
                }
                else if (_CarCode.NotCalSalary == true)
                {
                    var LstResType = DB.HRTerminTypes.Where(w => w.Code == HeaderCareer.resigntype).ToList();
                    if (LstResType.Count == 0)
                    {
                        return "SEPARATE_TYPE_EN";
                    }
                }
                if (ListCareer.Where(w => w.FromDate.Value.Date >= HeaderCareer.EffectDate.Value.Date).Any())
                {
                    return "INV_DATE";
                }
                if (ListCareer.Where(w => w.EmpCode == objMatchHeader.EmpCode).ToList().Count > 0)
                {
                    var result = ListCareer.OrderByDescending(x => x.FromDate).FirstOrDefault();
                    result.LCKEDIT = 1;
                    if (!string.IsNullOrEmpty(HeaderCareer.NewEmpCode))
                    {
                        NewEmpCode = HeaderCareer.NewEmpCode;
                        result.NewEmpCode = HeaderStaff.EmpCode;
                        var OtherStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == HeaderCareer.NewEmpCode);
                        var OtherCareer = DB.HREmpCareers.FirstOrDefault(w => w.NewEmpCode == HeaderCareer.NewEmpCode);
                        if (OtherStaff != null || OtherCareer != null) return "DUPLICATE_EMP";
                    }
                    result.ToDate = HeaderCareer.EffectDate.Value.AddDays(-1);
                    DBI.HREmpCareers.Attach(result);
                    DBI.Entry(result).Property(w => w.LCKEDIT).IsModified = true;
                    DBI.Entry(result).Property(w => w.NewEmpCode).IsModified = true;
                    DBI.Entry(result).Property(w => w.ToDate).IsModified = true;
                    int row1 = DBI.SaveChanges();
                }
                bool IsSalary = IsHideSalary(HeaderCareer.LevelCode);
                if (IsSalary == true)
                {
                    HeaderCareer.OldSalary = objMatchHeader.Salary;
                    HeaderCareer.NewSalary = Convert.ToDecimal(NewSalary);
                    HeaderCareer.Increase = Convert.ToDecimal(Increase);
                }
                else
                {
                    HeaderCareer.OldSalary = objMatchHeader.Salary;
                    HeaderCareer.NewSalary = objMatchHeader.Salary;
                    HeaderCareer.Increase = 0;
                }
                HeaderCareer.NewSalary = HeaderCareer.OldSalary + HeaderCareer.Increase;
                HeaderCareer.EmpCode = HeaderStaff.EmpCode;
                HeaderCareer.ProDate = HeaderCareer.EffectDate;
                HeaderCareer.FromDate = HeaderCareer.EffectDate;
                HeaderCareer.ToDate = new DateTime(5000, 1, 1);
                HeaderCareer.CreateBy = User.UserName;
                HeaderCareer.CreateOn = DateTime.Now;
                objMatchHeader.Status = SYDocumentStatus.A.ToString();
                //Update Staff Profile
                if (HeaderCareer.CareerCode == "NEWJOIN")
                {

                    var ProType = DB.HRProbationTypes.FirstOrDefault(w => w.Code == objMatchHeader.ProbationType);
                    objMatchHeader.StartDate = HeaderCareer.EffectDate;
                    objMatchHeader.Probation = objMatchHeader.StartDate.Value.AddMonths(ProType.InMonth);
                    objMatchHeader.LeaveConf = objMatchHeader.StartDate;
                    HeaderCareer.resigntype = "";
                }
                if (_CarCode.NotCalSalary == true)
                // if (HeaderCareer.CareerCode == "TERMINAT" || HeaderCareer.CareerCode == "RES")
                {
                    //  HeaderCareer.resigntype = "RES";
                    objMatchHeader.DateTerminate = HeaderCareer.EffectDate.Value;
                    objMatchHeader.TerminateStatus = HeaderCareer.CareerCode;
                    objMatchHeader.TerminateRemark = HeaderCareer.Remark;
                    objMatchHeader.Status = SYDocumentStatus.I.ToString();
                    var user = DP.SYUsers.FirstOrDefault(w => w.UserName == objMatchHeader.EmpCode);
                    if (user != null)
                    {
                        user.ExpireDate = HeaderCareer.EffectDate.Value.AddDays(-1);
                        DP.SYUsers.Attach(user);
                        DP.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                else
                {
                    objMatchHeader.DateTerminate = new DateTime(1900, 1, 1);
                    objMatchHeader.TerminateStatus = "";
                    objMatchHeader.TerminateRemark = "";
                }

                SwapStaff(objMatchHeader, HeaderCareer);

                DBI = new HumicaDBContext();
                DBI.HREmpCareers.Add(HeaderCareer);
                DBI.HRStaffProfiles.Attach(objMatchHeader);
                DBI.Entry(objMatchHeader).State = System.Data.Entity.EntityState.Modified;
                int row = DBI.SaveChanges();
                DP.SaveChanges();
                if (HeaderCareer.ALRemaining == true)
                {
                    Generate_Leave_Cu(objMatchHeader, HeaderCareer.EffectDate.Value.Year, HeaderStaff.EmpCode);
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderCareer.EmpCode;
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
                log.DocurmentAction = HeaderCareer.EmpCode;
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
                log.DocurmentAction = HeaderCareer.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public void Generate_Leave_Cu(HRStaffProfile _staffPro, int InYear, string EmpCode)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                DateTime DateNow = DateTime.Now;
                DateTime StartDate = new DateTime(InYear, 1, 1);
                DateTime FromDate = new DateTime(InYear, 1, 1);
                DateTime ToDate = new DateTime(InYear, HeaderCareer.EffectDate.Value.Month, DateTime.DaysInMonth(InYear, HeaderCareer.EffectDate.Value.Month));
                var ListParam = DB.PRParameters.ToList();
                //var _listStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode== EmpCode);
                string approved = SYDocumentStatus.APPROVED.ToString();
                var EmpLeaveB = DB.HREmpLeaveBs.ToList();
                var EmpLeave = DB.HREmpLeaves.ToList();
                var empLeaveD = DB.HREmpLeaveDs.ToList();
                var LeaveType = DB.HRLeaveTypes.Where(w => w.IsParent == true).ToList();
                List<string> _lstStr = new List<string>();
                _lstStr.Add("AL");
                foreach (var item in LeaveType.Where(w => w.Parent == "AL").ToList())
                {
                    _lstStr.Add(item.Code);
                }
                var _ListLeave = EmpLeave.Where(w => w.Status == approved).ToList();
                List<HREmpLeaveD> _ListLeaveD = empLeaveD.Where(w => w.LeaveDate.Year == InYear && _lstStr.Where(x => x.Contains(w.LeaveCode)).Any()).ToList();
                _ListLeaveD = _ListLeaveD.Where(w => _ListLeave.Where(x => x.Increment == w.LeaveTranNo).Any()).ToList();

                EmpLeaveB = EmpLeaveB.Where(w => w.InYear == InYear && w.LeaveCode == "AL" && w.EmpCode == EmpCode).ToList();
                var ListLeave_Rate = DB.HRLeaveProRates.ToList();
                foreach (var emp in EmpLeaveB)
                {
                    var ObjMatch = emp;
                    decimal Balance = 0;
                    decimal Balance_new = 0;
                    decimal Balance_Res = 0;
                    decimal prorateAmount = 0;
                    PRParameter payParam = DB.PRParameters.Find(_staffPro.PayParam);
                    DateTime MonthLy = new DateTime();
                    var WHDPay = ListParam.FirstOrDefault(w => w.Code == _staffPro.PayParam);
                    var Leave = _ListLeaveD.Where(w => w.EmpCode == emp.EmpCode && w.LeaveDate.Date <= ToDate.Date).ToList();
                    decimal User_AL = (decimal)Leave.Sum(w => w.LHour) / (decimal)WHDPay.WHOUR;
                    if (_staffPro.LeaveConf.Value.Date > StartDate.Date)
                    {
                        MonthLy = _staffPro.LeaveConf.Value;
                        var LeaveRate = ListLeave_Rate.Where(w => w.Status == "NEWJOIN" && w.LeaveType == emp.LeaveCode).ToList();
                        //Balance_new = LeaveRate.Where(w => w.FromDay <= MonthLy.Day && MonthLy.Day <= w.ToDay).Sum(x => x.Rate);
                        DateTime EndDatetOfMonth = MonthLy.EndDateOfMonth();
                        DateTime LeaveCofDate = Convert.ToDateTime(_staffPro.LeaveConf);
                        decimal actWorkDay = PRPayParameterObject.Get_WorkingDay(payParam, MonthLy, EndDatetOfMonth);
                        HRLeaveProRate prorateLeave = LeaveRate.Where(w => w.ActWorkDayFrom <= actWorkDay && w.ActWorkDayTo >= actWorkDay).FirstOrDefault();
                        prorateAmount = prorateLeave == null ? 0 : prorateLeave.Rate;
                        MonthLy = MonthLy.AddMonths(1).StartDateOfMonth();
                    }
                    else
                    {
                        MonthLy = StartDate;
                        Balance_new = 1.5M;
                    }
                    var _listLeaveProRate = DB.HRLeaveProRates.ToList();
                    DateTime _Todate = new DateTime();
                    if (_staffPro.DateTerminate.Year != 1900)
                    {
                        var Leave_Rate = _listLeaveProRate.Where(w => w.Status == "RESIGN" && w.LeaveType == "AL").ToList();
                        DateTime FromDateInMonth = Convert.ToDateTime(_staffPro.DateTerminate).StartDateOfMonth();
                        DateTime LeaveCofDate = Convert.ToDateTime(_staffPro.LeaveConf);
                        //DateTime EndDatetOfMonth = Convert.ToDateTime(_staffPro.DateTerminate).EndDateOfMonth();
                        FromDateInMonth = LeaveCofDate >= FromDateInMonth ? LeaveCofDate : FromDateInMonth;
                        _Todate = _staffPro.DateTerminate.AddDays(-1);
                        decimal actWorkDay = PRPayParameterObject.Get_WorkingDay(payParam, FromDateInMonth, _Todate);
                        HRLeaveProRate prorateLeave = Leave_Rate.Where(w => w.ActWorkDayFrom <= actWorkDay && w.ActWorkDayTo >= actWorkDay).FirstOrDefault();
                        prorateAmount += prorateLeave == null ? 0 : prorateLeave.Rate;
                    }
                    else
                    {
                        _Todate = ToDate;
                    }
                    Balance = Convert.ToDecimal(1.5 * MonthDiff(MonthLy, _Todate));
                    Balance += prorateAmount;
                    if (emp.ForwardUse > 0)
                    {
                        if (emp.ForwardUse == emp.Forward)
                        {
                            ObjMatch.ALPayBalance = Balance + emp.Forward - User_AL;
                        }
                        else
                        {
                            //if(emp.ForWardExp>HeaderCareer.EffectDate)
                            //    ObjMatch.ALPayBalance = (Balance - emp.ForwardUse) + emp.Forward;
                            //else 
                            ObjMatch.ALPayBalance = Balance;//-User_AL;
                        }

                    }
                    else
                        ObjMatch.ALPayBalance = Balance - User_AL; //+ Balance_Res;
                    ObjMatch.ALPayMonth = HeaderCareer.EffectDate;
                    ObjMatch.InMonth = HeaderCareer.EffectDate.Value.Month;
                    DB.HREmpLeaveBs.Attach(ObjMatch);
                    DB.Entry(ObjMatch).Property(w => w.ALPayBalance).IsModified = true;
                    DB.Entry(ObjMatch).Property(w => w.ALPayMonth).IsModified = true;
                    DB.Entry(ObjMatch).Property(w => w.InMonth).IsModified = true;
                }
                var row = DB.SaveChanges();
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public static int MonthDiff(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
        public string EditCareerStaff(string ID)
        {
            try
            {
                int TranNo = Convert.ToInt32(ID);
                HumicaDBContext DBI = new HumicaDBContext();
                HRStaffProfile objMatchHeader = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                //var objMatch = DB.HREmpCareers.FirstOrDefault(w => w.TranNo == HeaderCareer.TranNo && w.EmpCode == ID);
                var objCareer = DB.HREmpCareers.Where(w => w.EmpCode == HeaderCareer.EmpCode).ToList();
                var objMatch = objCareer.FirstOrDefault(w => w.TranNo == HeaderCareer.TranNo && w.EmpCode == HeaderStaff.EmpCode);
                if (objMatch == null)
                {
                    return "CAREERHISTORY_NE";
                }
                if (objMatchHeader == null)
                {
                    return "CAREERHISTORY_NE";
                }
                var LstCareerCode = DB.HRCareerHistories.ToList();
                var _CarCode = LstCareerCode.FirstOrDefault(x => x.Code == HeaderCareer.CareerCode);
                if (_CarCode == null)
                {
                    return "CAREERCODE_EN";
                }
                else if (_CarCode.NotCalSalary == true)
                {
                    IsInAtive = Convert.ToBoolean(_CarCode.NotCalSalary);
                    var LstResType = DB.HRTerminTypes.Where(w => w.Code == HeaderCareer.resigntype).ToList();
                    if (LstResType.Count == 0)
                    {
                        return "SEPARATE_TYPE_EN";
                    }
                }
                IsInAtive = Convert.ToBoolean(_CarCode.NotCalSalary);
                ListCareer = new List<HREmpCareer>();
                ListCareer = objCareer.Where(w => w.EmpCode == HeaderCareer.EmpCode).ToList();
                HREmpCareer Career = ListCareer.FirstOrDefault(w => w.EmpCode == HeaderCareer.EmpCode && w.TranNo == HeaderCareer.TranNo);
                bool IsSalary = IsHideSalary(HeaderCareer.LevelCode);
                if (IsSalary == true)
                {
                    HeaderCareer.OldSalary = Career.OldSalary;
                    HeaderCareer.NewSalary = Convert.ToDecimal(NewSalary);
                    HeaderCareer.Increase = Convert.ToDecimal(Increase);
                }
                else
                {
                    HeaderCareer.OldSalary = Career.OldSalary;
                    HeaderCareer.NewSalary = Career.NewSalary;
                    HeaderCareer.Increase = Career.Increase;
                }
                if (ListCareer.Where(w => w.TranNo != HeaderCareer.TranNo && w.FromDate.Value.Date >= HeaderCareer.EffectDate.Value.Date).Any())
                {
                    return "INV_DATE";
                }
                if (ListCareer.Where(w => w.TranNo < HeaderCareer.TranNo).ToList().Count > 0)
                {
                    var result = ListCareer.Where(w => w.TranNo < HeaderCareer.TranNo).OrderByDescending(x => x.FromDate).FirstOrDefault();
                    result.LCKEDIT = 1;
                    result.ToDate = HeaderCareer.EffectDate.Value.AddDays(-1);
                    DBI.HREmpCareers.Attach(result);
                    DBI.Entry(result).Property(w => w.LCKEDIT).IsModified = true;
                    DBI.Entry(result).Property(w => w.ToDate).IsModified = true;
                    int row1 = DBI.SaveChanges();
                }
                if (!string.IsNullOrEmpty(HeaderCareer.NewEmpCode))
                {
                    NewEmpCode = HeaderCareer.NewEmpCode;
                    objMatch.EmpCode = HeaderCareer.NewEmpCode;
                    var OtherStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == HeaderCareer.NewEmpCode);
                    if (OtherStaff != null) return "DUPLICATE_EMP";
                }
                objMatch.CompanyCode = HeaderCareer.CompanyCode;
                objMatch.Functions = HeaderCareer.Functions;
                objMatch.CareerCode = HeaderCareer.CareerCode;
                objMatch.Functions = HeaderCareer.Functions;
                objMatch.EmpType = HeaderCareer.EmpType;
                objMatch.Branch = HeaderCareer.Branch;
                objMatch.LOCT = HeaderCareer.LOCT;
                objMatch.resigntype = HeaderCareer.resigntype;
                objMatch.Division = HeaderCareer.Division;
                objMatch.DEPT = HeaderCareer.DEPT;
                objMatch.LINE = HeaderCareer.LINE;
                objMatch.CATE = HeaderCareer.CATE;
                objMatch.SECT = HeaderCareer.SECT;
                objMatch.JobCode = HeaderCareer.JobCode;
                objMatch.JobGrade = HeaderCareer.JobGrade;
                objMatch.JobDesc = HeaderCareer.JobDesc;
                objMatch.JobSpec = HeaderCareer.JobSpec;
                objMatch.OldSalary = HeaderCareer.OldSalary;
                objMatch.Increase = HeaderCareer.Increase;
                objMatch.NewSalary = HeaderCareer.NewSalary;
                objMatch.EstartSAL = "";
                objMatch.EIncrease = "";
                objMatch.ESalary = "";
                objMatch.SupCode = HeaderCareer.SupCode;
                objMatch.LevelCode = HeaderCareer.LevelCode;
                objMatch.LastDate = HeaderCareer.LastDate;
                objMatch.Reason = HeaderCareer.Reason;
                objMatch.Remark = HeaderCareer.Remark;
                objMatch.ProDate = HeaderCareer.EffectDate;
                objMatch.EffectDate = HeaderCareer.EffectDate;
                objMatch.FromDate = HeaderCareer.EffectDate;
                objMatch.resigntype = HeaderCareer.resigntype;
                objMatch.StaffType = HeaderCareer.StaffType;
                objMatch.ToDate = new DateTime(5000, 1, 1);
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                objMatch.GroupDept = HeaderCareer.GroupDept;
                objMatch.Office = HeaderCareer.Office;
                objMatch.Groups = HeaderCareer.Groups;
                objMatch.AttachFile= HeaderCareer.AttachFile;
                //objMatch.JobDiscription = HeaderCareer.JobDiscription;
                //objMatch.JobResponsive = HeaderCareer.JobResponsive;
                //objMatch.AttachJD = HeaderCareer.AttachJD;
                objMatchHeader.Status = SYDocumentStatus.A.ToString();
                //Update Staff Profile
                if (HeaderCareer.CareerCode == "NEWJOIN")
                {
                    var ProType = DB.HRProbationTypes.FirstOrDefault(w => w.Code == objMatchHeader.ProbationType);
                    if (ProType != null)
                    {
                        objMatchHeader.StartDate = HeaderCareer.EffectDate;
                        objMatchHeader.Probation = objMatchHeader.StartDate.Value.AddMonths(ProType.InMonth);
                        objMatchHeader.LeaveConf = objMatchHeader.StartDate;
                        HeaderCareer.resigntype = "";
                    }
                }
                if (_CarCode.NotCalSalary == true)
                // if (HeaderCareer.CareerCode == "TERMINAT" || HeaderCareer.CareerCode == "RES")
                {
                    // HeaderCareer.resigntype = "RES";
                    objMatchHeader.DateTerminate = HeaderCareer.EffectDate.Value;
                    objMatchHeader.TerminateStatus = HeaderCareer.CareerCode;
                    objMatchHeader.TerminateRemark = HeaderCareer.Remark;
                    objMatchHeader.Status = SYDocumentStatus.I.ToString();
                }
                else
                {
                    HeaderCareer.resigntype = "";
                    objMatchHeader.DateTerminate = new DateTime(1900, 1, 1);
                    objMatchHeader.TerminateStatus = "";
                    objMatchHeader.TerminateRemark = "";
                }
                SwapStaff(objMatchHeader, HeaderCareer);
                DB.HREmpCareers.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Modified;
                DB.HRStaffProfiles.Attach(objMatchHeader);
                DB.Entry(objMatchHeader).State = System.Data.Entity.EntityState.Modified;

                if (objMatch.CareerCode == "TERMINAT")
                {
                    if (HeaderCareer.ALRemaining == true)
                    {
                        Generate_Leave_Cu(objMatchHeader, HeaderCareer.EffectDate.Value.Year, HeaderStaff.EmpCode);
                    }
                    else
                    {
                        var Match = DB.HREmpLeaveBs.FirstOrDefault(x => x.EmpCode == objMatch.EmpCode && x.InYear == objMatch.EffectDate.Value.Year && x.LeaveCode == "AL");
                        if (Match != null)
                        {
                            Match.ALPayBalance = 0;
                            Match.ALPayMonth = null;
                            DB.HREmpLeaveBs.Attach(Match);
                            DB.Entry(Match).Property(w => w.ALPayBalance).IsModified = true;
                            DB.Entry(Match).Property(w => w.ALPayMonth).IsModified = true;
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
                log.DocurmentAction = HeaderCareer.EmpCode;
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
                log.DocurmentAction = HeaderCareer.EmpCode;
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
                log.DocurmentAction = HeaderCareer.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string Delete_EmployeeCareerHistory(string id)
        {
            try
            {
                using (var DB = new HumicaDBContext())
                {
                    int TranNo = Convert.ToInt32(id);
                    HeaderCareer = new HREmpCareer();
                    HeaderCareer.TranNo = TranNo;
                    var objMatch = DB.HREmpCareers.Find(TranNo);
                    if (objMatch == null)
                    {
                        return "CAREERHISTORY_NE";
                    }

                    if (objMatch.CareerCode == "TERMINAT")
                    {
                        var matchLeave = DB.HREmpLeaveBs.FirstOrDefault(x => x.EmpCode == objMatch.EmpCode && x.InYear == objMatch.EffectDate.Value.Year && x.LeaveCode == "AL");
                        if (matchLeave != null)
                        {
                            matchLeave.ALPayBalance = 0;
                            matchLeave.ALPayMonth = null;
                            matchLeave.InMonth = null;
                            DB.Entry(matchLeave).State = System.Data.Entity.EntityState.Modified;
                        }
                    }

                    if (objMatch.LCK == 0 || objMatch.LCK == null)
                    {
                        var listCareer = DB.HREmpCareers.Where(w => w.EmpCode == objMatch.EmpCode).ToList();
                        var previousCareer = listCareer.Where(w => w.TranNo < TranNo).OrderByDescending(x => x.FromDate).FirstOrDefault();

                        if (previousCareer != null)
                        {
                            if (!string.IsNullOrEmpty(previousCareer.NewEmpCode))
                            {
                                NewEmpCode = objMatch.EmpCode;
                                OldEmpCode = previousCareer.NewEmpCode;
                            }

                            previousCareer.ToDate = new DateTime(5000, 1, 1);
                            DB.Entry(previousCareer).State = System.Data.Entity.EntityState.Modified;
                        }

                        DB.Entry(objMatch).State = System.Data.Entity.EntityState.Deleted;
                        DB.SaveChanges();
                    }

                    return SYConstant.OK;
                }
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = HeaderCareer.TranNo.ToString();
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
                log.ScreenId = HeaderCareer.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public void IsValidChecking(int ID)
        {
            var objMatch = DB.HREmpCareers.Find(ID);
            if (objMatch != null)
            {
                HeaderCareer = objMatch;
                HeaderCareer.OldSalary = objMatch.NewSalary;
                HeaderCareer.Increase = 0;
                HeaderCareer.NewSalary = objMatch.NewSalary;
                HeaderCareer.EffectDate = DateTime.Now;
                HeaderCareer.CareerCode = "";
            }
        }
        public bool IsHideSalary(string Level)
        {
            var ListLevel = DB.SYHRModifySalarys.Where(w => w.UserName == User.UserName && w.Level == Level).ToList();
            if (ListLevel.Count > 0)
            {
                return true;
            }
            //else if (User.IsSalary == true)
            //    return true;
            return false;
        }

        #region "Upload Staff" 
        public string uploadStaff()
        {
            DB = new HumicaDBContext();
            DB.Configuration.AutoDetectChangesEnabled = false;
            try
            {
                if (ListHeader.Count == 0)
                {
                    return "NO_DATA";
                }
                var date = DateTime.Now;
                var ListLevel = DP.HRLevels.ToList();
                var listBranch = DP.HRBranches.ToList();
                //var ListProType = DB.HRProbationTypes.ToList();
                //HRProbationType _ProType = new HRProbationType();
                //_ProType.Code = "PP003";
                //_ProType.InMonth = 3;
                //if (ListProType.Count > 0)
                //{
                //    var Min = ListProType.Min(w => w.InMonth);
                //    _ProType = ListProType.FirstOrDefault(w => w.InMonth == Min);
                //}
                var FIRST_LASTNAME = SYSettings.getSetting("FIRST_LASTNAME");
                var FIRST_LASTNAME_KH = SYSettings.getSetting("FIRST_LASTNAME_KH");

                foreach (var staffs in ListHeader.ToList())
                {
                    var checkEmp = DB.HRStaffProfiles.Where(w => w.EmpCode == staffs.EmpCode && w.CardNo == staffs.CardNo).ToList();
                    if (checkEmp.Count == 0 && (staffs.EmpCode != null || staffs.EmpCode != ""))
                    {
                        ErrorMessage = staffs.EmpCode;
                        MessageError = staffs.EmpCode;
                        var Company = DB.HRCompanies.FirstOrDefault(w => w.Company == staffs.CompanyCode);
                        if (Company == null || string.IsNullOrEmpty(staffs.CompanyCode)) return "COMPANY_EN";
                        string CompanyName = Company.Description;
                        if (String.IsNullOrEmpty(staffs.Branch) || listBranch.Where(w => w.Code == staffs.Branch).ToList().Count == 0)
                        {
                            return "BRANCH_EN";
                        }
                        if (String.IsNullOrEmpty(staffs.LevelCode) || ListLevel.Where(w => w.Code == staffs.LevelCode).ToList().Count == 0)
                        {
                            return "LEVEL_EN";
                        }
                        if (string.IsNullOrEmpty(staffs.ProbationType))
                        {
                            return "PROBATION_TYPE";
                        }
                        Header = new HRStaffProfile();
                        Header.EmpCode = staffs.EmpCode;
                        Header.FirstName = staffs.FirstName;
                        Header.LastName = staffs.LastName;
                        Header.OthFirstName = staffs.OthFirstName;
                        Header.OthLastName = staffs.OthLastName;
                        Header.Sex = staffs.Sex;
                        Header.CompanyCode = staffs.CompanyCode;
                        Header.Title = staffs.Title;
                        Header.Marital = staffs.Marital;
                        Header.CardNo = staffs.CardNo;
                        Header.DOB = staffs.DOB;
                        Header.POB = staffs.POB;
                        Header.Country = staffs.Country;
                        Header.Nation = staffs.Nation;
                        Header.Race = staffs.Race;
                        Header.Religion = staffs.Religion;
                        Header.State = staffs.State;
                        Header.Phone1 = staffs.Phone1;
                        Header.Phone2 = staffs.Phone2;
                        Header.PhoneExt = staffs.PhoneExt;
                        Header.Email2 = staffs.Email2;
                        Header.Email = staffs.Email;
                        Header.SOCSO = staffs.SOCSO;
                        Header.Peraddress = staffs.Peraddress;
                        Header.PeraddressOth = staffs.PeraddressOth;
                        Header.EmpType = staffs.EmpType;
                        Header.Branch = staffs.Branch;
                        Header.LOCT = staffs.LOCT;
                        Header.Division = staffs.Division;
                        Header.DEPT = staffs.DEPT;
                        Header.SECT = staffs.SECT;
                        Header.JobCode = staffs.JobCode;
                        Header.JobGrade = staffs.JobGrade;
                        Header.Salary = staffs.Salary;
                        Header.StartDate = staffs.StartDate;
                        Header.EffectDate = staffs.StartDate;
                        Header.BankBranch = staffs.BankBranch;
                        Header.BankAccName = staffs.BankAccName;
                        Header.BankName = staffs.BankName;
                        Header.CATE = staffs.CATE;
                        Header.SalaryType = "M";
                        Header.TemLeave ="LB";
                        Header.PayParam  = "DEFAULT";
                        Header.CareerDesc  = "NEWJOIN";
                        if (staffs.BankName == null || staffs.BankName == "")
                            Header.BankName = "CC";
                        Header.BankAcc = staffs.BankAcc;
                        Header.LevelCode = staffs.LevelCode;
                        Header.LeaveConf = staffs.LeaveConf;
                        Header.PayParam = staffs.PayParam;
                        Header.TXPayType = staffs.TXPayType;
                        Header.CareerDesc = staffs.CareerDesc;
                        Header.AllName = Header.FirstName + " " + Header.LastName;
                        Header.OthAllName = Header.OthFirstName + " " + Header.OthLastName;
                        if (FIRST_LASTNAME != null)
                        {
                            if (FIRST_LASTNAME.SettinValue != "TRUE") Header.AllName = Header.LastName + " " + Header.FirstName;
                        }
                        if (FIRST_LASTNAME_KH != null)
                        {
                            if (FIRST_LASTNAME_KH.SettinValue != "TRUE") Header.OthAllName = Header.OthLastName + " " + Header.OthFirstName;
                        }

                        Header.HODCode = staffs.HODCode;
                        Header.FirstLine = staffs.FirstLine;
                        Header.FirstLine2 = staffs.FirstLine2;
                        Header.SecondLine = staffs.SecondLine;
                        Header.SecondLine2 = staffs.SecondLine2;
                        Header.ROSTER = staffs.ROSTER;
                        Header.TeleGroup = staffs.TeleGroup;
                        Header.CATE = staffs.CATE;
                        Header.DateTerminate = new DateTime(1900, 1, 1);
                        Header.IsCalSalary = true;
                        Header.Status = SYDocumentStatus.A.ToString();
                        Header.StaffType = staffs.StaffType;
                        Header.IsResident = true;
                        var IDCard = ListEmpIdentity.Where(w => w.EmpCode == staffs.EmpCode).ToList();
                        if (IDCard.Count > 0)
                        {
                            foreach (var empID in IDCard)
                            {
                                if (empID.ToString().Trim() != "")
                                    if (empID.IDCardNo.Trim() != "")
                                        DB.HREmpIdentities.Add(empID);
                            }
                        }
                        var Pro = DB.HRProbationTypes.FirstOrDefault(w => w.Code == staffs.ProbationType);
                        if (Pro != null)
                        {
                            int Months = 0;
                            int day = 0;
                            if (Pro != null)
                            {
                                Months = Pro.InMonth;
                                day = Pro.Day;
                            }
                            DateTime Probation = staffs.StartDate.Value.AddMonths(Months).AddDays(day);
                            Header.Probation = Probation;
                            Header.ProbationType = staffs.ProbationType;
                        }
                        else
                        {
                            Header.Probation = staffs.StartDate.Value.AddMonths(1).AddDays(1);
                            Header.ProbationType = "PRO01";
                        }
                       

                        HeaderCareer = new HREmpCareer();
                        HeaderCareer.EmpCode = Header.EmpCode;
                        HeaderCareer.CareerCode = Header.CareerDesc;
                        HeaderCareer.CompanyCode = Header.CompanyCode;
                        HeaderCareer.EmpType = Header.EmpType;
                        HeaderCareer.Branch = Header.Branch;
                        HeaderCareer.DEPT = Header.DEPT;
                        HeaderCareer.LOCT = Header.LOCT;
                        HeaderCareer.Division = Header.Division;
                        HeaderCareer.LINE = Header.Line;
                        HeaderCareer.SECT = Header.SECT;
                        HeaderCareer.LevelCode = Header.LevelCode;
                        HeaderCareer.JobGrade = Header.JobGrade;
                        HeaderCareer.JobCode = Header.JobCode;
                        HeaderCareer.JobDesc = Header.POSTDESC;
                        HeaderCareer.JobSpec = Header.JOBSPEC;
                        HeaderCareer.EstartSAL = Header.Salary.ToString();
                        HeaderCareer.EIncrease = Header.Salary.ToString();
                        HeaderCareer.ESalary = Header.ESalary;
                        HeaderCareer.SupCode = Header.SubFunction;
                        HeaderCareer.FromDate = Header.StartDate;
                        HeaderCareer.ToDate = Convert.ToDateTime("01/01/5000");
                        HeaderCareer.EffectDate = Header.EffectDate;
                        HeaderCareer.ProDate = Header.StartDate;
                        HeaderCareer.Reason = "New Join";
                        HeaderCareer.Remark = "Welcome to " + CompanyName;
                        HeaderCareer.Appby = "";
                        HeaderCareer.AppDate = Header.StartDate.Value.ToString("dd-MM-yyyy");
                        HeaderCareer.VeriFyBy = "";
                        HeaderCareer.VeriFYDate = Header.StartDate.Value.ToString("dd-MM-yyyy");
                        HeaderCareer.LCK = 1;
                        HeaderCareer.OldSalary = Header.Salary;
                        HeaderCareer.Increase = 0;
                        HeaderCareer.NewSalary = Header.Salary;
                        HeaderCareer.JobGrade = Header.JobGrade;
                        HeaderCareer.PersGrade = Header.PersGrade;
                        HeaderCareer.HomeFunction = Header.HomeFunction;
                        HeaderCareer.Functions = Header.Functions;
                        HeaderCareer.SubFunction = Header.SubFunction;
                        HeaderCareer.StaffType = Header.StaffType;
                        HeaderCareer.Office = Header.Office;
                        HeaderCareer.CATE = Header.CATE;
                        HeaderCareer.Groups = Header.Groups;
                        HeaderCareer.CreateBy = User.UserName;
                        HeaderCareer.CreateOn = DateTime.Now;

                        DB.HREmpCareers.Add(HeaderCareer);
                        DB.HRStaffProfiles.Add(Header);
                    }

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

        public string uploadSalary()
        {
            try
            {

                if (ListHeader.Count == 0)
                {
                    return "NO_DATA";
                }
                var date = DateTime.Now;
                var ListStaff = DB.HRStaffProfiles.ToList();
                var ListEmpCareer = DB.HREmpCareers.ToList();
                Header = new HRStaffProfile();
                int i = 0;
                foreach (var Emp in ListHeader.ToList())
                {
                    i += 1;
                    Header.EmpCode = Emp.EmpCode;
                    var checkEmp = ListStaff.Where(w => w.EmpCode == Emp.EmpCode).ToList();
                    if (checkEmp.Count != 0)
                    {
                        var Staff = ListStaff.FirstOrDefault(x => x.EmpCode == Emp.EmpCode);
                        var StaffHis = ListEmpCareer.FirstOrDefault(x => x.EmpCode == Emp.EmpCode);

                        if (Staff != null)
                        {
                            Staff.Salary = Emp.Salary;
                            DB.HRStaffProfiles.Attach(Staff);
                            DB.Entry(Staff).Property(x => x.Salary).IsModified = true;
                        }
                        if (StaffHis != null)
                        {

                            StaffHis.OldSalary = Emp.Salary;
                            StaffHis.Increase = 0;
                            StaffHis.NewSalary = Emp.Salary;
                            DB.HREmpCareers.Attach(StaffHis);
                            DB.Entry(StaffHis).Property(x => x.OldSalary).IsModified = true;
                            DB.Entry(StaffHis).Property(x => x.Increase).IsModified = true;
                            DB.Entry(StaffHis).Property(x => x.NewSalary).IsModified = true;
                        }
                    }
                    else
                    {
                        return "dupulicated" + Header.EmpCode;
                    }
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
        #endregion
        #region "Upload Career" 
        public string uploadCareer()
        {
            var DBD = new HumicaDBContext();
            var DBI = new HumicaDBContext();
            DB = new HumicaDBContext();
            try
            {
                DBD.Configuration.AutoDetectChangesEnabled = false;
                DBI.Configuration.AutoDetectChangesEnabled = false;
                DB.Configuration.AutoDetectChangesEnabled = false;
                if (ListCareer.Count == 0)
                {
                    return "NO_DATA";
                }
                var _lstCareerCode = DB.HRCareerHistories.ToList();
                var _lstResType = DB.HRTerminTypes.ToList();
                MessageError = "";
                foreach (var staffs in ListCareer.ToList())
                {
                    Header = new HRStaffProfile();
                    Header = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == staffs.EmpCode.Trim());
                    if (Header == null)
                    {
                        MessageError = "EmpCode:" + staffs.EmpCode;
                        return "INV_DOC";
                    }
                   var EmpCareer = DB.HREmpCareers.Where(w => w.EmpCode == staffs.EmpCode).ToList();
                    if (EmpCareer.Where(w => w.FromDate.Value.Date >= staffs.EffectDate.Value.Date).Any())
                    {
                        MessageError = "EmpCode:" + staffs.EmpCode;
                        return "INV_DATE";
                    }
                    var objMatch = DB.HREmpCareers.Where(w => w.EmpCode == staffs.EmpCode).OrderByDescending(w => w.FromDate).ToList();
                    if (objMatch.ToList().Count() > 0)
                    {
                        var ObjUpdate = objMatch.Where(w => w.EffectDate.Value.Date != staffs.EffectDate.Value.Date).OrderByDescending(w => w.FromDate).First();
                        DateTime ToDate = new DateTime();
                        ToDate = staffs.EffectDate.Value.AddDays(-1);
                        if (objMatch.Where(w => w.EmpCode == staffs.EmpCode).ToList().Count > 0)
                        {
                            var _Careers = DB.HREmpCareers.Where(w => EntityFunctions.TruncateTime(w.EffectDate.Value) != EntityFunctions.TruncateTime(staffs.EffectDate.Value) && w.EmpCode == staffs.EmpCode).OrderByDescending(x => x.FromDate).FirstOrDefault();
                            var _Career = new HREmpCareer();
                            _Career = _Careers;
                            _Career.LCKEDIT = 1;
                            _Career.ToDate = ToDate;
                            if (!string.IsNullOrEmpty(staffs.NewEmpCode))
                            {
                                _Career.NewEmpCode = staffs.EmpCode;
                                var OtherStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == staffs.NewEmpCode);
                                var OtherCareer = DB.HREmpCareers.FirstOrDefault(w => w.NewEmpCode == staffs.NewEmpCode);
                                if (OtherStaff != null || OtherCareer != null)
                                {
                                    MessageError = "Duplicate_EmpCode: " + staffs.NewEmpCode;
                                    return "DUPLICATE_EMP";
                                }
                            }
                            DBI.HREmpCareers.Attach(_Career);
                            DBI.Entry(_Career).Property(w => w.LCKEDIT).IsModified = true;
                            DBI.Entry(_Career).Property(w => w.ToDate).IsModified = true;
                            DBI.Entry(_Career).Property(w => w.NewEmpCode).IsModified = true;
                        }
                        var ListEmpDub = objMatch.Where(w => w.EffectDate.Value.Date == staffs.EffectDate.Value.Date).ToList();
                        foreach (var read in ListEmpDub)
                        {
                            DBD.HREmpCareers.Attach(read);
                            DBD.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                        }
                        if (!_lstCareerCode.Where(w => w.Code == staffs.CareerCode.Trim()).Any())
                        {
                            MessageError = "EmpCode:" + staffs.EmpCode + " CareerCode:" + staffs.CareerCode;
                            return "INV_DOC";
                        }
                        if (staffs.resigntype.Trim() != "")
                        {
                            if (!_lstResType.Where(w => w.Code == staffs.resigntype.Trim()).Any())
                            {
                                MessageError = "EmpCode:" + staffs.EmpCode + " Separate Type:" + staffs.resigntype;
                                return "INV_DOC";
                            }
                        }
                        HeaderCareer = new HREmpCareer();
                        HeaderCareer.EmpCode = staffs.EmpCode;
                        HeaderCareer.CareerCode = staffs.CareerCode;
                        HeaderCareer.CompanyCode = ObjUpdate.CompanyCode;
                        HeaderCareer.EmpType = ObjUpdate.EmpType;
                        
                        if (!string.IsNullOrEmpty(staffs.Branch))
                            HeaderCareer.Branch = staffs.Branch;
                        else HeaderCareer.Branch = ObjUpdate.Branch;
                        if (!string.IsNullOrEmpty(staffs.DEPT))
                            HeaderCareer.DEPT = staffs.DEPT;
                        else HeaderCareer.DEPT = ObjUpdate.DEPT;
                        HeaderCareer.LOCT = ObjUpdate.LOCT;
                        HeaderCareer.Division = ObjUpdate.Division;
                        HeaderCareer.LINE = ObjUpdate.LINE;
                        HeaderCareer.CATE = ObjUpdate.CATE;
                        if (!string.IsNullOrEmpty(staffs.SECT))
                            HeaderCareer.SECT = staffs.SECT;
                        else HeaderCareer.SECT = ObjUpdate.SECT;
                        if (!string.IsNullOrEmpty(staffs.LevelCode))
                            HeaderCareer.LevelCode = staffs.LevelCode;
                        else HeaderCareer.LevelCode = ObjUpdate.LevelCode;
                        if (!string.IsNullOrEmpty(staffs.Office))
                            HeaderCareer.Office = staffs.Office;
                        else HeaderCareer.Office = ObjUpdate.Office;
                        if (!string.IsNullOrEmpty(staffs.Groups))
                            HeaderCareer.Groups = staffs.Groups;
                        else HeaderCareer.Groups = ObjUpdate.Groups;
                        if (!string.IsNullOrEmpty(staffs.JobCode))
                            HeaderCareer.JobCode = staffs.JobCode;
                        else HeaderCareer.JobCode = ObjUpdate.JobCode;
                        HeaderCareer.JobGrade = ObjUpdate.JobGrade;
                        HeaderCareer.JobDesc = ObjUpdate.JobDesc;
                        HeaderCareer.JobSpec = ObjUpdate.JobSpec;
                        HeaderCareer.SupCode = ObjUpdate.SupCode;
                        HeaderCareer.FromDate = staffs.EffectDate;
                        HeaderCareer.ToDate = Convert.ToDateTime("01/01/5000");
                        HeaderCareer.EffectDate = staffs.EffectDate;
                        HeaderCareer.ProDate = staffs.EffectDate;
                        HeaderCareer.LCK = 0;
                        HeaderCareer.OldSalary = ObjUpdate.NewSalary;
                        HeaderCareer.Increase = staffs.Increase;
                        HeaderCareer.NewSalary = ObjUpdate.NewSalary + staffs.Increase;
                        HeaderCareer.JobGrade = ObjUpdate.JobGrade;
                        HeaderCareer.LOCT= staffs.LOCT;
                        HeaderCareer.GroupDept= staffs.GroupDept;
                        HeaderCareer.Division= staffs.Division;
                        HeaderCareer.CATE= staffs.CATE;
                        HeaderCareer.PersGrade = ObjUpdate.PersGrade;
                        HeaderCareer.HomeFunction = ObjUpdate.HomeFunction;
                        HeaderCareer.Functions = ObjUpdate.Functions;
                        HeaderCareer.SubFunction = ObjUpdate.SubFunction;
                        HeaderCareer.StaffType = ObjUpdate.StaffType;
                        HeaderCareer.CreateBy = User.UserName;
                        HeaderCareer.CreateOn = DateTime.Now;
                        HeaderCareer.resigntype = staffs.resigntype;
                        HeaderCareer.Remark = staffs.Remark;
                        ///
                        var _CarCode = _lstCareerCode.FirstOrDefault(w => w.Code == HeaderCareer.CareerCode.Trim());
                        if (_CarCode.NotCalSalary == true)
                        {
                            Header.DateTerminate = HeaderCareer.EffectDate.Value;
                            Header.TerminateStatus = HeaderCareer.CareerCode;
                            Header.TerminateRemark = HeaderCareer.Remark;
                            Header.Status = SYDocumentStatus.I.ToString();
                        }
                        else
                        {
                            Header.DateTerminate = new DateTime(1900, 1, 1);
                            Header.TerminateStatus = "";
                            Header.TerminateRemark = "";
                            Header.Status = SYDocumentStatus.A.ToString();
                        }
                        Header.Salary = HeaderCareer.NewSalary;
                        Header.CareerDesc = HeaderCareer.CareerCode;
                        Header.DEPT = HeaderCareer.DEPT;
                        Header.Office = HeaderCareer.Office;
                        Header.SECT = HeaderCareer.SECT;
                        Header.Groups = HeaderCareer.Groups;
                        Header.JobCode = HeaderCareer.JobCode;
                        Header.LevelCode = HeaderCareer.LevelCode;
                        DBI.HRStaffProfiles.Attach(Header);
                        DBI.Entry(Header).Property(w => w.DateTerminate).IsModified = true;
                        DBI.Entry(Header).Property(w => w.TerminateStatus).IsModified = true;
                        DBI.Entry(Header).Property(w => w.TerminateRemark).IsModified = true;
                        DBI.Entry(Header).Property(w => w.Status).IsModified = true;
                        DBI.Entry(Header).Property(w => w.Salary).IsModified = true;
                        DBI.Entry(Header).Property(w => w.CareerDesc).IsModified = true;
                        DBI.Entry(Header).Property(w => w.DEPT).IsModified = true;
                        DBI.Entry(Header).Property(w => w.Office).IsModified = true;
                        DBI.Entry(Header).Property(w => w.SECT).IsModified = true;
                        DBI.Entry(Header).Property(w => w.Groups).IsModified = true;
                        DBI.Entry(Header).Property(w => w.JobCode).IsModified = true;
                        DBI.Entry(Header).Property(w => w.LevelCode).IsModified = true;

                        DB.HREmpCareers.Add(HeaderCareer);
                    }
                }
                int rowD = DBD.SaveChanges();
                int rowU = DBI.SaveChanges();
                DB.SaveChanges();
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
                DBD.Configuration.AutoDetectChangesEnabled = true;
                DBI.Configuration.AutoDetectChangesEnabled = true;
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        #endregion
        public string DeleteEmp(string EmpCode)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    var ListGen = DB.HISGenSalaryDs.Where(w => w.EmpCode == EmpCode).ToList();
                    if (ListGen.Count > 0)
                    {
                        return "ISGEN_SALARY";
                    }

                    var objMatch = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode);
                    if (objMatch == null)
                    {
                        return "STAFF_NE";
                    }
                    var ListEmpID = DB.HREmpIdentities.Where(w => w.EmpCode == EmpCode).ToList();
                    foreach (var item in ListEmpID)
                    {
                        DB.HREmpIdentities.Attach(item);
                        DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }
                    var objMatchEmp = DB.HREmpCareers.FirstOrDefault(w => w.EmpCode == EmpCode);
                    DB.HRStaffProfiles.Attach(objMatch);
                    DB.Entry(objMatch).State = System.Data.Entity.EntityState.Deleted;

                    DB.HREmpCareers.Attach(objMatchEmp);
                    DB.Entry(objMatchEmp).State = System.Data.Entity.EntityState.Deleted;

                    int row = DB.SaveChanges();
                    return SYConstant.OK;
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserID.ToString();
                    log.ScreenId = EmpCode.ToString();
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
                    log.ScreenId = EmpCode.ToString();
                    log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string DeletePro(int TranNo)
        {
            try
            {
                var objMatch = DB.HRDelayProbations.FirstOrDefault(w => w.TranNo == TranNo);
                if (objMatch == null)
                {
                    return "STAFF_NE";
                }
                DB.HRDelayProbations.Attach(objMatch);
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
                log.ScreenId = TranNo.ToString();
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
                log.ScreenId = TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        #region Province
        public static IEnumerable<OBDesc> District()
        {
            HumicaDBContext DBB = new HumicaDBContext();
            List<OBDesc> _List = new List<OBDesc>();
            if (HttpContext.Current.Session["S_PROVINCE"] != null)
            {
                string ProvinceCode = HttpContext.Current.Session["S_PROVINCE"].ToString();
                _List = DBB.OBDescs.SqlQuery("SELECT distinct District as Code,DistrictDesc1 as Desc1 ,DistrictDesc2 as Desc2 FROM CFPostalCode WHERE Province=" + ProvinceCode).ToList();

            }
            return _List;
        }
        public static IEnumerable<OBDesc> Commune()
        {
            HumicaDBContext DBB = new HumicaDBContext();
            List<OBDesc> _List = new List<OBDesc>();
            if (HttpContext.Current.Session["DISTRICT"] != null)
            {
                string DistrictCode = HttpContext.Current.Session["DISTRICT"].ToString();
                _List = DBB.OBDescs.SqlQuery("SELECT distinct Commune as Code,CommuneDesc1 as Desc1 ,CommuneDesc2 as Desc2 FROM CFPostalCode WHERE District=" + DistrictCode).ToList();

            }
            return _List;
        }
        public static IEnumerable<OBDesc> Village()
        {
            HumicaDBContext DBB = new HumicaDBContext();
            List<OBDesc> _List = new List<OBDesc>();
            if (HttpContext.Current.Session["COMMUNE"] != null)
            {
                string CommuneCode = HttpContext.Current.Session["COMMUNE"].ToString();
                _List = DBB.OBDescs.SqlQuery("SELECT distinct Village as Code,VillageDesc1 as Desc1 ,VillageDesc2 as Desc2 FROM CFPostalCode WHERE Commune=" + CommuneCode).ToList();
            }
            return _List;
        }
        #endregion
        #region JobType
        public static IEnumerable<HRDivision> GetDivision()
        {
            HumicaDBContext DBB = new HumicaDBContext();
            List<HRDivision> _List = new List<HRDivision>();
            var WorkGroup = GetDataCompanyGroup("Division");
            //if (!string.IsNullOrEmpty(WorkGroup))
            //{
            //    if (HttpContext.Current.Session["Division"] != null)
            //    {
            //        string CompanyGroup = HttpContext.Current.Session["Division"].ToString();
            //        if (!string.IsNullOrEmpty(CompanyGroup))
            //        {
            //            var _HRList = (from Group in DBB.HRCompanyTrees
            //                           join JobType in DBB.HRDivisions on Group.CompanyMember equals JobType.Code
            //                           where Group.ParentWorkGroupID == CompanyGroup && JobType.IsActive == true
            //                           select JobType);
            //            _List = _HRList.ToList();
            //        }
            //    }
            //}
            //else
            //{
                _List = DBB.HRDivisions.Where(w => w.IsActive == true).ToList();
            //}
            return _List;
        }
        public static IEnumerable<HRGroupDepartment> GetGroupDepartment()
        {
            HumicaDBContext DBB = new HumicaDBContext();
            List<HRGroupDepartment> _List = new List<HRGroupDepartment>();
            var WorkGroup = GetDataCompanyGroup("GroupDepartment");
            //if (!string.IsNullOrEmpty(WorkGroup))
            //{
            //    if (HttpContext.Current.Session["GroupDepartment"] != null)
            //    {
            //        string CompanyGroup = HttpContext.Current.Session["GroupDepartment"].ToString();
            //        if (!string.IsNullOrEmpty(CompanyGroup))
            //        {
            //            var _HRList = (from Group in DBB.HRCompanyTrees
            //                           join JobType in DBB.HRGroupDepartments on Group.CompanyMember equals JobType.Code
            //                           where Group.ParentWorkGroupID == CompanyGroup
            //                           select JobType);
            //            _List = _HRList.ToList();
            //        }
            //    }
            //}
            //else
            //{
                _List = DBB.HRGroupDepartments.ToList();
           // }
            return _List;
        }
        public static IEnumerable<HRDepartment> GetDepartment()
        {
            HumicaDBContext DBB = new HumicaDBContext();
            List<HRDepartment> _List = new List<HRDepartment>();
            //var WorkGroup = GetDataCompanyGroup("Department");
            //if (!string.IsNullOrEmpty(WorkGroup))
            //{
            //    if (HttpContext.Current.Session["Department"] != null)
            //    {
            //        string CompanyGroup = HttpContext.Current.Session["Department"].ToString();
            //        if (!string.IsNullOrEmpty(CompanyGroup))
            //        {
            //            var _HRList = (from Group in DBB.HRCompanyTrees
            //                           join JobType in DBB.HRDepartments on Group.CompanyMember equals JobType.Code
            //                           where Group.ParentWorkGroupID == CompanyGroup && JobType.IsActive == true
            //                           select JobType);
            //            _List = _HRList.ToList();
            //        }
            //    }
            //}
            //else
            //{
                _List = DBB.HRDepartments.Where(w => w.IsActive == true).ToList();
            //}
            return _List;
        }
        public static IEnumerable<HRPosition> GetPosition()
        {
            HumicaDBContext DBB = new HumicaDBContext();
            List<HRPosition> _List = new List<HRPosition>();
            //var WorkGroup = GetDataCompanyGroup("Position");
            //if (!string.IsNullOrEmpty(WorkGroup))
            //{
            //    if (HttpContext.Current.Session["Position"] != null)
            //    {
            //        string CompanyGroup = HttpContext.Current.Session["Position"].ToString();
            //        if (!string.IsNullOrEmpty(CompanyGroup))
            //        {
            //            var _HRList = (from Group in DBB.HRCompanyTrees
            //                           join JobType in DBB.HRPositions on Group.CompanyMember equals JobType.Code
            //                           where Group.ParentWorkGroupID == CompanyGroup && JobType.IsActive == true
            //                           select JobType);
            //            _List = _HRList.ToList();
            //        }
            //    }
            //}
            //else
            //{
                _List = DBB.HRPositions.Where(w => w.IsActive == true).ToList();
            //
            return _List;
        }
        public static IEnumerable<HRSection> GetSection()
        {
            HumicaDBContext DBB = new HumicaDBContext();
            List<HRSection> _List = new List<HRSection>();
            var WorkGroup = GetDataCompanyGroup("Section");
            //if (!string.IsNullOrEmpty(WorkGroup))
            //{
            //    if (HttpContext.Current.Session["Section"] != null)
            //    {
            //        string CompanyGroup = HttpContext.Current.Session["Section"].ToString();
            //        if (!string.IsNullOrEmpty(CompanyGroup))
            //        {
            //            var _HRList = (from Group in DBB.HRCompanyTrees
            //                           join JobType in DBB.HRSections on Group.CompanyMember equals JobType.Code
            //                           where Group.ParentWorkGroupID == CompanyGroup && JobType.IsActive == true
            //                           select JobType);
            //            _List = _HRList.ToList();
            //        }
            //    }
            //}
            //else
            //{
                _List = DBB.HRSections.Where(w => w.IsActive == true).ToList();
            //
            return _List;
        }
        public static IEnumerable<HRLevel> GetLevel()
        {
            SMSystemEntity SMS = new SMSystemEntity();
            HumicaDBContext DBB = new HumicaDBContext();
            List<HRLevel> _List = new List<HRLevel>();
            var WorkGroup = GetDataCompanyGroup("Level");
            var ListLevel = SYConstant.getLevelDataAccess();
            //if (!string.IsNullOrEmpty(WorkGroup))
            //{
            //    if (HttpContext.Current.Session["Level"] != null)
            //    {
            //        string CompanyGroup = HttpContext.Current.Session["Level"].ToString();
            //        if (!string.IsNullOrEmpty(CompanyGroup))
            //        {
            //            var ListGroup = DBB.HRCompanyTrees.Where(w => w.ParentWorkGroupID == CompanyGroup).ToList();
            //            //var _HRList = (from Group in DBB.HRCompanyTrees
            //            //               join JobType in SMS.HRLevels on Group.CompanyMember equals JobType.Code
            //            //               where Group.ParentWorkGroupID == CompanyGroup
            //            //               select JobType).ToList();
            //            //_List = _HRList.ToList();
            //            var _ListTemp = SMS.HRLevels.ToList();
            //            _List = _ListTemp.Where(w => ListGroup.Where(x => x.CompanyMember == w.Code).Any()).ToList();
            //        }
            //    }
            //}
            //else
            //{
                _List = SMS.HRLevels.ToList();
           // }
            return _List.Where(w => ListLevel.Where(x => x.Code == w.Code).Any()).ToList();
        }

        public static IEnumerable<HRJobGrade> GetJobGrade()
        {
            HumicaDBContext DBB = new HumicaDBContext();
            List<HRJobGrade> _List = new List<HRJobGrade>();
            var WorkGroup = GetDataCompanyGroup("JobGrade");
            //if (!string.IsNullOrEmpty(WorkGroup))
            //{
            //    if (HttpContext.Current.Session["JobGrade"] != null)
            //    {
            //        string CompanyGroup = HttpContext.Current.Session["JobGrade"].ToString();
            //        if (!string.IsNullOrEmpty(CompanyGroup))
            //        {
            //            var _HRList = (from Group in DBB.HRCompanyTrees
            //                           join JobType in DBB.HRJobGrades on Group.CompanyMember equals JobType.Code
            //                           where Group.ParentWorkGroupID == CompanyGroup
            //                           select JobType);
            //            _List = _HRList.ToList();
            //        }
            //    }
            //}
            //else
            //{
                _List = DBB.HRJobGrades.ToList();
           // }
            return _List;
        }
        public static string GetDataCompanyGroup(string WorkGroup)
        {
            string Result = "";
            HumicaDBContext DBB = new HumicaDBContext();
            var _listCom = DBB.HRCompanyGroups.Where(w => w.WorkGroup == WorkGroup).ToList();
            if (_listCom.Count() > 0)
            {
                var obj = _listCom.FirstOrDefault();
                Result = obj.WorkGroup;
            }
            return Result;
        }
        #endregion

        public string isValidEffectiveDate(List<HREmpCareer> _lstCareer, HREmpCareer Career )
        {
            if (_lstCareer.Where(w => w.FromDate.Value.Date >= Career.EffectDate.Value.Date).Any())
            {
                return "INV_DATE";
            }

            return SYSConstant.OK;
        }
        public async Task<string> TransferInscreaseSalary(DateTime EffectiveDate, List<HRAPPIncSalaryItem> ListEmpInscrease)
        {
            DB = new HumicaDBContext();
            string ID = "";
            try
            {
                var _ListEmp = await DB.HREmpCareers.ToListAsync();
                var _lstCareerCode = await DB.HRCareerHistories.ToListAsync();
                var _lstResType = await DB.HRTerminTypes.ToListAsync();
                var _lstStaff = await DB.HRStaffProfiles.ToListAsync();
                HREmpCareer Career = new HREmpCareer();
                foreach (var Emp in ListEmpInscrease)
                {
                    var _lstCareer = _ListEmp.Where(w => w.EmpCode == Emp.EmpCode).ToList();
                    if (_lstCareer.Count > 0)
                    {
                        var ObjOld=new HREmpCareer();
                        ObjOld= _lstCareer.OrderByDescending(w=>w.EffectDate).FirstOrDefault();
                        Career = new HREmpCareer();
                        Career.EffectDate = EffectiveDate;
                        Career.EmpCode=Emp.EmpCode;
                        Career.OldSalary = ObjOld.NewSalary;
                        Career.Increase = Emp.Increase;
                        Career.NewSalary = ObjOld.NewSalary + Emp.Increase;
                       var mss= isValidEffectiveDate(_lstCareer, Career);
                        if (mss != SYConstant.OK)
                        {
                            return mss;
                        }
                        SwapCareer(Career, ObjOld);
                        Career.CareerCode = "INCREASE";
                        Career.ProDate = Career.EffectDate;
                        Career.FromDate = Career.EffectDate;
                        Career.ToDate = new DateTime(5000, 1, 1);
                        Career.CreateBy = User.UserName;
                        Career.CreateOn = DateTime.Now;
                        DB.HREmpCareers.Add(Career);
                        //Update Old Career
                        var _Career = new HREmpCareer();
                        _Career = ObjOld;
                        _Career.ToDate = EffectiveDate.AddDays(-1);
                        DB.HREmpCareers.Attach(_Career);
                        DB.Entry(_Career).Property(w => w.ToDate).IsModified = true;
                        //Update salary in staff
                        var Staff = new HRStaffProfile();
                        Staff = _lstStaff.FirstOrDefault(w => w.EmpCode == Emp.EmpCode);
                        if(Staff!=null)
                        {
                            Staff.Salary = Career.NewSalary;
                            DB.HRStaffProfiles.Attach(Staff);
                            DB.Entry(Staff).Property(w => w.Salary).IsModified = true;
                        }
                    }
                }

                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return e.Message;
            }
            catch (DbUpdateException e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return e.Message;
            }
            catch (Exception e)
            {
                await ClsEventLog.SaveEventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
                return e.Message;
            }
        }

        public void SwapStaff(HRStaffProfile S, HREmpCareer D)
        {
            S.CompanyCode = D.CompanyCode;
            S.GroupDept = D.GroupDept;
            S.Branch =  D.Branch;
            S.LOCT = D.LOCT;
            S.Division = D.Division;
            S.DEPT = D.DEPT;
            S.Line = D.LINE;
            S.CATE = D.CATE;
            S.SECT = D.SECT;
            S.Functions = D.Functions;
            S.LevelCode = D.LevelCode;
            S.JobCode = D.JobCode;
            S.JobGrade = D.JobGrade;
            S.JOBSPEC = D.JobSpec;
            S.ESalary = "";
            S.EffectDate = D.EffectDate;
            S.EmpType = D.EmpType;
            S.CareerDesc = D.CareerCode;
            S.Salary = D.NewSalary;
            S.StaffType = D.StaffType;
            S.ChangedBy = User.UserName;
            S.ChangedOn = DateTime.Now;
            S.Functions = D.Functions;
            S.Office = D.Office;
            S.Groups = D.Groups;
        }
        public void SwapCareer(HREmpCareer S,HREmpCareer D)
        {
            S.CompanyCode = D.CompanyCode;
            S.Branch = D.Branch;
            S.Division = D.Division;
            S.GroupDept = D.GroupDept;
            S.DEPT = D.DEPT;
            S.Office = D.Office;
            S.SECT = D.SECT;
            S.Groups = D.Groups;
            S.JobCode = D.JobCode;
            S.LevelCode = D.LevelCode;
            S.JobGrade = D.JobGrade;
            S.CATE = D.CATE;
            S.LINE = D.LINE;
            S.EmpType = D.EmpType;
            S.LOCT = D.LOCT;
            S.StaffType = D.StaffType;

        }
    }
    public class HR_Career
    {
        public string EmpCode { get; set; }
        public string NewEmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpNameKH { get; set; }
        public string Gender { get; set; }
        public string Branch { get; set; }
        public string Divison { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Level { get; set; }
        public string JobGrad { get; set; }
        public string OldSalary { get; set; }
        public string Increase { get; set; }
        public string NewSalary { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ChangedOn { get; set; }
        public string ChangedBy { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Career { get; set; }
        public DateTime StartDate { get; set; }

        public string AttachFile { get; set; }
    }
    public class HREmpBanKList
    {
        public string EmpCode { get; set; }
        public int LineItem { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BranchName { get; set; }
        public string Salary { get; set; }
        public Nullable<bool> IsTax { get; set; }
    }
    public class ClsStaff
    {
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeName2 { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
        public string BranchCode { get; set; }
        public string Dept { get; set; }
    }
}