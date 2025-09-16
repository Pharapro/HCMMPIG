using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.CF;
using Humica.Logic.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;

namespace Humica.Logic.EOB
{
    public class ClsEOBHire : IClsEOBHire
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public RCMApplicant Filter { get; set; }
        public RCMHire Hire { get; set; }
        public ClsEmail EmailObject { get; set; }
        public List<RCMHire> ListHire { get; set; }
        public List<ClsWaitingHire> ListWaiting { get; set; }
        public List<RCMApplicant> ListApplicant { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public string MessageError { get; set; }
        public string MessageCode { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public ClsEOBHire()
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
            var listReqProb = unitOfWork.Set<RCMHire>().AsQueryable().Where(w => w.Status == pending).ToList();
            var listApp = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                .Where(w => listReqProb.AsEnumerable().Where(x => w.DocumentNo == x.EmpCode && w.DocumentType == x.StaffType && w.Approver == userName).Any()).ToList();
            foreach (var item in listApp)
            {
                if (item.ApproveLevel == 1 && item.Status == open)
                {
                    var EmpStaff = listReqProb.FirstOrDefault(w => w.EmpCode == item.DocumentNo);
                    if (EmpStaff == null) continue;
                    ListWaiting.Add(new ClsWaitingHire
                    {
                        ApplicantID = EmpStaff.ApplicantID,
                        EmpCode = EmpStaff.EmpCode,
                        ApplicantName = EmpStaff.ApplicantName,
                        StartDate = EmpStaff.StartDate,
                        CompanyCode = EmpStaff.CompanyCode,
                        Branch = EmpStaff.Branch,
                        Department = EmpStaff.Department,
                        Position = EmpStaff.Position,
                        Status = EmpStaff.Status,
                    });
                }
                else if (item.ApproveLevel > 1 && item.Status == open)
                {
                    var level = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == item.DocumentNo && w.DocumentType == item.DocumentType && w.ApproveLevel < item.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                    if (level != null) continue;
                    var EmpStaff = listReqProb.FirstOrDefault(w => w.EmpCode == item.DocumentNo);
                    if (EmpStaff == null) continue;
                    ListWaiting.Add(new ClsWaitingHire
                    {
                        ApplicantID = EmpStaff.ApplicantID,
                        EmpCode = EmpStaff.EmpCode,
                        ApplicantName = EmpStaff.ApplicantName,
                        StartDate = EmpStaff.StartDate,
                        CompanyCode = EmpStaff.CompanyCode,
                        Branch = EmpStaff.Branch,
                        Department = EmpStaff.Department,
                        Position = EmpStaff.Position,
                        Status = EmpStaff.Status,
                    });
                }
            }
        }
        public string CreateHire()
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            string ApplicantID = "";
            try
            {
                if (string.IsNullOrEmpty(Hire.EmployeeType))
                    return "EMPTYPE_EN";
                if (string.IsNullOrEmpty(Hire.CompanyCode))
                    return "COMPANY_EN";
                if (string.IsNullOrEmpty(Hire.Branch))
                    return "BRANCH_EN";
                if (string.IsNullOrEmpty(Hire.Department))
                    return "DEPARTMENT_EN";
                if (string.IsNullOrEmpty(Hire.Position))
                    return "POSITION_EN";
                if (string.IsNullOrEmpty(Hire.PayParameter))
                    return "PAYPARAM_EN";
                if (string.IsNullOrEmpty(Hire.TXPayType))
                    return "SALRY_PAID_EN";
                if (string.IsNullOrEmpty(Hire.Level))
                    return "LEVEL_EN";
                if (string.IsNullOrEmpty(Hire.ProbationType))
                    return "PROBATION_TYPE_EN";
                if (string.IsNullOrEmpty(Hire.StaffType))
                    return "STAFF_TYPE_EN";
                if (Hire.StartDate == null || Hire.StartDate <= DateTime.MinValue)
                    return "START_DATE";
                if (Hire.LeaveConf == null || Hire.LeaveConf <= DateTime.MinValue)
                    return "START_DATE";
                if (!ListApproval.Any()) return "INVALID_APPROVER";

                //var chkHire = unitOfWork.Repository<RCMHire>().Queryable().FirstOrDefault(w => w.ApplicantID == ApplicantID);
                //if (chkHire != null)
                //    return "Candidate already hire";

                //var objNumber = new CFNumberRank(Hire.StaffType, Hire.CompanyCode, Hire.StartDate.Year, true);
                //if (objNumber == null) return "NUMBER_RANK_NE";
                //if (objNumber.NextNumberRank == null) return "NUMBER_RANK_NE";
                //Hire.EmpCode = objNumber.NextNumberRank;
                if (string.IsNullOrEmpty(Hire.EmpCode)) return "INV_EMPCODE";
                ApplicantID = Hire.EmpCode;
                var Staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(x => x.EmpCode == Hire.EmpCode);
                if (Staff != null)
                    return "DUPLICATE_EMPCODE";
                if (ListApproval.Any())
                {
                    ListApproval.ToList().ForEach(h =>
                    {
                        h.DocumentNo = Hire.EmpCode;
                        h.DocumentType = Hire.StaffType;
                        h.Status = SYDocumentStatus.OPEN.ToString();
                        h.WFObject = "HIRE";
                        h.ApprovedBy = "";
                        h.ApprovedName = "";
                        h.ApproverName = "";
                    });
                    unitOfWork.BulkInsert(ListApproval);
                }

                Hire.ApplicantID = ApplicantID;
                Hire.Status = SYDocumentStatus.OPEN.ToString();
                Hire.CreatedBy = User.UserName;
                Hire.CreatedOn = DateTime.Now;

                unitOfWork.Add(Hire);
                unitOfWork.Save();
                unitOfWork.Commit();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string saveHire(string ApplicantID)
        {
            try
            {
                if (string.IsNullOrEmpty(Hire.EmployeeType))
                    return "EMPTYPE_EN";
                if (string.IsNullOrEmpty(Hire.CompanyCode))
                    return "COMPANY_EN";
                if (string.IsNullOrEmpty(Hire.Branch))
                    return "BRANCH_EN";
                if (string.IsNullOrEmpty(Hire.Department))
                    return "DEPARTMENT_EN";
                if (string.IsNullOrEmpty(Hire.Position))
                    return "POSITION_EN";
                if (string.IsNullOrEmpty(Hire.PayParameter))
                    return "PAYPARAM_EN";
                if (string.IsNullOrEmpty(Hire.TXPayType))
                    return "SALRY_PAID_EN";
                if (string.IsNullOrEmpty(Hire.Level))
                    return "LEVEL_EN";
                if (string.IsNullOrEmpty(Hire.ProbationType))
                    return "PROBATION_TYPE_EN";
                if (string.IsNullOrEmpty(Hire.StaffType))
                    return "STAFF_TYPE_EN";
                if (Hire.StartDate == null || Hire.StartDate <= DateTime.MinValue)
                    return "START_DATE";
                if (Hire.LeaveConf == null || Hire.LeaveConf <= DateTime.MinValue)
                    return "START_DATE";
                if (!ListApproval.Any()) return "INVALID_APPROVER";

                var chkHire = unitOfWork.Repository<RCMHire>().Queryable().FirstOrDefault(w => w.ApplicantID == ApplicantID);
                if (chkHire != null)
                    return "Candidate already hire";

                var objNumber = new CFNumberRank(Hire.StaffType, Hire.CompanyCode, Hire.StartDate.Year, true);
                if (objNumber == null) return "NUMBER_RANK_NE";
                if (objNumber.NextNumberRank == null) return "NUMBER_RANK_NE";
                Hire.EmpCode = objNumber.NextNumberRank;
                
                var Staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(x => x.EmpCode == Hire.EmpCode);
                if (Staff != null)
                    return "DUPLICATE_EMPCODE";
                if (ListApproval.Any())
                {
                    ListApproval.ToList().ForEach(h =>
                    {
                        h.DocumentNo = Hire.EmpCode;
                        h.DocumentType = Hire.StaffType;
                        h.Status = SYDocumentStatus.OPEN.ToString();
                        h.WFObject = "HIRE";
                        h.ApprovedBy = "";
                        h.ApprovedName = "";
                        h.ApproverName = "";
                    });
                    unitOfWork.BulkInsert(ListApproval);
                }

                Hire.ApplicantID = ApplicantID;
                Hire.Status = SYDocumentStatus.OPEN.ToString();
                Hire.CreatedBy = User.UserName;
                Hire.CreatedOn = DateTime.Now;

                unitOfWork.Add(Hire);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Delete(string EmpCode)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var objmatch = unitOfWork.Repository<RCMHire>().Queryable().FirstOrDefault(w => w.EmpCode == EmpCode);
                if (objmatch == null) return "DOC_INV";
                var approver = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                  .Where(w => w.DocumentType == objmatch.StaffType && w.DocumentNo == objmatch.EmpCode)
                  .OrderBy(w => w.ApproveLevel).ToList();
                if (approver.Any())
                    unitOfWork.BulkDelete(approver);
                unitOfWork.Delete(objmatch);
                unitOfWork.Save();
                unitOfWork.Commit();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        #region 'Convert Status'
        public string Cancel(string EmpCode)
        {
            OnLoad();
            try
            {
                var objmatch = unitOfWork.Repository<RCMHire>().Queryable().FirstOrDefault(w => w.EmpCode == EmpCode);
                if (objmatch == null) return "DOC_INV";
                objmatch.Status = SYDocumentStatus.CANCELLED.ToString();
                unitOfWork.Update(objmatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Request(string EmpCode)
        {
            OnLoad();
            try
            {
                var objmatch = unitOfWork.Repository<RCMHire>().Queryable().FirstOrDefault(w => w.EmpCode == EmpCode);
                if (objmatch == null) return "DOC_INV";
                objmatch.Status = SYDocumentStatus.PENDING.ToString();

                #region *****Send to Telegram
                var excfObject = unitOfWork.Set<ExCfWorkFlowItem>().AsQueryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                if (excfObject != null && !string.IsNullOrEmpty(excfObject.Telegram))
                {
                    string Createby = objmatch.CreatedBy;
                    var staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == objmatch.CreatedBy);
                    if (staff != null) { Createby = staff.AllName; }
                    string str = $" I would like to request staff new join %0A <b>{objmatch.ApplicantName} </b>";
                    var post = objmatch.ApplyPosition;
                    var PostD = unitOfWork.Repository<HRPosition>().Queryable().FirstOrDefault(w => w.Code == objmatch.ApplyPosition);
                    if (PostD != null) { post = PostD.Description; }
                    str += $"%0A Position: <b>{post}</b> ";
                    str += $"%0A Start Date: <b>{objmatch.StartDate.ToString("dd/MMM/yyyy")}</b> ";
                    str += $"%0A Base salary: <b>{objmatch.Salary}$</b> ";
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
                unitOfWork.Update(objmatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public void SendEmail(string Upload, string Rerceiver)
        {
            try
            {
                #region Email
                var AlertTO = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == Rerceiver);
                var EmailConf = unitOfWork.Repository<CFEmailAccount>().Queryable().FirstOrDefault();
                if (EmailConf != null && AlertTO != null)
                {
                    CFEmailAccount emailAccount = EmailConf;
                    string subject = string.Format("Hire Request Form {0:dd-MMM-yyyy}", DateTime.Today);
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
        public string Approved(string EmpCode)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var objmatch = unitOfWork.Repository<RCMHire>().Queryable().FirstOrDefault(w => w.EmpCode == EmpCode);
                if (objmatch == null) return "DOC_INV";

                string openStatus = SYDocumentStatus.OPEN.ToString();
                var approvalList = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
                    .Where(w => w.DocumentType == objmatch.StaffType && w.DocumentNo == objmatch.EmpCode && w.Status == openStatus)
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

                if (objmatch.Status == SYDocumentStatus.APPROVED.ToString())
                {
                    var App_ = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(x => x.ApplicantID == objmatch.ApplicantID);
                    if (App_ != null)
                    {

                        App_.Status = SYDocumentStatus.HIRED.ToString();
                        App_.IsHired = true;
                        App_.StartDate = objmatch.StartDate;
                        unitOfWork.Update(App_);
                    }
                    else
                    {
                        App_ = new RCMApplicant();
                    }

                    CreateStaff(objmatch, App_);
                    //NotifyRequestor(objmatch, Upload);
                }
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
                                        %0AI’m approved to the hire staff for: {objmatch.ApplicantName}
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
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
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
        //private void NotifyNextApprover(List<ExDocApproval> approvals, string fileName, string openStatus)
        //{
        //    var nextApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
        //        .Where(a => a.Status == openStatus && a.DocumentNo == approvals.FirstOrDefault().DocumentNo
        //               && a.DocumentType == approvals.FirstOrDefault().DocumentType && a.ApproveLevel > approvals.FirstOrDefault().ApproveLevel)
        //        .OrderBy(a => a.ApproveLevel)
        //        .FirstOrDefault();

        //    if (nextApproval == null) return;

        //    var staffProfile = unitOfWork.Set<HRStaffProfile>()
        //        .FirstOrDefault(x => x.EmpCode == nextApproval.Approver);

        //    if (staffProfile == null || string.IsNullOrEmpty(staffProfile.Email)) return;

        //    SendApprovalEmail(staffProfile, fileName);
        //}
        //private void SendApprovalEmail(HRStaffProfile approver, string Upload)
        //{

        //    try
        //    {
        //        var emailConfig = unitOfWork.Set<CFEmailAccount>().FirstOrDefault();
        //        if (emailConfig == null) return;

        //        CFEmailAccount emailAccount = emailConfig;
        //        string subject = string.Format("Hire Request Form {0:dd-MMM-yyyy}", DateTime.Today);
        //        string filePath = Upload;
        //        string fileName = Path.GetFileName(filePath);
        //        EmailObject = new ClsEmail();
        //        int rs = EmailObject.SendMail(emailAccount, "", approver.Email,
        //            subject, "", filePath, fileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("FAIL_TO_SEND_MAIL: " + ex.Message);
        //    }
        //}
        private void UpdateObjStatus(RCMHire record, List<ExDocApproval> approvals, string openStatus)
        {
            record.Status = approvals.Any(w => w.Status == openStatus)
                ? SYDocumentStatus.PENDING.ToString()
                : SYDocumentStatus.APPROVED.ToString();

            unitOfWork.Update(record);
        }
        private void CreateStaff(RCMHire record, RCMApplicant App)
        {
            if (App == null)
            {
                App.FirstName = " ";
                App.LastName = " ";
                App.OthFirstName = " ";
                App.OthLastName = " ";
                App.Gender = "";
                App.Title = "";
                App.Email = null;
                App.Nationality = "CAM";
                App.Marital = "Single";
                App.Country = "CAM";
                App.POB = "";
                App.Phone1 = "";
                App.Phone2 = "";
                App.CurAddr = "";
                App.PermanentAddr = "";
                App.DOB = DateTime.Now;
            }
            var StaffPf = new HRStaffProfile
            {
                EmpCode = record.EmpCode,
                CreatedOn = DateTime.Now,
                CreatedBy = User.UserName,
                DateTerminate = new DateTime(1900, 1, 1),
                ReSalary = DateTime.Now,
                CompanyCode = record.CompanyCode,
                CareerDesc = "NEWJOIN",
                BankName = "CC",
                IsCalSalary = true,
                IsResident = true,
                IsOTApproval = false,
                EmpType = record.EmployeeType,
                IsHold = false,
                Status = SYDocumentStatus.A.ToString(),
                BankFee = 0,
                FirstName = App.FirstName,
                LastName = App.LastName,
                AllName = $"{App.LastName} {App.FirstName}",
                OthFirstName = App.OthFirstName,
                OthLastName = App.OthLastName,
                OthAllName = $"{App.OthLastName} {App.OthFirstName}",
                Sex = App.Gender,
                Title = App.Title,
                Email = App.Email,
                Nation = App.Nationality,
                Marital = App.Marital,
                Country = App.Country,
                POB = App.POB,
                Phone1 = App.Phone1,
                Phone2 = App.Phone2,
                ConAddress = App.CurAddr,
                Peraddress = App.PermanentAddr,
                DOB = App.DOB,
                Branch = record.Branch,
                Division = record.Division,
                DEPT = record.Department,
                LOCT = record.Location,
                CATE = record.Category,
                LevelCode = record.Level,
                JobCode = record.Position,
                Salary = (decimal)record.Salary,
                StartDate = record.StartDate,
                Probation = record.ProbationEndDate,
                PayParam = record.PayParameter,
                JobGrade = record.JobGrade,
                TXPayType = record.TXPayType,
                ROSTER = record.ROSTER,
                LeaveConf = record.LeaveConf,
                Line = record.Line,
                StaffType = record.StaffType,
                Images = record.Images,
                HODCode = record.HODCode,
                SalaryType = record.SalaryType
            };

            int tranNo = unitOfWork.Repository<HREmpCareer>().Queryable()
                          .Select(w => (int?)w.TranNo)
                          .DefaultIfEmpty(0)
                          .Max() ?? 0;
            var HeaderCareer = new HREmpCareer
            {
                TranNo = tranNo + 1,
                EmpCode = record.EmpCode,
                CareerCode = "NEWJOIN",
                EmpType = record.EmployeeType,
                Branch = record.Branch,
                DEPT = record.Department,
                LOCT = record.Location,
                Division = record.Division,
                LINE = record.Line,
                SECT = record.Section,
                CATE = record.Category,
                LevelCode = record.Level,
                JobCode = record.Position,
                CompanyCode = record.CompanyCode,
                EstartSAL = record.Salary.ToString(),
                FromDate = record.StartDate,
                ToDate = new DateTime(5000, 1, 1),
                EffectDate = record.StartDate,
                ProDate = record.StartDate,
                Reason = "New Join",
                Remark = record.Remark,
                Appby = "",
                AppDate = record.StartDate.ToString("dd-MM-yyyy"),
                VeriFyBy = "",
                VeriFYDate = record.StartDate.ToString("dd-MM-yyyy"),
                LCK = 1,
                OldSalary = record.Salary.Value,
                Increase = 0,
                NewSalary = record.Salary.Value,
                JobGrade = record.JobGrade,
                CreateBy = User.UserName,
                CreateOn = DateTime.Now
            };

            unitOfWork.Add(StaffPf);
            unitOfWork.Add(HeaderCareer);
            UpdateBankAccounts(record, tranNo + 1);
        }
        private void UpdateBankAccounts(RCMHire record, long tranNo)
        {
            var empIden = new HREmpCareerBankList
            {
                OldSalary = record.Salary,
                Increase = 0,
                NewSalary = record.Salary,
                Company = record.CompanyCode,
                Reference = tranNo,
                EmpCode = record.EmpCode,
                FromDate = record.StartDate,
                Todate = new DateTime(5000, 1, 1)
            };
            unitOfWork.Add(empIden);

            var bankAcc = new HREmpBankAcc
            {
                EmpCode = record.EmpCode,
                Salary = record.Salary ?? 0,
                Reference = tranNo,
                Company = record.CompanyCode,
                BankName = " ",
                AccountNo = " ",
                AccountName = " ",
                IsTax = false,
                IsNSSF = false,
                Association = false,
                IsActive = true
            };
            unitOfWork.Add(bankAcc);
        }
        //private void NotifyRequestor(RCMHire record, string Upload)
        //{
        //    // 1. Send email to requestor
        //    var EmpCreate = unitOfWork.Set<HRStaffProfile>()
        //        .FirstOrDefault(s => s.EmpCode == record.CreatedBy);

        //    if (EmpCreate != null && !string.IsNullOrEmpty(EmpCreate.Email))
        //    {
        //        var EmailConf = unitOfWork.Set<CFEmailAccount>().FirstOrDefault();
        //        if (EmailConf != null)
        //        {
        //            CFEmailAccount emailAccount = EmailConf;
        //            string subject = string.Format("Hire Request Form {0:dd-MMM-yyyy}", DateTime.Today);
        //            string filePath = Upload;
        //            string fileName = Path.GetFileName(filePath);
        //            EmailObject = new ClsEmail();
        //            int rs = EmailObject.SendMail(emailAccount, "", EmpCreate.Email,
        //                subject, "", filePath, fileName);
        //        }
        //    }
        //}
        public string Reject(string EmpCode)
        {
            try
            {
                string[] c = EmpCode.Split(';');
                foreach (var Doc in c)
                {
                    if (Doc == "") continue;
                    var rejectedStatus = SYDocumentStatus.REJECTED.ToString();
                    var objmatch = unitOfWork.Repository<RCMHire>().Queryable().FirstOrDefault(w => w.EmpCode == Doc);
                    if (objmatch == null) return "INV_EN";
                    if (objmatch.Status == rejectedStatus) return "DOC_RJ_AR";
                    var openStatus = SYDocumentStatus.OPEN.ToString();
                    var approvals = GetOpenApprovals(objmatch, openStatus);
                    var userApprovals = GetUserApprovals();
                    if (!ProcessApprovals(approvals, userApprovals, rejectedStatus)) return "USER_NOT_APPROVOR";
                    objmatch.Status = DetermineFinalStatus(approvals, openStatus);
                    //SendEmailToApprovers(objmatch.EmpCode, User.UserName);
                    //SendEmailToRequester(objmatch.CreatedBy, User.UserName);
                }
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        private List<ExDocApproval> GetOpenApprovals(RCMHire objmatch, string openStatus)
        {
            return unitOfWork.Repository<ExDocApproval>().Queryable()
                .Where(w => w.DocumentType == objmatch.StaffType && w.DocumentNo == objmatch.EmpCode && w.Status == openStatus)
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
        private void SendEmailToApprovers(string EmpCode, string userName)
        {
            var approvals = unitOfWork.Repository<ExDocApproval>().Queryable()
                .Where(w => w.DocumentType == "LR" && w.DocumentNo == EmpCode && w.Status == SYDocumentStatus.APPROVED.ToString()).OrderBy(w => w.ApproveLevel).ToList();
            string subject = "Hire Request Form Reject";
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
                    string subject = "Hire Request Form Reject";
                    SendEmail(emailConfig, alertTo.Email, userName, subject);
                }
            }
        }
        private void SendEmail(CFEmailAccount emailAccount, string recipientEmail, string userName, string subject)
        {
            string body = $"Dear {recipientEmail},<b>{recipientEmail}</b>,<br/>The Hire Request Form has been rejected by {userName}.";
            // Add file path and filename if necessary
            EmailObject = new ClsEmail();
            EmailObject.SendMail(emailAccount, "", recipientEmail, subject, body, "", "");
        }
        #endregion
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
    }
    public class ClsWaitingHire
    {
        public string ApplicantID { get; set; }
        public string EmpCode { get; set; }
        public string ApplicantName { get; set; }
        public DateTime StartDate { get; set; }
        public string CompanyCode { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }

    }
}