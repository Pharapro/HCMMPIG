using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Training.Setup
{

    public class ClsTRELearningAnswer
    {
        private HumicaDBContextTR DBX = new HumicaDBContextTR();
        public SYUser User { get; set; }
        public string ScreenID { get; set; }

        public const string PARAM_BRANCH = "BRANCH";
        public SYUserBusiness BS { get; set; }
        public TRELearningAnswer Header { get; set; }
        public List<TRELearningAnswer> ListAnswer { get; set; }
        public List<TRTrainingType> ListTrainType { get; set; }
        public List<TrainigModule> ListModule { get; set; }
        public List<TRELearningQuestion> ListQuestion { get; set; }
        public string TrainingType { get; set; }
        public string Course { get; set; }
        public ClsTRELearningAnswer()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }


        #region "Import Answer"
        public string ImportAnswer()
        {
            try
            {

                //SparepartEntities DBB = new SparepartEntities();

                //checking the duplicate date
                if (ListAnswer.Count == 0)
                {
                    return "LIST_NE";
                }
                try
                {
                    DBX.Configuration.AutoDetectChangesEnabled = false;

                    var lst = DBX.TRELearningAnswer.ToList();
                    foreach (var item in ListAnswer)
                    {
                        var check = lst.Where(w => w.TrainingType == item.TrainingType && w.CourseCode == item.CourseCode && w.Topic == item.Topic && w.QuestionCode == item.QuestionCode && w.AnswerCode == item.AnswerCode).ToList();
                        if (check.Count > 0)
                        {
                            DBX.TRELearningAnswer.Remove(check.First());
                        }


                    }

                    foreach (var item in ListAnswer)
                    {
                        if (
                        item.TrainingType == "" || item.CourseCode == "" ||
                        item.Topic == "" || item.QuestionCode == "" ||
                        item.Description == "" || item.AnswerCode == "" ||
                        item.TotalScore == null || item.ColumnCheck == null ||
                        item.CorrectValue == null
                       )
                        {
                            return "REQUIRED_FIELD(*)";
                        }

                        item.CreatedBy = User.UserName;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;

                        DBX.TRELearningAnswer.Add(item);

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
        #endregion

    }
}