using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections;
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
        public List<ClsStaff> ListStaff { get; set; }
        public ClsFilterStaff()
        {
            OnLoad();
        }
        public IEnumerable<HRStaffProfile> OnLoadStaff(bool IsCurrent = false)
        {
            DateTime DateResign = DateTime.Now.AddYears(-2).Date;
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListBranchCode = ListBranch.Select(s => s.Code).ToList();
            IEnumerable<HRStaffProfile> ListTemp = unitOfWork.Set<HRStaffProfile>().Where(w => ListBranchCode.Contains(w.Branch) &&
             (w.DateTerminate == null || w.DateTerminate.Year == 1900 || w.DateTerminate > DateResign)).ToList();
            if (IsCurrent)
            {
                ListTemp = ListTemp.Where(w => w.Status == "A").ToList();
            }
            return ListTemp;
        }
        public IQueryable<ClsStaff> OnLoandingEmployee()
        {
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListBranchCode = ListBranch.Select(s => s.Code).ToList();
            var lstStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w => w.Status == "A"
            && ListBranchCode.Contains(w.Branch));
            ListStaff = new List<ClsStaff>();
            foreach (var staff in lstStaff)
            {
                var objStaff = new ClsStaff();
                objStaff.EmpCode = staff.EmpCode;
                objStaff.EmployeeName = staff.AllName;
                objStaff.StartDate = staff.StartDate.Value;
                objStaff.Branch = Get_Branch(staff.Branch);
                objStaff.Department = Get_Department(staff.DEPT);
                objStaff.Position = Get_Positon(staff.JobCode);
                objStaff.Status = staff.Status;
                ListStaff.Add(objStaff);
            }
            return ListStaff.AsQueryable();
        }
        public string Get_Branch(string Code)
        {
            string Description = "";
            var ListBranch = SYConstant.getBranchDataAccess();
            HRBranch JobDec = ListBranch.FirstOrDefault(w => w.Code == Code);
            if (JobDec != null)
            {
                Description = JobDec.Description;
            }
            return Description;
        }
        public string Get_Department(string Post)
        {
            string Description = "";
            HRDepartment JobDec = unitOfWork.Repository<HRDepartment>().Queryable().FirstOrDefault(w => w.Code == Post);
            if (JobDec != null)
            {
                Description = JobDec.Description;
            }
            return Description;
        }
        public string Get_Positon(string Post)
        {
            string Description = "";
            HRPosition position = unitOfWork.Repository<HRPosition>().Queryable().FirstOrDefault(w => w.Code == Post);
            if (position != null)
            {
                Description = position.Description;
            }
            return Description;
        }
        public string Get_Location(string Location)
        {
            string Description = "";
            HRLocation position = unitOfWork.Set<HRLocation>().FirstOrDefault(w => w.Code == Location);
            if (position != null)
            {
                Description = position.Description;
            }
            return Description;
        }
        public string Get_Career(string Post)
        {
            string Description = "";
            HRCareerHistory JobDec = unitOfWork.Repository<HRCareerHistory>().Queryable().FirstOrDefault(w => w.Code == Post);
            if (JobDec != null)
            {
                Description = JobDec.Description;
            }
            return Description;
        }
        public ClsStaff GetStaff(string EmpCode)
        {
            ClsStaff staff = new ClsStaff();
            var staffPro = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (staffPro != null)
            {
                staff.EmpCode = staffPro.EmpCode;
                staff.EmployeeName = staffPro.AllName;
                staff.EmployeeName2 = staffPro.OthAllName;
                staff.Department = Get_Department(staffPro.DEPT);
                staff.Position = Get_Positon(staffPro.JobCode);
            }

            return staff;
        }
        public List<ClsPositionReporting> LoadpositionReportings(params object[] keys)
        {
            bool IsDetail = false;
            if (keys != null && keys.Length > 0)
            {
                IsDetail = (bool)keys[0];
            }
            IQueryable<HRPositionStructure> ListPostReporting = null;
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

        public IQueryable<ClsStaff> OnLoandUnderManager(string EmpCode)
        {
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListBranchCode = ListBranch.Select(s => s.Code).ToList();
            var lstStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w => w.Status == "A" &&
            (w.HODCode == EmpCode || w.FirstLine == EmpCode || w.FirstLine2 == EmpCode ||
            w.SecondLine == EmpCode || w.SecondLine2 == EmpCode ||
            w.OTFirstLine == EmpCode || w.OTSecondLine == EmpCode || w.OTthirdLine == EmpCode ||
            w.APPAppraisal == EmpCode || w.APPAppraisal2 == EmpCode || w.APPEvaluator == EmpCode ||
            w.APPTracking == EmpCode));
            ListStaff = new List<ClsStaff>();
            ClsFilterStaff filterStaff = new ClsFilterStaff();
            var ListRoleMapping = ClsFilterGeneral.LoadData_RoleMapping();
            foreach (var staff in lstStaff)
            {
                var staffProperties = typeof(HRStaffProfile).GetProperties();
                foreach (var role in ListRoleMapping)
                {
                    var property = staffProperties.FirstOrDefault(p => p.Name == role.Code);
                    var roleValue = property.GetValue(staff)?.ToString();
                    if (roleValue == EmpCode)
                    {
                        ListStaff.Add(new ClsStaff
                        {
                            EmpCode = staff.EmpCode,
                            EmployeeName = staff.AllName,
                            Position = filterStaff.Get_Positon(staff.JobCode),
                            DocumentType = role.Description,
                        });
                    }
                }
            }
            return ListStaff.AsQueryable();
        }
        public IQueryable<ClsStaff> OnLoandManager(string EmpCode)
        {
            var ListStaffMG = new List<ClsStaff>();
            var Staff =unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w=>w.EmpCode == EmpCode);
            var managerRoles = new (string code, string DocumentType)[]
            {
                (Staff.HODCode, "Head of Department"),
                (Staff.FirstLine, "First Line"),
                (Staff.FirstLine2, "First Line 2"),
                (Staff.SecondLine, "Second Line"),
                (Staff.SecondLine2, "Second Line 2"),
                (Staff.OTFirstLine, "OT First Line"),
                (Staff.OTSecondLine, "OT Second Line"),
                (Staff.OTthirdLine, "OT Third Line"),
                (Staff.APPAppraisal, "Appraisal 1"),
                (Staff.APPAppraisal2, "Appraisal 2"),
                (Staff.APPEvaluator, "Evaluator"),
                (Staff.APPTracking, "Tracking")
            };
            foreach (var (code, DocumentType) in managerRoles)
            {
                if (!string.IsNullOrEmpty(code))
                {
                    var manager = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == code && w.Status == "A");
                    if (manager != null)
                    {
                        ListStaffMG.Add(MapStaffToClsStaff(manager, DocumentType));
                    }
                }
            }

            //if (Staff != null)
            //{
            //    if(string.IsNullOrEmpty(Staff.HODCode))
            //    {
            //        var StaffMD = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staff.HODCode && w.Status == "A");
            //        if (StaffMD != null)
            //        {
            //            var objStaff = new ClsStaff();
            //            objStaff.EmpCode = StaffMD.EmpCode;
            //            objStaff.EmployeeName = StaffMD.AllName;
            //            objStaff.Position = Get_Positon(StaffMD.JobCode);
            //            objStaff.DocumentType = "Head of Department";
            //            //else if (staff.FirstLine == EmpCode) objStaff.DocumentType = "First Line";
            //            //else if (staff.FirstLine2 == EmpCode) objStaff.DocumentType = "First Line 2";
            //            //else if (staff.SecondLine == EmpCode) objStaff.DocumentType = "Second Line";
            //            //else if (staff.SecondLine2 == EmpCode) objStaff.DocumentType = "Second Line 2";
            //            //else if (staff.OTFirstLine == EmpCode) objStaff.DocumentType = "OT First Line";
            //            //else if (staff.OTSecondLine == EmpCode) objStaff.DocumentType = "OT Second Line";
            //            //else if (staff.OTthirdLine == EmpCode) objStaff.DocumentType = "OT Third Line";
            //            //else if (staff.APPAppraisal == EmpCode) objStaff.DocumentType = "Appraisal 1";
            //            //else if (staff.APPAppraisal2 == EmpCode) objStaff.DocumentType = "Appraisal 2";
            //            //else if (staff.APPEvaluator == EmpCode) objStaff.DocumentType = "Evaluator";
            //            //else if (staff.APPTracking == EmpCode) objStaff.DocumentType = "Tracking";
            //            ListStaffMG.Add(objStaff);
            //        }
            //    }
            //    if (string.IsNullOrEmpty(Staff.FirstLine))
            //    {
            //        var StaffMD = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staff.FirstLine && w.Status == "A");
            //        if (StaffMD != null)
            //        {
            //            ListStaffMG.Add(MapStaffToClsStaff(manager, label));

            //            var objStaff = new ClsStaff();
            //            objStaff.EmpCode = StaffMD.EmpCode;
            //            objStaff.EmployeeName = StaffMD.AllName;
            //            objStaff.Position = Get_Positon(StaffMD.JobCode);
            //            objStaff.DocumentType = "First Line";
            //            ListStaffMG.Add(objStaff);
            //        }
            //    }
            //    if (string.IsNullOrEmpty(Staff.FirstLine2))
            //    {
            //        var StaffMD = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staff.FirstLine2 && w.Status == "A");
            //        if (StaffMD != null)
            //        {
            //            var objStaff = new ClsStaff();
            //            objStaff.EmpCode = StaffMD.EmpCode;
            //            objStaff.EmployeeName = StaffMD.AllName;
            //            objStaff.Position = Get_Positon(StaffMD.JobCode);
            //            objStaff.DocumentType = "First Line 2";
            //            ListStaffMG.Add(objStaff);
            //        }
            //    }
            //    if (string.IsNullOrEmpty(Staff.SecondLine))
            //    {
            //        var StaffMD = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staff.SecondLine && w.Status == "A");
            //        if (StaffMD != null)
            //        {
            //            var objStaff = new ClsStaff();
            //            objStaff.EmpCode = StaffMD.EmpCode;
            //            objStaff.EmployeeName = StaffMD.AllName;
            //            objStaff.Position = Get_Positon(StaffMD.JobCode);
            //            objStaff.DocumentType = "Second Line";
            //            ListStaffMG.Add(objStaff);
            //        }
            //    }
            //    if (string.IsNullOrEmpty(Staff.SecondLine2))
            //    {
            //        var StaffMD = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staff.SecondLine2 && w.Status == "A");
            //        if (StaffMD != null)
            //        {
            //            var objStaff = new ClsStaff();
            //            objStaff.EmpCode = StaffMD.EmpCode;
            //            objStaff.EmployeeName = StaffMD.AllName;
            //            objStaff.Position = Get_Positon(StaffMD.JobCode);
            //            objStaff.DocumentType = "Second Line 2";
            //            ListStaffMG.Add(objStaff);
            //        }
            //    }
            //    if (string.IsNullOrEmpty(Staff.OTFirstLine))
            //    {
            //        var StaffMD = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staff.OTFirstLine && w.Status == "A");
            //        if (StaffMD != null)
            //        {
            //            var objStaff = new ClsStaff();
            //            objStaff.EmpCode = StaffMD.EmpCode;
            //            objStaff.EmployeeName = StaffMD.AllName;
            //            objStaff.Position = Get_Positon(StaffMD.JobCode);
            //            objStaff.DocumentType = "OT First Line";
            //            ListStaffMG.Add(objStaff);
            //        }
            //    }
            //    if (string.IsNullOrEmpty(Staff.OTSecondLine))
            //    {
            //        var StaffMD = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staff.OTSecondLine && w.Status == "A");
            //        if (StaffMD != null)
            //        {
            //            var objStaff = new ClsStaff();
            //            objStaff.EmpCode = StaffMD.EmpCode;
            //            objStaff.EmployeeName = StaffMD.AllName;
            //            objStaff.Position = Get_Positon(StaffMD.JobCode);
            //            objStaff.DocumentType = "OT Second Line";
            //            ListStaffMG.Add(objStaff);
            //        }
            //    }
            //    if (string.IsNullOrEmpty(Staff.OTthirdLine))
            //    {
            //        var StaffMD = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staff.OTthirdLine && w.Status == "A");
            //        if (StaffMD != null)
            //        {
            //            var objStaff = new ClsStaff();
            //            objStaff.EmpCode = StaffMD.EmpCode;
            //            objStaff.EmployeeName = StaffMD.AllName;
            //            objStaff.Position = Get_Positon(StaffMD.JobCode);
            //            objStaff.DocumentType = "OT Third Line";
            //            ListStaffMG.Add(objStaff);
            //        }
            //    }
            //    if (string.IsNullOrEmpty(Staff.APPAppraisal))
            //    {
            //        var StaffMD = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staff.APPAppraisal && w.Status == "A");
            //        if (StaffMD != null)
            //        {
            //            var objStaff = new ClsStaff();
            //            objStaff.EmpCode = StaffMD.EmpCode;
            //            objStaff.EmployeeName = StaffMD.AllName;
            //            objStaff.Position = Get_Positon(StaffMD.JobCode);
            //            objStaff.DocumentType = "Appraisal 1";
            //            ListStaffMG.Add(objStaff);
            //        }
            //    }
            //    if (string.IsNullOrEmpty(Staff.APPAppraisal2))
            //    {
            //        var StaffMD = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staff.APPAppraisal2 && w.Status == "A");
            //        if (StaffMD != null)
            //        {
            //            var objStaff = new ClsStaff();
            //            objStaff.EmpCode = StaffMD.EmpCode;
            //            objStaff.EmployeeName = StaffMD.AllName;
            //            objStaff.Position = Get_Positon(StaffMD.JobCode);
            //            objStaff.DocumentType = "Appraisal 2";
            //            ListStaffMG.Add(objStaff);
            //        }
            //    }
            //    if (string.IsNullOrEmpty(Staff.APPEvaluator))
            //    {
            //        var StaffMD = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staff.APPEvaluator && w.Status == "A");
            //        if (StaffMD != null)
            //        {
            //            var objStaff = new ClsStaff();
            //            objStaff.EmpCode = StaffMD.EmpCode;
            //            objStaff.EmployeeName = StaffMD.AllName;
            //            objStaff.Position = Get_Positon(StaffMD.JobCode);
            //            objStaff.DocumentType = "Evaluator";
            //            ListStaffMG.Add(objStaff);
            //        }
            //    }
            //    if (string.IsNullOrEmpty(Staff.APPTracking))
            //    {
            //        var StaffMD = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Staff.APPTracking && w.Status == "A");
            //        if (StaffMD != null)
            //        {
            //            var objStaff = new ClsStaff();
            //            objStaff.EmpCode = StaffMD.EmpCode;
            //            objStaff.EmployeeName = StaffMD.AllName;
            //            objStaff.Position = Get_Positon(StaffMD.JobCode);
            //            objStaff.DocumentType = "Tracking";
            //            ListStaffMG.Add(objStaff);
            //        }
            //    }
            //}
           
            return ListStaffMG.AsQueryable();
        }
        private ClsStaff MapStaffToClsStaff(HRStaffProfile staff, string documentType)
        {
            return new ClsStaff
            {
                EmpCode = staff.EmpCode,
                EmployeeName = staff.AllName,
                Position = Get_Positon(staff.JobCode),
                DocumentType = documentType
            };
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
            var ListTemp = unitOfWork.Repository<HRDepartment>().Queryable().Where(w => w.IsActive == true).ToList();
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
        public IEnumerable<HRTerminType> LoadSeperateType(string CareerCode)
        {
            return unitOfWork.Repository<HRTerminType>().Queryable().Where(w => w.CareerCode == CareerCode).ToList();
        }

        #region Payroll
        public IEnumerable<PRPayPeriodItem> LoadPeriod()
        {
            var ListTemp = unitOfWork.Set<PRPayPeriodItem>().OrderByDescending(w => w.StartDate).ToList();
            return ListTemp;
        }
        #endregion
    }
    public class ClsPositionReporting
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string SecDescription { get; set; }
        public string DocReference { get; set; }
        public string Reporting { get; set; }
    }
}