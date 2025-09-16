using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;
using static Humica.EForm.CLsESSEmpTermination;

namespace Humica.EForm
{
    public interface ICLsESSEmpTermination : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        FTINYear FInYear { get; set; }
        string MessageCode { get; set; }
        string MessageError { get; set; }
        HRStaffProfile Staff { get; set; }
        HREFEmpResign Header { get; set; }
        ExDocApproval DocApproval { get; set; }
        string DocType { get; set; }
        HR_STAFF_VIEW HeaderStaff { get; set; }
        HREmpCareer HeaderCareer { get; set; }
        List<HREmpCareer> ListCareer { get; set; }
        List<HR_STAFF_VIEW> ListStaff { get; set; }
        List<HREFEmpResign> listHeader { get; set; }
        List<ClsReuestTerminat> ListRequestPending { get; set; }
        List<ExDocApproval> ListApproval { get; set; }
        List<ExCfWFApprover> ListEXCFApproval { get; set; }
        void LoadData(string userName, string ResignType);
        string SaveResign(bool IsResign);
        void SetAutoApproval(string SCREEN_ID, string DocType, string branch, string DeptCode);
        string RequestForApprove(string Doc, string fileName, string URL, bool IsResign);
        string Cancel(string Doc, bool IsResign);
        string Reject(string Document, bool IsResign);
        string approveTheDoc(string Inc, string fileName, string URL, bool IsResign);
    }
}