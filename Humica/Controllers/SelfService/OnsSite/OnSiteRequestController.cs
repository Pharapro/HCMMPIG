using DevExpress.Web.Mvc;
using DevExpress.Web;
using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.OnSite
{
    public class OnSiteRequestController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESSOS00001";
        private const string URL_SCREEN = "/SelfService/OnSite/OnSiteRequest/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        IUnitOfWork unitOfWork;
        public OnSiteRequestController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            ATOnSiteRequestObject BSM = new ATOnSiteRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATOnSiteRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            var obj = unitOfWork.Set<ATRequestOnSite>().ToList();
            if (obj.Any()) BSM.ListEmpOnSite = obj;
            //BSM.OnLoadIndex();
            BSM.OnLoadPending();
            
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        //[HttpPost]
        //public ActionResult Index(ATOnSiteRequestObject BSM)
        //{
        //    ActionName = "Index";
        //    UserSession();
        //    UserConfList(this.KeyName);
        //    DataSelector();
        //    BSM.OnLoadIndex(); 
        //    BSM.OnLoadPending();
        //    var obj = unitOfWork.Set<ATRequestOnSite>().ToList();
        //    if (obj.Any()) BSM.ListEmpOnSite = obj;
        //    Session[Index_Sess_Obj + ActionName] = BSM;
        //    return View(BSM);
        //}
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ATOnSiteRequestObject BSM = new ATOnSiteRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATOnSiteRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListEmpOnSite);
        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            ATOnSiteRequestObject BSM = new ATOnSiteRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATOnSiteRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListEmpOnSitePending);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ATOnSiteRequestObject BSM = new ATOnSiteRequestObject();
            BSM.User = SYSession.getSessionUser();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.HeaderEmpOnSite = new ATRequestOnSite();
            BSM.HeaderEmpOnSite.FromDate = DateTime.Now;
            BSM.HeaderEmpOnSite.ToDate = DateTime.Now;
            Session[PATH_FILE] = null;
            BSM.HeaderEmpOnSite.Unit = "Day";
            BSM.Units = "Day";
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(string ID, ATOnSiteRequestObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            var BSM = new ATOnSiteRequestObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATOnSiteRequestObject)Session[Index_Sess_Obj + ActionName];
                BSM.HeaderEmpOnSite = collection.HeaderEmpOnSite;
                BSM.HeaderStaff = collection.HeaderStaff;
                if (Session[PATH_FILE] != null)
                {
                    BSM.HeaderEmpOnSite.Attachment = Session[PATH_FILE].ToString();
                }
            }
            if (ModelState.IsValid)
            {
                string msg = BSM.RequestOnsite();
                if (msg == SYConstant.OK)
                {
                    BSM.ScreenId = SCREEN_ID;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    SYMessages mess = SYMessages.getMessageObject(msg, user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }

            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
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
            ATOnSiteRequestObject BSM = new ATOnSiteRequestObject();
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
            ATOnSiteRequestObject BSM = new ATOnSiteRequestObject();
            if (ID.ToString() != "null")
            {
                string fileName = Server.MapPath("~/Content/TEMPLATE/humica-e0886-firebase-adminsdk-95iz2-87c45a528b.json");

                string URL = SYUrl.getBaseUrl() + "/SelfService/OnSite/OnSiteRequest/";
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
        public ActionResult Reject(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id != null)
            {
                ATOnSiteRequestObject BSM = new ATOnSiteRequestObject();
                string comment = "";
                if (Session[Index_Sess_Obj + id] != null)
                {
                    ClsReason objReason = (ClsReason)Session[Index_Sess_Obj + id];
                    comment = objReason.Comment;
                    objReason = null;
                }

                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.RejectTheDoc(id, comment);
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
        public ActionResult UploadControlCallbackActionImage(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            var path = unitOfWork.Repository<CFUploadPath>().Queryable().FirstOrDefault(w => w.PathCode == "IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "STAFF";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("UploadControl",
                sfi.ValidationSettings,
                sfi.uc_FileUploadCompleteFile);
            Session[PATH_FILE] = sfi.ObjectTemplate.UpoadPath;
            return null;
        }
        public ActionResult GridItemDetails()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            ATOnSiteRequestObject BSM = new ATOnSiteRequestObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATOnSiteRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemDetails";
            return PartialView("GridItemDetails", BSM);
        }
        public ActionResult GridIteEdit(ATRequestOnSiteItem MModel)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            ATOnSiteRequestObject BSM = new ATOnSiteRequestObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ATOnSiteRequestObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var ListOnsite = BSM.ListOnSiteItem.Where(w => w.LineItem == MModel.LineItem).ToList();
                    if (ListOnsite.Count > 0)
                    {
                        var objUpdate = ListOnsite.FirstOrDefault();
                        
                        objUpdate.Longitude = MModel.Longitude;
                        objUpdate.Latitude = MModel.Latitude;
                        objUpdate.MapURL = MModel.MapURL;
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            return PartialView("GridItemDetails", BSM);
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
        [HttpPost]
        public ActionResult ShowData(string ID, DateTime FromDate, DateTime ToDate, string Action)
        {

            ActionName = Action;
            var Policy = unitOfWork.Set<ATPolicy>().FirstOrDefault();
            DateTime From = Policy.LFromDate;
            int year = FromDate.Year;
            if (From.Month == 12 && FromDate.Month == From.Month && FromDate.Day >= From.Day)
                year = year + 1;

            ATOnSiteRequestObject BSM = new ATOnSiteRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATOnSiteRequestObject)Session[Index_Sess_Obj + ActionName];
                BSM.ListOnSiteItem = new List<ATRequestOnSiteItem>();
            }
            if (ID == "" || ID == null)
            {
                var rs1 = new { MS = "INVALIT" };
                return Json(rs1, JsonRequestBehavior.DenyGet);
            }
            if (BSM.HeaderStaff != null)
            {
                BSM.ListOnSiteItem = BSM.Get_LeaveDay(BSM.HeaderStaff.EmpCode, FromDate, ToDate, ID);
                Session[Index_Sess_Obj + ActionName] = BSM;
                var result = new { MS = SYConstant.OK };
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {

            ActionName = "Details";
            var EmpStaff = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                Session["EmpCode"] = EmpCode;
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmpType,
                    Division = EmpStaff.Division,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.LevelCode,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate
                };
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult ShowUNITS(string ID, string Action)
        {
            ActionName = Action;
            ATOnSiteRequestObject BSM = new ATOnSiteRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATOnSiteRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            var message = SYConstant.OK;
            if (ID == "Hours")
            {
                SYSetting _list = unitOfWork.Set<SYSetting>().FirstOrDefault(w => w.SettingName == "ONSITE_HOURS");
                if (_list != null)
                {
                    if (_list.SettinValue == "FALSE")
                    {
                        message = "Not allow request by hours!";
                    }
                    ID = "Day";
                }
            }
            BSM.Units = ID;

            Session[Index_Sess_Obj + ActionName] = BSM;
            var rs = new
            {
                MS = message,
                Units = ID
            };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            var ListBranch = SYConstant.getBranchDataAccess();
            DateTime date = DateTime.Now.AddMonths(-1);
            var staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == user.UserName);
            var ListStaff = new List<HRStaffProfile>();
            if (staff != null)
            {
                var _listEmp = unitOfWork.Set<HRStaffProfile>().Where(w => w.FirstLine == staff.EmpCode || w.SecondLine == staff.EmpCode || w.HODCode == staff.EmpCode
                || w.FirstLine2 == staff.EmpCode || w.SecondLine2 == staff.EmpCode || w.EmpCode == staff.EmpCode).ToList();
                _listEmp = _listEmp.Where(w => w.Status == "A" || w.DateTerminate > DateTime.Now).ToList();
                if (_listEmp.Count > 0)
                {
                    ListStaff = _listEmp.ToList();
                }
                else
                {
                    ListStaff = unitOfWork.Set<HRStaffProfile>().AsEnumerable().Where(w => ListBranch.AsEnumerable().Where(x => x.Code == w.Branch).Any()).ToList();
                    ListStaff = ListStaff.Where(w => w.DEPT == staff.DEPT && (w.Status == "A" || (w.DateTerminate > date && w.Status == "I"))).ToList();
                }
            }
            SYDataList objList = new SYDataList("LEAVE_TIME");
            ViewData["UNITS_SELECT"] = ClsUnits.LoadUnit();
            objList = new SYDataList("STATUS_LEAVE_APPROVAL");
            ViewData["STATUS_APPROVAL"] = objList.ListData;
            ViewData["STAFF_SELECT"] = ListStaff.ToList();
        }
    }
}
