using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.MD;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Users
{
    public class UserAccessController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYUA0001";
        private const string URL_SCREEN = "/Administrator/Users/UserAccess/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "UserName;Branch;DeptCode";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity DS = new SMSystemEntity();

        public UserAccessController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm();
            SYUserAceessObject BSM = new SYUserAceessObject();
            BSM.ListHeader = DB.SYAccessDepartments.ToList();
            return View(BSM);
        }

        #region 'User Access'
        public ActionResult GridItems()
        {
            ActionName = "Index";
            DataSelector();
            UserConf(ActionBehavior.EDIT);

            SYUserAceessObject BSM = new SYUserAceessObject();
            BSM.ListHeader = DB.SYAccessDepartments.ToList();
            return PartialView("GridItems", BSM.ListHeader);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateUser(SYAccessDepartment MModel)
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            SYUserAceessObject BSM = new SYUserAceessObject();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.SYAccessDepartments.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListHeader = DB.SYAccessDepartments.ToList();
            return PartialView("GridItems", BSM.ListHeader);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditUser(SYAccessDepartment MModel)
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            SYUserAceessObject BSM = new SYUserAceessObject();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.SYAccessDepartments.FirstOrDefault(w => w.UserName == MModel.UserName);
                    ObjMatch.DeptCode = MModel.DeptCode;

                    DB.SYAccessDepartments.Attach(ObjMatch);
                    DB.Entry(ObjMatch).Property(x => x.DeptCode).IsModified = true;
                    DB.SaveChanges();
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
            BSM.ListHeader = DB.SYAccessDepartments.OrderBy(w => w.UserName).ToList();
            return PartialView("GridItems", BSM.ListHeader);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteUser(string UserName, string DeptCode)
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            SYUserAceessObject BSM = new SYUserAceessObject();
            if (UserName != null)
            {
                try
                {
                    var obj = DB.SYAccessDepartments.FirstOrDefault(w => w.UserName == UserName && w.DeptCode == DeptCode);
                    if (obj != null)
                    {
                        DB.SYAccessDepartments.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListHeader = DB.SYAccessDepartments.OrderBy(w => w.UserName).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListHeader = DB.SYAccessDepartments.OrderBy(w => w.UserName).ToList();
            return PartialView("GridItems", BSM.ListHeader);
        }
        #endregion 'User Access'
        private void DataSelector()
        {
            // ViewData["BRANCHES_SELECT"] = DB.HRBranches.ToList().OrderBy(w => w.Description);
            ViewData["USER_SELECT"] = DS.SYUsers.ToList().OrderBy(w => w.UserName);
            //ViewData["DEPARTMENT_SELECT"] = DB.HRDepartments.ToList().OrderBy(w => w.Description);
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
        }
    }
}
