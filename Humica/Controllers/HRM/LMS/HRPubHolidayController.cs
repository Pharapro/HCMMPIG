using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using DevExpress.Xpo.Logger.Transport;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.LMS
{

    public class HRPubHolidayController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRS0000009";
        private const string URL_SCREEN = "/HRM/LMS/HRPubHoliday/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";

        IClsPubHoliDay BSM;
        IUnitOfWork unitOfWork;
        public HRPubHolidayController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsPubHoliDay();
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }


        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.Filter = new FTFilterEmployee();
            BSM.Filter.InDate = DateTime.Now;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IClsPubHoliDay)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = obj.Filter;
            }
           
            BSM.ListHeader = unitOfWork.Set<HRPubHoliday>().AsQueryable().Where(w => w.PDate.Year == BSM.Filter.InDate.Year)
                            .OrderByDescending(w => w.PDate).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsPubHoliDay collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.Filter = new FTFilterEmployee();
            BSM.Filter.InDate = collection.Filter.InDate;
            BSM.Filter.Branch = collection.Filter.Branch;
            if (!string.IsNullOrEmpty(BSM.Filter.Branch) && BSM.Filter.InDate != null)
                BSM.ListHeader = unitOfWork.Set<HRPubHoliday>().AsQueryable().
                                Where(w => w.PDate.Year == BSM.Filter.InDate.Year && w.Branch == BSM.Filter.Branch)
                                .OrderByDescending(w => w.PDate).ToList();
            else if (!string.IsNullOrEmpty(BSM.Filter.Branch))
                BSM.ListHeader = unitOfWork.Set<HRPubHoliday>().AsQueryable()
                    .Where(w => w.Branch == BSM.Filter.Branch).OrderByDescending(w => w.PDate).ToList();
            else if (BSM.Filter.InDate.Year > 0)
                BSM.ListHeader = unitOfWork.Set<HRPubHoliday>().AsQueryable()
                    .Where(w => w.PDate.Year == BSM.Filter.InDate.Year).OrderByDescending(w => w.PDate).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.ListHeader = new List<HRPubHoliday>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPubHoliDay)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion

        public ActionResult GridViews()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IClsPubHoliDay)Session[Index_Sess_Obj + ActionName];
                BSM.ListHeader = obj.ListHeader;
            }
            else
                BSM.ListHeader = unitOfWork.Set<HRPubHoliday>().AsQueryable().OrderByDescending(w => w.PDate).ToList();
            return PartialView("GridViews", BSM.ListHeader);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateItem(HRPubHoliday MModel)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (!string.IsNullOrEmpty(MModel.Branch))
            {
                try
                {
                    var listPH = unitOfWork.Set<HRPubHoliday>().AsQueryable().ToList();
                    if (listPH.Where(w => w.PDate.Date == MModel.PDate.Date).ToList().Count() == 0)
                    {
                        MModel.CreatedOn = DateTime.Now.Date;
                        MModel.CreatedBy = user.UserName;
                        unitOfWork.Add(MModel);
                        unitOfWork.Save();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("PUB_EN");
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
            int date = MModel.PDate.Year;
            BSM.ListHeader = unitOfWork.Set<HRPubHoliday>().AsQueryable().Where(w => w.PDate.Year == date)
                            .OrderByDescending(w => w.PDate).ToList();
            return PartialView("GridViews", BSM.ListHeader);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditItem(HRPubHoliday MModel)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (!string.IsNullOrEmpty(MModel.Branch))
            {
                try
                {
                    var objUpdate = unitOfWork.Set<HRPubHoliday>().AsQueryable()
                                    .FirstOrDefault(w => w.PDate == MModel.PDate.Date && w.Branch == MModel.Branch);
                    if (objUpdate != null)
                    {
                        objUpdate.Description = MModel.Description;
                        objUpdate.SecDescription = MModel.SecDescription;
                        objUpdate.ChangedOn = DateTime.Now.Date;
                        objUpdate.ChangedBy = user.UserName;
                        unitOfWork.Update(objUpdate);
                        unitOfWork.Save();
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
            int date = MModel.PDate.Year;
            BSM.ListHeader = unitOfWork.Set<HRPubHoliday>().AsQueryable().Where(w => w.PDate.Year == date)
                            .OrderByDescending(w => w.PDate).ToList();
            return PartialView("GridViews", BSM.ListHeader);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteItem(string PDate,string Branch)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (PDate != null)
            {
                try
                {
                    DateTime _PDate = Convert.ToDateTime(PDate);
                    var objUpdate = unitOfWork.Set<HRPubHoliday>().AsQueryable()
                                    .FirstOrDefault(w => w.PDate == _PDate && w.Branch == Branch);
                    if (objUpdate != null)
                    {
                        unitOfWork.Delete(objUpdate);
                        unitOfWork.Save();
                    }
                    int date = _PDate.Year;
                    BSM.ListHeader = unitOfWork.Set<HRPubHoliday>().AsQueryable().Where(w => w.PDate.Year == date)
                                    .OrderByDescending(w => w.PDate).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridViews", BSM.ListHeader);
        }
        #region Import
        public ActionResult Import()
        {
            UserSession();
            ActionName = "Import";
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRPubHoliday", SYSConstant.DEFAULT_UPLOAD_LIST);

            var objHoliday = new ClsPubHoliDay();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objHoliday = (ClsPubHoliDay)Session[Index_Sess_Obj + ActionName];
                if (objHoliday.ListHeader != null)
                {
                    objHoliday.ListHeader.Clear();
                }

            }

            objHoliday.ListTemplate = unitOfWork.Set<MDUploadTemplate>().AsQueryable().Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();

            if (objHoliday.ListTemplate.Count > 0)
            {
                SYExcel excel = new SYExcel();
                foreach (var read in objHoliday.ListTemplate.ToList())
                {
                    excel.FileName = read.UpoadPath;
                }
                DataTable dtHeader = excel.GenerateExcelData();
                objHoliday.ListHeader = new List<HRPubHoliday>();
                if (dtHeader != null)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        var objHeader = new HRPubHoliday();
                        objHeader.Branch = dtHeader.Rows[i][0].ToString();
                        objHeader.PDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][1].ToString());
                        objHeader.Description = dtHeader.Rows[i][2].ToString();
                        objHeader.SecDescription = dtHeader.Rows[i][3].ToString();

                        objHoliday.ListHeader.Add(objHeader);

                    }
                }
            }
            Session[Index_Sess_Obj + ActionName] = objHoliday;
            return View(objHoliday);
        }

        [HttpPost]
        public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        {

            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRPubHoliday", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(unitOfWork.Set<CFUploadPath>().Find("PUB"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "HR";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
                sfi.ValidationSettings,
                sfi.uc_FileUploadComplete);


            var objStaff = new ClsPubHoliDay();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (ClsPubHoliDay)Session[Index_Sess_Obj + ActionName];
            }


            objStaff.ListTemplate = unitOfWork.Set<MDUploadTemplate>().AsQueryable().Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            objStaff.ListHeader = new List<HRPubHoliday>();


            Session[Index_Sess_Obj + ActionName] = objStaff;
            return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
            //return null;
        }
        public ActionResult UploadList()
        {
            UserSession();
            ActionName = "Import";
            DataSelector();
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRPubHoliday", SYSConstant.DEFAULT_UPLOAD_LIST);

            var objStaff = new ClsPubHoliDay();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (ClsPubHoliDay)Session[Index_Sess_Obj + ActionName];
            }

            objStaff.ListTemplate = unitOfWork.Set<MDUploadTemplate>().AsQueryable().Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            objStaff.ListHeader = new List<HRPubHoliday>();


            Session[Index_Sess_Obj + ActionName] = objStaff;
            return PartialView(SYListConfuration.ListDefaultUpload, objStaff.ListTemplate);
        }

        [HttpGet]
        public ActionResult GenerateUpload(int id)
        {
            UserSession();
            DataSelector();
            MDUploadTemplate obj = unitOfWork.Set<MDUploadTemplate>().Find(id);
            if (obj != null)
            {
                SYExcel excel = new SYExcel();
                excel.FileName = obj.UpoadPath;
                DataTable dtHeader = excel.GenerateExcelData();
                var objHoliday = new ClsPubHoliDay();
                objHoliday.Header = new HRPubHoliday();
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
                            objHoliday.ListHeader = new List<HRPubHoliday>();
                            for (int i = 0; i < dtHeader.Rows.Count; i++)
                            {
                                var objHeader = new HRPubHoliday();
                                string branchCell = dtHeader.Rows[i][0]?.ToString() ?? string.Empty;
                                string[] parts = branchCell.Split(new[] { " - " }, StringSplitOptions.None);
                                objHeader.Branch = parts.Length > 0 ? parts[0].Trim() : string.Empty;
                                objHeader.PDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][1].ToString());
                                objHeader.Description = dtHeader.Rows[i][2].ToString();
                                objHeader.SecDescription = dtHeader.Rows[i][3].ToString();


                                objHoliday.ListHeader.Add(objHeader);

                            }

                            msg = objHoliday.uploadPubHoliday();
                            if (msg == SYConstant.OK)
                            {
                                obj.Message = SYConstant.OK;
                                obj.IsGenerate = true;
                                unitOfWork.Update(obj);
                                unitOfWork.Save();
                                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("IMPORTED", user.Lang);

                                //if (obj != null)
                                //{
                                //    unitOfWork.Delete(obj);
                                //    unitOfWork.Save();

                                //    if (System.IO.File.Exists(obj.UpoadPath))
                                //    {
                                //        System.IO.File.Delete(obj.UpoadPath);
                                //    }
                                //}

                            }
                            else
                            {
                                obj.Message = SYMessages.getMessage(msg);
                                obj.Message += ":" + BSM.MessageError;
                                obj.IsGenerate = false;
                                unitOfWork.Update(obj);
                                unitOfWork.Save();
                                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                            }


                        }
                    }
                    catch (Exception e)
                    {
                        obj.Message = ClsEventLog.Save_EventLogs(SCREEN_ID, user.UserName, "UPLOAD", SYActionBehavior.ADD.ToString(), e, true);
                        unitOfWork.Update(obj);
                        unitOfWork.Save();
                    }
                }

            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
        }

        public ActionResult DownloadTemplate()
        {
            try
            {
                var branches = unitOfWork.Set<HRBranch>().AsQueryable().ToList();

                using (var workbook = new DevExpress.Spreadsheet.Workbook())
                {
                    // ===== 1. MASTER WORKSHEET SETUP =====
                    Worksheet masterWorksheet = workbook.Worksheets[0];
                    masterWorksheet.Name = "Master";

                    // Configure columns
                    var masterColumns = new List<ExCFUploadMapping>
            {
                new ExCFUploadMapping { FieldName = "Branch Code" },
                new ExCFUploadMapping { FieldName = "Date" },
                new ExCFUploadMapping { FieldName = "Description" },
                new ExCFUploadMapping { FieldName = "Sec Description" }
            };

                    // Apply column headers
                    ClsConstant.ExportDataToWorksheet(masterWorksheet, masterColumns);

                    // Format headers
                    CellRange masterHeader = masterWorksheet.Range["A1:D1"];
                    masterHeader.FillColor = Color.LightGray;
                    masterWorksheet.Columns["A"].WidthInCharacters = 15;
                    masterWorksheet.Columns["B"].WidthInCharacters = 15;
                    masterWorksheet.Columns["C"].WidthInCharacters = 25;
                    masterWorksheet.Columns["D"].WidthInCharacters = 30;

                    // Add comments and make them visible
                    Comment commentA1 = masterWorksheet.Comments.Add(masterWorksheet["A1"], workbook.CurrentAuthor, "Select from dropdown list");
                    //commentA1.Visible = true;

                    Comment commentB1 = masterWorksheet.Comments.Add(masterWorksheet["B1"], workbook.CurrentAuthor, "Date format: DD/MM/YYYY");
                    //commentB1.Visible = true;

                    CommentRunCollection runA = commentA1.Runs;
                    if (runA.Count > 0)
                    {
                        runA[0].Font.Bold = true;
                        runA[0].Font.Color = Color.Blue;
                    }

                    CommentRunCollection runB = commentB1.Runs;
                    if (runB.Count > 0)
                    {
                        runB[0].Font.Bold = true;
                        runB[0].Font.Color = Color.Blue;
                    }

                    // ===== 2. BRANCH CODE WORKSHEET =====
                    Worksheet branchWorksheet = workbook.Worksheets.Add("BranchCode");

                    // Populate branch data
                    var branchData = branches.Select(b => new
                    {
                        Code = b.Code,
                        Description = b.Description,
                        SecDescription = b.SecDescription
                    }).ToList();

                    // Add data to the BranchCode worksheet
                    for (int i = 0; i < branchData.Count; i++)
                    {
                        branchWorksheet.Cells[i + 1, 0].Value = branchData[i].Code; // Column A: Code
                        branchWorksheet.Cells[i + 1, 1].Value = branchData[i].Description; // Column B: Description
                        branchWorksheet.Cells[i + 1, 2].Formula = $"=A{i + 2} & \" - \" & B{i + 2}"; // Column C: Combined
                    }

                    // ===== 3. DATA VALIDATION =====
                    CellRange validationRange = masterWorksheet.Range["A2:A1048576"];
                    string formula = $"='BranchCode'!$C$2:$C${branchData.Count + 1}";
                    var validation = masterWorksheet.DataValidations.Add(
                        validationRange,
                        DataValidationType.List,
                        ValueObject.FromFormula(formula, false)
                    );

                    // Configure validation messages
                    validation.ShowInputMessage = true;
                    //validation.InputTitle = "Branch Selection";
                    //validation.InputMessage = "Choose from the dropdown list";
                    validation.ShowErrorMessage = true;
                    validation.ErrorStyle = DataValidationErrorStyle.Stop;
                    validation.ErrorTitle = "Invalid Value";
                    validation.ErrorMessage = "Must select a valid branch code";

                    //// ===== 4. EXTRACT CODE FROM SELECTION =====
                    //// Add formula to extract code from selected value
                    //masterWorksheet.Cells["E1"].Value = "Branch Code Extracted";
                    //for (int i = 0; i < 1000; i++) // Adjust the range as needed
                    //{
                    //    masterWorksheet.Cells[i + 1, 4].Formula = $"=LEFT(A{i + 2}, FIND(\" -\", A{i + 2}) - 1)";
                    //}

                    //// Hide the helper column if desired
                    //masterWorksheet.Columns["E"].Visible = false;

                    // ===== 5. HIDE BRANCHCODE WORKSHEET =====
                    branchWorksheet.Visible = false;

                    // ===== 6. GENERATE FILE =====
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveDocument(stream, DocumentFormat.Xlsx);
                        return File(stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "PublicHolidayTemplate.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Template Error: {ex}");
                return Content($"Error generating template: {ex.Message}. Please contact IT support.");
            }
        }


        public ActionResult GridItems()
        {
            ActionName = "Import";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsPubHoliDay objHoliday = new ClsPubHoliDay();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objHoliday = (ClsPubHoliDay)Session[Index_Sess_Obj + ActionName];
                if (objHoliday.ListHeader != null)
                {
                    objHoliday.ListHeader.Clear();
                }
            }
            if (objHoliday.ListTemplate.Count > 0)
            {
                SYExcel excel = new SYExcel();
                foreach (var read in objHoliday.ListTemplate.ToList())
                {
                    excel.FileName = read.UpoadPath;
                }
                DataTable dtHeader = excel.GenerateExcelData();
                objHoliday.ListHeader = new List<HRPubHoliday>();

                if (dtHeader != null)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        var objHeader = new HRPubHoliday();
                        objHeader.Branch = dtHeader.Rows[i][0].ToString();
                        objHeader.PDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][1].ToString());
                        objHeader.Description = dtHeader.Rows[i][2].ToString();
                        objHeader.SecDescription = dtHeader.Rows[i][3].ToString();

                        objHoliday.ListHeader.Add(objHeader);

                    }
                }
            }
            //BSM.ListDepreMethod = DB.ExFADepreciationMethods.ToList();
            Session[Index_Sess_Obj + ActionName] = objHoliday;
            return PartialView("GridItems", objHoliday);
        }
        #endregion
        private void DataSelector()
        {
            //SYDataList objList = new SYDataList("FAAVERAGE_SELECT");
            //ViewData["FAAVERAGE_SELECT"] = objList.ListData;
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
        }
    }
}
