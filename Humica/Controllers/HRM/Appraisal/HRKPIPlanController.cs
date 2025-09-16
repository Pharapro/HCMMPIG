using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using Humica.Models.Report.KPI;
using Humica.Models.SY;
using Humica.Performance;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HRKPIPlanController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRA0000002";
        private const string URL_SCREEN = "/HRM/Appraisal/HRKPIPlan/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "LineItem";
        protected IClsKPIPlan BSM;
        private IUnitOfWork unitOfWork;
        public HRKPIPlanController()
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
            BSM.OnIndexLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(CLsKPIPlan BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.OnIndexLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            DataList();
            UserConfListAndForm(this.KeyName);
            BSM.OnCreatingLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(CLsKPIPlan collection)
        {
            UserSession();
            DataSelector();
            DataList();
            UserConfListAndForm(this.KeyName);
            ActionName = "Create";
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                BSM.Header = collection.Header;

                string msg = BSM.Create();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                    mess_err.DocumentNumber = collection.Header.Code.ToString();
                    mess_err.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + collection.Header.Code;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);

                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");

        }
        #endregion

        #region Edit
        public ActionResult Edit(string ID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            DataList();
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
            DataList();
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

                string msg = BSM.Update(id);
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
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id != null)
            {
                string msg = BSM.Delete(id);
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

        #region Copy
        public ActionResult Copy(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            DataList();
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                BSM.GetDataCopy(id);
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
        public ActionResult Copy(CLsKPIPlan collection, string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            DataList();
            UserConfForm(SYActionBehavior.ADD);
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                BSM.Header = collection.Header;

                string msg = BSM.Create();

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

        public ActionResult ReleaseDoc(string id)
        {
            UserSession();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.ReleaseTheDoc(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RELEASED", user.Lang);
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
        public ActionResult RequestForApproval(string id)
        {
            UserSession();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.RequestRelease(id);
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

        public ActionResult SelectCriteriaType(string BU, string DepartmentCode, string Action)
        {
            ActionName = Action;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIPlan)Session[Index_Sess_Obj + ActionName];
            }
            if (BU != null)
            {
                if (BU == "BSN")
                {
                    ViewData["LIST_DEPARTMENT"] = ClsFilter.LoadBusinessUnit();
                }
                else if (BU == "DPM")
                {
                    ViewData["LIST_DEPARTMENT"] = ClsFilter.LoadDepartment();
                }
                else if (BU == "BRANCH")
                {
                    ClsFilter _fil = new ClsFilter();
                    ViewData["LIST_DEPARTMENT"] = _fil.LoadBranch();
                }
                var data = new
                {
                    MS = SYConstant.OK,
                    DATA = ViewData["LIST_DEPARTMENT"]
                };
                return Json(data, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }

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

        public ActionResult GetPeriod(string Code)
        {
            DateTime PeriodFrom = DateTime.Now;
            DateTime PeriodTo = DateTime.Now;
            HRKPIType Plan = unitOfWork.Set<HRKPIType>().FirstOrDefault(w => w.Code == Code && w.IsActive == true);
            if (Plan != null)
            {
                if (Plan.StartDate.HasValue)
                    PeriodFrom = Plan.StartDate.Value;
                if (Plan.EndDate.HasValue)
                    PeriodTo = Plan.EndDate.Value;
            }
            var data = new
            {
                PeriodFrom = PeriodFrom,
                PeriodTo = PeriodTo,
                MS = SYConstant.OK
            };
            return Json(data, (JsonRequestBehavior)1);
        }
        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelectorLoading())
            {
                ViewData[data.Key] = data.Value;
            }
        }
        protected void EmployeeKPI(string CriteriaType)
        {
            foreach (var data in BSM.OnDataSelectorKPI(CriteriaType))
            {
                ViewData[data.Key] = data.Value;
            }
        }
        protected void DataList()
        {
            foreach (var data in BSM.OnDataList())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}