using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Models.SY;
using Humica.Performance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HRAppPlanIncSalaryController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRA0000012";
        private const string URL_SCREEN = "/HRM/Appraisal/HRAppPlanIncSalary/";
        private string ActionName;
        private string KeyName = "ID";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        HumicaDBContext DB = new HumicaDBContext();
        private string PARAM_INDEX = "ID;";

        public HRAppPlanIncSalaryController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region List
        public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            await DataSelector();
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            BSM.Filter = new FTFilterEmployee();
            BSM.ListMatrixIncrease = new List<HRAPPMatrixIncreaseSalary>();
            BSM.TotalSalary = 0;
            BSM.SalaryInBgP = 0;
            BSM.SalaryIncBgUSD = 0;
            BSM.SalaryIncBgUtilised = 0;
            BSM.SalaryIncBgBalance = 0;
            BSM.Filter.INYear = DateTime.Now.Year;
            Session[Index_Sess_Obj + ActionName] = BSM;
            ViewData[ClsConstant.PARAM_INDEX] = PARAM_INDEX;
            return View(BSM);
        }
        [HttpPost]
        public async Task<ActionResult> Index(ClsAPPIncreaseSalary BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            await DataSelector();
            BSM.ListMatrixIncrease = await BSM.LoadData(BSM.Filter, SYConstant.getBranchDataAccess());
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion
        #region "Save"
        public async Task<ActionResult> Save()
        {
            ActionName = "Index";
            UserSession();
            await DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
            }
            if (BSM.ListMatrixIncrease.Count > 0)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CareerMatrix();

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region Matrix
        public ActionResult GridMatix()
        {
            ActionName = "Index";
            UserConf(ActionBehavior.EDIT);
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            BSM = (ClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
            BSM.ListMatrixIncrease.ToList();
            return PartialView("GidMatixIncrease", BSM.ListMatrixIncrease);
        }
        public async Task<ActionResult> ItemEdit(HRAPPMatrixIncreaseSalary MModel)
        {
            ActionName = "Index";
            UserSession();
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
                var msg = await BSM.EditMatrix(MModel);
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                else
                    ViewData["EditError"] = SYMessages.getMessage(msg) + BSM.MessageError;
            }
            else
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            return PartialView("GidMatixIncrease", BSM.ListMatrixIncrease);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ItemDelete(string EmpCode, int Inyear)
        {
            ActionName = "Index";
            UserSession();
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            if (EmpCode != null)
            {
                BSM = (ClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
                try
                {
                    var list = DB.HRAPPMatrixIncreaseSalaries.FirstOrDefault(w => w.EmpCode == EmpCode && w.InYear == Inyear);
                    if (list != null)
                    {
                        DB.HRAPPMatrixIncreaseSalaries.Remove(list);
                        int row = DB.SaveChanges();
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        var obj = BSM.ListMatrixIncrease.FirstOrDefault(w => w.EmpCode == EmpCode);
                        if (obj != null)
                        {
                            BSM.ListMatrixIncrease.Remove(obj);
                            Session[Index_Sess_Obj + ActionName] = BSM;
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GidMatixIncrease", BSM.ListMatrixIncrease);
        }
        #endregion
        public async Task<ActionResult> Refreshvalue()
        {
            ActionName = "Index";
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
                //await BSM.Refreshvalue(BSM.ListMatrixIncrease, BSM.Filter.INYear);
                var result = new
                {
                    MS = SYConstant.OK,
                    TotalSalary = BSM.TotalSalary,
                    SalaryInBgP = BSM.SalaryInBgP,
                    SalaryIncBgUSD = BSM.SalaryIncBgUSD,
                    SalaryIncBgUtilised = BSM.SalaryIncBgUtilised,
                    SalaryIncBgBalance = BSM.SalaryIncBgBalance,
                };
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private async Task DataSelector()
        {
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
            ViewData["BusinessUnit_SELECT"] = await DB.HRGroupDepartments.ToListAsync();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
        }
    }
}