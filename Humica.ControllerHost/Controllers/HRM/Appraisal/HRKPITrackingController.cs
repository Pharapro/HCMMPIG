using DevExpress.Web.Mvc;
using DevExpress.Web;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using Humica.Performance;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Humica.Core;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HRKPITrackingController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRA0000010";
        private const string URL_SCREEN = "/HRM/Appraisal/HRKPITracking/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        protected IClsKPITracking BSM;
        public HRKPITrackingController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsKPITracking();
            BSM.OnLoad();
        }
        #region List
        public ActionResult Index()
        {
            UserSession();
            ActionName = "Index";
            UserConfListAndForm(this.KeyName);
            DataList();
            BSM.FInYear = new Core.FT.FTINYear();
			BSM.FInYear.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			BSM.FInYear.ToDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,
				DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)
			);
			BSM.FInYear.Status = SYDocumentStatus.PENDING.ToString();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IClsKPITracking)Session[Index_Sess_Obj + ActionName];
                BSM.FInYear = obj.FInYear;
            }
            BSM.OnIndexLoading(BSM.FInYear.INYear, BSM.FInYear.FromDate, BSM.FInYear.ToDate, BSM.FInYear.Status);
            BSM.ListKPIEmpPending = BSM.OnIndexLoadingAssign();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Index(ClsKPITracking BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
			BSM.OnIndexLoading(BSM.FInYear.INYear, BSM.FInYear.FromDate, BSM.FInYear.ToDate, BSM.FInYear.Status);
			BSM.ListKPIEmpPending = BSM.OnIndexLoadingAssign();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPITracking)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.listKPITracking);
        }
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPITracking)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListKPIEmpPending);
        }
        #endregion
        #region "Create"
        public ActionResult Create(string id,string EmpCode)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.OnCreatingLoading(id, EmpCode);
            Session[Index_Sess_Obj + ActionName] = BSM;
            EmployeeTask(id);
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsKPITracking collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            string KPICode = "";
            string KPIType = "";
            string DirectedByCode = "";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPITracking)Session[Index_Sess_Obj + ActionName];
                KPICode = BSM.HeaderKPITracking.AssignCode;
                KPIType = BSM.HeaderKPITracking.KPIType;
                DirectedByCode = BSM.HeaderKPITracking.DirectedByCode;
            }
            BSM.HeaderKPITracking = collection.HeaderKPITracking;
            BSM.HeaderKPITracking.AssignCode = KPICode;
            BSM.HeaderKPITracking.KPIType = KPIType;
            BSM.HeaderKPITracking.DirectedByCode = DirectedByCode;
            BSM.ScreenId = SCREEN_ID;

            string msg = BSM.Create();

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSM.HeaderKPITracking.TranNo.ToString();
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                Session[Index_Sess_Obj + ActionName] = null;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create?ID=" + BSM.HeaderKPITracking.AssignCode + "&EmpCode=" + BSM.HeaderKPITracking.EmpCode);
            }
            else
            {
                EmployeeTask(BSM.HeaderKPITracking.AssignCode);
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Edit
        public ActionResult Edit(int ID)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;

            BSM.OnDetailLoading(ID);
            if (BSM.HeaderKPITracking != null)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public ActionResult Edit(int id, ClsKPITracking collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPITracking)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ScreenId = SCREEN_ID;
            BSM.HeaderKPITracking = collection.HeaderKPITracking;
            string msg = BSM.Update(id);
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = id.ToString();
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(BSM);
            }
            else
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return View(BSM);
        }
        #endregion
        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            string msg = BSM.DeleteAll(id);
            if (msg == SYConstant.OK)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Details
        public ActionResult Details(int id)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            BSM.OnDetailLoading(id);
            if (BSM.HeaderKPITracking != null)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #endregion
        #region "Reason"

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

        #endregion
        #region Approve
        public ActionResult Approve(string id)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (!string.IsNullOrEmpty(id))
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsKPITracking)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Approved(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.Description = mess.Description;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Reject(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (!string.IsNullOrEmpty(id))
            {
                string comment = "";
                if (Session[Index_Sess_Obj + id] != null)
                {
                    ClsReason objReason = (ClsReason)Session[Index_Sess_Obj + id];
                    comment = objReason.Comment;
                    objReason = null;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Reject(id, comment);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    mess.Description = mess.Description;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Time Sheet
        public ActionResult GridItemsDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPITracking)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemsDetails", BSM.ListTimeSheet);
        }
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPITracking)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItems", BSM);
        }
        public ActionResult CreateItems(HRKPITimeSheet ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (IClsKPITracking)Session[Index_Sess_Obj + ActionName];
                    }
                    if (Session[PATH_FILE] != null)
                    {
                        ModelObject.Attachment = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    var msg = BSM.OnGridModify(ModelObject, SYActionBehavior.ADD.ToString());
                    if (msg == SYConstant.OK)
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
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
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM);
        }
        public ActionResult EditItems(HRKPITimeSheet ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPITracking)Session[Index_Sess_Obj + ActionName];
                var listCheck = BSM.ListTimeSheet.Where(w => w.LineItem == ModelObject.LineItem).ToList();
                if (Session[PATH_FILE] != null)
                {
                    ModelObject.Attachment = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                else
                {
                    ModelObject.Attachment = listCheck.FirstOrDefault().Attachment;
                }
                var msg = BSM.OnGridModify(ModelObject, SYActionBehavior.EDIT.ToString());
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItems", BSM);
        }
        public ActionResult DeleteItems(HRKPITimeSheet ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPITracking)Session[Index_Sess_Obj + ActionName];
                var msg =  BSM.OnGridModify(ModelObject, SYActionBehavior.DELETE.ToString());
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItems", BSM);
        }
        public ActionResult UploadControlCallbackActionTimeSheet(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.AttachmentMulti = "";
            sfi.DocumentMulti = "";
            sfi.ScreenID = SCREEN_ID;
            sfi.UploadBy = user.UserName;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadTimeSheet",
                sfi.ValidationSettings,
                sfi.uc_FileUploadCompleteMultiFile);
            if (!string.IsNullOrEmpty(sfi.AttachmentMulti))
            {
                int lastSemicolonIndex = sfi.AttachmentMulti.LastIndexOf(';');
                sfi.AttachmentMulti = sfi.AttachmentMulti.Substring(0, lastSemicolonIndex);
            }
            Session[PATH_FILE] = sfi.AttachmentMulti;
            return null;
        }
        #endregion
        [HttpPost]
        public ActionResult ShowData_EmpKPI(string TaskID)
        {
            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPITracking)Session[Index_Sess_Obj + ActionName];
            }
            var objStaff = BSM.GetDataTaskItem(BSM.HeaderKPITracking.AssignCode, TaskID);
            var result = new
            {
                MS = SYConstant.OK,
                KPI = objStaff.KPI,
                Measure = objStaff.Measure,
                Options = BSM.Options,
                Target = objStaff.Target,
                Actual = BSM.TotalActual,
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult ShowData_Date(string EmpCode,DateTime documentDate)
        {
            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPITracking)Session[Index_Sess_Obj + ActionName];
            }
            BSM.GetTimer(EmpCode, documentDate);
            var result = new
            {
                MS = SYConstant.OK,
            };
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult CalculateHour(DateTime DocmentDate, DateTime FromTime,DateTime ToTime)
        {
            var ret_result = BSM.CalculateHour(DocmentDate, FromTime, ToTime);
            var result = new
            {
                MS = SYConstant.OK,
                Hour = ret_result.Hours,
                FromTime = ret_result.From_Time,
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        protected void EmployeeTask(string id)
        {
            foreach (var data in BSM.OnDataSelectorLoading(id))
            {
                ViewData[data.Key] = data.Value;
            }
        }
        protected void DataList()
        {
            foreach (var data in BSM.OnDataStatusLoading())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}