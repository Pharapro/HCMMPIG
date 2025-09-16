using DevExpress.Data.WcfLinq.Helpers;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.Config.Setup
{

    public class SYChangeEmpHeadController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "SYHR000002";
        private const string URL_SCREEN = "/Config/Setup/SYChangeEmpHead/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ChangeInforType";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYChangeEmpHeadController()
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
            DataSelector();
            UserConfListAndForm(this.KeyName);
            MDChangeEmpInfor BSM = new MDChangeEmpInfor();
            BSM.ListEmp = new List<HRStaffProfile>();
            BSM.Filter = new FTFilterEmployee();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (MDChangeEmpInfor)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(MDChangeEmpInfor BSM)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            BSM.ListEmp = BSM.LoadDataEmpGen(BSM.Filter, SYConstant.getBranchDataAccess());
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        #endregion
        #region 'Assign'
        public ActionResult AssignStaff(string ChangeInforType, string FromCode, string ToCode)
        {
            ActionName = "Index";
            ClsRunScript BSM = new ClsRunScript();
            var BSM_ = new MDChangeEmpInfor();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM_ = (MDChangeEmpInfor)Session[Index_Sess_Obj + ActionName];
            }
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            try
            {

                //var ChkupToCode = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == ToCode);
                if (FromCode == "null") FromCode = null;
                if (ToCode == "null") ToCode = null;
                if (BSM_.EmpCode == null) BSM_.EmpCode = null;
                if (ChangeInforType == "null") ChangeInforType = null;
                if (ChangeInforType != null)// ChkupToCode != null && (BSM_.EmpCode != null || BSM_.EmpCode != "")
                {
                    string result = "", Code, Cmd;
                    string[] Emp = BSM_.EmpCode.Split(';');
                    foreach (var include in Emp)
                    {
                        Code = "'" + include + "'";
                        result += Code + ",";
                    }
                    result = result.Remove(result.Length - 1);

                    if (FromCode != null)
                        Cmd = "UPDATE HRStaffProfile SET " + ChangeInforType + " = '" + ToCode + "' WHERE EmpCode in (" + result + ") AND " + ChangeInforType + " = '" + FromCode + "';";
                    else
                        Cmd = "UPDATE HRStaffProfile SET " + ChangeInforType + " = '" + ToCode + "' WHERE EmpCode in (" + result + ") ;";

                    BSM.Script = Cmd;
                    string msg = BSM.RunScript(Cmd);

                    if (msg == SYConstant.OK)
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        Session[SYConstant.MESSAGE_SUBMIT] = mess;
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                }
                else
                {
                    SYMessages mess = SYMessages.getMessageObject("INVALTID_TPYE", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region 'GridItems'
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            MDChangeEmpInfor BSM = new MDChangeEmpInfor();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (MDChangeEmpInfor)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmp);
        }
        #endregion
        #region 'Code'
        public string getEmpCode(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            MDChangeEmpInfor BSM = new MDChangeEmpInfor();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (MDChangeEmpInfor)Session[Index_Sess_Obj + ActionName];

                BSM.EmpCode = EmpCode;

                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_ALLOWANCE");
            }
        }
        private void DataSelector()
        {
            string status = SYDocumentStatus.A.ToString();
            SYDataList objList = new SYDataList("CHANGE_EMP_HEAD");
            ViewData["INFORTYPE_SELECT"] = objList.ListData;

            var allCodes = DB.HRStaffProfiles
                .SelectMany(w => new List<string> { w.FirstLine, w.FirstLine2, w.SecondLine, w.SecondLine2, w.HODCode })
                .Where(code => !string.IsNullOrEmpty(code))
                .Distinct()
                .ToList();

            var staffFromQuery = DB.HRStaffProfiles
                .Where(w => allCodes.Contains(w.EmpCode))
                .Select(w => new { w.EmpCode, w.OthAllName, w.AllName })
                .ToList();

            ViewData["STAFFFROM_SELECT"] = staffFromQuery;
            var staffToQuery = DBV.HR_STAFF_VIEW.AsNoTracking()
                                .Where(w => w.StatusCode == status)
                                .Select(w => new { w.EmpCode,w.OthAllName, w.AllName, w.StatusCode })
                                .ToList();
            ViewData["STAFFTO_SELECT"] = staffToQuery;
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["LEVEL_SELECT"] =SYConstant.getLevelDataAccess();
            ViewData["BusinessUnit_SELECT"]= DB.HRGroupDepartments.ToList();
            ViewData["STAFF_SELECT"]= DB.HRStaffProfiles.Where(w => w.Status == "A").ToList();
            ViewData["APPRTYPE_SELECT"] = DB.HRApprTypes.ToList();
            ViewData["OFFICE_SELECT"] = DB.HROffices.ToList();
            ViewData["GROUP_SELECT"] = DB.HRGroups.ToList();
            ViewData["Category_SELECT"] = DB.HRCategories.ToList();
        }

        #endregion 
    }

}