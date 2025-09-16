using Humica.Attendance;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using System;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.Attendance
{

    public class AttendanceCheckController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ATM0000004";
        private const string URL_SCREEN = "/Attendance/Attendance/AttendanceCheck/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "TranNo";
        IClsAttendanceCheck BSM;
        public AttendanceCheckController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsAttendanceCheck();
            BSM.OnLoad();
        }


        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexLoading();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IClsAttendanceCheck)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = obj.Filter;
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsAttendanceCheck BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexLoading(BSM.Filter);
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }

        #endregion
        public ActionResult CreateSch(VIEW_ATInOut MModel)
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsAttendanceCheck)Session[Index_Sess_Obj + ActionName];
                }
                var msg = BSM.OnGridModify(MModel, SYActionBehavior.ADD.ToString());
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
            return PartialView("GridItems", BSM.ListInOut);
        }
        public ActionResult EditSch(VIEW_ATInOut MModel)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (IClsAttendanceCheck)Session[Index_Sess_Obj + ActionName];
                    }
                    var msg = BSM.OnGridModify(MModel, SYActionBehavior.EDIT.ToString());
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
            return PartialView("GridItems", BSM.ListInOut);
        }
        public ActionResult DeleteSch(VIEW_ATInOut MModel)
        {
            ActionName = "Index";
            UserSession();
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsAttendanceCheck)Session[Index_Sess_Obj + ActionName];
                }
                var msg = BSM.OnGridModify(MModel, SYActionBehavior.DELETE.ToString());
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
            DataSelector();
            return PartialView("GridItems", BSM.ListInOut);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAttendanceCheck)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListInOut);
        }
        public ActionResult ShowData(string ID, string EmpCode)
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
        private void DataSelector()
        {
            foreach (var data in BSM.OnDataSelector())
            {
                ViewData[data.Key] = data.Value;
            }

            ViewData["BUSINESSUNIT_SELECT"] = ClsFilter.LoadBusinessUnit();
            ViewData["OFFICE_SELECT"] = ClsFilter.LoadOffice();
            ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
            ViewData["GROUP_SELECT"] = ClsFilter.LoadGroups();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
        }
    }
}
