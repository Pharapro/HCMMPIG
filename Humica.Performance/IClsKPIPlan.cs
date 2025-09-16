using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using System.Collections.Generic;

namespace Humica.Performance
{
    public interface IClsKPIPlan : IClsApplication
    {
        string ScreenId { get; set; }
        string MessageError { get; set; }
        HRKPIPlanHeader Header { get; set; }
        List<HRKPIPlanHeader> ListHeader { get; set; }
        List<HRKPIPlanItem> ListItem { get; set; }
        List<HRKPIPlanHeader> ListHeaderPending { get; set; }
        FTFilerIndex Filter { get; set; }

        void OnIndexLoading(bool ESS = false);
        void OnIndexLoadingPending();
        void OnCreatingLoading();
        string Create();
        string BeforSave(HRKPIPlanHeader PlanHeader);
        string Update(string id, bool IS_ESS = false);
        string Delete(string id);
        void OnDetailLoading(params object[] keys);
        HRKPIPlanHeader GetDataCopy(string id);
        string ReleaseTheDoc(string id, bool IS_ESS = false);
        string RequestRelease(string id);
        string OnGridModify(HRKPIPlanItem MModel, string Action);
        Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataList(params object[] keys);
        Dictionary<string, dynamic> OnDataSelectorKPI(params object[] keys);
    }
}
