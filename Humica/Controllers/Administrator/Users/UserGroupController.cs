using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Users
{
    public class UserGroupController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYU00002";
        private const string URL_SCREEN = "/Administrator/Users/UserGroup/";
        private const string VIEW_PART = "~/Views/Administrator/UserGroup/";
        private const string KEY_LIST = "TokenCode";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        SMSystemEntity DBA = new SMSystemEntity();
        HumicaDBContext DB = new HumicaDBContext();
        private string ActionName = "";
        public UserGroupController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            this.ViewPathRender = VIEW_PART;
        }
        #region "List"
        public ActionResult Index()
        {
            DataSelector();
            this.ActionName = "Index";
            ClsRollUser BSM = new ClsRollUser();
            UserSession();
            UserConfList(KEY_LIST);
            UserConfFormFitler();
            BSM.Filter = new FTFilterEmployee();
            BSM.UserObject = new SYUser();
            BSM.UserObject.UserType = UserType.N.ToString();
            if (BSM.UserObject.UserType == UserType.N.ToString())
            {
                BSM.ListTRCode = DBA.CFTRCodes.ToList();
            }
            BSM.ListHeaderRole = BSM.DB.SYRoles.Where(w => w.UserType == BSM.UserObject.UserType).ToList();
            BSM.ListHeaderRoleAPP = BSM.DB.SYRoleAPPs.ToList();
            BSM.ListStaff = new List<HRStaffProfile>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Index(ClsRollUser BSM)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            DateTime date = DateTime.Now;
            BSM.UserObject = new SYUser();
            BSM.UserObject.UserType = UserType.N.ToString();
            if (BSM.UserObject.UserType == UserType.N.ToString())
            {
                BSM.ListTRCode = DBA.CFTRCodes.ToList();
            }
            BSM.ListHeaderRole = BSM.DB.SYRoles.Where(w => w.UserType == BSM.UserObject.UserType).ToList();
            BSM.ListHeaderRoleAPP = BSM.DB.SYRoleAPPs.ToList();
            BSM.ListStaff = BSM.LoadDataEmp(BSM.Filter);
            
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }

        #endregion
        #region "Generate"
        public ActionResult Generate(string Branch,string Level, string WEB,string APP)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ClsRollUser BSM_ = new ClsRollUser();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM_ = (ClsRollUser)Session[Index_Sess_Obj + ActionName];
            }
            if (!string.IsNullOrEmpty(BSM_.EmpID))
            {
                BSM_.UserBusinessObject = new SYUserBusiness();
                BSM_.UserObject = new SYUser();
                BSM_.DataAccess = new SYUserDataAcess();
                BSM_.UserObject.IsActive = true;
                BSM_.ListTRCode = new List<CFTRCode>();
                BSM_.UserObject.UserType = UserType.N.ToString();
                if (!string.IsNullOrEmpty(WEB)) BSM_.RoleSelected = WEB;
                if (!string.IsNullOrEmpty(APP)) BSM_.RoleSelectedAPP = APP;
                if (!string.IsNullOrEmpty(Branch))
                {
                    string[] Branch_ = Branch.Split(',');
                    foreach (var item in Branch_)
                    {
                        var obj = DBA.HRBranches.FirstOrDefault(w => w.Code == item);
                        if (obj != null)
                            BSM_.StorageSelected += obj.ID + ";";
                    }
                }
                if (!string.IsNullOrEmpty(Level))
                {
                    string[] Level_ = Level.Split(',');
                    foreach (var item in Level_)
                    {
                        if (item == "null") continue;
                        BSM_.LevelSelected += item + ";";
                    }
                }
                if (BSM_.UserObject.UserType == UserType.N.ToString())
                {
                    BSM_.ListTRCode = DBA.CFTRCodes.ToList();
                }
                BSM_.UserObject.ExpireDate = new DateTime(9999, 1, 1);
                BSM_.UserObject.CompanyOwner = DBA.HRCompanies.FirstOrDefault().Company;
                string[] EmpCode = BSM_.EmpID.Split(';');
                string msg = "";
                foreach (var ID in EmpCode)
                {
                    var staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == ID);
                    if (string.IsNullOrEmpty(BSM_.StorageSelected))
                    {
                        var obj = DBA.HRBranches.FirstOrDefault(w => w.Code == staff.Branch && w.CompanyCode == staff.CompanyCode);
                        if (obj != null)
                            BSM_.StorageSelected += obj.ID + ";";
                    }
                    if (string.IsNullOrEmpty(BSM_.LevelSelected))
                        BSM_.LevelSelected += staff.LevelCode;
                    BSM_.UserName = ID;
                    BSM_.UserObject.Password = "1234";
                    BSM_.UserObject.UserName = ID;
                    BSM_.UserObject.LoginName = staff.AllName;
                    var msg_ = BSM_.createUser();
                    msg = msg_;
                }
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("PLS_SELECT_EMP", user.Lang);
            }
            Session[Index_Sess_Obj + ActionName] = BSM_;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            ClsRollUser BSM = new ClsRollUser();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRollUser)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListStaff);
        }
        [HttpPost]
        public string getEmpCode(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            ClsRollUser BSM = new ClsRollUser();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRollUser)Session[Index_Sess_Obj + ActionName];

                BSM.EmpID = EmpCode;

                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_ALLOWANCE");
            }
        }
        public ActionResult TreeRole()
        {
            ActionName = "Index";
            ClsRollUser SYObject = new ClsRollUser();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                SYObject = (ClsRollUser)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("TreeRole", SYObject);
        }
        public ActionResult TreeRoleAPP()
        {
            ActionName = "Index";
            ClsRollUser SYObject = new ClsRollUser();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                SYObject = (ClsRollUser)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("TreeRoleAPP", SYObject);
        }
        #region "Private Code"
        private void DataSelector()
        {
            var _listBranch = SYConstant.getBranchDataAccess();
            ViewData["BRANCHES_SELECT"] = _listBranch.ToList();
            var Level = SYConstant.getLevelDataAccess();
            ViewData["LEVEL_SELECT"] = Level.ToList();
            ViewData["DEPARTMENT_SELECT"] = DB.HRDepartments.ToList();
            ViewData["POSITION_SELECT"] = DB.HRPositions.ToList();
        }

        #endregion

    }
}
