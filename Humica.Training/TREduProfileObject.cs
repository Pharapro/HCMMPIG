using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Training
{
    public class TREduProfileObject
    {
        public string ScreenId { get; set; }
        HumicaDBContextTR DB = new HumicaDBContextTR();
        Humica.Core.DB.HumicaDBContext DBX = new Core.DB.HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public TREducationProfile Header { get; set; }
        public List<TREducationProfile> ListHeader { get; set; }

        public string ErrorMessage { get; set; }
        public List<Core.DB.HR_STAFF_VIEW> ListStaff { get; set; }

        public TREduProfileObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateEduPro()
        {
            try
            {
                DB = new HumicaDBContextTR();
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;

                DB.TREducationProfile.Add(Header);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

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
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditEduPro(int id)
        {
            try
            {
                var objMatch = DB.TREducationProfile.FirstOrDefault(w => w.ID == id);
                if (objMatch == null)
                {
                    return "FAMILY_NE";
                }

                objMatch.QualificationType = Header.QualificationType;
                objMatch.QualificationClass = Header.QualificationClass;
                objMatch.QualificationName = Header.QualificationName;
                objMatch.FirstSubject = Header.FirstSubject;
                objMatch.SecondSubject = Header.SecondSubject;
                objMatch.DateAwarded = Header.DateAwarded;
                objMatch.AwardingInstitution = Header.AwardingInstitution;
                objMatch.AttachFile = Header.AttachFile;
                objMatch.ChangedBy = Header.ChangedBy;
                objMatch.ChangedOn = Header.ChangedOn;

                DB.TREducationProfile.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Modified;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteEduPro(int id)
        {
            try
            {
                Header = new TREducationProfile();
                var objMatch = DB.TREducationProfile.FirstOrDefault(w => w.ID == id);
                if (objMatch == null)
                {
                    return "EDCUATION _NE";
                }
                Header = objMatch;
                DB.TREducationProfile.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Deleted;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}