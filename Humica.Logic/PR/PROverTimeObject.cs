using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.Helper;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Humica.Logic.PR
{
    public class PROverTimeObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SMSystemEntity SMS = new SMSystemEntity();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public string DocType { get; set; }
        public HRRequestOT HeaderOT { get; set; }
        public PREmpOverTime Header { get; set; }
        public List<PREmpOverTime> ListHeader { get; set; }
        public List<HRRequestOT> ListHeaderOT { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<DayInMonth> ListDayInMonth { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public FTINYear Filter { get; set; }
        public FTFilterEmployee _Filter { get; set; }
        public List<HRRequestOT> ListOTRequest { get; set; }
        public List<ClsReuestOT> ListOTReqPending { get; set; }
        public List<_ListStaff> ListEmpStaff { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public string EmpCode { get; set; }
        public DateTime InDate { get; set; }
        public PROverTimeObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        #region "Request OT"
        public void SetAutoApproval(string EmpCode,DateTime DoDate)
        {
            ListApproval = new List<ExDocApproval>();
            var LstStaff = DB.HRStaffProfiles.Where(w => w.Status == "A" || (w.Status == "I" && w.DateTerminate >= DoDate));
            HRStaffProfile _StaffReq = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode);
            int line = 0;
            if (LstStaff.Where(w => w.EmpCode == _StaffReq.OTFirstLine).Any())
            {
                var _OTF = LstStaff.FirstOrDefault(w => w.EmpCode == _StaffReq.OTFirstLine);
                line += 1;
                var DocApproval = new ExDocApproval();
                DocApproval.Approver = _OTF.EmpCode;
                DocApproval.ApproverName = _OTF.AllName;
                DocApproval.ApprovedBy = "";
                DocApproval.ApprovedName = "";
                DocApproval.ApproveLevel = line;
                DocApproval.DocumentType = "REQ_OT";
                DocApproval.WFObject = "WF02";
                ListApproval.Add(DocApproval);
            }
            if (LstStaff.Where(w => w.EmpCode == _StaffReq.OTSecondLine).Any())
            {
                var _OTF = LstStaff.FirstOrDefault(w => w.EmpCode == _StaffReq.OTSecondLine);
                line += 1;
                var DocApproval = new ExDocApproval();
                DocApproval.Approver = _OTF.EmpCode;
                DocApproval.ApproverName = _OTF.AllName;
                DocApproval.ApprovedBy = "";
                DocApproval.ApprovedName = "";
                DocApproval.ApproveLevel = line;
                DocApproval.DocumentType = "REQ_OT";
                DocApproval.WFObject = "WF02";
                ListApproval.Add(DocApproval);
            }
            if (LstStaff.Where(w => w.EmpCode == _StaffReq.OTthirdLine).Any())
            {
                var _OTF = LstStaff.FirstOrDefault(w => w.EmpCode == _StaffReq.OTthirdLine);
                line += 1;
                var DocApproval = new ExDocApproval();
                DocApproval.Approver = _OTF.EmpCode;
                DocApproval.ApproverName = _OTF.AllName;
                DocApproval.ApprovedBy = "";
                DocApproval.ApprovedName = "";
                DocApproval.ApproveLevel = line;
                DocApproval.DocumentType = "REQ_OT";
                DocApproval.WFObject = "WF02";
                ListApproval.Add(DocApproval);
            }
        }
        public HRStaffProfile getNextApprover(string id)
        {
            var objStaff = new HRStaffProfile();
            var DBX = new HumicaDBContext();
            //var objHeader = DBX.HRReqPayments.Find(id);
            //if (objHeader == null)
            //{
            //    return new HRStaffProfile();
            //}
            string open = SYDocumentStatus.OPEN.ToString();
            var listCanApproval = DBX.ExDocApprovals.Where(w => w.DocumentNo == id && w.Status == open && w.DocumentType == "REQ_OT").ToList();

            if (listCanApproval.Count == 0)
            {
                return new HRStaffProfile();
            }

            var min = listCanApproval.Min(w => w.ApproveLevel);
            var NextApp = listCanApproval.Where(w => w.ApproveLevel == min).First();
            objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == NextApp.Approver);//, objHeader.Property);
            return objStaff;
        }

        public string ESSCreateOTReq(string URL)
        {
            try
            {
                var ObjEmp = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == User.UserName);
                if (ObjEmp != null)
                {
                    var obj = DB.HRRequestOTs.Where(w => w.EmpCode == ObjEmp.EmpCode).ToList();
                    if ((obj.Where(x => HeaderOT.OTStartTime < x.OTEndTime && HeaderOT.OTEndTime > x.OTStartTime && x.EmpCode == ObjEmp.EmpCode).Count() > 0))
                    {
                        return "DUPLICATE_TIME";
                    }
                    if (HeaderOT.Hours <= 0)
                    {
                        return "INVALID_HOUR";
                    }
                    DateTime DateNow = DateTime.Now;
                    var objAtten = DB.ATEmpSchedules.FirstOrDefault(w => w.EmpCode == ObjEmp.EmpCode && EntityFunctions.TruncateTime(w.TranDate) == EntityFunctions.TruncateTime(HeaderOT.OTStartTime));
                    if (objAtten != null)
                    {
                        if (HeaderOT.OTCategory == "OT")
                        {

                            if (objAtten.Flag1 == 1 && objAtten.Flag2 == 2)
                            {
                                if (HeaderOT.OTStartTime < objAtten.OUT1)
                                {
                                    return "INVALIED_TIME_OT";
                                }
                                else if (DateNow.Date != objAtten.TranDate.Date)
                                {
                                    if (objAtten.End1.Value < HeaderOT.OTEndTime)
                                    {
                                        return "INVALIED_TIME_OT";
                                    }
                                }
                            }
                            else if (objAtten.Flag1 == 1 && objAtten.Flag2 == 1)
                            {
                                if (HeaderOT.OTStartTime < objAtten.OUT2)
                                {
                                    return "INVALIED_TIME_OT";
                                }
                                else if (DateNow.Date != objAtten.TranDate.Date)
                                {
                                    if (objAtten.End2.Value < HeaderOT.OTEndTime)
                                    {
                                        return "INVALIED_TIME_OT";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return "IVALIED_ROSTER";
                    }
                    SetAutoApproval(ObjEmp.EmpCode, HeaderOT.OTEndTime.Date);
                    if (!ListApproval.Any()) return "INVALIED_APPROVER";
                    var objNumber = new CFNumberRank(DocType);
                    if (objNumber == null) return "NUMBER_RANK_NE";
                    HeaderOT.OTRNo = objNumber.NextNumberRank;
                    string Status = SYDocumentStatus.PENDING.ToString();
                    HeaderOT.Status = Status;
                    HeaderOT.EmpCode = ObjEmp.EmpCode;
                    HeaderOT.AllName = ObjEmp.AllName;
                    HeaderOT.Department = ObjEmp.Department;
                    HeaderOT.Position = ObjEmp.Position;
                    HeaderOT.CreatedBy = User.UserName;
                    HeaderOT.CreatedOn = DateTime.Now;


                    DB.HRRequestOTs.Add(HeaderOT);

                    //Add approver
                    foreach (var read in ListApproval)
                    {
                        read.ID = 0;
                        read.LastChangedDate = DateTime.Now;
                        read.DocumentNo = HeaderOT.OTRNo;
                        read.Status = SYDocumentStatus.OPEN.ToString();
                        read.ApprovedBy = "";
                        read.ApprovedName = "";
                        DB.ExDocApprovals.Add(read);
                    }
                    int row = DB.SaveChanges();
                    var approve = ListApproval.OrderBy(w => w.ApproveLevel).ToList();
                    if (approve.Count > 0)
                    {
                        var Appr_ = approve.FirstOrDefault().Approver;
                        var StaffApp = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == Appr_);
                        if (StaffApp != null)
                        {
                            if (!string.IsNullOrEmpty(StaffApp.Email))
                            {
                                #region Send Email
                                SYWorkFlowEmailObject wfo =
                                           new SYWorkFlowEmailObject("ESSOT", WorkFlowType.REQUESTER,
                                                UserType.N, SYDocumentStatus.PENDING.ToString());
                                wfo.SelectListItem = new SYSplitItem(HeaderOT.OTRNo);
                                wfo.User = User;
                                wfo.BS = BS;
                                wfo.UrlView = SYUrl.getBaseUrl();
                                wfo.ScreenId = ScreenId;
                                wfo.Module = "HR";// CModule.PURCHASE.ToString();
                                wfo.ListLineRef = new List<BSWorkAssign>();
                                wfo.DocNo = HeaderOT.OTRNo;
                                wfo.Action = SYDocumentStatus.PENDING.ToString();
                                wfo.ListObjectDictionary = new List<object>();
                                wfo.ListObjectDictionary.Add(HeaderOT);
                                HRStaffProfile Staff = StaffApp;
                                wfo.ListObjectDictionary.Add(Staff);
                                WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Staff);
                                MessageError = wfo.getErrorMessage(result1);
                                #endregion
                            }
                            #region ---Send To Telegram---
                            var EmailTemplate = SMS.TPEmailTemplates.Find("ESSOT_TELEGRAM");
                            if (EmailTemplate != null && !string.IsNullOrEmpty(StaffApp.TeleGroupOT))
                            {
                                SYSendTelegramObject Tel = new SYSendTelegramObject();
                                Tel.User = User;
                                Tel.BS = BS;
                                List<object> ListObjectDictionary = new List<object>();
                                ListObjectDictionary.Add(HeaderOT);
                                ListObjectDictionary.Add(StaffApp);
                                WorkFlowResult result1 = Tel.Send_SMS_Telegram("ESSOT_TELEGRAM", EmailTemplate.RequestContent, StaffApp.TeleGroupOT, ListObjectDictionary, URL);
                                MessageError = Tel.getErrorMessage(result1);
                            }
                            #endregion
                        }
                        #region Notifican on Mobile

                        var access = DB.TokenResources.FirstOrDefault(w => w.UserName == StaffApp.EmpCode);
                        if (access != null)
                        {
                            if (!string.IsNullOrEmpty(access.FirebaseID))
                            {
                                string Desc = "I would like to request OT " + HeaderOT.Hours + " hour. \n Date " + HeaderOT.OTStartTime.ToString("yyyy.MMM.dd") + ". \n Reason: " + HeaderOT.Reason;
                                Notification.Notificationf Noti = new Notification.Notificationf();
                                var clientToken = new List<string>();
                                clientToken.Add(access.FirebaseID);
                                var dd = Noti.SendNotification(clientToken, "Request OT", Desc);
                            }
                        }
                        #endregion
                    }

                }
                    return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderOT.OTRNo.ToString();
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
                log.DocurmentAction = HeaderOT.OTRNo.ToString();
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
                log.DocurmentAction = HeaderOT.OTRNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CreateOTReq()
        {
            string Increment = "";
            try
            {
                var ObjEmp = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == HeaderOT.EmpCode);
                if (ObjEmp != null)
                {
                    var obj = DB.HRRequestOTs.Where(w => w.EmpCode == HeaderOT.EmpCode).ToList();
                    if ((obj.Where(x => HeaderOT.OTStartTime < x.OTEndTime && HeaderOT.OTEndTime > x.OTStartTime && x.EmpCode == HeaderOT.EmpCode).Count() > 0))
                    {
                        return "DUPLICATE_TIME";
                    }
                    if (string.IsNullOrEmpty(HeaderOT.OTCategory)) return "INV_OTCATEGORY";
                    if (HeaderOT.Hours <= 0)
                    {
                        return "INVALID_HOUR";
                    }
                    DateTime DateNow = DateTime.Now;
                    var objAtten = DB.ATEmpSchedules.FirstOrDefault(w => w.EmpCode == ObjEmp.EmpCode && EntityFunctions.TruncateTime(w.TranDate) == EntityFunctions.TruncateTime(HeaderOT.OTStartTime));
                    if (objAtten != null)
                    {
                        if (HeaderOT.OTCategory == "OT")
                        {

                            if (objAtten.Flag1 == 1 && objAtten.Flag2 == 2)
                            {
                                if (HeaderOT.OTStartTime < objAtten.OUT1)
                                {
                                    return "INVALIED_TIME_OT";
                                }
                                else if (DateNow.Date != objAtten.TranDate.Date)
                                {
                                    if (objAtten.End1.Value < HeaderOT.OTEndTime)
                                    {
                                        return "INVALIED_TIME_OT";
                                    }
                                }
                            }
                            else if (objAtten.Flag1 == 1 && objAtten.Flag2 == 1)
                            {
                                if (HeaderOT.OTStartTime < objAtten.OUT2)
                                {
                                    return "INVALIED_TIME_OT";
                                }
                                else if (DateNow.Date != objAtten.TranDate.Date)
                                {
                                    if (objAtten.End2.Value < HeaderOT.OTEndTime)
                                    {
                                        return "INVALIED_TIME_OT";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return "IVALIED_ROSTER";
                    }
                    var objNumber = new CFNumberRank(DocType);
                    if (objNumber == null) return "NUMBER_RANK_NE";
                    HeaderOT.OTRNo = objNumber.NextNumberRank;
                    Increment = HeaderOT.OTRNo;
                    string Status = SYDocumentStatus.APPROVED.ToString();
                    HeaderOT.Status = Status;
                    HeaderOT.Department = ObjEmp.Department;
                    HeaderOT.Position = ObjEmp.Position;
                    HeaderOT.RequestBy = User.UserName;
                    HeaderOT.CreatedBy = User.UserName;
                    HeaderOT.CreatedOn = DateTime.Now;
                    DB.HRRequestOTs.Add(HeaderOT);

                    int row = DB.SaveChanges();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
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
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CreateOTReq(string URL)
        {
            string Increment = "";
            try
            {
                var ObjEmp = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == HeaderOT.EmpCode);
                if (ObjEmp != null)
                {
                    var obj = DB.HRRequestOTs.Where(w => w.EmpCode == HeaderOT.EmpCode).ToList();
                    if ((obj.Where(x => HeaderOT.OTStartTime < x.OTEndTime && HeaderOT.OTEndTime > x.OTStartTime && x.EmpCode == HeaderOT.EmpCode).Count() > 0))
                    {
                        return "DUPLICATE_TIME";
                    }
                    if (string.IsNullOrEmpty(HeaderOT.OTCategory)) return "INV_OTCATEGORY";
                    if (HeaderOT.Hours <= 0)
                    {
                        return "INVALID_HOUR";
                    }
                    DateTime DateNow = DateTime.Now;
                    var objAtten = DB.ATEmpSchedules.FirstOrDefault(w => w.EmpCode == HeaderOT.EmpCode && EntityFunctions.TruncateTime(w.TranDate) == EntityFunctions.TruncateTime(HeaderOT.OTStartTime));
                    if (objAtten != null)
                    {
                        if (HeaderOT.OTCategory == "OT")
                        {

                            if (objAtten.Flag1 == 1 && objAtten.Flag2 == 2)
                            {
                                if (HeaderOT.OTStartTime < objAtten.OUT1)
                                {
                                    return "INVALIED_TIME_OT";
                                }
                                else if (DateNow.Date != objAtten.TranDate.Date)
                                {
                                    if (objAtten.End1.Value < HeaderOT.OTEndTime)
                                    {
                                        return "INVALIED_TIME_OT";
                                    }
                                }
                            }
                            else if (objAtten.Flag1 == 1 && objAtten.Flag2 == 1)
                            {
                                if (HeaderOT.OTStartTime < objAtten.OUT2)
                                {
                                    return "INVALIED_TIME_OT";
                                }
                                else if (DateNow.Date != objAtten.TranDate.Date)
                                {
                                    if (objAtten.End2.Value < HeaderOT.OTEndTime)
                                    {
                                        return "INVALIED_TIME_OT";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return "IVALIED_ROSTER";
                    }
                    SetAutoApproval(HeaderOT.EmpCode, HeaderOT.OTEndTime.Date);
                    if (!ListApproval.Any()) return "INVALIED_APPROVER";
                    var objNumber = new CFNumberRank(DocType);
                    if (objNumber == null) return "NUMBER_RANK_NE";
                    HeaderOT.OTRNo = objNumber.NextNumberRank;
                    Increment = HeaderOT.OTRNo;
                    string Status = SYDocumentStatus.PENDING.ToString();
                    HeaderOT.Status = Status;
                    HeaderOT.Department = ObjEmp.Department;
                    HeaderOT.Position = ObjEmp.Position;
                    HeaderOT.RequestBy = User.UserName;
                    HeaderOT.CreatedBy = User.UserName;
                    HeaderOT.CreatedOn = DateTime.Now;
                    DB.HRRequestOTs.Add(HeaderOT);

                    foreach (var read in ListApproval)
                    {
                        read.ID = 0;
                        read.LastChangedDate = DateTime.Now;
                        read.DocumentNo = HeaderOT.OTRNo;
                        read.Status = SYDocumentStatus.OPEN.ToString();
                        read.ApprovedBy = "";
                        read.ApprovedName = "";
                        DB.ExDocApprovals.Add(read);
                    }
                    int row = DB.SaveChanges();

                    SYHRAnnouncement _announ = new SYHRAnnouncement();
                    if (ListApproval.Count() > 0)
                    {
                        _announ.Type = "Request OverTime";
                        _announ.Subject = ObjEmp.AllName;
                        _announ.Description = ObjEmp.EmpCode + ":" + ObjEmp.AllName + @" REVIEW/APPROVAL  of Resquest OT in from"
                           + HeaderOT.OTStartTime.ToString("yyyy.MM.dd hh:mm tt") + " to " + HeaderOT.OTEndTime.ToString("yyyy.MM.dd hh:mm tt")
                           + " My Reason: " + HeaderOT.Reason;
                        _announ.DocumentNo = HeaderOT.OTRNo.ToString();
                        _announ.DocumentDate = DateTime.Now;
                        _announ.IsRead = false;
                        _announ.UserName = ListApproval.First().Approver;
                        _announ.CreatedBy = User.UserName;
                        _announ.CreatedOn = DateTime.Now;
                        DB.SYHRAnnouncements.Add(_announ);
                        // Notification
                        var access = DB.TokenResources.FirstOrDefault(w => w.UserName == _announ.UserName);
                        if (access != null)
                        {
                            if (!string.IsNullOrEmpty(access.FirebaseID))
                            {
                                Notification.Notificationf Noti = new Notification.Notificationf();
                                var clientToken = new List<string>();
                                clientToken.Add(access.FirebaseID);
                                var dd = Noti.SendNotification(clientToken, "REQUEST OT", _announ.Description);
                            }
                        }
                    }
                }
                HRStaffProfile StaffApp = getNextApprover(Increment);
                if (StaffApp != null)
                {
                    if (!string.IsNullOrEmpty(StaffApp.Email))
                    {
                        #region Send Email
                        SYWorkFlowEmailObject wfo =
                                   new SYWorkFlowEmailObject("ESSOT", WorkFlowType.REQUESTER,
                                        UserType.N, SYDocumentStatus.PENDING.ToString());
                        wfo.SelectListItem = new SYSplitItem(HeaderOT.OTRNo);
                        wfo.User = User;
                        wfo.BS = BS;
                        wfo.UrlView = SYUrl.getBaseUrl();
                        wfo.ScreenId = ScreenId;
                        wfo.Module = "HR";// CModule.PURCHASE.ToString();
                        wfo.ListLineRef = new List<BSWorkAssign>();
                        wfo.DocNo = HeaderOT.OTRNo;
                        wfo.Action = SYDocumentStatus.PENDING.ToString();
                        wfo.ListObjectDictionary = new List<object>();
                        wfo.ListObjectDictionary.Add(HeaderOT);
                        HRStaffProfile Staff = StaffApp;
                        wfo.ListObjectDictionary.Add(Staff);
                        WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Staff);
                        MessageError = wfo.getErrorMessage(result1);
                        #endregion
                    }
                    #region ---Send To Telegram---
                    var EmailTemplate = SMS.TPEmailTemplates.Find("ESSOT_TELEGRAM");
                    if (EmailTemplate != null && !string.IsNullOrEmpty(StaffApp.TeleGroupOT))
                    {
                        SYSendTelegramObject Tel = new SYSendTelegramObject();
                        Tel.User = User;
                        Tel.BS = BS;
                        List<object> ListObjectDictionary = new List<object>();
                        ListObjectDictionary.Add(HeaderOT);
                        ListObjectDictionary.Add(StaffApp);
                        WorkFlowResult result1 = Tel.Send_SMS_Telegram("ESSOT_TELEGRAM", EmailTemplate.RequestContent, StaffApp.TeleGroupOT, ListObjectDictionary, URL);
                        MessageError = Tel.getErrorMessage(result1);
                    }
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
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditOTReq(string id)
        {
            try
            {
                HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = DB.HRRequestOTs.FirstOrDefault(w => w.OTRNo == id);
                if (objMatch == null)
                {
                    return "OTREQUEST_NE";
                }
                if (string.IsNullOrEmpty(HeaderOT.OTCategory)) return "INV_OTCATEGORY";
                var tblRequesOT = DB.HRRequestOTs.AsEnumerable().Where(w => w.OTRNo != id 
                                  && w.EmpCode == HeaderOT.EmpCode).ToList();
                if (tblRequesOT.Any())
                {
                    if ((tblRequesOT.Where(x => HeaderOT.OTStartTime < x.OTEndTime && HeaderOT.OTEndTime > x.OTStartTime).Count() > 0))
                    {
                        return "DUPLICATE_TIME";
                    }
                }

                if (HeaderOT.Hours <= 0)
                {
                    return "INVALID_HOUR";
                }
                DateTime DateNow = DateTime.Now;
                var objAtten = DB.ATEmpSchedules.FirstOrDefault(w => w.EmpCode == objMatch.EmpCode && EntityFunctions.TruncateTime(w.TranDate) == EntityFunctions.TruncateTime(HeaderOT.OTStartTime));
                if (objAtten != null)
                {
                    if (HeaderOT.OTCategory == "OT")
                    {

                        if (objAtten.Flag1 == 1 && objAtten.Flag2 == 2)
                        {
                            if (HeaderOT.OTStartTime < objAtten.OUT1)
                            {
                                return "INVALIED_TIME_OT";
                            }
                            else if (DateNow.Date != objAtten.TranDate.Date)
                            {
                                if (objAtten.End1.Value < HeaderOT.OTEndTime)
                                {
                                    return "INVALIED_TIME_OT";
                                }
                            }
                        }
                        else if (objAtten.Flag1 == 1 && objAtten.Flag2 == 1)
                        {
                            if (HeaderOT.OTStartTime < objAtten.OUT2)
                            {
                                return "INVALIED_TIME_OT";
                            }
                            else if (DateNow.Date != objAtten.TranDate.Date)
                            {
                                if (objAtten.End2.Value < HeaderOT.OTEndTime)
                                {
                                    return "INVALIED_TIME_OT";
                                }
                            }
                        }
                    }
                }
                else
                {
                    return "IVALIED_ROSTER";
                }
                var objEmp = DB.HRRequestOTs.Where(w => w.OTRNo == objMatch.OTRNo).ToList();
                foreach (var item in objEmp)
                {
                    DB.HRRequestOTs.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                objMatch.OTStartTime = HeaderOT.OTStartTime;
                objMatch.OTEndTime = HeaderOT.OTEndTime;
                objMatch.Hours = HeaderOT.Hours;
                objMatch.BreakTime = HeaderOT.BreakTime;
                objMatch.Reason = HeaderOT.Reason;
                objMatch.OTCategory = HeaderOT.OTCategory;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                DB.HRRequestOTs.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.OTStartTime).IsModified = true;
                DB.Entry(objMatch).Property(w => w.OTEndTime).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Hours).IsModified = true;
                DB.Entry(objMatch).Property(w => w.BreakTime).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Reason).IsModified = true;
                DB.Entry(objMatch).Property(w => w.OTCategory).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;


                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderOT.OTRNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ApproveOTReq(string OTRNo)
        {
            try
            {
                DB = new HumicaDBContext();
                string[] c = OTRNo.Split(';');
                foreach (var ID in c)
                {
                    var objMatch = DB.HRRequestOTs.FirstOrDefault(w => w.OTRNo == ID);
                    if (objMatch == null)
                    {
                        return "EE001";
                    }
                    if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                    {
                        return "INV_DOC";
                    }
                    var status = SYDocumentStatus.APPROVED.ToString();
                    objMatch.Status = status;
                    objMatch.ChangedOn = DateTime.Now;
                    objMatch.ChangedBy = User.UserName;
                    DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                    DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                    DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                    int row = DB.SaveChanges();
                }
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = OTRNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ApproveOTReq(string OTRNo, string URL)
        {
            try
            {
                DB = new HumicaDBContext();
                string[] c = OTRNo.Split(';');
                foreach (var ID in c)
                {
                    var objMatch = DB.HRRequestOTs.FirstOrDefault(w => w.OTRNo == ID);
                    if (objMatch == null)
                    {
                        return "EE001";
                    }
                    if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                    {
                        return "INV_DOC";
                    }
                    string open = SYDocumentStatus.OPEN.ToString();
                    string DocNo = objMatch.OTRNo;
                    var listApproval = DB.ExDocApprovals
                        .Where(w => w.DocumentType == "REQ_OT" && w.DocumentNo == DocNo)
                        .OrderBy(w => w.ApproveLevel)
                        .ToList();
                    var listUser = DB.HRStaffProfiles
                        .Where(w => w.EmpCode == User.UserName)
                        .ToList();
                    var b = false;
                    if (listApproval.Count == 0)
                    {
                        return "RESTRICT_ACCESS";
                    }
                    var APPROVED = SYDocumentStatus.APPROVED.ToString();
                    var REJECTED = SYDocumentStatus.REJECTED.ToString();
                    var CANCELLED = SYDocumentStatus.CANCELLED.ToString();
                    var approver = "";
                    foreach (var read in listApproval)
                    {
                        if (listApproval.Where(w => w.ApproveLevel == read.ApproveLevel
                            && (w.Status == APPROVED || w.Status == REJECTED || w.Status == CANCELLED)).Any())
                            continue;
                        var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
                        if (checklist.Count > 0)
                        {
                            if (read.Status == SYDocumentStatus.APPROVED.ToString())
                            {
                                return "USER_ALREADY_APP";
                            }

                            if (read.ApproveLevel > listApproval.Where(w => w.Status == open).Min(w => w.ApproveLevel))
                            {
                                return "REQUIRED_PRE_LEVEL";
                            }
                            var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                            if (objStaff != null)
                            {
                                foreach (var item in listApproval.Where(w => w.ApproveLevel == read.ApproveLevel))
                                {
                                    item.ApprovedBy = objStaff.EmpCode;
                                    item.ApprovedName = objStaff.AllName;
                                    item.LastChangedDate = DateTime.Now.Date;
                                    item.ApprovedDate = DateTime.Now;
                                    item.Status = SYDocumentStatus.APPROVED.ToString();
                                    DB.ExDocApprovals.Attach(item);
                                    DB.Entry(item).State = System.Data.Entity.EntityState.Modified;
                                    approver = objStaff.EmpCode;
                                }
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
                    if (listApproval.Where(w => w.Status == open).ToList().Count > 0)
                    {
                        status = SYDocumentStatus.PENDING.ToString();
                    }
                    objMatch.Status = status;

                    DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                    DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                    DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                    int row = DB.SaveChanges();

                    var StaffApp = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                    if (StaffApp != null)
                    {
                        string WFObject = "ESSOT";
                        string Email_Template = "ESSOT_TELEGRAM";
                        if (objMatch.Status == SYDocumentStatus.APPROVED.ToString())
                        {
                            WFObject = "ESSOT_APP";
                            Email_Template = "ESSOT_TELEGRAM_APP";
                        }
                        else
                        {
                            StaffApp = getNextApprover(objMatch.OTRNo);
                        }
                        if (!string.IsNullOrEmpty(StaffApp.Email))
                        {
                            #region Send Email
                            SYWorkFlowEmailObject wfo =
                                       new SYWorkFlowEmailObject(WFObject, WorkFlowType.REQUESTER,
                                            UserType.N, SYDocumentStatus.PENDING.ToString());
                            wfo.SelectListItem = new SYSplitItem(objMatch.OTRNo);
                            wfo.User = User;
                            wfo.BS = BS;
                            wfo.UrlView = SYUrl.getBaseUrl();
                            wfo.ScreenId = ScreenId;
                            wfo.Module = "HR";// CModule.PURCHASE.ToString();
                            wfo.ListLineRef = new List<BSWorkAssign>();
                            wfo.DocNo = objMatch.OTRNo;
                            wfo.Action = SYDocumentStatus.PENDING.ToString();
                            wfo.ListObjectDictionary = new List<object>();
                            wfo.ListObjectDictionary.Add(objMatch);
                            HRStaffProfile Staff = StaffApp;
                            wfo.ListObjectDictionary.Add(Staff);
                            WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Staff);
                            MessageError = wfo.getErrorMessage(result1);
                            #endregion
                        }
                        #region ---Send To Telegram---
                        var EmailTemplate = SMS.TPEmailTemplates.Find(Email_Template);
                        if (EmailTemplate != null && !string.IsNullOrEmpty(StaffApp.TeleChartID))
                        {
                            SYSendTelegramObject Tel = new SYSendTelegramObject();
                            Tel.User = User;
                            Tel.BS = BS;
                            List<object> ListObjectDictionary = new List<object>();
                            ListObjectDictionary.Add(objMatch);
                            ListObjectDictionary.Add(StaffApp);
                            WorkFlowResult result1 = Tel.Send_SMS_Telegram(Email_Template, EmailTemplate.RequestContent, StaffApp.TeleChartID, ListObjectDictionary, URL);
                            MessageError = Tel.getErrorMessage(result1);
                        }
                        #endregion
                        if (objMatch.Status == SYDocumentStatus.APPROVED.ToString())
                        {
                            var Sett = DB.SYHRSettings.First();
                            var EmailTemplateCC = SMS.TPEmailTemplates.Find("ESSOT_TELE_CC_HR");
                            if (EmailTemplateCC != null && Sett != null && !string.IsNullOrEmpty(Sett.TelegOT))
                            {
                                SYSendTelegramObject Tel = new SYSendTelegramObject();
                                Tel.User = User;
                                Tel.BS = BS;
                                List<object> ListObjectDictionary = new List<object>();
                                ListObjectDictionary.Add(objMatch);
                                ListObjectDictionary.Add(StaffApp);
                                WorkFlowResult result1 = Tel.Send_SMS_Telegram("ESSOT_TELE_CC_HR", EmailTemplateCC.RequestContent, Sett.TelegOT, ListObjectDictionary, URL);
                                MessageError = Tel.getErrorMessage(result1);
                            }
                        }
                    }

                }
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = OTRNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ImportAOTRequest()
        {
            try
            {
                var emp = DB.HRStaffProfiles.ToList();
                var TranNo =DB.HRRequestOTs.OrderByDescending(u => u.OTRNo).FirstOrDefault();
                

                foreach (var read in ListOTRequest)
                {
                    var empcheck = emp.Where(w => w.EmpCode == read.EmpCode).ToList();
                    if (empcheck.Count() == 0) return "Invalid EmpCode : " + read.EmpCode;
                    if (read.OTStartTime > read.OTEndTime) return read.EmpCode + " has invalid FromDate";
                    if (read.OTEndTime < read.OTStartTime) return read.EmpCode + " has invalid Todate";
                    if (empcheck.Count > 0 )
                    {
                        var obj = new HRRequestOT();
                        obj.OTRNo = read.OTRNo;
                        string Status = SYDocumentStatus.PENDING.ToString();
                        obj.Status = Status;
                        obj.EmpCode = read.EmpCode;
                        obj.AllName = empcheck.First().AllName;
                        obj.CreatedBy = User.UserName;
                        obj.CreatedOn = DateTime.Now;
                        obj.OTStartTime = read.OTStartTime;
                        obj.OTEndTime = read.OTEndTime;
                        obj.Hours = read.Hours;
                        obj.BreakTime = read.BreakTime;
                        obj.Reason = read.Reason;
                        DB.HRRequestOTs.Add(obj);
                        
                    }
                    else
                    {
                        MessageError = "EmpCode :" + read.EmpCode;
                        return "INV_DOC";
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
                log.DocurmentAction = Header.TranNo.ToString();
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
                log.DocurmentAction = Header.TranNo.ToString();
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
                log.DocurmentAction = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string requestToApprove(string OTRNo, string URL)
        {
            try
            {
                string[] c = OTRNo.Split(';');
                foreach (var id in c)
                {
                    HumicaDBContext DBX = new HumicaDBContext();
                    var objMatch = DB.HRRequestOTs.FirstOrDefault(w => w.OTRNo == id);
                    if (objMatch == null)
                    {
                        return "REQUEST_NE";
                    }
                    HeaderOT = objMatch;
                    var LstStaffProfiles = DB.HRStaffProfiles.Where(w => w.Status == "A").ToList();
                    var StaffReq = LstStaffProfiles.FirstOrDefault(w => w.EmpCode == HeaderOT.EmpCode);
                    HRStaffProfile StaffApp = new HRStaffProfile();
                    var chkFLName = LstStaffProfiles.Where(w => w.JobCode == StaffReq.FirstLine).ToList();
                    if (chkFLName.Count() > 0)
                    {
                        StaffApp = chkFLName.FirstOrDefault(w => w.DEPT == StaffReq.DEPT);
                    }
                    else if (chkFLName.Count() == 0)
                    {
                        var chkSLName = LstStaffProfiles.Where(w => w.JobCode == StaffReq.SecondLine).ToList();
                        if (chkSLName.Count() > 0)
                        {
                            StaffApp = chkSLName.First();
                        }
                    }

                    if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                    {
                        return "INV_DOC";
                    }

                    objMatch.Status = SYDocumentStatus.PENDING.ToString();
                    if (StaffApp != null)
                    {
                        var DocApproval = new ExDocApproval();
                        DocApproval.Approver = StaffApp.EmpCode;
                        DocApproval.ApproverName = StaffApp.AllName;
                        DocApproval.ApprovedBy = "";
                        DocApproval.ApprovedName = "";
                        DocApproval.ApproveLevel = 1;
                        DocApproval.DocumentType = "REQ_OT";
                        DocApproval.DocumentNo = objMatch.OTRNo.ToString();
                        DocApproval.Status = SYDocumentStatus.OPEN.ToString();
                        DocApproval.WFObject = "WF02";
                        DB.ExDocApprovals.Add(DocApproval);
                    }
                    DB.HRRequestOTs.Attach(objMatch);
                    DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                    DB.SaveChanges();

                    // int row = DBX.SaveChanges();

                    if (StaffApp != null)
                    {
                        if (!string.IsNullOrEmpty(StaffApp.Email))
                        {
                            #region Send Email
                            SYWorkFlowEmailObject wfo =
                                       new SYWorkFlowEmailObject("ESSOT", WorkFlowType.REQUESTER,
                                            UserType.N, SYDocumentStatus.PENDING.ToString());
                            wfo.SelectListItem = new SYSplitItem(objMatch.OTRNo);
                            wfo.User = User;
                            wfo.BS = BS;
                            wfo.UrlView = SYUrl.getBaseUrl();
                            wfo.ScreenId = ScreenId;
                            wfo.Module = "HR";// CModule.PURCHASE.ToString();
                            wfo.ListLineRef = new List<BSWorkAssign>();
                            wfo.DocNo = objMatch.OTRNo;
                            wfo.Action = SYDocumentStatus.PENDING.ToString();
                            wfo.ListObjectDictionary = new List<object>();
                            wfo.ListObjectDictionary.Add(objMatch);
                            HRStaffProfile Staff = StaffApp;
                            wfo.ListObjectDictionary.Add(Staff);
                            WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Staff);
                            MessageError = wfo.getErrorMessage(result1);
                            #endregion
                        }
                        #region ---Send To Telegram---
                        var EmailTemplate = SMS.TPEmailTemplates.Find("ESSOT_TELEGRAM");
                        if (EmailTemplate != null&&!string.IsNullOrEmpty(StaffApp.TeleChartID))
                        {
                            SYSendTelegramObject Tel = new SYSendTelegramObject();
                            Tel.User = User;
                            Tel.BS = BS;
                            List<object> ListObjectDictionary = new List<object>();
                            ListObjectDictionary.Add(objMatch);
                            ListObjectDictionary.Add(StaffApp);
                            Tel.Send_SMS_Telegram("ESSOT_TELEGRAM", EmailTemplate.RequestContent, StaffApp.TeleChartID, ListObjectDictionary, URL);
                        }
                        #endregion
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = HeaderOT.OTRNo;
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
                log.DocurmentAction = HeaderOT.OTRNo;
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
                log.DocurmentAction = HeaderOT.OTRNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string RejectOTReq(string OTRNo)
        {
            try
            {
                string[] c = OTRNo.Split(';');
                foreach (var ID in c)
                {
                    var objMatch = DB.HRRequestOTs.FirstOrDefault(w => w.OTRNo == ID);
                    if (objMatch == null)
                    {
                        return "EE001";
                    }
                    objMatch.Status = SYDocumentStatus.REJECTED.ToString();
                    objMatch.ChangedBy = User.UserName;
                    objMatch.ChangedOn = DateTime.Now;


                    DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                    DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                    DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                }
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }

            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = OTRNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CancelOTReq(string OTRNo)
        {
            try
            {
                string[] c = OTRNo.Split(';');
                foreach (var ID in c)
                {
                    var objMatch = DB.HRRequestOTs.FirstOrDefault(w => w.OTRNo == ID);
                    if (objMatch == null)
                    {
                        return "EE001";
                    }
                    objMatch.Status = SYDocumentStatus.CANCELLED.ToString();
                    objMatch.ChangedBy = User.UserName;
                    objMatch.ChangedOn = DateTime.Now;


                    DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                    DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                    DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                    int row = DB.SaveChanges();
                }
                return SYConstant.OK;
            }

            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = OTRNo;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion

        public List<ClsReuestOT> LoadOTPending(FTINYear filter)
        {
            string approved = SYDocumentStatus.APPROVED.ToString();
            string Reject = SYDocumentStatus.REJECTED.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();
            var staff = DB.HRStaffProfiles.ToList();
            var staffcheck = staff.ToList();
            List<HRRequestOT> _ListOT = DB.HRRequestOTs.Where(x => x.Status != approved && x.Status != Reject && x.Status != Cancel).ToList();
            if (filter.Department != null)
            {
                staffcheck = staffcheck.Where(w => w.DEPT == filter.Department).ToList();
                _ListOT = _ListOT.Where(w => staffcheck.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            }
            if (filter.Position != null)
            {
                staffcheck = staffcheck.Where(w => w.JobCode == filter.Position).ToList();
                _ListOT = _ListOT.Where(w => staffcheck.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            }
            List<ClsReuestOT> _list = new List<ClsReuestOT>();
            foreach (var read in _ListOT)
            {
                ClsReuestOT _OT = new ClsReuestOT();
                _OT.EmpCode = read.EmpCode;
                _OT.EmpName = read.AllName;
                _OT.OTStartTime = read.OTStartTime;
                _OT.OTEndTime = read.OTEndTime;
                _OT.OTHour = (decimal)read.Hours;
                _OT.BreakTime = read.BreakTime;
                _OT.Reason = read.Reason;
                _OT.Status = read.Status;
                _OT.OTRNo = read.OTRNo;
                _list.Add(_OT);
            }
            return _list;
        }
        #region "Create OT"
        public string CreateOT()
        {
            DB = new HumicaDBContext();
            try
            {
                if (HeaderStaff.EmpCode == null)
                {
                    return "EMPCodeEmty";
                }
                if (ListHeader.Count == 0)
                {
                    return "LIST_INV";
                }
                var OTRate = DB.PROTRates.ToList();
                foreach (var read in ListHeader)
                {
                    var obj = new PREmpOverTime();
                    obj = read;
                    obj.EmpCode = HeaderStaff.EmpCode;
                    obj.EmpName = HeaderStaff.AllName;
                    obj.CreateBy = User.UserName;
                    obj.CreateOn = DateTime.Now;
                    obj.OTFromTime = read.OTFromTime.HasValue && read.OTFromTime.Value.Year != 5000 ? read.OTFromTime : null;
                    obj.OTToTime = read.OTToTime.HasValue && read.OTToTime.Value.Year != 5000 ? read.OTToTime : null;
                    obj.BreakTime = read.BreakTime;
                    obj.OTHour = read.OTHour;

                    foreach (var item in OTRate.Where(w => w.OTCode == read.OTType).ToList())
                    {
                        obj.OTDescription = item.OTType;
                    }

                    DB.PREmpOverTimes.Add(obj);
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
                log.DocurmentAction = Header.TranNo.ToString();
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
                log.DocurmentAction = Header.TranNo.ToString();
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
                log.DocurmentAction = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteOT(int id)
        {
            try
            {
                Header = new PREmpOverTime();
                Header.TranNo = id;
                var objMatch = DB.PREmpOverTimes.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                Header = objMatch;
                DB.PREmpOverTimes.Attach(objMatch);
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
        public string EditOT(int id)
        {
            var DBI = new HumicaDBContext();
            var DBD = new HumicaDBContext();
            DB = new HumicaDBContext();
            try
            {
                DBI.Configuration.AutoDetectChangesEnabled = false;
                DBD.Configuration.AutoDetectChangesEnabled = false;
                DB.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    if (ListHeader.Count == 0)
                    {
                        return "LIST_INV";
                    }
                    Header = new PREmpOverTime();
                    var OTRate = DB.PROTRates.ToList();
                    var ListEmpOT = DB.PREmpOverTimes.Where(w => w.EmpCode == HeaderStaff.EmpCode && w.TranNo == id).ToList();
                    foreach (var read in ListHeader)
                    {
                        Header = read;
                        MessageError = read.EmpCode + ":" + read.OTType;
                        var Typecheck = OTRate.Where(w => w.OTCode == read.OTType).ToList();
                        if (read.OTHour == 0)
                        {
                            //delete
                            var objUpdate = ListEmpOT.Where(w => w.EmpCode == read.EmpCode &&
                            w.OTType == read.OTType && w.OTDate.Value.Date == read.OTDate.Value.Date).ToList();
                            if (objUpdate.Count > 0)
                            {
                                var obj = objUpdate.First();
                                DBD.PREmpOverTimes.Attach(obj);
                                DBD.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                            }
                        }
                        else
                        {
                            var obj = new PREmpOverTime();
                            obj.EmpCode = HeaderStaff.EmpCode;
                            obj.EmpName = HeaderStaff.AllName;
                            obj.OTType = read.OTType;
                            obj.OTDate = read.OTDate;
                            obj.OTHour = read.OTHour;
                            obj.BreakTime = read.BreakTime;
                            obj.OTFromTime = read.OTFromTime.HasValue && read.OTFromTime.Value.Year != 5000 ? read.OTFromTime : null;
                            obj.OTToTime = read.OTToTime.HasValue && read.OTToTime.Value.Year != 5000 ? read.OTToTime : null;
                            obj.PayMonth = read.PayMonth;
                            obj.LCK = 0;
                            obj.CreateBy = User.UserName;
                            obj.CreateOn = DateTime.Now;
                            obj.Reason = read.Reason;
                            if (OTRate.Where(w => w.OTCode == read.OTType).Any())
                                obj.OTDescription = OTRate.FirstOrDefault(w => w.OTCode == read.OTType).OTType;
                            else
                            {
                                return MessageError;
                            }
                            var EmpOT = ListEmpOT.Where(w => w.EmpCode == read.EmpCode && w.OTType == read.OTType
                            && w.OTDate.Value.Date == read.OTDate.Value.Date).ToList();
                            if (EmpOT.Count > 0)
                            {
                                var objMatch = EmpOT.First();
                                objMatch.OTHour = read.OTHour;
                                objMatch.PayMonth = read.PayMonth;
                                objMatch.ChangedBy = User.UserName;
                                objMatch.ChangedOn = DateTime.Now;
                                  objMatch.OTFromTime = obj.OTFromTime;
                                objMatch.OTToTime = obj.OTToTime;
                                objMatch.BreakTime = obj.BreakTime;
                                objMatch.Reason = read.Reason;
                                DBI.PREmpOverTimes.Attach(objMatch);
                                DBI.Entry(objMatch).Property(w => w.OTHour).IsModified = true;
                                DBI.Entry(objMatch).Property(w => w.PayMonth).IsModified = true;
                                DBI.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                                DBI.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                                DBI.Entry(objMatch).Property(w => w.OTFromTime).IsModified = true;
                                DBI.Entry(objMatch).Property(w => w.OTToTime).IsModified = true;
                                DBI.Entry(objMatch).Property(w => w.BreakTime).IsModified = true;
                                DBI.Entry(objMatch).Property(w => w.Reason).IsModified = true;
                            }
                            else
                            {
                                DB.PREmpOverTimes.Add(obj);
                            }
                        }
                    }
                    DBD.SaveChanges();
                    int row1 = DBI.SaveChanges();
                    int row = DB.SaveChanges();

                    return SYConstant.OK;
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.TranNo.ToString();
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
                    log.DocurmentAction = Header.TranNo.ToString();
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
                    log.DocurmentAction = Header.TranNo.ToString();
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DBI.Configuration.AutoDetectChangesEnabled = true;
                DBD.Configuration.AutoDetectChangesEnabled = true;
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string ImportOTTime()
        {
            var DBI = new HumicaDBContext();
            var DBD = new HumicaDBContext();
            DB = new HumicaDBContext();
            try
            {
                DBI.Configuration.AutoDetectChangesEnabled = false;
                DBD.Configuration.AutoDetectChangesEnabled = false;
                DB.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    var ListStaff = DB.HRStaffProfiles.ToList();
                    var OTType = DB.PROTRates.ToList();
                    var ListEmpOT = DB.PREmpOverTimes.ToList();

                    foreach (var read in ListHeader)
                    {
                        MessageError = read.EmpCode + ":" + read.OTType;
                        var empcheck = ListStaff.Where(w => w.EmpCode == read.EmpCode).ToList();
                        var Typecheck = OTType.Where(w => w.OTCode == read.OTType).ToList();
                        if (read.OTHour == 0)
                        {
                            //delete
                            var objUpdate = ListEmpOT.Where(w => w.EmpCode == read.EmpCode &&
                            w.OTType == read.OTType && w.OTDate.Value.Date == read.OTDate.Value.Date).ToList();
                            if (objUpdate.Count > 0)
                            {
                                var obj = objUpdate.First();
                                DBD.PREmpOverTimes.Attach(obj);
                                DBD.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                            }
                        }
                        else
                        {
                            var obj = new PREmpOverTime();
                            obj.EmpCode = read.EmpCode;
                            obj.OTType = read.OTType;
                            obj.OTDate = read.OTDate;
                            obj.OTFromTime = read.OTFromTime;
                            obj.OTToTime = read.OTToTime;
                            obj.OTHour = ClsRounding.Rounding(read.OTHour, 2, "N");
                            obj.BreakTime = read.BreakTime;
                            obj.PayMonth = read.PayMonth;
                            obj.LCK = 0;
                            obj.CreateBy = User.UserName;
                            obj.CreateOn = DateTime.Now;
                            if (OTType.Where(w => w.OTCode == read.OTType).Any())
                                obj.OTDescription = OTType.FirstOrDefault(w => w.OTCode == read.OTType).OTType;
                            else
                            {
                                return MessageError;
                            }
                            if (empcheck.Count > 0)
                                obj.EmpName = empcheck.FirstOrDefault().AllName;
                            var EmpOT = ListEmpOT.FirstOrDefault(w => w.EmpCode == read.EmpCode && w.OTType == read.OTType
                            && w.OTDate.Value.Date == read.OTDate.Value.Date);
                            if (EmpOT!=null)
                            {
                                var objMatch = EmpOT;
                                objMatch.OTHour = ClsRounding.Rounding(read.OTHour, 2, "N");
                                objMatch.PayMonth = read.PayMonth;
                                objMatch.ChangedBy = User.UserName;
                                objMatch.ChangedOn = DateTime.Now;
                                objMatch.OTFromTime = read.OTFromTime;
                                objMatch.OTToTime = read.OTToTime;
                                objMatch.BreakTime = read.BreakTime;
                                DBI.PREmpOverTimes.Attach(objMatch);
                                DBI.Entry(objMatch).Property(w => w.OTHour).IsModified = true;
                                DBI.Entry(objMatch).Property(w => w.PayMonth).IsModified = true;
                                DBI.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                                DBI.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                                DBI.Entry(objMatch).Property(w => w.OTFromTime).IsModified = true;
                                DBI.Entry(objMatch).Property(w => w.OTToTime).IsModified = true;
                                DBI.Entry(objMatch).Property(w => w.BreakTime).IsModified = true;
                            }
                            else
                            {
                                DB.PREmpOverTimes.Add(obj);
                            }
                        }
                    }
                    DBD.SaveChanges();
                    int row1 = DBI.SaveChanges();
                    int row = DB.SaveChanges();

                    return SYConstant.OK;
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = Header.TranNo.ToString();
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
                    log.DocurmentAction = Header.TranNo.ToString();
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
                    log.DocurmentAction = Header.TranNo.ToString();
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DBI.Configuration.AutoDetectChangesEnabled = true;
                DBD.Configuration.AutoDetectChangesEnabled = true;
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public int IsExistOT(string EmpCode, string OTType, DateTime OTDate)
        {
            int Result = -1;
            var _list = DB.PREmpOverTimes.Where(w => w.EmpCode == EmpCode).ToList();
            _list = _list.Where(w => w.OTDate.Value == OTDate.Date && w.OTType == OTType).ToList();
            if (_list.Count > 0)
            {
                foreach (var item in _list)
                {
                    if (!ListHeader.Where(w => w.EmpCode == EmpCode && w.OTDate.Value == OTDate.Date
                     && w.OTType == OTType && w.TranNo > 0).Any())
                        Result = Convert.ToInt32(item.Status);
                }
            }
            return Result;
        }
        #endregion
        public string DeleteOT(string id)
        {
            try
            {
                var objMatch = DB.HRRequestOTs.FirstOrDefault(w => w.OTRNo == id);
                var objAppro_ = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == "REQ_OT").ToList().OrderBy(w => w.ApproveLevel).ToList();
                DB.HRRequestOTs.Attach(objMatch);
                foreach (var item in objAppro_)
                {
                    DB.ExDocApprovals.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;

                }
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
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public class DayInMonth
        {
            public string Empcode { get; set; }
            public int Year { get; set; }
            public int Day { get; set; }
            public string Jan { get; set; }
            public string Feb { get; set; }
            public string Mar { get; set; }
            public string Apr { get; set; }
            public string May { get; set; }
            public string Jun { get; set; }
            public string Jul { get; set; }
            public string Aug { get; set; }
            public string Sep { get; set; }
            public string Oct { get; set; }
            public string Nov { get; set; }
            public string Dec { get; set; }
        }
        public async Task<List<_ListStaff>> LoadData(FTFilterEmployee Filter1, List<HRBranch> _lstBranch)
        {
            DateTime date = new DateTime(1900, 1, 1);
            var _List = new List<_ListStaff>();
            var positions =await DB.HRPositions.ToListAsync();
            var Department = await DB.HRDepartments.ToListAsync();
            var Branch = await SMS.HRBranches.ToListAsync();
            var Section = await DB.HRSections.ToListAsync();
            var _staff = await DB.HRStaffProfiles.ToListAsync();
            _staff = _staff.Where(w => w.StartDate.Value.Date <= Filter1.InDate.Date && (w.DateTerminate.Date == date.Date
            || w.DateTerminate.AddDays(-1) >= Filter1.InDate.Date)).ToList();

            if (_staff.Where(w => w.EmpCode == User.UserName).Any())
            {
                var Staff = _staff.FirstOrDefault(w => w.EmpCode == User.UserName);
                _staff = _staff.Where(w => w.DEPT == Staff.DEPT || w.HODCode == Staff.EmpCode).ToList();
            }
            List<ATEmpSchedule> _att = new List<ATEmpSchedule>();
            if(!string.IsNullOrEmpty(Filter1.Shift))
            {
                _att = await DB.ATEmpSchedules.Where(w => w.SHIFT == Filter1.Shift && w.TranDate == Filter1.InDate.Date).ToListAsync();
            }
            else
            {
                _att = await DB.ATEmpSchedules.Where(w => w.TranDate == Filter1.InDate.Date).ToListAsync();
            }
            _staff = _staff.Where(w => _att.Where(x => x.EmpCode == x.EmpCode).Any()).ToList();

            _staff = _staff.Where(x => _lstBranch.Where(w => w.Code == x.Branch).Any()).ToList();
            if (Filter1.Branch != null)
                _staff = _staff.Where(w => w.Branch == Filter1.Branch).ToList();
            if (Filter1.Division != null)
                _staff = _staff.Where(w => w.Division == Filter1.Division).ToList();
            if (Filter1.Department != null)
                _staff = _staff.Where(w => w.DEPT == Filter1.Department).ToList();
            if (Filter1.Section != null)
                _staff = _staff.Where(w => w.SECT == Filter1.Section).ToList();
            if (Filter1.Position != null)
                _staff = _staff.Where(w => w.JobCode == Filter1.Position).ToList();
            if (Filter1.Level != null)
                _staff = _staff.Where(w => w.LevelCode == Filter1.Level).ToList();
            foreach (var item in _staff)
            {
                var s = _staff.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                var EmpStaff = new _ListStaff();
                EmpStaff.EmpCode = item.EmpCode;
                EmpStaff.AllNameKH = item.OthAllName;
                EmpStaff.AllNameENG = item.AllName;
                if (Branch.Where(w => w.Code == item.Branch).Any())
                    EmpStaff.Branch = Branch.FirstOrDefault(w => w.Code == item.Branch).Description;
                if (Department.Where(w => w.Code == item.DEPT).Any())
                    EmpStaff.Department = Department.FirstOrDefault(w => w.Code == item.DEPT).Description;
                if (positions.Where(w => w.Code == item.JobCode).Any())
                    EmpStaff.Position = positions.FirstOrDefault(w => w.Code == item.JobCode).Description;
                if (Section.Where(w => w.Code == item.SECT).Any())
                    EmpStaff.Section = Section.FirstOrDefault(w => w.Code == item.SECT).Description;
                EmpStaff.Sex = item.Sex;
                _List.Add(EmpStaff);
            }
            return _List.OrderBy(w => w.EmpCode).ToList();
        }
        public string Request(string EmpCode, FTFilterEmployee _Filter1)
        {
            string ErrorCode = "";
            try
            {
                try
                {
                    DB = new HumicaDBContext();
                    if (string.IsNullOrEmpty(_Filter1.OTCategory)) return "INV_OTCATEGORY";
                    DB.Configuration.AutoDetectChangesEnabled = false;
                    DateTime InMonth = new DateTime(_Filter1.InMonth.Year, _Filter1.InMonth.Month, 1);
                    DateTime LastDayofMonth = (new DateTime(_Filter1.InMonth.Year, _Filter1.InMonth.Month, 1).AddMonths(1)).AddDays(-1);
                    string[] Emp = EmpCode.Split(';');
                    var ListStaff = DB.HRStaffProfiles.ToList();
                    var LitsV_Staff = DBV.HR_STAFF_VIEW.Where(w => w.StatusCode == "A").ToList();
                    var ListStaffOT = new List<HRRequestOT>();
                    if (string.IsNullOrEmpty(_Filter1.Remark) || _Filter1.Remark == "null")
                        return "REASON";
                    _Filter1.StartTime = new DateTime(_Filter1.InDate.Year, _Filter1.InDate.Month, _Filter1.InDate.Day, _Filter1.StartTime.Hour, _Filter1.StartTime.Minute, 0);
                    _Filter1.EndTime = new DateTime(_Filter1.InDate.Year, _Filter1.InDate.Month, _Filter1.InDate.Day, _Filter1.EndTime.Hour, _Filter1.EndTime.Minute, 0);
                    _Filter1.TotalHours = Math.Round(Convert.ToDecimal(_Filter1.EndTime.Subtract(_Filter1.StartTime).TotalHours), 2) - _Filter1.BreakTime;
                    foreach (var Code in Emp)
                    {
                        ErrorCode = Code;
                        if (Code.Trim() == "") continue;
                        var Emp_ = DB.HRRequestOTs.Where(w => w.EmpCode == Code).ToList();
                        Emp_ = Emp_.Where(w => _Filter1.StartTime >= w.OTStartTime && _Filter1.StartTime <= w.OTEndTime
                                            || _Filter1.EndTime >= w.OTStartTime && _Filter1.EndTime <= w.OTEndTime).ToList();
                        if (Emp_.Count > 0)
                        {
                            MessageError = Code;
                            return "INV_DATE";
                        }
                        var Staff = ListStaff.FirstOrDefault(x => x.EmpCode == Code);
                        var StaffReq = ListStaff.FirstOrDefault(w => w.EmpCode == Code);
                        if (_Filter1.TotalHours <= 0)
                        {
                            return "INVALID_HOUR";
                        }
                        DateTime DateNow = DateTime.Now;
                        var objAtten = DB.ATEmpSchedules.FirstOrDefault(w => w.EmpCode == Code && EntityFunctions.TruncateTime(w.TranDate) == EntityFunctions.TruncateTime(_Filter1.StartTime));
                        if (objAtten != null)
                        {
                            if (_Filter1.OTCategory == "OT")
                            {

                                if (objAtten.Flag1 == 1 && objAtten.Flag2 == 2)
                                {
                                    if (_Filter1.StartTime < objAtten.OUT1)
                                    {
                                        return "INVALIED_TIME_OT";
                                    }
                                    else if (DateNow.Date != objAtten.TranDate.Date)
                                    {
                                        if (objAtten.End1.Value < _Filter1.EndTime)
                                        {
                                            return "INVALIED_TIME_OT";
                                        }
                                    }
                                }
                                else if (objAtten.Flag1 == 1 && objAtten.Flag2 == 1)
                                {
                                    if (_Filter1.StartTime < objAtten.OUT2)
                                    {
                                        return "INVALIED_TIME_OT";
                                    }
                                    else if (DateNow.Date != objAtten.TranDate.Date)
                                    {
                                        if (objAtten.End2.Value < _Filter1.EndTime)
                                        {
                                            return "INVALIED_TIME_OT";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            return "IVALIED_ROSTER";
                        }
                        var RequestOT = new HRRequestOT();
                        var objNumber = new CFNumberRank(DocType);
                        if (objNumber == null) return "NUMBER_RANK_NE";
                        RequestOT.OTRNo = objNumber.NextNumberRank;
                        string Status = SYDocumentStatus.PENDING.ToString();
                        RequestOT.EmpCode = Code;
                        RequestOT.AllName = Staff.AllName;
                        RequestOT.Status = Status;
                        RequestOT.CreatedBy = User.UserName;
                        RequestOT.CreatedOn = DateTime.Now;
                        RequestOT.Reason = _Filter1.Remark;
                        RequestOT.OTStartTime = _Filter1.StartTime;
                        RequestOT.OTEndTime = _Filter1.EndTime;
                        RequestOT.BreakTime = _Filter1.BreakTime;
                        RequestOT.Hours = _Filter1.TotalHours;
                        RequestOT.RequestBy = _Filter1.RequestBy;
                        var VStaff = LitsV_Staff.FirstOrDefault(w => w.EmpCode == Code);
                        if (VStaff != null)
                        {
                            HeaderOT.Department = VStaff.Department;
                            HeaderOT.Position = VStaff.Position;
                        }
                        DB.HRRequestOTs.Add(RequestOT);
                        ListStaffOT.Add(RequestOT);

                        SetAutoApproval(Staff.EmpCode, _Filter1.EndTime.Date);
                        int line = 1;
                        var read = new ExDocApproval();
                        read.ID = 0;
                        read.LastChangedDate = DateTime.Now;
                        read.DocumentNo = RequestOT.OTRNo;
                        read.Status = SYDocumentStatus.OPEN.ToString();
                        read.Approver = Code;
                        read.ApproverName = Staff.AllName;
                        read.ApprovedBy = "";
                        read.ApprovedName = "";
                        read.ApproveLevel = line;
                        read.DocumentType = "REQ_OT";
                        read.WFObject = "WF02";
                        DB.ExDocApprovals.Add(read);
                        //
                        foreach (var item in ListApproval)
                        {
                            line += 1;
                            item.ID = 0;
                            item.LastChangedDate = DateTime.Now;
                            item.DocumentNo = RequestOT.OTRNo;
                            item.Status = SYDocumentStatus.OPEN.ToString();
                            item.ApprovedBy = "";
                            item.ApprovedName = "";
                            item.ApproveLevel = line;
                            DB.ExDocApprovals.Add(item);
                        }
                        // Announcement
                        var ReqStaff = ListStaff.FirstOrDefault(w => w.EmpCode == Code);
                        SYHRAnnouncement _announ = new SYHRAnnouncement();

                        _announ.Type = "OTRequest";
                        _announ.Subject = RequestOT.AllName;
                        _announ.Description = ReqStaff.Title + " " + ReqStaff.AllName + "\nDear " + RequestOT.AllName + "\nI would like to request you for overtime on " + RequestOT.OTStartTime.Date.ToString("dd/MM/yyyy") +
                                              "\nfrom " + RequestOT.OTStartTime.TimeOfDay.Hours.ToString() + ":" + RequestOT.OTStartTime.TimeOfDay.Minutes.ToString()
                                              + " to " + RequestOT.OTEndTime.TimeOfDay.Hours.ToString() + ":" + RequestOT.OTEndTime.TimeOfDay.Minutes.ToString()
                                              + " Reason: " + RequestOT.Reason;
                        _announ.DocumentNo = RequestOT.OTRNo;
                        _announ.DocumentDate = DateTime.Now;
                        _announ.IsRead = false;
                        _announ.UserName = RequestOT.EmpCode;
                        _announ.CreatedBy = User.UserName;
                        _announ.CreatedOn = DateTime.Now;
                        DB.SYHRAnnouncements.Add(_announ);

                        var Staff_ = ListStaff.FirstOrDefault(x => x.EmpCode == Code);
                        var StaffOT = ListStaffOT.FirstOrDefault(w => w.EmpCode == Staff_.EmpCode);
                        var Template_Req = SMS.TPEmailTemplates.Find("REQUEST_STAFFOT");
                        if (Template_Req != null)
                        {
                            if (!string.IsNullOrEmpty(Staff_.TeleChartID))
                            {
                                SYSendTelegramObject Tel = new SYSendTelegramObject();
                                Tel.User = User;
                                Tel.BS = BS;
                                List<object> ListObjectDictionary = new List<object>();
                                ListObjectDictionary.Add(StaffOT);
                                ListObjectDictionary.Add(Staff_);
                                WorkFlowResult result2 = Tel.Send_SMS_Telegram(Template_Req.EMTemplateObject, Template_Req.RequestContent, Staff_.TeleChartID, ListObjectDictionary, "");
                                MessageError = Tel.getErrorMessage(result2);
                            }
                        }
                        
                        DB.SaveChanges();
                        // Notification
                        var clientToken = new List<string>();
                        var access = DB.TokenResources.FirstOrDefault(w => w.UserName == _announ.UserName);
                        if (access != null)
                        {
                            if (!string.IsNullOrEmpty(access.FirebaseID))
                            {
                                Notification.Notificationf Noti = new Notification.Notificationf();
                                clientToken.Add(access.FirebaseID);
                                var dd = Noti.SendNotification(clientToken, "OTRequest", _announ.Description);
                            }
                        }
                    }


                    return SYConstant.OK;

                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = EmpCode;
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
                    log.DocurmentAction = EmpCode;
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
                    log.DocurmentAction = EmpCode;
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string SendtoTelegram(string EmpCode, FTFilterEmployee Filter1, string fileName,string URL)
        {
            try
            {
                #region ---Send To Telegram---
                var EmailTemplate = SMS.TPEmailTemplates.Find("ESSOT_TELEGRAM");
                if (EmailTemplate != null)
                {
                    SYSendTelegramObject Tel = new SYSendTelegramObject();
                    Tel.User = User;
                    var staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == User.UserName);
                    if(staff == null) 
                    {
                        return "Telegram cannot send)";
                    }
                    Tel.BS = BS;
                    List<object> ListObjectDictionary = new List<object>();
                    if (!string.IsNullOrEmpty(staff.TeleGroupOT))
                    {
                        WorkFlowResult result1 = Tel.Send_SMS_TelegramOT(EmailTemplate.RequestContent, staff.TeleGroupOT, fileName, ListObjectDictionary, URL);
                        MessageError = Tel.getErrorMessage(result1);
                    }
                    else
                        return "Pls set Telegram OT in Function Staff Profile";

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
                //log.DocurmentAction = Filter.INYear.ToString();
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
                //log.DocurmentAction = Filter.INYear.ToString();
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
                // log.DocurmentAction = Filter.INYear.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }

        }
        public Dictionary<string, dynamic> OnDataSelectorPolicy(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("OTCATEGORY_SELECT", ClsFilterGeneral.LoadDataOTCategory());

            return keyValues;
        }
    }
    public class _ListStaff
    {
        public string EmpCode { get; set; }
        public string AllNameKH { get; set; }
        public string AllNameENG { get; set; }
        public string Sex { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Section { get; set; }
        public string FirstLine { get; set; }
        public string SecondLine { get; set; }
    }
    public class ClsReuestOT
    {
        public string OTRNo { get; set; }
        public string Remark { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public DateTime OTStartTime { get; set; }
        public DateTime OTEndTime { get; set; }
        public decimal? OTHour { get; set; }
        public decimal? BreakTime { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }

}