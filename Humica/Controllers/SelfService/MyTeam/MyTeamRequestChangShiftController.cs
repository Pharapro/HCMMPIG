using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.EForm;
using Humica.Models.SY;
using HUMICA.Models.Report.EFORM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using static Humica.EForm.ClsRequestChangShift;

namespace Humica.Controllers.SelfService.MyTeam
{
    public class MyTeamRequestChangShiftController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESS0000024";
        private const string URL_SCREEN = "/SelfService/MyTeam/MyTeamRequestChangShift/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        private string DOCTYPE = "CHSH";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        IClsRequestChangShift BSM;
        IUnitOfWork unitOfWork;
        public MyTeamRequestChangShiftController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsRequestChangShift();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }

        #region List
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            var userName = user.UserName;
            BSM.ListRequestPending = new List<ClsRequestChangeShift>();
            BSM.listHeader = new List<HREFReqChangShift>();
            BSM.ProcessChangeShift(userName);

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            BSM.listHeader = new List<HREFReqChangShift>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRequestChangShift)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.listHeader);
        }
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfList(KeyName);

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRequestChangShift)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListRequestPending);
        }
        #endregion
        #region Create
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.Header = new HREFReqChangShift();
            BSM.listHeader = new List<HREFReqChangShift>();
            var Staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == user.UserName);
            if (BSM.User.UserName != null && Staff != null)
            {
                BSM.HeaderStaff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(x => x.EmpCode == BSM.User.UserName);
                BSM.Header.EmpCode = user.UserName;
                BSM.Header.Status = "OPEN";
                BSM.Header.RequestDate = DateTime.Now;
                BSM.DocType = DOCTYPE;
                if (Staff != null)
                    BSM.SetAutoApproval(SCREEN_ID, BSM.DocType, Staff.Branch, Staff.DEPT);
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
        public ActionResult Create(ClsRequestChangShift collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.ADD);
            DataSelector();
            collection.ListApproval = BSM.ListApproval;
            collection.ScreenId = SCREEN_ID;
            if (collection.Header != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var obj = (ClsRequestChangShift)Session[Index_Sess_Obj + ActionName];
                    BSM.ListApproval = obj.ListApproval;
                }
                BSM.Header = collection.Header;
                if (Session[PATH_FILE] != null)
                {
                    BSM.Header.AttachFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                BSM.HeaderStaff = collection.HeaderStaff;
                BSM.ScreenId = SCREEN_ID;
                BSM.DocType = DOCTYPE;
                string msg = BSM.CreateReqChangShift();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;

                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Index");
                }
                BSM.ListApproval = new List<ExDocApproval>();
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");

        }
        #endregion
        #region Details
        public ActionResult Details(string Inc)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = Inc;
            BSM.Header = new HREFReqChangShift();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            if (Inc == "null") Inc = null;
            if (Inc != null)
            {

                BSM.Header = unitOfWork.Set<HREFReqChangShift>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.Increment == Inc && w.DocType == DOCTYPE);
                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    BSM.ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == BSM.Header.Increment
                                       && w.DocumentType == BSM.Header.DocType).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Print
        public ActionResult Print(string Inc)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = Inc;
            UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string Inc)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            var SD = unitOfWork.Set<HREFReqChangShift>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.Increment == Inc && w.DocType == DOCTYPE);
            if (SD != null)
            {
                try
                {
                    var _Appro = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == Inc && w.DocumentType == SD.DocType && w.Status != "OPEN")
                            .OrderByDescending(x => x.ApproveLevel)
                            .ToList().FirstOrDefault();
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = Inc;
                    var sa = new RPTChaneShift();
                    var reportHelper = new clsReportHelper();
                    string path = reportHelper.Get_Path(SCREEN_ID);
                    if (!string.IsNullOrEmpty(path))
                    {
                        sa.LoadLayoutFromXml(path);
                    }
                    int level = 0;
                    if (_Appro != null) level = _Appro.ApproveLevel;
                    sa.Parameters["Increment"].Value = Inc;
                    sa.Parameters["Increment"].Visible = false;
                    sa.Parameters["APROLEVEL"].Value = level;
                    sa.Parameters["APROLEVEL"].Visible = false;

                    Session[Index_Sess_Obj + ActionName] = sa;
                    Session[Index_Sess_Obj] = sa;
                    return PartialView("PrintForm", sa);
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    //log.DocurmentAction = id;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string Inc)
        {
            ActionName = "Print";
            var SD = unitOfWork.Set<HREFReqChangShift>().AsQueryable().AsEnumerable().FirstOrDefault(w => w.Increment == Inc && w.DocType == DOCTYPE);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = Inc;
            if (SD != null)
            {
                RPTChaneShift reportModel = new RPTChaneShift();

                reportModel = (RPTChaneShift)Session[Index_Sess_Obj + ActionName];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }


        #endregion
        #region Request
        public ActionResult RequestForApproval(string Inc)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (Inc == "null") Inc = null;
            if (!string.IsNullOrEmpty(Inc))
            {
                BSM.ScreenId = SCREEN_ID;
                var Objmatch = unitOfWork.Set<HREFReqChangShift>().AsQueryable().FirstOrDefault(w => w.Increment == Inc && w.DocType == DOCTYPE);
                if (Objmatch != null)
                {

                    string fileName = Server.MapPath("~/Content/UPLOAD/" + Inc + "_" + Objmatch.EmpCode + ".pdf");
                    string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSEmpTermination/";
                    RPTChaneShift sa = new RPTChaneShift();
                    var reportHelper = new clsReportHelper();
                    string path = reportHelper.Get_Path(SCREEN_ID);
                    if (!string.IsNullOrEmpty(path))
                    {
                        sa.LoadLayoutFromXml(path);
                    }
                    sa.Parameters["Increment"].Value = Inc;
                    sa.Parameters["Increment"].Visible = false;
                    sa.Parameters["APROLEVEL"].Value = 0;
                    sa.Parameters["APROLEVEL"].Visible = false;
                    string UploadDirectory = Server.MapPath("~/Content/UPLOAD");
                    if (!Directory.Exists(UploadDirectory))
                    {
                        Directory.CreateDirectory(UploadDirectory);
                    }
                    sa.ExportToPdf(fileName);
                    string msg = BSM.RequestForApprove(Inc, fileName, URL);
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
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Index");

        }
        #endregion
        #region Approve
        public ActionResult Approve(string Inc)
        {

            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = Inc;
            string msg = "";
            string[] c = Inc.Split(';');
            foreach (var Document in c)
            {
                ViewData[SYSConstant.PARAM_ID] = Document;
                if (Document == "") continue;
                var Objmatch = unitOfWork.Set<HREFReqChangShift>().AsQueryable().FirstOrDefault(w => w.Increment == Document && w.DocType == DOCTYPE);
                if (!string.IsNullOrEmpty(Document) && Objmatch != null)
                {
                    string fileName = Server.MapPath("~/Content/UPLOAD/ " + Document + "_" + Objmatch.RequestDate.ToString("MMMM-yyyy") + ".pdf");
                    BSM.ScreenId = SCREEN_ID;
                    string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/MyTeamRequestChangShift/";
                    RPTChaneShift sa = new RPTChaneShift();
                    var reportHelper = new clsReportHelper();
                    string path = reportHelper.Get_Path(SCREEN_ID);
                    if (!string.IsNullOrEmpty(path))
                    {
                        sa.LoadLayoutFromXml(path);
                    }
                    var _Appro = unitOfWork.Set<ExDocApproval>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Document && w.DocumentType == Objmatch.DocType
                                && w.Approver == user.UserName);
                    int level = 0;
                    if (_Appro != null) level = _Appro.ApproveLevel;
                    sa.Parameters["Increment"].Value = Document;
                    sa.Parameters["Increment"].Visible = false;
                    sa.Parameters["APROLEVEL"].Value = level;
                    sa.Parameters["APROLEVEL"].Visible = false;
                    string UploadDirectory = Server.MapPath("~/Content/UPLOAD");
                    if (!Directory.Exists(UploadDirectory))
                    {
                        Directory.CreateDirectory(UploadDirectory);
                    }
                    sa.ExportToPdf(fileName);
                    msg = BSM.approveTheDoc(Document, fileName, URL);

                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
                }
            }
            if (msg == SYConstant.OK)
            {
                SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                messageObject.Description = messageObject.Description + BSM.MessageError;
                messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Index";
                SYMessages mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);


        }
        #endregion
        #region Reject
        public ActionResult Reject(string Inc)
        {
            this.UserSession();
            UserConfForm(SYActionBehavior.EDIT);

            if (!string.IsNullOrEmpty(Inc))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Reject(Inc);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = Inc;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?Inc=" + Inc;
                    SYMessages mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Upload
        [HttpPost]
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = unitOfWork.Set<CFUploadPath>().AsQueryable().FirstOrDefault(w => w.PathCode == "IMG_UPLOAD");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);

            objFile.ObjectTemplate = new MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREEN_ID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;

            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("UploadControl", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            Session[PATH_FILE] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }
        #endregion
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRequestChangShift)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListApproval == null)
                {
                    BSM.ListApproval = new List<ExDocApproval>();
                }
            }
            DataSelector();
            return PartialView("GridApproval", BSM.ListApproval);
        }
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {
            ActionName = "Details";
            var EmpStaff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().Where(w => w.EmpCode == EmpCode).FirstOrDefault();
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    POST = EmpStaff.Position,
                    DEPT = EmpStaff.Department,
                    SHIFT = "",
                    StartDate = EmpStaff.StartDate,
                    LevelCode = EmpStaff.Level,
                    SECT = EmpStaff.Section,
                };
                GetData(EmpStaff, "Create");
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public string GetData(HR_STAFF_VIEW Staff, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();
            if (Staff != null)
            {
                BSM.SetAutoApproval(SCREEN_ID, DOCTYPE, Staff.BranchID, Staff.DEPT);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_Employee");
            }
        }
        private void DataSelector()
        {
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListLevel = SYConstant.getLevelDataAccess();
            var staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == user.UserName);
            var ListStaff = new List<HR_STAFF_VIEW>();
            if (staff != null)
            {
                var _listEmp = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().AsEnumerable()
                        .Where(w => ListBranch.Any(x => w.BranchID == x.Code) && w.StatusCode == SYDocumentStatus.A.ToString()).ToList();
                _listEmp = _listEmp.Where(w => ListLevel.Any(x => w.LevelCode == x.Code)).ToList();
                ListStaff = _listEmp.ToList();
            }
            ViewData["STAFF_SELECT"] = ListStaff.ToList();
            ViewData["SECTIONS_SELECT"] = unitOfWork.Set<HRSection>().AsQueryable().ToList();
            ViewData["POSITION_SELECT"] = unitOfWork.Set<HRPosition>().AsQueryable().ToList();
            ViewData["DEPARTMENT_SELECT"] = unitOfWork.Set<HRDepartment>().AsQueryable().ToList();
        }
    }
}