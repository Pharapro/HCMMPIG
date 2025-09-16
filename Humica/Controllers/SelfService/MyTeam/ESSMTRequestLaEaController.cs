using DevExpress.CodeParser;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.MyTeam
{

    public class ESSMTRequestLaEaController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000016";
        private const string URL_SCREEN = "/SelfService/MyTeam/ESSMTRequestLaEa/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ReqLaEaNo";
        private string DOCTYPE = "RLE01";
        IClsReqLateEarly BSM;
        IUnitOfWork unitOfWork;
        public ESSMTRequestLaEaController()
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
            var userName = user.UserName;
            BSM.ListHeader = new List<HRReqLateEarly>();
            BSM.ListReqPending = new List<ClsReuestLaEa>();
            BSM.DocType = DOCTYPE;
            BSM.LoadData(userName);
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
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfList(KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsReqLateEarly)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListReqPending);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            BSM.Header = new HRReqLateEarly();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.Header.Qty = 0;
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.Header.LeaveDate = DateTime.Now;
            BSM.Header.Status = SYDocumentStatus.PENDING.ToString();
            BSM.DocType = DOCTYPE;
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
                BSM.HeaderStaff = collection.HeaderStaff;
                BSM.DocType = DOCTYPE;
                BSM.ScreenId = SCREEN_ID;
            }
            if (ModelState.IsValid)
            {
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSMTRequestLaEa/Details/";
                string msg = BSM.ESSRequestLaEa(URL);
                if (msg == SYConstant.OK)
                {
                    BSM.ScreenId = SCREEN_ID;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.ReqLaEaNo.ToString();
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + BSM.Header.ReqLaEaNo;
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
        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            BSM.Header = unitOfWork.Repository<HRReqLateEarly>().Queryable().FirstOrDefault(x => x.ReqLaEaNo == id);
            if (BSM.Header != null)
            {
                BSM.HeaderStaff = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                BSM.ListApproval = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == id && w.DocumentType == DOCTYPE).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, ClsReqLateEarly collection)
        {
            ActionName = "Edit";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsReqLateEarly)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.editReqLateEarly(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return View(BSM);

        }
        #endregion
        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ViewData[SYSConstant.PARAM_ID] = id;
            BSM.Header = unitOfWork.Repository<HRReqLateEarly>().Queryable().FirstOrDefault(x => x.ReqLaEaNo == id);
            if (BSM.Header != null)
            {
                BSM.HeaderStaff = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                BSM.ListApproval = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == id && w.DocumentType == DOCTYPE).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region "Approve"
        public ActionResult Approve(string id)
        {
            //ActionName = "Details";
            this.UserSession();
            DataSelector();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id.ToString() != "null")
            {
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/HROTRequest/Details/";
                BSM.DocType = DOCTYPE;
                string sms = BSM.ApproveOTReq(id, URL);
                if (sms == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(sms, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.Description = messageObject.Description + BSM.MessageError;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region "Reject"
        public ActionResult Reject(string id)
        {
            this.UserSession();
            DataSelector();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id.ToString() != "null")
            {
                BSM.DocType = DOCTYPE;
                string sms = BSM.RejectOTReq(id);
                if (sms == "OK")
                {
                    SYMessages messageObject = SYMessages.getMessageObject(sms, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
        }
        #endregion
        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            //ActionName = "Details";
            this.UserSession();
            DataSelector();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id.ToString() != "null")
            {
                string sms = BSM.CancelOTReq(id);
                if (sms == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(sms, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
        }
        #endregion
        #region "Ajax Approval"
        public ActionResult GridApprovalDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsReqLateEarly)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        #endregion
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {
            var EmpStaff = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmpType,
                    Branch = EmpStaff.Branch,
                    Division = EmpStaff.Division,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.LevelCode,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            SYDataList objListt = new SYDataList("Request_Misscan");
            ViewData["LEAVE_TIME_SELECT"] = objListt.ListData;
            SYDataList objList = new SYDataList("Request_Late_Early");
            ViewData["REQUEST_SELECT"] = objList.ListData;
            var ListBranch = SYConstant.getBranchDataAccess();
            var LisCompany = SYConstant.getCompanyDataAccess();
            var userCode = user.UserName;
            var Emp = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == userCode);
            if (Emp == null)
            {
                ViewData["STAFF_SELECT"] = new List<HRStaffProfile>();
                return;
            }
            var userBranch = Emp.Branch;
            var userCompanyCode = Emp.CompanyCode;
            var userDept = Emp.DEPT;
            var listStaff = unitOfWork.Repository<HRStaffProfile>().Queryable()
                .AsEnumerable()
                .Where(w => ListBranch.Any(x => x.Code == w.Branch && x.CompanyCode == w.CompanyCode) &&
                            w.DEPT == userDept && w.Status == "A").Select(x => new { x.EmpCode, x.DEPT, x.AllName }).ToList();
            var section = unitOfWork.Repository<HRStaffProfile>().Queryable()
                .Where(x => x.EmpCode == userCode).Select(x => x.DEPT).ToList();
            if (section.Any())
                listStaff = listStaff.Where(x => section.Contains(x.DEPT)).ToList();
            ViewData["STAFF_SELECT"] = listStaff;
        }

    }
}
