using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Performance;
using System;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.Performance
{
    public class ESSKPIConfigurationController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESSS000001";
        private const string URL_SCREEN = "/SelfService/Performance/ESSKPIConfiguration/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        protected IClsKPIConfig BSM;
        public ESSKPIConfigurationController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsKPIConfig();
            BSM.OnLoad();
        }
        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexIndicatorBU(true);
            BSM.OnIndexLoadingIndicator(true);
            BSM.OnIndexLoadingBU(true);
            BSM.OnIndexLoadingDept(true);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        #region KPI List By Indicator
        public ActionResult GridIndicatorByDept()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexLoadingIndicator(true);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridIndicatorByDept", BSM.ListIndicatorByDept);
        }
        public ActionResult CreateIndiByDept(HRKPIIndicator ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (ModelState.IsValid)
            {
                try
                {
                    string Department = BSM.OnLoadingDepartment(ModelObject.Department);
                    ModelObject.Department = Department;
                    var msg = BSM.OnGridModifyIndicator(ModelObject, SYActionBehavior.ADD.ToString(), true);
                    if (msg == SYConstant.OK)
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
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
            BSM.OnIndexLoadingIndicator(true);
            return PartialView("GridIndicatorByDept", BSM.ListIndicatorByDept);

        }
        public ActionResult EditIndiByDept(HRKPIIndicator ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            try
            {
                var msg = BSM.OnGridModifyIndicator(ModelObject, SYActionBehavior.EDIT.ToString(), true);
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.OnIndexLoadingIndicator(true);
            return PartialView("GridIndicatorByDept", BSM.ListIndicatorByDept);
        }
        public ActionResult DeleteIndiByDept(HRKPIIndicator ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM = (IClsKPIConfig)Session[Index_Sess_Obj + ActionName];
            var msg = BSM.OnGridModifyIndicator(ModelObject, SYActionBehavior.DELETE.ToString(), true);
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexLoadingIndicator(true);
            return PartialView("GridIndicatorByDept", BSM.ListIndicatorByDept);
        }
        #endregion

        #region KPI List By Dept
        public ActionResult GridKPIListByDept()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexLoadingDept(true);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridKPIListByDept", BSM.ListKPIListByDept);
        }
        public ActionResult CreateItemByDept(HRKPIList ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (ModelState.IsValid)
            {
                try
                {
                    string Department = BSM.OnLoadingDepartment(ModelObject.Department);
                    ModelObject.Department = Department;
                    var msg = BSM.OnGridModifyKPI(ModelObject, SYActionBehavior.ADD.ToString(), true);
                    if (msg == SYConstant.OK)
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
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
            BSM.OnIndexLoadingDept(true);
            return PartialView("GridKPIListByDept", BSM.ListKPIListByDept);

        }
        public ActionResult EditItemByDept(HRKPIList ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            try
            {
                var msg = BSM.OnGridModifyKPI(ModelObject, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.OnIndexLoadingDept(true);
            return PartialView("GridKPIListByDept", BSM.ListKPIListByDept);
        }
        public ActionResult DeleteItemByDept(HRKPIList ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM = (IClsKPIConfig)Session[Index_Sess_Obj + ActionName];
            var msg = BSM.OnGridModifyKPI(ModelObject, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexLoadingDept(true);
            return PartialView("GridKPIListByDept", BSM.ListKPIListByDept);
        }
        #endregion

        #region KPI By Indicator BY BU
        public ActionResult GridIndicatorByBU()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexIndicatorBU(true);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridIndicatorByBU", BSM.ListIndicatorByBU);
        }
        public ActionResult CreateIndiByBU(HRKPIIndicator ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (ModelState.IsValid)
            {
                try
                {
                    string Department = BSM.OnLoadingBU(ModelObject.Department);
                    ModelObject.Department = Department;
                    var msg = BSM.OnGridModifyIndicatorBU(ModelObject, SYActionBehavior.ADD.ToString(), "BU", true);
                    if (msg == SYConstant.OK)
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
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
            BSM.OnIndexIndicatorBU(true);
            return PartialView("GridIndicatorByBU", BSM.ListIndicatorByBU);

        }
        public ActionResult EditIndiByBU(HRKPIIndicator ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            try
            {
                var msg = BSM.OnGridModifyIndicatorBU(ModelObject, SYActionBehavior.EDIT.ToString(),"BU", true);
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.OnIndexIndicatorBU(true);
            return PartialView("GridIndicatorByBU", BSM.ListIndicatorByBU);
        }
        public ActionResult DeleteIndiByBU(HRKPIIndicator ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM = (IClsKPIConfig)Session[Index_Sess_Obj + ActionName];
            var msg = BSM.OnGridModifyIndicatorBU(ModelObject, SYActionBehavior.DELETE.ToString(),"BU", true);
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexIndicatorBU(true);
            return PartialView("GridIndicatorByBU", BSM.ListIndicatorByBU);
        }
        #endregion

        #region KPI List By BU
        public ActionResult GridKPIListByBU()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexLoadingBU(true);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridKPIListByBU", BSM.ListKPIListByBU);
        }
        public ActionResult CreateItemByBU(HRKPIList ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (ModelState.IsValid)
            {
                try
                {
                    string Department = BSM.OnLoadingBU(ModelObject.Department);
                    ModelObject.Department = Department;
                    var msg = BSM.OnGridModifyKPIBU(ModelObject, SYActionBehavior.ADD.ToString(),"BU", true);
                    if (msg == SYConstant.OK)
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
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
            BSM.OnIndexLoadingBU(true);
            return PartialView("GridKPIListByBU", BSM.ListKPIListByBU);

        }
        public ActionResult EditItemByBU(HRKPIList ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            try
            {
                var msg = BSM.OnGridModifyKPIBU(ModelObject, SYActionBehavior.EDIT.ToString(), "BU");
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.OnIndexLoadingBU(true);
            return PartialView("GridKPIListByBU", BSM.ListKPIListByBU);
        }
        public ActionResult DeleteItemByBU(HRKPIList ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM = (IClsKPIConfig)Session[Index_Sess_Obj + ActionName];
            var msg = BSM.OnGridModifyKPIBU(ModelObject, SYActionBehavior.DELETE.ToString(), "BU");
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexLoadingBU(true);
            return PartialView("GridKPIListByBU", BSM.ListKPIListByBU);
        }
        #endregion

        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelectorLoading(true))
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}