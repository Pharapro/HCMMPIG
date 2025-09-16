using DevExpress.Web.Mvc;
using DevExpress.Xpo;
using DevExpress.XtraPrinting;
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
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static Humica.EForm.ClsHREmpBenefit;

namespace Humica.Controllers.SelfService.MyTeam
{
    public class MyTeamReqBenefitController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESS0000030";
        private const string URL_SCREEN = "/SelfService/MyTeam/MyTeamReqBenefit/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "DocNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string DOCTYPE = "REBEN";
        private string _LOCATION_ = "_LOCATION_";
        IClsHREmpBenefit BSM;
        IUnitOfWork unitOfWork;
        public MyTeamReqBenefitController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsHREmpBenefit();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.ListHeader = new List<HREFBenefit>();
            BSM.ListRequestPending = new List<ClsRequestBenefit>();
            var userName = user.UserName;
            BSM.ProcessSupense(userName);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.ListHeader = new List<HREFBenefit>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHREmpBenefit)Session[Index_Sess_Obj + ActionName];
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
                BSM = (ClsHREmpBenefit)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListRequestPending);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            var Staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == user.UserName);
            if (user.UserName != null && Staff != null)
            {
                BSM.HeaderStaff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(x => x.EmpCode == user.UserName);

                BSM.Header = new HREFBenefit();
                BSM.ListHeader = new List<HREFBenefit>();
                BSM.Header.EmpCode = user.UserName;
                BSM.Header.FromDate = DateTime.Now;
                BSM.Header.ToDate = DateTime.Now;
                BSM.Header.Status = "OPEN";
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
        public ActionResult Create(ClsHREmpBenefit collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.ADD);
            DataSelector();
            BSM.ListApproval = new List<ExDocApproval>();
            if (!string.IsNullOrEmpty(collection.Header.EmpCode))
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var obj = (ClsHREmpBenefit)Session[Index_Sess_Obj + ActionName];
                    BSM.ListApproval = obj.ListApproval;
                    if (Session[PATH_FILE] != null)
                    {
                        collection.Header.AttachFile = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHREmpBenefit)Session[Index_Sess_Obj + ActionName];
                    }
                }

                BSM.Header = collection.Header;
                BSM.HeaderStaff = collection.HeaderStaff;
                BSM.ScreenId = SCREEN_ID;
                BSM.DocType = DOCTYPE;
                string msg = BSM.CreateSuspen();

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
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region "Details"
        public ActionResult Details(string Doc)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = Doc;
            BSM.Header = new HREFBenefit();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            if (Doc == "null") Doc = null;
            if (Doc != null)
            {

                BSM.Header = unitOfWork.Set<HREFBenefit>().AsQueryable().FirstOrDefault(w => w.DocNo == Doc);
                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    BSM.ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == BSM.Header.DocNo
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
        #region "Print"
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
            var SD = unitOfWork.Set<HREFBenefit>().AsQueryable().FirstOrDefault(w => w.DocNo == Doc);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = Doc;
                    var sa = new FRMBenefit();
                    var reportHelper = new clsReportHelper();
                    string path = reportHelper.Get_Path(SCREEN_ID);
                    if (!string.IsNullOrEmpty(path))
                    {
                        sa.LoadLayoutFromXml(path);
                    }
                    var Approver = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == Doc && w.DocumentType == SD.DocType && w.Status != "OPEN")
                            .OrderByDescending(x => x.ApproveLevel)
                            .ToList().FirstOrDefault();
                    int level = 0;
                    if (Approver != null) level = Approver.ApproveLevel;
                    sa.Parameters["DocNo"].Value = Doc;
                    sa.Parameters["DocNo"].Visible = false;
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
                    log.DocurmentAction = Doc;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string Doc, string exportFormat = "pdf")
        {
            ActionName = "Print";
            var SD = unitOfWork.Set<HREFBenefit>().AsQueryable().FirstOrDefault(w => w.DocNo == Doc);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = Doc;
            if (SD != null)
            {
                FRMBenefit reportModel = (FRMBenefit)Session[Index_Sess_Obj + ActionName];
                if (reportModel != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        exportFormat = exportFormat.ToLower();
                        switch (exportFormat)
                        {
                            case "pdf":
                                var pdfOptions = new PdfExportOptions
                                {
                                    NeverEmbeddedFonts = "",
                                    PdfACompatibility = PdfACompatibility.None
                                };
                                reportModel.ExportToPdf(ms, pdfOptions);
                                return File(ms.ToArray(), "application/pdf", $"Report_{Doc}.pdf");

                            case "xls":
                                var xlsOptions = new XlsExportOptions { ExportMode = XlsExportMode.SingleFile };
                                reportModel.ExportToXls(ms, xlsOptions);
                                return File(ms.ToArray(), "application/vnd.ms-excel", $"Report_{Doc}.xls");

                            case "xlsx":
                                var xlsxOptions = new XlsxExportOptions { ExportMode = (XlsxExportMode)XlsExportMode.SingleFile };
                                reportModel.ExportToXlsx(ms, xlsxOptions);
                                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Report_{Doc}.xlsx");

                            case "rtf":
                                var rtfOptions = new RtfExportOptions();
                                reportModel.ExportToRtf(ms, rtfOptions);
                                return File(ms.ToArray(), "application/rtf", $"Report_{Doc}.rtf");

                            case "docx":
                                var docxOptions = new DocxExportOptions();
                                reportModel.ExportToDocx(ms, docxOptions);
                                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"Report_{Doc}.docx");

                            case "mht":
                                var mhtOptions = new MhtExportOptions { ExportMode = HtmlExportMode.SingleFile };
                                reportModel.ExportToMht(ms, mhtOptions);
                                return File(ms.ToArray(), "message/rfc822", $"Report_{Doc}.mht");

                            case "html":
                                var htmlOptions = new HtmlExportOptions { ExportMode = HtmlExportMode.SingleFile };
                                reportModel.ExportToHtml(ms, htmlOptions);
                                return File(ms.ToArray(), "text/html", $"Report_{Doc}.html");

                            case "text":
                                var textOptions = new TextExportOptions();
                                reportModel.ExportToText(ms, textOptions);
                                return File(ms.ToArray(), "text/plain", $"Report_{Doc}.txt");

                            case "csv":
                                var csvOptions = new CsvExportOptions();
                                reportModel.ExportToCsv(ms, csvOptions);
                                return File(ms.ToArray(), "text/csv", $"Report_{Doc}.csv");

                            case "image":
                                var imgOptions = new ImageExportOptions { Format = ImageFormat.Png };
                                reportModel.ExportToImage(ms, imgOptions);
                                return File(ms.ToArray(), "image/png", $"Report_{Doc}.png");

                            default:
                                return new HttpNotFoundResult("Unsupported export format");
                        }
                    }

                }
            }
            return null;
        }
        #endregion
        public ActionResult RequestForApproval(string DocNo)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
            {
                BSM.ScreenId = SCREEN_ID;
                var Objmatch = unitOfWork.Set<HREFBenefit>().AsQueryable().FirstOrDefault(w => w.DocNo == DocNo);

                string fileName = Server.MapPath("~/Content/UPLOAD/" + DocNo + "_" + Objmatch.FromDate.ToString("MMMM-yyyy") + ".pdf");
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/MyTeamReqSupense/";
                FRMBenefit sa = new FRMBenefit();
                var reportHelper = new clsReportHelper();
                string path = reportHelper.Get_Path(SCREEN_ID);
                if (!string.IsNullOrEmpty(path))
                {
                    sa.LoadLayoutFromXml(path);
                }
                sa.Parameters["DocNo"].Value = DocNo;
                sa.Parameters["DocNo"].Visible = false;
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
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
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
        #region "Approve"
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
                var Objmatch = unitOfWork.Set<HREFBenefit>().AsQueryable().FirstOrDefault(w => w.DocNo == Document);
                if (!string.IsNullOrEmpty(Document) && Objmatch != null)
                {
                    string fileName = Server.MapPath("~/Content/UPLOAD/ " + Document + "_" + Objmatch.FromDate.ToString("MMMM-yyyy") + ".pdf");
                    BSM.ScreenId = SCREEN_ID;
                    string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/MyTeamReqSupense/";
                    FRMBenefit sa = new FRMBenefit();
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
                    sa.Parameters["DocNo"].Value = Document;
                    sa.Parameters["DocNo"].Visible = false;
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
        #region "Reject"
        public ActionResult Reject(string Doc)
        {
            this.UserSession();
            UserConfForm(SYActionBehavior.EDIT);

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
                BSM = (ClsHREmpBenefit)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListApproval == null)
                {
                    BSM.ListApproval = new List<ExDocApproval>();
                }
            }
            DataSelector();
            return PartialView("GridApproval", BSM.ListApproval);
        }
        public ActionResult GridItemViewDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHREmpBenefit)Session[Index_Sess_Obj + ActionName];
            }

            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemViewDetails";
            return PartialView("GridItemViewDetails", BSM);
        }
        public ActionResult GridApprovalDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHREmpBenefit)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        public ActionResult ShowDataEmp(string EmpCode)
        {

            ActionName = "Details";
            var EmpStaff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmpType,
                    Division = EmpStaff.DivisionDesc,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.LevelCode,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate
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
        public ActionResult Refreshvalue(string Increase, string EmpCode,string BenefitType)
        {
            ActionName = "Create";
            var Reward = unitOfWork.Set<PR_RewardsType>().AsQueryable().FirstOrDefault(w => w.Code == BenefitType);
            decimal oldamount = 0;
            decimal NewSalary = 0;
            if (Reward != null)
            {
                if (Reward.ReCode == "ALLW")
                {
                    var allw = unitOfWork.Set<PREmpAllw>().AsQueryable().Where(w => w.EmpCode == EmpCode && w.AllwCode == BenefitType).OrderByDescending(w => w.ToDate).FirstOrDefault();
                    if (allw != null) oldamount = allw.Amount ?? 0;
                }
                else if (Reward.ReCode == "BON")
                {
                    var Bonus = unitOfWork.Set<PREmpBonu>().AsQueryable().Where(w => w.EmpCode == EmpCode && w.BonCode == BenefitType).OrderByDescending(w => w.ToDate).FirstOrDefault();
                    if (Bonus != null) oldamount = Convert.ToDecimal(Bonus.Amount);
                }
                decimal Inc = 0;
                if (Increase == "null" || string.IsNullOrEmpty(Increase)) Inc = 0;
                else
                {
                    Inc = Convert.ToDecimal(Increase);
                    NewSalary = oldamount + Inc;
                }
                var result = new
                {
                    MS = SYConstant.OK,
                    oldAm = oldamount,
                    NewAm = NewSalary
                };

                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = "INVALIDED_BENEFITTYPE" };
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
            ViewData["BRANCH_SELECT"] = ListBranch;
            ViewData["SECTIONS_SELECT"] = unitOfWork.Set<HRSection>().AsQueryable().ToList();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData["POSITION_SELECT"] = unitOfWork.Set<HRPosition>().AsQueryable().ToList();
            ViewData["DEPARTMENT_SELECT"] = unitOfWork.Set<HRDepartment>().AsQueryable().ToList();
            ViewData["LOCATION_SELECT"] = unitOfWork.Set<HRLocation>().AsQueryable().ToList().OrderBy(w => w.Description);
            ViewData["BENEFIT_SELECT"] = unitOfWork.Set<PR_RewardsType>().AsQueryable().Where(w => w.ReCode != "DED").ToList().OrderBy(w => w.ReCode).ThenBy(w => w.Code);
        }
    }
}