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
using System.Linq;

namespace Humica.Logic.RCM
{
    public class ClsRCMPShortList : IClsRCMPShortList
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public RCMApplicant Header { get; set; }
        public List<RCMApplicant> ListHeader { get; set; }
        public List<RCMAWorkHistory> ListWorkHistory { get; set; }
        public List<RCMAEdu> ListEdu { get; set; }
        public List<RCMALanguage> ListLang { get; set; }
        public List<RCMAReference> ListRef { get; set; }
        public FilterShortLsit Filtering { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsRCMPShortList()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public string Passed(string ApplicantIDs)
        {
            OnLoad();
            try
            {
                string[] ids = ApplicantIDs.Split(';');
                foreach (var id in ids)
                {
                    var applicant = unitOfWork.Repository<RCMApplicant>().Queryable()
                        .FirstOrDefault(x => x.ApplicantID == id);
                    if (applicant == null) continue;
                    //if (!string.IsNullOrEmpty(applicant.ShortList)) continue;
                    bool hasInterview = unitOfWork.Repository<RCMPInterview>().Queryable()
                        .Any(w => w.ApplicantID == id);
                    if (hasInterview) continue;
                    applicant.ShortList = SYDocumentStatus.PASS.ToString();
                    applicant.IntvStep = 1;
                    applicant.CurStage = "ShortListing";
                    applicant.IntVStatus = SYDocumentStatus.OPEN.ToString();
                    applicant.ShortListingBy = User.UserName;
                    applicant.ShortListingDate = DateTime.Now;
                    unitOfWork.Update(applicant);
                }
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantIDs, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantIDs, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantIDs, SYActionBehavior.EDIT.ToString(), e, true);
            }
        }
        public string Reject(string ApplicantIDs)
        {
            OnLoad();
            try
            {
                string rejectStatus = SYDocumentStatus.REJECTED.ToString();
                string[] ids = ApplicantIDs.Split(';');
                foreach (var id in ids)
                {
                    var applicant = unitOfWork.Repository<RCMApplicant>().Queryable()
                        .FirstOrDefault(x => x.ApplicantID == id);
                    if (applicant == null) continue;
                    //if (!string.IsNullOrEmpty(applicant.ShortList)) continue;
                    bool hasInterview = unitOfWork.Repository<RCMPInterview>().Queryable()
                        .Any(w => w.ApplicantID == id);
                    if (hasInterview) continue;
                    applicant.ShortList = rejectStatus;
                    applicant.IntvStep = 0;
                    applicant.CurStage = "ShortListing";
                    applicant.ShortListingBy = User.UserName;
                    applicant.ShortListingDate = DateTime.Now;
                    unitOfWork.Update(applicant);
                }
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantIDs, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantIDs, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantIDs, SYActionBehavior.EDIT.ToString(), e, true);
            }
        }
        public string Fail(string ApplicantIDs)
        {
            OnLoad();
            try
            {
                string[] ids = ApplicantIDs.Split(';');
                foreach (var id in ids)
                {
                    var applicant = unitOfWork.Repository<RCMApplicant>().Queryable()
                        .FirstOrDefault(x => x.ApplicantID == id);
                    if (applicant == null) continue;
                    //if (!string.IsNullOrEmpty(applicant.ShortList)) continue;
                    bool hasInterview = unitOfWork.Repository<RCMPInterview>().Queryable()
                        .Any(w => w.ApplicantID == id);
                    if (hasInterview) continue;
                    applicant.ShortList = "FAIL";
                    applicant.IntvStep = 0;
                    applicant.CurStage = "ShortListing";
                    applicant.ShortListingBy = User.UserName;
                    applicant.ShortListingDate = DateTime.Now;
                    unitOfWork.Update(applicant);
                }
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantIDs, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantIDs, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantIDs, SYActionBehavior.EDIT.ToString(), e, true);
            }
        }
        public string Kept(string ApplicantIDs)
        {
            OnLoad();
            try
            {
                string rejectStatus = SYDocumentStatus.REJECTED.ToString();
                string[] ids = ApplicantIDs.Split(';');
                foreach (var id in ids)
                {
                    var applicant = unitOfWork.Repository<RCMApplicant>().Queryable()
                        .FirstOrDefault(x => x.ApplicantID == id);
                    if (applicant == null) continue;
                    //if (!string.IsNullOrEmpty(applicant.ShortList)) continue;
                    bool hasInterview = unitOfWork.Repository<RCMPInterview>().Queryable()
                        .Any(w => w.ApplicantID == id);
                    if (hasInterview) continue;
                    applicant.ShortList = "KEEP";
                    applicant.IntvStep = 0;
                    applicant.CurStage = "ShortListing";
                    applicant.ShortListingBy = User.UserName;
                    applicant.ShortListingDate = DateTime.Now;
                    unitOfWork.Update(applicant);
                }
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantIDs, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantIDs, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantIDs, SYActionBehavior.EDIT.ToString(), e, true);
            }
        }
    }
    public class FilterShortLsit
    {
        public string Vacancy { get; set; }
        public string ApplyPost { get; set; }
        public string Status { get; set; }
    }
}
