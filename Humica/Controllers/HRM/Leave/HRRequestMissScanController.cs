using Humica.Attendance;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Leave
{

    public class HRRequestMissScanController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "RMS0000001";
        private const string URL_SCREEN = "/HRM/Leave/HRRequestMissScan/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "DocumentNo";
        private string DOCTYPE = "RLE01";
        IClsATRequestMissScan BSM;
        IUnitOfWork unitOfWork;
        public HRRequestMissScanController()
            : base()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsATRequestMissScan();
            BSM.OnLoad();
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
            {
                var obj = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListPending = new List<RequestMissScan>();
            BSM.ListHeader = new List<ATEmpMissscan>();
            BSM.ListHeaderPending = new List<ATEmpMissscan>();
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListStaff = unitOfWork.Set<HRStaffProfile>().AsEnumerable().Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();

            var ListEmpLeaveReq = unitOfWork.Set<ATEmpMissscan>().Where(w => w.MissscanDate.Year == BSM.FInYear.INYear).ToList();
            ListEmpLeaveReq = ListEmpLeaveReq.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).OrderByDescending(w => w.MissscanDate).ToList();
            //BSM.FInYear.Status = "";
            
            string approved = SYDocumentStatus.APPROVED.ToString();
            string pending = SYDocumentStatus.PENDING.ToString();

            string open = SYDocumentStatus.OPEN.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();
            BSM.ListHeader = ListEmpLeaveReq.Where(x => x.Status == approved).ToList();
            BSM.ListHeaderPending = ListEmpLeaveReq.Where(x => x.Status == pending).ToList();
            var ListLeaveCreater = unitOfWork.Set<ATEmpMissscan>().AsEnumerable()
                              .Where(x => unitOfWork.Set<ExDocApproval>().Any(w => x.DocumentNo == w.DocumentNo && "MISSSCAN" == w.DocumentType && (x.Status != Cancel && x.Status != open))).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsATRequestMissScan collection)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.ListPending = new List<RequestMissScan>();
            BSM.FInYear = collection.FInYear;
            //    var ListBranch = SYConstant.getBranchDataAccess();
            //    //var ListStaff = DBV.HR_STAFF_VIEW.ToList();
            //    var ListStaff = unitOfWork.Set<HR_STAFF_VIEW>().AsEnumerable().Where(w => ListBranch.Where(x => x.Code == w.BranchID).Any()).ToList();

            //    // var ListEmpLeaveReq = DBV.HR_VIEW_EmpLeave.ToList();
            //    var ListEmpLeaveReq = unitOfWork.Set<ATEmpMissscan>().AsEnumerable().Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            //    var ListLeave = ListEmpLeaveReq.Where(w => w.MissscanDate.Year == collection.FInYear.INYear).ToList();
            //    string approved = SYDocumentStatus.APPROVED.ToString();
            //    string Reject = SYDocumentStatus.REJECTED.ToString();
            //    string Cancel = SYDocumentStatus.CANCELLED.ToString();

            //    BSM.ListHeader = ListEmpLeaveReq.Where(x => x.Status != approved && x.Status != Reject && x.Status != Cancel).ToList();

            //    if (BSM.FInYear.Status != null)
            //    {
            //    if (BSM.FInYear.Status == SYDocumentStatus.PENDING.ToString())
            //    {
            //        ListLeave = ListLeave.Where(x => x.Status != approved && x.Status != Reject && x.Status != Cancel).ToList();
            //    }
            //    else
            //    {
            //        ListLeave = ListLeave.Where(w => w.Status == BSM.FInYear.Status).ToList();
            //    }
            //}
            //ListLeave = ListLeave.OrderByDescending(x => x.MissscanDate).ToList();

            //BSM.ListHeader = ListLeave.OrderByDescending(w => w.MissscanDate).ToList();
            BSM.ListHeader = new List<ATEmpMissscan>();
            BSM.ListHeaderPending = new List<ATEmpMissscan>();
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListStaff = unitOfWork.Set<HRStaffProfile>().AsEnumerable().Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();

            var ListEmpLeaveReq = unitOfWork.Set<ATEmpMissscan>().Where(w => w.MissscanDate.Year == BSM.FInYear.INYear).ToList();
            ListEmpLeaveReq = ListEmpLeaveReq.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).OrderByDescending(w => w.MissscanDate).ToList();
            //BSM.FInYear.Status = "";

            string approved = SYDocumentStatus.APPROVED.ToString();
            string pending = SYDocumentStatus.PENDING.ToString();

            string open = SYDocumentStatus.OPEN.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();
            BSM.ListHeader = ListEmpLeaveReq.Where(x => x.Status == approved).ToList();
            BSM.ListHeaderPending = ListEmpLeaveReq.Where(x => x.Status == pending).ToList();
            var ListLeaveCreater = unitOfWork.Set<ATEmpMissscan>().AsEnumerable()
                              .Where(x => unitOfWork.Set<ExDocApproval>().Any(w => x.DocumentNo == w.DocumentNo && x.RequestType == w.DocumentType && (x.Status != Cancel && x.Status != open))).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.ListHeader = new List<ATEmpMissscan>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.ListHeader);
        }

        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfList(KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListHeaderPending);
        }

        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector("C");
            UserConfListAndForm(this.KeyName);
            BSM.User = SYSession.getSessionUser();
            BSM.ListHeader = new List<ATEmpMissscan>();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.Header = new ATEmpMissscan();
            BSM.Header.MissscanDate = DateTime.Now;
            BSM.Header.RequestDate = DateTime.Now;
            BSM.Header.EmpCode = user.UserName;
            Session["LEAVE_TYEP"] = null;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(string DocNo, ClsATRequestMissScan collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector("C");
            UserConfForm(SYActionBehavior.EDIT);
            BSM = new ClsATRequestMissScan();
            ViewData[SYSConstant.PARAM_ID] = DocNo;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            if (ModelState.IsValid)
            {
                string URL = SYUrl.getBaseUrl() + "/SelfService/HRRequestMissScan/Details/";
                BSM.DocType = DOCTYPE;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.HRCreate(DocNo);

                if (msg == SYConstant.OK)
                {
                    BSM.ScreenId = SCREEN_ID;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.DocumentNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?DocumentNo=" + mess.DocumentNumber;
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
            return View(BSM);
        }
        #endregion
        #region Edit
        public ActionResult Edit(string DocNo)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = DocNo;
            if (DocNo != null)
            {
                var msg = BSM.OnEditLoading(DocNo);
                if (msg != SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string DocNo, ClsATRequestMissScan collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ViewData[SYSConstant.PARAM_ID] = DocNo;
            if (DocNo == "null") DocNo = null;
            if (DocNo != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Update(DocNo);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = DocNo;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?DocumentNo=" + mess.DocumentNumber;
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
        public ActionResult Details(string DocumentNo)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = DocumentNo;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            BSM.OnDetailLoading(DocumentNo);
            if (BSM.Header != null)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
        }
        #endregion
        #region "Status"
        public ActionResult Approve(string DocNo)
        {
            UserSession();
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.HRApprove(DocNo);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.Description = mess.Description;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Reject(string DocNo)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.HRReject(DocNo);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    mess.Description = mess.Description;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region "Ajax Approval"
        public ActionResult GridApprovalDetail()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListApproval == null)
                {
                    BSM.ListApproval = new List<ExDocApproval>();
                }
            }
            DataSelector();
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        #endregion
        public ActionResult ShowDataEmp(string EmpCode)
        {
            HR_STAFF_VIEW EmpStaff = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    //EmpType = EmpStaff.EmpType,
                    Division = EmpStaff.DivisionDesc,
                    Branch = EmpStaff.Branch,
                    DEPT = EmpStaff.Department,
                    DEPTCode = EmpStaff.DEPT,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.LevelCode,
                    Position = EmpStaff.Position,
                    location = EmpStaff.Location,
                    StartDate = EmpStaff.StartDate
                };
                //GetData(EmpCode, "Create");
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector(string Action)
        {
            SYDataList objList = new SYDataList("Request_Late_Early");
            ViewData["REQUEST_SELECT"] = objList.ListData.Where(w => w.SelectValue == "MISSSCAN");
            SYDataList objListt = new SYDataList("Request_Misscan");
            ViewData["MISSSCAN_SELECT"] = objListt.ListData;

            var ListBranch = SYConstant.getBranchDataAccess();
            var ListStaff = unitOfWork.Set<HRStaffProfile>().AsEnumerable().Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            var Staff = ListStaff.FirstOrDefault(w => w.EmpCode == user.UserName);
            if (Staff != null)
            {
                ListStaff = ListStaff.Where(x => x.DEPT == Staff.DEPT).ToList();
            }
            ViewData["STAFF_SELECT"] = ListStaff.ToList();
        }
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("STATUS_LEAVE_APPROVAL");
            ViewData["STATUS_APPROVAL"] = objList.ListData;
        }
    }
}