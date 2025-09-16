using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.PR
{
    public class PRSParameter
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PRParameter Header { get; set; }
        public string ScreenId { get; set; }
        public string CompanyCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<PRParameter> ListHeader { get; set; }
        public List<BiMonthlySalarySetting> ListBIMonthlySalarySettings { get; set; }

        public BiMonthlySalarySetting BIMonthlySalarySetting { get; set; }
        public PRSParameter()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateParameter()
        {
            try
            {
                if (Header.Code == null)
                {
                    return "CODE_EN";
                }
                Header.Code = Header.Code.ToUpper().Trim();
                var objmast = DB.PRParameters.Find(Header.Code);
                if (objmast != null)
                {
                    return "DUP_CODE_EN";
                }
                Header.CreateOn = DateTime.Now;
                Header.CreateBy = User.UserName;
                BIMonthlySalarySetting.PayrollParameterID = Header.Code;
                BIMonthlySalarySetting.CreatedDateTime = DateTime.Now;
                BIMonthlySalarySetting.CreatedUser = User.UserName;
                DB.BiMonthlySalarySettings.Add(BIMonthlySalarySetting);

                DB.PRParameters.Add(Header);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.Code;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string editParameter(string id)
        {
            try
            {
                var ObjMatch = DB.PRParameters.Find(id);
                var ObjMatchItem = DB.BiMonthlySalarySettings.FirstOrDefault(x => x.PayrollParameterID == id);
                if (ObjMatch == null)
                {
                    return "Parameters_NE";
                }
                ObjMatch.Description = Header.Description;
                ObjMatch.WDMON = Header.WDMON;
                ObjMatch.WDMONDay = Header.WDMONDay;
                ObjMatch.WDTUE = Header.WDTUE;
                ObjMatch.WDTUEDay = Header.WDTUEDay;
                ObjMatch.WDWED = Header.WDWED;
                ObjMatch.WDWEDDay = Header.WDWEDDay;
                ObjMatch.WDTHU = Header.WDTHU;
                ObjMatch.WDTHUDay = Header.WDTHUDay;
                ObjMatch.WDFRI = Header.WDFRI;
                ObjMatch.WDFRIDay = Header.WDFRIDay;
                ObjMatch.WDSAT = Header.WDSAT;
                ObjMatch.WDSATDay = Header.WDSATDay;
                ObjMatch.WDSUN = Header.WDSUN;
                ObjMatch.WDSUNDay = Header.WDSUNDay;
                ObjMatch.WHOUR = Header.WHOUR;
                ObjMatch.WHPWEEK = Header.WHPWEEK;
                ObjMatch.ALWTYPE = Header.ALWTYPE;
                ObjMatch.ALWVAL = Header.ALWVAL;
                ObjMatch.DEDTYPE = Header.DEDTYPE;
                ObjMatch.DEDVAL = Header.DEDVAL;
                ObjMatch.FROMDATE = Header.FROMDATE;
                ObjMatch.TODATE = Header.TODATE;
                ObjMatch.SALWKTYPE = Header.SALWKTYPE;
                ObjMatch.SALWKVAL = Header.SALWKVAL;
                ObjMatch.OTWKTYPE = Header.OTWKTYPE;
                ObjMatch.OTWKVAL = Header.OTWKVAL;
                ObjMatch.ChangeOn = DateTime.Now;
                ObjMatch.ChangedBy = User.UserName;
                if (ObjMatchItem == null)
                {
                    ObjMatchItem = new BiMonthlySalarySetting();
                    ObjMatchItem.PayrollParameterID = ObjMatch.Code;
                    ObjMatchItem.CreatedDateTime = DateTime.Now;
                    ObjMatchItem.CreatedUser = User.UserName;
                    DB.BiMonthlySalarySettings.Add(ObjMatchItem);
                }
                else
                {
                    ObjMatchItem.IsAccrualTax = BIMonthlySalarySetting.IsAccrualTax;
                    ObjMatchItem.IsCalAllowan = BIMonthlySalarySetting.IsCalAllowan;
                    ObjMatchItem.IsCalBounus = BIMonthlySalarySetting.IsCalBounus;
                    ObjMatchItem.IsCalDeduction = BIMonthlySalarySetting.IsCalDeduction;
                    ObjMatchItem.IsCalOvertime = BIMonthlySalarySetting.IsCalOvertime;
                    ObjMatchItem.IsCalNightWork = BIMonthlySalarySetting.IsCalNightWork;
                    ObjMatchItem.IsCalLeaveDed = BIMonthlySalarySetting.IsCalLeaveDed;
                    ObjMatchItem.FirstEndDay = BIMonthlySalarySetting.FirstEndDay;
                    ObjMatchItem.ModifiedUser = User.UserName;
                    ObjMatchItem.ModifiedDateTime = new DateTime?(DateTime.Now);
                    DB.BiMonthlySalarySettings.Attach(ObjMatchItem);
                    DB.Entry(ObjMatchItem).State = System.Data.Entity.EntityState.Modified;
                }

                DB.PRParameters.Attach(ObjMatch);
                DB.Entry(ObjMatch).State = System.Data.Entity.EntityState.Modified;


                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.Code;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string deleteParameter(string id)
        {
            try
            {

                var objCust = DB.PRParameters.Find(id);
                if (objCust == null)
                {
                    return "PAR_NE";
                }
                var objEmp = DB.HRStaffProfiles.ToList();
                if (objEmp.Where(w => w.PayParam == id).Any())
                {
                    return "DATA_USE";
                }
                var ObjMatchItem = DB.BiMonthlySalarySettings.FirstOrDefault(x => x.PayrollParameterID == id);
                HumicaDBContext DBM = new HumicaDBContext();
                if (ObjMatchItem != null)
                {
                    DBM.BiMonthlySalarySettings.Attach(ObjMatchItem);
                    DBM.Entry(ObjMatchItem).State = System.Data.Entity.EntityState.Deleted;
                }

                DBM.PRParameters.Attach(objCust);
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
                log.ScreenId = Header.Code;
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
                log.ScreenId = Header.Code;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }


    }

}