using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.HR
{
    public class HREmpDepositObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public HREmpDeposit Header { get; set; }
        public string MessageCode { get; set; }
        public List<HRCertificationType> ListEduType { get; set; }
        public List<HREmpDeposit> ListHeader { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }
        public List<HR_STAFF_VIEW> ListStaffView { get; set; }
        public List<HREmpDepositItem> ListHeaderD { get; set; }
        public HREmpDepositObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateDeposit()
        {
            try
            {
                if (HeaderStaff.EmpCode == null) return "EMPCODE_EN";
                if (Header.DeductionType == null) return "DEDTYPE_EN";

                var objDED = DB.PR_RewardsType.Where(w => w.ReCode == "DED" && w.Code == Header.DeductionType);
                if (objDED == null)
                {
                    return "DEDUCTION_TYPE";
                }
                if (ListHeaderD.Count <= 0)
                {
                    return "INV_DOC";
                }
                if (Header.FromDate > Header.ToDate)
                {
                    return "INVALID_DATE";
                }
                var objNumber = new CFNumberRank("ST_DEPOS");
                if (objNumber == null)
                {
                    return "INV_CODE";
                }
                Header.DepositNum = objNumber.NextNumberRank.Trim();
                Header.EmpCode = HeaderStaff.EmpCode;
                Header.Deduction = objDED.FirstOrDefault().Description;
                Header.EmployeeName = HeaderStaff.AllName;
                Header.TotalDeposit = 0;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                DB.HREmpDeposits.Add(Header);

                int Line = 0;

                foreach (var item in ListHeaderD)
                {
                    Line += 1;
                    var result = new HREmpDepositItem();
                    result.DepositNum = Header.DepositNum;
                    result.EmpCode = Header.EmpCode;
                    result.InMonth = item.InMonth;
                    result.PayMonth = item.PayMonth;
                    result.Deposit = item.Deposit;
                    result.Remark = item.Remark;
                    result.LineItem = Line;
                    DB.HREmpDepositItems.Add(result);
                }

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.DepositNum;
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
                log.DocurmentAction = Header.DepositNum;
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
                log.DocurmentAction = Header.DepositNum;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditDeposit(string id)
        {
            try
            {
                Header.EmpCode = HeaderStaff.EmpCode;
                HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = DB.HREmpDeposits.FirstOrDefault(w => w.DepositNum == id);
                if (objMatch == null)
                {
                    return "STAFF_NE";
                }
                var objDED = DB.PR_RewardsType.Where(w => w.ReCode == "DED" && w.Code == Header.DeductionType);
                if (objDED == null)
                {
                    return "DEDUCTION_TYPE";
                }
                var objEmp = DB.HREmpDepositItems.Where(w => w.DepositNum == objMatch.DepositNum && w.EmpCode == objMatch.EmpCode).ToList();
                foreach (var item in objEmp)
                {
                    DB.HREmpDepositItems.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }

                objMatch.Deduction = objDED.FirstOrDefault().Description;
                objMatch.FromDate = Header.FromDate;
                objMatch.ToDate = Header.ToDate;
                objMatch.Remark = Header.Remark;
                objMatch.Amount = Header.Amount;
                objMatch.Deposit = Header.Deposit;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                DB.HREmpDeposits.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Deduction).IsModified = true;
                DB.Entry(objMatch).Property(w => w.FromDate).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ToDate).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Remark).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Amount).IsModified = true;
                DB.Entry(objMatch).Property(w => w.Deposit).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;

                int Line = 0;
                foreach (var item in ListHeaderD)
                {
                    Line += 1;
                    var result = new HREmpDepositItem();
                    result.DepositNum = objMatch.DepositNum;
                    result.EmpCode = Header.EmpCode;
                    result.InMonth = item.InMonth;
                    result.PayMonth = item.PayMonth;
                    result.Deposit = item.Deposit;
                    result.Remark = item.Remark;
                    result.LineItem = Line;
                    DB.HREmpDepositItems.Add(result);
                }
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.DepositNum;
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
                log.DocurmentAction = Header.DepositNum;
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
                log.DocurmentAction = Header.DepositNum;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }

        }
        public string DeleteDeposit(string id)
        {
            try
            {
                Header = new HREmpDeposit();
                Header.DepositNum = id;
                var objMatch = DB.HREmpDeposits.FirstOrDefault(w => w.DepositNum == id);
                if (objMatch == null)
                {
                    return "DOC_INV";
                }
                Header = objMatch;
                var objEmp = DB.HREmpDepositItems.Where(w => w.DepositNum == objMatch.DepositNum).ToList();
                foreach (var item in objEmp)
                {
                    DB.HREmpDepositItems.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                DB.HREmpDeposits.Attach(objMatch);
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
                log.ScreenId = Header.DepositNum;
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
                log.ScreenId = Header.DepositNum;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string PayBackDeposit(string ID, DateTime InMonth)
        {
            try
            {
                var objMatch = DB.HREmpDeposits.FirstOrDefault(w => w.DepositNum == ID);
                if (objMatch == null)
                {
                    return "EE001";
                }
                //var EmpSalary = DB.HISGenSalaries.Where(w => w.EmpCode == objMatch.EmpCode &&
                //  w.INYear == objMatch.InMonth.Year && w.INMonth == objMatch.InMonth.Month).ToList();
                //if (EmpSalary.Where(w => w.Status == SYDocumentStatus.HOLD.ToString()).Count() > 0)
                //{
                //    var Obj = EmpSalary.FirstOrDefault();
                //    Obj.Status = SYDocumentStatus.OPEN.ToString();
                //    DB.HISGenSalaries.Attach(Obj);
                //    DB.Entry(Obj).Property(w => w.Status).IsModified = true;
                //}


                objMatch.Status = SYDocumentStatus.CLEARED.ToString();
                objMatch.PayBack = InMonth;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;

                DB.HREmpDeposits.Attach(objMatch);
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
    }
}