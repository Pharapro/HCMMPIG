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
    public class HRSettingController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "INF0000013";
        private const string URL_SCREEN = "/Config/Setup/HRSetting/";
        private string ActionName;
        private string KeyName = "ID";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        private string PARAM_INDEX = "ID;";

        public HRSettingController()
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
            DataSelector();
            MDSetting BSM = new MDSetting();
            BSM.Header = new SYHRSetting();
            BSM.isvisible = false;
            var obj = DB.SYHRSettings.FirstOrDefault();
            if (obj != null)
            {
                BSM.Header = obj;
                BSM.staff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == obj.Hr_manager);
            }

            Session[Index_Sess_Obj + ActionName] = BSM;
            ViewData[ClsConstant.PARAM_INDEX] = PARAM_INDEX;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(MDSetting collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDSetting BSM = new MDSetting();
            BSM = (MDSetting)Session[Index_Sess_Obj + ActionName];
            collection.ListHRSetting = BSM.ListHRSetting;

            collection.ScreenId = SCREEN_ID;
            string msg = collection.UpdateSettingS();

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
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            DataSelector();
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
        public ActionResult ShowDataEmp(string EmpCode)
        {

            ActionName = "Details";
            HR_STAFF_VIEW EmpStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    Position = EmpStaff.Position,
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            ViewData["TELEGRAM_SELECT"] = DB.TelegramBots.ToList();
            string DEDType = "DED";
            ViewData["DED_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == DEDType).ToList();
            //ViewData["ALLW_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == "ALLW").ToList();
            //ViewData["LATE_DED_TYPE"] = new SYDataList("LATEDEDTYPE").ListData;
            ViewData["LEAVE_TYPE"] = DB.HRLeaveTypes.ToList();
        }
    }

}