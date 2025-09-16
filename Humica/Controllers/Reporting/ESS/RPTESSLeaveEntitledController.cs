using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using Humica.Models.Report;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Humica.Controllers.Reporting
{
    public class RPTESSLeaveEntitledController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RPTESS0003";
        private const string URL_SCREEN = "/Reporting/RPTESSLeaveEntitled/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        IClsFilter BSM;
        IUnitOfWork unitOfWork;
        public RPTESSLeaveEntitledController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsFilter();
            BSM.OnLoad();
            unitOfWork = new UnitOfWork<HumicaDBContext>(new HumicaDBContext());
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            FTFilterReport Filter = new FTFilterReport();
            UserConfListAndForm();
            DateTime DateNow = DateTime.Now;
            Filter.InMonth = DateNow;
            return View(Filter);
        }
        [HttpPost]
        public ActionResult Index(FTFilterReport Filter)
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
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var RPT = (FTFilterReport)Session[Index_Sess_Obj + ActionName];
                    RPTLeaveEntitled sa = new RPTLeaveEntitled();

                    var objRpt = unitOfWork.Set<CFReportObject>().Where(w => w.ScreenID == SCREEN_ID
                   && w.IsActive == true).ToList();
                    if (objRpt.Count > 0)
                    {
                        sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                    }

                    var Dict = RPT.GetType()
               .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(prop => prop.Name, prop => prop.GetValue(RPT, null));
                    string EmpCode = "";
                    if (string.IsNullOrEmpty(RPT.EmpCode))
                    {
                        var HOD_ = BSM.OnLoadStaffByHOD();
                        if (HOD_ != null)
                        {
                            foreach (var item in HOD_)
                            {
                                EmpCode += item.EmpCode + ",";
                            }
                            if (EmpCode != "")
                                EmpCode = EmpCode.Substring(0, EmpCode.Length - 1);
                        }
                    }
                    else
                    {
                        EmpCode = RPT.EmpCode;
                    }
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
                            if (read.Name == "EmpCode")
                            {
                                sa.Parameters[read.Name].Value = EmpCode;
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
                RPTLeaveEntitled reportModel = (RPTLeaveEntitled)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
    }
}