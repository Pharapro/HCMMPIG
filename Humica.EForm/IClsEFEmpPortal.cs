using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using System.Collections.Generic;

namespace Humica.EForm
{
    public interface IClsEFEmpPortal : IClsApplication
    {
        List<EFEmpPortal> ListHeader { get; set; }
        EFEmpPortal Header { get; set; }
        List<EFEmpPortalItem> ListItem { get; set; }
        List<HRApprSelfAssessment> ListSelfAssItem { get; set; }
        string ScreenId { get; set; }
        List<EFEmpPortal> ListAssessmentPending { get; set; }
        List<HRApprSelfAssQCM> ListSelfAssQCM { get; set; }
        string MessageCode { get; set; }
        List<ClsStaff> ListEmpStaff { get; set; }
        FTFilterReport Filter { get; set; }

        string ApproveDoc(string id);
        string CancelDoc(string id);
        string Create();
        string CreateMulti(string EmpCode, EFEmpPortal Obj);
        string Delete(string id);
        ClsStaff GetStaffFilter(string id);
        void OnCreatingLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataSelectorTeam(params object[] keys);
        string OnEditLoading(params object[] keys);
        void OnIndexLoading(bool IsESS = false);
        void OnIndexLoadingPending();
        void OnIndexLoadingTeam();
        string RequestToApprove(string id);
        string Update(string id, bool IsESS = false);
    }
}
