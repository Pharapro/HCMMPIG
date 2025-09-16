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

namespace Humica.EForm
{
    public class CLsESSEmpTermination : ICLsESSEmpTermination
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public FTINYear FInYear { get; set; }
        public string MessageCode { get; set; }
        public string MessageError { get; set; }
        public HRStaffProfile Staff { get; set; }
        public HREFEmpResign Header { get; set; }
        public ExDocApproval DocApproval { get; set; }
        public string DocType { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public HREmpCareer HeaderCareer { get; set; }
        public List<HREmpCareer> ListCareer { get; set; }
        public List<HR_STAFF_VIEW> ListStaff { get; set; }
        public List<HREFEmpResign> listHeader { get; set; }
        public List<ClsReuestTerminat> ListRequestPending { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<ExCfWFApprover> ListEXCFApproval { get; set; }
        public ClsEmail EmailObject { get; private set; }
        long TranNoCareer;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public CLsESSEmpTermination()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void LoadData(string userName, string ResignType)
        {
            OnLoad();
            var staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().Where(w => w.EmpCode == userName).ToList();
            if (staff.Any())
            {
                string approved = SYDocumentStatus.APPROVED.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                string Pending = SYDocumentStatus.PENDING.ToString();
                var Resig = unitOfWork.Set<HREFEmpResign>().AsQueryable().Where(x => x.Status == Pending && x.ResignType == ResignType).ToList();
                var ListApp = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable().Where(w => Resig.Where(x => x.DocumentNo == w.DocumentNo && x.DocType == w.DocumentType && w.Approver == userName).Any()).ToList();
                foreach (var read in ListApp)
                {
                    var objemp = new ClsReuestTerminat();
                    if (read.ApproveLevel > 1 && read.Status == Open)
                    {
                        var level = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == read.DocumentNo && w.DocumentType == read.DocumentType && w.ApproveLevel < read.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                        if (level != null) continue;
                        var EmpStaff = Resig.FirstOrDefault(w => w.DocumentNo == read.DocumentNo);
                        if (EmpStaff == null) continue;
                        objemp.EmpCode = EmpStaff.EmpCode;
                        objemp.DocumentNo = EmpStaff.DocumentNo;
                        objemp.EmpName = EmpStaff.EmpName;
                        objemp.Branch = EmpStaff.Branch;
                        objemp.Department = EmpStaff.Department;
                        objemp.Position = EmpStaff.Position;
                        objemp.Section = EmpStaff.Section;
                        objemp.Reason = EmpStaff.Reason;
                        objemp.ResigneDate = EmpStaff.ResigneDate;
                        objemp.ResignType = EmpStaff.ResignType;
                        objemp.DocDate = EmpStaff.DocDate;
                        objemp.Status = EmpStaff.Status;
                        ListRequestPending.Add(objemp);
                    }
                    else if (read.ApproveLevel == 1 && read.Status == Open)
                    {
                        var EmpStaff = Resig.FirstOrDefault(w => w.DocumentNo == read.DocumentNo);
                        if (EmpStaff == null) continue;
                        objemp.EmpCode = EmpStaff.EmpCode;
                        objemp.DocumentNo = EmpStaff.DocumentNo;
                        objemp.EmpName = EmpStaff.EmpName;
                        objemp.Branch = EmpStaff.Branch;
                        objemp.Department = EmpStaff.Department;
                        objemp.Position = EmpStaff.Position;
                        objemp.Section = EmpStaff.Section;
                        objemp.Reason = EmpStaff.Reason;
                        objemp.ResigneDate = EmpStaff.ResigneDate;
                        objemp.ResignType = EmpStaff.ResignType;
                        objemp.DocDate = EmpStaff.DocDate;
                        objemp.Status = EmpStaff.Status;
                        ListRequestPending.Add(objemp);
                    }

                }
                var ListLeaveCreater = unitOfWork.Set<HREFEmpResign>().AsQueryable().AsEnumerable()
                                  .Where(x => unitOfWork.Set<ExDocApproval>().AsQueryable()
                                      .Any(w => x.DocumentNo == w.DocumentNo
                                                 && x.DocType == w.DocumentType && w.Approver == userName && x.Status != Open && x.Status != Cancel)
                                      || x.CreatedBy == userName)
                                  .ToList();
                ListLeaveCreater = ListLeaveCreater.Where(x => x.ResignType == ResignType).ToList();
                listHeader = ListLeaveCreater.ToList();
            }
        }
        public string SaveResign(bool IsResign)
        {
            OnLoad();
            try
            {
                if (!ListApproval.Any()) return "INVALID_APPROVER";
                if (string.IsNullOrEmpty(Header.Reason)) return "REASON";
                if (Header.ResigneDate == null || Header.ResigneDate <= DateTime.MinValue) return "INVALIDED_EFFECTDATE";
                string Status = SYDocumentStatus.OPEN.ToString();
                var staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                if (staff == null) return "INVALID_EMP";
                var objNumber = new CFNumberRank(DocType, staff.CompanyCode, Header.ResigneDate.Year, true);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.ResignType = "TERMINAT";
                string WFObject = "TER";
                if (IsResign)
                {
                    Header.ResignType = "RESIGN";
                    WFObject = "RRS";
                }
                Header.DocumentNo = objNumber.NextNumberRank;
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.EmpName = HeaderStaff.AllName;
                Header.Branch = staff.Branch;
                Header.DocDate = DateTime.Now;
                Header.Position = staff.JobCode;
                Header.Section = staff.SECT;
                Header.Department = staff.DEPT;
                Header.DocType = DocType;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                unitOfWork.Add(Header);
                foreach (var read in ListApproval)
                {
                    if (read.Approver == Header.CreatedBy) continue;
                    var objApp = new ExDocApproval();
                    objApp.DocumentNo = Header.DocumentNo;
                    objApp.DocumentType = DocType;
                    objApp.Status = Status;
                    objApp.Approver = read.Approver;
                    objApp.ApproverName = read.ApproverName;
                    objApp.ApproveLevel = read.ApproveLevel;
                    objApp.WFObject = WFObject;
                    objApp.ApprovedBy = "";
                    objApp.ApprovedName = "";
                    objApp.LastChangedDate = DateTime.Now;
                    unitOfWork.Add(objApp);
                }
                unitOfWork.Save();
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
        public void SetAutoApproval(string screenId, string docType, string branch, string deptCode)
        {
            ListApproval = new List<ExDocApproval>();

            // Get workflow configuration
            var workflowConfig = unitOfWork.Set<ExCfWorkFlowItem>()
                .FirstOrDefault(w => w.ScreenID == screenId && w.DocType == docType);

            if (workflowConfig?.IsRequiredApproval != true)
                return;

            // Get base list of approvers
            var approvers = unitOfWork.Set<ExCfWFApprover>()
                .Where(w => w.WFObject == workflowConfig.ApprovalFlow && w.IsSelected)
                .ToList();

            if (!approvers.Any())
                return;

            // Apply branch filter
            if (!string.IsNullOrEmpty(branch))
            {
                var branchApprovers = approvers.Where(w => w.Branch == branch).ToList();
                approvers = branchApprovers.Any()
                    ? branchApprovers
                    : approvers.Where(w => string.IsNullOrEmpty(w.Branch)).ToList();
            }

            // Apply department filter
            if (!string.IsNullOrEmpty(deptCode))
            {
                var deptApprovers = approvers.Where(w => w.Department == deptCode).ToList();
                approvers = deptApprovers.Any()
                    ? deptApprovers
                    : approvers.Where(w => string.IsNullOrEmpty(w.Department)).ToList();
            }

            // Create approval records
            ListApproval = approvers
       .GroupBy(a => a.ApproveLevel)  // Group by approval level
       .OrderBy(g => g.Key)           // Sort groups by level
       .SelectMany(g => g             // Flatten groups while maintaining level order
           .Select(approver => new ExDocApproval
           {
               Approver = approver.Employee,
               ApproverName = approver.EmployeeName,
               DocumentType = docType,
               ApproveLevel = approver.ApproveLevel,
               WFObject = workflowConfig.ApprovalFlow
           })).ToList();
        }
        #region Status
        public string RequestForApprove(string Doc, string fileName, string URL, bool IsResign)
        {
            OnLoad();
            try
            {
                string ResignType = "Terminate";
                string Type = "TERMINAT";
                if (IsResign)
                {
                    ResignType = "Resign";
                    Type = "RESIGN";
                }
                var objMatch = unitOfWork.Set<HREFEmpResign>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Doc && w.ResignType == Type);
                if (objMatch == null) return "DOC_INV";
                string open = SYDocumentStatus.OPEN.ToString();
                var ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentType == objMatch.DocType
                                   && w.DocumentNo == objMatch.DocumentNo && w.Status == open).OrderBy(w => w.ApproveLevel).ToList().FirstOrDefault();
                if (ListApproval == null) return "INVALID_APPROVER";
                objMatch.Status = SYDocumentStatus.PENDING.ToString();
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                string creater = objMatch.CreatedBy;
                var Createdby = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == objMatch.CreatedBy);
                if (Createdby != null) creater = Createdby.AllName;
                var StaffApp = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == ListApproval.Approver);
                if (StaffApp != null)
                {
                    #region Email
                    if (!string.IsNullOrEmpty(StaffApp.Email))
                    {
                        try
                        {
                            // Retrieve email configuration
                            var EmailConf = unitOfWork.Set<CFEmailAccount>().AsQueryable().FirstOrDefault();
                            if (EmailConf != null)
                            {
                                CFEmailAccount emailAccount = EmailConf;
                                string str = $"Dear {StaffApp.Title} {StaffApp.AllName}";
                                str += $" <br /> I would like to Request {ResignType} staff  <br /> <b> {objMatch.EmpName} </b> ";
                                str += $"<br /> on date  <b>{objMatch.ResigneDate.ToString("dd/MMM/yyyy")} </b>";
                                str += $"<br /> <b> Reason: </b> {objMatch.Reason} <br /> Please review and approve with favor.";
                                str += $"<br /> <br /> Yours sincerely,<br /> <br /> <b>{creater}</b>";

                                // Prepare email details
                                string subject = $"Request {ResignType}";
                                string body = str; // Use the constructed message as the body
                                string filePath = fileName; // Assuming fileName is defined elsewhere
                                string fileName_ = Path.GetFileName(fileName);

                                // Send the email
                                EmailObject = new ClsEmail();
                                List<string> filePathsList = new List<string> { fileName };
                                if (!string.IsNullOrEmpty(objMatch.AttachFile))
                                    filePathsList.Add(objMatch.AttachFile);
                                string[] filePaths = filePathsList.ToArray();
                                int rs = EmailObject.SendMails(emailAccount, "", StaffApp.Email, subject, body, filePaths);

                                if (rs != 0)
                                {
                                    // Handle the case where email was not sent successfully
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the exception or handle it as needed
                            throw new Exception("FAIL_TO_SEND_MAIL: " + ex.Message);
                        }
                    }
                    #endregion

                    #region *****Send to Telegram
                    var excfObject = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                    if (excfObject != null && !string.IsNullOrEmpty(excfObject.Telegram))
                    {
                        string str = $" I would like to Request {ResignType} staff %0A <b> {objMatch.EmpName}";
                        str += $"</b> %0A on date  <b>{objMatch.ResigneDate.ToString("dd/MMM/yyyy")} </b>";
                        str += $"%0A <b> Reason: </b> {objMatch.Reason} %0A Please review and approve with favor.";
                        str += $"%0A%0AYours sincerely,%0A%0A<b>{creater}</b>";

                        // Initialize Telegram sender
                        SYSendTelegramObject Tel = new SYSendTelegramObject
                        {
                            User = User,
                            BS = BS
                        };

                        // Send the Telegram message
                        WorkFlowResult result = Tel.Send_SMS_Telegram(str, excfObject.Telegram, false);
                        MessageError = Tel.getErrorMessage(result);
                    }
                    #endregion
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.RELEASE.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.RELEASE.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.RELEASE.ToString(), e, true);
            }
        }
        public string Cancel(string Doc, bool IsResign)
        {
            OnLoad();
            try
            {
                string ResignType = "TERMINAT";
                if (IsResign) ResignType = "RESIGN";
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                string Pending = SYDocumentStatus.PENDING.ToString();
                var objmatch = unitOfWork.Set<HREFEmpResign>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Doc && w.ResignType == ResignType);
                if (objmatch == null)
                {
                    return "DOC_INV";
                }
                if (objmatch.Status == Open || objmatch.Status == Pending)
                {
                    objmatch.Status = Cancel;
                    unitOfWork.Update(objmatch);
                }
                else
                {
                    return "Document is approved cannot cancel";
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.EDIT.ToString(), e, true);
            }
        }
        public string Reject(string Document, bool IsResign)
        {
            OnLoad();
            try
            {
                string ResignType = "TERMINAT";
                string Type = "Terminate";
                if (IsResign)
                {
                    ResignType = "RESIGN";
                    Type = "Resign";
                }
                string Reject = SYDocumentStatus.REJECTED.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                string[] c = Document.Split(';');
                foreach (var Doc in c)
                {
                    if (Doc == "") continue;
                    var objmatch = unitOfWork.Set<HREFEmpResign>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Doc && w.ResignType == ResignType);
                    if (objmatch == null)
                    {
                        return "INV_EN";
                    }
                    var _obj = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable().Where(w => w.DocumentType == objmatch.DocType
                                                 && w.DocumentNo == Doc && w.Approver == User.UserName && w.Status == Open);
                    if (!_obj.Any()) return "USER_CANNOT_REJECT";
                    foreach (var read in _obj)
                    {
                        read.Status = Reject;
                        read.LastChangedDate = DateTime.Now;
                        read.ApprovedBy = User.UserName;
                        var appName = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == User.UserName);
                        if (appName != null)
                        {
                            read.ApprovedName = appName.AllName;
                        }
                        read.ApprovedDate = DateTime.Now;
                        unitOfWork.Update(read);
                    }
                    objmatch.Status = Reject;
                    unitOfWork.Update(objmatch);

                    unitOfWork.Save();
                    var EmpCreate = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(s => s.EmpCode == objmatch.CreatedBy);
                    var StaffView = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == objmatch.EmpCode);
                    if (EmpCreate != null && !string.IsNullOrEmpty(EmpCreate.Email) && StaffView != null)
                    {
                        var EmailConf = unitOfWork.Set<CFEmailAccount>().AsQueryable().FirstOrDefault();
                        if (EmailConf != null)
                        {
                            CFEmailAccount emailAccount = EmailConf;
                            string subject = $"Request {Type}";
                            string body = $"Your request for the {Type} of staff <b>{StaffView.AllName}</b> has been rejected.";
                            string filePath = "";// fileName; // Ensure fileName is defined
                            string fileName_ = ""; //Path.GetFileName(filePath);

                            EmailObject = new ClsEmail();
                            int rs = EmailObject.SendMail(emailAccount, "", EmpCreate.Email, subject, body, filePath, fileName_);
                        }
                    }
                    var excfObject = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                    if (excfObject != null && !string.IsNullOrEmpty(excfObject.Telegram))
                    {
                        string str = $" Your request for the {Type} of staff <b> {StaffView.AllName} </b> has been rejected.";

                        // Initialize Telegram sender
                        SYSendTelegramObject Tel = new SYSendTelegramObject
                        {
                            User = User,
                            BS = BS
                        };

                        WorkFlowResult result = Tel.Send_SMS_Telegram(str, excfObject.Telegram, false);
                        MessageError = Tel.getErrorMessage(result);
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.RELEASE.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.RELEASE.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.EmpCode, SYActionBehavior.RELEASE.ToString(), e, true);
            }
        }
        public string approveTheDoc(string Inc, string fileName, string URL, bool IsResign)
        {
            OnLoad();
            string latestCareer = SYConstant.OK;
            unitOfWork.BeginTransaction();
            try
            {
                latestCareer = ProcessApproval(Inc, fileName, URL, IsResign);
                unitOfWork.Commit();
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Inc, SYActionBehavior.RELEASE.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Inc, SYActionBehavior.RELEASE.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Inc, SYActionBehavior.RELEASE.ToString(), e, true);
            }
            return latestCareer;
        }
        private string ProcessApproval(string Inc, string fileName, string URL, bool IsResign)
        {
            string ResignType = "TERMINAT";
            if (IsResign)
            {
                ResignType = "RESIGN";
            }
            var resignationRecord = unitOfWork.Set<HREFEmpResign>()
                .FirstOrDefault(x => x.DocumentNo == Inc && x.ResignType == ResignType);

            if (resignationRecord == null) return "INVALID_DOC";

            string openStatus = SYDocumentStatus.OPEN.ToString();
            var approvalList = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                .Where(w => w.DocumentType == resignationRecord.DocType &&
                           w.DocumentNo == Inc &&
                           w.Status == openStatus)
                .OrderBy(w => w.ApproveLevel)
                .ToList();
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
            NotifyNextApprover(resignationRecord, objapprover, fileName, openStatus, IsResign);
            UpdateResignationStatus(resignationRecord, approvalList, openStatus);

            if (resignationRecord.Status == SYDocumentStatus.APPROVED.ToString())
            {
                ProcessTermination(resignationRecord, fileName, objapprover, IsResign);
            }

            return SYConstant.OK;
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
        private void NotifyNextApprover(HREFEmpResign record, List<ExDocApproval> approvals, string fileName, string openStatus, bool IsResign)
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

            var staffView = unitOfWork.Set<HR_STAFF_VIEW>()
                .FirstOrDefault(w => w.EmpCode == record.EmpCode);

            if (staffView == null) return;

            SendApprovalEmail(record, staffProfile, staffView, fileName, IsResign);
        }
        private void SendApprovalEmail(HREFEmpResign record, HRStaffProfile approver, HR_STAFF_VIEW staffView, string fileName, bool IsResign)
        {
            string creater = record.CreatedBy;
            var Createdby = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == record.CreatedBy);
            if (Createdby != null) creater = Createdby.AllName;
            string Type = "Terminate";
            if (IsResign)
                Type = "Resign";
            try
            {
                var emailConfig = unitOfWork.Set<CFEmailAccount>().FirstOrDefault();
                if (emailConfig == null) return;

                var emailBody = $@"Dear {approver.Title} {approver.AllName}
            <br/><br/>I would like to Request {Type} staff
            <br/><b>{record.EmpName}</b>
            <br/>on date <b>{record.ResigneDate:dd/MMM/yyyy}</b>
            <br/><b>Reason:</b> {record.Reason}
            <br/><br/>Please review and approve with favor.
            <br/><br/>Yours sincerely,
            <br/><br/><b>{creater}</b>";
                var Subject = $"Request {Type}";
                var email = new ClsEmail();
                List<string> filePathsList = new List<string> { fileName };
                if (!string.IsNullOrEmpty(record.AttachFile))
                    filePathsList.Add(record.AttachFile);
                string[] filePaths = filePathsList.ToArray();
                email.SendMails(emailConfig, "", approver.Email, Subject, emailBody, filePaths);
            }
            catch (Exception ex)
            {
                throw new Exception("FAIL_TO_SEND_MAIL: " + ex.Message);
            }
        }
        private void UpdateResignationStatus(HREFEmpResign record, List<ExDocApproval> approvals, string openStatus)
        {
            record.Status = approvals.Any(w => w.Status == openStatus)
                ? SYDocumentStatus.PENDING.ToString()
                : SYDocumentStatus.APPROVED.ToString();

            unitOfWork.Update(record);
        }
        private void ProcessTermination(HREFEmpResign record, string fileName, List<ExDocApproval> approvals, bool IsResign)
        {
            var staff = unitOfWork.Set<HRStaffProfile>()
                .FirstOrDefault(w => w.EmpCode == record.EmpCode);
            if (staff == null) throw new Exception("STAFF_NOT_FOUND");

            UpdateCareerHistory(record, staff, IsResign);
            NotifyRequestor(record, fileName, approvals, IsResign);
        }
        private void UpdateCareerHistory(HREFEmpResign record, HRStaffProfile staff, bool IsResign)
        {
            string ResignType = "TERMINAT";
            string Type = "Terminate";
            if (IsResign)
            {
                ResignType = "RESIGN";
                Type = "Resign";
            }
            // 1. Validate required entities
            var objMatchHeader = unitOfWork.Set<HRStaffProfile>()
                .FirstOrDefault(w => w.EmpCode == record.EmpCode);
            if (objMatchHeader == null)
                throw new Exception("CAREERHISTORY_NE");

            // 2. Retrieve career data with concurrency check
            var LstCareerCode = unitOfWork.Set<HRCareerHistory>().ToList();
            var ListCareer = unitOfWork.Set<HREmpCareer>()
                .Where(w => w.EmpCode == objMatchHeader.EmpCode)
                .ToList();

            // 3. Find termination career code
            var _CarCode = LstCareerCode.FirstOrDefault(x => x.Code == "TERMINAT")
                        ?? LstCareerCode.FirstOrDefault(w => w.NotCalSalary == true);
            if (_CarCode == null)
                throw new Exception("CAREERCODE_EN");

            var LstResType = unitOfWork.Set<HRTerminType>()
                .FirstOrDefault(w => w.Description.Contains(ResignType));
            if (_CarCode.NotCalSalary == true && LstResType == null)
                throw new Exception("SEPARATE_TYPE_EN");

            // 4. Validate career dates
            if (ListCareer.Any(w => w.FromDate.Value.Date >= record.ResigneDate.Date))
                throw new Exception("INV_DATE");

            // 5. Update existing career records with concurrency check
            var latestCareer = ListCareer.OrderByDescending(x => x.FromDate).FirstOrDefault();
            if (latestCareer != null)
            {
                latestCareer.LCKEDIT = 1;
                latestCareer.ToDate = record.ResigneDate.AddDays(-1);
                unitOfWork.Update(latestCareer);
            }

            // 6. Create new termination career record
            long tranNo = unitOfWork.Set<HREmpCareer>()
                .Select(w => (long?)w.TranNo)
                .DefaultIfEmpty(0)
                .Max()
                .GetValueOrDefault() + 1;
            TranNoCareer = tranNo;
            var HeaderCareer = new HREmpCareer
            {
                TranNo = tranNo,
                EmpCode = objMatchHeader.EmpCode,
                CareerCode = _CarCode.Code,
                resigntype = LstResType?.Code,
                EmpType = latestCareer?.EmpType,
                CompanyCode = latestCareer?.CompanyCode,
                Branch = latestCareer?.Branch,
                GroupDept = latestCareer.GroupDept,
                DEPT = latestCareer?.DEPT,
                LOCT = latestCareer?.LOCT,
                Division = latestCareer?.Division,
                LINE = objMatchHeader.Line,
                SECT = latestCareer?.SECT,
                CATE = latestCareer?.CATE,
                LevelCode = latestCareer?.LevelCode,
                JobCode = latestCareer?.JobCode,
                JobGrade = latestCareer?.JobGrade,
                JobDesc = objMatchHeader.POSTDESC,
                JobSpec = objMatchHeader.JOBSPEC,
                EstartSAL = latestCareer?.EstartSAL,
                EIncrease = latestCareer?.EIncrease,
                ESalary = latestCareer?.ESalary,
                SupCode = latestCareer?.SubFunction,
                FromDate = record.ResigneDate.Date,
                ToDate = new DateTime(5000, 1, 1),
                EffectDate = record.ResigneDate.Date,
                ProDate = objMatchHeader.StartDate,
                Reason = Type,
                Remark = "",
                Appby = "",
                AppDate = objMatchHeader.StartDate.Value.ToString("dd-MM-yyyy"),
                VeriFyBy = "",
                VeriFYDate = objMatchHeader.StartDate.Value.ToString("dd-MM-yyyy"),
                LCK = 0,
                OldSalary = latestCareer.OldSalary,
                Increase = latestCareer.Increase,
                Functions = latestCareer.Functions,
                NewSalary = latestCareer.NewSalary,
                PersGrade = latestCareer.PersGrade,
                HomeFunction = latestCareer.HomeFunction,
                SubFunction = latestCareer.SubFunction,
                StaffType = latestCareer.StaffType,
                CreateBy = User.UserName,
                CreateOn = DateTime.Now,
            };

            unitOfWork.Add(HeaderCareer);

            staff.Status = SYDocumentStatus.I.ToString();
            staff.CareerDesc = _CarCode.Code;
            staff.DateTerminate = record.ResigneDate;
            staff.EffectDate = record.ResigneDate;
            unitOfWork.Update(staff);
            UpdateBankAccounts(HeaderCareer, staff);
        }
        private void UpdateBankAccounts(HREmpCareer HeaderCareer, HRStaffProfile staff)
        {
            var objMatchHeader = staff;
            //var HeaderCareer = unitOfWork.Set<HREmpCareer>()
            //    .OrderByDescending(c => c.TranNo)
            //    .FirstOrDefault(c => c.EmpCode == record.EmpCode);

            if (HeaderCareer == null) return;

            // 1. Get active bank accounts
            var objBank = unitOfWork.Set<HREmpBankAcc>()
                .Where(w => w.EmpCode == objMatchHeader.EmpCode && w.IsActive == true)
                .ToList();

            var bankDict = objBank.ToDictionary(b => new { b.Company, b.Reference });
            var ListCareerBankList = new List<HREmpCareerBankList>();

            // 2. Process existing accounts with concurrency checks
            foreach (var item in objBank)
            {
                if (bankDict.TryGetValue(new { item.Company, item.Reference }, out var objBank_))
                {
                    // Update account
                    objBank_.Salary = 0;
                    objBank_.IsActive = false;
                    unitOfWork.Update(objBank_);

                    // Update career bank list
                    var objBankCareer = unitOfWork.Set<HREmpCareerBankList>()
                        .FirstOrDefault(w => w.Company == item.Company
                                            && w.EmpCode == objMatchHeader.EmpCode
                                            && w.Reference == item.Reference);

                    if (objBankCareer != null)
                    {

                        objBankCareer.Todate = HeaderCareer.EffectDate.Value.AddDays(-1);
                        unitOfWork.Update(objBankCareer);
                        ListCareerBankList.Add(objBankCareer);
                    }
                }
            }

            // 3. Create new bank records
            foreach (var items in ListCareerBankList)
            {
                var objBank_ = objBank.FirstOrDefault(w => w.Company == items.Company);
                if (objBank_ == null) continue;

                // Create career bank list entry
                var empIden = new HREmpCareerBankList
                {
                    OldSalary = items.OldSalary,
                    Increase = items.Increase,
                    NewSalary = items.NewSalary,
                    Company = HeaderCareer.CompanyCode,
                    Reference = TranNoCareer,
                    EmpCode = objMatchHeader.EmpCode,
                    FromDate = HeaderCareer.EffectDate,
                    Todate = new DateTime(5000, 1, 1)
                };
                unitOfWork.Add(empIden);

                // Create new bank account
                var bankAcc = new HREmpBankAcc
                {
                    EmpCode = objMatchHeader.EmpCode,
                    Salary = items.NewSalary ?? 0,
                    Reference = TranNoCareer,
                    Company = items.Company,
                    BankName = objBank_.BankName ?? " ",
                    AccountNo = objBank_.AccountNo ?? " ",
                    AccountName = objBank_.AccountName ?? " ",
                    IsTax = objBank_.IsTax ?? false,
                    IsNSSF = objBank_.IsNSSF ?? false,
                    Association = objBank_.Association ?? false,
                    IsActive = true
                };
                unitOfWork.Add(bankAcc);
            }
        }
        private void NotifyRequestor(HREFEmpResign record, string fileName, List<ExDocApproval> approvals, bool IsResign)
        {
            string Type = "Terminate";
            if (IsResign)
                Type = "Resign";
            var staffView = unitOfWork.Set<HRStaffProfile>()
                .FirstOrDefault(w => w.EmpCode == record.EmpCode);

            if (staffView == null) return;

            // 1. Send email to requestor
            var EmpCreate = unitOfWork.Set<HRStaffProfile>()
                .FirstOrDefault(s => s.EmpCode == record.CreatedBy);

            //var Applevel = unitOfWork.Set<ExDocApproval>()
            //    .FirstOrDefault(w => w.DocumentNo == record.DocumentNo
            //                        && w.Status == SYDocumentStatus.APPROVED.ToString()
            //                        && w.DocumentType == record.DocType);
            var Applevel = approvals.FirstOrDefault();

            if (Applevel == null) return;

            var Approve = unitOfWork.Set<HRStaffProfile>()
                .FirstOrDefault(w => w.EmpCode == Applevel.Approver);

            if (EmpCreate != null && !string.IsNullOrEmpty(EmpCreate.Email))
            {
                var EmailConf = unitOfWork.Set<CFEmailAccount>().FirstOrDefault();
                if (EmailConf != null)
                {
                    string subject = $"Request {Type} Approved";
                    string body = $@"Dear <b>{EmpCreate.Title} {EmpCreate.AllName}</b>
                <br/><br/>Your {Type} request for:
                <br/><b>{staffView.Title} {staffView.AllName}</b>
                <br/>has been approved by: <b>{Approve?.AllName}</b>
                <br/><br/>Yours sincerely,
                <br/><br/><b>{Approve?.AllName}</b>";

                    new ClsEmail().SendMail(EmailConf, "", EmpCreate.Email, subject, body, fileName, Path.GetFileName(fileName));
                }
            }

            // 2. Send Telegram notification
            var excfObject = unitOfWork.Set<ExCfWorkFlowItem>()
                .FirstOrDefault(w => w.ScreenID == ScreenId);

            if (excfObject != null && !string.IsNullOrEmpty(excfObject.Telegram))
            {
                string message = $@"Dear {EmpCreate.Title} {EmpCreate.AllName}
            %0AYour {Type} request for: {staffView.AllName}
            %0Ahas been approved by: {Approve.AllName}
            %0A%0AYours sincerely,
            %0A{Approve.AllName}";

                SYSendTelegramObject Tel = new SYSendTelegramObject { User = User, BS = BS };
                WorkFlowResult result = Tel.Send_SMS_Telegram(message, excfObject.Telegram, false);
            }
        }
        #endregion
        public class ClsReuestTerminat
        {
            public int ID { get; set; }
            public string DocumentNo { get; set; }
            public string EmpCode { get; set; }
            public string EmpName { get; set; }
            public string Department { get; set; }
            public string Position { get; set; }
            public string Branch { get; set; }
            public DateTime DocDate { get; set; }
            public string Section { get; set; }
            public DateTime ResigneDate { get; set; }
            public string Reason { get; set; }
            public string Status { get; set; }
            public string ResignType { get; set; }
            public string CreatedBy { get; set; }
            public string CreatedOn { get; set; }

        }
    }
}