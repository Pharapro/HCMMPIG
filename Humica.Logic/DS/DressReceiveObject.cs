using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.DS
{
    public class DressReceiveObject
    {

        HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public HRDressReceive Header { get; set; }
        public HRDressReceiveItem Itm { get; set; }

        public HRDressRequest HeaderReq { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public FTINYear FInYear { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<HRDressReceive> ListHeader { get; set; }
        public List<HRDressReceiveItem> ListItem { get; set; }
        public List<HRDressRequest> ListReqH { get; set; }
        public List<HRDressRequestItem> ListReqItem { get; set; }
        public List<HRUniform> ListUniform { get; set; }
        public List<HR_STAFF_VIEW> ListStaff { get; set; }
        public string items { get; set; }
        public DressReceiveObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string isValidItem(HRDressReceiveItem item, EnumActionGridLine Action)
        {
            if (Action == EnumActionGridLine.Add)
            {
                if (ListItem.Where(w => w.ItemCode == item.ItemCode).ToList().Count > 0)
                {
                    return "DUPLICATED_ITEM";
                }
            }

            return SYConstant.OK;
        }
        public string CreateDR()
        {
            try
            {
                var emp = Header.EmpCode;
                if (ListItem.Count <= 0)
                {
                    return "Item Can not null";
                }
                //Add item
                var objNumber = new CFNumberRank(Header.DocType);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }

                //Add item
                Header.DocNO = objNumber.NextNumberRank;
                //Header.Status= SYDocumentStatus.OPEN.ToString();
                foreach (var item in ListItem)
                {
                    var obj = new HRDressReceiveItem();
                    obj.DocNO = Header.DocNO;
                    obj.ItemCode = item.ItemCode;
                    obj.QTY = item.QTY;
                    obj.EmpCode = emp;
                    obj.Description = item.Description;
                    obj.Status = SYDocumentStatus.OPEN.ToString();
                    DB.HRDressReceiveItems.Add(obj);
                }
                Header.EmpCode = Header.EmpCode;
                Header.EmpName = Header.EmpName;
                Header.Post = Header.Post;
                Header.Branch = Header.Branch;
                Header.CreatedOn = DateTime.Now;
                Header.CreateBy = User.UserName;

                DB.HRDressReceives.Add(Header);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
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
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditDR(string id)
        {
            try
            {
                var objMatch = DB.HRDressReceives.FirstOrDefault(w => w.DocNO == id);
                if (objMatch == null)
                {
                    return "Document cann't be null";
                }
                objMatch.DocNO = Header.DocNO;
                objMatch.Branch = Header.Branch;
                objMatch.DocType = Header.DocType;
                objMatch.ReceiveDATE = Header.ReceiveDATE;
                objMatch.PathFile = Header.PathFile;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                objMatch.Remark = Header.Remark;
                objMatch.Status = Header.Status;
                //int num = 0;
                var ListAtt = DB.HRDressReceives.Where(w => w.DocNO == id).ToList();
                foreach (var read in ListItem.ToList())
                {
                    read.DocNO = objMatch.DocNO;
                    objMatch.ReceiveQTY = read.QTY;
                    read.QTY = read.QTY;
                    //num += read.Qty;
                    DB.HRDressReceiveItems.Attach(read);
                    DB.Entry(read).Property(w => w.DocNO).IsModified = true;
                    DB.Entry(read).Property(w => w.QTY).IsModified = true;
                }
                objMatch.ReceiveQTY = Header.ReceiveQTY;
                DB.HRDressReceives.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.DocNO).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Branch).IsModified = true;
                DB.Entry(objMatch).Property(w => w.DocType).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ReceiveDATE).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ReceiveQTY).IsModified = true;
                DB.Entry(objMatch).Property(w => w.PathFile).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Remark).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                int row = DB.SaveChanges();
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
        }
        public string DeleteDR(string DocNo)
        {
            try
            {
                Header = new HRDressReceive();
                var objMatch = DB.HRDressReceives.FirstOrDefault(w => w.DocNO == DocNo);
                var item = DB.HRDressReceiveItems.Where(w => w.DocNO == DocNo).ToList();
                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                if (objMatch.Status != "OPEN" || objMatch.TransferDate != null || objMatch.DeductDate != null)
                {
                    return "DOC_USE";
                }
                Header.DocNO = DocNo;
                foreach (var read in item)
                {
                    DB.HRDressReceiveItems.Attach(read);
                    DB.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                }

                DB.HRDressReceives.Attach(objMatch);
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
                log.ScreenId = Header.DocNO.ToString();
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
                log.ScreenId = Header.DocNO.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }

        }

    }

}