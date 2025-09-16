using DevExpress.Web.Mvc;
using DevExpress.XtraReports.UI;
using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using Humica.Models.Report.Payroll;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Humica.Controllers.Reporting
{
    public class PayslipReportController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "RPTPR00001";
        private const string URL_SCREEN = "/Reporting/PayslipReport/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        IUnitOfWork unitOfWork;
        public PayslipReportController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }

        #region "Index"
        // GET: /KPIReport/
        public ActionResult Index()
        {
            ActionName = "Print";
            UserSession();
            UserConfListAndForm();
            DataList();
            FTFilerPayroll Filter = new FTFilerPayroll();
            var Period = unitOfWork.Set<PRPayPeriodItem>().AsQueryable().Where(w => w.IsActive == true).OrderByDescending(w => w.StartDate).ToList();
            if (Period.Count > 0)
            {
                Filter.Period = Period.FirstOrDefault().PeriodID;
            }
            return View(Filter);
        }
        [HttpPost]
        public ActionResult Index(FTFilerPayroll Filter)
        {
            ActionName = "Print";
            UserSession();
            UserConfListAndForm();
            DataList();
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
                    var RPT = (FTFilerPayroll)Session[Index_Sess_Obj + ActionName];
                    RPTPaySlip1 sa = new RPTPaySlip1();
                   var reportHelper = new clsReportHelper();
                    string path = reportHelper.Get_Path(SCREEN_ID, RPT.ReportTypes);
                    if (!string.IsNullOrEmpty(path))
                    {
                        sa.LoadLayoutFromXml(path);
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
                            else if (read.Name == "UserName")
                            {
                                sa.Parameters[read.Name].Value = user.UserName;
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
                XtraReport reportModel = (XtraReport)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }

        #endregion

        private void DataList()
        {
            ViewData["STAFF_SELECT"] = unitOfWork.Set<HRStaffProfile>().AsQueryable().ToList();
            ClsFilterJob clsFilter = new ClsFilterJob();
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["COMPANY_SELECT"] = SYConstant.getCompanyDataAccess();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
			ViewData["BUSINESSUNIT_SELECT"] = ClsFilter.LoadBusinessUnit();
			ViewData["OFFICE_SELECT"] = ClsFilter.LoadOffice();
			ViewData["GROUP_SELECT"] = ClsFilter.LoadGroups();
			ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData["PERIOD_SELECT"] = unitOfWork.Set<PRPayPeriodItem>().AsQueryable().OrderByDescending(w => w.StartDate).ToList();
        }
    }
}
