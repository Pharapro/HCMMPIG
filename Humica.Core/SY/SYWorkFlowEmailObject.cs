using Humica.Core.BS;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;

namespace Humica.Core.SY
{
    public class SYWorkFlowEmailObject
    {
        public SYUser User { get; set; }
        public UserType WFFirm { get; set; }
        public SYUserBusiness BS { get; set; }
        public int WorkFlowLevel { get; set; }
        public WorkFlowType WFType { get; set; }
        public SLevel LevelAction { get; set; }
        public string ScreenId { get; set; }
        public object ObjectDictionary { get; set; }
        public string DocumentNo { get; set; }
        public SYSplitItem SelectListItem { get; set; }
        public bool IsWholeDocument { get; set; }
        public string Module { get; set; }
        public string UrlSecondView { get; set; }
        public string UrlView { get; set; }
        public string Action { get; set; }
        public string CompanyCode { get; set; }
        public string NotifcationSubject { get; set; }
        public SYNotification Notification { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string ChangedBy { get; set; }
        public string PathAttachment { get; set; }
        public string FileName { get; set; }
        public Nullable<DateTime> ChangedDate { get; set; }
        private Humica.EF.MD.SMSystemEntity DB = new Humica.EF.MD.SMSystemEntity();
        public BSWorkFlow BSHeader { get; set; }
        public BSWorkFlowItem BSItem { get; set; }
        public List<BSWorkFlowItem> ListBSItems { get; set; }
        public List<BSWorkAssign> ListLineRef { get; set; }
        public CFWorkFlow CFHeader { get; set; }
        //   public List<MDDealer> ListDealer { get; set; }
        public CFWorkFlowCompany CFHeaderDLR { get; set; }
        public CFWorkFlowItem CFItem { get; set; }
        public List<CFWorkFlowItem> ListCFItems { get; set; }
        public TPEmailTemplate EmailTemplate { get; set; }
        public CFEmailAccount EmailAccount { get; set; }
        public ClsEmail EmailObject { get; set; }
        public string WorkFlowObject { get; set; }
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public List<CFWorkFlowApprover> ListApprover { get; set; }
        public List<object> ListObjectDictionary { get; set; }
        public string DocType { get; set; }
        public string ErrorMessage { get; set; }
        public string DocNo { get; set; }
        //workflow NCX normal
        public SYWorkFlowEmailObject(string WFObject, WorkFlowType WFType, UserType WFUserType, string actionProcess)
        {
            CFHeader = DB.CFWorkFlows.Find(WFObject);
            if (CFHeader != null)
            {
                WFFirm = WFUserType;
                string app = WorkFlowType.APPROVER.ToString();
                if (WFFirm == UserType.N)
                {
                    app = WorkFlowType.APPROVER.ToString();
                }
                else
                {
                    app = WorkFlowType.RECEIVER.ToString();
                }

                //ListApprover = DB.CFWorkFlowApprovers.Where(w => w.WFObject == WFObject && w.WFType == app).ToList();

                CFItem = DB.CFWorkFlowItems.Find(WFObject, WFType.ToString(), actionProcess);
                this.WFType = WFType;
                if (CFItem != null)
                {
                    EmailTemplate = DB.TPEmailTemplates.Find(CFItem.EMTemplate);
                    if (EmailTemplate != null)
                    {
                        EmailAccount = DB.CFEmailAccounts.Find(EmailTemplate.StmpObject);
                    }
                }
            }
        }

        public WorkFlowResult ApproProcessWorkFlow(HRStaffProfile Approver, string[] filename, string subject_, string body_, bool isNotSendLastApp = false)
        {
            try
            {
                string err = null;
                if (CFHeader == null)
                {
                    err = "NO_WF";
                }
                if (CFItem == null)
                {
                    err = "NO_WF_ITEM";
                }
                if (CFHeader.IsActive != true)
                {
                    return WorkFlowResult.COMPLETED;
                }
                BSDocConfg doc = new BSDocConfg("OWF_N01", DocConfType.Normal, true);
                if (doc == null)
                {
                    err = "NO_WF_NUMBER";
                }
                string userID = "";
                //var objStaffUser = DB.CFStaffUsers.Where(w => w.StaffID == Approver.StaffID).ToList();
                //if (objStaffUser.Count > 0)
                //{
                //    var objUser = DB.SYUsers.Find(objStaffUser.First().UserID);
                //    if (objUser != null)
                //    {
                //        userID = objUser.UserName;
                //    }
                //}

                Humica.EF.MD.SMSystemEntity DBA = new Humica.EF.MD.SMSystemEntity();

                BSHeader = new BSWorkFlow();
                BSHeader.ScreenId = ScreenId;

                BSHeader.Module = Module;
                BSHeader.WFType = WFType.ToString();
                BSHeader.CreatedBy = User.UserName;
                BSHeader.CreatedOn = DateTime.Now;
                BSHeader.WFObjectNo = doc.NextNumberRank;
                BSHeader.WFObject = CFHeader.WFObject;
                BSHeader.EmailContact = CFItem.EmailContact;
                BSHeader.PhoneContact = CFItem.PhoneContact;
                BSHeader.WFLevel = CFItem.WFLevel;
                BSHeader.SLevel = WFFirm.ToString();
                BSHeader.CompanyCode = User.CompanyOwner;
                BSHeader.Action = Action;
                BSHeader.PersonInCharge = User.LoginName;
                //find signature
                //var cfObj = DB.CFStaffUsers.Find(User.CompanyOwner, User.UserID);
                //if (cfObj != null)
                //{
                //    var objStaff = DB.Staffs.Find(cfObj.StaffID, User.CompanyOwner);
                //    if (objStaff.Signature != null)
                //    {
                //        BSHeader.Signature = UrlView + objStaff.Signature.Replace("~", "");
                //    }
                //}

                //add notification screen
                Notification = new SYNotification();
                Notification.WFObjectNo = BSHeader.WFObjectNo;
                Notification.UserRequest = User.UserName;

                if (WFType == WorkFlowType.REQUESTER)
                {
                    Notification.UrlLink = UrlView + CFHeader.URLApproverBaseView + "?batch=" + BSHeader.WFObjectNo;
                }
                else if (WFType == WorkFlowType.APPROVER)
                {
                    Notification.UrlLink = UrlView + CFHeader.URLReleaseBaseView + "?batch=" + BSHeader.WFObjectNo;
                }
                else if (WFType == WorkFlowType.RECEIVER)
                {
                    Notification.UrlLink = UrlView + CFHeader.URLResponeBaseView + "?batch=" + BSHeader.WFObjectNo;
                }
                Notification.ScreenId = ScreenId;
                Notification.DocumentNo = Notification.WFObjectNo;
                Notification.Text = CFHeader.Description;
                Notification.NotifyDate = DateTime.Now;
                Notification.ObjectCode = BSHeader.CompanyCode;
                Notification.IsRead = false;
                Notification.IsClick = false;

                Notification.UserTo = userID;
                BSHeader.ToUser = userID;
                DBA.SYNotifications.Add(Notification);

                DBA.BSWorkFlows.Add(BSHeader);

                if (SelectListItem.ListDocument.Count > 0)
                {
                    BSHeader.DocumentNo = SelectListItem.ListDocument.First().DocumentNo;
                    int i = 0;
                    foreach (var read in SelectListItem.ListDocument)
                    {
                        i++;
                        BSWorkFlowItem wfi = new BSWorkFlowItem();
                        wfi.WFObjectNo = BSHeader.WFObjectNo;
                        wfi.DocumentNo = read.DocumentNo;
                        wfi.Item = i;
                        wfi.CompanyCode = BSHeader.CompanyCode;
                        wfi.WFLevel = (byte)BSHeader.WFLevel;
                        wfi.WFObjectNo = BSHeader.WFObjectNo;
                        wfi.ScreenId = BSHeader.ScreenId;
                        wfi.Action = BSHeader.Action;
                        if (BSHeader.Action == WorkFlowAction.APPROVED.ToString())
                        {
                            wfi.IsItemApprove = true;
                        }
                        DBA.BSWorkFlowItems.Add(wfi);
                    }
                }

                int row = DBA.SaveChanges();
                if (WFFirm == UserType.N)
                {
                    if (EmailAccount != null)
                    {
                        string mailTo = null;
                        string mailCc = null;

                        //EmailObject = new ClsEmail();
                        //EmailObject.SmtpHost = EmailAccount.SMTPHostName;
                        //EmailObject.SmtpName = EmailAccount.UserName;
                        //EmailObject.SmtpPort = EmailAccount.SMTPPort.Value.ToString();
                        //EmailObject.SmtpPassword = EmailAccount.Password;
                        //EmailObject.MailFrom = EmailAccount.EmailAccount;
                        // EmailObject.MailCC = User.Email;
                        //EmailObject.MailTo = Approver.Email;

                        mailTo = isNotSendLastApp == true ? MailTo : Approver.Email ?? MailTo;

                        //EmailObject.IsEnableSSL = EmailAccount.IsEnableSSL.Value;

                        if (CFItem.MailCCAfterResponse != null)
                        {
                            if (mailCc == null || mailCc == "")
                            {
                                mailCc = CFItem.MailCCAfterResponse;
                            }
                            else
                            {
                                mailCc += ";" + CFItem.MailCCAfterResponse;
                            }
                        }

                        //EmailObject.Subject = getEmailContentByParam(EmailTemplate.RequestSubject);
                        //EmailObject.Body = getEmailContentByParam(EmailTemplate.RequestContent);
                        //EmailObject.FilePath = this.PathAttachment;
                        //EmailObject.FileName = this.FileName;
                        //EmailObject.ScreenId = ScreenId;
                        //EmailObject.DocumentNo = documentNo;

                        if (string.IsNullOrEmpty(mailTo))
                        {
                            ErrorMessage = SYMessages.getMessage("NO_MAIL_TO");
                            return WorkFlowResult.EMAIL_NOT_SEND;
                        }

                        CFEmailAccount emailAccount = EmailAccount;
                        //string subject = getEmailContentByParam(EmailTemplate.RequestSubject);
                        //string body = getEmailContentByParam(EmailTemplate.RequestContent);
                        string subject = subject_;
                        string body = getEmailContentByParam(body_);
                        //PathAttachment = filename;
                        string filePath = PathAttachment;
                        string fileName = Path.GetFileName(filePath);

                        EmailObject = new ClsEmail();
                        int rs = EmailObject.SendMails(emailAccount, mailTo, mailCc, subject, body, filename);
                        if (rs == 0)
                        {
                            return WorkFlowResult.COMPLETED;
                        }
                    }
                }

                ErrorMessage = SYMessages.getMessage("MAIL_CON_ERR");
                return WorkFlowResult.EMAIL_NOT_SEND;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Action;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return WorkFlowResult.ERROR;
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Action;
                log.DocurmentAction = Action;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return WorkFlowResult.ERROR;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Action;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                return WorkFlowResult.ERROR;
            }
        }
        //Staff
        public WorkFlowResult InsertProcessWorkFlow(HRStaffProfile Approver, bool isNotSendLastApp = false)
        {
            try
            {
                string err = null;
                if (CFHeader == null)
                {
                    err = "NO_WF";
                }
                if (CFItem == null)
                {
                    err = "NO_WF_ITEM";
                }
                if (CFHeader.IsActive != true)
                {
                    return WorkFlowResult.COMPLETED;
                }
                BSDocConfg doc = new BSDocConfg("OWF_N01", DocConfType.Normal, true);
                if (doc == null)
                {
                    err = "NO_WF_NUMBER";
                }
                string userID = "";
                //var objStaffUser = DB.CFStaffUsers.Where(w => w.StaffID == Approver.StaffID).ToList();
                //if (objStaffUser.Count > 0)
                //{
                //    var objUser = DB.SYUsers.Find(objStaffUser.First().UserID);
                //    if (objUser != null)
                //    {
                //        userID = objUser.UserName;
                //    }
                //}

                Humica.EF.MD.SMSystemEntity DBA = new Humica.EF.MD.SMSystemEntity();

                BSHeader = new BSWorkFlow();
                BSHeader.ScreenId = ScreenId;

                BSHeader.Module = Module;
                BSHeader.WFType = WFType.ToString();
                BSHeader.CreatedBy = User.UserName;
                BSHeader.CreatedOn = DateTime.Now;
                BSHeader.WFObjectNo = doc.NextNumberRank;
                BSHeader.WFObject = CFHeader.WFObject;
                BSHeader.EmailContact = CFItem.EmailContact;
                BSHeader.PhoneContact = CFItem.PhoneContact;
                BSHeader.WFLevel = CFItem.WFLevel;
                BSHeader.SLevel = WFFirm.ToString();
                BSHeader.CompanyCode = User.CompanyOwner;
                BSHeader.Action = Action;
                BSHeader.PersonInCharge = User.LoginName;
                //find signature
                //var cfObj = DB.CFStaffUsers.Find(User.CompanyOwner, User.UserID);
                //if (cfObj != null)
                //{
                //    var objStaff = DB.Staffs.Find(cfObj.StaffID, User.CompanyOwner);
                //    if (objStaff.Signature != null)
                //    {
                //        BSHeader.Signature = UrlView + objStaff.Signature.Replace("~", "");
                //    }
                //}

                //add notification screen
                Notification = new SYNotification();
                Notification.WFObjectNo = BSHeader.WFObjectNo;
                Notification.UserRequest = User.UserName;

                if (WFType == WorkFlowType.REQUESTER)
                {
                    Notification.UrlLink = UrlView + CFHeader.URLApproverBaseView + "?batch=" + BSHeader.WFObjectNo;
                }
                else if (WFType == WorkFlowType.APPROVER)
                {
                    Notification.UrlLink = UrlView + CFHeader.URLReleaseBaseView + "?batch=" + BSHeader.WFObjectNo;
                }
                else if (WFType == WorkFlowType.RECEIVER)
                {
                    Notification.UrlLink = UrlView + CFHeader.URLResponeBaseView + "?batch=" + BSHeader.WFObjectNo;
                }
                Notification.ScreenId = ScreenId;
                Notification.DocumentNo = Notification.WFObjectNo;
                Notification.Text = CFHeader.Description;
                Notification.NotifyDate = DateTime.Now;
                Notification.ObjectCode = BSHeader.CompanyCode;
                Notification.IsRead = false;
                Notification.IsClick = false;

                Notification.UserTo = userID;
                BSHeader.ToUser = userID;
                DBA.SYNotifications.Add(Notification);

                DBA.BSWorkFlows.Add(BSHeader);

                if (SelectListItem.ListDocument.Count > 0)
                {
                    BSHeader.DocumentNo = SelectListItem.ListDocument.First().DocumentNo;
                    int i = 0;
                    foreach (var read in SelectListItem.ListDocument)
                    {
                        i++;
                        BSWorkFlowItem wfi = new BSWorkFlowItem();
                        wfi.WFObjectNo = BSHeader.WFObjectNo;
                        wfi.DocumentNo = read.DocumentNo;
                        wfi.Item = i;
                        wfi.CompanyCode = BSHeader.CompanyCode;
                        wfi.WFLevel = (byte)BSHeader.WFLevel;
                        wfi.WFObjectNo = BSHeader.WFObjectNo;
                        wfi.ScreenId = BSHeader.ScreenId;
                        wfi.Action = BSHeader.Action;
                        if (BSHeader.Action == WorkFlowAction.APPROVED.ToString())
                        {
                            wfi.IsItemApprove = true;
                        }
                        DBA.BSWorkFlowItems.Add(wfi);
                    }
                }

                int row = DBA.SaveChanges();
                if (WFFirm == UserType.N)
                {
                    if (EmailAccount != null)
                    {
                        string mailTo = null;
                        string mailCc = null;

                        //EmailObject = new ClsEmail();
                        //EmailObject.SmtpHost = EmailAccount.SMTPHostName;
                        //EmailObject.SmtpName = EmailAccount.UserName;
                        //EmailObject.SmtpPort = EmailAccount.SMTPPort.Value.ToString();
                        //EmailObject.SmtpPassword = EmailAccount.Password;
                        //EmailObject.MailFrom = EmailAccount.EmailAccount;
                        // EmailObject.MailCC = User.Email;
                        //EmailObject.MailTo = Approver.Email;

                        mailTo = isNotSendLastApp == true ? MailTo : Approver.Email ?? MailTo;

                        //EmailObject.IsEnableSSL = EmailAccount.IsEnableSSL.Value;

                        if (CFItem.MailCCAfterResponse != null)
                        {
                            if (mailCc == null || mailCc == "")
                            {
                                mailCc = CFItem.MailCCAfterResponse;
                            }
                            else
                            {
                                mailCc += ";" + CFItem.MailCCAfterResponse;
                            }
                        }

                        //EmailObject.Subject = getEmailContentByParam(EmailTemplate.RequestSubject);
                        //EmailObject.Body = getEmailContentByParam(EmailTemplate.RequestContent);
                        //EmailObject.FilePath = this.PathAttachment;
                        //EmailObject.FileName = this.FileName;
                        //EmailObject.ScreenId = ScreenId;
                        //EmailObject.DocumentNo = documentNo;

                        if (string.IsNullOrEmpty(mailTo))
                        {
                            ErrorMessage = SYMessages.getMessage("NO_MAIL_TO");
                            return WorkFlowResult.EMAIL_NOT_SEND;
                        }

                        CFEmailAccount emailAccount = EmailAccount;
                        string subject = getEmailContentByParam(EmailTemplate.RequestSubject);
                        string body = getEmailContentByParam(EmailTemplate.RequestContent);
                        string filePath = PathAttachment;
                        string fileName = Path.GetFileName(filePath);

                        EmailObject = new ClsEmail();
                        int rs = EmailObject.SendMail(emailAccount, mailTo, mailCc,
                            subject, body, filePath, fileName);
                        if (rs == 0)
                        {
                            return WorkFlowResult.COMPLETED;
                        }
                    }
                }

                ErrorMessage = SYMessages.getMessage("MAIL_CON_ERR");
                return WorkFlowResult.EMAIL_NOT_SEND;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Action;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return WorkFlowResult.ERROR;
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Action;
                log.DocurmentAction = Action;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return WorkFlowResult.ERROR;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Action;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                return WorkFlowResult.ERROR;
            }
        }
        // Leave Request
        public WorkFlowResult InsertProcessWorkFlowLeave(HRStaffProfile Approver, bool isNotSendLastApp = false)
        {
            try
            {
                string err = null;
                if (CFHeader == null)
                {
                    err = "NO_WF";
                }
                if (CFItem == null)
                {
                    err = "NO_WF_ITEM";
                }
                if (CFHeader.IsActive != true)
                {
                    return WorkFlowResult.COMPLETED;
                }

                Humica.EF.MD.SMSystemEntity DBA = new Humica.EF.MD.SMSystemEntity();
                BSHeader = new BSWorkFlow();
                Notification = new SYNotification();
                if (WFType == WorkFlowType.REQUESTER)
                {
                    Notification.UrlLink = UrlView + CFHeader.URLReleaseBaseView + "/Details/" + DocNo;
                }
                else if (WFType == WorkFlowType.APPROVER)
                {
                    Notification.UrlLink = UrlView + CFHeader.URLApproverBaseView + "/Details/" + DocNo;
                }
                if (WFFirm == UserType.N)
                {
                    if (EmailAccount != null)
                    {
                        string mailTo = null;
                        string mailCc = null;

                        //EmailObject = new ClsEmail();
                        //EmailObject.SmtpHost = EmailAccount.SMTPHostName;
                        //EmailObject.SmtpName = EmailAccount.UserName;
                        //EmailObject.SmtpPort = EmailAccount.SMTPPort.Value.ToString();
                        //EmailObject.SmtpPassword = EmailAccount.Password;
                        //EmailObject.MailFrom = EmailAccount.EmailAccount;
                        //EmailObject.MailCC = User.Email;
                        //EmailObject.MailTo = Approver.Email;

                        mailTo = isNotSendLastApp == true ? MailTo : Approver.Email ?? MailTo;

                        //EmailObject.IsEnableSSL = EmailAccount.IsEnableSSL.Value;

                        if (CFItem.MailCCAfterResponse != null)
                        {
                            if (mailCc == null || mailCc == "")
                            {
                                mailCc = CFItem.MailCCAfterResponse;
                            }
                            else
                            {
                                mailCc += ";" + CFItem.MailCCAfterResponse;
                            }
                        }
                        if (string.IsNullOrEmpty(mailTo))
                        {
                            ErrorMessage = SYMessages.getMessage("NO_MAIL_TO");
                            return WorkFlowResult.EMAIL_NOT_SEND;
                        }

                        CFEmailAccount emailAccount = EmailAccount;
                        string subject = getEmailContentByParamList(EmailTemplate.RequestSubject);
                        string body = getEmailContentByParamList(EmailTemplate.RequestContent);
                        string filePath = PathAttachment;
                        string fileName = Path.GetFileName(filePath);

                        EmailObject = new ClsEmail();
                        int rs = EmailObject.SendMailNew(emailAccount, mailTo, mailCc,
                            subject, body, filePath, fileName);
                        //EmailObject = new ClsEmail();
                        //int rs = EmailObject.SendMail(emailAccount, mailTo, mailCc,
                        //    subject, body, filePath, fileName);

                        //EmailObject.Subject = EmailTemplate.RequestSubject;
                        //EmailObject.Body = getEmailContentByParamList(EmailTemplate.RequestContent);
                        //int rs = EmailObject.SendEmail();
                        if (rs == 0)
                        {
                            return WorkFlowResult.COMPLETED;
                        }
                    }
                }
                ErrorMessage = SYMessages.getMessage("MAIL_CON_ERR");
                return WorkFlowResult.EMAIL_NOT_SEND;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Action;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return WorkFlowResult.ERROR;
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Action;
                log.DocurmentAction = Action;
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return WorkFlowResult.ERROR;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Action;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                return WorkFlowResult.ERROR;
            }
        }
        public string getEmailContent(string text)
        {
            string number = SelectListItem.ListDocument.Count.ToString();
            string batch = BSHeader.WFObjectNo;
            string link = Notification.UrlLink;
            text = text.Replace("@NUMBER", number);
            text = text.Replace("@LINK", link);
            text = text.Replace("@BATCH_NUMBER", batch);
            return text;
        }
        public string getEmailContentByParam(string text)
        {
            string[] textsp = text.Split(' ');
            if (textsp.LongLength > 0)
            {
                foreach (string param in textsp)
                {
                    if (param.Trim() == "") continue;
                    if (param.Substring(0, 1) == "@")
                    {
                        var objParam = DB.SYEmailParameters.FirstOrDefault(w => w.Parameter == param
                        && w.TemplateID == EmailTemplate.EMTemplateObject);
                        if (objParam != null)
                        {
                            if (ListObjectDictionary.Count > 0)
                            {
                                string textstr = ClsInformation.GetFieldValues(objParam.ObjectName, ListObjectDictionary, objParam.FieldName, param);
                                if (textstr != null)
                                {
                                    text = text.Replace(param, textstr);
                                }
                            }
                        }
                    }

                }
            }
            string number = SelectListItem.ListDocument.Count.ToString();
            string batch = BSHeader.WFObjectNo;
            string link = Notification.UrlLink;
            text = text.Replace("@NUMBER", number);
            text = text.Replace("@LINK", link);
            text = text.Replace("@BATCH_NUMBER", batch);

            return text;
        }
        public string getEmailContentByParamList(string text)
        {
            string[] textsp = text.Split(' ');
            DateTime DateNow = new DateTime();
            if (textsp.LongLength > 0)
            {
                foreach (string param in textsp)
                {
                    if (param.Trim() == "") continue;
                    if (param.Substring(0, 1) == "@")
                    {
                        var objParam = DB.SYEmailParameters.FirstOrDefault(w => w.Parameter == param
                        && w.TemplateID == EmailTemplate.EMTemplateObject);
                        if (objParam != null)
                        {
                            if (ListObjectDictionary.Count > 0)
                            {
                                string textstr = ClsInformation.GetFieldValues(objParam.ObjectName, ListObjectDictionary, objParam.FieldName, param);
                                if (textstr != null)
                                {
                                    text = text.Replace(param, textstr);
                                    if (objParam.FieldName == "ToDate")
                                    {
                                        DateTime PToDate = DateTime.ParseExact(textstr, "dd.MM.yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                                        // DateNow = Convert.ToDateTime(textstr);
                                        DateNow = PToDate.AddDays(1);
                                    }
                                }
                            }
                        }
                    }

                }

            }
            string number = SelectListItem.ListDocument.Count.ToString();
            string batch = BSHeader.WFObjectNo;
            string link = Notification.UrlLink;
            text = text.Replace("@NUMBER", number);
            text = text.Replace("@NowDate", DateNow.ToString("dd.MM.yyyy"));
            text = text.Replace("@LINK", link);
            text = text.Replace("@BATCH_NUMBER", batch);

            return text;
        }

        public static bool IsDealerStatusSign(string Status)
        {
            return Status == SYDocumentStatus.RELEASED.ToString()
|| Status == SYDocumentStatus.RECEIVED.ToString()
|| Status == SYDocumentStatus.RELEASED.ToString()
|| Status == SYDocumentStatus.COMPLETED.ToString()
|| Status == SYDocumentStatus.CANCELLED.ToString();
        }

        public static bool IsDealerStatusSignLevel(string level)
        {
            if (level != SLevel.D.ToString())
            {
                return true;
            }
            return false;
        }
        public string getErrorMessage(WorkFlowResult result)
        {
            if (result == WorkFlowResult.ERROR)
            {
                return SYMessages.getMessage("WF_ERR");
            }
            if (result == WorkFlowResult.EMAIL_NOT_SEND)
            {
                return SYMessages.getMessage("WF_ENS");
            }
            return "";
        }
    }
}