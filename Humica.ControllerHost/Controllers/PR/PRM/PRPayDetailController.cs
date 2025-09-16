using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Payroll;
using System.Web.Mvc;

namespace HR.Controllers.PR.PRM
{
    public class PRPayDetailController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000007";
        private const string URL_SCREEN = "/PR/PRM/PRPayDetail/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "ID";
        IClsPRPayroll BSM;
        public PRPayDetailController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsPRPayroll();
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            BSM.OnIndexLoadingDetail();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        //[HttpPost]
        //public ActionResult Index(ClsPRPayroll BSM)
        //{
        //    DataSelector();
        //    ActionName = "Index";
        //    UserSession();
        //    UserConfListAndForm(this.KeyName);
        //    BSM.ListEmployeeGen = BSM.LoadDataEmpGen(BSM.Filter, SYConstant.getBranchDataAccess());

        //    Session[Index_Sess_Obj + ActionName] = BSM;

        //    return View(BSM);
        //}
        #endregion

        public ActionResult Delete(string EmpCode, int Period)
        {
            UserSession();
            UserConfForm(SYActionBehavior.DELETE);
            DataSelector();
            if (EmpCode != null)
            {
                string msg = BSM.Delete_PayRecord(EmpCode, Period);
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
        public ActionResult _BasicSalary()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_BasicSalary", BSM.ListBasicSalary);
        }

        public ActionResult _Allowance()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_Allowance", BSM.ListAllowance);
        }
        public ActionResult _Bonus()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_Bonus", BSM.ListBonus);
        }
        public ActionResult _OverTime()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_OverTime", BSM.ListOverTime);
        }
        public ActionResult _Deduction()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_Deduction", BSM.ListDeduction);
        }
        public ActionResult _LeaveDed()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_LeaveDed", BSM.ListLeaveDed);
        }
        public ActionResult GridSoSe()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridSoSe", BSM.ListEmpSoSe);
        }
        public ActionResult _GLCharge()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_GLCharge", BSM.ListGLCharge);
        }
        //public ActionResult _CostCharge()
        //{
        //    ActionName = "Index";
        //    UserSession();
        //    DataSelector();
        //    UserConfForm(ActionBehavior.ACC_REV);
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
        //    }
        //    Session[Index_Sess_Obj + ActionName] = BSM;
        //    return PartialView("_CostCharge", BSM.ListCostCharge);
        //}
        //public ActionResult _PaySlip()
        //{
        //    ActionName = "Index";
        //    UserSession();
        //    DataSelector();
        //    UserConfForm(ActionBehavior.ACC_REV);
        //    IClsPRPayroll BSM = new IClsPRPayroll();
        //    BSM.ScreenId = SCREEN_ID;
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
        //    }
        //    Session[Index_Sess_Obj + ActionName] = BSM;
        //    Session[SYConstant.CURRENT_URL] = URL_SCREEN + "_PaySlip";
        //    return PartialView("_PaySlip", BSM.ListPaySlip);
        //}
        //public ActionResult DataBindingPartial(bool showServiceColumns = false)
        public ActionResult DataBindingPartial()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            //ViewBag.ShowServiceColumns = showServiceColumns;
            return PartialView("PartialTree", BSM.ListEmpPayment);
        }
        public ActionResult ShowData(string EmpCode, int Period)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
            }
            BSM.GetDataDetail(EmpCode, Period);
            if (BSM.STAFF_VIEW != null)
            {
                var EmpStaff = BSM.STAFF_VIEW;
                var GenSalary = BSM.HeaderSalary;
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmpType,
                    Division = EmpStaff.DivisionDesc,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.Level,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate,
                    Salary = GenSalary.Salary,
                    Spouse = GenSalary.UTAXSP,
                    Child = GenSalary.UTAXCH,
                    Tax = GenSalary.TAXAM,
                    AmToBeTax = GenSalary.AMTOBETAX,
                    FTax = GenSalary.AMFRINGTAX,
                    AmToBeFTax = GenSalary.FRINGAM,
                    GrossPay = GenSalary.GrossPay,
                    NetPay = GenSalary.NetWage,
                    EmpPensionFunRate = GenSalary.StaffPensionFundRate,
                    EmpPensionFunAmount = GenSalary.StaffPensionFundAmount,
                    ComPensionFunRate = GenSalary.CompanyPensionFundRate,
                    ComPensionFunAmount = GenSalary.CompanyPensionFundAmount,
                    SeniorityTaxable = GenSalary.SeniorityTaxable,
                    FirstPaymentAmount = GenSalary.FirstPaymentAmount
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelector())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}
