using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{
    public class PRGenerateSCController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000012";
        private const string URL_SCREEN = "/PR/PRM/PRGenerateSC/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public PRGenerateSCController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            PREmpSVCObject BSM = new PREmpSVCObject();
            BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
            BSM.ListEmpSVC = new List<HR_EMPSVC_VIEW>();
            BSM.Filter.InMonth = DateTime.Now;
            BSM.Filter.Amount = 0;
            BSM.Filter.Day = 26;
            var ExchValue = DB.PRSVCValues.FirstOrDefault(w => w.InYear == BSM.Filter.InMonth.Year && w.InMonth == BSM.Filter.InMonth.Month);
            if (ExchValue != null) BSM.Filter.Amount = Convert.ToDecimal(ExchValue.Amount);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PREmpSVCObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(PREmpSVCObject BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.ListEmpSVC = BSM.LoadDataGenSVC(BSM.Filter.InMonth);

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }

        public ActionResult Calculate(string EmpCode)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new PREmpSVCObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpSVCObject)Session[Index_Sess_Obj + ActionName];
                //BSM.Filter.InMonth = Temp;
            }
            if (EmpCode != "")
            {

                var msg = BSM.getCalculate_NoSVCDay(EmpCode);

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
        public ActionResult Generate(string EmpCode)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new PREmpSVCObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpSVCObject)Session[Index_Sess_Obj + ActionName];
                //BSM.Filter.InMonth = Temp;
            }
            if (EmpCode != "")
            {

                var msg = BSM.Calculate_SVC();

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
        public ActionResult Delete(string EmpCode, DateTime INMonth)
        {
            UserSession();
            UserConfForm(SYActionBehavior.DELETE);
            DataSelector();
            if (EmpCode != null)
            {
                PREmpSVCObject GLA = new PREmpSVCObject();
                string msg = GLA.Delete_Emp_SVC(EmpCode, INMonth);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }

        public ActionResult ShowData(DateTime InMonth)
        {
            decimal Exch = 0;
            var ExchValue = DB.PRSVCValues.FirstOrDefault(w => w.InYear == InMonth.Year && w.InMonth == InMonth.Month);
            if (ExchValue != null) Exch = Convert.ToDecimal(ExchValue.Amount);
            var result = new
            {
                MS = SYConstant.OK,
                Exchange = Exch
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            PREmpSVCObject BSM = new PREmpSVCObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpSVCObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmpSVC);
        }
        private void DataSelector()
        {

            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
            string ALLType = "ALLW";
            ViewData["ALLOWANCE_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == ALLType).ToList();
        }
    }
}