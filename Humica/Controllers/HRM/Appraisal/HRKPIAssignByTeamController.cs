using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Models.SY;
using Humica.Performance;
using HUMICA.Models.Report.KPI;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HRKPIAssignByTeamController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRA0000015";
        private const string URL_SCREEN = "/HRM/Appraisal/HRKPIAssignByTeam/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "AssignCode";
        protected IClsKPIAssign BSM;
        private IUnitOfWork unitOfWork;
        public HRKPIAssignByTeamController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new CLsKPIAssign();
            BSM.OnLoad();
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        #region List
        public ActionResult Index()
        {
            UserSession();
            ActionName = "Index";
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
            }
            BSM.OnIndexPending();
            BSM.OnIndexLoading(SYDocumentASSIGN.BYTEAM.ToString());
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(CLsKPIAssign BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.OnIndexPending();
            BSM.OnIndexLoading(SYDocumentASSIGN.BYTEAM.ToString());
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
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListProcess", BSM.listAssignHeader);
        }
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListPending);
        }
        #endregion

        #region Create
        public ActionResult Create(string id)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            var msg = BSM.OnCreateMultiLoading(id);
            if (msg != SYConstant.OK)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            EmployeeKPI(BSM.AssignHeader.CriteriaType, BSM.AssignHeader.PeriodTo.Value.Year);
            DataList(BSM.AssignHeader.CriteriaType);
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(string id, CLsKPIAssign collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            string CriteriaType = "";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
                CriteriaType = BSM.AssignHeader.CriteriaType;
            }
            BSM.ScreenId = SCREEN_ID;
            BSM.AssignHeader = collection.AssignHeader;
            BSM.AssignHeader.CriteriaType = CriteriaType;
            BSM.AssignHeader.AssignedBy = SYDocumentASSIGN.BYTEAM.ToString();
            string msg = BSM.CreateMulti("HRA0000003");
            if (msg == SYConstant.OK)
            {
                SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                mess_err.DocumentNumber = BSM.AssignHeader.AssignCode.ToString();
                mess_err.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess_err.DocumentNumber;
                Session[Index_Sess_Obj + ActionName] = BSM;
                Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
            }
            else
            {
                EmployeeKPI(BSM.AssignHeader.CriteriaType, BSM.AssignHeader.PeriodTo.Value.Year);
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                BSM.OnDetailMultiLoading(id);
                if (BSM.AssignHeader != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    EmployeeKPI(BSM.AssignHeader.CriteriaType, BSM.AssignHeader.PeriodTo.Value.Year);
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public ActionResult Edit(string id, CLsKPIAssign collection)
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
                    BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
                }
                BSM.AssignHeader = collection.AssignHeader;
                BSM.ScreenId = SCREEN_ID;
                BSM.AssignHeader.AssignedBy = SYDocumentASSIGN.BYTEAM.ToString();
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
                BSM.OnDetailMultiLoading(id);
                if (BSM.AssignHeader != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    EmployeeKPI(BSM.AssignHeader.CriteriaType, BSM.AssignHeader.PeriodTo.Value.Year);
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
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsDetails", BSM.listAssignInsertItem);
        }
        #endregion
        public ActionResult Calculate(string ID)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (ID != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
                }
                string resutl = BSM.Calculate(ID);
                if (resutl == SYSConstant.OK.ToString())
                {
                    SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #region Grid Create
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            EmployeeKPI(BSM.AssignHeader.CriteriaType, BSM.AssignHeader.PeriodTo.Value.Year);
            return PartialView("GridItems", BSM.listAssignInsertItem);
        }
        public ActionResult CreateItems(HRKPIAssignItem ModelObject)
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
                        BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
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
            EmployeeKPI(BSM.AssignHeader.CriteriaType, BSM.AssignHeader.PeriodTo.Value.Year);
            return PartialView("GridItems", BSM.listAssignInsertItem);

        }
        public ActionResult EditItems(HRKPIAssignItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
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
                EmployeeKPI(BSM.AssignHeader.CriteriaType, BSM.AssignHeader.PeriodTo.Value.Year);
            }
            return PartialView("GridItems", BSM.listAssignInsertItem);
        }
        public ActionResult DeleteItems(HRKPIAssignItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
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
                EmployeeKPI(BSM.AssignHeader.CriteriaType, BSM.AssignHeader.PeriodTo.Value.Year);
            }
            return PartialView("GridItems", BSM.listAssignInsertItem);
        }
        #endregion

        #region RequestReleaseKPI
        public ActionResult RequestForApproval(string id)
        {
            UserSession();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
            }
            if (id != null)
            {
                string resutl = BSM.RequestRelease(id);
                if (resutl == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("DOC_REQ", user.Lang);
                    mess.DocumentNumber = id;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        public ActionResult Approve(string id)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (!string.IsNullOrEmpty(id))
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Approved(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.Description = mess.Description + ". " + BSM.ErrorMessage;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Accept(string id)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (!string.IsNullOrEmpty(id))
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Accept(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.Description = mess.Description + ". " + BSM.ErrorMessage;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #region Copy
        public async Task<ActionResult> Copy(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            //DataList();
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                CLsKPIAssign BSM = new CLsKPIAssign();
                BSM.AssignHeader = await BSM.getSetupHeader(id);

                Session[Index_Sess_Obj + ActionName] = BSM;

                if (BSM.AssignHeader != null)
                {
                    if (BSM.listAssignInsertItem.Count() > 0)
                    {
                        BSM.listAssignInsertItem.ForEach(w => w.Acheivement = 0);
                        BSM.listAssignInsertItem.ForEach(w => w.Actual = 0);
                        BSM.listAssignInsertItem.ForEach(w => w.Score = 0);
                    }
                    EmployeeKPI(BSM.AssignHeader.CriteriaType, BSM.AssignHeader.PeriodTo.Value.Year);
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public async Task<ActionResult> Copy(CLsKPIAssign collection, string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            //DataList();
            UserConfForm(SYActionBehavior.ADD);
            CLsKPIAssign BSM = new CLsKPIAssign();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (CLsKPIAssign)Session[Index_Sess_Obj + ActionName];
                }
                string CriteriaType = "";
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (CLsKPIAssign)Session[Index_Sess_Obj + ActionName];
                    CriteriaType = BSM.AssignHeader.CriteriaType;
                }
                BSM.ScreenId = SCREEN_ID;
                BSM.AssignHeader = collection.AssignHeader;
                BSM.AssignHeader.CriteriaType = CriteriaType;
                string msg =await BSM.CreateAssign("HRA0000003");
                if (msg == SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                    mess_err.DocumentNumber = BSM.AssignHeader.AssignCode.ToString();
                    mess_err.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + collection.AssignHeader.AssignCode;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(BSM);
                }
                else
                {
                   EmployeeKPI(BSM.AssignHeader.CriteriaType, BSM.AssignHeader.PeriodTo.Value.Year);

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
            var SD = unitOfWork.Set<HRKPIAssignHeader>().FirstOrDefault(w => w.AssignCode == id);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    var sa = new RptKPIEmpByTeam();
                    var objRpt = unitOfWork.Set<CFReportObject>().Where(w => w.ScreenID == SCREEN_ID
                   && w.IsActive == true).ToList();
                    if (objRpt.Count > 0)
                    {
                        sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                    }
                    sa.Parameters["AssignCode"].Value = SD.AssignCode;
                    sa.Parameters["AssignCode"].Visible = false;

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
                RptKPIEmpByTeam reportModel = (RptKPIEmpByTeam)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion

        #region 'Grid Member'
        public ActionResult GridMember()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridMember", BSM.ListKPIMember);
        }
        public ActionResult GridMemberDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridMemberDetails", BSM.ListKPIMember);
        }
        public ActionResult CreateMember(HRKPIAssignMember ModelObject)
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
                        BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
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
                    Session[Index_Sess_Obj + ActionName] = BSM;

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridMember", BSM.ListKPIMember);
        }
        public ActionResult EditMember(HRKPIAssignMember ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
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
            }
            return PartialView("GridMember", BSM.ListKPIMember);
        }
        public ActionResult DeleteMember(HRKPIAssignMember ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
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
            return PartialView("GridMember", BSM.ListKPIMember);
        }
        #endregion 
        public ActionResult RefreshTotalMember(string action)
        {
            ActionName = action;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
                if (BSM.AssignHeader == null)
                {
                    BSM.AssignHeader = new HRKPIAssignHeader();
                }
                BSM.AssignHeader.Member = BSM.ListKPIMember.Count();

                var result = new
                {
                    MS = SYConstant.OK,
                    TotalAmount = BSM.AssignHeader.Member,
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult ShowData_Indicator(string Indicator)
        {
            Session[SYConstant.KPI_INDICATOR] = Indicator;
            CLsKPIAssign.GetKPIList();
            var result = new
            {
                MS = SYConstant.OK,
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult GetTotalWeight(string actionname)
        {
            ActionName = actionname;
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIAssign)Session[Index_Sess_Obj + ActionName];
                var ListItem = BSM.listAssignInsertItem.ToList();
                decimal? TotalWeight = 0;
                if (BSM.listAssignInsertItem.ToList().Count() > 0)
                {
                    TotalWeight = BSM.listAssignInsertItem.Sum(w => w.Weight);
                }

                Session[Index_Sess_Obj + ActionName] = BSM;
                var data = new
                {
                    TotalWeight = TotalWeight,
                    MS = SYSConstant.OK
                };
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        protected void EmployeeKPI(string CriteriaType, int InYear)
        {
            foreach (var data in BSM.OnDataSelectorKPI(CriteriaType, InYear))
            {
                ViewData[data.Key] = data.Value;
            }
        }
        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelectorLoading())
            {
                ViewData[data.Key] = data.Value;
            }
        }
        protected void DataList(string Dept)
        {
            foreach (var data in BSM.OnDataListByDept(Dept))
            {
                ViewData[data.Key] = data.Value;
            }
            //var ListStaff = await HRStaffProfileObject.LoadStaff();
            //ViewData["STAFF_LIST"] = ListStaff.Where(w => w.Status == "A").ToList();
        }
    }
}