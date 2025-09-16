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
    public class PRBonusObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public HumicaDBViewContext DBV = new HumicaDBViewContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PREmpBonu Header { get; set; }
        public string ScreenId { get; set; }
        public string MessageCode { get; set; }
        public string MessageError { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public List<PREmpBonu> ListHeader { get; set; }
        public List<BonusAnnual> ListTemBonusAnnual { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public HR_STAFF_VIEW HeaderStaff { get; set; }

        public PRBonusObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<BonusAnnual> ListEmployeeBonusAnnual(FTFilterEmployee Filter1, List<HRBranch> _listBranch)
        {
            var _List = new List<BonusAnnual>();
            var TempBon = DB.Temp_BonusAnnual.Where(w => w.InYear == Filter1.INYear).ToList();
            var ListStaff = DB.HRStaffProfiles.ToList();
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            Staff = Staff.Where(w => _listBranch.Where(x => x.Code == w.BranchID).Any()).ToList();
            ListStaff = ListStaff.Where(w => w.CareerDesc != "TERMINAT").ToList();
            if (Filter1.IsNewJoin == true)
                ListStaff = ListStaff.Where(w => w.CareerDesc == "NEWJOIN").ToList();
            Staff = Staff.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();

            TempBon = TempBon.Where(w => Staff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            if (TempBon.Count() > 0)
            {
                foreach (var item in TempBon)
                {
                    var _bonus = new BonusAnnual();
                    _bonus.EmpCode = item.EmpCode;
                    _bonus.AllName = Staff.FirstOrDefault(w => w.EmpCode == item.EmpCode).AllName;
                    _bonus.Position = Staff.FirstOrDefault(w => w.EmpCode == item.EmpCode).Position;
                    _bonus.StartDate = Staff.FirstOrDefault(w => w.EmpCode == item.EmpCode).StartDate;
                    _bonus.Salary = item.Salary;
                    _bonus.UnpaidLeave = item.Unpaid_leave;
                    _bonus.Warning = item.Warning;
                    _bonus.Adding = item.Adding;
                    _bonus.TotalBonus = item.TotalBonus;
                    if (item.LOCK == 0) _bonus.Transfer = "No";
                    else _bonus.Transfer = "Yes";
                    _bonus.Remark = item.REMARK;
                    _List.Add(_bonus);
                }
            }
            else
            {
                var empCareer = DB.HREmpCareers.Where(w => w.ToDate.Value.Year == 5000).ToList();
                if (Filter1.Branch != null)
                    empCareer = empCareer.Where(w => w.Branch == Filter1.Branch).ToList();
                if (Filter1.Division != null)
                    empCareer = empCareer.Where(w => w.Division == Filter1.Division).ToList();
                if (Filter1.Department != null)
                    empCareer = empCareer.Where(w => w.DEPT == Filter1.Department).ToList();
                if (Filter1.EmpType != null)
                    empCareer = empCareer.Where(w => w.EmpType == Filter1.EmpType).ToList();

                var d = (from f in empCareer
                         join emp in Staff on f.EmpCode equals emp.EmpCode
                         where emp.StartDate.Value.Year <= Filter.INYear
                         select new
                         {
                             EmpCode = f.EmpCode,
                             AllName = emp.AllName,
                             Position = emp.Position,
                             StartDate = emp.StartDate,
                             Salary = f.NewSalary
                         }).ToList();
                foreach (var item in d)
                {
                    var _bonus = new BonusAnnual();
                    _bonus.EmpCode = item.EmpCode;
                    _bonus.AllName = item.AllName;
                    _bonus.Position = item.Position;
                    _bonus.StartDate = item.StartDate;
                    _bonus.Salary = item.Salary;
                    _bonus.UnpaidLeave = 0;
                    _bonus.Warning = 0;
                    _bonus.Adding = 0;
                    _bonus.TotalBonus = 0;
                    _bonus.Transfer = "No";
                    _List.Add(_bonus);
                }
            }
            return _List.OrderBy(w => w.Transfer).ToList();
        }
        public string CreateBonus()
        {
            try
            {
                Header = new PREmpBonu();

                if (HeaderStaff.EmpCode == null) return "EMPCODE_EN";
                if (ListHeader.Count == 0) return "LIST_INV";
                var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == HeaderStaff.EmpCode);


                var TranNo = DB.PREmpBonus.OrderByDescending(u => u.TranNo).FirstOrDefault();
                var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "BON").ToList();
                foreach (var read in ListHeader)
                {

                    int status = 0;
                    int FDate = read.FromDate.Value.Month;
                    int TDate = read.ToDate.Value.Month;
                    foreach (var item in RewardType.Where(w => w.Code == read.BonCode).ToList())
                    {
                        read.BonDescription = item.Description;
                    }
                    if (FDate == TDate && read.FromDate.Value.Year == read.ToDate.Value.Year)
                    {
                        status = 1;
                    }
                    read.TranDate = DateTime.Now;
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
                    DB.PREmpBonus.Add(read);
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

        public string EditBonus(int id, string EmpCode)
        {
            try
            {
                Header = new PREmpBonu();
                var check = DB.PREmpBonus.Where(u => u.TranNo == id && u.LCK == 1 && u.EmpCode == EmpCode).ToList();

                if (check.Count > 0) return "ALLLock";
                if (ListHeader.Count == 0) return "LIST_INV";

                var List = DB.PREmpBonus.Where(u => u.TranNo == id && u.EmpCode == EmpCode).ToList();

                foreach (var read in List)
                {
                    DB.PREmpBonus.Remove(read);
                    DB.SaveChanges();
                }
                var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "BON").ToList();
                foreach (var read in ListHeader)
                {
                    foreach (var item in RewardType.Where(w => w.Code == read.BonCode).ToList())
                    {
                        read.BonDescription = item.Description;
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
                    read.TranDate = DateTime.Now;
                    Header.TranNo = id;
                    DB.PREmpBonus.Add(read);
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
                Header = new PREmpBonu();
                var objMatch = DB.PREmpBonus.FirstOrDefault(u => u.TranNo == id && u.EmpCode == EmpCode);

                if (objMatch == null) return "DOC_NE";

                Header.TranNo = id;
                DB.PREmpBonus.Attach(objMatch);
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
        public string ImportBonus()
        {
            try
            {
                var emp = DB.HRStaffProfiles.ToList();
                var TranNo = DB.PREmpBonus.OrderByDescending(u => u.TranNo).FirstOrDefault();
                var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "BON").ToList();
                long TN = 0;
                TN = (TranNo == null) ? 0 : TranNo.TranNo;

                foreach (var read in ListHeader)
                {
                    var empcheck = emp.Where(w => w.EmpCode == read.EmpCode).ToList();
                    var RewardTypecheck = RewardType.Where(w => w.Code == read.BonCode).ToList();
                    if (empcheck.Count() == 0) return "Invalid EmpCode : " + read.EmpCode;
                    if (RewardTypecheck.Count() == 0) return read.EmpCode + " has invalid Bouns Type " + read.BonCode;
                    if (read.FromDate > read.ToDate) return read.EmpCode + " has invalid FromDate";
                    if (read.ToDate < read.FromDate) return read.EmpCode + " has invalid Todate";
                    if (empcheck.Count > 0 && RewardTypecheck.Count > 0)
                    {
                        var obj = new PREmpBonu();
                        int status = 0;
                        int FDate = read.FromDate.Value.Month;
                        int TDate = read.ToDate.Value.Month;
                        read.TranDate = DateTime.Now;
                        if (FDate == TDate && read.FromDate.Value.Year == read.ToDate.Value.Year)
                        {
                            status = 1;
                        }

                        obj.Status = status;
                        obj.LCK = 0;
                        obj.BonCode = read.BonCode;
                        obj.BonDescription = RewardTypecheck.First().Description;
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
                        DB.PREmpBonus.Add(obj);
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
        public string GenerateBonus(int Year)
        {
            try
            {
                var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "BON").ToList();
                var BonusType = RewardType.FirstOrDefault(w => w.Code == Filter.RewardsType);
                //if(BonusType==null)
                //{
                //    return "INVALTID_BONUS";
                //}

                var Temp = DB.Temp_BonusAnnual.Where(w => w.InYear == Year).ToList();
                if (Temp.Count() > 0)
                {
                    foreach (var item in Temp)
                    {
                        DB.Temp_BonusAnnual.Attach(item);
                        DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        var rowD = DB.SaveChanges();
                    }
                }
                var ListStaff = DB.HRStaffProfiles.ToList();
                var ListGenSalary = DB.HISGenSalaries.Where(w => w.INYear == Year).ToList();
                ListStaff = ListStaff.Where(w => w.CareerDesc != "TERMINAT").ToList();
                if (Filter.IsNewJoin == true)
                    ListStaff = ListStaff.Where(w => w.CareerDesc == "NEWJOIN").ToList();
                //   ListGenSalary = ListGenSalary.Where(w=>ListStaff.Where(x=>x.EmpCode==w.EmpCode).Any()).ToList();
                var empBonus = (from f in ListGenSalary
                                join emp in ListStaff on f.EmpCode equals emp.EmpCode
                                group f by new { f.EmpCode, f.Salary }
                     into myGroup
                                where myGroup.Count() > 0
                                select new
                                {
                                    myGroup.Key.EmpCode,
                                    myGroup.Key.Salary,
                                    Bonus = myGroup.Sum(x => x.Salary)
                                }).ToList();
                foreach (var item in empBonus)
                {
                    var _Bonus = new Temp_BonusAnnual();
                    _Bonus.EmpCode = item.EmpCode;
                    _Bonus.InYear = Year;
                    if (BonusType != null)
                    {
                        _Bonus.BonusCode = BonusType.Code;
                        _Bonus.BonusDesc = BonusType.Description;
                    }
                    _Bonus.Salary = item.Salary;
                    _Bonus.Unpaid_leave = 0;
                    _Bonus.Warning = 0;
                    _Bonus.Adding = 0;
                    _Bonus.TotalBonus = Convert.ToDecimal(item.Bonus / 12);
                    _Bonus.Bonus = _Bonus.TotalBonus;
                    _Bonus.S_Month = 0;
                    _Bonus.O_Salary = 0;
                    _Bonus.O_Month = 0;
                    _Bonus.PR_Salary = 0;
                    _Bonus.PR_Month = 0;
                    _Bonus.LOCK = 0;
                    _Bonus.CreatedBy = User.UserName;
                    _Bonus.CreatedOn = DateTime.Now;
                    DB.Temp_BonusAnnual.Add(_Bonus);

                }
                var row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Filter.INYear.ToString();
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
                log.DocurmentAction = Filter.INYear.ToString();
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
                log.DocurmentAction = Filter.INYear.ToString();
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string TransferBonus(string EmpCode, string _BonusType, DateTime InMonth)
        {
            try
            {
                var RewardType = DB.PR_RewardsType.Where(w => w.ReCode == "BON").ToList();
                var BonusType = RewardType.FirstOrDefault(w => w.Code == _BonusType);
                if (BonusType == null)
                {
                    return "INVALTID_BONUS";
                }

                var DBI = new HumicaDBContext();
                var ListEmpBonus = DB.PREmpBonus.ToList();
                var ID = ListEmpBonus.OrderByDescending(u => u.TranNo).FirstOrDefault();
                string[] _TranNo = EmpCode.Split(';');
                DateTime TranDate = new DateTime(InMonth.Year, InMonth.Month, DateTime.DaysInMonth(InMonth.Year, InMonth.Month));
                var TempBA = DB.Temp_BonusAnnual.Where(w => w.InYear == InMonth.Year).ToList();
                if (TempBA.Count > 0)
                {
                    foreach (var Code in _TranNo)
                    {
                        var _TBA = TempBA.Where(w => w.EmpCode == Code).ToList();
                        if (_TBA.Count > 0)
                        {
                            var ObjUpdate = _TBA.First();
                            var _empBonus = ListEmpBonus.FirstOrDefault(w => w.EmpCode == Code && w.BonCode == BonusType.Code);
                            if (_empBonus != null)
                            {
                                DBI.PREmpBonus.Attach(_empBonus);
                                DBI.Entry(_empBonus).State = System.Data.Entity.EntityState.Deleted;
                            }
                            Header = new PREmpBonu();
                            Header.TranNo = ID.TranNo + 1;
                            Header.EmpCode = Code;
                            Header.BonCode = BonusType.Code;
                            Header.BonDescription = BonusType.Description;
                            Header.InYear = InMonth.Year;
                            Header.InMonth = InMonth.Month;
                            Header.TranDate = TranDate;
                            Header.FromDate = new DateTime(TranDate.Year, TranDate.Month, 1);
                            Header.ToDate = TranDate;
                            Header.Remark = "Transfer from bonus annual";
                            Header.Amount = Convert.ToDecimal(ObjUpdate.TotalBonus);
                            ObjUpdate.LOCK = 1;
                            ObjUpdate.REMARK = TranDate.ToString("dd.MM.yyyy");

                            DB.PREmpBonus.Add(Header);
                            DB.Temp_BonusAnnual.Attach(ObjUpdate);
                            DB.Entry(ObjUpdate).Property(w => w.LOCK).IsModified = true;
                            DB.Entry(ObjUpdate).Property(w => w.REMARK).IsModified = true;
                        }
                    }
                }
                var rowD = DBI.SaveChanges();
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
        //public bool IsExistEmployeeBonus(string EmpCode, string BonCode, int InYear, int InMonth)
        //{
        //    var _list = DB.PREmpBonus.Where(w => w.EmpCode == EmpCode).ToList();
        //    _list = _list.Where(w => w.BonCode == BonCode && w.InYear==InYear && w.InMonth==InMonth).ToList();
        //    if (_list.Count > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        public int IsExistEmployeeBonus(string EmpCode, string BonCode, DateTime FromDate, DateTime ToDate)
        {
            int Result = -1;
            var _list = DB.PREmpBonus.Where(w => w.EmpCode == EmpCode).ToList();
            _list = _list.Where(w => w.BonCode == BonCode &&
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
    public class BonusAnnual
    {
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public DateTime? StartDate { get; set; }
        public string Department { get; set; }
        public string LevelCode { get; set; }
        public decimal? Salary { get; set; }
        public string Position { get; set; }
        public decimal? UnpaidLeave { get; set; }
        public decimal? Warning { get; set; }
        public decimal? Adding { get; set; }
        public decimal? TotalBonus { get; set; }
        public string Transfer { get; set; }
        public string Remark { get; set; }
    }
}


