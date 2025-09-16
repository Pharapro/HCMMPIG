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
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace Humica.Training
{
    public class ESSTrainingInvitationObject
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
        public TRTrainingAgenda Agenda { get; set; }
        public List<Core.DB.HRStaffProfile> ListStaff { get; set; }
        public List<TRTrainingEmployee> ListTrainee { get; set; }
        public List<TRTrainingEmployee> ListPendingTrainee { get; set; }
        public List<TRTrainingInvitation> ListInviPending { get; set; }
        public List<TRTrainingInvitation> ListInviApproved { get; set; }
        public List<TRPendingDeptInvit> ListApproved { get; set; }
        public List<TRPendingDeptInvit> ListPending { get; set; }
        public TRTrainingEmployee HeaderTrainee { get; set; }
        public List<string> ListEmpCode { get; set; }
        public List<TRTrainingCalender> ListCalender { get; set; }
        public ESSTrainingInvitationObject()
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

                            int row = context.SaveChanges();
                            i = (int)Header.TrainNo;
                            int LineItem = 0;
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
                                var _GP = ListGroup.FirstOrDefault(w => w.ID == Convert.ToInt32(Header.TrainingGroup));
                                if (_GP != null) HeaderTrainee.GroupDescription = _GP.Description;
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
        public string requestToApprove(int id)
        {
            try
            {
                var objMatch = DB.TRTrainingInvitations.FirstOrDefault(w => w.TrainNo == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                Header = objMatch;

                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }

                objMatch.Status = SYDocumentStatus.PENDING.ToString();

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
        public string approveTheDoc(int id ,string fileName)
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
                var HeadOfDept = DBStaff.HRStaffProfiles.Where(w => w.HODCode == User.UserName).FirstOrDefault();
                foreach(var staff in ListTrainee)
                {
                    staff.IsInvite = true;
                    DB.TRTrainingEmployees.Attach(staff);
                    DB.Entry(staff).Property(w => w.IsInvite).IsModified = true;
                }
                var ObjDept = DB.TRPendingDeptInvits.Where(w => w.DocumentNo == id && w.Department == HeadOfDept.DEPT && w.Status == SYDocumentStatus.PENDING.ToString()).FirstOrDefault();
                if (ObjDept != null)
                {
                    ObjDept.Status =SYDocumentStatus.APPROVED.ToString();
                    DB.Entry(ObjDept).Property(w => w.Status).IsModified = true;
                    DB.SaveChanges();
                    
                }
                string Approved = SYDocumentStatus.APPROVED.ToString();
                var ListAppInv = DB.TRPendingDeptInvits.Where(w => w.DocumentNo == id && w.Status != Approved).ToList();
                if (ListAppInv.Count == 0)
                {
                    objMatch.Status = Approved;
                }
                #region Notifican on Mobile

                //var ListStaffDept = DBStaff.HRStaffProfiles.Where(w => w.HODCode == User.UserName).ToList();
                //var Trainee = DBX.TRTrainingEmployees.Where(w => w.TrainNo == Header.TrainNo.ToString()).ToList();
                //if(Trainee.Count > 0)
                //{
                //    Trainee = Trainee
                //    .Where(w => ListStaffDept.Any(x => x.EmpCode == w.EmpCode))
                //    .ToList();
                //    foreach (var x in Trainee) {
                //        var FirebaseID = DBStaff.TokenResources.FirstOrDefault(w => w.IsLock == true && w.UserName == x.EmpCode);
                //        if (FirebaseID != null)
                //        {
                //            string Desc = "You have request to join staff trainning by Head of Department.";
                //            Notification.Notificationf Noti = new Notification.Notificationf();
                //            var LineToken = new List<string>();
                //            LineToken.Add(FirebaseID.FirebaseID);
                //            Noti.SendNotification(LineToken, " Staff Training", Desc, fileName);
                //        }
                //    }

                //}


                #endregion
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
                var ListStaff = DB.TRTrainingEmployees.Where(w => w.TrainNo == objMatch.TrainNo.ToString()).ToList();
                foreach(var item in ListStaff)
                {
                    item.Status= objMatch.Status;
                    item.ReStatus = objMatch.Status;
                    item.IsInvite = true;
                    DBX.TRTrainingEmployees.Attach(item);
                    DBX.Entry(item).Property(w => w.Status).IsModified = true;
                    DBX.Entry(item).Property(w => w.ReStatus).IsModified = true;
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
        public string CancelTheDoc(long ApprovalID)
        {
            try
            {
                string cancelled = SYDocumentStatus.CANCELLED.ToString();
                var objmatch = DB.TRTrainingInvitations.Find(ApprovalID);
                if (objmatch.Status != cancelled)
                {
                    objmatch.Status = cancelled;
                    DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                }
                var HeadOfDept = DBStaff.HRStaffProfiles.Where(w => w.HODCode == User.UserName).FirstOrDefault();
                var ObjDept = DB.TRPendingDeptInvits.Where(w => w.DocumentNo == ApprovalID && w.Department == HeadOfDept.DEPT && w.Status == SYDocumentStatus.PENDING.ToString()).FirstOrDefault();
                if (ObjDept != null)
                {
                    ObjDept.Status = cancelled;
                    DB.Entry(ObjDept).Property(w => w.Status).IsModified = true;
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
                log.DocurmentAction = ApprovalID.ToString();
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
                log.DocurmentAction = ApprovalID.ToString();
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
                log.DocurmentAction = ApprovalID.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public async Task<string> AcceptTheDoc(long ID,string fileName)
        {
            DB = new HumicaDBContextTR();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
               
                HeaderTrainee = await DB.TRTrainingEmployees.FirstOrDefaultAsync(w => w.TrainNo == ID.ToString() && w.EmpCode == User.UserName);
                if (HeaderTrainee == null)
                {
                    return "DOC_INV";
                }
                var TRInv = DB.TRTrainingInvitations.FirstOrDefault(w => w.TrainNo == ID);
                if (DateTime.Now.Date > TRInv.DeadlineAcc.Value.Date)
                {
                    return "Please check deadline accept wiht HR";
                }
                HeaderTrainee.IsAccept = true;
                HeaderTrainee.Status = "ACCEPTED";
                DB.Entry(HeaderTrainee).State = EntityState.Modified;
                int row = await DB.SaveChangesAsync();
                #region Notifican send to manager
                //var EmpCode = DBStaff.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).FirstOrDefault();
                //var Hod = DBStaff.HRStaffProfiles.Where(w => w.EmpCode == EmpCode.HODCode).FirstOrDefault();
                //var FirebaseHod = DBStaff.TokenResources.FirstOrDefault(w => w.IsLock == true && w.UserName == EmpCode.HODCode);
                //if (FirebaseHod != null)
                //{
                //    string Desc = "Dear " + Hod.Title + " " + Hod.AllName + ", " + EmpCode.Title + " " +EmpCode.AllName +" have been accept the training invitation.";
                //    Notification.Notificationf Noti = new Notification.Notificationf();
                //    var LineToken = new List<string>();
                //    LineToken.Add(FirebaseHod.FirebaseID);
                //    await Noti.SendNotification(LineToken, " Staff Training", Desc, fileName);
                //}
                #endregion
                #region ---
                //var ListEmpInvit = await DB.TRTrainingEmployees.Where(w => w.TrainNo == ID.ToString() && w.Status != "ACCEPTED").ToListAsync();
                //if(ListEmpInvit.Count == 0)
                //{
                //    var ListPendingInvit= await DB.TRPendingDeptInvits.Where(w=>w.DocumentNo == ID).ToListAsync();
                //    foreach (var item in ListPendingInvit) {
                //        item.Status= SYDocumentStatus.COMPLETED.ToString();
                //        DB.Entry(item).State = EntityState.Modified;
                //    }
                //    #region Notifican on Mobile
                //    //var HR =await DB.TRTrainingInvitations.Where(w => w.TrainNo == ID).FirstOrDefaultAsync();
                //    //var HRTo = DBStaff.HRStaffProfiles.Where(w => w.EmpCode == HR.CreatedBy).FirstOrDefault();
                //    //var FirebaseID = DBStaff.TokenResources.FirstOrDefault(w => w.IsLock == true && w.UserName == HRTo.EmpCode);
                //    //if (FirebaseID != null)
                //    //{
                //    //    string Desc = "Dear " + HRTo.Title + " " + HRTo.AllName + "," + HR.CourseName + " have been accepted from the all staff.";
                //    //    Notification.Notificationf Noti = new Notification.Notificationf();
                //    //    var LineToken = new List<string>();
                //    //    LineToken.Add(FirebaseID.FirebaseID);
                //    //    await Noti.SendNotification(LineToken, " Staff Training", Desc, fileName);
                //    //}
                //    #endregion
                //}
                #endregion

                await DB.SaveChangesAsync();
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
        public async Task<string> CancelInvit(long ID,string fileName)
        {
            DB = new HumicaDBContextTR();
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;

                HeaderTrainee = await DB.TRTrainingEmployees.FirstOrDefaultAsync(w => w.TrainNo == ID.ToString() && w.EmpCode == User.UserName);
                if (HeaderTrainee == null)
                {
                    return "DOC_INV";
                }
                HeaderTrainee.IsAccept = true;
                HeaderTrainee.ReStatus = SYDocumentStatus.CANCELLED.ToString();
                DB.Entry(HeaderTrainee).State = EntityState.Modified;
                int row = await DB.SaveChangesAsync();
                #region Notifican on Mobile
                //var HR = await DB.TRTrainingInvitations.Where(w => w.TrainNo == ID).FirstOrDefaultAsync();
                //var HRTo = DBStaff.HRStaffProfiles.Where(w => w.EmpCode == HR.CreatedBy).FirstOrDefault();
                //var FirebaseID = DBStaff.TokenResources.FirstOrDefault(w => w.IsLock == true && w.UserName == HRTo.EmpCode);
                //if (FirebaseID != null)
                //{
                //    string Desc = "Dear " + HRTo.Title + " " + HRTo.AllName + "," + HR.CourseName + " have been cancelled from the all staff.";
                //    Notification.Notificationf Noti = new Notification.Notificationf();
                //    var LineToken = new List<string>();
                //    LineToken.Add(FirebaseID.FirebaseID);
                //    await Noti.SendNotification(LineToken, " Staff Training", Desc, fileName);
                //}
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
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
    }
}