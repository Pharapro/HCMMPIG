using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Humica.Controllers.HRM.EmployeeInfo
{

    public class HRAnnouncementController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRE0000016";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HRAnnouncement/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string PATH_FILE2 = "12313123123sadfsdfsdfs12fxdf";
        Humica.Core.DB.HumicaDBContext DB = new Humica.Core.DB.HumicaDBContext();

        public HRAnnouncementController()
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
            DataSelector();

            CFAnnouncementObject BSM = new CFAnnouncementObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (CFAnnouncementObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = DB.HRAnnouncements.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult GridItem()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            CFAnnouncementObject BSM = new CFAnnouncementObject();
            BSM.ListHeader = new List<HRAnnouncement>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (CFAnnouncementObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItem", BSM);
        }

        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            CFAnnouncementObject BSD = new CFAnnouncementObject();
            BSD.Header = new HRAnnouncement();
            BSD.Header.ValidDate = DateTime.Now;
            return View(BSD);
        }
        [HttpPost]
        public ActionResult Create(CFAnnouncementObject BSM)
        {
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            CFAnnouncementObject BSD = new CFAnnouncementObject();

            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                //string Code = BSM.Header.Code.ToUpper();
                //BSM.Header.Code = Code;
                BSD.Header = BSM.Header;
                if (Session[PATH_FILE] != null)
                {
                    if (Session[PATH_FILE] != null)
                    {
                        BSM.Header.AttachFile = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                        BSM.Header.Document = Session[PATH_FILE2].ToString();
                        Session[PATH_FILE2] = null;
                    }
                }
                string msg = BSM.CreateAnnouncement();

                if (msg == SYConstant.OK)
                {

                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.ID.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM = new CFAnnouncementObject();
                    BSM.Header = new HRAnnouncement();
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);

        }
        #endregion
        #region "Edit"
        public ActionResult Edit(int id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            if (id.ToString() != null)
            {
                CFAnnouncementObject BSM = new CFAnnouncementObject();
                BSM.Header = DB.HRAnnouncements.Find(id);
                if (BSM.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("LEAVE_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(int id, CFAnnouncementObject BSM)
        {
            ActionName = "Edit";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            CFAnnouncementObject BSD = new CFAnnouncementObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSD = (CFAnnouncementObject)Session[Index_Sess_Obj + ActionName];
            }

            if (id.ToString() != null)
            {
                if (Session[PATH_FILE] != null)
                {
                    if (Session[PATH_FILE] != null)
                    {
                        BSM.Header.AttachFile = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                        BSM.Header.Document = Session[PATH_FILE2].ToString();
                        Session[PATH_FILE2] = null;
                    }
                }
                else
                {
                    BSM.Header.AttachFile = BSD.Header.AttachFile;
                    BSM.Header.Document = BSD.Header.Document;
                }
                BSM.ScreenId = SCREEN_ID;

                BSD.Header = BSM.Header;

                string msg = BSM.EditAnnouncement(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);

        }
        #endregion
        #region "Delete"
        public ActionResult Delete(int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id.ToString() != null)
            {
                CFAnnouncementObject Del = new CFAnnouncementObject();
                string msg = Del.deleteAnnouncement(id);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("ANNOUN_DEL", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("ANNOUN_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        public ActionResult Details(int ID)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;

            if (ID != null)
            {
                CFAnnouncementObject BSD = new CFAnnouncementObject();
                BSD.Header = DB.HRAnnouncements.Find(ID);
                if (BSD.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSD;
                    return View(BSD);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("WORKFLOW_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #region 'Announce'
        public ActionResult Announcement(int ID, string TeleGroup)
        {
            UserSession();
            DataSelector();
            var BSM = new CFAnnouncementObject();

            var obj = DB.HRAnnouncements.FirstOrDefault(w => w.ID == ID);
            if (obj != null)
            {
                string msg = "";
                if (!string.IsNullOrEmpty(obj.AttachFile))
                {
                    ViewData[SYSConstant.PARAM_ID] = ID;
                    //string[] fileName = obj.AttachFile.Split('~');
                    //foreach (var part in fileName)
                    //{
                    //    if (string.IsNullOrEmpty(part)) continue;
                    //    string FileNames = Server.MapPath(obj.AttachFile);
                    //    BSM.ScreenId = SCREEN_ID;
                    //    msg = BSM.Annount(obj, TeleGroup, FileNames, "");
                    //}
                    string[] fileName = obj.AttachFile.Split(';');
                    List<string> FileNames = new List<string>();
                    foreach (var PName in fileName)
                    {
                        if (string.IsNullOrEmpty(PName)) continue;
                        //string[] fileName1 = PName.Split('~');
                        //foreach (var part in fileName1)
                        //{
                        string FileName = Server.MapPath(PName);
                        FileNames.Add(FileName);
                        //}
                    }
                    msg = BSM.Annount(obj, TeleGroup, FileNames, "");
                }
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = null;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        [HttpPost]
        public ActionResult UploadControlCallbackActionImage(HttpPostedFileBase[] file_Uploader)
        {
            UserSession();
            DataSelector();
            var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
            var sfi = new SYFileImport(path);
            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("UploadControl",
                sfi.ValidationSettings,
                this.uc_FileUploadCompleteM);
            return null;
        }

        public void uc_FileUploadCompleteM(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
                var objFile = new SYFileImport(path);

                string[] sp = e.UploadedFile.FileName.Split('.');

                string filename = "";
                if (sp.Length > 0)
                {
                    filename = e.UploadedFile.FileName;
                }
                else
                {
                    filename = e.UploadedFile.FileName;
                }
                string resultFilePath = Server.MapPath(path.PathDirectory + filename);


                e.UploadedFile.SaveAs(resultFilePath);

                IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                if (urlResolver != null)
                {
                    e.CallbackData = urlResolver.ResolveClientUrl(resultFilePath);
                    Session[PATH_FILE2] += e.UploadedFile.FileName + ";";
                    Session[PATH_FILE] += path.PathDirectory.Replace("~", "") + "/" + filename + ";";
                }
                else
                {
                    e.ErrorText = SYMessages.getMessage("FILE_EXIST");
                }


            }
        }
        private void DataSelector()
        {
            ViewData["SELECT_TGGROUP"] = DB.TelegramBots.ToList();
            ViewData["Announcement_SELECT"] = DB.HRAnnounceTypes.ToList();
        }
    }
}
