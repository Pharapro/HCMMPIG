using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.HR
{
    public class HREmpSupenseObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public HREmpSupense Header { get; set; }
        public HRStaffProfile Staff { get; set; }
        public HRContractType EduType { get; set; }
        public string CompanyCode { get; set; }
        public string Plant { get; set; }
        public string MessageCode { get; set; }
        public string MessageError { get; set; }
        public List<HRStaffProfile> ListStaff { get; set; }
        public List<HRContractType> ListEduType { get; set; }
        public List<HREmpSupense> ListHeader { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<HR_STAFF_VIEW> ListStaffView { get; set; }
        public HREmpSupenseObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<HREmpSupense> LoadData()
        {
            return ListHeader = DB.HREmpSupenses.ToList();
        }
        public string CreateSupense()
        {
            try
            {
                HumicaDBContext DBM = new HumicaDBContext();
                if (HeaderStaff.EmpCode == null) return "EMPCODE_EN";
                var Staff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                var Staff_ = DBV.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);
                Header.EmpName = Staff.AllName;
                Header.Department = Staff.Department;
                Header.Position = Staff.Position;
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                DB.HREmpSupenses.Add(Header);

                var OldCareer = DB.HREmpCareers.Where(w => w.EmpCode == Header.EmpCode).ToList();
                var _OldCareer = OldCareer.OrderByDescending(w => w.TranNo).FirstOrDefault();
                _OldCareer.ToDate = Header.FromDate.Value.AddDays(-1);
                _OldCareer.EffectDate = Header.FromDate.Value.AddDays(-1);
                DB.HREmpCareers.Attach(_OldCareer);
                DB.Entry(_OldCareer).State = System.Data.Entity.EntityState.Modified;
                int row = DB.SaveChanges();

                var HeaderCareer = new HREmpCareer();
                HeaderCareer.CareerCode = "SUSPENSE";
                HeaderCareer.EmpCode = Staff_.EmpCode;
                HeaderCareer.EmpType = Staff_.EmpType;
                HeaderCareer.Branch = Staff_.Branch;
                HeaderCareer.LOCT = Staff_.LOCT;
                HeaderCareer.Division = Staff_.Division;
                HeaderCareer.DEPT = Staff_.DEPT;
                HeaderCareer.SECT = Staff_.SECT;
                HeaderCareer.LevelCode = Staff_.LevelCode;
                HeaderCareer.JobGrade = Staff_.JobGrade;
                HeaderCareer.JobCode = Staff_.JobCode;
                HeaderCareer.OldSalary = Staff_.Salary;
                HeaderCareer.Increase = 0;
                HeaderCareer.NewSalary = Staff_.Salary;
                HeaderCareer.SupCode = Header.TranNo.ToString();
                HeaderCareer.FromDate = Header.FromDate;
                HeaderCareer.ToDate = new DateTime(5000, 01, 01);
                HeaderCareer.ProBation = Header.FromDate;
                HeaderCareer.EffectDate = Header.FromDate;
                HeaderCareer.StaffType = Staff_.StaffType;
                HeaderCareer.CreateBy = User.UserName;
                HeaderCareer.CreateOn = DateTime.Now;
                DB.HREmpCareers.Add(HeaderCareer);
                int row1 = DB.SaveChanges();


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
        public string EditSupense(int id)
        {
            try
            {
                Header.EmpCode = HeaderStaff.EmpCode;
                HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = DB.HREmpSupenses.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                var Staff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == objMatch.EmpCode);
                objMatch.EmpName = Staff.AllName;
                objMatch.Department = Staff.Department;
                objMatch.Position = Staff.Position;
                objMatch.FromDate = Header.FromDate;
                objMatch.ToDate = Header.ToDate;
                objMatch.Description = Header.Description;
                objMatch.AttachFile = Header.AttachFile;
                objMatch.ChangedBy = Header.ChangedBy;
                objMatch.ChangedOn = Header.ChangedOn;

                DB.HREmpSupenses.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Modified;
                string ID = objMatch.TranNo.ToString();
                var ObjCareerUpdate = DB.HREmpCareers.Where(w => w.SupCode == ID).FirstOrDefault();
                ObjCareerUpdate.FromDate = Header.FromDate;
                DB.HREmpCareers.Attach(ObjCareerUpdate);
                DB.Entry(ObjCareerUpdate).State = System.Data.Entity.EntityState.Modified;
                int row = DB.SaveChanges();

                var OldCareer = DB.HREmpCareers.Where(w => w.EmpCode == Header.EmpCode).ToList();
                var _OldCareer = OldCareer.OrderByDescending(w => w.TranNo).Skip(1).FirstOrDefault();
                _OldCareer.ToDate = Header.FromDate.Value.AddDays(-1);
                _OldCareer.EffectDate = Header.FromDate.Value.AddDays(-1);
                DB.HREmpCareers.Attach(_OldCareer);
                DB.Entry(_OldCareer).State = System.Data.Entity.EntityState.Modified;
                int row1 = DB.SaveChanges();
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
        public string DeleteSupense(int id)
        {
            try
            {
                Header = new HREmpSupense();
                var objMatch = DB.HREmpSupenses.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                Header.TranNo = id;
                DB.HREmpSupenses.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Deleted;
                string ID = objMatch.TranNo.ToString();
                var Emp_C = DB.HREmpCareers.Where(w => w.SupCode == ID).FirstOrDefault();
                DB.HREmpCareers.Attach(Emp_C);
                DB.Entry(Emp_C).State = System.Data.Entity.EntityState.Deleted;
                int row = DB.SaveChanges();


                var OldCareer = DB.HREmpCareers.Where(w => w.EmpCode == objMatch.EmpCode).ToList();
                var _OldCareer = OldCareer.OrderByDescending(w => w.TranNo).FirstOrDefault();
                _OldCareer.ToDate = new DateTime(5000, 01, 01);
                _OldCareer.EffectDate = _OldCareer.FromDate;
                DB.HREmpCareers.Attach(_OldCareer);
                DB.Entry(_OldCareer).State = System.Data.Entity.EntityState.Modified;
                int row1 = DB.SaveChanges();

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