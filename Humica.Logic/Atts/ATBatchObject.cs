using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.Atts
{
    public class ATBatchObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public ATBatch Header { get; set; }
        public List<ATBatch> ListHeader { get; set; }
        public List<ATBatchItem> ListItem { get; set; }
        public List<HRStaffProfile> ListStaffs { get; set; }
        public FTFilterAttendance Filter { get; set; }
        public ATBatchObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateBatch()
        {
            try
            {
                if (Header.Code == null) return "CODE_EN";

                var chkdup = DB.ATBatches.Find(Header.Code);
                if (chkdup != null) return "TR_CODE_EXIST";
                if (ListItem.Count == 0) return "LIST_INV";

                Header.Code = Header.Code.ToUpper().Trim();
                foreach (var read in ListItem)
                {
                    var objitem = new ATBatchItem();
                    objitem = read;
                    objitem.BatchNo = Header.Code;
                    DB.ATBatchItems.Add(objitem);
                }
                DB.ATBatches.Add(Header);

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.Code;
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
                log.DocurmentAction = Header.Code;
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
                log.DocurmentAction = Header.Code;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditBatch(string id)
        {
            DB = new HumicaDBContext();
            try
            {
                var objMatch = DB.ATBatches.Find(id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                var objMatchD = DB.ATBatchItems.Where(w => w.BatchNo == id).ToList();
                foreach (var item in objMatchD)
                {
                    DB.ATBatchItems.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }

                objMatch.Description = Header.Description;
                objMatch.SecDescription = Header.SecDescription;
                objMatch.Remark = Header.Remark;
                foreach (var read in ListItem)
                {
                    read.BatchNo = Header.Code;
                    DB.ATBatchItems.Add(read);
                }
                DB.ATBatches.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Description).IsModified = true;
                DB.Entry(objMatch).Property(w => w.SecDescription).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Remark).IsModified = true;

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
        public string DeleteBatch(string id)
        {
            try
            {
                ATBatch objCust = DB.ATBatches.Find(id);
                if (objCust == null)
                {
                    return "DOC_INV";
                }

                HumicaDBContext DBM = new HumicaDBContext();
                var objMatchD = DB.ATBatchItems.Where(w => w.BatchNo == id).ToList();
                foreach (var item in objMatchD)
                {
                    DBM.ATBatchItems.Attach(item);
                    DBM.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }

                DBM.ATBatches.Attach(objCust);
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
        public string AssStaffRoster(string EmpCode, string Batch)
        {
            string Result = "";
            try
            {
                var RewardType = DB.ATBatches.Where(w => w.Code == Batch).ToList();
                //if (RewardType.Count() == 0)
                //{
                //    return "INVALTID_BATCH";
                //}
                string[] multiArrayBill = EmpCode.Split(';');
                var staff = DB.HRStaffProfiles.ToList();
                foreach (var item in multiArrayBill)
                {
                    if (item.Trim() != "")
                    {
                        string EmpCodeS = item.ToString();
                        var StaffUpdate = staff.Where(w => w.EmpCode == EmpCodeS).First();
                        if (StaffUpdate != null)
                        {
                            StaffUpdate.ROSTER = Batch;
                            DB.Entry(StaffUpdate).State = EntityState.Modified;
                        }
                    }
                }
                var row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Result;
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
                log.DocurmentAction = Result;
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
                log.DocurmentAction = Result;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}