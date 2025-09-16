using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using System;
using System.Collections.Generic;
using static Humica.EForm.ClsRequestChangShift;

namespace Humica.EForm
{
    public interface IClsRequestChangShift : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        string MessageError { get; set; }
        FTINYear FInYear { get; set; }
        string DocType { get; set; }
        HREFReqChangShift Header { get; set; }
        List<HREFReqChangShift> listHeader { get; set; }
        List<ClsRequestChangeShift> ListRequestPending { get; set; }
        HR_STAFF_VIEW HeaderStaff { get; set; }
        HRStaffProfile Staff { get; set; }
        List<HR_STAFF_VIEW> ListStaffView { get; set; }
        List<ExDocApproval> ListApproval { get; set; }
        SYUser UserObject { get; set; }
        SYUserBusiness UserBusinessObject { get; set; }
        CFEmailAccount EmailAccount { get; set; }
        void ProcessChangeShift(string userName);
        string CreateReqChangShift();
        string RequestForApprove(string Inc, string fileName, string URL);
        void SetAutoApproval(string SCREEN_ID, string DocType, string Branch, string DeptCode);
        string Reject(string Document);
        string Cancel(string OTRNo);
        string approveTheDoc(string Inc, string fileName, string URL);
    }
}