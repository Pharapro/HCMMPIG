using Humica.Core;
using Humica.Core.DB;
using Humica.Core.Helper;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.Att;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Attendance
{
    public class ClsAttendance
    {
        public void Validate_Late_Early(ATEmpSchedule result, HRStaffProfile Staff, List<ATPolicyLaEa> ListLa_Ea)
        {
            int _late1 = 0;
            int _Early1 = 0;
            int _late2 = 0;
            int _Early2 = 0;
            if (ListLa_Ea.Where(w => w.Branch == Staff.Branch).Any())
            {
                var tempLata = ListLa_Ea.Where(w => w.Branch == Staff.Branch).FirstOrDefault();
                _late1 = tempLata.Late1;
                _Early1 = tempLata.Early1;
                _late2 = tempLata.Late2;
                _Early2 = tempLata.Early2;
            }
            if (result.Flag1 == 1)
            {
                if (result.Late1.Value > 0)
                {
                    var Start1 = result.IN1.Value.AddMinutes(_late1);
                    if (Start1 > result.Start1)
                    {
                        if (result.Late1.Value <= _late1) result.Late1 = 0;
                    }
                }
            }
            if (result.Early1.Value <= _Early1) result.Early1 = 0;
            if (result.Late2 <= _late2) result.Late2 = 0;
            if (result.Early2 <= _late2) result.Early2 = 0;
        }
        public ATEmpSchedule GenrateAttendance(ATEmpSchedule result, string EmpCode,
            ATPolicy NWPolicy, string LeaveDesc,
            List<HRPubHoliday> ListPub, List<ATShift> ListSHift,
            List<HREmpLeaveD> ListLeaveD, List<HRLeaveType> _LeaveType)
        {
            DateTime NWFromDate = result.TranDate + NWPolicy.NWFROM.Value.TimeOfDay;
            DateTime NWToDate = result.TranDate + NWPolicy.NWTO.Value.TimeOfDay;
            ATShift _Shift = ListSHift.FirstOrDefault(w => w.Code == result.SHIFT);
            result = Attendance_New(result);
            if (result.Flag1 == 1)
            {
                if (ListLeaveD.Where(w => w.LeaveDate.Date == result.TranDate.Date && w.EmpCode == EmpCode).Any())
                {
                    var Leave = ListLeaveD.Where(w => w.LeaveDate == result.TranDate && w.EmpCode == EmpCode).FirstOrDefault();
                    result.LeaveNo = Leave.LeaveTranNo;
                    result.LeaveCode = Leave.LeaveCode;
                    result.LeaveDesc = Leave.LeaveCode;
                }
                var Atten = ValidateAttendance(result.LeaveCode, result.IN1.Value, result.OUT1.Value,
                            result.Start1, result.End1, _Shift,
                            NWFromDate, NWToDate, result.Flag1.Value, result.Flag2.Value);
                result.LeaveDesc = Atten.Leave;
                result.GEN = Atten.GEN;
                result.Late1 = Atten.Late;
                result.Early1 = Atten.Early;
                result.WHOUR = Atten.WHour;
                result.NWH = Atten.NWH;
                result.WOT = Atten.WOT;
                result.WOTMin = Atten.OTMin;
            }
            if (result.Flag2 == 1)
            {
                var Atten = ValidateAttendance(result.LeaveCode, result.IN2.Value, result.OUT2.Value,
                result.Start2, result.End2, _Shift, NWFromDate, NWToDate, result.Flag1.Value, result.Flag2.Value);
                result.LeaveDesc = Atten.Leave;
                result.GEN = Atten.GEN;
                result.Late2 = Atten.Late;
                result.Early2 = Atten.Early;
                result.WHOUR += Atten.WHour;
                result.NWH = Atten.NWH;
                result.WOT = Atten.WOT;
                result.WOTMin = Atten.OTMin;
            }

            if (result.Flag1 == 2 && result.Flag2 == 2)
            {
                if (result.Start1.Value.Year != 1900 && result.End1.Value.Year != 1900)
                {
                    var interval = result.End1.Value.Subtract(result.Start1.Value).TotalHours;
                    result.WHOUR = Math.Round(Convert.ToDecimal(interval), 2);
                    result.WOT = result.WHOUR;
                    result.GEN = true;
                }
            }
            if (result.Flag1 == 1 && result.Flag2 == 2)
            {
                if (result.Start1.Value.Year == 1900 && result.End1.Value.Year == 1900)
                    result.LeaveDesc = "ABS";
            }
            else if (result.Flag1 == 1 && result.Flag2 == 1)
            {
                if (result.Start1.Value.Year == 1900 && result.End1.Value.Year == 1900 &&
                    result.Start2.Value.Year == 1900 && result.End2.Value.Year == 1900)
                    result.LeaveDesc = "ABS";
            }
            if (!string.IsNullOrEmpty(result.LeaveCode))
            {
                result.LeaveDesc = "";
            }
            return result;
        }
        public ATEmpSchedule GetResult(ATEmpSchedule emp_sch, List<ATShift> ListShift, List<ATInOut> raws, bool IsOT, ATPolicy aTPolicy, List<HRRequestOT> OTReq)
        {
            ListShift = ListShift.ToList();
            DateTime Checkdate = new DateTime(1900, 1, 1);
            var attendancexTemp = new ATEmpSchedule();
            attendancexTemp = new ATEmpSchedule()
            {
                Start1 = Checkdate,
                End1 = Checkdate,
                Start2 = Checkdate,
                End2 = Checkdate,
            };
            if (emp_sch.SHIFT.ToUpper() == "PH" || emp_sch.SHIFT.ToUpper() == "OFF")
            {
                DateTime TranDate = emp_sch.TranDate;
                DateTime TranDate_ = emp_sch.TranDate;
                if (IsOT)
                {
                    var OTRest = OTReq.OrderByDescending(w => w.OTEndTime).FirstOrDefault();
                    var P1 = OTRest.OTStartTime.AddHours(-2);
                    var P2 = OTRest.OTStartTime.AddHours(2);
                    var P3 = OTRest.OTEndTime.AddHours(-3);
                    var P4 = OTRest.OTEndTime.AddHours(3);
                    var r = raws.Where(x => x.FullDate >= P1 && x.FullDate <= P4 && x.LCK != 1).ToList();
                    attendancexTemp.Start1 = Check_Scan_Attendance(r, P1, P2);
                    if (r.Where(w => w.FullDate >= P3 && w.FullDate <= P4 && w.LCK != 1).Any())
                    {
                        attendancexTemp.End1 = Check_Scan_Attendance(r, P3, P4, false);
                    }
                }
                else
                {

                    DateTime P1 = new DateTime(TranDate.Year, TranDate.Month, TranDate.Day, 0, 0, 0);
                    DateTime P2 = new DateTime(TranDate_.Year, TranDate_.Month, TranDate_.Day, 23, 59, 0);
                    var r = raws.Where(x => x.FullDate >= P1 && x.FullDate <= P2).ToList().OrderBy(x => x.FullDate);
                    if (r.ToList().Count > 0)
                    {
                        attendancexTemp.Start1 = r.First().FullDate;
                        attendancexTemp.Start1 = DateTimeHelper.DateInHourMin(attendancexTemp.Start1.Value);
                        if (r.ToList().Count > 1)
                        {
                            attendancexTemp.End1 = r.LastOrDefault().FullDate;
                            attendancexTemp.End1 = DateTimeHelper.DateInHourMin(attendancexTemp.End1.Value);
                        }

                    }
                }
                emp_sch.Start1 = attendancexTemp.Start1;
                emp_sch.End1 = attendancexTemp.End1;
                if (IsOT == true)
                {
                    emp_sch.OTStart = attendancexTemp.Start1;
                    emp_sch.OTEnd = attendancexTemp.End1;
                }
            }
            else
            {
                var _shift = ListShift.Where(w => w.Code == emp_sch.SHIFT);
                foreach (var shift in _shift)
                {
                    if (shift != null)
                    {
                        DateTime InDate = emp_sch.TranDate;
                        emp_sch.IN1 = DateTimeHelper.DateInHourMin(InDate.Date + shift.CheckIn1.Value.TimeOfDay);
                        emp_sch.OUT1 = DateTimeHelper.DateInHourMin(InDate.Date + shift.CheckOut1.Value.TimeOfDay);
                        if (shift.OverNight1 == true) emp_sch.OUT1 = emp_sch.OUT1.Value.AddDays(1);
                        if (shift.SplitShift == true && shift.CheckIn2.HasValue && shift.CheckOut2.HasValue)
                        {
                            emp_sch.Flag2 = 1;
                            emp_sch.IN2 = DateTimeHelper.DateInHourMin(InDate.Date + shift.CheckIn2.Value.TimeOfDay);
                            emp_sch.OUT2 = DateTimeHelper.DateInHourMin(InDate.Date + shift.CheckOut2.Value.TimeOfDay);
                            if (shift.OverNight1 == true)
                            {
                                emp_sch.IN2 = emp_sch.IN2.Value.AddDays(1);
                                emp_sch.OUT2 = emp_sch.OUT2.Value.AddDays(1);
                            }
                            else if (shift.OverNight2 == true) emp_sch.OUT2 = emp_sch.OUT2.Value.AddDays(1);
                        }
                        emp_sch.Flag1 = 1;
                    }

                    DateTime P1 = new DateTime();
                    DateTime P2 = new DateTime();
                    DateTime P3 = new DateTime();
                    DateTime P4 = new DateTime();
                    DateTime P5 = new DateTime();
                    DateTime P6 = new DateTime();
                    DateTime P7 = new DateTime();
                    DateTime P8 = new DateTime();
                    attendancexTemp = emp_sch;
                    if (emp_sch.Flag1 == 1 && emp_sch.Flag2 == 1)
                    {
                        if (shift.CheckInBefore1.HasValue && shift.CheckInAfter1.HasValue && shift.CheckOutBefore1.HasValue && shift.CheckOutAfter1.HasValue
                            && shift.CheckInBefore2.HasValue && shift.CheckInAfter2.HasValue && shift.CheckOutBefore2.HasValue && shift.CheckOutAfter2.HasValue)
                        {
                            P1 = emp_sch.IN1.Value.Date + shift.CheckInBefore1.Value.TimeOfDay;
                            P2 = emp_sch.IN1.Value.Date + shift.CheckInAfter1.Value.TimeOfDay;
                            P3 = emp_sch.OUT1.Value.Date + shift.CheckOutBefore1.Value.TimeOfDay;
                            P4 = emp_sch.OUT1.Value.Date + shift.CheckOutAfter1.Value.TimeOfDay;
                            P5 = emp_sch.IN2.Value.Date + shift.CheckInBefore2.Value.TimeOfDay;
                            P6 = emp_sch.IN2.Value.Date + shift.CheckInAfter2.Value.TimeOfDay;
                            P7 = emp_sch.OUT2.Value.Date + shift.CheckOutBefore2.Value.TimeOfDay;
                            P8 = emp_sch.OUT2.Value.Date + shift.CheckOutAfter2.Value.TimeOfDay;
                            if (shift.CheckInAfter2.Value.TimeOfDay > shift.CheckOutAfter2.Value.TimeOfDay)
                            {
                                P7 = emp_sch.IN2.Value.Date + shift.CheckOutBefore2.Value.TimeOfDay;
                            }
                        }
                        else
                        {
                            P1 = emp_sch.IN1.Value.AddMinutes(-300);
                            int Min2 = Convert.ToInt32(emp_sch.OUT1.Value.Subtract(emp_sch.IN1.Value).TotalMinutes / 2);
                            int Min3 = Convert.ToInt32(emp_sch.IN2.Value.Subtract(emp_sch.OUT1.Value).TotalMinutes / 2);
                            int Min4 = Convert.ToInt32(emp_sch.OUT2.Value.Subtract(emp_sch.IN2.Value).TotalMinutes / 2);
                            P2 = emp_sch.IN1.Value.AddMinutes(Min2);
                            P3 = P2;
                            P4 = emp_sch.OUT1.Value.AddMinutes(Min3);

                            P5 = emp_sch.OUT1.Value.AddMinutes(Min3);
                            P6 = emp_sch.IN2.Value.AddMinutes(Min4);
                            P7 = P6;
                            P8 = emp_sch.OUT2.Value.AddMinutes(300);
                        }
                    }
                    else
                    {
                        if (shift.CheckInBefore1.HasValue && shift.CheckInAfter1.HasValue
                            && shift.CheckOutBefore1.HasValue && shift.CheckOutAfter1.HasValue)
                        {
                            P1 = emp_sch.IN1.Value.Date + shift.CheckInBefore1.Value.TimeOfDay;
                            P2 = emp_sch.IN1.Value.Date + shift.CheckInAfter1.Value.TimeOfDay;
                            P3 = emp_sch.OUT1.Value.Date + shift.CheckOutBefore1.Value.TimeOfDay;
                            P4 = emp_sch.OUT1.Value.Date + shift.CheckOutAfter1.Value.TimeOfDay;
                            if (shift.OverNight1 == true)
                            {
                                if (P1 > P2)
                                {
                                    P2 = P2.AddDays(1);
                                }
                            }
                            else if (P3 > P4)
                            {
                                P4 = P4.AddDays(1);
                            }
                        }
                        else
                        {
                            P1 = emp_sch.IN1.Value.AddMinutes(-300);
                            P4 = emp_sch.OUT1.Value.AddMinutes(300);
                            int TotalMin = Convert.ToInt32(emp_sch.OUT1.Value.Subtract(emp_sch.IN1.Value).TotalMinutes / 2);
                            P2 = emp_sch.IN1.Value.AddMinutes(TotalMin);
                            P3 = P2;
                        }
                    }
                    if (emp_sch.Flag1 == 1)
                    {
                        var r = raws.Where(x => x.FullDate >= P1 && x.FullDate <= P4 && x.LCK != 1).ToList();
                        attendancexTemp.Start1 = Check_Scan_Attendance(r, P1, P2);
                        if (r.Where(w => w.FullDate >= P3 && w.FullDate <= P4 && w.LCK != 1).Any())
                        {
                            ATInOut _INOut = new ATInOut();
                            if (IsOT == true)
                            {
                                DateTime temp_End = attendancexTemp.OUT1.Value;
                                attendancexTemp.End1 = Check_Scan_Attendance(r, P3, P4);
                                attendancexTemp.OTStart = Check_Scan_OT(r, temp_End, P4);
                                attendancexTemp.OTEnd = Check_Scan_OT(r, temp_End, P4, false);
                            }
                            else
                            {
                                //if (emp_sch.Flag2 != 1)
                                attendancexTemp.End1 = Check_Scan_Attendance(r, P3, P4, false);
                                //else
                                //    attendancexTemp.End1 = Check_Scan_Attendance(r, P3, P4);
                            }
                        }
                        else
                        {
                            if (aTPolicy.BaseOnScan == true)
                            {
                                var P9 = attendancexTemp.IN1.Value.AddMinutes(aTPolicy.AfterScan.Value);
                                r = raws.Where(x => x.FullDate >= P9 && x.FullDate <= P2 && x.LCK != 1).ToList();
                                attendancexTemp.End1 = Check_Scan_Attendance(r, P9, P2, false);
                            }
                            else
                            {
                                attendancexTemp.End1 = new DateTime(1900, 1, 1);
                            }
                        }
                        if (attendancexTemp.Start1.Value.Year == 1900)
                        {
                            if (aTPolicy.BaseOnScan == true && attendancexTemp.End1.Value.Year != 1900)
                            {
                                var P9 = attendancexTemp.OUT1.Value.AddMinutes(-60);
                                r = raws.Where(x => x.FullDate >= P3 && x.FullDate < P9 && x.LCK != 1).ToList();
                                attendancexTemp.Start1 = Check_Scan_Attendance(r, P3, P9, false);
                            }
                        }
                    }
                    if (emp_sch.Flag2 == 1)
                    {
                        var r = raws.Where(x => x.FullDate >= P5 && x.FullDate <= P8 && x.LCK != 1).ToList();
                        attendancexTemp.Start2 = Check_Scan_Attendance(r, P5, P6);
                        if (r.Where(w => w.FullDate >= P7 && w.FullDate <= P8 && w.LCK != 1).Any())
                        {
                            if (IsOT == true)
                            {
                                DateTime temp_End = attendancexTemp.OUT2.Value;
                                attendancexTemp.End2 = Check_Scan_Attendance(r, P7, P8);
                                attendancexTemp.OTStart = Check_Scan_OT(r, temp_End, P8);
                                attendancexTemp.OTEnd = Check_Scan_OT(r, temp_End, P8, false);
                            }
                            else
                            {
                                attendancexTemp.End2 = Check_Scan_Attendance(r, P7, P8, false);
                            }
                        }
                        else
                        {
                            if (aTPolicy.BaseOnScan == true)
                            {
                                var P9 = attendancexTemp.IN2.Value.AddMinutes(aTPolicy.AfterScan.Value);
                                r = raws.Where(x => x.FullDate >= P9 && x.FullDate <= P6 && x.LCK != 1).ToList();
                                attendancexTemp.End2 = Check_Scan_Attendance(r, P9, P6, false);
                            }
                            else
                            {
                                attendancexTemp.End2 = new DateTime(1900, 1, 1);
                            }
                        }
                    }
                }
            }
            raws.Where(w => w.LCK == 1).ToList().ForEach(x => x.LCK = 0);
            return emp_sch;
        }
        public ATLateEarlyDeduct AddLateEarly(ATLateEarlyDeduct EmpLateEalry, string DeductType,
            decimal BeforeAmount, decimal Amount, string Remark, string StrMissScanIN, ATEmpSchedule item,
            ClsMissScan _MissScan)
        {
            if (Amount == 0)
                EmpLateEalry.IsMissScan = 1;
            else EmpLateEalry.IsMissScan = 0;
            EmpLateEalry.Remark1 = Remark;

            EmpLateEalry.Remark = StrMissScanIN;
            EmpLateEalry.EmpCode = item.EmpCode;
            EmpLateEalry.DeductType = DeductType;
            EmpLateEalry.DocumentDate = item.TranDate;
            EmpLateEalry.BeforeAmount = BeforeAmount;
            EmpLateEalry.Amount = Amount;
            EmpLateEalry.LeaveCode = _MissScan.LeaveCode;

            return EmpLateEalry;
        }
        public void IsMissScan(PRParameter _param, HRLeaveType LeaveType, ATEmpSchedule item, ClsMissScan _MissScan, string Type)
        {
            DateTime day = item.TranDate;
            if (item.Flag1 == 1 && Type == "Morning")
            {
                //if (_MissScan.Shift.SplitShift == false)
                //{

                //}
                _MissScan.LeaveCode = LeaveType.Code + "(M)";
                _MissScan.IsMissIN = true;
                _MissScan.IsMissOut = true;
            }
            if (item.Flag2 == 1 && Type == "Afternoon")
            {
                _MissScan.LeaveCode = LeaveType.Code + "(A)";
                _MissScan.IsMissIN2 = true;
                _MissScan.IsMissOut2 = true;
            }
            if (_param.WDSAT == true && day.DayOfWeek == DayOfWeek.Saturday && LeaveType.InRest == true && _param.WDSATDay == 0.5M)
            {
                _MissScan.IsMissIN = true;
                _MissScan.IsMissOut = true;
            }
        }
        public ClsAtt ValidateAttendance(string LeaveCode, DateTime CheckIn, DateTime CheckOut,
           DateTime? StartDate, DateTime? EndDate, ATShift Shift, DateTime NWFromDate, DateTime NWToDate,
           int Flag1, int Flag2)
        {
            ClsAtt Atten = new ClsAtt();
            DateTime TempFromDate = new DateTime();
            DateTime TempToDate = new DateTime();
            if (StartDate.Value.Year != 1900 && EndDate.Value.Year != 1900)
            {
                if (StartDate > CheckIn) Atten.Late = (int)StartDate.Value.Subtract(CheckIn).TotalMinutes;
                if (EndDate < CheckOut) Atten.Early = Math.Abs((int)EndDate.Value.Subtract(CheckOut).TotalMinutes);
                if (EndDate > CheckOut)
                {
                    Atten.WOT = Convert.ToDecimal(Math.Round(EndDate.Value.Subtract(CheckOut).TotalHours, 2));
                    Atten.OTMin = (decimal)EndDate.Value.Subtract(CheckOut).TotalMinutes;
                }
                DateTime Start1 = new DateTime();
                DateTime End1 = new DateTime();
                Start1 = StartDate.Value;
                End1 = EndDate.Value;
                var interval = End1.Subtract(Start1);

                TimeSpan BreakHour = new TimeSpan();
                if (Shift.BreakStart.HasValue && Shift.BreakEnd.HasValue)
                {
                    if (Shift.BreakStart > Shift.BreakEnd) Shift.BreakEnd = Shift.BreakEnd.Value.AddDays(1);
                    BreakHour = Shift.BreakEnd.Value.Subtract(Shift.BreakStart.Value);
                }
                if (Flag1 == 1 && Flag2 == 2)
                {
                    decimal WH = Convert.ToDecimal(interval.Subtract(BreakHour).TotalHours);
                    if (WH > 0)
                        Atten.WHour = Math.Round(WH, 2);
                }
                else
                {
                    decimal WH = Convert.ToDecimal(interval.TotalHours);
                    if (WH > 0)
                        Atten.WHour = Math.Round(WH, 2);
                }
                Atten.GEN = true;
                if (StartDate.Value > NWFromDate) TempFromDate = StartDate.Value;
                else if (StartDate.Value <= NWFromDate) TempFromDate = NWFromDate;
                if (EndDate.Value > NWToDate) TempToDate = NWToDate;
                else if (EndDate.Value <= NWToDate && EndDate <= CheckOut) TempToDate = EndDate.Value;
                else if (EndDate.Value <= NWToDate && CheckOut <= EndDate) TempToDate = CheckOut;
                Atten.NWH = Convert.ToDecimal(TempToDate.Subtract(TempFromDate).TotalHours);
                if (Atten.NWH < 0) Atten.NWH = 0;
            }
            else
            {
                //if (StartDate.Value.Year == 1900 && EndDate.Value.Year == 1900 && LeaveCode == "")
                //{
                //    Atten.Leave = "ABS";
                //    Atten.GEN = true;
                //}
                //else
                if (StartDate.Value.Year != 1900)
                {
                    if (StartDate > CheckIn) Atten.Late = (int)StartDate.Value.Subtract(CheckIn).TotalMinutes;
                    Atten.GEN = true;
                }
                else if (EndDate.Value.Year != 1900)
                {
                    if (EndDate < CheckOut) Atten.Early = Math.Abs((int)EndDate.Value.Subtract(CheckOut).TotalMinutes);
                    if (EndDate > CheckOut)
                    {
                        Atten.WOT = Math.Round(Convert.ToDecimal(EndDate.Value.Subtract(CheckOut).TotalHours), 2);
                        Atten.GEN = true;
                    }
                }
            }

            return Atten;
        }
        public ATEmpSchedule Attendance_New(ATEmpSchedule result)
        {
            result.NWH = 0;
            result.WOT = 0;
            result.OTApproval = 0;
            result.OTRequest = 0;
            result.WOTMin = 0;
            result.WHOUR = 0;
            result.LeaveNo = -1;
            result.LeaveCode = "";
            result.Late1 = 0;
            result.Early1 = 0;
            result.Late2 = 0;
            result.Early2 = 0;
            result.LeaveDesc = "";
            result.OTTYPE = "";
            result.OTReason = "";
            result.GEN = false;
            result.WokingHour = "";
            return result;
        }
        public ATEmpSchedule Cal_Maternity_LateEarly(ATEmpSchedule result, List<HRReqMaternity> _EmpResMater)
        {
            var LaEa = _EmpResMater.Sum(x => x.LateEarly);
            int? Total_Late = result.Late1 + result.Late2;
            int? Total_Ealry = result.Early1 + result.Early2;
            int? Total = LaEa;
            if ((Total - Total_Late) >= 0)
            {
                Total -= (result.Late1 + result.Late2);
                result.Late1 = 0;
                result.Late2 = 0;
            }
            else if (Total - Total_Late < 0)
            {
                if ((Total - result.Late1) >= 0)
                {
                    Total -= result.Late1;
                    result.Late1 = 0;
                    if ((Total - result.Late2) < 0)
                    {
                        result.Late2 -= Total;
                        Total = 0;
                    }
                }
                else if ((Total - result.Late1) < 0)
                {
                    result.Late1 -= Total;
                    Total = 0;
                }
            }
            if (Total > 0)
            {
                if ((Total - result.Early1) >= 0)
                {
                    Total -= result.Early1;
                    result.Early1 = 0;
                    if (result.Early2 > 0)
                    {
                        if ((Total - result.Early2) < 0)
                        {
                            result.Early2 -= Total;
                            Total = 0;
                        }
                        else
                        {
                            Total -= result.Early2;
                            result.Early2 = 0;
                        }
                    }
                }
                else if ((Total - result.Early1) < 0)
                {
                    result.Early1 -= Total;
                    Total = 0;
                }
            }

            if (Total == 0) result.WHOUR += Convert.ToDecimal((LaEa / 60.00));
            else result.WHOUR += Convert.ToDecimal(((LaEa - Total) / 60.00));

            return result;
        }
        public ATEmpSchedule Cal_Req_Late_Early(ATEmpSchedule result, List<HRReqLateEarly> _EmpResLateEalry)
        {
            int? Total_Ealry = _EmpResLateEalry.Where(x => x.RequestType == "EARLY").Sum(w => w.Qty);
            int? Total_Late = _EmpResLateEalry.Where(x => x.RequestType == "LATE").Sum(w => w.Qty);
            if (Total_Late > 0)
            {
                var Total = Total_Late;
                if ((Total - (result.Late1 + result.Late2)) >= 0)
                {
                    Total -= (result.Late1 + result.Late2);
                    result.Late1 = 0;
                    result.Late2 = 0;
                }
                else if (Total - (result.Late1 + result.Late2) < 0)
                {
                    if ((Total - result.Late1) >= 0)
                    {
                        Total -= result.Late1;
                        result.Late1 = 0;
                        if ((Total - result.Late2) < 0)
                        {
                            result.Late2 -= Total;
                            Total = 0;
                        }
                    }
                    else if ((Total - result.Late1) < 0)
                    {
                        result.Late1 -= Total;
                        Total = 0;
                    }
                }

                if (Total == 0) result.WHOUR += Convert.ToDecimal((Total_Late / 60.00));
                else result.WHOUR += Convert.ToDecimal(((Total_Late - Total) / 60.00));
            }
            if (Total_Ealry > 0)
            {
                var Total = Total_Ealry;
                if ((Total - (result.Early1 + result.Early2)) >= 0)
                {
                    Total -= (result.Early1 + result.Early2);
                    result.Early1 = 0;
                    result.Early2 = 0;
                }
                else if (Total - (result.Early1 + result.Early2) < 0)
                {
                    if ((Total - result.Early1) >= 0)
                    {
                        Total -= result.Early1;
                        result.Early1 = 0;
                        if ((Total - result.Early2) < 0)
                        {
                            result.Early2 -= Total;
                            Total = 0;
                        }
                    }
                    else if ((Total - result.Early1) < 0)
                    {
                        result.Early1 -= Total;
                        Total = 0;
                    }
                }

                if (Total == 0) result.WHOUR += Convert.ToDecimal((Total_Ealry / 60.00));
                else result.WHOUR += Convert.ToDecimal(((Total_Ealry - Total) / 60.00));

            }

            return result;
        }

        public DateTime Check_Scan_Attendance(List<ATInOut> raws, DateTime P1, DateTime P2, bool FirstIn = true)
        {
            DateTime InDate = new DateTime(1900, 1, 1);
            if (raws.Where(w => w.FullDate >= P1 && w.FullDate <= P2 && w.LCK != 1).Any())
            {
                ATInOut _INOut = raws.Where(w => w.FullDate >= P1 && w.FullDate <= P2 && w.LCK != 1).OrderBy(x => x.FullDate).FirstOrDefault();
                if (FirstIn == false)
                    _INOut = raws.Where(w => w.FullDate >= P1 && w.FullDate <= P2 && w.LCK != 1).OrderByDescending(x => x.FullDate).FirstOrDefault();
                if (_INOut != null)
                {
                    _INOut.LCK = 1;
                    InDate = DateTimeHelper.DateInHourMin(_INOut.FullDate);
                }
            }
            return InDate;
        }
        public DateTime? Check_Scan_OT(List<ATInOut> raws, DateTime P1, DateTime P2, bool FirstIn = true)
        {
            DateTime? InDate = null;
            if (raws.Where(w => w.FullDate >= P1 && w.FullDate <= P2 && w.LCK != 1).Any())
            {
                ATInOut _INOut = raws.Where(w => w.FullDate >= P1 && w.FullDate <= P2 && w.LCK != 1).OrderBy(x => x.FullDate).FirstOrDefault();
                if (FirstIn == false)
                    _INOut = raws.Where(w => w.FullDate >= P1 && w.FullDate <= P2 && w.LCK != 1).OrderByDescending(x => x.FullDate).FirstOrDefault();
                if (_INOut != null)
                {
                    _INOut.LCK = 1;
                    InDate = DateTimeHelper.DateInHourMin(_INOut.FullDate);
                }
            }
            return InDate;
        }
        public ATEmpSchedule Cal_OT(ATEmpSchedule result, List<HRRequestOT> EmpOTReq, List<ATOTPolicy> ListOTPolic, List<ATInOut> scanInout)
        {
            foreach (var OTR in EmpOTReq)
            {
                OTR.OTStartTime = DateTimeHelper.DateInHourMin(OTR.OTStartTime);
                OTR.OTEndTime = DateTimeHelper.DateInHourMin(OTR.OTEndTime);
                OTR.OTActStart = result.OTStart;
                OTR.OTActEnd = result.OTEnd;
                OTR.OTActual = 0;
                decimal OTClaim = 0;
                if (result.OTStart != null && result.OTEnd != null)
                {
                    DateTime OTStartTime = result.OTStart.Value;
                    DateTime OTEndTime = result.OTEnd.Value;
                    if (result.OTEnd.Value >= OTR.OTEndTime)
                    {
                        OTEndTime = OTR.OTEndTime;
                    }
                    else if (result.OTEnd.Value < OTR.OTEndTime)
                    {
                        OTEndTime = result.OTEnd.Value;
                    }
                    if (result.OTStart.Value <= OTR.OTStartTime)
                    {
                        OTStartTime = OTR.OTStartTime;
                    }
                    else if (result.OTStart.Value > OTR.OTStartTime)
                    {
                        OTStartTime = result.OTStart.Value;
                    }
                    var OTNM = (int)OTEndTime.Subtract(OTStartTime).TotalMinutes - OTR.BreakTime;
                    var _OTR = ListOTPolic.Where(w => w.OTFrom <= OTNM && w.OTTo >= OTNM).ToList();
                    if (_OTR.Count() > 0)
                        OTClaim = ClsRounding.Rounding(_OTR.Sum(w => w.OTHour), 2, "N");
                    else
                    {
                        OTClaim = ClsRounding.Rounding((OTNM) / 60.00M, 2, "N");
                    }
                    if (OTClaim < 0) OTClaim = 0;
                    if (OTClaim > OTR.Hours) OTClaim = OTR.Hours.Value;
                    if (OTClaim != 0)
                    {
                        result.OTStart = OTStartTime;
                        result.OTEnd = OTEndTime;
                        result.OTApproval += OTClaim;
                        result.WOT += OTClaim;
                        OTR.OTActual = OTClaim;
                        OTR.ReferenceNo = Convert.ToInt32(result.TranNo);
                    }
                    result.OTReason = OTR.Reason;
                }
                else if ((result.OTStart == null || result.OTEnd == null) && result.Flag1 == 1 && result.Flag2 == 2)
                {
                    if (result.Start1 == null || result.Start1 == DateTime.MinValue || result.End1 == null || result.End1 == DateTime.MinValue) continue;
                    DateTime OTStartTime = OTR.OTStartTime;
                    DateTime OTEndTime = OTR.OTEndTime;
                    bool isOT = false;
                    if (OTR.OTCategory == "OBS" && result.Start1 < result.IN1)
                    {
                        isOT = true;
                        OTR.OTActStart = OTR.OTStartTime;
                        OTR.OTActEnd = OTR.OTEndTime;
                        if (result.Start1 > OTR.OTStartTime)
                        {
                            OTR.OTActStart = result.Start1;
                            OTStartTime = result.Start1.Value;
                        }
                        if (result.OUT1 < OTR.OTEndTime)
                        {
                            OTR.OTActEnd = result.OUT1;
                            OTEndTime = result.OUT1.Value;
                        }
                    }
                    if (OTR.OTCategory == "OT")
                    {
                        if (result.End1 > result.OUT1 && result.End1 > OTR.OTStartTime)
                        {
                            isOT = true;
                            OTR.OTActStart = OTR.OTStartTime;
                            OTR.OTActEnd = OTR.OTEndTime;
                            if (OTR.OTStartTime < result.OUT1)
                            {
                                OTR.OTActStart = result.OUT1;
                                OTStartTime = result.OUT1.Value;
                            }
                            if (result.End1 < OTR.OTEndTime)
                            {
                                OTR.OTActEnd = result.End1;
                                OTEndTime = result.End1.Value;
                            }
                        }
                        else if (result.End1.Value.Year != 1900)
                        {
                            DateTime TranDate = result.TranDate;
                            DateTime P1 = new DateTime(TranDate.Year, TranDate.Month, TranDate.Day, 0, 0, 0);
                            DateTime P2 = new DateTime(TranDate.Year, TranDate.Month, TranDate.Day, 23, 59, 0);
                            var r = scanInout.Where(x => x.FullDate >= P1 && x.FullDate <= P2).ToList().OrderBy(x => x.FullDate);
                            if (r.Any())
                            {
                                DateTime dateout = r.LastOrDefault().FullDate;
                                if (dateout > OTStartTime && dateout > result.End1)
                                {
                                    isOT = true;
                                    OTR.OTActStart = OTR.OTStartTime;
                                    OTR.OTActEnd = OTR.OTEndTime;
                                    if (OTStartTime < result.End1)
                                    {
                                        OTR.OTActStart = result.End1;
                                        OTStartTime = result.End1.Value;
                                    }
                                    if(dateout< OTR.OTEndTime)
                                    {
                                        OTR.OTActEnd = dateout;
                                        OTEndTime = dateout;
                                    }
                                }
                            }
                        }
                    }
                    if (OTR.OTCategory == "OBT" && result.Start1.Value.Year != 1900 && result.End1.Value.Year != 1900)
                    {
                        isOT = true;
                        OTR.OTActStart = OTR.OTStartTime;
                        OTR.OTActEnd = OTR.OTEndTime;
                        if (OTR.OTStartTime < result.Start1)
                        {
                            OTR.OTActStart = result.Start1;
                            OTStartTime = result.Start1.Value;
                        }
                        if (result.End1 < OTR.OTEndTime)
                        {
                            OTR.OTActEnd = result.End1;
                            OTEndTime = result.End1.Value;
                        }
                    }
                    if (isOT)
                    {
                        var OTNM = (int)OTEndTime.Subtract(OTStartTime).TotalMinutes - OTR.BreakTime;
                        var _OTR = ListOTPolic.Where(w => w.OTFrom <= OTNM && w.OTTo >= OTNM).ToList();
                        if (_OTR.Count() > 0)
                            OTClaim = ClsRounding.Rounding(_OTR.Sum(w => w.OTHour), 2, "N");
                        else
                        {
                            OTClaim = ClsRounding.Rounding((OTNM) / 60.00M, 2, "N");
                        }
                        if (OTClaim < 0) OTClaim = 0;
                        if (OTClaim > OTR.Hours) OTClaim = OTR.Hours.Value;
                        if (OTClaim != 0)
                        {
                            result.OTApproval += OTClaim;
                            result.WOT += OTClaim;
                            OTR.OTActual = OTClaim;
                            OTR.ReferenceNo = Convert.ToInt32(result.TranNo);
                        }
                    }
                }
            }

            return result;
        }
        public void ValidateMissScan(DateTime? CheckInOut, List<ClsMissScans> ListMissScan, string Type, string Remark
            , int Flag)
        {
            if (CheckInOut.HasValue && CheckInOut.Value.Year == 1900)
            {
                ListMissScan.Add(new ClsMissScans()
                {
                    MissScanType = "MissScan" + Type,
                    Remark = "MissScan" + Remark,
                    Flag = Flag,
                    IsUpdate = false
                });
                //MissScanIN = 1;
                //NumMissSCanIN = 1;
                //StrMissScanIN = "MissScanIN";
                //Remark = "MissScanIN" + Type;
            }
        }
        public ATEmpSchedule Check_NightShift(ATEmpSchedule result, ATPolicy NWPolicy)
        {
            result.NSWStart = null;
            result.NSWEnd = null;
            DateTime TempFromDate = new DateTime();
            DateTime TempToDate = new DateTime();
            decimal NWH = 0;
            DateTime NWFromDate = result.TranDate + NWPolicy.NWFROM.Value.TimeOfDay;
            DateTime NWToDate = result.TranDate + NWPolicy.NWTO.Value.TimeOfDay;
            NWToDate = NWToDate.AddDays(1);

            if (result.Flag1 == 1 && result.Start1.Value.Year != 1900 && result.End1.Value.Year != 1900)
            {
                if (result.Start1.Value > NWFromDate) TempFromDate = result.Start1.Value;
                else if (result.Start1.Value <= NWFromDate) TempFromDate = NWFromDate;

                if (result.End1.Value > NWToDate) TempToDate = NWToDate;
                else if (result.End1.Value <= NWToDate && result.End1.Value <= result.OUT1.Value) TempToDate = result.End1.Value;
                else if (result.End1.Value <= NWToDate && result.OUT1.Value <= result.End1.Value) TempToDate = result.OUT1.Value;
                NWH = Convert.ToDecimal(TempToDate.Subtract(TempFromDate).TotalHours);
                if (NWH < 0)
                {
                    NWH = 0;
                }
                else
                {
                    result.NSWStart = TempFromDate;
                    result.NSWEnd = TempToDate;
                }
            }
            if (result.Flag2 == 1 && result.Start2.Value.Year != 1900 && result.End2.Value.Year != 1900)
            {
                if (result.Start2.Value > NWFromDate) TempFromDate = NWToDate;
                else if (result.Start2.Value <= NWFromDate) TempFromDate = NWFromDate;

                if (result.End2.Value > NWToDate) TempToDate = result.End2.Value;
                else if (result.End2.Value <= NWToDate && result.End2.Value <= result.OUT2.Value) TempToDate = result.End2.Value;
                else if (result.End2.Value <= NWToDate && result.OUT2.Value <= result.End2.Value) TempToDate = result.OUT2.Value;
                NWH += Convert.ToDecimal(TempToDate.Subtract(TempFromDate).TotalHours);
                if (result.NSWStart == null)
                    result.NSWStart = TempFromDate;
                result.NSWEnd = TempToDate;
            }
            if (NWH <= 0) NWH = 0;
            else
            {

            }
            result.NWH = NWH;
            return result;
        }
        public string Insert_LateEarly(IUnitOfWork unitOfWork, ATEmpSchedule item, ClsMissScan _MissScan,List<ATLateEarlyDeduct> ListAddLaEa)
        {
            if ((item.Late1 + item.Late2) > 0)
            {
                var Amount = item.Late1.Value + item.Late2.Value;
                var EmpLateEalry = new ATLateEarlyDeduct();
                EmpLateEalry.IsLate_Early = 1;
                AddLateEarly(EmpLateEalry, "LATE", (int)Amount, Amount, "", "", item, _MissScan);
                ListAddLaEa.Add(EmpLateEalry);
            }
            if ((item.Early1.Value + item.Early2.Value) > 0)
            {
                var Amount = item.Early1.Value + item.Early2.Value;
                var EmpLateEalry = new ATLateEarlyDeduct();
                EmpLateEalry.IsLate_Early = 1;
                AddLateEarly(EmpLateEalry, "EARLY", (int)Amount, Amount, "", "", item, _MissScan);
                ListAddLaEa.Add(EmpLateEalry);
            }
            return SYConstant.OK;
        }
        public string Insert_MissScan(IUnitOfWork unitOfWork, ATEmpSchedule item, ClsMissScan _MissScan,List<ATLateEarlyDeduct > ListAddLaEa)
        {
            List<ClsMissScans> ListMissScan = new List<ClsMissScans>();
            if (item.Flag1 == 1 && item.LeaveDesc != "ABS")
            {
                ValidateMissScan(item.Start1, ListMissScan, "IN", "IN1", 1);
                ValidateMissScan(item.End1, ListMissScan, "Out", "Out1", 1);
            }
            if (item.Flag2 == 1 && item.LeaveDesc != "ABS")
            {
                ValidateMissScan(item.Start2, ListMissScan, "IN", "IN2", 2);
                ValidateMissScan(item.End2, ListMissScan, "Out", "Out2", 2);
            }
            foreach (var Scan in ListMissScan)
            {
                if (ListMissScan.Where(x => x.Flag == Scan.Flag && x.IsUpdate == false).Count() > 1)
                {
                    var EmpLateEalry = new ATLateEarlyDeduct();
                    AddLateEarly(EmpLateEalry, "ABS", 0.5M, 0.5M, "ABS" + Scan.Flag, Scan.MissScanType, item, _MissScan);
                    ListAddLaEa.Add(EmpLateEalry);
                    //ListEmpLateEarly.Add(EmpLateEalry);
                    ListMissScan.Where(x => x.Flag == Scan.Flag).ToList().ForEach(w => w.IsUpdate = true);
                }
                else
                {
                    if (Scan.IsUpdate == false)
                    {
                        Scan.IsUpdate = true;
                        var EmpLateEalry = new ATLateEarlyDeduct();
                        AddLateEarly(EmpLateEalry, "MissScan", 1, 1, Scan.Remark, Scan.MissScanType, item, _MissScan);
                        ListAddLaEa.Add(EmpLateEalry);
                        //ListEmpLateEarly.Add(EmpLateEalry);
                    }
                }
            }
            return SYConstant.OK;
        }
    }
    public class ClsMissScans
    {
        public int Flag { get; set; }
        public string MissScanType { get; set; }
        public string Remark { get; set; }
        public bool IsUpdate { get; set; }
    }
}
