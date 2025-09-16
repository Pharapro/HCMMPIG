using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.MyTeam
{

    public class ESSRoomBookingController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000014";
        private const string URL_SCREEN = "/SelfService/MyTeam/ESSRoomBooking/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "BookingNo";
        IClsBookingRoom BSM;
        public ESSRoomBookingController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsBookingRoom();
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            DataSelector();
            //if (Session[Index_Sess_Obj + ActionName] != null)
            //{
            //    BSM = (ClsBookingRoom)Session[Index_Sess_Obj + ActionName];
            //}
            BSM.OnLoadingIndex();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        //[HttpPost]
        //public ActionResult Index(BookingRoomObject collection)
        //{
        //    ActionName = "Index";
        //    UserSession();
        //    UserConfList(this.KeyName);
        //    DataSelector();
        //    BookingRoomObject BSM = new BookingRoomObject();
        //    BSM.ListHeader = new List<HRBookingRoom>();
        //    var ListHeader = DB.HRBookingRooms.ToList();
        //    ListHeader = ListHeader.OrderByDescending(x => x.DocumentDate).ToList();
        //    BSM.ListHeader = ListHeader.ToList();

        //    Session[Index_Sess_Obj + ActionName] = BSM;

        //    return View(BSM);
        //}
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsBookingRoom)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridListItems", BSM.ListHeaderItem);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnCreated();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(string ID, ClsBookingRoom collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsBookingRoom)Session[Index_Sess_Obj + ActionName];
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
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion

        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            Session[Index_Sess_Obj + ActionName] = BSM;
            if (!string.IsNullOrEmpty(id))
            {
                BSM.OnDetail(id);
                if (BSM.Header != null)
                {
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, ClsBookingRoom collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (!string.IsNullOrEmpty(id))
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var BSD = (IClsBookingRoom)Session[Index_Sess_Obj + ActionName];
                    collection.ListBookingSchedule = BSD.ListBookingSchedule;
                }
                collection.ScreenId = SCREEN_ID;
                string msg = collection.EditBooking(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = collection.Header.BookingNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=";
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    return View(collection);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(collection);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(collection);
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

            if (!string.IsNullOrEmpty(id))
            {
                BSM.OnDetail(id);
                if (BSM.Header != null)
                {
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Reject
        public ActionResult Reject(string id, string Comment)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.RejctedBooking(id, Comment);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        #region "Ajax select item for time"
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsBookingRoom)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            return PartialView("GridItems", BSM.ListBookingSchedule);
        }
        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateItem(HRBookingSchedule ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (IClsBookingRoom)Session[Index_Sess_Obj + ActionName];
                    }
                    DateTime StartTime = ModelObject.BookingDate + ModelObject.StartTime.TimeOfDay;
                    DateTime EndTime = ModelObject.BookingDate + ModelObject.EndTime.TimeOfDay;
                    ModelObject.StartTime = StartTime;
                    ModelObject.EndTime = EndTime;

                    string msg = BSM.IsValidBookingTime(ModelObject, BSM.ListBookingSchedule);
                    if (msg == SYConstant.OK)
                    {
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
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg);
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
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditItem(HRBookingSchedule ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (IClsBookingRoom)Session[Index_Sess_Obj + ActionName];
                    }
                    var error = 0;
                    DateTime StartTime = ModelObject.BookingDate + ModelObject.StartTime.TimeOfDay;
                    DateTime EndTime = ModelObject.BookingDate + ModelObject.EndTime.TimeOfDay;
                    ModelObject.StartTime = StartTime;
                    ModelObject.EndTime = EndTime;
                    string msg = BSM.IsValidBookingTimes(ModelObject, BSM.ListBookingSchedule);
                    if (msg != SYConstant.OK)
                    {
                        error = 1;
                        ViewData["EditError"] = SYMessages.getMessage(msg);
                    }
                    if (error == 0)
                    {
                        var objUpdate = BSM.ListBookingSchedule.Where(w => w.LineItem == ModelObject.LineItem).First();

                        objUpdate.RoomID = ModelObject.RoomID;
                        objUpdate.BookingDate = ModelObject.BookingDate;
                        objUpdate.StartTime = ModelObject.StartTime;
                        objUpdate.EndTime = ModelObject.EndTime;

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
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteItem(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (IClsBookingRoom)Session[Index_Sess_Obj + ActionName];
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

        public ActionResult RefreshTotal(string id)
        {
            ActionName = "Create";

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsBookingRoom)Session[Index_Sess_Obj + ActionName];
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
            foreach (var data in BSM.OnDataSelector())
            {
                ViewData[data.Key] = data.Value;
            }

        }
    }
}
