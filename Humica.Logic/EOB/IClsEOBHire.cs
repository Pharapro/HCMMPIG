using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.EOB
{
    public interface IClsEOBHire : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        RCMApplicant Filter { get; set; }
        RCMHire Hire { get; set; }
        ClsEmail EmailObject { get; set; }
        List<RCMHire> ListHire { get; set; }
        List<ClsWaitingHire> ListWaiting { get; set; }
        List<RCMApplicant> ListApplicant { get; set; }
        List<ExDocApproval> ListApproval { get; set; }
        string MessageError { get; set; }
        string MessageCode { get; set; }
        void ProcessLoadData(string userName);
        string CreateHire();
        string saveHire(string ApplicantID);
        string Delete(string EmpCode);
        string Cancel(string RequestNo);
        string Request(string RequestNo);
        void SendEmail(string Upload, string Rerceiver);
        string Approved(string RequestNo);
        string Reject(string RequestNo);
        void SetAutoApproval(string screenId, string docType, string branch, string deptCode);
    }
}