using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.Logic.LM;
using Humica.EF;
using System.Collections.Generic;
using System.Security.Policy;

namespace Humica.Attendance
{
    public interface IClsATRequestMissScan : IClsApplication
    {
         SYUser User { get; set; }
         SYUserBusiness BS { get; set; }
         string ScreenId { get; set; }
         string MessageError { get; set; }
         string DocType { get; set; }
         ATEmpMissscan Header { get; set; }
         List<ATEmpMissscan> ListHeader { get; set; }
         List<ATEmpMissscan> ListHeaderPending { get; set; }
         FTINYear FInYear { get; set; }
         HR_STAFF_VIEW HeaderStaff { get; set; }
         List<ExDocApproval> ListApproval { get; set; }
         List<RequestMissScan> ListPending { get; set; }
        void OnIndexLoading(bool IsESS = false);
        string OnEditLoading(params object[] keys);
        void OnDetailLoading(params object[] keys);
        void ProcessMissScan(string userName);
        string Create(string URL);
        string ESSCreate(string URL);
        string HRCreate(string DocNo);
        string Update(string Doc, bool IsESS = false);
        string Delete(string DocumentNo);
        string Approve(string DocumentNo, string URL, string comment);
        string Reject(string DocumentNo, string Remark, bool ESS = false);
        string HRApprove(string DocumentNo);
        string HRReject(string DocumentNo);
        string Cancel(string DocumentNo);
    }
}
