using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.EForm;
using Humica.Models.SY;
using HUMICA.Models.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.SelfService.MyTeam
{
    public class ESSRequestProbationController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESSEF00002";
        private const string URL_SCREEN = "/SelfService/MyTeam/ESSRequestProbation/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string DOCTYPE = "PRO";
        IClsESSRequestProbation BSM;
        IUnitOfWork unitOfWork;
        public ESSRequestProbationController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsESSRequestProbation();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        #region List
        public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.ListHeader = new List<HREFReqProbation>();
            BSM.ListPending = new List<ClsEmpProbation>();
            var userName = user.UserName;
            BSM.ProcessProbation(userName);

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);

            BSM.ListHeader = new List<HREFReqProbation>();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsESSRequestProbation)Session[Index_Sess_Obj + ActionName];
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
                BSM = (ClsESSRequestProbation)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListPending);
        }
        #endregion
        #region Create
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            string cancel = SYDocumentStatus.CANCELLED.ToString();
            BSM.Header = new HREFReqProbation();
            BSM.ListHeader = new List<HREFReqProbation>();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.Header.EmpCode = user.UserName;
            BSM.Header.Status = "OPEN";
            BSM.DocType = DOCTYPE;
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            HRStaffProfile Staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().Where(w => w.EmpCode == BSM.Header.EmpCode).FirstOrDefault();
            if (Staff != null)
                BSM.SetAutoApproval(SCREEN_ID, BSM.DocType, Staff.Branch, Staff.DEPT);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsESSRequestProbation collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.ListApproval = new List<ExDocApproval>();
            if (!string.IsNullOrEmpty(collection.HeaderStaff.EmpCode))
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var obj = (ClsESSRequestProbation)Session[Index_Sess_Obj + ActionName];
                   
                    BSM.ListApproval = obj.ListApproval;
                }
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.AttachFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                BSM.Header = collection.Header;
                BSM.HeaderStaff = collection.HeaderStaff;
                BSM.ScreenId = SCREEN_ID;
                BSM.DocType = DOCTYPE;
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSRequestProbation/Details/";
                string msg = BSM.CreateReqPro(URL);

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;

                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Index");
                }
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(BSM);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;

            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }

        #endregion
        #region Edit
        public ActionResult Edit(string DocNo)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ViewData[SYSConstant.PARAM_ID] = DocNo;
            if (DocNo == "null") DocNo = null;
            if (DocNo != null)
            {
                BSM.Header = new HREFReqProbation();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                BSM.Header = unitOfWork.Set<HREFReqProbation>().AsQueryable().FirstOrDefault(w => w.DocumentNo == DocNo);
                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    BSM.ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == BSM.Header.DocumentNo && w.DocumentType == BSM.Header.DocType).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }

            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string DocNo, ClsESSRequestProbation collection)
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
                    BSM = (ClsESSRequestProbation)Session[Index_Sess_Obj + ActionName];
                }
                //if (Session[PATH_FILE] != null)
                //{
                //    collection.Header.AttachPath = Session[PATH_FILE].ToString();
                //    Session[PATH_FILE] = null;
                //}
                //else
                //{
                //    collection.Header.AttachPath = BSM.Header.AttachPath;
                //}
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditReqPro(DocNo);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = DocNo;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?DocNo=" + mess.DocumentNumber;
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
        #region "Delete"
        public ActionResult Delete(string DocNo)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (DocNo == "null") DocNo = null;
            if (DocNo != null)
            {
                string msg = BSM.DeleteReqPro(DocNo);
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
        #region "Status"
        public ActionResult RequestForApproval(string DocNo)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (DocNo != null)
            {
                BSM.ScreenId = SCREEN_ID;
                var Objmatch = unitOfWork.Set<HREFReqProbation>().AsQueryable().FirstOrDefault(w => w.DocumentNo == DocNo);

                string fileName = Server.MapPath("~/Content/UPLOAD/" + DocNo + "_" + Objmatch.EmpCode + ".pdf");
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSRequestProbation/";
                RPTProbationEvaluation sa = new RPTProbationEvaluation();
                var reportHelper = new clsReportHelper();
                string path = reportHelper.Get_Path(SCREEN_ID);
                if (!string.IsNullOrEmpty(path))
                {
                    sa.LoadLayoutFromXml(path);
                }
                sa.Parameters["DocumentNo"].Value = DocNo;
                sa.Parameters["DocumentNo"].Visible = false;
                sa.Parameters["APROLEVEL"].Value = 0;
                sa.Parameters["APROLEVEL"].Visible = false;
                string UploadDirectory = Server.MapPath("~/Content/UPLOAD");
                if (!Directory.Exists(UploadDirectory))
                {
                    Directory.CreateDirectory(UploadDirectory);
                }
                sa.ExportToPdf(fileName);
                string msg = BSM.RequestForApprove(DocNo, fileName, URL);
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
        public ActionResult Cancel(string DocNo)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (DocNo != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Cancel(DocNo);
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
        public ActionResult Reject(string DocNo)
        {
            this.UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (DocNo != null)
            {
               
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSRequestProbation/Details/";
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Reject(DocNo);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = DocNo;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Index";
                    SYMessages mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Index");
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Index");

        }
        public ActionResult Approve(string DocNo)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            
            string msg = "";
            string[] c = DocNo.Split(';');
            foreach (var Document in c)
            {
                ViewData[SYSConstant.PARAM_ID] = Document;
                if (Document == "") continue;
                BSM.ScreenId = SCREEN_ID;
                var Objmatch = unitOfWork.Set<HREFReqProbation>().AsQueryable().FirstOrDefault(w => w.DocumentNo == Document);
                string fileName = Server.MapPath("~/Content/UPLOAD/" + Document + "_" + Objmatch.EmpCode + ".pdf");
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSRequestProbation/Details/";
                RPTProbationEvaluation sa = new RPTProbationEvaluation();
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
            if (msg == SYConstant.OK)
            {
                var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                mess.Description = mess.Description + ". " + BSM.MessageError;
                Session[SYSConstant.MESSAGE_SUBMIT] = mess;
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #endregion
        #region "Print"
        public ActionResult Print(string DocNo)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = DocNo;
            UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string DocNo)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            var SD = unitOfWork.Set<HREFReqProbation>().AsQueryable().FirstOrDefault(w => w.DocumentNo == DocNo);
            if (SD != null)
            {
                try
                {
                    var Approver = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == DocNo && w.DocumentType == SD.DocType && w.Status != "OPEN")
                           .OrderByDescending(x => x.ApproveLevel)
                           .ToList().FirstOrDefault();
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = DocNo;
                    var sa = new RPTProbationEvaluation();
                    var reportHelper = new clsReportHelper();
                    string path = reportHelper.Get_Path(SCREEN_ID);
                    if (!string.IsNullOrEmpty(path))
                    {
                        sa.LoadLayoutFromXml(path);
                    }
                    int Level = 0;
                    if (Approver != null) Level = Approver.ApproveLevel;
                    sa.Parameters["DocumentNo"].Value = DocNo;
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
                    log.DocurmentAction = DocNo;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string DocNo)
        {
            ActionName = "Print";
            var SD = unitOfWork.Set<HREFReqProbation>().AsQueryable().FirstOrDefault(w => w.DocumentNo == DocNo);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = DocNo;
            if (SD != null)
            {
                RPTProbationEvaluation reportModel = new RPTProbationEvaluation();

                reportModel = (RPTProbationEvaluation)Session[Index_Sess_Obj + ActionName];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }


        #endregion
        public ActionResult Details(string DocNo)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = DocNo;
            if (DocNo == "null") DocNo = null;
            if (DocNo != null)
            {
                BSM.ListApproval = new List<ExDocApproval>();
                BSM.Header = new HREFReqProbation();
                BSM.Header = unitOfWork.Set<HREFReqProbation>().AsQueryable().FirstOrDefault(w => w.DocumentNo == DocNo);
                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    BSM.ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == DocNo && w.DocumentType == BSM.Header.DocType).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsESSRequestProbation)Session[Index_Sess_Obj + ActionName];
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
                BSM = (ClsESSRequestProbation)Session[Index_Sess_Obj + ActionName];
            }

            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemViewDetails";
            return PartialView("GridItemViewDetails", BSM);
        }
        public ActionResult UploadControlCallbackActionImage(HttpPostedFileBase file_Uploader)
        {

            UserSession();
            var path = unitOfWork.Set<CFUploadPath>().AsQueryable().FirstOrDefault(w => w.PathCode == "IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "STAFF";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("UploadControl",
                sfi.ValidationSettings,
                sfi.uc_FileUploadCompleteFile);
            Session[PATH_FILE] = sfi.ObjectTemplate.UpoadPath;
            return null;
        }

        public ActionResult ShowDataEmp(string EmpCode)
        {
            HR_STAFF_VIEW EmpStaff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == EmpCode);
            var EmpSalary = unitOfWork.Set<HREmpBankAcc>().AsQueryable().Where(w => w.EmpCode == EmpCode && w.IsActive == true).ToList();
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
                    DEPTCode = EmpStaff.DEPT,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.LevelCode,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate,
                    Salary = EmpSalary.Sum(w => w.Salary),
                };
                GetData(EmpStaff, "Create");
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult GetData(HR_STAFF_VIEW EmpCode, string ActionName)
        {
            UserSession();
            BSM.DocType = DOCTYPE;
            BSM.SetAutoApproval(SCREEN_ID, BSM.DocType, EmpCode.BranchID, EmpCode.DEPT);
            Session[Index_Sess_Obj + ActionName] = BSM;
            var result = new
            {
                MS = SYConstant.OK,
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        public ActionResult Refreshvalue(string Increase, string EmpCode)
        {
            ActionName = "Create";
            var EmpSalary = unitOfWork.Set<HREmpBankAcc>().AsQueryable().Where(w => w.EmpCode == EmpCode && w.IsActive == true).ToList();
            if (Session[Index_Sess_Obj + ActionName] != null && EmpSalary != null)
            {
                decimal Inc = Convert.ToDecimal(Increase);
                BSM = (ClsESSRequestProbation)Session[Index_Sess_Obj + ActionName];
                if (BSM.Header == null)
                    BSM.Header = new HREFReqProbation();
                BSM.Header.Increase = Inc;
                BSM.Header.NewSalary = EmpSalary.Sum(w => w.Salary);
                if (BSM.Header.Increase != 0)
                    BSM.Header.NewSalary = BSM.Header.NewSalary + Inc;
                var result = new
                {
                    MS = SYConstant.OK,
                    NewSalary = BSM.Header.NewSalary
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
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
            ViewData["PROBATION_LIST"] = unitOfWork.Set<HREFProbationType>().AsQueryable().ToList();
            ViewData["BRANCH_SELECT"] = ListBranch;
            ViewData["SECTIONS_SELECT"] = unitOfWork.Set<HRSection>().AsQueryable().ToList();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData["POSITION_SELECT"] = unitOfWork.Set<HRPosition>().AsQueryable().ToList();
            ViewData["DEPARTMENT_SELECT"] = unitOfWork.Set<HRDepartment>().AsQueryable().ToList();
            ViewData["LOCATION_SELECT"] = unitOfWork.Set<HRLocation>().AsQueryable().ToList().OrderBy(w => w.Description);
        }
    }
}