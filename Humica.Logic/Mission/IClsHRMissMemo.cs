using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Models.Mission;
using System.Collections.Generic;

namespace Humica.Models.HR
{
    public interface IClsHRMissMemo : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        HRMissionPlan Header { get; set; }
        HRResgierVehicle HeaderVechicle { get; set; }
        string MessageCode { get; set; }
        List<HRMissionPlan> ListHeader { get; set; }
        List<HRMissionPlanMem> ListMember { get; set; }
        List<HRResgierVehicle> ListVehicle { get; set; }
        List<ExDocApproval> ListApproval { get; set; }
        List<ClsListPlanPending> ListPlan { get; set; }
        HR_STAFF_VIEW HeaderStaff { get; set; }
        string UpdateMemo(string id);
        void LoadData(string userName);
        void AddPendingToList(ExDocApproval item, List<HRMissionPlan> listobj);

        string approveTheDoc(string DocNo);
        string Reject(string DocNo);
        string CancelDoc(string ApprovalID);
        void SetAutoApproval(string DocType, string Branch, string Department, string SCREEN_ID);
    }
}
