using Humica.Calculate;
using Humica.Condition;
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
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Employee
{
    public class ClsEmpCareerMovement : IClsEmpCareerMovement
    {
        protected IUnitOfWork unitOfWork;
        public string ScreenId { get; set; }
        public string EmpID { get; set; }
        public bool IsSalary { get; set; }
        public string NewSalary { get; set; }
        public string OldSalary { get; set; }
        public string Increase { get; set; }
        public bool IsInAtive { get; set; }
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public HREmpCareerMovement Header { get; set; }
        public IQueryable<ClsStaff> ListStaff { get; set; }
        public List<HREmpUnderManager> ListUnderManager { get; set; }
        public List<HREmpUnderManager> ListAddUManager { get; set; }
        public List<HRStaffProfile> ListAddEmp { get; set; }
        public List<HRStaffProfile> ListEmpByDept { get; set; }
        public List<ClsEmpCarrer> ListEmpCareerMove { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsEmpCareerMovement()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void OnLoandCareerMovement()
        {
            if (Filter == null)
            {
                Filter = new FTFilterEmployee();
                Filter.Status = SYDocumentStatus.A.ToString();
            }
            ListEmpCareerMove = new List<ClsEmpCarrer>();
            var lstCareerMove = unitOfWork.Repository<HREmpCareerMovement>().Queryable();
            List<ClsEmpCarrer> Carrer = new List<ClsEmpCarrer>();
            ClsEmpValidate empValidate = new ClsEmpValidate();
            foreach (var item in lstCareerMove)
            {
                var Obj = new ClsEmpCarrer();
                Obj.ID = item.ID;
                Obj.CompanyCode = item.CompanyCode;
                Obj.EmpCode = item.EmpCode;
                Obj.EmployeeName = item.EmployeeName;
                Obj.CareerType = item.CareerType;
                Obj.Branch = item.Branch;
                Obj.Department = item.Department;
                Obj.Position = item.Position;
                //bool IsSalary = empValidate.IsHideSalary(item.JobLevel, User.UserName);
                //if (IsSalary)
                //{
                //    Obj.OldSalary = item.OldSalary.ToString();
                //    Obj.Increase = item.Increase.ToString();
                //    Obj.NewSalary = item.NewSalary.ToString();
                //}
                //else
                //{
                //    Obj.NewSalary = "#####";
                //    Obj.OldSalary = "#####";
                //    Obj.Increase = "#####";
                //}
                Obj.EffectDate = item.EffectDate;
                Obj.Status = item.Status;
                ListEmpCareerMove.Add(Obj);
            }
            ListEmpCareerMove = ListEmpCareerMove.OrderByDescending(w => w.EffectDate).ToList();
            OnLoandindEmployee();
        }
        public void OnCreatingLoading(string ID)
        {
            ClsFilterStaff filterStaff = new ClsFilterStaff();
            Header = new HREmpCareerMovement();
            ListAddEmp = new List<HRStaffProfile>();
            ListAddUManager = new List<HREmpUnderManager>();
            ListUnderManager = new List<HREmpUnderManager>();
            ListEmpByDept = new List<HRStaffProfile>();
            Header.Status = SYDocumentStatus.OPEN.ToString();
            //Header.EffectDate = DateTime.Now;
            var staffPro = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == ID);
            if (staffPro != null)
            {
                Header.EmpCode = staffPro.EmpCode;
                Header.EmployeeName = staffPro.AllName;
                Header.CompanyCode = staffPro.CompanyCode;
                Header.Division = staffPro.Division;
                Header.BusinessUnit = staffPro.GroupDept;
                Header.BranchCode = staffPro.Branch;
                Header.DepartmentCode = staffPro.DEPT;
                Header.Office = staffPro.Office;
                Header.Groups = staffPro.Groups;
                Header.Section = staffPro.SECT;
                Header.PositionCode = staffPro.JobCode;
                Header.JobLevel = staffPro.LevelCode;
                Header.JobGrade = staffPro.JobGrade;
                Header.Category = staffPro.CATE;
                Header.EmployeeType = staffPro.EmpType;
                Header.Location = staffPro.LOCT;
                Header.StaffType = staffPro.StaffType;
                Header.TeleGroup = staffPro.TeleGroup;
                OldSalary = staffPro.Salary.ToString();
                NewSalary = staffPro.Salary.ToString();

                Header.NewCompanyCode = Header.CompanyCode;
                Header.NewBranch = Header.BranchCode;
                Header.NewJobLevel = Header.JobLevel;

                Header.HOD = staffPro.HODCode;
                Header.FirstLine = staffPro.FirstLine;
                Header.FirstLine2 = staffPro.FirstLine2;
                Header.SecondLine = staffPro.SecondLine;
                Header.SecondLine2 = staffPro.SecondLine2;
                Header.OTFirstLine = staffPro.OTFirstLine;
                Header.OTSecondLine = staffPro.OTSecondLine;
                Header.OTthirdLine = staffPro.OTthirdLine;
                Header.APPAppraisal = staffPro.APPAppraisal;
                Header.APPAppraisal2 = staffPro.APPAppraisal2;
                Header.APPEvaluator = staffPro.APPEvaluator;
                Header.APPTracking = staffPro.APPTracking;
            }

            var ListStaff = filterStaff.OnLoandUnderManager(ID);
            foreach (var staff in ListStaff.OrderBy(w => w.DocumentType))
            {
                var objUM = new HREmpUnderManager();
                objUM.EmpCode = staff.EmpCode;
                objUM.EmployeeName = staff.EmployeeName;
                objUM.Position = staff.Position;
                objUM.DocumentType = staff.DocumentType;
                objUM.ChangedTo = Header.EmpCode;
                ListUnderManager.Add(objUM);
            }
            DateTime dateTime = DateTime.Now;
            string Closed = SYDocumentStatus.CLOSED.ToString();
            string Completed = SYDocumentStatus.COMPLETED.ToString();
            //Performance 
            var obj = unitOfWork.Repository<HREmpAppraisal>().Queryable().FirstOrDefault(w => w.EmpCode == ID && w.InYear == dateTime.Year
            && (w.Status != Closed || w.Status != Completed));
            if (obj != null)
            {
                Header.PerformanceID = obj.ApprID;
                Header.AppraisalType = obj.AppraisalType;
                Header.KPIType = obj.KPIType;
            }
            var objkPI = unitOfWork.Repository<HRKPIAssignHeader>().Queryable().FirstOrDefault(w => w.HandleCode == ID && w.InYear == dateTime.Year
           && (w.Status != Closed || w.Status != Completed));
            if (objkPI != null)
            {
                Header.KPI = objkPI.AssignCode;
            }
            ValidateSalry(Header);
        }
        public string ValidateData()
        {
            if (string.IsNullOrEmpty(Header.CareerCode))
            {
                return "CAREERCODE_EN";
            }
            if (Header.EffectDate == new DateTime(0001, 1, 1))
            {
                return "INVALIT_EFFECT_DATE";
            }
            if (ListUnderManager.Count > 0 && !Header.SubordEffectDate.HasValue)
            {
                return "IVALID_SubordEffectDate";
            }
            if(Header.EffectDate< Header.SubordEffectDate.Value)
            {
                return "IVALID_EFFECT_DATE_SubordEffectDate";
            }
            return SYConstant.OK;
        }
        public string Create()
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var msm = ValidateData();
                if (msm != SYConstant.OK)
                {
                    return msm;
                }
                ClsEmpValidate empValidate = new ClsEmpValidate();
                msm = empValidate.Validate_Car_Move(Header.EmpCode, Header.EffectDate.Date);
                if (msm != SYConstant.OK)
                {
                    return msm;
                }
                msm = empValidate.Validate_EffectDate(Header.EmpCode, Header.EffectDate);
                if (msm != SYConstant.OK)
                {
                    return msm;
                }
                msm = empValidate.Validate_CareerType(Header.CareerCode, Header.Resigntype);
                if (msm != SYConstant.OK)
                {
                    return msm;
                }
                if (empValidate.IsNotCalSalary)
                {
                    Validate_Resign(Header);
                }
                bool IsSalary = empValidate.IsHideSalary(Header.JobLevel, User.UserName);
                if (IsSalary == true)
                {
                    Header.OldSalary = Convert.ToDecimal(OldSalary);
                    Header.NewSalary = Convert.ToDecimal(NewSalary);
                    Header.Increase = Convert.ToDecimal(Increase);
                }
                else
                {
                    HRStaffProfile objMatchHeader = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Header.EmpCode);

                    Header.OldSalary = objMatchHeader.Salary;
                    Header.NewSalary = objMatchHeader.Salary;
                    Header.Increase = 0;
                }
                Header.NewPosition = empValidate.GetPosition(Header.NewPosition);
                Header.PositionID = empValidate.PositionID;
                Get_Description(Header);
                string Status = SYDocumentStatus.OPEN.ToString();
                Header.CreatedOn = DateTime.Now;
                Header.CreatedBy = User.UserName;

                Header.NewPosition = empValidate.GetPosition(Header.NewPosition);
                Header.PositionID = empValidate.PositionID;

                //Update Old Career move
                var ListCarrOld = unitOfWork.Repository<HREmpCareerMovement>().Where(w => w.EmpCode == Header.EmpCode && w.Status == Status);
                foreach (var item in ListCarrOld)
                {
                    item.Status = SYDocumentStatus.CLEARED.ToString();
                    unitOfWork.Repository<HREmpCareerMovement>().Update(item);
                }
                unitOfWork.Add(Header);
                unitOfWork.Save();
                foreach (var item in ListUnderManager)
                {
                    item.ID = Header.ID;
                    unitOfWork.Add(item);
                }
                msm = UpdatePerformance(Header);
                if (msm != SYConstant.OK)
                {
                    unitOfWork.Rollback();
                    return msm;
                }
                unitOfWork.Save();
                unitOfWork.Commit();

                DateTime DateNow= DateTime.Now;
                if (Header.EffectDate.Date <= DateNow.Date)
                {
                    UpdateCareer(Header.ID);
                }
                return SYConstant.OK;
                
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Update(int ID)
        {
            OnLoad();
            try
            {
                Header.ID = ID;
                var objMatch = unitOfWork.Repository<HREmpCareerMovement>().Queryable().FirstOrDefault(w => w.ID == ID);
                if (objMatch == null)
                {
                    return "INV_DOC";
                }
                var msm = ValidateData();
                if (msm != SYConstant.OK)
                {
                    return msm;
                }
                ClsEmpValidate empValidate = new ClsEmpValidate();
                msm = empValidate.Validate_EffectDate(Header.EmpCode, Header.EffectDate, objMatch.DocReference);
                if (msm != SYConstant.OK)
                {
                    return msm;
                }
                msm = empValidate.Validate_CareerType(Header.CareerCode, Header.Resigntype);
                if (msm != SYConstant.OK)
                {
                    return msm;
                }

                bool IsSalary = empValidate.IsHideSalary(Header.JobLevel, User.UserName);
                if (IsSalary == true)
                {
                    objMatch.OldSalary = Convert.ToDecimal(OldSalary);
                    objMatch.NewSalary = Convert.ToDecimal(NewSalary);
                    objMatch.Increase = Convert.ToDecimal(Increase);
                }
                objMatch.NewCompanyCode = Header.NewCompanyCode;
                objMatch.NewDivision = Header.NewDivision;
                objMatch.NewBusinessUnit = Header.NewBusinessUnit;
                objMatch.NewBranch = Header.NewBranch;
                objMatch.NewDepartment = Header.NewDepartment;
                objMatch.NewOffice = Header.NewOffice;
                objMatch.NewGroups = Header.NewGroups;
                objMatch.NewSection = Header.NewSection;
                objMatch.NewPosition = Header.NewPosition;
                objMatch.NewJobLevel = Header.NewJobLevel;
                objMatch.NewJobGrade = Header.NewJobGrade;
                objMatch.NewEmployeeType = Header.NewEmployeeType;
                objMatch.NewCategory = Header.NewCategory;
                objMatch.NewLocation = Header.NewLocation;
                objMatch.NewStaffType = Header.NewStaffType;
                objMatch.Resigntype = Header.Resigntype;
                objMatch.Reason = Header.Reason;
                objMatch.PerformanceID = Header.PerformanceID;
                objMatch.AppraisalType = Header.AppraisalType;
                objMatch.IsClosedPA = Header.IsClosedPA;
                objMatch.KPI = Header.KPI;
                objMatch.KPIStatus = Header.KPIStatus;
                objMatch.CareerCode = Header.CareerCode;
                objMatch.EffectDate = Header.EffectDate;
                objMatch.ChangedOn = DateTime.Now;
                objMatch.CreatedBy = User.UserName;

                objMatch.HOD = Header.HOD;
                objMatch.FirstLine = Header.FirstLine;
                objMatch.FirstLine2 = Header.FirstLine2;
                objMatch.SecondLine = Header.SecondLine;
                objMatch.SecondLine2 = Header.SecondLine2;
                objMatch.OTFirstLine = Header.OTFirstLine;
                objMatch.OTSecondLine = Header.OTSecondLine;
                objMatch.OTthirdLine = Header.OTthirdLine;
                objMatch.APPAppraisal = Header.APPAppraisal;
                objMatch.APPAppraisal2 = Header.APPAppraisal2;
                objMatch.APPEvaluator = Header.APPEvaluator;
                objMatch.APPTracking = Header.APPTracking;
                objMatch.IsEvalPA = Header.IsEvalPA;
                objMatch.PAStartDate = Header.PAStartDate;
                objMatch.PADeadline1 = Header.PADeadline1;
                objMatch.PADeadline2 = Header.PADeadline2;
                objMatch.IsEvalKPI = Header.IsEvalKPI;
                objMatch.KPIStartDate = Header.KPIStartDate;
                objMatch.KPIDeadline = Header.KPIDeadline;
                objMatch.KPIType = Header.KPIType;
                objMatch.TeleGroup = Header.TeleGroup;

                objMatch.NewPosition = empValidate.GetPosition(Header.NewPosition);
                objMatch.PositionID = empValidate.PositionID;
                Get_Description(objMatch);

                unitOfWork.Update(objMatch);
                var objMatchitem = unitOfWork.Repository<HREmpUnderManager>().Queryable().Where(w => w.ID == ID);
                foreach (var item in ListUnderManager)
                {
                    item.ID = objMatch.ID;
                    var existingItem = objMatchitem.FirstOrDefault(x => x.ID == item.ID);
                    if (existingItem != null)
                    {
                        existingItem.ChangedTo = item.ChangedTo;
                        unitOfWork.Update(existingItem);
                    }
                    else
                    {
                        unitOfWork.Add(item);
                    }
                }
                msm = UpdatePerformance(objMatch);
                if (msm != SYConstant.OK)
                {
                    unitOfWork.Rollback();
                    return msm;
                }
                unitOfWork.Save();
                if (!string.IsNullOrEmpty(objMatch.DocReference))
                {
                    UpdateCareer(objMatch.ID);
                }
                DateTime DateNow = DateTime.Now;
                if (Header.EffectDate.Date <= DateNow.Date)
                {
                    UpdateCareer(Header.ID);
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.EDIT.ToString(), e, true);
            }
        }
        public void OnLoandindEmployee()
        {
            ListStaff = null;
            ClsFilterStaff filterStaff = new ClsFilterStaff();
            ListStaff = filterStaff.OnLoandingEmployee();
        }

        public virtual void OnDetailLoading(params object[] keys)
        {
            int ID = Convert.ToInt32(keys[0]);
            IsInAtive = false;
            Header = unitOfWork.Repository<HREmpCareerMovement>().Queryable().FirstOrDefault(w => w.ID == ID);
            if (Header != null)
            {
                var CarCode = unitOfWork.Repository<HRCareerHistory>().Queryable().FirstOrDefault(w => w.Code == Header.CareerCode);
                if (CarCode != null) IsInAtive = Convert.ToBoolean(CarCode.NotCalSalary);
                ValidateSalry(Header);

                if (Header.PositionID.HasValue || Header.PositionID > 0)
                {
                    ClsEmpValidate empValidate = new ClsEmpValidate();
                    var pos = empValidate.GetPositionView(Header.NewPosition);
                    Header.NewPosition = pos.ToString();
                }
            }
            ListUnderManager = unitOfWork.Repository<HREmpUnderManager>().Queryable().Where(w => w.ID == ID).ToList();
               
        }
        public virtual string OnGridModify(HREmpUnderManager MModel, string Action)
        {
            if (Action == "ADD")
            {
            }
            else if (Action == "EDIT")
            {
                var objCheck = ListUnderManager.Where(w => w.DocumentType == MModel.DocumentType && w.EmpCode == MModel.EmpCode).FirstOrDefault();
                if (objCheck != null)
                {
                    objCheck.ChangedTo = MModel.ChangedTo;
                    //ListUnderManager.Remove(objCheck);
                }
                else
                {
                    return "INV_DOC";
                }
            }
            else if (Action == "DELETE")
            {
                var objCheck = ListUnderManager.Where(w => w.DocumentType == MModel.DocumentType && w.EmpCode == MModel.EmpCode).FirstOrDefault();
                if (objCheck != null)
                {
                    ListUnderManager.Remove(objCheck);
                }
            }
            //var check = ListUnderManager.Where(w => w.DocumentType == MModel.DocumentType && w.EmpCode == MModel.EmpCode).ToList();
            //if (check.Count() == 0)
            //{
            //    ListUnderManager.Add(MModel);
            //}

            return SYConstant.OK;
        }
        public virtual string OnGridModifyAdd(string DocumentType)
        {
            ClsFilterStaff filterStaff = new ClsFilterStaff();
            foreach (var item in ListAddEmp)
            {
                if (!ListUnderManager.Where(w => w.DocumentType == DocumentType && w.EmpCode == item.EmpCode).Any())
                {
                    var objUM = new HREmpUnderManager();
                    objUM.DocumentType = DocumentType;
                    objUM.EmpCode = item.EmpCode;
                    objUM.EmployeeName = item.AllName;
                    objUM.Position = filterStaff.Get_Positon(item.JobCode);
                    objUM.ChangedTo = Header.EmpCode;
                    ListUnderManager.Add(objUM);
                }
            }
            return SYConstant.OK;
        }
        public virtual string OnGridModifyAddEmp(string EmpCode, string Action)
        {
            ListAddEmp = new List<HRStaffProfile>();
            string[] Emp = EmpCode.Split(';');
            var ListEmp = unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w => w.Status == "A");
            foreach (var Code in Emp)
            {
                var objEmp = ListEmp.FirstOrDefault(w => w.EmpCode == Code);
                if (objEmp != null)
                {
                    ListAddEmp.Add(objEmp);
                }

            }
            return SYConstant.OK;
        }
        public virtual string OnGridModifyTransfer(string EmpCode, string Transfer)
        {
            string[] Emp = EmpCode.Split(';');
            foreach (var Code in Emp)
            {
                string[] strDoc = Code.Split(',');
                var DocumentType = strDoc[0].ToString();
                var _EmpCode = strDoc[1].ToString();
                var objCheck = ListUnderManager.Where(w => w.DocumentType == DocumentType && w.EmpCode == _EmpCode).FirstOrDefault();
                if (objCheck != null)
                {
                    objCheck.ChangedTo = Transfer;
                }
            }               
            return SYConstant.OK;
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
            keyValues.Add("POSITION_SELECT", clsfStaff.LoadpositionReportings(IsDetail));
            keyValues.Add("Level_SELECT", unitOfWork.Repository<HRLevel>().Queryable().ToList());
            keyValues.Add("JOBGRADE_SELECT", unitOfWork.Repository<HRJobGrade>().Queryable().ToList());
            keyValues.Add("LOCATION_SELECT", clsFilter.LoadLocation());
            keyValues.Add("CATEGORY_SELECT", unitOfWork.Repository<HRCategory>().Queryable().ToList());
            keyValues.Add("EMPTYPE_SELECT", unitOfWork.Repository<HREmpType>().Queryable().ToList().OrderBy(w => w.Description));
            keyValues.Add("STAFFTYPE_SELECT", unitOfWork.Repository<HRWorkingType>().Queryable().ToList());
            keyValues.Add("TELEGRAM_SELECT", unitOfWork.Repository<TelegramBot>().Queryable().ToList());

            keyValues.Add("CareerHistories_SELECT", unitOfWork.Repository<HRCareerHistory>().Queryable().ToList());
            if (IsDetail)
                keyValues.Add("SEPARATE_SELECT", unitOfWork.Repository<HRTerminType>().Queryable().ToList());

            keyValues.Add("STAFF_SELECT", clsfStaff.OnLoadStaff(true));

            keyValues.Add("APPRTYPE_SELECT", unitOfWork.Repository<HRApprType>().Queryable().ToList());
            keyValues.Add("KPI_STATUS_SELECT", ClsFilterGeneral.LoadDataKPI_Status());

            keyValues.Add("LIST_GROUPKPI", unitOfWork.Set<HRKPIType>().Where(w => w.IsActive == true).ToList());
            keyValues.Add("SELECT_MANAGER", ClsFilterGeneral.LoadData_RoleMapping());
            return keyValues;
        }
        public virtual void OnLoadingEmpByDept(string Dept)
        {
            ListEmpByDept = unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w => w.DEPT == Dept && w.Status == "A").ToList();
            ListAddEmp = new List<HRStaffProfile>();
        }
        public void ValidateSalry(HREmpCareerMovement _header)
        {
            ClsEmpValidate empValidate = new ClsEmpValidate();
            bool Is_Salary = empValidate.IsHideSalary(_header.JobLevel, User.UserName);
            if (Is_Salary == true)
            {
                IsSalary = true;
                NewSalary = _header.NewSalary.ToString();
                OldSalary = _header.OldSalary.ToString();
                Increase = _header.Increase.ToString();
            }
            else
            {
                NewSalary = "#####";
                OldSalary = "#####";
                Increase = "#####";
            }
        }
        public void Get_Description(HREmpCareerMovement _header)
        {
            ClsFilterStaff clsFilterJob = new ClsFilterStaff();
            if (!string.IsNullOrEmpty(_header.NewBranch))
            {
                _header.Branch = clsFilterJob.Get_Branch(_header.NewBranch);
            }
            if (!string.IsNullOrEmpty(_header.NewDepartment))
            {
                _header.Department = clsFilterJob.Get_Department(_header.NewDepartment);
            }
            if (!string.IsNullOrEmpty(_header.NewPosition))
            {
                _header.Position = clsFilterJob.Get_Positon(_header.NewPosition);
            }
            if (!string.IsNullOrEmpty(_header.CareerCode))
            {
                _header.CareerType = clsFilterJob.Get_Career(_header.CareerCode);
            }
        }
        public void Validate_Resign(HREmpCareerMovement S)
        {
            if (!string.IsNullOrEmpty(S.NewCompanyCode)) S.NewCompanyCode = S.CompanyCode;
            if (string.IsNullOrEmpty(S.NewDivision)) S.NewDivision = S.Division;
            if (string.IsNullOrEmpty(S.NewBusinessUnit)) S.NewBusinessUnit = S.BusinessUnit;
            if (string.IsNullOrEmpty(S.NewBranch)) S.NewBranch = S.Branch;
            if (string.IsNullOrEmpty(S.NewDepartment)) S.NewDepartment = S.DepartmentCode;
            if (string.IsNullOrEmpty(S.NewOffice)) S.NewOffice = S.Office;
            if (string.IsNullOrEmpty(S.NewGroups)) S.NewGroups = S.Groups;
            if (string.IsNullOrEmpty(S.NewSection)) S.NewSection = S.Section;
            if (string.IsNullOrEmpty(S.NewPosition)) S.NewPosition = S.PositionCode;
            if (string.IsNullOrEmpty(S.NewJobLevel)) S.NewJobLevel = S.JobLevel;
            if (string.IsNullOrEmpty(S.NewJobGrade)) S.NewJobGrade = S.JobGrade;
            if (string.IsNullOrEmpty(S.NewEmployeeType)) S.NewEmployeeType = S.EmployeeType;
            if (string.IsNullOrEmpty(S.NewCategory)) S.NewCategory = S.Category;
            if (string.IsNullOrEmpty(S.NewLocation)) S.NewLocation = S.Location;
            if (string.IsNullOrEmpty(S.NewStaffType)) S.NewStaffType = S.StaffType;
        }

        public void ListUpdateCareer()
        {
            string Open = SYDocumentStatus.OPEN.ToString();
            DateTime DateNow = DateTime.Now.Date;
            var lstCarMove = unitOfWork.Repository<HREmpCareerMovement>().Queryable().
                Where(w => w.Status == Open && string.IsNullOrEmpty(w.DocReference)).ToList();
                //&& w.EffectDate <= DateNow);
            foreach (var Movement in lstCarMove)
            {
                UpdateCareer(Movement.ID);
            }
        }
        public string UpdateCareer(int ID)
        {
            OnLoad();
            ClsFilterStaff clsFilterJob = new ClsFilterStaff();
            unitOfWork.BeginTransaction();
            string Open = SYDocumentStatus.OPEN.ToString();
            DateTime DateNow = DateTime.Now.Date;
            var objMatch = unitOfWork.Repository<HREmpCareerMovement>().FirstOrDefault(w => w.ID == ID);
            if (objMatch.EffectDate <= DateNow)
            {
                try
                {
                    bool IsResign = false;
                    bool IsEvalPA = objMatch.IsEvalPA.Value;
                    bool IsEvalKPI = objMatch.IsEvalKPI.Value;
                    ClsEmpValidate empValidate = new ClsEmpValidate();
                    var msm = empValidate.Validate_CareerType(objMatch.CareerCode, objMatch.Resigntype);
                    if (msm == SYConstant.OK)
                    {
                        IsResign = empValidate.IsNotCalSalary;
                    }
                    else
                    {
                        IsResign = objMatch.IsClosedPA.Value;
                    }
                    if (!string.IsNullOrEmpty(objMatch.PerformanceID))
                    {
                        string Pendin = SYDocumentStatus.PENDING.ToString();
                        var ObjPA = unitOfWork.Repository<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == objMatch.PerformanceID && w.ReStatus == Pendin);
                        if (ObjPA != null)
                        {
                            bool IsClose = false;
                            if (IsResign == true)
                            {
                                if (objMatch.IsEvalPA != true)
                                {
                                    ObjPA.Status = SYDocumentStatus.CLOSED.ToString();
                                    ObjPA.ReStatus = SYDocumentStatus.CLOSED.ToString();
                                    ObjPA.KPIStatus = SYDocumentStatus.CLOSED.ToString();
                                    if (!string.IsNullOrEmpty(ObjPA.KPIReference))
                                    {
                                        IsClose = true;
                                    }
                                }
                            }
                            else
                            {
                                var ListAPPItem = unitOfWork.Repository<HREmpAppraisalItem>().Queryable().Where(w => w.ApprID == ObjPA.ApprID);
                                foreach (var item in ListAPPItem)
                                {
                                    unitOfWork.Delete(item);
                                }
                                var ListAPPSummary = unitOfWork.Repository<HREmpAppraisalSummary>().Queryable().Where(w => w.AppraisalNo == ObjPA.ApprID);
                                foreach (var item in ListAPPSummary)
                                {
                                    unitOfWork.Delete(item);
                                }
                                ObjPA.AppraisalType = objMatch.AppraisalType;
                                ObjPA.Department = objMatch.Department;
                                ObjPA.Position = objMatch.Position;
                                ObjPA.KPIType = objMatch.KPIType;
                                if (ObjPA.DirectedByCode != objMatch.HOD)
                                {
                                    ObjPA.DirectedByCode = objMatch.HOD;
                                    var StaffPlan = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == objMatch.HOD);
                                    if (StaffPlan != null)
                                    {
                                        ObjPA.DirectedByName = StaffPlan.AllName;
                                    }
                                }
                                if (ObjPA.AppraiserCode != objMatch.APPAppraisal)
                                {
                                    ObjPA.AppraiserCode = objMatch.APPAppraisal;
                                    var StaffPlan = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == objMatch.APPAppraisal);
                                    if (StaffPlan != null)
                                    {
                                        ObjPA.AppraiserName = StaffPlan.AllName;
                                        ObjPA.AppraiserPosition = clsFilterJob.Get_Positon(StaffPlan.JobCode);
                                    }
                                }
                                if (ObjPA.AppraiserCode2 != objMatch.APPAppraisal2)
                                {
                                    ObjPA.AppraiserCode2 = objMatch.APPAppraisal2;
                                    var StaffPlan = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == objMatch.APPAppraisal2);
                                    if (StaffPlan != null)
                                    {
                                        ObjPA.AppraiserName2 = StaffPlan.AllName;
                                        ObjPA.AppraiserPosition2 = clsFilterJob.Get_Positon(StaffPlan.JobCode);
                                    }
                                }
                                var _ListRegion = unitOfWork.Repository<HRApprRegion>().Queryable().Where(w => w.AppraiselType == ObjPA.AppraisalType).ToList();
                                var quiz = unitOfWork.Set<HRApprFactor>().Where(w => w.AppraiselType == ObjPA.AppraisalType).ToList();
                                var lstappr = quiz.Where(w => _ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
                                var objCF = unitOfWork.Set<ExCfWorkFlowItem>().FirstOrDefault(w => w.ScreenID == ScreenId);
                                var ListEmployee = unitOfWork.Set<HRStaffProfile>().Where(w => w.Status == "A").ToList();
                                if (lstappr.Count() > 0)
                                {
                                    ClsPerformance performance = new ClsPerformance();
                                    foreach (var item in lstappr)
                                    {
                                        var obj = new HREmpAppraisalItem();
                                        performance.NewAppraisalItem(obj, _ListRegion, item, ObjPA.ApprID);
                                        unitOfWork.Add(obj);
                                    }
                                    foreach (var item in _ListRegion)
                                    {
                                        var AppSum = new HREmpAppraisalSummary();
                                        performance.NewAppraisalSummary(AppSum, item, ObjPA.ApprID, ObjPA.AppraisalType);
                                        unitOfWork.Add(AppSum);
                                    }
                                }
                            }
                            unitOfWork.Repository<HREmpAppraisal>().Update(ObjPA);


                            if (!string.IsNullOrEmpty(ObjPA.KPIReference) && !string.IsNullOrEmpty(objMatch.KPIStatus))
                            {
                                var objKPI = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == ObjPA.KPIReference);
                                if (objKPI != null && objMatch.KPIStatus == "CLOSED")
                                {
                                    IsClose = true;
                                }
                            }
                            if (!string.IsNullOrEmpty(ObjPA.KPIReference))
                            {
                                if (IsClose)
                                {
                                    if (objMatch.IsEvalKPI != true)
                                    {
                                        var objKPI = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == ObjPA.KPIReference);
                                        if (objKPI != null)
                                        {
                                            objKPI.Status = SYDocumentStatus.CLOSED.ToString();
                                            objKPI.ReStatus = SYDocumentStatus.CLOSED.ToString();
                                            unitOfWork.Repository<HRKPIAssignHeader>().Update(objKPI);

                                            string Approval = SYDocumentStatus.APPROVED.ToString();
                                            var ListKPI = Get_DataTracking(objKPI.AssignCode);
                                            foreach (var item in ListKPI)
                                            {
                                                item.Status = Approval;
                                                unitOfWork.Repository<HRKPITracking>().Update(item);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var objKPI = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == ObjPA.KPIReference);
                                    if (objKPI != null)
                                    {
                                        objKPI.KPIType = objMatch.KPIType;
                                        objKPI.Department = objMatch.Department;
                                        objKPI.Position = objMatch.Position;
                                        objKPI.CriteriaType = objMatch.NewDepartment;
                                        var KPI = unitOfWork.Repository<HRKPIType>().FirstOrDefault(w => w.Code == objKPI.KPIType);
                                        if (KPI != null) objKPI.KPIAverage = KPI.KPIAverage;
                                        if (objMatch.KPIStatus == "CHANGED")
                                        {
                                            objKPI.Status = SYDocumentStatus.OPEN.ToString();
                                            objKPI.ReStatus = SYDocumentStatus.OPEN.ToString();
                                        }
                                        if (objKPI.DirectedByCode == objMatch.HOD)
                                        {
                                            objKPI.DirectedByCode = objMatch.HOD;
                                            var StaffPlan = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == objMatch.HOD);
                                            if (StaffPlan != null)
                                            {
                                                objKPI.DirectedByName = StaffPlan.AllName;
                                            }
                                        }
                                        if (objKPI.PlanerCode != objMatch.APPAppraisal)
                                        {
                                            objKPI.PlanerCode = objMatch.APPAppraisal;
                                            var StaffPlan = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == objMatch.APPAppraisal);
                                            if (StaffPlan != null)
                                            {
                                                objKPI.PlanerName = StaffPlan.AllName;
                                                objKPI.PlanerPosition = clsFilterJob.Get_Positon(StaffPlan.JobCode);
                                            }
                                        }
                                        unitOfWork.Repository<HRKPIAssignHeader>().Update(objKPI);
                                    }
                                }
                            }
                        }
                    }
                    ClsSwapData swapData = new ClsSwapData();
                    ClsEmployee employee1 = new ClsEmployee();
                    employee1.HeaderStaff = new HR_STAFF_VIEW();
                    employee1.HeaderCareer = new HREmpCareer();
                    employee1.HeaderStaff.EmpCode = objMatch.EmpCode;
                    employee1.CareerMoveID = objMatch.ID.ToString();
                    swapData.SwapCar_Movement(employee1.HeaderCareer, objMatch);
                    string sms = "";
                    if (!string.IsNullOrEmpty(objMatch.DocReference))
                    {
                        sms = employee1.EditCareerStaff(objMatch.DocReference);
                    }
                    else
                    {
                        sms = employee1.CreateCareerStaff(objMatch);
                    }
                    if (sms != SYConstant.OK)
                    {
                        return "";
                    }
                    objMatch.DocReference = employee1.HeaderCareer.TranNo.ToString();
                    
                    unitOfWork.Update(objMatch);
                    unitOfWork.Save();
                    unitOfWork.Commit();
                    UpdateStaff(objMatch);
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                }
            }
            if (objMatch.SubordEffectDate.HasValue && objMatch.SubordEffectDate <= DateNow)
            {
                OnLoad();
                ClsEmployeeCondition employee = new ClsEmployeeCondition();
                var ListEmpUnder = unitOfWork.Repository<HREmpUnderManager>().Queryable().Where(w => w.ID == objMatch.ID);
                foreach (var empUnder in ListEmpUnder)
                {
                    var ObjStaff = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == empUnder.EmpCode);
                    if (ObjStaff != null)
                    {
                        var updateMap = employee.BuildUpdateMap(empUnder.ChangedTo, ObjStaff);
                        if (updateMap.TryGetValue(empUnder.DocumentType, out var update))
                        {
                            update();
                            unitOfWork.Repository<HRStaffProfile>().Update(ObjStaff);

                            if (empUnder.DocumentType == "Head of Department")
                            {
                                var ObjApp = unitOfWork.Repository<HREmpAppraisal>().FirstOrDefault(w => w.EmpCode == ObjStaff.EmpCode && w.InYear == objMatch.EffectDate.Year);
                                if (ObjApp != null && ObjApp.DirectedByCode == objMatch.EmpCode)
                                {
                                    ObjApp.DirectedByCode = empUnder.ChangedTo;
                                    var StaffPlan = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == empUnder.ChangedTo);
                                    if (StaffPlan != null)
                                    {
                                        ObjApp.DirectedByName = StaffPlan.AllName;
                                    }
                                    unitOfWork.Repository<HREmpAppraisal>().Update(ObjApp);
                                }
                            }
                            if (empUnder.DocumentType == "Appraisal 1")
                            {
                                var ObjApp = unitOfWork.Repository<HREmpAppraisal>().FirstOrDefault(w => w.EmpCode == ObjStaff.EmpCode && w.InYear == objMatch.EffectDate.Year);
                                if (ObjApp != null && ObjApp.AppraiserCode == objMatch.EmpCode)
                                {
                                    ObjApp.AppraiserCode = empUnder.ChangedTo;
                                    var StaffPlan = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == empUnder.ChangedTo);
                                    if (StaffPlan != null)
                                    {
                                        ObjApp.AppraiserName = StaffPlan.AllName;
                                        ObjApp.AppraiserPosition = clsFilterJob.Get_Positon(StaffPlan.JobCode);
                                    }
                                    unitOfWork.Repository<HREmpAppraisal>().Update(ObjApp);
                                }
                            }
                            if (empUnder.DocumentType == "Appraisal 2")
                            {
                                var ObjApp = unitOfWork.Repository<HREmpAppraisal>().FirstOrDefault(w => w.EmpCode == ObjStaff.EmpCode && w.InYear == objMatch.EffectDate.Year);
                                if (ObjApp != null && ObjApp.AppraiserCode2 == objMatch.EmpCode)
                                {
                                    ObjApp.AppraiserCode2 = empUnder.ChangedTo;
                                    var StaffPlan = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == empUnder.ChangedTo);
                                    if (StaffPlan != null)
                                    {
                                        ObjApp.AppraiserName2 = StaffPlan.AllName;
                                        ObjApp.AppraiserPosition2 = clsFilterJob.Get_Positon(StaffPlan.JobCode);
                                    }
                                    unitOfWork.Repository<HREmpAppraisal>().Update(ObjApp);
                                }
                            }
                            if (empUnder.DocumentType == "Evaluator")
                            {
                                var ObjKPI = unitOfWork.Repository<HRKPIAssignHeader>().FirstOrDefault(w => w.HandleCode == ObjStaff.EmpCode && w.InYear == objMatch.EffectDate.Year);
                                if (ObjKPI != null && ObjKPI.DirectedByCode == objMatch.EmpCode)
                                {
                                    ObjKPI.DirectedByCode = empUnder.ChangedTo;
                                    var StaffPlan = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == empUnder.ChangedTo);
                                    if (StaffPlan != null)
                                    {
                                        ObjKPI.DirectedByName = StaffPlan.AllName;
                                    }
                                    unitOfWork.Repository<HRKPIAssignHeader>().Update(ObjKPI);
                                }
                            }
                            if (empUnder.DocumentType == "Tracking")
                            {
                                var ObjKPI = unitOfWork.Repository<HRKPIAssignHeader>().FirstOrDefault(w => w.HandleCode == ObjStaff.EmpCode && w.InYear == objMatch.EffectDate.Year);
                                if (ObjKPI != null && ObjKPI.PlanerCode == objMatch.EmpCode)
                                {
                                    ObjKPI.PlanerCode = empUnder.ChangedTo;
                                    var StaffPlan = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == empUnder.ChangedTo);
                                    if (StaffPlan != null)
                                    {
                                        ObjKPI.PlanerName = StaffPlan.AllName;
                                        ObjKPI.PlanerPosition = clsFilterJob.Get_Positon(StaffPlan.JobCode);

                                        var ListKPI = Get_DataTracking(ObjKPI.AssignCode);
                                        foreach (var item in ListKPI)
                                        {
                                            item.DirectedByCode = ObjKPI.PlanerCode;
                                            unitOfWork.Repository<HRKPITracking>().Update(item);
                                        }
                                    }
                                    unitOfWork.Repository<HRKPIAssignHeader>().Update(ObjKPI);
                                }
                            }
                        }
                    }
                }
                if (ListEmpUnder.Count() > 0)
                    unitOfWork.Save();
            }
            return SYConstant.OK;
        }
        public void UpdateStaff(HREmpCareerMovement objMatch)
        {
            OnLoad();
            var StaffPro = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
            if (StaffPro != null)
            {
                StaffPro.HODCode = objMatch.HOD;
                StaffPro.FirstLine = objMatch.FirstLine;
                StaffPro.FirstLine2 = objMatch.FirstLine2;
                StaffPro.SecondLine = objMatch.SecondLine;
                StaffPro.SecondLine2 = objMatch.SecondLine2;
                StaffPro.OTFirstLine = objMatch.OTFirstLine;
                StaffPro.OTSecondLine = objMatch.OTSecondLine;
                StaffPro.OTthirdLine = objMatch.OTthirdLine;
                StaffPro.APPAppraisal = objMatch.APPAppraisal;
                StaffPro.APPAppraisal2 = objMatch.APPAppraisal2;
                StaffPro.APPEvaluator = objMatch.APPEvaluator;
                StaffPro.APPTracking = objMatch.APPTracking;
                StaffPro.TeleGroup = objMatch.TeleGroup;
                unitOfWork.Repository<HRStaffProfile>().Update(StaffPro);
                unitOfWork.Save();
            }
        }

        public List<HRKPITracking> Get_DataTracking(string AssignCode)
        {
            string Open = SYDocumentStatus.PENDING.ToString();
            string Approval = SYDocumentStatus.APPROVED.ToString();
            var ListKPI = unitOfWork.Repository<HRKPITracking>().Queryable().Where(w => w.AssignCode == AssignCode && w.Status == Open);
            //ListKPI.ToList().ForEach(w => w.Status = Approval);

            return ListKPI.ToList();
        }

        public string UpdatePerformance(HREmpCareerMovement objMatch)
        {
            var objPA = unitOfWork.Repository<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == objMatch.PerformanceID);
            if (objPA != null && objMatch.IsEvalPA == true)
            { 
                if(!objMatch.PAStartDate.HasValue && !objMatch.PADeadline1.HasValue && !objMatch.PADeadline2.HasValue)
                {
                    return "PAStartDate_REQ";
                }
                if(objMatch.IsEvalKPI == true && objMatch.KPIDeadline >= objMatch.PAStartDate)
                {
                    return "IN_VALIDATE_KPI_Deadline";
                }
                if(objMatch.PAStartDate>= objMatch.PADeadline1)
                {
                    return "IN_VALIDATE_PA_StartDate";
                }
                if (objMatch.PADeadline1 >= objMatch.PADeadline2)
                {
                    return "IN_VALIDATE_PA_Deadline1";
                }
                if (objMatch.PADeadline2 > objMatch.PADeadline2)
                {
                    return "IN_VALIDATE_PA_Deadline2";
                }
                objPA.KPIExpectedDate = objMatch.KPIStartDate;
                objPA.KPIDeadline = objMatch.KPIDeadline;
                objPA.AppraiserStart = objMatch.PAStartDate.Value;
                objPA.AppraiserDeadline = objMatch.PADeadline1;
                objPA.AppraiserDeadline2 = objMatch.PADeadline2;
                unitOfWork.Repository<HREmpAppraisal>().Update(objPA);
            }
            ClsKPICalculate KPICa = new ClsKPICalculate();
            var objKPI = unitOfWork.Repository<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == objMatch.KPI);
            if (objKPI != null && objMatch.IsEvalKPI == true)
            {
                if (!objMatch.KPIDeadline.HasValue)
                {
                    return "KPIStartDate_REQ";
                }
                if(objMatch.KPIStartDate >= objMatch.KPIDeadline)
                {
                    return "IN_VALIDATE_KPI_DATE";
                }
                objKPI = KPICa.Update_Start_PA(unitOfWork, objMatch.KPI, objMatch.KPIStartDate.Value);

                objKPI.ExpectedDate = objMatch.KPIStartDate;
                objKPI.Deadline = objMatch.KPIDeadline;
                string Approval = SYDocumentStatus.APPROVED.ToString();
                var ListKPI = Get_DataTracking(objKPI.AssignCode);
                foreach (var item in ListKPI)
                {
                    item.Status = Approval;
                    unitOfWork.Repository<HRKPITracking>().Update(item);
                }
                unitOfWork.Repository<HRKPIAssignHeader>().Update(objKPI);
            }
            return SYConstant.OK;
        }
    }
}