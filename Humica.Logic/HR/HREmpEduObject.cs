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
    public class HREmpEduObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public HREmpEduc Header { get; set; }
        public HRStaffProfile Staff { get; set; }
        public HREduType EduType { get; set; }
        public string CompanyCode { get; set; }
        public string Plant { get; set; }
        public string MessageCode { get; set; }
        public List<HRStaffProfile> ListStaff { get; set; }
        public List<HREduType> ListEduType { get; set; }
        public List<HREmpEduc> ListHeader { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<HR_STAFF_VIEW> ListStaffView { get; set; }
        public List<EmpEducation> ListEducation { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public List<HREmpEduc> ListImport { get; set; }
        public HREmpEduObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<EmpEducation> LoadData()
        {
            var AccLevel = SMS.SYUserLevels.ToList();
            AccLevel = AccLevel.Where(w => w.UserName == User.UserName).ToList();

            var _list = new List<EmpEducation>();
            var empEdu = DB.HREmpEducs.ToList();
            var Staff = DB.HRStaffProfiles.ToList();
            Staff = Staff.Where(w => AccLevel.Where(x => x.LevelCode == w.LevelCode).Any()).ToList();
            var EduType = DB.HREduTypes.ToList();
            var Dept = DB.HRDepartments.ToList();
            var resu = (from edu in empEdu
                        join emp in Staff on edu.EmpCode equals emp.EmpCode
                        join ET in EduType on edu.EduType equals ET.Code
                        join D in Dept on emp.DEPT equals D.Code into DeptGroup
                        from D in DeptGroup.DefaultIfEmpty()  // Left join handling
                        select new
                        {
                            edu.TranNo,
                            edu.EmpCode,
                            emp.AllName,
                            Department = D != null ? D.Description : null,  // Handle null in left join
                            EduType = ET.Description,
                            edu.StartDate,
                            edu.EndDate,
                            edu.EdcCenter,
                            edu.Major,
                            edu.Result,
                            edu.Remark
                        }).ToList();

            foreach (var item in resu)
            {
                var empedu = new EmpEducation();
                empedu.TranNo = item.TranNo;
                empedu.EmpCode = item.EmpCode;
                empedu.AllName = item.AllName;
                empedu.EduType = item.EduType;
                empedu.Department = item.Department;
                if (item.StartDate.HasValue)
                    empedu.StartDate = item.StartDate.Value;
                if (item.EndDate.HasValue)
                    empedu.EndDate = item.EndDate.Value;
                empedu.EdcCenter = item.EdcCenter;
                empedu.Major = item.Major;
                empedu.Result = item.Result;
                empedu.Remark = item.Remark;
                _list.Add(empedu);
            }

            return _list;
        }
        public string upload()
        {

            if (ListImport.Count == 0)
            {
                return "NO_DATA";
            }
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                var _list = new List<HREmpEduc>();
                List<HRStaffProfile> _listStaff = new List<HRStaffProfile>();
                _listStaff = DB.HRStaffProfiles.ToList();
                List<HRDepartment> _listDept = new List<HRDepartment>();
                _listDept = DB.HRDepartments.ToList();
                MessageCode = "";
                var date = DateTime.Now.Date;
                foreach (var staffs in ListImport.ToList())
                {
                    MessageCode = staffs.EmpCode;
                    if (date == staffs.StartDate)
                        staffs.StartDate = null;
                    if (date == staffs.EndDate)
                        staffs.EndDate = null;
                    var Departcode = _listStaff.Where(w => w.EmpCode == staffs.EmpCode).FirstOrDefault();
                    if (Departcode == null)
                        return "NO_DATA";
                    var DeptDes = _listDept.Where(w => w.Code == Departcode.DEPT).FirstOrDefault();
                    if (DeptDes == null)
                        return "NO_DATA";
                    Header = new HREmpEduc();
                    Header.EmpCode = staffs.EmpCode;
                    Header.Department = DeptDes.Description;
                    Header.EduType = staffs.EduType;
                    Header.StartDate = staffs.StartDate;
                    Header.EndDate = staffs.EndDate;
                    Header.EdcCenter = staffs.EdcCenter;
                    Header.Major = staffs.Major;
                    Header.EmpCode = staffs.EmpCode;
                    Header.Result = staffs.Result;
                    Header.CreatedBy = User.UserName;
                    Header.CreatedOn = DateTime.Now;
                    DB.HREmpEducs.Add(Header);
                }

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
            catch (Exception e)
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
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public string CreateEmpEdu()
        {
            try
            {
                HumicaDBContext DBM = new HumicaDBContext();
                if (HeaderStaff.EmpCode == null) return "EMPCODE_EN";
                if (Header.EduType == null) return "EDUTYPE_EN";
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;

                DB.HREmpEducs.Add(Header);
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
        public string EditEmpEdu(int id)
        {
            try
            {
                Header.EmpCode = HeaderStaff.EmpCode;
                HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = DB.HREmpEducs.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "FAMILY_NE";
                }

                objMatch.EduType = Header.EduType;
                objMatch.StartDate = Header.StartDate;
                objMatch.EndDate = Header.EndDate;
                objMatch.EdcCenter = Header.EdcCenter;
                objMatch.Major = Header.Major;
                objMatch.Result = Header.Result;
                objMatch.Remark = Header.Remark;
                objMatch.AttachFile = Header.AttachFile;
                objMatch.ChangedBy = Header.ChangedBy;
                objMatch.ChangedOn = Header.ChangedOn;

                DB.HREmpEducs.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Modified;
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
        public string DeleteEmpEdu(int id)
        {
            try
            {
                Header = new HREmpEduc();
                var objMatch = DB.HREmpEducs.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "EDCUATION _NE";
                }
                Header.TranNo = id;
                DB.HREmpEducs.Attach(objMatch);
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
    public class EmpEducation
    {
        public long TranNo { get; set; }
        public string EmpCode { get; set; }

        public string AllName { get; set; }
        public string Department { get; set; }
        public string EduType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EdcCenter { get; set; }
        public string Major { get; set; }
        public string Result { get; set; }
        public string Remark { get; set; }
    }
}