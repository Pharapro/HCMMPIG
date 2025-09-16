using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core;
using Humica.Core.BS;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Controllers.PR.PRM
{
    public class PROverTimeController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000001";
        private const string URL_SCREEN = "/PR/PRM/PROverTime/";
        private string KeyName = "TranNo";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public PROverTimeController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        [HttpPost]
        public ActionResult GetData(string FromTime, string ToTime)
        {
            string message = "";
            double total = 0;
            if (String.IsNullOrEmpty(FromTime) || (String.IsNullOrEmpty(ToTime)))
            {
                message = "";
                total = 0;
            }
            else
            {
                int hours = 0;
                int minutes = 0;
                DateTime toDate = DateTime.Today;
                if (!String.IsNullOrEmpty(FromTime))
                {
                    hours = Convert.ToInt32(FromTime.Substring(0, 2));
                    minutes = Convert.ToInt32(FromTime.Substring(3, 2));
                }
                DateTime fromTime = new DateTime(toDate.Year, toDate.Month, toDate.Day, hours, minutes, 0);
                if (!String.IsNullOrEmpty(ToTime))
                {
                    hours = Convert.ToInt32(ToTime.Substring(0, 2));
                    minutes = Convert.ToInt32(ToTime.Substring(3, 2));
                }
                DateTime toTime = new DateTime(toDate.Year, toDate.Month, toDate.Day, hours, minutes, 0);
                var pDate = fromTime.ToString("tt", CultureInfo.InvariantCulture);
                var cDate = toTime.ToString("tt", CultureInfo.InvariantCulture);
                bool sliptDate = false;
                if (pDate == "PM" && cDate == "AM") sliptDate = true;
                if (sliptDate)
                    toTime = toTime.AddDays(1);
                TimeSpan ts = toTime - fromTime;
                message = SYConstant.OK;

                total = Math.Round(ts.TotalHours, 2);
            }

            var result = new
            {
                MS = message,//SYConstant.OK,
                totalHour1 = total//10
            };
            return Json(result, JsonRequestBehavior.AllowGet);
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
            DateTime DNow = DateTime.Now;
            BSM.Filter.FromDate = new DateTime(DNow.Year, DNow.Month, 1);
            BSM.Filter.ToDate = new DateTime(DNow.Year, DNow.Month, DateTime.DaysInMonth(DNow.Year, DNow.Month));
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = obj.Filter;
            }
            BSM.ListHeader = new List<PREmpOverTime>();
            var ListHeader = DB.PREmpOverTimes.Where(w => EntityFunctions.TruncateTime(w.OTDate) >= BSM.Filter.FromDate.Date
            && EntityFunctions.TruncateTime(w.OTDate) <= BSM.Filter.ToDate).ToList();
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();
            ListHeader = ListHeader.Where(x => Staff.Where(w => w.EmpCode == x.EmpCode).Any()).ToList();
            BSM.ListHeader = ListHeader.OrderByDescending(w => w.OTDate).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(PROverTimeObject BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            var ListHeader = DB.PREmpOverTimes.Where(w => EntityFunctions.TruncateTime(w.OTDate) >= BSM.Filter.FromDate.Date
            && EntityFunctions.TruncateTime(w.OTDate) <= BSM.Filter.ToDate).ToList();
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();
            ListHeader = ListHeader.Where(x => Staff.Where(w => w.EmpCode == x.EmpCode).Any()).ToList();

            BSM.ListHeader = ListHeader.OrderByDescending(w => w.OTDate).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PROverTimeObject BSM = new PROverTimeObject();
            BSM.ListHeader = new List<PREmpOverTime>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            DataSelector();
            UserSession();
            PROverTimeObject BSM = new PROverTimeObject();
            UserConfListAndForm(this.KeyName);
            BSM.Header = new PREmpOverTime();
            BSM.ListHeader = new List<PREmpOverTime>();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(PROverTimeObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PROverTimeObject BSM = new PROverTimeObject();
            BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            BSM.ScreenId = SCREEN_ID;
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                    BSM.HeaderStaff = collection.HeaderStaff;
                }
                string msg = BSM.CreateOT();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.HeaderStaff.EmpCode.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM.HeaderStaff = new HR_STAFF_VIEW();
                    BSM.ListHeader = new List<PREmpOverTime>();
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
                return View(BSM);
            }
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
                PROverTimeObject BSM = new PROverTimeObject();
                BSM.ListHeader = new List<PREmpOverTime>();
                BSM.Header = new PREmpOverTime();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(ID);
                BSM.Header = DB.PREmpOverTimes.FirstOrDefault(w => w.TranNo == TranNo);
                if (BSM.Header != null)
                {
                    BSM.ListHeader = DB.PREmpOverTimes.Where(w => w.TranNo == TranNo).ToList();
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, PROverTimeObject collection)
        {
            ActionName = "Create";
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
                    BSM.Header = collection.Header;
                    BSM.HeaderStaff = collection.HeaderStaff;
                }
                BSM.ScreenId = SCREEN_ID;
                int TranNo = Convert.ToInt32(id);
                string msg = BSM.EditOT(TranNo);
                if (msg == SYConstant.OK)
                {
                    DB = new HumicaDBContext();
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
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
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id == "null") id = null;
            if (id != null)
            {
                PROverTimeObject BSM = new PROverTimeObject();
                BSM.Header = new PREmpOverTime();
                BSM.ListHeader = new List<PREmpOverTime>();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.PREmpOverTimes.FirstOrDefault(w => w.TranNo == TranNo);
                if (BSM.Header != null)
                {
                    BSM.ListHeader = DB.PREmpOverTimes.Where(w => w.TranNo == TranNo).ToList();
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
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
            if (!String.IsNullOrEmpty(id))
            {
                int TranNo = Convert.ToInt32(id);
                PROverTimeObject Del = new PROverTimeObject();
                string msg = Del.DeleteOT(TranNo);
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
        #region Grid Create Action
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PROverTimeObject BSM = new PROverTimeObject();
            BSM.ListHeader = new List<PREmpOverTime>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridItems", BSM);
        }
        public ActionResult CreateItem(PREmpOverTime ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PROverTimeObject BSM = new PROverTimeObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        DateTime otFromTime = ModelObject.OTFromTime == null ? DateTimeHelper.MaxValue : (DateTime)ModelObject.OTFromTime;
                        DateTime otToTime = ModelObject.OTToTime == null ? DateTimeHelper.MaxValue : (DateTime)ModelObject.OTToTime;
                        GetDateTime(ModelObject.OTDate, ref otFromTime, ref otToTime);
                        ModelObject.OTFromTime = otFromTime;
                        ModelObject.OTToTime = otToTime;
                        var BreakTime = ModelObject.BreakTime / 60;
                        ModelObject.OTHour = (decimal)otToTime.Subtract(otFromTime).TotalHours - BreakTime.Value;

                        BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                        var Result = BSM.IsExistOT(BSM.HeaderStaff.EmpCode, ModelObject.OTType, ModelObject.OTDate.Value);
                        if (Result == -1)
                        {
                            BSM.ListHeader.Add(ModelObject);
                            Session[Index_Sess_Obj + ActionName] = BSM;
                        }
                        else
                        {
                            Session[Index_Sess_Obj + ActionName] = BSM;
                            ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST");
                        }
                    }
                    else
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        ViewData["EditError"] = SYMessages.getMessage("NO_CATCHING");
                    }
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

            return PartialView("GridItems", BSM);
        }
        public ActionResult CreateItemEdit(PREmpOverTime ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PROverTimeObject BSM = new PROverTimeObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        DateTime otFromTime = ModelObject.OTFromTime == null ? DateTimeHelper.MaxValue : (DateTime)ModelObject.OTFromTime;
                        DateTime otToTime = ModelObject.OTToTime == null ? DateTimeHelper.MaxValue : (DateTime)ModelObject.OTToTime;
                        GetDateTime(ModelObject.OTDate, ref otFromTime, ref otToTime);
                        BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                        var Result = BSM.IsExistOT(BSM.HeaderStaff.EmpCode, ModelObject.OTType, ModelObject.OTDate.Value);
                        if (Result == -1)
                        {
                            var objCheck = BSM.ListHeader.Where(w => w.OTType == ModelObject.OTType).ToList();
                            var BreakTime = ModelObject.BreakTime / 60;
                            if (objCheck.Count > 0)
                            {
                                //objCheck.First().OTHour = ModelObject.OTHour;
                                objCheck.First().PayMonth = ModelObject.PayMonth;
                                objCheck.First().OTDate = ModelObject.OTDate;
                                objCheck.First().BreakTime = ModelObject.BreakTime;
                                objCheck.First().OTFromTime = otFromTime;
                                objCheck.First().OTToTime = otToTime;
                                objCheck.First().OTHour = (decimal)otToTime.Subtract(otFromTime).TotalHours - BreakTime.Value;
                                if (objCheck.First().OTHour <= 0)
                                {
                                    Session[Index_Sess_Obj + ActionName] = BSM;
                                    ViewData["EditError"] = SYMessages.getMessage("RECORD_OT");
                                }
                            }
                        }
                        else
                        {
                            Session[Index_Sess_Obj + ActionName] = BSM;
                            ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST");
                        }
                    }
                    else
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        ViewData["EditError"] = SYMessages.getMessage("NO_CATCHING");
                    }
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
            return PartialView("GridItems", BSM);
        }
        public ActionResult CreateItemDelete(string OTType)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ActionName = "Create";
            PROverTimeObject BSM = new PROverTimeObject();
            if (OTType != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                    if (BSM.ListHeader.Count > 0)
                    {
                        var objCheck = BSM.ListHeader.FirstOrDefault(w => w.OTType == OTType);
                        BSM.ListHeader.Remove(objCheck);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }
            }
            return PartialView("GridItems", BSM);
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
            BSM.ListHeader = new List<PREmpOverTime>();
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
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PROverTime", SYSConstant.DEFAULT_UPLOAD_LIST);

            PROverTimeObject BSM = new PROverTimeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }

            BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();

            if (BSM.ListTemplate.Count > 0)
            {
                SYExcel excel = new SYExcel();
                excel.FileName = BSM.ListTemplate.First().UpoadPath;
                DataTable dt = excel.GenerateExcelData();
                BSM.ListHeader = new List<PREmpOverTime>();

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var objHeader = new PREmpOverTime();
                        objHeader.EmpCode = dt.Rows[i][0].ToString();
                        objHeader.OTType = dt.Rows[i][2].ToString();
                        objHeader.OTDate = SYSettings.getDateValue(dt.Rows[i][3].ToString());
                        objHeader.PayMonth = SYSettings.getDateValue(dt.Rows[i][4].ToString());
                        objHeader.OTFromTime = SYSettings.getDateValue(dt.Rows[i][5].ToString());
                        objHeader.OTToTime = SYSettings.getDateValue(dt.Rows[i][6].ToString());
                        objHeader.OTHour = (decimal)SYSettings.getNumberValue(dt.Rows[i][7].ToString());
                        objHeader.BreakTime = (decimal)SYSettings.getNumberValue(dt.Rows[i][8].ToString());
                        BSM.ListHeader.Add(objHeader);
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
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PROverTime", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("OT"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "PAYROLL";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadMQ",
                sfi.ValidationSettings,
                sfi.uc_FileUploadComplete);

            return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
            //return null;
        }

        public ActionResult UploadList()
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PROverTime", SYSConstant.DEFAULT_UPLOAD_LIST);
            IEnumerable<MDUploadTemplate> listu = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID).OrderByDescending(w => w.UploadDate);
            return PartialView(SYListConfuration.ListDefaultUpload, listu.ToList());
        }

        public ActionResult GenerateUpload(int id)
        {
            UserSession();

            MDUploadTemplate obj = DB.MDUploadTemplates.Find(id);
            HumicaDBContext DBB = new HumicaDBContext();
            if (obj != null)
            {
                SYExcel excel = new SYExcel();
                excel.FileName = obj.UpoadPath;
                DataTable dt = excel.GenerateExcelData();
                if (obj.IsGenerate == true)
                {
                    SYMessages mess = SYMessages.getMessageObject("FILE_RG", user.Lang);
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
                }
                if (dt != null)
                {
                    PROverTimeObject BSM = new PROverTimeObject();

                    BSM.ScreenId = ScreendIDControl;
                    BSM.ListHeader = new List<PREmpOverTime>();
                    BSDocConfg DocBatch = new BSDocConfg("BATCH_UPLOAD", DocConfType.Normal, true);

                    try
                    {
                        if (dt.Rows.Count > 0)
                        {

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                var objHeader = new PREmpOverTime();
                                objHeader.EmpCode = dt.Rows[i][0].ToString();
                                objHeader.OTType = dt.Rows[i][2].ToString();
                                objHeader.OTDate = SYSettings.getDateValue(dt.Rows[i][3].ToString());
                                objHeader.PayMonth = SYSettings.getDateValue(dt.Rows[i][4].ToString());
                                objHeader.OTFromTime = SYSettings.getDateValue(dt.Rows[i][5].ToString());
                                objHeader.OTToTime = SYSettings.getDateValue(dt.Rows[i][6].ToString());
                                objHeader.OTHour = (decimal)SYSettings.getNumberValue(dt.Rows[i][7].ToString());
                                objHeader.BreakTime = (decimal)SYSettings.getNumberValue(dt.Rows[i][8].ToString());
                                BSM.ListHeader.Add(objHeader);
                            }
                            var msg = BSM.ImportOTTime();
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
                        }
                        obj.Message = SYConstant.OK;
                        obj.DocumentNo = DocBatch.NextNumberRank;
                        obj.IsGenerate = true;
                        DBB.MDUploadTemplates.Attach(obj);
                        DBB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DBB.Entry(obj).Property(w => w.DocumentNo).IsModified = true;
                        DBB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DBB.SaveChanges();

                    }
                    catch (DbEntityValidationException e)
                    {
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserID.ToString();
                        log.DocurmentAction = "OT_UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e);
                        /*----------------------------------------------------------*/
                        obj.Message = e.Message;
                        obj.IsGenerate = false;
                        DB.MDUploadTemplates.Attach(obj);
                        DB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DB.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        obj.Message = e.Message;
                        obj.IsGenerate = false;
                        DB.MDUploadTemplates.Attach(obj);
                        DB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DB.SaveChanges();
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserName.ToString();
                        log.ScreenId = "OT_UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e, true);
                        /*----------------------------------------------------------*/

                    }
                    catch (Exception e)
                    {
                        obj.Message = e.Message;
                        obj.IsGenerate = false;
                        DB.MDUploadTemplates.Attach(obj);
                        DB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DB.SaveChanges();

                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserName.ToString();
                        log.DocurmentAction = "OT_UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e, true);
                        /*----------------------------------------------------------*/
                    }
                }
                else
                {
                    obj.Message = SYMessages.getMessage("EX_DT");
                    obj.IsGenerate = false;
                    DB.MDUploadTemplates.Attach(obj);
                    DB.Entry(obj).Property(w => w.Message).IsModified = true;
                    DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                    DB.SaveChanges();

                }

            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
        }

        public ActionResult DownloadTemplate()
        {
            var OTCode = DB.PROTRates.ToList();

            using (var workbook = new DevExpress.Spreadsheet.Workbook())
            {
                // Ensure sheet names are unique
                workbook.Worksheets[0].Name = "Master";
                List<ExCFUploadMapping> _ListMaster = new List<ExCFUploadMapping>();
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Employee Code" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Employee Name" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "OT Code" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "OT Date\n(date)" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "PayMonth\n(date)" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "From Time\n(datetime)" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "To Time\n(datetime)" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "OT Hour\n(number)" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "BreakTime(mn)\n(number)" });
                Worksheet worksheet = workbook.Worksheets[0];
                var sheet2 = workbook.Worksheets.Add("OT-Code");

                List<ExCFUploadMapping> _ListMaster1 = new List<ExCFUploadMapping>();
                _ListMaster1.Add(new ExCFUploadMapping { Caption = "Code", FieldName = "Code" });
                _ListMaster1.Add(new ExCFUploadMapping { Caption = "Description", FieldName = "Description" });
                _ListMaster1.Add(new ExCFUploadMapping { Caption = "Remark", FieldName = "Remark" });

                List<ClsUploadMapping> _ListData = new List<ClsUploadMapping>();
                foreach (var read in OTCode)
                {
                    _ListData.Add(new ClsUploadMapping
                    {
                        FieldName = read.OTCode,
                        FieldName1 = read.OTType
                    });
                }
                // Export data to each sheet with header formatting
                ClsConstant.ExportDataToWorksheet(worksheet, _ListMaster);
                ClsConstant.ExportDataToWorksheet(sheet2, _ListMaster1);
                ClsConstant.ExportDataToWorksheetRow(sheet2, _ListData);

                // Save the workbook to a memory stream
                using (var stream = new System.IO.MemoryStream())
                {
                    workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OT_TEMPLATE.xlsx");
                }
            }
            return null;
        }
        #endregion
        #region 'Code'
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {

            ActionName = "Create";
            var Stafff = DBV.HR_STAFF_VIEW.ToList();
            PROverTimeObject BSM = new PROverTimeObject();
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

                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
                    BSM.HeaderStaff = EmpStaff;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();

            ViewData["STAFF_SELECT"] = Staff;
            ViewData["OTType_SELECT"] = DB.PROTRates.ToList();
        }
        #endregion
        private void GetDateTime(DateTime? otDate, ref DateTime fromTime, ref DateTime toTime)
        {
            //fromTime = new DateTime(otDate.Value.Year, otDate.Value.Month, otDate.Value.Day, fromTime.Hour, fromTime.Minute, 0);
            if (fromTime.Year != 5000)
                fromTime = new DateTime(otDate.Value.Year, otDate.Value.Month, otDate.Value.Day, fromTime.Hour, fromTime.Minute, 0);
            if (toTime.Year != 5000)
                toTime = new DateTime(otDate.Value.Year, otDate.Value.Month, otDate.Value.Day, toTime.Hour, toTime.Minute, 0);

            bool sliptDate = false;
            if ((fromTime.Year != 5000) && (toTime.Year != 5000))
            {
                var pDateTime = fromTime.ToString("tt", CultureInfo.InvariantCulture);//toDate.ToString("dd-MMM-yyyy hh:mm tt");
                var cDateTime = toTime.ToString("tt", CultureInfo.InvariantCulture);
                if (pDateTime == "PM" && cDateTime == "AM") sliptDate = true;
                if (sliptDate)
                    toTime = toTime.AddDays(1);
            }
        }
    }
}