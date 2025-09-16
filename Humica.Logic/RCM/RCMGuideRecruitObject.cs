using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.RCM
{
    public class RCMGuideRecruitObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string MessageError { get; set; }
        public string ScreenId { get; set; }
        public string DocType { get; set; }
        public RCMGuideRecruit Header { get; set; }
        public List<RCMGuideRecruit> ListHeader { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }

        public RCMGuideRecruitObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        #region 'Create'
        public string createGuideRecruit()
        {
            try
            {
                if (Header.StaffRequestNo <= 0) return ("EESTAFFREQNO");
                if (Header.PositionRequest <= 0) return ("EEPOSTREQNO");
                if (Header.DEPT == null) return ("DEPARTMENT_EN");

                var objCF = DB.ExCfWorkFlowItems.Find(ScreenId, DocType);
                if (objCF == null) return "REQUEST_TYPE_NE";

                var objNumber = new CFNumberRank(objCF.DocType, objCF.ScreenID);
                if (objNumber.NextNumberRank == null) return "NUMBER_RANK_NE";

                Header.GuideRecruitNo = objNumber.NextNumberRank;

                Header.Status = SYDocumentStatus.OPEN.ToString();
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                DB.RCMGuideRecruits.Add(Header);

                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.GuideRecruitNo;
                log.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.GuideRecruitNo;
                log.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.GuideRecruitNo;
                log.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                return "EE001";
            }
        }
        #endregion 
        #region 'Update'
        public string updGuideRecruit(string GuideRecruitNo)
        {
            try
            {
                if (Header.StaffRequestNo <= 0) return "EESTAFFREQNO";
                if (Header.PositionRequest <= 0) return "EEPOSTREQNO";
                if (Header.DEPT == null) return "DEPARTMENT_EN";


                DB = new HumicaDBContext();

                var ObjMatch = DB.RCMGuideRecruits.FirstOrDefault(w => w.GuideRecruitNo == GuideRecruitNo);
                if (ObjMatch == null) return "DOC_INV";

                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;

                ObjMatch.DEPT = Header.DEPT;
                ObjMatch.StaffRequestNo = Header.StaffRequestNo;
                ObjMatch.PositionRequest = Header.PositionRequest;
                ObjMatch.Gender = Header.Gender;
                ObjMatch.WorkingType = Header.WorkingType;
                ObjMatch.RequestedDate = Header.RequestedDate;
                ObjMatch.Attachment = Header.Attachment;

                DB.RCMGuideRecruits.Attach(ObjMatch);

                DB.Entry(ObjMatch).Property(x => x.DEPT).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.StaffRequestNo).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.PositionRequest).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Gender).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.WorkingType).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.RequestedDate).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Attachment).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChangedOn).IsModified = true;

                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.GuideRecruitNo;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.GuideRecruitNo;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                return "EE001";
            }
        }
        #endregion 
        #region 'Delete'
        public string deleteGuideRecruit(string GuideRecruitNo)
        {
            try
            {
                DB = new HumicaDBContext();

                var ObjMatch = DB.RCMGuideRecruits.FirstOrDefault(w => w.GuideRecruitNo == GuideRecruitNo);
                string Approve = SYDocumentStatus.APPROVED.ToString();

                if (ObjMatch == null) return "DOC_INV";
                if (ObjMatch.Status == Approve) return "DOC_INV";


                DB.RCMGuideRecruits.Remove(ObjMatch);
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = GuideRecruitNo;
                log.Action = SYActionBehavior.DELETE.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = GuideRecruitNo;
                log.Action = SYActionBehavior.DELETE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = GuideRecruitNo;
                log.Action = SYActionBehavior.DELETE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion 
        #region 'Status'
        public string Approved(string GuideRecruitNo)
        {
            try
            {
                HumicaDBContext DBI = new HumicaDBContext();

                RCMGuideRecruit objmatch = DB.RCMGuideRecruits.FirstOrDefault(w => w.GuideRecruitNo == GuideRecruitNo);

                if (objmatch == null) return "DOC_INV";

                objmatch.Status = SYDocumentStatus.APPROVED.ToString();
                DB.RCMGuideRecruits.Attach(objmatch);
                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;

                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = GuideRecruitNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = GuideRecruitNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = GuideRecruitNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                return "EE001";
            }
        }
        public string Cancel(string GuideRecruitNo)
        {
            try
            {
                HumicaDBContext DBI = new HumicaDBContext();

                RCMGuideRecruit objmatch = DB.RCMGuideRecruits.FirstOrDefault(w => w.GuideRecruitNo == GuideRecruitNo);
                if (objmatch == null) return "DOC_INV";

                objmatch.Status = SYDocumentStatus.CANCELLED.ToString();
                objmatch.ChangedBy = User.UserName;
                objmatch.ChangedOn = DateTime.Now;

                DB.RCMGuideRecruits.Attach(objmatch);

                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;

                DB.SaveChanges();
                return SYConstant.OK;


            }
            catch (DbEntityValidationException e)
            {
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = GuideRecruitNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e);
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = GuideRecruitNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                return "EE001";
            }
            catch (Exception e)
            {
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = GuideRecruitNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                return "EE001";
            }
        }
        #endregion 
    }
}
