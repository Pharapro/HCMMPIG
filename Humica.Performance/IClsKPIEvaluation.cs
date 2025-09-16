using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using System.Collections.Generic;

namespace Humica.Performance
{
    public interface IClsKPIEvaluation : IClsApplication
    {
        List<HRKPIAssignHeader> ListKPIPending { get; set; }
        List<HRKPIEvaluation> ListHeander { get; set; }
        HRKPIEvaluation Header { get; set; }
        List<HRKPIEvaItem> ListItem { get; set; }
        List<HRKPIIndicator> ListKPIIndicator { get; set; }
        string ScreenId { get; set; }
        FTFilerIndex Filter { get; set; }

        string ApproveDoc(string id);
        string Create(string Screen_ID);
        void OnCreatingLoading(string ID, string EmpCode);
        void OnDetailLoading(params object[] keys);
        void OnIndexLoading(bool IsESS = false);
        string Update(string ID, bool IS_ESS = false);
    }
}
