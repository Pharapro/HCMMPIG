using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.Report.HRM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.EmployeeInfo
{
    public class HREmpDisciplinaryController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRE0000008";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HREmpDisciplinary/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public HREmpDisciplinaryController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HREmpDisciplinaryObject BSM = new HREmpDisciplinaryObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (HREmpDisciplinaryObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = BSM.LoadData();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HREmpDisciplinaryObject BSM = new HREmpDisciplinaryObject();
            BSM.ListStaffView = new List<HR_STAFF_VIEW>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpDisciplinaryObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            HREmpDisciplinaryObject BSM = new HREmpDisciplinaryObject();
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpDisciplinaryObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            HREmpDisciplinaryObject BSM = new HREmpDisciplinaryObject();
            BSM.Lisgrd = new List<HREmpDisciplinary>();
            BSM.Header = new HREmpDisciplinary();
            BSM.Header.TranDate = DateTime.Now;
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(HREmpDisciplinaryObject collection)
        {
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ActionName = "Create";
            HREmpDisciplinaryObject BSM = new HREmpDisciplinaryObject();
            if (ModelState.IsValid)
            {
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.AttachPath = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpDisciplinaryObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.HeaderStaff = collection.HeaderStaff;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateEmpDiscp();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                    mess_err.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess_err.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess_err.DocumentNumber;
                    BSM.Header = new HREmpDisciplinary();
                    BSM.Header.TranDate = DateTime.Now;
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
        #region "Edit"
        public ActionResult Edit(string ID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            if (ID == "null") ID = null;
            if (ID != null)
            {
                HREmpDisciplinaryObject BSM = new HREmpDisciplinaryObject();
                BSM.Header = new HREmpDisciplinary();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(ID);
                BSM.Header = DB.HREmpDisciplinaries.FirstOrDefault(w => w.TranNo == TranNo);
                var resualt = DB.HREmpDisciplinaries;
                if (BSM.Header != null)
                {

                    List<HREmpDisciplinary> listEmpfa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                    BSM.Lisgrd = listEmpfa.ToList();
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, HREmpDisciplinaryObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            HREmpDisciplinaryObject BSM = new HREmpDisciplinaryObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpDisciplinaryObject)Session[Index_Sess_Obj + ActionName];
                }
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.AttachPath = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                else
                {
                    collection.Header.AttachPath = BSM.Header.AttachPath;
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                int TranNo = Convert.ToInt32(id);
                string msg = BSM.EditEmpDiscp(TranNo);
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
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id == "null") id = null;
            if (id != null)
            {
                int TranNo = Convert.ToInt32(id);
                HREmpDisciplinaryObject Del = new HREmpDisciplinaryObject();
                string msg = Del.DeleteEmpDiscp(TranNo);
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
        #endregion 'Upload'
        #region "Print"
        public ActionResult Print(string id)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string id)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            //UserMVC();
            int TranNo = Convert.ToInt32(id);
            var SD = DB.HREmpDisciplinaries.FirstOrDefault(w => w.TranNo == TranNo);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    rptDisciplineLetter reportModel = new rptDisciplineLetter();

                    reportModel.Parameters["TranNo"].Value = SD.TranNo;
                    reportModel.Parameters["TranNo"].Visible = false;

                    Session[Index_Sess_Obj + ActionName] = reportModel;

                    return PartialView("PrintForm", reportModel);
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    log.DocurmentAction = id;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string id)
        {
            ActionName = "Print";
            int TranNo = Convert.ToInt32(id);
            var SD = DB.HREmpDisciplinaries.FirstOrDefault(w => w.TranNo == TranNo);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            if (SD != null)
            {
                rptDisciplineLetter reportModel = new rptDisciplineLetter();

                reportModel = (rptDisciplineLetter)Session[Index_Sess_Obj + ActionName];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }


        #endregion
        #region "Details"
        public ActionResult Details(string id)
        {
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id == "null") id = null;
            if (id != null)
            {
                HREmpDisciplinaryObject BSM = new HREmpDisciplinaryObject();
                BSM.Header = new HREmpDisciplinary();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.HREmpDisciplinaries.FirstOrDefault(w => w.TranNo == TranNo);
                var resualt = DB.HREmpDisciplinaries;
                if (BSM.Header != null)
                {

                    List<HREmpDisciplinary> listEmpfa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                    BSM.Lisgrd = listEmpfa.ToList();
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        public ActionResult DownloadENG(string id)
        {
            ActionName = "Index";
            UserSession();
            //UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new HREmpDisciplinaryObject();
            if (id == "null") id = null;
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpDisciplinaryObject)Session[Index_Sess_Obj + ActionName];
                    //BSM.Filter.InMonth = Temp;
                }
                string fileName = "";// Server.MapPath("~/Content/UPLOAD/Contract.docx");

                int TranNo = Convert.ToInt32(id);
                var disciplinary = DB.HREmpDisciplinaries.FirstOrDefault(w => w.TranNo == TranNo);
                if (disciplinary != null)
                {
                    var DisType = DB.HRDisciplinActions.FirstOrDefault(w => w.Code == disciplinary.DiscAction);
                    if (DisType != null)
                    {
                        if (DisType.TemplatePath != "" && DisType.TemplatePath != null)
                        {
                            fileName = DisType.TemplatePath;
                        }
                    }

                    SYExecuteFindAndReplace Para = new SYExecuteFindAndReplace();
                    string TfileName = Server.MapPath(fileName);
                    var STAFFP = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == disciplinary.EmpCode);


                    string FileSource = Server.MapPath("~/Content/TEMPLATE/" + disciplinary.EmpCode + "Disciplinary.docx");

                    Para.ListObjectDictionary = new List<object>();
                    // var contract = DB.HREmpContracts.FirstOrDefault(w => w.TranNo == TranNo);
                    Para.ListObjectDictionary.Add(disciplinary);
                    Para.ListObjectDictionary.Add(STAFFP);

                    //Para.ListObjectDictionary.Add(STAFF);


                    var msg = Para.ExecuteFindAndReplaceDOC(TfileName, FileSource, "EmpDisc");
                    //var msg = Para.ExecuteFindAndReplaceDOCPDF(TfileName, FileSource, "EmpContract");

                    if (msg == SYConstant.OK)
                    {

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + disciplinary.EmpCode + "Disciplinary.docx");
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
        public ActionResult DownloadKH(string id)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new HREmpDisciplinaryObject();
            if (id == "null") id = null;
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpDisciplinaryObject)Session[Index_Sess_Obj + ActionName];
                    //BSM.Filter.InMonth = Temp;
                }
                string fileName = "";// Server.MapPath("~/Content/UPLOAD/Contract.docx");

                int TranNo = Convert.ToInt32(id);
                var disciplinary = DB.HREmpDisciplinaries.FirstOrDefault(w => w.TranNo == TranNo);
                if (disciplinary != null)
                {
                    var DisType = DB.HRDisciplinActions.FirstOrDefault(w => w.Code == disciplinary.DiscAction);
                    if (DisType != null)
                    {
                        if (DisType.TemplatePathKH != "" && DisType.TemplatePathKH != null)
                        {
                            fileName = DisType.TemplatePathKH;
                        }
                    }
                    SYExecuteFindAndReplace Para = new SYExecuteFindAndReplace();
                    string TfileName = Server.MapPath(fileName);
                    var STAFFP = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == disciplinary.EmpCode);
                    var STAFF_V = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == disciplinary.EmpCode);
                    string FileSource = Server.MapPath("~/Content/TEMPLATE/" + disciplinary.EmpCode + "DisciplinaryKH.docx");
                    Para.ListObjectDictionary = new List<object>();
                    if (disciplinary != null)
                    {
                        disciplinary.DiscAction = DisType.SecDescription;
                        Para.ListObjectDictionary.Add(disciplinary);
                    }
                    Para.ListObjectDictionary.Add(STAFFP);
                    Para.ListObjectDictionary.Add(STAFF_V);
                    var msg = Para.ExecuteFindAndReplaceDOC(TfileName, FileSource, "EmpDisc");

                    if (msg == SYConstant.OK)
                    {

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + disciplinary.EmpCode + "DisciplinaryKH.docx");
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
        #region "Private Code"
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

            HREmpDisciplinaryObject BSM = new HREmpDisciplinaryObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var resualt = DB.HREmpDisciplinaries;
                List<HREmpDisciplinary> listEmpFa = resualt.Where(x => x.EmpCode == EmpCode).ToList();
                BSM.Lisgrd = listEmpFa.OrderByDescending(x => x.TranNo).ToList();
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
        private void DataSelector()
        {
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListStaff = DBV.HR_STAFF_VIEW.ToList();
            ListStaff = ListStaff.Where(w => ListBranch.Where(x => x.Code == w.BranchID).Any()).ToList();

            ViewData["STAFF_SELECT"] = ListStaff;
            ViewData["DISCIPLINAY_LIST"] = DB.HRDisciplinTypes.ToList();
            ViewData["DISCIPLINACTION_SELECT"] = DB.HRDisciplinActions.ToList();
        }
        #endregion
    }
}