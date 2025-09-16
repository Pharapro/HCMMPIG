using Humica.Core;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Training
{
    public class TrainingCalenderObject
    {
        HumicaDBContextTR DB = new HumicaDBContextTR();
        public SYUser User { get; set; }
        public SMSystemEntity DP = new SMSystemEntity();
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public FTTraining FTTraining { get; set; }
        public TRTrainingCalender HeaderCalender { get; set; }
        public List<ClsTrainingPlan> ListPlan { get; set; }
        public List<TRTrainingCalender> ListTrainingCalender { get; set; }
        public List<TRTrainingAgenda> ListAgenda { get; set; }
        public TrainingCalenderObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<ClsTrainingPlan> LoadDataTrainingPlan()
        {
            List<ClsTrainingPlan> _listTemp = new List<ClsTrainingPlan>();
            string Status = SYDocumentStatus.RELEASED.ToString();

            var _listTPlan = from TP in DB.TRTrainingPlans
                             join TS in DB.TRTrainingSessions
                             on TP.TrainNo equals TS.TrainingCalendarID
                             where TP.Status == Status && TP.Restatus != "Created"
                             group TP by new
                             {
                                 TP.TrainNo,
                                 TP.InYear,
                                 TP.CourseName,
                                 TP.CourseCategory,
                                 TP.TrainingType,
                                 TS.InMonth,
                                 TS.Week
                             } into Plan
                             select new
                             {
                                 TrainNo = Plan.Key.TrainNo,
                                 InYear = Plan.Key.InYear,
                                 CourseName = Plan.Key.CourseName,
                                 CourseCategory = Plan.Key.CourseCategory,
                                 TrainingType = Plan.Key.TrainingType,
                                 InMonth = Plan.Key.InMonth,
                                 Week = Plan.Key.Week,
                                 TotalTopic = Plan.Count()
                             };
            foreach (var Item in _listTPlan)
            {
                var obj = new ClsTrainingPlan();
                obj.TrainNo = Item.TrainNo;
                obj.InYear = Item.InYear;
                obj.CourseName = Item.CourseName;
                obj.CourseCategory = Item.CourseCategory;
                obj.TrainingType = Item.TrainingType;
                obj.InMonth = Item.InMonth;
                obj.Week = Item.Week;
                obj.TotalTopic = Item.TotalTopic;
                _listTemp.Add(obj);
            }
            return _listTemp;
        }
        public string GetDataTrainingPlan(string ID)
        {
            HeaderCalender = new TRTrainingCalender();
            ListAgenda = new List<TRTrainingAgenda>();
            string[] Emp = ID.Split(';');
            string CusName = "";
            foreach (var item in Emp)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string[] Train = item.Split(',');

                    if (CusName == "" || CusName == Train[0])
                    {
                        CusName = Train[0];
                        int TrainNo = 0;
                        int InMonth = 0;
                        int Week = 0;
                        if (!string.IsNullOrEmpty(Train[0]))
                            TrainNo = Convert.ToInt32(Train[0]);
                        if (!string.IsNullOrEmpty(Train[2]))
                            InMonth = Convert.ToInt32(Train[1]);
                        if (!string.IsNullOrEmpty(Train[2]))
                            Week = Convert.ToInt32(Train[2]);

                        var ListSession = new List<TRTrainingSession>();
                        if (TrainNo > 0)
                        {
                            ListSession = DB.TRTrainingSessions.Where(w => w.TrainingCalendarID == TrainNo).ToList();
                            if (InMonth > 0)
                            {
                                ListSession = DB.TRTrainingSessions.Where(w => w.TrainingCalendarID == TrainNo &&
                                                                                w.InMonth == InMonth).ToList();
                                if (Week > 0)
                                {
                                    ListSession = DB.TRTrainingSessions.Where(w => w.TrainingCalendarID == TrainNo &&
                                                                                    w.InMonth == InMonth && w.Week == Week).ToList();
                                }
                            }

                        }
                        var objPlan = DB.TRTrainingPlans.FirstOrDefault(w => w.TrainNo == TrainNo);
                        if (objPlan == null)
                        {
                            return "DOC_INV";
                        }
                        HeaderCalender.InYear = objPlan.InYear;
                        HeaderCalender.CourseID = objPlan.CourseID;
                        HeaderCalender.CourseCategoryID = objPlan.CourseCategoryID;
                        HeaderCalender.TrainingTypeID = objPlan.TrainingTypeID;
                        HeaderCalender.MinTrainee = objPlan.MinTrainee;
                        HeaderCalender.MaxTrainee = objPlan.MinTrainee;
                        HeaderCalender.TotalCost = objPlan.TotalCost;
                        HeaderCalender.Status = SYDocumentStatus.OPEN.ToString();
                        HeaderCalender.StartDate = DateTime.Now;
                        HeaderCalender.EndDate = DateTime.Now;
                        HeaderCalender.RequesterCode = objPlan.RequesterCode;

                        
                        int LineItem = 0;
                        foreach (var Session in ListSession)
                        {
                            LineItem += 1;
                            var obj = new TRTrainingAgenda();
                            obj.LineItem = LineItem;
                            obj.TrainerType = Session.TrainerType;
                            obj.TrainerID = Session.TrainerID;
                            obj.TrainerNames = Session.TrainerNames;
                            obj.TopicID = Session.TopicID;
                            obj.InMonth = Session.InMonth;
                            obj.Week = Session.Week;
                            ListAgenda.Add(obj);
                        }
                    }
                    else if (CusName != Train[0])
                    {
                        return "INVALID_CUSNAME";
                    }
                }
            }

            return SYConstant.OK;
        }
        public string CreateCalender()
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
                            if (HeaderCalender.InYear == null)
                            {
                                return "IN_YEAR";
                            }
                            if (string.IsNullOrEmpty(HeaderCalender.CourseID))
                            {
                                return "COURSE_NAME";
                            }
                            //if (context.TRTrainingCalendars.Where(w => w.InYear == HeaderCalender.InYear
                            //    && w.CourseID == HeaderCalender.CourseID).Count() > 0)
                            //{
                            //    return "TRAINING_EXIST";
                            //}
                            #region training calendar
                            var CourseType = DB.TRTrainingCourses.FirstOrDefault(w => w.Code == HeaderCalender.CourseID);
                            if (CourseType != null)
                            {
                                HeaderCalender.CourseName = CourseType.Description;
                            }
                            var TrainType = DB.TRTrainingTypes.FirstOrDefault(w => w.Code == HeaderCalender.TrainingTypeID);
                            if (TrainType != null)
                            {
                                HeaderCalender.TrainingType = TrainType.Description;
                            }
                            var CourseCategory = DB.TRCourseCategories.FirstOrDefault(w => w.Code == HeaderCalender.CourseCategoryID);
                            if (CourseCategory != null)
                            {
                                HeaderCalender.CourseCategory = CourseCategory.Description;
                            }
                            HeaderCalender = HeaderCalender;
                            HeaderCalender.CreatedBy = User.UserName;
                            HeaderCalender.CreatedOn = DateTime.Today;
                            context.TRTrainingCalenders.Add(HeaderCalender);

                            int row = context.SaveChanges();
                            i = HeaderCalender.TrainNo;
                            var CalCreate = DB.TRTrainingPlans.FirstOrDefault(w => w.TrainNo == i);
                            if (CalCreate != null)
                            {
                                CalCreate.Restatus = "Created";
                                context.TRTrainingPlans.Attach(CalCreate);
                                context.Entry(CalCreate).State = EntityState.Modified;
                            }
                            int row1 = context.SaveChanges();
                            #endregion
                            #region session
                            List<TRTrainerInfo> _listTrainerInfo = DB.TRTrainerInfo.ToList();
                            int lineItem = 0;
                            foreach (var item in ListAgenda)
                            {
                                lineItem += 1;
                                TRTrainerInfo TrainerInfo = _listTrainerInfo.FirstOrDefault(w => w.TrainNo == Convert.ToInt32(item.TrainerID));
                                if (TrainerInfo != null) item.TrainerNames = TrainerInfo.TrainerName;
                                DateTime? fromTime = this.TimeNullable(item.FromTime);
                                DateTime? toTime = this.TimeNullable(item.ToTime);
                                item.FromTime = fromTime;
                                item.ToTime = toTime;
                                item.CalendarID = i;
                                item.CreatedBy = User.UserName;
                                item.CreatedOn = DateTime.Today;
                                item.LineItem = lineItem;
                                context.TRTrainingAgendas.Add(item);
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
                log.DocurmentAction = HeaderCalender.TrainNo.ToString();
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
                log.DocurmentAction = HeaderCalender.TrainNo.ToString();
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditCalender(int id)
        {

            var DBM = new HumicaDBContextTR();
            try
            {
                DBM.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    var calendar = DB.TRTrainingCalenders.FirstOrDefault(w => w.TrainNo == id);
                    if (calendar == null)
                        return "INV_DOC";
                    var _ListAgenda = DB.TRTrainingAgendas.Where(w => w.CalendarID == id).ToList();
                    foreach (var item in _ListAgenda)
                    {
                        DBM.TRTrainingAgendas.Attach(item);
                        DBM.Entry(item).State = EntityState.Deleted;
                    }
                    calendar.ChangedBy = User.UserName;
                    calendar.ChangedOn = DateTime.Now;
                    calendar.CourseID = HeaderCalender.CourseID;
                    calendar.TrainingTypeID = HeaderCalender.TrainingTypeID;
                    calendar.CourseCategoryID = HeaderCalender.CourseCategoryID;
                    calendar.VenueCode = HeaderCalender.VenueCode;
                    calendar.StartDate = HeaderCalender.StartDate;
                    calendar.EndDate = HeaderCalender.EndDate;
                    calendar.MinTrainee = HeaderCalender.MinTrainee;
                    calendar.MaxTrainee = HeaderCalender.MaxTrainee;
                    calendar.TotalCost = HeaderCalender.TotalCost;
                    calendar.Status = HeaderCalender.Status;
                    DBM.Entry(calendar).State = EntityState.Modified;
                    List<TRTrainerInfo> _listTrainerInfo = DB.TRTrainerInfo.ToList();
                    int lineItem = 0;
                    foreach (var item in ListAgenda)
                    {
                        lineItem += 1;
                        TRTrainerInfo TrainerInfo = _listTrainerInfo.FirstOrDefault(w => w.TrainNo == Convert.ToInt32(item.TrainerID));
                        if (TrainerInfo != null) item.TrainerNames = TrainerInfo.TrainerName;
                        DateTime? fromTime = this.TimeNullable(item.FromTime);
                        DateTime? toTime = this.TimeNullable(item.ToTime);
                        item.FromTime = fromTime;
                        item.ToTime = toTime;
                        item.CalendarID = calendar.TrainNo;
                        item.CreatedBy = User.UserName;
                        item.CreatedOn = DateTime.Today;
                        item.LineItem = lineItem;
                        DBM.TRTrainingAgendas.Add(item);
                    }
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
        public string DeleteCalender(int id)
        {
            try
            {
                HeaderCalender = new TRTrainingCalender();
                var ObjMatch = DB.TRTrainingCalenders.FirstOrDefault(w => w.TrainNo == id);
                if (ObjMatch == null)
                {
                    return "DOC_NE";
                }
                if (ObjMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "DOC_NE";
                }
                HeaderCalender = ObjMatch;
                DB.TRTrainingCalenders.Attach(ObjMatch);
                DB.Entry(ObjMatch).State = System.Data.Entity.EntityState.Deleted;
                //delete record from session
                ListAgenda = DB.TRTrainingAgendas.Where(x => x.CalendarID == id).ToList();
                foreach (var item in ListAgenda)
                {
                    DB.TRTrainingAgendas.Attach(item);
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
        // end training attendance
        public DateTime? TimeNullable(DateTime? value)
        {
            DateTime? dateTime = null;
            return value.HasValue ? value.Time() : dateTime;

        }
        public void EditSession(TRTrainingAgenda Agenda)
        {
            string msg = string.Empty;
            try
            {
                using (var con = new HumicaDBContextTR())
                {
                    var trainingSession = ListAgenda.FirstOrDefault(x => x.LineItem == Agenda.LineItem);
                    if (trainingSession != null)
                    {
                        DateTime? fromTime = this.TimeNullable(Agenda.FromTime);
                        DateTime? toTime = this.TimeNullable(Agenda.ToTime);
                        trainingSession.FromTime = fromTime;
                        trainingSession.ToTime = toTime;
                        trainingSession.TrainerType = Agenda.TrainerType;
                        trainingSession.TrainerID = Agenda.TrainerID;
                        trainingSession.TopicID = Agenda.TopicID;
                        trainingSession.TrainingDate = Agenda.TrainingDate;
                        trainingSession.InMonth = Agenda.InMonth;
                        trainingSession.Week = Agenda.Week;
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

        public string ReleaseTheDoc(int id)
        {
            HumicaDBContextTR DBX = new HumicaDBContextTR();
            try
            {

                var objMatch = DB.TRTrainingCalenders.FirstOrDefault(w => w.TrainNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                HeaderCalender = objMatch;

                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }
                objMatch.Status = SYDocumentStatus.RELEASED.ToString();
              
                DBX.TRTrainingCalenders.Attach(objMatch);
                DBX.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DBX.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = HeaderCalender.TrainNo.ToString();
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
                log.DocurmentAction = HeaderCalender.TrainNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ClosedTheDoc(int id)
        {
            HumicaDBContextTR DBX = new HumicaDBContextTR();
            try
            {

                var objMatch = DB.TRTrainingCalenders.FirstOrDefault(w => w.TrainNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                HeaderCalender = objMatch;

                //if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                //{
                //    return "INV_DOC";
                //}
                objMatch.Status = SYDocumentStatus.CLOSED.ToString();

                DBX.TRTrainingCalenders.Attach(objMatch);
                DBX.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DBX.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = HeaderCalender.TrainNo.ToString();
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
                log.DocurmentAction = HeaderCalender.TrainNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

    }

    public class ClsTrainingPlan
    {
        public int TrainNo { get; set; }
        public int LineItem { get; set; }
        public int? InYear { get; set; }
        public string CourseName { get; set; }
        public string CourseCategory { get; set; }
        public string TrainingType { get; set; }
        public int? InMonth { get; set; }
        public int? Week { get; set; }
        public int TotalTopic { get; set; }
    }
}