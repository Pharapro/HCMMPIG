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
    public class ClsRequestChangShift: IClsRequestChangShift
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public FTINYear FInYear { get; set; }
        public string DocType { get; set; }
        public HREFReqChangShift Header { get; set; }
        public List<HREFReqChangShift> listHeader { get; set; }
        public List<ClsRequestChangeShift> ListRequestPending { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public HRStaffProfile Staff { get; set; }
        public List<HR_STAFF_VIEW> ListStaffView { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public SYUser UserObject { get; set; }
        public SYUserBusiness UserBusinessObject { get; set; }
        public CFEmailAccount EmailAccount { get; set; }
        public ClsEmail EmailObject { get; private set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsRequestChangShift()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void ProcessChangeShift(string userName)
        {
            OnLoad();
            string pending = SYDocumentStatus.PENDING.ToString();
            string open = SYDocumentStatus.OPEN.ToString();
            string approved = SYDocumentStatus.APPROVED.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();
            var obj = unitOfWork.Set<HREFReqChangShift>().AsQueryable().Where(x => x.Status == pending).ToList();
            var ListApp = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable().Where(w => obj.Where(x => x.Increment == w.DocumentNo && x.DocType == w.DocumentType && w.Approver == userName).Any()).ToList();
            foreach (var read in ListApp)
            {
                var objemp = new ClsRequestChangeShift();
                if (read.ApproveLevel > 1 && read.Status == open)
                {
                    var level = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == read.DocumentNo && w.DocumentType == read.DocumentType && w.ApproveLevel < read.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                    if (level != null) continue;
                    var EmpStaff = obj.Where(w => w.Increment == read.DocumentNo).ToList();
                    if (EmpStaff == null) continue;
                    AddPendingToList(read, EmpStaff);
                }
                else if (read.ApproveLevel == 1 && read.Status == open)
                {
                    var EmpStaff = obj.Where(w => w.Increment == read.DocumentNo).ToList();
                    if (EmpStaff == null) continue;
                    AddPendingToList(read, EmpStaff);
                }

            }
            var ListLeaveCreater = unitOfWork.Set<HREFReqChangShift>().AsQueryable().AsEnumerable()
                              .Where(x => unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                                  .Any(w => x.Increment == w.DocumentNo
                                             && x.DocType == w.DocumentType && w.Approver == userName && x.Status != open && x.Status != Cancel)
                                  || x.CreatedBy == userName)
                              .ToList();
            listHeader = ListLeaveCreater.ToList();
        }
        private void AddPendingToList(ExDocApproval item, List<HREFReqChangShift> listReqTransfer)
        {
            foreach (var read in listReqTransfer)
            {
                var ListPen = new ClsRequestChangeShift();
                var empTransfer = listReqTransfer.FirstOrDefault(w => w.Increment == item.DocumentNo);
                if (empTransfer != null)
                {
                    ListPen.ID = empTransfer.ID;
                    ListPen.Increment = empTransfer.Increment;
                    ListPen.EmpCode = empTransfer.EmpCode;
                    ListPen.EmpName = empTransfer.EmpName;
                    ListPen.Section = empTransfer.Section;
                    ListPen.Department = empTransfer.Department;
                    ListPen.Position = empTransfer.Position;
                    ListPen.Reason = empTransfer.Reason;
                    ListPen.StartTime = empTransfer.StartTime;
                    ListPen.EndTime = empTransfer.EndTime;
                    ListPen.Status = empTransfer.Status;
                    ListRequestPending.Add(ListPen);
                }
            }
        }
        public string CreateReqChangShift()
        {
            OnLoad();
            try
            {
                string Status = SYDocumentStatus.OPEN.ToString();
                Header.EmpCode = HeaderStaff.EmpCode;
                if (string.IsNullOrEmpty(Header.EmpCode)) return "INVALID_EMP";
                if (string.IsNullOrEmpty(Header.DutyRequest)|| string.IsNullOrEmpty(Header.CurrentDuty)) return "INVALID_DUTY";
                var staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                if (staff == null) return "INVALID_EMP";
                if (!ListApproval.Any()) return "IVALIDED_APPROVER";
                if (string.IsNullOrEmpty(Header.Reason)) return "REASON";
                if (Header.RequestDate == null || Header.RequestDate <= DateTime.MinValue) return "INVALIDED_EFFECTDATE";
                //if (Header.StartTime == null || Header.StartTime <= DateTime.MinValue
                //    || Header.EndTime == null || Header.EndTime <= DateTime.MinValue) return "INVALIDED_TIME";
                var objNumber = new CFNumberRank(DocType, staff.CompanyCode, Header.RequestDate.Date.Year, true);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.Increment = objNumber.NextNumberRank;
                Header.DocType = DocType;
                Header.EmpName = HeaderStaff.AllName;
                Header.Position = staff.JobCode;
                Header.Section = staff.SECT;
                Header.Department = staff.DEPT;
                Header.StartDate = HeaderStaff.StartDate;
                if (Header.StartTime.HasValue && Header.EndTime.HasValue)
                {
                    Header.StartTime = Header.RequestDate.Date + Header.StartTime.Value.TimeOfDay;
                    Header.EndTime = Header.RequestDate.Date + Header.EndTime.Value.TimeOfDay;
                }
                Header.Status = Status;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;

                unitOfWork.Add(Header);
                foreach (var read in ListApproval)
                {
                    if (read.Approver == Header.CreatedBy) continue;
                    var objApp = new ExDocApproval();
                    objApp.DocumentNo = Header.Increment;
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
        public string RequestForApprove(string Inc, string fileName, string URL)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HREFReqChangShift>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.Increment == Inc);
                if (objMatch == null) return "DOC_INV";
                string open = SYDocumentStatus.OPEN.ToString();
                var ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable().Where(w => w.DocumentType == objMatch.DocType
                                   && w.DocumentNo == objMatch.Increment && w.Status == open).OrderBy(w => w.ApproveLevel).ToList().FirstOrDefault();
                if (ListApproval == null) return "INVALID_APPROVER";
                objMatch.Status = SYDocumentStatus.PENDING.ToString();
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                var StaffApp = unitOfWork.Set<HRStaffProfile>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.EmpCode == ListApproval.Approver);
                if (StaffApp != null)
                {
                    //DateTime currectdate = DateTime.Now;
                    //var EmpSchedule = unitOfWork.Set<ATEmpSchedule>().AsQueryable().AsEnumerable()
                    //        .Where(w => w.EmpCode == objMatch.EmpCode && w.TranDate <= currectdate && w.Flag1 == 1 && string.IsNullOrEmpty(w.LeaveCode))
                    //        .OrderByDescending(w => w.TranDate).FirstOrDefault();
                    //string remark = "";
                    //if (EmpSchedule != null)
                    //{
                    //    var ATShift = unitOfWork.Set<ATShift>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.Code == EmpSchedule.SHIFT);
                    //    if (ATShift != null) remark = ATShift.Remark;
                    //}
                    #region Email
                    if (!string.IsNullOrEmpty(StaffApp.Email))
                    {
                        try
                        {
                            // Retrieve email configuration
                            var EmailConf = unitOfWork.Set<CFEmailAccount>().AsQueryable().AsEnumerable().FirstOrDefault();

                            if (EmailConf != null)
                            {
                                CFEmailAccount emailAccount = EmailConf;
                                string subject = "Request Change Shift";
                                var body = $@"Dear {StaffApp.Title} {StaffApp.AllName}
                                 <br/><br/>I would like to Request Change Shift
                                 <br/>From <b> {objMatch.CurrentDuty}</b> To <b> {objMatch.DutyRequest}</b>
                                 <br/><b>Reason:</b> {objMatch.Reason}
                                 <br/><br/>Please review and approve with favor.
                                 <br/><br/>Yours sincerely,
                                 <br/><br/><b>{objMatch.EmpName}</b>";
                                string filePath = fileName;
                                string fileName_ = Path.GetFileName(fileName);

                                EmailObject = new ClsEmail();
                                List<string> filePathsList = new List<string> { filePath };
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
                    var excfObject = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.ScreenID == ScreenId);

                    if (excfObject != null && !string.IsNullOrEmpty(excfObject.Telegram))
                    {
                        string message = "I would like to Request Change Shift %0A" +
                             $"From: {objMatch.CurrentDuty} To: {objMatch.DutyRequest} %0A" +
                             $"Reason: {objMatch.Reason} %0A" +
                             "Please review and approve with favor. %0A%0A" +
                             "Yours sincerely,%0A" +
                             $"{objMatch.EmpName}";

                        // Initialize Telegram sender
                        SYSendTelegramObject Tel = new SYSendTelegramObject
                        {
                            User = User,
                            BS = BS
                        };

                        // Send the Telegram message
                        WorkFlowResult result1 = Tel.Send_SMS_Telegram(message, excfObject.Telegram, false);
                        MessageError = Tel.getErrorMessage(result1);
                    }
                    #endregion
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Inc, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Inc, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Inc, SYActionBehavior.ADD.ToString(), e, true);
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
                    var objmatch = unitOfWork.Set<HREFReqChangShift>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.Increment == Doc);
                    if (objmatch == null)
                    {
                        return "INV_EN";
                    }
                    var _obj = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable().Where(w => w.DocumentType == objmatch.DocType && w.DocumentNo == Doc && w.Approver == User.UserName && w.Status == Open);
                    if (!_obj.Any()) return "USER_CANNOT_REJECT";
                    foreach (var read in _obj)
                    {
                        read.Status = Reject;
                        read.LastChangedDate = DateTime.Now;
                        read.ApprovedBy = User.UserName;
                        var appName = unitOfWork.Set<HRStaffProfile>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.EmpCode == User.UserName);
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
                    var EmpCreate = unitOfWork.Set<HRStaffProfile>().AsQueryable().AsEnumerable().FirstOrDefault(s => s.EmpCode == objmatch.CreatedBy);
                    var StaffView = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.EmpCode == objmatch.EmpCode);
                    //var Applevel = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.DocumentNo == objmatch.Increment && w.DocumentType == objmatch.DocType && w.Status == Reject);
                    var Approve = unitOfWork.Set<HRStaffProfile>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.EmpCode == User.UserName);
                    if (EmpCreate != null && !string.IsNullOrEmpty(EmpCreate.Email) && StaffView != null)
                    {
                        var EmailConf = unitOfWork.Set<CFEmailAccount>().AsQueryable().AsEnumerable().FirstOrDefault();
                        if (EmailConf != null)
                        {
                            CFEmailAccount emailAccount = EmailConf;
                            string subject = "Request Change Shift";
                            string body = $"Dear <b> {StaffView.Title} {objmatch.EmpName} </b> <br />";
                            body += $"<br /> I’m reject to the  Change Shift request for  <b> {StaffView.Title} {objmatch.EmpName}</b> ";
                            body += $"<br /> <br /> Yours sincerely,<br /> <br /> <b>{Approve.AllName}</b>";
                            string filePath = "";// fileName; // Ensure fileName is defined
                            string fileName_ = ""; //Path.GetFileName(filePath);

                            EmailObject = new ClsEmail();
                            int rs = EmailObject.SendMail(emailAccount, "", EmpCreate.Email, subject, body, filePath, fileName_);
                        }
                    }
                    var excfObject = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.ScreenID == ScreenId);
                    if (excfObject != null && !string.IsNullOrEmpty(excfObject.Telegram))
                    {
                        string str = $" Dear <b> {StaffView.Title} {objmatch.EmpName} </b> %0A";
                        str += $" %0A I’m reject to the  Change Shift request for %0A <b>{StaffView.Title} {objmatch.EmpName}</b>";
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
            try
            {
                string[] c = OTRNo.Split(';');
                foreach (var ID in c)
                {
                    var objMatch = unitOfWork.Set<HREFReqChangShift>().AsQueryable().FirstOrDefault(w => w.Increment == ID);
                    if (objMatch == null)
                    {
                        return "EE001";
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
            var obj = unitOfWork.Set<HREFReqChangShift>()
                .FirstOrDefault(x => x.Increment == Inc );

            if (obj == null) return "INVALID_DOC";

            string openStatus = SYDocumentStatus.OPEN.ToString();
            var approvalList = unitOfWork.Set<ExDocApproval>()
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
            UpdateChangeShiftStatus(obj, approvalList, openStatus);

            if (obj.Status == SYDocumentStatus.APPROVED.ToString())
            {
                NotifyRequestor(obj, fileName, objapprover);
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
        private void NotifyNextApprover(HREFReqChangShift record, List<ExDocApproval> approvals, string fileName, string openStatus)
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

            SendApprovalEmail(record, staffProfile, staffView, fileName);
        }
        private void SendApprovalEmail(HREFReqChangShift record, HRStaffProfile approver, HR_STAFF_VIEW staffView, string fileName)
        {
            try
            {
                //string remark = "";
                //var StaffApp = unitOfWork.Set<HRStaffProfile>().AsQueryable().AsEnumerable().FirstOrDefault(x => x.EmpCode == approver.EmpCode);
                //DateTime currectdate = DateTime.Now;
                //var EmpSchedule = unitOfWork.Set<ATEmpSchedule>().AsQueryable().AsEnumerable()
                //        .Where(w => w.EmpCode == record.EmpCode && w.TranDate <= currectdate && w.Flag1 == 1 && string.IsNullOrEmpty(w.LeaveCode))
                //        .OrderByDescending(w => w.TranDate).FirstOrDefault();
                //if (EmpSchedule != null)
                //{
                //    var ATShift = unitOfWork.Set<ATShift>().AsQueryable().FirstOrDefault(w => w.Code == EmpSchedule.SHIFT);
                //    if (ATShift != null) remark = ATShift.Remark;
                //}
                //if (StaffApp != null)
                //{

                var emailConfig = unitOfWork.Set<CFEmailAccount>().FirstOrDefault();
                if (emailConfig == null) return;

                // Prepare email details
                string subject = "Request Change Shift";
                var emailBody = $@"Dear {approver.Title} {approver.AllName}
                            <br/><br/>I would like to Request Change Shift
                            <br/>From <b> {record.CurrentDuty}</b> To <b> {record.DutyRequest}</b>
                            <br/><b>Reason:</b> {record.Reason}
                            <br/><br/>Please review and approve with favor.
                            <br/><br/>Yours sincerely,
                            <br/><br/><b>{record.EmpName}</b>";

                var email = new ClsEmail();
                List<string> filePathsList = new List<string> { fileName };
                if (!string.IsNullOrEmpty(record.AttachFile))
                    filePathsList.Add(record.AttachFile);
                string[] filePaths = filePathsList.ToArray();
                email.SendMails(emailConfig, "", approver.Email, subject, emailBody, filePaths);
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("FAIL_TO_SEND_MAIL: " + ex.Message);
            }
        }
        private void UpdateChangeShiftStatus(HREFReqChangShift record, List<ExDocApproval> approvals, string openStatus)
        {
            record.Status = approvals.Any(w => w.Status == openStatus)
                ? SYDocumentStatus.PENDING.ToString()
                : SYDocumentStatus.APPROVED.ToString();

            unitOfWork.Update(record);
        }
        private void NotifyRequestor(HREFReqChangShift record, string fileName, List<ExDocApproval> approvals)
        {
            var staffView = unitOfWork.Set<HR_STAFF_VIEW>()
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
                    string subject = "Request Termination Approved";
                    string body = $@"Dear <b>{EmpCreate.Title} {EmpCreate.AllName}</b>
                <br/><br/>Your change shift request:
                <br/>has been approved by: <b>{Approve?.AllName}</b>
                <br/><br/>Yours sincerely,
                <br/><br/><b>{Approve?.AllName}</b>";

                    new ClsEmail().SendMail( EmailConf,"",EmpCreate.Email,subject, body,fileName,Path.GetFileName(fileName));
                }
            }

            // 2. Send Telegram notification
            var excfObject = unitOfWork.Set<ExCfWorkFlowItem>()
                .FirstOrDefault(w => w.ScreenID == ScreenId);

            if (excfObject != null && !string.IsNullOrEmpty(excfObject.Telegram))
            {
                string message = $@"Dear {EmpCreate.Title} {EmpCreate.AllName}
            %0AChange shift request for: {staffView.AllName}
            %0Ahas been approved by: {Approve.AllName}
            %0A%0AYours sincerely,
            %0A{Approve.AllName}";

                SYSendTelegramObject Tel = new SYSendTelegramObject { User = User, BS = BS };
                WorkFlowResult result = Tel.Send_SMS_Telegram(message, excfObject.Telegram, false);
            }
        }
        public class ClsRequestChangeShift
        {
            public int ID { get; set; }
            public string EmpCode { get; set; }
            public string EmpName { get; set; }
            public string Department { get; set; }
            public string Position { get; set; }
            public DateTime? StartDate { get; set; }
            public string Section { get; set; }
            public string Status { get; set; }
            public DateTime? StartTime { get; set; }
            public DateTime? EndTime { get; set; }
            public string Reason { get; set; }
            public DateTime RequestDate { get; set; }
            public string Increment { get; set; }
            public string ApprovedBy { get; set; }
            public DateTime? ApprovedDate { get; set; }

        }
    }
}