using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.CF
{
    public class CFWorkFlowLeave
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public HRWorkFlowLeave Header { get; set; }
        public string ScreenId { get; set; }
        public string BranchID { get; set; }
        public string Division { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<HRWorkFlowLeave> ListHeader { get; set; }
        public CFWorkFlowLeave()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateApprovalWorkflow()
        {
            try
            {
                var DBI = new HumicaDBContext();

                if (Header.Code == null) return "CODE_EN";

                var chkdup = DBI.HRWorkFlowLeaves.FirstOrDefault(w => w.Code == Header.Code);
                if (chkdup != null) return "DUP_CODE_EN";

                Header.CreateBy = User.UserName;
                Header.CreateOn = DateTime.Now;

                DBI.HRWorkFlowLeaves.Add(Header);
                int row = DBI.SaveChanges();

                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.DocurmentAction = "HRWorkFlowLeave";
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
                log.UserId = User.UserID.ToString();
                log.DocurmentAction = "HRWorkFlowLeave";
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string UpdateApprovalWorkflow(int ID)
        {
            try
            {
                var ObjMatch = DB.HRWorkFlowLeaves.Find(ID);
                if (ObjMatch == null)
                {
                    return "INVALID_DATA";
                }

                ObjMatch.Description = Header.Description;
                ObjMatch.Step = Header.Step;
                ObjMatch.Qty = Header.Qty;
                ObjMatch.AssignBy = Header.AssignBy;
                ObjMatch.ApproveBy = Header.ApproveBy;
                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;

                DB.HRWorkFlowLeaves.Attach(ObjMatch);
                DB.Entry(ObjMatch).State = System.Data.Entity.EntityState.Modified;
                int row = DB.SaveChanges();

                return "OK";
            }

            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.DocurmentAction = "HRWorkFlowLeave";
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.DocurmentAction = "HRWorkFlowLeave";
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteWFL(int ID)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                try
                {

                    var ObjMatch = DB.HRWorkFlowLeaves.Find(ID);
                    if (ObjMatch == null)
                    {
                        return "INVALID_DATA";
                    }

                    DB.HRWorkFlowLeaves.Attach(ObjMatch);
                    DB.Entry(ObjMatch).State = System.Data.Entity.EntityState.Deleted;

                    int row = DB.SaveChanges();
                    return SYConstant.OK;
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserID.ToString();
                    log.ScreenId = ID.ToString();
                    log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

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
                    log.ScreenId = ID.ToString();
                    log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
    }

}