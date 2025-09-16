using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace Humica.Logic.RCM
{
    public class ClsRCMCheckList : IClsRCMCheckList
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ApplyPosition { get; set; }
        public string DocType { get; set; }
        public string ApplicantName { get; set; }
        public string ApplyBranch { get; set; }
        public string Gender { get; set; }
        public string ApplyDept { get; set; }
        public string WorkType { get; set; }
        public string VacNo { get; set; }
        public int? IntVStep { get; set; }
        public decimal ExpectSalary { get; set; }
        public RCMApplicant App { get; set; }
        public RCMPInterview Header { get; set; }
        public RCMIntVQuestionnaire IntVQ { get; set; }
        public List<RCMApplicant> ListCandidate { get; set; }
        public List<RCMPInterview> ListInt { get; set; }
        public List<RCMIntVQuestionnaire> ListQuestionnair { get; set; }
        public List<RCMVInterviewer> ListInterViewer { get; set; }
        public FilterCandidate Filtering { get; set; }
        public string ScreenId { get; set; }
        public string ErrorMessage { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsRCMCheckList()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }

        #region 'Create'
        public string createJobIntV()
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                string Open = SYDocumentStatus.OPEN.ToString();
                var _Interview = unitOfWork.Repository<RCMPInterview>().Queryable().ToList();
                var _chkdup = _Interview.FirstOrDefault(w => w.IntVStep == IntVStep && w.ApplicantID == Header.ApplicantID);
                if (_chkdup != null)
                    if (_chkdup.Status == Open) return "Candidate have been checklist!";

                var _Interviewer = unitOfWork.Repository<RCMVInterviewer>().Queryable().Where(w => w.IntStep == IntVStep && w.Code == VacNo).ToList();
                if (_Interviewer.Any())
                    unitOfWork.BulkDelete(_Interviewer);
                if (ListQuestionnair.Any())
                {
                    ListQuestionnair.ToList().ForEach(h =>
                    {
                        h.ApplicantID = VacNo;
                        h.IntVStep = Convert.ToInt32(IntVStep);
                    });
                    unitOfWork.BulkInsert(ListQuestionnair);
                }
                if (ListInterViewer.Any())
                {
                    ListInterViewer.ToList().ForEach(h =>
                    {
                        h.Code = Header.ApplicantID;
                        h.Position = Header.ApplyPost;
                    });
                    unitOfWork.BulkInsert(ListInterViewer);
                }
                var _App = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == Header.ApplicantID);
                if (_App != null)
                {
                    Header.DocType = _App.StaffType;
                    _App.IntVStatus = "Interview";
                    unitOfWork.Update(_App);
                }
                Header.VacNo = VacNo;
                Header.DocDate = DateTime.Now;
                Header.Status = SYDocumentStatus.OPEN.ToString();
                Header.ReStatus = SYDocumentStatus.OPEN.ToString();
                Header.IntVStep = Convert.ToInt32(IntVStep);

                unitOfWork.Add(Header);
                unitOfWork.Save();
                unitOfWork.Commit();

                #region Email
                //SYEmail SE = new SYEmail();
                //var EmailConf = DP.SYEmailConfs.Where(w => w.ProjectID == "1").ToList();
                //var StageAssigntoEmail = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == Header.StageAssignTo);
                //string Title = "";
                //if (EmailConf.Count > 0)
                //{
                //    if (Header.AlertToInterviewer == "EM" && StageAssigntoEmail!=null)
                //    {
                //        var dt = Header.AppointmentDate;
                //        SE.MailFrom = EmailConf.First().EmailAddress;
                //        SE.SmtpPassword = Convert.ToString(EmailConf.First().SmtpPassword);
                //        SE.SmtpPort = Convert.ToString(EmailConf.First().SmtpPort);
                //        SE.SmtpHost = Convert.ToString(EmailConf.First().SmtpAddress);
                //        SE.Subject = "Interview Candidate";
                //        Title += "<b>" + StageAssigntoEmail.Title + "</b>";
                //        string str = "Dear " + Title + "<b>"+ StageAssigntoEmail.AllName+"</b>";
                //        str += "<br /> <p>I would like to inform you that you have to interview a candidate on " +"<b>" + Header.AppointmentDate.ToString() + " " + Header.SetTime + "</b>";
                //        str += "<br /> At " + Header.Location ;
                //        str += "<br /><br />";
                //        str += "Thank you !";
                //        SE.Body = str;

                //        var MailTo = StageAssigntoEmail;

                //        if (MailTo != null)
                //        {
                //            SE.MailTo = MailTo.Email;

                //            string MailCC = "";
                //            if (MailTo.Email != null)
                //            {
                //                MailCC += User.Email + ";";
                //            }
                //            SE.MailCC = MailCC;
                //            int re = SE.SendEmail();
                //        }
                //    }
                //}

                #endregion
                return SYConstant.OK;

            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        #endregion 
        public string updateCheckList(string TranNo)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                int Tran = Convert.ToInt32(TranNo);
                var ObjMatch = unitOfWork.Repository<RCMPInterview>().Queryable().FirstOrDefault(w => w.TranNo == Tran);
                if (ObjMatch == null) return "DOC_INV";
                //if (ObjMatch.Status == "PASS" || ObjMatch.Status == "FAIL") return "DOC_INV";
                if (ObjMatch.Status == "PASS") return "DOC_INV";
                if (ObjMatch.Status == "NEXTSTEP") return "DOC_INV";

                var _ListQuest = unitOfWork.Repository<RCMIntVQuestionnaire>().Queryable().Where(w => w.ApplicantID == ObjMatch.ApplicantID && w.IntVStep == ObjMatch.IntVStep).ToList();
                if (_ListQuest.Any())
                    unitOfWork.BulkDelete(_ListQuest);

                if (ListQuestionnair.Any())
                {
                    ListQuestionnair.ToList().ForEach(h =>
                    {
                        h.ApplicantID = ObjMatch.ApplicantID;
                        h.IntVStep = ObjMatch.IntVStep;
                    });
                    unitOfWork.BulkInsert(ListQuestionnair);
                }
                var Interviewer = unitOfWork.Repository<RCMVInterviewer>().Queryable().Where(w => w.IntStep == ObjMatch.IntVStep && w.Code == ObjMatch.VacNo).ToList();
                if (Interviewer.Any())
                    unitOfWork.BulkDelete(Interviewer);

                int Line = 0;
                var Interviewer_ = unitOfWork.Repository<RCMVInterviewer>().Queryable().Where(w => w.Code == ObjMatch.VacNo).OrderByDescending(w => w.LineItem).First();
                if (Interviewer_ != null)
                {
                    Line = Interviewer_.LineItem;
                }
                foreach (var read in ListInterViewer.ToList())
                {
                    Line = Line + 1;
                    var objNew = new RCMVInterviewer()
                    {
                        Code = ObjMatch.VacNo,
                        LineItem = Line,
                        IntStep = ObjMatch.IntVStep,
                        EmpCode = read.EmpCode,
                        EmpName = read.EmpName,
                        Remark = read.Remark,
                        Position = Header.ApplyPost,
                    };
                    unitOfWork.Add(objNew);
                }
                ObjMatch.Remark = Header.Remark;
                ObjMatch.AppointmentDate = Header.AppointmentDate;
                ObjMatch.StartTime = Header.StartTime;
                ObjMatch.EndTime = Header.EndTime;
                ObjMatch.Location = Header.Location;
                ObjMatch.AlertToInterviewer = Header.AlertToInterviewer;
                ObjMatch.Status = "OPEN";
                ObjMatch.ReStatus = "OPEN";
                unitOfWork.Update(ObjMatch);
                unitOfWork.Save();
                unitOfWork.Commit();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo, SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, TranNo, SYActionBehavior.EDIT.ToString(), e, true);
            }
        }
        public string ReleaseDoc(int ID)
        {
            try
            {
                var objMatch = unitOfWork.Repository<RCMPInterview>().Queryable().FirstOrDefault(w => w.TranNo == ID);
                if (objMatch == null)
                    return "REQUEST_NE";
                if (objMatch.ReStatus != SYDocumentStatus.OPEN.ToString())
                    return "INV_DOC";
                Header = objMatch;
                objMatch.ReStatus = SYDocumentStatus.PENDING.ToString();
                unitOfWork.Update(objMatch);
                unitOfWork.Save();
                #region Email
                var emailConfig = unitOfWork.Repository<CFEmailAccount>().Queryable().FirstOrDefault();
                var applicant = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == objMatch.ApplicantID);
                if (applicant == null) return "INV_DOC";
                var positionDescription = unitOfWork.Repository<HRPosition>().Queryable().FirstOrDefault(w => w.Code == applicant.ApplyPosition);
                if (emailConfig != null)
                {
                    var ccEmailList = new List<string> { User.Email };
                    var interviewers = unitOfWork.Repository<RCMVInterviewer>().Queryable()
                        .Where(w => w.Code == objMatch.VacNo && w.IntStep == objMatch.IntVStep).ToList();
                    foreach (var interviewer in interviewers)
                    {
                        var staffProfile = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == interviewer.EmpCode);
                        if (staffProfile != null)
                        {
                            ccEmailList.Add(staffProfile.Email);
                        }
                    }
                    var company = unitOfWork.Repository<SYHRCompany>().Queryable().FirstOrDefault();
                    string positionName = positionDescription?.Description ?? "";
                    var emailBody = new StringBuilder();
                    emailBody.Append($"Dear {applicant.Title} <b>{Header.CandidateName}</b>,");
                    emailBody.Append("<br /><br /> Kindly note the interview details:");
                    emailBody.Append($"<br /><br /> {Header.Location}");
                    emailBody.Append("<br /> Please reply to this email if you have any questions or need to reschedule. We look forward to speaking with you.");
                    emailBody.Append("<br /> Sincerely,<br />HR Department");
                    string subject = $"Interview Invitation for the position of {positionName} at {company?.CompENG}";
                    ClsEmail emailObject = new ClsEmail();
                    int result = emailObject.SendMail(emailConfig, applicant.Email, string.Join(";", ccEmailList), subject, emailBody.ToString(), "", "");
                }
                #endregion
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID.ToString(), SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID.ToString(), SYActionBehavior.EDIT.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID.ToString(), SYActionBehavior.EDIT.ToString(), e, true);
            }
        }
    }
    public class FilterCandidate
    {
        public string Vacancy { get; set; }
        public string ApplyPost { get; set; }
        public int IntVStep { get; set; }
    }
}