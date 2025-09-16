using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using Humica.Training.DB;
using Humica.Training.Setup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Humica.Controllers.Training.SetUp
{
    public class TRELearningModuleController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "TRM000005";
        private const string URL_SCREEN = "/Training/Setup/TRELearningModule/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Topic";
        private string PATH_FILE = "12313123123sadfsdfsdfs12f";
        private string PATH_FILE1 = "12313123123sadfsdfsdfs12fxde";
        private string PATH_FILE2 = "12313123123sadfsdfsdfs12fxdf";
        private string PATH_FILE3 = "12313123123sadfsdfsdfs12fxdg";
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        SMSystemEntity SMS = new SMSystemEntity();
        Humica.Core.DB.HumicaDBContext DB = new Humica.Core.DB.HumicaDBContext();
        Humica.Core.DB.HumicaDBViewContext DBV = new Humica.Core.DB.HumicaDBViewContext();
        public TRELearningModuleController()
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
            DataSelector();
            Session[PATH_FILE] = null;
            Session[PATH_FILE1] = null;
            Session[PATH_FILE2] = null;
            Session[PATH_FILE3] = null;
            Session["Video"] = "D";
            UserConfList(KeyName);
            ClsELearningModule BSM = new ClsELearningModule();

            BSM.ListQuestion = DBX.TrainigModule.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Index(ClsELearningModule collection)
        {

            ActionName = "Index";
            UserSession();
            DataSelector();
            Session[PATH_FILE] = null;
            Session[PATH_FILE1] = null;
            Session[PATH_FILE2] = null;
            Session[PATH_FILE3] = null;
            Session["Video"] = "D";
            UserConfList(KeyName);
            ClsELearningModule BSM = new ClsELearningModule();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsELearningModule)Session[Index_Sess_Obj + ActionName];
            }
            BSM.TrainingType = collection.TrainingType;
            BSM.Course = collection.Course;
            BSM.ListQuestion = DBX.TrainigModule.ToList();
            if (BSM.TrainingType != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.TrainingType == BSM.TrainingType).ToList();
            }

            if (BSM.Course != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.CourseCode == BSM.Course).ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion

        #region "_GridItem"
        public ActionResult _GridItem()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsELearningModule BSM = new ClsELearningModule();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsELearningModule)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListQuestion = DBX.TrainigModule.ToList();
            if (BSM.TrainingType != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.TrainingType == BSM.TrainingType).ToList();
            }

            if (BSM.Course != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.CourseCode == BSM.Course).ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_GridItem", BSM);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TrainigModule ModelObject)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsELearningModule BSM = new ClsELearningModule();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsELearningModule)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelObject.TrainingType == null || ModelObject.CourseCode == null ||
                        ModelObject.Topic == null || ModelObject.Description == null ||
                        ModelObject.Category == null || ModelObject.Score == null ||
                        ModelObject.ScorePr == null || ModelObject.Timer == 0)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                    }
                    else
                    {
                        var check = DBX.TrainigModule.FirstOrDefault(w => w.TrainingType == ModelObject.TrainingType && w.CourseCode == ModelObject.CourseCode && w.Topic == ModelObject.Topic);

                        if (check != null)
                        {
                            ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED", user.Lang);
                        }
                        else
                        {
                            ModelObject.UrlDocument = "";
                            ModelObject.OtherDocument = "";
                            if (Session[PATH_FILE] != null)
                            {
                                ModelObject.UrlDocument = Session[PATH_FILE].ToString();
                                Session[PATH_FILE] = null;
                                ModelObject.Document = Session[PATH_FILE2].ToString();
                                Session[PATH_FILE2] = null;
                            }
                            if (Session[PATH_FILE1] != null)
                            {
                                ModelObject.UrlOtherDocument = Session[PATH_FILE1].ToString();
                                Session[PATH_FILE1] = null;
                                ModelObject.OtherDocument = Session[PATH_FILE3].ToString();
                                Session[PATH_FILE3] = null;
                            }
                            //string resultFilePath=SYUrl.getBaseUrl() + ModelObject.Document;
                            //string resultFilePath = Server.MapPath("~/Content/UPLOAD/TRAINING/DOC/" + ModelObject.TrainingType + "/" + ModelObject.CourseCode + "/" + ModelObject.Document);
                            //ModelObject.UrlDocument = resultFilePath;
                            ModelObject.CreatedOn = DateTime.Now;
                            ModelObject.CreatedBy = user.UserName;
                            DBX.TrainigModule.Add(ModelObject);
                            DBX.SaveChanges();
                            //  Session["FILES_NAME"] = null;
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListQuestion = DBX.TrainigModule.ToList();
            if (BSM.TrainingType != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.TrainingType == BSM.TrainingType).ToList();
            }

            if (BSM.Course != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.CourseCode == BSM.Course).ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_GridItem", BSM);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(TrainigModule ModelObject)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ClsELearningModule BSM = new ClsELearningModule();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsELearningModule)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {

                try
                {
                    if (
                        ModelObject.TrainingType == null || ModelObject.CourseCode == null ||
                        ModelObject.Topic == null || ModelObject.Description == null ||
                        ModelObject.Category == null || ModelObject.Score == null ||
                        ModelObject.ScorePr == null || ModelObject.Timer == 0
                       )
                    {
                        ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                    }
                    else
                    {
                        var check = DBX.TrainigModule.FirstOrDefault(w => w.ID == ModelObject.ID);
                        if (check != null)
                        {
                            check.Topic = ModelObject.Topic;
                            check.TrainingType = ModelObject.TrainingType;
                            check.CourseCode = ModelObject.CourseCode;
                            check.CourseName = ModelObject.CourseName;
                            check.Description = ModelObject.Description;
                            check.Score = ModelObject.Score;
                            check.ScorePr = ModelObject.ScorePr;
                            check.Target = ModelObject.Target;
                            check.Timer = ModelObject.Timer;
                            check.Category = ModelObject.Category;
                            check.DayTerm = ModelObject.DayTerm;
                            if (Session[PATH_FILE] != null)
                            {
                                check.UrlDocument = Session[PATH_FILE].ToString();
                                Session[PATH_FILE] = null;
                                check.Document = Session[PATH_FILE2].ToString();
                                Session[PATH_FILE2] = null;
                            }
                            if (Session[PATH_FILE1] != null)
                            {
                                //this.DeleteFile(check.Picture);

                                check.UrlOtherDocument = Session[PATH_FILE1].ToString();
                                Session[PATH_FILE1] = null;
                                check.OtherDocument = Session[PATH_FILE3].ToString();
                                Session[PATH_FILE1] = null;
                            }
                            //check.UrlDocument = user.UserName;
                            check.ChangedBy = user.UserName;
                            check.ChangedOn = DateTime.Now;
                            DBX.TrainigModule.Attach(check);
                            DBX.Entry(check).State = System.Data.Entity.EntityState.Modified;
                            int row = DBX.SaveChanges();
                        }
                    }

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            BSM.ListQuestion = DBX.TrainigModule.ToList();
            if (BSM.TrainingType != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.TrainingType == BSM.TrainingType).ToList();
            }

            if (BSM.Course != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.CourseCode == BSM.Course).ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_GridItem", BSM);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int ID)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ClsELearningModule BSM = new ClsELearningModule();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsELearningModule)Session[Index_Sess_Obj + ActionName];
            }
            if (ID > 0)
            {
                try
                {
                    var obj = DBX.TrainigModule.FirstOrDefault(w => w.ID == ID);
                    if (obj != null)
                    {
                        //if (obj.Picture != null)
                        //{
                        //    DeleteFile(obj.Picture);
                        //}
                        //else
                        //{
                        //    obj.Picture = "";
                        //}
                        //string root = Server.MapPath("~/Content/UPLOAD/" + PartCatalogueID);
                        //DeleteDirectory(root);

                        DBX.TrainigModule.Remove(obj);
                        int row = DBX.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListQuestion = DBX.TrainigModule.ToList();
            if (BSM.TrainingType != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.TrainingType == BSM.TrainingType).ToList();
            }

            if (Session["Course"] != null)
            {
                string course = Session["Course"].ToString();
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.CourseCode == course).ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_GridItem", BSM);
        }
        #endregion

        #region "Upload Image"
        [HttpPost]
        public ActionResult UploadControlCallbackActionImage(HttpPostedFileBase[] file_Uploader)
        {
            UserSession();
            DataSelector();
            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("uc_image",
                this.ValidationSettings,
                this.uc_FileUploadCompleteM);
            return null;
        }
        public ActionResult UploadControlCallbackActionImage1()
        {
            UserSession();
            DataSelector();
            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("uc_image1",
                this.ValidationSettings,
                this.uc_FileUploadCompleteS);
            return null;
        }
        public string DeleteFile(string path)
        {
            //string test = "9586-202-10072";
            //string result = FileName.Substring(FileName.LastIndexOf('/') + 1);

            try
            {
                if (path != null)
                {
                    //    var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
                    //    string root = HostingEnvironment.ApplicationPhysicalPath;
                    //    obj.UpoadPath = obj.UpoadPath.Replace("~", "").Replace("/", "\\");
                    //obj.UpoadPath = obj.UpoadPath.Replace("/IMG", "");
                    string fileNameDelete = path;
                    if (System.IO.File.Exists(fileNameDelete))
                    {
                        System.IO.File.Delete(fileNameDelete);
                    }
                }
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        //[HttpPost]
        //public string FitlerType(string code, int addType)
        //{

        //    UserSession();
        //    //UserConfListAndForm();
        //    if (addType == 1)
        //    {
        //        Session["TrainingType"] = code;
        //    }
        //    else if (addType == 2)
        //    {
        //        Session["Course"] = code;
        //    }
        //    else if (addType == 4)
        //    {
        //        Session["Module"] = code;
        //    }
        //    else if (addType == 3)
        //    {
        //        Session["Video"] = code;
        //    }

        //    ActionName = "Index";
        //    ClsELearningModule BSM = new ClsELearningModule();
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        BSM = (ClsELearningModule)Session[Index_Sess_Obj + ActionName];
        //    }
        //    if (Session["TrainingType"] != null)
        //    {
        //        string TrainingType = Session["TrainingType"].ToString();
        //        var District = DBX.TrainingCourseMaster.Where(w => w.Program == TrainingType).ToList();
        //        BSM.ListCourseMaster = District.ToList();
        //    }
        //    Session[Index_Sess_Obj + ActionName] = BSM;
        //    return SYConstant.OK;
        //}

        #region Attachment File
        [HttpPost]
        public ActionResult Attachment(ClsELearningModule BSM, HttpPostedFileBase[] file_Uploader)
        {
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsELearningModule)Session[Index_Sess_Obj + ActionName];
            }

            if (BSM.Header.TrainingType != null && BSM.Header.CourseCode != null && BSM.Header.Topic != null)
            {
                string root = Server.MapPath("~/Content/UPLOAD/TRAINING/DOC/" + BSM.Header.TrainingType + "/" + BSM.Header.CourseCode + "/" + BSM.Header.Topic);
                System.IO.DirectoryInfo di = new DirectoryInfo(root);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                //DeleteDirectory(root);

                UploadedFile[] files = UploadControlExtension.GetUploadedFiles("UploadControl",
                this.ValidationSettings,
                this.uc_FileUploadComplete);
                string TrainingType = Session["TrainingType"].ToString();
                string Course = Session["Course"].ToString();
                string Video = Session["Video"].ToString();
                string Module = Session["Module"].ToString();
                string p = "~/Content/UPLOAD/TRAINING/DOC/";

                string doc = "";
                string Url = "";
                foreach (var r in files)
                {
                    if (TrainingType != "" && Course != "")
                    {
                        if (Video == "V")
                        {
                            p = "~/Content/UPLOAD/TRAINING/VIDEO/";
                        }
                        root = Server.MapPath(p + TrainingType + "/" + Course + "/" + Module + "/" + r.FileName);
                        doc += r.FileName + ";";
                        Url += root + ";";
                    }
                }
                //var check = DBX.TrainigModule.Where(w=>w.Branch=="SV" && w.TrainingType==TrainingType && w.CourseCode==Course && w.Topic==Module).ToList();
                //if(check.Count > 0)
                //{
                //    check.First().Document = doc;
                //    check.First().UrlDocument = Url;
                //    check.First().ChangedBy = user.UserName;
                //    check.First().ChangedOn = DateTime.Now;
                //    DBX.TrainigModule.Attach(check.First());
                //    DBX.Entry(check).State = System.Data.Entity.EntityState.Modified;
                //    int row = DBX.SaveChanges();
                //}

            }


            return View(BSM);
        }
        #endregion

        #region Upload Picture
        public ActionResult Upload()
        {
            UserSession();
            DataSelector();
            Session["TrainingType"] = "";
            Session["Course"] = "";
            Session["Video"] = "D";
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "NTrainingRequest", SYSConstant.DEFAULT_UPLOAD_LIST);
            ClsELearningModule product = new ClsELearningModule();
            return View(product);
        }
        [HttpPost]
        public ActionResult Upload(ClsELearningModule collection, HttpPostedFileBase[] file_Uploader)
        {
            DataSelector();
            //IUrlResolutionService urlResolver = collection as IUrlResolutionService;
            string TrainingType = Session["TrainingType"].ToString();
            string Course = Session["Course"].ToString();
            string Video = Session["Video"].ToString();
            string Module = Session["Module"].ToString();
            string p = "~/Content/UPLOAD/TRAINING/DOC/";
            if (TrainingType != "" && Course != "")
            {
                if (Video == "V")
                {
                    p = "~/Content/UPLOAD/TRAINING/VIDEO/";
                }
                string root = Server.MapPath(p + TrainingType + "/" + Course + "/" + Module);
                DeleteDirectory(root);

                UploadedFile[] files = UploadControlExtension.GetUploadedFiles("UploadControl",
                this.ValidationSettings,
                this.uc_FileUploadComplete);
                string doc = "";
                string Url = "";
                foreach (var r in files)
                {
                    if (TrainingType != "" && Course != "")
                    {

                        root = SYUrl.getBaseUrl() + "/Content/UPLOAD/TRAINING/DOC/" + TrainingType + "/" + Course + "/" + Module + "/" + r.FileName;
                        doc += r.FileName + ";";
                        Url += root + ";";
                    }
                }
                var check = DBX.TrainigModule.Where(w => w.TrainingType == TrainingType && w.CourseCode == Course && w.Topic == Module).ToList();
                if (check.Count > 0)
                {
                    check.First().Document = doc;
                    check.First().UrlDocument = Url;
                    DBX.TrainigModule.Attach(check.First());
                    DBX.Entry(check.First()).State = System.Data.Entity.EntityState.Modified;
                    int row = DBX.SaveChanges();
                }
            }


            return View(collection);
        }
        private void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(path);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }
        public void uc_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                //string resultFilePath = HttpContext.Current.Request.MapPath(UploadDirectory + e.UploadedFile.FileName);
                string TrainingType = Session["TrainingType"].ToString();
                string Course = Session["Course"].ToString();
                string Video = Session["Video"].ToString();
                string Module = Session["Module"].ToString();
                string p = "~/Content/UPLOAD/TRAINING/DOC/";
                if (TrainingType != "" && Course != "")
                {
                    if (Video == "V")
                    {
                        p = "~/Content/UPLOAD/TRAINING/VIDEO/";
                    }
                    Directory.CreateDirectory(Server.MapPath(p + TrainingType + "/" + Course + "/" + Module));
                    string resultFilePath = Server.MapPath(p + TrainingType + "/" + Course + "/" + Module + "/" + e.UploadedFile.FileName);

                    //SMMasterData DB = new SMMasterData();

                    e.UploadedFile.SaveAs(resultFilePath);//Code Central Mode - Uncomment This Line

                    IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                    if (urlResolver != null)
                    {
                        e.CallbackData = urlResolver.ResolveClientUrl(resultFilePath);
                    }
                    else
                    {
                        e.ErrorText = SYMessages.getMessage("FILE_EXIST");
                    }

                }

            }
        }
        public void uc_FileUploadCompleteM(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
                var objFile = new SYFileImportImage(path);
                objFile.TokenKey = ClsCrypo.GetUniqueKey(15);
                //string resultFilePath = HttpContext.Current.Request.MapPath(UploadDirectory + e.UploadedFile.FileName);

                string[] sp = e.UploadedFile.FileName.Split('.');

                string extendtion = "";
                string filename = "";
                if (sp.Length > 0)
                {
                    extendtion = sp[sp.Length - 1];
                    filename = objFile.TokenKey + "." + extendtion;
                }
                else
                {
                    filename = e.UploadedFile.FileName;
                }
                string resultFilePath = Server.MapPath(path.PathDirectory + filename);

                //SMMasterData DB = new SMMasterData();

                e.UploadedFile.SaveAs(resultFilePath);//Code Central Mode - Uncomment This Line

                IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                if (urlResolver != null)
                {
                    //objFile.ObjectTemplate.UpoadPath.Replace("~", "");
                    e.CallbackData = urlResolver.ResolveClientUrl(resultFilePath);
                    Session[PATH_FILE2] += e.UploadedFile.FileName + ";";
                    Session[PATH_FILE] += SYUrl.getBaseUrl() + path.PathDirectory.Replace("~", "") + "/" + filename + ";";
                }
                else
                {
                    e.ErrorText = SYMessages.getMessage("FILE_EXIST");
                }


            }
        }
        public void uc_FileUploadCompleteS(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
                var objFile = new SYFileImportImage(path);
                objFile.TokenKey = ClsCrypo.GetUniqueKey(15);
                //string resultFilePath = HttpContext.Current.Request.MapPath(UploadDirectory + e.UploadedFile.FileName);

                string[] sp = e.UploadedFile.FileName.Split('.');

                string extendtion = "";
                string filename = "";
                if (sp.Length > 0)
                {
                    extendtion = sp[sp.Length - 1];
                    filename = objFile.TokenKey + "." + extendtion;
                }
                else
                {
                    filename = e.UploadedFile.FileName;
                }
                string resultFilePath = Server.MapPath(path.PathDirectory + filename);

                //SMMasterData DB = new SMMasterData();

                e.UploadedFile.SaveAs(resultFilePath);//Code Central Mode - Uncomment This Line

                IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                if (urlResolver != null)
                {
                    //objFile.ObjectTemplate.UpoadPath.Replace("~", "");
                    e.CallbackData = urlResolver.ResolveClientUrl(resultFilePath);
                    Session[PATH_FILE3] += e.UploadedFile.FileName + ";";
                    Session[PATH_FILE1] += SYUrl.getBaseUrl() + path.PathDirectory.Replace("~", "") + "/" + filename + ";";
                }
                else
                {
                    e.ErrorText = SYMessages.getMessage("FILE_EXIST");
                }


            }
        }
        public readonly UploadControlValidationSettings ValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png", ".pdf", ".xls", ".xlsx", ".doc", ".docx", ".pptx", ".ppt", ".mp4" },
            MaxFileSize = 1024000000,
        };
        #endregion

        #region "Import & Upload"
        public ActionResult Import()
        {
            UserSession();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "TRELearningModule", SYSConstant.DEFAULT_UPLOAD_LIST);
            IEnumerable<Humica.Core.DB.MDUploadTemplate> listu = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate);
            return View(listu.ToList());
        }

        [HttpPost]
        public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "TRELearningModule", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("SALEPRICE_MASTER"));
            sfi.ObjectTemplate = new Humica.Core.DB.MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "SALE";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadMQ",
                sfi.ValidationSettings, sfi.uc_FileUploadComplete);
            ViewData[SYSConstant.DATE_FORMAT] = SYSettings.getSetting(SYSConstant.DATE_FORMAT).SettinValue;
            ViewData[SYSConstant.LIST_CONF_SETTING] = SYSettings.getListConf(SYSConstant.DEFAULT_UPLOAD_LIST, user.Lang);
            IEnumerable<Humica.Core.DB.MDUploadTemplate> listu = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID).OrderByDescending(w => w.UploadDate);
            return PartialView("UploadList", listu.ToList());
            //return null;
        }

        public ActionResult UploadList()
        {
            UserSession();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "TRELearningModule", SYSConstant.DEFAULT_UPLOAD_LIST);
            IEnumerable<Humica.Core.DB.MDUploadTemplate> listu = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate);
            return PartialView(SYListConfuration.ListDefaultUpload, listu.ToList());
        }

        public ActionResult GenerateUpload(string id)
        {
            UserSession();
            if (id.ToString() != "null")
            {
                ClsELearningModule BSM = new ClsELearningModule();
                int ID = Convert.ToInt32(id);
                Humica.Core.DB.MDUploadTemplate obj = DB.MDUploadTemplates.Find(ID);
                if (obj != null)
                {
                    SYExcel excel = new SYExcel();
                    excel.FileName = obj.UpoadPath;
                    DataTable dt = excel.GenerateExcelData();
                    if (obj.IsGenerate == true)
                    {
                        SYMessages mess = SYMessages.getMessageObject("FILE_RG", user.Lang);
                        Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                    }
                    if (dt != null)
                    {

                        try
                        {
                            if (Session[Index_Sess_Obj + ActionName] != null)
                            {
                                BSM = (ClsELearningModule)Session[Index_Sess_Obj + ActionName];
                            }
                            BSM.ListQuestion = new List<TrainigModule>();
                            string msg = SYConstant.OK;
                            //var ListSalePrice = DP.SPMDPriceSetupItems.ToList();
                            DateTime create = DateTime.Now;
                            if (dt.Rows.Count > 0)
                            {
                                string error = "";
                                Humica.Core.DB.HumicaDBContext DBB = new Humica.Core.DB.HumicaDBContext();
                                int line = 1;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    TrainigModule objAdd = new TrainigModule();
                                    objAdd.ID = line;
                                    objAdd.TrainingType = dt.Rows[i][0].ToString();
                                    objAdd.CourseCode = dt.Rows[i][1].ToString();
                                    objAdd.Category = dt.Rows[i][2].ToString();
                                    objAdd.Description = dt.Rows[i][4].ToString();
                                    objAdd.Topic = dt.Rows[i][5].ToString();
                                    objAdd.Document = dt.Rows[i][6].ToString();
                                    //objAdd.OtherDocument = dt.Rows[i][7].ToString();
                                    objAdd.Score = Convert.ToDecimal(dt.Rows[i][7]);
                                    objAdd.ScorePr = Convert.ToDecimal(dt.Rows[i][8]);
                                    objAdd.Target = Convert.ToInt32(dt.Rows[i][9]);
                                    objAdd.DayTerm = dt.Rows[i][10].ToString();
                                    //objAdd.Startdate = Convert.ToDateTime(dt.Rows[i][11].ToString());
                                    //objAdd.Enddate = Convert.ToDateTime(dt.Rows[i][12].ToString());
                                    objAdd.Timer = Convert.ToDecimal(dt.Rows[i][11]);
                                    objAdd.CreatedOn = create;
                                    objAdd.CreatedBy = user.UserName;

                                    line++;
                                    var ObjSaleprice = BSM.ListQuestion.Where(w => w.TrainingType == objAdd.TrainingType && w.CourseCode == objAdd.CourseCode && w.Topic == objAdd.Topic).ToList();
                                    if (ObjSaleprice.Count == 0)
                                    {
                                        BSM.ListQuestion.Add(objAdd);
                                        //error = objAdd.MaterialCode;
                                        //msg = "MAT_NE";
                                        //obj.Message = SYMessages.getMessage(msg) + ":" + error;
                                        //obj.IsGenerate = false;
                                        //DB.MDUploadTemplates.Attach(obj);
                                        //DB.Entry(obj).Property(w => w.Message).IsModified = true;
                                        //DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                        //DB.SaveChanges();
                                        //return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                                    }
                                }
                                msg = BSM.ImportQuestion();
                                if (msg == SYConstant.OK)
                                {
                                    // BSQuotaConf Configure = new BSQuotaConf(DOC_TYPE, true);
                                    obj.Message = SYConstant.OK;
                                    //obj.DocumentNo = Configure.NextNumberRank;
                                    obj.IsGenerate = true;
                                    DBB.MDUploadTemplates.Attach(obj);
                                    DBB.Entry(obj).Property(w => w.Message).IsModified = true;
                                    DBB.Entry(obj).Property(w => w.DocumentNo).IsModified = true;
                                    DBB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                    //int row1 = DP.SaveChanges();
                                    int row = DBB.SaveChanges();

                                    string p = "~/Content/UPLOAD/TRAINING/DOC/";
                                    foreach (var r in BSM.ListQuestion)
                                    {
                                        if (r.Document != null && r.TrainingType != null && r.CourseCode != null && r.Topic != null)
                                        {
                                            string root = Server.MapPath(p + r.TrainingType + "/" + r.CourseCode + "/" + r.Topic);
                                            DeleteDirectory(root);

                                        }
                                    }

                                }
                                else
                                {
                                    obj.Message = SYMessages.getMessage(msg) + ":" + error;
                                    obj.IsGenerate = false;
                                    DB.MDUploadTemplates.Attach(obj);
                                    DB.Entry(obj).Property(w => w.Message).IsModified = true;
                                    DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                    DB.SaveChanges();
                                }
                            }
                        }
                        catch (DbUpdateException e)
                        {
                            /*------------------SaveLog----------------------------------*/
                            SYEventLog log = new SYEventLog();
                            log.ScreenId = SCREEN_ID;
                            log.UserId = user.UserID.ToString();
                            log.DocurmentAction = "UPLOAD";
                            log.Action = SYActionBehavior.ADD.ToString();

                            SYEventLogObject.saveEventLog(log, e, true);
                            /*----------------------------------------------------------*/
                            obj.Message = e.Message;
                            obj.IsGenerate = false;
                            DB.MDUploadTemplates.Attach(obj);
                            DB.Entry(obj).Property(w => w.Message).IsModified = true;
                            DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                            DB.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            obj.Message = e.Message;
                            obj.IsGenerate = false;
                            DB.MDUploadTemplates.Attach(obj);
                            DB.Entry(obj).Property(w => w.Message).IsModified = true;
                            DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                            DB.SaveChanges();
                        }
                    }

                }
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
        }


        public ActionResult DownloadTemplate(string ID)
        {

            string fileName = Server.MapPath("~/Content/TEMPLATE/NService/TrainingModule.xlsx");
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=TrainingModule.xlsx");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.WriteFile(fileName);
            Response.End();
            return null;
        }

        #endregion

        #region "Private Code"
        private void DataSelector()
        {
            //ViewData["PLANT"] = DB.CFPlants.ToList();
            //ViewData["ACCOUNT_LIST"] = DBX.TrainigModule.Where(w => w.Plant == ClsConstant.DEFAULT_PLANT).ToList();
            //ViewData["BRANCH_SELECT"] = ClsConstant.getLocationDataAccess();
            //ViewData["Program_List"] = DBX.TrainingProgram.ToList();
            //ViewData["Course_List"] = DBX.TrainingCourse.Where(w => w.Branch == "SV").ToList();
            //ViewData["StaffLevel_List"] = SMS.HRLevels.ToList();
            ////ViewData["Category_List"] = DBX.TRTrainingRequirement.Where(w => w.Category == "C").ToList();
            //ViewData["SubCategory_List"] = DBX.TRTrainingRequirement.Where(w => w.Category == "S").ToList();
            //ViewData["DayTerm_List"] = DBX.TRTrainingRequirement.Where(w => w.Category == "D").ToList();
            //ViewData["UploadType_List"] = DBX.TRTrainingRequirement.Where(w => w.Category == "U").ToList();

            //New
            ViewData["Course_List"] = DBX.TRTrainingCourses.ToList();
            ViewData["Training_List"] = DBX.TRTrainingTypes.ToList();
            ViewData["Topic_List"] = DBX.TRTopics.ToList();
            ViewData["Category_List"] = DBX.TRCourseCategories.ToList();
        }
        #endregion
    }
}