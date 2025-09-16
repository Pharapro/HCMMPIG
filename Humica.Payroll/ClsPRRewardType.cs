using Humica.Core;
using Humica.Core.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Payroll
{
    public class ClsPRRewardType
    {
        public List<PR_RewardsType> GetRewardsType(List<PR_RewardsType> LstRewardsType, string ReCode, string Payterm = "")
        {
            List<PR_RewardsType> items = new List<PR_RewardsType>(LstRewardsType);
            if (Payterm == TermCalculation.FIRST.ToString())
                items = items.Where(w => w.ReCode == ReCode && (!w.IsBIMonthly.IsNullOrZero() && w.BIPercentageAm > 0)).ToList();

            return items;
        }
    }
    //public class ClsHisEmpReward
    //{
    //    public int PeriodID { get; set; }
    //    public string CompanyCode { get; set; }
    //    public string EmpCode { get; set; }
    //    public DateTime FromDate { get; set; }
    //    public DateTime ToDate { get; set; }
    //    public string RewardType { get; set; }
    //    public string Code { get; set; }
    //    public string Description { get; set; }
    //    public decimal Amount { get; set; }
    //    public bool TaxAble { get; set; }
    //    public bool FTax { get; set; }
    //}
    
}
