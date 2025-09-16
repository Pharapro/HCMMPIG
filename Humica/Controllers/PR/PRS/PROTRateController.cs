using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.WH.OP
{
    public class PROTRateController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRS0000002";
        private const string URL_SCREEN = "/PR/PRS/PROTRate/";
        private string KeyName = "OTCode";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        HumicaDBContext DB = new HumicaDBContext();

        public PROTRateController()
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
            PRSOTRate BSM = new PRSOTRate();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRSOTRate)Session[Index_Sess_Obj + ActionName];
            }
            var ListHeader = DB.PROTRates.ToList();
            foreach (var item in ListHeader)
            {
                item.Foperand = "(" + item.Foperand + ")" + item.Soperator + item.Toperand;
                if (item.Measure == "H") item.Measure = "Hour";
            }
            BSM.ListHeader = ListHeader.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PRSOTRate BSM = new PRSOTRate();
            BSM.ListHeader = new List<PROTRate>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRSOTRate)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            PRSOTRate BSM = new PRSOTRate();
            BSM.Header = new PROTRate();
            BSM.Header.Foperand = "B/W*H";
            BSM.Header.Soperator = "*";
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);

        }
        [HttpPost]
        public ActionResult Create(PRSOTRate MModel)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            PRSOTRate BSM = new PRSOTRate();
            BSM = MModel;
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateOTType();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.OTCode;
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
            ActionName = "Edit";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            DataSelector();
            if (id != null)
            {
                PRSOTRate BSM = new PRSOTRate();
                BSM.Header = DB.PROTRates.Find(id);
                if (BSM.Header != null)
                {
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("OTRATE_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, PRSOTRate BSM)
        {
            ActionName = "Edit";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            DataSelector();
            if (id == "null") id = null;
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditOTType(id);
                if (msg == SYConstant.OK)
                {
                    //MS001=Message load data has complate
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.OTCode;
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
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            DataSelector();
            if (id != null)
            {
                PRSOTRate BSM = new PRSOTRate();
                BSM.Header = DB.PROTRates.Find(id);
                if (BSM.Header != null)
                {
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MS001", user.Lang);
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("OTRATE_NE", user.Lang);
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
                PRSOTRate GLA = new PRSOTRate();
                string msg = GLA.DeleteOTType(id);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("OTRATE_DEL", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("OTRATE_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("LEAVETYPE");
            var List = objList.ListData.Where(w => w.SelectValue == "B/W*H").ToList();
            ViewData["LEAVETYPE_SELECT"] = List.ToList();
            objList = new SYDataList("Operator");
            ViewData["Operator_SELECT"] = objList.ListData;
        }
    }


}
