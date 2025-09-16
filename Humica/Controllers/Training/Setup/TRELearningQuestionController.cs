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
    public class TRELearningQuestionController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "TRM000004";
        private const string URL_SCREEN = "/Training/Setup/TRELearningQuestion/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "QuestionCode";
        private string PATH_FILE = "12313123123sadfsdfsdfs12f";
        private HumicaDBContextTR DB = new HumicaDBContextTR();
        private Humica.Core.DB.HumicaDBContext DBX = new Humica.Core.DB.HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public TRELearningQuestionController()
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
            this.Session[this.PATH_FILE] = null;
            Session["TrainingType"] = null;
            Session["Course"] = null;
            UserConfList(KeyName);
            ClsELearningQuestion BSM = new ClsELearningQuestion();
            BSM.ListModule = new List<TrainigModule>();
            BSM.ListTrainType = new List<TRTrainingType>();
            BSM.ListQuestion = DB.TRELearningQuestion.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Index(ClsELearningQuestion collection)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            this.Session[this.PATH_FILE] = null;
            UserConfList(KeyName);
            ClsELearningQuestion BSM = new ClsELearningQuestion();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsELearningQuestion)Session[Index_Sess_Obj + ActionName];
            }
            BSM.TrainingType = collection.TrainingType;
            BSM.Course = collection.Course;
            BSM.ListQuestion = DB.TRELearningQuestion.ToList();
            if (BSM.Course != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.CourseCode == BSM.Course).ToList();
            }
            if (BSM.TrainingType != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.TrainingType == BSM.TrainingType).ToList();
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
            ClsELearningQuestion BSM = new ClsELearningQuestion();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsELearningQuestion)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListQuestion = DB.TRELearningQuestion.ToList();
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
        public ActionResult Create(TRELearningQuestion ModelObject)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsELearningQuestion BSM = new ClsELearningQuestion();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsELearningQuestion)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRELearningQuestion.FirstOrDefault(w => w.TrainingType == ModelObject.TrainingType && w.CourseCode == ModelObject.CourseCode && w.Topic == ModelObject.Topic && w.QuestionCode == ModelObject.QuestionCode);

                    if (check != null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED", user.Lang);
                    }
                    else
                    {
                        if (ModelObject.TrainingType == null || ModelObject.CourseCode == null ||
                        ModelObject.Topic == null || ModelObject.QuestionCode == null ||
                        ModelObject.Description == null)
                        {
                            ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                        }
                        else
                        {
                            if (Session[PATH_FILE] != null)
                            {
                                ModelObject.Attachement = Session[PATH_FILE].ToString();
                                Session[PATH_FILE] = null;
                            }
                            else ModelObject.Attachement = "";
                            ModelObject.CreatedOn = DateTime.Now;
                            ModelObject.CreatedBy = user.UserName;
                            DB.TRELearningQuestion.Add(ModelObject);
                            DB.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListQuestion = DB.TRELearningQuestion.ToList();
            if (BSM.TrainingType != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.TrainingType == BSM.TrainingType).ToList();
            }
            if (BSM.Course != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.CourseCode == BSM.Course).ToList();
            }
            BSM.ListTrainType = new List<TRTrainingType>();
            BSM.ListModule = new List<TrainigModule>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_GridItem", BSM);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(TRELearningQuestion ModelObject)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ClsELearningQuestion BSM = new ClsELearningQuestion();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsELearningQuestion)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRELearningQuestion.FirstOrDefault(w => w.ID == ModelObject.ID);
                    if (check != null)
                    {
                        if (ModelObject.TrainingType == null || ModelObject.CourseCode == null ||
                        ModelObject.Topic == null || ModelObject.QuestionCode == null ||
                        ModelObject.Description == null)
                        {
                            ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                        }
                        else
                        {
                            check.TrainingType = ModelObject.TrainingType;
                            check.CourseCode = ModelObject.CourseCode;
                            check.QuestionCode = ModelObject.QuestionCode;
                            check.Description = ModelObject.Description;
                            if (Session[PATH_FILE] != null)
                            {
                                check.Attachement = Session[PATH_FILE].ToString();
                                Session[PATH_FILE] = null;
                            }
                            check.ChangedBy = user.UserName;
                            check.ChangedOn = DateTime.Now;
                            DB.TRELearningQuestion.Attach(check);
                            DB.Entry(check).State = System.Data.Entity.EntityState.Modified;
                            int row = DB.SaveChanges();
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

            BSM.ListQuestion = DB.TRELearningQuestion.ToList();
            if (BSM.TrainingType != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.TrainingType == BSM.TrainingType).ToList();
            }
            if (BSM.Course != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.CourseCode == BSM.Course).ToList();
            }
            BSM.ListTrainType = new List<TRTrainingType>();
            BSM.ListModule = new List<TrainigModule>();
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
            ClsELearningQuestion BSM = new ClsELearningQuestion();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsELearningQuestion)Session[Index_Sess_Obj + ActionName];
            }
            if (ID > 0)
            {
                try
                {
                    var obj = DB.TRELearningQuestion.FirstOrDefault(w => w.ID == ID);
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

                        DB.TRELearningQuestion.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListQuestion = DB.TRELearningQuestion.ToList();
            if (BSM.TrainingType != null)
            {
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.TrainingType == BSM.TrainingType).ToList();
            }
            if (Session["Course"] != null)
            {
                string course = Session["Course"].ToString();
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.CourseCode == course).ToList();
            }
            if (Session["TrainingType"] != null)
            {
                string TrainingType = Session["TrainingType"].ToString();
                BSM.ListQuestion = BSM.ListQuestion.Where(w => w.TrainingType == TrainingType).ToList();
            }
            BSM.ListTrainType = new List<TRTrainingType>();
            BSM.ListModule = new List<TrainigModule>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_GridItem", BSM);
        }
        #endregion

        #region "Upload Image"
        [HttpPost]
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();
            DataSelector();
            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }
            var path = DBX.CFUploadPaths.Find("IMG_UPLOAD");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);
            objFile.ObjectTemplate = new Humica.Core.DB.MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREEN_ID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;
            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("uc_image", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            objFile.ObjectTemplate.UpoadPath = objFile.ObjectTemplate.UpoadPath.Replace("~", "");

            Session[PATH_FILE] = SYUrl.getBaseUrl() + objFile.ObjectTemplate.UpoadPath;
            // Session["FILES_NAME"] = objFile.ObjectTemplate.UploadName;
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

        [HttpPost]
        public string FitlerType(string code, int addType)
        {
            UserSession();
            //UserConfListAndForm();
            if (addType == 1)
            {
                Session["Course"] = code;
            }
            else if (addType == 2)
            {
                Session["TrainingType"] = code;
            }
            //else if (addType == 3)
            //{
            //    Session["Module"] = code;
            //}

            ActionName = "Index";
            ClsELearningQuestion BSM = new ClsELearningQuestion();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsELearningQuestion)Session[Index_Sess_Obj + ActionName];
            }
            if (Session["Course"] != null)
            {
                string Course = Session["Course"].ToString();
                var District = DB.TrainigModule.Where(w => w.CourseCode == Course).ToList();
                var ListTrainType = DB.TRTrainingTypes.ToList();
                var _ListTrainType = ListTrainType.Where(w => District.Where(x => x.TrainingType == w.Code).Any()).ToList();
                BSM.ListTrainType = _ListTrainType.ToList();
            }
            if (Session["TrainingType"] != null && Session["Course"] != null)
            {
                string TrainingType = Session["TrainingType"].ToString();
                string Course = Session["Course"].ToString();
                var District = DB.TrainigModule.Where(w => w.TrainingType == TrainingType && w.CourseCode == Course).ToList();
                BSM.ListModule = District.ToList();
            }
            return SYConstant.OK;
        }
        #region Upload Picture
        public ActionResult Upload()
        {
            UserSession();
            DataSelector();
            Session["TrainingType"] = "";
            Session["Course"] = "";
            Session["Module"] = "";
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "NTrainingRequest", SYSConstant.DEFAULT_UPLOAD_LIST);
            ClsELearningModule product = new ClsELearningModule();
            return View(product);
        }
        [HttpPost]
        public ActionResult Upload(ClsELearningModule collection, HttpPostedFileBase[] file_Uploader)
        {
            DataSelector();
            string TrainingType = Session["TrainingType"].ToString();
            string Course = Session["Course"].ToString();
            string Module = Session["Module"].ToString();

            if (TrainingType != "" && Course != "" && Module != "")
            {
                string url = "~/Content/UPLOAD/TRAINING/QUIZ/" + TrainingType + "/" + Course + "/" + Module;
                string root = Server.MapPath(url);
                DeleteDirectory(root);

                UploadedFile[] files = UploadControlExtension.GetUploadedFiles("UploadControl",
                this.ValidationSettings,
                this.uc_FileUploadComplete);
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
                string TrainingType = Session["TrainingType"].ToString();
                string Course = Session["Course"].ToString();
                string Module = Session["Module"].ToString();

                if (TrainingType != "" && Course != "" && Module != "")
                {
                    string url = "~/Content/UPLOAD/TRAINING/QUIZ/" + TrainingType + "/" + Course + "/" + Module;
                    Directory.CreateDirectory(Server.MapPath(url));
                    string resultFilePath = Server.MapPath(url + "/" + e.UploadedFile.FileName);

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
        public readonly UploadControlValidationSettings ValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".gif", ".png" },
            MaxFileSize = 1024000000,
        };
        #endregion

        #region "Import & Upload"
        public ActionResult Import()
        {
            UserSession();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "TRELearningQuestion", SYSConstant.DEFAULT_UPLOAD_LIST);
            IEnumerable<Humica.Core.DB.MDUploadTemplate> listu = DBX.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate);
            return View(listu.ToList());
        }

        [HttpPost]
        public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "TRELearningQuestion", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DBX.CFUploadPaths.Find("SALEPRICE_MASTER"));
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
            IEnumerable<Humica.Core.DB.MDUploadTemplate> listu = DBX.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID).OrderByDescending(w => w.UploadDate);
            return PartialView("UploadList", listu.ToList());
            //return null;
        }

        public ActionResult UploadList()
        {
            UserSession();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "TRELearningQuestion", SYSConstant.DEFAULT_UPLOAD_LIST);
            IEnumerable<Humica.Core.DB.MDUploadTemplate> listu = DBX.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate);
            return PartialView(SYListConfuration.ListDefaultUpload, listu.ToList());
        }

        public ActionResult GenerateUpload(string id)
        {
            UserSession();
            if (id.ToString() != "null")
            {
                ClsELearningQuestion BSM = new ClsELearningQuestion();
                int ID = Convert.ToInt32(id);
                Humica.Core.DB.MDUploadTemplate obj = DBX.MDUploadTemplates.Find(ID);
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
                                BSM = (ClsELearningQuestion)Session[Index_Sess_Obj + ActionName];
                            }
                            BSM.ListQuestion = new List<TRELearningQuestion>();
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
                                    TRELearningQuestion objAdd = new TRELearningQuestion();
                                    objAdd.ID = line;
                                    objAdd.TrainingType = dt.Rows[i][0].ToString();
                                    objAdd.CourseCode = dt.Rows[i][1].ToString();
                                    objAdd.Topic = dt.Rows[i][2].ToString();
                                    objAdd.QuestionCode = dt.Rows[i][3].ToString();
                                    objAdd.Description = dt.Rows[i][4].ToString();
                                    objAdd.Attachement = dt.Rows[i][5].ToString();
                                    objAdd.CreatedOn = create;
                                    objAdd.CreatedBy = user.UserName;

                                    line++;
                                    var ObjSaleprice = BSM.ListQuestion.Where(w => w.TrainingType == objAdd.TrainingType && w.CourseCode == objAdd.CourseCode && w.Topic == objAdd.Topic && w.QuestionCode == objAdd.QuestionCode).ToList();
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
                                    foreach (var item in BSM.ListQuestion)
                                    {
                                        if (item.Attachement != null)
                                        {
                                            string url = "~/Content/UPLOAD/TRAINING/QUIZ/" + item.TrainingType + "/" + item.CourseCode + "/" + item.Topic;
                                            string root = Server.MapPath(url);
                                            DeleteDirectory(root);
                                        }

                                    }



                                }
                                else
                                {
                                    obj.Message = SYMessages.getMessage(msg) + ":" + error;
                                    obj.IsGenerate = false;
                                    DBX.MDUploadTemplates.Attach(obj);
                                    DBX.Entry(obj).Property(w => w.Message).IsModified = true;
                                    DBX.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                    DBX.SaveChanges();
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
                            DBX.MDUploadTemplates.Attach(obj);
                            DBX.Entry(obj).Property(w => w.Message).IsModified = true;
                            DBX.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                            DBX.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            obj.Message = e.Message;
                            obj.IsGenerate = false;
                            DBX.MDUploadTemplates.Attach(obj);
                            DBX.Entry(obj).Property(w => w.Message).IsModified = true;
                            DBX.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                            DBX.SaveChanges();
                        }
                    }

                }
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
        }


        public ActionResult DownloadTemplate(string ID)
        {

            string fileName = Server.MapPath("~/Content/TEMPLATE/NService/Question.xlsx");
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=Question.xlsx");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.WriteFile(fileName);
            Response.End();
            return null;
        }

        #endregion

        #region "Private Code"
        private void DataSelector()
        {
            ViewData["Course_List"] = DB.TRTrainingCourses.ToList();
            ViewData["Training_List"] = DB.TRTrainingTypes.ToList();
        }
        #endregion
    }

}