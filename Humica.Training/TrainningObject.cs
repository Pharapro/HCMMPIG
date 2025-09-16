using Humica.Core;
using Humica.Core.BS;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using Humica.Logic.LM;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Web.UI.WebControls;

namespace Humica.Training
{
    public class TrainningObject
    {
        HumicaDBContextTR DB = new HumicaDBContextTR();
        HumicaDBContext DBX = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public SYUser User { get; set; }
        public string MessageError { get; set; }
        public string ScreenId { get; set; }
        public SYUserBusiness BS { get; set; }
        public TrainingProgram THeader { get; set; }
        public ClsEmail EmailObject { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public HRStaffProfile HRStaffProfile { get; set; }
        public List<TrainingProgram> ListHeader { get; set; }
        public List<TRTrainingInvitation> ListInvitation { get; set; }
        public List<TRTrainingType> ListTrainingType { get; set; }
        public List<TRTrainingVenue> ListTRTrainingVenues { get; set; }
        //public List<TRTrainingCourseType> ListCourseTypeTypes { get; set; }
        public List<TRCourseCategory> ListTRCourseCategory { get; set; }
        //public List<TRCourseType> ListTRCourseType { get; set; }
        public TRTrainingCourse TRTrainingCourse { get; set; }
        public List<TRTrainingCourse> ListCourseType { get; set; }
        public List<TRTopic> ListTRTopic { get; set; }
        public List<TRTrainingCategory> ListTRTrainingCategory { get; set; }

        //public List<CFDPTruckAllow> ListTruckMaster { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }

        public TrainingRequestHeader RequestHeader { get; set; }
        public List<TrainingRequestHeader> ListRequestHeader { get; set; }
        public List<TrainingRequestHeader> WaitingListRequestHeader { get; set; }
        public TrainingRequestItem RequestItem { get; set; }
        public List<TrainingRequestItem> ListRequestItem { get; set; }
        public List<TrainingRequestItem> ListWaitingRequestItem { get; set; }
        public List<TRTrainingEmployee> ListWaitingRItem { get; set; }
        public TRTrainingEmployee TRTrainingEmployee { get; set; }
        public List<TRTrainingEmployee> ListTRTrainingEmployee { get; set; }

        public List<TRELearningQuestion> ListQuestion { get; set; }
        public List<TRELearningAnswer> ListAnswer { get; set; }
        public List<TrainigModule> ListModule { get; set; }
        public TrainingExamHeader ExamHeader { get; set; }
        public List<TrainingExamHeader> ListExamHeader { get; set; }
        public List<TrainingExamItem> ListExamItem { get; set; }
        public List<TrainingCourseItem> ListInvited { get; set; }
        public List<TrainingCourseMaster> ListCourseMaster { get; set; }
        public List<TrainigModuleTemp> TrainingBoard { get; set; }
        public List<TrainingAnouncementItem> TrainingAnounceDetail { get; set; }
        public TRTrainingAttendance TRTrainingAttendances { get; set; }
        public ESSTrainingSchedule ESSTrainingSchedules { get; set; }
        public List<ESSTrainingSchedule> ListSchedule { get; set; }

        public string InvCode { get; set; }
        //public string Program { get; set; }
        public string Course { get; set; }
        public string TrainingType { get; set; }
        public string ModuleDescription { get; set; }
        public string Question { get; set; }
        public string Pic { get; set; }
        public string Dealer { get; set; }
        public string ActionType { get; set; }
        public string AnswerSelected { get; set; }
        public IEnumerable<String> SelectedSport { get; set; }

        public List<int> ListDelete { get; set; }
        public string DocumentNo { get; set; }
        public string ShipmentType { get; set; }
        public string WCType = "SPC01";
        public string GIDocumentType = "NGI01";
        public string DeliveryType = "NDO01";
        public string TREXAM = "TREX01";
        public string TRRQ = "TRRQ01";
        public string CompanyCode { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ShipDate { get; set; }
        public BSDocConfg DocConfigure { get; set; }
        public FTDGeneralPeriod Filter { get; set; }
        public SYSplitItem SelectListItem { get; set; }
        //public BSQuotaConf Configure { get; set; }

        private const string REMAINING = "REMAINING";
        public string DefaultPlant { get; set; }
        public string DefaultStorage { get; set; }
        public int Group { get; set; }
        public int Timer { get; set; }
        public decimal DG1 { get; set; }
        public decimal DG { get; set; }

        //Result Initial
        public decimal DGInitial { get; set; }
        public decimal DGInitial1 { get; set; }
        public int TotalModuleInitial { get; set; }
        public int PassModuleInitial { get; set; }
        public int FailModuleInitial { get; set; }
        public decimal AcheiveInitial { get; set; }
        public string StatusInitial { get; set; }

        //Result Final
        public decimal DGFinal { get; set; }
        public decimal DGFinal1 { get; set; }
        public int TotalModuleFinal { get; set; }
        public int PassModuleFinal { get; set; }
        public int FailModuleFinal { get; set; }
        public decimal AcheiveFinal { get; set; }
        public string StatusFinal { get; set; }
        public bool IsFN { get; set; }

        public List<TRTrainingRequirement> ListTrainingRequirement { get; set; }
        public List<TRTrainingRequirement> ListTrainingGroup { get; set; }
        public List<TRTrainingRequirement> ListTrainingRoom { get; set; }
        public List<TRTrainingRequirement> ListTrainingDays { get; set; }
        public List<TRTrainingRequirement> ListTrainingCategory { get; set; }
        public List<TRTrainingRequirement> ListTrainingSubCategory { get; set; }
        public List<TRTrainingRequirement> ListExamType { get; set; }
        public List<TrainingAnouncement> ListAnouncement { get; set; }
        public List<TrainingAnouncementItem> ListAnouncementItem { get; set; }
        public List<HRLevel> ListLevel { get; set; }
        public List<TrainigModuleTemp> ListModuleTemp { get; set; }
        //public List<SPMDDeliveryLeadTime> ListDealerSelected { get; set; }
        //public List<SPMDDeliveryLeadTime> ListDealer { get; set; }
        public List<string> ListEmpCode { get; set; }
        public List<Core.DB.HR_STAFF_VIEW> ListStaff { get; set; }
        public List<Core.DB.HRStaffProfile> ListStaffs { get; set; }

        public TrainningObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string ReleaseAnoun(string Course, string Dealer)
        {
            try
            {
                DB = new HumicaDBContextTR();
                if (Course == null || Dealer == null)
                {
                    return "COURSE_DLR_EMPTY";
                }
                var trc = DB.TrainingAnouncement.ToList();
                //var trp = DB.TrainingReleasePrograms.ToList();
                string[] c = Course.Split(';');

                foreach (var r in c)
                {
                    int id = Convert.ToInt32(r);
                    string OPEN = SYDocumentStatus.OPEN.ToString();
                    var h = trc.Single(w => w.ID == id && w.Status == OPEN);
                    if (h != null)
                    {
                        h.Status = SYDocumentStatus.RELEASED.ToString();
                        DB.TrainingAnouncement.Attach(h);
                        DB.Entry(h).Property(w => w.Status).IsModified = true;

                        //Add Dealer
                        string[] d = Dealer.Split(';');
                        int l = 1;
                        foreach (var r2 in d)
                        {
                            var obj = new TrainingAnouncementItem();
                            obj.LineItem = l;
                            obj.AnounceCode = h.Code;
                            obj.Status = SYDocumentStatus.RELEASED.ToString();
                            DB.TrainingAnouncementItem.Add(obj);
                            l++;
                        }
                    }
                    else
                    {
                        return "DOC_INV";
                    }

                }
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = "";
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
                log.DocurmentAction = "";
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
                log.DocurmentAction = "";
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string SavePro()
        {
            try
            {
                if (ListHeader.Count == 0)
                {
                    return "RQU_EMPTY";
                }
                DB = new HumicaDBContextTR();

                var rh = DB.TrainingProgram.ToList();
                var rhc = DB.TrainingCourseMaster.ToList();

                foreach (var r in ListHeader)
                {
                    var c = rh.Where(w => w.ProgramCode == r.ProgramCode).ToList();
                    if (c.Count > 0)
                    {
                        DB.TrainingProgram.Remove(c.First());
                    }

                }
                foreach (var r in ListCourseMaster)
                {
                    var c = rhc.Where(w => w.Program == r.Program && w.Code == r.Code).ToList();
                    if (c.Count > 0)
                    {
                        DB.TrainingCourseMaster.Remove(c.First());
                    }

                }

                var lst = new List<TrainingProgram>();
                foreach (var r in ListHeader)
                {
                    if (
                         r.ProgramCode == "" || r.ProgramName == ""
                        )
                    {
                        return "REQUIRED_FIELD(*)";
                    }
                    if (lst.Where(w => w.ProgramCode == r.ProgramCode).ToList().Count == 0)
                    {
                        DB.TrainingProgram.Add(r);
                        lst.Add(r);
                    }

                }

                var lstcourse = new List<TrainingCourseMaster>();
                foreach (var r in ListCourseMaster)
                {
                    if (
                         r.Program == "" || r.Code == "" || r.Description == ""
                        )
                    {
                        return "REQUIRED_FIELD(*)";
                    }
                    if (lstcourse.Where(w => w.Program == r.Program && w.Code == r.Code).ToList().Count == 0)
                    {
                        DB.TrainingCourseMaster.Add(r);
                        lstcourse.Add(r);
                    }

                }
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = "";
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
                log.DocurmentAction = "";
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
                log.DocurmentAction = "";
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string SAVERequest(string InvCode)
        {
            try
            {
                if (ListRequestItem.Count == 0)
                {
                    return "RQU_EMPTY";
                }
                DB = new HumicaDBContextTR();

                var rh = DB.TrainingRequestHeader.ToList();

                //int p = rh.Count;
                DocConfigure = new BSDocConfg(TRRQ, BS.CompanyCode, true);
                //Header.InvCode = Configure.NextNumberRank;
                RequestHeader.RequestCode = DocConfigure.NextNumberRank;
                if (RequestHeader.RequestCode == null)
                {
                    return "NUMBERANGE_EMPTY";
                }
                RequestHeader.CreatedBy = User.UserName;
                RequestHeader.CreatedOn = DateTime.Now;
                RequestHeader.ChangedBy = User.UserName;
                RequestHeader.ChangedOn = DateTime.Now;
                RequestHeader.Status = SYDocumentStatus.OPEN.ToString();

                int l = 1;
                var st = DBX.HRStaffProfiles.ToList();
                foreach (var r in ListRequestItem)
                {
                    r.RequestCode = RequestHeader.RequestCode;

                    DB.TrainingRequestItem.Add(r);
                    l++;
                }
                DB.TrainingRequestHeader.Add(RequestHeader);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = RequestHeader.RequestCode;
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
                log.DocurmentAction = RequestHeader.RequestCode;
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
                log.DocurmentAction = RequestHeader.RequestCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string SAVEExam(string id, string InvCode)
        {
            try
            {
                string[] Answers = id.Split(';');

                if (id == null)
                {
                    return "CHECKED_EMPTY";
                }


                DB = new HumicaDBContextTR();

                //var chstaff = DB.CFStaffUsers.Where(w => w.Owner == BS.CompanyCode && w.UserID == User.UserID).ToList();
                //if (chstaff.Count == 0)
                //{
                //    return "STAFF_MAPPING";
                //}
                var rh = DB.TrainingExamHeader.ToList();
                var getAnswer = DB.TRELearningAnswer.ToList();
                var getModule = DB.TrainigModule.ToList();

                //string stid = chstaff.First().StaffID.ToString();
                var staff = DBX.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                if (staff.Count == 0)
                {
                    return "STAFF_MAPPING";
                }
                //int p = rh.Count;
                var objCF = DBX.ExCfWorkFlowItems.FirstOrDefault(w => w.ScreenID == ScreenId);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                var objNumber = new CFNumberRank(objCF.DocType, objCF.ScreenID);
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                //DocConfigure = new BSDocConfg(TREXAM, BS.CompanyCode, true);
                //Header.InvCode = Configure.NextNumberRank;
                //ExamHeader.ExamCode = DocConfigure.NextNumberRank;
                ExamHeader.ExamCode = objNumber.NextNumberRank;
                if (ExamHeader.ExamCode == null)
                {
                    return "NUMBERANGE_EMPTY";
                }
                ExamHeader.StaffID = staff.First().EmpCode.ToString();
                ExamHeader.CreatedBy = User.UserName;
                ExamHeader.CreatedOn = DateTime.Now;
                ExamHeader.Type = 67;
                ExamHeader.Result = SYDocumentStatus.FAILURED.ToString();
                ExamHeader.IsInitialTest = true;
                ExamHeader.InvCode = InvCode;
                var chectest = rh.Where(w => w.TrainingType == ExamHeader.TrainingType && w.Course == ExamHeader.Course && w.Topic == ExamHeader.Topic && w.InvCode == ExamHeader.InvCode && w.StaffID == ExamHeader.StaffID).ToList();
                if (chectest.Count > 0)
                {
                    ExamHeader.IsFinalTest = true;
                }
                var lst = new List<TrainingExamItem>();

                int l = 1;
                foreach (var r in Answers)
                {

                    if (r != "")
                    {
                        string[] anstr = r.Split(',');
                        string qu = anstr[0].ToString();
                        string ans = anstr[1].ToString();
                        string vl = anstr[2].ToString();

                        var obj = new TrainingExamItem();
                        obj.ExamCode = ExamHeader.ExamCode;
                        obj.ExamDate = ExamHeader.ExamDate;
                        obj.Coursecode = ExamHeader.Course;
                        obj.TrainingType = ExamHeader.TrainingType;
                        obj.StaffID = ExamHeader.StaffID;
                        obj.LineItem = l;
                        obj.Topic = ExamHeader.Topic;
                        obj.InvCode = ExamHeader.InvCode;
                        obj.QuestionCode = qu;
                        obj.AnswerCode = ans;
                        obj.CheckValue = Convert.ToInt32(vl);

                        var gscore = getAnswer.FirstOrDefault(w => w.TrainingType == obj.TrainingType && w.CourseCode == obj.Coursecode && w.Topic == obj.Topic && w.QuestionCode == obj.QuestionCode && w.AnswerCode == obj.AnswerCode);
                        if (gscore != null)
                        {
                            if (obj.CheckValue == gscore.CorrectValue && gscore.CorrectValue > 0)
                            {
                                obj.TotalScore = gscore.TotalScore;
                                ExamHeader.CorrectAnswer += 1;
                            }
                            else
                            {
                                obj.CheckValue = 1;
                                ExamHeader.InCorrectAnswer += 1;
                            }
                        }
                        ExamHeader.TotalScore += obj.TotalScore;

                        lst.Add(obj);
                        DB.TrainingExamItem.Add(obj);

                        l++;
                    }

                }

                var listModule = getAnswer.Where(w => w.TrainingType == ExamHeader.TrainingType && w.CourseCode == ExamHeader.Course && w.Topic == ExamHeader.Topic).ToList();
                foreach (var r in listModule.ToList())
                {
                    var coutnotcheck = lst.Where(w => w.TrainingType == r.TrainingType && w.Coursecode == r.CourseCode && w.Topic == r.Topic && w.QuestionCode == r.QuestionCode && w.AnswerCode == r.AnswerCode).ToList();
                    if (coutnotcheck.Count == 0)
                    {
                        var gscore = getAnswer.FirstOrDefault(w => w.TrainingType == r.TrainingType && w.TrainingType == r.CourseCode && w.Topic == r.Topic && w.QuestionCode == r.QuestionCode && w.AnswerCode == r.AnswerCode);
                        if (gscore != null)
                        {
                            if (gscore.ColumnCheck == 1 && gscore.CorrectValue == 0)
                            {
                                var obj = new TrainingExamItem();
                                obj.ExamCode = ExamHeader.ExamCode;
                                obj.ExamDate = ExamHeader.ExamDate;
                                obj.Coursecode = ExamHeader.Course;
                                obj.TrainingType = ExamHeader.TrainingType;
                                obj.StaffID = ExamHeader.StaffID;
                                obj.LineItem = l;
                                obj.Topic = ExamHeader.Topic;
                                obj.QuestionCode = r.QuestionCode;
                                obj.AnswerCode = r.AnswerCode;
                                obj.InvCode = ExamHeader.InvCode;

                                if (r.CheckValue == gscore.CorrectValue)
                                {
                                    obj.TotalScore = gscore.TotalScore;
                                    obj.CheckValue = Convert.ToInt32(r.CorrectValue);
                                    ExamHeader.TotalScore += obj.TotalScore;
                                    ExamHeader.CorrectAnswer += 1;
                                }
                                else
                                {
                                    obj.CheckValue = 1;
                                    ExamHeader.InCorrectAnswer += 1;
                                }

                                DB.TrainingExamItem.Add(obj);

                                l++;
                            }
                            else
                            {
                                var obj = new TrainingExamItem();
                                obj.ExamCode = ExamHeader.ExamCode;
                                obj.ExamDate = ExamHeader.ExamDate;
                                obj.Coursecode = ExamHeader.Course;
                                obj.TrainingType = ExamHeader.TrainingType;
                                obj.StaffID = ExamHeader.StaffID;
                                obj.LineItem = l;
                                obj.Topic = ExamHeader.Topic;
                                obj.QuestionCode = r.QuestionCode;
                                obj.AnswerCode = r.AnswerCode;
                                obj.InvCode = ExamHeader.InvCode;
                                obj.CheckValue = 0;
                                ExamHeader.InCorrectAnswer += 1;

                                DB.TrainingExamItem.Add(obj);

                                l++;
                            }

                        }
                    }
                }
                var chmodule = getModule.FirstOrDefault(w => w.TrainingType == ExamHeader.TrainingType && w.CourseCode == ExamHeader.Course && w.Topic == ExamHeader.Topic);
                if (chmodule != null)
                {
                    decimal? sc = chmodule.Score;
                    decimal? pc = chmodule.Target;
                    ExamHeader.TotalAchieve = Math.Round(ExamHeader.TotalScore / (decimal)sc, 2) * 100;
                    if (ExamHeader.TotalAchieve >= pc)
                    {
                        ExamHeader.Result = SYDocumentStatus.PASS.ToString();
                    }
                    ExamHeader.TotalModuleScore = (decimal)sc;
                    ExamHeader.TopicDescription = chmodule.Description;
                }

                DB.TrainingExamHeader.Add(ExamHeader);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ExamHeader.ExamCode;
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
                log.DocurmentAction = ExamHeader.ExamCode;
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
                log.DocurmentAction = ExamHeader.ExamCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string RequestTraining(string id)
        {
            try
            {
                DB = new HumicaDBContextTR();
                //var DBI = new DBBusinessProcess();
                if (id == null)
                {
                    return "CHECKNULL";
                }

                var OPEN = SYDocumentStatus.OPEN.ToString();
                var checkdup = DB.TrainingRequestHeader.FirstOrDefault(w => w.RequestCode == id && w.Status == OPEN);
                if (checkdup != null)
                {
                    var checkItem = DB.TrainingRequestItem.Where(w => w.RequestCode == id).ToList();
                    foreach (var r in checkItem)
                    {
                        r.Status = SYDocumentStatus.PENDING.ToString();
                        DB.TrainingRequestItem.Attach(r);
                        DB.Entry(r).Property(w => w.Status).IsModified = true;
                    }

                    checkdup.ChangedOn = DateTime.Now;
                    checkdup.ChangedBy = User.UserName;
                    checkdup.Status = SYDocumentStatus.PENDING.ToString();
                    checkdup.ChangedBy = User.UserName;
                    checkdup.ChangedOn = DateTime.Now;
                    DB.TrainingRequestHeader.Attach(checkdup);
                    DB.Entry(checkdup).Property(w => w.RequestDate).IsModified = true;
                    DB.Entry(checkdup).Property(w => w.Status).IsModified = true;
                    DB.Entry(checkdup).Property(w => w.ChangedOn).IsModified = true;
                    DB.Entry(checkdup).Property(w => w.ChangedBy).IsModified = true;
                    DB.Entry(checkdup).Property(w => w.CreatedOn).IsModified = true;
                    DB.Entry(checkdup).Property(w => w.CreatedBy).IsModified = true;
                }
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
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
                log.DocurmentAction = id;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        //public string IMPORTExam()
        //{
        //    try
        //    {
        //        ;

        //        if (ListExamHeader.Count == 0)
        //        {
        //            return "CHECKED_EMPTY";
        //        }


        //        DB = new DBBusinessProcess();

        //       // var chstaff = DB.CFStaffUsers.ToList();

        //        var rh = DB.TrainingExamHeaders.ToList();
        //        foreach (var r in ListExamHeader)
        //        {
        //            var check = rh.Where(w =>  w.Program == r.Program && w.Course == r.Course && w.Module == r.Module).ToList();
        //            if (check.Count > 0)
        //            {
        //                DB.TrainingExamHeaders.Remove(check.First());
        //            }

        //        }
        //        int p = rh.Count + 1;
        //        var stafflist = DB.HRStaffProfiles.ToList();
        //        foreach (var r in ListExamHeader)
        //        {
        //            //if (
        //            //    r.DealerCode == "" || r.Program == "" ||
        //            //    r.IsFinalTest == null || r.Type == null ||
        //            //    r.Course == "" || r.Module == "" ||
        //            //    r.StaffID == "" || r.ExamDate == null ||
        //            //    r.Result == "" || r.TotalScore == null ||
        //            //    r.TotalAchieve == null || r.CorrectAnswer == null
        //            //    || r.InCorrectAnswer == null || r.InvCode == ""
        //            //    )
        //            //{
        //            //    return "REQUIRED_FIELD(*)";
        //            //}
        //            ExamHeader = new TrainingExamHeader();
        //            DocConfigure = new BSDocConfg(TREXAM);
        //            //Header.InvCode = Configure.NextNumberRank;
        //            ExamHeader.ExamCode = DocConfigure.NextNumberRank;
        //            ExamHeader.Type = r.Type;
        //            ExamHeader.Program = r.Program;
        //            ExamHeader.Course = r.Course;
        //            ExamHeader.InvCode = r.InvCode;
        //            ExamHeader.Module = r.Module;
        //            var checkst = stafflist.Where(w => w.EmpCode == r.StaffID).ToList();
        //            if (checkst.Count > 0)
        //            {
        //                ExamHeader.StaffID = checkst.First().EmpCode.ToString();
        //            }

        //            ExamHeader.ExamDate = r.ExamDate;
        //            ExamHeader.TotalScore = r.TotalScore;
        //            ExamHeader.TotalAchieve = r.TotalAchieve;
        //            ExamHeader.CorrectAnswer = r.CorrectAnswer;
        //            ExamHeader.InCorrectAnswer = r.InCorrectAnswer;
        //            ExamHeader.Result = r.Result;
        //            ExamHeader.NCXComment1 = r.NCXComment1;
        //            ExamHeader.NCXComment2 = r.NCXComment2;
        //            ExamHeader.NCXComment3 = r.NCXComment3;
        //            ExamHeader.CreatedBy = User.UserName;
        //            ExamHeader.CreatedOn = DateTime.Now;
        //            ExamHeader.IsInitialTest = true;
        //            ExamHeader.IsFinalTest = r.IsFinalTest;

        //            DB.TrainingExamHeader.Add(ExamHeader);
        //            p++;
        //        }


        //        int row = DB.SaveChanges();
        //        return SYConstant.OK;
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = ExamHeader.ExamCode;
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = ExamHeader.ExamCode;
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = ExamHeader.ExamCode;
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //}

        public string UpdateRequest(string id)
        {
            try
            {
                DB = new HumicaDBContextTR();
                //var DBI = new DBBusinessProcess();
                if (id == null || ListRequestItem.Count == 0)
                {
                    return "CHECKNULL";
                }

                var OPEN = SYDocumentStatus.OPEN.ToString();
                var checkdup = DB.TrainingRequestHeader.FirstOrDefault(w => w.RequestCode == id && w.Status == OPEN);
                if (checkdup != null)
                {
                    var checkItem = DB.TrainingRequestItem.Where(w => w.RequestCode == id).ToList();
                    foreach (var r in checkItem)
                    {
                        DB.TrainingRequestItem.Remove(r);
                    }
                    int l = 1;
                    foreach (var r in ListRequestItem)
                    {
                        r.RequestCode = checkdup.RequestCode;
                        r.RequestDate = RequestHeader.RequestDate;
                        r.Status = RequestHeader.Status;
                        r.LineItem = l;
                        DB.TrainingRequestItem.Add(r);
                        l++;
                    }

                    checkdup.RequestDate = RequestHeader.RequestDate;
                    checkdup.ChangedOn = DateTime.Now;
                    checkdup.ChangedBy = User.UserName;
                    DB.TrainingRequestHeader.Attach(checkdup);
                    DB.Entry(checkdup).Property(w => w.RequestDate).IsModified = true;
                    DB.Entry(checkdup).Property(w => w.ChangedOn).IsModified = true;
                    DB.Entry(checkdup).Property(w => w.ChangedBy).IsModified = true;
                }
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = RequestHeader.RequestCode;
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
                log.DocurmentAction = RequestHeader.RequestCode;
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
                log.DocurmentAction = RequestHeader.RequestCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string SaveInfo()
        {
            try
            {
                DB = new HumicaDBContextTR();
                THeader.Status = SYDocumentStatus.OPEN.ToString();
                DB.TrainingProgram.Add(THeader);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = THeader.ProgramCode;
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
                log.DocurmentAction = THeader.ProgramCode;
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
                log.DocurmentAction = THeader.ProgramCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string UpdatePO(string id)
        {
            try
            {
                DB = new HumicaDBContextTR();
                //var DBI = new DBBusinessProcess();
                if (id == null)
                {
                    return "CHECKNULL";
                }

                var status = SYDocumentStatus.OPEN.ToString();
                var checkdup = DB.TrainingProgram.FirstOrDefault(w => w.ProgramCode == id);
                if (checkdup != null)
                {
                    checkdup.TrainingRequirement = THeader.TrainingRequirement;
                    checkdup.Target = THeader.Target;
                    checkdup.Capacity = THeader.Capacity;
                    checkdup.TrainingGroup = THeader.TrainingGroup;
                    checkdup.StartDate = THeader.StartDate;
                    checkdup.EndDate = THeader.EndDate;
                    DB.TrainingProgram.Attach(checkdup);
                    DB.Entry(checkdup).Property(w => w.TrainingRequirement).IsModified = true;
                    DB.Entry(checkdup).Property(w => w.Target).IsModified = true;
                    DB.Entry(checkdup).Property(w => w.Capacity).IsModified = true;
                    DB.Entry(checkdup).Property(w => w.TrainingGroup).IsModified = true;
                    DB.Entry(checkdup).Property(w => w.StartDate).IsModified = true;
                    DB.Entry(checkdup).Property(w => w.EndDate).IsModified = true;
                }
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = THeader.ProgramCode;
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
                log.DocurmentAction = THeader.ProgramCode;
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
                log.DocurmentAction = THeader.ProgramCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ReleaseInfo(string Code)
        {
            try
            {
                string[] Emp = Code.Split(';');
                foreach (var Procode in Emp)
                {
                    if (Procode.Trim() != "")
                    {
                        string status = SYDocumentStatus.OPEN.ToString();
                        var objMatch = DB.TrainingProgram.FirstOrDefault(w => w.ProgramCode == Procode && w.Status == status);
                        if (objMatch == null)
                        {
                            return "EE001";
                        }
                        objMatch.Status = SYDocumentStatus.RELEASED.ToString();
                        DB.TrainingProgram.Attach(objMatch);
                        DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                        int row = DB.SaveChanges();

                    }
                }
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Code;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string InactiveInfo(string Code)
        {
            try
            {
                string[] Emp = Code.Split(';');
                foreach (var Procode in Emp)
                {
                    if (Procode.Trim() != "")
                    {
                        string status = SYDocumentStatus.RELEASED.ToString();
                        var objMatch = DB.TrainingProgram.FirstOrDefault(w => w.ProgramCode == Procode && w.Status == status);
                        if (objMatch == null)
                        {
                            return "EE001";
                        }
                        objMatch.Status = SYDocumentStatus.CANCELLED.ToString();
                        DB.TrainingProgram.Attach(objMatch);
                        DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                        int row = DB.SaveChanges();
                    }
                }
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Code;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        //Training Course
        public string CreateTrainingCourse()
        {
            try
            {
                if (TRTrainingCourse.Code == null)
                {
                    return "CODE";
                }
                if (string.IsNullOrEmpty(TRTrainingCourse.Description))
                {
                    return "DESCRIPTION";
                }
                TRTrainingCourse.Code = TRTrainingCourse.Code.Trim().ToUpper();
                int count = DB.TRTrainingCourses.Where(x => x.Code == TRTrainingCourse.Code).Count();

                if (count > 0)
                {
                    return "CODE_EXISTS";
                }
                TRTrainingCourse.CreatedBy = User.UserName;
                TRTrainingCourse.CreatedOn = DateTime.Now;

                DB.TRTrainingCourses.Add(TRTrainingCourse);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditTrainingCourse(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(TRTrainingCourse.Description))
                {
                    return "DESCRIPTION";
                }
                var trainingCourse = DB.TRTrainingCourses.Find(id);
                trainingCourse.Description = TRTrainingCourse.Description;
                trainingCourse.SecondDescription = TRTrainingCourse.SecondDescription;
                trainingCourse.Objective = TRTrainingCourse.Objective;
                trainingCourse.ChangedBy = User.UserName;
                trainingCourse.ChangedOn = DateTime.Now;

                DB.TRTrainingCourses.Attach(trainingCourse);
                DB.Entry(trainingCourse).Property(w => w.Description).IsModified = true;
                DB.Entry(trainingCourse).Property(w => w.SecondDescription).IsModified = true;
                DB.Entry(trainingCourse).Property(w => w.Objective).IsModified = true;
                DB.Entry(trainingCourse).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(trainingCourse).Property(w => w.ChangedOn).IsModified = true;

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteTrainingCourse(int id)
        {
            try
            {

                var objCust = DB.TRTrainingCourses.Find(id);
                if (objCust == null)
                {
                    return "TRAINING_EN";
                }

                DB.TRTrainingCourses.Attach(objCust);
                DB.Entry(objCust).State = System.Data.Entity.EntityState.Deleted;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = TRTrainingCourse.Code;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

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
                log.ScreenId = TRTrainingCourse.Code; ;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CreateTraineeItems()
        {
            try
            {
                int i = 0;
                List<HRStaffProfile> ListStaff = DBX.HRStaffProfiles.Where(w => w.Status == "A").ToList();
                List<HRDepartment> ListDepartment = DBX.HRDepartments.ToList();
                List<HRPosition> ListPosition = DBX.HRPositions.ToList();
                //List<TRTrainingRequirement> ListGroup = DB.TRTrainingRequirements.Where(w => w.Category == "G").ToList();
                var RequesterName = DBX.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == User.UserName);
                var Approved = DBX.ExCfWFApprovers.FirstOrDefault(w => w.WFObject == "TR");
                using (var context = new HumicaDBContextTR())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var objCF = DBX.ExCfWorkFlowItems.Where(w => w.ScreenID == ScreenId).ToList();
                            if (objCF.Count() == 0)
                            {
                                return "REQUEST_TYPE_NE";
                            }
                            var objNumber = new CFNumberRank(objCF.First().DocType, ScreenId);
                            if (objNumber.NextNumberRank == null)
                            {
                                return "NUMBER_RANK_NE";
                            }
                            if (RequestHeader.Branch == null)
                                return "Please select Branch";
                            if (RequestHeader.FromDate == null)
                                return "Please select From Date";
                            if (RequestHeader.ToDate == null)
                                return "Please select To Date";
                            if (RequestHeader.Locations == null)
                                return "Please input Training Center";
                            if (RequestHeader.Purpose == null)
                                return "Please input Detail/Purpose";
                            if (RequestHeader.Scopes == null)
                                return "Please input Scope";
                            RequestHeader.TrainNo = objNumber.NextNumberRank;
                            RequestHeader.RequestCode = RequesterName.EmpCode;
                            RequestHeader.DocType = objCF.First().DocType;
                            RequestHeader.RequesterName = RequesterName.AllName;
                            RequestHeader.TrainingCourse = RequestHeader.TrainingCourse;
                            RequestHeader.TrainingType = RequestHeader.TrainingType;
                            RequestHeader.Department = RequesterName.DEPT;
                            RequestHeader.Position = RequesterName.JobCode;
                            RequestHeader.TrainingCategory = RequestHeader.TrainingCategory;
                            RequestHeader.TrainerName = RequestHeader.TrainerName;
                            RequestHeader.NumOfEmpJoin = RequestHeader.NumOfEmpJoin;
                            RequestHeader.FromDate = RequestHeader.FromDate;
                            RequestHeader.ToDate = RequestHeader.ToDate;
                            RequestHeader.Locations = RequestHeader.Locations;
                            RequestHeader.Scopes = RequestHeader.Scopes;
                            RequestHeader.Remark = RequestHeader.Remark;
                            RequestHeader.AttachFile = RequestHeader.AttachFile;
                            RequestHeader.ApproverCode = Approved.Employee;
                            RequestHeader.ApproverName = Approved.EmployeeName;
                            RequestHeader.Status = SYDocumentStatus.OPEN.ToString();
                            RequestHeader.RequestDate = DateTime.Now;
                            RequestHeader.TrainingFee = RequestHeader.TrainingFee;
                            RequestHeader.CreatedBy = User.UserName;
                            RequestHeader.CreatedOn = DateTime.Today;

                            context.TrainingRequestHeader.Add(RequestHeader);
                            decimal HAmount = 0;
                            foreach (var items in ListSchedule)
                            {
                                items.TrainNo = RequestHeader.TrainNo;
                                context.ESSTrainingSchedule.Add(items);
                                HAmount += items.Hour;
                            }
                            context.SaveChanges();
                            i = (int)RequestHeader.ID;
                            int LineItem = 0;
                            foreach (var item in ListRequestItem)
                            {
                                LineItem++;
                                var RequestItem = new TrainingRequestItem();
                                RequestItem.TrainID = i;
                                RequestItem.ID = i;
                                RequestItem.TrainNo = RequestHeader.TrainNo;
                                RequestItem.LineItem = LineItem;
                                RequestItem.RequestCode = RequestHeader.RequestCode;
                                RequestItem.RequesterName = RequesterName.AllName;
                                RequestItem.TrainingCourse = RequestHeader.TrainingCourse;
                                RequestItem.TrainingType = RequestHeader.TrainingType;
                                RequestItem.TrainingCategory = RequestHeader.TrainingCategory;
                                RequestItem.TrainerName = RequestHeader.TrainerName;
                                RequestItem.NumOfEmpJoin = RequestHeader.NumOfEmpJoin;
                                RequestItem.FromDate = RequestHeader.FromDate;
                                RequestItem.ToDate = RequestHeader.ToDate;
                                RequestItem.HAmount = HAmount;
                                RequestItem.Locations = RequestHeader.Locations;
                                RequestItem.EmpCode = item.EmpCode;
                                RequestItem.EmpName = item.EmpName;
                                RequestItem.Department = RequestHeader.Department;
                                RequestItem.Position = item.Position;
                                RequestItem.RequestDate = RequestHeader.RequestDate;
                                if (item.EmpCode == RequestHeader.RequestCode)
                                    RequestItem.Status = "ACCEPTED";
                                else RequestItem.Status = SYDocumentStatus.PENDING.ToString();
                                var Staff = ListStaff.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                                if (Staff != null)
                                {
                                    RequestItem.EmpName = Staff.AllName;
                                    if (!string.IsNullOrEmpty(Staff.JobCode))
                                    {
                                        var Post = ListPosition.FirstOrDefault(w => w.Code == Staff.JobCode);
                                        if (Post != null) RequestItem.Position = Post.Description;
                                    }
                                }
                                context.TrainingRequestItem.Add(RequestItem);
                            }
                            string ApproveFlow = objCF.First().ApprovalFlow;
                            var listDefaultApproval = DBX.ExCfWFApprovers.Where(w => w.Branch == RequestHeader.Branch && w.WFObject == ApproveFlow).ToList();
                            
                            foreach (var read in ListApproval)
                            {
                                var objApp = new ExDocApproval();
                                objApp.DocumentNo = RequestHeader.TrainNo;
                                objApp.DocumentType = RequestHeader.DocType;
                                objApp.Status = SYDocumentStatus.OPEN.ToString();
                                objApp.Approver = read.Approver;
                                objApp.ApproverName = read.ApproverName;
                                objApp.ApproveLevel = read.ApproveLevel;
                                objApp.WFObject = ApproveFlow;
                                objApp.ApprovedBy = "";
                                objApp.ApprovedName = "";
                                objApp.LastChangedDate = DateTime.Now;
                                DBX.ExDocApprovals.Add(objApp);
                            }
                            DBX.SaveChanges();
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
                //log.DocurmentAction = RequestItem.ID.ToString();
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
                log.DocurmentAction = RequestItem.ID.ToString();
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
        public string EditTraineeItems(int id)
        {
            List<Core.DB.HRStaffProfile> ListStaff = DBX.HRStaffProfiles.Where(w => w.Status == "A").ToList();
            List<Core.DB.HRDepartment> ListDepartment = DBX.HRDepartments.ToList();
            List<Core.DB.HRPosition> ListPosition = DBX.HRPositions.ToList();
            List<TRTrainingRequirement> ListGroup = DB.TRTrainingRequirements.Where(w => w.Category == "G").ToList();
            
            using (var context = new HumicaDBContextTR())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var Requester = ListStaff.FirstOrDefault(w => w.EmpCode == User.UserName);
                        var ObjMatch = DB.TrainingRequestHeader.FirstOrDefault(w => w.ID == id);
                        if (ObjMatch == null)
                            return "INV_DOC";
                        var _ListItem = DB.TrainingRequestItem.Where(w => w.ID == ObjMatch.ID).ToList();
                        foreach (var item in _ListItem)
                        {
                            context.TrainingRequestItem.Attach(item);
                            context.Entry(item).State = EntityState.Deleted;
                        }
                        ObjMatch.ChangedBy = User.UserName;
                        ObjMatch.ChangedOn = DateTime.Now;
                        ObjMatch.TrainingCourse = RequestHeader.TrainingCourse;
                        ObjMatch.TrainingType = RequestHeader.TrainingType;
                        ObjMatch.TrainingCategory = RequestHeader.TrainingCategory;
                        ObjMatch.TrainerName = RequestHeader.TrainerName;
                        ObjMatch.NumOfEmpJoin = RequestHeader.NumOfEmpJoin;
                        ObjMatch.FromDate = RequestHeader.FromDate;
                        ObjMatch.ToDate = RequestHeader.ToDate;
                        ObjMatch.Locations = RequestHeader.Locations;
                        ObjMatch.Purpose = RequestHeader.Purpose;
                        ObjMatch.Scopes = RequestHeader.Scopes;
                        ObjMatch.Remark = RequestHeader.Remark;
                        ObjMatch.AttachFile = RequestHeader.AttachFile;
                        ObjMatch.Status = SYDocumentStatus.OPEN.ToString();
                        ObjMatch.RequestDate = DateTime.Now;
                        ObjMatch.TrainingFee = RequestHeader.TrainingFee;

                        context.TrainingRequestHeader.Attach(ObjMatch);

                        context.Entry(ObjMatch).State = EntityState.Modified;

                        context.SaveChanges();

                        int LineItem = 0;
                        foreach (var item in ListRequestItem)
                        {
                            LineItem++;
                            RequestItem = new TrainingRequestItem
                            {
                                ID = id,
                                LineItem = LineItem,
                                RequestCode = User.UserName,
                                RequesterName = Requester.AllName,
                                TrainingCourse = RequestHeader.TrainingCourse,
                                TrainingType = RequestHeader.TrainingType,
                                TrainingCategory = RequestHeader.TrainingCategory,
                                TrainerName = RequestHeader.TrainerName,
                                NumOfEmpJoin = RequestHeader.NumOfEmpJoin,
                                FromDate = RequestHeader.FromDate,
                                ToDate = RequestHeader.ToDate,
                                Locations = RequestHeader.Locations,
                                EmpCode = item.EmpCode,
                                EmpName = item.EmpName,
                                Position = item.Position,
                                RequestDate = DateTime.Now,
                                Status = SYDocumentStatus.OPEN.ToString()
                            };
            
                            var Staff = ListStaff.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                            if (Staff != null)
                            {
                                RequestItem.EmpName = Staff.AllName;
                                if (!string.IsNullOrEmpty(Staff.JobCode))
                                {
                                    var Post = ListPosition.FirstOrDefault(w => w.Code == Staff.JobCode);
                                    if (Post != null) RequestItem.Position = Post.Description;
                                }
                            }
                            context.TrainingRequestItem.Add(RequestItem);
                        }
                        foreach (var items in ListSchedule)
                        {
                            context.ESSTrainingSchedule.Attach(items);
                            context.Entry(items).State = EntityState.Modified;
                        }
                        DBX.SaveChanges();
                        context.SaveChanges();
                        dbContextTransaction.Commit();
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
                    finally
                    {
                        DB.Configuration.AutoDetectChangesEnabled = true;
                    }
                }
            }
        }
        public void GetAllStaff()
        {
            
            var tblStaffProfile = DBX.HRStaffProfiles.Where(w => w.Status == "A" ).ToList();
            //if()
            //this.ListStaffs = tblStaffProfile.Where(w => !this.ListEmpCode.Contains(w.EmpCode)).ToList();
            this.ListStaffs =tblStaffProfile.ToList();
        }
        public void GetAllStaffs()
        {
            var staff = DBX.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == User.UserName);

            var tblStaffProfile = DBX.HRStaffProfiles.Where(w => w.Status == "A" && w.DEPT == staff.DEPT).ToList();

            //if()
            //this.ListStaffs = tblStaffProfile.Where(w => !this.ListEmpCode.Contains(w.EmpCode)).ToList();
            this.ListStaffs = tblStaffProfile.ToList();
        }
        public string Request(string TrainNo,string URL)
        {
            try
            {
                HumicaDBContextTR DBI = new HumicaDBContextTR();

                TrainingRequestHeader ObjMatch = DBI.TrainingRequestHeader.FirstOrDefault(w => w.TrainNo == TrainNo);

                if (ObjMatch == null) return "DOC_INV";

                ObjMatch.Status = SYDocumentStatus.PENDING.ToString();
                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;

                DBI.TrainingRequestHeader.Attach(ObjMatch);

                DBI.Entry(ObjMatch).Property(w => w.Status).IsModified = true;
                DBI.Entry(ObjMatch).Property(w => w.ChangedBy).IsModified = true;
                DBI.Entry(ObjMatch).Property(w => w.ChangedOn).IsModified = true;

                DBI.SaveChanges();
                var ListTrainee = DB.TrainingRequestItem.Where(w => w.TrainNo == ObjMatch.TrainNo && w.EmpCode != ObjMatch.RequestCode).ToList();
                foreach (var item in ListTrainee)
                {
                    var Trainee = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                    var DocNo = ObjMatch.ID;
                    if (Trainee != null) 
                    {
                        if (ObjMatch.Status == SYDocumentStatus.PENDING.ToString())
                        {
                            SYWorkFlowEmailObject wfo = new SYWorkFlowEmailObject("REQTRAINEE", WorkFlowType.REQUESTER, UserType.N, SYDocumentStatus.PENDING.ToString());
                            wfo.SelectListItem = new SYSplitItem(Convert.ToString(DocNo));
                            wfo.User = User;
                            wfo.BS = BS;
                            wfo.ScreenId = ScreenId;
                            wfo.UrlView = URL;
                            wfo.Module = "HR"; // CModule.PURCHASE.ToString();
                            wfo.DocNo = ObjMatch.ID.ToString();
                            wfo.ListLineRef = new List<BSWorkAssign>();
                            wfo.Action = SYDocumentStatus.PENDING.ToString();
                            wfo.ListObjectDictionary = new List<object>();
                            wfo.ListObjectDictionary.Add(ObjMatch);
                            wfo.ListObjectDictionary.Add(Trainee);

                            if (!string.IsNullOrEmpty(Trainee.Email))
                            {
                                wfo.ListObjectDictionary.Add(Trainee);
                                WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(Trainee);
                                MessageError = wfo.getErrorMessage(result1);
                            }
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
                log.DocurmentAction = TrainNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = TrainNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = TrainNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        private static List<T> GetNextItemFromList<T>(List<T> list, T currentObj)
        {
            return list.SkipWhile(obj => obj.Equals(currentObj)).Take(1).ToList();
        }
        public string ESSApproved(string TrainNo,string fileName, string Comment)
        {
            try
            {
                HumicaDBContextTR DBI = new HumicaDBContextTR();

                TrainingRequestHeader objmatch = DB.TrainingRequestHeader.FirstOrDefault(w => w.TrainNo == TrainNo);

                if (objmatch == null) return "DOC_INV";
                //if (objmatch.Status != SYDocumentStatus.PENDING.ToString()) return "INV_DOC";

                string Open = SYDocumentStatus.OPEN.ToString();
                var listApproval = DBX.ExDocApprovals.Where(w => w.DocumentType == objmatch.DocType
                                    && w.DocumentNo == objmatch.TrainNo && w.Status == Open).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                var b = false;
                int approverLevel = 0;
                foreach (var read in listApproval)
                {
                    approverLevel = read.ApproveLevel;
                    var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
                    if (checklist.Count > 0)
                    {
                        if (read.Status == SYDocumentStatus.APPROVED.ToString())
                        {
                            return "USER_ALREADY_APP";
                        }
                        var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                        if (objStaff != null)
                        {
                            if (listApproval.Where(w => w.ApproveLevel <= read.ApproveLevel).Count() >= listApproval.Count())
                            {
                                listApproval.ForEach(w => w.Status = SYDocumentStatus.APPROVED.ToString());
                            }
                            HRStaffProfile = objStaff;
                            read.ApprovedBy = objStaff.EmpCode;
                            read.ApprovedName = objStaff.AllName;
                            read.LastChangedDate = DateTime.Now.Date;
                            read.ApprovedDate = DateTime.Now;
                            read.Status = SYDocumentStatus.APPROVED.ToString();
                            read.Comment = Comment;
                            DBX.ExDocApprovals.Attach(read);
                            DBX.Entry(read).State = System.Data.Entity.EntityState.Modified;
                            DBX.SaveChanges();
                            b = true;
                            break;
                        }
                    }
                }
                if (listApproval.Count > 0)
                {
                    if (b == false)
                    {
                        return "USER_NOT_APPROVOR";
                    }
                }
                var Status = SYDocumentStatus.APPROVED.ToString();
                var Request = objmatch.RequestCode;
                // Approver
                if ((listApproval.Where(w => w.ApproveLevel > approverLevel && w.Status == SYDocumentStatus.OPEN.ToString()).Count() > 0))
                {
                    Status = SYDocumentStatus.PENDING.ToString();
                    Request = listApproval.Where(w => w.ApproveLevel == approverLevel).Select(w => w.Approver)?.FirstOrDefault();
                }
                objmatch.Status = Status;
                objmatch.ApprovedDate = DateTime.Now;
                DB.TrainingRequestHeader.Attach(objmatch);
                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                DB.Entry(objmatch).Property(w => w.ApprovedDate).IsModified = true;
                DB.SaveChanges();
                string APP = SYDocumentStatus.APPROVED.ToString();
                var ListHeader = DB.TrainingRequestHeader.AsEnumerable().Where(w => w.TrainNo == objmatch.TrainNo).ToList();
                var Trainee = DB.TrainingRequestItem.AsEnumerable().Where(w => w.TrainNo == objmatch.TrainNo && w.Status == "ACCEPTED" && ListHeader.Where(x => x.Status == APP).Any()).ToList();
                foreach (var item in Trainee)
                {
                    var LStaffAtt = DB.ESSTrainingSchedule.Where(x => x.TrainNo == item.TrainNo).ToList();
                    foreach (var items in LStaffAtt)
                    {
                        TRTrainingEmployee = new TRTrainingEmployee
                        {
                            TrainNo = item.TrainNo,
                            CalendarID = item.ID,
                            LineItem = item.LineItem,
                            EmpCode = item.EmpCode,
                            EmpName = item.EmpName,
                            Department = item.Department,
                            Position = item.Position,
                            InviteTrainingDate = items.TrainDate,
                            HAmount = item.HAmount,
                            RequestDate = item.RequestDate,
                            TrainingType = item.TrainingType,
                            CourseName = item.TrainingCourse,
                            CourseCategory = item.TrainingCategory,
                            TrainerName = item.TrainerName,
                            Status = item.Status,
                            IsInvite = true,
                            IsAccept = true,
                            InYear = item.RequestDate.Value.Year,
                            CreatedBy = item.EmpCode
                        };
                        DBI.TRTrainingEmployees.Add(TRTrainingEmployee);
                        DBI.SaveChanges();
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
                log.DocurmentAction = TrainNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = TrainNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = TrainNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ESSRejected(string TrainNo, string Upload, string Comment)
        {
            try
            {
                HumicaDBContextTR DBI = new HumicaDBContextTR();

                TrainingRequestHeader objmatch = DB.TrainingRequestHeader.FirstOrDefault(w => w.TrainNo == TrainNo);

                if (objmatch == null) return "DOC_INV";
                //if (objmatch.Status != SYDocumentStatus.PENDING.ToString()) return "INV_DOC";

                string Open = SYDocumentStatus.OPEN.ToString();
                var listApproval = DBX.ExDocApprovals.Where(w => w.DocumentType == objmatch.DocType
                                    && w.DocumentNo == objmatch.TrainNo && w.Status == Open).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                var b = false;
                int approverLevel = 0;
                foreach (var read in listApproval)
                {
                    approverLevel = read.ApproveLevel;
                    var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
                    if (checklist.Count > 0)
                    {
                        if (read.Status == SYDocumentStatus.APPROVED.ToString())
                        {
                            return "USER_ALREADY_APP";
                        }
                        var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                        if (objStaff != null)
                        {
                            if (listApproval.Where(w => w.ApproveLevel <= read.ApproveLevel).Count() >= listApproval.Count())
                            {
                                listApproval.ForEach(w => w.Status = SYDocumentStatus.APPROVED.ToString());
                            }
                            HRStaffProfile = objStaff;
                            read.ApprovedBy = objStaff.EmpCode;
                            read.ApprovedName = objStaff.AllName;
                            read.LastChangedDate = DateTime.Now.Date;
                            read.ApprovedDate = DateTime.Now;
                            read.Status = SYDocumentStatus.REJECTED.ToString();
                            read.Comment = Comment;
                            DBX.ExDocApprovals.Attach(read);
                            DBX.Entry(read).State = System.Data.Entity.EntityState.Modified;
                            DBX.SaveChanges();
                            b = true;
                            break;
                        }
                    }
                }
                if (listApproval.Count > 0)
                {
                    if (b == false)
                    {
                        return "USER_NOT_APPROVOR";
                    }
                }

                var Status = SYDocumentStatus.REJECTED.ToString();
                var Appro_ = objmatch.RequestCode;

                objmatch.Status = Status;
                objmatch.ApprovedDate = DateTime.Now;

                DB.TrainingRequestHeader.Attach(objmatch);

                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                DB.Entry(objmatch).Property(w => w.ApprovedDate).IsModified = true;

                #region Email
                //if ((listApproval.Where(w => w.ApproveLevel > approverLevel && w.Status == SYDocumentStatus.OPEN.ToString()).Count() > 0))
                //{
                //    ExDocApproval currenctApproval_ = listApproval.Where(w => w.Approver == Appro_).FirstOrDefault();
                //    ExDocApproval exDocApproval_ = GetNextItemFromList(listApproval, currenctApproval_).ToList().FirstOrDefault();
                //    var AlertTO_ = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == exDocApproval_.Approver);
                //    var EmailConf_ = SMS.CFEmailAccounts.FirstOrDefault();

                //    if (EmailConf_ != null && AlertTO_ != null)
                //    {
                //        CFEmailAccount emailAccount = EmailConf_;
                //        string subject = string.Format("Training Request Form {0:dd-MMM-yyyy}", DateTime.Today);
                //        //string body = str;
                //        string filePath = Upload;
                //        string fileName = Path.GetFileName(filePath);
                //        EmailObject = new ClsEmail();
                //        int rs = EmailObject.SendMail(emailAccount, "", AlertTO_.Email,
                //            subject, "", filePath, fileName);
                //        //}
                //    }
                //}
                //else
                //{
                //    var AlertTO = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objmatch.RequestCode);
                //    var EmailConf = SMS.CFEmailAccounts.FirstOrDefault();
                //    if (EmailConf != null && AlertTO != null)
                //    {
                //        CFEmailAccount emailAccount = EmailConf;
                //        string subject = string.Format("Training Request Form {0:dd-MMM-yyyy}", DateTime.Today);
                //        //string body = str;
                //        string filePath = Upload;
                //        string fileName = Path.GetFileName(filePath);
                //        EmailObject = new ClsEmail();
                //        int rs = EmailObject.SendMail(emailAccount, "", AlertTO.Email,
                //            subject, "", filePath, fileName);
                //        //}
                //    }
                //}
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
                log.DocurmentAction = TrainNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = TrainNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = TrainNo;
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string Approved(int Id, string fileName)
        {
            try
            {
                HumicaDBContextTR DBI = new HumicaDBContextTR();

                TrainingRequestHeader objmatch = DBI.TrainingRequestHeader.FirstOrDefault(w => w.ID == Id);

                if (objmatch == null) return "DOC_INV";
                var ListInvites = DB.TrainingRequestItem.FirstOrDefault(w => w.ID == Id);
                var Trainees = DBX.HRStaffProfiles.Where(w => w.EmpCode == ListInvites.EmpCode).FirstOrDefault();
                string Pending = SYDocumentStatus.PENDING.ToString();
                var listUser = DBX.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                int approverLevel = 0;
                var Approved = DBX.ExCfWFApprovers.FirstOrDefault(w => w.WFObject == "TR");
                var checklist = listUser.FirstOrDefault(w => w.EmpCode == Approved.Employee);
                if (objmatch.Status == SYDocumentStatus.APPROVED.ToString())
                {
                    return "USER_ALREADY_APP";
                }
                if (checklist != null)
                {
                    objmatch.ApprovedDate = DateTime.Now;
                    objmatch.Status = SYDocumentStatus.ACCEPTED.ToString();
                    DBI.TrainingRequestHeader.Attach(objmatch);
                    DBI.Entry(objmatch).State = System.Data.Entity.EntityState.Modified;
                }
                DBI.SaveChanges();
                SYHRAnnouncement _announ = new SYHRAnnouncement();
                if (checklist != null)
                {
                    _announ.Type = "TrainingRequest";
                    _announ.Subject = Trainees.AllName;
                    _announ.Description = "Dear " + Trainees.Title + " " + Trainees.AllName + ", We would like to invite you to Training." + " " + string.Format("From Date : {0:dd-MMM-yyyy}", objmatch.FromDate) + ", " + string.Format("To Date : {0:dd-MMM-yyyy}", objmatch.ToDate) + ", " + "Location : " + objmatch.Locations;
                    _announ.DocumentNo = Id.ToString();
                    _announ.DocumentDate = DateTime.Now;
                    _announ.IsRead = false;
                    _announ.UserName = Approved.Employee;
                    _announ.CreatedBy = User.UserName;
                    _announ.CreatedOn = DateTime.Now;
                    DBX.SYHRAnnouncements.Add(_announ);
                }
                DBX.SaveChanges();
                #region Notifican on Mobile
                //var ListInvite = DB.TrainingRequestItem.Where(w => w.ID == Id).ToList();
                //foreach (var item in ListInvite)
                //{
                //    var Trainee = DBX.HRStaffProfiles.Where(w => w.EmpCode == item.EmpCode).FirstOrDefault();
                //    var FirebaseID = DBX.TokenResources.FirstOrDefault(w => w.IsLock == true && w.UserName == Trainee.EmpCode);
                //    if (FirebaseID != null)
                //    {
                //        string Desc = "Dear " + Trainee.Title + " " + Trainee.AllName + ", We would like to invite you to Training." + " " + string.Format("From Date : {0:dd-MMM-yyyy}", objmatch.FromDate) + ", " + string.Format("To Date : {0:dd-MMM-yyyy}", objmatch.ToDate) + ", " + "Location : " + objmatch.Locations;
                //        Notification.Notificationf Noti = new Notification.Notificationf();
                //        var LineToken = new List<string>();
                //        LineToken.Add(FirebaseID.FirebaseID);
                //        Noti.SendNotification(LineToken, " Staff Training", Desc, fileName);
                //    }
                //}
                #endregion
                #region Email
                //var AlertTO = DBX.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objmatch.RequestCode);
                //var EmailConf = SMS.CFEmailAccounts.FirstOrDefault();
                //if (EmailConf != null && AlertTO != null)
                //{
                //    CFEmailAccount emailAccount = EmailConf;
                //    string subject = string.Format("Training Request Form {0:dd-MMM-yyyy}", DateTime.Today);
                //    //string body = str;
                //    string filePath = Upload;
                //    string fileName = Path.GetFileName(filePath);
                //    EmailObject = new ClsEmail();
                //    int rs = EmailObject.SendMail(emailAccount, "", AlertTO.Email,
                //        subject, "", filePath, fileName);
                //    //}
                //}
                #endregion
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Id.ToString();
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = Id.ToString();
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = Id.ToString();
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string Rejected(int Id,string URL, string fileName)
        {
            try
            {
                HumicaDBContextTR DBI = new HumicaDBContextTR();

                TrainingRequestHeader objmatch = DBI.TrainingRequestHeader.FirstOrDefault(w => w.ID == Id);

                if (objmatch == null) return "DOC_INV";
                var listUser = DBX.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                int approverLevel = 0;
                if (objmatch.Status == SYDocumentStatus.APPROVED.ToString())
                {
                    return "USER_ALREADY_APP";
                }
                if (objmatch != null)
                {
                    objmatch.ChangedBy = User.UserName;
                    objmatch.ChangedOn = DateTime.Now;
                    objmatch.Status = SYDocumentStatus.REJECTED.ToString();
                    DBI.TrainingRequestHeader.Attach(objmatch);
                    DBI.Entry(objmatch).State = System.Data.Entity.EntityState.Modified;
                }
                DBI.SaveChanges();
                #region Send Email
                var AlertTO = DBX.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objmatch.RequestCode);
                var EmailConf = SMS.CFEmailAccounts.FirstOrDefault();
                string DocNo = objmatch.ID.ToString();
                if (AlertTO != null)
                {
                    if (objmatch.Status == SYDocumentStatus.REJECTED.ToString())
                    {
                        SYWorkFlowEmailObject wfo = new SYWorkFlowEmailObject("TRRE", WorkFlowType.REQUESTER, UserType.N, SYDocumentStatus.REJECTED.ToString());
                        wfo.SelectListItem = new SYSplitItem(Convert.ToString(DocNo));
                        wfo.User = User;
                        wfo.BS = BS;
                        wfo.ScreenId = ScreenId;
                        wfo.UrlView = URL;
                        wfo.Module = "HR"; // CModule.PURCHASE.ToString();
                        wfo.DocNo = objmatch.ID.ToString();
                        wfo.ListLineRef = new List<BSWorkAssign>();
                        wfo.Action = SYDocumentStatus.REJECTED.ToString();
                        wfo.ListObjectDictionary = new List<object>();
                        wfo.ListObjectDictionary.Add(objmatch);

                        if (!string.IsNullOrEmpty(AlertTO.Email))
                        {
                            wfo.ListObjectDictionary.Add(AlertTO);
                            WorkFlowResult result1 = wfo.InsertProcessWorkFlowLeave(AlertTO);
                            MessageError = wfo.getErrorMessage(result1);
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
                log.DocurmentAction = Id.ToString();
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = Id.ToString();
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = Id.ToString();
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ESSAccept( int Id)
        {
            try
            {
                HumicaDBContextTR DBI = new HumicaDBContextTR();

                string Pending = SYDocumentStatus.PENDING.ToString();
                var objmatch = DBI.TrainingRequestItem.Where(w => w.ID == Id && w.Status == Pending).ToList();
                if (objmatch == null) return "DOC_INV";
                foreach (var item in objmatch)
                {
                    var user = DBX.HRStaffProfiles.Where(w => w.EmpCode == User.UserName);
                    int approverLevel = 0;
                    var Trainee = user.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                    if (Trainee != null)
                    {
                        item.Status = "ACCEPTED";
                        DBI.TrainingRequestItem.Attach(item);
                        DBI.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        DBI.SaveChanges();
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
                log.DocurmentAction = Id.ToString();
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = Id.ToString();
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = Id.ToString();
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ESSReject(int Id)
        {
            try
            {
                HumicaDBContextTR DBI = new HumicaDBContextTR();

                string Pending = SYDocumentStatus.PENDING.ToString();
                string Reject = SYDocumentStatus.REJECTED.ToString();
                var objmatch = DBI.TrainingRequestItem.Where(w => w.ID == Id && w.Status == Pending).ToList();
                if (objmatch == null) return "DOC_INV";
                var user = DBX.HRStaffProfiles.Where(w => w.EmpCode == User.UserName);
                int approverLevel = 0;
                foreach (var item in objmatch)
                {
                    var Trainee = user.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                    if (Trainee != null)
                    {
                        item.Status = Reject;
                        DBI.TrainingRequestItem.Attach(item);
                        DBI.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        DBI.SaveChanges();
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
                log.DocurmentAction = Id.ToString();
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = Id.ToString();
                log.Action = SYActionBehavior.RELEASE.ToString();

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
                log.DocurmentAction = Id.ToString();
                log.Action = SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public void SendEmail(string TrainNo,string Upload,string Rerceiver)
        {
            try
            {
                #region Email
                var ReqTraining = DB.TrainingRequestHeader.FirstOrDefault(w => w.TrainNo == TrainNo);
                var AlertTO = DBX.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == Rerceiver);
                var PIC = DBX.ExCfWFApprovers.FirstOrDefault(w => w.WFObject == "PIC");
                var receiver = "";
                if (PIC != null) {
                    var StaffPIC = DBX.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == PIC.Employee);
                    receiver = AlertTO.Email + ";" + StaffPIC.Email;
                }
                else receiver = AlertTO.Email;
                string URL = SYUrl.getBaseUrl() + "/Training/Process/ESSTrainingRequest/Details/" + ReqTraining.ID;
                var EmailConf = SMS.CFEmailAccounts.FirstOrDefault();
                if (!string.IsNullOrEmpty(receiver))
                {
                    string str = "<div style=\"margin-top: 30px;\"> <h1 style=\"background-color:#14A3C7;color:white; width: 600px;text-align:center;margin-bottom: 30px;\">TRAINING REQUEST FORM: " + "HCA" + "/" + ReqTraining.TrainNo + " </h1 > " + " </div>";
                    str += "<b>" + "Dear " + AlertTO.Title + " " + AlertTO.AllName + "</b>";
                    str += "<br /><br /> I would like to request your review and recommend on the training request form:";
                    str += "<br /><br /><b> Training Subject: " + ReqTraining.TrainingCourse + "</b>";
                    str += "<br /><br /><b> Training: Start Date: " + ReqTraining.FromDate.ToString("dd-MM-yyyy") + " " + "End Date: " + ReqTraining.ToDate.ToString("dd-MM-yyyy") + "</b>";
                    str += "<br /><br /> Please kindly click on the link for<b> approval:</b> " + string.Format("<a href='{0}'>Link</a>", URL);
                    CFEmailAccount emailAccount = EmailConf;
                    string subject = string.Format("Training Request Form {0:dd-MMM-yyyy}", DateTime.Today);
                    string body = str;
                    string filePath = Upload;
                    string fileName = Path.GetFileName(filePath);
                    EmailObject = new ClsEmail();
                    int rs = EmailObject.SendMailNew(emailAccount, "", receiver,
                        subject, body, filePath, fileName);
                }
                #endregion
            }
            catch
            {
                throw new Exception("FAIL_TO_SEND_MAIL");
            }
        }

        
        public string Approved(int id)
        {
            throw new NotImplementedException();
        }
        public List<ExCfWFApprover> getApproverListByDocType(string DocType, string Location,string department, string _ScreenID)
        {
            var listResult = new List<ExCfWFApprover>();
            ClsDocApproval docApproval = new ClsDocApproval();
            ListApproval = docApproval.SetAutoApproval(_ScreenID, DocType, Location, department);
            return listResult;
        }
        public void SetAutoApproval(string DocType, string Branch,string Department, string SCREEN_ID)
        {
            ListApproval = new List<ExDocApproval>();
            ClsDocApproval docApproval = new ClsDocApproval();
            ListApproval = docApproval.SetAutoApproval(SCREEN_ID, DocType, Branch, Department);
        }
        public string isValidApproval(ExDocApproval Approver, EnumActionGridLine Action)
        {
            if (Action == EnumActionGridLine.Add)
            {
                if (ListApproval.Where(w => w.Approver == Approver.Approver).ToList().Count > 0)
                {
                    return "DUPLICATED_ITEM";
                }
            }

            return SYConstant.OK;
        }
        //public void GetAllStaffs()
        //{
        //    var tblStaffProfile = DBX.HRStaffProfiles.Where(w => w.Status == "A" && w.DEPT == Dept).ToList();
        //    this.ListStaffs = tblStaffProfile.ToList();
        //}
    }

    public partial class TrainigModuleTemp
    {
        public int ID { get; set; }
        public string Invcode { get; set; }
        public string Coursecode { get; set; }
        public string TrainingType { get; set; }
        public string Topic { get; set; }
        public string Coursename { get; set; }
        public string ProgramName { get; set; }
        public string Document { get; set; }
        public string UrlDocument { get; set; }
        public string OtherDocument { get; set; }
        public string UrlOtherDocument { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
        public string Description { get; set; }
        public string DayTerm { get; set; }
        public decimal Timer1 { get; set; }
        public decimal Timer2 { get; set; }
    }
    public partial class TrainingExamQuizAnswer
    {
        public int ID { get; set; }
        public string ExamCode { get; set; }
        public string StaffID { get; set; }
        public string Program { get; set; }
        public string Course { get; set; }
        public string Module { get; set; }
        public Nullable<System.DateTime> ExamDate { get; set; }
        public decimal TotalScore { get; set; }
        public int CorrectAnswer { get; set; }
        public int InCorrectAnswer { get; set; }
        public bool IsInitialTest { get; set; }
        public bool IsFinalTest { get; set; }
    }
    public enum ExtStatus
    {
        RETURN, TRAINING, INVITED, FAILURED, PROCCESSING, WAITING, PASS, FINISHED, REPAIRING, CONFIRMED, ACKNOWLEDGE, PAID, SHIPPING, SHIPPED, BACKORDER, HOLD, OPEN, PENDING, PARTIAL, RELEASED, COMPLETED, APPROVED, RECEIVED, REJECTED, SOLD,
        PRECANCEL, CANCELLED, PRERELEASE, POSTED, DEPOSITED, REVERSED, CLEARED, NOTCLEAR, CLOSED, SIMULATED, COVERED, RESERVED, CREDIT
    }
}