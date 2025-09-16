using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Models.Report.Payroll;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Humica.Controllers.Reporting
{
    public class RPTPayHisReportController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "RPTPR00013";
        private const string URL_SCREEN = "/Reporting/RPTPayHisReport/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        HumicaDBContext DB = new HumicaDBContext();
        public RPTPayHisReportController()
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
            Filter.INYear = DateTime.Now.Year;
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
            DataList();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            //UserMVC();
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var RPT = (FTFilterEmployee)Session[Index_Sess_Obj + ActionName];
                    RptPayHistory sa = new RptPayHistory();
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
                RptPayHistory reportModel = (RptPayHistory)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }

        #endregion

        private void DataList()
        {
            ViewData["DEPARTMENT_SELECT"] = DB.HRDepartments.ToList();
            ViewData["COMPANY_SELECT"] = SYConstant.getCompanyDataAccess();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
			ViewData["BUSINESSUNIT_SELECT"] = ClsFilter.LoadBusinessUnit();
			ViewData["OFFICE_SELECT"] = ClsFilter.LoadOffice();
			ViewData["GROUP_SELECT"] = ClsFilter.LoadGroups();
			ViewData["SECTION_SELECT"] = DB.HRSections.ToList();
            ViewData["POSITION_SELECT"] = DB.HRPositions.ToList();
            ViewData["DIVISION_SELECT"] = DB.HRDivisions.ToList();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
        }
    }
}
