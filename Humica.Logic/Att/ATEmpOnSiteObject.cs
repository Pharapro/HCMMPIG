using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.Helper;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Integration.EF.Models;
using Humica.Logic.PR;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;

namespace Humica.Logic.Att
{
    public class ATEmpOnSiteObject
	{
        public HumicaDBContext DB = new HumicaDBContext();
        public SMSystemEntity DP = new SMSystemEntity();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public ATRequestOnSite HeaderEmpOnSite { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public FTFilterData Filter { get; set; }
        public FTINYear FInYear { get; set; }
        public List<ATRequestOnSite> ListEmpOnSite { get; set; }
		public List<ATRequestOnSite> ListEmpOnSitePending { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<HR_STAFF_VIEW> ListStaff { get; set; }
        public string EmpID { get; set; }
        public string Units { get; set; }
        public ATEmpOnSiteObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<ATRequestOnSite> LoadDataLeave(FTINYear Filter, List<HRBranch> _lstBranch)
        {
            var staff = DB.HRStaffProfiles.ToList();
            if (staff.Where(w => w.EmpCode == User.UserName).ToList().Count > 0)
            {
                string approved = SYDocumentStatus.APPROVED.ToString();
                string Reject = SYDocumentStatus.REJECTED.ToString();
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                string Pending = SYDocumentStatus.PENDING.ToString();
                var ListEmpReqOnSite = DB.ATRequestOnSites.Where(x => x.Status != approved && x.Status != Reject && x.Status != Cancel).ToList();
                var ListApp = DB.ExDocApprovals.AsEnumerable().Where(w => ListEmpReqOnSite.Where(x => x.ID.ToString() == w.DocumentNo && w.Approver == User.UserName).Any()).ToList();
                var ListEmpReqOnSiteInYear = DBV.ATRequestOnSites.Where(w => w.FromDate.Year == Filter.INYear).ToList();
                foreach (var read in ListApp)
                {
                    var objemp = new ATRequestOnSite();
                    if (read.ApproveLevel > 1 && read.Status == Open)
                    {
                        var level = DB.ExDocApprovals.Where(w => w.DocumentNo == read.DocumentNo && w.ApproveLevel < read.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
                        if (level != null) continue;
                        var EmpOnSite = ListEmpReqOnSite.FirstOrDefault(w => w.ID.ToString() == read.DocumentNo);
                        //var EmpStaff = ListEmpLeaveReq_.FirstOrDefault(w => w.EmpCode == EmpLeave.EmpCode);
                        //var leavetype = DB.HRLeaveTypes.FirstOrDefault(w => w.Code == EmpLeave.LeaveCode);
                        objemp.EmpCode = EmpOnSite.EmpCode;
                        objemp.EmployeeName = EmpOnSite.EmployeeName;
						//objemp.AllName = EmpStaff.EmployeeName;
						//objemp.Department = EmpStaff.Department;
						//objemp.Position = EmpStaff.Position;
                        // objemp.Level = EmpStaff.Level;
                        //objemp.TranNo = EmpLeave.TranNo;
                        //objemp.LeaveCode = EmpLeave.LeaveCode;
                        //objemp.LeaveType = leavetype.Description;
                        objemp.FromDate = EmpOnSite.FromDate;
                        objemp.ToDate = EmpOnSite.ToDate;
                        //objemp.NoDay = EmpLeave.NoDay;
                        //objemp.NoPH = EmpLeave.NoPH;
                        //objemp.NoRest = EmpLeave.NoRest;
                        objemp.Reason = EmpOnSite.Reason;
                        objemp.Status = EmpOnSite.Status;
						//objemp.RequestDate = EmpLeave.RequestDate;
						//objemp.Units = EmpLeave.Units;
						ListEmpOnSite.Add(objemp);
                    }
                    else if (read.ApproveLevel == 1 && read.Status == Open)
                    {
                        var EmpOnSite = ListEmpReqOnSite.FirstOrDefault(w => w.ID.ToString() == read.DocumentNo);
						//var EmpStaff = ListEmpLeaveReq_.FirstOrDefault(w => w.EmpCode == EmpLeave.EmpCode);
						//var leavetype = DB.HRLeaveTypes.FirstOrDefault(w => w.Code == EmpLeave.LeaveCode);
						objemp.EmpCode = EmpOnSite.EmpCode;
						objemp.EmployeeName = EmpOnSite.EmployeeName;
						//objemp.AllName = EmpStaff.EmployeeName;
						//objemp.Department = EmpStaff.Department;
						//objemp.Position = EmpStaff.Position;
						// objemp.Level = EmpStaff.Level;
						//objemp.TranNo = EmpLeave.TranNo;
						//objemp.LeaveCode = EmpLeave.LeaveCode;
						//objemp.LeaveType = leavetype.Description;
						objemp.FromDate = EmpOnSite.FromDate;
						objemp.ToDate = EmpOnSite.ToDate;
						//objemp.NoDay = EmpLeave.NoDay;
						//objemp.NoPH = EmpLeave.NoPH;
						//objemp.NoRest = EmpLeave.NoRest;
						objemp.Reason = EmpOnSite.Reason;
						objemp.Status = EmpOnSite.Status;
						//objemp.RequestDate = EmpLeave.RequestDate;
						//objemp.Units = EmpLeave.Units;
						ListEmpOnSite.Add(objemp);
					}
                }
            }
            return ListEmpOnSite.OrderBy(w => w.EmpCode).ToList();
        }
        public string approveTheDoc(string id, string URL, string fileName)
        {
            try
            {
                DB = new HumicaDBContext();
                int TranNo = Convert.ToInt32(id);
                var objMatch = DB.HREmpLeaves.Find(TranNo);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                var _LeaveType = DB.HRLeaveTypes.Find(objMatch.LeaveCode);
                // Header = objMatch;
                //if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                //{
                //   return "INV_DOC";
                //}
                string open = SYDocumentStatus.OPEN.ToString();
                string DocNo = objMatch.Increment.ToString();
                var listApproval = DB.ExDocApprovals.Where(w => w.DocumentType == "LR"
                                    && w.DocumentNo == DocNo && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                var b = false;
                var approverLevel = 0;
                if (listApproval.Count == 0)
                {
                    return "RESTRICT_ACCESS";
                }
                foreach (var read in listApproval)
                {
                    approverLevel = read.ApproveLevel;
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
                            //New
                            //if (listApproval.Where(w => w.ApproveLevel <= read.ApproveLevel).Count() >= listApproval.Count())
                            //{
                            //    listApproval.ForEach(w => w.Status = SYDocumentStatus.APPROVED.ToString());
                            //}
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
                }

                objMatch.Status = status;
                DB.HREmpLeaves.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;

                //Update Leave in Att
                var ListAtt = DB.ATEmpSchedules.Where(w => w.EmpCode == objMatch.EmpCode
                && EntityFunctions.TruncateTime(w.TranDate) >= objMatch.FromDate.Date
                && EntityFunctions.TruncateTime(w.TranDate) <= objMatch.ToDate.Date).ToList();
                foreach (var item in ListAtt)
                {
                    item.LeaveDesc = "";
                    item.LeaveCode = objMatch.LeaveCode;
                    item.LeaveNo = objMatch.Increment;
                    DB.ATEmpSchedules.Attach(item);
                    DB.Entry(item).Property(w => w.LeaveDesc).IsModified = true;
                    DB.Entry(item).Property(w => w.LeaveCode).IsModified = true;
                    DB.Entry(item).Property(w => w.LeaveNo).IsModified = true;
                }

                SYHRAnnouncement _announ = new SYHRAnnouncement();
                var _Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                var _Lstapp = listApproval.Where(w => w.Status == SYDocumentStatus.OPEN.ToString()).ToList();
                _announ.Type = "LeaveRequest";
                if (_Lstapp.Count() > 0)
                {
                    var min = _Lstapp.Min(w => w.ApproveLevel);
                    _announ.Subject = _Staff.AllName;
                    _announ.UserName = _Lstapp.FirstOrDefault(w => w.ApproveLevel == min).Approver;
                    _announ.Description = @"Leave type of " + _LeaveType.Description +
                        " from " + objMatch.FromDate.ToString("yyyy.MM.dd") + " to " + objMatch.ToDate.ToString("yyyy.MM.dd") + " My Reason: " + objMatch.Reason;
                }
                if (status == SYDocumentStatus.APPROVED.ToString())
                {
                    _announ.Type = "LeaveApproved";
                    _announ.Subject = "Approved";
                    _announ.UserName = objMatch.EmpCode;
                    _announ.Description = "Leave type of " + _LeaveType.Description;
                }
                if (!string.IsNullOrEmpty(_announ.Description))
                {
                    _announ.DocumentNo = objMatch.Increment.ToString();
                    _announ.DocumentDate = DateTime.Now;
                    _announ.IsRead = false;
                    _announ.CreatedBy = User.UserName;
                    _announ.CreatedOn = DateTime.Now;
                    DB.SYHRAnnouncements.Add(_announ);
                }

                string Approval = SYDocumentStatus.APPROVED.ToString();
                List<HRClaimLeave> _listClaim = DB.HRClaimLeaves.Where(w => w.EmpCode == objMatch.EmpCode
                && (w.IsExpired.Value != true || w.IsUsed.Value != true) && w.Status == Approval).ToList();
                DateTime DateNow = DateTime.Now;
                bool Isused = false;
                foreach (var claim in _listClaim.ToList().OrderBy(w => w.WorkingDate))
                {
                    if (Isused == true) continue;
                    if (claim.Expired.Value.Date < DateNow.Date)
                    {
                        claim.IsExpired = true;
                        DB.HRClaimLeaves.Attach(claim);
                        DB.Entry(claim).Property(x => x.IsExpired).IsModified = true;
                    }
                    else
                    {
                        Isused = true;
                        claim.IsUsed = true;
                        claim.DocumentRef = objMatch.Increment.ToString();
                        DB.HRClaimLeaves.Attach(claim);
                        DB.Entry(claim).Property(x => x.IsUsed).IsModified = true;
                        DB.Entry(claim).Property(x => x.DocumentRef).IsModified = true;
                    }
                }

                int row = DB.SaveChanges();
                DBV = new HumicaDBViewContext();
                //var listApproval = DB.ExDocApprovals.Where(w => w.DocumentType == objMatch.DocumentType
                //                   && w.DocumentNo == objMatch.TranNo && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                if (objMatch.Status == SYDocumentStatus.APPROVED.ToString())
                {
                    //ReGenerateLeaveToken(objMatch.EmpCode, objMatch.FromDate, objMatch.ToDate);
                    #region Send Email
                    HR_VIEW_EmpLeave EmpLeave = new HR_VIEW_EmpLeave();
                    EmpLeave = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.TranNo == objMatch.TranNo);
                    SYWorkFlowEmailObject wfo =
                               new SYWorkFlowEmailObject("ESSLA", WorkFlowType.REQUESTER,
                                    UserType.N, SYDocumentStatus.PENDING.ToString());
                    wfo.SelectListItem = new SYSplitItem(Convert.ToString(DocNo));
                    wfo.User = User;
                    wfo.BS = BS;
                    wfo.UrlView = URL;
                    wfo.ScreenId = ScreenId;
                    wfo.Module = "HR";// CModule.PURCHASE.ToString();
                    wfo.DocNo = objMatch.TranNo.ToString();
                    wfo.ListLineRef = new List<BSWorkAssign>();
                    wfo.Action = SYDocumentStatus.PENDING.ToString();
                    //wfo.ObjectDictionary = HeaderEmpLeave;
                    wfo.ListObjectDictionary = new List<object>();
                    wfo.ListObjectDictionary.Add(EmpLeave);
                    HRStaffProfile Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpLeave.EmpCode);
                    if (Staff.Email != null && Staff.Email != "")
                    {
                        wfo.ListObjectDictionary.Add(Staff);
                        WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Staff);
                        MessageError = wfo.getErrorMessage(result1);
                    }
                    #endregion

                    #region *****Send to Telegram
                    if (Staff.TeleGroup != null && Staff.TeleGroup != "")
                    {
                        Humica.Core.SY.SYSendTelegramObject wfo1 =
                          new Humica.Core.SY.SYSendTelegramObject("ESSLA", WorkFlowType.APPROVER, objMatch.Status);
                        wfo1.User = User;
                        wfo1.ListObjectDictionary = new List<object>();
                        wfo1.ListObjectDictionary.Add(EmpLeave);
                        wfo1.ListObjectDictionary.Add(Staff);
                        wfo1.Send_SMS_Telegram(Staff.TeleGroup, "");
                    }
                    #endregion
                    //Alert to HR

                    var EmailTemplateCC = DP.TPEmailTemplates.Find("ESSLEAVE_TELECC_HR");
                    var Sett = DB.SYHRSettings.First();
                    if (EmailTemplateCC != null && !string.IsNullOrEmpty(Sett.TelegLeave))
                    {
                        SYSendTelegramObject Tel = new SYSendTelegramObject();
                        Tel.User = User;
                        Tel.BS = BS;
                        List<object> ListObjectDictionary = new List<object>();
                        ListObjectDictionary.Add(EmpLeave);
                        ListObjectDictionary.Add(Staff);
                        WorkFlowResult result2 = Tel.Send_SMS_Telegram("ESSLEAVE_TELECC_HR", EmailTemplateCC.RequestContent, Sett.TelegLeave, ListObjectDictionary, URL);
                        MessageError = Tel.getErrorMessage(result2);
                    }
                    //Alert to HOD
                    var EmailTemplate_HOD = DP.TPEmailTemplates.Find("ESSLEAVE_TELECC_HOD");
                    if (EmailTemplate_HOD != null)
                    {
                        var HOD = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == Staff.HODCode);
                        if (HOD != null && !string.IsNullOrEmpty(HOD.TeleChartID))
                        {
                            SYSendTelegramObject Tel = new SYSendTelegramObject();
                            Tel.User = User;
                            Tel.BS = BS;
                            List<object> ListObjectDictionary = new List<object>();
                            ListObjectDictionary.Add(EmpLeave);
                            ListObjectDictionary.Add(Staff);
                            ListObjectDictionary.Add(HOD);
                            WorkFlowResult result2 = Tel.Send_SMS_Telegram(EmailTemplate_HOD.EMTemplateObject, EmailTemplate_HOD.RequestContent, HOD.TeleChartID, ListObjectDictionary, URL);
                            MessageError = Tel.getErrorMessage(result2);
                        }
                    }
                    //Alert to Requester 
                    var Template_Req = DP.TPEmailTemplates.Find("TAPPLEAVE");
                    if (Template_Req != null)
                    {
                        if (!string.IsNullOrEmpty(Staff.TeleChartID))
                        {
                            SYSendTelegramObject Tel = new SYSendTelegramObject();
                            Tel.User = User;
                            Tel.BS = BS;
                            List<object> ListObjectDictionary = new List<object>();
                            ListObjectDictionary.Add(EmpLeave);
                            ListObjectDictionary.Add(Staff);
                            WorkFlowResult result2 = Tel.Send_SMS_Telegram(Template_Req.EMTemplateObject, Template_Req.RequestContent, Staff.TeleChartID, ListObjectDictionary, URL);
                            MessageError = Tel.getErrorMessage(result2);
                        }
                    }

                    #region Notifican on Mobile
                    var access = DB.TokenResources.FirstOrDefault(w => w.UserName == _Staff.EmpCode);
                    if (access != null)
                    {
                        if (!string.IsNullOrEmpty(access.FirebaseID))
                        {
                            string Desc = _announ.Description;
                            Notification.Notificationf Noti = new Notification.Notificationf();
                            var clientToken = new List<string>();
                            clientToken.Add(access.FirebaseID);
                            //clientToken.Add("d7Xt0qR7JkfnnLKGf4xCw2:APA91bHfJMAlQRQlZDwDqG9h8FQfbf8lEijFo4zlzI1i17tEVhZVT7lzTAy3q7ePb7vbgok5bxJWQjdSgBM37NKkSQ_mYnsQInV7ZmRHyVOmM6xektGYp0e8AhGSulzpZZnhvuR19v32");
                            var dd = Noti.SendNotification(clientToken, "LeaveApproved", Desc);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Send Email
                    HR_VIEW_EmpLeave EmpLeave = new HR_VIEW_EmpLeave();
                    EmpLeave = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.TranNo == objMatch.TranNo);
                    SYWorkFlowEmailObject wfo =
                               new SYWorkFlowEmailObject("ESSLR", WorkFlowType.REQUESTER,
                                    UserType.N, SYDocumentStatus.PENDING.ToString());
                    wfo.SelectListItem = new SYSplitItem(Convert.ToString(DocNo));
                    wfo.User = User;
                    wfo.BS = BS;
                    wfo.UrlView = URL;
                    wfo.ScreenId = ScreenId;
                    wfo.Module = "HR";// CModule.PURCHASE.ToString();
                    wfo.ListLineRef = new List<BSWorkAssign>();
                    wfo.Action = SYDocumentStatus.PENDING.ToString();
                    //wfo.ObjectDictionary = HeaderEmpLeave;
                    wfo.ListObjectDictionary = new List<object>();
                    wfo.ListObjectDictionary.Add(EmpLeave);
                    //HRStaffProfile Staff = getNextApprover(DocNo, "");
                    //if (!string.IsNullOrEmpty(Staff.Email))
                    //{
                    //    wfo.ListObjectDictionary.Add(Staff);
                    //    WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Staff);
                    //    MessageError = wfo.getErrorMessage(result1);
                    //}
                    #endregion

                }
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
        public string ApproveEmpOnSite(string ApprovalID, string URL)
        {
            HumicaDBContext DBI = new HumicaDBContext();
            try
            {
                DBI.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    string[] c = ApprovalID.Split(';');
                    foreach (var r in c)
                    {
                        if (r == "") continue;
                        //var ListStaff = DBI.HRStaffProfiles.ToList();
                        string approved = SYDocumentStatus.APPROVED.ToString();
                        int TranNo = Convert.ToInt32(r);
                        var objmatch = DBI.ATRequestOnSites.Find(TranNo);
                        //var ObjLeaveB = DBI.HREmpLeaveBs;
                        if (objmatch == null)
                            return "INV_EN";
                        objmatch.Status = approved;
                        objmatch.ChangedBy = User.UserName;
                        objmatch.ChangedOn = DateTime.Now;
                        DBI.ATRequestOnSites.Attach(objmatch);
                        //DBI.Entry(objmatch).Property(w => w.RejectDate).IsModified = true;
                        DBI.Entry(objmatch).Property(w => w.Status).IsModified = true;
                        DBI.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                        DBI.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;
                        //Update Leave in Att
                        var ListAtt = DB.ATEmpSchedules.Where(w => w.EmpCode == objmatch.EmpCode
                                                        && EntityFunctions.TruncateTime(w.TranDate) >= objmatch.FromDate.Date
                                                        && EntityFunctions.TruncateTime(w.TranDate) <= objmatch.ToDate.Date).ToList();
                        foreach (var item in ListAtt)
                        {
                            item.LeaveDesc = "";
                            //item.LeaveCode = objmatch.LeaveCode;
                            item.LeaveNo = objmatch.ID;
                            DBI.ATEmpSchedules.Attach(item);
                            DBI.Entry(item).Property(w => w.LeaveDesc).IsModified = true;
                            DBI.Entry(item).Property(w => w.LeaveCode).IsModified = true;
                            DBI.Entry(item).Property(w => w.LeaveNo).IsModified = true;
                        }
                        //HRApproverLeave approver = new HRApproverLeave();
                        //approver.TranNo = TranNo;
                        //approver.BranchID = User.CompanyOwner;
                        //approver.Division = "";
                        //approver.Department = "";
                        //approver.EmpCode = User.UserName;
                        //approver.EmpName = User.LoginName;
                        //approver.Status = SYDocumentStatus.APPROVED.ToString();
                        //approver.Comment = ApprovalWorkFlow.Comment;
                        //DB.HRApproverLeaves.Add(approver);
                        string Approval = SYDocumentStatus.APPROVED.ToString();
                        List<HRClaimLeave> _listClaim = DBI.HRClaimLeaves.Where(w => w.EmpCode == objmatch.EmpCode
                        && (w.IsExpired.Value != true || w.IsUsed.Value != true) && w.Status == Approval).ToList();
                        DateTime DateNow = DateTime.Now;
                        bool Isused = false;
                        foreach (var claim in _listClaim.ToList().OrderBy(w => w.WorkingDate))
                        {
                            if (Isused == true) continue;
                            if (claim.Expired.Value.Date < DateNow.Date)
                            {
                                claim.IsExpired = true;
                                DBI.HRClaimLeaves.Attach(claim);
                                DBI.Entry(claim).Property(x => x.IsExpired).IsModified = true;
                            }
                            else
                            {
                                Isused = true;
                                claim.IsUsed = true;
                                claim.DocumentRef = HeaderEmpOnSite.ID.ToString();
                                DBI.HRClaimLeaves.Attach(claim);
                                DBI.Entry(claim).Property(x => x.IsUsed).IsModified = true;
                                DBI.Entry(claim).Property(x => x.DocumentRef).IsModified = true;
                            }
                        }

                        DBI.SaveChanges();
                        //ReGenerateLeaveToken(objmatch.EmpCode, objmatch.FromDate, objmatch.ToDate);
                        //string DocNo = objmatch.ID.ToString();
                        //#region Send Email
                        //DBV = new HumicaDBViewContext();
                        //HR_VIEW_EmpLeave EmpLeave = new HR_VIEW_EmpLeave();
                        //EmpLeave = DBV.HR_VIEW_EmpLeave.FirstOrDefault(w => w.TranNo == objmatch.TranNo);
                        //SYWorkFlowEmailObject wfo =
                        //           new SYWorkFlowEmailObject("ESSLA", WorkFlowType.REQUESTER,
                        //                UserType.N, SYDocumentStatus.PENDING.ToString());
                        //wfo.SelectListItem = new SYSplitItem(Convert.ToString(DocNo));
                        //wfo.User = User;
                        //wfo.BS = BS;
                        //wfo.UrlView = URL;
                        //wfo.ScreenId = ScreenId;
                        //wfo.Module = "HR";// CModule.PURCHASE.ToString();
                        //wfo.ListLineRef = new List<BSWorkAssign>();
                        //wfo.Action = SYDocumentStatus.PENDING.ToString();
                        //wfo.ObjectDictionary = HeaderEmpLeave;
                        //wfo.ListObjectDictionary = new List<object>();
                        //wfo.ListObjectDictionary.Add(EmpLeave);
                        //HRStaffProfile Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpLeave.EmpCode);
                        //wfo.ListObjectDictionary.Add(Staff);
                        //WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Staff);
                        //MessageError = wfo.getErrorMessage(result1);
                        ////str += "<br /><br /> <a href=" + URL + ">" + URL + "</a>";
                        ////      str += "<p style='color:#C4C3C3;padding-top:20px;'><This email was generated by system, please do not reply.></p>";
                        //#endregion

                        //#region *****Send to Telegram
                        //Humica.Core.SY.SYSendTelegramObject wfo1 =
                        //    new Humica.Core.SY.SYSendTelegramObject("ESSLA", WorkFlowType.APPROVER, objmatch.Status);
                        //wfo1.User = User;
                        //wfo1.ListObjectDictionary = new List<object>();
                        //wfo1.ListObjectDictionary.Add(EmpLeave);
                        //wfo1.ListObjectDictionary.Add(Staff);
                        //wfo1.Send_SMS_Telegram(Staff.TeleGroup, "");
                        //#endregion
                    }
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
            finally
            {
                DBI.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string RejectEmpOnSite(string ApprovalID)
        {
            try
            {
                HumicaDBContext DBI = new HumicaDBContext();
                string[] c = ApprovalID.Split(';');
                foreach (var r in c)
                {
                    if (r == "") continue;
                    int TranNo = Convert.ToInt32(r);
                    string Reject = SYDocumentStatus.REJECTED.ToString();
                    ATRequestOnSite objmatch = DB.ATRequestOnSites.First(w => w.ID == TranNo);
                    if (objmatch == null)
                    {
                        return "INV_EN";
                    }
                    //var _obj = DB.ExDocApprovals.Where(x => x.DocumentNo == objmatch.Increment.ToString());
                    //foreach (var read in _obj)
                    //{
                    //    read.Status = Reject;
                    //    read.LastChangedDate = DateTime.Now;
                    //    DB.Entry(read).Property(w => w.Status).IsModified = true;
                    //    DB.Entry(read).Property(w => w.LastChangedDate).IsModified = true;
                    //}
                    //objmatch.RejectDate = DateTime.Now;
                    objmatch.Status = Reject;
                    objmatch.ChangedBy = User.UserName;
                    objmatch.ChangedOn = DateTime.Now;
                    DB.ATRequestOnSites.Attach(objmatch);
                    //DB.Entry(objmatch).Property(w => w.RejectDate).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;


                    var Staff = DB.HRStaffProfiles.FirstOrDefault(x => x.EmpCode == objmatch.EmpCode);
                    //HRApproverLeave approver = new HRApproverLeave();
                    //approver.TranNo = TranNo;
                    //approver.BranchID = Staff.Branch;
                    //approver.Division = Staff.Division;
                    //approver.Department = Staff.DEPT;
                    //approver.EmpCode = Staff.EmpCode;
                    //approver.EmpName = Staff.AllName;
                    //approver.Status = Reject;
                    //DB.HRApproverLeaves.Add(approver);

                    DB.SaveChanges();

                    //#region *****Send to Telegram
                    //Humica.Core.SY.SYSendTelegramObject wfo =
                    //    new Humica.Core.SY.SYSendTelegramObject("ESSLA", WorkFlowType.REJECTOR, objmatch.Status);
                    //wfo.User = User;
                    //wfo.ListObjectDictionary = new List<object>();
                    //wfo.ListObjectDictionary.Add(objmatch);
                    //wfo.Send_SMS_Telegram(Staff.TeleGroup, "");
                    //#endregion
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ApprovalID;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

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
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

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
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CancelEmpOnSite(string ApprovalID)
        {
            try
            {
                HumicaDBContext DBI = new HumicaDBContext();
                string[] c = ApprovalID.Split(';');
                foreach (var r in c)
                {
                    if (r == "") continue;
                    int TranNo = Convert.ToInt32(r);
                    string Cancell = SYDocumentStatus.CANCELLED.ToString();
					ATRequestOnSite objmatch = DB.ATRequestOnSites.First(w => w.ID == TranNo);
                    if (objmatch == null)
                    {
                        return "INV_EN";
                    }
                    //var _obj = DB.ExDocApprovals.Where(x => x.DocumentNo == objmatch.Increment.ToString());
                    //foreach (var read in _obj)
                    //{
                    //    read.Status = Reject;
                    //    read.LastChangedDate = DateTime.Now;
                    //    DB.Entry(read).Property(w => w.Status).IsModified = true;
                    //    DB.Entry(read).Property(w => w.LastChangedDate).IsModified = true;
                    //}
                    //objmatch.RejectDate = DateTime.Now;
                    objmatch.Status = Cancell;
                    objmatch.ChangedBy = User.UserName;
                    objmatch.ChangedOn = DateTime.Now;
                    DB.ATRequestOnSites.Attach(objmatch);
                    //DB.Entry(objmatch).Property(w => w.RejectDate).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                    DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;

                    var Staff = DB.HRStaffProfiles.FirstOrDefault(x => x.EmpCode == objmatch.EmpCode);
                    //var _ListApp = DB.HRApproverLeaves.Where(w => w.TranNo == TranNo && w.EmpCode == User.UserName).ToList();
                    //if (_ListApp.Count > 0)
                    //{
                    //    var approver = _ListApp.First();
                    //    approver.Status = Reject;
                    //    DB.HRApproverLeaves.Attach(approver);
                    //    DB.Entry(approver).Property(w => w.Status).IsModified = true;
                    //}
                    //else
                    //{
                    //    HRApproverLeave approver = new HRApproverLeave();
                    //    approver.TranNo = TranNo;
                    //    approver.BranchID = Staff.Branch;
                    //    approver.Division = Staff.Division;
                    //    approver.Department = Staff.DEPT;
                    //    approver.EmpCode = Staff.EmpCode;
                    //    approver.EmpName = Staff.AllName;
                    //    approver.Status = Reject;
                    //    DB.HRApproverLeaves.Add(approver);
                    //}
                    DB.SaveChanges();

                    //#region *****Send to Telegram
                    //if (!string.IsNullOrEmpty(Staff.TeleGroup))
                    //{
                    //    Humica.Core.SY.SYSendTelegramObject wfo =
                    //    new Humica.Core.SY.SYSendTelegramObject("ESSLA", WorkFlowType.COLLECTOR, objmatch.Status);
                    //    wfo.User = User;
                    //    wfo.ListObjectDictionary = new List<object>();
                    //    wfo.ListObjectDictionary.Add(objmatch);
                    //    wfo.Send_SMS_Telegram(Staff.TeleGroup, "");
                    //}
                    //#endregion
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ApprovalID;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

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
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

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
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}