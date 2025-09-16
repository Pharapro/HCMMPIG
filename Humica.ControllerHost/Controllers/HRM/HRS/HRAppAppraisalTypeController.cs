using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using Humica.Performance;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.HRS
{
    public class HRAppAppraisalTypeController : EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "INF0000017";
        private const string URL_SCREEN = "/HRM/HRS/HRAppAppraisalType/";
        private string ActionName;
        private string KeyName = "Code";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        IClsAppraisalType BSM;
        public HRAppAppraisalTypeController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsAppraisalType();
        }
        #region
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
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
                BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListAppraisalType);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.OnCreatingLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsAppraisalType collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.ADD);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Header = collection.Header;
            BSM.ScreenId = SCREEN_ID;
            string msg = BSM.Create();

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSM.Header.Code;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?Code=" + BSM.Header.Code;
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
        public ActionResult Edit(string Code)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = Code;
            DataSelector();
            BSM.OnDetailLoading(Code);
            if (BSM.Header != null)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public ActionResult Edit(string Code, ClsAppraisalType collection)
        {
            ActionName = "Create";
            UserSession();
            ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = Code;
            DataLisRegion();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ScreenId = SCREEN_ID;
            BSM.Header = collection.Header;
            string msg = BSM.Update(Code);
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = Code;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?Code=" + BSM.Header.Code;
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
        public ActionResult Details(string Code)
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM.OnDetailLoading(Code);
            if (BSM.Header != null)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        #region Appraisal Region
        public ActionResult GridItemRegionDetails()
        {
            ActionName = "Details";
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemRegionDetails", BSM.ListApprRegion);
        }
        public ActionResult GridItemRegion()
        {
            UserConf(ActionBehavior.EDIT);
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemRegion", BSM.ListApprRegion);
        }
        public ActionResult CreateRegion(HRApprRegion ModelObject)
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
                        BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
                    }
                    var msg = BSM.OnGridModifyRegion(ModelObject, SYActionBehavior.ADD.ToString());
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
            return PartialView("GridItemRegion", BSM.ListApprRegion);
        }
        public ActionResult EditRegion(HRApprRegion ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
                var msg = BSM.OnGridModifyRegion(ModelObject, SYActionBehavior.EDIT.ToString());
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
            return PartialView("GridItemRegion", BSM.ListApprRegion);
        }
        public ActionResult DeleteRegion(HRApprRegion ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
                var msg = BSM.OnGridModifyRegion(ModelObject, SYActionBehavior.DELETE.ToString());
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
            return PartialView("GridItemRegion", BSM.ListApprRegion);
        }
        #endregion

        #region Appraisal Factor
        public ActionResult GridItemFactorDetails()
        {
            ActionName = "Details";
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemFactorDetails", BSM.ListApprFactor);
        }
        public ActionResult GridItemFactor()
        {
            ActionName = "Create";
            UserConf(ActionBehavior.ADD);
            UserSession();
            UserConfListAndForm();
            DataLisRegion();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemFactor", BSM.ListApprFactor);
        }
        public ActionResult CreateFactor(HRApprFactor ModelObject)
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
                        BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
                    }
                    var msg = BSM.OnGridModifyFactor(ModelObject, SYActionBehavior.ADD.ToString());
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
            DataLisRegion();
            return PartialView("GridItemFactor", BSM.ListApprFactor);
        }
        public ActionResult EditFactor(HRApprFactor ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
                var msg = BSM.OnGridModifyFactor(ModelObject, SYActionBehavior.EDIT.ToString());
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
            DataLisRegion();
            return PartialView("GridItemFactor", BSM.ListApprFactor);
        }
        public ActionResult DeleteFactor(HRApprFactor ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
                var msg = BSM.OnGridModifyFactor(ModelObject, SYActionBehavior.DELETE.ToString());
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
            DataLisRegion();
            return PartialView("GridItemFactor", BSM.ListApprFactor);
        }
        #endregion

        private void DataLisRegion()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisalType)Session[Index_Sess_Obj + ActionName];
            }
            ViewData["REGION_SELECT"] = BSM.ListApprRegion.Where(w => w.IsKPI != true).ToList();
        }
        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelectorLoading())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}//1144