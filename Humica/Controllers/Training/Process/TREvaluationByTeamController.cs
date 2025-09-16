
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EForm;
using Humica.Training;
using System;
using System.Web.Mvc;

namespace Humica.Controllers.Training.Process
{
    public class TREvaluationByTeamController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "TR00000013";
        private const string URL_SCREEN = "/Training/Process/TREvaluationByTeam/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ApprID";
        protected IClsEFEmpPortal BSM;
        public TREvaluationByTeamController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsEFEmpPortal();
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
        public ActionResult Index(ClsEFEmpPortal BSM)
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
                BSM = (IClsEFEmpPortal)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmpStaff);
        }
        public ActionResult AssignStaff(string PortalType, DateTime DocumentDate,
            string EmpCode, DateTime Deadline,string SurveyName)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEFEmpPortal)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ScreenId = SCREEN_ID;
            if (EmpCode != "")
            {
                EFEmpPortal Obj = new EFEmpPortal();
                Obj.PortalType = PortalType;
                Obj.DocumentDate = DocumentDate;
                Obj.SurveyName = SurveyName;
                Obj.Deadline = Deadline;
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