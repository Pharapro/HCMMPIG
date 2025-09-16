using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;

namespace Humica.Training.Setup
{


    public class ClsELearningQuestion
    {
        public HumicaDBContextTR DBX = new HumicaDBContextTR();
        public SYUser User { get; set; }
        public string ScreenID { get; set; }
        public const string PARAM_BRANCH = "BRANCH";
        public SYUserBusiness BS { get; set; }
        public TRELearningQuestion Header { get; set; }
        public List<TRELearningQuestion> ListQuestion { get; set; }
        public List<TrainigModule> ListModule { get; set; }
        public List<TRTrainingType> ListTrainType { get; set; }
        public string Course { get; set; }
        public string TrainingType { get; set; }
        public ClsELearningQuestion()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
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
        public string ImportQuestion()
        {
            try
            {
                //checking the duplicate date
                if (ListQuestion.Count == 0)
                {
                    return "LIST_NE";
                }
                try
                {
                    DBX.Configuration.AutoDetectChangesEnabled = false;

                    var lst = DBX.TRELearningQuestion.ToList();
                    foreach (var item in ListQuestion)
                    {
                        var check = lst.Where(w => w.TrainingType == item.TrainingType && w.CourseCode == item.CourseCode && w.Topic == item.Topic && w.QuestionCode == item.QuestionCode).ToList();
                        if (check.Count > 0)
                        {

                            DBX.TRELearningQuestion.Remove(check.First());
                        }
                    }

                    foreach (var item in ListQuestion)
                    {
                        if (
                        item.TrainingType == "" || item.CourseCode == "" ||
                        item.Topic == "" || item.QuestionCode == "" ||
                        item.Description == ""
                       )
                        {
                            return "REQUIRED_FIELD(*)";
                        }
                        if (item.Attachement != null)
                        {
                            item.Attachement = SYUrl.getBaseUrl() + "/Content/UPLOAD/TRAINING/QUIZ/" + item.TrainingType + "/" + item.CourseCode + "/" + item.Topic + "/" + item.Attachement;
                        }

                        item.CreatedBy = User.UserName;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;

                        DBX.TRELearningQuestion.Add(item);
                    }
                }
                finally
                {
                    DBX.Configuration.AutoDetectChangesEnabled = true;
                }

                DBX.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserID.ToString();
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
                log.ScreenId = ScreenID;
                log.UserId = User.UserID.ToString();
                log.DocurmentAction = "";
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch
            {
                return "EE001";
            }
        }

        public string DeleteQuestion(string Course)
        {
            try
            {
                DBX = new HumicaDBContextTR();
                if (Course == null)
                {
                    return "COURSE_EMPTY";
                }
                var trc = DBX.TRELearningQuestion.ToList();
                string[] c = Course.Split(';');

                foreach (var r in c)
                {
                    int id = Convert.ToInt32(r);
                    var h = trc.Single(w => w.ID == id);
                    if (h != null)
                    {
                        DBX.TRELearningQuestion.Remove(h);
                    }
                }

                DBX.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.QuestionCode;
                //log.DocurmentAction = ActionName;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.QuestionCode;
                //log.DocurmentAction = ActionName;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.QuestionCode;
                //log.DocurmentAction = ActionName;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}