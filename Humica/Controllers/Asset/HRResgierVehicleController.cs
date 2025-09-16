using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Logic.HR
{
    public class HRResgierVehicleController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RV0000002";
        private const string URL_SCREEN = "/Asset/HRResgierVehicle/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public HRResgierVehicleController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            DB.Database.CommandTimeout = 1800;
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            HRResgierVehicleObject BSM = new HRResgierVehicleObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (HRResgierVehicleObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeaderView = DBV.HR_AssetRegisterVehicle_View.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRResgierVehicleObject BSM = new HRResgierVehicleObject();
            BSM.ListHeaderView = new List<HR_AssetRegisterVehicle_View>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRResgierVehicleObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeaderView);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
			ViewData[SYSConstant.PARAM_ID1] = false;
			UserConfListAndForm(this.KeyName);
            HRResgierVehicleObject BSM = new HRResgierVehicleObject();
            BSM.Header = new HRResgierVehicle();
            BSM.Header.Year = DateTime.Now.Year;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(HRResgierVehicleObject collection)
        {
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ActionName = "Create";
			ViewData[SYSConstant.PARAM_ID1] = false;
			HRResgierVehicleObject BSM = new HRResgierVehicleObject();
            if (Session[PATH_FILE] != null)
            {
                collection.Header.PathFile = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRResgierVehicleObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Header = collection.Header;
            BSM.HeaderStaff = collection.HeaderStaff;
            BSM.ScreenId = SCREEN_ID;
            string msg = BSM.createVehicle();
            if (msg == SYConstant.OK)
            {
                SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                mess_err.DocumentNumber = BSM.Header.ID.ToString();
                mess_err.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess_err.DocumentNumber;
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                return View(BSM);
            }
            else
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);

            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");

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
        #region Edit
        public ActionResult Edit(string ID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
			ViewData[SYSConstant.PARAM_ID1] = false;
			if (ID != null)
            {
                HRResgierVehicleObject BSM = new HRResgierVehicleObject();
                BSM.Header = new HRResgierVehicle();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                BSM.Header = DB.HRResgierVehicles.FirstOrDefault(w => w.ID.ToString() == ID);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);

            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, HRResgierVehicleObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
			ViewData[SYSConstant.PARAM_ID1] = false;
			HRResgierVehicleObject BSM = new HRResgierVehicleObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HRResgierVehicleObject)Session[Index_Sess_Obj + ActionName];
                }
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.PathFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                } 
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditVehicle(id);
                if (msg == SYConstant.OK)
                {
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
            if (id != null)
            {
                HRResgierVehicleObject BSM = new HRResgierVehicleObject();
                string msg = BSM.DeleteVehicle(id);
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
        #region "Detail"
        public ActionResult Details(string id)
        {
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
			ViewData[SYSConstant.PARAM_ID1] = true;
			if (id == "null") id = null;
            if (id != null)
            {
                HRResgierVehicleObject BSM = new HRResgierVehicleObject();
                BSM.Header = new HRResgierVehicle();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                BSM.Header = DB.HRResgierVehicles.FirstOrDefault(w => w.ID.ToString() == id);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
		#endregion
		public string GetAssetClass(string code)
		{

			UserSession();
			UserConfListAndForm();
			if (!string.IsNullOrEmpty(code))
			{
				Session["AssetClass"] = code;
			}
			else
			{
				Session["AssetClass"] = null;
			}
			return SYConstant.OK;
		}
		public async Task<ActionResult> GetAssetCode()
		{
			UserSession();
			UserConfListAndForm();
			if (Session["AssetClass"] != null)
			{
				string assetClass = Session["AssetClass"].ToString();
				var getCate = DB.HRAssetRegisters.Where(s => s.AssetClassCode == assetClass);
				ViewData["GetAssetCode"] = getCate.ToList();
				return PartialView("GetAssetCode");
			}
			return null;
		}
		public ActionResult getCarModel(string id, string Action)
		{
			ActionName = Action;
			HRResgierVehicleObject BSM = new HRResgierVehicleObject();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (HRResgierVehicleObject)Session[Index_Sess_Obj + ActionName];
				var obj = DB.HRMissOilRatings.FirstOrDefault(w => w.ID.ToString() == id);
				if (obj != null)
				{
					var result = new
					{
						MS = SYConstant.OK,
						Model = obj.Model,
						Horsepower = obj.Horsepower,
						Rate = obj.Rate
					};
					return Json(result, JsonRequestBehavior.DenyGet);
				}
			}

			var rs = new { MS = SYConstant.OK };
			return Json(rs, JsonRequestBehavior.DenyGet);
		}
		public ActionResult GetAssetDes(string id, string Action)
		{
			ActionName = Action;
			HRResgierVehicleObject BSM = new HRResgierVehicleObject();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (HRResgierVehicleObject)Session[Index_Sess_Obj + ActionName];
				var obj = DB.HRAssetRegisters.FirstOrDefault(w => w.AssetCode == id);
				if (obj != null)
				{
					var result = new
					{
						MS = SYConstant.OK,
						Model = obj.Description,
						AssetCode = obj.AssetCode
					};
					return Json(result, JsonRequestBehavior.DenyGet);
				}
			}

			var rs = new { MS = SYConstant.OK };
			return Json(rs, JsonRequestBehavior.DenyGet);
		}
		private void DataSelector()
        {
            ViewData["VEHICLE_SELECT"] = DB.HRMissOilRatings.ToList();
			ViewData["AssetClass_SELECT"] = DB.HRAssetClasses.Where(s=>s.IsVehicle == true).ToList();
			ViewData["MissionCode_SELECT"] = DB.HRMissionPlans.ToList();
		}
    }
}
