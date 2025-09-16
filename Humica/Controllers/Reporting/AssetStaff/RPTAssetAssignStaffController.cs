using Humica.Core.FT;
using Humica.EF.Models.SY;
using DevExpress.Web.Mvc;
using System;
using System.Linq;
using System.Web.Mvc;
using Humica.EF;
using Humica.Models.Report;
using System.Reflection;
using Humica.Models.SY;
using Humica.EF.MD;
using Humica.Core.DB;
using HUMICA.Models.Report.Payroll;
using DevExpress.XtraReports.UI;
using HUMICA.Models.Report.Asset;

namespace Humica.Controllers.Reporting.AssetStaff
{
    public class RPTAssetAssignStaffController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RPTAS00001";
        private const string URL_SCREEN = "/Reporting/RPTAssetAssignStaff/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity DBA = new SMSystemEntity();
        public RPTAssetAssignStaffController()
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
            Filter.FromDate = DateTime.Now;
            Filter.ToDate = DateTime.Now;
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
                    if (RPT.ReportTypes == "AssetStaff")
                    {
                        sa = new RPTAssetStaffReporting();
                    }
                    else if (RPT.ReportTypes == "AssetDepartment")
                    {
                        sa = new RPTAssetDepartmentReporting();
                    }
                    else if (RPT.ReportTypes == "AssetNoneStaff")
                    {
                        sa = new RPTAssetNoneStaffReporting();
                    }
                    else
                    {
						sa = new RPTAssetTrainsition();
					}
                    //else if (RPT.ReportTypes == "AssetTransfer")
                    //               {
                    //                   sa = new RPTAssetTransfer();
                    //               }
                    //else if (RPT.ReportTypes == "CompareSummary")
                    //{
                    //    sa = new RptMonthlyCompare();
                    //}
                    var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREEN_ID
                      && w.DocType == RPT.ReportTypes && w.IsActive == true).ToList();
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
                            //if (read.Name == "Branch")
                            //{
                            //    sa.Parameters[read.Name].Value = SYConstant.Branch_Condition;
                            //}
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
            var listReportByScreen = DBA.CFReports.Where(w => w.ScreenId == SCREEN_ID).OrderBy(w => w.InOrder).ToList();
            ViewData["REPORT_TYPE_SELECT"] = listReportByScreen;
            ViewData["DEPARTMENT_SELECT"] = DB.HRDepartments.ToList();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["SECTION_SELECT"] = DB.HRSections.ToList();
            ViewData["POSITION_SELECT"] = DB.HRPositions.ToList();
            ViewData["DIVISION_SELECT"] = DB.HRDivisions.ToList();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();

			SYDataList objList = new SYDataList("AssetStaff_SELECT");
			ViewData["STATUSASSET_SELECT"] = objList.ListData;
		}
    }
}