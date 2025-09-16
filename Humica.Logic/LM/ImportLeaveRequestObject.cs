using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Logic.LM
{
    public class ImportLeaveRequestObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SMSystemEntity DP = new SMSystemEntity();
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public List<HRStaffProfile> ListHeader { get; set; }
        public HREmpLeave HREmpLeave { get; set; }
        public HREmpLeaveD HREmpLeaveD { get; set; }
        public SYRoleGroup RolesGroup { get; set; }
        public SYRoleGroupItem RoleGroupItem { get; set; }
        public List<SYUserGroup> ListUserGroup { get; set; }
        public string MessageCode { get; set; }
        public string EmpCode { get; set; }
        public string AllName { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string LeaveCode { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public decimal LHour { get; set; }
        public double WorkHourPerDay { get; set; }
        public ImportLeaveRequestObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<ImportLeaveRequestObject> LeaveImportItem { get; set; }

        public string GenerateLeaveImport()
        {
            try
            {
                DB = new HumicaDBContext();
                DB.Configuration.AutoDetectChangesEnabled = false;

                if (LeaveImportItem.Count == 0)
                {
                    return "NO_DATA";
                }

                int Line = 0;
                int Increment = GenerateLeaveObject.GetLastIncrement();
                var lisEmpLeave = new List<HREmpLeaveD>();
                foreach (var Emp in LeaveImportItem.FindAll(x => String.IsNullOrEmpty(x.MessageError)))
                {
                    MessageError = Emp.LeaveDate.Value.ToString("yyyy-MMM-dd") + " : EmpCode:" + Emp.EmpCode;
                    string Reject = SYDocumentStatus.REJECTED.ToString();
                    string Cancel = SYDocumentStatus.CANCELLED.ToString();
                    //var ATSche = DB.ATEmpSchedules.Where(w => w.EmpCode == Emp.EmpCode && w.TranDate == Emp.LeaveDate).ToList();
                    EmpCode = Emp.EmpCode;
                    var lstEmpLeave = DB.HREmpLeaves.Where(w => w.EmpCode == Emp.EmpCode).ToList();
                    Increment = ++Increment;
                    Line = ++Line;
                    var leaveH = lstEmpLeave.Where(w => w.Status != Cancel && w.Status != Reject && w.EmpCode == Emp.EmpCode).ToList();
                    var EmpLUpload = lisEmpLeave.Where(w => w.EmpCode == Emp.EmpCode).ToList();
                    var _EmpLUpload = EmpLUpload.Where(w => w.LeaveDate.Date == Emp.LeaveDate.Value.Date).ToList();
                    SYDataList objList = new SYDataList("LEAVE_TIME");
                    var ListData = objList.ListData.Where(w => w.SelectValue == Emp.Status).ToList();
                    if (!ListData.Any()) return "INVALIDED_STATUS";
                    if (_EmpLUpload.Where(w => w.Remark == Emp.Status).ToList().Count > 0)
                    {
                        return "INV_DATE";
                    }
                    else if (_EmpLUpload.Where(w => w.Remark == "FullDay").Any())
                    {
                        return "INV_DATE";
                    }
                    if (leaveH.Where(w => w.FromDate.Date <= Emp.LeaveDate.Value.Date && w.ToDate.Date >= Emp.LeaveDate.Value.Date).Any())
                    {
                        var EmpLeaveD = DB.HREmpLeaveDs.Where(w => w.EmpCode == Emp.EmpCode).ToList();
                        var _EmpLeaveD = EmpLeaveD.Where(w => w.LeaveDate.Date == Emp.LeaveDate.Value.Date).ToList();
                        //_EmpLeaveD = _EmpLeaveD.Where(w => w.Remark == Emp.Status).ToList();
                        if (_EmpLeaveD.Where(w => w.Remark == Emp.Status).ToList().Count > 0)
                        {
                            return "INV_DATE";
                        }
                        else if (_EmpLeaveD.Where(w => w.Remark == "FullDay").Any())
                        {
                            return "INV_DATE";
                        }
                        #region Close
                        //EmpLeaveD = EmpLeaveD.Where(w => w.LeaveDate.Date == Emp.LeaveDate.Value.Date).ToList();
                        //var Result = LeaveImportItem.Where(w => EmpLeaveD.Where(x => x.LeaveDate.Date == w.LeaveDate.Value.Date && w.Status != "Hours").Any()).ToList();
                        //var Result_ = LeaveImportItem.Where(w => EmpLeaveD.Where(x => x.LeaveDate.Date == w.LeaveDate.Value.Date && w.Status != "FullDay").Any()).ToList();
                        //var ResultHour = LeaveImportItem.Where(w => EmpLeaveD.Where(x => x.LeaveDate.Date == w.LeaveDate.Value.Date && (x.Remark == "Hours" || w.Status == "Hours")).Any()).ToList();
                        //var empATSche = ATSche.Where(w => ResultHour.Where(x => w.TranDate == x.LeaveDate && w.EmpCode == Emp.EmpCode).Any()).ToList();
                        //if (Result.Where(w => w.Status == "FullDay").Any())
                        //{
                        //    if (ResultHour.Count > 0) return "INV_DATE";
                        //}
                        //if (EmpLeaveD.Where(w => Result.Where(x => x.LeaveDate.Value.Date == w.LeaveDate.Date && x.Status == w.Remark).Any()).Any()) return "INV_DATE";
                        //if (EmpLeaveD.Where(w => ResultHour.Where(x => x.LeaveDate.Value.Date == w.LeaveDate.Date).Any()).Any())
                        //{
                        //    if (empATSche.Count > 0)
                        //    {
                        //        foreach (var read in empATSche)
                        //        {
                        //            TimeSpan time = new TimeSpan(0, 4, 0, 0);
                        //            DateTime brackstart = read.IN1.Value.Add(time);
                        //            DateTime brackend = read.OUT1.Value.Subtract(time);
                        //            if (read.Flag1 == 1 && read.Flag2 == 1 && ResultHour.Count > 0)
                        //            {
                        //                if (EmpLeaveD.Where(w => Result_.Where(x => x.LeaveDate.Value.Date == read.TranDate.Date && ((read.IN1 >= w.StartTime && read.IN1 <= w.EndTime)
                        //                                    || (read.OUT1 >= w.StartTime && read.OUT1 <= w.EndTime) || (w.StartTime >= read.IN1 && w.StartTime <= read.OUT1)
                        //                                    || (w.EndTime >= read.IN1 && w.EndTime <= read.OUT1)) && x.Status == "Morning").Any()).Any()) return "INV_DATE" + ": " + Emp.EmpCode;
                        //                else if (EmpLeaveD.Where(w => Result_.Where(x => x.LeaveDate.Value.Date == read.TranDate.Date && ((read.IN2 >= w.StartTime && read.IN2 <= w.EndTime)
                        //                                    || (read.OUT2 >= w.StartTime && read.OUT2 <= w.EndTime) || (w.StartTime >= read.IN2 && w.StartTime <= read.OUT2)
                        //                                    || (w.EndTime >= read.IN2 && w.EndTime <= read.OUT2)) && x.Status == "Afternoon").Any()).Any()) return "INV_DATE" + ": " + Emp.EmpCode;
                        //            }
                        //            else if (read.Flag1 == 1 && read.Flag2 == 2 && ResultHour.Count > 0)
                        //            {
                        //                if (EmpLeaveD.Where(w => Result_.Where(x => x.LeaveDate.Value.Date == read.TranDate.Date && ((read.IN1 >= w.StartTime && read.IN1 <= w.EndTime)
                        //                                    || (brackstart >= w.StartTime && brackstart <= w.EndTime) || (w.StartTime >= read.IN1 && w.StartTime <= brackstart)
                        //                                    || (w.EndTime >= read.IN1 && w.EndTime <= brackstart)) && x.Status == "Morning").Any()).Any()) return "INV_DATE" + ": " + Emp.EmpCode;
                        //                else if (EmpLeaveD.Where(w => Result_.Where(x => x.LeaveDate.Value.Date == read.TranDate.Date && ((brackend >= w.StartTime && brackend <= w.EndTime)
                        //                                    || (read.OUT2 >= w.StartTime && read.OUT2 <= w.EndTime) || (w.StartTime >= brackend && w.StartTime <= read.OUT2)
                        //                                    || (w.EndTime >= brackend && w.EndTime <= read.OUT2)) && x.Status == "Afternoon").Any()).Any()) return "INV_DATE" + ": " + Emp.EmpCode;
                        //            }
                        //        }
                        //    }
                        //}
                        //if (EmpLeaveD.Where(w => w.Remark == "FullDay").Any())
                        //{
                        //    if (ResultHour.Count > 0) return "INV_DATE";
                        //    if (Result.Count > 0) return "INV_DATE";
                        //}
                        #endregion
                    }
                    HREmpLeave = new HREmpLeave()
                    {
                        Status = SYDocumentStatus.APPROVED.ToString(),
                        RequestDate = DateTime.Now,
                        Increment = Increment,
                        CreateBy = User.UserName,
                        CreateOn = DateTime.Now,
                        EmpCode = Emp.EmpCode,
                        LeaveCode = Emp.LeaveCode,
                        FromDate = Emp.LeaveDate.Value,
                        ToDate = Emp.LeaveDate.Value,
                        NoDay = Emp.WorkHourPerDay <= 0 ? 0 : ((double)Emp.LHour / Emp.WorkHourPerDay),
                        Reason = Emp.Reason,
                        Units= "Day",
                    };
                    //add record into leave detail
                    HREmpLeaveD = new HREmpLeaveD()
                    {
                        LeaveTranNo = Increment,
                        EmpCode = Emp.EmpCode,
                        LeaveCode = Emp.LeaveCode,
                        LeaveDate = Emp.LeaveDate.Value,
                        CutMonth = Emp.LeaveDate.Value,
                        Status = SYDocumentStatus.Leave.ToString(),
                        LineItem = Line,
                        LHour = Emp.LHour,
                        CreateBy = User.UserName,
                        CreateOn = DateTime.Now,
                        Remark = Emp.Status
                    };
                    DB.HREmpLeaveDs.Add(HREmpLeaveD);
                    DB.HREmpLeaves.Add(HREmpLeave);
                    lisEmpLeave.Add(HREmpLeaveD);
                    // update attendance schedule
                    var listEmpAtt = DB.ATEmpSchedules.Where(w => w.EmpCode == Emp.EmpCode &&
                                  EntityFunctions.TruncateTime(w.TranDate) >= Emp.LeaveDate &&
                                  EntityFunctions.TruncateTime(w.TranDate) <= Emp.LeaveDate).ToList();
                    if (listEmpAtt.Count > 0)
                    {
                        var empAtt = listEmpAtt.Where(x => x.EmpCode == HREmpLeaveD.EmpCode &&
                        x.TranDate.Date == HREmpLeaveD.LeaveDate.Date).FirstOrDefault();
                        empAtt.LeaveDesc = "";
                        empAtt.LeaveCode = HREmpLeaveD.LeaveCode;
                        empAtt.LeaveNo = HREmpLeaveD.LeaveTranNo;
                        DB.ATEmpSchedules.Attach(empAtt);
                        DB.Entry(empAtt).Property(x => x.LeaveDesc).IsModified = true;
                        DB.Entry(empAtt).Property(x => x.LeaveCode).IsModified = true;
                        DB.Entry(empAtt).Property(x => x.LeaveNo).IsModified = true;
                    }
                }
                DB.SaveChanges();
                foreach (var Emp in LeaveImportItem.FindAll(x => String.IsNullOrEmpty(x.MessageError)))
                {
                    GenerateLeaveObject generateLeave = new GenerateLeaveObject();
                    generateLeave.GET_Leave_LeaveBalance(EmpCode, Emp.LeaveDate.Value.Year);
                }
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = EmpCode;
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
                log.DocurmentAction = EmpCode;
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
                log.DocurmentAction = EmpCode;
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

    }
}
