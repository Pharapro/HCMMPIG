using Humica.Core.FT;
using Humica.EF.Models.SY;
using DevExpress.Web.Mvc;
using System;
using System.Linq;
using System.Web.Mvc;
using Humica.EF;
using Humica.Models.Report.Payroll;
using Humica.Models.SY;
using System.Reflection;
using Humica.EF.MD;
using Humica.Core.DB;
using System.Collections.Generic;
using Humica.EF.Repo;
using DevExpress.XtraReports.UI;
using Humica.Core;

namespace Humica.Controllers.SelfService.MyTeam
{
    public class RPTPaySlipController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000023";
        private const string URL_SCREEN = "/SelfService/MyTeam/RPTPaySlip/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        IUnitOfWork unitOfWork;
        public RPTPaySlipController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }

        #region "Index"
        //// GET: /KPIReport/
        ////public ActionResult Index()
        ////{
        ////    ActionName = "Print";
        ////    DataList();
        ////    UserSession();
        ////    UserConfListAndForm();
        ////    FTFilerPayroll Filter = new FTFilerPayroll();
        ////    Filter.EmpCode = user.UserName;
        ////    Filter.InMonth = DateTime.Now;
        ////    Filter.ToMonth = DateTime.Now;
        ////    return View(Filter);
        ////}
        //[HttpPost]
        //public ActionResult Index(FTFilerPayroll Filter)
        //{
        //    ActionName = "Print";
        //    DataList();
        //    UserSession();
        //    UserConfListAndForm();
        //    Session[Index_Sess_Obj + ActionName] = Filter;
        //    return View("ReportView", Filter);
        //}
        //public ActionResult DocumentViewerPartial()
        //{
        //    UserSession();
        //    UserConf(ActionBehavior.VIEWONLY);
        //    ActionName = "Print";
        //    try
        //    {
        //        if (Session[Index_Sess_Obj + ActionName] != null)
        //        {
        //            var RPT = (FTFilerPayroll)Session[Index_Sess_Obj + ActionName];
        //            RptPaySlipByEmp sa = new RptPaySlipByEmp();
        //            var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREEN_ID
        //          && w.IsActive == true).ToList();
        //            if (objRpt.Count > 0)
        //            {
        //                sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
        //            }
        //            var Dict = RPT.GetType()
        //       .GetProperties(BindingFlags.Instance | BindingFlags.Public)
        //            .ToDictionary(prop => prop.Name, prop => prop.GetValue(RPT, null));

        //            foreach (var read in sa.Parameters)
        //            {
        //                if (Dict[read.Name] == null)
        //                {
        //                    sa.Parameters[read.Name].Value = "";
        //                }
        //                else
        //                {
        //                    sa.Parameters[read.Name].Value = Dict[read.Name].ToString();
        //                }

        //                read.Visible = false;
        //            }

        //            Session[Index_Sess_Obj] = sa;

        //            return PartialView("PrintForm", sa);
        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserID.ToString();
        //        log.DocurmentAction = "Print";
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //    }
        //    return null;
        //}
        //public ActionResult DocumentViewerExportTo()
        //{
        //    ActionName = "Print";

        //    if (Session[Index_Sess_Obj] != null)
        //    {
        //        RptPaySlipByEmp reportModel = (RptPaySlipByEmp)Session[Index_Sess_Obj];
        //        return ReportViewerExtension.ExportTo(reportModel);
        //    }
        //    return null;
        //}

        #endregion
        public ActionResult Index()
        {
            ActionName = "Print";
            UserSession();
            UserConfListAndForm();
            DataList();
            FTFilerPayroll Filter = new FTFilerPayroll();
            var Period = unitOfWork.Set<PRPayPeriodItem>().Where(w => w.IsActive == true).OrderByDescending(w => w.StartDate).ToList();
            if (Period.Count > 0)
            {
                Filter.Period = Period.FirstOrDefault().PeriodID;
            }
            Session[Index_Sess_Obj + ActionName] = Filter;
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
                        }
                        else
                        {
                            sa.Parameters[read.Name].Value = Dict[read.Name].ToString();
                        }

                        read.Visible = false;
                    }
                    var Liststaff = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == user.UserName);

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

        //#endregion
        private void DataList()
        {
            //var ListStaff = DBV.HR_STAFF_VIEW.ToList();
            //var Liststaff = unitOfWork.Set<HRStaffProfile>().AsQueryable().ToList();
            //ListStaff = ListStaff.Where(w => w.EmpCode == user.UserName || w.HODCode == user.UserName).ToList();
            var Liststaff = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == user.UserName);
            var ListStaff = new List<HRStaffProfile>();
            if (Liststaff != null)
            {
                var list_staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().Where(w => w.HODCode == Liststaff.EmpCode).ToList();
                ListStaff = list_staff.ToList();
            }
            ViewData["STAFF_SELECT"] = ListStaff;
        }
        public ActionResult EmpCode()
        {
            UserSession();
            UserConfListAndForm();
            ViewData["PERIOD_SELECT"] = unitOfWork.Set<PRPayPeriodItem>().AsQueryable().OrderByDescending(w => w.StartDate).ToList();
            var Liststaff = unitOfWork.Set<HRStaffProfile>().AsQueryable().FirstOrDefault(w => w.EmpCode == user.UserName);
            var ListStaff = new List<HRStaffProfile>();
            if (Liststaff != null)
            {
                var list_staff = unitOfWork.Set<HRStaffProfile>().AsQueryable().Where(w => w.HODCode == Liststaff.EmpCode).ToList();
                ListStaff = list_staff.ToList();
            }
            ViewData["STAFF_SELECT"] = ListStaff;
            return PartialView("EmpCode_");
        }
    }
}