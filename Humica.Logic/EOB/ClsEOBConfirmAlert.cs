using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;

namespace Humica.Logic.EOB
{

    public class ClsEOBConfirmAlert : IClsEOBConfirmAlert
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string DocType { get; set; }
        public EOBConfirmAlert Header { get; set; }
        public RCMApplicant Applicant { get; set; }
        public List<EOBConfirmAlert> ListHeader { get; set; }
        public List<RCMApplicant> ListApplicant { get; set; }
        public ClsEmail EmailObject { get; set; }
        public TPEmailTemplate EmailTemplate { get; set; }
        public string Action { get; set; }
        public SYSplitItem SelectListItem { get; set; }
        public List<object> ListObjectDictionary { get; set; }
        public string MessageError { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public ClsEOBConfirmAlert()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public string ConfAlert(string ID)
        {
            OnLoad();
            try
            {
                var _interview = unitOfWork.Repository<RCMPInterview>().Queryable().FirstOrDefault(w => w.ApplicantID == ID);
                Header.ID = _interview.ApplicantID;
                Header.Name = _interview.CandidateName;
                Header.Status = SYDocumentStatus.OPEN.ToString();
                Header.Confirmed = false;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;

                unitOfWork.Add(Header);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string updConfirm(string ID)
        {
            OnLoad();
            try
            {
                var ObjMatch = unitOfWork.Repository<EOBConfirmAlert>().Queryable().FirstOrDefault(w => w.ID == ID);
                if (ObjMatch == null) return "DOC_INV";
                if (Header.JoinDate == null || Header.JoinDate == DateTime.MinValue)
                    return "Join_Date_is_require";

                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;
                ObjMatch.AttachForm = Header.AttachForm;
                ObjMatch.JoinDate = Header.JoinDate;
                ObjMatch.Confirmed = Header.Confirmed;

                unitOfWork.Update(ObjMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string ApproveTheDoc(string ID, string Upload)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Repository<EOBConfirmAlert>().Queryable().FirstOrDefault(w => w.ID == ID);
                if (objMatch == null)
                    return "INV_DOC";
                if (objMatch.Status != SYDocumentStatus.OPEN.ToString() && objMatch.Status != "CONSIDER")
                    return "INV_DOC";
                var objMatchApp = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == ID);
                if (objMatchApp == null)
                    return "INV_DOC";
                objMatchApp.IsConfirm = true;
                objMatch.Status = SYDocumentStatus.CONFIRMED.ToString();
                objMatch.Confirmed = true;

                unitOfWork.Update(objMatchApp);
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                #region Email
                var _interview = unitOfWork.Repository<RCMPInterview>().Queryable().FirstOrDefault(w => w.ApplicantID == ID);
                var EmailConf = unitOfWork.Repository<CFEmailAccount>().Queryable().FirstOrDefault();
                var _Position = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == ID);
                if (_Position == null) return "INV_DOC";
                var _chkpostdesc = unitOfWork.Repository<HRPosition>().Queryable().FirstOrDefault(w => w.Code == _Position.ApplyPosition);
                if (EmailConf != null)
                {
                    if (objMatch.SendingSelected == "EM" || objMatch.SendingSelected == "Email")
                    {
                        string Position = "";
                        if (_chkpostdesc == null) Position = "";
                        else Position = _chkpostdesc.Description;
                        string str = "Dear Mr./Mrs. " + "<b>" + _interview.CandidateName + "</b>" + " ,";
                        str += "<br /> <p>Congratulations, you have successfully appointed for the role of <b>" + Position + "</b>";
                        str += "<br /> We have attached a file below for you to fullfill your information.";
                        str += "<br /> We look forward to seeing you soon." + "</p>";
                        str += "<br /><br />" + "Thank you!";

                        CFEmailAccount emailAccount = EmailConf;
                        string subject = "Congratulation Letter";
                        string body = str;
                        string filePath = Upload;
                        string fileName = Path.GetFileName(filePath);
                        EmailObject = new ClsEmail();
                        int rs = EmailObject.SendMail(emailAccount, objMatch.Remark, User.Email,
                            subject, body, filePath, fileName);
                    }
                }

                #endregion
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string requestApprove(string ID)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Repository<EOBConfirmAlert>().Queryable().FirstOrDefault(w => w.ID == ID);
                if (objMatch == null)
                    return "REQUEST_NE";
                if (objMatch.Status != SYDocumentStatus.OPEN.ToString() && objMatch.Status != "CONSIDER")
                    return "INV_DOC";
                Header = objMatch;
                objMatch.Status = SYDocumentStatus.PENDING.ToString();
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string RejectTheDoc(string ID)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Repository<EOBConfirmAlert>().Queryable().FirstOrDefault(w => w.ID == ID);
                if (objMatch == null)
                    return "INV_DOC";
                objMatch.Status = SYDocumentStatus.REJECTED.ToString();
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Consider(string ID)
        {
            OnLoad();
            try
            {
                var ObjMatch = unitOfWork.Repository<EOBConfirmAlert>().Queryable().FirstOrDefault(w => w.ID == ID);
                if (ObjMatch == null)
                {
                    return "REQUEST_NE";
                }
                if (ObjMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }
                Header = ObjMatch;
                ObjMatch.Status = "CONSIDER";
                unitOfWork.Update(ObjMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
    }
}