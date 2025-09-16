using Humica.Core.SY;
using Humica.EF;
using Humica.EF.Models.SY;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setup
{
    public class MailSendBoxController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYEMT100";
        private const string URL_SCREEN = "/Config/Setup/MailSendBox/";
        private readonly string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private readonly string KeyName = "Id";
        private readonly string GRID_EDITOR_ERROR = "EditError";

        public MailSendBoxController()
            : base()
        {
            ScreendIDControl = SCREEN_ID;
            ScreenUrl = URL_SCREEN;
        }

        #region "List"

        public ActionResult Index()
        {
            ActionName = "Create";
            UserSession();
            UserConf(ActionBehavior.LIST);

            ClsEmail BSM = new ClsEmail
            {
                ScreenId = SCREEN_ID
            };

            /*string msg = */
            BSM.GetAllList();
            //if (msg != SYConstant.OK)
            //{
            //    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            //}

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        //[HttpPost]
        //public ActionResult Index(ClsIntegration BSM)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfList(SYActionBehavior.LIST, KeyName, ControllerContext.RouteData.Values["controller"].ToString());

        //    try
        //    {
        //        BSM.ScreenID = SCREEN_ID;
        //        string msg = BSM.GetAllList();
        //        if (msg != SYConstant.OK)
        //        {
        //            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
        //        }
        //        else
        //        {
        //            //Todo: Create XML file by the setting
        //            var DBC = new DBGenerateX();
        //            var sYIntegration = DBC.SYIntegrations.FirstOrDefault(s => s.IntegrationModuleName == "MASTER");
        //            string fileNameDest = Path.Combine(sYIntegration.PathFileStore, "Reset/reset.a");
        //            Thread.Sleep(4000);

        //            if (System.IO.File.Exists(fileNameDest))
        //            {
        //                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("SYNC_C", user.Lang);
        //            }
        //            else
        //            {
        //                XmlSerializer serializer = new XmlSerializer(typeof(SSCITM));
        //                Stream fs = new FileStream(fileNameDest, FileMode.Create);
        //                XmlWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
        //                //serializer.Serialize(writer, "reset");
        //                writer.Close();
        //                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("SYNC_C", user.Lang);
        //            }
        //        }
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        var log = new SYEventLog
        //        {
        //            ScreenId = BSM.ScreenID,
        //            UserId = BSM.User.UserName,
        //            Action = SYActionBehavior.VIEW.ToString()
        //        };
        //        SYEventLogObject.saveEventLog(log, e);
        //        /*----------------------------------------------------------*/
        //        Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        var log = new SYEventLog
        //        {
        //            ScreenId = BSM.ScreenID,
        //            UserId = BSM.User.UserName,
        //            Action = SYActionBehavior.VIEW.ToString()
        //        };
        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
        //    }
        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        var log = new SYEventLog
        //        {
        //            ScreenId = BSM.ScreenID,
        //            UserId = BSM.User.UserName,
        //            Action = SYActionBehavior.VIEW.ToString()
        //        };
        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
        //    }

        //    Session[Index_Sess_Obj + ActionName] = BSM;
        //    return View(BSM);
        //}

        #endregion "List"

        #region "GridItems1"

        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();

            ClsEmail BSM = new ClsEmail();
            BSM.GetAllList(int.Parse(id));

            return View(BSM);
        }

        public ActionResult GridItems1()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClsEmail BSM = new ClsEmail();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmail)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            return PartialView("GridItems1", BSM.ListJobs);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartialGridItems1(string Id)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClsEmail BSM = new ClsEmail();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmail)Session[Index_Sess_Obj + ActionName];
            }
            string msg = BSM.DeleteGridItems1(Id);
            if (msg != SYConstant.OK)
            {
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage(msg);
            }
            return PartialView("GridItems1", BSM.ListJobs);
        }

        public ActionResult GridJobDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            ClsEmail BSM = new ClsEmail();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmail)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            return PartialView("GridJobDetails", BSM.ListJobStates);
        }


        #endregion "GridItems1"

        #region "Private Code"

        private void DataList()
        {
            //ViewData["LIST_INTEGRATION_MODULE"] = new SYDataList("INTEGRATION_MODULE").ListData;
        }

        #endregion "Private Code"
    }
}