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
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.Training.SetUp
{
    public class TRELearningAnswerController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "TRM000003";
        private const string URL_SCREEN = "/Training/Setup/TRELearningAnswer/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "AnswerCode";

        HumicaDBContextTR DB = new HumicaDBContextTR();
        SMSystemEntity SMS = new SMSystemEntity();
        Humica.Core.DB.HumicaDBContext DBC = new Humica.Core.DB.HumicaDBContext();
        Humica.Core.DB.HumicaDBViewContext DBV = new Humica.Core.DB.HumicaDBViewContext();
        public TRELearningAnswerController()
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
            Session["TrainingType"] = null;
            Session["Course"] = null;
            UserConfList(KeyName);
            ClsTRELearningAnswer BSM = new ClsTRELearningAnswer();
            BSM.ListAnswer = new List<TRELearningAnswer>();
            BSM.ListModule = new List<TrainigModule>();
            BSM.ListQuestion = new List<TRELearningQuestion>();
            BSM.ListAnswer = DB.TRELearningAnswer.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Index(ClsTRELearningAnswer collection)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfList(KeyName);
            ClsTRELearningAnswer BSM = new ClsTRELearningAnswer();
            BSM.ListAnswer = DB.TRELearningAnswer.ToList();
            BSM.TrainingType = collection.TrainingType;
            BSM.Course = collection.Course;
            if (BSM.Course != null)
            {
                BSM.ListAnswer = BSM.ListAnswer.Where(w => w.CourseCode == BSM.Course).ToList();
            }
            if (BSM.TrainingType != null)
            {
                BSM.ListAnswer = BSM.ListAnswer.Where(w => w.TrainingType == BSM.TrainingType).ToList();
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
            ClsTRELearningAnswer BSM = new ClsTRELearningAnswer();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsTRELearningAnswer)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListAnswer = DB.TRELearningAnswer.ToList();
            if (BSM.TrainingType != null)
            {
                BSM.ListAnswer = BSM.ListAnswer.Where(w => w.TrainingType == BSM.TrainingType).ToList();
            }
            if (BSM.Course != null)
            {
                BSM.ListAnswer = BSM.ListAnswer.Where(w => w.CourseCode == BSM.Course).ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_GridItem", BSM);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TRELearningAnswer ModelObject)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsTRELearningAnswer BSM = new ClsTRELearningAnswer();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsTRELearningAnswer)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRELearningAnswer.FirstOrDefault(w => w.TrainingType == ModelObject.TrainingType && w.CourseCode == ModelObject.CourseCode && w.Topic == ModelObject.Topic && w.QuestionCode == ModelObject.QuestionCode && w.AnswerCode == ModelObject.AnswerCode);

                    if (check != null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED", user.Lang);
                    }
                    else
                    {
                        if (ModelObject.TrainingType == null || ModelObject.CourseCode == null ||
                         ModelObject.Topic == null || ModelObject.QuestionCode == null ||
                         ModelObject.Description == null || ModelObject.AnswerCode == null ||
                         ModelObject.TotalScore == null || ModelObject.ColumnCheck == null ||
                         ModelObject.CorrectValue == null)
                        {
                            ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                        }
                        else
                        {
                            ModelObject.CreatedOn = DateTime.Now;
                            ModelObject.CreatedBy = user.UserName;
                            DB.TRELearningAnswer.Add(ModelObject);
                            DB.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListAnswer = DB.TRELearningAnswer.ToList();
            if (BSM.TrainingType != null)
            {
                BSM.ListAnswer = BSM.ListAnswer.Where(w => w.TrainingType == BSM.TrainingType).ToList();
            }
            if (BSM.Course != null)
            {
                BSM.ListAnswer = BSM.ListAnswer.Where(w => w.CourseCode == BSM.Course).ToList();
            }
            BSM.ListModule = new List<TrainigModule>();
            BSM.ListQuestion = new List<TRELearningQuestion>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_GridItem", BSM);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(TRELearningAnswer ModelObject)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ClsTRELearningAnswer BSM = new ClsTRELearningAnswer();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsTRELearningAnswer)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRELearningAnswer.FirstOrDefault(w => w.ID == ModelObject.ID);
                    if (check != null)
                    {
                        if (ModelObject.TrainingType == null || ModelObject.CourseCode == null ||
                         ModelObject.Topic == null || ModelObject.QuestionCode == null ||
                         ModelObject.Description == null || ModelObject.AnswerCode == null ||
                         ModelObject.TotalScore == null || ModelObject.ColumnCheck == null ||
                         ModelObject.CorrectValue == null)
                        {
                            ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                        }
                        else
                        {
                            check.TrainingType = ModelObject.TrainingType;
                            check.CourseCode = ModelObject.CourseCode;
                            check.QuestionCode = ModelObject.QuestionCode;
                            check.Topic = ModelObject.Topic;
                            check.AnswerCode = ModelObject.AnswerCode;
                            check.Description = ModelObject.Description;
                            check.TotalScore = ModelObject.TotalScore;
                            check.CorrectValue = ModelObject.CorrectValue;
                            check.ColumnCheck = ModelObject.ColumnCheck;
                            check.ChangedBy = user.UserName;
                            check.ChangedOn = DateTime.Now;
                            DB.TRELearningAnswer.Attach(check);
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

            BSM.ListAnswer = DB.TRELearningAnswer.ToList();
            if (BSM.TrainingType != null)
            {
                BSM.ListAnswer = BSM.ListAnswer.Where(w => w.TrainingType == BSM.TrainingType).ToList();
            }
            if (BSM.Course != null)
            {
                BSM.ListAnswer = BSM.ListAnswer.Where(w => w.CourseCode == BSM.Course).ToList();
            }
            BSM.ListModule = new List<TrainigModule>();
            BSM.ListQuestion = new List<TRELearningQuestion>();
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
            ClsTRELearningAnswer BSM = new ClsTRELearningAnswer();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsTRELearningAnswer)Session[Index_Sess_Obj + ActionName];
            }
            if (ID > 0)
            {
                try
                {
                    var obj = DB.TRELearningAnswer.FirstOrDefault(w => w.ID == ID);
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

                        DB.TRELearningAnswer.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListAnswer = DB.TRELearningAnswer.ToList();
            if (BSM.TrainingType != null)
            {
                BSM.ListAnswer = BSM.ListAnswer.Where(w => w.TrainingType == BSM.TrainingType).ToList();
            }

            if (BSM.Course != null)
            {
                BSM.ListAnswer = BSM.ListAnswer.Where(w => w.CourseCode == BSM.Course).ToList();
            }
            BSM.ListModule = new List<TrainigModule>();
            BSM.ListQuestion = new List<TRELearningQuestion>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_GridItem", BSM);
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
            else if (addType == 3)
            {
                Session["Module"] = code;
            }

            ActionName = "Index";
            ClsTRELearningAnswer BSM = new ClsTRELearningAnswer();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsTRELearningAnswer)Session[Index_Sess_Obj + ActionName];
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
                var model = DB.TrainigModule.Where(w => w.TrainingType == TrainingType && w.CourseCode == Course).ToList();
                BSM.ListModule = model.ToList();

            }
            if (Session["TrainingType"] != null && Session["Course"] != null && Session["Module"] != null)
            {
                string TrainingType = Session["TrainingType"].ToString();
                string Course = Session["Course"].ToString();
                string Module = Session["Module"].ToString();
                var quest = DB.TRELearningQuestion.Where(w => w.TrainingType == TrainingType && w.CourseCode == Course && w.Topic == Module).ToList();
                BSM.ListQuestion = quest.ToList();

            }
            return SYConstant.OK;
        }
        #region "Import & Upload"
        public ActionResult Import()
        {
            UserSession();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "TRELearningAnswer", SYSConstant.DEFAULT_UPLOAD_LIST);
            IEnumerable<Humica.Core.DB.MDUploadTemplate> listu = DBC.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate);
            return View(listu.ToList());
        }

        [HttpPost]
        public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "TRELearningAnswer", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DBC.CFUploadPaths.Find("SALEPRICE_MASTER"));
            sfi.ObjectTemplate = new Core.DB.MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "SALE";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadMQ",
                sfi.ValidationSettings, sfi.uc_FileUploadComplete);
            ViewData[SYSConstant.DATE_FORMAT] = SYSettings.getSetting(SYSConstant.DATE_FORMAT).SettinValue;
            ViewData[SYSConstant.LIST_CONF_SETTING] = SYSettings.getListConf(SYSConstant.DEFAULT_UPLOAD_LIST, user.Lang);
            IEnumerable<Core.DB.MDUploadTemplate> listu = DBC.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID).OrderByDescending(w => w.UploadDate);
            return PartialView("UploadList", listu.ToList());
            //return null;
        }

        public ActionResult UploadList()
        {
            UserSession();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "TRELearningAnswer", SYSConstant.DEFAULT_UPLOAD_LIST);
            IEnumerable<Humica.Core.DB.MDUploadTemplate> listu = DBC.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate);
            return PartialView(SYListConfuration.ListDefaultUpload, listu.ToList());
        }

        public ActionResult GenerateUpload(string id)
        {
            UserSession();
            if (id.ToString() != "null")
            {
                ClsTRELearningAnswer BSM = new ClsTRELearningAnswer();
                int ID = Convert.ToInt32(id);
                Humica.Core.DB.MDUploadTemplate obj = DBC.MDUploadTemplates.Find(ID);
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
                                BSM = (ClsTRELearningAnswer)Session[Index_Sess_Obj + ActionName];
                            }
                            BSM.ListAnswer = new List<TRELearningAnswer>();
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
                                    TRELearningAnswer objAdd = new TRELearningAnswer();
                                    objAdd.ID = line;
                                    objAdd.TrainingType = dt.Rows[i][0].ToString();
                                    objAdd.CourseCode = dt.Rows[i][1].ToString();
                                    objAdd.Topic = dt.Rows[i][2].ToString();
                                    objAdd.QuestionCode = dt.Rows[i][3].ToString();
                                    objAdd.AnswerCode = dt.Rows[i][4].ToString();
                                    objAdd.Description = dt.Rows[i][5].ToString();
                                    objAdd.TotalScore = Convert.ToInt32(dt.Rows[i][6]);
                                    objAdd.ColumnCheck = Convert.ToInt32(dt.Rows[i][7]);
                                    objAdd.CorrectValue = Convert.ToInt32(dt.Rows[i][8]);
                                    objAdd.CreatedOn = create;
                                    objAdd.CreatedBy = user.UserName;

                                    line++;
                                    var ObjSaleprice = BSM.ListAnswer.Where(w => w.TrainingType == objAdd.TrainingType && w.CourseCode == objAdd.CourseCode && w.Topic == objAdd.Topic && w.QuestionCode == objAdd.QuestionCode && w.AnswerCode == objAdd.AnswerCode).ToList();
                                    if (ObjSaleprice.Count == 0)
                                    {
                                        BSM.ListAnswer.Add(objAdd);
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
                                msg = BSM.ImportAnswer();
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

                                }
                                else
                                {
                                    obj.Message = SYMessages.getMessage(msg) + ":" + error;
                                    obj.IsGenerate = false;
                                    DBB.MDUploadTemplates.Attach(obj);
                                    DBB.Entry(obj).Property(w => w.Message).IsModified = true;
                                    DBB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                    DBB.SaveChanges();
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
                            DBC.MDUploadTemplates.Attach(obj);
                            DBC.Entry(obj).Property(w => w.Message).IsModified = true;
                            DBC.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                            DBC.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            obj.Message = e.Message;
                            obj.IsGenerate = false;
                            DBC.MDUploadTemplates.Attach(obj);
                            DBC.Entry(obj).Property(w => w.Message).IsModified = true;
                            DBC.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                            DBC.SaveChanges();
                        }
                    }

                }
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
        }


        public ActionResult DownloadTemplate(string ID)
        {

            string fileName = Server.MapPath("~/Content/UPLOAD/TEMPLATE/Answer.xlsx");
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=SparePartSalePrice.xlsx");
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
            //ViewData["Program_List"] = DB.TrainingProgram.ToList();
            //ViewData["Course_List"] = DB.TrainingCourseMaster.ToList();
            //ViewData["Module_List"] = DB.TrainigModule.ToList();
            //ViewData["Question_List"] = DB.TRELearningQuestion.ToList();
            //ViewData["StaffLevel_List"] = SMS.HRLevels.ToList();
        }
        #endregion
    }

}