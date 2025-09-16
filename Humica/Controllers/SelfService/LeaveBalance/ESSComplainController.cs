using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.LeaveBalance
{

    public class ESSComplainController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000010";
        private const string URL_SCREEN = "/SelfService/LeaveBalance/ESSComplain/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        HumicaDBContext DB = new HumicaDBContext();

        public ESSComplainController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            ESSComplainObject BSM = new ESSComplainObject();
            BSM.Header = new HRComplain();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ESSComplainObject collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            ESSComplainObject BSM = new ESSComplainObject();
            BSM = (ESSComplainObject)Session[Index_Sess_Obj + ActionName];
            collection.ScreenId = SCREEN_ID;
            if (ModelState.IsValid)
            {
                string msg = collection.SaveComplain();

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM.Header = new HRComplain();
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Index");
                }
                Session[Index_Sess_Obj + this.ActionName] = collection;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(collection);
            }
            Session[Index_Sess_Obj + this.ActionName] = collection;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(collection);
        }
    }
}
