using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Performance
{
    public interface IClsKPIAssign : IClsApplication
    {
        string EmpID { get; set; }
        SYUser User { get; set; }
        string ScreenId { get; set; }
        string ErrorMessage { get; set; }
        HRKPIAssignHeader AssignHeader { get; set; }
        List<HRKPIAssignHeader> listAssignHeader { get; set; }
        List<HRKPIAssignItem> listAssignInsertItem { get; set; }
        List<HRKPIAssignHeader> ListPlanPending { get; set; }
        List<HREmpAppraisal> ListPending { get; set; }

        List<HRKPIAssignMember> ListKPIMember { get; set; }
        FTFilerIndex Filter { get; set; }

        List<HRKPIAssignHeader> OnIndexLoading(string AssignedBy, bool ESS = false);
        List<HRKPIAssignHeader> OnIndexLoadingPending(bool ESS = false);
        void OnIndexPending(bool ESS = false);

        void OnCreatingLoading(string ID);
        string OnCreateMultiLoading(string ID);
        string CreateMulti(string Screen_Id);
        string Create();
        void OnDetailLoading(params object[] keys);
        string OnEditLoading(params object[] keys);
        void OnDetailMultiLoading(params object[] keys);
        string Delete(string id);
        string Update(string id);
        string RequestRelease(string id);
        string ReleaseDoc(string id);
        string Accept(string id, bool ESS = false);
        string Approved(string ID);
        string OnGridModify(HRKPIAssignItem MModel, string Action);
        string OnGridModify(HRKPIAssignMember MModel, string Action);
        string Calculate(string id);
        Dictionary<string, dynamic> OnDataSelectorKPI(params object[] keys);
        Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataListByDept(params object[] keys);
        string CopyData(string id);
        Dictionary<string, dynamic> OnDataListLoading(string Department, int InYear, bool ESS = false);
        ClsStaff GetEmpAppraisal(string AppraisalNo);
    }
}
