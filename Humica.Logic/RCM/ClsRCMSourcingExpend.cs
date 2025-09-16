using Humica.Core.DB;
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

namespace Humica.Logic.RCM
{
    public class ClsRCMSourcingExpend : IClsRCMSourcingExpend
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }

        public string MessageError { get; set; }

        public SYUserBusiness BS { get; set; }

        public string SaleOrderNo { get; set; }

        public string ScreenId { get; set; }

        public string DocType { get; set; }

        public RCMSourcingExpend Header { get; set; }

        public List<RCMSourcingExpend> ListHeader { get; set; }

        public CFDocType DocTypeObject { get; set; }

        public decimal VATRate { get; set; }
        public string PLANT { get; set; }

        public string Token { get; set; }

        public string PenaltyNo { get; set; }

        public bool IsSave { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ClsRCMSourcingExpend()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }

        public string CreateEX()
        {
            OnLoad();
            try
            {
                if (string.IsNullOrEmpty(Header.ExpendType)) { return "ET"; }
                else
                if (string.IsNullOrEmpty(Header.Remark)) { return "RM"; }
                Header.CreatedDate = DateTime.Now.Date;
                Header.CreatedBy = User.UserName;
                unitOfWork.Add(Header);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.ExpendType, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.ExpendType, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.ExpendType, SYActionBehavior.ADD.ToString(), e, true);
            }
        }

        public string EditEX(int ID)
        {
            OnLoad();
            try
            {
                var obj = unitOfWork.Repository<RCMSourcingExpend>().Queryable().FirstOrDefault(w => w.ID == ID);
                if (obj == null)
                    return "INV_DOC";
                if (string.IsNullOrEmpty(Header.ExpendType)) return "INV_EXPENDTYPE";
                if (string.IsNullOrEmpty(Header.Remark)) return "INV_REMARK";

                obj.DocumentDate = DateTime.Now;
                obj.Amount = Header.Amount;
                obj.Remark = Header.Remark;
                obj.ExpendType = Header.ExpendType;
                obj.DocumentReference = Header.DocumentReference;
                obj.AttachFile = Header.AttachFile;
                obj.VacancyNumber = Header.VacancyNumber;
                obj.ChangeDate = DateTime.Now;
                obj.ChangedBy = User.UserName;
                unitOfWork.Update(obj);
                unitOfWork.Save();
                return "OK";
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
        }

        public string DeleteEX(int ID)
        {
            OnLoad();
            try
            {
                var objMatch = unitOfWork.Repository<RCMSourcingExpend>().Queryable().FirstOrDefault(w => w.ID == ID);
                if (objMatch == null)
                    return "INV_DOC";
                unitOfWork.Delete(objMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, ID.ToString(), SYActionBehavior.ADD.ToString(), e, true);
            }
        }
    }
}