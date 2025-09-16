using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.LM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Leave
{
    public class GenerateLeaveController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRL0000001";
        private const string URL_SCREEN = "/HRM/Leave/GenerateLeave/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "ID";
        HumicaDBContext DB = new HumicaDBContext();

        public GenerateLeaveController()
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
            BSM.Filter.INYear = DateTime.Now.Year;
            BSM.ListEmpLeaveB = new List<HREmpLeaveB>();
            BSM.ListEmpGen = new List<Employee_Generate_Leave>();
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
            BSM.ListEmpGen = BSM.LoadDataEmpGen(BSM.Filter, SYConstant.getBranchDataAccess());

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
            var BSM = new GenerateLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
                //BSM.Filter.TemLeave = Temp;
            }
            //string TranNo = BSM.EmpID;
            if (BSM.EmpID != "")
            {
                var msg = BSM.GenerateLeave(BSM.EmpID);

                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            // DataList();
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
            return PartialView("GridItems", BSM.ListEmpGen);
        }
        [HttpPost]
        public string getEmpCode(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            GenerateLeaveObject BSM = new GenerateLeaveObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];

                BSM.EmpID = EmpCode;

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
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();// DH.HRBranches.ToList();
            //ViewData["SetEntitle_SELECT"] = DH.HRSetEntitleHs.ToList();
        }

        private void DataList()
        {
            //ViewData["LEAVETYPE_LIST"] = DH.HRLeaveTypes.ToList();
        }
    }
}
