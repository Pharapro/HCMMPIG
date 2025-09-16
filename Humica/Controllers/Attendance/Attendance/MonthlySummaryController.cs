using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.Att;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.Attendance
{
    public class MonthlySummaryController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ATM0000006";
        private const string URL_SCREEN = "/Attendance/Attendance/MonthlySummary/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public MonthlySummaryController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            ATEmpSchObject BSM = new ATEmpSchObject();
            FTFilterEmployee Filter = new FTFilterEmployee();
            BSM.ListEmpSchdule = new List<VIEW_ATEmpSchedule>();
            BSM.ListEmpSchduleDetail = new List<VIEW_ATEmpSchedule>();
            BSM.FMonthly = new Humica.Core.FT.FTMonthlySum();
            DateTime DNow = DateTime.Now;
            //BSM.FMonthly.FromDate = new DateTime(DNow.Year, DNow.Month, 1);
            //BSM.FMonthly.ToDate = new DateTime(DNow.Year, DNow.Month, DateTime.DaysInMonth(DNow.Year, DNow.Month));
            var period = DB.ATPayperiods.OrderByDescending(w => w.FromDate).FirstOrDefault();
            if (period != null)
            {
                BSM.FMonthly.PayPeriodId = period.PeriodID;
            }
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ATEmpSchObject BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            var period = DB.ATPayperiods.FirstOrDefault(w => w.PeriodID == BSM.FMonthly.PayPeriodId);
            if (period != null)
            {
                var ListEmpSchdule = DBV.VIEW_ATEmpSchedule.Where(w => w.TranDate >= period.FromDate
                && w.TranDate <= period.ToDate).ToList();
                var listBranch = SYConstant.getBranchDataAccess();
                BSM.ListEmpSchduleDetail = new List<VIEW_ATEmpSchedule>();

                
                if (BSM.FMonthly.Branch != null)
                {
                    string[] Branch = BSM.FMonthly.Branch.Split(',');
                    List<string> LstBranch = new List<string>();
                    foreach (var read in Branch)
                    {
                        if (read.Trim() != "")
                        {
                            LstBranch.Add(read.Trim());
                        }
                    }

                    ListEmpSchdule = ListEmpSchdule.Where(w => LstBranch.Contains(w.Branch)).ToList();
                }

                    if (BSM.FMonthly.Department != null)
                        ListEmpSchdule = ListEmpSchdule.Where(w => w.DEPT == BSM.FMonthly.Department).ToList();
                    if (BSM.FMonthly.Location != null)
                        ListEmpSchdule = ListEmpSchdule.Where(w => w.LOCT == BSM.FMonthly.Location).ToList();
                    if (BSM.FMonthly.Division != null)
                        ListEmpSchdule = ListEmpSchdule.Where(w => w.DivisionCode == BSM.FMonthly.Division).ToList();
                    if (BSM.FMonthly.EmpCode != null)
                        ListEmpSchdule = ListEmpSchdule.Where(w => w.EmpCode == BSM.FMonthly.EmpCode).ToList();
                    var _empMonthlySum = ListEmpSchdule.GroupBy(w => new { w.EmpCode, w.AllName, w.Department, w.LevelCode, w.Position, w.StartDate }).
                        Select(x => new VIEW_ATEmpSchedule
                        {
                            EmpCode = x.Key.EmpCode,
                            AllName = x.Key.AllName,
                            Department = x.Key.Department,
                            LevelCode = x.Key.LevelCode,
                            Position = x.Key.Position,
                            StartDate = x.Key.StartDate,
                            WHOUR = x.Sum(s => s.WHOUR),
                            Late1 = x.Sum(s => s.Late1),
                            Early1 = x.Sum(s => s.Early1),
                            Late2 = x.Sum(s => s.Late2),
                            Early2 = x.Sum(s => s.Early2),
                            NWH = x.Sum(s => s.NWH),
                            WOT = x.Sum(s => s.WOT),
                            MEAL = x.Sum(s => s.MEAL)
                        }).ToList();
                    BSM.ListEmpSchdule = _empMonthlySum.OrderBy(x => x.EmpCode).ToList();
               // }
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);

            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                
        }

        #endregion
        public ActionResult TransferAtt(string TranNo)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new ATEmpSchObject();
            if (TranNo != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
                }
                var msg = "";// BSM.TransferAttendance(TranNo);

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
        public ActionResult TransferWorkingDay(string TranNo)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new ATEmpSchObject();
            if (TranNo != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
                }
                var msg = BSM.TransferWorkingHour(TranNo);

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
        public ActionResult TransferLeave(string TranNo)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new ATEmpSchObject();
            if (TranNo != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
                }
                var msg = BSM.TransferLeave(TranNo);

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
            ATEmpSchObject BSM = new ATEmpSchObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmpSchdule);
        }
        public ActionResult GridItemDetails()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            ATEmpSchObject BSM = new ATEmpSchObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemDetails";
            return PartialView("GridItemDetails", BSM.ListEmpSchduleDetail);
        }
        public ActionResult ShowEmpDetail(string EmpCode,int PayPeriodId)
        {
            ActionName = "Index";
            ATEmpSchObject BSM = new ATEmpSchObject();
            var Stafff = DBV.VIEW_ATEmpSchedule.ToList();
            var period = DB.ATPayperiods.FirstOrDefault(w => w.PeriodID == PayPeriodId);

            BSM.ListEmpSchduleDetail = Stafff.Where(w => w.EmpCode == EmpCode && w.TranDate.Date >= period.FromDate.Date
            && w.TranDate.Date <= period.ToDate.Date).OrderBy(x => x.TranDate).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            if (BSM.ListEmpSchduleDetail != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,

                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult ShowData(string ID, string EmpCode)
        {

            ActionName = "Details";
            var Stafff = DBV.HR_STAFF_VIEW;
            HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == EmpCode);
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
            //ViewData["SHIFT_SELECT"] = DB.ATShifts.ToList();
            ViewData["BRANCHES_SELECT"] = ListBranch;// DB.HRBranches.ToList();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["LOCATION_SELECT"] = DB.HRLocations.ToList().OrderBy(w => w.Description);
            ViewData["PERIOD_SELECT"] = DB.ATPayperiods.ToList().OrderByDescending(w => w.FromDate);
        }
    }
}
