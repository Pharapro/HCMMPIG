using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Integration.EF;
using Humica.Training;
using Humica.Training.DB;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Humica.Controllers.TrainingN.Process
{
    public class TRTrainingEmployeeController : EF.Controllers.MasterSaleController
    {
        const string SCREEN_ID = "TR00000006";
        private const string URL_SCREEN = "/Training/Process/TRTrainingEmployee/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TrainNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        public SMSystemEntity DP = new SMSystemEntity();
        Core.DB.HumicaDBViewContext DBStaff = new Core.DB.HumicaDBViewContext();
        public TRTrainingEmployeeController()
           : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            DataSelector();
        }
        #region Index
        public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.FTTraining = new Core.FT.FTTraining();
            DateTime InYear = DateTime.Now;
            string Pending = SYDocumentStatus.PENDING.ToString();
            BSM.FTTraining.INYear = InYear.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
                BSM.FTTraining = obj.FTTraining;
            }
            BSM.ListTrainee = await DBX.TRTrainingEmployees.Where(w => w.IsAccept == true && w.InYear == BSM.FTTraining.INYear && w.Status == "ACCEPTED").ToListAsync();
            BSM.ListTraineePeding = DBX.TRTrainingEmployees.Where(w => w.IsInvite == true && w.IsAccept != true || w.Status == Pending).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public async Task<ActionResult> Index(TrainingProcessObject collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.FTTraining = new Humica.Core.FT.FTTraining();
            BSM.FTTraining.INYear = collection.FTTraining.INYear;
            BSM.FTTraining.Course = collection.FTTraining.Course;
            var trainee = DBX.TRTrainingEmployees.Where(x => x.InYear == BSM.FTTraining.INYear && x.IsAccept == true).ToList();
            if (!string.IsNullOrEmpty(BSM.FTTraining.Course))
            {
                trainee = trainee.Where(x => x.CourseID == BSM.FTTraining.Course).ToList();
            }
            BSM.ListTrainee = trainee;
            BSM.ListTraineePeding = await DBX.TRTrainingEmployees.Where(w => w.IsInvite == true && w.IsAccept != true).ToListAsync();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialTrainee()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainingProcessObject BSM = new TrainingProcessObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialTrainee", BSM.ListTrainee);
        }

        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainingProcessObject BSM = new TrainingProcessObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListTraineePeding);
        }
        #endregion
        public ActionResult Details(string id, string EmpCode)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ViewData[SYSConstant.PARAM_ID] = id;
            ViewData[SYSConstant.PARAM_ID1] = true;
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.HeaderInvite = new TRTrainingInvitation();
            BSM.HeaderTrainee = DBX.TRTrainingEmployees.FirstOrDefault(w => w.TrainNo == id.ToString() && w.EmpCode == EmpCode);
            if (BSM.HeaderTrainee != null)
            {
                BSM.HeaderInvite = DBX.TRTrainingInvitations.FirstOrDefault(w => w.TrainNo.ToString() == id);
                BSM.Staff_View = DBStaff.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == BSM.HeaderTrainee.EmpCode);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #region Create
        public ActionResult Create(int id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.HeaderCalender = new TRTrainingCalender();
            BSM.HeaderCalender = DBX.TRTrainingCalenders.FirstOrDefault(w => w.TrainNo == id);

            BSM.ListTempTrainee = DBX.TRTrainingEmployees.Where(x => x.CalendarID == id).OrderBy(x => x.EmpCode).ToList();
            BSM.ListTempTrainee.ToList().ForEach(w => BSM.ListEmpCode.Add(w.EmpCode));
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Create(TrainingProcessObject ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ViewData[SYSConstant.PARAM_ID1] = false;
            TrainingProcessObject BSM = new TrainingProcessObject();
            try
            {
                if (ModelState.IsValid)
                {
                    BSM.ScreenId = SCREEN_ID;
                    try
                    {
                        if (Session[Index_Sess_Obj + ActionName] != null)
                        {
                            BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
                        }
                        string msg = BSM.CreateTrainee();
                        if (msg == SYConstant.OK)
                        {
                            SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);

                            mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Index";
                            ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                            ViewData[SYSConstant.PARAM_ID1] = true;
                            return View(BSM);
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
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                    return View(BSM);
                }
            }
            catch
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
                return View(BSM);
            }
        }
        #endregion

        #region Edit
        public async Task<ActionResult> Edit(int id, string EmpCode)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.HeaderTrainee = await DBX.TRTrainingEmployees.Where(x => x.TrainNo == id.ToString() && x.EmpCode == EmpCode).FirstOrDefaultAsync();
            if (BSM.HeaderTrainee != null)
            {
                BSM.HeaderInvite = DBX.TRTrainingInvitations.FirstOrDefault(w => w.TrainNo == id);
                BSM.Staff_View = DBStaff.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == BSM.HeaderTrainee.EmpCode);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("CODE", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, TrainingProcessObject ModelObject)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ViewData[SYSConstant.PARAM_ID1] = false;
            TrainingProcessObject BSM = new TrainingProcessObject();
            try
            {
                BSM.ScreenId = SCREEN_ID;
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
                    }
                    BSM.HeaderTrainee = ModelObject.HeaderTrainee;
                    string msg = BSM.EditTrainee(id);
                    if (msg == SYConstant.OK)
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);

                        mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Index";
                        ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                        ViewData[SYSConstant.PARAM_ID1] = true;
                        return View(BSM);
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
            catch
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
                return View(BSM);
            }
        }
        #endregion

        //// GET: TRTrainingEmployee/Delete/5
        //public ActionResult Delete(string IDs)
        //{
        //    ActionName = "Index";
        //    UserSession();
        //    UserConfListAndForm(this.KeyName);
        //    TrainingProcessObject BSM = new TrainingProcessObject();
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
        //        string[] ids = IDs.Split(',');
        //        string msg = BSM.DeleteTrainee(ids.ToList());
        //        if (msg == SYConstant.OK)
        //        {
        //            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
        //        }
        //        else
        //        {
        //            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
        //        }
        //    }
        //    Session[Index_Sess_Obj + ActionName] = BSM;
        //    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        //}
        #region popup form
        //public ActionResult PartialEmployeeSearch()
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    DataSelector();
        //    UserConfForm(ActionBehavior.ACC_REV);
        //    TrainingProcessObject BSM = new TrainingProcessObject();
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
        //    }
        //    //BSM.GetAllStaff();
        //    return PartialView("PartialEmployeeSearch", BSM);

        //}
        //public ActionResult gvemployeeselector()
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    DataSelector();
        //    UserConfForm(ActionBehavior.ACC_REV);
        //    ViewData[SYSConstant.PARAM_ID1] = true;
        //    TrainingProcessObject BSM = new TrainingProcessObject();
        //    BSM.ScreenId = SCREEN_ID;

        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
        //    }
        //    else
        //    {
        //        BSM.ListStaff = new List<Core.DB.HRStaffProfile>();
        //    }
        //    Session[Index_Sess_Obj + ActionName] = BSM;
        //    Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";

        //    ViewData["TRAINING_COURSE"] = DBX.TRTrainingCourses.ToList();
        //    BSM.GetAllStaff();
        //    return PartialView("gvemployeeselector", BSM);

        //}
        //public ActionResult GridItems()
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    DataSelector();
        //    TrainingProcessObject BSM = new TrainingProcessObject();

        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
        //        if (BSM.ListTempTrainee != null)
        //        {
        //            BSM.ListTempTrainee = BSM.ListTempTrainee.OrderBy(x => x.EmpCode).ToList();
        //        }
        //    }

        //    return PartialView("GridItems", BSM);
        //}
        //public ActionResult SelectedTrainee(string EmpCode)
        //{

        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    ViewData[SYSConstant.PARAM_ID1] = true;
        //    TrainingProcessObject BSM = new TrainingProcessObject();
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
        //    }
        //    if (EmpCode != null)
        //    {
        //        string[] empCodes = EmpCode.Split(',');
        //        empCodes.ToList().ForEach(x => BSM.ListEmpCode.Add(x));
        //        BSM.ListEmpCode = BSM.ListEmpCode.Distinct().ToList();
        //        var staff = DBStaff.HRStaffProfiles.ToList();
        //        List<Core.DB.HRDepartment> ListDepartment = DBStaff.HRDepartments.ToList();
        //        List<Core.DB.HRPosition> ListPosition = DBStaff.HRPositions.ToList();
        //        var list = staff.Where(w => empCodes.Contains(w.EmpCode));
        //        int i = BSM.ListTempTrainee.Count();
        //        foreach (var item in list)
        //        {
        //            if (!BSM.ListTempTrainee.Where(w => w.EmpCode == item.EmpCode).Any())
        //            {
        //                i++;
        //                string Department = "";
        //                string Position = "";

        //                if (!string.IsNullOrEmpty(item.DEPT))
        //                {
        //                    var Dept = ListDepartment.FirstOrDefault(w => w.Code == item.DEPT);
        //                    if (Dept != null) Department = Dept.Description;
        //                }
        //                if (!string.IsNullOrEmpty(item.JobCode))
        //                {
        //                    var Post = ListPosition.FirstOrDefault(w => w.Code == item.JobCode);
        //                    if (Post != null) Position = Post.Description;
        //                }
        //                BSM.ListTempTrainee.Add(new TRTrainingEmployee
        //                {
        //                    TrainNo = i,
        //                    EmpCode = item.EmpCode,
        //                    EmpName = item.AllName,
        //                    Department = Department,
        //                    Position = Position
        //                });
        //            }
        //        }
        //        BSM.ListTempTrainee.OrderBy(x => x.EmpCode);
        //        var result = new
        //        {
        //            MS = SYConstant.OK,
        //        };
        //        Session[Index_Sess_Obj + ActionName] = BSM;
        //        return Json(result, JsonRequestBehavior.DenyGet);
        //    }

        //    var rs = new { MS = SYConstant.FAIL };
        //    return Json(rs, JsonRequestBehavior.DenyGet);
        //}
        #endregion

        //private DataTable GetTrainingCatalogues()
        //{
        //    var trCatalogues = DBX.TRTrainingCatalogues;
        //    //create a new datatable
        //    DataTable table = new DataTable();
        //    //declare datacolumn and datarow object object
        //    DataColumn column;
        //    DataRow row;
        //    // Create new DataColumn, set DataType, ColumnName and add to DataTable
        //    column = new DataColumn();
        //    column.DataType = Type.GetType("System.String");
        //    column.ColumnName = "InYearName";
        //    table.Columns.Add(column);
        //    //add second column
        //    column = new DataColumn();
        //    column.DataType = Type.GetType("System.Int32");
        //    column.ColumnName = "InYear";
        //    table.Columns.Add(column);
        //    foreach (var item in trCatalogues.ToList().OrderBy(x => x.InYear).Select(x => x.InYear).Distinct())
        //    {
        //        row = table.NewRow();
        //        row["InYearName"] = item.Value.ToString();
        //        row["InYear"] = item.Value;

        //        table.Rows.Add(row);
        //    }
        //    return table;
        //}
        #region trainee(s)
        //[HttpPost]
        //public ActionResult DeleteTrainee(int TrainNo, string EmpCode)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    DataSelector();
        //    TrainingProcessObject BSM = new TrainingProcessObject();

        //    if (TrainNo != 0)
        //    {
        //        if (Session[Index_Sess_Obj + ActionName] != null)
        //        {
        //            BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
        //            BSM.ListEmpCode.Remove(EmpCode);
        //            if (BSM.ListTempTrainee.Count > 0)
        //            {
        //                var objCheck = BSM.ListTempTrainee.FirstOrDefault(w => w.TrainNo == TrainNo);
        //                BSM.ListTempTrainee.Remove(objCheck);
        //                BSM.ListTempTrainee.OrderBy(x => x.EmpCode);
        //                Session[Index_Sess_Obj + ActionName] = BSM;
        //            }
        //        }
        //    }
        //    return PartialView("GridItems", BSM);
        //}
        #region trainees list

        //[HttpPost]
        //public ActionResult DeletePartialTrainee(int TrainNo, string EmpCode)
        //{
        //    ActionName = "Index";
        //    UserSession();
        //    UserConfListAndForm();
        //    DataSelector();
        //    TrainingProcessObject BSM = new TrainingProcessObject();

        //    if (TrainNo != 0)
        //    {
        //        if (Session[Index_Sess_Obj + ActionName] != null)
        //        {
        //            BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
        //            BSM.ListEmpCode.Remove(EmpCode);
        //            if (BSM.ListTempTrainee.Count > 0)
        //            {
        //                var objCheck = BSM.ListTempTrainee.FirstOrDefault(w => w.TrainNo == TrainNo);
        //                BSM.ListTempTrainee.Remove(objCheck);

        //                BSM.DeleteTraineeInvitation(TrainNo, TrainNo.ToString());
        //                BSM.ListTempTrainee.OrderBy(x => x.EmpCode);
        //                Session[Index_Sess_Obj + ActionName] = BSM;
        //            }
        //        }
        //    }
        //    return PartialView("PartialTrainee", BSM.ListTempTrainee);
        //}
        #endregion

        #region "Import & Upload"
        //public ActionResult GridItemsImport()
        //{
        //    ActionName = "Import";
        //    UserSession();
        //    UserConfListAndForm();
        //    DataSelector();
        //    TrainingProcessObject BSM = new TrainingProcessObject();
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
        //    }
        //    Session[Index_Sess_Obj + ActionName] = BSM;
        //    return PartialView("GridItemsImport", BSM);
        //}
        //public ActionResult Import()
        //{
        //    UserSession();
        //    ActionName = "Import";
        //    UserConfListAndForm(this.KeyName);
        //    var obj = new TrainingProcessObject();
        //    UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "TRTrainingEmployee", SYSConstant.DEFAULT_UPLOAD_LIST);
        //    obj.ListTemplate = DBStaff.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID).OrderByDescending(w => w.UploadDate).ToList();
        //    Session[Index_Sess_Obj + ActionName] = obj;
        //    return View(obj);
        //}

        //[HttpPost]
        //public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        //{
        //    UserSession();
        //    ActionName = "Import";
        //    this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
        //    UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "TRTrainingEmployee", SYSConstant.DEFAULT_UPLOAD_LIST);
        //    SYFileImport sfi = new SYFileImport(DBStaff.CFUploadPaths.Find("TRAINING"));
        //    sfi.ObjectTemplate = new Core.DB.MDUploadTemplate();
        //    sfi.ObjectTemplate.UploadDate = DateTime.Now;
        //    sfi.ObjectTemplate.ScreenId = SCREEN_ID;
        //    sfi.ObjectTemplate.UploadBy = user.UserName;
        //    sfi.ObjectTemplate.Module = "TRAINING";
        //    sfi.ObjectTemplate.IsGenerate = false;

        //    UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadMQ",
        //        sfi.ValidationSettings,
        //        sfi.uc_FileUploadComplete);

        //    return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
        //    //return null;
        //}
        //public ActionResult UploadList()
        //{
        //    UserSession();
        //    ActionName = "Import";
        //    this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
        //    UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "TRTrainingEmployee", SYSConstant.DEFAULT_UPLOAD_LIST);
        //    IEnumerable<Core.DB.MDUploadTemplate> listu = DBStaff.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID).OrderByDescending(w => w.UploadDate);
        //    return PartialView(SYListConfuration.ListDefaultUpload, listu.ToList());
        //}
        //public ActionResult GenerateUpload(int id)
        //{
        //    UserSession();

        //    Core.DB.MDUploadTemplate obj = DBStaff.MDUploadTemplates.Find(id);
        //    //HumicaDBContext DBB = new HumicaDBContext();
        //    if (obj != null)
        //    {
        //        SYExcel excel = new SYExcel();
        //        excel.FileName = obj.UpoadPath;
        //        DataTable dt = excel.GenerateExcelData();
        //        if (obj.IsGenerate == true)
        //        {
        //            SYMessages mess = SYMessages.getMessageObject("FILE_RG", user.Lang);
        //            Session[SYSConstant.MESSAGE_SUBMIT] = mess;
        //            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
        //        }
        //        if (dt != null)
        //        {
        //            TrainingProcessObject BSM = new TrainingProcessObject();
        //            StringBuilder sb = new StringBuilder();
        //            BSM.ScreenId = ScreendIDControl;
        //            BSM.ListTrainee = new List<TRTrainingEmployee>();
        //            BSDocConfg DocBatch = new BSDocConfg("BATCH_UPLOAD", DocConfType.Normal, true);
        //            var resualt = DBX.TRTrainingEmployees;
        //            try
        //            {
        //                if (dt.Rows.Count > 0)
        //                {
        //                    for (int i = 0; i < dt.Rows.Count; i++)
        //                    {
        //                        string empCode = dt.Rows[i][0]?.ToString();
        //                        decimal score = Convert.ToDecimal(dt.Rows[i][2]?.ToString());
        //                        int inYear = Convert.ToInt32(dt.Rows[i][3].ToString());
        //                        string courseCode = dt.Rows[i][4]?.ToString();
        //                        int? trainNoCal;
        //                        if (BSM.IsExistCalendar(inYear, courseCode, out trainNoCal))
        //                        {
        //                            var objHeader = resualt.FirstOrDefault(x => x.EmpCode == empCode && x.CalendarID == trainNoCal);
        //                            if (objHeader != null)
        //                            {
        //                                try
        //                                {
        //                                    objHeader.Score = score;
        //                                    DBX.Entry(objHeader).State = System.Data.Entity.EntityState.Modified;
        //                                    DBX.Entry(objHeader).Property(w => w.Score).IsModified = true;

        //                                    DBX.SaveChanges();
        //                                }
        //                                catch { throw; }
        //                            }
        //                        }
        //                    }
        //                }
        //                obj.Message = SYConstant.OK;
        //                obj.DocumentNo = DocBatch.NextNumberRank;
        //                obj.IsGenerate = true;
        //                DBStaff.MDUploadTemplates.Attach(obj);
        //                DBStaff.Entry(obj).Property(w => w.Message).IsModified = true;
        //                DBStaff.Entry(obj).Property(w => w.DocumentNo).IsModified = true;
        //                DBStaff.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
        //                DBStaff.SaveChanges();
        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = SCREEN_ID;
        //                log.UserId = user.UserID.ToString();
        //                log.DocurmentAction = "Trainging_UPLOAD";
        //                log.Action = SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e);
        //                /*----------------------------------------------------------*/
        //                obj.Message = e.Message;
        //                obj.IsGenerate = false;
        //                DBStaff.MDUploadTemplates.Attach(obj);
        //                DBStaff.Entry(obj).Property(w => w.Message).IsModified = true;
        //                DBStaff.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
        //                DBStaff.SaveChanges();
        //            }
        //            catch (DbUpdateException e)
        //            {
        //                obj.Message = e.Message;
        //                obj.IsGenerate = false;
        //                DBStaff.MDUploadTemplates.Attach(obj);
        //                DBStaff.Entry(obj).Property(w => w.Message).IsModified = true;
        //                DBStaff.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
        //                DBStaff.SaveChanges();
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = SCREEN_ID;
        //                log.UserId = user.UserName.ToString();
        //                log.ScreenId = "Trainging_UPLOAD";
        //                log.Action = SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/

        //            }
        //            catch (Exception e)
        //            {
        //                obj.Message = e.Message;
        //                obj.IsGenerate = false;
        //                DBStaff.MDUploadTemplates.Attach(obj);
        //                DBStaff.Entry(obj).Property(w => w.Message).IsModified = true;
        //                DBStaff.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
        //                DBStaff.SaveChanges();

        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = SCREEN_ID;
        //                log.UserId = user.UserName.ToString();
        //                log.DocurmentAction = "Trainging_UPLOAD";
        //                log.Action = SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //            }
        //        }
        //        else
        //        {
        //            obj.Message = SYMessages.getMessage("EX_DT");
        //            obj.IsGenerate = false;
        //            DBStaff.MDUploadTemplates.Attach(obj);
        //            DBStaff.Entry(obj).Property(w => w.Message).IsModified = true;
        //            DBStaff.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
        //            DBStaff.SaveChanges();
        //        }
        //    }
        //    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
        //}
        //public ActionResult DownloadTemplate()
        //{
        //    var Master = new DataTable("Trainee");
        //    Master.Columns.Add("Trainee Code", typeof(string));
        //    Master.Columns.Add("Trainee Name", typeof(string));
        //    Master.Columns.Add("Score", typeof(string));
        //    Master.Columns.Add("INYear", typeof(DateTime));
        //    Master.Columns.Add("Course", typeof(string));

        //    var Code = new DataTable("Course Setup");
        //    Code.Columns.Add("Code", typeof(string));
        //    Code.Columns.Add("Description", typeof(string));
        //    DataRow dr;
        //    var trainingCourse = DBX.TRTrainingCourses;
        //    List<TRTrainingCourse> items = trainingCourse.ToList();
        //    foreach (TRTrainingCourse item in items)
        //    {
        //        dr = Code.NewRow();
        //        dr["Code"] = item.Code;
        //        dr["Description"] = item.Description;
        //        Code.Rows.Add(dr);
        //    }
        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        wb.Worksheets.Add(Master);
        //        wb.Worksheets.Add(Code);

        //        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.Charset = "";
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename=TRTraining_TEMPLATE.xlsx");
        //        using (MemoryStream MyMemoryStream = new MemoryStream())
        //        {
        //            wb.SaveAs(MyMemoryStream);
        //            MyMemoryStream.WriteTo(Response.OutputStream);
        //            Response.Flush();
        //            Response.End();
        //        }
        //    }
        //    return null;
        //}

        #endregion
        public async Task<ActionResult> Accept(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ActionName = "Index";
            ViewData[SYSConstant.PARAM_ID] = id;
            TrainingProcessObject BSM = new TrainingProcessObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
            }
            if (id != null)
            {
                string msg = await BSM.AcceptTheDoc(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_ACC", user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }


        #endregion
        private void DataSelector()
        {
            //ViewData["INYEAR"] = GetTrainingCatalogues();
            ViewData["TRAINING_COURSE"] = DBX.TRTrainingCourses.ToList();
            //ViewData["COURSE_CATEGORY"] = DBX.TRCourseCategories.ToList();
            //ViewData["TRAINING_TYPE"] = DBX.TRTrainingTypes.ToList();
            //List<TRTrainingPlan> listCalendar = new List<TRTrainingPlan> { new TRTrainingPlan() };
            //listCalendar.AddRange(DBX.TRTrainingPlans.ToList());
            //ViewData["CourseID"] = listCalendar;
        }
    }
}
