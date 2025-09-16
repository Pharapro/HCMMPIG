using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setup
{
    public class PRSettingController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "INF0000008";
        private const string URL_SCREEN = "/Config/Setup/PRSetting/";
        private string ActionName;
        private string KeyName = "ID";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        private string PARAM_INDEX = "ID;";

        public PRSettingController()
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
            DataSelector();
            MDSetting BSM = new MDSetting();
            BSM.Header = new SYHRSetting();
            BSM.isvisible = false;
            var obj = await DB.SYHRSettings.FirstOrDefaultAsync();
            if (obj != null)
            {
                BSM.Header = obj;
            }

            Session[Index_Sess_Obj + ActionName] = BSM;
            ViewData[ClsConstant.PARAM_INDEX] = PARAM_INDEX;
            return View(BSM);
        }
        [HttpPost]
        public async Task<ActionResult> Index(MDSetting collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDSetting BSM = new MDSetting();
            BSM = (MDSetting)Session[Index_Sess_Obj + ActionName];
            collection.ListHRSetting = BSM.ListHRSetting;
            collection.ScreenId = SCREEN_ID;
            string msg = collection.UpdateSetting();

            if (msg != SYConstant.OK)
            {
                SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + this.ActionName] = collection;
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

        public ActionResult ShowHide(string value)
        {

            this.ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            string h_ = "";
            MDSetting BSM = new MDSetting();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (MDSetting)Session[Index_Sess_Obj + ActionName];
                if (value != "LEAVE")
                {
                    h_ = "LEAVE";
                    BSM.hide = h_;
                    BSM.isvisible = true;
                    ViewData["Visiblie"] = false;
                }
                var data = new
                {
                    MS = SYConstant.OK,
                    Value_ = h_
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(data, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public async Task<ActionResult> PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            MDSetting BSM = new MDSetting();
            BSM.Header = new SYHRSetting();
            BSM.ListHRSetting = new List<SYHRSetting>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (MDSetting)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHRSetting);
        }
        #endregion
        private void DataSelector()
        {
            IEnumerable<PR_RewardsType> ListReward = DB.PR_RewardsType.ToList();
            ViewData["ALLW_SELECT"] = ListReward.Where(w => w.ReCode == "ALLW").ToList();
            ViewData["SALARTYPE_SELECT"] = ClsSelaryType.LoadData();
        }
    }

}