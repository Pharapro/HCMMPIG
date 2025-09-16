using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.HR;
using Humica.Models.Report.HRM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Reporting
{
    public class RPTNameCardController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RPTHR00013";
        private const string URL_SCREEN = "/Reporting/RPTNameCard/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string Index_Sess_Report = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "EmpCode";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public RPTNameCardController()
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
            UserConfListAndForm(this.KeyName);
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            BSM.ListStaffView = new List<HR_STAFF_VIEW>();
            BSM.ListStaffView = DBV.HR_STAFF_VIEW.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            BSM.ScreenId = SCREEN_ID;
            //BSM.ListStaffView = new List<HR_STAFF_VIEW>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            // Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListStaffView);
        }
        public ActionResult Print(string id)
        {
            ActionName = "Index";
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            id = "";
            UserMVCReportView();
            var BSM = new HRStaffProfileObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
            }
            return View("ReportView");
        }

        public ActionResult DocumentViewerPartial(string id)
        {
            UserSession();
            DataList();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Index";
            try
            {
                id = "";
                var BSM = new HRStaffProfileObject();
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
                }
                ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;

                RPTFrontCard reportModel = new RPTFrontCard();
                var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREEN_ID
                  && w.DocType == "PrintFront"
                   //  && w.Company == ClsConstant.DEFAULT_PLANT
                   && w.IsActive == true).ToList();
                if (objRpt.Count > 0)
                {
                    reportModel.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                }
                //  RPTFrontCard reportModel = new RPTFrontCard();
                reportModel.Parameters["EmpCode"].Value = BSM.EmpCode;
                reportModel.Parameters["EmpCode"].Visible = false;

                Session[Index_Sess_Report] = reportModel;

                return PartialView("PrintForm", reportModel);
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
        public ActionResult DocumentViewerExportTo(string id)
        {
            ActionName = "Index";
            id = "";
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            // RPTFrontCard reportModel = new RPTFrontCard();
            RPTFrontCard reportModel = new RPTFrontCard();
            reportModel = (RPTFrontCard)Session[Index_Sess_Obj];

            return ReportViewerExtension.ExportTo(reportModel);
        }

        public ActionResult PrintBack(string id)
        {
            ActionName = "Index";
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            id = "";
            var BSM = new HRStaffProfileObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
            }
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            UserMVCReportView();
            return View("ReportBackView");
        }
        public ActionResult DocumentViewerPartialPrintBack(string id)
        {
            UserSession();
            DataList();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Index";
            try
            {
                id = "";
                var BSM = new HRStaffProfileObject();
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
                }
                ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                RPTBackCard reportModel = new RPTBackCard();
                var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREEN_ID
                   && w.DocType == "PrintBack"
                  //  && w.Company == ClsConstant.DEFAULT_PLANT
                  && w.IsActive == true).ToList();
                if (objRpt.Count > 0)
                {
                    reportModel.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                }
                reportModel.Parameters["EmpCode"].Value = BSM.EmpCode;
                reportModel.Parameters["EmpCode"].Visible = false;
                Session[Index_Sess_Report] = reportModel;

                return PartialView("PrintFormBack", reportModel);
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
        public ActionResult DocumentViewerExportToPrintBack(string id)
        {
            ActionName = "Index";
            id = "";
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            RPTBackCard reportModel = new RPTBackCard();
            reportModel = (RPTBackCard)Session[Index_Sess_Obj];
            return ReportViewerExtension.ExportTo(reportModel);
        }
        [HttpPost]
        public string getEmpCode(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            HRStaffProfileObject BSM = new HRStaffProfileObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];

                BSM.EmpCode = EmpCode;

                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_ALLOWANCE");
            }
        }
        private void DataList()
        {
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
        }
    }
}
