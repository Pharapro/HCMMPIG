using Humica.Core;
using Humica.Core.DB;
using Humica.EF.MD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Logic.PR
{
    public class PRSRewardType
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public PR_RewardsType Header { get; set; }
        public string ScreenId { get; set; }
        public string CompanyCode { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<PR_RewardsType> ListHeader { get; set; }
        public PRSRewardType()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public List<PR_RewardsType> GetRewardsType(List<PR_RewardsType> LstRewardsType, string ReCode, string Payterm = "")
        {
            List<PR_RewardsType> items = new List<PR_RewardsType>(LstRewardsType);
            if (Payterm == TermCalculation.FIRST.ToString())
                items = items.Where(w => w.ReCode == ReCode && (!w.IsBIMonthly.IsNullOrZero() && w.BIPercentageAm > 0)).ToList();

            return items;
        }

    }
    public enum TermCalculation
    {
        FIRST,
        SECOND
    }
    public enum RewardTypeCode
    {
        ALLW,
        BON,
        DED
    }
}