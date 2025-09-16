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
using System.Text;

namespace Humica.EForm
{
    public class ClsRequestTransferStaff: IClsRequestTransferStaff
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public FTINYear FInYear { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public string DocType { get; set; }
        public List<HR_STAFF_VIEW> ListStaffView { get; set; }
        public HREFRequestTransferStaff Header { get; set; }
        public List<HREFRequestTransferStaff> ListHeader { get; set; }
        public List<ClsEmpTransfer> ListPending { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<RCMAEdu> ListRCMAEdu { get; set; }
        public List<RCMAWorkHistory> ListRCMAWorkHistory { get; set; }
        public List<HR_HisCareer> ListHisCareer { get; set; }
        public ClsEmail EmailObject { get; set; }
        public string NewSalary { get; set; }
        public string OldSalary { get; set; }
        public string Increase { get; set; }
        public long TranNoCareer;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsRequestTransferStaff()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void ProcessTransfers(string userName)
        {
            OnLoad();
            string pending = SYDocumentStatus.PENDING.ToString();
            string open = SYDocumentStatus.OPEN.ToString();
            string approved = SYDocumentStatus.APPROVED.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();

            var listReqTransfer = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable()
                .Where(w => w.Status == pending)
                .ToList();

            var listCreate = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable()
                .Where(w => w.CreatedBy == userName)
                .ToList();

            var listApp = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                .Where(w => listReqTransfer.AsEnumerable().Where(x => w.DocumentNo == x.DocNo && w.DocumentType == x.DocType && w.Approver == userName).Any())
                .ToList();

            foreach (var item in listApp)
            {

                if (item.ApproveLevel == 1 && item.Status == open)
                {
                    var EmpStaff = listReqTransfer.Where(w => w.DocNo == item.DocumentNo).ToList();
                    if (!EmpStaff.Any()) continue;
                    AddTransferToList(item, EmpStaff);
                }
                else if (item.ApproveLevel > 1 && item.Status == open)
                {
                    var level = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == item.DocumentNo && w.DocumentType == item.DocumentType && w.ApproveLevel < item.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                    if (level != null) continue;
                    var EmpStaff = listReqTransfer.Where(w => w.DocNo == item.DocumentNo).ToList();
                    if (!EmpStaff.Any()) continue;
                    AddTransferToList(item, EmpStaff);
                }
            }

            var ListLeaveCreater = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable().AsEnumerable()
                                .Where(x => unitOfWork.Set<ExDocApproval>().AsQueryable()
                                    .Any(w => x.DocNo == w.DocumentNo
                                               && x.DocType == w.DocumentType && w.Approver == userName)
                                    || x.CreatedBy == userName)
                                .ToList();
            //ListLeaveCreater = ListLeaveCreater.Where(x => x.Status != open && x.Status != Cancel).ToList();
            ListHeader = ListLeaveCreater.ToList();
        }
        private void AddTransferToList(ExDocApproval item, List<HREFRequestTransferStaff> listReqTransfer)
        {
            foreach(var read in listReqTransfer)
            {
                var Staff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == read.EmpCode);
                if (Staff == null) continue;
                var ListPen = new ClsEmpTransfer();
                var empTransfer = listReqTransfer.FirstOrDefault(w => w.DocNo == item.DocumentNo);
                if (empTransfer != null)
                {
                    ListPen.DocNo =  read.DocNo;
                    ListPen.EmpCode =  read.EmpCode;
                    ListPen.AllName =  Staff.AllName;
                    ListPen.EffectDate = read.EffectDate.Value;
                    ListPen.Status = read.Status;
                    ListPending.Add(ListPen);
                }
            }
           
        }
        public string CreateTransferStaff()
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                string Status = SYDocumentStatus.OPEN.ToString();
                var StaffReq = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                if (StaffReq == null) return "INVALID_EMP";
                if (string.IsNullOrEmpty(Header.Reason))
                    return "REASON";
                if (!ListApproval.Any()) return "INVATID_APPROVER";
                var objNumber = new CFNumberRank(DocType, StaffReq.CompanyCode, Header.EffectDate.Value.Year, true);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                int lineitem = 1;
                Header.DocNo = objNumber.NextNumberRank;
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.Salary = HeaderStaff.Salary;
                Header.DocType = DocType;
                Header.RequestDate = DateTime.Now;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;

                unitOfWork.Add(Header);
                foreach (var item in ListRCMAEdu)
                {
                    var read = new RCMAEdu();
                    read.ApplicantID = Header.DocNo;
                    read.LineItem = lineitem;
                    read.EduType = item.EduType;
                    read.EduCenter = item.EduCenter;
                    read.Major = item.Major;
                    lineitem++;
                    unitOfWork.Add(read);

                }
                foreach (var item in ListRCMAWorkHistory)
                {
                    var read = new RCMAWorkHistory();
                    read.ApplicantID = Header.DocNo;
                    read.LineItem = lineitem;
                    read.Company = item.Company;
                    read.Position = item.Position;
                    read.FromDate = item.FromDate;
                    read.ToDate = item.ToDate;
                    lineitem++;
                    unitOfWork.Add(read);

                }
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
                    objApp.WFObject = "TRAN";
                    objApp.ApprovedBy = "";
                    objApp.ApprovedName = "";
                    objApp.LastChangedDate = DateTime.Now;
                    unitOfWork.Add(objApp);
                }

                unitOfWork.Commit();
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
        public string DeleteEmp(string id)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var objMatch = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable().FirstOrDefault(w => w.DocNo == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                var edu = unitOfWork.Set<RCMAEdu>().AsQueryable().Where(w => w.ApplicantID == id).ToList();
                if (edu.Any())
                    unitOfWork.BulkDelete(edu);
                var WorkH = unitOfWork.Set<RCMAWorkHistory>().AsQueryable().Where(w => w.ApplicantID == id).ToList();
                if (WorkH.Any())
                    unitOfWork.BulkDelete(WorkH);
                var Approver = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == id && w.DocumentType == objMatch.DocType).ToList();
                if (Approver.Any())
                    unitOfWork.BulkDelete(Approver);
                unitOfWork.Delete(objMatch);

                unitOfWork.Commit();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string EditEmp(string id)
        {
            OnLoad();
            try
            {
                Header.EmpCode = HeaderStaff.EmpCode;
                HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable().FirstOrDefault(w => w.DocNo == id);
                if (objMatch == null)
                {
                    return "DISCIPLINAY_NE";
                }
                objMatch.DocNo = Header.DocNo;
                objMatch.EmpCode = Header.EmpCode;
                objMatch.Department = Header.Department;
                objMatch.RequestDate = Header.RequestDate;
                objMatch.Increase = Header.Increase;
                objMatch.NewSalary = Header.NewSalary;
                objMatch.Level = Header.Level;
                objMatch.CompanyCode = Header.CompanyCode;
                objMatch.TransferCode = Header.TransferCode;
                objMatch.Section = Header.Section;
                objMatch.Status = Header.Status;
                objMatch.ChangedBy = Header.ChangedBy;
                objMatch.Reason = Header.Reason;
                objMatch.ChangedOn = Header.ChangedOn;
                objMatch.Branch = Header.Branch;
                objMatch.Division = Header.Division;
                objMatch.GroupDEPT = Header.GroupDEPT;
                objMatch.AttachFile = Header.AttachFile;
                objMatch.EffectDate = Header.EffectDate;

                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public void SetAutoApproval(string screenId, string docType, string branch, string deptCode)
        {
            ListApproval = new List<ExDocApproval>();
            // Get workflow configuration
            var workflowConfig = unitOfWork.Set<ExCfWorkFlowItem>()
                .FirstOrDefault(w => w.ScreenID == screenId && w.DocType == docType);
            if (workflowConfig?.IsRequiredApproval != true) return;
            // Get base list of approvers
            var approvers = unitOfWork.Set<ExCfWFApprover>()
                .Where(w => w.WFObject == workflowConfig.ApprovalFlow && w.IsSelected)
                .ToList();
            if (!approvers.Any()) return;
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
        public string Approve(string Inc, string fileName, string URL)
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
            var obj = unitOfWork.Set<HREFRequestTransferStaff>()
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
            UpdateResignationStatus(obj, approvalList, openStatus);

            if (obj.Status == SYDocumentStatus.APPROVED.ToString())
            {
                ProcessTransfer(obj, fileName, objapprover);
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
        private void NotifyNextApprover(HREFRequestTransferStaff record, List<ExDocApproval> approvals, string fileName, string openStatus)
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
        private void SendApprovalEmail(HREFRequestTransferStaff record, HRStaffProfile approver, HR_STAFF_VIEW staffView, string fileName)
        {
            try
            {
                // Retrieve email configuration
                var emailConfig = unitOfWork.Set<CFEmailAccount>().FirstOrDefault();
                if (emailConfig == null) return;

                // Build email body using StringBuilder for efficiency
                var emailBody = new StringBuilder();
                emailBody.AppendLine($"Dear {approver.Title} {approver.AllName},");
                emailBody.AppendLine("<br />");
                emailBody.AppendLine($"I would like to request a Transfer/Promotion for <b>{staffView.AllName}</b>:");
                emailBody.AppendLine("<br /><br />");

                // Check for position change
                if (staffView.JobCode != record.Position && !string.IsNullOrEmpty(record.Position))
                {
                    var newPosition = unitOfWork.Set<HRPosition>()
                        .FirstOrDefault(w => w.Code == record.Position);

                    emailBody.AppendLine("<b>Position Change:</b>");
                    emailBody.AppendLine("<ul>");
                    emailBody.AppendLine($"<li>Current: {staffView.Position}</li>");
                    emailBody.AppendLine($"<li>Proposed: {newPosition?.Description ?? record.Position}</li>");
                    emailBody.AppendLine("</ul>");
                }

                // Check for company change
                if (staffView.CompanyCode != record.CompanyCode && !string.IsNullOrEmpty(record.CompanyCode))
                {
                    emailBody.AppendLine("<b>Company Change:</b>");
                    emailBody.AppendLine("<ul>");
                    emailBody.AppendLine($"<li>Current: {staffView.CompanyCode}</li>");
                    emailBody.AppendLine($"<li>Proposed: {record.CompanyCode}</li>");
                    emailBody.AppendLine("</ul>");
                }

                // Add call to action
                emailBody.AppendLine("<br />");
                emailBody.AppendLine("Please review and approve this request at your earliest convenience.");
                emailBody.AppendLine("<br /><br />");
                emailBody.AppendLine("Yours sincerely,");
                emailBody.AppendLine($"<br /><b>{record.ChangedBy}</b>");

                // Create basic HTML structure
                var fullBody = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; font-size: 14px; color: #333; }}
                        ul {{ padding-left: 20px; margin: 5px 0; }}
                        li {{ margin-bottom: 3px; }}
                    </style>
                </head>
                <body>
                    {emailBody}
                </body>
                </html>";
                string subject = "Transfer/Promotion Request";
                // Send email
                List<string> filePathsList = new List<string> { fileName };
                if (!string.IsNullOrEmpty(record.AttachFile))
                    filePathsList.Add(record.AttachFile);
                string[] filePaths = filePathsList.ToArray();
                new ClsEmail().SendMails(emailConfig, "", approver.Email, subject, fullBody, filePaths);
            }
            catch (Exception ex)
            {
                // Add logging here in real implementation
                // Logger.Error($"Failed to send approval email: {ex.Message}", ex);
                throw new Exception("FAIL_TO_SEND_MAIL: " + ex.Message);
            }
        }
        private void UpdateResignationStatus(HREFRequestTransferStaff record, List<ExDocApproval> approvals, string openStatus)
        {
            record.Status = approvals.Any(w => w.Status == openStatus)
                ? SYDocumentStatus.PENDING.ToString()
                : SYDocumentStatus.APPROVED.ToString();

            unitOfWork.Update(record);
        }
        private void ProcessTransfer(HREFRequestTransferStaff record, string fileName, List<ExDocApproval> approvals)
        {
            var staff = unitOfWork.Set<HRStaffProfile>()
                .FirstOrDefault(w => w.EmpCode == record.EmpCode);
            if (staff == null) throw new Exception("STAFF_NOT_FOUND");

            UpdateCareerHistory(record, staff);
            NotifyRequestor(record, fileName, approvals);
        }
        private void UpdateCareerHistory(HREFRequestTransferStaff record, HRStaffProfile staff)
        {
            // Retrieve career history data
            var careerCodes = unitOfWork.Set<HRCareerHistory>().ToList();
            var careerHistory = unitOfWork.Set<HREmpCareer>()
                .Where(w => w.EmpCode == staff.EmpCode)
                .ToList();

            // Validate career code
            var careerCode = careerCodes.FirstOrDefault(x => x.Code == record.TransferCode)
                ?? throw new Exception("CAREER_CODE_NOT_FOUND");

            // Validate career dates
            if (careerHistory.Any(w => w.FromDate?.Date >= record.EffectDate.Value.Date))
                throw new Exception("INVALID_CAREER_DATE");

            // Get latest career record
            var latestCareer = careerHistory.OrderByDescending(x => x.FromDate).FirstOrDefault();

            // Update latest career record if exists
            if (latestCareer != null)
            {
                latestCareer.LCKEDIT = 1;
                latestCareer.ToDate = record.EffectDate.Value.AddDays(-1);
                unitOfWork.Update(latestCareer);
            }

            // Create new career record
            var newCareer = CreateCareerRecord(record, staff, latestCareer);
            TranNoCareer = newCareer.TranNo;
            unitOfWork.Add(newCareer);

            // Update staff profile
            UpdateStaffProfile(staff, newCareer, careerCode);

            // Update bank accounts
            UpdateBankAccounts(newCareer, staff);
        }
        private HREmpCareer CreateCareerRecord(HREFRequestTransferStaff record,HRStaffProfile staff,HREmpCareer latestCareer)
        {
            // Determine values for new career record
            string company = !string.IsNullOrEmpty(record.CompanyCode) ? record.CompanyCode : latestCareer?.CompanyCode;
            string branch = !string.IsNullOrEmpty(record.Branch) ? record.Branch : latestCareer?.Branch;
            string groupDept = !string.IsNullOrEmpty(record.GroupDEPT) ? record.GroupDEPT : latestCareer?.GroupDept;
            string division = !string.IsNullOrEmpty(record.Division) ? record.Division : latestCareer?.Division;
            string department = !string.IsNullOrEmpty(record.Department) ? record.Department : latestCareer?.DEPT;
            string position = !string.IsNullOrEmpty(record.Position) ? record.Position : latestCareer?.JobCode;
            string section = !string.IsNullOrEmpty(record.Section) ? record.Section : latestCareer?.SECT;
            string location = latestCareer?.LOCT;
            string level = !string.IsNullOrEmpty(record.Level) ? record.Level : latestCareer?.LevelCode;

            // Get next career ID
            long tranNo = unitOfWork.Set<HREmpCareer>()
                .Select(w => (long?)w.TranNo)
                .DefaultIfEmpty(0)
                .Max()
                .GetValueOrDefault() + 1;

            // Create and return new career record
            return new HREmpCareer
            {
                TranNo = tranNo,
                EmpCode = staff.EmpCode,
                CareerCode = record.TransferCode,
                EmpType = latestCareer?.EmpType,
                CompanyCode = company,
                Branch = branch,
                DEPT = department,
                LOCT = location,
                Division = division,
                LINE = staff.Line,
                SECT = section,
                CATE = latestCareer?.CATE,
                LevelCode = level,
                JobCode = position,
                JobGrade = latestCareer?.JobGrade,
                JobDesc = staff.POSTDESC,
                JobSpec = staff.JOBSPEC,
                EstartSAL = record.Salary?.ToString("N2"),
                EIncrease = record.Salary?.ToString("N2"),
                ESalary = latestCareer?.ESalary,
                SupCode = latestCareer?.SubFunction,
                FromDate = record.EffectDate,
                ToDate = new DateTime(5000, 1, 1),
                EffectDate = record.EffectDate,
                ProDate = staff.StartDate,
                Reason = record.Reason,
                Remark = null,
                Appby = "",
                AppDate = staff.StartDate?.ToString("dd-MM-yyyy") ?? "",
                VeriFyBy = "",
                VeriFYDate = staff.StartDate?.ToString("dd-MM-yyyy") ?? "",
                LCK = 0,
                OldSalary = record.Salary ?? 0,
                Increase = record.Increase ?? 0,
                Functions = latestCareer?.Functions,
                NewSalary = record.NewSalary ?? 0,
                PersGrade = latestCareer?.PersGrade,
                HomeFunction = latestCareer?.HomeFunction,
                SubFunction = latestCareer?.SubFunction,
                StaffType = latestCareer?.StaffType,
                CreateBy = User.UserName,
                CreateOn = DateTime.Now,
                GroupDept = groupDept
            };
        }
        private void UpdateStaffProfile(HRStaffProfile staff, HREmpCareer newCareer,HRCareerHistory careerCode)
        {
            // Handle NEWJOIN special case
            if (newCareer.CareerCode == "NEWJOIN")
            {
                var probationType = unitOfWork.Set<HRProbationType>().FirstOrDefault(w => w.Code == staff.ProbationType);
                if (probationType != null)
                {
                    staff.StartDate = newCareer.EffectDate;
                    staff.Probation = staff.StartDate.Value.AddMonths(probationType.InMonth);
                    staff.LeaveConf = staff.StartDate;
                }
            }

            // Update termination status if needed
            if (careerCode.NotCalSalary == true)
            {
                staff.DateTerminate = newCareer.EffectDate.GetValueOrDefault();
                staff.TerminateStatus = newCareer.CareerCode;
                staff.TerminateRemark = newCareer.Remark;
                staff.Status = SYDocumentStatus.I.ToString();
            }
            else
            {
                staff.DateTerminate = new DateTime(1900, 1, 1);
                staff.TerminateStatus = "";
                staff.TerminateRemark = "";
                staff.Status = SYDocumentStatus.A.ToString();
            }

            // Update staff properties
            staff.Branch = newCareer.Branch;
            staff.LOCT = newCareer.LOCT;
            staff.Division = newCareer.Division;
            staff.GroupDept = newCareer.GroupDept;
            staff.DEPT = newCareer.DEPT;
            staff.Line = newCareer.LINE;
            staff.CATE = newCareer.CATE;
            staff.SECT = newCareer.SECT;
            staff.Functions = newCareer.Functions;
            staff.LevelCode = newCareer.LevelCode;
            staff.JobGrade = newCareer.JobGrade;
            staff.JobCode = newCareer.JobCode;
            staff.JOBSPEC = newCareer.JobSpec;
            staff.StaffType = newCareer.StaffType;
            staff.ESalary = newCareer.ESalary;
            staff.EffectDate = newCareer.EffectDate;
            staff.EmpType = newCareer.EmpType;
            staff.CareerDesc = newCareer.CareerCode;
            staff.Salary = newCareer.NewSalary;
            staff.ChangedBy = User.UserName;
            staff.ChangedOn = DateTime.Now;

            unitOfWork.Update(staff);
        }
        private void UpdateBankAccounts(HREmpCareer HeaderCareer, HRStaffProfile staff)
        {
            var objMatchHeader = staff;
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
        private void NotifyRequestor(HREFRequestTransferStaff record, string fileName, List<ExDocApproval> approvals)
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
                    string subject = $"Request Transfer/Promotion";
                    string body = $@"Dear <b>{EmpCreate.Title} {EmpCreate.AllName}</b>
                <br/><br/>I’m approved to the  Transfer/Promotion for :
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
            %0AI’m approved to the  Transfer/Promotion for : {staffView.AllName}
            %0Ahas been approved by: {Approve.AllName}
            %0A%0AYours sincerely,
            %0A{Approve.AllName}";

                SYSendTelegramObject Tel = new SYSendTelegramObject { User = User, BS = BS };
                WorkFlowResult result = Tel.Send_SMS_Telegram(message, excfObject.Telegram, false);
            }
        }
        public string Reject(string Document)
        {
            OnLoad();
            string ApprovalID = "";
            try
            {
                string Reject = SYDocumentStatus.REJECTED.ToString();
                string open = SYDocumentStatus.OPEN.ToString();
                string[] c = Document.Split(';');
                foreach (var Doc in c)
                {
                    ApprovalID = Doc;
                    if (Doc == "") continue;
                    var objmatch = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable().FirstOrDefault(w => w.DocNo == Doc);
                    if (objmatch == null)
                    {
                        return "INV_EN";
                    }
                    var _obj = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentType == objmatch.DocType && w.DocumentNo == objmatch.DocNo && w.Approver == User.UserName && w.Status == open);
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
                    var Approve = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == User.UserName);
                    if (EmpCreate != null && !string.IsNullOrEmpty(EmpCreate.Email) && StaffView != null)
                    {
                        var EmailConf = unitOfWork.Set<CFEmailAccount>().AsQueryable().FirstOrDefault();
                        if (EmailConf != null)
                        {
                            CFEmailAccount emailAccount = EmailConf;
                            string subject = "Transfer/Promotion Request";
                            string body = $"Dear <b> {StaffView.Title} {EmpCreate.AllName} </b> <br />";
                            body += $"<br /> I’m reject to the Transfer/Promotion request for  <b> {StaffView.Title} {StaffView.AllName}</b> ";
                            body += $"<br /> <br /> Yours sincerely,<br /> <br /> <b>{Approve.AllName?? User.UserName}</b>";
                            string filePath = "";// fileName; // Ensure fileName is defined
                            string fileName_ = ""; //Path.GetFileName(filePath);

                            EmailObject = new ClsEmail();
                            int rs = EmailObject.SendMail(emailAccount, "", EmpCreate.Email, subject, body, filePath, fileName_);
                        }
                    }
                    var excfObject = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                    if (excfObject != null && !string.IsNullOrEmpty(excfObject.Telegram))
                    {
                        string str = $" Dear <b> {StaffView.Title} {EmpCreate.AllName} </b> %0A";
                        str += $" %0A I’m reject to the Transfer/Promotion for %0A <b>{StaffView.Title} {StaffView.AllName}</b>";
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
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApprovalID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApprovalID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApprovalID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string RequestForApprove(string DocNo, string fileName, string URL)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable().FirstOrDefault(x => x.DocNo == DocNo);
                HRStaffProfile StaffApp = new HRStaffProfile();
                if (objMatch == null) return "DOC_INV";
                string open = SYDocumentStatus.OPEN.ToString();
                var ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentType == objMatch.DocType
                                    && w.DocumentNo == objMatch.DocNo && w.Status == open)
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

                    if (!string.IsNullOrEmpty(StaffApp.Email) && StaffView != null)
                    {
                        try
                        {
                            // Retrieve email configuration
                            var EmailConf = unitOfWork.Set<CFEmailAccount>().AsQueryable().FirstOrDefault();

                            if (EmailConf != null)
                            {
                                CFEmailAccount emailAccount = EmailConf;
                                string str = $"Dear {StaffApp.Title} {StaffApp.AllName}";
                                str += $" <br /> I would like to request staff for Transfer/Promotion <br /> <b>{StaffView.AllName}</b>";

                                if (StaffView.JobCode != objMatch.Position && !string.IsNullOrEmpty(objMatch.Position))
                                {
                                    var pos = unitOfWork.Set<HRPosition>().AsQueryable().FirstOrDefault(w => w.Code == objMatch.Position);
                                    if (pos != null)
                                    {
                                        str += $"<br /> Old Position: {StaffView.Position} New Position: {pos.Description}";
                                    }
                                }
                                if (StaffView.CompanyCode != objMatch.CompanyCode && !string.IsNullOrEmpty(objMatch.CompanyCode))
                                {
                                    str += $"<br /> Old Company: {StaffView.CompanyCode} New Company: {objMatch.CompanyCode}";
                                }
                                str += "<br /> Please review and approve with favor.";
                                str += $"<br /> <br /> Yours sincerely,<br /> <br /> <b>{objMatch.ChangedBy}</b>";

                                // Prepare email details
                                string subject = "Transfer/Promotion Request";
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
                            string str = $"I would like to request staff for Transfer/Promotion %0A <b>{StaffView.AllName}</b>";

                            if (StaffView.JobCode != objMatch.Position && !string.IsNullOrEmpty(objMatch.Position))
                            {
                                var pos = unitOfWork.Set<HRPosition>().AsQueryable().FirstOrDefault(w => w.Code == objMatch.Position);
                                if (pos != null)
                                {
                                    str += $"%0A Old Position: {StaffView.Position} New Position: {pos.Description}";
                                }
                            }

                            if (StaffView.CompanyCode != objMatch.CompanyCode && !string.IsNullOrEmpty(objMatch.CompanyCode))
                            {
                                str += $"%0A Old Company: {StaffView.CompanyCode} New Company: {objMatch.CompanyCode}";
                            }

                            str += "%0A Please review and approve with favor.";
                            str += $"%0A%0AYours sincerely,%0A%0A<b>{objMatch.ChangedBy}</b>";

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
                var objmatch = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable().First(w => w.DocNo == DocNo);
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
        #region JobType
        //public static IEnumerable<HRDivision> GetDivision()
        //{
        //    List<HRDivision> _List = new List<HRDivision>();
        //    var WorkGroup = GetDataCompanyGroup("Division");
        //    if (!string.IsNullOrEmpty(WorkGroup))
        //    {
        //        if (HttpContext.Current.Session["Division"] != null)
        //        {
        //            string CompanyGroup = HttpContext.Current.Session["Division"].ToString();
        //            if (!string.IsNullOrEmpty(CompanyGroup))
        //            {
        //                var _HRList = (from Group in unitOfWork.Set<HRCompanyTree>().AsQueryable()
        //                               join JobType in unitOfWork.Set<HRDivision>().AsQueryable() on Group.CompanyMember equals JobType.Code
        //                               where Group.ParentWorkGroupID == CompanyGroup
        //                               select JobType);
        //                _List = _HRList.ToList();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        _List = unitOfWork.Set<HRDivision>().AsQueryable().ToList();
        //    }
        //    return _List;
        //}
        public IEnumerable<HRDivision> GetDivision()
        {
            string workGroupType = GetDataCompanyGroup("Division");

            if (string.IsNullOrEmpty(workGroupType))
            {
                return unitOfWork.Set<HRDivision>().ToList();
            }

            string sessionDivision = null;
            if (HttpContext.Current?.Session != null)
            {
                sessionDivision = HttpContext.Current.Session["Division"] as string;
            }

            if (string.IsNullOrEmpty(sessionDivision))
            {
                return Enumerable.Empty<HRDivision>();
            }

            return unitOfWork.Set<HRCompanyTree>()
                .Where(g => g.ParentWorkGroupID == sessionDivision)
                .Join(unitOfWork.Set<HRDivision>(),
                    companyTree => companyTree.CompanyMember,
                    division => division.Code,
                    (companyTree, division) => division)
                .Distinct()
                .ToList();
        }
        public IEnumerable<HRDepartment> GetDepartment()
        {
            
            List<HRDepartment> _List = new List<HRDepartment>();
            var WorkGroup = GetDataCompanyGroup("Department");
            if (!string.IsNullOrEmpty(WorkGroup))
            {
                if (HttpContext.Current.Session["Department"] != null)
                {
                    string CompanyGroup = HttpContext.Current.Session["Department"].ToString();
                    if (!string.IsNullOrEmpty(CompanyGroup))
                    {
                        var _HRList = (from Group in unitOfWork.Set<HRCompanyTree>().AsQueryable()
                                       join JobType in unitOfWork.Set<HRDepartment>().AsQueryable() on Group.CompanyMember equals JobType.Code
                                       where Group.ParentWorkGroupID == CompanyGroup
                                       select JobType);
                        _List = _HRList.ToList();
                    }
                }
            }
            else
            {
                _List = unitOfWork.Set<HRDepartment>().AsQueryable().ToList();
            }
            return _List;
        }
        public IEnumerable<HRPosition> GetPosition()
        {
            
            List<HRPosition> _List = new List<HRPosition>();
            var WorkGroup = GetDataCompanyGroup("Position");
            if (!string.IsNullOrEmpty(WorkGroup))
            {
                if (HttpContext.Current.Session["Position"] != null)
                {
                    string CompanyGroup = HttpContext.Current.Session["Position"].ToString();
                    if (!string.IsNullOrEmpty(CompanyGroup))
                    {
                        var _HRList = (from Group in unitOfWork.Set<HRCompanyTree>().AsQueryable()
                                       join JobType in unitOfWork.Set<HRPosition>().AsQueryable() on Group.CompanyMember equals JobType.Code
                                       where Group.ParentWorkGroupID == CompanyGroup
                                       select JobType);
                        _List = _HRList.ToList();
                    }
                }
            }
            else
            {
                _List = unitOfWork.Set<HRPosition>().AsQueryable().ToList();
            }
            return _List;
        }
        public IEnumerable<HRSection> GetSection()
        {
            
            List<HRSection> _List = new List<HRSection>();
            var WorkGroup = GetDataCompanyGroup("Section");
            if (!string.IsNullOrEmpty(WorkGroup))
            {
                if (HttpContext.Current.Session["Section"] != null)
                {
                    string CompanyGroup = HttpContext.Current.Session["Section"].ToString();
                    if (!string.IsNullOrEmpty(CompanyGroup))
                    {
                        var _HRList = (from Group in unitOfWork.Set<HRCompanyTree>().AsQueryable()
                                       join JobType in unitOfWork.Set<HRSection>().AsQueryable() on Group.CompanyMember equals JobType.Code
                                       where Group.ParentWorkGroupID == CompanyGroup
                                       select JobType);
                        _List = _HRList.ToList();
                    }
                }
            }
            else
            {
                _List = unitOfWork.Set<HRSection>().AsQueryable().ToList();
            }
            return _List;
        }
        public IEnumerable<HRLevel> GetLevel()
        {
            SMSystemEntity SMS = new SMSystemEntity();
            
            List<HRLevel> _List = new List<HRLevel>();
            var WorkGroup = GetDataCompanyGroup("Level");
            var ListLevel = SYConstant.getLevelDataAccess();
            if (!string.IsNullOrEmpty(WorkGroup))
            {
                if (HttpContext.Current.Session["Level"] != null)
                {
                    string CompanyGroup = HttpContext.Current.Session["Level"].ToString();
                    if (!string.IsNullOrEmpty(CompanyGroup))
                    {
                        var ListGroup = unitOfWork.Set<HRCompanyTree>().AsQueryable().Where(w => w.ParentWorkGroupID == CompanyGroup).ToList();
                        //var _HRList = (from Group in unitOfWork.Set<HRCompanyTree>().AsQueryable()
                        //               join JobType in SMS.HRLevels on Group.CompanyMember equals JobType.Code
                        //               where Group.ParentWorkGroupID == CompanyGroup
                        //               select JobType).ToList();
                        //_List = _HRList.ToList();
                        var _ListTemp = SMS.HRLevels.ToList();
                        _List = _ListTemp.Where(w => ListGroup.Where(x => x.CompanyMember == w.Code).Any()).ToList();
                    }
                }
            }
            else
            {
                _List = SMS.HRLevels.ToList();
            }
            return _List.Where(w => ListLevel.Where(x => x.Code == w.Code).Any()).ToList();
        }

        public IEnumerable<HRJobGrade> GetJobGrade()
        {
            
            List<HRJobGrade> _List = new List<HRJobGrade>();
            var WorkGroup = GetDataCompanyGroup("JobGrade");
            if (!string.IsNullOrEmpty(WorkGroup))
            {
                if (HttpContext.Current.Session["JobGrade"] != null)
                {
                    string CompanyGroup = HttpContext.Current.Session["JobGrade"].ToString();
                    if (!string.IsNullOrEmpty(CompanyGroup))
                    {
                        var _HRList = (from Group in unitOfWork.Set<HRCompanyTree>().AsQueryable()
                                       join JobType in unitOfWork.Set<HRJobGrade>().AsQueryable() on Group.CompanyMember equals JobType.Code
                                       where Group.ParentWorkGroupID == CompanyGroup
                                       select JobType);
                        _List = _HRList.ToList();
                    }
                }
            }
            else
            {
                _List = unitOfWork.Set<HRJobGrade>().AsQueryable().ToList();
            }
            return _List;
        }
        public string GetDataCompanyGroup(string WorkGroup)
        {
            string Result = "";
            
            var _listCom = unitOfWork.Set<HRCompanyGroup>().AsQueryable().Where(w => w.WorkGroup == WorkGroup).ToList();
            if (_listCom.Count() > 0)
            {
                var obj = _listCom.FirstOrDefault();
                Result = obj.WorkGroup;
            }
            return Result;
        }
        #endregion
        public bool IsHideSalary(string Level)
        {
            var ListLevel = unitOfWork.Set<SYHRModifySalary>().AsQueryable().Where(w => w.UserName == User.UserName && w.Level == Level).ToList();
            if (ListLevel.Count > 0)
            {
                return true;
            }
            //else if (User.IsSalary == true)
            //    return true;
            return false;
        }

    }
    public class HR_HisCareer
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpNameKH { get; set; }
        public string Gender { get; set; }
        public string Company { get; set; }
        public string Branch { get; set; }
        public string Divison { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Level { get; set; }
        public string JobGrad { get; set; }
        public string OldSalary { get; set; }
        public string Increase { get; set; }
        public string NewSalary { get; set; }
        public string CreatedBy { get; set; }
        public string ChangedBy { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Career { get; set; }
        public DateTime StartDate { get; set; }

        public string AttachFile { get; set; }
    }
    public class ClsEmpTransfer
    {
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public DateTime EffectDate { get; set; }
        public string Remark { get; set; }
        public string DocNo { get; set; }
        public string Status { get; set; }
    }
}