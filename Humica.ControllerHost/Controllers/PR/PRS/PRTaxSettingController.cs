using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Payroll;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRS
{
    public class PRTaxSettingController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRS0000005";
        private const string URL_SCREEN = "/PR/PRS/PRTaxSetting";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        protected IClsPRTaxSetting BSM;
        public PRTaxSettingController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsPRTaxSetting();
        }

        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM.OnIndexLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsPRTaxSetting collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM = (IClsPRTaxSetting)Session[Index_Sess_Obj + ActionName];
            BSM.PayrollSetting = collection.PayrollSetting;
            BSM.ScreenId = SCREEN_ID;
            string msg = BSM.UpdateSetting();
            if (msg != SYConstant.OK)
            {
                SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + ActionName] = BSM;
                Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                return View(BSM);
            }
            SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.MESSAGE_SUBMIT] = mess;
            return View(BSM);
        }
        #region Tax Setting
        public ActionResult GridItems()
        {
            UserConf(ActionBehavior.EDIT);
            BSM.OnIndexTaxSetting();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("Gridview", BSM.ListHeader);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(PRTaxSetting MModel)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyTaxSetting(MModel, SYActionBehavior.ADD.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexTaxSetting();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(PRTaxSetting MModel)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyTaxSetting(MModel, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            BSM.OnIndexTaxSetting();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(PRTaxSetting MModel)
        {
            UserSession();
            var msg = BSM.OnGridModifyTaxSetting(MModel, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexTaxSetting();
            return PartialView("Gridview", BSM.ListHeader);
        }
        #endregion

        #region ExceptType
        public ActionResult GridItemExceptType()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            BSM.OnIndexExceptType();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemExceptType", BSM.ListExceptType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateExceptType(PRExceptType MModel)
        {
            UserSession();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyExceptType(MModel, SYActionBehavior.ADD.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexExceptType();
            return PartialView("GridItemExceptType", BSM.ListExceptType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditExceptType(PRExceptType MModel)
        {
            UserSession();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyExceptType(MModel, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            BSM.OnIndexExceptType();
            return PartialView("GridItemExceptType", BSM.ListExceptType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteExceptType(PRExceptType MModel)
        {
            UserSession();
            DataSelector();
            var msg = BSM.OnGridModifyExceptType(MModel, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexExceptType();
            return PartialView("GridItemExceptType", BSM.ListExceptType);
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
