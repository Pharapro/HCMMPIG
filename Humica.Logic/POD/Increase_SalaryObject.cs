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

namespace Humica.Logic.POD
{
    public class Increase_SalaryObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public HRIncreaseSalary Header { get; set; }
        public HRStaffProfile HeaderStaff { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<HRIncreaseSalary> ListHeader { get; set; }
        public List<HRIncreaseSalary> ListHeaderHis { get; set; }
        public List<HRStaffProfile> ListHeaderStaff { get; set; }
        public Increase_SalaryObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        public string CreateAppSalary()
        {
            try
            {
                DB = new HumicaDBContext();
                var objCF = DB.ExCfWorkFlowItems.Find(ScreenId, Header.DocType);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }

                var objNumber = new CFNumberRank(Header.DocType, ScreenId);
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.DocNo = objNumber.NextNumberRank.Trim();
                Header.Status = SYDocumentStatus.OPEN.ToString(); ;
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
                    var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                    if (objStaff != null)
                    {
                        read.ApproverName = objStaff.AllName;
                    }
                    DB.ExDocApprovals.Add(read);
                }
                Header.CreatedOn = DateTime.Now;
                Header.CreatedBy = User.UserName;

                DB.HRIncreaseSalaries.Add(Header);
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
        public string EditIncreaseSal(string id)
        {
            try
            {
                HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = DB.HRIncreaseSalaries.FirstOrDefault(w => w.DocNo == id);
                if (objMatch == null)
                {
                    return "Document cann't be null";
                }
                //if(objMatch.Status  == SYDocumentStatus.APPROVED.ToString())
                //{
                //    return "Cann't Edit this document cuz it has been APPROVED";
                //}
                //if (objMatch.Status == SYDocumentStatus.CANCELLED.ToString())
                //{
                //    return "Cann't Edit this document cuz it has been CANCELLED";
                //}
                objMatch.Increase = Header.Increase;
                objMatch.Description = Header.Description;
                objMatch.Requestor = Header.Requestor;
                objMatch.EffecDate = Header.EffecDate;
                objMatch.DocumentDate = Header.DocumentDate;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                DB.HRIncreaseSalaries.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Increase).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Description).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Requestor).IsModified = true;
                DB.Entry(objMatch).Property(w => w.EffecDate).IsModified = true;
                DB.Entry(objMatch).Property(w => w.DocumentDate).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                //var ListAtt = DB.HRApproverIncSalaries.Where(w => w.DocumentNo == Header.DocNo).ToList();
                //foreach (var item in ListAtt.Where(w => w.Status != SYDocumentStatus.APPROVED.ToString()).ToList())
                //{
                //    item.ApproverName =;
                //    item. = ;
                //    DB.HRApproverIncSalaries.Attach(attFirst);
                //    DB.Entry(item).Property(w => w.).IsModified = true;
                //    DB.Entry(item).Property(w => w.).IsModified = true;
                //}
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
        public void SetAutoApproval(string SCREEN_ID, string DocType)
        {
            ListApproval = new List<ExDocApproval>();
            var DBX = new HumicaDBContext();
            var objDoc = DBX.ExCfWorkFlowItems.Find(SCREEN_ID, DocType);
            if (objDoc != null)
            {
                if (objDoc.IsRequiredApproval == true)
                {
                    var listDefaultApproval = DBX.ExCfWFSalaryApprovers.Where(w => w.WFObject == objDoc.ApprovalFlow && w.IsSelected == true).ToList();
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
        public string requestToApprove(string id)
        {
            try
            {
                HumicaDBContext DBX = new HumicaDBContext();
                var objMatch = DB.HRIncreaseSalaries.FirstOrDefault(w => w.DocNo == id);
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
                DBX.HRIncreaseSalaries.Attach(objMatch);
                DBX.Entry(objMatch).Property(w => w.Status).IsModified = true;

                int row = DBX.SaveChanges();
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
                DB = new HumicaDBContext();
                var objMatch = DB.HRIncreaseSalaries.FirstOrDefault(w => w.DocNo == id);
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
                                    && w.DocumentNo == objMatch.DocNo && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
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
                            ////New
                            //if (listApproval.Where(w => w.ApproveLevel <= read.ApproveLevel).Count() >= listApproval.Count())
                            //{
                            //    listApproval.ForEach(w => w.Status = SYDocumentStatus.APPROVED.ToString());
                            //}
                            //StaffProfile = objStaff;
                            read.Approver = objStaff.EmpCode;
                            read.ApproverName = objStaff.AllName;
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

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;

                int row = DB.SaveChanges();
                var _staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objMatch.Requestor);
                objMatch.Requestor = _staff.AllName;
                #region Send Email
                var excfObject = DB.ExCfWorkFlowItems.Find(ScreenId, objMatch.DocType);
                if (excfObject != null)
                {
                    SYWorkFlowEmailObject wfo =
                           new SYWorkFlowEmailObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER,
                                UserType.N, SYDocumentStatus.PENDING.ToString());
                    wfo.SelectListItem = new SYSplitItem(Convert.ToString(id));
                    wfo.User = User;
                    wfo.BS = BS;
                    // wfo.UrlView = URL;
                    wfo.ScreenId = ScreenId;
                    wfo.Module = "HR";// CModule.PURCHASE.ToString();
                    wfo.ListLineRef = new List<BSWorkAssign>();
                    wfo.Action = SYDocumentStatus.PENDING.ToString();
                    wfo.ObjectDictionary = Header;
                    wfo.ListObjectDictionary = new List<object>();
                    wfo.ListObjectDictionary.Add(objMatch);
                    HRStaffProfile Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == User.UserName);
                    wfo.ListObjectDictionary.Add(Staff);
                    WorkFlowResult result1 = wfo.InsertProcessWorkFlow(Staff);
                    MessageCode = wfo.getErrorMessage(result1);
                }
                #endregion
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
        public string CancelAppIncSalary(string ASNumber)
        {
            try
            {
                string approved = SYDocumentStatus.CANCELLED.ToString();
                var objmatch = DB.HRIncreaseSalaries.FirstOrDefault(w => w.DocNo == ASNumber);
                var _obj = DB.ExDocApprovals.Where(x => x.DocumentNo == ASNumber);
                if (objmatch == null)
                {
                    return "INV_EN";
                }

                foreach (var read in _obj)
                {
                    read.Status = approved;
                    DB.Entry(read).Property(w => w.Status).IsModified = true;
                    DB.SaveChanges();
                }

                objmatch.Status = approved;
                objmatch.ChangedOn = DateTime.Now;
                objmatch.ChangedBy = User.UserName;
                DB.HRIncreaseSalaries.Attach(objmatch);
                DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;
                DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
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
        public HRStaffProfile getNextApprover(string id, string pro)
        {
            var objStaff = new HRStaffProfile();
            var DBI = new HumicaDBContext();
            var objHeader = DBI.HRIncreaseSalaries.FirstOrDefault(w => w.DocNo == id);
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