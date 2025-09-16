using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.Atts;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.AttSetup
{
    public class ATBatchController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ATS0000005";
        private const string URL_SCREEN = "/Attendance/AttSetup/ATBatch/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "Code";
        HumicaDBContext DB = new HumicaDBContext();

        public ATBatchController()
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
            DataList();

            ATBatchObject BSM = new ATBatchObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ATBatchObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = DB.ATBatches.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
            ATBatchObject BSM = new ATBatchObject();
            BSM.ListHeader = new List<ATBatch>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATBatchObject)Session[Index_Sess_Obj + ActionName];
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
            DataList();
            ATBatchObject BSM = new ATBatchObject();
            BSM.Header = new ATBatch();
            BSM.ListItem = new List<ATBatchItem>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ATBatchObject collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
            var BSM = new ATBatchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATBatchObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateBatch();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.Code;
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
        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();

            UserConfForm(SYActionBehavior.EDIT);
            DataList();
            ViewData[SYSConstant.PARAM_ID] = id;

            if (id != null)
            {
                ATBatchObject BSM = new ATBatchObject();
                BSM.Header = DB.ATBatches.Find(id);
                if (BSM.Header != null)
                {
                    BSM.ListItem = DB.ATBatchItems.Where(w => w.BatchNo == id).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        public ActionResult GridItemsDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            ATBatchObject BSM = new ATBatchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATBatchObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemsDetails", BSM.ListItem);
        }
        #endregion
        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID] = id;
            DataList();
            if (id != null)
            {
                ATBatchObject BSM = new ATBatchObject();
                BSM.Header = DB.ATBatches.Find(id);
                if (BSM.Header != null)
                {
                    BSM.ListItem = DB.ATBatchItems.Where(x => x.BatchNo == id).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(string id, ATBatchObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            UserConfListAndForm();
            DataList();
            ViewData[SYSConstant.PARAM_ID] = id;
            ATBatchObject BSM = new ATBatchObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ATBatchObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditBatch(id);
                if (msg == SYConstant.OK)
                {

                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.Code.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    return View(BSM);
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
        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataList();
            if (id != null)
            {
                ATBatchObject Del = new ATBatchObject();
                string msg = Del.DeleteBatch(id);
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
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("LEAVE_NE", user.Lang);
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
            DataList();
            ATBatchObject BSM = new ATBatchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATBatchObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItems", BSM.ListItem);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateD(ATBatchItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ATBatchObject BSM = new ATBatchObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ATBatchObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var check = BSM.ListItem.Where(w => w.ShiftCode == ModelObject.ShiftCode).ToList();
                    if (check.Count() == 0)
                    {
                        BSM.ListItem.Add(ModelObject);
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED", user.Lang);
                    }
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
            DataList();

            return PartialView("GridItems", BSM.ListItem);
        }

        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditD(ATBatchItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ATBatchObject BSM = new ATBatchObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ATBatchObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var objCheck = BSM.ListItem.Where(w => w.ShiftCode == ModelObject.ShiftCode).ToList();
                    if (objCheck.Count > 0)
                    {
                        objCheck.First().Mon = ModelObject.Mon;
                        objCheck.First().Tue = ModelObject.Tue;
                        objCheck.First().Wed = ModelObject.Wed;
                        objCheck.First().Thu = ModelObject.Thu;
                        objCheck.First().Fri = ModelObject.Fri;
                        objCheck.First().Sat = ModelObject.Sat;
                        objCheck.First().Sun = ModelObject.Sun;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                    }
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
            DataList();

            return PartialView("GridItems", BSM.ListItem);
        }

        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteD(string ShiftCode)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ATBatchObject BSM = new ATBatchObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ATBatchObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var error = 0;

                    var checkList = BSM.ListItem.Where(w => w.ShiftCode == ShiftCode).ToList();
                    if (checkList.Count() == 0)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("NO_ITEM_DELETE");
                        error = 1;
                    }
                    if (error == 0)
                    {
                        BSM.ListItem.Remove(checkList.First());
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
            DataList();

            return PartialView("GridItems", BSM.ListItem);
        }


        #endregion
        private void DataList()
        {
            ViewData["SHIFT_LIST"] = DB.ATShifts.ToList();
        }
    }
}
