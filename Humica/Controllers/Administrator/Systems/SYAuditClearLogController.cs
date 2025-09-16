using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.SY;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace CLOUDVIEW.Controllers.Configuration.Setup
{
    public class SYAuditClearLogController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYUP00003";
        private const string URL_SCREEN = "/Configuration/Setup/SYAuditClearLog/";
        private string Index_Sess_Obj  = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string KeyName = "ID";
        private string GRID_EDITOR_ERROR = "EditError";
        private SMSystemEntity DBX = new SMSystemEntity();
        private string ActionName = "";
        public SYAuditClearLogController()
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
            UserConf(ActionBehavior.LIST);
            SYBackupRestore BSM = new SYBackupRestore();
            BSM.ScreenId = SCREEN_ID;
            BSM.IsLock = true;
            BSM.ListAuditLog = new System.Collections.Generic.List<ExEventAuditLog>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(SYBackupRestore BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConf(ActionBehavior.LIST);
            BSM.ScreenId = SCREEN_ID;

            BSM.ListAuditLog = DBX.ExEventAuditLogs.Where(w => w.Company == SYConstant.DEFAULT_PLANT).ToList();
            if (BSM.ScreenFilter != null)
            {
                BSM.ListAuditLog = BSM.ListAuditLog.Where(w => w.ScreenID == BSM.ScreenFilter).ToList();
            }
            if (BSM.UserName != null)
            {
                BSM.ListAuditLog = BSM.ListAuditLog.Where(w => w.UserName == BSM.UserName).ToList();
            }
            if (BSM.DocumentFilter != null)
            {
                BSM.ListAuditLog = BSM.ListAuditLog.Where(w => w.KeyValue == BSM.DocumentFilter).ToList();
            }

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion
        #region "Audit Log"
        public ActionResult GridItemsAuditLog()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            SYBackupRestore BSM = new SYBackupRestore();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SYBackupRestore)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemsAuditLog", BSM.ListAuditLog);
        }
        #endregion

    }
}
