using Humica.Attendance;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.LMS
{
    public class HRLeavePolicyController : EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRS0000012";
        private const string URL_SCREEN = "/HRM/LMS/HRLeavePolicy/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        IClsLeaveType BSM;
        public HRLeavePolicyController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsLeaveType();
        }
        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM.OnIndexLoadingPolicy();
            return View(BSM);
        }
        #region LeaveProRates

        public ActionResult GridviewProrate()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            BSM.OnIndexPolicy();
            return PartialView("GridviewProrate", BSM);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(HRLeaveProRate MModel)
        {
            UserSession();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyPolicy(MModel, SYActionBehavior.ADD.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            BSM.OnIndexPolicy();
            return PartialView("GridviewProrate", BSM);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(HRLeaveProRate MModel)
        {
            UserSession();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyPolicy(MModel, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            BSM.OnIndexPolicy();
            return PartialView("GridviewProrate", BSM);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(HRLeaveProRate MModel)
        {
            UserSession();
            DataSelector();
            var msg = BSM.OnGridModifyPolicy(MModel, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexPolicy();
            return PartialView("GridviewProrate", BSM);
        }

        #endregion
        #region Leave Hour
        public ActionResult GridItemLHour()
        {
            UserConf(ActionBehavior.VIEW);
            DataSelector();
            BSM.OnIndexHourPolicy();
            return PartialView("GridItemLHour", BSM);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateLHour(HRLeaveHourPolicy MModel)
        {
            UserSession();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyHour(MModel, SYActionBehavior.ADD.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            BSM.OnIndexHourPolicy();
            return PartialView("GridItemLHour", BSM);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditLHour(HRLeaveHourPolicy MModel)
        {
            UserSession();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyHour(MModel, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            BSM.OnIndexHourPolicy();
            return PartialView("GridItemLHour", BSM);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteLHour(HRLeaveHourPolicy MModel)
        {
            UserSession();
            DataSelector();
            var msg = BSM.OnGridModifyHour(MModel, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexHourPolicy();
            return PartialView("GridItemLHour", BSM);
        }
        #endregion
        #region Leave Condition
        public ActionResult GridItemLeave()
        {
            UserConf(ActionBehavior.VIEW);
            DataSelector();
            BSM.OnIndexCondition();
            return PartialView("GridItemLeave", BSM);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateLeave(HRLeaveDedPolicy MModel)
        {
            UserSession();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyLeaveCondition(MModel, SYActionBehavior.ADD.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            BSM.OnIndexCondition();
            return PartialView("GridItemLeave", BSM);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditLeave(HRLeaveDedPolicy MModel)
        {
            UserSession();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyLeaveCondition(MModel, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            BSM.OnIndexCondition();
            return PartialView("GridItemLeave", BSM);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteLeave(HRLeaveDedPolicy MModel)
        {
            UserSession();
            DataSelector();
            var msg = BSM.OnGridModifyLeaveCondition(MModel, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexCondition();
            return PartialView("GridItemLeave", BSM);
        }
        #endregion
        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelectorPolicy())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}