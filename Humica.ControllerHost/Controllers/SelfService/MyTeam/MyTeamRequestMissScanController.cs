using Humica.Attendance;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.MyTeam
{
    public class MyTeamRequestMissScanController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000032";
        private const string URL_SCREEN = "/SelfService/MyTeam/MyTeamRequestMissScan/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "DocumentNo";
        private string DOCTYPE = "RLE01";
        IClsATRequestMissScan BSM;
        IUnitOfWork unitOfWork;

        public MyTeamRequestMissScanController()
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
            Session[DOCTYPE] = DOCTYPE;
            var userName = user.UserName;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
            }
            BSM.OnIndexLoading(true);
            BSM.ProcessMissScan(userName);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsATRequestMissScan collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
            }
            BSM.User = SYSession.getSessionUser();
            BSM.ListHeader = new List<ATEmpMissscan>();
            BSM.FInYear = collection.FInYear;
            var ListLeave = unitOfWork.Set<ATEmpMissscan>().Where(w => w.MissscanDate.Year == BSM.FInYear.INYear && w.EmpCode == BSM.User.UserName).OrderByDescending(x => x.MissscanDate).ToList();
            BSM.ListHeader = ListLeave.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.ListHeader);
        }
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListPending);
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
            BSM.Header = new ATEmpMissscan();
            DateTime DateNow = DateTime.Now;
            BSM.Header.EmpCode = user.UserName;
            var emp = unitOfWork.Set<HR_STAFF_VIEW>().Where(w => w.EmpCode == BSM.User.UserName).ToList();
            if (emp.Count > 0)
            {
                BSM.HeaderStaff = emp.FirstOrDefault(x => x.EmpCode == BSM.User.UserName);
                BSM.Header = new ATEmpMissscan();
                BSM.Header.MissscanDate = DateTime.Now;
                BSM.Header.RequestDate = DateTime.Now;
                BSM.Header.EmpCode = user.UserName;
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
        public ActionResult Create(string DocNo, ClsATRequestMissScan collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = DocNo;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeamRequestMissScan/Details/";
            BSM.DocType = DOCTYPE;
            BSM.ScreenId = SCREEN_ID;
            BSM.Header = collection.Header;
            BSM.HeaderStaff = collection.HeaderStaff;
            BSM.ScreenId = SCREEN_ID;
            if (BSM.Header != null)
            {
                string msg = BSM.Create(URL);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.DocumentNo.ToString();
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?DocumentNo=" + BSM.Header.DocumentNo;
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
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[SYSConstant.PARAM_ID] = DocumentNo;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Header = unitOfWork.Set<ATEmpMissscan>().FirstOrDefault(w => w.DocumentNo == DocumentNo);
            if (BSM.Header != null)
            {
                BSM.HeaderStaff = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                BSM.ListApproval = unitOfWork.Repository<ExDocApproval>().Where(w => w.DocumentNo == DocumentNo && w.DocumentType == BSM.Header.RequestType).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region "Status"
        public ActionResult Approve(string DocNo, string Remark)
        {
            UserSession();
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
            {
                BSM.ScreenId = SCREEN_ID;
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeamRequestMissScan/Details/";
                string msg = BSM.Approve(DocNo, URL, Remark);
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
        public ActionResult Reject(string DocNo, string Remark)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (DocNo == "null") DocNo = null;
            if (Remark == "null") Remark = null;
            if (!string.IsNullOrEmpty(DocNo) && !string.IsNullOrEmpty(Remark))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Reject(DocNo, Remark, true);
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
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("PLEASE_COMMENT", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        public ActionResult GridApproval()
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
            return PartialView("GridApproval", BSM.ListApproval);
        }
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
        private void DataSelector()
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
    }
}
