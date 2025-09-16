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
    public class PRDeduction
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PREmpDeduc Header { get; set; }
        public PREmpLateDeduct HeaderLE { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public string MessageError { get; set; }
        public FTINYear Filter { get; set; }
        public List<HR_EmpLateEarly_View> ListEmpLateEarly { get; set; }
        public List<PREmpDeduc> ListHeader { get; set; }
        public List<PREmpLateDeduct> ListHeaderLE { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }

        public PRDeduction()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        public string CreateDeduction()
        {
            try
            {
                Header = new PREmpDeduc();

                if (HeaderStaff.EmpCode == null) return "EMPCODE_EN";
                if (ListHeader.Count == 0) return "LIST_INV";

                var TranNo = DB.PREmpDeducs.OrderByDescending(u => u.TranNo).FirstOrDefault();
                var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "DED").ToList();
                int ID = 0;
                var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);


                foreach (var read in ListHeader)
                {
                    ID++;
                    foreach (var item in RewardType.Where(w => w.Code == read.DedCode).ToList())
                    {
                        read.DedDescription = item.Description;
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
                    read.CompanyCode = objStaff.CompanyCode;
                    if (TranNo != null)
                    {
                        read.TranNo = TranNo.TranNo + ID;
                    }
                    else
                    {
                        read.TranNo = ID;
                    }
                    Header.TranNo = read.TranNo;
                    DB.PREmpDeducs.Add(read);
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
        public string ImportDeduction()
        {
            try
            {

                var emp = DB.HRStaffProfiles.ToList();
                var TranNo = DB.PREmpDeducs.OrderByDescending(u => u.TranNo).FirstOrDefault();
                var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "DED").ToList();

                long TN = 0;
                TN = (TranNo == null) ? 0 : TranNo.TranNo;

                foreach (var read in ListHeader)
                {
                    var empcheck = emp.Where(w => w.EmpCode == read.EmpCode).ToList();
                    var RewardTypecheck = RewardType.Where(w => w.Code == read.DedCode).ToList();
                    if (empcheck.Count() == 0) return "Invalid EmpCode : " + read.EmpCode;
                    if (RewardTypecheck.Count() == 0) return read.EmpCode + " has invalid Deduction Type " + read.DedCode;
                    if (read.FromDate > read.ToDate) return read.EmpCode + " has invalid FromDate";
                    if (read.ToDate < read.FromDate) return read.EmpCode + " has invalid Todate";
                    if (empcheck.Count > 0 && RewardTypecheck.Count > 0)
                    {
                        var obj = new PREmpDeduc();
                        int status = 0;
                        int FDate = read.FromDate.Value.Month;
                        int TDate = read.ToDate.Value.Month;

                        if (FDate == TDate && read.FromDate.Value.Year == read.ToDate.Value.Year)
                        {
                            status = 1;
                        }

                        obj.Status = status;
                        obj.LCK = 0;
                        obj.DedCode = read.DedCode;
                        obj.DedDescription = RewardTypecheck.First().Description;
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
                        DB.PREmpDeducs.Add(obj);
                        TN++;
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
        public string EditDeduction(int id, string EmpCode)
        {
            try
            {
                Header = new PREmpDeduc();
                var check = DB.PREmpDeducs.Where(u => u.TranNo == id && u.LCK == 1 && u.EmpCode == EmpCode).ToList();

                if (check.Count > 0) return "ALLLock";
                if (ListHeader.Count == 0) return "LIST_INV";

                var List = DB.PREmpDeducs.Where(u => u.TranNo == id && u.EmpCode == EmpCode).ToList();

                foreach (var read in List)
                {
                    DB.PREmpDeducs.Remove(read);
                    DB.SaveChanges();
                }
                var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "DED").ToList();
                foreach (var read in ListHeader)
                {
                    foreach (var item in RewardType.Where(w => w.Code == read.DedCode).ToList())
                    {
                        read.DedDescription = item.Description;
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
                    DB.PREmpDeducs.Add(read);
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
                Header = new PREmpDeduc();
                var objMatch = DB.PREmpDeducs.FirstOrDefault(u => u.TranNo == id && u.EmpCode == EmpCode);

                if (objMatch == null) return "DOC_NE";

                Header.TranNo = id;
                DB.PREmpDeducs.Attach(objMatch);
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
        public string CreateLateEarly()
        {
            try
            {
                if (HeaderStaff.EmpCode == null) return "EMPCODE_EN";
                if (ListHeaderLE.Count == 0) return "LIST_INV";

                var TranNo = DB.PREmpLateDeducts.OrderByDescending(u => u.TranNo).FirstOrDefault();
                foreach (var read in ListHeaderLE)
                {
                    var Result = new PREmpLateDeduct();
                    Result.EmpCode = HeaderStaff.EmpCode;
                    Result.DedCode = read.DedCode;
                    Result.Day = read.Day;
                    Result.Minute = read.Minute;
                    Result.InMonth = read.InMonth;
                    Result.FromDate = read.FromDate;
                    Result.ToDate = read.ToDate;
                    Result.Remark = read.Remark;
                    Result.Status = 1;
                    Result.LCK = 0;
                    Result.CreateBy = User.UserName;
                    Result.CreateOn = DateTime.Now;
                    DB.PREmpLateDeducts.Add(Result);
                    HeaderLE.TranNo = Result.TranNo;
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
                log.DocurmentAction = HeaderLE.TranNo.ToString(); ;
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
                log.DocurmentAction = HeaderLE.TranNo.ToString();
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
                log.DocurmentAction = HeaderLE.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditLateEarly(int id)
        {
            try
            {
                HeaderLE = new PREmpLateDeduct();
                //var check = DB.PREmpDeducs.Where(u => u.TranNo == id && u.LCK == 1).ToList();
                //if (check.Count > 0)
                //{
                //    return "ALLLock";
                //}

                if (HeaderStaff.EmpCode == null) return "EMPCODE_EN";

                var List = DB.PREmpLateDeducts.Where(u => u.TranNo == id).ToList();

                foreach (var read in List)
                {
                    DB.PREmpLateDeducts.Remove(read);
                    DB.SaveChanges();
                }
                foreach (var read in ListHeaderLE)
                {
                    var Result = new PREmpLateDeduct();
                    Result.EmpCode = HeaderStaff.EmpCode;
                    Result.DedCode = read.DedCode;
                    Result.Day = read.Day;
                    Result.Minute = read.Minute;
                    Result.InMonth = read.ToDate.Value;
                    Result.FromDate = read.FromDate;
                    Result.ToDate = read.ToDate;
                    Result.Remark = read.Remark;
                    Result.Status = 1;
                    Result.LCK = 0;
                    Result.ChangedBy = User.UserName;
                    Result.ChangedOn = DateTime.Now;
                    DB.PREmpLateDeducts.Add(Result);
                    HeaderLE.TranNo = Result.TranNo;
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
                log.DocurmentAction = HeaderLE.TranNo.ToString();
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
                log.DocurmentAction = HeaderLE.TranNo.ToString();
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
                log.DocurmentAction = HeaderLE.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteLateEarly(int id)
        {
            try
            {
                HeaderLE = new PREmpLateDeduct();
                var objMatch = DB.PREmpLateDeducts.FirstOrDefault(u => u.TranNo == id);

                if (objMatch == null)
                {
                    return "DEDCUTION_EN";
                }
                HeaderLE.TranNo = id;
                DB.PREmpLateDeducts.Attach(objMatch);
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
                log.DocurmentAction = HeaderLE.TranNo.ToString();
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
                log.DocurmentAction = HeaderLE.TranNo.ToString();
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
                log.DocurmentAction = HeaderLE.TranNo.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }

        public int IsExistEmployeeDeduction(string EmpCode, string DedCode, DateTime FromDate, DateTime ToDate)
        {
            int Result = -1;
            var _list = DB.PREmpDeducs.Where(w => w.EmpCode == EmpCode).ToList();
            _list = _list.Where(w => w.DedCode == DedCode &&
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
        public int IsExistLateEarlyDeduction(string EmpCode, string DedCode, DateTime FromDate, DateTime ToDate)
        {
            int Result = -1;
            var _list = DB.PREmpLateDeducts.Where(w => w.EmpCode == EmpCode).ToList();
            _list = _list.Where(w => w.DedCode == DedCode &&
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


