using Humica.Core;
using Humica.Core.CF;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Training
{
    public class ClsEFEmpPortal : IClsEFEmpPortal
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public FTFilterReport Filter { get; set; }
        public EFEmpPortal Header { get; set; }
        public List<EFEmpPortal> ListHeader { get; set; }
        public List<EFEmpPortal> ListAssessmentPending { get; set; }
        public List<EFEmpPortalItem> ListItem { get; set; }
        public List<HRApprSelfAssessment> ListSelfAssItem { get; set; }
        public List<HRApprSelfAssQCM> ListSelfAssQCM { get; set; }
        public List<ClsStaff> ListEmpStaff { get; set; }

        public ClsEFEmpPortal()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        public void OnIndexLoading(bool IsESS = false)
        {
            if (IsESS)
            {
                string Open = SYDocumentStatus.OPEN.ToString();
                ListHeader = unitOfWork.Set<EFEmpPortal>().Where(w => w.EmpCode == User.UserName
                && w.Status != Open).ToList();
            }
            else
            {
                ListHeader = unitOfWork.Set<EFEmpPortal>().ToList();
            }
        }
        public void OnIndexLoadingPending()
        {
            string pending = SYDocumentStatus.PENDING.ToString();
            string userName = User.UserName;
            ListAssessmentPending = unitOfWork.Set<EFEmpPortal>().Where(x => x.EmpCode == userName && x.Status == pending
            && x.IsRead != true).ToList();
        }
        public void OnIndexLoadingTeam()
        {
            DateTime InDate = DateTime.Now;
            Header = new EFEmpPortal();
            Header.DocumentDate = DateTime.Now;
            Header.ExpectedDate = DateTime.Now;
            Header.Deadline = DateTime.Now;
            ListEmpStaff = new List<ClsStaff>();
        }
        public void LoadData(FTFilterReport Filter1, List<HRBranch> _lstBranch)
        {
            OnLoad();
            DateTime date = new DateTime(1900, 1, 1);
            ListEmpStaff = new List<ClsStaff>();
            var positions = unitOfWork.Set<HRPosition>().ToList();
            var Department = unitOfWork.Set<HRDepartment>().ToList();
            var Section = unitOfWork.Set<HRSection>().ToList();
            var _staff = unitOfWork.Set<HRStaffProfile>().Where(w => w.Status == "A"
                           && (string.IsNullOrEmpty(Filter1.Branch) || w.Branch == Filter1.Branch)
                           && (string.IsNullOrEmpty(Filter1.Division) || w.Division == Filter1.Division)
                           && (string.IsNullOrEmpty(Filter1.BusinessUnit) || w.GroupDept == Filter1.BusinessUnit)
                           && (string.IsNullOrEmpty(Filter1.Department) || w.DEPT == Filter1.Department)
                           && (string.IsNullOrEmpty(Filter1.Office) || w.Office == Filter1.Office)
                           && (string.IsNullOrEmpty(Filter1.Section) || w.SECT == Filter1.Section)
                           && (string.IsNullOrEmpty(Filter1.Group) || w.Groups == Filter1.Group)
                           && (string.IsNullOrEmpty(Filter1.Position) || w.JobCode == Filter1.Position)
                           && (string.IsNullOrEmpty(Filter1.Level) || w.LevelCode == Filter1.Level)
                           //&& (string.IsNullOrEmpty(Filter1.Category) || w.CATE == Filter1.Category)
                           ).ToList();

            foreach (var item in _staff)
            {
                var s = _staff.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                var EmpStaff = new ClsStaff();
                EmpStaff.EmpCode = item.EmpCode;
                EmpStaff.EmployeeName = item.OthAllName;
                EmpStaff.EmployeeName2 = item.AllName;
                if (_lstBranch.Where(w => w.Code == item.Branch).Any())
                    EmpStaff.Branch = _lstBranch.FirstOrDefault(w => w.Code == item.Branch).Description;
                if (Department.Where(w => w.Code == item.DEPT).Any())
                    EmpStaff.Department = Department.FirstOrDefault(w => w.Code == item.DEPT).Description;
                if (positions.Where(w => w.Code == item.JobCode).Any())
                    EmpStaff.Position = positions.FirstOrDefault(w => w.Code == item.JobCode).Description;
                if (Section.Where(w => w.Code == item.SECT).Any())
                    EmpStaff.Section = Section.FirstOrDefault(w => w.Code == item.SECT).Description;
                EmpStaff.Sex = item.Sex;
                ListEmpStaff.Add(EmpStaff);
            }
        }
        public void OnCreatingLoading(params object[] keys)
        {
            string PortalType = (string)keys[1];
            string EmpCode = (string)keys[0];
            Header = new EFEmpPortal();
            Header.DocumentDate = DateTime.Now;
            Header.ExpectedDate = DateTime.Now;
            Header.Deadline = DateTime.Now;
            Header.PortalType = PortalType;
            var Staff = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (Staff != null)
            {
                Header.EmpCode = EmpCode;
                Header.EmployeeName = Staff.AllName;
                Header.Department = Staff.Department;
                Header.Position = Staff.Position;
            }
            ListItem = new List<EFEmpPortalItem>();
            ListSelfAssQCM = new List<HRApprSelfAssQCM>();
            ListSelfAssItem = new List<HRApprSelfAssessment>();
            var objAppType = unitOfWork.Set<EFPortalType>().FirstOrDefault(w => w.Code == PortalType);
            if (objAppType != null)
            {
                ListSelfAssItem = unitOfWork.Set<HRApprSelfAssessment>().Where(w => w.PortalCode == PortalType).ToList();
                ListSelfAssQCM = unitOfWork.Set<HRApprSelfAssQCM>().Where(w => w.PortalCode == PortalType).ToList();
            }

        }
        public string Create()
        {
            OnLoad();
            try
            {
                var lstAssass = unitOfWork.Set<HRApprSelfAssessment>().Where(w => w.PortalCode == Header.PortalType).ToList();
                var lstAssassQCM = unitOfWork.Set<HRApprSelfAssQCM>().Where(w => w.PortalCode == Header.PortalType).ToList();
                var objCF = unitOfWork.Set<ExCfWorkFlowItem>().FirstOrDefault(w => w.ScreenID == ScreenId);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                var AppType = unitOfWork.Set<HRApprType>().FirstOrDefault(w => w.Code == Header.PortalType);
                if (AppType != null)
                {
                    Header.PortalName = AppType.Description;
                }
                var objNumber = new CFNumberRank(objCF.DocType, objCF.ScreenID);
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.PortalNo = objNumber.NextNumberRank;
                foreach (var item in lstAssass)
                {
                    var obj = new EFEmpPortalItem();
                    obj.PortalNo = Header.PortalNo;
                    var mess = Swap_Item(obj, item, ListItem, lstAssassQCM);
                    if (mess != SYConstant.OK)
                    {
                        return mess;
                    }
                    unitOfWork.Add(obj);
                }
                Header.Status = SYDocumentStatus.OPEN.ToString();
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
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
        }
        public string CreateMulti(string EmpCode, EFEmpPortal Obj)
        {
            ScreenId = "HRA0000006";
            string ErrorCode = "";
            try
            {
                if (string.IsNullOrEmpty(Obj.PortalType))
                {
                    return "EE_DOCTYPE";
                }
                var PortalType = unitOfWork.Set<EFPortalType>().FirstOrDefault(w => w.Code == Obj.PortalType);
                Obj.PortalName = PortalType.Description;
                var _ListQuestion = unitOfWork.Set<HRApprSelfAssessment>().Where(w => w.PortalCode == Obj.PortalType).ToList();
                var ListQcm = unitOfWork.Set<HRApprSelfAssQCM>().Where(w => w.PortalCode == Obj.PortalType).ToList();
                var objCF = unitOfWork.Set<ExCfWorkFlowItem>().FirstOrDefault(w => w.ScreenID == ScreenId);
                var ListEmployee = unitOfWork.Set<HRStaffProfile>().Where(w => w.Status == "A").ToList();
                if (_ListQuestion.Count() == 0)
                {
                    return "INVALID_QUESTION";
                }
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                string[] Emp = EmpCode.Split(';');
                ClsFilterStaff FTSTaff = new ClsFilterStaff();
                foreach (var Code in Emp)
                {
                    if (Code.Trim() == "") continue;
                    Header = new EFEmpPortal();
                    Header.PortalType = Obj.PortalType;
                    Header.PortalName = Obj.PortalName;
                    Header.EmpCode = Obj.EmpCode;
                    Header.DocumentDate = Obj.DocumentDate;
                    Header.Deadline = Obj.Deadline;
                    Header.SurveyName = Obj.SurveyName;
                    Header.Status = SYDocumentStatus.PENDING.ToString();
                    Header.IsRead = false;
                    ErrorCode = Code;
                    Header.EmpCode = Code;
                    var EmpStaff = ListEmployee.FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                    var objNumber = new CFNumberRank(objCF.NumberRank, EmpStaff.CompanyCode, Header.DocumentDate.Year, true);

                    if (objNumber.NextNumberRank == null)
                    {
                        return "NUMBER_RANK_NE";
                    }
                    Header.PortalNo = objNumber.NextNumberRank;
                    if (EmpStaff != null)
                    {
                        Header.Position = FTSTaff.Get_Positon(EmpStaff.JobCode);
                        Header.EmployeeName = EmpStaff.AllName;
                        Header.Department = FTSTaff.Get_Department(EmpStaff.DEPT);
                        foreach (var item in _ListQuestion)
                        {
                            var obj = new EFEmpPortalItem();
                            obj.PortalNo = Header.PortalNo;
                            obj.QuestionCode = item.QuestionCode;
                            obj.Description1 = item.Description1;
                            obj.IsQCM = item.IsQCM;
                            unitOfWork.Add(obj);
                        }
                        Header.CreatedBy = User.UserName;
                        Header.CreatedOn = DateTime.Now;
                        unitOfWork.Add(Header);
                    }
                }
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, ErrorCode, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, ErrorCode, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public virtual string OnEditLoading(params object[] keys)
        {
            string PortalNo = (string)keys[0];
            Header = unitOfWork.Set<EFEmpPortal>().FirstOrDefault(w => w.PortalNo == PortalNo);
            ListItem = new List<EFEmpPortalItem>();
            ListSelfAssQCM = new List<HRApprSelfAssQCM>();
            if (Header != null)
            {
                ListSelfAssItem = unitOfWork.Set<HRApprSelfAssessment>().Where(w => w.PortalCode == Header.PortalType).ToList();
                ListItem = unitOfWork.Set<EFEmpPortalItem>().Where(w => w.PortalNo == PortalNo).ToList();
                ListSelfAssQCM = unitOfWork.Set<HRApprSelfAssQCM>().Where(w => w.PortalCode == Header.PortalType).ToList();
            }
            return SYConstant.OK;
        }
        public string Update(string id, bool IsESS = false)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<EFEmpPortal>().FirstOrDefault(w => w.PortalNo == id);
                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                var lstAssass = unitOfWork.Set<HRApprSelfAssessment>().Where(w => w.PortalCode == objMatch.PortalType).ToList();
                var lstAssassQCM = unitOfWork.Set<HRApprSelfAssQCM>().Where(w => w.PortalCode == Header.PortalType).ToList();
                var ObjMatchItem = unitOfWork.Set<EFEmpPortalItem>().Where(w => w.PortalNo == objMatch.PortalNo).ToList();
                foreach (var read in ObjMatchItem)
                {
                    unitOfWork.Delete(read);
                }

                foreach (var item in lstAssass)
                {
                    var obj = new EFEmpPortalItem();
                    obj.PortalNo = Header.PortalNo;
                    var mess = Swap_Item(obj, item, ListItem, lstAssassQCM);
                    if (mess != SYConstant.OK)
                    {
                        return mess;
                    }
                    unitOfWork.Add(obj);
                }
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                if (IsESS)
                {
                    objMatch.IsRead = true;
                    objMatch.Status = SYDocumentStatus.APPROVED.ToString();
                }
                unitOfWork.Update(objMatch);
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
        }
        public string Delete(string id)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<EFEmpPortal>().FirstOrDefault(w => w.PortalNo == id);
                if (objMatch == null)
                {
                    return "DOC_EN";
                }
                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "DELETE_CAN_N";
                }
                var ObjMatchItem = unitOfWork.Set<EFEmpPortalItem>().Where(w => w.PortalNo == objMatch.PortalNo).ToList();
                foreach (var read in ObjMatchItem)
                {
                    unitOfWork.Delete(read);
                }
                unitOfWork.Delete(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string RequestToApprove(string id)
        {
            try
            {
                var objMatch = unitOfWork.Set<EFEmpPortal>().FirstOrDefault(w => w.PortalNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;
                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }

                objMatch.Status = SYDocumentStatus.PENDING.ToString();
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public string CancelDoc(string id)
        {
            try
            {
                var objMatch = unitOfWork.Set<EFEmpPortal>().FirstOrDefault(w => w.PortalNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;

                objMatch.Status = SYDocumentStatus.CANCELLED.ToString();

                unitOfWork.Update(objMatch);
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                ClsEventLog.SaveEventLogs(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                return "EE001";
            }
        }
        public string ApproveDoc(string id)
        {
            try
            {
                var objMatch = unitOfWork.Set<EFEmpPortal>().FirstOrDefault(w => w.PortalNo == id);
                if (objMatch == null) return "DOC_NE";
                Header = objMatch;
                if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                {
                    return "INV_DOC";
                }

                objMatch.Status = SYDocumentStatus.APPROVED.ToString();

                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public ClsStaff GetStaffFilter(string id)
        {
            ClsFilterStaff clsfStaff = new ClsFilterStaff();
            ClsStaff Staff = clsfStaff.GetStaff(id);
            return Staff;
        }
        public Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            ClsFilterStaff clsfStaff = new ClsFilterStaff();
            keyValues.Add("STAFF_SELECT", clsfStaff.OnLoadStaff(true));
            keyValues.Add("PortalType_SELECT", unitOfWork.Set<EFPortalType>().ToList());

            return keyValues;
        }
        public Dictionary<string, dynamic> OnDataSelectorTeam(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("DEPARTMENT_SELECT", ClsFilter.LoadDepartment());
            keyValues.Add("BRANCHES_SELECT", SYConstant.getBranchDataAccess());
            keyValues.Add("SECTION_SELECT", ClsFilter.LoadSection());
            keyValues.Add("POSITION_SELECT", ClsFilter.LoadPosition());
            keyValues.Add("DIVISION_SELECT", ClsFilter.LoadDivision());
            keyValues.Add("LEVEL_SELECT", SYConstant.getLevelDataAccess());
            keyValues.Add("BusinessUnit_SELECT", unitOfWork.Set<HRGroupDepartment>().ToList());
            keyValues.Add("STAFF_SELECT", unitOfWork.Set<HRStaffProfile>().Where(w => w.Status == "A").ToList());
            keyValues.Add("OFFICE_SELECT", unitOfWork.Set<HROffice>().ToList());
            keyValues.Add("GROUP_SELECT", unitOfWork.Set<HRGroup>().ToList());
            keyValues.Add("Category_SELECT", unitOfWork.Set<HRCategory>().ToList());
            keyValues.Add("PORTTAL_TYPE_SELECT", unitOfWork.Set<EFPortalType>().ToList());

            return keyValues;
        }
        public string Swap_Item(EFEmpPortalItem D, HRApprSelfAssessment S
            , List<EFEmpPortalItem> lstItem, List<HRApprSelfAssQCM> lstAssassQCM)
        {
            D.QuestionCode = S.QuestionCode;
            D.IsQCM = S.IsQCM;
            D.Description1 = S.Description1;
            D.Description2 = S.Description2;
            if (lstItem.Where(w => w.QuestionCode == S.QuestionCode).Count() == 0
                && S.IsRequired == true)
            {
                MessageCode = D.Description1;
                return "REQUIRE";
            }
            foreach (var read in lstItem.Where(w => w.QuestionCode == S.QuestionCode).ToList())
            {
                if (S.IsRequired == true && D.IsQCM != true && string.IsNullOrEmpty(read.Comment))
                {
                    MessageCode = D.Description1;
                    return "COMMENT_IS_REQUIRED";
                }
                D.Comment = read.Comment;
                D.CorrectValue = read.CorrectValue;
                if (D.IsQCM == true)
                {
                    var objQCM = lstAssassQCM.FirstOrDefault(w => w.QuestionCode == read.QuestionCode
                    && w.LineItem.ToString() == read.CorrectValue);
                    if (objQCM != null && objQCM.IsRequiredComment == true && string.IsNullOrEmpty(read.Comment))
                    {
                        MessageCode = D.Description1;
                        return "COMMENT_IS_REQUIRED";
                    }
                }
            }
            return SYConstant.OK;
        }
    }

}//391