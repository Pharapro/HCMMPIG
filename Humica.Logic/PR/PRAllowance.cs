using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.PR
{
    public class PRAllowance
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PREmpAllw Header { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public string MessageError { get; set; }
        public FTINYear Filter { get; set; }
        public List<HR_View_EmpAllowance> ListHeaderLoad { get; set; }
        public List<PREmpAllw> ListHeader { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }

        public PRAllowance()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateAllowance()
        {
            try
            {
                Header = new PREmpAllw();

                if (HeaderStaff.EmpCode == null) return "EMPCODE_EN";
                if (ListHeader.Count == 0) return "LIST_INV";
                var TranNo = DB.PREmpAllws.OrderByDescending(u => u.TranNo).FirstOrDefault();
                var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "ALLW").ToList();
                var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);

                foreach (var read in ListHeader)
                {

                    int status = 0;
                    int FDate = read.FromDate.Value.Month;
                    int TDate = read.ToDate.Value.Month;
                    foreach (var item in RewardType.Where(w => w.Code == read.AllwCode).ToList())
                    {
                        read.AllwDescription = item.Description;
                    }
                    if (FDate == TDate && read.FromDate.Value.Year == read.ToDate.Value.Year)
                    {
                        status = 1;
                    }
                    read.Status = status;
                    read.LCK = 0;
                    read.EmpCode = HeaderStaff.EmpCode;
                    read.EmpName = HeaderStaff.AllName;
                    read.CompanyCode = objStaff.CompanyCode;
                    if (TranNo != null)
                    {
                        read.TranNo = TranNo.TranNo + 1;
                    }
                    else
                    {
                        read.TranNo = 1;
                    }
                    Header.TranNo = read.TranNo;
                    DB.PREmpAllws.Add(read);
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
        public string EditAllowance(int id, string EmpCode)
        {
            try
            {
                Header = new PREmpAllw();
                var check = DB.PREmpAllws.Where(u => u.TranNo == id && u.LCK == 1 && u.EmpCode == EmpCode).ToList();

                if (check.Count > 0) return "ALLLock";
                if (ListHeader.Count == 0) return "LIST_INV";

                var List = DB.PREmpAllws.Where(u => u.TranNo == id && u.EmpCode == EmpCode).ToList();

                foreach (var read in List)
                {
                    DB.PREmpAllws.Remove(read);
                    DB.SaveChanges();
                }
                var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "ALLW").ToList();
                foreach (var read in ListHeader)
                {
                    foreach (var item in RewardType.Where(w => w.Code == read.AllwCode).ToList())
                    {
                        read.AllwDescription = item.Description;
                    }
                    int status = 0;
                    int FDate = read.FromDate.Value.Month;
                    int TDate = read.ToDate.Value.Month;

                    if (FDate == TDate && read.FromDate.Value.Year == read.ToDate.Value.Year)
                    {
                        status = 1;
                    }
                    read.Status = status;
                    read.LCK = 0;
                    read.EmpCode = HeaderStaff.EmpCode;
                    read.EmpName = HeaderStaff.AllName;
                    read.TranNo = id;
                    Header.TranNo = id;
                    DB.PREmpAllws.Add(read);
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
        public string Delete(int id, string EmpCode)
        {
            try
            {
                Header = new PREmpAllw();
                var objMatch = DB.PREmpAllws.FirstOrDefault(u => u.TranNo == id && u.EmpCode == EmpCode);

                if (objMatch == null) return "DOC_NE";

                Header.TranNo = id;
                DB.PREmpAllws.Attach(objMatch);
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
        public string ImportAllowance()
        {
            try
            {
                var emp = DB.HRStaffProfiles.ToList();
                var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "ALLW").ToList();
                var TranNo = DB.PREmpAllws.OrderByDescending(u => u.TranNo).FirstOrDefault();
                int TN = 0;
                TN = (TranNo == null) ? 0 : TranNo.TranNo;

                foreach (var read in ListHeader)
                {
                    var empcheck = emp.Where(w => w.EmpCode == read.EmpCode).ToList();
                    var RewardTypecheck = RewardType.Where(w => w.Code == read.AllwCode).ToList();
                    if (empcheck.Count() == 0) return "Invalid EmpCode : " + read.EmpCode;
                    if (RewardTypecheck.Count() == 0) return read.EmpCode + " has invalid Allowance Type " + read.AllwCode;
                    if (read.FromDate > read.ToDate) return read.EmpCode + " has invalid FromDate";
                    if (read.ToDate < read.FromDate) return read.EmpCode + " has invalid Todate";
                    if (empcheck.Count > 0 && RewardTypecheck.Count > 0)
                    {
                        var obj = new PREmpAllw();
                        int status = 0;
                        int FDate = read.FromDate.Value.Month;
                        int TDate = read.ToDate.Value.Month;

                        if (FDate == TDate && read.FromDate.Value.Year == read.ToDate.Value.Year)
                        {
                            status = 1;
                        }

                        obj.Status = status;
                        obj.LCK = 0;
                        obj.AllwCode = read.AllwCode;
                        obj.AllwDescription = RewardTypecheck.First().Description;
                        obj.EmpCode = read.EmpCode;
                        obj.EmpName = empcheck.First().AllName;
                        obj.CompanyCode = empcheck.First().CompanyCode;
                        obj.CreateBy = User.UserName;
                        obj.CreateOn = DateTime.Now;
                        obj.TranNo = TN + 1;
                        obj.FromDate = read.FromDate;
                        obj.ToDate = read.ToDate;
                        obj.Amount = read.Amount;
                        obj.Remark = read.Remark;
                        DB.PREmpAllws.Add(obj);
                        TN++;
                    }
                    else
                    {
                        MessageError = "EmpCode :" + read.EmpCode + " INVALID CODE: " + read.AllwCode;
                        return "INV_DOC";
                    }
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
        public int IsExistEmployeeAllowance(string EmpCode, string AllowanceCode, DateTime FromDate, DateTime ToDate)
        {
            int Result = -1;
            var _list = DB.PREmpAllws.Where(w => w.EmpCode == EmpCode).ToList();
            _list = _list.Where(w => w.AllwCode == AllowanceCode &&
               ((FromDate.Date >= w.FromDate.Value.Date && FromDate.Date <= w.ToDate.Value.Date) || (ToDate.Date >= w.FromDate.Value.Date && ToDate.Date <= w.ToDate.Value.Date))).ToList();
            if (_list.Count > 0)
            {
                foreach (var item in _list)
                {
                    Result = Convert.ToInt32(item.Status);
                }
            }
            return Result;
        }
    }
}


