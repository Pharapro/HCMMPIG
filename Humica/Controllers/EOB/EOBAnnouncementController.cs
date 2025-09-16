using DevExpress.XtraPrinting;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.EOB;
using Humica.Models.Report.HRM;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.EOB
{
    public class EOBAnnouncementController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "EOB0000002";
        private const string URL_SCREEN = "/EOB/EOBAnnouncement/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "EmpCode";
        public string FileName;
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public EOBAnnouncementController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region 'List' 
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfList(this.KeyName);
            EOBAnnouncementObject BSM = new EOBAnnouncementObject();
            BSM.ListAnnounce = new List<HRStaffProfile>();
            BSM.ListAnnounce = DB.HRStaffProfiles.Where(W => W.IsAnnouncement == false).ToList(); // DB.RCMHires.ToList();
            //if (Session[Index_Sess_Obj + ActionName] != null)
            //{
            //    BSM = (EOBAnnouncementObject)Session[Index_Sess_Obj + ActionName];
            //}
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion
        #region 'Grid'
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(KeyName);
            DataSelector();
            EOBAnnouncementObject BSM = new EOBAnnouncementObject();
            BSM.ListAnnounce = new List<HRStaffProfile>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (EOBAnnouncementObject)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridItems", BSM.ListAnnounce);
        }
        #endregion
        #region 'Announce'
        public ActionResult Announcement(string EmpCode, string TeleGroup)
        {
            UserSession();
            DataSelector();
            var BSM = new EOBAnnouncementObject();
            if (!string.IsNullOrEmpty(EmpCode))
            {

                string[] EMP = EmpCode.Split(',');
                foreach (var _EmpCode in EMP)
                {
                    var obj = DB.HRStaffProfiles.Where(w => w.EmpCode == _EmpCode & w.IsAnnouncement == false).ToList();
                    if (obj.Count > 0)
                    {
                        ViewData[SYSConstant.PARAM_ID] = _EmpCode;
                        FRMEOBAnnounce FRMEOBAnnounce = new FRMEOBAnnounce();
                        FRMEOBAnnounce.Parameters["EmpCode"].Value = obj.First().EmpCode;
                        FRMEOBAnnounce.Parameters["EmpCode"].Visible = false;
                        Session[this.Index_Sess_Obj + this.ActionName] = FRMEOBAnnounce;

                        string fileName = Server.MapPath("~/Content/Announcement/" + _EmpCode + ".png");
                        string UploadDirectory = Server.MapPath("~/Content/Announcement");
                        if (!Directory.Exists(UploadDirectory))
                        {
                            Directory.CreateDirectory(UploadDirectory);
                        }

                        FRMEOBAnnounce.ExportOptions.Image.ExportMode = ImageExportMode.SingleFile;
                        FRMEOBAnnounce.ExportToImage(fileName);
                        FileName += fileName + ",";
                    }

                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Announce(EmpCode, TeleGroup, FileName);

                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = null;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                //}
                //ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                //return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region 'private code'
        private void DataSelector()
        {
            ViewData["POST_SELECT"] = DB.HRPositions.ToList();
            ViewData["LEVEL_SELECT"] = SMS.HRLevels.ToList();
            ViewData["BRANCH_SELECT"] = SMS.HRBranches.ToList();
            ViewData["DEPT_SELECT"] = DB.HRDepartments.ToList();
            ViewData["SELECT_TGGROUP"] = DB.TelegramBots.ToList();
        }
        #endregion 
    }
}
