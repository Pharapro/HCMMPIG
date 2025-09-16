using Humica.Core.DB;
using Humica.EF;
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
    public class HREmpEvaluateObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string CompanyCode { get; set; }
        public string DocType { get; set; }
        public string Plant { get; set; }
        public string MessageCode { get; set; }
        public HREmpEvaluate Header { get; set; }
        public HREmpEvaluateScore Score { get; set; }
        public string ErrorMessage { get; set; }
        public List<HREvaluateFactor> ListFactor { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }

        public List<HREmpEvaluateScore> ListScore { get; set; }
        public List<HREmpEvaluate> ListHeader { get; set; }
        public List<HREvaluateRating> ListEvaluateRating { get; set; }
        public List<HREmpEvalRating> ListEmpRating { get; set; }
        public List<HREvaluateRegion> ListRegion { get; set; }
        public List<HREvaluateType> ListEvalType { get; set; }
        public List<HREvalSelfEvaluate> ListSelfEvaluateSetup { get; set; }
        public List<HR_VIEW_EmpEvaluationForm> ListEvaluatePending { get; set; }
        public List<HR_VIEW_EmpEvaluationForm> ListEvaluate { get; set; }
        public HREmpEvaluateProcess EvalProcess { get; set; }
        public List<HREmpEvaluateProcess> ListEvaluateProcess { get; set; }

        public HREmpEvaluateObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        #region 'HREmpEvaluationForm'
        public string CreateAppr()
        {
            try
            {
                DB = new HumicaDBContext();

                if (Header.EmpCode == null) return "EMPCODE_EN";
                if (Header.EvaluateType == null) return "EVALUATE_EN";
                if (Header.AssignedTo == null) return "ASSIGNEDEMP_EN";

                var lstRating = DB.HREvaluateRatings.ToList();
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
                Header.EvaluateID = objNumber.NextNumberRank;

                var EmpStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                List<HREvaluateRegion> ListRegion = DB.HREvaluateRegions.Where(w => w.EvaluateType == Header.EvaluateType).ToList();
                if (EmpStaff != null)
                {
                    Header.Position = EmpStaff.Position;
                    Header.Department = EmpStaff.Department;
                    Header.DateOfHire = EmpStaff.StartDate;
                    Header.EmpName = EmpStaff.AllName;
                    Header.TotalScore = 0;
                    foreach (var item in ListFactor)
                    {
                        var obj = new HREmpEvaluateScore();
                        obj.EvaluateID = Header.EvaluateID;
                        obj.Region = item.Region;
                        obj.Description = item.Description;
                        obj.SecDescription = item.SecDescription;
                        obj.Code = item.Code;
                        foreach (var read in ListScore.Where(w => w.Code == item.Code).ToList())
                        {
                            obj.RatingID = read.RatingID;
                            obj.Remark = read.Remark;
                            foreach (var read1 in lstRating.Where(w => w.Region == item.Region && w.ID == read.RatingID).ToList())
                            {
                                obj.Score = read1.Rating;
                                Header.TotalScore += (int)obj.Score;
                            }
                            if (read.Score.HasValue)
                            {
                                Header.TotalScore += (int)read.Score;
                            }
                        }
                        foreach (var read in ListRegion.Where(w => w.Code == item.Region).ToList())
                        {
                            obj.RegionDescription = read.Description;
                        }
                        DB.HREmpEvaluateScores.Add(obj);
                    }
                    var EvaRate = DB.HRAppGrades.ToList();
                    var Rate = EvaRate.Where(w => w.FromScore <= Header.TotalScore && w.ToScore >= Header.TotalScore).ToList();
                    foreach (var item in Rate)
                    {
                        Header.Result = item.Grade;
                    }
                    Header.Status = SYDocumentStatus.OPEN.ToString();
                    Header.CreatedBy = User.UserName;
                    Header.CreatedOn = DateTime.Now;
                    DB.HREmpEvaluates.Add(Header);
                }
                foreach (var item in lstRating)
                {
                    var EmpRating = new HREmpEvalRating();
                    EmpRating.EvaluateID = Header.EvaluateID;
                    EmpRating.RatingID = item.ID;
                    EmpRating.Region = item.Region;
                    EmpRating.Description = item.Description;
                    EmpRating.Rating = item.Rating;
                    DB.HREmpEvalRatings.Add(EmpRating);
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
        public string EditAppr(string EvaluateID)
        {
            try
            {
                DB = new HumicaDBContext();
                var objMatch = DB.HREmpEvaluates.FirstOrDefault(w => w.EvaluateID == EvaluateID);
                var _listScore = DB.HREmpEvaluateScores.Where(w => w.EvaluateID == EvaluateID).ToList();
                var EvalRating = DB.HREmpEvalRatings.ToList();
                List<HREvaluateRegion> ListRegion = DB.HREvaluateRegions.Where(w => w.EvaluateType == Header.EvaluateType).ToList();
                if (objMatch == null)
                {
                    return "DOC_INV";
                }

                objMatch.EmpName = Header.EmpName;
                objMatch.Position = Header.Position;
                objMatch.Department = Header.Department;
                objMatch.DateOfHire = Header.DateOfHire;
                objMatch.EvaluateType = Header.EvaluateType;
                objMatch.EvalFromDate = Header.EvalFromDate;
                objMatch.EvalToDate = Header.EvalToDate;
                objMatch.Strengths = Header.Strengths;
                objMatch.Weakness = Header.Weakness;
                objMatch.TrainingProgram = Header.TrainingProgram;
                objMatch.Comments = Header.Comments;
                objMatch.ActionPlan = Header.ActionPlan;
                foreach (var read in _listScore)
                {

                    DB.HREmpEvaluateScores.Attach(read);
                    DB.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                }
                Header.TotalScore = 0;
                foreach (var item in ListFactor)
                {
                    var obj = new HREmpEvaluateScore();
                    obj.EvaluateID = objMatch.EvaluateID;
                    obj.Region = item.Region;
                    obj.Description = item.Description;
                    obj.SecDescription = item.SecDescription;
                    obj.Code = item.Code;
                    foreach (var read in ListScore.Where(w => w.Code == item.Code).ToList())
                    {
                        obj.RatingID = read.RatingID;
                        obj.Remark = read.Remark;
                        foreach (var read1 in EvalRating.Where(w => w.Region == item.Region && w.RatingID == read.RatingID).ToList())
                        {
                            obj.Score = read1.Rating;
                            Header.TotalScore += (int)obj.Score;
                        }
                    }
                    foreach (var read in ListRegion.Where(w => w.Code == item.Region).ToList())
                    {
                        obj.RegionDescription = read.Description;
                    }
                    DB.HREmpEvaluateScores.Add(obj);
                }
                var EvaRate = DB.HRAppGrades.ToList();
                var Rate = EvaRate.Where(w => w.FromScore <= Header.TotalScore && w.ToScore >= Header.TotalScore).ToList();
                foreach (var item in Rate)
                {
                    Header.Result = item.Grade;
                    objMatch.Result = item.Grade;

                }
                objMatch.TotalScore = Header.TotalScore;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChengedOn = DateTime.Now;
                DB.HREmpEvaluates.Attach(objMatch);
                DB.Entry(objMatch).Property(x => x.TotalScore).IsModified = true;
                DB.Entry(objMatch).Property(x => x.DateOfHire).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Position).IsModified = true;
                DB.Entry(objMatch).Property(x => x.EmpName).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Department).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Strengths).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Weakness).IsModified = true;
                DB.Entry(objMatch).Property(x => x.TrainingProgram).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Comments).IsModified = true;
                DB.Entry(objMatch).Property(x => x.ActionPlan).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Result).IsModified = true;
                DB.Entry(objMatch).Property(x => x.EvalFromDate).IsModified = true;
                DB.Entry(objMatch).Property(x => x.EvalToDate).IsModified = true;
                DB.Entry(objMatch).Property(x => x.EvaluateType).IsModified = true;
                DB.Entry(objMatch).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(x => x.ChengedOn).IsModified = true;
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
        }
        public string DeleteAppr(string id)
        {
            try
            {
                var objMatch = DB.HREmpEvaluates.FirstOrDefault(w => w.EvaluateID == id);
                var objScore = DB.HREmpEvaluateScores.Where(w => w.EvaluateID == id).ToList();
                var objRating = DB.HREmpEvalRatings.Where(w => w.EvaluateID == id).ToList();
                if (objMatch == null)
                {
                    return "DOC_EN";
                }
                DB.HREmpEvaluates.Remove(objMatch);
                foreach (var read in objScore)
                {
                    DB.HREmpEvaluateScores.Remove(read);
                }
                foreach (var read in objRating)
                {
                    DB.HREmpEvalRatings.Remove(read);
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
        #endregion
        #region 'SelfServiceEvaluateForm'
        public string EditEvaluateForm(string EvaluateID)
        {
            try
            {
                DB = new HumicaDBContext();
                var objMatch = DB.HREmpEvaluates.FirstOrDefault(w => w.EvaluateID == EvaluateID);
                var _listScore = DB.HREmpEvaluateScores.Where(w => w.EvaluateID == EvaluateID).ToList();
                var EvalRating = DB.HREmpEvalRatings.ToList();

                if (objMatch == null) return "DOC_INV";

                foreach (var read in _listScore)
                {
                    DB.HREmpEvaluateScores.Attach(read);
                    DB.Entry(read).State = System.Data.Entity.EntityState.Deleted;
                }
                Header.TotalScore = 0;
                foreach (var item in ListFactor)
                {
                    var obj = new HREmpEvaluateScore();
                    obj.EvaluateID = objMatch.EvaluateID;
                    obj.Region = item.Region;
                    obj.Description = item.Description;
                    obj.SecDescription = item.SecDescription;
                    obj.Code = item.Code;
                    foreach (var read in ListScore.Where(w => w.Code == item.Code).ToList())
                    {
                        obj.RatingID = read.RatingID;
                        obj.Remark = read.Remark;
                        foreach (var read1 in EvalRating.Where(w => w.Region == item.Region && w.RatingID == read.RatingID).ToList())
                        {
                            obj.Score = read1.Rating;
                            Header.TotalScore += (int)obj.Score;
                        }
                    }
                    DB.HREmpEvaluateScores.Add(obj);
                }
                var EvaRate = DB.HRAppGrades.ToList();
                var Rate = EvaRate.Where(w => w.FromScore <= Header.TotalScore && w.ToScore >= Header.TotalScore).ToList();
                foreach (var item in Rate)
                {
                    Header.Result = item.Grade;
                    objMatch.Result = item.Grade;
                }
                objMatch.Status = SYDocumentStatus.PROCESSING.ToString();
                objMatch.TotalScore = Header.TotalScore;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChengedOn = DateTime.Now;
                DB.HREmpEvaluates.Attach(objMatch);
                DB.Entry(objMatch).Property(x => x.TotalScore).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Status).IsModified = true;
                DB.Entry(objMatch).Property(x => x.Result).IsModified = true;
                DB.Entry(objMatch).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(x => x.ChengedOn).IsModified = true;
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
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion
        #region 'EvaluationProcess'
        public string EditEvaluateProcess(string EvaluateID)
        {
            try
            {
                DB = new HumicaDBContext();

                if (EvalProcess.Status == null) return "EE00S2";

                var objMatch = DB.HREmpEvaluates.FirstOrDefault(w => w.EvaluateID == EvaluateID);

                if (objMatch == null) return "DOC_INV";

                var _EvalProcess = new HREmpEvaluateProcess();

                _EvalProcess.EvaluateID = objMatch.EvaluateID;
                _EvalProcess.EmpName = objMatch.EmpName;
                _EvalProcess.EmpCode = objMatch.EmpCode;
                _EvalProcess.EvaluateType = objMatch.EvaluateType;
                _EvalProcess.EvalFromDate = objMatch.EvalFromDate;
                _EvalProcess.EvalToDate = objMatch.EvalToDate;
                _EvalProcess.Position = objMatch.Position;
                _EvalProcess.Result = objMatch.Result;
                _EvalProcess.TotalScore = objMatch.TotalScore;
                _EvalProcess.AssignedTo = objMatch.AssignedTo;
                _EvalProcess.AssignedPosition = objMatch.AssignedPosition;
                _EvalProcess.EvaluateDate = EvalProcess.EvaluateDate;
                _EvalProcess.Status = EvalProcess.Status;
                _EvalProcess.Comment = EvalProcess.Comment;
                _EvalProcess.Increase = EvalProcess.Increase;
                _EvalProcess.Attachment = EvalProcess.Attachment;
                _EvalProcess.CreatedBy = User.UserName;
                _EvalProcess.CreatedOn = DateTime.Now;


                objMatch.Status = SYDocumentStatus.COMPLETED.ToString();
                DB.HREmpEvaluates.Attach(objMatch);
                DB.Entry(objMatch).Property(x => x.Status).IsModified = true;

                DB.HREmpEvaluateProcesses.Add(_EvalProcess);

                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.EmpCode;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion
        #region 'Status'
        public string requestToApprove(string id)
        {
            try
            {
                var objMatch = DB.HREmpEvaluates.FirstOrDefault(w => w.EvaluateID == id);
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
                int row = DB.SaveChanges();

                //#region *****Send to Telegram
                //var Objmatch = DB.HR_PO_VIEW.FirstOrDefault(w => w.PONumber == Header.PONumber);
                //var excfObject = DB.ExCfWorkFlowItems.Find(ScreenId, Header.DocumentType);
                //Humica.Core.SY.SYSendTelegramObject wfo =
                //    new Humica.Core.SY.SYSendTelegramObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER, Header.Status);
                //wfo.User = User;
                //wfo.ListObjectDictionary = new List<object>();
                //wfo.ListObjectDictionary.Add(Objmatch);
                //HRStaffProfile Staff = getNextApprover(Header.PONumber, "");
                //wfo.ListObjectDictionary.Add(Staff);
                //wfo.Send_SMS_Telegram(excfObject.Telegram, "");
                //#endregion

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = Header.EvaluateID;
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
                log.DocurmentAction = Header.EvaluateID;
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
                log.DocurmentAction = Header.EvaluateID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string ApproveDoc(string id)
        {
            try
            {
                var objMatch = DB.HREmpEvaluates.FirstOrDefault(w => w.EvaluateID == id);
                if (objMatch == null) return "DOC_NE";
                Header = objMatch;
                if (objMatch.Status != SYDocumentStatus.PROCESSING.ToString()) return "INV_DOC";

                objMatch.Status = SYDocumentStatus.APPROVED.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = Header.EvaluateID;
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
                log.DocurmentAction = Header.EvaluateID;
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
                log.DocurmentAction = Header.EvaluateID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string CancelDoc(string id)
        {
            try
            {
                var objMatch = DB.HREmpEvaluates.FirstOrDefault(w => w.EvaluateID == id);
                if (objMatch == null) return "DOC_NE";
                Header = objMatch;
                //if (objMatch.Status != SYDocumentStatus.PROCESSING.ToString()) return "INV_DOC";

                objMatch.Status = SYDocumentStatus.CANCELLED.ToString();

                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = Header.EvaluateID;
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
                log.DocurmentAction = Header.EvaluateID;
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
                log.DocurmentAction = Header.EvaluateID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion
    }
}