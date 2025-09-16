using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Repo;
using System;
namespace Humica.Employee
{
    public class ClsSwapData
    {
        public void SwapStaff(HRStaffProfile S, HREmpCareer D,string UserName)
        {
            S.CompanyCode = D.CompanyCode;
            S.GroupDept = D.GroupDept;
            S.Branch = D.Branch;
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
            S.ChangedBy = UserName;
            S.ChangedOn = DateTime.Now;
            S.Functions = D.Functions;
            S.Office = D.Office;
            S.Groups = D.Groups;
        }
        public void SwapStaffCarrer(HREmpCareer D, HRStaffProfile S,SYUser User)
        {
            D.EmpCode = S.EmpCode;
            D.CareerCode = S.CareerDesc;
            D.EmpType = S.EmpType;
            D.CompanyCode = S.CompanyCode;
            D.Branch = S.Branch;
            D.DEPT = S.DEPT;
            D.LOCT = S.LOCT;
            D.Division = S.Division;
            D.LINE = S.Line;
            D.SECT = S.SECT;
            D.CATE = S.CATE;
            D.LevelCode = S.LevelCode;
            D.JobCode = S.JobCode;
            D.JobGrade = S.JobGrade;
            D.JobDesc = S.POSTDESC;
            D.JobSpec = S.JOBSPEC;
            D.EstartSAL = S.Salary.ToString();
            D.EIncrease = S.Salary.ToString();
            D.ESalary = S.ESalary;
            D.SupCode = S.SubFunction;
            D.FromDate = S.StartDate;
            D.ToDate = Convert.ToDateTime("01/01/5000");
            D.EffectDate = S.EffectDate;
            D.ProDate = S.StartDate;
            D.Reason = "New Join";
            D.Remark = "Welcome to " + S.CompanyCode;
            D.Appby = "";
            D.AppDate = S.StartDate.Value.ToString("dd-MM-yyyy");
            D.VeriFyBy = "";
            D.VeriFYDate = S.StartDate.Value.ToString("dd-MM-yyyy");
            D.LCK = 1;
            D.OldSalary = S.Salary;
            D.Increase = 0;
            D.Functions = S.Functions;
            D.NewSalary = S.Salary;
            D.JobGrade = S.JobGrade;
            D.PersGrade = S.PersGrade;
            D.HomeFunction = S.HomeFunction;
            D.Functions = S.Functions;
            D.SubFunction = S.SubFunction;
            D.StaffType = S.StaffType;
            D.CreateBy = User.UserName;
            D.CreateOn = DateTime.Now;
            D.GroupDept = S.GroupDept;
            D.Office = S.Office;
            D.Groups = S.Groups;
        }
        public void SwapEmpIdentity(HREmpIdentity D, HREmpIdentity S, HRStaffProfile Staff)
        {
            D.CompanyCode = Staff.CompanyCode;
            D.EmpCode = Staff.EmpCode;
            D.IdentityTye = S.IdentityTye;
            D.IDCardNo = S.IDCardNo;
            D.IssueDate = S.IssueDate;
            D.ExpiryDate = S.ExpiryDate;
            D.Document = S.Document;
            D.IsActive = S.IsActive;
        }
        public void SwapEmpJD(HREmpJobDescription D, HREmpJobDescription S, HRStaffProfile Staff)
        {
            D.CompanyCode = Staff.CompanyCode;
            D.EmpCode = Staff.EmpCode;
            D.JobDescription = S.JobDescription;
            D.JobResponsive = S.JobResponsive;
            D.Document = S.Document;
            D.IsActive = S.IsActive;
        }

        public void SwapEmpFamily(HREmpFamily D, HREmpFamily S, HRStaffProfile Staff)
        {
            D.EmpCode = Staff.EmpCode;
            D.RelCode = S.RelCode;
            D.RelName = S.RelName;
            D.Sex = S.Sex;
            D.DateOfBirth = S.DateOfBirth;
			D.EffectiveDate = S.EffectiveDate;
			D.ToDate = S.ToDate;
			D.IDCard = S.IDCard;
            D.PhoneNo = S.PhoneNo;
            D.TaxDeduc = S.TaxDeduc;
            D.Occupation = S.Occupation;
            D.Child = S.Child;
            D.Spouse = S.Spouse;
            D.InSchool = S.InSchool;
            D.Document = S.Document;
            D.OtherExceptType = S.OtherExceptType;
        }
        public void SwapEmpEducation(HREmpEduc D, HREmpEduc S, HRStaffProfile Staff)
        {
            D.EmpCode = Staff.EmpCode;
            D.EduType = S.EduType;
            D.StartDate = S.StartDate;
            D.EndDate = S.EndDate;
            D.EdcCenter = S.EdcCenter;
            D.Major = S.Major;
            D.Result = S.Result;
            D.Remark = S.Remark;
            //D.AttachFile = S.AttachFile;
        }
        public void SwapEmpContract(HREmpContract D, HREmpContract S, HRStaffProfile Staff)
        {
            D.EmpCode = Staff.EmpCode;
            D.ConType = S.ConType;
            D.Conterm = S.Conterm;
            D.FromDate = S.FromDate;
            D.Description = S.Description;
            //D.ContractPath = S.ContractPath;
        }

        public void SwapEmpCertificate(HREmpCertificate D, HREmpCertificate S, HRStaffProfile Staff)
        {
            D.EmpCode = Staff.EmpCode;
            D.AllName = Staff.AllName;
            D.CertType = S.CertType;
            D.IssueDate = S.IssueDate;
            D.Description = S.Description;
            D.Remark = S.Remark;
        }
        public void SwapEmpMedical(HREmpPhischk D, HREmpPhischk S, HRStaffProfile Staff)
        {
            D.EmpCode = Staff.EmpCode;
            D.CHKDate = S.CHKDate;
            D.MedicalType = S.MedicalType;
            D.HospCHK = S.HospCHK;
            D.Result = S.Result;
            D.Description = S.Description;
            D.Remark = S.Remark;
        }
        public void SwapEmpDisciplinary(HREmpDisciplinary D, HREmpDisciplinary S, HRStaffProfile Staff)
        {
            D.EmpCode = Staff.EmpCode;
            D.DiscType = S.DiscType;
            D.TranDate = S.TranDate;
            D.DiscAction = S.DiscAction;
            //D.AttachPath = S.AttachPath;
            D.Remark = S.Remark;
            D.Reference = S.Reference;
            D.Consequence = S.Consequence;
        }
        public void SwapCar_Movement(HREmpCareer S, HREmpCareerMovement D)
        {
            S.EmpCode = D.EmpCode;
            S.CompanyCode = D.CompanyCode;
            S.GroupDept = D.NewBusinessUnit;
            S.Division = D.NewDivision;
            S.Branch = D.NewBranch;
            S.DEPT = D.NewDepartment;
            S.Office = D.NewOffice;
            S.SECT = D.NewSection;
            S.Groups = D.NewGroups;
            S.JobCode = D.NewPosition;
            if (D.PositionID.HasValue && D.PositionID.Value > 0)
            {
                S.JobCode = D.PositionID.ToString();
            }
            S.LevelCode = D.NewJobLevel;
            S.JobGrade = D.NewJobGrade;
            S.CATE = D.NewCategory;
            S.EmpType = D.NewEmployeeType;
            S.LOCT = D.NewLocation;
            S.StaffType = D.NewStaffType;
            S.CareerCode = D.CareerCode;
            S.OldSalary = D.OldSalary;
            S.Increase = D.Increase;
            S.NewSalary = D.NewSalary;
            S.EffectDate = D.EffectDate;
            S.NewEmpCode = D.NewEmpCode;
            S.Reason = D.Reason;
            S.resigntype = D.Resigntype;
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
}
