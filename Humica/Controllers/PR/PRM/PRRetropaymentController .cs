using Humica.Core.DB;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Humica.EF;
using Humica.Logic.PR;
using Humica.Models.SY;

namespace Humica.Controllers.PR.PRM
{
    public class PRRetropaymentController : EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000040";
        private const string URL_SCREEN = "/PR/PRM/PRRetropayment/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TrainNo";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public PRRetropaymentController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);
            PRRetropayment BSM = new PRRetropayment();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRRetropayment)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = DB.PREmpRetroPaymen.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PRRetropayment BSM = new PRRetropayment();
         
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRRetropayment)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            DataSelector();
            UserSession();
            PRRetropayment BSM = new PRRetropayment();
            UserConfListAndForm(this.KeyName);
            BSM.Header = new PREmpRetroPayman();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.ListHeader = new List<PREmpRetroPayman>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(PRRetropayment collection)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ActionName = "Create";
            PRRetropayment BSM = new PRRetropayment();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PRRetropayment)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                    BSM.HeaderStaff = collection.HeaderStaff;
                }

                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateRetro();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.TrainNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
        }
        #endregion
        #region Edit
        public ActionResult Edit(string id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            if (id != null)
            {
                PRRetropayment BSM = new PRRetropayment();
                BSM.Header = new PREmpRetroPayman();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.PREmpRetroPaymen.FirstOrDefault(w => w.TrainNo == TranNo);
                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(int id, PRRetropayment collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            PRRetropayment BSM = new PRRetropayment();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PRRetropayment)Session[Index_Sess_Obj + ActionName];
                    BSM.HeaderStaff = collection.HeaderStaff;
                    BSM.Header = collection.Header;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditRetro(id);
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
        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id == "null") id = null;
            if (id != null)
            {
                int TranNo = Convert.ToInt32(id);
                PRRetropayment Del = new PRRetropayment();
                string msg = Del.Delete(TranNo);
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
        #endregion
        #endregion
        #region "Details"
        public ActionResult Details(string id)
        {
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id == "null") id = null;
            if (id != null)
            {
                PRRetropayment BSM = new PRRetropayment();
                BSM.Header = new PREmpRetroPayman();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.PREmpRetroPaymen.FirstOrDefault(w => w.TrainNo == TranNo);
                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #endregion

        private void DataSelector()
        {
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            ViewData["STAFF_SELECT"] = Staff;// DB.HRStaffProfiles.ToList();
            ViewData["STAFF_SELECT"] = Staff;// DB.HRStaffProfiles.ToList();
            
        }
    }
}
