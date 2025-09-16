using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.BS;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Leave
{

    public class ATOTRequestController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ATM0000013";
        private const string URL_SCREEN = "/Attendance/Attendance/ATOTRequest/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "OTRNo";
        private string DOCTYPE = "OTR01";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public ATOTRequestController()
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
            DataSelector();

            PROverTimeObject BSM = new PROverTimeObject();
            BSM.Filter = new Humica.Core.FT.FTINYear();
            BSM.Filter.INYear = DateTime.Now.Year;
            DateTime DNow = DateTime.Now;
            BSM.Filter.FromDate = new DateTime(DNow.Year, DNow.Month, 1);
            BSM.Filter.ToDate = new DateTime(DNow.Year, DNow.Month, DateTime.DaysInMonth(DNow.Year, DNow.Month));
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var Obj = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = Obj.Filter;
            }
            BSM.ListOTRequest = new List<HRRequestOT>();
            BSM.ListOTReqPending = new List<ClsReuestOT>();
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListStaff = DBV.HRStaffProfiles.ToList();
            ListStaff = ListStaff.Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();

            var ListOTRequest = DBV.HRRequestOTs.Where(w => EntityFunctions.TruncateTime(w.OTStartTime) <= BSM.Filter.ToDate.Date
                                                      && EntityFunctions.TruncateTime(w.OTStartTime) >= BSM.Filter.FromDate.Date).ToList();
            ListOTRequest = ListOTRequest.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).OrderByDescending(w => w.OTStartTime.Date).ToList();
            BSM.Filter.Status = "";
            BSM.ListOTRequest = ListOTRequest.ToList();
            BSM.ListOTReqPending = BSM.LoadOTPending(BSM.Filter);

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(PROverTimeObject collection)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            PROverTimeObject BSM = new PROverTimeObject();
            BSM.User = SYSession.getSessionUser();
            BSM.ListOTRequest = new List<HRRequestOT>();
            BSM.Filter = collection.Filter;
            var ListBranch = SYConstant.getBranchDataAccess();
            var staff = DB.HRStaffProfiles.ToList();
            var staffcheck = staff.ToList();
            staff = staff.Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();

            var ListOTRequest = DBV.HRRequestOTs.Where(w => EntityFunctions.TruncateTime(w.OTStartTime) <= BSM.Filter.ToDate.Date
                                                      && EntityFunctions.TruncateTime(w.OTStartTime) >= BSM.Filter.FromDate.Date).ToList();
            ListOTRequest = ListOTRequest.Where(w => staff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            string approved = SYDocumentStatus.APPROVED.ToString();
            string Reject = SYDocumentStatus.REJECTED.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();

            BSM.ListOTReqPending = BSM.LoadOTPending(BSM.Filter);

            if (BSM.Filter.Status != null)
            {
                if (BSM.Filter.Status == SYDocumentStatus.PENDING.ToString())
                {
                    ListOTRequest = ListOTRequest.Where(x => x.Status != approved && x.Status != Reject && x.Status != Cancel).ToList();
                }
                else
                {
                    ListOTRequest = ListOTRequest.Where(w => w.Status == BSM.Filter.Status).ToList();
                }

            }
            if (BSM.Filter.Department != null)
            {
                staffcheck = staffcheck.Where(w => w.DEPT == BSM.Filter.Department).ToList();
                ListOTRequest = ListOTRequest.Where(w => staffcheck.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            }
            if (BSM.Filter.Position != null)
            {
                staffcheck = staffcheck.Where(w => w.JobCode == BSM.Filter.Position).ToList();
                ListOTRequest = ListOTRequest.Where(w => staffcheck.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            }

            BSM.ListOTRequest = ListOTRequest.OrderByDescending(w => w.OTStartTime).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            PROverTimeObject BSM = new PROverTimeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListOTRequest);
        }
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfList(KeyName);
            PROverTimeObject BSM = new PROverTimeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListOTReqPending);
        }

        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PROverTimeObject BSM = new PROverTimeObject();
            BSM.HeaderOT = new HRRequestOT();
            BSM.HeaderOT.OTStartTime = DateTime.Now;
            BSM.HeaderOT.OTEndTime = DateTime.Now;
            BSM.HeaderOT.Hours = 0;
            BSM.HeaderOT.Status = SYDocumentStatus.PENDING.ToString();
            var staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == user.UserName);
            if (staff != null)
            {
                BSM.HeaderOT.RequestBy = staff.EmpCode;
            }
            else
                BSM.HeaderOT.EmpCode = user.UserName;
            BSM.ListOTRequest = new List<HRRequestOT>();
            BSM.User = SYSession.getSessionUser();
            BSM.DocType = DOCTYPE;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(string ID, PROverTimeObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            var BSM = new PROverTimeObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                BSM.HeaderOT = collection.HeaderOT;
                BSM.DocType = DOCTYPE;
            }
            if (!string.IsNullOrEmpty(BSM.HeaderOT.EmpCode))
            {
                string msg = BSM.CreateOTReq();

                if (msg == SYConstant.OK)
                {
                    BSM.ScreenId = SCREEN_ID;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.HeaderOT.OTRNo.ToString();
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + BSM.HeaderOT.OTRNo;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion

        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            PROverTimeObject BSM = new PROverTimeObject();
            BSM.HeaderOT = new HRRequestOT();
            if (!string.IsNullOrEmpty(id))
            {
                BSM.HeaderOT = DB.HRRequestOTs.FirstOrDefault(x => x.OTRNo == id);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, PROverTimeObject collection)
        {
            ActionName = "Edit";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            PROverTimeObject BSM = new PROverTimeObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                }
                if (Session[PATH_FILE] != null)
                {
                    //collection.HeaderOT.AttachFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                else
                {
                    //collection.HeaderOT.AttachFile = BSM.HeaderOT.AttachFile;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                    BSM.HeaderOT = collection.HeaderOT;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditOTReq(id);
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
        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ViewData[SYSConstant.PARAM_ID] = id;
            PROverTimeObject BSM = new PROverTimeObject();
            BSM.HeaderOT = DB.HRRequestOTs.FirstOrDefault(x => x.OTRNo == id);
            if (BSM.HeaderOT != null)
            {
                BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == "REQ_OT").ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
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
                PROverTimeObject BSM = new PROverTimeObject();
                string msg = BSM.DeleteOT(id);
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
        #region "Approve"
        public ActionResult Approve(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            PROverTimeObject BSM = new PROverTimeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }

            if (!string.IsNullOrEmpty(id))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.ApproveOTReq(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.Description = mess.Description + ". " + BSM.MessageError;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        #region "Reject"
        public ActionResult Reject(string id)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (!string.IsNullOrEmpty(id))
            {
                PROverTimeObject BSM = new PROverTimeObject();
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                }

                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.RejectOTReq(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
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
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);

        }
        #endregion

        #region "Import & Upload"
        public ActionResult GridItemsImport()
        {
            ActionName = "Import";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PROverTimeObject BSM = new PROverTimeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsImport", BSM);
        }
        public ActionResult Import()
        {
            UserSession();
            ActionName = "Import";
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "ATOTRequest", SYSConstant.DEFAULT_UPLOAD_LIST);

            var BSM = new PROverTimeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }

            BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();

            if (BSM.ListTemplate.Count > 0)
            {
                SYExcel excel = new SYExcel();
                foreach (var read in BSM.ListTemplate.ToList())
                {
                    excel.FileName = read.UpoadPath;
                }
                DataTable dtHeader = excel.GenerateExcelData();
                BSM.ListOTRequest = new List<HRRequestOT>();

                if (dtHeader != null)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        var objHeader = new HRRequestOT();
                        objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                        objHeader.AllName = dtHeader.Rows[i][1].ToString();
                        objHeader.OTStartTime = SYSettings.getDateValue(dtHeader.Rows[i][2].ToString());
                        objHeader.OTEndTime = SYSettings.getDateValue(dtHeader.Rows[i][3].ToString());
                        objHeader.Hours = SYSettings.getNumberValue(dtHeader.Rows[i][4].ToString());
                        objHeader.BreakTime = SYSettings.getNumberValue(dtHeader.Rows[i][5].ToString());
                        objHeader.Reason = dtHeader.Rows[i][6].ToString();

                        BSM.ListOTRequest.Add(objHeader);
                    }
                }

            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "ATOTRequest", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("ATOTRequest"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "PAYROLL";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
                sfi.ValidationSettings,
                sfi.uc_FileUploadComplete);

            PROverTimeObject BSM = new PROverTimeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            BSM.ListOTRequest = new List<HRRequestOT>();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
        }
        public ActionResult UploadList()
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "ATOTRequest", SYSConstant.DEFAULT_UPLOAD_LIST);

            var BSM = new PROverTimeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }

            BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            BSM.ListOTRequest = new List<HRRequestOT>();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView(SYListConfuration.ListDefaultUpload, BSM.ListTemplate);
        }
        [HttpGet]
        public ActionResult GenerateUpload(int id)
        {
            UserSession();
            MDUploadTemplate obj = DB.MDUploadTemplates.Find(id);
            HumicaDBContext DBB = new HumicaDBContext();
            if (obj != null)
            {
                SYExcel excel = new SYExcel();
                excel.FileName = obj.UpoadPath;
                DataTable dtHeader = excel.GenerateExcelData();
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
                        PROverTimeObject BSM = new PROverTimeObject();
                        BSDocConfg DocBatch = new BSDocConfg("BATCH_UPLOAD", DocConfType.Normal, true);

                        string msg = SYConstant.OK;

                        DateTime create = DateTime.Now;
                        if (dtHeader.Rows.Count > 0)
                        {
                            BSM.ListOTRequest = new List<HRRequestOT>();

                            for (int i = 0; i < dtHeader.Rows.Count; i++)
                            {

                                var objHeader = new HRRequestOT();
                                objHeader.OTRNo = dtHeader.Rows[i][0].ToString();
                                objHeader.EmpCode = dtHeader.Rows[i][1].ToString();
                                objHeader.AllName = dtHeader.Rows[i][2].ToString();
                                objHeader.OTStartTime = SYSettings.getDateValue(dtHeader.Rows[i][3].ToString());
                                objHeader.OTEndTime = SYSettings.getDateValue(dtHeader.Rows[i][4].ToString());
                                objHeader.Hours = SYSettings.getNumberValue(dtHeader.Rows[i][5].ToString());
                                objHeader.BreakTime = SYSettings.getNumberValue(dtHeader.Rows[i][6].ToString());
                                objHeader.Reason = dtHeader.Rows[i][7].ToString();

                                BSM.ListOTRequest.Add(objHeader);
                            }

                            msg = BSM.ImportAOTRequest();
                            if (msg != SYConstant.OK)
                            {
                                obj.Message = SYMessages.getMessage(msg);
                                obj.Message += ":" + BSM.MessageError;
                                obj.IsGenerate = false;
                                DB.MDUploadTemplates.Attach(obj);
                                DB.Entry(obj).Property(w => w.Message).IsModified = true;
                                DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                DB.SaveChanges();
                                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                            }
                            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);
                            obj.DocumentNo = DocBatch.NextNumberRank;
                            obj.IsGenerate = true;
                            DBB.MDUploadTemplates.Attach(obj);
                            DBB.Entry(obj).Property(w => w.Message).IsModified = true;
                            DBB.Entry(obj).Property(w => w.DocumentNo).IsModified = true;
                            DBB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                            DBB.SaveChanges();
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
            SYSGridSettingExport gSetting = new SYSGridSettingExport();
            gSetting.GridName = "DownloadTemplate" + SCREEN_ID; List<ExCFUploadMapping> _List = new List<ExCFUploadMapping>();
            gSetting.CallBackControllerName = ""; 
            _List.Add(new ExCFUploadMapping { Caption = "OTRNo", FieldName = "OTRNo" });
            gSetting.CallBackActionName = ""; 
            _List.Add(new ExCFUploadMapping { Caption = "Employee Code", FieldName = "Employee Code" });
            GridViewExportFormat format = GridViewExportFormat.Xlsx; 
            _List.Add(new ExCFUploadMapping { Caption = "OT StartTime", FieldName = "OT StartTime" });
            _List.Add(new ExCFUploadMapping { Caption = "OT EndTime", FieldName = "OT EndTime" });
            _List.Add(new ExCFUploadMapping { Caption = "OTHour", FieldName = "OTHour" });
            _List.Add(new ExCFUploadMapping { Caption = "BreakTime", FieldName = "BreakTime" });
            _List.Add(new ExCFUploadMapping { Caption = "Reason", FieldName = "Reason" });

            if (SYExportFile.ExportFormatsInfo.ContainsKey(format))
            {
                GridViewSettings settingAdd = ClsConstant.CreateExportGridViewSettings(gSetting, _List, "ATOTRequest_TEMPLATE");
                SYExportFile.ExportFormatsInfo[format](settingAdd, null);
                return SYExportFile.ExportFormatsInfo[format](settingAdd, null);
            }

            return null;
        }

        #endregion

        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            //ActionName = "Details";
            this.UserSession();
            DataSelector();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID] = id;
            PROverTimeObject BSD = new PROverTimeObject();
            if (id.ToString() != "null")
            {
                string sms = BSD.CancelOTReq(id);
                if (sms == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(sms, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
        }
        #endregion

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
                    LevelCode = EmpStaff.Level,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult GridApprovalDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            PROverTimeObject BSM = new PROverTimeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        [HttpPost]
        public ActionResult ShowData(DateTime FromDate, DateTime ToDate, decimal BreakTime)
        {

            ActionName = "Create";

            decimal Hour = Math.Round(Convert.ToDecimal(ToDate.Subtract(FromDate).TotalHours), 2) - (BreakTime / 60);
            var result = new
            {
                MS = SYConstant.OK,
                Hour = Hour
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListStaff = DBV.HR_STAFF_VIEW.ToList();
            ListStaff = ListStaff.Where(w => ListBranch.Where(x => x.Code == w.BranchID).Any()).ToList();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            var objList = new SYDataList("STATUS_LEAVE_APPROVAL");
            ViewData["STATUS_APPROVAL"] = objList.ListData;
            ViewData["STAFF_SELECT"] = ListStaff.ToList();
            var BS = new PROverTimeObject();
            foreach (var data in BS.OnDataSelectorPolicy())
            {
                ViewData[data.Key] = data.Value;
            }

        }
    }
}