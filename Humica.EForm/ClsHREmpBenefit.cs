using Humica.Core.DB;
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
    public class ClsHREmpBenefit : IClsHREmpBenefit
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public HREFBenefit Header { get; set; }
        public string DocType { get; set; }
        public string MessageError { get; set; }
        public List<HREFBenefit> ListHeader { get; set; }
        public List<ClsRequestBenefit> ListRequestPending { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<HR_STAFF_VIEW> ListStaffView { get; set; }
        public ClsEmail EmailObject { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsHREmpBenefit()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void ProcessSupense(string userName)
        {
            OnLoad();
            string pending = SYDocumentStatus.PENDING.ToString();
            string open = SYDocumentStatus.OPEN.ToString();
            string approved = SYDocumentStatus.APPROVED.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();

            var listReqSupense = unitOfWork.Set<HREFBenefit>().AsQueryable()
                .Where(w => w.Status == pending)
                .ToList();

            var listApp = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                .Where(w => listReqSupense.AsEnumerable().Where(x => w.DocumentNo == x.DocNo && w.DocumentType == x.DocType && w.Approver == userName).Any())
                .ToList();

            foreach (var item in listApp)
            {

                if (item.ApproveLevel == 1 && item.Status == open)
                {
                    var EmpStaff = listReqSupense.Where(w => w.DocNo == item.DocumentNo).ToList();
                    if (!EmpStaff.Any()) continue;
                    AddPendingToList(item, EmpStaff);
                }
                else if (item.ApproveLevel > 1 && item.Status == open)
                {
                    var level = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == item.DocumentNo && w.DocumentType == item.DocumentType && w.ApproveLevel < item.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                    if (level != null) continue;
                    var EmpStaff = listReqSupense.Where(w => w.DocNo == item.DocumentNo).ToList();
                    if (!EmpStaff.Any()) continue;
                    AddPendingToList(item, EmpStaff);
                }
            }

            var ListLeaveCreater = unitOfWork.Set<HREFBenefit>().AsQueryable().AsEnumerable()
                    .Where(x => unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                        .Any(w => x.DocNo == w.DocumentNo
                                   && x.DocType == w.DocumentType && w.Approver == userName && x.Status != open && x.Status != Cancel)
                        || x.CreatedBy == userName)
                    .ToList();
            ListHeader = ListLeaveCreater.ToList();
        }
        private void AddPendingToList(ExDocApproval item, List<HREFBenefit> listReqSupense)
        {
            foreach (var read in listReqSupense)
            {
                var ListPen = new ClsRequestBenefit();
                var empSupense = listReqSupense.FirstOrDefault(w => w.DocNo == item.DocumentNo);
                if (empSupense != null)
                {
                    ListPen.DocNo = empSupense.DocNo;
                    ListPen.EmpCode = empSupense.EmpCode;
                    ListPen.EmpName = empSupense.EmpName;
                    ListPen.Section = empSupense.Section;
                    ListPen.Department = empSupense.Department;
                    ListPen.Position = empSupense.Position;
                    ListPen.Description = empSupense.Description;
                    ListPen.FromDate = empSupense.FromDate;
                    ListPen.ToDate = empSupense.ToDate;
                    ListPen.Status = empSupense.Status;
                    ListRequestPending.Add(ListPen);
                }
            }

        }
        public string CreateSuspen()
        {
            OnLoad();
            try
            {
                if (string.IsNullOrEmpty(Header.Description)) return "REASON";
                if (Header.NewAmount == 0|| Header.Increase == 0) return "INVALIDED_AMOUNT";
                if (string.IsNullOrEmpty(Header.BenefitType)) return "INVALIEDED_BENEFITEYPE";
                if (!ListApproval.Any()) return "INVALIDED_APPROVER";
                if (Header.FromDate == null || Header.FromDate <= DateTime.MinValue) return "INVALIDED_FROMDATE";
                if (Header.ToDate == null || Header.ToDate <= DateTime.MinValue) return "INVALIDED_TODATE";
                if (Header.FromDate > Header.ToDate) return "INVALIDED_DATE";
                string Status = SYDocumentStatus.OPEN.ToString();
                var staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                if (staff == null) return "INVALID_EMP";
                var objNumber = new CFNumberRank(DocType, staff.CompanyCode, Header.FromDate.Year, true);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.DocNo = objNumber.NextNumberRank;
                Header.DocType = DocType;
                Header.EmpName = HeaderStaff.AllName;
                Header.Position = staff.JobCode;
                Header.Section = staff.SECT;
                Header.Department = staff.DEPT;
                Header.Status = Status;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;

                unitOfWork.Add(Header);
                foreach (var read in ListApproval)
                {
                    if (read.Approver == Header.CreatedBy) continue;
                    var objApp = new ExDocApproval();
                    objApp.DocumentNo = Header.DocNo;
                    objApp.DocumentType = DocType;
                    objApp.Status = SYDocumentStatus.OPEN.ToString();
                    objApp.Approver = read.Approver;
                    objApp.ApproverName = read.ApproverName;
                    objApp.ApproveLevel = read.ApproveLevel;
                    objApp.WFObject = "CHSH";
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
        public string RequestForApprove(string Doc, string fileName, string URL)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HREFBenefit>().AsQueryable().FirstOrDefault(w => w.DocNo == Doc);
                if (objMatch == null) return "DOC_INV";
                string open = SYDocumentStatus.OPEN.ToString();
                var ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentType == objMatch.DocType
                                   && w.DocumentNo == objMatch.DocNo && w.Status == open).OrderBy(w => w.ApproveLevel).ToList().FirstOrDefault();
                if (ListApproval == null) return "INVALID_APPROVER";
                objMatch.Status = SYDocumentStatus.PENDING.ToString();
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
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
                                string str = $"Dear <b>{StaffApp.Title} {StaffApp.AllName}</b>";
                                str += $" <br /> I would like to Request Benefit <br />from <b>{objMatch.FromDate.ToString("dd/MMM/yyyy")} </b>";
                                str += $" to <b> {objMatch.ToDate.ToString("dd/MMM/yyyy")} </b>";
                                str += $"<br />  Reason: <b> {objMatch.Description} </b> <br /> Please review and approve with favor.";
                                str += $"<br /> <br /> Yours sincerely,<br /> <br /> <b>{objMatch.EmpName}</b>";

                                // Prepare email details
                                string subject = $"Request Benefit";
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
                        string str = $" I would like to Request Benefit %0A from <b>{objMatch.FromDate.ToString("dd/MMM/yyyy")} </b>";
                        str += $" to <b> {objMatch.ToDate.ToString("dd/MMM/yyyy")} </b>";
                        str += $"%0A Reason: <b> {objMatch.Description} </b> %0A Please review and approve with favor.";
                        str += $"%0A%0AYours sincerely,%0A%0A<b>{objMatch.EmpName}</b>";

                        // Initialize Telegram sender
                        SYSendTelegramObject Tel = new SYSendTelegramObject
                        {
                            User = User,
                            BS = BS
                        };

                        // Send the Telegram message
                        WorkFlowResult result1 = Tel.Send_SMS_Telegram(str, excfObject.Telegram, false);
                        MessageError = Tel.getErrorMessage(result1);
                    }
                    #endregion
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Doc, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Doc, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Doc, SYActionBehavior.ADD.ToString(), e, true);
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
        public string Edit(string Doc)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var objMatch = unitOfWork.Set<HREFBenefit>().AsQueryable().FirstOrDefault(w => w.DocNo == Doc);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                var listApprovalDoc = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == Doc && w.DocumentType == objMatch.DocType).ToList();
                if (listApprovalDoc.Any())
                    unitOfWork.BulkDelete(listApprovalDoc);
                foreach (var read in ListApproval)
                {
                    if (read.Approver == Header.CreatedBy) continue;
                    var objApp = new ExDocApproval();
                    objApp.DocumentNo = Doc;
                    objApp.DocumentType = DocType;
                    objApp.Status = SYDocumentStatus.OPEN.ToString();
                    objApp.Approver = read.Approver;
                    objApp.ApproverName = read.ApproverName;
                    objApp.ApproveLevel = read.ApproveLevel;
                    objApp.WFObject = "CHSH";
                    objApp.ApprovedBy = "";
                    objApp.ApprovedName = "";
                    objApp.LastChangedDate = DateTime.Now;
                    unitOfWork.Add(objApp);
                }
                objMatch.FromDate = Header.FromDate;
                objMatch.ToDate = Header.ToDate;
                objMatch.Description = Header.Description;
                objMatch.CreatedBy = User.UserName;
                objMatch.CreatedOn = DateTime.Now.Date;

                unitOfWork.Update(objMatch);

                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Doc, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Doc, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Doc, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Delete(string Doc)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var objMatch = unitOfWork.Set<HREFBenefit>().AsQueryable().FirstOrDefault(w => w.DocNo == Doc);
                if (objMatch == null)
                {
                    return "DOC_RM";
                }
                var listApprovalDoc = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == Doc && w.DocumentType == objMatch.DocType).ToList();
                if (listApprovalDoc.Any())
                {
                    unitOfWork.BulkDelete(listApprovalDoc);
                }
                unitOfWork.Delete(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Doc, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Doc, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Doc, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Reject(string Document)
        {
            OnLoad();
            try
            {
                string Reject = SYDocumentStatus.REJECTED.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                string[] c = Document.Split(';');
                foreach (var Doc in c)
                {
                    if (Doc == "") continue;
                    var objmatch = unitOfWork.Set<HREFBenefit>().AsQueryable().FirstOrDefault(w => w.DocNo == Doc);
                    if (objmatch == null)
                    {
                        return "INV_EN";
                    }
                    var _obj = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentType == objmatch.DocType && w.DocumentNo == Doc && w.Approver == User.UserName && w.Status == Open);
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
                    //var Applevel = unitOfWork.Set<ExDocApproval>().AsQueryable().FirstOrDefault(w => w.DocumentNo == objmatch.DocNo && w.DocumentType == objmatch.DocType && w.Status == Reject);
                    var Approve = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == User.UserName);
                    if (EmpCreate != null && !string.IsNullOrEmpty(EmpCreate.Email) && StaffView != null)
                    {
                        var EmailConf = unitOfWork.Set<CFEmailAccount>().AsQueryable().FirstOrDefault();
                        if (EmailConf != null)
                        {
                            CFEmailAccount emailAccount = EmailConf;
                            string subject = $"Request Benefit";
                            string body = $"Dear <b> {StaffView.Title} {objmatch.EmpName} </b> <br />";
                            body += $"<br /> I’m reject to the  Benefit request for  <b> {StaffView.Title} {objmatch.EmpName}</b> ";
                            body += $"<br /> <br /> Yours sincerely,<br /> <br /> <b>{Approve.AllName}</b>";
                            string filePath = "";// fileName; // Ensure fileName is defined
                            string fileName_ = ""; //Path.GetFileName(filePath);

                            EmailObject = new ClsEmail();
                            int rs = EmailObject.SendMail(emailAccount, "", EmpCreate.Email, subject, body, filePath, fileName_);
                        }
                    }
                    var excfObject = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                    if (excfObject != null && !string.IsNullOrEmpty(excfObject.Telegram))
                    {
                        string str = $" Dear <b> {StaffView.Title} {objmatch.EmpName} </b> %0A";
                        str += $" %0A I’m reject to the  Benefit request for %0A <b>{StaffView.Title} {objmatch.EmpName}</b>";
                        str += $"%0A%0AYours sincerely,%0A%0A<b> {Approve.AllName} </b>";

                        // Initialize Telegram sender
                        SYSendTelegramObject Tel = new SYSendTelegramObject
                        {
                            User = User,
                            BS = BS
                        };

                        WorkFlowResult result1 = Tel.Send_SMS_Telegram(str, excfObject.Telegram, false);
                        MessageError = Tel.getErrorMessage(result1);
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Document, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Document, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Document, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Cancel(string OTRNo)
        {
            OnLoad();
            try
            {
                string[] c = OTRNo.Split(';');
                foreach (var ID in c)
                {
                    var objMatch = unitOfWork.Set<HREFBenefit>().AsQueryable().FirstOrDefault(w => w.DocNo == ID);
                    if (objMatch == null)
                    {
                        return "INVALIED";
                    }
                    objMatch.Status = SYDocumentStatus.CANCELLED.ToString();
                    objMatch.ChangedBy = User.UserName;
                    objMatch.ChangedOn = DateTime.Now;
                    unitOfWork.Update(objMatch);
                    unitOfWork.Save();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, OTRNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, OTRNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, OTRNo, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string approveTheDoc(string Inc, string fileName, string URL)
        {
            OnLoad();
            string latestCareer = SYConstant.OK;
            unitOfWork.BeginTransaction();
            try
            {
                latestCareer = ProcessApproval(Inc, fileName, URL);
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
        private string ProcessApproval(string Inc, string fileName, string URL)
        {
            var obj = unitOfWork.Set<HREFBenefit>()
                .FirstOrDefault(x => x.DocNo == Inc);

            if (obj == null) return "INVALID_DOC";

            string openStatus = SYDocumentStatus.OPEN.ToString();
            var approvalList = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                .Where(w => w.DocumentType == obj.DocType &&
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
            NotifyNextApprover(obj, objapprover, fileName, openStatus);
            UpdateStatus(obj, approvalList, openStatus);

            if (obj.Status == SYDocumentStatus.APPROVED.ToString())
            {
                ProcessHeader(obj, fileName, objapprover);
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
        private void NotifyNextApprover(HREFBenefit record, List<ExDocApproval> approvals, string fileName, string openStatus)
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

            SendApprovalEmail(record, staffProfile, fileName);
        }
        private void SendApprovalEmail(HREFBenefit record, HRStaffProfile approver, string fileName)
        {
            try
            {
                var emailConfig = unitOfWork.Set<CFEmailAccount>().FirstOrDefault();
                if (emailConfig == null) return;

                var emailBody = $@"Dear {approver.Title} {approver.AllName}
            <br/><br/>I would like to Request Benefit 
            <br/>from <b>{record.FromDate.ToString("dd/MMM/yyyy")}</b>
            <br/>to <b>{record.ToDate.ToString("dd/MMM/yyyy")}</b>
            <br/><b>Reason:</b> {record.Description}
            <br/><br/>Please review and approve with favor.
            <br/><br/>Yours sincerely,
            <br/><br/><b>{record.CreatedBy}</b>";
                var Subject = $"Request Benefit";
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
        private void UpdateStatus(HREFBenefit record, List<ExDocApproval> approvals, string openStatus)
        {
            record.Status = approvals.Any(w => w.Status == openStatus)
                ? SYDocumentStatus.PENDING.ToString()
                : SYDocumentStatus.APPROVED.ToString();

            unitOfWork.Update(record);
        }
        private void ProcessHeader(HREFBenefit record, string fileName, List<ExDocApproval> approvals)
        {
            var staff = unitOfWork.Set<HRStaffProfile>()
                .FirstOrDefault(w => w.EmpCode == record.EmpCode);
            if (staff == null) throw new Exception("STAFF_NOT_FOUND");

            UpdateBenefit(record, staff);
            NotifyRequestor(record, fileName, approvals);
        }
        private void UpdateBenefit(HREFBenefit record, HRStaffProfile staff)
        {
            var rewardType = unitOfWork.Set<PR_RewardsType>()
                .AsQueryable()
                .FirstOrDefault(w => w.Code == record.BenefitType);

            if (rewardType != null)
            {
                if (rewardType.ReCode == "ALLW")
                {
                    AddAllowance(record, staff, rewardType);
                }
                else if (rewardType.ReCode == "BON")
                {
                    AddBonus(record, staff, rewardType);
                }
            }
        }
        private void AddAllowance(HREFBenefit record, HRStaffProfile staff, PR_RewardsType rewardType)
        {
            var lastTran = unitOfWork.Set<PREmpAllw>()
                .AsQueryable()
                .OrderByDescending(u => u.TranNo)
                .FirstOrDefault();

            var allowance = new PREmpAllw
            {
                CompanyCode = staff.CompanyCode,
                AllwCode = record.BenefitType,
                AllwDescription = rewardType.Description,
                FromDate = record.FromDate,
                ToDate = record.ToDate,
                Status = 0,
                LCK = 0,
                EmpCode = staff.EmpCode,
                EmpName = staff.AllName,
                Amount = record.NewAmount,
                Remark = record.Remark,
                CreateBy = record.CreatedBy,
                CreateOn = record.ChangedOn,
                TranNo = (lastTran?.TranNo ?? 0) + 1
            };

            unitOfWork.Add(allowance);
        }
        private void AddBonus(HREFBenefit record, HRStaffProfile staff, PR_RewardsType rewardType)
        {
            var lastTran = unitOfWork.Set<PREmpBonu>()
                .AsQueryable()
                .OrderByDescending(u => u.TranNo)
                .FirstOrDefault();

            var bonus = new PREmpBonu
            {
                CompanyCode = staff.CompanyCode,
                BonCode = record.BenefitType,
                BonDescription = rewardType.Description,
                FromDate = record.FromDate,
                ToDate = record.ToDate,
                Status = 0,
                LCK = 0,
                EmpCode = staff.EmpCode,
                EmpName = staff.AllName,
                Amount = record.NewAmount,
                Remark = record.Remark,
                CreateBy = record.CreatedBy,
                CreateOn = record.ChangedOn,
                TranNo = (lastTran?.TranNo ?? 0) + 1
            };

            unitOfWork.Add(bonus);
        }
        private void NotifyRequestor(HREFBenefit record, string fileName, List<ExDocApproval> approvals)
        {
            var staffView = unitOfWork.Set<HRStaffProfile>()
                .FirstOrDefault(w => w.EmpCode == record.EmpCode);

            if (staffView == null) return;

            // 1. Send email to requestor
            var EmpCreate = unitOfWork.Set<HRStaffProfile>()
                .FirstOrDefault(s => s.EmpCode == record.CreatedBy);

            var Applevel = approvals.FirstOrDefault();

            if (Applevel == null) return;

            var Approve = unitOfWork.Set<HRStaffProfile>()
                .FirstOrDefault(w => w.EmpCode == Applevel.Approver);

            if (EmpCreate != null && !string.IsNullOrEmpty(EmpCreate.Email))
            {
                var EmailConf = unitOfWork.Set<CFEmailAccount>().FirstOrDefault();
                if (EmailConf != null)
                {
                    string subject = $"Request Benefit";
                    string body = $@"Dear <b>{EmpCreate.Title} {EmpCreate.AllName}</b>
                <br/><br/>I’m approved to the Benefit Request for:
                <br/><b>{staffView.Title} {staffView.AllName}</b>
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
            %0AI’m approved to the Benefit Request for: {staffView.AllName}
            %0A%0AYours sincerely,
            %0A{Approve.AllName}";

                SYSendTelegramObject Tel = new SYSendTelegramObject { User = User, BS = BS };
                WorkFlowResult result = Tel.Send_SMS_Telegram(message, excfObject.Telegram, false);
            }
        }
        public class ClsRequestBenefit
        {
            public long TranNo { get; set; }
            public string EmpCode { get; set; }
            public string EmpName { get; set; }
            public string DocNo { get; set; }
            public string Description { get; set; }
            public string Department { get; set; }
            public string Section { get; set; }
            public string Position { get; set; }
            public string Status { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public string AttachFile { get; set; }

        }
    }
}