using Humica.Core.FT;
using Humica.EF.Models.SY;
using DevExpress.Web.Mvc;
using System;
using System.Linq;
using System.Web.Mvc;
using Humica.EF;
using Humica.Models.SY;
using System.Reflection;
using Humica.EF.MD;
using Humica.Core.DB;
using Humica.Logic;
using HUMICA.Models.Report;
using Humica.EF.Repo;

namespace Humica.Controllers.Reporting.Performance
{
    public class RPTKPITimeSheetStatisticController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RPTAP00008";
        private const string URL_SCREEN = "/Reporting/RPTKPITimeSheetStatistic/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        IClsFilter BSM;
        IUnitOfWork unitOfWork;
        public RPTKPITimeSheetStatisticController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsFilter();
            BSM.OnLoad();
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }

        #region "Index"
        // GET: /KPIReport/
        public ActionResult Index()
        {
            ActionName = "Print";
            UserSession();
            UserConfListAndForm();
            FTFilerPerformance Filter = new FTFilerPerformance();
            Filter.InYear = DateTime.Now.Year;
            return View(Filter);
        }
        [HttpPost]
        public ActionResult Index(FTFilerPerformance Filter)
        {
            ActionName = "Print";
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
                    RptKPITimeSheetStatistic sa = new RptKPITimeSheetStatistic();
                    var objRpt = unitOfWork.Set<CFReportObject>().Where(w => w.ScreenID == SCREEN_ID
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
                RptKPITimeSheetStatistic reportModel = (RptKPITimeSheetStatistic)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }

        #endregion
       
    }
}