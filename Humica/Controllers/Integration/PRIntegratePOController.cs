using DevExpress.Web.Mvc;
using DevExpress.XtraReports.UI;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using HUMICA.Models.Report.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{

    public class PRIntegratePOController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRI0000003";
        private const string URL_SCREEN = "/PR/PRM/PRIntegratePO/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "IntegrateNo;Status";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public PRIntegratePOController()
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
            DataSelector();

            PRGLmappingObject BSM = new PRGLmappingObject();
            BSM.Filter = new Humica.Core.FT.FTFilterData();
            BSM.Filter.PaymentDate = DateTime.Now;
            BSM.Filter.PostingDate = DateTime.Now;
            BSM.ListGLCharge = new List<ClsAccount>();
            BSM.ListPOReference = DBV.PR_POPending_view.ToList();
            BSM.ListHeaderPO = DB.PRIntegratePOes.ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult _GLCharge()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            PRGLmappingObject BSM = new PRGLmappingObject();
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_GLCharge", BSM.ListPOItem);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PRGLmappingObject BSM = new PRGLmappingObject();
            BSM.ListHeaderPO = new List<PRIntegratePO>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeaderPO);
        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(KeyName);
            PRGLmappingObject BSM = new PRGLmappingObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListPOReference);
        }
        #endregion
        #region "Create"
        public ActionResult Create(int InYear, int InMonth, string Branch,
            string Warehouse, string Project,string DocType)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRGLmappingObject BSM = new PRGLmappingObject();
            BSM.HeaderPO = new PRIntegratePO();
            BSM.ListPOItem = new List<PRIntegratePOItem>();
            BSM.ListPOItem = BSM.LoadDataPOItem(InYear, InMonth, Branch, Warehouse, Project, DocType);
            if (BSM.ListPOItem.Count > 0)
            {
                BSM.HeaderPO.Branch = Branch;
                BSM.HeaderPO.Project = Project;
                BSM.HeaderPO.DocumentDate = DateTime.Now;
                BSM.HeaderPO.Status = SYDocumentStatus.OPEN.ToString();
                BSM.HeaderPO.PaymentDate = new DateTime(InYear, InMonth, DateTime.DaysInMonth(InYear, InMonth));
                BSM.HeaderPO.Amount = 0;
                foreach (var item in BSM.ListPOItem)
                {
                    BSM.HeaderPO.Amount += Convert.ToDecimal(item.Amount);
                }
                var Company = SMS.SYHRCompanies.First();
                if (Company != null)
                {
                    BSM.HeaderPO.CompanyCode = Company.CompanyCode;
                    BSM.HeaderPO.CurrencyCode = Company.BaseCurrency;
                }
                BSM.ScreenId = SCREEN_ID;
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("NO_REQUEST");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }

            return View(BSM);

        }

        [HttpPost]
        public ActionResult Create(int InYear, int InMonth, string Branch,
            string Warehouse, string Project, string DocType, PRGLmappingObject BSM)
        {
            UserSession();
            //DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
                BSM.ListPOItem = obj.ListPOItem;
            }
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                BSM.HeaderPO.InYear = InYear;
                BSM.HeaderPO.InMonth = InMonth;
                string msg = BSM.CreatePO(DocType);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.HeaderPO.IntegrateNo;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
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
        public ActionResult Details(string ID)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ViewData[SYSConstant.PARAM_ID] = ID;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (ID != null)
            {
                PRGLmappingObject BSM = new PRGLmappingObject();
                BSM.HeaderPO = DB.PRIntegratePOes.FirstOrDefault(w => w.IntegrateNo == ID);
                if (BSM.HeaderPO != null)
                {
                    BSM.ListPOItem = DB.PRIntegratePOItems.Where(w => w.IntegrateNo == ID).ToList();

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region 'Print'
        public ActionResult Print(string id)
        {
            this.UserSession();
            this.UserConf(ActionBehavior.VIEWONLY);
            ViewData[SYSConstant.PARAM_ID] = id;
            this.UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string id)
        {
            this.UserSession();
            this.UserConf(ActionBehavior.VIEWONLY);
            this.ActionName = "Print";
            var obj = DB.PRIntegratePOItems.Where(w => w.IntegrateNo == id).ToList();
            if (obj.Count > 0)
            {
                try
                {
                    ViewData[SYSConstant.PARAM_ID] = id;
                    FRMPO FRMRequest = new FRMPO();

                    var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREEN_ID && w.IsActive == true).ToList();
                    if (objRpt.Count > 0)
                    {
                        FRMRequest.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                    }

                    FRMRequest.Parameters["IntergrateNO"].Value = obj.First().IntegrateNo;
                    FRMRequest.Parameters["IntergrateNO"].Visible = false;

                    Session[this.Index_Sess_Obj + this.ActionName] = FRMRequest;
                    return PartialView("PrintForm", FRMRequest);
                }
                catch (Exception ex)
                {
                    SYEventLogObject.saveEventLog(new SYEventLog()
                    {
                        ScreenId = SCREEN_ID,
                        UserId = this.user.UserID.ToString(),
                        DocurmentAction = id,
                        Action = SYActionBehavior.ADD.ToString()
                    }, ex, true);
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string RequestNo)
        {
            ActionName = "Print";
            FRMPO reportModel = (FRMPO)Session[Index_Sess_Obj + ActionName];
            ViewData[SYSConstant.PARAM_ID] = RequestNo;
            return ReportViewerExtension.ExportTo(reportModel);
        }
        #endregion 
        public async Task<ActionResult> CUSCEN(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();

            if (id != null)
            {
                Humica.Integration.EF.StaffIntegrateObject BSM = new Humica.Integration.EF.StaffIntegrateObject();
                BSM.ScreenId = SCREEN_ID;
                string msg = await BSM.Release(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RELEASED", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    var mess = SYMessages.getMessageObject(msg, user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }

            }
            //Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Details?id=" + id);
        }

        private void DataSelector()
        {
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["COMPANY_CODE"] = SMS.SYHRCompanies.ToList();
            ViewData["JOURNAL_SELECT"] = DB.HRJournalTypes.ToList();
            ViewData["Vendor_SELECT"] = DB.CFExVendors.ToList();
        }
    }
}
