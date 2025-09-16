using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.CF
{
    public class CFHRWorkFlow
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public HRWorkFlow Header { get; set; }
        public HRWorkFlowHeader WHeader { get; set; }
        public string ScreenId { get; set; }
        public string BranchID { get; set; }
        public string Division { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<HRWorkFlow> ListHeader { get; set; }
        public List<HRWorkFlowHeader> ListWHeader { get; set; }
        public CFHRWorkFlow()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }


        public string CreateApprovalWorkflow()
        {
            try
            {

                if (WHeader.BranchID == null || ListHeader.Count == 0)
                {
                    return "EE001";
                }

                //Header.BranchID=WHeader.BranchID;

                HRWorkFlowHeader obj = new HRWorkFlowHeader();
                obj.BranchID = WHeader.BranchID;
                DB.HRWorkFlowHeaders.Add(obj);

                foreach (var read in ListHeader)
                {

                    read.BranchID = WHeader.BranchID;
                    DB.HRWorkFlows.Add(read);

                }

                int row = DB.SaveChanges();

                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.DocurmentAction = "HRWorkFlow";
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
                log.DocurmentAction = "HRWorkFlow";
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
                if (WHeader.BranchID == null || ListHeader.Count == 0)
                {
                    return "EE001";
                }

                var listH = DB.HRWorkFlowHeaders.FirstOrDefault(w => w.ID == ID);

                if (listH == null)
                {
                    return "EE001";
                }

                listH.BranchID = WHeader.BranchID;
                DB.HRWorkFlowHeaders.Attach(listH);
                DB.Entry(listH).Property(w => w.BranchID).IsModified = true;

                var list = DB.HRWorkFlows.Where(w => w.BranchID == listH.BranchID).ToList();
                foreach (var read in list)
                {
                    DB.HRWorkFlows.Remove(read);
                    DB.SaveChanges();
                }


                foreach (var read in ListHeader)
                {

                    read.BranchID = WHeader.BranchID;
                    DB.HRWorkFlows.Add(read);

                }

                int row = DB.SaveChanges();

                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.DocurmentAction = "HRWorkFlow";
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
                log.DocurmentAction = "HRWorkFlow";
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

    }

}