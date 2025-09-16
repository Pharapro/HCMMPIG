using DevExpress.XtraCharts;
using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Performance;
using System;
using System.Data.Entity.Validation;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HRKPIConfigurationController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRAS000005";
        private const string URL_SCREEN = "/HRM/Appraisal/HRKPIConfiguration/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        protected IClsKPIConfig BSM;
        public HRKPIConfigurationController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsKPIConfig();
            BSM.OnLoad();
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            BSM.OnIndexLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        #region "Setup KPI"
        public ActionResult GridItemsKPI()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            UserConf(ActionBehavior.EDIT);
            BSM.OnIndexLoadingKPIType();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsKPI", BSM.ListKPIGroupHeader);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateKPI(HRKPIType MModel)
        {
            UserSession();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsKPIConfig)Session[Index_Sess_Obj + ActionName];
                }
                var msg = BSM.OnGridModifyKPIType(MModel, SYActionBehavior.ADD.ToString());
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexLoadingKPIType();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsKPI", BSM.ListKPIGroupHeader);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditKPI(HRKPIType MModel)
        {
            UserSession();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
                try
                {
                    var msg = BSM.OnGridModifyKPIType(MModel, SYActionBehavior.EDIT.ToString());
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

            BSM.OnIndexLoadingKPIType();
            return PartialView("GridItemsKPI", BSM.ListKPIGroupHeader);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteKPI(HRKPIType MModel)
        {
            UserSession();
            UserConfListAndForm();
            try
            {
                var msg = BSM.OnGridModifyKPIType(MModel, SYActionBehavior.DELETE.ToString()); 
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.OnIndexLoadingKPIType();
            return PartialView("GridItemsKPI", BSM.ListKPIGroupHeader);
        }
        #endregion

        #region "GridItemsCategory"
        public ActionResult GridItemsCategory()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM.OnIndexLoadingCategory();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsCategory", BSM.ListCategory);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CategoryCreate(HRKPICategory MModel)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyCategory(MModel, SYActionBehavior.ADD.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexLoadingCategory();
            return PartialView("GridItemsCategory", BSM.ListCategory);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult CategoryEdit(HRKPICategory MModel)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (ModelState.IsValid)
            {
                var msg = BSM.OnGridModifyCategory(MModel, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexLoadingCategory();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsCategory", BSM.ListCategory);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult CategoryDelete(HRKPICategory MModel)
        {
            UserSession();
            DataSelector();
            UserConfListAndForm();
            var msg = BSM.OnGridModifyCategory(MModel, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexLoadingCategory();
            return PartialView("GridItemsCategory", BSM.ListCategory);
        }

        #endregion

        #region "GridItemsIndicator"
        public ActionResult GridItemsSection()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            BSM.OnIndexLoadingIndicator();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsSection", BSM.ListIndicatorByDept);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult SectionCreate(HRKPIIndicator MModel)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(MModel.Code))
                    {
                        ViewData["EditError"] = SYMessages.getMessage("CODE", user.Lang);
                    }
                    else
                    {
                        MModel.Code = MModel.Code.Trim().ToUpper();
                        var msg = BSM.OnGridModifyIndicator(MModel, SYActionBehavior.ADD.ToString());
                        if (msg == SYConstant.OK)
                        {
                            Session[Index_Sess_Obj + ActionName] = BSM;
                        }
                        else
                        {
                            ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                        }
                    }
                }
                catch (DbEntityValidationException e)
                {
                    ViewData["EditError"] = e.Message;
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

            BSM.OnIndexLoadingIndicator();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsSection", BSM.ListIndicatorByDept);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult SectionEdit(HRKPIIndicator MModel)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            try
            {
                var msg = BSM.OnGridModifyIndicator(MModel, SYActionBehavior.EDIT.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            catch (DbEntityValidationException e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.OnIndexLoadingIndicator();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsSection", BSM.ListIndicatorByDept);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult SectionDelete(HRKPIIndicator ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM = (IClsKPIConfig)Session[Index_Sess_Obj + ActionName];
            var msg = BSM.OnGridModifyIndicator(ModelObject, SYActionBehavior.DELETE.ToString());
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexLoadingIndicator();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsSection", BSM.ListIndicatorByDept);
        }

        #endregion

        #region KPI Grade
        public ActionResult GridItemGrade()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            BSM.OnIndexLoadingKPIGrade();
            return PartialView("GridItemGrade", BSM.ListKPIGrade);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateGrade(HRKPIGrade ModelObject)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    var msg = BSM.OnGridModifyGrade(ModelObject, SYActionBehavior.ADD.ToString());
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
            BSM.OnIndexLoadingKPIGrade();
            return PartialView("GridItemGrade", BSM.ListKPIGrade);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditGrade(HRKPIGrade ModelObject)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    var msg = BSM.OnGridModifyGrade(ModelObject, SYActionBehavior.EDIT.ToString());
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
            BSM.OnIndexLoadingKPIGrade();
            return PartialView("GridItemGrade", BSM.ListKPIGrade);
        }
        // delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteGrade(HRKPIGrade ModelObject)
        {
            UserSession();
            try
            {

                BSM = (IClsKPIConfig)Session[Index_Sess_Obj + ActionName];
                var msg = BSM.OnGridModifyGrade(ModelObject, SYActionBehavior.DELETE.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.OnIndexLoadingKPIGrade();
            return PartialView("GridItemGrade", BSM.ListKPIGrade);
        }
        #endregion

        #region KPI Norm
        public ActionResult GridItemNorm()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            BSM.OnIndexLoadingKPINorm();
            return PartialView("GridItemNorm", BSM.ListKPINorm);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateNorm(HRKPINorm ModelObject)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    var msg = BSM.OnGridModifyNorm(ModelObject, SYActionBehavior.ADD.ToString());
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
            BSM.OnIndexLoadingKPINorm();
            return PartialView("GridItemNorm", BSM.ListKPINorm);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditNorm(HRKPINorm ModelObject)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    var msg = BSM.OnGridModifyNorm(ModelObject, SYActionBehavior.EDIT.ToString());
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
            BSM.OnIndexLoadingKPINorm();
            return PartialView("GridItemNorm", BSM.ListKPINorm);
        }
        // delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteNorm(HRKPINorm ModelObject)
        {
            UserSession();
            try
            {

                BSM = (IClsKPIConfig)Session[Index_Sess_Obj + ActionName];
                var msg = BSM.OnGridModifyNorm(ModelObject, SYActionBehavior.DELETE.ToString());
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.OnIndexLoadingKPINorm();
            return PartialView("GridItemNorm", BSM.ListKPINorm);
        }
        #endregion


        #region "KPI Action"
        public ActionResult GridKPIAction()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            UserConf(ActionBehavior.EDIT);
            BSM.OnLoadingAction();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridKPIAction", BSM.ListKPIAction);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateKPIAction(HRKPIAction MModel)
        {
            UserSession();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
                try
                {
                    var msg = BSM.OnGridModifyAction(MModel, SYActionBehavior.ADD.ToString());
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

            BSM.OnLoadingAction();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridKPIAction", BSM.ListKPIAction);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditKPIAction(HRKPIAction MModel)
        {
            UserSession();
            UserConfListAndForm();
            ClsKPIConfig BSM = new ClsKPIConfig();
            if (ModelState.IsValid)
            {
                try
                {
                    var msg = BSM.OnGridModifyAction(MModel, SYActionBehavior.EDIT.ToString());
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


            BSM.OnLoadingAction();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridKPIAction", BSM.ListKPIAction);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteKPIAction(HRKPIAction MModel)
        {
            UserSession();
            UserConfListAndForm();
            try
            {
                var msg = BSM.OnGridModifyAction(MModel, SYActionBehavior.DELETE.ToString());
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

            BSM.OnLoadingAction();
            return PartialView("GridKPIAction", BSM.ListKPIAction);
        }
        #endregion

        #region "KPI NON Finance"
        public ActionResult GridNonFinance()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            UserConf(ActionBehavior.EDIT);
            BSM.OnLoadingNonFinance();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridNonFinance", BSM.ListKPINonFinance);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateNonFinance(HRKPINonFinance MModel)
        {
            UserSession();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
               try
                {
                    var msg = BSM.OnGridModifyNonFinance(MModel, SYActionBehavior.ADD.ToString());
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
            BSM.OnLoadingNonFinance();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridNonFinance", BSM.ListKPINonFinance);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditNonFinance(HRKPINonFinance MModel)
        {
            UserSession();
            UserConfListAndForm();
            ClsKPIConfig BSM = new ClsKPIConfig();
            if (ModelState.IsValid)
            {
                try
                {
                    var msg = BSM.OnGridModifyNonFinance(MModel, SYActionBehavior.EDIT.ToString());
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
            BSM.OnLoadingNonFinance();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridNonFinance", BSM.ListKPINonFinance);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteNonFinance(HRKPINonFinance MModel)
        {
            UserSession();
            UserConfListAndForm();
            try
            {
                var msg = BSM.OnGridModifyNonFinance(MModel, SYActionBehavior.DELETE.ToString());
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

            BSM.OnLoadingNonFinance();
            return PartialView("GridNonFinance", BSM.ListKPINonFinance);
        }
        #endregion

        #region "KPI List"
        public ActionResult GridKPIList()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            UserConf(ActionBehavior.EDIT);
            BSM.OnIndexLoadingKPIList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridKPIList", BSM.ListKPIList);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateKPIList(HRKPIList ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(ModelObject.Code))
                    {
                        ViewData["EditError"] = SYMessages.getMessage("CODE", user.Lang);
                    }
                    else
                    {
                        var msg = BSM.OnGridModifyKPI(ModelObject, SYActionBehavior.ADD.ToString());
                        if (msg == SYConstant.OK)
                        {
                            Session[Index_Sess_Obj + ActionName] = BSM;
                        }
                        else
                        {
                            ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
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

            BSM.OnIndexLoadingKPIList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridKPIList", BSM.ListKPIList);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditKPIList(HRKPIList ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            if (ModelState.IsValid)
            {
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
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.OnIndexLoadingKPIList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridKPIList", BSM.ListKPIList);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteKPIList(HRKPIList ModelObject)
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
            BSM.OnIndexLoadingKPIList();
            return PartialView("GridKPIList", BSM.ListKPIList);
        }
        #endregion

        #region KPI List By Dept
        public ActionResult GridKPIListByDept()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexLoadingDept();
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
            BSM.OnIndexLoadingDept();
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
            BSM.OnIndexLoadingDept();
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
            BSM.OnIndexLoadingDept();
            return PartialView("GridKPIListByDept", BSM.ListKPIListByDept);
        }
        #endregion

        #region KPI By Indicator BY BU
        public ActionResult GridIndicatorByBU()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexIndicatorBU();
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
            BSM.OnIndexIndicatorBU();
            return PartialView("GridIndicatorByBU", BSM.ListIndicatorByBU);

        }
        public ActionResult EditIndiByBU(HRKPIIndicator ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            try
            {
                var msg = BSM.OnGridModifyIndicatorBU(ModelObject, SYActionBehavior.EDIT.ToString(), "BU");
                if (msg != SYConstant.OK)
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.OnIndexIndicatorBU();
            return PartialView("GridIndicatorByBU", BSM.ListIndicatorByBU);
        }
        public ActionResult DeleteIndiByBU(HRKPIIndicator ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM = (IClsKPIConfig)Session[Index_Sess_Obj + ActionName];
            var msg = BSM.OnGridModifyIndicatorBU(ModelObject, SYActionBehavior.DELETE.ToString(), "BU");
            if (msg != SYConstant.OK)
            {
                ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
            }
            BSM.OnIndexIndicatorBU();
            return PartialView("GridIndicatorByBU", BSM.ListIndicatorByBU);
        }
        #endregion

        #region KPI List By BU
        public ActionResult GridKPIListByBU()
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexLoadingBU();
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
                    var msg = BSM.OnGridModifyKPIBU(ModelObject, SYActionBehavior.ADD.ToString(), "BU", true);
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
            BSM.OnIndexLoadingBU();
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
            BSM.OnIndexLoadingBU();
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
            BSM.OnIndexLoadingBU();
            return PartialView("GridKPIListByBU", BSM.ListKPIListByBU);
        }
        #endregion

        [HttpPost]
        public ActionResult CalculateDate(DateTime StartDate, DateTime EndDate, string Actionname)
        {
            ActionName = Actionname;
            UserSession();
            UserConfListAndForm();

            int Period = DateTimeHelper.CountMonth(StartDate, EndDate);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIConfig)Session[Index_Sess_Obj + ActionName];
            }
            var data = new
            {
                Period = Math.Abs(Period),
                MS = SYConstant.OK,
            };
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Json(data, JsonRequestBehavior.DenyGet);
        }
        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelectorLoading())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}