using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace Humica.Logic.Att
{
    public class ATApprovalOnSiteObject
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
        public ATApprovalOnSiteObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void OnLoadIndex()
        {
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
            if(FInYear.Department != null|| FInYear.Position != null)
            {
				var ListStaff = unitOfWork.Set<HRStaffProfile>().ToList();
				if (FInYear.Department != null)
				{
					ListStaff = ListStaff.Where(w => w.DEPT == FInYear.Department).ToList();
					ListEmpOnSite = ListEmpOnSite.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
				}
				if (FInYear.Position != null)
				{
					ListStaff = ListStaff.Where(w => w.JobCode == FInYear.Position).ToList();
					ListEmpOnSite = ListEmpOnSite.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
				}
			}
		}
        public void OnLoadPending()
        {
            ListEmpOnSitePending = new List<ATRequestOnSite>();
            string PENDING = SYDocumentStatus.PENDING.ToString();
            ListEmpOnSitePending = unitOfWork.Set<ATRequestOnSite>().Where(w => w.Status == PENDING).ToList();
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
                    if (listApproval.Count == 0)
                    {
                        return "RESTRICT_ACCESS";
                    }
                    var approver = "";
                    foreach (var read in listApproval)
                    {
						read.ApprovedBy = read.Approver;
                        var checkStaff= ListStaff.FirstOrDefault(s=>s.EmpCode == read.Approver);
                        if (checkStaff != null) 
                        { 
                           read.ApprovedName = checkStaff.AllName;
                        }
						read.LastChangedDate = DateTime.Now.Date;
						read.ApprovedDate = DateTime.Now;
						read.Status = SYDocumentStatus.APPROVED.ToString();
						unitOfWork.Update(read);
					}

                    var status = SYDocumentStatus.APPROVED.ToString();
					objMatch.Status = status;
					objMatch.IsReAlert = null;
					objMatch.AlertDate = null;

					HRStaffProfile Staff = ListStaff.FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
					HRStaffProfile StaffNext = new HRStaffProfile();
                    ClsTemplateAlert _alert = new ClsTemplateAlert();
                    SYHRAnnouncement _announ = new SYHRAnnouncement();
                   // List<ExDocApproval> _Lstapp = listApproval.Where(w => w.Status == SYDocumentStatus.OPEN.ToString()).ToList();
                    //_announ.Type = "Request On-Site";
                    //if (_Lstapp.Count() > 0)
                    //{
                    //    string Description = objMatch.EmpCode + ":" + Staff.AllName + @" REVIEW/APPROVAL  of Request On-Site from " + objMatch.FromDate.ToString("yyyy.MM.dd") + " to " + objMatch.ToDate.ToString("yyyy.MM.dd") + " My Reason: " + objMatch.Reason;
                    //    _announ = _alert.AddAnnouncement(Staff.AllName, "Request On-Site", Description, objMatch.EmpCode, objMatch.ID.ToString());
                    //    int min = _Lstapp.Min(w => w.ApproveLevel);
                    //    StaffNext = ListStaff.FirstOrDefault(w => w.EmpCode == _Lstapp.FirstOrDefault(x => x.ApproveLevel == min).Approver);
                    //    _announ.UserName = _Lstapp.FirstOrDefault(w => w.ApproveLevel == min).Approver;
                    //}
                    if (status == SYDocumentStatus.APPROVED.ToString())
                    {
                        _announ = _alert.AddAnnouncement("Approved", "Approved On-Site", "Your request On-Site is Approved", objMatch.EmpCode, objMatch.ID.ToString());
                    }
                    unitOfWork.Add(_announ);
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
        public string RejectTheDoc(string ApprovalID,string Comment)
        {
            string Error_Status = "";
            OnLoad();
            if (string.IsNullOrEmpty(Comment))
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
                    if (listApproval.Count == 0)
                    {
                        return "RESTRICT_ACCESS";
                    }
                    var approver = "";
                    foreach (var read in listApproval)
                    {
						read.ApprovedBy = read.Approver;
						var checkStaff = ListStaff.FirstOrDefault(s => s.EmpCode == read.Approver);
						if (checkStaff != null)
						{
							read.ApprovedName = checkStaff.AllName;
						}
						read.LastChangedDate = DateTime.Now.Date;
						read.ApprovedDate = DateTime.Now;
						read.Status = SYDocumentStatus.REJECTED.ToString();
						unitOfWork.Update(read);
					}
                 
                    var status = SYDocumentStatus.REJECTED.ToString();
                    objMatch.Status = status;
                    objMatch.Comment = Comment;
                    ClsTemplateAlert _alert = new ClsTemplateAlert();
                    SYHRAnnouncement _announ = new SYHRAnnouncement();
                    _announ = _alert.AddAnnouncement("Rejected", "Rejected On-Site", "Your request On-Site is Rejected", objMatch.EmpCode, objMatch.ID.ToString());
                    unitOfWork.Add(_announ);
                    unitOfWork.Update(objMatch);
                    unitOfWork.Save();
                    HRStaffProfile StaffApp = ListStaff.FirstOrDefault(w => w.EmpCode == User.UserName);
                    HRStaffProfile Staff = ListStaff.FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                    ClsContent _content = new ClsContent();
                    _content.Requester = objMatch.EmpCode;
                    var ListItem = unitOfWork.Set<ATRequestOnSiteItem>().Where(w => w.ID == objMatch.ID);
                    
                    _content = _alert.GetSubjectMessage("Rejected On-Site", "Rejected", StaffApp.Title + ". " + StaffApp.AllName + " (" + StaffApp.EmpCode + ")", Staff.Title + ". " + Staff.AllName + " (" + Staff.EmpCode + ")", Staff.TeleGroup);
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

    }
}