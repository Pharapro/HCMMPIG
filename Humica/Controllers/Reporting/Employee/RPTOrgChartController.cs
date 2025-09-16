using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Reporting
{
    public class RPTOrgChartController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RPTOR00001";
        private const string URL_SCREEN = "/Reporting/RPTOrgChart/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string KeyName = "EmpCode";
        private HumicaDBContext DB = new HumicaDBContext();

        public RPTOrgChartController() : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        public ActionResult Index(string branch, string department)
        {
            MDOrgChart orgChartModel = new MDOrgChart
            {
                Filter = new Humica.Core.FT.FTFilterEmployee
                {
                    Branch = branch,
                    Department = department
                }
            };
            DataSelector();
            return View(orgChartModel);
        }

        [HttpPost]
        public ActionResult GetData(string Branch, string Department)
        {
            System.Diagnostics.Debug.WriteLine($"Branch: {Branch}, Department: {Department}");

            MDOrgChart orgChart = new MDOrgChart();
            var filteredData = orgChart.LoadDatas(Branch, Department);

            var data = new
            {
                MS = "OK",
                DT = filteredData,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private void DataSelector()
        {
            ClsFilterJob clsFilter = new ClsFilterJob();
            var _listBranch = SYConstant.getBranchDataAccess();
            ViewData["BRANCHES_SELECT"] = _listBranch.ToList();
            ViewData["DEPARTMENT_SELECT"] = clsFilter.LoadDepartment();
        }
    }
}