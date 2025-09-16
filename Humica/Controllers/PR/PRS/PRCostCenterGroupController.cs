using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.LMS
{

    public class PRCostCenterGroupController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "PRS0000011";
        private const string URL_SCREEN = "/PR/PRS/PRCostCenterGroup/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "Code";
        HumicaDBContext DB = new HumicaDBContext();

        public PRCostCenterGroupController()
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
            PRCostCenterObject BSM = new PRCostCenterObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRCostCenterObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeaderCCGroup = DB.PRCostCenterGroups.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
            PRCostCenterObject BSM = new PRCostCenterObject();
            BSM.ListHeaderCCGroup = new List<PRCostCenterGroup>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRCostCenterObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeaderCCGroup);
        }

        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
            PRCostCenterObject BSM = new PRCostCenterObject();
            BSM.HeaderCCGroup = new PRCostCenterGroup();
            BSM.ListCCGroupItem = new List<PRCostCenterGroupItem>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(PRCostCenterObject collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
            var BSM = new PRCostCenterObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRCostCenterObject)Session[Index_Sess_Obj + ActionName];
                BSM.HeaderCCGroup = collection.HeaderCCGroup;
            }
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateCCGroup();

                if (msg == SYConstant.OK)
                {

                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.HeaderCCGroup.Code;
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
                PRCostCenterObject BSM = new PRCostCenterObject();
                BSM.HeaderCCGroup = DB.PRCostCenterGroups.Find(id);
                if (BSM.HeaderCCGroup != null)
                {
                    BSM.ListCCGroupItem = DB.PRCostCenterGroupItems.Where(w => w.CodeCCGoup == id).ToList();
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
                PRCostCenterObject BSM = new PRCostCenterObject();
                BSM.HeaderCCGroup = DB.PRCostCenterGroups.Find(id);
                BSM.ListCCGroupItem = DB.PRCostCenterGroupItems.Where(x => x.CodeCCGoup == id).ToList();
                if (BSM.HeaderCCGroup != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(string id, PRCostCenterObject BSM)
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
                    string descripiton = BSM.HeaderCCGroup.Description;
                    string Remark = BSM.HeaderCCGroup.Remark;
                    BSM = (PRCostCenterObject)Session[Index_Sess_Obj + ActionName];
                    BSM.HeaderCCGroup.Description = descripiton;
                    BSM.HeaderCCGroup.Remark = Remark;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditCCGroup(id);
                if (msg == SYConstant.OK)
                {

                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.HeaderCCGroup.Code.ToString();
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
                PRCostCenterObject Del = new PRCostCenterObject();
                string msg = Del.DeleteCCGroup(id);
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
            PRCostCenterObject BSM = new PRCostCenterObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRCostCenterObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItems", BSM.ListCCGroupItem);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateD(PRCostCenterGroupItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            PRCostCenterObject BSM = new PRCostCenterObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRCostCenterObject)Session[Index_Sess_Obj + ActionName];
                    }
                    BSM.ListCCGroupItem.Add(ModelObject);
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

            return PartialView("GridItems", BSM.ListCCGroupItem);
        }

        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditD(PRCostCenterGroupItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            PRCostCenterObject BSM = new PRCostCenterObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRCostCenterObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var objUpdate = BSM.ListCCGroupItem.Where(w => w.CostCenterType == ModelObject.CostCenterType).First();

                    objUpdate.Charge = ModelObject.Charge;
                    objUpdate.Description = ModelObject.Description;
                    objUpdate.Remark = ModelObject.Remark;
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

            return PartialView("GridItems", BSM.ListCCGroupItem);
        }

        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteD(string CostCenterType)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            PRCostCenterObject BSM = new PRCostCenterObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRCostCenterObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var error = 0;

                    var checkList = BSM.ListCCGroupItem.Where(w => w.CostCenterType == CostCenterType).ToList();
                    if (checkList.Count == 0)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("NO_ITEM_DELETE");
                        error = 1;
                    }

                    if (error == 0)
                    {
                        BSM.ListCCGroupItem.Remove(checkList.First());
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

            return PartialView("GridItems", BSM.ListCCGroupItem);
        }


        #endregion

        #region "Ajax select edit"
        public ActionResult GridItemsEdit()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            PRCostCenterObject BSM = new PRCostCenterObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRCostCenterObject)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            return PartialView("GridItemsEdit", BSM.ListCCGroupItem);
        }
        #endregion
        private void DataList()
        {
            ViewData["CostCenter_LIST"] = DB.PRCostCenters.ToList();
        }
    }
}
