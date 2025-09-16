using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;

namespace Humica.Attendance
{
    public class ClsAtEmpOT : IClsAtEmpOT
    {
        protected IUnitOfWork unitOfWork;
        public string ScreenId { get; set; }
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public List<VIEW_ATEmpSchedule> ListEmpOTNS { get; set; }
        public FTFilterAttendance Attendance { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsAtEmpOT()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public void OnLoadingFilter()
        {
            ListEmpOTNS = new List<VIEW_ATEmpSchedule>();
            var payPeriod = unitOfWork.Repository<ATPayperiod>().Queryable().FirstOrDefault(w => w.PeriodID == Attendance.PeriodID);
            if (payPeriod != null)
            {
                ListEmpOTNS = unitOfWork.Repository<VIEW_ATEmpSchedule>().Where(w =>
                w.TranDate >= payPeriod.FromDate && w.TranDate <= payPeriod.ToDate
                                        && w.NWH > 0).ToList();
                if (!string.IsNullOrEmpty(Attendance.Department))
                    ListEmpOTNS = ListEmpOTNS.Where(x => x.DEPT == Attendance.Department).ToList();
                if (!string.IsNullOrEmpty(Attendance.Locations))
                    ListEmpOTNS = ListEmpOTNS.Where(x => x.LOCT == Attendance.Locations).ToList();
                if (!string.IsNullOrEmpty(Attendance.Division))
                    ListEmpOTNS = ListEmpOTNS.Where(x => x.Division == Attendance.Division).ToList();
                if (!string.IsNullOrEmpty(Attendance.EmpCode))
                    ListEmpOTNS = ListEmpOTNS.Where(x => x.EmpCode == Attendance.EmpCode).ToList();
                ListEmpOTNS = ListEmpOTNS.OrderBy(w => w.EmpCode).ToList();
            }
        }
        public string TransferOT(string ID, List<VIEW_ATEmpSchedule> List)
        {
            if (ID.Trim() == "")
            {
                return "INV_DOC";
            }
            try
            {
                var Policy = unitOfWork.Repository<ATPolicy>().Queryable().FirstOrDefault();
                if (Policy == null || string.IsNullOrEmpty(Policy.ExtraNS))
                {
                    return "INVALID_OTCODE_NS";
                }
                string approve = SYDocumentStatus.APPROVED.ToString();
                var payperiord = unitOfWork.Repository<ATPayperiod>().Queryable().FirstOrDefault(w => w.PeriodID == Attendance.PeriodID);
                var ListEmpAtt = unitOfWork.Repository<ATEmpSchedule>().Queryable().Where(w =>
                DbFunctions.TruncateTime(w.TranDate) >= payperiord.FromDate &&
                DbFunctions.TruncateTime(w.TranDate) <= payperiord.ToDate && w.NWH > 0).ToList();

                string[] _TranNo = ID.Split(';');
                var OTRate = unitOfWork.Repository<PROTRate>().Queryable().Where(w => w.OTCode == Policy.ExtraNS).ToList();
                var ListEmpOT = unitOfWork.Repository<PREmpOverTime>().Queryable().Where(w => w.OTType == Policy.ExtraNS
                  && DbFunctions.TruncateTime(w.OTDate) >= payperiord.FromDate &&
                  DbFunctions.TruncateTime(w.OTDate) <= payperiord.ToDate).ToList();
                ListEmpOT = ListEmpOT.ToList();
                foreach (var _OT in ListEmpOT.Where(w => w.TranType == 1))
                {
                    unitOfWork.Delete(_OT);
                }
                foreach (var TranNo in _TranNo)
                {
                    long _ID = Convert.ToInt64(TranNo);
                    var item = ListEmpAtt.FirstOrDefault(w => w.TranNo == _ID);
                    if (item == null) continue;
                    decimal OT = item.NWH;
                    var OTH = new PREmpOverTime();
                    string OTCode = "";
                    if (item.NWH > 0)
                    {
                        ATOTSetting OTSetting = new ATOTSetting();
                        DateTime? StartOT = item.NSWStart;
                        DateTime? OTEnd = item.NSWEnd;
                        decimal Temp_OT = item.NWH;
                        OTH = new PREmpOverTime();
                        OTCode = Policy.ExtraNS;
                        if (!OTRate.Where(w => w.OTCode == OTCode).Any())
                        {
                            return "INVALID_OTCODE";
                        }
                        var _staff1 = List.Where(w => w.EmpCode == item.EmpCode).FirstOrDefault();
                        OTH.EmpCode = item.EmpCode;
                        OTH.EmpName = _staff1.AllName;
                        OTH.OTDate = item.TranDate;
                        OTH.PayMonth = item.TranDate.Date;
                        OTH.OTType = OTCode;
                        OTH.LCK = 0;
                        OTH.TranType = 1;
                        OTH.OTHour = Math.Round(OT, 2);
                        OTH.CreateBy = User.UserName;
                        OTH.CreateOn = DateTime.Now;
                        OTH.OTFromTime = StartOT;
                        OTH.OTToTime = OTEnd;
                        OTH.Reason = "Extra Night Shift";
                        if (OTRate.Where(w => w.OTCode == OTCode).Any())
                            OTH.OTDescription = OTRate.FirstOrDefault(w => w.OTCode == OTCode).OTType;
                        unitOfWork.Add(OTH);
                        unitOfWork.Update(item);
                    }
                }

                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Attendance.PeriodID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Attendance.PeriodID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Attendance.PeriodID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
        }
    }
}
