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
    public class DressReturnObject
    {
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public HRDressReturn Header { get; set; }
        public HRDressReceive HeaderReceive { get; set; }
        public HRDressRequest HeaderReq { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public FTINYear FInYear { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<HRDressReturn> ListHeader { get; set; }
        public List<HRDressReturnItem> ListItem { get; set; }
        public List<HRDressRequest> ListReqH { get; set; }
        public List<ExCfWorkFlowItem> ListWF { get; set; }
        public List<ExCfWFSalaryApprover> ListWFSA { get; set; }
        public List<HRDressRequestItem> ListReqItem { get; set; }
        public List<HRDressReceiveItem> ListReceive { get; set; }
        public List<DeleteItem> ListDelete { get; set; }

        public List<HRUniform> ListUniform { get; set; }
        public DressReturnObject()
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
                Header.EmpCode = Header.EmpCode;
                var Name = DB.HRStaffProfiles.Where(w => w.EmpCode == Header.EmpCode).FirstOrDefault();
                Header.EmpName = Name.AllName;
                Header.DocType = Header.DocType;
                Header.Branch = Header.Branch;
                Header.ReturnDate = DateTime.Now;
                Header.PathFile = Header.PathFile;
                Header.Status = Header.Status;
                Header.QTY = Header.QTY;
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
                    var obj = new HRDressReturnItem();
                    id = Convert.ToInt32(item.ReceivedItemID);
                    obj.DocNo = Header.DocNo;
                    obj.ItemName = item.ItemName;
                    obj.Qty = item.Qty;
                    obj.Description = item.Description;
                    obj.ReceivedItemID = item.ReceivedItemID;

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
                                //    read.Status = "RETURN";
                                //}
                                read.Status = "RETURN";
                                read.QTY -= obj.Qty;
                                read.ReturnItem += obj.Qty;
                                DB.HRDressReceiveItems.Attach(read);
                                DB.Entry(read).Property(w => w.QTY).IsModified = true;
                                DB.Entry(read).Property(w => w.ReturnItem).IsModified = true;
                                DB.Entry(read).Property(w => w.Status).IsModified = true;
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
                                //    i.Status = "RETURN";
                                //}
                                i.Status = "RETURN";
                                i.QTY -= obj.Qty;
                                i.ReturnItem += obj.Qty;
                                DB.HRDressReceiveItems.Attach(i);
                                DB.Entry(i).Property(w => w.QTY).IsModified = true;
                                DB.Entry(i).Property(w => w.ReturnItem).IsModified = true;
                                DB.Entry(i).Property(w => w.Status).IsModified = true;
                                DB.SaveChanges();
                            }
                        }
                    }
                    DB.HRDressReturnItems.Add(obj);
                    DB.SaveChanges();
                }
                /// update table HRDressReceive
                List<string> dressReceiveId = new List<string>();
                ListItem.ForEach(x => dressReceiveId.Add(x.DocNo));
                dressReceiveId.Distinct();
                foreach (var drid in dressReceiveId)
                {
                    var STRE = DB.HRDressReceives.Where(w => w.DocNO == drid).FirstOrDefault();
                    int total = DB.HRDressReceiveItems.Where(x => x.DocNO == drid && x.Status == "RETURN").Select(x => x.QTY).Sum();
                    int total_ = DB.HRDressReceiveItems.Where(x => x.DocNO == drid).Select(x => x.ReturnItem.Value).Sum();
                    int UP = DB.HRDressReceiveItems.Where(x => x.DocNO == drid).Select(x => x.QTY).Sum();


                    //HeaderReceive = new HRDressReceive();
                    if (total == 0)
                    {
                        STRE.Status = "REUTURN";
                        if (UP == 0) STRE.FlexStatus = 1;
                        DB.HRDressReceives.Attach(STRE);
                        DB.Entry(STRE).Property(w => w.Status).IsModified = true;
                        DB.Entry(STRE).Property(w => w.FlexStatus).IsModified = true;
                        DB.SaveChanges();
                    }
                    if (total_ == STRE.ReceiveQTY)
                    {
                        STRE.FlexStatus = 1;
                        DB.HRDressReceives.Attach(STRE);
                        DB.Entry(STRE).Property(w => w.FlexStatus).IsModified = true;
                    }
                }
                DB.HRDressReturns.Add(Header);
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
        public string isValidItem(HRDressReturnItem item, EnumActionGridLine Action)
        {
            if (Action == EnumActionGridLine.Add)
            {
                string id = item.ID.ToString();
                int REid = Convert.ToInt32(item.ReceivedItemID);
                var ReciveItem = DB.HRDressReceiveItems.Where(w => w.ID == REid).FirstOrDefault();
                if (item.Qty > ReciveItem.QTY + ReciveItem.ReturnItem || item.Qty < 0)
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
                var objMatch = DB.HRDressReturns.FirstOrDefault(w => w.DocNo == id);
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
                objMatch.EmpCode = Header.EmpCode;
                objMatch.EmpName = HeaderStaff.AllName;
                objMatch.DocType = Header.DocType;
                objMatch.Description = Header.Description;
                objMatch.Status = SYDocumentStatus.OPEN.ToString();
                objMatch.ReturnDate = Header.ReturnDate;
                objMatch.PathFile = Header.PathFile;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                DB.HRDressReturns.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.DocNo).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Branch).IsModified = true;
                DB.Entry(objMatch).Property(w => w.EmpCode).IsModified = true;
                DB.Entry(objMatch).Property(w => w.DocType).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Description).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ReturnDate).IsModified = true;
                DB.Entry(objMatch).Property(w => w.PathFile).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;

                string Item_id = string.Empty;
                //Edit Item
                var itemReturn = DB.HRDressReturnItems.Where(w => w.DocNo == id).ToList();
                var itemrecive = DB.HRDressReceiveItems.ToList();
                //GridItem Remove
                var itemDelete = ListDelete.ToList();
                foreach (var _itemDelete in itemDelete)
                {
                    foreach (var _itemrecive in itemrecive)
                    {
                        if (_itemDelete.ReceiveID == _itemrecive.ID.ToString())
                        {
                            _itemrecive.QTY = _itemrecive.ReturnItem.Value + _itemrecive.QTY;
                            _itemrecive.ReturnItem = 0;
                            _itemrecive.Status = SYDocumentStatus.OPEN.ToString();
                            DB.HRDressReceiveItems.Attach(_itemrecive);
                            DB.Entry(_itemrecive).Property(w => w.QTY).IsModified = true;
                            DB.Entry(_itemrecive).Property(w => w.ReturnItem).IsModified = true;
                            DB.Entry(_itemrecive).Property(w => w.Status).IsModified = true;
                            var headerRe = DB.HRDressReceives.Where(w => w.DocNO == _itemrecive.DocNO).FirstOrDefault();
                            headerRe.Status = SYDocumentStatus.OPEN.ToString();
                            DB.HRDressReceives.Attach(headerRe);
                            DB.Entry(headerRe).Property(w => w.Status).IsModified = true;
                            String _id = _itemrecive.ID.ToString();
                            var objDel = DB.HRDressReturnItems.Where(w => w.ReceivedItemID == _id).FirstOrDefault();
                            if (objDel != null)
                                DB.HRDressReturnItems.Remove(objDel);
                        }


                    }
                }

                //GrideItem  No remove
                foreach (var item in itemReturn)
                {
                    foreach (var read in ListItem)
                    {

                        if (item.ItemName == read.ItemName)
                        {
                            foreach (var _itemrecive in itemrecive)
                            {
                                if (read.Qty > (_itemrecive.ReturnItem + _itemrecive.QTY) && _itemrecive.Description == read.ItemName)
                                {
                                    return "INVALID_ITEM";
                                }
                                if (_itemrecive.ReturnItem > read.Qty && _itemrecive.Description == read.ItemName)
                                {

                                    item.Qty = read.Qty;
                                    _itemrecive.QTY = _itemrecive.ReturnItem.Value - item.Qty;
                                    _itemrecive.ReturnItem = item.Qty;
                                    _itemrecive.Status = SYDocumentStatus.OPEN.ToString();

                                }
                                else if ((_itemrecive.ReturnItem + _itemrecive.QTY) == read.Qty && _itemrecive.Description == read.ItemName)
                                {
                                    item.Qty = read.Qty;
                                    _itemrecive.QTY = 0;
                                    _itemrecive.ReturnItem = item.Qty;
                                    _itemrecive.Status = SYDocumentStatus.RETURN.ToString();

                                }
                                else if (_itemrecive.ReturnItem < read.Qty && _itemrecive.Description == read.ItemName)
                                {
                                    item.Qty = read.Qty;
                                    _itemrecive.QTY -= (_itemrecive.ReturnItem.Value + item.Qty) - read.Qty;
                                    _itemrecive.ReturnItem = item.Qty;
                                    _itemrecive.Status = SYDocumentStatus.RETURN.ToString();
                                }

                                DB.HRDressReceiveItems.Attach(_itemrecive);
                                DB.Entry(_itemrecive).Property(w => w.QTY).IsModified = true;
                                DB.Entry(_itemrecive).Property(w => w.ReturnItem).IsModified = true;
                                DB.Entry(_itemrecive).Property(w => w.Status).IsModified = true;
                                DB.SaveChanges();
                                //update status HrDressReceive
                                var STRE = DB.HRDressReceives.Where(w => w.DocNO == _itemrecive.DocNO).FirstOrDefault();
                                int total = DB.HRDressReceiveItems.Where(x => x.DocNO == _itemrecive.DocNO).Select(x => x.QTY).Sum();
                                //HeaderReceive = new HRDressReceive();
                                if (total == 0)
                                {
                                    STRE.Status = SYDocumentStatus.RETURN.ToString();
                                }
                                else
                                {
                                    STRE.Status = SYDocumentStatus.OPEN.ToString();
                                    STRE.FlexStatus = 0;
                                }

                                DB.HRDressReceives.Attach(STRE);
                                DB.Entry(STRE).Property(w => w.Status).IsModified = true;
                                DB.Entry(STRE).Property(w => w.FlexStatus).IsModified = true;
                            }
                            item.Description = read.Description;
                            DB.HRDressReturnItems.Attach(item);
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
                Header = new HRDressReturn();
                int id_ = Convert.ToInt32(id);
                var objMatch = DB.HRDressReturns.FirstOrDefault(w => w.TranNo == id_);
                var item = DB.HRDressReturnItems.Where(w => w.DocNo == objMatch.DocNo).ToList();
                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                Header.DocNo = objMatch.DocNo;
                foreach (var read in item)
                {
                    DB.HRDressReturnItems.Attach(read);
                    DB.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                    int _id = Convert.ToInt32(read.ReceivedItemID);
                    var itemRe = DB.HRDressReceiveItems.Where(w => w.ID == _id).FirstOrDefault();
                    if (item != null)
                    {
                        itemRe.QTY += itemRe.ReturnItem.Value;
                        itemRe.Status = SYDocumentStatus.OPEN.ToString();
                        itemRe.ReturnItem = 0;
                        DB.HRDressReceiveItems.Attach(itemRe);
                        DB.Entry(itemRe).Property(w => w.QTY).IsModified = true;
                        DB.Entry(itemRe).Property(w => w.ReturnItem).IsModified = true;
                        DB.Entry(itemRe).Property(w => w.Status).IsModified = true;
                        DB.SaveChanges();
                        var headerRe = DB.HRDressReceives.Where(w => w.DocNO == itemRe.DocNO).FirstOrDefault();
                        if (headerRe != null)
                        {
                            headerRe.Status = SYDocumentStatus.OPEN.ToString();
                            DB.HRDressReceives.Attach(headerRe);
                            DB.Entry(headerRe).Property(w => w.Status).IsModified = true;
                            DB.SaveChanges();
                        }
                    }
                }

                DB.HRDressReturns.Attach(objMatch);
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
        public void SetAutoApproval(string DocType, string Branch, string SCREEN_ID)
        {
            ListApproval = new List<ExDocApproval>();
            var objDoc = DB.ExCfWorkFlowItems.Find(SCREEN_ID, DocType);
            if (objDoc != null)
            {
                if (objDoc.IsRequiredApproval == true)
                {
                    var listDefaultApproval = DB.ExCfWFApprovers.Where(w => w.Branch == Branch && w.WFObject == objDoc.ApprovalFlow && w.IsSelected == true).ToList();
                    foreach (var read in listDefaultApproval)
                    {
                        var objApp = new ExDocApproval();
                        objApp.Approver = read.Employee;
                        objApp.ApproverName = read.EmployeeName;
                        objApp.DocumentType = DocType;
                        objApp.ApproveLevel = read.ApproveLevel;
                        objApp.WFObject = objDoc.ApprovalFlow;
                        ListApproval.Add(objApp);
                    }
                }
            }
        }
        public void SetAutoItem()
        {
            ListItem = new List<HRDressReturnItem>();
            var objDoc = DB.HRUniforms.Where(w => w.IsAutoSelected == true);
            foreach (var read in objDoc)
            {
                var objApp = new HRDressReturnItem();
                objApp.ItemName = read.Description;
                objApp.Qty = 1;
                ListItem.Add(objApp);
            }
        }
        public string CancelDR(string ASNumber)
        {
            try
            {

                string cancelled = SYDocumentStatus.CANCELLED.ToString();
                var objmatch = DB.HRDressReturns.FirstOrDefault(w => w.DocNo == ASNumber);
                var _obj = DB.ExDocApprovals.Where(x => x.DocumentNo == ASNumber);
                if (objmatch == null)
                {
                    return "INV_EN";
                }

                foreach (var read in _obj)
                {
                    read.Status = cancelled;
                    DB.Entry(read).Property(w => w.Status).IsModified = true;
                }
                objmatch.Status = cancelled;
                DB.HRDressReturns.Attach(objmatch);
                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ASNumber;
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
                log.DocurmentAction = ASNumber;
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
                log.DocurmentAction = ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string isValidApproval(ExDocApproval Approver, EnumActionGridLine Action)
        {
            if (Action == EnumActionGridLine.Add)
            {
                if (ListApproval.Where(w => w.Approver == Approver.Approver).ToList().Count > 0)
                {
                    return "DUPLICATED_ITEM";
                }
            }

            return SYConstant.OK;
        }


        public string ReturnToApprove(string id)
        {
            try
            {
                string[] DocNo = id.Split(';');
                foreach (var List in DocNo)
                {
                    var objMatch = DB.HRDressReturns.FirstOrDefault(w => w.DocNo == List);
                    if (objMatch == null)
                    {
                        return "REQUEST_NE";
                    }
                    Header = objMatch;

                    if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                    {
                        return "INV_DOC";
                    }

                    objMatch.Status = SYDocumentStatus.PENDING.ToString();
                    DB.HRDressReturns.Attach(objMatch);
                    DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                    DB.SaveChanges();
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
                log.ScreenId = Header.DocNo;
                log.Action = SYActionBehavior.ADD.ToString();

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
        public string approveTheDoc(string id)
        {
            try
            {
                var objMatch = DB.HRDressReturns.FirstOrDefault(w => w.DocNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;

                if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                {
                    return "INV_DOC";
                }
                string open = SYDocumentStatus.OPEN.ToString();
                var listApproval = DB.ExDocApprovals.Where(w => w.DocumentType == objMatch.DocType
                                    && w.DocumentNo == id && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                var b = false;
                foreach (var read in listApproval)
                {
                    var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
                    if (checklist.Count > 0)
                    {
                        if (read.Status == SYDocumentStatus.APPROVED.ToString())
                        {
                            return "USER_ALREADY_APP";
                        }
                        if (read.ApproveLevel > listApproval.Min(w => w.ApproveLevel))
                        {
                            return "REQUIRED_PRE_LEVEL";
                        }
                        var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                        if (objStaff != null)
                        {
                            read.ApprovedBy = objStaff.EmpCode;
                            read.ApprovedName = objStaff.AllName;
                            read.LastChangedDate = DateTime.Now.Date;
                            read.ApprovedDate = DateTime.Now;
                            read.Status = SYDocumentStatus.APPROVED.ToString();
                            DB.ExDocApprovals.Attach(read);
                            DB.Entry(read).State = System.Data.Entity.EntityState.Modified;
                            b = true;
                            break;
                        }
                    }

                }
                if (listApproval.Count > 0)
                {
                    if (b == false)
                    {
                        return "USER_NOT_APPROVOR";
                    }
                }
                var status = SYDocumentStatus.APPROVED.ToString();
                //var open = SYDocumentStatus.OPEN.ToString();
                // objMatch.IsApproved = true;
                if (listApproval.Where(w => w.Status == open).ToList().Count > 0)
                {
                    status = SYDocumentStatus.PENDING.ToString();
                    // objMatch.IsApproved = false;
                }

                objMatch.Status = status;
                DB.HRDressReturns.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = Header.DocNo;
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
        public HRStaffProfile getNextApprover(string id, string pro)
        {
            var objStaff = new HRStaffProfile();
            var objHeader = DB.HRDressReturns.FirstOrDefault(w => w.DocNo == id);
            if (objHeader == null)
            {
                return new HRStaffProfile();
            }
            string open = SYDocumentStatus.OPEN.ToString();
            var listCanApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.Status == open && w.DocumentType == objHeader.DocType).ToList();

            if (listCanApproval.Count == 0)
            {
                return new HRStaffProfile();
            }

            var min = listCanApproval.Min(w => w.ApproveLevel);
            var NextApp = listCanApproval.Where(w => w.ApproveLevel == min).First();
            objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == NextApp.Approver);//, objHeader.Property);
            return objStaff;
        }

    }
    public class DeleteItem
    {
        public int id;
        public string DocNO;
        public string itemName;
        public int Qty;
        public string description;
        public string ReceiveID;
    }
    public class Return
    {
        public string EmpCode { get; set; }
        public string AllName { get; set; }
    }

}