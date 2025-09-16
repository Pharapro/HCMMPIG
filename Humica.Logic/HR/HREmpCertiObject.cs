using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Humica.Logic.HR
{
    public class HREmpCertiObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public HREmpCertificate Header { get; set; }
        public HRStaffProfile Staff { get; set; }
        public HRCertificationType EduType { get; set; }
        public string CompanyCode { get; set; }
        public string Plant { get; set; }
        public string MessageCode { get; set; }
        public List<HRStaffProfile> ListStaff { get; set; }
        public List<HRCertificationType> ListEduType { get; set; }
        public List<HREmpCertificate> ListHeader { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<HR_STAFF_VIEW> ListStaffView { get; set; }

        public HREmpCertiObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public async Task<List<HREmpCertificate>> LoadData()
        {
            List<HRStaffProfile> _listTemp = new List<HRStaffProfile>();
            List<HRCompany> _Listcomapay = SYConstant.getCompanyDataAccess();
            List<HRBranch> _ListBranch = SYConstant.getBranchDataAccess();
            List<HRLevel> _ListLevel = SYConstant.getLevelDataAccess();
            var branchids = _ListBranch.Select(s => s.Code).ToList();

            _listTemp = await new HumicaDBContext().HRStaffProfiles.Where(w => branchids.Contains(w.Branch)).ToListAsync();
            _listTemp = _listTemp.Where(w => _Listcomapay.Where(x => x.Company == w.CompanyCode).Any()).ToList();
            _listTemp = _listTemp.Where(w => _ListLevel.Where(x => x.Code == w.LevelCode).Any()).ToList();

            ListHeader = await new HumicaDBContext().HREmpCertificates.ToListAsync();
            ListHeader = ListHeader.Where(w => _listTemp.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            return ListHeader;
        }
        public string CreateCertificate()
        {
            try
            {
                HumicaDBContext DBM = new HumicaDBContext();

                if (HeaderStaff.EmpCode == null) return "EMPCODE_EN";
                if (Header.CertType == null) return "CERTIFICATE_EN";

                var obj = DB.HRCertificationTypes.Where(w => w.Code == Header.CertType);
                if (obj == null)
                {
                    return "CERTIFICATE_TYPE";
                }
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.CertDesc = obj.FirstOrDefault().Description;
                Header.AllName = HeaderStaff.AllName;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;

                DB.HREmpCertificates.Add(Header);
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
        public string EditCertificate(int id)
        {
            try
            {
                Header.EmpCode = HeaderStaff.EmpCode;
                HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = DB.HREmpCertificates.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "CERTIFICATE_NE";
                }
                var obj = DB.HRCertificationTypes.Where(w => w.Code == Header.CertType);
                if (obj == null)
                {
                    return "CERTIFICATE_TYPE";
                }

                objMatch.CertDesc = obj.FirstOrDefault().Description;
                objMatch.CertType = Header.CertType;
                objMatch.IssueDate = Header.IssueDate;
                objMatch.Remark = Header.Remark;
                objMatch.Description = Header.Description;
                objMatch.AttachFile = Header.AttachFile;
                objMatch.ChangedBy = Header.ChangedBy;
                objMatch.ChangedOn = Header.ChangedOn;

                DB.HREmpCertificates.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.CertType).IsModified = true;
                DB.Entry(objMatch).Property(w => w.CertDesc).IsModified = true;
                DB.Entry(objMatch).Property(w => w.IssueDate).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Remark).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Description).IsModified = true;
                DB.Entry(objMatch).Property(w => w.AttachFile).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderStaff.EmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteCertificate(int id)
        {
            try
            {
                Header = new HREmpCertificate();
                var objMatch = DB.HREmpCertificates.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "CERTIFICATE_NE";
                }
                Header.TranNo = id;
                DB.HREmpCertificates.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Deleted;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.TranNo.ToString();
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
                log.ScreenId = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}