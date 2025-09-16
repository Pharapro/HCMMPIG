using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace Humica.Logic.PR
{
    public class PRSOTRate
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PROTRate Header { get; set; }
        public string ScreenId { get; set; }
        public string CompanyCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<PROTRate> ListHeader { get; set; }
        public PRSOTRate()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateOTType()
        {
            try
            {
                if (Header.OTCode == null)
                {
                    return "CODE_EN";
                }
                Header.OTCode = Header.OTCode.ToUpper().Trim();
                Header.OTCode = Header.OTCode.ToUpper();
                var objmast = DB.PROTRates.Find(Header.OTCode);
                if (objmast != null)
                {
                    return "DUP_CODE_EN";
                }
                Header.Measure = "H";
                Header.CreateOn = DateTime.Now;
                Header.CreateBy = User.UserName;

                DB.PROTRates.Add(Header);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.OTCode;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditOTType(string id)
        {
            try
            {
                var objMatch = DB.PROTRates.Find(id);
                if (objMatch == null)
                {
                    return "OTRATE_NE";
                }
                objMatch.OTHDESC = Header.OTHDESC;
                objMatch.OTType = Header.OTType;
                objMatch.Toperand = Header.Toperand;
                objMatch.Soperator = Header.Soperator;
                objMatch.Soperand = Header.Soperand;
                objMatch.ChangedOn = DateTime.Now;
                objMatch.ChangedBy = User.UserName;

                HumicaDBContext DBM = new HumicaDBContext();
                DBM.PROTRates.Attach(objMatch);
                DBM.Entry(objMatch).Property(w => w.OTType).IsModified = true;
                DBM.Entry(objMatch).Property(w => w.OTHDESC).IsModified = true;
                DBM.Entry(objMatch).Property(w => w.Soperator).IsModified = true;
                DBM.Entry(objMatch).Property(w => w.Soperand).IsModified = true;
                DBM.Entry(objMatch).Property(w => w.Toperand).IsModified = true;
                DBM.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                DBM.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                int row = DBM.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.OTCode;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteOTType(string id)
        {
            try
            {

                var objCust = DB.PROTRates.Find(id);
                if (objCust == null)
                {
                    return "OTRATE_NE";
                }

                HumicaDBContext DBM = new HumicaDBContext();
                DBM.PROTRates.Attach(objCust);
                DBM.Entry(objCust).State = System.Data.Entity.EntityState.Deleted;
                int row = DBM.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.OTCode;
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
                log.ScreenId = Header.OTCode;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }


    }

}