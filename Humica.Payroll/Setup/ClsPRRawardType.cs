using Humica.Core;
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

namespace Humica.Payroll
{
    public class ClsPRRawardType : IClsPRRawardType
    {
        protected IUnitOfWork unitOfWork;
        public string ScreenId { get; set; }
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PR_RewardsType Header { get; set; }
        public List<PR_RewardsType> ListHeader { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public ClsPRRawardType()
        {
            OnLoad();
        }
        public void OnIndexLoading()
        {
            ListHeader = unitOfWork.Set<PR_RewardsType>().ToList();
        }
        public void OnCreatingLoading(params object[] keys)
        {
            Header =new PR_RewardsType();
            Header.Amount = 0;
            Header.IsBIMonthly = false;
            Header.BIPercentageAm = 0;
            Header.ReCode = "ALLW";
        }
        public string Create()
        {
            OnLoad();
            try
            {
                if (Header.Code == null)
                {
                    return "CODE_EN";
                }
                Header.Code = Header.Code.ToUpper().Trim();
                var Count = unitOfWork.Set<PR_RewardsType>().ToList();
                if (Count.Where(w => w.Code == Header.Code && w.ReCode == Header.ReCode).ToList().Count() > 0)
                {
                    return "INVALID_CODE";
                }
                var listType = ClsFilterGeneral.OnLoadRewardType().ToList();
                Header.RewardType = listType.Where(w => w.Code == Header.ReCode).FirstOrDefault().Description;
                Header.CreateOn = DateTime.Now;
                Header.CreateBy = User.UserName;

                unitOfWork.Add(Header);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (DbUpdateException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Header.Code, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public virtual void OnDetailLoading(params object[] keys)
        {
            string Code = (string)keys[0];
            Header = unitOfWork.Set<PR_RewardsType>().FirstOrDefault(w => w.Code == Code);
        }
        public string Update(string Code)
        {
            OnLoad();
            try
            {
                var ObjMatch = unitOfWork.Set<PR_RewardsType>().FirstOrDefault(w => w.ReCode == Header.ReCode && w.Code == Code);
                if (ObjMatch == null)
                {
                    return "RawardType_NE";
                }
                ObjMatch.Description = Header.Description;
                ObjMatch.TaxType = Header.TaxType;
                ObjMatch.Tax = Header.Tax;
                ObjMatch.FTax = Header.FTax;
                ObjMatch.Amount = Header.Amount;
                ObjMatch.IncomeType = Header.IncomeType;
                ObjMatch.IsBIMonthly = Header.IsBIMonthly;
                ObjMatch.BIPercentageAm = Header.BIPercentageAm;
                ObjMatch.ChangedOn = DateTime.Now;
                ObjMatch.ChangedBy = User.UserName;

                unitOfWork.Update(ObjMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public string Delete(string Code)
        {
            OnLoad();
            try
            {
                Header = new PR_RewardsType();
                Header.Code = Code;
                var ObjMatch = unitOfWork.Set<PR_RewardsType>().FirstOrDefault(w => w.Code == Code);
                if (ObjMatch == null)
                {
                    return "RawardType_NE";
                }
                unitOfWork.Delete(ObjMatch);
                unitOfWork.Save();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
            catch (Exception e)
            {
                return ClsEventLog.Save_EventLog(ScreenId, User.UserName, Code, SYActionBehavior.ADD.ToString(), e, true);
            }
        }
        public Dictionary<string, dynamic> OnDataSelectorLoading()
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            keyValues.Add("SELECT_INCOME_TYPE", unitOfWork.Set<PRIncomeType>().ToList());
            keyValues.Add("RAWARD_SELECT", ClsFilterGeneral.OnLoadRewardType().ToList());
            keyValues.Add("TAX_TYPE_SELECT", ClsFilterGeneral.OnLoadTaxType().ToList());
            return keyValues;
        }
    }
}
