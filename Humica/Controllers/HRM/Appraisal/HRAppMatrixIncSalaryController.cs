using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using Humica.Performance;
using System;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HRAppMatrixIncSalaryController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRA0000014";
        private const string URL_SCREEN = "/HRM/Appraisal/HRAppMatrixIncSalary/";
        private string ActionName;
        private string KeyName = "ID";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string PARAM_INDEX = "ID;";
        protected IClsAPPIncreaseSalary BSM;
        //IUnitOfWork unitOfWork;
        public HRAppMatrixIncSalaryController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsAPPIncreaseSalary();
            BSM.OnLoad();
        }

        #region List
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            DataSelector();
            BSM.OnIndexLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;
            ViewData[ClsConstant.PARAM_INDEX] = PARAM_INDEX;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsAPPIncreaseSalary BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM.OnLoadingFilter(BSM.Filter, SYConstant.getBranchDataAccess());
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion
        #region "Save"
        public ActionResult Save()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
            }
            if (BSM.ListMatrixIncrease.Count > 0)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CareerMatrix();

                if (msg == SYConstant.OK)
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MS001", user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region Matrix
        public ActionResult GridMatix()
        {
            ActionName = "Index";
            UserConf(ActionBehavior.EDIT);
            BSM = (IClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
            return PartialView("GidMatixIncrease", BSM.ListMatrixIncrease);
        }
        public ActionResult ItemEdit(HRAPPMatrixIncreaseSalary MModel)
        {
            ActionName = "Index";
            UserSession();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
                var msg = BSM.EditMatrix_Adding(MModel);
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                else
                    ViewData["EditError"] = SYMessages.getMessage(msg) + BSM.MessageError;
            }
            else
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            return PartialView("GidMatixIncrease", BSM.ListMatrixIncrease);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ItemDelete(HRAPPMatrixIncreaseSalary MModel)
        {
            ActionName = "Index";
            UserSession();
            BSM = (IClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
            try
            {
                BSM.OnGridModifyMaxSalary(MModel, SYActionBehavior.DELETE.ToString());
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("GidMatixIncrease", BSM.ListMatrixIncrease);
        }
        #endregion
        public ActionResult Refreshvalue()
        {
            ActionName = "Index";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
                BSM.Refreshvalue(BSM.ListMatrixIncrease, BSM.Filter.INYear);
                var result = new
                {
                    MS = SYConstant.OK,
                    TotalSalary = BSM.TotalSalary,
                    SalaryInBgP = BSM.SalaryInBgP,
                    SalaryIncBgUSD = BSM.SalaryIncBgUSD,
                    SalaryIncBgUtilised = BSM.SalaryIncBgUtilised,
                    SalaryIncBgBalance = BSM.SalaryIncBgBalance,
                };
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
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