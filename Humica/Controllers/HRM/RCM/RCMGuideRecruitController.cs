using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.RCM;
using Humica.Models.Report.HRM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.RCM
{
    public class RCMGuideRecruitController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RCM0000011";
        private const string URL_SCREEN = "/HRM/RCM/RCMGuideRecruit/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "GuideRecruitNo";
        private string DocType = "GRE01";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";

        HumicaDBContext DB = new HumicaDBContext();
        public RCMGuideRecruitController()
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
            UserConfListAndForm(this.KeyName);
            RCMGuideRecruitObject BSM = new RCMGuideRecruitObject();
            BSM.ListHeader = new List<RCMGuideRecruit>();
            BSM.ListHeader = DB.RCMGuideRecruits.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            RCMGuideRecruitObject BSM = new RCMGuideRecruitObject();
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (RCMGuideRecruitObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion 
        #region 'Create'
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();

            RCMGuideRecruitObject BSD = new RCMGuideRecruitObject();

            BSD.Header = new RCMGuideRecruit();
            BSD.ListHeader = new List<RCMGuideRecruit>();
            BSD.Header.RequestedDate = DateTime.Now;
            BSD.Header.Status = SYDocumentStatus.OPEN.ToString();
            BSD.Header.StaffRequestNo = 0;
            BSD.Header.PositionRequest = 0;
            BSD.Header.Gender = "B";
            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        [HttpPost]
        public ActionResult Create(RCMGuideRecruitObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            var BSM = (RCMGuideRecruitObject)Session[Index_Sess_Obj + ActionName];

            if (Session[PATH_FILE] != null)
            {
                collection.Header.Attachment = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            collection.DocType = DocType;

            if (ModelState.IsValid)
            {
                collection.ScreenId = SCREEN_ID;
                string msg = collection.createGuideRecruit();

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = collection.Header.GuideRecruitNo;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?GuideRecruitNo=" + mess.DocumentNumber;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;

                    BSM = NewGuideRecruit();

                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;

                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                Session[Index_Sess_Obj + this.ActionName] = collection;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(collection);
            }
            Session[Index_Sess_Obj + this.ActionName] = collection;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(collection);
        }
        #endregion 
        #region 'Edit'
        public ActionResult Edit(string GuideRecruitNo)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            RCMGuideRecruitObject BSM = new RCMGuideRecruitObject();

            BSM.Header = DB.RCMGuideRecruits.FirstOrDefault(w => w.GuideRecruitNo == GuideRecruitNo);

            if (BSM.Header != null)
            {
                string Approve = SYDocumentStatus.APPROVED.ToString();

                if (BSM.Header.Status != Approve)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV");
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string GuideRecruitNo, RCMGuideRecruitObject collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();

            RCMGuideRecruitObject BSM = new RCMGuideRecruitObject();

            BSM = (RCMGuideRecruitObject)Session[Index_Sess_Obj + ActionName];

            collection.DocType = DocType;
            collection.ScreenId = SCREEN_ID;

            if (Session[PATH_FILE] != null)
            {
                collection.Header.Attachment = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            else
            {
                collection.Header.Attachment = BSM.Header.Attachment;
            }
            if (ModelState.IsValid)
            {
                string msg = collection.updGuideRecruit(GuideRecruitNo);

                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(collection);
                }

                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = GuideRecruitNo;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?GuideRecruitNo=" + mess.DocumentNumber;
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
        public ActionResult Details(string GuideRecruitNo)
        {
            ActionName = "Details";
            UserSession();
            RCMGuideRecruitObject BSM = new RCMGuideRecruitObject();
            DataSelector();
            ViewData[SYConstant.PARAM_ID] = GuideRecruitNo;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            BSM.Header = DB.RCMGuideRecruits.FirstOrDefault(w => w.GuideRecruitNo == GuideRecruitNo);
            if (BSM.Header == null)
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            UserConfForm(SYActionBehavior.VIEW);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
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

            var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
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
        #region 'Delete'  
        public ActionResult Delete(string GuideRecruitNo)
        {
            UserSession();
            RCMGuideRecruitObject BSM = new RCMGuideRecruitObject();

            BSM.Header = DB.RCMGuideRecruits.FirstOrDefault(w => w.GuideRecruitNo == GuideRecruitNo);

            if (BSM.Header != null)
            {
                string msg = BSM.deleteGuideRecruit(GuideRecruitNo);

                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DELETE_M", user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion 
        #region 'Print'
        public ActionResult Print(string id)
        {
            this.UserSession();
            this.UserConf(ActionBehavior.VIEWONLY);
            ViewData[SYSConstant.PARAM_ID] = id;
            this.UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string id)
        {
            this.UserSession();
            this.UserConf(ActionBehavior.VIEWONLY);
            this.ActionName = "Print";
            var obj = DB.RCMGuideRecruits.Where(w => w.GuideRecruitNo == id).ToList();
            if (obj.Count > 0)
            {
                try
                {
                    ViewData[SYSConstant.PARAM_ID] = id;
                    FRMGuideRecruitment FRMGuideRecruitment = new FRMGuideRecruitment();
                    FRMGuideRecruitment.Parameters["GuideRecruitNo"].Value = obj.First().GuideRecruitNo;
                    FRMGuideRecruitment.Parameters["GuideRecruitNo"].Visible = false;
                    Session[this.Index_Sess_Obj + this.ActionName] = FRMGuideRecruitment;
                    return PartialView("PrintForm", FRMGuideRecruitment);
                }
                catch (Exception ex)
                {
                    SYEventLogObject.saveEventLog(new SYEventLog()
                    {
                        ScreenId = "RCM0000011",
                        UserId = this.user.UserID.ToString(),
                        DocurmentAction = id,
                        Action = SYActionBehavior.ADD.ToString()
                    }, ex, true);
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string GuideRecruitNo)
        {
            ActionName = "Print";
            FRMGuideRecruitment reportModel = (FRMGuideRecruitment)Session[Index_Sess_Obj + ActionName];
            ViewData[SYSConstant.PARAM_ID] = GuideRecruitNo;
            return ReportViewerExtension.ExportTo(reportModel);
        }

        #endregion
        #region 'Convert Status'
        public ActionResult Approve(string GuideRecruitNo)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (GuideRecruitNo != null)
            {
                RCMGuideRecruitObject BSM = new RCMGuideRecruitObject();
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (RCMGuideRecruitObject)Session[Index_Sess_Obj + ActionName];
                }

                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Approved(GuideRecruitNo);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.Description = mess.Description + ". " + BSM.MessageError;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?GuideRecruitNo=" + GuideRecruitNo);

        }
        public ActionResult Cancel(string GuideRecruitNo)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (GuideRecruitNo != null)
            {
                RCMGuideRecruitObject BSM = new RCMGuideRecruitObject();
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (RCMGuideRecruitObject)Session[Index_Sess_Obj + ActionName];
                }

                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Cancel(GuideRecruitNo);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_CANCELLED", user.Lang);
                    mess.Description = mess.Description + ". " + BSM.MessageError;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?GuideRecruitNo=" + GuideRecruitNo);

        }
        #endregion 
        #region 'private code'
        private RCMGuideRecruitObject NewGuideRecruit()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();

            RCMGuideRecruitObject BSD = new RCMGuideRecruitObject();
            BSD.Header = new RCMGuideRecruit();
            BSD.Header.RequestedDate = DateTime.Now;
            BSD.Header.Status = SYDocumentStatus.OPEN.ToString();
            BSD.Header.StaffRequestNo = 0;
            BSD.Header.PositionRequest = 0;
            BSD.Header.Gender = "B";

            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = BSD;
            return BSD;
        }
        private void DataSelector()
        {
            SYDataList ObjData = new SYDataList("GENDER_SELECT");
            ViewData["GENDER_SELECT"] = ObjData.ListData;
            ViewData["DEPT_SELECT"] = DB.HRDepartments.ToList().OrderBy(w => w.Description);
            ViewData["EMPCODE_SELECT"] = DB.HRStaffProfiles.ToList();
            ViewData["WORKTYPE_SELECT"] = DB.HRWorkingTypes.ToList().OrderBy(w => w.Description);
        }
        #endregion 
    }
}
