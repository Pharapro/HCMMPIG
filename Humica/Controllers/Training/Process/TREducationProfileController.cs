using DevExpress.Web.Mvc;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using Humica.Training;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Training
{
    public class TREducationProfileController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "TR00000010";
        private const string URL_SCREEN = "/Training/Process/TREducationProfile/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContextTR DB = new HumicaDBContextTR();
        Humica.Core.DB.HumicaDBContext DBB = new Humica.Core.DB.HumicaDBContext();
        Humica.Core.DB.HumicaDBViewContext DBV = new Humica.Core.DB.HumicaDBViewContext();
        public TREducationProfileController()
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
            UserConfListAndForm(this.KeyName);
            TREduProfileObject BSM = new TREduProfileObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (TREduProfileObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = DB.TREducationProfile.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            TREduProfileObject BSM = new TREduProfileObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TREduProfileObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            TREduProfileObject BSM = new TREduProfileObject();
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TREduProfileObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM);
        }
        #endregion
        #region Create
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            TREduProfileObject BSM = new TREduProfileObject();
            BSM.Header = new TREducationProfile();
            BSM.Header.DateAwarded = DateTime.Now;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(TREduProfileObject collection)
        {
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ActionName = "Create";
            TREduProfileObject BSM = new TREduProfileObject();
            if (ModelState.IsValid)
            {
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.AttachFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (TREduProfileObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateEduPro();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.ID.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);

                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");

        }
        #endregion
        #region Edit
        public ActionResult Edit(int ID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            if (ID != null)
            {
                TREduProfileObject BSM = new TREduProfileObject();
                BSM.Header = new TREducationProfile();
                //BSM.HeaderStaff = new HR_STAFF_VIEW();
                BSM.Header = DB.TREducationProfile.FirstOrDefault(w => w.ID == ID);
                var resualt = DB.TREducationProfile.ToList();
                if (BSM.Header != null)
                {
                    List<TREducationProfile> listEmpfa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                    BSM.ListHeader = listEmpfa.ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(int id, TREduProfileObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            TREduProfileObject BSM = new TREduProfileObject();

            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (TREduProfileObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.AttachFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                else
                {
                    collection.Header.AttachFile = BSM.Header.AttachFile;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditEduPro(id);
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
                    BSM.ListHeader = DB.TREducationProfile.ToList();
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return View(BSM);

        }
        #endregion
        #region Delete
        public ActionResult Delete(int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id != null)
            {
                TREduProfileObject Del = new TREduProfileObject();
                string msg = Del.DeleteEduPro(id);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region Details
        public ActionResult Details(int id)
        {
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                TREduProfileObject BSM = new TREduProfileObject();
                BSM.Header = new TREducationProfile();
                //BSM.HeaderStaff = new HR_STAFF_VIEW();
                BSM.Header = DB.TREducationProfile.FirstOrDefault(w => w.ID == id);
                var resualt = DB.TREducationProfile.ToList();
                if (BSM.Header != null)
                {
                    List<TREducationProfile> listEmpfa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                    BSM.ListHeader = listEmpfa.ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Upload
        [HttpPost]
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = DBB.CFUploadPaths.Find("IMG_UPLOAD");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);

            objFile.ObjectTemplate = new Humica.Core.DB.MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREEN_ID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;

            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("UploadControl", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            Session[PATH_FILE] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }
        #endregion 'Upload'
        private void DataSelector()
        {
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListStaff = DBV.HR_STAFF_VIEW.ToList();
            ListStaff = ListStaff.Where(w => ListBranch.Where(x => x.Code == w.BranchID).Any()).ToList();

            ViewData["STAFF_SELECT"] = ListStaff;
            ViewData["QUALI_CLASS"] = DB.TRQualificatClass.ToList();
            ViewData["QUALI_NAME"] = DB.TRQualificatName.ToList();
            ViewData["QUALI_TYPE"] = DB.TRQualificatType.ToList();
        }
    }
}