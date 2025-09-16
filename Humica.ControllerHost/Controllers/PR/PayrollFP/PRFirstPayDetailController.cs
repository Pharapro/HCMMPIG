using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Payroll;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{
    public class PRFirstPayDetailController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "PRM0000020";
        private const string URL_SCREEN = "/PR/PRM/PRFirstPayDetail/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "ID";
        IClsPRFirstPaySalary BSM;
        public PRFirstPayDetailController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsPRFirstPaySalary();
            BSM.OnLoad();
        }


        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = obj.Filter;
            }
            BSM.OnIndexLoadingDetail();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsPRFirstPaySalary BSM)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            //BSM.ListEmployeeGen = BSM.LoadDataEmpGen(BSM.Filter, SYConstant.getBranchDataAccess());

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion

        public ActionResult Delete(string EmpCode, int Period)
        {
            UserSession();
            UserConfForm(SYActionBehavior.DELETE);
            DataSelector();
            if (EmpCode != null)
            {
                string msg = BSM.DeleteGenerate(EmpCode, Period);
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
                BSM = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
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
                BSM = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            //Session[SYConstant.CURRENT_URL] = URL_SCREEN + "_Allowance";
            return PartialView("_Allowance", BSM.ListEmpAllowanceFP);
        }
        public ActionResult _Bonus()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            //Session[SYConstant.CURRENT_URL] = URL_SCREEN + "_Bonus";
            return PartialView("_Bonus", BSM.ListEmpBonusFP);
        }
        public ActionResult _OverTime()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            //Session[SYConstant.CURRENT_URL] = URL_SCREEN + "_OverTime";
            return PartialView("_OverTime", BSM.ListEmpOTFP);
        }
        public ActionResult _Deduction()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            //Session[SYConstant.CURRENT_URL] = URL_SCREEN + "_Deduction";
            return PartialView("_Deduction", BSM.ListDeductionFP);
        }
        public ActionResult _LeaveDed()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            //Session[SYConstant.CURRENT_URL] = URL_SCREEN + "_LeaveDed";
            return PartialView("_LeaveDed", BSM.ListLeaveDed);
        }
        public ActionResult _PaySlip()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            //Session[SYConstant.CURRENT_URL] = URL_SCREEN + "_PaySlip";
            return PartialView("_PaySlip", BSM.ListPaySlip);
        }
        public ActionResult ShowData(string EmpCode, int Period)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRFirstPaySalary)Session[Index_Sess_Obj + ActionName];
            }
            BSM.GetDataDetailFP(EmpCode, Period);
            if (BSM.STAFF_VIEW != null)
            {
                var EmpStaff = BSM.STAFF_VIEW;
                if (BSM.HeaderSalaryFP != null)
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
                        StartDate = EmpStaff.StartDate,
                        Salary = BSM.HeaderSalaryFP.Salary,
                        Spouse = BSM.HeaderSalaryFP.UTAXSP,
                        Child = BSM.HeaderSalaryFP.UTAXCH,
                        AmToBeTax = BSM.HeaderSalaryFP.AMTOBETAX,
                        GrossPay = BSM.HeaderSalaryFP.GrossPay,
                        NetPay = BSM.HeaderSalaryFP.NetWage,
                    };
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return Json(result, JsonRequestBehavior.DenyGet);
                }
                else
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
                        StartDate = EmpStaff.StartDate,
                        Salary = 0,
                        Spouse = 0,
                        Child = 0,
                        Tax = 0,
                        AmToBeTax = 0,
                        FTax = 0,
                        AmToBeFTax = 0,
                        GrossPay = 0,
                        NetPay = 0
                    };
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return Json(result, JsonRequestBehavior.DenyGet);
                }
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelectorDetailFP())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}
