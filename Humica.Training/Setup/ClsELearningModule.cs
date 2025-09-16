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
    public class ClsELearningModule
    {
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        public SYUser User { get; set; }
        public string ScreenID { get; set; }
        public string Course { get; set; }
        public string TrainingType { get; set; }
        public SYUserBusiness BS { get; set; }
        public TrainigModule Header { get; set; }
        public List<TrainigModule> ListQuestion { get; set; }

        public ClsELearningModule()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateQuestion()
        {
            try
            {
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                DBX.TrainigModule.Add(Header);

                DBX.SaveChanges();

                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.Topic;
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
                log.DocurmentAction = Header.Topic;
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
                log.DocurmentAction = Header.Topic;
                //log.DocurmentAction = ActionName;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ImportQuestion()
        {
            try
            {

                //SparepartEntities DBB = new SparepartEntities();

                //checking the duplicate date
                if (ListQuestion.Count == 0)
                {
                    return "LIST_NE";
                }
                try
                {
                    DBX.Configuration.AutoDetectChangesEnabled = false;

                    var lst = DBX.TrainigModule.ToList();
                    foreach (var item in ListQuestion)
                    {
                        var check = lst.Where(w => w.TrainingType == item.TrainingType && w.CourseCode == item.CourseCode && w.Topic == item.Topic).ToList();
                        if (check.Count > 0)
                        {
                            DBX.TrainigModule.Remove(check.First());
                        }


                    }

                    foreach (var item in ListQuestion)
                    {
                        if (
                        item.TrainingType == "" || item.CourseCode == "" ||
                        item.Topic == "" || item.Description == "" ||
                        item.Category == "" ||
                        item.DayTerm == "" || item.ScorePr == null ||
                        item.Score == null || item.Timer == null
                       )
                        {
                            return "REQUIRED_FIELD(*)";
                        }
                        if (item.Document != null)
                        {
                            string[] doc = item.Document.Split(';');
                            {
                                foreach (var r in doc)
                                {
                                    if (r != "")
                                    {
                                        item.UrlDocument += SYUrl.getBaseUrl() + "/Content/UPLOAD/TRAINING/DOC/" + item.TrainingType + "/" + item.CourseCode + "/" + item.Topic + "/" + r + ";";
                                    }
                                }
                            }

                        }
                        if (item.OtherDocument != null)
                        {
                            string[] doc = item.OtherDocument.Split(';');
                            {
                                foreach (var r in doc)
                                {
                                    if (r != "")
                                    {
                                        item.UrlOtherDocument += SYUrl.getBaseUrl() + "/Content/UPLOAD/TRAINING/VIDEO/" + item.TrainingType + "/" + item.CourseCode + "/" + item.Topic + "/" + r + ";";
                                    }
                                }
                            }

                        }
                        item.CourseName = DBX.TrainingCourseMaster.Where(w => w.Code == item.CourseCode).First().Description;
                        item.CreatedBy = User.UserName;
                        item.CreatedOn = DateTime.Now;
                        item.IsActive = true;
                        DBX.TrainigModule.Add(item);

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
        public string EditQuestion()
        {

            try
            {
                DBX = new HumicaDBContextTR();


                var question = DBX.TrainigModule.SingleOrDefault(w => w.Topic == Header.Topic);

                //Update user infor CampaignClass Header
                question.CreatedBy = User.UserName;
                question.CreatedOn = DateTime.Now;
                question.ChangedBy = User.UserName;
                question.ChangedOn = DateTime.Now;
                question.Description = Header.Description;
                question.IsActive = Header.IsActive;

                DBX.TrainigModule.Attach(question);

                DBX.Entry(question).Property(x => x.CreatedOn).IsModified = true;
                DBX.Entry(question).Property(x => x.CreatedBy).IsModified = true;
                DBX.Entry(question).Property(x => x.ChangedBy).IsModified = true;
                DBX.Entry(question).Property(x => x.ChangedOn).IsModified = true;
                DBX.Entry(question).Property(x => x.Description).IsModified = true;
                DBX.Entry(question).Property(x => x.IsActive).IsModified = true;
                DBX.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.Topic;
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
                log.DocurmentAction = Header.Topic;
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
                log.DocurmentAction = Header.Topic;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public string DeleteQuestion()
        {
            try
            {
                DBX = new HumicaDBContextTR();
                var program = DBX.TrainigModule.FirstOrDefault(w => w.Topic == Header.Topic);

                DBX.TrainigModule.Remove(program);

                DBX.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenID;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.Topic;
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
                log.DocurmentAction = Header.Topic;
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
                log.DocurmentAction = Header.Topic;
                //log.DocurmentAction = ActionName;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

    }
}