using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.LeaveBalance
{

    public class ESSOTRequestController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000013";
        private const string URL_SCREEN = "/SelfService/LeaveBalance/ESSOTRequest/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "OTRNo";
        private string DOCTYPE = "OTR01";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public ESSOTRequestController()
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
            BSM.User = SYSession.getSessionUser();
            BSM.ListOTRequest = DB.HRRequestOTs.Where(w => w.EmpCode == BSM.User.UserName).ToList();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(PROverTimeObject collection)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            PROverTimeObject BSM = new PROverTimeObject();
            BSM.User = SYSession.getSessionUser();
            BSM.ListOTRequest = DB.HRRequestOTs.Where(w => w.EmpCode == BSM.User.UserName).ToList();

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
            BSM.User = SYSession.getSessionUser();
            var emp = DBV.HR_STAFF_VIEW.Where(w => w.EmpCode == BSM.User.UserName).ToList();

            if (emp.Count > 0)
            {
                //  BSM.HeaderStaff = DB.HR_STAFF_VIEW.FirstOrDefault(x => x.EmpCode == BSM.User.UserName);
                BSM.HeaderOT.OTStartTime = DateTime.Now;
                BSM.HeaderOT.OTEndTime = DateTime.Now;
                BSM.HeaderOT.Hours = 0;
                BSM.DocType = DOCTYPE;
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
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
            if (ModelState.IsValid)
            {
                if (Session[PATH_FILE] != null)
                {
                    //collection.Header.AttachFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                    BSM.HeaderOT = collection.HeaderOT;
                    BSM.DocType = DOCTYPE;
                }
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/HROTRequest/Details/";
                string msg = BSM.ESSCreateOTReq(URL);

                if (msg == SYConstant.OK)
                {
                    BSM.ScreenId = SCREEN_ID;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.HeaderOT.OTRNo;
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
            if (id != "null")
            {
                BSM.HeaderOT = DB.HRRequestOTs.SingleOrDefault(x => x.OTRNo == id);

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
            if (id != "null")
            {
                BSM.HeaderOT = DB.HRRequestOTs.FirstOrDefault(x => x.OTRNo == id);
                BSM.ListApproval = DB.ExDocApprovals.Where(x => x.DocumentNo == id && x.DocumentType == "REQ_OT").ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

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
        public ActionResult GridApprovalDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            PROverTimeObject BSM = new PROverTimeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        [HttpPost]
        public ActionResult ShowData(DateTime FromDate, DateTime ToDate, decimal BreakTime)
        {

            ActionName = "Create";

            decimal Hour = Math.Round(Convert.ToDecimal(ToDate.Subtract(FromDate).TotalHours) - BreakTime, 2);
            var result = new
            {
                MS = SYConstant.OK,
                Hour = Hour
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            // ViewData["STAFF_SELECT"] = DBV.HR_STAFF_VIEW.ToList();
            //var LstLeaveType = DB.PROTRates.ToList();
            //ViewData["OTTYPE_SELECT"] = DB.PROTRates.ToList();
            var BS = new PROverTimeObject();
            foreach (var data in BS.OnDataSelectorPolicy())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}
