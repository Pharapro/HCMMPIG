using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Humica.Attendance
{
    public class ClsATPolicyObject : IClsATPolicyObject
    {
        protected IUnitOfWork unitOfWork;
        public string ScreenId { get; set; }
        public SYUser User { get; set; }
        public ATPolicy Header { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>(new HumicaDBContext());
        }
        public ClsATPolicyObject()
        {
            User = SYSession.getSessionUser();
            OnLoad();
        }
        public void OnIndexLoading()
        {
            Header = unitOfWork.Set<ATPolicy>().FirstOrDefault();
        }
        public string Update()
        {
            string Status_Error = "";
            try
            {
                var ObjMatch = unitOfWork.Set<ATPolicy>().FirstOrDefault();
                if (ObjMatch == null)
                {
                    return "DOC_INV";
                }
                ObjMatch.NWFROM = Header.NWFROM;
                ObjMatch.NWTO = Header.NWTO;
                ObjMatch.ExtraNS = Header.ExtraNS;
                ObjMatch.OTMN = Header.OTMN.HasValue ? Header.OTMN : 0;
                ObjMatch.ChangedBy = User.UserName;
                ObjMatch.ChangedOn = DateTime.Now;
                ObjMatch.Late = Header.Late.HasValue ? Header.Late : 0;
                ObjMatch.Early = Header.Early.HasValue ? Header.Early : 0;
                ObjMatch.IsLate_Early = Header.IsLate_Early.HasValue ? Header.IsLate_Early : 0;
                ObjMatch.MissScan = Header.MissScan.HasValue ? Header.MissScan : 0;
                ObjMatch.BaseOnScan = Header.BaseOnScan.HasValue ? Header.BaseOnScan : true;
                ObjMatch.AfterScan = Header.AfterScan.HasValue ? Header.AfterScan : 0;
                ObjMatch.LHourByRate = Header.LHourByRate;
                ObjMatch.LFromDate = Header.LFromDate;
                ObjMatch.LToDate = Header.LToDate;
                ObjMatch.MaxEarly = Header.MaxEarly;
                ObjMatch.MaxLate = Header.MaxLate;

                unitOfWork.Update(ObjMatch);
                unitOfWork.Save();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Status_Error, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Status_Error, SYActionBehavior.ADD.ToString(), e, true);
            }
        }

        public Dictionary<string, dynamic> OnDataSelector(params object[] keys)
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();

            keyValues.Add("OT_RATE_SELECT", unitOfWork.Repository<PROTRate>().Queryable().ToList());

            return keyValues;
        }
    }
}
