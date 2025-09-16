using Humica.Attendance;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.Attendance
{
    public class GenerateAttenController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ATM0000005";
        private const string URL_SCREEN = "/Attendance/Attendance/GenerateAtten/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "TranNo";
        IAttendanceObject BSM;
        public GenerateAttenController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new AttendanceObject();
            BSM.OnLoad();
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            BSM.ListEmpSchdule = new List<VIEW_ATEmpSchedule>();
            BSM.Attendance = new FTFilterAttendance();
            BSM.Attendance.FromDate = DateTime.Now;
            BSM.Attendance.ToDate = DateTime.Now;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IAttendanceObject)Session[Index_Sess_Obj + ActionName];
                BSM.Attendance = obj.Attendance;
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(AttendanceObject BSM)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            BSM.OnIndexLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }

        #endregion
        public ActionResult Generate()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IAttendanceObject)Session[Index_Sess_Obj + ActionName];
            }
            string TranNo = BSM.EmpID;
            if (TranNo != null)
            {
                var msg = BSM.GenrateAttendance(TranNo);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult DefaultRoster(DateTime FromDate, DateTime ToDate)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var msg = BSM.Set_DefaultShift(FromDate, ToDate, SYConstant.getBranchDataAccess());

            if (msg == SYConstant.OK)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.LIST);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IAttendanceObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmpSchdule);
        }
        public ActionResult ShowData(string EmpCode)
        {
            BSM.OnFilterStaff(EmpCode);
            if (BSM.StaffProfile != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = BSM.StaffProfile.AllName,
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public string getEmpCode(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IAttendanceObject)Session[Index_Sess_Obj + ActionName];
                BSM.EmpID = EmpCode;
                string[] Emp = EmpCode.Split(';');
                BSM.Progress = Emp.Count();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_ALLOWANCE");
            }
        }
        private void DataSelector()
        {
            foreach (var data in BSM.OnDataSelector())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}