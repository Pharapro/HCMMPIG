using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRS
{
    public class PRParameterController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRS0000001";
        private const string URL_SCREEN = "/PR/PRS/PRParameter/";
        private string KeyName = "Code";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        HumicaDBContext DB = new HumicaDBContext();

        public PRParameterController()
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

            PRSParameter BSM = new PRSParameter();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRSParameter)Session[Index_Sess_Obj + ActionName];
            }
            var ListHeader = DB.PRParameters.ToList();
            DateTime inDate = DateTime.Now;
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            foreach (var item in ListHeader)
            {
                if (!item.IsPrevoiuseMonth.IsNullOrZero())
                {
                    PRPayParameterObject.GetDateTime(item, inDate, ref fromDate, ref toDate);
                    ListHeader.Where(w => w.Code == item.Code).ToList().ForEach(x => x.FROMDATE = fromDate);
                    ListHeader.Where(w => w.Code == item.Code).ToList().ForEach(x => x.TODATE = toDate);
                }
            }

            BSM.ListHeader = ListHeader.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            PRSParameter BSM = new PRSParameter();
            BSM.ListHeader = new List<PRParameter>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRSParameter)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            PRSParameter BSM = new PRSParameter();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ViewData[ClsConstant.IS_READ_ONLY] = false;
            ViewData[SYSConstant.PARAM_ID] = "";
            BSM.Header = new PRParameter();
            BSM.BIMonthlySalarySetting = new BiMonthlySalarySetting();
            BSM.Header.SALWKTYPE = 1;
            BSM.Header.ALWTYPE = 1;
            BSM.Header.DEDTYPE = 1;
            BSM.Header.OTWKTYPE = 1;

            BSM.Header.WDMONDay = 1;
            BSM.Header.WDTUEDay = 1;
            BSM.Header.WDWEDDay = 1;
            BSM.Header.WDTHUDay = 1;
            BSM.Header.WDFRIDay = 1;
            BSM.Header.WDSATDay = 1;
            BSM.Header.WDSUNDay = 1;
            BSM.Header.WHOUR = 8;
            BSM.Header.WHPWEEK = 48;
            BSM.Header.FROMDATE = DateTime.Now;
            BSM.Header.TODATE = DateTime.Now;

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);

        }
        [HttpPost]
        public ActionResult Create(PRSParameter MModel)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ViewData[ClsConstant.IS_READ_ONLY] = false;
            ViewData[SYSConstant.PARAM_ID] = "";
            PRSParameter BSM = new PRSParameter();
            BSM = MModel;
            if (ModelState.IsValid)
            {
                ViewData[ClsConstant.IS_READ_ONLY] = false;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateParameter();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.Code;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region "Edit"
        public ActionResult Edit(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ViewData[SYSConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = false;
            if (id != null)
            {
                PRSParameter BSM = new PRSParameter();
                BSM.Header = DB.PRParameters.Find(id);
                DateTime inDate = DateTime.Now;
                DateTime fromDate = new DateTime();
                DateTime toDate = new DateTime();
                if (BSM.Header != null)
                {
                    BSM.BIMonthlySalarySetting = DB.BiMonthlySalarySettings.ToList().Find(x => x.PayrollParameterID == id);
                    PRParameter item = BSM.Header;
                    PRPayParameterObject.GetDateTime(item, inDate, ref fromDate, ref toDate);
                    BSM.Header.FROMDATE = fromDate;
                    BSM.Header.TODATE = toDate;

                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, PRSParameter BSM)
        {
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ViewData[SYSConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.editParameter(id);
                if (msg == SYConstant.OK)
                {
                    //MS001=Message load data has complate
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.Code;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            //EE001= Message load Error data not complate
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);

        }
        #endregion
        #region "Detail"
        public ActionResult Details(string id)
        {
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;

            if (id != null)
            {
                PRSParameter BSM = new PRSParameter();
                BSM.Header = DB.PRParameters.Find(id);
                BSM.BIMonthlySalarySetting = DB.BiMonthlySalarySettings.ToList().Find(x => x.PayrollParameterID == id);
                PRParameter item = BSM.Header;
                DateTime inDate = DateTime.Now;
                DateTime fromDate = new DateTime();
                DateTime toDate = new DateTime();
                if (BSM.Header != null)
                {
                    PRPayParameterObject.GetDateTime(item, inDate, ref fromDate, ref toDate);
                    BSM.Header.FROMDATE = fromDate;
                    BSM.Header.TODATE = toDate;

                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MS001", user.Lang);
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            // DataSelector();
            if (id != null)
            {
                PRSParameter GLA = new PRSParameter();
                string msg = GLA.deleteParameter(id);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("PAR_DEL", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("PAR_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }

        #endregion
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("IS_ACCRUAL_TAX");
            ViewData["ISTAX_SELECT"] = objList.ListData;
        }
    }


}
