using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.PR
{
    public class PREmpLoan
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public HREmpLoanH Header { get; set; }
        public HREmpLoan HeaderD { get; set; }
        public string ScreenId { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public string CountryCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<HR_VIEW_EMPLOAN> ListHeader { get; set; }
        public List<HREmpLoan> ListHeaderD { get; set; }

        public PREmpLoan()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        public string CreateLoan()
        {
            try
            {
                if (Filter.EmpCode == null) return "EMPCODE_EN";
                if (ListHeaderD.Count <= 0) return "INV_DOC";
                //if (Header.FromDate > Header.ToDate)
                //{
                //    return "INVALID_DATE";
                //}
                var objNumber = new CFNumberRank("LOAN");
                if (objNumber == null)
                {
                    return "INV_CODE";
                }
                Header.LoanID = objNumber.NextNumberRank.Trim();
                Header.EmpCode = Filter.EmpCode;
                Header.CreatedBy = User.UserName;
                Header.CreatedOn = DateTime.Now;
                DB.HREmpLoanHs.Add(Header);

                int Line = 0;

                foreach (var item in ListHeaderD)
                {
                    Line += 1;
                    var result = new HREmpLoan();
                    result.LoanID = Header.LoanID;
                    result.EmpCode = Header.EmpCode;
                    result.LoanDate = item.LoanDate;
                    result.PayMonth = item.PayMonth;
                    result.LoanAm = item.LoanAm;
                    result.Remark = item.Remark;
                    result.LineItem = Line;
                    result.BeginingBalance = item.BeginingBalance;
                    result.EndingBalance = item.EndingBalance;
                    result.Status = SYDocumentStatus.OPEN.ToString();
                    DB.HREmpLoans.Add(result);
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
                log.DocurmentAction = Header.LoanID;
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
                log.DocurmentAction = Header.LoanID;
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
                log.DocurmentAction = Header.LoanID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditEmpLoan(string id)
        {
            try
            {
                HumicaDBContext DBM = new HumicaDBContext();
                var objMatch = DB.HREmpLoanHs.FirstOrDefault(w => w.LoanID == id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                if (objMatch.LoanAmount != ListHeaderD.Sum(x => x.LoanAm))
                {
                    return "INV_AMOUNT";
                }
                //if (Header.FromDate > Header.ToDate)
                //{
                //    return "INVALID_DATE";
                //}
                string Open = SYDocumentStatus.OPEN.ToString();
                var objEmpLoan = DB.HREmpLoans.Where(w => w.LoanID == objMatch.LoanID && w.EmpCode == objMatch.EmpCode).ToList();
                int Count_pay = objEmpLoan.Where(w => w.Status != Open).ToList().Count();
                var objEmp = objEmpLoan.Where(w => w.Status == Open).ToList();

                foreach (var item in objEmp)
                {
                    DB.HREmpLoans.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }

                Header.EmpCode = objMatch.EmpCode;
                objMatch.Amount = Header.Amount;
                objMatch.FromDate = Header.FromDate;
                objMatch.ToDate = Header.ToDate;
                objMatch.ChangedBy = User.UserName;
                objMatch.ChangedOn = DateTime.Now;
                DB.HREmpLoanHs.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Amount).IsModified = true;
                DB.Entry(objMatch).Property(w => w.FromDate).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ToDate).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objMatch).Property(w => w.ChangedOn).IsModified = true;

                int Line = Count_pay;
                foreach (var item in ListHeaderD.Where(w => w.Status == Open))
                {
                    Line += 1;
                    var result = new HREmpLoan();
                    result.LoanID = objMatch.LoanID;
                    result.EmpCode = Header.EmpCode;
                    result.LoanDate = item.LoanDate;
                    result.PayMonth = item.PayMonth;
                    result.LoanAm = item.LoanAm;
                    result.Remark = item.Remark;
                    result.LineItem = Line;
                    result.BeginingBalance = item.BeginingBalance;
                    result.EndingBalance = item.EndingBalance;
                    result.Status = SYDocumentStatus.OPEN.ToString();
                    DB.HREmpLoans.Add(result);
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
                log.DocurmentAction = Header.LoanID;
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
                log.DocurmentAction = Header.LoanID;
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
                log.DocurmentAction = Header.LoanID;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }

        }

        public string DeleteLoan(string id)
        {
            try
            {
                Header = new HREmpLoanH();
                Header.LoanID = id;
                var objLoan = DB.HREmpLoanHs.FirstOrDefault(w => w.LoanID == id);
                if (objLoan == null)
                {
                    return "LOAN_NE";
                }
                Header = objLoan;
                var objEmp = DB.HREmpLoans.Where(w => w.LoanID == objLoan.LoanID).ToList();
                foreach (var item in objEmp)
                {
                    DB.HREmpLoans.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                DB.HREmpLoanHs.Attach(objLoan);
                DB.Entry(objLoan).State = System.Data.Entity.EntityState.Deleted;

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.LoanID;
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
                log.ScreenId = Header.LoanID;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}


