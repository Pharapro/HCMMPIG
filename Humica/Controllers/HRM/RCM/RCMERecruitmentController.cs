using DevExpress.Web.Mvc;
using DevExpress.XtraReports.UI;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.RCM;
using Humica.Models.Report.HRM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SunPeople.Controllers.HRM.RCM
{

    public class RCMERecruitmentController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "RCM0000010";
        private const string URL_SCREEN = "/HRM/RCM/RCMERecruitment/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string Index_Sess_ObjR = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string ActionReport;
        private string KeyName = "JobID";
        private string DocType = "ERE01";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";

        IClsRCMERecruit BSM;
        IUnitOfWork unitOfWork;
        public RCMERecruitmentController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsRCMERecruit();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }

        #region 'List' 
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMERecruit)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = new List<RCMERecruit>();
            BSM.ListHeader = unitOfWork.Repository<RCMERecruit>().Queryable().ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult GridIndex()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);

            BSM.ListHeader = new List<RCMERecruit>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMERecruit)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_GridList", BSM.ListHeader);
        }
        #endregion 
        #region 'Create'
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            BSM.Header = new RCMERecruit();
            BSM.ListHeader = new List<RCMERecruit>();
            BSM.ListDetails = new List<RCMERecruitD>();
            BSM.Header.PosterNo = 1;
            BSM.Header.PostedDate = DateTime.Now;
            BSM.Header.EndDate = DateTime.Now;
            BSM.Header.Status = SYDocumentStatus.OPEN.ToString();

            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsRCMERecruit collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);
            if (Session[PATH_FILE] != null)
            {
                collection.Header.Attachfile = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMERecruit)Session[Index_Sess_Obj + ActionName];
            }
            BSM.DocType = DocType;
            BSM.Header = collection.Header;

            if (BSM.Header != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.createERecruit();

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.JobID;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?JobID=" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(BSM);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region 'Edit'
        public ActionResult Edit(string JobID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();

            UserConfListAndForm();
            BSM.Header = new RCMERecruit();
            if (JobID == "null") JobID = null;
            if (!string.IsNullOrEmpty(JobID))
            {
                BSM.Header = unitOfWork.Repository<RCMERecruit>().Queryable().FirstOrDefault(w => w.JobID == JobID);
                BSM.ListDetails = unitOfWork.Repository<RCMERecruitD>().Queryable().Where(w => w.JobID == JobID).ToList();
                if (BSM.Header != null)
                {
                    string Posted = SYDocumentStatus.POSTED.ToString();
                    if (BSM.Header.Status != Posted)
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
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string JobID, ClsRCMERecruit collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();


            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMERecruit)Session[Index_Sess_Obj + ActionName];
            }
            if (Session[PATH_FILE] != null)
            {
                collection.Header.Attachfile = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            else
            {
                collection.Header.Attachfile = BSM.Header.Attachfile;
            }
            BSM.DocType = DocType;
            BSM.ScreenId = SCREEN_ID;
            collection.ListDetails = BSM.ListDetails;
            if (JobID == "null") JobID = null;
            if (!string.IsNullOrEmpty(JobID))
            {
                string msg = collection.updERecruit(JobID);
                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(BSM);
                }

                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = JobID;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?JobID=" + mess.DocumentNumber;
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(BSM);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region 'Details'
        public ActionResult Details(string JobID)
        {
            ActionName = "Details";
            UserSession();

            DataSelector();
            BSM.Header = new RCMERecruit();
            if (JobID == "null") JobID = null;
            if (!string.IsNullOrEmpty(JobID))
            {
                ViewData[SYConstant.PARAM_ID] = JobID;
                ViewData[ClsConstant.IS_READ_ONLY] = true;
                BSM.Header = unitOfWork.Repository<RCMERecruit>().Queryable().FirstOrDefault(w => w.JobID == JobID);
                if (BSM.Header == null)
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                BSM.ListDetails = unitOfWork.Repository<RCMERecruitD>().Queryable().Where(w => w.JobID == JobID).ToList();
                UserConfForm(SYActionBehavior.VIEW);
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            return View(BSM);
        }
        #endregion 
        #region 'Delete'  
        public ActionResult Delete(string JobID)
        {
            UserSession();

            if (JobID == "null") JobID = null;
            if (!string.IsNullOrEmpty(JobID))
            {
                string msg = BSM.deleteERecruit(JobID);
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
        #region 'Upload'
        [HttpPost]
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = unitOfWork.Repository<CFUploadPath>().Queryable().FirstOrDefault(w => w.PathCode == "IMG_UPLOAD");
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
        #region 'Print'
        public ActionResult Print(string id)
        {
            this.ActionName = "Print";
            this.UserSession();
            this.UserConf(ActionBehavior.VIEWONLY);
            ViewData[SYSConstant.PARAM_ID] = id;
            BSM.ID = id;
            Session[Index_Sess_Obj + ActionName] = BSM;
            this.UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial()
        {
            this.UserSession();
            this.UserConf(ActionBehavior.VIEWONLY);
            this.ActionName = "Print";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMERecruit)Session[Index_Sess_Obj + ActionName];
            }
            string id = BSM.ID;
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    ViewData[SYSConstant.PARAM_ID] = id;
                    XtraReport sa = new XtraReport();
                    var obj = unitOfWork.Repository<RCMERecruitD>().Queryable().Where(w => w.JobID == id).ToList();
                    var objRpt = unitOfWork.Repository<CFReportObject>().Queryable().Where(w => w.ScreenID == SCREEN_ID && w.IsActive == true).ToList();
                    if (obj.Count() == 1)
                    {
                        sa = new FRMERecruitment();
                        if (objRpt.Count > 0)
                        {
                            sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                        }
                    }
                    if (obj.Count() > 1)
                        sa.Parameters["RequestNo"].Value = id;
                    sa.Parameters["RequestNo"].Visible = false;
                    Session[this.Index_Sess_ObjR + this.ActionReport] = sa;
                    return PartialView("PrintForm", sa);
                }
                catch (Exception ex)
                {
                    SYEventLogObject.saveEventLog(new SYEventLog()
                    {
                        ScreenId = SCREEN_ID,
                        UserId = this.user.UserID.ToString(),
                        DocurmentAction = id,
                        Action = SYActionBehavior.ADD.ToString()
                    }, ex, true);
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo()
        {
            ActionReport = "Print";

            if (Session[Index_Sess_ObjR] != null)
            {
                XtraReport reportModel = (XtraReport)Session[Index_Sess_ObjR];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion
        #region Post
        public async Task<ActionResult> Processing(string JobID, int mt)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (JobID == "null") JobID = null;
            if (!string.IsNullOrEmpty(JobID))
            {
                var listtoken = unitOfWork.Repository<SYSocialMedia>().Queryable().Where(w => w.ID == mt).FirstOrDefault();
                var captions = unitOfWork.Repository<RCMERecruit>().Queryable().Where(w => w.JobID == JobID).FirstOrDefault();
                string accessToken = listtoken.AccessToken;
                string caption = captions.ContactInfo;

                if (captions.Attachfile != null)
                {
                    string[] fileName = captions.Attachfile.Split('~');
                    string FileNames = Server.MapPath(captions.Attachfile);
                    try
                    {
                        // Post to Facebook
                        await PostPhotoToFacebook(accessToken, FileNames, caption);
                        // Update fields after successful post
                        string msg = BSM.UpdateAfferPost(JobID);
                        if (msg != SYConstant.OK)
                        {
                            SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                            Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                        }
                        SYMessages mess = SYMessages.getMessageObject("TRN_COM", user.Lang);
                        UserConfForm(ActionBehavior.SAVEGRID);
                        Session[SYConstant.MESSAGE_SUBMIT] = mess;
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
                else
                {
                    SYMessages mess_err = SYMessages.getMessageObject("INVALID_ATF", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            else
            {
                SYMessages mess_err = SYMessages.getMessageObject("INVALID_ATF", user.Lang);
                Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        static async Task PostPhotoToFacebook(string accessToken, string imagePath, string caption)
        {
            using (var httpClient = new HttpClient())
            {
                var form = new MultipartFormDataContent();
                using (var stream = new FileStream(imagePath, FileMode.Open))
                {
                    form.Add(new StreamContent(stream), "source", Path.GetFileName(imagePath));
                    form.Add(new StringContent(caption), "caption");
                    form.Add(new StringContent(accessToken), "access_token");

                    var response = await httpClient.PostAsync("https://graph.facebook.com/me/photos", form);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Failed to post photo on Facebook. Error message: {errorMessage}");
                    }
                }
            }
        }
        #endregion
        #region 'Grid Member'
        public ActionResult GridPosit()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMERecruit)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_List", BSM.ListDetails);
        }
        public ActionResult CreatePosit(RCMERecruitD ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMERecruit)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsRCMERecruit)Session[Index_Sess_Obj + ActionName];
                    }
                    if (!BSM.ListDetails.Where(w => w.RequestNo == ModelObject.RequestNo).Any())
                    {
                        BSM.ListDetails.Add(ModelObject);
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPLICATED_ITEM");
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_List", BSM.ListDetails);
        }
        public ActionResult EditPosit(RCMERecruitD ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMERecruit)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListDetails.Where(w => w.RequestNo == ModelObject.RequestNo).ToList();

                if (objCheck.Count > 0)
                {
                    objCheck.First().NoOfRecruit = ModelObject.NoOfRecruit;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("_List", BSM.ListDetails);
        }
        public ActionResult DeletePosit(string RequestNo)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMERecruit)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListDetails.Where(w => w.RequestNo == RequestNo).ToList();

                if (objCheck.Count > 0)
                {
                    BSM.ListDetails.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("_List", BSM.ListDetails);
        }
        #endregion 
        #region 'private code'
        private void DataSelector()
        {
            string Approved = SYDocumentStatus.APPROVED.ToString();
            var Datenow = DateTime.Now;
            var results = unitOfWork.Repository<RCMRecruitRequest>().Queryable()
             .Where(w => w.Status == Approved && EntityFunctions.TruncateTime(w.ExpectedDate) > Datenow)
             .OrderByDescending(w => w.RequestNo)
             .Join(
                 unitOfWork.Repository<HRPosition>().Queryable(),
                 recruitRequest => recruitRequest.POST,
                 position => position.Code,
                 (recruitRequest, position) => new
                 {
                     recruitRequest.POST,
                     recruitRequest.RequestNo,
                     recruitRequest.NoOfRecruit,
                     position.Description
                 }
             ).ToList();
            ViewData["POSITION_SELECT"] = results;
            ViewData["ADS_SELECT"] = unitOfWork.Repository<RCMSAdvertise>().Queryable().ToList();
            ViewData["TOKEN_SELECT"] = unitOfWork.Repository<SYSocialMedia>().Queryable().ToList();
        }
        #endregion 
    }
}
    