using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.Atts;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.AttSetup
{

    public class SetupShiftController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ATS0000001";
        private const string URL_SCREEN = "/Attendance/AttSetup/SetupShift/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "Code";
        HumicaDBContext DB = new HumicaDBContext();
        public SetupShiftController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            SetupShiftObject BSM = new SetupShiftObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (SetupShiftObject)Session[Index_Sess_Obj + ActionName];
            }
            var ListHeader = DB.ATShifts;
            BSM.ListHeader = ListHeader.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            SetupShiftObject BSM = new SetupShiftObject();
            BSM.ListHeader = new List<ATShift>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SetupShiftObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            SetupShiftObject BSM = new SetupShiftObject();
            BSM.Header = new ATShift();
            DateTime DateNow = DateTime.Now;
            BSM.Header.OverNight1 = false;
            BSM.Header.SplitShift = false;
            BSM.Header.OverNight2 = false;
            BSM.Header.BreakFast = false;
            BSM.Header.Lunch = false;
            BSM.Header.Dinner = false;
            BSM.Header.NightMeal = false;
			BSM.Header.IsActive = true;
			//BSM.Header.CheckInBefore1 = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, 3, 0, 0);
			//BSM.Header.CheckInAfter1 = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, 10, 0, 0);
			//BSM.Header.CheckOutBefore1 = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, 9, 0, 0);
			//BSM.Header.CheckOutAfter1 = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, 14, 0, 0);
			BSM.Header.CheckIn1 = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, 6, 0, 0);
            BSM.Header.CheckOut1 = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, 12, 0, 0);
            BSM.Header.CheckIn2 = BSM.Header.CheckIn1;
            BSM.Header.CheckOut2 = BSM.Header.CheckOut1;
            BSM.Header.BreakStart = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, 12, 0, 0);
            BSM.Header.BreakEnd = new DateTime(DateNow.Year, DateNow.Month, DateNow.Day, 13, 0, 0);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(SetupShiftObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            var BSM = new SetupShiftObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SetupShiftObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            BSM.ScreenId = SCREEN_ID;
            string msg = BSM.CreateShift();

            if (msg == SYConstant.OK)
            {

                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSM.Header.Code;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;

                Session[Index_Sess_Obj + ActionName] = BSM;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
            }
            else
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);

        }
        #endregion
        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            if (id != null)
            {
                SetupShiftObject BSM = new SetupShiftObject();
                BSM.Header = DB.ATShifts.Find(id);
                if (BSM.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #endregion
        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            if (id != null)
            {
                SetupShiftObject BSM = new SetupShiftObject();
                BSM.Header = DB.ATShifts.Find(id);
                if (BSM.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("LEAVE_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(string id, SetupShiftObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            var BSM = new SetupShiftObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (SetupShiftObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditShift(id);
                if (msg == SYConstant.OK)
                {

                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.Code;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);

        }
        #endregion
        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id != null)
            {
                SetupShiftObject Del = new SetupShiftObject();
                string msg = Del.DeleteShift(id);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region 'PrivateCode'
        public ActionResult Get_CheckInOut1(DateTime FromDate, DateTime ToDate, Boolean OverNight)
        {
            DateTime DateNow = DateTime.Now;
            DateTime _FromDate = DateNow.Date + FromDate.TimeOfDay;
            DateTime _ToDate = DateNow.Date + ToDate.TimeOfDay;
            if (OverNight == true)
            {
                _ToDate = _ToDate.AddDays(1);
            }
            var total = _ToDate.Subtract(_FromDate).TotalHours;
            var result = new
            {
                MS = SYConstant.OK,
                totalHour1 = total
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        public ActionResult Get_CheckInOut2(DateTime FromDate, DateTime ToDate, Boolean OverNight)
        {
            DateTime DateNow = DateTime.Now;
            DateTime _FromDate = DateNow.Date + FromDate.TimeOfDay;
            DateTime _ToDate = DateNow.Date + ToDate.TimeOfDay;
            if (OverNight == true)
            {
                _ToDate = _ToDate.AddDays(1);
            }
            var total = _ToDate.Subtract(_FromDate).TotalHours;
            var result = new
            {
                MS = SYConstant.OK,
                totalHour2 = total
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {

        }
        #endregion
    }
}
