using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Payroll;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{
    public class PRGenerateFirstPayController : Humica.EF.Controllers.MasterSaleController
    {
        private static string Error = "";
        private const string SCREEN_ID = "PRM0000019";
        private const string URL_SCREEN = "/PR/PRM/PRGenerateFirstPay/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "EmpCode";

        IClsPRFirstPaySalary BSM;
        public PRGenerateFirstPayController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsPRFirstPaySalary();
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
                var obj = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = obj.Filter;
                BSM.ListLog = obj.ListLog;
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsPRFirstPaySalary BSM)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            var msg = BSM.OnIndexLoadingFilter();
            if (msg != SYConstant.OK)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            BSM.Progress = BSM.ListEmployeeGen.Count();
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

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
            }
            var msg = BSM.GenerateSalaryFirstPay();
            if (msg == SYConstant.OK)
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Delete(int Period)
        {
            UserSession();
            UserConfForm(SYActionBehavior.DELETE);
            DataSelector();
            BSM.ScreenId = SCREEN_ID;
            string msg = BSM.Delete_GenerateAll(Period);
            if (msg == SYConstant.OK)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmployeeGen);
        }
        public ActionResult GridItemLog()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.LIST);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemLog", BSM.ListLog);
        }
        [HttpPost]
        public string getEmpCode(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
                BSM.EmpID = EmpCode;
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_EMPLOYEE");
            }
        }
        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelectorLoading())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}
