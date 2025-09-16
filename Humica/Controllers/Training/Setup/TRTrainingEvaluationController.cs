
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EForm;
using Humica.Models.SY;
using System;
using System.Web.Mvc;

namespace Humica.Controllers.Training.Setup
{
    public class TRTrainingEvaluationController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "TRM000009";
        private const string URL_SCREEN = "/Training/Setup/TRTrainingEvaluation/";
        private string ActionName;
        private string KeyName = "PortalCode;QuestionCode";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        IClsEFSPortal BSM;
        public TRTrainingEvaluationController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsEFSPortal();
            BSM.OnLoad();
        }
        #region
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexLoading();
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
                BSM = (IClsEFSPortal)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListSelfAssessment);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            BSM.OnCreatingLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsEFSPortal collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.ADD);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEFSPortal)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Header = collection.Header;
            BSM.ScreenId = SCREEN_ID;
            string msg = BSM.Create();

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSM.Header.QuestionCode;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?PortalCode=" + BSM.Header.PortalCode + "&QuestionCode=" + BSM.Header.QuestionCode;
                Session[Index_Sess_Obj + ActionName] = null;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
            }
            else
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Edit
        public ActionResult Edit(string PortalCode, string QuestionCode)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = PortalCode;
            DataSelector();
            BSM.OnDetailLoading(PortalCode, QuestionCode);
            if (BSM.Header != null)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public ActionResult Edit(string PortalCode, string QuestionCode, ClsEFSPortal collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = QuestionCode;
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEFSPortal)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ScreenId = SCREEN_ID;
            BSM.Header = collection.Header;
            string msg = BSM.Update(PortalCode, QuestionCode);
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = QuestionCode;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?PortalCode=" + BSM.Header.PortalCode + "&QuestionCode=" + BSM.Header.QuestionCode;
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
        #region Details
        public ActionResult Details(string PortalCode, string QuestionCode)
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM.OnDetailLoading(PortalCode, QuestionCode);
            if (BSM.Header != null)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region "Delete"
        public ActionResult Delete(string PortalCode, string QuestionCode)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            string msg = BSM.Delete(PortalCode, QuestionCode);
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
        #region QCM
        public ActionResult GridItemsDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEFSPortal)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemsDetails", BSM.ListSelfAssQCM);
        }
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEFSPortal)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItems", BSM.ListSelfAssQCM);
        }
        public ActionResult CreateItems(HRApprSelfAssQCM ModelObject)
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
                        BSM = (IClsEFSPortal)Session[Index_Sess_Obj + ActionName];
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
            return PartialView("GridItems", BSM.ListSelfAssQCM);
        }
        public ActionResult EditItems(HRApprSelfAssQCM ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEFSPortal)Session[Index_Sess_Obj + ActionName];
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
            return PartialView("GridItems", BSM.ListSelfAssQCM);
        }
        public ActionResult DeleteItems(HRApprSelfAssQCM ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEFSPortal)Session[Index_Sess_Obj + ActionName];
                var msg = BSM.OnGridModify(ModelObject, SYActionBehavior.DELETE.ToString());
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
            return PartialView("GridItems", BSM.ListSelfAssQCM);
        }
        #endregion

        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelectorLoading())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}
