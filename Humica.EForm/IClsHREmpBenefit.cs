using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;
using static Humica.EForm.ClsHREmpBenefit;

namespace Humica.EForm
{
    public interface IClsHREmpBenefit : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        HREFBenefit Header { get; set; }
        string DocType { get; set; }
        string MessageError { get; set; }
        List<HREFBenefit> ListHeader { get; set; }
        List<ClsRequestBenefit> ListRequestPending { get; set; }
        List<ExDocApproval> ListApproval { get; set; }
        HR_STAFF_VIEW HeaderStaff { get; set; }
        List<HR_STAFF_VIEW> ListStaffView { get; set; }
        void ProcessSupense(string userName);
        string CreateSuspen();
        string RequestForApprove(string Doc, string fileName, string URL);
        void SetAutoApproval(string SCREEN_ID, string DocType, string Branch, string DeptCode);
        string Edit(string Doc);
        string Delete(string Doc);
        string Reject(string Document);
        string Cancel(string OTRNo);
        string approveTheDoc(string Inc, string fileName, string URL);
    }
}