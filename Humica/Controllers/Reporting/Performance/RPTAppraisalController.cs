using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Models.Report.KPI;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Humica.Controllers.Reporting.Performance
{
    public class RPTAppraisalController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RPTAP00004";
        private const string URL_SCREEN = "/Reporting/RPTAppraisal/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private const string REPORT_PARTIAL = "PrintForm";
        HumicaDBContext DB = new HumicaDBContext();

        public RPTAppraisalController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "Index"
        // GET: /KPIReport/
        public ActionResult Index()
        {
            ActionName = "Print";
            DataList();
            UserSession();
            UserConfListAndForm();
            FTFilerPerformance Filter = new FTFilerPerformance();
            Filter.FromDate = DateTime.Now;
            Filter.ToDate = DateTime.Now;
            return View(Filter);
        }
        [HttpPost]
        public ActionResult Index(FTFilerPerformance Filter)
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
            //UserMVC();
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var RPT = (FTFilerPerformance)Session[Index_Sess_Obj + ActionName];
                    RPTAppraisal sa = new RPTAppraisal();
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
                            if (read.Name == "Branch")
                            {
                                sa.Parameters[read.Name].Value = SYConstant.Branch_Condition;
                            }
                        }
                        else
                        {
                            sa.Parameters[read.Name].Value = Dict[read.Name].ToString();
                        }
                        read.Visible = false;
                    }
                    Session[Index_Sess_Obj] = sa;
                    //  Session[Index_Sess_Obj + ActionName] = RPT;

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
                RPTAppraisal reportModel = (RPTAppraisal)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }

        #endregion

        private void DataList()
        {
			ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
			ViewData["COMPANY_SELECT"] = SYConstant.getCompanyDataAccess();
			ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
			ViewData["BUSINESSUNIT_SELECT"] = ClsFilter.LoadBusinessUnit();
			ViewData["OFFICE_SELECT"] = DB.HROffices.ToList();
			ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
			ViewData["GROUP_SELECT"] = DB.HRGroups.ToList();
			ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
			ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
			ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
			ViewData["LOCATION_SELECT"] = DB.HRLocations.ToList();
		}
    }
}