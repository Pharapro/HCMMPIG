using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.PR
{
    public class PREmpHoldObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PREmpHold Header { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public string ScreenId { get; set; }
        public string EmpName { get; set; }
        public string EmpCode { get; set; }
        public string MessageCode { get; set; }
        public string MessageError { get; set; }
        public List<PREmpHold> ListHeader { get; set; }


        public PREmpHoldObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        public string CreateHold()
        {
            try
            {
                if (Header.Salary <= 0)
                {
                    return "STAFF_NE";
                }
                var Open = SYDocumentStatus.OPEN.ToString();
                var EmpSalary = DB.HISGenSalaries.Where(w => w.EmpCode == HeaderStaff.EmpCode &&
                  w.INYear == Header.InMonth.Year && w.INMonth == Header.InMonth.Month && w.Status == Open).ToList();
                if (EmpSalary.Count() <= 0)
                {
                    return "DOC_INV";
                }
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.AllName = HeaderStaff.AllName;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                DB.PREmpHolds.Add(Header);
                int row = DB.SaveChanges();

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

        public string EditHold(int id)
        {
            try
            {

                var objMatch = DB.PREmpHolds.FirstOrDefault(w => w.TranNo == id);
                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                if (Header.Salary <= 0)
                {
                    return "STAFF_NE";
                }
                var Open = SYDocumentStatus.OPEN.ToString();
                var EmpSalary = DB.HISGenSalaries.Where(w => w.EmpCode == HeaderStaff.EmpCode &&
                  w.INYear == Header.InMonth.Year && w.INMonth == Header.InMonth.Month && w.Status == Open).ToList();
                if (EmpSalary.Count() <= 0)
                {
                    return "DOC_INV";
                }
                Header.EmpCode = objMatch.EmpCode;
                objMatch.AllName = HeaderStaff.AllName;
                objMatch.InMonth = Header.InMonth;
                objMatch.Salary = Header.Salary;
                objMatch.Reason = Header.Reason;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                DB.PREmpHolds.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Modified;
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
        public string Delete(int id)
        {
            try
            {
                Header = new PREmpHold();
                var objMatch = DB.PREmpHolds.FirstOrDefault(u => u.TranNo == id);

                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                Header.TranNo = id;
                DB.PREmpHolds.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Deleted;
                int row = DB.SaveChanges();
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
        public string ApproveOTReq(int ID)
        {
            try
            {
                var objMatch = DB.PREmpHolds.FirstOrDefault(w => w.TranNo == ID);
                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                Header = new PREmpHold();
                Header = objMatch;
                var Open = SYDocumentStatus.OPEN.ToString();
                var EmpSalary = DB.HISGenSalaries.Where(w => w.EmpCode == objMatch.EmpCode &&
                  w.INYear == objMatch.InMonth.Year && w.INMonth == objMatch.InMonth.Month && w.Status == Open).ToList();
                if (EmpSalary.Count() <= 0)
                {
                    return "DOC_INV";
                }
                else
                {
                    var Obj = EmpSalary.FirstOrDefault();
                    Obj.Status = SYDocumentStatus.HOLD.ToString();
                    DB.HISGenSalaries.Attach(Obj);
                    DB.Entry(Obj).Property(w => w.Status).IsModified = true;
                }
                objMatch.Status = SYDocumentStatus.HOLD.ToString();
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                DB.PREmpHolds.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }

            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string PayBackHold(int ID, DateTime InMonth)
        {
            try
            {
                var objMatch = DB.PREmpHolds.FirstOrDefault(w => w.TranNo == ID);
                if (objMatch == null)
                {
                    return "EE001";
                }
                var EmpSalary = DB.HISGenSalaries.Where(w => w.EmpCode == objMatch.EmpCode &&
                  w.INYear == objMatch.InMonth.Year && w.INMonth == objMatch.InMonth.Month).ToList();
                if (EmpSalary.Where(w => w.Status == SYDocumentStatus.HOLD.ToString()).Count() > 0)
                {
                    var Obj = EmpSalary.FirstOrDefault();
                    Obj.Status = SYDocumentStatus.OPEN.ToString();
                    DB.HISGenSalaries.Attach(Obj);
                    DB.Entry(Obj).Property(w => w.Status).IsModified = true;
                }


                objMatch.Status = SYDocumentStatus.CLEARED.ToString();
                objMatch.PayBack = InMonth;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                DB.PREmpHolds.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.Entry(objMatch).Property(w => w.PayBack).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                int row = DB.SaveChanges();

                return SYConstant.OK;
            }

            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }

        }
        public string Cancel(int ID)
        {
            try
            {
                var objMatch = DB.PREmpHolds.FirstOrDefault(w => w.TranNo == ID);
                if (objMatch == null)
                {
                    return "EE001";
                }
                var Open = SYDocumentStatus.OPEN.ToString();
                var EmpSalary = DB.HISGenSalaries.Where(w => w.EmpCode == objMatch.EmpCode &&
                  w.INYear == objMatch.InMonth.Year && w.INMonth == objMatch.InMonth.Month).ToList();
                if (EmpSalary.Count() > 0)
                {
                    var Obj = EmpSalary.FirstOrDefault();
                    Obj.Status = Open;
                    DB.HISGenSalaries.Attach(Obj);
                    DB.Entry(Obj).Property(w => w.Status).IsModified = true;
                }
                objMatch.Status = SYDocumentStatus.CANCELLED.ToString();
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                DB.PREmpHolds.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                int row = DB.SaveChanges();


                return SYConstant.OK;
            }

            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ID.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}


