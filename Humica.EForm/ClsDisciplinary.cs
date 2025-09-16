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
    public class ClsDisciplinary : IClsDisciplinary
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public FTINYear FInYear { get; set; }
        public string DocType { get; set; }
        public string MessageError { get; set; }
        public HREmpDisciplinary Header { get; set; }
        public HRStaffProfile Staff { get; set; }
        public string CompanyCode { get; set; }
        public string Plant { get; set; }
        public string MessageCode { get; set; }
        public List<ClsEmpDisciplinary> ListRequestPending { get; set; }
        public List<HRStaffProfile> ListStaff { get; set; }
        public List<HREmpDisciplinary> ListHeader { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<HR_STAFF_VIEW> ListStaffView { get; set; }
        public List<ExCfWFApprover> ListEXCFApproval { get; set; }
        public ClsEmail EmailObject { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsDisciplinary()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void LoadData(string userName)
        {
            OnLoad();
            var staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().Where(w => w.EmpCode == userName).ToList();
            if (staff.Any())
            {
                string approved = SYDocumentStatus.APPROVED.ToString();
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                string Pending = SYDocumentStatus.PENDING.ToString();
                var Disc = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().Where(x => x.Status == Pending).ToList();
                var ListApp = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable().Where(w => Disc.Where(x => x.DocumentNo == w.DocumentNo && x.DocType == w.DocumentType && w.Approver == userName).Any()).ToList();
                foreach (var read in ListApp)
                {
                    var objemp = new ClsEmpDisciplinary();
                    if (read.ApproveLevel > 1 && read.Status == Open)
                    {
                        var level = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == read.DocumentNo && w.DocumentType == read.DocumentType && w.ApproveLevel < read.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                        if (level != null) continue;
                        var EmpStaff = Disc.FirstOrDefault(w => w.DocumentNo == read.DocumentNo);
                        if (EmpStaff == null) continue;
                        objemp.EmpCode = EmpStaff.EmpCode;
                        objemp.DocumentNo = EmpStaff.DocumentNo;
                        objemp.AllName = EmpStaff.AllName;
                        objemp.DisciplinaryAction = EmpStaff.DiscAction;
                        objemp.DisciplinayType = EmpStaff.DiscType;
                        objemp.TranDate = EmpStaff.TranDate;
                        objemp.Remark = EmpStaff.Remark;
                        objemp.DescriptionofInfraction = EmpStaff.Consequence;
                        objemp.PlanForImprovement = EmpStaff.Reference;
                        objemp.Status = EmpStaff.Status;
                        ListRequestPending.Add(objemp);
                    }
                    else if (read.ApproveLevel == 1 && read.Status == Open)
                    {
                        var EmpStaff = Disc.FirstOrDefault(w => w.DocumentNo == read.DocumentNo);
                        if (EmpStaff == null) continue;
                        objemp.EmpCode = EmpStaff.EmpCode;
                        objemp.DocumentNo = EmpStaff.DocumentNo;
                        objemp.AllName = EmpStaff.AllName;
                        objemp.DisciplinaryAction = EmpStaff.DiscAction;
                        objemp.DisciplinayType = EmpStaff.DiscType;
                        objemp.TranDate = EmpStaff.TranDate;
                        objemp.Remark = EmpStaff.Remark;
                        objemp.DescriptionofInfraction = EmpStaff.Consequence;
                        objemp.PlanForImprovement = EmpStaff.Reference;
                        objemp.Status = EmpStaff.Status;
                        ListRequestPending.Add(objemp);
                    }

                }
                var ListLeaveCreater = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().AsEnumerable()
                                 .Where(x => unitOfWork.Set<ExDocApproval>().AsQueryable()
                                     .Any(w => x.DocumentNo == w.DocumentNo
                                                && x.DocType == w.DocumentType && w.Approver == userName)
                                     || x.CreatedBy == userName)
                                 .ToList();
                ListHeader = ListLeaveCreater.ToList();
            }
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
        public string CreateEmpDiscp(string URL, bool IsDoc = false)
        {
            OnLoad();
            try
            {
                var StaffReq = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                if (StaffReq == null) return "INVALID_EMP";
                string Status = SYDocumentStatus.OPEN.ToString();
                if (Header.DiscType == null) return "DISC_EN";
                if (Header.DiscAction == null) return "DISC_ACT_EN";
                if (Header.TranDate == DateTime.MinValue || Header.TranDate == null)
                    Header.TranDate = DateTime.Now;
                if (!ListApproval.Any() && IsDoc) return "INVALIED_APPROVER";
                var objNumber = new CFNumberRank(DocType, StaffReq.CompanyCode, Header.TranDate.Year, true);
                if (objNumber == null && IsDoc)
                {
                    return "NUMBER_RANK_NE";
                }
                if (objNumber.NextNumberRank == null && IsDoc)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.DocumentNo = objNumber.NextNumberRank;
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.AllName = HeaderStaff.AllName;
                Header.DocType = DocType;
                Header.Remark = Header.Remark;
                Header.AttachPath = Header.AttachPath;
                Header.AttachDoc = Header.AttachDoc;
                Header.CreatedOn = DateTime.Now;
                Header.CreatedBy = User.UserName;

                unitOfWork.Add(Header);
                foreach (var read in ListApproval)
                {
                    if (read.Approver == Header.CreatedBy) continue;
                    var objApp = new ExDocApproval();
                    objApp.DocumentNo = Header.DocumentNo;
                    objApp.DocumentType = DocType;
                    objApp.Status = SYDocumentStatus.OPEN.ToString();
                    objApp.Approver = read.Approver;
                    objApp.ApproverName = read.ApproverName;
                    objApp.ApproveLevel = read.ApproveLevel;
                    objApp.WFObject = "WAR";
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
        public string EditEmpDiscp(int id)
        {
            OnLoad();
            try
            {
                Header.EmpCode = HeaderStaff.EmpCode;
                var objMatch = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "DISCIPLINAY_NE";
                }

                objMatch.DiscType = Header.DiscType;
                objMatch.TranDate = Header.TranDate;
                objMatch.Remark = Header.Remark;
                objMatch.Reference = Header.Reference;
                objMatch.DiscAction = Header.DiscAction;
                objMatch.Consequence = Header.Consequence;
                objMatch.AttachPath = Header.AttachPath;
                objMatch.ChangedBy = Header.ChangedBy;
                objMatch.ChangedOn = Header.ChangedOn;

                unitOfWork.Update(objMatch);
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
        public string DeleteEmpDiscp(int id)
        {
            OnLoad();
            try
            {
                Header = new HREmpDisciplinary();
                var objMatch = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "DISCIPLINAY_NE";
                }
                Header.TranNo = id;
                unitOfWork.Delete(objMatch);
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
        public HRStaffProfile getNextApprover(string id, string pro)
        {
            var objStaff = new HRStaffProfile();
            string open = SYDocumentStatus.OPEN.ToString();
            var Doc = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().FirstOrDefault(w => w.ScreenID == "ESS0000023");
            var listCanApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == id && w.Status == open && w.DocumentType == Doc.DocType).ToList();

            if (listCanApproval.Count == 0)
            {
                return new HRStaffProfile();
            }

            var min = listCanApproval.Min(w => w.ApproveLevel);
            var NextApp = listCanApproval.Where(w => w.ApproveLevel == min).First();
            objStaff = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == NextApp.Approver);//, objHeader.Property);
            return objStaff;
        }
        public string RequestForApprove(string Doc, string fileName, string URL)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Doc);
                if (objMatch == null) return "DOC_INV";
                string open = SYDocumentStatus.OPEN.ToString();
                var ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentType == objMatch.DocType
                                   && w.DocumentNo == objMatch.DocumentNo && w.Status == open).OrderBy(w => w.ApproveLevel).ToList().FirstOrDefault();
                if (ListApproval == null) return "INVALID_APPROVER";
                objMatch.Status = SYDocumentStatus.PENDING.ToString();
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                var StaffApp = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == ListApproval.Approver);
                var Staff_ = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == objMatch.CreatedBy);
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
                                string str = $"Dear <b> {StaffApp.Title} {StaffApp.AllName}</b>";
                                str += $" <br /> I would like to Request Warning staff  <br /> <b> {objMatch.AllName} </b>";
                                str += $"<br />  Reason: <b> {objMatch.Remark} </b> <br /> Please review and approve with favor.";
                                str += $"<br /> <br /> Yours sincerely,<br /> <br /> <b>{Staff_.AllName}</b>";

                                // Prepare email details
                                string subject = "Request Warning";
                                string body = str; // Use the constructed message as the body
                                string filePath = fileName; // Assuming fileName is defined elsewhere
                                string fileName_ = Path.GetFileName(fileName);

                                // Send the email
                                EmailObject = new ClsEmail();
                                List<string> filePathsList = new List<string> { fileName };
                                if (!string.IsNullOrEmpty(objMatch.AttachPath))
                                    filePathsList.Add(objMatch.AttachPath);
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
                        string str = $" I would like to Request Warning staff %0A <b>{objMatch.AllName} </b>";
                        str += $"%0A Reason: <b>{objMatch.Remark}</b> %0A Please review and approve with favor.";
                        str += $"%0A%0AYours sincerely,%0A%0A<b>{Staff_.AllName}</b>";

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
        public string Cancel(string Doc)
        {
            OnLoad();
            try
            {
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                string Pending = SYDocumentStatus.PENDING.ToString();
                var objmatch = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Doc);
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
                    var objmatch = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Doc);
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
                    //var Applevel = unitOfWork.Set<ExDocApproval>().AsQueryable().FirstOrDefault(w => w.DocumentNo == objmatch.DocumentNo && w.DocumentType == objmatch.DocType && w.Status == Reject);
                    var Approve = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == User.UserName);
                    if (EmpCreate != null && !string.IsNullOrEmpty(EmpCreate.Email) && StaffView != null)
                    {
                        var EmailConf = unitOfWork.Set<CFEmailAccount>().AsQueryable().FirstOrDefault();
                        if (EmailConf != null)
                        {
                            CFEmailAccount emailAccount = EmailConf;
                            string subject = "Request Warning";
                            string body = $"Dear <b> {StaffView.Title} {StaffView.AllName} </b> <br />";
                            body += $"<br /> I’m reject to the  Warning request for  <b> {StaffView.Title} {objmatch.AllName}</b> ";
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
                        string str = $" Dear <b> {StaffView.Title} {StaffView.AllName} </b> %0A";
                        str += $" %0A I’m reject to the  Warning request for %0A <b>{StaffView.Title} {objmatch.AllName}</b>";
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
        public string approveTheDoc(string Inc, string fileName, string URL)
        {
            OnLoad();
            string EmpCode = "";
            try
            {
                var resignationRecord = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().FirstOrDefault(x => x.DocumentNo == Inc);

                if (resignationRecord == null)
                {
                    return "INVALID_DOC";
                }
                string openStatus = SYDocumentStatus.OPEN.ToString();
                var approvalList = unitOfWork.Set<ExDocApproval>().AsQueryable()
                                     .Where(w => w.DocumentType == resignationRecord.DocType && w.DocumentNo == Inc
                                     && w.Status == openStatus).OrderBy(w => w.ApproveLevel).ToList();

                var approverList = unitOfWork.Set<ExCfWFApprover>().AsQueryable().Where(w => w.Employee == User.UserName).ToList();

                bool hasApproved = false;

                foreach (var approval in approvalList)
                {
                    var matchingApprover = approverList.FirstOrDefault(w => w.Employee == approval.Approver);

                    if (matchingApprover != null)
                    {
                        if (approval.Status == SYDocumentStatus.APPROVED.ToString())
                        {
                            return "USER_ALREADY_APP";
                        }

                        var approverProfile = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == approval.Approver);
                        if (approverProfile != null)
                        {
                            approval.ApprovedBy = approverProfile.EmpCode;
                            approval.ApprovedName = approverProfile.AllName;
                            approval.LastChangedDate = DateTime.Now;
                            approval.ApprovedDate = DateTime.Now;
                            approval.Status = SYDocumentStatus.APPROVED.ToString();

                            unitOfWork.Update(approval);
                            unitOfWork.Save();

                            hasApproved = true;
                            break;
                        }
                    }
                }
                if (!hasApproved)
                {
                    return "USER_CANNOT_APPROVE";
                }
                var StaffView = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == resignationRecord.EmpCode);
                //send to next level
                foreach (var sendNext in approvalList)
                {
                    if (sendNext.Status == openStatus)
                    {
                        var minLevel = approvalList
                                       .Where(a => a.Status == openStatus)
                                       .Min(a => a.ApproveLevel);
                        var nextApp = unitOfWork.Set<ExDocApproval>().AsQueryable().FirstOrDefault(s => s.Approver == sendNext.Approver && s.ApproveLevel == minLevel);
                        var StaffApp = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(x => x.EmpCode == nextApp.Approver);
                        if (nextApp != null && StaffView != null)
                        {
                            if (StaffApp.Email != "" && StaffApp.Email != null)
                            {
                                #region Send Email

                                try
                                {
                                    // Retrieve email configuration
                                    var EmailConf = unitOfWork.Set<CFEmailAccount>().AsQueryable().FirstOrDefault();
                                    if (EmailConf != null)
                                    {
                                        CFEmailAccount emailAccount = EmailConf;
                                        string str = $"Dear <b>{StaffApp.Title} {StaffApp.AllName}</b>";
                                        str += $" <br /> I would like to Request Warning staff  <br /> <b> {resignationRecord.AllName} </b>";
                                        str += $"<br />  Reason: <b> {resignationRecord.Remark} <b> <br /> Please review and approve with favor.";
                                        str += $"<br /> <br /> Yours sincerely,<br /> <br /> <b>{resignationRecord.CreatedBy}</b>";

                                        // Prepare email details
                                        string subject = "Request Warning";
                                        string body = str; // Use the constructed message as the body
                                        string filePath = fileName; // Assuming fileName is defined elsewhere
                                        string fileName_ = Path.GetFileName(fileName);

                                        // Send the email
                                        EmailObject = new ClsEmail();
                                        List<string> filePathsList = new List<string> { fileName };
                                        if (!string.IsNullOrEmpty(resignationRecord.AttachPath))
                                            filePathsList.Add(resignationRecord.AttachPath);
                                        string[] filePaths = filePathsList.ToArray();
                                        int rs = EmailObject.SendMail(emailAccount, "", StaffApp.Email, subject, body, filePath, fileName_);

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
                                #endregion
                            }
                        }
                        break;
                    }

                }
                resignationRecord.Status = approvalList.Any(w => w.Status == openStatus) ?
                    SYDocumentStatus.PENDING.ToString() : SYDocumentStatus.APPROVED.ToString();
                unitOfWork.Update(resignationRecord);
                unitOfWork.Save();
                HRStaffProfile Staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == resignationRecord.EmpCode);
                if (resignationRecord.Status == SYDocumentStatus.APPROVED.ToString())
                {
                    //if all approved  send email to requestor
                    var EmpCreate = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(s => s.EmpCode == resignationRecord.CreatedBy);
                    var Applevel = unitOfWork.Set<ExDocApproval>().AsQueryable().FirstOrDefault(w => w.DocumentNo == resignationRecord.DocumentNo && w.Status == SYDocumentStatus.APPROVED.ToString() && w.DocumentType == resignationRecord.DocType);
                    var Approve = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == Applevel.Approver);
                    if (EmpCreate != null && !string.IsNullOrEmpty(EmpCreate.Email))
                    {
                        var EmailConf = unitOfWork.Set<CFEmailAccount>().AsQueryable().FirstOrDefault();
                        if (EmailConf != null)
                        {
                            CFEmailAccount emailAccount = EmailConf;
                            string subject = "Request Warning";
                            string body = $"Dear <b>{StaffView.Title}{EmpCreate.AllName}</b>";
                            body += $"<br /> I’m approved to the Warning Request for  <b>{StaffView.Title} {StaffView.AllName}</b>";
                            body += $"<br /> <br /> Yours sincerely,<br /> <br /> <b>{Approve.AllName}</b>";
                            //string filePath = fileName; // Ensure fileName is defined
                            //string fileName_ = Path.GetFileName(filePath);

                            EmailObject = new ClsEmail();
                            int rs = EmailObject.SendMail(emailAccount, "", EmpCreate.Email, subject, body, "", "");
                        }
                    }
                    var excfObject = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                    if (excfObject != null && !string.IsNullOrEmpty(excfObject.Telegram))
                    {
                        string str = $" Dear <b>{StaffView.Title} {EmpCreate.AllName}</b>";
                        str += $"%0A I’m approved to the Warning Request for <b> {StaffView.AllName} </b> ";
                        str += $"%0A%0AYours sincerely,%0A%0A<b>{Approve.AllName}</b>";

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
        public List<HREmpDisciplinary> LoadData()
        {
            OnLoad();
            var AccLevel = unitOfWork.Set<SYUserLevel>().AsQueryable().ToList();
            AccLevel = AccLevel.Where(w => w.UserName == User.UserName).ToList();
            var _list = new List<HREmpDisciplinary>();
            var empCon = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().ToList();
            var DisciplinType = unitOfWork.Set<HRDisciplinType>().AsQueryable().ToList();
            var Staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().ToList();
            Staff = Staff.Where(w => AccLevel.Where(x => x.LevelCode == w.LevelCode).Any()).ToList();
            var DiscpAction = unitOfWork.Set<HRDisciplinAction>().AsQueryable().ToList();
            var resu = (from edu in empCon
                        join emp in Staff on edu.EmpCode equals emp.EmpCode
                        join ET in DiscpAction on edu.DiscAction equals ET.Code
                        join disciplinDesc in DisciplinType on edu.DiscType equals disciplinDesc.Code
                        select new
                        {
                            edu.TranNo,
                            edu.EmpCode,
                            emp.AllName,
                            DisciplinayType = ET.Description,
                            edu.TranDate,
                            DisciplinaryAction = disciplinDesc.Description,
                            edu.Remark,
                            edu.Reference,
                            edu.Consequence
                        }).ToList();
            foreach (var item in resu)
            {
                var _empCon = new HREmpDisciplinary();
                _empCon.TranNo = item.TranNo;
                _empCon.EmpCode = item.EmpCode;
                _empCon.AllName = item.AllName;
                _empCon.DiscType = item.DisciplinayType;
                _empCon.Reference = item.Reference;
                _empCon.DiscAction = item.DisciplinaryAction;
                _empCon.TranDate = item.TranDate;
                _empCon.Remark = item.Remark;
                _empCon.Consequence = item.Consequence;
                _list.Add(_empCon);
            }

            return _list;
        }
    }
    public class ClsEmpDisciplinary
    {
        public long TranNo { get; set; }
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public string DisciplinayType { get; set; }
        public string DisciplinaryAction { get; set; }
        public DateTime TranDate { get; set; }
        public string Remark { get; set; }
        public string DescriptionofInfraction { get; set; }
        public string PlanForImprovement { get; set; }
        public string ConsequencesofFurtherInfractions { get; set; }
        public string DocumentNo { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string RequestBy { get; set; }
    }
}