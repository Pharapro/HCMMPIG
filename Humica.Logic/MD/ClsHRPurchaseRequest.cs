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

    public class ClsHRPurchaseRequest
    {
        private HumicaDBContext DB = new HumicaDBContext();
        private HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public string ScreenID { get; set; }
        public SYUserBusiness BS { get; set; }
        public HRPORequest Header { get; set; }

        public HRPORequestItem Item { get; set; }

        public static string PARAM_BRANCH = "PARAM_BRANCH";
        public string Plant { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangedOn { get; set; }
        public bool IsActive { get; set; }
        public List<HR_PR_VIEW> ListPORequest { get; set; }
        public List<HRPORequestItem> ListPRItem { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public HRStaffProfile StaffProfile { get; set; }
        public ClsHRPurchaseRequest()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
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
        public void SetAutoApproval(string ScreenID, string DocType, string Branch)
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
        public string DeletePORequest(string id)
        {
            try
            {
                var objItem = DB.HRPORequests.FirstOrDefault(w => w.RequestNumber == id);
                if (objItem == null)
                {
                    return "REQUEST_NE";
                }
                if (objItem.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }
                //var objInfo = DB.HRPORequestItems.Where(w => w.RequestNumber == id);

                var checkdupInfo = DB.HRPORequestItems.Where(w => w.RequestNumber == id).ToList();

                foreach (var read in checkdupInfo.ToList())
                {
                    DB.HRPORequestItems.Remove(read);
                }

                DB.HRPORequests.Remove(objItem);
                DB.SaveChanges();
                return SYConstant.OK;
            }

            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.RequestNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CreatePORequest()
        {
            DB = new HumicaDBContext();
            try
            {
                if (string.IsNullOrEmpty(Header.Branch))
                {
                    return "BRANCH_EN";
                }
                if (ListPRItem.Count == 0)
                {
                    return "LIST_NE";
                }
                var objCF = DB.ExCfWorkFlowItems.Find(ScreenID, Header.DocumentType);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                var objNumber = new CFNumberRank(Header.DocumentType, ScreenID);
                Header.RequestNumber = objNumber.NextNumberRank.Trim();
                int lineItem = 0;
                Header.TotalQty = 0;
                foreach (var read in ListPRItem.ToList())
                {
                    lineItem++;
                    var objItem = new HRPORequestItem();
                    objItem.LineItem = lineItem;
                    objItem.ItemCode = read.ItemCode;
                    objItem.RequestNumber = Header.RequestNumber;
                    objItem.ItemDescription = read.ItemDescription;
                    objItem.Branch = Header.Branch;
                    objItem.Brand = read.Brand;
                    objItem.Unit = read.Unit;
                    objItem.Quantity = read.Quantity;
                    objItem.Remark = read.Remark;
                    objItem.Attachment = read.Attachment;
                    Header.TotalQty += read.Quantity;
                    DB.HRPORequestItems.Add(objItem);
                }
                //Add approver
                foreach (var read in ListApproval)
                {
                    read.ID = 0;
                    read.LastChangedDate = DateTime.Now;
                    read.DocumentNo = Header.RequestNumber;
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
                DB.HRPORequests.Add(Header);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.RequestNumber;
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
                log.DocurmentAction = Header.RequestNumber;

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
                log.DocurmentAction = Header.RequestNumber;
                //Header.CalendarID= HeaderCalen.CalendarID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditPORequest(string id)
        {
            try
            {
                DB = new HumicaDBContext();
                if (ListPRItem.Count == 0)
                {
                    return "LIST_NE";
                }
                //Check Duplicate CampaignClass Header
                var ObjMatch = DB.HRPORequests.FirstOrDefault(w => w.RequestNumber == id);

                if (ObjMatch == null)
                {
                    return "DOC_INV";
                }
                var ObjMatchItem = DB.HRPORequestItems.Where(w => w.RequestNumber == id).ToList();
                foreach (var item in ObjMatchItem)
                {
                    DB.HRPORequestItems.Remove(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                int line = 1;
                foreach (var read in ListPRItem.ToList())
                {
                    var objItem = new HRPORequestItem();
                    objItem.LineItem = line;
                    objItem.ItemCode = read.ItemCode;
                    objItem.RequestNumber = Header.RequestNumber;
                    objItem.ItemDescription = read.ItemDescription;
                    objItem.Branch = Header.Branch;
                    objItem.Brand = read.Brand;
                    objItem.Unit = read.Unit;
                    objItem.Quantity = read.Quantity;
                    objItem.Remark = read.Remark;
                    objItem.Attachment = read.Attachment;

                    DB.HRPORequestItems.Add(objItem);
                    line++;
                }
                var listApprovalDoc = DB.ExDocApprovals.Where(w => w.DocumentType == ObjMatch.DocumentType && w.DocumentNo == ObjMatch.RequestNumber).ToList();

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
                    read.DocumentNo = Header.RequestNumber;
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
                ObjMatch.Branch = Header.Branch;
                ObjMatch.ExtectedDate = Header.ExtectedDate;
                ObjMatch.Requestor = Header.Requestor;
                ObjMatch.Description = Header.Description;
                ObjMatch.TotalQty = Convert.ToDecimal(ListPRItem.Sum(w => w.Quantity));

                DB.HRPORequests.Attach(ObjMatch);
                DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Branch).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.TotalQty).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ExtectedDate).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChangedOn).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Requestor).IsModified = true;
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.RequestNumber;
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
                log.DocurmentAction = Header.RequestNumber;
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
                var objMatch = DB.HRPORequests.FirstOrDefault(w => w.RequestNumber == id);
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
                var Objmatch = DBV.HR_PR_VIEW.FirstOrDefault(w => w.RequestNumber == Header.RequestNumber);

                var excfObject = DB.ExCfWorkFlowItems.Find(ScreenID, Header.DocumentType);
                Humica.Core.SY.SYSendTelegramObject wfo =
                    new Humica.Core.SY.SYSendTelegramObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER, Header.Status);
                wfo.User = User;
                wfo.ListObjectDictionary = new List<object>();
                wfo.ListObjectDictionary.Add(Objmatch);
                HRStaffProfile Staff = getNextApprover(Header.RequestNumber, "");
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
                log.ScreenId = Header.RequestNumber;
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
                log.DocurmentAction = Header.RequestNumber;
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
                log.DocurmentAction = Header.RequestNumber;
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
                var objMatch = DB.HRPORequests.FirstOrDefault(w => w.RequestNumber == id);
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
                                    && w.DocumentNo == objMatch.RequestNumber && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
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
                var _Objmatch = DBV.HR_PR_VIEW.FirstOrDefault(w => w.RequestNumber == objMatch.RequestNumber);

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
                log.ScreenId = Header.RequestNumber;
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
                log.DocurmentAction = Header.RequestNumber;
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
                log.DocurmentAction = Header.RequestNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CancelPR(string ApprovalID)
        {
            try
            {
                //   HumicaDBContext DB = new HumicaDBContext();
                string cancelled = SYDocumentStatus.CANCELLED.ToString();
                string PONumber = ApprovalID;
                var objmatch = DB.HRPORequests.Find(PONumber);
                if (objmatch.Status != cancelled)
                {
                    objmatch.Status = cancelled;
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                }
                DB.SaveChanges();
                #region *****Send to Telegram
                var _Objmatch = DBV.HR_PR_VIEW.FirstOrDefault(w => w.RequestNumber == objmatch.RequestNumber);

                HRStaffProfile Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objmatch.Requestor);
                var excfObject = DB.ExCfWorkFlowItems.Find(ScreenID, objmatch.DocumentType);
                Humica.Core.SY.SYSendTelegramObject wfo =
                    new Humica.Core.SY.SYSendTelegramObject(excfObject.ApprovalFlow, WorkFlowType.COLLECTOR, objmatch.Status);
                wfo.User = User;
                wfo.ListObjectDictionary = new List<object>();
                wfo.ListObjectDictionary.Add(Staff);
                wfo.ListObjectDictionary.Add(_Objmatch);
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
}