using Humica.Core.DB;
using Humica.EF;
using System.Collections.Generic;
namespace Humica.Payroll
{
    public interface IClsPRRawardType : IClsApplication
    {
        List<PR_RewardsType> ListHeader { get; set; }
        string ScreenId { get; set; }
        PR_RewardsType Header { get; set; }

        string Create();
        string Delete(string Code);
        void OnCreatingLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataSelectorLoading();
        void OnDetailLoading(params object[] keys);
        void OnIndexLoading();
        string Update(string Code);
    }
}
