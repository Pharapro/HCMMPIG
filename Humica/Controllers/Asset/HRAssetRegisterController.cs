using DevExpress.Web.Mvc;
using DevExpress.Web;
using Humica.Core.BS;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.Asset;
using Humica.Models.SY;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System;
using Humica.Training.DB;
using Humica.Training;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;
using System.Text.RegularExpressions;
using DevExpress.Spreadsheet;

namespace Humica.Controllers.Asset
{
    public class HRAssetRegisterController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "AM00000001";
        private const string URL_SCREEN = "/Asset/HRAssetRegister/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "AssetCode;status";
		private string PATH_FILE = "12313123123sadfsdfsdfsdf";
		HumicaDBContext DB = new HumicaDBContext();
        public HRAssetRegisterController()
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
			AssetRegisterObject BSM = new AssetRegisterObject();
			BSM.ListHeader = DB.HRAssetRegisters.AsEnumerable()
				.OrderBy(x =>
				{
					var code = x.AssetCode ?? "";
					if (string.IsNullOrEmpty(code))
						return (string.Empty, 0); 
					var parts = code.Split('-');
					if (parts.Length < 2)
						return (code, 0);
					var prefix = string.Join("-", parts.Take(parts.Length - 1));
					var suffixStr = parts.Last();
					int suffix = int.TryParse(suffixStr, out int num) ? num : 0;
					return (prefix, suffix);
				}).ToList();
			Session[Index_Sess_Obj + ActionName] = BSM;
			return View(BSM);
		}
		public ActionResult GridItemAssetRecord()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            AssetRegisterObject BSM = new AssetRegisterObject();
            BSM.ScreenId = SCREEN_ID;
            BSM.ListRecordDetail = new List<ListRecordDetail>();
            BSM.ListHeader = new List<HRAssetRegister>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemAssetRecord";
            return PartialView("GridItemAssetRecord", BSM.ListRecordDetail);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            AssetRegisterObject BSM = new AssetRegisterObject();
            BSM.ListHeader = new List<HRAssetRegister>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
		public ActionResult GridItemQtyDetail()
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm(this.KeyName);
			AssetRegisterObject BSM = new AssetRegisterObject();
			BSM.ListHeader = new List<HRAssetRegister>();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("GridItemAssetQTY", BSM.ListAssetQTYDetail);
		}
		public ActionResult EditGridItemQtyDetail(AssetRegisterQTYDetail MModel)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			AssetRegisterObject BSM = new AssetRegisterObject();
			if (ModelState.IsValid)
			{
				try
				{
					if (Session[Index_Sess_Obj + ActionName] != null)
					{
						BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];
					}
					var listSchedule = BSM.ListAssetQTYDetail.Where(w => w.ID == MModel.ID).ToList();
					if (listSchedule.Count > 0)
					{
						var objUpdate = listSchedule.FirstOrDefault();
						if (objUpdate != null) {
							objUpdate.AssetCode = MModel.AssetCode;
							objUpdate.QTY = MModel.QTY;
							objUpdate.SerialNumber = MModel.SerialNumber;
							objUpdate.Model = MModel.Model;
							Session[Index_Sess_Obj + ActionName] = BSM;
						}
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
			return PartialView("GridItemAssetQTY", BSM.ListAssetQTYDetail);
		}
		#endregion
		#region "Create"
		public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
			Session["getAssetCode"] = null;
			AssetRegisterObject BSM = new AssetRegisterObject();
            BSM.Header = new HRAssetRegister();
            BSM.ListAssetStaff = new List<HRAssetStaff>();
			BSM.ListAssetQTYDetail = new List<AssetRegisterQTYDetail>();
			BSM.Header.Status = "Active";
			BSM.Header.StatusUse = SYDocumentStatus.OPEN.ToString();
			Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(AssetRegisterObject collection)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ActionName = "Create";
            AssetRegisterObject BSM = new AssetRegisterObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Header = collection.Header;
            BSM.ScreenId = SCREEN_ID;
			List<string> savedCodes;
			string msg = BSM.CreateFixAsset(out savedCodes);
			if (msg == SYConstant.OK)
			{
				SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
				mess.DocumentNumber = string.Join(", ", savedCodes.Select(code =>
					$"<a href='{SYUrl.getBaseUrl()}{URL_SCREEN}Details?id={code}'>{code}</a>"
				));
				ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
				BSM = new AssetRegisterObject();
				BSM.Header = new HRAssetRegister();
				BSM.ListAssetQTYDetail = new List<AssetRegisterQTYDetail>();
				BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
				Session["getAssetCode"] = null;
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
        #endregion

        #region "Edit"
        public ActionResult Edit(string ID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
			Session["getAssetCode"] = null;
			ViewData[SYSConstant.PARAM_ID] = ID;
            if (ID == "null") ID = null;
            if (ID != null)
            {
                AssetRegisterObject BSM = new AssetRegisterObject();
                BSM.Header = new HRAssetRegister();
                BSM.Header = DB.HRAssetRegisters.FirstOrDefault(w => w.AssetCode == ID);
				if (BSM.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
					//var ListAssetStaff = DB.HRAssetTransfers.Where(w => w.Status == "RETURN" && w.AssetCode == BSM.Header.AssetCode).ToList();
					var ListAssetStaff = BSM.LoadRecordDetail(BSM.Header.AssetCode);
					BSM.ListRecordDetail = ListAssetStaff;
					return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, AssetRegisterObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            AssetRegisterObject BSM = new AssetRegisterObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditFixAsset(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
					Session["getAssetCode"] = null;
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
        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id == "null") id = null;
            if (id != null)
            {
                AssetRegisterObject Del = new AssetRegisterObject();
                string msg = Del.DeleteFixAsset(id);
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
			ActionName = "Create";
			UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id == "null") id = null;
            if (id != null)
            {
                AssetRegisterObject BSM = new AssetRegisterObject();
                BSM.Header = new HRAssetRegister();
                BSM.Header = DB.HRAssetRegisters.FirstOrDefault(w => w.AssetCode == id);
				if (BSM.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
					var ListAssetStaff = BSM.LoadRecordDetail(BSM.Header.AssetCode);
					BSM.ListRecordDetail = ListAssetStaff;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
		#endregion
		#region "Import & Upload"
		public ActionResult GridItemsImport()
		{
			ActionName = "Import";
			UserSession();
			UserConfListAndForm();
			DataSelector();
			AssetRegisterObject BSM = new AssetRegisterObject();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];
			}
			Session[Index_Sess_Obj + ActionName] = BSM;
			return PartialView("GridItemsImport", BSM);
		}
		public ActionResult Import()
		{
			UserSession();
			ActionName = "Import";
			UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRAssetRegister", SYSConstant.DEFAULT_UPLOAD_LIST);

			var BSM = new AssetRegisterObject();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];
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
				BSM.ListHeader = new List<HRAssetRegister>();

				if (dtHeader != null)
				{
					for (int i = 0; i < dtHeader.Rows.Count; i++)
					{
						var objHeader = new HRAssetRegister();
						objHeader.AssetCode = dtHeader.Rows[i][0].ToString();
						objHeader.AssetClassCode = dtHeader.Rows[i][1].ToString();
						objHeader.Description = dtHeader.Rows[i][2].ToString();
						objHeader.PropertyType = dtHeader.Rows[i][3].ToString();

						if (Decimal.TryParse(dtHeader.Rows[i][4].ToString(), out decimal YearValue))
						{
							objHeader.UsefulLifeYear = YearValue;
						}
						else
						{
							objHeader.UsefulLifeYear = 0m;
						}

						if (Decimal.TryParse(dtHeader.Rows[i][5].ToString(), out decimal qtyValue))
						{
							objHeader.Qty = qtyValue;
						}
						else
						{
							objHeader.Qty = 0m;
						}
						objHeader.ReceiptDate = SYSettings.getDateValue(dtHeader.Rows[i][6].ToString());

						if (Decimal.TryParse(dtHeader.Rows[i][7].ToString(), out decimal CostValue))
						{
							objHeader.AcquisitionCost = CostValue;
						}
						else
						{
							objHeader.AcquisitionCost = 0m;
						}
						objHeader.BranchCode = dtHeader.Rows[i][8].ToString();
						objHeader.BuildingCD = dtHeader.Rows[i][9].ToString();
						objHeader.Floor = dtHeader.Rows[i][10].ToString();
						objHeader.Room = dtHeader.Rows[i][11].ToString();
						objHeader.DepartmentCD = dtHeader.Rows[i][12].ToString();
						objHeader.Reason = dtHeader.Rows[i][13].ToString();
						objHeader.TagNbr = dtHeader.Rows[i][14].ToString();
						objHeader.Model = dtHeader.Rows[i][15].ToString();
						objHeader.SerialNumber = dtHeader.Rows[i][16].ToString();
						objHeader.WarrantyExpirationDate = SYSettings.getDateValue(dtHeader.Rows[i][17].ToString());
						objHeader.OPNumber = dtHeader.Rows[i][18].ToString();
						objHeader.ReceiptNumber = dtHeader.Rows[i][19].ToString();
						objHeader.BuildingNumber = dtHeader.Rows[i][20].ToString();
						objHeader.Condition = dtHeader.Rows[i][21].ToString();
						string rawValue = dtHeader.Rows[i][22].ToString().Trim();
						bool isCombone = rawValue == "1" || rawValue.Equals("true", StringComparison.OrdinalIgnoreCase);
						objHeader.IsCombone = isCombone;
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
			UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRAssetRegister", SYSConstant.DEFAULT_UPLOAD_LIST);
			SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("ASSETREGISTER"));
			sfi.ObjectTemplate = new MDUploadTemplate();
			sfi.ObjectTemplate.UploadDate = DateTime.Now;
			sfi.ObjectTemplate.ScreenId = SCREEN_ID;
			sfi.ObjectTemplate.UploadBy = user.UserName;
			sfi.ObjectTemplate.Module = "HR";
			sfi.ObjectTemplate.IsGenerate = false;

			UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
				sfi.ValidationSettings,
				sfi.uc_FileUploadComplete);

			AssetRegisterObject BSM = new AssetRegisterObject();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];
			}
			BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
			BSM.ListHeader = new List<HRAssetRegister>();

			Session[Index_Sess_Obj + ActionName] = BSM;
			return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
		}
		public ActionResult UploadList()
		{
			UserSession();
			ActionName = "Import";
			this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
			UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRAssetRegister", SYSConstant.DEFAULT_UPLOAD_LIST);

			var BSM = new AssetRegisterObject();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];
			}

			BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
			BSM.ListHeader = new List<HRAssetRegister>();

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
						AssetRegisterObject BSM = new AssetRegisterObject();
						BSDocConfg DocBatch = new BSDocConfg("BATCH_UPLOAD", DocConfType.Normal, true);

						string msg = SYConstant.OK;

						DateTime create = DateTime.Now;
						if (dtHeader.Rows.Count > 0)
						{
							BSM.ListHeader = new List<HRAssetRegister>();

							for (int i = 0; i < dtHeader.Rows.Count; i++)
							{
								var objHeader = new HRAssetRegister();
								objHeader.AssetCode = dtHeader.Rows[i][0].ToString();
								objHeader.AssetClassCode = dtHeader.Rows[i][1].ToString();
								objHeader.Description = dtHeader.Rows[i][2].ToString();
								objHeader.PropertyType = dtHeader.Rows[i][3].ToString();

								if (Decimal.TryParse(dtHeader.Rows[i][4].ToString(), out decimal YearValue))
								{
									objHeader.UsefulLifeYear = YearValue;
								}
								else
								{
									objHeader.UsefulLifeYear = 0m;
								}

								if (Decimal.TryParse(dtHeader.Rows[i][5].ToString(), out decimal qtyValue))
								{
									objHeader.Qty = qtyValue;
								}
								else
								{
									objHeader.Qty = 0m;
								}
								objHeader.ReceiptDate = SYSettings.getDateValue(dtHeader.Rows[i][6].ToString());

								if (Decimal.TryParse(dtHeader.Rows[i][7].ToString(), out decimal CostValue))
								{
									objHeader.AcquisitionCost = CostValue;
								}
								else
								{
									objHeader.AcquisitionCost = 0m;
								}
								objHeader.BranchCode = dtHeader.Rows[i][8].ToString();
								objHeader.BuildingCD = dtHeader.Rows[i][9].ToString();
								objHeader.Floor = dtHeader.Rows[i][10].ToString();
								objHeader.Room = dtHeader.Rows[i][11].ToString();
								objHeader.DepartmentCD = dtHeader.Rows[i][12].ToString();
								objHeader.Reason = dtHeader.Rows[i][13].ToString();
								objHeader.TagNbr = dtHeader.Rows[i][14].ToString();
								objHeader.Model = dtHeader.Rows[i][15].ToString();
								objHeader.SerialNumber = dtHeader.Rows[i][16].ToString();
								objHeader.WarrantyExpirationDate = SYSettings.getDateValue(dtHeader.Rows[i][17].ToString());
								objHeader.OPNumber = dtHeader.Rows[i][18].ToString();
								objHeader.ReceiptNumber = dtHeader.Rows[i][19].ToString();
								objHeader.BuildingNumber = dtHeader.Rows[i][20].ToString();
								objHeader.Condition = dtHeader.Rows[i][21].ToString();
								string rawValue = dtHeader.Rows[i][22].ToString().Trim();
								bool isCombone = rawValue == "1" || rawValue.Equals("true", StringComparison.OrdinalIgnoreCase);
								objHeader.IsCombone = isCombone;
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
					new ExCFUploadMapping { FieldName = "Asset Code" },
					new ExCFUploadMapping { FieldName = "Asset ClassCode" },
					new ExCFUploadMapping { FieldName = "Description" },
					new ExCFUploadMapping { FieldName = "PropertyType" },
					new ExCFUploadMapping { FieldName = "UsefulLife,Year\n(number)" },
					new ExCFUploadMapping { FieldName = "Quantity\n(number)" },
					new ExCFUploadMapping { FieldName = "Receipt Date\n(date)" },
					new ExCFUploadMapping { FieldName = "Org. Acquisition Cost\n(number)" },
					new ExCFUploadMapping { FieldName = "Branch Code" },
					new ExCFUploadMapping { FieldName = "Building(Code)" },
					new ExCFUploadMapping { FieldName = "Floor" },
					new ExCFUploadMapping { FieldName = "Room" },
					new ExCFUploadMapping { FieldName = "Department(Code)" },
					new ExCFUploadMapping { FieldName = "Reason" },
					new ExCFUploadMapping { FieldName = "Tag Number" },
					new ExCFUploadMapping { FieldName = "Model" },
					new ExCFUploadMapping { FieldName = "Serial Number" },
					new ExCFUploadMapping { FieldName = "Warranty Expiration Date\n(date)" },
					new ExCFUploadMapping { FieldName = "OP Number" },
					new ExCFUploadMapping { FieldName = "Receipt Number" },
					new ExCFUploadMapping { FieldName = "Building Number" },
					new ExCFUploadMapping { FieldName = "Condition" },
					new ExCFUploadMapping { FieldName = "IsCombine\n1=IsCombine" }
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
					return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AssetMaster_TEMPLATE.xlsx");
				}
			}
		}
		#endregion
		public ActionResult getAssetClass(string id, string Action)
		{
			ActionName = Action;
			AssetRegisterObject BSM = new AssetRegisterObject();

			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];
				var obj = DB.HRAssetClasses.FirstOrDefault(w => w.AssetClassCode == id);

				if (obj != null)
				{
					BSM.ListAssetQTYDetail = new List<AssetRegisterQTYDetail>();
					//string assetCode = DB.HRAssetRegisters
					//					 .Where(r => r.AssetClassCode == id)
					//					 .OrderByDescending(r => r.AssetCode)
					//					 .FirstOrDefault()?.AssetCode ?? "";
					string newAssetCode = null;
					//if (assetCode == "")
					//{
						newAssetCode = BSM.GetAssetCode(id);
					//}
					//else
					//{
					//	var match = Regex.Match(assetCode, @"^(.+?)(\d+)$");
					//	if (!match.Success)
					//	{
					//		return Json(new { MS = "Asset code format not supported" }, JsonRequestBehavior.DenyGet);
					//	}

					//	string prefix = match.Groups[1].Value;
					//	string numericPart = match.Groups[2].Value;

					//	if (!int.TryParse(numericPart, out int baseNumber))
					//	{
					//		return Json(new { MS = "Invalid numeric part in asset code" }, JsonRequestBehavior.DenyGet);
					//	}
					//     newAssetCode = $"{prefix}{(baseNumber + 1).ToString().PadLeft(numericPart.Length, '0')}";
					//}
					if (newAssetCode == "NUMBER_RANK_NE")
					{
						return Json(new { MS = "Number rank does not exist" }, JsonRequestBehavior.DenyGet);
					}
					Session["getAssetCode"] = newAssetCode;
					Session[Index_Sess_Obj + ActionName] = BSM;
					var result = new
					{
						MS = SYConstant.OK,
						AssetType = obj.AssetTypeCode,
						CheckNumberRank = obj.NumberRank,
						assetCode = newAssetCode
					};
					return Json(result, JsonRequestBehavior.DenyGet);
				}
			}

			return Json(new { MS = SYConstant.OK }, JsonRequestBehavior.DenyGet);
		}

		public ActionResult GetIsCombone(bool IsCombined, string Action)
		{
			string actionName = Action ?? "Create";
			var bsm = Session[Index_Sess_Obj + actionName] as AssetRegisterObject ?? new AssetRegisterObject();
			bsm.Header.IsCombone = IsCombined; 
			Session[Index_Sess_Obj + actionName] = bsm;
			return Json(new { MS = SYConstant.OK }, JsonRequestBehavior.DenyGet); 
		}
		public ActionResult GetQtyAssetCode(int Qty, string Action)
		{
			if (Qty <= 0)
			{
				var rss = new { MS = "Quantity must be greater than zero" };
				return Json(rss, JsonRequestBehavior.DenyGet);
			}
			string actionName = Action ?? "Create";
			var bsm = Session[Index_Sess_Obj + actionName] as AssetRegisterObject ?? new AssetRegisterObject();
			bsm.ListAssetQTYDetail = new List<AssetRegisterQTYDetail>();
			bsm.Header = bsm.Header ?? new HRAssetRegister();

			if (Session["getAssetCode"] == null)
			{
				var rss = new { MS = "Asset code does not exist" };
				return Json(rss, JsonRequestBehavior.DenyGet);
			}
			string assetCode = Session["getAssetCode"].ToString();
			var match = Regex.Match(assetCode, @"^(.+?)(\d+)$");
			if (!match.Success)
			{
				var rss = new { MS = "Asset code format not supported" };
				return Json(rss, JsonRequestBehavior.DenyGet);
			}

			string prefix = match.Groups[1].Value;            
			string numericPart = match.Groups[2].Value;      

			if (!int.TryParse(numericPart, out int baseNumber))
			{
				var rss = new { MS = "Invalid numeric part in asset code" };
				return Json(rss, JsonRequestBehavior.DenyGet);
			}

			for (var i = 1; i <= Qty; i++)
			{
				var qtyDetail = new AssetRegisterQTYDetail
				{
					ID = i,
					QTY = 1,
					AssetCode = bsm.Header.IsCombone
								? $"{assetCode}-{i}"
								: $"{prefix}{(baseNumber + i - 1).ToString().PadLeft(numericPart.Length, '0')}"
				};
				bsm.ListAssetQTYDetail.Add(qtyDetail);
			}


			Session[Index_Sess_Obj + actionName] = bsm;
			return Json(new { MS = SYConstant.OK }, JsonRequestBehavior.DenyGet);
		}
		public ActionResult ValidateAssetCode(string code, string Action)
		{
			string actionName = Action ?? "Create";
			var bsm = Session[Index_Sess_Obj + actionName] as AssetRegisterObject ?? new AssetRegisterObject();
			bsm.Header = bsm.Header ?? new HRAssetRegister();
			if (string.IsNullOrWhiteSpace(code))
			{
				var rss = new { MS = "Asset code cannot be empty" };
				return Json(rss, JsonRequestBehavior.DenyGet);
			}
			var existingAsset = DB.HRAssetRegisters.FirstOrDefault(w => w.AssetCode == code);
			if (existingAsset != null)
			{
				var rss = new { MS = "Asset code already exists" };
				return Json(rss, JsonRequestBehavior.DenyGet);
			}
			Session["getAssetCode"] = code;
			bsm.ListAssetQTYDetail = new List<AssetRegisterQTYDetail>();
			Session[Index_Sess_Obj + actionName] = bsm;
			return Json(new { MS = SYConstant.OK }, JsonRequestBehavior.DenyGet);
		}
		public ActionResult getAssetType(string id, string Action)
        {
            ActionName = Action;
            AssetRegisterObject BSM = new AssetRegisterObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];

                var obj = DB.HRApprTypes.FirstOrDefault(w => w.Code == id);
                if (obj != null)
                {
                    var result = new
                    {
                        MS = SYConstant.OK,
                        AssetType = obj.Code,
                    };
                    return Json(result, JsonRequestBehavior.DenyGet);

                }
            }

            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        #region 'Private Code'
        private void DataSelector()
        {
            SYDataList objList1 = new SYDataList("PROPERTYPE_SELECT");
            ViewData["PROPERTYPE_SELECT"] = objList1.ListData;

            SYDataList objList2 = new SYDataList("STATUSUse_SELECT");
            ViewData["STATUS_SELECT"] = objList2.ListData;
			SYDataList objList5 = new SYDataList("StatusAsset_SELECT");
			ViewData["STATUSASSET_SELECT"] = objList5.ListData;
			SYDataList objList3 = new SYDataList("BUILDING_SELECT");
			ViewData["BUILDING_SELECT"] = objList3.ListData;
			SYDataList objList4 = new SYDataList("CONDITION_SELECT");
            ViewData["CONDITION_SELECT"] = objList4.ListData;

            ViewData["FIXASSETTYPE"] = DB.HRAssetTypes.ToList();
            ViewData["FIXASSETCLASS"] = DB.HRAssetClasses.ToList();
            ViewData["FIXED_ASSET_LIST"] = DB.HRAssetRegisters.Where(w => w.IsActive == true).ToList();
            ViewData["DEPARTMENT_SELECT"] = DB.HRDepartments.ToList();

            var ListBranch = SYConstant.getBranchDataAccess();
            ListBranch.Add(new HRBranch());
            ViewData["BRANCH_SELECT"] = ListBranch;
			ViewData["ASSETLocation_SELECT"] = DB.HRAssetLocations.ToList();
		}
        #endregion
    }
}
