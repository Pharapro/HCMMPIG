using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Models.Report;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Humica.Controllers.Reporting
{
    public class RPTEmpCareerController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "RPTHR00006";
        private const string URL_SCREEN = "/Reporting/RPTEmpCareer/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        HumicaDBContext DB = new HumicaDBContext();

        public RPTEmpCareerController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        public ActionResult Index()
        {
            ActionName = "Index";
            DataList();
            UserSession();
            UserConfListAndForm();
            FTFilterReport Filter = new FTFilterReport();
            DateTime DateNow = DateTime.Now;
            Filter.FromDate = new DateTime(DateNow.Year, DateNow.Month, 1);
            Filter.ToDate = new DateTime(DateNow.Year, DateNow.Month, DateTime.DaysInMonth(DateNow.Year, DateNow.Month));

            Filter.Status = SYDocumentStatus.A.ToString();
            return View(Filter);
        }
        [HttpPost]
        public ActionResult Index(FTFilterReport Filter)
        {
            ActionName = "Print";
            UserSession();
            DataList();
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
                    var RPT = (FTFilterReport)Session[Index_Sess_Obj + ActionName];
                    RptEmpCareer sa = new RptEmpCareer();

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
                RptEmpCareer reportModel = (RptEmpCareer)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }

        private void DataList()
        {
            SYDataList objList = new SYDataList("STATUS_EMPLOYEE");
            ViewData["STATUS_EMPLOYEE"] = objList.ListData;

            ViewData["COMPANY_SELECT"] = SYConstant.getCompanyDataAccess();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["BUSINESSUNIT_SELECT"] = ClsFilter.LoadBusinessUnit();
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["OFFICE_SELECT"] = ClsFilter.LoadOffice();
            ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
            ViewData["GROUP_SELECT"] = ClsFilter.LoadGroups();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
            ViewData["CAREERCODE_SELECT"] = DB.HRCareerHistories.ToList();
        }
    }
}