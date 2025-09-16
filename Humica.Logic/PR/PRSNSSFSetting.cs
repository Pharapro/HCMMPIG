using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace Humica.Logic.PR
{
    public class PRSNSSFSetting
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PRSocsec Header { get; set; }
        public string ScreenId { get; set; }
        public string CountryCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<PRSocsec> ListHeader { get; set; }


        public PRSNSSFSetting()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        public string CreateNSSFset()
        {
            try
            {
                var objmast = DB.PRSocsecs.Find(Header.ID);
                if (objmast != null)
                {
                    return "INVALID_CODE";
                }
                Header.CreateOn = DateTime.Now;
                Header.CreateBy = User.UserName;

                DB.PRSocsecs.Add(Header);
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
        }
        public string deleteNSSFset(int id)
        {
            try
            {
                DB = new HumicaDBContext();
                var NSSF = DB.PRSocsecs.Find(id);
                if (NSSF == null)
                {
                    return "CUST_IN_OP";
                }

                DB.PRSocsecs.Attach(NSSF);
                DB.Entry(NSSF).State = System.Data.Entity.EntityState.Deleted;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = id.ToString();
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
                log.ScreenId = id.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string editNSSFset(int id)
        {
            try
            {
                DB = new HumicaDBContext();
                var objCust = DB.PRSocsecs.Find(id);
                if (objCust == null)
                {
                    return "NSSF_NE";
                }
                objCust.AvgGr = Header.AvgGr;
                objCust.HealthStaff = Header.HealthStaff;
                objCust.HealthComp = Header.HealthComp;
                objCust.GrwaGet = Header.GrwaGet;
                objCust.Grwagef = Header.Grwagef;
                objCust.ContriBut = Header.ContriBut;
                objCust.ChangedOn = DateTime.Now;
                objCust.ChangedBy = User.UserName;
                DB.PRSocsecs.Attach(objCust);
                DB.Entry(objCust).State = System.Data.Entity.EntityState.Modified;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = id.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}


