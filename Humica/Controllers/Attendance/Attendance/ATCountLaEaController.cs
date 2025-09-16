using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.Att;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.Attendance
{
    public class ATCountLaEaController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ATM0000010";
        private const string URL_SCREEN = "/Attendance/Attendance/ATCountLaEa/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();

        public ATCountLaEaController()
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

            ATEmpSchObject BSM = new ATEmpSchObject();
            BSM.ListSumLaEa = new List<ClsSumLaEa>();
            BSM.FMonthly = new Humica.Core.FT.FTMonthlySum();
            DateTime DNow = DateTime.Now;
            BSM.FMonthly.FromDate = new DateTime(DNow.Year, DNow.Month, 1);
            BSM.FMonthly.ToDate = new DateTime(DNow.Year, DNow.Month, DateTime.DaysInMonth(DNow.Year, DNow.Month));
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ATEmpSchObject BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.LoadDataLateEarly(BSM.FMonthly);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        #endregion
        public ActionResult TransferAtt(string TranNo)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new ATEmpSchObject();
            if (TranNo != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
                }
                var msg = BSM.TransferLateEarly(TranNo);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("TANSFER_COMPLATED", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            ATEmpSchObject BSM = new ATEmpSchObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListSumLaEa);
        }
        private void DataSelector()
        {
            var ListBranch = SYConstant.getBranchDataAccess();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            //ViewData["SHIFT_SELECT"] = DB.ATShifts.ToList();
            ViewData["BRANCHES_SELECT"] = ListBranch;// DB.HRBranches.ToList();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["LOCATION_SELECT"] = DB.HRLocations.ToList().OrderBy(w => w.Description);
            ViewData["LEAVETYPE_SELECT"] = DB.HRLeaveTypes.ToList();
        }
    }
}
