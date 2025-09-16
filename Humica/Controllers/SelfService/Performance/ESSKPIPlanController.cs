using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Models.Report.KPI;
using Humica.Models.SY;
using Humica.Performance;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.Performance
{
    public class ESSKPIPlanController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESSA000003";
        private const string URL_SCREEN = "/SelfService/Performance/ESSKPIPlan/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code;Status";
        protected IClsKPIPlan BSM;
        private IUnitOfWork unitOfWork;
        public ESSKPIPlanController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new CLsKPIPlan();
            BSM.OnLoad();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        #region Index
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
            }
            BSM.OnIndexLoading(true);
            BSM.OnIndexLoadingPending();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(CLsKPIPlan BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.OnIndexLoading(true);
            BSM.OnIndexLoadingPending();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialListProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListProcess", BSM.ListHeader);
        }
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListHeaderPending);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string ID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            if (ID != null)
            {
                BSM.OnDetailLoading(ID);
                if (BSM.Header != null)
                {
                    EmployeeKPI(BSM.Header.CriteriaType);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, CLsKPIPlan collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;

                string msg = BSM.Update(id, true);
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

        #region Details
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            if (id != null)
            {
                BSM.OnDetailLoading(id);
                if (BSM.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    EmployeeKPI(BSM.Header.CriteriaType);
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
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsDetails", BSM.ListItem);
        }
        #endregion
        #region "Print"
        public ActionResult Print(string id)
        {
            ActionName = "Print";
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string id)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            var SD = unitOfWork.Set<HRKPIPlanHeader>().FirstOrDefault(w => w.Code == id);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    var sa = new RptKPIPlanDepartment();
                    var objRpt = unitOfWork.Set<CFReportObject>().Where(w => w.ScreenID == SCREEN_ID
                   && w.IsActive == true).ToList();
                    if (objRpt.Count > 0)
                    {
                        sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                    }
                    sa.Parameters["Code"].Value = SD.Code;
                    sa.Parameters["Code"].Visible = false;

                    Session[Index_Sess_Obj + ActionName] = sa;
                    Session[Index_Sess_Obj] = sa;
                    return PartialView("PrintForm", sa);
                }
                catch (Exception e)
                {
                    ClsEventLog.SaveEventLogs(SCREEN_ID, user.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string id)
        {
            ActionName = "Print";

            if (Session[Index_Sess_Obj] != null)
            {
                RptKPIPlanDepartment reportModel = (RptKPIPlanDepartment)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion
        #region 'Status'
        public ActionResult ReleaseDoc(string id)
        {
            UserSession();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.ReleaseTheDoc(id, true);
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
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        public ActionResult GetTotalWeight(string Action)
        {
            ActionName = Action;
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
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
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            EmployeeKPI(BSM.Header.CriteriaType);
            return PartialView("GridItems", BSM.ListItem);
        }
        public ActionResult CreateItems(HRKPIPlanItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
                    }
                    var msg = BSM.OnGridModify(ModelObject, SYActionBehavior.ADD.ToString());
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
            EmployeeKPI(BSM.Header.CriteriaType);
            return PartialView("GridItems", BSM.ListItem);
        }
        public ActionResult EditItems(HRKPIPlanItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
                var msg = BSM.OnGridModify(ModelObject, SYActionBehavior.EDIT.ToString());
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
                EmployeeKPI(BSM.Header.CriteriaType);
            }
            return PartialView("GridItems", BSM.ListItem);
        }
        public ActionResult DeleteItems(HRKPIPlanItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
                var msg = BSM.OnGridModify(ModelObject, SYActionBehavior.DELETE.ToString());
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
        protected void EmployeeKPI(string CriteriaType)
        {
            foreach (var data in BSM.OnDataSelectorKPI(CriteriaType))
            {
                ViewData[data.Key] = data.Value;
            }
        }

        private void DataSelector()
        {
            //ViewData["LIST_KPI_CATEGORY"] = await DB.HRKPICategories.Where(w => w.IsActive == true).ToListAsync();
            //ViewData["LIST_Indicator"] = await DB.HRKPIIndicators.Where(w => w.IsActive == true).ToListAsync();
            //ViewData["LIST_GROUPKPI"] = await DB.HRKPITypes.Where(w => w.IsActive == true).ToListAsync();
            ViewData["KPI_Measure"] = ClsMeasure.LoadDataMeasure();
            ViewData["KPI_Symbol"] = ClsMeasure.LoadDataSymbol();
            //ViewData["KPI_LIST"] = await DB.HRKPILists.ToListAsync();

        }
    }
}