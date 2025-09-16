using DevExpress.XtraPrinting;
using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Integration.EF.Models;
using Humica.Logic;
using Humica.Logic.PR;
using HUMICA.Models.Report.Payroll;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HR.Controllers.PR.PRM
{

    public class HODRequestOTController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000022";
        private const string URL_SCREEN = "/SelfService/MyTeam/HODRequestOT/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "ID";
        private string DOCTYPE = "OTR01";
        HumicaDBContext DB = new HumicaDBContext();
        CUSCENDB DP = new CUSCENDB();
        public HODRequestOTController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "List"
        public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            PROverTimeObject BSM = new PROverTimeObject();
            BSM.HeaderOT = new HRRequestOT();
            BSM._Filter = new FTFilterEmployee();
            BSM.Filter = new FTINYear();
            BSM.ListEmpStaff = new List<_ListStaff>();
            BSM._Filter.InDate = DateTime.Now;
            BSM._Filter.StartTime = DateTime.Now;
            BSM._Filter.EndTime = DateTime.Now;
            BSM._Filter.TotalHours = 0;
            var staff = await DB.HRStaffProfiles.FirstOrDefaultAsync(w => w.EmpCode == user.UserName);
            if (staff != null)
            {
                BSM.HeaderOT.RequestBy = staff.EmpCode;
            }
            else
                BSM.HeaderOT.EmpCode = user.UserName;
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public async Task<ActionResult> Index(PROverTimeObject BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.ListEmpStaff = await BSM.LoadData(BSM._Filter, SYConstant.getBranchDataAccess());
            var staff = await DB.HRStaffProfiles.FirstOrDefaultAsync(w => w.EmpCode == user.UserName);
            if (staff != null)
            {
                BSM.HeaderOT.RequestBy = staff.EmpCode;
            }
            else
                BSM.HeaderOT.EmpCode = user.UserName;
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        #endregion
        public ActionResult AssignStaff(DateTime OTDate, string EmpCode, DateTime FromDate, DateTime ToDate, string Remark, string RequestBy, decimal TotalHour, string Category)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new PROverTimeObject();
            BSM.HeaderOT = new HRRequestOT();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.DocType = DOCTYPE;
            if (EmpCode != "")
            {
                BSM._Filter.Remark = Remark;
                BSM._Filter.TotalHours = TotalHour;
                BSM._Filter.InDate = OTDate;
                BSM._Filter.StartTime = FromDate;
                BSM._Filter.EndTime = ToDate;
                BSM._Filter.EmpCode = EmpCode;
                BSM._Filter.RequestBy = RequestBy;
                BSM._Filter.OTCategory = Category;
                BSM.ScreenId = SCREEN_ID;
                var msg = BSM.Request(EmpCode, BSM._Filter);

                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("REQUEST_SUCCESSFULLY", user.Lang);
                    ViewData[SYSConstant.PARAM_ID] = EmpCode;
                }
                else
                {
                    SYMessages mess = SYMessages.getMessageObject(msg, user.Lang);
                    mess.Description = BSM.MessageError + ":" + mess.Description;


                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INVALID_EMP", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            PROverTimeObject BSM = new PROverTimeObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PROverTimeObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmpStaff);
        }
        [HttpPost]
        public ActionResult ShowData(DateTime FromDate, DateTime ToDate, decimal BreakTime)
        {

            ActionName = "Index";

            decimal Hour = Math.Round(Convert.ToDecimal(ToDate.Subtract(FromDate).TotalMinutes), 2) - BreakTime;
            Hour = Hour / 60.00M;
            var result = new
            {
                MS = SYConstant.OK,
                Hour = Hour
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            string DEDType = "ALLW";
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();// DH.HRBranches.ToList();
            ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData["ALLW_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == DEDType).ToList();
            ViewData["SALARTYPE_SELECT"] = ClsSelaryType.LoadData();
            ViewData["SHIFT_SELECT"] = DB.ATShifts.ToList();
            var staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == user.UserName);
            var ListStaff = new List<HRStaffProfile>();
            if (staff != null)
            {
                var _listEmp = DB.HRStaffProfiles.Where(w => w.EmpCode == user.UserName || w.FirstLine == staff.EmpCode || w.SecondLine == staff.EmpCode || w.HODCode == staff.EmpCode).ToList();
                ListStaff = _listEmp.ToList();
            }
            ViewData["STAFF_SELECT"] = ListStaff.ToList();
            var BS = new PROverTimeObject();
            foreach (var data in BS.OnDataSelectorPolicy())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}
