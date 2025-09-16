using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.RCM
{
    public class ClsRCMVacancy : IClsRCMVacancy
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string JobResponsibility { get; set; }
        public string JobRequirement { get; set; }
        public string PostOfJD { get; set; }
        public string MessageError { get; set; }
        public RCMVacancy Header { get; set; }
        public RCMRecruitRequest RecruitRequest { get; set; }
        public RCMApplicant Applicants { get; set; }
        public List<RCMVacancy> ListHeader { get; set; }
        public List<RCMRecruitRequest> ListPending { get; set; }
        public RCMVInterviewer VInt { get; set; }
        public List<RCMVInterviewer> ListInt { get; set; }
        public List<RCMAdvertising> ListAdvertise { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }

        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsRCMVacancy()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }

        #region 'Create'
        public string createVAC()
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                if (Header.VacancyType == null) return "VACTYPE_EN";
                var objCF = unitOfWork.Repository<ExCfWorkFlowItem>().Queryable().FirstOrDefault(w => w.ScreenID == ScreenId);
                if (objCF == null)
                    return "REQUEST_TYPE_N";
                DateTime currentdate = DateTime.Now;
                string CompanyCode = "";
                var staff = unitOfWork.Repository<HRStaffProfile>().Queryable().FirstOrDefault(w => w.EmpCode == User.UserName);
                if (staff != null) CompanyCode = staff.CompanyCode;
                else
                {
                    var Com_ = unitOfWork.Repository<HRCompany>().Queryable().FirstOrDefault();
                    if (Com_ != null)
                        CompanyCode = Com_.Company;
                }
                var objNumber = new CFNumberRank(objCF.DocType, CompanyCode, currentdate.Year, true);
                if (objNumber == null) return "NUMBER_RANK_NE";
                if (objNumber.NextNumberRank == null) return "NUMBER_RANK_NE";

                Header.Code = objNumber.NextNumberRank;
                var _ReqForm = unitOfWork.Repository<RCMRecruitRequest>().Queryable().FirstOrDefault(w => w.RequestNo == Header.DocRef);
                if (_ReqForm != null)
                {
                    Header.Position = _ReqForm.POST;
                    Header.Sect = _ReqForm.Sect;
                    Header.Branch = _ReqForm.Branch;
                    Header.Dept = _ReqForm.DEPT;
                    Header.StaffType = _ReqForm.StaffType;
                    Header.WorkingType = _ReqForm.WorkingType;
                    Header.JobLevel = _ReqForm.JobLevel;
                }
                if (ListInt.Any())
                {
                    ListInt.ToList().ForEach(h =>
                    {
                        h.Code = Header.Code;
                        h.Position = Header.Position;
                    });
                    unitOfWork.BulkInsert(ListInt);
                }
                if (ListAdvertise.Any())
                {
                    ListAdvertise.ToList().ForEach(h =>
                    {
                        h.VacNo = Header.Code;
                    });
                    unitOfWork.BulkInsert(ListAdvertise);
                }
                Header.AppApplied = 0;
                Header.DocDate = DateTime.Now;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;

                unitOfWork.Add(Header);
                unitOfWork.Save();
                unitOfWork.Commit();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.VacancyType, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.VacancyType, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.VacancyType, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        #endregion 
        #region 'Update'
        public string updateVAC(string Code)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var ObjMatch = unitOfWork.Repository<RCMVacancy>().Queryable().FirstOrDefault(w => w.Code == Code);
                if (ObjMatch == null) return "DOC_INV";

                var checkdupListIntv = unitOfWork.Repository<RCMVInterviewer>().Queryable().Where(w => w.Code == Code).ToList();
                var checkdupListAds = unitOfWork.Repository<RCMAdvertising>().Queryable().Where(w => w.VacNo == Code).ToList();
                if (checkdupListIntv.Any())
                    unitOfWork.BulkDelete(checkdupListIntv);
                if (checkdupListAds.Any())
                    unitOfWork.BulkDelete(checkdupListAds);

                if (ListInt.Any())
                {
                    ListInt.ToList().ForEach(h =>
                    {
                        h.Code = Code;
                        h.Position = ObjMatch.Position;
                    });
                    unitOfWork.BulkInsert(ListInt);
                }
                if (ListAdvertise.Any())
                {
                    ListAdvertise.ToList().ForEach(h =>
                    {
                        h.VacNo = Code;
                    });
                    unitOfWork.BulkInsert(ListAdvertise);
                }
                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;
                ObjMatch.VacancyType = Header.VacancyType;
                ObjMatch.Description = Header.Description;
                ObjMatch.ClosedDate = Header.ClosedDate;

                unitOfWork.Update(ObjMatch);

                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        #endregion
        #region 'Delete'
        public string deleteVAC(string Code)
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                var ObjMatch = unitOfWork.Repository<RCMVacancy>().Queryable().FirstOrDefault(w => w.Code == Code);
                var _chkApplicant = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.VacNo == Code);

                string Process = SYDocumentStatus.PROCESSING.ToString();

                if (ObjMatch.Status == Process) return "DOC_INV";
                if (_chkApplicant != null) return "EE_APPCHK";

                var checkdupListInt = unitOfWork.Repository<RCMVInterviewer>().Queryable().Where(w => w.Code == Code).ToList();
                var checkdupListAds = unitOfWork.Repository<RCMAdvertising>().Queryable().Where(w => w.VacNo == Code).ToList();
                if (checkdupListInt.Any())
                    unitOfWork.BulkDelete(checkdupListInt);
                if (checkdupListAds.Any())
                    unitOfWork.BulkDelete(checkdupListAds);

                unitOfWork.Delete(ObjMatch);
                unitOfWork.Save();
                unitOfWork.Commit();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        #endregion
        #region 'Convert Status'
        public string Processing(string Code)
        {
            OnLoad();
            try
            {
                var objmatch = unitOfWork.Repository<RCMVacancy>().Queryable().FirstOrDefault(w => w.Code == Code);
                if (objmatch == null) return "DOC_INV";
                objmatch.Status = SYDocumentStatus.PROCESSING.ToString();
                objmatch.ProcessBy = User.UserName;
                objmatch.ProcessDate = DateTime.Now;
                unitOfWork.Update(objmatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Closed(string Code)
        {
            OnLoad();
            try
            {
                var objmatch = unitOfWork.Repository<RCMVacancy>().Queryable().FirstOrDefault(w => w.Code == Code);
                if (objmatch == null) return "DOC_INV";
                objmatch.Status = SYDocumentStatus.CLOSED.ToString();
                objmatch.ClosedDate = DateTime.Now;
                objmatch.ClosedBy = User.UserName;

                unitOfWork.Update(objmatch);
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Completed(string Code)
        {
            OnLoad();
            try
            {
                var objmatch = unitOfWork.Repository<RCMVacancy>().Queryable().FirstOrDefault(w => w.Code == Code);

                if (objmatch == null)
                    return "DOC_INV";

                objmatch.Status = SYDocumentStatus.COMPLETED.ToString();
                objmatch.ChangedBy = User.UserName;
                objmatch.ChangedOn = DateTime.Now;

                unitOfWork.Update(objmatch);
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        #endregion 
    }
}
