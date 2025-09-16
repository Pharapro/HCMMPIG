using DevExpress.Web.Mvc;
using DevExpress.XtraReports.UI;
using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Models.Report.Payroll;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.LeaveBalance
{
    public class ESSPayslipController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000009";
        private const string URL_SCREEN = "/SelfService/LeaveBalance/ESSPayslip/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        IUnitOfWork unitOfWork;
        public ESSPayslipController()
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
            Filter.EmpCode = user.UserName;
            var Period = unitOfWork.Set<PRPayPeriodItem>().Where(w => w.IsActive == true).OrderByDescending(w => w.StartDate).ToList();
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
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var RPT = (FTFilerPayroll)Session[Index_Sess_Obj + ActionName];
                    RptPaySlipByEmp sa = new RptPaySlipByEmp();
                   var reportHelper = new clsReportHelper();
                    var screenID = "RPTPR00001";
                    string path = reportHelper.Get_Path(screenID);
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

        #endregion

        private void DataList()
        {
            ViewData["PERIOD_SELECT"] = unitOfWork.Set<PRPayPeriodItem>().AsQueryable().OrderByDescending(w => w.StartDate).ToList();
        }
    }
}
