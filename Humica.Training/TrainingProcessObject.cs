using Humica.Core;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Humica.Training
{
    public class TrainingProcessObject
    {
        HumicaDBContextTR DB = new HumicaDBContextTR();
        Core.DB.HumicaDBContext DBStaff = new Core.DB.HumicaDBContext();
        MDHRCurrency currency_ = new MDHRCurrency();
        public SYUser User { get; set; }
        public SMSystemEntity DP = new SMSystemEntity();
        public IEnumerable<object> ListQuestion;

        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public FTTraining FTTraining { get; set; }
        public TRTrainingCatalogue TRTrainingCatalogue { get; set; }
        public List<TRTrainingCatalogue> ListTRTrainingCatalogue { get; set; }
        public List<Core.DB.MDUploadTemplate> ListTemplate { get; set; }
        public List<TRTrainingCatalogueCourse> ListTRTrainingCatalogueCourse { get; set; }
        public List<TRTrainingCourse> ListCourseType { get; set; }
        public List<ModelTRTrainingCatalogue> ListModelTRTrainingCatalogue { get; set; }
        public TRTrainingPlan TrainingPlan { get; set; }
        public TRTrainingCalender HeaderCalender { get; set; }
        public TRTrainingInvitation HeaderInvite { get; set; }
        //public TRTrainingCatalogueModel TRTrainingCatalogueModel { get; set; }
        public List<TRTrainingSession> ListTRTrainingSession { get; set; }
        public Core.DB.HRStaffProfile staffProfile { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string SessionName { get; set; }
        public List<TRTrainingPlan> ListTrainingPlan { get; set; }
        public List<Core.DB.HRStaffProfile> ListStaff { get; set; }
        public List<TRTrainingEmployeeModel> ListTraineeModel { get; set; }
        //public TRTrainingEmployeeModel TrainingEmployeeModel { get; set; }
        public List<TRTrainingEmployee> ListTrainee { get; set; }
        public List<TRTrainingEmployee> ListTraineePeding { get; set; }
        public TRTrainingEmployee HeaderTrainee { get; set; }
        public TRTrainingAttendance TrainingAttendance { get; set; }
        public TRTrainingSession TRTrainingSession { get; set; }
        public Core.DB.HRStaffProfile Staff_View { get; set; }
        public List<string> ListEmpCode { get; set; }
        public List<string> CatalogueCourseID { get; set; }
        public List<TrainingAttendanceModel> TrainingAttendanceModels { get; set; }
        public TrainingAttendanceModel TrainingAttendanceModel { get; set; }
        public List<TRTrainingCalender> ListCalender { get; set; }
        public List<TRTrainingEmployee> ListTempTrainee { get; set; }
        public List<TRTrainingAttendance> ListHeader { get; set; }
        
        public TrainingProcessObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            ListTraineeModel = new List<TRTrainingEmployeeModel>();
            ListTrainee = new List<TRTrainingEmployee>();
            ListEmpCode = new List<string>();
            CatalogueCourseID = new List<string>();
        }
        public void GetAllStaff()
        {
            var tblStaffProfile = DBStaff.HRStaffProfiles.Where(w => w.Status == "A").ToList();
            this.ListStaff = tblStaffProfile.Where(w => !this.ListEmpCode.Contains(w.EmpCode)).ToList();
        }
        public void GetCourseSetup() => this.ListCourseType = this.ListCourseType.Where(w => !this.CatalogueCourseID.Contains(w.TrainNo.ToString())).ToList();
        public void GetStaffProfile(string empCode)
        {
            var tblStaffProfile = DBStaff.HRStaffProfiles;
            this.staffProfile = tblStaffProfile.Find(empCode);
        }

        public static IEnumerable<TRTrainerInfo> GetTrainerInfo()
        {
            HumicaDBContextTR DBB = new HumicaDBContextTR();
            Core.DB.HumicaDBContext _DB = new Core.DB.HumicaDBContext();
            List<TRTrainerInfo> trainers = new List<TRTrainerInfo>();
            string trainerTypeCode = HttpContext.Current.Session["TrainerTypeType"]?.ToString();
            var objTrainerType = DBB.TRTrainerType;
            TRTrainerType trainerType = objTrainerType.Where(x => x.Code == trainerTypeCode).FirstOrDefault();
            var trainer = DBB.TRTrainerInfo;
            var objStaff = _DB.HRStaffProfiles;
            trainers = trainer.ToList();
            if (trainerType != null)
            {
                trainers = new List<TRTrainerInfo>();
                trainers = trainer.Where(w => w.TrainerTypeID == trainerType.TrainNo.ToString()).ToList();
            };
            trainers.ForEach(w => w.TrainerName = trainerTypeCode == TrainerType.EXT ? w.TrainerName : objStaff.ToList().Where(x => x.EmpCode == w.TrainerCode).FirstOrDefault()?.AllName);
            return trainers;
        }
        #region training attendance
        public static IEnumerable<TRTopic> GetTopic()
        {
            HumicaDBContextTR DBB = new HumicaDBContextTR();
            string inYear = HttpContext.Current.Session["InYear"]?.ToString();
            string courseCode = HttpContext.Current.Session["CourseCode"]?.ToString();
            // get all tops from training session
            // noted: before get training session, you need to get data from training calendar
            var objCalendar = DBB.TRTrainingPlans;
            TRTrainingPlan calendar = objCalendar.Where(w => w.InYear.ToString() == inYear && w.CourseID == courseCode).FirstOrDefault();

            List<TRTopic> topics = new List<TRTopic>();
            if (inYear != null && courseCode != null)
            {
                var objTopic = DBB.TRTopics;
                var objSession = DBB.TRTrainingSessions;
                List<TRTrainingSession> trainingSessions = new List<TRTrainingSession>(objSession.Where(w => w.TrainingCalendarID == calendar.TrainNo));
                topics = objTopic.AsEnumerable().Where(w => trainingSessions.AsEnumerable().Where(x => x.TopicID == w.TrainNo).Any()).ToList();
            }

            return topics;
        }
        public static DataTable GetSessionDate()
        {
            //HumicaDBContext DBB = new HumicaDBContext();
            string inYear = HttpContext.Current.Session["InYear"]?.ToString();
            string courseCode = HttpContext.Current.Session["CourseCode"]?.ToString();
            // get all tops from training session
            // noted: before get training session, you need to get data from training calendar
            //var objCalendar = DBB.TrainingPlan;
            //TrainingPlan calendar = objCalendar.Where(w => w.InYear.ToString() == inYear && w.CourseID == courseCode).FirstOrDefault();
            TRTrainingCalender calendar = GetTrainingCatalogueCalendar(inYear, courseCode);

            DataTable sessionDate = new DataTable();
            if (calendar != null)
            {
                //var objSession = DBB.TRTrainingSession;
                //List<TRTrainingSession> trainingSessions = new List<TRTrainingSession>(objSession.Where(w => w.TrainingCalendarID == calendar.TrainNo));
                List<TRTrainingAgenda> trainingSessions = GetTrainingSessions(calendar);
                List<DateTime?> dateTimes = new List<DateTime?>();
                trainingSessions.ForEach(w => dateTimes.Add(w.TrainingDate));
                sessionDate = GetTrainingSessionDate(dateTimes);
            }
            return sessionDate;
        }
        protected static DataTable GetTrainingSessionDate(List<DateTime?> dateTimes)
        {

            //create a new datatable
            DataTable table = new DataTable();
            //declare datacolumn and datarow object object
            DataColumn column;
            DataRow row;
            // Create new DataColumn, set DataType, ColumnName and add to DataTable
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "StartDateName";
            table.Columns.Add(column);
            //add second column
            column = new DataColumn();
            column.DataType = Type.GetType("System.DateTime");
            column.ColumnName = "StartDate";
            table.Columns.Add(column);
            string dateName = string.Empty;
            string newDateName = string.Empty;
            foreach (var date in dateTimes)
            {
                newDateName = string.Format("{0:dd-MMM-yyyy}", date);
                if (date == null || dateName == newDateName) continue;
                row = table.NewRow();
                row["StartDateName"] = newDateName;//string.Format("{0:dd-MMM-yyyy}", date);
                row["StartDate"] = newDateName;//string.Format("{0:dd-MMM-yyyy}", date);
                dateName = newDateName;
                table.Rows.Add(row);
            }
            return table;
        }
        //private List<TRTrainingAttendance> GetTrainingAttendances(TRTrainingSession trainingSession)
        //{
        //    HumicaDBContext humicaDBContext = new HumicaDBContext();
        //    List<TRTrainingAttendance> trainingAttendances = new List<TRTrainingAttendance>();
        //    try
        //    {
        //        var objTrainingAtt = humicaDBContext.TRTrainingAttendances;
        //        trainingAttendances = objTrainingAtt.Where(w => w.TrainingCalendarID == trainingSession.TrainingCalendarID &&
        //             w.TrainingSessionID == trainingSession.TrainNo).ToList();
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return trainingAttendances;
        //}
       
        public string DeleteTrainingAttendance(string trainNo)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                var objTrainingAtt = DB.TRTrainingAttendances.Find(trainNo);
                if (objTrainingAtt == null)
                {
                    return "DOC_NE";
                }
                DB.TRTrainingAttendances.Attach(objTrainingAtt);
                DB.Entry(objTrainingAtt).State = EntityState.Deleted;
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = trainNo.ToString();
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        
        //public bool IsExist(List<TrainingAttendanceModel> items, string empCode)
        //{
        //    var objTrainingAtt = DB.TRTrainingAttendances;
        //    this.TRTrainingSession = this.TrainingAttendanceModel.TRTrainingSession;
        //    int count = TRTrainingSession == null ? 0 : objTrainingAtt.Where(w => w.TrainingSessionID == this.TRTrainingSession.TrainNo
        //            && w.TrainingCalendarID == this.TRTrainingSession.TrainingCalendarID
        //            && w.EmpCode == empCode).Count();
        //    return items.Where(w => w.TrainingAttendance.EmpCode == empCode).Count() > 0 || count > 0;
        //}
        //public bool IsExistCalendar(int inYear, string courseCode, out int? trainNoCal)
        //{
        //    trainNoCal = 0;
        //    using (HumicaDBContext context = new HumicaDBContext())
        //    {
        //        var objTrainingCal = context.TRTrainingPlans;
        //        List<TRTrainingPlan> items = new List<TRTrainingPlan>();
        //        items = objTrainingCal.Where(w => w.InYear == inYear && w.CourseID == courseCode).ToList();
        //        if (items.Count() > 0)
        //        {
        //            trainNoCal = items.FirstOrDefault().TrainNo;
        //            return true;
        //        }
        //        return false;
        //    }
        //}
        #endregion
        // end training attendance
        public DateTime? TimeNullable(DateTime? value)
        {
            DateTime? dateTime = null;
            return value.HasValue ? value.Time() : dateTime;

        }
        public List<Currency> Currency
        {
            get
            {
                List<Currency> items = new List<Currency>();
                var hRCurrencies = currency_.DB.HRCurrencies;
                var lists = hRCurrencies.ToList();
                lists.ForEach(x => items.Add(new Currency { Code = x.CurrencyCode, Description = x.Description }));
                return items;
            }
        }
        #region Training Plan
        public string CreateTrainingPlan()
        {

            try
            {
                int i = 0;
                using (var context = new HumicaDBContextTR())
                {

                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (TrainingPlan.InYear == null)
                            {
                                return "IN_YEAR";
                            }
                            if (string.IsNullOrEmpty(TrainingPlan.CourseID))
                            {
                                return "COURSE_NAME";
                            }
                            //if (context.TRTrainingPlans.Where(w => w.InYear == TrainingPlan.InYear
                            //    && w.CourseID == TrainingPlan.CourseID).Count() > 0)
                            //{
                            //    return "TRAINING_EXIST";
                            //}
                            #region training calendar
                            var trainingCatalogCal = new TRTrainingPlan();
                            var CourseType = DB.TRTrainingCourses.FirstOrDefault(w => w.Code == TrainingPlan.CourseID);
                            if (CourseType != null)
                            {
                                TrainingPlan.CourseName = CourseType.Description;
                            }
                            var TrainType = DB.TRTrainingTypes.FirstOrDefault(w => w.Code == TrainingPlan.TrainingTypeID);
                            if (TrainType != null)
                            {
                                TrainingPlan.TrainingType = TrainType.Description;
                            }
                            var CourseCategory = DB.TRCourseCategories.FirstOrDefault(w => w.Code == TrainingPlan.CourseCategoryID);
                            if (CourseCategory != null)
                            {
                                TrainingPlan.CourseCategory = CourseCategory.SecondDescription;
                            }
                            trainingCatalogCal = TrainingPlan;
                            trainingCatalogCal.RequesterCode = staffProfile.EmpCode;
                            trainingCatalogCal.CreatedBy = User.UserName;
                            trainingCatalogCal.CreatedOn = DateTime.Today;
                            context.TRTrainingPlans.Add(trainingCatalogCal);

                            int row = context.SaveChanges();
                            i = trainingCatalogCal.TrainNo;
                            #endregion
                            #region session
                            int sessionTrainNo = 0;
                            string _SessionTrainNo = string.Empty;
                            var _ObjSession = context.TRTrainingSessions;
                            List<TRTrainingSession> trainingSessions = new List<TRTrainingSession>();
                            trainingSessions = _ObjSession.ToList();
                            if (trainingSessions.Count() > 0)
                            {
                                _SessionTrainNo = trainingSessions.Max(w => int.Parse(w.TrainNo)).ToString();
                                sessionTrainNo = int.Parse(_SessionTrainNo);
                            }
                            List<TRTrainerInfo> _listTrainerInfo = DB.TRTrainerInfo.ToList();
                            foreach (var item in ListTRTrainingSession)
                            {
                                TRTrainerInfo TrainerInfo = _listTrainerInfo.FirstOrDefault(w => w.TrainNo == Convert.ToInt32(item.TrainerID));
                                if (TrainerInfo != null) item.TrainerNames = TrainerInfo.TrainerName;
                                DateTime? fromTime = this.TimeNullable(item.FromTime);
                                DateTime? toTime = this.TimeNullable(item.ToTime);
                                item.FromTime = fromTime;
                                item.ToTime = toTime;
                                item.TrainingCalendarID = i;
                                item.CreatedBy = User.UserName;
                                item.CreatedOn = DateTime.Today;
                                item.TrainNo = (++sessionTrainNo).ToString();
                                context.TRTrainingSessions.Add(item);
                                context.SaveChanges();
                            }
                            #endregion

                            dbContextTransaction.Commit();
                        }
                        catch (DbEntityValidationException e)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = TrainingPlan.TrainNo.ToString();
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = TrainingPlan.TrainNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = TrainingPlan.TrainNo.ToString();
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditTrainingPlan(int id)
        {

            DB = new HumicaDBContextTR();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    var calendar = DB.TRTrainingPlans.Find(id);
                    if (calendar == null)
                        return "INV_DOC";
                    if (string.IsNullOrEmpty(calendar.CourseID))
                    {
                        return "COURSE_NAME";
                    }
                    int sessionTrainNo = 0;
                    string _SessionTrainNo = string.Empty;
                    var _ObjSession = DB.TRTrainingSessions;
                    List<TRTrainingSession> trainingSessions = new List<TRTrainingSession>();
                    trainingSessions = _ObjSession.ToList();
                    foreach (var item in trainingSessions.Where(w => !ListTRTrainingSession.Any(x => x.TrainNo == w.TrainNo) && w.TrainingCalendarID == id))
                    {
                        DB.TRTrainingSessions.Attach(item);
                        DB.Entry(item).State = EntityState.Deleted;
                        DB.SaveChanges();
                    }
                    var CourseType = DB.TRTrainingCourses.FirstOrDefault(w => w.Code == TrainingPlan.CourseID);
                    if (CourseType != null)
                    {
                        calendar.CourseName = CourseType.Description;
                    }
                    var TrainType = DB.TRTrainingTypes.FirstOrDefault(w => w.Code == TrainingPlan.TrainingTypeID);
                    if (TrainType != null)
                    {
                        calendar.TrainingType = TrainType.Description;
                    }
                    var CourseCategory = DB.TRCourseCategories.FirstOrDefault(w => w.Code == TrainingPlan.CourseCategoryID);
                    if (CourseCategory != null)
                    {
                        calendar.CourseCategory = CourseCategory.Description;
                    }
                    calendar.ChangedBy = User.UserName;
                    calendar.ChangedOn = DateTime.Now;
                    calendar.CourseID = TrainingPlan.CourseID;
                    calendar.TrainingTypeID = TrainingPlan.TrainingTypeID;
                    //calendar.TrainingCate = TrainingPlan.TrainingCate;
                    calendar.CourseCategoryID = TrainingPlan.CourseCategoryID;
                    calendar.VenueCode = TrainingPlan.VenueCode;
                    calendar.StartDate = TrainingPlan.StartDate;
                    calendar.EndDate = TrainingPlan.EndDate;
                    calendar.MinTrainee = TrainingPlan.MinTrainee;
                    calendar.MaxTrainee = TrainingPlan.MaxTrainee;
                    calendar.TotalCost = TrainingPlan.TotalCost;
                    calendar.Status = TrainingPlan.Status;
                    DB.Entry(calendar).State = EntityState.Modified;
                    #region session
                    if (trainingSessions.Count() > 0)
                    {
                        _SessionTrainNo = trainingSessions.Max(w => int.Parse(w.TrainNo)).ToString();
                        sessionTrainNo = int.Parse(_SessionTrainNo);
                    }
                    using (var con = new HumicaDBContextTR())
                    {
                        var objSession = con.TRTrainingSessions;
                        List<TRTrainerInfo> _listTrainerInfo = DB.TRTrainerInfo.ToList();
                        //sessionTrainNo = objSession.ToList().Count() > 0 ? int.Parse().ToString()) : 0;
                        foreach (var item in ListTRTrainingSession)
                        {
                            TRTrainingSession session = new TRTrainingSession();
                            TRTrainerInfo TrainerInfo = _listTrainerInfo.FirstOrDefault(w => w.TrainNo == Convert.ToInt32(item.TrainerID));
                            if (TrainerInfo != null) session.TrainerNames = TrainerInfo.TrainerName;
                            session = objSession.AsNoTracking().Where(w => w.TrainNo == item.TrainNo).FirstOrDefault();
                            DateTime? fromTime = this.TimeNullable(item.FromTime);
                            DateTime? toTime = this.TimeNullable(item.ToTime);
                            if (session != null)
                            {
                                session = item;
                                session.TrainingCalendarID = id;
                                session.FromTime = fromTime;
                                session.ToTime = toTime;
                                session.ChangedBy = User.UserName;
                                session.ChangedOn = DateTime.Now;
                                con.Entry(session).State = EntityState.Modified;
                            }
                            else
                            {
                                item.FromTime = fromTime;
                                item.ToTime = toTime;
                                item.TrainingCalendarID = id;
                                item.CreatedBy = User.UserName;
                                item.TrainNo = (++sessionTrainNo).ToString();
                                con.TRTrainingSessions.Add(item);
                            }

                        }
                        con.SaveChanges();
                    }
                    #endregion
                    DB.SaveChanges();
                    return SYConstant.OK;
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = id.ToString();
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = id.ToString();
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = id.ToString();
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string DeleteTrainingPlan(int id)
        {
            try
            {
                TrainingPlan = new TRTrainingPlan();
                var ObgMatch = DB.TRTrainingPlans.Find(id);
                if (ObgMatch == null)
                {
                    return "DOC_NE";
                }
                if (ObgMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "DOC_NE";
                }
                TrainingPlan = ObgMatch;
                DB.TRTrainingPlans.Attach(ObgMatch);
                DB.Entry(ObgMatch).State = System.Data.Entity.EntityState.Deleted;

                //delete record from session
                ListTRTrainingSession = new List<TRTrainingSession>();
                ListTRTrainingSession = DB.TRTrainingSessions.Where(x => x.TrainingCalendarID == id).ToList();
                foreach (var item in ListTRTrainingSession)
                {
                    DB.TRTrainingSessions.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = id.ToString();
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = id.ToString();
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ReleaseDoc(int id)
        {
            try
            {
                HumicaDBContextTR DBX = new HumicaDBContextTR();
                var objMatch = DB.TRTrainingPlans.FirstOrDefault(w => w.TrainNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }
                objMatch.Status = SYDocumentStatus.RELEASED.ToString();
                DB.TRTrainingPlans.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = TrainingPlan.TrainNo.ToString();
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = TrainingPlan.TrainNo.ToString();
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = TrainingPlan.TrainNo.ToString();
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion

        public string Create()
        {
            try
            {
                if (TRTrainingCatalogue.InYear == null)
                {
                    return "TR_CATALOGUE_";
                }
                if (DB.TRTrainingCatalogues.Where(w => w.InYear == TRTrainingCatalogue.InYear).Count() > 0)
                {
                    return "TR_CATALOGUE";
                }
                int i = 0;
                using (var context = new HumicaDBContextTR())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            /// add data to master
                            var trainingCatalog = new TRTrainingCatalogue();
                            trainingCatalog = TRTrainingCatalogue;
                            trainingCatalog.CreatedBy = User.UserName;
                            trainingCatalog.CreatedOn = DateTime.Today;
                            context.TRTrainingCatalogues.Add(trainingCatalog);

                            int row = context.SaveChanges();
                            i = trainingCatalog.TrainNo;

                            /// add item into child table
                            foreach (var item in ListTRTrainingCatalogueCourse)
                            {
                                item.CreatedBy = User.UserName;
                                item.CreatedOn = DateTime.Now;
                                item.TrainingCatalogueID = i;
                                context.TRTrainingCatalogueCourses.Add(item);
                                context.SaveChanges();
                            }
                            dbContextTransaction.Commit();
                        }
                        catch (DbEntityValidationException e)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = TRTrainingCatalogue.TrainNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = TRTrainingCatalogue.TrainNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Header.TranNo.ToString();
                log.Action = EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string Edit(int id)
        {

            DB = new HumicaDBContextTR();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    var trainingCatalog = DB.TRTrainingCatalogues.Find(id);
                    if (TRTrainingCatalogue == null)
                        return string.Empty;

                    trainingCatalog.Description = TRTrainingCatalogue.Description;
                    trainingCatalog.Remark = TRTrainingCatalogue.Remark;
                    trainingCatalog.ChangedBy = User.UserName;
                    trainingCatalog.ChangedOn = DateTime.Now;
                    DB.TRTrainingCatalogues.Attach(trainingCatalog);
                    DB.Entry(trainingCatalog).Property(w => w.Description).IsModified = true;
                    DB.Entry(trainingCatalog).Property(w => w.Remark).IsModified = true;
                    DB.Entry(trainingCatalog).Property(w => w.ChangedBy).IsModified = true;
                    DB.Entry(trainingCatalog).Property(w => w.ChangedOn).IsModified = true;
                    //delete detail
                    using (var con = new HumicaDBContextTR())
                    {
                        var courseCatalogues = con.TRTrainingCatalogueCourses.Where(w => w.TrainingCatalogueID == id).ToList();
                        foreach (var item in courseCatalogues)
                        {
                            con.TRTrainingCatalogueCourses.Attach(item);
                            con.Entry(item).State = EntityState.Deleted;
                            con.SaveChanges();
                        }
                    }

                    if (ListTRTrainingCatalogueCourse.Count > 0)
                    {
                        foreach (var item in ListTRTrainingCatalogueCourse)
                        {
                            item.CreatedBy = User.UserName;
                            item.CreatedOn = DateTime.Today;
                            item.TrainingCatalogueID = id;
                            DB.TRTrainingCatalogueCourses.Add(item);
                        }
                    }
                    DB.SaveChanges();

                    return SYConstant.OK;
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = id.ToString();
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = id.ToString();
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = id.ToString();
                    log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string Delete(int id)
        {
            try
            {
                TRTrainingCatalogue = new TRTrainingCatalogue();
                var tblTRTrainingCatalogue = DB.TRTrainingCatalogues.Find(id);
                if (tblTRTrainingCatalogue == null)
                {
                    return "DOC_NE";
                }
                TRTrainingCatalogue = tblTRTrainingCatalogue;
                DB.TRTrainingCatalogues.Attach(tblTRTrainingCatalogue);
                DB.Entry(tblTRTrainingCatalogue).State = System.Data.Entity.EntityState.Deleted;
                int row = DB.SaveChanges();
                //delete record from item
                ListTRTrainingCatalogueCourse = new List<TRTrainingCatalogueCourse>();
                ListTRTrainingCatalogueCourse = DB.TRTrainingCatalogueCourses.Where(x => x.TrainingCatalogueID == id).ToList();
                foreach (var item in ListTRTrainingCatalogueCourse)
                {
                    DB.TRTrainingCatalogueCourses.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    DB.SaveChanges();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = id.ToString();
                log.Action = EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = id.ToString();
                log.Action = EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        private static TRTrainingCalender GetTrainingCatalogueCalendar(string InYear, string CourseCode)
        {
            HumicaDBContextTR DBB = new HumicaDBContextTR();
            var objCalendar = DBB.TRTrainingCalenders.FirstOrDefault(w => w.InYear.ToString() == InYear && w.CourseID == CourseCode);
            return objCalendar;
        }
        private static List<TRTrainingAgenda> GetTrainingSessions(TRTrainingCalender Calendar)
        {
            HumicaDBContextTR DBB = new HumicaDBContextTR();
            var objSession = DBB.TRTrainingAgendas.Where(w => w.CalendarID == Calendar.TrainNo).ToList();
            return objSession;
        }
        public void InsertListTrainingAttendanceModel(List<TrainingAttendanceModel> TrainingAttendanceModels,
            List<TRTrainingAttendance> trainingAttendances,
            List<TRTrainingCalender> calendars)
        {
            var objCourse = DB.TRTrainingCourses;
            ListCourseType = objCourse.ToList();
            trainingAttendances.ForEach(w => TrainingAttendanceModels.Add(
            new TrainingAttendanceModel
            {
                TrainingAttendance = w,
                TrainNo = w.TrainNo.ToString(),
                HRStaffProfile = ListStaff.Where(x => x.EmpCode == w.EmpCode).FirstOrDefault(),
                TrainingCalender = calendars.Where(x => x.TrainNo == w.CalendarID).FirstOrDefault(),
                TrainingCourse = calendars.Where(x => x.TrainNo == w.CalendarID).FirstOrDefault() == null ?
                    new TRTrainingCourse() : ListCourseType.Where(z => z.Code == calendars.Where(x => x.TrainNo == w.CalendarID).FirstOrDefault().CourseID).FirstOrDefault()
            }));
        }
        public string DeleteTrainingAttendance(string TrainNo, int CalendarID, int LineItem)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                var objTrainingAtt = DB.TRTrainingAttendances.FirstOrDefault(w => w.TrainNo == TrainNo &&  w.CalendarID == CalendarID && w.LineItem == LineItem);
                if (objTrainingAtt == null)
                {
                    return "DOC_NE";
                }
                DB.TRTrainingAttendances.Attach(objTrainingAtt);
                DB.Entry(objTrainingAtt).State = EntityState.Deleted;
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = TrainNo.ToString();
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
                // DBD.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        public void EditSession(TRTrainingSession trainingSessionObj)
        {
            string msg = string.Empty;
            try
            {
                using (var con = new HumicaDBContextTR())
                {
                    var trainingSession = ListTRTrainingSession.Where(x => x.TrainNo == trainingSessionObj.TrainNo).FirstOrDefault();
                    if (trainingSession != null)
                    {
                        DateTime? fromTime = this.TimeNullable(trainingSessionObj.FromTime);
                        DateTime? toTime = this.TimeNullable(trainingSessionObj.ToTime);
                        trainingSession.FromTime = fromTime;
                        trainingSession.ToTime = toTime;
                        trainingSession.TrainerType = trainingSessionObj.TrainerType;
                        trainingSession.TrainerID = trainingSessionObj.TrainerID;
                        trainingSession.TopicID = trainingSessionObj.TopicID;
                        trainingSession.StartDate = trainingSessionObj.StartDate;
                        trainingSession.EndDate = trainingSessionObj.EndDate;
                        trainingSession.ChangedBy = User.UserName;
                        trainingSession.ChangedOn = DateTime.Today;
                        msg = "OK";
                    }
                }
            }
            catch (DbUpdateException e)
            {
                msg = "EE001";
            }
            catch (Exception e)
            {
                msg = "EE001";
            }
            //return msg;
        }
        public string EditTrainingAttendance(string TrainNo, int CalendarID, int LineItem)
        {

            DB = new HumicaDBContextTR();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    var objTrainingAtt = DB.TRTrainingAttendances.FirstOrDefault(w => w.TrainNo == TrainNo && w.CalendarID == CalendarID && w.LineItem == LineItem);
                    if (objTrainingAtt == null) return "INV_DOC";

                    objTrainingAtt.EmpCode = TrainingAttendance.EmpCode;
                    objTrainingAtt.ChangedBy = User.UserName;
                    objTrainingAtt.ChangedOn = DateTime.Now;
                    objTrainingAtt.IsAttend = TrainingAttendance.IsAttend;
                    objTrainingAtt.Remark = TrainingAttendance.Remark;
                    DB.Entry(objTrainingAtt).State = EntityState.Modified;
                    DB.SaveChanges();
                    return SYConstant.OK;
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = TrainNo.ToString();
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = TrainNo.ToString();
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ScreenId;
                    log.UserId = User.UserName;
                    log.DocurmentAction = TrainNo.ToString();
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    return "EE001";
                }
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        

        public string Att(string ID,string TrainNo, string CalendarID, string LineItem, string Remark, int id)
        {
            try
            {
                string[] IDs = ID.Split(';');
                string[] TraNo = TrainNo.Split(';');
                string[] CaID = CalendarID.Split(';');
                string[] LiI = LineItem.Split(';');
                var DBU = new Humica.Training.DB.HumicaDBContextTR();
                for (var i = 0; i < TraNo.Length; i++)
                {
                    var _interview = new TRTrainingAttendance();
                    if (string.IsNullOrEmpty(TraNo[i])) continue;
                    int selectedID = int.Parse(IDs[i]);
                    string selectedTrainNo = TraNo[i];
                    int selectedCalendarID = int.Parse(CaID[i]);
                    int selectedLineItem = int.Parse(LiI[i]);

                    var ObjMatch = DB.TRTrainingEmployees.Where(w => w.ID == selectedID && w.TrainNo == selectedTrainNo.ToString() && w.CalendarID == selectedCalendarID && w.LineItem == selectedLineItem).ToList().OrderBy(w => w.EmpName).Last();
                    if (ObjMatch != null)
                    {
                        _interview.ID = ObjMatch.ID;
                        _interview.TrainNo = ObjMatch.TrainNo.ToString();
                        _interview.CalendarID = ObjMatch.CalendarID;
                        _interview.LineItem = ObjMatch.LineItem;
                        _interview.EmpCode = ObjMatch.EmpCode;
                        _interview.EmpName = ObjMatch.EmpName;
                        if (ObjMatch.TrainerName == null) _interview.TrainerName = " ";
                        else _interview.TrainerName = ObjMatch.TrainerName.ToString();
                        _interview.Department = ObjMatch.Department;
                        _interview.Position = ObjMatch.Position;
                        _interview.TrainingType = ObjMatch.TrainingType;
                        _interview.CourseName = ObjMatch.CourseName;
                        _interview.CourseCategory = ObjMatch.CourseCategory;
                        _interview.CreatedOn = DateTime.Now;
                        if (ObjMatch.InviteTrainingDate == null) _interview.InDate = DateTime.Parse("01-01-1900");
                        else
                            _interview.InDate = (DateTime)ObjMatch.InviteTrainingDate;
                        _interview.HourAmount = ObjMatch.HAmount;
                        _interview.CreatedBy = User.UserName;
                        _interview.ChangedOn = DateTime.Now;
                        _interview.ChangedBy = User.UserName;
                        if (id == 0)
                        {
                            if (string.IsNullOrEmpty(Remark) || Remark == "null") return "invalid Remark";
                            _interview.IsAttend = false;
                            _interview.Remark = Remark;
                        }
                        else _interview.IsAttend = true;
                    }
                    DBU.TRTrainingAttendances.Add(_interview);
                    DBU.SaveChanges();
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Header.ApplicantID;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                // log.DocurmentAction = Header.ApplicantID;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        // trainee invitation
        public string DeleteTrainee(List<string> IDs)
        {
            try
            {
                TRTrainingEmployee trainingEmployees = new TRTrainingEmployee();
                var trainees = DB.TRTrainingEmployees;
                foreach (string id in IDs)
                {
                    trainingEmployees = trainees.Find(id);
                    DB.TRTrainingEmployees.Attach(trainingEmployees);
                    DB.Entry(trainingEmployees).State = EntityState.Deleted;

                }
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = ScreenId.ToString();
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = ScreenId.ToString();
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public void DeleteTraineeInvitation(int CalendarID = 0, string TraineeID = "")
        {
            if (string.IsNullOrEmpty(TraineeID))
            {
                var trainees = DB.TRTrainingEmployees.Where(x => x.CalendarID == CalendarID).ToList();
                foreach (TRTrainingEmployee item in trainees)
                {
                    DB.TRTrainingEmployees.Attach(item);
                    DB.Entry(item).State = EntityState.Deleted;
                    DB.SaveChanges();
                }
            }
            else
            {
                TRTrainingEmployee trainingEmployees = new TRTrainingEmployee();
                var trainees = DB.TRTrainingEmployees;
                trainingEmployees = trainees.Find(TraineeID);
                DB.TRTrainingEmployees.Attach(trainingEmployees);
                DB.Entry(trainingEmployees).State = EntityState.Deleted;
                DB.SaveChanges();
            }
        }
        #region Trainnee
        public string CreateTrainee()
        {
            try
            {
                List<Core.DB.HRStaffProfile> ListStaff = DBStaff.HRStaffProfiles.Where(w => w.Status == "A").ToList();
                List<Core.DB.HRDepartment> ListDepartment = DBStaff.HRDepartments.ToList();
                List<Core.DB.HRPosition> ListPosition = DBStaff.HRPositions.ToList();
                using (var context = new HumicaDBContextTR())
                {
                    this.DeleteTraineeInvitation(this.HeaderCalender.TrainNo);
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in ListTempTrainee)
                            {
                                HeaderTrainee = new TRTrainingEmployee();
                                HeaderTrainee.RequestDate = DateTime.Now;
                                HeaderTrainee.ScoreTheory = item.ScoreTheory;
                                HeaderTrainee.ScorePractice = item.ScorePractice;
                                HeaderTrainee.CalendarID = HeaderCalender.TrainNo;
                                HeaderTrainee.InYear = HeaderCalender.InYear;
                                HeaderTrainee.CourseID = HeaderCalender.CourseID;
                                HeaderTrainee.CourseName = HeaderCalender.CourseName;
                                HeaderTrainee.CourseCategory = HeaderCalender.CourseCategory;
                                HeaderTrainee.TrainingType = HeaderCalender.TrainingTypeID;
                                HeaderTrainee.EmpCode = item.EmpCode;
                                Core.DB.HRStaffProfile Staff = ListStaff.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                                if (Staff != null)
                                {
                                    HeaderTrainee.EmpName = Staff.AllName;
                                    if (!string.IsNullOrEmpty(Staff.DEPT))
                                    {
                                        var Dept = ListDepartment.FirstOrDefault(w => w.Code == Staff.DEPT);
                                        if (Dept != null) HeaderTrainee.Department = Dept.Description;
                                    }
                                    if (!string.IsNullOrEmpty(Staff.JobCode))
                                    {
                                        var Post = ListPosition.FirstOrDefault(w => w.Code == Staff.JobCode);
                                        if (Post != null) HeaderTrainee.Position = Post.Description;
                                    }
                                }
                                HeaderTrainee.CreatedBy = User.UserName;
                                HeaderTrainee.CreatedOn = DateTime.Today;
                                HeaderTrainee.Status = SYDocumentStatus.OPEN.ToString();
                                HeaderTrainee.ReStatus = SYDocumentStatus.OPEN.ToString();
                                context.TRTrainingEmployees.Add(HeaderTrainee);
                                context.SaveChanges();
                            }
                            dbContextTransaction.Commit();
                        }
                        catch (DbEntityValidationException e)
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderCalender.TrainNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderCalender.TrainNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditTrainee(int id)
        {
            var DBM = new HumicaDBContextTR();
            try
            {
                var objMatch = DB.TRTrainingEmployees.FirstOrDefault(w => w.TrainNo == id.ToString() && w.EmpCode == HeaderTrainee.EmpCode);
                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                objMatch.ScoreTheory = HeaderTrainee.ScoreTheory;
                objMatch.ScorePractice = HeaderTrainee.ScorePractice;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                DBM.TRTrainingEmployees.Attach(objMatch);
                DBM.Entry(objMatch).Property(w => w.ScoreTheory).IsModified = true;
                DBM.Entry(objMatch).Property(w => w.ScorePractice).IsModified = true;
                DBM.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                DBM.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                DBM.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                //log.DocurmentAction = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion
        public string getEmailContentByParam(string text, List<object> ListObjectDictionary, string URL)
        {
            string[] array = text.Split(' ');
            if (array.LongLength > 0)
            {
                string[] array2 = array;
                foreach (string param in array2)
                {
                    if (param.Trim() == "" || !(param.Substring(0, 1) == "@"))
                    {
                        continue;
                    }
                    SYEmailParameter sYEmailParameter = DP.SYEmailParameters.FirstOrDefault((SYEmailParameter w) => w.Parameter == param && w.TemplateID == "TELEGRAM");
                    if (sYEmailParameter != null && ListObjectDictionary.Count > 0)
                    {
                        string fieldValues = ClsInformation.GetFieldValues(sYEmailParameter.ObjectName, ListObjectDictionary, sYEmailParameter.FieldName, param);
                        if (fieldValues != null)
                        {
                            text = text.Replace(param, fieldValues);
                        }
                    }
                }
            }

            text = text.Replace("@LINK", URL);
            return text;
        }
        private WorkFlowResult Send_SMS_Telegram(string TemBody, string ChatID, List<object> ListObjectDictionary, string URL)
        {
            try
            {
                Core.DB.TelegramBot telegramBot = DBStaff.TelegramBots.FirstOrDefault((Core.DB.TelegramBot w) => w.ChatID == ChatID);
                if (telegramBot != null)
                {
                    string format = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=HTML";
                    string tokenID = telegramBot.TokenID;
                    string chatID = telegramBot.ChatID;
                    string emailContentByParam = getEmailContentByParam(TemBody, ListObjectDictionary, URL);
                    format = string.Format(format, tokenID, chatID, emailContentByParam);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    WebRequest webRequest = WebRequest.Create(format);
                    Stream responseStream = webRequest.GetResponse().GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream);
                    string text = "";
                    StringBuilder stringBuilder = new StringBuilder();
                    while (text != null)
                    {
                        text = streamReader.ReadLine();
                        if (text != null)
                        {
                            stringBuilder.Append(text);
                        }
                    }

                    string text2 = stringBuilder.ToString();
                    return WorkFlowResult.COMPLETED;
                }

                return WorkFlowResult.TELEGRAM_NOT_SEND;
            }
            catch (Exception e)
            {
                SYEventLog sYEventLog = new SYEventLog();
                sYEventLog.ScreenId = ScreenId;
                sYEventLog.UserId = User.UserName;
                sYEventLog.ScreenId = HeaderTrainee.EmpCode;
                sYEventLog.Action = SYActionBehavior.EDIT.ToString();
                SYEventLogObject.saveEventLog(sYEventLog, e, b: true);
                return WorkFlowResult.ERROR;
            }
        }
        public string InviteTheDoc(string IDs, string URL, string fileName)
        {
            try
            {
                if (String.IsNullOrEmpty(IDs))
                {
                    return "INV_TRAINEE";
                }

                var objStaff = DBStaff.HRStaffProfiles;
                var objTrainee = DB.TRTrainingEmployees;
                var objcal = DB.TRTrainingPlans;
                var TRTrainC = DB.TRTrainingCourses;
                List<string> ids = new List<string>(IDs.Split(','));

                List<TRTrainingEmployee> trainees = new List<TRTrainingEmployee>(objTrainee.ToList());
                trainees = trainees.Where(w => w.IsInvite != true && ids.Contains(w.TrainNo.ToString())).ToList();

                #region ---Telegram alert to Line Manager---

                var EmailTemplate = DP.TPEmailTemplates.Find("TRTraining");

                foreach (var trainee in trainees)
                {
                    List<object> ListObjectDictionary = new List<object>();
                    Core.DB.HRStaffProfile staff = new Core.DB.HRStaffProfile();

                    staff = objStaff.Find(trainee.EmpCode);
                    SYSendTelegramObject Tel = new SYSendTelegramObject();
                    Tel.User = User;
                    Tel.BS = BS;
                    TRTrainingCourse course = new TRTrainingCourse();
                    TRTrainingPlan calendar = new TRTrainingPlan();
                    calendar = objcal.Find(trainee.CalendarID);
                    course = TRTrainC.FirstOrDefault(w => w.Code == calendar.CourseID);
                    ListObjectDictionary.Add(calendar);
                    ListObjectDictionary.Add(staff);
                    ListObjectDictionary.Add(course);
                    //WorkFlowResult result2 = Tel.Send_Telegram(EmailTemplate1.EMTemplateObject, EmailTemplate1.RequestContent, Staff.TeleChartID, ListObjectDictionary, "");

                    WorkFlowResult result2 = Tel.Send_SMS_Telegram(EmailTemplate.EMTemplateObject, EmailTemplate.RequestContent, staff.TeleGroup, ListObjectDictionary, URL);
                    MessageError = Tel.getErrorMessage(result2);
                    if (String.IsNullOrEmpty(MessageError))
                    {
                        HeaderTrainee = new TRTrainingEmployee();
                        HeaderTrainee = objTrainee.FirstOrDefault(w => w.TrainNo == trainee.TrainNo);
                        HeaderTrainee.Status = "INVITED";
                        HeaderTrainee.CreatedBy = User.UserName;
                        HeaderTrainee.CreatedOn = DateTime.Now;
                        HeaderTrainee.IsInvite = true;
                        DB.Entry(HeaderTrainee).State = EntityState.Modified;
                        int row = DB.SaveChanges();
                    }
                }
                #endregion

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = HeaderTrainee.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public async Task<string> AcceptTheDoc(string ID)
        {
            DB = new HumicaDBContextTR();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                string[] Emp = ID.Split(';');
                foreach (var item in Emp)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] Train = item.Split(',');
                        int TrainNo = Convert.ToInt32(Train[0]);
                        string EmpCode = Train[1].ToString();
                        HeaderTrainee = await DB.TRTrainingEmployees.FirstOrDefaultAsync(w => w.TrainNo == TrainNo.ToString()&& w.EmpCode == EmpCode);
                        if (HeaderTrainee == null)
                        {
                            return "DOC_INV";
                        }

                        HeaderTrainee.IsAccept = true;
                        HeaderTrainee.Status = "ACCEPTED";
                        DB.Entry(HeaderTrainee).State = EntityState.Modified;
                        int row = await DB.SaveChangesAsync();
                    }
                }

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = HeaderTrainee.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
    }
    public class ModelTRTrainingCatalogue
    {
        public int? TrainNo { get; set; }
        public int InYear { get; set; }
        public string Description { get; set; }
        public int TotalCourse { get; set; }
    }
    public class Currency
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class TrainingStatus
    {
        public const string CLOSE = "C";
        public const string OPEN = "OPEN";
        public const string PROGRESSING = "P";
    }
}

