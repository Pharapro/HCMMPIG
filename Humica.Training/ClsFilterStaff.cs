using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Core
{
    public class ClsFilterStaff
    {
        private IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>(new HumicaDBContext());
        }
        public ClsFilterStaff()
        {
            OnLoad();
        }
        public IEnumerable<HRStaffProfile> OnLoadStaff(bool IsCurrent=false)
        {
            DateTime DateResign = DateTime.Now.AddYears(-2).Date;
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListBranchCode = ListBranch.Select(s => s.Code).ToList();
            IEnumerable<HRStaffProfile> ListTemp = unitOfWork.Set<HRStaffProfile>().Where(w => ListBranchCode.Contains(w.Branch) &&
             (w.DateTerminate == null || w.DateTerminate.Year == 1900 || w.DateTerminate > DateResign)).ToList();
            if(IsCurrent)
            {
                ListTemp = ListTemp.Where(w => w.Status == "A").ToList();
            }
            return ListTemp;
        }
        public string Get_Department(string Post)
        {
            string Description = "";
            HRDepartment JobDec = unitOfWork.Set<HRDepartment>().FirstOrDefault(w => w.Code == Post);
            if (JobDec != null)
            {
                Description = JobDec.Description;
            }
            return Description;
        }
        public string Get_Positon(string Post)
        {
            string Description = "";
            HRPosition position = unitOfWork.Set<HRPosition>().FirstOrDefault(w => w.Code == Post);
            if (position != null)
            {
                Description = position.Description;
            }
            return Description;
        }
        public string Get_Career(string Post)
        {
            string Description = "";
            HRCareerHistory JobDec = unitOfWork.Set<HRCareerHistory>().FirstOrDefault(w => w.Code == Post);
            if (JobDec != null)
            {
                Description = JobDec.Description;
            }
            return Description;
        }
        public ClsStaff GetStaff(string EmpCode)
        {
            ClsStaff staff = new ClsStaff();
            var  staffPro = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (staffPro != null)
            {
                staff.EmpCode=staffPro.EmpCode;
                staff.EmployeeName = staffPro.AllName;
                staff.EmployeeName2 = staffPro.OthAllName;
                staff.Department = Get_Department(staffPro.DEPT);
                staff.Position = Get_Positon(staffPro.JobCode);
            }

            return staff;
        }
    }
    public class ClsFilterJob
    {
        IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>(new HumicaDBContext());
        }
        public ClsFilterJob()
        {
            OnLoad();
        }
        public IEnumerable<HRBranch> LoadBranch()
        {
            var ListTemp = SYConstant.getBranchDataAccess();
            return ListTemp;
        }
        public IEnumerable<HRGroupDepartment> LoadBusinessUnit()
        {
            var ListTemp = unitOfWork.Set<HRGroupDepartment>().ToList();
            return ListTemp;
        }
        public IEnumerable<HRDivision> LoadDivision()
        {
            var ListTemp = unitOfWork.Set<HRDivision>().ToList();
            return ListTemp;
        }
        public IEnumerable<HRDepartment> LoadDepartment()
        {
            var ListTemp = unitOfWork.Set<HRDepartment>().Where(w => w.IsActive == true).ToList();
            return ListTemp;
        }
        public IEnumerable<HROffice> LoadOffice()
        {
            var ListTemp = unitOfWork.Set<HROffice>().Where(w => w.IsActive == true).ToList();
            return ListTemp;
        }
        public IEnumerable<HRSection> LoadSection()
        {
            var ListTemp = unitOfWork.Set<HRSection>().Where(w => w.IsActive == true).ToList();
            return ListTemp;
        }
        public IEnumerable<HRGroup> LoadGroups()
        {
            var ListTemp = unitOfWork.Set<HRGroup>().Where(w => w.IsActive == true).ToList();
            return ListTemp;
        }
        public IEnumerable<HRPosition> LoadPosition()
        {
            var ListTemp = unitOfWork.Set<HRPosition>().ToList();
            return ListTemp;
        }
        public IEnumerable<HRLocation> LoadLocation()
        {
            var ListTemp = unitOfWork.Set<HRLocation>().ToList();
            return ListTemp;
        }
        #region Payroll
        //public IEnumerable<PRPayPeriodItem> LoadPeriod()
        //{
        //    var ListTemp = unitOfWork.Set<PRPayPeriodItem>().OrderByDescending(w => w.StartDate).ToList();
        //    return ListTemp;
        //}
        #endregion
    }
}