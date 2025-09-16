using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.PR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HR.Controllers.PR.PRM
{

    public class GenerateSeverancePayController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "PRM0000029";
        private const string URL_SCREEN = "/PR/PRM/GenerateSeverancePay/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "ID";
        HumicaDBContext DB = new HumicaDBContext();
        public GenerateSeverancePayController()
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

            PRGenerateSeverance BSM = new PRGenerateSeverance();
            BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
            BSM.ListEmpSeverance = new List<EmpSeverance>();
            BSM.ListDataYear = new List<GetDataYear>();
            BSM.ListYear = new List<GetDataYear>();
            BSM.Filter.FromDate = DateTime.Now;
            BSM.Filter.ToDate = DateTime.Now;
            BSM.Filter.InMonth = DateTime.Now;
            BSM.Filter.ContractType = "H-FDC";
            BSM.Filter.Rate = 5;
            BSM.InYear = DateTime.Now.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRGenerateSeverance)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = obj.Filter;
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(PRGenerateSeverance BSM)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.ListDataYear = new List<GetDataYear>();
            BSM.ListYear = new List<GetDataYear>();
            BSM.ListEmpSeverance = BSM.LoadDataSeverance(BSM.Filter, SYConstant.getBranchDataAccess());
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRGenerateSeverance)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        #endregion
        public ActionResult Generate(string EmpCode)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new PRGenerateSeverance();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGenerateSeverance)Session[Index_Sess_Obj + ActionName];
            }
            if (EmpCode != "")
            {
                var msg = BSM.Generate_Severance(EmpCode, BSM.Filter);

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
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            PRGenerateSeverance BSM = new PRGenerateSeverance();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGenerateSeverance)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmpSeverance);
        }
        private void DataSelector()
        {
            string DEDType = "ALLW";
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData["ALLW_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == DEDType).ToList();
            ViewData["SALARTYPE_SELECT"] = ClsSelaryType.LoadData();
            ViewData["CONTRACT_SELECT"] = DB.HRContractTypes.ToList();
        }

    }
}
