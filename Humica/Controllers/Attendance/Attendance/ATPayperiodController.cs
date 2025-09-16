using Humica.Attendance;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.Attendance
{
    public class ATPayperiodController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ATM0000017";
        private const string URL_SCREEN = "/Attendance/Attendance/ATPayperiod/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "PeriodID";
        protected IClsAttPayPeriod BSM;
        public ATPayperiodController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsAttPayPeriod();
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
        public ActionResult Create(ATPayperiod MModel)
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
        public ActionResult Edit(ATPayperiod MModel)
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
        public ActionResult Delete(ATPayperiod MModel)
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
