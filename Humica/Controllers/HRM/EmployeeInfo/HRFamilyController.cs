using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Integration.EF.Models;
using Humica.Logic.HR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.EmployeeInfo
{
    public class HRFamilyController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "HRE0000003";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HRFamily/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public HRFamilyController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);

            HRFamilyObject BSM = new HRFamilyObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (HRFamilyObject)Session[Index_Sess_Obj + ActionName];
            }
            await BSM.LoadDataFamily();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRFamilyObject BSM = new HRFamilyObject();
            BSM.ListHeader = new List<HR_Family_View>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRFamilyObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader.OrderBy(w => w.EmpCode).ToList();
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            HRFamilyObject BSM = new HRFamilyObject();
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRFamilyObject)Session[Index_Sess_Obj + ActionName];
            }
            //BSM.ListHeader = new List<HR_Family_View>();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM);
        }
		#endregion
		#region "Create"
		public async Task<ActionResult> Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            await DataList();
            UserConfListAndForm(this.KeyName);
            HRFamilyObject BSM = new HRFamilyObject();
            BSM.Header = new HREmpFamily();
            BSM.Header.TaxDeduc = false;
            BSM.Header.InSchool = false;
            BSM.Header.DateOfBirth = DateTime.Now;
			BSM.Header.EffectiveDate = DateTime.Now;
			BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.ListHeader = new List<HR_Family_View>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(HRFamilyObject collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            HRFamilyObject BSM = new HRFamilyObject();
            if (ModelState.IsValid)
            {
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.AttachFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HRFamilyObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                    BSM.HeaderStaff = collection.HeaderStaff;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateEmp();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
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

                HRFamilyObject BSM = new HRFamilyObject();
                BSM.Header = new HREmpFamily();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(ID);
                BSM.Header = DB.HREmpFamilies.FirstOrDefault(w => w.TranNo == TranNo);
                var resualt = DBV.HR_Family_View.ToList();
                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    List<HR_Family_View> listEmpfa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                    BSM.ListHeader = listEmpfa.ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, HRFamilyObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            HRFamilyObject BSM = new HRFamilyObject();

            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HRFamilyObject)Session[Index_Sess_Obj + ActionName];
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
                int TranNo = Convert.ToInt32(id);
                string msg = BSM.EditEmp(TranNo);
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
                HRFamilyObject Del = new HRFamilyObject();
                string msg = Del.DeleteFamily(TranNo);
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
                HRFamilyObject BSM = new HRFamilyObject();
                BSM.Header = new HREmpFamily();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.HREmpFamilies.FirstOrDefault(w => w.TranNo == TranNo);
                var resualt = DBV.HR_Family_View.ToList();
                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    List<HR_Family_View> listEmpfa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                    BSM.ListHeader = listEmpfa.ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #endregion
        #region Import
        public ActionResult Import()
        {

            UserSession();
            ActionName = "Import";
            UserConfListAndForm(this.KeyName);
            var obj = new HRFamilyObject();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRFamily", SYSConstant.DEFAULT_UPLOAD_LIST);
            obj.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID).OrderByDescending(w => w.UploadDate).ToList();
            Session[Index_Sess_Obj + ActionName] = obj;
            return View(obj);

        }

        [HttpPost]
        public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRFamily", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("FAMILY"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "FAMILY";
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
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRFamily", SYSConstant.DEFAULT_UPLOAD_LIST);
            IEnumerable<MDUploadTemplate> listu = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID).OrderByDescending(w => w.UploadDate);
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
                var objStaff = new HRFamilyObject();
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
                            objStaff.ListImport = new List<HREmpFamily>();
                            for (int i = 0; i < dtHeader.Rows.Count; i++)
                            {
                                var objHeader = new HREmpFamily();
                                objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                                objHeader.RelCode = dtHeader.Rows[i][2].ToString();
                                objHeader.RelName = dtHeader.Rows[i][3].ToString();
                                objHeader.Sex = dtHeader.Rows[i][4].ToString();
                                objHeader.DateOfBirth = SYSettings.getDateTimeValue(dtHeader.Rows[i][5].ToString());
                                objHeader.IDCard = dtHeader.Rows[i][6].ToString();
                                objHeader.PhoneNo = dtHeader.Rows[i][7].ToString();
                                int True_ = Convert.ToInt32(dtHeader.Rows[i][9].ToString());
                                objHeader.Occupation = dtHeader.Rows[i][8].ToString();
                                objHeader.TaxDeduc = Convert.ToBoolean(True_);
                                int T = Convert.ToInt32(dtHeader.Rows[i][10].ToString());
                                objHeader.InSchool = Convert.ToBoolean(T);
                                int Ch = Convert.ToInt32(dtHeader.Rows[i][11].ToString());
                                objHeader.Child = Convert.ToBoolean(Ch);
                                int Sp = Convert.ToInt32(dtHeader.Rows[i][12].ToString());
                                objHeader.Spouse = Convert.ToBoolean(Sp);
                                objStaff.ListImport.Add(objHeader);
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
            var RelCode = DB.HRRelationshipTypes.ToList();

            using (var workbook = new DevExpress.Spreadsheet.Workbook())
            {
                // Ensure sheet names are unique
                workbook.Worksheets[0].Name = "Master";
                List<ExCFUploadMapping> _ListMaster = new List<ExCFUploadMapping>();
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Employee Code" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Employee Name" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Relationship" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Relation Name" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Sex" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "DOB\n(date)" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "IDCard" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Phone" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Occupation" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Tax Deduct" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "In School" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Child" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Spouse" });
                Worksheet worksheet = workbook.Worksheets[0];
                var sheet2 = workbook.Worksheets.Add("Relationship-Code");

                List<ExCFUploadMapping> _ListMaster1 = new List<ExCFUploadMapping>();
                _ListMaster1.Add(new ExCFUploadMapping { Caption = "Code", FieldName = "Code" });
                _ListMaster1.Add(new ExCFUploadMapping { Caption = "Description", FieldName = "Description" });
                _ListMaster1.Add(new ExCFUploadMapping { Caption = "Remark", FieldName = "Remark" });

                List<ClsUploadMapping> _ListData = new List<ClsUploadMapping>();
                foreach (var read in RelCode)
                {
                    _ListData.Add(new ClsUploadMapping
                    {
                        FieldName = read.Code,
                        FieldName1 = read.Description,
                        FieldName2 = read.SecDescription
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

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TEMPLATE_FAMILY.xlsx");
                }
            }
            return null;
        }

        #endregion
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
        #region private code
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

            HRFamilyObject BSM = new HRFamilyObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var resualt = DBV.HR_Family_View;
                List<HR_Family_View> listEmpFa = resualt.Where(x => x.EmpCode == EmpCode).ToList();
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
        private void DataSelector()
        {
            ViewData["RelationshipType_LIST"] = DB.HRRelationshipTypes.ToList();
            SYDataList objList = new SYDataList("SEX");
            ViewData["GENDER_SELECT"] = objList.ListData;
        }

		private async Task DataList()
        {
            HRStaffProfileObject Staff = new HRStaffProfileObject();
            ViewData["STAFF_SELECT"] = await Staff.LoadData();// ListStaff;// DB.HR_STAFF_VIEW.ToList();
        }
        #endregion
    }
}