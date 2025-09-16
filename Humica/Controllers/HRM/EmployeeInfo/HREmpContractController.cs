using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.XtraReports.UI;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.Report;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.EmployeeInfo
{
    public class HREmpContractController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRE0000006";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HREmpContract/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public HREmpContractController()
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
            HREmpContObject BSM = new HREmpContObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (HREmpContObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListContract =await BSM.LoadData();
            Session[Index_Sess_Obj + ActionName] = BSM;
            ViewData["IsVisible"] = false;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HREmpContObject BSM = new HREmpContObject();
            BSM.ListStaffView = new List<HR_STAFF_VIEW>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpContObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListContract);
        }
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            HREmpContObject BSM = new HREmpContObject();
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpContObject)Session[Index_Sess_Obj + ActionName];
            }
            //BSM.ListHeader = new List<HR_Family_View>();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM);
        }
        #endregion
        #region Create
        public async Task<ActionResult> Create()
        {
            ActionName = "Create";
            UserSession();
			await DataSelector();
            UserConfListAndForm(this.KeyName);
            HREmpContObject BSM = new HREmpContObject();
            BSM.ListHeader = new List<HREmpContract>();
            BSM.Header = new HREmpContract();
			//BSM.Header.FromDate = DateTime.Now;
			//BSM.Header.ToDate = DateTime.Now;
			BSM.HeaderStaff = new HR_STAFF_VIEW();
            Session[Index_Sess_Obj + ActionName] = BSM;
            ViewData["IsVisible"] = false;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(HREmpContObject collection)
        {
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ActionName = "Create";
            HREmpContObject BSM = new HREmpContObject();
            if (ModelState.IsValid)
            {
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.ContractPath = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpContObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.HeaderStaff = collection.HeaderStaff;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateEmpCon();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                    mess_err.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess_err.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess_err.DocumentNumber;
                    BSM.Header = new HREmpContract();
                    //BSM.Header.FromDate = DateTime.Now;
                    //BSM.Header.ToDate = DateTime.Now;
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
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            ViewData[SYSConstant.PARAM_ID] = "UDC";
            if (ID == "null") ID = null;
            if (ID != null)
            {
                HREmpContObject BSM = new HREmpContObject();
                BSM.Header = new HREmpContract();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(ID);
                BSM.Header = DB.HREmpContracts.FirstOrDefault(w => w.TranNo == TranNo);
                var resualt = DB.HREmpContracts;
                if (BSM.Header != null)
                {
                    List<HREmpContract> listEmpfa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                    BSM.ListHeader = listEmpfa.ToList();
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    ViewData[SYSConstant.PARAM_ID] = "UDC";
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, HREmpContObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            HREmpContObject BSM = new HREmpContObject();

            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpContObject)Session[Index_Sess_Obj + ActionName];
                }
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.ContractPath = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                else
                {
                    collection.Header.ContractPath = BSM.Header.ContractPath;
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                int TranNo = Convert.ToInt32(id);
                string msg = BSM.EditEmpCon(TranNo);
                if (msg == SYConstant.OK)
                {
                    DB = new HumicaDBContext();
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    ViewData[SYSConstant.PARAM_ID2] = "UDC";
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
                HREmpContObject Del = new HREmpContObject();
                string msg = Del.DeleteEmpCon(TranNo);
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
        #endregion
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
            var SD = DB.HREmpContracts.FirstOrDefault(w => w.TranNo == TranNo);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    string Company = SMS.SYHRCompanies.FirstOrDefault().CompENG;
                    XtraReport reportModel = new XtraReport();
                    if (SD.ConType == "CE")
                    {
                        reportModel = new RptEmployeeContract();

                    }
                    else if (SD.ConType == "LE")
                    {
                        reportModel = new RptEmpContractLetter();
                    }
                    else if (SD.ConType == "CP")
                    {
                        reportModel = new RptEmpContractProbation();
                    }

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
            var SD = DB.HREmpContracts.FirstOrDefault(w => w.TranNo == TranNo);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            if (SD != null)
            {
                XtraReport reportModel = new XtraReport();
                if (SD.ConType == "CE")
                {
                    reportModel = (RptEmployeeContract)Session[Index_Sess_Obj + ActionName];
                }
                else if (SD.ConType == "LE")
                {
                    reportModel = (RptEmpContractLetter)Session[Index_Sess_Obj + ActionName];
                }
                else if (SD.ConType == "CP")
                {
                    reportModel = (RptEmpContractProbation)Session[Index_Sess_Obj + ActionName];

                }
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion
        #region Import
        public ActionResult Import()
        {

            UserSession();
            ActionName = "Import";
            UserConfListAndForm(this.KeyName);
            var obj = new HREmpContObject();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HREmpContract", SYSConstant.DEFAULT_UPLOAD_LIST);
            obj.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            Session[Index_Sess_Obj + ActionName] = obj;
            return View(obj);

        }

        [HttpPost]
        public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HREmpContract", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("FAMILY"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "CONTRACT";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
              sfi.ValidationSettings,
              sfi.uc_FileUploadComplete);
            return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
        }

        public ActionResult UploadList()
        {
            UserSession();
            ActionName = "Import";
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HREmpContract", SYSConstant.DEFAULT_UPLOAD_LIST);
            IEnumerable<MDUploadTemplate> listu = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate);
            return PartialView(SYListConfuration.ListDefaultUpload, listu.ToList());
        }

        [HttpGet]
        public ActionResult GenerateUpload(int id)
        {
            UserSession();
            ActionName = "Import";
            MDUploadTemplate obj = DB.MDUploadTemplates.Find(id);
            if (obj != null)
            {
                var DBB = new HumicaDBContext();
                SYExcel excel = new SYExcel();
                excel.FileName = obj.UpoadPath;
                DataTable dtHeader = excel.GenerateExcelData();
                var objStaff = new HREmpContObject();
                if (obj.IsGenerate == true)
                {
                    SYMessages mess = SYMessages.getMessageObject("FILE_RG", user.Lang);
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
                }
                if (dtHeader != null)
                {
                    try
                    {
                        string msg = SYConstant.OK;

                        DateTime create = DateTime.Now;
                        if (dtHeader.Rows.Count > 0)
                        {
                            objStaff.ListHeader = new List<HREmpContract>();
                            for (int i = 0; i < dtHeader.Rows.Count; i++)
                            {
                                var objHeader = new HREmpContract();
                                objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                                objHeader.ConType = dtHeader.Rows[i][2].ToString();
                                objHeader.FromDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][3].ToString());
                                objHeader.ToDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][4].ToString());
                                objHeader.Description = dtHeader.Rows[i][5].ToString();
                                objStaff.ListHeader.Add(objHeader);
                            }

                            msg = objStaff.upload();
                            if (msg == SYConstant.OK)
                            {

                                obj.Message = SYConstant.OK;
                                //obj.DocumentNo = DocBatch.NextNumberRank;
                                SYMessages mess = SYMessages.getMessageObject("IMPORTED", user.Lang);
                                Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                                obj.IsGenerate = true;
                                DBB.MDUploadTemplates.Attach(obj);
                                DBB.Entry(obj).Property(w => w.Message).IsModified = true;
                                DBB.Entry(obj).Property(w => w.DocumentNo).IsModified = true;
                                DBB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                DBB.SaveChanges();

                            }
                            else
                            {
                                obj.Message = SYMessages.getMessage(msg);
                                obj.Message += ":" + objStaff.MessageError;
                                obj.IsGenerate = false;
                                DB.MDUploadTemplates.Attach(obj);
                                DB.Entry(obj).Property(w => w.Message).IsModified = true;
                                DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                DB.SaveChanges();
                                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessage(msg, user.Lang) + "(" + objStaff.MessageError + ")";
                                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                            }


                        }
                    }
                    catch (DbUpdateException e)
                    {
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserID.ToString();
                        log.DocurmentAction = "UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e, true);
                        /*----------------------------------------------------------*/
                        obj.Message = e.Message;
                        obj.IsGenerate = false;
                        DB.MDUploadTemplates.Attach(obj);
                        DB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DB.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserID.ToString();
                        log.DocurmentAction = "UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e, true);
                        obj.Message = e.Message;
                        obj.IsGenerate = false;
                        DB.MDUploadTemplates.Attach(obj);
                        DB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DB.SaveChanges();
                    }
                }

            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
        }

        public ActionResult DownloadTemplate()
        {
            string fileName = Server.MapPath("~/Content/TEMPLATE/TEMPLATE_CONTRACT.xlsx");
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=TEMPLATE_CONTRACT.xlsx");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.WriteFile(fileName);
            Response.End();
            return null;
        }

        #endregion
        public ActionResult DownloadENG(string id)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new HREmpContObject();
            if (id == "null") id = null;
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpContObject)Session[Index_Sess_Obj + ActionName];
                    //BSM.Filter.InMonth = Temp;
                }
                string fileName = "";// Server.MapPath("~/Content/UPLOAD/Contract.docx");
                int TranNo = Convert.ToInt32(id);
                var contract = DB.HREmpContracts.FirstOrDefault(w => w.TranNo == TranNo);
                if (contract != null)
                {
                    var ConType = DB.HRContractTypes.FirstOrDefault(w => w.Code == contract.ConType);
                    if (ConType != null)
                    {
                        if (ConType.TemplatePath != "" && ConType.TemplatePath != null)
                        {
                            fileName = ConType.TemplatePath;
                        }
                    }
                    SYExecuteFindAndReplace Para = new SYExecuteFindAndReplace();
                    string TfileName = Server.MapPath(fileName);
                    var STAFFP = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == contract.EmpCode);

                    var STAFF_V = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == contract.EmpCode);
                    var HREmpCareer = DB.HREmpCareers.FirstOrDefault(w => w.EmpCode == contract.EmpCode && w.CareerCode == "INCREASE");
                    var POS = DB.HRPositions.FirstOrDefault(w => w.Code == STAFFP.JobCode);
                    var NATION = DB.HRNations.FirstOrDefault(w => w.Code == STAFFP.Nation);
                    var Division = DB.HRDivisions.FirstOrDefault(w => w.Code == STAFFP.Division);
                    var Company = SMS.SYHRCompanies.FirstOrDefault();

                    var _shift = DB.ATShifts.Where(w => w.Code == STAFFP.ROSTER).FirstOrDefault();
                    if (POS.SecDescription == null)
                    {
                        POS.SecDescription = POS.Description;
                    }
                    string FileSource = Server.MapPath("~/Content/TEMPLATE/" + contract.EmpCode + "Contract.docx");
                    Para.ListObjectDictionary = new List<object>();

                    Para.ListObjectDictionary.Add(POS);
                    Para.ListObjectDictionary.Add(Company);

                    if (_shift != null)
                        Para.ListObjectDictionary.Add(_shift);
                    if (NATION != null)
                    {
                        Para.ListObjectDictionary.Add(NATION);
                    }
                    if (STAFF_V.Sex == "M") STAFF_V.Sex = "Male";
                    else STAFF_V.Sex = "Female";
                    if (STAFFP.ProbationType == "PP003") STAFFP.ProbationType = "៣ខែ";
                    if (STAFFP.ProbationType == "PP004") STAFFP.ProbationType = "៤ខែ";
                    if (STAFFP.ProbationType == "PP005") STAFFP.ProbationType = "៥ខែ";
                    if (STAFFP.ProbationType == "PP006") STAFFP.ProbationType = "៦ខែ";
                    Para.ListObjectDictionary.Add(contract);
                    Para.ListObjectDictionary.Add(STAFFP);
                    Para.ListObjectDictionary.Add(STAFF_V);
                    if (HREmpCareer != null)
                    {
                        Para.ListObjectDictionary.Add(HREmpCareer);
                    }
                    else HREmpCareer.NewSalary = 0;
                    var msg = Para.ExecuteFindAndReplaceDOC(TfileName, FileSource, "EmpContract");

                    if (msg == SYConstant.OK)
                    {
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + contract.EmpCode + "Contract.docx");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.WriteFile(FileSource);
                        Response.End();
                    }
                    else
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    }
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        public ActionResult Details(string id)
        {
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id == "null") id = null;
            if (id != null)
            {
                HREmpContObject BSM = new HREmpContObject();
                BSM.Header = new HREmpContract();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.HREmpContracts.FirstOrDefault(w => w.TranNo == TranNo);
                var resualt = DB.HREmpContracts;
                if (BSM.Header != null)
                {
                    List<HREmpContract> listEmpfa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                    BSM.ListHeader = listEmpfa.ToList();
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {

            ActionName = "Details";
            var EmpStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == EmpCode);
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

            HREmpContObject BSM = new HREmpContObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var resualt = DB.HREmpContracts;
                List<HREmpContract> listEmpFa = resualt.Where(x => x.EmpCode == EmpCode).ToList();
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
            //var ListBranch = SYConstant.getBranchDataAccess();
            //var ListStaff = DB.HRStaffProfiles;
            //var hRStaffProfiles = ListStaff.ToArray();
            //var listStaff = hRStaffProfiles.Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            //ListStaff = ListStaff.Where(w => ListBranch.Where(x => x.Code == w.BranchID).Any()).ToList();

            List<SYData> list = new List<SYData>();
            SYDataList objList = new SYDataList("CONTRACT_TERM");
            list = (List<SYData>)objList.ListData;
            list.Add(new SYData());
            ViewData["CONTRACT_TERM"] = list;
			HRStaffProfileObject Staff = new HRStaffProfileObject();
			ViewData["STAFF_SELECT"] = await Staff.LoadData(); 
			ViewData["CONTRACT_TYPE_LIST"] = DB.HRContractTypes.ToList();
        }
        [HttpPost]
		public ActionResult ContractTerm(string code)
		{

			List<HRContractType> contractTypes = new List<HRContractType>();
			DB.HRContractTypes.ToList().ForEach(x => contractTypes.Add(x));
			if (contractTypes.Count > 0)
			{
				string conTerm = string.Empty;
				bool isVisible = true;
				HRContractType contractType = new HRContractType();
				contractType = contractTypes.Find(x => x.Code == code);
				if (contractType == null)
				{
					conTerm = code;
					isVisible = code == "UDC" ? true : false;
				}
				else
				{
					conTerm = contractType.IsUDC == true ? "UDC" : "FDC";
					isVisible = this.IsNullOrFalse(contractType.IsUDC);
				}
				var result = new
				{
					MS = SYConstant.OK,
					ConTerm = conTerm,
					IsVisible = isVisible
				};

				return Json(result, JsonRequestBehavior.DenyGet);
			}
			else
			{
				var rs = new { MS = SYConstant.FAIL };
				return Json(rs, JsonRequestBehavior.DenyGet);

			}
		}
		private bool IsNullOrFalse(bool? value)
        {
            return value ?? false;
        }
    }
}