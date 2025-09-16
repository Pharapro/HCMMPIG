using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.EForm
{
    public interface IClsESSRequestProbation : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        FTINYear FInYear { get; set; }
        string DocType { get; set; }
        string MessageError { get; set; }
        HREFReqProbation Header { get; set; }
        HRStaffProfile Staff { get; set; }
        string CompanyCode { get; set; }
        string Plant { get; set; }
        string MessageCode { get; set; }
        List<HREFReqProbation> ListHeader { get; set; }
        List<ClsEmpProbation> ListPending { get; set; }
        List<ExDocApproval> ListApproval { get; set; }
        HR_STAFF_VIEW HeaderStaff { get; set; }
        void ProcessProbation(string userName);
        string CreateReqPro(string URL);
        string EditReqPro(string DocNo);
        string DeleteReqPro(string DocNo);
        void SetAutoApproval(string SCREEN_ID, string DocType, string branch, string DeptCode);
        HRStaffProfile getNextApprover(string id, string pro);
        string RequestForApprove(string DocNo, string fileName, string URL);
        string Cancel(string DocNo);
        string approveTheDoc(string DocNo, string fileName, string URL);
        string Reject(string DocNo);
    }
}