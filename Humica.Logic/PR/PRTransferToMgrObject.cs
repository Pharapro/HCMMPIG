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
    public class PRTransferToMgrObject
    {
        public SMSystemEntity DBI = new SMSystemEntity();
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public DateTime PaymentDate { get; set; }
        public PRTransferToMgr Header { get; set; }
        public PRTransferToMgrItem Item { get; set; }
        public List<PRTransferToMgr> ListHeader { get; set; }
        public List<PRTransferToMgrItem> ListItem { get; set; }
        public List<HR_STAFF_VIEW> ListView { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }

        public PRTransferToMgrObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string TransferToMgr()
        {
            try
            {
                //var _chkTransfer = DB.PRTransferToMgrs.FirstOrDefault(w =>w.Branch == Header.Branch);
                //if (_chkTransfer != null)
                //    return "Already transfer to Manager!";
                var _Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == Header.MgrCode);
                if (_Staff != null)
                {
                    Header.BankName = _Staff.BankName;
                }
                var _listTransferToMgr = DB.PRTransferToMgrs.ToList();
                int Increment;
                if (_listTransferToMgr.Count == 0) Increment = 0;
                else Increment = _listTransferToMgr.Max(w => w.Increment);
                Header.Increment = Increment + 1;
                foreach (var read in ListItem.ToList())
                {
                    read.ID = Header.Increment;
                    read.HOD = Header.MgrCode;
                    DB.PRTransferToMgrItems.Add(read);
                }
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                DB.PRTransferToMgrs.Add(Header);
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ID.ToString();
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
                log.DocurmentAction = Header.ID.ToString();
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
                log.DocurmentAction = Header.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string upd(string id)
        {
            try
            {
                DB = new HumicaDBContext();

                int ID = Convert.ToInt32(id);
                var ObjMatch = DB.PRTransferToMgrs.FirstOrDefault(w => w.ID == ID);

                if (ObjMatch == null)
                    return "DOC_INV";

                var checkdupList = DB.PRTransferToMgrItems.Where(w => w.ID == ObjMatch.Increment).ToList();

                foreach (var read in checkdupList.ToList())
                {
                    DB.PRTransferToMgrItems.Remove(read);
                }

                foreach (var read in ListItem.ToList())
                {
                    read.ID = ObjMatch.Increment;
                    read.HOD = Header.MgrCode;
                    DB.PRTransferToMgrItems.Add(read);
                }

                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;

                DB.PRTransferToMgrs.Attach(ObjMatch);

                DB.Entry(ObjMatch).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChangedOn).IsModified = true;

                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

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
                log.DocurmentAction = Header.ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string delete(string id)
        {
            try
            {
                DB = new HumicaDBContext();

                int ID = Convert.ToInt32(id);
                var ObjMatch = DB.PRTransferToMgrs.FirstOrDefault(w => w.ID == ID);
                var checkdupListItem = DB.PRTransferToMgrItems.Where(w => w.ID == ObjMatch.Increment).ToList();

                foreach (var read in checkdupListItem.ToList())
                {
                    DB.PRTransferToMgrItems.Remove(read);
                }

                DB.PRTransferToMgrs.Remove(ObjMatch);
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

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
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

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
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}
