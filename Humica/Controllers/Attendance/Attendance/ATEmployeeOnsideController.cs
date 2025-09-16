using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.Controllers;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.Att;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.Attendance
{
    public class ATEmployeeOnsideController : MasterSaleController

    {
        private const string SCREEN_ID = "ATM0000014";
        private const string URL_SCREEN = "/Attendance/Attendance/ATEmployeeOnside/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public ATEmployeeOnsideController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        // GET: Branch
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ATEmpSchObject BSM = new ATEmpSchObject();
            BSM.Filter = new FTFilterEmployee();
            BSM.Filter.FromDate = DateTime.Now;
            BSM.Filter.ToDate = DateTime.Now;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ATEmpSchObject BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            var _ListActivity = DBV.VIEW_EmpOnsite.Where(w => DbFunctions.TruncateTime(w.ScanDate) >= BSM.Filter.FromDate.Date && DbFunctions.TruncateTime(w.ScanDate) <= BSM.Filter.ToDate.Date).ToList();
            List<VIEW_EmpOnsite> ListActivity = _ListActivity.ToList();
            List<HRCompany> _ListCompany = SYConstant.getCompanyDataAccess();
            List<HRBranch> _ListBranch = SYConstant.getBranchDataAccess();
            List<HRLevel> _ListLevel = SYConstant.getLevelDataAccess();
            var branchid = _ListBranch.Select(s => s.Code).ToList();
            var companyCodes = _ListCompany.Select(x => x.Company).ToList();
            var levelCodes = _ListLevel.Select(x => x.Code).ToList();
            ListActivity = ListActivity.Where(w => companyCodes.Contains(w.CompanyCode))
                           .Where(w => levelCodes.Contains(w.LevelCode))
                           .Where(w => branchid.Contains(w.Branch)).ToList();
            if (!string.IsNullOrEmpty(BSM.Filter.Branch))
            {
                var branchFilter = BSM.Filter.Branch.Split(',').Select(b => b.Trim()).Where(b => !string.IsNullOrEmpty(b)).ToList();
                ListActivity = ListActivity.Where(w => branchFilter.Contains(w.Branch)).ToList();
            }
            if (!string.IsNullOrEmpty(BSM.Filter.Department))
                ListActivity = ListActivity.Where(w => w.DEPT == BSM.Filter.Department).ToList();
            if (!string.IsNullOrEmpty(BSM.Filter.Position))
                ListActivity = ListActivity.Where(w => w.JobCode == BSM.Filter.Position).ToList();
            if (!string.IsNullOrEmpty(BSM.Filter.Division))
                ListActivity = ListActivity.Where(w => w.Division == BSM.Filter.Division).ToList();
            if (!string.IsNullOrEmpty(BSM.Filter.EmpCode))
                ListActivity = ListActivity.Where(w => w.EmpCode == BSM.Filter.EmpCode).ToList();
            if (!string.IsNullOrEmpty(BSM.Filter.Locations))
                ListActivity = ListActivity.Where(w => w.LOCT == BSM.Filter.Locations).ToList();
            if (!string.IsNullOrEmpty(BSM.Filter.Office))
                ListActivity = ListActivity.Where(w => w.Office == BSM.Filter.Office).ToList();
            if (!string.IsNullOrEmpty(BSM.Filter.Team))
                ListActivity = ListActivity.Where(w => w.Groups == BSM.Filter.Team).ToList();
            if (!string.IsNullOrEmpty(BSM.Filter.Section))
                ListActivity = ListActivity.Where(w => w.SECT == BSM.Filter.Section).ToList();
            if (!string.IsNullOrEmpty(BSM.Filter.Level))
                ListActivity = ListActivity.Where(w => w.LevelCode == BSM.Filter.Level).ToList();
            BSM.ListEmpOnSite = ListActivity
                             .OrderByDescending(w => w.ScanDate)
                             .ThenBy(w => w.EmpCode)
                             .ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult Gridviews()
        {
            ActionName = "Index";
            UserConf(ActionBehavior.EDIT);
            ATEmpSchObject BSM = new ATEmpSchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            //BSM.ListEmpOnSite = DBV.VIEW_EmpOnsite.ToList();
            return PartialView("Gridviews", BSM);
        }

        private void DataSelector()
        {
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.Where(w => w.Status == "A").Select(x => new { x.EmpCode, x.AllName, x.OthAllName }).ToList();
            ViewData["BUSINESSUNIT_SELECT"] = ClsFilter.LoadBusinessUnit();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["OFFICE_SELECT"] = ClsFilter.LoadOffice();
            ViewData["GROUP_SELECT"] = ClsFilter.LoadGroups();
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["LOCATION_SELECT"] = ClsFilter.LoadLocation().OrderBy(w => w.Description);
        }
    }
}