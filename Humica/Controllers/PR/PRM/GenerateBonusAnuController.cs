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

    public class GenerateBonusAnuController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "PRM0000013";
        private const string URL_SCREEN = "/PR/PRM/GenerateBonusAnu/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "ID";
        HumicaDBContext DB = new HumicaDBContext();

        public GenerateBonusAnuController()
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

            PRBonusObject BSM = new PRBonusObject();
            BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
            BSM.ListTemBonusAnnual = new List<BonusAnnual>();
            BSM.Filter.INYear = DateTime.Now.Year;
            BSM.Filter.InMonth = DateTime.Now;
            BSM.Filter.IsNewJoin = false;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRBonusObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(PRBonusObject BSM)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.ListTemBonusAnnual = BSM.ListEmployeeBonusAnnual(BSM.Filter, SYConstant.getBranchDataAccess());

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        #endregion
        public ActionResult Generate(int InYear)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new PRBonusObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRBonusObject)Session[Index_Sess_Obj + ActionName];
                //   BSM.Filter.InMonth = collection.Filter;
            }

            var msg = BSM.GenerateBonus(InYear);

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
        public ActionResult Transfer(string TranNo, string BonusType, DateTime InMonth)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new PRBonusObject();
            if (TranNo != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PRBonusObject)Session[Index_Sess_Obj + ActionName];
                }
                var msg = BSM.TransferBonus(TranNo, BonusType, InMonth);

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
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("TANSFER_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            PRBonusObject BSM = new PRBonusObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRBonusObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListTemBonusAnnual);
        }
        private void DataSelector()
        {
            string DEDType = "BON";
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();// DB.HRBranches.ToList();
            ViewData["EMPTYPE_SELECT"] = DB.HREmpTypes.ToList();
            ViewData["BON_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == DEDType).ToList();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
        }

    }
}
