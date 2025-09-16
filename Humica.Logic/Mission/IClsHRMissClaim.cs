using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.Mission;
using Humica.Models.Mission;
using System.Collections.Generic;

namespace Humica.Models.HR
{
    public interface IClsHRMissClaim : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        string MessageCode { get; set; }
         HRMissionClaim Header { get; set; }
         List<HRMissionClaim> ListHeader { get; set; }
         List<HRMissionClaimItem> ListItem { get; set; }
        List<ExDocApproval> ListApproval { get; set; }
         HRMissionPlan HeaderPlan { get; set; }
        //List<ClsListClaimPending> ListPlans { get; set; }
        HR_STAFF_VIEW HeaderStaff { get; set; }
        string CreateMissClaim();
        string UpdateMClaim(string DocNo);
        string deleteMClaim(string DocNo);
        void SetAutoApproval(string DocType, string Branch, string Department, string SCREEN_ID);
          
        string requestToApprove(string DocNo);
        string approveTheDoc(string DocNo);
        string Reject(string DocNo);
        string CancelDoc(string ApprovalID);
      

    }
}
