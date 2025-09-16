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

    public class ClsHRResquestPayment
    {
        private HumicaDBContext DB = new HumicaDBContext();
        private HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public string ScreenID { get; set; }
        public SYUserBusiness BS { get; set; }
        public HRReqPayment Header { get; set; }

        public HRReqPaymentItem Item { get; set; }

        public static string PARAM_BRANCH = "PARAM_BRANCH";
        public string Plant { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangedOn { get; set; }
        public bool IsActive { get; set; }
        public List<HR_RP_View> ListPayment { get; set; }
        public List<HR_PO_VIEW> ListPO { get; set; }
        public List<HRPODetail> ListPOItem { get; set; }
        public List<HRReqPaymentItem> ListPItem { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public HRStaffProfile StaffProfile { get; set; }
        public ClsHRResquestPayment()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public HRStaffProfile getNextApprover(string id, string pro)
        {
            var objStaff = new HRStaffProfile();
            var DBX = new HumicaDBContext();
            var objHeader = DBX.HRReqPayments.Find(id);
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
        public string isValidReference(string ids)
        {
            try
            {
                ListPItem = new List<HRReqPaymentItem>();
                int line = 0;

                var DBX = new HumicaDBContext();

                var objPO = DB.HRPOHeaders.Find(ids);
                //var objRP = DB.HRReqPayTypes.FirstOrDefault();
                //if (objRP != null)
                //{
                //    Header.DocumentType = objRP.RPType;
                //}

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
                    var objNew = new HRReqPaymentItem();
                    SwapItemFromPO(obj, objNew);
                }
                else
                {
                    ListPOItem = DBX.HRPODetails.Where(w => w.PONumber == objPO.PONumber).ToList();

                    foreach (var read in ListPOItem)
                    {
                        var obj = read;
                        var objNew = new HRReqPaymentItem();
                        SwapItemFromPO(obj, objNew);
                        line = line + 1;
                        objNew.LineItem = line;
                        ListPItem.Add(objNew);
                    }
                }
                Header.Description = objPO.Description;
                Header.AdvanceDate = objPO.PromisedDate;
            }
            catch
            {
                return "EE001";
            }

            return SYConstant.OK;
        }
        public void SwapItemFromPO(HRPODetail S, HRReqPaymentItem D)
        {
            D.DocumentReference = S.PONumber;
            //  D.ItemCode = S.ItemCode;
            //  D.Unit = S.Unit;
            //  D.Quantity = S.Qty;
            D.Description = S.Descr;
            D.AmountReq = S.SubTotal;
            D.AmountRev = 0;
        }

        public string DeleteHRRPayment(string id)
        {
            try
            {
                var objItem = DB.HRReqPayments.FirstOrDefault(w => w.RPNumber == id);
                var objInfo = DB.HRReqPaymentItems.Where(w => w.RPNumber == id);
                if (objItem == null)
                {
                    return "EE001";
                }
                var checkdupInfo = DB.HRReqPaymentItems.Where(w => w.RPNumber == id).ToList();

                foreach (var read in checkdupInfo.ToList())
                {
                    DB.HRReqPaymentItems.Remove(read);
                }

                DB.HRReqPayments.Remove(objItem);
                DB.SaveChanges();
                return SYConstant.OK;
            }

            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.RPNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CreateHRRPayment()
        {
            try
            {
                var DBR = new HumicaDBContext();
                if (ListPItem.Count == 0)
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
                Header.RPNumber = objNumber.NextNumberRank.Trim();
                int lineItem = 0;
                Header.TotalAmountRev = 0;
                Header.TotalAmountReq = 0;
                var listNumber = new List<DocNo>();
                foreach (var read in ListPItem.ToList())
                {
                    lineItem++;
                    var objItem = new HRReqPaymentItem();
                    objItem.LineItem = lineItem;
                    objItem.RPNumber = Header.RPNumber;
                    objItem.Description = read.Description;
                    objItem.SupportingDoc = read.SupportingDoc;
                    objItem.AmountReq = read.AmountReq;
                    objItem.AmountRev = read.AmountRev;
                    objItem.DocumentReference = read.DocumentReference;
                    Header.TotalAmountReq += Convert.ToDecimal(read.AmountReq);
                    Header.TotalAmountRev += Convert.ToDecimal(read.AmountRev);
                    var objDoc = new DocNo();
                    objDoc.No = objItem.DocumentReference;
                    listNumber.Add(objDoc);
                    DB.HRReqPaymentItems.Add(objItem);
                }
                //Add approver
                foreach (var read in ListApproval)
                {
                    read.ID = 0;
                    read.LastChangedDate = DateTime.Now;
                    read.DocumentNo = Header.RPNumber;
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
                DB.HRReqPayments.Add(Header);
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.RPNumber;
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
                log.DocurmentAction = Header.RPNumber;

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
                log.DocurmentAction = Header.RPNumber;
                //Header.CalendarID= HeaderCalen.CalendarID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditHRRPayment(string id)
        {
            try
            {
                DB = new HumicaDBContext();
                if (ListPItem.Count == 0)
                {
                    return "LIST_NE";
                }
                //Check Duplicate CampaignClass Header
                var ObjMatch = DB.HRReqPayments.FirstOrDefault(w => w.RPNumber == id);

                if (ObjMatch == null)
                {
                    return "DOC_INV";
                }
                var ObjMatchItem = DB.HRReqPaymentItems.Where(w => w.RPNumber == id).ToList();
                int line = 1;
                foreach (var read in ListPItem.ToList())
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
                    var objItem = new HRReqPaymentItem();
                    objItem.LineItem = line;
                    objItem.RPNumber = Header.RPNumber;
                    objItem.Description = read.Description;
                    objItem.SupportingDoc = read.SupportingDoc;
                    objItem.AmountReq = read.AmountReq;
                    objItem.AmountRev = read.AmountRev;
                    if (checkListEdit.Count > 0)
                    {
                        var objUpdate = checkListEdit.First();
                        objUpdate.Description = read.Description;
                        objUpdate.SupportingDoc = read.SupportingDoc;
                        objUpdate.AmountReq = read.AmountReq;
                        objUpdate.AmountRev = read.AmountRev;
                        DB.HRReqPaymentItems.Attach(objUpdate);
                        DB.Entry(objUpdate).Property(w => w.Description).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.SupportingDoc).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.AmountReq).IsModified = true;
                        DB.Entry(objUpdate).Property(w => w.AmountRev).IsModified = true;
                    }
                    else
                    {
                        DB.HRReqPaymentItems.Add(objItem);
                    }
                }
                var listApprovalDoc = DB.ExDocApprovals.Where(w => w.DocumentType == ObjMatch.DocumentType && w.DocumentNo == ObjMatch.RPNumber).ToList();

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
                    read.DocumentNo = Header.RPNumber;
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
                ObjMatch.PaymentVendor = Header.PaymentVendor;
                ObjMatch.Description = Header.Description;
                ObjMatch.AdvanceAmount = Header.AdvanceAmount;
                ObjMatch.AdvanceDate = Header.AdvanceDate;
                ObjMatch.DeliveryDate = Header.DeliveryDate;
                ObjMatch.DueToEmployee = Header.DueToEmployee;
                ObjMatch.DueToCompany = Header.DueToCompany;
                ObjMatch.Advance = Header.Advance;
                ObjMatch.Branch = Header.Branch;
                ObjMatch.BeneAcc = Header.BeneAcc;
                ObjMatch.BeneBank = Header.BeneBank;
                ObjMatch.BeneName = Header.BeneName;
                ObjMatch.Currency = Header.Currency;
                ObjMatch.PayymenyStaff = Header.PayymenyStaff;
                ObjMatch.Requestor = Header.Requestor;
                ObjMatch.SettlementAdvance = Header.SettlementAdvance;
                ObjMatch.TotalAmountRev = Convert.ToDecimal(ListPItem.Sum(w => w.AmountRev));
                ObjMatch.TotalAmountReq = Convert.ToDecimal(ListPItem.Sum(w => w.AmountReq));

                DB.HRReqPayments.Attach(ObjMatch);
                DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.DeliveryDate).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Branch).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.PayymenyStaff).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.PaymentVendor).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChangedOn).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Requestor).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.SettlementAdvance).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.BeneName).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.BeneBank).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.BeneAcc).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Advance).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Currency).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.TotalAmountReq).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.TotalAmountRev).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.AdvanceAmount).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.AdvanceDate).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.DueToCompany).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.DueToEmployee).IsModified = true;
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.RPNumber;
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
                log.DocurmentAction = Header.RPNumber;
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
                var objMatch = DB.HRReqPayments.FirstOrDefault(w => w.RPNumber == id);
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
                var Objmatch = DBV.HR_RP_View.FirstOrDefault(w => w.RPNumber == Header.RPNumber);
                var excfObject = DB.ExCfWorkFlowItems.Find(ScreenID, Header.DocumentType);
                Humica.Core.SY.SYSendTelegramObject wfo =
                    new Humica.Core.SY.SYSendTelegramObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER, Header.Status);
                wfo.User = User;
                wfo.ListObjectDictionary = new List<object>();
                wfo.ListObjectDictionary.Add(Objmatch);
                HRStaffProfile Staff = getNextApprover(Header.RPNumber, "");
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
                log.ScreenId = Header.RPNumber;
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
                log.DocurmentAction = Header.RPNumber;
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
                log.DocurmentAction = Header.RPNumber;
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
                HumicaDBContext DBX = new HumicaDBContext();
                var objMatch = DB.HRReqPayments.FirstOrDefault(w => w.RPNumber == id);
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
                                    && w.DocumentNo == objMatch.RPNumber && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
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
                var _Objmatch = DBV.HR_RP_View.FirstOrDefault(w => w.RPNumber == objMatch.RPNumber);
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
                log.ScreenId = Header.RPNumber;
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
                log.DocurmentAction = Header.RPNumber;
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
                log.DocurmentAction = Header.RPNumber;
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
                //   HumicaDBContext DB = new HumicaDBContext();
                string cancelled = SYDocumentStatus.CANCELLED.ToString();
                string PONumber = ApprovalID;
                var objmatch = DB.HRReqPayments.Find(PONumber);
                if (objmatch.Status != cancelled)
                {
                    objmatch.Status = cancelled;
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                }
                DB.SaveChanges();

                #region *****Send to Telegram
                var _Objmatch = DBV.HR_RP_View.FirstOrDefault(w => w.RPNumber == objmatch.RPNumber);

                var excfObject = DB.ExCfWorkFlowItems.Find(ScreenID, objmatch.DocumentType);
                HRStaffProfile Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objmatch.Requestor);

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
    }
    public class DocNo
    {
        public string No { get; set; }
        public string Pro { get; set; }
    }
}
