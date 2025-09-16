using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.EmployeeInfo
{
    public class HRStaffDepositController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRE0000012";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HRStaffDeposit/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "DepositNum";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public HRStaffDepositController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);

            HREmpDepositObject BSM = new HREmpDepositObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpDepositObject)Session[Index_Sess_Obj + ActionName];
            }
            var ListHeaderLoad = await DB.HREmpDeposits.ToListAsync();
            var Staff = await DBV.HR_STAFF_VIEW.ToListAsync();
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
            HREmpDepositObject BSM = new HREmpDepositObject();
            BSM.ListHeader = new List<HREmpDeposit>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpDepositObject)Session[Index_Sess_Obj + ActionName];
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
            HREmpDepositObject BSM = new HREmpDepositObject();
            UserConfListAndForm(this.KeyName);
            BSM.Header = new HREmpDeposit();
            BSM.ListHeaderD = new List<HREmpDepositItem>();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.Header.FromDate = DateTime.Now;
            BSM.Header.ToDate = DateTime.Now;
            BSM.Header.Amount = 0;
            BSM.Header.Deposit = 0;
            BSM.Header.Period = 1;
            BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(HREmpDepositObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            HREmpDepositObject BSM = new HREmpDepositObject();
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpDepositObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
                BSM.HeaderStaff = collection.HeaderStaff;
            }
            if (ModelState.IsValid)
            {
                if (BSM.Header.Amount == BSM.ListHeaderD.Sum(x => x.Deposit))
                {
                    string msg = BSM.CreateDeposit();
                    if (msg == SYConstant.OK)
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        mess.DocumentNumber = BSM.Header.DepositNum;
                        mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                        Session[Index_Sess_Obj + ActionName] = null;
                        Session[SYConstant.MESSAGE_SUBMIT] = mess;
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                    }
                    else
                    {
                        ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
                        return View(BSM);
                    }

                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_AMOUNT", user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion

        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id.ToString() != "null")
            {
                HREmpDepositObject BSM = new HREmpDepositObject();
                BSM.Header = new HREmpDeposit();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                BSM.Header = DB.HREmpDeposits.FirstOrDefault(w => w.DepositNum == id);
                if (BSM.Header != null)
                {
                    BSM.ListHeaderD = DB.HREmpDepositItems.Where(w => w.DepositNum == BSM.Header.DepositNum).ToList();
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, HREmpDepositObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            var BSM = new HREmpDepositObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpDepositObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
                BSM.HeaderStaff = collection.HeaderStaff;
            }

            if (id != null)
            {
                if (BSM.Header.Amount == BSM.ListHeaderD.Sum(x => x.Deposit))
                {
                    BSM.ScreenId = SCREEN_ID;
                    string msg = BSM.EditDeposit(id);
                    if (msg == SYConstant.OK)
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        mess.DocumentNumber = id.ToString();
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
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_AMOUNT", user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return View(BSM);

        }
        #endregion

        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.DELETE);
            DataSelector();
            if (id != "null")
            {
                HREmpDepositObject GLA = new HREmpDepositObject();
                string msg = GLA.DeleteDeposit(id);
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
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != "null")
            {
                HREmpDepositObject BSM = new HREmpDepositObject();
                BSM.Header = new HREmpDeposit();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                BSM.Header = DB.HREmpDeposits.FirstOrDefault(w => w.DepositNum == id);
                if (BSM.Header != null)
                {
                    BSM.ListHeaderD = DB.HREmpDepositItems.Where(w => w.DepositNum == id).ToList();
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #endregion
        public ActionResult GridItemDetails()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            HREmpDepositObject BSM = new HREmpDepositObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpDepositObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemDetails";
            return PartialView("GridItemDetails", BSM.ListHeaderD);
        }
        public ActionResult CreatEditDeposit(HREmpDepositItem MModel)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            HREmpDepositObject BSM = new HREmpDepositObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (HREmpDepositObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var DBU = new HumicaDBContext();
                    var ListHeaderD = BSM.ListHeaderD.Where(w => w.LineItem == MModel.LineItem).ToList();
                    if (ListHeaderD.Count > 0)
                    {
                        var objUpdate = ListHeaderD.First();
                        objUpdate.PayMonth = MModel.PayMonth;
                        objUpdate.Deposit = MModel.Deposit;
                        objUpdate.Remark = MModel.Remark;
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            DataSelector();
            return PartialView("GridItemDetails", BSM.ListHeaderD);
        }
        public ActionResult GridItemView()
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            HREmpDepositObject BSM = new HREmpDepositObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpDepositObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemView";
            return PartialView("GridItemView", BSM.ListHeaderD);
        }
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
        #region "PayBack"
        public ActionResult PayBack(string id, DateTime PayBack)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = id;
            HREmpDepositObject BSM = new HREmpDepositObject();
            if (id.ToString() != "null")
            {
                string msg = BSM.PayBackDeposit(id, PayBack);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.Description = messageObject.Description;
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
        public ActionResult ShowDataEmp(string ID, string EmpCode)
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
                    Division = EmpStaff.DivisionDesc,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.LevelCode,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate
                };

                // GetData(EmpCode, "Create");
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);

        }
        [HttpPost]
        public ActionResult GetData(DateTime FromDate, int Period, decimal LoanAmount, decimal Amount)
        {
            this.ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Period > 0) Period -= 1;
            DateTime ToDate = FromDate.AddMonths(Period);
            HREmpDepositObject BSM = new HREmpDepositObject();
            //if (FromDate.Date > ToDate.Date)
            //    ToDate = FromDate;
            //if (Convert.ToDecimal(Amount) > 0)
            //{
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpDepositObject)Session[Index_Sess_Obj + ActionName];
                BSM.ListHeaderD = new List<HREmpDepositItem>();
                Decimal _loanAmount = LoanAmount;
                int CountMount = (((ToDate.Year - FromDate.Year) * 12) + ToDate.Month - FromDate.Month);
                int C_Month = (((ToDate.Year - FromDate.Year) * 12) + ToDate.Month - FromDate.Month) + 1;
                int Line = 0;

                for (var i = 0; i <= CountMount; i++)
                {

                    Line += 1;
                    var _loan = new HREmpDepositItem();
                    if (Line == C_Month) Amount = _loanAmount;
                    _loan.InMonth = FromDate.AddMonths(i);
                    _loan.PayMonth = FromDate.AddMonths(i);
                    if (C_Month == 0) _loan.Deposit = Amount;
                    else _loan.Deposit = Amount;
                    _loan.LineItem = Line;

                    _loanAmount -= Amount;
                    if (_loanAmount < 0)
                    {
                        var rs1 = new { MS = "Invalid Period" };
                        return Json(rs1, JsonRequestBehavior.DenyGet);
                    }
                    if (_loan.Deposit > 0)
                        BSM.ListHeaderD.Add(_loan);
                }
                var result = new
                {
                    MS = SYConstant.OK,
                    ToDate = ToDate
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            //   }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }

        private void DataSelector()
        {
            var Staff = DBV.HR_STAFF_VIEW.Where(w => w.StatusCode == "A").ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();

            ViewData["STAFF_SELECT"] = Staff;
            string ALLType = "DED";
            ViewData["DEDUCTION_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == ALLType).ToList();
        }
    }
}