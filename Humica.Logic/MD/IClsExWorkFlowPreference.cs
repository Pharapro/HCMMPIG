using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.MD
{
    public interface IClsExWorkFlowPreference : IClsApplication
    {
        List<CFWorkFlow> ListWF { get; set; }
        List<ExCfWFApprover> ListWFApprover { get; set; }
        List<ExCfWFSalaryApprover> ListSalaryApprover { get; set; }
        List<ExCfWorkFlowItem> ListWFItem { get; set; }
        List<ExCFWFDepartmentApprover> ListWFDepartmentApprover { get; set; }

        Dictionary<string, dynamic> OnDataSelectorLoading();
        string OnGridModifyByBranch(ExCfWFApprover MModel, string Action);
        string OnGridModifyByDepartment(ExCFWFDepartmentApprover MModel, string Action);
        string OnGridModifySalaryApprover(ExCfWFSalaryApprover MModel, string Action);
        string OnGridModifyWFItem(ExCfWorkFlowItem MModel, string Action);
        string OnGridModifyWorkFLow(CFWorkFlow MModel, string Action);
        void OnIndexByBranch();
        void OnIndexByDepartment();
        void OnIndexLoading();
        void OnIndexSalaryApprover();
        void OnIndexWFItem();
        void OnIndexWorkFLow();
    }
}
