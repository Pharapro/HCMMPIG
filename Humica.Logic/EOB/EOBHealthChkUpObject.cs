using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.EOB
{
    public class EOBHealthChkUpObject
    {
        public SMSystemEntity DBI = new SMSystemEntity();
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public HR_STAFF_VIEW staffProfile { get; set; }
        public EOBEmpHealthCheckUp Header { get; set; }
        public List<EOBEmpHealthCheckUp> ListHeader { get; set; }
        public List<EOBEmpHealthCheckUpRecord> ListItem { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }

        public EOBHealthChkUpObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        public string createChkUp()
        {
            try
            {
                var _chkStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == staffProfile.EmpCode);
                if (_chkStaff == null)
                {
                    return "Emp_Already_Health Check Up";
                }
                Header.EmpCode = staffProfile.EmpCode;
                Header.DocDate = DateTime.Now;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;

                DB.EOBEmpHealthCheckUps.Add(Header);

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
        }
        public string UpdChkUp(string EmpCode)
        {
            try
            {
                DB = new HumicaDBContext();
                var ObjMatch = DB.EOBEmpHealthCheckUps.FirstOrDefault(w => w.EmpCode == EmpCode);
                if (ObjMatch == null)
                    return "DOC_INV";

                var _chkListItem = DB.EOBEmpHealthCheckUpRecords.Where(w => w.EmpCode == EmpCode).ToList();
                foreach (var read in _chkListItem.ToList())
                {
                    DB.EOBEmpHealthCheckUpRecords.Remove(read);
                }
                foreach (var read in ListItem.ToList())
                {
                    read.EmpCode = ObjMatch.EmpCode;
                    DB.EOBEmpHealthCheckUpRecords.Add(read);
                }

                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;
                ObjMatch.DrName = Header.DrName;
                ObjMatch.Checked = Header.Checked;
                ObjMatch.PhoneNo = Header.PhoneNo;
                ObjMatch.ChkUpDate = Header.ChkUpDate;
                ObjMatch.Description = Header.Description;
                ObjMatch.AttachmentRef = Header.AttachmentRef;

                DB.EOBEmpHealthCheckUps.Attach(ObjMatch);

                DB.Entry(ObjMatch).Property(x => x.DrName).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Checked).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.PhoneNo).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChkUpDate).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.AttachmentRef).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChangedBy).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.ChangedOn).IsModified = true;

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
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

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
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string deleteCheckUp(string EmpCode)
        {
            try
            {
                DB = new HumicaDBContext();

                var ObjMatch = DB.EOBEmpHealthCheckUps.FirstOrDefault(w => w.EmpCode == EmpCode);
                if (ObjMatch.Checked == true)
                    return "DOC_INV";

                var _chkListItem = DB.EOBEmpHealthCheckUpRecords.Where(w => w.EmpCode == EmpCode).ToList();

                foreach (var read in _chkListItem.ToList())
                {
                    DB.EOBEmpHealthCheckUpRecords.Remove(read);
                }

                DB.EOBEmpHealthCheckUps.Remove(ObjMatch);
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = EmpCode;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

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
                log.DocurmentAction = EmpCode;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

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
                log.DocurmentAction = EmpCode;
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}
