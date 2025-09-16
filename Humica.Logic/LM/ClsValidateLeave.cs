using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Humica.Logic.LM
{
    public class ClsValidateLeave
    {
        protected IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsValidateLeave()
        {
            OnLoad();
        }
        public List<HREmpLeaveD> Get_LeaveDay (string EmpCode, DateTime FromDate, DateTime ToDate, string LeaveCode,string Units)
        {
            HRLeaveType _LeaveType = unitOfWork.Repository<HRLeaveType>().FirstOrDefault(w => w.Code == LeaveCode);
            var ListAttendance = unitOfWork.Repository<ATEmpSchedule>().Queryable().Where(w => w.EmpCode == EmpCode &&
                  DbFunctions.TruncateTime(w.TranDate) >= DbFunctions.TruncateTime(FromDate) 
                  && DbFunctions.TruncateTime(w.TranDate) <= DbFunctions.TruncateTime(ToDate));
            var Listpub = unitOfWork.Repository<HRPubHoliday>().Queryable()
                .Where(w => DbFunctions.TruncateTime(w.PDate) >= DbFunctions.TruncateTime(FromDate) 
                && DbFunctions.TruncateTime(w.PDate) <= DbFunctions.TruncateTime(ToDate));
            HRStaffProfile _staff = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == EmpCode);
            PRParameter _param = unitOfWork.Repository <PRParameter>().FirstOrDefault(w => w.Code == _staff.PayParam);
            string remark = "Morning";
            List<HREmpLeaveD> _ListLeave = new List<HREmpLeaveD>();
            int Line = 0;
            for (DateTime day = FromDate.Date; day.Date <= ToDate.Date; day = day.AddDays(1))
            {
                HREmpLeaveD Leaved = new HREmpLeaveD();

                Leaved.Status = SYDocumentStatus.Leave.ToString();
                var objSchedule = ListAttendance.FirstOrDefault(w => DbFunctions.TruncateTime(w.TranDate) == DbFunctions.TruncateTime(day));
                if (objSchedule != null)
                {
                    if (objSchedule.SHIFT == "OFF")
                    {
                        Leaved.Status = SYDocumentStatus.Rest.ToString();
                    }
                    else if (objSchedule.SHIFT == "PH")
                    {
                        Leaved.Status = SYDocumentStatus.PH.ToString();
                    }
                }
                else
                {
                    if (_LeaveType.IncPub.HasValue && _LeaveType.IncPub.Value == true)
                    {
                        if (Listpub.Where(w => DbFunctions.TruncateTime(w.PDate) == DbFunctions.TruncateTime(day)).Count() > 0)
                        {
                            Leaved.Status = SYDocumentStatus.PH.ToString();
                        }
                    }
                    else if(_LeaveType.InRest.HasValue && _LeaveType.InRest.Value == true)
                    {
                        if (_param != null)
                        {
                            if (_param.WDMON != true && day.DayOfWeek == DayOfWeek.Monday && _LeaveType.InRest == true)
                                Leaved.Status = SYDocumentStatus.Rest.ToString();
                            else if (_param.WDTUE != true && day.DayOfWeek == DayOfWeek.Tuesday && _LeaveType.InRest == true)
                                Leaved.Status = SYDocumentStatus.Rest.ToString();
                            else if (_param.WDWED != true && day.DayOfWeek == DayOfWeek.Wednesday && _LeaveType.InRest == true)
                                Leaved.Status = SYDocumentStatus.Rest.ToString(); 
                            else if (_param.WDTHU != true && day.DayOfWeek == DayOfWeek.Thursday && _LeaveType.InRest == true)
                                Leaved.Status = SYDocumentStatus.Rest.ToString();
                            else if (_param.WDFRI != true && day.DayOfWeek == DayOfWeek.Friday && _LeaveType.InRest == true)
                                Leaved.Status = SYDocumentStatus.Rest.ToString();
                            else if (_param.WDSAT != true && day.DayOfWeek == DayOfWeek.Saturday && _LeaveType.InRest == true)
                                Leaved.Status = SYDocumentStatus.Rest.ToString();
                            else if (_param.WDSUN != true && day.DayOfWeek == DayOfWeek.Sunday && _LeaveType.InRest == true) 
                                Leaved.Status = SYDocumentStatus.Rest.ToString();

                        }
                    }
                     
                }
                Leaved.LeaveDate = day;
                Leaved.CutMonth = day;
                if (_LeaveType.IsParent == true && _LeaveType.Amount < 1)
                {
                    Leaved.Remark = remark;
                }
                else
                {
                    Leaved.Remark = "FullDay";
                }
                if (_param.WDSAT == true && day.DayOfWeek == DayOfWeek.Saturday && _LeaveType.InRest == true && _param.WDSATDay == 0.5M)
                {
                    Leaved.Remark = remark;
                }
                if (Units != "Day")
                {
                    Leaved.StartTime = new DateTime(day.Year, day.Month, day.Day, 8, 0, 0);
                    Leaved.EndTime = new DateTime(day.Year, day.Month, day.Day, 9, 0, 0);
                    var totals = Leaved.EndTime.Value.Subtract(Leaved.StartTime.Value).TotalHours;
                    Leaved.LHour = (decimal)totals;
                    var para = unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
                    if (para != null && para.LHourByRate == true)
                    {
                        var totalMin = (Leaved.EndTime.Value - Leaved.StartTime.Value).TotalMinutes;
                        var LPolicy = unitOfWork.Repository<HRLeaveHourPolicy>().FirstOrDefault(w => w.From < totalMin && w.To >= totalMin);
                        if (LPolicy != null) Leaved.LHour = LPolicy.Rate / 60m;
                    }
                    Leaved.Remark = "Hours";
                }
                Line++;
                Leaved.LineItem = Line;
                Leaved.LHour = (decimal)_param.WHOUR;
                _ListLeave.Add(Leaved);
            }
            return _ListLeave;
        }
    }
}
