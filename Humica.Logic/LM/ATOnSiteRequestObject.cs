using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Integration.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Policy;
using System.Web.Configuration;
using static System.Net.Mime.MediaTypeNames;

namespace Humica.Logic.LM
{
    public class ATOnSiteRequestObject
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public ATRequestOnSite HeaderEmpOnSite { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public FTINYear FInYear { get; set; }
        public List<ATRequestOnSite> ListEmpOnSite { get; set; }
		public List<ATRequestOnSite> ListEmpOnSitePending { get; set; }
        public List<ATRequestOnSiteItem> ListOnSiteItem { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public List<HRStaffProfile> ListStaff { get; set; }
        public string EmpID { get; set; }
        public string Units { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        public ATOnSiteRequestObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void OnLoadIndex()
        {
            OnLoad();
            ListEmpOnSite = new List<ATRequestOnSite>();
            if (FInYear == null)
            {
                FInYear = new FTINYear();
                var (fromDate, toDate) = DateTimeHelper.GetWeekStartEnd();
                FInYear.FromDate = fromDate;
                FInYear.ToDate = toDate;
            }
            ListEmpOnSite = unitOfWork.Set<ATRequestOnSite>().Where(w => w.FromDate >= FInYear.FromDate.Date
            && w.ToDate <= FInYear.ToDate.Date).ToList();
            if (FInYear.Status != null)
            {
                ListEmpOnSite = ListEmpOnSite.Where(w => w.Status == FInYear.Status).ToList();
            }
            ClsDocApproval docApproval = new ClsDocApproval();
            var ListDoc = docApproval.OnLoadDoc("ON-SITE", User.UserName).ToList();
            ListEmpOnSite = ListEmpOnSite.Where(w => ListDoc.Where(x => x.DocumentNo ==
            w.ID.ToString()).Any()).ToList();
        }
        public void OnLoadPending()
        {
            ListEmpOnSitePending = new List<ATRequestOnSite>();
            string PENDING = SYDocumentStatus.PENDING.ToString();
            ListEmpOnSitePending = unitOfWork.Set<ATRequestOnSite>().Where(w => w.Status == PENDING).ToList();
            ClsDocApproval docApproval = new ClsDocApproval();
            var ListDoc = docApproval.OnLoadDocPending("ON-SITE", User.UserName).ToList();
            ListEmpOnSitePending = ListEmpOnSitePending.Where(w => ListDoc.Where(x => x.DocumentNo ==
            w.ID.ToString()).Any()).ToList();
        }
        public virtual void OnDetailLoading(long ID)
        {
            OnLoad();
            ListOnSiteItem = new List<ATRequestOnSiteItem>();
            HeaderEmpOnSite = unitOfWork.Set<ATRequestOnSite>().FirstOrDefault(w => w.ID == ID);
            if (HeaderEmpOnSite != null)
            {
                ListOnSiteItem = unitOfWork.Set<ATRequestOnSiteItem>().Where(w => w.ID == ID).ToList();
                foreach (var item in ListOnSiteItem.Where(w=>string.IsNullOrEmpty( w.MapURL)))
                {
                    item.MapURL = GetGoogleMapsUrl(Convert.ToDouble(item.Latitude), Convert.ToDouble(item.Longitude));
                }
                ListApproval = unitOfWork.Set<ExDocApproval>().Where(w => w.DocumentNo == ID.ToString()
                && w.DocumentType == "ON-SITE").ToList();
            }
        }
        public string RequestOnsite()
        {
            OnLoad();
            try
            {
                unitOfWork.BeginTransaction();
                if (string.IsNullOrEmpty(HeaderEmpOnSite.EmpCode))
                    return "INV_EMPCODE";
                if (string.IsNullOrEmpty(HeaderEmpOnSite.Reason))
                    return "INV_REASON";
                if (!ListOnSiteItem.Any()) return "INV_ITEM";
                var staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(x => x.EmpCode == HeaderStaff.EmpCode);
                if (staff == null)
                    return "INVALID_STAFF";
                if (HeaderEmpOnSite.FromDate == DateTime.MinValue || HeaderEmpOnSite.ToDate == DateTime.MinValue)
                    return "INVALID_DATE";
                if (HeaderEmpOnSite.FromDate.Date > HeaderEmpOnSite.ToDate.Date)
                    return "INVALID_DATE";
                HeaderEmpOnSite.EmpCode = HeaderStaff.EmpCode;
                HeaderEmpOnSite.EmployeeName = HeaderStaff.AllName;
                HeaderEmpOnSite.CreatedBy = User.UserName;
                HeaderEmpOnSite.CreatedOn = DateTime.Now;
                HeaderEmpOnSite.Status = SYDocumentStatus.OPEN.ToString();
                if (HeaderEmpOnSite.Unit != "Day")
                {
                    if (HeaderEmpOnSite.EndTime < HeaderEmpOnSite.StartTime)
                    {
                        return "Invalid Time";
                    }
                    HeaderEmpOnSite.ToDate = HeaderEmpOnSite.FromDate;
                    HeaderEmpOnSite.StartTime = DateTimeHelper.DateInHourMin(HeaderEmpOnSite.FromDate.Date + HeaderEmpOnSite.StartTime.Value.TimeOfDay);
                    HeaderEmpOnSite.EndTime = DateTimeHelper.DateInHourMin(HeaderEmpOnSite.FromDate.Date + HeaderEmpOnSite.EndTime.Value.TimeOfDay);
                }
                SetAutoApproval(staff.EmpCode, staff.Branch, HeaderEmpOnSite.ToDate.Date);
                if (!ListApproval.Any()) return "NO_LINE_MN";
                unitOfWork.Set<ATRequestOnSite>().Add(HeaderEmpOnSite);
                unitOfWork.Save();
                int LineItem = 0;
                foreach (var item in ListOnSiteItem)
                {
                    item.Location = item.Location.Replace("#", "");
                    LineItem += 1;
                    item.ID = HeaderEmpOnSite.ID;
                    item.LineItem = LineItem;
                    unitOfWork.Add(item);
                }
                foreach (ExDocApproval read in ListApproval)
                {
                    read.ID = 0;
                    read.LastChangedDate = DateTime.Now;
                    read.DocumentNo = HeaderEmpOnSite.ID.ToString();
                    read.Status = SYDocumentStatus.OPEN.ToString();
                    read.ApprovedBy = "";
                    read.ApprovedName = "";
                    unitOfWork.Add(read);
                }
                var Approver = ListApproval.OrderBy(w => w.ApproveLevel).FirstOrDefault();
                SYHRAnnouncement _announ = new SYHRAnnouncement();
                if (ListApproval.Count() > 0)
                {
                    ClsTemplateAlert _alert = new ClsTemplateAlert();
                    string Description = HeaderEmpOnSite.EmpCode + ":" + staff.AllName + @" REVIEW/APPROVAL  of Request On-Site from " + HeaderEmpOnSite.FromDate.ToString("yyyy.MM.dd") + " to " + HeaderEmpOnSite.ToDate.ToString("yyyy.MM.dd") + " My Reason: " + HeaderEmpOnSite.Reason;
                    _announ = _alert.AddAnnouncement(staff.AllName, "Request On-Site", Description, ListApproval.FirstOrDefault().Approver, HeaderEmpOnSite.ID.ToString());
                    unitOfWork.Add(_announ);
                }
                unitOfWork.Save();
                unitOfWork.Commit();
                #region Notifican on Mobile
                var access = unitOfWork.Repository<TokenResource>().Queryable().FirstOrDefault(w => w.UserName == _announ.UserName);
                if (access != null)
                {
                    if (!string.IsNullOrEmpty(access.FirebaseID))
                    {
                        Notification.Notificationf Noti = new Notification.Notificationf();
                        var clientToken = new List<string>();
                        clientToken.Add(access.FirebaseID);
                        var dd = Noti.SendNotification(clientToken, _announ.Type, _announ.Description);
                    }
                }
                #endregion
                #region ---Send To Telegram---
                //TPEmailTemplate EmailTemplate = unitOfWork.Repository<TPEmailTemplate>().Queryable().FirstOrDefault(w => w.EMTemplateObject == "TELEGRAM");
                var HOD = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == Approver.Approver);
                if (!string.IsNullOrEmpty(_announ.Description) && !string.IsNullOrEmpty(HOD.TeleGroup))
                {
                    SYSendTelegramObject Tel = new SYSendTelegramObject();
                    Tel.User = User;
                    Tel.BS = BS;
                    List<object> ListObjectDictionary = new List<object>();
                    WorkFlowResult result2 = Tel.Send_SMS_Telegram("TELEGRAM", _announ.Description, HOD.TeleGroup, ListObjectDictionary, "");
                    MessageError = Tel.getErrorMessage(result2);
                }
                #endregion
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderStaff.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderStaff.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, HeaderStaff.EmpCode, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string approveTheDoc(string ApprovalID, string URL)
        {
            string Error_Status = "";
            OnLoad();
            try
            {
                var APPROVED = SYDocumentStatus.APPROVED.ToString();
                var REJECTED = SYDocumentStatus.REJECTED.ToString();
                var CANCELLED = SYDocumentStatus.CANCELLED.ToString();
                string open = SYDocumentStatus.OPEN.ToString();
                string[] c = ApprovalID.Split(';');
                ListStaff = unitOfWork.Set<HRStaffProfile>().Where(w => w.Status == "A").ToList();
                var listUser = ListStaff.Where(w => w.EmpCode == User.UserName).ToList();
                foreach (var id in c)
                {
                    Error_Status = id;
                    if (id == "") continue;
                    int TranNo = Convert.ToInt32(id);
                    var objMatch = unitOfWork.Set<ATRequestOnSite>().Find(TranNo);
                    if (objMatch == null)
                    {
                        return "REQUEST_NE";
                    }
                    var ListItem = unitOfWork.Set<ATRequestOnSiteItem>().Where(w => w.ID == TranNo).ToList();

                    string DocNo = id.ToString();
                    var listApproval = unitOfWork.Set<ExDocApproval>()
                   .Where(w => w.DocumentType == "ON-SITE" && w.DocumentNo == DocNo)
                   .OrderBy(w => w.ApproveLevel).ToList();
                    var b = false;
                    if (listApproval.Count == 0)
                    {
                        return "RESTRICT_ACCESS";
                    }
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
                            var objStaff = listUser.FirstOrDefault(w => w.EmpCode == read.Approver);
                            if (objStaff != null)
                            {
                                foreach (var item in listApproval.Where(w => w.ApproveLevel == read.ApproveLevel))
                                {
                                    item.ApprovedBy = objStaff.EmpCode;
                                    item.ApprovedName = objStaff.AllName;
                                    item.LastChangedDate = DateTime.Now.Date;
                                    item.ApprovedDate = DateTime.Now;
                                    item.Status = SYDocumentStatus.APPROVED.ToString();
                                    unitOfWork.Update(item);
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
                    HRStaffProfile Staff = ListStaff.FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                    HRStaffProfile StaffNext = new HRStaffProfile();
                    ClsTemplateAlert _alert = new ClsTemplateAlert();
                    SYHRAnnouncement _announ = new SYHRAnnouncement();
                    List<ExDocApproval> _Lstapp = listApproval.Where(w => w.Status == SYDocumentStatus.OPEN.ToString()).ToList();
                    _announ.Type = "Request On-Site";
                    if (_Lstapp.Count() > 0)
                    {
                        string Description = objMatch.EmpCode + ":" + Staff.AllName + @" REVIEW/APPROVAL  of Request On-Site from " + objMatch.FromDate.ToString("yyyy.MM.dd") + " to " + objMatch.ToDate.ToString("yyyy.MM.dd") + " My Reason: " + objMatch.Reason;
                        _announ = _alert.AddAnnouncement(Staff.AllName, "Request On-Site", Description, objMatch.EmpCode, objMatch.ID.ToString());
                        int min = _Lstapp.Min(w => w.ApproveLevel);
                        StaffNext = ListStaff.FirstOrDefault(w => w.EmpCode == _Lstapp.FirstOrDefault(x => x.ApproveLevel == min).Approver);
                        _announ.UserName = _Lstapp.FirstOrDefault(w => w.ApproveLevel == min).Approver;
                    }
                    if (status == SYDocumentStatus.APPROVED.ToString())
                    {
                        _announ = _alert.AddAnnouncement("Approved", "Approved On-Site", "Your request On-Site is Approved", objMatch.EmpCode, objMatch.ID.ToString());
                    }
                    unitOfWork.Add(_announ);

                    objMatch.Status = status;
                    //objMatch.Comment = request.Comment;
                    if (status == SYDocumentStatus.APPROVED.ToString())
                    {
                        objMatch.IsReAlert = null;
                        objMatch.AlertDate = null;
                    }
                    else
                    {
                        DateTime DateNow = DateTime.Now;
                        objMatch.AlertDate = DateNow.AddHours(2);
                        objMatch.IsReAlert = false;
                    }
                    unitOfWork.Update(objMatch);
                    unitOfWork.Save();
                    HRStaffProfile StaffApp = ListStaff.FirstOrDefault(w => w.EmpCode == User.UserName);
                    ClsContent _content = new ClsContent();
                    _content.Requester = objMatch.EmpCode;
                    if (objMatch.Status == SYDocumentStatus.APPROVED.ToString())
                    {
                        _content = _alert.GetSubjectMessage("Approval On-Site", "Approved", StaffApp.Title + ". " + StaffApp.AllName + " (" + StaffApp.EmpCode + ")", Staff.Title + ". " + Staff.AllName + " (" + Staff.EmpCode + ")", Staff.TeleGroup);
                        _content.CompanyCode = StaffApp.CompanyCode;
                        _content.BodyTG = GetMessage_KH(objMatch, ListItem.ToList(), "");
                        _content.TokenID = _alert.GetTokenID(StaffApp.TeleGroup);

                        List<string> clientToken = new List<string>();
                        _content.FirebaseID = _alert.GetFirebaseID(objMatch.EmpCode);
                        _content.BodyAPP = _announ.Description;
                        _content.SubjectNumber = _announ.DocumentNo;
                        _content.SubjectTypeEA = _announ.Type;
                        _content.SubjectEmail = _announ.Type;


                        var i = ClsSendNotification.SendEmail(_content);
                    }
                    else
                    {
                        _content.Subject = "Request On-Site";
                        _content.BodyAPP = _announ.Description;
                        _content.SubjectNumber = _announ.DocumentNo;
                        _content.SubjectTypeEA = _announ.Type;
                        var i = ClsSendNotification.SendEmail(_content);
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Error_Status, SYActionBehavior.RELEASE.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Error_Status, SYActionBehavior.RELEASE.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Error_Status, SYActionBehavior.RELEASE.ToString(), e, true);
            }
        }
        public string RejectTheDoc(string ApprovalID,string comment)
        {
            string Error_Status = "";
            OnLoad();
            if (string.IsNullOrEmpty(comment))
            {
                return "COMMENT_REQUIRED";
            }
            try
            {
                var APPROVED = SYDocumentStatus.APPROVED.ToString();
                var REJECTED = SYDocumentStatus.REJECTED.ToString();
                var CANCELLED = SYDocumentStatus.CANCELLED.ToString();
                string open = SYDocumentStatus.OPEN.ToString();
                string[] c = ApprovalID.Split(';');
                ListStaff = unitOfWork.Set<HRStaffProfile>().Where(w => w.Status == "A").ToList();
                var listUser = ListStaff.Where(w => w.EmpCode == User.UserName).ToList();
                foreach (var id in c)
                {
                    Error_Status = id;
                    if (id == "") continue;
                    int TranNo = Convert.ToInt32(id);
                    var objMatch = unitOfWork.Set<ATRequestOnSite>().Find(TranNo);
                    if (objMatch == null)
                    {
                        return "REQUEST_NE";
                    }
                    string DocNo = id.ToString();
                    var listApproval = unitOfWork.Set<ExDocApproval>()
                   .Where(w => w.DocumentType == "ON-SITE" && w.DocumentNo == DocNo)
                   .OrderBy(w => w.ApproveLevel).ToList();
                    var b = false;
                    if (listApproval.Count == 0)
                    {
                        return "RESTRICT_ACCESS";
                    }
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
                            var objStaff = listUser.FirstOrDefault(w => w.EmpCode == read.Approver);
                            if (objStaff != null)
                            {
                                foreach (var item in listApproval)
                                {
                                    item.ApprovedBy = objStaff.EmpCode;
                                    item.ApprovedName = objStaff.AllName;
                                    item.LastChangedDate = DateTime.Now.Date;
                                    item.ApprovedDate = DateTime.Now;
                                    item.Status = SYDocumentStatus.REJECTED.ToString();
                                    unitOfWork.Update(item);
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
                    var status = SYDocumentStatus.REJECTED.ToString();
                    objMatch.Status = status;
                    objMatch.Comment = comment;
                    ClsTemplateAlert _alert = new ClsTemplateAlert();
                    SYHRAnnouncement _announ = new SYHRAnnouncement();
                    _announ = _alert.AddAnnouncement("Rejected", "Rejected On-Site", "Your request On-Site is Rejected", objMatch.EmpCode, objMatch.ID.ToString());
                    unitOfWork.Add(_announ);
                    //unitOfWork.Update(objMatch);
                    //unitOfWork.Save();
                    HRStaffProfile StaffApp = ListStaff.FirstOrDefault(w => w.EmpCode == User.UserName);
                    HRStaffProfile Staff = ListStaff.FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                    ClsContent _content = new ClsContent();
                    _content.Requester = objMatch.EmpCode;
                    var ListItem = unitOfWork.Set<ATRequestOnSiteItem>().Where(w => w.ID == objMatch.ID);
                    
                    _content = _alert.GetSubjectMessage("Rejected On-Site", "Rejected", StaffApp.Title + ". " + StaffApp.AllName + " (" + StaffApp.EmpCode + ")", StaffApp.Title + ". " + Staff.AllName + " (" + Staff.EmpCode + ")", Staff.TeleGroup);
                    _content.BodyTG = GetMessage_KH(objMatch, ListItem.ToList(),"Rejected");
                    _content.TokenID = _alert.GetTokenID(Staff.TeleGroup);

                    //List<string> clientToken = new List<string>();
                    //_content.FirebaseID = ClsEmailObject.GetFirebaseID(_dbContext, objMatch.EmpCode);
                    _content.BodyAPP = _announ.Description;
                    _content.SubjectNumber = _announ.DocumentNo;
                    _content.SubjectTypeEA = _announ.Type;
                    _content.SubjectEmail = _announ.Type;

                    var i = ClsSendNotification.SendEmail(_content);

                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Error_Status, SYActionBehavior.RELEASE.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Error_Status, SYActionBehavior.RELEASE.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Error_Status, SYActionBehavior.RELEASE.ToString(), e, true);
            }
        }
        public string GetGoogleMapsUrl(double latitude, double longitude)
        {
            return $"https://www.google.com/maps?q={latitude},{longitude}";
        }
        public string GetMessage_KH(ATRequestOnSite OnSite, List<ATRequestOnSiteItem> ListItem, string Action = "")
        {
            string str = "%0A<b>កាលបរិច្ឆេទចេញក្រៅ៖</b> " + OnSite.FromDate.ToString("dd-MMM-yyyy") + " ដល់ " + OnSite.ToDate.ToString("dd-MMM-yyyy");
            str += "%0A<b>មូលហេតុ៖</b> " + OnSite.Reason;
            if (Action != "Rejected")
            {
                foreach (ATRequestOnSiteItem item in ListItem)
                {
                    str += "%0A<b>ទីតាំង៖</b> " + item.Location;
                }
            }
            return str;
        }
        public List<ATRequestOnSiteItem> Get_LeaveDay(string EmpCode, DateTime FromDate, DateTime ToDate, string LeaveCode)
        {
            List<ATRequestOnSiteItem> list = new List<ATRequestOnSiteItem>();
            int line = 1;
            for (DateTime date = FromDate; date <= ToDate; date = date.AddDays(1))
            {
                list.Add(new ATRequestOnSiteItem
                {
                    LineItem = line,
                    InDate = date,
                    Location = "Default Location",
                    Longitude = "",
                    Latitude = "",
                    MapURL = ""
                });

                line++;
            }

            return list;
        }
        public void SetAutoApproval(string EmpCode, string Branch, DateTime ToDate)
        {
            ListApproval = new List<ExDocApproval>();
            var LstStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w => w.Status == "A" || (DbFunctions.TruncateTime(w.DateTerminate) > DbFunctions.TruncateTime(ToDate) && w.Status == "I")).ToList();
            var ListWorkFlow = unitOfWork.Repository<HRWorkFlowLeave>().Queryable().ToList();
            var _staffApp = new HRStaffProfile();
            foreach (var item in ListWorkFlow)
            {
                var Staff = LstStaff.FirstOrDefault(w => w.EmpCode == EmpCode);
                if (item.ApproveBy == "1st")
                {
                    var Read = LstStaff.Where(w => w.EmpCode == Staff.FirstLine).ToList();
                    _staffApp = Read.FirstOrDefault();
                    if (_staffApp != null)
                    {
                        ExDocApproval objApp1 = AddDocApproval(_staffApp, item.Step);
                        ListApproval.Add(objApp1);
                    }
                    HRStaffProfile _staff = LstStaff.FirstOrDefault(w => w.EmpCode == Staff.FirstLine2);
                    if (_staff != null)
                    {
                        ExDocApproval objApp1 = AddDocApproval(_staff, item.Step);
                        ListApproval.Add(objApp1);
                    }
                }
                else if (item.ApproveBy == "2nd")
                {
                    List<HRStaffProfile> Read = LstStaff.Where(w => w.EmpCode == Staff.SecondLine).ToList();
                    _staffApp = Read.FirstOrDefault();
                    if (_staffApp != null)
                    {
                        ExDocApproval objApp1 = AddDocApproval(_staffApp, item.Step);
                        ListApproval.Add(objApp1);
                    }
                    HRStaffProfile _staff = LstStaff.FirstOrDefault(w => w.EmpCode == Staff.SecondLine2);
                    if (_staff != null)
                    {
                        ExDocApproval objApp1 = AddDocApproval(_staff, item.Step);
                        ListApproval.Add(objApp1);
                    }
                }
                else
                {
                    _staffApp = LstStaff.FirstOrDefault(w => w.JobCode == item.ApproveBy && w.Branch == Branch);
                    if (_staffApp == null)
                        _staffApp = LstStaff.FirstOrDefault(w => w.JobCode == item.ApproveBy);
                    if (_staffApp == null) continue;

                    if (ListApproval.Where(w => w.Approver == _staffApp.EmpCode).Count() > 0) continue;
                    var objApp = new ExDocApproval();
                    objApp.Approver = _staffApp.EmpCode;
                    objApp.ApproverName = _staffApp.AllName;
                    objApp.DocumentType = "ON-SITE";
                    objApp.ApproveLevel = item.Step;
                    objApp.WFObject = "WF02";
                    ListApproval.Add(objApp);
                }
            }
        }
        public HRStaffProfile getNextApprover(string id, string pro)
        {
            var objStaff = new HRStaffProfile();
            string open = SYDocumentStatus.OPEN.ToString();
            var listCanApproval = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == id && w.Status == open && w.DocumentType == "ON-SITE").ToList();

            if (listCanApproval.Count == 0)
            {
                return new HRStaffProfile();
            }

            var min = listCanApproval.Min(w => w.ApproveLevel);
            var NextApp = listCanApproval.Where(w => w.ApproveLevel == min).First();
            objStaff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == NextApp.Approver);
            return objStaff;
        }
        public ExDocApproval AddDocApproval(HRStaffProfile Staff, int Step)
        {
            ExDocApproval objApp = new ExDocApproval();
            objApp.Approver = Staff.EmpCode;
            objApp.ApproverName = Staff.AllName;
            objApp.DocumentType = "ON-SITE";
            objApp.ApproveLevel = Step;
            objApp.WFObject = "WF02";

            return objApp;
        }
    }
}