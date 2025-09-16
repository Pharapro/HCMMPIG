using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EForm;
using System;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class EFSPortalTypeController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "INF0000015";
        private const string URL_SCREEN = "/HRM/HRS/EFSPortalType/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        protected IClsEFPortalType BSM;
        public EFSPortalTypeController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsEFPortalType();
            BSM.OnLoad();
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.OnIndexLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        #region "Setup PortalType"
        public ActionResult GridItemsPortalType()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            UserConf(ActionBehavior.EDIT);
            BSM.OnIndexLoadingPortalType();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsPortalType", BSM.ListPortalType);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePortalType(EFPortalType MModel)
        {
            UserSession();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEFPortalType)Session[Index_Sess_Obj + ActionName];
                }
                var msg = BSM.OnGridModifyPortalType(MModel, SYActionBehavior.ADD.ToString());
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexLoadingPortalType();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsPortalType", BSM.ListPortalType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPortalType(EFPortalType MModel)
        {
            UserSession();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
                try
                {
                    var msg = BSM.OnGridModifyPortalType(MModel, SYActionBehavior.EDIT.ToString());
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

            BSM.OnIndexLoadingPortalType();
            return PartialView("GridItemsPortalType", BSM.ListPortalType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePortalType(EFPortalType MModel)
        {
            UserSession();
            UserConfListAndForm();
            try
            {
                var msg = BSM.OnGridModifyPortalType(MModel, SYActionBehavior.DELETE.ToString()); 
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.OnIndexLoadingPortalType();
            return PartialView("GridItemsPortalType", BSM.ListPortalType);
        }
        #endregion
    }
}