using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Users
{

    public class SYUserMobileController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "SYMS000003";
        private const string URL_SCREEN = "/Administrator/Users/SYUserMobile/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "UserID";

        Humica.Core.DB.HumicaDBContext DB = new Humica.Core.DB.HumicaDBContext();
        private SMSystemEntity DBA = new SMSystemEntity();

        public SYUserMobileController()
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
            UserConfListAndForm(this.KeyName);
            DataList();
            SYUserTokenObject BSM = new SYUserTokenObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (SYUserTokenObject)Session[Index_Sess_Obj + ActionName];
            }
            var ListHader = DB.TokenResources;
            BSM.ListHeader = ListHader.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        #endregion
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            DataList();
            SYUserTokenObject BSM = new SYUserTokenObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SYUserTokenObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListHeader);
        }
        public ActionResult GridItemsEdit(TokenResource MModel)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataList();
            SYUserTokenObject BSM = new SYUserTokenObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (SYUserTokenObject)Session[Index_Sess_Obj + ActionName];
                    }
                    //var DBU = new SMSystemEntity();
                    var ListToken = BSM.ListHeader.Where(w => w.MUserID == MModel.MUserID).ToList();
                    if (ListToken.Count > 0)
                    {
                        var objUpdate = ListToken.First();
                        objUpdate.IsLock = MModel.IsLock;
                        DB.TokenResources.Attach(objUpdate);
                        DB.Entry(objUpdate).Property(w => w.IsLock).IsModified = true;
                        var row = DB.SaveChanges();
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
            return PartialView("GridItems", BSM.ListHeader);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridItemsDelete(int MUserID)
        {
            UserSession();
            DataList();
            SYUserTokenObject BSM = new SYUserTokenObject();
            if (MUserID != null)
            {
                try
                {
                    var obj = DB.TokenResources.Where(w=>w.MUserID==MUserID).FirstOrDefault();
                    if (obj != null)
                    {
                        DB.TokenResources.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListHeader = DB.TokenResources.OrderBy(w => w.UserName).ToList();
                    
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridItems", BSM.ListHeader);
        }

        #region "Private Code"
        private void DataList()
        {
            var listUser = DBA.SYUsers.ToList();
            ViewData["USER_LIST"] = listUser;
        }
        #endregion
    }
}
