using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.SY;
using Humica.Performance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.Performance
{
    public class ESSKPIPlanIndivController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESSA000004";
        private const string URL_SCREEN = "/SelfService/Performance/ESSKPIPlanIndiv/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code;Status";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public ESSKPIPlanIndivController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region Index
        public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ClsKPIPlanIndividual)Session[Index_Sess_Obj + ActionName];
            }
            var ListHeader = await BSM.LoadData(true);
            BSM.ListPlanPending = await BSM.LoadDataPlanInPending(true);
            BSM.ListHeader = ListHeader.OrderByDescending(w => w.DocumentDate).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsKPIPlanIndividual)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(KeyName);
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsKPIPlanIndividual)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListPlanPending);
        }
        #endregion
        #region Create
        public async Task<ActionResult> Create(string id)
        {
            ActionName = "Create";
            UserSession();
            await DataSelector();
            UserConfListAndForm(this.KeyName);
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (string.IsNullOrEmpty(id))
            {
                BSM.Header = new HRKPIPlanIndividual();
                BSM.ListItem = new List<HRKPIPlanIndivItem>();
            }
            else
            {
                await BSM.GetDataPlanIndivid(id);
                await DataList(BSM.Header.CriteriaType);
                await EmployeeKPI(BSM.Header.CriteriaType, ActionName);
            }

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public async Task<ActionResult> Create(ClsKPIPlanIndividual collection)
        {
            UserSession();
            await DataSelector();
            UserConfListAndForm(this.KeyName);
            ActionName = "Create";
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsKPIPlanIndividual)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                BSM.Header = collection.Header;

                string msg = await BSM.CreateKPIPlanIndvid(BSM.Header.Code);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                    mess_err.DocumentNumber = collection.Header.Code.ToString();
                    mess_err.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + collection.Header.Code;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);

                }
            }
            await DataList(BSM.Header.CriteriaType);
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");

        }
        #endregion
        #region Edit
        public async Task<ActionResult> Edit(string ID)
        {
            ActionName = "Create";
            UserSession();
            await DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            if (ID != null)
            {
                ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
                BSM.Header = await DB.HRKPIPlanIndividuals.FirstOrDefaultAsync(w => w.Code == ID);
                if (BSM.Header != null)
                {
                    BSM.ListItem = await DB.HRKPIPlanIndivItems.Where(w => w.KPIPlanCode == ID).ToListAsync();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    await EmployeeKPI(BSM.Header.CriteriaType, ActionName);
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(string id, ClsKPIPlanIndividual collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            await DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsKPIPlanIndividual)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;

                string msg = await BSM.EditSetUpKPI(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
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
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return View(BSM);

        }
        #endregion
        #region "Delete"
        public async Task<ActionResult> Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            await DataSelector();
            if (id != null)
            {
                ClsKPIPlanIndividual Del = new ClsKPIPlanIndividual();
                string msg = Del.DeleteSetupKPI(id);
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
        #region Details
        public async Task<ActionResult> Details(string id)
        {
            ActionName = "Details";
            UserSession();
            await DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            if (id != null)
            {
                ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();

                BSM.Header = await DB.HRKPIPlanIndividuals.FirstOrDefaultAsync(w => w.Code == id);
                if (BSM.Header != null)
                {
                    BSM.ListItem = await DB.HRKPIPlanIndivItems.Where(w => w.KPIPlanCode == id).ToListAsync();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    await EmployeeKPI(BSM.Header.CriteriaType, ActionName);
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public async Task<ActionResult> GridItemsDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            await DataSelector();
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsKPIPlanIndividual)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsDetails", BSM.ListItem);
        }
        #endregion
        #region Copy
        public async Task<ActionResult> Copy(string id)
        {
            ActionName = "Create";
            UserSession();
            await DataSelector();
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
                BSM.Header = BSM.getSetupHeader(id);
                BSM.Header.CriteriaType = "";
                BSM.Header.CriteriaName = "";

                BSM.ListItem = await DB.HRKPIPlanIndivItems.Where(w => w.KPIPlanCode == id).ToListAsync();
                await DataList(BSM.Header.CriteriaType);
                Session[Index_Sess_Obj + ActionName] = BSM;

                if (BSM.Header != null)
                {
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public async Task<ActionResult> Copy(ClsKPIPlanIndividual collection, string id)
        {
            ActionName = "Create";
            UserSession();
            await DataSelector();
            UserConfForm(SYActionBehavior.ADD);
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsKPIPlanIndividual)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                BSM.Header = collection.Header;

                string msg = await BSM.CreateKPIPlan("HRA0000002");

                if (msg == SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                    mess_err.DocumentNumber = collection.Header.Code.ToString();
                    mess_err.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + collection.Header.Code;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        public async Task<ActionResult> ReleaseDoc(string id)
        {
            UserSession();
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = await BSM.ReleaseTheDoc(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    var mess = SYMessages.getMessageObject(msg, user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public async Task<ActionResult> RequestForApproval(string id)
        {
            UserSession();
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = await BSM.RequestRelease(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    var mess = SYMessages.getMessageObject(msg, user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        public async Task<ActionResult> GetTotalWeight(string Action)
        {
            ActionName = Action;
            UserSession();
            UserConfListAndForm();
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsKPIPlanIndividual)Session[Index_Sess_Obj + ActionName];
                var ListItem = BSM.ListItem.ToList();
                decimal? TotalWeight = ListItem.Sum(w => w.Weight);
                if (TotalWeight <= 100)
                {
                    var data = new
                    {
                        MS = SYSConstant.OK,
                        TotalW = TotalWeight
                    };
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        #region Create Item
        public async Task<ActionResult> GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            await DataSelector();
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            BSM.ListItem = new List<HRKPIPlanIndivItem>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsKPIPlanIndividual)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            await EmployeeKPI(BSM.Header.CriteriaType, ActionName);
            return PartialView("GridItems", BSM.ListItem);
        }
        public async Task<ActionResult> CreateItems(HRKPIPlanIndivItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            await DataSelector();
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsKPIPlanIndividual)Session[Index_Sess_Obj + ActionName];
                    }
                    var msg = await BSM.ModifyItem(ModelObject, SYActionBehavior.ADD.ToString());
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
            Session[Index_Sess_Obj + ActionName] = BSM;
            await EmployeeKPI(BSM.Header.CriteriaType, ActionName);
            return PartialView("GridItems", BSM.ListItem);
        }
        public async Task<ActionResult> EditItems(HRKPIPlanIndivItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            await DataSelector();
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();

            BSM.ListItem = new List<HRKPIPlanIndivItem>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsKPIPlanIndividual)Session[Index_Sess_Obj + ActionName];
                var msg = await BSM.ModifyItem(ModelObject, SYActionBehavior.EDIT.ToString());
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
                await EmployeeKPI(BSM.Header.CriteriaType, ActionName);
            }
            return PartialView("GridItems", BSM.ListItem);
        }
        public async Task<ActionResult> DeleteItems(HRKPIPlanIndivItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            await DataSelector();
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsKPIPlanIndividual)Session[Index_Sess_Obj + ActionName];
                var msg = await BSM.ModifyItem(ModelObject, SYActionBehavior.DELETE.ToString());
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItems", BSM.ListItem);
        }
        #endregion
        private async Task EmployeeKPI(string CriteriaType, string actionname)
        {
            ClsKPIPlanIndividual BSM = new ClsKPIPlanIndividual();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsKPIPlanIndividual)Session[Index_Sess_Obj + ActionName];
            }
            ViewData["KPI_LIST"] = await BSM.LoadDataHRKPIList(CriteriaType);
            ViewData["LIST_Indicator"] = await BSM.LoadDataIndicator(CriteriaType);
        }
        
        private async Task DataSelector()
        {
            //ViewData["LIST_KPI_CATEGORY"] = await DB.HRKPICategories.Where(w => w.IsActive == true).ToListAsync();
            //ViewData["LIST_Indicator"] = await DB.HRKPIIndicators.Where(w => w.IsActive == true).ToListAsync();
            //ViewData["LIST_GROUPKPI"] = await DB.HRKPITypes.Where(w => w.IsActive == true).ToListAsync();
            ViewData["KPI_Measure"] = ClsMeasure.LoadDataMeasure();
            ViewData["KPI_Symbol"] = ClsMeasure.LoadDataSymbol();
            ViewData["KPI_LIST"] = await DB.HRKPILists.ToListAsync();

        }
        private async Task DataList(string Dept)
        {
            string Release = SYDocumentStatus.RELEASED.ToString();
            var ListStaff = await HRStaffProfileObject.LoadStaff();
            ViewData["STAFF_LIST"] = ListStaff.Where(w => w.Status == "A" && w.Dept == Dept).ToList();
        }
    }
}