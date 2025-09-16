using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Payroll;
using System.Web.Mvc;

namespace HR.Controllers.PR.PRM
{
    public class PRBIExchangeRatesController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000031";
        private const string URL_SCREEN = "/PR/PRM/PRBIExchangeRates/";
        private string KeyName = "TranNo";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";

        IClsPRExchangeRate BSM;
        public PRBIExchangeRatesController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsPRExchangeRate();
            BSM.OnLoad();
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM.OnIndexLoadingBiExchangeRate();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM.OnIndexLoadingBiExchangeRate();
            return PartialView("GridItems", BSM.ListBiHeader);
        }
        [HttpPost]
        public ActionResult Create(PRBiExchangeRate MModel)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyBiExchangeRate(MModel, SYActionBehavior.ADD.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexLoadingBiExchangeRate();
            return PartialView("GridItems", BSM.ListBiHeader);
        }
        [HttpPost]
        public ActionResult Edit(PRBiExchangeRate MModel)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyBiExchangeRate(MModel, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexLoadingBiExchangeRate();
            return PartialView("GridItems", BSM.ListBiHeader);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(PRBiExchangeRate MModel)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            var msg = BSM.OnGridModifyBiExchangeRate(MModel, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexLoadingBiExchangeRate();
            return PartialView("GridItems", BSM.ListBiHeader);
        }
        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelectorLoading())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}