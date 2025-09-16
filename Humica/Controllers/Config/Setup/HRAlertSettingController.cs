using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using Humica.Models.SY;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setup
{
    public class HRAlertSettingController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "INF0000004";
        private const string URL_SCREEN = "/Config/Setup/HRAlertSetting/";
        private string ActionName;
        private string KeyName = "TranNo";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string PARAM_INDEX = "TranNo;";
        HumicaDBContext DB = new HumicaDBContext();
        public HRAlertSettingController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region List
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            ClsAlertSetting BSM = new ClsAlertSetting();
            BSM.Header = new SYSHRAlert();
            var obj = DB.SYSHRAlerts.FirstOrDefault(w => w.TranNo == 1);
            if (obj != null)
            {
                BSM.Header = obj;
            }

            Session[Index_Sess_Obj + ActionName] = BSM;
            ViewData[ClsConstant.PARAM_INDEX] = PARAM_INDEX;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsAlertSetting collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            ClsAlertSetting BSM = new ClsAlertSetting();
            BSM = (ClsAlertSetting)Session[Index_Sess_Obj + ActionName];
            collection.ListAlert = BSM.ListAlert;

            collection.ScreenId = SCREEN_ID;
            if (ModelState.IsValid)
            {

                string msg = collection.UpdateG();

                if (msg != "OK")
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(collection);

                }
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                Session[Index_Sess_Obj + this.ActionName] = collection;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(collection);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);

        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            ClsAlertSetting BSM = new ClsAlertSetting();
            BSM.Header = new SYSHRAlert();
            BSM.ListAlert = new List<SYSHRAlert>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsAlertSetting)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListAlert);
        }
        #endregion
    }

}