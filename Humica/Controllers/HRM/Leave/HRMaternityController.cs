using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Leave
{

    public class HRMaternityController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRL0000005";
        private const string URL_SCREEN = "/HRM/Leave/HRMaternity/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public HRMaternityController()
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

            MaternityObject BSM = new MaternityObject();

            BSM.ListHeader = DB.HRReqMaternities.ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }

        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            MaternityObject BSM = new MaternityObject();
            BSM.ListHeader = new List<HRReqMaternity>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (MaternityObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            MaternityObject BSM = new MaternityObject();

            BSM.User = SYSession.getSessionUser();
            BSM.Header = new HRReqMaternity();
            BSM.Header.FromDate = DateTime.Now;
            BSM.Header.ToDate = DateTime.Now;
            BSM.Header.RequestDate = DateTime.Now;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(MaternityObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            //ViewData[SYSConstant.PARAM_ID] = ID;
            var BSM = new MaternityObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (MaternityObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;

            }
            if (ModelState.IsValid)
            {

                string msg = BSM.CreateMaternity();

                if (msg == SYConstant.OK)
                {
                    BSM.ScreenId = SCREEN_ID;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;

                    BSM.Header = new HRReqMaternity();
                    BSM.Header.FromDate = DateTime.Now;
                    BSM.Header.ToDate = DateTime.Now;
                    BSM.Header.RequestDate = DateTime.Now;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion

        #region "Edit"
        public ActionResult Edit(int id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            MaternityObject BSM = new MaternityObject();
            BSM.Header = new HRReqMaternity();

            if (id != null)
            {
                BSM.Header = DB.HRReqMaternities.Find(id);
                var staff = DBV.HR_STAFF_VIEW.FirstOrDefault(x => x.EmpCode == BSM.Header.EmpCode);
                if (staff != null)
                {
                    BSM.Position = staff.Position;
                    BSM.EmpName = staff.AllName;
                    BSM.Department = staff.Department;

                }

                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(int id, MaternityObject collection)
        {
            ActionName = "Edit";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            MaternityObject BSM = new MaternityObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (MaternityObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ScreenId = SCREEN_ID;
            BSM.Header = collection.Header;

            //int TranNo = Convert.ToInt32(id);
            string msg = BSM.editMaternity(id);
            if (msg == SYConstant.OK)
            {
                DB = new HumicaDBContext();
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = Convert.ToString(BSM.Header.TranNo);
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            else
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

        }
        #endregion
        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.DELETE);
            DataSelector();
            if (id == "null") id = null;
            if (id != null)
            {
                MaternityObject Del = new MaternityObject();
                int TranNo = Convert.ToInt32(id);
                string msg = Del.deleteMaternity(TranNo);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("LEAVE_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region "Details"
        public ActionResult Details(int id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[SYSConstant.PARAM_ID] = id;
            MaternityObject BSM = new MaternityObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (MaternityObject)Session[Index_Sess_Obj + ActionName];
            }
            if (id != null)
            {
                BSM.Header = DB.HRReqMaternities.Find(id);
                var staff = DBV.HR_STAFF_VIEW.FirstOrDefault(x => x.EmpCode == BSM.Header.EmpCode);

                if (staff != null)
                {
                    BSM.Position = staff.Position;
                    BSM.EmpName = staff.AllName;
                    BSM.Department = staff.Department;

                }
                return View(BSM);

            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion


        [HttpPost]
        public ActionResult ShowData(string ID, DateTime FromDate, DateTime ToDate, string EmpCode, string Action)
        {

            ActionName = Action;// "Create";
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
                BSM.ListEmpLeaveD = new List<HREmpLeaveD>();
            }
            if (EmpCode == "")
            {
                var rs1 = new { MS = "INVALIT_EMPLOYEE" };
                return Json(rs1, JsonRequestBehavior.DenyGet);
            }
            if (ID == "" || ID == null)
            {
                var rs1 = new { MS = "LeaveType_INVALIT" };
                return Json(rs1, JsonRequestBehavior.DenyGet);
            }
            int result1 = 0;
            int Line = 0;
            int NPH = 0;
            int NRest = 0;
            var Pub = DB.HRPubHolidays.ToList();
            BSM.HeaderStaff = DBV.HR_STAFF_VIEW.SingleOrDefault(w => w.EmpCode == EmpCode);
            Pub = Pub.Where(w => w.PDate.Date >= FromDate.Date && w.PDate <= ToDate.Date).ToList();
            if (BSM.HeaderStaff != null)
            {
                var Parameters = DB.PRParameters.Find(BSM.HeaderStaff.PayParam);
                string Status = "";
                for (var day = FromDate.Date; day.Date <= ToDate.Date; day = day.AddDays(1))
                {
                    Status = SYDocumentStatus.Leave.ToString();
                    if (Pub.ToList().Count > 0)
                    {
                        if (Pub.Where(w => w.PDate.Date == day.Date).Any())
                        {
                            NPH += 1; Status = SYDocumentStatus.PH.ToString();
                        }
                    }
                    else if (Parameters != null)
                    {
                        if (Parameters.WDMON != true && day.DayOfWeek == DayOfWeek.Monday) { NRest += 1; Status = SYDocumentStatus.Rest.ToString(); }
                        else if (Parameters.WDTUE != true && day.DayOfWeek == DayOfWeek.Tuesday) { NRest += 1; Status = SYDocumentStatus.Rest.ToString(); }
                        else if (Parameters.WDWED != true && day.DayOfWeek == DayOfWeek.Wednesday) { NRest += 1; Status = SYDocumentStatus.Rest.ToString(); }
                        else if (Parameters.WDTHU != true && day.DayOfWeek == DayOfWeek.Thursday) { NRest += 1; Status = SYDocumentStatus.Rest.ToString(); }
                        else if (Parameters.WDFRI != true && day.DayOfWeek == DayOfWeek.Friday) { NRest += 1; Status = SYDocumentStatus.Rest.ToString(); }
                        else if (Parameters.WDSAT != true && day.DayOfWeek == DayOfWeek.Saturday) { NRest += 1; Status = SYDocumentStatus.Rest.ToString(); }
                        else if (Parameters.WDSUN != true && day.DayOfWeek == DayOfWeek.Sunday) { NRest += 1; Status = SYDocumentStatus.Rest.ToString(); }
                    }
                    result1 += 1;
                    Line += 1;

                    var Leaved = new HREmpLeaveD();
                    Leaved.LeaveDate = day;
                    Leaved.CutMonth = day;
                    Leaved.Remark = "FullDay";
                    Leaved.LHour = Parameters.WHOUR;
                    Leaved.LineItem = Line;
                    Leaved.Status = Status;
                    BSM.ListEmpLeaveD.Add(Leaved);
                }

                //if (Balance.Balance <= BSM.ListEmpLeaveD.Where(w => w.Status == SYDocumentStatus.Leave.ToString()).Count())
                //{
                //    //Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("BALANCE_INVALIT", user.Lang);
                //    var rs1 = new { MS = "BALANCE_INVALIT" };
                //    return Json(rs1, JsonRequestBehavior.DenyGet);
                //}
                var result = new
                {
                    MS = SYConstant.OK,
                    NoDay = BSM.ListEmpLeaveD.Where(w => w.Status == SYDocumentStatus.Leave.ToString()).Count(),
                    NoPH = BSM.ListEmpLeaveD.Where(w => w.Status == SYDocumentStatus.PH.ToString()).Count(),
                    NoRest = BSM.ListEmpLeaveD.Where(w => w.Status == SYDocumentStatus.Rest.ToString()).Count(),
                    Balance = 0// Balance.Balance
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {

            ActionName = "Details";
            var Stafff = DBV.HR_STAFF_VIEW;
            HR_STAFF_VIEW EmpStaff = Stafff.SingleOrDefault(w => w.EmpCode == EmpCode);
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

            ViewData["STAFF_SELECT"] = DBV.HR_STAFF_VIEW.Where(x => x.Sex == "F").ToList();

        }
    }
}
