using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.Atts
{
    public class ATMealInOutSetupObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public ATMealSetup Header { get; set; }
        //public ATMealSetupItem Item { get; set; }
        public List<ATMealSetup> ListHeader { get; set; }
        public List<ATMealSetupItem> ListItem { get; set; }
        public ATMealInOutSetupObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string updateMealSetup()
        {
            try
            {
                DB = new HumicaDBContext();
                var ObjMatch = DB.ATMealSetups.FirstOrDefault();
                Header.BreakfastFrom = DateTimeHelper.SetTimeInDate(Header.BreakfastFrom);
                Header.BreakfastTo = DateTimeHelper.SetTimeInDate(Header.BreakfastTo);
                Header.LunchFrom = DateTimeHelper.SetTimeInDate(Header.LunchFrom);
                Header.LunchTo = DateTimeHelper.SetTimeInDate(Header.LunchTo);
                Header.DinnerFrom = DateTimeHelper.SetTimeInDate(Header.DinnerFrom);
                Header.DinnerTo = DateTimeHelper.SetTimeInDate(Header.DinnerTo);
                Header.NightFrom = DateTimeHelper.SetTimeInDate(Header.NightFrom);
                Header.NightTo = DateTimeHelper.SetTimeInDate(Header.NightTo);
                if (ObjMatch == null)
                {
                    Header.CreatedBy = User.UserName;
                    Header.CreatedOn = DateTime.Now;
                    DB.ATMealSetups.Add(Header);
                }
                else
                {
                    ObjMatch.MealAllowanceType = Header.MealAllowanceType;
                    ObjMatch.BreakfastFrom = Header.BreakfastFrom;
                    ObjMatch.BreakfastTo = Header.BreakfastTo;
                    ObjMatch.LunchFrom = Header.LunchFrom;
                    ObjMatch.LunchTo = Header.LunchTo;
                    ObjMatch.DinnerFrom = Header.DinnerFrom;
                    ObjMatch.DinnerTo = Header.DinnerTo;
                    ObjMatch.NightFrom = Header.NightFrom;
                    ObjMatch.NightTo = Header.NightTo;
                    ObjMatch.OTAdj = Header.OTAdj;
                    ObjMatch.WDAdj = Header.WDAdj;
                    ObjMatch.ChangedBy = User.UserName;
                    ObjMatch.ChangedOn = DateTime.Now;

                    DB.ATMealSetups.Attach(ObjMatch);

                    DB.Entry(ObjMatch).Property(x => x.MealAllowanceType).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.BreakfastFrom).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.BreakfastTo).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.LunchFrom).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.LunchTo).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.DinnerFrom).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.DinnerTo).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.NightFrom).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.NightTo).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.OTAdj).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.WDAdj).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.ChangedOn).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.ChangedBy).IsModified = true;
                }
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
                log.Action = SYActionBehavior.ADD.ToString();

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
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

    }
}