using Humica.Core;
using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Humica.Logic.Atts
{
    public class SetupShiftObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public ATShift Header { get; set; }
        public List<ATShift> ListHeader { get; set; }
        public int TotalHour { get; set; }
        public int TotalHour2 { get; set; }

        public SetupShiftObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public string CreateShift()
        {
            try
            {
                if (Header.Code == null) return "TR_CODE_RE";
                var objmatch = DB.ATShifts.FirstOrDefault(w => w.Code == Header.Code);
                if (objmatch != null) return "TR_CODE_EXIST";
                Header.Code = Header.Code.ToUpper().Trim();
                Header.CreateOn = DateTime.Now.Date;
                Header.CreateBy = User.UserName;
                if (Header.CheckInBefore1.HasValue)
                    Header.CheckInBefore1 = Header.CheckIn1.Value.Date + Header.CheckInBefore1.Value.TimeOfDay;
                if (Header.CheckInAfter1.HasValue)
                    Header.CheckInAfter1 = Header.CheckIn1.Value.Date + Header.CheckInAfter1.Value.TimeOfDay;
                if (Header.CheckOutBefore1.HasValue)
                    Header.CheckOutBefore1 = Header.CheckOut1.Value.Date + Header.CheckOutBefore1.Value.TimeOfDay;
                if (Header.CheckOutAfter1.HasValue)
                    Header.CheckOutAfter1 = Header.CheckOut1.Value.Date + Header.CheckOutAfter1.Value.TimeOfDay;
                if (Header.CheckInBefore2.HasValue)
                    Header.CheckInBefore2 = Header.CheckIn2.Value.Date + Header.CheckInBefore2.Value.TimeOfDay;
                if (Header.CheckInAfter2.HasValue)
                    Header.CheckInAfter2 = Header.CheckIn2.Value.Date + Header.CheckInAfter2.Value.TimeOfDay;
                if (Header.CheckOutBefore2.HasValue)
                    Header.CheckOutBefore2 = Header.CheckOut2.Value.Date + Header.CheckOutBefore2.Value.TimeOfDay;
                if (Header.CheckOutAfter2.HasValue)
                    Header.CheckOutAfter2 = Header.CheckOut2.Value.Date + Header.CheckOutAfter2.Value.TimeOfDay;
                DB.ATShifts.Add(Header);

                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = Header.Code;
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
                log.DocurmentAction = Header.Code;
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
                log.DocurmentAction = Header.Code;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string EditShift(string id)
        {
            DB = new HumicaDBContext();
            try
            {
                var objMatch = DB.ATShifts.Find(id);
                if (objMatch == null)
                {
                    return "DOC_NE";
                }
                DateTime? breakStart = null;
                DateTime? breakEnd = null;
                DateTime DateNow = DateTime.Now;
                if (Header.BreakStart.HasValue)
                    breakStart = DateTimeHelper.DateInHourMin(Header.CheckIn1.Value.Date + Header.BreakStart.Value.TimeOfDay);
                if (Header.BreakEnd.HasValue)
                    breakEnd = DateTimeHelper.DateInHourMin(Header.CheckIn1.Value.Date + Header.BreakEnd.Value.TimeOfDay);
                if (Header.CheckInBefore1.HasValue)
                    Header.CheckInBefore1 = Header.CheckIn1.Value.Date + Header.CheckInBefore1.Value.TimeOfDay;
                if (Header.CheckInAfter1.HasValue)
                    Header.CheckInAfter1 = Header.CheckIn1.Value.Date + Header.CheckInAfter1.Value.TimeOfDay;
                if (Header.CheckOutBefore1.HasValue)
                    Header.CheckOutBefore1 = Header.CheckOut1.Value.Date + Header.CheckOutBefore1.Value.TimeOfDay;
                if (Header.CheckOutAfter1.HasValue)
                    Header.CheckOutAfter1 = Header.CheckOut1.Value.Date + Header.CheckOutAfter1.Value.TimeOfDay;
                if (Header.CheckIn2.HasValue)
                    Header.CheckIn2 = DateNow.Date + Header.CheckIn2.Value.TimeOfDay;
                if (Header.CheckOut2.HasValue)
                    Header.CheckOut2 = DateNow.Date + Header.CheckOut2.Value.TimeOfDay;
                if (Header.CheckInBefore2.HasValue)
                    Header.CheckInBefore2 = Header.CheckIn2.Value.Date + Header.CheckInBefore2.Time().TimeOfDay;
                if (Header.CheckInAfter2.HasValue)
                    Header.CheckInAfter2 = Header.CheckIn2.Value.Date + Header.CheckInAfter2.Time().TimeOfDay;
                if (Header.CheckOutBefore2.HasValue)
                    Header.CheckOutBefore2 = Header.CheckOut2.Value.Date + Header.CheckOutBefore2.Time().TimeOfDay;
                if (Header.CheckOutAfter2.HasValue)
                    Header.CheckOutAfter2 = Header.CheckOut2.Value.Date + Header.CheckOutAfter2.Time().TimeOfDay;
       
                Header.BreakStart = breakStart;
                Header.BreakEnd = breakEnd;
                Header.ChangedOn = DateTime.Now;
                Header.ChangedBy = User.UserName;
                Header.CreateBy = objMatch.CreateBy;
                Header.CreateOn = objMatch.CreateOn;
                HumicaDBContext DBM = new HumicaDBContext();
                DBM.Entry(Header).State = System.Data.Entity.EntityState.Modified;
                int row = DBM.SaveChanges();

               // ClsAuditLog.AuditLog(ScreenId, objMatch.Code, "Header", objMatch, Header);
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.Code;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string DeleteShift(string id)
        {
            try
            {
                var objEmp = DB.ATEmpSchedules.Where(w => w.SHIFT == id).ToList().Count();
                if (objEmp > 0)
                {
                    return "DATA_USE";
                }
                ATShift objCust = DB.ATShifts.Find(id);
                if (objCust == null)
                {
                    return "SHIFT_NE";
                }

                HumicaDBContext DBM = new HumicaDBContext();
                DBM.ATShifts.Attach(objCust);
                DBM.Entry(objCust).State = System.Data.Entity.EntityState.Deleted;
                int row = DBM.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserID.ToString();
                log.ScreenId = Header.Code;
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
                log.ScreenId = Header.Code;
                log.Action = Humica.EF.SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
    }
}