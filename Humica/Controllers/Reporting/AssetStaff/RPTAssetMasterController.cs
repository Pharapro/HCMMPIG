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
using HUMICA.Models.Report.Asset;
using System.Security.Cryptography;
using DevExpress.XtraReports.UI;
namespace Humica.Controllers.Reporting.AssetStaff
{
    public class RPTAssetMasterController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RPTAS00002";
        private const string URL_SCREEN = "/Reporting/RPTAssetMaster/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        HumicaDBContext DB = new HumicaDBContext();
		SMSystemEntity DBA = new SMSystemEntity();
		public RPTAssetMasterController()
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
			FTAssetMaster Filter = new FTAssetMaster();
			return View(Filter);
        }
        [HttpPost]
        public ActionResult Index(FTAssetMaster Filter)
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
					var RPT = (FTAssetMaster)Session[Index_Sess_Obj + ActionName];
					XtraReport sa = new XtraReport();
					sa = new RPTMasterAsset();
					//if (RPT.ReportTypes == "AssetInOut")
					//{
					//	sa = new RPTAssetInOut();
					//}
					//               else if(RPT.ReportTypes == "AssetMaster")
					//               {
					//                   sa = new RPTMasterAsset();
					//               }
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
                RPTMasterAsset reportModel = (RPTMasterAsset)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
		private void DataList()
        {
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
			SYDataList objList = new SYDataList("StatusAsset_SELECT");
			ViewData["STATUSASSET_SELECT"] = objList.ListData;
			ViewData["AssetMaster_SELECT"] = DB.HRAssetRegisters.ToList();
			ViewData["AssetType_SELECT"] = DB.HRAssetTypes.ToList();
		}
    }
}