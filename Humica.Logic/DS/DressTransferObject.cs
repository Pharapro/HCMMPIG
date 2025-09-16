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
    public class DressTransferObject
    {
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public HRDressTransfer Header { get; set; }
        public HRDressReceive HeaderReceive { get; set; }
        public HRDressRequest HeaderReq { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public HRStaffProfile HeaderST_P { get; set; }
        public FTINYear FInYear { get; set; }
        public List<HRDressTransfer> ListHeader { get; set; }
        public List<HRDressTransferItem> ListItem { get; set; }
        public List<ExCfWorkFlowItem> ListWF { get; set; }
        public List<ExCfWFSalaryApprover> ListWFSA { get; set; }
        public List<HRDressRequestItem> ListReqItem { get; set; }
        public List<HRDressReceiveItem> ListReceive { get; set; }
        public List<DeleteItem> ListDelete { get; set; }

        public List<HRUniform> ListUniform { get; set; }
        public DressTransferObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateDR()
        {
            try
            {

                //Add Header

                var objNumber = new CFNumberRank(Header.DocType);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.DocNo = objNumber.NextNumberRank;
                Header.Status = SYDocumentStatus.OPEN.ToString();
                Header.FEmpCode = Header.FEmpCode;
                Header.TEmpCode = Header.TEmpCode;
                Header.DocType = Header.DocType;
                Header.Branch = Header.Branch;
                Header.PathFile = Header.PathFile;
                Header.TransferDate = Header.TransferDate;
                Header.Status = Header.Status;
                Header.CreatedOn = DateTime.Now;
                Header.CreatedBy = User.UserName;

                if (ListItem.Count <= 0)
                {
                    return "ITEN_NULL";
                }
                //Add item
                int id = 0;
                foreach (var item in ListItem)
                {
                    var obj = new HRDressTransferItem();
                    id = Convert.ToInt32(item.ReceivedItemID);
                    obj.DocNo = Header.DocNo;
                    obj.ItemName = item.ItemName;
                    obj.Qty = item.Qty;
                    obj.Description = item.Description;
                    obj.ReceivedItemID = id;

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
                                read.StatusTransfer = "TRANSFER";
                                read.QTY -= obj.Qty;
                                read.TransferItem += obj.Qty;
                                DB.HRDressReceiveItems.Attach(read);
                                DB.Entry(read).Property(w => w.QTY).IsModified = true;
                                DB.Entry(read).Property(w => w.TransferItem).IsModified = true;
                                DB.Entry(read).Property(w => w.StatusTransfer).IsModified = true;
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
                                i.StatusTransfer = "TRANSFER";
                                i.QTY -= obj.Qty;
                                i.TransferItem += obj.Qty;
                                DB.HRDressReceiveItems.Attach(i);
                                DB.Entry(i).Property(w => w.QTY).IsModified = true;
                                DB.Entry(i).Property(w => w.TransferItem).IsModified = true;
                                DB.Entry(i).Property(w => w.StatusTransfer).IsModified = true;
                                DB.SaveChanges();
                            }
                        }
                    }
                    DB.HRDressTransferItems.Add(obj);
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
                        STRE.StatusTransfer = "TRANSFER";
                        STRE.TransferDate = Header.TransferDate;
                        STRE.FlexStatus = 1;
                        DB.HRDressReceives.Attach(STRE);
                        DB.Entry(STRE).Property(w => w.StatusTransfer).IsModified = true;
                        DB.Entry(STRE).Property(w => w.TransferDate).IsModified = true;
                        DB.Entry(STRE).Property(w => w.FlexStatus).IsModified = true;
                    }
                    else
                    {
                        STRE.StatusTransfer = "TRANSFER";
                        STRE.TransferDate = Header.TransferDate;
                        DB.HRDressReceives.Attach(STRE);
                        DB.Entry(STRE).Property(w => w.StatusTransfer).IsModified = true;
                        DB.Entry(STRE).Property(w => w.TransferDate).IsModified = true;
                    }
                }
                DB.HRDressTransfers.Add(Header);
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
        public string isValidItem(HRDressTransferItem item, EnumActionGridLine Action)
        {
            if (Action == EnumActionGridLine.Add)
            {
                int REid = Convert.ToInt32(item.ReceivedItemID);
                var ReciveItem = DB.HRDressReceiveItems.Where(w => w.ID == REid).FirstOrDefault();
                if (item.Qty > ReciveItem.QTY + ReciveItem.TransferItem || item.Qty < 0)
                {
                    return "INVALID_ITEM";
                }
            }

            return SYConstant.OK;
        }

        public string EditDR(string id)
        {
            try
            {
                var objMatch = DB.HRDressTransfers.FirstOrDefault(w => w.DocNo == id);
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
                objMatch.DocNo = Header.DocNo;//objNumber.NextNumberRank.Trim();
                objMatch.Branch = Header.Branch;
                objMatch.FEmpCode = Header.FEmpCode;
                objMatch.TEmpCode = Header.TEmpCode;
                objMatch.DocType = Header.DocType;
                objMatch.PathFile = Header.PathFile;
                objMatch.Description = Header.Description;
                objMatch.Status = SYDocumentStatus.OPEN.ToString();
                objMatch.TransferDate = Header.TransferDate;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                DB.HRDressTransfers.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.DocNo).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Branch).IsModified = true;
                DB.Entry(objMatch).Property(w => w.TEmpCode).IsModified = true;
                DB.Entry(objMatch).Property(w => w.FEmpCode).IsModified = true;
                DB.Entry(objMatch).Property(w => w.DocType).IsModified = true;
                DB.Entry(objMatch).Property(w => w.PathFile).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Description).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.Entry(objMatch).Property(w => w.TransferDate).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;

                string Item_id = string.Empty;
                //Edit Item
                var itemTransfer = DB.HRDressTransferItems.Where(w => w.DocNo == id).ToList();
                var itemrecive = DB.HRDressReceiveItems.ToList();
                //GridItem Remove
                var itemDelete = ListDelete.ToList();
                foreach (var _itemDelete in itemDelete)
                {
                    foreach (var _itemrecive in itemrecive)
                    {
                        if (_itemDelete.ReceiveID == _itemrecive.ID.ToString())
                        {
                            _itemrecive.QTY = _itemrecive.TransferItem.Value + _itemrecive.QTY;
                            _itemrecive.TransferItem = 0;
                            _itemrecive.StatusTransfer = SYDocumentStatus.OPEN.ToString();
                            DB.HRDressReceiveItems.Attach(_itemrecive);
                            DB.Entry(_itemrecive).Property(w => w.QTY).IsModified = true;
                            DB.Entry(_itemrecive).Property(w => w.TransferItem).IsModified = true;
                            DB.Entry(_itemrecive).Property(w => w.StatusTransfer).IsModified = true;
                            var headerRe = DB.HRDressReceives.Where(w => w.DocNO == _itemrecive.DocNO).FirstOrDefault();
                            headerRe.StatusTransfer = SYDocumentStatus.OPEN.ToString();
                            DB.HRDressReceives.Attach(headerRe);
                            DB.Entry(headerRe).Property(w => w.StatusTransfer).IsModified = true;
                            //String _id = _itemrecive.ID.ToString();
                            var objDel = DB.HRDressTransferItems.Where(w => w.ReceivedItemID == _itemrecive.ID).FirstOrDefault();
                            if (objDel != null)
                                DB.HRDressTransferItems.Remove(objDel);

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
                                if (read.Qty > (_itemrecive.TransferItem + _itemrecive.QTY) && _itemrecive.Description == read.ItemName)
                                {
                                    return "INVALID_ITEM";
                                }
                                if (_itemrecive.TransferItem >= read.Qty && _itemrecive.Description == read.ItemName)
                                {

                                    item.Qty = read.Qty;
                                    _itemrecive.QTY = _itemrecive.TransferItem.Value - item.Qty;
                                    _itemrecive.TransferItem = item.Qty;
                                    _itemrecive.StatusTransfer = "TRANSFER";

                                }
                                else if (_itemrecive.TransferItem < read.Qty && _itemrecive.Description == read.ItemName)
                                {
                                    item.Qty = read.Qty;
                                    _itemrecive.QTY -= (_itemrecive.TransferItem.Value + item.Qty) - read.Qty;
                                    _itemrecive.TransferItem = item.Qty;
                                    _itemrecive.Status = "TRANSFER";
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
                                DB.Entry(_itemrecive).Property(w => w.TransferItem).IsModified = true;
                                DB.Entry(_itemrecive).Property(w => w.StatusTransfer).IsModified = true;
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
                                if (_itemrecive.TransferItem == 0)
                                {
                                    _itemrecive.StatusTransfer = null;
                                }
                                STRE.StatusTransfer = "TRANSFER";
                                DB.HRDressReceives.Attach(STRE);
                                DB.Entry(STRE).Property(w => w.StatusTransfer).IsModified = true;
                                DB.Entry(STRE).Property(w => w.FlexStatus).IsModified = true;
                            }
                            item.Description = read.Description;
                            DB.HRDressTransferItems.Attach(item);
                            DB.Entry(item).Property(w => w.Qty).IsModified = true;
                            DB.Entry(item).Property(w => w.Description).IsModified = true;
                        }

                    }

                }

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.FEmpCode;
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
                Header = new HRDressTransfer();
                var objMatch = DB.HRDressTransfers.FirstOrDefault(w => w.DocNo == id);
                var item = DB.HRDressTransferItems.Where(w => w.DocNo == objMatch.DocNo).ToList();
                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                Header.DocNo = objMatch.DocNo;
                foreach (var read in item)
                {
                    DB.HRDressTransferItems.Attach(read);
                    DB.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                    int _id = Convert.ToInt32(read.ReceivedItemID);
                    var itemRe = DB.HRDressReceiveItems.Where(w => w.ID == _id).FirstOrDefault();
                    if (item != null)
                    {
                        itemRe.QTY += itemRe.TransferItem.Value;
                        itemRe.StatusTransfer = null;//SYDocumentStatus.OPEN.ToString();
                        itemRe.TransferItem = 0;
                        DB.HRDressReceiveItems.Attach(itemRe);
                        DB.Entry(itemRe).Property(w => w.QTY).IsModified = true;
                        DB.Entry(itemRe).Property(w => w.TransferItem).IsModified = true;
                        DB.Entry(itemRe).Property(w => w.StatusTransfer).IsModified = true;
                        DB.SaveChanges();
                        var headerRe = DB.HRDressReceives.Where(w => w.DocNO == itemRe.DocNO).FirstOrDefault();
                        if (headerRe != null)
                        {
                            headerRe.StatusTransfer = null;//SYDocumentStatus.OPEN.ToString();
                            DB.HRDressReceives.Attach(headerRe);
                            DB.Entry(headerRe).Property(w => w.StatusTransfer).IsModified = true;
                            DB.SaveChanges();
                        }
                    }
                }

                DB.HRDressTransfers.Attach(objMatch);
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