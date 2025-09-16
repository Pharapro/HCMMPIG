using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Logic.SY;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.Services
{
    public class SYServiceBackgroundController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYS0001";
        private const string URL_SCREEN = "/Services/SYServiceBackground/";
        private string ActionName;
        private string KeyName = "ID";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        HumicaDBContext DB = new HumicaDBContext();

        public SYServiceBackgroundController()
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
            await DataSelector();
            ClsService BSM = new ClsService();
            BSM.BackgroundService = await DB.SyBackgroundServices.FirstOrDefaultAsync();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public async Task<ActionResult> Index(ClsService collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            await DataSelector();
            ClsService BSM = new ClsService();
            BSM = (ClsService)Session[Index_Sess_Obj + ActionName];

            collection.ScreenId = SCREEN_ID;
            string msg = await collection.UpdateSetting();

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
        #endregion
        private async Task DataSelector()
        {
            ViewData["DOWNLOAD_SELECT"] = ClsDownload.LoadData();
            ViewData["SCHEDULE_SELECT"] = ClsSchedule.LoadData();
            ViewData["TELEGRAM_SELECT"] = await DB.TelegramBots.ToListAsync();
        }
    }

}