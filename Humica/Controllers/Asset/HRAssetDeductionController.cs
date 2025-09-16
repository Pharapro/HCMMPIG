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
using Humica.Models.Report.Asset;

namespace Humica.Controllers.Asset
{

    public class HRAssetDeductionController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "AM00000005";
        private const string URL_SCREEN = "/Asset/HRAssetDeduction/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "ID";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";

        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public HRAssetDeductionController()
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
            var listDedSalary = DB.HRAssetTransfers.Where(s => s.IsDedSalary == true && s.Amount != null);
            var listPending = DB.HRAssetTransfers.Where(s => s.IsDedSalary == true && s.Amount == null);
			BSM.ListHeader = listDedSalary.OrderByDescending(s => s.ID).ToList();
			BSM.ListHeaderPending = listPending.ToList();
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
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListHeaderPending);
        }
        #endregion
        [HttpPost]
        public ActionResult GetData(DateTime FromDate, int Period, decimal? DedAmount, decimal Amount, string ActionName="CREATE")
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

		public ActionResult GetDataUpdate(DateTime FromDate, int Period, decimal? DedAmount, decimal Amount, string ActionName)
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
				//BSM.ListDed = new List<PREmpDeduc>();
				Decimal _DedAmount = (decimal)DedAmount;
				int CountMount = (((ToDate.Year - FromDate.Year) * 12) + ToDate.Month - FromDate.Month);
				int C_Month = (((ToDate.Year - FromDate.Year) * 12) + ToDate.Month - FromDate.Month) + 1;
				int Line = 0;
				// 🔹 Get the highest existing TranNo to ensure unique numbers
				long maxTranNo = (DB.PREmpDeducs.Any()) ? DB.PREmpDeducs.Max(s => s.TranNo) : 0;
				if (BSM.ListDed.Any())
				{
					maxTranNo = Math.Max(maxTranNo, BSM.ListDed.Max(d => d.TranNo));
				}
				for (var i = 0; i <= CountMount; i++)
				{

					Line += 1;
					var Ded = new PREmpDeduc();
					if (Line == C_Month) Amount = _DedAmount;
					Ded.FromDate = FromDate.AddMonths(i);
					Ded.ToDate = FromDate.AddMonths(i);
					if (C_Month == 0) Ded.Amount = Amount;
					else Ded.Amount = Amount;

					if (i < BSM.ListDed.Count)
					{
						Ded.TranNo = BSM.ListDed[i].TranNo;  
					}
					else
					{
						Ded.TranNo = ++maxTranNo;
					}

					Ded.StatusAssetDed = SYDocumentStatus.OPEN.ToString();
					_DedAmount -= Amount;
					if (_DedAmount < 0)
					{
						var rs1 = new { MS = "Invalid Period" };
						return Json(rs1, JsonRequestBehavior.DenyGet);
					}
					if (Ded.Amount > 0)
                    {
						
						if (i < BSM.ListDed.Count)
						{
							BSM.ListDed[i] = Ded;
						}
						else
						{
							BSM.ListDed.Add(Ded);
						}
                    }
				}
				// ✅ Trim excess items from BSM.ListDed
				if (BSM.ListDed.Count > CountMount + 1)
				{
					BSM.ListDed.RemoveRange(CountMount + 1, BSM.ListDed.Count - (CountMount + 1));
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
		public ActionResult Create(int ID)
        {
            ActionName = "Create";
            Session["Index"] = ActionName;
            UserSession();
            UserConfListAndForm();
            DataSelector();

            AssetTransferObject BSM = new AssetTransferObject();
            BSM.Header = new HRAssetTransfer();
            BSM.ListDed = new List<PREmpDeduc>();
            BSM.Header.DedType = "ASD";
			//if (Session[Index_Sess_Obj + ActionName] != null)
			//{
			//    BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
			//    BSM.Header = new HRAssetTransfer();
			//}
			if (ID == null) return View(BSM);
            var AssetStaff = DB.HRAssetTransfers.Where(w => w.ID == ID).FirstOrDefault();
            BSM.Header.EmpCode = AssetStaff.EmpCode;
            BSM.Header.EmployeName = AssetStaff.EmployeName;
            BSM.Header.AssetCode = AssetStaff.AssetCode;
            BSM.Header.AssetDescription = AssetStaff.AssetDescription;
            BSM.Header.AssignDate = AssetStaff.AssignDate;
            BSM.Header.Status = AssetStaff.Status;
			BSM.Header.ReturnDate = AssetStaff.ReturnDate;
			BSM.Header.IsDedSalary = AssetStaff.IsDedSalary;
			BSM.Header.Condition = AssetStaff.Condition;
			BSM.Header.Attachment = AssetStaff.Attachment;
			BSM.Header.Remark = AssetStaff.Remark;

            BSM.Header.FromDate = DateTime.Now;
			BSM.Header.ToDate = DateTime.Now;
            BSM.Header.Amount = 0;
            BSM.Header.Period = 1;
            BSM.Header.DedAmount = 0;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
		}
		[HttpPost]
        public ActionResult Create(AssetTransferObject obj,int ID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);
            AssetTransferObject BSM = new AssetTransferObject();
            BSM.Header = new HRAssetTransfer();
            BSM.HeaderDed = new PREmpDeduc();
            bool getIsDud =false;
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
			    string msg = BSM.UpdateAmoutDeu(ID.ToString());
			    if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = ID.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            //}
            //ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
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
            int checkEdit = 1;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AssetTransferObject)Session[Index_Sess_Obj + ActionName];
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
                
                string msg = BSM.Update(id,checkEdit);
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
            int ID = Convert.ToInt32(id);
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
					if (SD.TransferType == "IsNone")
					{
						sa = new RptAssetDedutionNoneStaff();
					}
                    else
                    {
					   sa = new RptAssetDedution();
					}
                    var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREEN_ID
                    && w.IsActive == true && w.DocType == SD.TransferType).ToList();
                    if (objRpt.Count > 0)
                    {
                        sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                    }
                    sa.Parameters["ID"].Value = SD.ID;
                    sa.Parameters["ID"].Visible = false;

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
				RptAssetDedution reportModel = (RptAssetDedution)Session[Index_Sess_Obj];
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
        private void DataSelector()
        {
            string DEDType = "Ded";
            string status_Open = SYDocumentStatus.OPEN.ToString();
            ViewData["ASSETCODE_SELECT"] = DB.HRAssetRegisters.Where(w => w.StatusUse == status_Open).ToList();
            //ViewData["EMP_SELECT"] = DBV.HR_STAFF_VIEW.ToList();
            var ObjAsset = new SYDataList("ASSET_SELECT");
            ViewData["ASSET_SELECT"] = ObjAsset.ListData;
            ViewData["DED_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == DEDType && w.Code== "ASD").ToList();
        }
        //#endregion
    }
}
