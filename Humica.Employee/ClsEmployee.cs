using Humica.Core;
using Humica.Core.CF;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.Hosting;
using System.Xml.Schema;

namespace Humica.Employee
{
    public class ClsEmployee : IClsEmployee
    {
        protected IUnitOfWork unitOfWork;
        public string ScreenId { get; set; }
        public string NewSalary { get; set; }
        public string OldSalary { get; set; }
        public string Increase { get; set; }
        public string MessageError { get; set; }
        public string Salary { get; set; }
        public bool IsEmpAuto { get; set; }
        public string SalaryTax { get; set; }
        public string SalaryNSSF { get; set; }
        public string IsSalary { get; set; }
        public bool IsTax { get; set; }
        public bool Fist_Last { get; set; }
        public bool Fist_Last_KH { get; set; }
        public string CareerMoveID { get; set; }
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public HRStaffProfile Header { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public HREmpCareer HeaderCareer { get; set; }
        public List<HR_STAFF_VIEW> ListStaffView { get; set; }
        public List<HRStaffProfile> ListStaffProfile { get; set; }
        public List<HREmpIdentity> ListEmpIdentity { get; set; }
        public List<HREmpIdentity> ListEmpIdentityUP { get; set; }
        public List<HREmpJobDescription> ListEmpJobDescription { get; set; }
        public List<HREmpFamily> ListEmpFamily { get; set; }
        public List<HREmpEduc> ListEducation { get; set; }
        public List<HREmpContract> ListContract { get; set; }
        public List<HRDelayProbation> ListDelayProbation { get; set; }
        public List<HREmpDisciplinary> ListEmpDisciplinary { get; set; }
        public List<HREmpCertificate> ListEmpCertificate { get; set; }
        public List<HREmpPhischk> ListEmpMedical { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public List<HR_Career> ListCareerHis { get; set; }
        public List<HREmpCareer> ListCareer { get; set; }
        public List<HR_EmpCareer_View> ListEmpCareer { get; set; }
        public List<HRStaffProfile> ListEmp { get; set; }
        public List<HREmpBankAcc> ListSaparate { get; set; }
        public List<ClassCareerBankList> ListSalrySaparate { get; set; }
        public List<HREmpCareerBankList> ListCareerBankList { get; set; }
        public List<HR_HisBanKList> ListHisBanKList { get; set; }
        public bool IsInAtive { get; set; }
        public string ToDate { get; set; }
        public string NewEmpCode { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsEmployee()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void OnLoandindEmployee()
        {
            if (Filter == null)
            {
                Filter = new FTFilterEmployee();
                Filter.Status = SYDocumentStatus.A.ToString();
            }
            DateTime CurrentDate = DateTime.Now;
            ListStaffView = new List<HR_STAFF_VIEW>();

            List<HRCompany> _ListCompany = SYConstant.getCompanyDataAccess();
            List<HRBranch> _ListBranch = SYConstant.getBranchDataAccess();
            List<HRLevel> _ListLevel = SYConstant.getLevelDataAccess();
            var branchid = _ListBranch.Select(s => s.Code).ToList();
            var companyCodes = _ListCompany.Select(x => x.Company).ToList();
            var levelCodes = _ListLevel.Select(x => x.Code).ToList();
            var businessUnits = string.IsNullOrEmpty(Filter.BusinessUnit)
                        ? new string[0] : Filter.BusinessUnit.Split(',').Select(bu => bu.Trim()).ToArray();
            var ListStaff_View = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable()
                           .Where(w => companyCodes.Contains(w.CompanyCode)
                           && levelCodes.Contains(w.LevelCode)
                           && branchid.Contains(w.BranchID)
                           && (string.IsNullOrEmpty(Filter.Office) || w.Office == Filter.Office)
                           && (string.IsNullOrEmpty(Filter.Department) || w.DEPT == Filter.Department)
                           && (string.IsNullOrEmpty(Filter.Team) || w.Groups == Filter.Team)
                           );
            if (businessUnits.Any())
            {
                ListStaff_View = ListStaff_View.Where(w => businessUnits.Contains(w.GroupDept));
            }
            if (Filter.StartDate != null)
            {
                if (Filter.Status == "A")
                    ListStaff_View = ListStaff_View.Where(w => (w.StatusCode == Filter.Status
                                    && w.StartDate.Value.Month == Filter.StartDate.Value.Month
                                    && w.StartDate.Value.Year == Filter.StartDate.Value.Year)
                                    || (w.StatusCode == "I" && w.DateTerminate > CurrentDate));
                else if (Filter.Status == "I")
                    ListStaff_View = ListStaff_View.Where(w => w.StatusCode == Filter.Status
                                && w.DateTerminate.Value.AddDays(-1).Month == Filter.StartDate.Value.Month
                                && w.DateTerminate.Value.AddDays(-1).Year == Filter.StartDate.Value.Year
                                && w.DateTerminate <= CurrentDate);
                if (Filter.Status == "I/A")
                {
                    ListStaff_View = ListStaff_View.Where(w => (w.StatusCode == "A" || (w.StatusCode == "I" && w.DateTerminate > CurrentDate)) || (w.DateTerminate.Value.AddDays(-1).Month == Filter.StartDate.Value.Month
                                                   && w.DateTerminate.Value.AddDays(-1).Year == Filter.StartDate.Value.Year));
                }
            }
            else
            {
                if (Filter.Status == "A")
                    ListStaff_View = ListStaff_View.Where(w => w.StatusCode == Filter.Status || (w.StatusCode == "I" && w.DateTerminate > CurrentDate));
                if (Filter.Status == "I")
                    ListStaff_View = ListStaff_View.Where(w => w.StatusCode == Filter.Status && w.DateTerminate <= CurrentDate);
            }
            if (!string.IsNullOrEmpty(Filter.Branch))
            {
                var branchFilter = Filter.Branch.Split(',').Select(b => b.Trim()).Where(b => !string.IsNullOrEmpty(b)).ToList();
                ListStaff_View = ListStaff_View.Where(w => branchFilter.Contains(w.BranchID));
            }
            ListStaffView = ListStaff_View.OrderBy(w => w.EmpCode).ToList();
        }
        public void OnCreatingLoading(params object[] keys)
        {
            DateTime DateNow = DateTime.Now;
            Header = new HRStaffProfile();
            ListEmpIdentity = new List<HREmpIdentity>();
            ListEmpJobDescription = new List<HREmpJobDescription>();
            ListEmpFamily = new List<HREmpFamily>();
            ListEducation = new List<HREmpEduc>();
            ListContract = new List<HREmpContract>();
            ListDelayProbation = new List<HRDelayProbation>();
            ListEmpDisciplinary = new List<HREmpDisciplinary>();
            ListEmpCertificate = new List<HREmpCertificate>();
            ListEmpMedical = new List<HREmpPhischk>();
            ListSaparate = new List<HREmpBankAcc>();
            Header.StartDate = DateNow;
            Header.Probation = DateNow;
            Header.DOB = DateNow;
            Header.LeaveConf = DateNow;
            Header.CareerDesc = "NEWJOIN";
            Header.BankName = "CC";
            Header.IsCalSalary = true;
            Header.IsResident = true;
            Header.IsOTApproval = false;
            Header.EmpType = "LOCAL";
            Header.IsHold = false;
            Header.Nation = "CAM";
            Header.IsAtten = false;
            Header.Sex = "M";
            Header.StaffType = "OS";
            Header.IsPayPartial = true;
            Header.TemLeave = "LB";
            Header.SalaryType = SYSalaryType.Monthly.ToString();
            Header.IsExceptScan = false;
            Header.ReSalary = DateTime.Now;

            Fist_Last = false;
            Fist_Last_KH = false;
            var FIRST_LASTNAME = SYSettings.getSetting("FIRST_LASTNAME");
            var FIRST_LASTNAME_KH = SYSettings.getSetting("FIRST_LASTNAME_KH");
            if (FIRST_LASTNAME != null)
            {
                if (FIRST_LASTNAME.SettinValue == "TRUE") Fist_Last = true;
            }
            if (FIRST_LASTNAME_KH != null)
            {
                if (FIRST_LASTNAME_KH.SettinValue == "TRUE") Fist_Last_KH = true;
            }
            var Is_EmpAuto = SYSettings.getSetting("EMPCODE").SettinValue;
            if (Is_EmpAuto == "YES") IsEmpAuto = true;
            if (User.IsSalary == true)
            {
                Salary = "0";
            }
            else
            {
                Salary = "#####";
            }
            var TemLeave = unitOfWork.Set<HRSetEntitleH>().FirstOrDefault();
            if (TemLeave != null)
            {
                Header.TemLeave = TemLeave.Code;
            }

        }
        public string CreatStaff()
        {
            OnLoad();
            try
            {
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
                    var DocConf = unitOfWork.Set<HREmpCode>().ToList();
                    if (!string.IsNullOrEmpty(Header.CompanyCode))
                        DocConf = DocConf.Where(w => w.Company == Header.CompanyCode || string.IsNullOrEmpty(w.Company)).ToList();
                    if (!string.IsNullOrEmpty(Header.Branch))
                        DocConf = DocConf.Where(w => w.Branch == Header.Branch || string.IsNullOrEmpty(w.Branch)).ToList();
                    if (!string.IsNullOrEmpty(Header.GroupDept))
                        DocConf = DocConf.Where(w => w.BU == Header.GroupDept || string.IsNullOrEmpty(w.BU)).ToList();
                    if (!DocConf.Any())
                    {
                        return "NUMBER_RANK_NE";
                    }
                    var objNumber = new CFNumberRank(DocConf.FirstOrDefault().NumberRank, Header.CompanyCode, Header.StartDate.Value.Year, true);
                    if (objNumber.NextNumberRank == null)
                    {
                        return "NUMBER_RANK_NE";
                    }
                    Header.EmpCode = objNumber.NextNumberRank.Trim();
                }
                Header.EmpCode = Header.EmpCode.Trim().ToUpper();
                var EmpD = unitOfWork.Set<HRStaffProfile>().Where(w => w.EmpCode == Header.EmpCode).ToList();
                if (EmpD.Count() > 0)
                {
                    return "DUPL_KEY";
                }
                bool IsSalary = IsHideSalary(Header.LevelCode);
                if (IsSalary == true) Header.Salary = Convert.ToDecimal(Salary);
                else Header.Salary = 0;
                if (Header.FirstLine == Header.FirstLine2 && !string.IsNullOrEmpty(Header.FirstLine))
                {
                    return "FIRSTLINE_EN";
                }
                if (Header.SecondLine == Header.SecondLine2 && !string.IsNullOrEmpty(Header.SecondLine))
                {
                    return "SECONDLINE_EN";
                }
                if (Header.LeaveConf == null) Header.LeaveConf = Header.StartDate;
                Header.EffectDate = Header.StartDate;
                Header.Attachment = Header.Attachment;
                Header.GPSData = Header.GPSData;
                Header.AttachFile = Header.AttachFile;
                Header.CreatedOn = DateTime.Now.Date;
                Header.CreatedBy = User.UserName;
                Header.DateTerminate = new DateTime(1900, 1, 1);
                Header.Status = SYDocumentStatus.A.ToString();
                ClsEmpValidate empValidate = new ClsEmpValidate();
                Header.JobCode = empValidate.GetPosition(Header.JobCode);
                if (string.IsNullOrEmpty(Header.Currency)) Header.Currency = "USD";
                HeaderCareer = new HREmpCareer();
                ClsSwapData SwapData = new ClsSwapData();

                SwapData.SwapStaffCarrer(HeaderCareer, Header, User);
                ClsCopyFile clsCopyFile = new ClsCopyFile();
                int LinItem = 0;
                string Attachment = "";
                var listInden = unitOfWork.Set<HRIdentityType>().ToList();
                foreach (var item in ListEmpIdentity)
                {
                    Attachment = item.Attachment;
                    LinItem += 1;
                    var empIden = new HREmpIdentity();
                    SwapData.SwapEmpIdentity(empIden, item, Header);
                    var DescInd = listInden.FirstOrDefault(w => w.Code == item.IdentityTye);
                    if (DescInd != null)
                    {
                        string DocumentType = DescInd.Description;
                        var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, DocumentType, Header.EmpCode, Header.DEPT, Attachment);
                        if (strpath == "No_Path")
                        {
                            return "File_Exit";
                        }
                        else
                        {
                            empIden.Attachment = strpath;
                        }
                    }
                    empIden.LineItem = LinItem;
                    unitOfWork.Add(empIden);
                }
                #region Save Emp Family
                LinItem = 0;
                Attachment = "";
                foreach (var item in ListEmpFamily)
                {
                    Attachment = item.AttachFile;
                    LinItem += 1;
                    var emp = new HREmpFamily();
                    SwapData.SwapEmpFamily(emp, item, Header);
                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, "Family", Header.EmpCode, Header.DEPT, Attachment);
                        if (strpath == "No_Path")
                        {
                            return "File_Exit";
                        }
                        emp.AttachFile = strpath;
                    }
                    emp.LineItem = LinItem;
                    emp.CreateBy = item.CreateBy;
                    emp.CreateOn = DateTime.Now;
                    unitOfWork.Add(emp);
                }
                #endregion

                #region Save Emp Education
                Attachment = "";
                foreach (var item in ListEducation)
                {
                    Attachment = item.AttachFile;
                    var emp = new HREmpEduc();
                    SwapData.SwapEmpEducation(emp, item, Header);
                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, "Education", Header.EmpCode, Header.DEPT, Attachment);
                        if (strpath == "No_Path")
                        {
                            return "File_Exit";
                        }
                        else
                        {
                            emp.AttachFile = strpath;
                        }
                    }
                   // emp.AttachFile = item.AttachFile;
                    emp.CreatedBy = item.CreatedBy;
                    emp.CreatedOn = DateTime.Now;
                    unitOfWork.Add(emp);
                }
                #endregion

                #region Save Emp Contract
                Attachment = "";
                foreach (var item in ListContract)
                {
                    Attachment = item.ContractPath;
                    var emp = new HREmpContract();
                    SwapData.SwapEmpContract(emp, item, Header);
                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, "Contract", Header.EmpCode, Header.DEPT, Attachment);
                        if (strpath == "No_Path")
                        {
                            return "File_Exit";
                        }
                        else
                        {
                            emp.ContractPath = strpath;
                        }
                    }
                    emp.CreateBy = item.CreateBy;
                    emp.CreateOn = DateTime.Now;
                    unitOfWork.Add(emp);
                }
                #endregion

                #region Save Emp Disciplinary
                Attachment = "";
                foreach (var item in ListEmpDisciplinary)
                {
                    Attachment = item.AttachPath;
                    var emp = new HREmpDisciplinary();
                    SwapData.SwapEmpDisciplinary(emp, item, Header);
                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, "Disciplinary", Header.EmpCode, Header.DEPT, Attachment);
                        if (strpath == "No_Path")
                        {
                            return "File_Exit";
                        }
                        else
                        {
                            emp.AttachPath = strpath;
                        }
                    }
                    emp.CreatedBy = item.CreatedBy;
                    emp.CreatedOn = DateTime.Now;
                    unitOfWork.Add(emp);
                }
                #endregion

                #region Save Emp Certificate
                Attachment = "";
                foreach (var item in ListEmpCertificate)
                {
                    Attachment = item.AttachFile;
                    var emp = new HREmpCertificate();
                    SwapData.SwapEmpCertificate(emp, item, Header);
                    var getDes = unitOfWork.Set<HRCertificationType>().FirstOrDefault(w => w.Code == item.CertType);
                    if (getDes != null)
                    {
                        emp.CertDesc = getDes.Description;
                    }
                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, "Certificate", Header.EmpCode, Header.DEPT, Attachment);
                        if (strpath == "No_Path")
                        {
                            return "File_Exit";
                        }
                        else
                        {
                            emp.AttachFile = strpath;
                        }
                    }
                    emp.CreatedBy = item.CreatedBy;
                    emp.CreatedOn = DateTime.Now;
                    unitOfWork.Add(emp);
                }
                #endregion

                #region Save Emp Medical
                foreach (var item in ListEmpMedical)
                {
                    var emp = new HREmpPhischk();
                    SwapData.SwapEmpMedical(emp, item, Header);
                    emp.Createdby = item.Createdby;
                    emp.CreatedOn = DateTime.Now;
                    unitOfWork.Add(emp);
                }
                #endregion

                LinItem = 0;
                foreach (var item in ListEmpJobDescription)
                {
                    Attachment = item.Attachment;
                    LinItem += 1;
                    var empJD = new HREmpJobDescription();
                    SwapData.SwapEmpJD(empJD, item, Header);
                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, "Job Description", Header.EmpCode, Header.DEPT, Attachment);
                        if (strpath == "No_Path")
                        {
                            return "File_Exit";
                        }
                        else
                        {
                            empJD.Attachment = strpath;
                        }
                    }
                    empJD.LineItem = LinItem;
                    unitOfWork.Add(empJD);
                }
                //Image
                string checkPhoto = CopyPathPhoto(Header, "Photo", Header.Images);
                if (checkPhoto == "No_Path")
                {
                    return "File_Exit";
                }
                else
                {
                    Header.Images = checkPhoto;
                }
                //Signature
                string checkSign = CopyPathPhoto(Header, "Signature", Header.Signature);
                if (checkSign == "No_Path")
                {
                    return "File_Exit";
                }
                else
                {
                    Header.Signature = checkSign;
                }
				//QRCode
				string checkQr = CopyPathPhoto(Header, "QRCode", Header.ImgQr);
				if (checkQr == "No_Path")
				{
					return "File_Exit";
				}
				else
				{
					Header.ImgQr = checkQr;
				}
				//Update with Reporting
				GetPosition(Header, Header.JobCode);
                unitOfWork.Add(HeaderCareer);
                unitOfWork.Add(Header);

                unitOfWork.Save();
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
        }
        public virtual void OnDetailLoading(params object[] keys)
        {
            string Empcode = (string)keys[0];
            ListEmpIdentity = new List<HREmpIdentity>();
            ListEmpDisciplinary = new List<HREmpDisciplinary>();
            ListEmpFamily = new List<HREmpFamily>();
            ListEducation = new List<HREmpEduc>();
            ListContract = new List<HREmpContract>();
            ListDelayProbation = new List<HRDelayProbation>();
            ListEmpCertificate = new List<HREmpCertificate>();
            ListEmpMedical = new List<HREmpPhischk>();
            ListSaparate = new List<HREmpBankAcc>();
            Header = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(x => x.EmpCode == Empcode);
            Fist_Last = false;
            Fist_Last_KH = false;
            var FIRST_LASTNAME = SYSettings.getSetting("FIRST_LASTNAME");
            var FIRST_LASTNAME_KH = SYSettings.getSetting("FIRST_LASTNAME_KH");
            if (FIRST_LASTNAME != null)
            {
                if (FIRST_LASTNAME.SettinValue == "TRUE") Fist_Last = true;
            }
            if (FIRST_LASTNAME_KH != null)
            {
                if (FIRST_LASTNAME_KH.SettinValue == "TRUE") Fist_Last_KH = true;
            }
            if (Header != null)
            {
                if (Header.SalaryFlag != true)
                {
                    Header.ReSalary = DateTime.Now;
                }
                ListEmpIdentity = unitOfWork.Repository<HREmpIdentity>().Queryable().Where(w => w.EmpCode == Empcode).ToList();
                ListEmpJobDescription = unitOfWork.Repository<HREmpJobDescription>().Queryable().Where(w => w.EmpCode == Empcode).ToList();
                ListEmpFamily = unitOfWork.Repository<HREmpFamily>().Queryable().Where(w => w.EmpCode == Empcode).ToList();
                ListEducation = unitOfWork.Repository<HREmpEduc>().Queryable().Where(w => w.EmpCode == Empcode).ToList();
                ListContract = unitOfWork.Repository<HREmpContract>().Queryable().Where(w => w.EmpCode == Empcode).ToList();
                ListDelayProbation = unitOfWork.Repository<HRDelayProbation>().Queryable().Where(w => w.EmpCode == Empcode).ToList();
                ListEmpDisciplinary = unitOfWork.Repository<HREmpDisciplinary>().Queryable().Where(w => w.EmpCode == Empcode).ToList();
                ListEmpCertificate = unitOfWork.Repository<HREmpCertificate>().Queryable().Where(w => w.EmpCode == Empcode).ToList();
                ListEmpMedical = unitOfWork.Repository<HREmpPhischk>().Queryable().Where(w => w.EmpCode == Empcode).ToList();
                ListSaparate = unitOfWork.Repository<HREmpBankAcc>().Queryable().Where(w => w.EmpCode == Empcode && w.IsActive == true).ToList();

                var IsSalary = IsHideSalary(Header.LevelCode);
                if (IsSalary != true)
                {
                    Salary = "#####";
                }
                else
                {
                    Salary = Header.Salary.ToString();
                }
            }
        }
        public string UpdateStaff(string ID)
        {
            OnLoad();
            try
            {
                HRStaffProfile objMast = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == ID);
                if (objMast == null)
                {
                    return "STAFF_NE";
                }
                if (Header.SalaryType == null)
                {
                    return "SALARY_TYPE_EN";
                }
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
                //objMast.Images = Header.Images;
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
                objMast.IsExceptScan = Header.IsExceptScan;
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
                if (Header.LeaveConf == null)
                    objMast.LeaveConf = Header.StartDate;
                objMast.HODCode = Header.HODCode;
                //objMast.NSSFContributionType = Header.NSSFContributionType;
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
                objMast.IsOnsite = Header.IsOnsite;
                objMast.APPTracking = Header.APPTracking;
                objMast.APPEvaluator = Header.APPEvaluator;
                objMast.APPAppraisal = Header.APPAppraisal;
                objMast.APPAppraisal2 = Header.APPAppraisal2;

                #region Save Emp HREmpIdentity

                var LisIdentityDelete = unitOfWork.Set<HREmpIdentity>().Where(w => w.EmpCode == Header.EmpCode).ToList();
                var ListEmpIdentityOld = LisIdentityDelete.Select(x => new HREmpIdentity
                {
                    IdentityTye = x.IdentityTye,
                    Attachment = x.Attachment
                }).ToList();
                var removedIdentity = ListEmpIdentityOld.Where(oldItem => !ListEmpIdentity.Any(newItem => newItem.IdentityTye == oldItem.IdentityTye)).ToList();
                foreach (var removed in removedIdentity)
                {
                    if (!string.IsNullOrEmpty(removed.Attachment))
                    {
                        var files = removed.Attachment.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var file in files)
                        {
                            DeleteFile(file);
                        }
                    }
                }
                foreach (var item in LisIdentityDelete)
                {
                    unitOfWork.Delete(item);
                }
                int LinItem = 0;
                string Attachment = "";
                ClsSwapData SwapData = new ClsSwapData();

                ClsCopyFile clsCopyFile = new ClsCopyFile();
                var listInden = unitOfWork.Set<HRIdentityType>().ToList();
                foreach (var item in ListEmpIdentity)
                {
                    Attachment = item.Attachment;
                    LinItem += 1;
                    var empIden = new HREmpIdentity();
                    SwapData.SwapEmpIdentity(empIden, item, Header);
                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        var objIdentMo = LisIdentityDelete.Where(w => w.IdentityTye == item.IdentityTye).ToList();
                        if (!objIdentMo.Where(w => w.Attachment == item.Attachment).Any())
                        {
                            foreach (var existingFile in objIdentMo)
                            {
                                if (!string.IsNullOrEmpty(existingFile.Attachment))
                                {
                                    var str1 = existingFile.Attachment.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    var str2 = Attachment.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                    var filesToDelete = str1.Except(str2).ToList();
                                    foreach (var fileName in filesToDelete)
                                    {
                                        DeleteFile(fileName);
                                    }
                                }
                            }
                            var DescInd = listInden.FirstOrDefault(w => w.Code == item.IdentityTye);
                            if (DescInd != null)
                            {
                                string DocumentType = DescInd.Description;
                                var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, DocumentType, Header.EmpCode, Header.DEPT, Attachment);
                                if (strpath == "No_Path")
                                {
                                    return "File_Exit";
                                }
                                else
                                {
                                    empIden.Attachment = strpath;
                                }
                            }
                        }
                        else
                        {
                            empIden.Attachment = item.Attachment;
                        }
                    }
                    else
                    {
                        var objIdentMo = LisIdentityDelete.Where(w => w.IdentityTye == item.IdentityTye).ToList();
                        foreach (var existingFile in objIdentMo)
                        {
                            DeleteFile(existingFile.Attachment);
                        }
                    }
                    empIden.LineItem = LinItem;
                    unitOfWork.Add(empIden);
                }
                #endregion

                #region Save Emp JobDescription
                var LisJobDelete = unitOfWork.Set<HREmpJobDescription>().Where(w => w.EmpCode == Header.EmpCode).ToList();
                foreach (var item in LisJobDelete)
                {
                    unitOfWork.Delete(item);
                }
                LinItem = 0;
                foreach (var item in ListEmpJobDescription)
                {
                    Attachment = item.Attachment;
                    LinItem += 1;
                    var empJD = new HREmpJobDescription();
                    SwapData.SwapEmpJD(empJD, item, Header);
                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, "Job Description", Header.EmpCode, Header.DEPT, Attachment);
                        if (strpath == "No_Path")
                        {
                            return "File_Exit";
                        }
                        else
                        {
                            empJD.Attachment = strpath;
                        }
                    }
                    empJD.LineItem = LinItem;
                    unitOfWork.Add(empJD);
                }
                if (objMast.Images != Header.Images)
                {
                    string checkFile = CopyPathPhoto(Header, "Photo", Header.Images);
                    if (checkFile == "No_Path")
                    {
                        return "File_Exit";
                    }
                    else
                    {
                        objMast.Images = checkFile;
                    }
                }
				if (objMast.ImgQr != Header.ImgQr)
				{
					string checkFile = CopyPathPhoto(Header, "QRCode", Header.ImgQr);
					if (checkFile == "No_Path")
					{
						return "File_Exit";
					}
					else
					{
						objMast.ImgQr = checkFile;
					}
				}
				#endregion

				unitOfWork.Update(objMast);

                #region Save Emp Family

                var objEmpFamily = unitOfWork.Set<HREmpFamily>().Where(w => w.EmpCode == Header.EmpCode).ToList();
                var ListEmpFamilyOld = objEmpFamily.Select(x => new HREmpFamily
                {
                    RelName = x.RelName,
                    AttachFile = x.AttachFile
                }).ToList();
                var removedFamily = ListEmpFamilyOld.Where(oldItem => !ListEmpFamily.Any(newItem => newItem.RelName == oldItem.RelName)).ToList();
                foreach (var removed in removedFamily)
                {
                    if (!string.IsNullOrEmpty(removed.AttachFile))
                    {
                        var files = removed.AttachFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var file in files)
                        {
                            DeleteFile(file);
                        }
                    }
                }
                foreach (var item in objEmpFamily)
                {
                    unitOfWork.Delete(item);
                }
                LinItem = 0;
                foreach (var item in ListEmpFamily)
                {
                    Attachment = item.AttachFile;
                    LinItem += 1;
                    var emp = new HREmpFamily();
                    SwapData.SwapEmpFamily(emp, item, Header);
                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        var objIdentMo = objEmpFamily.Where(w => w.RelName == item.RelName).ToList();
                        if (!objIdentMo.Where(w => w.AttachFile == item.AttachFile).Any())
                        {
                            foreach (var existingFile in objIdentMo)
                            {
                                if (!string.IsNullOrEmpty(existingFile.AttachFile))
                                {
                                    var str1 = existingFile.AttachFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    var str2 = Attachment.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                    var filesToDelete = str1.Except(str2).ToList();
                                    foreach (var fileName in filesToDelete)
                                    {
                                        DeleteFile(fileName);
                                    }
                                }
                            }
                            var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, "Family", Header.EmpCode, Header.DEPT, Attachment);
                            if (strpath == "No_Path")
                            {
                                return "File_Exit";
                            }
                            else
                            {
                                emp.AttachFile = strpath;
                            }
                        }
                        else
                        {
                            emp.AttachFile = item.AttachFile;
                        }
                    }
                    else
                    {
                        var objIdentMo = objEmpFamily.Where(w => w.RelName == item.RelName).ToList();
                        foreach (var existingFile in objIdentMo)
                        {
                            DeleteFile(existingFile.AttachFile);
                        }
                    }
                    emp.LineItem = LinItem;
                    emp.ChangedBy = User.UserName;
                    emp.ChangedOn = DateTime.Now;
                    unitOfWork.Add(emp);
                }
                #endregion

                #region Save Emp Education
                var objEmpEducation = unitOfWork.Set<HREmpEduc>().Where(w => w.EmpCode == Header.EmpCode).ToList();
                var ListEmpEduOld = objEmpEducation.Select(x => new HREmpEduc
                {
                    EduType = x.EduType,
                    AttachFile = x.AttachFile
                }).ToList();
                var removedEdu = ListEmpEduOld.Where(oldItem => !ListEducation.Any(newItem => newItem.EduType == oldItem.EduType)).ToList();
                foreach (var removed in removedEdu)
                {
                    if (!string.IsNullOrEmpty(removed.AttachFile))
                    {
                        var files = removed.AttachFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var file in files)
                        {
                            DeleteFile(file);
                        }
                    }
                }
                foreach (var item in objEmpEducation)
                {
                    unitOfWork.Delete(item);
                }
                foreach (var item in ListEducation)
                {
                    Attachment = item.AttachFile;
                    var emp = new HREmpEduc();
                    SwapData.SwapEmpEducation(emp, item, Header);
                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        var objIdentMo = objEmpEducation.Where(w => w.TranNo == item.TranNo).ToList();
                        if (!objIdentMo.Where(w => w.AttachFile == item.AttachFile).Any())
                        {
                            foreach (var existingFile in objIdentMo)
                            {
                                if (!string.IsNullOrEmpty(existingFile.AttachFile))
                                {
                                    var str1 = existingFile.AttachFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    var str2 = Attachment.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                    var filesToDelete = str1.Except(str2).ToList();
                                    foreach (var fileName in filesToDelete)
                                    {
                                        DeleteFile(fileName);
                                    }
                                }
                            }
                            var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, "Education", Header.EmpCode, Header.DEPT, Attachment);
                            if (strpath == "No_Path")
                            {
                                return "File_Exit";
                            }
                            else
                            {
                                emp.AttachFile = strpath;
                            }
                        }
                        else
                        {
                            emp.AttachFile = item.AttachFile;
                        }
                    }
                    emp.ChangedBy = User.UserName;
                    emp.ChangedOn = DateTime.Now;
                    unitOfWork.Add(emp);
                }
                #endregion

                #region Save Emp Contract
                var objEmpContract = unitOfWork.Set<HREmpContract>().Where(w => w.EmpCode == Header.EmpCode).ToList();
                var ListEmpContractOld = objEmpContract.Select(x => new HREmpContract
                {
                    ConType = x.ConType,
                    ContractPath = x.ContractPath
                }).ToList();
                var removedContract = ListEmpContractOld.Where(oldItem => !ListContract.Any(newItem => newItem.ConType == oldItem.ConType)).ToList();
                foreach (var removed in removedContract)
                {
                    if (!string.IsNullOrEmpty(removed.ContractPath))
                    {
                        var files = removed.ContractPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var file in files)
                        {
                            DeleteFile(file);
                        }
                    }
                }
                foreach (var item in objEmpContract)
                {
                    unitOfWork.Delete(item);
                }
                foreach (var item in ListContract)
                {
                    Attachment = item.ContractPath;
                    var emp = new HREmpContract();
                    SwapData.SwapEmpContract(emp, item, Header);
                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        var objContract = objEmpContract.Where(w => w.TranNo == item.TranNo).ToList();
                        if (!objContract.Where(w => w.ContractPath == item.ContractPath).Any())
                        {
                            foreach (var existingFile in objContract)
                            {
                                if (!string.IsNullOrEmpty(existingFile.ContractPath))
                                {
                                    var str1 = existingFile.ContractPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    var str2 = Attachment.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                    var filesToDelete = str1.Except(str2).ToList();
                                    foreach (var fileName in filesToDelete)
                                    {
                                        DeleteFile(fileName);
                                    }
                                }
                            }
                            var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, "Contract", Header.EmpCode, Header.DEPT, Attachment);
                            if (strpath == "No_Path")
                            {
                                return "File_Exit";
                            }
                            else
                            {
                                emp.ContractPath = strpath;
                            }
                        }
                        else
                        {
                            emp.ContractPath = item.ContractPath;
                        }
                    }
                    emp.ChangedBy = User.UserName;
                    emp.ChangedOn = DateTime.Now;
                    unitOfWork.Add(emp);
                }
                #endregion

                #region Save Emp Disciplinary
                var objEmpDisciplinary = unitOfWork.Set<HREmpDisciplinary>().Where(w => w.EmpCode == Header.EmpCode).ToList();
                var ListEmpDisciplinaryOld = objEmpDisciplinary.Select(x => new HREmpDisciplinary
                {
                    DiscType = x.DiscType,
                    AttachPath = x.AttachPath
                }).ToList();
                var removedDisciplinary = ListEmpDisciplinaryOld.Where(oldItem => !ListEmpDisciplinary.Any(newItem => newItem.DiscType == oldItem.DiscType)).ToList();
                foreach (var removed in removedDisciplinary)
                {
                    if (!string.IsNullOrEmpty(removed.AttachPath))
                    {
                        var files = removed.AttachPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var file in files)
                        {
                            DeleteFile(file);
                        }
                    }
                }
                foreach (var item in objEmpDisciplinary)
                {
                    unitOfWork.Delete(item);
                }
                foreach (var item in ListEmpDisciplinary)
                {
                    Attachment = item.AttachPath;
                    var emp = new HREmpDisciplinary();
                    SwapData.SwapEmpDisciplinary(emp, item, Header);
                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        var objIdentMo = objEmpDisciplinary.Where(w => w.DiscType == item.DiscType).ToList();
                        if (!objIdentMo.Where(w => w.AttachPath == item.AttachPath).Any())
                        {
                            foreach (var existingFile in objIdentMo)
                            {
                                if (!string.IsNullOrEmpty(existingFile.AttachPath))
                                {
                                    var str1 = existingFile.AttachPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    var str2 = Attachment.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                    var filesToDelete = str1.Except(str2).ToList();
                                    foreach (var fileName in filesToDelete)
                                    {
                                        DeleteFile(fileName);
                                    }
                                }
                            }
                            var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, "Disciplinary", Header.EmpCode, Header.DEPT, Attachment);
                            if (strpath == "No_Path")
                            {
                                return "File_Exit";
                            }
                            else
                            {
                                emp.AttachPath = strpath;
                            }
                        }
                        else
                        {
                            emp.AttachPath = item.AttachPath;
                        }
                    }
                    else
                    {
                        var objIdentMo = objEmpDisciplinary.Where(w => w.DiscType == item.DiscType).ToList();
                        foreach (var existingFile in objIdentMo)
                        {
                            DeleteFile(existingFile.AttachPath);
                        }
                    }
                    emp.ChangedBy = User.UserName;
                    emp.ChangedOn = DateTime.Now;
                    unitOfWork.Add(emp);
                }
                #endregion

                #region Save Emp Certificate
                var objEmpCertificate = unitOfWork.Set<HREmpCertificate>().Where(w => w.EmpCode == Header.EmpCode).ToList();
                var ListEmpCertificateOld = objEmpCertificate.Select(x => new HREmpCertificate
                {
                    CertType = x.CertType,
                    AttachFile = x.AttachFile
                }).ToList();
                var removedCertificate = ListEmpCertificateOld.Where(oldItem => !ListEmpCertificate.Any(newItem => newItem.CertType == oldItem.CertType)).ToList();
                foreach (var removed in removedCertificate)
                {
                    if (!string.IsNullOrEmpty(removed.AttachFile))
                    {
                        var files = removed.AttachFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var file in files)
                        {
                            DeleteFile(file);
                        }
                    }
                }
                foreach (var item in objEmpCertificate)
                {
                    unitOfWork.Delete(item);
                }
                foreach (var item in ListEmpCertificate)
                {
                    Attachment = item.AttachFile;
                    var emp = new HREmpCertificate();
                    var getDes = unitOfWork.Set<HRCertificationType>().FirstOrDefault(w => w.Code == item.CertType);
                    if (getDes != null)
                    {
                        emp.CertDesc = getDes.Description;
                    }
                    SwapData.SwapEmpCertificate(emp, item, Header);
                    if (!string.IsNullOrEmpty(Attachment))
                    {
                        var objIdentMo = objEmpCertificate.Where(w => w.CertType == item.CertType).ToList();
                        if (!objIdentMo.Where(w => w.AttachFile == item.AttachFile).Any())
                        {
                            foreach (var existingFile in objIdentMo)
                            {
                                if (!string.IsNullOrEmpty(existingFile.AttachFile))
                                {
                                    var str1 = existingFile.AttachFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    var str2 = Attachment.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                    var filesToDelete = str1.Except(str2).ToList();
                                    foreach (var fileName in filesToDelete)
                                    {
                                        DeleteFile(fileName);
                                    }
                                }
                            }
                            var strpath = clsCopyFile.CopyStructurePath(Header.CompanyCode, "Certificate", Header.EmpCode, Header.DEPT, Attachment);
                            if (strpath == "No_Path")
                            {
                                return "File_Exit";
                            }
                            else
                            {
                                emp.AttachFile = strpath;
                            }
                        }
                        else
                        {
                            emp.AttachFile = item.AttachFile;
                        }
                    }
                    else
                    {
                        var objIdentMo = objEmpCertificate.Where(w => w.CertType == item.CertType).ToList();
                        foreach (var existingFile in objIdentMo)
                        {
                            DeleteFile(existingFile.AttachFile);
                        }
                    }
                    emp.ChangedBy = User.UserName;
                    emp.ChangedOn = DateTime.Now;
                    unitOfWork.Add(emp);
                }
                #endregion

                #region Save Emp Medical
                var objEmpMedical = unitOfWork.Set<HREmpPhischk>().Where(w => w.EmpCode == Header.EmpCode).ToList();
                foreach (var item in objEmpMedical)
                {
                    unitOfWork.Delete(item);
                }
                foreach (var item in ListEmpMedical)
                {
                    var emp = new HREmpPhischk();
                    SwapData.SwapEmpMedical(emp, item, Header);
                    emp.ChangedBy = User.UserName;
                    emp.ChangedOn = DateTime.Now;
                    unitOfWork.Add(emp);
                }
                #endregion

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string DeleteEmp(string EmpCode)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == EmpCode);
                if (objMatch == null)
                {
                    return "STAFF_NE";
                }
                var ListGen = unitOfWork.Set<HISGenSalaryD>().Where(w => w.EmpCode == EmpCode).ToList();
                if (ListGen.Count > 0)
                {
                    return "ISGEN_SALARY";
                }

                var ListEmpID = unitOfWork.Set<HREmpIdentity>().Where(w => w.EmpCode == EmpCode).ToList();
                foreach (var item in ListEmpID)
                {
                    unitOfWork.Delete(item);
                }
                var ListEmpJobDes = unitOfWork.Set<HREmpJobDescription>().Where(w => w.EmpCode == EmpCode).ToList();
                foreach (var item in ListEmpJobDes)
                {
                    unitOfWork.Delete(item);
                }

                var SalaryAcc = unitOfWork.Repository<HREmpBankAcc>().Queryable().Where(w => w.EmpCode == EmpCode).ToList();
                foreach (var item in SalaryAcc)
                {
                    unitOfWork.Delete(item);
                }
                var SalaryAccList = unitOfWork.Repository<HREmpCareerBankList>().Queryable().Where(w => w.EmpCode == EmpCode).ToList();
                foreach (var item in SalaryAccList)
                {
                    unitOfWork.Delete(item);
                }
                var objMatchEmp = unitOfWork.Set<HREmpCareer>().FirstOrDefault(w => w.EmpCode == EmpCode);
                unitOfWork.Delete(objMatchEmp);
                unitOfWork.Delete(objMatch);
                unitOfWork.Save();
                ClsCopyFile copyFile = new ClsCopyFile();
                foreach (var item in ListEmpID)
                {
                    if (!string.IsNullOrEmpty(item.Attachment))
                    {
                        copyFile.DeleteFileStore(item.Attachment);
                    }
                }
                foreach (var item in ListEmpJobDes)
                {
                    if (!string.IsNullOrEmpty(item.Attachment))
                    {
                        copyFile.DeleteFileStore(item.Attachment);
                    }
                }
                if (!string.IsNullOrEmpty(objMatch.Images))
                {
                    copyFile.DeleteFileStore_URL(objMatch.Images);
                }
                if (!string.IsNullOrEmpty(objMatch.Signature))
                {
                    copyFile.DeleteFileStore_URL(objMatch.Signature);
                }
                string str = $"This staff {EmpCode} in company {objMatch.CompanyCode} has been deleted by user: {User.UserName}";
                SYSendTelegramObject Tel = new SYSendTelegramObject();
                Tel.User = User;
                Tel.BS = BS;
                List<object> ListObjectDictionary = new List<object>();
                WorkFlowResult result1 = Tel.Send_SMS_Telegram(str, "-425215333", true);//Group SunHR_
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string OnGridModifyIdentity(HREmpIdentity MModel, string Action, bool IsEdit = false, bool IsAddNewFile = false)
        {
            int LineItem = 1;
            if (ListEmpIdentity.Count() > 0)
            {
                LineItem = ListEmpIdentity.Max(w => w.LineItem) + 1;
            }
            if (MModel.IssueDate.HasValue && MModel.ExpiryDate.HasValue)
            {
                if (MModel.ExpiryDate.Value < MModel.IssueDate.Value)
                {
                    return "IN_DATE_RANK";
                }
            }
            if (Action == "ADD")
            {
                if (string.IsNullOrEmpty(MModel.IDCardNo))
                {
                    return "INV_DOC";
                }
                MModel.LineItem = LineItem;
                ListEmpIdentity.Add(MModel);
            }
            else if (Action == "EDIT")
            {
                var objCheck = ListEmpIdentity.Where(w => w.LineItem == MModel.LineItem).FirstOrDefault();
                if (objCheck != null)
                {
                    objCheck.IDCardNo = MModel.IDCardNo;
                    objCheck.IssueDate = MModel.IssueDate;
                    objCheck.ExpiryDate = MModel.ExpiryDate;
                    objCheck.IsActive = MModel.IsActive;
                    string temppath = objCheck.Attachment;
                    if (IsEdit)
                    {
                        if (!string.IsNullOrWhiteSpace(MModel.Attachment))
                        {
                            if (!string.IsNullOrWhiteSpace(objCheck.Attachment))
                            {
                                var existingFiles = objCheck.Attachment.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                var newFiles = MModel.Attachment.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                if (IsAddNewFile)
                                {
                                    foreach (var file in newFiles)
                                    {
                                        if (!existingFiles.Contains(file, StringComparer.OrdinalIgnoreCase))
                                        {
                                            existingFiles.Add(file);
                                        }
                                    }
                                    objCheck.Attachment = string.Join(";", existingFiles);
                                }
                                else
                                {
                                    existingFiles = newFiles;
                                    objCheck.Attachment = string.Join(";", existingFiles);
                                }
                            }
                            else
                            {
                                objCheck.Attachment = MModel.Attachment;
                            }
                        }
                        else
                        {
                            objCheck.Attachment = string.Empty;
                        }
                        //objCheck.AttachFile = MModel.AttachFile;
                        if (objCheck.TranNo == 0)
                        {
                            DeleteFile(temppath, objCheck.Attachment);
                        }
                    }

                }
            }
            else if (Action == "DELETE")
            {
                var objCheck = ListEmpIdentity.Where(w => w.LineItem == MModel.LineItem).FirstOrDefault();
                if (objCheck != null)
                {
                    ListEmpIdentity.Remove(objCheck);
                    if (objCheck.TranNo == 0)
                    {
                        DeleteFile(objCheck.Attachment);
                    }
                    return SYConstant.OK;
                }
                else
                {
                    return "INV_DOC";
                }
            }
            return SYConstant.OK;
        }
        public string OnGridModifyJobDescription(HREmpJobDescription MModel, string Action, bool IsEdit = false)
        {
            int LineItem = 1;
            if (string.IsNullOrEmpty(MModel.JobResponsive) && string.IsNullOrEmpty(MModel.JobDescription))
            {
                return "JOB_RESPONSIVE_AND_JOBDESCRIPTION_INVALID";
            }
            if (ListEmpJobDescription.Count() > 0)
            {
                LineItem = ListEmpJobDescription.Max(w => w.LineItem) + 1;
            }
            if (Action == "ADD")
            {
                MModel.LineItem = LineItem;
                ListEmpJobDescription.Add(MModel);
            }
            else if (Action == "EDIT")
            {
                var objCheck = ListEmpJobDescription.Where(w => w.LineItem == MModel.LineItem).FirstOrDefault();
                if (objCheck != null)
                {
                    objCheck.JobResponsive = MModel.JobResponsive;
                    objCheck.JobDescription = MModel.JobDescription;
                    objCheck.IsActive = MModel.IsActive;
                    string temppath = objCheck.Attachment;
                    if (IsEdit)
                    {
                        objCheck.Attachment = MModel.Attachment;
                        objCheck.Document = MModel.Document;
                        if (objCheck.TranNo == 0)
                        {
                            DeleteFile(temppath, objCheck.Attachment);
                        }
                    }

                }
            }
            else if (Action == "DELETE")
            {
                var objCheck = ListEmpJobDescription.Where(w => w.LineItem == MModel.LineItem).FirstOrDefault();
                if (objCheck != null)
                {
                    ListEmpJobDescription.Remove(objCheck);
                    if (objCheck.TranNo == 0)
                    {
                        DeleteFile(objCheck.Attachment);
                    }
                    return SYConstant.OK;
                }
                else
                {
                    return "INV_DOC";
                }
            }
            return SYConstant.OK;
        }

        public string OnGridModifyFamily(HREmpFamily MModel, string Action, bool IsEdit = false, bool IsAddNewFile = false)
        {
			//if (Action == "ADD")
			//{
			//    ListEmpFamily.Add(MModel);
			//}
			if (string.IsNullOrEmpty(MModel.RelCode) || string.IsNullOrEmpty(MModel.RelName) || string.IsNullOrEmpty(MModel.Sex))
			{
				if (Action != "DELETE")
					return "INV_DOC";
			}
			if (Action == "ADD")
			{
				int LineItem = 1;
				if (ListEmpFamily.Count() > 0)
				{
					LineItem = (int)ListEmpFamily.Max(w => w.TranNo) + 1;
				}
				MModel.TranNo = LineItem;
				ListEmpFamily.Add(MModel);
			}
			else if (Action == "EDIT")
            {
                var objCheck = ListEmpFamily.Where(w => w.TranNo == MModel.TranNo).FirstOrDefault();
                if (objCheck != null)
                {
                    objCheck.RelCode = MModel.RelCode;
                    objCheck.RelName = MModel.RelName;
                    objCheck.Sex = MModel.Sex;
                    objCheck.TaxDeduc = MModel.TaxDeduc;
                    objCheck.Child = MModel.Child;
                    objCheck.Spouse = MModel.Spouse;
                    objCheck.IDCard = MModel.IDCard;
                    objCheck.Occupation = MModel.Occupation;
                    objCheck.InSchool = MModel.InSchool;
                    objCheck.PhoneNo = MModel.PhoneNo;
                    objCheck.OtherExceptType = MModel.OtherExceptType;
                    string temppath = objCheck.AttachFile;
                    if (IsEdit)
                    {
                        if (!string.IsNullOrWhiteSpace(MModel.AttachFile))
                        {
                            if (!string.IsNullOrWhiteSpace(objCheck.AttachFile))
                            {
                                var existingFiles = objCheck.AttachFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                var newFiles = MModel.AttachFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                if (IsAddNewFile)
                                {
                                    foreach (var file in newFiles)
                                    {
                                        if (!existingFiles.Contains(file, StringComparer.OrdinalIgnoreCase))
                                        {
                                            existingFiles.Add(file);
                                        }
                                    }
                                    objCheck.AttachFile = string.Join(";", existingFiles);
                                }
                                else
                                {
                                    existingFiles = newFiles;
                                    objCheck.AttachFile = string.Join(";", existingFiles);
                                }
                            }
                            else
                            {
                                objCheck.AttachFile = MModel.AttachFile;
                            }
                        }
                        else
                        {
                            objCheck.AttachFile = string.Empty;
                        }
                        //objCheck.AttachFile = MModel.AttachFile;
                        if (objCheck.TranNo == 0)
                        {
                            DeleteFile(temppath, objCheck.AttachFile);
                        }
                    }

                }
            }
            else if (Action == "DELETE")
            {
                var objCheck = ListEmpFamily.Where(w => w.TranNo == MModel.TranNo).FirstOrDefault();
                if (objCheck != null)
                {
                    ListEmpFamily.Remove(objCheck);
                    if (objCheck.TranNo == 0)
                    {
                        DeleteFile(objCheck.AttachFile);
                    }
                    return SYConstant.OK;
                }
                else
                {
                    return "INV_DOC";
                }
            }
            return SYConstant.OK;
        }
        public string OnGridModifyEducation(HREmpEduc MModel, string Action, bool IsEdit = false, bool IsAddNewFile = false)
        {
			//if (Action == "ADD")
			//{
			//    ListEducation.Add(MModel);
			//}
			if (MModel.StartDate.HasValue && MModel.EndDate.HasValue)
			{
				if (MModel.StartDate.Value > MModel.EndDate.Value)
				{
					return "IN_DATE_RANK";
				}
			}
			if (string.IsNullOrEmpty(MModel.EduType) || string.IsNullOrEmpty(MModel.EdcCenter) || string.IsNullOrEmpty(MModel.Major))
			{
				if (Action != "DELETE")
					return "INV_DOC";
			}
			if (Action == "ADD")
			{
				long LineItem = 1;
				if (ListEducation.Count() > 0)
				{
					LineItem = ListEducation.Max(w => w.TranNo) + 1;
				}
				MModel.TranNo = LineItem;
				ListEducation.Add(MModel);
			}
			else if (Action == "EDIT")
            {
                var objCheck = ListEducation.Where(w => w.TranNo == MModel.TranNo).FirstOrDefault();
                if (objCheck != null)
                {
                    objCheck.EduType = MModel.EduType;
                    objCheck.StartDate = MModel.StartDate;
                    objCheck.EndDate = MModel.EndDate;
                    objCheck.EdcCenter = MModel.EdcCenter;
                    objCheck.Major = MModel.Major;
                    objCheck.Result = MModel.Result;
                    objCheck.Remark = MModel.Remark;
                    string temppath = objCheck.AttachFile;
                    if (IsEdit)
                    {
                        if (!string.IsNullOrWhiteSpace(MModel.AttachFile))
                        {
                            if (!string.IsNullOrWhiteSpace(objCheck.AttachFile))
                            {
                                var existingFiles = objCheck.AttachFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                var newFiles = MModel.AttachFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                if (IsAddNewFile)
                                {
                                    foreach (var file in newFiles)
                                    {
                                        if (!existingFiles.Contains(file, StringComparer.OrdinalIgnoreCase))
                                        {
                                            existingFiles.Add(file);
                                        }
                                    }
                                    objCheck.AttachFile = string.Join(";", existingFiles);
                                }
                                else
                                {
                                    existingFiles = newFiles;
                                    objCheck.AttachFile = string.Join(";", existingFiles);
                                }
                            }
                            else
                            {
                                objCheck.AttachFile = MModel.AttachFile;
                            }
                        }
                        else
                        {
                            objCheck.AttachFile = string.Empty;
                        }
                        //objCheck.AttachFile = MModel.AttachFile;
                        if (objCheck.TranNo == 0)
                        {
                            DeleteFile(temppath, objCheck.AttachFile);
                        }
                    }
                }
            }
            else if (Action == "DELETE")
            {
                var objCheck = ListEducation.Where(w => w.TranNo == MModel.TranNo).FirstOrDefault();
                if (objCheck != null)
                {
                    ListEducation.Remove(objCheck);
                    if (objCheck.TranNo == 0)
                    {
                        DeleteFile(objCheck.AttachFile);
                    }
                    return SYConstant.OK;
                }
                else
                {
                    return "INV_DOC";
                }
            }
            return SYConstant.OK;
        }
        public string OnGridModifyContract(HREmpContract MModel, string Action, bool IsEdit = false, bool IsAddNewFile = false)
        {
			if (string.IsNullOrEmpty(MModel.ConType) || string.IsNullOrEmpty(MModel.Conterm) || !MModel.FromDate.HasValue)
			{
				if (Action != "DELETE")
					return "INV_DOC";
			}
			if (Action == "ADD")
            {
				long LineItem = 1;
				if (ListContract.Count() > 0)
				{
					LineItem = ListContract.Max(w => w.TranNo) + 1;
				}
				MModel.TranNo = LineItem;
				ListContract.Add(MModel);
			}
            else if (Action == "EDIT")
            {
                var objCheck = ListContract.Where(w => w.TranNo == MModel.TranNo).FirstOrDefault();
                if (objCheck != null)
                {
                    objCheck.ConType = MModel.ConType;
                    objCheck.Conterm = MModel.Conterm;
                    objCheck.FromDate = MModel.FromDate;
                    objCheck.Description = MModel.Description;
                    string temppath = objCheck.ContractPath;
                    if (IsEdit)
                    {
                        if (!string.IsNullOrWhiteSpace(MModel.ContractPath))
                        {
                            if (!string.IsNullOrWhiteSpace(objCheck.ContractPath))
                            {
                                var existingFiles = objCheck.ContractPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                var newFiles = MModel.ContractPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                if (IsAddNewFile)
                                {
                                    foreach (var file in newFiles)
                                    {
                                        if (!existingFiles.Contains(file, StringComparer.OrdinalIgnoreCase))
                                        {
                                            existingFiles.Add(file);
                                        }
                                    }
                                    objCheck.ContractPath = string.Join(";", existingFiles);
                                }
                                else
                                {
                                    existingFiles = newFiles;
                                    objCheck.ContractPath = string.Join(";", existingFiles);
                                }
                            }
                            else
                            {
                                objCheck.ContractPath = MModel.ContractPath;
                            }
                        }
                        else
                        {
                            objCheck.ContractPath = string.Empty;
                        }
                        //objCheck.AttachFile = MModel.AttachFile;
                        if (objCheck.TranNo == 0)
                        {
                            DeleteFile(temppath, objCheck.ContractPath);
                        }
                    }

                }
            }
            else if (Action == "DELETE")
            {
                var objCheck = ListContract.Where(w => w.TranNo == MModel.TranNo).FirstOrDefault();
                if (objCheck != null)
                {
                    ListContract.Remove(objCheck);
                    if (objCheck.TranNo == 0)
                    {
                        DeleteFile(objCheck.ContractPath);
                    }
                    return SYConstant.OK;
                }
                else
                {
                    return "INV_DOC";
                }
            }
            return SYConstant.OK;
        }
        public string OnGridModifyDisciplinary(HREmpDisciplinary MModel, string Action, bool IsEdit = false, bool IsAddNewFile = false)
        {
			if (string.IsNullOrEmpty(MModel.DiscType) || string.IsNullOrEmpty(MModel.DiscAction))
			{
				if (Action != "DELETE")
					return "INV_DOC";
			}
			if (Action == "ADD")
            {
				long LineItem = 1;
				if (ListEmpDisciplinary.Count() > 0)
				{
					LineItem = ListEmpDisciplinary.Max(w => w.TranNo) + 1;
				}
				MModel.TranNo = LineItem;
				ListEmpDisciplinary.Add(MModel);
			}
            else if (Action == "EDIT")
            {
                var objCheck = ListEmpDisciplinary.Where(w => w.TranNo == MModel.TranNo).FirstOrDefault();
                if (objCheck != null)
                {
                    objCheck.DiscType = MModel.DiscType;
                    objCheck.TranDate = MModel.TranDate;
                    objCheck.DiscAction = MModel.DiscAction;
                    objCheck.Remark = MModel.Remark;
                    objCheck.Reference = MModel.Reference;
                    objCheck.Consequence = MModel.Consequence;
                    string temppath = objCheck.AttachPath;
                    if (IsEdit)
                    {
                        if (!string.IsNullOrWhiteSpace(MModel.AttachPath))
                        {
                            if (!string.IsNullOrWhiteSpace(objCheck.AttachPath))
                            {
                                var existingFiles = objCheck.AttachPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                var newFiles = MModel.AttachPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                if (IsAddNewFile)
                                {
                                    foreach (var file in newFiles)
                                    {
                                        if (!existingFiles.Contains(file, StringComparer.OrdinalIgnoreCase))
                                        {
                                            existingFiles.Add(file);
                                        }
                                    }
                                    objCheck.AttachPath = string.Join(";", existingFiles);
                                }
                                else
                                {
                                    existingFiles = newFiles;
                                    objCheck.AttachPath = string.Join(";", existingFiles);
                                }
                            }
                            else
                            {
                                objCheck.AttachPath = MModel.AttachPath;
                            }
                        }
                        else
                        {
                            objCheck.AttachPath = string.Empty;
                        }
                        //objCheck.AttachFile = MModel.AttachFile;
                        if (objCheck.TranNo == 0)
                        {
                            DeleteFile(temppath, objCheck.AttachPath);
                        }
                    }

                }
            }
            else if (Action == "DELETE")
            {
                var objCheck = ListEmpDisciplinary.Where(w => w.TranNo == MModel.TranNo).FirstOrDefault();
                if (objCheck != null)
                {
                    ListEmpDisciplinary.Remove(objCheck);
                    if (objCheck.TranNo == 0)
                    {
                        DeleteFile(objCheck.AttachPath);
                    }
                    return SYConstant.OK;
                }
                else
                {
                    return "INV_DOC";
                }
            }
            return SYConstant.OK;
        }
        public string OnGridModifyCertificate(HREmpCertificate MModel, string Action, bool IsEdit = false, bool IsAddNewFile = false)
        {
			if (string.IsNullOrEmpty(MModel.CertType))
			{
				if (Action != "DELETE")
					return "INV_DOC";
			}
			if (Action == "ADD")
            {
				long LineItem = 1;
				if (ListEmpCertificate.Count() > 0)
				{
					LineItem = ListEmpCertificate.Max(w => w.TranNo) + 1;
				}
				MModel.TranNo = LineItem;
				ListEmpCertificate.Add(MModel);
			}
            else if (Action == "EDIT")
            {
                var objCheck = ListEmpCertificate.Where(w => w.TranNo == MModel.TranNo).FirstOrDefault();
                if (objCheck != null)
                {
                    objCheck.CertType = MModel.CertType;
                    objCheck.IssueDate = MModel.IssueDate;
                    objCheck.Description = MModel.Description;
                    objCheck.Remark = MModel.Remark;
                    string temppath = objCheck.AttachFile;
                    if (IsEdit)
                    {
                        if (!string.IsNullOrWhiteSpace(MModel.AttachFile))
                        {
                            if (!string.IsNullOrWhiteSpace(objCheck.AttachFile))
                            {
                                var existingFiles = objCheck.AttachFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                var newFiles = MModel.AttachFile.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                if (IsAddNewFile)
                                {
                                    foreach (var file in newFiles)
                                    {
                                        if (!existingFiles.Contains(file, StringComparer.OrdinalIgnoreCase))
                                        {
                                            existingFiles.Add(file);
                                        }
                                    }
                                    objCheck.AttachFile = string.Join(";", existingFiles);
                                }
                                else
                                {
                                    existingFiles = newFiles;
                                    objCheck.AttachFile = string.Join(";", existingFiles);
                                }
                            }
                            else
                            {
                                objCheck.AttachFile = MModel.AttachFile;
                            }
                        }
                        else
                        {
                            objCheck.AttachFile = string.Empty;
                        }
                        //objCheck.AttachFile = MModel.AttachFile;
                        if (objCheck.TranNo == 0)
                        {
                            DeleteFile(temppath, objCheck.AttachFile);
                        }
                    }

                }
            }
            else if (Action == "DELETE")
            {
                var objCheck = ListEmpCertificate.Where(w => w.TranNo == MModel.TranNo).FirstOrDefault();
                if (objCheck != null)
                {
                    ListEmpCertificate.Remove(objCheck);
                    if (objCheck.TranNo == 0)
                    {
                        DeleteFile(objCheck.AttachFile);
                    }
                    return SYConstant.OK;
                }
                else
                {
                    return "INV_DOC";
                }
            }
            return SYConstant.OK;
        }
        public string OnGridModifyMedical(HREmpPhischk MModel, string Action, bool IsEdit = false)
        {
			if (string.IsNullOrEmpty(MModel.MedicalType) || string.IsNullOrEmpty(MModel.HospCHK) || string.IsNullOrEmpty(MModel.Result))
			{
				if (Action != "DELETE")
					return "INV_DOC";
			}
			if (Action == "ADD")
            {
				long LineItem = 1;
				if (ListEmpMedical.Count() > 0)
				{
					LineItem = ListEmpMedical.Max(w => w.TranNo) + 1;
				}
				MModel.TranNo = LineItem;
				ListEmpMedical.Add(MModel);
			}
            else if (Action == "EDIT")
            {
                var objCheck = ListEmpMedical.Where(w => w.TranNo == MModel.TranNo).FirstOrDefault();
                if (objCheck != null)
                {
                    objCheck.CHKDate = MModel.CHKDate;
                    objCheck.MedicalType = MModel.MedicalType;
                    objCheck.HospCHK = MModel.HospCHK;
                    objCheck.Result = MModel.Result;
                    objCheck.Description = MModel.Description;
                    objCheck.Remark = MModel.Remark;
                }
            }
            else if (Action == "DELETE")
            {
                var objCheck = ListEmpMedical.Where(w => w.TranNo == MModel.TranNo).FirstOrDefault();
                if (objCheck != null)
                {
                    ListEmpMedical.Remove(objCheck);
                    return SYConstant.OK;
                }
                else
                {
                    return "INV_DOC";
                }
            }
            return SYConstant.OK;
        }
        public string CreateMulti()
        {
            OnLoad();
            if (ListStaffProfile == null)
            {
                ListStaffProfile = new List<HRStaffProfile>();
            }
            if (ListEmpIdentityUP == null)
            {
                ListEmpIdentityUP = new List<HREmpIdentity>();
            }
            var ListLevel = unitOfWork.Set<HRLevel>().ToList();
            var listBranch = unitOfWork.Set<HRBranch>().ToList();
            var ListProType = unitOfWork.Set<HRProbationType>().ToList();
            try
            {
                var FIRST_LASTNAME = SYSettings.getSetting("FIRST_LASTNAME");
                var FIRST_LASTNAME_KH = SYSettings.getSetting("FIRST_LASTNAME_KH");
                int tranNo = unitOfWork.Repository<HREmpCareer>().Queryable()
                       .Select(w => (int?)w.TranNo)
                       .DefaultIfEmpty(0)
                       .Max()
                       .GetValueOrDefault();
                foreach (HRStaffProfile staff in ListStaffProfile)
                {
                    MessageError = staff.EmpCode;
                    Header = staff;
                    Header.TXPayType = staff.TXPayType.Trim().ToUpper();
                    if (Header.LeaveConf == null) Header.LeaveConf = Header.StartDate;
                    if (Header.DOB == null) Header.DOB = DateTime.Now.Date;
                    if (string.IsNullOrEmpty(Header.Branch) || listBranch.Where(w => w.Code == Header.Branch).ToList().Count == 0)
                    {
                        return "BRANCH_EN";
                    }
                    if (string.IsNullOrEmpty(Header.LevelCode) || ListLevel.Where(w => w.Code == Header.LevelCode).ToList().Count == 0)
                    {
                        return "LEVEL_EN";
                    }
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
                    if (string.IsNullOrEmpty(Header.BankName))
                    {
                        Header.BankName = "CC";
                    }
                    if (string.IsNullOrEmpty(Header.Currency)) Header.Currency = "USD";
                    Header.PayParam = "DEFAULT";
                    Header.CareerDesc = "NEWJOIN";
                    Header.TemLeave = "LB";
                    Header.CreatedOn = DateTime.Now.Date;
                    Header.CreatedBy = User.UserName;
                    Header.DateTerminate = new DateTime(1900, 1, 1);
                    Header.Status = SYDocumentStatus.A.ToString();
                    Header.IsExceptScan = false;
                    if (string.IsNullOrEmpty(Header.SalaryType)) Header.SalaryType = "M";
                    if (string.IsNullOrEmpty(Header.TemLeave)) Header.TemLeave = "LB";
                    if (string.IsNullOrEmpty(Header.PayParam)) Header.PayParam = "DEFAULT";
                    if (string.IsNullOrEmpty(Header.CareerDesc)) Header.CareerDesc = "NEWJOIN";
                    var Pro = ListProType.FirstOrDefault(w => w.Code == Header.ProbationType);
                    if (Pro != null)
                    {
                        int Months = 0;
                        int day = 0;
                        if (Pro != null)
                        {
                            Months = Pro.InMonth;
                            day = Pro.Day;
                        }
                        DateTime Probation = Header.StartDate.Value.AddMonths(Months).AddDays(day);
                        Header.Probation = Probation;
                        Header.ProbationType = Header.ProbationType;
                    }
                    else
                    {
                        Header.Probation = Header.StartDate.Value.AddMonths(1).AddDays(1);
                        Header.ProbationType = "PRO01";
                    }
                    Header.EffectDate = Header.StartDate;
                    HeaderCareer = new HREmpCareer();
                    ClsSwapData SwapData = new ClsSwapData();
                    SwapData.SwapStaffCarrer(HeaderCareer, Header, User);
                    var Iden = ListEmpIdentityUP.Where(w => w.EmpCode == Header.EmpCode).ToList();
                    int LinItem = 0;
                    foreach (var item in Iden)
                    {
                        item.IdentityTye = "IDCard";
                        LinItem += 1;
                        var empIden = new HREmpIdentity();
                        SwapData.SwapEmpIdentity(empIden, item, Header);
                        empIden.LineItem = LinItem;
                        unitOfWork.Add(empIden);
                    }

                    var empIden_ = new HREmpCareerBankList()
                    {
                        Company = Header.CompanyCode,
                        OldSalary = Header.Salary,
                        Increase = 0,
                        NewSalary = Header.Salary,
                        Reference = HeaderCareer.TranNo,
                        EmpCode = Header.EmpCode,
                        FromDate = HeaderCareer.EffectDate,
                        Todate = new DateTime(5000, 1, 1),
                    };
                    unitOfWork.Add(empIden_);

                    var bankAcc = new HREmpBankAcc()
                    {
                        EmpCode = Header.EmpCode,
                        Salary = Header.Salary,
                        Company = Header.CompanyCode,
                        BankName = Header.BankName,
                        IsNSSF = true,
                        Association = false,
                        IsActive = true,
                        Reference = HeaderCareer.TranNo,
                    };
                    bool Tax = false;
                    if (Header.TXPayType == "S") Tax = true;
                    bankAcc.IsTax = Tax;
                    unitOfWork.Add(bankAcc);

                    unitOfWork.Add(HeaderCareer);
                    unitOfWork.Add(Header);
                    unitOfWork.Save();
                }
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

        public string DeleteFile(string FilePath, string NewFilePath = ";")
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                return SYConstant.OK;
            }
            string root = HostingEnvironment.ApplicationPhysicalPath;
            string[] path = FilePath.Split(';');
            string[] newPaths = NewFilePath.Split(';');
            foreach (var pathItem in path)
            {
                if (!newPaths.Contains(pathItem))
                {
                    string fileNameDelete = root + pathItem;
                    fileNameDelete = fileNameDelete.Replace("~", "").Replace("/", "\\");
                    if (System.IO.File.Exists(fileNameDelete))
                    {
                        System.IO.File.Delete(fileNameDelete);
                    }
                }
            }
            return SYConstant.OK;
        }
        public DateTime GetProbationDate(DateTime StartDate, string ProType)
        {
            DateTime ProbationDate = DateTime.Now;
            var objProbation = unitOfWork.Set<HRProbationType>().FirstOrDefault(w => w.Code == ProType);
            int Months = 0;
            int day = 0;
            if (objProbation != null)
            {
                Months = objProbation.InMonth;
                day = objProbation.Day;
            }
            ProbationDate = StartDate.AddMonths(Months).AddDays(day);
            return ProbationDate;
        }
        public void IsValidChecking(int ID)
        {
            var objMatch = unitOfWork.Repository<HREmpCareer>().Queryable().FirstOrDefault(w => w.TranNo == ID);
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
            var ListLevel = unitOfWork.Set<SYHRModifySalary>().Where(w => w.UserName == User.UserName && w.Level == Level).ToList();
            if (ListLevel.Count > 0)
            {
                return true;
            }
            return false;
        }
        public Dictionary<string, dynamic> OnDataPersonal(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            keyValues.Add("GENDER_SELECT", new SYDataList(unitOfWork, "SEX").ListData);
            keyValues.Add("INITIAL_SELECT", new SYDataList(unitOfWork, "INITIAL").ListData);
            keyValues.Add("MARITAL_SELECT", new SYDataList(unitOfWork, "MARITAL").ListData);
            keyValues.Add("COUNTRY_SELECT", unitOfWork.Set<HRCountry>().ToList().OrderBy(w => w.Description));
            keyValues.Add("NATION_SELECT", unitOfWork.Set<HRNation>().ToList().OrderBy(w => w.Description));
            keyValues.Add("PROVICES_SELECT", unitOfWork.Set<HRProvice>().ToList().OrderBy(w => w.Description));
            keyValues.Add("COSTCENT_LIST", unitOfWork.Set<PRCostCenterGroup>().ToList());
            keyValues.Add("TELEGRAM_SELECT", unitOfWork.Set<TelegramBot>().ToList());

            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataJobLoading(params object[] keys)
        {
            bool IsDetail = false;
            if (keys != null && keys.Length > 0)
            {
                IsDetail = (bool)keys[0];
            }
            ClsFilterJob clsFilter = new ClsFilterJob();
            ClsFilterStaff clsfStaff = new ClsFilterStaff();
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            keyValues.Add("Company_SELECT", SYConstant.getCompanyDataAccess());
            keyValues.Add("BUSINESSUNIT_SELECT", clsFilter.LoadBusinessUnit());
            keyValues.Add("BRANCHES_SELECT", clsFilter.LoadBranch());
            keyValues.Add("DIVISION_SELECT", clsFilter.LoadDivision());
            keyValues.Add("DEPARTMENT_SELECT", clsFilter.LoadDepartment());
            keyValues.Add("OFFICE_SELECT", clsFilter.LoadOffice());
            keyValues.Add("SECTION_SELECT", clsFilter.LoadSection());
            keyValues.Add("GROUP_SELECT", clsFilter.LoadGroups());
            keyValues.Add("POSITION_SELECT", LoadpositionReportings(IsDetail));
            //keyValues.Add("POSITION_SELECT", clsFilter.LoadPosition());
            keyValues.Add("Level_SELECT", unitOfWork.Repository<HRLevel>().Queryable().ToList());
            keyValues.Add("JOBGRADE_SELECT", unitOfWork.Repository<HRJobGrade>().Queryable().ToList());
            keyValues.Add("LOCATION_SELECT", clsFilter.LoadLocation());
            keyValues.Add("CATEGORY_SELECT", unitOfWork.Repository<HRCategory>().Queryable().ToList());
            keyValues.Add("LINE_SELECT", unitOfWork.Repository<HRLine>().Queryable().ToList());
            keyValues.Add("EMPTYPE_SELECT", unitOfWork.Repository<HREmpType>().Queryable().ToList().OrderBy(w => w.Description));
            keyValues.Add("STAFFTYPE_SELECT", unitOfWork.Repository<HRWorkingType>().Queryable().ToList());
            keyValues.Add("FUNCTION_SELECT", unitOfWork.Repository<HRFunction>().Queryable().ToList().OrderBy(w => w.Description));

            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataLoadingOther(params object[] keys)
        {
            ClsFilterJob clsFilter = new ClsFilterJob();
            ClsFilterStaff clsfStaff = new ClsFilterStaff();
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            List<HRBankAccount> ListBankAccount = unitOfWork.Set<HRBankAccount>().ToList();
            var ListBank = ListBankAccount.Select(w => w.Bank).ToList();
            ListBank.Add("CC");
            keyValues.Add("IdentityTye_SELECT", unitOfWork.Set<HRIdentityType>().ToList());
            keyValues.Add("STAFF_SELECT", clsfStaff.OnLoadStaff(true));
            keyValues.Add("TAXPAID_SELECT", new SYDataList(unitOfWork, "TAXPAID").ListData);
            keyValues.Add("SALARYTYPE_SELECT", new SYDataList(unitOfWork, "SALARYTYPE_SELECT").ListData);
            keyValues.Add("PERAMETER_SELECT", unitOfWork.Set<PRParameter>().ToList().OrderBy(w => w.Description));
            keyValues.Add("PROBATIONTYPE_SELECT", unitOfWork.Set<HRProbationType>().ToList());
            keyValues.Add("SetEntitle_SELECT", unitOfWork.Set<HRSetEntitleH>().ToList());
            keyValues.Add("ROSTER_SELECT", unitOfWork.Set<ATBatch>().ToList());
            keyValues.Add("SEX_LIST", unitOfWork.Set<SYData>().Where(w => w.DropDownType == "SEX").ToList());
            keyValues.Add("BANK_SELECT", unitOfWork.Set<HRBank>().Where(w => w.IsActive == true
            && ListBank.Contains(w.Code)).ToList().OrderBy(w => w.Description));
            keyValues.Add("CURRENCY_SELECT", unitOfWork.Set<HRCurrency>().ToList());
            var Province = unitOfWork.Set<OBDesc>().SqlQuery("SELECT distinct Province as Code,ProvinceDesc1 as Desc1 ,ProvinceDesc2 as Desc2 FROM CFPostalCode");
            keyValues.Add("PROVINCE", Province.ToList());

            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataFamily(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            keyValues.Add("RelationshipType_LIST", unitOfWork.Set<HRRelationshipType>().ToList());
            keyValues.Add("SEX_LIST", new SYDataList(unitOfWork, "SEX").ListData);
            keyValues.Add("EXCEPT_TYPE", unitOfWork.Set<PRExceptType>().Where(w => w.IsActive == true).ToList());
            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataEducation(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            keyValues.Add("Education_LIST", unitOfWork.Set<HREduType>().ToList());
            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataContract(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            keyValues.Add("ContractType_LIST", unitOfWork.Set<HRContractType>().ToList());
            keyValues.Add("ContractConterm_LIST", new SYDataList(unitOfWork, "CONTRACT_TERM").ListData);
            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataDisciplinary(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            keyValues.Add("DISCIPLINAY_LIST", unitOfWork.Set<HRDisciplinType>().ToList());
            keyValues.Add("DISCIPLINACTION_SELECT", unitOfWork.Set<HRDisciplinAction>().ToList());
            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataCertificate(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            keyValues.Add("HREmpCertificate_LIST", unitOfWork.Set<HRCertificationType>().ToList());
            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataMedical(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            keyValues.Add("MEDICALTYPE_SELECT", unitOfWork.Set<HRMedicalType>().ToList());
            keyValues.Add("HOSP_SELECT", unitOfWork.Set<HRHospital>().ToList());
            keyValues.Add("PHCHKRESULT_SELECT", unitOfWork.Set<HRPHCKHResult>().ToList());
            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataListLoading(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            ClsFilterJob clsFilter = new ClsFilterJob();
            var objStatus = new SYDataList("STATUS_EMPLOYEE");
            keyValues.Add("STATUS_EMPLOYEE", objStatus.ListData);
            keyValues.Add("BUSINESSUNIT_SELECT", clsFilter.LoadBusinessUnit());
            keyValues.Add("BRANCHES_SELECT", clsFilter.LoadBranch());
            keyValues.Add("DEPARTMENT_SELECT", clsFilter.LoadDepartment());
            keyValues.Add("OFFICE_SELECT", clsFilter.LoadOffice());
            keyValues.Add("SECTION_SELECT", clsFilter.LoadSection());
            keyValues.Add("GROUP_SELECT", clsFilter.LoadGroups());

            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataListCarrer(params object[] keys)
        {
            bool IsDetail = false;
            if (keys != null && keys.Length > 0)
            {
                IsDetail = (bool)keys[0];
            }
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            ClsFilterStaff clsfStaff = new ClsFilterStaff();
            keyValues.Add("CareerHistories_SELECT", unitOfWork.Repository<HRCareerHistory>().Queryable().ToList());
            if (IsDetail) 
                keyValues.Add("SEPARATE_SELECT", unitOfWork.Repository<HRTerminType>().Queryable().ToList());
            keyValues.Add("STAFF_SELECT", clsfStaff.OnLoadStaff());
            return keyValues;
        }
        public string CopyPathPhoto(HRStaffProfile Staff, string DocType, string Image)
        {
            if (!string.IsNullOrEmpty(Image))
            {
                ClsCopyFile clsCopyFile = new ClsCopyFile();
                string savedFilePath;
                savedFilePath = clsCopyFile.CopyStructurePath(Staff.CompanyCode, DocType, Staff.EmpCode, Staff.DEPT, Image, true);

                return savedFilePath;
            }
            return null;
        }

        #region Carrer History
        public void OnLoandindEmpCarrer()
        {
            if (Filter == null)
            {
                Filter = new FTFilterEmployee();
                Filter.Status = SYDocumentStatus.A.ToString();
            }
            DateTime CurrentDate = DateTime.Now;
            //List<HR_EmpCareer_View> _listTemp = new List<HR_EmpCareer_View>();
            List<HRCompany> _ListCompany = SYConstant.getCompanyDataAccess();
            List<HRBranch> _ListBranch = SYConstant.getBranchDataAccess();
            List<HRLevel> _ListLevel = SYConstant.getLevelDataAccess();
            var branchid = _ListBranch.Select(s => s.Code).ToList();
            var companyCodes = _ListCompany.Select(x => x.Company).ToList();
            var levelCodes = _ListLevel.Select(x => x.Code).ToList();
            var businessUnits = string.IsNullOrEmpty(Filter.BusinessUnit)
                        ? new string[0] : Filter.BusinessUnit.Split(',').Select(bu => bu.Trim()).ToArray();
            var _listTemp = unitOfWork.Repository<HR_EmpCareer_View>().Queryable()
                           .Where(w => companyCodes.Contains(w.CompanyCode))
                           .Where(w => levelCodes.Contains(w.LevelCode))
                           .Where(w => branchid.Contains(w.BranchID));
			if (Filter.StartDate != null)
			{
				int filterMonth = Filter.StartDate.Value.Month;
				int filterYear = Filter.StartDate.Value.Year;
				DateTime filterStartDate = Filter.StartDate.Value;
				DateTime oneDayAfterStart = filterStartDate.AddDays(1);
				DateTime startOfMonth = new DateTime(filterYear, filterMonth, 1);
				DateTime startOfNextMonth = new DateTime(oneDayAfterStart.Year, oneDayAfterStart.Month, 1);

				if (Filter.Status == "A")
				{
					_listTemp = _listTemp.Where(w =>
						(w.Status == Filter.Status &&
						 w.StartDate.HasValue &&
						 w.StartDate.Value.Month == filterMonth &&
						 w.StartDate.Value.Year == filterYear &&
						 w.ToDate >= CurrentDate)
						|| (w.Status == "I" && w.FromDate > CurrentDate));
				}
				else if (Filter.Status == "I")
				{
                    _listTemp = _listTemp.Where(w =>
                     (w.Status == Filter.Status &&
                      w.StartDate.HasValue &&
                      w.StartDate.Value.Month == filterMonth &&
                      w.StartDate.Value.Year == filterYear &&
                      w.ToDate >= CurrentDate)
                     || (w.Status == "I" && w.FromDate > CurrentDate));
                }
                else if (Filter.Status == "I/A")
				{
					_listTemp = _listTemp.Where(w =>
						((w.Status == "A"||w.Status=="I") &&
						 w.StartDate.HasValue &&
						 w.StartDate.Value.Month == filterMonth &&
						 w.StartDate.Value.Year == filterYear &&
						 w.ToDate >= CurrentDate)
						|| (w.Status == "I" && w.FromDate > CurrentDate));
				}
			}
			else
            {
                if (Filter.Status == "A")
                    _listTemp = _listTemp.Where(w => (w.Status == Filter.Status && w.ToDate >= CurrentDate) || (w.Status == "I" && w.FromDate > CurrentDate));
                if (Filter.Status == "I")
                    _listTemp = _listTemp.Where(w => w.Status == Filter.Status && w.FromDate <= CurrentDate);
            }
            if (!string.IsNullOrEmpty(Filter.Branch))
            {
                var branchFilter = Filter.Branch.Split(',').Select(b => b.Trim()).Where(b => !string.IsNullOrEmpty(b)).ToList();
                _listTemp = _listTemp.Where(w => branchFilter.Contains(w.BranchID));
            }
            if (businessUnits.Any())
                _listTemp = _listTemp.Where(w => businessUnits.Contains(w.GroupDept));
            if (!string.IsNullOrEmpty(Filter.Department))
                _listTemp = _listTemp.Where(w => w.DEPT == Filter.Department);
            if (!string.IsNullOrEmpty(Filter.Office))
                _listTemp = _listTemp.Where(w => w.Office == Filter.Office);
            if (!string.IsNullOrEmpty(Filter.Team))
                _listTemp = _listTemp.Where(w => w.Groups == Filter.Team);
            _listTemp.OrderBy(w => w.EmpCode).OrderByDescending(w => w.FromDate);

            var ListCareer = GetCareerList(_listTemp.ToList());
          //.GroupBy(w => w.EmpCode)
          //.Select(g => g.OrderByDescending(w => w.FromDate.HasValue ? w.FromDate.Value : DateTime.MinValue).FirstOrDefault())
          //.Where(entry => entry != null)
          //.OrderBy(w => w.EmpCode)
          //.ToList();


            ListEmpCareer = ListCareer.OrderBy(w => w.FromDate).ToList();
        }
        public void OnCreatingEmpCarrer()
        {
            HeaderCareer = new HREmpCareer();
            ListCareerHis = new List<HR_Career>();
            HeaderStaff = new HR_STAFF_VIEW();
            ListHisBanKList = new List<HR_HisBanKList>();
            ListCareerBankList = new List<HREmpCareerBankList>();
            ToDate = "NOW";
            Increase = "0";
        }
        public List<HR_EmpCareer_View> GetCareerList(List<HR_EmpCareer_View> _listTemp)
        {
            var latestEntries = new Dictionary<string, HR_EmpCareer_View>();

            foreach (var entry in _listTemp)
            {
                if (entry == null || string.IsNullOrEmpty(entry.EmpCode))
                    continue;

                if (!latestEntries.TryGetValue(entry.EmpCode, out var existing) ||
                    (entry.FromDate.HasValue &&
                     (!existing.FromDate.HasValue || entry.FromDate.Value > existing.FromDate.Value)))
                {
                    latestEntries[entry.EmpCode] = entry;
                }
            }

            return latestEntries.Values
                .OrderBy(entry => entry.EmpCode)
                .ToList();
        }
        public string CreateCareerStaff()
        {
            OnLoad();
            try
            {
                ClsEmpValidate empValidate = new ClsEmpValidate();
                var msm = empValidate.Validate_Car_Move(HeaderCareer.EmpCode, HeaderCareer.EffectDate.Value, CareerMoveID);
                if (msm != SYConstant.OK)
                {
                    return msm;
                }
                //Career_BeforSave();
                HRStaffProfile objMatchHeader = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                ListCareer = new List<HREmpCareer>();
                var LstCareerCode = unitOfWork.Repository<HRCareerHistory>().Queryable().ToList();
                ListCareer = unitOfWork.Repository<HREmpCareer>().Queryable().Where(w => w.EmpCode == objMatchHeader.EmpCode).ToList();
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
                    var LstResType = unitOfWork.Repository<HRTerminType>().Queryable().Where(w => w.Code == HeaderCareer.resigntype).ToList();
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
                        var OtherStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == HeaderCareer.NewEmpCode);
                        var OtherCareer = unitOfWork.Repository<HREmpCareer>().FirstOrDefault(w => w.NewEmpCode == HeaderCareer.NewEmpCode);
                        if (OtherStaff != null || OtherCareer != null) return "DUPLICATE_EMP";
                    }
                    result.ToDate = HeaderCareer.EffectDate.Value.AddDays(-1);
                    unitOfWork.Update(result);
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
                    var ProType = unitOfWork.Repository<HRProbationType>().FirstOrDefault(w => w.Code == objMatchHeader.ProbationType);
                    objMatchHeader.StartDate = HeaderCareer.EffectDate;
                    objMatchHeader.Probation = objMatchHeader.StartDate.Value.AddMonths(ProType.InMonth);
                    objMatchHeader.LeaveConf = objMatchHeader.StartDate;
                    HeaderCareer.resigntype = "";
                }
                if (_CarCode.NotCalSalary == true)
                {
                    objMatchHeader.DateTerminate = HeaderCareer.EffectDate.Value;
                    objMatchHeader.TerminateStatus = HeaderCareer.CareerCode;
                    objMatchHeader.TerminateRemark = HeaderCareer.Remark;
                    objMatchHeader.Status = SYDocumentStatus.I.ToString();
                    var user = unitOfWork.Repository<SYUser>().FirstOrDefault(w => w.UserName == objMatchHeader.EmpCode);
                    if (user != null)
                    {
                        user.ExpireDate = HeaderCareer.EffectDate.Value.AddDays(-1);
                        unitOfWork.Update(user);
                    }
                }
                else
                {
                    objMatchHeader.DateTerminate = new DateTime(1900, 1, 1);
                    objMatchHeader.TerminateStatus = "";
                    objMatchHeader.TerminateRemark = "";
                }
                ClsSwapData swapData = new ClsSwapData();
                swapData.SwapStaff(objMatchHeader, HeaderCareer, User.UserName);
                var Pos = GetPosition(objMatchHeader, HeaderCareer.JobCode);
                HeaderCareer.JobCode = Pos;
                objMatchHeader.JobCode = Pos;
                unitOfWork.Add(HeaderCareer);
                unitOfWork.Update(objMatchHeader);
                unitOfWork.Save();

                if (!string.IsNullOrEmpty(NewEmpCode))
                {
                    var ChkCode = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                    var ChkNCode = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == HeaderCareer.NewEmpCode);
                    if (ChkCode != null && ChkNCode == null)
                    {
                        var DBC = new HumicaDBContext();
                        string Cmd = "exec dbo.Change_EmpCode '" + HeaderCareer.NewEmpCode + "','" + HeaderStaff.EmpCode + "','" + User.UserName + "'";
                        DBC.Database.ExecuteSqlCommand(Cmd);
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderCareer.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderCareer.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderCareer.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string CreateCareerStaff(HREmpCareerMovement EmpCarMov)
        {
            OnLoad();
            try
            {
                ClsEmpValidate empValidate = new ClsEmpValidate();
                var msm = empValidate.Validate_Car_Move(HeaderCareer.EmpCode, HeaderCareer.EffectDate.Value, CareerMoveID);
                if (msm != SYConstant.OK)
                {
                    return msm;
                }
                //Career_BeforSave();
                HRStaffProfile objMatchHeader = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                ListCareer = new List<HREmpCareer>();
                var LstCareerCode = unitOfWork.Repository<HRCareerHistory>().Queryable().ToList();
                ListCareer = unitOfWork.Repository<HREmpCareer>().Queryable().Where(w => w.EmpCode == objMatchHeader.EmpCode).ToList();
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
                    var LstResType = unitOfWork.Repository<HRTerminType>().Queryable().Where(w => w.Code == HeaderCareer.resigntype).ToList();
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
                        var OtherStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == HeaderCareer.NewEmpCode);
                        var OtherCareer = unitOfWork.Repository<HREmpCareer>().FirstOrDefault(w => w.NewEmpCode == HeaderCareer.NewEmpCode);
                        if (OtherStaff != null || OtherCareer != null) return "DUPLICATE_EMP";
                    }
                    result.ToDate = HeaderCareer.EffectDate.Value.AddDays(-1);
                    unitOfWork.Update(result);
                }
                bool IsSalary = empValidate.IsHideSalary(HeaderCareer.LevelCode, EmpCarMov.CreatedBy);
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
                HeaderCareer.CreateBy = EmpCarMov.CreatedBy;
                HeaderCareer.CreateOn = DateTime.Now;
                objMatchHeader.Status = SYDocumentStatus.A.ToString();
                //Update Staff Profile
                if (HeaderCareer.CareerCode == "NEWJOIN")
                {
                    var ProType = unitOfWork.Repository<HRProbationType>().FirstOrDefault(w => w.Code == objMatchHeader.ProbationType);
                    objMatchHeader.StartDate = HeaderCareer.EffectDate;
                    objMatchHeader.Probation = objMatchHeader.StartDate.Value.AddMonths(ProType.InMonth);
                    objMatchHeader.LeaveConf = objMatchHeader.StartDate;
                    HeaderCareer.resigntype = "";
                }
                if (_CarCode.NotCalSalary == true)
                {
                    objMatchHeader.DateTerminate = HeaderCareer.EffectDate.Value;
                    objMatchHeader.TerminateStatus = HeaderCareer.CareerCode;
                    objMatchHeader.TerminateRemark = HeaderCareer.Remark;
                    objMatchHeader.Status = SYDocumentStatus.I.ToString();
                    var user = unitOfWork.Repository<SYUser>().FirstOrDefault(w => w.UserName == objMatchHeader.EmpCode);
                    if (user != null)
                    {
                        user.ExpireDate = HeaderCareer.EffectDate.Value.AddDays(-1);
                        unitOfWork.Update(user);
                    }
                }
                else
                {
                    objMatchHeader.DateTerminate = new DateTime(1900, 1, 1);
                    objMatchHeader.TerminateStatus = "";
                    objMatchHeader.TerminateRemark = "";
                }
                ClsSwapData swapData = new ClsSwapData();
                swapData.SwapStaff(objMatchHeader, HeaderCareer, EmpCarMov.CreatedBy);
                var Pos = GetPosition(objMatchHeader, HeaderCareer.JobCode);
                HeaderCareer.JobCode = Pos;
                objMatchHeader.JobCode = Pos;
                unitOfWork.Add(HeaderCareer);
                unitOfWork.Update(objMatchHeader);
                unitOfWork.Save();

                if (!string.IsNullOrEmpty(NewEmpCode))
                {
                    var ChkCode = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                    var ChkNCode = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == HeaderCareer.NewEmpCode);
                    if (ChkCode != null && ChkNCode == null)
                    {
                        var DBC = new HumicaDBContext();
                        string Cmd = "exec dbo.Change_EmpCode '" + HeaderCareer.NewEmpCode + "','" + HeaderStaff.EmpCode + "','" + User.UserName + "'";
                        DBC.Database.ExecuteSqlCommand(Cmd);
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, EmpCarMov.CreatedBy, HeaderCareer.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, EmpCarMov.CreatedBy, HeaderCareer.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, EmpCarMov.CreatedBy, HeaderCareer.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string EditCareerStaff(string ID)
        {
            OnLoad();
            try
            {
                int TranNo = Convert.ToInt32(ID);
                HeaderCareer.TranNo = TranNo;
                HRStaffProfile objMatchHeader = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                var objCareer = unitOfWork.Repository < HREmpCareer>().Queryable().Where(w => w.EmpCode == HeaderCareer.EmpCode).ToList();
                var objMatch = objCareer.FirstOrDefault(w => w.TranNo == HeaderCareer.TranNo && w.EmpCode == HeaderStaff.EmpCode);
                if (objMatch == null)
                {
                    return "CAREERHISTORY_NE";
                }
                if (objMatchHeader == null)
                {
                    return "CAREERHISTORY_NE";
                }
                var LstCareerCode = unitOfWork.Repository < HRCareerHistory>().Queryable().ToList();
                var _CarCode = LstCareerCode.FirstOrDefault(x => x.Code == HeaderCareer.CareerCode);
                if (_CarCode == null)
                {
                    return "CAREERCODE_EN";
                }
                else if (_CarCode.NotCalSalary == true)
                {
                    IsInAtive = Convert.ToBoolean(_CarCode.NotCalSalary);
                    var LstResType = unitOfWork.Repository < HRTerminType>().Where(w => w.Code == HeaderCareer.resigntype).ToList();
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
                    unitOfWork.Update(result);
                }
                if (!string.IsNullOrEmpty(HeaderCareer.NewEmpCode))
                {
                    NewEmpCode = HeaderCareer.NewEmpCode;
                    objMatch.EmpCode = HeaderCareer.NewEmpCode;
                    var OtherStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == HeaderCareer.NewEmpCode);
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
                objMatch.AttachFile = HeaderCareer.AttachFile;
                //objMatch.JobDiscription = HeaderCareer.JobDiscription;
                //objMatch.JobResponsive = HeaderCareer.JobResponsive;
                //objMatch.AttachJD = HeaderCareer.AttachJD;
                objMatchHeader.Status = SYDocumentStatus.A.ToString();
                //Update Staff Profile
                if (HeaderCareer.CareerCode == "NEWJOIN")
                {
                    var ProType = unitOfWork.Repository<HRProbationType>().Queryable().FirstOrDefault(w => w.Code == objMatchHeader.ProbationType);
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
                ClsSwapData swapData = new ClsSwapData();
                swapData.SwapStaff(objMatchHeader, HeaderCareer, User.UserName);
                //Update with Reporting
                var Pos = GetPosition(objMatchHeader, HeaderCareer.JobCode);
                objMatch.JobCode= Pos;
                objMatchHeader.JobCode = Pos;
                unitOfWork.Update(objMatch);
                unitOfWork.Update(objMatchHeader);

                unitOfWork.Save();
                if (!string.IsNullOrEmpty(NewEmpCode))
                {
                    var ChkCode = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                    var ChkNCode = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == HeaderCareer.NewEmpCode);
                    if (ChkCode != null && ChkNCode == null)
                    {
                        var DBC = new HumicaDBContext();
                        string Cmd = "exec dbo.Change_EmpCode '" + HeaderCareer.NewEmpCode + "','" + HeaderStaff.EmpCode + "','" + User.UserName + "'";
                        DBC.Database.ExecuteSqlCommand(Cmd);
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.EDIT.ToString(), e, true);
            }
        }
        
        public List<HREmpCareer> GetEmployee(string EmpCode)
        {
            HeaderStaff = unitOfWork.Repository<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == EmpCode);
            var listEmpCareer = unitOfWork.Repository<HREmpCareer>().Queryable().Where(x => x.EmpCode == EmpCode).ToList();
            OnloadCareerHistory(EmpCode, HeaderStaff.LevelCode);
            return listEmpCareer;
        }
        public void OnloadCareerHistory(string EmpCode,string LevelCode)
        {
            var IsSalary = IsHideSalary(LevelCode);
            ListCareerHis = new List<HR_Career>();
            var listEmpCareer = unitOfWork.Repository<HREmpCareer>().Queryable().Where(w => w.EmpCode == EmpCode).ToList();
            foreach (var read in listEmpCareer.OrderByDescending(w => w.FromDate).ToList())
            {
                var obj = new HR_Career();
                obj.FromDate = read.FromDate.Value;
                obj.ToDate = read.ToDate.Value;
                obj.Department = read.DEPT;
                obj.Position = read.JobCode;
                obj.NewEmpCode = read.NewEmpCode;
                obj.Career = read.CareerCode;
                obj.Level = read.LevelCode;
                if (IsSalary == true)
                {
                    obj.NewSalary = read.NewSalary.ToString();
                    obj.OldSalary = read.OldSalary.ToString();
                    obj.Increase = read.Increase.ToString();
                }
                else
                {
                    obj.NewSalary = "#####";
                    obj.OldSalary = "#####";
                    obj.Increase = "#####";
                }
                obj.CreatedBy = read.CreateBy;
                obj.ChangedBy = read.ChangedBy;
                obj.CreatedOn = read.CreateOn;
                obj.ChangedOn = read.ChangedOn;
                ListCareerHis.Add(obj);
            }

        }

        public string Delete_EmpCareerHistory(string ID)
        {
            OnLoad();
            try
            {

                int TranNo = Convert.ToInt32(ID);
                HeaderCareer = new HREmpCareer();
                HeaderCareer.TranNo = TranNo;
                var objMatch = unitOfWork.Repository<HREmpCareer>().Queryable().FirstOrDefault(w => w.TranNo == TranNo);
                if (objMatch == null)
                {
                    return "CAREERHISTORY_NE";
                }

                if (objMatch.CareerCode == "TERMINAT")
                {
                    var matchLeave = unitOfWork.Repository<HREmpLeaveB>().Queryable().FirstOrDefault(x => x.EmpCode == objMatch.EmpCode && x.InYear == objMatch.EffectDate.Value.Year && x.LeaveCode == "AL");
                    if (matchLeave != null)
                    {
                        matchLeave.ALPayBalance = 0;
                        matchLeave.ALPayMonth = null;
                        matchLeave.InMonth = null;
                        unitOfWork.Update(matchLeave);
                    }
                }
                var objBankCareer = unitOfWork.Repository<HREmpCareerBankList>().Queryable().Where(w => w.EmpCode == objMatch.EmpCode && w.Reference == TranNo).ToList();
                foreach (var read in objBankCareer)
                {
                    unitOfWork.Delete(read);
                }
                long? objAcc = unitOfWork.Repository<HREmpBankAcc>().Queryable().Where(w => w.EmpCode == objMatch.EmpCode && w.Reference < TranNo).ToList().OrderByDescending(w => w.Reference).Max(x => x.Reference);
                var obj = unitOfWork.Repository<HREmpBankAcc>().Queryable().Where(w => w.EmpCode == objMatch.EmpCode && w.Reference == objAcc).ToList();
                foreach (var read in obj)
                {
                    var salaryCareer = unitOfWork.Repository<HREmpCareerBankList>().Queryable().FirstOrDefault(w => w.EmpCode == objMatch.EmpCode && w.Reference == read.Reference && w.Company == read.Company);
                    var objAcc_ = unitOfWork.Repository<HREmpBankAcc>().Queryable().FirstOrDefault(w => w.EmpCode == objMatch.EmpCode && w.Reference == read.Reference);
                    if (objAcc_ != null)
                    {
                        objAcc_.Salary = salaryCareer.NewSalary ?? 0;
                        objAcc_.IsActive = true;
                       unitOfWork.Update(objAcc_);
                    }
                }
                if (objMatch.LCK == 0 || objMatch.LCK == null)
                {
                    var listCareer = unitOfWork.Repository<HREmpCareer>().Queryable().Where(w => w.EmpCode == objMatch.EmpCode).ToList();
                    var previousCareer = listCareer.Where(w => w.TranNo < TranNo).OrderByDescending(x => x.FromDate).FirstOrDefault();

                    if (previousCareer != null)
                    {

                        previousCareer.ToDate = new DateTime(5000, 1, 1);
                        unitOfWork.Update(previousCareer);
                    }

                    unitOfWork.Delete(objMatch);
                    unitOfWork.Save();
                    if (!string.IsNullOrEmpty(previousCareer.NewEmpCode))
                    {
                        NewEmpCode = objMatch.EmpCode;

                        var DBC = new HumicaDBContext();
                        string Cmd = "exec dbo.Change_EmpCode '" + previousCareer.NewEmpCode + "','" + NewEmpCode + "','" + User.UserName + "'";
                        DBC.Database.ExecuteSqlCommand(Cmd);
                    }
                }

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.DELETE.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.DELETE.ToString(), e, true);
            }
        }
        public string uploadCareer()
        {
            OnLoad();
            try
            {
                if (ListCareer.Count == 0)
                {
                    return "NO_DATA";
                }
                var _ListEmp = unitOfWork.Repository<HREmpCareer>().Queryable().ToList();
                var _lstStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().ToList();
                var _lstCareerCode = unitOfWork.Repository<HRCareerHistory>().Queryable().ToList();
                var _lstResType = unitOfWork.Repository < HRTerminType>().Queryable().ToList();
                MessageError = "";
                int tranNo = _ListEmp.Select(w => (int?)w.TranNo).DefaultIfEmpty(0)
                            .Max().GetValueOrDefault();
                foreach (var staffs in ListCareer.ToList())
                {
                    Header = new HRStaffProfile();
                    Header = _lstStaff.FirstOrDefault(w => w.EmpCode == staffs.EmpCode.Trim());
                    if (Header == null)
                    {
                        MessageError = "EmpCode:" + staffs.EmpCode;
                        return "INV_DOC";
                    }
                    var objMatch = _ListEmp.Where(w => w.EmpCode == staffs.EmpCode).OrderByDescending(w => w.FromDate).ToList();
                    if (objMatch.ToList().Count() > 0)
                    {
                        var ObjUpdate = objMatch.Where(w => w.EffectDate.Value.Date != staffs.EffectDate.Value.Date).OrderByDescending(w => w.FromDate).First();
                        DateTime ToDate = new DateTime();
                        ToDate = staffs.EffectDate.Value.AddDays(-1);
                        if (objMatch.Where(w => w.EmpCode == staffs.EmpCode).ToList().Count > 0)
                        {
                            var _Careers = _ListEmp.Where(w => w.EffectDate.Value.Date != staffs.EffectDate.Value.Date && w.EmpCode == staffs.EmpCode).OrderByDescending(x => x.FromDate).FirstOrDefault();
                            var _Career = new HREmpCareer();
                            _Career = _Careers;
                            _Career.LCKEDIT = 1;
                            _Career.ToDate = ToDate;
                            unitOfWork.Update(_Career);
                        }
                        var ListEmpDub = objMatch.Where(w => w.EffectDate.Value.Date == staffs.EffectDate.Value.Date).ToList();
                        foreach (var read in ListEmpDub)
                        {
                           unitOfWork.Delete(read);
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
                        /////
                        HeaderCareer.TranNo = tranNo + 1;
                        HeaderCareer.EmpCode = staffs.EmpCode;
                        HeaderCareer.EmpCode = staffs.EmpCode;
                        HeaderCareer.CareerCode = staffs.CareerCode;
                        HeaderCareer.EmpType = ObjUpdate.EmpType;
                        HeaderCareer.CompanyCode = ObjUpdate.CompanyCode;
                        HeaderCareer.Branch = ObjUpdate.Branch;
                        HeaderCareer.DEPT = ObjUpdate.DEPT;
                        HeaderCareer.LOCT = ObjUpdate.LOCT;
                        HeaderCareer.Division = ObjUpdate.Division;
                        HeaderCareer.LINE = ObjUpdate.LINE;
                        HeaderCareer.SECT = ObjUpdate.SECT;
                        HeaderCareer.CATE = ObjUpdate.CATE;
                        HeaderCareer.LevelCode = ObjUpdate.LevelCode;
                        HeaderCareer.JobCode = ObjUpdate.JobCode;
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
                        bool IsSalary = IsHideSalary(HeaderCareer.LevelCode);
                        if (IsSalary)
                        {
                            var objBank = unitOfWork.Repository<HREmpBankAcc>().Queryable().Where(w => w.EmpCode == staffs.EmpCode && w.IsActive == true).ToList();
                            foreach (var item in objBank)
                            {
                                var objBank_ = unitOfWork.Repository<HREmpBankAcc>().Queryable().FirstOrDefault(w => w.Company == item.Company
                                                                && w.EmpCode == staffs.EmpCode && w.Reference == item.Reference);
                                objBank_.Salary = 0;
                                objBank_.IsActive = false;
                                unitOfWork.Update(objBank_);
                                //
                                var objBankCareer = unitOfWork.Repository<HREmpCareerBankList>().Queryable().FirstOrDefault(w => w.Company == item.Company
                                                                && w.EmpCode == staffs.EmpCode && w.Reference == item.Reference);
                                objBankCareer.Todate = HeaderCareer.EffectDate.Value.AddDays(-1);
                               unitOfWork.Update(objBankCareer);
                            }
                            foreach (var items in ListSalrySaparate.Where(w => w.EmpCode == staffs.EmpCode))
                            {
                                var empIden = new HREmpCareerBankList();
                                empIden.Company = items.Company;
                                empIden.OldSalary = items.Salary;
                                empIden.Increase = items.Increase;
                                empIden.NewSalary = items.Salary + items.Increase;
                                empIden.Reference = HeaderCareer.TranNo;
                                empIden.EmpCode = staffs.EmpCode;
                                empIden.FromDate = HeaderCareer.EffectDate;
                                empIden.Todate = new DateTime(5000, 1, 1);
                                unitOfWork.Add(empIden);
                                // Bank acc
                                var bankAcc = new HREmpBankAcc();
                                bankAcc.EmpCode = staffs.EmpCode;
                                bankAcc.Salary = items.Salary + items.Increase;
                                bankAcc.Reference = HeaderCareer.TranNo;
                                bankAcc.Company = items.Company;
                                bankAcc.BankName = items.BankName ?? "CC";
                                bankAcc.AccountNo = items.AccountNo ?? " ";
                                bankAcc.AccountName = items.AccountName ?? " ";
                                bankAcc.IsTax = items.IsTax;
                                bankAcc.IsNSSF = items.IsNSSF;
                                bankAcc.Association = items.Association;
                                bankAcc.IsActive = true;
                                unitOfWork.Add(bankAcc);
                            }
                        }

                        unitOfWork.Update(Header);

                        unitOfWork.Update(HeaderCareer);
                    }
                    tranNo++;
                }
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderCareer.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderCareer.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderCareer.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string uploadSalary()
        {
            try
            {

                if (ListSaparate.Count == 0)
                {
                    return "NO_DATA";
                }
                Header = new HRStaffProfile();
                string emp = "";
                foreach (var Emp in ListSaparate.ToList())
                {
                    Header.EmpCode = Emp.EmpCode;
                    var checkEmp = unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w => w.EmpCode == Emp.EmpCode).ToList();
                    var careers = unitOfWork.Repository<HREmpCareer>().Queryable().Where(x => x.EmpCode == Emp.EmpCode).ToList();
                    if (checkEmp.Count != 0 && careers.Count == 1)
                    {
                        var Staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(x => x.EmpCode == Emp.EmpCode);
                        var StaffHis = careers.FirstOrDefault(x => x.EmpCode == Emp.EmpCode);
                        if (emp != Emp.EmpCode)
                        {
                            var objBank = unitOfWork.Repository<HREmpCareerBankList>().Queryable().Where(w => w.EmpCode == Emp.EmpCode).ToList();
                            foreach (var item in objBank)
                            {
                                unitOfWork.Delete(item);
                            }
                            var objBankAcc = unitOfWork.Repository<HREmpBankAcc>().Queryable().AsEnumerable().Where(w => w.EmpCode == Emp.EmpCode).ToList();
                            foreach (var item in objBankAcc)
                            {
                                unitOfWork.Delete(item);
                            }
                        }
                        emp = Emp.EmpCode;
                        //add
                        var empIden = new HREmpCareerBankList();
                        empIden.Company = Emp.Company;
                        empIden.OldSalary = Emp.Salary;
                        empIden.Increase = 0;
                        empIden.NewSalary = Emp.Salary;
                        empIden.Reference = StaffHis.TranNo;
                        empIden.EmpCode = Emp.EmpCode;
                        empIden.FromDate = StaffHis.EffectDate;
                        empIden.Todate = new DateTime(5000, 1, 1);
                       unitOfWork.Add(empIden);

                        var bankAcc = new HREmpBankAcc();
                        bankAcc.EmpCode = Emp.EmpCode;
                        bankAcc.Salary = Emp.Salary;
                        bankAcc.Company = Emp.Company;
                        bankAcc.BankName = Emp.BankName;
                        bankAcc.IsTax = Emp.IsTax;
                        bankAcc.IsNSSF = Emp.IsNSSF;
                        bankAcc.Association = false;
                        bankAcc.IsActive = true;
                        bankAcc.Reference = StaffHis.TranNo;
                        unitOfWork.Add(bankAcc);
                    }
                    else
                    {
                        return "dupulicated" + Header.EmpCode;
                    }
                }

                unitOfWork.Save();
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
        }
        public virtual void OnDetailCarrerLoading(params object[] keys)
        {
            int ID = (int)keys[0];
            bool IsEdit = false;
            if (keys.Length > 1 && keys[1] is string str && !string.IsNullOrWhiteSpace(str))
            {
                IsEdit = true;
            }
            HeaderCareer = new HREmpCareer();
            ListCareerHis = new List<HR_Career>();
            ListHisBanKList = new List<HR_HisBanKList>();
            ListHisBanKList = new List<HR_HisBanKList>();
            ListCareerBankList = new List<HREmpCareerBankList>();

            var resualt = unitOfWork.Repository<HREmpCareer>().Queryable();
            IsInAtive = false;
            HeaderCareer = resualt.FirstOrDefault(w => w.TranNo == ID);
            ListCareerHis = new List<HR_Career>();
            if (HeaderCareer != null)
            {
                var CarCode = unitOfWork.Repository<HRCareerHistory>().Queryable().FirstOrDefault(w => w.Code == HeaderCareer.CareerCode);
                if (CarCode != null) IsInAtive = Convert.ToBoolean(CarCode.NotCalSalary);
                var IsSalary = IsHideSalary(HeaderCareer.LevelCode);
                if (IsSalary == true)
                {
                    NewSalary = HeaderCareer.NewSalary.ToString();
                    OldSalary = HeaderCareer.OldSalary.ToString();
                    Increase = HeaderCareer.Increase.ToString();
                }
                else
                {
                    //ViewData[ClsConstant.IS_SALARY] = true;
                    NewSalary = "#####";
                    OldSalary = "#####";
                    Increase = "#####";
                }
                OnloadCareerHistory(HeaderCareer.EmpCode, HeaderCareer.LevelCode);
                HeaderStaff = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().FirstOrDefault(x => x.EmpCode == HeaderCareer.EmpCode);
                if (IsEdit)
                {
                    var staffProfile = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(x => x.EmpCode == HeaderCareer.EmpCode);
                    if (staffProfile.PositionID.HasValue || staffProfile.PositionID > 0)
                    {
                        var pos = GetPositionView(HeaderCareer.JobCode);
                        HeaderCareer.JobCode = pos.ToString();
                    }
                }
                ListCareerBankList = unitOfWork.Repository<HREmpCareerBankList>().Queryable().Where(w => w.Reference == ID).ToList();
                List<HREmpCareerBankList> bankList = unitOfWork.Repository<HREmpCareerBankList>().Queryable().Where(w => w.EmpCode == HeaderCareer.EmpCode).ToList();
                foreach (var read in bankList.OrderByDescending(w => w.FromDate).ToList())
                {
                    var obj = new HR_HisBanKList();
                    obj.FromDate = read.FromDate.Value;
                    obj.Todate = read.Todate.Value;
                    obj.Branch = read.Company;
                    if (IsSalary == true)
                    {
                        obj.NewSalary = read.NewSalary.ToString();
                        obj.OldSalary = read.OldSalary.ToString();
                        obj.Increase = read.Increase.ToString();
                    }
                    else
                    {
                        obj.NewSalary = "#####";
                        obj.OldSalary = "#####";
                        obj.Increase = "#####";
                    }
                    ListHisBanKList.Add(obj);
                }
            }
        }

        public string Career_BeforSave(HREmpCareer empCareer)
        {
            ClsEmpValidate empValidate = new ClsEmpValidate();
            var msm = empValidate.Validate_CareerType(empCareer.CareerCode, empCareer.resigntype);
            if(empValidate.IsNotCalSalary)
            {
                var user = unitOfWork.Repository<SYUser>().FirstOrDefault(w => w.UserName == empCareer.EmpCode);
                if (user != null)
                {
                    user.ExpireDate = empCareer.EffectDate.Value;
                }
            }

            return SYConstant.OK;
        }
        #endregion

        #region Position Reporting
        public List<ClsPositionReporting> LoadpositionReportings(params object[] keys)
        {
            bool IsDetail = false;
            if (keys != null && keys.Length > 0)
            {
                IsDetail = (bool)keys[0];
            }
            IQueryable<HRPositionStructure> ListPostReporting=null;
            List<ClsPositionReporting> positionReportings = new List<ClsPositionReporting>();
            var Position = unitOfWork.Repository<HRPosition>().Queryable();
            if (!IsDetail)
            {
                ListPostReporting = unitOfWork.Repository<HRPositionStructure>().Queryable();
            }
            foreach (var JobCode in Position.Where(w => w.IsActive == true))
            {
                var Obj = new ClsPositionReporting();
                if (!IsDetail && ListPostReporting != null)
                {
                    var ListPR = ListPostReporting.Where(w => w.Code == JobCode.Code).ToList();
                    foreach (var Postion in ListPR)
                    {
                        Obj = new ClsPositionReporting();
                        SwapPosReport(Obj, JobCode, Postion.ID.ToString(), Postion.Code);
                        Obj.Reporting = Postion.ReportingID;
                        if (!string.IsNullOrEmpty(Obj.Reporting))
                        {
                            var PR = ListPostReporting.FirstOrDefault(w => w.ID.ToString() == Obj.Reporting);
                            if (PR != null) Obj.Reporting += ":" + JobCode.Code + ":" + PR.Description;
                        }
                        positionReportings.Add(Obj);
                    }
                }
                Obj = new ClsPositionReporting();
                SwapPosReport(Obj, JobCode, JobCode.Code, null);
                positionReportings.Add(Obj);
            }

            return positionReportings;
        }
        public void SwapPosReport(ClsPositionReporting S, HRPosition D, string Code, string DocReference)
        {
            S.Code = Code;
            S.Description = D.Description;
            S.SecDescription = D.SecDescription;
            S.DocReference = DocReference;
        }
        public string GetPosition(HRStaffProfile Staff,string Code)
        {
            string PostCode = Code;
            var Postion = unitOfWork.Repository<HRPositionStructure>().Queryable().FirstOrDefault(w => w.ID.ToString() == Code);
            if (Postion != null)
            {
                Staff.PositionID = Postion.ID;
                PostCode = Postion.Code;
            }
            else
            {
                Staff.PositionID = null;
            }
            return PostCode;
        }
        public string GetPositionView(string Code)
        {
            string PostCode = Code;
            var Postion = unitOfWork.Repository<HRPositionStructure>().Queryable().FirstOrDefault(w => w.Code == Code);
            if (Postion != null)
            {
                PostCode = Postion.ID.ToString();
            }
            return PostCode;
        }
        #endregion

        public class HR_HisBanKList
        {
            public string EmpCode { get; set; }
            public string Branch { get; set; }
            public string NewSalary { get; set; }
            public string OldSalary { get; set; }
            public string Increase { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime Todate { get; set; }

            public long Referent { get; set; }

            public int id { get; set; }

        }
        public class ClassCareerBankList
        {

            public string EmpCode { get; set; }

            public string Company { get; set; }

            public string BankName { get; set; }

            public string AccountNo { get; set; }

            public string AccountName { get; set; }

            public decimal Salary { get; set; }
            public decimal Increase { get; set; }

            public bool? IsTax { get; set; }

            public bool? IsNSSF { get; set; }

            public bool? Association { get; set; }

        }
    }

}