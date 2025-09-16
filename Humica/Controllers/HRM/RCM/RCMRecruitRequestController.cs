using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.RCM;
using Humica.Models.SY;
using Humica.Logic.MD;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Humica.EF.Repo;
using HUMICA.Models.Report.RCM;
using System.Net;
namespace Humica.Controllers.HRM.RCM
{
    public class RCMRecruitRequestController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RCM0000001";
        private const string URL_SCREEN = "/HRM/RCM/RCMRecruitRequest/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "DocNo";
        private string _DOCTYPE_ = "_DOCTYPE_";
        private string _LOCATION_ = "_LOCATION_";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string _Dept_ = "_Dept_";
        IClsRCMRequest BSM;
        IUnitOfWork unitOfWork;
        public RCMRecruitRequestController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsRCMRequest();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }

        #region 'List'
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            var userName = user.UserName;
            //BSM.Filters = new Filters();
            BSM.ListWaiting = new List<ClsWaitingList>();
            BSM.ListHeader = new List<RCMRecruitRequest>();
            BSM.ProcessLoadData(userName);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        //[HttpPost]
        //public ActionResult Index(ClsRCMRequest collection)
        //{
        //    ActionName = "Index";
        //    UserSession();
        //    UserConfListAndForm(this.KeyName);
        //    DataSelector();
        //   
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        BSM = (ClsRCMRequest)Session[Index_Sess_Obj + ActionName];
        //    }
        //    BSM.ListRecruitRequest = new List<RCM_RecruitRequest_VIEW>();
        //    BSM.Filters = collection.Filters;
        //    DateTime fromDate = BSM.Filters.FromDate.HasValue ? BSM.Filters.FromDate.Value : Core.DateTimeHelper.MinValue;
        //    DateTime toDate = BSM.Filters.ToDate.HasValue ? BSM.Filters.ToDate.Value : Core.DateTimeHelper.MaxValue;
        //    var _listRequest = await unitOfWork.Repository<RCMRecruitRequest>().Queryable().Where(w => EntityFunctions.TruncateTime(w.RequestedDate) >= fromDate.Date &&
        //        EntityFunctions.TruncateTime(w.RequestedDate) <= toDate.Date).ToListAsync();
        //    if (BSM.Filters.Status?.ToUpper() == "ALL")
        //    {
        //        BSM.ListHeader = _listRequest.ToList();
        //    }
        //    else
        //    {
        //        BSM.ListHeader = _listRequest.Where(w => w.Status == BSM.Filters.Status).ToList();
        //    }
        //    Session[Index_Sess_Obj + ActionName] = BSM;
        //    return View(BSM);
        //}
        public ActionResult LPending()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);

            //BSM.ListWaiting = new List<ClsWaitingList>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMRequest)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("ListPending", BSM.ListWaiting);
        }
        public ActionResult _ListRequest()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
           
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMRequest)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_ListRequest", BSM.ListHeader);
        }
        #endregion 
        #region 'Create'
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM.Header = new RCMRecruitRequest();
            BSM.ListHeader = new List<RCMRecruitRequest>();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.ExCfWFApprover = new ExCfWFApprover();
            BSM.Header.ExpectedDate = DateTime.Now.AddDays(30);
            BSM.Header.RequestedDate = DateTime.Now;
            BSM.Header.Gender = "B";
            BSM.Header.StaffType = "KHMER";
            BSM.Header.RecruitFor = "New";
            BSM.Header.RecruitType = "INE";
            BSM.Header.ProposedSalaryFrom = 0;
            BSM.Header.ProposedSalaryTo = 0;
            BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsRCMRequest collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.ADD);
            DataSelector();

            var BSM = (ClsRCMRequest)Session[Index_Sess_Obj + ActionName];
            collection.ListApproval = BSM.ListApproval;
            collection.ScreenId = SCREEN_ID;
            if (Session[PATH_FILE] != null)
            {
                collection.Header.Attachment = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            string RequestNo = string.Empty;
            string msg = collection.createRRequest(RequestNo);

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = collection.Header.RequestNo;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?RequestNo=" + mess.DocumentNumber;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;

                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
            }

            Session[Index_Sess_Obj + this.ActionName] = collection;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            return View(collection);
        }
        #endregion 
        #region 'Details'
        public ActionResult Details(string RequestNo)
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ViewData[SYConstant.PARAM_ID] = RequestNo;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            BSM.Header = new RCMRecruitRequest();
            BSM.ListApproval = new List<ExDocApproval>();
            if (RequestNo == "null") RequestNo = null;
            if (!string.IsNullOrEmpty(RequestNo))
            {
                var obj = unitOfWork.Repository<RCMRecruitRequest>().Queryable().FirstOrDefault(w => w.RequestNo == RequestNo);
                if (obj != null)
                {
                    BSM.Header = obj;
                    var Approver = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == RequestNo && w.DocumentType == BSM.Header.DocType).ToList();
                    if (Approver.Any()) BSM.ListApproval = Approver;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            UserConfForm(SYActionBehavior.VIEW);
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region 'Edit'
        public ActionResult Edit(string RequestNo)
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (RequestNo == "null") RequestNo = null;
            if (!string.IsNullOrEmpty(RequestNo))
            {
                BSM.Header = new RCMRecruitRequest();
                BSM.ListApproval = new List<ExDocApproval>();
                var obj = unitOfWork.Repository<RCMRecruitRequest>().Queryable().FirstOrDefault(w => w.RequestNo == RequestNo);
                if (obj != null)
                {
                    BSM.Header = obj;
                    var Approver = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == RequestNo && w.DocumentType == BSM.Header.DocType).ToList();
                    if (Approver.Any()) BSM.ListApproval = Approver;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string RequestNo, ClsRCMRequest collection)
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMRequest)Session[Index_Sess_Obj + ActionName];
                collection.ListApproval = BSM.ListApproval;
            }
            if (Session[PATH_FILE] != null)
            {
                collection.Header.Attachment = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            collection.ScreenId = SCREEN_ID;
            if (!string.IsNullOrEmpty(RequestNo))
            {
                string msg = collection.UpdRRequest(RequestNo);

                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(collection);
                }
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = RequestNo;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?RequestNo=" + mess.DocumentNumber;
                Session[Index_Sess_Obj + this.ActionName] = collection;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(collection);
            }
            Session[Index_Sess_Obj + this.ActionName] = collection;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(collection);
        }
        #endregion
        #region 'Delete'  
        public ActionResult Delete(string RequestNo)
        {
            UserSession();
            if (RequestNo == "null") RequestNo = null;
            if (!string.IsNullOrEmpty(RequestNo))
            {
                var obj = unitOfWork.Repository<RCMRecruitRequest>().Queryable().FirstOrDefault(w => w.RequestNo == RequestNo);
                if (obj != null)
                {
                    string msg = BSM.deleteRRequest(RequestNo);

                    if (msg == SYConstant.OK)
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DELETE_M", user.Lang);
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                    else
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Request For Approve
        public ActionResult RequestForApproval(string RequestNo)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (RequestNo == "null") RequestNo = null;
            if (!string.IsNullOrEmpty(RequestNo))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Request(RequestNo);
                if (msg == SYConstant.OK)
                {
                    #region template
                    //string receiver = string.Empty;
                    //DateTime currentDate = DateTime.Now;
                    //var app_ = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == RequestNo).OrderBy(w => w.ApproveLevel).FirstOrDefault();
                    //if (app_ != null) receiver = app_.Approver;
                    //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    //string fileName = Server.MapPath("~/Content/UPLOAD/" + "Request form_" + currentDate.ToString("MMMM-yyyy") + ".pdf");
                    //FRMRequestForm sa = new FRMRequestForm();
                    //var reportHelper = new clsReportHelper();
                    //string path = reportHelper.Get_Path(SCREEN_ID);
                    //if (!string.IsNullOrEmpty(path))
                    //{
                    //    sa.LoadLayoutFromXml(path);
                    //}
                    //sa.Parameters["DocNo"].Value = RequestNo;
                    //sa.Parameters["DocNo"].Visible = false;
                    //sa.Parameters["APROLEVEL"].Value = 0;
                    //sa.Parameters["APROLEVEL"].Visible = false;
                    //string UploadDirectory = Server.MapPath("~/Content/UPLOAD");
                    //if (!Directory.Exists(UploadDirectory))
                    //{
                    //    Directory.CreateDirectory(UploadDirectory);
                    //}
                    //sa.ExportToPdf(fileName);
                    //BSM.SendEmail(fileName, receiver);
                    #endregion
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    SYMessages mess = SYMessages.getMessageObject("DOC_RELEASED", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
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
        #region 'Convert Status'

        public ActionResult Cancel(string RequestNo)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (RequestNo == "null") RequestNo = null;
            if (!string.IsNullOrEmpty(RequestNo))
            {
                string msg = BSM.Cancel(RequestNo);
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
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Approve(string RequestNo)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (RequestNo == "null") RequestNo = null;
            if (!string.IsNullOrEmpty(RequestNo))
            {
                string msg = "";
                string[] c = RequestNo.Split(';');
                foreach (var Document in c)
                {
                    ViewData[SYSConstant.PARAM_ID] = Document;
                    if (Document == "") continue;
                    BSM.ScreenId = SCREEN_ID;
                    //FRMRequestForm sa = new FRMRequestForm();
                    //var reportHelper = new clsReportHelper();
                    //string path = reportHelper.Get_Path(SCREEN_ID);
                    //if (!string.IsNullOrEmpty(path))
                    //{
                    //    sa.LoadLayoutFromXml(path);
                    //}
                    //var _Appro = unitOfWork.Repository<ExDocApproval>().Queryable().FirstOrDefault(w => w.DocumentNo == Document && w.Approver == user.UserName);
                    //int level = 0;
                    //if (_Appro != null) level = _Appro.ApproveLevel;
                    //sa.Parameters["DocNo"].Value = Document;
                    //sa.Parameters["APROLEVEL"].Value = level;
                    //sa.Parameters["DocNo"].Visible = false;
                    //sa.Parameters["APROLEVEL"].Visible = false;
                    //Session[this.Index_Sess_Obj + this.ActionName] = sa;

                    //string fileName = Server.MapPath("~/Content/UPLOAD/" + "STAFF REQUISITION FORM.pdf");
                    //string UploadDirectory = Server.MapPath("~/Content/UPLOAD");
                    //if (!Directory.Exists(UploadDirectory))
                    //{
                    //    Directory.CreateDirectory(UploadDirectory);
                    //}
                    //sa.ExportToPdf(fileName);
                    //string msg = BSM.Approved(Document, fileName);
                    msg = BSM.Approved(Document);
                }
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
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Reject(string RequestNo)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (RequestNo == "null") RequestNo = null;
            if (!string.IsNullOrEmpty(RequestNo))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Reject(RequestNo);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_REJECT", user.Lang);
                    mess.Description = mess.Description + ". " + BSM.MessageCode;
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
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?RequestNo=" + RequestNo);

        }
        #endregion 
        #region 'Print'
        public ActionResult Print(string id)
        {
            this.UserSession();
            this.UserConf(ActionBehavior.VIEWONLY);
            ViewData[SYSConstant.PARAM_ID] = id;
            this.UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string id)
        {
            this.UserSession();
            this.UserConf(ActionBehavior.VIEWONLY);
            this.ActionName = "Print";
            if (id == "null") id = null;
            if (!string.IsNullOrEmpty(id))
            {
                var obj = unitOfWork.Repository<RCMRecruitRequest>().Queryable().Where(w => w.RequestNo == id).ToList();
                var Approve = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == id && w.Status == SYDocumentStatus.APPROVED.ToString()).OrderByDescending(w => w.ApproveLevel).ToList();
                if (obj.Count > 0)
                {
                    try
                    {
                        ViewData[SYSConstant.PARAM_ID] = id;
                        FRMRequestForm FRMRequest = new FRMRequestForm();
                        var reportHelper = new clsReportHelper();
                        string path = reportHelper.Get_Path(SCREEN_ID);
                        if (!string.IsNullOrEmpty(path))
                        {
                            FRMRequest.LoadLayoutFromXml(path);
                        }
                        int Level = 0;
                        if (Approve.Any()) Level = Approve.FirstOrDefault().ApproveLevel;
                        FRMRequest.Parameters["DocNo"].Value = obj.First().RequestNo;
                        FRMRequest.Parameters["APROLEVEL"].Value = Level;
                        FRMRequest.Parameters["DocNo"].Visible = false;
                        FRMRequest.Parameters["APROLEVEL"].Visible = false;

                        Session[this.Index_Sess_Obj + this.ActionName] = FRMRequest;
                        return PartialView("PrintForm", FRMRequest);
                    }
                    catch (Exception ex)
                    {
                        SYEventLogObject.saveEventLog(new SYEventLog()
                        {
                            ScreenId = SCREEN_ID,
                            UserId = this.user.UserID.ToString(),
                            DocurmentAction = id,
                            Action = SYActionBehavior.ADD.ToString()
                        }, ex, true);
                    }
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string RequestNo)
        {
            ActionName = "Print";
            FRMRequestForm reportModel = (FRMRequestForm)Session[Index_Sess_Obj + ActionName];
            ViewData[SYSConstant.PARAM_ID] = RequestNo;
            return ReportViewerExtension.ExportTo(reportModel);
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
        #region 'Private Code'

        public ActionResult ShowData(string Code, string Action)
        {
            ActionName = Action;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMRequest)Session[Index_Sess_Obj + ActionName];
            }
            var _JDesc = unitOfWork.Repository<RCMSJobDesc>().Queryable().FirstOrDefault(w => w.Code == Code);
            if (_JDesc != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    JobRespon = _JDesc.JobResponsibility,
                    JobRequire = _JDesc.JobRequirement
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult GetPosition(string Code)
        {
            if (!string.IsNullOrEmpty(Code))
            {
                Session["Position"] = Code;
                var result = new
                {
                    MS = SYConstant.OK,
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        #endregion
        #region "Ajax Approval"
        public ActionResult SelectParam(string docType, string location, string Dept)
        {
            UserSession();
            Session[_DOCTYPE_] = docType;
            Session[_LOCATION_] = location;
            Session[_Dept_] = Dept;
            var rs = new { MS = SYConstant.OK };
            ActionName = "Create";
           
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMRequest)Session[Index_Sess_Obj + ActionName];
                BSM.SetAutoApproval(SCREEN_ID,docType, location, Dept);
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
           
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMRequest)Session[Index_Sess_Obj + ActionName];
            }
            DataApprover();
            return PartialView("GridApproval", BSM.ListApproval);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateApproval(ExDocApproval ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataApprover();

            if (!string.IsNullOrEmpty(ModelObject.Approver))
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsRCMRequest)Session[Index_Sess_Obj + ActionName];
                    }
                    if (BSM.ListApproval.Count == 0)
                    {
                        ModelObject.ID = 1;
                    }
                    else
                    {
                        ModelObject.ID = BSM.ListApproval.Max(w => w.ID) + 1;
                    }
                    ModelObject.DocumentNo = "N/A";
                    BSM.ListApproval.Add(ModelObject);

                    Session[Index_Sess_Obj + ActionName] = BSM;

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            return PartialView("GridApproval", BSM.ListApproval);
        }

        public ActionResult DeleteApproval(string Approver)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
           
            if (!string.IsNullOrEmpty(Approver))
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsRCMRequest)Session[Index_Sess_Obj + ActionName];
                    }

                    BSM.ListApproval.Where(w => w.Approver == Approver).ToList();
                    if (BSM.ListApproval.Count > 0)
                    {
                        var objDel = BSM.ListApproval.Where(w => w.Approver == Approver).First();
                        BSM.ListApproval.Remove(objDel);
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("APPROVER_NE");
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
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            DataApprover();

            return PartialView("GridApproval", BSM.ListApproval);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditApproval(ExDocApproval approval)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
           
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM=(ClsRCMRequest)Session[Index_Sess_Obj + ActionName];
                var listcheck = BSM.ListApproval.Where(w=>w.Approver == approval.Approver).ToList();
                if (listcheck.ToList().Count > 0)
                {
                    var objUpdate = listcheck.First();
                    objUpdate.ApproveLevel = approval.ApproveLevel;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }
            DataApprover();
            return PartialView("GridApproval", BSM.ListApproval);
        }
        #endregion

        //public ActionResult GetJD()
        //{
        //    UserSession();
        //    return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
        //    {
        //        cboProperties.CallbackRouteValues = new { Controller = "RCMRecruitRequest", Action = "GetJD" };
        //        cboProperties.Width = Unit.Percentage(100);
        //        cboProperties.ValueField = "Code";
        //        cboProperties.TextField = "Description";
        //        cboProperties.TextFormatString = "{1}";
        //        cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
        //        cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
        //        cboProperties.BindList(ClsRCMRequest.GetJD());
        //    });
        //}
        public ActionResult GetJD()
        {
            UserSession();
            var jobDescriptions = ClsRCMRequest.GetJD(unitOfWork);

            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Description";
                cboProperties.TextFormatString = "{0} - {1}";
                cboProperties.Columns.Add(col => {
                    col.FieldName = "Code";
                    col.Caption = SYSettings.getLabel("Code");
                    col.Width = 70;
                });
                cboProperties.Columns.Add(col => {
                    col.FieldName = "Description";
                    col.Caption = SYSettings.getLabel("Description");
                    col.Width = 250;
                });
                cboProperties.BindList(jobDescriptions);
            });
        }
        private void DataApprover()
        {
            var objWf = new ExWorkFlowPreference();
            var Branch = "";
            if (Session[_LOCATION_] != null)
                Branch = Session[_LOCATION_].ToString();
            var docType = "";
            if (Session[_DOCTYPE_] != null)
                docType = Session[_DOCTYPE_].ToString();
            var Dept = "";
            if (Session[_Dept_] != null)
                Dept = Session[_Dept_].ToString();
            ViewData["WF_LIST"] = objWf.getApproverListByDocType(docType, Branch, SCREEN_ID, Dept);
        }
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("GENDER_SELECT");
            ViewData["GENDER_SELECT"] = objList.ListData;
            ViewData["COUNTRY_SELECT"] = unitOfWork.Repository<HRCountry>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["NATION_SELECT"] = unitOfWork.Repository<HRNation>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData["POSITION_SELECT"] = unitOfWork.Repository<HRPosition>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["WORKINGTYPE_SELECT"] = unitOfWork.Repository<HRWorkingType>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["DEPARTMENT_SELECT"] = unitOfWork.Repository<HRDepartment>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["SECTION_SELECT"] = unitOfWork.Repository<HRSection>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["CONTRACT"] = unitOfWork.Repository<HRContractType>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            objList = new SYDataList("KINDSEARCH_SELECT");
            ViewData["Kind_Search_SELECT"] = objList.ListData;
            objList = new SYDataList("TERM_RC");
            ViewData["TERM_SELECT"] = objList.ListData;
            objList = new SYDataList("RECRUITFOR_SELECT");
            ViewData["RECRUITFOR_SELECT"] = objList.ListData;
            objList = new SYDataList("RECRUITTYPE_SELECT");
            ViewData["RECRUITTYPE_SELECT"] = objList.ListData;
            ViewData["EMPCODE_SELECT"] = unitOfWork.Repository<HRStaffProfile>().Queryable().Select(s => new { s.EmpCode, s.AllName }).ToList();
            objList = new SYDataList("STAFF_TYPE");
            ViewData["STAFFTYPE_SELECT"] = objList.ListData;
            objList = new SYDataList("STATUS_LEAVE_APPROVAL");
            ViewData["STATUS_APPROVAL"] = objList.ListData;
            objList = new SYDataList("WORKING_TYPE");
            ViewData["WORKING_TYPE"] = objList.ListData;
        }
    }
}
