using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.EOB;
using Humica.Models.SY;
using HUMICA.Models.Report.EOB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.EOB
{
    public class EOBConfirmAlertController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "EOB0000001";
        private const string URL_SCREEN = "/EOB/EOBConfirmAlert/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        IClsEOBConfirmAlert BSM;
        IUnitOfWork unitOfWork;
        public EOBConfirmAlertController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsEOBConfirmAlert();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        #region 'List' 
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.ListHeader = new List<EOBConfirmAlert>();
            BSM.ListApplicant = new List<RCMApplicant>();
            BSM.ListHeader = unitOfWork.Repository<EOBConfirmAlert>().Queryable().OrderByDescending(w => w.ID).ToList();
            var _Confirmed = unitOfWork.Repository<EOBConfirmAlert>().Queryable().Select(x => x.ID).ToList();
            BSM.ListApplicant = unitOfWork.Repository<RCMApplicant>().Queryable().Where(w => !_Confirmed.Contains(w.ApplicantID) && w.IntVStatus == "PASS").OrderByDescending(w => w.ApplicantID).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion
        #region 'Create'
        public ActionResult Create(string ApplicantID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            BSM.Header = new EOBConfirmAlert();
            BSM.ListHeader = new List<EOBConfirmAlert>();
            BSM.Applicant = new RCMApplicant();
            if (ApplicantID == "null") ApplicantID = null;
            if (!string.IsNullOrEmpty(ApplicantID))
            {
                BSM.Applicant = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == ApplicantID);
                var _chkID = unitOfWork.Repository<RCMPInterview>().Queryable().Select(w => w.ApplicantID);
                var _chkApplicant = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => _chkID.Contains(w.ApplicantID));
                if (_chkApplicant != null)
                {
                    BSM.Header.Remark = _chkApplicant.Email;
                }
                if (BSM.Applicant != null)
                {
                    BSM.Header.ID = BSM.Applicant.ApplicantID;
                }
                BSM.Header.Confirmed = false;
                BSM.Header.DateOfSending = DateTime.Now;
                BSM.Header.SendingSelected = "EM";

                UserConfListAndForm();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Create(string ApplicantID, ClsEOBConfirmAlert collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            if (Session[PATH_FILE] != null)
            {
                collection.Header.AttachForm = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            string msg = collection.ConfAlert(ApplicantID);

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = ApplicantID;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?ID=" + mess.DocumentNumber;
                Session[Index_Sess_Obj + this.ActionName] = collection;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(collection);
            }
            Session[Index_Sess_Obj + this.ActionName] = collection;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            return View(collection);
        }
        #endregion 
        #region 'Edit'
        public ActionResult Edit(string ID)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (ID == "null") ID = null;
            if (!string.IsNullOrEmpty(ID))
            {
                BSM.Header = unitOfWork.Repository<EOBConfirmAlert>().Queryable().FirstOrDefault(w => w.ID == ID);
                if (BSM.Header != null)
                {
                    if (BSM.Header.Confirmed != true)
                    {
                        BSM.Header.Confirmed = true;
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        return View(BSM);
                    }
                    else
                    {
                        Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV");
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                }
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string ID, ClsEOBConfirmAlert collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();

            BSM = (ClsEOBConfirmAlert)Session[Index_Sess_Obj + ActionName];
            collection.ScreenId = SCREEN_ID;

            if (Session[PATH_FILE] != null)
            {
                collection.Header.AttachForm = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            else
            {
                collection.Header.AttachForm = BSM.Header.AttachForm;
            }
            if (ModelState.IsValid)
            {
                string msg = collection.updConfirm(ID);

                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(collection);
                }

                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = ID;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?ID=" + mess.DocumentNumber;
                Session[Index_Sess_Obj + this.ActionName] = collection;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(collection);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }

        #endregion 
        #region 'Details'
        public ActionResult Details(string ID)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            if (ID == "null") ID = null;
            if (!string.IsNullOrEmpty(ID))
            {
                ViewData[SYConstant.PARAM_ID] = ID;
                ViewData[ClsConstant.IS_READ_ONLY] = true;
                BSM.Header = unitOfWork.Repository<EOBConfirmAlert>().Queryable().FirstOrDefault(w => w.ID == ID);
                if (BSM.Header == null)
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("Error");
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                UserConfForm(SYActionBehavior.VIEW);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region 'Grid'
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEOBConfirmAlert)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridItems", BSM.ListApplicant);
        }
        public ActionResult GridConfirms()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();


            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEOBConfirmAlert)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridConfirms", BSM.ListHeader);
        }
        #endregion 
        #region 'Upload'
        [HttpPost]
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = unitOfWork.Repository<CFUploadPath>().FirstOrDefault(w => w.PathCode == "IMG_UPLOAD");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);

            objFile.ObjectTemplate = new MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREEN_ID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;

            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("UploadControl", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            Session[PATH_FILE] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }
        #endregion 
        #region "Confirm"
        public ActionResult Confirm(string id)
        {
            this.UserSession();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id == "null") id = null;
            if (!string.IsNullOrEmpty(id))
            {
                FRM_ConfirmAlert sa = new FRM_ConfirmAlert();
                var reportHelper = new clsReportHelper();
                string path = reportHelper.Get_Path(SCREEN_ID);
                if (!string.IsNullOrEmpty(path))
                    sa.LoadLayoutFromXml(path);
                sa.Parameters["ID"].Value = id;
                sa.Parameters["ID"].Visible = false;

                Session[this.Index_Sess_Obj + this.ActionName] = sa;

                string fileName = Server.MapPath("~/Content/UPLOAD/" + id + ".pdf");
                string UploadDirectory = Server.MapPath("~/Content/UPLOAD");
                if (!Directory.Exists(UploadDirectory))
                {
                    Directory.CreateDirectory(UploadDirectory);
                }
                sa.ExportToPdf(fileName);
                string sms = BSM.ApproveTheDoc(id, fileName);
                if (sms == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(sms, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.Description = messageObject.Description + BSM.MessageError;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN;
                    SYMessages mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        public ActionResult ReleaseDoc(string id)
        {
            UserSession();

            if (id == "null") id = null;
            if (!string.IsNullOrEmpty(id))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.requestApprove(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id;
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Consider(string id)
        {
            UserSession();
            if (id == "null") id = null;
            if (!string.IsNullOrEmpty(id))
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsEOBConfirmAlert)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;

                string msg = BSM.Consider(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id;
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #region "Reject"
        public ActionResult Reject(string id)
        {
            this.UserSession();
            DataSelector();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id == "null") id = null;
            if (!string.IsNullOrEmpty(id))
            {
                string sms = BSM.RejectTheDoc(id);
                if (sms == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(sms, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN;
                    SYMessages mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region 'private code'
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("SEX");
            ViewData["GENDER_SELECT"] = objList.ListData;
            objList = new SYDataList("MARITAL");
            ViewData["MARITAL_SELECT"] = objList.ListData;
            objList = new SYDataList("LANG_SKILLS");
            ViewData["POST_SELECT"] = unitOfWork.Repository<HRPosition>().Queryable().ToList();
        }
        #endregion 
    }
}