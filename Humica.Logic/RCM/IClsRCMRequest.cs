using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.RCM
{
    public interface IClsRCMRequest : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string MessageError { get; set; }
        string ScreenId { get; set; }
        string JDDescription { get; set; }
        RCMRecruitRequest Header { get; set; }
        ClsEmail EmailObject { get; set; }
        List<RCMRecruitRequest> ListHeader { get; set; }
        List<ExDocApproval> ListApproval { get; set; }
        List<ClsWaitingList> ListWaiting { get; set; }
        Filters Filters { get; set; }
        string Code { get; set; }
        string MessageCode { get; set; }
        ExCfWFApprover ExCfWFApprover { get; set; }

        void ProcessLoadData(string userName);
        void SetAutoApproval(string screenId, string docType, string branch, string deptCode);
        string createRRequest(string RequestNo);
        string UpdRRequest(string RequestNo);
        string deleteRRequest(string RequestNo);
        string Cancel(string RequestNo);
        string Request(string RequestNo);
        void SendEmail(string Upload, string Rerceiver);
        //string Approved(string RequestNo, string Upload);
        string Approved(string RequestNo);
        string Reject(string RequestNo);
    }
}