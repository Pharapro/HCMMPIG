using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.PR
{
    public class PRClaimBenObject
    {
        public SMSystemEntity DBI = new SMSystemEntity();
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public PREmpClaimBen Header { get; set; }
        public string CompanyCode { get; set; }
        public string Plant { get; set; }
        public string MessageCode { get; set; }
        public List<PR_EmpClaimBen_View> ListHeader { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }

        public PRClaimBenObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateClaim()
        {
            try
            {
                var objmatch = DB.PREmpClaimBens.Where(w => w.EmpCode == HeaderStaff.EmpCode).ToList();
                objmatch = objmatch.Where(w => ((Header.FromDate.Date >= w.FromDate.Date && Header.FromDate.Date <= w.ToDate.Date)
                || (Header.ToDate.Date >= w.FromDate.Date && Header.ToDate.Date <= w.ToDate.Date))).ToList();
                if (objmatch.Count > 0)
                {
                    return "RECORD_EXIST";
                }
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;

                DB.PREmpClaimBens.Add(Header);
                var row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
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
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditClaim(int id)
        {
            try
            {

                var objMatch = DB.PREmpClaimBens.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "BENEFIT_NE";
                }
                Header.EmpCode = objMatch.EmpCode;
                objMatch.FromDate = Header.FromDate;
                objMatch.ToDate = Header.ToDate;
                objMatch.PayDate = Header.PayDate;
                objMatch.Amount = Header.Amount;
                objMatch.Remark = Header.Remark;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                DB.PREmpClaimBens.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Modified;
                int row1 = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderStaff.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string DeleteClaim(int TranNo)
        {
            try
            {
                Header = new PREmpClaimBen();
                var objMatch = DB.PREmpClaimBens.FirstOrDefault(w => w.TranNo == TranNo);
                if (objMatch == null)
                {
                    return "BENEFIT_NE";
                }
                Header.TranNo = TranNo;
                DB.PREmpClaimBens.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Deleted;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.TranNo.ToString();
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
                log.ScreenId = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}
