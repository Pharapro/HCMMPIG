using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using Humica.Core;
namespace Humica.Logic.MD
{
    public class MDSetting
    {
        private HumicaDBContext DB = new HumicaDBContext();

        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string ActionName { get; set; }
        public static string PARAM_BRANCH = "PARAM_BRANCH";
        public string hide { get; set; }
        public bool isvisible { get; set; }
        public SYHRSetting Header { get; set; }
        public HR_STAFF_VIEW staff { get; set; }
        public List<SYHRSetting> ListHRSetting { get; set; }
        public MDSetting()
        {
            this.User = SYSession.getSessionUser();
            this.BS = SYSession.getSessionUserBS();
        }
        public List<HREmpType> EmpTypes()
        {
            var t = DB.HREmpTypes;
            return t.ToList();
        }
        public string UpdateSetting()
        {
            try
            {
                DB = new HumicaDBContext();
                var ObjMatch = DB.SYHRSettings.First();
                if (Header == null)
                {
                    return "DOC_INV";
                }
                ObjMatch.SeniorityException = Header.SeniorityException;
                ObjMatch.IsTax = Header.IsTax;
                ObjMatch.EmpType = Header.EmpType;
                ObjMatch.SeniorityType = Header.SeniorityType;
                DB.SYHRSettings.Attach(ObjMatch);
                DB.Entry(ObjMatch).Property(x => x.SeniorityException).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.IsTax).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.EmpType).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.SeniorityType).IsModified = true;
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ID.ToString();
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
                log.DocurmentAction = Header.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string UpdateSettingS()
        {
            try
            {
                DB = new HumicaDBContext();
                var ObjMatch = DB.SYHRSettings.First();
                if (Header == null)
                {
                    return "DOC_INV";
                }
                ObjMatch.TelegOT = Header.TelegOT;
                ObjMatch.TelegLeave = Header.TelegLeave;
                ObjMatch.DeductEalary = Header.DeductEalary;
                ObjMatch.DeductLate = Header.DeductLate;
                ObjMatch.MisScanUP = Header.MisScanUP;
                ObjMatch.MisScanAL = Header.MisScanAL;
                ObjMatch.CountMisscan = Header.CountMisscan;
                DB.SYHRSettings.Attach(ObjMatch);
                DB.Entry(ObjMatch).Property(x => x.TelegOT).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.TelegLeave).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.DeductEalary).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.DeductLate).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.MisScanAL).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.MisScanUP).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.CountMisscan).IsModified = true;
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ID.ToString();
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
                log.DocurmentAction = Header.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}