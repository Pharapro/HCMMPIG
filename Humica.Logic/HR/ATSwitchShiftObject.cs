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
    public class ATSwitchShiftObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public ATSwitchShift Header { get; set; }
        public string CompanyCode { get; set; }
        public string Plant { get; set; }
        public string MessageCode { get; set; }
        public List<ATSwitchShift> ListHeader { get; set; }

        public ATSwitchShiftObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        public string CreateSwitch()
        {
            try
            {
                var DB = new HumicaDBContext();

                if (Header == null)
                {
                    return "INVALIED";
                }
                if (string.IsNullOrEmpty(Header.FromEmpCode) || string.IsNullOrEmpty(Header.ToEmpCode))
                {
                    return "EMPLOYEE_SELECTION_REQUIRED";
                }
                if (string.IsNullOrEmpty(Header.Reason))
                {
                    return "REASON_BLANK";
                }
                var HOD = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == Header.FromEmpCode);
                var FromAtt = DB.ATEmpSchedules
                    .FirstOrDefault(w => w.TranDate == Header.FromDate.Date && w.EmpCode == Header.FromEmpCode);
                var FromAtt_ = DB.ATEmpSchedules
                    .FirstOrDefault(w => w.TranDate == Header.ToDate.Date && w.EmpCode == Header.FromEmpCode);
                var ToAtt = DB.ATEmpSchedules
                    .FirstOrDefault(w => w.TranDate == Header.ToDate.Date && w.EmpCode == Header.ToEmpCode);
                var ToAtt_ = DB.ATEmpSchedules
                    .FirstOrDefault(w => w.TranDate == Header.FromDate.Date && w.EmpCode == Header.ToEmpCode);

                if (FromAtt != null && ToAtt != null)
                {
                    var FromTranDate = Header.FromDate;
                    var ToTranDate = Header.ToDate;
                    var FromTranDate_ = FromAtt_.TranDate;
                    var ToTranDate_ = ToAtt_.TranDate;

                    FromAtt.EmpCode = Header.ToEmpCode;
                    ToAtt.EmpCode = Header.FromEmpCode;
                    FromAtt_.EmpCode = Header.ToEmpCode;
                    ToAtt_.EmpCode = Header.FromEmpCode;

                    DB.ATEmpSchedules.Attach(FromAtt);
                    DB.Entry(FromAtt).Property(w => w.EmpCode).IsModified = true;
                    DB.Entry(FromAtt).Property(w => w.TranDate).CurrentValue = FromTranDate;

                    DB.ATEmpSchedules.Attach(ToAtt);
                    DB.Entry(ToAtt).Property(w => w.EmpCode).IsModified = true;
                    DB.Entry(ToAtt).Property(w => w.TranDate).CurrentValue = ToTranDate;


                    if (Header.FromDate != Header.ToDate)
                    {
                        DB.ATEmpSchedules.Attach(FromAtt_);
                        DB.Entry(FromAtt_).Property(w => w.EmpCode).IsModified = true;
                        DB.Entry(FromAtt_).Property(w => w.TranDate).CurrentValue = FromTranDate_;

                        DB.ATEmpSchedules.Attach(ToAtt_);
                        DB.Entry(ToAtt_).Property(w => w.EmpCode).IsModified = true;
                        DB.Entry(ToAtt_).Property(w => w.TranDate).CurrentValue = ToTranDate_;
                    }

                    Header.CreatedBy = User.UserName;
                    Header.CreatedOn = DateTime.Now;

                    DB.ATSwitchShifts.Add(Header);

                    DB.SaveChanges();
                    #region Notifican on Mobile
                    if (HOD != null && !string.IsNullOrEmpty(HOD.FirstLine))
                    {
                        var access = DB.TokenResources.FirstOrDefault(w => w.UserName == HOD.FirstLine);
                        if (access != null)
                        {
                            if (!string.IsNullOrEmpty(access.FirebaseID))
                            {
                                
                                string Desc = "I would like tell you that check roster with " + Header.ToEmployeeName + " from " + Header.ToDate.ToString("yyyy.MMM.dd")+" to "+ Header.FromDate.ToString("yyyy.MMM.dd") + ". \n Reason: " + Header.Reason;
                                Notification.Notificationf Noti = new Notification.Notificationf();
                                var clientToken = new List<string>();
                                clientToken.Add(access.FirebaseID);
                                var dd = Noti.SendNotification(clientToken, "Switch Shift", Desc);
                            }
                        }
                    }
                    
                    #endregion
                    return SYConstant.OK;
                }
                else
                {
                    return "SHIFT_NOT_FOUND";
                }
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.FromEmpCode;
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
                log.DocurmentAction = Header.FromEmpCode;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditShift(int id)
        {
            try
            {
                var FromAtt = DB.ATEmpSchedules.FirstOrDefault(w => w.TranDate == Header.FromDate.Date && w.EmpCode == Header.FromEmpCode);
                var ToAtt = DB.ATEmpSchedules.FirstOrDefault(w => w.TranDate == Header.ToDate.Date && w.EmpCode == Header.ToEmpCode);
                if (Header.Reason == null)
                {
                    return "REASON_BLANK";
                }
                if (FromAtt != null && ToAtt != null)
                {
                    string FShift = FromAtt.EmpCode;
                    string ToShift = ToAtt.EmpCode;
                    Header.FromShiftCode = FromAtt.SHIFT;
                    Header.ToShiftCode = ToAtt.SHIFT;
                    FromAtt.EmpCode = ToShift;
                    ToAtt.EmpCode = FShift;

                    DB.ATEmpSchedules.Attach(FromAtt);
                    DB.Entry(FromAtt).Property(w => w.EmpCode).IsModified = true;
                    DB.ATEmpSchedules.Attach(ToAtt);
                    DB.Entry(ToAtt).Property(w => w.EmpCode).IsModified = true;
                }
                var objMatch = DB.ATSwitchShifts.FirstOrDefault(w => w.ID == id);
                if (objMatch == null)
                {
                    return "DOC_INV";
                }

                objMatch.FromEmpCode = Header.FromEmpCode;
                objMatch.FromEmployeeName = Header.FromEmployeeName;
                objMatch.ToEmployeeName = Header.ToEmployeeName;
                objMatch.ToEmpCode = Header.ToEmpCode;
                objMatch.Reason = Header.Reason;
                objMatch.FromDate = Header.FromDate;
                objMatch.ToDate = Header.ToDate;

                DB.ATSwitchShifts.Attach(objMatch);
                DB.Entry(objMatch).State = System.Data.Entity.EntityState.Modified;
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = User.UserName;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }

}