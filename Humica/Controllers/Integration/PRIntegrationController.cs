using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{

    public class PRIntegrationController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRI0000001";
        private const string URL_SCREEN = "/PR/PRM/PRIntegration/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "IntegrateNo;Status";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public PRIntegrationController()
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

            PRGLmappingObject BSM = new PRGLmappingObject();
            BSM.Filter = new Humica.Core.FT.FTFilterData();
            BSM.Filter.PaymentDate = DateTime.Now;
            BSM.Filter.PostingDate = DateTime.Now;
            BSM.ListGLCharge = new List<ClsAccount>();
            BSM.ListGLReference = DBV.PR_InetAccount_view.ToList();
            BSM.ListHeaderAcc = DB.PRInteAccounts.ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult _GLCharge()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            PRGLmappingObject BSM = new PRGLmappingObject();
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_GLCharge", BSM.ListAccItem);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PRGLmappingObject BSM = new PRGLmappingObject();
            BSM.ListHeaderAcc = new List<PRInteAccount>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeaderAcc);
        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(KeyName);
            PRGLmappingObject BSM = new PRGLmappingObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListGLReference);
        }
        #endregion
        #region "Create"
        public ActionResult Create(int InYear, int InMonth, string Branch)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRGLmappingObject BSM = new PRGLmappingObject();
            BSM.HeaderAcc = new PRInteAccount();
            BSM.ListAccItem = new List<PRInteAccountItem>();
            BSM.ListAccItem = BSM.LoadDataGLItem(InYear, InMonth, Branch);
            if (BSM.ListAccItem.Count > 0)
            {
                BSM.HeaderAcc.DocumentDate = DateTime.Now;
                BSM.HeaderAcc.Status = SYDocumentStatus.OPEN.ToString();
                BSM.HeaderAcc.PaymentDate = new DateTime(InYear, InMonth, DateTime.DaysInMonth(InYear, InMonth));
                BSM.HeaderAcc.CreditAmount = 0;
                BSM.HeaderAcc.DebitAmount = 0;
                foreach (var item in BSM.ListAccItem)
                {
                    BSM.HeaderAcc.CreditAmount += Convert.ToDecimal(item.CreditAmount);
                    BSM.HeaderAcc.DebitAmount += Convert.ToDecimal(item.DebitAmount);
                }
                var Company = SMS.SYHRCompanies.First();
                if (Company != null)
                {
                    BSM.HeaderAcc.CompanyCode = Company.CompanyCode;
                    BSM.HeaderAcc.CurrencyCode = Company.BaseCurrency;
                }
                BSM.ScreenId = SCREEN_ID;
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("NO_REQUEST");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }

            return View(BSM);

        }

        [HttpPost]
        public ActionResult Create(int InYear, int InMonth, string Branch, PRGLmappingObject BSM)
        {
            UserSession();
            //DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
                BSM.ListAccItem = obj.ListAccItem;
            }
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                BSM.HeaderAcc.InYear = InYear;
                BSM.HeaderAcc.InMonth = InMonth;
                string msg = BSM.CreateGLPost();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.HeaderAcc.IntegrateNo;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }

        #endregion
        #region "Details"
        public ActionResult Details(string ID)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ViewData[SYSConstant.PARAM_ID] = ID;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (ID != null)
            {
                PRGLmappingObject BSM = new PRGLmappingObject();
                BSM.HeaderAcc = DB.PRInteAccounts.FirstOrDefault(w => w.IntegrateNo == ID);
                if (BSM.HeaderAcc != null)
                {
                    BSM.ListAccItem = DB.PRInteAccountItems.Where(w => w.IntegrateNo == ID).ToList();

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        public ActionResult CUSCEN(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id != null)
            {
                Humica.Integration.EF.StaffIntegrateObject BSM = new Humica.Integration.EF.StaffIntegrateObject();
                BSM.ScreenId = SCREEN_ID;
                BSM.Header = DB.PRInteAccounts.FirstOrDefault(w => w.IntegrateNo == id);
                if (BSM.Header != null)
                {
                    string msg = BSM.ReleaseGL(BSM.Header.IntegrateNo);
                    if (msg == SYConstant.OK)
                    {
                        var mess = SYMessages.getMessageObject("DOC_RELEASED", user.Lang);
                        mess.DocumentNumber = BSM.Header.IntegrateNo;
                        mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    }
                    else
                    {
                        var mess = SYMessages.getMessageObject(msg, user.Lang);
                        mess.DocumentNumber = BSM.Header.IntegrateNo;
                        mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                        Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                    }

                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Details?id=" + id);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Details?id=" + id);
        }

        public ActionResult Refreshvalue(string id, string action)
        {
            ActionName = action;
            PRGLmappingObject BSM = new PRGLmappingObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
                if (BSM.HeaderAcc == null)
                {
                    BSM.HeaderAcc = new PRInteAccount();
                }

                BSM.HeaderAcc.CreditAmount = 0;
                BSM.HeaderAcc.DebitAmount = 0;
                foreach (var item in BSM.ListAccItem)
                {
                    BSM.HeaderAcc.CreditAmount += Convert.ToDecimal(item.CreditAmount);
                    BSM.HeaderAcc.DebitAmount += Convert.ToDecimal(item.DebitAmount);
                }

                var result = new
                {
                    MS = SYConstant.OK,
                    CreditAmount = BSM.HeaderAcc.CreditAmount,
                    DebitAmount = BSM.HeaderAcc.DebitAmount
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["COMPANY_CODE"] = SMS.SYHRCompanies.ToList();
            ViewData["JOURNAL_SELECT"] = DB.HRJournalTypes.ToList();
        }
    }
}
