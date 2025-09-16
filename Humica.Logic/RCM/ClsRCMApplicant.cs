using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.CF;
using Humica.Logic.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.RCM
{
    public class ClsRCMApplicant : IClsRCMApplicant
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public RCMApplicant Header { get; set; }
        public RCMApplicant PersonalDate { get; set; }
        public RCMPInterview PIntV { get; set; }
        public List<RCMApplicant> ListHeader { get; set; }
        public List<RCMADependent> ListDependent { get; set; }
        public List<RCMAEdu> ListEdu { get; set; }
        public List<RCMALanguage> ListLang { get; set; }
        public List<RCMATraining> ListTraining { get; set; }
        public List<RCMAWorkHistory> ListWorkHistory { get; set; }
        public List<RCMAReference> ListRef { get; set; }
        public List<RCMAIdentity> ListIdentity { get; set; }
        public FTINYear Filter { get; set; }
        public Filters Filters { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public ClsEmail EmailObject { get; set; }
        public Filtering Filtering { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }

        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsRCMApplicant()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public string createApplicant(string savedFilePath, string URL)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(Header.VacNo)) return "EE_VAC";
                if (string.IsNullOrEmpty(Header.FirstName)) return "EE_FNAME";
                if (string.IsNullOrEmpty(Header.LastName)) return "EE_LNAME";
                if (Header.ApplyDate == null || Header.ApplyDate <= DateTime.MinValue) return "INV_APPLYDATE";
                var objCF = unitOfWork.Repository<ExCfWorkFlowItem>().Queryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                if (objCF == null)
                    return "REQUEST_TYPE_N";
                string CompanyCode = "";
                var staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == User.UserName);
                if (staff != null) CompanyCode = staff.CompanyCode;
                else
                {
                    var Com_ = unitOfWork.Repository<HRCompany>().Queryable().FirstOrDefault();
                    if (Com_ != null)
                        CompanyCode = Com_.Company;
                }
                var objNumber = new CFNumberRank(objCF.DocType, CompanyCode, Header.ApplyDate.Year, true);
                if (objNumber == null) return "NUMBER_RANK_NE";
                if (objNumber.NextNumberRank == null) return "NUMBER_RANK_NE";
                Header.ApplicantID = objNumber.NextNumberRank;
                if (ListDependent.Any())
                {
                    ListDependent.ToList().ForEach(h =>
                    {
                        h.ApplicantID = Header.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListDependent);
                }
                if (ListEdu.Any())
                {
                    ListEdu.ToList().ForEach(h =>
                    {
                        h.ApplicantID = Header.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListEdu);
                }

                if (ListLang.Any())
                {
                    ListLang.ToList().ForEach(h =>
                    {
                        h.ApplicantID = Header.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListLang);
                }
                if (ListTraining.Any())
                {
                    ListTraining.ToList().ForEach(h =>
                    {
                        h.ApplicantID = Header.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListTraining);
                }
                if (ListWorkHistory.Any())
                {
                    ListWorkHistory.ToList().ForEach(h =>
                    {
                        h.ApplicantID = Header.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListWorkHistory);
                }
                if (ListRef.Any())
                {
                    ListRef.ToList().ForEach(h =>
                    {
                        h.ApplicantID = Header.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListRef);
                }
                if (ListIdentity.Any())
                {
                    ListIdentity.ToList().ForEach(h =>
                    {
                        h.ApplicantID = Header.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListIdentity);
                }
                Header.AllName = Header.LastName + " " + Header.FirstName;
                Header.OthAllName = Header.OthLastName + " " + Header.OthFirstName;
                var FIRST_LASTNAME = SYSettings.getSetting("FIRST_LASTNAME");
                var FIRST_LASTNAME_KH = SYSettings.getSetting("FIRST_LASTNAME_KH");
                if (FIRST_LASTNAME != null)
                {
                    if (FIRST_LASTNAME.SettinValue == "TRUE")
                    {
                        Header.AllName = Header.FirstName + " " + Header.LastName;
                    }
                }
                if (FIRST_LASTNAME_KH != null)
                {
                    if (FIRST_LASTNAME_KH.SettinValue == "TRUE")
                        Header.OthAllName = Header.OthFirstName + " " + Header.OthLastName;
                }
                Header.Status = SYDocumentStatus.APPLY.ToString();
                Header.DocDate = DateTime.Now;
                Header.Salary = 0;
                Header.SalaryAfterProb = 0;
                Header.ProposedSalary = 0;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                Header.IsHired = false;
                Header.ShortList = SYDocumentStatus.OPEN.ToString();
                var updVac = unitOfWork.Repository<RCMVacancy>().Queryable().FirstOrDefault(w => w.Code == Header.VacNo);
                if (updVac != null)
                {
                    updVac.AppApplied += 1;
                    unitOfWork.Update(updVac);

                    Header.Sect = updVac.Sect;
                    Header.StaffType = updVac.StaffType;
                    Header.JobLevel = updVac.JobLevel;
                }
                unitOfWork.Add(Header);
                unitOfWork.Save();
                unitOfWork.Commit();

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
        public string updateApplicant(string ApplicantID)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var ObjMatch = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == ApplicantID);
                if (ObjMatch == null) return "DOC_INV";
                var chkLstDep = unitOfWork.Repository<RCMADependent>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                var chkLstEdu = unitOfWork.Repository<RCMAEdu>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                var chkLstLang = unitOfWork.Repository<RCMALanguage>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                var chkLstTrain = unitOfWork.Repository<RCMATraining>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                var chkLstWorkHistory = unitOfWork.Repository<RCMAWorkHistory>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                var chkLstRef = unitOfWork.Repository<RCMAReference>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                var chkdupLstIdent = unitOfWork.Repository<RCMAIdentity>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();

                if (chkLstDep.Any())
                    unitOfWork.BulkDelete(chkLstDep);
                if (chkLstEdu.Any())
                    unitOfWork.BulkDelete(chkLstEdu);
                if (chkLstLang.Any())
                    unitOfWork.BulkDelete(chkLstLang);
                if (chkLstTrain.Any())
                    unitOfWork.BulkDelete(chkLstTrain);
                if (chkLstWorkHistory.Any())
                    unitOfWork.BulkDelete(chkLstWorkHistory);
                if (chkLstRef.Any())
                    unitOfWork.BulkDelete(chkLstRef);
                if (chkdupLstIdent.Any())
                    unitOfWork.BulkDelete(chkdupLstIdent);

                if (ListDependent.Any())
                {
                    ListDependent.ToList().ForEach(h =>
                    {
                        h.ApplicantID = ObjMatch.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListDependent);
                }
                if (ListEdu.Any())
                {
                    ListEdu.ToList().ForEach(h =>
                    {
                        h.ApplicantID = ObjMatch.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListEdu);
                }

                if (ListLang.Any())
                {
                    ListLang.ToList().ForEach(h =>
                    {
                        h.ApplicantID = ObjMatch.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListLang);
                }
                if (ListTraining.Any())
                {
                    ListTraining.ToList().ForEach(h =>
                    {
                        h.ApplicantID = ObjMatch.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListTraining);
                }
                if (ListWorkHistory.Any())
                {
                    ListWorkHistory.ToList().ForEach(h =>
                    {
                        h.ApplicantID = ObjMatch.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListWorkHistory);
                }
                if (ListRef.Any())
                {
                    ListRef.ToList().ForEach(h =>
                    {
                        h.ApplicantID = ObjMatch.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListRef);
                }
                if (ListIdentity.Any())
                {
                    ListIdentity.ToList().ForEach(h =>
                    {
                        h.ApplicantID = ObjMatch.ApplicantID;
                    });
                    unitOfWork.BulkInsert(ListIdentity);
                }
                if (ObjMatch.VacNo != Header.VacNo)
                {
                    var updVac = unitOfWork.Repository<RCMVacancy>().Queryable().FirstOrDefault(w => w.Code == ObjMatch.VacNo);
                    if (updVac != null)
                    {
                        updVac.AppApplied -= 1;
                        unitOfWork.Update(updVac);
                    }
                    var updCurVac = unitOfWork.Repository<RCMVacancy>().Queryable().FirstOrDefault(w => w.Code == Header.VacNo);
                    if (updCurVac != null)
                    {
                        updCurVac.AppApplied += 1;
                        unitOfWork.Update(updCurVac);
                    }
                }
                ObjMatch.AllName = Header.LastName + " " + Header.FirstName;
                ObjMatch.OthAllName = Header.OthLastName + " " + Header.OthFirstName;
                var FIRST_LASTNAME = SYSettings.getSetting("FIRST_LASTNAME");
                var FIRST_LASTNAME_KH = SYSettings.getSetting("FIRST_LASTNAME_KH");
                if (FIRST_LASTNAME != null)
                {
                    if (FIRST_LASTNAME.SettinValue == "TRUE")
                    {
                        ObjMatch.AllName = Header.FirstName + " " + Header.LastName;
                    }
                }
                if (FIRST_LASTNAME_KH != null)
                {
                    if (FIRST_LASTNAME_KH.SettinValue == "TRUE")
                        ObjMatch.OthAllName = Header.OthFirstName + " " + Header.OthLastName;
                }

                ObjMatch.CurStage = "APPLY";
                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;
                ObjMatch.VacNo = Header.VacNo;
                ObjMatch.ApplyDate = Header.ApplyDate;
                ObjMatch.ApplyBranch = Header.ApplyBranch;
                ObjMatch.FirstName = Header.FirstName;
                ObjMatch.LastName = Header.LastName;
                ObjMatch.ApplyPosition = Header.ApplyPosition;
                ObjMatch.ApplyDept = Header.ApplyDept;
                ObjMatch.OthFirstName = Header.OthFirstName;
                ObjMatch.OthLastName = Header.OthLastName;

                ObjMatch.ExpectSalary = Header.ExpectSalary;
                ObjMatch.Gender = Header.Gender;
                ObjMatch.DOB = Header.DOB;
                ObjMatch.Email = Header.Email;
                ObjMatch.Phone1 = Header.Phone1;
                ObjMatch.Source = Header.Source;
                ObjMatch.CurAddr = Header.CurAddr;
                ObjMatch.PermanentAddr = Header.PermanentAddr;
                ObjMatch.Remark = Header.Remark;
                if (!string.IsNullOrEmpty(Header.ResumeFile))
                    ObjMatch.ResumeFile = Header.ResumeFile;
                ObjMatch.Nationality = Header.Nationality;
                unitOfWork.Update(ObjMatch);
                unitOfWork.Save();
                unitOfWork.Commit();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string deleteApplicant(string ApplicantID)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var ObjMatch = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == ApplicantID);
                if (ObjMatch == null) return "DOC_INV";
                string Pass = SYDocumentStatus.PASS.ToString();
                if (ObjMatch.Status == Pass) return "EE_APPCHK";
                var PInterview = unitOfWork.Repository<RCMPInterview>().Queryable().FirstOrDefault(w => w.ApplicantID == ApplicantID);
                if (PInterview != null) return "EE_APPCHK";
                var chkLstDep = unitOfWork.Repository<RCMADependent>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                var chkLstEdu = unitOfWork.Repository<RCMAEdu>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                var chkLstLang = unitOfWork.Repository<RCMALanguage>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                var chkLstTrain = unitOfWork.Repository<RCMATraining>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                var chkLstWorkHistory = unitOfWork.Repository<RCMAWorkHistory>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                var chkLstRef = unitOfWork.Repository<RCMAReference>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                var chkdupLstIdent = unitOfWork.Repository<RCMAIdentity>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                if (chkLstDep.Any())
                    unitOfWork.BulkDelete(chkLstDep);
                if (chkLstEdu.Any())
                    unitOfWork.BulkDelete(chkLstEdu);
                if (chkLstLang.Any())
                    unitOfWork.BulkDelete(chkLstLang);
                if (chkLstTrain.Any())
                    unitOfWork.BulkDelete(chkLstTrain);
                if (chkLstWorkHistory.Any())
                    unitOfWork.BulkDelete(chkLstWorkHistory);
                if (chkLstRef.Any())
                    unitOfWork.BulkDelete(chkLstRef);
                if (chkdupLstIdent.Any())
                    unitOfWork.BulkDelete(chkdupLstIdent);
                var updVac = unitOfWork.Repository<RCMVacancy>().Queryable().FirstOrDefault(w => w.Code == Header.VacNo);
                if (updVac != null)
                {
                    updVac.AppApplied -= 1;
                    unitOfWork.Update(updVac);
                }
                unitOfWork.Delete(ObjMatch);
                unitOfWork.Save();
                unitOfWork.Commit();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string upload()
        {
            OnLoad();
            string ApplicantID = "";
            try
            {
                if (!ListHeader.Any())
                    return "NO_DATA";
                var applicants = unitOfWork.Repository<RCMApplicant>().Queryable().ToList();
                var applicantLookup = applicants.ToDictionary(a => a.ApplicantID);
                var currentDate = DateTime.Now;
                foreach (var staff in ListHeader)
                {
                    ApplicantID = staff.ApplicantID;
                    var header = new RCMApplicant
                    {
                        VacNo = staff.VacNo,
                        ApplicantID = staff.ApplicantID,
                        FirstName = staff.FirstName,
                        LastName = staff.LastName,
                        OthFirstName = staff.OthFirstName,
                        OthLastName = staff.OthLastName,
                        Gender = staff.Gender,
                        Title = staff.Title,
                        ShortList = SYDocumentStatus.OPEN.ToString(),
                        Marital = staff.Marital,
                        DOB = staff.DOB,
                        ApplyBranch = staff.ApplyBranch,
                        ApplyPosition = staff.ApplyPosition,
                        WorkingType = staff.WorkingType,
                        ApplyDate = staff.ApplyDate,
                        ExpectSalary = staff.ExpectSalary,
                        Phone1 = staff.Phone1,
                        Email = staff.Email,
                        ResumeFile = null,
                        CreatedBy = User.UserName,
                        CreatedOn = currentDate
                    };
                    if (applicantLookup.TryGetValue(staff.ApplicantID, out var existingApplicant))
                    {
                        header.ApplicantID = existingApplicant.ApplicantID;
                    }
                    SetFullNames(header);
                    unitOfWork.Add(header);
                }
                unitOfWork.Save();
                return SYConstant.OK;
            }
            #region close
            //try
            //{
            //    if (!ListHeader.Any())
            //        return "NO_DATA";
            //    var _list = new List<RCMApplicant>();
            //    List<RCMApplicant> _listStaff = new List<RCMApplicant>();
            //    var Employee = unitOfWork.Repository<RCMApplicant>().Queryable();
            //    _listStaff = Employee.ToList();
            //    var date = DateTime.Now;
            //    foreach (var staffs in ListHeader.ToList())
            //    {
            //        Header = new RCMApplicant();
            //        var EmpCode = _listStaff.Where(w => w.ApplicantID == staffs.ApplicantID).ToList();
            //        Header.ApplicantID = "";
            //        if (EmpCode.Count <= 1)
            //        {
            //            if (EmpCode.Count == 1)
            //            {
            //                Header.ApplicantID = EmpCode.FirstOrDefault().ApplicantID;
            //            }
            //            Header.VacNo = staffs.VacNo;
            //            Header.ApplicantID = staffs.ApplicantID;
            //            Header.FirstName = staffs.FirstName;
            //            Header.LastName = staffs.LastName;
            //            Header.OthFirstName = staffs.OthFirstName;
            //            Header.OthLastName = staffs.OthLastName;

            //            Header.AllName = Header.LastName + " " + Header.FirstName;
            //            Header.OthAllName = Header.OthLastName + " " + Header.OthFirstName;
            //            var FIRST_LASTNAME = SYSettings.getSetting("FIRST_LASTNAME");
            //            var FIRST_LASTNAME_KH = SYSettings.getSetting("FIRST_LASTNAME_KH");
            //            if (FIRST_LASTNAME != null)
            //            {
            //                if (FIRST_LASTNAME.SettinValue == "TRUE")
            //                {
            //                    Header.AllName = Header.FirstName + " " + Header.LastName;
            //                }
            //            }
            //            if (FIRST_LASTNAME_KH != null)
            //            {
            //                if (FIRST_LASTNAME_KH.SettinValue == "TRUE")
            //                    Header.OthAllName = Header.OthFirstName + " " + Header.OthLastName;
            //            }
            //            Header.Gender = staffs.Gender;
            //            Header.Title = staffs.Title;
            //            Header.ShortList = SYDocumentStatus.OPEN.ToString();
            //            Header.Marital = staffs.Marital;
            //            Header.DOB = staffs.DOB;
            //            Header.ApplyBranch = staffs.ApplyBranch;
            //            Header.ApplyPosition = staffs.ApplyPosition;
            //            Header.WorkingType = staffs.WorkingType;
            //            Header.ApplyDate = staffs.ApplyDate;
            //            Header.ExpectSalary = staffs.ExpectSalary;
            //            Header.Phone1 = staffs.Phone1;
            //            Header.Email = staffs.Email;
            //            Header.ResumeFile = null;
            //            Header.CreatedBy = User.UserName;
            //            Header.CreatedOn = DateTime.Now;
            //            unitOfWork.Add(Header);
            //        }
            //    }
            //    unitOfWork.Save();
            //    return SYConstant.OK;
            //}
            #endregion
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        private void SetFullNames(RCMApplicant header)
        {
            var firstLastNameSetting = SYSettings.getSetting("FIRST_LASTNAME");
            var firstLastNameKhSetting = SYSettings.getSetting("FIRST_LASTNAME_KH");
            header.AllName = $"{header.LastName} {header.FirstName}";
            header.OthAllName = $"{header.OthLastName} {header.OthFirstName}";
            if (firstLastNameSetting?.SettinValue == "TRUE")
                header.AllName = $"{header.FirstName} {header.LastName}";
            if (firstLastNameKhSetting?.SettinValue == "TRUE")
                header.OthAllName = $"{header.OthFirstName} {header.OthLastName}";
        }
        public string SENTCV(string ApplicantID, string URL)
        {
            string Applic_ = "";
            try
            {
                string[] IDs = ApplicantID.Split(';');
                List<string> filePaths = new List<string>();
                HashSet<string> positions = new HashSet<string>();
                string VacNo = "";
                foreach (var read in IDs)
                {
                    Applic_ = read;
                    var ObjMatch = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == read);
                    if (ObjMatch == null || ObjMatch.TestStatus == "SENT" || string.IsNullOrEmpty(ObjMatch.ResumeFile)) continue;

                    positions.Add(ObjMatch.ApplyPosition);
                    filePaths.Add(System.Web.Hosting.HostingEnvironment.MapPath(ObjMatch.ResumeFile));
                    VacNo = ObjMatch.VacNo;
                    ObjMatch.TestStatus = "SENT";
                    unitOfWork.Update(ObjMatch);
                }

                var updVac = unitOfWork.Repository<RCMVacancy>().Queryable().FirstOrDefault(w => w.Code == VacNo);
                if (updVac == null) return "INVALID_VACANCY";

                var request = unitOfWork.Repository<RCMRecruitRequest>().Queryable().FirstOrDefault(w => w.RequestNo == updVac.DocRef);
                ApplicantEmail(request.RequestedBy, Applic_, URL, filePaths, positions.ToList());
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ApplicantID, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public void ApplicantEmail(string RequestedBy, string Applic_, string URL, List<string> filePaths, List<string> positions)
        {
            var sendEmail = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == RequestedBy);
            var createdBy = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == User.UserName);
            string applyPost = string.Join(", ", positions.Select(pos =>
            {
                var position = unitOfWork.Repository<HRPosition>().Queryable().FirstOrDefault(w => w.Code == pos);
                return position?.Description;
            }).Where(desc => !string.IsNullOrEmpty(desc)));

            if (sendEmail != null)
            {
                URL += "?ApplicantID=" + Applic_;

                var emailConf = unitOfWork.Repository<CFEmailAccount>().Queryable().FirstOrDefault();
                if (emailConf != null)
                {
                    var creatorName = createdBy?.AllName ?? User.UserName;
                    var phone = createdBy?.Phone1 ?? string.Empty;
                    var email = createdBy?.Email ?? User.Email;

                    var subject = "Applicant";
                    var description = $@"
                Dear {sendEmail.Title} <b>{sendEmail.AllName}</b><br /><br />
                Here are the CVs for the positions <b>{applyPost}</b> for your review.<br /><br />
                <b>Best regards,</b><br /><br />
                {creatorName}<br />
                H/P: {phone}<br />
                Email: {email}<br /><br />
                Please login at <a href='{URL}'>URL</a>
            ";

                    // Pass multiple files
                    var emailObject = new ClsEmail();
                    int result = emailObject.SendMails(emailConf, "", sendEmail.Email, subject, description, filePaths.ToArray());
                }
            }
        }
    }
    public class Filtering
    {
        public string Vacancy { get; set; }
        public string Branch { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
