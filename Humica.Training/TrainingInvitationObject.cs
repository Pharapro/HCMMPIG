using Humica.Core.FT;
using Humica.Core.SY;
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
using System.Security.Policy;
using System.Threading.Tasks;

namespace Humica.Training
{
    public class TrainingInvitationObject
    {
        HumicaDBContextTR DB = new HumicaDBContextTR();
        Core.DB.HumicaDBContext DBStaff = new Core.DB.HumicaDBContext();
       
        public SMSystemEntity DP = new SMSystemEntity();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public FTTraining FTTraining { get; set; }
        public TRTrainingInvitation Header { get; set; }
        public TRTrainingCalender HeaderCalender { get; set; }
        public List<Core.DB.HRStaffProfile> ListStaff { get; set; }
        public List<TRTrainingEmployee> ListTrainee { get; set; }
        public List<TRTrainingInvitation> ListInvitation { get; set; }
        public TRTrainingEmployee HeaderTrainee { get; set; }
        public List<string> ListEmpCode { get; set; }
        public List<TRTrainingCalender> ListCalender { get; set; }
        public List<TRPendingDeptInvit> ListPendingDeptInvit { get; set; }
        public TrainingInvitationObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public async Task<string> GetCalender(int ID)
        {
            Header = new TRTrainingInvitation();
            ListEmpCode = new List<string>();
            HeaderCalender = await DB.TRTrainingCalenders.FirstOrDefaultAsync(w => w.TrainNo == ID);
            if (HeaderCalender == null)
            {
                return "DOC_INV";
            }
            var _ListTempTrainee = await DB.TRTrainingEmployees.Where(x => x.CalendarID == ID).ToListAsync();
            if (_ListTempTrainee.Count() > 0)
                _ListTempTrainee.ToList().ForEach(w => ListEmpCode.Add(w.EmpCode));

            Header.InYear = HeaderCalender.InYear.Value;
            Header.CalendarID = HeaderCalender.TrainNo;
            Header.CourseID = HeaderCalender.CourseID;
            Header.CourseName = HeaderCalender.CourseName;
            Header.TrainingTypeID = HeaderCalender.TrainingTypeID;
            Header.TrainingType = HeaderCalender.TrainingType;
            Header.CourseCategoryID = HeaderCalender.CourseCategoryID;
            Header.CourseCategory = HeaderCalender.CourseCategory;
            Header.ScheduleFrom = HeaderCalender.StartDate.Value;
            Header.ScheduleTo = HeaderCalender.EndDate.Value;
            Header.ScoreTheory = 0;
            Header.ScorePractice = 0;
            Header.Target = 0;
            Header.Capacity = 0;
            Header.RequestDate = DateTime.Now;
            Header.Status = SYDocumentStatus.OPEN.ToString();

            return SYConstant.OK;
        }
        public async Task GetAllStaff()
        {

            var tblStaffProfile =await DBStaff.HRStaffProfiles.Where(w => w.Status == "A").ToListAsync();
            this.ListStaff = tblStaffProfile.Where(w => !this.ListEmpCode.Contains(w.EmpCode)).ToList();
        }
        public string CreateTrainee()
        {
            try
            {
                int i = 0;
                List<Core.DB.HRStaffProfile> ListStaff = DBStaff.HRStaffProfiles.Where(w => w.Status == "A").ToList();
                List<Core.DB.HRDepartment> ListDepartment = DBStaff.HRDepartments.ToList();
                List<Core.DB.HRPosition> ListPosition = DBStaff.HRPositions.ToList();
                List<TRTrainingRequirement> ListGroup = DB.TRTrainingRequirements.Where(w => w.Category == "G").ToList();
                using (var context = new HumicaDBContextTR())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            Header.CalendarID = HeaderCalender.TrainNo;
                            Header.InYear = HeaderCalender.InYear.Value;
                            Header.CourseName = HeaderCalender.CourseName;
                            Header.TrainingType = HeaderCalender.TrainingType;
                            Header.CourseCategory = HeaderCalender.CourseCategory;
                            Header.CreatedBy = User.UserName;
                            Header.CreatedOn = DateTime.Now;

                            context.TRTrainingInvitations.Add(Header);

                            HeaderCalender.Restatus = "Created";
                            context.TRTrainingCalenders.Attach(HeaderCalender);
                            context.Entry(HeaderCalender).State = EntityState.Modified;

                            int row = context.SaveChanges();
                            i = (int)Header.TrainNo;
                            int LineItem = 0;
                            var Trainner = DB.TRTrainingAgendas.FirstOrDefault(w => w.CalendarID == Header.CalendarID);
                            foreach (var item in ListTrainee)
                            {
                                LineItem += 1;
                                HeaderTrainee = new TRTrainingEmployee();
                                HeaderTrainee.TrainNo = Header.TrainNo.ToString();
                                HeaderTrainee.LineItem = LineItem;
                                //HeaderTrainee.RequestDate = DateTime.Now;
                                HeaderTrainee.ScoreTheory = item.ScoreTheory;
                                HeaderTrainee.ScorePractice = item.ScorePractice;
                                HeaderTrainee.CalendarID = HeaderCalender.TrainNo;
                                HeaderTrainee.InYear = HeaderCalender.InYear;
                                HeaderTrainee.CourseID = HeaderCalender.CourseID;
                                HeaderTrainee.CourseName = HeaderCalender.CourseName;
                                HeaderTrainee.CourseCategoryID = HeaderCalender.CourseCategoryID;
                                HeaderTrainee.CourseCategory = HeaderCalender.CourseCategory;
                                HeaderTrainee.TrainingType = HeaderCalender.TrainingTypeID;
                                HeaderTrainee.EmpCode = item.EmpCode;
                                HeaderTrainee.TrainerName = Trainner.TrainerNames;
                                HeaderTrainee.Status = SYDocumentStatus.PENDING.ToString();
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
                                //var _GP = ListGroup.FirstOrDefault(w => w.ID == Convert.ToInt32(Header.TrainingGroup));
                                //if (_GP != null) HeaderTrainee.GroupDescription = _GP.Description;
                                HeaderTrainee.CreatedBy = User.UserName;
                                HeaderTrainee.CreatedOn = DateTime.Today;
                                //HeaderTrainee.Status = SYDocumentStatus.OPEN.ToString();
                                //HeaderTrainee.ReStatus = SYDocumentStatus.OPEN.ToString();
                                context.TRTrainingEmployees.Add(HeaderTrainee);
                                context.SaveChanges();

                                //var Trainee = new TRPendingDeptInvit();
                                //Trainee.DocumentNo = Header.TrainNo;
                                //Trainee.InYear = Header.InYear;
                                //Trainee.CourseName = Header.CourseName;
                                //Trainee.CourseCategory = Header.CourseCategory;
                                //Trainee.StartDate = Header.ScheduleFrom;
                                //Trainee.EndDate = Header.ScheduleTo;
                                //Trainee.Department = Staff.DEPT;
                                //Trainee.Status = Header.Status;
                                //ListPendingDeptInvit.Add(Trainee);
                            }
                            //var ListGroupDept = ListPendingDeptInvit.GroupBy(w => w.Department).ToList();
                            //foreach(var item in ListGroupDept)
                            //{
                            //    var obj = new TRPendingDeptInvit();
                            //    obj.Status = Header.Status;
                            //    obj.InYear = Header.InYear;
                            //    obj.Department = item.Key;
                            //    obj.DocumentNo = Header.TrainNo;
                            //    obj.CourseName = Header.CourseName;
                            //    obj.CourseCategory = Header.CourseCategory;
                            //    obj.StartDate = Header.ScheduleFrom;
                            //    obj.EndDate = Header.ScheduleTo;
                            //    obj.Status = Header.Status;
                            //    context.TRPendingDeptInvits.Add(obj);
                            //}
                            context.SaveChanges();
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
            List<Core.DB.HRStaffProfile> ListStaff = DBStaff.HRStaffProfiles.Where(w => w.Status == "A").ToList();
            List<Core.DB.HRDepartment> ListDepartment = DBStaff.HRDepartments.ToList();
            List<Core.DB.HRPosition> ListPosition = DBStaff.HRPositions.ToList();
            List<TRTrainingRequirement> ListGroup = DB.TRTrainingRequirements.Where(w => w.Category == "G").ToList();
            var DBM = new HumicaDBContextTR();
            try
            {
                DBM.Configuration.AutoDetectChangesEnabled = false;
                try
                {
                    var ObjMatch = DB.TRTrainingInvitations.FirstOrDefault(w => w.TrainNo == id);
                    if (ObjMatch == null)
                        return "INV_DOC";
                    var _ListAgenda = DB.TRTrainingEmployees.Where(w => w.TrainNo == ObjMatch.TrainNo.ToString()).ToList();
                    foreach (var item in _ListAgenda)
                    {
                        DBM.TRTrainingEmployees.Attach(item);
                        DBM.Entry(item).State = EntityState.Deleted;
                    }
                    ObjMatch.ChangedBy = User.UserName;
                    ObjMatch.ChangedOn = DateTime.Now;
                    ObjMatch.RequirementCode = Header.RequirementCode;
                    ObjMatch.ScorePractice = Header.ScorePractice;
                    ObjMatch.TrainingGroup = Header.TrainingGroup;
                    ObjMatch.Target = Header.Target;
                    ObjMatch.TrainingGroup = Header.TrainingGroup;
                    ObjMatch.Venue = Header.Venue;
                    DBM.Entry(ObjMatch).State = EntityState.Modified;
                    int LineItem = 0;
                    foreach (var item in ListTrainee)
                    {
                        LineItem += 1;
                        HeaderTrainee = new TRTrainingEmployee();
                        HeaderTrainee.TrainNo = ObjMatch.TrainNo.ToString();
                        HeaderTrainee.LineItem = LineItem;
                        HeaderTrainee.ScoreTheory = item.ScoreTheory;
                        HeaderTrainee.ScorePractice = item.ScorePractice;
                        HeaderTrainee.CalendarID = (int)ObjMatch.CalendarID;
                        HeaderTrainee.InYear = ObjMatch.InYear;
                        HeaderTrainee.CourseID = ObjMatch.CourseID;
                        HeaderTrainee.CourseName = ObjMatch.CourseName;
                        HeaderTrainee.CourseCategoryID = ObjMatch.CourseCategoryID;
                        HeaderTrainee.CourseCategory = ObjMatch.CourseCategory;
                        HeaderTrainee.TrainingType = ObjMatch.TrainingTypeID;
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
                        var _GP = ListGroup.FirstOrDefault(w => w.ID ==Convert.ToInt32( Header.TrainingGroup));
                        if (_GP != null) HeaderTrainee.GroupDescription = _GP.Description;
                        HeaderTrainee.CreatedBy = User.UserName;
                        HeaderTrainee.CreatedOn = DateTime.Today;
                        HeaderTrainee.Status = SYDocumentStatus.OPEN.ToString();
                        HeaderTrainee.ReStatus = SYDocumentStatus.OPEN.ToString();
                        DBM.TRTrainingEmployees.Add(HeaderTrainee);
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
                    log.Action = SYActionBehavior.EDIT.ToString();

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
                    log.Action = SYActionBehavior.EDIT.ToString();

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
                    log.Action = SYActionBehavior.EDIT.ToString();

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
        public string requestToApprove(long id, string fileName,string URL)
        {
            try
            {
                var objMatch = DB.TRTrainingInvitations.FirstOrDefault(w => w.TrainNo == id);
                var Agenda = DB.TRTrainingAgendas.FirstOrDefault(w => w.CalendarID == objMatch.CalendarID);
                var ListTrainee = DB.TRTrainingEmployees.Where(w => w.TrainNo == objMatch.TrainNo.ToString()).ToList();

                objMatch.Status = "REQUESTED";
                DB.Entry(objMatch).State = EntityState.Modified;

                foreach (var item in ListTrainee)
                {
                    item.Status = SYDocumentStatus.PENDING.ToString();
                    item.InviteTrainingDate = Agenda.TrainingDate;
                    item.IsInvite = true;
                    DB.TRTrainingEmployees.Attach(item);
                    DB.Entry(item).State = EntityState.Modified;
                }
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;

                
                DB.SaveChanges();
                #region Notifican on Mobile
                //var ListDept = DB.TRPendingDeptInvits.Where(w => w.DocumentNo == id).ToList();
                //foreach(var i in ListDept)
                //{
                //    var emp =  DBStaff.HRStaffProfiles.Where(w => w.DEPT == i.Department).FirstOrDefault();
                //    var HOD =  DBStaff.HRStaffProfiles.Where(w => w.EmpCode == emp.HODCode).FirstOrDefault();
                //    var FirebaseID = DBStaff.TokenResources.FirstOrDefault(w => w.IsLock == true && w.UserName == HOD.HODCode);
                //    if(FirebaseID != null)
                //    {
                //        string Desc = "Dear " + HOD.Title + " " + HOD.AllName  + ", We would like to request you to check and approved staff trainning invitation in your department. Please review and check.";
                //        Notification.Notificationf Noti = new Notification.Notificationf();
                //        var LineToken = new List<string>();
                //        LineToken.Add(FirebaseID.FirebaseID);
                //        Noti.SendNotification(LineToken, " Staff Training", Desc, fileName);
                //    }
                //}
                #endregion
                #region Send Email
                foreach (var item in ListTrainee)
                {
                    var Trainee = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                    var DocNo = item.ID;
                    if (Trainee != null)
                    {
                        if (item.Status == SYDocumentStatus.PENDING.ToString())
                        {
                            SYWorkFlowEmailObject wfo = new SYWorkFlowEmailObject("HRREQTRAIN", WorkFlowType.REQUESTER, UserType.N, SYDocumentStatus.PENDING.ToString());
                            wfo.SelectListItem = new SYSplitItem(Convert.ToString(DocNo));
                            wfo.User = User;
                            wfo.BS = BS;
                            wfo.ScreenId = ScreenId;
                            wfo.UrlView = URL;
                            wfo.Module = "HR"; // CModule.PURCHASE.ToString();
                            wfo.DocNo = DocNo.ToString();
                            wfo.ListLineRef = new List<BSWorkAssign>();
                            wfo.Action = SYDocumentStatus.PENDING.ToString();
                            wfo.ListObjectDictionary = new List<object>();
                            wfo.ListObjectDictionary.Add(Trainee);
                            wfo.ListObjectDictionary.Add(objMatch);

                            if (!string.IsNullOrEmpty(Trainee.Email))
                            {
                                wfo.ListObjectDictionary.Add(Trainee);
                                WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Trainee);
                                MessageError = wfo.getErrorMessage(result1);
                            }
                        }
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
                log.UserId = User.UserName;
                log.ScreenId = Header.TrainNo.ToString();
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
                log.DocurmentAction = Header.TrainNo.ToString();
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
                log.DocurmentAction = Header.TrainNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string approveTheDoc(int id, string fileName)
        {
            try
            {
                HumicaDBContextTR DBX = new HumicaDBContextTR();
                var objMatch = DB.TRTrainingInvitations.FirstOrDefault(w => w.TrainNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;

                if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                {
                    return "INV_DOC";
                }
                //var Trainee = DBX.TRTrainingEmployees.Where(w => w.TrainNo == Header.TrainNo.ToString()).ToList();
                //if (Trainee.Count > 0)
                //{
                //    foreach (var x in Trainee)
                //    {
                //        var FirebaseID = DBStaff.TokenResources.FirstOrDefault(w => w.IsLock == true && w.UserName == x.EmpCode);
                //        if (FirebaseID != null)
                //        {
                //            string Desc = "You have request to join staff trainning by HR.";
                //            //Notification.Notificationf Noti = new Notification.Notificationf();
                //            var LineToken = new List<string>();
                //            LineToken.Add(FirebaseID.FirebaseID);
                //            //Noti.SendNotification(LineToken, " Staff Training", Desc, fileName);
                //        }
                //    }
                //}

                var status = SYDocumentStatus.APPROVED.ToString();

                objMatch.Status = status;

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
                log.ScreenId = Header.TrainNo.ToString();
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
                log.DocurmentAction = Header.TrainNo.ToString();
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
                log.DocurmentAction = Header.TrainNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ReleaseTheDoc(int id)
        {
            HumicaDBContextTR DBX = new HumicaDBContextTR();
            try
            {
               
                var objMatch = DB.TRTrainingInvitations.FirstOrDefault(w => w.TrainNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;

                if (objMatch.Status != SYDocumentStatus.APPROVED.ToString())
                {
                    return "INV_DOC";
                }
                objMatch.Status = SYDocumentStatus.RELEASED.ToString();
                var Agenda = DB.TRTrainingAgendas.FirstOrDefault(w => w.CalendarID == objMatch.CalendarID);
                var ListStaff = DB.TRTrainingEmployees.Where(w => w.TrainNo == objMatch.TrainNo.ToString()).ToList();
                foreach(var item in ListStaff)
                {
                    item.Status= objMatch.Status;
                    item.InviteTrainingDate = Agenda.TrainingDate;
                    item.IsInvite = true;
                    DBX.TRTrainingEmployees.Attach(item);
                    DBX.Entry(item).Property(w => w.Status).IsModified = true;
                    DBX.Entry(item).Property(w => w.InviteTrainingDate).IsModified = true;
                    DBX.Entry(item).Property(w => w.IsInvite).IsModified = true;
                }
                DBX.TRTrainingInvitations.Attach(objMatch);
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
                log.ScreenId = Header.TrainNo.ToString();
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
                log.DocurmentAction = Header.TrainNo.ToString();
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
                log.DocurmentAction = Header.TrainNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CancelTheDoc(string ApprovalID)
        {
            try
            {
                string cancelled = SYDocumentStatus.CANCELLED.ToString();
                string PONumber = ApprovalID;
                var objmatch = DB.TRTrainingInvitations.Find(PONumber);
                if (objmatch.Status != cancelled)
                {
                    objmatch.Status = cancelled;
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
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
                log.DocurmentAction = ApprovalID;
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
                log.DocurmentAction = ApprovalID;
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
                log.DocurmentAction = ApprovalID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
    public class ClsPendingDeptInvit
    {
        public int InYear { set; get; }
        public string CourseName { set; get; }
        public string CourseCategory { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string Department { set; get; }
        public string Status { set; get; }
    }
}