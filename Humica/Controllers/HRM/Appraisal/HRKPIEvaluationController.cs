using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.Helper;
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

namespace Humica.Controllers.HRM.Appraisal
{
    public class HRKPIEvaluationController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRA0000019";
        private const string URL_SCREEN = "/HRM/Appraisal/HRKPIEvaluation/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "KPIEvaCode";
        IClsKPIEvaluation BSM;
        IUnitOfWork unitOfWork;
        public HRKPIEvaluationController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsKPIEvaluation();
            BSM.OnLoad();
            unitOfWork = new UnitOfWork<HumicaDBContext>(new HumicaDBContext());
        }
        #region List
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIEvaluation)Session[Index_Sess_Obj + ActionName];
            }
            BSM.OnIndexLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsKPIEvaluation BSM)
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
                BSM = (IClsKPIEvaluation)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeander);
        }
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIEvaluation)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListKPIPending);
        }
        #endregion
        #region Create
        public ActionResult Create(string id, string EmpCode)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnCreatingLoading(id, EmpCode);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(string id,ClsKPIEvaluation collection)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIEvaluation)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Header = collection.Header;
            BSM.Header.DocRef = id;
            BSM.ScreenId = SCREEN_ID;
            string msg =  BSM.Create(SCREEN_ID);
            if (msg == SYConstant.OK)
            {
                SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                mess_err.DocumentNumber = BSM.Header.KPIEvaCode;
                mess_err.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess_err.DocumentNumber;
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + BSM.Header.KPIEvaCode);
            }
            else
            {
                SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                ViewData[SYConstant.MESSAGE_SUBMIT] = mess_err;
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);

            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
        }
        #endregion
        #region
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            BSM.OnDetailLoading(id);
            if (BSM.Header != null)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
        }
        [HttpPost]

        public ActionResult Edit(string id, ClsKPIEvaluation collection)
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
                    BSM = (IClsKPIEvaluation)Session[Index_Sess_Obj + ActionName];
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
        #region Details
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            BSM.OnDetailLoading(id);
            if (BSM.Header != null)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
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
            var SD = unitOfWork.Set<HRKPIEvaluation>().FirstOrDefault(w => w.KPIEvaCode == id);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    var sa = new RptKPIEvaluation();
                    var objRpt = unitOfWork.Set<CFReportObject>().Where(w => w.ScreenID == SCREEN_ID
                   && w.IsActive == true).ToList();
                    if (objRpt.Count > 0)
                    {
                        sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                    }
                    sa.Parameters["KPIEvaCode"].Value = SD.KPIEvaCode;
                    sa.Parameters["KPIEvaCode"].Visible = false;

                    Session[Index_Sess_Obj + ActionName] = sa;
                    Session[Index_Sess_Obj] = sa;
                    return PartialView("PrintForm", sa);
                }
                catch (Exception e)
                {
                    ClsEventLog.Save_EventLog(SCREEN_ID, user.UserName, id, SYActionBehavior.ADD.ToString(), e, true);
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string id)
        {
            ActionName = "Print";

            if (Session[Index_Sess_Obj] != null)
            {
                RptKPIEvaluation reportModel = (RptKPIEvaluation)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion

        #region Action
        public ActionResult Approve(string id)
        {
            UserSession();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.ApproveDoc(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.DocumentNumber = id;
                    mess.Description = mess.Description;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        [HttpPost]
        public ActionResult RatingChange(string Code, string Value, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsKPIEvaluation)Session[Index_Sess_Obj + ActionName];
                if (string.IsNullOrEmpty(Value)) Value = "0";
                decimal Score = Convert.ToDecimal(Value);
                decimal SubScore = 0;
                decimal TotalScoreEval = 0;
                var checkexist = BSM.ListItem.FirstOrDefault(w => w.ItemCode == Code);
                if (checkexist != null)
                {
                    if (checkexist.Weight * 100 < Score)
                    {
                        Score = checkexist.Weight * 100;
                    }
                    checkexist.ScoreEval = Score;
                    SubScore = ClsRounding.Rounding((checkexist.ScoreEval.Value + checkexist.Score.Value) / 2.00M, 2, "N");
                    checkexist.SubScore = SubScore;
                }
                TotalScoreEval = BSM.ListItem.Where(x=>x.ScoreEval.HasValue).Sum(w => w.ScoreEval.Value);
                var result = new
                {
                    MS = SYConstant.OK,
                    SubScore = SubScore,
                    Scoring = Score,
                    TotalScore = TotalScoreEval
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }

        private void DataSelector()
        {
        }
    }
}