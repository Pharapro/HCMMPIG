using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Windows.Interop;

namespace Humica.Controllers.Asset
{
    public class HRBookingCarController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RV0000001";
        private const string URL_SCREEN = "/Asset/HRBookingCar/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "BookingNo";
        HumicaDBContext DB = new HumicaDBContext();

        public HRBookingCarController()
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
            DataSelector();

            HRBookingCarObject BSM = new HRBookingCarObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRBookingCarObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = new List<HRBookingCar>();
            var ListHeader = DB.HRBookingCars.ToList();
            ListHeader = ListHeader.OrderByDescending(x => x.BookingNo).ToList();
            BSM.ListHeader = ListHeader.ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(HRBookingCarObject collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            DataSelector();
            HRBookingCarObject BSM = new HRBookingCarObject();
            BSM.ListHeader = new List<HRBookingCar>();
            var ListHeader = DB.HRBookingCars.ToList();
            ListHeader = ListHeader.OrderByDescending(x => x.DocumentDate).ToList();
            BSM.ListHeader = ListHeader.ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRBookingCarObject BSM = new HRBookingCarObject();
            BSM.ListHeader = new List<HRBookingCar>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRBookingCarObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            HRBookingCarObject BSM = new HRBookingCarObject();
            BSM.ListBookingSchedule = new List<HRBookingCarSchedule>();
            BSM.Header = new HRBookingCar();
			var maxBookingNo = DB.HRBookingCars.Select(b => (int?)b.BookingNo) .Max() ?? 0; 
			BSM.Header.BookingNo = maxBookingNo + 1;
			BSM.Header.DocumentDate = DateTime.Now;
			BSM.Header.Status = SYDocumentStatus.BOOKING.ToString();
			Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(string ID, HRBookingCarObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            var BSM = new HRBookingCarObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRBookingCarObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.createBooking();

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.BookingNo.ToString();
                    mess.Description = mess.Description;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM.Header.DocumentDate = DateTime.Now;
                    return View(BSM);
                }
                else
                {
					SYMessages mess = SYMessages.getMessageObject(msg, user.Lang);
					mess.Description = string.Format(mess.Description, BSM.MessageError);
					Session[SYConstant.MESSAGE_SUBMIT] = mess;
					Session[Index_Sess_Obj + ActionName] = BSM;
					return View(BSM);
				}
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
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
            if (!string.IsNullOrEmpty(ID))
            {
                HRBookingCarObject BSM = new HRBookingCarObject();
                BSM.Header = new HRBookingCar();
                BSM.ListBookingSchedule = new List<HRBookingCarSchedule>();
                BSM.Header = DB.HRBookingCars.FirstOrDefault(w => w.BookingNo.ToString() == ID);
                BSM.ListBookingSchedule = DB.HRBookingCarSchedules.Where(w => w.BookingNo.ToString() == ID).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, HRBookingCarObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            HRBookingCarObject BSM = new HRBookingCarObject();
			if (!string.IsNullOrEmpty(id))
			{
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HRBookingCarObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Edit(id);
                if (msg == SYConstant.OK)
                {
                    DB = new HumicaDBContext();
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
                else
                {
					SYMessages mess = SYMessages.getMessageObject(msg, user.Lang);
					mess.Description = string.Format(mess.Description, BSM.MessageError);
					Session[SYConstant.MESSAGE_SUBMIT] = mess;
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
			if (!string.IsNullOrEmpty(id))
			{
                HRBookingCarObject Del = new HRBookingCarObject();
                string msg = Del.Delete(id);
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
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
			if (id == "null") id = null;
			if (!string.IsNullOrEmpty(id))
			{
                HRBookingCarObject BSM = new HRBookingCarObject();
				int bookingId = Convert.ToInt32(id);
				BSM.Header = DB.HRBookingCars.Find(bookingId);
				//BSM.Header = DB.HRBookingCars.Find(id);
                if (BSM.Header != null)
                {
                    BSM.ListBookingSchedule = DB.HRBookingCarSchedules.Where(w => w.BookingNo.ToString() == id).OrderBy(w => w.LineItem).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion

        #region "Ajax select item for time"
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            HRBookingCarObject BSM = new HRBookingCarObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRBookingCarObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            return PartialView("GridItems", BSM.ListBookingSchedule);
        }
        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateItem(HRBookingCarSchedule ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            HRBookingCarObject BSM = new HRBookingCarObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (HRBookingCarObject)Session[Index_Sess_Obj + ActionName];
                    }
                    if(ModelObject.HaveDriver == true)
                    {
                        if (ModelObject.DriverCode == null)
                        {
							ViewData["EditError"] = "Driver is required";
							return PartialView("GridItems", BSM.ListBookingSchedule);
						}
                    }
                    DateTime StartTime = ModelObject.BookingDate + ModelObject.StartTime.TimeOfDay;
                    DateTime EndTime = ModelObject.BookingDate + ModelObject.EndTime.TimeOfDay;
                    ModelObject.StartTime = StartTime;
                    ModelObject.EndTime = EndTime;
					if (StartTime >= EndTime)
					{
						SYMessages mess = SYMessages.getMessageObject("TIME_CAR", user.Lang);
						ViewData["EditError"] = mess.Description;
						Session[Index_Sess_Obj + ActionName] = BSM;
						return PartialView("GridItems", BSM.ListBookingSchedule);
					}
					int line = 0;
                    if (BSM.ListBookingSchedule.Count == 0)
                    {
                       line = 1;
                    }
                    else
                    {
                       line = BSM.ListBookingSchedule.Max(w => w.LineItem);
                       line = line + 1;
                    }
                    ModelObject.LineItem = line;
                    BSM.ListBookingSchedule.Add(ModelObject);
                    Session[Index_Sess_Obj + ActionName] = BSM;
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

            return PartialView("GridItems", BSM.ListBookingSchedule);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditItem(HRBookingCarSchedule ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            HRBookingCarObject BSM = new HRBookingCarObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (HRBookingCarObject)Session[Index_Sess_Obj + ActionName];
                    }
					if (ModelObject.HaveDriver == true)
					{
						if (ModelObject.DriverCode == null)
						{
							ViewData["EditError"] = "Driver is required";
							return PartialView("GridItems", BSM.ListBookingSchedule);
						}
					}
					var error = 0;
                    DateTime StartTime = ModelObject.BookingDate + ModelObject.StartTime.TimeOfDay;
                    DateTime EndTime = ModelObject.BookingDate + ModelObject.EndTime.TimeOfDay;
                    ModelObject.StartTime = StartTime;
                    ModelObject.EndTime = EndTime;
					if (StartTime >= EndTime)
					{
						SYMessages mess = SYMessages.getMessageObject("TIME_CAR", user.Lang);
						ViewData["EditError"] = mess.Description;
						Session[Index_Sess_Obj + ActionName] = BSM;
						return PartialView("GridItems", BSM.ListBookingSchedule);
					}
					//var filteredList = BSM.ListBookingSchedule.Where(w => w.LineItem != ModelObject.LineItem).ToList();
					//string msg = BSM.IsValidBookingTime(ModelObject, filteredList);
					//if (msg != SYConstant.OK)
					//               {
					//                   error = 1;
					//                   ViewData["EditError"] = SYMessages.getMessage(msg);
					//               }
					//var checkList = BSM.ListBookingSchedule.Where(w => w.RoomID == ModelObject.RoomID
					//&& w.BookingDate == ModelObject.BookingDate
					//&& w.EndTime.TimeOfDay >= ModelObject.StartTime.TimeOfDay).ToList();
					//if (checkList.Count > 1)
					//{
					//    ViewData["EditError"] = SYMessages.getMessage("CON_NOT_ALLOW");
					//    error = 1;
					//}
					//var objCheck = DP.HRBookingCarSchedules.Where(w=>w.RoomID== ModelObject.RoomID &&
					//w.BookingDate== ModelObject.BookingDate).ToList();
					//var _objCheck = objCheck.FirstOrDefault(w => w.EndTime.TimeOfDay >= ModelObject.StartTime.TimeOfDay);
					//if (_objCheck != null)
					//{
					//    ViewData["EditError"] = SYMessages.getMessage("DUP_WITH_OTHER");
					//    error = 2;
					//}
					//if (error == 0)
					//{
					var objUpdate = BSM.ListBookingSchedule.Where(w => w.LineItem == ModelObject.LineItem).First();
						objUpdate.HaveDriver = ModelObject.HaveDriver;
						objUpdate.DriverCode = ModelObject.DriverCode;
						objUpdate.CarID = ModelObject.CarID;
                        objUpdate.BookingDate = ModelObject.BookingDate;
                        objUpdate.StartTime = ModelObject.StartTime;
                        objUpdate.EndTime = ModelObject.EndTime;

                        Session[Index_Sess_Obj + ActionName] = BSM;
                    //}
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

            return PartialView("GridItems", BSM.ListBookingSchedule);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteItem(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            HRBookingCarObject BSM = new HRBookingCarObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (HRBookingCarObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var error = 0;

                    var checkList = BSM.ListBookingSchedule.Where(w => w.LineItem == LineItem).ToList();
                    if (checkList.Count == 0)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("NO_ITEM_DELETE");
                        error = 1;
                    }

                    if (error == 0)
                    {
                        BSM.ListBookingSchedule.Remove(checkList.First());
                        Session[Index_Sess_Obj + ActionName] = BSM;
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

            return PartialView("GridItems", BSM.ListBookingSchedule);
        }

		#endregion
		public ActionResult GetFromAssetStaff(string id, string Action)
		{
			ActionName = Action;
			HRBookingCarObject BSM = new HRBookingCarObject();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (HRBookingCarObject)Session[Index_Sess_Obj + ActionName];
				var obj = DB.HRResgierVehicles.FirstOrDefault(w => w.ID.ToString() == id);
				if (obj != null)
				{
                    var getAsset = DB.HRAssetStaffs.FirstOrDefault(w => w.AssetCode == obj.AssetCode);
                    if (getAsset != null)
                    {
                        var getName = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == getAsset.EmpCode);
						if (getName != null)
                        {
							var result = new
							{
								MS = SYConstant.OK,
								EmpCode = $"{getName.EmpCode}:{getName.AllName}"
							};
							return Json(result, JsonRequestBehavior.DenyGet);
						}
					}
				}
			}
			var rs = new { MS = SYConstant.FAIL };
			return Json(rs, JsonRequestBehavior.DenyGet);
		}
		public ActionResult RefreshTotal(string id)
        {
            ActionName = "Create";
            HRBookingCarObject BSM = new HRBookingCarObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRBookingCarObject)Session[Index_Sess_Obj + ActionName];
                decimal TotalHour = 0;
                foreach (var read in BSM.ListBookingSchedule)
                {
                    var interval = read.EndTime.Subtract(read.StartTime);
                    var Hour = interval.TotalHours;
                    TotalHour += Convert.ToDecimal(Math.Round(Hour, 2));
                }
                BSM.Header.TotalHour = TotalHour;
                var result = new
                {
                    MS = SYConstant.OK,
                    Total = BSM.Header.TotalHour
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }

        private void DataSelector()
        {
            SYDataList objList = new SYDataList("LEAVE_TIME");
			ViewData["CARTypes_SELECT"] = DB.HRResgierVehicles.ToList();
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
			ViewData["Driver_SELECT"] = DB.HRDrivers.Where(s=>s.IsActive==true).ToList();
			objList = new SYDataList("STATUS_LEAVE_APPROVAL");
            ViewData["STATUS_APPROVAL"] = objList.ListData;
        }
    }
}
