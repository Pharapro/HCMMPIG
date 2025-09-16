using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.LeaveBalance
{

    public class SwitchShiftController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "SWS0000001";
        private const string URL_SCREEN = "/SelfService/LeaveBalance/SwitchShift/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SwitchShiftController()
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

            ATSwitchShiftObject BSM = new ATSwitchShiftObject();
            //BSM.FInYear = new Humica.Core.FT.FTINYear();
            //BSM.FInYear.INYear = DateTime.Now.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATSwitchShiftObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.User = SYSession.getSessionUser();
            BSM.ListHeader = new List<ATSwitchShift>();

            BSM.ListHeader = DB.ATSwitchShifts.Where(w=>w.CreatedBy==user.UserName).ToList();


            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ATSwitchShiftObject collection)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ATSwitchShiftObject BSM = new ATSwitchShiftObject();
            BSM.User = SYSession.getSessionUser();
            BSM.ListHeader = new List<ATSwitchShift>();

            BSM.ListHeader = DB.ATSwitchShifts.ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ATSwitchShiftObject BSM = new ATSwitchShiftObject();
            //BSM.ListEmpLeaveB = new List<HREmpLeaveB>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATSwitchShiftObject)Session[Index_Sess_Obj + ActionName];
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
            ATSwitchShiftObject BSM = new ATSwitchShiftObject();
            BSM.User = SYSession.getSessionUser();
            BSM.Header = new ATSwitchShift();
            BSM.Header.FromDate = DateTime.Now;
            BSM.Header.ToDate = DateTime.Now;

            var emp = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == BSM.User.UserName);


            if (emp == null)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ATSwitchShiftObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);

            var BSM = new ATSwitchShiftObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATSwitchShiftObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            if (BSM.Header != null)
            {
                //string URL = SYUrl.getBaseUrl() + "/SelfService/LeaveBalance/LeaveRequestApp/Details/";
                string msg = BSM.CreateSwitch();

                if (msg == SYConstant.OK)
                {
                    BSM.ScreenId = SCREEN_ID;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.ID.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
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

        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            ATSwitchShiftObject BSM = new ATSwitchShiftObject();
            if (id == "null") id = null;
            if (id != null)
            {
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.ATSwitchShifts.FirstOrDefault(w => w.ID == TranNo);
                if (BSM.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }

        [HttpPost]
        public ActionResult Edit(int id, ATSwitchShiftObject collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            ATSwitchShiftObject BSM = new ATSwitchShiftObject();

            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ATSwitchShiftObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditShift(id);
                if (msg == SYConstant.OK)
                {
                    DB = new HumicaDBContext();
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
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
            ATSwitchShiftObject BSM = new ATSwitchShiftObject();
            if (id == "null") id = null;
            if (id != null)
            {
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.ATSwitchShifts.FirstOrDefault(w => w.ID == TranNo);
                if (BSM.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("LEAVE_TIME");
            ViewData["LEAVE_TIME_SELECT"] = objList.ListData;
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.Where(w => w.FirstLine == user.UserName || w.SecondLine == user.UserName || w.HODCode == user.UserName || w.EmpCode == user.UserName).ToList();
            SYDataList objList1 = new SYDataList("FUALTY_SELECT");
            ViewData["FUALTY_SELECT"] = objList1.ListData;
            ViewData["SELECT_SHIFT"] =DB.ATShifts.ToList();
        }
    }
}
