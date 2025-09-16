using DevExpress.Web.Mvc;
using DevExpress.XtraReports.UI;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Models.Report.Payroll;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Humica.Controllers.Reporting
{
    public class RPTBankListReportController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "RPTPR00008";
        private const string URL_SCREEN = "/Reporting/RPTBankListReport/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        HumicaDBContext DB = new HumicaDBContext();
        public string FirstPayTempt { get; set; }

        public RPTBankListReportController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "Index"
        public ActionResult Index()
        {
            ActionName = "Print";
            DataList();
            UserSession();
            UserConfListAndForm();
            FTFilterEmployee Filter = new FTFilterEmployee();
            Filter.InMonth = DateTime.Now;
            Filter.ValueDate = DateTime.Now;
            Filter.BankName = "CC";
            return View(Filter);
        }
        [HttpPost]
        public ActionResult Index(FTFilterEmployee Filter)
        {
            ActionName = "Print";
            DataList();
            UserSession();
            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = Filter;
            return View("ReportView", Filter);
        }
        public ActionResult DocumentViewerPartial()
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var RPT = (FTFilterEmployee)Session[Index_Sess_Obj + ActionName];
                    FirstPayTempt = RPT.BankName;
                    XtraReport sa = new XtraReport();
                    if (RPT.IsFirstPay == true)
                    {
                        FirstPayTempt = RPT.BankName + "FPay";
                        if (RPT.BankName == "ABA")
                        {
                            sa = new RPTBankABAFPay();
                        }
                        else if (RPT.BankName == "CAMPU")
                        {
                            sa = new RPTBankCamPuFPay();
                        }
                        else if (RPT.BankName == "PNB")
                        {
                            sa = new RPTBankPrinceFPay();
                        }
                        else if (RPT.BankName == "ACLEDA")
                        {
                            sa = new RptBankAcledaFPay();
                        }
                        else
                        {
                            sa = new RPTBankCashReport();
                        }
                    }
                    else
                    {
                        if (RPT.BankName == "ABA")
                        {
                            sa = new RPTBankABAReport();
                        }
                        else if (RPT.BankName == "CU")
                        {
                            sa = new RPTBankCU();
                        }
                        else if (RPT.BankName == "CAMPU")
                        {
                            sa = new RptBankCamPu();
                        }
                        else if (RPT.BankName == "ACLEDA")
                        {
                            sa = new RptBankAcleda();
                        }
                        else if (RPT.BankName == "W")
                        {
                            sa = new RptBankWin();
                        }
                        else if (RPT.BankName == "CIMB")
                        {
                            sa = new RptBankCamPu();
                        }
                        else if (RPT.BankName == "PNB")
                        {
                            sa = new RPTBankPrince();
                        }
                        else
                        {
                            sa = new RPTBankCashReport();
                        }
                    }
                    var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREEN_ID &&
                    w.DocType == FirstPayTempt && w.IsActive == true).ToList();
                    if (objRpt.Count > 0)
                    {
                        sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                    }

                    var Dict = RPT.GetType()
     .GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .ToDictionary(prop => prop.Name, prop => prop.GetValue(RPT, null));

                    foreach (var read in sa.Parameters)
                    {
                        if (Dict[read.Name] == null)
                        {
                            sa.Parameters[read.Name].Value = "";
                            if (read.Name == "Branch")
                            {
                                sa.Parameters[read.Name].Value = SYConstant.Branch_Condition;
                            }
                            if (read.Name == "Company")
                            {
                                sa.Parameters[read.Name].Value = SYConstant.Company_Condition;
                            }
                        }
                        else
                        {
                            sa.Parameters[read.Name].Value = Dict[read.Name].ToString();
                        }

                        read.Visible = false;
                    }
                    Session[Index_Sess_Obj] = sa;

                    Session[Index_Sess_Obj + ActionName] = RPT;

                    return PartialView("PrintForm", sa);
                }
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserID.ToString();
                log.DocurmentAction = "Print";
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo()
        {
            ActionName = "Print";

            if (Session[Index_Sess_Obj] != null)
            {
                XtraReport reportModel = (XtraReport)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion
        private void DataList()
        {
            var Branch = SYConstant.getBranchDataAccess();
            ViewData["BankName_SELECT"] = DB.HRBanks.Where(w => w.IsActive == true).ToList();
            ViewData["COMPANY_SELECT"] = SYConstant.getCompanyDataAccess();
            ViewData["BRANCHES_SELECT"] = Branch.ToList();
            ViewData["DIVISION_SELECT"] = DB.HRDivisions.ToList();
        }
    }
}
