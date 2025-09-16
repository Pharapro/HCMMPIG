using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.Att
{

    public class ATReplaceWorkObject
    {
        public HumicaDBContext DB = new HumicaDBContext();

        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public ATEmpRelWork Header { get; set; }
        public string ScreenId { get; set; }
        public List<ATEmpRelWork> ListRelWork { get; set; }
        public ATReplaceWorkObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateWork()
        {
            try
            {
                DB = new HumicaDBContext();
                Header.CreatedBy = User.UserName;
                Header.CreateOn = DateTime.Now;
                if(Header.ToEmpCode == null)
                {
                    return "EMP_BLANK";
                }
                if(Header.Remark==null || Header.Remark == "")
                {
                    return "REMARK_RE";
                }
                if(Header.RequestDate == null || Header.InDate == null)
                {
                    return "INV_DATE";
                }
                DB.ATEmpRelWorks.Add(Header);

                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Header.;
                //log.DocurmentAction = ActionName;
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
                //log.DocurmentAction = Header.;
                //log.DocurmentAction = ActionName;
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
                //log.DocurmentAction = Header.workCode;
                //log.DocurmentAction = ActionName;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditWork(int TranNo)
        {
            try
            {
                DB = new HumicaDBContext();
                var work = DB.ATEmpRelWorks.FirstOrDefault(x => x.TranNo == TranNo);

                work.ChangedBy = User.UserName;
                work.ChangedOn = DateTime.Now;
                if (Header.ToEmpCode == null)
                {
                    return "EMP_BLANK";
                }
                if (Header.Remark == null || Header.Remark == "")
                {
                    return "REMARK_RE";
                }
                if (Header.RequestDate == null || Header.InDate == null)
                {
                    return "INV_DATE";
                }
                work.RequestDate = Header.RequestDate;
                work.InDate = Header.InDate;
                work.FromEmpCode = Header.FromEmpCode;
                work.ToEmpCode = Header.ToEmpCode;
                work.Remark = Header.Remark;
                DB.ATEmpRelWorks.Attach(work);
                DB.Entry(work).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(work).Property(x => x.ChangedOn).IsModified = true;
                DB.Entry(work).Property(x => x.RequestDate).IsModified = true;
                DB.Entry(work).Property(x => x.InDate).IsModified = true;
                DB.Entry(work).Property(x => x.FromEmpCode).IsModified = true;
                DB.Entry(work).Property(x => x.ToEmpCode).IsModified = true;
                DB.Entry(work).Property(x => x.Remark).IsModified = true;

                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Header.workCode;
                //log.DocurmentAction = ActionName;
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
                //log.DocurmentAction = Header.workCode;
                //log.DocurmentAction = ActionName;
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
                //log.DocurmentAction = ActionName;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteWork(int TranNo)
        {
            try
            {
                DB = new HumicaDBContext();
                var work = DB.ATEmpRelWorks.FirstOrDefault(w => w.TranNo == TranNo);

                DB.ATEmpRelWorks.Remove(work);

                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Header.workCode;
                //log.DocurmentAction = ActionName;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();

                log.UserId = User.UserName;
                //log.DocurmentAction = Header.workCode;
                //log.DocurmentAction = ActionName;
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
                //log.DocurmentAction = Header.workCode;
                //log.DocurmentAction = ActionName;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}