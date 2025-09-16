using Humica.Core.DB;
using Humica.EF;
using System.Collections.Generic;

namespace Humica.Performance
{
    public interface IClsAppraisalType : IClsApplication
    {
        HRApprType Header { get; set; }
        List<HRApprType> ListAppraisalType { get; set; }
        List<HRApprRegion> ListApprRegion { get; set; }
        List<HRApprFactor> ListApprFactor { get; set; }
        string ScreenId { get; set; }

        string Create();
        void OnCreatingLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataSelectorLoading();
        void OnDetailLoading(params object[] keys);
        string OnGridModifyFactor(HRApprFactor MModel, string Action);
        string OnGridModifyRegion(HRApprRegion MModel, string Action);
        void OnIndexLoading();
        string Update(string Code);
    }
}