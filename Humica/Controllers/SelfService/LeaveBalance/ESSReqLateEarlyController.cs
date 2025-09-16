using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.LeaveBalance
{
    public class ESSReqLateEarlyController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000015";
        private const string URL_SCREEN = "/SelfService/LeaveBalance/ESSReqLateEarly/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ReqLaEaNo";
        private string DOCTYPE = "RLE01";
        IClsReqLateEarly BSM;
        IUnitOfWork unitOfWork;
        public ESSReqLateEarlyController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsReqLateEarly();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.FInYear = new Humica.Core.FT.FTINYear();
            BSM.FInYear.INYear = DateTime.Now.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
                BSM = (ClsReqLateEarly)Session[Index_Sess_Obj + ActionName];
            BSM.User = SYSession.getSessionUser();
            BSM.ListHeader = new List<HRReqLateEarly>();
            BSM.ListHeader = unitOfWork.Repository<HRReqLateEarly>().Queryable().Where(w => w.LeaveDate.Year == BSM.FInYear.INYear && w.EmpCode == BSM.User.UserName).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsReqLateEarly collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.User = SYSession.getSessionUser();
            BSM.ListHeader = new List<HRReqLateEarly>();
            BSM.FInYear = collection.FInYear;
            var ListLeave = unitOfWork.Repository<HRReqLateEarly>().Queryable().Where(w => w.LeaveDate.Year == BSM.FInYear.INYear && w.EmpCode == BSM.User.UserName).OrderByDescending(x => x.LeaveDate).ToList();
            if (ListLeave.Any())
                BSM.ListHeader = ListLeave.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.ListHeader = new List<HRReqLateEarly>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsReqLateEarly)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);

            BSM.User = SYSession.getSessionUser();
            BSM.Header = new HRReqLateEarly();
            var emp = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().Where(w => w.EmpCode == BSM.User.UserName).ToList();
            if (emp.Any())
            {
                BSM.HeaderStaff = emp.FirstOrDefault(x => x.EmpCode == BSM.User.UserName);
                BSM.Header = new HRReqLateEarly();
                BSM.Header.Qty = 0;
                BSM.Header.LeaveDate = DateTime.Now;
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(string ID, ClsReqLateEarly collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsReqLateEarly)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            if (ModelState.IsValid)
            {
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSMTRequestLaEa/Details/";
                BSM.DocType = DOCTYPE;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.ESSRequestLaEa(URL);

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.ReqLaEaNo.ToString();
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + BSM.Header.ReqLaEaNo.ToString();
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
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[SYSConstant.PARAM_ID] = id;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsReqLateEarly)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Header = unitOfWork.Repository<HRReqLateEarly>().Queryable().FirstOrDefault(w => w.ReqLaEaNo == id);
            if (BSM.Header != null)
            {
                BSM.ListApproval = unitOfWork.Repository<ExDocApproval>().Where(w => w.DocumentNo == id && w.DocumentType == DOCTYPE).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        public ActionResult GridApproval()
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);

            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsReqLateEarly)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridApproval";
            return PartialView("GridApproval", BSM.ListApproval);
        }
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("Request_Late_Early");
            ViewData["REQUEST_SELECT"] = objList.ListData;
            SYDataList objListt = new SYDataList("Request_Misscan");
            ViewData["LEAVE_TIME_SELECT"] = objListt.ListData;
        }
    }
}
