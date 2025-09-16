using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.EForm;
using Humica.Models.Report.HRM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.SelfService.MyTeam
{
    public class ESSRequestWarningController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESS0000027";
        private const string URL_SCREEN = "/SelfService/MyTeam/ESSRequestWarning/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string DOCTYPE = "WAR";
        IClsDisciplinary BSM;
        IUnitOfWork unitOfWork;
        public ESSRequestWarningController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsDisciplinary();
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
            BSM.ListHeader = new List<HREmpDisciplinary>();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.ListRequestPending = new List<ClsEmpDisciplinary>();
            BSM.LoadData(userName);
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.ListHeader = new List<HREmpDisciplinary>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsDisciplinary)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.ListHeader);
        }

        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsDisciplinary)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListRequestPending);
        }

        #endregion
        #region Create
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            string Open = SYDocumentStatus.OPEN.ToString();
            BSM.Header = new HREmpDisciplinary();
            BSM.ListHeader = new List<HREmpDisciplinary>();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.Header.EmpCode = user.UserName;
            BSM.Header.Status = Open;
            BSM.DocType = DOCTYPE;
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            HRStaffProfile Staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().Where(w => w.EmpCode == BSM.Header.EmpCode).FirstOrDefault();
            if (Staff != null)
                BSM.SetAutoApproval(SCREEN_ID, BSM.DocType, Staff.Branch, Staff.DEPT);

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsDisciplinary collection)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ActionName = "Create";
            if (!string.IsNullOrEmpty(collection.HeaderStaff.EmpCode))
            {
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.AttachPath = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {

                    var obj = (ClsDisciplinary)Session[Index_Sess_Obj + ActionName];
                    BSM.ListApproval = obj.ListApproval;
                }

                BSM.Header = collection.Header;
                BSM.HeaderStaff = collection.HeaderStaff;
                BSM.ScreenId = SCREEN_ID;
                BSM.DocType = DOCTYPE;
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSRequestWarning/Details/";
                string msg = BSM.CreateEmpDiscp(URL, true);

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
                BSM.ListHeader = new List<HREmpDisciplinary>();
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(BSM);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;

            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region Delete
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id == "null") id = null;
            if (id != null)
            {
                int TranNo = Convert.ToInt32(id);
                string msg = BSM.DeleteEmpDiscp(TranNo);
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
        #region Upload
        [HttpPost]
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
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
        #region Print
        public ActionResult Print(string Doc)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = Doc;
            UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string Doc)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            var SD = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Doc);
            if (SD != null)
            {
                try
                {
                    var Approver = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == Doc && w.DocumentType == SD.DocType && w.Status != "OPEN")
                           .OrderByDescending(x => x.ApproveLevel)
                           .ToList().FirstOrDefault();
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = Doc;
                    var sa = new rptDisciplineLetter();
                    var reportHelper = new clsReportHelper();
                    string path = reportHelper.Get_Path(SCREEN_ID);
                    if (!string.IsNullOrEmpty(path))
                    {
                        sa.LoadLayoutFromXml(path);
                    }
                    int Level = 0;
                    if (Approver != null) Level = Approver.ApproveLevel;
                    sa.Parameters["DocumentNo"].Value = Doc;
                    sa.Parameters["DocumentNo"].Visible = false;
                    sa.Parameters["APROLEVEL"].Value = Level;
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
                    log.DocurmentAction = Doc;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string Doc)
        {
            ActionName = "Print";
            var SD = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Doc);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = Doc;
            if (SD != null)
            {
                rptDisciplineLetter reportModel = new rptDisciplineLetter();

                reportModel = (rptDisciplineLetter)Session[Index_Sess_Obj + ActionName];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }


        #endregion
        #region Status
        public ActionResult RequestForApproval(string Doc)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (Doc == "null") Doc = null;
            if (!string.IsNullOrEmpty(Doc))
            {
                BSM.ScreenId = SCREEN_ID;
                var Objmatch = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Doc);

                string fileName = Server.MapPath("~/Content/UPLOAD/" + Doc + "_" + Objmatch.EmpCode + ".pdf");
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSRequestWarning/";
                rptDisciplineLetter sa = new rptDisciplineLetter();
                var reportHelper = new clsReportHelper();
                string path = reportHelper.Get_Path(SCREEN_ID);
                if (!string.IsNullOrEmpty(path))
                {
                    sa.LoadLayoutFromXml(path);
                }
                sa.Parameters["DocumentNo"].Value = Doc;
                sa.Parameters["DocumentNo"].Visible = false;
                sa.Parameters["APROLEVEL"].Value = 0;
                sa.Parameters["APROLEVEL"].Visible = false;
                string UploadDirectory = Server.MapPath("~/Content/UPLOAD");
                if (!Directory.Exists(UploadDirectory))
                {
                    Directory.CreateDirectory(UploadDirectory);
                }
                sa.ExportToPdf(fileName);
                string msg = BSM.RequestForApprove(Doc, fileName, URL);
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
        public ActionResult Approve(string Doc)
        {

            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = Doc;
            string msg = "";
            string[] c = Doc.Split(';');
            foreach (var Document in c)
            {
                ViewData[SYSConstant.PARAM_ID] = Document;
                if (Document == "") continue;
                var Objmatch = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Document);
                if (!string.IsNullOrEmpty(Document) && Objmatch != null)
                {
                    string fileName = Server.MapPath("~/Content/UPLOAD/ " + Document + "_" + Objmatch.TranDate.ToString("MMMM-yyyy") + ".pdf");
                    BSM.ScreenId = SCREEN_ID;
                    string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSRequestWarning/";
                    rptDisciplineLetter sa = new rptDisciplineLetter();
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
                    sa.Parameters["DocumentNo"].Value = Document;
                    sa.Parameters["DocumentNo"].Visible = false;
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
        public ActionResult Reject(string Doc)
        {
            this.UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (Doc == "null") Doc = null;
            if (!string.IsNullOrEmpty(Doc))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Reject(Doc);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = Doc;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?Doc=" + Doc;
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
        public ActionResult Cancel(string Doc)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (Doc == "null") Doc = null;
            if (!string.IsNullOrEmpty(Doc))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Cancel(Doc);
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
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsDisciplinary)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListApproval == null)
                {
                    BSM.ListApproval = new List<ExDocApproval>();
                }
            }
            DataSelector();
            return PartialView("GridApproval", BSM.ListApproval);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridItemViewDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsDisciplinary)Session[Index_Sess_Obj + ActionName];
            }

            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemViewDetails";
            return PartialView("GridItemViewDetails", BSM);
        }
        public ActionResult Details(string Doc)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = Doc;
            if (Doc == "null") Doc = null;
            if (Doc != null)
            {
                BSM.ListApproval = new List<ExDocApproval>();
                BSM.ListHeader = new List<HREmpDisciplinary>();
                BSM.Header = new HREmpDisciplinary();
                BSM.Header = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Doc);
                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    BSM.ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == BSM.Header.DocumentNo && w.DocumentType == BSM.Header.DocType).ToList();
                    BSM.ListHeader = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().Where(w => w.EmpCode == BSM.Header.EmpCode).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult ShowDataEmp(string id, string EmpCode)
        {
            var EmpStaff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmpType,
                    Division = EmpStaff.Division,
                    Branch = EmpStaff.Branch,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.LevelCode,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate
                };
                GetData(EmpStaff, EmpCode, "Create");
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public string GetData(HR_STAFF_VIEW Staff, string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            BSM.ListHeader = new List<HREmpDisciplinary>();
            List<HREmpDisciplinary> HREmpDisciplinary = unitOfWork.Set<HREmpDisciplinary>().AsQueryable().Where(x => x.EmpCode == EmpCode).ToList();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj_ = (ClsDisciplinary)Session[Index_Sess_Obj + ActionName];
                BSM.ListApproval = obj_.ListApproval;
                foreach (var read in HREmpDisciplinary.OrderByDescending(w => w.TranDate).ToList())
                {
                    var obj = new HREmpDisciplinary();
                    obj.TranDate = read.TranDate;
                    obj.DiscType = read.DiscType;
                    obj.Remark = read.Remark;
                    obj.Reference = read.Reference;
                    obj.Consequence = read.Consequence;

                    BSM.ListHeader.Add(obj);
                }
                BSM.SetAutoApproval(SCREEN_ID, DOCTYPE, Staff.BranchID, Staff.DEPT);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_Employee");
            }
        }
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsDisciplinary)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListHeader == null)
                {
                    BSM.ListHeader = new List<HREmpDisciplinary>();
                }
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListHeader);
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
            ViewData["BRANCH_SELECT"] = ListBranch;
            ViewData["SECTIONS_SELECT"] = unitOfWork.Set<HRSection>().AsQueryable().ToList();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData["POSITION_SELECT"] = unitOfWork.Set<HRPosition>().AsQueryable().ToList();
            ViewData["DEPARTMENT_SELECT"] = unitOfWork.Set<HRDepartment>().AsQueryable().ToList();
            ViewData["LOCATION_SELECT"] = unitOfWork.Set<HRLocation>().AsQueryable().ToList().OrderBy(w => w.Description);
            ViewData["DISCIPLINAY_LIST"] = unitOfWork.Set<HRDisciplinType>().AsQueryable().ToList();
            ViewData["DISCIPLINACTION_SELECT"] = unitOfWork.Set<HRDisciplinAction>().AsQueryable().ToList();
        }
    }
}