using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using Humica.Models.SY;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Users
{

    public class SYModifySalaryController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "SYMS000001";
        private const string URL_SCREEN = "/Administrator/Users/SYModifySalary/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "UserName";

        private SMSystemEntity DBA = new SMSystemEntity();
        private HumicaDBContext DB = new HumicaDBContext();


        public SYModifySalaryController()
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
            UserConfList(this.KeyName);
            DataSelector();
            SYUserAceessObject BSM = new SYUserAceessObject();
            BSM.HeaderSalary = new SYHRModifySalary();
            BSM.ListModiSalary = BSM.LoadDataModify();
            BSM.ListUserLevel = DBA.HRLevels.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            this.ActionName = "Index";
            SYUserAceessObject BSM = new SYUserAceessObject();
            UserSession();
            UserConfList(KeyName);
            UserConfFormFitler();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SYUserAceessObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListModiSalary);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            SYUserAceessObject SYObject = new SYUserAceessObject();
            SYObject.HeaderSalary = new SYHRModifySalary();

            SYObject.ListUserLevel = DBA.HRLevels.ToList();
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);
            Session[Index_Sess_Obj + ActionName] = SYObject;
            return View(SYObject);
        }
        [HttpPost]
        public ActionResult Create(SYUserAceessObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);
            SYUserAceessObject GLA = new SYUserAceessObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                GLA = (SYUserAceessObject)Session[Index_Sess_Obj + ActionName];
            }
            collection.ListUserLevel = DBA.HRLevels.ToList();

            if (Request.Form["LevelSelected"] != null)
            {
                GLA.LevelSelected = Request.Form["LevelSelected"].ToString();
            }
            GLA.HeaderSalary = collection.HeaderSalary;

            string msg = GLA.createUser();

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = GLA.HeaderSalary.UserName;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
                ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                collection.HeaderSalary = new SYHRModifySalary();
                return View(collection);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            return View(collection);
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
                SYUserAceessObject BSM = new SYUserAceessObject();
                BSM.HeaderSalary = DB.SYHRModifySalarys.FirstOrDefault(w => w.UserName == id);
                if (BSM.HeaderSalary != null)
                {
                    BSM.ListUserLevel = DBA.HRLevels.ToList();

                    BSM.ListLevelAssigned = BSM.DB.SYHRModifySalarys.Where(w => w.UserName == BSM.HeaderSalary.UserName).ToList();
                    foreach (var read in BSM.ListLevelAssigned)
                    {
                        BSM.LevelSelected += read.Level + ";";
                    }

                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(string id, SYUserAceessObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            SYUserAceessObject GLA = new SYUserAceessObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                GLA = (SYUserAceessObject)Session[Index_Sess_Obj + ActionName];
            }
            if (Request.Form["LevelSelected"] != null)
            {
                GLA.LevelSelected = Request.Form["LevelSelected"].ToString();
            }
            GLA.HeaderSalary = collection.HeaderSalary;

            string msg = GLA.editUser(id);

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = GLA.HeaderSalary.UserName;
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
        public ActionResult TreeLevelStorage()
        {
            ActionName = "Create";
            SYUserAceessObject SYObject = new SYUserAceessObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                SYObject = (SYUserAceessObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("TreeLevelStorage", SYObject);
        }
        #region "Delete"
        public ActionResult Delete(string id)
        {
            DataSelector();
            UserSession();
            SYUserAceessObject BSM = new SYUserAceessObject();

            string msg = BSM.deleteUser(id);
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("USR_DEL", user.Lang);
                mess.DocumentNumber = id;
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
        private void DataSelector()
        {
            ViewData["USER_SELECT"] = DBA.SYUsers.ToList().OrderBy(w => w.UserName);
            //ViewData["LEVEL_SELECT"] = DBA.HRLevels.ToList().OrderBy(w => w.Description);
        }
    }
}
