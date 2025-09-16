using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.BS;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.Asset;
using Humica.Models.SY;
using HUMICA.Models.Report.Asset;
using HUMICA.Models.Report.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.Asset
{
    public class HRAssetDepartmentController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "AM00000006";
        private const string URL_SCREEN = "/Asset/HRAssetDepartment/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "ID";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";

        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public HRAssetDepartmentController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }


        #region 'List' 
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);
            AssetDepartmentObject BSM = new AssetDepartmentObject();
            BSM.List_AssetDepartment_View = new List<HR_AssetDepartment_View>();
			BSM.List_AssetDepartment_View = DBV.HR_AssetDepartment_View.OrderByDescending(s=>s.ID).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
			AssetDepartmentObject BSM = new AssetDepartmentObject();
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetDepartmentObject)Session[Index_Sess_Obj + ActionName];
            }
			return PartialView(SYListConfuration.ListDefaultView, BSM.List_AssetDepartment_View);
        }
        #endregion
        #region 'Create'
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
			UserConfListAndForm(this.KeyName);
            AssetDepartmentObject BSD = new AssetDepartmentObject();
            BSD.Header = new HRAssetDepartment();
            BSD.ListHeader = new List<HRAssetDepartment>();
			BSD.ListAssetDepartmentDetail = new List<HRAssetDepartmentDetail>();
			BSD.Header.RequestDate = DateTime.Now;
			Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        [HttpPost]
		public ActionResult Create(AssetDepartmentObject collection)
		{
			ActionName = "Create";
			UserSession();
			//DataSelector();
			UserConfForm(SYActionBehavior.ADD);
			var BSM = (AssetDepartmentObject)Session[Index_Sess_Obj + ActionName];
			if (ModelState.IsValid)
			{
				if (collection.ListAssetDepartmentDetail == null || collection.ListAssetDepartmentDetail.Count == 0)
				{
					collection.ListAssetDepartmentDetail = BSM.ListAssetDepartmentDetail;
				}
				if (Session[PATH_FILE] != null)
				{
					collection.Header.Attachment = Session[PATH_FILE].ToString();
					Session[PATH_FILE] = null;
				}
				if (collection.ListAssetDepartmentDetail != null && collection.ListAssetDepartmentDetail.Count != 0 && collection.HeaderStaff != null)
				{
					List<int> savedIds;
					string msg = collection.AssignAsset(out savedIds);
					if (msg == SYConstant.OK)
					{
						SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
						mess.DocumentNumber = string.Join(", ", savedIds.Select(id =>
											  $"<a href='{SYUrl.getBaseUrl()}{URL_SCREEN}Details?id={id}'>{id}</a>"));
						Session[SYConstant.MESSAGE_SUBMIT] = mess;
						BSM = NewAssign();
						Session[Index_Sess_Obj + this.ActionName] = BSM;
						UserConfForm(ActionBehavior.SAVEGRID);
						return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
					}
					else
					{
						ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
						Session[Index_Sess_Obj + ActionName] = BSM;
						return View(BSM);
					}
				}
				else
				{
					Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
					return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
				}
				return View(collection);
			}
			Session[Index_Sess_Obj + this.ActionName] = collection;
			ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
			return View(collection);
		}
		#endregion
		#region 'Edit'
		public ActionResult Edit(string id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            AssetDepartmentObject BSM = new AssetDepartmentObject();
            UserConfListAndForm();
            int ID = Convert.ToInt32(id);
            BSM.Header = DB.HRAssetDepartments.FirstOrDefault(w => w.ID == ID);
            if (BSM.Header != null)
            {
                BSM.ListHeader = DB.HRAssetDepartments.Where(w => w.HandlerCode == BSM.Header.HandlerCode).ToList();
                BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.HandlerCode);
				BSM.ListHeaderDetail = DB.HRAssetDepartments.Where(w => w.HandlerCode == BSM.Header.HandlerCode && w.AssetCode == BSM.Header.AssetCode).ToList();
				Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MATERIAL_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, AssetDepartmentObject collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();

			AssetDepartmentObject BSM = new AssetDepartmentObject();
			BSM = (AssetDepartmentObject)Session[Index_Sess_Obj + ActionName];
            collection.Header.Attachment=BSM.Header.Attachment; 

			BSM.Header= collection.Header;
			if (Session[PATH_FILE] != null)
			{
				BSM.Header.Attachment = Session[PATH_FILE].ToString();
				Session[PATH_FILE] = null;
			}
			collection.ScreenId = SCREEN_ID;

            if (ModelState.IsValid)
            {
                string msg = collection.updAssign(id);

                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(BSM);

                }
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = id;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                Session[Index_Sess_Obj + this.ActionName] = collection;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(BSM);

            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion 
        #region 'Details'
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
			AssetDepartmentObject BSM = new AssetDepartmentObject();
            ViewData[SYConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
			int ID;
			if (!int.TryParse(id, out ID))
			{
				Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("Invalid ID format");
				return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
			}
			BSM.Header = DB.HRAssetDepartments.FirstOrDefault(w => w.ID == ID);
            if (BSM.Header == null)
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("Error");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.HandlerCode);
				BSM.ListHeaderDetail = DB.HRAssetDepartments.Where(w => w.HandlerCode == BSM.Header.HandlerCode && w.AssetCode == BSM.Header.AssetCode).ToList();
				BSM.ListHeader = DB.HRAssetDepartments.Where(w => w.HandlerCode == BSM.Header.HandlerCode).ToList();
            }
            UserConfForm(SYActionBehavior.VIEW);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion
        #region 'Delete'
        public ActionResult Delete(string id)
        {
            UserSession();
            AssetDepartmentObject BSM = new AssetDepartmentObject();
            if (id != null)
            {
                string msg = BSM.deleteAssign(id);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MS001", user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
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
            int ID = Convert.ToInt32(id);
            var SD = DB.HRAssetDepartments.FirstOrDefault(w => w.ID == ID);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    var sa = new RptAssetDepartment();
                    var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREEN_ID
                   && w.IsActive == true).ToList();
                    if (objRpt.Count > 0)
                    {
                        sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                    }
					sa.Parameters["ReferenceNum"].Value = SD.ReferenceNum;
					sa.Parameters["ReferenceNum"].Visible = false;

					Session[Index_Sess_Obj + ActionName] = sa;
                    Session[Index_Sess_Obj] = sa;
                    return PartialView("PrintForm", sa);
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
            if (Session[Index_Sess_Obj] != null)
            {
				RptAssetDepartment reportModel = (RptAssetDepartment)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
		#endregion
		#region "Import & Upload"
		public ActionResult GridItemsImport()
        {
            ActionName = "Import";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            AssetDepartmentObject BSM = new AssetDepartmentObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetDepartmentObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsImport", BSM);
        }
        public ActionResult Import()
        {
            UserSession();
            ActionName = "Import";
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRAssetDepartment", SYSConstant.DEFAULT_UPLOAD_LIST);

            var BSM = new AssetDepartmentObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetDepartmentObject)Session[Index_Sess_Obj + ActionName];
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
                BSM.ListHeader = new List<HRAssetDepartment>();

                if (dtHeader != null)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        var objHeader = new HRAssetDepartment();
                        objHeader.HandlerCode = dtHeader.Rows[i][0].ToString();
                        objHeader.HandlerName = dtHeader.Rows[i][1].ToString();
                        objHeader.AssetCode = dtHeader.Rows[i][2].ToString();
                        objHeader.AssetDescription = dtHeader.Rows[i][3].ToString();
                        objHeader.AssignDate = SYSettings.getDateValue(dtHeader.Rows[i][4].ToString());
                        objHeader.Department = dtHeader.Rows[i][5].ToString();
						objHeader.HandlerSection = dtHeader.Rows[i][6].ToString();
						objHeader.Remark = dtHeader.Rows[i][7].ToString();
						objHeader.ReferenceNum = dtHeader.Rows[i][8].ToString();
						objHeader.RequestDate = SYSettings.getDateValue(dtHeader.Rows[i][9].ToString());
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
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRAssetDepartment", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("ASSETDEPARTMENT"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "HR";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
                sfi.ValidationSettings,
                sfi.uc_FileUploadComplete);

			AssetDepartmentObject BSM = new AssetDepartmentObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetDepartmentObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            BSM.ListHeader = new List<HRAssetDepartment>();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
        }
        public ActionResult UploadList()
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRAssetDepartment", SYSConstant.DEFAULT_UPLOAD_LIST);

            var BSM = new AssetDepartmentObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetDepartmentObject)Session[Index_Sess_Obj + ActionName];
            }

            BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            BSM.ListHeader = new List<HRAssetDepartment>();

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
						AssetDepartmentObject BSM = new AssetDepartmentObject();
                        BSDocConfg DocBatch = new BSDocConfg("BATCH_UPLOAD", DocConfType.Normal, true);

                        string msg = SYConstant.OK;

                        DateTime create = DateTime.Now;
                        if (dtHeader.Rows.Count > 0)
                        {
                            BSM.ListHeader = new List<HRAssetDepartment>();

                            for (int i = 0; i < dtHeader.Rows.Count; i++)
                            {
								var objHeader = new HRAssetDepartment();
								objHeader.HandlerCode = dtHeader.Rows[i][0].ToString();
								objHeader.HandlerName = dtHeader.Rows[i][1].ToString();
								objHeader.AssetCode = dtHeader.Rows[i][2].ToString();
								objHeader.AssetDescription = dtHeader.Rows[i][3].ToString();
								objHeader.AssignDate = SYSettings.getDateValue(dtHeader.Rows[i][4].ToString());
								objHeader.Department = dtHeader.Rows[i][5].ToString();
								objHeader.HandlerSection = dtHeader.Rows[i][6].ToString();
								objHeader.Remark = dtHeader.Rows[i][7].ToString();
								objHeader.ReferenceNum = dtHeader.Rows[i][8].ToString();
								objHeader.RequestDate = SYSettings.getDateValue(dtHeader.Rows[i][9].ToString());
								BSM.ListHeader.Add(objHeader);
                            }

                            msg = BSM.Import();
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
		public ActionResult DownloadTemplate()
		{
			using (var workbook = new DevExpress.Spreadsheet.Workbook())
			{
				workbook.Worksheets[0].Name = "Asset Department";
				List<ExCFUploadMapping> _ListMaster = new List<ExCFUploadMapping>
				{
					new ExCFUploadMapping { FieldName = "HandlerCode" },
					new ExCFUploadMapping { FieldName = "HandlerName" },
					new ExCFUploadMapping { FieldName = "Asset Code" },
					new ExCFUploadMapping { FieldName = "AssetDescription" },
					new ExCFUploadMapping { FieldName = "AssignDate" },
					new ExCFUploadMapping { FieldName = "HandlerDepartment(Code)" },
					new ExCFUploadMapping { FieldName = "HandlerSection(Code)" },
					new ExCFUploadMapping { FieldName = "Remark" },
					new ExCFUploadMapping { FieldName = "ReferenceNum" },
					new ExCFUploadMapping { FieldName = "Request Date" }
				};
				Worksheet worksheet = workbook.Worksheets[0];
				ClsConstant.ExportDataToWorksheet(worksheet, _ListMaster);
				for (int i = 0; i < _ListMaster.Count; i++)
				{
					double width = Math.Max(15, _ListMaster[i].FieldName.Length * 1.2);
					worksheet.Columns[i].WidthInCharacters = width;
				}
				using (var stream = new System.IO.MemoryStream())
				{
					workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
					stream.Seek(0, System.IO.SeekOrigin.Begin);
					return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TEMPALTE_ASSETDEPARTMENT.xlsx");
				}
			}
		}
		#endregion
		#region 'Grid'
		public ActionResult GridItems()
		{
			UserSession();
			UserConfListAndForm();
			DataSelector();
			var actionNames = new[] { "Create", "Details", "Edit" };
			AssetDepartmentObject BSM = new AssetDepartmentObject();
			BSM.ListHeader = new List<HRAssetDepartment>();
			foreach (var action in actionNames)
			{
				var key = Index_Sess_Obj + action;
				if (Session[key] != null)
				{
					var obj = (AssetDepartmentObject)Session[key];
					BSM.ListHeader.AddRange(obj.ListHeader);
				}
			}
			BSM.ListHeader = BSM.ListHeader.GroupBy(x => new {
				x.ReferenceNum,
				x.AssetCode,
				x.AssetDescription,
				x.AssignDate,
				x.ReturnDate,
				x.Status,
				x.Condition,
				x.Remark
			}).Select(g => g.First()).ToList();
			return PartialView("GridItems", BSM);
		}
		#endregion
		#region 'GridItemGenaral'
		public ActionResult GridItemGenaral()
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			DataSelector();
			AssetDepartmentObject BSM = new AssetDepartmentObject();

			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetDepartmentObject)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("GridItemGenaral", BSM);
		}
		public ActionResult CreateItemGenaral(HRAssetDepartmentDetail MModel)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			AssetDepartmentObject BSM = new AssetDepartmentObject();
			try
			{
			    if (Session[Index_Sess_Obj + ActionName] != null)
				{
					BSM = (AssetDepartmentObject)Session[Index_Sess_Obj + ActionName];
				}
				if (MModel != null && !string.IsNullOrEmpty(MModel.AssetCode))
				{
				    var isDuplicate = BSM.ListAssetDepartmentDetail.Any(x => x.AssetCode == MModel.AssetCode);
					if (isDuplicate)
					{
					    ViewData["EditError"] = "Asset Code already exists!";
					}
					else
					{
						BSM.ListAssetDepartmentDetail.Add(MModel);
						Session[Index_Sess_Obj + ActionName] = BSM;
					}
				}
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
			}
			Session[Index_Sess_Obj + ActionName] = BSM;
			return PartialView("GridItemGenaral", BSM);
		}
		//public ActionResult EditItemGenaral(HRAssetDepartmentDetail MModel)
		//{
		//	ActionName = "Create";
		//	UserSession();
		//	UserConfListAndForm();
		//	DataSelector();
		//	AssetDepartmentObject BSM = new AssetDepartmentObject();
		//	if (Session[Index_Sess_Obj + ActionName] != null)
		//	{
		//		BSM = (AssetDepartmentObject)Session[Index_Sess_Obj + ActionName];
		//		var objCheck = BSM.ListAssetDepartmentDetail.Where(w => w.EmpCode == MModel.EmpCode && w.AssetCode == MModel.AssetCode).ToList();
		//		if (objCheck.Count > 0)
		//		{
		//			objCheck.First().AssetCode = MModel.AssetCode;
		//			objCheck.First().AssetDescription = MModel.AssetDescription;
		//		}
		//		else
		//		{
		//			ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
		//		}
		//		Session[Index_Sess_Obj + ActionName] = BSM;
		//	}
		//	return PartialView("GridItemGenaral", BSM);
		//}
		public ActionResult DeleteItemGenaral(string AssetCode)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			DataSelector();
			AssetDepartmentObject BSM = new AssetDepartmentObject();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetDepartmentObject)Session[Index_Sess_Obj + ActionName];
				var objCheck = BSM.ListAssetDepartmentDetail.Where(w => w.AssetCode == AssetCode).ToList();
				if (objCheck.Count > 0)
				{
					BSM.ListAssetDepartmentDetail.Remove(objCheck.First());
				}
				else
				{
					ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
				}
				Session[Index_Sess_Obj + ActionName] = BSM;
			}
			return PartialView("GridItemGenaral", BSM);
		}
		#endregion
		#region private code
		public ActionResult ShowDataEmp(string EmpCode, string Action)
        {
            ActionName = Action;
            AssetDepartmentObject BSM = new AssetDepartmentObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetDepartmentObject)Session[Index_Sess_Obj + ActionName];
            }

            var Stafff = DBV.HR_STAFF_VIEW.ToList();
            HR_STAFF_VIEW EmpStaff = Stafff.SingleOrDefault(w => w.EmpCode == EmpCode);
            var _ListAsset = DB.HRAssetDepartments.Where(w => w.HandlerCode == EmpCode).ToList();
            BSM.ListHeader = _ListAsset;
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
				return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
			return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private AssetDepartmentObject NewAssign()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();

			AssetDepartmentObject BSD = new AssetDepartmentObject();
            BSD.Header = new HRAssetDepartment();
            BSD.ListHeader = new List<HRAssetDepartment>();
            BSD.Header.AssignDate = DateTime.Now;
            BSD.Header.Status = SYDocumentStatus.ASSIGN.ToString();

            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = BSD;
            return BSD;
        }
        private void DataSelector()
        {
            string status_Open = SYDocumentStatus.OPEN.ToString();
            ViewData["ASSETCODE_SELECT"] = DB.HRAssetRegisters.Where(w => w.StatusUse == status_Open).ToList();
			ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.Where(s => s.Status == "A")
				.Select(s => new { s.EmpCode, s.AllName, s.OthAllName }).ToList();
			var ObjAsset = new SYDataList("ASSET_SELECT");
            ViewData["ASSET_SELECT"] = ObjAsset.ListData;
			ViewData["Department_SELECT"] = DB.HRDepartments.ToList();
			ViewData["Section_SELECT"] = DB.HRSections.ToList();
		}
        #endregion
    }
}
