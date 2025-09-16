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
using System.Linq;

namespace Humica.Logic.LM
{
    public class ClsPubHoliDay : IClsPubHoliDay
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string MessageError { get; set; }
        public HRPubHoliday Header { get; set; }
        public List<HRPubHoliday> ListHeader { get; set; }
        public List<MDUploadTemplate> ListTemplate { get; set; }
        public FTFilterEmployee Filter { get; set; }

        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        public ClsPubHoliDay()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
        public string uploadPubHoliday()
        {
            OnLoad();
            unitOfWork.BeginTransaction();
            try
            {
                if (ListHeader.Count == 0)
                {
                    return "NO_DATA";
                }
                if (ListHeader.Any(h => string.IsNullOrEmpty(h.Branch) || h.PDate == null))
                {
                    return "INVALID_BRANCH_OR_PDATE";
                }
                var distinctHolidays = ListHeader.Select(h => new { h.Branch, PDate = h.PDate.Date }).Distinct().ToList();
                foreach (var holiday in distinctHolidays)
                {
                    var existingHoliday = unitOfWork.Repository<HRPubHoliday>()
                        .Queryable().Where(w => w.PDate.Year == holiday.PDate.Year && w.Branch == holiday.Branch).ToList();
                    if (existingHoliday.Any())
                        unitOfWork.BulkDelete(existingHoliday);
                }
                ListHeader.ToList().ForEach(h =>
                {
                    h.CreatedOn = DateTime.Now;
                    h.CreatedBy = User.UserName;
                });
                unitOfWork.BulkInsert(ListHeader);
                unitOfWork.Commit();

                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, "", SYActionBehavior.ADD.ToString(), e, true);
            }
        }
    }
}