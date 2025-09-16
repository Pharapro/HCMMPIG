using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using Humica.Training.DB;

namespace Humica.Training
{

    public class TrainingSurveyObject
    {
        public HumicaDBContextTR DB = new HumicaDBContextTR();

        private HumicaDBViewContext DBV = new HumicaDBViewContext();

        public Core.DB.HumicaDBContext DBX = new Core.DB.HumicaDBContext();

        public SYUser User { get; set; }

        public SYUserBusiness BS { get; set; }

        public string ScreenId { get; set; }

        public string DocType { get; set; }
        public FTDGeneralAccount filter { get; set; }
        public TRTrainingSurvey Header { get; set; }
        public List<TRSurveyFactor> ListFactor { get; set; }

        public List<TREmpSurveyScore> ListScore { get; set; }

        public List<TRTrainingSurvey> ListHeader { get; set; }

        public List<TRSurveyRating> ListSurveyRating { get; set; }
        public List<TRSurveyRegion> ListRegion { get; set; }
        public List<TRTrainingAttendance> ListAttence { get; set; }
        public List<TRTrainingEmployee> ListEmpStaff { get; set; }
        public List<TRTrainingInvitation> Listinvite { get; set; }


        public TrainingSurveyObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        public string CreateSurvey(string CalendarNO, string lineItem)
        {
            try
            {
                DB = new HumicaDBContextTR();
                string[] Emp = CalendarNO.Split(';');
                string[] Line = lineItem.Split(';');
                for (var i = 0; i < Emp.Length; i++)
                {
                    var Header = new TRTrainingSurvey();
                    if (Emp[i] == null)
                    {
                        return "EMPCODE_EN";
                    }
                    var objCF = DBX.ExCfWorkFlowItems.Find(ScreenId, DocType);
                    if (objCF == null)
                    {
                        return "REQUEST_TYPE_NE";
                    }
                    var objNumber = new CFNumberRank(objCF.DocType, objCF.ScreenID);
                    if (objNumber == null)
                    {
                        return "NUMBER_RANK_NE";
                    }

                    Header.SurveyID = objNumber.NextNumberRank;
                    int Seleteids = Convert.ToInt32(Emp[i]);
                    int SelectLine = Int32.Parse(Line[i]);
                    var listemp = DB.TRTrainingEmployees.Where(w => w.CalendarID == Seleteids && w.LineItem == SelectLine).ToList();
                    HR_STAFF_VIEW hR_STAFF_VIEW = DBV.HR_STAFF_VIEW.AsEnumerable().FirstOrDefault(w => listemp.Where(y => w.EmpCode == y.EmpCode).Any());
                    if (hR_STAFF_VIEW != null)
                    {
                        Header.SurveyDate = DateTime.Now;
                        Header.SurFromDate = DateTime.Now;
                        Header.SurToDate = DateTime.Now;
                        Header.Position = hR_STAFF_VIEW.Position;
                        Header.Department = hR_STAFF_VIEW.Department;
                        Header.DateOfHire = hR_STAFF_VIEW.StartDate;
                        Header.EmpName = hR_STAFF_VIEW.AllName;
                        Header.EmpCode = hR_STAFF_VIEW.EmpCode;
                        Header.CreatedBy = User.UserName;
                        Header.CreatedOn = DateTime.Now;
                        Header.TotalScore = 0;
                        foreach (TRTrainingEmployee item in listemp)
                        {
                            item.IsAssign = true;
                            DB.TRTrainingEmployees.Attach(item);
                            DB.Entry(item).Property(w => w.IsAssign).IsModified = true;
                        }
                        DB.TRTrainingSurveys.Add(Header);
                    }
                }
                DB.SaveChanges();
                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                SYEventLog sYEventLog = new SYEventLog();
                sYEventLog.ScreenId = ScreenId;
                sYEventLog.UserId = User.UserName;
                sYEventLog.DocurmentAction = CalendarNO;
                sYEventLog.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog, e);
                return "EE001";
            }
            catch (DbUpdateException e2)
            {
                SYEventLog sYEventLog2 = new SYEventLog();
                sYEventLog2.ScreenId = ScreenId;
                sYEventLog2.UserId = User.UserName;
                sYEventLog2.DocurmentAction = CalendarNO;
                sYEventLog2.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog2, e2, catchError: true);
                return "EE001";
            }
        }

        public string EditAppr(string ID)
        {
            try
            {
                DB = new HumicaDBContextTR();

                TRTrainingSurvey tRTrainingSurvey = DB.TRTrainingSurveys.FirstOrDefault((TRTrainingSurvey w) => w.SurveyID == ID);
                List<TREmpSurveyScore> list = DB.TREmpSurveyScores.Where((TREmpSurveyScore w) => w.SurveyID == ID).ToList();
                List<TRSurveyRating> source = DB.TRSurveyRatings.ToList();
                if (tRTrainingSurvey == null)
                {
                    return "DOC_INV";
                }
                foreach (TREmpSurveyScore item2 in list)
                {
                    DB.TREmpSurveyScores.Attach(item2);
                    DB.Entry(item2).State = EntityState.Deleted;
                }
                Header.TotalScore = 0;

                foreach (var read in ListScore)//.Where(w => w.Code == item.Code).ToList()
                {
                    var _Factor = DB.TRSurveyFactors.FirstOrDefault(w => w.Code == read.Code);
                    var obj = new TREmpSurveyScore();
                    obj.Region = _Factor.Region;
                    obj.Description = _Factor.Description;
                    obj.SecDescription = _Factor.SecDescription;
                    obj.Score = read.Score;
                    obj.Code = read.Code;
                    obj.Remark = read.Remark;
                    if (list.Count > 0)
                    {
                        obj.Region = _Factor.Region;
                        obj.Description = _Factor.Description;
                        obj.SecDescription = _Factor.SecDescription;
                        obj.Score = read.Score;
                        obj.Code = read.Code;
                        obj.Remark = read.Remark;
                        if (list.Count > 0)
                        {
                            obj.SurveyID = read.SurveyID;
                        }
                        else
                        {
                            obj.SurveyID = ID;
                        }
                        Header.TotalScore += read.Score.Value;
                        DB.TREmpSurveyScores.Add(obj);
                    }
                    tRTrainingSurvey.TotalScore = Header.TotalScore;
                    tRTrainingSurvey.ChangedBy = User.UserName;
                    tRTrainingSurvey.ChengedOn = DateTime.Now;
                    DB.TRTrainingSurveys.Attach(tRTrainingSurvey);
                    DB.Entry(tRTrainingSurvey).State = EntityState.Modified;
                    int num = DB.SaveChanges();
                    
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                SYEventLog sYEventLog = new SYEventLog();
                sYEventLog.ScreenId = ScreenId;
                sYEventLog.UserId = User.UserName;
                sYEventLog.DocurmentAction = Header.EmpCode;
                sYEventLog.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog, e);
                return "EE001";
            }
        }

        //public string DeleteAppr(string id)
        //{
        //    try
        //    {

        //        TRTrainingSurvey tREmpSurvey = DB.TRTrainingSurveys.FirstOrDefault((TRTrainingSurvey w) => w.SurveyID == id);
        //        List<TREmpSurveyScore> list = DB.TREmpSurveyScores.Where((TREmpSurveyScore w) => w.SurveyID == id).ToList();
        //        List<TREmpSurRating> list2 = DB.TREmpSurRatings.Where((TREmpSurRating w) => w.SurveyID == id).ToList();
        //        if (tREmpSurvey == null)
        //        {
        //            return "DOC_EN";
        //        }

        //        DB.TRTrainingSurveys.Remove(tREmpSurvey);
        //        foreach (TREmpSurveyScore item in list)
        //        {
        //            DB.TREmpSurveyScores.Remove(item);
        //        }

        //        int num = DB.SaveChanges();
        //        return "OK";
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        SYEventLog sYEventLog = new SYEventLog();
        //        sYEventLog.ScreenId = ScreenId;
        //        sYEventLog.UserId = User.UserID.ToString();
        //        sYEventLog.ScreenId = Header.EmpCode.ToString();
        //        sYEventLog.Action = SYActionBehavior.EDIT.ToString();
        //        SYEventLogObject.saveEventLog(sYEventLog, e);
        //        return "EE001";
        //    }
        //    catch (Exception e2)
        //    {
        //        SYEventLog sYEventLog2 = new SYEventLog();
        //        sYEventLog2.ScreenId = ScreenId;
        //        sYEventLog2.UserId = User.UserID.ToString();
        //        sYEventLog2.ScreenId = Header.EmpCode.ToString();
        //        sYEventLog2.Action = SYActionBehavior.EDIT.ToString();
        //        SYEventLogObject.saveEventLog(sYEventLog2, e2, b: true);
        //        return "EE001";
        //    }
        //}

        public string EditSurveyForm(string SurveyID)
        {
            try
            {
                DB = new HumicaDBContextTR();
                TRTrainingSurvey tREmpSurvey = DB.TRTrainingSurveys.FirstOrDefault((TRTrainingSurvey w) => w.SurveyID == SurveyID);
                List<TREmpSurveyScore> list = DB.TREmpSurveyScores.Where((TREmpSurveyScore w) => w.Code == "").ToList();
                List<TREmpSurRating> source = DB.TREmpSurRatings.ToList();
                if (tREmpSurvey == null)
                {
                    return "DOC_INV";
                }

                foreach (TREmpSurveyScore item2 in list)
                {
                    DB.TREmpSurveyScores.Attach(item2);
                    DB.Entry(item2).State = EntityState.Deleted;
                }

                Header.TotalScore = 0;
                foreach (TRSurveyFactor item in ListFactor)
                {
                    TREmpSurveyScore tREmpSurveyScore = new TREmpSurveyScore();
                    //tREmpSurveyScore.SurveyID = tREmpSurvey.SurveyID;
                    tREmpSurveyScore.Region = item.Region;
                    tREmpSurveyScore.Description = item.Description;
                    tREmpSurveyScore.SecDescription = item.SecDescription;
                    tREmpSurveyScore.Code = item.Code;
                    foreach (TREmpSurveyScore read in ListScore.Where((TREmpSurveyScore w) => w.Code == item.Code).ToList())
                    {
                        // tREmpSurveyScore.RatingID = read.RatingID;
                        tREmpSurveyScore.Remark = read.Remark;
                        foreach (TREmpSurRating item3 in source.Where((TREmpSurRating w) => w.Region == item.Region).ToList())
                        {
                            tREmpSurveyScore.Score = item3.Rating;
                            Header.TotalScore += tREmpSurveyScore.Score.Value;
                        }
                    }

                    DB.TREmpSurveyScores.Add(tREmpSurveyScore);
                }

                List<HRAppGrade> source2 = DBX.HRAppGrades.ToList();
                List<HRAppGrade> list2 = source2.Where((HRAppGrade w) => w.FromScore <= (decimal)Header.TotalScore && w.ToScore >= (decimal)Header.TotalScore).ToList();
                foreach (HRAppGrade item4 in list2)
                {
                    Header.Result = item4.Grade;
                    Header.Result = item4.Grade;
                }

                //tREmpSurvey.Status = SYDocumentStatus.PROCESSING.ToString();
                tREmpSurvey.TotalScore = Header.TotalScore;
                tREmpSurvey.ChangedBy = User.UserName;
                tREmpSurvey.ChengedOn = DateTime.Now;
                DB.TRTrainingSurveys.Attach(tREmpSurvey);

                DB.Entry(tREmpSurvey).Property((TRTrainingSurvey x) => x.TotalScore).IsModified = true;
                //DBD.Entry(tREmpSurvey).Property((TRTrainingSurvey x) => x.Status).IsModified = true;
                DB.Entry(tREmpSurvey).Property((TRTrainingSurvey x) => x.Result).IsModified = true;
                DB.Entry(tREmpSurvey).Property((TRTrainingSurvey x) => x.ChangedBy).IsModified = true;
                DB.Entry(tREmpSurvey).Property((TRTrainingSurvey x) => x.ChengedOn).IsModified = true;
                int num = DB.SaveChanges();
                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                SYEventLog sYEventLog = new SYEventLog();
                sYEventLog.ScreenId = ScreenId;
                sYEventLog.UserId = User.UserName;
                sYEventLog.DocurmentAction = Header.EmpCode;
                sYEventLog.Action = SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(sYEventLog, e);
                return "EE001";
            }
        }
    }
}
