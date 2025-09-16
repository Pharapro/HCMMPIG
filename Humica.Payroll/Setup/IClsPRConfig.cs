using Humica.Core.DB;
using Humica.EF;
using System.Collections.Generic;
namespace Humica.Payroll
{
    public interface IClsPRConfig : IClsApplication
    {
        List<PRIncomeType> ListIncomeType { get; set; }
        List<PRSocialSecurity> ListSocialSecurity { get; set; }
        List<SYRoundingRule> ListRoundingRule { get; set; }
        List<PRFringeBenefitSetting> ListFringeBenefit { get; set; }

        Dictionary<string, dynamic> OnDataSelectorLoading();
        string OnGridModifyConfig(PRIncomeType MModel, string Action);
        string OnGridModifyFringeBenefit(PRFringeBenefitSetting MModel, string Action);
        string OnGridModifyRounding(SYRoundingRule MModel, string Action);
        string OnGridModifySoSe(PRSocialSecurity MModel, string Action);
        void OnIndexLoading();
        void OnIndexLoadingConfig();
        void OnIndexLoadingFringeBenefit();
        void OnIndexLoadingRounding();
        void OnIndexLoadingSoSe();
    }
}
