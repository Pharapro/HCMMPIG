using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.LM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Leave
{

    public class ForwardLeaveController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRL0000002";
        private const string URL_SCREEN = "/HRM/Leave/ForwardLeave/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "EmpCode";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();

        public ForwardLeaveController()
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

            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.Filter = new Humica.Core.FT.FTFilterData();
            BSM.Filter.FYear = DateTime.Now.Year - 1;
            BSM.Filter.TYear = DateTime.Now.Year;
            var maxforward = SMS.SYSettings.FirstOrDefault(w => w.SettingName == "LEAVE_FORWARD_BANANCE");
            if (maxforward != null) BSM.Filter.MaxForward = Convert.ToDecimal(maxforward.SettinValue);
            else BSM.Filter.MaxForward = 5;
            BSM.Filter.LeaveType = "AL";
            BSM.Filter.ForwardExp = new DateTime(DateTime.Now.Year, 3, 31);
            BSM.ListForward = new List<Employee_ListForwardLeave>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(GenerateLeaveObject BSM)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.ListForward = BSM.LoadDataEmpForward(BSM.Filter, SYConstant.getBranchDataAccess());

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        #endregion
        public ActionResult Transfer(string EmpCode, int TYear)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new GenerateLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
                BSM.Filter.TYear = TYear;
            }
            if (EmpCode != "")
            {

                var msg = BSM.TransferLeave(EmpCode);

                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("TRANSFER_COMPLATED", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("TRANSFER_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListForward);
        }
        private void DataSelector()
        {
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();// DH.HRBranches.ToList();
            ViewData["LeaveTypes_SELECT"] = DB.HRLeaveTypes.ToList();
            ViewData["Levels_SELECT"] = SMS.HRLevels.ToList();

        }
    }
}
