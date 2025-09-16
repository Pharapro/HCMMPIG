using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace Humica.Logic.PR
{
    public class PREmpADVPay
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PRADVPay Header { get; set; }
        public string ScreenId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public string CountryCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<PRADVPay> ListHeader { get; set; }


        public PREmpADVPay()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        public string CreateAdvPay()
        {
            try
            {

                if (EmpCode == null) return "EMPCODE_EN";
                if (Header.Amount <= 0) return "INV_AMOUNT";

                Header.EmpCode = EmpCode;
                Header.AllName = Filter.AllName;
                Header.Balance = Header.Amount;
                Header.CreateBy = User.UserName;
                Header.CreateOn = DateTime.Now;
                DB.PRADVPays.Add(Header);
                int row = DB.SaveChanges();

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

        public string EditAdvPay(int id)
        {
            try
            {
                var ObjMatch = DB.PRADVPays.Find(id);

                if (ObjMatch == null) return "DOC_NE";
                if (Header.Amount <= 0) return "INV_AMOUNT";

                ObjMatch.Amount = Header.Amount;
                ObjMatch.Balance = Header.Balance;
                ObjMatch.TranDate = Header.TranDate;
                ObjMatch.IsBIMonthly = Header.IsBIMonthly;
                ObjMatch.BIPercentageAm = Header.BIPercentageAm;
                ObjMatch.Reason = Header.Reason;
                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;

                DB.PRADVPays.Attach(ObjMatch);
                DB.Entry(ObjMatch).Property(w => w.Amount).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.Balance).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.TranDate).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.Reason).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.IsBIMonthly).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.BIPercentageAm).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(ObjMatch).Property(w => w.ChangedOn).IsModified = true;
                int row = DB.SaveChanges();

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
        public string DeleteAdvPay(int id)
        {
            try
            {
                var ObjMatch = DB.PRADVPays.Find(id);

                if (ObjMatch == null) return "DOC_NE";

                DB.PRADVPays.Remove(ObjMatch);
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
    }
}


