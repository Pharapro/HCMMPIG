using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.Report.Payroll;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Humica.Controllers.Reporting
{
    public class RPTInsuranceReportController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "RPTRIS0005";
        private const string URL_SCREEN = "/Reporting/RPTInsuranceReport";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "No";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public RPTInsuranceReportController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "Index"
        // GET: /KPIReport/
        public ActionResult Index()
        {
            ActionName = "Index";
            DataList();
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRInsuranceObject BSM = new HRInsuranceObject();
            BSM.Filter = new FTFilterEmployee();
            BSM.Filter.FromDate = DateTime.Now.Date;
            BSM.Filter.ToDate = DateTime.Now.Date;
            BSM.GridReportInsurance = BSM.GridListReportInsurance();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(HRInsuranceObject BSM)
        {
            ActionName = "Index";
            DataList();
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRInsuranceObject obj = new HRInsuranceObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                obj = (HRInsuranceObject)Session[Index_Sess_Obj + ActionName];
                obj.Filter = BSM.Filter;
            }
            BSM.GridReportInsurance = obj.GridListReportInsurance();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PaitailList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRInsuranceObject bsm = new HRInsuranceObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                bsm = (HRInsuranceObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = bsm;
            return PartialView(SYListConfuration.ListDefaultView, bsm.GridReportInsurance);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRInsuranceObject bsm = new HRInsuranceObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                bsm = (HRInsuranceObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = bsm;
            return PartialView("GridItems", bsm.GridReportInsurance);
        }
        public ActionResult DocumentViewerPartial()
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            //UserMVC();
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var RPT = (FTFilterEmployee)Session[Index_Sess_Obj + ActionName];
                    RptBouns sa = new RptBouns();
                    var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREEN_ID
                   && w.IsActive == true).ToList();
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
                        }
                        else
                        {
                            sa.Parameters[read.Name].Value = Dict[read.Name].ToString();
                        }

                        read.Visible = false;
                    }

                    Session[Index_Sess_Obj] = sa;

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
                RptBouns reportModel = (RptBouns)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }

        public ActionResult ExportTo()
        {
            UserSession();
            object obj = null;
            HRInsuranceObject BSM = new HRInsuranceObject();
            SYSGridSettingExport gSetting = new SYSGridSettingExport();
            gSetting.GridName = "ListConf";
            gSetting.DateFormat = SYSettings.getSetting(SYSConstant.DATE_FORMAT).SettinValue;
            GridViewExportFormat format = GridViewExportFormat.Xlsx;
            var dix = ClsInformation.GetDictionaryInfo(obj);
            return SYExportFile.ExportFormatsInfo[format](SYExportFile.CreateExportGridViewSettings(gSetting, dix), BSM.GridReportInsurance);
        }

        #endregion
        private void DataList()
        {
            ViewData["DEPARTMENT_SELECT"] = DB.HRDepartments.ToList();
            ViewData["BRANCHES_SELECT"] = SMS.HRBranches.ToList();
            ViewData["SECTION_SELECT"] = DB.HRSections.ToList();
            ViewData["POSITION_SELECT"] = DB.HRPositions.ToList();
            ViewData["DIVISION_SELECT"] = DB.HRDivisions.ToList();
            ViewData["LEVEL_SELECT"] = SMS.HRLevels.ToList();
        }
    }
}
