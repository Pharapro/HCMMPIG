using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{
    public class PRSCValueController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000011";
        private const string URL_SCREEN = "/PR/PRM/PRSCValue/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();

        public PRSCValueController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);

            PRSVCValueObject BSM = new PRSVCValueObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRSVCValueObject)Session[Index_Sess_Obj + ActionName];
            }

            BSM.ListHeader = DB.PRSVCValues.ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PRSVCValueObject BSM = new PRSVCValueObject();
            BSM.ListHeader = new List<PRSVCValue>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRSVCValueObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult Create()
        {
            ActionName = "Create";
            DataSelector();
            UserSession();
            PRSVCValueObject BSM = new PRSVCValueObject();
            UserConfListAndForm(this.KeyName);
            BSM.Header = new PRSVCValue();
            BSM.Header.Amount = 0;
            BSM.Header.TranDate = DateTime.Now;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);

        }
        [HttpPost]
        public ActionResult Create(PRSVCValueObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PRSVCValueObject BSM = new PRSVCValueObject();
            BSM = (PRSVCValueObject)Session[Index_Sess_Obj + ActionName];
            BSM.Header = collection.Header;
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                try
                {
                    string msg = BSM.CreateSVCValues();
                    if (msg == SYConstant.OK)
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        mess.DocumentNumber = BSM.Header.TranNo.ToString();
                        mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit/" + mess.DocumentNumber;
                        ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                        BSM.Header = new PRSVCValue();
                        BSM.Header.TranDate = DateTime.Now;
                        BSM.Header.Amount = 0;
                        return View(BSM);
                    }
                    else
                    {
                        ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                        return View(BSM);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
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
        public ActionResult Edit(int id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                PRSVCValueObject BSM = new PRSVCValueObject();
                BSM.Header = new PRSVCValue();
                BSM.Header = DB.PRSVCValues.FirstOrDefault(w => w.TranNo == id);
                if (BSM.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(int id, PRSVCValueObject collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            var BSM = new PRSVCValueObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRSVCValueObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }

            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditSVCValue(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
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
        public ActionResult Delete(int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id != 0)
            {
                PRSVCValueObject GLA = new PRSVCValueObject();
                string msg = GLA.DeleteSVCValues(id);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("SVC_DEL", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("SVC_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        private void DataSelector()
        {
        }
    }
}