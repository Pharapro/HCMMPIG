using Humica.Attendance;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.AttSetup
{

    public class ATPolicySettingController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ATS0000003";
        private const string URL_SCREEN = "/Attendance/AttSetup/ATPolicySetting/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "TranNo";
        private string PARAM_INDEX = "TranNo;";

        IClsATPolicyObject BSM;
        public ATPolicySettingController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsATPolicyObject();
            BSM.OnLoad();
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;
            ViewData[ClsConstant.PARAM_INDEX] = PARAM_INDEX;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsATPolicyObject collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM = (IClsATPolicyObject)Session[Index_Sess_Obj + ActionName];
            collection.ScreenId = SCREEN_ID;
            if (ModelState.IsValid)
            {
                string msg = collection.Update();
                if (msg != SYConstant.OK)
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
        private void DataSelector()
        {
            foreach (var data in BSM.OnDataSelector())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}
