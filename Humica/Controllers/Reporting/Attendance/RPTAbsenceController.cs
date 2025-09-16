using DevExpress.Web.Mvc;
using DevExpress.XtraReports.UI;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using Humica.Models.Report.Attendance;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Humica.Controllers.Reporting
{
    public class RPTAbsenceController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "RPTAT00008";
        private const string URL_SCREEN = "/Reporting/RPTAbsence/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        IUnitOfWork unitOfWork;
        HumicaDBContext DB = new HumicaDBContext();
        public RPTAbsenceController()
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
            FTFilterEmployee Filter = new FTFilterEmployee();
            DateTime DateNow = DateTime.Now;
            Filter.FromDate = DateNow;
            Filter.ToDate = DateNow;
            Filter.IsLate = false;
            Filter.IsEalary = false;
            Filter.IsMissScan = false;
            return View(Filter);
        }
        [HttpPost]
        public ActionResult Index(FTFilterEmployee Filter)
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
                    var RPT = (FTFilterEmployee)Session[Index_Sess_Obj + ActionName];
                    XtraReport sa = new XtraReport();
                    if (RPT.ReportTypes == "ABS")
                    {
                        sa = new RPTAbsence();
                    }
                    else if (RPT.ReportTypes == "ABSSum")
                    {
                        sa = new RPTAbsenceSummary();
                    }

                    var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREEN_ID && w.DocType== RPT.ReportTypes
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

        private void DataList()
        {
            var listReportByScreen = DB.CFReports.Where(w => w.ScreenId == SCREEN_ID).ToList();
            ViewData["REPORT_TYPE_SELECT"] = listReportByScreen;
            ViewData["DEPARTMENT_SELECT"] = DB.HRDepartments.ToList();
            ViewData["OFFICE_SELECT"] = ClsFilter.LoadOffice();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["COMPANY_SELECT"] = SYConstant.getCompanyDataAccess();
            ViewData["SECTION_SELECT"] = DB.HRSections.ToList();
            ViewData["GROUP_SELECT"] = ClsFilter.LoadGroups();
            ViewData["POSITION_SELECT"] = DB.HRPositions.ToList();
            ViewData["DIVISION_SELECT"] = DB.HRDivisions.ToList();
            ViewData["BUSINESSUNIT_SELECT"] = ClsFilter.LoadBusinessUnit();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
            ViewData["LOCATION_SELECT"] = DB.HRLocations.ToList();
        }
    }
}