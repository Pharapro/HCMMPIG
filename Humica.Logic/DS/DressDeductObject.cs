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
    public class DressDeductObject
    {
        public SMSystemEntity DBI = new SMSystemEntity();
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public HRDressDeduct Header { get; set; }
        public HRDressReceive HeaderReceive { get; set; }
        public HRDressRequest HeaderReq { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public HRStaffProfile HeaderST_P { get; set; }
        public FTINYear FInYear { get; set; }
        public List<HRDressDeduct> ListHeader { get; set; }
        public List<HRDressDeductItem> ListItem { get; set; }
        public List<ExCfWorkFlowItem> ListWF { get; set; }
        public List<ExCfWFSalaryApprover> ListWFSA { get; set; }
        public List<HRDressRequestItem> ListReqItem { get; set; }
        public List<HRDressReceiveItem> ListReceive { get; set; }
        public List<DeleteItem> ListDelete { get; set; }
        public decimal AmountDed;

        public List<HRUniform> ListUniform { get; set; }
        public DressDeductObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        public string isValidItem(HRDressDeductItem item, EnumActionGridLine Action)
        {
            if (Action == EnumActionGridLine.Add)
            {
                int REid = Convert.ToInt32(item.ReceivedItemID);
                var ReciveItem = DB.HRDressReceiveItems.Where(w => w.ID == REid).FirstOrDefault();
                if (item.Qty > ReciveItem.QTY + ReciveItem.DeductItem || item.Qty < 0)
                {
                    return "INVALID_ITEM";
                }
            }

            return SYConstant.OK;
        }
        public string CreateDR()
        {
            try
            {

                //Add Header
                var objNumber = new CFNumberRank(Header.DocType);
                var StaffName = DB.HRStaffProfiles.Where(w => w.EmpCode == Header.EmpCode).FirstOrDefault();
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.DocNo = objNumber.NextNumberRank;
                Header.Status = SYDocumentStatus.OPEN.ToString();
                Header.EmpCode = Header.EmpCode;
                Header.DocType = Header.DocType;
                Header.Branch = Header.Branch;
                Header.AllName = StaffName.AllName;
                Header.DeductDate = Header.DeductDate;
                Header.Status = Header.Status;
                Header.PathFile = Header.PathFile;
                Header.CreatedDate = DateTime.Now;
                Header.CreatedBy = User.UserName;

                if (ListItem.Count <= 0)
                {
                    return "ITEN_NULL";
                }
                //Add item
                int id = 0;
                foreach (var item in ListItem)
                {
                    var obj = new HRDressDeductItem();
                    id = Convert.ToInt32(item.ReceivedItemID);
                    obj.DocNo = Header.DocNo;
                    obj.ItemName = item.ItemName;
                    obj.Qty = item.Qty;
                    obj.Description = item.Description;
                    obj.ReceivedItemID = id;
                    obj.Amount = item.Amount;
                    var REitem = DB.HRDressReceiveItems.Where(w => w.ID == id).ToList();
                    if (REitem.Count == 1)
                    {
                        foreach (var read in REitem)
                        {
                            if (item.Qty > (read.QTY + read.ReturnItem) && item.ItemName == read.Description) return "INVALID_ITEM";
                            if (obj.Qty <= read.QTY)
                            {
                                //if (obj.Qty == read.Qty)
                                //{
                                //    read.StatusTransfer ="TRANSFER";
                                //}
                                read.StatusDeduct = "DEDUCT";
                                read.QTY -= obj.Qty;
                                read.DeductItem += obj.Qty;
                                DB.HRDressReceiveItems.Attach(read);
                                DB.Entry(read).Property(w => w.QTY).IsModified = true;
                                DB.Entry(read).Property(w => w.DeductItem).IsModified = true;
                                DB.Entry(read).Property(w => w.StatusDeduct).IsModified = true;
                                DB.SaveChanges();
                            }

                        }
                    }
                    else
                    {
                        foreach (var i in REitem)
                        {
                            if (obj.Qty <= i.QTY && obj.ItemName == i.Description)
                            {
                                //if (obj.Qty == i.Qty)
                                //{
                                //    i.StatusTransfer = "TRANSFER";
                                //}
                                i.StatusDeduct = "DEDUCT";
                                i.QTY -= obj.Qty;
                                i.DeductItem += obj.Qty;
                                DB.HRDressReceiveItems.Attach(i);
                                DB.Entry(i).Property(w => w.QTY).IsModified = true;
                                DB.Entry(i).Property(w => w.DeductItem).IsModified = true;
                                DB.Entry(i).Property(w => w.StatusDeduct).IsModified = true;
                                DB.SaveChanges();
                            }
                        }
                    }
                    Header.DedAm += item.Amount;

                    DB.HRDressDeductItems.Add(obj);
                    DB.SaveChanges();
                }
                // update table HRDressReceive
                List<string> dressReceiveId = new List<string>();
                ListItem.ForEach(x => dressReceiveId.Add(x.DocNo));
                dressReceiveId.Distinct();
                foreach (var drid in dressReceiveId)
                {
                    var STRE = DB.HRDressReceives.Where(w => w.DocNO == drid).FirstOrDefault();
                    int total = DB.HRDressReceiveItems.Where(x => x.DocNO == drid).Select(x => x.QTY).Sum();
                    //HeaderReceive = new HRDressReceive();
                    if (total == 0)
                    {
                        STRE.StatusDeduct = "DEDUCT";
                        STRE.DeductDate = Header.DeductDate;
                        STRE.FlexStatus = 1;
                        DB.HRDressReceives.Attach(STRE);
                        DB.Entry(STRE).Property(w => w.StatusDeduct).IsModified = true;
                        DB.Entry(STRE).Property(w => w.DeductDate).IsModified = true;
                        DB.Entry(STRE).Property(w => w.FlexStatus).IsModified = true;
                    }
                    else
                    {
                        STRE.StatusDeduct = "DEDUCT";
                        STRE.DeductDate = Header.DeductDate;
                        DB.HRDressReceives.Attach(STRE);
                        DB.Entry(STRE).Property(w => w.StatusDeduct).IsModified = true;
                        DB.Entry(STRE).Property(w => w.DeductDate).IsModified = true;
                    }
                }
                DB.HRDressDeducts.Add(Header);
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.DocNo;
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
                log.DocurmentAction = Header.DocNo;
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
                log.DocurmentAction = Header.DocNo;
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
                decimal amountDed = 0;
                var objMatch = DB.HRDressDeducts.FirstOrDefault(w => w.DocNo == id);
                if (objMatch == null)
                {
                    return "DOC_NULL";
                }
                var objNumber = new CFNumberRank(Header.DocType, ScreenId);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                //Edit header
                //objMatch.DocNo = Header.DocNo;//objNumber.NextNumberRank.Trim();
                objMatch.Branch = Header.Branch;
                objMatch.EmpCode = Header.EmpCode;
                objMatch.DocType = Header.DocType;
                objMatch.Description = Header.Description;
                objMatch.Status = SYDocumentStatus.OPEN.ToString();
                objMatch.DeductDate = Header.DeductDate;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;


                string Item_id = string.Empty;
                //Edit Item
                var itemTransfer = DB.HRDressDeductItems.Where(w => w.DocNo == id).ToList();
                var itemrecive = DB.HRDressReceiveItems.ToList();
                //GridItem Remove
                var itemDelete = ListDelete.ToList();
                foreach (var _itemDelete in itemDelete)
                {
                    foreach (var _itemrecive in itemrecive)
                    {
                        if (_itemDelete.ReceiveID == _itemrecive.ID.ToString())
                        {
                            _itemrecive.QTY = _itemrecive.DeductItem.Value + _itemrecive.QTY;
                            _itemrecive.DeductItem = 0;
                            _itemrecive.StatusDeduct = SYDocumentStatus.OPEN.ToString();
                            DB.HRDressReceiveItems.Attach(_itemrecive);
                            DB.Entry(_itemrecive).Property(w => w.QTY).IsModified = true;
                            DB.Entry(_itemrecive).Property(w => w.DeductItem).IsModified = true;
                            DB.Entry(_itemrecive).Property(w => w.StatusDeduct).IsModified = true;
                            var headerRe = DB.HRDressReceives.Where(w => w.DocNO == _itemrecive.DocNO).FirstOrDefault();
                            headerRe.StatusDeduct = SYDocumentStatus.OPEN.ToString();
                            DB.HRDressReceives.Attach(headerRe);
                            DB.Entry(headerRe).Property(w => w.StatusDeduct).IsModified = true;
                            //String _id = _itemrecive.ID.ToString();
                            var objDel = DB.HRDressDeductItems.Where(w => w.ReceivedItemID == _itemrecive.ID).FirstOrDefault();
                            if (objDel != null)
                                DB.HRDressDeductItems.Remove(objDel);

                        }


                    }
                }

                //GrideItem  No remove
                foreach (var item in itemTransfer)
                {
                    foreach (var read in ListItem)
                    {

                        if (item.ItemName == read.ItemName)
                        {
                            foreach (var _itemrecive in itemrecive)
                            {
                                if (read.Qty > (_itemrecive.DeductItem + _itemrecive.QTY) && _itemrecive.Description == read.ItemName)
                                {
                                    return "INVALID_ITEM";
                                }
                                if (_itemrecive.DeductItem >= read.Qty && _itemrecive.Description == read.ItemName)
                                {

                                    item.Qty = read.Qty;
                                    item.Amount = read.Amount;
                                    amountDed = item.Amount;
                                    _itemrecive.QTY = _itemrecive.DeductItem.Value - item.Qty;
                                    _itemrecive.DeductItem = item.Qty;
                                    _itemrecive.StatusDeduct = "DEDUCT";

                                }
                                //else if ((_itemrecive.TransferItem + _itemrecive.Qty) == read.Qty && _itemrecive.Description == read.ItemName)
                                //{
                                //    item.Qty = read.Qty;
                                //    _itemrecive.Qty = 0;
                                //    _itemrecive.TransferItem = item.Qty;
                                //    _itemrecive.StatusTransfer = SYDocumentStatus.RETURN.ToString();

                                //}

                                DB.HRDressReceiveItems.Attach(_itemrecive);
                                DB.Entry(_itemrecive).Property(w => w.QTY).IsModified = true;
                                DB.Entry(_itemrecive).Property(w => w.DeductItem).IsModified = true;
                                DB.Entry(_itemrecive).Property(w => w.StatusDeduct).IsModified = true;
                                DB.SaveChanges();
                                //update status HrDressReceive
                                var STRE = DB.HRDressReceives.Where(w => w.DocNO == _itemrecive.DocNO).FirstOrDefault();
                                int total = DB.HRDressReceiveItems.Where(x => x.DocNO == _itemrecive.DocNO).Select(x => x.QTY).Sum();
                                //HeaderReceive = new HRDressReceive();
                                if (total == 0)
                                {
                                    STRE.FlexStatus = 1;
                                }
                                else
                                {
                                    STRE.FlexStatus = 0;
                                }
                                if (_itemrecive.DeductItem == 0)
                                {
                                    _itemrecive.StatusDeduct = null;
                                }
                                STRE.StatusDeduct = "DEDUCT";
                                DB.HRDressReceives.Attach(STRE);
                                DB.Entry(STRE).Property(w => w.StatusDeduct).IsModified = true;
                                DB.Entry(STRE).Property(w => w.FlexStatus).IsModified = true;
                            }
                            item.Description = read.Description;

                            DB.HRDressDeductItems.Attach(item);
                            DB.Entry(item).Property(w => w.Qty).IsModified = true;
                            DB.Entry(item).Property(w => w.Description).IsModified = true;
                        }

                    }
                }
                //var STRE = DB.HRDressDeduct.Where(w => w.DocNo ==).FirstOrDefault();
                decimal totalAmount = DB.HRDressDeductItems.Where(x => x.DocNo == Header.DocNo).Select(x => x.Amount).Sum();
                objMatch.DedAm = totalAmount;
                DB.HRDressDeducts.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Branch).IsModified = true;
                DB.Entry(objMatch).Property(w => w.EmpCode).IsModified = true;
                DB.Entry(objMatch).Property(w => w.DocType).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Description).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.Entry(objMatch).Property(w => w.DeductDate).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(w => w.DedAm).IsModified = true;
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
        public string DeleteDR(string id)
        {
            try
            {
                Header = new HRDressDeduct();
                var objMatch = DB.HRDressDeducts.FirstOrDefault(w => w.DocNo == id);
                var item = DB.HRDressDeductItems.Where(w => w.DocNo == objMatch.DocNo).ToList();
                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                Header.DocNo = objMatch.DocNo;
                foreach (var read in item)
                {
                    DB.HRDressDeductItems.Attach(read);
                    DB.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                    int _id = Convert.ToInt32(read.ReceivedItemID);
                    var itemRe = DB.HRDressReceiveItems.Where(w => w.ID == _id).FirstOrDefault();
                    if (item != null)
                    {
                        itemRe.QTY += itemRe.DeductItem.Value;
                        itemRe.StatusDeduct = null;//SYDocumentStatus.OPEN.ToString();
                        itemRe.DeductItem = 0;

                        DB.HRDressReceiveItems.Attach(itemRe);
                        DB.Entry(itemRe).Property(w => w.QTY).IsModified = true;
                        DB.Entry(itemRe).Property(w => w.DeductItem).IsModified = true;
                        DB.Entry(itemRe).Property(w => w.StatusDeduct).IsModified = true;
                        DB.SaveChanges();
                        var headerRe = DB.HRDressReceives.Where(w => w.DocNO == itemRe.DocNO).FirstOrDefault();
                        if (headerRe != null)
                        {
                            headerRe.StatusDeduct = null;//SYDocumentStatus.OPEN.ToString();
                            headerRe.FlexStatus = 0;
                            DB.HRDressReceives.Attach(headerRe);
                            DB.Entry(headerRe).Property(w => w.StatusDeduct).IsModified = true;
                            DB.Entry(headerRe).Property(w => w.FlexStatus).IsModified = true;
                            DB.SaveChanges();
                        }
                    }
                }

                DB.HRDressDeducts.Attach(objMatch);
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
                log.ScreenId = Header.DocNo;
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
                log.ScreenId = Header.DocNo;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }

        }
    }
}
