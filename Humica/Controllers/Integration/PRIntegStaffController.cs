using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Integration.EF;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{

    public class PRIntegStaffController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRI0000002";
        private const string URL_SCREEN = "/PR/PRM/PRIntegStaff/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "IntegrateNo";

        public PRIntegStaffController()
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

            StaffIntegrateObject BSM = new StaffIntegrateObject();
            BSM.ListCareerHis = BSM.LoadStaff();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            StaffIntegrateObject BSM = new StaffIntegrateObject();
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (StaffIntegrateObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM.ListCareerHis);
        }
        #endregion
        public ActionResult CUSCEN()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new StaffIntegrateObject();
            string EmpCode = "";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (StaffIntegrateObject)Session[Index_Sess_Obj + ActionName];
                EmpCode = BSM.EmpCode;
            }
            if (EmpCode != "")
            {
                var msg = BSM.TransferStaff(EmpCode);

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
        [HttpPost]
        public string getEmpCode(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            StaffIntegrateObject BSM = new StaffIntegrateObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (StaffIntegrateObject)Session[Index_Sess_Obj + ActionName];

                BSM.EmpCode = EmpCode;

                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_EMPLOYEE");
            }
        }
        private void DataSelector()
        {
            //ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            //ViewData["COMPANY_CODE"] = DB.SYHRCompanies.ToList();
            //ViewData["JOURNAL_SELECT"] = DH.HRJournalTypes.ToList();
        }
    }
}
