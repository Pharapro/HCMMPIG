using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Leave
{
    public class ImportLeaveRequestController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRL0000008";
        private const string URL_SCREEN = "/HRM/Leave/ImportLeaveRequest/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public ImportLeaveRequestController() : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        // GET: ImportLeaveRequest
        public ActionResult Index()
        {
            ActionName = "Import";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ImportLeaveRequestObject BSM = new ImportLeaveRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ImportLeaveRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
        }

        public ActionResult GridItems()
        {
            ActionName = "Import";
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            ImportLeaveRequestObject BSM = new ImportLeaveRequestObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ImportLeaveRequestObject)Session[Index_Sess_Obj + ActionName];
                if (BSM.LeaveImportItem != null)
                {
                    BSM.LeaveImportItem.Clear();
                }
            }
            //if (BSM.ListTemplate.Count > 0)
            //{
            //    SYExcel excel = new SYExcel();
            //    foreach (var read in BSM.ListTemplate.ToList())
            //    {
            //        excel.FileName = read.UpoadPath;
            //    }
            //    DataTable dtHeader = excel.GenerateExcelData();
            //    //BSM.ListHeader = new List<HRStaffProfile>();
            //    BSM.LeaveImportItem = new List<ImportLeaveRequestObject>();

            //    if (dtHeader != null)
            //    {
            //        LeaveImportItem(dtHeader, ref BSM);
            //    }
            //}
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM);
        }
        #region "Import"
        public ActionResult Import()
        {
            UserSession();
            ActionName = "Import";
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "ImportLeaveRequest", SYSConstant.DEFAULT_UPLOAD_LIST);

            var BSM = new ImportLeaveRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ImportLeaveRequestObject)Session[Index_Sess_Obj + ActionName];
                if (BSM.LeaveImportItem != null)
                {
                    BSM.LeaveImportItem.Clear();
                }

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

                //BSM.ListHeader = new List<HRStaffProfile>();
                //BSM.LeaveImportItem = new List<ImportLeaveRequestObject>();
                //if (dtHeader != null)
                //{
                //    this.LeaveImportItem(dtHeader, ref BSM);
                //}

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
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "ImportLeaveRequest", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("ImportLeave"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "HR";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
                sfi.ValidationSettings,
                sfi.uc_FileUploadComplete);


            var objStaff = new ImportLeaveRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (ImportLeaveRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            objStaff.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            Session[Index_Sess_Obj + ActionName] = objStaff;
            return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
            //return null;
        }
        public ActionResult UploadList()
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "ImportLeaveRequest", SYSConstant.DEFAULT_UPLOAD_LIST);

            var objStaff = new ImportLeaveRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (ImportLeaveRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            objStaff.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();

            Session[Index_Sess_Obj + ActionName] = objStaff;
            return PartialView(SYListConfuration.ListDefaultUpload, objStaff.ListTemplate);
        }
        [HttpGet]
        public ActionResult GenerateUpload(int id)
        {
            UserSession();
            MDUploadTemplate obj = DB.MDUploadTemplates.Find(id);
            var objStaff = new ImportLeaveRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (ImportLeaveRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            if (obj != null)
            {
                var DBB = new HumicaDBContext();
                SYExcel excel = new SYExcel();
                excel.FileName = obj.UpoadPath;
                DataTable dtHeader = excel.GenerateExcelData();
                var BSM = new ImportLeaveRequestObject();

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
                        if (dtHeader.Rows.Count > 0) // Header
                        {
                            if (dtHeader != null)
                            {
                                this.LeaveImportItem(dtHeader, ref BSM);
                            }
                        }

                        DateTime create = DateTime.Now;

                        msg = BSM.GenerateLeaveImport();
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
                        obj.Message = SYConstant.OK;
                        obj.IsGenerate = true;
                        DBB.MDUploadTemplates.Attach(obj);
                        DBB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DBB.Entry(obj).Property(w => w.DocumentNo).IsModified = true;
                        DBB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DBB.SaveChanges();

                    }
                    catch (Exception e)
                    {
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserID.ToString();
                        log.DocurmentAction = "UPLOAD";
                        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

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
            var RelCode = DB.HRLeaveTypes.ToList();
            SYDataList objList = new SYDataList("LEAVE_TIME");

            using (var workbook = new DevExpress.Spreadsheet.Workbook())
            {
                // Ensure sheet names are unique
                workbook.Worksheets[0].Name = "import-leave";
                List<ExCFUploadMapping> _ListMaster = new List<ExCFUploadMapping>();
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Employee Code" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Employee Name" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Join Date\n(date)" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Leave Type" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Leave Date\n(date)" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Leave Status" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Remark" });

                Worksheet worksheet = workbook.Worksheets[0];
                var sheet2 = workbook.Worksheets.Add("leave-type");

                List<ExCFUploadMapping> _ListMaster1 = new List<ExCFUploadMapping>();
                _ListMaster1.Add(new ExCFUploadMapping { Caption = "Code", FieldName = "Code" });
                _ListMaster1.Add(new ExCFUploadMapping { Caption = "Description", FieldName = "Description" });
                _ListMaster1.Add(new ExCFUploadMapping { Caption = "Remark", FieldName = "Remark" });

                Worksheet worksheet1 = workbook.Worksheets[1];
                var sheet3 = workbook.Worksheets.Add("leave status");

                List<ExCFUploadMapping> _ListMaster2 = new List<ExCFUploadMapping>();
                _ListMaster2.Add(new ExCFUploadMapping { Caption = "Code", FieldName = "Code" });
                _ListMaster2.Add(new ExCFUploadMapping { Caption = "Description", FieldName = "Description" });

                List<ClsUploadMapping> _ListData = new List<ClsUploadMapping>();
                foreach (var read in RelCode)
                {
                    _ListData.Add(new ClsUploadMapping
                    {
                        FieldName = read.Code,
                        FieldName1 = read.Description,
                        FieldName2 = read.OthDesc
                    });
                }
                List<ClsUploadMapping> _ListData1 = new List<ClsUploadMapping>();
                foreach (var read in objList.ListData)
                {
                    _ListData1.Add(new ClsUploadMapping
                    {
                        FieldName = read.SelectValue,
                        FieldName1 = read.SelectText,
                    });
                }
                // Export data to each sheet with header formatting
                ClsConstant.ExportDataToWorksheet(worksheet, _ListMaster);
                ClsConstant.ExportDataToWorksheet(sheet2, _ListMaster1);
                ClsConstant.ExportDataToWorksheetRow(sheet2, _ListData);
                //
                ClsConstant.ExportDataToWorksheet(sheet3, _ListMaster2);
                ClsConstant.ExportDataToWorksheetRow(sheet3, _ListData1);

                // Save the workbook to a memory stream
                using (var stream = new System.IO.MemoryStream())
                {
                    workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "import-leave.xlsx");
                }
            }
            return null;
        }
        #endregion

        //private void DataSelector()
        //{
        //    ViewData["STAFF_SELECT"] = DBV.HR_STAFF_VIEW.ToList();
        //}
        private void LeaveImportItem(DataTable dtHeader, ref ImportLeaveRequestObject BSM)
        {
            ImportLeaveRequestObject importLeaves = null;
            BSM.LeaveImportItem = new List<ImportLeaveRequestObject>();
            for (int i = 0; i < dtHeader.Rows.Count; i++)
            {
                string empCode = dtHeader.Rows[i][0].ToString().Trim();
                string leaveCode = dtHeader.Rows[i][3].ToString().Trim();
                DateTime? leaveDate = Humica.Core.DateTimeHelper.ObjectToDateTime(dtHeader.Rows[i][4].ToString());
                string leaveStatus = dtHeader.Rows[i][5].ToString().Trim();
                string leaveReason = dtHeader.Rows[i][6].ToString();
                string empName = "";
                string errorEmpCode = "";
                string errorLeaveCode = "";
                string errorDate = "";
                string separator = string.Empty;
                StringBuilder remark = new StringBuilder();
                decimal WHour = 8;
                decimal LHour = 0;

                //validation
                List<HRStaffProfile> emp = DB.HRStaffProfiles.Where(x => x.EmpCode == empCode).ToList();
                if (emp.Count <= 0)
                {
                    errorEmpCode = "Invalid employee code";
                    remark.Append(errorEmpCode);
                }
                List<HRLeaveType> lt = DB.HRLeaveTypes.Where(x => x.Code == leaveCode).ToList();
                if (lt.Count <= 0)
                {
                    errorLeaveCode = "Invalid leave code";
                    if (remark.Length > 0)
                        separator = ",";

                    remark.Append(separator + errorLeaveCode);
                }
                if (leaveDate == null)
                {
                    errorDate = "Invalid date";
                    if (remark.Length > 0)
                        separator = ",";

                    remark.Append(separator + errorDate);
                }
                if (String.IsNullOrEmpty(errorEmpCode))
                {
                    var _PayPram = DB.HRStaffProfiles.Where(w => w.EmpCode == empCode).FirstOrDefault();
                    empName = DB.HRStaffProfiles.ToList().Where(x => x.EmpCode == empCode).FirstOrDefault().AllName;
                    if (_PayPram != null)
                    {
                        WHour = Convert.ToDecimal(DB.PRParameters.Find(_PayPram.PayParam).WHOUR);
                        LHour = WHour;
                    }
                }
                if (leaveStatus == "Morning" || leaveStatus == "Afternoon")
                    LHour = WHour / 2;
                importLeaves = new ImportLeaveRequestObject()
                {
                    EmpCode = empCode,
                    AllName = empName,
                    LeaveCode = leaveCode,
                    Status = leaveStatus,
                    LeaveDate = leaveDate,
                    Reason = leaveReason,
                    MessageError = remark.ToString(),
                    LHour = LHour,
                    WorkHourPerDay = Convert.ToDouble(WHour)
                };
                BSM.LeaveImportItem.Add(importLeaves);
            }

        }
    }
}