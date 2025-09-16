using Humica.EF;
using Humica.EF.MD;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setup
{

    public class SYErrorLogController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "SYCFL001";
        private const string URL_SCREEN = "/Administrator/Systems/SYErrorLog/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        SMSystemEntity DB = new SMSystemEntity();

        public SYErrorLogController()
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
            List<SYEventLog> EvenLogList = new List<SYEventLog>();
            DateTime date = DateTime.Now.AddMonths(-1);
            EvenLogList = DB.SYEventLogs.OrderByDescending(w => w.ID).ToList();
            EvenLogList = EvenLogList.Where(w => w.LogDate.Value.Date > date.Date).ToList();
            return View(EvenLogList);
        }

        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            List<SYEventLog> EvenLogList = new List<SYEventLog>();
            DateTime date = DateTime.Now.AddMonths(-1);
            EvenLogList = DB.SYEventLogs.OrderByDescending(w => w.ID).ToList();
            EvenLogList = EvenLogList.Where(w => w.LogDate.Value.Date > date.Date).ToList();
            return PartialView(SYListConfuration.ListDefaultView, EvenLogList);
        }

        #endregion



    }
}
