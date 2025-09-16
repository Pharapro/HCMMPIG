using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.Att;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.Attendance
{

    public class ATDownloadAttController : Humica.EF.Controllers.MasterSaleController
    {
        private static string Error = "";
        private const string SCREEN_ID = "ATM0000015";
        private const string URL_SCREEN = "/Attendance/Attendance/ATDownloadAtt/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "DeviceID";

        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SM = new SMSystemEntity();

        public ATDownloadAttController()
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
            ATDownloadObject BSM = new ATDownloadObject();
            BSM.Filter = new Humica.Core.FT.FTFilterInOut();
            BSM.Filter.FromDate = DateTime.Now;
            BSM.Filter.ToDate = DateTime.Now;
            return View(BSM);
        }
        #region "Download"
        public async Task<ActionResult> Download(DateTime FromDate, DateTime ToDate)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ATDownloadObject BSM = new ATDownloadObject();
            if (FromDate != null)
            {
                string msg = await BSM.DownloadData(FromDate, ToDate);

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #endregion
    }
}
