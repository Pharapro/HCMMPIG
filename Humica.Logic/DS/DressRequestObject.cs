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
    public class DressRequestObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public HRDressRequest Header { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public FTINYear FInYear { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<HRDressRequest> ListHeader { get; set; }
        // public List<HRDressRequest> ListHeaderPending { get; set; }
        public List<HRDressRequestItem> ListItem { get; set; }
        public List<HRUniform> ListUniform { get; set; }
        public DressRequestObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        public string CreateDR()
        {
            try
            {
                var objCF = DB.ExCfWorkFlowItems.Find(ScreenId, Header.DocType);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                if (ListItem.Count <= 0)
                {
                    return "Item Can not null";
                }
                var objNumber = new CFNumberRank(Header.DocType, ScreenId);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                //Add item
                Header.DocNo = objNumber.NextNumberRank.Trim();
                Header.Status = SYDocumentStatus.OPEN.ToString();
                foreach (var item in ListItem)
                {
                    var obj = new HRDressRequestItem();
                    obj.DocNo = Header.DocNo;
                    obj.ItemName = item.ItemName;
                    obj.Qty = item.Qty;
                    obj.Description = item.Description;
                    DB.HRDressRequestItems.Add(obj);
                }
                //Add approver
                foreach (var read in ListApproval)
                {

                    read.DocumentNo = Header.DocNo;
                    read.DocumentType = Header.DocType;
                    read.Status = SYDocumentStatus.OPEN.ToString();
                    read.WFObject = objCF.ApprovalFlow;
                    read.ApprovedBy = "";
                    read.ApprovedName = "";
                    read.ApproverName = "";
                    var objStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == read.Approver);
                    if (objStaff != null)
                    {
                        read.ApproverName = objStaff.AllName;
                    }
                    DB.ExDocApprovals.Add(read);
                }
                Header.EmpCode = Header.EmpCode;
                Header.EmpName = Header.EmpName;
                Header.CreatedOn = DateTime.Now;
                Header.RequestDate = DateTime.Now;
                Header.CreatedBy = User.UserName;

                DB.HRDressRequests.Add(Header);
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
                var objMatch = DB.HRDressRequests.FirstOrDefault(w => w.DocNo == id);
                var objCF = DB.ExCfWorkFlowItems.Find(ScreenId, Header.DocType);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                if (objMatch == null)
                {
                    return "Document cann't be null";
                }
                objMatch.VendorName = Header.VendorName;
                objMatch.Branch = Header.Branch;
                objMatch.Description = Header.Description;
                objMatch.RequestDate = Header.RequestDate;
                objMatch.PathFile = Header.PathFile;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                DB.HRDressRequests.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.VendorName).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Branch).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Description).IsModified = true;
                DB.Entry(objMatch).Property(w => w.RequestDate).IsModified = true;
                DB.Entry(objMatch).Property(w => w.PathFile).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                var ListAtt = DB.HRDressRequestItems.Where(w => w.DocNo == id).ToList();
                foreach (var read in ListAtt.ToList())
                {
                    DB.HRDressRequestItems.Remove(read);
                }
                var listApprovalDoc = DB.ExDocApprovals.Where(w => w.DocumentType == objMatch.DocType && w.DocumentNo == objMatch.DocNo).ToList();

                foreach (var read in listApprovalDoc)
                {
                    DB.ExDocApprovals.Attach(read);
                    DB.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                }
                foreach (var read in ListItem.ToList())
                {
                    read.DocNo = objMatch.DocNo;
                    DB.HRDressRequestItems.Add(read);
                }
                foreach (var read in ListApproval)
                {

                    read.DocumentNo = Header.DocNo;
                    read.DocumentType = Header.DocType;
                    read.Status = SYDocumentStatus.OPEN.ToString();
                    read.WFObject = objCF.ApprovalFlow;
                    read.ApprovedBy = "";
                    read.ApprovedName = "";
                    read.ApproverName = "";
                    var objStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == read.Approver);
                    if (objStaff != null)
                    {
                        read.ApproverName = objStaff.AllName;
                    }
                    DB.ExDocApprovals.Add(read);
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
        public string DeleteDR(string DocNo)
        {
            try
            {
                Header = new HRDressRequest();
                var objMatch = DB.HRDressRequests.FirstOrDefault(w => w.DocNo == DocNo);
                var item = DB.HRDressRequestItems.Where(w => w.DocNo == DocNo).ToList();
                var App = DB.ExDocApprovals.Where(w => w.DocumentNo == DocNo).ToList();
                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                Header.DocNo = DocNo;
                foreach (var read in item)
                {
                    DB.HRDressRequestItems.Attach(read);
                    DB.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                }
                foreach (var list in App)
                {
                    DB.ExDocApprovals.Attach(list);
                    DB.Entry(list).State = System.Data.Entity.EntityState.Deleted;
                }
                DB.HRDressRequests.Attach(objMatch);
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
                log.ScreenId = Header.DocNo.ToString();
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
                log.ScreenId = Header.DocNo.ToString();
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
        //public void SetAutoApproval(string SCREEN_ID, string DocType)
        //{
        //    ListApproval = new List<ExDocApproval>();
        //    var DBX = new HumicaDBContext();
        //    var objDoc = DBX.ExCfWorkFlowItems.Find(SCREEN_ID, DocType);
        //    if (objDoc != null)
        //    {
        //        if (objDoc.IsRequiredApproval == true)
        //        {
        //            var listDefaultApproval = DBX.ExCfWFSalaryApprovers.Where(w => w.WFObject == objDoc.ApprovalFlow && w.IsSelected == true).ToList();
        //            foreach (var read in listDefaultApproval)
        //            {
        //                var objApp = new ExDocApproval();
        //                objApp.Approver = read.Employee;
        //                objApp.ApproverName = read.EmployeeName;
        //                objApp.DocumentType = DocType;
        //                objApp.ApproveLevel = read.ApproveLevel;
        //                objApp.WFObject = objDoc.ApprovalFlow;
        //                ListApproval.Add(objApp);
        //            }
        //        }
        //    }
        //}
        public void SetAutoItem()
        {
            ListItem = new List<HRDressRequestItem>();
            var objDoc = DB.HRUniforms.Where(w => w.IsAutoSelected == true);
            foreach (var read in objDoc)
            {
                var objApp = new HRDressRequestItem();
                objApp.ItemName = read.Description;
                objApp.Qty = 1;
                ListItem.Add(objApp);
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
        public string isValidItem(HRDressRequestItem item, EnumActionGridLine Action)
        {
            if (Action == EnumActionGridLine.Add)
            {
                if (ListItem.Where(w => w.ItemName == item.ItemName).ToList().Count > 0)
                {
                    return "DUPLICATED_ITEM";
                }
            }

            return SYConstant.OK;
        }
        public string requestToApprove(string id)
        {
            try
            {
                var objMatch = DB.HRDressRequests.FirstOrDefault(w => w.DocNo == id);
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
                DB.HRDressRequests.Attach(objMatch);
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
                var objMatch = DB.HRDressRequests.FirstOrDefault(w => w.DocNo == id);
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
                DB.HRDressRequests.Attach(objMatch);
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
        public string CancelDR(string ASNumber)
        {
            try
            {

                string cancelled = SYDocumentStatus.CANCELLED.ToString();
                var objmatch = DB.HRDressRequests.FirstOrDefault(w => w.DocNo == ASNumber);
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
                objmatch.ChangedOn = DateTime.Now;
                objmatch.ChangedBy = User.UserName;
                DB.HRDressRequests.Attach(objmatch);
                DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;
                DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                var row = DB.SaveChanges();
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
        public HRStaffProfile getNextApprover(string id, string pro)
        {
            var objStaff = new HRStaffProfile();
            var objHeader = DB.HRDressRequests.FirstOrDefault(w => w.DocNo == id);
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

}