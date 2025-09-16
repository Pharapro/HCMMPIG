using Humica.Core;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.Att;
using Humica.Logic.LM;
using Humica.Models.SY;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.Attendance
{
    public class ATApprovalOnSiteController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ATM0000016";
        private const string URL_SCREEN = "/Attendance/Attendance/ATApprovalOnSite/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        public ATApprovalOnSiteController()
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
            UserConfList(this.KeyName);
            DataSelector();

			ATApprovalOnSiteObject BSM = new ATApprovalOnSiteObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATApprovalOnSiteObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.OnLoadIndex();
            BSM.OnLoadPending();
            
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ATApprovalOnSiteObject BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            DataSelector();
            BSM.OnLoadIndex(); 
            BSM.OnLoadPending();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
			ATApprovalOnSiteObject BSM = new ATApprovalOnSiteObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATApprovalOnSiteObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListEmpOnSite);
        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
			ATApprovalOnSiteObject BSM = new ATApprovalOnSiteObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATApprovalOnSiteObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListEmpOnSitePending);
        }
        #endregion
       
        #region "Details"
        public ActionResult Details(long id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
			ATApprovalOnSiteObject BSM = new ATApprovalOnSiteObject();
            if (id != 0)
            {
                BSM.OnDetailLoading(id);
                if (BSM.HeaderEmpOnSite != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        
        #region "Approve"
        public ActionResult Approve(string ID)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = ID;
			ATApprovalOnSiteObject BSM = new ATApprovalOnSiteObject();
            if (ID.ToString() != "null")
            {
                string fileName = Server.MapPath("~/Content/TEMPLATE/humica-e0886-firebase-adminsdk-95iz2-87c45a528b.json");

                string URL = SYUrl.getBaseUrl() + "/Attendance/Attendance/ATApprovalOnSite/";
                string msg = BSM.approveTheDoc(ID, URL);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = ID;
                    messageObject.Description = messageObject.Description + BSM.MessageError;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + ID;
                    SYMessages mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region "Reject"
        public ActionResult Reject(string id,string Comment)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id != null)
            {
				ATApprovalOnSiteObject BSM = new ATApprovalOnSiteObject();
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.RejectTheDoc(id, Comment);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        public ActionResult GridItemDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
			ATApprovalOnSiteObject BSM = new ATApprovalOnSiteObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATApprovalOnSiteObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemDetails", BSM.ListOnSiteItem);
        }
        #region "Ajax Approval"
        public ActionResult GridApprovalDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            ATOnSiteRequestObject BSM = new ATOnSiteRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATOnSiteRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        #endregion
        [HttpPost]
        public ActionResult ReasonDoc(string id)
        {
            ClsReason obj = new ClsReason();
            if (Request.Form["Comment"] != null)
            {
                obj.Comment = Request.Form["Comment"].ToString();
            }
            Session[Index_Sess_Obj + id] = obj;
            var result = new
            {
                MS = SYConstant.OK,
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("LEAVE_TIME");
            ViewData["UNITS_SELECT"] = ClsUnits.LoadUnit();
            objList = new SYDataList("STATUS_LEAVE_APPROVAL");
            ViewData["STATUS_APPROVAL"] = objList.ListData;
			ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
			ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
		}
    }
}
