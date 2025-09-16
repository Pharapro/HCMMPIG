using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.MD
{

    public class ClsHRPurchaseReceipt
    {
        private HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public string ScreenID { get; set; }
        public SYUserBusiness BS { get; set; }
        public static string PARAM_BRANCH = "PARAM_BRANCH";
        public string Plant { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangedOn { get; set; }
        public bool IsActive { get; set; }
        public HRPOReceipt Header { get; set; }
        public HRPOReceiptItem Item { get; set; }
        public List<HRPOReceipt> ListReceitp { get; set; }
        public List<HRPOReceiptItem> ListRCItem { get; set; }
        public List<HR_POReceipt_View> ListHeader { get; set; }
        public List<HR_PO_VIEW> ListPO { get; set; }
        public List<HRPODetail> ListPOItem { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public HRStaffProfile StaffProfile { get; set; }
        public ClsHRPurchaseReceipt()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public void SetAutoApproval(string DocType, string Branch)
        {
            ListApproval = new List<ExDocApproval>();
            var DBX = new HumicaDBContext();
            var objDoc = DBX.ExCfWorkFlowItems.Find(ScreenID, DocType);
            if (objDoc != null)
            {
                if (objDoc.IsRequiredApproval == true)
                {
                    var listDefaultApproval = DBX.ExCfWFApprovers.Where(w => w.Branch == Branch && w.WFObject == objDoc.ApprovalFlow && w.IsSelected == true).ToList();
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
        public string CreateHRPReceipt()
        {
            try
            {
                var DBR = new HumicaDBContext();
                if (ListRCItem.Count == 0)
                {
                    return "LIST_NE";
                }
                var objCF = DB.ExCfWorkFlowItems.Find(ScreenID, Header.DocumentType);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                var objNumber = new CFNumberRank(Header.DocumentType, ScreenID);
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.ReceiptNo = objNumber.NextNumberRank.Trim();
                int lineItem = 0;
                Header.NetAmount = 0;
                Header.TotalDiscount = 0;
                Header.TotalAmount = 0;
                var listNumber = new List<DocNo>();
                foreach (var read in ListRCItem.ToList())
                {
                    lineItem++;
                    var objItem = new HRPOReceiptItem();
                    objItem.LineItem = lineItem;
                    objItem.ReceiptNo = Header.ReceiptNo;
                    objItem.Attachment = read.Attachment;
                    objItem.ItemCode = read.ItemCode;
                    objItem.ItemDescription = read.ItemDescription;
                    objItem.Unit = read.Unit;
                    objItem.Quantity = read.Quantity;
                    objItem.UnitPrice = read.UnitPrice;
                    objItem.Discount = read.Discount;
                    objItem.PercentageDiscount = read.PercentageDiscount;
                    objItem.NetAmount = read.NetAmount;
                    objItem.Amount = read.Amount;
                    objItem.Status = SYDocumentStatus.OPEN.ToString();
                    Header.NetAmount += Convert.ToDecimal(read.NetAmount);
                    Header.TotalAmount += Convert.ToDecimal(read.Amount);
                    Header.TotalDiscount += Convert.ToDecimal(read.Discount);
                    var objDoc = new DocNo();
                    objDoc.No = Header.DocurementReference;
                    listNumber.Add(objDoc);
                    DB.HRPOReceiptItems.Add(objItem);
                }
                //Add approver
                foreach (var read in ListApproval)
                {
                    read.ID = 0;
                    read.LastChangedDate = DateTime.Now;
                    read.DocumentNo = Header.ReceiptNo;
                    read.DocumentType = Header.DocumentType;
                    read.Status = SYDocumentStatus.OPEN.ToString();
                    read.WFObject = objCF.ApprovalFlow;
                    read.ApprovedBy = "";
                    read.ApprovedName = "";
                    read.ApproverName = "";
                    var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                    if (objStaff != null)
                    {
                        read.ApproverName = objStaff.AllName;
                    }
                    DB.ExDocApprovals.Add(read);
                }
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;

                foreach (var item in listNumber.GroupBy(w => w.No).ToList())
                {
                    var objUpdate = DBR.HRPOHeaders.Find(item.Key);
                    if (objUpdate != null)
                    {
                        objUpdate.Status = SYDocumentStatus.COMPLETED.ToString();
                        DB.HRPOHeaders.Attach(objUpdate);
                        DB.Entry(objUpdate).Property(w => w.Status).IsModified = true;
                    }
                }
                DB.SaveChanges();
                DB.HRPOReceipts.Add(Header);
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ReceiptNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ReceiptNo;

                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ReceiptNo;

                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditHRPReceipt(string id)
        {
            try
            {
                DB = new HumicaDBContext();
                if (ListRCItem.Count == 0)
                {
                    return "LIST_NE";
                }
                //Check Duplicate CampaignClass Header
                var ObjMatch = DB.HRPOReceipts.FirstOrDefault(w => w.ReceiptNo == id);

                if (ObjMatch == null)
                {
                    return "DOC_INV";
                }
                var ObjMatchItem = DB.HRPOReceiptItems.Where(w => w.ReceiptNo == id).ToList();
                foreach (var item in ObjMatchItem)
                {
                    DB.HRPOReceiptItems.Remove(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                int line = 1;
                foreach (var read in ListRCItem.ToList())
                {
                    var checkListEdit = ObjMatchItem.Where(w => w.LineItem == read.LineItem).ToList();
                    if (checkListEdit.Count == 0)
                    {
                        line++;
                    }
                    else
                    {
                        line = checkListEdit.First().LineItem;
                    }
                    var objItem = new HRPOReceiptItem();
                    objItem.LineItem = line;
                    objItem.ReceiptNo = Header.ReceiptNo;
                    objItem.Attachment = read.Attachment;
                    objItem.ItemCode = read.ItemCode;
                    objItem.ItemDescription = read.ItemDescription;
                    objItem.Quantity = read.Quantity;
                    objItem.Unit = read.Unit;
                    objItem.UnitPrice = read.UnitPrice;
                    objItem.Discount = read.Discount;
                    objItem.PercentageDiscount = read.PercentageDiscount;
                    objItem.NetAmount = read.NetAmount;
                    objItem.Amount = read.Amount;
                    objItem.Status = SYDocumentStatus.OPEN.ToString();
                    objItem.LineReference = read.LineReference;
                    objItem.VatTaxable = read.VatExemtion;
                    objItem.Remark = read.Remark;
                    if (checkListEdit.Count > 0)
                    {
                        var objUpdate = checkListEdit.First();
                        objUpdate.Attachment = read.Attachment;
                        objUpdate.ItemCode = read.ItemCode;
                        objUpdate.ItemDescription = read.ItemDescription;
                        objUpdate.Quantity = read.Quantity;
                        objUpdate.Unit = read.Unit;
                        objUpdate.UnitPrice = read.UnitPrice;
                        objUpdate.Discount = read.Discount;
                        objUpdate.PercentageDiscount = read.PercentageDiscount;
                        objUpdate.NetAmount = read.NetAmount;
                        objUpdate.Amount = read.Amount;
                        objUpdate.LineReference = read.LineReference;
                        objUpdate.VatTaxable = read.VatExemtion;
                        objUpdate.Remark = read.Remark;
                        DB.HRPOReceiptItems.Attach(objUpdate);
                        DB.Entry(objUpdate).Property(w => w.Attachment).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.ItemCode).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.ItemDescription).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.Unit).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.Quantity).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.UnitPrice).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.Discount).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.PercentageDiscount).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.NetAmount).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.Amount).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.LineReference).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.VatTaxable).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.Remark).IsModified = true;
                    }
                    else
                    {
                        DB.HRPOReceiptItems.Add(objItem);
                    }
                }
                var listApprovalDoc = DB.ExDocApprovals.Where(w => w.DocumentType == ObjMatch.DocumentType && w.DocumentNo == ObjMatch.ReceiptNo).ToList();

                foreach (var read in listApprovalDoc)
                {
                    DB.ExDocApprovals.Attach(read);
                    DB.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                }
                var objCF = DB.ExCfWorkFlowItems.Find(ScreenID, Header.DocumentType);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                foreach (var read in ListApproval)
                {
                    read.ID = 0;
                    read.LastChangedDate = DateTime.Now;
                    read.DocumentNo = Header.ReceiptNo;
                    read.DocumentType = Header.DocumentType;
                    read.Status = SYDocumentStatus.OPEN.ToString();
                    read.WFObject = objCF.ApprovalFlow;
                    read.ApprovedBy = "";
                    read.ApprovedName = "";
                    read.ApproverName = "";
                    var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                    if (objStaff != null)
                    {
                        read.ApproverName = objStaff.AllName;
                    }
                    DB.ExDocApprovals.Add(read);
                }
                //Update user infor CampaignClass Header
                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;
                ObjMatch.DocumentDate = Header.DocumentDate;
                ObjMatch.Branch = Header.Branch;
                ObjMatch.VendorName = Header.VendorName;
                ObjMatch.VendorReference = Header.VendorReference;
                ObjMatch.CurrencyCode = Header.CurrencyCode;
                ObjMatch.DocurementReference = Header.DocurementReference;
                ObjMatch.VatTotal = Header.VatTotal;
                ObjMatch.Description = Header.Description;
                ObjMatch.TotalAmount = Convert.ToDecimal(ListRCItem.Sum(w => w.Amount));
                ObjMatch.TotalDiscount = Convert.ToDecimal(ListRCItem.Sum(w => w.Discount));
                ObjMatch.NetAmount = Convert.ToDecimal(ListRCItem.Sum(w => w.NetAmount));

                DB.HRPOReceipts.Attach(ObjMatch);
                DB.Entry(ObjMatch).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChangedOn).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.DocumentDate).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Branch).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.VendorName).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.VendorReference).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.CurrencyCode).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.DocurementReference).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.VatTotal).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.TotalAmount).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.TotalDiscount).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.NetAmount).IsModified = true;


                DB.SaveChanges();
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ReceiptNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ReceiptNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteHRReceipt(string id)
        {
            try
            {
                var objItem = DB.HRPOReceipts.FirstOrDefault(w => w.ReceiptNo == id);
                if (objItem == null)
                {
                    return "EE001";
                }
                var checkdupInfo = DB.HRPOReceiptItems.Where(w => w.ReceiptNo == id).ToList();
                foreach (var read in checkdupInfo.ToList())
                {
                    DB.HRPOReceiptItems.Remove(read);
                }
                DB.HRPOReceipts.Remove(objItem);
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ReceiptNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string requestToApprove(string id)
        {
            try
            {
                HumicaDBContext DBX = new HumicaDBContext();
                var objMatch = DB.HRPOReceipts.FirstOrDefault(w => w.ReceiptNo == id);
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

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                int row = DBX.SaveChanges();

                #region *****Send to Telegram
                var Objmatch = DBV.HR_POReceipt_View.FirstOrDefault(w => w.ReceiptNo == Header.ReceiptNo);
                var excfObject = DB.ExCfWorkFlowItems.Find(ScreenID, Header.DocumentType);
                Humica.Core.SY.SYSendTelegramObject wfo =
                    new Humica.Core.SY.SYSendTelegramObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER, Header.Status);
                wfo.User = User;
                wfo.ListObjectDictionary = new List<object>();
                wfo.ListObjectDictionary.Add(Objmatch);
                HRStaffProfile Staff = getNextApprover(Header.ReceiptNo, "");
                wfo.ListObjectDictionary.Add(Staff);
                wfo.Send_SMS_Telegram(excfObject.Telegram, "");
                #endregion

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.ScreenId = Header.ReceiptNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ReceiptNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ReceiptNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ReleaseTheDoc(string id)
        {
            try
            {
                HumicaDBContext DBX = new HumicaDBContext();
                var objMatch = DB.HRPOReceipts.FirstOrDefault(w => w.ReceiptNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;

                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }
                //string open = SYDocumentStatus.OPEN.ToString();
                //var listApproval = DBX.ExDocApprovals.Where(w => w.DocumentType == objMatch.DocumentType
                //                    && w.DocumentNo == objMatch.ReceiptNo && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                //var listUser = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                //var b = false;
                //foreach (var read in listApproval)
                //{
                //    var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
                //    if (checklist.Count > 0)
                //    {
                //        if (read.Status == SYDocumentStatus.APPROVED.ToString())
                //        {
                //            return "USER_ALREADY_APP";
                //        }

                //        var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                //        if (objStaff != null)
                //        {
                //            //New
                //            if (listApproval.Where(w => w.ApproveLevel <= read.ApproveLevel).Count() >= listApproval.Count())
                //            {
                //                listApproval.ForEach(w => w.Status = SYDocumentStatus.APPROVED.ToString());
                //            }
                //            StaffProfile = objStaff;
                //            read.ApprovedBy = objStaff.EmpCode;
                //            read.ApprovedName = objStaff.AllName;
                //            read.LastChangedDate = DateTime.Now.Date;
                //            read.ApprovedDate = DateTime.Now;
                //            read.Status = SYDocumentStatus.APPROVED.ToString();
                //            DBX.ExDocApprovals.Attach(read);
                //            DBX.Entry(read).State = System.Data.Entity.EntityState.Modified;
                //            b = true;
                //            break;
                //        }
                //    }

                //}
                //if (listApproval.Count > 0)
                //{
                //    if (b == false)
                //    {
                //        return "USER_NOT_APPROVOR";
                //    }
                //}
                var status = SYDocumentStatus.RELEASED.ToString();

                //if (listApproval.Where(w => w.Status == open).ToList().Count > 0)
                //{
                //    status = SYDocumentStatus.PENDING.ToString();
                //    // objMatch.IsApproved = false;
                //}

                objMatch.Status = status;

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                int row = DBX.SaveChanges();
                #region *****Send to Telegram
                var _Objmatch = DBV.HR_POReceipt_View.FirstOrDefault(w => w.ReceiptNo == objMatch.ReceiptNo);
                var excfObject = DB.ExCfWorkFlowItems.Find(ScreenID, objMatch.DocumentType);
                Humica.Core.SY.SYSendTelegramObject wfo =
                    new Humica.Core.SY.SYSendTelegramObject(excfObject.ApprovalFlow, WorkFlowType.APPROVER, objMatch.Status);
                wfo.User = User;
                wfo.ListObjectDictionary = new List<object>();
                wfo.ListObjectDictionary.Add(_Objmatch);
                wfo.ListObjectDictionary.Add(StaffProfile);
                wfo.Send_SMS_Telegram(excfObject.Telegram, "");
                #endregion
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.ScreenId = Header.ReceiptNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ReceiptNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.ReceiptNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CancelRP(string ApprovalID)
        {
            try
            {
                string cancelled = SYDocumentStatus.CANCELLED.ToString();
                string PONumber = ApprovalID;
                var objmatch = DB.HRPOReceipts.Find(PONumber);
                if (objmatch.Status != cancelled)
                {
                    objmatch.Status = cancelled;
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                }
                DB.SaveChanges();

                #region *****Send to Telegram
                var _Objmatch = DBV.HR_POReceipt_View.FirstOrDefault(w => w.ReceiptNo == objmatch.ReceiptNo);

                var excfObject = DB.ExCfWorkFlowItems.Find(ScreenID, objmatch.DocumentType);
                HRStaffProfile Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objmatch.RequestedBy);

                Humica.Core.SY.SYSendTelegramObject wfo =
                    new Humica.Core.SY.SYSendTelegramObject(excfObject.ApprovalFlow, WorkFlowType.COLLECTOR, objmatch.Status);
                wfo.User = User;
                wfo.ListObjectDictionary = new List<object>();
                wfo.ListObjectDictionary.Add(_Objmatch);
                wfo.ListObjectDictionary.Add(Staff);
                wfo.Send_SMS_Telegram(excfObject.Telegram, "");
                #endregion
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = ApprovalID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = ApprovalID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = ApprovalID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public HRStaffProfile getNextApprover(string id, string pro)
        {
            var objStaff = new HRStaffProfile();
            var DBX = new HumicaDBContext();
            var objHeader = DBX.HRPORequests.Find(id);
            if (objHeader == null)
            {
                return new HRStaffProfile();
            }
            string open = SYDocumentStatus.OPEN.ToString();
            var listCanApproval = DBX.ExDocApprovals.Where(w => w.DocumentNo == id && w.Status == open && w.DocumentType == objHeader.DocumentType).ToList();

            if (listCanApproval.Count == 0)
            {
                return new HRStaffProfile();
            }

            var min = listCanApproval.Min(w => w.ApproveLevel);
            var NextApp = listCanApproval.Where(w => w.ApproveLevel == min).First();
            objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == NextApp.Approver);//, objHeader.Property);
            return objStaff;
        }
        public string isValidReference(string ids)
        {
            try
            {
                ListRCItem = new List<HRPOReceiptItem>();
                int line = 0;

                var DBX = new HumicaDBContext();

                var objPO = DB.HRPOHeaders.Find(ids);

                if (objPO.Status != SYDocumentStatus.APPROVED.ToString())
                {
                    return "INV_DOC";
                }

                if (objPO == null)
                {
                    var checkList = ListPOItem.Where(w => w.PONumber == ids).ToList();

                    if (checkList.Count == 0)
                    {
                        return "NO_ITEM";
                    }
                    var obj = checkList.First();
                    var objNew = new HRPOReceiptItem();
                    //var objj = new HRPOReceipt();
                    SwapItemFromPO(obj, objNew);
                }
                else
                {
                    ListPOItem = DBX.HRPODetails.Where(w => w.PONumber == objPO.PONumber).ToList();

                    foreach (var read in ListPOItem)
                    {
                        var obj = read;
                        var objNew = new HRPOReceiptItem();
                        SwapItemFromPO(obj, objNew);
                        line = line + 1;
                        objNew.LineItem = line;
                        ListRCItem.Add(objNew);
                    }
                }
                Header.Description = objPO.Description;
                Header.Branch = objPO.Branch;
            }
            catch
            {
                return "EE001";
            }

            return SYConstant.OK;
        }
        public void SwapItemFromPO(HRPODetail S, HRPOReceiptItem D)
        {
            D.ReceiptNo = S.DocumentReference;
            D.ItemCode = S.ItemCode;
            D.Unit = S.Unit;
            D.Quantity = S.Qty;
            D.ItemDescription = S.Descr;
            D.NetAmount = S.SubTotal;
            D.Amount = S.Amount;
            D.Attachment = S.Attachment;
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
        public class DocNo
        {
            public string No { get; set; }
            public string Pro { get; set; }
        }
    }

}
