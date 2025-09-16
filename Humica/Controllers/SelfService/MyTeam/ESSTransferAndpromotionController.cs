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
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.SelfService.MyTeam
{
    public class ESSTransferAndpromotionController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESSEF00001";
        private const string URL_SCREEN = "/SelfService/MyTeam/ESSTransferAndpromotion/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "DocNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string DOCTYPE = "TRAN";
        IClsRequestTransferStaff BSM;
        IUnitOfWork unitOfWork;
        public ESSTransferAndpromotionController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsRequestTransferStaff();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        #region List
        public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            var userName = user.UserName;
            BSM.ListPending = new List<ClsEmpTransfer>();
            BSM.ListHeader = new List<HREFRequestTransferStaff>();
            BSM.ProcessTransfers(userName);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.ListHeader = new List<HREFRequestTransferStaff>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];
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
                BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListPending);
        }
        #endregion
        #region Create
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.ListHeader = new List<HREFRequestTransferStaff>();
            BSM.ListRCMAEdu = new List<RCMAEdu>();
            BSM.ListRCMAWorkHistory = new List<RCMAWorkHistory>();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.ListHisCareer = new List<HR_HisCareer>();
            BSM.Header = new HREFRequestTransferStaff();
            BSM.Header.EmpCode = user.UserName;
			BSM.Header.Status = "OPEN";
			BSM.Header.CreatedOn = DateTime.Now;
            BSM.Header.EffectDate = DateTime.Now;
            BSM.DocType = DOCTYPE;
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            HRStaffProfile Staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().Where(w => w.EmpCode == BSM.Header.EmpCode).FirstOrDefault();
            if (Staff != null)
                BSM.SetAutoApproval(SCREEN_ID, BSM.DocType,Staff.Branch, Staff.DEPT);
			Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsRequestTransferStaff collection)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ActionName = "Create";
			if (!string.IsNullOrEmpty(collection.HeaderStaff.EmpCode))
			{
				if (Session[PATH_FILE] != null)
                {
                    collection.Header.AttachFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.HeaderStaff = collection.HeaderStaff;
                BSM.NewSalary = collection.NewSalary;
                BSM.Increase = collection.Increase;
                BSM.ScreenId = SCREEN_ID;
                BSM.DocType = DOCTYPE;
                BSM.Header.Status = "OPEN";
                string msg = BSM.CreateTransferStaff();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                    mess_err.DocumentNumber = BSM.Header.DocNo.ToString();
                    mess_err.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?DocNo=" + mess_err.DocumentNumber;
                    BSM.Header = new HREFRequestTransferStaff();
                    BSM.HeaderStaff = new HR_STAFF_VIEW();
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(BSM);
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
        public ActionResult Edit(string ID)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ViewData[SYSConstant.PARAM_ID] = ID;
            if (ID == "null") ID = null;
            if (ID != null)
            {
                BSM.Header = new HREFRequestTransferStaff();
                BSM.ListRCMAEdu = new List<RCMAEdu>();
                BSM.ListRCMAWorkHistory = new List<RCMAWorkHistory>();
                BSM.ListApproval = new List<ExDocApproval>();
                BSM.ListHisCareer = new List<HR_HisCareer>();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                BSM.Header = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable().FirstOrDefault(w => w.DocNo == ID);
                if (BSM.Header != null)
                {
                    //add Grid to edit 
                    BSM.ListRCMAEdu = unitOfWork.Set<RCMAEdu>().AsQueryable().Where(w => w.ApplicantID == BSM.Header.DocNo).ToList();
                    BSM.ListRCMAWorkHistory = unitOfWork.Set<RCMAWorkHistory>().AsQueryable().Where(w => w.ApplicantID == BSM.Header.DocNo).ToList();

                    BSM.HeaderStaff = unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    BSM.ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable().Where(w => w.DocumentNo == BSM.Header.DocNo && w.DocumentType == BSM.Header.DocType).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, ClsRequestTransferStaff collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];
                }
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.AttachFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                else
                {
                    collection.Header.AttachFile = BSM.Header.AttachFile;
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                //int TranNo = Convert.ToInt32(id);
                string msg = BSM.EditEmp(id);
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
        #region Detail
        public ActionResult Details(string DocNo)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ViewData[SYSConstant.PARAM_ID] = DocNo;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (DocNo == "null") DocNo = null;
            if (DocNo != null)
            {
                BSM.ListApproval = new List<ExDocApproval>();
                BSM.ListRCMAEdu = new List<RCMAEdu>();
                BSM.ListRCMAWorkHistory = new List<RCMAWorkHistory>();
                BSM.ListApproval = new List<ExDocApproval>();
                BSM.ListHisCareer = new List<HR_HisCareer>();
                BSM.Header = new HREFRequestTransferStaff();
                BSM.Header = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable().FirstOrDefault(w => w.DocNo == DocNo);
                if (BSM.Header != null)
                {
                    BSM.ListRCMAEdu = unitOfWork.Set<RCMAEdu>().AsQueryable().Where(w => w.ApplicantID == BSM.Header.DocNo).ToList();
                    BSM.ListRCMAWorkHistory = unitOfWork.Set<RCMAWorkHistory>().AsQueryable().Where(w => w.ApplicantID == BSM.Header.DocNo).ToList();
                    BSM.HeaderStaff =unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    BSM.ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable().Where(w => w.DocumentNo == BSM.Header.DocNo && w.DocumentType == BSM.Header.DocType).ToList();
                    var resualt = unitOfWork.Set<HREmpCareer>().AsQueryable();
                    List<HREmpCareer> listEmpCareer = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode)
                         .OrderBy(w => w.TranNo)
                                .ThenBy(w => w.CompanyCode)
                                .GroupBy(x => new { x.CompanyCode, x.JobCode })
                                .Select(g => g.FirstOrDefault())
                                .Where(item => item != null)
                                .ToList();
                    int tranNo = Convert.ToInt32(listEmpCareer.Max(w => w.TranNo));
                    var IsSalary = BSM.IsHideSalary(BSM.HeaderStaff.LevelCode);
                    
                    foreach (var read in listEmpCareer.OrderByDescending(w => w.FromDate).ToList())
                    {
                        var EmpCar = unitOfWork.Set<HREmpCareer>().AsQueryable()
                        .Where(w => w.EmpCode == read.EmpCode &&
                                    w.CompanyCode == read.CompanyCode &&
                                    w.JobCode == read.JobCode).OrderByDescending(w => w.ToDate).FirstOrDefault();
                        var obj = new HR_HisCareer();
                        obj.FromDate = read.FromDate.Value;
                        obj.ToDate = EmpCar.ToDate.Value;
                        obj.Department = read.DEPT;
                        obj.Company = read.CompanyCode;
                        obj.Position = read.JobCode;
                        obj.Career = read.CareerCode;
                        obj.Level = read.LevelCode;
                        if (IsSalary == true)
                        {
                            obj.NewSalary = read.NewSalary.ToString();
                            obj.OldSalary = read.OldSalary.ToString();
                            obj.Increase = read.Increase.ToString();
                        }
                        else
                        {
                            obj.NewSalary = "#####";
                            obj.OldSalary = "#####";
                            obj.Increase = "#####";
                        }
                        obj.CreatedBy = read.CreateBy;
                        obj.ChangedBy = read.ChangedBy;
                        BSM.ListHisCareer.Add(obj);
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Delete
        public ActionResult Delete(string DocNo)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
            {
                string msg = BSM.DeleteEmp(DocNo);
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
        #region Status
        public ActionResult RequestForApproval(string DocNo)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
            {
                BSM.ScreenId = SCREEN_ID;
                //var Don = Convert.ToString(id);
                var Objmatch = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable().FirstOrDefault(w => w.DocNo == DocNo);

                string fileName = Server.MapPath("~/Content/UPLOAD/" + DocNo + "_" + Objmatch.EmpCode + ".pdf");
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSTransferAndpromotion/";
                RptTransferAndPromote sa = new RptTransferAndPromote();
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
                var Objmatch = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable().FirstOrDefault(w => w.DocNo == Document);
                if (!string.IsNullOrEmpty(Document) && Objmatch != null)
                {
                    BSM.ScreenId = SCREEN_ID;
                    string fileName = Server.MapPath("~/Content/UPLOAD/" + Document + "_" + Objmatch.EmpCode + ".pdf");
                    string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSTransferAndpromotion/Details/";
                    RptTransferAndPromote sa = new RptTransferAndPromote();
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
                    msg = BSM.Approve(DocNo, fileName, URL);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
                }
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
        public ActionResult Reject(string DocNo)
        {
            this.UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
            {
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
        #endregion
        #region Print
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
            var SD = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable().FirstOrDefault(w => w.DocNo == DocNo);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = DocNo;
                    RptTransferAndPromote reportModel = new RptTransferAndPromote();
                    var reportHelper = new clsReportHelper();
                    string path = reportHelper.Get_Path(SCREEN_ID);
                    if (!string.IsNullOrEmpty(path))
                    {
                        reportModel.LoadLayoutFromXml(path);
                    }
                    var Approver = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == DocNo && w.DocumentType == SD.DocType && w.Status != "OPEN")
                            .OrderByDescending(x => x.ApproveLevel)
                            .ToList().FirstOrDefault();
                    int level = 0;
                    if (Approver != null) level = Approver.ApproveLevel;
                    reportModel.Parameters["DocNo"].Value = SD.DocNo;
                    reportModel.Parameters["DocNo"].Visible = false;
                    reportModel.Parameters["APROLEVEL"].Value = level;
                    reportModel.Parameters["APROLEVEL"].Visible = false;

                    Session[Index_Sess_Obj + ActionName] = reportModel;

                    return PartialView("PrintForm", reportModel);
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
            var SD = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable().FirstOrDefault(w => w.DocNo == DocNo);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = DocNo;
            if (SD != null)
            {
                RptTransferAndPromote reportModel = new RptTransferAndPromote();

                reportModel = (RptTransferAndPromote)Session[Index_Sess_Obj + ActionName];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion
        #region GridItemsStudy
        public ActionResult GridItemsStudy()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemsStudy", BSM.ListRCMAEdu);
        }
        public ActionResult CreatePItem(RCMAEdu ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (!string.IsNullOrEmpty(ModelObject.EduType))
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];
                    }
                    if (!BSM.ListRCMAEdu.Where(w => w.LineItem == ModelObject.LineItem).Any())
                    {
                        BSM.ListRCMAEdu.Add(ModelObject);
                        ModelObject.LineItem = BSM.ListRCMAEdu.Max(w => w.LineItem) + 1;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPLICATED_ITEM");
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsStudy", BSM.ListRCMAEdu);
        }
        public ActionResult EditPItem(RCMAEdu ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListRCMAEdu.Where(w => w.LineItem == ModelObject.LineItem).ToList();

                if (objCheck.Count > 0)
                {
                    objCheck.First().EduType = ModelObject.EduType;
                    objCheck.First().EduCenter = ModelObject.EduCenter;
                    objCheck.First().Major = ModelObject.Major;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItemsStudy", BSM.ListRCMAEdu);
        }
        public ActionResult DeletePItem(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClsRequestTransferStaff BSM = new ClsRequestTransferStaff();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListRCMAEdu.Where(w => w.LineItem == LineItem).ToList();

                if (objCheck.Count > 0)
                {
                    BSM.ListRCMAEdu.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItemsStudy", BSM.ListRCMAEdu);
        }
        #endregion
        #region Carrer History
        public ActionResult GridHisCareer()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            DataSelector();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridHisCareer";
            return PartialView("GridHisCareer", BSM.ListHisCareer);
        }
        #endregion
        #region History Before
        public ActionResult GridHisBefore()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridHisBefore", BSM.ListRCMAWorkHistory);
        }
        public ActionResult CreateHisBefore(RCMAWorkHistory ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (!string.IsNullOrEmpty(ModelObject.Company))
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];
                    }
                    if (!BSM.ListRCMAWorkHistory.Where(w => w.LineItem == ModelObject.LineItem).Any())
                    {
                        BSM.ListRCMAWorkHistory.Add(ModelObject);
                        ModelObject.LineItem = BSM.ListRCMAWorkHistory.Max(w => w.LineItem) + 1;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPLICATED_ITEM");
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridHisBefore", BSM.ListRCMAWorkHistory);
        }
        public ActionResult EditHisBefore(RCMAWorkHistory ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListRCMAWorkHistory.Where(w => w.LineItem == ModelObject.LineItem).ToList();

                if (objCheck.Count > 0)
                {
                    objCheck.First().Company = ModelObject.Company;
                    objCheck.First().Position = ModelObject.Position;
                    objCheck.First().FromDate = ModelObject.FromDate;
                    objCheck.First().ToDate = ModelObject.ToDate;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridHisBefore", BSM.ListRCMAWorkHistory);
        }
        public ActionResult DeleteHisBefore(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListRCMAWorkHistory.Where(w => w.LineItem == LineItem).ToList();

                if (objCheck.Count > 0)
                {
                    BSM.ListRCMAWorkHistory.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridHisBefore", BSM.ListRCMAWorkHistory);
        }
        #endregion
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListApproval == null)
                {
                    BSM.ListApproval = new List<ExDocApproval>();
                }
            }
            return PartialView("GridApproval", BSM.ListApproval);
        }
        public ActionResult Refreshvalue(string id, string Increase,string EmpCode)
        {
            ActionName = "Create";
            var EmpStaff = unitOfWork.Set<HREmpBankAcc>().AsQueryable().Where(w => w.EmpCode == EmpCode && w.IsActive == true).ToList();

            if (Session[Index_Sess_Obj + ActionName] != null && EmpStaff.Any())
            {
                decimal Inc = Convert.ToDecimal(Increase);
                BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];
                BSM.Header.Increase = Inc;
                BSM.Header.NewSalary = EmpStaff.Sum(w => w.Salary) + Inc;
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
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {
            UserSession();
            ActionName = "Details";
            HR_STAFF_VIEW EmpStaff =unitOfWork.Set<HR_STAFF_VIEW>().AsQueryable().FirstOrDefault(w => w.EmpCode == EmpCode);
            var EmpSalary = unitOfWork.Set<HREmpBankAcc>().AsQueryable().Where(w => w.EmpCode == EmpCode && w.IsActive == true).ToList();
            List<HREFRequestTransferStaff> ListHeader = unitOfWork.Set<HREFRequestTransferStaff>().AsQueryable().Where(x => x.EmpCode == EmpCode).ToList();
            if (EmpStaff != null)
            {
                var EmpTransfer = ListHeader.FirstOrDefault(w => w.DocNo == ID);
                if (EmpTransfer != null)
                {
                    var Salary = "";
                    var IsSalary = BSM.IsHideSalary(EmpTransfer.Level);
                    if (IsSalary == true)
                    {
                        Salary = EmpStaff.Salary.ToString();
                    }
                    else
                    {
                        ViewData[ClsConstant.IS_SALARY] = true;
                        Salary = "#####";
                    }
                    JobType(EmpTransfer.Branch, "Branch");
                    var result = new
                    {
                        MS = SYConstant.OK,
                        AllName = EmpStaff.AllName,
                        EmpType = EmpStaff.EmployeeType,
                        Division = EmpStaff.Division,
                        DEPT = EmpStaff.Department,
                        Bracnh = EmpStaff.Branch,
                        SECT = EmpStaff.Section,
                        LevelCode = EmpStaff.Level,
                        Position = EmpStaff.Position,
                        StartDate = EmpStaff.StartDate,
                        Status = EmpStaff.Status,
                        Salary = EmpSalary.Sum(w => w.Salary),

                        CCompany = EmpTransfer.CompanyCode,
                        CBranch = EmpTransfer.Branch,
                        CDivi = EmpTransfer.Division,
                        CGDept = EmpTransfer.GroupDEPT,
                        CPosition = EmpTransfer.Position,
                        CSect = EmpTransfer.Section,
                        CLevel = EmpTransfer.Level,
                        CEff = DateTime.Now,
                        CSalary = Salary,
                    };
                    GetData(EmpCode, "Create");
                    return Json(result, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    var result = new
                    {
                        MS = SYConstant.OK,
                        AllName = EmpStaff.AllName,
                        EmpType = EmpStaff.EmployeeType,
                        Division = EmpStaff.Division,
                        Branch= EmpStaff.Branch,
                        DEPT = EmpStaff.Department,
                        SECT = EmpStaff.Section,
                        LevelCode = EmpStaff.Level,
                        Position = EmpStaff.Position,
                        StartDate = EmpStaff.StartDate,
                        Status= EmpStaff.Status,
                        Salary = EmpSalary.Sum(w => w.Salary),

                        CCompany = EmpStaff.CompanyCode,
                        CEmpType = EmpStaff.EmpType,
                        CBranch = EmpStaff.Branch,
                        CLocation = EmpStaff.LOCT,
                        CDivi = EmpStaff.Division,
						CPosition = EmpStaff.Position,
						CDept = EmpStaff.DEPT,
                        CSect = EmpStaff.Section,
                        CLevel = EmpStaff.Level,
                        CEff = DateTime.Now,
                    };
                    GetData(EmpCode, "Create");
                    return Json(result, JsonRequestBehavior.DenyGet);
                }
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public string GetData(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            BSM.ListHisCareer = new List<HR_HisCareer>();
            //BSM.ListCareerBankList = new List<HREmpCareerBankList>();
            var staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (Session[Index_Sess_Obj + ActionName] != null && staff != null)
            {
                BSM = (ClsRequestTransferStaff)Session[Index_Sess_Obj + ActionName];
                var resualt = unitOfWork.Set<HREmpCareer>().AsQueryable();
                List<HREmpCareer> listEmpCareer = resualt.Where(x => x.EmpCode == EmpCode)
                     .OrderBy(w => w.TranNo)
                            .ThenBy(w => w.CompanyCode)
                            .GroupBy(x => new { x.CompanyCode, x.JobCode })
                            .Select(g => g.FirstOrDefault())
                            .Where(item => item != null)
                            .ToList();
                int tranNo = Convert.ToInt32(listEmpCareer.Max(w => w.TranNo));
                var IsSalary = BSM.IsHideSalary(staff.LevelCode);
                //Salary Info
                //var OldCareer = DB.HREmpCareerBankLists.Where(w => w.Reference == tranNo && w.EmpCode == EmpCode).ToList();

                //foreach (var read in OldCareer)
                //{
                //    var obj = new HREmpCareerBankList();
                //    obj = read;
                //    if (IsSalary)
                //    {
                //        obj.OldSalary = read.NewSalary;
                //        obj.Increase = 0;
                //        obj.NewSalary = read.NewSalary;
                //    }
                //    else
                //    {
                //        obj.OldSalary = 0;
                //        obj.Increase = 0;
                //        obj.NewSalary = 0;
                //    }

                //    BSM.ListCareerBankList.Add(obj);
                //}
                
                foreach (var read in listEmpCareer.OrderByDescending(w => w.FromDate).ToList())
                {
                    var EmpCar = unitOfWork.Set<HREmpCareer>().AsQueryable()
                      .Where(w => w.EmpCode == read.EmpCode &&
                                  w.CompanyCode == read.CompanyCode &&
                                  w.JobCode == read.JobCode).OrderByDescending(w => w.ToDate).FirstOrDefault();
                    var obj = new HR_HisCareer();
                    obj.FromDate = read.FromDate.Value;
                    obj.ToDate = EmpCar.ToDate.Value;
                    obj.Department = read.DEPT;
                    obj.Company = read.CompanyCode;
                    obj.Position = read.JobCode;
                    obj.Career = read.CareerCode;
                    obj.Level = read.LevelCode;
                    if (IsSalary == true)
                    {
                        obj.NewSalary = read.NewSalary.ToString();
                        obj.OldSalary = read.OldSalary.ToString();
                        obj.Increase = read.Increase.ToString();
                    }
                    else
                    {
                        obj.NewSalary = "#####";
                        obj.OldSalary = "#####";
                        obj.Increase = "#####";
                    }
                    obj.CreatedBy = read.CreateBy;
                    obj.ChangedBy = read.ChangedBy;
                    BSM.ListHisCareer.Add(obj);
                }
                BSM.SetAutoApproval(SCREEN_ID, DOCTYPE, staff.Branch, staff.DEPT);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_Employee");
            }
        }
        #region JobType
        [HttpPost]
		public ActionResult JobType(string code, string addType)
		{
			UserSession();
			UserConfListAndForm();
			if (!string.IsNullOrEmpty(code))
			{
				var _listCom = unitOfWork.Set<HRCompanyGroup>().AsQueryable().Where(w => w.ParentWorkGroupID == addType).ToList();
				string Res = "";
				if (_listCom.Count() > 0)
				{
					var obj = _listCom.FirstOrDefault();
					Res = obj.WorkGroup;
					if (obj.WorkGroup == "Division")
						Session["Division"] = code;
					else if (obj.WorkGroup == "Department")
						Session["Department"] = code;
					else if (obj.WorkGroup == "Position")
						Session["Position"] = code;
					else if (obj.WorkGroup == "Section")
						Session["Section"] = code;
					else if (obj.WorkGroup == "Level")
						Session["Level"] = code;
				}
				var result = new
				{
					MS = Res,
				};
				return Json(result, JsonRequestBehavior.DenyGet);
			}
			var rs = new { MS = SYConstant.FAIL };
			return Json(rs, JsonRequestBehavior.DenyGet);
		}
        public ActionResult GetDivision()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "ESSTransferAndpromotion", Action = "GetDivision" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Description";
                cboProperties.TextFormatString = "{0}:{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(BSM.GetDivision());
            });
        }
        public ActionResult GetDepartment()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "ESSTransferAndpromotion", Action = "GetDepartment" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Description";
                cboProperties.TextFormatString = "{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(BSM.GetDepartment());
            });
        }
        public ActionResult GetPosition()
		{
			UserSession();
			return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
			{
				cboProperties.CallbackRouteValues = new { Controller = "ESSTransferAndpromotion", Action = "GetPosition" };
				cboProperties.Width = Unit.Percentage(100);
				cboProperties.ValueField = "Code";
				cboProperties.TextField = "Description";
				cboProperties.TextFormatString = "{1}";
				cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
				cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
				cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
				cboProperties.BindList(BSM.GetPosition());
			});
		}
		public ActionResult GetSection()
		{
			UserSession();
			return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
			{
				cboProperties.CallbackRouteValues = new { Controller = "ESSTransferAndpromotion", Action = "GetSection" };
				cboProperties.Width = Unit.Percentage(100);
				cboProperties.ValueField = "Code";
				cboProperties.TextField = "Description";
				cboProperties.TextFormatString = "{1}";
				cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
				cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
				cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
				cboProperties.BindList(BSM.GetSection());
			});
		}
		public ActionResult GetLevel()
		{
			UserSession();
			return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
			{
				cboProperties.CallbackRouteValues = new { Controller = "ESSTransferAndpromotion", Action = "GetLevel" };
				cboProperties.Width = Unit.Percentage(100);
				cboProperties.ValueField = "Code";
				cboProperties.TextField = "Description";
				cboProperties.TextFormatString = "{1}";
				cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
				cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
				cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
				cboProperties.BindList(BSM.GetLevel());
			});
		}
		#endregion
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
            ViewData["BRANCHES_SELECT"] = ListBranch;
            ViewData["BU_SELECT"] = unitOfWork.Set<HRGroupDepartment>().AsQueryable().ToList();
            ViewData["Sections_SELECT"] = unitOfWork.Set<HRSection>().AsQueryable().ToList();
            ViewData["Department_SELECT"] = unitOfWork.Set<HRDepartment>().AsQueryable().ToList();
            ViewData["DIVISION_SELECT"] = unitOfWork.Set<HRDivision>().AsQueryable().ToList();
            ViewData["Position_SELECT"] = unitOfWork.Set<HRPosition>().AsQueryable().ToList();
            ViewData["Levels_SELECT"] = ListLevel;
            ViewData["STAFF_SELECT"] = ListStaff.ToList();
            ViewData["TransferType_SELECT"] = unitOfWork.Set<HREFTransferAndPromotionType>().AsQueryable().ToList();
            ViewData["Company_SELECT"] = unitOfWork.Set<HRCompany>().AsQueryable().ToList();
            ViewData["LOCATION_SELECT"] = unitOfWork.Set<HRLocation>().AsQueryable().ToList().OrderBy(w => w.Description);
        }

    }
}