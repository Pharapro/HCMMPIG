using Humica.EF.MD;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System;
using System.Linq;
using Humica.Logic.CF;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Core.DB;
using Humica.EF.Repo;
using Humica.Core.SY;

namespace Humica.Logic.RCM
{
    public class ClsRCMERecruit : IClsRCMERecruit
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string DocType { get; set; }
        public string ID { get; set; }
        public RCMERecruit Header { get; set; }
        public List<RCMERecruit> ListHeader { get; set; }
        public List<RCMERecruitD> ListDetails { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsRCMERecruit()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }

        #region Create
        public string createERecruit()
        {
            OnLoad();
            try
            {
                if (ListDetails.Count <= 0) return "INVALID_DETAILS";
                if (Header.MediaType == null)
                    return "FB_TYPE";
                DateTime currentdate = DateTime.Now;
                string CompanyCode = "";
                var staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == User.UserName);
                if (staff != null) CompanyCode = staff.CompanyCode;
                else
                {
                    var Com_ = unitOfWork.Repository<HRCompany>().Queryable().FirstOrDefault();
                    if (Com_ != null)
                        CompanyCode = Com_.Company;
                }
                var objNumber = new CFNumberRank(DocType, CompanyCode, currentdate.Year, true);
                if (objNumber == null) return "NUMBER_RANK_NE";
                if (objNumber.NextNumberRank == null) return "NUMBER_RANK_NE";

                Header.JobID = objNumber.NextNumberRank;
                Header.Status = SYDocumentStatus.OPEN.ToString();
                Header.PostedDate = DateTime.Now;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                int noRec = 0;
                foreach (var item in ListDetails)
                {
                    var objItem = new RCMERecruitD
                    {
                        JobID = Header.JobID,
                        RequestNo = item.RequestNo,
                        Position = item.Position,
                        NoOfRecruit = item.NoOfRecruit
                    };
                    unitOfWork.Add(objItem);
                    noRec += item.NoOfRecruit ?? 0;
                }
                Header.PosterNo = noRec;
                unitOfWork.Add(Header);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.JobID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.JobID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.JobID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        #endregion 
        #region Update
        public string updERecruit(string JobID)
        {
            OnLoad();
            try
            {
                if (!ListDetails.Any()) return "INVALID_DETAILS";
                var ObjMatch = unitOfWork.Repository<RCMERecruit>().Queryable().FirstOrDefault(w => w.JobID == JobID);
                if (ObjMatch == null) return "DOC_INV";
                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;
                ObjMatch.Location = Header.Location;
                ObjMatch.ContactInfo = Header.ContactInfo;
                ObjMatch.EndDate = Header.EndDate;
                if (!string.IsNullOrEmpty(Header.Attachfile))
                    ObjMatch.Attachfile = Header.Attachfile;
                ObjMatch.MediaType = Header.MediaType;
                var existingRecords = unitOfWork.Repository<RCMERecruitD>().Queryable().Where(w => w.JobID == JobID).ToList();
                int NoRec = 0;
                foreach (var item in ListDetails)
                {
                    var objitem = new RCMERecruitD
                    {
                        JobID = ObjMatch.JobID,
                        RequestNo = item.RequestNo,
                        Position = item.Position,
                        NoOfRecruit = item.NoOfRecruit ?? 0,
                    };
                    var existingItem = existingRecords.FirstOrDefault(e => e.RequestNo == item.RequestNo);
                    if (existingItem != null)
                    {
                        existingItem.Position = item.Position;
                        existingItem.NoOfRecruit = item.NoOfRecruit ?? 0;
                        unitOfWork.Update(existingItem);
                    }
                    else
                        unitOfWork.Add(objitem);
                    NoRec += item.NoOfRecruit ?? 0;
                }

                ObjMatch.PosterNo = NoRec;
                unitOfWork.Update(ObjMatch);
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, JobID, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, JobID, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, JobID, SYActionBehavior.EDIT.ToString(), e, true);
            }
        }

        #endregion 
        #region Delete
        public string deleteERecruit(string JobID)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var ObjMatch = unitOfWork.Repository<RCMERecruit>().Queryable().FirstOrDefault(w => w.JobID == JobID);
                if (ObjMatch == null)
                    return "DOC_INV";
                string Approve = SYDocumentStatus.APPROVED.ToString();
                if (ObjMatch.Status == Approve)
                    return "DOC_INV";
                var obj = unitOfWork.Repository<RCMERecruitD>().Queryable().Where(w => w.JobID == JobID).ToList();
                if (obj.Any())
                    unitOfWork.BulkDelete(obj);
                unitOfWork.Delete(ObjMatch);
                unitOfWork.Save();
                unitOfWork.Commit();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, JobID, SYActionBehavior.DELETE.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, JobID, SYActionBehavior.DELETE.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, JobID, SYActionBehavior.DELETE.ToString(), e, true);
            }
        }
        #endregion
        #region UpdateAfferPost
        public string UpdateAfferPost(string JobID)
        {
            OnLoad();
            try
            {
                var ObjMatch = unitOfWork.Repository<RCMERecruit>().Queryable().FirstOrDefault(w => w.JobID == JobID);
                if (ObjMatch == null)
                    return "DOC_INV";
                string user = User.UserName;
                var Staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == User.UserName);
                if (Staff != null) user = Staff.AllName;

                ObjMatch.PostedDate = DateTime.Now;
                ObjMatch.PersonInCharge = user;
                ObjMatch.Status = SYDocumentStatus.POSTED.ToString();
                unitOfWork.Update(ObjMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, JobID, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, JobID, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, JobID, SYActionBehavior.EDIT.ToString(), e, true);
            }
        }

        #endregion 
    }
}
