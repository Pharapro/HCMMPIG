using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;
using static Humica.EForm.ClsHREmpSupense;

namespace Humica.EForm
{
    public interface IClsHREmpSupense : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        HREFLeave Header { get; set; }
        string DocType { get; set; }
        string MessageError { get; set; }
        List<HREFLeave> ListHeader { get; set; }
        List<ClsRequestSupense> ListRequestPending { get; set; }
        List<ExDocApproval> ListApproval { get; set; }
        HR_STAFF_VIEW HeaderStaff { get; set; }
        List<HR_STAFF_VIEW> ListStaffView { get; set; }
        void ProcessSupense(string userName, string Type);
        string CreateSuspen(string Type);
        string RequestForApprove(string Doc, string fileName, string URL, bool issuspen);
        void SetAutoApproval(string SCREEN_ID, string DocType, string Branch, string DeptCode);
        string Edit(string Doc);
        string Delete(string Doc);
        string Reject(string Document, bool issuspen);
        string Cancel(string OTRNo, string Type);
        string approveTheDoc(string Inc, string fileName, string URL, bool issuspen);
    }
}