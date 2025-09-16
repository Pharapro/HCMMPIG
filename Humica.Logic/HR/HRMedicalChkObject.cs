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
    public class HRMedicalChkObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public HREmpPhischk Header { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<HREmpPhischk> ListHeader { get; set; }
        public string MessageCode { get; set; }

        public HRMedicalChkObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        #region 'Create'
        public string saveEmpMChk()
        {
            try
            {
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.Createdby = User.UserName;
                Header.CreatedOn = DateTime.Now;

                DB.HREmpPhischks.Add(Header);

                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.TranNo.ToString();
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
                log.DocurmentAction = Header.TranNo.ToString();
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
                log.DocurmentAction = Header.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();
                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion
        #region 'Update'
        public string UpdEmpMChk(int TranNo)
        {
            try
            {
                DB = new HumicaDBContext();
                var ObjMatch = DB.HREmpPhischks.FirstOrDefault(w => w.TranNo == TranNo);
                if (ObjMatch == null)
                {
                    return "DOC_INV";
                }

                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;

                ObjMatch.EmpCode = Header.EmpCode;
                ObjMatch.CHKDate = Header.CHKDate;
                ObjMatch.MedicalType = Header.MedicalType;
                ObjMatch.HospCHK = Header.HospCHK;
                ObjMatch.Result = Header.Result;
                ObjMatch.Description = Header.Description;
                ObjMatch.Remark = Header.Remark;

                DB.HREmpPhischks.Attach(ObjMatch);

                DB.Entry(ObjMatch).Property(x => x.EmpCode).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.CHKDate).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.MedicalType).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.HospCHK).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Result).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
                DB.Entry(ObjMatch).Property(x => x.Remark).IsModified = true;
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
                log.DocurmentAction = TranNo.ToString();
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
                log.DocurmentAction = TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        #endregion
        #region 'Delete'
        public string dtEmpMCkh(int TranNo)
        {
            try
            {
                DB = new HumicaDBContext();

                var ObjMatch = DB.HREmpPhischks.FirstOrDefault(w => w.TranNo == TranNo);
                if (ObjMatch == null)
                {
                    return "DOC_INV";
                }

                DB.HREmpPhischks.Remove(ObjMatch);
                DB.SaveChanges();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = TranNo.ToString();
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
                log.DocurmentAction = TranNo.ToString();
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
                log.DocurmentAction = TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        #endregion
    }
}