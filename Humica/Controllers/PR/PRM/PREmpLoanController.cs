using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using HUMICA.Models.Report.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{
    public class PREmpLoanController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000018";
        private const string URL_SCREEN = "/PR/PRM/PREmpLoan/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "LoanID";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public PREmpLoanController()
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

            PREmpLoan BSM = new PREmpLoan();
            BSM.ListHeader = new List<HR_VIEW_EMPLOAN>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpLoan)Session[Index_Sess_Obj + ActionName];
            }

            var ListHeader = DBV.HR_VIEW_EMPLOAN.ToList();
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();
            BSM.ListHeader = ListHeader.Where(x => Staff.Where(w => w.EmpCode == x.EmpCode).Any()).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PREmpLoan BSM = new PREmpLoan();
            BSM.ListHeader = new List<HR_VIEW_EMPLOAN>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpLoan)Session[Index_Sess_Obj + ActionName];
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
            PREmpLoan BSM = new PREmpLoan();
            UserConfListAndForm(this.KeyName);
            BSM.Header = new HREmpLoanH();
            BSM.ListHeaderD = new List<HREmpLoan>();
            BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
            BSM.Header.FromDate = DateTime.Now;
            BSM.Header.ToDate = DateTime.Now;
            BSM.Header.Amount = 0;
            BSM.Header.Period = 1;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(PREmpLoan collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PREmpLoan BSM = new PREmpLoan();
            BSM = (PREmpLoan)Session[Index_Sess_Obj + ActionName];
            BSM.Filter = collection.Filter;
            BSM.Header = collection.Header;
            if (BSM.Header.LoanAmount == BSM.ListHeaderD.Sum(x => x.LoanAm))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateLoan();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.LoanID;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
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
                PREmpLoan BSM = new PREmpLoan();
                BSM.ListHeaderD = new List<HREmpLoan>();
                BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
                BSM.Header = DB.HREmpLoanHs.Find(id);
                if (BSM.Header != null)
                {
                    BSM.ListHeaderD = DB.HREmpLoans.Where(w => w.LoanID == BSM.Header.LoanID).ToList();
                    var res = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    if (res != null)
                    {
                        BSM.Filter.EmpCode = res.EmpCode;
                        BSM.Filter.AllName = res.AllName;
                        BSM.Filter.EmpType = res.EmployeeType;
                        BSM.Filter.Division = res.Division;
                        BSM.Filter.Department = res.Department;
                        BSM.Filter.Section = res.Section;
                        BSM.Filter.Position = res.Position;
                        BSM.Filter.Level = res.Level;
                        BSM.Filter.StartDate = Convert.ToDateTime(res.StartDate);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        return View(BSM);
                    }
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, PREmpLoan collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            var BSM = new PREmpLoan();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpLoan)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = collection.Filter;
                BSM.Header = collection.Header;
            }

            if (id != null)
            {
                if (BSM.Header.LoanAmount == BSM.ListHeaderD.Sum(x => x.LoanAm))
                {
                    BSM.ScreenId = SCREEN_ID;
                    string msg = BSM.EditEmpLoan(id);
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
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
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
                PREmpLoan Del = new PREmpLoan();
                string msg = Del.DeleteLoan(id);
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
                PREmpLoan BSM = new PREmpLoan();
                BSM.ListHeaderD = new List<HREmpLoan>();
                BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
                BSM.Header = DB.HREmpLoanHs.Find(id);
                if (BSM.Header != null)
                {
                    BSM.ListHeaderD = DB.HREmpLoans.Where(w => w.LoanID == BSM.Header.LoanID).ToList();
                    var res = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    if (res != null)
                    {
                        BSM.Filter.EmpCode = res.EmpCode;
                        BSM.Filter.AllName = res.AllName;
                        BSM.Filter.EmpType = res.EmployeeType;
                        BSM.Filter.Division = res.Division;
                        BSM.Filter.Department = res.Department;
                        BSM.Filter.Section = res.Section;
                        BSM.Filter.Position = res.Position;
                        BSM.Filter.Level = res.Level;
                        BSM.Filter.StartDate = Convert.ToDateTime(res.StartDate);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        return View(BSM);
                    }
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
            PREmpLoan BSM = new PREmpLoan();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpLoan)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemDetails";
            return PartialView("GridItemDetails", BSM.ListHeaderD);
        }
        public ActionResult CreatEditLoan(HREmpLoan MModel)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            PREmpLoan BSM = new PREmpLoan();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PREmpLoan)Session[Index_Sess_Obj + ActionName];
                    }
                    var DBU = new HumicaDBContext();
                    string Open = SYDocumentStatus.OPEN.ToString();
                    var ListHeaderD = BSM.ListHeaderD.Where(w => w.LineItem == MModel.LineItem).ToList();
                    if (ListHeaderD.Where(w => w.Status == Open).Any())
                    {
                        if (ListHeaderD.Count > 0)
                        {
                            var objUpdate = ListHeaderD.First();
                            objUpdate.PayMonth = MModel.PayMonth;
                            objUpdate.LoanAm = MModel.LoanAm;
                            objUpdate.Remark = MModel.Remark;
                            Session[Index_Sess_Obj + ActionName] = BSM;
                        }
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("LOAN_READY");
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
            PREmpLoan BSM = new PREmpLoan();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpLoan)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemView";
            return PartialView("GridItemView", BSM.ListHeaderD);
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
        [HttpPost]
        public ActionResult GetData(DateTime FromDate, int Period, decimal LoanAmount, decimal Amount)
        {
            this.ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Period > 0) Period -= 1;
            DateTime ToDate = FromDate.AddMonths(Period);
            PREmpLoan BSM = new PREmpLoan();
            //if (FromDate.Date > ToDate.Date)
            //    ToDate = FromDate;
            //if (Convert.ToDecimal(Amount) > 0)
            //{
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpLoan)Session[Index_Sess_Obj + ActionName];
                BSM.ListHeaderD = new List<HREmpLoan>();
                Decimal _loanAmount = LoanAmount;
                int CountMount = (((ToDate.Year - FromDate.Year) * 12) + ToDate.Month - FromDate.Month);
                int C_Month = (((ToDate.Year - FromDate.Year) * 12) + ToDate.Month - FromDate.Month) + 1;
                // for (var day = FromDate.Date; day.Date <= ToDate.Date; day = day.AddDays(1))
                int Line = 0;

                for (var i = 0; i <= CountMount; i++)
                {

                    Line += 1;
                    var _loan = new HREmpLoan();
                    if (Line == C_Month) Amount = _loanAmount;
                    _loan.LoanDate = FromDate.AddMonths(i);
                    _loan.PayMonth = FromDate.AddMonths(i);
                    if (C_Month == 0) _loan.LoanAm = Amount;
                    else _loan.LoanAm = Amount;
                    _loan.LineItem = Line;
                    _loan.Status = SYDocumentStatus.OPEN.ToString();
                    _loan.BeginingBalance = _loanAmount;
                    _loan.EndingBalance = _loanAmount - Amount;
                    _loanAmount -= Amount;
                    if (_loanAmount < 0)
                    {
                        var rs1 = new { MS = "Invalid Period" };
                        return Json(rs1, JsonRequestBehavior.DenyGet);
                    }
                    if (_loan.LoanAm > 0)
                        BSM.ListHeaderD.Add(_loan);
                }
                //   Session[Index_Sess_Obj + ActionName] = BSM;
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
        #region "Print"
        public ActionResult Print(string id)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ViewData[SYSConstant.PARAM_ID] = id;
            UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string id)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            //UserMVC();
            var SD = DBV.HR_VIEW_EMPLOAN.FirstOrDefault(w => w.LoanID == id);
            if (SD != null)
            {
                try
                {
                    ViewData[SYSConstant.PARAM_ID] = id;
                    var sa = new RptEmpLoan();
                    var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREEN_ID
                   && w.IsActive == true).ToList();
                    if (objRpt.Count > 0)
                    {
                        sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                    }
                    sa.Parameters["LoanID"].Value = SD.LoanID;
                    sa.Parameters["LoanID"].Visible = false;

                    Session[Index_Sess_Obj + ActionName] = sa;
                    Session[Index_Sess_Obj] = sa;
                    return PartialView("PrintForm", sa);
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    log.DocurmentAction = id;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string id)
        {
            ActionName = "Print";

            if (Session[Index_Sess_Obj] != null)
            {
                RptEmpLoan reportModel = (RptEmpLoan)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion
        private void DataSelector()
        {
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();

            ViewData["STAFF_SELECT"] = Staff;// DB.HRStaffProfiles.ToList();
        }
    }
}