using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Users
{
    public class UserController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYU00001";
        private const string URL_SCREEN = "/Administrator/Users/User/";
        private const string VIEW_PART = "~/Views/Administrator/User/";
        private SMSystemEntity DBA = new SMSystemEntity();
        private const string KEY_LIST = "TokenCode";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;

        private string ActionName = "";
        public UserController()
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
            SYUserObject BSM = new SYUserObject();
            UserSession();
            UserConfList(KEY_LIST);
            UserConfFormFitler();
            BSM.ListHeader = DBA.SYUsers.OrderByDescending(w => w.CreatedOn).ToList();


            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Index(SYUserObject BSM)
        {
            DataSelector();
            this.ActionName = "Index";
            UserSession();
            UserConfList(KEY_LIST);
            UserConfFormFitler();
            BSM.ListHeader = DBA.SYUsers.Where(w => w.UserType == BSM.HeaderRole.UserType).OrderByDescending(w => w.CreatedOn).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        public ActionResult PartialList()
        {
            this.ActionName = "Index";
            SYUserObject BSM = new SYUserObject();
            UserSession();
            UserConfList(KEY_LIST);
            UserConfFormFitler();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SYUserObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        #region"Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                SYUserObject BSM = new SYUserObject();
                BSM.UserObject = BSM.getUserByToken(id);

                BSM.ListTRCode = new List<CFTRCode>();


                if (BSM.UserObject.UserType == UserType.N.ToString())
                {
                    BSM.ListTRCode = DBA.CFTRCodes.ToList();
                }

                BSM.ListStorage = DBA.HRBranches.ToList();
                BSM.ListUserLevel = DBA.HRLevels.ToList();


                BSM.UserBusinessObject = BSM.DB.SYUserBusinesses.Where(w => w.UserId == BSM.UserObject.UserID).FirstOrDefault();

                BSM.ListRoleAssigned = BSM.DB.SYUserRoles.Where(w => w.UserID == BSM.UserObject.UserID).ToList();
                foreach (var read in BSM.ListRoleAssigned)
                {
                    BSM.RoleSelected += read.RoleId + ";";
                }
                BSM.ListRoleAPPAssigned = BSM.DB.SYUserRoleAPPs.Where(w => w.UserID == BSM.UserObject.UserID).ToList();
                foreach (var read in BSM.ListRoleAPPAssigned)
                {
                    BSM.RoleSelectedAPP += read.RoleId + ";";
                }
                BSM.ListDataAccessAssigned = BSM.DB.SYUserDataAcesses.Where(w => w.UserId == BSM.UserObject.UserID).ToList();
                foreach (var read in BSM.ListDataAccessAssigned)
                {
                    BSM.StorageSelected += read.CompanyCode + ";";
                }
                BSM.ListLevelAssigned = BSM.DB.SYUserLevels.Where(w => w.UserName == BSM.UserObject.UserName).ToList();
                foreach (var read in BSM.ListLevelAssigned)
                {
                    BSM.LevelDataSelected += read.LevelCode + ";";
                }

                BSM.ListHeaderRole = BSM.DB.SYRoles.Where(w => w.UserType == BSM.UserObject.UserType).ToList();
                BSM.ListHeaderRoleAPP = BSM.DB.SYRoleAPPs.ToList();

                Session[Index_Sess_Obj + ActionName] = BSM;

                if (BSM.UserObject != null)
                {
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(string id, SYUserObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            SYUserObject GLA = new SYUserObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                GLA = (SYUserObject)Session[Index_Sess_Obj + ActionName];
            }
            collection.ListHeaderRole = GLA.DB.SYRoles.ToList();
            collection.ListHeaderRoleAPP = GLA.DB.SYRoleAPPs.ToList();

            collection.ListTRCode = new List<CFTRCode>();

            if (collection.UserObject.UserType == UserType.N.ToString())
            {
                collection.ListTRCode = DBA.CFTRCodes.ToList();
            }

            //if (ModelState.IsValid)
            //{
            // TODO: Add insert logic here

            string repwd = Request.Form["RePassword"].ToString();

            if (repwd != "")
            {
                if (repwd != collection.UserObject.Password)
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("PWD_NMT", user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + id);
                }
            }

            if (Request.Form["RoleSelected"] != null)
            {
                GLA.RoleSelected = Request.Form["RoleSelected"].ToString();
            }
            if (Request.Form["RoleSelectedAPP"] != null)
            {
                GLA.RoleSelectedAPP = Request.Form["RoleSelectedAPP"].ToString();
            }
            if (Request.Form["DataSelected"] != null)
            {
                GLA.DataSelected = Request.Form["DataSelected"].ToString();
            }
            if (Request.Form["StorageSelected"] != null)
            {
                GLA.StorageSelected = Request.Form["StorageSelected"].ToString();
            }
            if (Request.Form["LevelSelected"] != null)
            {
                GLA.LevelSelected = Request.Form["LevelSelected"].ToString();
            }
            GLA.UserObject = collection.UserObject;
            GLA.ListHeaderRole = GLA.DB.SYRoles.ToList();
            GLA.ListHeaderRoleAPP = GLA.DB.SYRoleAPPs.ToList();
            GLA.UserBusinessObject = collection.UserBusinessObject;

            string msg = GLA.editUser(id);

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = GLA.UserObject.TokenCode;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + id);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            SYUserObject SYObject = new SYUserObject();
            SYObject.UserBusinessObject = new SYUserBusiness();
            SYObject.UserObject = new SYUser();
            SYObject.DataAccess = new SYUserDataAcess();
            SYObject.UserBusinessObject.BaseCurrency = "USD";
            SYObject.UserBusinessObject.DecimalPlace = 2;
            SYObject.UserBusinessObject.DecimalSepartor = ".";
            var comany = DBA.HRCompanies.FirstOrDefault();
            if (comany != null)
                SYObject.UserObject.CompanyOwner = comany.Company;
            SYObject.UserObject.UserType = UserType.N.ToString();

            SYObject.UserObject.IsActive = true;
            SYObject.ListTRCode = new List<CFTRCode>();

            if (SYObject.UserObject.UserType == UserType.N.ToString())
            {
                SYObject.ListTRCode = DBA.CFTRCodes.ToList();
            }
            SYObject.ListUserLevel = DBA.HRLevels.ToList();
            SYObject.ListStorage = DBA.HRBranches.ToList();
            SYObject.ListHeaderRole = SYObject.DB.SYRoles.Where(w => w.UserType == SYObject.UserObject.UserType).ToList();
            SYObject.ListHeaderRoleAPP = SYObject.DB.SYRoleAPPs.ToList();
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);
            Session[Index_Sess_Obj + ActionName] = SYObject;
            return View(SYObject);
        }

        public ActionResult TreeRole()
        {
            ActionName = "Create";
            SYUserObject SYObject = new SYUserObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                SYObject = (SYUserObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("TreeRole", SYObject);
        }
        public ActionResult TreeRoleAPP()
        {
            ActionName = "Create";
            SYUserObject SYObject = new SYUserObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                SYObject = (SYUserObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("TreeRoleAPP", SYObject);
        }
        public ActionResult TreeStorage()
        {
            ActionName = "Create";
            SYUserObject SYObject = new SYUserObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                SYObject = (SYUserObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("TreeStorage", SYObject);
        }
        public ActionResult TreeLevelStorage()
        {
            ActionName = "Create";
            SYUserObject SYObject = new SYUserObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                SYObject = (SYUserObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("TreeLevelStorage", SYObject);
        }
        public ActionResult TreeData()
        {
            ActionName = "Create";
            SYUserObject SYObject = new SYUserObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                SYObject = (SYUserObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("TreeData", SYObject);
        }

        public ActionResult ChangeRoleByType(string UserType)
        {
            ActionName = "Create";
            SYUserObject SYObject = new SYUserObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                SYObject = (SYUserObject)Session[Index_Sess_Obj + ActionName];
                SYObject.ListHeaderRole = SYObject.DB.SYRoles.Where(w => w.UserType == UserType).ToList();
            }
            Session[Index_Sess_Obj + ActionName] = SYObject;
            return PartialView("TreeRole", SYObject);
        }

        public ActionResult RoleList()
        {
            SYUserObject SYObject = new SYUserObject();
            SYObject.ListHeaderRole = SYObject.DB.SYRoles.ToList();
            return PartialView("RoleList", SYObject.ListHeaderRole);
        }

        [HttpPost]
        public ActionResult Create(SYUserObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);
            SYUserObject GLA = new SYUserObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                GLA = (SYUserObject)Session[Index_Sess_Obj + ActionName];
            }
            collection.ListHeaderRole = GLA.DB.SYRoles.ToList();
            collection.ListHeaderRoleAPP = GLA.DB.SYRoleAPPs.ToList();

            collection.UserObject.IsActive = true;
            collection.ListTRCode = new List<CFTRCode>();
            collection.UserObject.UserType = UserType.N.ToString();

            if (collection.UserObject.UserType == UserType.N.ToString())
            {
                collection.ListTRCode = DBA.CFTRCodes.ToList();
            }
            collection.ListStorage = DBA.HRBranches.ToList();
            collection.ListUserLevel = DBA.HRLevels.ToList();

            // TODO: Add insert logic here
            if (Request.Form["RoleSelected"] != null)
            {
                GLA.RoleSelected = Request.Form["RoleSelected"].ToString();
            }
            if (Request.Form["RoleSelectedAPP"] != null)
            {
                GLA.RoleSelectedAPP = Request.Form["RoleSelectedAPP"].ToString();
            }
            if (Request.Form["DataSelected"] != null)
            {
                GLA.DataSelected = Request.Form["DataSelected"].ToString();
            }
            if (Request.Form["StorageSelected"] != null)
            {
                GLA.StorageSelected = Request.Form["StorageSelected"].ToString();
            }
            if (Request.Form["LevelSelected"] != null)
            {
                GLA.LevelSelected = Request.Form["LevelSelected"].ToString();
            }
            GLA.UserObject = collection.UserObject;
            GLA.ListHeaderRole = GLA.DB.SYRoles.ToList();
            GLA.ListHeaderRoleAPP = GLA.DB.SYRoleAPPs.ToList();
            GLA.UserBusinessObject = collection.UserBusinessObject;
            //GLA.CurrencyObject.CompanyCode = bs.CompanyCode;
            if (Request.Form["RePassword"] != null)
            {
                if (Request.Form["RePassword"].ToString() == "")
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("PWD_NMT", user.Lang);
                    return View(collection);
                }
            }
            string repwd = Request.Form["RePassword"].ToString();
            if (!collection.UserObject.Password.Equals(repwd, StringComparison.Ordinal))
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("PWD_NMT", user.Lang);
                return View(collection);
            }

            string msg = GLA.createUser();

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = GLA.UserObject.TokenCode;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
                ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                collection.UserObject = new SYUser();
                return View(collection);
            }
            else
            {
                collection.ListHeaderRole = collection.DB.SYRoles.Where(w => w.UserType == collection.UserObject.UserType).ToList();
                collection.ListHeaderRoleAPP = collection.DB.SYRoleAPPs.ToList();
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(collection);
            }

            collection.ListHeaderRole = collection.DB.SYRoles.Where(w => w.UserType == collection.UserObject.UserType).ToList();
            return View(collection);

        }
        #endregion
        #region "Delete"
        public ActionResult Delete(string id)
        {
            DataSelector();
            UserSession();
            SYUserObject BSM = new SYUserObject();

            string msg = BSM.deleteUser(id);
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("USR_DEL", user.Lang);
                mess.DocumentNumber = BSM.UserObject.UserName;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                SYMessages mess = SYMessages.getMessageObject(msg, user.Lang);
                mess.Description = mess.Description;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
        }
        #endregion
        #region "Reset Password"
        public ActionResult ResetPwd(string id)
        {
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                SYUserObject BSM = new SYUserObject();
                BSM.UserObject = BSM.getUserByToken(id);
                Session[Index_Sess_Obj + ActionName] = BSM;

                if (BSM.UserObject != null)
                {
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult ResetPwd(SYUserObject BSM, string id)
        {
            UserSession();
            UserConfListAndForm();
            if (Request.Form["RePassword"] != null)
            {
                if (Request.Form["RePassword"].ToString() == "")
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("PWD_NMT", user.Lang);
                    return View(BSM);
                }
            }
            string repwd = Request.Form["RePassword"].ToString();
            if (repwd != BSM.UserObject.Password)
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("PWD_NMT", user.Lang);
                return View(BSM);
            }
            BSM.UserObject = BSM.getUserByToken(id);
            string msg = BSM.changePwd(BSM.UserObject.UserID, repwd);

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSM.User.TokenCode;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "ResetPwd?id=" + mess.DocumentNumber;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "ResetPwd?id=" + id);
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "ResetPwd?id=" + id);
            }


        }
        #endregion
        #region Copy
        public ActionResult Copy(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                SYUserObject BSM = new SYUserObject();
                BSM.UserObject = BSM.getUserByToken(id);
                string Username = BSM.UserObject.UserName;
                BSM.UserObject.UserName = "";
                BSM.UserObject.Email = "";
                BSM.ListTRCode = new List<CFTRCode>();
                if (BSM.UserObject.UserType == UserType.N.ToString())
                {
                    BSM.ListTRCode = DBA.CFTRCodes.ToList();
                }

                BSM.ListStorage = DBA.HRBranches.ToList();
                BSM.ListUserLevel = DBA.HRLevels.ToList();

                BSM.UserBusinessObject = BSM.DB.SYUserBusinesses.Where(w => w.UserId == BSM.UserObject.UserID).FirstOrDefault();

                BSM.ListRoleAssigned = BSM.DB.SYUserRoles.Where(w => w.UserID == BSM.UserObject.UserID).ToList();
                foreach (var read in BSM.ListRoleAssigned)
                {
                    BSM.RoleSelected += read.RoleId + ";";
                }
                BSM.ListRoleAPPAssigned = BSM.DB.SYUserRoleAPPs.Where(w => w.UserID == BSM.UserObject.UserID).ToList();
                foreach (var read in BSM.ListRoleAPPAssigned)
                {
                    BSM.RoleSelectedAPP += read.RoleId + ";";
                }
                BSM.ListDataAccessAssigned = BSM.DB.SYUserDataAcesses.Where(w => w.UserId == BSM.UserObject.UserID).ToList();
                foreach (var read in BSM.ListDataAccessAssigned)
                {
                    BSM.StorageSelected += read.CompanyCode + ";";
                }
                BSM.ListLevelAssigned = BSM.DB.SYUserLevels.Where(w => w.UserName == Username).ToList();
                foreach (var read in BSM.ListLevelAssigned)
                {
                    BSM.LevelSelected += read.LevelCode + ";";
                }

                BSM.ListHeaderRole = BSM.DB.SYRoles.Where(w => w.UserType == BSM.UserObject.UserType).ToList();
                BSM.ListHeaderRoleAPP = BSM.DB.SYRoleAPPs.ToList();

                Session[Index_Sess_Obj + ActionName] = BSM;

                if (BSM.UserObject != null)
                {
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Copy(SYUserObject collection, string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);
            SYUserObject GLA = new SYUserObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                GLA = (SYUserObject)Session[Index_Sess_Obj + ActionName];
            }
            collection.ListHeaderRole = GLA.DB.SYRoles.ToList();
            collection.ListHeaderRoleAPP = GLA.DB.SYRoleAPPs.ToList();

            collection.UserObject.IsActive = true;
            collection.ListTRCode = new List<CFTRCode>();
            collection.UserObject.UserType = UserType.N.ToString();

            if (collection.UserObject.UserType == UserType.N.ToString())
            {
                collection.ListTRCode = DBA.CFTRCodes.ToList();
            }
            collection.ListStorage = DBA.HRBranches.ToList();
            collection.ListUserLevel = DBA.HRLevels.ToList();

            // TODO: Add insert logic here
            if (Request.Form["RoleSelected"] != null)
            {
                GLA.RoleSelected = Request.Form["RoleSelected"].ToString();
            }
            if (Request.Form["RoleSelectedAPP"] != null)
            {
                GLA.RoleSelectedAPP = Request.Form["RoleSelectedAPP"].ToString();
            }
            if (Request.Form["DataSelected"] != null)
            {
                GLA.DataSelected = Request.Form["DataSelected"].ToString();
            }
            if (Request.Form["StorageSelected"] != null)
            {
                GLA.StorageSelected = Request.Form["StorageSelected"].ToString();
            }
            if (Request.Form["LevelSelected"] != null)
            {
                GLA.LevelSelected = Request.Form["LevelSelected"].ToString();
            }
            GLA.UserObject = collection.UserObject;
            GLA.ListHeaderRole = GLA.DB.SYRoles.ToList();
            GLA.ListHeaderRoleAPP = GLA.DB.SYRoleAPPs.ToList();
            GLA.UserBusinessObject = collection.UserBusinessObject;
            //GLA.CurrencyObject.CompanyCode = bs.CompanyCode;
            if (Request.Form["RePassword"] != null && Request.Form["RePassword"].ToString() == "")
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("PWD_NMT", user.Lang);
                return View(collection);
            }
            string repwd = Request.Form["RePassword"].ToString();
            if (!collection.UserObject.Password.Equals(repwd, StringComparison.Ordinal))
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("PWD_NMT", user.Lang);
                return View(collection);
            }
            string msg = GLA.createUser();

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = GLA.UserObject.TokenCode;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
                ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                collection.UserObject = new SYUser();
                return View(collection);
            }
            else
            {
                collection.ListHeaderRole = collection.DB.SYRoles.Where(w => w.UserType == collection.UserObject.UserType).ToList();
                collection.ListHeaderRoleAPP = collection.DB.SYRoleAPPs.ToList();
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(collection);
            }

            collection.ListHeaderRole = collection.DB.SYRoles.Where(w => w.UserType == collection.UserObject.UserType).ToList();
            return View(collection);
        }
        #endregion
        #region "Private Code"
        private void DataSelector()
        {
            SYDataList DL = new SYDataList("USER_TYPE");
            ViewData["USER_TYPE"] = DL.ListData;

            DL = new SYDataList("DECIMAL_SEPARETOR");
            ViewData["DECIMAL_SEPARETOR"] = DL.ListData;

            DL = new SYDataList("BASE_CURRENCY");
            ViewData["BASE_CURRENCY"] = DL.ListData;
            ViewData["DLR_LIST"] = DBA.SYHRCompanies.ToList();
        }

        #endregion

    }
}
