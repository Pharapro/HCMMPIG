using Humica.EF.MD;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System;
using System.Linq;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Core.DB;
using Humica.EF.Repo;
using Humica.Core.SY;

namespace Humica.Logic.RCM
{
    public class ClsRCMRefCheck : IClsRCMRefCheck
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public RCMApplicant Filter { get; set; }
        public RCMRefCheckPerson Header { get; set; }
        public RCMAReference RefPerson { get; set; }
        public List<RCMRefCheckPerson> ListRefPerson { get; set; }
        public List<RCMApplicant> ListApplicant { get; set; }
        public List<RCMRefQuestionnaire> ListRefQuestion { get; set; }
        public string ErrorMessage { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsRCMRefCheck()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }

        public string RefCheck()
        {
            OnLoad();
            try
            {
                if (Header.NameOfRef == null) return "EEREFPERSON";
                var chkRef = unitOfWork.Repository<RCMRefCheckPerson>().Queryable().FirstOrDefault(w => w.ApplicantID == Header.ApplicantID);
                if (chkRef != null) return "Candidate already check reference!";
                if (ListRefQuestion.Any())
                {
                    ListRefQuestion.ToList().ForEach(h =>
                    {
                        h.ApplicantID = Header.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListRefQuestion);
                }
                var _App = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == Header.ApplicantID);
                if (_App != null)
                {
                    _App.RefCHK = true;
                    unitOfWork.Update(_App);
                }
                unitOfWork.Add(Header);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string UpdateRefCheck(string ApplicantID)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var _chkRef = unitOfWork.Repository<RCMRefCheckPerson>().Queryable().FirstOrDefault(w => w.ApplicantID == ApplicantID);

                if (_chkRef == null) return "DOC_NE";
                var objMatch = unitOfWork.Repository<RCMRefQuestionnaire>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                if (objMatch.Any())
                    unitOfWork.Delete(objMatch);
                if (ListRefQuestion.Any())
                {
                    ListRefQuestion.ToList().ForEach(h =>
                    {
                        h.ApplicantID = ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListRefQuestion);
                }
                _chkRef.NameOfRef = Header.NameOfRef;
                _chkRef.OccupationOfRef = Header.OccupationOfRef;
                _chkRef.CompanyOfRef = Header.CompanyOfRef;
                _chkRef.PhoneNo = Header.PhoneNo;
                _chkRef.ReasonForLeaving = Header.ReasonForLeaving;
                _chkRef.Attachment = Header.Attachment;
                _chkRef.Description = Header.Description;
                _chkRef.Relationship = Header.Relationship;
                _chkRef.RefChkDate = Header.RefChkDate;
                _chkRef.CompanyCan = Header.CompanyCan;
                _chkRef.PositionCan = Header.PositionCan;
                unitOfWork.Update(_chkRef);
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
    }
}
