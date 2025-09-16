using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.LMS
{

    public class SetLeaveEntitleController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRS0000011";
        private const string URL_SCREEN = "/HRM/LMS/SetLeaveEntitle/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "Code";
        HumicaDBContext DB = new HumicaDBContext();

        public SetLeaveEntitleController()
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

            SetLeaveEntitleHeader BSM = new SetLeaveEntitleHeader();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (SetLeaveEntitleHeader)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = DB.HRSetEntitleHs.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
            SetLeaveEntitleHeader BSM = new SetLeaveEntitleHeader();
            BSM.ListHeader = new List<HRSetEntitleH>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SetLeaveEntitleHeader)Session[Index_Sess_Obj + ActionName];
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
            SetLeaveEntitleHeader BSM = new SetLeaveEntitleHeader();
            BSM.Header = new HRSetEntitleH();
            BSM.ListSetEntitleDetail = new List<HRSetEntitleD>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(SetLeaveEntitleHeader collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
            var BSM = new SetLeaveEntitleHeader();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SetLeaveEntitleHeader)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateSetEntitle();

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
                SetLeaveEntitleHeader BSM = new SetLeaveEntitleHeader();
                BSM.Header = DB.HRSetEntitleHs.Find(id);
                if (BSM.Header != null)
                {
                    BSM.ListSetEntitleDetail = DB.HRSetEntitleDs.Where(w => w.CodeH == id).ToList();
                    BSM.ListSetEntitleDetail = BSM.ListSetEntitleDetail.OrderBy(w => w.LineItem).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

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
                SetLeaveEntitleHeader BSM = new SetLeaveEntitleHeader();
                BSM.Header = DB.HRSetEntitleHs.Find(id);
                BSM.ListSetEntitleDetail = DB.HRSetEntitleDs.Where(x => x.CodeH == id).ToList();
                if (BSM.Header != null)
                {
                    BSM.ListSetEntitleDetail = BSM.ListSetEntitleDetail.OrderBy(w => w.LineItem).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(string id, SetLeaveEntitleHeader BSM)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            UserConfListAndForm();
            DataList();
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    string descripiton = BSM.Header.Description;
                    string OthDes = BSM.Header.OthDesc;
                    string Remark = BSM.Header.Remark;
                    BSM = (SetLeaveEntitleHeader)Session[Index_Sess_Obj + ActionName];
                    BSM.Header.Description = descripiton;
                    BSM.Header.OthDesc = OthDes;
                    BSM.Header.Remark = Remark;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditSetEntitle(id);
                if (msg == SYConstant.OK)
                {

                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.Code.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
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
                SetLeaveEntitleHeader Del = new SetLeaveEntitleHeader();
                string msg = Del.deleteHRSetEntitleH(id);
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
            SetLeaveEntitleHeader BSM = new SetLeaveEntitleHeader();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SetLeaveEntitleHeader)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItems", BSM.ListSetEntitleDetail);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateD(HRSetEntitleD ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            SetLeaveEntitleHeader BSM = new SetLeaveEntitleHeader();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (SetLeaveEntitleHeader)Session[Index_Sess_Obj + ActionName];
                    }
                    int line = 0;
                    if (BSM.ListSetEntitleDetail.Count == 0)
                    {
                        line = 1;
                    }
                    else
                    {
                        line = BSM.ListSetEntitleDetail.Max(w => w.LineItem);
                        line = line + 1;
                    }
                    ModelObject.LineItem = line;
                    BSM.ListSetEntitleDetail.Add(ModelObject);
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

            return PartialView("GridItems", BSM.ListSetEntitleDetail);
        }

        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditD(HRSetEntitleD ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            SetLeaveEntitleHeader BSM = new SetLeaveEntitleHeader();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (SetLeaveEntitleHeader)Session[Index_Sess_Obj + ActionName];
                    }
                    var objUpdate = BSM.ListSetEntitleDetail.Where(w => w.LineItem == ModelObject.LineItem).First();

                    objUpdate.LeaveCode = ModelObject.LeaveCode;
                    objUpdate.FromMonth = ModelObject.FromMonth;
                    objUpdate.ToMonth = ModelObject.ToMonth;
                    objUpdate.Entitle = ModelObject.Entitle;
                    objUpdate.SeniorityBalance = ModelObject.SeniorityBalance;
                    objUpdate.Remark = ModelObject.Remark;
                    objUpdate.IsProRate = ModelObject.IsProRate;
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

            return PartialView("GridItems", BSM.ListSetEntitleDetail);
        }

        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteD(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            SetLeaveEntitleHeader BSM = new SetLeaveEntitleHeader();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (SetLeaveEntitleHeader)Session[Index_Sess_Obj + ActionName];
                    }
                    var error = 0;

                    var checkList = BSM.ListSetEntitleDetail.Where(w => w.LineItem == LineItem).ToList();
                    if (checkList.Count == 0)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("NO_ITEM_DELETE");
                        error = 1;
                    }

                    if (error == 0)
                    {
                        BSM.ListSetEntitleDetail.Remove(checkList.First());
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

            return PartialView("GridItems", BSM.ListSetEntitleDetail);
        }


        #endregion

        #region "Ajax select edit"
        public ActionResult GridItemsEdit()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            SetLeaveEntitleHeader BSM = new SetLeaveEntitleHeader();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SetLeaveEntitleHeader)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            return PartialView("GridItemsEdit", BSM.ListSetEntitleDetail);
        }
        #endregion

        private void DataList()
        {
            ViewData["LEAVETYPE_LIST"] = DB.HRLeaveTypes.Where(w => w.IsParent != true).ToList();
        }
    }
}
