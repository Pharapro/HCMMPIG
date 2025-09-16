using DevExpress.XtraReports.UI;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Humica.Models.SY
{
    public class clsReportHelper
    {
        IUnitOfWork unitOfWork;
        public clsReportHelper() {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public string Get_Path(string ScreenID, string DocType = null, string CompanyCode = null)
        {
            try
            {
                // Validate base path
                string basePath = ClsConstant.DEFAULT_REPORT_PATH;
                if (string.IsNullOrWhiteSpace(basePath) || !Directory.Exists(basePath))
                    return string.Empty;

                // Build query
                var query = unitOfWork.Repository<CFReportObject>().Queryable()
                    .Where(w => w.ScreenID == ScreenID && w.IsActive);

                if (!string.IsNullOrEmpty(DocType))
                    query = query.Where(w => w.DocType == DocType);

                // Add company filter if provided
                if (!string.IsNullOrEmpty(CompanyCode))
                    query = query.Where(w => w.Company == CompanyCode);

                // Execute query and get first valid path
                var report = query.OrderBy(r => r.Company).FirstOrDefault();  // Prioritize specific companies

                if (report == null || string.IsNullOrWhiteSpace(report.PathStore))
                    return string.Empty;

                // Construct the full path
                string normalizedPathStore = report.PathStore.Trim();
                if (normalizedPathStore.StartsWith("~"))
                {
                    normalizedPathStore = normalizedPathStore.Replace("~/Content/RPT/", basePath);
                    normalizedPathStore = normalizedPathStore.Replace('/', '\\');
                }

                // Construct the full path
                string fullPath = Path.Combine(normalizedPathStore);

                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        //public string Get_Path(string ScreenID, string DocType = null, string CompanyCode = null)
        //{
        //    string path = "";
        //    var objRpt = unitOfWork.Repository<CFReportObject>().Queryable().Where(w => w.ScreenID == ScreenID
        //           && w.IsActive == true);
        //    if (!string.IsNullOrEmpty(DocType))
        //        objRpt = objRpt.Where(w => w.DocType == DocType);
        //    if (objRpt.Any())
        //    {
        //        if (!string.IsNullOrEmpty(CompanyCode))
        //        {
        //            objRpt = objRpt.Where(w => w.Company == CompanyCode);
        //        }
        //        if (objRpt.Any())
        //        {
        //            if (Directory.Exists(ClsConstant.DEFAULT_REPORT_PATH+ objRpt.FirstOrDefault().PathStore))//validate path
        //                path = ClsConstant.DEFAULT_REPORT_PATH + objRpt.FirstOrDefault().PathStore;
        //        }
        //    }
        //    return path;
        //}

        //public int Index_Sess_Obj { get; set; }

        //public ActionResult LoadReportLayout(Controller controller, string screenId, FTFilterEmployee RPT, XtraReport sa, string reportPath,string user)
        //{
        //    using (var db = new HumicaDBContext())
        //    {
        //        var objRpt = db.CFReportObjects
        //            .Where(w => w.ScreenID == screenId && w.IsActive)
        //            .ToList();

        //        if (objRpt.Count > 0)
        //        {
        //            var fullPath = reportPath + objRpt.First().ReportObject;

        //            if (string.IsNullOrEmpty(fullPath))
        //            {
        //                controller.ViewBag.Message = "Report path is not found.";
        //                return controller.PartialView("ErrorView"); // Return an error view
        //            }

        //            sa.LoadLayoutFromXml(fullPath);

        //            var dict = RPT.GetType()
        //                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
        //                .ToDictionary(prop => prop.Name, prop => prop.GetValue(RPT, null));

        //            foreach (var read in sa.Parameters)
        //            {
        //                if (dict.TryGetValue(read.Name, out var value))
        //                {
        //                    read.Value = value?.ToString() ?? "";
        //                }
        //                else
        //                {
        //                    read.Value = "";
        //                }

        //                // Set specific values for Branch and Company
        //                if (read.Value == null)
        //                {
        //                    if (read.Name == "Branch")
        //                    {
        //                        read.Value = SYConstant.Branch_Condition;
        //                    }
        //                    if (read.Name == "Company")
        //                    {
        //                        sa.Parameters[read.Name].Value = SYConstant.Company_Condition;
        //                    }
        //                    else if (read.Name == "UserName")
        //                    {
        //                        sa.Parameters[read.Name].Value = user;
        //                    }
        //                }

        //                read.Visible = false; // Hide the parameter
        //            }

        //            controller.Session[Index_Sess_Obj] = sa; // Store the report in session
        //            return controller.PartialView("PrintForm", sa); // Return the report view
        //        }
        //        else
        //        {
        //            controller.ViewBag.Message = "No active report objects found.";
        //            return controller.PartialView("ErrorView"); // Return an error view
        //        }
        //    }
        //}
    }
}