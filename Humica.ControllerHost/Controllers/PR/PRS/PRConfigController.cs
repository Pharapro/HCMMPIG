using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Payroll;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRS
{
    public class PRConfigController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRS0000003";
        private const string URL_SCREEN = "/PR/PRS/PRConfig/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        protected IClsPRConfig BSM;
        public PRConfigController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsPRConfig();
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
        #region Config
        public ActionResult GridItems()
        {
            UserConf(ActionBehavior.EDIT);
            BSM.OnIndexLoadingConfig();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM.ListIncomeType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(PRIncomeType MModel)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyConfig(MModel, SYActionBehavior.ADD.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexLoadingConfig();
            return PartialView("GridItems", BSM.ListIncomeType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(PRIncomeType MModel)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyConfig(MModel, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            BSM.OnIndexLoadingConfig();
            return PartialView("GridItems", BSM.ListIncomeType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(PRIncomeType MModel)
        {
            UserSession();
            var msg = BSM.OnGridModifyConfig(MModel, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexLoadingConfig();
            return PartialView("GridItems", BSM.ListIncomeType);
        }
        #endregion
        #region SocialSecurity
        public ActionResult GridItemSoSe()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            BSM.OnIndexLoadingSoSe();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemSoSe", BSM.ListSocialSecurity);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateSoSe(PRSocialSecurity MModel)
        {
            UserSession();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifySoSe(MModel, SYActionBehavior.ADD.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexLoadingSoSe();
            return PartialView("GridItemSoSe", BSM.ListSocialSecurity);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditSoSe(PRSocialSecurity MModel)
        {
            UserSession();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifySoSe(MModel, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            BSM.OnIndexLoadingSoSe();
            return PartialView("GridItemSoSe", BSM.ListSocialSecurity);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteSoSe(PRSocialSecurity MModel)
        {
            UserSession();
            DataSelector();
            var msg = BSM.OnGridModifySoSe(MModel, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexLoadingSoSe();
            return PartialView("GridItemSoSe", BSM.ListSocialSecurity);
        }
        #endregion

        #region Rounding Rule
        public ActionResult GridItemRounding()
        {
            UserSession();
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            BSM.OnIndexLoadingRounding();
            return PartialView("GridItemRounding", BSM.ListRoundingRule);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateRounding(SYRoundingRule MModel)
        {
            UserSession();
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyRounding(MModel, SYActionBehavior.ADD.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexLoadingRounding();
            return PartialView("GridItemRounding", BSM.ListRoundingRule);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditRounding(SYRoundingRule MModel)
        {
            UserSession();
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyRounding(MModel, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            BSM.OnIndexLoadingRounding();
            return PartialView("GridItemRounding", BSM.ListRoundingRule);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteRounding(SYRoundingRule MModel)
        {
            UserSession();
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            var msg = BSM.OnGridModifyRounding(MModel, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexLoadingRounding();
            return PartialView("GridItemRounding", BSM.ListRoundingRule);
        }
        #endregion

        #region Fringe Benefit
        public ActionResult GridItemFringeBenefit()
        {
            UserSession();
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            BSM.OnIndexLoadingFringeBenefit();
            return PartialView("GridItemFringeBenefit", BSM.ListFringeBenefit);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateFringeBenefit(PRFringeBenefitSetting MModel)
        {
            UserSession();
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyFringeBenefit(MModel, SYActionBehavior.ADD.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexLoadingFringeBenefit();
            return PartialView("GridItemFringeBenefit", BSM.ListFringeBenefit);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditFringeBenefit(PRFringeBenefitSetting MModel)
        {
            UserSession();
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyFringeBenefit(MModel, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            BSM.OnIndexLoadingFringeBenefit();
            return PartialView("GridItemFringeBenefit", BSM.ListFringeBenefit);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteFringeBenefit(PRFringeBenefitSetting MModel)
        {
            UserSession();
            UserConf(ActionBehavior.DELETE);
            DataSelector();
            var msg = BSM.OnGridModifyFringeBenefit(MModel, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexLoadingFringeBenefit();
            return PartialView("GridItemFringeBenefit", BSM.ListFringeBenefit);
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