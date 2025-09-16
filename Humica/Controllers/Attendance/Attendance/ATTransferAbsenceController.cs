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
    public class ATTransferAbsenceController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ATM0000018";
        private const string URL_SCREEN = "/Attendance/Attendance/ATTransferAbsence/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "TranNo";
		ITransferAbsenceObject BSM;
        public ATTransferAbsenceController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new TransferAbsenceObject();
            BSM.OnLoad();
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.ListDepartment = new List<HRDepartment>();
			BSM.Attendance = new FTFilterAttendance();
            BSM.Attendance.FromDate = DateTime.Now;
            BSM.Attendance.ToDate = DateTime.Now;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ITransferAbsenceObject)Session[Index_Sess_Obj + ActionName];
                BSM.Attendance = obj.Attendance;
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(TransferAbsenceObject BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.OnIndexLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        #endregion
        public ActionResult Transfer()
        {
            ActionName = "Index";
            UserSession();
			UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ITransferAbsenceObject)Session[Index_Sess_Obj + ActionName];
            }
            string TranNo = BSM.EmpID;
            var Fromdate = BSM.Attendance.FromDate;
			var ToDate = BSM.Attendance.ToDate;
			if (TranNo != null)
            {
                var msg = BSM.GenrateAttendance(TranNo, Fromdate, ToDate);
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
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.LIST);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ITransferAbsenceObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListDepartment);
        }
        [HttpPost]
        public string getEmpCode(string Code, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ITransferAbsenceObject)Session[Index_Sess_Obj + ActionName];
                BSM.EmpID = Code;
                string[] Emp = Code.Split(';');
                BSM.Progress = Emp.Count();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_ALLOWANCE");
            }
        }
    }
}