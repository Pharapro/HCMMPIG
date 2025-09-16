using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.RCM
{
    public class ClsRCMInterviewProc : IClsRCMInterviewProc
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public RCMApplicant Filter { get; set; }
        public string ApplyPosition { get; set; }
        public string DocType { get; set; }
        public string Vacancy { get; set; }
        public string ChkData { get; set; }
        public int IntvStep { get; set; }
        public RCMPInterview Header { get; set; }
        public RCMApplicant App { get; set; }
        public string ScreenId { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public List<RCMPInterview> ListInterview { get; set; }
        public List<RCMPInterview> ListWaiting { get; set; }
        public List<RCMAEdu> ListEdu { get; set; }
        public List<RCMAWorkHistory> ListWorkHistory { get; set; }
        public List<RCMALanguage> ListLanguage { get; set; }
        public List<RCMInterveiwFactor> ListFactor { get; set; }
        public List<RCMEmpEvaluateScore> ListScore { get; set; }
        public List<RCMInterveiwRating> ListInterviewRating { get; set; }
        public List<RCMInterveiwRegion> ListRegion { get; set; }
        public string ErrorMessage { get; set; }

        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsRCMInterviewProc()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public string createIntV(string TranNo)
        {
            OnLoad();
            try
            {
                if (string.IsNullOrEmpty(Header.Status)) return "INV_STATUS";
                int Tran = Convert.ToInt32(TranNo);
                var lstRating = unitOfWork.Repository<RCMInterveiwRating>().Queryable().ToList();
                var Open = SYDocumentStatus.OPEN.ToString();
                var Pass = SYDocumentStatus.PASS.ToString();
                var ObjMatch = unitOfWork.Repository<RCMPInterview>().Queryable().FirstOrDefault(w => w.TranNo == Tran);
                if (ObjMatch != null)
                {
                    if (ObjMatch.Status != Open)
                        return "Candidate interview already!";
                }
                var chkApplicant = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == ObjMatch.ApplicantID);
                ObjMatch.TotalScore = 0;
                foreach (var read in ListScore)//.Where(w => w.Code == item.Code).ToList()
                {
                    var _Factor = unitOfWork.Repository<RCMInterveiwFactor>().Queryable().FirstOrDefault(w => w.Code == read.Code);
                    var obj = new RCMEmpEvaluateScore();
                    obj.Applicant = ObjMatch.ApplicantID;
                    obj.Region = _Factor.Region;
                    obj.Description = _Factor.Description;
                    obj.SecDescription = _Factor.SecDescription;
                    obj.Code = read.Code;
                    obj.Score = read.Score;
                    obj.InVStep = ObjMatch.IntVStep;
                    ObjMatch.TotalScore += (int)read.Score;
                    obj.Remark = read.Remark;
                    unitOfWork.Add(obj);
                }
                var Rating = unitOfWork.Repository<RCMInterveiwRegion>().Queryable().ToList().Sum(w => w.Rating);
                var Resul = ListScore.ToList().Sum(w => w.Score);
                if (Resul >= Rating)
                {
                    ObjMatch.Result = Pass;
                    if (Header.Status != "REJECT" && Header.Status != "CONSIDER") ObjMatch.Status = Pass;
                    else ObjMatch.Status = Header.Status;
                }
                else
                {
                    ObjMatch.Result = "FAIL";
                    if (Header.Status != "REJECT" && Header.Status != "Consider") ObjMatch.Status = "FAIL";
                    else ObjMatch.Status = Header.Status;
                }
                if (chkApplicant != null)
                {
                    if (Header.Status == "NEXTSTEP")
                    {
                        chkApplicant.IntvStep = ObjMatch.IntVStep + 1;
                        chkApplicant.IntVStatus = Open;
                        chkApplicant.CurStage = "Interview Step " + ObjMatch.IntVStep;
                    }
                    else
                    {
                        if (Header.Status == null)
                        {
                            chkApplicant.IntVStatus = ObjMatch.Status;
                        }
                        else
                            chkApplicant.IntVStatus = Header.Status;
                        chkApplicant.CurStage = Header.Status + " in Interview";
                    }
                    chkApplicant.PostOffer = Header.PositionOffer;
                    chkApplicant.SalaryAfterProb = Header.SalaryAfterProb;
                    chkApplicant.Salary = Header.ProposedSalary;
                    unitOfWork.Update(chkApplicant);
                }

                ObjMatch.ApplyDate = Header.ApplyDate;
                ObjMatch.ReStatus = Header.ReStatus;
                ObjMatch.IntVDate = Header.IntVDate;
                ObjMatch.Strength = Header.Strength;
                if (Header.Status != null)
                    ObjMatch.Status = Header.Status;
                ObjMatch.Weakness = Header.Weakness;
                ObjMatch.IntCmt = Header.IntCmt;
                ObjMatch.ProposedSalary = Header.ProposedSalary;
                ObjMatch.PositionOffer = Header.PositionOffer;
                ObjMatch.AttachFile = Header.AttachFile;
                ObjMatch.SalaryAfterProb = Header.SalaryAfterProb;

                unitOfWork.Update(ObjMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Cancel(string TranNo)
        {
            OnLoad();
            try
            {
                int Tran = Convert.ToInt32(TranNo);
                var objmatch = unitOfWork.Repository<RCMPInterview>().Queryable().First(w => w.TranNo == Tran);

                if (objmatch == null) return "DOC_INV";

                var chkApplicant = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == objmatch.ApplicantID);
                if (chkApplicant != null)
                {
                    chkApplicant.IntVStatus = SYDocumentStatus.CANCELLED.ToString();
                    unitOfWork.Update(chkApplicant);
                }
                objmatch.Status = SYDocumentStatus.CANCELLED.ToString();
                unitOfWork.Update(objmatch);
                unitOfWork.Save();
                return SYConstant.OK;

            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Passed(int TranNo)
        {
            OnLoad();
            try
            {
                var objmatch = unitOfWork.Repository<RCMPInterview>().Queryable().First(w => w.TranNo == TranNo);

                if (objmatch == null) return "DOC_INV";

                var chkApplicant = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == objmatch.ApplicantID);
                if (chkApplicant != null)
                {
                    chkApplicant.IntVStatus = SYDocumentStatus.PASS.ToString();
                    unitOfWork.Update(chkApplicant);
                }
                objmatch.Status = SYDocumentStatus.PASS.ToString();
                unitOfWork.Update(objmatch);
                unitOfWork.Save();
                return SYConstant.OK;

            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }

        }
        public string Keepped(int TranNo)
        {
            OnLoad();
            try
            {
                RCMPInterview objmatch = unitOfWork.Repository<RCMPInterview>().Queryable().First(w => w.TranNo == TranNo);

                if (objmatch == null) return "DOC_INV";

                var chkApplicant = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == objmatch.ApplicantID);
                if (chkApplicant != null)
                {
                    chkApplicant.IntVStatus = "KEEP";
                    unitOfWork.Update(chkApplicant);
                }
                objmatch.Status = "KEEP";
                unitOfWork.Update(objmatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }

        }
        public string NextStep(int TranNo)
        {
            OnLoad();
            var ApplicantID = "";
            try
            {
                var ObjMatch = unitOfWork.Repository<RCMPInterview>().Queryable().Where(w => w.TranNo == TranNo).ToList().OrderBy(w => w.IntVStep).LastOrDefault();
                if (ObjMatch == null) return "INV_DOC";
                ApplicantID = ObjMatch.ApplicantID;
                var candidate = unitOfWork.Repository<RCMPInterview>().Queryable().Where(w => w.ApplicantID == ObjMatch.ApplicantID).ToList();
                var candidate_open = candidate.Where(w => w.Status == SYDocumentStatus.OPEN.ToString()).ToList();
                var candidate_FAIL = candidate.Where(w => w.Status == "FAIL").ToList();
                var candidate_Consider = candidate.Where(w => w.Status == "Consider").ToList();
                if (candidate_open.Any()) return "CDD_OPEN";
                if (candidate_FAIL.Any()) return "CDD_FAIL";
                if (candidate_Consider.Any()) return "CDD_CONSIDER";
                var Applica_ = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == ObjMatch.ApplicantID);
                if (Applica_ != null && Applica_.IsHired == true) return "CDD_HIRED";

                var _interview = new RCMPInterview
                {
                    ApplyDate = ObjMatch.ApplyDate,
                    ApplicantID = ObjMatch.ApplicantID,
                    Status = SYDocumentStatus.OPEN.ToString(),
                    ReStatus = SYDocumentStatus.OPEN.ToString(),
                    IntVDate = ObjMatch.IntVDate,
                    IntVStep = candidate.Max(w => w.IntVStep) + 1,
                    Strength = ObjMatch.Strength,
                    Weakness = ObjMatch.Weakness,
                    IntCmt = ObjMatch.IntCmt,
                    ProposedSalary = ObjMatch.ProposedSalary,
                    PositionOffer = ObjMatch.PositionOffer,
                    AttachFile = ObjMatch.AttachFile,
                    SalaryAfterProb = ObjMatch.SalaryAfterProb,
                    CandidateName = ObjMatch.CandidateName,
                    VacNo = ObjMatch.VacNo,
                    ApplyPost = ObjMatch.ApplyPost,
                    AppointmentDate = ObjMatch.AppointmentDate,
                    DocType = ObjMatch.DocType,
                    StartTime = ObjMatch.StartTime,
                    EndTime = ObjMatch.EndTime,
                    Location = ObjMatch.Location,
                    Remark = ObjMatch.Remark,
                    DocDate = ObjMatch.DocDate
                };
                unitOfWork.Add(_interview);
                Applica_.IntVStatus = SYDocumentStatus.OPEN.ToString();
                Applica_.IntvStep = Applica_.IntvStep + 1;
                unitOfWork.Update(Applica_);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
        }
    }
}