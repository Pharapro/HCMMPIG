using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.Models.Mission;
using System;
using System.Collections.Generic;

namespace Humica.Models.HR
{
    public interface IClsHRMissPlan : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        HRMissionPlan Header { get; set; }
        List<HRMissionPlan> ListHeader { get; set; }
        List<HRMissionPlanMem> ListMember { get; set; }
        List<HRMissProvince> ListProvince { get; set; }
        List<ExDocApproval> ListApproval { get; set; }
        List<ClsHRMissPlanPending> ListPending { get; set; }
        HR_STAFF_VIEW HeaderStaff { get; set; }
        string MessageCode { get; set; }
        void LoadData(string userName);
        string CreateMissPlan();
        string UpdateMPlan(string DocNo);
        string deleteMPlan(string DocNo);
        string requestToApprove(string DocNo);
        string approveTheDoc(string DocNo);
        string Reject(string DocNo);
        string CancelDoc(string ApprovalID);
        void SetAutoApproval(string DocType, string Branch, string Department, string SCREEN_ID);
    }
}
