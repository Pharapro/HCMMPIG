using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.Inquiry;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Inquiry
{
    public class InqAttendanceController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "INQ000002";
        private const string URL_SCREEN = "/Inquiry/InqAttendance/";
        private string ActionName = "";
        private readonly string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        HumicaDBViewContext DBR = new HumicaDBViewContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public InqAttendanceController() : base()
        {
            ScreendIDControl = SCREEN_ID;
            ScreenUrl = URL_SCREEN;
        }

        #region "List Reports"

        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(ActionBehavior.LIST_ADD, "ReportId", "InqAttendance");

            RPReportingObject RPT = new RPReportingObject();
            var listReportByScreen = SMS.CFReports.Where(w => w.ScreenId == SCREEN_ID).OrderBy(w => w.InOrder).ToList();
            RPT.ListHeader = new List<CFReport>();
            var ListRoleItem = (List<SYRoleItem>)Session[SYSConstant.LIST_AUTTH_ROLE];

            if (ListRoleItem != null)
            {
                var listRoleItemByScreen = ListRoleItem.Where(w => w.ScreenId == SCREEN_ID).ToList();

                foreach (var read in listReportByScreen)
                {
                    if (listRoleItemByScreen.Where(w => w.ActionName == read.ReportId).ToList().Count > 0)
                    {
                        RPT.ListHeader.Add(read);
                    }
                }
            }

            Session[Index_Sess_Obj + ActionName] = RPT;
            return View(RPT);
        }

        public ActionResult PartialList()
        {
            this.ActionName = "Index";
            RPReportingObject BSM = new RPReportingObject();
            UserSession();
            UserConfList(ActionBehavior.LIST_ADD, "ReportId", "InqAttendance");
            UserConfFormFitler();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (RPReportingObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ReportingList, BSM.ListHeader);
        }

        #endregion

        #region "ATINOUT"

        public ActionResult ATINOUT()
        {
            DataFilter();
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ClsInqAttendance)Session[Index_Sess_Obj + ActionName];
            }
            ClsInqAttendance BSM = new ClsInqAttendance();
            BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
            //BSM.Filter.CompanyCode = ClsConstant.DEFAULT_PLANT;
            BSM.Filter.STReportType = SYReportType.PIVOT.ToString();
            BSM.ScreenID = "INQA000001";
            ActionName = BSM.ScreenID;

            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            BSM.Filter.FromDate = firstDayOfMonth;
            BSM.Filter.ToDate = lastDayOfMonth;
            var dbr = DBR.VIEW_ATEmpSchedule.ToList();
            BSM.List_EmpSchedule = DBR.VIEW_ATEmpSchedule.Where(w =>
                w.TranDate <= BSM.Filter.ToDate &&
                w.TranDate >= BSM.Filter.FromDate).ToList();

            BSM.SetDataReportHRPayHistoryFilter();

            UserConfReportPartial(BSM.Filter.STReportType, ControllerContext.RouteData.Values["action"].ToString() + "Partail", BSM.ScreenID);

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult ATINOUT(ClsInqAttendance BSM)
        {
            DataFilter();
            UserSession();
            UserConfListAndForm();

            BSM.ScreenID = "INQA000001";
            ActionName = BSM.ScreenID;
            BSM.List_EmpSchedule = DBR.VIEW_ATEmpSchedule.Where(w =>
                w.TranDate <= BSM.Filter.ToDate &&
                w.TranDate >= BSM.Filter.FromDate).ToList();
            BSM.List_EmpSchedule = BSM.List_EmpSchedule.OrderBy(x => x.TranDate).ToList();
            BSM.SetDataReportHRPayHistoryFilter();

            UserConfReportPartial(BSM.Filter.STReportType, ControllerContext.RouteData.Values["action"].ToString() + "Partail", BSM.ScreenID);

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        public ActionResult ATINOUTPartail()
        {
            UserSession();
            UserConfListAndForm();

            ClsInqAttendance BSM = new ClsInqAttendance();
            BSM.ScreenID = "INQA000001";
            ActionName = BSM.ScreenID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsInqAttendance)Session[Index_Sess_Obj + ActionName];
            }

            UserConfReportPartial(BSM.Filter.STReportType, ControllerContext.RouteData.Values["action"].ToString(), BSM.ScreenID);

            if (BSM.Filter.STReportType == SYReportType.PIVOT.ToString())
            {
                return PartialView(SYListConfuration.ListPivotSelectView, BSM.List_EmpSchedule);
            }
            else
            {
                return PartialView(SYListConfuration.ListRowSelectView, BSM.List_EmpSchedule);
            }
        }
        public ActionResult PivotATINOUT()
        {
            ActionName = "INQA000001";
            UserSession();
            UserConfListAndForm();
            ClsInqAttendance BSM = new ClsInqAttendance();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsInqAttendance)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PivotATINOUT", BSM);
        }
        #endregion

        #region "Private Code"

        public void DataFilter()
        {
            //ViewData["BRANCH_SELECT"] = ClsConstant.getLocationDataAccess();
            //var objList = ClsConstant.getLocationDataAccess();
            //ViewData["STORAGE_LIST"] = objList;

            //ViewData["CUSTOMER_LIST"] = DB.MDCustomers.Where(w => w.CompanyCode == ClsConstant.DEFAULT_PLANT).ToList();
            //ViewData["WAREHOUSE_SELECT"] = ClsConstant.getCostCenterLocationDataAccess();
            //ViewData["ITEM_TYPE"] = DB.CFMaterialTypes.ToList();
            //ViewData["ITEM_GROUP"] = DB.CFMaterialGroups.ToList();
            //ViewData["MOVEMENT_TYPE"] = DB.CFMovementTypes.ToList();
            //ViewData["STAFF_LIST"] = ClsConstant.getStaffDataAccess();
            //ViewData["CURRENCY_SELECT"] = DB.MDCurrencies.Where(w => w.CompanyCode == ClsConstant.DEFAULT_PLANT).ToList();

            //var listStaff = DB.Staffs.Where(w => w.CompanyCode == ClsConstant.DEFAULT_PLANT).ToList();
            //listStaff.Add(new SaleModule.MD.Staff());
            //ViewData["STAFF_SELECT"] = listStaff;
        }

        #endregion
    }

}
