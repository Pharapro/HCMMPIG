using Humica.EF.Models.SY;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Humica.EF;
using Humica.Logic.Asset;
using DevExpress.Web.Mvc;
using Humica.EF.MD;
using Humica.Core.DB;
using System.Data;
using HUMICA.Models.Report.Asset;
using DevExpress.XtraReports.UI;

namespace Humica.Controllers.Asset
{
    public class HRAssetTransferController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "AM00000004";
        private const string URL_SCREEN = "/Asset/HRAssetTransfer/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "ID";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
		private string PATH_FILE_MUTI = "12313123123sadfsdfsdfsdf";
		HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public HRAssetTransferController()
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
            AssetTransferObject BSM = new AssetTransferObject();
            BSM.ListHeader = new List<HRAssetTransfer>();
            BSM.ListStaffAsset = new List<HRAssetStaff>();
			BSM.ListAssetDepartment = new List<HRAssetDepartment>();
			BSM.ListAssetNoneStaff = new List<HRAssetNoneStaff>();
			BSM.Filter = 0;
			BSM.ListHeader = DB.HRAssetTransfers.Where(w => w.Status == "RETURN").OrderByDescending(w => w.ID).ToList();
            BSM.ListAssetTransferPending = BSM.getDataPending();
			Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
		[HttpPost]
		public ActionResult Index(AssetTransferObject BSM)
		{
			ActionName = "Index";
			UserSession();
			DataSelector();
			UserConfListAndForm(this.KeyName);
			BSM.ListHeader = new List<HRAssetTransfer>();
			BSM.ListStaffAsset = new List<HRAssetStaff>();
			BSM.ListAssetDepartment = new List<HRAssetDepartment>();
			BSM.ListAssetNoneStaff = new List<HRAssetNoneStaff>();
			BSM.ListHeader = DB.HRAssetTransfers.Where(w => w.Status == "RETURN").OrderByDescending(w => w.ID).ToList();

			if (BSM.Filter == 1)
			{
				BSM.ListStaffAsset = DB.HRAssetStaffs.Where(w => w.Status == "ASSIGN").OrderByDescending(x => x.ReferenceNum).ToList();
			}
			else if (BSM.Filter == 2)
            {
			    BSM.ListAssetDepartment = DB.HRAssetDepartments.Where(w => w.Status == "ASSIGN").OrderByDescending(x => x.ReferenceNum).ToList();
			}
			else if (BSM.Filter == 3)
			{
				BSM.ListAssetNoneStaff = DB.HRAssetNoneStaffs.Where(w => w.Status == "ASSIGN").OrderByDescending(x => x.ReferenceNum).ToList();
			}
			else
            {
				BSM.ListAssetTransferPending = BSM.getDataPending();
			}
			Session[Index_Sess_Obj + ActionName] = BSM;

			return View(BSM);
		}
		public ActionResult PartialList()
        {
            ActionName = "Index";
            AssetTransferObject BSM = new AssetTransferObject();
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            AssetTransferObject BSM = new AssetTransferObject();
            BSM.ListHeader = new List<HRAssetTransfer>();
            BSM.ListStaffAsset = new List<HRAssetStaff>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListStaffAsset);
        }
		public ActionResult PartialProcessNoneStaff()
		{
			ActionName = "Index";
			UserSession();
			DataSelector();
			UserConfListAndForm(this.KeyName);
			AssetTransferObject BSM = new AssetTransferObject();
			BSM.ListHeader = new List<HRAssetTransfer>();
			BSM.ListAssetNoneStaff = new List<HRAssetNoneStaff>();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("PartialProcessNoneStaff", BSM.ListAssetNoneStaff);
		}
		public ActionResult PartialProcessDEPT()
		{
			ActionName = "Index";
			UserSession();
			DataSelector();
			UserConfListAndForm(this.KeyName);
			AssetTransferObject BSM = new AssetTransferObject();
			BSM.ListHeader = new List<HRAssetTransfer>();
			BSM.ListAssetDepartment = new List<HRAssetDepartment>();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("PartialProcessDEPT", BSM.ListAssetDepartment);
		}
		public ActionResult PartialProcessAll()
		{
			ActionName = "Index";
			UserSession();
			DataSelector();
			UserConfListAndForm(this.KeyName);
			AssetTransferObject BSM = new AssetTransferObject();
			BSM.ListHeader = new List<HRAssetTransfer>();
			BSM.ListAssetTransferPending = new List<AssetTransferPending>();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("PartialProcessAll", BSM.ListAssetTransferPending);
		}
		#endregion
		[HttpPost]
        public ActionResult GetData(DateTime FromDate, int Period, decimal? DedAmount, decimal Amount , string ActionName="Create")
        {
            this.ActionName = ActionName;
            Session["Index"] = ActionName;
            UserSession();
            UserConfListAndForm();
            if (Period > 0) Period -= 1;
            DateTime ToDate = FromDate.AddMonths(Period);
            AssetTransferObject BSM = new AssetTransferObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
                BSM.ListDed = new List<PREmpDeduc>();
                Decimal _DedAmount = (decimal)DedAmount;
                int CountMount = (((ToDate.Year - FromDate.Year) * 12) + ToDate.Month - FromDate.Month);
                int C_Month = (((ToDate.Year - FromDate.Year) * 12) + ToDate.Month - FromDate.Month) + 1;
                int Line = 0;

                for (var i = 0; i <= CountMount; i++)
                {

                    Line += 1;
                    var Ded = new PREmpDeduc();
                    if (Line == C_Month) Amount = _DedAmount;
                    Ded.FromDate = FromDate.AddMonths(i);
                    Ded.ToDate = FromDate.AddMonths(i);
                    if (C_Month == 0) Ded.Amount = Amount;
                    else Ded.Amount = Amount;
                    Ded.TranNo = Line;
                    Ded.StatusAssetDed = SYDocumentStatus.OPEN.ToString();
                    _DedAmount -= Amount;
                    if (_DedAmount < 0)
                    {
                        var rs1 = new { MS = "Invalid Period" };
                        return Json(rs1, JsonRequestBehavior.DenyGet);
                    }
                    if (Ded.Amount > 0)
                        BSM.ListDed.Add(Ded);
                }
                var result = new
                {
                    MS = SYConstant.OK,
                    ToDate = ToDate
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        #region 'Create'
		public ActionResult CreateMultiRef(string id)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			DataSelector();
			var BSM = Session[Index_Sess_Obj + "Index"] as AssetTransferObject ?? new AssetTransferObject();
			BSM.Header = new HRAssetTransfer();
			string msg = BSM.GetDataCreateMuti(id);
			ViewData[SYConstant.PARAM_ID] = BSM.Filter;
			if (msg == SYConstant.OK)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
				var uniqueTypes = new HashSet<string>();
				foreach (var types in BSM.ListHeaderDetail)
				{
					uniqueTypes.Add(types.Type);
				}
				ViewData[SYConstant.TITLE] = uniqueTypes.FirstOrDefault();
				return View(BSM);
            }
            else
            {
				Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
				ViewData[SYConstant.PARAM_ID] = BSM.Filter;
				return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
			}
		}
        [HttpPost]
        public ActionResult CreateMultiRef(AssetTransferObject obj)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);
            AssetTransferObject BSM = new AssetTransferObject();
            BSM.Header = new HRAssetTransfer();
            BSM.HeaderDed = new PREmpDeduc();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = obj.Header;
            }
            if (Session[PATH_FILE] != null)
            {
                obj.Header.Attachment = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            BSM.ScreenId = SCREEN_ID;
            List<int> savedIds;
            string msg = BSM.CreateMuti(out savedIds);
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang); 
				mess.DocumentNumber = string.Join(", ", savedIds.Select(id =>
											  $"<a href='{SYUrl.getBaseUrl()}{URL_SCREEN}Details?id={id}'>{id}</a>"));
				Session[SYConstant.MESSAGE_SUBMIT] = mess;
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
		#endregion
		#region 'Edit'
		public ActionResult Edit(string id)
        {
            ActionName = "Edit";
            Session["Index"] = ActionName;
            UserSession();
            DataSelector();
            AssetTransferObject BSM = new AssetTransferObject();
            UserConfListAndForm();

            int ID = Convert.ToInt32(id);
            BSM.Header = DB.HRAssetTransfers.FirstOrDefault(w => w.ID == ID);
            if (BSM.Header != null)
            {
				ViewData[SYConstant.LAST_RECORD] = BSM.Header.TransferType;
				BSM.ListHeader = DB.HRAssetTransfers.Where(w => w.EmpCode == BSM.Header.EmpCode).ToList();
                BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                BSM.ListDed = DB.PREmpDeducs.Where(w => w.AssetTransferID == BSM.Header.ID).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MATERIAL_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, AssetTransferObject collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            AssetTransferObject BSM = new AssetTransferObject();
			int checkEdit = 0;
			if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
                collection.Header.Attachment=BSM.Header.Attachment; 
            }
                //if (ModelState.IsValid)
                //{

                if (Session[PATH_FILE] != null)
                {
                    collection.Header.Attachment = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                
                BSM.Header=collection.Header;
                BSM.ScreenId = SCREEN_ID;
                
                string msg = BSM.Update(id, checkEdit);
                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(collection);

                }

                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = id;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber; ;
                Session[Index_Sess_Obj + this.ActionName] = collection;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
			    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

			//}
			//Session[Index_Sess_Obj + this.ActionName] = BSM;
			//ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
			//return View(BSM);

		}
        #endregion 
        #region 'Details'
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
			AssetTransferObject BSM = new AssetTransferObject();
            ViewData[SYConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
			int ID;
			if (!int.TryParse(id, out ID))
			{
				Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("Invalid ID format");
				return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
			}
			BSM.Header = DB.HRAssetTransfers.FirstOrDefault(w => w.ID == ID);
			BSM.ListDed = DB.PREmpDeducs.Where(w => w.AssetTransferID == BSM.Header.ID).ToList();
            if(BSM.ListDed.Count == 0)
            {
                BSM.Header.FromDate = DateTime.Now;
                BSM.Header.ToDate = DateTime.Now;
                BSM.Header.Amount = 0;
                BSM.Header.Period = 1;
                BSM.Header.DedAmount = 0;
            }
            if (BSM.Header == null)
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("Error");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
				ViewData[SYConstant.LAST_RECORD] = BSM.Header.TransferType;
				BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                BSM.ListHeader = DB.HRAssetTransfers.Where(w => w.EmpCode == BSM.Header.EmpCode).ToList();
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
            AssetTransferObject BSM = new AssetTransferObject();
            if (id != null)
            {
                string msg = BSM.Delete(id);

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
		#region HeaderDetail
		public ActionResult GridHeaderDetail()
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			DataSelector();
			AssetTransferObject BSM = new AssetTransferObject();
			BSM.ListHeaderDetail = new List<AssetTransferDetail>();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = Session[Index_Sess_Obj + ActionName] as AssetTransferObject;
			}
			return PartialView("GridHeaderDetail", BSM.ListHeaderDetail);
		}
		public ActionResult EditGridHeaderDetail(AssetTransferDetail MModel)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			AssetTransferObject BSM = new AssetTransferObject();
			if (ModelState.IsValid)
			{
				try
				{
					if (Session[Index_Sess_Obj + ActionName] != null)
					{
						BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
					}
					var listSchedule = BSM.ListHeaderDetail.Where(w => w.AssetCode == MModel.AssetCode).ToList();
					if (listSchedule.Count > 0)
					{
						var objUpdate = listSchedule.FirstOrDefault();
                        if (objUpdate != null) {
							objUpdate.AssignDate = MModel.AssignDate;
							objUpdate.Receiver = MModel.Receiver;
							objUpdate.ReturnDate = MModel.ReturnDate;
							objUpdate.IsDedSalary = MModel.IsDedSalary;
							objUpdate.Condition = MModel.Condition;
							objUpdate.Location = MModel.Location;
							objUpdate.Remark = MModel.Remark;
                            if (Session[PATH_FILE_MUTI] != null)
                            {
                                objUpdate.AttachFile = Session[PATH_FILE_MUTI].ToString();
                                Session[PATH_FILE_MUTI] = null;
							}
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
			return PartialView("GridHeaderDetail", BSM.ListHeaderDetail);
		}
		public ActionResult DeleteGridHeaderDetail(AssetTransferDetail MModel)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			DataSelector();
			AssetTransferObject BSM = new AssetTransferObject();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
				if (BSM.ListHeaderDetail.Count > 0)
				{
					var objCheck = BSM.ListHeaderDetail.FirstOrDefault(w => w.AssetCode == MModel.AssetCode);
					BSM.ListHeaderDetail.Remove(objCheck);
					Session[Index_Sess_Obj + ActionName] = BSM;
				}
			}
			return PartialView("GridHeaderDetail", BSM.ListHeaderDetail);
		}
		#endregion
		public ActionResult GridEdit(PREmpDeduc MModel)
        {
            ActionName = Session["Index"].ToString();
            UserSession();
            UserConfListAndForm();
            AssetTransferObject BSM = new AssetTransferObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var DBU = new HumicaDBContext();
                    string Open = SYDocumentStatus.OPEN.ToString();
                    var ListHeaderD = BSM.ListDed.Where(w => w.TranNo == MModel.TranNo).ToList();
                    if (ListHeaderD.Where(w => w.StatusAssetDed == Open).Any())
                    {
                        if (ListHeaderD.Count > 0)
                        {
                            var objUpdate = ListHeaderD.First();
                            objUpdate.Amount = MModel.Amount;
                            objUpdate.Remark = MModel.Remark;
                            Session[Index_Sess_Obj + ActionName] = BSM;
                        }
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("LOAN_READY");
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
            DataSelector();
            return PartialView("GridItemDetails", BSM);
        }
        public ActionResult GridItemDetails()
        {
            ActionName = Session["Index"].ToString();
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            AssetTransferObject BSM = new AssetTransferObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemDetails";
            return PartialView("GridItemDetails", BSM);
        }
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
            var SD = DB.HRAssetTransfers.FirstOrDefault(w => w.ID == ID);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
					XtraReport sa = new XtraReport();
					if (SD.TransferType== "IsStaff")
                    {
						sa = new RptAssetStaffReturn();
					}
					else if (SD.TransferType == "IsDept")
					{
						sa = new RptAssetDepartmentReturn();
					}
					else if (SD.TransferType == "IsNone")
					{
						sa = new RptAssetNoneStaffReturn();
					}
                    else
                    {
						sa = new RptAssetTransfer();
					}
                    var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREEN_ID
                   && w.IsActive == true && w.DocType == SD.TransferType).ToList();
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
				XtraReport reportModel = (XtraReport)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
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
		public ActionResult UploadControlCallbackActionImageMuti()
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
			UploadControlExtension.GetUploadedFiles("AttachFiles", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
			Session[PATH_FILE_MUTI] = objFile.ObjectTemplate.UpoadPath;
			return null;
		}
		private void DataSelector()
        {
            string DEDType = "Ded";
            string status_Open = SYDocumentStatus.OPEN.ToString();
            ViewData["ASSETCODE_SELECT"] = DB.HRAssetRegisters.Where(w => w.StatusUse == status_Open).ToList();
            //ViewData["EMP_SELECT"] = DBV.HR_STAFF_VIEW.ToList();
            var ObjAsset = new SYDataList("ASSET_SELECT");
            ViewData["ASSET_SELECT"] = ObjAsset.ListData;
            ViewData["DED_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == DEDType).ToList();
            ViewData["Staff_SELECT"] = DB.HRStaffProfiles.ToList();
			ViewData["ASSETLocation_SELECT"] = DB.HRAssetLocations.ToList();
		}
        //#endregion
    }
}
