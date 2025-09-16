using DevExpress.Web.Mvc;
using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.LeaveBalance
{

    public class HROTRequestController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000003";
        private const string URL_SCREEN = "/SelfService/MyTeam/HROTRequest/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "OTRNo";
        private string DOCTYPE = "OTR01";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public HROTRequestController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            PROverTimeObject BSM = new PROverTimeObject();

            BSM.ListOTRequest = new List<HRRequestOT>();
            IEnumerable<HRRequestOT> ListOTRequest = DB.HRRequestOTs.ToList();
            string Pending = SYDocumentStatus.PENDING.ToString();
            string Open = SYDocumentStatus.OPEN.ToString();
            IEnumerable<ExDocApproval> ListApp = DB.ExDocApprovals.Where(w => w.DocumentType == "REQ_OT" && w.Status == Open).ToList();
            BSM.ListOTReqPending = new List<ClsReuestOT>();
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListStaff = DBV.HRStaffProfiles.ToList();
            var ListTeamStaff = ListStaff.Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            var ListOTReqPending = new List<HRRequestOT>();
            var staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == user.UserName);
            BSM.ListOTRequest = ListOTRequest.Where(w => ListTeamStaff.Where(x => x.EmpCode == w.EmpCode || x.EmpCode == w.RequestBy).Any()).ToList();
            if (staff != null)
            {
                var _listEmp = DB.HRStaffProfiles.Where(w => w.OTFirstLine == staff.EmpCode || w.OTSecondLine == staff.EmpCode
                                                  || w.HODCode == staff.EmpCode || w.OTthirdLine == staff.EmpCode).ToList();
                BSM.ListOTRequest = ListOTRequest.Where(w => _listEmp.Where(x => x.EmpCode == w.EmpCode || x.EmpCode == w.RequestBy).Any()).ToList();
            }
            ListOTRequest = ListOTRequest.Where(w => w.Status == Pending);
            ListApp = ListApp.Where(w => w.Approver == user.UserName).ToList();
            ListOTRequest = ListOTRequest.Where(x => ListApp.Where(w => w.DocumentNo == x.OTRNo).Any()).ToList();
            foreach (var item in ListOTRequest)
            {
                ExDocApproval UserApprovalDoc = ListApp.Where(w => w.Approver == user.UserName && w.DocumentNo == item.OTRNo).First();
                IEnumerable<ExDocApproval> ListUserApprovalDoc = DB.ExDocApprovals.Where(w => w.DocumentNo == item.OTRNo && w.DocumentType == "REQ_OT");
                ListUserApprovalDoc = ListUserApprovalDoc.Where(w => w.ApproveLevel < UserApprovalDoc.ApproveLevel && w.Status == Open).ToList();
                if (ListUserApprovalDoc.Count() == 0)
                {
                    ClsReuestOT _OT = new ClsReuestOT();
                    _OT.EmpCode = item.EmpCode;
                    _OT.EmpName = item.AllName;
                    _OT.OTStartTime = item.OTStartTime;
                    _OT.OTEndTime = item.OTEndTime;
                    _OT.Reason = item.Reason;
                    _OT.Status = item.Status;
                    _OT.OTRNo = item.OTRNo;
                    BSM.ListOTReqPending.Add(_OT);
                }
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            PROverTimeObject BSM = new PROverTimeObject();
            BSM.ListOTRequest = new List<HRRequestOT>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListOTRequest);
        }
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfList(KeyName);
            PROverTimeObject BSM = new PROverTimeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListOTReqPending);
        }

        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PROverTimeObject BSM = new PROverTimeObject();
            BSM.HeaderOT = new HRRequestOT();
            BSM.HeaderOT.OTStartTime = DateTime.Now;
            BSM.HeaderOT.OTEndTime = DateTime.Now;
            BSM.HeaderOT.Hours = 0;
            BSM.HeaderOT.Status = SYDocumentStatus.PENDING.ToString();
            var staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == user.UserName);
            if (staff != null)
            {
                BSM.HeaderOT.RequestBy = staff.EmpCode;
            }
            else
                BSM.HeaderOT.EmpCode = user.UserName;
            BSM.ListOTRequest = new List<HRRequestOT>();
            BSM.User = SYSession.getSessionUser();
            BSM.DocType = DOCTYPE;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(string ID, PROverTimeObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            var BSM = new PROverTimeObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                BSM.HeaderOT = collection.HeaderOT;
                BSM.DocType = DOCTYPE;
            }

            if (Session[Index_Sess_Obj + ActionName] != null)
                if (!string.IsNullOrEmpty(BSM.HeaderOT.EmpCode))
                {
                    if (Session[PATH_FILE] != null)
                    {
                        //collection.Header.AttachFile = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/HROTRequest/Details/";
                    string msg = BSM.CreateOTReq(URL);

                    if (msg == SYConstant.OK)
                    {
                        BSM.ScreenId = SCREEN_ID;
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        mess.DocumentNumber = BSM.HeaderOT.OTRNo.ToString();
                        mess.Description = mess.Description + BSM.MessageError;
                        mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + BSM.HeaderOT.OTRNo;

                        Session[Index_Sess_Obj + ActionName] = BSM;
                        Session[SYConstant.MESSAGE_SUBMIT] = mess;
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                    }
                    else
                    {
                        ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                        return View(BSM);
                    }
                }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion

        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            PROverTimeObject BSM = new PROverTimeObject();
            BSM.HeaderOT = new HRRequestOT();
            if (!string.IsNullOrEmpty(id))
            {
                BSM.HeaderOT = DB.HRRequestOTs.FirstOrDefault(x => x.OTRNo == id);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, PROverTimeObject collection)
        {
            ActionName = "Edit";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            PROverTimeObject BSM = new PROverTimeObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                }
                if (Session[PATH_FILE] != null)
                {
                    //collection.HeaderOT.AttachFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                else
                {
                    //collection.HeaderOT.AttachFile = BSM.HeaderOT.AttachFile;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                    BSM.HeaderOT = collection.HeaderOT;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditOTReq(id);
                if (msg == SYConstant.OK)
                {
                    DB = new HumicaDBContext();
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return View(BSM);

        }
        #endregion

        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ViewData[SYSConstant.PARAM_ID] = id;
            PROverTimeObject BSM = new PROverTimeObject();
            BSM.HeaderOT = DB.HRRequestOTs.FirstOrDefault(x => x.OTRNo == id);
            if (BSM.HeaderOT != null)
            {
                BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == "REQ_OT").ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        #region "Approve"
        public ActionResult Approve(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            PROverTimeObject BSM = new PROverTimeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }

            if (!string.IsNullOrEmpty(id))
            {
                BSM.ScreenId = SCREEN_ID;
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/HROTRequest/Details/";
                string msg = BSM.ApproveOTReq(id, URL);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.Description = mess.Description + ". " + BSM.MessageError;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        #region "Reject"
        public ActionResult Reject(string id)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (!string.IsNullOrEmpty(id))
            {
                PROverTimeObject BSM = new PROverTimeObject();
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                }

                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.RejectOTReq(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    mess.Description = mess.Description + ". " + BSM.MessageError;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion

        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            //ActionName = "Details";
            this.UserSession();
            DataSelector();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID] = id;
            PROverTimeObject BSD = new PROverTimeObject();
            if (id.ToString() != "null")
            {
                string sms = BSD.CancelOTReq(id);
                if (sms == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(sms, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
        }
        #endregion

        #region 'Upload'
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);

            objFile.ObjectTemplate = new MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREEN_ID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;

            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("UploadControl", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            Session[PATH_FILE] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }
        #endregion
        public ActionResult RequestForApproval(string id)
        {
            UserSession();
            PROverTimeObject BSM = new PROverTimeObject();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/HROTRequest/Details/";
                string msg = BSM.requestToApprove(id, URL);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id;
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }


        [HttpPost]
        public ActionResult ShowData(DateTime FromDate, DateTime ToDate, decimal BreakTime)
        {

            ActionName = "Create";

            decimal Hour = Math.Round(Convert.ToDecimal(ToDate.Subtract(FromDate).TotalHours), 2) - BreakTime;
            var result = new
            {
                MS = SYConstant.OK,
                Hour = Hour
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("LEAVE_TIME");
            ViewData["LEAVE_TIME_SELECT"] = objList.ListData;
            // var staff = DBV.HR_STAFF_VIEW.ToList();
            var ListBranch = SYConstant.getBranchDataAccess();
            var staff = DBV.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == user.UserName);
            var ListStaff = new List<HR_STAFF_VIEW>();
            if (staff != null)
            {
                var _listEmp = DBV.HR_STAFF_VIEW.Where(w => w.EmpCode == user.UserName && w.FirstLine == staff.EmpCode || w.SecondLine == staff.EmpCode || w.HODCode == staff.EmpCode).ToList();
                ListStaff = _listEmp.ToList();
            }
            ViewData["STAFF_SELECT"] = ListStaff.ToList();
            var LstLeaveType = DB.PROTRates.ToList();
            ViewData["OTTYPE_SELECT"] = DB.PROTRates.ToList();
            var BS = new PROverTimeObject();
            foreach (var data in BS.OnDataSelectorPolicy())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}
