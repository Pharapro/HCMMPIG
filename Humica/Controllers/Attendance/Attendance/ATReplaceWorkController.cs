using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.Att;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.Attendance
{
    public class ATReplaceWorkController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ATM0000007";
        private const string URL_SCREEN = "/Attendance/Attendance/ATReplaceWork/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public ATReplaceWorkController()
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
            ATReplaceWorkObject BSM = new ATReplaceWorkObject();
            BSM.ListRelWork = new List<ATEmpRelWork>();
            var Staff = DB.HRStaffProfiles.Where(w => w.EmpCode == user.UserName|| (w.HODCode == user.UserName 
                || w.FirstLine == user.UserName || w.SecondLine == user.UserName || w.FirstLine2 == user.UserName || w.SecondLine2 == user.UserName)).ToList();
            if (Staff != null)
            {
                BSM.ListRelWork = DB.ATEmpRelWorks.AsEnumerable().Where(w => Staff.Where(x => w.FromEmpCode == x.EmpCode || w.ToEmpCode == x.EmpCode).Any()).ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            ATReplaceWorkObject BSM = new ATReplaceWorkObject();
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATReplaceWorkObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListRelWork);
        }
        #endregion
        #region'Create'
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ATReplaceWorkObject BSM = new ATReplaceWorkObject();
            BSM.Header = new ATEmpRelWork();
            BSM.Header.InDate = DateTime.Now;
            BSM.Header.RequestDate = DateTime.Now;
            BSM.ListRelWork = new List<ATEmpRelWork>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ATReplaceWorkObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
                collection.ScreenId = SCREEN_ID;
                ATReplaceWorkObject BSM = new ATReplaceWorkObject();
                BSM.Header = collection.Header;
                string msg = collection.CreateWork();

                if (msg == SYConstant.OK)
                {
                    BSM.ScreenId = SCREEN_ID;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM.Header = new ATEmpRelWork();
                    BSM.Header.InDate = DateTime.Now;
                    BSM.Header.RequestDate = DateTime.Now;
                    return View(BSM);
                }

                Session[Index_Sess_Obj + this.ActionName] = collection;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(collection);

            }
            Session[Index_Sess_Obj + this.ActionName] = collection;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(collection);

        }
        #endregion
        #region "Edit"

        public ActionResult Edit(int TranNo)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ATReplaceWorkObject BSM = new ATReplaceWorkObject();
            BSM.Header = new ATEmpRelWork();
            BSM.Header = DB.ATEmpRelWorks.FirstOrDefault(x => x.TranNo == TranNo);
            if (BSM.Header != null)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(ATReplaceWorkObject BSM, int TranNo)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditWork(TranNo);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }

        #endregion "Edit"
        #region "Delete"

        public ActionResult Delete(int TranNo)
        {
            ActionName = "Delete";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ATReplaceWorkObject BSM = new ATReplaceWorkObject();
            BSM.Header = new ATEmpRelWork();
            BSM.ScreenId = SCREEN_ID;
            string msg = BSM.DeleteWork(TranNo);
            msg = msg == SYConstant.OK ? "MS001" : msg;

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #endregion "Delete"
        #region view
        public ActionResult Details(int TranNo)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ATReplaceWorkObject BSM = new ATReplaceWorkObject();
            BSM.Header = new ATEmpRelWork();
            BSM.Header = DB.ATEmpRelWorks.FirstOrDefault(x => x.TranNo == TranNo);
            if (BSM.Header != null)
            {
                //Prepare parameters for edit and delete
                ViewData[SYConstant.PARAM_ID] = TranNo;
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        private void DataSelector()
        {
            var Staff = DB.HRStaffProfiles.Where(w => w.EmpCode == user.UserName).FirstOrDefault();
            if (Staff != null)
            {
                var ListBranch = SYConstant.getBranchDataAccess();
                //var ListStaff = DB.HRStaffProfiles.AsEnumerable().Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
                //ListStaff = ListStaff.Where(w => w.DEPT == Staff.DEPT).ToList();
                DateTime date = DateTime.Now.AddMonths(-1);
                var staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == user.UserName);
                var ListStaff = new List<HRStaffProfile>();
                if (staff != null)
                {
                    var _listEmp = DB.HRStaffProfiles.Where(w => w.FirstLine == staff.EmpCode || w.SecondLine == staff.EmpCode || w.HODCode == staff.EmpCode || w.EmpCode==user.UserName).ToList();
                    if (_listEmp.Count > 0)
                    {
                        ListStaff = _listEmp.ToList();
                    }
                    else
                    {
                        ListStaff = DB.HRStaffProfiles.AsEnumerable().Where(w => ListBranch.AsEnumerable().Where(x => x.Code == w.Branch).Any()).ToList();
                        ListStaff = ListStaff.Where(w => w.DEPT == staff.DEPT && (w.Status == "A" || (w.DateTerminate > date && w.Status == "I"))).ToList();
                    }
                }
                ViewData["EMPCODE_SELECT"] = ListStaff.ToList();
            }
        }
    }

}