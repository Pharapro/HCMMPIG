using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;

namespace Humica.Logic.LM
{
    public interface IClsReqLateEarly : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        string MessageError { get; set; }
        string DocType { get; set; }
        HRReqLateEarly Header { get; set; }
        List<HRReqLateEarly> ListHeader { get; set; }
        FTINYear FInYear { get; set; }
        HR_STAFF_VIEW HeaderStaff { get; set; }
        List<ExDocApproval> ListApproval { get; set; }
        List<ClsReuestLaEa> ListReqPending { get; set; }

        void LoadData(string userName);
        HRStaffProfile getNextApprover(string id, string DocType);
        string CreateReqLateEarly();
        string editReqLateEarly(string id);
        string deleteReqLateEarly(string ReqLaEaNo);
        string ESSRequestLaEa(string URL);
        string isValidApproval(ExDocApproval Approver, EnumActionGridLine Action);
        string ApproveOTReq(string ReqLaEaNo, string URL);
        //string requestToApprove(string ReqLaEaNo, string URL);
        string RejectOTReq(string ID);
        string CancelOTReq(string ReqLaEaNo);
        void SetAutoApproval_(string empCode, string Doctype, DateTime toDate);

    }
}