using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.MD
{
    public class ClsAlertSetting
    {
        private HumicaDBContext DB = new HumicaDBContext();

        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string ActionName { get; set; }
        public static string PARAM_BRANCH = "PARAM_BRANCH";
        public SYSHRAlert Header { get; set; }
        public List<SYSHRAlert> ListAlert { get; set; }

        public ClsAlertSetting()
        {
            this.User = SYSession.getSessionUser();
            this.BS = SYSession.getSessionUserBS();
        }

        #region Update
        public string UpdateG()
        {
            try
            {
                DB = new HumicaDBContext();
                var ObjMatch = DB.SYSHRAlerts.FirstOrDefault(w => w.TranNo == 1);
                if (Header == null)
                {
                    return "DOC_INV";
                }
                ObjMatch.BDCHK = Header.BDCHK;
                ObjMatch.PBCHK = Header.PBCHK;
                ObjMatch.CECHK = Header.CECHK;
                ObjMatch.TMCHK = Header.TMCHK;
                ObjMatch.BDValue = Header.BDValue;
                ObjMatch.PBValue = Header.PBValue;
                ObjMatch.CEValue = Header.CEValue;
                ObjMatch.TMValue = Header.TMValue;
                ObjMatch.PPCHK = Header.PPCHK;
                ObjMatch.WBCHK = Header.WBCHK;
                ObjMatch.VISACHK = Header.VISACHK;
                ObjMatch.PPValue = Header.PPValue;
                ObjMatch.WBValue = Header.WBValue;
                ObjMatch.VISAValue = Header.VISAValue;
                ObjMatch.OpenShirtDateCHK = Header.OpenShirtDateCHK;
                ObjMatch.OPENSHIRTDATEValue = Header.OPENSHIRTDATEValue;

                DB.SYSHRAlerts.Attach(Header);

                DB.Entry(ObjMatch).Property(x => x.BDCHK).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.PBCHK).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.CECHK).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.TMCHK).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.BDValue).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.PBValue).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.CEValue).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.TMValue).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.PPCHK).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.WBCHK).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.VISACHK).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.PPValue).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.WBValue).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.VISAValue).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.OpenShirtDateCHK).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.OPENSHIRTDATEValue).IsModified = true;

                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.TranNo.ToString();
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
                log.DocurmentAction = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion

    }
}