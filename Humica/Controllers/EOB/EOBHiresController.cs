using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.EOB;
using Humica.Logic.MD;
using Humica.Logic.RCM;
using Humica.Models.SY;
using HUMICA.Models.Report.EOB;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Humica.Controllers.EOB
{
    public class EOBHiresController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RCM0000009";
        private const string URL_SCREEN = "/EOB/EOBHires/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ApplicantID";
        private string _Dept_ = "_Dept_";
        private string _DOCTYPE_ = "_DOCTYPE_";
        private string _LOCATION_ = "_LOCATION_";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        IClsEOBHire BSM;
        IUnitOfWork unitOfWork;
        public EOBHiresController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsEOBHire();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        #region 'list'
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            var userName = user.UserName;
            BSM.ListApplicant = new List<RCMApplicant>();
            BSM.ListWaiting = new List<ClsWaitingHire>();
            BSM.ListHire = unitOfWork.Repository<RCMHire>().Queryable().OrderByDescending(w => w.ApplicantID).ToList();
            var allApplicants = unitOfWork.Repository<RCMApplicant>().Queryable().Where(w => !w.IsHired && w.IsConfirm == true).ToList();
            if (allApplicants.Any() && BSM.ListHire.Any())
            {
                var Applic = allApplicants.Where(w => !BSM.ListHire.Any(x => x.ApplicantID == w.ApplicantID)).ToList();
                if (Applic.Any())
                {
                    BSM.ListApplicant = Applic;
                }
            }
            else if (allApplicants.Any() && !BSM.ListHire.Any())
                BSM.ListApplicant = allApplicants;
            BSM.ProcessLoadData(userName);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsEOBHire collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEOBHire)Session[Index_Sess_Obj + ActionName];
            }
            string pass = SYDocumentStatus.PASS.ToString();
            var chkHire = unitOfWork.Repository<RCMHire>().Queryable().Select(w => w.ApplicantID);
            var chkInt = unitOfWork.Repository<RCMPInterview>().Queryable().Where(x => x.Status == pass).Select(x => x.ApplicantID);
            var chkRef = unitOfWork.Repository<RCMRefCheckPerson>().Queryable().Select(x => x.ApplicantID);
            BSM.ListApplicant = unitOfWork.Repository<RCMApplicant>().Queryable().Where(x => chkInt.Contains(x.ApplicantID) && !chkHire.Contains(x.ApplicantID)
                                                                                           && !chkRef.Contains(x.ApplicantID)).ToList();
            collection.ListApplicant = BSM.ListApplicant;
            Session[Index_Sess_Obj + ActionName] = collection;
            return View(collection);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);

            BSM.ListHire = new List<RCMHire>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEOBHire)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHire.ToList());
        }
        public ActionResult LPending()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEOBHire)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("ListPending", BSM.ListWaiting);
        }
        #endregion  
        #region 'Grid' 
        public ActionResult GridCandidate()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.ListApplicant = new List<RCMApplicant>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEOBHire)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridCandidate", BSM.ListApplicant);
        }
        #endregion 
        #region 'Create'
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            BSM.Hire = new RCMHire();
            BSM.ListApproval = new List<ExDocApproval>();
            DateTime DateNow = DateTime.Now;
            DateTime Probation = new DateTime();
            var ProType = unitOfWork.Repository<HRProbationType>().Queryable().FirstOrDefault();
            int ProMonth = 3;
            int day = -1;
            if (ProType != null)
            {
                day = ProType.Day;
                ProMonth = ProType.InMonth;
                BSM.Hire.ProbationType = ProType.Code;
            }
            Probation = DateNow.AddMonths(ProMonth).AddDays(day);
            BSM.Hire.StartDate = DateNow;
            BSM.Hire.ProbationEndDate = Probation;
            BSM.Hire.LeaveConf = DateNow;
            BSM.Hire.IsResident = true;
            BSM.Hire.IsAtten = false;
            BSM.Hire.IsBiSalary = false;
            //BSM.Filter = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(x => x.ApplicantID == ApplicantID);

            //if (BSM.Filter != null)
            //{
            //    BSM.Hire.ApplicantID = BSM.Filter.ApplicantID;
            //    BSM.Hire.ApplicantName = BSM.Filter.AllName;
            //    BSM.Hire.ApplyPosition = BSM.Filter.ApplyPosition;
            //    BSM.Hire.Position = BSM.Filter.PostOffer;
            //    BSM.Hire.Section = BSM.Filter.Sect;
            //    BSM.Hire.Branch = BSM.Filter.ApplyBranch;
            //    BSM.Hire.Department = BSM.Filter.ApplyDept;
            //    BSM.Hire.Salary = BSM.Filter.Salary;
            //    BSM.Hire.ApplyDate = BSM.Filter.ApplyDate;
            //    BSM.Hire.WorkingType = BSM.Filter.WorkingType;
            //}

            if (BSM.Hire != null)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Create( ClsEOBHire collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            BSM = (ClsEOBHire)Session[Index_Sess_Obj + ActionName];
            collection.ListApproval = BSM.ListApproval;
            collection.ScreenId = SCREEN_ID;
            if (Session[PATH_FILE] != null)
            {
                collection.Hire.AttachFile = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            if (ModelState.IsValid)
            {
                string msg = collection.CreateHire();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);

                    Session[Index_Sess_Obj + ActionName] = collection;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

                }
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + ActionName] = collection;
                return View(collection);
            }
            Session[Index_Sess_Obj + this.ActionName] = collection;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region 'Hire'
        public ActionResult Hire(string ApplicantID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            BSM.Hire = new RCMHire();
            BSM.ListApproval = new List<ExDocApproval>();
            DateTime DateNow = DateTime.Now;
            DateTime Probation = new DateTime();
            var ProType = unitOfWork.Repository<HRProbationType>().Queryable().FirstOrDefault();
            int ProMonth = 3;
            int day = -1;
            if (ProType != null)
            {
                day = ProType.Day;
                ProMonth = ProType.InMonth;
                BSM.Hire.ProbationType = ProType.Code;
            }
            Probation = DateNow.AddMonths(ProMonth).AddDays(day);
            BSM.Hire.StartDate = DateNow;
            BSM.Hire.ProbationEndDate = Probation;
            BSM.Hire.LeaveConf = DateNow;
            BSM.Hire.IsResident = true;
            BSM.Hire.IsAtten = false;
            BSM.Hire.IsBiSalary = false;
            BSM.Filter = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(x => x.ApplicantID == ApplicantID);

            if (BSM.Filter != null)
            {
                BSM.Hire.ApplicantID = BSM.Filter.ApplicantID;
                BSM.Hire.ApplicantName = BSM.Filter.AllName;
                BSM.Hire.ApplyPosition = BSM.Filter.ApplyPosition;
                BSM.Hire.Position = BSM.Filter.PostOffer;
                BSM.Hire.Section = BSM.Filter.Sect;
                BSM.Hire.Branch = BSM.Filter.ApplyBranch;
                BSM.Hire.Department = BSM.Filter.ApplyDept;
                BSM.Hire.Salary = BSM.Filter.Salary;
                BSM.Hire.ApplyDate = BSM.Filter.ApplyDate;
                BSM.Hire.WorkingType = BSM.Filter.WorkingType;
            }

            if (BSM.Hire != null)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Hire(string ApplicantID, ClsEOBHire collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            BSM = (ClsEOBHire)Session[Index_Sess_Obj + ActionName];
            collection.ListApproval = BSM.ListApproval;
            collection.ScreenId = SCREEN_ID;
            if (Session[PATH_FILE] != null)
            {
                collection.Hire.AttachFile = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            if (ModelState.IsValid)
            {
                string msg = collection.saveHire(ApplicantID);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);

                    Session[Index_Sess_Obj + ActionName] = collection;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

                }
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + ActionName] = collection;
                return View(collection);
            }
            Session[Index_Sess_Obj + this.ActionName] = collection;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region 'Details'
        public ActionResult Details(string EmpCode)
        {
            ActionName = "Details";
            UserSession();

            DataSelector();
            ViewData[SYConstant.PARAM_ID] = EmpCode;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            BSM.ListHire = new List<RCMHire>();
            BSM.Hire = new RCMHire();
            BSM.Hire = unitOfWork.Repository<RCMHire>().Queryable().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (BSM.Hire == null)
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            BSM.ListApproval = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == EmpCode && w.DocumentType == BSM.Hire.StaffType).ToList();
            UserConfForm(SYActionBehavior.VIEW);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion
        #region "Delete"
        public ActionResult Delete(string EmpCode)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (EmpCode == "null") EmpCode = null;
            if (EmpCode != null)
            {
                string msg = BSM.Delete(EmpCode);
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
        #region Request For Approve
        public ActionResult RequestForApproval(string EmpCode)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (EmpCode == "null") EmpCode = null;
            if (!string.IsNullOrEmpty(EmpCode))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Request(EmpCode);
                if (msg == SYConstant.OK)
                {
                    #region template
                    //string receiver = string.Empty;
                    //DateTime currentDate = DateTime.Now;
                    //var app_ = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == EmpCode).OrderBy(w => w.ApproveLevel).FirstOrDefault();
                    //if (app_ != null) receiver = app_.Approver;
                    //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    //string fileName = Server.MapPath("~/Content/UPLOAD/" + "Request form_" + currentDate.ToString("MMMM-yyyy") + ".pdf");
                    //FRMEOBHire sa = new FRMEOBHire();
                    //var reportHelper = new clsReportHelper();
                    //string path = reportHelper.Get_Path(SCREEN_ID);
                    //if (!string.IsNullOrEmpty(path))
                    //{
                    //    sa.LoadLayoutFromXml(path);
                    //}
                    //sa.Parameters["EmpCode"].Value = EmpCode;
                    //sa.Parameters["EmpCode"].Visible = false;
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

        public ActionResult Cancel(string EmpCode)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (EmpCode == "null") EmpCode = null;
            if (!string.IsNullOrEmpty(EmpCode))
            {
                string msg = BSM.Cancel(EmpCode);
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
        public ActionResult Approve(string EmpCode)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (EmpCode == "null") EmpCode = null;
            if (!string.IsNullOrEmpty(EmpCode))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = "";
                string[] c = EmpCode.Split(';');
                foreach (var Document in c)
                {
                    //FRMEOBHire sa = new FRMEOBHire();
                    //var reportHelper = new clsReportHelper();
                    //string path = reportHelper.Get_Path(SCREEN_ID);
                    //if (!string.IsNullOrEmpty(path))
                    //{
                    //    sa.LoadLayoutFromXml(path);
                    //}
                    //var _Appro = unitOfWork.Repository<ExDocApproval>().Queryable().FirstOrDefault(w => w.DocumentNo == Document && w.Approver == user.UserName);
                    //int level = 0;
                    //if (_Appro != null) level = _Appro.ApproveLevel;
                    //sa.Parameters["EmpCode"].Value = Document;
                    //sa.Parameters["APROLEVEL"].Value = level;
                    //sa.Parameters["EmpCode"].Visible = false;
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
        public ActionResult Reject(string EmpCode)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (EmpCode == "null") EmpCode = null;
            if (!string.IsNullOrEmpty(EmpCode))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Reject(EmpCode);
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
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

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
                var obj = unitOfWork.Repository<RCMHire>().Queryable().Where(w => w.EmpCode == id).ToList();
                var Approve = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == id && w.Status == SYDocumentStatus.APPROVED.ToString()).OrderByDescending(w => w.ApproveLevel).ToList();
                if (obj.Count > 0)
                {
                    try
                    {
                        ViewData[SYSConstant.PARAM_ID] = id;
                        FRMEOBHire sa = new FRMEOBHire();
                        var reportHelper = new clsReportHelper();
                        string path = reportHelper.Get_Path(SCREEN_ID);
                        if (!string.IsNullOrEmpty(path))
                            sa.LoadLayoutFromXml(path);
                        int Level = 0;
                        if (Approve.Any()) Level = Approve.FirstOrDefault().ApproveLevel;
                        sa.Parameters["EmpCode"].Value = obj.FirstOrDefault().EmpCode;
                        sa.Parameters["APROLEVEL"].Value = Level;
                        sa.Parameters["EmpCode"].Visible = false;
                        sa.Parameters["APROLEVEL"].Visible = false;

                        Session[this.Index_Sess_Obj + this.ActionName] = sa;
                        return PartialView("PrintForm", sa);
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
        public ActionResult DocumentViewerExportTo(string EmpCode)
        {
            ActionName = "Print";
            FRMEOBHire reportModel = (FRMEOBHire)Session[Index_Sess_Obj + ActionName];
            ViewData[SYSConstant.PARAM_ID] = EmpCode;
            return ReportViewerExtension.ExportTo(reportModel);
        }
        #endregion
        #region "Ajax"
        [HttpPost]
        public string FitlerLevel(string code)
        {

            UserSession();
            UserConfListAndForm();
            if (code != null)
            {
                Session["Level"] = code;
            }

            return SYConstant.OK;
        }
        public ActionResult JobGrade()
        {
            UserSession();
            UserConfListAndForm();
            if (Session["Level"] != null)
            {
                string LevelCode = Session["Level"].ToString();
                var District = unitOfWork.Repository<HRJobGrade>().Queryable().ToList();
                ViewData["JOBGRADE_SELECT"] = District.ToList();
                return PartialView("JobGrade");
            }
            return null;
        }
        #endregion
        #region "Ajax Image'
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = unitOfWork.Repository<CFUploadPath>().Queryable().FirstOrDefault(w => w.PathCode == "IMG_UPLOAD");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);

            objFile.ObjectTemplate = new MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREEN_ID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;

            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("uc_image", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            Session["PATH_IMG"] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }
        public string DeleteFile(string FileName)
        {
            try
            {
                string[] sp = FileName.Split('/');
                string file = sp[sp.Length - 1];
                string[] spf = file.Split('.');
                string sfile = spf[0];

                var obj = unitOfWork.Repository<MDUploadImage>().Queryable().FirstOrDefault(w => w.TokenCode == sfile);
                if (obj != null)
                {
                    unitOfWork.Delete(obj);
                    unitOfWork.Save();
                    //if (row > 0)
                    //{
                    var path = unitOfWork.Repository<CFUploadPath>().Queryable().FirstOrDefault(w => w.PathCode == "IMG_UPLOAD");
                    string root = HostingEnvironment.ApplicationPhysicalPath;
                    // obj.UpoadPath = obj.UpoadPath.Replace("~", "").Replace("/", "\\");
                    obj.UpoadPath = obj.UpoadPath.Replace("~/", "").Replace("/", "\\");
                    string fileNameDelete = root + obj.UpoadPath;
                    if (System.IO.File.Exists(fileNameDelete))
                    {
                        System.IO.File.Delete(fileNameDelete);
                    }
                    //}
                }
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion
        #region Upload
        [HttpPost]
        public ActionResult UploadControlCallbackActionfile()
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
        #region Approval
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
                BSM = (ClsEOBHire)Session[Index_Sess_Obj + ActionName];
                BSM.SetAutoApproval(SCREEN_ID, docType, location, Dept);
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
                BSM = (ClsEOBHire)Session[Index_Sess_Obj + ActionName];
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
                        BSM = (ClsEOBHire)Session[Index_Sess_Obj + ActionName];
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
                        BSM = (ClsEOBHire)Session[Index_Sess_Obj + ActionName];
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
                BSM = (ClsEOBHire)Session[Index_Sess_Obj + ActionName];
                var listcheck = BSM.ListApproval.Where(w => w.Approver == approval.Approver).ToList();
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
        #region 'Private Code'

        [HttpPost]
        public ActionResult ShowData(DateTime StartDate, string ProType)
        {
            var Pro = unitOfWork.Repository<HRProbationType>().Queryable().FirstOrDefault(w => w.Code == ProType);
            int Months = 0;
            int day = 0;
            if (Pro != null)
            {
                Months = Pro.InMonth;
                day = Pro.Day;
            }
            DateTime Probation = StartDate.AddMonths(Months).AddDays(day);
            DateTime LeaveCof = StartDate;
            var result = new
            {
                MS = SYConstant.OK,
                Probation = Probation,
                LeaveCof = LeaveCof
            };
            return Json(result, JsonRequestBehavior.DenyGet);
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
            SYDataList objList = new SYDataList("SEX");
            ViewData["GENDER_SELECT"] = objList.ListData;
            objList = new SYDataList("INITIAL");
            ViewData["INITIAL_SELECT"] = objList.ListData;
            objList = new SYDataList("MARITAL");
            ViewData["MARITAL_SELECT"] = objList.ListData;
            var objStatus = new SYDataList("STATUS_EMPLOYEE");
            ViewData["STATUS_EMPLOYEE"] = objStatus.ListData;
            var _listBranch = SYConstant.getBranchDataAccess();
            ViewData["BRANCHES_SELECT"] = _listBranch.ToList();
            ViewData["COMPANY_SELECT"] = unitOfWork.Repository<HRCompany>().Queryable().ToList();
            ViewData["DIVISION_SELECT"] = unitOfWork.Repository<HRDivision>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["EMPTYPE_SELECT"] = unitOfWork.Repository<HREmpType>().Queryable().ToList().OrderBy(w => w.Description);
            var ObjTax = new SYDataList("TAXPAID");
            ViewData["TAXPAID_SELECT"] = ObjTax.ListData;
            ViewData["SECTIONS_SELECT"] = unitOfWork.Repository<HRSection>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData["JOBGRADE_SELECT"] = unitOfWork.Repository<HRJobGrade>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["POSITION_SELECT"] = unitOfWork.Repository<HRPosition>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["DEPARTMENT_SELECT"] = unitOfWork.Repository<HRDepartment>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["LOCATION_SELECT"] = unitOfWork.Repository<HRLocation>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["LINE_SELECT"] = unitOfWork.Repository<HRLine>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["PERAMETER_SELECT"] = unitOfWork.Repository<PRParameter>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["POSITIONFAMILY_SELECT"] = unitOfWork.Repository<HRPositionFamily>().Queryable().Where(w => w.IsActive == true).ToList().OrderBy(w => w.Description);
            var IdentityTye = new SYDataList("IdentityTye");
            ViewData["IdentityTye_SELECT"] = IdentityTye.ListData;
            ViewData["PROBATIONTYPE_SELECT"] = unitOfWork.Repository<HRProbationType>().Queryable().ToList();
            ViewData["ROSTER_SELECT"] = unitOfWork.Repository<ATBatch>().Queryable().ToList();
            ViewData["WORKINGTYPE_SELECT"] = unitOfWork.Repository<HRWorkingType>().Queryable().ToList();
            var SalaryType = new SYDataList("SALARYTYPE_SELECT");
            ViewData["SALARYTYPE_SELECT"] = SalaryType.ListData;
            ViewData["STAFF_SELECT"] = unitOfWork.Repository<HRStaffProfile>().Queryable().ToList();
            // ViewData["Company_SELECT"] = SYConstant.getCompanyDataAccess();
            //var results = unitOfWork.Repository<RCMApplicant>().Queryable().GroupBy(n => new { n.Vacancy, n.ApplyPosition })
            //        .Select(g => new {
            //            g.Key.Vacancy,
            //            g.Key.ApplyPosition
            //        }).ToList();
            //ViewData["VACANCY_SELECT"] = results.ToList();
        }
        #endregion 
    }
}