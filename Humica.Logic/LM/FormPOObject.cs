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

namespace Humica.Logic.LM
{
    public class FormPOObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public HRPOHeader Header { get; set; }
        public List<HR_PO_VIEW> ListHeader { get; set; }
        public List<HR_PR_VIEW> ListPR { get; set; }
        public List<HRPORequestItem> ListPRItem { get; set; }
        public HRPODetail Detail { get; set; }
        public List<HRPODetail> ListDetail { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public HRStaffProfile StaffProfile { get; set; }
        public FormPOObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public void SetAutoApproval(string SCREEN_ID, string DocType, string Branch)
        {
            ListApproval = new List<ExDocApproval>();
            var DBX = new HumicaDBContext();
            var objDoc = DBX.ExCfWorkFlowItems.Find(SCREEN_ID, DocType);
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
        public HRStaffProfile getNextApprover(string id, string pro)
        {
            var objStaff = new HRStaffProfile();
            var DBX = new HumicaDBContext();
            var objHeader = DBX.HRPOHeaders.Find(id);
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
                ListDetail = new List<HRPODetail>();
                //var itemSelected = new SYSplitItem(ids);
                int line = 0;

                var DBX = new HumicaDBContext();

                var objPR = DB.HRPORequests.Find(ids);
                //var objPO = DB.HRPOTypes.FirstOrDefault();
                //if (objPO != null)
                //{
                //    Header.DocumentType = objPO.POType;
                //}

                if (objPR.Status != SYDocumentStatus.APPROVED.ToString())
                {
                    return "INV_DOC";
                }

                if (objPR == null)
                {
                    var checkList = ListPRItem.Where(w => w.RequestNumber == ids).ToList();

                    if (checkList.Count == 0)
                    {
                        return "NO_ITEM";
                    }
                    var obj = checkList.First();
                    var objNew = new HRPODetail();
                    SwapItemFromPO(obj, objNew);
                }
                else
                {
                    ListPRItem = DBX.HRPORequestItems.Where(w => w.RequestNumber == objPR.RequestNumber).ToList();

                    foreach (var read in ListPRItem)
                    {

                        var obj = read;
                        var objNew = new HRPODetail();
                        SwapItemFromPO(obj, objNew);
                        objNew.Amount = objNew.Amount;
                        line = line + 1;
                        objNew.LineNbr = line;
                        ListDetail.Add(objNew);
                    }
                    //       Header.DocurementReference = objPR.RequestNumber;

                }
                Header.Description = objPR.Description;
                Header.Branch = objPR.Branch;
                Header.Requestor = objPR.Requestor;
                Header.PromisedDate = objPR.ExtectedDate.Value;
            }
            catch
            {
                return "EE001";
            }

            return SYConstant.OK;
        }
        public void SwapItemFromPO(HRPORequestItem S, HRPODetail D)
        {
            D.Attachment = S.Attachment;
            D.DocumentReference = S.RequestNumber;
            D.LineNbr = S.LineItem;
            D.ItemCode = S.ItemCode;
            D.Unit = S.Unit;
            D.Qty = S.Quantity;
            D.Descr = S.ItemDescription;
            D.Amount = 0;
        }

        public string CreateHRPO()
        {
            try
            {
                var DBR = new HumicaDBContext();
                var objCF = DB.ExCfWorkFlowItems.Find(ScreenId, Header.DocumentType);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                var objNumber = new CFNumberRank(Header.DocumentType, ScreenId);
                Header.PONumber = objNumber.NextNumberRank.Trim();
                int lineItem = 0;
                var listNumber = new List<DocNo>();
                foreach (var read in ListDetail)
                {
                    lineItem++;
                    var objItem = new HRPODetail();
                    objItem.Attachment = read.Attachment;
                    objItem.ItemCode = read.ItemCode;
                    objItem.LineNbr = lineItem;
                    objItem.PONumber = Header.PONumber;
                    objItem.Descr = read.Descr;
                    objItem.Qty = read.Qty;
                    objItem.Unit = read.Unit;
                    objItem.Amount = read.Amount;
                    objItem.SubTotal = read.SubTotal;
                    objItem.DocumentReference = read.DocumentReference;
                    Header.DocumentReference = read.DocumentReference;
                    var objDoc = new DocNo();
                    objDoc.No = objItem.DocumentReference;
                    listNumber.Add(objDoc);
                    DB.HRPODetails.Add(objItem);
                }
                //Add approver
                foreach (var read in ListApproval)
                {
                    read.ID = 0;
                    read.LastChangedDate = DateTime.Now;
                    read.DocumentNo = Header.PONumber;
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
                Header.CreatedOn = DateTime.Now.Date;
                Header.CreatedBy = User.UserName;

                foreach (var item in listNumber.GroupBy(w => w.No).ToList())
                {
                    var objUpdate = DBR.HRPORequests.Find(item.Key);
                    if (objUpdate != null)
                    {
                        objUpdate.Status = SYDocumentStatus.COMPLETED.ToString();
                        DB.HRPORequests.Attach(objUpdate);
                        DB.Entry(objUpdate).Property(w => w.Status).IsModified = true;
                    }
                }

                DB.HRPOHeaders.Add(Header);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.PONumber;
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
                log.DocurmentAction = Header.PONumber;
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
                log.DocurmentAction = Header.PONumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditHRPO(string ID)
        {
            try
            {
                //HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = DB.HRPOHeaders.Find(ID);

                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                var objMatchD = DB.HRPODetails.Where(w => w.PONumber == ID).ToList();

                foreach (var item in objMatchD)
                {
                    DB.HRPODetails.Remove(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }

                objMatch.DocumentDate = Header.DocumentDate;
                objMatch.Description = Header.Description;
                objMatch.PromisedDate = Header.PromisedDate;
                objMatch.Branch = Header.Branch;
                objMatch.Requestor = Header.Requestor;
                objMatch.VendorName = Header.VendorName;
                objMatch.PaymentTerm = Header.PaymentTerm;
                objMatch.VendorAddress = Header.VendorAddress;
                objMatch.VendorEmail = Header.VendorEmail;
                objMatch.VendorPhone = Header.VendorPhone;
                objMatch.ShipName = Header.ShipName;
                objMatch.ShipAddress = Header.ShipAddress;
                objMatch.ShipEmail = Header.ShipEmail;
                objMatch.ShipTerm = Header.ShipTerm;
                objMatch.ShipPhone = Header.ShipPhone;
                objMatch.ContactPerson = Header.ContactPerson;
                // objMatch.Total = Header.Total;
                objMatch.Total = 0;
                int line = 1;
                foreach (var read in ListDetail)
                {
                    var checkListEdit = objMatchD.Where(w => w.ItemCode == read.ItemCode).ToList();
                    if (checkListEdit.Count == 0)
                    {
                        line++;
                    }
                    else
                    {
                        line = checkListEdit.First().LineNbr;
                    }
                    read.PONumber = Header.PONumber;
                    read.Descr = read.Descr;
                    read.Qty = read.Qty;
                    read.Amount = read.Amount;
                    read.Unit = read.Unit;
                    read.SubTotal = read.SubTotal;
                    read.LineNbr = line;
                    objMatch.Total += (read.Qty * read.Amount);
                    read.Attachment = read.Attachment;
                    if (checkListEdit.Count > 0)
                    {
                        var objUpdate = checkListEdit.First();
                        objUpdate.Descr = read.Descr;
                        objUpdate.Qty = read.Qty;
                        objUpdate.Unit = read.Unit;
                        objUpdate.Amount = read.Amount;
                        objUpdate.SubTotal = read.Qty * read.Amount;
                        objUpdate.Attachment = read.Attachment;
                        DB.HRPODetails.Attach(objUpdate);
                        DB.Entry(objUpdate).Property(w => w.Descr).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.Attachment).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.Qty).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.Unit).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.Amount).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.SubTotal).IsModified = true;
                    }
                    else
                    {
                        DB.HRPODetails.Add(read);
                    }

                    //  line += 1;
                }
                //Approval
                var listApprovalDoc = DB.ExDocApprovals.Where(w => w.DocumentType == objMatch.DocumentType && w.DocumentNo == objMatch.PONumber).ToList();

                foreach (var read in listApprovalDoc)
                {
                    DB.ExDocApprovals.Attach(read);
                    DB.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                }
                var objCF = DB.ExCfWorkFlowItems.Find(ScreenId, Header.DocumentType);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                foreach (var read in ListApproval)
                {
                    read.ID = 0;
                    read.LastChangedDate = DateTime.Now;
                    read.DocumentNo = Header.PONumber;
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

                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                DB.HRPOHeaders.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Description).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Branch).IsModified = true;
                DB.Entry(objMatch).Property(w => w.PromisedDate).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Requestor).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Total).IsModified = true;
                DB.Entry(objMatch).Property(w => w.VendorName).IsModified = true;
                DB.Entry(objMatch).Property(w => w.VendorAddress).IsModified = true;
                DB.Entry(objMatch).Property(w => w.VendorEmail).IsModified = true;
                DB.Entry(objMatch).Property(w => w.VendorPhone).IsModified = true;
                DB.Entry(objMatch).Property(w => w.PaymentTerm).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ShipPhone).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ShipTerm).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ShipAddress).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ShipEmail).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ShipName).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ContactPerson).IsModified = true;
                // int row1 = DBM.SaveChanges();
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.PONumber;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string deleteHRPOHeader(string ID)
        {
            try
            {
                HRPOHeader objCust = DB.HRPOHeaders.Find(ID);
                if (objCust == null)
                {
                    return "REQUEST_NE";
                }
                if (objCust.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }

                HumicaDBContext DBM = new HumicaDBContext();
                DBM.HRPOHeaders.Attach(objCust);
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
                log.ScreenId = Header.PONumber;
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
                log.ScreenId = Header.PONumber;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string approveTheDoc(string id)
        {
            try
            {
                HumicaDBContext DBX = new HumicaDBContext();
                var objMatch = DB.HRPOHeaders.FirstOrDefault(w => w.PONumber == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;
                //if (!isValidDataRestriction(objMatch))
                //{
                //    return "RESTRICT_ACCESS";
                //}

                if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                {
                    return "INV_DOC";
                }
                string open = SYDocumentStatus.OPEN.ToString();
                var listApproval = DBX.ExDocApprovals.Where(w => w.DocumentType == objMatch.DocumentType
                                    && w.DocumentNo == objMatch.PONumber && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
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

                        //if (read.ApproveLevel > listApproval.Min(w => w.ApproveLevel))
                        //{
                        //    return "REQUIRED_PRE_LEVEL";
                        //}
                        var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                        if (objStaff != null)
                        {
                            //New
                            if (listApproval.Where(w => w.ApproveLevel <= read.ApproveLevel).Count() >= listApproval.Count())
                            {
                                listApproval.ForEach(w => w.Status = SYDocumentStatus.APPROVED.ToString());
                            }
                            StaffProfile = objStaff;
                            read.ApprovedBy = objStaff.EmpCode;
                            read.ApprovedName = objStaff.AllName;
                            read.LastChangedDate = DateTime.Now.Date;
                            read.ApprovedDate = DateTime.Now;
                            read.Status = SYDocumentStatus.APPROVED.ToString();
                            DBX.ExDocApprovals.Attach(read);
                            DBX.Entry(read).State = System.Data.Entity.EntityState.Modified;
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

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                int row = DBX.SaveChanges();
                #region *****Send to Telegram
                var _Objmatch = DBV.HR_PO_VIEW.FirstOrDefault(w => w.PONumber == objMatch.PONumber);

                var excfObject = DB.ExCfWorkFlowItems.Find(ScreenId, objMatch.DocumentType);
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
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
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
                var objMatch = DB.HRPOHeaders.FirstOrDefault(w => w.PONumber == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;
                //if (!isValidDataRestriction(objMatch))
                //{
                //    return "RESTRICT_ACCESS";
                //}

                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }

                objMatch.Status = SYDocumentStatus.PENDING.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                int row = DBX.SaveChanges();

                #region *****Send to Telegram
                var Objmatch = DBV.HR_PO_VIEW.FirstOrDefault(w => w.PONumber == Header.PONumber);
                var excfObject = DB.ExCfWorkFlowItems.Find(ScreenId, Header.DocumentType);
                Humica.Core.SY.SYSendTelegramObject wfo =
                    new Humica.Core.SY.SYSendTelegramObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER, Header.Status);
                wfo.User = User;
                wfo.ListObjectDictionary = new List<object>();
                wfo.ListObjectDictionary.Add(Objmatch);
                HRStaffProfile Staff = getNextApprover(Header.PONumber, "");
                wfo.ListObjectDictionary.Add(Staff);
                wfo.Send_SMS_Telegram(excfObject.Telegram, "");
                #endregion

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = Header.PONumber;
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
                log.DocurmentAction = Header.PONumber;
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
                log.DocurmentAction = Header.PONumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CancelPO(string ApprovalID)
        {
            try
            {
                //   HumicaDBContext DB = new HumicaDBContext();
                string cancelled = SYDocumentStatus.CANCELLED.ToString();
                string PONumber = ApprovalID;
                var objmatch = DB.HRPOHeaders.Find(PONumber);
                if (objmatch.Status != cancelled)
                {
                    objmatch.Status = cancelled;
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                }
                DB.SaveChanges();

                #region *****Send to Telegram
                var _Objmatch = DBV.HR_PO_VIEW.FirstOrDefault(w => w.PONumber == objmatch.PONumber);

                HRStaffProfile Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objmatch.Requestor);
                var excfObject = DB.ExCfWorkFlowItems.Find(ScreenId, objmatch.DocumentType);
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
                log.ScreenId = ScreenId;
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
                log.ScreenId = ScreenId;
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
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ApprovalID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
    public class DocNo
    {
        public string No { get; set; }
        public string Pro { get; set; }
    }
}