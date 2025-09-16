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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.Asset
{
    public class HRAssetNoneStaffRecordController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "AM00000008";
        private const string URL_SCREEN = "/Asset/HRAssetNoneStaffRecord/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
		private string PATH_FILE = "KH12313123123sadfsdfsdfsdf";
		private string ActionName;
        private string KeyName = "HandlerCode";
        HumicaDBContext DB = new HumicaDBContext();
		HumicaDBViewContext DBV = new HumicaDBViewContext();
		public HRAssetNoneStaffRecordController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
			AssetNoneStaffRecordObject BSM = new AssetNoneStaffRecordObject();
            BSM.ListNoneStaffRecord = DB.HRAssetNoneStaffRecords.ToList();
            return View(BSM);
        }
        #endregion
        public ActionResult Gridview()
        {
            UserSession();
            UserConfListAndForm();
			AssetNoneStaffRecordObject BSM = new AssetNoneStaffRecordObject();
			BSM.ListNoneStaffRecord = DB.HRAssetNoneStaffRecords.ToList();
			return PartialView("Gridview", BSM.ListNoneStaffRecord);
        }
		//create
		[HttpPost, ValidateInput(false)]
        public ActionResult Create(HRAssetNoneStaffRecord ModelObject)
        {
            UserSession();
            UserConfListAndForm();
			AssetNoneStaffRecordObject BSM = new AssetNoneStaffRecordObject();
			if (ModelState.IsValid)
            {
                try
                {
					DB.HRAssetNoneStaffRecords.Add(ModelObject);
                    int row = DB.SaveChanges();
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
			BSM.ListNoneStaffRecord = DB.HRAssetNoneStaffRecords.ToList();
			return PartialView("Gridview", BSM.ListNoneStaffRecord);
		}
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(HRAssetNoneStaffRecord ModelObject)
        {
            UserSession();
            UserConfListAndForm();
			AssetNoneStaffRecordObject BSM = new AssetNoneStaffRecordObject();
			if (ModelState.IsValid)
            {
				try
				{
					var objUpdate = DB.HRAssetNoneStaffRecords.Find(ModelObject.HandlerCode);
					if (objUpdate != null)
					{
						objUpdate.HandlerName = ModelObject.HandlerName;
						objUpdate.Company = ModelObject.Company;
						objUpdate.Position = ModelObject.Position;
						objUpdate.PhoneNumber = ModelObject.PhoneNumber;
						objUpdate.Commune = ModelObject.Commune;
						objUpdate.District = ModelObject.District;
						objUpdate.Province = ModelObject.Province;
						int row = DB.SaveChanges();
					}
					else
					{
						ViewData["EditError"] = "Record not found.";
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
			BSM.ListNoneStaffRecord = DB.HRAssetNoneStaffRecords.ToList();
			return PartialView("Gridview", BSM.ListNoneStaffRecord);
		}
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(string HandlerCode)
        {
            UserSession();
            UserConfListAndForm();
			AssetNoneStaffRecordObject BSM = new AssetNoneStaffRecordObject();
			if (HandlerCode != null)
            {
                try
                {
                    var obj = DB.HRAssetNoneStaffRecords.FirstOrDefault(w => w.HandlerCode == HandlerCode);
                    if (obj != null)
                    {
                        DB.HRAssetNoneStaffRecords.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListNoneStaffRecord = DB.HRAssetNoneStaffRecords.ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
			return PartialView("Gridview", BSM.ListNoneStaffRecord);
		}
		#region "Import & Upload"
		public ActionResult GridItemsImport()
		{
			ActionName = "Import";
			UserSession();
			UserConfListAndForm();
			AssetNoneStaffRecordObject BSM = new AssetNoneStaffRecordObject();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetNoneStaffRecordObject)Session[Index_Sess_Obj + ActionName];
			}
			Session[Index_Sess_Obj + ActionName] = BSM;
			return PartialView("GridItemsImport", BSM);
		}
		public ActionResult Import()
		{
			UserSession();
			ActionName = "Import";
			UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRAssetNoneStaffRecord", SYSConstant.DEFAULT_UPLOAD_LIST);
			var BSM = new AssetNoneStaffRecordObject();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetNoneStaffRecordObject)Session[Index_Sess_Obj + ActionName];
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
				BSM.ListNoneStaffRecord = new List<HRAssetNoneStaffRecord>();
				if (dtHeader != null)
				{
					for (int i = 0; i < dtHeader.Rows.Count; i++)
					{
						var objHeader = new HRAssetNoneStaffRecord();
						objHeader.HandlerCode = dtHeader.Rows[i][0].ToString();
						objHeader.HandlerName = dtHeader.Rows[i][1].ToString();
						objHeader.Company = dtHeader.Rows[i][2].ToString();
						objHeader.Position = dtHeader.Rows[i][3].ToString();
						objHeader.PhoneNumber = dtHeader.Rows[i][4].ToString();
						objHeader.Commune = dtHeader.Rows[i][5].ToString();
						objHeader.District = dtHeader.Rows[i][6].ToString();
						objHeader.Province = dtHeader.Rows[i][7].ToString();
						BSM.ListNoneStaffRecord.Add(objHeader);
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
			UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRAssetNoneStaffRecord", SYSConstant.DEFAULT_UPLOAD_LIST);
			SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("NONESTAFF"));
			sfi.ObjectTemplate = new MDUploadTemplate();
			sfi.ObjectTemplate.UploadDate = DateTime.Now;
			sfi.ObjectTemplate.ScreenId = SCREEN_ID;
			sfi.ObjectTemplate.UploadBy = user.UserName;
			sfi.ObjectTemplate.Module = "HR";
			sfi.ObjectTemplate.IsGenerate = false;

			UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
				sfi.ValidationSettings,
				sfi.uc_FileUploadComplete);

			AssetNoneStaffRecordObject BSM = new AssetNoneStaffRecordObject();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetNoneStaffRecordObject)Session[Index_Sess_Obj + ActionName];
			}
			BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
			BSM.ListNoneStaffRecord = new List<HRAssetNoneStaffRecord>();
			Session[Index_Sess_Obj + ActionName] = BSM;
			return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
		}
		public ActionResult UploadList()
		{
			UserSession();
			ActionName = "Import";
			this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
			UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRAssetNoneStaffRecord", SYSConstant.DEFAULT_UPLOAD_LIST);
			var BSM = new AssetNoneStaffRecordObject();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetNoneStaffRecordObject)Session[Index_Sess_Obj + ActionName];
			}
			BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
			BSM.ListNoneStaffRecord = new List<HRAssetNoneStaffRecord>();
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
						AssetNoneStaffRecordObject BSM = new AssetNoneStaffRecordObject();
						BSDocConfg DocBatch = new BSDocConfg("BATCH_UPLOAD", DocConfType.Normal, true);
						string msg = SYConstant.OK;
						DateTime create = DateTime.Now;
						if (dtHeader.Rows.Count > 0)
						{
							BSM.ListNoneStaffRecord = new List<HRAssetNoneStaffRecord>();
							for (int i = 0; i < dtHeader.Rows.Count; i++)
							{
								var objHeader = new HRAssetNoneStaffRecord();
								objHeader.HandlerCode = dtHeader.Rows[i][0].ToString();
								objHeader.HandlerName = dtHeader.Rows[i][1].ToString();
								objHeader.Company = dtHeader.Rows[i][2].ToString();
								objHeader.Position = dtHeader.Rows[i][3].ToString();
								objHeader.PhoneNumber = dtHeader.Rows[i][4].ToString();
								objHeader.Commune = dtHeader.Rows[i][5].ToString();
								objHeader.District = dtHeader.Rows[i][6].ToString();
								objHeader.Province = dtHeader.Rows[i][7].ToString();
								BSM.ListNoneStaffRecord.Add(objHeader);
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
						else
						{
							Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("NO_DATA", user.Lang);
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
				workbook.Worksheets[0].Name = "Asset Master";
				List<ExCFUploadMapping> _ListMaster = new List<ExCFUploadMapping>
				{
					new ExCFUploadMapping { FieldName = "HandlerCode" },
					new ExCFUploadMapping { FieldName = "HandlerName" },
					new ExCFUploadMapping { FieldName = "Company" },
					new ExCFUploadMapping { FieldName = "Position" },
					new ExCFUploadMapping { FieldName = "PhoneNumber" },
					new ExCFUploadMapping { FieldName = "Commune" },
					new ExCFUploadMapping { FieldName = "District" },
					new ExCFUploadMapping { FieldName = "Province" },
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
					return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NoneStaffRecord_TEMPLATE.xlsx");
				}
			}
		}
		#endregion
	}
}