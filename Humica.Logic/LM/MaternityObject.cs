using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.LM
{
    public class MaternityObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public string Position { get; set; }
        public string EmpName { get; set; }
        public string Department { get; set; }



        public HRReqMaternity Header { get; set; }
        public List<HRReqMaternity> ListHeader { get; set; }
        public MaternityObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateMaternity()
        {
            try
            {

                Header.CreateOn = DateTime.Now;
                Header.CreatedBy = User.UserName;
                if (Header.FromDate.Date > Header.ToDate.Date)
                {
                    return "INVALID_DATE";
                }
                var leaveH = DB.HRReqMaternities.Where(w => w.EmpCode == Header.EmpCode).ToList();
                if (leaveH.Where(w => (w.FromDate.Date >= Header.FromDate.Date && w.ToDate.Date <= Header.FromDate.Date)
                || (w.FromDate.Date <= Header.ToDate.Date && w.ToDate.Date >= Header.ToDate.Date)).Any())
                {
                    return "INV_DATE";
                }
                DB.HRReqMaternities.Add(Header);

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Convert.ToString(Header.TranNo);
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
                log.DocurmentAction = Convert.ToString(Header.TranNo);
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
                log.DocurmentAction = Convert.ToString(Header.TranNo);
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string editMaternity(int id)
        {
            try
            {
                HRReqMaternity objMast = DB.HRReqMaternities.Find(id);
                objMast.FromDate = Header.FromDate;
                objMast.ToDate = Header.ToDate;
                objMast.Remark = Header.Remark;
                objMast.LateEarly = Header.LateEarly;
                objMast.RequestDate = Header.RequestDate;
                objMast.EmpCode = Header.EmpCode;
                objMast.ChangedOn = DateTime.Now.Date;
                objMast.ChangedBy = User.UserName;
                if (Header.FromDate.Date > Header.ToDate.Date)
                {
                    return "INVALID_DATE";
                }

                DB.HRReqMaternities.Attach(objMast);
                DB.Entry(objMast).State = System.Data.Entity.EntityState.Modified;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Convert.ToString(Header.TranNo);
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string deleteMaternity(int TranNo)
        {
            try
            {

                HRReqMaternity objMater = DB.HRReqMaternities.Find(TranNo);

                DB.HRReqMaternities.Attach(objMater);
                DB.Entry(objMater).State = System.Data.Entity.EntityState.Deleted;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Convert.ToString(Header.TranNo);
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
                log.ScreenId = Convert.ToString(Header.TranNo);
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}