using Humica.Attendance;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.Attendance
{
    public class ATEmployeeOTController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ATM0000012";
        private const string URL_SCREEN = "/Attendance/Attendance/ATEmployeeOT/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "TranNo";
        IAttendanceObject BSM;
        IUnitOfWork unitOfWork;
        public ATEmployeeOTController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new AttendanceObject();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
            BSM.OnLoad();
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            BSM.ListOT = new List<HRRequestOT>();
            BSM.Attendance = new FTFilterAttendance();
            DateTime DNow = DateTime.Now;
            var period = unitOfWork.Repository<ATPayperiod>().Queryable().OrderByDescending(w => w.FromDate).FirstOrDefault();
            if (period != null)
            {
                BSM.Attendance.PeriodID = period.PeriodID;
            }
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (AttendanceObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(AttendanceObject BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            string approve = SYDocumentStatus.APPROVED.ToString();
            BSM.ListOT = new List<HRRequestOT>();
            var payPeriod = unitOfWork.Repository<ATPayperiod>().Queryable()
                .FirstOrDefault(w => w.PeriodID == BSM.Attendance.PeriodID);
            if (payPeriod != null)
            {
                var baseQuery = from ot in unitOfWork.Repository<HRRequestOT>().Queryable()
                                join staff in unitOfWork.Repository<HRStaffProfile>().Queryable()
                                    on ot.EmpCode equals staff.EmpCode
                                where ot.OTActual != 0 && ot.Status == approve
                                select new { ot, staff };
                baseQuery = baseQuery.Where(x =>
                    (DbFunctions.TruncateTime(x.ot.OTStartTime) <= payPeriod.ToDate && DbFunctions.TruncateTime(x.ot.OTEndTime) >= payPeriod.FromDate) ||
                    (DbFunctions.TruncateTime(x.ot.OTStartTime) >= payPeriod.FromDate && DbFunctions.TruncateTime(x.ot.OTStartTime) <= payPeriod.ToDate) ||
                    (DbFunctions.TruncateTime(x.ot.OTEndTime) >= payPeriod.FromDate && DbFunctions.TruncateTime(x.ot.OTEndTime) <= payPeriod.ToDate));
                if (!string.IsNullOrEmpty(BSM.Attendance.Branch))
                {
                    var branches = BSM.Attendance.Branch.Split(',')
                        .Select(b => b.Trim())
                        .Where(b => !string.IsNullOrEmpty(b))
                        .ToList();
                    baseQuery = baseQuery.Where(x => branches.Contains(x.staff.Branch));
                }
                if (!string.IsNullOrEmpty(BSM.Attendance.Department))
                    baseQuery = baseQuery.Where(x => x.staff.DEPT == BSM.Attendance.Department);
                if (!string.IsNullOrEmpty(BSM.Attendance.Locations))
                    baseQuery = baseQuery.Where(x => x.staff.LOCT == BSM.Attendance.Locations);
                if (!string.IsNullOrEmpty(BSM.Attendance.Division))
                    baseQuery = baseQuery.Where(x => x.staff.Division == BSM.Attendance.Division);
                if (!string.IsNullOrEmpty(BSM.Attendance.EmpCode))
                    baseQuery = baseQuery.Where(x => x.ot.EmpCode == BSM.Attendance.EmpCode);
                BSM.ListOT = baseQuery
                    .ToList()
                    .Select(x => x.ot)
                    .OrderBy(x => x.EmpCode)
                    .ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        #endregion
        public ActionResult TransferOT(string TranNo)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (TranNo != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (AttendanceObject)Session[Index_Sess_Obj + ActionName];
                }
                var msg = BSM.TransferOT(TranNo, BSM.ListOT);

                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("TANSFER_COMPLATED", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (AttendanceObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListOT);
        }
        public ActionResult ShowData(string ID, string EmpCode)
        {

            ActionName = "Details";
            var EmpStaff = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmpType,
                    Division = EmpStaff.Division,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.LevelCode,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            var ListBranch = SYConstant.getBranchDataAccess();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["BRANCHES_SELECT"] = ListBranch;
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["LOCATION_SELECT"] = unitOfWork.Repository<HRLocation>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["PERIOD_SELECT"] = unitOfWork.Repository<ATPayperiod>().Queryable().ToList().OrderByDescending(w => w.PeriodID);
        }
    }
}
