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
    public class ClsESSRequestProbation: IClsESSRequestProbation
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public FTINYear FInYear { get; set; }
        public string DocType { get; set; }
        public string MessageError { get; set; }
        public HREFReqProbation Header { get; set; }
        public HRStaffProfile Staff { get; set; }
        public string CompanyCode { get; set; }
        public string Plant { get; set; }
        public string MessageCode { get; set; }
        public List<HREFReqProbation> ListHeader { get; set; }
        public List<ClsEmpProbation> ListPending { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public ClsEmail EmailObject { get; set; }
        public long TranNoCareer;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsESSRequestProbation()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void ProcessProbation(string userName)
        {
            OnLoad();
            string pending = SYDocumentStatus.PENDING.ToString();
            string open = SYDocumentStatus.OPEN.ToString();
            string approved = SYDocumentStatus.APPROVED.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();

            var listReqProb = unitOfWork.Set<HREFReqProbation>().AsQueryable()
                .Where(w => w.Status == pending)
                .ToList();
            var listApp = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                .Where(w => listReqProb.AsEnumerable().Where(x => w.DocumentNo == x.DocumentNo && w.DocumentType == x.DocType && w.Approver == userName).Any())
                .ToList();

            foreach (var item in listApp)
            {
                if (item.ApproveLevel == 1 && item.Status == open)
                {
                    var EmpStaff = listReqProb.Where(w => w.DocumentNo == item.DocumentNo).ToList();
                    if (!EmpStaff.Any()) continue;
                    AddPendingToList(item, EmpStaff);
                }
                else if (item.ApproveLevel > 1 && item.Status == open)
                {
                    var level = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == item.DocumentNo && w.DocumentType == item.DocumentType && w.ApproveLevel < item.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                    if (level != null) continue;
                    var EmpStaff = listReqProb.Where(w => w.DocumentNo == item.DocumentNo).ToList();
                    if (!EmpStaff.Any()) continue;
                    AddPendingToList(item, EmpStaff);
                }
            }

            var ListLeaveCreater = unitOfWork.Set<HREFReqProbation>().AsQueryable().AsEnumerable()
                                .Where(x => unitOfWork.Set<ExDocApproval>().AsQueryable()
                                    .Any(w => x.DocumentNo == w.DocumentNo
                                               && x.DocType == w.DocumentType && w.Approver == userName)
                                    || x.CreatedBy == userName)
                                .ToList();
            ListHeader = ListLeaveCreater.ToList();
        }
        private void AddPendingToList(ExDocApproval item, List<HREFReqProbation> listReqTransfer)
        {
            foreach (var read in listReqTransfer)
            {
                var Staff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == read.EmpCode);
                if (Staff == null) continue;
                var ListPen = new ClsEmpProbation();
                var empTransfer = listReqTransfer.FirstOrDefault(w => w.DocumentNo == item.DocumentNo);
                if (empTransfer != null)
                {
                    ListPen.DocumentNo = read.DocumentNo;
                    ListPen.EmpCode = read.EmpCode;
                    ListPen.AllName = Staff.AllName;
                    ListPen.EffectDate = read.Probation;
                    ListPen.Status = read.Status;
                    ListPending.Add(ListPen);
                }
            }

        }
        public string CreateReqPro(string URL)
        {
            OnLoad();
            try
            {
                if (Header.Probation == DateTime.MinValue || Header.Probation == null)
                {
                    return "INVALID_PROBATION_DATE";
                }
                if (string.IsNullOrEmpty(Header.Reason))
                {
                    return "REASON";
                }
                if (string.IsNullOrEmpty(Header.ProbationType))
                {
                    return "INVALID_PROBATION_TYPE";
                }
                if (!ListApproval.Any()) return "INVALID_APPROVER";
                var StaffReq = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                if (StaffReq == null) return "INVALID_EMP";
                string Status = SYDocumentStatus.OPEN.ToString();
                var objNumber = new CFNumberRank(DocType, StaffReq.CompanyCode, Header.Probation.Year, true);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.DocumentNo = objNumber.NextNumberRank;
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.EmpName = HeaderStaff.AllName;
                Header.Position = StaffReq.JobCode;
                Header.Department = StaffReq.DEPT;
                Header.OldSalary = HeaderStaff.Salary;
                Header.Section = StaffReq.SECT;
                Header.StartDate = StaffReq.StartDate;
                Header.DocType = DocType;
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
                    objApp.WFObject = "PRO";
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
        public string EditReqPro(string DocNo)
        {
            OnLoad();
            try
            {
                Header.EmpCode = HeaderStaff.EmpCode;
                var objMatch = unitOfWork.Set<HREFReqProbation>().AsQueryable().FirstOrDefault(w => w.DocumentNo == DocNo);
                if (objMatch == null)
                {
                    return "DISCIPLINAY_NE";
                }
                objMatch.Increase = Header.Increase;
                objMatch.NewSalary = Header.NewSalary;
                objMatch.ProbationType = Header.ProbationType;
                objMatch.Probation = Header.Probation;
                objMatch.Reason = Header.Reason;
                objMatch.ChangedBy = Header.ChangedBy;
                objMatch.ChangedOn = Header.ChangedOn;

                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderStaff.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderStaff.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderStaff.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string DeleteReqPro(string DocNo)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                Header = new HREFReqProbation();
                var objMatch = unitOfWork.Set<HREFReqProbation>().AsQueryable().FirstOrDefault(w => w.DocumentNo == DocNo);
                if (objMatch == null)
                {
                    return "DISCIPLINAY_NE";
                }
                Header.DocumentNo = DocNo;
                var Approver = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == objMatch.DocumentNo && w.DocumentType == objMatch.DocType).ToList();
                if (Approver.Any())
                {
                    unitOfWork.BulkDelete(Approver);
                }

                unitOfWork.Delete(objMatch);

                unitOfWork.Commit();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.DocumentNo, SYActionBehavior.ADD.ToString(), e, true);
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
            var Doc = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().FirstOrDefault(w => w.ScreenID == "ESSEF00002");
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
		public string RequestForApprove(string DocNo, string fileName, string URL)
		{
            OnLoad();
            try
			{
				var objMatch = unitOfWork.Set<HREFReqProbation>().AsQueryable().FirstOrDefault(x => x.DocumentNo == DocNo);
				HRStaffProfile StaffApp = new HRStaffProfile();
				if (objMatch == null) return "DOC_INV";
				string open = SYDocumentStatus.OPEN.ToString();
                var ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentType == objMatch.DocType
                                    && w.DocumentNo == objMatch.DocumentNo && w.Status == open)
                                    .OrderBy(w => w.ApproveLevel).ToList().FirstOrDefault();
                if (ListApproval == null) return "INVALID_APPROVER";

				objMatch.Status = SYDocumentStatus.PENDING.ToString();
				unitOfWork.Update(objMatch);
				unitOfWork.Save();
                StaffApp = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == ListApproval.Approver);
                if (StaffApp != null)
                {
                    #region Email
                    var StaffView = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                    var Staff_ = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == objMatch.CreatedBy);
                    if (!string.IsNullOrEmpty(StaffApp.Email) && Staff_ != null && StaffView != null)
                    {
                        try
                        {
                            // Retrieve email configuration
                            var EmailConf = unitOfWork.Set<CFEmailAccount>().AsQueryable().FirstOrDefault();

                            if (EmailConf != null)
                            {
                                CFEmailAccount emailAccount = EmailConf;
                                string str = $"Dear <b>{StaffApp.Title} {StaffApp.AllName}</b>";
                                str += $" <br /> I would like to request staff for Probationary Evaluation <br /> <b>{StaffView.AllName}</b>";

                                str += $"<br /> Position: <b>{StaffView.Position}</b>";
                                str += $"<br /> Start Date: <b>{StaffView.StartDate.Value.ToString("dd/MMMM/yyyy")}</b>";
                                str += "<br /> Please review and approve with favor.";
                                str += $"<br /> <br /> Yours sincerely,<br /> <br /> <b>{Staff_.AllName}</b>";

                                // Prepare email details
                                string subject = "Probationary Evaluation";
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
                        if (StaffView != null)
                        {
                            string str = $"I would like to request staff for Probationary Evaluation %0A <b>{StaffView.AllName}</b>";

                            str += $"%0A Position: <b>{StaffView.Position}</b>";
                            str += $"%0A Start Date: <b>{StaffView.StartDate.Value.ToString("dd/MMMM/yyyy")}</b>";
                            str += "%0A Please review and approve with favor.";
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
                    }
                    #endregion
                }
                return SYConstant.OK;
			}
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocNo, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Cancel(string DocNo)
        {
            OnLoad();
            try
            {
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                string Pending = SYDocumentStatus.PENDING.ToString();
                var objmatch = unitOfWork.Set<HREFReqProbation>().AsQueryable().First(w => w.DocumentNo == DocNo);
                if (objmatch == null) return "INVALID";
                var chkstatusApp = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.Status == Open && w.DocumentNo == DocNo && w.DocumentType == objmatch.DocType).ToList();

                if (chkstatusApp != null)
                {
                    foreach (var read in chkstatusApp)
                    {
                        read.Status = Cancel;
                        unitOfWork.Update(read);
                    }
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
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocNo, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Reject(string DocNo)
        {
            OnLoad();
            try
            {
                string Reject = SYDocumentStatus.REJECTED.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                string[] c = DocNo.Split(';');
                foreach (var Doc in c)
                {
                    if (Doc == "") continue;
                    var objmatch = unitOfWork.Set<HREFReqProbation>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Doc);
                    if (objmatch == null)
                    {
                        return "INV_EN";
                    }
                    var _obj = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentType == objmatch.DocType && w.DocumentNo == objmatch.DocumentNo && w.Approver == User.UserName && w.Status == Open);
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
                    // var Applevel = unitOfWork.Set<ExDocApproval>().AsQueryable().FirstOrDefault(w => w.DocumentNo == objmatch.DocumentNo && w.DocumentType == objmatch.DocType && w.Status == Reject);
                    var Approve = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == User.UserName);
                    if (EmpCreate != null && !string.IsNullOrEmpty(EmpCreate.Email) && StaffView != null)
                    {
                        var EmailConf = unitOfWork.Set<CFEmailAccount>().AsQueryable().FirstOrDefault();
                        if (EmailConf != null)
                        {
                            CFEmailAccount emailAccount = EmailConf;
                            string subject = "Probationary Evaluation";
                            string body = $"Dear <b> {StaffView.Title} {objmatch.CreatedBy} </b> <br />";
                            body += $"<br /> I’m reject to the  Probationary request for  <b> {StaffView.Title} {objmatch.EmpName}</b> ";
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
                        string str = $" Dear <b> {StaffView.Title} {objmatch.CreatedBy} </b> %0A";
                        str += $" %0A I’m reject to the  Probationary request for %0A <b>{StaffView.Title} {objmatch.EmpName}</b>";
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
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, DocNo, SYActionBehavior.ADD.ToString(), e, true);
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
            var obj = unitOfWork.Set<HREFReqProbation>()
                .FirstOrDefault(x => x.DocumentNo == Inc);

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
            UpdateProbationStatus(obj, approvalList, openStatus);

            if (obj.Status == SYDocumentStatus.APPROVED.ToString())
            {
                ProcessProbation(obj, fileName, objapprover);
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
        private void NotifyNextApprover(HREFReqProbation record, List<ExDocApproval> approvals, string fileName, string openStatus)
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
        private void SendApprovalEmail(HREFReqProbation record, HRStaffProfile approver, HR_STAFF_VIEW staffView, string fileName)
        {
            try
            {
                var emailConfig = unitOfWork.Set<CFEmailAccount>().FirstOrDefault();
                if (emailConfig == null) return;

                var emailBody = $@"Dear {approver.Title} {approver.AllName}
            <br/><br/>I would like to request staff for Probationary Evaluation for :
            <br/><b>{staffView.AllName}</b>
            <br/>on date <b>{record.Probation:dd/MMM/yyyy}</b>
            <br/><b>Reason:</b> {record.Reason}
            <br/><br/>Please review and approve with favor.
            <br/><br/>Yours sincerely,
            <br/><br/><b>{record.CreatedBy}</b>";
                var Subject = $"Probationary Evaluation";
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
        private void UpdateProbationStatus(HREFReqProbation record, List<ExDocApproval> approvals, string openStatus)
        {
            record.Status = approvals.Any(w => w.Status == openStatus)
                ? SYDocumentStatus.PENDING.ToString()
                : SYDocumentStatus.APPROVED.ToString();

            unitOfWork.Update(record);
        }
        private void ProcessProbation(HREFReqProbation record, string fileName, List<ExDocApproval> approvals)
        {
            var staff = unitOfWork.Set<HRStaffProfile>()
                .FirstOrDefault(w => w.EmpCode == record.EmpCode);
            if (staff == null) throw new Exception("STAFF_NOT_FOUND");

            UpdateCareerHistory(record, staff);
            NotifyRequestor(record, fileName, approvals);
        }
        private void UpdateCareerHistory(HREFReqProbation record, HRStaffProfile staff)
        {
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

            string type = record.ProbationType;
            string ResignType = "";
            if (type == "REJECT")
            {
                ResignType = "REJECT";
                type = "TERMINAT";
            }
            var _CarCode = LstCareerCode.FirstOrDefault(x => x.Code == type);
            if (_CarCode == null)
            {
                throw new Exception("CAREERCODE_EN");
            }
            // 4. Validate career dates
            if (ListCareer.Any(w => w.FromDate.Value.Date >= record.Probation.Date))
                throw new Exception("INV_DATE");

            // 5. Update existing career records with concurrency check
            var latestCareer = ListCareer.OrderByDescending(x => x.FromDate).FirstOrDefault();
            if (latestCareer != null)
            {
                latestCareer.LCKEDIT = 1;
                latestCareer.ToDate = record.Probation.AddDays(-1);
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
                CareerCode = type,
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
                FromDate = record.Probation.Date,
                ToDate = new DateTime(5000, 1, 1),
                EffectDate = record.Probation.Date,
                ProDate = objMatchHeader.StartDate,
                Reason = record.Reason,
                Remark = "",
                Appby = "",
                AppDate = objMatchHeader.StartDate.Value.ToString("dd-MM-yyyy"),
                VeriFyBy = "",
                VeriFYDate = objMatchHeader.StartDate.Value.ToString("dd-MM-yyyy"),
                LCK = 0,
                OldSalary = record.OldSalary??0,
                Increase = record.Increase?? 0,
                Functions = latestCareer.Functions,
                NewSalary = record.NewSalary??0,
                PersGrade = latestCareer.PersGrade,
                HomeFunction = latestCareer.HomeFunction,
                SubFunction = latestCareer.SubFunction,
                StaffType = latestCareer.StaffType,
                CreateBy = User.UserName,
                CreateOn = DateTime.Now,
            };
            string Status = SYDocumentStatus.A.ToString();
            if (!string.IsNullOrEmpty(ResignType))
            {
                HeaderCareer.resigntype = ResignType;
                Status = SYDocumentStatus.I.ToString();
                objMatchHeader.CareerDesc = _CarCode.Code;
                objMatchHeader.DateTerminate = record.Probation.Date;
                objMatchHeader.EffectDate = record.Probation.Date;
            }
            unitOfWork.Add(HeaderCareer);

            staff.Status = Status;
            objMatchHeader.Salary = HeaderCareer.NewSalary;
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
        private void NotifyRequestor(HREFReqProbation record, string fileName, List<ExDocApproval> approvals)
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
                    string subject = $"Probationary Evaluation";
                    string body = $@"Dear <b>{EmpCreate.Title} {EmpCreate.AllName}</b>
                <br/><br/>Your probationary evaluation request for:
                <br/><b>{staffView.Title} {staffView.AllName}</b>
                <br/>has been approved by: <b>{Approve?.AllName}</b>
                <br/><br/>Yours sincerely,
                <br/><br/><b>{Approve?.AllName}</b>";

                    new ClsEmail().SendMail(EmailConf, "", EmpCreate.Email, subject, body, "", "");
                }
            }

            // 2. Send Telegram notification
            var excfObject = unitOfWork.Set<ExCfWorkFlowItem>()
                .FirstOrDefault(w => w.ScreenID == ScreenId);

            if (excfObject != null && !string.IsNullOrEmpty(excfObject.Telegram))
            {
                string message = $@"Dear {EmpCreate.Title} {EmpCreate.AllName}
            %0AI’m approved to the Probationary Request for: {staffView.AllName}
            %0A%0AYours sincerely,
            %0A{Approve.AllName}";

                SYSendTelegramObject Tel = new SYSendTelegramObject { User = User, BS = BS };
                WorkFlowResult result = Tel.Send_SMS_Telegram(message, excfObject.Telegram, false);
            }
        }
    }
    public class ClsEmpProbation
    {
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public DateTime EffectDate { get; set; }
        public string Remark { get; set; }
        public string DocumentNo { get; set; }
        public string Status { get; set; }
    }
}