using Humica.Core;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Payroll;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRS
{
    public class PRPeriodController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000034";
        private const string URL_SCREEN = "/PR/PRM/PRPeriod";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "PeriodID";
        protected IClsPRPayPeriod BSM;
        public PRPeriodController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsPRPayPeriod();
            BSM.OnLoad();
        }
        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            BSM.OnIndexLoading();
            return View(BSM);
        }
        public ActionResult GridItems()
        {
            UserSession();
            UserConfListAndForm();
            BSM.OnIndexLoadingPeriod();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM.ListPeriod);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(PRPayPeriodItem MModel)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyPeriod(MModel, SYActionBehavior.ADD.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexLoadingPeriod();
            return PartialView("GridItems", BSM.ListPeriod);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(PRPayPeriodItem MModel)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyPeriod(MModel, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexLoadingPeriod();
            return PartialView("GridItems", BSM.ListPeriod);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(PRPayPeriodItem MModel)
        {
            UserSession();
            var msg = BSM.OnGridModifyPeriod(MModel, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexLoadingPeriod();
            return PartialView("GridItems", BSM.ListPeriod.ToList());
        }
    }
}
