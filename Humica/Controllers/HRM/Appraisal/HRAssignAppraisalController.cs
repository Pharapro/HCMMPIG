using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Performance;
using System;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HRAssignAppraisalController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRA0000011";
        private const string URL_SCREEN = "/HRM/Appraisal/HRAssignAppraisal/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ApprID";
        protected IClsAppraisel BSM;
        public HRAssignAppraisalController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsAppraisel();
            BSM.OnLoad();
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexLoadingTeam();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsAppraisel BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.LoadData(BSM.Filter, SYConstant.getBranchDataAccess());
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        #endregion
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisel)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmpStaff);
        }

        [HttpPost]
        public ActionResult ReasonDoc(string id)
        {
            ActionName = "Index";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisel)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Header = new HREmpAppraisal();
            if (Request.Form["Deadline"] != null)
            {
                string date = Request.Form["Deadline"].ToString();
                BSM.Header.AppraiserDeadline = Convert.ToDateTime(date);
            }
            if (Request.Form["Deadline2"] != null)
            {
                string date = Request.Form["Deadline2"].ToString();
                BSM.Header.AppraiserDeadline2 = Convert.ToDateTime(date);
            }
            if (Request.Form["KPIType"] != null)
            {
                BSM.Header.KPIType = Request.Form["KPIType"].ToString();
            }

            if (Request.Form["KPIDeadline"] != null)
            {
                string date = Request.Form["KPIDeadline"].ToString();
                BSM.Header.KPIDeadline = Convert.ToDateTime(date);
            }
            if (Request.Form["KPIExpectedDate"] != null)
            {
                string date = Request.Form["KPIExpectedDate"].ToString();
                if (Convert.ToDateTime(date).Year != 1970)
                    BSM.Header.KPIExpectedDate = Convert.ToDateTime(date);
            }
            //if (Request.Form["Appraiser2"] != null)
            //{
            //    BSM.Header.AppraiserCode2 = Request.Form["Appraiser2"].ToString();
            //}
            Session[Index_Sess_Obj + ActionName] = BSM;
            var result = new
            {
                MS = SYConstant.OK,
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        public ActionResult AssignStaff(string AppType, DateTime AppraiserDate,
            string EmpCode, int InYear, DateTime PeriodFrom, DateTime PeriodTo)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisel)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ScreenId = SCREEN_ID;
            if (EmpCode != "")
            {
                HREmpAppraisal Obj = new HREmpAppraisal();
                if (BSM.Header != null)
                    Obj = BSM.Header;
                Obj.AppraisalType = AppType;
                Obj.AppraiserDate = AppraiserDate;
                Obj.InYear = InYear;
                Obj.PeriodFrom = PeriodFrom;
                Obj.PeriodTo = PeriodTo;
                var msg = BSM.CreateMulti(EmpCode,Obj);

                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MS001", user.Lang);
                    ViewData[SYSConstant.PARAM_ID] = EmpCode;
                }
                else
                {
                    SYMessages mess = SYMessages.getMessageObject(msg, user.Lang);
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelectorTeam())
            {
                ViewData[data.Key] = data.Value;
            }

        }
    }
}