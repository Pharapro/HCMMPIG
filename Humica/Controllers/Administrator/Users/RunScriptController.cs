using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using System.Web.Mvc;
namespace Humica.Controllers.Administrator.Users
{

    public class RunScriptController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "SYMS000002";
        private const string URL_SCREEN = "/Administrator/Users/RunScript/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "UserID";

        Humica.Core.DB.HumicaDBContext DB = new Humica.Core.DB.HumicaDBContext();
        private SMSystemEntity DBA = new SMSystemEntity();

        public RunScriptController()
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
            ClsRunScript BSM = new ClsRunScript();
            BSM.Script = "";
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM.DataSource);
        }
        [HttpPost]
        public ActionResult Index(string ID)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            ClsRunScript BSM = new ClsRunScript();
            string Script = ID.Trim();
            var d = BSM.DataSource;
            if (ModelState.IsValid)
            {

                string msg = "";// collection.UpdateG();

                if (msg != "OK")
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    // return View(collection);

                }
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);

        }
        #endregion
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            ClsRunScript BSM = new ClsRunScript();
            BSM.ScreenId = SCREEN_ID;
            BSM.Script = "";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRunScript)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.DataSource);
        }
        public ActionResult Execute(string Script)
        {
            UserSession();
            ActionName = "Index";
            ClsRunScript BSM = new ClsRunScript();
            BSM.Script = Script;
            //  select = Script;
            //var d= BSM.DataSource;

            var result = new
            {
                MS = SYConstant.OK
            };
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        //String select = "";
        //System.Web.UI.WebControls.SqlDataSource DataSource
        //{
        //    get
        //    {
        //      //  var select = @"select *from ATDevSetting";

        //        //var d = new System.Web.UI.WebControls.SqlDataSource(NorthwindConnectionString, select1);
        //        return new System.Web.UI.WebControls.SqlDataSource(NorthwindConnectionString, select);
        //    }
        //}
        //string NorthwindConnectionString
        //{
        //    get
        //    {
        //        return System.Configuration.ConfigurationManager.ConnectionStrings["NWindConnectionString"].ConnectionString;
        //    }
        //}

    }
}
