using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.LeaveBalance
{
    public class ESSResignController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000011";
        private const string URL_SCREEN = "/SelfService/LeaveBalance/ESSResign/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";

        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public ESSResignController()
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

            EmpResignObject BSM = new EmpResignObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (EmpResignObject)Session[Index_Sess_Obj + ActionName];
            }

            BSM.ListHeader = DB.HREmpResigns.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            EmpResignObject BSM = new EmpResignObject();
            BSM.ListHeader = new List<HREmpResign>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (EmpResignObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion 'list'
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            EmpResignObject BSD = new EmpResignObject();
            string cancel = SYDocumentStatus.CANCELLED.ToString();
            var chkEmp = DB.HREmpResigns.Where(w => w.EmpCode == user.UserName && w.Status != cancel).ToList();
            if (chkEmp.Count > 0)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EMP_RESIGN", user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            var emp = DBV.HR_STAFF_VIEW.Where(w => w.EmpCode == BSD.User.UserName).ToList();
            if (emp.Count > 0)
            {
                BSD.StaffInfor = DBV.HR_STAFF_VIEW.FirstOrDefault(x => x.EmpCode == BSD.User.UserName);
                BSD.EmpResign = new HREmpResign();
                BSD.ListHeader = new List<HREmpResign>();
                BSD.EmpResign.RequestedDate = DateTime.Now;
                BSD.EmpResign.Status = "OPEN";
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        [HttpPost]
        public ActionResult Create(EmpResignObject collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.ADD);

            var BSM = (EmpResignObject)Session[Index_Sess_Obj + ActionName];

            if (ModelState.IsValid)
            {
                string msg = collection.SaveResign();

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;

                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;

                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Index");
                }
                Session[Index_Sess_Obj + this.ActionName] = collection;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(collection);
            }
            Session[Index_Sess_Obj + this.ActionName] = collection;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(collection);
        }
        #endregion 'Create'
        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            EmpResignObject BSM = new EmpResignObject();
            ViewData[SYConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            int ID = Convert.ToInt32(id);
            BSM.EmpResign = DB.HREmpResigns.FirstOrDefault(w => w.ID == ID);
            if (BSM.EmpResign == null)
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("Error");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            if (user.UserName != BSM.EmpResign.EmpCode)
            {
                BSM.EmpResign.ReasonCEO = "#############";
            }
            BSM.StaffInfor = DBV.HR_STAFF_VIEW.FirstOrDefault(x => x.EmpCode == BSM.EmpResign.EmpCode);
            UserConfForm(SYActionBehavior.VIEW);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion 'Details'

        #region "Status"
        public ActionResult RequestForApproval(string id)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (id != null)
            {
                EmpResignObject BSM = new EmpResignObject();
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Request(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.Description = mess.Description + ". " + BSM.MessageError;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Index");

        }
        public ActionResult Approve(string id)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (id != null)
            {
                EmpResignObject BSM = new EmpResignObject();
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Approve(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.Description = mess.Description + ". " + BSM.MessageError;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Index");

        }
        public ActionResult Cancel(string id)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (id != null)
            {
                EmpResignObject BSM = new EmpResignObject();
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Cancel(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_CANCELLED", user.Lang);
                    mess.Description = mess.Description + ". " + BSM.MessageError;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Index");

        }
        #endregion 'Status'
    }
}
