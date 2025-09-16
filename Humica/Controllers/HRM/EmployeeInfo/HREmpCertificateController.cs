using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.EmployeeInfo
{
    public class HREmpCertificateController : Humica.EF.Controllers.MasterSaleController
    {
        // GET: HREmpCertificate
        private const string SCREEN_ID = "HRE0000005";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HREmpCertificate/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public HREmpCertificateController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region List
        public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HREmpCertiObject BSM = new HREmpCertiObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (HREmpCertiObject)Session[Index_Sess_Obj + ActionName];
            }
            await BSM.LoadData();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HREmpCertiObject BSM = new HREmpCertiObject();
            BSM.ListStaffView = new List<HR_STAFF_VIEW>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpCertiObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            HREmpCertiObject BSM = new HREmpCertiObject();
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpCertiObject)Session[Index_Sess_Obj + ActionName];
            }
            //BSM.ListHeader = new List<HR_Family_View>();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM);
        }
		#endregion
		#region 'Create'
		public async Task<ActionResult> Create()
        {
            ActionName = "Create";
            UserSession();
            await DataSelector();
            UserConfListAndForm(this.KeyName);
            HREmpCertiObject BSM = new HREmpCertiObject();
            BSM.ListHeader = new List<HREmpCertificate>();
            BSM.Header = new HREmpCertificate();
            BSM.Header.IssueDate = DateTime.Now;
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
		public async Task<ActionResult> Create(HREmpCertiObject collection)
        {
            UserSession();
            await DataSelector();
            UserConfListAndForm(this.KeyName);
            ActionName = "Create";
            HREmpCertiObject BSM = new HREmpCertiObject();
            if (ModelState.IsValid)
            {
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.AttachFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpCertiObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.HeaderStaff = collection.HeaderStaff;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateCertificate();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                    mess_err.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess_err.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess_err.DocumentNumber;
                    BSM.Header = new HREmpCertificate();
                    BSM.Header.IssueDate = DateTime.Now;
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
		#region 'Edit'
		public async Task<ActionResult> Edit(string ID)
        {
            ActionName = "Create";
            UserSession();
            await DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            if (ID == "null") ID = null;
            if (ID != null)
            {
                HREmpCertiObject BSM = new HREmpCertiObject();
                BSM.Header = new HREmpCertificate();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(ID);
                BSM.Header = DB.HREmpCertificates.FirstOrDefault(w => w.TranNo == TranNo);
                var resualt = DB.HREmpCertificates;
                if (BSM.Header != null)
                {
                    List<HREmpCertificate> listEmpfa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                    BSM.ListHeader = listEmpfa.ToList();
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
		public async Task<ActionResult> Edit(string id, HREmpCertiObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            await DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            HREmpCertiObject BSM = new HREmpCertiObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpCertiObject)Session[Index_Sess_Obj + ActionName];
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
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpCertiObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                int TranNo = Convert.ToInt32(id);
                string msg = BSM.EditCertificate(TranNo);
                if (msg == SYConstant.OK)
                {
                    DB = new HumicaDBContext();
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
		#region "Delete"
		public async Task<ActionResult> Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            await DataSelector();
            if (id == "null") id = null;
            if (id != null)
            {
                int TranNo = Convert.ToInt32(id);
                HREmpCertiObject Del = new HREmpCertiObject();
                string msg = Del.DeleteCertificate(TranNo);
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
		#region "Print"
		//public ActionResult Print(string id)
		//{
		//    UserSession();
		//    UserConf(ActionBehavior.VIEWONLY);
		//    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
		//    UserMVCReportView();
		//    return View("ReportView");
		//}
		//public ActionResult DocumentViewerPartial(string id)
		//{
		//    UserSession();
		//    UserConf(ActionBehavior.VIEWONLY);
		//    ActionName = "Print";
		//    //UserMVC();
		//    int TranNo = Convert.ToInt32(id);
		//    var SD = DP.HREmpContracts.FirstOrDefault(w => w.TranNo == TranNo);
		//    if (SD != null)
		//    {
		//        try
		//        {
		//            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
		//            string Company = DB.SYHRCompanies.FirstOrDefault().CompENG;
		//            XtraReport reportModel = new XtraReport();
		//            if (SD.ConType == "CE")
		//            {
		//                reportModel = new Models.Report.RptEmployeeContract();

		//            }
		//            else if (SD.ConType == "LE")
		//            {
		//                reportModel = new Models.Report.RptEmpContractLetter();
		//                reportModel.Parameters["Company"].Value = Company;
		//                reportModel.Parameters["Company"].Visible = false;
		//            }
		//            else if (SD.ConType == "CP")
		//            {
		//                reportModel = new Models.Report.RptEmpContractProbation();
		//                reportModel.Parameters["Company"].Value = Company;
		//                reportModel.Parameters["Company"].Visible = false;
		//            }

		//            reportModel.Parameters["TranNo"].Value = SD.TranNo;
		//            reportModel.Parameters["TranNo"].Visible = false;

		//            Session[Index_Sess_Obj + ActionName] = reportModel;

		//            return PartialView("PrintForm", reportModel);
		//        }
		//        catch (Exception e)
		//        {
		//            /*------------------SaveLog----------------------------------*/
		//            SYEventLog log = new SYEventLog();
		//            log.ScreenId = SCREEN_ID;
		//            log.UserId = user.UserID.ToString();
		//            log.DocurmentAction = id;
		//            log.Action = SYActionBehavior.ADD.ToString();

		//            SYEventLogObject.saveEventLog(log, e, true);
		//            /*----------------------------------------------------------*/
		//        }
		//    }
		//    return null;
		//}
		//public ActionResult DocumentViewerExportTo(string id)
		//{
		//    ActionName = "Print";
		//    int TranNo = Convert.ToInt32(id);
		//    var SD = DP.HREmpContracts.FirstOrDefault(w => w.TranNo == TranNo);
		//    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
		//    if (SD != null)
		//    {
		//        XtraReport reportModel = new XtraReport();
		//        if (SD.ConType == "CE")
		//        {
		//            reportModel = (RptEmployeeContract)Session[Index_Sess_Obj + ActionName];
		//        }
		//        else if (SD.ConType == "LE")
		//        {
		//            reportModel = (RptEmpContractLetter)Session[Index_Sess_Obj + ActionName];
		//        }
		//        else if (SD.ConType == "CP")
		//        {
		//            reportModel = (RptEmpContractProbation)Session[Index_Sess_Obj + ActionName];

		//        }
		//        return ReportViewerExtension.ExportTo(reportModel);
		//    }
		//    return null;
		//}


		#endregion
		public async Task<ActionResult> Details(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            await DataSelector();
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id == "null") id = null;
            if (id != null)
            {
                HREmpCertiObject BSM = new HREmpCertiObject();
                BSM.Header = new HREmpCertificate();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.HREmpCertificates.FirstOrDefault(w => w.TranNo == TranNo);
                var resualt = DB.HREmpCertificates;
                if (BSM.Header != null)
                {
                    List<HREmpCertificate> listEmpfa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                    BSM.ListHeader = listEmpfa.ToList();
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
		public async Task<ActionResult> DownloadENG(string id)
        {
            ActionName = "Index";
            UserSession();
            //UserConfForm(SYActionBehavior.EDIT);
            await DataSelector();
            var BSM = new HREmpCertiObject();
            if (id == "null") id = null;
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpCertiObject)Session[Index_Sess_Obj + ActionName];
                    //BSM.Filter.InMonth = Temp;
                }
                string fileName = "";// Server.MapPath("~/Content/UPLOAD/Contract.docx");

                int TranNo = Convert.ToInt32(id);
                var certificate = DB.HREmpCertificates.FirstOrDefault(w => w.TranNo == TranNo);
                if (certificate != null)
                {
                    var CertType = DB.HRCertificationTypes.FirstOrDefault(w => w.Code == certificate.CertType);
                    if (CertType != null)
                    {
                        if (CertType.TemplatePath != "" && CertType.TemplatePath != null)
                        {
                            fileName = CertType.TemplatePath;
                        }
                    }
                    SYExecuteFindAndReplace Para = new SYExecuteFindAndReplace();
                    string TfileName = Server.MapPath(fileName);
                    var STAFFP = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == certificate.EmpCode);
                    string FileSource = Server.MapPath("~/Content/TEMPLATE/" + certificate.EmpCode + "Certificate.docx");

                    Para.ListObjectDictionary = new List<object>();
                    // var contract = DB.HREmpContracts.FirstOrDefault(w => w.TranNo == TranNo);
                    Para.ListObjectDictionary.Add(certificate);
                    Para.ListObjectDictionary.Add(STAFFP);


                    var msg = Para.ExecuteFindAndReplaceDOC(TfileName, FileSource, "EmpCert");
                    //var msg = Para.ExecuteFindAndReplaceDOCPDF(TfileName, FileSource, "EmpContract");

                    if (msg == SYConstant.OK)
                    {

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + certificate.EmpCode + "Certificate.docx");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.WriteFile(FileSource);
                        Response.End();

                    }
                    else
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    }
                    //return null;
                    //return RedirectToAction("Index");
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
		public async Task<ActionResult> DownloadKH(string id)
        {
            ActionName = "Index";
            UserSession();
            //UserConfForm(SYActionBehavior.EDIT);
            await DataSelector();
            var BSM = new HREmpCertiObject();
            if (id == "null") id = null;
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpCertiObject)Session[Index_Sess_Obj + ActionName];
                    //BSM.Filter.InMonth = Temp;
                }
                string fileName = "";// Server.MapPath("~/Content/UPLOAD/Contract.docx");

                int TranNo = Convert.ToInt32(id);
                var certificate = DB.HREmpCertificates.FirstOrDefault(w => w.TranNo == TranNo);
                if (certificate != null)
                {
                    var CertType = DB.HRCertificationTypes.FirstOrDefault(w => w.Code == certificate.CertType);
                    if (CertType != null)
                    {
                        if (CertType.TemplatePathKH != "" && CertType.TemplatePathKH != null)
                        {
                            fileName = CertType.TemplatePathKH;
                        }
                    }
                    SYExecuteFindAndReplace Para = new SYExecuteFindAndReplace();
                    string TfileName = Server.MapPath(fileName);
                    var STAFFP = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == certificate.EmpCode);
                    string FileSource = Server.MapPath("~/Content/TEMPLATE/" + certificate.EmpCode + "CertificateKH.docx");

                    Para.ListObjectDictionary = new List<object>();
                    // var contract = DB.HREmpContracts.FirstOrDefault(w => w.TranNo == TranNo);
                    Para.ListObjectDictionary.Add(certificate);
                    Para.ListObjectDictionary.Add(STAFFP);


                    var msg = Para.ExecuteFindAndReplaceDOC(TfileName, FileSource, "EmpCert");
                    //var msg = Para.ExecuteFindAndReplaceDOCPDF(TfileName, FileSource, "EmpContract");

                    if (msg == SYConstant.OK)
                    {

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + certificate.EmpCode + "CertificateKH.docx");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.WriteFile(FileSource);
                        Response.End();

                    }
                    else
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    }
                    //return null;
                    //return RedirectToAction("Index");
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #region 'Upload'
        [HttpPost]
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
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
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {
            ActionName = "Details";
            var Stafff = DBV.HR_STAFF_VIEW;
            HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == EmpCode);
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
                GetData(EmpCode, "Create");
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public string GetData(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            HREmpCertiObject BSM = new HREmpCertiObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var resualt = DB.HREmpCertificates;
                List<HREmpCertificate> listEmpFa = resualt.Where(x => x.EmpCode == EmpCode).ToList();
                BSM.ListHeader = listEmpFa.OrderByDescending(x => x.TranNo).ToList();
                //int tranNo = Convert.ToInt32(listEmpFa.Max(w => w.TranNo));
                // BSM.IsValidChecking(tranNo);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_Employee");
            }
        }
		private async Task DataSelector()
		{
            HRStaffProfileObject Staff = new HRStaffProfileObject();

            ViewData["STAFF_SELECT"] = await Staff.LoadData();
            ViewData["HREmpCertificate_LIST"] = DB.HRCertificationTypes.ToList();
        }
    }
}