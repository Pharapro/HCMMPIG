using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.HR
{
    public class HRAchievmentObject
    {
        public SMSystemEntity DP = new SMSystemEntity();
        HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public HREmpAchieve Header { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<HREmpAchieve> ListHeader { get; set; }
        public SYUser UserObject { get; set; }
        public SYUserBusiness UserBusinessObject { get; set; }
        public HRAchievmentObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string createAchiev()
        {
            DB = new HumicaDBContext();
            try
            {
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.CreatedOn = DateTime.Now.Date;
                Header.CreatedBy = User.UserName;

                DB.HREmpAchieves.Add(Header);

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
            catch (Exception e)
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
        public string EditEmpAchiev(int id)
        {
            try
            {
                Header.EmpCode = HeaderStaff.EmpCode;
                HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = DB.HREmpAchieves.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                objMatch.FromDate = Header.FromDate;
                objMatch.ToDate = Header.ToDate;
                objMatch.Remark = Header.Remark;
                objMatch.Description = Header.Description;

                DB.HREmpAchieves.Attach(objMatch);
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
                log.DocurmentAction = HeaderStaff.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteEmpAchiev(int id)
        {
            Header = new HREmpAchieve();
            try
            {
                var objMatch = DB.HREmpAchieves.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "DOC_RM";
                }
                Header.TranNo = id;
                DB.HREmpAchieves.Attach(objMatch);
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
                log.ScreenId = Header.TranNo.ToString();
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
                log.ScreenId = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}