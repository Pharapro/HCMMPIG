using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.HR
{
    public class HRSelfEvaluationObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public HREmpSelfEvaluation Header { get; set; }
        public List<HREmpSelfEvaluation> ListHeader { get; set; }
        public List<HREmpSelfEvaluationItem> ListItem { get; set; }
        public List<HREvalSelfEvaluate> ListSelfAssItem { get; set; }

        public HRSelfEvaluationObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateSelfEval()
        {
            try
            {
                DB = new HumicaDBContext();
                var lstAssass = DB.HREvalSelfEvaluates.ToList();
                var objCF = DB.ExCfWorkFlowItems.FirstOrDefault(w => w.ScreenID == ScreenId);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                var objNumber = new CFNumberRank(objCF.DocType, objCF.ScreenID);
                if (objNumber.NextNumberRank == null)
                {
                    return "NUMBER_RANK_NE";
                }
                Header.EvaluationCode = objNumber.NextNumberRank;

                foreach (var item in lstAssass)
                {
                    var obj = new HREmpSelfEvaluationItem();
                    obj.EvaluationCode = Header.EvaluationCode;
                    obj.QuestionCode = item.QuestionCode;
                    obj.Description = item.Description;
                    obj.SecDescription = item.SecDescription;
                    foreach (var read in ListItem.Where(w => w.QuestionCode == item.QuestionCode).ToList())
                    {
                        obj.Comment = read.Comment;
                    }
                    DB.HREmpSelfEvaluationItems.Add(obj);
                }
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                DB.HREmpSelfEvaluations.Add(Header);

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
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
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditEval(string id)
        {
            try
            {
                var objMatch = DB.HREmpSelfEvaluations.FirstOrDefault(w => w.EvaluationCode == id);
                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                var lstAssass = DB.HREvalSelfEvaluates.ToList();
                var ObjMatchItem = DB.HREmpSelfEvaluationItems.Where(w => w.EvaluationCode == objMatch.EvaluationCode).ToList();
                foreach (var read in ObjMatchItem)
                {
                    DB.HREmpSelfEvaluationItems.Attach(read);
                    DB.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                }

                foreach (var item in lstAssass)
                {
                    var obj = new HREmpSelfEvaluationItem();
                    obj.EvaluationCode = Header.EvaluationCode;
                    obj.QuestionCode = item.QuestionCode;
                    obj.Description = item.Description;
                    obj.SecDescription = item.SecDescription;
                    foreach (var read in ListItem.Where(w => w.QuestionCode == item.QuestionCode).ToList())
                    {
                        obj.Comment = read.Comment;
                    }
                    DB.HREmpSelfEvaluationItems.Add(obj);
                }
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                DB.HREmpSelfEvaluations.Attach(objMatch);
                DB.Entry(objMatch).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(x => x.ChangedOn).IsModified = true;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
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
                log.DocurmentAction = Header.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteEval(string id)
        {
            try
            {
                var objMatch = DB.HREmpEvaluates.FirstOrDefault(w => w.EvaluateID == id);
                var objScore = DB.HREmpEvaluateScores.Where(w => w.EvaluateID == id).ToList();

                if (objMatch == null)
                {
                    return "DOC_EN";
                }
                DB.HREmpEvaluates.Remove(objMatch);
                foreach (var read in objScore)
                {
                    DB.HREmpEvaluateScores.Remove(read);
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
                log.ScreenId = Header.EmpCode.ToString();
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
                log.ScreenId = Header.EmpCode.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }

}