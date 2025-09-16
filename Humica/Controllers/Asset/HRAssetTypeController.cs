using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.Asset;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Asset
{
    public class HRAssetTypeController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "AM00000003";
        private const string URL_SCREEN = "/Asset/HRAssetType";
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBContext DB = new HumicaDBContext();
        public HRAssetTypeController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region 'List'
        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            HRAssetObject BSM = new HRAssetObject();
            BSM.ListAssetClass = DB.HRAssetClasses.ToList();
            BSM.ListAssetType = DB.HRAssetTypes.ToList();
			BSM.ListAssetLocation = DB.HRAssetLocations.ToList();
			BSM.ListDriver = DB.HRDrivers.ToList();
			return View(BSM);
        }
        #endregion
        #region 'GridItemAssetType'
        public ActionResult GridItemAssetType()
        {
            UserConf(ActionBehavior.EDIT);

            HRAssetObject BSM = new HRAssetObject();
            BSM.ListAssetType = DB.HRAssetTypes.ToList();
            return PartialView("GridItemAssetType", BSM.ListAssetType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateAssType(HRAssetType MModel)
        {
            UserSession();
            HRAssetObject BSM = new HRAssetObject();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.AssetTypeCode = MModel.AssetTypeCode.ToUpper();
                    DB.HRAssetTypes.Add(MModel);
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
            BSM.ListAssetType = DB.HRAssetTypes.ToList();
            return PartialView("GridItemAssetType", BSM.ListAssetType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditAssType(HRAssetType MModel)
        {
            UserSession();
            HRAssetObject BSM = new HRAssetObject();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.HRAssetTypes.FirstOrDefault(w => w.AssetTypeCode == MModel.AssetTypeCode);
                    if (ObjMatch != null)
                    {
                        ObjMatch.Description = MModel.Description;

                        DB.HRAssetTypes.Attach(ObjMatch);
                        DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
                        DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
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
            BSM.ListAssetType = DB.HRAssetTypes.ToList();
            return PartialView("GridItemAssetType", BSM.ListAssetType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteAssType(string AssetTypeCode)
        {
            UserSession();
            HRAssetObject BSM = new HRAssetObject();
            if (AssetTypeCode != null)
            {
                try
                {
                    var obj = DB.HRAssetTypes.Find(AssetTypeCode);
                    if (obj != null)
                    {
                        DB.HRAssetTypes.Remove(obj);
                        DB.SaveChanges();
                    }
                    BSM.ListAssetType = DB.HRAssetTypes.ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemAssetType", BSM.ListAssetType);
        }
        #endregion

        #region 'GridItemAssetClass'
        public ActionResult GridItemAssetClass()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            HRAssetObject BSM = new HRAssetObject();
            BSM.ListAssetClass = DB.HRAssetClasses.ToList();
            return PartialView("GridItemAssetClass", BSM.ListAssetClass);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateAssClass(HRAssetClass MModel)
        {
            UserSession();
            DataSelector();
            HRAssetObject BSM = new HRAssetObject();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.AssetClassCode = MModel.AssetClassCode.ToUpper();
                    DB.HRAssetClasses.Add(MModel);
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
            BSM.ListAssetClass = DB.HRAssetClasses.ToList();
            return PartialView("GridItemAssetClass", BSM.ListAssetClass);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditAssClass(HRAssetClass MModel)
        {
            UserSession();
            DataSelector();
            HRAssetObject BSM = new HRAssetObject();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.HRAssetClasses.FirstOrDefault(w => w.AssetClassCode == MModel.AssetClassCode);
                    if (ObjMatch != null)
                    {
                        ObjMatch.NumberRank = MModel.NumberRank;
                        ObjMatch.Description = MModel.Description;
                        ObjMatch.AssetTypeCode = MModel.AssetTypeCode;
                        ObjMatch.Remark = MModel.Remark;
						ObjMatch.IsVehicle = MModel.IsVehicle;
						DB.HRAssetClasses.Attach(ObjMatch);
                        DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
                        DB.Entry(ObjMatch).Property(x => x.AssetTypeCode).IsModified = true;
                        DB.Entry(ObjMatch).Property(x => x.NumberRank).IsModified = true;
                        DB.Entry(ObjMatch).Property(x => x.Remark).IsModified = true;
						DB.Entry(ObjMatch).Property(x => x.IsVehicle).IsModified = true;
						DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
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
            BSM.ListAssetClass = DB.HRAssetClasses.ToList();
            return PartialView("GridItemAssetClass", BSM.ListAssetClass);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteAssClass(string AssetClassCode)
        {
            UserSession();
            HRAssetObject BSM = new HRAssetObject();
            DataSelector();
            if (AssetClassCode != null)
            {
                try
                {
                    var obj = DB.HRAssetClasses.Find(AssetClassCode);
                    if (obj != null)
                    {
                        DB.HRAssetClasses.Remove(obj);
                        DB.SaveChanges();
                    }
                    BSM.ListAssetClass = DB.HRAssetClasses.ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemAssetClass", BSM.ListAssetClass);
        }
		#endregion
		#region 'GridItemAssetLocation'
		public ActionResult GridItemAssetLocation()
		{
			UserConf(ActionBehavior.EDIT);

			HRAssetObject BSM = new HRAssetObject();
			BSM.ListAssetLocation = DB.HRAssetLocations.ToList();
			return PartialView("GridItemAssetLocation", BSM.ListAssetLocation);
		}

		[HttpPost, ValidateInput(false)]
		public ActionResult CreateAssLocation(HRAssetLocation MModel)
		{
			UserSession();
			HRAssetObject BSM = new HRAssetObject();
			if (ModelState.IsValid)
			{
				try
				{
					MModel.Code = MModel.Code.ToUpper();
					DB.HRAssetLocations.Add(MModel);
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
			BSM.ListAssetLocation = DB.HRAssetLocations.ToList();
			return PartialView("GridItemAssetLocation", BSM.ListAssetLocation);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult EditAssLocation(HRAssetLocation MModel)
		{
			UserSession();
			HRAssetObject BSM = new HRAssetObject();
			if (ModelState.IsValid)
			{
				try
				{
					DB = new HumicaDBContext();
					var ObjMatch = DB.HRAssetLocations.FirstOrDefault(w => w.Code == MModel.Code);
					if (ObjMatch != null)
					{
						ObjMatch.Description = MModel.Description;
						ObjMatch.Remark = MModel.Remark;

						DB.HRAssetLocations.Attach(ObjMatch);
						DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
						DB.Entry(ObjMatch).Property(x => x.Remark).IsModified = true;
						DB.SaveChanges();
					}
					else
					{
						ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
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
			BSM.ListAssetLocation = DB.HRAssetLocations.ToList();
			return PartialView("GridItemAssetLocation", BSM.ListAssetLocation);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult DeleteAssLocation(string Code)
		{
			UserSession();
			HRAssetObject BSM = new HRAssetObject();
			if (Code != null)
			{
				try
				{
					var obj = DB.HRAssetLocations.Find(Code);
					if (obj != null)
					{
						DB.HRAssetLocations.Remove(obj);
						DB.SaveChanges();
					}
					BSM.ListAssetLocation = DB.HRAssetLocations.ToList();
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			return PartialView("GridItemAssetLocation", BSM.ListAssetLocation);
		}
		#endregion
		#region 'GridItemDriver'
		public ActionResult GridItemDriver()
		{
            DataSelector();
			UserConf(ActionBehavior.EDIT);
			HRAssetObject BSM = new HRAssetObject();
			BSM.ListDriver = DB.HRDrivers.ToList();
			return PartialView("GridItemDriver", BSM.ListDriver);
		}

		[HttpPost, ValidateInput(false)]
		public ActionResult CreateDriver(HRDriver MModel)
		{
			UserSession();
			DataSelector();
			HRAssetObject BSM = new HRAssetObject();
			if (ModelState.IsValid)
			{
				try
				{
					//MModel.Code = MModel.Code.ToUpper();
					DB.HRDrivers.Add(MModel);
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
			BSM.ListDriver = DB.HRDrivers.ToList();
			return PartialView("GridItemDriver", BSM.ListDriver);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult EditDriver(HRDriver MModel)
		{
			UserSession();
			DataSelector();
			HRAssetObject BSM = new HRAssetObject();
			if (ModelState.IsValid)
			{
				try
				{
					DB = new HumicaDBContext();
					var ObjMatch = DB.HRDrivers.FirstOrDefault(w=>w.DriverCode==MModel.DriverCode);
					if (ObjMatch != null)
					{
						ObjMatch.IsActive = MModel.IsActive;
						DB.HRDrivers.Attach(ObjMatch);
						DB.Entry(ObjMatch).Property(x => x.IsActive).IsModified = true;
						DB.SaveChanges();
					}
					else
					{
						ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
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
			BSM.ListDriver = DB.HRDrivers.ToList();
			return PartialView("GridItemDriver", BSM.ListDriver);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult DeleteDriver(string DriverCode)
		{
			UserSession();
			HRAssetObject BSM = new HRAssetObject();
			if (DriverCode != null)
			{
				try
				{
					var obj = DB.HRDrivers.FirstOrDefault(w => w.DriverCode == DriverCode);
					if (obj != null)
					{
						DB.HRDrivers.Remove(obj);
						DB.SaveChanges();
					}
					BSM.ListDriver = DB.HRDrivers.ToList();
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			return PartialView("GridItemDriver", BSM.ListDriver);
		}
		#endregion
		[HttpPost]
		public ActionResult getDriver(string id, string Action)
		{
			if (string.IsNullOrEmpty(id))
			{
				return Json(new { MS = "Error", Message = "DriverCode is required." }, JsonRequestBehavior.DenyGet);
			}
			var driver = DB.HRStaffProfiles
				.Where(w => w.EmpCode == id)
				.Select(d => new { AllName = d.AllName })
				.FirstOrDefault();

			if (driver != null)
			{
				return Json(new { MS = "OK", Model = driver.AllName }, JsonRequestBehavior.DenyGet);
			}
			return Json(new { MS = "Error", Message = "Driver not found." }, JsonRequestBehavior.DenyGet);
	    }
		#region 'Private Code'
		private void DataSelector()
        {
            ViewData["ASSETTYPE_SELECT"] = DB.HRAssetTypes.ToList();
            ViewData["NUMBERING_SELECT"] = SMS.BSNumberRanks.ToList();
			ViewData["Driver_SELECT"] = DB.HRStaffProfiles.Where(s=>s.Status=="A").ToList();
		}
        #endregion
    }
}
