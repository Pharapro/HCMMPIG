using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{
    public class PRHoldSalaryController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000026";
        private const string URL_SCREEN = "/PR/PRM/PRHoldSalary/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public PRHoldSalaryController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);

            PREmpHoldObject BSM = new PREmpHoldObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpHoldObject)Session[Index_Sess_Obj + ActionName];
            }
            var ListHeaderLoad = DB.PREmpHolds.ToList();
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();
            BSM.ListHeader = ListHeaderLoad.Where(x => Staff.Where(w => w.EmpCode == x.EmpCode).Any()).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PREmpHoldObject BSM = new PREmpHoldObject();
            BSM.ListHeader = new List<PREmpHold>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpHoldObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            DataSelector();
            UserSession();
            PREmpHoldObject BSM = new PREmpHoldObject();
            UserConfListAndForm(this.KeyName);
            BSM.Header = new PREmpHold();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.Header.InMonth = DateTime.Now;
            BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(PREmpHoldObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PREmpHoldObject BSM = new PREmpHoldObject();
            BSM.Header = collection.Header;
            BSM.HeaderStaff = collection.HeaderStaff;
            BSM.ScreenId = SCREEN_ID;
            try
            {
                string msg = BSM.CreateHold();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM.Header = new PREmpHold();
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
                return View(BSM);
            }
        }
        #endregion

        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id == "null") id = null;
            if (id != null)
            {
                PREmpHoldObject BSM = new PREmpHoldObject();
                BSM.Header = new PREmpHold();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.PREmpHolds.FirstOrDefault(w => w.TranNo == TranNo);
                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, PREmpHoldObject collection)
        {
            ActionName = "Edit";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            PREmpHoldObject BSM = new PREmpHoldObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PREmpHoldObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                    BSM.HeaderStaff = collection.HeaderStaff;
                }
                BSM.ScreenId = SCREEN_ID;
                int TranNo = Convert.ToInt32(id);
                string msg = BSM.EditHold(TranNo);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return View(BSM);

        }
        #endregion

        #region "Delete"
        public ActionResult Delete(int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.DELETE);
            DataSelector();
            if (id != 0)
            {
                PREmpHoldObject GLA = new PREmpHoldObject();
                string msg = GLA.Delete(id);
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
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion

        #region "Details"
        public ActionResult Details(string id)
        {
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id == "null") id = null;
            if (id != null)
            {
                PREmpHoldObject BSM = new PREmpHoldObject();
                BSM.Header = new PREmpHold();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.PREmpHolds.FirstOrDefault(w => w.TranNo == TranNo);
                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #endregion

        #region "Approve"
        public ActionResult Approve(string id)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = id;
            PREmpHoldObject BSM = new PREmpHoldObject();
            if (id.ToString() != "null")
            {
                int TranNo = Convert.ToInt32(id);
                string msg = BSM.ApproveOTReq(TranNo);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.Description = messageObject.Description + BSM.MessageError;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region "Approve"
        public ActionResult PayBack(string id, DateTime PayBack)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = id;
            PREmpHoldObject BSM = new PREmpHoldObject();
            if (id.ToString() != "null")
            {
                int TranNo = Convert.ToInt32(id);
                string msg = BSM.PayBackHold(TranNo, PayBack);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.Description = messageObject.Description + BSM.MessageError;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_RELEASED", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion

        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            //ActionName = "Details";
            this.UserSession();
            DataSelector();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID] = id;
            PREmpHoldObject BSD = new PREmpHoldObject();
            if (id.ToString() != "null")
            {
                int TranNo = Convert.ToInt32(id);
                string sms = BSD.Cancel(TranNo);
                if (sms == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(sms, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
        }
        #endregion
        public ActionResult getEmpCode(string EmpCode, DateTime InMonth)
        {
            var EmpStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == EmpCode);
            var ListSalary = DB.HISGenSalaries.Where(w => w.EmpCode == EmpCode && w.INYear == InMonth.Year && w.INMonth == InMonth.Month).ToList();
            if (ListSalary.Sum(x => x.NetWage) <= 0)
            {
                var rs1 = new
                {
                    MS = "SALARY_IS_NOT_GENERATE",
                    AllName = "",
                    EmpType = "",
                    Division = "",
                    DEPT = "",
                    SECT = "",
                    LevelCode = "",
                    Position = "",
                    StartDate = "",
                    Salary = 0
                };
                return Json(rs1, JsonRequestBehavior.DenyGet);
            }
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmployeeType,
                    Division = EmpStaff.DivisionDesc,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.LevelCode,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate,
                    Salary = ListSalary.Sum(x => x.NetWage)
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }

        private void DataSelector()
        {
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();

            ViewData["STAFF_SELECT"] = Staff;
            string ALLType = "ALLW";
            ViewData["ALLOWANCE_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == ALLType).ToList();
        }
    }
}