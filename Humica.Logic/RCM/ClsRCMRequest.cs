using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;

namespace Humica.Logic.RCM
{
    public class ClsRCMRequest : IClsRCMRequest
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string MessageError { get; set; }
        public string ScreenId { get; set; }
        public string JDDescription { get; set; }
        public RCMRecruitRequest Header { get; set; }
        public ClsEmail EmailObject { get; set; }
        public List<RCMRecruitRequest> ListHeader { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<ClsWaitingList> ListWaiting { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }
        public ExCfWFApprover ExCfWFApprover { get; set; }
        public Filters Filters { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsRCMRequest()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void ProcessLoadData(string userName)
        {
            OnLoad();
            string pending = SYDocumentStatus.PENDING.ToString();
            string open = SYDocumentStatus.OPEN.ToString();
            string approved = SYDocumentStatus.APPROVED.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();
            var listReqProb = unitOfWork.Set<RCMRecruitRequest>().AsQueryable().Where(w => w.Status == pending).ToList();
            var listApp = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                .Where(w => listReqProb.AsEnumerable().Where(x => w.DocumentNo == x.RequestNo && w.DocumentType == x.DocType && w.Approver == userName).Any()).ToList();
            foreach (var item in listApp)
            {
                if (item.ApproveLevel == 1 && item.Status == open)
                {
                    var EmpStaff = listReqProb.Where(w => w.RequestNo == item.DocumentNo).ToList();
                    if (!EmpStaff.Any()) continue;
                    AddPendingToList(item, EmpStaff);
                }
                else if (item.ApproveLevel > 1 && item.Status == open)
                {
                    var level = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == item.DocumentNo && w.DocumentType == item.DocumentType && w.ApproveLevel < item.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                    if (level != null) continue;
                    var EmpStaff = listReqProb.Where(w => w.RequestNo == item.DocumentNo).ToList();
                    if (!EmpStaff.Any()) continue;
                    AddPendingToList(item, EmpStaff);
                }
            }
            var ListLeaveCreater = unitOfWork.Set<RCMRecruitRequest>().AsQueryable().AsEnumerable()
                                .Where(x => unitOfWork.Set<ExDocApproval>().AsQueryable()
                                    .Any(w => x.RequestNo == w.DocumentNo
                                               && x.DocType == w.DocumentType && w.Approver == userName)
                                    || x.CreatedBy == userName)
                                .ToList();
            ListHeader = ListLeaveCreater.ToList();
        }
        private void AddPendingToList(ExDocApproval item, List<RCMRecruitRequest> listReqTransfer)
        {
            foreach (var read in listReqTransfer)
            {
                var empTransfer = listReqTransfer.FirstOrDefault(w => w.RequestNo == item.DocumentNo);
                if (empTransfer != null)
                {
                    ListWaiting.Add(new ClsWaitingList
                    {
                        RequestNo = empTransfer.RequestNo,
                        Department = empTransfer.DEPT,
                        Position = empTransfer.POST,
                        Branch = empTransfer.Branch,
                        DocDate = empTransfer.DocDate.Value,
                        Section = empTransfer.Sect,
                        RequestDate = empTransfer.RequestedDate.Value,
                        Reason = empTransfer.Reason,
                        Status = empTransfer.Status,
                        CreatedBy = empTransfer.CreatedBy,
                        CreatedOn = empTransfer.CreatedOn
                    });
                }
            }

        }
        public void SetAutoApproval(string screenId, string docType, string branch, string deptCode)
        {
            ListApproval = new List<ExDocApproval>();
            var workflowConfig = unitOfWork.Set<ExCfWorkFlowItem>()
                .FirstOrDefault(w => w.ScreenID == screenId && w.DocType == docType);
            if (workflowConfig?.IsRequiredApproval != true) return;
            var approvers = unitOfWork.Set<ExCfWFApprover>().Where(w => w.WFObject == workflowConfig.ApprovalFlow && w.IsSelected).ToList();
            if (!approvers.Any()) return;
            if (!string.IsNullOrEmpty(branch))
            {
                var branchApprovers = approvers.Where(w => w.Branch == branch).ToList();
                approvers = branchApprovers.Any()
                    ? branchApprovers
                    : approvers.Where(w => string.IsNullOrEmpty(w.Branch)).ToList();
            }
            if (!string.IsNullOrEmpty(deptCode))
            {
                var deptApprovers = approvers.Where(w => w.Department == deptCode).ToList();
                approvers = deptApprovers.Any()
                    ? deptApprovers
                    : approvers.Where(w => string.IsNullOrEmpty(w.Department)).ToList();
            }
            ListApproval = approvers.GroupBy(a => a.ApproveLevel).OrderBy(g => g.Key).SelectMany(g => g
           .Select(approver => new ExDocApproval
           {
               Approver = approver.Employee,
               ApproverName = approver.EmployeeName,
               DocumentType = docType,
               ApproveLevel = approver.ApproveLevel,
               WFObject = workflowConfig.ApprovalFlow
           })).ToList();
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
        #region 'Create'
        public string createRRequest(string RequestNo)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            RequestNo = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(Header.Reason)) return "REASON";
                if (string.IsNullOrEmpty(Header.Branch)) return "BRANCH_EN";
                if (string.IsNullOrEmpty(Header.DEPT)) return "DEPARTMENT_EN";
                if (string.IsNullOrEmpty(Header.POST)) return "POSITION_EN";
                if (Header.NoOfRecruit <= 0) return "EENO";
                if (string.IsNullOrEmpty(Header.RequestedBy)) return "EEREQ";
                if (string.IsNullOrEmpty(Header.JDCode)) return "EEJD";
                if (string.IsNullOrEmpty(Header.JobRequirement)) return "INV_REQUIREMENT";
                if (string.IsNullOrEmpty(Header.JobResponsibility)) return "INV_RESPON_";
                if (!ListApproval.Any()) return "INVALID_APPROVER";
                var objCF = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_N";
                }
                var staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == Header.RequestedBy);
                if (staff == null) return "INV_STAFF";
                var objNumber = new CFNumberRank(objCF.DocType, staff.CompanyCode, Header.RequestedDate.Value.Year, true);
                if (objNumber == null) return "NUMBER_RANK_NE";
                if (objNumber.NextNumberRank == null) return "NUMBER_RANK_NE";
                Header.RequestNo = objNumber.NextNumberRank;
                Header.DocType = objCF.DocType;
                Header.Reason = Header.Reason;
                Header.DocDate = DateTime.Now;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                if (ListApproval.Any())
                {
                    ListApproval.ToList().ForEach(h =>
                    {
                        h.DocumentNo = Header.RequestNo;
                        h.DocumentType = Header.DocType;
                        h.Status = SYDocumentStatus.OPEN.ToString();
                        h.WFObject = objCF.ApprovalFlow;
                        h.ApprovedBy = "";
                        h.ApprovedName = "";
                        h.ApproverName = "";
                    });
                    unitOfWork.BulkInsert(ListApproval);
                }
                unitOfWork.Add(Header);
                unitOfWork.Save();
                unitOfWork.Commit();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.RequestedBy, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.RequestedBy, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.RequestedBy, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        #endregion
        public string UpdRRequest(string RequestNo)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(Header.Reason)) return "REASON";
                if (string.IsNullOrEmpty(Header.Branch)) return "BRANCH_EN";
                if (string.IsNullOrEmpty(Header.DEPT)) return "DEPARTMENT_EN";
                if (string.IsNullOrEmpty(Header.POST)) return "POSITION_EN";
                if (Header.NoOfRecruit <= 0) return "EENO";
                if (string.IsNullOrEmpty(Header.RequestedBy)) return "EEREQ";
                if (string.IsNullOrEmpty(Header.JDCode)) return "EEJD";
                if (string.IsNullOrEmpty(Header.JobRequirement)) return "INV_REQUIREMENT";
                if (string.IsNullOrEmpty(Header.JobResponsibility)) return "INV_RESPON_";
                if (!ListApproval.Any()) return "INVALID_APPROVER";

                var ObjMatch = unitOfWork.Repository<RCMRecruitRequest>().Queryable().FirstOrDefault(w => w.RequestNo == RequestNo);
                if (ObjMatch == null) return "DOC_INV";

                var listApprovalDoc = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentType == ObjMatch.DocType && w.DocumentNo == ObjMatch.RequestNo).ToList();
                if (listApprovalDoc.Any())
                    unitOfWork.BulkDelete(listApprovalDoc);

                var objCF = unitOfWork.Repository<ExCfWorkFlowItem>().Queryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                if (ListApproval.Any())
                {
                    ListApproval.ToList().ForEach(h =>
                    {
                        h.DocumentNo = ObjMatch.RequestNo;
                        h.DocumentType = ObjMatch.DocType;
                        h.Status = SYDocumentStatus.OPEN.ToString();
                        h.WFObject = objCF.ApprovalFlow;
                        h.ApprovedBy = "";
                        h.ApprovedName = "";
                        h.ApproverName = "";
                    });
                    unitOfWork.BulkInsert(ListApproval);
                }

                ObjMatch.Branch = Header.Branch;
                ObjMatch.DEPT = Header.DEPT;
                ObjMatch.Status = Header.Status;
                ObjMatch.POST = Header.POST;
                ObjMatch.RecruitType = Header.RecruitType;
                ObjMatch.NoOfRecruit = Header.NoOfRecruit;
                ObjMatch.WorkingType = Header.WorkingType;
                ObjMatch.ProposedSalaryTo = Header.ProposedSalaryTo;
                ObjMatch.ProposedSalaryFrom = Header.ProposedSalaryFrom;
                ObjMatch.RecruitFor = Header.RecruitFor;
                ObjMatch.JobLevel = Header.JobLevel;
                ObjMatch.Gender = Header.Gender;
                ObjMatch.TermEmp = Header.TermEmp;
                ObjMatch.ExpectedDate = Header.ExpectedDate;
                ObjMatch.RequestedBy = Header.RequestedBy;
                ObjMatch.RequestedDate = Header.RequestedDate;
                ObjMatch.Reason = Header.Reason;
                ObjMatch.JobResponsibility = Header.JobResponsibility;
                ObjMatch.JobRequirement = Header.JobRequirement;
                if (!string.IsNullOrEmpty(Header.Attachment))
                    ObjMatch.Attachment = Header.Attachment;
                ObjMatch.StaffType = Header.StaffType;
                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;

                unitOfWork.Update(ObjMatch);
                unitOfWork.Save();
                unitOfWork.Commit();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        #region 'Delete'
        public string deleteRRequest(string RequestNo)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var ObjMatch = unitOfWork.Repository<RCMRecruitRequest>().Queryable().FirstOrDefault(w => w.RequestNo == RequestNo);
                if (ObjMatch == null) return "DOC_INV";
                string Approve = SYDocumentStatus.APPROVED.ToString();
                if (ObjMatch.Status == Approve) return "DOC_APPROVED";

                var listApprovalDoc = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentType == ObjMatch.DocType && w.DocumentNo == ObjMatch.RequestNo).ToList();
                if (listApprovalDoc.Any())
                    unitOfWork.BulkDelete(listApprovalDoc);

                var _chkVac = unitOfWork.Repository<RCMVacancy>().Queryable().FirstOrDefault(w => w.DocRef == RequestNo);
                if (_chkVac != null)
                    return "EE_APPCHK";

                unitOfWork.Delete(ObjMatch);
                unitOfWork.Save();
                unitOfWork.Commit();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        #endregion 
        #region 'Convert Status'
        public string Cancel(string RequestNo)
        {
            OnLoad();
            try
            {
                var objmatch = unitOfWork.Repository<RCMRecruitRequest>().Queryable().FirstOrDefault(w => w.RequestNo == RequestNo);
                if (objmatch == null) return "DOC_INV";
                objmatch.Status = SYDocumentStatus.CANCELLED.ToString();
                objmatch.ChangedBy = User.UserName;
                objmatch.ChangedOn = DateTime.Now;

                unitOfWork.Update(objmatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Request(string RequestNo)
        {
            OnLoad();
            try
            {
                var objmatch = unitOfWork.Repository<RCMRecruitRequest>().Queryable().FirstOrDefault(w => w.RequestNo == RequestNo);

                if (objmatch == null) return "DOC_INV";

                objmatch.Status = SYDocumentStatus.PENDING.ToString();
                objmatch.ChangedBy = User.UserName;
                objmatch.ChangedOn = DateTime.Now;

                unitOfWork.Update(objmatch);
                unitOfWork.Save();

                #region *****Send to Telegram
                var excfObject = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                if (excfObject != null && !string.IsNullOrEmpty(excfObject.Telegram))
                {
                    if (string.IsNullOrEmpty(objmatch.RequestedBy)) objmatch.RequestedBy = objmatch.CreatedBy;
                    string Createby = objmatch.RequestedBy;
                    var staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == objmatch.CreatedBy);
                    if (staff != null) { Createby = staff.AllName; }
                    var post = objmatch.POST;
                    var PostD = unitOfWork.Repository<HRPosition>().Queryable().FirstOrDefault(w => w.Code == objmatch.POST);
                    if (PostD != null) { post = PostD.Description; }
                    var Branch = objmatch.POST;
                    var BranchD = unitOfWork.Repository<HRBranch>().Queryable().FirstOrDefault(w => w.Code == objmatch.Branch);
                    if (BranchD != null) { Branch = BranchD.Description; }
                    string str = $" I would like to request Man Power %0A ";
                    str += $"%0A Position: <b>{post}</b> ";
                    str += $"%0A Branch: <b>{Branch}</b> ";
                    str += $"%0A Base salary: <b>{objmatch.BasicSalary}$</b> ";
                    str += $"%0A Allowance: <b>{objmatch.BenefitAmount}$</b> %0A Please review and approve with favor.";
                    str += $"%0A%0AYours sincerely,%0A%0A<b>{Createby}</b>";

                    // Initialize Telegram sender
                    SYSendTelegramObject Tel = new SYSendTelegramObject
                    {
                        User = User,
                        BS = BS
                    };

                    WorkFlowResult result1 = Tel.Send_SMS_Telegram(str, excfObject.Telegram, false);
                    MessageError = Tel.getErrorMessage(result1);
                }
                #endregion

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public void SendEmail(string Upload, string Rerceiver)
        {
            try
            {
                #region Email
                var AlertTO = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == Rerceiver);
                var EmailConf = unitOfWork.Repository<CFEmailAccount>().Queryable().FirstOrDefault();
                if (EmailConf != null && AlertTO != null && !string.IsNullOrEmpty(AlertTO.Email))
                {
                    CFEmailAccount emailAccount = EmailConf;
                    string subject = string.Format("Recruitment Request Form {0:dd-MMM-yyyy}", DateTime.Today);
                    string filePath = Upload;
                    string fileName = Path.GetFileName(filePath);
                    EmailObject = new ClsEmail();
                    int rs = EmailObject.SendMail(emailAccount, "", AlertTO.Email,
                        subject, "", filePath, fileName);
                }
                #endregion
            }
            catch
            {
                throw new Exception("FAIL_TO_SEND_MAIL");
            }
        }
        public string Approved(string RequestNo)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var objmatch = unitOfWork.Repository<RCMRecruitRequest>().Queryable().FirstOrDefault(w => w.RequestNo == RequestNo);
                if (objmatch == null) return "DOC_INV";

                string openStatus = SYDocumentStatus.OPEN.ToString();
                var approvalList = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                    .Where(w => w.DocumentType == objmatch.DocType && w.DocumentNo == objmatch.RequestNo && w.Status == openStatus)
                    .OrderBy(w => w.ApproveLevel).ToList();
                if (!approvalList.Any())
                {
                    return "RESTRICT_ACCESS";
                }
                var approverList = unitOfWork.Set<ExCfWFApprover>()
                    .Where(w => w.Employee == User.UserName)
                    .ToList();

                bool hasApproved = false;
                string approver = User.UserName;
                foreach (var approval in approvalList)
                {
                    if (!approverList.Any(w => w.Employee == approval.Approver)) continue;

                    if (approval.Status == SYDocumentStatus.APPROVED.ToString())
                        return "USER_ALREADY_APP";
                    if (approval.ApproveLevel > approvalList.Min(w => w.ApproveLevel))
                        return "REQUIRED_PRE_LEVEL";
                    var approverProfile = unitOfWork.Set<HRStaffProfile>()
                        .FirstOrDefault(w => w.EmpCode == approval.Approver);

                    if (approverProfile == null) continue;
                    approver = approval.Approver;
                    UpdateApprovalRecord(approval, approverProfile);
                    hasApproved = true;
                    break;
                }

                if (!hasApproved) return "USER_CANNOT_APPROVE";
                var objapprover = approvalList.Where(w => w.Approver == approver).ToList();
                //NotifyNextApprover(objapprover, Upload, openStatus);
                UpdateObjStatus(objmatch, approvalList, openStatus);

                //if (objmatch.Status == SYDocumentStatus.APPROVED.ToString())
                //{
                //    NotifyRequestor(objmatch, Upload);
                //}
                unitOfWork.Save();
                unitOfWork.Commit();
                if (objmatch.Status == SYDocumentStatus.APPROVED.ToString())
                {
                    #region *****Send to Telegram
                    var excfObject = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                    if (excfObject != null && !string.IsNullOrEmpty(excfObject.Telegram))
                    {
                        var EmpCreate = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(s => s.EmpCode == objmatch.CreatedBy);
                        var Applevel = objapprover.FirstOrDefault();
                        var Approve = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Applevel.Approver);
                        string message = $@"Dear {EmpCreate.Title} {EmpCreate.AllName}
                                        %0AI’m approved for requested Man Power
                                        %0A%0AYours sincerely,
                                        %0A{Approve.AllName}";

                        SYSendTelegramObject Tel = new SYSendTelegramObject { User = User, BS = BS };
                        WorkFlowResult result = Tel.Send_SMS_Telegram(message, excfObject.Telegram, false);
                        MessageError = Tel.getErrorMessage(result);
                    }
                    #endregion
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        private void UpdateApprovalRecord(ExDocApproval approval, HRStaffProfile profile)
        {
            approval.ApprovedBy = profile.EmpCode;
            approval.ApprovedName = profile.AllName;
            approval.LastChangedDate = DateTime.Now;
            approval.ApprovedDate = DateTime.Now;
            approval.Status = SYDocumentStatus.APPROVED.ToString();
            unitOfWork.Update(approval);
        }
        private void NotifyNextApprover(List<ExDocApproval> approvals, string fileName, string openStatus)
        {
            var nextApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                .Where(a => a.Status == openStatus && a.DocumentNo == approvals.FirstOrDefault().DocumentNo
                       && a.DocumentType == approvals.FirstOrDefault().DocumentType && a.ApproveLevel > approvals.FirstOrDefault().ApproveLevel)
                .OrderBy(a => a.ApproveLevel)
                .FirstOrDefault();

            if (nextApproval == null) return;

            var staffProfile = unitOfWork.Set<HRStaffProfile>()
                .FirstOrDefault(x => x.EmpCode == nextApproval.Approver);

            if (staffProfile == null || string.IsNullOrEmpty(staffProfile.Email)) return;

            SendApprovalEmail(staffProfile, fileName);
        }
        private void SendApprovalEmail(HRStaffProfile approver, string Upload)
        {

            try
            {
                var emailConfig = unitOfWork.Set<CFEmailAccount>().FirstOrDefault();
                if (emailConfig == null) return;

                CFEmailAccount emailAccount = emailConfig;
                string subject = string.Format("Recruitment Request Form {0:dd-MMM-yyyy}", DateTime.Today);
                string filePath = Upload;
                string fileName = Path.GetFileName(filePath);
                EmailObject = new ClsEmail();
                int rs = EmailObject.SendMail(emailAccount, "", approver.Email,
                    subject, "", filePath, fileName);
                //List<string> filePathsList = new List<string> { fileName };
                //if (!string.IsNullOrEmpty(record.AttachFile))
                //    filePathsList.Add(record.AttachFile);
                //string[] filePaths = filePathsList.ToArray();
                //email.SendMails(emailConfig, "", approver.Email, Subject, emailBody, filePaths);
            }
            catch (Exception ex)
            {
                throw new Exception("FAIL_TO_SEND_MAIL: " + ex.Message);
            }
        }
        private void UpdateObjStatus(RCMRecruitRequest record, List<ExDocApproval> approvals, string openStatus)
        {
            record.Status = approvals.Any(w => w.Status == openStatus)
                ? SYDocumentStatus.PENDING.ToString()
                : SYDocumentStatus.APPROVED.ToString();

            unitOfWork.Update(record);
        }
        private void NotifyRequestor(RCMRecruitRequest record, string Upload)
        {
            // 1. Send email to requestor
            var EmpCreate = unitOfWork.Set<HRStaffProfile>()
                .FirstOrDefault(s => s.EmpCode == record.CreatedBy);

            if (EmpCreate != null && !string.IsNullOrEmpty(EmpCreate.Email))
            {
                var EmailConf = unitOfWork.Set<CFEmailAccount>().FirstOrDefault();
                if (EmailConf != null)
                {
                    CFEmailAccount emailAccount = EmailConf;
                    string subject = string.Format("Recruitment Request Form {0:dd-MMM-yyyy}", DateTime.Today);
                    string filePath = Upload;
                    string fileName = Path.GetFileName(filePath);
                    EmailObject = new ClsEmail();
                    int rs = EmailObject.SendMail(emailAccount, "", EmpCreate.Email,
                        subject, "", filePath, fileName);
                }
            }
        }
        public string Reject(string RequestNo)
        {
            try
            {
                string[] c = RequestNo.Split(';');
                foreach (var Doc in c)
                {
                    if (Doc == "") continue;
                    var rejectedStatus = SYDocumentStatus.REJECTED.ToString();
                    var objmatch = unitOfWork.Repository<RCMRecruitRequest>().Queryable().FirstOrDefault(w => w.RequestNo == Doc);
                    if (objmatch == null) return "INV_EN";
                    if (objmatch.Status == rejectedStatus) return "DOC_RJ_AR";
                    var openStatus = SYDocumentStatus.OPEN.ToString();
                    var approvals = GetOpenApprovals(objmatch, openStatus);
                    var userApprovals = GetUserApprovals();
                    if (!ProcessApprovals(approvals, userApprovals, rejectedStatus)) return "USER_NOT_APPROVOR";
                    objmatch.Status = DetermineFinalStatus(approvals, openStatus);
                    //SendEmailToApprovers(objmatch.RequestNo, User.UserName);
                    //SendEmailToRequester(objmatch.RequestedBy, User.UserName);
                }
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, RequestNo, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        private List<ExDocApproval> GetOpenApprovals(RCMRecruitRequest objmatch, string openStatus)
        {
            return unitOfWork.Repository<ExDocApproval>().Queryable()
                .Where(w => w.DocumentType == objmatch.DocType && w.DocumentNo == objmatch.RequestNo && w.Status == openStatus)
                .OrderBy(w => w.ApproveLevel).ToList();
        }
        private List<HRStaffProfile> GetUserApprovals()
        {
            return unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w => w.EmpCode == User.UserName).ToList();
        }
        private bool ProcessApprovals(List<ExDocApproval> approvals, List<HRStaffProfile> userApprovals, string rejectedStatus)
        {
            bool isApprovalProcessed = false;

            foreach (var approval in approvals)
            {
                var userApproval = userApprovals.FirstOrDefault(w => w.EmpCode == approval.Approver);
                if (userApproval != null)
                {
                    if (approval.Status == SYDocumentStatus.APPROVED.ToString()) return true;
                    if (approval.ApproveLevel > approvals.Min(w => w.ApproveLevel)) return false;
                    approval.LastChangedDate = DateTime.Now.Date;
                    approval.Status = rejectedStatus;
                    unitOfWork.Update(approval);
                    isApprovalProcessed = true;
                    break;
                }
            }

            return isApprovalProcessed;
        }
        private string DetermineFinalStatus(List<ExDocApproval> approvals, string openStatus)
        {
            return approvals.Any(w => w.Status == openStatus) ? SYDocumentStatus.REJECTED.ToString() : SYDocumentStatus.REJECTED.ToString();
        }
        private void SendEmailToApprovers(string requestNo, string userName)
        {
            var approvals = unitOfWork.Repository<ExDocApproval>().Queryable()
                .Where(w => w.DocumentType == "LR" && w.DocumentNo == requestNo && w.Status == SYDocumentStatus.APPROVED.ToString()).OrderBy(w => w.ApproveLevel).ToList();
            string subject = "Recruitment Request Form Reject";
            foreach (var approval in approvals)
            {
                var alertTo = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == approval.Approver);
                var emailConfig = unitOfWork.Repository<CFEmailAccount>().Queryable().FirstOrDefault();

                if (emailConfig != null && alertTo != null && !string.IsNullOrEmpty(alertTo.Email))
                {
                    SendEmail(emailConfig, alertTo.Email, userName, subject);
                }
            }
        }
        private void SendEmailToRequester(string requesterCode, string userName)
        {
            if (!string.IsNullOrEmpty(requesterCode))
            {
                var alertTo = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == requesterCode);
                var emailConfig = unitOfWork.Repository<CFEmailAccount>().Queryable().FirstOrDefault();

                if (emailConfig != null && alertTo != null && !string.IsNullOrEmpty(alertTo.Email))
                {
                    string subject = "Recruitment Request Form Reject";
                    SendEmail(emailConfig, alertTo.Email, userName, subject);
                }
            }
        }
        private void SendEmail(CFEmailAccount emailAccount, string recipientEmail, string userName, string subject)
        {
            string body = $"Dear {recipientEmail},<b>{recipientEmail}</b>,<br/>The Recruitment Request Form has been rejected by {userName}.";
            // Add file path and filename if necessary
            EmailObject = new ClsEmail();
            EmailObject.SendMail(emailAccount, "", recipientEmail, subject, body, "", "");
        }
        #endregion
        public static IEnumerable<RCMSJobDesc> GetJD(IUnitOfWork unitOfWork)
        {
            var position = HttpContext.Current.Session["Position"] as string;

            if (!string.IsNullOrEmpty(position))
            {
                return unitOfWork.Repository<RCMSJobDesc>().Queryable()
                    .Where(w => w.Position == position)
                    .ToList();
            }

            return Enumerable.Empty<RCMSJobDesc>();
        }
    }
    public class ClsWaitingList
    {
        public string RequestNo { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Branch { get; set; }
        public DateTime DocDate { get; set; }
        public string Section { get; set; }
        public DateTime RequestDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
