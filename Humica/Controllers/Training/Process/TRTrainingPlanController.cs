using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using Humica.Training;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.Training.Process
{
    public class TRTrainingPlanController : EF.Controllers.MasterSaleController
    {
        const string SCREENID = "TR00000002";
        private const string URL_SCREEN = "/Training/Process/TRTrainingPlan/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREENID;
        private string ActionName = "";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string KeyName = "TrainNo";
        HumicaDBContextTR DBX = new HumicaDBContextTR();

        public TRTrainingPlanController() : base()
        {
            this.ScreendIDControl = SCREENID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region Index
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            //DataSelector();
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.ListTrainingPlan = DBX.TRTrainingPlans.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session["TrainerTypeType"] = null;
            Session["TrainerInfoID"] = null;
            Session["TrainingTypeType"] = null;
            Session["TrainingInfoID"] = null;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            TrainingProcessObject BSM = new TrainingProcessObject();
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.ListTrainingPlan);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ViewData[SYSConstant.PARAM_ID1] = false;
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.TrainingPlan = new TRTrainingPlan();
            BSM.ListTRTrainingSession = new List<TRTrainingSession>();
            BSM.staffProfile = new Core.DB.HRStaffProfile();
            BSM.staffProfile.EmpCode = user.UserName;
            BSM.staffProfile.AllName = user.LoginName;
            BSM.TrainingPlan.Status = SYDocumentStatus.OPEN.ToString();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }

        [HttpPost]
        public ActionResult Create(TrainingProcessObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ViewData[SYSConstant.PARAM_ID1] = false;
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;

            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
                    BSM.TrainingPlan = collection.TrainingPlan;
                    BSM.staffProfile = collection.staffProfile;
                }
                if (Session[PATH_FILE] != null)
                {
                    collection.TRTrainingSession.AttachFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                BSM.ScreenId = SCREENID;
                string msg = BSM.CreateTrainingPlan();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.TrainingPlan.TrainNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);

                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }

            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
                return View(BSM);
            }
        }
        #endregion

        #region Details
        public async Task<ActionResult> Details(int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYConstant.PARAM_ID] = id;
            ViewData[SYSConstant.PARAM_ID1] = true;
            DataSelector();
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.TrainingPlan = new TRTrainingPlan();
            BSM.ListTRTrainingSession = new List<TRTrainingSession>();
            BSM.staffProfile = new Core.DB.HRStaffProfile();

            BSM.TrainingPlan = await DBX.TRTrainingPlans.FindAsync(id);
            if (BSM.TrainingPlan != null)
            {
                BSM.ListTRTrainingSession = await DBX.TRTrainingSessions.Where(x => x.TrainingCalendarID == BSM.TrainingPlan.TrainNo).ToListAsync();
                BSM.GetStaffProfile(BSM.TrainingPlan.RequesterCode);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? trainNo)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ViewData[SYConstant.PARAM_ID] = trainNo;
            ViewData[SYSConstant.PARAM_ID1] = false;
            TrainingProcessObject BSM = new TrainingProcessObject();
            if (trainNo != null)
            {
                BSM.TrainingPlan = new TRTrainingPlan();
                BSM.ListTRTrainingSession = new List<TRTrainingSession>();
                var objCalendar = DBX.TRTrainingPlans;
                BSM.TrainingPlan = objCalendar.Find(trainNo);
                if (BSM.TrainingPlan != null)
                {
                    var objSession = DBX.TRTrainingSessions;
                    BSM.ListTRTrainingSession = objSession.Where(w => w.TrainingCalendarID == trainNo).ToList();
                    BSM.GetStaffProfile(BSM.TrainingPlan.RequesterCode);
                    BSM.CourseName = DBX.TRTrainingCourses.FirstOrDefaultAsync(x => x.Code == BSM.TrainingPlan.CourseID)?.GetAwaiter().GetResult().Description;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(int trainNo, TrainingProcessObject trainingProcessObject)
        {
            try
            {
                ActionName = "Create";
                this.ScreendIDControl = SCREENID;
                UserSession();
                UserConfListAndForm(this.KeyName);
                DataSelector();
                ViewData[SYSConstant.PARAM_ID1] = false;
                TrainingProcessObject BSM = new TrainingProcessObject();
                if (trainNo != null)
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
                        BSM.TrainingPlan = trainingProcessObject.TrainingPlan;
                    }
                    BSM.ScreenId = SCREENID;
                    string msg = BSM.EditTrainingPlan(trainNo);
                    if (msg == SYConstant.OK)
                    {

                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        mess.DocumentNumber = trainNo.ToString();
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
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                return View(BSM);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? trainNo)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();

            if (trainNo != null)
            {
                int TranNo = Convert.ToInt32(trainNo);
                TrainingProcessObject Del = new TrainingProcessObject();
                string msg = Del.DeleteTrainingPlan(TranNo);
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

        public ActionResult ReleaseDoc(string TrainNo)
        {
            UserSession();
            TrainingProcessObject BSM = new TrainingProcessObject();
            if (!string.IsNullOrEmpty(TrainNo))
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
                }

                int Tran = Convert.ToInt32(TrainNo);
                string msg = BSM.ReleaseDoc(Tran);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = TrainNo;
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + TrainNo;
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


        #region gridview session
        public ActionResult gvSession()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.ListTRTrainingSession = new List<TRTrainingSession>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
            }

            return PartialView("gvSession", BSM.ListTRTrainingSession);
        }
        public ActionResult CreateSession(TRTrainingSession ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TrainingProcessObject BSM = new TrainingProcessObject();
            if (ModelState.IsValid)
            {
                try
                {
                    int sessionTrainNo = 0;
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
                        string trainerID = ModelObject.TrainerID;
                        string _SessionTrainNo = string.Empty;

                        using (HumicaDBContextTR dBContext = new HumicaDBContextTR())
                        {
                            var objTrainerInfo = dBContext.TRTrainerInfo;
                            var _ObjSession = dBContext.TRTrainingSessions;
                            TRTrainerInfo trainerInfo = objTrainerInfo.Where(w => w.TrainNo.ToString() == trainerID).FirstOrDefault();
                            List<TRTrainingSession> trainingSessions = new List<TRTrainingSession>();
                            trainingSessions = _ObjSession.ToList();
                            if (trainingSessions.Count() > 0)
                            {
                                _SessionTrainNo = trainingSessions.Max(w => int.Parse(w.TrainNo)).ToString();
                                sessionTrainNo = int.Parse(_SessionTrainNo);
                            }
                            if (trainerInfo != null)
                            {
                                ModelObject.TrainerID = trainerID;//trainerInfo.TrainerCode;
                            }
                        }
                    }
                    if (Session[PATH_FILE] != null)
                    {
                        ModelObject.AttachFile = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    if (BSM.ListTRTrainingSession.Count == 0)
                    {
                        ModelObject.TrainNo = (++sessionTrainNo).ToString();
                    }
                    else
                    {
                        sessionTrainNo = int.Parse(BSM.ListTRTrainingSession.Max(w => w.TrainNo));
                        ModelObject.TrainNo = (++sessionTrainNo).ToString();
                    }
                    BSM.ListTRTrainingSession.Add(ModelObject);
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
            Session["TrainerTypeType"] = ModelObject.TrainerType;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("gvSession", BSM.ListTRTrainingSession);
        }
        public ActionResult EditSession(TRTrainingSession trainingSessionObj)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TrainingProcessObject BSM = new TrainingProcessObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                try
                {
                    BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
                    string trainerID = Session["TrainerInfoID"].ToString();
                    using (HumicaDBContextTR dBContext = new HumicaDBContextTR())
                    {
                        var objTrainerInfo = dBContext.TRTrainerInfo;
                        TRTrainerInfo trainerInfo = objTrainerInfo.Where(w => w.TrainNo.ToString() == trainerID).FirstOrDefault();
                        if (trainerInfo != null)
                        {
                            trainingSessionObj.TrainerID = trainerID;//trainerInfo.TrainerCode;
                        }
                    }
                    var objUpdate = DBX.TRTrainingSessions.Find(trainingSessionObj.TrainNo);
                    if (Session[PATH_FILE] != null)
                    {
                        trainingSessionObj.AttachFile = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    else
                    {
                        trainingSessionObj.AttachFile = objUpdate.AttachFile;
                    }
                    BSM.EditSession(trainingSessionObj);

                    Session[Index_Sess_Obj + ActionName] = BSM;

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            Session["TrainerTypeType"] = trainingSessionObj.TrainerType;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("gvSession", BSM.ListTRTrainingSession);
        }
        public ActionResult DeleteSession(string trainNo)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TrainingProcessObject BSM = new TrainingProcessObject();

            if (!string.IsNullOrEmpty(trainNo))
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
                    if (BSM.ListTRTrainingSession.Count > 0)
                    {
                        var objCheck = BSM.ListTRTrainingSession.FirstOrDefault(w => w.TrainNo == trainNo);
                        BSM.ListTRTrainingSession.Remove(objCheck);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }
            }
            return PartialView("gvSession", BSM.ListTRTrainingSession);
        }
        #endregion
        public ActionResult GetTrainerInfo()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "TRTrainingPlan", Action = "GetTrainerInfo" };
                cboProperties.ValueField = "TrainerCode";
                cboProperties.TextField = "TrainerName";
                cboProperties.TextFormatString = "{1}";
                cboProperties.Columns.Add("TrainerCode", SYSettings.getLabel("Code"));
                cboProperties.Columns.Add("TrainerName", SYSettings.getLabel("Name"));
                cboProperties.BindList(TrainingProcessObject.GetTrainerInfo());
            });
        }
        public ActionResult SelectTrainnerType(string Actionname, string TrainerType)
        {
            ActionName = Actionname;
            Session["TrainerTypeType"] = TrainerType;
            UserSession();
            TrainingProcessObject BSM = new TrainingProcessObject();
            ViewData[SYSConstant.PARAM_ID] = TrainerType;
            if (Session[Index_Sess_Obj + this.ActionName] != null)
            {
                BSM = (TrainingProcessObject)Session[Index_Sess_Obj + this.ActionName];
            }
            var data = new
            {
                MS = SYSConstant.OK
            };
            return Json(data, (JsonRequestBehavior)1);

        }
        public string cboTrainerID_SelectedIndexChanged(int? TrainerID)
        {

            UserSession();
            UserConfListAndForm();
            //if (!string.IsNullOrEmpty(code))
            //{
            //    Session["InYear"] = code;
            //}
            //else
            //{
            //    Session["InYear"] = null;
            //}
            Session["TrainerInfoID"] = TrainerID;
            return SYConstant.OK;
        }
        
        public void GetAllTrainerInfo()
        {

            List<TRTrainerInfo> HRList = new List<TRTrainerInfo>();
            string trainerTypeCode = Session["TrainerTypeType"].ToString();
            if (trainerTypeCode != null && trainerTypeCode != "")
            {
                HRList = DBX.TRTrainerInfo.Where(w => w.TrainerTypeID == trainerTypeCode).ToList();
            }
            ViewData["TRAINNER"] = HRList;
        }
        public async Task<ActionResult> Course()
        {
            UserSession();
            UserConfListAndForm();
            if (Session["InYear"] != null)
            {
                string inYear = Session["InYear"].ToString();
                TRTrainingCatalogue catalogue = await DBX.TRTrainingCatalogues.FirstOrDefaultAsync(w => w.InYear.ToString() == inYear);
                int trainNo = catalogue == null ? 0 : catalogue.TrainNo;
                var courses = (from a in DBX.TRTrainingCatalogueCourses
                               join b in DBX.TRTrainingCourses on a.TrainingCourseID equals b.TrainNo
                               where a.TrainingCatalogueID == trainNo
                               orderby b.Description
                               select new
                               {
                                   b.Code,
                                   b.Description
                               }).ToList();
                ViewData["COURSE"] = courses;

                return PartialView("Course");
            }
            return null;
        }
        private DataTable GetTrainingCatalogues()
        {
            var trCatalogues = DBX.TRTrainingCatalogues;
            //create a new datatable
            DataTable table = new DataTable();
            //declare datacolumn and datarow object object
            DataColumn column;
            DataRow row;
            // Create new DataColumn, set DataType, ColumnName and add to DataTable
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "InYearName";
            table.Columns.Add(column);
            //add second column
            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "InYear";
            table.Columns.Add(column);
            foreach (var item in trCatalogues.ToList().OrderBy(x => x.InYear).Select(x => x.InYear).Distinct())
            {
                row = table.NewRow();
                row["InYearName"] = item.Value.ToString();
                row["InYear"] = item.Value;

                table.Rows.Add(row);
            }
            return table;
        }
        public string SelectYear(string code)
        {

            UserSession();
            UserConfListAndForm();
            if (!string.IsNullOrEmpty(code))
            {
                Session["InYear"] = code;
            }
            else
            {
                Session["InYear"] = null;
            }
            return SYConstant.OK;
        }
        public string OnGridFocusedRowChanged(string TrainerType, string TrainerInfoID)
        {

            UserSession();
            UserConfListAndForm();
            Session["TrainerInfoID"] = null;
            if (!string.IsNullOrEmpty(TrainerType))
            {
                Session["TrainerTypeType"] = TrainerType;
            }
            else
            {
                Session["TrainerTypeType"] = null;
            }
            if (!string.IsNullOrEmpty(TrainerInfoID))
            {
                Session["TrainerInfoID"] = TrainerInfoID;
            }
            return SYConstant.OK;
        }
        public string OnBeginCallback(string TrainerType)
        {

            UserSession();
            UserConfListAndForm();
            Session["TrainerTypeType"] = null;
            //if (!string.IsNullOrEmpty(TrainerType))
            //{
            //    Session["TrainerTypeType"] = TrainerType;
            //}
            //else
            //{
            //    Session["TrainerTypeType"] = null;
            //}
            return SYConstant.OK;
        }
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {
            Core.DB.HumicaDBContext DB = new Core.DB.HumicaDBContext();
            ActionName = "Create";
            var tbl = DB.HRStaffProfiles;
            var staffProfile = tbl.ToList();
            TrainingProcessObject BSM = new TrainingProcessObject();
            Core.DB.HRStaffProfile staff = staffProfile.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (staff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = staff.AllName
                };

                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
                    BSM.staffProfile = staff;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }

        public ActionResult CreateItem(TRTrainingSession ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TrainingProcessObject BSM = new TrainingProcessObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
            }
            if (ModelState.IsValid)
            {
                try
                {

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
            return PartialView("gvtrainee", BSM.ListTRTrainingSession);
        }
        public ActionResult UploadControlCallbackActionImage(HttpPostedFileBase file_Uploader)
        {

            UserSession();
            var path = DBX.CFUploadPaths.Find("IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREENID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "STAFF";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
                sfi.ValidationSettings,
                sfi.uc_FileUploadCompleteFile);
            Session[PATH_FILE] = sfi.ObjectTemplate.UpoadPath;
            return null;
        }
        private void DataSelector()
        {
            ViewData["INYEAR"] = GetTrainingCatalogues();
            ViewData["COURSE_CATEGORY"] = DBX.TRCourseCategories.ToList();
            //ViewData["TRAINNER"] = DBX.TRTrainingType.ToList();
            ViewData["VENUE"] = DBX.TRTrainingVenues.ToList();
            SYDataList objList = new SYDataList("TRAINING_SESSION");
            ViewData["SESSION"] = objList.ListData;
            var list_ = objList.ListData;
            SYDataList DL = new SYDataList("TRAINER_TYPE");
            ViewData["LIST_TRAINERTYPE"] = DL.ListData;
            ViewData["Topic"] = DBX.TRTopics.ToList();
            ViewData["TRAINING_TYPE"] = DBX.TRTrainingTypes.ToList();
            ViewData["TRAINING_CATE"] = DBX.TRTrainingCategories.ToList();

        }
    }
}
